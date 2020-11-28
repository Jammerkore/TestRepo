using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class HierarchyPropertiesProfile : HierarchyPropertiesBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public HierarchyPropertiesProfile(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base(SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.Hierarchy)
        {
            
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            return true;
        }


        override public ROHierarchyPropertiesProfile HierarchyPropertiesGetData(ROHierarchyPropertyKeyParms parms, object HierarchyPropertiesData, ref string message, bool applyOnly = false)
        {
            if (_hierarchyProfile == null)
            {
                _hierarchyProfile = (HierarchyProfile)HierarchyPropertiesData;
            }

            KeyValuePair<int, string> hierarchy = new KeyValuePair<int, string>(key: _hierarchyProfile.Key, value: _hierarchyProfile.HierarchyID);
            ROHierarchyPropertiesProfile hierarchyProperties = new ROHierarchyPropertiesProfile(hierarchy: hierarchy);

            // populate modelProperties using Windows\HierarchyProperties.cs as a reference

            AddProfileValues(hierarchyProperties: hierarchyProperties);

            return hierarchyProperties;
        }

        private void AddProfileValues(ROHierarchyPropertiesProfile hierarchyProperties)
        {
            int levelIndex = 0;

            hierarchyProperties.HierarchyRootNodeRID = _hierarchyProfile.HierarchyRootNodeRID;
            hierarchyProperties.HierarchyType = _hierarchyProfile.HierarchyType;
            hierarchyProperties.OwnerKey = _hierarchyProfile.Owner;
            hierarchyProperties.HierarchyColor = _hierarchyProfile.HierarchyColor;
            hierarchyProperties.HierarchyRollupOption = _hierarchyProfile.HierarchyRollupOption;
            hierarchyProperties.PlanLevelType = _hierarchyProfile.OTSPlanLevelType;

            if (_hierarchyProfile.HierarchyType == eHierarchyType.organizational)
            {
                if (_hierarchyProfile.PostingDate == Include.UndefinedDate)
                {
                    hierarchyProperties.PostingDate =  MIDText.GetTextOnly(eMIDTextCode.lbl_Date_Not_Set);
                }
                else
                {
                    hierarchyProperties.PostingDate = _hierarchyProfile.PostingDate.ToShortDateString();
                }
            }
            else
            {
                hierarchyProperties.PostingDate = string.Empty;
            }

            if (_hierarchyProfile.HierarchyType == eHierarchyType.organizational)
            {
                for (levelIndex = 1; levelIndex <= _hierarchyProfile.HierarchyLevels.Count; levelIndex++)
                {
                    Level_Information_Load(hierarchyProperties: hierarchyProperties, levelIndex: levelIndex);
                }
            }
        }

        public void Level_Information_Load(ROHierarchyPropertiesProfile hierarchyProperties, int levelIndex)
        {
            HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hierarchyProfile.HierarchyLevels[levelIndex];
            ROHierarchyLevel level = new ROHierarchyLevel(new KeyValuePair<int, string>(levelIndex, hlp.LevelID));

            level.LevelColor = hlp.LevelColor;
            level.LevelLengthType = hlp.LevelLengthType;
            level.LevelRequiredSize = hlp.LevelRequiredSize;
            level.LevelSizeRangeFrom = hlp.LevelSizeRangeFrom;
            level.LevelSizeRangeTo = hlp.LevelSizeRangeTo;
            level.LevelNodesExist = hlp.LevelNodesExist;
            level.LevelType = hlp.LevelType;
            level.LevelOTSPlanLevelType = hlp.LevelOTSPlanLevelType;
            level.LevelDisplayOption = hlp.LevelDisplayOption;
            level.LevelIDFormat = hlp.LevelIDFormat;
            if (hlp.PurgeDailyHistory >= 0)
            {
                level.PurgeDailyHistory = hlp.PurgeDailyHistory;
            }
            if (hlp.PurgeWeeklyHistory >= 0)
            {
                level.PurgeWeeklyHistory = hlp.PurgeWeeklyHistory;
            }
            if (hlp.PurgePlans >= 0)
            {
                level.PurgePlans = hlp.PurgePlans;
            }
            if (hlp.PurgeHtASN >= 0)
            {
                level.PurgeHeaderASN = hlp.PurgeHtASN;
            }
            if (hlp.PurgeHtDropShip >= 0)
            {
                level.PurgeHeaderDropShip = hlp.PurgeHtDropShip;
            }
            if (hlp.PurgeHtDummy >= 0)
            {
                level.PurgeHeaderDummy = hlp.PurgeHtDummy;
            }
            if (hlp.PurgeHtPurchaseOrder >= 0)
            {
                level.PurgeHeaderPurchaseOrder = hlp.PurgeHtPurchaseOrder;
            }
            if (hlp.PurgeHtReceipt >= 0)
            {
                level.PurgeHeaderReceipt = hlp.PurgeHtReceipt;
            }
            if (hlp.PurgeHtReserve >= 0)
            {
                level.PurgeHeaderReserve = hlp.PurgeHtReserve;
            }
            if (hlp.PurgeHtVSW >= 0)
            {
                level.PurgeHeaderVSW = hlp.PurgeHtVSW;
            }
            if (hlp.PurgeHtWorkUpTot >= 0)
            {
                level.PurgeHeaderWorkupTotalBuy = hlp.PurgeHtWorkUpTot;
            }

            hierarchyProperties.LevelList.Add(level);

        }

        override public object HierarchyPropertiesUpdateData(ROHierarchyPropertiesProfile HierarchyPropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            ROHierarchyPropertiesProfile HierarchyPropertiesProfileData = (ROHierarchyPropertiesProfile)HierarchyPropertiesData;

            if (SetProfile(HierarchyPropertiesData: HierarchyPropertiesProfileData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateProfile(message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }


            return _hierarchyProfile;
        }

        /// <summary>
        /// Takes values from input class and updates the node profile memory object
        /// </summary>
        /// <param name="HierarchyPropertiesData">Input values for the node profile</param>
        /// <param name="message">The message</param>
        private bool SetProfile(ROHierarchyPropertiesProfile HierarchyPropertiesData, ref string message)
        {
            if (HierarchyIDIsValid(HierarchyPropertiesData: HierarchyPropertiesData, message: ref message))
            {
                if (_hierarchyProfile.Key > Include.NoRID)
                {
                    _hierarchyProfile.HierarchyChangeType = eChangeType.update;
                }
                else
                {
                    _hierarchyProfile.HierarchyChangeType = eChangeType.add;
                }

                _hierarchyProfile.HierarchyID = HierarchyPropertiesData.Hierarchy.Value;
                _hierarchyProfile.HierarchyRootNodeRID = HierarchyPropertiesData.HierarchyRootNodeRID;
                _hierarchyProfile.HierarchyType = HierarchyPropertiesData.HierarchyType;
                _hierarchyProfile.Owner = HierarchyPropertiesData.OwnerKey;
                _hierarchyProfile.HierarchyColor = HierarchyPropertiesData.HierarchyColor;
                _hierarchyProfile.HierarchyRollupOption = HierarchyPropertiesData.HierarchyRollupOption;
                _hierarchyProfile.OTSPlanLevelType = HierarchyPropertiesData.PlanLevelType;

                _hierarchyProfile.HierarchyLevels.Clear();
                if (HierarchyPropertiesData.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyProfile origHP = SAB.HierarchyServerSession.GetHierarchyData(_hierarchyProfile.Key);

                    foreach (ROHierarchyLevel level in HierarchyPropertiesData.LevelList)
                    {
                        Level_Information_Add(origHP: origHP, level: level);
                    }
                }
            }

            return true;
        }

        public void Level_Information_Add(HierarchyProfile origHP, ROHierarchyLevel level)
        {
            HierarchyLevelProfile hlp = new HierarchyLevelProfile(level.Level.Key);

            hlp.Level = level.Level.Key;
            if (origHP.HierarchyLevels.ContainsKey(hlp.Level))
            {
                hlp.LevelChangeType = eChangeType.update;
            }
            else
            {
                hlp.LevelChangeType = eChangeType.add;
            }

            hlp.LevelColor = level.LevelColor;
            hlp.LevelID = level.Level.Value;
            hlp.LevelLengthType = level.LevelLengthType;
            hlp.LevelRequiredSize = level.LevelRequiredSize;
            hlp.LevelSizeRangeFrom = level.LevelSizeRangeFrom;
            hlp.LevelSizeRangeTo = level.LevelSizeRangeTo;
            hlp.LevelType = level.LevelType;
            hlp.LevelOTSPlanLevelType = level.LevelOTSPlanLevelType;
            hlp.LevelDisplayOption = level.LevelDisplayOption;
            if (hlp.LevelType == eHierarchyLevelType.Style)  // only allow option to be set on style
            {
                hlp.LevelIDFormat = level.LevelIDFormat;
            }
            else
            {
                hlp.LevelIDFormat = eHierarchyIDFormat.Unique;
            }

            hlp.PurgeDailyHistoryTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeDailyHistoryIsSet)
            {
                hlp.PurgeDailyHistory = (int)level.PurgeDailyHistory;
            }
            hlp.PurgeWeeklyHistoryTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeWeeklyHistoryIsSet)
            {
                hlp.PurgeWeeklyHistory = (int)level.PurgeWeeklyHistory;
            }
            hlp.PurgePlansTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgePlansIsSet)
            {
                hlp.PurgePlans = (int)level.PurgePlans;
            }

            hlp.PurgeHtASNTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderASNIsSet)
            {
                hlp.PurgeHtASN = (int)level.PurgeHeaderASN;
            }
            hlp.PurgeHtDropShipTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderDropShipIsSet)
            {
                hlp.PurgeHtDropShip = (int)level.PurgeHeaderDropShip;
            }
            hlp.PurgeHtDummyTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderDummyIsSet)
            {
                hlp.PurgeHtDummy = (int)level.PurgeHeaderDummy;
            }
            hlp.PurgeHtPurchaseOrderTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderPurchaseOrderIsSet)
            {
                hlp.PurgeHtPurchaseOrder = (int)level.PurgeHeaderPurchaseOrder;
            }
            hlp.PurgeHtReceiptTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderReceiptIsSet)
            {
                hlp.PurgeHtReceipt = (int)level.PurgeHeaderReceipt;
            }
            hlp.PurgeHtReserveTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderReserveIsSet)
            {
                hlp.PurgeHtReserve = (int)level.PurgeHeaderReserve;
            }
            hlp.PurgeHtVSWTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderVSWIsSet)
            {
                hlp.PurgeHtVSW = (int)level.PurgeHeaderVSW;
            }
            hlp.PurgeHtWorkUpTotTimeframe = ePurgeTimeframe.Weeks;
            if (level.PurgeHeaderWorkupTotalBuyIsSet)
            {
                hlp.PurgeHtWorkUpTot = (int)level.PurgeHeaderWorkupTotalBuy;
            }
                                                      
            hlp.LevelNodesExist = level.LevelNodesExist; 

            _hierarchyProfile.HierarchyLevels.Add(hlp.Level, hlp);

        }

        private bool HierarchyIDIsValid(ROHierarchyPropertiesProfile HierarchyPropertiesData, ref string message)
        {
            try
            {
                if (HierarchyPropertiesData.Hierarchy.Key == Include.NoRID
                    || HierarchyPropertiesData.Hierarchy.Value != _hierarchyProfile.HierarchyID)
                {
                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(HierarchyPropertiesData.Hierarchy.Value);
                    if (hp.Key != -1)
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HierarchyAlreadyExists, true);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private bool UpdateProfile(ref string message)
        {
            bool successful = true;
            try
            {
                if (_hierarchyProfile.HierarchyChangeType == eChangeType.add
                    && _hierarchyProfile.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();
                    if (mainHierProf.Key != Include.NoRID)
                    {
                        message += SAB.ClientServerSession.Audit.GetText(messageCode: eMIDTextCode.msg_OnlyOneOrganizational, addToAuditReport: true);
                        return false;
                    }
                }

                _hierarchyProfile = SAB.HierarchyServerSession.HierarchyUpdate(_hierarchyProfile);

                if (_hierarchyProfile.HierarchyChangeType == eChangeType.add)
                {
                    if (!AddSecurityToHierarchy(false, Include.NoRID, _hierarchyProfile.HierarchyRootNodeRID, eSecurityLevel.Allow, ref message))
                    {
                        successful = false;
                    }
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
                successful = false;
            }

            return successful;
        }

        private bool AddSecurityToHierarchy(bool isForGroup, int aGroupRID, int aNodeRID, eSecurityLevel aSecurityLevel, ref string message)
        {
            SecurityAdmin security = new SecurityAdmin();
            try
            {
                security.OpenUpdateConnection();
                if (isForGroup)
                {
                    security.AssignGroupNode(aGroupRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, aSecurityLevel);
                    security.AssignGroupNode(aGroupRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, aSecurityLevel);
                    security.AssignGroupNode(aGroupRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, aSecurityLevel);
                }
                else
                {
                    security.AssignUserNode(SAB.ClientServerSession.UserRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, aSecurityLevel);
                    security.AssignUserNode(SAB.ClientServerSession.UserRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, aSecurityLevel);
                    security.AssignUserNode(SAB.ClientServerSession.UserRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, aSecurityLevel);
                }
                security.CommitData();
                SAB.HierarchyServerSession.AddToSecurityNodeList(aNodeRID);
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
            finally
            {
                security.CloseUpdateConnection();
            }

            return true;
        }

        override public bool HierarchyPropertiesDelete(int key, ref string message)
        {
            // Check in use on root node of hierarchy
            if (!ApplicationUtilities.AllowDeleteFromInUse(key: _hierarchyProfile.HierarchyRootNodeRID, profileType: eProfileType.HierarchyNode , SAB: SAB))
            {
                return false;
            }

            EditMsgs em = new EditMsgs();
            if (_hierarchyProfile.HierarchyType == eHierarchyType.organizational)
            {
                _hierarchyProfile.HierarchyChangeType = eChangeType.delete;
            }
            else
            {
                _hierarchyProfile.HierarchyChangeType = eChangeType.markedForDelete;
            }

            int nodeRID = _hierarchyProfile.Key;
            HierarchyMaintenance.ProcessHierarchyData(ref em, _hierarchyProfile);
            if (em.ErrorFound)
            {
                BuildMessage(em: em, message: ref message);
                return false;
            }
            else
            {
                SAB.HierarchyServerSession.DequeueHierarchy(_hierarchyProfile.Key);
                _hierarchyProfile.HierarchyLockStatus = eLockStatus.Undefined;
            }

            Folder_DeleteAll_Shortcut(key: nodeRID, folderType: eProfileType.HierarchyNode, message: ref message);

            return true;
        }

        override public object HierarchyPropertiesGetValues(ROHierarchyPropertyKeyParms parms)
        {
            if (_hierarchyProfile != null)
            {
                return _hierarchyProfile;
            }
            else
            {
                return SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID: parms.Key);
            }
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (key <= 0)
            {
                if (_hierarchyProfile.HierarchyType == eHierarchyType.organizational)
                {
                    _functionSecurity = new FunctionSecurityProfile(Convert.ToInt32(eSecurityFunctions.AdminHierarchiesOrg, CultureInfo.CurrentCulture));
                }
                else
                {
                    _functionSecurity = new FunctionSecurityProfile(Convert.ToInt32(eSecurityFunctions.AdminHierarchiesAlt, CultureInfo.CurrentCulture));
                }
                FunctionSecurity.SetFullControl();
            }
            else if (_hierarchyProfile.HierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgProperty, (int)eSecurityTypes.All);
            }
            else if (_hierarchyProfile.Owner != Include.GlobalUserRID)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalProperty, (int)eSecurityTypes.All);
            }
        }
        
    }
}
