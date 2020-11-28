using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public class entryHeaderStatus : filterDictionaryEntry
    {
        public entryHeaderStatus(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Status";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.HeaderStatusesGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.HeaderStatus;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.HeaderStatus), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryHeaderTypes : filterDictionaryEntry
    {
        public entryHeaderTypes(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Types";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.HeaderTypesGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.HeaderTypes;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.HeaderTypes), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryHeaderFields : filterDictionaryEntry
    {
        public entryHeaderFields(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Fields";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterHeaderFieldTypes.ToDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterHeaderFieldTypes.GetValueTypeForField);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterHeaderFieldTypes.GetNameFromIndex);
            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterHeaderFieldTypes.GetValueListForHeaderFields);
            base.groupSettings.valueFieldForData = "VALUE_INDEX";
            base.groupSettings.valueFieldForDisplay = "VALUE_NAME";
            base.groupSettings.listValueType = filterListValueTypes.HeaderField;

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.Field, "Header Field:");
            eb.isField = true;
            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + filterHeaderFieldTypes.FromIndex(fc.fieldIndex).Name);
            filterValueTypes vt = filterValueTypes.FromIndex(fc.valueTypeIndex);
            if (vt == filterValueTypes.List)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
                formattedText += filterUtility.FormatValue(options, " (");
                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.HeaderField), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
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
            fc.fieldIndex = 0;
            fc.operatorIndex = filterStringOperatorTypes.Contains;
        }
    }

    public class entryHeaderCharacteristics : filterDictionaryEntry
    {
        public entryHeaderCharacteristics(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Characteristics";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.HeaderCharacteristicsGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.HeaderCharacteristicsGetDataType);
            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterDataHelper.HeaderCharacteristicsGetNameFromIndex);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterDataHelper.HeaderCharacteristicsGetValuesForGroup);
            base.groupSettings.valueFieldForData = "HC_RID";
            base.groupSettings.valueFieldForDisplay = "CHAR_VALUE";
            base.groupSettings.listValueType = filterListValueTypes.HeaderCharacteristicRID;

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
                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.HeaderCharacteristicRID), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
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
            fc.fieldIndex = filterDataHelper.HeaderCharacteristicsGetFirstIndex();
            filterDataTypes dataType = filterDataHelper.HeaderCharacteristicsGetDataType(fc.fieldIndex);
            fc.valueTypeIndex = dataType.valueType.dbIndex;
            if (dataType.valueType == filterValueTypes.Text)
            {
                fc.operatorIndex = filterStringOperatorTypes.Contains;
            }
            else if (dataType.valueType == filterValueTypes.Numeric || dataType.valueType == filterValueTypes.Dollar)
            {
                fc.operatorIndex = filterNumericOperatorTypes.GreaterThan;
            }
            else if (dataType.valueType == filterValueTypes.Boolean)
            {
                fc.operatorIndex = -1;
            }
            else if (dataType.valueType == filterValueTypes.Date)
            {
                fc.operatorIndex = filterDateOperatorTypes.Last24Hours;
            }
            else if (dataType.valueType == filterValueTypes.List)
            {
               fc.operatorIndex = filterListOperatorTypes.Includes;
            }
         
        }
    }

    public class entryHeaderMerchandise : filterDictionaryEntry
    {
        public entryHeaderMerchandise(filterManager manager)
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
            fc.headerMerchandise_HN_RID = -1;
        }
    }

    public class entryHeaderDate : filterDictionaryEntry
    {
        public entryHeaderDate(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Date";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorDate, "Header Date:");
            eb.isOperatorDate = true;
            filterDataTypes vInfo = new filterDataTypes(filterValueTypes.Date);
            vInfo.dateType = filterDateTypes.DateOnly;
            eb.dataType = vInfo;

            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " Header Date: ");
            BuildFormattedTextForDate(ref formattedText, options, fc);


            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.fieldIndex = 0;
            fc.operatorIndex = filterDateOperatorTypes.Unrestricted;
            fc.valueToCompareDateBetweenFromDays = -7;
            fc.valueToCompareDateBetweenToDays = 0;
        }
    }

    public class entryHeaderReleaseDate : filterDictionaryEntry
    {
        public entryHeaderReleaseDate(filterManager manager)
        {
            base.groupSettings.groupTitle = "Release Date";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorDate, "Rls Date:");
            eb.isOperatorDate = true;
            filterDataTypes vInfo = new filterDataTypes(filterValueTypes.Date);
            vInfo.dateType = filterDateTypes.DateAndTime;
            eb.dataType = vInfo;
            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " Release Date: ");
            BuildFormattedTextForDate(ref formattedText, options, fc);


            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.fieldIndex = 0;
            fc.operatorIndex = filterDateOperatorTypes.Unrestricted;
            fc.valueToCompareDateBetweenFromDays = -7;
            fc.valueToCompareDateBetweenToDays = 0;
        }
    }

    public class entryHeaderSortBy : filterDictionaryEntry //TT#1468-MD -jsobek -Header Filter Sort Options
    {
        public entryHeaderSortBy(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Sort By";



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
            fc.sortByTypeIndex = filterSortByTypes.HeaderFields;
            fc.sortByFieldIndex = filterHeaderFieldTypes.HeaderID;
            fc.operatorIndex = filterSortByDirectionTypes.Ascending;
        }
    }
}
