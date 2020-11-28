using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.ScheduleInterface
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class ExecuteJob
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			string eventLogID = "MIDExecuteJob";
//			Security security;
//			eSecurityAuthenticate secAuth;
			eProcessCompletionStatus retCode = eProcessCompletionStatus.Successful;
//			Audit audit = null;
			int jobRID;
			SchedulerConfigInfo schedConfigInfo;
			JobProcessor jobProc;
			string sourceModule = "ExecuteJob.cs";
			string message = null;
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;

			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
//			BinaryServerFormatterSinkProvider provider; 
//			Hashtable port;
			System.Runtime.Remoting.Channels.IChannel channel;

			//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
			bool returnMIDMessageLevel = false;

			//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
			try
			{
				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}

				// Register callback channel

				try
				{
					channel = SAB.OpenCallbackChannel();
				}
				catch (Exception ex)
				{
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + ex.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				// Create Sessions

				try
				{
					SAB.CreateSessions((int)eServerType.Client);
				}
				catch (Exception ex)
				{
					Exception innerE = ex;
					while (innerE.InnerException != null) 
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

                // Begin TT#240 MD - JSmith - Change Scheduler Interface to have its own audit entry and not report as Scheduler Service
                //eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"], MIDConfigurationManager.AppSettings["Password"], eProcesses.schedulerService);
                eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"], MIDConfigurationManager.AppSettings["Password"], eProcesses.scheduleInterface);
                // End TT#240 MD
				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				SAB.ClientServerSession.Initialize();

				//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
				string strReturnMIDMessageLevel = MIDConfigurationManager.AppSettings["ReturnMIDMessageLevel"];
				if (strReturnMIDMessageLevel != null)
				{
					try
					{
						returnMIDMessageLevel = Convert.ToBoolean(strReturnMIDMessageLevel);
					}
					catch
					{
						returnMIDMessageLevel = false;
					}
				}

				//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
				if (args.Length > 0)
				{
					ScheduleData sd = new ScheduleData();
//					jobRID = Convert.ToInt32(args[0]);
					jobRID = sd.Job_GetKey(args[0]);
					if (jobRID != Include.NoRID)
					{
						//Begin TT#893 - JScott - Task did not fail immediately when error level thrown
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Scheduler Interface called with Job Name: " + args[0].ToString(), sourceModule);

						//End TT#893 - JScott - Task did not fail immediately when error level thrown
						schedConfigInfo = new SchedulerConfigInfo();
						jobProc = new JobProcessor(schedConfigInfo, SAB.ClientServerSession.Audit, jobRID);
						jobProc.ExecuteJob();

						retCode = jobProc.CompletionStatus;
						//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
						//return Convert.ToInt32(retCode, CultureInfo.CurrentUICulture);
                        // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
                        //return GetReturnCode(returnMIDMessageLevel, retCode);
                        return GetReturnCode(returnMIDMessageLevel, retCode, jobProc.HighestTaskMsgLevel);
                        // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
						//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
					}
					else
					{
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Job Name:" + args[0].ToString() + " is not valid", sourceModule);

						retCode = eProcessCompletionStatus.Failed;
						//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
						//return Convert.ToInt32(retCode, CultureInfo.CurrentUICulture);
                        // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
                        //return GetReturnCode(returnMIDMessageLevel, retCode);
                        return GetReturnCode(returnMIDMessageLevel, retCode, eMIDMessageLevel.Error);
                        // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
						//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
					}
				}
				else
				{
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Missing Argument: Job Name", sourceModule);

					retCode = eProcessCompletionStatus.Failed;
					//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
					//return Convert.ToInt32(retCode, CultureInfo.CurrentUICulture);
                    // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
                    //return GetReturnCode(returnMIDMessageLevel, retCode);
                    return GetReturnCode(returnMIDMessageLevel, retCode, eMIDMessageLevel.Error);
                    // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
					//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
				}
			}

			catch ( Exception err )
			{
				message = "";
				while(err != null)
				{
					message += " -- " + err.Message;
					err = err.InnerException;
				}
				if (SAB.ClientServerSession.Audit != null)
				{
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					//Begin TT#893 - JScott - Task did not fail immediately when error level thrown
					//SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
					//End TT#893 - JScott - Task did not fail immediately when error level thrown
				}

				retCode = eProcessCompletionStatus.Failed;
				//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
				//return Convert.ToInt32(retCode, CultureInfo.CurrentUICulture);
                // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
                //return GetReturnCode(returnMIDMessageLevel, retCode);
                return GetReturnCode(returnMIDMessageLevel, retCode, eMIDMessageLevel.Error);
                // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
				//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
			}
			finally
			{
				//Begin TT#893 - JScott - Task did not fail immediately when error level thrown
				//if (retCode == eProcessCompletionStatus.Successful)
				//{
				//    if (SAB.ClientServerSession.Audit != null)
				//    {
				//        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
				//    }
				//}
				if (SAB.ClientServerSession != null &&
                    SAB.ClientServerSession.Audit != null)
				{
					if (retCode == eProcessCompletionStatus.Successful)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
					else
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
					}
				}
				//End TT#893 - JScott - Task did not fail immediately when error level thrown
			}
		}

        // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
		//Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes

        //static private int GetReturnCode(bool aReturnMIDMessageLevel, eProcessCompletionStatus aProcessCompletionsStatus)
        //{
        //    try
        //    {
        //        if (aReturnMIDMessageLevel)
        //        {
        //            return Convert.ToInt32(CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(aProcessCompletionsStatus), CultureInfo.CurrentUICulture);
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(aProcessCompletionsStatus, CultureInfo.CurrentUICulture);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
		//End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
        static private int GetReturnCode(bool aReturnMIDMessageLevel, eProcessCompletionStatus aProcessCompletionsStatus, eMIDMessageLevel aHighestMessageLevel)
        {
            try
            {
                if (aReturnMIDMessageLevel)
                {
                    return Convert.ToInt32(aHighestMessageLevel, CultureInfo.CurrentUICulture);
                }
                else
                {
                    return Convert.ToInt32(aProcessCompletionsStatus, CultureInfo.CurrentUICulture);
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
	}
}
