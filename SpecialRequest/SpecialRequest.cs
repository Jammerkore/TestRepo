using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Globalization;
using System.Data;
using System.Threading;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.SpecialRequest
{
	class SpecialRequest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <remarks>
		/// First Arg = Method_RID
		/// Second Arg (optional) = Proccess_RID
		/// </remarks>
		[STAThread]
		static int Main(string[] args)
		{
			string sourceModule = "SpecialRequest.cs";
			string eventLogID = "SpecialRequest";
			SessionAddressBlock _sab;
			SessionSponsor _sponsor;
			IMessageCallback _messageCallback;
			DateTime _start = DateTime.Now;
			string _message = null;
			_messageCallback = new BatchMessageCallback();
			_sponsor = new SessionSponsor();
			_sab = new SessionAddressBlock(_messageCallback, _sponsor);
			System.Runtime.Remoting.Channels.IChannel _channel;
			int _taskListRid = Include.NoRID;
			int _taskSeq = 0;
			string _specialRequestName = null;
			ScheduleData _scheduleData;
			bool _errorFound = false;
			eMIDMessageLevel _highestMessage;
			ApplicationSessionTransaction _transaction;
			MethodBaseData _methodData;

			try
			{
				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}

				//==========================
				// Register callback channel
				//==========================
				try
				{
					_channel = _sab.OpenCallbackChannel();
				}
				catch (Exception e)
				{
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
					throw;
				}

				//===========================
				// edit for arguments
				//===========================
				if (args.Length > 0)
				{
					_specialRequestName = args[0].ToString();
				}
				else
				{
					EventLog.WriteEntry(eventLogID, "Missing Argument: Special Request Job Name", EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				//==================
				// Create Sessions
				//==================
				try
				{
					_sab.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Store | (int)eServerType.Hierarchy | (int)eServerType.Scheduler);
				}
				catch (Exception ex)
				{
					_errorFound = true;
					Exception innerE = ex;
					while (innerE.InnerException != null)
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}


				//=================================
				// get user rid from tasklist table
				//=================================
				_scheduleData = new ScheduleData();
				//DataRow taskListRow = _scheduleData.TaskList_Read(_taskListRid);
				int userRid = Include.AdministratorUserRID;
				string tasklistName = "Unknown";
				//if (taskListRow != null)
				//{
				//    userRid = Convert.ToInt32(taskListRow["USER_RID"], CultureInfo.CurrentUICulture);
				//    tasklistName = taskListRow["TASKLIST_NAME"].ToString();
				//}
				//else
				//{
				//    EventLog.WriteEntry(eventLogID, "Invalid tasklist RID:" + _taskListRid.ToString(), EventLogEntryType.Error);
				//    System.Console.Write("Invalid tasklist RID:" + _taskListRid.ToString());
				//    errorFound = true;
				//    return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				//}

				eSecurityAuthenticate authentication =
					_sab.ClientServerSession.UserLogin(userRid, eProcesses.specialRequest);

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                //BEGIN TT#1644 - MD- DOConnell - Process Control
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    _errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1644 - MD- DOConnell - Process Control
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

				if (authentication != eSecurityAuthenticate.ActiveUser)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user RID:" + userRid.ToString(), EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user RID:" + userRid.ToString());
					//errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				_sab.ClientServerSession.Initialize();
				_sab.ApplicationServerSession.Initialize();
				_sab.StoreServerSession.Initialize();
				_sab.HierarchyServerSession.Initialize();
				SchedulerConfigInfo schedConfigInfo = null;
				if (_sab.SchedulerServerSession != null)
				{
					_sab.SchedulerServerSession.Initialize();
					schedConfigInfo = _sab.SchedulerServerSession.SchedConfigInfo;
				}
				else
				{
					schedConfigInfo = new SchedulerConfigInfo();
					_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Scheduler Service not found.", sourceModule);
					EventLog.WriteEntry(eventLogID, "The Scheduler service is required to run Special Request jobs. Make sure the Scheduler Service is running." + _specialRequestName, EventLogEntryType.Error);
					System.Console.Write("Special Request Jobs require the Scheduler Service to be running.");
					//_errorFound = true;
					//return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				int aSpecReqRid = _scheduleData.SpecialRequest_GetKey(_specialRequestName);
				if (aSpecReqRid == Include.NoRID)
				{
					EventLog.WriteEntry(eventLogID, "Invalid Special Request Job Name: " + _specialRequestName, EventLogEntryType.Error);
					System.Console.Write("Invalid Special Request Job Name: " + _specialRequestName);
					_errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				SpecialRequestProcess srProcess = new SpecialRequestProcess(aSpecReqRid, schedConfigInfo, _sab);
				srProcess.Process();

			}
			catch (Exception err)
			{
				_message = err.Message;
				_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, sourceModule);
				_errorFound = true;
			}
			finally
			{
				if (_sab.ClientServerSession != null && _sab.ClientServerSession.Audit != null)
				{
					if (!_errorFound)
					{
						_sab.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _sab.GetHighestAuditMessageLevel());
					}
					else
					{
						_sab.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _sab.GetHighestAuditMessageLevel());
					}
					//_sab.ApplicationServerSession.Audit.SpecialRequestAuditInfo_Add(_totalJobs, _jobsProcessed, _jobsWithErrors, _successfulJobs);
				}
				_highestMessage = _sab.CloseSessions();
			}

			return Convert.ToInt32(_highestMessage, CultureInfo.CurrentUICulture);
		}

		
	}
	/// <summary>
	/// Processes a Special Request
	/// </summary>
	public class SpecialRequestProcess
	{
		SessionAddressBlock _sab;
		private int _specReqRid;
		SchedulerConfigInfo _schedConfigInfo;
		string _specialRequestName = null;
		ScheduleData _scheduleData;
		bool _errorFound = false;
		eMIDMessageLevel _highestMessage;
		ApplicationSessionTransaction _transaction;
		MethodBaseData _methodData;
		SpecialRequestProfile _specReqProf;

		int _totalJobs = 0;
		int _jobsProcessed = 0;
		int _jobsWithErrors = 0;
		int _successfulJobs = 0;
		List<int> _sucessfulJobList = new List<int>();

		public SpecialRequestProcess(int specReqRid, SchedulerConfigInfo schedConfigInfo, SessionAddressBlock sab)
		{
			_specReqRid = specReqRid;
			_schedConfigInfo = schedConfigInfo;
			_sab = sab;
		}

		public void Process()
		{
			try
			{
				_scheduleData = new ScheduleData();
				DataRow specReqRow = _scheduleData.SpecialRequest_Read(_specReqRid);
				if (specReqRow != null)
				{
					_specReqProf = new SpecialRequestProfile(specReqRow);
				}

				DataTable dtJobsInRequest = _scheduleData.SpecialRequest_ReadByJob(_specReqRid);
				_totalJobs = dtJobsInRequest.Rows.Count;

				//================================================================================
				// This pushes the jobs onto a stack in reverse order, so they'll at least begin
				// by processing the first jobs in the request first.
				//================================================================================
				Stack<JobProcessor> jobStack = new Stack<JobProcessor>();
				int maxJobs = dtJobsInRequest.Rows.Count;
				if (maxJobs > 0)
				{
					for (int i = maxJobs - 1; i >= 0; i--)
					{
						DataRow aJobRow = dtJobsInRequest.Rows[i];
						int jobRid = Convert.ToInt32(aJobRow["JOB_RID"]);
						string jobName = aJobRow["JOB_NAME"].ToString();
						JobProfile jobProf = new JobProfile(jobRid, jobName, false);

						JobProcessor jobProc = new JobProcessor(_schedConfigInfo, _sab.ApplicationServerSession.Audit, jobRid);
						jobStack.Push(jobProc);
					}
				}

				ProcessJobs(jobStack, _schedConfigInfo.CheckForTerminateInterval);

				//========================================================
				// Remove the successful jobs out of the Special Request
				//========================================================
				if (_sucessfulJobList.Count > 0)
				{
					try
					{
						_scheduleData.OpenUpdateConnection();
						foreach (int aJobRid in _sucessfulJobList)
						{
							_scheduleData.SpecialRequestJoin_DeleteByJob(_specReqRid, aJobRid);
						}
						_scheduleData.CommitData();
					}
					catch (Exception ex)
					{
						string msg = ex.ToString();
						throw;
					}
					finally
					{
						if (_scheduleData.ConnectionIsOpen)
							_scheduleData.CloseUpdateConnection();
					}
				}

			}
			catch (Exception ex)
			{
				string msg = ex.ToString();
				throw;
			}
			finally
			{
				if (_sab.ApplicationServerSession != null && _sab.ApplicationServerSession.Audit != null)
				{
					_sab.ApplicationServerSession.Audit.SpecialRequestAuditInfo_Add(_totalJobs, _jobsProcessed, _jobsWithErrors, _successfulJobs);
				}
			}
		}

		private eMIDMessageLevel ProcessJobs(Stack<JobProcessor> jobStack, int intervalWaitTime)
		{
			JobProcessor[] jobProcArray = null;
			JobProcessor[] jobProcStatusArray = null;  // Issue 5718 stodd 7/10/2008
			eMIDMessageLevel maxMessageLevel = eMIDMessageLevel.None;
			int i;

			try
			{
				jobProcArray = new JobProcessor[_specReqProf.ConcurrentJobs];
				// BEGIN Issue 5718 stodd 7/10/2008
				jobProcStatusArray = new JobProcessor[jobStack.Count];
				int statusCount = 0;
				// END Issue 5718 stodd 7/10/2008

				if (jobStack != null &&
					jobStack.Count > 0)
				{
					while (jobStack.Count > 0)
					{
						for (i = 0; i < _specReqProf.ConcurrentJobs && jobStack.Count > 0; i++)
						{
							if (jobProcArray[i] == null || !jobProcArray[i].isRunning)
							{
								jobProcArray[i] = (JobProcessor)jobStack.Pop();
								jobProcStatusArray[statusCount++] = jobProcArray[i]; // Issue 5718

								jobProcArray[i].ExecuteJobInThread();
								_jobsProcessed++;
							}
						}

						if (jobStack.Count > 0)
						{
							System.Threading.Thread.Sleep(intervalWaitTime);
						}
					}

					if (jobProcArray != null)
					{
						for (i = 0; i < _specReqProf.ConcurrentJobs; i++)
						{
							if (jobProcArray[i] != null)
							{
								jobProcArray[i].WaitForThreadExit();
								jobProcArray[i] = null;
							}
						}
					}
					// BEGIN Issue 5718 stodd 7/10/2008
					//========================
					// Set Job Status Counts
					//========================
					foreach (JobProcessor jobProc in jobProcStatusArray)
					{
						if (jobProc.CompletionStatus == eProcessCompletionStatus.Successful)
						{
							_successfulJobs++;
							_sucessfulJobList.Add(jobProc.JobRID);
						}
						else
						{
							_jobsWithErrors++;
						}
					}
					// END Issue 5718
				}
				else
				{
					maxMessageLevel = eMIDMessageLevel.NothingToDo;
				}

				return maxMessageLevel;
			}
			catch (ThreadAbortException)
			{
				if (jobProcArray != null)
				{
					foreach (JobProcessor jobProc in jobProcArray)
					{
						if (jobProc != null)
						{
							jobProc.AbortThread();
							jobProc.WaitForThreadExit();
						}
					}
				}

				//				_audit.Add_Msg(eMIDMessageLevel.Warning, "TaskList was cancelled by User", "ConcurrentProcess");

				return eMIDMessageLevel.Severe;
			}
			catch (Exception exc)
			{
				_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessJobs", "SpecialRequestProcess");
				_sab.ApplicationServerSession.Audit.Log_Exception(exc, GetType().Name);

				return eMIDMessageLevel.Severe;
			}
		}
	}
}
