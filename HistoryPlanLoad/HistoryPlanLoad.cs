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

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;


namespace MIDRetail.HistoryPlanLoad
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class HistoryPlanLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			string sourceModule = "HistoryPlanLoad.cs";
			string eventLogID = "MIDHistoryPlanLoad";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			string fileLocation = null;
			char[] delimiter = {'~'};
			string message = null;
			int commitLimit = 1000;
			HistoryPlanLoadProcess _hplp;
			bool errorFound = false;
			bool serializeXMLFile = false;
			bool allowAutoAdds = true;
			bool rollStoreDailyToWeekly = false;
			bool rollStoreDailyUpHierarchy = false;
			bool rollStoreWeeklyUpHierarchy = false;
			bool rollStoreToChain = false;
			bool rollChainUpHierarchy = false;
			bool rollIntransit = false;
			bool rollStock = true;
			// Begin MID Track #2435 - only update posting date if provided
			DateTime postingDate = DateTime.MinValue;
			// End MID Track #2435
			eMIDMessageLevel highestMessage;
			eHierarchyLevelType transactionHint = eHierarchyLevelType.Undefined;
			//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
            // Begin TT#1652 - JSmith - Computation model not found
            //string computationModel = "Default";
            string computationModel = null;
            // End TT#1652
			//End - Abercrombie & Fitch #4411

			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
            //BinaryServerFormatterSinkProvider provider; 
            //Hashtable port;
			System.Runtime.Remoting.Channels.IChannel channel;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			
			try
			{
				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}
				//				audit = new MIDErrorLog(eProcesses.historyPlanLoad, 1);

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
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Hierarchy|(int)eServerType.Store|(int)eServerType.Application);
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

				eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"], MIDConfigurationManager.AppSettings["Password"], eProcesses.historyPlanLoad);

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
					//					return Convert.ToInt32(eReturnCode.fatal,CultureInfo.CurrentUICulture);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				if (args.Length > 0)
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						fileLocation = args[1];
						_processId = Convert.ToInt32(args[2]);
                        // Begin TT#1054 - JSmith - Relieve Intransit not working.
                        //delimiter = ConfigurationSettings.AppSettings["Delimiter"].ToCharArray();
                        delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                        // End TT#1054
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						if (args[0].Length > 0)
						{
							fileLocation = args[0];
						}
						else
						{
							fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
						}

						if (args.Length > 1)
						{
							try
							{
								postingDate = Convert.ToDateTime(args[1]);
							}
							catch ( Exception ex )
							{
								errorFound = true;
								message = ex.Message;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
								EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
								System.Console.Write(message);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
							}
							if (args.Length > 2)
							{
								delimiter = args[2].ToCharArray();
							}
							else
							{
								delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
							}
						}
						else
						{
							string strDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
							if (strDelimiter != null)
							{
								delimiter = strDelimiter.ToCharArray();
							}
						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
				else
				{
					fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
					string strDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
					if (strDelimiter != null)
					{
						delimiter = strDelimiter.ToCharArray();
					}
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
				bool useDefault = true;

				string strCommitLimit = MIDConfigurationManager.AppSettings["CommitLimit"];
				if (strCommitLimit != null)
				{
					if (strCommitLimit.ToUpper(CultureInfo.CurrentCulture) == "UNLIMITED")
					{
						commitLimit = int.MaxValue;
						useDefault = false;
					}
					else
					{
						try
						{
							commitLimit = Convert.ToInt32(strCommitLimit);
							useDefault = false;
						}
						catch
						{
							useDefault = true;
						}
					}
				}

				if (useDefault)
				{
					message = MIDText.GetText(eMIDTextCode.msg_BatchCommitLimitDefaulted);
					message = message.Replace("{0}", Include.DefaultCommitLimit.ToString());
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
					commitLimit = Include.DefaultCommitLimit;
				}

				useDefault = true;
				string strSerializeXMLFile = MIDConfigurationManager.AppSettings["SerializeXMLFile"];
				if (strSerializeXMLFile != null)
				{
					try
					{
						serializeXMLFile = Convert.ToBoolean(strSerializeXMLFile);
						useDefault = false;
					}
					catch
					{
						useDefault = true;
					}
				}
				if (useDefault)
				{
					message = MIDText.GetText(eMIDTextCode.msg_BatchSerializeXMLFileDefaulted);
					message = message.Replace("{0}", Include.DefaultSerializeXMLFile.ToString());
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
					serializeXMLFile = Include.DefaultSerializeXMLFile;
				}

				useDefault = true;
				string strAllowAutoAdds = MIDConfigurationManager.AppSettings["AllowAutoAdds"];
				if (strAllowAutoAdds != null)
				{
					try
					{
						allowAutoAdds = Convert.ToBoolean(strAllowAutoAdds);
						useDefault = false;
					}
					catch
					{
						useDefault = true;
					}
				}
				if (useDefault)
				{
					message = MIDText.GetText(eMIDTextCode.msg_BatchAllowAutoAddsDefaulted);
					message = message.Replace("{0}", Include.DefaultAllowAutoAdds.ToString());
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
					allowAutoAdds = Include.DefaultAllowAutoAdds;
				}

				useDefault = true;
				string strRollStock = MIDConfigurationManager.AppSettings["RollStockForward"];
				if (strRollStock != null)
				{
					try
					{
						rollStock = Convert.ToBoolean(strRollStock, CultureInfo.CurrentUICulture);
						useDefault = false;
					}
					catch
					{
						useDefault = true;
					}
				}
				else
				{
					errorFound = true;
					message = MIDText.GetText(eMIDTextCode.msg_InvalidStockTypeLoadCancelled);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
				}

				useDefault = true;
				string strRollupValues = MIDConfigurationManager.AppSettings["RollStoreDailyToWeekly"];
				if (strRollupValues != null)
				{
					try
					{
						rollStoreDailyToWeekly = Convert.ToBoolean(strRollupValues);
						useDefault = false;
					}
					catch
					{
						useDefault = true;
					}
				}
				if (useDefault)
				{
					message = MIDText.GetText(eMIDTextCode.msg_BatchRollupValuesDefaulted);
					message = message.Replace("{0}", Include.DefaultRollupValues.ToString());
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
					rollStoreDailyToWeekly = Include.DefaultRollupValues;
				}

				strRollupValues = MIDConfigurationManager.AppSettings["RollStoreDailyUpHierarchy"];
				if (strRollupValues != null)
				{
					try
					{
						rollStoreDailyUpHierarchy = Convert.ToBoolean(strRollupValues);
					}
					catch
					{
						rollStoreDailyUpHierarchy = false;
					}
				}

				strRollupValues = MIDConfigurationManager.AppSettings["RollStoreWeeklyUpHierarchy"];
				if (strRollupValues != null)
				{
					try
					{
						rollStoreWeeklyUpHierarchy = Convert.ToBoolean(strRollupValues);
					}
					catch
					{
						rollStoreWeeklyUpHierarchy = false;
					}
				}

				strRollupValues = MIDConfigurationManager.AppSettings["RollStoreToChain"];
				if (strRollupValues != null)
				{
					try
					{
						rollStoreToChain = Convert.ToBoolean(strRollupValues);
					}
					catch
					{
						rollStoreToChain = false;
					}
				}

				strRollupValues = MIDConfigurationManager.AppSettings["RollChainUpHierarchy"];
				if (strRollupValues != null)
				{
					try
					{
						rollChainUpHierarchy = Convert.ToBoolean(strRollupValues);
					}
					catch
					{
						rollChainUpHierarchy = false;
					}
				}

				strRollupValues = MIDConfigurationManager.AppSettings["RollIntransit"];
				if (strRollupValues != null)
				{
					try
					{
						rollIntransit = Convert.ToBoolean(strRollupValues);
					}
					catch
					{
						rollIntransit = false;
					}
				}

				string strValue = MIDConfigurationManager.AppSettings["TransactionHint"];
				if (strValue != null)
				{
					try
					{
						switch (strValue.ToUpper())
						{
							case "STYLEANDABOVE":
								transactionHint = eHierarchyLevelType.Style;
								break;
							case "COLOR":
								transactionHint = eHierarchyLevelType.Color;
								break;
							case "SIZE":
								transactionHint = eHierarchyLevelType.Size;
								break;
							case "ALL":
								transactionHint = eHierarchyLevelType.Undefined;
								break;
							default:
								transactionHint = eHierarchyLevelType.Undefined;
								break;
						}
					}
					catch
					{
					}
				}

				//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
				strValue = MIDConfigurationManager.AppSettings["ComputationModel"];
				if (strValue != null)
				{
					computationModel = strValue;
				}
				//End - Abercrombie & Fitch #4411

				if (!errorFound)
				{
					message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

					if (fileLocation == "" || fileLocation == null)
					{
						message = message.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
						message += "History / Plan Load Process NOT run";

						// Begin Track #5035 - JSmith - file not found message level inconsistent
//						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
						// End Track #5035

						try
						{
							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
						}
						catch
						{
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
						}
						highestMessage = eMIDMessageLevel.None;
						try
						{
							highestMessage = SAB.GetHighestAuditMessageLevel();
						}
						catch
						{
							highestMessage = eMIDMessageLevel.Severe;
						}

						return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
					}
					else
					{
						if (!File.Exists(fileLocation))
						{
							message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
							message += "History / Plan Load Process NOT run";

							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);

							try
							{
								SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
							}
							catch
							{
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
								SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
							}
							highestMessage = eMIDMessageLevel.None;
							try
							{
								highestMessage = SAB.GetHighestAuditMessageLevel();
							}
							catch
							{
								highestMessage = eMIDMessageLevel.Severe;
							}

							return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
						}
						else
						{
							FileInfo txnFileInfo = new FileInfo(fileLocation);

							if (txnFileInfo.Length == 0)
							{
								message = message.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
								message += "History / Plan Load Process NOT run";

								// Begin Track #5035 - JSmith - file not found message level inconsistent
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
								// End Track #5035

								try
								{
									SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
								}
								catch
								{
									SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
									SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
								}
								highestMessage = eMIDMessageLevel.None;
								try
								{
									highestMessage = SAB.GetHighestAuditMessageLevel();
								}
								catch
								{
									highestMessage = eMIDMessageLevel.Severe;
								}

								return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
							}
							else
							{
								message = message.Replace("{0}", "[" + fileLocation + "]");

								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);

                                //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
								SAB.StoreServerSession.Initialize();
                                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                                // StoreServerSession must be initialized before HierarchyServerSession 
                                SAB.HierarchyServerSession.Initialize();
                                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
								SAB.ApplicationServerSession.Initialize();
		
								_hplp = new HistoryPlanLoadProcess(SAB, commitLimit, ref errorFound, SAB.ApplicationServerSession.GlobalOptions.ProductLevelDelimiter,
																   allowAutoAdds, rollStoreDailyToWeekly, rollStoreDailyUpHierarchy, rollStoreWeeklyUpHierarchy, 
																	rollStoreToChain, rollChainUpHierarchy, 
								//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
//																	rollIntransit, rollStock);
																	rollIntransit, rollStock, computationModel);
								//End - Abercrombie & Fitch #4411

								if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
								{
									if (serializeXMLFile)
									{
										_hplp.SerializeVariableFile(fileLocation, postingDate, transactionHint, ref errorFound);
									}
									else
									{
										DateTime startTime = DateTime.Now;
										// lookup or autoadd nodes before processing transactions
										if (transactionHint == eHierarchyLevelType.Undefined || 
											transactionHint == eHierarchyLevelType.Color ||
											transactionHint == eHierarchyLevelType.Size)
										{
											_hplp.ProcessVariableFile(fileLocation, postingDate, true, ref errorFound);
//											SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Before look up nodes", sourceModule);
											_hplp.LookupNodes();
											_hplp.ClearWorkingVariables();
//											SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "After look up nodes", sourceModule);
                                            // Begin TT#916 - JSmith - Daily Size Records with style = 0
                                            _hplp.InitializeNodeHash();
                                            // End TT#916
										}
										// process transactions
										_hplp.ProcessVariableFile(fileLocation, postingDate, false, ref errorFound);
										TimeSpan duration = DateTime.Now.Subtract(startTime);
										string strDuration = Convert.ToString(duration,CultureInfo.CurrentUICulture);
//										SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Process time=" + strDuration, sourceModule);
									}
								}
								else
								{
									// lookup or autoadd nodes before processing transactions
									if (transactionHint == eHierarchyLevelType.Undefined || 
										transactionHint == eHierarchyLevelType.Color ||
										transactionHint == eHierarchyLevelType.Size)
									{
										_hplp.ProcessVariableFile(fileLocation, delimiter, postingDate, true, ref errorFound);
										_hplp.LookupNodes();
										_hplp.ClearWorkingVariables();
                                        // Begin TT#916 - JSmith - Daily Size Records with style = 0
                                        _hplp.InitializeNodeHash();
                                        // End  TT#916
									}
									// process transactions
									_hplp.ProcessVariableFile(fileLocation, delimiter, postingDate, false, ref errorFound);
								}
							}
						}
					}
				}
// (CSMITH) - END MID Track #2979
			}
			catch ( Exception ex )
			{
				errorFound = true;
				message = ex.Message;
				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
			}
			finally
			{ 
				if (!errorFound)
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						try
						{
							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
						}
						catch
						{
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
						}
					}
				}
				else
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						try
						{
							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
						}
						catch
						{
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
						}
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
