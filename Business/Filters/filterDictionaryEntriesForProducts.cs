using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public class entryProductAny: filterDictionaryEntry
    {
        public entryProductAny(filterManager manager)
        {
            base.groupSettings.groupTitle = "Any";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorString, "Operator:");
            eb.dataType = new filterDataTypes(filterValueTypes.Text);
            base.elementList.Add(eb);
            base.elementList.Add(new elementBase(manager, filterElementMap.ValueToCompareString, "Value"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            BuildFormattedTextForString(ref formattedText, options, fc);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterStringOperatorTypes.Contains;
        }
    }
    public class entryProductID : filterDictionaryEntry
    {
        public entryProductID(filterManager manager)
        {
            base.groupSettings.groupTitle = "ID";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorString, "Operator:");
            eb.dataType = new filterDataTypes(filterValueTypes.Text);
            base.elementList.Add(eb);
            base.elementList.Add(new elementBase(manager, filterElementMap.ValueToCompareString, "Value"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            BuildFormattedTextForString(ref formattedText, options, fc);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterStringOperatorTypes.Contains;
        }
    }
    public class entryProductName : filterDictionaryEntry
    {
        public entryProductName(filterManager manager)
        {
            base.groupSettings.groupTitle = "Name";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorString, "Operator:");
            eb.dataType = new filterDataTypes(filterValueTypes.Text);
            base.elementList.Add(eb);
            base.elementList.Add(new elementBase(manager, filterElementMap.ValueToCompareString, "Value:"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            BuildFormattedTextForString(ref formattedText, options, fc);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterStringOperatorTypes.Contains;
        }
    }
    public class entryProductDescrip : filterDictionaryEntry
    {
        public entryProductDescrip(filterManager manager)
        {
            base.groupSettings.groupTitle = "Descrip";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorString, "Operator:");
            eb.dataType = new filterDataTypes(filterValueTypes.Text);
            base.elementList.Add(eb);
            base.elementList.Add(new elementBase(manager, filterElementMap.ValueToCompareString, "Value:"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            BuildFormattedTextForString(ref formattedText, options, fc);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterStringOperatorTypes.Contains;
        }
    }

    public class entryProductCharacteristics : filterDictionaryEntry
    {
        public entryProductCharacteristics(filterManager manager)
        {
            base.groupSettings.groupTitle = "Product Characteristics";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.ProductCharacteristicsGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.ProductCharacteristicsGetDataType);
            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterDataHelper.ProductCharacteristicsGetNameFromIndex);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterDataHelper.ProductCharacteristicsGetValuesForGroup);
            base.groupSettings.valueFieldForData = "HC_RID";
            base.groupSettings.valueFieldForDisplay = "CHAR_VALUE";
            base.groupSettings.listValueType = filterListValueTypes.ProductCharacteristicRID;

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.Field, "Characteristics:");
            eb.isField = true;
            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.GetNameFromField(fc.fieldIndex));
            filterValueTypes vt = filterValueTypes.FromIndex(fc.valueTypeIndex);
            if (vt == filterValueTypes.List)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
                formattedText += filterUtility.FormatValue(options, " (");
                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.ProductCharacteristicRID), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
                formattedText += filterUtility.FormatValue(options, ")");
            }
            else if (vt == filterValueTypes.Text)
            {
                BuildFormattedTextForString(ref formattedText, options, fc);
            }
            else if (vt == filterValueTypes.Boolean)
            {
                BuildFormattedTextForBoolean(ref formattedText, options, fc);
            }
            else if (vt == filterValueTypes.Date)
            {
                BuildFormattedTextForDate(ref formattedText, options, fc);
            }
            else if (vt == filterValueTypes.Numeric || vt == filterValueTypes.Dollar)
            {
                BuildFormattedTextForNumeric(ref formattedText, options, fc, vt);
            }


            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.fieldIndex = filterDataHelper.ProductCharacteristicsGetFirstIndex();
            filterDataTypes dataType = filterDataHelper.ProductCharacteristicsGetDataType(fc.fieldIndex);
            fc.valueTypeIndex = dataType.valueType.dbIndex;
            //if (dataType.valueType == filterValueTypes.Text)
            //{
            //    fc.operatorIndex = filterStringOperatorTypes.Contains;
            //}
            //else if (dataType.valueType == filterValueTypes.Numeric || dataType.valueType == filterValueTypes.Dollar)
            //{
            //    fc.operatorIndex = filterNumericOperatorTypes.GreaterThan;
            //}
            //else if (dataType.valueType == filterValueTypes.Boolean)
            //{
            //    fc.operatorIndex = -1;
            //}
            //else if (dataType.valueType == filterValueTypes.Date)
            //{
            //    fc.operatorIndex = filterDateOperatorTypes.Last24Hours;
            //}
            //else if (dataType.valueType == filterValueTypes.List)
            //{
               fc.operatorIndex = filterListOperatorTypes.Includes;
            //}
         
        }
    }

    public class entryProductActive : filterDictionaryEntry
    {
        public entryProductActive(filterManager manager)
        {
            base.groupSettings.groupTitle = "Active";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            base.elementList.Add(new elementBase(manager, filterElementMap.ValueToCompareBool, "Value"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            BuildFormattedTextForBoolean(ref formattedText, options, fc);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterNumericOperatorTypes.DoesEqual;
            fc.valueToCompareBool = true;
        }
    }

    public class entryProductMerchandise : filterDictionaryEntry
    {
        public entryProductMerchandise(filterManager manager)
        {
            base.groupSettings.groupTitle = "Merchandise";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase b1 = new elementBase(manager, filterElementMap.Merchandise, "Merchandise");
            b1.loadFromHeaderMerchandise = true;
            base.elementList.Add(b1);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatOperator(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " " + filterNumericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " " + filterUtility.getTextFromHierarchyNodeDelegate(fc.headerMerchandise_HN_RID));
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterNumericOperatorTypes.DoesEqual;

            int hnRid = Include.NoRID;
            if (filterUtility.GetFirstSelectedMerchandiseNode != null)
            { 
                HierarchyNodeProfile hnp = filterUtility.GetFirstSelectedMerchandiseNode();
                if (hnp != null)
                {
                    hnRid = hnp.Key;
                }
            }
            fc.headerMerchandise_HN_RID = hnRid;
        }
    }

    public class entryProductLevels : filterDictionaryEntry
    {
        public entryProductLevels(filterManager manager)
        {
            base.groupSettings.groupTitle = "Levels";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.ProductLevelsGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.ProductLevels;
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
            base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " (");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.ProductLevels), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
            formattedText += filterUtility.FormatValue(options, ")");
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterListOperatorTypes.Includes;
            fc.listConstantType = filterListConstantTypes.None;
        }
    }

    public class entryProductHierarchies : filterDictionaryEntry
    {
        public entryProductHierarchies(filterManager manager)
        {
            base.groupSettings.groupTitle = "Hierarchies";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.ProductHierarchiesGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.ProductHierarchies;
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
            base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " (");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.ProductHierarchies), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
            formattedText += filterUtility.FormatValue(options, ")");
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterListOperatorTypes.Includes;
            fc.listConstantType = filterListConstantTypes.None;
        }
    }
    public class entryProductSortBy : filterDictionaryEntry
    {
        public entryProductSortBy(filterManager manager)
        {
            base.groupSettings.groupTitle = "Sort By";

            elementBase eb = new elementBase(manager, filterElementMap.SortByType, "Sort By Type");
            eb.isSortBy = true;
            base.elementList.Add(eb);


        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            BuildFormattedTextForSortBy(ref formattedText, options, fc);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.sortByTypeIndex = filterSortByTypes.ProductSearchFields;
            fc.sortByFieldIndex = filterProductFieldTypes.Hierarchy;
            fc.operatorIndex = filterSortByDirectionTypes.Ascending;
        }
    }

}
