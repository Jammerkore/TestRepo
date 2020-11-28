using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public class entryAssortmentStatus : filterDictionaryEntry
    {
        public entryAssortmentStatus(filterManager manager)
        {
            base.groupSettings.groupTitle = "Status";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.AssortmentStatusesGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AssortmentStatus;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AssortmentStatus), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryAssortmentTypes : filterDictionaryEntry
    {
        public entryAssortmentTypes(filterManager manager)
        {
            base.groupSettings.groupTitle = "Header Types";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.AssortmentTypesGetDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AssortmentTypes;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AssortmentTypes), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryAssortmentFields : filterDictionaryEntry
    {
        filterManager _manager;
        public entryAssortmentFields(filterManager manager)
        {
            base.groupSettings.groupTitle = "Assortment Fields";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterAssortmentFieldTypes.ToDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterAssortmentFieldTypes.GetValueTypeForField);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterAssortmentFieldTypes.GetNameFromIndex);
            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterAssortmentFieldTypes.GetValueListForAssortmentFields);
            base.groupSettings.valueFieldForData = "VALUE_INDEX";
            base.groupSettings.valueFieldForDisplay = "VALUE_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AssortmentField;

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.Field, "Assortment Field:");
            eb.isField = true;
            base.elementList.Add(eb);
            _manager = manager;
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + filterAssortmentFieldTypes.FromIndex(fc.fieldIndex).Name);
            filterValueTypes vt = filterValueTypes.FromIndex(fc.valueTypeIndex);
            if (vt == filterValueTypes.List)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
                formattedText += filterUtility.FormatValue(options, " (");
                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AssortmentField), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
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
            else if (vt == filterValueTypes.Calendar)
            {
                BuildFormattedTextForCalendar(ref formattedText, options, fc, _manager);
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

    public class entryAssortmentMerchandise : filterDictionaryEntry
    {
        public entryAssortmentMerchandise(filterManager manager)
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

    public class entryAssortmentDate : filterDictionaryEntry
    {
        public entryAssortmentDate(filterManager manager)
        {
            base.groupSettings.groupTitle = "Assortment Date";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorDate, "Assortment Date:");
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
            formattedText += filterUtility.FormatNormal(options, " Date: ");
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

    public class entryAssortmentSortBy : filterDictionaryEntry //TT#1468-MD -jsobek -Header Filter Sort Options
    {
        public entryAssortmentSortBy(filterManager manager)
        {
            base.groupSettings.groupTitle = "Assortment Sort By";



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
            fc.sortByTypeIndex = filterSortByTypes.AssortmentFields;
            fc.sortByFieldIndex = filterAssortmentFieldTypes.AssortmentID;
            fc.operatorIndex = filterSortByDirectionTypes.Ascending;
        }
    }
}
