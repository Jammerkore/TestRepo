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
    public class NodePropertiesStockMinMax : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        private NodeStockMinMaxesProfile _nodeStockMinMaxesProfile = null;
        private StoreGradeList _storeGradeList = null;
        private bool _stockMinMaxIsPopulated = false;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesStockMinMax(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.StockMinMax)
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

        int _definedAttribute = Include.Undefined;

        override public RONodeProperties NodePropertiesGetData(ROProfileKeyParms parms, object nodePropertiesData, ref string message, bool applyOnly = false)
        {
            _nodeStockMinMaxesProfile = (NodeStockMinMaxesProfile)nodePropertiesData;

            int attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            int attributeSetKey = Include.NoRID;
            if (parms is RONodePropertyAttributeKeyParms)
            {
                RONodePropertyAttributeKeyParms nodePropertyStockMinMaxParms = (RONodePropertyAttributeKeyParms)parms;
                if (nodePropertyStockMinMaxParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyStockMinMaxParms.AttributeKey;
                    attributeSetKey = nodePropertyStockMinMaxParms.AttributeSetKey;
                }
            }

            if (_definedAttribute == Include.Undefined)
            {
                _definedAttribute = _nodeStockMinMaxesProfile.NodeStockStoreGroupRID;
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesStockMinMax nodeProperties = new RONodePropertiesStockMinMax(node: node,
                attribute: GetName.GetAttributeName(key: attributeKey),
                attributeSet: GetName.GetAttributeSetName(key: attributeSetKey),
                definedAttribute: GetName.GetAttributeName(key: _definedAttribute)
                );

            // populate modelProperties using Windows\NodeProperties.cs as a reference
            AddAttributeSetStoreGrades(nodeProperties: nodeProperties, 
                stockMinMaxesProfile: _nodeStockMinMaxesProfile,
                attributeSetKey: attributeSetKey,
                message: ref message);

            _stockMinMaxIsPopulated = true;
            return nodeProperties;
        }

        private void AddAttributeSetStoreGrades(RONodePropertiesStockMinMax nodeProperties, NodeStockMinMaxesProfile stockMinMaxesProfile, int attributeSetKey, ref string message)
        {
            RONodePropertiesStockMinMaxAttributeSet minMaxAttributeSet;
            HierarchyNodeProfile hnp = null;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            int attributeKey = nodeProperties.Attribute.Key;
            
            if (_storeGradeList == null)
            {
                _storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(nodeProperties.Node.Key, false, true);
            }

            if (_nodeStockMinMaxesProfile.NodeStockMinMaxsIsInherited)
            {
                hnp = GetHierarchyNodeProfile(key: _nodeStockMinMaxesProfile.NodeStockMinMaxsInheritedFromNodeRID);
                nodeProperties.MinimumMaximumsInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
            }

            nodeProperties.Attribute = GetName.GetAttributeName(key: attributeKey);

            ProfileList attributeSets = StoreMgmt.StoreGroup_GetLevelListViewList(attributeKey);

            // if attribute set is not a member of the selected attribute, remove the attribute set so the first set will be used
            if (attributeSets.FindKey(nodeProperties.AttributeSet.Key) == null)
            {
                nodeProperties.AttributeSet = default(KeyValuePair<int, string>);
            }

            if (!nodeProperties.AttributeSetIsSet)
            {
                nodeProperties.AttributeSet = GetName.GetAttributeSetName(key: attributeSets[0].Key);
                attributeSetKey = nodeProperties.AttributeSet.Key;
            }

            foreach (StoreGroupLevelListViewProfile sglp in attributeSets)
            {
                if (sglp.Key != attributeSetKey)
                {
                    continue;
                }

                minMaxAttributeSet = new RONodePropertiesStockMinMaxAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name));

                NodeStockMinMaxSetProfile minMaxSetProfile = (NodeStockMinMaxSetProfile)_nodeStockMinMaxesProfile.NodeSetList.FindKey(sglp.Key);

                AddDefaultEntries(minMaxAttributeSet: minMaxAttributeSet, minMaxSetProfile: minMaxSetProfile, message: ref message);

                AddGradeEntries(minMaxAttributeSet: minMaxAttributeSet, minMaxSetProfile: minMaxSetProfile, message: ref message);

                nodeProperties.StockMinMaxAttributeSet = minMaxAttributeSet;
                nodeProperties.AttributeSet = minMaxAttributeSet.AttributeSet;
            }
        }

        private void AddDefaultEntries (RONodePropertiesStockMinMaxAttributeSet minMaxAttributeSet, NodeStockMinMaxSetProfile minMaxSetProfile, ref string message)
        {
            RONodePropertiesStockMinMaxStoreGradeEntry minMaxStoreGradeEntry;
            NodeStockMinMaxProfile minMaxProfile = null;
            RONodePropertiesStockMinMaxStoreGrade minMaxStoreGrade;

            minMaxStoreGrade = new RONodePropertiesStockMinMaxStoreGrade(storeGrade: new KeyValuePair<int, string>(Include.Undefined, "Default"));

            // Add default date range
            minMaxStoreGradeEntry = new RONodePropertiesStockMinMaxStoreGradeEntry(dateRange: new KeyValuePair<int, string>(Include.UndefinedCalendarDateRange, "(Default)"));

            if (minMaxSetProfile == null)
            {
                minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);
            }
            else
            {
                minMaxProfile = (NodeStockMinMaxProfile)minMaxSetProfile.Defaults.MinMaxList.FindKey(Include.UndefinedCalendarDateRange);
                if (minMaxProfile != null)
                {
                    if (minMaxProfile.Minimum != int.MinValue)
                    {
                        minMaxStoreGradeEntry.Minimum = minMaxProfile.Minimum;
                    }
                    if (minMaxProfile.Maximum != int.MaxValue)
                    {
                        minMaxStoreGradeEntry.Maximum = minMaxProfile.Maximum;
                    }
                }

                minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);

                // set default date ranges
                foreach (NodeStockMinMaxProfile MMProfile in minMaxSetProfile.Defaults.MinMaxList)
                {
                    if (MMProfile.Key != Include.UndefinedCalendarDateRange)
                    {
                        minMaxStoreGradeEntry = new RONodePropertiesStockMinMaxStoreGradeEntry(GetName.GetCalendarDateRange(calendarDateRID: MMProfile.Key, SAB: SAB));
                        if (MMProfile.Minimum != int.MinValue)
                        {
                            minMaxStoreGradeEntry.Minimum = MMProfile.Minimum;
                        }
                        if (MMProfile.Maximum != int.MaxValue)
                        {
                            minMaxStoreGradeEntry.Maximum = MMProfile.Maximum;
                        }

                        minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);
                    }
                }
            }

            minMaxAttributeSet.StoreGrades.Add(minMaxStoreGrade);
        }

        private void AddGradeEntries(RONodePropertiesStockMinMaxAttributeSet minMaxAttributeSet, NodeStockMinMaxSetProfile minMaxSetProfile, ref string message)
        {
            RONodePropertiesStockMinMaxStoreGradeEntry minMaxStoreGradeEntry;
            NodeStockMinMaxProfile minMaxProfile = null;
            NodeStockMinMaxBoundaryProfile minMaxBoundaryProfile = null;
            RONodePropertiesStockMinMaxStoreGrade minMaxStoreGrade;

            // populate store grades
            foreach (StoreGradeProfile sgp in _storeGradeList)
            {
                minMaxStoreGrade = new RONodePropertiesStockMinMaxStoreGrade(storeGrade: new KeyValuePair<int, string>(sgp.Boundary, sgp.StoreGrade));

                // Add default date range
                minMaxStoreGradeEntry = new RONodePropertiesStockMinMaxStoreGradeEntry(dateRange: new KeyValuePair<int, string>(Include.UndefinedCalendarDateRange, "(Default)"));

                if (minMaxSetProfile == null)
                {
                    minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);
                }
                else
                {
                    minMaxBoundaryProfile = (NodeStockMinMaxBoundaryProfile)minMaxSetProfile.BoundaryList.FindKey(sgp.Boundary);
                    if (minMaxBoundaryProfile == null)
                    {
                        minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);
                    }
                    else
                    {
                        minMaxProfile = (NodeStockMinMaxProfile)minMaxBoundaryProfile.MinMaxList.FindKey(Include.UndefinedCalendarDateRange);
                        if (minMaxProfile != null)
                        {
                            if (minMaxProfile.Minimum != int.MinValue)
                            {
                                minMaxStoreGradeEntry.Minimum = minMaxProfile.Minimum;
                            }
                            if (minMaxProfile.Maximum != int.MaxValue)
                            {
                                minMaxStoreGradeEntry.Maximum = minMaxProfile.Maximum;
                            }
                        }

                        minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);

                        // Add grade entries
                        foreach (NodeStockMinMaxProfile MMProfile in minMaxBoundaryProfile.MinMaxList)
                        {
                            if (MMProfile.Key != Include.UndefinedCalendarDateRange)
                            {
                                minMaxStoreGradeEntry = new RONodePropertiesStockMinMaxStoreGradeEntry(GetName.GetCalendarDateRange(calendarDateRID: MMProfile.Key, SAB: SAB));
                                if (MMProfile.Minimum != int.MinValue)
                                {
                                    minMaxStoreGradeEntry.Minimum = MMProfile.Minimum;
                                }
                                if (MMProfile.Maximum != int.MaxValue)
                                {
                                    minMaxStoreGradeEntry.Maximum = MMProfile.Maximum;
                                }

                                minMaxStoreGrade.StoreGradeEntries.Add(minMaxStoreGradeEntry);
                            }
                        }
                    }
                }

                minMaxAttributeSet.StoreGrades.Add(minMaxStoreGrade);
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesStockMinMax nodePropertiesStockMinMaxData = (RONodePropertiesStockMinMax)nodePropertiesData;

            if (_nodeStockMinMaxesProfile == null)
            {
                _nodeStockMinMaxesProfile = GetStockMinMaxes(key: nodePropertiesStockMinMaxData.Node.Key);
            }

            if (_storeGradeList == null)
            {
                _storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(nodePropertiesStockMinMaxData.Node.Key, false, true);
            }

            if (SetAttributeSetStoreGrades(nodePropertiesStockMinMaxData: nodePropertiesStockMinMaxData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateMinMaxes(message: ref message))
                    {
                        successful = false;
                        _definedAttribute = Include.Undefined;
                    }
                }
            }
            else
            {
                successful = false;
            }

            return _nodeStockMinMaxesProfile;
        }

        private bool SetAttributeSetStoreGrades(RONodePropertiesStockMinMax nodePropertiesStockMinMaxData, ref string message)
        {
            NodeStockMinMaxProfile minMaxProfile = null;
            NodeStockMinMaxBoundaryProfile minMaxBoundaryProfile = null;
            int boundary, dateRangeRID, minimum, maximum;
            bool addSet = false;
            bool addedEntries = false;
            NodeStockMinMaxSetProfile minMaxSetProfile;

            _nodeStockMinMaxesProfile.NodeStockMinMaxsIsInherited = false;

            if (nodePropertiesStockMinMaxData.Attribute.Key != _nodeStockMinMaxesProfile.NodeStockStoreGroupRID)
            {
                _nodeStockMinMaxesProfile.NodeStockStoreGroupRID = nodePropertiesStockMinMaxData.Attribute.Key;
                _nodeStockMinMaxesProfile.NodeSetList.Clear();
                _nodeStockMinMaxesProfile.NodeStockMinMaxChangeType = eChangeType.delete;
            }
            //else
            {
                _nodeStockMinMaxesProfile.NodeStockMinMaxChangeType = eChangeType.update;
                RONodePropertiesStockMinMaxAttributeSet attributeSet = nodePropertiesStockMinMaxData.StockMinMaxAttributeSet;
                {
                    addSet = false;
                    addedEntries = false;
                    minMaxSetProfile = (NodeStockMinMaxSetProfile)_nodeStockMinMaxesProfile.NodeSetList.FindKey(attributeSet.AttributeSet.Key);
                    if (minMaxSetProfile == null)
                    {
                        minMaxSetProfile = new NodeStockMinMaxSetProfile(attributeSet.AttributeSet.Key);
                        addSet = true;
                    }
                    else
                    {
                        minMaxSetProfile.Defaults.MinMaxList.Clear();
                        minMaxSetProfile.BoundaryList.Clear();
                    }

                    // Reverse order to ascending by boundary so matches order of data from hierarchy service just in case
                    foreach (RONodePropertiesStockMinMaxStoreGrade storeGrade in attributeSet.StoreGrades.OrderBy(sg => sg.StoreGrade.Key))
                    {
                        boundary = storeGrade.StoreGrade.Key;
                        foreach (RONodePropertiesStockMinMaxStoreGradeEntry storeGradeEntry in storeGrade.StoreGradeEntries)
                        {
                            dateRangeRID = storeGradeEntry.DateRange.Key;

                            if (storeGradeEntry.MinimumIsSet)
                            {
                                minimum = (int)storeGradeEntry.Minimum;
                            }
                            else
                            {
                                minimum = int.MinValue;
                            }

                            if (storeGradeEntry.MaximumIsSet)
                            {
                                maximum = (int)storeGradeEntry.Maximum;
                            }
                            else
                            {
                                maximum = int.MaxValue;
                            }

                            if (minimum != int.MinValue ||
                                maximum != int.MaxValue)
                            {
                                addedEntries = true;
                                minMaxProfile = new NodeStockMinMaxProfile(dateRangeRID, minimum, maximum);

                                if (boundary == Include.Undefined)
                                {
                                    minMaxSetProfile.Defaults.MinMaxList.Add(minMaxProfile);
                                }
                                else if (dateRangeRID == Include.UndefinedCalendarDateRange)
                                {
                                    minMaxBoundaryProfile = new NodeStockMinMaxBoundaryProfile(boundary);
                                    minMaxBoundaryProfile.MinMaxList.Add(minMaxProfile);
                                    minMaxSetProfile.BoundaryList.Add(minMaxBoundaryProfile);
                                }
                                else
                                {
                                    if (minMaxBoundaryProfile != null &&
                                        boundary != minMaxBoundaryProfile.Key)
                                    {
                                        minMaxBoundaryProfile = (NodeStockMinMaxBoundaryProfile)minMaxSetProfile.BoundaryList.FindKey(boundary);
                                    }
                                    if (minMaxBoundaryProfile == null)
                                    {
                                        minMaxBoundaryProfile = new NodeStockMinMaxBoundaryProfile(boundary);
                                        minMaxSetProfile.BoundaryList.Add(minMaxBoundaryProfile);
                                    }
                                    minMaxBoundaryProfile.MinMaxList.Add(minMaxProfile);
                                }
                            }
                        }
                    }

                    if (addSet
                        && addedEntries)
                    {
                        _nodeStockMinMaxesProfile.NodeSetList.Add(minMaxSetProfile);
                    }
                }
            }

            return true;
        }

        private bool UpdateMinMaxes(ref string message)
        {
            SAB.HierarchyServerSession.StockMinMaxUpdate(HierarchyNodeProfile.Key, _nodeStockMinMaxesProfile);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_nodeStockMinMaxesProfile == null)
            {
                _nodeStockMinMaxesProfile = GetStockMinMaxes(key: key);
            }

            _nodeStockMinMaxesProfile.NodeStockMinMaxChangeType = eChangeType.delete;

            SAB.HierarchyServerSession.StockMinMaxUpdate(HierarchyNodeProfile.Key, _nodeStockMinMaxesProfile);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteStockMinMaxes: true);
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
            return GetStockMinMaxes(key: parms.Key); ;
        }

        private NodeStockMinMaxesProfile GetStockMinMaxes(int key)
        {
            return SAB.HierarchyServerSession.GetStockMinMaxes(nodeRID: key);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyStockMinMax, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyStockMinMax, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            int attributeSetKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesStockMinMax)
            {
                RONodePropertiesStockMinMax nodePropertiesStockMinMaxData = (RONodePropertiesStockMinMax)parms.RONodeProperties;
                attributeKey = nodePropertiesStockMinMaxData.Attribute.Key;
                if (nodePropertiesStockMinMaxData.AttributeSetIsSet)
                {
                    attributeSetKey = nodePropertiesStockMinMaxData.AttributeSet.Key;
                }
                else if (nodePropertiesStockMinMaxData.StockMinMaxAttributeSet != null)
                {
                    attributeSetKey = nodePropertiesStockMinMaxData.StockMinMaxAttributeSet.AttributeSet.Key;
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
