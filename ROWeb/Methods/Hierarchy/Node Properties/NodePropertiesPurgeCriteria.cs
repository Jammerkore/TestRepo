using System;
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Collections.Generic;

namespace Logility.ROWeb
{
    public class NodePropertiesPurgeCriteria : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesPurgeCriteria(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.PurgeCriteria)
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


        override public RONodeProperties NodePropertiesGetData(ROProfileKeyParms parms, object nodePropertiesData, ref string message, bool applyOnly = false)
        {
            if (_hierarchyNodeProfile == null)
            {
                _hierarchyNodeProfile = (HierarchyNodeProfile)nodePropertiesData;
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesPurgeCriteria nodeProperties = new RONodePropertiesPurgeCriteria(node: node);

            // populate modelProperties using Windows\NodeProperties.cs as a reference

            AddProfileValues(nodeProperties: nodeProperties);

            return nodeProperties;
        }

        private void AddProfileValues(RONodePropertiesPurgeCriteria nodeProperties)
        {
            RONodePropertiesPurgeCriteriaSettings purgeCriteriaSettings;

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.DailyHistory, purgeLabel: "Daily History");
            if (_hierarchyNodeProfile.PurgeDailyHistoryAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeDailyHistoryAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeDailyCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeDailyCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.WeeklyHistory, purgeLabel: "Weekly History");
            if (_hierarchyNodeProfile.PurgeWeeklyHistoryAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeWeeklyHistoryAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeWeeklyCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeWeeklyCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.ForecastPlans, purgeLabel: "Channel Plans");
            if (_hierarchyNodeProfile.PurgeOTSPlansAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeOTSPlansAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeOTSCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeOTSCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderASN, purgeLabel: "Worklist Line Type: ASN");
            if (_hierarchyNodeProfile.PurgeHtASNAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtASNAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtASNCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtASNCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderDropShip, purgeLabel: "Worklist Line Type: Drop Ship");
            if (_hierarchyNodeProfile.PurgeHtDropShipAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtDropShipAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtDropShipCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtDropShipCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderDummy, purgeLabel: "Worklist Line Type: Dummy");
            if (_hierarchyNodeProfile.PurgeHtDummyAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtDummyAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtDummyCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtDummyCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderPurchaseOrder, purgeLabel: "Worklist Line Type: Purchase Order");
            if (_hierarchyNodeProfile.PurgeHtPurchaseOrderAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtPurchaseOrderAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtPurchaseOrderCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtPurchaseOrderCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderReceipt, purgeLabel: "Worklist Line Type: Receipt");
            if (_hierarchyNodeProfile.PurgeHtReceiptAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtReceiptAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtReceiptCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtReceiptCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderReserve, purgeLabel: "Worklist Line Type: Reserve");
            if (_hierarchyNodeProfile.PurgeHtReserveAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtReserveAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtReserveCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtReserveCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderVSW, purgeLabel: "Worklist Line Type: VSW");
            if (_hierarchyNodeProfile.PurgeHtVSWAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtVSWAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtVSWCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtVSWCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

            purgeCriteriaSettings = new RONodePropertiesPurgeCriteriaSettings(purgeDataType: ePurgeDataType.HeaderWorkUpBuy, purgeLabel: "Worklist Line Type: Workup Total Buy");
            if (_hierarchyNodeProfile.PurgeHtWorkUpTotAfter > Include.Undefined)
            {
                purgeCriteriaSettings.PurgeValue = _hierarchyNodeProfile.PurgeHtWorkUpTotAfter;
                SetInheritanceValues(purgeCriteriaSettings: purgeCriteriaSettings,
                    inheritedFromType: _hierarchyNodeProfile.PurgeHtWorkUpTotCriteriaInherited,
                    inheritedFrom: _hierarchyNodeProfile.PurgeHtWorkUpTotCriteriaInheritedFrom
                    );
            }
            nodeProperties.PurgeCriteriaSettings.Add(purgeCriteriaSettings);

        }

        private void SetInheritanceValues(RONodePropertiesPurgeCriteriaSettings purgeCriteriaSettings, 
            eInheritedFrom inheritedFromType, 
            int inheritedFrom            
            )
        {
            HierarchyNodeProfile hnp = null;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            switch (inheritedFromType)
            {
                case eInheritedFrom.Node:
                    hnp = GetHierarchyNodeProfile(key: inheritedFrom);
                    purgeCriteriaSettings.PurgeInheritedFromNode = new KeyValuePair<int, string>(inheritedFrom, inheritedFromText + hnp.Text);
                    break;
                case eInheritedFrom.HierarchyLevel:
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)MainHierProf.HierarchyLevels[inheritedFrom];
                    purgeCriteriaSettings.PurgeInheritedFromNode = new KeyValuePair<int, string>(inheritedFrom, inheritedFromText + hlp.LevelID);
                    break;
                default:
                    break;
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesPurgeCriteria nodePropertiesPurgeData = (RONodePropertiesPurgeCriteria)nodePropertiesData;

            if (SetProfile(nodePropertiesPurgeData: nodePropertiesPurgeData, message: ref message, applyOnly: applyOnly))
            {
                if (!applyOnly)
                {
                    UpdateProfile();
                }
            }
            else
            {
                successful = false;
            }

            return _hierarchyNodeProfile;
        }

        /// <summary>
        /// Takes values from input class and updates the node profile memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the node purge criteria</param>
        /// <param name="message">The message</param>
        private bool SetProfile(RONodePropertiesPurgeCriteria nodePropertiesPurgeData, ref string message, bool applyOnly)
        {
            foreach (RONodePropertiesPurgeCriteriaSettings purgeCriteria in nodePropertiesPurgeData.PurgeCriteriaSettings)
            {
                switch (purgeCriteria.PurgeDataType)
                {
                    case ePurgeDataType.DailyHistory:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeDailyHistoryAfter)
                            {
                                _hierarchyNodeProfile.PurgeDailyHistoryAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeDailyCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeDailyCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeDailyHistoryAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeDailyHistoryAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeDailyHistoryAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.WeeklyHistory:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeWeeklyHistoryAfter)
                            {
                                _hierarchyNodeProfile.PurgeWeeklyHistoryAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeWeeklyCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeWeeklyCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeWeeklyHistoryAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeWeeklyHistoryAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeWeeklyHistoryAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.ForecastPlans:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeOTSPlansAfter)
                            {
                                _hierarchyNodeProfile.PurgeOTSPlansAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeOTSCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeOTSCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeOTSPlansAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeOTSPlansAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeOTSPlansAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderASN:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtASNAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtASNAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtASNCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtASNCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtASNAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtASNAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtASNAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderDropShip:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtDropShipAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtDropShipAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtDropShipCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtDropShipCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtDropShipAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtDropShipAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtDropShipAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderDummy:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtDummyAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtDummyAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtDummyCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtDummyCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtDummyAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtDummyAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtDummyAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderReceipt:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtReceiptAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtReceiptAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtReceiptCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtReceiptCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtReceiptAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtReceiptAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtReceiptAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderPurchaseOrder:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtPurchaseOrderAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtPurchaseOrderAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtPurchaseOrderCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtPurchaseOrderCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtPurchaseOrderAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtPurchaseOrderAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtPurchaseOrderAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderReserve:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtReserveAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtReserveAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtReserveCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtReserveCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtReserveAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtReserveAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtReserveAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderVSW:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtVSWAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtVSWAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtVSWCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtVSWCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtVSWAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtVSWAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtVSWAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                    case ePurgeDataType.HeaderWorkUpBuy:
                        if (purgeCriteria.PurgeValueIsSet)
                        {
                            if (purgeCriteria.PurgeValue != _hierarchyNodeProfile.PurgeHtWorkUpTotAfter)
                            {
                                _hierarchyNodeProfile.PurgeHtWorkUpTotAfter = purgeCriteria.PurgeValue;
                                _hierarchyNodeProfile.PurgeHtWorkUpTotCriteriaInherited = eInheritedFrom.None;
                                _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                            }
                            else if (_hierarchyNodeProfile.PurgeHtWorkUpTotCriteriaInherited != eInheritedFrom.None
                                    && !applyOnly)
                            {
                                _hierarchyNodeProfile.PurgeHtWorkUpTotAfter = Include.Undefined;
                            }
                        }
                        else if (_hierarchyNodeProfile.PurgeHtWorkUpTotAfter != Include.Undefined)
                        {
                            _hierarchyNodeProfile.PurgeHtWorkUpTotAfter = Include.Undefined;
                            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.update;
                        }
                        break;
                }
            }

            return true;
        }

        private bool UpdateProfile()
        {
            EditMsgs em = new EditMsgs();

            if (_hierarchyNodeProfile.PurgeCriteriaChangeType == eChangeType.update)
            {
                HierarchyMaintenance.ProcessNodeProfileInfo(ref em, _hierarchyNodeProfile);
            }

            // refresh node after update to get inheritance settings.
            eLockStatus lockStatus = _hierarchyNodeProfile.NodeLockStatus;
            _hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: _hierarchyNodeProfile.Key, chaseHierarchy: true);
            _hierarchyNodeProfile.NodeLockStatus = lockStatus;
            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.none;

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.delete;
            _hierarchyNodeProfile.PurgeDailyHistoryAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeWeeklyHistoryAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeOTSPlansAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtASNAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtDropShipAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtDummyAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtReceiptAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtPurchaseOrderAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtReserveAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtVSWAfter = Include.Undefined;
            _hierarchyNodeProfile.PurgeHtWorkUpTotAfter = Include.Undefined;

            EditMsgs em = new EditMsgs();

            HierarchyMaintenance.ProcessNodeProfileInfo(ref em, _hierarchyNodeProfile);

            // refresh node after update to get inheritance settings.
            _hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: _hierarchyNodeProfile.Key, chaseHierarchy: true);
            _hierarchyNodeProfile.PurgeCriteriaChangeType = eChangeType.none;

            if (em.ErrorFound)
            {
                BuildMessage(em: em, message: ref message);
            }

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deletePurgeCriteria: true);
                }
                else
                {
                    message = MIDText.GetText(eMIDTextCode.lbl_ACLL_LockAttemptFailed);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            finally
            {
                SAB.HierarchyServerSession.DequeueBranch(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key);
            }

            return true;
        }

        override public object NodePropertiesGetValues(ROProfileKeyParms parms)
        {
            return GetHierarchyNodeProfile(key: parms.Key, chaseHierarchy: true);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyPurge, (int)eSecurityTypes.All);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyPurge, (int)eSecurityTypes.All);
            }
        }
    }
}
