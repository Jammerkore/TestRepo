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
    public class NodePropertiesEligibility : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        StoreEligibilityList _storeEligList = null;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesEligibility(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.StoreEligibility)
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
            _storeEligList = (StoreEligibilityList)nodePropertiesData;

            // populate modelProperties using Windows\NodeProperties.cs as a reference

            int attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            int attributeSetKey = Include.NoRID;
            if (parms is RONodePropertyAttributeKeyParms)
            {
                RONodePropertyAttributeKeyParms nodePropertyEligibilityParms = (RONodePropertyAttributeKeyParms)parms;
                if (nodePropertyEligibilityParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyEligibilityParms.AttributeKey;
                    attributeSetKey = nodePropertyEligibilityParms.AttributeSetKey;
                }
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesEligibility nodeProperties = new RONodePropertiesEligibility(node: node,
                attribute: GetName.GetAttributeName(key: attributeKey)
                );

            AddAttributeSets(nodeProperties: nodeProperties,
                storeEligList: _storeEligList,
                attributeSetKey: attributeSetKey);

            return nodeProperties;
        }

        private void AddAttributeSets(RONodePropertiesEligibility nodeProperties, StoreEligibilityList storeEligList, int attributeSetKey)
        {
            RONodePropertiesEligibilityAttributeSet eligibilityAttributeSet;
            RONodePropertiesEligibilityStore eligibilityStore;
            List<KeyValuePair<int, string>> similarStores;
            StoreEligibilityProfile sep;
            HierarchyNodeProfile hnp;
            StoreProfile sp;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            ProfileList storeEligibilityGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(nodeProperties.Attribute.Key, true);
            if (attributeSetKey == Include.NoRID)
            {
                attributeSetKey = storeEligibilityGroupLevelList[0].Key;
            }

            foreach (StoreGroupLevelListViewProfile sglp in storeEligibilityGroupLevelList)
            {
                if (sglp.Key != attributeSetKey)
                {
                    continue;
                }
                eligibilityAttributeSet = new RONodePropertiesEligibilityAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name));
                similarStores = new List<KeyValuePair<int, string>>();
                foreach (StoreProfile storeProfile in sglp.Stores)
                {
                    eligibilityStore = new RONodePropertiesEligibilityStore(store: new KeyValuePair<int, string>(storeProfile.Key, storeProfile.Text));
                    if (storeEligList.Contains(storeProfile.Key))
                    {
                        sep = (StoreEligibilityProfile)storeEligList.FindKey(storeProfile.Key);

                        // eligibility
                        
                        if (sep.StoreIneligible)
                        {
                            eligibilityStore.EligibilityValues.StoreIneligible = sep.StoreIneligible;
                        }

                        if (sep.EligModelRID != Include.NoRID)
                        {
                            eligibilityStore.EligibilityValues.EligibilityModel = new KeyValuePair<int, string>(sep.EligModelRID, sep.EligModelName);
                        }

                        if (sep.EligInheritedFromNodeRID != nodeProperties.Node.Key
                            && sep.EligIsInherited
                            && sep.EligInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.EligInheritedFromNodeRID);
                            eligibilityStore.EligibilityValues.EligibilityInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        //stock modifier
                        switch (sep.StkModType)
                        {
                            case eModifierType.Model:
                                eligibilityStore.EligibilityValues.StockModifierModel = new KeyValuePair<int, string>(sep.StkModModelRID, sep.StkModModelName);
                                break;
                            case eModifierType.Percent:
                                eligibilityStore.EligibilityValues.StockModifierPct = sep.StkModPct;
                                break;
                            default:
                                break;
                        }

                        if (sep.StkModInheritedFromNodeRID != nodeProperties.Node.Key
                            && sep.StkModIsInherited
                            && sep.StkModInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.StkModInheritedFromNodeRID);
                            eligibilityStore.EligibilityValues.StockModifierInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        //sales modifier
                        switch (sep.SlsModType)
                        {
                            case eModifierType.Model:
                                eligibilityStore.EligibilityValues.SalesModifierModel = new KeyValuePair<int, string>(sep.SlsModModelRID, sep.SlsModModelName);
                                break;
                            case eModifierType.Percent:
                                eligibilityStore.EligibilityValues.SalesModifierPct = sep.SlsModPct;
                                break;
                            default:
                                break;
                        }

                        if (sep.SlsModInheritedFromNodeRID != nodeProperties.Node.Key
                            && sep.SlsModIsInherited
                            && sep.SlsModInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.SlsModInheritedFromNodeRID);
                            eligibilityStore.EligibilityValues.SalesModifierInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        //FWOS modifier
                        switch (sep.FWOSModType)
                        {
                            case eModifierType.Model:
                                eligibilityStore.EligibilityValues.FWOSModifierModel = new KeyValuePair<int, string>(sep.FWOSModModelRID, sep.FWOSModModelName);
                                break;
                            case eModifierType.Percent:
                                eligibilityStore.EligibilityValues.FWOSModifierPct = sep.FWOSModPct;
                                break;
                            default:
                                break;
                        }

                        if (sep.FWOSModInheritedFromNodeRID != nodeProperties.Node.Key
                            && sep.FWOSModIsInherited
                            && sep.FWOSModInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.FWOSModInheritedFromNodeRID);
                            eligibilityStore.EligibilityValues.FWOSModifierInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        // similar store
                        switch (sep.SimStoreType)
                        {
                            case eSimilarStoreType.Stores:
                                if (sep.SimStores != null &&
                                    sep.SimStores.Count > 0)
                                {
                                    foreach (int storeKey in sep.SimStores)
                                    {
                                        sp = StoreMgmt.StoreProfile_Get(storeKey);
                                        if (sp.ActiveInd)
                                        {
                                            eligibilityStore.EligibilityValues.SimilarStores.Add(new KeyValuePair<int, string>(sp.Key, sp.Text));
                                        }
                                    }
                                }
                                if (eligibilityStore.EligibilityValues.SimilarStores.Count > 0)
                                {
                                    eligibilityStore.EligibilityValues.SimilarStoreRatio = sep.SimStoreRatio;
                                    eligibilityStore.EligibilityValues.SimilarStoreUntilDateRange = new KeyValuePair<int, string>(sep.SimStoreUntilDateRangeRID, sep.SimStoreDisplayDate);
                                }
                                break;
                            default:
                                break;
                        }

                        if (sep.SimStoreInheritedFromNodeRID != nodeProperties.Node.Key
                            && sep.SimStoreIsInherited
                            && sep.SimStoreInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.SimStoreInheritedFromNodeRID);
                            eligibilityStore.EligibilityValues.SimilarStoreInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        // presentation plus sales
                        eligibilityStore.EligibilityValues.PresentationPlusSalesInd = sep.PresPlusSalesInd;
   
                        if (sep.PresPlusSalesInheritedFromNodeRID != nodeProperties.Node.Key
                            && sep.PresPlusSalesIsInherited
                            && sep.PresPlusSalesInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.PresPlusSalesInheritedFromNodeRID);
                            eligibilityStore.EligibilityValues.PresentationPlusSalesInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        // stock lead weeks
                        eligibilityStore.EligibilityValues.StockLeadWeeks = sep.StkLeadWeeks;

                        if (sep.StkLeadWeeksInheritedRid != nodeProperties.Node.Key
                            && sep.StkLeadWeeksInherited
                            && sep.StkLeadWeeksInheritedRid != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(sep.StkLeadWeeksInheritedRid);
                            eligibilityStore.EligibilityValues.StockLeadWeeksInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }
                    }
                    eligibilityAttributeSet.EligibilityStore.Add(eligibilityStore);
                }

                //nodeProperties.EligibilityAttributeSet.Add(eligibilityAttributeSet);
				nodeProperties.EligibilityAttributeSet = eligibilityAttributeSet;
                nodeProperties.AttributeSet = eligibilityAttributeSet.AttributeSet;
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesEligibility nodePropertiesEligibilityData = (RONodePropertiesEligibility)nodePropertiesData;

            if (_storeEligList == null)
            {
                _storeEligList = GetEligibility(key: nodePropertiesEligibilityData.Node.Key);
            }

            if (SetEligibility(nodePropertiesEligibilityData: nodePropertiesEligibilityData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateEligibility(nodeKey: nodePropertiesEligibilityData.Node.Key, message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }


            return _storeEligList;
        }

        /// <summary>
        /// Takes values from input class and updates the store eligibility memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the store eligibility</param>
        /// <param name="message">The message</param>
        private bool SetEligibility(RONodePropertiesEligibility nodePropertiesEligibilityData, ref string message)
        {
            StoreEligibilityProfile sep;

            //foreach (RONodePropertiesEligibilityAttributeSet eligibilityAttributeSet in nodePropertiesEligibilityData.EligibilityAttributeSet)
            RONodePropertiesEligibilityAttributeSet eligibilityAttributeSet = nodePropertiesEligibilityData.EligibilityAttributeSet;
            {
                foreach (RONodePropertiesEligibilityStore eligibilityStore in eligibilityAttributeSet.EligibilityStore)
                {
                    if (_storeEligList.Contains(eligibilityStore.Store.Key))
                    {
                        sep = (StoreEligibilityProfile)_storeEligList.FindKey(eligibilityStore.Store.Key);
                    }
                    else
                    {
                        sep = new StoreEligibilityProfile(eligibilityStore.Store.Key);
                        _storeEligList.Add(sep);
                    }

                    // eligibility
                    if (eligibilityAttributeSet.EligibilityValues.EligibilityType != eEligibilitySettingType.None)
                    {
                        sep.EligType = eligibilityAttributeSet.EligibilityValues.EligibilityType;
                        sep.EligIsInherited = false;
                        sep.EligInheritedFromNodeRID = Include.NoRID;
                        switch (eligibilityAttributeSet.EligibilityValues.EligibilityType)
                        {
                            case eEligibilitySettingType.Model:
                                if (sep.EligModelRID != eligibilityAttributeSet.EligibilityValues.EligibilityModel.Key)
                                {
                                    sep.EligModelRID = eligibilityAttributeSet.EligibilityValues.EligibilityModel.Key;
                                    sep.EligModelName = eligibilityAttributeSet.EligibilityValues.EligibilityModel.Value;
                                    sep.EligIsInherited = false;
                                    sep.EligInheritedFromNodeRID = Include.NoRID;
                                    sep.StoreIneligible = false;
                                }
                                break;
                            case eEligibilitySettingType.SetEligible:
                            case eEligibilitySettingType.SetIneligible:
                                if (sep.StoreIneligible != eligibilityAttributeSet.EligibilityValues.StoreIneligible)
                                {
                                    sep.StoreIneligible = Convert.ToBoolean(eligibilityAttributeSet.EligibilityValues.StoreIneligible);
                                    sep.EligIsInherited = false;
                                    sep.EligInheritedFromNodeRID = Include.NoRID;
                                    sep.EligModelRID = Include.NoRID;
                                    sep.EligModelName = string.Empty;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (eligibilityStore.EligibilityValues.EligibilityType != eEligibilitySettingType.None)
                    {
                        sep.EligType = eligibilityStore.EligibilityValues.EligibilityType;
                        switch (eligibilityStore.EligibilityValues.EligibilityType)
                        {
                            case eEligibilitySettingType.Model:
                                if (sep.EligModelRID != eligibilityStore.EligibilityValues.EligibilityModel.Key)
                                {
                                    sep.EligModelRID = eligibilityStore.EligibilityValues.EligibilityModel.Key;
                                    sep.EligModelName = eligibilityStore.EligibilityValues.EligibilityModel.Value;
                                    sep.EligIsInherited = false;
                                    sep.EligInheritedFromNodeRID = Include.NoRID;
                                    if (eligibilityStore.EligibilityValues.StoreIneligible.HasValue)
                                    {
                                        sep.StoreIneligible = Convert.ToBoolean(eligibilityStore.EligibilityValues.StoreIneligible);
                                    }
                                    else
                                    {
                                        sep.StoreIneligible = false;
                                    }
                                }
                                break;
                            case eEligibilitySettingType.SetEligible:
                            case eEligibilitySettingType.SetIneligible:
                                if (eligibilityStore.EligibilityValues.EligibilityModelIsSet)
                                {
                                    if (!eligibilityStore.EligibilityValues.EligibilityIsInherited
                                        && eligibilityStore.EligibilityValues.StoreIneligible.HasValue
                                        && !Convert.ToBoolean(eligibilityStore.EligibilityValues.StoreIneligible)
                                        )
                                    {
                                        sep.EligModelRID = eligibilityStore.EligibilityValues.EligibilityModel.Key;
                                        sep.EligModelName = eligibilityStore.EligibilityValues.EligibilityModel.Value;
                                        sep.EligType = eEligibilitySettingType.Model;
                                        sep.EligIsInherited = false;
                                        sep.EligInheritedFromNodeRID = Include.NoRID;
                                    }
                                }
                                else
                                {
                                    sep.EligModelRID = Include.NoRID;
                                    sep.EligModelName = string.Empty;
                                }
                                if (sep.StoreIneligible != eligibilityStore.EligibilityValues.StoreIneligible)
                                {
                                    sep.StoreIneligible = Convert.ToBoolean(eligibilityStore.EligibilityValues.StoreIneligible);
                                    sep.EligIsInherited = false;
                                    sep.EligInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (sep.EligType != eEligibilitySettingType.None)  // clear value
                    {
                        sep.EligType = eEligibilitySettingType.None;
                        sep.EligModelRID = Include.NoRID;
                        sep.StoreIneligible = false;
                        sep.EligIsInherited = false;
                        sep.EligInheritedFromNodeRID = Include.NoRID;
                    }

                    //stock modifier
                    if (eligibilityAttributeSet.EligibilityValues.StockModifierType != eModifierType.None)
                    {
                        sep.StkModType = eligibilityAttributeSet.EligibilityValues.StockModifierType;
                        sep.StkModIsInherited = false;
                        sep.StkModInheritedFromNodeRID = Include.NoRID;
                        switch (eligibilityAttributeSet.EligibilityValues.StockModifierType)
                        {
                            case eModifierType.Model:
                                if (sep.StkModModelRID != eligibilityAttributeSet.EligibilityValues.StockModifierModel.Key)
                                {
                                    sep.StkModModelRID = eligibilityAttributeSet.EligibilityValues.StockModifierModel.Key;
                                    sep.StkModModelName = eligibilityAttributeSet.EligibilityValues.StockModifierModel.Value;
                                    sep.StkModIsInherited = false;
                                    sep.StkModInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            case eModifierType.Percent:
                                if (sep.StkModPct != eligibilityAttributeSet.EligibilityValues.StockModifierPct)
                                {
                                    sep.StkModPct = Convert.ToDouble(eligibilityAttributeSet.EligibilityValues.StockModifierPct);
                                    sep.StkModIsInherited = false;
                                    sep.StkModInheritedFromNodeRID = Include.NoRID;
                                    sep.StkModModelRID = Include.NoRID;
                                    sep.StkModModelName = string.Empty;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (eligibilityStore.EligibilityValues.StockModifierType != eModifierType.None)
                    {
                        sep.StkModType = eligibilityStore.EligibilityValues.StockModifierType;
                        switch (eligibilityStore.EligibilityValues.StockModifierType)
                        {
                            case eModifierType.Model:
                                if (sep.StkModModelRID != eligibilityStore.EligibilityValues.StockModifierModel.Key)
                                {
                                    sep.StkModModelRID = eligibilityStore.EligibilityValues.StockModifierModel.Key;
                                    sep.StkModModelName = eligibilityStore.EligibilityValues.StockModifierModel.Value;
                                    sep.StkModIsInherited = false;
                                    sep.StkModInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            case eModifierType.Percent:
                                if (sep.StkModPct != eligibilityStore.EligibilityValues.StockModifierPct)
                                {
                                    sep.StkModPct = Convert.ToDouble(eligibilityStore.EligibilityValues.StockModifierPct);
                                    sep.StkModIsInherited = false;
                                    sep.StkModInheritedFromNodeRID = Include.NoRID;
                                    sep.StkModModelRID = Include.NoRID;
                                    sep.StkModModelName = string.Empty;
                                }
                                break;
                            default:
                                sep.StkModPct = 0;
                                break;
                        }
                    }
                    else if (sep.StkModType != eModifierType.None)  // clear value
                    {
                        sep.StkModType = eModifierType.None;
                        sep.StkModModelRID = Include.NoRID;
                        sep.StkModModelName = null;
                        sep.StkModPct = 0;
                        sep.StkModIsInherited = false;
                        sep.StkModInheritedFromNodeRID = Include.NoRID;
                    }

                    //sales modifier
                    if (eligibilityAttributeSet.EligibilityValues.SalesModifierType != eModifierType.None)
                    {
                        sep.SlsModType = eligibilityAttributeSet.EligibilityValues.SalesModifierType;
                        sep.SlsModIsInherited = false;
                        sep.SlsModInheritedFromNodeRID = Include.NoRID;
                        switch (eligibilityAttributeSet.EligibilityValues.SalesModifierType)
                        {
                            case eModifierType.Model:
                                if (sep.SlsModModelRID != eligibilityAttributeSet.EligibilityValues.SalesModifierModel.Key)
                                {
                                    sep.SlsModModelRID = eligibilityAttributeSet.EligibilityValues.SalesModifierModel.Key;
                                    sep.SlsModModelName = eligibilityAttributeSet.EligibilityValues.SalesModifierModel.Value;
                                    sep.SlsModIsInherited = false;
                                    sep.SlsModInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            case eModifierType.Percent:
                                if (sep.SlsModPct != eligibilityAttributeSet.EligibilityValues.SalesModifierPct)
                                {
                                    sep.SlsModPct = Convert.ToDouble(eligibilityAttributeSet.EligibilityValues.SalesModifierPct);
                                    sep.SlsModIsInherited = false;
                                    sep.SlsModInheritedFromNodeRID = Include.NoRID;
                                    sep.SlsModModelRID = Include.NoRID;
                                    sep.SlsModModelName = string.Empty;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (eligibilityStore.EligibilityValues.SalesModifierType != eModifierType.None)
                    {
                        sep.SlsModType = eligibilityStore.EligibilityValues.SalesModifierType;
                        switch (eligibilityStore.EligibilityValues.SalesModifierType)
                        {
                            case eModifierType.Model:
                                if (sep.SlsModModelRID != eligibilityStore.EligibilityValues.SalesModifierModel.Key)
                                {
                                    sep.SlsModModelRID = eligibilityStore.EligibilityValues.SalesModifierModel.Key;
                                    sep.SlsModModelName = eligibilityStore.EligibilityValues.SalesModifierModel.Value;
                                    sep.SlsModIsInherited = false;
                                    sep.SlsModInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            case eModifierType.Percent:
                                if (sep.SlsModPct != eligibilityStore.EligibilityValues.SalesModifierPct)
                                {
                                    sep.SlsModPct = Convert.ToDouble(eligibilityStore.EligibilityValues.SalesModifierPct);
                                    sep.SlsModIsInherited = false;
                                    sep.SlsModInheritedFromNodeRID = Include.NoRID;
                                    sep.SlsModModelRID = Include.NoRID;
                                    sep.SlsModModelName = string.Empty;
                                }
                                break;
                            default:
                                sep.SlsModPct = 0;
                                break;
                        }
                    }
                    else if (sep.SlsModType != eModifierType.None)  // clear value
                    {
                        sep.SlsModType = eModifierType.None;
                        sep.SlsModModelRID = Include.NoRID;
                        sep.SlsModModelName = null;
                        sep.SlsModPct = 0;
                        sep.SlsModIsInherited = false;
                        sep.SlsModInheritedFromNodeRID = Include.NoRID;
                    }

                    //FWOS modifier
                    if (eligibilityAttributeSet.EligibilityValues.FWOSModifierType != eModifierType.None)
                    {
                        sep.FWOSModType = eligibilityAttributeSet.EligibilityValues.FWOSModifierType;
                        sep.FWOSModIsInherited = false;
                        sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                        switch (eligibilityAttributeSet.EligibilityValues.FWOSModifierType)
                        {
                            case eModifierType.Model:
                                if (sep.FWOSModModelRID != eligibilityAttributeSet.EligibilityValues.FWOSModifierModel.Key)
                                {
                                    sep.FWOSModModelRID = eligibilityAttributeSet.EligibilityValues.FWOSModifierModel.Key;
                                    sep.FWOSModModelName = eligibilityAttributeSet.EligibilityValues.FWOSModifierModel.Value;
                                    sep.FWOSModIsInherited = false;
                                    sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            case eModifierType.Percent:
                                if (sep.FWOSModPct != eligibilityAttributeSet.EligibilityValues.FWOSModifierPct)
                                {
                                    sep.FWOSModPct = Convert.ToDouble(eligibilityAttributeSet.EligibilityValues.FWOSModifierPct);
                                    sep.FWOSModIsInherited = false;
                                    sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                                    sep.FWOSModModelRID = Include.NoRID;
                                    sep.FWOSModModelName = string.Empty;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (eligibilityStore.EligibilityValues.FWOSModifierType != eModifierType.None)
                    {
                        sep.FWOSModType = eligibilityStore.EligibilityValues.FWOSModifierType;
                        switch (eligibilityStore.EligibilityValues.FWOSModifierType)
                        {
                            case eModifierType.Model:
                                if (sep.FWOSModModelRID != eligibilityStore.EligibilityValues.FWOSModifierModel.Key)
                                {
                                    sep.FWOSModModelRID = eligibilityStore.EligibilityValues.FWOSModifierModel.Key;
                                    sep.FWOSModModelName = eligibilityStore.EligibilityValues.FWOSModifierModel.Value;
                                    sep.FWOSModIsInherited = false;
                                    sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                                }
                                break;
                            case eModifierType.Percent:
                                if (sep.FWOSModPct != eligibilityStore.EligibilityValues.FWOSModifierPct)
                                {
                                    sep.FWOSModPct = Convert.ToDouble(eligibilityStore.EligibilityValues.FWOSModifierPct);
                                    sep.FWOSModIsInherited = false;
                                    sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                                    sep.FWOSModModelRID = Include.NoRID;
                                    sep.FWOSModModelName = string.Empty;
                                }
                                break;
                            default:
                                sep.FWOSModPct = 0;
                                break;
                        }
                    }
                    else if (sep.FWOSModType != eModifierType.None)  // clear value
                    {
                        sep.FWOSModType = eModifierType.None;
                        sep.FWOSModModelRID = Include.NoRID;
                        sep.FWOSModModelName = null;
                        sep.FWOSModPct = 0;
                        sep.FWOSModIsInherited = false;
                        sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                    }

                    // similar store
                    if (eligibilityAttributeSet.EligibilityValues.SimilarStoreType != eSimilarStoreType.None)
                    {
                        sep.SimStoreType = eligibilityAttributeSet.EligibilityValues.SimilarStoreType;
                        sep.SimStoreIsInherited = false;
                        sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                        switch (eligibilityAttributeSet.EligibilityValues.SimilarStoreType)
                        {
                            case eSimilarStoreType.Stores:
                                sep.SimStores.Clear();
                                foreach (KeyValuePair<int, string> store in eligibilityAttributeSet.EligibilityValues.SimilarStores)
                                {
                                    sep.SimStores.Add(store.Key);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (eligibilityStore.EligibilityValues.SimilarStoreType != eSimilarStoreType.None)
                    {
                        sep.SimStoreType = eligibilityStore.EligibilityValues.SimilarStoreType;
                        switch (eligibilityStore.EligibilityValues.SimilarStoreType)
                        {
                            case eSimilarStoreType.Stores:
                                bool storesChanged = false;
                                if (eligibilityStore.EligibilityValues.SimilarStores.Count != sep.SimStores.Count)  // if count is different, the stores were changed
                                {
                                    storesChanged = true;
                                }
                                else   // check each store to see if any changed
                                {
                                    foreach (KeyValuePair<int, string> store in eligibilityStore.EligibilityValues.SimilarStores)
                                    {
                                        if (!sep.SimStores.Contains(store.Key))
                                        {
                                            storesChanged = true;
                                            break;
                                        }
                                    }
                                }
                                if (storesChanged)
                                {
                                    sep.SimStoreIsInherited = false;
                                    sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                    sep.SimStores.Clear();
                                    foreach (KeyValuePair<int, string> store in eligibilityStore.EligibilityValues.SimilarStores)
                                    {
                                        sep.SimStores.Add(store.Key);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (sep.SimStoreType != eSimilarStoreType.None)  // clear value
                    {
                        sep.SimStoreType = eSimilarStoreType.None;
                        sep.SimStores.Clear();
                        sep.SimStoreRatio = 0;
                        sep.SimStoreUntilDateRangeRID = Include.NoRID;
                        sep.SimStoreDisplayDate = string.Empty;
                        sep.SimStoreIsInherited = false;
                        sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                    }

                    if (eligibilityAttributeSet.EligibilityValues.SimilarStoreRatioIsSet)
                    {
                        sep.SimStoreRatio = Convert.ToDouble(eligibilityAttributeSet.EligibilityValues.SimilarStoreRatio);
                        sep.SimStoreIsInherited = false;
                        sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                    }
                    else if (eligibilityStore.EligibilityValues.SimilarStoreRatioIsSet)
                    {
                        if (sep.SimStoreRatio != eligibilityStore.EligibilityValues.SimilarStoreRatio)
                        {
                            sep.SimStoreRatio = Convert.ToDouble(eligibilityStore.EligibilityValues.SimilarStoreRatio);
                            sep.SimStoreIsInherited = false;
                            sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                        }
                    }
                    else if (sep.SimStoreRatio != 0)
                    {
                        sep.SimStoreRatio = 0;
                    }

                    if (eligibilityAttributeSet.EligibilityValues.SimilarStoreUntilDateRangeIsSet)
                    {
                        sep.SimStoreUntilDateRangeRID = eligibilityAttributeSet.EligibilityValues.SimilarStoreUntilDateRange.Key;
                        sep.SimStoreDisplayDate = eligibilityAttributeSet.EligibilityValues.SimilarStoreUntilDateRange.Value;
                        sep.SimStoreIsInherited = false;
                        sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                    }
                    else if (eligibilityStore.EligibilityValues.SimilarStoreUntilDateRangeIsSet)
                    {
                        if (sep.SimStoreUntilDateRangeRID != eligibilityStore.EligibilityValues.SimilarStoreUntilDateRange.Key)
                        {
                            sep.SimStoreUntilDateRangeRID = eligibilityStore.EligibilityValues.SimilarStoreUntilDateRange.Key;
                            sep.SimStoreDisplayDate = eligibilityStore.EligibilityValues.SimilarStoreUntilDateRange.Value;
                            sep.SimStoreIsInherited = false;
                            sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                        }
                    }
                    else if (sep.SimStoreUntilDateRangeRID != Include.NoRID)
                    {
                        sep.SimStoreUntilDateRangeRID = Include.NoRID;
                        sep.SimStoreDisplayDate = null;
                    }

                    // presentation plus sales
                    if (eligibilityAttributeSet.EligibilityValues.PresentationPlusSalesIndIsSet)
                    {
                        sep.PresPlusSalesInd = (bool)eligibilityAttributeSet.EligibilityValues.PresentationPlusSalesInd;
                        sep.PresPlusSalesIsInherited = false;
                        sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                    }
                    else if (eligibilityStore.EligibilityValues.PresentationPlusSalesIndIsSet)
                    {
                        if (sep.PresPlusSalesInd != eligibilityStore.EligibilityValues.PresentationPlusSalesInd)
                        {
                            sep.PresPlusSalesInd = (bool)eligibilityStore.EligibilityValues.PresentationPlusSalesInd;
                            sep.PresPlusSalesIsInherited = false;
                            sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                        }
                    }
                    else if (sep.PresPlusSalesInd)  // clear value
                    {
                        sep.PresPlusSalesInd = false;
                        sep.PresPlusSalesIsInherited = false;
                        sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                    }

                    // stock lead weeks
                    if (eligibilityAttributeSet.EligibilityValues.StockLeadWeeksIsSet)
                    {
                        sep.StkLeadWeeks = Convert.ToInt32(eligibilityAttributeSet.EligibilityValues.StockLeadWeeks);
                        sep.StkLeadWeeksInherited = false;
                        sep.StkLeadWeeksInheritedRid = Include.NoRID;
                    }
                    else if (eligibilityStore.EligibilityValues.StockLeadWeeksIsSet)
                    {
                        if (sep.StkLeadWeeks != eligibilityStore.EligibilityValues.StockLeadWeeks)
                        {
                            sep.StkLeadWeeks = Convert.ToInt32(eligibilityStore.EligibilityValues.StockLeadWeeks);
                            sep.StkLeadWeeksInherited = false;
                            sep.StkLeadWeeksInheritedRid = Include.NoRID;
                        }
                    }
                    else if (sep.StkLeadWeeks != 0)  // clear value
                    {
                        sep.StkLeadWeeks = 0;
                        sep.StkLeadWeeksInherited = false;
                        sep.StkLeadWeeksInheritedRid = Include.NoRID;
                    }
                }
            }

            return true;
        }

        private bool UpdateEligibility(int nodeKey, ref string message)
        {
            bool successful = true;
            ArrayList sepSimStores = null;
            StoreProfile sp;
            int storeRID;
            string storeID = null;
            ArrayList ErrorList = new ArrayList();
            bool similarStoreChanged = false;
            System.Data.DataSet storeEligibilityDataSet = MIDEnvironment.CreateDataSet("storeEligibilityDataSet");
            storeEligibilityDataSet seds = new storeEligibilityDataSet();
            storeEligibilityDataSet = seds.StoreEligibility_Define(storeEligibilityDataSet);
            ProfileList storeList = SAB.StoreServerSession.GetActiveStoresList();
            // get original values to update with new values
            StoreEligibilityList storeEligList = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, nodeKey, true, false);
            StoreEligibilityProfile sep_orig;

            // build dataset with new values for validation
            foreach (StoreEligibilityProfile sep in _storeEligList)
            {
                object eligModKey = System.DBNull.Value;
                object Ineligible = System.DBNull.Value;
                object slsModKey = System.DBNull.Value;
                object stkModKey = System.DBNull.Value;
                object FWOSModKey = System.DBNull.Value;
                object FWOSmodifierValue = System.DBNull.Value;
                object eligibilityValue = System.DBNull.Value;
                object stockmodifierValue = System.DBNull.Value;
                object salesmodifierValue = System.DBNull.Value;
                object simStores = System.DBNull.Value;
                object simStoresIndex = System.DBNull.Value;
                object MinPlusSales = System.DBNull.Value;
                object FWOSmodifierModel = System.DBNull.Value;
                object eligibilityModel = System.DBNull.Value;
                object stockmodifierModel = System.DBNull.Value;
                object salesmodifierModel = System.DBNull.Value;
                int dateRangeKey = Include.NoRID;
                string dateRangeID = null;
                similarStoreChanged = false;

                try
                {
                    sp = (StoreProfile)storeList.FindKey(sep.Key);
                    if (sp == null)
                    {
                        sp = Include.GetUnknownStoreProfile();
                    }
                    storeRID = sp.Key;
                    storeID = sp.Text;

                    sep_orig = null;
                    if (storeEligList.Contains(sp.Key))
                    {
                        sep_orig = (StoreEligibilityProfile)_storeEligList.FindKey(sp.Key);
                    }

                    if (sep_orig == null
                        || sep_orig.SimStores.Count != sep.SimStores.Count)
                    {
                        similarStoreChanged = true;
                    }

                    sepSimStores = sep.SimStores;
                    simStoresIndex = sep.SimStoreRatio;
                    dateRangeID = sep.SimStoreDisplayDate;
                    dateRangeKey = sep.SimStoreUntilDateRangeRID;
                    for (int ss = 0; ss < sep.SimStores.Count; ss++)
                    {
                        int SSRID = Convert.ToInt32(sep.SimStores[ss]);
                        StoreProfile SSsp = null;
                        if (storeList != null)
                        {
                            SSsp = (StoreProfile)storeList.FindKey(Convert.ToInt32(SSRID));
                        }
                        if (SSsp == null)
                        {
                            SSsp = Include.GetUnknownStoreProfile();
                        }
                        if (ss > 0 && ss != sep.SimStores.Count)
                        {
                            simStores = simStores + ",";
                        }
                        simStores = simStores + SSsp.StoreId;
                        if (sep_orig != null
                            && !sep_orig.SimStores.Contains(SSRID))
                        {
                            similarStoreChanged = true;
                        }
                    }
                    if (!sep.EligIsInherited)
                    {
                        eligModKey = sep.EligModelRID;
                        Ineligible = sep.StoreIneligible;
                        eligibilityModel = sep.EligModelName;
                    }
                    if (!sep.SlsModIsInherited)
                    {
                        if (sep.SlsModType == eModifierType.Model)
                        {
                            slsModKey = sep.SlsModModelRID;
                            salesmodifierModel = sep.SlsModModelRID;
                            salesmodifierValue = sep.SlsModModelName;
                        }
                        else if (sep.SlsModType == eModifierType.Percent)
                        {
                            salesmodifierValue = sep.SlsModPct;
                            slsModKey = Include.NoRID;
                            salesmodifierModel = Include.NoRID;
                        }
                        else
                        {
                            slsModKey = Include.NoRID;
                            salesmodifierModel = Include.NoRID;
                        }
                    }
                    if (!sep.StkModIsInherited)
                    {
                        if (sep.StkModType == eModifierType.Model)
                        {
                            stkModKey = sep.StkModModelRID;
                            stockmodifierModel = sep.StkModModelRID;
                            stockmodifierValue = sep.StkModModelName;
                        }
                        else if (sep.StkModType == eModifierType.Percent)
                        {
                            stockmodifierValue = sep.StkModPct;
                            stkModKey = Include.NoRID;
                            stockmodifierModel = Include.NoRID;
                        }
                        else
                        {
                            stkModKey = Include.NoRID;
                            stockmodifierModel = Include.NoRID;
                        }
                    }
                    if (!sep.FWOSModIsInherited)
                    {
                        if (sep.FWOSModType == eModifierType.Model)
                        {
                            FWOSModKey = sep.FWOSModModelRID;
                            FWOSmodifierModel = sep.FWOSModModelRID;
                            FWOSmodifierValue = sep.FWOSModModelName;
                        }
                        else if (sep.FWOSModType == eModifierType.Percent)
                        {
                            FWOSmodifierValue = sep.FWOSModPct;
                            FWOSModKey = Include.NoRID;
                            FWOSmodifierModel = Include.NoRID;
                        }
                        else
                        {
                            FWOSModKey = Include.NoRID;
                            FWOSmodifierModel = Include.NoRID;
                        }
                    }
                    if (!sep.PresPlusSalesIsInherited)
                    {
                        MinPlusSales = sep.PresPlusSalesInd;
                    }
                    if (!sep.SimStoreIsInherited)
                    {
                        sepSimStores = sep.SimStores;
                        simStoresIndex = sep.SimStoreRatio;
                        similarStoreChanged = true;
                    }

                    storeEligibilityDataSet.Tables["Stores"].Rows.Add(new object[] {null, sep.EligIsInherited, sep.EligInheritedFromNodeRID, true, storeRID, storeID,
                                                                                          eligModKey, eligibilityModel, Ineligible,
                                                                                          MinPlusSales, sep.PresPlusSalesIsInherited, sep.PresPlusSalesInheritedFromNodeRID,
                                                                                          sep.StkModIsInherited, sep.StkModInheritedFromNodeRID, stockmodifierModel, stockmodifierValue,
                                                                                          sep.SlsModIsInherited, sep.SlsModInheritedFromNodeRID, salesmodifierModel, salesmodifierValue,
                                                                                          sep.FWOSModIsInherited, sep.FWOSModInheritedFromNodeRID, FWOSmodifierModel, FWOSmodifierValue,
                                                                                          sep.SimStoreIsInherited, sep.SimStoreInheritedFromNodeRID,
                                                                                          false, similarStoreChanged, sepSimStores, simStores, simStoresIndex, dateRangeKey, dateRangeID});

                }
                catch (Exception err)
                {
                    message = err.ToString();
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, this.GetType().Name);
                    successful = false;
                }
            }

            if (successful)
            {
                // validate new values and update eligibility list if values pass
                ErrorList = HierarchyMaintenance.ValidEligibilityData(storeList, storeEligibilityDataSet, storeEligList, ErrorList);
                string errNodeID = SAB.HierarchyServerSession.GetNodeID(nodeKey);
                for (int e = 0; e < ErrorList.Count; e++)
                {
                    StoreEligibilityErrors see = (StoreEligibilityErrors)ErrorList[e];

                    if (see.typeErr == true || see.simStoreErr == true)
                    {
                        string errMessage = MIDText.GetText(eMIDTextCode.msg_InvalidArgument, false);
                        string detMessage = see.message;
                        if (see.simStoreErr)
                        {
                            StoreProfile sp1 = (StoreProfile)storeList.FindKey(see.storeRID);

                            detMessage = detMessage.Replace("{0}", see.dataString);
                            detMessage = detMessage.Replace("{1}", errNodeID);
                            detMessage = detMessage.Replace("{2}", sp1.StoreId.ToString());
                        }
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, detMessage, this.GetType().Name);
                        successful = false;
                    }
                }
            }

            // save the new values is all looks good
            if (successful)
            {
                SAB.HierarchyServerSession.StoreEligibilityUpdate(nodeKey, storeEligList, false);
                // set global list to updated values including inheritance for values removed
                _storeEligList = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, nodeKey, true, false);
            }

            return successful;
        }
        

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            ProfileList storeList = SAB.StoreServerSession.GetActiveStoresList();

            if (_storeEligList == null)
            {
                _storeEligList = GetEligibility(key: key);
            }

            foreach (StoreEligibilityProfile sep in _storeEligList)
            {
                sep.StoreEligChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.StoreEligibilityUpdate(key, _storeEligList, false);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteEligibility: true);
                }
                else
                {
                    message = MIDText.GetText(eMIDTextCode.lbl_ACLL_LockAttemptFailed);
                }
            }
            catch(Exception ex)
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
            return GetEligibility(key: parms.Key);
        }

        private StoreEligibilityList GetEligibility(int key)
        {
            return SAB.HierarchyServerSession.GetStoreEligibilityList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), nodeRID: key, chaseHierarchy: true, forCopy: false);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyEligibility, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyEligibility, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            int attributeSetKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesEligibility)
            {
                RONodePropertiesEligibility nodePropertiesEligibilityData = (RONodePropertiesEligibility)parms.RONodeProperties;
                attributeKey = nodePropertiesEligibilityData.Attribute.Key;
                if (nodePropertiesEligibilityData.AttributeSetIsSet)
                {
                    attributeSetKey = nodePropertiesEligibilityData.AttributeSet.Key;
                }
                else if (nodePropertiesEligibilityData.EligibilityAttributeSet != null)
                {
                    attributeSetKey = nodePropertiesEligibilityData.EligibilityAttributeSet.AttributeSet.Key;
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
