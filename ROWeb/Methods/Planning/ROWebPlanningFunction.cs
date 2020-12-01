using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using Logility.ROUI;


namespace Logility.ROWeb
{
    abstract public class ROWebPlanningFunction : ROWebFunction
    {
        //=======
        // FIELDS
        //=======

        private PlanOpenParms _openParms;
        protected ROPlanManager planManager;
        protected string sPrevUnitScaling = "1";
        protected string sPrevDollarScaling = "1";
        //protected DataSet dsCubeData;
        //protected CubeCoordinatesMap cubeCoordinatesMap;
        protected bool bAddedColumn;
        protected PagingCoordinates pagingCoordinates;
        //protected eGridOrientation gridOrientation = eGridOrientation.None;

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instace of ROWebFunction.
        /// </summary>
        /// <remarks>Creates an instance of the PlanOpenParms to manage the cubes</remarks>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROWebPlanningFunction(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            //MIDRetail.DataCommon.ePlanSessionType myPlanningType = (MIDRetail.DataCommon.ePlanSessionType)((int)planSessionType);

            //_openParms = new PlanOpenParms(planSessionType, SAB.ApplicationServerSession.GetDefaultComputations());
        }

        //===========
        // PROPERTIES
        //===========


        public PlanOpenParms OpenParms
        {
            get { return _openParms; }
        }

        protected int iAddedColumnCount
        {
            get
            {
                int locallyAddedColumnCount = bAddedColumn ? 1 : 0;

                return planManager.iAddedColumnsCount + locallyAddedColumnCount;
            }
        }

        //========
        // ABSTRACTS
        //========

        abstract protected DataTable GetForecastVersions();
        abstract protected ROOut OpenCubeGroup(ROCubeOpenParms openParams);
        abstract protected ROOut CellChanged(ROGridChangesParms gridChanges);
        abstract protected ROCubeMetadata GetROCubeMetadata(ROCubeGetMetadataParams metadataParams);
        abstract protected ROGridData GetCubeData(ROCubeGetDataParams getDataParams);
        //abstract protected void GetCubeDataSet();

        abstract public ROOut SaveCubeGroup();


        //========
        // METHODS
        //========

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.CellChanged:
                    return CellChanged((ROGridChangesParms)Parms);
                case eRORequest.UndoLastRecompute:
                    return UndoLastRecompute();
                case eRORequest.CloseCube:
                    return CloseCubeGroup();
                case eRORequest.SaveCubeGroup:
                    return SaveCubeGroup();
                case eRORequest.SelectVariables:
                    return SelectVariables((RODataTableParms)Parms);
                case eRORequest.SelectComparatives:
                    return SelectComparatives((RODataTableParms)Parms);
                case eRORequest.OpenCube:
                    return OpenCubeGroup((ROCubeOpenParms)Parms);
                case eRORequest.GetROCubeMetadata:
                    return GetROCubeMetadata(Parms as ROCubeGetMetadataParams);
                case eRORequest.GetCubeData:
                    return GetCubeData((ROCubeGetDataParams)Parms);
                case eRORequest.RecomputeCubes:
                    RecomputeCubes();
                    return GetCubeData((ROCubeGetDataParams)Parms);
                case eRORequest.HandlePeriodChange:
                    return HandlePeriodChange((ROCubePeriodChangeParams)Parms);
                case eRORequest.GetPlanningFilterData:
                    return GetPlanningFilterControlsData();
                case eRORequest.GetVariables:
                    return GetVariables();
                case eRORequest.GetComparatives:
                    return GetComparatives();
                case eRORequest.GetViewDetails:
                    return GetViewDetails();
                case eRORequest.SaveViewDetails:
                    return SaveViewDetails((ROPlanningViewDetailsParms)Parms);
                case eRORequest.DeleteViewDetails:
                    return DeleteViewDetails();
                case eRORequest.SaveViewFormat:
                    return planManager.GetViewData.SaveViewFormat((ROViewFormatParms)Parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        protected void CreatePlanOpenParms(MIDRetail.DataCommon.ePlanSessionType planSessionType)
        {
            _openParms = new PlanOpenParms(planSessionType, SAB.ApplicationServerSession.GetDefaultComputations());
            _openParms.SetSummaryDateProfile = true;
            MIDRetail.Data.OTSPlanSelection planSelectDL = new MIDRetail.Data.OTSPlanSelection();
            LoadSelectionData(planSessionType, planSelectDL.GetPlanSelectionMainInfo(SAB.ClientServerSession.UserRID), 0);
        }

        private void LoadSelectionData(MIDRetail.DataCommon.ePlanSessionType planSessionType, DataTable aDTSelection, int passedNodeID)
        {
            try
            {
                ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                ProfileList chainPlanVersionProfList = SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);
                ProfileList chainBasisVersionProfList = SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);
                ProfileList storePlanVersionProfList = SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);
                ProfileList storeBasisVersionProfList = SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);

                if (aDTSelection.Rows.Count == 0)
                {
                    _openParms.StoreGroupRID = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
                    _openParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
                    _openParms.ViewRID = Include.DefaultPlanViewRID;
                    _openParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
                    _openParms.IneligibleStores = false;
                    _openParms.SimilarStores = true;
                    _openParms.LowLevelsType = eLowLevelsType.None;

                    if (chainPlanVersionProfList.Count > 0)
                    {
                        _openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)chainPlanVersionProfList[0];
                    }
                    else
                    {
                        _openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                    }

                    if (storePlanVersionProfList.Count > 0)
                    {
                        _openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)storePlanVersionProfList[0];
                    }
                    else
                    {
                        _openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                    }

                    if (versionProfList.Count > 0)
                    {
                        _openParms.LowLevelVersionDefault = (VersionProfile)versionProfList[0];
                    }
                    else
                    {
                        _openParms.LowLevelVersionDefault = new VersionProfile(Include.NoRID);
                    }
                    if (passedNodeID > 0)
                    {
                        _openParms.StoreHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(passedNodeID, true, true);
                        _openParms.ChainHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(passedNodeID, true, true);
                    }
                    else
                    {
                        _openParms.StoreHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                        _openParms.ChainHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                    }
                    _openParms.OverrideLowLevelRid = Include.NoRID;
                    _openParms.CustomOverrideLowLevelRid = Include.NoRID;
                    _openParms.IsLadder = false;
                    _openParms.IsMulti = false;
                    _openParms.IsTotRT = false;
                }
                else
                {
                    string computationMode;
                    if (aDTSelection.Rows[0]["CALC_MODE"] == System.DBNull.Value)
                    {
                        computationMode = SAB.ApplicationServerSession.GetDefaultComputations();
                    }
                    else
                    {
                        computationMode = Convert.ToString(aDTSelection.Rows[0]["CALC_MODE"], CultureInfo.CurrentUICulture);
                    }

                    if (aDTSelection.Rows[0]["SG_RID"] == System.DBNull.Value)
                    {
                        _openParms.StoreGroupRID = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
                    }
                    else
                    {
                        _openParms.StoreGroupRID = Convert.ToInt32(aDTSelection.Rows[0]["SG_RID"], CultureInfo.CurrentUICulture);
                    }
                    _openParms.FilterRID = Convert.ToInt32(aDTSelection.Rows[0]["FILTER_RID"], CultureInfo.CurrentUICulture);
                    _openParms.GroupBy = (eStorePlanSelectedGroupBy)TypeDescriptor.GetConverter(_openParms.GroupBy).ConvertFrom(aDTSelection.Rows[0]["GROUP_BY_ID"].ToString());
                    if (aDTSelection.Rows[0]["VIEW_RID"] == System.DBNull.Value)
                    {
                        _openParms.ViewRID = Include.DefaultPlanViewRID;
                    }
                    else
                    {
                        _openParms.ViewRID = Convert.ToInt32(aDTSelection.Rows[0]["VIEW_RID"], CultureInfo.CurrentUICulture);
                    }
                    if (passedNodeID > 0)
                    {
                        _openParms.StoreHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(passedNodeID, true, true);
                    }
                    else
                    {
                        if (aDTSelection.Rows[0]["STORE_HN_RID"] == System.DBNull.Value)
                        {
                            _openParms.StoreHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                        }
                        else
                        {
                            _openParms.StoreHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["STORE_HN_RID"], CultureInfo.CurrentUICulture), true, true);
                        }
                    }
                    if (aDTSelection.Rows[0]["STORE_FV_RID"] == System.DBNull.Value)
                    {
                        if (storePlanVersionProfList.Count > 0)
                        {
                            _openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)storePlanVersionProfList[0];
                        }
                        else
                        {
                            _openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                        }
                    }
                    else
                    {
                        _openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["STORE_FV_RID"], CultureInfo.CurrentUICulture));
                    }
                    if (aDTSelection.Rows[0]["TIME_PERIOD_CDR_RID"] == System.DBNull.Value)
                    {
                        _openParms.DateRangeProfile = null;
                    }
                    else
                    {
                        _openParms.DateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(aDTSelection.Rows[0]["TIME_PERIOD_CDR_RID"], CultureInfo.CurrentUICulture));
                    }
                    _openParms.DisplayTimeBy = (eDisplayTimeBy)TypeDescriptor.GetConverter(_openParms.DisplayTimeBy).ConvertFrom(aDTSelection.Rows[0]["DISPLAY_TIME_BY_ID"].ToString());
                    if (passedNodeID > 0)
                    {
                        _openParms.ChainHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(passedNodeID, true, true);
                    }
                    else
                    {
                        if (aDTSelection.Rows[0]["CHAIN_HN_RID"] == System.DBNull.Value)
                        {
                            _openParms.ChainHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                        }
                        else
                        {
                            _openParms.ChainHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_HN_RID"], CultureInfo.CurrentUICulture), true, true);
                        }
                    }
                    if (aDTSelection.Rows[0]["CHAIN_FV_RID"] == System.DBNull.Value)
                    {
                        if (chainPlanVersionProfList.Count > 0)
                        {
                            _openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)chainPlanVersionProfList[0];
                        }
                        else
                        {
                            _openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                        }
                    }
                    else
                    {
                        _openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_FV_RID"], CultureInfo.CurrentUICulture));
                    }
                    _openParms.IneligibleStores = Include.ConvertCharToBool(Convert.ToChar(aDTSelection.Rows[0]["INCLUDE_INELIGIBLE_STORES_IND"], CultureInfo.CurrentUICulture));
                    _openParms.SimilarStores = Include.ConvertCharToBool(Convert.ToChar(aDTSelection.Rows[0]["INCLUDE_SIMILAR_STORES_IND"], CultureInfo.CurrentUICulture));

                    if (aDTSelection.Rows[0]["LOW_LEVEL_FV_RID"] == System.DBNull.Value)
                    {
                        if (versionProfList.Count > 0)
                        {
                            _openParms.LowLevelVersionDefault = (VersionProfile)versionProfList[0];
                        }
                        else
                        {
                            _openParms.LowLevelVersionDefault = new VersionProfile(Include.NoRID);
                        }
                    }
                    else
                    {
                        _openParms.LowLevelVersionDefault = (VersionProfile)versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_FV_RID"], CultureInfo.CurrentUICulture));
                    }
                    if (aDTSelection.Rows[0]["LOW_LEVEL_TYPE"] == System.DBNull.Value)
                    {
                        _openParms.LowLevelsType = eLowLevelsType.None;
                    }
                    else
                    {
                        _openParms.LowLevelsType = (eLowLevelsType)Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["LOW_LEVEL_OFFSET"] != System.DBNull.Value)
                    {
                        _openParms.LowLevelsOffset = Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["LOW_LEVEL_SEQUENCE"] != System.DBNull.Value)
                    {
                        _openParms.LowLevelsSequence = Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_SEQUENCE"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["OLL_RID"] == System.DBNull.Value)
                    {
                        _openParms.OverrideLowLevelRid = Include.NoRID;
                        _openParms.CustomOverrideLowLevelRid = Include.NoRID;
                    }
                    else
                    {
                        _openParms.OverrideLowLevelRid = Convert.ToInt32(aDTSelection.Rows[0]["OLL_RID"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["CUSTOM_OLL_RID"] == System.DBNull.Value)
                    {
                        _openParms.CustomOverrideLowLevelRid = Include.NoRID;
                    }
                    else
                    {
                        _openParms.CustomOverrideLowLevelRid = Convert.ToInt32(aDTSelection.Rows[0]["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
                    }

                    if (aDTSelection.Rows[0]["IS_LADDER"] == System.DBNull.Value
                        || planSessionType != ePlanSessionType.ChainSingleLevel)
                    {
                        _openParms.IsLadder = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(aDTSelection.Rows[0]["IS_LADDER"], CultureInfo.CurrentUICulture) == 0)
                        {
                            _openParms.IsLadder = false;
                        }
                        else
                        {
                            _openParms.IsLadder = true;
                        }
                    }
                    if (aDTSelection.Rows[0]["IS_MULTI"] == System.DBNull.Value)
                    {
                        _openParms.IsMulti = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(aDTSelection.Rows[0]["IS_MULTI"], CultureInfo.CurrentUICulture) == 0)
                        {
                            _openParms.IsMulti = false;
                        }
                        else
                        {
                            _openParms.IsMulti = true;
                        }
                    }

                    if (aDTSelection.Rows[0]["TOT_RIGHT"] == System.DBNull.Value)
                    {
                        _openParms.IsTotRT = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(aDTSelection.Rows[0]["TOT_RIGHT"], CultureInfo.CurrentUICulture) == 0)
                        {
                            _openParms.IsTotRT = false;
                        }
                        else
                        {
                            _openParms.IsTotRT = true;
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

        protected int GetViewRID(string sView, out int iViewUserID)
        {
            int iViewRID = Include.NoRID;
            iViewUserID = Include.NoRID;

            PlanViewData viewDL = new PlanViewData();
            ArrayList userRIDList = new ArrayList();
            userRIDList.Add(Include.GlobalUserRID);
            DataTable dt = viewDL.PlanView_Read(userRIDList);
            DataRow[] dr = dt.Select(@"VIEW_ID = '" + sView + @"'");
            if (dr.Length > 0)
            {
                iViewUserID = Convert.ToInt32(dr[0]["USER_RID"]);
                iViewRID = Convert.ToInt32(dr[0]["VIEW_RID"]);
            }

            return iViewRID;
        }
        protected int GetViewRID(int iViewKey, out int iViewUserID, out string sViewName)
        {
            int iViewRID = Include.NoRID;
            iViewUserID = Include.NoRID;
            sViewName = null;

            PlanViewData viewDL = new PlanViewData();
            ArrayList userRIDList = new ArrayList();
            userRIDList.Add(Include.GlobalUserRID);
            DataTable dt = viewDL.PlanView_Read(userRIDList);
            DataRow[] dr = dt.Select(@"VIEW_RID = " + iViewKey + @"");
            if (dr.Length > 0)
            {
                iViewUserID = Convert.ToInt32(dr[0]["USER_RID"]);
                iViewRID = Convert.ToInt32(dr[0]["VIEW_RID"]);
                sViewName = Convert.ToString(dr[0]["VIEW_ID"]);
            }

            return iViewRID ;
        }

        protected HierarchyNodeProfile GetNodeProfile(int nodeRID)
        {
            return SAB.HierarchyServerSession.GetNodeData(nodeRID, true, false);
        }

        protected VersionProfile GetVersionProfile(string sVersion, ePlanType planType, ePlanBasisType planBasisType)
        {
            ProfileList chainPlanVersionProfList = GetVersionProfileList(planType, planBasisType);
            foreach (VersionProfile vp in chainPlanVersionProfList)
            {
                if (vp.Description == sVersion)
                {
                    return vp;
                }
            }
            ROWebTools.LogMessage(eROMessageLevel.Error, "Could not find version " + sVersion);
            throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
        }

        protected VersionProfile GetVersionProfile(int version, ePlanType planType, ePlanBasisType planBasisType)
        {
            ProfileList chainPlanVersionProfList = GetVersionProfileList(planType, planBasisType);
            foreach (VersionProfile vp in chainPlanVersionProfList)
            {
                if (vp.Key == version)
                {
                    return vp;
                }
            }
            ROWebTools.LogMessage(eROMessageLevel.Error, "Could not find version " + version);
            throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
        }

        protected ProfileList GetVersionProfileList(ePlanType planType, ePlanBasisType planBasisType)
        {
            if (planType == ePlanType.Chain)
            {
                if (planBasisType == ePlanBasisType.Plan)
                {
                    return SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);
                }
                else
                {
                    return SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);
                }
            }
            else
            {
                if (planBasisType == ePlanBasisType.Plan)
                {
                    return SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);
                }
                else
                {
                    return SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);
                }
            }

        }

        protected FunctionSecurityProfile GetFunctionSecurity(ePlanType planType, bool bIsMultiLevel)
        {
            if (planType == ePlanType.Chain)
            {
                if (bIsMultiLevel)
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelChain);
                }
                else
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
                }
            }
            else
            {
                if (bIsMultiLevel)
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelStore);
                }
                else
                {
                    return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelStore);
                }
            }

        }

        protected HierarchyNodeSecurityProfile GetNodeSecurity(ePlanType planType, int iNodeRID)
        {
            if (planType == ePlanType.Chain)
            {
                return SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(iNodeRID, (int)eSecurityTypes.Chain);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(iNodeRID, (int)eSecurityTypes.Store);
            }

        }

        protected DateRangeProfile GetDateRangeProfile(int cdrRID)
        {
            return SAB.ApplicationServerSession.Calendar.GetDateRange(cdrRID);
            //return SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(Convert.ToInt32(sFromWeekYYYYWW), Convert.ToInt32(sToWeekYYYYWW));
        }

        protected ROOut GetPlanningFilterControlsData()
        {
            DataSet ds = new DataSet();

            AddViewsTable(ds);
            AddSessionTypesTable(ds);
            AddPeriodsTable(ds);
            AddVersionsTable(ds);
            AddUnitsScalingTable(ds);
            AddDollarsScalingTable(ds);

            return new RODataSetOut(eROReturnCode.Successful, null, ROInstanceID, ds);
        }

        protected void AddSessionTypesTable(DataSet ds)
        {
            DataTable dt = new DataTable("Session Types");

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            foreach (MIDRetail.DataCommon.ePlanSessionType sessionType in Enum.GetValues(typeof(MIDRetail.DataCommon.ePlanSessionType)))
            {
                if (sessionType == MIDRetail.DataCommon.ePlanSessionType.None)
                {
                    continue;
                }

                DataRow dr = dt.NewRow();

                dr["ID"] = (int)sessionType;
                dr["NAME"] = sessionType.ToString();

                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
        }

        protected void AddPeriodsTable(DataSet ds)
        {
            DataTable dt = new DataTable("Periods");

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            foreach (eTimePeriod timePeriod in Enum.GetValues(typeof(eTimePeriod)))
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = (int)timePeriod;
                dr["NAME"] = timePeriod.ToString();

                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
        }

        protected void AddVersionsTable(DataSet ds)
        {
            DataTable dt = GetForecastVersions();

            ds.Tables.Add(dt);
        }

        protected void AddViewsTable(DataSet ds)
        {
            DataTable dt = GetViews();

            ds.Tables.Add(dt);
        }

        protected void AddUnitsScalingTable(DataSet ds)
        {
            DataTable dt = ScalingUnits_GetDataTable();

            ds.Tables.Add(dt);
        }

        protected void AddDollarsScalingTable(DataSet ds)
        {
            DataTable dt = ScalingDollar_GetDataTable();

            ds.Tables.Add(dt);
        }

        public DataTable GetViews()
        {
            DataTable dt = new DataTable("Views");
            DataRow dr;

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            PlanViewData viewDL = new PlanViewData();
            ArrayList userRIDList = new ArrayList();
            userRIDList.Add(Include.GlobalUserRID);

            DataTable dtViews = viewDL.PlanView_Read(userRIDList);

            foreach (DataRow drView in dtViews.Rows)
            {
                dr = dt.NewRow();

                dr["ID"] = drView["VIEW_RID"];
                dr["NAME"] = drView["VIEW_ID"];

                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected void AddBasisToOpenParms(List<ROBasisProfile> basisProfiles)
        {
            try
            {
                _openParms.BasisProfileList.Clear();
                BasisProfile basisProfile;
                BasisDetailProfile basisDetailProfile;
                ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                foreach (var item in basisProfiles)
                {
                    basisProfile = new BasisProfile(Convert.ToInt32(item.BasisId, CultureInfo.CurrentUICulture), Convert.ToString(item.BasisName, CultureInfo.CurrentUICulture), _openParms);
                    for (int i = 0; i < item.BasisDetailProfiles.Count; i++)
                    {
                        basisDetailProfile = new BasisDetailProfile(i + 1, _openParms);
                        basisDetailProfile.VersionProfile = (VersionProfile)versionProfList.FindKey((Convert.ToInt32(item.BasisDetailProfiles[i].VersionId, CultureInfo.CurrentUICulture)));

                        switch (_openParms.PlanSessionType)
                        {
                            case MIDRetail.DataCommon.ePlanSessionType.ChainSingleLevel:
                                basisDetailProfile.HierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(item.BasisDetailProfiles[i].MerchandiseId, CultureInfo.CurrentUICulture), true, true);
                                basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
                                break;
                            case MIDRetail.DataCommon.ePlanSessionType.ChainMultiLevel:
                                basisDetailProfile.HierarchyNodeProfile = _openParms.ChainHLPlanProfile.NodeProfile;
                                basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
                                break;
                            case MIDRetail.DataCommon.ePlanSessionType.StoreMultiLevel:
                                basisDetailProfile.HierarchyNodeProfile = _openParms.StoreHLPlanProfile.NodeProfile;
                                basisDetailProfile.HierarchyNodeProfile.StoreSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Store);
                                basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
                                break;
                            default:
                                basisDetailProfile.HierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(item.BasisDetailProfiles[i].MerchandiseId, CultureInfo.CurrentUICulture), true, true);
                                basisDetailProfile.HierarchyNodeProfile.StoreSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Store);
                                basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
                                break;
                        }
                        //basisDetailProfile.DateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(item.BasisDetailProfiles[i].DateRangeId, CultureInfo.CurrentUICulture));
                        //basisDetailProfile.DateRangeProfile.DisplayDate = Convert.ToString(item.BasisDetailProfiles[i].DateRange, CultureInfo.CurrentUICulture);
                        if (_openParms.DateRangeProfile.Key != Include.UndefinedCalendarDateRange)
                        {
                            basisDetailProfile.DateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(item.BasisDetailProfiles[i].DateRangeId), _openParms.DateRangeProfile.Key);
                        }
                        else
                        {
                            basisDetailProfile.DateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(item.BasisDetailProfiles[i].DateRangeId), SAB.ClientServerSession.Calendar.CurrentDate);
                        }
                        basisDetailProfile.DateRangeProfile.Name = "Basis Total";
                        if (Convert.ToBoolean(item.BasisDetailProfiles[i].IsIncluded, CultureInfo.CurrentUICulture) == true)
                        {
                            basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
                        }
                        else
                        {
                            basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Exclude;
                        }

                        basisDetailProfile.Weight = Convert.ToSingle(item.BasisDetailProfiles[i].Weight, CultureInfo.CurrentUICulture);
                        DateRangeProfile planDrp = SAB.ApplicationServerSession.Calendar.GetDateRange(_openParms.DateRangeProfile.Key);
                        ProfileList weekRange = SAB.ApplicationServerSession.Calendar.GetWeekRange(planDrp, null);
                        basisDetailProfile.ForecastingInfo = new BasisDetailForecastInfo();
                        basisDetailProfile.ForecastingInfo.PlanWeek = (WeekProfile)weekRange[0];
                        basisDetailProfile.ForecastingInfo.OrigWeekListCount = basisDetailProfile.GetWeekProfileList(SAB.ApplicationServerSession).Count;
                        basisDetailProfile.ForecastingInfo.BasisPeriodList = SAB.ApplicationServerSession.Calendar.GetDateRangePeriods(basisDetailProfile.DateRangeProfile, (WeekProfile)weekRange[0]);

                        basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
                    }
                    _openParms.BasisProfileList.Add(basisProfile);
                }


            }
            catch (Exception exc)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "AddBasisToOpenParms failed: " + exc.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        #region "Scaling"
        public DataTable ScalingDollar_GetDataTable()
        {
            DataTable dtDollarScaling = new DataTable("Dollar Scaling");
            dtDollarScaling.Columns.Add("TEXT_CODE", typeof(int));
            dtDollarScaling.Columns.Add("TEXT_VALUE", typeof(string));

            DataTable dtText = MIDText.GetTextType(eMIDTextType.eDollarScaling, eMIDTextOrderBy.TextValue);

            DataRow dr = dtDollarScaling.NewRow();
            dr["TEXT_CODE"] = (int)eDollarScaling.Ones;
            dr["TEXT_VALUE"] = "1";
            dtDollarScaling.Rows.Add(dr);

            foreach (DataRow row in dtText.Rows)
            {
                if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
                {
                    DataRow dr2 = dtDollarScaling.NewRow();
                    dr2["TEXT_CODE"] = Convert.ToInt32(row["TEXT_CODE"]);
                    dr2["TEXT_VALUE"] = Convert.ToString(row["TEXT_VALUE"]);
                    dtDollarScaling.Rows.Add(dr2);

                }
            }

            return dtDollarScaling;

        }

        public int ScalingDollar_GetDefaultValue()
        {
            return (int)eDollarScaling.Ones;
        }

        public DataTable ScalingUnits_GetDataTable()
        {
            DataTable dtUnitsScaling = new DataTable("Unit Scaling");
            dtUnitsScaling.Columns.Add("TEXT_CODE", typeof(int));
            dtUnitsScaling.Columns.Add("TEXT_VALUE", typeof(string));

            DataTable dtText = MIDText.GetTextType(eMIDTextType.eUnitScaling, eMIDTextOrderBy.TextValue);

            DataRow dr = dtUnitsScaling.NewRow();
            dr["TEXT_CODE"] = (int)eUnitScaling.Ones;
            dr["TEXT_VALUE"] = "1";
            dtUnitsScaling.Rows.Add(dr);

            foreach (DataRow row in dtText.Rows)
            {

                if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
                {
                    DataRow dr2 = dtUnitsScaling.NewRow();
                    dr2["TEXT_CODE"] = Convert.ToInt32(row["TEXT_CODE"]);
                    dr2["TEXT_VALUE"] = Convert.ToString(row["TEXT_VALUE"]);
                    dtUnitsScaling.Rows.Add(dr2);
                }
            }

            return dtUnitsScaling;
        }

        public int ScalingUnits_GetDefaultValue()
        {
            return (int)eUnitScaling.Ones;
        }
        #endregion // scaling

        public ROOut GetVariables()
        {
            DataTable dt = new DataTable("Variables");
            dt.Columns.Add("Variable", typeof(string));
            dt.Columns.Add("IsDisplayed", typeof(bool));
            ArrayList variables = planManager.GetSelectableVariableHeaders();

            foreach (RowColProfileHeader variable in variables)
            {
                DataRow dr = dt.NewRow();
                dr["IsDisplayed"] = variable.IsDisplayed;
                dr["Variable"] = variable.Name;
                dt.Rows.Add(dr);
                //dt.Rows.Add(variable.Name);
            }

            return new RODataTableOut(eROReturnCode.Successful, null, ROInstanceID, dt);
        }

        public ROOut SelectVariables(RODataTableParms selectedVariables)
        {
            return SelectCubeValues(planManager.GetSelectableVariableHeaders(), "Variable", selectedVariables.dtValue, true);
        }

        public ROOut GetComparatives()
        {
            DataTable dt = new DataTable("Cmparatives");
            dt.Columns.Add("Comparative", typeof(string));
            dt.Columns.Add("IsDisplayed", typeof(bool));
            ArrayList comparatives = planManager.GetSelectableQuantityHeaders();

            foreach (RowColProfileHeader comparative in comparatives)
            {
                DataRow dr = dt.NewRow();
                dr["IsDisplayed"] = comparative.IsDisplayed;
                dr["Comparative"] = comparative.Name;
                dt.Rows.Add(dr);
                //dt.Rows.Add(comparative.Name);
            }

            return new RODataTableOut(eROReturnCode.Successful, null, ROInstanceID, dt);
        }

        public ROOut SelectComparatives(RODataTableParms selecteComparables)
        {
            return SelectCubeValues(planManager.GetSelectableQuantityHeaders(), "Comparative", selecteComparables.dtValue, false);
        }

        public ROOut GetViewDetails()
        {
            ePlanType planType;

            // Add variable groupings and selected variables
            ArrayList variables = planManager.GetSelectableVariableHeaders();
            if (planManager.OpenParms.PlanSessionType == ePlanSessionType.StoreMultiLevel
                || planManager.OpenParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
            {
                planType = ePlanType.Store;
            }
            else
            {
                planType = ePlanType.Chain;
            }

            ApplicationSessionTransaction applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
            ArrayList al = applicationSessionTransaction.PlanComputations.PlanVariables.GetVariableGroupings();
            ROVariableGroupings variableList = VariableGroupings.BuildVariableGroupings(planType, variables, al);

            ROPlanningViewDetails viewDetails = new ROPlanningViewDetails(
                view: GetName.GetForecastViewName(key: OpenParms.ViewRID, userKey: SAB.ClientServerSession.UserRID),
                ROVariableGroupings: variableList);

            // Add comparatives
            ArrayList comparatives = planManager.GetSelectableQuantityHeaders();

            foreach (RowColProfileHeader comparative in comparatives)
            {
                viewDetails.Comparatives.Add(new ROSelectedField(fieldkey: comparative.Profile.Key.ToString(),
                    field: comparative.Name,
                    selected: comparative.IsDisplayed)
                    );
            }

            // Add time selections
            bool selectYear;
            bool selectSeason;
            bool selectQuarter;
            bool selectMonth;
            bool selectWeek;

            selectMonth = planManager.ShowMonths();
            selectWeek = planManager.ShowWeeks();

            if (planType == ePlanType.Chain)
            {
                selectYear = planManager.ShowYears();
                selectSeason = planManager.ShowSeasons();
                selectQuarter = planManager.ShowQuarters();

                viewDetails.TimePeriods.Add(new ROSelectedField(fieldkey: ((int)eProfileType.Year).ToString(),
                   field: "Show Years",
                   selected: selectYear)
                   );

                viewDetails.TimePeriods.Add(new ROSelectedField(fieldkey: ((int)eProfileType.Season).ToString(),
                   field: "Show Seasons",
                   selected: selectSeason)
                   );

                viewDetails.TimePeriods.Add(new ROSelectedField(fieldkey: ((int)eProfileType.Quarter).ToString(),
                   field: "Show Quarters",
                   selected: selectQuarter)
                   );

                if (!selectYear && !selectSeason && !selectQuarter && !selectMonth && !selectWeek)
                {
                    selectMonth = true;
                }
            }
            else
            {
                if (!selectMonth && !selectWeek)
                {
                    selectMonth = true;
                }
            }

            viewDetails.TimePeriods.Add(new ROSelectedField(fieldkey: ((int)eProfileType.Month).ToString(),
                   field: "Show Months",
                   selected: selectMonth)
                   );

            viewDetails.TimePeriods.Add(new ROSelectedField(fieldkey: ((int)eProfileType.Week).ToString(),
                   field: "Show Weeks",
                   selected: selectWeek)
                   );

            return new ROPlanningViewDetailsOut(eROReturnCode.Successful, null, ROInstanceID, viewDetails);
        }

        /// <summary>
        /// Save Planning View Details to the database.
        /// </summary>
        /// <param name="viewDetails">An instance of the ROPlanningViewDetailsParms class containing the view settings
        /// </param>
        /// <returns>An instance of the ROPlanningViewDetailsOut class with the updated view settings
        /// </returns>
        public ROOut SaveViewDetails(ROPlanningViewDetailsParms viewDetails)
        {
            string message = null;
            PlanViewData planViewData;
            int viewRID = Include.NoRID, viewUserRID;
            Audit audit = SAB.ClientServerSession.Audit;

            // make sure at least one variable is selected
            if (viewDetails.ROPlanningViewDetails.VariableGroupings.SelectedVariables.Count == 0)
            {
                return new ROPlanningViewDetailsOut(
                    ROReturnCode: eROReturnCode.Failure, 
                    sROMessage: audit.GetText(messageCode: eMIDTextCode.msg_NeedAtLeastOneVariable, addToAuditReport: true), 
                    ROInstanceID: ROInstanceID, 
                    ROPlanningViewDetails: viewDetails.ROPlanningViewDetails);
            }

            planViewData = new PlanViewData();

            // Update variables
            ArrayList selectableVariableHeaders = planManager.GetSelectableVariableHeaders();
            List<ROSelectedField> selectedVariables = viewDetails.ROPlanningViewDetails.VariableGroupings.SelectedVariables;
            foreach (RowColProfileHeader variable in selectableVariableHeaders)
            {
                ROSelectedField selectableVariable = selectedVariables.Find(v => v.Field.Key == variable.Profile.Key.ToString());
                if (selectableVariable != null)
                {
                    variable.IsDisplayed = true;
                    variable.Sequence = selectedVariables.FindIndex(v => v.Field.Key == variable.Profile.Key.ToString());
                }
                else
                {
                    variable.IsDisplayed = false;
                    variable.Sequence = Include.Undefined;
                }
            }

            // Update comparatives
            ArrayList selectableQuantityHeaders = planManager.GetSelectableQuantityHeaders();
            List<ROSelectedField> comparatives = viewDetails.ROPlanningViewDetails.Comparatives;
            foreach (RowColProfileHeader comparative in selectableQuantityHeaders)
            {
                ROSelectedField selectableComparative = comparatives.Find(v => v.Field.Key == comparative.Profile.Key.ToString());
                if (selectableComparative != null)
                {
                    comparative.IsDisplayed = selectableComparative.IsSelected;
                }
                else
                {
                    comparative.IsDisplayed = false;
                }
            }

            // Update time periods
            ArrayList selectablePeriodHeaders = new ArrayList();

            foreach (ROSelectedField timeSelection in viewDetails.ROPlanningViewDetails.TimePeriods)
            {
                selectablePeriodHeaders.Add(
                    new RowColProfileHeader(
                        aName: timeSelection.Field.Value,
                        aIsDisplayed: timeSelection.IsSelected,
                        aSequence: Convert.ToInt32(timeSelection.Field.Key),
                        aProfile: null)
                );
            }

            try
            {
                planViewData.OpenUpdateConnection();

                if (viewDetails.ROPlanningViewDetails.IsUserView)
                {
                    viewUserRID = SAB.ClientServerSession.UserRID;
                }
                else
                {
                    viewUserRID = Include.GlobalUserRID;
                }

                viewRID = planViewData.PlanView_GetKey(
                    aUserRID: viewUserRID, 
                    aViewID: viewDetails.ROPlanningViewDetails.View.Value
                    );

                if (viewRID != -1)
                {
                    planViewData.PlanViewDetail_Delete(
                        aViewRID: viewRID
                        );
                }
                else
                {
                    viewRID = planViewData.PlanView_Insert(
                        aUserRID: viewUserRID, 
                        aViewID: viewDetails.ROPlanningViewDetails.View.Value, 
                        aGroupBy: eStorePlanSelectedGroupBy.ByTimePeriod
                        );
                    string viewName = viewDetails.ROPlanningViewDetails.View.Value;
                    viewDetails.ROPlanningViewDetails.View = new KeyValuePair<int, string>(viewRID, viewName);
                }

                planViewData.PlanViewDetail_Insert(
                    aViewRID: viewRID,
                    aVariableHeaders: selectableVariableHeaders,
                    aQuantityHeaders: selectableQuantityHeaders,
                    aPeriodHeaders: selectablePeriodHeaders
                    );
                planViewData.CommitData();
            }
            catch (Exception exc)
            {
                planViewData.Rollback();
                message = exc.ToString();
                throw;
            }
            finally
            {
                planViewData.CloseUpdateConnection();
                if (viewRID == planManager.GetViewRID())
                {
                    planManager.GetViewData.ViewUpdated = true;
                }
            }

            return new ROPlanningViewDetailsOut(
                ROReturnCode: eROReturnCode.Successful,
                sROMessage: message,
                ROInstanceID: ROInstanceID,
                ROPlanningViewDetails: viewDetails.ROPlanningViewDetails
                );
        }

        public ROOut DeleteViewDetails()   
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;
            bool successful = true;

            int view_RID = planManager.GetViewRID();
            if (view_RID == Include.DefaultPlanViewRID)
            {
                message = "Default view cannot be deleted";
                returnCode = eROReturnCode.Failure;
                successful = false;
            }
            else
            {
                PlanViewData data = new PlanViewData();

                try
                {
                    if (view_RID > 0)
                    {
                        data.OpenUpdateConnection();
                        if (data.PlanView_Delete(view_RID) > 0)
                        {
                            data.CommitData();
                        }
                        else
                        {
                            message = "View delete failed";
                            returnCode = eROReturnCode.Failure;
                            successful = false;
                        }
                    }
                    else
                    {
                        message = "View not selected";
                        returnCode = eROReturnCode.Failure;
                        successful = false;
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    returnCode = eROReturnCode.Failure;
                    successful = false;
                }
                finally
                {
                    data.CloseUpdateConnection();
                }
            }

            return new ROBoolOut(returnCode, message, ROInstanceID, successful);
        }

        private ROOut SelectCubeValues(ArrayList headersList, string sColumnName, DataTable dtSelected, bool setSequence)
        {
            Dictionary<string, RowColProfileHeader> selectableVariables = new Dictionary<string, RowColProfileHeader>();

            foreach (RowColProfileHeader variable in headersList)
            {
                if (setSequence)
                {
                    variable.Sequence = -1;
                }
                variable.IsDisplayed = false;
                selectableVariables.Add(variable.Name, variable);
            }

            int seqNum = 0;

            foreach (DataRow dr in dtSelected.Rows)
            {
                string variableName = dr[sColumnName].ToString();
                RowColProfileHeader header;

                if (selectableVariables.TryGetValue(variableName, out header))
                {
                    if (setSequence)
                    {
                        header.Sequence = seqNum++;
                    }
                    header.IsDisplayed = true;
                }
            }
            //planManager.ReconstructCubeCoordinatesAndDataset();
            //dsCubeData = null;

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        /// <summary>
        /// Request the recompile formulas are executed for the changed cells
        /// </summary>
        public void RecomputeCubes()
        {
            planManager.RecomputePlanCubes();
            planManager.RebuildGridData();
            //dsCubeData = null;
        }

        public ROOut UndoLastRecompute()
        {
            planManager.UndoLastRecompute();
            planManager.RebuildGridData();
            //dsCubeData = null;

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }


        /// <summary>
        /// Disposes of any internal resources used to manage cube group data.
        /// </summary>
        public ROOut CloseCubeGroup()
        {
            if (planManager != null)
            {
                //dsCubeData = null;
                //cubeCoordinatesMap = null;
                planManager.CubeGroup.CloseCubeGroup();
                planManager.CubeGroup.Dispose();
                planManager = null;
            }

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        public DataTable GetUnitScalings()
        {
            return planManager.ScalingUnits_GetDataTable();
        }

        public DataTable GetDollarScalings()
        {
            return planManager.ScalingDollar_GetDataTable();
        }

        protected DateRangeProfile getDateRangeProfileByName(string sProfileName)
        {
            CalendarDateSelectorManager mgr = new CalendarDateSelectorManager(SAB);
            int cdrRID = mgr.getCDRRIDForRangeName(sProfileName);
            DateRangeProfile profile = null;

            if (cdrRID != Include.UndefinedCalendarDateRange)
            {
                profile = mgr.GetDateRangeProfile(cdrRID);
            }

            return profile;
        }

        /// <summary>
        /// Update what time periods to get data for
        /// </summary>
        /// <param name="periodChangeParams">The data to use in determining what tables to return</param>
        /// <returns>The updated meta-data for the DataSet</returns>
        virtual protected ROCubeMetadata HandlePeriodChange(ROCubePeriodChangeParams periodChangeParams)
        {
            return null;
        }

        #region "Code for Jira RO-1380"
        protected bool SaveOTSForecast(ROCubeOpenParms openParms)
        {
            MIDRetail.Data.OTSPlanSelection planSelectDL = new MIDRetail.Data.OTSPlanSelection();

            planSelectDL.SavePlanSelection(SAB.ClientServerSession.UserRID, OpenParms, ConvertBasisListDataset(openParms.BasisProfiles));

            return true;
        }

        private DataSet ConvertBasisListDataset(List<ROBasisProfile> basisProfiles)
        {
            MIDRetail.Data.OTSPlanSelection planSelectDL = new MIDRetail.Data.OTSPlanSelection();
            DataSet dsBasis = MIDEnvironment.CreateDataSet();
            DataTable dtBasis = planSelectDL.SetupBasisTable();
            dtBasis.TableName = "Basis";

            DataTable dtBasisDetails = planSelectDL.SetupBasisDetailsTable();
            dtBasisDetails.TableName = "BasisDetails";

            foreach (var basisProfile in basisProfiles)
            {
                DataRow row = dtBasis.NewRow();
                row["BasisID"] = basisProfile.BasisId;
                row["BasisName"] = basisProfile.BasisName;
                dtBasis.Rows.Add(row);

                foreach (var basisDetailsProfile in basisProfile.BasisDetailProfiles)
                {
                    DataRow rowDtlProf = dtBasisDetails.NewRow();
                    rowDtlProf["BasisID"] = basisDetailsProfile.BasisId;
                    rowDtlProf["Merchandise"] = basisDetailsProfile.Merchandise;
                    rowDtlProf["MerchandiseID"] = basisDetailsProfile.MerchandiseId;
                    rowDtlProf["Version"] = basisDetailsProfile.Version;
                    rowDtlProf["VersionID"] = basisDetailsProfile.VersionId;
                    rowDtlProf["DateRange"] = basisDetailsProfile.DateRange;
                    rowDtlProf["DateRangeID"] = basisDetailsProfile.DateRangeId;
                    rowDtlProf["Picture"] = basisDetailsProfile.Picture;
                    rowDtlProf["Weight"] = basisDetailsProfile.Weight;
                    rowDtlProf["IsIncluded"] = basisDetailsProfile.IsIncluded;
                    rowDtlProf["IncludeButton"] = basisDetailsProfile.IncludeButton;
                    dtBasisDetails.Rows.Add(rowDtlProf);
                }

            }
            dsBasis.Tables.Add(dtBasis);
            dsBasis.Tables.Add(dtBasisDetails);
            return dsBasis;
        }
        #endregion
    }


}
