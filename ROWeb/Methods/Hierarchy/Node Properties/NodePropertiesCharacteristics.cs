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
    public class NodePropertiesCharacteristics : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        NodeCharProfileList _nodeCharProfileList;  
		      
        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesCharacteristics(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.ProductCharacteristic)
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
            _nodeCharProfileList = (NodeCharProfileList)nodePropertiesData;

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesCharacteristics nodeProperties = new RONodePropertiesCharacteristics(node: node);

            // populate nodeProperties using Windows\NodeProperties.cs as a reference
            AddCharacteristics(nodeProperties: nodeProperties, nodeCharProfileList: _nodeCharProfileList, message: ref message);

            return nodeProperties;
        }

        private void AddCharacteristics(RONodePropertiesCharacteristics nodeProperties, NodeCharProfileList nodeCharProfileList, ref string message)
        {
            RONodePropertiesCharacteristicsValue characteristicValue = null;
            HierarchyNodeProfile hnp = null;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);
            SortedList characteristicsSL = new SortedList();

            ProductCharProfileList productCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics();
            foreach (ProductCharProfile pcp in productCharProfileList)
            {
                characteristicsSL.Add(pcp.ProductCharID, pcp);
            }

            foreach (ProductCharProfile pcp in characteristicsSL.Values)
            {
                characteristicValue = new RONodePropertiesCharacteristicsValue(characteristic: new KeyValuePair<int, string>(pcp.Key, pcp.ProductCharID));

                NodeCharProfile ncp = (NodeCharProfile)_nodeCharProfileList.FindKey(pcp.Key);
                if (ncp != null &&
                    ncp.ProductCharValueRID != Include.NoRID)
                {
                    characteristicValue.CharacteristicValue = new KeyValuePair<int, string>(ncp.ProductCharValueRID, ncp.ProductCharValue);

                    if (ncp.TypeInherited != eInheritedFrom.None)
                    {
                        if (hnp == null || hnp.Key != ncp.InheritedFrom)
                        {
                            hnp = GetHierarchyNodeProfile(key: ncp.InheritedFrom);
                        }
                        characteristicValue.CharacteristicInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text); 
                    }
                }

                foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
                {
                    characteristicValue.CharacteristicValues.Add(new KeyValuePair<int, string>(pcvp.Key, pcvp.ProductCharValue));
                }

                nodeProperties.Characteristics.Add(characteristicValue);
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            RONodePropertiesCharacteristics nodePropertiesCharacteristicsData = (RONodePropertiesCharacteristics)nodePropertiesData;

            if (_nodeCharProfileList == null)
            {
                _nodeCharProfileList = GetCharacteristics(key: nodePropertiesCharacteristicsData.Node.Key);
            }

            if (SetCharacteristics(nodePropertiesCharacteristicsData: nodePropertiesCharacteristicsData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateCharacteristics(message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }

            return _nodeCharProfileList;
        }


        /// <summary>
        /// Takes values from input class and updates the store grade memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the store grade</param>
        /// <param name="message">The message</param>
        private bool SetCharacteristics(RONodePropertiesCharacteristics nodePropertiesCharacteristicsData, ref string message)
        {
            NodeCharProfile ncp;

            foreach (RONodePropertiesCharacteristicsValue characteristicValue in nodePropertiesCharacteristicsData.Characteristics)
            {
                ncp = (NodeCharProfile)_nodeCharProfileList.FindKey(characteristicValue.Characteristic.Key);
                if (ncp == null
                    && characteristicValue.CharacteristicValueIsSet)
                {
                    ncp = new NodeCharProfile(characteristicValue.Characteristic.Key);
                    ncp.ProductCharValueRID = characteristicValue.CharacteristicValue.Key;
                    ncp.ProductCharValue = characteristicValue.CharacteristicValue.Value;
                    ncp.ProductCharChangeType = eChangeType.add;
                    _nodeCharProfileList.Add(ncp);
                }
                else if (ncp != null)
                {
                    if (!characteristicValue.CharacteristicValueIsSet)
                    {
                        ncp.ProductCharValueRID = Include.NoRID;
                        ncp.ProductCharValue = string.Empty;
                        ncp.ProductCharChangeType = eChangeType.delete;
                        ncp.TypeInherited = eInheritedFrom.None;
                        ncp.InheritedFrom = Include.NoRID;
                        _nodeCharProfileList.Update(ncp);
                    }
                    else if (characteristicValue.CharacteristicValue.Key != ncp.ProductCharValueRID)
                    {
                        ncp.ProductCharValueRID = characteristicValue.CharacteristicValue.Key;
                        ncp.ProductCharValue = characteristicValue.CharacteristicValue.Value;
                        ncp.ProductCharChangeType = eChangeType.update;
                        ncp.TypeInherited = eInheritedFrom.None;
                        ncp.InheritedFrom = Include.NoRID;
                        _nodeCharProfileList.Update(ncp);
                    }
                }
            }

            return true;
        }

        private bool UpdateCharacteristics(ref string message)
        {
            // make copy of list with only updates
            NodeCharProfileList nodeCharProfileList = new NodeCharProfileList(eProfileType.ProductCharacteristic);
            foreach (NodeCharProfile ncp in _nodeCharProfileList)
            {
                if (ncp.ProductCharChangeType != eChangeType.none)
                {
                    nodeCharProfileList.Add(ncp);
                }
            }
            SAB.HierarchyServerSession.UpdateProductCharacteristics(HierarchyNodeProfile.Key, nodeCharProfileList);

            // read to get inheritance
            _nodeCharProfileList = GetCharacteristics(key: HierarchyNodeProfile.Key);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_nodeCharProfileList == null)
            {
                _nodeCharProfileList = GetCharacteristics(key: key);
            }

            foreach (NodeCharProfile ncp in _nodeCharProfileList)
            {
                ncp.ProductCharChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.UpdateProductCharacteristics(HierarchyNodeProfile.Key, _nodeCharProfileList);

            // read to get inheritance
            _nodeCharProfileList = GetCharacteristics(key: key);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteCharacteristics: true);
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
            return GetCharacteristics(key: parms.Key);
        }

        private NodeCharProfileList GetCharacteristics(int key)
        {
            return SAB.HierarchyServerSession.GetProductCharacteristics(aNodeRID: key, aChaseHierarchy: true); 
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyCharacteristic, (int)eSecurityTypes.All);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyCharacteristic, (int)eSecurityTypes.All);
            }
        }
    }
}
