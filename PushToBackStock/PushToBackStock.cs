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
using System.Linq;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.PushToBackStock
{
	class PushToBackStock
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			MIDTimer jobTimer = new MIDTimer();
			try
			{
				jobTimer.Start();
				PushToBackStockWorker pushToBackStock = new PushToBackStockWorker();
				return pushToBackStock.Process(args);
			}
			finally
			{
				jobTimer.Stop("End of Job");
			}
		}

		public class PushToBackStockWorker
		{
			string sourceModule = "PushToBackStock.cs";
			string eventLogID = "PushToBackStock";
			SessionAddressBlock _SAB;
			SessionSponsor _sponsor;
			IMessageCallback _messageCallback;
			MIDLog _log = null;
			bool _LOGGING = false;
			ScheduleData _scheduleData;
			eMIDMessageLevel highestMessage;
			DataRow taskListRow;
			int userRid;
			string tasklistName;
			string msg;
			string message = null;
			bool errorFound = false;

			System.Runtime.Remoting.Channels.IChannel channel;
			int _processId = Include.NoRID;

			private string _headerCharForBackStockDate;
			private string _headerCharForForceToBackStock;
			private string _forceToBackStockValue;
			private string _headerId;

			private int _headersRead = 0;
			private int _headersWithErrors = 0;
			private int _headersProcessed = 0;
			private int _headersSkipped = 0;

			private ApplicationSessionTransaction _transaction;
			private string _forcedFlag = string.Empty;
			private DayProfile _headerBackStockDate = null;
			private int _pushToBackStockNumWeeks = 0;
			private DayProfile _currentSellingDay = null;
			private DayProfile _pushToBackStockDay = null;

			public int Process(string[] args)
			{

				try
				{
					_messageCallback = new BatchMessageCallback();
					_sponsor = new SessionSponsor();
					_SAB = new SessionAddressBlock(_messageCallback, _sponsor);

					if (!EventLog.SourceExists(eventLogID))
					{
						EventLog.CreateEventSource(eventLogID, null);
					}

					// Register callback channel

					try
					{
						channel = _SAB.OpenCallbackChannel();
					}
					catch (Exception exception)
					{
						errorFound = true;
						EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + exception.Message, EventLogEntryType.Error);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					//=======================
					// Create Sessions
					//=======================

					try
					{
						_SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Application);
					}
					catch (Exception exception)
					{
						errorFound = true;
						Exception innerE = exception;
						while (innerE.InnerException != null)
						{
							innerE = innerE.InnerException;
						}
						EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					_scheduleData = new ScheduleData();

					userRid = Include.UndefinedUserRID;

					eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
					MIDConfigurationManager.AppSettings["Password"], eProcesses.pushToBackStock);

                    //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                    //BEGIN TT#1644 - MD- DOConnell - Process Control
                    if (authentication == eSecurityAuthenticate.Unavailable)
                    {
                        //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                        errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        return Convert.ToInt32(eMIDMessageLevel.Severe);
                    }
                    //END TT#1644 - MD- DOConnell - Process Control
                    //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

					if (authentication != eSecurityAuthenticate.UserAuthenticated)
					{
						errorFound = true;
						EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
						System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
						//					return Convert.ToInt32(eReturnCode.fatal,CultureInfo.CurrentUICulture);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					if (_processId != Include.NoRID)
					{
						_SAB.ClientServerSession.Initialize(_processId);
					}
					else
					{
						_SAB.ClientServerSession.Initialize();
					}

                    //_SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
					_SAB.ApplicationServerSession.Initialize();

					_SAB.StoreServerSession.Initialize();
                    // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                    // StoreServerSession must be initialized before HierarchyServerSession 
                    _SAB.HierarchyServerSession.Initialize();
                    // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.


					//=================
					// PROCESSING
					//=================
					if (!errorFound)
					{
						errorFound = ProcessPushToBackStock();
					}
				}

				catch (Exception exception)
				{
					errorFound = true;
					message = "";
					while (exception != null)
					{
						message += " -- " + exception.Message;
						exception = exception.InnerException;
					}
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				}
				finally
				{
					if (!errorFound)
					{
						if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
						{
							_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
						}
					}
					else
					{
						if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
						{
							_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
						}
					}

					highestMessage = _SAB.CloseSessions();

					if (_LOGGING)
					{
						_log.CloseLogFile();
					}
				}

				return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
			}

			

			/// <summary>
			/// Main processing method for the Push to Back-stock API
			/// </summary>
			/// <returns></returns>
			public bool ProcessPushToBackStock()
			{
				MIDTimer processTimer = new MIDTimer();
			
				bool errorFound = false;

				try
				{
					processTimer.Start();

					//==============================================
					// Get Header Characteristics for comparison
					//==============================================
					if (ValidateCharacteristicNames())
					{
						Header headerData = new Header();
						DataTable dtReservationHeaders = headerData.GetReservationHeaders();

						foreach (DataRow hRow in dtReservationHeaders.Rows)
						{
							MIDTimer headerTimer = new MIDTimer();
							headerTimer.Start();
							_headerId = hRow["HDR_ID"].ToString();
							InitHeaderWorkFields();
							_headersRead++;
							bool pushTopBackStock = false;
							bool released = false;
							bool releasedApproved = false;
							try
							{
								released = Include.ConvertStringToBool(hRow["Released"].ToString());
							}
							catch
							{
							}
							try
							{
								releasedApproved = Include.ConvertStringToBool(hRow["ReleaseApproved"].ToString());
							}
							catch
							{
							}
							if (released || releasedApproved)
							{
								string eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderAlreadyReleased);
								eMsg = eMsg.Replace("{0}", _headerId);
								_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_HeaderAlreadyReleased, eMsg, this.ToString(), true);
								_headersSkipped++;
								continue;
							}

							int headerRid = int.Parse(hRow["HDR_RID"].ToString());
							// Get Allocation Profile
							_transaction = (ApplicationSessionTransaction)_SAB.ApplicationServerSession.CreateTransaction();
							AllocationProfile ap = new AllocationProfile(_transaction, _headerId, headerRid, _SAB.ApplicationServerSession);
							if (!ap.ReceivedInBalance)
							{
								string eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotInReceivedInBalance);
								eMsg = eMsg.Replace("{0}", _headerId);
								_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_HeaderNotInReceivedInBalance, eMsg, this.ToString(), true);
								_headersSkipped++;
								continue;
							}
							_transaction.AddAllocationProfile(ap);

							DataTable dtHdrChars = headerData.GetCharacteristics(headerRid, false);
							foreach (DataRow cRow in dtHdrChars.Rows)
							{
								string hdrCharId = cRow["hcg_id"].ToString();
								//======================================================
								// Check and process Force to Back Stock indicator
								//======================================================
								if (hdrCharId.ToUpper() == _headerCharForForceToBackStock.ToUpper())
								{
									_forcedFlag = cRow["text_value"].ToString();
									if (_forcedFlag.Trim() == _forceToBackStockValue)
									{
										pushTopBackStock = true;
										break;
									}
								}
								//======================================================
								// Check and process Back Stock date for comparison
								//======================================================
								if (!pushTopBackStock)
								{
									if (hdrCharId.ToUpper() == _headerCharForBackStockDate.ToUpper())
									{
										string backStockDate = cRow["date_value"].ToString();
										_headerBackStockDate = GetDayProfile(backStockDate);
										if (_headerBackStockDate != null)
										{
											HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(ap.PlanLevelStartHnRID);
                                            // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                            //ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_SAB.StoreServerSession.GetActiveStoresList();
                                            ProfileList storeList = _SAB.StoreServerSession.GetActiveStoresList();
                                            // End TT#1902-MD - JSmith - Store Services - VSW API Error
											IMOProfileList IMOProfList = _SAB.HierarchyServerSession.GetNodeIMOList(storeList, ap.PlanLevelStartHnRID);
											string imoId = hRow["IMO_ID"].ToString();

											for (int s = 0; s < storeList.Count; s++)
											{
												StoreProfile sp = (StoreProfile)storeList[s];
												if (sp.IMO_ID == null || sp.IMO_ID.Trim() == string.Empty)
												{
													// No IMO_ID for store
												}
												else
												{
													IMOProfile IMOProf = null;
													foreach (IMOProfile aIMOProf in IMOProfList.ArrayList)
													{
														if (sp.Key == aIMOProf.Key)
														{
															IMOProf = aIMOProf;
															break;
														}
													}

													if (sp.IMO_ID.Trim().ToUpper() == imoId.Trim().ToUpper())
													{
														if (IMOProf.IMOPshToBackStock > -1)
														{
															_pushToBackStockNumWeeks = IMOProf.IMOPshToBackStock;
															int numDays = _pushToBackStockNumWeeks * 7;
															_pushToBackStockDay = _SAB.ClientServerSession.Calendar.Add(_headerBackStockDate, numDays);
															_currentSellingDay = _SAB.ClientServerSession.Calendar.PostDate;
															if (_currentSellingDay >= _pushToBackStockDay)
															{
																pushTopBackStock = true;
																break;
															}
														}
													}
												}
											}
										}
									}
								}
							}

							// BEGIN TT#1401 - stodd - _currentSellingDate was sometimes null
							_currentSellingDay = _SAB.ClientServerSession.Calendar.PostDate;
							// END TT#1401

							if (pushTopBackStock)
							{
								// BEGIN TT#2769 - stodd -  Push to Backstock enqueue error 
								string enqueueMsg = string.Empty;
								List<int> enqHdrList = new List<int>();
								enqHdrList.Add(headerRid);
								if (_transaction.EnqueueHeaders(enqHdrList, out enqueueMsg))
								{
									ap.ReReadHeader();

									PushToBackStock(headerRid, _headerId);

									_transaction.DequeueHeaders(enqHdrList);

									string eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_PushToBackStockProcessed);
									eMsg = FormatAuditMessage(eMsg);
									_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PushToBackStockProcessed, eMsg, this.ToString(), true);
									_headersProcessed++;
								}
								else
								{
									_SAB.ApplicationServerSession.Audit.Add_Msg(
										eMIDMessageLevel.Error,
										" Unable to enqueue Headers linked to header [" + _headerId + "]: " + enqueueMsg,
										this.GetType().Name);
								}
								// END TT#2769 - stodd -  Push to Backstock enqueue error 
							}
							else
							{
								string eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_PushToBackStockSkipped);
								eMsg = FormatAuditMessage(eMsg);
								_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PushToBackStockSkipped, eMsg, this.ToString(), true);
								_headersSkipped++;
							}
							headerTimer.Stop("End of Processing Header: " + _headerId);
						}
					}
					processTimer.Stop();
				}
				catch (Exception ex)
				{
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
					throw;
				}
				catch
				{
					throw;
				}
				finally
				{
					// BEGIN TT#2769 - stodd -  Push to Backstock enqueue error
					_transaction.DequeueHeaders();
					// END TT#2769 - stodd -  Push to Backstock enqueue error
					_SAB.ApplicationServerSession.Audit.PushToBackStockAuditInfo_Add(_headersRead, _headersWithErrors, _headersProcessed, _headersSkipped);
				}
				return errorFound;
			}

			private string FormatAuditMessage(string eMsg)
			{
				eMsg = eMsg.Replace("{0}", _headerId);
				eMsg = eMsg.Replace("{1}", _forcedFlag);
				if (_headerBackStockDate == null)
				{
					eMsg = eMsg.Replace("{2}", " ");
				}
				else
				{
					eMsg = eMsg.Replace("{2}", _headerBackStockDate.Date.ToShortDateString());
				}
				eMsg = eMsg.Replace("{3}", _pushToBackStockNumWeeks.ToString());
				if (_pushToBackStockDay == null)
				{
					eMsg = eMsg.Replace("{4}", " ");
				}
				else
				{
					eMsg = eMsg.Replace("{4}", _pushToBackStockDay.Date.ToShortDateString());
				}
				if (_currentSellingDay == null)
				{
					eMsg = eMsg.Replace("{5}", " ");
				}
				else
				{
					eMsg = eMsg.Replace("{5}", _currentSellingDay.Date.ToShortDateString());
				}
				return eMsg;
			}


			private void InitHeaderWorkFields()
			{
				_forcedFlag = string.Empty;
				_headerBackStockDate = null;
				_pushToBackStockNumWeeks = 0;
				_currentSellingDay = null;
				_pushToBackStockDay = null;

			}

			private bool ValidateCharacteristicNames()
			{
				bool isValid = true;
				_headerCharForBackStockDate = MIDConfigurationManager.AppSettings["DateComparisonCharacteristic"];
				_headerCharForForceToBackStock = MIDConfigurationManager.AppSettings["ForceToBackStockCharacteristic"];
				_forceToBackStockValue = MIDConfigurationManager.AppSettings["ForceToBackStockValue"];
				_forceToBackStockValue = _forceToBackStockValue.Trim();
				HeaderCharacteristicsData hdrCharData = new HeaderCharacteristicsData();
				string eMsg = string.Empty;
				if (!hdrCharData.HeaderCharGroup_Exists(_headerCharForBackStockDate))
				{
					eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_InvalidHeaderCharacteristic);
					eMsg = eMsg.Replace("{0}", _headerCharForBackStockDate);
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InvalidHeaderCharacteristic, eMsg, this.ToString(), true);

					isValid = false;
				}
				if (!hdrCharData.HeaderCharGroup_Exists(_headerCharForForceToBackStock))
				{
					eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_InvalidHeaderCharacteristic);
					eMsg = eMsg.Replace("{0}", _headerCharForForceToBackStock);
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InvalidHeaderCharacteristic, eMsg, this.ToString(), true);

					isValid = false;
				}
				return isValid;
			}

			private WeekProfile GetWeekProfile(string date)
			{
				WeekProfile aWeek = null;
				DateTime dt = Include.UndefinedDate;
				try
				{
					dt = Convert.ToDateTime(date);
				}
				catch
				{
					string eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_DateConversionError);
					eMsg = eMsg.Replace("{0}", _headerCharForBackStockDate); 
					eMsg = eMsg.Replace("{1}", _headerId);
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_DateConversionError, eMsg, this.ToString(), true);
				}
				aWeek = _SAB.ClientServerSession.Calendar.GetWeek(dt);
				return aWeek;
			}

			private DayProfile GetDayProfile(string date)
			{
				DayProfile aDay = null;
				DateTime dt = Include.UndefinedDate;
				try
				{
					dt = Convert.ToDateTime(date);
				}
				catch
				{
					string eMsg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_DateConversionError);
					eMsg = eMsg.Replace("{0}", _headerCharForBackStockDate);
					eMsg = eMsg.Replace("{1}", _headerId);
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_DateConversionError, eMsg, this.ToString(), true);
				}
				aDay = _SAB.ClientServerSession.Calendar.GetDay(dt);
				return aDay;
			}


			private void PushToBackStock(int hdrRid, string hdrId)
			{
				try
				{
					MIDTimer balTimer = new MIDTimer();
					balTimer.Start();
					//=============================
					// Balance to Reserve Action
					//=============================
					GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
					bool aReviewFlag = false;
					bool aUseSystemTolerancePercent = false;
					double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
					int aStoreFilter = Include.AllStoreFilterRID;
					int aWorkFlowStepKey = -1;
					ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.BalanceToDC);
					AllocationWorkFlowStep aAllocationWorkFlowStep
						= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					_transaction.DoAllocationAction(aAllocationWorkFlowStep);
					balTimer.Stop("  BALANCE");
					//=============================
					// Release
					//=============================
					MIDTimer relTimer = new MIDTimer();
					relTimer.Start();
					aMethod = _transaction.CreateNewMethodAction(eMethodType.Release);
					aAllocationWorkFlowStep
						= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					_transaction.DoAllocationAction(aAllocationWorkFlowStep);
					relTimer.Stop("  RELEASE");
				}
				catch
				{
					throw;
				}

			}
		}
	}
}
