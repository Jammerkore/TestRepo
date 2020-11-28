using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Globalization;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.PlanForecasting
{
	/// <summary>
	/// Summary description for PlanForecasting.
	/// </summary>
	class PlanForecasting
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
			string sourceModule = "PlanForecasting.cs";
			string eventLogID = "PlanForecasting";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			DateTime start = DateTime.Now;
			string message = null;
			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
			System.Runtime.Remoting.Channels.IChannel channel;
			int _taskListRid = Include.NoRID;
			int _taskSeq = 0;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			ScheduleData _scheduleData;
			bool errorFound = false;
			eMIDMessageLevel highestMessage;
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
					channel = SAB.OpenCallbackChannel();
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
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						_taskListRid = Convert.ToInt32(args[1]);
						_taskSeq = Convert.ToInt32(args[2]);
						_processId = Convert.ToInt32(args[3]);
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						_taskListRid = Convert.ToInt32(args[0]);
						if (args.Length > 1)
						{
							_taskSeq = Convert.ToInt32(args[1]);
						}
						else
						{
							EventLog.WriteEntry(eventLogID, "Missing Argument: Task Sequence", EventLogEntryType.Error);
						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
				else
				{
					EventLog.WriteEntry(eventLogID, "Missing Argument: Task List RID", EventLogEntryType.Error);
				}

				//==================
				// Create Sessions
				//==================
				try
				{
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Application|(int)eServerType.Store|(int)eServerType.Hierarchy);
				}
				catch (Exception ex)
				{
					errorFound = true;
					Exception innerE = ex;
					while (innerE.InnerException != null) 
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}


				//=================================
				// get user rid from tasklist table
				//=================================
				_scheduleData = new ScheduleData();
				DataRow taskListRow = _scheduleData.TaskList_Read(_taskListRid);
				int userRid = Include.UndefinedUserRID;
				string tasklistName = "Unknown";
				if (taskListRow != null)
				{
					userRid = Convert.ToInt32(taskListRow["USER_RID"], CultureInfo.CurrentUICulture);
					tasklistName = taskListRow["TASKLIST_NAME"].ToString();
				}
				else
				{
					EventLog.WriteEntry(eventLogID, "Invalid tasklist RID:" + _taskListRid.ToString(), EventLogEntryType.Error);
					System.Console.Write("Invalid tasklist RID:" + _taskListRid.ToString());
					errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				eSecurityAuthenticate authentication = 
					SAB.ClientServerSession.UserLogin(userRid, eProcesses.forecasting);

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                //BEGIN TT#1644-VSuart-Process Control-MID
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1644-VSuart-Process Control-MID
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

				if (authentication != eSecurityAuthenticate.ActiveUser)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user RID:" + userRid.ToString(), EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user RID:" + userRid.ToString());
					errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}
				
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
				if (_processId != Include.NoRID)
				{
					SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					SAB.ClientServerSession.Initialize();
				}

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				SAB.ApplicationServerSession.Initialize();
				SAB.StoreServerSession.Initialize();
				SAB.HierarchyServerSession.Initialize();

				// Begin Track #5973 stodd
				MIDTimer taskTimer = new MIDTimer();
				taskTimer.Start();
				string msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListBegin);
				msg = msg.Replace("{0}", tasklistName);
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, sourceModule, true);
				// End Track #5973 stodd

				// Create a method data class so later we can determine what type of Method we have from
				// the RID. 
				_methodData = new MethodBaseData();

				//=========================================
				// get the forecast tasks for this request
				//=========================================
				DataTable dtTaskForecast = _scheduleData.TaskForecast_ReadByTaskList(_taskListRid, _taskSeq);
				int rowCount = dtTaskForecast.Rows.Count;
				int r=0;
				int forecastSeq = 0, nodeRid = 0, versionRid = 0;
				bool dateInRange = false;
				//================================================
				// gather up all the forecasts for this Task List
				//================================================
				for (r=0;r<rowCount;r++)
				{
					DataRow aRow = dtTaskForecast.Rows[r];
					forecastSeq = Convert.ToInt32(aRow["FORECAST_SEQUENCE"],CultureInfo.CurrentUICulture);
					if (aRow["HN_RID"] == DBNull.Value)
						nodeRid = Include.NoRID;
					else
						nodeRid = Convert.ToInt32(aRow["HN_RID"],CultureInfo.CurrentUICulture);
					if (aRow["FV_RID"] == DBNull.Value)
						versionRid = Include.NoRID;
					else
						versionRid = Convert.ToInt32(aRow["FV_RID"],CultureInfo.CurrentUICulture);
				
					DataTable dtTaskForecastDetail = _scheduleData.TaskForecastDetail_ReadByTaskList(_taskListRid, _taskSeq, forecastSeq);
					int DetailRowCount = dtTaskForecastDetail.Rows.Count;
					int r2=0;
					//=================================================================================
					// For each forecast detail, check to see if current date is within date range.
					// if it is, run the forecast.
					//=================================================================================
					// Begin Issue 4010 - stodd
					for (r2=0;r2<DetailRowCount;r2++)
					{
						DataRow aDetailRow = dtTaskForecastDetail.Rows[r2];
						if (aDetailRow["EXECUTE_CDR_RID"] == System.DBNull.Value)
							dateInRange = true;
						else
						{
							int cdrRid = Convert.ToInt32(aDetailRow["EXECUTE_CDR_RID"],CultureInfo.CurrentUICulture);
							dateInRange = SAB.ApplicationServerSession.Calendar.IsCurrentDateWithinRange(cdrRid);
						}

						//=============================
						//  LET'S DO SOME FORECASTING!
						//=============================
						if (dateInRange)
						{
							int methodRid = Include.NoRID;
							int workflowMethodInd = (int)eWorkflowMethodType.None;
							int workflowRid = Include.NoRID;
							int detailSeq = Convert.ToInt32(aDetailRow["DETAIL_SEQUENCE"],CultureInfo.CurrentUICulture);
							if (aDetailRow["METHOD_RID"] != DBNull.Value)
								methodRid = Convert.ToInt32(aDetailRow["METHOD_RID"],CultureInfo.CurrentUICulture); 
							if (aDetailRow["WORKFLOW_METHOD_IND"] != DBNull.Value)
								workflowMethodInd = Convert.ToInt32(aDetailRow["WORKFLOW_METHOD_IND"],CultureInfo.CurrentUICulture); 
							eWorkflowMethodType workflowMethodType = (eWorkflowMethodType)workflowMethodInd;
							if (aDetailRow["WORKFLOW_RID"] != DBNull.Value)
								workflowRid = Convert.ToInt32(aDetailRow["WORKFLOW_RID"],CultureInfo.CurrentUICulture); 

							_transaction = SAB.ApplicationServerSession.CreateTransaction();

							//================================================================================
							// this adds the node and version to the forecasting override list.
							// the override list is used during forecasting to override the node and version
							// specified on the method.
							//================================================================================
							_transaction.ForecastingOverride_ClearAll();
							_transaction.ForecastingOverride_Add(nodeRid, versionRid);

							//==================================================
							// Process either a workflow or the correct method
							//==================================================
							if (workflowMethodType == eWorkflowMethodType.Workflow)
							{
								_transaction.ProcessOTSPlanWorkflow(workflowRid, true, true, 1);
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, 
                                    "Forecast task: " + workflowMethodType.ToString(),
                                    sourceModule, true);   //TT#237 - Invalid Date - RBeck
							}
							else if (workflowMethodType == eWorkflowMethodType.Method)
							{
								eMethodType aMethodType = _methodData.GetMethodType(methodRid);

                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information,
                                    "Forecast task: " + aMethodType.ToString(), 
                                    sourceModule, true);   //TT#237 - Invalid Date - RBeck
								switch (aMethodType)
								{
									case eMethodType.OTSPlan:
										_transaction.ProcessMethod(eMethodType.OTSPlan, methodRid);
										break;
									case eMethodType.ForecastBalance:
										_transaction.ProcessMethod(eMethodType.ForecastBalance, methodRid);
										break;
									case eMethodType.CopyStoreForecast:
										_transaction.ProcessMethod(eMethodType.CopyStoreForecast, methodRid);
										break;
									case eMethodType.CopyChainForecast:
										_transaction.ProcessMethod(eMethodType.CopyChainForecast, methodRid);
										break;
                                    //Begin Enhancement - JScott - Export Method - Part 11
									case eMethodType.Export:
										_transaction.ProcessMethod(eMethodType.Export, methodRid);
										break;
                                    //End Enhancement - JScott - Export Method - Part 11
                                    //Begin Enhancement - KJohnson - Global Unlock
                                    case eMethodType.GlobalUnlock:
                                        _transaction.ProcessMethod(eMethodType.GlobalUnlock, methodRid);
                                        break;
                                    //End Enhancement - KJohnson - Global Unlock
                                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    case eMethodType.GlobalLock:
                                        _transaction.ProcessMethod(eMethodType.GlobalLock, methodRid);
                                        break;
                                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    // Begin Issue # 5595 kjohnson
                                    case eMethodType.Rollup:
                                        _transaction.ProcessMethod(eMethodType.Rollup, methodRid);
                                        break;
                                    // End Issue # 5595
									case eMethodType.ForecastSpread:
										_transaction.ProcessMethod(eMethodType.ForecastSpread, methodRid);
										break;
									case eMethodType.ForecastModifySales:
										_transaction.ProcessMethod(eMethodType.ForecastModifySales, methodRid);
										break;
									default:
										msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_InvalidMethod);
										msg = msg.Replace("{0}", tasklistName);
										msg = msg.Replace("{1}", detailSeq.ToString(CultureInfo.CurrentUICulture));
										EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Error);
										System.Console.Write(msg);
                                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error,
                                            eMIDTextCode.msg_InvalidMethod,
                                            msg, sourceModule, true);   //TT#237 - Invalid Date - RBeck
										errorFound = true;
										return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
								}
							}
							else
							{
								msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTasklistStep);
								msg = msg.Replace("{0}", tasklistName);
								msg = msg.Replace("{1}", detailSeq.ToString(CultureInfo.CurrentUICulture));
								EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Error);
								System.Console.Write(msg);
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error,
                                    eMIDTextCode.msg_InvalidTasklistStep,
                                    msg, sourceModule, true);       //TT#237 - Invalid Date - RBeck
								errorFound = true;
								return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
							}

							if (_transaction != null)
							{
								_transaction.Dispose();
							}
						}
                //Begin TT#237 - Invalid Date - RBeck
                        else
                        {
                            SAB.ClientServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Error,
                                eMIDTextCode.msg_al_CurrentDateOutsideRange,
                                MIDText.GetText(eMIDTextCode.msg_al_CurrentDateOutsideRange),
                                sourceModule, true);
                        }
                //End   TT#237 - Invalid Date - RBeck
					}
					// End Issue 4010 - stodd
				}
				// Begin Track #5973 stodd
				taskTimer.Stop();
				msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListEnd);
				msg = msg.Replace("{0}", tasklistName);
				msg = msg.Replace("{1}", taskTimer.ElaspedTimeString);
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, sourceModule, true);
				// End Track #5973 stodd\
			}
			catch ( Exception err )
			{
				message = err.Message;
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, sourceModule);
				errorFound = true;
			}
			finally
			{ 
				if (!errorFound)
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
				}
				else
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
