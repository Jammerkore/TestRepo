using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{

    public class entryAuditProcessTime : filterDictionaryEntry
    {
        public entryAuditProcessTime(filterManager manager)
        {
            base.groupSettings.groupTitle = "Start Time";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorDate, "Start Time:");
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
            formattedText += filterUtility.FormatNormal(options, " Start Time: ");
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

    public class entryAuditDetailTime : filterDictionaryEntry
    {
        public entryAuditDetailTime(filterManager manager)
        {
            base.groupSettings.groupTitle = "Detail Time";
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.OperatorDate, "Detail Time:");
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
            formattedText += filterUtility.FormatNormal(options, " Detail Time: ");
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

    public class entryAuditFields : filterDictionaryEntry
    {
        public entryAuditFields(filterManager manager)
        {
            base.groupSettings.groupTitle = "Audit Fields";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterAuditFieldTypes.ToDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterAuditFieldTypes.GetValueTypeForField);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterAuditFieldTypes.GetNameFromIndex);
            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterAuditFieldTypes.GetValueListForAuditFields);
            base.groupSettings.valueFieldForData = "VALUE_INDEX";
            base.groupSettings.valueFieldForDisplay = "VALUE_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AuditField;

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.Field, "Audit Field:");
            eb.isField = true;
            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + filterAuditFieldTypes.FromIndex(fc.fieldIndex).Name);
            filterValueTypes vt = filterValueTypes.FromIndex(fc.valueTypeIndex);
            if (vt == filterValueTypes.List)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
                formattedText += filterUtility.FormatValue(options, " (");
                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AuditField), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
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

    //public class entryAuditStatus : filterDictionaryEntry
    //{
    //    public entryAuditStatus(filterManager manager)
    //    {
    //        base.groupSettings.groupTitle = "Audit Status";
    //        base.groupSettings.loadValueList = new FilterLoadListDelegate(filterAuditStatusType.ToDataTable);
    //        base.groupSettings.fieldForData = "FIELD_INDEX";
    //        base.groupSettings.fieldForDisplay = "FIELD_NAME";
    //        base.groupSettings.listValueType = filterListValueTypes.AuditStatus;
    //        base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
    //        base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
    //        base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
    //    }
    //    public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
    //    {
    //        string formattedText = string.Empty;
    //        formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
    //        formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
    //        formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
    //        formattedText += filterUtility.FormatValue(options, " (");
    //        formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AuditStatus), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
    //        formattedText += filterUtility.FormatValue(options, ")");
    //        fc.NodeFormattedText = formattedText;
    //    }
    //    public override void SetDefaults(ref filterCondition fc)
    //    {
    //        fc.logicIndex = filterLogicTypes.And;
    //        fc.operatorIndex = filterListOperatorTypes.Includes;
    //        fc.listConstantType = filterListConstantTypes.None;
    //    }
    //}

    public class entryAuditExecutionStatus : filterDictionaryEntry
    {
        public entryAuditExecutionStatus(filterManager manager)
        {
            base.groupSettings.groupTitle = "Execution Status";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterAuditProcessExecutionStatusType.ToDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AuditExecutionStatus;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AuditExecutionStatus), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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
    public class entryAuditCompletionStatus : filterDictionaryEntry
    {
        public entryAuditCompletionStatus(filterManager manager)
        {
            base.groupSettings.groupTitle = "Completion Status";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterAuditProcessCompletionStatusType.ToDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AuditCompletionStatus;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AuditCompletionStatus), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryAuditProcessMessageLevel : filterDictionaryEntry
    {
        public entryAuditProcessMessageLevel(filterManager manager)
        {
            base.groupSettings.groupTitle = "Message Level";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterAuditMessageLevelType.ToDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AuditMessageLevel;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AuditMessageLevel), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryAuditDetailMessageLevel : filterDictionaryEntry
    {
        public entryAuditDetailMessageLevel(filterManager manager)
        {
            base.groupSettings.groupTitle = "Detail Level";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterAuditMessageLevelType.ToDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.AuditMessageLevel;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.AuditMessageLevel), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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
    public class entryAuditSortBy : filterDictionaryEntry
    {
        public entryAuditSortBy(filterManager manager)
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
            fc.sortByTypeIndex = filterSortByTypes.AuditSearchFields;
            fc.sortByFieldIndex = filterAuditFieldTypes.ProcessName;
            fc.operatorIndex = filterSortByDirectionTypes.Ascending;
        }
    }
}
