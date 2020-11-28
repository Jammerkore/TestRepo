using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public class entryStoreGroupName : filterDictionaryEntry
    {
        public entryStoreGroupName(filterManager manager)
        {
            base.groupSettings.groupTitle = "Set Name";
            base.elementList.Add(new elementBase(manager, filterElementMap.NameAndLimit, base.groupSettings.groupTitle));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            //formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            //formattedText += filterUtility.FormatOperator(options, " = ");
            formattedText += filterUtility.FormatNormal(options, fc.valueToCompare);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.valueToCompare = "New Set";
        }
    }
    public class entryStoreGroupExclusionList : filterDictionaryEntry
    {
        public entryStoreGroupExclusionList(filterManager manager)
        {
            base.groupSettings.groupTitle = "Excluded Stores";    
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.StoresGetDataTable);
            base.groupSettings.fieldForData = "STORE_RID";
            base.groupSettings.fieldForDisplay = "STORE_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreRID;
            base.elementList.Add(new elementBase(manager, filterElementMap.ExclusionList, base.groupSettings.groupTitle, true));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            //string formattedText = string.Empty;
            ////formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            ////formattedText += filterUtility.FormatOperator(options, " = ");
            //formattedText += filterUtility.FormatNormal(options, fc.valueToCompare);
            //fc.NodeFormattedText = formattedText;

            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, "Exclude:");
            formattedText += filterUtility.FormatValue(options, " (");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.StoreRID), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
            formattedText += filterUtility.FormatValue(options, ")");
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.operatorIndex = filterListOperatorTypes.Includes;
            fc.listConstantType = filterListConstantTypes.None;
        }
    }

    public class entryStoreGroupDynamic : filterDictionaryEntry
    {
        public entryStoreGroupDynamic(filterManager manager)
        {
            
            base.groupSettings.groupTitle = "Dynamic Set";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.StoreFieldsAndCharGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreCharacteristicRID;
            //base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            //base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
            //base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
            base.elementList.Add(new elementBase(manager, filterElementMap.DynamicSet, base.groupSettings.groupTitle));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
           
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " = ");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayForDropDown(fc.operatorIndex, base.groupSettings.loadValueList(), (int)fc.valueToCompareInt, base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
            formattedText += filterUtility.FormatOperator(options, " (" + filterSortByDirectionTypes.FromIndex((int)fc.valueToCompareInt2).Name + ")");
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.valueToCompareInt = -1;
            fc.valueToCompareInt2 = filterSortByDirectionTypes.Ascending.dbIndex;
            //fc.logicIndex = filterLogicTypes.And;
            //fc.operatorIndex = filterListOperatorTypes.Includes;
            //fc.listConstantType = filterListConstantTypes.None;
        }
    }

    public class entryStoreGroupOverride : filterDictionaryEntry
    {
        public entryStoreGroupOverride(filterManager manager)
        {

            base.groupSettings.groupTitle = "Override";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.StoreFieldsAndCharGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreCharacteristicRID;
            //base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            //base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
            //base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
            base.elementList.Add(new elementBase(manager, filterElementMap.DynamicSetOverride, base.groupSettings.groupTitle));
            base.elementList.Add(new elementBase(manager, filterElementMap.ValueToCompareString, "Value:"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;

            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " = ");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayForDropDown(fc.operatorIndex, base.groupSettings.loadValueList(), (int)fc.valueToCompareInt, base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
            formattedText += filterUtility.FormatLogic(options, " and ");
            formattedText += filterUtility.FormatNormal(options, "Value");
            formattedText += filterUtility.FormatOperator(options, " = ");
            formattedText += filterUtility.FormatValue(options, fc.valueToCompare);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            //fc.logicIndex = filterLogicTypes.And;
            //fc.operatorIndex = filterListOperatorTypes.Includes;
            //fc.listConstantType = filterListConstantTypes.None;
        }
    }
}
