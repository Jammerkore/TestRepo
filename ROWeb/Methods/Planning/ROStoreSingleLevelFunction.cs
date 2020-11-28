using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;

using Logility.ROWebCommon;

// BEGIN TT#1156-MD CTeegarden - make cube functionality generic
using Logility.ROWebSharedTypes;
using Logility.ROUI;

namespace Logility.ROWeb
{
    public class ROStoreSingleLevel : ROStoreFunction
    {

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="sessionType">the type of planning session (store/chain and single/multi level)</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROStoreSingleLevel(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {

        }

        /// <summary>
        /// Disposes of any internal resources used to manage cube group data.
        /// </summary>
        public override void CleanUp()
        {
            CloseCubeGroup();
        }

        /// <summary>
        /// Sets the parameters needed to open the cube
        /// </summary>
        /// <param name="openParams">The details for what data to load in the cube group</param>
        override protected ROOut OpenCubeGroup(ROCubeOpenParms openParams)
        {
            int iViewUserID;
            string sViewName;
            CreatePlanOpenParms(openParams.ePlanSessionType);

            if (openParams.ViewKey != Include.Undefined)
            {
                OpenParms.ViewRID = GetViewRID(openParams.ViewKey, out iViewUserID, out sViewName);
                OpenParms.ViewUserID = iViewUserID;
                OpenParms.ViewName = sViewName;
            }
            else
            {
                OpenParms.ViewName = openParams.sView;
                OpenParms.ViewRID = GetViewRID(openParams.sView, out iViewUserID);
                OpenParms.ViewUserID = iViewUserID;
            }

            if (OpenParms.ViewRID <= 0)
            {
                OpenParms.ViewRID = Include.DefaultPlanViewRID;
                OpenParms.ViewUserID = Include.UndefinedUserRID;
            }

            OpenParms.ChainHLPlanProfile.NodeProfile = GetNodeProfile(openParams.NodeRID);
            if (openParams.VersionKey != Include.Undefined)
            {
                OpenParms.ChainHLPlanProfile.VersionProfile = GetVersionProfile(openParams.VersionKey, ePlanType.Chain, ePlanBasisType.Plan);
            }
            else
            {
                OpenParms.ChainHLPlanProfile.VersionProfile = GetVersionProfile(openParams.sVersion, ePlanType.Chain, ePlanBasisType.Plan);
            }
            OpenParms.StoreHLPlanProfile.NodeProfile = GetNodeProfile(openParams.StoreNodeRID);
            if (openParams.StoreVersionKey != Include.Undefined)
            {
                OpenParms.StoreHLPlanProfile.VersionProfile = GetVersionProfile(openParams.StoreVersionKey, ePlanType.Store, ePlanBasisType.Plan);
            }
            else
            {
                OpenParms.StoreHLPlanProfile.VersionProfile = GetVersionProfile(openParams.StoreFVRID, ePlanType.Store, ePlanBasisType.Plan);
            }
            OpenParms.DateRangeProfile = GetDateRangeProfile(openParams.cdrRID);
            OpenParms.FunctionSecurityProfile = GetFunctionSecurity(ePlanType.Store, false);
            OpenParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = GetNodeSecurity(ePlanType.Store, OpenParms.StoreHLPlanProfile.NodeProfile.Key);
            OpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = GetNodeSecurity(ePlanType.Chain, OpenParms.ChainHLPlanProfile.NodeProfile.Key);

            if (openParams.eStorePlanSelectedGroupBy == eStorePlanSelectedGroupBy.ByVariable)
            {
                OpenParms.GroupBy = openParams.eStorePlanSelectedGroupBy;
            }
            else
            {
                //default from OTSPlanSelection.cs
                OpenParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
            }

            if (openParams.filterRID > Include.Undefined)
            {
                OpenParms.FilterRID = openParams.filterRID;
            }
            else
            {
                OpenParms.FilterRID = Include.Undefined;
            }

            // find the Low Level Version
            ProfileList _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
            if (openParams.LowLevelVersionKey == Include.Undefined)
            {
                // same defaults from OTSPlanSelection.cs
                OpenParms.LowLevelsType = eLowLevelsType.None;
                OpenParms.LowLevelsOffset = 0;
                OpenParms.LowLevelsSequence = 0;
            }
            else
            {
                if (openParams.eLowLevelsType == eLowLevelsType.LevelOffset || openParams.eLowLevelsType == eLowLevelsType.HierarchyLevel)
                {
                    OpenParms.LowLevelsType = openParams.eLowLevelsType;
                }
                else
                {
                    OpenParms.LowLevelsType = eLowLevelsType.None;
                }
                if (openParams.lowLevelsOffset > 0)
                {
                    OpenParms.LowLevelsOffset = openParams.lowLevelsOffset;
                }
                else
                {
                    OpenParms.LowLevelsOffset = 0;
                }
                if (openParams.lowLevelsSequence > 0)
                {
                    OpenParms.LowLevelsSequence = openParams.lowLevelsSequence;
                }
                else
                {
                    OpenParms.LowLevelsSequence = 0;
                }
            }

            if (openParams.ollRID > Include.Undefined)
            {
                OpenParms.OverrideLowLevelRid = openParams.ollRID;
            }
            else
            {
                OpenParms.OverrideLowLevelRid = Include.Undefined;
            }

            if (openParams.customOLLRID > Include.Undefined)
            {
                OpenParms.CustomOverrideLowLevelRid = openParams.customOLLRID;
            }
            else
            {
                OpenParms.CustomOverrideLowLevelRid = Include.Undefined;
            }

            if (openParams.StoreGroupRID > Include.Undefined)
            {
                OpenParms.StoreGroupRID = openParams.StoreGroupRID;
            }
            else
            {
                OpenParms.StoreGroupRID = Include.Undefined;
            }

            OpenParms.StoreId = null;
            OpenParms.StoreIdNm = null;
            if (openParams.storeKey > Include.Undefined)
            {
                StoreProfile sp = StoreMgmt.StoreProfile_Get(openParams.storeKey); // _SAB.StoreServerSession.GetStoreProfile(rsRID);   (StoreProfile)            {
                if (sp.Key > Include.Undefined)
                {
                    OpenParms.StoreId = sp.StoreId;
                    OpenParms.StoreIdNm = sp.StoreName;
                }
            }
            else
            {
                OpenParms.StoreId = openParams.storeRID;
            }

            if (openParams.filterRID > Include.Undefined)
            {
                OpenParms.FilterRID = openParams.filterRID;
            }
            else
            {
                OpenParms.FilterRID = Include.Undefined;
            }

            //defaults for multi level chain function from OTSPlanSelection
            OpenParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
            //OpenParms.IsLadder = true;
            OpenParms.IsMulti = false;
            //OpenParms.IsTotRT = false;
            OpenParms.SimilarStores = OpenParms.SimilarStores;
            OpenParms.IneligibleStores = OpenParms.IneligibleStores;

            // don't call the add basis to open if basisprofiles is not defined
            if (openParams.showBasis && openParams.BasisProfiles != null)
            {
                AddBasisToOpenParms(openParams.BasisProfiles);
            }

            planManager = new ROPlanStoreSingleLevelManager(SAB, OpenParms);

            //Code implementation for Jira RO-1380
            SaveOTSForecast(openParams);
            //

            try
            {                                             
                planManager.InitializeData();
            }
            catch (PlanInUseException ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error during OpenCubeGroup..." + ex.ToString() + ex.StackTrace);
                return new RONoDataOut(eROReturnCode.Failure, ex.InUseMessage, ROInstanceID);
            }

            return new ROLongOut(eROReturnCode.Successful, null, ROInstanceID, ROInstanceID);
        }


        private DataSet BuildBasisDataSet(bool useYearBasisDateRange)
        {
            string basisDateRangeName = useYearBasisDateRange ? "basis date range_year" : "basis date range";
            DateRangeProfile basisDateRangeProfile = getDateRangeProfileByName(basisDateRangeName);
            VersionProfile basisVersion = GetVersionProfile("Actual", ePlanType.Chain, ePlanBasisType.Plan);
            DataSet dsBasis = new DataSet();
            DataTable dtBasis = new DataTable("Basis");

            if (basisDateRangeProfile == null)
            {
                string msg = string.Format("Could not find date range profile '{0}'", basisDateRangeName);

                throw new Exception(msg);
            }
            dtBasis.Columns.Add("BasisID", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("BasisName", System.Type.GetType("System.String"));

            DataTable dtBasisDetail = new DataTable("BasisDetails");
            dtBasisDetail.Columns.Add("BasisID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("Merchandise", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("MerchandiseID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("Version", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("VersionID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("DateRange", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("DateRangeID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("Picture", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("Weight", System.Type.GetType("System.Decimal"));
            dtBasisDetail.Columns.Add("IsIncluded", System.Type.GetType("System.Boolean"));
            dtBasisDetail.Columns.Add("IncludeButton", System.Type.GetType("System.String"));

            DataRow basisRow = dtBasis.NewRow();
            basisRow["BasisID"] = 0;
            basisRow["BasisName"] = "Basis 1";

            DataRow basisDetailRow = dtBasisDetail.NewRow();
            basisDetailRow["BasisID"] = basisRow["BasisID"];
            basisDetailRow["IsIncluded"] = true;
            basisDetailRow["Merchandise"] = OpenParms.ChainHLPlanProfile.NodeProfile.Text;
            basisDetailRow["MerchandiseID"] = OpenParms.ChainHLPlanProfile.NodeProfile.Key;
            basisDetailRow["Version"] = basisVersion.Description;
            basisDetailRow["VersionID"] = basisVersion.Key;
            basisDetailRow["DateRange"] = basisDateRangeProfile.DisplayDate;
            basisDetailRow["DateRangeID"] = basisDateRangeProfile.Key;

            dtBasis.Rows.Add(basisRow);
            dtBasisDetail.Rows.Add(basisDetailRow);

            dsBasis.Tables.Add(dtBasis);
            dsBasis.Tables.Add(dtBasisDetail);
            return dsBasis;
        }

        //RO-1999 Sotre Single Level Apply Changes
        override protected int GetExtraColumnCount()
        {
            //return ((ROPlanStoreSingleLevelManager)planManager).StoreSingleLevelViewData is Logility.ROUI.ROStoreSingleLevelLadderViewData ? iAddedColumnCount : 3;
            return iAddedColumnCount;
        }


        /// <summary>
        /// Update what time periods to get data for
        /// </summary>
        /// <param name="periodChangeParams">The data to use in determining what tables to return</param>
        /// <returns>The updated meta-data for the DataSet</returns>
        override protected ROCubeMetadata HandlePeriodChange(ROCubePeriodChangeParams periodChangeParams)
        {
            ((ROPlanChainSingleLevelManager)planManager).PeriodChanged(periodChangeParams.bShowYears, periodChangeParams.bShowSeasons, periodChangeParams.bShowQuarters,
                                        periodChangeParams.bShowMonths, periodChangeParams.bShowWeeks);
            planManager.ReconstructPage();

            return GetROCubeMetadata(periodChangeParams);
        }

        /// <summary>
        /// get cube metadata
        /// </summary>
        /// <param name="ROCubeGetMetadataParams">
        /// <returns>The updated meta-data for the cube</returns>
        override protected ROCubeMetadata GetROCubeMetadata(ROCubeGetMetadataParams metadataParams)
        {
            return planManager.CunstructMetadata(metadataParams);
        }
    }
}
