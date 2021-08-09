using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;
using System.Collections.Generic;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for OTSForecastCopyMethod.
	/// </summary>
	public class OTSForecastCopyMethod : OTSPlanBaseMethod
	{
		private OTSForecastCopyMethodData	_ForecastCopyData;
		private DataSet _dsForecastCopy;
		private	int	_hierNodeRid;
		private int _versionRid;
		private int _dateRangeRid;
		private ePlanType _planType; //Chain or Store
		private int _storeFilterRid;
        private bool _multiLevelInd;
        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        //private int _fromLevel;
        //private int _toLevel;
        private eFromLevelsType _fromLevelType;
        private int _fromLevelOffset;
        private int _fromLevelSequence;
        private eToLevelsType _toLevelType;
        private int _toLevelOffset;
        private int _toLevelSequence;
        // END Track #6107
		private ProfileList _groupLevelBasisDetail;

		private string _monitor;
		private string _monitorFilePath;
		private SessionAddressBlock _SAB;
		private string _infoMsg;
		private PlanCubeGroup _cubeGroup;
		private PlanOpenParms _openParms;
		private ApplicationSessionTransaction _applicationTransaction;
		private string _computationsMode = "Default";
		private int _nodeOverrideRid = Include.NoRID;
		private int _versionOverrideRid = Include.NoRID;
		private int _currentSglRid = Include.NoRID;
        private int _overrideLowLevelRid;
		private LowLevelVersionOverrideProfileList _overrideLowLevelList;
        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        private bool _copyPreInitValues;
        // End Track #6347
        private System.Data.DataTable _dtBasis;
        private System.Data.DataTable _dtGroupLevel;

		#region Properties
		/// <summary>
		/// Gets the ProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
                if (_planType == ePlanType.Chain)
                {
                    return eProfileType.MethodCopyChainForecast;
                }
                else
                {
                    return eProfileType.MethodCopyStoreForecast;
                }
			}
		}
		
		public DataSet DSForecastCopy
		{
			get	{return _dsForecastCopy;}
			set	{_dsForecastCopy = value;}
		}

		public int HierNodeRID
		{
			get	{return _hierNodeRid;}
			set	{_hierNodeRid = value;}
		}
		
		public int VersionRID
		{
			get	{return _versionRid;}
			set	{_versionRid = value;}
		}

		public int DateRangeRID
		{
			get	{return _dateRangeRid;}
			set	{_dateRangeRid = value;}
		}
		
		public ePlanType PlanType
		{
			get	{return _planType;}
			set	{_planType = value;}
		}
		public int StoreFilterRID
		{
			get	{return _storeFilterRid;}
			set	{_storeFilterRid = value;}
		}

        public bool MultiLevelInd
        {
            get { return _multiLevelInd; }
            set { _multiLevelInd = value; }
        }

        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        //public int FromLevel
        //{
        //    get { return _fromLevel; }
        //    set { _fromLevel = value; }
        //}

        //public int ToLevel
        //{
        //    get { return _toLevel; }
        //    set { _toLevel = value; }
        //}
        public eFromLevelsType FromLevelType
        {
            get { return _fromLevelType; }
            set { _fromLevelType = value; }
        }

        public int FromLevelOffset
        {
            get { return _fromLevelOffset; }
            set { _fromLevelOffset = value; }
        }

        public int FromLevelSequence
        {
            get { return _fromLevelSequence; }
            set { _fromLevelSequence = value; }
        }
        public eToLevelsType ToLevelType
        {
            get { return _toLevelType; }
            set { _toLevelType = value; }
        }

        public int ToLevelOffset
        {
            get { return _toLevelOffset; }
            set { _toLevelOffset = value; }
        }

        public int ToLevelSequence
        {
            get { return _toLevelSequence; }
            set { _toLevelSequence = value; }
        }
        // END Track #6107

		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        public bool CopyPreInitValues
        {
            get { return _copyPreInitValues; }
            set { _copyPreInitValues = value; }
        }
        // End Track #6347
		#endregion Properties


		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public OTSForecastCopyMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType methodType)
		//    : base(SAB,
		//    aMethodRID, methodType)
		public OTSForecastCopyMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType methodType, eProfileType profileType)
			: base(SAB,
			aMethodRID, methodType, profileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_groupLevelBasisDetail = new ProfileList(eProfileType.GroupLevelFunction);
			_SAB = SAB;

			if (base.Filled)
			{
				_ForecastCopyData = new OTSForecastCopyMethodData(aMethodRID, eChangeType.populate);
				_hierNodeRid = _ForecastCopyData.HierNodeRID;
				_versionRid = _ForecastCopyData.VersionRID;
				_dateRangeRid = _ForecastCopyData.CDR_RID;
				_planType  = (ePlanType)_ForecastCopyData.PlanType;
				_storeFilterRid = _ForecastCopyData.FilterRID;
                _multiLevelInd = _ForecastCopyData.MultiLevelInd;
                // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                //_fromLevel = _ForecastCopyData.FromLevel;
                //_toLevel = _ForecastCopyData.ToLevel;
                _fromLevelType = _ForecastCopyData.FromLevelType;
                _fromLevelOffset = _ForecastCopyData.FromLevelOffset;
                _fromLevelSequence = _ForecastCopyData.FromLevelSequence;
                _toLevelType = _ForecastCopyData.ToLevelType;
                _toLevelOffset = _ForecastCopyData.ToLevelOffset;
                _toLevelSequence = _ForecastCopyData.ToLevelSequence;
                // END Track #6107
				_overrideLowLevelRid = _ForecastCopyData.OverrideLowLevelRid;
                // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                _copyPreInitValues = _ForecastCopyData.CopyPreInitValues;
                // End Track #6347
				_dsForecastCopy = _ForecastCopyData.GetForecastCopyChildData();
			}
			else
			{	//Defaults
				_hierNodeRid = Include.NoRID;
				_versionRid = Include.NoRID;
				_dateRangeRid = Include.NoRID;
				_planType = (methodType == eMethodType.CopyChainForecast)? ePlanType.Chain: ePlanType.Store;
                _multiLevelInd = false;
                // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                //_fromLevel = 0;
                //_toLevel = 0;
                _fromLevelType = eFromLevelsType.None;
                _fromLevelOffset = 0;
                _fromLevelSequence = 0;
                _toLevelType = eToLevelsType.None;
                _toLevelOffset = 0;
                _toLevelSequence = 0;
                // END Track #6107
				_storeFilterRid = Include.UndefinedStoreFilter;
				_overrideLowLevelRid = Include.NoRID;
                // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                _copyPreInitValues = false;
                // End Track #6347
			}	
		}

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(_storeFilterRid))
            {
                return true;
            }

            if (IsStoreGroupUser(SG_RID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(_hierNodeRid))
            {
                return true;
            }

            foreach (DataRow dr in _dsForecastCopy.Tables["Basis"].Rows)
            {
                if (dr["HN_RID"] != DBNull.Value
                    && IsHierarchyNodeUser(Convert.ToInt32(dr["HN_RID"])))
                {
                    return true;
                }
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		/// <summary>
		/// Updates the OTS Forecast Copy method
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		//		new public void Update(TransactionData td)
		override public void Update(TransactionData td)
		{
			if (_ForecastCopyData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_ForecastCopyData = new OTSForecastCopyMethodData(td, base.Key);
			}

			_ForecastCopyData.HierNodeRID  = _hierNodeRid;
			_ForecastCopyData.VersionRID = _versionRid;
			_ForecastCopyData.CDR_RID = _dateRangeRid; 
			_ForecastCopyData.PlanType = Convert.ToInt32(_planType, CultureInfo.CurrentUICulture);
			_ForecastCopyData.FilterRID = _storeFilterRid;
            _ForecastCopyData.MultiLevelInd = _multiLevelInd;
            // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
            //_ForecastCopyData.ToLevel = _toLevel;
            //_ForecastCopyData.FromLevel = _fromLevel;
            _ForecastCopyData.FromLevelType = _fromLevelType;
            _ForecastCopyData.FromLevelOffset = _fromLevelOffset;
            _ForecastCopyData.FromLevelSequence = _fromLevelSequence;
            _ForecastCopyData.ToLevelType = _toLevelType;
            _ForecastCopyData.ToLevelOffset = _toLevelOffset;
            _ForecastCopyData.ToLevelSequence = _toLevelSequence;
            // END Track #6107
			_ForecastCopyData.OverrideLowLevelRid = _overrideLowLevelRid;
            // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
            _ForecastCopyData.CopyPreInitValues = _copyPreInitValues;
            // End Track #6347
			_ForecastCopyData.DSForecastCopy = _dsForecastCopy;

			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_ForecastCopyData.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
						_ForecastCopyData.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_ForecastCopyData.DeleteMethod(base.Key, td);
						base.Update(td);
						break;
				}
			}
			catch
			{
				throw;
			}

			finally
			{
			}
		}

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			ArrayList forecastingOverrideList = aApplicationTransaction.ForecastingOverrideList;
			try
			{
				if (forecastingOverrideList != null)
				{
					foreach (ForecastingOverride fo in forecastingOverrideList)
					{
						if (fo.HierarchyNodeRid != Include.NoRID)
						{
							this._hierNodeRid = fo.HierarchyNodeRid;
							this._nodeOverrideRid = fo.HierarchyNodeRid;
						}
						if (fo.ForecastVersionRid != Include.NoRID)
						{
							this._versionRid = fo.ForecastVersionRid;
							this._versionOverrideRid = fo.ForecastVersionRid;
						}

						if (_hierNodeRid == Include.NoRID)
						{
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanHierarchyNodeMissing, this.ToString());
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_PlanHierarchyNodeMissing,
								MIDText.GetText(eMIDTextCode.msg_pl_PlanHierarchyNodeMissing));
						}
						if (_versionRid == Include.NoRID)
						{
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanVersionMissing, this.ToString());
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_PlanVersionMissing,
								MIDText.GetText(eMIDTextCode.msg_pl_PlanVersionMissing));
						}
		
						this.ProcessAction(
							aApplicationTransaction.SAB,
							aApplicationTransaction,
							null,
							methodProfile,
							true,
							//Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
							//						Include.NoRID);
							aStoreFilter);
						//End Track #5188 - JScott - Method Filters not being over overriden from Workflow
					}
				}
				else
				{
					this.ProcessAction(
						aApplicationTransaction.SAB,
						aApplicationTransaction,
						null,
						methodProfile,
						true,
						//Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
						//					Include.NoRID);
						aStoreFilter);
					//End Track #5188 - JScott - Method Filters not being over overriden from Workflow
				}
			}
			catch
			{
				aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
			}
		}

		public override void ProcessAction(SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aWorkFlowStep, 
			Profile aProfile, bool WriteToDB, int aStoreFilterRID)
		{
			try
			{
				MIDTimer aTimer = new MIDTimer();
				_SAB = aSAB;
				this._applicationTransaction = aApplicationTransaction;
				_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

				// Begin MID Track 4858 - JSmith - Security changes
				// Need to check here incase part of workflow
				if (!AuthorizedToUpdate(_SAB.ApplicationServerSession, _SAB.ClientServerSession.UserRID))
				{
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, this.ToString());
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
					_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
					return;
				}
				// End MID Track 4858


				if (this.PlanType == ePlanType.Chain)
				{
					_infoMsg = "Starting OTS Forecast Chain Copy: " + this.Name;
					aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());

					aTimer.Start();
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    //ProcessCopyChain(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB, aStoreFilterRID);
                    if (CopyPreInitValues)
                    {
                        ProcessPreInitCopyChain(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB);
                    }
                    else
                    {
                        ProcessCopyChain(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB, aStoreFilterRID);
                    }
                    // End Track #6347
					aTimer.Stop();

					_infoMsg = "Completed OTS Forecast Chain Copy: " + this.Name + " " + 
						"Elasped time: " + aTimer.ElaspedTimeString;
					aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());
				}
				else
				{
					_infoMsg = "Starting OTS Forecast Store Copy: " + this.Name;
					aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());

					aTimer.Start();
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    //ProcessCopyStore(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB, aStoreFilterRID);
                    if (CopyPreInitValues)
                    {
                        ProcessPreInitCopyStore(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB);
                    }
                    else
                    {
                        ProcessCopyStore(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB, aStoreFilterRID);
                    }
                    // End Track #6347
					aTimer.Stop();

					_infoMsg = "Completed OTS Forecast Store Copy: " + this.Name + " " + 
						"Elasped time: " + aTimer.ElaspedTimeString;;
					aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());
				}

				_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;


			}
			catch (Exception err)
			{
				aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessAction: " + err.Message, this.ToString());
				aSAB.ApplicationServerSession.Audit.Log_Exception(err);
				_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
				throw;
			}
		}

		

		public void ProcessCopyChain(SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aWorkFlowStep, 
			Profile aProfile, bool WriteToDB, int aStoreFilterRID)
		{
			try
			{

                if (!MultiLevelInd)
                {
					_cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateChainPlanMaintCubeGroup();  

                    FillOpenParmForPlan();
                    FillOpenParmForBasis(Include.AllStoreGroupRID);
                    ((ChainPlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);	

        			((ChainPlanMaintCubeGroup)_cubeGroup).CopyBasisToChain();

					Save();

					((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
					_cubeGroup.Dispose();
					_cubeGroup = null;
                }
                else
                {
					// BEGIN Issue 5635 stodd
					LowLevelVersionOverrideProfileList overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
					LowLevelVersionOverrideProfileList tempOverrideList = null;
					HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
					HierarchyNodeProfile mainHnp1 = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);
                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    //int tmpFromLevelOffset = FromLevel - mainHnp1.NodeLevel;
                    //int tmpToLevelOffset = ToLevel - mainHnp1.NodeLevel;

                    //for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                    //{
                    //    tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                    //                        offset, Include.NoRID, true, false);
                    //    overrideList.AddRange(tempOverrideList.ArrayList);
                    //}
                    if (FromLevelType == eFromLevelsType.LevelOffset)
                    {
                        // Begin TT#4425 - JSmith - Copy Chain Method fails; but will copy if manually do a SAVE AS
                        //int tmpFromLevelOffset = FromLevelOffset - mainHnp1.NodeLevel;
                        //int tmpToLevelOffset = ToLevelOffset - mainHnp1.NodeLevel;
                        int tmpFromLevelOffset = FromLevelOffset;
                        int tmpToLevelOffset = ToLevelOffset;
                        // End TT#4425 - JSmith - Copy Chain Method fails; but will copy if manually do a SAVE AS

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    else
                    {
                        int tmpFromLevelOffset = FromLevelSequence - mainHnp1.NodeLevel;
                        int tmpToLevelOffset = ToLevelSequence - mainHnp1.NodeLevel;

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    // END Track #6107

                    //for (int i = FromLevel; i <= ToLevel; i++)
                    //{
                        //HierarchyNodeList lowLevels = SAB.HierarchyServerSession.GetDescendantDataByLevel(this.HierNodeRID, i, false, eNodeSelectType.NoVirtual);

						//===============================================================
						// Be design GetDescendantDataByLevel doesn't include the starting 
						// node if we happen to be processing
						// it's own node level. This adds it in.
						//HierarchyNodeProfile mainHnp = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);
						//if (mainHnp.NodeLevel == i)
						//{
						//	lowLevels.Add(mainHnp);
						//}


					foreach (LowLevelVersionOverrideProfile llvop in overrideList.ArrayList)
					{
						if (!llvop.Exclude)
						{
							_cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateChainPlanMaintCubeGroup();

							FillOpenParmForPlan(ePlanSessionType.ChainMultiLevel, ePlanSessionType.StoreMultiLevel, llvop.NodeProfile.Key);
							_nodeOverrideRid = llvop.NodeProfile.Key;
							FillOpenParmForBasis(Include.AllStoreGroupRID);
							((ChainPlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);	// Issue 4243 - stodd

							((ChainPlanMaintCubeGroup)_cubeGroup).CopyBasisToChain();

							Save();

							((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
							_cubeGroup.Dispose();
							_cubeGroup = null;
						}
                    }
                }
				// END Issue 5635 stodd
				// Begin MID Track #5399 - JSmith - Plan In Use error
				//				// Cleanup & dequeue
				//				((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
				//				// Begin MID Track #5210 - JSmith - Out of memory
				//				_cubeGroup.Dispose();
				//				_cubeGroup = null;
				//				// End MID Track #5210
				// End MID Track #5399

			}
			catch
			{
				throw;
			}
			// Begin MID Track #5399 - JSmith - Plan In Use error
			finally
			{
				if (_cubeGroup != null)
				{
					// Cleanup & dequeue
					((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
					_cubeGroup.Dispose();
					_cubeGroup = null;
				}
			}
			// End MID Track #5399
		}

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        private void ProcessPreInitCopyChain(SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aWorkFlowStep,
            Profile aProfile, bool WriteToDB)
        {
            ProfileList variableList;
            
            try
            {
                variableList = aApplicationTransaction.GetProfileList(eProfileType.Variable);

                if (!MultiLevelInd)
                {
                    FillOpenParmForPlan(ePlanSessionType.ChainSingleLevel, ePlanSessionType.None, HierNodeRID);
                    FillOpenParmForBasis(Include.AllStoreGroupRID);
                    ProcessPreInitCopyChainNode(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB,
                        variableList);
                }
                else
                {
                    LowLevelVersionOverrideProfileList overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                    LowLevelVersionOverrideProfileList tempOverrideList = null;
                    HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
                    HierarchyNodeProfile mainHnp1 = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);

                    if (FromLevelType == eFromLevelsType.LevelOffset)
                    {
                        // Begin TT#4425 - JSmith - Copy Chain Method fails; but will copy if manually do a SAVE AS
                        //int tmpFromLevelOffset = FromLevelOffset - mainHnp1.NodeLevel;
                        //int tmpToLevelOffset = ToLevelOffset - mainHnp1.NodeLevel;
                        int tmpFromLevelOffset = FromLevelOffset;
                        int tmpToLevelOffset = ToLevelOffset;
                        // End TT#4425 - JSmith - Copy Chain Method fails; but will copy if manually do a SAVE AS

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    else
                    {
                        int tmpFromLevelOffset = FromLevelSequence - mainHnp1.NodeLevel;
                        int tmpToLevelOffset = ToLevelSequence - mainHnp1.NodeLevel;

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }

                    foreach (LowLevelVersionOverrideProfile llvop in overrideList.ArrayList)
                    {
                        if (!llvop.Exclude)
                        {
                            FillOpenParmForPlan(ePlanSessionType.ChainMultiLevel, ePlanSessionType.StoreMultiLevel, llvop.NodeProfile.Key);
                            _nodeOverrideRid = llvop.NodeProfile.Key;
                            FillOpenParmForBasis(Include.AllStoreGroupRID);
                            ProcessPreInitCopyChainNode(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB,
                                                    variableList);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                
            }
        }

        private void ProcessPreInitCopyChainNode(SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aWorkFlowStep,
            Profile aProfile, bool WriteToDB, ProfileList aVariableList)
        {
            VariablesData variablesData = null;
            WeekProfile basisWeekProfile;
            BasisProfile basisProfile;
            BasisDetailProfile basisDetailProfile;
            ProfileList planWeeks, basisWeeks;
            int i = 0;

            try
            {
                // Begin TT#5124 - JSmith - Performance
                //variablesData = new VariablesData();
                variablesData = new VariablesData(aSAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
                // End TT#5124 - JSmith - Performance
                planWeeks = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(_openParms.DateRangeProfile, null);
                basisProfile = (BasisProfile)_openParms.BasisProfileList[0];
                basisDetailProfile = (BasisDetailProfile)basisProfile.BasisDetailProfileList[0];
                basisWeeks = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(basisDetailProfile.DateRangeProfile, null);
                variablesData.OpenUpdateConnection();

                variablesData.ChainWeek_Delete(
                     _openParms.ChainHLPlanProfile.NodeProfile.Key,
                     _openParms.ChainHLPlanProfile.VersionProfile.Key,
                     planWeeks);

                // if weeks are the same, do all at once
                if (((WeekProfile)planWeeks[0]).Key == ((WeekProfile)basisWeeks[0]).Key &&
                    planWeeks.Count == basisWeeks.Count)
                {
                    variablesData.ChainWeek_Copy(
                            aVariableList,
                            planWeeks,
                            _openParms.StoreHLPlanProfile.NodeProfile.Key,
                            _openParms.StoreHLPlanProfile.VersionProfile.Key,
                            basisDetailProfile.HierarchyNodeProfile.Key,
                            basisDetailProfile.VersionProfile.Key);
                }
                else
                {
                    foreach (WeekProfile weekProfile in planWeeks)
                    {
                        basisWeekProfile = (WeekProfile)basisWeeks[i];
                        variablesData.ChainWeek_Copy(
                            aVariableList,
                            _openParms.ChainHLPlanProfile.NodeProfile.Key,
                            _openParms.ChainHLPlanProfile.VersionProfile.Key,
                            weekProfile.Key,
                            basisDetailProfile.HierarchyNodeProfile.Key,
                            basisDetailProfile.VersionProfile.Key,
                            basisWeekProfile.Key);
                        ++i;
                    }
                }
                variablesData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (variablesData != null &&
                    variablesData.ConnectionIsOpen)
                {
                    variablesData.CloseUpdateConnection();
                }
            }
        }
        // End Track #6347

		public void ProcessCopyStore(SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aWorkFlowStep, 
			Profile aProfile, bool WriteToDB, int aStoreFilterRID)
		{
			try
			{

				//_cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateStorePlanMaintCubeGroup();
//Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow

				if (aStoreFilterRID != Include.NoRID && aStoreFilterRID != Include.UndefinedStoreFilter)
				{
					_storeFilterRid = aStoreFilterRID;
				}

//End Track #5188 - JScott - Method Filters not being over overriden from Workflow
                int totCellsCopied = 0;

				if (!MultiLevelInd)
				{
                    // Begin MID Track #5699 - KJohnson - Null Error
                    _cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateStorePlanMaintCubeGroup();
                    // End MID Track #5699

                    FillOpenParmForPlan();

					ArrayList sglList = GetUniqueSglList();

					bool firstSgl = true;
					foreach (int sglRid in sglList)
					{
						_openParms.BasisProfileList.Clear();
						FillOpenParmForBasis(sglRid);
						if (firstSgl)
						{
							((StorePlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);
							firstSgl = false;
						}
						else
						{
							((StorePlanMaintCubeGroup)_cubeGroup).RefreshStoreBasis(_openParms);
						}

                        //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sglRid); //_SAB.StoreServerSession.GetStoreGroupLevel(sglRid);
                        string levelName = StoreMgmt.StoreGroupLevel_GetName(sglRid); //TT#1517-MD -jsobek -Store Service Optimization
						ProfileList storeList = GetStoreList(sglRid);
						if (storeList.Count > 0)
						{
							int cellsCopied = ((StorePlanMaintCubeGroup)_cubeGroup).CopyBasisToStores(storeList);
							totCellsCopied += cellsCopied;
							string msg = MIDText.GetText(eMIDTextCode.msg_pl_NumberOfValuesCopied);
							msg = msg.Replace("{0}", cellsCopied.ToString(CultureInfo.CurrentUICulture));
                            msg = msg.Replace("{1}", levelName);
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

						}
						else
						{
							string msg = MIDText.GetText(eMIDTextCode.msg_pl_NoStoresToCopy);
                            msg = msg.Replace("{0}", levelName);
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
						}
					}

                    // Begin MID Track #5699 - KJohnson - Null Error
                    Save();

                    ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                    _cubeGroup.Dispose();
                    _cubeGroup = null;
                    // End MID Track #5699
                }
				else
				{
					// BEGIN Issue 5635 stodd
					LowLevelVersionOverrideProfileList overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
					LowLevelVersionOverrideProfileList tempOverrideList = null;
					HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
					HierarchyNodeProfile mainHnp1 = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);
                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    //int tmpFromLevelOffset = FromLevel - mainHnp1.NodeLevel;
                    //int tmpToLevelOffset = ToLevel - mainHnp1.NodeLevel;

                    //for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                    //{
                    //    tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                    //                        offset, Include.NoRID, true, false);
                    //    overrideList.AddRange(tempOverrideList.ArrayList);
                    //}

                    if (FromLevelType == eFromLevelsType.LevelOffset)
                    {
                        // Begin TT#2267 - JSmith - Error when trying to Copy Store Forecast
                        //int tmpFromLevelOffset = FromLevelOffset - mainHnp1.NodeLevel;
                        //int tmpToLevelOffset = ToLevelOffset - mainHnp1.NodeLevel;
                        int tmpFromLevelOffset = FromLevelOffset;
                        int tmpToLevelOffset = ToLevelOffset;
                        // End TT#2267

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    else
                    {
                        int tmpFromLevelOffset = FromLevelSequence - mainHnp1.NodeLevel;
                        int tmpToLevelOffset = ToLevelSequence - mainHnp1.NodeLevel;

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    // END Track #6107

					//for (int i = FromLevel; i <= ToLevel; i++)
					//{
						//HierarchyNodeList lowLevels = SAB.HierarchyServerSession.GetDescendantDataByLevel(this.HierNodeRID, i, false, eNodeSelectType.NoVirtual);

						//===============================================================
						// Be design GetDescendantDataByLevel doesn't include the starting 
						// node if we happen to be processing
						// it's own node level. This adds it in.
						//HierarchyNodeProfile mainHnp = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);
						//if (mainHnp.NodeLevel == i)
						//{
						//	lowLevels.Add(mainHnp);
						//}

					foreach (LowLevelVersionOverrideProfile llvop in overrideList.ArrayList)
					{
						if (!llvop.Exclude)
						{
							_cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateStorePlanMaintCubeGroup();

							FillOpenParmForPlan(ePlanSessionType.ChainMultiLevel, ePlanSessionType.StoreMultiLevel, llvop.NodeProfile.Key);
							_nodeOverrideRid = llvop.NodeProfile.Key;

							ArrayList sglList = GetUniqueSglList();

							bool firstSgl = true;
							foreach (int sglRid in sglList)
							{
								_openParms.BasisProfileList.Clear();
								FillOpenParmForBasis(sglRid);
								if (firstSgl)
								{
									((StorePlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);
									firstSgl = false;
								}
								else
								{
									((StorePlanMaintCubeGroup)_cubeGroup).RefreshStoreBasis(_openParms);
								}

                                //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sglRid); //_SAB.StoreServerSession.GetStoreGroupLevel(sglRid);
                                string levelName = StoreMgmt.StoreGroupLevel_GetName(sglRid); //TT#1517-MD -jsobek -Store Service Optimization
								ProfileList storeList = GetStoreList(sglRid);
								if (storeList.Count > 0)
								{
									int cellsCopied = ((StorePlanMaintCubeGroup)_cubeGroup).CopyBasisToStores(storeList);

									totCellsCopied += cellsCopied;
									string msg = MIDText.GetText(eMIDTextCode.msg_pl_NumberOfValuesCopied);
									msg = msg.Replace("{0}", cellsCopied.ToString(CultureInfo.CurrentUICulture));
                                    msg = msg.Replace("{1}", levelName);
									_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

								}
								else
								{
									string msg = MIDText.GetText(eMIDTextCode.msg_pl_NoStoresToCopy);
                                    msg = msg.Replace("{0}", levelName);
									_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
								}

							}
							// Begin MID Track #5699 - KJohnson - Null Error
                            if (sglList.Count > 0)
                            {
                                Save();
                            }
                            // End MID Track #5699

							((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
							_cubeGroup.Dispose();
							_cubeGroup = null;
						}
					}
				}
				// END Issue 5635 stodd

				string totMsg = MIDText.GetText(eMIDTextCode.msg_pl_TotalNumberOfValuesCopied);
				totMsg = totMsg.Replace("{0}",totCellsCopied.ToString(CultureInfo.CurrentUICulture));
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, totMsg, this.ToString());

				// Begin MID Track #5399 - JSmith - Plan In Use error
				//				// Cleanup & dequeue
				//				((PlanCubeGroup)_cubeGroup).CloseCubeGroup();	
				//				// Begin MID Track #5210 - JSmith - Out of memory
				//				_cubeGroup.Dispose();
				//				_cubeGroup = null;
				//				// End MID Track #5210
				// End MID Track #5399
			}
			catch
			{
				throw;
			}
			// Begin MID Track #5399 - JSmith - Plan In Use error
			finally
			{
				if (_cubeGroup != null)
				{
					// Cleanup & dequeue
					((PlanCubeGroup)_cubeGroup).CloseCubeGroup();	
					_cubeGroup.Dispose();
					_cubeGroup = null;
				}
			}
			// End MID Track #5399
		}

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        public void ProcessPreInitCopyStore(SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aWorkFlowStep,
            Profile aProfile, bool WriteToDB)
        {
            ProfileList variableList;

            try
            {
                variableList = aApplicationTransaction.GetProfileList(eProfileType.Variable);

                if (!MultiLevelInd)
                {
                    FillOpenParmForPlan();
                    ProcessPreInitCopyStoreNode_All(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB,
                            variableList);
                }
                else
                {
                    LowLevelVersionOverrideProfileList overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
					LowLevelVersionOverrideProfileList tempOverrideList = null;
					HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
					HierarchyNodeProfile mainHnp1 = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);
                    
                    if (FromLevelType == eFromLevelsType.LevelOffset)
                    {
                        // Begin TT#4425 - JSmith - Copy Chain Method fails; but will copy if manually do a SAVE AS
                        //int tmpFromLevelOffset = FromLevelOffset - mainHnp1.NodeLevel;
                        //int tmpToLevelOffset = ToLevelOffset - mainHnp1.NodeLevel;
                        int tmpFromLevelOffset = FromLevelOffset;
                        int tmpToLevelOffset = ToLevelOffset;
                        // End TT#4425 - JSmith - Copy Chain Method fails; but will copy if manually do a SAVE AS

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    else
                    {
                        int tmpFromLevelOffset = FromLevelSequence - mainHnp1.NodeLevel;
                        int tmpToLevelOffset = ToLevelSequence - mainHnp1.NodeLevel;

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                offset, Include.NoRID, true, false);
                            overrideList.AddRange(tempOverrideList.ArrayList);
                        }
                    }
                    
                    foreach (LowLevelVersionOverrideProfile llvop in overrideList.ArrayList)
					{
						if (!llvop.Exclude)
						{
                            FillOpenParmForPlan(ePlanSessionType.ChainMultiLevel, ePlanSessionType.StoreMultiLevel, llvop.NodeProfile.Key);
							_nodeOverrideRid = llvop.NodeProfile.Key;

							ProcessPreInitCopyStoreNode_All(aSAB, aApplicationTransaction, aWorkFlowStep, aProfile, WriteToDB,
                                            variableList);
						}
					}
				}
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }

        private void ProcessPreInitCopyStoreNode_All(SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aWorkFlowStep,
            Profile aProfile, bool WriteToDB, ProfileList aVariableList)
        {
            VariablesData variablesData = null;
            WeekProfile basisWeekProfile;
            BasisProfile basisProfile;
			BasisDetailProfile basisDetailProfile;
            ProfileList basisWeeks, planWeeks;
            int i = 0;

            try
            {
                // Begin TT#5124 - JSmith - Performance
                //variablesData = new VariablesData();
                variablesData = new VariablesData(aSAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
                // End TT#5124 - JSmith - Performance
                planWeeks = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(_openParms.DateRangeProfile, null);

                _openParms.BasisProfileList.Clear();
                FillOpenParmForBasis(Include.AllStoreGroupRID);
                basisProfile = (BasisProfile)_openParms.BasisProfileList[0];
                basisDetailProfile = (BasisDetailProfile)basisProfile.BasisDetailProfileList[0];
                basisWeeks = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(basisDetailProfile.DateRangeProfile, null);

                variablesData.OpenUpdateConnection();
                variablesData.StoreWeek_Delete(
                        _openParms.StoreHLPlanProfile.NodeProfile.Key,
                        _openParms.StoreHLPlanProfile.VersionProfile.Key,
                        planWeeks);
                //variablesData.CommitData();

                // if weeks are the same, do all at once
                if (((WeekProfile)planWeeks[0]).Key == ((WeekProfile)basisWeeks[0]).Key &&
                    planWeeks.Count == basisWeeks.Count)
                {
                    variablesData.StoreWeek_Copy(
                            aVariableList,
                            planWeeks,
                            _openParms.StoreHLPlanProfile.NodeProfile.Key,
                            _openParms.StoreHLPlanProfile.VersionProfile.Key,
                            basisDetailProfile.HierarchyNodeProfile.Key,
                            basisDetailProfile.VersionProfile.Key);
                }
                else
                {
                    i = 0;
                    foreach (WeekProfile weekProfile in planWeeks)
                    {
                        basisWeekProfile = (WeekProfile)basisWeeks[i];

                        variablesData.StoreWeek_Copy(
                            aVariableList,
                            _openParms.StoreHLPlanProfile.NodeProfile.Key,
                            _openParms.StoreHLPlanProfile.VersionProfile.Key,
                            weekProfile.Key,
                            basisDetailProfile.HierarchyNodeProfile.Key,
                            basisDetailProfile.VersionProfile.Key,
                            basisWeekProfile.Key);
                        ++i;
                    }
                }
                variablesData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (variablesData != null &&
                    variablesData.ConnectionIsOpen)
                {
                    variablesData.CloseUpdateConnection();
                }
            }
        }

        private void ProcessPreInitCopyStoreNode_BySet(SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aWorkFlowStep,
            Profile aProfile, bool WriteToDB, ProfileList aVariableList,
            int aSetRID)
        {
            VariablesData variablesData = null;
            WeekProfile basisWeekProfile;
            BasisProfile basisProfile;
            BasisDetailProfile basisDetailProfile;
            ProfileList basisWeeks, planWeeks, variables;
            int i, writeCount = 0, stRID, basisTimeID, planTimeID;
            DataTable dataTable;
            Hashtable values, locks, weeks;

            try
            {
                // Begin TT#5124 - JSmith - Performance
                //variablesData = new VariablesData();
                variablesData = new VariablesData(_SAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
                // End TT#5124 - JSmith - Performance
                values = new Hashtable();
                locks = new Hashtable();
                weeks = new Hashtable();
                variables = new ProfileList(eProfileType.Variable);
                planWeeks = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(_openParms.DateRangeProfile, null);

                //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(aSetRID); //_SAB.StoreServerSession.GetStoreGroupLevel(aSetRID);
                ProfileList storeList = GetStoreList(aSetRID);
                if (storeList.Count > 0)
                {
                    _openParms.BasisProfileList.Clear();
                    FillOpenParmForBasis(aSetRID);
                    basisProfile = (BasisProfile)_openParms.BasisProfileList[0];
                    basisDetailProfile = (BasisDetailProfile)basisProfile.BasisDetailProfileList[0];
                    basisWeeks = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(basisDetailProfile.DateRangeProfile, null);
                    i = 0;
                    foreach (WeekProfile planWeekProfile in planWeeks)
                    {
                        basisWeekProfile = (WeekProfile)basisWeeks[i];
                        weeks.Add(basisWeekProfile.Key, planWeekProfile.Key);
                        ++i;
                    }
                    foreach (VariableProfile varProf in aVariableList)
                    {
                        if (varProf.isDatabaseVariable(eVariableCategory.Store,
                            basisDetailProfile.VersionProfile.Key,
                            eCalendarDateType.Week))
                        {
                            variables.Add(varProf);
                        }
                    }

                    variablesData.OpenUpdateConnection();
                    variablesData.StoreWeek_Delete(
                        _openParms.StoreHLPlanProfile.NodeProfile.Key,
                        _openParms.StoreHLPlanProfile.VersionProfile.Key,
                        planWeeks,
                        storeList);
                    //variablesData.CommitData();

                    dataTable = variablesData.StoreWeek_Read(
                        basisDetailProfile.HierarchyNodeProfile.Key,
                        basisDetailProfile.VersionProfile.Key,
                        basisWeeks,
                        aVariableList);

                    if (dataTable.Rows.Count > 0)
                    {
                        variablesData.Variable_XMLInit();
                        foreach (System.Data.DataRow dataRow in dataTable.Rows)
                        {
                            stRID = Convert.ToInt32(dataRow["ST_RID"]);
                            if (storeList.FindKey(stRID) != null)
                            {
                                basisTimeID = Convert.ToInt32(dataRow["TIME_ID"]);
                                planTimeID = Convert.ToInt32(weeks[basisTimeID]);
                                values.Clear();
                                foreach (VariableProfile varProf in variables)
                                {
                                    values.Add(varProf, Convert.ToDouble(dataRow[varProf.DatabaseColumnName]));
                                }
                                variablesData.StoreWeek_Update_Insert(_openParms.StoreHLPlanProfile.NodeProfile.Key,
                                    planTimeID,
                                    _openParms.StoreHLPlanProfile.VersionProfile.Key,
                                    stRID,
                                    values,
                                    locks,
                                    false);
                                ++writeCount;
                                if (writeCount > MIDConnectionString.CommitLimit)
                                {
                                    variablesData.StoreWeek_XMLUpdate(_openParms.StoreHLPlanProfile.VersionProfile.Key, false);
                                    variablesData.Variable_XMLInit();
                                    writeCount = 0;
                                }
                            }
                        }

                        if (writeCount > 0)
                        {
                            variablesData.StoreWeek_XMLUpdate(_openParms.StoreHLPlanProfile.VersionProfile.Key, false);
                        }
                    }
                    variablesData.CommitData();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (variablesData != null &&
                    variablesData.ConnectionIsOpen)
                {
                    variablesData.CloseUpdateConnection();
                }
            }
        }
        // End Track #6347

		/// <summary>
		/// Gets a list of unique store group level Rids from the basis information.
		/// </summary>
		/// <returns></returns>
		private ArrayList GetUniqueSglList()
		{
			ArrayList sglList = new ArrayList();
			int maxRows = this._dsForecastCopy.Tables[1].Rows.Count;  //the basis detail table
			for (int row=0;row<maxRows;row++)
			{
				int sglRid = Convert.ToInt32(_dsForecastCopy.Tables[1].Rows[row]["SGL_RID"], CultureInfo.CurrentUICulture);
				if (!sglList.Contains(sglRid))
					sglList.Add(sglRid);
			}
			return sglList;
		}

		private ProfileList GetStoreList(int sglRid)
		{
			ProfileList storeList = null;
			//==================================================================================================
			// If a store filter is present, then the intersection of the store filter and store group level
			// is used to get the stores.
			// If there is no store filter, the stores in the store group level are used.
			//==================================================================================================
			if (StoreFilterRID != Include.NoRID)
			{
				ProfileList storeFilterList = null;
				ProfileList storeSglList = null;
				bool outdatedFilter = false;
				storeFilterList = _applicationTransaction.GetAllocationFilteredStoreList(_hierNodeRid, StoreFilterRID, ref outdatedFilter);
				// BEGIN Issue 5727 stodd
				if (outdatedFilter)
				{
					FilterData storeFilterData = new FilterData();
					string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
					msg = msg.Replace("{0}",storeFilterData.FilterGetName(StoreFilterRID));
					string suffix = ". Method " + this.Name + ". ";
					string auditMsg = msg + suffix;
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
				}
				// END Issue 5727
                storeSglList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(this.SG_RID, sglRid); //_SAB.StoreServerSession.GetStoresInGroup(this.SG_RID, sglRid);
				storeList = new ProfileList(eProfileType.Store);
				foreach (StoreProfile filterStore in storeFilterList.ArrayList)
				{
					if (storeSglList.Contains(filterStore.Key))
						storeList.Add(filterStore);
				}
			}
			else
			{
                storeList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(this.SG_RID, sglRid); //_SAB.StoreServerSession.GetStoresInGroup(this.SG_RID, sglRid);
			}
			return storeList;
		}


		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		/// <summary>
		/// Fills in the plan part of the CubeGroup open parms
		/// </summary>
		private void FillOpenParmForPlan()
        {
            FillOpenParmForPlan(ePlanSessionType.ChainSingleLevel, ePlanSessionType.StoreSingleLevel, this.HierNodeRID);
        }
		private void FillOpenParmForPlan(ePlanSessionType aChainPlanSessionType, ePlanSessionType aStorePlanSessionType, int aHierNodeRID)
		{
			if (this.PlanType == ePlanType.Chain)
				_openParms = new PlanOpenParms(ePlanSessionType.ChainSingleLevel, _computationsMode);
			else
				_openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _computationsMode);

			if (this.GlobalUserType == eGlobalUserType.User)
			{
				_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsUserCopyChain);
			}
			else
			{
				_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsGlobalCopyChain);
			}
			_openParms.FunctionSecurityProfile.SetAllowUpdate();

			HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(aHierNodeRID);
            hnp.ChainSecurityProfile = new HierarchyNodeSecurityProfile(aHierNodeRID);
//			hnp.ChainSecurityProfile.SetReadOnly();
            hnp.StoreSecurityProfile = new HierarchyNodeSecurityProfile(aHierNodeRID);
			//Begin Track #4457 - JSmith - Add forecast versions
			ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
			VersionProfile vp = fvpb.Build(VersionRID);
			//End Track #4457
			vp.StoreSecurity = new VersionSecurityProfile(this.VersionRID);
			vp.ChainSecurity = new VersionSecurityProfile(this.VersionRID);
			
			_openParms.StoreHLPlanProfile.VersionProfile = vp;
			_openParms.StoreHLPlanProfile.NodeProfile = hnp;
			_openParms.ChainHLPlanProfile.VersionProfile = vp;
			_openParms.ChainHLPlanProfile.NodeProfile = hnp;
//Begin Track #4018 - JScott - Calendar Serialization error in Forecast Balance Method
			_openParms.DateRangeProfile = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.DateRangeRID);
//End Track #4018 - JScott - Calendar Serialization error in Forecast Balance Method		
			_openParms.StoreGroupRID = this.SG_RID;
			_openParms.FilterRID = this.StoreFilterRID;
			_openParms.IneligibleStores = false; 
			_openParms.SimilarStores = true;

			if (_computationsMode != null)
			{
				_openParms.ComputationsMode = _computationsMode;
			}
			else
			{
				_openParms.ComputationsMode = _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
			}

			_openParms.LowLevelVersionDefault = vp;
            if (aChainPlanSessionType == ePlanSessionType.ChainMultiLevel ||
                aStorePlanSessionType == ePlanSessionType.StoreMultiLevel)
            {
                FillOpenParmsLowLevels(aHierNodeRID);
            }
		}

		private void FillOpenParmsLowLevels(int nodeRid)
		{
			try
			{
				_openParms.ClearLowLevelPlanProfileList();
				LowLevelVersionOverrideProfileList lowLeveOverrideList = GetLowLevelOverrideList(nodeRid);

				foreach (LowLevelVersionOverrideProfile lvop in lowLeveOverrideList)
				{
					if (!lvop.Exclude)
					{
						PlanProfile planProfile = new PlanProfile(lvop.Key);
						planProfile.NodeProfile = lvop.NodeProfile;
						planProfile.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Chain);
						planProfile.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Store);

						planProfile.IncludeExclude = eBasisIncludeExclude.Include;

						if (lvop.VersionIsOverridden)
						{
							planProfile.VersionProfile = lvop.VersionProfile;
						}
						else
						{
							planProfile.VersionProfile = _openParms.LowLevelVersionDefault;
						}
						planProfile.VersionProfile.ChainSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(planProfile.VersionProfile.Key, (int)eSecurityTypes.Chain);
						_openParms.AddLowLevelPlanProfile(planProfile);
					}
				}
			}
			catch
			{
			}
		}

		private LowLevelVersionOverrideProfileList GetLowLevelOverrideList(int nodeRid)
		{
			LowLevelVersionOverrideProfileList overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
			try
			{
				HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
				overrideList = hTran.GetOverrideList(_overrideLowLevelRid, nodeRid, this.VersionRID,
																1, Include.NoRID, true, false);
				return overrideList;
			}
			catch (Exception ex)
			{
				string msg = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Fills in the basis part of the CubeGroup open parms.
		/// </summary>
		private void FillOpenParmForBasis(int sglRid)
		{
			BasisProfile basisProfile;
			BasisDetailProfile basisDetailProfile;
			int bdpKey = 1;
			HierarchyNodeProfile hnp = null;
			HierarchyNodeProfile overrideHnp = null;

			if (_nodeOverrideRid != Include.NoRID)
			{
				overrideHnp = _SAB.HierarchyServerSession.GetNodeData(_nodeOverrideRid);
			}

			//=======================
			// Set up Basis Profile
			//=======================
			basisProfile = new BasisProfile(1, null, _openParms);
			basisProfile.BasisType = eTyLyType.NonTyLy;

			int maxRows = this._dsForecastCopy.Tables[1].Rows.Count;  //the basis detail table
			for (int row=0;row<maxRows;row++)
			{
				int basisSglRid = Convert.ToInt32(_dsForecastCopy.Tables[1].Rows[row]["SGL_RID"], CultureInfo.CurrentUICulture);
				if (basisSglRid == sglRid)
				{
					//=====================
					// Get Hierarchy Node
					//=====================
					int basisHierNodeRid = Include.NoRID;
					if (_dsForecastCopy.Tables[1].Rows[row]["HN_RID"] == DBNull.Value || MultiLevelInd)
					{
						if (_nodeOverrideRid != Include.NoRID)
						{
							basisHierNodeRid = _nodeOverrideRid;
							hnp = overrideHnp;
						}
						else
						{
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_BasisHierarchyNodeMissing, this.ToString());
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_BasisHierarchyNodeMissing,
								MIDText.GetText(eMIDTextCode.msg_pl_BasisHierarchyNodeMissing));
						}
					}
					else
					{
						basisHierNodeRid = Convert.ToInt32(_dsForecastCopy.Tables[1].Rows[row]["HN_RID"], CultureInfo.CurrentUICulture);
						hnp = _SAB.HierarchyServerSession.GetNodeData(basisHierNodeRid);
					}
						
					int basisVersionRid = Convert.ToInt32(_dsForecastCopy.Tables[1].Rows[row]["FV_RID"], CultureInfo.CurrentUICulture);
					int basisDateRangeRid = Convert.ToInt32(_dsForecastCopy.Tables[1].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);

					basisDetailProfile = new BasisDetailProfile(bdpKey++, _openParms);
					//Begin Track #4457 - JSmith - Add forecast versions
					ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
					basisDetailProfile.VersionProfile = fvpb.Build(basisVersionRid);
					//End Track #4457
					basisDetailProfile.HierarchyNodeProfile = hnp;
					DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basisDateRangeRid);
					basisDetailProfile.DateRangeProfile = drp;

					basisDetailProfile.IncludeExclude = (eBasisIncludeExclude)Convert.ToInt32(_dsForecastCopy.Tables[1].Rows[row]["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
					if (_dsForecastCopy.Tables[1].Rows[row]["WEIGHT"] == System.DBNull.Value)
						basisDetailProfile.Weight = 1;
					else
						basisDetailProfile.Weight = (float)Convert.ToDouble(_dsForecastCopy.Tables[1].Rows[row]["WEIGHT"], CultureInfo.CurrentUICulture);
					basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
				}
			}
			_openParms.BasisProfileList.Add(basisProfile);
		}

		private void Save()
		{
			try
			{
				PlanSaveParms planSaveParms = new PlanSaveParms();

				if (this.PlanType == ePlanType.Store)
				{
					planSaveParms.SaveStoreHighLevel = true;
					planSaveParms.StoreHighLevelNodeRID = _openParms.StoreHLPlanProfile.NodeProfile.Key;
					planSaveParms.StoreHighLevelVersionRID = _openParms.StoreHLPlanProfile.VersionProfile.Key;
					planSaveParms.StoreHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
				}

				if (this.PlanType == ePlanType.Chain)
				{
					planSaveParms.SaveChainHighLevel = true;
					planSaveParms.ChainHighLevelNodeRID = _openParms.ChainHLPlanProfile.NodeProfile.Key;
					planSaveParms.ChainHighLevelVersionRID = _openParms.ChainHLPlanProfile.VersionProfile.Key;
					planSaveParms.ChainHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
					planSaveParms.SaveHighLevelAllStoreAsChain = false;
				}

				_cubeGroup.SaveCubeGroup(planSaveParms);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
		{
			OTSForecastCopyMethod newOTSForecastCopyMethod = null;
			int maxRows;
			int basisDateRangeRid;
            // Begin Track #5912 - JSmith - Save As needs to clone custom override models
            OverrideLowLevelProfile ollp;
            int customUserRID;
            // End Track #5912

			try
			{
				newOTSForecastCopyMethod = (OTSForecastCopyMethod)this.MemberwiseClone();
				if (aCloneDateRanges &&
					DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					newOTSForecastCopyMethod.DateRangeRID = aSession.Calendar.GetDateRangeClone(DateRangeRID).Key;
				}
				else
				{
					newOTSForecastCopyMethod.DateRangeRID = DateRangeRID;
				}
				newOTSForecastCopyMethod.DSForecastCopy = DSForecastCopy.Copy();
				if (aCloneDateRanges)
				{
					maxRows = newOTSForecastCopyMethod.DSForecastCopy.Tables["Basis"].Rows.Count;  //the basis detail table
					for (int row=0;row<maxRows;row++)
					{
						basisDateRangeRid = Convert.ToInt32(DSForecastCopy.Tables["Basis"].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);
						newOTSForecastCopyMethod.DSForecastCopy.Tables["Basis"].Rows[row]["CDR_RID"] = aSession.Calendar.GetDateRangeClone(basisDateRangeRid).Key;
					}
				}
				newOTSForecastCopyMethod.DSForecastCopy.AcceptChanges();
				newOTSForecastCopyMethod.HierNodeRID = HierNodeRID;
				newOTSForecastCopyMethod.Method_Change_Type = eChangeType.none;
				newOTSForecastCopyMethod.Method_Description = Method_Description;
				newOTSForecastCopyMethod.MethodStatus = MethodStatus;
				newOTSForecastCopyMethod.Name = Name;
				newOTSForecastCopyMethod.PlanType = PlanType;
				newOTSForecastCopyMethod.SG_RID = SG_RID;
				newOTSForecastCopyMethod.StoreFilterRID = StoreFilterRID;
				newOTSForecastCopyMethod.User_RID = User_RID;
				newOTSForecastCopyMethod.VersionRID = VersionRID;
				newOTSForecastCopyMethod.Virtual_IND = Virtual_IND;
                newOTSForecastCopyMethod.Template_IND = Template_IND;
                // Begin Track #5912 - JSmith - Save As needs to clone custom override models
                if (aCloneCustomOverrideModels &&
                    CustomOLL_RID != Include.NoRID)
                {
                    ollp = new OverrideLowLevelProfile(CustomOLL_RID);
                    ollp.Key = Include.NoRID;
                    ollp.ModelChangeType = eChangeType.add;
                    customUserRID = Include.NoRID;
                    customUserRID = ollp.WriteProfile(ref customUserRID, SAB.ClientServerSession.UserRID);
                    if (CustomOLL_RID == OverrideLowLevelRid)
                    {
                        OverrideLowLevelRid = customUserRID;
                    }
                    newOTSForecastCopyMethod.CustomOLL_RID = customUserRID;
                }
                // End Track #5912
				newOTSForecastCopyMethod.OverrideLowLevelRid = OverrideLowLevelRid;

				return newOTSForecastCopyMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Returns a flag identifying if the user can update the data on the method.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aUserRID">
		/// The internal key of the user</param>
		/// <returns>
		/// A flag.
		/// </returns>
		override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
		{
			try
			{
				if (VersionRID == Include.NoRID ||
					HierNodeRID == Include.NoRID)
				{
					return false;
				}

				VersionSecurityProfile versionSecurity;
				if (_planType == ePlanType.Chain)
				{
					versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Chain);
				}
				else
				{
					versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Store);
				}
				if (!versionSecurity.AllowUpdate)
				{
					return false;
				}

				HierarchyNodeSecurityProfile hierNodeSecurity;
				if (_planType == ePlanType.Chain)
				{
					hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierNodeRID, (int)eSecurityTypes.Chain);
				}
				else
				{
					hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierNodeRID, (int)eSecurityTypes.Store);
				}
				if (!hierNodeSecurity.AllowUpdate)
				{
					return false;
				}
				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// End MID Track 4858

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;
            VersionSecurityProfile versionSecurity;

            DataTable dtBasis = DSForecastCopy.Tables["Basis"];
            int basisHnRID, basisFVRID;
            foreach (DataRow dr in dtBasis.Rows)
            {
                basisFVRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
                if (_planType == ePlanType.Store)
                {
                    versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(basisFVRID, (int)eSecurityTypes.Store);
                }
                else
                {
                    versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(basisFVRID, (int)eSecurityTypes.Chain);
                }
                if (!versionSecurity.AllowView)
                {
                    return false;
                }

                basisHnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                if (basisHnRID != Include.NoRID)
                {
                    if (_planType == ePlanType.Store)
                    {
                        hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisHnRID, (int)eSecurityTypes.Store);
                    }
                    else
                    {
                        hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisHnRID, (int)eSecurityTypes.Chain);
                    }
                    if (!hierNodeSecurity.AllowView)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.PlanType == ePlanType.Chain)
            {
                if (this.GlobalUserType == eGlobalUserType.Global)
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalCopyChain);
                }
                else
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserCopyChain);
                }
            }
            else
            {
                if (this.GlobalUserType == eGlobalUserType.Global)
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalCopyStore);
                }
                else
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserCopyStore);
                }
            }      

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROOverrideLowLevel overrideLowLevel = new ROOverrideLowLevel();
            overrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(OverrideLowLevelRid, SAB);

            ROLevelInformation fromLevel = new ROLevelInformation();
            fromLevel.LevelType = (eROLevelsType)FromLevelType;
            fromLevel.LevelOffset = FromLevelOffset;
            fromLevel.LevelSequence = FromLevelSequence;
            fromLevel.LevelValue = GetName.GetLevelName(
               levelType: (eROLevelsType)FromLevelType,
               levelSequence: FromLevelSequence,
               levelOffset: FromLevelOffset,
               SAB: SAB
               );
            ROLevelInformation toLevel = new ROLevelInformation();
            toLevel.LevelType = (eROLevelsType)ToLevelType;
            toLevel.LevelOffset = ToLevelOffset;
            toLevel.LevelSequence = ToLevelSequence;
            toLevel.LevelValue = GetName.GetLevelName(
               levelType: (eROLevelsType)ToLevelType,
               levelSequence: ToLevelSequence,
               levelOffset: ToLevelOffset,
               SAB: SAB
               );

            ROMethodCopyForecastProperties method;

            if (_dsForecastCopy == null
                || _dtGroupLevel == null
                || _dtBasis == null)
            {
                LoadBasis();
            }

            if (PlanType == ePlanType.Chain)
            {
                method = new ROMethodCopyChainForecastProperties(
                        method: GetName.GetMethod(method: this),
                        description: Method_Description,
                        userKey: User_RID,
                        merchandise: GetName.GetMerchandiseName(nodeRID: HierNodeRID, SAB: SAB),
                        version: GetName.GetVersion(versionRID: VersionRID, SAB: SAB),
                        timePeriod: GetName.GetCalendarDateRange(calendarDateRID: DateRangeRID, SAB: SAB),
                        multiLevel: MultiLevelInd,
                        planType: PlanType,
                        fromLevel: fromLevel,
                        toLevel: toLevel,
                        overrideLowLevel: overrideLowLevel,
                        copyPreInitValues: CopyPreInitValues,
                        emethodType: eMethodType.CopyChainForecast,
                        basisProfile: ConvertChainBasisDataToList(_dsForecastCopy),
                        isTemplate: Template_IND
                    );
            }
            else
            {
                // get default attribute and attribute set if not set
                if (SG_RID <= 0)
                {
                    SG_RID = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
                }
                if (_currentSglRid <= 0)
                {
                    ProfileList attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID, false);
                    if (attributeSetList.Count > 0)
                    {
                        _currentSglRid = attributeSetList[0].Key;
                    }
                }

                method = new ROMethodCopyStoreForecastProperties(
                        method: GetName.GetMethod(method: this),
                        description: Method_Description,
                        userKey: User_RID,
                        merchandise: GetName.GetMerchandiseName(nodeRID: HierNodeRID, SAB: SAB),
                        version: GetName.GetVersion(versionRID: VersionRID, SAB: SAB),
                        timePeriod: GetName.GetCalendarDateRange(calendarDateRID: DateRangeRID, SAB: SAB),
                        multiLevel: MultiLevelInd,
                        planType: PlanType,
                        storeFilter: GetName.GetStoreName(_storeFilterRid),
                        attribute: GetName.GetAttributeName(SG_RID),
                        attributeSet: GetName.GetAttributeSetName(_currentSglRid),
                        fromLevel: fromLevel,
                        toLevel: toLevel,
                        overrideLowLevel: overrideLowLevel,
                        copyPreInitValues: CopyPreInitValues,
                        emethodType: eMethodType.CopyStoreForecast,
                        attributeSetValues: GetAttributeSetValues(),
                        isTemplate: Template_IND
                    );
            }

            BuildVersionLists(method: method);

            if (_hierNodeRid > 0)
            {
                BuildLowLevelLists(method: method);
                
               
                if (method.FromLevel != null
                    && method.FromLevel.LevelType != eROLevelsType.None)
                {
                    // if different hierarchy types, update from level to 1st from entry
                    if (method.FromLevel != null
                        && !LevelTypesSame(
                            merchandiseType: method.FromLevelsType,
                            ROLevelType: method.FromLevel.LevelType)
                        )
                    {
                        if (method.FromLevels.Count == 0)
                        {
                            FromLevelType = eFromLevelsType.None;
                            FromLevelOffset = -1;
                            FromLevelSequence = -1;
                        }
                        else if (method.FromLevelsType == eMerchandiseType.HierarchyLevel)
                        {
                            FromLevelType = eFromLevelsType.HierarchyLevel;
                            FromLevelOffset = -1;
                            FromLevelSequence = method.FromLevels[0].Key;
                        }
                        else
                        {
                            FromLevelType = eFromLevelsType.LevelOffset;
                            FromLevelOffset = method.FromLevels[0].Key;
                            FromLevelSequence = -1;
                        }
                        method.FromLevel = new ROLevelInformation();
                        method.FromLevel.LevelType = (eROLevelsType)FromLevelType;
                        method.FromLevel.LevelOffset = FromLevelOffset;
                        method.FromLevel.LevelSequence = FromLevelSequence;
                        method.FromLevel.LevelValue = GetName.GetLevelName(
                            levelType: (eROLevelsType)FromLevelType,
                            levelSequence: FromLevelSequence,
                            levelOffset: FromLevelOffset,
                            SAB: SAB
                            );
                    }
                    if (method.ToLevel != null
                        && !LevelTypesSame(
                            merchandiseType: method.ToLevelsType,
                            ROLevelType: method.ToLevel.LevelType)
                        )
                    {
                        if (method.ToLevels.Count == 0)
                        {
                            ToLevelType = eToLevelsType.None;
                            ToLevelOffset = -1;
                            ToLevelSequence = -1;
                        }
                        else if (method.ToLevelsType == eMerchandiseType.HierarchyLevel)
                        {
                            ToLevelType = eToLevelsType.HierarchyLevel;
                            ToLevelOffset = -1;
                            ToLevelSequence = method.ToLevels[0].Key;
                        }
                        else
                        {
                            ToLevelType = eToLevelsType.LevelOffset;
                            ToLevelOffset = method.ToLevels[0].Key;
                            ToLevelSequence = -1;
                        }
                        method.ToLevel = new ROLevelInformation();
                        method.ToLevel.LevelType = (eROLevelsType)ToLevelType;
                        method.ToLevel.LevelOffset = ToLevelOffset;
                        method.ToLevel.LevelSequence = ToLevelSequence;
                        method.ToLevel.LevelValue = GetName.GetLevelName(
                           levelType: (eROLevelsType)ToLevelType,
                           levelSequence: ToLevelSequence,
                           levelOffset: ToLevelOffset,
                           SAB: SAB
                           );
                    }

                    // remove entries in to level list that are before the selected from level
                    int toOffset = -1;
                    foreach (KeyValuePair<int, string> level in method.FromLevels)
                    {
                        ++toOffset;
                        if (method.FromLevel.LevelType == eROLevelsType.HierarchyLevel)
                        {
                            if (method.FromLevel.LevelSequence == level.Key)
                            {
                                break;
                            }
                        }
                        else if (method.FromLevel.LevelType == eROLevelsType.LevelOffset)
                        {
                            if (method.FromLevel.LevelOffset == level.Key)
                            {
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < toOffset; i++)
                    {
                        method.ToLevels.RemoveAt(0);
                    }
                    // set to first level if no longer in the list
                    if (method.ToLevel != null
                        && method.ToLevel.LevelType != eROLevelsType.None)
                    {
                        bool foundToLevel = false;
                        foreach (KeyValuePair<int, string> level in method.ToLevels)
                        {
                            if (method.ToLevel.LevelType == eROLevelsType.HierarchyLevel)
                            {
                                if (method.ToLevel.LevelSequence == level.Key)
                                {
                                    foundToLevel = true;
                                    break;
                                }
                            }
                            else if (method.ToLevel.LevelType == eROLevelsType.LevelOffset)
                            {
                                if (method.ToLevel.LevelOffset == level.Key)
                                {
                                    foundToLevel = true;
                                    break;
                                }
                            }
                        }
                        if (!foundToLevel)  // set to the first entry
                        {
                            if (method.ToLevel.LevelType == eROLevelsType.HierarchyLevel)
                            {
                                method.ToLevel.LevelSequence = method.ToLevels[0].Key;
                            }
                            else if (method.ToLevel.LevelType == eROLevelsType.LevelOffset)
                            {
                                method.ToLevel.LevelOffset = method.ToLevels[0].Key;
                            }
                            method.ToLevel.LevelValue = GetName.GetLevelName(
                                           levelType: method.ToLevel.LevelType,
                                           levelSequence: method.ToLevel.LevelSequence,
                                           levelOffset: method.ToLevel.LevelOffset,
                                           SAB: SAB
                                           );
                        }
                    }
                }
            }

            return method;
        }

        private bool LevelTypesSame(eMerchandiseType merchandiseType, eROLevelsType ROLevelType)
        {
            if (merchandiseType == eMerchandiseType.HierarchyLevel
                && ROLevelType == eROLevelsType.HierarchyLevel)
            {
                return true;
            }

            if (merchandiseType == eMerchandiseType.LevelOffset
                && ROLevelType == eROLevelsType.LevelOffset)
            {
                return true;
            }

            return false;
        }

        private void BuildVersionLists(ROMethodCopyForecastProperties method)
        {
            ProfileList versionList;
            if (PlanType == ePlanType.Chain)
            {
                versionList = GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain, false, _versionRid, true);
            }
            else
            {
                versionList = GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, false, _versionRid, true);
            }

            foreach (VersionProfile versionProfile in versionList)
            {
                method.Versions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

            if (PlanType == ePlanType.Chain)
            {
                versionList = GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain, false, Include.NoRID);
            }
            else
            {
                versionList = GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store, false, Include.NoRID);
            }

            foreach (VersionProfile versionProfile in versionList)
            {
                method.BasisVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }
        }

        private void BuildLowLevelLists(ROMethodCopyForecastProperties method)
        {
            eMerchandiseType merchandiseType;
            int homeHierarchyKey;
            List<HierarchyLevelComboObject> levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: _hierNodeRid,
                includeHomeLevel: true,
                includeLowestLevel: false,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            method.FromLevelsType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    method.FromLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    method.FromLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }

            levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: _hierNodeRid,
                includeHomeLevel: false,
                includeLowestLevel: true,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            method.ToLevelsType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    method.ToLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    method.ToLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }
        }

        private ROMethodCopyStoreForecastAttributeSetProperties GetAttributeSetValues()
        {
            return new ROMethodCopyStoreForecastAttributeSetProperties(
                attributeSet: GetName.GetAttributeSetName(_currentSglRid),
                basisProfile: ConvertStoreBasisDataToList(_dsForecastCopy)
                );
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodCopyForecastProperties rOMethodCopyForecastProperties = (ROMethodCopyForecastProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                _hierNodeRid = rOMethodCopyForecastProperties.Merchandise.Key;
                _versionRid = rOMethodCopyForecastProperties.Version.Key;
                _dateRangeRid = rOMethodCopyForecastProperties.TimePeriod.Key;
                _multiLevelInd = rOMethodCopyForecastProperties.MultiLevel;
                _planType = rOMethodCopyForecastProperties.PlanType;
                

                _fromLevelType = (eFromLevelsType)rOMethodCopyForecastProperties.FromLevel.LevelType;
                _fromLevelOffset = rOMethodCopyForecastProperties.FromLevel.LevelOffset;
                _fromLevelSequence = rOMethodCopyForecastProperties.FromLevel.LevelSequence;

                _toLevelType = (eToLevelsType)rOMethodCopyForecastProperties.ToLevel.LevelType;
                _toLevelOffset = rOMethodCopyForecastProperties.ToLevel.LevelOffset;
                _toLevelSequence = rOMethodCopyForecastProperties.ToLevel.LevelSequence;

                _overrideLowLevelRid = rOMethodCopyForecastProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                _copyPreInitValues = rOMethodCopyForecastProperties.CopyPreInitValues;

                if (_dsForecastCopy == null
                    || _dtGroupLevel == null
                    || _dtBasis == null)
                {
                    LoadBasis();
                }

                if (rOMethodCopyForecastProperties.MethodType == eMethodType.CopyChainForecast)
                {
                    SetChainData((ROMethodCopyChainForecastProperties)rOMethodCopyForecastProperties);
                    SaveBasisForSetValues(Include.AllStoreGroupRID);
                }
                else
                {
                    SetStoreData((ROMethodCopyStoreForecastProperties)rOMethodCopyForecastProperties);
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SetChainData(ROMethodCopyChainForecastProperties rOMethodCopyChainForecastProperties)
        {
            _dsForecastCopy = ConvertChainBasisListDataset(rOMethodCopyChainForecastProperties.BasisProfiles);
            return true;
        }

        private bool SetStoreData(ROMethodCopyStoreForecastProperties rOMethodCopyStoreForecastProperties)
        {
            // modify attribute and attribute set values if changed
            if (SG_RID != rOMethodCopyStoreForecastProperties.Attribute.Key)
            {
                _dsForecastCopy.Tables["GroupLevel"].Rows.Clear();
                _dsForecastCopy.Tables["Basis"].Rows.Clear();
                ProfileList attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(rOMethodCopyStoreForecastProperties.Attribute.Key, false);
                if (attributeSetList.Count > 0)
                {
                    for (int i = 0; i < attributeSetList.Count; i++)
                    {
                        DataRow dataRow = _dsForecastCopy.Tables["GroupLevel"].NewRow();
                        dataRow["SGL_RID"] = attributeSetList[i].Key;
                        _dsForecastCopy.Tables["Basis"].Rows.Add(dataRow);
                    }
                }
            }

            // update basis values before updating attribute and set references
            _dsForecastCopy = ConvertStoreBasisListDataset(rOMethodCopyStoreForecastProperties.AttributeSetValues.BasisProfiles);
            SG_RID = rOMethodCopyStoreForecastProperties.Attribute.Key;
            _currentSglRid = rOMethodCopyStoreForecastProperties.AttributeSet.Key;
            _storeFilterRid = rOMethodCopyStoreForecastProperties.StoreFilter.Key;
            
            return true;
        }

        private List<ROBasisDetailProfile> ConvertChainBasisDataToList(DataSet dsBasis)
        {
            DataTable dtBasisDetails = dsBasis.Tables["Basis"];
            KeyValuePair<int, string> workKVP;
            //int basisDtlCtr = 0;
            List<ROBasisDetailProfile> basisDetailProfiles = new List<ROBasisDetailProfile>();

            for (int basisDtlCtr = 0; basisDtlCtr < dtBasisDetails.Rows.Count; basisDtlCtr++)
            {

                //int iBasisId = Convert.ToInt32(dtBasis.Rows[basisDtlRowCtr]["SGL_RID"].ToString());
                int iBasisId = 0;

                int iMerchandiseId = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["HN_RID"].ToString());
                // if Multi-Level, use the name of the plan merchandise in the basis
                if (MultiLevelInd)
                {
                    iMerchandiseId = HierNodeRID;
                }

                workKVP = GetName.GetMerchandiseName(iMerchandiseId, SAB);
                string sMerchandise = workKVP.Value;
                int iVersionId = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["FV_RID"].ToString());
                workKVP = GetName.GetVersion(iVersionId, SAB);
                string sVersion = workKVP.Value;
                int iDateRangeID = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["CDR_RID"].ToString());
                if (Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["CDR_RID"].ToString()) != Include.UndefinedCalendarDateRange)
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["CDR_RID"].ToString()));
                }
                else
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, SAB.ClientServerSession.Calendar.CurrentDate);
                }
                string sDateRange = workKVP.Value;
                string sPicture = string.Empty;
                float fWeight = float.Parse(dtBasisDetails.Rows[basisDtlCtr]["Weight"] == DBNull.Value ? "0" : dtBasisDetails.Rows[basisDtlCtr]["Weight"].ToString());
                bool bIsIncluded = false;
                if (Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture) == (int)eBasisIncludeExclude.Include)
                {
                    bIsIncluded = true;
                }

                string sIncludeButton = dtBasisDetails.Rows[basisDtlCtr]["IncludeButton"].ToString();
                ROBasisDetailProfile basisDetailProfile = new ROBasisDetailProfile(iBasisId, iMerchandiseId, sMerchandise, iVersionId, sVersion,
                    iDateRangeID, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton);
                basisDetailProfiles.Add(basisDetailProfile);
                //++basisDtlCtr;
            }
            return basisDetailProfiles;
        }

        private List<ROBasisDetailProfile> ConvertStoreBasisDataToList(DataSet dsBasis)
        {
            DataTable dtBasisDetails = dsBasis.Tables["Basis"];
            KeyValuePair<int, string> workKVP;
            //int basisDtlCtr = 0;
            List<ROBasisDetailProfile> basisDetailProfiles = new List<ROBasisDetailProfile>();

            for (int basisDtlCtr = 0; basisDtlCtr < dtBasisDetails.Rows.Count; basisDtlCtr++)
            {
                if (Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["SGL_RID"]) != _currentSglRid)
                {
                    continue;
                }
                //int iBasisId = Convert.ToInt32(dtBasis.Rows[basisDtlRowCtr]["SGL_RID"].ToString());
                int iBasisId = 0;

                int iMerchandiseId = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["HN_RID"].ToString());
                workKVP = GetName.GetMerchandiseName(iMerchandiseId, SAB);
                string sMerchandise = workKVP.Value;
                int iVersionId = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["FV_RID"].ToString());
                workKVP = GetName.GetVersion(iVersionId, SAB);
                string sVersion = workKVP.Value;
                int iDateRangeID = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["CDR_RID"].ToString());
                if (Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["CDR_RID"].ToString()) != Include.UndefinedCalendarDateRange)
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["CDR_RID"].ToString()));
                }
                else
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, SAB.ClientServerSession.Calendar.CurrentDate);
                }
                string sDateRange = workKVP.Value;
                string sPicture = string.Empty;
                float fWeight = float.Parse(dtBasisDetails.Rows[basisDtlCtr]["Weight"] == DBNull.Value ? "0" : dtBasisDetails.Rows[basisDtlCtr]["Weight"].ToString());
                bool bIsIncluded = false;
                if (Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture) == (int)eBasisIncludeExclude.Include)
                {
                    bIsIncluded = true;
                }

                string sIncludeButton = dtBasisDetails.Rows[basisDtlCtr]["IncludeButton"].ToString();
                ROBasisDetailProfile basisDetailProfile = new ROBasisDetailProfile(iBasisId, iMerchandiseId, sMerchandise, iVersionId, sVersion,
                    iDateRangeID, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton);
                basisDetailProfiles.Add(basisDetailProfile);
                //++basisDtlCtr;
            }
            return basisDetailProfiles;
        }

        private DataSet ConvertChainBasisListDataset(List<ROBasisDetailProfile> basisProfiles)
        {
            // clear and rebuild all rows
            DataTable dtBasisDetails = _dsForecastCopy.Tables["Basis"];
            dtBasisDetails.Rows.Clear();
            int sequence = 0;

            foreach (var basisDetailsProfile in basisProfiles)
            {
                DataRow rowDtlProf = dtBasisDetails.NewRow();
                rowDtlProf["SGL_RID"] = 1;
                rowDtlProf["DETAIL_SEQ"] = sequence;
                rowDtlProf["Merchandise"] = basisDetailsProfile.Merchandise;
                rowDtlProf["HN_RID"] = basisDetailsProfile.MerchandiseId;
                rowDtlProf["FV_RID"] = basisDetailsProfile.VersionId;
                rowDtlProf["DateRange"] = basisDetailsProfile.DateRange;
                rowDtlProf["CDR_RID"] = basisDetailsProfile.DateRangeId;
                rowDtlProf["WEIGHT"] = basisDetailsProfile.Weight;
                rowDtlProf["INCLUDE_EXCLUDE"] = basisDetailsProfile.IsIncluded ? (int)eBasisIncludeExclude.Include : (int)eBasisIncludeExclude.Exclude;
                rowDtlProf["IncludeButton"] = basisDetailsProfile.IncludeButton;
                dtBasisDetails.Rows.Add(rowDtlProf);
                sequence++;
            }

            return _dsForecastCopy;
        }

        private DataSet ConvertStoreBasisListDataset(List<ROBasisDetailProfile> basisProfiles)
        {
            // delete rows for the current set and rebuild the rows
            DataTable dtBasisDetails = _dsForecastCopy.Tables["Basis"];

            string selectString = "SGL_RID=" + _currentSglRid;
            if (dtBasisDetails != null)
            {
                DataRow[] basisDataRows = dtBasisDetails.Select(selectString);
                foreach (var taskDataRow in basisDataRows)
                {
                    taskDataRow.Delete();
                }
                dtBasisDetails.AcceptChanges();
            }
            int sequence = 0;

            foreach (var basisDetailsProfile in basisProfiles)
            {
                DataRow rowDtlProf = dtBasisDetails.NewRow();
                rowDtlProf["SGL_RID"] = _currentSglRid;
                rowDtlProf["DETAIL_SEQ"] = sequence;
                rowDtlProf["Merchandise"] = basisDetailsProfile.Merchandise;
                rowDtlProf["HN_RID"] = basisDetailsProfile.MerchandiseId;
                rowDtlProf["FV_RID"] = basisDetailsProfile.VersionId;
                rowDtlProf["DateRange"] = basisDetailsProfile.DateRange;
                rowDtlProf["CDR_RID"] = basisDetailsProfile.DateRangeId;
                rowDtlProf["WEIGHT"] = basisDetailsProfile.Weight;
                rowDtlProf["INCLUDE_EXCLUDE"] = basisDetailsProfile.IsIncluded ? (int)eBasisIncludeExclude.Include : (int)eBasisIncludeExclude.Exclude;
                rowDtlProf["IncludeButton"] = basisDetailsProfile.IncludeButton;
                dtBasisDetails.Rows.Add(rowDtlProf);
                sequence++;
            }

            return _dsForecastCopy;
        }

        private void LoadBasis()
        {
                _dtGroupLevel = MIDEnvironment.CreateDataTable("GroupLevel");
                _dtGroupLevel.Columns.Add("SGL_RID", System.Type.GetType("System.Int32"));

                _dtBasis = MIDEnvironment.CreateDataTable("Basis");

                _dtBasis.Columns.Add("SGL_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("Merchandise", System.Type.GetType("System.String"));
                _dtBasis.Columns.Add("HN_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("DateRange", System.Type.GetType("System.String"));
                _dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Decimal"));
                _dtBasis.Columns.Add("INCLUDE_EXCLUDE", System.Type.GetType("System.Int32")); //this column will be hidden. We'll use the buttons column for display.
                _dtBasis.Columns.Add("IncludeButton", System.Type.GetType("System.String")); //button column for include/exclude

                if (_dsForecastCopy != null)
                {
                    _dtGroupLevel = _dsForecastCopy.Tables["GroupLevel"];
                    _dtBasis = _dsForecastCopy.Tables["Basis"];
                }
                else
                {
                    _dsForecastCopy = MIDEnvironment.CreateDataSet();
                    _dsForecastCopy.Tables.Add(_dtGroupLevel);
                    _dsForecastCopy.Tables.Add(_dtBasis);
                }

                _dtBasis.Columns["WEIGHT"].DefaultValue = 1;

        }

        private void SaveBasisForSetValues(int aSetValue)
        {
            DataRow setRow = null;
            foreach (DataRow row in _dtGroupLevel.Rows)
            {
                if ((int)row["SGL_RID"] == aSetValue)
                {
                    setRow = row;
                    break;
                }
            }
            if (setRow == null)
            {
                if (_dtBasis.DefaultView.Count > 0)
                {
                    setRow = _dtGroupLevel.NewRow();
                    _dtGroupLevel.Rows.Add(setRow);
                    setRow["SGL_RID"] = aSetValue;
                    _dtGroupLevel.AcceptChanges();
                }
            }
            else if (_dtBasis.DefaultView.Count == 0)
            {
                _dtGroupLevel.Rows.Remove(setRow);
                _dtGroupLevel.AcceptChanges();
            }
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }



    /// <summary>
    /// Summary description for OTSPlanProfile.
    /// </summary>
    public class ForecastCopyGroupLevelProfile:Profile
	{
		

		public ForecastCopyGroupLevelProfile(SessionAddressBlock aSAB, int aKey)
			: base(aKey)
		{
		
		}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.OTSPlan;
			}
		}
	}



}
