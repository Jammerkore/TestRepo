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
using System.Reflection;


namespace MIDRetail.Business
{
    /// <summary>
    /// Summary description for OTSForecastModifySales.
    /// </summary>
    public class OTSForecastModifySales : OTSPlanBaseMethod
    {
        private int _hierNodeRid;
        private int _dateRangeRid;
        private int _filter;
        private eStoreAverageBy _averageBy;
        private DataTable _dtGrades;
        private DataTable _dtSellThru;
        private DataTable _dtMatrixRules;
        private SessionAddressBlock _sab;
        private ForecastModifySalesMethodData _modifySalesData;
        private ApplicationSessionTransaction _applicationTransaction;
        private string _computationsMode = "Default";
        private int _nodeOverrideRid = Include.NoRID;
        private string _infoMsg;
        //private ForecastCubeGroup _cubeGroup;
        private PlanCubeGroup _cubeGroup;
        private PlanOpenParms _openParms;
        private HierarchyNodeProfile _hnp;
        private ProfileList _weekList;
        private ModelVariableProfile _salesVariable;        // Will either be TOTAL or SALES R/P  
        private ModelVariableProfile _salesStockRatioVariable;
        //private ModelVariableProfile	_averageStoreVariable;
        private ModelVariableProfile _sellThruVariable;
        private ModelVariableProfile _salesTotalVariable;
        private ModelVariableProfile _salesRegVariable;
        //private ModelVariableProfile	_salesRegPromoVariable;
        private ModelVariableProfile _salesPromoVariable;
        private ModelVariableProfile _salesMkdnVariable;
        private ModelVariableProfile _stockVariable;


        private ProfileList _allStoreList;
        private ProfileList _setStoreList;
        private ProfileList _StoresNotProcessedList;
        private ProfileList _storeList;
        StoreGradeList _storeGradeList;
        private StoreWeekEligibilityList _storeEligibilty;
        private double[] _sellThruList;
        private ArrayList _modSalesRegValueList;
        private ArrayList _modSalesTotValueList;
        private ArrayList _modPromoValueList;
        private ArrayList _modMkdnValueList;
        private ArrayList _basisSalesValueList;
        private ArrayList _basisSalesRegValueList;
        private ArrayList _basisPromoValueList;
        private ArrayList _basisMkdnValueList;
        private ArrayList _basisStockValueList;
        private ArrayList _basisSellThruValueList;
        private ArrayList _salesStockRatioValueList;
        private double _avgStoreValue = 0.0d;
        private double _avgStoreSalesStockRatioValue = 0.0d;
        private bool _salesStockRatioNeeded;
        //private bool _avgStoreNeeded;
        private bool _avgStoreSSNeeded;
        //private bool _basisSalesNeeded;
        private BasicSpread _spreader = new BasicSpread();
        private Hashtable _storeGrades = null;
        private Hashtable _sglStoreCountsCompleted = null;
        private StoreCounter _storeCounter = null;


        //===================================
        // Logging Info
        //===================================
        private bool _monitor;
        private string _monitorFilePath;
        private ModifySalesLog _log;


        private int _currGradeBoundary;
        private int _currSellThruBoundary;
        private eModifySalesRuleType _currRule;
        private double _currRuleQty;
        private Hashtable _storeToIndexHash = null;
        private ArrayList _storeRule;
        private ArrayList _storeRuleQty;




        #region Properties
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodModifySales;
            }
        }

        public int HierNodeRid
        {
            get { return _hierNodeRid; }
            set { _hierNodeRid = value; }
        }
        public int DateRangeRid
        {
            get { return _dateRangeRid; }
            set { _dateRangeRid = value; }
        }
        public int Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }
        public eStoreAverageBy AverageBy
        {
            get { return _averageBy; }
            set { _averageBy = value; }
        }
        public DataTable GradesDataTable
        {
            get { return _dtGrades; }
            set { _dtGrades = value; }
        }
        public DataTable SellThruDataTable
        {
            get { return _dtSellThru; }
            set { _dtSellThru = value; }
        }
        public DataTable MatrixDataTable
        {
            get { return _dtMatrixRules; }
            set { _dtMatrixRules = value; }
        }

        public bool MONITOR
        {
            get { return _monitor; }
            set { _monitor = value; }
        }
        public string MonitorFilePath
        {
            get { return _monitorFilePath; }
            set { _monitorFilePath = value; }
        }




        #endregion

        public OTSForecastModifySales(SessionAddressBlock SAB, int aMethodRID) : base(SAB,
            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //aMethodRID, eMethodType.ForecastModifySales)
            aMethodRID, eMethodType.ForecastModifySales, eProfileType.MethodModifySales)
        //End TT#523 - JScott - Duplicate folder when new folder added
        {
            _sab = SAB;
            _modifySalesData = new ForecastModifySalesMethodData(aMethodRID, eChangeType.populate);


            //			_monitor = _sab.MonitorForecastAppSetting;
            //			_monitorFilePath = _sab.GetMonitorFilePathAppSetting();
            _monitor = _sab.ClientServerSession.UserOptions.ModifySalesMonitorIsActive;
            _monitorFilePath = _sab.ClientServerSession.UserOptions.ModifySalesMonitorDirectory;

            if (base.Filled)
            {
                _hierNodeRid = _modifySalesData.HierNodeRID;
                _dateRangeRid = _modifySalesData.CDR_RID;
                _filter = _modifySalesData.Filter;
                _averageBy = _modifySalesData.AverageBy;
                _dtGrades = _modifySalesData.GradeDataTable;
                _dtSellThru = _modifySalesData.SellThruDataTable;
                _dtMatrixRules = _modifySalesData.MatrixDataTable;


            }
            else
            {   //Defaults
                _hierNodeRid = Include.NoRID;
                _dateRangeRid = Include.UndefinedCalendarDateRange;
                _filter = Include.UndefinedStoreFilter;
                _averageBy = eStoreAverageBy.None;
                _dtGrades = _modifySalesData.GetGrades(Include.NoRID);
                _dtSellThru = _modifySalesData.GetSellThru(Include.NoRID);
                _dtMatrixRules = _modifySalesData.GetMatrix(Include.NoRID);

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

                        if (_hierNodeRid == Include.NoRID)
                        {
                            _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanHierarchyNodeMissing, this.ToString());
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_sab.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _sab.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            throw new MIDException(eErrorLevel.severe,
                                (int)eMIDTextCode.msg_pl_PlanHierarchyNodeMissing,
                                MIDText.GetText(eMIDTextCode.msg_pl_PlanHierarchyNodeMissing));
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
            }
            catch
            {
                aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
            }
            finally
            {
                _allStoreList = null;
                _setStoreList = null;
                _StoresNotProcessedList = null;
                _storeList = null;
                _storeGradeList = null;
                _storeEligibilty = null;
                _modSalesRegValueList = null;
                _modSalesTotValueList = null;
                _modPromoValueList = null;
                _modMkdnValueList = null;
                _basisSalesValueList = null;
                _basisSalesRegValueList = null;
                _basisPromoValueList = null;
                _basisMkdnValueList = null;
                _basisStockValueList = null;
                _basisSellThruValueList = null;
                _salesStockRatioValueList = null;
                _storeGrades = null;
                _sglStoreCountsCompleted = null;
                _storeRule = null;
                _storeRuleQty = null;
                _storeToIndexHash = null;
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
                _sab = aSAB;
                _applicationTransaction = aApplicationTransaction;
                _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

                // Begin MID Track 4858 - JSmith - Security changes
                // Need to check here incase part of workflow
                if (!AuthorizedToUpdate(_sab.ApplicationServerSession, _sab.ClientServerSession.UserRID))
                {
                    _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, this.ToString());
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_sab.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _sab.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    return;
                }
                // End MID Track 4858

                //==============================================================
                // Override store filter with the one coming in from workflow.
                //==============================================================
                if (aStoreFilterRID == Include.NoRID || aStoreFilterRID == Include.UndefinedStoreFilter)
                {
                }
                else
                    _filter = aStoreFilterRID;

                _infoMsg = "Starting OTS Forecast Modify Sales: " + this.Name;
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());
                aTimer.Start();

                //=====================
                // Get Range of weeks
                //=====================
                DateRangeProfile drp = _sab.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);
                _weekList = _sab.ApplicationServerSession.Calendar.GetWeekRange(drp, null);
                _storeGradeList = BuildStoreGradeList();
                _sellThruList = GetSellThruList();
                //TT#753-754 - MD - Log informational message added to audit - RBeck
                //_hnp = _sab.HierarchyServerSession.GetNodeData(this._hierNodeRid);
                _hnp = _sab.HierarchyServerSession.GetNodeData(this._hierNodeRid, true, true);
                //TT#753-754 - MD - Log informational message added to audit - RBeck
                _sglStoreCountsCompleted = new Hashtable();
                _storeCounter = new StoreCounter();


                //========================
                // Begin Monitor Logging
                //========================
                if (MONITOR)
                {
                    try
                    {

                        //Begin TT#339 - MD - Modify Forecast audit message - RBeck
                        string qualifiedNodeID = _hnp.QualifiedNodeID;
                        string fileName = "ModifySales_";
                        //string fileName = "ModifySales" + this.Key.ToString(CultureInfo.CurrentUICulture) + "_" +
                        //                  _hnp.NodeID;
                        //_log = new ModifySalesLog(_sab, fileName, _monitorFilePath, _sab.ClientServerSession.UserRID, this.Name, this.Key);                        
                        _log = new ModifySalesLog(_sab, fileName, _monitorFilePath, _sab.ClientServerSession.UserRID, this.Name, qualifiedNodeID);
                        //End  TT#339 - MD - Modify Forecast audit message - RBeck    

                        _log.WriteLine("Hierarchy Node Desc: " + _hnp.NodeDescription +
                            "  Node ID: " + _hnp.NodeID + "  " +
                            "(" + _hnp.Key.ToString(CultureInfo.CurrentUICulture) + ")");
                        string displayDate = _sab.ApplicationServerSession.Calendar.GetDisplayDate(drp);
                        _log.WriteLine("Date Range:		  " + displayDate + "(" + _dateRangeRid.ToString(CultureInfo.CurrentUICulture) + ")");
                        //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID); //_sab.StoreServerSession.GetStoreGroup(SG_RID);
                        string groupName = StoreMgmt.StoreGroup_GetName(SG_RID); //TT#1517-MD -jsobek -Store Service Optimization
                        _log.WriteLine("Attribute:        " + groupName + "(" + SG_RID.ToString(CultureInfo.CurrentUICulture) + ")");
                        _log.WriteLine("Average By:       " + AverageBy.ToString());

                        _log.WriteGradesAndSellThru(_storeGradeList, _sellThruList);
                        _log.WriteFomulas();
                    }
                    catch
                    {
                        MONITOR = false;
                        string errMsg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_pl_ForecastMonitoringProblem));

                        System.Windows.Forms.DialogResult diagResult = SAB.MessageCallback.HandleMessage(
                            errMsg,
                            "Problem Creating Modify Sales Log File",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                }

                if (_filter == Include.UndefinedStoreFilter)
                    ProcessWithoutFilter();
                else
                    ProcessWithFilter();


                aTimer.Stop();

                _infoMsg = "Completed OTS Forecast Modify: " + this.Name + " " +
                    "Elasped time: " + aTimer.ElaspedTimeString;
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());

                _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;

                if (MONITOR)
                {
                    _log.CloseLogFile();
                }

                WriteAuditInfo();


            }
            catch (PlanInUseException)
            {
            }
            catch
            {
                throw;
            }
        }


        private void ProcessWithoutFilter()
        {
            try
            {
                _storeList = GetStoreList();  // Also applies store filter

                //===================
                // Build Cube
                //===================
                // Begin MID Track #5210 - JSmith - Out of memory
                //				_cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup(true);  // Issue 4364 - stodd
                _cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                // End MID Track #5210
                _openParms = FillOpenParmForPlan(Include.FV_ModifiedRID, null);
                FillOpenParmForBasis(Include.AllStoreGroupRID, null);  // 4575

                DeleteStoreValues();

                ((ForecastCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

                SetVariables();

                //===========================================
                // Gets store group levels that have rules
                //===========================================
                ArrayList sglList = GetStoreGroupLevelList();
                //=================================
                // Process Rules for a single set
                //=================================
                foreach (int sglRid in sglList)
                {
                    //============================
                    // Get Rules for the set
                    //============================
                    DataRow[] ruleRows = _dtMatrixRules.Select("SGL_RID = " + sglRid.ToString());
                    //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sglRid); //_sab.StoreServerSession.GetStoreGroupLevel(sglRid);
                    string levelName = StoreMgmt.StoreGroupLevel_GetName(sglRid); //TT#1517-MD -jsobek -Store Service Optimization

                    if (MONITOR)
                    {
                        _log.WriteLine("");
                        _log.WriteLine("Rules for Attribute Set: " + levelName + "(" + sglRid.ToString(CultureInfo.CurrentUICulture) + ")");

                        foreach (DataRow row in ruleRows)
                        {
                            int grade = Convert.ToInt32(row["BOUNDARY"], CultureInfo.CurrentUICulture);
                            int sellThru = Convert.ToInt32(row["SELL_THRU"], CultureInfo.CurrentUICulture);
                            eModifySalesRuleType rule = (eModifySalesRuleType)Convert.ToInt32(row["MATRIX_RULE"], CultureInfo.CurrentUICulture);
                            double ruleQty = Convert.ToDouble(row["MATRIX_RULE_QUANTITY"], CultureInfo.CurrentUICulture);
                            _log.WriteLine("  Grade: " + grade.ToString(CultureInfo.CurrentUICulture) +
                                "  SellThru: " + sellThru.ToString(CultureInfo.CurrentUICulture) +
                                "  Rule: " + rule.ToString() +
                                "  RuleQty: " + ruleQty.ToString(CultureInfo.CurrentUICulture));
                        }
                        _log.WriteLine("");
                    }

                    //============
                    // Setup
                    //============
                    ProfileList storeList = this.ApplySetFilter(sglRid, _storeList);
                    SetValuesNeededSwitches(sglRid);

                    //==============================
                    // Process each week requested
                    //==============================
                    foreach (WeekProfile aWeek in _weekList.ArrayList)
                    {
                        if (MONITOR)
                        {
                            string msg = "Attribute Set: " + levelName + "(" + sglRid.ToString(CultureInfo.CurrentUICulture) + ")" +
                                "  Week: " + aWeek.YearWeek.ToString(CultureInfo.CurrentUICulture);
                            _log.WriteLine("===================================================");
                            _log.WriteLine(msg);
                            _log.WriteLine("===================================================");
                        }

                        CoreProcessing(storeList, aWeek, sglRid, ruleRows);

                        if (!_sglStoreCountsCompleted.ContainsKey(sglRid))
                        {
                            _sglStoreCountsCompleted.Add(sglRid, true);
                        }
                    }
                }

                _storeCounter.Debug();

                if (sglList.Count > 0)   // Issue 4323 stodd 2.21.07
                    Save();

                // Begin MID Track #5399 - JSmith - Plan In Use error
                //				// Cleanup & dequeue
                //				// Begin MID Track #5210 - JSmith - Out of memory
                //				((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
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

        private void ProcessWithFilter()
        {
            bool variablesSet = false;
            try
            {
                foreach (WeekProfile aWeek in _weekList.ArrayList)
                {
                    // Begin Track #6225 - JSmith - Plan In Use error
                    try
                    {
                        // End Track #6225
                        _storeList = GetStoreList();
                        _storeList = ApplyStoreFilter(aWeek);

                        if (_storeList.Count == 0) // Issue 4535 - 8.1.07
                        {
                            _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_pl_EmptyStoreListFromFilter), this.ToString());
                        }
                        else
                        {
                            if (MONITOR)
                            {
                                string msg = "  Week: " + aWeek.YearWeek.ToString(CultureInfo.CurrentUICulture);
                                _log.WriteLine("===================================================");
                                _log.WriteLine(msg);
                                _log.WriteLine("===================================================");
                                FilterData storeFilterData = new FilterData();
                                string filterName = storeFilterData.FilterGetName(_filter);
                                _log.WriteLine("Store Filter:     " + filterName + "(" + _filter.ToString(CultureInfo.CurrentUICulture) + ")");
                                if (_filter != Include.UndefinedStoreFilter)
                                {
                                    _log.WriteLine("STORES in FILTER:");
                                    foreach (StoreProfile aStore in _storeList.ArrayList)
                                    {
                                        _log.Write(aStore.StoreId + "(" + aStore.Key.ToString(CultureInfo.CurrentUICulture) + ")" + "\t");
                                    }
                                    _log.Write("\r");
                                }
                            }

                            // Begin MID Track #5210 - JSmith - Out of memory
                            //						_cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup(true);  // Issue 4364 - stodd
                            _cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                            // End MID Track #5210
                            _openParms = FillOpenParmForPlan(Include.FV_ModifiedRID, aWeek);  // 4575
                            FillOpenParmForBasis(Include.AllStoreGroupRID, aWeek);

                            // Begin Track #6325 - stodd - rerunning method caused values to disappear
                            //=======================
                            // Deletes store values
                            //=======================
                            DeleteStoreValues(aWeek);
                            // End Track #6325 - stodd - rerunning method caused values to disappear

                            ((ForecastCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

                            if (!variablesSet)
                            {
                                SetVariables();
                                variablesSet = true;
                            }

                            ArrayList sglList = GetStoreGroupLevelList();
                            //=================================
                            // Process Rules for a single set
                            //=================================
                            foreach (int sglRid in sglList)
                            {
                                //============================
                                // Get Rules for the set
                                //============================
                                DataRow[] ruleRows = _dtMatrixRules.Select("SGL_RID = " + sglRid.ToString());
                                //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sglRid); //_sab.StoreServerSession.GetStoreGroupLevel(sglRid);
                                string levelName = StoreMgmt.StoreGroupLevel_GetName(sglRid); //TT#1517-MD -jsobek -Store Service Optimization

                                if (MONITOR)
                                {
                                    string msg = "Attribute Set: " + levelName + "(" + sglRid.ToString(CultureInfo.CurrentUICulture) + ")" +
                                        "  Week: " + aWeek.YearWeek.ToString(CultureInfo.CurrentUICulture);
                                    _log.WriteLine("===================================================");
                                    _log.WriteLine(msg);
                                    _log.WriteLine("===================================================");
                                }

                                ProfileList storeList = this.ApplySetFilter(sglRid, _storeList);
                                SetValuesNeededSwitches(sglRid);

                                CoreProcessing(storeList, aWeek, sglRid, ruleRows);

                                if (!_sglStoreCountsCompleted.ContainsKey(sglRid))
                                {
                                    _sglStoreCountsCompleted.Add(sglRid, true);
                                }

                            }

                            _storeCounter.Debug();

                            Save();

                            // Begin MID Track #5399 - JSmith - Plan In Use error
                            //						// Cleanup & dequeue
                            //						// Begin MID Track #5210 - JSmith - Out of memory
                            //						((PlanCubeGroup)_cubeGroup).CloseCubeGroup();
                            //						_cubeGroup.Dispose();
                            //						_cubeGroup = null;
                            //						// End MID Track #5210
                            // End MID Track #5399
                        }
                        // Begin Track #6225 - JSmith - Plan In Use error
                    }
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
                    // End Track #6225
                }
            }
            catch
            {
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

        private void CoreProcessing(ProfileList storeList, WeekProfile aWeek, int sglRid, DataRow[] ruleRows)
        {
            try
            {
                // Begin Issue 4288
                BuildStoreToIndexHashtable(storeList);
                _storeRule = new ArrayList();
                for (int i = 0; i < storeList.Count; i++)
                {
                    _storeRule.Add(eModifySalesRuleType.None);
                }
                _storeRuleQty = new ArrayList();
                for (int i = 0; i < storeList.Count; i++)
                {
                    _storeRuleQty.Add(0.0d);
                }
                // End issue 4288

                _storeEligibilty = _applicationTransaction.GetStoreEligibilityForSales(this._hierNodeRid, aWeek.Key);

                //==================================
                // Read store values for week
                //==================================
                Read(storeList, aWeek, sglRid);

                if (MONITOR)
                {
                    _log.WriteLine("Avg Store Value: " + _avgStoreValue.ToString(CultureInfo.CurrentUICulture));
                    _log.WriteLine("Avg Store S/S Ratio: " + _avgStoreSalesStockRatioValue.ToString(CultureInfo.CurrentUICulture));
                    _log.WriteCaptions();
                    //_log.Test();
                }

                //=====================================================================================
                // Build store grade list for stores
                //=====================================================================================
                // If average by set then store grades are figured for each set.
                // if all store average then store grades are figured for using all store average. 
                //=====================================================================================
                // BEGIN Issue 4575 stodd 08.13.07
                //				if (_averageBy == eStoreAverageBy.Set) 
                //					_storeGrades = GetStoreGrades(_storeGradeList, _setStoreList, aWeek);
                //				else
                //				{
                //					if (_storeGrades == null)
                //						_storeGrades = GetStoreGrades(_storeGradeList, _allStoreList, aWeek);
                //				}

                _storeGrades = GetStoreGrades(_storeGradeList, _allStoreList, aWeek);
                // END Issue 4575 stodd 08.13.07

                StoreCounter storeCounter = new StoreCounter();

                //================================================================================
                // If we haven't already done it, gather the store count info for this 
                // store group level. This entire method is executed for each week and we only do
                // the store counts for the first week.
                //================================================================================
                if (!_sglStoreCountsCompleted.ContainsKey(sglRid))
                {
                    for (int s = 0; s < storeList.Count; s++)
                    {
                        StoreProfile sp = (StoreProfile)storeList[s];
                        if (((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(sp.Key)).StoreIsEligible)
                        {
                            // Find store sell thru boundary
                            PlanCellReference pcr = (PlanCellReference)_basisSellThruValueList[s];
                            double storeSellThruValue = pcr.HiddenCurrentCellValue;
                            double storeSellThruBoundary = GetStoreSellThruBoundary(storeSellThruValue);
                            int storeGrade = (int)_storeGrades[storeList[s].Key];
                            _storeCounter.Add(sglRid, storeGrade, storeSellThruBoundary);
                        }
                    }
                }

                //=================================================================================
                // MAIN PROCESSING
                // For each rule, march through the stores and see if the store has a match
                // on Grade Boundary and Sell Thru Boundary -- If so, apply rule
                //=================================================================================
                foreach (DataRow row in ruleRows)
                {
                    int rowSglRid = Convert.ToInt32(row["SGL_RID"], CultureInfo.CurrentUICulture);
                    _currGradeBoundary = Convert.ToInt32(row["BOUNDARY"], CultureInfo.CurrentUICulture);
                    _currSellThruBoundary = Convert.ToInt32(row["SELL_THRU"], CultureInfo.CurrentUICulture);
                    _currRule = (eModifySalesRuleType)Convert.ToInt32(row["MATRIX_RULE"], CultureInfo.CurrentUICulture);
                    _currRuleQty = Convert.ToDouble(row["MATRIX_RULE_QUANTITY"], CultureInfo.CurrentUICulture);

                    for (int s = 0; s < storeList.Count; s++)
                    {
                        StoreProfile sp = (StoreProfile)storeList[s];
                        if (((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(sp.Key)).StoreIsEligible)
                        {
                            // Find store sell thru boundary
                            PlanCellReference pcr = (PlanCellReference)_basisSellThruValueList[s];
                            double storeSellThruIndex = pcr.HiddenCurrentCellValue;
                            double storeSellThruBoundary = GetStoreSellThruBoundary(storeSellThruIndex);
                            int storeGrade = (int)_storeGrades[storeList[s].Key];

                            if (storeGrade == _currGradeBoundary && storeSellThruBoundary == _currSellThruBoundary)
                            {
                                // Begin Issue 4288 - stodd
                                _storeRule[s] = _currRule;
                                _storeRuleQty[s] = _currRuleQty;
                                // End Issue 4288 - stodd

                                bool storeProcessed = ProcessRuleForStore(storeList, s, aWeek, _currRule, _currRuleQty);
                                if (storeProcessed && MONITOR)
                                {
                                    WriteLogDetail(storeList, s);
                                    StoreProfile delSp = (StoreProfile)_StoresNotProcessedList.FindKey(sp.Key);
                                    if (delSp != null)
                                        _StoresNotProcessedList.Remove(delSp);
                                }
                            }
                        }
                    }
                }
                //=======================================================
                // Handle Unprocessed Stores if logging is turned on
                //=======================================================
                if (MONITOR)
                {
                    _log.WriteLine("UNPROCESSED STORES");
                    WriteUnprocessedLogDetail(_StoresNotProcessedList, aWeek);
                }
            }
            catch
            {
                throw;
            }
        }

        private void WriteAuditInfo()
        {
            //Begin - Abercrombie & Fitch #4448 - STodd - Audit
            int auditRID;
            AuditData auditData = new AuditData();
            try
            {
                auditData.OpenUpdateConnection();
                DateRangeProfile dtrp = _sab.ApplicationServerSession.Calendar.GetDateRange(this._dateRangeRid);
                ProfileList weekRange = _sab.ApplicationServerSession.Calendar.GetWeekRange(dtrp, null);
                //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID); //_sab.StoreServerSession.GetStoreGroup(SG_RID);
                string groupName = StoreMgmt.StoreGroup_GetName(SG_RID); //TT#1517-MD -jsobek -Store Service Optimization
                ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID); //_sab.StoreServerSession.GetStoreGroupLevelListViewList(SG_RID);
                                                                                      //				StoreGroupLevelListViewProfile sgllvp;
                                                                                      //				GroupLevelNodeFunction glnf = null;
                                                                                      //				string forecastType = null;
                                                                                      //				bool stockMinMax = false;
                                                                                      //				int sequence = 0;

                auditRID = auditData.ForecastAuditForecast_Add(DateTime.Now,
                    _sab.ApplicationServerSession.Audit.ProcessRID,
                    _sab.ClientServerSession.UserRID,
                    this._hierNodeRid,
                    _hnp.Text,                          //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
                    this.Key,
                    this.Name,
                    this.MethodType,
                    Include.NoRID,
                    Include.NoRID,
                    dtrp.DateRangeType,
                    dtrp.Name,
                    dtrp.DisplayDate,
                    ((WeekProfile)weekRange[0]).YearWeek,
                    ((WeekProfile)weekRange[weekRange.Count - 1]).YearWeek,
                    this.SG_RID,
                    groupName);

                //=================================
                // write details for Modify Sales
                //=================================
                FilterData storeFilterData = new FilterData();
                string filterName = storeFilterData.FilterGetName(_filter);
                auditData.ForecastAuditModifySales_Insert(auditRID, filterName, this._averageBy.ToString());

                //===================================
                // write Matrix details for each set
                //===================================
                ArrayList sglList = GetStoreGroupLevelList();
                foreach (int sglRid in sglList)
                {
                    //============================
                    // Get Rules for the set
                    //============================
                    DataRow[] sglRows = _dtMatrixRules.Select("SGL_RID = " + sglRid.ToString());
                    //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sglRid); //_sab.StoreServerSession.GetStoreGroupLevel(sglRid);

                    //=================================================================================
                    // If this set has rules defined, save the grade, sell thru, and rule information
                    //=================================================================================
                    eModifySalesRuleType rule = eModifySalesRuleType.None;
                    double ruleQty = 0;
                    if (sglRows.Length > 0)
                    {
                        foreach (StoreGradeProfile stgp in _storeGradeList)
                        {
                            foreach (int sellThruPct in _sellThruList)
                            {
                                DataRow[] ruleRows = _dtMatrixRules.Select("SGL_RID = " + sglRid.ToString() +
                                    " and BOUNDARY = " + stgp.Boundary.ToString() +
                                    " and SELL_THRU = " + sellThruPct.ToString());

                                rule = eModifySalesRuleType.None;
                                ruleQty = 0;
                                if (ruleRows.Length > 0)
                                {
                                    DataRow row = ruleRows[0];
                                    rule = (eModifySalesRuleType)Convert.ToInt32(row["MATRIX_RULE"], CultureInfo.CurrentUICulture);
                                    ruleQty = Convert.ToDouble(row["MATRIX_RULE_QUANTITY"], CultureInfo.CurrentUICulture);
                                }

                                int storeCnt = _storeCounter.Get(sglRid, stgp.Boundary, sellThruPct);

                                auditData.ForecastAuditModifySalesMatrix_Insert(auditRID, sglRid, stgp.Boundary, stgp.StoreGrade,
                                    sellThruPct, storeCnt, rule, ruleQty);

                            }
                        }
                    }
                }

                auditData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                auditData.CloseUpdateConnection();
            }
            //End - Abercrombie & Fitch #4448
        }

        private void BuildStoreToIndexHashtable(ProfileList storeList)
        {
            try
            {
                if (_storeToIndexHash == null)
                    _storeToIndexHash = new Hashtable();
                else
                    _storeToIndexHash.Clear();
                for (int s = 0; s < storeList.Count; s++)
                {
                    StoreProfile sp = (StoreProfile)storeList[s];
                    _storeToIndexHash.Add(sp.Key, s);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fills in the plan part of the CubeGroup open parms
        /// </summary>
        private PlanOpenParms FillOpenParmForPlan(int planVersion, WeekProfile aWeek)
        {
            try
            {
                PlanOpenParms openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _computationsMode);

                if (this.GlobalUserType == eGlobalUserType.User)
                {
                    openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsUserOTSModifySales);
                }
                else
                {
                    openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);
                }
                openParms.FunctionSecurityProfile.SetAllowUpdate();

                _hnp.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_hnp.Key, (int)eSecurityTypes.Chain);
                _hnp.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_hnp.Key, (int)eSecurityTypes.Store);
                //Begin Track #4457 - JSmith - Add forecast versions
                //				VersionProfile vp = new VersionProfile(planVersion);
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                VersionProfile vp = fvpb.Build(planVersion);
                //End Track #4457
                vp.StoreSecurity = new VersionSecurityProfile(planVersion);
                vp.ChainSecurity = new VersionSecurityProfile(planVersion);
                vp.StoreSecurity.SetAllowUpdate();
                vp.ChainSecurity.SetAllowUpdate();

                openParms.StoreHLPlanProfile.VersionProfile = vp;
                openParms.StoreHLPlanProfile.NodeProfile = _hnp;
                //			_openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.StoreHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Store);

                openParms.ChainHLPlanProfile.VersionProfile = vp;
                openParms.ChainHLPlanProfile.NodeProfile = _hnp;
                //			_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);

                if (aWeek == null)
                    openParms.DateRangeProfile = _sab.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);
                else
                    openParms.DateRangeProfile = _sab.ApplicationServerSession.Calendar.GetDateRange(aWeek);

                if (SG_RID == Include.NoRID)
                    openParms.StoreGroupRID = Include.AllStoreGroupRID;
                else
                    openParms.StoreGroupRID = SG_RID;

                openParms.IneligibleStores = false;
                openParms.SimilarStores = false;

                if (_computationsMode != null)
                {
                    openParms.ComputationsMode = _computationsMode;
                }
                else
                {
                    openParms.ComputationsMode = _sab.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
                }
                return openParms;
            }
            catch
            {
                throw;
            }
        }

        private PlanOpenParms FillOpenParmForPlan2()
        {
            try
            {
                PlanOpenParms openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _computationsMode);

                if (this.GlobalUserType == eGlobalUserType.User)
                {
                    openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsUserOTSModifySales);
                }
                else
                {
                    openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);
                }
                openParms.FunctionSecurityProfile.SetAllowUpdate();

                HierarchyNodeProfile hnp = _sab.HierarchyServerSession.GetNodeData(this._hierNodeRid);
                hnp.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Chain);
                hnp.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Store);

                //Begin Track #4457 - JSmith - Add forecast versions
                //				VersionProfile vp = new VersionProfile(Include.FV_ActualRID);
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                VersionProfile vp = fvpb.Build(Include.FV_ActualRID);
                //End Track #4457
                vp.StoreSecurity = new VersionSecurityProfile(Include.FV_ActualRID);
                vp.ChainSecurity = new VersionSecurityProfile(Include.FV_ActualRID);
                vp.StoreSecurity.SetAllowUpdate();
                vp.ChainSecurity.SetAllowUpdate();

                openParms.StoreHLPlanProfile.VersionProfile = vp;
                openParms.StoreHLPlanProfile.NodeProfile = hnp;
                //			_openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.StoreHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Store);

                openParms.ChainHLPlanProfile.VersionProfile = vp;
                openParms.ChainHLPlanProfile.NodeProfile = hnp;
                //			_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);

                openParms.DateRangeProfile = _sab.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);

                //if (SG_RID == Include.NoRID)
                openParms.StoreGroupRID = Include.AllStoreGroupRID;
                //else
                //	openParms.StoreGroupRID = SG_RID;

                openParms.FilterRID = _filter;
                openParms.IneligibleStores = false;
                openParms.SimilarStores = false;

                if (_computationsMode != null)
                {
                    openParms.ComputationsMode = _computationsMode;
                }
                else
                {
                    openParms.ComputationsMode = _sab.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
                }
                return openParms;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fills in the basis part of the CubeGroup open parms.
        /// </summary>
        private void FillOpenParmForBasis(int sglRid, WeekProfile aWeek)  // 4575
        {
            BasisProfile basisProfile;
            BasisDetailProfile basisDetailProfile;
            int bdpKey = 1;

            //=======================
            // Set up Basis Profile
            //=======================
            basisProfile = new BasisProfile(1, null, _openParms);
            basisProfile.BasisType = eTyLyType.NonTyLy;

            //=============================================
            // Build 1 basis using the data from the method
            //=============================================
            basisDetailProfile = new BasisDetailProfile(bdpKey, _openParms);
            //Begin Track #4457 - JSmith - Add forecast versions
            //			basisDetailProfile.VersionProfile = new VersionProfile(Include.FV_ActualRID);
            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
            basisDetailProfile.VersionProfile = fvpb.Build(Include.FV_ActualRID);
            //End Track #4457
            basisDetailProfile.HierarchyNodeProfile = _hnp;
            // Begin Issue 4575 stodd 08.10.07
            // Using the _dateRangeRid (plan date range) during processing when a filter
            // is involved, throws off how the basis weeks lines up with the plan week.
            DateRangeProfile drp = null;
            if (_filter == Include.UndefinedStoreFilter)
                drp = _sab.ApplicationServerSession.Calendar.GetDateRange(_dateRangeRid);
            else
                drp = _sab.ApplicationServerSession.Calendar.GetDateRange(aWeek);
            // End Issue 4575
            basisDetailProfile.DateRangeProfile = drp;
            basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
            basisDetailProfile.Weight = 1;
            basisProfile.BasisDetailProfileList.Add(basisDetailProfile);

            _openParms.BasisProfileList.Add(basisProfile);
        }

        private void SetVariables()
        {

            //=======================================================
            // Determine which variables to use and set them
            //=======================================================
            if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Regular)
            {
                //_modSalesVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable.Key);
                _salesVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key);
                _salesStockRatioVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesStockRatioRegPromoVariable.Key);
                // Begin Issue (A&F Defect #524)
                if (_averageBy == eStoreAverageBy.AllStores)
                {
                    // Begin Issue # 4316 - stodd - 2.9.2007
                    _sellThruVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SellThruPctRegPromoAllStoreIndexVariable.Key);
                    // End Issue # 4316 - stodd - 2.9.2007
                }
                else
                {
                    _sellThruVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SellThruPctRegPromoSetIndexVariable.Key);
                }
                // END Issue (A&F Defect #524)
            }
            else
            {
                //_modSalesVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key);
                _salesVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key);
                _salesStockRatioVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesStockRatioTotalVariable.Key);
                // Begin Issue (A&F Defect #524)
                if (_averageBy == eStoreAverageBy.AllStores)
                {
                    // Begin Issue # 4316 - stodd - 2.9.2007
                    _sellThruVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SellThruPctTotalAllStoreIndexVariable.Key);
                    // End Issue # 4316 - stodd - 2.9.2007
                }
                else
                {
                    _sellThruVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SellThruPctTotalSetIndexVariable.Key);
                }
                // END Issue (A&F Defect #524)
            }

            ProfileList variables = _cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;

            VariableProfile vp = (VariableProfile)variables.FindKey(_salesVariable.Key);
            _salesVariable.VariableProfile = vp;
            vp = (VariableProfile)variables.FindKey(_sellThruVariable.Key);
            _sellThruVariable.VariableProfile = vp;

            _salesTotalVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key);
            _salesTotalVariable.VariableProfile = (VariableProfile)variables.FindKey(_salesTotalVariable.Key);
            _salesRegVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable.Key);
            _salesRegVariable.VariableProfile = (VariableProfile)variables.FindKey(_salesRegVariable.Key);
            _salesPromoVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesPromoUnitsVariable.Key);
            _salesPromoVariable.VariableProfile = (VariableProfile)variables.FindKey(_salesPromoVariable.Key);
            _salesMkdnVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesMarkdownUnitsVariable.Key);
            _salesMkdnVariable.VariableProfile = (VariableProfile)variables.FindKey(_salesMkdnVariable.Key);
            _stockVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key);
            _stockVariable.VariableProfile = (VariableProfile)variables.FindKey(_stockVariable.Key);

        }

        /// <summary>
        /// Sets the _salesStockRatioNeeded switch and the _avgStoreNeeded switch
        /// process those sets that have rules.
        /// </summary>
        /// <returns></returns>
        private void SetValuesNeededSwitches(int sglRid)
        {
            _salesStockRatioNeeded = false;
            //_avgStoreNeeded = false;
            _avgStoreSSNeeded = false;
            //_basisSalesNeeded = false;

            try
            {
                _dtMatrixRules.DefaultView.RowFilter = "SGL_RID = " + sglRid.ToString();
                foreach (DataRow row in _dtMatrixRules.Rows)
                {
                    eModifySalesRuleType rule = (eModifySalesRuleType)Convert.ToInt32(row["MATRIX_RULE"], CultureInfo.CurrentUICulture);

                    switch (rule)
                    {
                        case eModifySalesRuleType.PlugSales:
                            break;
                        case eModifySalesRuleType.SalesModifier:
                            //_basisSalesNeeded = true;
                            break;
                        case eModifySalesRuleType.SalesIndex:
                            //_avgStoreNeeded = true;
                            break;
                        case eModifySalesRuleType.StockToSalesIndex:
                            _salesStockRatioNeeded = true;
                            _avgStoreSSNeeded = true;
                            break;
                        case eModifySalesRuleType.StockToSalesRatio:
                        case eModifySalesRuleType.StockToSalesMinmum:       // issue 4288
                        case eModifySalesRuleType.StockToSalesMaximum:      // issue 4288	
                            _salesStockRatioNeeded = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void Read(ProfileList storeList, WeekProfile aWeek, int sglRid)
        {
            try
            {
                WeekProfile wk1 = _sab.ApplicationServerSession.Calendar.GetWeek(2006, 48);
                ArrayList sales1 = ReadBasisValues(wk1, _salesVariable, storeList);
                WeekProfile wk2 = _sab.ApplicationServerSession.Calendar.GetWeek(2006, 49);
                ArrayList sales2 = ReadBasisValues(wk2, _salesVariable, storeList);
                WeekProfile wk3 = _sab.ApplicationServerSession.Calendar.GetWeek(2006, 50);
                ArrayList sales3 = ReadBasisValues(wk3, _salesVariable, storeList);

                //====================
                // Always need these
                //====================

                _basisSellThruValueList = ReadBasisValues(aWeek, _sellThruVariable, storeList);

                _basisSalesValueList = ReadBasisValues(aWeek, _salesVariable, storeList);
                _basisSalesRegValueList = ReadBasisValues(aWeek, _salesRegVariable, storeList);
                _basisPromoValueList = ReadBasisValues(aWeek, _salesPromoVariable, storeList);
                _basisMkdnValueList = ReadBasisValues(aWeek, _salesMkdnVariable, storeList);
                _basisStockValueList = ReadBasisValues(aWeek, _stockVariable, storeList);

                _modSalesTotValueList = ReadPlanValues(aWeek, _salesTotalVariable, storeList);
                _modSalesRegValueList = ReadPlanValues(aWeek, _salesRegVariable, storeList);
                _modPromoValueList = ReadPlanValues(aWeek, _salesPromoVariable, storeList);
                _modMkdnValueList = ReadPlanValues(aWeek, _salesMkdnVariable, storeList);


                if (this._salesStockRatioNeeded)
                    _salesStockRatioValueList = ReadBasisValues(aWeek, _salesStockRatioVariable, storeList);
                //if (this._avgStoreNeeded)  // issue 4288 - stodd 2.14.2007
                _avgStoreValue = ReadAverageStore(aWeek, _salesVariable, sglRid);
                if (this._avgStoreSSNeeded)
                    _avgStoreSalesStockRatioValue = ReadAverageStoreSalesStockRatio(aWeek, _salesStockRatioVariable, sglRid);
            }
            catch
            {
                throw;
            }

        }

        private StoreGradeList BuildStoreGradeList()
        {
            Hashtable storeGrades = new Hashtable();

            try
            {
                StoreGradeList storeGradeList = new StoreGradeList(eProfileType.StoreGrade);

                for (int i = 0; i < _dtGrades.Rows.Count; i++)
                {
                    DataRow gradeRow = _dtGrades.Rows[i];
                    int key = Convert.ToInt32(gradeRow["BOUNDARY"], CultureInfo.CurrentUICulture);
                    StoreGradeProfile sp = new StoreGradeProfile(key);
                    // BEGIN Issue 5550 stodd
                    sp.StoreGrade = gradeRow["GRADE_CODE"].ToString();
                    // END Issue 5550 
                    sp.Boundary = Convert.ToInt32(gradeRow["BOUNDARY"], CultureInfo.CurrentUICulture);
                    storeGradeList.Add(sp);
                }
                return storeGradeList;
            }
            catch
            {
                throw;
            }
        }


        private Hashtable GetStoreGrades(StoreGradeList storeGradeList, ProfileList storeList, WeekProfile aWeek)
        {
            Hashtable storeGrades = new Hashtable();

            try
            {
                ArrayList basisSetSalesValueList = ReadBasisValues(aWeek, _salesVariable, storeList);

                GradeStoreBin[] gradeStoreBinList = new GradeStoreBin[storeList.ArrayList.Count];
                for (int s = 0; s < storeList.ArrayList.Count; s++)
                {
                    StoreProfile storeProfile = (StoreProfile)storeList[s];
                    GradeStoreBin gradeStoreBin = new GradeStoreBin();
                    gradeStoreBin.StoreKey = storeProfile.Key;
                    PlanCellReference cr = (PlanCellReference)basisSetSalesValueList[s];
                    gradeStoreBin.StoreGradeUnits = cr.CurrentCellValue;
                    gradeStoreBin.StoreEligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(storeProfile.Key)).StoreIsEligible;
                    gradeStoreBinList[s] = gradeStoreBin;
                }
                StoreGrade sg = new StoreGrade();
                // BEGIN issue 4288 - stodd 2.14.2007
                int[] StoreGradeBoundary = StoreGrade.GetGradeProfileKey(storeGradeList, gradeStoreBinList, _avgStoreValue);
                // END issue 4288 - stodd 2.14.2007

                for (int s = 0; s < storeList.ArrayList.Count; s++)
                {
                    StoreProfile sp = (StoreProfile)storeList[s];
                    int aGrade = StoreGradeBoundary[s];
                    storeGrades.Add(sp.Key, aGrade);
                }
                return storeGrades;
            }
            catch
            {
                throw;
            }
        }

        private double[] GetSellThruList()
        {
            try
            {
                double[] sellThruList = new double[_dtSellThru.Rows.Count];
                _dtSellThru.DefaultView.Sort = "SELL_THRU DESC";
                for (int i = 0; i < _dtSellThru.Rows.Count; i++)
                {
                    DataRow sellThruRow = _dtSellThru.Rows[i];
                    double sellThru = Convert.ToDouble(sellThruRow["SELL_THRU"], CultureInfo.CurrentUICulture);
                    sellThruList[i] = sellThru;
                }

                return sellThruList;
            }
            catch
            {
                throw;
            }
        }

        private double GetStoreSellThruBoundary(double aValue)
        {
            //double aValue = aPctValue * 100;  // Convert the pct into an index
            double sellThruBoundary = 0;
            try
            {
                for (int i = 0; i < _sellThruList.Length; i++)
                {
                    if (i == 0)
                    {
                        if (aValue > _sellThruList[i])
                        {
                            sellThruBoundary = _sellThruList[i];
                            break;
                        }
                    }
                    else
                    {
                        if (aValue > _sellThruList[i] && aValue <= _sellThruList[i - 1])
                        {
                            sellThruBoundary = _sellThruList[i];
                            break;
                        }
                    }
                }
                return sellThruBoundary;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Reads Actuals (basis values)
        /// </summary>
        /// <param name="planWeek"></param>
        /// <param name="aVariable"></param>
        /// <param name="storeList"></param>
        /// <returns></returns>
        public ArrayList ReadBasisValues(WeekProfile planWeek, ModelVariableProfile aVariable, ProfileList storeList)
        {
            PlanCellReference planCellRef = null;
            try
            {
                Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisWeekDetail);

                planCellRef = new PlanCellReference((PlanCube)myCube);
                planCellRef[eProfileType.Version] = Include.FV_ActualRID;
                planCellRef[eProfileType.HierarchyNode] = this._hierNodeRid;
                planCellRef[eProfileType.Basis] = 1;
                planCellRef[eProfileType.Week] = planWeek.Key;
                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                planCellRef[eProfileType.Variable] = aVariable.Key;

                ArrayList storeBasisValues = planCellRef.GetCellRefArray(storeList);

                //				for (int i=0;i<storeBasisValues.Count;i++)
                //				{
                //					pcr = (PlanCellReference)storeBasisValues[i];
                //					double storeValue = pcr.HiddenCurrentCellValue;
                //					Debug.WriteLine(i.ToString() + " " + planWeek.YearWeek.ToString() + " " + storeValue.ToString());
                //				}

                return storeBasisValues;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Reads Average Store information for Actuals (Basis)
        /// </summary>
        /// <param name="planWeek"></param>
        /// <param name="aVariable"></param>
        /// <param name="sglRid"></param>
        /// <returns></returns>
        public double ReadAverageStore(WeekProfile planWeek, ModelVariableProfile aVariable, int sglRid)
        {
            Cube myCube = null;
            double avgStoreValue = 0;
            try
            {
                if (_averageBy == eStoreAverageBy.AllStores)
                    myCube = _cubeGroup.GetCube(eCubeType.StoreBasisStoreTotalWeekDetail);
                else
                    myCube = _cubeGroup.GetCube(eCubeType.StoreBasisGroupTotalWeekDetail);

                PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
                planCellRef[eProfileType.Version] = Include.FV_ActualRID;
                planCellRef[eProfileType.HierarchyNode] = this._hierNodeRid;
                planCellRef[eProfileType.Basis] = 1;
                planCellRef[eProfileType.Week] = planWeek.Key;
                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.StoreAverageQuantity.Key;

                if (_averageBy == eStoreAverageBy.Set)
                    planCellRef[eProfileType.StoreGroupLevel] = sglRid;

                planCellRef[eProfileType.Variable] = aVariable.Key;

                avgStoreValue = planCellRef.CurrentCellValue;
                // BEGIN issue 4323 - stodd 2.21.2007
                if (avgStoreValue < 0)
                    avgStoreValue = 0;
                // END issue 4323 - stodd 2.21.2007

                //Debug.WriteLine("SET: " + sglRid.ToString() + " " + _averageBy.ToString() + " Average " + planWeek.YearWeek.ToString() + " " + avgStoreValue.ToString());

                return avgStoreValue;
            }
            catch
            {
                throw;
            }
        }

        public double ReadAverageStoreSalesStockRatio(WeekProfile planWeek, ModelVariableProfile aVariable, int sglRid)
        {
            Cube myCube = null;
            double avgStoreValue = 0;
            try
            {
                if (_averageBy == eStoreAverageBy.AllStores)
                    myCube = _cubeGroup.GetCube(eCubeType.StoreBasisStoreTotalWeekDetail);
                else
                    myCube = _cubeGroup.GetCube(eCubeType.StoreBasisGroupTotalWeekDetail);

                PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
                planCellRef[eProfileType.Version] = Include.FV_ActualRID;
                planCellRef[eProfileType.HierarchyNode] = this._hierNodeRid;
                planCellRef[eProfileType.Basis] = 1;
                planCellRef[eProfileType.Week] = planWeek.Key;
                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.StoreAverageQuantity.Key;

                if (_averageBy == eStoreAverageBy.Set)
                    planCellRef[eProfileType.StoreGroupLevel] = sglRid;

                planCellRef[eProfileType.Variable] = aVariable.Key;

                avgStoreValue = planCellRef.CurrentCellValue;
                // BEGIN issue 4323 - stodd 2.21.2007
                if (avgStoreValue < 0)
                    avgStoreValue = 0;
                // END issue 4323 - stodd 2.21.2007
                //Debug.WriteLine("SET: " + sglRid.ToString() + " " + _averageBy.ToString() + " Average " + planWeek.YearWeek.ToString() + " Ratio " + avgStoreValue.ToString());

                return avgStoreValue;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Reads Modified version information that we're going to update.
        /// </summary>
        /// <param name="planWeek"></param>
        /// <param name="aVariable"></param>
        /// <param name="storeList"></param>
        /// <returns></returns>
        public ArrayList ReadPlanValues(WeekProfile planWeek, ModelVariableProfile aVariable, ProfileList storeList)
        {
            try
            {
                Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanWeekDetail);
                //Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisDetail);

                PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
                planCellRef[eProfileType.Version] = Include.FV_ModifiedRID;
                planCellRef[eProfileType.HierarchyNode] = this._hierNodeRid;
                planCellRef[eProfileType.Week] = planWeek.Key;
                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                planCellRef[eProfileType.Variable] = aVariable.Key;

                ArrayList storePlanValues = planCellRef.GetCellRefArray(storeList);

                //				for (int i=0;i<storePlanValues.Count;i++)
                //				{
                //					PlanCellReference pcr = (PlanCellReference)storePlanValues[i];
                //					double storeValue = pcr.HiddenCurrentCellValue;
                //					Debug.WriteLine(i.ToString() + " " + planWeek.YearWeek.ToString() + " " + storeValue.ToString());
                //				}

                return storePlanValues;
            }
            catch
            {
                throw;
            }
        }


        private bool ProcessRuleForStore(ProfileList storeList, int storeIdx, WeekProfile aWeek,
            eModifySalesRuleType rule, double ruleQty)
        {
            try
            {
                bool storeProcessed = false;
                double basisValue = 0;
                double modValue = 0;
                double ssrValue = 0;
                PlanCellReference basisPcr = null;
                PlanCellReference ssrPcr = null;
                StoreProfile sp = (StoreProfile)storeList[storeIdx];
                switch (rule)
                {
                    case eModifySalesRuleType.PlugSales:
                        //basisPcr = (PlanCellReference)_basisSalesValueList[storeIdx];
                        //basisValue = basisPcr.HiddenCurrentCellValue;
                        //modPcr = (PlanCellReference)_modSalesValueList[storeIdx];
                        //modValue = ruleQty;
                        //modPcr.SetEntryCellValue(modValue);
                        Spread(ruleQty, storeIdx);
                        //Debug.WriteLine(sp.StoreId + " " + aWeek.YearWeek.ToString() + " " + rule.ToString() + " " + ruleQty.ToString() + " " +
                        //	"BASIS " + basisValue.ToString() + " MOD " + modValue.ToString());
                        storeProcessed = true;
                        break;
                    case eModifySalesRuleType.SalesModifier:
                        basisPcr = (PlanCellReference)_basisSalesValueList[storeIdx];
                        basisValue = basisPcr.HiddenCurrentCellValue;
                        // BEGIN issue 4323 - stodd 2.21.2007
                        if (basisValue < 0)
                            basisValue = 0;
                        // END issue 4323 - stodd 2.21.2007
                        //modPcr = (PlanCellReference)_modSalesValueList[storeIdx];
                        // Calculation
                        modValue = (double)((int)((basisValue * ruleQty) + 0.5d));
                        //modPcr.SetEntryCellValue(modValue);
                        Spread(modValue, storeIdx);
                        //Debug.WriteLine(sp.StoreId + " " + aWeek.YearWeek.ToString() + " " + rule.ToString() + " " + ruleQty.ToString() + " " +
                        //	"BASIS " + basisValue.ToString() + " MOD " + modValue.ToString());
                        storeProcessed = true;
                        break;
                    case eModifySalesRuleType.SalesIndex:
                        //modPcr = (PlanCellReference)_modSalesValueList[storeIdx];
                        // Calculation
                        modValue = (double)((int)((_avgStoreValue * ruleQty) + 0.5d));
                        //modPcr.SetEntryCellValue(modValue);
                        Spread(modValue, storeIdx);
                        //Debug.WriteLine(sp.StoreId + " " + aWeek.YearWeek.ToString() + " " + rule.ToString() + " " + ruleQty.ToString() + " " +
                        //	"AVG ST " + _avgStoreValue.ToString() + " MOD " + modValue.ToString());
                        storeProcessed = true;
                        break;
                    case eModifySalesRuleType.StockToSalesIndex:
                        basisPcr = (PlanCellReference)_basisSalesValueList[storeIdx];
                        basisValue = basisPcr.HiddenCurrentCellValue;
                        ssrPcr = (PlanCellReference)_salesStockRatioValueList[storeIdx];
                        ssrValue = ssrPcr.HiddenCurrentCellValue;
                        // BEGIN issue 4323 - stodd 2.21.2007
                        if (ssrValue <= 0)
                        {
                            Spread(0, storeIdx);
                        }
                        else
                        // END issue 4323 - stodd 2.21.2007
                        {
                            //modPcr = (PlanCellReference)_modSalesValueList[storeIdx];
                            // Calculation
                            modValue = (double)((int)((((_avgStoreSalesStockRatioValue * ruleQty) / ssrValue) * basisValue) + 0.5d));
                            //modPcr.SetEntryCellValue(modValue);
                            Spread(modValue, storeIdx);
                        }
                        //Debug.WriteLine(sp.StoreId + " " + aWeek.YearWeek.ToString() + " " + rule.ToString() + " " + ruleQty.ToString() + " " +
                        //	"BASIS " + basisValue.ToString() + " AVG s/s ratio " + _avgStoreSalesStockRatioValue.ToString() +
                        //	" s/s ratio " + ssrValue.ToString() + " MOD " + modValue.ToString());
                        storeProcessed = true;
                        break;
                    case eModifySalesRuleType.StockToSalesRatio:
                        StockToSalesRatio(storeIdx, ruleQty);
                        storeProcessed = true;
                        break;
                    case eModifySalesRuleType.StockToSalesMinmum:
                        ssrPcr = (PlanCellReference)_salesStockRatioValueList[storeIdx];
                        ssrValue = ssrPcr.HiddenCurrentCellValue;
                        if (ssrValue < ruleQty)
                        {
                            StockToSalesRatio(storeIdx, ruleQty);
                            storeProcessed = true;
                        }
                        break;
                    case eModifySalesRuleType.StockToSalesMaximum:
                        ssrPcr = (PlanCellReference)_salesStockRatioValueList[storeIdx];
                        ssrValue = ssrPcr.HiddenCurrentCellValue;
                        if (ssrValue > ruleQty)
                        {
                            StockToSalesRatio(storeIdx, ruleQty);
                            storeProcessed = true;
                        }

                        break;
                    default:
                        break;
                }

                //				if (MONITOR)
                //				{
                //					if (storeProcessed)
                //						WriteLogDetail(storeList, storeIdx);
                //				}

                return storeProcessed;
            }
            catch
            {
                throw;
            }
        }

        private void StockToSalesRatio(int storeIdx, double ruleQty)
        {
            double basisValue = 0;
            double modValue = 0;
            double ssrValue = 0;
            PlanCellReference basisPcr = null;
            PlanCellReference ssrPcr = null;
            try
            {
                basisPcr = (PlanCellReference)_basisSalesValueList[storeIdx];
                basisValue = basisPcr.HiddenCurrentCellValue;
                ssrPcr = (PlanCellReference)_salesStockRatioValueList[storeIdx];
                ssrValue = ssrPcr.HiddenCurrentCellValue;
                //modPcr = (PlanCellReference)_modSalesValueList[storeIdx];
                // Calculation
                // BEGIN issue 4323 - stodd 2.21.2007
                if (ssrValue <= 0)
                {
                    Spread(0, storeIdx);
                }
                else
                // END issue 4323 - stodd 2.21.2007
                {
                    modValue = (double)((int)(((ruleQty / ssrValue) * basisValue) + 0.5d));
                    //modPcr.SetEntryCellValue(modValue);
                    Spread(modValue, storeIdx);
                }
                //Debug.WriteLine(sp.StoreId + " " + aWeek.YearWeek.ToString() + " " + rule.ToString() + " " + ruleQty.ToString() + " " +
                //	"BASIS " + basisValue.ToString() + " s/s ratio " + ssrValue.ToString() + " MOD " + modValue.ToString());
            }
            catch
            {
                throw;
            }
        }

        private void WriteLogDetail(ProfileList storeList, int storeIdx)
        {
            PlanCellReference salesTotalPcr = null;             // Plan/Modified
            PlanCellReference salesRegPcr = null;               // Plan/Modified
            PlanCellReference salesPromoPcr = null;             // Plan/Modified	
            PlanCellReference salesMkdnPcr = null;              // Plan/Modified
            PlanCellReference bSalesTotalPcr = null;            // Basis/Actuals
            PlanCellReference bSalesRegPcr = null;              // Basis/Actuals
            PlanCellReference bSalesPromoPcr = null;            // Basis/Actuals
            PlanCellReference bSalesMkdnPcr = null;             // Basis/Actuals
            PlanCellReference bSSRatioPcr = null;               // Basis/Actuals
            PlanCellReference bStockPcr = null;                 // Basis/Actuals
            PlanCellReference bSellThruPcr = null;              // Basis/Actuals

            try
            {

                StoreProfile sp = (StoreProfile)storeList[storeIdx];

                salesTotalPcr = (PlanCellReference)_modSalesTotValueList[storeIdx];
                salesRegPcr = (PlanCellReference)_modSalesRegValueList[storeIdx];
                salesPromoPcr = (PlanCellReference)_modPromoValueList[storeIdx];
                salesMkdnPcr = (PlanCellReference)_modMkdnValueList[storeIdx];


                bSalesTotalPcr = (PlanCellReference)_basisSalesValueList[storeIdx];
                bSalesRegPcr = (PlanCellReference)_basisSalesRegValueList[storeIdx];
                bSalesPromoPcr = (PlanCellReference)_basisPromoValueList[storeIdx];
                bSalesMkdnPcr = (PlanCellReference)_basisMkdnValueList[storeIdx];
                bStockPcr = (PlanCellReference)_basisStockValueList[storeIdx];
                bSellThruPcr = (PlanCellReference)_basisSellThruValueList[storeIdx];    // Added under 4316

                double ssRatio = 0.0d;
                if (_salesStockRatioValueList != null)
                {
                    bSSRatioPcr = (PlanCellReference)_salesStockRatioValueList[storeIdx];
                    ssRatio = bSSRatioPcr.CurrentCellValue;
                }

                //================================================================================
                // IF the plan level type is TOTAL, the _basisSalesValueList will contain
                // values for SALES TOT. (We don't need Sales Total when the plan level type is 
                // REG, and it is left as 0.
                //================================================================================
                double bTotal = 0.0d;
                if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Total)
                    bTotal = bSalesTotalPcr.CurrentCellValue;

                //StoreGradeProfile sgp = _storeGradeList.FindKey(_currGradeBoundary);

                _log.WriteDetail(
                    sp.StoreId,
                    sp.Key,
                    ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(sp.Key)).StoreIsEligible,
                    _currGradeBoundary,
                    _currSellThruBoundary,
                    _currRule,
                    _currRuleQty,
                    bTotal,
                    bSalesRegPcr.CurrentCellValue,
                    bSalesPromoPcr.CurrentCellValue,
                    bSalesMkdnPcr.CurrentCellValue,
                    bStockPcr.CurrentCellValue,
                    ssRatio,
                    bSellThruPcr.CurrentCellValue,
                    salesTotalPcr.CurrentCellValue,
                    salesRegPcr.CurrentCellValue,
                    salesPromoPcr.CurrentCellValue,
                    salesMkdnPcr.CurrentCellValue
                    );
            }
            catch
            {
                throw;
            }
        }

        //===================================================================
        // Write to the log the stores not processed in the attribute set
        //===================================================================
        private void WriteUnprocessedLogDetail(ProfileList storeList, WeekProfile aWeek)
        {
            PlanCellReference bSalesTotalPcr = null;            // Basis/Actuals
            PlanCellReference bSalesRegPcr = null;              // Basis/Actuals
            PlanCellReference bSalesPromoPcr = null;            // Basis/Actuals
            PlanCellReference bSalesMkdnPcr = null;             // Basis/Actuals
            PlanCellReference bSSRatioPcr = null;               // Basis/Actuals
            PlanCellReference bStockPcr = null;                 // Basis/Actuals
            PlanCellReference bSellThruPcr = null;              // Basis/Actuals

            try
            {
                ArrayList sellThruValueList = ReadBasisValues(aWeek, _sellThruVariable, storeList);

                for (int s = 0; s < storeList.Count; s++)
                {
                    StoreProfile sp = (StoreProfile)storeList[s];
                    PlanCellReference pcr = (PlanCellReference)sellThruValueList[s];
                    double storeSellThruValue = pcr.CurrentCellValue;
                    double storeSellThruBoundary = GetStoreSellThruBoundary(storeSellThruValue);
                    int storeGrade = (int)_storeGrades[sp.Key];
                    bool isElig = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(sp.Key)).StoreIsEligible;

                    // Begin Issue 4288 - stodd
                    int storeIdx = (int)_storeToIndexHash[sp.Key];

                    bSalesTotalPcr = (PlanCellReference)_basisSalesValueList[storeIdx];
                    bSalesRegPcr = (PlanCellReference)_basisSalesRegValueList[storeIdx];
                    bSalesPromoPcr = (PlanCellReference)_basisPromoValueList[storeIdx];
                    bSalesMkdnPcr = (PlanCellReference)_basisMkdnValueList[storeIdx];   // Fixed under 4316
                    bSellThruPcr = (PlanCellReference)_basisSellThruValueList[storeIdx];    // Added under 4316
                    bStockPcr = (PlanCellReference)_basisStockValueList[storeIdx];

                    double ssRatio = 0.0d;
                    if (_salesStockRatioValueList != null)
                    {
                        bSSRatioPcr = (PlanCellReference)_salesStockRatioValueList[storeIdx];
                        ssRatio = bSSRatioPcr.CurrentCellValue;
                    }

                    //================================================================================
                    // IF the plan level type is TOTAL, the _basisSalesValueList will contain
                    // values for SALES TOT. (We don't need Sales Total when the plan level type is 
                    // REG, and it is left as 0.
                    //================================================================================
                    double bTotal = 0.0d;
                    if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Total)
                        bTotal = bSalesTotalPcr.CurrentCellValue;

                    _log.WriteDetail(
                        sp.StoreId,
                        sp.Key,
                        isElig,
                        storeGrade,
                        (int)storeSellThruBoundary,
                        (eModifySalesRuleType)_storeRule[storeIdx],
                        (double)_storeRuleQty[storeIdx],
                        bTotal,
                        bSalesRegPcr.CurrentCellValue,
                        bSalesPromoPcr.CurrentCellValue,
                        bSalesMkdnPcr.CurrentCellValue,
                        bStockPcr.CurrentCellValue,
                        ssRatio,
                        bSellThruPcr.CurrentCellValue,
                        0.0d,
                        0.0d,
                        0.0d,
                        0.0d
                        );
                    // End Issue 4288 - stodd
                }
            }
            catch
            {
                throw;
            }
        }


        private void DebugModifiedSales(WeekProfile aWeek, ProfileList storeList)
        {
            PlanCellReference salesTotalPcr = null;             // Plan/Modified
            PlanCellReference salesRegPcr = null;               // Plan/Modified
            PlanCellReference salesPromoPcr = null;             // Plan/Modified	
            PlanCellReference salesMkdnPcr = null;              // Plan/Modified
            PlanCellReference bSalesTotalPcr = null;            // Plan/Actuals
            PlanCellReference bSalesRegPcr = null;              // Plan/Actuals
            PlanCellReference bSalesPromoPcr = null;            // Basis/Actuals
            PlanCellReference bSalesMkdnPcr = null;             // Basis/Actuals

            try
            {
                ArrayList salesTotalList = ReadPlanValues(aWeek, this._salesTotalVariable, storeList);
                ArrayList salesRegList = ReadPlanValues(aWeek, this._salesRegVariable, storeList);
                ArrayList salesPromoList = ReadPlanValues(aWeek, this._salesPromoVariable, storeList);
                ArrayList salesMkdnList = ReadPlanValues(aWeek, this._salesMkdnVariable, storeList);
                ArrayList bSalesTotalList = ReadBasisValues(aWeek, this._salesTotalVariable, storeList);
                ArrayList bSalesRegList = ReadBasisValues(aWeek, this._salesRegVariable, storeList);
                ArrayList bSalesPromoList = ReadBasisValues(aWeek, this._salesPromoVariable, storeList);
                ArrayList bSalesMkdnList = ReadBasisValues(aWeek, this._salesMkdnVariable, storeList);


                for (int i = 0; i < salesTotalList.Count; i++)
                {
                    StoreProfile sp = (StoreProfile)storeList[i];

                    salesTotalPcr = (PlanCellReference)salesTotalList[i];
                    salesRegPcr = (PlanCellReference)salesRegList[i];
                    salesPromoPcr = (PlanCellReference)salesPromoList[i];
                    salesMkdnPcr = (PlanCellReference)salesMkdnList[i];

                    bSalesTotalPcr = (PlanCellReference)bSalesTotalList[i];
                    bSalesRegPcr = (PlanCellReference)bSalesRegList[i];
                    bSalesPromoPcr = (PlanCellReference)bSalesPromoList[i];
                    bSalesMkdnPcr = (PlanCellReference)bSalesMkdnList[i];


                    Debug.WriteLine(sp.StoreId + "(" + sp.Key.ToString() + ")" +
                        "  TOT: " + salesTotalPcr.CurrentCellValue.ToString() +
                        "  REG: " + salesRegPcr.CurrentCellValue.ToString() +
                        "  PROMO: " + salesPromoPcr.CurrentCellValue.ToString() +
                        "  MKDN: " + salesMkdnPcr.CurrentCellValue.ToString() +
                        "  BASIS TOT: " + bSalesTotalPcr.CurrentCellValue.ToString() +
                        "  BASIS REG: " + bSalesRegPcr.CurrentCellValue.ToString() +
                        "  BASIS PROMO: " + bSalesPromoPcr.CurrentCellValue.ToString() +
                        "  BASIS MKDN: " + bSalesMkdnPcr.CurrentCellValue.ToString());
                }
            }
            catch
            {
                throw;
            }
        }

        private void Spread(double spreadValue, int storeIdx)
        {
            ArrayList inValues = new ArrayList();
            ArrayList outValues;

            try
            {
                if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Regular)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        inValues.Add(0);
                    }
                    // Load up spread array list
                    PlanCellReference salesPcr = (PlanCellReference)_basisSalesRegValueList[storeIdx];
                    inValues[0] = (int)salesPcr.CurrentCellValue;
                    PlanCellReference promoPcr = (PlanCellReference)_basisPromoValueList[storeIdx];
                    inValues[1] = (int)promoPcr.CurrentCellValue;

                    // BEGIN Issue 4617 - stodd 9.12.07
                    if ((int)inValues[0] == 0 && (int)inValues[1] == 0)
                    {
                        PlanCellReference modSalesPcr = (PlanCellReference)_modSalesRegValueList[storeIdx];
                        modSalesPcr.SetEntryCellValue(spreadValue);
                        PlanCellReference modPromoPcr = (PlanCellReference)_modPromoValueList[storeIdx];
                        modPromoPcr.SetEntryCellValue(0);
                    }
                    else
                    {
                        // Spread
                        _spreader.ExecuteSimpleSpread(spreadValue, inValues, 0, out outValues);
                        // Unload new values to mod values
                        PlanCellReference modSalesPcr = (PlanCellReference)_modSalesRegValueList[storeIdx];
                        modSalesPcr.SetEntryCellValue((double)outValues[0]);
                        PlanCellReference modPromoPcr = (PlanCellReference)_modPromoValueList[storeIdx];
                        modPromoPcr.SetEntryCellValue((double)outValues[1]);
                    }
                    // END Issue 4617 - stodd 9.12.07
                }
                else  // Total
                {
                    for (int i = 0; i < 3; i++)
                    {
                        inValues.Add(0);
                    }
                    // Load up spread array list
                    PlanCellReference salesPcr = (PlanCellReference)_basisSalesRegValueList[storeIdx];
                    inValues[0] = (int)salesPcr.CurrentCellValue;
                    PlanCellReference promoPcr = (PlanCellReference)_basisPromoValueList[storeIdx];
                    inValues[1] = (int)promoPcr.CurrentCellValue;
                    PlanCellReference mkdnPcr = (PlanCellReference)_basisMkdnValueList[storeIdx];
                    inValues[2] = (int)mkdnPcr.CurrentCellValue;

                    // BEGIN Issue 4617 - stodd 9.12.07
                    if ((int)inValues[0] == 0 && (int)inValues[1] == 0 && (int)inValues[2] == 0)
                    {
                        PlanCellReference modSalesTotPcr = (PlanCellReference)_modSalesTotValueList[storeIdx];
                        modSalesTotPcr.SetEntryCellValue(spreadValue);
                        PlanCellReference modSalesRegPcr = (PlanCellReference)_modSalesRegValueList[storeIdx];
                        modSalesRegPcr.SetEntryCellValue(spreadValue);
                        PlanCellReference modPromoPcr = (PlanCellReference)_modPromoValueList[storeIdx];
                        modPromoPcr.SetEntryCellValue(0);
                        PlanCellReference modMkdnPcr = (PlanCellReference)_modMkdnValueList[storeIdx];
                        modMkdnPcr.SetEntryCellValue(0);
                    }
                    else
                    {
                        // Spread
                        _spreader.ExecuteSimpleSpread(spreadValue, inValues, 0, out outValues);
                        // Unload new values to mod values
                        PlanCellReference modSalesTotPcr = (PlanCellReference)_modSalesTotValueList[storeIdx];
                        modSalesTotPcr.SetEntryCellValue(spreadValue);
                        PlanCellReference modSalesRegPcr = (PlanCellReference)_modSalesRegValueList[storeIdx];
                        modSalesRegPcr.SetEntryCellValue((double)outValues[0]);
                        PlanCellReference modPromoPcr = (PlanCellReference)_modPromoValueList[storeIdx];
                        modPromoPcr.SetEntryCellValue((double)outValues[1]);
                        PlanCellReference modMkdnPcr = (PlanCellReference)_modMkdnValueList[storeIdx];
                        modMkdnPcr.SetEntryCellValue((double)outValues[2]);
                    }
                    // END Issue 4617 - stodd 9.12.07
                }
            }
            catch
            {
                throw;
            }

        }


        private ProfileList GetStoreList()
        {
            try
            {
                if (_allStoreList == null)
                {
                    int reserveStoreRid = _applicationTransaction.GlobalOptions.ReserveStoreRID;
                    _allStoreList = (ProfileList)(_applicationTransaction.GetMasterProfileList(eProfileType.Store)).Clone();
                    if (_allStoreList.Contains(reserveStoreRid))
                    {
                        _allStoreList.Remove((Profile)_allStoreList.FindKey(reserveStoreRid));
                    }
                }
                return _allStoreList;
            }
            catch
            {
                throw;
            }
        }

        private ProfileList ApplyStoreFilter(WeekProfile aWeek)
        {
            try
            {
                int reserveStoreRid = _applicationTransaction.GlobalOptions.ReserveStoreRID;
                ProfileList filteredStoreList = new ProfileList(eProfileType.Store);
                //*******************************
                // Apply STORE FILTER if present
                //*******************************
                if (_filter != Include.UndefinedStoreFilter)
                {
                    PlanOpenParms openParms = null;
                    PlanCubeGroup cubeGroup = null;
                    // Begin MID Track #5399 - JSmith - Plan In Use error
                    try
                    {
                        // End MID Track #5399
                        // Begin MID Track #5210 - JSmith - Out of memory
                        //					cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup(true);  // Issue 4364 - stodd
                        cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                        // End MID Track #5210
                        openParms = FillOpenParmForPlan(Include.FV_ActualRID, aWeek);
                        //FillOpenParmForBasis(Include.AllStoreGroupRID);
                        ((ForecastCubeGroup)cubeGroup).OpenCubeGroup(openParms);
                        // BEGIN Issue 5727 stodd
                        if (!cubeGroup.SetStoreFilter(_filter, cubeGroup))
                        {
                            FilterData storeFilterData = new FilterData();
                            string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                            msg = msg.Replace("{0}", storeFilterData.FilterGetName(_filter));
                            string suffix = ". Method " + this.Name + ". ";
                            string auditMsg = msg + suffix;
                            _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
                            throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                        }
                        // END Issue 5727
                        filteredStoreList = cubeGroup.GetFilteredProfileList(eProfileType.Store);
                        ((PlanCubeGroup)cubeGroup).CloseCubeGroup();
                        // Begin MID Track #5210 - JSmith - Out of memory
                        cubeGroup.Dispose();
                        cubeGroup = null;
                        // End MID Track #5210
                        //includedStoreList = _applicationTransaction.GetAllocationFilteredStoreList(this._hierNodeRid, this._filter);
                        if (filteredStoreList.Contains(reserveStoreRid))
                        {
                            filteredStoreList.Remove((Profile)filteredStoreList.FindKey(reserveStoreRid));
                        }
                        // Begin MID Track #5399 - JSmith - Plan In Use error
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (cubeGroup != null)
                        {
                            ((PlanCubeGroup)cubeGroup).CloseCubeGroup();
                            cubeGroup.Dispose();
                            cubeGroup = null;
                        }
                    }
                    // End MID Track #5399
                }
                else
                {
                    filteredStoreList = _allStoreList;
                }

                return filteredStoreList;
            }
            catch
            {
                throw;
            }
        }

        private ProfileList ApplySetFilter(int sglRid, ProfileList storeList)
        {
            try
            {
                ProfileList newStoreList = new ProfileList(eProfileType.Store);

                //================================
                // Apply store SET if present
                //================================
                if (this.SG_RID != Include.NoRID)
                {
                    _setStoreList = _applicationTransaction.GetStoresInGroup(this.SG_RID, sglRid);
                }
                else // otherwise we do ALL the stores
                {
                    _setStoreList = storeList;
                }

                //=================================================================================
                // Merge all store lists, taking the intersection of the storeList & setStoreList
                //=================================================================================
                foreach (StoreProfile aStore in storeList.ArrayList)
                {
                    if (_setStoreList.Contains(aStore.Key))
                    {
                        newStoreList.Add(aStore);
                    }
                }

                if (MONITOR)
                {
                    if (_StoresNotProcessedList == null)
                        _StoresNotProcessedList = new ProfileList(eProfileType.Store);
                    else
                        _StoresNotProcessedList.Clear();
                    foreach (StoreProfile sp in newStoreList.ArrayList)
                    {
                        _StoresNotProcessedList.Add(sp);
                    }
                }

                return newStoreList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Pulls out the Store Group Levels from the rules. We only 
        /// process those sets that have rules.
        /// </summary>
        /// <returns></returns>
        private ArrayList GetStoreGroupLevelList()
        {
            try
            {
                ArrayList sglList = new ArrayList();
                foreach (DataRow row in this._dtMatrixRules.Rows)
                {
                    int sglRid = Convert.ToInt32(row["SGL_RID"], CultureInfo.CurrentUICulture);
                    if (!sglList.Contains(sglRid))
                        sglList.Add(sglRid);
                }

                // Issue 4323 stodd 2.21.07 -- allow processing with no rules
                //				if (sglList.Count == 0)
                //				{
                //					throw new MIDException(eErrorLevel.warning, 12345, "No Rules Defined");
                //				}

                return sglList;
            }
            catch
            {
                throw;
            }
        }

        private void Save()
        {
            try
            {
                _infoMsg = "Saving Modified Sales values...";
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());

                // BEGIN MID Track #4370 - John Smith - FWOS Models
                //				_cubeGroup.SaveCubeGroup(null);
                PlanSaveParms planSaveParms = new PlanSaveParms();
                _cubeGroup.SaveCubeGroup(planSaveParms);
                // END MID Track #4370

                _infoMsg = "Saving Modified Sales Values Completed";
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, this.ToString());
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Uses the current class variables to delete data for stores from the Modified version.
        /// </summary>
        private void DeleteStoreValues()
        {
            // Begin TT#5124 - JSmith - Performance
            //VariablesData vd = new VariablesData();
            VariablesData vd = new VariablesData(_sab.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
            // End TT#5124 - JSmith - Performance
            try
            {
                vd.OpenUpdateConnection();
                vd.StoreWeek_Delete(this._hierNodeRid, Include.FV_ModifiedRID, _weekList, _storeList);
                vd.CommitData();
            }
            catch (EmptyStoreList)      // Issue 4535 - 8.1.07
            {
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_pl_EmptyStoreListFromFilter), this.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (vd.ConnectionIsOpen)
                    vd.CloseUpdateConnection();
            }
        }

        /// <summary>
        /// Uses the current class variables to delete data for stores from the Modified version.
        /// </summary>
        private void DeleteStoreValues(WeekProfile aWeek)
        {
            // Begin TT#5124 - JSmith - Performance
            //VariablesData vd = new VariablesData();
            VariablesData vd = new VariablesData(_sab.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
            // End TT#5124 - JSmith - Performance
            try
            {
                ProfileList weekList = new ProfileList(eProfileType.Week);
                weekList.Add(aWeek);

                vd.OpenUpdateConnection();
                vd.StoreWeek_Delete(this._hierNodeRid, Include.FV_ModifiedRID, weekList, _storeList);
                vd.CommitData();
            }
            catch (EmptyStoreList)  // Issue 4535 - 8.1.07
            {
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_pl_EmptyStoreListFromFilter), this.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (vd.ConnectionIsOpen)
                    vd.CloseUpdateConnection();
            }
        }


        /// <summary>
        /// Updates the OTS Forecast Copy method
        /// </summary>
        /// <param name="td">An instance of the TransactionData class which contains the database connection</param>
        //		new public void Update(TransactionData td)
        override public void Update(TransactionData td)
        {
            if (_modifySalesData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _modifySalesData = new ForecastModifySalesMethodData(td, base.Key);
            }

            _modifySalesData.HierNodeRID = _hierNodeRid;
            _modifySalesData.CDR_RID = _dateRangeRid;
            _modifySalesData.Filter = _filter;
            _modifySalesData.AverageBy = _averageBy;
            _modifySalesData.GradeDataTable = _dtGrades;
            _modifySalesData.SellThruDataTable = _dtSellThru;
            _modifySalesData.MatrixDataTable = _dtMatrixRules;

            try
            {
                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _modifySalesData.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _modifySalesData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _modifySalesData.DeleteMethod(base.Key, td);
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

        public override bool WithinTolerance(double aTolerancePercent)
        {
            return true;
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
            OTSForecastModifySales newOTSForecastModifySales = null;

            try
            {
                newOTSForecastModifySales = (OTSForecastModifySales)this.MemberwiseClone();
                newOTSForecastModifySales.AverageBy = AverageBy;
                if (aCloneDateRanges &&
                    newOTSForecastModifySales.DateRangeRid != Include.UndefinedCalendarDateRange)
                {
                    newOTSForecastModifySales.DateRangeRid = aSession.Calendar.GetDateRangeClone(DateRangeRid).Key;
                }
                else
                {
                    newOTSForecastModifySales.DateRangeRid = DateRangeRid;
                }
                newOTSForecastModifySales.Filter = Filter;
                newOTSForecastModifySales.GradesDataTable = GradesDataTable.Copy();
                newOTSForecastModifySales.HierNodeRid = HierNodeRid;
                newOTSForecastModifySales.MatrixDataTable = MatrixDataTable.Copy();
                newOTSForecastModifySales.Method_Change_Type = eChangeType.none;
                newOTSForecastModifySales.Method_Description = Method_Description;
                newOTSForecastModifySales.MethodStatus = MethodStatus;
                newOTSForecastModifySales.Name = Name;
                newOTSForecastModifySales.SellThruDataTable = SellThruDataTable.Copy();
                newOTSForecastModifySales.SG_RID = SG_RID;
                newOTSForecastModifySales.User_RID = User_RID;
                newOTSForecastModifySales.Virtual_IND = Virtual_IND;

                return newOTSForecastModifySales;
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
                if (HierNodeRid == Include.NoRID)
                {
                    return false;
                }

                VersionSecurityProfile versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(Include.FV_ModifiedRID, (int)eSecurityTypes.Store);
                if (!versionSecurity.AllowUpdate)
                {
                    return false;
                }
                HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierNodeRid, (int)eSecurityTypes.Store);
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

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSModifySales);
            }

        }


        override public ROMethodProperties MethodGetData(bool processingApply)
        {
            List<ROStoreGradeList> sgLst = DataTableToStoreGradeList(GradesDataTable);
            List<ROSellThruList> sellthruLst = DataTableToSellThru(SellThruDataTable);

            List<ROMatrixRuleList> matrixRule = DataTableToMatrixRule(MatrixDataTable);

            ROPlanningModifySalesProperties method = new ROPlanningModifySalesProperties(
               method: GetName.GetMethod(method: this),
               description: Method_Description,
               userKey: User_RID,
               dateRange: GetName.GetCalendarDateRange(DateRangeRid, SAB),
               filter: GetName.GetFilterName(Filter),
               averageBy: AverageBy,
               storeGradesList: sgLst,
               sellThruList: sellthruLst,
               matrixRulesList: matrixRule,

               merchandise: GetName.GetMerchandiseName(_modifySalesData.HierNodeRID, SAB),
               attribute: GetName.GetAttributeName(_modifySalesData.SG_RID)
              );
            return method;
        }
        #region private methods related to MethodGetData

        private List<ROMatrixRuleList> DataTableToMatrixRule(DataTable dataTable)
        {
            List<ROMatrixRuleList> MatrixRulelst = new List<ROMatrixRuleList>();

            dataTable.DefaultView.Sort = "SGL_RID DESC, BOUNDARY DESC";

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {

                    string gradeCode = string.Empty;
                    int boundary = Convert.ToInt32(dr["BOUNDARY"]);
                    DataRow[] gradesRows = _dtGrades.Select("BOUNDARY = " + boundary.ToString());
                    if (gradesRows.Length > 0)
                    {
                        gradeCode = Convert.ToString(gradesRows[0]["GRADE_CODE"]);
                    }

                    KeyValuePair<eModifySalesRuleType, string> rule = dr["MATRIX_RULE"] == System.DBNull.Value ? GetName.GetSalesRule(eModifySalesRuleType.None) : GetName.GetSalesRule((eModifySalesRuleType)dr["MATRIX_RULE"]);
                    double quantity = Convert.ToDouble(dr["MATRIX_RULE_QUANTITY"]);
                    KeyValuePair<int, string> attributeSet = GetName.GetAttributeSetName(Convert.ToInt32(dr["SGL_RID"]));
                    MatrixRulelst.Add(new ROMatrixRuleList(boundary, 0, gradeCode, rule, quantity, attributeSet));
                }
            }
            return MatrixRulelst;
        }

        private List<ROStoreGradeList> DataTableToStoreGradeList(DataTable dataTable)
        {
            List<ROStoreGradeList> SoreGardelst = new List<ROStoreGradeList>();


            if (dataTable.Rows.Count > 0)
            {
                for (int iCounter = 0; iCounter < dataTable.Rows.Count; iCounter++)
                {
                    int BOUNDARY = Convert.ToInt32(dataTable.Rows[iCounter]["BOUNDARY"].ToString());
                    string GRADE_CODE = dataTable.Rows[iCounter]["GRADE_CODE"].ToString();
                    SoreGardelst.Add(new ROStoreGradeList(BOUNDARY, GRADE_CODE));
                }
            }
            return SoreGardelst;
        }

        private List<ROSellThruList> DataTableToSellThru(DataTable dataTable)
        {
            List<ROSellThruList> SellThrulst = new List<ROSellThruList>();


            if (dataTable.Rows.Count > 0)
            {
                for (int iCounter = 0; iCounter < dataTable.Rows.Count; iCounter++)
                {
                    int sellThru = Convert.ToInt32(dataTable.Rows[iCounter]["SELL_THRU"].ToString());
                    SellThrulst.Add(new ROSellThruList(sellThru));
                }
            }
            return SellThrulst;
        }



        #endregion

        override public bool MethodSetData(ROMethodProperties methodProperties, bool processingApply)
        {

            ROPlanningModifySalesProperties roMethodModifySalesOtsForecastProperties = (ROPlanningModifySalesProperties)methodProperties;

            try
            {

                Method_Description = roMethodModifySalesOtsForecastProperties.Description;
                User_RID = roMethodModifySalesOtsForecastProperties.UserKey;
                Filter = roMethodModifySalesOtsForecastProperties.Filter.Key;

                HierNodeRid = roMethodModifySalesOtsForecastProperties.Merchandise.Key;
                SG_RID = roMethodModifySalesOtsForecastProperties.Attribute.Key;

                _dateRangeRid = roMethodModifySalesOtsForecastProperties.DateRange.Key;
                _filter = roMethodModifySalesOtsForecastProperties.Filter.Key;
                _averageBy = roMethodModifySalesOtsForecastProperties.AverageBy;

                int methodRID = roMethodModifySalesOtsForecastProperties.Method.Key;
                _dtGrades = BuildStoreGradeTable(roMethodModifySalesOtsForecastProperties.StoreGradeList, methodRID);

                _dtSellThru = BuildSellThruTable(roMethodModifySalesOtsForecastProperties.SellThruList, methodRID);


                _dtMatrixRules = BuildMatrixTable(roMethodModifySalesOtsForecastProperties.SalesMatrixList, methodRID);


                return true;
            }
            catch 
            {
                return false;
            }
            //throw new NotImplementedException("MethodSaveData is not implemented");
        }
        private DataTable BuildStoreGradeTable(List<ROStoreGradeList> storeGradelist, int methodRID)
        {
            DataTable dtStoreGradeDetail = new DataTable("StoreGradeDetails");
            dtStoreGradeDetail.Columns.Add("METHOD_RID", System.Type.GetType("System.Int32"));
            dtStoreGradeDetail.Columns.Add("BOUNDARY", System.Type.GetType("System.Int32"));
            dtStoreGradeDetail.Columns.Add("GRADE_CODE", System.Type.GetType("System.String"));

            foreach (var storeGradeItem in storeGradelist)
            {
                dtStoreGradeDetail.Rows.Add(methodRID, storeGradeItem.Boundary, storeGradeItem.Grade_Code);
            }

            return dtStoreGradeDetail;
        }

        private DataTable BuildSellThruTable(List<ROSellThruList> sellThrulist, int methodRID)
        {
            DataTable dtSellThruDetail = new DataTable("SellThruDetails");
            dtSellThruDetail.Columns.Add("METHOD_RID", System.Type.GetType("System.Int32"));
            dtSellThruDetail.Columns.Add("SELL_THRU", System.Type.GetType("System.Int32"));

            foreach (var sellThruItem in sellThrulist)
            {
                dtSellThruDetail.Rows.Add(methodRID,sellThruItem.Sell_Thru);
            }

            return dtSellThruDetail;
        }

        private DataTable BuildMatrixTable(List<ROMatrixRuleList> matrixRulelist, int methodRID)
        {

            DataTable dtMatrixRulesDetail = new DataTable("MatrixRulesDetails");
            dtMatrixRulesDetail.Columns.Add("METHOD_RID", System.Type.GetType("System.Int32"));
            dtMatrixRulesDetail.Columns.Add("SGL_RID", System.Type.GetType("System.Int32"));
            dtMatrixRulesDetail.Columns.Add("BOUNDARY", System.Type.GetType("System.Int32"));
            // dtMatrixRulesDetail.Columns.Add("Grade_Code", System.Type.GetType("System.String"));

            dtMatrixRulesDetail.Columns.Add("SELL_THRU", System.Type.GetType("System.String"));
            dtMatrixRulesDetail.Columns.Add("MATRIX_RULE", System.Type.GetType("System.Int32"));
            dtMatrixRulesDetail.Columns.Add("MATRIX_RULE_QUANTITY", System.Type.GetType("System.Double"));



            foreach (var ruleItem in matrixRulelist)
            {
                //ruleItem.Grade_Code,
                dtMatrixRulesDetail.Rows.Add(methodRID,ruleItem.AttributeSet.Key, ruleItem.Boundary, ruleItem.Sell_Thru, ruleItem.Matrix_Rule.Key, ruleItem.Matrix_Rule_Quantity);
            }

            return dtMatrixRulesDetail;
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }

    /// <summary>
    /// This class is used by the modify sale method to help gather and hold the count of stores in 
    /// each sgl/boundary/sell thru.
    /// </summary>
    public class StoreCounter
    {
        private Hashtable _sglHash;

        public StoreCounter()
        {
            _sglHash = new Hashtable();
        }

        /// <summary>
        /// Adds 1 to the store count for the particular keys
        /// </summary>
        /// <param name="sglRid"></param>
        /// <param name="boundary"></param>
        /// <param name="sellThru"></param>
        public void Add(int sglRid, int boundary, double sellThru)
        {
            try
            {
                if (_sglHash.ContainsKey(sglRid))
                {
                    Hashtable boundaryHash = (Hashtable)_sglHash[sglRid];
                    if (boundaryHash.ContainsKey(boundary))
                    {
                        Hashtable sellThruHash = (Hashtable)boundaryHash[boundary];
                        if (sellThruHash.ContainsKey(sellThru))
                        {
                            int currValue = (int)sellThruHash[sellThru];
                            sellThruHash[sellThru] = ++currValue;
                        }
                        else
                        {
                            // Begin Issue 4575 stodd 08.13.07
                            sellThruHash.Add(sellThru, 1);
                            // End Issue 4575
                        }
                    }
                    else
                    {
                        Hashtable sHash = new Hashtable();
                        sHash.Add(sellThru, 1);
                        boundaryHash.Add(boundary, sHash);
                    }

                }
                else
                {

                    Hashtable sHash = new Hashtable();
                    Hashtable bHash = new Hashtable();
                    sHash.Add(sellThru, 1);
                    bHash.Add(boundary, sHash);
                    _sglHash.Add(sglRid, bHash);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Get(int sglRid, int boundary, double sellThru)
        {
            try
            {
                if (_sglHash.ContainsKey(sglRid))
                {
                    Hashtable boundaryHash = (Hashtable)_sglHash[sglRid];
                    if (boundaryHash.ContainsKey(boundary))
                    {
                        Hashtable sellThruHash = (Hashtable)boundaryHash[boundary];
                        if (sellThruHash.ContainsKey(sellThru))
                        {
                            int numStores = (int)sellThruHash[sellThru];
                            return numStores;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public void Debug()
        {
            IDictionaryEnumerator sglEnum;
            IDictionaryEnumerator boundaryEnum;
            IDictionaryEnumerator sellThruEnum;

            int count;

            try
            {
                sglEnum = _sglHash.GetEnumerator();
                while (sglEnum.MoveNext())
                {
                    Hashtable bHash = (Hashtable)sglEnum.Value;
                    boundaryEnum = bHash.GetEnumerator();
                    while (boundaryEnum.MoveNext())
                    {
                        Hashtable sHash = (Hashtable)boundaryEnum.Value;
                        sellThruEnum = sHash.GetEnumerator();
                        while (sellThruEnum.MoveNext())
                        {
                            count = (int)sellThruEnum.Value;
                            System.Diagnostics.Debug.WriteLine(sglEnum.Key.ToString() + " " +
                                boundaryEnum.Key.ToString() + " " +
                                sellThruEnum.Key.ToString() + " " +
                                count.ToString());
                        }
                    }
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
