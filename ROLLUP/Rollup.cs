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


namespace MIDRetail.Rollup
{
	/// <summary>
	/// Summary description for Rollup.
	/// </summary>
	class Rollup
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			string sourceModule = "Rollup.cs";
			string eventLogID = "MIDRollup";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			DateTime start = DateTime.Now;
			int taskListRID = 0;
			int taskSequence = 0;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			// Begin MID Track #5257 - JSmith - Rollup timeout
//			int batchSize = 250;
			int batchSize = 2000;
			// Begin MID Track #5257
			int concurrentProcesses = 5;
			bool includeZeroInAverage = true;
			bool honorLocks = false;
            //Begin Track #5454 - JSmith - zero parents with no children
            bool zeroParentsWithNoChildren = true;
            //End Track #5454
			string message = null;
			string fileLocation = null;
			bool errorFound = false;
			bool fromScheduler = false;
			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
			System.Runtime.Remoting.Channels.IChannel channel;
			eSecurityAuthenticate authentication = eSecurityAuthenticate.UnknownUser;
			MIDRetail.Business.Rollup rollup;
			ArrayList rollupVariables = new ArrayList();
			eMIDMessageLevel highestMessage;


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
				catch (Exception e)
				{
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
					throw;
				}

				// Create Sessions

				try
				{
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Hierarchy|(int)eServerType.Application);
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

				authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
					MIDConfigurationManager.AppSettings["Password"], eProcesses.rollup);

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

				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				// called from scheduler?
				if (args.Length > 0)
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						fromScheduler = true;
						taskListRID = Convert.ToInt32(args[1]);
						taskSequence = Convert.ToInt32(args[2]);
						_processId = Convert.ToInt32(args[3]);
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//Begin Track #4672 - JSmith - Allow input file a argument
						if (args.Length == 1)
						{
							fileLocation = args[0];
						}
						else
						{
//End Track #4672
							fromScheduler = true;
							if (args[0].Length > 0)
							{
								try
								{
									taskListRID = Convert.ToInt32(args[0]);
								}
								catch
								{
									errorFound = true;
									message = MIDText.GetText(eMIDTextCode.msg_InvalidArgument);
									message = message.Replace("{0}", args[0].ToString());
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//									SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
									EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
									System.Console.Write(message);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
								}
							}

							if (args[1].Length > 0)
							{
								try
								{
									taskSequence = Convert.ToInt32(args[1]);
								}
								catch
								{
									errorFound = true;
									message = MIDText.GetText(eMIDTextCode.msg_InvalidArgument);
									message = message.Replace("{0}", args[1].ToString());
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//									SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
									EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
									System.Console.Write(message);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
								}
							}
//Begin Track #4672 - JSmith - Allow input file a argument
							if (args.Length == 3 
								&& args[2].Length > 0)
							{
								fileLocation = args[2];
							}
						}
//End Track #4672
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
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
//Begin Track #4672 - JSmith - Allow input file a argument
//				fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
				if (fileLocation == null)
				{
					fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
				}
//Begin Track #4672

				string strParm = MIDConfigurationManager.AppSettings["BatchSize"];
				if (strParm != null)
				{
					try
					{
						batchSize = Convert.ToInt32(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["ConcurrentProcesses"];
				if (strParm != null)
				{
					try
					{
						concurrentProcesses = Convert.ToInt32(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["IncludeZeroInAverage"];
				if (strParm != null)
				{
					try
					{
						includeZeroInAverage = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["HonorLocks"];
				if (strParm != null)
				{
					try
					{
						honorLocks = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

                //Begin Track #5454 - JSmith - zero parents with no children
                strParm = MIDConfigurationManager.AppSettings["ZeroParentsWithNoChildren"];
				if (strParm != null)
				{
					try
					{
                        zeroParentsWithNoChildren = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}
                //End Track #5454

				if (!errorFound)
				{
					SAB.HierarchyServerSession.Initialize();
					SAB.ApplicationServerSession.Initialize();

					message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
					if (fileLocation == "" || fileLocation == null)
					{
						errorFound = true;

						message = message.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
						message += "Rollup Process NOT run";

						// Begin Track #5035 - JSmith - file not found message level inconsistent
//						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
						// End Track #5035

						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

						return Convert.ToInt32(SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
					}
					else
					{
                        // Begin TT1140 - JSmith - Received input file does not exist Rollup Process NOT run
                        if (!fromScheduler)
                        {
                            // End TT1140
                            if (!File.Exists(fileLocation))
                            {
                                errorFound = true;

                                message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
                                message += "Rollup Process NOT run";

                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);

                                SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

                                return Convert.ToInt32(SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                FileInfo txnFileInfo = new FileInfo(fileLocation);

                                if (txnFileInfo.Length == 0)
                                {
                                    errorFound = true;

                                    message = message.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
                                    message += "Rollup Process NOT run";

                                    // Begin Track #5035 - JSmith - file not found message level inconsistent
                                    //								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
                                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
                                    // End Track #5035

                                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

                                    return Convert.ToInt32(SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
                                }
                            }
                        // Begin TT1140 - JSmith - Received input file does not exist Rollup Process NOT run
                        }
                        else if (!File.Exists(fileLocation))
                        {
                            message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist.  Defaults will be used.");

                            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                            fileLocation = null;
                        }
                        // End TT1140
					}

                    // Begin TT1140 - JSmith - Received input file does not exist Rollup Process NOT run
                    //message = message.Replace("{0}", "[" + fileLocation + "]");

                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);
                    if (fileLocation != null)
                    {
                        message = message.Replace("{0}", "[" + fileLocation + "]");

                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);
                    }
                    // End TT1140

                    //Begin Track #5454 - JSmith - zero parents with no children
                    //rollup = new MIDRetail.Business.Rollup(SAB, batchSize, concurrentProcesses, includeZeroInAverage, honorLocks);
                    rollup = new MIDRetail.Business.Rollup(SAB, batchSize, concurrentProcesses, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren);
                    //End Track #5454
                    rollup.DetermineRollupVariables(SAB.ClientServerSession, fileLocation, 0);
					//Begin Track #4637 - JSmith - Split variables by type
//					if (rollup.DailyDatabaseVariables.Count == 0 &&
//						rollup.WeeklyDatabaseVariables.Count == 0)
					if (rollup.NoVariablesToRoll)
				//End Track #4637
					{
						errorFound = true;

						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_RollLevelsNotFound, sourceModule, true);

						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

						return Convert.ToInt32(SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
					}

					if (!fromScheduler)
					{
						// if there are not any overrides, all requests can be processed at the same time
						if (!rollup.AnyOverrides(SAB.ClientServerSession, fileLocation))
						{
						
							rollup.BuildRollupRequestsFromFile(fileLocation, SAB.ClientServerSession);

							if (rollup.SchedulePostingRollup)
							{
								rollup.ProcessPostingRollupRequests(SAB.ClientServerSession);
							}
							
							if (rollup.ScheduleReclassRollup)
							{
								rollup.ProcessReclassRollupRequests(SAB.ClientServerSession);
							}

							rollup.ProcessRollupRequests((int)eProcesses.rollup, SAB.ClientServerSession);
						}
						else
						{
                            rollup.ProcessEachRollupRequests(fileLocation, (int)eProcesses.rollup, SAB.ClientServerSession);
						}
						SAB.ClientServerSession.Audit.RollupAuditInfo_Add(rollup.TotalItems, batchSize, concurrentProcesses,
							rollup.TotalBatches, rollup.TotalErrors);
					}
					else
					{
						rollup.BuildRollupRequestsFromSchedule(taskListRID, taskSequence, SAB.ClientServerSession);

						if (rollup.SchedulePostingRollup)
						{
							rollup.ProcessPostingRollupRequests(SAB.ClientServerSession);
						}

						if (rollup.ScheduleReclassRollup)
						{
							rollup.ProcessReclassRollupRequests(SAB.ClientServerSession);
						}

						if (rollup.ScheduleRollup)
						{
                            rollup.ProcessRollupRequests((int)eProcesses.rollup, SAB.ClientServerSession);
						}
						SAB.ClientServerSession.Audit.RollupAuditInfo_Add(rollup.TotalItems, batchSize, concurrentProcesses,
							rollup.TotalBatches, rollup.TotalErrors);
					}
				}
// (CSMITH) - END MID Track #2979
			}

			catch ( Exception err )
			{
				errorFound = true;
				message = "";
				while(err != null)
				{
					message += " -- " + err.Message;
					err = err.InnerException;
				}
				if (SAB.ClientServerSession.Audit != null)
				{
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
				}
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
                        //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
