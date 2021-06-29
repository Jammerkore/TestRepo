using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Configuration;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
    /// <summary>
    /// Summary description for OTSRollupSpreadMethod.
    /// </summary>
    public class OTSRollupMethod : OTSPlanBaseMethod
    {
        private OTSRollupMethodData _rollupData;
        private DataSet _dsRollup;
        //private DataTable _dtLowerLevels;  <---DKJ
        private int _hierNodeRid;
        private HierarchyNodeProfile _hnp;          //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
        private int _versionRid;
        private int _dateRangeRid;
        private string _lastProcessedDateTime;
        private string _lastProcessedUser;
        private DataTable _dtEqWgtBasis;            // ANF - Weighting Multiple Basis
                                                    //		private ePlanType _planType; //Chain or Store
                                                    //		private int _storeFilterRid;
        private ProfileList _groupLevelBasisDetail;
        private ProfileList _versionProfList;
        private ProfileList _lowlevelVersionOverrideList;

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
        private int _currentSglRid;
        private HierarchyNodeList _hnl; // Issue 5204
        private System.Data.DataTable _dtBasis;  // RO-748 RDewey 

        #region Properties
        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodRollup;
            }
        }

        public DataSet DSRollup
        {
            get { return _dsRollup; }
            set { _dsRollup = value; }
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

        #endregion Properties


        public OTSRollupMethod(SessionAddressBlock SAB, int aMethodRID)
            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //: base(SAB, aMethodRID, eMethodType.Rollup)
            : base(SAB, aMethodRID, eMethodType.Rollup, eProfileType.MethodRollup)
        //End TT#523 - JScott - Duplicate folder when new folder added
        {
            _groupLevelBasisDetail = new ProfileList(eProfileType.GroupLevelFunction);
            _SAB = SAB;

            //_monitor = _SAB.MonitorForecastAppSetting;
            //_monitorFilePath = _SAB.GetMonitorFilePathAppSetting();

            if (base.Filled)
            {
                _rollupData = new OTSRollupMethodData(aMethodRID, eChangeType.populate);
                _hierNodeRid = _rollupData.HierNodeRID;
                _versionRid = _rollupData.VersionRID;
                _dateRangeRid = _rollupData.CDR_RID;
                _dsRollup = _rollupData.DSRollup;
                //_dtLowerLevels = _rollupData.DTLowerLevels;  <---DKJ
            }
            else
            {   //Defaults
                _hierNodeRid = Include.NoRID;
                _versionRid = Include.NoRID;
                _dateRangeRid = Include.NoRID;
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
            if (_rollupData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _rollupData = new OTSRollupMethodData(td, base.Key);
            }

            _rollupData.HierNodeRID = _hierNodeRid;
            _rollupData.VersionRID = _versionRid;
            _rollupData.CDR_RID = _dateRangeRid;
            _rollupData.DSRollup = _dsRollup;
            //_rollupData.DTLowerLevels = _dtLowerLevels;  <---DKJ

            try
            {
                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _rollupData.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _rollupData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _rollupData.DeleteMethod(base.Key, td);
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
                _rollupData = null;
                _dsRollup = null;
                //_dtLowerLevels = null;  <---DKJ
                _groupLevelBasisDetail = null;
                _versionProfList = null;
                _lowlevelVersionOverrideList = null;
                _cubeGroup = null;
                _openParms = null;
                _hnl = null;
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
                MIDRetail.Business.Rollup rollup;
                int batchSize = 2000;
                int concurrentProcesses = 3;
                bool includeZeroInAverage = true;
                bool honorLocks = false;
                bool zeroParentsWithNoChildren = true;
                string fileLocation = null;
                MIDTimer aTimer = new MIDTimer();
                _SAB = aSAB;
                this._applicationTransaction = aApplicationTransaction;
                _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

                _infoMsg = "Starting OTS Rollup: " + this.Name;
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
                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //string strParm = ConfigurationSettings.AppSettings["InputFile"];
                string strParm = MIDConfigurationManager.AppSettings["InputFile"];
                // End TT#1054
                if (strParm != null && strParm.Trim() != "")
                {
                    // Begin TT#1054 - JSmith - Relieve Intransit not working.
                    //fileLocation = ConfigurationSettings.AppSettings["InputFile"];
                    fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
                    // End TT#1054
                }
                else
                {
                    fileLocation = null;
                }

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //strParm = ConfigurationSettings.AppSettings["BatchSize"];
                strParm = MIDConfigurationManager.AppSettings["BatchSize"];
                // End TT#1054
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

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //strParm = ConfigurationSettings.AppSettings["ConcurrentProcesses"];
                strParm = MIDConfigurationManager.AppSettings["ConcurrentProcesses"];
                // End TT#1054
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

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //strParm = ConfigurationSettings.AppSettings["IncludeZeroInAverage"];
                strParm = MIDConfigurationManager.AppSettings["IncludeZeroInAverage"];
                // End TT#1054
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

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //strParm = ConfigurationSettings.AppSettings["HonorLocks"];
                strParm = MIDConfigurationManager.AppSettings["HonorLocks"];
                // End TT#1054
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

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //strParm = ConfigurationSettings.AppSettings["ZeroParentsWithNoChildren"];
                strParm = MIDConfigurationManager.AppSettings["ZeroParentsWithNoChildren"];
                // End TT#1054
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

                rollup = new MIDRetail.Business.Rollup(SAB, batchSize, concurrentProcesses, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren);
                rollup.DetermineRollupVariables(SAB.ClientServerSession, fileLocation, 0);
                if (rollup.NoVariablesToRoll)
                {
                    aTimer.Stop();

                    _infoMsg = MIDText.GetText(eMIDTextCode.msg_RollLevelsNotFound) + " Elasped time: " + aTimer.ElaspedTimeString;
                    aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _infoMsg, this.ToString());

                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                }
                else
                {
                    // Begin Track #6346 - JSmith - Duplicate rows after Rollup
                    //int aProcessID = (_SAB.ClientServerSession.UserRID * 1000000) + (int)eProcesses.rollup;
                    int aProcessID = (_SAB.SessionID * 1000000) + (int)eProcesses.rollup;
                    // End Track #6346
                    //Begin Track 6082 - JSmith - B OTB Rollup method zeroes out the dept's projected numbers
                    //rollup.BuildRollupRequestsFromMethod(this.HierNodeRID, this.VersionRID, this.DateRangeRID, SAB.ClientServerSession, aProcessID);
                    // Begin TT#2090 - JSmith - Rollup method not logging parameters in audit
                    //rollup.BuildRollupRequestsFromMethod(Key, SAB.ClientServerSession, aProcessID);
                    if (!rollup.BuildRollupRequestsFromMethod(Key, SAB.ClientServerSession, aProcessID))
                    {
                        _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                        return;
                    }
                    // End TT#2090
                    //End Track 6082

                    if (rollup.ScheduleRollup)
                    {
                        rollup.ProcessRollupRequests(aProcessID, SAB.ClientServerSession);
                    }
                    //_SAB.ClientServerSession.Audit.RollupAuditInfo_Add(rollup.TotalItems, batchSize, concurrentProcesses,
                    //    rollup.TotalBatches, rollup.TotalErrors);

                    aTimer.Stop();

                    _infoMsg = "Completed OTS Rollup: " + this.Name + " Elasped time: " + aTimer.ElaspedTimeString;
                    aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());

                    //Begin TT#971 - JSmith - Method incorrectly reports success
                    //_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                    if (rollup.TotalErrors == 0)
                    {
                        _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                    }
                    else
                    {
                        _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    }
                    //End TT#971

                }
                //===================== End Business Logic ===========================================================

            }
            catch
            {
                throw;
            }
        }

        // BEGIN ANF - Weighting Multiple Basis
        private void ProcessEqualizedWeighting(ApplicationSessionTransaction aApplicationTransaction)
        {
            BasisProfile basisProfile;
            BasisDetailProfile basisDetailProfile;
            try
            {
                _dtEqWgtBasis = _dsRollup.Tables[0].Copy();

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
                for (int k = 0; k < _dsRollup.Tables[0].Rows.Count; k++)
                {
                    if ((float)alBasisTotal[k] > 0)     // Issue 4817 stodd
                    {
                        DataRow row = _dsRollup.Tables[0].Rows[k];

                        float enteredWeight = (float)Convert.ToDouble(row["WEIGHT"], CultureInfo.CurrentUICulture);

                        float equalizedWeight = (totRawBasis * enteredWeight) / (float)alBasisTotal[k];

                        row["WEIGHT"] = equalizedWeight;

                        string msg = "Weighted Basis Info: " + equalizedWeight.ToString() + " = " + " (" + totRawBasis.ToString() + " * " +
                            enteredWeight.ToString() + ") / " + alBasisTotal[k].ToString();
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());
                    }
                }

                ((PlanCubeGroup)_cubeGroup).CloseCubeGroup();

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
                hnp = _SAB.HierarchyServerSession.GetNodeData(basisHierNodeRid);

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
                //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID); //_SAB.StoreServerSession.GetStoreGroup(SG_RID);
                string groupName = StoreMgmt.StoreGroup_GetName(SG_RID); //TT#1517-MD -jsobek -Store Service Optimization
                ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(SG_RID);

                auditRID = auditData.ForecastAuditForecast_Add(DateTime.Now,
                    _SAB.ApplicationServerSession.Audit.ProcessRID,
                    _SAB.ClientServerSession.UserRID,
                    _hierNodeRid,
                    _hnp.Text,                  //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
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

            // BEGIN Issue 4328 - stodd 2.28.07
            // BEGIN Issue 5204 - stodd 2.15.08
            _hnl = null;
            // END Issue 5204
            //BuildLowLevelVersionOverrideList(HierNodeRID, LowerLevelType, LowerLevelOffset, LowerLevelSequence); <---DKJ
            BuildLowLevelVersionOverrideList(HierNodeRID, eLowLevelsType.LevelOffset, 1, 0);
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
            if (lowLevelType == eLowLevelsType.LevelOffset)
            {
                //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                //				hnl = _SAB.HierarchyServerSession.GetDescendantData(parentNodeRid, lowLevelOffset, true);
                _hnl = _SAB.HierarchyServerSession.GetDescendantData(parentNodeRid, lowLevelOffset, true, eNodeSelectType.NoVirtual);  // Issue 5204
                //End Track #4037
            }
            else
            {
                //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                //				hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(parentNodeRid, lowLevelSeq, true);
                _hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(parentNodeRid, lowLevelSeq, true, eNodeSelectType.NoVirtual);   // Issue 5204
                //End Track #4037
            }
            // End Issue 4328 - stodd 2.28.07

            foreach (HierarchyNodeProfile hnp in _hnl)	// Issue 5204
            {
                LowLevelVersionOverrideProfile lvop = new LowLevelVersionOverrideProfile(hnp.Key);
                lvop.NodeProfile = hnp;

                //if (_dtLowerLevels != null)
                //{
                //    DataRow[] rows = _dtLowerLevels.Select("HN_RID = " + hnp.Key.ToString(CultureInfo.CurrentUICulture));
                //    if (rows.Length > 0)
                //    {
                //        DataRow aRow = rows[0];
                //        int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
                //        eBasisIncludeExclude ieInd = (eBasisIncludeExclude)Convert.ToInt32(aRow["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
                //        if (versionRid == this._versionRid)
                //        {
                //            lvop.VersionIsOverridden = false;
                //            //Begin Track #4457 - JSmith - Add forecast versions
                //            //							lvop.VersionProfile = new VersionProfile(_versionRid);
                //            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                //            lvop.VersionProfile = fvpb.Build(_versionRid);
                //            //End Track #4457
                //            lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);
                //        }
                //        else
                //        {
                //            lvop.VersionIsOverridden = true;
                //            //Begin Track #4457 - JSmith - Add forecast versions
                //            //							lvop.VersionProfile = new VersionProfile(versionRid);
                //            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                //            lvop.VersionProfile = fvpb.Build(versionRid);
                //            //End Track #4457
                //            lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(versionRid);
                //        }
                //        if (ieInd == eBasisIncludeExclude.Exclude)
                //        {
                //            lvop.Exclude = true;
                //        }
                //        else
                //        {
                //            lvop.Exclude = false;
                //        }
                //        _lowlevelVersionOverrideList.Add(lvop);
                //    }
                //    else
                //    {
                //        lvop.VersionIsOverridden = false;
                //        //Begin Track #4457 - JSmith - Add forecast versions
                //        //						lvop.VersionProfile = new VersionProfile(_versionRid);
                //        ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                //        lvop.VersionProfile = fvpb.Build(_versionRid);
                //        //End Track #4457
                //        lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

                //        lvop.Exclude = false;
                //        _lowlevelVersionOverrideList.Add(lvop);
                //    }

                //}
                //else
                //{
                lvop.VersionIsOverridden = false;
                //Begin Track #4457 - JSmith - Add forecast versions
                //					lvop.VersionProfile = new VersionProfile(_versionRid);
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                lvop.VersionProfile = fvpb.Build(_versionRid);
                //End Track #4457
                lvop.VersionProfile.ChainSecurity = new VersionSecurityProfile(_versionRid);

                lvop.Exclude = false;
                _lowlevelVersionOverrideList.Add(lvop);
                //}
            }
            return _lowlevelVersionOverrideList;
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

            //=======================
            // Set up Basis Profile
            //=======================
            basisProfile = new BasisProfile(1, null, _openParms);
            basisProfile.BasisType = eTyLyType.NonTyLy;

            int maxRows = this._dsRollup.Tables[0].Rows.Count;  //the basis detail table
            for (int row = 0; row < maxRows; row++)
            {
                {
                    //=====================
                    // Get Hierarchy Node
                    //=====================
                    int basisHierNodeRid = this._hierNodeRid;
                    hnp = _SAB.HierarchyServerSession.GetNodeData(basisHierNodeRid);

                    int basisVersionRid = Convert.ToInt32(_dsRollup.Tables[0].Rows[row]["FV_RID"], CultureInfo.CurrentUICulture);
                    int basisDateRangeRid = Convert.ToInt32(_dsRollup.Tables[0].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);

                    basisDetailProfile = new BasisDetailProfile(bdpKey++, _openParms);
                    //Begin Track #4457 - JSmith - Add forecast versions
                    //					basisDetailProfile.VersionProfile = new VersionProfile(basisVersionRid);
                    ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                    basisDetailProfile.VersionProfile = fvpb.Build(basisVersionRid);
                    //End Track #4457
                    basisDetailProfile.HierarchyNodeProfile = hnp;
                    DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basisDateRangeRid);
                    basisDetailProfile.DateRangeProfile = drp;

                    basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
                    //basisDetailProfile.IncludeExclude = (eBasisIncludeExclude)Convert.ToInt32(_dsRollup.Tables[1].Rows[row]["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
                    if (_dsRollup.Tables[0].Rows[row]["WEIGHT"] == System.DBNull.Value)
                        basisDetailProfile.Weight = 1;
                    else
                        basisDetailProfile.Weight = (float)Convert.ToDouble(_dsRollup.Tables[0].Rows[row]["WEIGHT"], CultureInfo.CurrentUICulture);
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
        /// <returns>
        /// A copy of the object.
        /// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
        {
            OTSRollupMethod newOTSRollupMethod = null;
            int maxRows;
            int basisDateRangeRid;

            try
            {
                newOTSRollupMethod = (OTSRollupMethod)this.MemberwiseClone();
                if (aCloneDateRanges &&
                    DateRangeRID != Include.UndefinedCalendarDateRange)
                {
                    newOTSRollupMethod.DateRangeRID = aSession.Calendar.GetDateRangeClone(DateRangeRID).Key;
                }
                else
                {
                    newOTSRollupMethod.DateRangeRID = DateRangeRID;
                }
                newOTSRollupMethod.DSRollup = DSRollup.Copy();
                // Begin Track #5869 - JSmith - When doing a Save as for a Roll up method receive an Argument Eception
                //if (aCloneDateRanges)
                //{
                //    maxRows = newOTSRollupMethod.DSRollup.Tables["Basis"].Rows.Count;  //the basis detail table
                //    for (int row=0;row<maxRows;row++)
                //    {
                //        basisDateRangeRid = Convert.ToInt32(DSRollup.Tables["Basis"].Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture);
                //        newOTSRollupMethod.DSRollup.Tables["Basis"].Rows[row]["CDR_RID"] = aSession.Calendar.GetDateRangeClone(basisDateRangeRid).Key;
                //    }
                //}
                // End Track #5869
                newOTSRollupMethod.DSRollup.AcceptChanges();
                //newOTSRollupMethod.DTLowerLevels = DTLowerLevels.Copy(); <---DKJ
                newOTSRollupMethod.HierNodeRID = HierNodeRID;
                newOTSRollupMethod.Method_Change_Type = eChangeType.none;
                newOTSRollupMethod.Method_Description = Method_Description;
                newOTSRollupMethod.MethodStatus = MethodStatus;
                newOTSRollupMethod.Name = Name;
                newOTSRollupMethod.SG_RID = SG_RID;
                newOTSRollupMethod.User_RID = User_RID;
                newOTSRollupMethod.VersionRID = VersionRID;
                newOTSRollupMethod.Virtual_IND = Virtual_IND;
                newOTSRollupMethod.Template_IND = Template_IND;

                if (_lowlevelVersionOverrideList != null)
                {
                    if (_versionProfList == null)
                    {
                        _versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();
                    }
                    CopyVersionOverrideList(newOTSRollupMethod);
                }

                return newOTSRollupMethod;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CopyVersionOverrideList(OTSRollupMethod OTSRollupMethod)
        {
            try
            {
                OTSRollupMethod._lowlevelVersionOverrideList = new ProfileList(eProfileType.LowLevelVersionOverride);
                foreach (LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList.ArrayList)
                {
                    LowLevelVersionOverrideProfile newlvop = lvop.Copy();
                    if (newlvop.VersionProfile == null)
                    {
                        newlvop.VersionProfile = (VersionProfile)_versionProfList.FindKey(_versionRid);
                    }
                    OTSRollupMethod._lowlevelVersionOverrideList.Add(newlvop);
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
                if (VersionRID == Include.NoRID ||
                    HierNodeRID == Include.NoRID)
                {
                    return false;
                }

                // BEGIN Track #5858 stodd
                HierarchyNodeSecurityProfile storeNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierNodeRID, (int)eSecurityTypes.Store);
                HierarchyNodeSecurityProfile chainNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierNodeRID, (int)eSecurityTypes.Chain);
                if (!storeNodeSecurity.AllowUpdate)
                {
                    if (!chainNodeSecurity.AllowUpdate)
                    {
                        return false;
                    }
                }
                VersionSecurityProfile storeVersionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Store);
                VersionSecurityProfile chainVersionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Chain);
                if (!storeVersionSecurity.AllowUpdate)
                {
                    if (!chainVersionSecurity.AllowUpdate)
                    {
                        return false;
                    }
                }

                //FillOpenParmForPlan();
                //foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                //{
                //    if (planProf.IncludeExclude == eBasisIncludeExclude.Include)
                //    {
                //        if (planProf.VersionProfile == null ||
                //            !planProf.VersionProfile.ChainSecurity.AllowUpdate)
                //        {
                //            return false;
                //        }
                //        HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(planProf.NodeProfile.Key, (int)eSecurityTypes.Store);
                //        if (!hierNodeSecurity.AllowUpdate)
                //        {
                //            return false;
                //        }
                //    }
                //}
                //return true;

                //VersionSecurityProfile versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Store);
                //if (!versionSecurity.AllowUpdate)
                //{
                //    return false;
                //}
                //HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierNodeRID, (int)eSecurityTypes.Store);
                //if (!hierNodeSecurity.AllowUpdate)
                //{
                //    return false;
                //}
                // End Tack #5858
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
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalRollup);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserRollup);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            // Begin RO-748 RDewey
            eROLevelsType levelType;
            eHierarchyDescendantType hierarchyDescendantType;

            ROMethodRollupProperties method = new ROMethodRollupProperties(
                method: GetName.GetMethod(method: this),
                description: Method_Description,
                userKey: User_RID,
                merchandise: GetName.GetMerchandiseName(nodeRID: _hierNodeRid, SAB: SAB),
                version: GetName.GetVersion(versionRID: _versionRid, SAB: SAB),
                dateRange: GetName.GetCalendarDateRange(calendarDateRID: _dateRangeRid, SAB: SAB),
                methodRollupBasisOptions: new System.Collections.Generic.List<ROMethodRollupOptionsBasis>(),
                isTemplate: Template_IND
                );

            for (int k = 0; k < _dsRollup.Tables[0].Rows.Count; k++)
            {
                DataRow row = _dsRollup.Tables[0].Rows[k];

                ROLevelInformation fromLevelInformation = new ROLevelInformation();
                fromLevelInformation.LevelSequence = Convert.ToInt32(row["FROM_LEVEL_SEQ"]);
                hierarchyDescendantType = (eHierarchyDescendantType)Convert.ToInt32(row["FROM_LEVEL_TYPE"]);
                levelType = EnumTools.ConverteHierarchyDescendantTypeToeROLevelsType(hierarchyDescendantType);

                if (levelType == eROLevelsType.Characteristic)
                {
                    levelType = eROLevelsType.HierarchyLevel;
                }
                fromLevelInformation.LevelType = levelType;
                try
                {
                    fromLevelInformation.LevelOffset = Convert.ToInt32(row["FROM_LEVEL_OFFSET"]);
                }
                catch
                {
                    fromLevelInformation.LevelOffset = -1;
                }

                fromLevelInformation.LevelValue = GetName.GetLevelName(levelType, fromLevelInformation.LevelSequence, fromLevelInformation.LevelOffset, SAB);

                ROLevelInformation toLevelInformation = new ROLevelInformation();

                toLevelInformation.LevelSequence = Convert.ToInt32(row["TO_LEVEL_SEQ"]);
                hierarchyDescendantType = (eHierarchyDescendantType)Convert.ToInt32(row["TO_LEVEL_TYPE"]);
                levelType = EnumTools.ConverteHierarchyDescendantTypeToeROLevelsType(hierarchyDescendantType);
                if (levelType == eROLevelsType.Characteristic)
                {
                    levelType = eROLevelsType.HierarchyLevel;
                }
                toLevelInformation.LevelType = levelType;
                try
                {
                    toLevelInformation.LevelOffset = Convert.ToInt32(row["TO_LEVEL_OFFSET"]);
                }
                catch
                {
                    toLevelInformation.LevelOffset = -1;
                }
                toLevelInformation.LevelValue = GetName.GetLevelName(levelType, toLevelInformation.LevelSequence, toLevelInformation.LevelOffset, SAB);
                ROMethodRollupOptionsBasis options = new ROMethodRollupOptionsBasis(
                    optionsDetailSeq: Convert.ToInt32(row["DETAIL_SEQ"]),
                    fromMerchandise: GetName.GetMerchandiseName(Convert.ToInt32(row["FROM_LEVEL_HRID"]), SAB),
                    fromROLevelInformation: fromLevelInformation,
                    toMerchandise: GetName.GetMerchandiseName(Convert.ToInt32(row["TO_LEVEL_HRID"]), SAB),
                    toROLevelInformation: toLevelInformation,
                    isStore: Convert.ToBoolean(row["Store"]),
                    isChain: Convert.ToBoolean(row["Chain"]),
                    isStoreToChain: Convert.ToBoolean(row["StoreToChain"])
                    );
                method.MethodRollupBasisOptions.Add(options);
            }

            BuildVersionList(method: method);

            if (_hierNodeRid > 0)
            {
                BuildLowLevelList(method: method);
            }

            return method;
        }

        private void BuildVersionList(ROMethodRollupProperties method)
        {
            ProfileList storeVersionList = GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain | eSecurityTypes.Store, false, _versionRid, false);
            foreach (VersionProfile versionProfile in storeVersionList)
            {
                method.Versions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }
        }

        private void BuildLowLevelList(ROMethodRollupProperties method)
        {
            eMerchandiseType merchandiseType;
            int homeHierarchyKey;
            List<HierarchyLevelComboObject> levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: _hierNodeRid,
                includeHomeLevel: true,
                includeLowestLevel: true,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            method.HierarchyLevelsType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    method.HierarchyLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    method.HierarchyLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodRollupProperties roMethodRollupProperties = (ROMethodRollupProperties)methodProperties;
            try
            {
                Template_IND = methodProperties.IsTemplate;
                _versionRid = roMethodRollupProperties.Version.Key;
                _dateRangeRid = roMethodRollupProperties.DateRange.Key;
                _hierNodeRid = roMethodRollupProperties.Merchandise.Key;

                // The stored procedure will delete all the BASIS_DETAIL rows and re-add them.
                if (_dsRollup != null)
                {
                    _dsRollup.Tables[0].Rows.Clear();
                }
                else
                {
                    LoadBasis();
                }

                DataRow row;
                int intDetailSeq = -1;

                foreach (ROMethodRollupOptionsBasis optionsBasis in roMethodRollupProperties.MethodRollupBasisOptions)
                {
                    row = _dsRollup.Tables[0].NewRow();
                    intDetailSeq += 1;
                    row["DETAIL_SEQ"] = intDetailSeq;
                    row["FromLevel"] = optionsBasis.FromROLevelInformation.LevelValue;
                    row["FROM_LEVEL_HRID"] = optionsBasis.FromMerchandise.Key;
                    row["FROM_LEVEL_TYPE"] = EnumTools.ConverteROLevelsTypeToeHierarchyDescendantType(optionsBasis.FromROLevelInformation.LevelType);
                    row["FROM_LEVEL_SEQ"] = optionsBasis.FromROLevelInformation.LevelSequence;
                    row["FROM_LEVEL_OFFSET"] = optionsBasis.FromROLevelInformation.LevelOffset;
                    row["ToLevel"] = optionsBasis.ToROLevelInformation.LevelValue;
                    row["TO_LEVEL_HRID"] = optionsBasis.ToMerchandise.Key;
                    row["TO_LEVEL_TYPE"] = EnumTools.ConverteROLevelsTypeToeHierarchyDescendantType(optionsBasis.ToROLevelInformation.LevelType);
                    row["TO_LEVEL_SEQ"] = optionsBasis.ToROLevelInformation.LevelSequence;
                    row["TO_LEVEL_OFFSET"] = optionsBasis.ToROLevelInformation.LevelOffset;
                    row["Store"] = optionsBasis.IsStore;
                    row["Chain"] = optionsBasis.IsChain;
                    row["StoreToChain"] = optionsBasis.IsStoreToChain;
                    row["STORE_IND"] = Convert.ToInt16(optionsBasis.IsStore);
                    row["CHAIN_IND"] = Convert.ToInt16(optionsBasis.IsChain);
                    row["STORE_TO_CHAIN_IND"] = Convert.ToInt16(optionsBasis.IsStoreToChain);
                    _dsRollup.Tables[0].Rows.Add(row);
                }

                return true;
            }
            catch
            {
                return false;
            }

            //throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey

        // Begin MID Track 5852 - KJohnson - Security changes
        public bool StoreAuthorizedToUpdate()
        {
            try
            {
                if (VersionRID == Include.NoRID || HierNodeRID == Include.NoRID)
                {
                    return false;
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
        public bool ChainAuthorizedToUpdate()
        {
            try
            {
                if (VersionRID == Include.NoRID || HierNodeRID == Include.NoRID)
                {
                    return false;
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

        // Begin RO-748 RDewey - Data Transport For Rollup Method - Need to define Basis DataTable when creating new Rollup Methods
        private void LoadBasis()
        {
                _dtBasis = MIDEnvironment.CreateDataTable("Basis");

                _dtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FromLevel", System.Type.GetType("System.Object"));
                _dtBasis.Columns.Add("FROM_LEVEL_HRID", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FROM_LEVEL_TYPE", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FROM_LEVEL_SEQ", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FROM_LEVEL_OFFSET", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("ToLevel", System.Type.GetType("System.Object"));
                _dtBasis.Columns.Add("TO_LEVEL_HRID", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("TO_LEVEL_TYPE", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("TO_LEVEL_SEQ", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("TO_LEVEL_OFFSET", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("Store", System.Type.GetType("System.Boolean"));
                _dtBasis.Columns.Add("Chain", System.Type.GetType("System.Boolean"));
                _dtBasis.Columns.Add("StoreToChain", System.Type.GetType("System.Boolean"));
                _dtBasis.Columns.Add("STORE_IND", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("CHAIN_IND", System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("STORE_TO_CHAIN_IND", System.Type.GetType("System.Int32")); //this column will be hidden.
                                                                   
                if (_dsRollup == null)
                { 
                    _dsRollup = new DataSet();
                    _dsRollup.Tables.Add(_dtBasis);
                }
        }
        // End RO-748 RDewey
    }
}
