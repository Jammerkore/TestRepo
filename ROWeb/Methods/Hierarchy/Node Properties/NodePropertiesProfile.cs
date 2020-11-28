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
    public class NodePropertiesProfile : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        private List<HierarchyLevelComboObject> _OTSLevelList;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesProfile(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base(SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.HierarchyNode)
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
            RONodePropertiesProfile nodeProperties = new RONodePropertiesProfile(node: node);

            // populate modelProperties using Windows\NodeProperties.cs as a reference

            AddProfileValues(nodeProperties: nodeProperties);

            return nodeProperties;
        }

        private void AddProfileValues(RONodePropertiesProfile nodeProperties)
        {
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);
            HierarchyNodeProfile hnp = null;

            if (_hierarchyNodeProfile.HomeHierarchyParentRID != Include.NoRID)
            {
                nodeProperties.Parent = GetName.GetMerchandiseName(nodeRID: _hierarchyNodeProfile.HomeHierarchyParentRID, SAB: SAB);
            }

            nodeProperties.IsHierarchyNode = _hierarchyNodeProfile.NodeLevel == 0;

            nodeProperties.NodeDescription = _hierarchyNodeProfile.NodeDescription;
            nodeProperties.NodeID = _hierarchyNodeProfile.NodeID;
            nodeProperties.NodeName = _hierarchyNodeProfile.NodeName;
            nodeProperties.ProductType = EnumTools.VerifyEnumValue(_hierarchyNodeProfile.ProductType);
            switch (_hierarchyNodeProfile.ProductTypeInherited)
            {
                case eInheritedFrom.Node:
                    hnp = GetHierarchyNodeProfile(key: _hierarchyNodeProfile.ProductTypeInheritedFrom);
                    nodeProperties.ProductTypeInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                    break;
                case eInheritedFrom.HierarchyLevel:
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)MainHierProf.HierarchyLevels[_hierarchyNodeProfile.ProductTypeInheritedFrom];
                    nodeProperties.ProductTypeInheritedFromNode = new KeyValuePair<int, string>(_hierarchyNodeProfile.ProductTypeInheritedFrom, inheritedFromText + hlp.LevelID);
                    break;
                default:
                    break;
            }

            nodeProperties.LevelType = EnumTools.VerifyEnumValue(_hierarchyNodeProfile.LevelType);
            switch (_hierarchyNodeProfile.LevelType)
            {
                case eHierarchyLevelType.Color:
                    ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(_hierarchyNodeProfile.ColorOrSizeCodeRID);
                    nodeProperties.NodeName = ccp.ColorCodeName;
                    nodeProperties.ColorGroup = ccp.ColorCodeGroup;
                    break;
                case eHierarchyLevelType.Size:
                    SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(_hierarchyNodeProfile.ColorOrSizeCodeRID);
                    if (scp.Key != Include.NoRID)
                    {
                        nodeProperties.NodeName = scp.SizeCodeName;
                    }
                    break;
                default:
                    break;
            }

            nodeProperties.IsActive = _hierarchyNodeProfile.Active;

            nodeProperties.IsAllowApply = (!(_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational));
            if (_hierarchyNodeProfile.ApplyHNRIDFrom != Include.NoRID)
            {
                nodeProperties.ApplyNodePropertiesFromNode = new KeyValuePair<int, string>(_hierarchyNodeProfile.ApplyHNRIDFrom, GetHierarchyNodeProfile(key: _hierarchyNodeProfile.ApplyHNRIDFrom, chaseHierarchy: false).Text);
                if (_hierarchyNodeProfile.ApplyFromInherited != eInheritedFrom.None)
                {
                    if (hnp == null ||
                        hnp.Key != _hierarchyNodeProfile.ApplyFromInheritedFrom)
                    {
                        hnp = GetHierarchyNodeProfile(key: _hierarchyNodeProfile.ApplyFromInheritedFrom);
                    }
                    switch (_hierarchyNodeProfile.ApplyFromInherited)
                    {
                        case eInheritedFrom.Node:
                            if (_hierarchyNodeProfile.ApplyFromInheritedFrom != Include.NoRID)
                            {
                                nodeProperties.ApplyNodePropertiesFromNodeInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            nodeProperties.OTSForecastSelectType = EnumTools.VerifyEnumValue(_hierarchyNodeProfile.OTSPlanLevelSelectType);
            if (_hierarchyNodeProfile.OTSPlanLevelAnchorNode != Include.NoRID)
            {
                nodeProperties.OTSForecastAnchorNode = new KeyValuePair<int, string>(_hierarchyNodeProfile.OTSPlanLevelAnchorNode, SAB.HierarchyServerSession.GetNodeText(_hierarchyNodeProfile.OTSPlanLevelAnchorNode));
            }

            if (_hierarchyNodeProfile.OTSPlanLevelHierarchyLevelSequence > Include.Undefined)
            {
                BuildOTSLevelList(nodeProperties: nodeProperties);
            }

            nodeProperties.OTSForecastMaskType = EnumTools.VerifyEnumValue(_hierarchyNodeProfile.OTSPlanLevelMaskField);
            nodeProperties.OTSForecastMask = _hierarchyNodeProfile.OTSPlanLevelMask;

            switch (_hierarchyNodeProfile.OTSPlanLevelInherited)
            {
                case eInheritedFrom.Node:
                    if (hnp == null ||
                        hnp.Key != _hierarchyNodeProfile.OTSPlanLevelInheritedFrom)
                    {
                        hnp = GetHierarchyNodeProfile(key: _hierarchyNodeProfile.OTSPlanLevelInheritedFrom);
                    }
                    nodeProperties.OTSForecastInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                    break;
                default:
                    break;
            }

            nodeProperties.OTSForecastType = EnumTools.VerifyEnumValue(_hierarchyNodeProfile.OTSPlanLevelType);
            switch (_hierarchyNodeProfile.OTSPlanLevelTypeInherited)
            {
                case eInheritedFrom.Node:
                    if (hnp == null ||
                        hnp.Key != _hierarchyNodeProfile.OTSPlanLevelTypeInheritedFrom)
                    {
                        hnp = GetHierarchyNodeProfile(key: _hierarchyNodeProfile.OTSPlanLevelTypeInheritedFrom);
                    }
                    nodeProperties.OTSForecastTypeInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                    break;
                case eInheritedFrom.HierarchyLevel:

                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)MainHierProf.HierarchyLevels[_hierarchyNodeProfile.OTSPlanLevelTypeInheritedFrom];
                    nodeProperties.OTSForecastTypeInheritedFromNode = new KeyValuePair<int, string>(_hierarchyNodeProfile.OTSPlanLevelTypeInheritedFrom, inheritedFromText + hlp.LevelID);
                    break;
                case eInheritedFrom.HierarchyDefaults:
                    nodeProperties.OTSForecastTypeInheritedFromNode = new KeyValuePair<int, string>(Include.NoRID, inheritedFromText + "Hierarchy Defaults");
                    break;
                default:
                    break;
            }

        }

        private void BuildOTSLevelList(RONodePropertiesProfile nodeProperties)
        {
            int i;
            int levelIndex = 0;
            HierarchyLevelComboObject valItem;
            HierarchyNodeProfile anchorNodeProfile = null;
            int selectedIndex = -1;

            try
            {
                _OTSLevelList = new List<HierarchyLevelComboObject>();
                HierarchyProfile hpHome = SAB.HierarchyServerSession.GetHierarchyData(_hierarchyNodeProfile.HomeHierarchyRID);

                if (_hierarchyNodeProfile.OTSPlanLevelAnchorNode != Include.NoRID)
                {
                    anchorNodeProfile = GetHierarchyNodeProfile(key: _hierarchyNodeProfile.OTSPlanLevelAnchorNode);
                }

                // Load Level arrays

                _OTSLevelList.Add(new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.Undefined, Include.NoRID, Include.NoRID, "Undefined"));
                ++levelIndex;

                if (_hierarchyNodeProfile.OTSPlanLevelAnchorNode == Include.NoRID ||
                    (anchorNodeProfile != null &&
                    anchorNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
                    )
                {
                    _OTSLevelList.Add(new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.Undefined, hpHome.Key, 0, hpHome.HierarchyID));
                    ++levelIndex;

                    for (i = 1; i <= hpHome.HierarchyLevels.Count; i++)
                    {
                        if (((HierarchyLevelProfile)hpHome.HierarchyLevels[i]).LevelType != eHierarchyLevelType.Size)
                        {
                            _OTSLevelList.Add(new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.HierarchyLevel, hpHome.Key, ((HierarchyLevelProfile)hpHome.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)hpHome.HierarchyLevels[i]).LevelID));
                            ++levelIndex;
                        }
                    }
                }
                else
                {
                    _OTSLevelList.Add(new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.LevelOffset, anchorNodeProfile.Key, 0, anchorNodeProfile.Text, anchorNodeProfile.Text));
                    ++levelIndex;

                    int highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(_hierarchyNodeProfile.OTSPlanLevelAnchorNode);

                    // add guest levels
                    if (highestGuestLevel != int.MaxValue)
                    {
                        for (i = highestGuestLevel; i <= MainHierProf.HierarchyLevels.Count; i++)
                        {
                            if (i == 0)
                            {
                                _OTSLevelList.Add(new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.HierarchyLevel, MainHierProf.Key, 0, hpHome.HierarchyID));
                                ++levelIndex;
                            }
                            else
                            {
                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)MainHierProf.HierarchyLevels[i];
                                if (hlp.LevelType != eHierarchyLevelType.Size)
                                {
                                    _OTSLevelList.Add(new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.HierarchyLevel, MainHierProf.Key, ((HierarchyLevelProfile)MainHierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)MainHierProf.HierarchyLevels[i]).LevelID));
                                    ++levelIndex;
                                }
                            }
                        }
                    }

                    // add offsets 


                    DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(_hierarchyNodeProfile.OTSPlanLevelAnchorNode);
                    int longestBranchCount = hierarchyLevels.Rows.Count - 1;
                    int offset = 0;
                    for (i = 0; i < longestBranchCount; i++)
                    {
                        ++offset;
                        _OTSLevelList.Add(
                            new HierarchyLevelComboObject(levelIndex, ePlanLevelLevelType.LevelOffset,
                            anchorNodeProfile.HomeHierarchyRID,
                            offset,
                            null));
                        ++levelIndex;
                    }
                }

                for (i = 0; i < _OTSLevelList.Count; i++)
                {
                    valItem = (HierarchyLevelComboObject)_OTSLevelList[i];

                    if (_hierarchyNodeProfile.OTSPlanLevelLevelType == valItem.PlanLevelLevelType && valItem.HierarchyRID == _hierarchyNodeProfile.OTSPlanLevelHierarchyRID && valItem.Level == _hierarchyNodeProfile.OTSPlanLevelHierarchyLevelSequence)
                    {
                        selectedIndex = i;
                        nodeProperties.OTSForecastLevel = new KeyValuePair<int, string>(valItem.LevelIndex, valItem.ToString());
                        break;
                    }
                }

                foreach (HierarchyLevelComboObject level in _OTSLevelList)
                {
                    nodeProperties.OTSForecastLevelList.Add(new KeyValuePair<int, string>(level.LevelIndex, level.ToString()));
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesProfile nodePropertiesProfileData = (RONodePropertiesProfile)nodePropertiesData;

            

            if (SetProfile(nodePropertiesProfileData: nodePropertiesProfileData, message: ref message))
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
        /// <param name="nodePropertiesData">Input values for the node profile</param>
        /// <param name="message">The message</param>
        private bool SetProfile(RONodePropertiesProfile nodePropertiesProfileData, ref string message)
        {
            EditMsgs em = new EditMsgs();
            HierarchyNodeProfile parent = null;
            HierarchyProfile hp = null;

            if (NodeIDIsValid(ref em, nodePropertiesProfileData, ref message))
            {
                if (nodePropertiesProfileData.ParentIsSet)
                {
                    parent = GetHierarchyNodeProfile(key: nodePropertiesProfileData.Parent.Key);
                    hp = SAB.HierarchyServerSession.GetHierarchyData(parent.HomeHierarchyRID);
                }

                if (nodePropertiesProfileData.Node.Key == Include.NoRID)
                {
                    if (parent == null
                        || parent.Key == Include.NoRID)
                    {
                        message = "Parent is required to add node";
                        return false;
                    }
                    _hierarchyNodeProfile.NodeChangeType = eChangeType.add;
                    _hierarchyNodeProfile.HierarchyRID = parent.HomeHierarchyRID;
                    _hierarchyNodeProfile.HomeHierarchyParentRID = parent.Key;
                    if (!_hierarchyNodeProfile.Parents.Contains(parent.Key))
                    {
                        _hierarchyNodeProfile.Parents.Add(parent.Key);
                    }
                    if (hp.HierarchyType == eHierarchyType.alternate)
                    {
                        _hierarchyNodeProfile.LevelType = eHierarchyLevelType.Undefined;
                    }
                    else
                    {
                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[parent.HomeHierarchyLevel + 1];
                        _hierarchyNodeProfile.DisplayOption = hlp.LevelDisplayOption;
                        _hierarchyNodeProfile.LevelType = hlp.LevelType;
                    }
                }
                else
                if (_hierarchyNodeProfile.NodeChangeType == eChangeType.none)
                {
                    _hierarchyNodeProfile.NodeChangeType = eChangeType.update;
                }

                if (hp != null
                    && parent != null
                    && hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[parent.HomeHierarchyLevel + 1];
                    if (hlp.LevelType == eHierarchyLevelType.Size)
                    {
                        SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(nodePropertiesProfileData.NodeID);
                        _hierarchyNodeProfile.NodeName = scp.SizeCodeName;
                    }
                    else
                    {
                        _hierarchyNodeProfile.NodeName = nodePropertiesProfileData.NodeName;
                    }
                }
                else
                {
                    _hierarchyNodeProfile.NodeName = nodePropertiesProfileData.NodeName;
                }

                _hierarchyNodeProfile.NodeID = nodePropertiesProfileData.NodeID;
                _hierarchyNodeProfile.NodeDescription = nodePropertiesProfileData.NodeDescription;

                if (nodePropertiesProfileData.ProductType != nodePropertiesProfileData.OriginalProductType)
                {
                    _hierarchyNodeProfile.ProductType = nodePropertiesProfileData.ProductType;
                    _hierarchyNodeProfile.ProductTypeIsOverridden = true;
                }

                if (nodePropertiesProfileData.ApplyNodePropertiesFromNodeIsSet)
                {
                    _hierarchyNodeProfile.ApplyHNRIDFrom = nodePropertiesProfileData.ApplyNodePropertiesFromNode.Key;
                }
                else
                {
                    _hierarchyNodeProfile.ApplyHNRIDFrom = Include.NoRID;
                }

                if (nodePropertiesProfileData.OTSForecastAnchorNodeIsSet)
                {
                    if (_hierarchyNodeProfile.OTSPlanLevelAnchorNode != nodePropertiesProfileData.OTSForecastAnchorNode.Key)
                    {
                        _hierarchyNodeProfile.OTSPlanLevelAnchorNode = nodePropertiesProfileData.OTSForecastAnchorNode.Key;
                        _hierarchyNodeProfile.OTSPlanLevelIsOverridden = true;
                    }
                }
                else
                {
                    _hierarchyNodeProfile.OTSPlanLevelAnchorNode = Include.NoRID;
                }

                if (_hierarchyNodeProfile.OTSPlanLevelSelectType != nodePropertiesProfileData.OTSForecastSelectType)
                {
                    _hierarchyNodeProfile.OTSPlanLevelIsOverridden = true;
                }

                if (nodePropertiesProfileData.OTSForecastSelectType == ePlanLevelSelectType.HierarchyLevel)
                {
                    _hierarchyNodeProfile.OTSPlanLevelSelectType = ePlanLevelSelectType.HierarchyLevel;
                    if (nodePropertiesProfileData.OTSForecastLevelIsSet)
                    {
                        if (nodePropertiesProfileData.OTSForecastLevel.Key < _OTSLevelList.Count)
                        {
                            HierarchyLevelComboObject hlco = _OTSLevelList[nodePropertiesProfileData.OTSForecastLevel.Key];
                            if (_hierarchyNodeProfile.OTSPlanLevelHierarchyRID != hlco.HierarchyRID
                                || _hierarchyNodeProfile.OTSPlanLevelHierarchyLevelSequence != hlco.Level)
                            {
                                _hierarchyNodeProfile.OTSPlanLevelLevelType = hlco.PlanLevelLevelType;
                                _hierarchyNodeProfile.OTSPlanLevelHierarchyRID = hlco.HierarchyRID;
                                _hierarchyNodeProfile.OTSPlanLevelHierarchyLevelSequence = hlco.Level;
                                _hierarchyNodeProfile.OTSPlanLevelIsOverridden = true;
                            }
                        }
                    }
                }
                else if (nodePropertiesProfileData.OTSForecastSelectType == ePlanLevelSelectType.Node)
                {
                    _hierarchyNodeProfile.OTSPlanLevelSelectType = ePlanLevelSelectType.Node;
                    if (_hierarchyNodeProfile.OTSPlanLevelMaskField != nodePropertiesProfileData.OTSForecastMaskType)
                    {
                        _hierarchyNodeProfile.OTSPlanLevelMaskField = nodePropertiesProfileData.OTSForecastMaskType;
                        _hierarchyNodeProfile.OTSPlanLevelIsOverridden = true;
                    }

                    if (nodePropertiesProfileData.OTSForecastMaskIsSet)
                    {
                        if (_hierarchyNodeProfile.OTSPlanLevelMask != nodePropertiesProfileData.OTSForecastMask)
                        {
                            _hierarchyNodeProfile.OTSPlanLevelMask = nodePropertiesProfileData.OTSForecastMask;
                            _hierarchyNodeProfile.OTSPlanLevelIsOverridden = true;
                        }
                    }
                }

                if (nodePropertiesProfileData.OTSForecastType != nodePropertiesProfileData.OriginalOTSForecastType)
                {
                    _hierarchyNodeProfile.OTSPlanLevelType = nodePropertiesProfileData.OTSForecastType;
                    _hierarchyNodeProfile.OTSPlanLevelTypeIsOverridden = true;
                }
            }
            else
            {
                BuildMessage(em: em, message: ref message);
                return false;
            }

            return true;
        }

        

        private bool NodeIDIsValid(ref EditMsgs em, RONodePropertiesProfile nodePropertiesProfileData, ref string message)
        {
            try
            {
                HierarchyMaintenance.NodeIDValid(ref em, _hierarchyNodeProfile.HierarchyRID, _hierarchyNodeProfile.HomeHierarchyParentRID, nodePropertiesProfileData.NodeID);
                if (em.ErrorFound)
                {
                    BuildMessage(em: em, message: ref message);
                    return false;
                }
                else if (nodePropertiesProfileData.LevelType != eHierarchyLevelType.Color &&
                    nodePropertiesProfileData.LevelType != eHierarchyLevelType.Size)
                {
                    HierarchyNodeProfile hnp = GetHierarchyNodeProfile(ID: nodePropertiesProfileData.NodeID);
                    if (hnp.Key != Include.NoRID &&
                        hnp.Key != _hierarchyNodeProfile.Key)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeIDAlreadyExists, this.GetType().Name);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

         private bool UpdateProfile()
        {
            EditMsgs em = new EditMsgs();
            bool nodeAdded = false;

            if (_hierarchyNodeProfile.Key > Include.NoRID)
            {
                _hierarchyNodeProfile.NodeChangeType = eChangeType.update;
            }
            else
            {
                _hierarchyNodeProfile.NodeChangeType = eChangeType.add;
                nodeAdded = true;
            }

            _hierarchyNodeProfile.Key = HierarchyMaintenance.ProcessNodeProfileInfo(ref em, _hierarchyNodeProfile);

            if (nodeAdded)
            {
                _hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeDataForUpdate(aNodeRID: _hierarchyNodeProfile.Key, aAllowReadOnly: true);
            }

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (!ApplicationUtilities.AllowDeleteFromInUse(key: _hierarchyNodeProfile.Key, profileType: _hierarchyNodeProfile.ProfileType, SAB: SAB))
            {
                return false;
            }

            int nodeRID = _hierarchyNodeProfile.Key;
            EditMsgs em = new EditMsgs();
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _hierarchyNodeProfile.NodeChangeType = eChangeType.delete;
            }
            else
            {
                _hierarchyNodeProfile.DeleteNode = true;
                _hierarchyNodeProfile.NodeChangeType = eChangeType.markedForDelete;
            }

            HierarchyMaintenance.ProcessNodeProfileInfo(ref em, _hierarchyNodeProfile);

            if (!em.ErrorFound)
            {
                Folder_DeleteAll_Shortcut(key: nodeRID, folderType: eProfileType.HierarchyNode, message: ref message);
            }
            else
            {
                BuildMessage(em: em, message: ref message);
            }

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {



            return false;
        }

        override public object NodePropertiesGetValues(ROProfileKeyParms parms)
        {
            if (_hierarchyNodeProfile != null)
            {
                return _hierarchyNodeProfile;
            }
            else
            {
                return GetHierarchyNodeProfile(key: parms.Key, chaseHierarchy: true);
            }
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyProfile, (int)eSecurityTypes.All);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyProfile, (int)eSecurityTypes.All);
            }
        }
    }
}
