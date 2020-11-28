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
//Begin TT#707 - JScott - Size Curve process needs to multi-thread
using System.Threading;
//End TT#707 - JScott - Size Curve process needs to multi-thread

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.SizeCurveGenerate
{
	/// <summary>
	/// Summary description for SizeCurveGenerate.
	/// </summary>
	class SizeCurveGenerate
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
			string sourceModule = "SizeCurveGenerate.cs";
			string eventLogID = "SizeCurveGenerate";
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
			int _processId = Include.NoRID;
			ScheduleData _scheduleData;
			bool errorFound = false;
			eMIDMessageLevel highestMessage;
			ApplicationSessionTransaction _transaction;
			MethodBaseData _methodData;
			Exception innerE;
			DataRow taskListRow;
			int userRid;
			string tasklistName;
			MIDTimer taskTimer;
			string msg;
			DataTable dtTask;
			//Begin TT#707 - JScott - Size Curve process needs to multi-thread
			//int generateSeq;
			//End TT#707 - JScott - Size Curve process needs to multi-thread
			int nodeRid;
			//Begin TT#707 - JScott - Size Curve process needs to multi-thread
			//int versionRid;
			//End TT#707 - JScott - Size Curve process needs to multi-thread
			bool dateInRange;
			int cdrRid;
			int methodRid;
			//Begin TT#707 - JScott - Size Curve process needs to multi-thread
			//int workflowMethodInd;
			//eMethodType aMethodType;
			eMethodType methodType;
			//End TT#707 - JScott - Size Curve process needs to multi-thread
			SizeCurveMethod scMthd;
			//Begin TT#707 - JScott - Size Curve process needs to multi-thread
			int concurrentProcesses = Include.ConcurrentSizeCurveProcesses;
			string strParm;
			MethodSizeCurveData sizeCurveMethodData;
			SizeCurveCriteriaList nodeCriteriaList;
			LowLevelVersionOverrideProfileList lowLevelList;
			HierarchySessionTransaction hierTran;
			DataTable dtMerchBasisDetail;
			DataTable dtCurveBasisDetail;
			DataRow row;
			int sequence;
			Hashtable processHash;
			SizeCurveProcessStatus processStatus;
			SizeCurveConcurrentProcess methodProcess;
			SizeCurveToleranceProfile toleranceProfile;
			IDictionaryEnumerator iEnum;
			SortedList processList;
			Stack processStack;
			int i;
			SizeCurveGenerateThread[] processArray;
			//End TT#707 - JScott - Size Curve process needs to multi-thread
			//Begin TT#1076 - JScott - Size Curves by Set
			eSizeCurvesByType szCrvByType;
			//End TT#1076 - JScott - Size Curves by Set

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
					if (args[0] == Include.SchedulerID)
					{
						_taskListRid = Convert.ToInt32(args[1]);
						_taskSeq = Convert.ToInt32(args[2]);
						_processId = Convert.ToInt32(args[3]);
					}
					else
					{
						_taskListRid = Convert.ToInt32(args[0]);
						if (args.Length > 1)
						{
							_taskSeq = Convert.ToInt32(args[1]);
						}
						else
						{
							EventLog.WriteEntry(eventLogID, "Missing Argument: Task Sequence", EventLogEntryType.Error);
						}
					}
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
					SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Store | (int)eServerType.Hierarchy);
				}
				catch (Exception ex)
				{
					errorFound = true;
					innerE = ex;

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

				taskListRow = _scheduleData.TaskList_Read(_taskListRid);
				userRid = Include.UndefinedUserRID;
				tasklistName = string.Empty;

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
					SAB.ClientServerSession.UserLogin(userRid, eProcesses.sizeCurveGenerate);

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

				if (authentication != eSecurityAuthenticate.ActiveUser)
				{
					errorFound = true;
					System.Console.Write("Unable to log in with user RID:" + userRid.ToString());

					EventLog.WriteEntry(eventLogID, "Unable to log in with user RID:" + userRid.ToString(), EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				if (_processId != Include.NoRID)
				{
					SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					SAB.ClientServerSession.Initialize();
				}

				SAB.ApplicationServerSession.Initialize();
				SAB.StoreServerSession.Initialize();
				SAB.HierarchyServerSession.Initialize();

				taskTimer = new MIDTimer();
				taskTimer.Start();

				msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListBegin);
				msg = msg.Replace("{0}", tasklistName);
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, sourceModule, true);

				// Create a method data class so later we can determine what type of Method we have from
				// the RID.

				_methodData = new MethodBaseData();

				//Begin TT#707 - JScott - Size Curve process needs to multi-thread
				////===================================
				//// process the tasks for this request
				////===================================

				//dtTask = _scheduleData.TaskSizeCurveGenerate_ReadByTaskList(_taskListRid, _taskSeq);

				//foreach (DataRow aRow in dtTask.Rows)
				//{
				//    generateSeq = Convert.ToInt32(aRow["GENERATE_SEQUENCE"], CultureInfo.CurrentUICulture);

				//    if (aRow["HN_RID"] == DBNull.Value)
				//    {
				//        nodeRid = Include.NoRID;
				//    }
				//    else
				//    {
				//        nodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
				//    }

				//    if (aRow["METHOD_RID"] == DBNull.Value)
				//    {
				//        methodRid = Include.NoRID;
				//    }
				//    else
				//    {
				//        methodRid = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);
				//    }

				//    if (aRow["EXECUTE_CDR_RID"] == System.DBNull.Value)
				//    {
				//        dateInRange = true;
				//    }
				//    else
				//    {
				//        cdrRid = Convert.ToInt32(aRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);
				//        dateInRange = SAB.ApplicationServerSession.Calendar.IsCurrentDateWithinRange(cdrRid);
				//    }

				//    if (dateInRange)
				//    {
				//        _transaction = SAB.ApplicationServerSession.CreateTransaction();

				//        if (methodRid != Include.NoRID)
				//        {
				//            aMethodType = _methodData.GetMethodType(methodRid);

				//            switch (aMethodType)
				//            {
				//                case eMethodType.SizeCurve:
				//                    _transaction.ProcessMethod(eMethodType.SizeCurve, methodRid);
				//                    break;
				//                default:
				//                    errorFound = true;
				//                    System.Console.Write(msg);

				//                    msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_InvalidMethod);
				//                    msg = msg.Replace("{0}", tasklistName);
				//                    msg = msg.Replace("{1}", "N/A");
				//                    EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Error);
				//                    return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				//            }
				//        }
				//        else
				//        {
				//            // Untested Code for future implmentation of Size Curve data on the Hierarchy.  Force through SizeCurveMethod
				//            // by creating a new SizeCurveMethod, setting the parameters, and executing.

				//            scMthd = new SizeCurveMethod(SAB, SAB.HierarchyServerSession.GetNodeData(nodeRid));

				//            // Set Method Fields here

				//            scMthd.ProcessMethod(_transaction, Include.NoRID, null);
				//        }

				//        if (_transaction != null)
				//        {
				//            _transaction.Dispose();
				//        }
				//    }
				//}
				//===========================================
				// get the concurrent processes for this task
				//===========================================

				taskListRow = _scheduleData.TaskSizeCurveGenerateNode_Read(_taskListRid, _taskSeq);

				if (taskListRow != null)
				{
					concurrentProcesses = Convert.ToInt32(taskListRow["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture);
				}

				//================================================
				// Read ConcurrentProcesses Configuration Settings
				//================================================

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

				//===================================
				// process the tasks for this request
				//===================================

				dtTask = _scheduleData.TaskSizeCurveGenerate_ReadByTaskList(_taskListRid, _taskSeq);

				if (dtTask.Rows.Count > 0)
				{
					if (dtTask.Rows[0]["METHOD_RID"] != DBNull.Value)
					{
						foreach (DataRow aRow in dtTask.Rows)
						{
							methodRid = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);

							if (aRow["EXECUTE_CDR_RID"] == System.DBNull.Value)
							{
								dateInRange = true;
							}
							else
							{
								cdrRid = Convert.ToInt32(aRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);
								dateInRange = SAB.ApplicationServerSession.Calendar.IsCurrentDateWithinRange(cdrRid);
							}

							if (dateInRange)
							{
								_transaction = SAB.ApplicationServerSession.CreateTransaction();
								methodType = _methodData.GetMethodType(methodRid);

								switch (methodType)
								{
									case eMethodType.SizeCurve:
										_transaction.ProcessMethod(eMethodType.SizeCurve, methodRid);
										break;
									default:
										errorFound = true;
										System.Console.Write(msg);

										msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_InvalidMethod);
										msg = msg.Replace("{0}", tasklistName);
										msg = msg.Replace("{1}", "N/A");
										EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Error);
										return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
								}

								_transaction.Dispose();
							}
						}
					}
					else
					{
						sizeCurveMethodData = new MethodSizeCurveData();
						hierTran = new HierarchySessionTransaction(SAB);
						processHash = new Hashtable();
						processStatus = new SizeCurveProcessStatus();
						sequence = 0;

						foreach (DataRow aRow in dtTask.Rows)
						{
							nodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);

							if (aRow["EXECUTE_CDR_RID"] == System.DBNull.Value)
							{
								dateInRange = true;
							}
							else
							{
								cdrRid = Convert.ToInt32(aRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);
								dateInRange = SAB.ApplicationServerSession.Calendar.IsCurrentDateWithinRange(cdrRid);
							}

							if (dateInRange)
							{
								nodeCriteriaList = SAB.HierarchyServerSession.GetSizeCurveCriteriaList(nodeRid, false);

								foreach (SizeCurveCriteriaProfile sccp in nodeCriteriaList)
								{
									if (sccp.CriteriaLevelType == eLowLevelsType.LevelOffset)
									{
										lowLevelList = hierTran.GetOverrideList(sccp.CriteriaOLLRID, nodeRid, Include.FV_ActualRID,
																		   sccp.CriteriaLevelOffset, Include.NoRID, true, false);
									}
									else
									{
										lowLevelList = hierTran.GetOverrideList(sccp.CriteriaOLLRID, nodeRid, Include.FV_ActualRID,
																			eHierarchyDescendantType.levelType, sccp.CriteriaLevelSequence, Include.NoRID, true, false);
									}

									foreach (LowLevelVersionOverrideProfile lowLevel in lowLevelList)
									{
                                        // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
                                        if (lowLevel.Exclude)
                                        {
                                            continue;
                                        }
                                        // End TT#1952

										dtMerchBasisDetail = sizeCurveMethodData.GetMerchBasisData(Include.NoRID);

										row = dtMerchBasisDetail.NewRow();
										row["BASIS_SEQ"] = 0;
										row["HN_RID"] = lowLevel.NodeProfile.Key;
										row["FV_RID"] = Include.FV_ActualRID;
										row["CDR_RID"] = sccp.CriteriaDateRID;
										row["WEIGHT"] = 1;
										row["MERCH_TYPE"] = eMerchandiseType.Node;
										row["OLL_RID"] = Include.NoRID;
										row["CUSTOM_OLL_RID"] = Include.NoRID;
										dtMerchBasisDetail.Rows.Add(row);

										dtCurveBasisDetail = sizeCurveMethodData.GetCurveBasisData(Include.NoRID);

										toleranceProfile = SAB.HierarchyServerSession.GetSizeCurveToleranceProfile(lowLevel.NodeProfile.Key, false);

										//Begin TT#1076 - JScott - Size Curves by Set
										if (sccp.CriteriaSgRID == Include.NoRID)
										{
											szCrvByType = eSizeCurvesByType.Store;
										}
										else
										{
											szCrvByType = eSizeCurvesByType.AttributeSet;
										}

										//End TT#1076 - JScott - Size Curves by Set
										scMthd = new SizeCurveMethod(
											SAB,
											SAB.HierarchyServerSession.GetSizeCurveCriteriaProfileCurveName(lowLevel.NodeProfile, sccp),
											sccp.CriteriaSizeGroupRID,
											//Begin TT#1076 - JScott - Size Curves by Set
											szCrvByType,
											sccp.CriteriaSgRID,
											//End TT#1076 - JScott - Size Curves by Set
                                            // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                            //SAB.HierarchyServerSession.GetSizeCurveSimilarStoreList(StoreMgmt.StoreProfiles_GetActiveStoresList(), lowLevel.NodeProfile.Key, true, false), //TT#1517-MD -jsobek -Store Service Optimization
                                            SAB.HierarchyServerSession.GetSizeCurveSimilarStoreList(SAB.StoreServerSession.GetActiveStoresList(), lowLevel.NodeProfile.Key, true, false), //TT#1517-MD -jsobek -Store Service Optimization
                                            // End TT#1902-MD - JSmith - Store Services - VSW API Error
											sccp.CriteriaApplyLostSalesInd,
											toleranceProfile.ToleranceMinAvg,
											toleranceProfile.ToleranceLevelType,
											toleranceProfile.ToleranceLevelRID,
											toleranceProfile.ToleranceLevelSeq,
											toleranceProfile.ToleranceLevelOffset,
											toleranceProfile.ToleranceSalesTolerance,
											toleranceProfile.ToleranceIdxUnitsInd,
											toleranceProfile.ToleranceMinTolerance,
                                            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                                            toleranceProfile.ApplyMinToZeroTolerance,
                                            //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
											toleranceProfile.ToleranceMaxTolerance,
											dtMerchBasisDetail,
											dtCurveBasisDetail);

										methodProcess = new SizeCurveConcurrentProcess(
											SAB.ApplicationServerSession.Audit,
											sequence,
											lowLevel.NodeProfile.Key,
											sccp.CriteriaCurveName,
											scMthd,
											processStatus);

										processHash[methodProcess] = null;
										sequence++;
									}
								}
							}
						}

						processList = new SortedList();
						iEnum = processHash.GetEnumerator();

						while (iEnum.MoveNext())
						{
							processList.Add(iEnum.Key, null);
						}

						processStack = new Stack();
						iEnum = processList.GetEnumerator();

						while (iEnum.MoveNext())
						{
							processStack.Push(iEnum.Key);
						}

						if (concurrentProcesses > 1)
						{
							processArray = new SizeCurveGenerateThread[concurrentProcesses];

							for (i = 0; i < concurrentProcesses; i++)
							{
								processArray[i] = new SizeCurveGenerateThread(processStack);
								processArray[i].Initialize(SAB.ClientServerSession, sourceModule);
							}

							for (i = 0; i < concurrentProcesses; i++)
							{
								processArray[i].StartThread();
							}

							for (i = 0; i < concurrentProcesses; i++)
							{
								processArray[i].JoinThread();
								processArray[i].Cleanup();
							}
						}
						else
						{
							_transaction = SAB.ApplicationServerSession.CreateTransaction();
							_transaction.NeedHeaders = false;

							while (processStack.Count > 0)
							{
								methodProcess = (SizeCurveConcurrentProcess)processStack.Pop();
								methodProcess.ExecuteProcess(_transaction);
							}

							_transaction.Dispose();
						}

						SAB.ClientServerSession.Audit.SizeCurveGenerateAuditInfo_Add(
							processStatus.MethodsExecuted,
							processStatus.CompletedSuccessfully,
							processStatus.ActionFailed            // TT#241 - MD JEllis - Header Enqueue Process
                            + processStatus.HeaderEnqueueFailed   // TT#241 - MD Jellis - Header Enqueue Process
                            + processStatus.NoHeaderResourceLocks // TT#241 - MD JEllis - Header Enqueue Process
                            ,processStatus.NoActionPerformed);    // TT#241 - MD JEllis - Header Enqueue Process
					}
				}
				//End TT#707 - JScott - Size Curve process needs to multi-thread

				taskTimer.Stop();
				msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListEnd);
				msg = msg.Replace("{0}", tasklistName);
				msg = msg.Replace("{1}", taskTimer.ElaspedTimeString);
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, sourceModule, true);
			}
			catch (Exception err)
			{
				errorFound = true;

				message = err.Message;
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, sourceModule);
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
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread

		private class SizeCurveProcessStatus
		{
			//=======
			// FIELDS
			//=======

			private ArrayList lockList;
			private int _methodsExecuted;
			private int _completedSuccessfully;
			private int _actionFailed;
			private int _noActionPerformed;
            // begin TT#241 - MD JEllis - Header Enqueue Process
            private int _noHeaderResourceLocks;
            private int _headerEnqueueFailed;
            // end TT#241 - MD JEllis - Header Enqueue Process

			//=============
			// CONSTRUCTORS
			//=============

			public SizeCurveProcessStatus()
			{
				lockList = new ArrayList();
			}

			//===========
			// PROPERTIES
			//===========

			public int MethodsExecuted
			{
				get
				{
					return _methodsExecuted;
				}
			}

			public int CompletedSuccessfully
			{
				get
				{
					return _completedSuccessfully;
				}
			}

			public int ActionFailed
			{
				get
				{
					return _actionFailed;
				}
			}

			public int NoActionPerformed
			{
				get
				{
					return _noActionPerformed;
				}
			}
            // begin TT#241 - MD JEllis - Header Enqueue Process
            public int NoHeaderResourceLocks
            {
                get
                {
                    return _noHeaderResourceLocks;
                }
            }
            public int HeaderEnqueueFailed
            {
                get
                {
                    return _headerEnqueueFailed;
                }
            }
            // end TT#241 - MD JEllis - Header Enqueue Process
			//========
			// METHODS
			//========

			public void IncrementExecutedCount()
			{
				lock (lockList.SyncRoot)
				{
					_methodsExecuted++;
				}
			}

			public void IncrementStatusCount(eAllocationActionStatus aStatus)
			{
				lock (lockList.SyncRoot)
				{
					switch (aStatus)
					{
						case eAllocationActionStatus.ActionCompletedSuccessfully:
							_completedSuccessfully++;
							break;

						case eAllocationActionStatus.ActionFailed:
							_actionFailed++;
							break;

						case eAllocationActionStatus.NoActionPerformed:
							_noActionPerformed++;
							break;
                            // begin TT#241 - MD - JEllis - Header Enqueue Process
                        case eAllocationActionStatus.NoHeaderResourceLocks:
                            _noHeaderResourceLocks++;
                            break;
                        case eAllocationActionStatus.HeaderEnqueueFailed:
                            _headerEnqueueFailed++;
                            break;
                            // end TT#241 - MD - JEllis - Header Enqueue Process
					}
				}
			}
		}

		private class SizeCurveConcurrentProcess : IComparable
		{
			//=======
			// FIELDS
			//=======

			private int _sequence;
			private int _nodeRID;
			private string _curveName;
			private SizeCurveMethod _szCrvMthd;
			private SizeCurveProcessStatus _processStatus;

			//=============
			// CONSTRUCTORS
			//=============

			public SizeCurveConcurrentProcess(
				Audit aAudit,
				int aSequence,
				int aNodeRID,
				string aCurveName,
				SizeCurveMethod aSzCrvMthd,
				SizeCurveProcessStatus aProcessStatus)
			{
				_sequence = aSequence;
				_nodeRID = aNodeRID;
				_curveName = aCurveName;
				_szCrvMthd = aSzCrvMthd;
				_processStatus = aProcessStatus;
			}

			//===========
			// PROPERTIES
			//===========

			public SizeCurveMethod SizeCurveMethod
			{
				get
				{
					return _szCrvMthd;
				}
			}

			//========
			// METHODS
			//========

			public void ExecuteProcess(ApplicationSessionTransaction aTransaction)
			{
				try
				{
					_processStatus.IncrementExecutedCount();
					_szCrvMthd.ProcessMethod(aTransaction, Include.NoRID, null);
					_processStatus.IncrementStatusCount(aTransaction.AllocationActionAllHeaderStatus);
				}
				catch
				{
					throw;
				}
			}

			public override bool Equals(object obj)
			{
				try
				{
					if (obj.GetType() == typeof(SizeCurveConcurrentProcess))
					{
						if (((SizeCurveConcurrentProcess)obj)._nodeRID == _nodeRID &&
							((SizeCurveConcurrentProcess)obj)._curveName == _curveName)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
				catch
				{
					throw;
				}
			}

			public override int GetHashCode()
			{
				try
				{
					return Include.CreateHashKey(_nodeRID, _curveName.GetHashCode());
				}
				catch
				{
					throw;
				}
			}

			public int CompareTo(object obj)
			{
				try
				{
					if (obj.GetType() == typeof(SizeCurveConcurrentProcess))
					{
						if (_sequence == ((SizeCurveConcurrentProcess)obj)._sequence)
						{
							return 0;
						}
						else
						{
							if (_sequence < ((SizeCurveConcurrentProcess)obj)._sequence)
							{
								return 1;
							}
							else
							{
								return -1;
							}
						}
					}
					else
					{
						return 1;
					}
				}
				catch
				{
					throw;
				}
			}
		}

		private class SizeCurveSAB
		{
			//=======
			// FIELDS
			//=======

			private bool _errorFound;
			private SessionAddressBlock _SAB;

			//=============
			// CONSTRUCTORS
			//=============

			public SizeCurveSAB(Session aOwnerSession, string aOwnerModule)
			{
				_errorFound = false;
				Initialize(aOwnerSession, aOwnerModule);
			}

			//===========
			// PROPERTIES
			//===========

			public SessionAddressBlock SAB
			{
				get
				{
					return _SAB;
				}
			}

			public bool ErrorFound
			{
				get
				{
					return _errorFound;
				}
				set
				{
					_errorFound = value;
				}
			}

			//========
			// METHODS
			//========

			private void Initialize(Session aOwnerSession, string aOwnerModule)
			{
				SessionSponsor sponsor;
				IMessageCallback messageCallback;
				Exception innerE;
				string userId = null;
				string passWd = null;
				eSecurityAuthenticate authentication;

				try
				{
					sponsor = new SessionSponsor();
					messageCallback = new BatchMessageCallback();
					_SAB = new SessionAddressBlock(messageCallback, sponsor);

					// ===============
					// Create Sessions
					// ===============

					try
					{
						_SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Store);
					}
					catch (Exception Ex)
					{
						innerE = Ex;

						while (innerE.InnerException != null)
						{
							innerE = innerE.InnerException;
						}

						aOwnerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							"SizeCurveConcurrentProcess:Initialize():Error creating sessions - " + innerE.ToString(),
							aOwnerModule);

						_errorFound = true;
						return;
					}

					// =====
					// Login
					// =====

					userId = MIDConfigurationManager.AppSettings["User"];
					passWd = MIDConfigurationManager.AppSettings["Password"];

					if ((userId == "" || userId == null) &&
						(passWd == "" || passWd == null))
					{
						aOwnerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							"SizeCurveConcurrentProcess:Initialize():User and Password NOT specified",
							aOwnerModule);

						_errorFound = true;
						return;
					}

					authentication = _SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.sizeCurveGenerateThread);

					if (authentication != eSecurityAuthenticate.UserAuthenticated)
					{
						aOwnerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							"SizeCurveConcurrentProcess:Initialize():Unable to log in with user: [" + userId + "] password: [" + passWd + "]",
							aOwnerModule);

						_errorFound = true;
						return;
					}

					// ===================
					// Initialize Sessions
					// ===================

					_SAB.ClientServerSession.Initialize();
					_SAB.ApplicationServerSession.Initialize();
                    //_SAB.HierarchyServerSession.Initialize();
					_SAB.StoreServerSession.Initialize();
                    // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                    // StoreServerSession must be initialized before HierarchyServerSession 
                    SAB.HierarchyServerSession.Initialize();
                    // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
				}
				catch (Exception Ex)
				{
					aOwnerSession.Audit.Add_Msg(
						eMIDMessageLevel.Error,
						"SizeCurveConcurrentProcess:Initialize():Error Encountered - " + Ex.ToString(),
						aOwnerModule);

					_errorFound = true;
					return;
				}
			}
		}

		private class SizeCurveGenerateThread
		{
			//=======
			// FIELDS
			//=======

			private Stack _processStack;

			//Begin TT#1112 - JScott - Size curve build ends abnormal but still appears running
			private string _ownerModule;
			//End TT#1112 - JScott - Size curve build ends abnormal but still appears running
			private SizeCurveSAB _sizeCurveSAB;
			private Thread _processThread;
			private ApplicationSessionTransaction _transaction;

			//=============
			// CONSTRUCTORS
			//=============

			public SizeCurveGenerateThread(Stack aProcessStack)
			{
				_processStack = aProcessStack;

			}

			//===========
			// PROPERTIES
			//===========

			//========
			// METHODS
			//========

			public void Initialize(Session aOwnerSession, string aOwnerModule)
			{
				try
				{
					//Begin TT#1112 - JScott - Size curve build ends abnormal but still appears running
					_ownerModule = aOwnerModule;

					//End TT#1112 - JScott - Size curve build ends abnormal but still appears running
					_sizeCurveSAB = new SizeCurveSAB(aOwnerSession, aOwnerModule);
					_processThread = new Thread(new ThreadStart(Execute));
					_transaction = _sizeCurveSAB.SAB.ApplicationServerSession.CreateTransaction();
					_transaction.NeedHeaders = false;
				}
				catch
				{
					throw;
				}
			}

			public void StartThread()
			{
				try
				{
					_processThread.Start();
				}
				catch
				{
					throw;
				}
			}

			public void JoinThread()
			{
				try
				{
					_processThread.Join();
				}
				catch
				{
					throw;
				}
			}

			public void Cleanup()
			{
				try
				{
					_transaction.Dispose();

					if (_sizeCurveSAB.SAB.ClientServerSession != null && _sizeCurveSAB.SAB.ClientServerSession.Audit != null)
					{
						if (!_sizeCurveSAB.ErrorFound)
						{
							_sizeCurveSAB.SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _sizeCurveSAB.SAB.GetHighestAuditMessageLevel());
						}
						else
						{
							_sizeCurveSAB.SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _sizeCurveSAB.SAB.GetHighestAuditMessageLevel());
						}
					}

					_sizeCurveSAB.SAB.CloseSessions();
				}
				catch
				{
					throw;
				}
			}

			private void Execute()
			{
				SizeCurveConcurrentProcess process;

				try
				{
					process = GetNextProcess();

					while (process != null)
					{
						//Begin TT#1112 - JScott - Size curve build ends abnormal but still appears running
						//process.ExecuteProcess(_transaction);
						try
						{
							process.ExecuteProcess(_transaction);
						}
						catch (MIDException MIDEx)
						{
							_sizeCurveSAB.SAB.ClientServerSession.Audit.Add_Msg(
								Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
						}
						catch (Exception Ex)
						{
							_sizeCurveSAB.SAB.ClientServerSession.Audit.Add_Msg(
								eMIDMessageLevel.Severe,
								"SizeCurveGenerateThread:Execute(): Unknown error encountered in SizeCurveConcurrentProcess:ExecuteProcess() - " + Ex.ToString(),
								_ownerModule);
						}

						//End TT#1112 - JScott - Size curve build ends abnormal but still appears running
						process = GetNextProcess();
					}
				}
				//Begin TT#1112 - JScott - Size curve build ends abnormal but still appears running
				//catch
				//{
				//    throw;
				//}
				catch (MIDException MIDEx)
				{
					_sizeCurveSAB.SAB.ClientServerSession.Audit.Add_Msg(
						Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
				}
				catch (Exception Ex)
				{
					_sizeCurveSAB.SAB.ClientServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						"SizeCurveGenerateThread:Execute(): Error Encountered - " + Ex.ToString(),
						_ownerModule);
				}
				//End TT#1112 - JScott - Size curve build ends abnormal but still appears running
			}

			private SizeCurveConcurrentProcess GetNextProcess()
			{
				SizeCurveConcurrentProcess process;

				try
				{
					lock (_processStack.SyncRoot)
					{
						if (_processStack.Count > 0)
						{
							process = (SizeCurveConcurrentProcess)_processStack.Pop();
						}
						else
						{
							process = null;
						}
					}

					return process;
				}
				catch
				{
					throw;
				}
			}
		}
		//End TT#707 - JScott - Size Curve process needs to multi-thread
	}
}
