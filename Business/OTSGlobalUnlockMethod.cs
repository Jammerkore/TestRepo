using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
    /// Summary description for OTSGlobalUnlockMethod.
	/// </summary>
	public class OTSGlobalUnlockMethod : OTSPlanBaseMethod
	{
		private OTSGlobalUnlockMethodData	_globalUnlockData;
		private DataSet _dsGlobalUnlock;
		private DataTable _dtLowerLevels;
        private int _hierNodeRid;
        private int _versionRid;
		private int _dateRangeRid;
        private string _lastProcessedDateTime;
        private string _lastProcessedUser;
        private eSpreadOption _spreadOption;
        private ArrayList _sglRidList;
        private bool _multiLevel;
        private bool _stores;
        private int _filter;
        private bool _chain;
        private eFromLevelsType _fromLevelType;
		private int _fromLevelOffset;
		private int _fromLevelSequence;
        private eToLevelsType _toLevelType;
        private int _toLevelOffset;
        private int _toLevelSequence;
        //		private ePlanType _planType; //Chain or Store
//		private int _storeFilterRid;
		private ProfileList _groupLevelBasisDetail;
		private ProfileList _versionProfList;
		private ProfileList _lowlevelVersionOverrideList;

		private SessionAddressBlock _SAB;
		private string _infoMsg;
		private PlanCubeGroup _cubeGroup;
		private PlanOpenParms _openParms;
		private ApplicationSessionTransaction _applicationTransaction;
		private string _computationsMode = "Default";
		private int _nodeOverrideRid = Include.NoRID;
		private int _versionOverrideRid = Include.NoRID;

		#region Properties
		/// <summary>
		/// Gets the ProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.OTSPlan;
			}
		}
		
		public DataSet DSGlobalUnlock
		{
			get	{return _dsGlobalUnlock;}
			set	{_dsGlobalUnlock = value;}
		}

		public DataTable DTLowerLevels
		{
			get	{return _dtLowerLevels;}
			set	{_dtLowerLevels = value;}
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

		public string LastProcessedDateTime
		{
            get { return _lastProcessedDateTime; }
            set { _lastProcessedDateTime = value; }
		}

        public string LastProcessedUser
        {
            get { return _lastProcessedUser; }
            set { _lastProcessedUser = value; }
        }

		public int DateRangeRID
		{
			get	{return _dateRangeRid;}
			set	{_dateRangeRid = value;}
		}

		public eSpreadOption SpreadOption
		{
			get	{return _spreadOption;}
			set	{_spreadOption = value;}
		}

        public bool MultiLevel
		{
            get {return _multiLevel; }
            set {_multiLevel = value; }
		}

        public ArrayList SGL_RID_List
        {
            get { return _sglRidList; }
            set { _sglRidList = value; }
        }

        public bool Stores
        {
            get {return _stores; }
            set {_stores = value; }
        }

        public bool Chain
        {
            get {return _chain; }
            set {_chain = value; }
        }
		
		public eFromLevelsType FromLevelType
		{
			get	{return _fromLevelType;}
			set	{_fromLevelType = value;}
		} 

		public int FromLevelOffset
		{
			get	{return _fromLevelOffset;}
			set	{_fromLevelOffset = value;}
		}

		public int FromLevelSequence
		{
			get	{return _fromLevelSequence;}
			set	{_fromLevelSequence = value;}
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

        public int Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }		

		#endregion Properties


        public OTSGlobalUnlockMethod(SessionAddressBlock SAB, int aMethodRID)
            : base(SAB,
            aMethodRID, eMethodType.GlobalUnlock)
		{
			_groupLevelBasisDetail = new ProfileList(eProfileType.GroupLevelFunction);
			_SAB = SAB;

			//_monitor = _SAB.MonitorForecastAppSetting;
			//_monitorFilePath = _SAB.GetMonitorFilePathAppSetting();

			if (base.Filled)
			{
                _globalUnlockData = new OTSGlobalUnlockMethodData(aMethodRID, eChangeType.populate);
                _hierNodeRid = _globalUnlockData.HierNodeRID;
                _versionRid = _globalUnlockData.VersionRID;
                _dateRangeRid = _globalUnlockData.CDR_RID;
                _sglRidList = _globalUnlockData.SGL_RID_List;
                _spreadOption = _globalUnlockData.SpreadOption;
                _multiLevel = _globalUnlockData.MultiLevel;
                _stores = _globalUnlockData.Stores;
                _chain = _globalUnlockData.Chain;
                _filter = _globalUnlockData.Filter;
                _fromLevelType = _globalUnlockData.FromLevelType;
                _fromLevelOffset = _globalUnlockData.FromLevelOffset;
                _fromLevelSequence = _globalUnlockData.FromLevelSequence;
                _toLevelType = _globalUnlockData.ToLevelType;
                _toLevelOffset = _globalUnlockData.ToLevelOffset;
                _toLevelSequence = _globalUnlockData.ToLevelSequence;
            }
			else
			{	//Defaults
				_hierNodeRid = Include.NoRID;
				_versionRid = Include.NoRID;
				_dateRangeRid = Include.NoRID;
                _sglRidList = null;
                _spreadOption = eSpreadOption.Plan;
				_multiLevel = false;
				_stores = false;
				_chain = false;
                _filter = Include.UndefinedStoreFilter;
				_fromLevelType = eFromLevelsType.None;
				_fromLevelOffset = Include.NoRID;
				_fromLevelSequence = Include.NoRID;
                _toLevelType = eToLevelsType.None;
                _toLevelOffset = Include.NoRID;
                _toLevelSequence = Include.NoRID;
            }	
		}

		/// <summary>
        /// Updates the OTS Global Unlock Copy method
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		//		new public void Update(TransactionData td)
		override public void Update(TransactionData td)
		{
            if (_globalUnlockData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
                _globalUnlockData = new OTSGlobalUnlockMethodData(td, base.Key);
			}

            _globalUnlockData.HierNodeRID = _hierNodeRid;
            _globalUnlockData.VersionRID = _versionRid;
            _globalUnlockData.CDR_RID = _dateRangeRid;
            _globalUnlockData.SGL_RID_List = _sglRidList;
            _globalUnlockData.SpreadOption = _spreadOption;
            _globalUnlockData.MultiLevel = _multiLevel;
            _globalUnlockData.Stores = _stores;
            _globalUnlockData.Chain = _chain;
            _globalUnlockData.Filter = _filter;
            _globalUnlockData.FromLevelType = _fromLevelType;
            _globalUnlockData.FromLevelOffset = _fromLevelOffset;
            _globalUnlockData.FromLevelSequence = _fromLevelSequence;
            _globalUnlockData.ToLevelType = _toLevelType;
            _globalUnlockData.ToLevelOffset = _toLevelOffset;
            _globalUnlockData.ToLevelSequence = _toLevelSequence;

			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
                        _globalUnlockData.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
                        _globalUnlockData.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
                        _globalUnlockData.DeleteMethod(base.Key, td);
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
						_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_PlanHierarchyNodeMissing,
							MIDText.GetText(eMIDTextCode.msg_pl_PlanHierarchyNodeMissing));
					}
					if (_versionRid == Include.NoRID)
					{
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanVersionMissing, this.ToString());
						_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
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
                        aStoreFilter);
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
                    aStoreFilter);
			}
            WriteAuditInfo();
		}

		public override void ProcessAction(SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aWorkFlowStep, 
			Profile aProfile, bool WriteToDB, int aStoreFilterRID)
		{
			try
			{
                HierarchyNodeProfile tmpHierarchyNodeProfile = null;
                int grpLvlRID = Include.NoRID;
                ProfileList storeProfileList = null;
                StoreGroupProfile sgp = null;
                ProfileList weekList = null;
				MIDTimer aTimer = new MIDTimer();
				_SAB = aSAB;
				this._applicationTransaction = aApplicationTransaction;
                tmpHierarchyNodeProfile = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid, false);
				_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

                _infoMsg = "Starting OTS Global Unlock: " + this.Name;
				aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());

				// Begin MID Track 4858 - JSmith - Security changes
				// Need to check here incase part of workflow
				if (!AuthorizedToUpdate(_SAB.ApplicationServerSession, _SAB.ClientServerSession.UserRID))
				{
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, this.ToString());
					_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
					_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
					return;
				}
				// End MID Track 4858

				aTimer.Start();

                //===================== Begin Business Logic ===========================================================

                if (_dateRangeRid > 0)
                {
                    //---Time Period Selected (Valid Selection)

                    DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);
                    weekList = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);

                    //==================Get Nodes Back===============================
                    NodeDescendantList hdl = null;
                    if (_multiLevel == true)
                    {
                        int tmpFromLevelOffset = 0;
                        int tmpToLevelOffset = 0;

                        //----Calculate From Level Temp Variable-----------------------
                        if (_fromLevelType == eFromLevelsType.LevelOffset)
                        {
                            //---Combo Type is LevelOffset---
                            tmpFromLevelOffset = FromLevelOffset;
                        }
                        else 
                        { 
                            if(tmpHierarchyNodeProfile.HomeHierarchyType == eHierarchyType.alternate)
                            {
                                //---Alternate with Organizational Level----
                                tmpFromLevelOffset = FromLevelOffset;
                            } else
                            { 
                                //---Organizational Level---
                                tmpFromLevelOffset = FromLevelSequence - tmpHierarchyNodeProfile.HomeHierarchyLevel;
                            }
                        }

                        //----Calculate To Level Temp Variable-----------------------
                        if (_toLevelType == eToLevelsType.LevelOffset)
                        {
                            //---Combo Type is LevelOffset---
                            tmpToLevelOffset = ToLevelOffset;
                        }
                        else
                        {
                            if (tmpHierarchyNodeProfile.HomeHierarchyType == eHierarchyType.alternate)
                            {
                                //---Alternate with Organizational Level----
                                tmpToLevelOffset = ToLevelOffset;
                            }
                            else
                            {
                                //---Organizational Level---
                                tmpToLevelOffset = ToLevelSequence - tmpHierarchyNodeProfile.HomeHierarchyLevel;
                            }
                        }

                        hdl = _SAB.HierarchyServerSession.GetNodeDescendantList(_openParms.ChainHLPlanProfile.NodeProfile.Key, eNodeSelectType.All, tmpFromLevelOffset, tmpToLevelOffset);
                    }
                    else 
                    {
                        hdl = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);
                        hdl.Add(new NodeDescendantProfile(_hierNodeRid));
                    }

                    //==================Get Stores Back===============================
                    storeProfileList = new ProfileList(eProfileType.StoreGroupLevel);
                    sgp = _SAB.StoreServerSession.GetStoreGroupFilled(SG_RID); //<--Store Group Profile

                    foreach (StoreGroupLevelProfile sglp in sgp.GroupLevels)
                    {
                        grpLvlRID = sglp.Key;

                        //-----Look Through Screen Attribute Set-----------------
                        for (int i = 0; i < _sglRidList.Count; i++)
                        {
                            if (Convert.ToInt32(_sglRidList[i]) == grpLvlRID)
                            {
                                foreach (StoreProfile sp in sglp.Stores)
                                {
                                    storeProfileList.Add(sp);
                                }
                            }
                        }

                    }

                    //==================Get a List Of Filtered Stores Back============
                    ProfileList filteredStoreList = GetFilteredStores(aStoreFilterRID, aApplicationTransaction);

                    foreach (NodeDescendantProfile ndp in hdl)
                    {
                        DeleteLocksForNode(aStoreFilterRID, aApplicationTransaction, ndp.Key, weekList, storeProfileList, filteredStoreList);
                    }
                }
                else 
                {
                    //---No Time Period Selected (Invalid Selection)
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_TimePeriodRequired, this.ToString());
                    _SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    return;
                }

                //===================== End Business Logic ===========================================================

				aTimer.Stop();

                _infoMsg = "Completed OTS Global Unlock: " + this.Name + " " + 
					"Elasped time: " + aTimer.ElaspedTimeString;
				aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());
				
				_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes the nodes based on what the user selects on the screen
		/// </summary>
        private void DeleteLocksForNode(int aStoreFilterRID, ApplicationSessionTransaction aApplicationTransaction, int ndpKey, ProfileList weekList, ProfileList storeProfileList, ProfileList filteredStoreList)
        {
            try
            {
                //----------------------------Loop Through Chains----------------------------------
                if (_chain == true)
                {
                    OTSGlobalUnlockMethodData OTSGlobalUnlockMethodData = new OTSGlobalUnlockMethodData();
                    try
                    {
                        bool success = false;
                        OTSGlobalUnlockMethodData.OpenUpdateConnection();
                        foreach (WeekProfile weeks in weekList)
                        {
                            success = OTSGlobalUnlockMethodData.DeleteChainData(_versionRid, ndpKey, weeks.Key);
                            if (!success)
                            {
                                _infoMsg = MIDText.GetText(eMIDTextCode.msg_UnableToDeleteChainLock);
                                _infoMsg = _infoMsg.Replace("{0}", SAB.HierarchyServerSession.GetNodeData(ndpKey, false).Text);
                                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
                                break;
                            }
                        }
                        if (success)
                        {
                            OTSGlobalUnlockMethodData.CommitData();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        OTSGlobalUnlockMethodData.CloseUpdateConnection();
                    }
                }


                //----------------------Loop Through List Of Store Group Levels-----------------------
                if (_stores == true)
                {
                    int storeRID = Include.UndefinedStoreRID;
                    bool deleteRecords = false;
                    foreach (StoreProfile sp in storeProfileList)
                    {
                        storeRID = sp.Key;
                        deleteRecords = true;

                        if (filteredStoreList != null)
                        {
                            //---Have Filtered Set Of Stores To Check Against----
                            deleteRecords = false;
                            foreach (StoreProfile fsp in filteredStoreList)
                            {
                                if (fsp.Key == storeRID)
                                {
                                    deleteRecords = true;
                                    break;
                                }
                            }
                        }

                        if (deleteRecords)
                        {
                            OTSGlobalUnlockMethodData OTSGlobalUnlockMethodData = new OTSGlobalUnlockMethodData();
                            try
                            {
                                bool success = false;
                                OTSGlobalUnlockMethodData.OpenUpdateConnection();
                                foreach (WeekProfile weeks in weekList)
                                {
                                    success = OTSGlobalUnlockMethodData.DeleteStoreData(_versionRid, ndpKey, storeRID, weeks.Key);
                                    if (!success)
                                    {
                                        _infoMsg = MIDText.GetText(eMIDTextCode.msg_UnableToDeleteStoreLock);
                                        _infoMsg = _infoMsg.Replace("{0}", SAB.HierarchyServerSession.GetNodeData(ndpKey, false).Text);
                                        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
                                        break;
                                    }
                                }
                                if (success)
                                {
                                    OTSGlobalUnlockMethodData.CommitData();
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                OTSGlobalUnlockMethodData.CloseUpdateConnection();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Opens a cub and gets a list of stores base on a SG_RID.
        /// </summary>
        private ProfileList GetFilteredStores(int aStoreFilterRID, ApplicationSessionTransaction aApplicationTransaction)
        {
            try
            {
                ProfileList filteredStoreList = null;

                //======================Store Section===============================
                if (_stores == true)
                {
                    //------Have A Store At This Point------------------
                    if (_filter > 1)
                    {
                        //------Have A Filter At This Point------------------

                        //------------------------------------------------------------------
                        //  Open Cube To Get List Of Stores
                        //------------------------------------------------------------------

                        _cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateStorePlanMaintCubeGroup();

                        if (aStoreFilterRID != Include.NoRID && aStoreFilterRID != Include.UndefinedStoreFilter)
                        {
                            _filter = aStoreFilterRID;
                        }

                        FillOpenParmForPlan();

                        //------Open Cube---------------------------------------------------
                        ((StorePlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

                        _cubeGroup.SetStoreFilter(Filter);
                        filteredStoreList = _cubeGroup.GetFilteredProfileList(eProfileType.Store);

                        //------Close Cube---------------------------------------------------
                        ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                        _cubeGroup.Dispose();
                        _cubeGroup = null;
                    }
                }

                return filteredStoreList;

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void GetAuditInfo()
        {
            AuditData auditData = new AuditData();
            DataTable ar = auditData.ForecastAuditForecast_GetLastProcessed(this.Key);
            foreach (DataRow dr in ar.Rows)
            {
                if (dr["PROCESS_DATE_TIME"] != System.DBNull.Value)
                {
                    _lastProcessedDateTime = Convert.ToString(dr["PROCESS_DATE_TIME"], CultureInfo.CurrentUICulture);
                }
                if (dr["USER_NAME"] != System.DBNull.Value)
                {
                    _lastProcessedUser = Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentUICulture);
                }
            }
        }

        private void WriteAuditInfo()
        {
            int auditRID;
            AuditData auditData = new AuditData();
            try
            {
                auditData.OpenUpdateConnection();
                DateRangeProfile planDRP = _SAB.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);
                ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(planDRP, null);
                StoreGroupProfile sgp = _SAB.StoreServerSession.GetStoreGroup(SG_RID);
                ProfileList sgll = _SAB.StoreServerSession.GetStoreGroupLevelListViewList(SG_RID);

                auditRID = auditData.ForecastAuditForecast_Add(DateTime.Now,
                    _SAB.ApplicationServerSession.Audit.ProcessRID,
                    _SAB.ClientServerSession.UserRID,
                    _hierNodeRid,
                    this.Key,
                    this.Name,
                    this.MethodType,
                    _versionRid,
                    _versionRid,
                    planDRP.DateRangeType,
                    planDRP.Name,
                    planDRP.DisplayDate,
                    ((WeekProfile)weekRange[0]).YearWeek,
                    ((WeekProfile)weekRange[weekRange.Count - 1]).YearWeek,
                    this.SG_RID,
                    sgp.Name);

                auditData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                auditData.CloseUpdateConnection();
                GetAuditInfo();
            }

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
			_openParms = new PlanOpenParms(ePlanSessionType.ChainMultiLevel, _computationsMode);

			if (this.GlobalUserType == eGlobalUserType.User)
			{
				_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsUserCopyChain);
			}
			else
			{
				_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsGlobalCopyChain);
			}
			_openParms.FunctionSecurityProfile.SetAllowUpdate();

			HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(this.HierNodeRID);

            hnp.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.HierNodeRID, (int)eSecurityTypes.Chain);
            hnp.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.HierNodeRID, (int)eSecurityTypes.Store);

			ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
			VersionProfile vp = fvpb.Build(VersionRID);
            vp.ChainSecurity = _SAB.ClientServerSession.GetMyVersionSecurityAssignment(this.VersionRID, (int)eSecurityTypes.Chain);
            vp.StoreSecurity = _SAB.ClientServerSession.GetMyVersionSecurityAssignment(this.VersionRID, (int)eSecurityTypes.Store);
			
			_openParms.ChainHLPlanProfile.VersionProfile = vp;
			_openParms.StoreHLPlanProfile.VersionProfile = vp;

			_openParms.ChainHLPlanProfile.NodeProfile = hnp;
            _openParms.StoreHLPlanProfile.NodeProfile = hnp;
			_openParms.DateRangeProfile = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.DateRangeRID);
			_openParms.LowLevelVersionDefault = vp;

            _openParms.StoreGroupRID = SG_RID;
            _openParms.FilterRID = Filter;

			if (_computationsMode != null)
			{
				_openParms.ComputationsMode = _computationsMode;
			}
			else
			{
				_openParms.ComputationsMode = _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
			}

            GetAuditInfo();

			// BEGIN Issue 4328 - stodd 2.28.07
			//BuildLowLevelVersionOverrideList(HierNodeRID, LowerLevelType, LowerLevelOffset, LowerLevelSequence);
			// END Issue 4328

			//BuildLowLevelVersionList();
		}
	
		private void BuildLowLevelVersionList()
		{
			try
			{
				_openParms.ClearLowLevelPlanProfileList();

				foreach (LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList)
				{
					// BEGIN Issue 5089 stodd 1.4.2008 Truly exclude low levels
					if (!lvop.Exclude)
					{
						PlanProfile planProfile = new PlanProfile(lvop.Key);
						planProfile.NodeProfile = lvop.NodeProfile;
						planProfile.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Chain);
						planProfile.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Store);
						//==========================================================
						// We really don't want to 'exclude' the lower level.  
						// We just want the value to be treated as
						// excluded for the purposes of the spread.
						//==========================================================
						// BEGIN Issue 4858 stodd 11.12.2007
//						if (lvop.Exclude)
//						{
//							// BEGIN Issue 5084 stodd 01.02.2008 (Put his code back in.)
//							planProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();
//							// END Issue 5084
//							planProfile.IncludeExclude = eBasisIncludeExclude.Exclude;
//						}
//						else
//						{
							//planProfile.NodeProfile.ChainSecurityProfile.SetAllowUpdate();
							planProfile.IncludeExclude = eBasisIncludeExclude.Include;
//						}
						// END Issue 4858 stodd 11.12.2007

						if (lvop.VersionIsOverridden)
						{
							planProfile.VersionProfile = lvop.VersionProfile;
						}
						else
						{
							planProfile.VersionProfile = _openParms.LowLevelVersionDefault;
						}
						planProfile.VersionProfile.ChainSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(planProfile.VersionProfile.Key, (int)eSecurityTypes.Chain);
						//planProfile.VersionProfile.ChainSecurity.SetAllowUpdate();  // Removed Issue 4858
						//Begin Track #3867 -- Low level not sorted on Store Multi view
						//					_openParms.LowLevelPlanProfileList.Add(planProfile);
						_openParms.AddLowLevelPlanProfile(planProfile);
						//End Track #3867 -- Low level not sorted on Store Multi view
					}
					// END Issue 5089
				}
			}
			catch
			{
			}
		}

		

		/// <summary>
		/// Creates a list of LowLevelVersionOverrideProfiles.
		/// </summary>
		/// <param name="parentNodeRid"></param>
		/// <param name="lowLevelSeq"></param>
		/// <returns></returns>
		private ProfileList BuildLowLevelVersionOverrideList(int parentNodeRid, eLowLevelsType lowLevelType, int lowLevelOffset, int lowLevelSeq)
		{
			_lowlevelVersionOverrideList = new ProfileList(eProfileType.LowLevelVersionOverride);
			// Begin Issue 4328 - stodd 2.28.07
			HierarchyNodeList hnl = null;
			if (lowLevelType == eLowLevelsType.LevelOffset)
			{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//				hnl = _SAB.HierarchyServerSession.GetDescendantData(parentNodeRid, lowLevelOffset, true);
				hnl = _SAB.HierarchyServerSession.GetDescendantData(parentNodeRid, lowLevelOffset, true, eNodeSelectType.NoVirtual);
//End Track #4037
			}
			else
			{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//				hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(parentNodeRid, lowLevelSeq, true);
				hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(parentNodeRid, lowLevelSeq, true, eNodeSelectType.NoVirtual);
//End Track #4037
			}
			// End Issue 4328 - stodd 2.28.07

			foreach (HierarchyNodeProfile hnp in hnl)
			{
				LowLevelVersionOverrideProfile lvop = new LowLevelVersionOverrideProfile(hnp.Key);
				lvop.NodeProfile = hnp;

				if (_dtLowerLevels != null)
				{
					DataRow [] rows = _dtLowerLevels.Select("HN_RID = " + hnp.Key.ToString(CultureInfo.CurrentUICulture));
					if (rows.Length > 0)
					{
						DataRow aRow = rows[0];
						int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
						eBasisIncludeExclude ieInd = (eBasisIncludeExclude)Convert.ToInt32(aRow["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
						if (versionRid == this._versionRid)
						{
							lvop.VersionIsOverridden = false;
							//Begin Track #4457 - JSmith - Add forecast versions
//							lvop.VersionProfile = new VersionProfile(_versionRid);
							ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
							lvop.VersionProfile = fvpb.Build(_versionRid);
							//End Track #4457
							lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);
						}
						else
						{
							lvop.VersionIsOverridden = true;
							//Begin Track #4457 - JSmith - Add forecast versions
//							lvop.VersionProfile = new VersionProfile(versionRid);
							ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
							lvop.VersionProfile = fvpb.Build(versionRid);
							//End Track #4457
							lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(versionRid);
						}
						if (ieInd == eBasisIncludeExclude.Exclude)
						{
							lvop.Exclude = true;
						}
						else
						{
							lvop.Exclude = false;
						}
						_lowlevelVersionOverrideList.Add(lvop);
					}
					else
					{
						lvop.VersionIsOverridden = false;
						//Begin Track #4457 - JSmith - Add forecast versions
//						lvop.VersionProfile = new VersionProfile(_versionRid);
						ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
						lvop.VersionProfile = fvpb.Build(_versionRid);
						//End Track #4457
						lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

						lvop.Exclude = false;
						_lowlevelVersionOverrideList.Add(lvop);
					}

				}
				else
				{
					lvop.VersionIsOverridden = false;
					//Begin Track #4457 - JSmith - Add forecast versions
//					lvop.VersionProfile = new VersionProfile(_versionRid);
					ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
					lvop.VersionProfile = fvpb.Build(_versionRid);
					//End Track #4457
					lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

					lvop.Exclude = false;
					_lowlevelVersionOverrideList.Add(lvop);
				}
			}
			return _lowlevelVersionOverrideList;
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
		override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
		{
            OTSGlobalUnlockMethod newOTSGlobalUnlockMethod = null;
			int maxRows;
			int basisDateRangeRid;

			try
			{
                newOTSGlobalUnlockMethod = (OTSGlobalUnlockMethod)this.MemberwiseClone();
				if (aCloneDateRanges &&
					DateRangeRID != Include.UndefinedCalendarDateRange)
				{
                    newOTSGlobalUnlockMethod.DateRangeRID = aSession.Calendar.GetDateRangeClone(DateRangeRID).Key;
				}
				else
				{
                    newOTSGlobalUnlockMethod.DateRangeRID = DateRangeRID;
				}
                newOTSGlobalUnlockMethod.DSGlobalUnlock = DSGlobalUnlock.Copy();
				if (aCloneDateRanges)
				{
                    maxRows = newOTSGlobalUnlockMethod.DSGlobalUnlock.Tables["Basis"].Rows.Count;  //the basis detail table
					for (int row=0;row<maxRows;row++)
					{
                        basisDateRangeRid = Convert.ToInt32(DSGlobalUnlock.Tables["Basis"].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);
                        newOTSGlobalUnlockMethod.DSGlobalUnlock.Tables["Basis"].Rows[row]["CDR_RID"] = aSession.Calendar.GetDateRangeClone(basisDateRangeRid).Key;
					}
				}
                newOTSGlobalUnlockMethod.DSGlobalUnlock.AcceptChanges();
                newOTSGlobalUnlockMethod.DTLowerLevels = DTLowerLevels.Copy();
                newOTSGlobalUnlockMethod.HierNodeRID = HierNodeRID;
                newOTSGlobalUnlockMethod.MultiLevel = MultiLevel;
                newOTSGlobalUnlockMethod.Stores = Stores;
                newOTSGlobalUnlockMethod.Chain = Chain;
                newOTSGlobalUnlockMethod.Filter = Filter;
                newOTSGlobalUnlockMethod.FromLevelOffset = FromLevelOffset;
                newOTSGlobalUnlockMethod.FromLevelSequence = FromLevelSequence;
                newOTSGlobalUnlockMethod.FromLevelType = FromLevelType;
                newOTSGlobalUnlockMethod.ToLevelOffset = ToLevelOffset;
                newOTSGlobalUnlockMethod.ToLevelSequence = ToLevelSequence;
                newOTSGlobalUnlockMethod.ToLevelType = ToLevelType;
                newOTSGlobalUnlockMethod.Method_Change_Type = eChangeType.none;
                newOTSGlobalUnlockMethod.Method_Description = Method_Description;
                newOTSGlobalUnlockMethod.MethodStatus = MethodStatus;
                newOTSGlobalUnlockMethod.Name = Name;
                newOTSGlobalUnlockMethod.SG_RID = SG_RID;
                newOTSGlobalUnlockMethod.SpreadOption = SpreadOption;
                newOTSGlobalUnlockMethod.User_RID = User_RID;
                newOTSGlobalUnlockMethod.VersionRID = VersionRID;
                newOTSGlobalUnlockMethod.Virtual_IND = Virtual_IND;

				if (_lowlevelVersionOverrideList != null)
				{
					if (_versionProfList == null)
					{
						_versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();
					}
                    CopyVersionOverrideList(newOTSGlobalUnlockMethod);
				}

                return newOTSGlobalUnlockMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private void CopyVersionOverrideList(OTSGlobalUnlockMethod OTSGlobalUnlockMethod)
		{
			try
			{
                OTSGlobalUnlockMethod._lowlevelVersionOverrideList = new ProfileList(eProfileType.LowLevelVersionOverride);
				foreach(LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList.ArrayList)
				{
					LowLevelVersionOverrideProfile newlvop = lvop.Copy();
					if (newlvop.VersionProfile == null)
					{
						newlvop.VersionProfile = (VersionProfile)_versionProfList.FindKey(_versionRid);
					}
                    OTSGlobalUnlockMethod._lowlevelVersionOverrideList.Add(newlvop);
				}
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
				if (VersionRID == Include.NoRID || HierNodeRID == Include.NoRID)
				{
					return false;
				}

				FillOpenParmForPlan();

                if (_stores == true)
                {
                    if (_openParms.StoreHLPlanProfile.VersionProfile == null || !_openParms.StoreHLPlanProfile.VersionProfile.StoreSecurity.AllowUpdate)
				    {
					    return false;
				    }

                    HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.StoreHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Store);
				    if (!hierNodeSecurity.AllowUpdate)
				    {
					    return false;
				    }
                }

                if (_chain == true)
                {
                    if (_openParms.ChainHLPlanProfile.VersionProfile == null || !_openParms.ChainHLPlanProfile.VersionProfile.ChainSecurity.AllowUpdate)
				    {
					    return false;
				    }

                    HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);
				    if (!hierNodeSecurity.AllowUpdate)
				    {
					    return false;
				    }
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
	}

	
}
