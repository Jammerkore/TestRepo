using Logility.ROWebSharedTypes;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace MIDRetail.Business
{
    /// <summary>
    /// Summary description for OTSForcastSpreadMethod.
    /// </summary>
    public class OTSForecastSpreadMethod : OTSPlanBaseMethod
    {
        private OTSForecastSpreadMethodData _forecastSpreadData;
        private DataSet _dsForecastSpread;
        //private DataTable _dtLowerLevels;  <---DKJ
        private int _hierNodeRid;
        private HierarchyNodeProfile _hnp;                  //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
        private int _versionRid;
        private int _dateRangeRid;
        private string _lastProcessedDateTime;
        private string _lastProcessedUser;
        private eSpreadOption _spreadOption;
        private bool _ignoreLocks;
        private bool _multiLevel;
        private eFromLevelsType _fromLevelType;
        private int _fromLevelOffset;
        private int _fromLevelSequence;
        private eToLevelsType _toLevelType;
        private int _toLevelOffset;
        private int _toLevelSequence;
        private bool _equalizeWeighting;            // ANF - Weighting Multiple Basis
        private DataTable _dtEqWgtBasis;            // ANF - Weighting Multiple Basis
                                                    //		private ePlanType _planType; //Chain or Store
                                                    //		private int _storeFilterRid;
        private ProfileList _groupLevelBasisDetail;
        private ProfileList _versionProfList;
        private LowLevelVersionOverrideProfileList _lowlevelVersionOverrideList; // Override low level enhancement

        //private string _monitor;
        //private string _monitorFilePath;
        private SessionAddressBlock _SAB;
        private string _infoMsg;
        private PlanCubeGroup _cubeGroup;
        private PlanOpenParms _openParms;
        private ApplicationSessionTransaction _applicationTransaction;
        private string _computationsMode = "Default";
        private int _nodeOverrideRid = Include.NoRID;
        private int _versionOverrideRid = Include.NoRID;
        //private int _currentSglRid;
        private int _overrideLowLevelRid;

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        bool _foundDuplicate = false;
        string _duplicateMessage = null;
        // End TT#2281

        #region Properties
        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodForecastSpread;
            }
        }

        public DataSet DSForecastSpread
        {
            get { return _dsForecastSpread; }
            set { _dsForecastSpread = value; }
        }

        //public DataTable DTLowerLevels   <---DKJ
        //{
        //	get	{return _dtLowerLevels;}
        //	set	{_dtLowerLevels = value;}
        //}

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

        public bool IgnoreLocks
        {
            get { return _ignoreLocks; }
            set { _ignoreLocks = value; }
        }

        public bool MultiLevel
        {
            get { return _multiLevel; }
            set { _multiLevel = value; }
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

        // BEGIN ANF - Weighting Multiple Basis
        public bool EqualizeWeighting
        {
            get { return _equalizeWeighting; }
            set { _equalizeWeighting = value; }
        }
        // END ANF - Weighting Multiple Basis

        public int OverrideLowLevelRid
        {
            get { return _overrideLowLevelRid; }
            set { _overrideLowLevelRid = value; }
        }

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public bool FoundDuplicate
        {
            get
            {
                return _foundDuplicate;
            }
        }

        public string DuplicateMessage
        {
            get
            {
                return _duplicateMessage;
            }
        }
        // End TT#2281
        #endregion Properties


        public OTSForecastSpreadMethod(SessionAddressBlock SAB, int aMethodRID) : base(SAB,
            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //aMethodRID, eMethodType.ForecastSpread)
            aMethodRID, eMethodType.ForecastSpread, eProfileType.MethodForecastSpread)
        //End TT#523 - JScott - Duplicate folder when new folder added
        {
            _groupLevelBasisDetail = new ProfileList(eProfileType.GroupLevelFunction);
            _SAB = SAB;

            //_monitor = _SAB.MonitorForecastAppSetting;
            //_monitorFilePath = _SAB.GetMonitorFilePathAppSetting();

            if (base.Filled)
            {
                _forecastSpreadData = new OTSForecastSpreadMethodData(aMethodRID, eChangeType.populate);
                _hierNodeRid = _forecastSpreadData.HierNodeRID;
                _versionRid = _forecastSpreadData.VersionRID;
                _dateRangeRid = _forecastSpreadData.CDR_RID;
                _dsForecastSpread = _forecastSpreadData.DSForecastSpread;
                //_dtLowerLevels = _forecastSpreadData.DTLowerLevels;  <---DKJ
                _spreadOption = _forecastSpreadData.SpreadOption;
                _ignoreLocks = _forecastSpreadData.IgnoreLocks;
                _multiLevel = _forecastSpreadData.MultiLevel;
                _fromLevelType = _forecastSpreadData.FromLevelType;
                _fromLevelOffset = _forecastSpreadData.FromLevelOffset;
                _fromLevelSequence = _forecastSpreadData.FromLevelSequence;
                _toLevelType = _forecastSpreadData.ToLevelType;
                _toLevelOffset = _forecastSpreadData.ToLevelOffset;
                _toLevelSequence = _forecastSpreadData.ToLevelSequence;
                _equalizeWeighting = _forecastSpreadData.EqualizeWeighting;     // ANF - Weighting Multiple Basis
                _overrideLowLevelRid = _forecastSpreadData.OverrideLowLevelRid;
            }
            else
            {   //Defaults
                _hierNodeRid = Include.NoRID;
                _versionRid = Include.NoRID;
                _dateRangeRid = Include.NoRID;
                _spreadOption = eSpreadOption.Plan;
                _ignoreLocks = false;
                _multiLevel = false;
                _fromLevelType = eFromLevelsType.None;
                _fromLevelOffset = Include.NoRID;
                _fromLevelSequence = Include.NoRID;
                _toLevelType = eToLevelsType.None;
                _toLevelOffset = Include.NoRID;
                _toLevelSequence = Include.NoRID;
                _equalizeWeighting = false;             // ANF - Weighting Multiple Basis
                _overrideLowLevelRid = Include.NoRID;
            }
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsHierarchyNodeUser(_hierNodeRid))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        /// <summary>
        /// Updates the OTS MultiLevel method
        /// </summary>
        /// <param name="td">An instance of the TransactionData class which contains the database connection</param>
        //		new public void Update(TransactionData td)
        override public void Update(TransactionData td)
        {
            if (_forecastSpreadData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _forecastSpreadData = new OTSForecastSpreadMethodData(td, base.Key);
            }

            _forecastSpreadData.HierNodeRID = _hierNodeRid;
            _forecastSpreadData.VersionRID = _versionRid;
            _forecastSpreadData.CDR_RID = _dateRangeRid;
            _forecastSpreadData.SpreadOption = _spreadOption;
            _forecastSpreadData.IgnoreLocks = _ignoreLocks;
            _forecastSpreadData.MultiLevel = _multiLevel;
            _forecastSpreadData.FromLevelType = _fromLevelType;
            _forecastSpreadData.FromLevelOffset = _fromLevelOffset;
            _forecastSpreadData.FromLevelSequence = _fromLevelSequence;
            _forecastSpreadData.ToLevelType = _toLevelType;
            _forecastSpreadData.ToLevelOffset = _toLevelOffset;
            _forecastSpreadData.ToLevelSequence = _toLevelSequence;
            _forecastSpreadData.EqualizeWeighting = _equalizeWeighting;    // ANF - Weighting Multiple Basis
            _forecastSpreadData.DSForecastSpread = _dsForecastSpread;
            //_forecastSpreadData.DTLowerLevels = _dtLowerLevels;  <---DKJ
            _forecastSpreadData.OverrideLowLevelRid = _overrideLowLevelRid;

            try
            {
                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _forecastSpreadData.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _forecastSpreadData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _forecastSpreadData.DeleteMethod(base.Key, td);
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
            // Begin MID Track #5210 - JSmith - Out of memory
            try
            {
                // End MID Track #5210
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
                            //Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
                            //							Include.NoRID);
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
                        //						Include.NoRID);
                        aStoreFilter);
                    //End Track #5188 - JScott - Method Filters not being over overriden from Workflow
                }
                // Begin MID Track #5210 - JSmith - Out of memory
                WriteAuditInfo();
            }
            catch
            {
                throw;
            }
            finally
            {
                _forecastSpreadData = null;
                _dsForecastSpread = null;
                //_dtLowerLevels = null;  <---DKJ
                _groupLevelBasisDetail = null;
                _versionProfList = null;
                _lowlevelVersionOverrideList = null;
                _cubeGroup = null;
                _openParms = null;
            }
            // End MID Track #5210
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

                _infoMsg = "Starting OTS Forecast Chain Spread: " + this.Name;
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

                HierarchyNodeProfile tmpHierarchyNodeProfile = _SAB.HierarchyServerSession.GetNodeData(this.HierNodeRID, false, false);
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

                    hdl = _SAB.HierarchyServerSession.GetNodeDescendantList(_openParms.ChainHLPlanProfile.NodeProfile.Key, eNodeSelectType.All, tmpFromLevelOffset, tmpToLevelOffset - 1);
                    if (tmpFromLevelOffset == 0)
                    {
                        hdl.Insert(0, new NodeDescendantProfile(_hierNodeRid));
                    }
                }
                else
                {
                    hdl = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);
                    hdl.Add(new NodeDescendantProfile(_hierNodeRid));
                }

                foreach (NodeDescendantProfile ndp in hdl)
                {
                    this._hierNodeRid = ndp.Key;

                    _cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateChainMultiLevelPlanMaintCubeGroup();

                    FillOpenParmForPlan();

                    // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                    if (_foundDuplicate)
                    {
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_DuplicateDescendantInMethodError, DuplicateMessage), this.ToString());
                        _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                        continue;
                    }
                    // End TT#2281

                    // BEGIN Issue 5204 2.15.2008
                    if (_lowlevelVersionOverrideList.Count == 0)
                    {
                        _infoMsg = "OTS Forecast Chain Spread: " + this.Name + ". " +
                                "No lower level merchandise nodes were found to spread to.";
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());
                    }
                    else
                    {
                        bool useBasis = false;
                        if (_spreadOption == eSpreadOption.Basis)
                        {
                            useBasis = true;
                            //FillOpenParmForBasis(Include.AllStoreGroupRID);

                            // BEGIN ANF - Weighting Multiple Basis
                            if (_equalizeWeighting)
                            {
                                ProcessEqualizedWeighting(aApplicationTransaction);
                            }
                            else
                            {
                                FillOpenParmForBasis(Include.AllStoreGroupRID);
                            }
                        }	// END ANF - Weighting Multiple Basis

                        ((ChainMultiLevelPlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

                        ((ChainMultiLevelPlanMaintCubeGroup)_cubeGroup).SpreadHighToLowLevelChain(useBasis, _ignoreLocks);

                        Save();
                    }
                    // END Issue 5204

                    // Cleanup & dequeue
                    ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                    // Begin MID Track #5210 - JSmith - Out of memory
                    _cubeGroup.Dispose();
                    _cubeGroup = null;
                    _lowlevelVersionOverrideList.Clear();
                    // End MID Track #5210
                }

                //===================== End Business Logic ===========================================================

                aTimer.Stop();

                _infoMsg = "Completed OTS Forecast Chain Spread: " + this.Name + " " +
                    "Elasped time: " + aTimer.ElaspedTimeString;
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());


                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                //_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                if (_applicationTransaction.OTSPlanActionStatus == eOTSPlanActionStatus.NoActionPerformed)
                {
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                }
                // End TT#2281
            }
            catch (Exception ex)
            {
                // BEGIN Issue 5401 stodd
                string msg = MIDText.GetText(eMIDTextCode.msg_MethodException);
                msg = msg.Replace("{0}", this.Name);
                msg = msg.Replace("{1}", ex.ToString());
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, this.ToString());
                // END Issue 5401
                throw;
            }
            // Begin MID Track #5399 - JSmith - Plan In Use error
            finally
            {
                // Cleanup & dequeue
                if (_cubeGroup != null)
                {
                    ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                    _cubeGroup.Dispose();
                    _cubeGroup = null;
                }
            }
            // End MID Track #5399
        }

        // BEGIN ANF - Weighting Multiple Basis
        private void ProcessEqualizedWeighting(ApplicationSessionTransaction aApplicationTransaction)
        {
            BasisProfile basisProfile;
            BasisDetailProfile basisDetailProfile;
            try
            {
                // Begin MID Track #5399 - JSmith - Plan In Use error
                try
                {
                    // End MID Track #5399
                    _dtEqWgtBasis = _dsForecastSpread.Tables[0].Copy();

                    foreach (DataRow eqRow in _dtEqWgtBasis.Rows)
                    {
                        eqRow["WEIGHT"] = 1; // change wgt to 1 to get raw basis  
                    }

                    FillOpenParmForWeightedBasis(Include.AllStoreGroupRID);

                    ((ChainMultiLevelPlanMaintCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

                    float totRawBasis = 0;
                    ArrayList alBasisTotal = new ArrayList();
                    //========================================================
                    // Get raw basis totals
                    //========================================================
                    for (int j = 0; j < _openParms.BasisProfileList.Count; j++)
                    {
                        basisProfile = (BasisProfile)_openParms.BasisProfileList[j];
                        for (int i = 0; i < basisProfile.BasisDetailProfileList.Count; i++)
                        {
                            basisDetailProfile = (BasisDetailProfile)basisProfile.BasisDetailProfileList[i];
                            float basisTotal = SumBasisValues(basisDetailProfile.Key, basisDetailProfile);
                            alBasisTotal.Add(basisTotal);
                            totRawBasis += basisTotal;
                        }
                    }
                    //========================================================
                    // Replace entered weight with calculated equalized weight
                    //========================================================
                    for (int k = 0; k < _dsForecastSpread.Tables[0].Rows.Count; k++)
                    {
                        if ((float)alBasisTotal[k] > 0)     // Issue 4817 stodd
                        {
                            DataRow row = _dsForecastSpread.Tables[0].Rows[k];

                            float enteredWeight = (float)Convert.ToDouble(row["WEIGHT"], CultureInfo.CurrentUICulture);

                            float equalizedWeight = (totRawBasis * enteredWeight) / (float)alBasisTotal[k];

                            row["WEIGHT"] = equalizedWeight;

                            string msg = "Weighted Basis Info: " + equalizedWeight.ToString() + " = " + " (" + totRawBasis.ToString() + " * " +
                                enteredWeight.ToString() + ") / " + alBasisTotal[k].ToString();
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());
                        }
                    }
                    //					((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                    // Begin MID Track #5399 - JSmith - Plan In Use error
                }
                catch
                {
                    throw;
                }
                finally
                {
                    ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                    _cubeGroup.Dispose();
                    _cubeGroup = null;
                }
                // End MID Track #5399

                _cubeGroup = (PlanCubeGroup)aApplicationTransaction.CreateChainMultiLevelPlanMaintCubeGroup();

                FillOpenParmForPlan();
                FillOpenParmForBasis(Include.AllStoreGroupRID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fills in the weighted basis part of the CubeGroup open parms.
        /// </summary>
        private void FillOpenParmForWeightedBasis(int sglRid)
        {
            BasisProfile basisProfile = null;
            BasisDetailProfile basisDetailProfile;
            int bdpKey = 1;
            HierarchyNodeProfile hnp = null;

            int maxRows = this._dtEqWgtBasis.Rows.Count;  //the eq weight basis detail table
            for (int row = 0; row < maxRows; row++)
            {
                //=======================
                // Set up Basis Profile
                //=======================
                int key = row;
                key++;
                basisProfile = new BasisProfile(key, null, _openParms);
                basisProfile.BasisType = eTyLyType.NonTyLy;

                //=====================
                // Get Hierarchy Node
                //=====================
                int basisHierNodeRid = this._hierNodeRid;
                hnp = _SAB.HierarchyServerSession.GetNodeData(basisHierNodeRid, true, true);

                int basisVersionRid = Convert.ToInt32(_dtEqWgtBasis.Rows[row]["FV_RID"], CultureInfo.CurrentUICulture);
                int basisDateRangeRid = Convert.ToInt32(_dtEqWgtBasis.Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);

                basisDetailProfile = new BasisDetailProfile(bdpKey++, _openParms);

                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();

                basisDetailProfile.VersionProfile = fvpb.Build(basisVersionRid);
                basisDetailProfile.HierarchyNodeProfile = hnp;
                DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basisDateRangeRid);
                basisDetailProfile.DateRangeProfile = drp;
                basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;

                if (_dtEqWgtBasis.Rows[row]["WEIGHT"] == System.DBNull.Value)
                {
                    basisDetailProfile.Weight = 1;
                }
                else
                {
                    basisDetailProfile.Weight = (float)Convert.ToDouble(_dtEqWgtBasis.Rows[row]["WEIGHT"], CultureInfo.CurrentUICulture);
                }
                basisProfile.BasisDetailProfileList.Add(basisDetailProfile);

                _openParms.BasisProfileList.Add(basisProfile);
            }
        }

        public float SumBasisValues(int aBasisNumber, BasisDetailProfile aBasisDetailProfile)
        {
            float rawBasisTotal = 0;

            Cube myCube = _cubeGroup.GetCube(eCubeType.ChainBasisLowLevelTotalDateTotal);

            PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
            planCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
            planCellRef[eProfileType.Basis] = aBasisNumber;
            planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

            if (aBasisDetailProfile.HierarchyNodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
            {
                planCellRef[eProfileType.TimeTotalVariable] = (_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable.GetTimeTotalVariable(1)).Key;
                planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable.Key;
            }
            else
            {
                planCellRef[eProfileType.TimeTotalVariable] = (_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.GetTimeTotalVariable(1)).Key;
                planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
            }

            ArrayList basisCellRefs = planCellRef.GetSpreadDetailCellRefArray(false);

            foreach (PlanCellReference pcr in basisCellRefs)
            {
                rawBasisTotal += (float)pcr.CurrentCellValue;
            }

            return rawBasisTotal;
        }
        // END ANF - Weighting Multiple Basis

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
                //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID);  //_SAB.StoreServerSession.GetStoreGroup(SG_RID);
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

            HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(this.HierNodeRID, true, true);

            //hnp.ChainSecurityProfile = new HierarchyNodeSecurityProfile(this.HierNodeRID);
            //hnp.ChainSecurityProfile.SetReadOnly();
            //ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
            //VersionProfile vp = fvpb.Build(VersionRID);
            //vp.ChainSecurity = new VersionSecurityProfile(this.VersionRID);
            //vp.ChainSecurity.SetReadOnly();

            hnp.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.HierNodeRID, (int)eSecurityTypes.Chain);
            //hnp.ChainSecurityProfile.SetReadOnly();
            hnp.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.HierNodeRID, (int)eSecurityTypes.Store);
            //hnp.ChainSecurityProfile.SetReadOnly();

            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
            VersionProfile vp = fvpb.Build(VersionRID);
            vp.ChainSecurity = _SAB.ClientServerSession.GetMyVersionSecurityAssignment(this.VersionRID, (int)eSecurityTypes.Chain);
            //vp.ChainSecurity.SetReadOnly();
            vp.StoreSecurity = _SAB.ClientServerSession.GetMyVersionSecurityAssignment(this.VersionRID, (int)eSecurityTypes.Store);
            //vp.StoreSecurity.SetReadOnly();

            _openParms.ChainHLPlanProfile.VersionProfile = vp;
            _openParms.StoreHLPlanProfile.VersionProfile = vp;

            _openParms.StoreHLPlanProfile.NodeProfile = hnp;		// ANF - Weighting Multiple Basis
            _openParms.ChainHLPlanProfile.NodeProfile = hnp;
            _openParms.DateRangeProfile = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.DateRangeRID);
            _openParms.LowLevelVersionDefault = vp;

            //_openParms.StoreGroupRID = Include.AllStoreGroupRID;	// ANF - Weighting Multiple Basis
            _openParms.StoreGroupRID = SG_RID;


            //_openParms.LowLevelsType = this.LowerLevelType; 
            //_openParms.LowLevelsOffset = this.LowerLevelOffset;
            //_openParms.LowLevelsSequence = this.LowerLevelSequence;

            if (_computationsMode != null)
            {
                _openParms.ComputationsMode = _computationsMode;
            }
            else
            {
                _openParms.ComputationsMode = _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
            }

            // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
            // BEGIN Issue 4328 - stodd 2.28.07
            //if (_multiLevel == true)
            //{
            //    PopulateOverrideList(eToLevelsType.LevelOffset, 1, 0);
            //}
            //else 
            //{
            //    PopulateOverrideList(ToLevelType, ToLevelOffset, ToLevelSequence);
            //}
            try
            {
                if (_multiLevel == true)
                {
                    PopulateOverrideList(eToLevelsType.LevelOffset, 1, 0);
                }
                else
                {
                    PopulateOverrideList(ToLevelType, ToLevelOffset, ToLevelSequence);
                }
            }
            catch (DuplicateOverrideListEntry ex)
            {
                _foundDuplicate = true;
                _duplicateMessage = ex.Message;
                return;
            }
            // End TT#2281
            // END Issue 4328

            BuildLowLevelVersionList();

            GetAuditInfo();
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

        // BEGIN Override Low Level Enhancement
        /// <summary>
        /// Populates the class's _lowlevelVersionOverrideList.
        /// </summary>
        public void PopulateOverrideList(eToLevelsType lowLevelType, int lowLevelOffset, int lowLevelSeq)
        {
            try
            {
                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                _foundDuplicate = false;
                _duplicateMessage = string.Empty;
                // End TT#2281

                // BEGIN Override Low Level Enhancement
                HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
                if (lowLevelType == eToLevelsType.LevelOffset)
                {
                    // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                    //_lowlevelVersionOverrideList = hTran.GetOverrideList(OverrideLowLevelRid, HierNodeRID, VersionRID,
                    //                                                           lowLevelOffset, Include.NoRID, true, false);
                    _lowlevelVersionOverrideList = hTran.GetOverrideListWithIgnore(OverrideLowLevelRid, HierNodeRID, VersionRID,
                                                                               lowLevelOffset, Include.NoRID, true, false, false);
                    // End TT#2281
                }
                else if (lowLevelType == eToLevelsType.HierarchyLevel)
                {
                    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(HierNodeRID);

                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    //int offset = lowLevelSeq - hnp.NodeLevel;
                    //_lowlevelVersionOverrideList = hTran.GetOverrideList(OverrideLowLevelRid, HierNodeRID, VersionRID,
                    //                                                           offset, Include.NoRID, true, false);
                    _lowlevelVersionOverrideList = hTran.GetOverrideList(OverrideLowLevelRid, HierNodeRID, VersionRID,
                                                                          eHierarchyDescendantType.levelType, lowLevelSeq, Include.NoRID, true, false);
                    // END Track #6107
                }
            }
            catch
            {
                throw;
            }
        }

        ///// <summary>
        ///// Creates a list of LowLevelVersionOverrideProfiles.
        ///// </summary>
        ///// <param name="parentNodeRid"></param>
        ///// <param name="lowLevelSeq"></param>
        ///// <returns></returns>
        //private ProfileList BuildLowLevelVersionOverrideList(int parentNodeRid, eToLevelsType lowLevelType, int lowLevelOffset, int lowLevelSeq)
        //{
        //    _lowlevelVersionOverrideList = new ProfileList(eProfileType.LowLevelVersionOverride);
        //    // Begin Issue 4328 - stodd 2.28.07
        //    if (lowLevelType == eToLevelsType.LevelOffset)
        //    {
        //        //Begin Track #4037 - JSmith - Optionally include dummy color in child list
        //        //				hnl = _SAB.HierarchyServerSession.GetDescendantData(parentNodeRid, lowLevelOffset, true);
        //        _hnl = _SAB.HierarchyServerSession.GetDescendantData(parentNodeRid, lowLevelOffset, true, eNodeSelectType.NoVirtual);  // Issue 5204
        //        //End Track #4037
        //    }
        //    else
        //    {
        //        //Begin Track #4037 - JSmith - Optionally include dummy color in child list
        //        //				hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(parentNodeRid, lowLevelSeq, true);
        //        _hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(parentNodeRid, lowLevelSeq, true, eNodeSelectType.NoVirtual);   // Issue 5204
        //        //End Track #4037
        //    }
        //    // End Issue 4328 - stodd 2.28.07

        //    foreach (HierarchyNodeProfile hnp in _hnl)	// Issue 5204
        //    {
        //        LowLevelVersionOverrideProfile lvop = new LowLevelVersionOverrideProfile(hnp.Key);
        //        lvop.NodeProfile = hnp;

        //        //if (_dtLowerLevels != null)
        //        //{
        //        //    DataRow[] rows = _dtLowerLevels.Select("HN_RID = " + hnp.Key.ToString(CultureInfo.CurrentUICulture));
        //        //    if (rows.Length > 0)
        //        //    {
        //        //        DataRow aRow = rows[0];
        //        //        int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
        //        //        eBasisIncludeExclude ieInd = (eBasisIncludeExclude)Convert.ToInt32(aRow["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
        //        //        if (versionRid == this._versionRid)
        //        //        {
        //        //            lvop.VersionIsOverridden = false;
        //        //            //Begin Track #4457 - JSmith - Add forecast versions
        //        //            //							lvop.VersionProfile = new VersionProfile(_versionRid);
        //        //            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
        //        //            lvop.VersionProfile = fvpb.Build(_versionRid);
        //        //            //End Track #4457
        //        //            lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);
        //        //        }
        //        //        else
        //        //        {
        //        //            lvop.VersionIsOverridden = true;
        //        //            //Begin Track #4457 - JSmith - Add forecast versions
        //        //            //							lvop.VersionProfile = new VersionProfile(versionRid);
        //        //            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
        //        //            lvop.VersionProfile = fvpb.Build(versionRid);
        //        //            //End Track #4457
        //        //            lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(versionRid);
        //        //        }
        //        //        if (ieInd == eBasisIncludeExclude.Exclude)
        //        //        {
        //        //            lvop.Exclude = true;
        //        //        }
        //        //        else
        //        //        {
        //        //            lvop.Exclude = false;
        //        //        }
        //        //        _lowlevelVersionOverrideList.Add(lvop);
        //        //    }
        //        //    else
        //        //    {
        //        //        lvop.VersionIsOverridden = false;
        //        //        //Begin Track #4457 - JSmith - Add forecast versions
        //        //        //						lvop.VersionProfile = new VersionProfile(_versionRid);
        //        //        ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
        //        //        lvop.VersionProfile = fvpb.Build(_versionRid);
        //        //        //End Track #4457
        //        //        lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

        //        //        lvop.Exclude = false;
        //        //        _lowlevelVersionOverrideList.Add(lvop);
        //        //    }

        //        //}
        //        //else
        //        //{
        //            lvop.VersionIsOverridden = false;
        //            //Begin Track #4457 - JSmith - Add forecast versions
        //            //					lvop.VersionProfile = new VersionProfile(_versionRid);
        //            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
        //            lvop.VersionProfile = fvpb.Build(_versionRid);
        //            //End Track #4457
        //            lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

        //            lvop.Exclude = false;
        //            _lowlevelVersionOverrideList.Add(lvop);
        //        //}
        //    }
        //    return _lowlevelVersionOverrideList;
        //}
        // END Override Low Level Enhancement


        /// <summary>
        /// Fills in the basis part of the CubeGroup open parms.
        /// </summary>
        private void FillOpenParmForBasis(int sglRid)
        {
            BasisProfile basisProfile;
            BasisProfile currentBasisProfile;
            BasisProfile staticBasisProfile = null;
            BasisDetailProfile basisDetailProfile;
            int bdpKey = 1;
            int staticBdpKey = 1;
            int bpKey = 1;
            HierarchyNodeProfile hnp = null;

            //=======================
            // Set up Basis Profile
            //=======================


            int maxRows = this._dsForecastSpread.Tables[0].Rows.Count;  //the basis detail table
            for (int row = 0; row < maxRows; row++)
            {
                int basisHierNodeRid = this._hierNodeRid;
                hnp = _SAB.HierarchyServerSession.GetNodeData(basisHierNodeRid, true, true);
                int basisVersionRid = Convert.ToInt32(_dsForecastSpread.Tables[0].Rows[row]["FV_RID"], CultureInfo.CurrentUICulture);
                int basisDateRangeRid = Convert.ToInt32(_dsForecastSpread.Tables[0].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);
                DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basisDateRangeRid);

                // This determines whether to make a new BasisProfile/BasisDetail set (done for each Dyn to Plan)
                // or add the row as a BasisDetail to the StaticBasisProfile (done for Static and Dyn to Current)
                if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
                {
                    basisProfile = new BasisProfile(bpKey++, null, _openParms);
                    _openParms.BasisProfileList.Add(basisProfile);
                    basisProfile.BasisType = eTyLyType.NonTyLy;
                    bdpKey = 1;
                    currentBasisProfile = basisProfile;
                }
                else
                {
                    if (staticBasisProfile == null)
                    {
                        staticBasisProfile = new BasisProfile(bpKey++, null, _openParms);
                        _openParms.BasisProfileList.Add(staticBasisProfile);
                    }
                    staticBasisProfile.BasisType = eTyLyType.NonTyLy;
                    bdpKey = staticBdpKey++;
                    currentBasisProfile = staticBasisProfile;
                }

                basisDetailProfile = new BasisDetailProfile(bdpKey, _openParms);
                //Begin Track #4457 - JSmith - Add forecast versions
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                basisDetailProfile.VersionProfile = fvpb.Build(basisVersionRid);
                //End Track #4457
                basisDetailProfile.HierarchyNodeProfile = hnp;
                basisDetailProfile.DateRangeProfile = drp;
                DateRangeProfile planDrp = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.DateRangeRID);
                ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(planDrp, null);
                basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
                basisDetailProfile.ForecastingInfo = new BasisDetailForecastInfo();
                basisDetailProfile.ForecastingInfo.PlanWeek = (WeekProfile)weekRange[0]; //Issue 4025
                basisDetailProfile.ForecastingInfo.OrigWeekListCount = basisDetailProfile.GetWeekProfileList(_SAB.ApplicationServerSession).Count;
                basisDetailProfile.ForecastingInfo.BasisPeriodList = _SAB.ApplicationServerSession.Calendar.GetDateRangePeriods(drp, (WeekProfile)weekRange[0]); //Issue 4025
                                                                                                                                                                 //basisDetailProfile.IncludeExclude = (eBasisIncludeExclude)Convert.ToInt32(_dsForcastSpread.Tables[1].Rows[row]["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
                if (_dsForecastSpread.Tables[0].Rows[row]["WEIGHT"] == System.DBNull.Value)
                {
                    basisDetailProfile.Weight = 1;
                }
                else
                {
                    basisDetailProfile.Weight = (float)Convert.ToDouble(_dsForecastSpread.Tables[0].Rows[row]["WEIGHT"], CultureInfo.CurrentUICulture);
                }

                currentBasisProfile.BasisDetailProfileList.Add(basisDetailProfile);
            }
        }

        private void Save()
        {
            try
            {
                PlanSaveParms planSaveParms = new PlanSaveParms();

                planSaveParms.SaveChainLowLevel = true;

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
            OTSForecastSpreadMethod newOTSForecastSpreadMethod = null;
            int maxRows;
            int basisDateRangeRid;
            // Begin Track #5912 - JSmith - Save As needs to clone custom override models
            OverrideLowLevelProfile ollp;
            int customUserRID;
            // End Track #5912

            try
            {
                newOTSForecastSpreadMethod = (OTSForecastSpreadMethod)this.MemberwiseClone();
                if (aCloneDateRanges &&
                    DateRangeRID != Include.UndefinedCalendarDateRange)
                {
                    newOTSForecastSpreadMethod.DateRangeRID = aSession.Calendar.GetDateRangeClone(DateRangeRID).Key;
                }
                else
                {
                    newOTSForecastSpreadMethod.DateRangeRID = DateRangeRID;
                }
                newOTSForecastSpreadMethod.DSForecastSpread = DSForecastSpread.Copy();
                if (aCloneDateRanges)
                {
                    maxRows = newOTSForecastSpreadMethod.DSForecastSpread.Tables["Basis"].Rows.Count;  //the basis detail table
                    for (int row = 0; row < maxRows; row++)
                    {
                        basisDateRangeRid = Convert.ToInt32(DSForecastSpread.Tables["Basis"].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);
                        newOTSForecastSpreadMethod.DSForecastSpread.Tables["Basis"].Rows[row]["CDR_RID"] = aSession.Calendar.GetDateRangeClone(basisDateRangeRid).Key;
                    }
                }
                newOTSForecastSpreadMethod.DSForecastSpread.AcceptChanges();
                //newOTSForecastSpreadMethod.DTLowerLevels = DTLowerLevels.Copy(); <---DKJ
                newOTSForecastSpreadMethod.HierNodeRID = HierNodeRID;
                newOTSForecastSpreadMethod.IgnoreLocks = IgnoreLocks;
                //newOTSForecastSpreadMethod.LowerLevelOffset = LowerLevelOffset; <---DKJ
                //newOTSForecastSpreadMethod.LowerLevelSequence = LowerLevelSequence;
                //newOTSForecastSpreadMethod.LowerLevelType = LowerLevelType;
                newOTSForecastSpreadMethod.MultiLevel = MultiLevel;
                newOTSForecastSpreadMethod.FromLevelOffset = FromLevelOffset;
                newOTSForecastSpreadMethod.FromLevelSequence = FromLevelSequence;
                newOTSForecastSpreadMethod.FromLevelType = FromLevelType;
                newOTSForecastSpreadMethod.ToLevelOffset = ToLevelOffset;
                newOTSForecastSpreadMethod.ToLevelSequence = ToLevelSequence;
                newOTSForecastSpreadMethod.ToLevelType = ToLevelType;
                newOTSForecastSpreadMethod.Method_Change_Type = eChangeType.none;
                newOTSForecastSpreadMethod.Method_Description = Method_Description;
                newOTSForecastSpreadMethod.MethodStatus = MethodStatus;
                newOTSForecastSpreadMethod.Name = Name;
                newOTSForecastSpreadMethod.SG_RID = SG_RID;
                newOTSForecastSpreadMethod.SpreadOption = SpreadOption;
                newOTSForecastSpreadMethod.User_RID = User_RID;
                newOTSForecastSpreadMethod.VersionRID = VersionRID;
                newOTSForecastSpreadMethod.Virtual_IND = Virtual_IND;
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
                    newOTSForecastSpreadMethod.CustomOLL_RID = customUserRID;
                }
                // End Track #5912
                newOTSForecastSpreadMethod.OverrideLowLevelRid = OverrideLowLevelRid;

                // BEGIN override low level enhancement
                //if (_lowlevelVersionOverrideList != null)
                //{
                //    if (_versionProfList == null)
                //    {
                //        _versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();
                //    }
                //    CopyVersionOverrideList(newOTSForecastSpreadMethod);
                //}
                // END override low level enhancement

                return newOTSForecastSpreadMethod;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // BEGIN override low level enhancement
        //private void CopyVersionOverrideList(OTSForecastSpreadMethod OTSForecastSpreadMethod)
        //{
        //    try
        //    {
        //        OTSForecastSpreadMethod._lowlevelVersionOverrideList = new ProfileList(eProfileType.LowLevelVersionOverride);
        //        foreach(LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList.ArrayList)
        //        {
        //            LowLevelVersionOverrideProfile newlvop = lvop.Copy();
        //            if (newlvop.VersionProfile == null)
        //            {
        //                newlvop.VersionProfile = (VersionProfile)_versionProfList.FindKey(_versionRid);
        //            }
        //            OTSForecastSpreadMethod._lowlevelVersionOverrideList.Add(newlvop);
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
        // END override low level enhancement

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
                FillOpenParmForPlan();
                foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                {
                    if (planProf.IncludeExclude == eBasisIncludeExclude.Include)
                    {
                        if (planProf.VersionProfile == null ||
                            !planProf.VersionProfile.ChainSecurity.AllowUpdate)
                        {
                            return false;
                        }
                        // Begin Track #5936 - JSmith - Process button disabled
                        //HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(planProf.NodeProfile.Key, (int)eSecurityTypes.Store);
                        HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(planProf.NodeProfile.Key, (int)eSecurityTypes.Chain);
                        // End Track #5936
                        if (!hierNodeSecurity.AllowUpdate)
                        {
                            return false;
                        }
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
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSSpread);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSSpread);
            }

        }

        #region "Data transport for Forecast Spread Method #RO-742"

        override public ROMethodProperties MethodGetData(bool processingApply)
        {
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

            ROMethodForecastSpreadProperties method = new ROMethodForecastSpreadProperties(
                    method: GetName.GetMethod(method: this),
                    description: Method_Description,
                    userKey: User_RID,
                    emethodType: eMethodType.ForecastSpread,
                    merchandise: GetName.GetMerchandiseName(nodeRID: HierNodeRID, SAB: SAB),
                    version: GetName.GetVersion(versionRID: VersionRID, SAB: SAB),
                    timePeriod: GetName.GetCalendarDateRange(calendarDateRID: DateRangeRID, SAB: SAB),
                    multiLevel: MultiLevel,
                    fromLevel: fromLevel,
                    toLevel: toLevel,
                    overrideLowLevel: overrideLowLevel,
                    spreadOption: SpreadOption,
                    ignoreLocks: IgnoreLocks,
                    equalizeWeighting: EqualizeWeighting,
                    basisProfile: ConvertBasisDataToList(_dsForecastSpread)
                );

            return method;
        }

        private List<ROBasisDetailProfile> ConvertBasisDataToList(DataSet dsBasis)
        {
            DataTable dtBasis = dsBasis.Tables[0];
            KeyValuePair<int, string> workKVP;
            int basisDtlCtr = 0;
            List<ROBasisDetailProfile> basisDetailProfiles = new List<ROBasisDetailProfile>();

            for (int basisDtlRowCtr = 0; basisDtlRowCtr < dtBasis.Rows.Count; basisDtlRowCtr++)
            {

                int iBasisId = Convert.ToInt32(dtBasis.Rows[basisDtlRowCtr]["DETAIL_SEQ"].ToString());

                int iVersionId = Convert.ToInt32(dtBasis.Rows[basisDtlCtr]["FV_RID"].ToString());
                workKVP = GetName.GetVersion(iVersionId, SAB);
                string sVersion = workKVP.Value;
                int iDateRangeID = Convert.ToInt32(dtBasis.Rows[basisDtlCtr]["CDR_RID"].ToString());
                if (Convert.ToInt32(dtBasis.Rows[basisDtlCtr]["CDR_RID"].ToString()) != Include.UndefinedCalendarDateRange)
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, Convert.ToInt32(dtBasis.Rows[basisDtlCtr]["CDR_RID"].ToString()));
                }
                else
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, SAB.ClientServerSession.Calendar.CurrentDate);
                }
                string sDateRange = workKVP.Value;
                string sPicture = string.Empty;
                float fWeight = float.Parse(dtBasis.Rows[basisDtlCtr]["Weight"] == DBNull.Value ? "0" : dtBasis.Rows[basisDtlCtr]["Weight"].ToString());
                bool bIsIncluded = true;

                string sIncludeButton = null;
                ROBasisDetailProfile basisDetailProfile = new ROBasisDetailProfile(iBasisId, Include.Undefined, "", iVersionId, sVersion,
                    iDateRangeID, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton);
                basisDetailProfiles.Add(basisDetailProfile);
            }
            return basisDetailProfiles;
        }


        override public bool MethodSetData(ROMethodProperties methodProperties, bool processingApply)
        {
            ROMethodForecastSpreadProperties rOMethodForecastSpreadProperties = (ROMethodForecastSpreadProperties)methodProperties;

            try
            {
                _hierNodeRid = rOMethodForecastSpreadProperties.Merchandise.Key;
                _versionRid = rOMethodForecastSpreadProperties.Version.Key;
                _dateRangeRid = rOMethodForecastSpreadProperties.TimePeriod.Key;
                MultiLevel = rOMethodForecastSpreadProperties.MultiLevel;

                _fromLevelType = (eFromLevelsType)rOMethodForecastSpreadProperties.FromLevel.LevelType;
                _fromLevelOffset = rOMethodForecastSpreadProperties.FromLevel.LevelOffset;
                _fromLevelSequence = rOMethodForecastSpreadProperties.FromLevel.LevelSequence;

                _toLevelType = (eToLevelsType)rOMethodForecastSpreadProperties.ToLevel.LevelType;
                _toLevelOffset = rOMethodForecastSpreadProperties.ToLevel.LevelOffset;
                _toLevelSequence = rOMethodForecastSpreadProperties.ToLevel.LevelSequence;

                _overrideLowLevelRid = rOMethodForecastSpreadProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                SpreadOption = rOMethodForecastSpreadProperties.SpreadOption;
                IgnoreLocks = rOMethodForecastSpreadProperties.IgnoreLocks;
                EqualizeWeighting = rOMethodForecastSpreadProperties.EqualizeWeighting;
                _dsForecastSpread = ConvertBasisListDataset(rOMethodForecastSpreadProperties.BasisProfiles);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private DataSet ConvertBasisListDataset(List<ROBasisDetailProfile> basisProfiles)
        {
            if (_dsForecastSpread == null)
            {
                CreateDataset();
            }

            _dtEqWgtBasis = _dsForecastSpread.Tables[0];

            _dtEqWgtBasis.Rows.Clear();
            _dtEqWgtBasis.AcceptChanges();

            int seq = 0;
            foreach (var basisDetailsProfile in basisProfiles)
            {
                DataRow rowDtlProf = _dtEqWgtBasis.NewRow();
                rowDtlProf["DETAIL_SEQ"] = seq;
                rowDtlProf["FV_RID"] = basisDetailsProfile.VersionId;
                rowDtlProf["CDR_RID"] = basisDetailsProfile.DateRangeId;
                rowDtlProf["Weight"] = basisDetailsProfile.Weight;

                _dtEqWgtBasis.Rows.Add(rowDtlProf);

                ++seq;
            }

            return _dsForecastSpread;
        }

        private void CreateDataset()
        {
            _dtEqWgtBasis = MIDEnvironment.CreateDataTable("Basis");

            _dtEqWgtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32")); //this column will be hidden.
            _dtEqWgtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
            _dtEqWgtBasis.Columns.Add("DateRange", System.Type.GetType("System.String"));
            _dtEqWgtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
            _dtEqWgtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Decimal"));

            _dsForecastSpread = new DataSet();
            _dsForecastSpread.Tables.Add(_dtEqWgtBasis);

            _dtEqWgtBasis.Columns["WEIGHT"].DefaultValue = 1;
        }

        #endregion

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }


}
