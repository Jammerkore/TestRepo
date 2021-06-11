using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
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
    public class NodePropertiesSizeCurves : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======

        private SizeCurveCriteriaList _sizeCurvesCriteriaList;
        private SizeCurveDefaultCriteriaProfile _sizeCurveDefaultCriteriaProfile;
        private SizeCurveToleranceProfile _sizeCurveToleranceProfile;
        private SizeCurveSimilarStoreList _sizeCurveSimilarStoreList;
        private List<HierarchyLevelComboObject> _merchandiseLevelList;
        private List<HierarchyLevelComboObject> _toleranceLevelList;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesSizeCurves(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.SizeCurve)
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
            _sizeCurvesCriteriaList = (SizeCurveCriteriaList)nodePropertiesData;
            var deletedProfileList = (from SizeCurveCriteriaProfile profile in _sizeCurvesCriteriaList.ArrayList
                                      where profile.CriteriaChangeType == eChangeType.delete
                                      select profile).ToList();
            if(deletedProfileList.Count > 0)
            {
                deletedProfileList.ForEach(p => { _sizeCurvesCriteriaList.Remove(p); });
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesSizeCurves nodeProperties = new RONodePropertiesSizeCurves(node: node);

            // populate nodeProperties using Windows\NodeProperties.cs as a reference
            int attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            if (parms is RONodePropertyAttributeKeyParms)
            {
                RONodePropertyAttributeKeyParms nodePropertyParms = (RONodePropertyAttributeKeyParms)parms;
                if (nodePropertyParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyParms.AttributeKey;
                }
            }

            nodeProperties.SimilarStoresAttribute = GetName.GetAttributeName(key: attributeKey);

            AddSizeCurveCriteria(nodeProperties: nodeProperties, message: ref message);

            AddSizeCurveTolerance(nodeProperties: nodeProperties, message: ref message);

            AddSizeCurveSimilarStores(nodeProperties: nodeProperties, message: ref message);

            return nodeProperties;
        }

        private void AddSizeCurveCriteria(RONodePropertiesSizeCurves nodeProperties, ref string message)
        {
            RONodePropertiesSizeCurveCriteria sizeCurveCriteria;
            HierarchyNodeProfile hnp;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);
            int i;
            HierarchyLevelComboObject valItem;
            int selectedIndex = -1;

            if (_sizeCurveDefaultCriteriaProfile == null)
            {
                _sizeCurveDefaultCriteriaProfile = GetSizeCurveDefaultCriteria(key: nodeProperties.Node.Key);
            }

            if (_sizeCurveDefaultCriteriaProfile.DefaultRIDIsInheritedFromRID != Include.NoRID)
            {
                BuildSizeCurveCriteriaLevelList(aStartNode: _sizeCurveDefaultCriteriaProfile.DefaultRIDIsInheritedFromRID);
            }
            else
            {
                BuildSizeCurveCriteriaLevelList(aStartNode: HierarchyNodeProfile.Key);
            }

            foreach (SizeCurveCriteriaProfile sccp in _sizeCurvesCriteriaList)
            {
                sizeCurveCriteria = new RONodePropertiesSizeCurveCriteria(
                    key: sccp.Key
                    );

                if (_sizeCurveDefaultCriteriaProfile.DefaultRID != Include.NoRID
                    && _sizeCurveDefaultCriteriaProfile.DefaultRID == sccp.Key)
                {
                    sizeCurveCriteria.CriteriaIsDefault = true;
                    if (_sizeCurveDefaultCriteriaProfile.DefaultRIDIsInherited)
                    {
                        hnp = GetHierarchyNodeProfile(key: _sizeCurveDefaultCriteriaProfile.DefaultRIDIsInheritedFromRID);
                        sizeCurveCriteria.CriteriaInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                    }
                }

                if (sccp.CriteriaInheritedFromNodeRID != Include.NoRID)
                {
                    hnp = GetHierarchyNodeProfile(key: sccp.CriteriaInheritedFromNodeRID);
                    sizeCurveCriteria.CriteriaInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }

                for (i = 0; i < _merchandiseLevelList.Count; i++)
                {
                    valItem = (HierarchyLevelComboObject)_merchandiseLevelList[i];

                    if ((sccp.CriteriaLevelType == eLowLevelsType.HierarchyLevel && valItem.PlanLevelLevelType == ePlanLevelLevelType.HierarchyLevel && valItem.Level == sccp.CriteriaLevelSequence)
                        || sccp.CriteriaLevelType == eLowLevelsType.LevelOffset && valItem.PlanLevelLevelType == ePlanLevelLevelType.LevelOffset && valItem.Level == sccp.CriteriaLevelOffset)
                    {
                        selectedIndex = i;
                        sizeCurveCriteria.Merchandise = new KeyValuePair<int, string>(valItem.LevelIndex, valItem.ToString());
                        break;
                    }
                }

                DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(sccp.CriteriaDateRID);
                sizeCurveCriteria.CriteriaDate = new KeyValuePair<int, string>(sccp.CriteriaDateRID, dr.DisplayDate);

                if (sccp.CriteriaApplyLostSalesInd)
                {
                    sizeCurveCriteria.ApplyLostSales = sccp.CriteriaApplyLostSalesInd;
                }

                if (sccp.CriteriaOLLRID != Include.NoRID)
                {
                    sizeCurveCriteria.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(modelRID: sccp.CriteriaOLLRID, SAB: SAB);
                }

                if (sccp.CriteriaCustomOLLRID != Include.NoRID)
                {
                    sizeCurveCriteria.CustomLowLevelsModel = GetName.GetOverrideLowLevelsModel(modelRID: sccp.CriteriaCustomOLLRID, SAB: SAB);
                }

                if (sccp.CriteriaSizeGroupRID != Include.NoRID)
                {
                    sizeCurveCriteria.SizeGroup = GetName.GetSizeGroup(key: sccp.CriteriaSizeGroupRID);
                }

                if (!string.IsNullOrWhiteSpace(sccp.CriteriaCurveName))
                {
                    sizeCurveCriteria.SizeCurve = sccp.CriteriaCurveName;
                }

                if (sccp.CriteriaSgRID != Include.NoRID)
                {
                    sizeCurveCriteria.Attribute = GetName.GetAttributeName(key: sccp.CriteriaSgRID);
                }
    
                if (sccp.CriteriaIsInherited)
                {
                    nodeProperties.SizeCurveInheritedCriteria.Add(sizeCurveCriteria);
                }
                else
                {
                    nodeProperties.SizeCurveCriteria.Add(sizeCurveCriteria);
                }
            }

            // build list for merchandise drop down
            foreach (HierarchyLevelComboObject level in _merchandiseLevelList)
            {
                nodeProperties.MerchandiseList.Add(new KeyValuePair<int, string>(level.LevelIndex, level.ToString()));
            }
            BuildSizeCurveCriteriaSizeGroupList(nodeProperties);
            BuildSizeCurveAttributeList(nodeProperties);
        }

        private void BuildSizeCurveCriteriaLevelList(int aStartNode)
        {
            HierarchyProfile localHome;                 
            HierarchyNodeProfile nodeProf;
            int startLevel;
            int i;
            int highestGuestLevel;
            int longestBranchCount;
            int offset;

            try
            {
                nodeProf = GetHierarchyNodeProfile(key: aStartNode);
                localHome = SAB.HierarchyServerSession.GetHierarchyData(nodeProf.HomeHierarchyRID);

                if (HierarchyProfile == null)
                {
                    HierarchyProfile = SAB.HierarchyServerSession.GetHierarchyData(nodeProf.HomeHierarchyRID);
                }

                // Load Level arrays

                _merchandiseLevelList = new List<HierarchyLevelComboObject>();
                _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.Undefined, Include.NoRID, Include.Undefined, " "));

                if (nodeProf.HomeHierarchyType == eHierarchyType.organizational)
                {
                    if (nodeProf.HomeHierarchyLevel == 0)
                    {
                        _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.HierarchyLevel, localHome.Key, 0, localHome.HierarchyID));
                        startLevel = 1;
                    }
                    else
                    {
                        _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.HierarchyLevel, localHome.Key, ((HierarchyLevelProfile)localHome.HierarchyLevels[nodeProf.HomeHierarchyLevel]).Key, ((HierarchyLevelProfile)localHome.HierarchyLevels[nodeProf.HomeHierarchyLevel]).LevelID));
                        startLevel = nodeProf.HomeHierarchyLevel + 1;
                    }

                    for (i = startLevel; i <= localHome.HierarchyLevels.Count; i++)
                    {
                        if (((HierarchyLevelProfile)localHome.HierarchyLevels[i]).LevelType != eHierarchyLevelType.Size)
                        {
                            _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.HierarchyLevel, localHome.Key, ((HierarchyLevelProfile)localHome.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)localHome.HierarchyLevels[i]).LevelID));
                        }
                    }
                }
                else
                {
                    _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.HierarchyLevel, HierarchyProfile.Key, 0, nodeProf.Text));
                    
                    highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(nodeProf.Key);

                    // add guest levels to comboBox
                    if (highestGuestLevel != int.MaxValue)
                    {
                        for (i = highestGuestLevel; i <= MainHierProf.HierarchyLevels.Count; i++)
                        {
                            if (i == 0)
                            {
                                _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.HierarchyLevel, MainHierProf.Key, 0, localHome.HierarchyID));
                            }
                            else
                            {
                                if (((HierarchyLevelProfile)MainHierProf.HierarchyLevels[i]).LevelType != eHierarchyLevelType.Size)
                                {
                                    _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.HierarchyLevel, MainHierProf.Key, ((HierarchyLevelProfile)MainHierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)MainHierProf.HierarchyLevels[i]).LevelID));
                                }
                            }
                        }
                    }

                    // add offsets to comboBox

                    DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(nodeProf.Key);
                    longestBranchCount = hierarchyLevels.Rows.Count - 1;
                    offset = 0;

                    for (i = 0; i < longestBranchCount; i++)
                    {
                        ++offset;
                        _merchandiseLevelList.Add(new HierarchyLevelComboObject(_merchandiseLevelList.Count, ePlanLevelLevelType.LevelOffset, HierarchyProfile.Key, offset, "+" + offset.ToString()));
                    }
                        
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildSizeCurveCriteriaSizeGroupList(RONodePropertiesSizeCurves nodeProperties)
        {
            SizeGroupList _sizeCurveCriteriaGroupList = new SizeGroupList(eProfileType.SizeGroup);
            _sizeCurveCriteriaGroupList.LoadAll(false);
            nodeProperties.SizeGroupList.Add(new KeyValuePair<int, string>(Include.NoRID, " "));
            SizeGroupProfile valueItem;
            for (int i = 0; i < _sizeCurveCriteriaGroupList.Count; i++)
            {
                valueItem = (SizeGroupProfile)_sizeCurveCriteriaGroupList[i];
                nodeProperties.SizeGroupList.Add(new KeyValuePair<int, string>(valueItem.Key, valueItem.SizeGroupName));
            }
        }

        private void BuildSizeCurveAttributeList(RONodePropertiesSizeCurves nodeProperties)
        {
            eStoreGroupSelectType storeGroupSelectType;
            ProfileList _storeEligibilityGroupList;
            StoreGroupListViewProfile valueItem;
            FunctionSecurityProfile _storeUserAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
            if (_storeUserAttrSecLvl.AccessDenied)
            {
                storeGroupSelectType = eStoreGroupSelectType.GlobalOnly;
            }
            else
            {
                storeGroupSelectType = eStoreGroupSelectType.All;
            }
            _storeEligibilityGroupList = StoreMgmt.StoreGroup_GetListViewList(storeGroupSelectType, storeGroupSelectType != eStoreGroupSelectType.GlobalOnly);
            nodeProperties.AttributeList.Add(new KeyValuePair<int, string>(Include.NoRID, " "));
            for (int i = 0; i < _storeEligibilityGroupList.Count; i++)
            {
                valueItem = (StoreGroupListViewProfile)_storeEligibilityGroupList[i];
                nodeProperties.AttributeList.Add(new KeyValuePair<int, string>(valueItem.Key, valueItem.Name));
            }

        }

        private void AddSizeCurveTolerance(RONodePropertiesSizeCurves nodeProperties, ref string message)
        {
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);
            HierarchyNodeProfile hnp = null;
            int i;
            HierarchyLevelComboObject valItem;
            int selectedIndex = -1;

            if (_sizeCurveToleranceProfile == null)
            {
                _sizeCurveToleranceProfile = GetSizeCurveTolerance(key: nodeProperties.Node.Key);
            }

            BuildSizeCurveToleranceLevelList(aStartNode: HierarchyNodeProfile.Key);

            nodeProperties.ApplyMinimumToZeroTolerance = _sizeCurveToleranceProfile.ApplyMinToZeroTolerance;
            if (_sizeCurveToleranceProfile.ToleranceMinAvg > Include.Undefined)
            {
                nodeProperties.ToleranceMinimumAverage = _sizeCurveToleranceProfile.ToleranceMinAvg;

                if (_sizeCurveToleranceProfile.ToleranceMinAvgIsInherited)
                {
                    if (hnp == null ||
                        hnp.Key != _sizeCurveToleranceProfile.ToleranceMinAvgInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: _sizeCurveToleranceProfile.ToleranceMinAvgInheritedFromNodeRID);
                    }

                    nodeProperties.ToleranceMinimumAverageInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
            }

            if (_sizeCurveToleranceProfile.ToleranceLevelType != eLowLevelsType.None)
            {
                for (i = 0; i < _toleranceLevelList.Count; i++)
                {
                    valItem = (HierarchyLevelComboObject)_toleranceLevelList[i];

                    if ((_sizeCurveToleranceProfile.ToleranceLevelType == eLowLevelsType.HierarchyLevel && valItem.PlanLevelLevelType == ePlanLevelLevelType.HierarchyLevel && valItem.Level == _sizeCurveToleranceProfile.ToleranceLevelSeq)
                        || _sizeCurveToleranceProfile.ToleranceLevelType == eLowLevelsType.LevelOffset && valItem.PlanLevelLevelType == ePlanLevelLevelType.LevelOffset && valItem.Level == _sizeCurveToleranceProfile.ToleranceLevelOffset)
                    {
                        selectedIndex = i;
                        nodeProperties.ToleranceLevel = new KeyValuePair<int, string>(valItem.LevelIndex, valItem.ToString());
                        break;
                    }
                }

                if (_sizeCurveToleranceProfile.ToleranceLevelIsInherited)
                {
                    if (hnp == null ||
                        hnp.Key != _sizeCurveToleranceProfile.ToleranceLevelInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: _sizeCurveToleranceProfile.ToleranceLevelInheritedFromNodeRID);
                    }

                    nodeProperties.ToleranceLevelInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
            }

            if (_sizeCurveToleranceProfile.ToleranceSalesTolerance > Include.Undefined)
            {
                nodeProperties.ToleranceSalesTolerance = _sizeCurveToleranceProfile.ToleranceSalesTolerance;

                if (_sizeCurveToleranceProfile.ToleranceSalesToleranceIsInherited)
                {
                    if (hnp == null ||
                        hnp.Key != _sizeCurveToleranceProfile.ToleranceSalesToleranceInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: _sizeCurveToleranceProfile.ToleranceSalesToleranceInheritedFromNodeRID);
                    }

                    nodeProperties.ToleranceSalesToleranceInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
            }


            if (_sizeCurveToleranceProfile.ToleranceIdxUnitsInd != eNodeChainSalesType.None)
            {
                nodeProperties.ToleranceSalesToleranceType = _sizeCurveToleranceProfile.ToleranceIdxUnitsInd;

                //if (_sizeCurveToleranceProfile.ToleranceIdxUnitsIndIsInherited)
                //{
                //    if (hnp == null ||
                //        hnp.Key != _sizeCurveToleranceProfile.ToleranceIdxUnitsIndInheritedFromNodeRID)
                //    {
                //        hnp = GetHierarchyNodeProfile(key: _sizeCurveToleranceProfile.ToleranceIdxUnitsIndInheritedFromNodeRID);
                //    }

                //    nodeProperties.ToleranceSalesToleranceTypeInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                //}
            }

            if (_sizeCurveToleranceProfile.ToleranceMinTolerance > Include.Undefined)
            {
                nodeProperties.ToleranceMinimumPercent = _sizeCurveToleranceProfile.ToleranceMinTolerance;
                if (_sizeCurveToleranceProfile.ToleranceMinToleranceIsInherited)
                {
                    if (hnp == null ||
                        hnp.Key != _sizeCurveToleranceProfile.ToleranceMinToleranceInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: _sizeCurveToleranceProfile.ToleranceMinToleranceInheritedFromNodeRID);
                    }

                    nodeProperties.ToleranceMinimumPercentInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
            }

            if (_sizeCurveToleranceProfile.ToleranceMaxTolerance > Include.Undefined)
            {
                nodeProperties.ToleranceMaximumPercent = _sizeCurveToleranceProfile.ToleranceMaxTolerance;

                if (_sizeCurveToleranceProfile.ToleranceMaxToleranceIsInherited)
                {
                    if (hnp == null ||
                        hnp.Key != _sizeCurveToleranceProfile.ToleranceMaxToleranceInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: _sizeCurveToleranceProfile.ToleranceMaxToleranceInheritedFromNodeRID);
                    }

                    nodeProperties.ToleranceMaximumPercentInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
            }

            // build list for tolerance level drop down
            foreach (HierarchyLevelComboObject level in _toleranceLevelList)
            {
                nodeProperties.ToleranceLevelList.Add(new KeyValuePair<int, string>(level.LevelIndex, level.ToString()));
            }
        }

        private void BuildSizeCurveToleranceLevelList(int aStartNode)
        {
            HierarchyProfile localHome;
            HierarchyNodeProfile nodeProf;
            NodeAncestorList hierNal;
            HierarchyProfile hierProf;
            int i;
            SortedList ancestorList;
            NodeAncestorProfile ancProf;
            int orgHierRID;
            string levelName;
            HierarchyLevelProfile hlp;

            try
            {
                nodeProf = GetHierarchyNodeProfile(key: aStartNode);
                localHome = SAB.HierarchyServerSession.GetHierarchyData(nodeProf.HomeHierarchyRID);

                // Load Level arrays

                _toleranceLevelList = new List<HierarchyLevelComboObject>();
                _toleranceLevelList.Add(new HierarchyLevelComboObject(_toleranceLevelList.Count, ePlanLevelLevelType.Undefined, Include.NoRID, Include.Undefined, MIDText.GetTextOnly(eMIDTextCode.lbl_NoHigherLevel)));
                
                ancestorList = SAB.HierarchyServerSession.GetAllNodeAncestors(nodeProf.Key);

                if (nodeProf.HomeHierarchyType == eHierarchyType.organizational)
                {
                    orgHierRID = nodeProf.HomeHierarchyRID;
                }
                else
                {
                    orgHierRID = Include.NoRID;
                }

                hlp = null;

                _toleranceLevelList.Add(new HierarchyLevelComboObject(_toleranceLevelList.Count, ePlanLevelLevelType.HierarchyLevel, localHome.Key, 0, localHome.HierarchyID));

                for (i = 1; i <= localHome.HierarchyLevels.Count && (hlp == null || hlp.LevelType != eHierarchyLevelType.Style); i++)
                {
                    hlp = (HierarchyLevelProfile)localHome.HierarchyLevels[i];
                    _toleranceLevelList.Add(new HierarchyLevelComboObject(_toleranceLevelList.Count, ePlanLevelLevelType.HierarchyLevel, localHome.Key, ((HierarchyLevelProfile)localHome.HierarchyLevels[hlp.Key]).Key, ((HierarchyLevelProfile)localHome.HierarchyLevels[hlp.Key]).LevelID));
                }

                foreach (DictionaryEntry dictEnt in ancestorList)
                {
                    hierNal = (NodeAncestorList)dictEnt.Value;
                    hierProf = SAB.HierarchyServerSession.GetHierarchyData((int)dictEnt.Key);

                    for (i = hierNal.Count - 1; i >= 0; i--)
                    {
                        ancProf = (NodeAncestorProfile)hierNal[i];

                        if (ancProf.HomeHierarchyOwner == Include.GlobalUserRID)
                        {
                            if (ancProf.HomeHierarchyRID != orgHierRID)
                            {
                                if (nodeProf.HomeHierarchyType == eHierarchyType.organizational ||
                                    (nodeProf.HomeHierarchyRID != ancProf.HomeHierarchyRID))
                                {
                                    levelName = hierProf.HierarchyID + ": " + GetHierarchyNodeProfile(key: ancProf.Key, chaseHierarchy: false, buildQualifiedID: true).Text;
                                }
                                else
                                {
                                    levelName = "-" + Convert.ToString(nodeProf.HomeHierarchyLevel - ancProf.HomeHierarchyLevel);
                                }

                                _toleranceLevelList.Add(new HierarchyLevelComboObject(_toleranceLevelList.Count, ePlanLevelLevelType.HierarchyLevel, ancProf.HomeHierarchyRID, ancProf.HomeHierarchyLevel, levelName));
                            }
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

        private void AddSizeCurveSimilarStores(RONodePropertiesSizeCurves nodeProperties, ref string message)
        {
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            if (_sizeCurveSimilarStoreList == null)
            {
                _sizeCurveSimilarStoreList = GetSizeCurveSimilarStores(key: nodeProperties.Node.Key);
            }

            AddAttributeSets(nodeProperties: nodeProperties,
                sizeCurveSimilarStoreList: _sizeCurveSimilarStoreList);
        }

        private void AddAttributeSets(RONodePropertiesSizeCurves nodeProperties, SizeCurveSimilarStoreList sizeCurveSimilarStoreList)
        {
            RONodePropertiesSizeCurvesSimilarStoresAttributeSet similarStoresAttributeSet;
            RONodePropertiesSizeCurvesSimilarStore sizeCurvesStore;
            SizeCurveSimilarStoreProfile sccp;
            HierarchyNodeProfile hnp;
            StoreProfile sp;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            ProfileList storeSimilarStoresGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(nodeProperties.SimilarStoresAttribute.Key, true);

            if (!nodeProperties.SimilarStoresAttributeSetIsSet)
            {
                StoreGroupLevelListViewProfile attributeSet = (StoreGroupLevelListViewProfile)storeSimilarStoresGroupLevelList[0];
                nodeProperties.SimilarStoresAttributeSet = new KeyValuePair<int, string>(attributeSet.Key, attributeSet.Name);
            }


            foreach (StoreGroupLevelListViewProfile sglp in storeSimilarStoresGroupLevelList)
            {
                if (sglp.Key != nodeProperties.SimilarStoresAttributeSet.Key)
                {
                    continue;
                }

                similarStoresAttributeSet = new RONodePropertiesSizeCurvesSimilarStoresAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name));
                foreach (StoreProfile storeProfile in sglp.Stores)
                {
                    sizeCurvesStore = new RONodePropertiesSizeCurvesSimilarStore(store: new KeyValuePair<int, string>(storeProfile.Key, storeProfile.Text));
                    if (sizeCurveSimilarStoreList.Contains(storeProfile.Key))
                    {
                        sccp = (SizeCurveSimilarStoreProfile)sizeCurveSimilarStoreList.FindKey(storeProfile.Key);

                        // similar store
                        sp = StoreMgmt.StoreProfile_Get(sccp.SimStoreRID);
                        if (sp.ActiveInd)
                        {
                            sizeCurvesStore.SimilarStoresValues.SimilarStore = new KeyValuePair<int, string>(sp.Key, sp.Text);
                        }

                        if (sizeCurvesStore.SimilarStoresValues.SimilarStoreIsSet)
                        {
                            sizeCurvesStore.SimilarStoresValues.SimilarStoreTimePeriod = new KeyValuePair<int, string>(sccp.SimStoreUntilDateRangeRID, sccp.SimStoreDisplayDate);
                        }

                        if (sccp.SimStoreInheritedFromNodeRID != nodeProperties.Node.Key
                            && sccp.SimStoreIsInherited
                            && sccp.SimStoreInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sccp.SimStoreInheritedFromNodeRID);
                            sizeCurvesStore.SimilarStoresValues.SimilarStoreInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }
                    }
                    similarStoresAttributeSet.SimilarStores.Add(sizeCurvesStore);
                }

                // nodeProperties.SimilarStoresAttributeSets.Add(similarStoresAttributeSet);
                nodeProperties.SimilarStoresAttributeSetValues = similarStoresAttributeSet;
            }
        }

        

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesSizeCurves nodePropertiesSizeCurvesData = (RONodePropertiesSizeCurves)nodePropertiesData;

            if (_sizeCurvesCriteriaList == null)
            {
                _sizeCurvesCriteriaList = GetSizeCurves(key: nodePropertiesSizeCurvesData.Node.Key);
            }

            if (_sizeCurveDefaultCriteriaProfile == null)
            {
                _sizeCurveDefaultCriteriaProfile = GetSizeCurveDefaultCriteria(key: nodePropertiesSizeCurvesData.Node.Key);
            }

            if (_sizeCurveToleranceProfile == null)
            {
                _sizeCurveToleranceProfile = GetSizeCurveTolerance(key: nodePropertiesSizeCurvesData.Node.Key);
            }

            if (_sizeCurveSimilarStoreList == null)
            {
                _sizeCurveSimilarStoreList = GetSizeCurveSimilarStores(key: nodePropertiesSizeCurvesData.Node.Key);
            }

            if (SetSizeCurves(nodePropertiesSizeCurvesData: nodePropertiesSizeCurvesData, message: ref message))
            {
                if (SetSizeCurveTolerance(nodePropertiesSizeCurvesData: nodePropertiesSizeCurvesData, message: ref message))
                {
                    if (SetSizeCurveSimilarStores(nodePropertiesSizeCurvesData: nodePropertiesSizeCurvesData, message: ref message))
                    {
                        if (!applyOnly)
                        {
                            if (!UpdateSizeCurves(message: ref message))
                            {
                                successful = false;
                            }
                            else if (!UpdateSizeCurveDefaultProfile(message: ref message))
                            {
                                successful = false;
                            }
                            else if (!UpdateSizeCurveTolerance(message: ref message))
                            {
                                successful = false;
                            }
                            else if (!UpdateSizeCurveSimilarStores(message: ref message))
                            {
                                successful = false;
                            }
                        }
                    }
                    else
                    {
                        successful = false;
                    }
                }
                else
                {
                    successful = false;
                }

            }
            else
            {
                successful = false;
            }

            return _sizeCurvesCriteriaList;
        }

        /// <summary>
        /// Takes values from input class and updates the size curves memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the size curves</param>
        /// <param name="message">The message</param>
        private bool SetSizeCurves(RONodePropertiesSizeCurves nodePropertiesSizeCurvesData, ref string message)
        {
            SizeCurveCriteriaProfile sccp;
            _sizeCurveDefaultCriteriaProfile.DefaultRID = Include.NoRID;
            int sequence = 0;

            foreach (RONodePropertiesSizeCurveCriteria sizeCurveCriteria in nodePropertiesSizeCurvesData.SizeCurveInheritedCriteria)
            {
                _sizeCurveDefaultCriteriaProfile.DefaultChangeType = eChangeType.update;
                if (sizeCurveCriteria.CriteriaIsDefault == true)
                {
                    if (_sizeCurveDefaultCriteriaProfile.DefaultRID != sizeCurveCriteria.Key)
                    {
                        _sizeCurveDefaultCriteriaProfile.DefaultRID = sizeCurveCriteria.Key;
                        _sizeCurveDefaultCriteriaProfile.DefaultRIDIsInherited = false;
                        _sizeCurveDefaultCriteriaProfile.DefaultRIDIsInheritedFromRID = Include.NoRID;
                    }
                }
            }

            foreach (RONodePropertiesSizeCurveCriteria sizeCurveCriteria in nodePropertiesSizeCurvesData.SizeCurveCriteria)
            {
                if (_sizeCurvesCriteriaList.Contains(sizeCurveCriteria.Key))
                {
                    sccp = (SizeCurveCriteriaProfile)_sizeCurvesCriteriaList.FindKey(sizeCurveCriteria.Key);
                    if (sccp.CriteriaChangeType == eChangeType.none)
                    {
                        sccp.CriteriaChangeType = eChangeType.update;
                    }
                    _sizeCurvesCriteriaList.Update(sccp);
                }
                else
                {
                    sccp = new SizeCurveCriteriaProfile(sizeCurveCriteria.Key);
                    sccp.CriteriaChangeType = eChangeType.add;
                    _sizeCurvesCriteriaList.Add(sccp);
                }

                sccp.CriteriaSequence = sequence;
                if (sizeCurveCriteria.MerchandiseIsSet
                    && sizeCurveCriteria.Merchandise.Key < _merchandiseLevelList.Count)
                {
                    HierarchyLevelComboObject levelCombo = _merchandiseLevelList[sizeCurveCriteria.Merchandise.Key];
                    sccp.CriteriaLevelType = (eLowLevelsType)levelCombo.PlanLevelLevelType;
                    sccp.CriteriaLevelRID = levelCombo.HierarchyRID;
                    if (sccp.CriteriaLevelType == eLowLevelsType.HierarchyLevel)
                    {
                        sccp.CriteriaLevelSequence = levelCombo.Level;
                        sccp.CriteriaLevelOffset = Include.Undefined;
                    }
                    else
                    {
                        sccp.CriteriaLevelOffset = levelCombo.Level;
                        sccp.CriteriaLevelSequence = Include.Undefined;
                    }
                }
                else
                {
                    sccp.CriteriaLevelType = eLowLevelsType.None;
                    sccp.CriteriaLevelRID = Include.NoRID;
                    sccp.CriteriaLevelSequence = Include.Undefined;
                    sccp.CriteriaLevelOffset = Include.Undefined;
                }
                if (sizeCurveCriteria.CriteriaDateIsSet)
                {
                    sccp.CriteriaDateRID = sizeCurveCriteria.CriteriaDate.Key;
                }
                else
                {
                    sccp.CriteriaDateRID = Include.NoRID;
                }
                if (sizeCurveCriteria.ApplyLostSalesIsSet)
                {
                    sccp.CriteriaApplyLostSalesInd = (bool)sizeCurveCriteria.ApplyLostSales;
                }
                else
                {
                    sccp.CriteriaApplyLostSalesInd = false;
                }
                if (sizeCurveCriteria.OverrideLowLevelsModelIsSet)
                {
                    sccp.CriteriaOLLRID = sizeCurveCriteria.OverrideLowLevelsModel.Key;
                }
                else
                {
                    sccp.CriteriaOLLRID = Include.NoRID;
                }
                if (sizeCurveCriteria.CustomLowLevelsModelIsSet)
                {
                    sccp.CriteriaCustomOLLRID = sizeCurveCriteria.CustomLowLevelsModel.Key;
                }
                else
                {
                    sccp.CriteriaCustomOLLRID = Include.NoRID;
                }
                if (sizeCurveCriteria.SizeGroupIsSet)
                {
                    sccp.CriteriaSizeGroupRID = sizeCurveCriteria.SizeGroup.Key;
                }
                else
                {
                    sccp.CriteriaSizeGroupRID = Include.NoRID;
                }
                if (sizeCurveCriteria.SizeCurveIsSet)
                {
                    sccp.CriteriaCurveName = sizeCurveCriteria.SizeCurve;
                }
                else
                {
                    sccp.CriteriaCurveName = string.Empty;
                }
                if (sizeCurveCriteria.AttributeIsSet)
                {
                    sccp.CriteriaSgRID = sizeCurveCriteria.Attribute.Key;
                }
                else
                {
                    sccp.CriteriaSgRID = Include.NoRID;
                }

                _sizeCurveDefaultCriteriaProfile.DefaultChangeType = eChangeType.update;

                if (sizeCurveCriteria.CriteriaIsDefault == true)
                {
                    if (_sizeCurveDefaultCriteriaProfile.DefaultRID != sizeCurveCriteria.Key)
                    {
                        _sizeCurveDefaultCriteriaProfile.DefaultChangeType = eChangeType.update;
                        _sizeCurveDefaultCriteriaProfile.DefaultRID = sizeCurveCriteria.Key;
                        _sizeCurveDefaultCriteriaProfile.DefaultRIDIsInherited = false;
                        _sizeCurveDefaultCriteriaProfile.DefaultRIDIsInheritedFromRID = Include.NoRID;
                    }
                }

                ++sequence;
            }

            // delete all criteria that was not updated.
            foreach (SizeCurveCriteriaProfile sccp2 in _sizeCurvesCriteriaList)
            {
                if (!sccp2.CriteriaIsInherited
                    && sccp2.CriteriaChangeType != eChangeType.update
                    && sccp2.CriteriaChangeType != eChangeType.add
                    )
                {
                    sccp2.CriteriaChangeType = eChangeType.delete;
                }
                else
                {
                    sccp2.CriteriaSequence = sequence;
                    ++sequence;
                }
            }

            return true;
        }

        /// <summary>
        /// Takes values from input class and updates the size curve tolerance memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the size curve tolerance</param>
        /// <param name="message">The message</param>
        private bool SetSizeCurveTolerance(RONodePropertiesSizeCurves nodePropertiesSizeCurvesData, ref string message)
        {
            if (nodePropertiesSizeCurvesData.ApplyMinimumToZeroToleranceIsSet)
                _sizeCurveToleranceProfile.ApplyMinToZeroTolerance = nodePropertiesSizeCurvesData.ApplyMinimumToZeroTolerance.Value;
            if (nodePropertiesSizeCurvesData.ToleranceMinimumAverageIsSet)
            {
                if (_sizeCurveToleranceProfile.ToleranceMinAvg != (double)nodePropertiesSizeCurvesData.ToleranceMinimumAverage)
                {
                    _sizeCurveToleranceProfile.ToleranceMinAvg = (double)nodePropertiesSizeCurvesData.ToleranceMinimumAverage;
                    _sizeCurveToleranceProfile.ToleranceMinAvgIsInherited = false;
                }
            }
            else
            {
                _sizeCurveToleranceProfile.ToleranceMinAvg = Include.Undefined;
            }

            if (nodePropertiesSizeCurvesData.ToleranceLevelIsSet
                && nodePropertiesSizeCurvesData.ToleranceLevel.Key < _toleranceLevelList.Count)
            {
                HierarchyLevelComboObject levelCombo = _toleranceLevelList[nodePropertiesSizeCurvesData.ToleranceLevel.Key];
                if (_sizeCurveToleranceProfile.ToleranceLevelType == eLowLevelsType.HierarchyLevel
                    && _sizeCurveToleranceProfile.ToleranceLevelSeq != levelCombo.Level)
                {
                    _sizeCurveToleranceProfile.ToleranceLevelType = (eLowLevelsType)levelCombo.PlanLevelLevelType;
                    _sizeCurveToleranceProfile.ToleranceLevelRID = levelCombo.HierarchyRID;
                    _sizeCurveToleranceProfile.ToleranceLevelSeq = levelCombo.Level;
                    _sizeCurveToleranceProfile.ToleranceLevelOffset = Include.Undefined;
                    _sizeCurveToleranceProfile.ToleranceLevelIsInherited = false;
                }
                else if (_sizeCurveToleranceProfile.ToleranceLevelType == eLowLevelsType.LevelOffset
                    && _sizeCurveToleranceProfile.ToleranceLevelOffset != levelCombo.Level)
                {
                    _sizeCurveToleranceProfile.ToleranceLevelType = (eLowLevelsType)levelCombo.PlanLevelLevelType;
                    _sizeCurveToleranceProfile.ToleranceLevelRID = levelCombo.HierarchyRID;
                    _sizeCurveToleranceProfile.ToleranceLevelSeq = Include.Undefined;
                    _sizeCurveToleranceProfile.ToleranceLevelOffset = levelCombo.Level;
                    _sizeCurveToleranceProfile.ToleranceLevelIsInherited = false;
                }
            }
            else
            {
                _sizeCurveToleranceProfile.ToleranceLevelType = eLowLevelsType.None;
                _sizeCurveToleranceProfile.ToleranceLevelRID = Include.NoRID;
                _sizeCurveToleranceProfile.ToleranceLevelSeq = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceLevelOffset = Include.Undefined;
            }

            if (nodePropertiesSizeCurvesData.ToleranceSalesToleranceIsSet)
            {
                if (_sizeCurveToleranceProfile.ToleranceSalesTolerance != (double)nodePropertiesSizeCurvesData.ToleranceSalesTolerance)
                {
                    _sizeCurveToleranceProfile.ToleranceSalesTolerance = (double)nodePropertiesSizeCurvesData.ToleranceSalesTolerance;
                    _sizeCurveToleranceProfile.ToleranceSalesToleranceIsInherited = false;
                }
            }
            else
            {
                _sizeCurveToleranceProfile.ToleranceSalesTolerance = Include.Undefined;
            }

            if (nodePropertiesSizeCurvesData.ToleranceSalesToleranceTypeIsSet)
            {
                if (_sizeCurveToleranceProfile.ToleranceIdxUnitsInd != nodePropertiesSizeCurvesData.ToleranceSalesToleranceType)
                {
                    _sizeCurveToleranceProfile.ToleranceIdxUnitsInd = nodePropertiesSizeCurvesData.ToleranceSalesToleranceType;
                    _sizeCurveToleranceProfile.ToleranceIdxUnitsIndIsInherited = false;
                }
            }
            else
            {
                _sizeCurveToleranceProfile.ToleranceIdxUnitsInd = eNodeChainSalesType.None;
            }

            if (nodePropertiesSizeCurvesData.ToleranceMinimumPercentIsSet)
            {
                if (_sizeCurveToleranceProfile.ToleranceMinTolerance != (double)nodePropertiesSizeCurvesData.ToleranceMinimumPercent)
                {
                    _sizeCurveToleranceProfile.ToleranceMinTolerance = (double)nodePropertiesSizeCurvesData.ToleranceMinimumPercent;
                    _sizeCurveToleranceProfile.ToleranceMinToleranceIsInherited = false;
                }
            }
            else
            {
                _sizeCurveToleranceProfile.ToleranceMinTolerance = Include.Undefined;
            }

            if (nodePropertiesSizeCurvesData.ToleranceMaximumPercentIsSet)
            {
                if (_sizeCurveToleranceProfile.ToleranceMaxTolerance != (double)nodePropertiesSizeCurvesData.ToleranceMaximumPercent)
                {
                    _sizeCurveToleranceProfile.ToleranceMaxTolerance = (double)nodePropertiesSizeCurvesData.ToleranceMaximumPercent;
                    _sizeCurveToleranceProfile.ToleranceMaxToleranceIsInherited = false;
                }
            }
            else
            {
                _sizeCurveToleranceProfile.ToleranceMaxTolerance = Include.Undefined;
            }

            _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;

            return true;
        }

        /// <summary>
        /// Takes values from input class and updates the size curve similar stores memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the size curve similar stores</param>
        /// <param name="message">The message</param>
        private bool SetSizeCurveSimilarStores(RONodePropertiesSizeCurves nodePropertiesSizeCurvesData, ref string message)
        {
            bool setSuccessful = true;
            bool similarStoreUpdated = false;

            SizeCurveSimilarStoreProfile scsp;

            //foreach (RONodePropertiesSizeCurvesSimilarStoresAttributeSet attributeSet in nodePropertiesSizeCurvesData.SimilarStoresAttributeSets)
            RONodePropertiesSizeCurvesSimilarStoresAttributeSet attributeSet = nodePropertiesSizeCurvesData.SimilarStoresAttributeSetValues;
            {
                if (attributeSet != null)
                {
                    foreach (RONodePropertiesSizeCurvesSimilarStore similarStore in attributeSet.SimilarStores)
                    {
                        similarStoreUpdated = false;

                        if (_sizeCurveSimilarStoreList.Contains(similarStore.Store.Key))
                        {
                            scsp = (SizeCurveSimilarStoreProfile)_sizeCurveSimilarStoreList.FindKey(similarStore.Store.Key);
                            scsp.SimilarStoreChangeType = eChangeType.update;
                        }
                        else
                        {
                            scsp = new SizeCurveSimilarStoreProfile(similarStore.Store.Key);
                            scsp.SimilarStoreChangeType = eChangeType.add;
                        }

                        if (attributeSet.SimilarStoresValues.SimilarStoreIsSet
                            && scsp.SimStoreRID != attributeSet.SimilarStoresValues.SimilarStore.Key)
                        {
                            StoreProfile sp = StoreMgmt.StoreProfile_Get(attributeSet.SimilarStoresValues.SimilarStore.Key);
                            if (sp.ActiveInd)
                            {
                                if (sp.Key == Include.NoRID)
                                {
                                    sp.Key = 1;
                                }
                                scsp.SimStoreRID = sp.Key;
                                similarStoreUpdated = true;
                                scsp.SimStoreIsInherited = false;
                                scsp.SimStoreInheritedFromNodeRID = Include.NoRID;
                            }
                            else
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreNotFound);
                                setSuccessful = false;
                            }
                        }
                        else if (similarStore.SimilarStoresValues.SimilarStoreIsSet
                            && scsp.SimStoreRID != similarStore.SimilarStoresValues.SimilarStore.Key)
                        {
                            if (similarStore.SimilarStoresValues.SimilarStore.Key != Include.NoRID)
                            {
                                StoreProfile sp = StoreMgmt.StoreProfile_Get(similarStore.SimilarStoresValues.SimilarStore.Key);
                                if (sp.ActiveInd)
                                {
                                    if (sp.Key == Include.NoRID)
                                    {
                                        sp.Key = 1;
                                    }
                                    scsp.SimStoreRID = sp.Key;
                                    similarStoreUpdated = true;
                                    scsp.SimStoreIsInherited = false;
                                    scsp.SimStoreInheritedFromNodeRID = Include.NoRID;
                                }
                                else
                                {
                                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreNotFound);
                                    setSuccessful = false;
                                }
                            }
                            else
                            {
                                if (scsp.SimStoreRID != Include.NoRID)
                                {
                                    scsp.SimilarStoreChangeType = eChangeType.delete;
                                }
                                scsp.SimStoreRID = Include.NoRID;
                            }
                        }
                        else if (!similarStore.SimilarStoresValues.SimilarStoreIsSet
                           && scsp.SimilarStoreChangeType == eChangeType.update)
                        {
                            scsp.SimilarStoreChangeType = eChangeType.delete;
                        }

                        if (scsp.SimStoreRID != Include.NoRID
                            && attributeSet.SimilarStoresValues.SimilarStoreTimePeriodIsSet
                            && scsp.SimStoreUntilDateRangeRID != similarStore.SimilarStoresValues.SimilarStoreTimePeriod.Key)
                        {
                            similarStoreUpdated = true;
                            scsp.SimStoreUntilDateRangeRID = attributeSet.SimilarStoresValues.SimilarStoreTimePeriod.Key;
                            scsp.SimStoreDisplayDate = attributeSet.SimilarStoresValues.SimilarStoreTimePeriod.Value;
                            scsp.SimStoreIsInherited = false;
                            scsp.SimStoreInheritedFromNodeRID = Include.NoRID;
                        }
                        else if (scsp.SimStoreRID != Include.NoRID
                            && similarStore.SimilarStoresValues.SimilarStoreTimePeriodIsSet
                            && scsp.SimStoreUntilDateRangeRID != similarStore.SimilarStoresValues.SimilarStoreTimePeriod.Key)
                        {
                            similarStoreUpdated = true;
                            scsp.SimStoreUntilDateRangeRID = similarStore.SimilarStoresValues.SimilarStoreTimePeriod.Key;
                            scsp.SimStoreDisplayDate = similarStore.SimilarStoresValues.SimilarStoreTimePeriod.Value;
                            scsp.SimStoreIsInherited = false;
                            scsp.SimStoreInheritedFromNodeRID = Include.NoRID;
                        }

                        if (similarStoreUpdated
                            && scsp.SimilarStoreChangeType == eChangeType.add)
                        {
                            _sizeCurveSimilarStoreList.Add(scsp);
                        }
                    }
                }
            }

            return setSuccessful;
        }


        private bool UpdateSizeCurves(ref string message)
        {
            SAB.HierarchyServerSession.SizeCurveCriteriaUpdate(HierarchyNodeProfile.Key, _sizeCurvesCriteriaList);

            return true;
        }

        private bool UpdateSizeCurveDefaultProfile(ref string message)
        {
            SAB.HierarchyServerSession.SizeCurveDefaultCriteriaUpdate(HierarchyNodeProfile.Key, _sizeCurveDefaultCriteriaProfile);

            return true;
        }

        private bool UpdateSizeCurveTolerance(ref string message)
        {
            SAB.HierarchyServerSession.SizeCurveToleranceUpdate(HierarchyNodeProfile.Key, _sizeCurveToleranceProfile);

            return true;
        }

        private bool UpdateSizeCurveSimilarStores(ref string message)
        {
            SAB.HierarchyServerSession.SizeCurveSimilarStoreUpdate(HierarchyNodeProfile.Key, _sizeCurveSimilarStoreList);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            ProfileList storeList = SAB.StoreServerSession.GetActiveStoresList();

            // Criteria
            if (_sizeCurvesCriteriaList == null)
            {
                _sizeCurvesCriteriaList = GetSizeCurves(key: key);
            }

            if (_sizeCurveDefaultCriteriaProfile == null)
            {
                _sizeCurveDefaultCriteriaProfile = GetSizeCurveDefaultCriteria(key: key);
            }

            foreach (SizeCurveCriteriaProfile sccp in _sizeCurvesCriteriaList)
            {
                if (!sccp.CriteriaIsInherited)
                {
                    sccp.CriteriaChangeType = eChangeType.delete;
                }
            }

            if (_sizeCurveDefaultCriteriaProfile.DefaultRID != Include.NoRID && !_sizeCurveDefaultCriteriaProfile.DefaultRIDIsInherited)
            {
                _sizeCurveDefaultCriteriaProfile.DefaultRID = Include.NoRID;
                _sizeCurveDefaultCriteriaProfile.DefaultChangeType = eChangeType.update;
            }

            SAB.HierarchyServerSession.SizeCurveCriteriaUpdate(key, _sizeCurvesCriteriaList);

            SAB.HierarchyServerSession.SizeCurveDefaultCriteriaUpdate(key, _sizeCurveDefaultCriteriaProfile);

            // Tolerance

            if (_sizeCurveToleranceProfile == null)
            {
                _sizeCurveToleranceProfile = GetSizeCurveTolerance(key: key);
            }

            if (_sizeCurveToleranceProfile.ToleranceMinAvg != Include.Undefined && !_sizeCurveToleranceProfile.ToleranceMinAvgIsInherited)
            {
                _sizeCurveToleranceProfile.ToleranceMinAvg = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;
            }
            if (_sizeCurveToleranceProfile.ToleranceLevelType != eLowLevelsType.None && !_sizeCurveToleranceProfile.ToleranceLevelIsInherited)
            {
                _sizeCurveToleranceProfile.ToleranceLevelType = eLowLevelsType.None;
                _sizeCurveToleranceProfile.ToleranceLevelRID = Include.NoRID;
                _sizeCurveToleranceProfile.ToleranceLevelSeq = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceLevelOffset = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;
            }
            if (_sizeCurveToleranceProfile.ToleranceSalesTolerance != Include.Undefined && !_sizeCurveToleranceProfile.ToleranceSalesToleranceIsInherited)
            {
                _sizeCurveToleranceProfile.ToleranceSalesTolerance = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;
            }
            if (_sizeCurveToleranceProfile.ToleranceIdxUnitsInd != eNodeChainSalesType.None && !_sizeCurveToleranceProfile.ToleranceIdxUnitsIndIsInherited)
            {
                _sizeCurveToleranceProfile.ToleranceIdxUnitsInd = eNodeChainSalesType.None;
                _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;
            }
            if (_sizeCurveToleranceProfile.ToleranceMinTolerance != Include.Undefined && !_sizeCurveToleranceProfile.ToleranceMinToleranceIsInherited)
            {
                _sizeCurveToleranceProfile.ToleranceMinTolerance = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;
            }
            if (_sizeCurveToleranceProfile.ToleranceMaxTolerance != Include.Undefined && !_sizeCurveToleranceProfile.ToleranceMaxToleranceIsInherited)
            {
                _sizeCurveToleranceProfile.ToleranceMaxTolerance = Include.Undefined;
                _sizeCurveToleranceProfile.ToleranceChangeType = eChangeType.update;
            }

            SAB.HierarchyServerSession.SizeCurveToleranceUpdate(key, _sizeCurveToleranceProfile);

            // Similar Stores
            if (_sizeCurveSimilarStoreList == null)
            {
                _sizeCurveSimilarStoreList = GetSizeCurveSimilarStores(key: key);
            }

            foreach (SizeCurveSimilarStoreProfile scssp in _sizeCurveSimilarStoreList)
            {
                scssp.SimilarStoreChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.SizeCurveSimilarStoreUpdate(key, _sizeCurveSimilarStoreList);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteSizeCurveCriteria: true, deleteSizeCurveTolerance: true, deleteSizeCurveSimilarStore: true);
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
            return GetSizeCurves(key: parms.Key);
        }

        private SizeCurveCriteriaList GetSizeCurves(int key)
        {
            return SAB.HierarchyServerSession.GetSizeCurveCriteriaList(nodeRID: key, forCopy: false);
        }

        private SizeCurveDefaultCriteriaProfile GetSizeCurveDefaultCriteria(int key)
        {
            if (_sizeCurvesCriteriaList == null)
            {
                GetSizeCurves(key: key);
            }

            return SAB.HierarchyServerSession.GetSizeCurveDefaultCriteriaProfile(nodeRID: key, sccl: _sizeCurvesCriteriaList, forCopy: false);
        }

        private SizeCurveToleranceProfile GetSizeCurveTolerance(int key)
        {
            return SAB.HierarchyServerSession.GetSizeCurveToleranceProfile(nodeRID: key, forCopy: false);
        }

        private SizeCurveSimilarStoreList GetSizeCurveSimilarStores(int key)
        {
            return SAB.HierarchyServerSession.GetSizeCurveSimilarStoreList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), nodeRID: key, chaseHierarchy: true, forCopy: false);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertySizeCurves, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertySizeCurves, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            int attributeSetKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesSizeCurves)
            {
                RONodePropertiesSizeCurves nodePropertiesSizeCurvesData = (RONodePropertiesSizeCurves)parms.RONodeProperties;
                attributeKey = nodePropertiesSizeCurvesData.SimilarStoresAttribute.Key;
                if (nodePropertiesSizeCurvesData.SimilarStoresAttributeSetIsSet)
                {
                    attributeSetKey = nodePropertiesSizeCurvesData.SimilarStoresAttributeSet.Key;
                }
            }

            RONodePropertyAttributeKeyParms profileKeyParms = new RONodePropertyAttributeKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                profileType: profileType,
                key: key,
                readOnly: readOnly,
                attributeKey: attributeKey,
                attributeSetKey: attributeSetKey
                );

            return profileKeyParms;
        }

    }
}
