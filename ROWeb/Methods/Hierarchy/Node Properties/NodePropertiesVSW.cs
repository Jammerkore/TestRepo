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
    public class NodePropertiesVSW : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        IMOProfileList _VSWList = null;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesVSW(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.IMO)
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
            _VSWList = (IMOProfileList)nodePropertiesData;

            int attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            int attributeSetKey = Include.NoRID;
            if (parms is RONodePropertyAttributeKeyParms)
            {
                RONodePropertyAttributeKeyParms nodePropertyVSWParms = (RONodePropertyAttributeKeyParms)parms;
                if (nodePropertyVSWParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyVSWParms.AttributeKey;
                }
                if (nodePropertyVSWParms.AttributeSetKey != Include.NoRID)
                {
                    attributeSetKey = nodePropertyVSWParms.AttributeSetKey;
                }
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesVSW nodeProperties = new RONodePropertiesVSW(node: node,
                attribute: GetName.GetAttributeName(key: attributeKey)
                );

            // populate modelProperties using Windows\NodeProperties.cs as a reference

            AddAttributeSets(nodeProperties: nodeProperties,
                VSWList: _VSWList,
                attributeSetKey: attributeSetKey);


            return nodeProperties;
        }

        private void AddAttributeSets(RONodePropertiesVSW nodeProperties, IMOProfileList VSWList, int attributeSetKey)
        {
            RONodePropertiesVSWAttributeSet VSWAttributeSet;
            RONodePropertiesVSWStore VSWStore;
            IMOProfile VSWProfile;
            HierarchyNodeProfile hnp;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            ProfileList storeVSWGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(nodeProperties.Attribute.Key, true);

            foreach (StoreGroupLevelListViewProfile sglp in storeVSWGroupLevelList)
            {
                if (sglp.Key != attributeSetKey)
                {
                    continue;
                }

                VSWAttributeSet = new RONodePropertiesVSWAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name));
                foreach (StoreProfile storeProfile in sglp.Stores)
                {
                    VSWStore = new RONodePropertiesVSWStore(store: new KeyValuePair<int, string>(storeProfile.Key, storeProfile.Text));
                    VSWStore.VSWValues.VSWID = storeProfile.IMO_ID;

                    if (VSWList.Contains(storeProfile.Key))
                    {
                        VSWProfile = (IMOProfile)VSWList.FindKey(storeProfile.Key);

                        if (VSWProfile.IMOInheritedFromNodeRID != nodeProperties.Node.Key
                            && VSWProfile.IMOIsInherited
                            && VSWProfile.IMOInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(VSWProfile.IMOInheritedFromNodeRID);
                            VSWStore.VSWInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }
                        
                        VSWStore.VSWValues.FWOSMaxType = VSWProfile.IMOFWOS_MaxType;
                        if (VSWStore.VSWValues.FWOSMaxType == eModifierType.Model)
                        {
                            VSWStore.VSWValues.FWOSMaxModel = new KeyValuePair<int, string>(VSWProfile.IMOFWOS_MaxModelRID, VSWProfile.IMOFWOS_MaxModelName);
                        }
                        else if (VSWStore.VSWValues.FWOSMaxType == eModifierType.Percent)
                        {
                            VSWStore.VSWValues.FWOSMax = VSWProfile.IMOFWOS_Max;
                        }

                        if (VSWProfile.IMOMinShipQty != 0)
                        {
                            VSWStore.VSWValues.MinShipQty = VSWProfile.IMOMinShipQty;
                        }

                        if (VSWProfile.IMOPackQty != Include.PercentPackThresholdDefault)
                        {
                            VSWStore.VSWValues.PackQty = Math.Round(VSWProfile.IMOPackQty * 100, 2);
                        }

                        if (VSWProfile.IMOMaxValue != int.MaxValue)
                        {
                            VSWStore.VSWValues.MaxValue = VSWProfile.IMOMaxValue;
                        }

                        if (VSWProfile.IMOPshToBackStock > 0)
                        {
                            VSWStore.VSWValues.PushToBackStock = VSWProfile.IMOPshToBackStock;
                        }
                    }

                    VSWAttributeSet.Store.Add(VSWStore);
                }

                nodeProperties.VSWAttributeSet = VSWAttributeSet;
                nodeProperties.AttributeSet = VSWAttributeSet.AttributeSet;
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesVSW nodePropertiesVSWData = (RONodePropertiesVSW)nodePropertiesData;

            if (_VSWList == null)
            {
                _VSWList = GetVSW(key: nodePropertiesVSWData.Node.Key);
            }

            if (SetVSW(nodePropertiesVSWData: nodePropertiesVSWData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateVSW(nodeKey: nodePropertiesVSWData.Node.Key, message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }


            return _VSWList;
        }

        private bool SetVSW(RONodePropertiesVSW nodePropertiesVSWData, ref string message)
        {
            IMOProfile vsw;
            bool valueChanged = false;

            //foreach (RONodePropertiesVSWAttributeSet VSWAttributeSet in nodePropertiesVSWData.AttributeSet)
            RONodePropertiesVSWAttributeSet VSWAttributeSet = nodePropertiesVSWData.VSWAttributeSet;
            {
                foreach (RONodePropertiesVSWStore VSWStore in VSWAttributeSet.Store)
                {
                    valueChanged = false;
                    if (_VSWList.Contains(VSWStore.Store.Key))
                    {
                        vsw = (IMOProfile)_VSWList.FindKey(VSWStore.Store.Key);
                    }
                    else
                    {
                        vsw = new IMOProfile(VSWStore.Store.Key);
                        vsw.IMOStoreRID = VSWStore.Store.Key;
                        vsw.IMONodeRID = nodePropertiesVSWData.Node.Key;
                        vsw.IMOChangeType = eChangeType.add;
                        _VSWList.Add(vsw);
                    }

                    // item max
                    if (VSWAttributeSet.VSWValues.MaxValueIsSet)
                    {
                        vsw.IMOMaxValue = Convert.ToInt32(VSWAttributeSet.VSWValues.MaxValue);
                        vsw.IMOIsInherited = false;
                        vsw.IMOInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }
                    else if (VSWStore.VSWValues.MaxValueIsSet)
                    {
                        if (vsw.IMOMaxValue != VSWStore.VSWValues.MaxValue)
                        {
                            vsw.IMOMaxValue = Convert.ToInt32(VSWStore.VSWValues.MaxValue);
                            vsw.IMOIsInherited = false;
                            vsw.IMOInheritedFromNodeRID = Include.NoRID;
                            valueChanged = true;
                        }
                    }
                    else if (vsw.IMOMaxValue != int.MaxValue)  // clear value
                    {
                        vsw.IMOMaxValue = int.MaxValue;
                        valueChanged = true;
                    }

                    // minimum ship quantity
                    if (VSWAttributeSet.VSWValues.MinShipQtyIsSet)
                    {
                        vsw.IMOMinShipQty = Convert.ToInt32(VSWAttributeSet.VSWValues.MinShipQty);
                        vsw.IMOIsInherited = false;
                        vsw.IMOInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }
                    else if (VSWStore.VSWValues.MinShipQtyIsSet)
                    {
                        if (vsw.IMOMinShipQty != VSWStore.VSWValues.MinShipQty)
                        {
                            vsw.IMOMinShipQty = Convert.ToInt32(VSWStore.VSWValues.MinShipQty);
                            vsw.IMOIsInherited = false;
                            vsw.IMOInheritedFromNodeRID = Include.NoRID;
                            valueChanged = true;
                        }
                    }
                    else if (vsw.IMOMinShipQty != 0)  // clear value
                    {
                        vsw.IMOMinShipQty = 0;
                        valueChanged = true;
                    }

                    // FWOS Maximum
                    if (VSWAttributeSet.VSWValues.FWOSMaxIsSet
                        || VSWAttributeSet.VSWValues.FWOSMaxModelIsSet)
                    {
                        if (VSWAttributeSet.VSWValues.FWOSMaxModelIsSet)
                        {
                            vsw.IMOFWOS_MaxType = eModifierType.Model;
                            vsw.IMOFWOS_MaxModelName = VSWAttributeSet.VSWValues.FWOSMaxModel.Value;
                            vsw.IMOFWOS_MaxModelRID = VSWAttributeSet.VSWValues.FWOSMaxModel.Key;
                            vsw.IMOFWOS_Max = int.MaxValue;
                        }
                        else
                        {
                            vsw.IMOFWOS_Max = Convert.ToDouble(VSWAttributeSet.VSWValues.FWOSMax);
                            vsw.IMOFWOS_MaxModelRID = Include.NoRID;
                            vsw.IMOFWOS_MaxModelName = string.Empty;
                            vsw.IMOFWOS_MaxType = eModifierType.Percent;
                        }
                        vsw.IMOIsInherited = false;
                        vsw.IMOInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }
                    else if (VSWStore.VSWValues.FWOSMaxIsSet
                        || VSWStore.VSWValues.FWOSMaxModelIsSet)
                    {
                        if (VSWStore.VSWValues.FWOSMaxModelIsSet)
                        {
                            if (vsw.IMOFWOS_MaxModelRID != VSWStore.VSWValues.FWOSMaxModel.Key)
                            {
                                vsw.IMOFWOS_MaxType = eModifierType.Model;
                                vsw.IMOFWOS_MaxModelName = VSWStore.VSWValues.FWOSMaxModel.Value;
                                vsw.IMOFWOS_MaxModelRID = VSWStore.VSWValues.FWOSMaxModel.Key;
                                vsw.IMOFWOS_Max = int.MaxValue;
                                vsw.IMOIsInherited = false;
                                vsw.IMOInheritedFromNodeRID = Include.NoRID;
                                valueChanged = true;
                            }
                        }
                        else
                        {
                            if (vsw.IMOFWOS_Max != VSWStore.VSWValues.FWOSMax)
                            {
                                vsw.IMOFWOS_Max = Convert.ToDouble(VSWStore.VSWValues.FWOSMax);
                                vsw.IMOFWOS_MaxModelRID = Include.NoRID;
                                vsw.IMOFWOS_MaxModelName = string.Empty;
                                vsw.IMOFWOS_MaxType = eModifierType.Percent;
                                vsw.IMOIsInherited = false;
                                vsw.IMOInheritedFromNodeRID = Include.NoRID;
                                valueChanged = true;
                            }
                        }
                    }
                    else if (vsw.IMOFWOS_MaxType != eModifierType.None)
                    {
                        vsw.IMOFWOS_Max = int.MaxValue;
                        vsw.IMOFWOS_MaxModelRID = Include.NoRID;
                        vsw.IMOFWOS_MaxType = eModifierType.None;
                        valueChanged = true;
                    }

                    // percent pack threshold
                    if (VSWAttributeSet.VSWValues.PackQtyIsSet)
                    {
                        vsw.IMOPackQty = Convert.ToDouble(VSWAttributeSet.VSWValues.PackQty) / 100;
                        vsw.IMOIsInherited = false;
                        vsw.IMOInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }
                    else if (VSWStore.VSWValues.PackQtyIsSet)
                    {
                        double packQty = Math.Round(vsw.IMOPackQty * 100, 2);
                        if (packQty != VSWStore.VSWValues.PackQty)
                        {
                            vsw.IMOPackQty = Convert.ToDouble(VSWStore.VSWValues.PackQty) / 100;
                            vsw.IMOIsInherited = false;
                            vsw.IMOInheritedFromNodeRID = Include.NoRID;
                            valueChanged = true;
                        }
                    }
                    else if (vsw.IMOPackQty != Include.PercentPackThresholdDefault)  // clear value
                    {
                        vsw.IMOPackQty = Include.PercentPackThresholdDefault;
                        valueChanged = true;
                    }

                    // push to backstock
                    if (VSWAttributeSet.VSWValues.PushToBackStockIsSet)
                    {
                        vsw.IMOPshToBackStock = Convert.ToInt32(VSWAttributeSet.VSWValues.PushToBackStock);
                        vsw.IMOIsInherited = false;
                        vsw.IMOInheritedFromNodeRID = Include.NoRID;
                        valueChanged = true;
                    }
                    else if (VSWStore.VSWValues.PushToBackStockIsSet)
                    {
                        if (vsw.IMOPshToBackStock != VSWStore.VSWValues.PushToBackStock)
                        {
                            vsw.IMOPshToBackStock = Convert.ToInt32(VSWStore.VSWValues.PushToBackStock);
                            vsw.IMOIsInherited = false;
                            vsw.IMOInheritedFromNodeRID = Include.NoRID;
                            valueChanged = true;
                        }
                    }
                    else if (vsw.IMOPshToBackStock != Include.NoRID)  // clear value
                    {
                        vsw.IMOPshToBackStock = Include.NoRID;
                        valueChanged = true;
                    }

                    if (vsw.IsDefaultValues)
                    {
                        vsw.IMOChangeType = eChangeType.delete;
                    }
                    else if(valueChanged)
                    {
                        vsw.IMOChangeType = eChangeType.update;
                    }
                }
            }

            return true;
        }

        private bool UpdateVSW(int nodeKey, ref string message)
        {
            SAB.HierarchyServerSession.IMOUpdate(nodeKey, _VSWList, false);

            // set global list to updated values including inheritance for values removed
            _VSWList = GetVSW(key: nodeKey);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_VSWList == null)
            {
                _VSWList = GetVSW(key: key);
            }

            foreach (IMOProfile vsw in _VSWList)
            {
                vsw.IMOChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.IMOUpdate(key, _VSWList, false);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteIMO: true);
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
            return GetVSW(key: parms.Key);
        }

        private IMOProfileList GetVSW(int key)
        {
            return SAB.HierarchyServerSession.GetNodeIMOList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), aNodeRID: key);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyVSW, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyVSW, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            int attributeSetKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesVSW)
            {
                RONodePropertiesVSW nodePropertiesVSWData = (RONodePropertiesVSW)parms.RONodeProperties;
                attributeKey = nodePropertiesVSWData.Attribute.Key;
                if (nodePropertiesVSWData.AttributeSetIsSet)
                {
                    attributeSetKey = nodePropertiesVSWData.AttributeSet.Key;
                }
                else if (nodePropertiesVSWData.VSWAttributeSet != null)
                {
                    attributeSetKey = nodePropertiesVSWData.VSWAttributeSet.AttributeSet.Key;
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
