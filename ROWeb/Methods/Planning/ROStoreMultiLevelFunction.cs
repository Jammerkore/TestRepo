using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;

using Logility.ROWebCommon;

using Logility.ROWebSharedTypes;
using Logility.ROUI;
using System.Data;

namespace Logility.ROWeb
{
    public class ROStoreMultiLevel : ROStoreFunction
    {
        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="sessionType">the type of planning session (store/chain and single/multi level)</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROStoreMultiLevel(SessionAddressBlock SAB, ROWebTools ROWebTools)
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
            int iVP;
            int lowLevelVersionKey = Include.Undefined;
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
                OpenParms.StoreHLPlanProfile.VersionProfile = GetVersionProfile(openParams.StoreVersionKey, ePlanType.Store, ePlanBasisType.Plan);
            }
            else
            {
                OpenParms.ChainHLPlanProfile.VersionProfile = GetVersionProfile(openParams.sVersion, ePlanType.Chain, ePlanBasisType.Plan);
                OpenParms.StoreHLPlanProfile.VersionProfile = GetVersionProfile(openParams.StoreVersionKey, ePlanType.Store, ePlanBasisType.Plan);
            }
            OpenParms.StoreHLPlanProfile.NodeProfile = GetNodeProfile(openParams.StoreNodeRID);
            OpenParms.DateRangeProfile = GetDateRangeProfile(openParams.cdrRID);
            OpenParms.FunctionSecurityProfile = GetFunctionSecurity(ePlanType.Store, false);
            OpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = GetNodeSecurity(ePlanType.Store, OpenParms.ChainHLPlanProfile.NodeProfile.Key);


            //OpenParms.ChainHLPlanProfile.NodeProfile = GetNodeProfile(openParams.NodeRID);
            //OpenParms.ChainHLPlanProfile.VersionProfile = GetVersionProfile(openParams.sVersion, ePlanType.Chain, ePlanBasisType.Plan);
            //OpenParms.StoreHLPlanProfile.NodeProfile = GetNodeProfile(openParams.StoreNodeRID);
            //OpenParms.StoreHLPlanProfile.VersionProfile = GetVersionProfile(openParams.StoreFVRID, ePlanType.Store, ePlanBasisType.Plan);
            //OpenParms.DateRangeProfile = GetDateRangeProfile(openParams.cdrRID);
            //OpenParms.FunctionSecurityProfile = GetFunctionSecurity(ePlanType.Store, false);
            //OpenParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = GetNodeSecurity(ePlanType.Store, OpenParms.StoreHLPlanProfile.NodeProfile.Key);
            //OpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = GetNodeSecurity(ePlanType.Chain, OpenParms.ChainHLPlanProfile.NodeProfile.Key);


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

            // find the Low Level Version Key for the Version String
            ProfileList _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
            if (openParams.LowLevelVersionKey != Include.Undefined)
            {
                for (iVP = 0; iVP < _versionProfList.Count; iVP++)
                {
                    if (((VersionProfile)_versionProfList[iVP]).Key == openParams.LowLevelVersionKey)
                    {
                        lowLevelVersionKey = iVP;
                        OpenParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[lowLevelVersionKey];
                    }
                }
            }
            else
            {
                for (iVP = 0; iVP < _versionProfList.Count; iVP++)
                {
                    if (((VersionProfile)_versionProfList[iVP]).Description == openParams.sLowLevelVersion)
                    {
                        lowLevelVersionKey = iVP;
                        OpenParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[lowLevelVersionKey];
                    }
                }
            }
            if (lowLevelVersionKey == Include.Undefined)
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

            //defaults for multi level chain function from OTSPlanSelection
            OpenParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
            OpenParms.IsLadder = false;
            OpenParms.IsMulti = true;
            //OpenParms.IsTotRT = true;
            OpenParms.SimilarStores = true;
            OpenParms.IneligibleStores = false;

            // don't call the add basis to open if basisprofiles is not defined
            if (openParams.BasisProfiles != null)
            {
                AddBasisToOpenParms(openParams.BasisProfiles);
            }

            planManager = new ROPlanStoreMultiLevelManager(SAB, OpenParms);

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


        //RO-1170 Replace the GetExtraColumnCount with an Abstract Class
        override protected int GetExtraColumnCount()
        {
            return iAddedColumnCount;
        }

        /// <summary>
        /// Save the changes to the database
        /// </summary>
        override public ROOut SaveCubeGroup()
        {
            RecomputeCubes();

            PlanSaveParms planSaveParms = new PlanSaveParms();

            // Save everything for now until save options can be developed.

            planSaveParms.SaveChainHighLevel = true;
            if (planSaveParms.SaveChainHighLevel)
            {
                planSaveParms.ChainHighLevelNodeRID = planManager.OpenParms.ChainHLPlanProfile.NodeProfile.Key;
                planSaveParms.ChainHighLevelVersionRID = planManager.OpenParms.ChainHLPlanProfile.VersionProfile.Key;
                planSaveParms.ChainHighLevelDateRangeRID = planManager.OpenParms.DateRangeProfile.Key;
                planSaveParms.SaveHighLevelAllStoreAsChain = false;
            }

            planSaveParms.SaveChainLowLevel = true;
            if (planSaveParms.SaveChainLowLevel)
            {
                planSaveParms.SaveLowLevelAllStoreAsChain = false;
                //if (chkChainLowLevelOverride.Checked)
                //{
                //    planSaveParms.OverrideChainLowLevel = true;
                //    planSaveParms.ChainLowLevelVersionRID = Convert.ToInt32(cboChainLowLevelVers.SelectedValue, CultureInfo.CurrentUICulture);
                //    planSaveParms.ChainLowLevelDateRangeRID = drsChainLowLevelTime.DateRangeRID;
                //}
            }

            planSaveParms.SaveStoreHighLevel = true;
            if (planSaveParms.SaveStoreHighLevel)
            {
                planSaveParms.StoreHighLevelNodeRID = planManager.OpenParms.StoreHLPlanProfile.NodeProfile.Key;
                planSaveParms.StoreHighLevelVersionRID = planManager.OpenParms.StoreHLPlanProfile.VersionProfile.Key;
                planSaveParms.StoreHighLevelDateRangeRID = planManager.OpenParms.DateRangeProfile.Key;
            }

            planSaveParms.SaveStoreLowLevel = true;
            if (planSaveParms.SaveStoreLowLevel)
            {
                //if (chkStoreLowLevelOverride.Checked)
                //{
                //    planSaveParms.OverrideStoreLowLevel = true;
                //    planSaveParms.StoreLowLevelVersionRID = Convert.ToInt32(cboStoreLowLevelVers.SelectedValue, CultureInfo.CurrentUICulture);
                //    planSaveParms.StoreLowLevelDateRangeRID = drsStoreLowLevelTime.DateRangeRID;
                //}
            }

            planManager.CubeGroup.SaveCubeGroup(planSaveParms);
            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
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
