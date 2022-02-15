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
    /// Summary description for OTSGlobalUnlockMethod.
    /// </summary>
    public class OTSGlobalUnlockMethod : OTSPlanBaseMethod
    {
        private OTSGlobalUnlockMethodData _globalUnlockData;
        private int _hierNodeRid;
        private HierarchyNodeProfile _hnp;                  //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
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
        private int _overrideLowLevelRid;

        #region Properties
        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodGlobalUnlock;
            }
        }

        public int HierNodeRID
        {
            get { return _hierNodeRid; }
            set { _hierNodeRid = value; }
        }

        public int VersionRID
        {
            get { return _versionRid; }
            set { _versionRid = value; }
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
            get { return _dateRangeRid; }
            set { _dateRangeRid = value; }
        }

        public eSpreadOption SpreadOption
        {
            get { return _spreadOption; }
            set { _spreadOption = value; }
        }

        public bool MultiLevel
        {
            get { return _multiLevel; }
            set { _multiLevel = value; }
        }

        public ArrayList SGL_RID_List
        {
            get { return _sglRidList; }
            set { _sglRidList = value; }
        }

        public bool Stores
        {
            get { return _stores; }
            set { _stores = value; }
        }

        public bool Chain
        {
            get { return _chain; }
            set { _chain = value; }
        }

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

        public int Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public int OverrideLowLevelRid
        {
            get { return _overrideLowLevelRid; }
            set { _overrideLowLevelRid = value; }
        }

        #endregion Properties


        public OTSGlobalUnlockMethod(SessionAddressBlock SAB, int aMethodRID)
            : base(SAB,
            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //aMethodRID, eMethodType.GlobalUnlock)
            aMethodRID, eMethodType.GlobalUnlock, eProfileType.MethodGlobalUnlock)
        //End TT#523 - JScott - Duplicate folder when new folder added
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
                _overrideLowLevelRid = _globalUnlockData.OverrideLowLevelRid;
            }
            else
            {   //Defaults
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
                _overrideLowLevelRid = Include.NoRID;
            }
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(_filter))
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

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

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
            _globalUnlockData.OverrideLowLevelRid = _overrideLowLevelRid;
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
                        // make sure the key in the data layer is the same
                        _globalUnlockData.MethodRid = base.Key;
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
            try
            {
                // Begin TT#381-MD - JSmith - OTS Forecast Workflow with a Filter. The filter criteria is not being honored for Global Unlock, Global Lock. and Matrix Balance.
                if (aStoreFilter != Include.UndefinedStoreFilter &&
                    aStoreFilter != Include.NoRID)
                {
                    Filter = aStoreFilter;
                }
                // End TT#381-MD - JSmith - OTS Forecast Workflow with a Filter. The filter criteria is not being honored for Global Unlock, Global Lock. and Matrix Balance.

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
            // BEGIN Issue 5727 stodd
            catch
            {
                aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
            }
            // END Issue 5727 
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
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
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
                    LowLevelVersionOverrideProfileList overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                    LowLevelVersionOverrideProfileList tempOverrideList = null;
                    HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
                    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid);
                    ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                    VersionProfile vp = fvpb.Build(VersionRID);
                    vp.StoreSecurity = new VersionSecurityProfile(this.VersionRID);
                    vp.ChainSecurity = new VersionSecurityProfile(this.VersionRID);

                    //==================Get Nodes Back===============================
                    NodeDescendantList hdl = null;
                    //BEGIN TT#273 - MD - DOConnell - Delete of Lock Method receives error
                    // Begin TT#2904 - JSmith - Global Unlock Issue
                    if (_multiLevel == true)
                    //if (_multiLevel == false)
                    // End TT#2904 - JSmith - Global Unlock Issue
                    //END TT#273 - MD - DOConnell - Delete of Lock Method receives error
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
                            if (tmpHierarchyNodeProfile.HomeHierarchyType == eHierarchyType.alternate)
                            {
                                //---Alternate with Organizational Level----
                                tmpFromLevelOffset = FromLevelOffset;
                            }
                            else
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

                        for (int offset = tmpFromLevelOffset; offset <= tmpToLevelOffset; offset++)
                        {
                            if (offset != 0)
                            {
                                tempOverrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                                                    offset, Include.NoRID, true, false);
                                overrideList.AddRange(tempOverrideList.ArrayList);
                            }
                        }
                        //BEGIN TT#259 - MD - DOConnell - Cell Lock/Unlock does not work when cell is not selected
                        // Begin TT#2904 - JSmith - Global Unlock Issue
                        //foreach (LowLevelVersionOverrideProfile lvop in tempOverrideList)
                        //{
                        //    lvop.ExcludeIsOverridden = true;
                        //    lvop.Exclude = true;
                        //}
                        // End TT#2904 - JSmith - Global Unlock Issue
                        overrideList.Insert(0, new LowLevelVersionOverrideProfile(_hierNodeRid, hnp, false, vp, false));
                        //END TT#259 - MD - DOConnell - Cell Lock/Unlock does not work when cell is not selected

                        //hdl = _SAB.HierarchyServerSession.GetNodeDescendantList(_openParms.ChainHLPlanProfile.NodeProfile.Key, eNodeSelectType.All, tmpFromLevelOffset, tmpToLevelOffset);
                        //if (tmpFromLevelOffset == 0)
                        //{
                        //    hdl.Insert(0, new NodeDescendantProfile(_hierNodeRid));
                        //}
                    }
                    else
                    {
                        //hdl = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);
                        //hdl.Add(new NodeDescendantProfile(_hierNodeRid));
                        // Begin TT#2904 - JSmith - Global Unlock Issue
                        //overrideList = hTran.GetOverrideList(_overrideLowLevelRid, _hierNodeRid, this.VersionRID,
                        //                        1, Include.NoRID, true, false);
                        overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                        // End TT#2904 - JSmith - Global Unlock Issue
                        // Insert the method hierarchy node to the override list as a non-override node
                        overrideList.Insert(0, new LowLevelVersionOverrideProfile(_hierNodeRid, hnp, false, vp, false));
                    }

                    //==================Get Stores Back===============================
                    storeProfileList = new ProfileList(eProfileType.StoreGroupLevel);
                    sgp = StoreMgmt.StoreGroup_GetFilled(SG_RID); //_SAB.StoreServerSession.GetStoreGroupFilled(SG_RID); //<--Store Group Profile

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
                    //ProfileList filteredStoreList = GetFilteredStores(aStoreFilterRID, aApplicationTransaction);
                    ProfileList filteredStoreList = GetFilteredStores(this.Filter, aApplicationTransaction);
                    //END TT#2589 - DOConnell - global lock not locking with filter selected
                    foreach (LowLevelVersionOverrideProfile llvop in overrideList.ArrayList)
                    {
                        // BEGIN Issue 5636 stodd 6.24.2008
                        if (!llvop.Exclude)
                            DeleteLocksForNode(aStoreFilterRID, aApplicationTransaction, llvop, weekList, storeProfileList, filteredStoreList);
                        // END Issue 5636
                    }
                }
                else
                {
                    //---No Time Period Selected (Invalid Selection)
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_TimePeriodRequired, this.ToString());
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
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
        private void DeleteLocksForNode(int aStoreFilterRID, ApplicationSessionTransaction aApplicationTransaction,
            LowLevelVersionOverrideProfile llvop, ProfileList weekList, ProfileList storeProfileList, ProfileList filteredStoreList)
        {
            try
            {
                // Set which version to use for the node
                int versionKey = this.VersionRID;
                if (llvop.VersionIsOverridden)
                    versionKey = llvop.OverrideVersionProfile.Key;

                // Begin TT#54 - Global Unlock running slow
                WeekProfile startWeek = (WeekProfile)weekList[0];
                WeekProfile endWeek = (WeekProfile)weekList[weekList.Count - 1];
                int startWeekKey = startWeek.Key;
                int endWeekKey = endWeek.Key;
                // End TT#54

                //----------------------------Loop Through Chains----------------------------------
                if (_chain == true)
                {
                    OTSGlobalUnlockMethodData OTSGlobalUnlockMethodData = new OTSGlobalUnlockMethodData();
                    try
                    {
                        bool success = false;
                        OTSGlobalUnlockMethodData.OpenUpdateConnection();

                        // Begin TT#54 - Global Unlock running slow
                        //foreach (WeekProfile weeks in weekList)
                        //{
                        //    success = OTSGlobalUnlockMethodData.DeleteChainData(versionKey, llvop.Key, weeks.Key);
                        //    if (!success)
                        //    {
                        //        _infoMsg = MIDText.GetText(eMIDTextCode.msg_UnableToDeleteChainLock);
                        //        _infoMsg = _infoMsg.Replace("{0}", SAB.HierarchyServerSession.GetNodeData(llvop.Key, false).Text);
                        //        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
                        //        break;
                        //    }
                        //}

                        success = OTSGlobalUnlockMethodData.DeleteChainData(versionKey, llvop.Key, startWeekKey, endWeekKey);
                        if (!success)
                        {
                            _infoMsg = MIDText.GetText(eMIDTextCode.msg_UnableToDeleteChainLock);
                            _infoMsg = _infoMsg.Replace("{0}", SAB.HierarchyServerSession.GetNodeData(llvop.Key, false).Text);
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
                        }
                        // End TT#54

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
                    // Begin TT#54 - Global Unlock running slow
                    //int storeRID = Include.UndefinedStoreRID;
                    //bool deleteRecords = false;
                    //foreach (StoreProfile sp in storeProfileList)
                    //{
                    //    storeRID = sp.Key;
                    //    deleteRecords = true;

                    //    if (filteredStoreList != null)
                    //    {
                    //        //---Have Filtered Set Of Stores To Check Against----
                    //        deleteRecords = false;
                    //        foreach (StoreProfile fsp in filteredStoreList)
                    //        {
                    //            if (fsp.Key == storeRID)
                    //            {
                    //                deleteRecords = true;
                    //                break;
                    //            }
                    //        }
                    //    }

                    //    if (deleteRecords)
                    //    {
                    //        OTSGlobalUnlockMethodData OTSGlobalUnlockMethodData = new OTSGlobalUnlockMethodData();
                    //        try
                    //        {
                    //            bool success = false;
                    //            OTSGlobalUnlockMethodData.OpenUpdateConnection();
                    //            foreach (WeekProfile weeks in weekList)
                    //            {
                    //                success = OTSGlobalUnlockMethodData.DeleteStoreData(versionKey, llvop.Key, storeRID, weeks.Key);
                    //                if (!success)
                    //                {
                    //                    _infoMsg = MIDText.GetText(eMIDTextCode.msg_UnableToDeleteStoreLock);
                    //                    _infoMsg = _infoMsg.Replace("{0}", SAB.HierarchyServerSession.GetNodeData(llvop.Key, false).Text);
                    //                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
                    //                    break;
                    //                }
                    //            }
                    //            if (success)
                    //            {
                    //                OTSGlobalUnlockMethodData.CommitData();
                    //            }
                    //        }
                    //        catch (Exception)
                    //        {
                    //            throw;
                    //        }
                    //        finally
                    //        {
                    //            OTSGlobalUnlockMethodData.CloseUpdateConnection();
                    //        }
                    //    }
                    //}

                    ArrayList ridList = new ArrayList();
                    if (filteredStoreList != null)
                    {
                        foreach (StoreProfile sp in filteredStoreList)
                        {
                            //BEGIN TT#2589 - DOConnell - global lock not locking with filter selected
                            if (storeProfileList.Contains(sp.Key))
                            {
                                ridList.Add(sp.Key);
                            }
                            //END TT#2589 - DOConnell - global lock not locking with filter selected
                        }
                    }
                    else
                    {
                        foreach (StoreProfile sp in storeProfileList)
                        {
                            ridList.Add(sp.Key);
                        }
                    }
                    OTSGlobalUnlockMethodData OTSGlobalUnlockMethodData = new OTSGlobalUnlockMethodData();
                    try
                    {
                        bool success = false;
                        OTSGlobalUnlockMethodData.OpenUpdateConnection();

                        success = OTSGlobalUnlockMethodData.DeleteStoreData(versionKey, llvop.Key, startWeekKey, endWeekKey, ridList);
                        if (!success)
                        {
                            _infoMsg = MIDText.GetText(eMIDTextCode.msg_UnableToDeleteStoreLock);
                            _infoMsg = _infoMsg.Replace("{0}", SAB.HierarchyServerSession.GetNodeData(llvop.Key, false).Text);
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
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
                    // End TT#54  
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
                        // BEGIN Issue 5727 stodd
                        try
                        {
                            ((StorePlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

                            // BEGIN Issue 5727 stodd
                            if (!_cubeGroup.SetStoreFilter(Filter, _cubeGroup))
                            {
                                FilterData storeFilterData = new FilterData();
                                string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                                msg = msg.Replace("{0}", storeFilterData.FilterGetName(Filter));
                                string suffix = ". Method " + this.Name + ". ";
                                string auditMsg = msg + suffix;
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
                                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                            }
                            // END Issue 5727
                            filteredStoreList = _cubeGroup.GetFilteredProfileList(eProfileType.Store);
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            //------Close Cube---------------------------------------------------
                            ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                            _cubeGroup.Dispose();
                            _cubeGroup = null;
                        }
                        // End Issue 5727 stodd
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
            //BEGIN TT#335 - MD - DOConnell - Last Processed information incorrect on Global Lock and Unlock
            if (this.Key != Include.NoRID)
            {
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
            else
            {
                _lastProcessedDateTime = Convert.ToString(DateTime.Now);
                _lastProcessedUser = _SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID);
            }
            //END TT#335 - MD - DOConnell - Last Processed information incorrect on Global Lock and Unlock
        }

        private void WriteAuditInfo()
        {
            int auditRID;
            AuditData auditData = new AuditData();
            try
            {
                auditData.OpenUpdateConnection();
                _hnp = _SAB.HierarchyServerSession.GetNodeData(_hierNodeRid, false, false);                 //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
                DateRangeProfile planDRP = _SAB.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);
                ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(planDRP, null);
                //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID); //_SAB.StoreServerSession.GetStoreGroup(SG_RID);
                string groupName = StoreMgmt.StoreGroup_GetName(SG_RID); //TT#1517-MD -jsobek -Store Service Optimization
                ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(SG_RID);

                auditRID = auditData.ForecastAuditForecast_Add(DateTime.Now,
                    _SAB.ApplicationServerSession.Audit.ProcessRID,
                    _SAB.ClientServerSession.UserRID,
                    _hierNodeRid,
                    _hnp.Text,                      //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
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
                    groupName);

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
                lvop.VersionIsOverridden = false;
                //Begin Track #4457 - JSmith - Add forecast versions
                //				lvop.VersionProfile = new VersionProfile(_versionRid);
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                lvop.VersionProfile = fvpb.Build(_versionRid);
                //End Track #4457
                lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

                lvop.Exclude = false;
                _lowlevelVersionOverrideList.Add(lvop);
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
            OTSGlobalUnlockMethod newOTSGlobalUnlockMethod = null;
            int maxRows;
            int basisDateRangeRid;
            // Begin Track #5912 - JSmith - Save As needs to clone custom override models
            OverrideLowLevelProfile ollp;
            int customUserRID;
            // End Track #5912

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
                newOTSGlobalUnlockMethod.Template_IND = Template_IND;
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
                    newOTSGlobalUnlockMethod.CustomOLL_RID = customUserRID;
                }
                // End Track #5912
                newOTSGlobalUnlockMethod.OverrideLowLevelRid = OverrideLowLevelRid;


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
                foreach (LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList.ArrayList)
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

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalGlobalUnlock);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserGlobalUnlock);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROOverrideLowLevel overrideLowLevel = new ROOverrideLowLevel();
            overrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(OverrideLowLevelRid, SAB); //CustomOLL_RID;
            overrideLowLevel.OverrideLowLevelsModelList = BuildOverrideLowLevelList(
                overrideLowLevelRid: OverrideLowLevelRid,
                customOverrideLowLevelRid: CustomOLL_RID
                );

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                overrideLowLevel.IsCustomModel = true;
            }

            ROLevelInformation fromLevel = new ROLevelInformation();
            fromLevel.LevelType = (eROLevelsType)FromLevelType;
            fromLevel.LevelOffset = FromLevelOffset;
            fromLevel.LevelSequence = FromLevelSequence;
            fromLevel.LevelValue= GetName.GetLevelName(
               levelType: (eROLevelsType)FromLevelType,
               levelSequence: FromLevelSequence,
               levelOffset: FromLevelOffset,
               SAB: SAB
               );
            ROLevelInformation toLevel = new ROLevelInformation();
            toLevel.LevelType = (eROLevelsType)ToLevelType;
            toLevel.LevelOffset = ToLevelOffset;
            toLevel.LevelSequence = ToLevelSequence;
            toLevel.LevelValue= GetName.GetLevelName(
               levelType: (eROLevelsType)ToLevelType,
               levelSequence: ToLevelSequence,
               levelOffset: ToLevelOffset,
               SAB: SAB
               );

            ROPlanningGlobalLockUnlockProperties method = new ROPlanningGlobalLockUnlockProperties(
                eMethodType.GlobalUnlock,
                kvpMethod: GetName.GetMethod(method: this),
                sDescription: Method_Description,
                iUserKey: User_RID,
                kvpMerchandise: GetName.GetMerchandiseName(HierNodeRID, SAB),
                kvpVersion: GetName.GetVersion(VersionRID, SAB),
                bIsMultilevel: MultiLevel,
                FromLevel: fromLevel,
                ToLevel: toLevel,
                kvpTimePeriod: GetName.GetCalendarDateRange(DateRangeRID, SAB),
                overrideLowLevel: overrideLowLevel,
                bStoreOptions: Stores,
                bChainOptions: Chain,

                kvpStoreAttribute: GetName.GetAttributeName(SG_RID),
                attributeSet: SGL_RID_List,
                kvpFilter: GetName.GetFilterName(Filter),
                sLastProcessedDateTime: LastProcessedDateTime,
                sLastProcessedUser: LastProcessedUser,
                isTemplate: Template_IND


              );
            return method;
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROPlanningGlobalLockUnlockProperties roOTSGlobalUnlockProperties = (ROPlanningGlobalLockUnlockProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                HierNodeRID = roOTSGlobalUnlockProperties.Merchandise.Key;
                VersionRID = roOTSGlobalUnlockProperties.Version.Key;
                MultiLevel = roOTSGlobalUnlockProperties.IsMultiLevel;

                FromLevelType = (eFromLevelsType)roOTSGlobalUnlockProperties.FromLevel.LevelType;
                FromLevelOffset = roOTSGlobalUnlockProperties.FromLevel.LevelOffset;
                FromLevelSequence = roOTSGlobalUnlockProperties.FromLevel.LevelSequence;

                ToLevelType = (eToLevelsType)roOTSGlobalUnlockProperties.ToLevel.LevelType;
                ToLevelOffset = roOTSGlobalUnlockProperties.ToLevel.LevelOffset;
                ToLevelSequence = roOTSGlobalUnlockProperties.ToLevel.LevelSequence;

                DateRangeRID = roOTSGlobalUnlockProperties.TimePeriod.Key;
                OverrideLowLevelRid = roOTSGlobalUnlockProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                Stores = roOTSGlobalUnlockProperties.IsStoreOptions;
                Chain = roOTSGlobalUnlockProperties.IsChainOptions;
                SG_RID = roOTSGlobalUnlockProperties.StoreAttribute.Key;
                SGL_RID_List = roOTSGlobalUnlockProperties.AttributeSet;
                Filter = roOTSGlobalUnlockProperties.Filter.Key;
                LastProcessedDateTime = roOTSGlobalUnlockProperties.LastProcessedDateTime;
                LastProcessedUser = roOTSGlobalUnlockProperties.LastProcessedUser;

                return true;
            }
            catch
            {
                return false;
            }
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey


        // Begin MID Track 5852 - KJohnson - Security changes
        public bool StoreAuthorizedToPlan()	// Track #5859
        {
            try
            {
                if (VersionRID == Include.NoRID || HierNodeRID == Include.NoRID)
                {
                    return true;	// #5852 stodd
                }

                FillOpenParmForPlan();

                if (_openParms.StoreHLPlanProfile.VersionProfile == null || !_openParms.StoreHLPlanProfile.VersionProfile.StoreSecurity.AllowUpdate)
                {
                    return false;
                }

                HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.StoreHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Store);
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
        // End MID Track 5852

        // Begin MID Track 5852 - KJohnson - Security changes
        public bool ChainAuthorizedToPlan()	// TRack #5859 stodd
        {
            try
            {
                if (VersionRID == Include.NoRID || HierNodeRID == Include.NoRID)
                {
                    return true;	// Track #5852 stodd
                }

                FillOpenParmForPlan();

                if (_openParms.ChainHLPlanProfile.VersionProfile == null || !_openParms.ChainHLPlanProfile.VersionProfile.ChainSecurity.AllowUpdate)
                {
                    return false;
                }

                HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);
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
        // End MID Track 5852

    }


}
