using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business.Allocation;

namespace Logility.ROWeb
{ 
    public partial class ROPlanning : ROWebFunction
    {
        private ApplicationSessionTransaction _applicationSessionTransaction;
        private MIDRetail.Data.OTSPlanSelection _planSelectDL = new MIDRetail.Data.OTSPlanSelection();
        private ProfileList _chainPlanVersionProfList;
        private ProfileList _storePlanVersionProfList;
        private ProfileList _versionProfList;
        private int _passedNodeID;
        private DataSet _dsBasis;
        private ROIListOut GetViews(RONoParms parms)
        {
            PlanViewData planViewData;
            planViewData = new PlanViewData();

            FunctionSecurityProfile viewUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
            FunctionSecurityProfile viewGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);

            ArrayList userRIDList = new ArrayList();

            userRIDList.Add(-1);

            if (viewUserSecurity.AllowView)
            {
                userRIDList.Add(SAB.ClientServerSession.UserRID);
            }

            if (viewGlobalSecurity.AllowView)
            {
                userRIDList.Add(Include.GlobalUserRID);
            }

            DataTable dtView;
            dtView = planViewData.PlanView_Read(userRIDList);
            dtView.Columns.Add(new DataColumn("DISPLAY_ID", typeof(string)));
            DataView dv = new DataView(dtView);
            dv.Sort = "VIEW_ID";

            List<ROPlanningView> viewsList = new List<ROPlanningView>();

            foreach (DataRowView rowView in dv)
            {
                int viewRID = Convert.ToInt32(rowView.Row["VIEW_RID"]);
                int userRID = Convert.ToInt32(rowView.Row["USER_RID"]);
                string viewID = Convert.ToString(rowView.Row["VIEW_ID"]);
                int groupByType = Convert.ToInt32(rowView.Row["GROUPBY_TYPE"]);
                int itemType= Convert.ToInt32(rowView.Row["ITEM_TYPE"]);
                int ownerUserRID = Convert.ToInt32(rowView.Row["OWNER_USER_RID"]);
                if (userRID != Include.GlobalUserRID)
                {
                    rowView["DISPLAY_ID"] = Convert.ToString(rowView["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + UserNameStorage.GetUserName(userRID) + ")";
                }
                else
                {
                    rowView["DISPLAY_ID"] = Convert.ToString(rowView["VIEW_ID"], CultureInfo.CurrentUICulture);
                }
                string displayID = Convert.ToString(rowView.Row["DISPLAY_ID"]);
                viewsList.Add(new ROPlanningView(viewRID, userRID, viewID, groupByType, itemType, ownerUserRID, displayID));
            }

            ROIListOut ROViewListOut = new ROIListOut(eROReturnCode.Successful, null, parms.ROInstanceID, viewsList);
            return ROViewListOut;
        }

        #region Retrieval of OTSPlanSelection

        internal ROOut GetOTSPlanSelectionData()
        {
            try
            {
                GetDataSource();
                var result = BuildROPlanningReview(BuildPlanOpenParams());

                return new ROPlanningReviewSelectionPropertiesOut(eROReturnCode.Successful, null, ROInstanceID, result);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetOTSPlanSelectionData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw ex;
            }
        }

        private PlanOpenParms BuildPlanOpenParams()
        {
            PlanOpenParms openParms = null;
            try
            {
                DataTable aDTSelection = _planSelectDL.GetPlanSelectionMainInfo(SAB.ClientServerSession.UserRID);
                _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();

                if (aDTSelection.Rows.Count == 0)
                {
                    openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, SAB.ApplicationServerSession.GetDefaultComputations());
                    openParms.StoreGroupRID = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
                    openParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
                    openParms.ViewRID = Include.DefaultPlanViewRID;
                    openParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
                    openParms.IneligibleStores = false;
                    openParms.SimilarStores = true;
                    openParms.LowLevelsType = eLowLevelsType.None;

                    _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                    _chainPlanVersionProfList = SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);
                    _storePlanVersionProfList = SAB.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);

                    if (_chainPlanVersionProfList.Count > 0)
                    {
                        openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_chainPlanVersionProfList[0];
                    }
                    else
                    {
                        openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                    }

                    if (_storePlanVersionProfList.Count > 0)
                    {
                        openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_storePlanVersionProfList[0];
                    }
                    else
                    {
                        openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                    }

                    if (_versionProfList.Count > 0)
                    {
                        openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[0];
                    }
                    else
                    {
                        openParms.LowLevelVersionDefault = new VersionProfile(Include.NoRID);
                    }

                    if (_passedNodeID > 0)
                    {
                        openParms.StoreHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
                        openParms.ChainHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
                    }
                    else
                    {
                        openParms.StoreHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                        openParms.ChainHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                    }

                    openParms.OverrideLowLevelRid = Include.NoRID;
                    openParms.CustomOverrideLowLevelRid = Include.NoRID;
                    openParms.IsLadder = false;
                    openParms.IsMulti = false;
                    openParms.IsTotRT = false;
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
                    if (aDTSelection.Rows[0]["SESSION_TYPE"] == System.DBNull.Value ||
                        !SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled)
                    {
                        openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, computationMode);
                    }
                    else
                    {
                        openParms = new PlanOpenParms((ePlanSessionType)Convert.ToInt32(aDTSelection.Rows[0]["SESSION_TYPE"], CultureInfo.CurrentUICulture), computationMode);
                    }
                    if (aDTSelection.Rows[0]["SG_RID"] == System.DBNull.Value)
                    {
                        openParms.StoreGroupRID = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
                    }
                    else
                    {
                        openParms.StoreGroupRID = Convert.ToInt32(aDTSelection.Rows[0]["SG_RID"], CultureInfo.CurrentUICulture);
                    }
                    openParms.FilterRID = Convert.ToInt32(aDTSelection.Rows[0]["FILTER_RID"], CultureInfo.CurrentUICulture);
                    openParms.GroupBy = (eStorePlanSelectedGroupBy)TypeDescriptor.GetConverter(openParms.GroupBy).ConvertFrom(aDTSelection.Rows[0]["GROUP_BY_ID"].ToString());
                    if (aDTSelection.Rows[0]["VIEW_RID"] == System.DBNull.Value)
                    {
                        openParms.ViewRID = Include.DefaultPlanViewRID;
                    }
                    else
                    {
                        openParms.ViewRID = Convert.ToInt32(aDTSelection.Rows[0]["VIEW_RID"], CultureInfo.CurrentUICulture);
                    }
                    if (_passedNodeID > 0)
                    {
                        openParms.StoreHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
                    }
                    else
                    {
                        if (aDTSelection.Rows[0]["STORE_HN_RID"] == System.DBNull.Value)
                        {
                            openParms.StoreHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                        }
                        else
                        {
                            openParms.StoreHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["STORE_HN_RID"], CultureInfo.CurrentUICulture), true, true);
                        }
                    }
                    if (aDTSelection.Rows[0]["STORE_FV_RID"] == System.DBNull.Value)
                    {
                        if (_storePlanVersionProfList.Count > 0)
                        {
                            openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_storePlanVersionProfList[0];
                        }
                        else
                        {
                            openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                        }
                    }
                    else
                    {
                        openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["STORE_FV_RID"], CultureInfo.CurrentUICulture));
                    }
                    if (aDTSelection.Rows[0]["TIME_PERIOD_CDR_RID"] == System.DBNull.Value)
                    {
                        openParms.DateRangeProfile = null;
                    }
                    else
                    {
                        openParms.DateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(aDTSelection.Rows[0]["TIME_PERIOD_CDR_RID"], CultureInfo.CurrentUICulture));
                    }
                    openParms.DisplayTimeBy = (eDisplayTimeBy)TypeDescriptor.GetConverter(openParms.DisplayTimeBy).ConvertFrom(aDTSelection.Rows[0]["DISPLAY_TIME_BY_ID"].ToString());
                    if (_passedNodeID > 0)
                    {
                        openParms.ChainHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
                    }
                    else
                    {
                        if (aDTSelection.Rows[0]["CHAIN_HN_RID"] == System.DBNull.Value)
                        {
                            openParms.ChainHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
                        }
                        else
                        {
                            openParms.ChainHLPlanProfile.NodeProfile = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_HN_RID"], CultureInfo.CurrentUICulture), true, true);
                        }
                    }
                    if (aDTSelection.Rows[0]["CHAIN_FV_RID"] == System.DBNull.Value)
                    {
                        if (_chainPlanVersionProfList.Count > 0)
                        {
                            openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_chainPlanVersionProfList[0];
                        }
                        else
                        {
                            openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
                        }
                    }
                    else
                    {
                        openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_FV_RID"], CultureInfo.CurrentUICulture));
                    }
                    openParms.IneligibleStores = Include.ConvertCharToBool(Convert.ToChar(aDTSelection.Rows[0]["INCLUDE_INELIGIBLE_STORES_IND"], CultureInfo.CurrentUICulture));
                    openParms.SimilarStores = Include.ConvertCharToBool(Convert.ToChar(aDTSelection.Rows[0]["INCLUDE_SIMILAR_STORES_IND"], CultureInfo.CurrentUICulture));

                    if (aDTSelection.Rows[0]["LOW_LEVEL_FV_RID"] == System.DBNull.Value)
                    {
                        if (_versionProfList.Count > 0)
                        {
                            openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[0];
                        }
                        else
                        {
                            openParms.LowLevelVersionDefault = new VersionProfile(Include.NoRID);
                        }
                    }
                    else
                    {
                        openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_FV_RID"], CultureInfo.CurrentUICulture));
                    }
                    if (aDTSelection.Rows[0]["LOW_LEVEL_TYPE"] == System.DBNull.Value)
                    {
                        openParms.LowLevelsType = eLowLevelsType.None;
                    }
                    else
                    {
                        openParms.LowLevelsType = (eLowLevelsType)Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["LOW_LEVEL_OFFSET"] != System.DBNull.Value)
                    {
                        openParms.LowLevelsOffset = Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["LOW_LEVEL_SEQUENCE"] != System.DBNull.Value)
                    {
                        openParms.LowLevelsSequence = Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_SEQUENCE"], CultureInfo.CurrentUICulture);
                    }

                    if (aDTSelection.Rows[0]["OLL_RID"] == System.DBNull.Value)
                    {
                        openParms.OverrideLowLevelRid = Include.NoRID;
                        openParms.CustomOverrideLowLevelRid = Include.NoRID;
                    }
                    else
                    {
                        openParms.OverrideLowLevelRid = Convert.ToInt32(aDTSelection.Rows[0]["OLL_RID"], CultureInfo.CurrentUICulture);
                    }
                    if (aDTSelection.Rows[0]["CUSTOM_OLL_RID"] == System.DBNull.Value)
                    {
                        openParms.CustomOverrideLowLevelRid = Include.NoRID;
                    }
                    else
                    {
                        openParms.CustomOverrideLowLevelRid = Convert.ToInt32(aDTSelection.Rows[0]["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
                    }

                    if (aDTSelection.Rows[0]["IS_LADDER"] == System.DBNull.Value)
                    {
                        openParms.IsLadder = false;
                    }
                    else
                    {
                        openParms.IsLadder = (Convert.ToInt32(aDTSelection.Rows[0]["IS_LADDER"], CultureInfo.CurrentUICulture) == 0);
                    }
                    if (aDTSelection.Rows[0]["IS_MULTI"] == System.DBNull.Value)
                    {
                        openParms.IsMulti = false;
                    }
                    else
                    {
                        openParms.IsMulti = (Convert.ToInt32(aDTSelection.Rows[0]["IS_MULTI"], CultureInfo.CurrentUICulture) == 0);
                    }

                    if (aDTSelection.Rows[0]["TOT_RIGHT"] == System.DBNull.Value)
                    {
                        openParms.IsTotRT = false;
                    }
                    else
                    {
                        openParms.IsTotRT = (Convert.ToInt32(aDTSelection.Rows[0]["TOT_RIGHT"], CultureInfo.CurrentUICulture) == 0);
                    }
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "BuildPlanOpenParams failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw ex;
            }

            return openParms;
        }

        internal ROPlanningReviewSelectionProperties BuildROPlanningReview(PlanOpenParms planOpenParms)
        {
            ROPlanningReviewSelectionProperties rOPlanningReviewSelectionProperties = new ROPlanningReviewSelectionProperties();
            rOPlanningReviewSelectionProperties.StoreAttribute = GetName.GetAttributeName(planOpenParms.StoreGroupRID);

            rOPlanningReviewSelectionProperties.Filter = GetName.GetFilterName(planOpenParms.FilterRID);
            rOPlanningReviewSelectionProperties.GroupBy = planOpenParms.GroupBy;
            rOPlanningReviewSelectionProperties.View = GetName.GetViewName(planOpenParms.ViewRID);
            rOPlanningReviewSelectionProperties.DisplayTimeBy = planOpenParms.DisplayTimeBy;
            rOPlanningReviewSelectionProperties.InEligibleStores = planOpenParms.IneligibleStores;
            rOPlanningReviewSelectionProperties.SimilarStores = planOpenParms.SimilarStores;
            rOPlanningReviewSelectionProperties.LowLevelsType = planOpenParms.LowLevelsType;
            rOPlanningReviewSelectionProperties.LowLevelsOffset = planOpenParms.LowLevelsOffset;
            rOPlanningReviewSelectionProperties.LowLevelsSequence = planOpenParms.LowLevelsSequence;

            if (rOPlanningReviewSelectionProperties.LowLevelsType == eLowLevelsType.LevelOffset)
            {
                rOPlanningReviewSelectionProperties.LowLevelsValue = "+" + rOPlanningReviewSelectionProperties.LowLevelsSequence.ToString();
            }
            else if (rOPlanningReviewSelectionProperties.LowLevelsType == eLowLevelsType.HierarchyLevel)
            {
                HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();
                if (rOPlanningReviewSelectionProperties.LowLevelsSequence == 0)
                {
                    rOPlanningReviewSelectionProperties.LowLevelsValue = mainHierProf.HierarchyID;
                }
                else
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[rOPlanningReviewSelectionProperties.LowLevelsSequence];
                    rOPlanningReviewSelectionProperties.LowLevelsValue = hlp.LevelID;
                }
            }

            rOPlanningReviewSelectionProperties.IsLadder = planOpenParms.IsLadder;
            rOPlanningReviewSelectionProperties.IsMulti = planOpenParms.IsMulti;
            rOPlanningReviewSelectionProperties.ComputationsMode = planOpenParms.ComputationsMode;
            rOPlanningReviewSelectionProperties.StoreNode = GetName.GetMerchandiseName(planOpenParms.StoreHLPlanProfile.NodeProfile.Key, SAB);

            int planDateRangeRID = Include.UndefinedCalendarDateRange;
            rOPlanningReviewSelectionProperties.Node = GetName.GetMerchandiseName(planOpenParms.ChainHLPlanProfile.NodeProfile.Key, SAB);
            if (planOpenParms.DateRangeProfile != null)
            {
                rOPlanningReviewSelectionProperties.DateRange = GetName.GetCalendarDateRange(planOpenParms.DateRangeProfile.Key, SAB);
                planDateRangeRID = planOpenParms.DateRangeProfile.Key;
            }

            rOPlanningReviewSelectionProperties.Version = GetName.GetVersion(planOpenParms.ChainHLPlanProfile.VersionProfile.Key, SAB);
            rOPlanningReviewSelectionProperties.StoreVersion = GetName.GetVersion(planOpenParms.StoreHLPlanProfile.VersionProfile.Key, SAB);
            rOPlanningReviewSelectionProperties.LowLevelVersion = GetName.GetVersion(planOpenParms.LowLevelVersionDefault.Key, SAB);
            rOPlanningReviewSelectionProperties.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(planOpenParms.OverrideLowLevelRid, SAB);
            rOPlanningReviewSelectionProperties.PlanSessionType = planOpenParms.PlanSessionType;

            rOPlanningReviewSelectionProperties.ROBasisProfiles = ConvertBasisDataToList(_dsBasis, planDateRangeRID);

            return rOPlanningReviewSelectionProperties;
        }        

        private List<ROBasisProfile> ConvertBasisDataToList(DataSet dsBasis, int planDateRangeRID)
        {
            List<ROBasisProfile> basisProfiles = new List<ROBasisProfile>();
            DataTable dtBasis = dsBasis.Tables[0];
            DataTable dtBasisDetails = dsBasis.Tables[1];
            KeyValuePair<int, string> workKVP;
            int basisDtlCtr = 0;

            for (int basisCtr = 0; basisCtr < dtBasis.Rows.Count; basisCtr++)
            {
                int intBasisId = Convert.ToInt32(dtBasis.Rows[basisCtr]["BasisID"].ToString());
                string strBasisName = dtBasis.Rows[basisCtr]["BasisName"].ToString();
                ROBasisProfile basisProfile = new ROBasisProfile(intBasisId, strBasisName);

                List<ROBasisDetailProfile> basisDetailProfiles = new List<ROBasisDetailProfile>();

                for (int basisDtlRowCtr = 0; basisDtlRowCtr < dtBasisDetails.Select("BasisID=" + intBasisId).Length; basisDtlRowCtr++)
                {
                    int iBasisId = Convert.ToInt32(dtBasis.Rows[basisCtr]["BasisID"].ToString());
                   
                    int iMerchandiseId = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["MerchandiseID"].ToString());
                    workKVP = GetName.GetMerchandiseName(iMerchandiseId, SAB);
                    string sMerchandise = workKVP.Value;                    
                    int iVersionId = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["VersionID"].ToString());
                    workKVP = GetName.GetVersion(iVersionId, SAB);
                    string sVersion = workKVP.Value;                    
                    int iDateRangeID = Convert.ToInt32(dtBasisDetails.Rows[basisDtlCtr]["DateRangeID"].ToString());
                    if (planDateRangeRID != Include.UndefinedCalendarDateRange)
                    {
                        workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, planDateRangeRID);
                    }
                    else
                    {
                        workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, SAB.ClientServerSession.Calendar.CurrentDate);
                    }
                    string sDateRange = workKVP.Value;
                    string sPicture = dtBasisDetails.Rows[basisDtlCtr]["Picture"].ToString();
                    float fWeight = float.Parse(dtBasisDetails.Rows[basisDtlCtr]["Weight"] == DBNull.Value ? "0" : dtBasisDetails.Rows[basisDtlCtr]["Weight"].ToString());
                    int iIsIncluded = Convert.ToInt16(dtBasisDetails.Rows[basisDtlCtr]["IsIncluded"].ToString());
                    bool bIsIncluded = false;
                    if (iIsIncluded == 1) bIsIncluded = true;

                    string sIncludeButton = dtBasisDetails.Rows[basisDtlCtr]["IncludeButton"].ToString();
                    ROBasisDetailProfile basisDetailProfile = new ROBasisDetailProfile(iBasisId, iMerchandiseId, sMerchandise, iVersionId, sVersion,
                        iDateRangeID, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton);
                    basisDetailProfiles.Add(basisDetailProfile);
                    ++basisDtlCtr;
                }
                basisProfile.BasisDetailProfiles = basisDetailProfiles;
                basisProfiles.Add(basisProfile);
            }

            return basisProfiles;
        }

        private void GetDataSource()
        {
            DataRelation drBasis;
            try
            {
                _dsBasis = _planSelectDL.GetPlanSelectionBasis(SAB.ClientServerSession.UserRID);
                drBasis = new DataRelation("Basis Details", _dsBasis.Tables["Basis"].Columns["BasisID"], _dsBasis.Tables["BasisDetails"].Columns["BasisID"]);
                _dsBasis.Relations.Add(drBasis);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Application Session Transaction
        internal ApplicationSessionTransaction GetApplicationSessionTransaction(bool getNewTransaction = false)
        {
            if (_applicationSessionTransaction == null
                || getNewTransaction)
            {
                _applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
            }
            return _applicationSessionTransaction;
        }

        #endregion
    }

    
}
