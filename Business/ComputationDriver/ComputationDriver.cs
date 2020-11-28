using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for ComputationDriver.
	/// </summary>
	public class ComputationDriver
	{
		//=======
		// FIELDS
		//=======

		SessionAddressBlock _SAB;
		int _concurrentProcesses;
		Hashtable _forecastVersions = new Hashtable();
		ComputationData _cd = new ComputationData();
		private int _totalItems = 0;
		private int _totalErrors = 0;
		MRSCalendar _cal = null;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationDriver(SessionAddressBlock aSAB, int aConcurrentProcesses)
		{
			_SAB = aSAB;
			_concurrentProcesses = aConcurrentProcesses;
		}

		//===========
		// PROPERTIES
		//===========

		public int TotalItems 
		{
			get { return _totalItems ; }
		}

		public int TotalErrors 
		{
			get { return _totalErrors ; }
		}

		private MRSCalendar Cal 
		{
			get 
			{ 
				if (_cal == null)
				{
					_cal = new MRSCalendar();
				}
				return _cal ; 
			}
		}

		//========
		// METHODS
		//========


		public void ProcessComputationDriverRequests(Session aSession, eProcesses aProcess)
		{
			try
			{
				try
				{
					int totalItems = _cd.GetItemCount(aProcess);
					_totalItems += totalItems;
					Stack computationStack;
					// use ArrayList to maintain reference until errors are counted
					ArrayList computationArrayList = new ArrayList();
					
					computationStack = new Stack();
					BuildComputationStack(_SAB.ClientServerSession.Audit, computationStack, computationArrayList, aProcess, aSession);
#if (DEBUG)
					foreach (ComputationDriverProcess cdp in computationArrayList)
					{
						Debug.WriteLine(cdp.ComputationGroupRID.ToString() + " | " 
							+ cdp.NodeProfile.Key.ToString() + " | "
							+ cdp.VersionProfile.Key.ToString() + " | "
							+ cdp.FromYearWeek.ToString() + " | "
							+ cdp.ToYearWeek.ToString()
							);
					}
#endif
					if (_concurrentProcesses > 1)
					{
						ConcurrentProcessManager cpm = new ConcurrentProcessManager(_SAB.ClientServerSession.Audit, computationStack, _concurrentProcesses, 5000);
						cpm.ProcessCommands();
						foreach (ConcurrentProcess cp in computationArrayList)
						{
							_totalErrors += cp.NumberOfErrors;
						}
					}
					else
					{
						while (computationStack.Count > 0)
						{
							ConcurrentProcess cp = (ConcurrentProcess)computationStack.Pop();
							cp.ExecuteProcess();
							_totalErrors += cp.NumberOfErrors;
						}
					}

					// remove all processed items
					if (!_cd.ConnectionIsOpen)
					{
						_cd.OpenUpdateConnection();
					}
					// clean up processed items
					_cd.DeleteProcessedComputationItems();
					// clean up processed groups
					Hashtable computationGroups = new Hashtable();
					foreach (ComputationDriverProcess cdp in computationArrayList)
					{
						if (!computationGroups.ContainsKey(cdp.ComputationGroupRID))
						{
							if (_cd.GetItemCount(cdp.ComputationGroupRID) == 0)
							{
								_cd.DeleteComputationGroup(cdp.ComputationGroupRID);
							}
							computationGroups.Add(cdp.ComputationGroupRID, null);
						}
					}
					_cd.CommitData();
					
				}
				catch ( Exception ex )
				{
					aSession.Audit.Log_Exception(ex, GetType().Name);
				}
				finally
				{
					if (_cd.ConnectionIsOpen)
					{
						_cd.CloseUpdateConnection();
					}
				}
			}
			catch ( Exception ex )
			{
				aSession.Audit.Log_Exception(ex, GetType().Name);
			}
			
		}

		private eReturnCode BuildComputationStack(Audit aAudit, Stack acomputationStack, ArrayList computationArrayList, eProcesses aProcess, Session aSession)
		{
			try
			{
				int computationGroupRID = -1;
				int nodeRID = -1;
				eComputationType computationType = eComputationType.None;
				int versionRID = -1;
				string computationMode = string.Empty;

				int currComputationGroupRID = -1;
				int currNodeRID = -1;
				eComputationType currComputationType = eComputationType.None;
				int currVersionRID = -1;
				int currYearWeek = -1;
				string currComputationMode = string.Empty;

				int priorYearWeek = -1;
				int fromYearWeek = 0;
				int toYearWeek = 0;
				computationArrayList.Clear();
				ConcurrentProcess computationDriverProcess = null;
				bool addProcess = false;
				WeekProfile week;

				ProfileList versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();
				VersionProfile versionProfile = null;
				HierarchyNodeProfile nodeProfile = null;
				int storeGroupRID = _SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;

				ComputationData cd = new ComputationData();
				DataTable dt = cd.ComputationItems_Read(aProcess);
				foreach (DataRow dr in dt.Rows)
				{
					currComputationGroupRID = Convert.ToInt32(dr["CG_RID"], CultureInfo.CurrentUICulture);
					currNodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					currComputationType = (eComputationType)Convert.ToInt32(dr["ITEM_TYPE"], CultureInfo.CurrentUICulture);
					currVersionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
					currYearWeek = Convert.ToInt32(dr["FISCAL_YEAR_WEEK"], CultureInfo.CurrentUICulture);
					currComputationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);

					if (computationType == eComputationType.None)
					{
						computationGroupRID = currComputationGroupRID;
						nodeRID = currNodeRID;
						computationType = currComputationType;
						versionRID = currVersionRID;
						priorYearWeek = currYearWeek;
						fromYearWeek = currYearWeek;
						toYearWeek = currYearWeek;
						computationMode = currComputationMode;
					}
					else if (computationGroupRID != currComputationGroupRID ||
						nodeRID != currNodeRID ||
						computationType != currComputationType ||
						versionRID != currVersionRID ||
						computationMode != currComputationMode)
					{
						addProcess = true;
					}
					else // check for contiguous weeks
					{
						if (currYearWeek - priorYearWeek != 1)
						{
							week = Cal.GetWeek(currYearWeek);
							week = Cal.Add(week, -1);
							if (week.YearWeek != priorYearWeek)
							{
								addProcess = true;
							}
						}
					}

					if (versionProfile == null ||
						versionProfile.Key != versionRID)
					{
						versionProfile = (VersionProfile)versionProfList.FindKey(versionRID);
					}

					if (nodeProfile == null ||
						nodeProfile.Key != nodeRID)
					{
						// get node profile and set security to full control
						nodeProfile = _SAB.HierarchyServerSession.GetNodeData(nodeRID);
						nodeProfile.StoreSecurityProfile = new HierarchyNodeSecurityProfile(nodeRID);
						nodeProfile.StoreSecurityProfile.SetFullControl();
						nodeProfile.ChainSecurityProfile = new HierarchyNodeSecurityProfile(nodeRID);
						nodeProfile.ChainSecurityProfile.SetFullControl();
					}

					if (addProcess)
					{
						toYearWeek = priorYearWeek;
						switch (computationType)
						{
							case eComputationType.Chain:
								computationDriverProcess = new ChainComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, computationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
								acomputationStack.Push(computationDriverProcess);
								computationArrayList.Add(computationDriverProcess);
								break;
							case eComputationType.Store:
								computationDriverProcess = new StoreComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, computationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
								acomputationStack.Push(computationDriverProcess);
								computationArrayList.Add(computationDriverProcess);
								break;
							case eComputationType.All:
								computationDriverProcess = new ChainComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, computationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
								acomputationStack.Push(computationDriverProcess);
								computationArrayList.Add(computationDriverProcess);
								computationDriverProcess = new StoreComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, computationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
								acomputationStack.Push(computationDriverProcess);
								computationArrayList.Add(computationDriverProcess);
								break;
						}
						addProcess = false;
						computationGroupRID = currComputationGroupRID;
						nodeRID = currNodeRID;
						computationType = currComputationType;
						versionRID = currVersionRID;
						computationMode = currComputationMode;
						fromYearWeek = currYearWeek;
					}

					priorYearWeek = currYearWeek;
				}

				if (currComputationType != eComputationType.None)
				{
					if (versionProfile == null ||
						versionProfile.Key != versionRID)
					{
						versionProfile = (VersionProfile)versionProfList.FindKey(versionRID);
					}

					if (nodeProfile == null ||
						nodeProfile.Key != nodeRID)
					{
						// get node profile and set security to full control
						nodeProfile = _SAB.HierarchyServerSession.GetNodeData(nodeRID);
						nodeProfile.StoreSecurityProfile = new HierarchyNodeSecurityProfile(nodeRID);
						nodeProfile.StoreSecurityProfile.SetFullControl();
						nodeProfile.ChainSecurityProfile = new HierarchyNodeSecurityProfile(nodeRID);
						nodeProfile.ChainSecurityProfile.SetFullControl();
					}

					toYearWeek = priorYearWeek;
					switch (currComputationType)
					{
						case eComputationType.Chain:
							computationDriverProcess = new ChainComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, currComputationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
							acomputationStack.Push(computationDriverProcess);
							computationArrayList.Add(computationDriverProcess);
							break;
						case eComputationType.Store:
							computationDriverProcess = new StoreComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, currComputationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
							acomputationStack.Push(computationDriverProcess);
							computationArrayList.Add(computationDriverProcess);
							break;
						case eComputationType.All:
							computationDriverProcess = new ChainComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, currComputationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
							acomputationStack.Push(computationDriverProcess);
							computationArrayList.Add(computationDriverProcess);
							computationDriverProcess = new StoreComputationDriverProcess(_SAB, _SAB.ClientServerSession, aProcess, currComputationGroupRID, nodeProfile, versionProfile, fromYearWeek, toYearWeek, computationMode, storeGroupRID);
							acomputationStack.Push(computationDriverProcess);
							computationArrayList.Add(computationDriverProcess);
							break;
					}
				}

				return eReturnCode.successful;
			}
			catch (Exception exc)
			{
				aAudit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildComputationStack", "ComputationDriver");
				aAudit.Log_Exception(exc, GetType().Name);
	
				return eReturnCode.severe;
			}
		}
	}

	abstract public class ComputationDriverProcess : ConcurrentProcess
	{
		//=======
		// FIELDS
		//=======
		private eProcesses _process;
		private eComputationType _computationType;
		private int _computationDriverRID;
		private int _computationGroupRID;
        //private int _nodeRID;
        //private int _version;
		private int _fromYearWeek;
		private int _toYearWeek;
		private string _computationMode;
		private int _storeGroupRID;
		private ePlanSessionType _planSessionType;
		private ComputationData _cd = null;
		private SessionAddressBlock _SAB;
		private Session _session;
		private ApplicationSessionTransaction _transaction = null;
		private PlanCubeGroup _planCubeGroup;
		private PlanOpenParms _openParms = null;
		private PlanSaveParms _planSaveParms = null;
		private VersionProfile _versionProfile = null;
		private HierarchyNodeProfile _nodeProfile = null;
		
		//=============
		// CONSTRUCTORS
		//=============

		public ComputationDriverProcess(SessionAddressBlock aSAB, Session aSession, eComputationType aComputationType, 
			eProcesses aProcess, int aComputationGroupRID, HierarchyNodeProfile aNodeProfile, VersionProfile aVersionProfile, 
			int aFromYearWeek, int aToYearWeek,
			ePlanSessionType aPlanSessionType, string aComputationMode,  
			int aStoreGroupRID)
			: base(aSession.Audit)
		{
			try
			{
				_SAB = aSAB;
				_session = aSession;
				_process = aProcess;
				_computationGroupRID = aComputationGroupRID;
				_fromYearWeek = aFromYearWeek;
				_toYearWeek = aToYearWeek;
				_computationType = aComputationType;
				_computationMode = aComputationMode;
				_planSessionType = aPlanSessionType;
				_versionProfile = aVersionProfile;
				_storeGroupRID = aStoreGroupRID;
				_nodeProfile = aNodeProfile;
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public eProcesses Process 
		{
			get { return _process ; }
		}

		public int ComputationDriverRID 
		{
			get { return _computationDriverRID ; }
		}

		public int ComputationGroupRID 
		{
			get { return _computationGroupRID ; }
		}

		public int NodeRID 
		{
			get { return _nodeProfile.Key ; }
		}

		public int VersionRID 
		{
			get { return _versionProfile.Key ; }
		}

		public int FromYearWeek 
		{
			get { return _fromYearWeek ; }
		}

		public int ToYearWeek 
		{
			get { return _toYearWeek ; }
		}

		public int StoreGroupRID 
		{
			get { return _storeGroupRID ; }
		}

		public ComputationData ComputationData 
		{
			get 
			{ 
				if (_cd == null)
				{
					_cd = new ComputationData();
				}
				return _cd ; 
			}
		}

		public SessionAddressBlock SAB 
		{
			get { return _SAB ; }
		}

		public Session Session 
		{
			get { return _session ; }
		}

		public ApplicationSessionTransaction Transaction 
		{
			get 
			{ 
				if (_transaction == null)
				{
					_transaction = _SAB.ApplicationServerSession.CreateTransaction();
				}
				return _transaction ; 
			}
		}

		public PlanCubeGroup PlanCubeGroup 
		{
			get { return _planCubeGroup ; }
			set { _planCubeGroup = value ; }
		}

		public PlanOpenParms OpenParms 
		{
			get 
			{ 
				if (_openParms == null)
				{
					_openParms = new PlanOpenParms(_planSessionType, _computationMode);
				}
				return _openParms ; 
			}
		}

		public PlanSaveParms PlanSaveParms 
		{
			get 
			{ 
				if (_planSaveParms == null)
				{
					_planSaveParms = new PlanSaveParms();
				}
				return _planSaveParms ; 
			}
		}

		public VersionProfile VersionProfile 
		{
			get 
			{ 
				return _versionProfile ; 
			}
		}

		public HierarchyNodeProfile NodeProfile 
		{
			get 
			{ 
				return _nodeProfile ; 
			}
		}

		//========
		// METHODS
		//========

		public void AddComputationDriverProcess()
		{
			try
			{
				ComputationData.OpenUpdateConnection();
				_computationDriverRID = ComputationData.AddComputationProcess(SAB.ClientServerSession.Audit.ProcessRID, _computationType, eProcessCompletionStatus.None);
				ComputationData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				ComputationData.CommitData();
			}
		}

		public void UpdateComputationDriverProcess(eProcessCompletionStatus aCompletionStatus)
		{
			try
			{
				try
				{
					ComputationData.OpenUpdateConnection();
					ComputationData.UpdateItemProcessed(Process, ComputationGroupRID, _computationType, 
						NodeRID, VersionRID,
						FromYearWeek, ToYearWeek);
					ComputationData.UpdateComputationDriverProcess(_computationDriverRID, aCompletionStatus);
					ComputationData.CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					if (ComputationData != null &&
						ComputationData.ConnectionIsOpen)
					{
						ComputationData.CloseUpdateConnection();
					}
				}
			}
			catch (Exception exc)
			{
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

	}

	public class StoreComputationDriverProcess : ComputationDriverProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

		public StoreComputationDriverProcess(SessionAddressBlock aSAB, Session aSession, eProcesses aProcess, 
			int aComputationGroupRID, HierarchyNodeProfile aNodeProfile, VersionProfile aVersionProfile, 
			int aFromYearWeek, int aToYearWeek, string aComputationMode, int aStoreGroupRID)
			: base(aSAB, aSession, eComputationType.Store, aProcess, aComputationGroupRID, 
			aNodeProfile, aVersionProfile, aFromYearWeek, aToYearWeek, ePlanSessionType.StoreSingleLevel, 
			aComputationMode, aStoreGroupRID)
		{
			try
			{
				
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreComputationDriverProcess");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.None;

			string message = "Executing store ComputationDriver for node:" + NodeProfile.Text +
				"; version=" + VersionProfile.Description + " from " + FromYearWeek.ToString() + " to " + ToYearWeek.ToString();
			try
			{
				
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreComputationDriverProcess");

				try
				{
					try
					{
						AddComputationDriverProcess();
						PlanCubeGroup = Transaction.CreateStorePlanMaintCubeGroup();

						OpenParms.StoreGroupRID = StoreGroupRID;
						OpenParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
						OpenParms.ViewRID = Include.DefaultPlanViewRID;
						OpenParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
						OpenParms.IneligibleStores = false;
						OpenParms.SimilarStores = true;
						OpenParms.LowLevelsType = eLowLevelsType.None;
						OpenParms.ChainHLPlanProfile.VersionProfile = VersionProfile;
						OpenParms.StoreHLPlanProfile.VersionProfile = VersionProfile;
						OpenParms.LowLevelVersionDefault = VersionProfile;
						OpenParms.StoreHLPlanProfile.NodeProfile = NodeProfile;
						OpenParms.ChainHLPlanProfile.NodeProfile = NodeProfile;
						OpenParms.DateRangeProfile = Session.Calendar.AddDateRangeFromWeeks(FromYearWeek, ToYearWeek);
						OpenParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastReview);
						OpenParms.FunctionSecurityProfile.SetAllowUpdate();
						OpenParms.AllowReadOnlyOnConflict = false;
						OpenParms.FormatErrorsForMessageBox = false;
						OpenParms.UpdateAuditHeaderOnError = false;
						PlanCubeGroup.OpenCubeGroup(OpenParms);

						PlanSaveParms.StoreHighLevelNodeRID = OpenParms.StoreHLPlanProfile.NodeProfile.Key;
						PlanSaveParms.StoreHighLevelVersionRID = OpenParms.StoreHLPlanProfile.VersionProfile.Key;
						PlanSaveParms.StoreHighLevelDateRangeRID = OpenParms.DateRangeProfile.Key;
						PlanSaveParms.SaveStoreHighLevel = true;
						PlanCubeGroup.SaveCubeGroup(PlanSaveParms);

						//						PlanCubeGroup.CloseCubeGroup();
						//						PlanCubeGroup.Dispose();
						//						// Begin MID Track #5210 - JSmith - Out of memory
						//						PlanCubeGroup = null;
						//						// End MID Track #5210

						UpdateComputationDriverProcess(eProcessCompletionStatus.Successful);
					}
					catch (PlanInUseException ex)
					{
						++NumberOfErrors;
						Audit.Add_Msg(eMIDMessageLevel.Warning, ex.InUseMessage, "StoreComputationDriverProcess");
						messageLevel = eMIDMessageLevel.Warning;
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						UpdateComputationDriverProcess(eProcessCompletionStatus.Failed);
						messageLevel = eMIDMessageLevel.Severe;
					}
					finally
					{
						if (PlanCubeGroup != null)
						{
							PlanCubeGroup.CloseCubeGroup();
							PlanCubeGroup.Dispose();
							PlanCubeGroup = null;
						}
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteComputationDriver", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

				Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
			}
		}
	}

	public class ChainComputationDriverProcess : ComputationDriverProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

		public ChainComputationDriverProcess(SessionAddressBlock aSAB, Session aSession, eProcesses aProcess, int aComputationGroupRID, 
			HierarchyNodeProfile aNodeProfile, VersionProfile aVersionProfile, 
			int aFromYearWeek, int aToYearWeek, 
			string aComputationMode, int aStoreGroupRID)
			: base(aSAB, aSession, eComputationType.Chain, aProcess, aComputationGroupRID, aNodeProfile, 
			aVersionProfile, aFromYearWeek, aToYearWeek, ePlanSessionType.ChainSingleLevel, 
			aComputationMode, aStoreGroupRID)
		{
			try
			{
				
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ChainComputationDriverProcess");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.None;

			string message = "Executing chain ComputationDriver for node:" + NodeProfile.Text +
				"; version=" + VersionProfile.Description + " from " + FromYearWeek.ToString() + " to " + ToYearWeek.ToString();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "ChainComputationDriverProcess");

				try
				{
					try
					{
						AddComputationDriverProcess();
						PlanCubeGroup = Transaction.CreateChainPlanMaintCubeGroup();

//						OpenParms.StoreGroupRID = StoreGroupRID;
						OpenParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
						OpenParms.ViewRID = Include.DefaultPlanViewRID;
						OpenParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
//						OpenParms.IneligibleStores = false;
//						OpenParms.SimilarStores = true;
						OpenParms.LowLevelsType = eLowLevelsType.None;
						OpenParms.ChainHLPlanProfile.VersionProfile = VersionProfile;
//						OpenParms.StoreHLPlanProfile.VersionProfile = VersionProfile;
						OpenParms.LowLevelVersionDefault = VersionProfile;
//						OpenParms.StoreHLPlanProfile.NodeProfile = NodeProfile;
						OpenParms.ChainHLPlanProfile.NodeProfile = NodeProfile;
						OpenParms.DateRangeProfile = Session.Calendar.AddDateRangeFromWeeks(FromYearWeek, ToYearWeek);
						OpenParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastReview);
						OpenParms.FunctionSecurityProfile.SetAllowUpdate();
						OpenParms.AllowReadOnlyOnConflict = false;
						OpenParms.FormatErrorsForMessageBox = false;
						OpenParms.UpdateAuditHeaderOnError = false;
						PlanCubeGroup.OpenCubeGroup(OpenParms);


						PlanSaveParms.ChainHighLevelNodeRID = OpenParms.ChainHLPlanProfile.NodeProfile.Key;
						PlanSaveParms.ChainHighLevelVersionRID = OpenParms.ChainHLPlanProfile.VersionProfile.Key;
						PlanSaveParms.ChainHighLevelDateRangeRID = OpenParms.DateRangeProfile.Key;
						PlanSaveParms.SaveHighLevelAllStoreAsChain = false;
						PlanSaveParms.SaveChainHighLevel = true;
						PlanCubeGroup.SaveCubeGroup(PlanSaveParms);

						//						PlanCubeGroup.CloseCubeGroup();
						//						PlanCubeGroup.Dispose();
						//						// Begin MID Track #5210 - JSmith - Out of memory
						//						PlanCubeGroup = null;
						//						// End MID Track #5210

						UpdateComputationDriverProcess(eProcessCompletionStatus.Successful);
					}
					catch (PlanInUseException ex)
					{
						++NumberOfErrors;
						Audit.Add_Msg(eMIDMessageLevel.Warning, ex.InUseMessage, "ChainComputationDriverProcess");
						messageLevel = eMIDMessageLevel.Warning;
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						UpdateComputationDriverProcess(eProcessCompletionStatus.Failed);
						messageLevel = eMIDMessageLevel.Severe;
					}
					finally
					{
						if (PlanCubeGroup != null)
						{
							PlanCubeGroup.CloseCubeGroup();
							PlanCubeGroup.Dispose();
							PlanCubeGroup = null;
						}
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "ChainComputationDriverProcess");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteComputationDriver", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

				Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
			}
		}
	}

	
}
