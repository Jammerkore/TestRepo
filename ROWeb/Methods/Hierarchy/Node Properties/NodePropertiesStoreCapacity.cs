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
    public class NodePropertiesStoreCapacity : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        StoreCapacityList _storeCapacityList = null;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesStoreCapacity(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.StoreCapacity)
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
            _storeCapacityList = (StoreCapacityList)nodePropertiesData;

            int attributeKey = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            int attributeSetKey = Include.NoRID;
            if (parms is RONodePropertyAttributeKeyParms)
            {
                RONodePropertyAttributeKeyParms nodePropertyStoreCapacityParms = (RONodePropertyAttributeKeyParms)parms;
                if (nodePropertyStoreCapacityParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyStoreCapacityParms.AttributeKey;
                    attributeSetKey = nodePropertyStoreCapacityParms.AttributeSetKey;
                }
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesStoreCapacity nodeProperties = new RONodePropertiesStoreCapacity(node: node,
                attribute: GetName.GetAttributeName(key: attributeKey)
                );

            // populate modelProperties using Windows\NodeProperties.cs as a reference

            AddAttributeSets(nodeProperties: nodeProperties,
                storeCapacityList: _storeCapacityList,
                attributeSetKey: attributeSetKey);


            return nodeProperties;
        }

        private void AddAttributeSets(RONodePropertiesStoreCapacity nodeProperties, StoreCapacityList storeCapacityList, int attributeSetKey)
        {
            RONodePropertiesStoreCapacityAttributeSet storeCapacityAttributeSet;
            RONodePropertiesStoreCapacityStore storeCapacityStore;
            StoreCapacityProfile storeCapacityProfile;
            HierarchyNodeProfile hnp;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            ProfileList storeStoreCapacityGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(nodeProperties.Attribute.Key, true);

            foreach (StoreGroupLevelListViewProfile sglp in storeStoreCapacityGroupLevelList)
            {
                if (sglp.Key != attributeSetKey)
                {
                    continue;
                }

                storeCapacityAttributeSet = new RONodePropertiesStoreCapacityAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name));
                foreach (StoreProfile storeProfile in sglp.Stores)
                {
                    storeCapacityStore = new RONodePropertiesStoreCapacityStore(store: new KeyValuePair<int, string>(storeProfile.Key, storeProfile.Text));

                    if (storeCapacityList.Contains(storeProfile.Key))
                    {
                        storeCapacityProfile = (StoreCapacityProfile)storeCapacityList.FindKey(storeProfile.Key);

                        if (storeCapacityProfile.StoreCapacityInheritedFromNodeRID != nodeProperties.Node.Key
                            && storeCapacityProfile.StoreCapacityIsInherited
                            && storeCapacityProfile.StoreCapacityInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(storeCapacityProfile.StoreCapacityInheritedFromNodeRID);
                            storeCapacityStore.StoreCapacityInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        if (storeCapacityProfile.StoreCapacity > Include.Undefined)
                        {
                            storeCapacityStore.StoreCapacityValues.Capacity = storeCapacityProfile.StoreCapacity;
                        }
                    }

                    storeCapacityAttributeSet.Store.Add(storeCapacityStore);
                }

                nodeProperties.StoreCapacityAttributeSet = storeCapacityAttributeSet;
                nodeProperties.AttributeSet = storeCapacityAttributeSet.AttributeSet;
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesStoreCapacity nodePropertiesStoreCapacityData = (RONodePropertiesStoreCapacity)nodePropertiesData;

            if (_storeCapacityList == null)
            {
                _storeCapacityList = GetStoreCapacity(key: nodePropertiesStoreCapacityData.Node.Key);
            }

            if (SetStoreCapacity(nodePropertiesStoreCapacityData: nodePropertiesStoreCapacityData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateStoreCapacity(nodeKey: nodePropertiesStoreCapacityData.Node.Key, message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }


            return _storeCapacityList;
        }

        private bool SetStoreCapacity(RONodePropertiesStoreCapacity nodePropertiesStoreCapacityData, ref string message)
        {
            StoreCapacityProfile storeCapacity;
            bool valueChanged = false;

            //foreach (RONodePropertiesStoreCapacityAttributeSet storeCapacityAttributeSet in nodePropertiesStoreCapacityData.AttributeSet)
            RONodePropertiesStoreCapacityAttributeSet storeCapacityAttributeSet = nodePropertiesStoreCapacityData.StoreCapacityAttributeSet;
            {
                foreach (RONodePropertiesStoreCapacityStore storeCapacityStore in storeCapacityAttributeSet.Store)
                {
                    valueChanged = false;
                    if (_storeCapacityList.Contains(storeCapacityStore.Store.Key))
                    {
                        storeCapacity = (StoreCapacityProfile)_storeCapacityList.FindKey(storeCapacityStore.Store.Key);
                    }
                    else
                    {
                        storeCapacity = new StoreCapacityProfile(storeCapacityStore.Store.Key);
                        storeCapacity.StoreCapacityChangeType = eChangeType.add;
                        storeCapacity.StoreCapacityIsInherited = false;
                        _storeCapacityList.Add(storeCapacity);
                    }

                    // store capacity
                    if (storeCapacityAttributeSet.StoreCapacityValues.CapacityIsSet)
                    {
                        storeCapacity.StoreCapacity = Convert.ToInt32(storeCapacityAttributeSet.StoreCapacityValues.Capacity);
                        storeCapacity.StoreCapacityIsInherited = false;
                        storeCapacity.StoreCapacityInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }
                    else if (storeCapacityStore.StoreCapacityValues.CapacityIsSet)
                    {
                        if (storeCapacity.StoreCapacity != storeCapacityStore.StoreCapacityValues.Capacity)
                        {
                            storeCapacity.StoreCapacity = Convert.ToInt32(storeCapacityStore.StoreCapacityValues.Capacity);
                            storeCapacity.StoreCapacityIsInherited = false;
                            storeCapacity.StoreCapacityInheritedFromNodeRID = Include.NoRID;
                            valueChanged = true;
                        }
                    }
                    else if (storeCapacity.StoreCapacity > Include.Undefined)  // clear value
                    {
                        storeCapacity.StoreCapacity = Include.Undefined;
                        storeCapacity.StoreCapacityIsInherited = false;
                        storeCapacity.StoreCapacityInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }

                    if (storeCapacity.StoreCapacity == Include.Undefined
                        && valueChanged)
                    {
                        storeCapacity.StoreCapacityChangeType = eChangeType.delete;
                    }
                    else if (valueChanged
                        && storeCapacity.StoreCapacityChangeType == eChangeType.none)
                    {
                        storeCapacity.StoreCapacityChangeType = eChangeType.update;
                    }
                }
            }

            return true;
        }

        private bool UpdateStoreCapacity(int nodeKey, ref string message)
        {
            SAB.HierarchyServerSession.StoreCapacityUpdate(nodeKey, _storeCapacityList, false);

            // set global list to updated values including inheritance for values removed
            _storeCapacityList = GetStoreCapacity(key: nodeKey);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_storeCapacityList == null)
            {
                _storeCapacityList = GetStoreCapacity(key: key);
            }

            foreach (StoreCapacityProfile StoreCapacity in _storeCapacityList)
            {
                StoreCapacity.StoreCapacityChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.StoreCapacityUpdate(key, _storeCapacityList, false);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteStoreCapacity: true);
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
            return GetStoreCapacity(key: parms.Key);
        }

        private StoreCapacityList GetStoreCapacity(int key)
        {
            return SAB.HierarchyServerSession.GetStoreCapacityList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), nodeRID: key, stopOnFind: false, forCopy: false);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyCapacity, (int)eSecurityTypes.Allocation);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyCapacity, (int)eSecurityTypes.Allocation);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            int attributeSetKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesStoreCapacity)
            {
                RONodePropertiesStoreCapacity nodePropertiesStoreCapacityData = (RONodePropertiesStoreCapacity)parms.RONodeProperties;
                attributeKey = nodePropertiesStoreCapacityData.Attribute.Key;
                if (nodePropertiesStoreCapacityData.AttributeSetIsSet)
                {
                    attributeSetKey = nodePropertiesStoreCapacityData.AttributeSet.Key;
                }
                else if (nodePropertiesStoreCapacityData.StoreCapacityAttributeSet != null)
                {
                    attributeSetKey = nodePropertiesStoreCapacityData.StoreCapacityAttributeSet.AttributeSet.Key;
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
