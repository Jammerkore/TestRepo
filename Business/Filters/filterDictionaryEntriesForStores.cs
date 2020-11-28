using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{

    public class entryStoreList : filterDictionaryEntry
    {
        public entryStoreList(filterManager manager)
        {
            base.groupSettings.groupTitle = "Store List";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.StoresGetDataTable);
            base.groupSettings.fieldForData = "STORE_RID";
            base.groupSettings.fieldForDisplay = "STORE_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreRID;
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
            base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " Store");
            formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " (");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.StoreRID), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryStoreFields : filterDictionaryEntry
    {
        public entryStoreFields(filterManager manager)
        {
            base.groupSettings.groupTitle = "Store Fields";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterStoreFieldTypes.ToDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterStoreFieldTypes.GetValueTypeInfoForField);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterStoreFieldTypes.GetNameFromIndex);
            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterStoreFieldTypes.GetValueListForStoreFields);
            base.groupSettings.valueFieldForData = "VALUE_INDEX";
            base.groupSettings.valueFieldForDisplay = "VALUE_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreField;

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            elementBase eb = new elementBase(manager, filterElementMap.Field, "Store Field:");
            eb.isField = true;
            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + filterStoreFieldTypes.FromIndex(fc.fieldIndex).Name);
            filterValueTypes vt = filterValueTypes.FromIndex(fc.valueTypeIndex);
            if (vt == filterValueTypes.List)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
                formattedText += filterUtility.FormatValue(options, " (");
                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.StoreField), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
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

    public class entryStoreStatus : filterDictionaryEntry
    {
        public entryStoreStatus(filterManager manager)
        {
            base.groupSettings.groupTitle = "Store Status";
            base.groupSettings.loadValueList = new FilterLoadListDelegate(filterStoreStatusTypes.ToDataTable);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreStatus;
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
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.StoreStatus), base.groupSettings.loadValueList(), base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
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

    public class entryStoreAttributeSet : filterDictionaryEntry
    {
        public entryStoreAttributeSet(filterManager manager)
        {
            base.groupSettings.groupTitle = "Attribute Set";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.AttributeSetGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.AttributeSetGetDataType);
            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterDataHelper.AttributeSetGetNameFromIndex);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterDataHelper.AttributeSetGetValuesForGroup);
            base.groupSettings.valueFieldForData = "GROUP_INDEX";
            base.groupSettings.valueFieldForDisplay = "GROUP_NAME";
            base.groupSettings.listValueType = filterListValueTypes.StoreGroupLevel;

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            string attributeSet = MIDRetail.Data.MIDText.GetTextFromCode((int)eMIDTextCode.lbl_AttributeSet) + ":";
            elementBase eb = new elementBase(manager, filterElementMap.Field, attributeSet);
            eb.isField = true;
            eb.dragDropTypesAllowed.Add(eProfileType.StoreGroup);
            base.elementList.Add(eb);
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.GetNameFromField(fc.fieldIndex));
            filterValueTypes vt = filterValueTypes.FromIndex(fc.valueTypeIndex);
            //if (vt == valueTypes.List)
            //{
            formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " (");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.StoreGroupLevel), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, "GROUP_NAME_NO_COUNT"));
            formattedText += filterUtility.FormatValue(options, ")");
            //}
            //else if (vt == valueTypes.Text)
            //{
            //    BuildFormattedTextForString(ref formattedText, options, fc);
            //}
            //else if (vt == valueTypes.Boolean)
            //{
            //    BuildFormattedTextForBoolean(ref formattedText, options, fc);
            //}
            //else if (vt == valueTypes.Date)
            //{
            //    BuildFormattedTextForDate(ref formattedText, options, fc);
            //}
            //else if (vt == valueTypes.Numeric || vt == valueTypes.Dollar)
            //{
            //    BuildFormattedTextForNumeric(ref formattedText, options, fc, vt);
            //}


            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.fieldIndex = -1;
            fc.operatorIndex = filterListOperatorTypes.Includes;
        }
    }

    public class entryStoreCharacteristics : filterDictionaryEntry
    {
        public entryStoreCharacteristics(filterManager manager)
        {
            base.groupSettings.groupTitle = "Store Characteristics";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.StoreCharacteristicsGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.StoreCharacteristicsGetDataType);
            base.groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterDataHelper.StoreCharacteristicsGetNameFromIndex);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.groupSettings.loadValueListFromField = new FilterLoadValueListFromFieldDelegate(filterDataHelper.StoreCharacteristicsGetValuesForGroup);
            base.groupSettings.valueFieldForData = "SC_RID";
            base.groupSettings.valueFieldForDisplay = "CHAR_VALUE";
            base.groupSettings.listValueType = filterListValueTypes.StoreCharacteristicRID;

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
                if (fc.operatorIndex >= 0)
                {
                    formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
                    formattedText += filterUtility.FormatValue(options, " (");
                    formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(filterListValueTypes.StoreCharacteristicRID), base.groupSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueFieldForData, base.groupSettings.valueFieldForDisplay));
                    formattedText += filterUtility.FormatValue(options, ")");
                }
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
            fc.fieldIndex = filterDataHelper.StoreCharacteristicsGetFirstIndex();
            filterDataTypes dataType = filterDataHelper.StoreCharacteristicsGetDataType(fc.fieldIndex);
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

    public class entryStoreVariableToConstant : filterDictionaryEntry
    {
        public entryStoreVariableToConstant(filterManager manager)
        {
            base.groupSettings.groupTitle = "Variable to Constant";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.VariablesGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetDataType);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));

            elementBase b1 = new elementBase(manager, filterElementMap.Variable, "Variable:");
            b1.isVariable = true;
            b1.loadFromVariable1 = true;
            b1.loadFromVariable2 = false;
            b1.useDynamicOperator = true;
            base.elementList.Add(b1);


        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, true, false);


            if (fc.valueTypeIndex == filterValueTypes.Text)
            {
                BuildFormattedTextForString(ref formattedText, options, fc);
            }
            else
            {
                BuildFormattedTextForNumeric(ref formattedText, options, fc, filterValueTypes.Numeric);
            }
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.variable1_Index = filterDataHelper.VariablesGetIndexFromName("Sales Reg");
            fc.variable1_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
            fc.variable1_HN_RID = -1;
            fc.variable1_CDR_RID = Include.UndefinedCalendarDateRange;
            fc.variable1_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
            fc.variable1_TimeTypeIndex = variableTimeTypes.Any.dbIndex;


            fc.operatorIndex = filterNumericOperatorTypes.GreaterThan;

            filterDataTypes dataType = filterDataHelper.VariablesGetDataType(fc.variable1_Index);


            fc.valueTypeIndex = dataType.valueType.dbIndex; // filterValueTypes.Numeric;
            fc.numericTypeIndex = dataType.numericType.Index; // filterNumericTypes.DoubleFreeForm;

            if (filterDataHelper.VariablesGetIsGrade(fc.variable1_Index))
            {
            }
            else
            {
                if (fc.numericTypeIndex == filterNumericTypes.Integer)
                {
                    fc.valueToCompareInt = 0;
                }
                else
                {
                    fc.valueToCompareDouble = 0;
                }
            }
         
            
        }
    }

    public class entryStoreVariableToVariable : filterDictionaryEntry
    {
        public entryStoreVariableToVariable(filterManager manager)
        {
            base.groupSettings.groupTitle = "Variable to Variable";
            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.VariablesGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetDataType);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));

            elementBase b1 = new elementBase(manager, filterElementMap.Variable, "1st Variable:");
            b1.loadFromVariable1 = true;
            b1.loadFromVariable2 = false;
            base.elementList.Add(b1);

            elementBase bOp = new elementBase(manager, filterElementMap.OperatorNumericForVariables, "Operator");
            base.elementList.Add(bOp);

            elementBase b2 = new elementBase(manager, filterElementMap.Variable, "2nd Variable:");
            b2.loadFromVariable1 = false;
            b2.loadFromVariable2 = true;
            base.elementList.Add(b2);


        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, true, false);
            formattedText += filterUtility.FormatOperator(options, " " + filterNumericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, false, true);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.variable1_Index = filterDataHelper.VariablesGetIndexFromName("Sales Reg");
            fc.variable1_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
            fc.variable1_HN_RID = -1;
            fc.variable1_CDR_RID = Include.UndefinedCalendarDateRange;
            fc.variable1_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
            fc.variable1_TimeTypeIndex = variableTimeTypes.Any.dbIndex;

            fc.operatorIndex = filterNumericOperatorTypes.LessThan;

            fc.variable2_Index = filterDataHelper.VariablesGetIndexFromName("Stock Reg");
            fc.variable2_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
            fc.variable2_HN_RID = -1;
            fc.variable2_CDR_RID = Include.UndefinedCalendarDateRange;
            fc.variable2_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
            fc.variable2_TimeTypeIndex = variableTimeTypes.Corresponding.dbIndex; //TT#1536-MD -jsobek -Store Filters - Default 2nd variable time modifier to Corresponding
        }
    }

    public class entryStoreVariablePercentage : filterDictionaryEntry
    {
        public entryStoreVariablePercentage(filterManager manager)
        {
            base.groupSettings.groupTitle = "Variable Percentage";

            base.groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.VariablesGetDataTable);
            base.groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetDataType);
            base.groupSettings.fieldForData = "FIELD_INDEX";
            base.groupSettings.fieldForDisplay = "FIELD_NAME";

            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));

            elementBase b1 = new elementBase(manager, filterElementMap.Variable, "1st Variable:");
            b1.loadFromVariable1 = true;
            b1.loadFromVariable2 = false;
            base.elementList.Add(b1);

            elementBase bPercentOp = new elementBase(manager, filterElementMap.OperatorVariablePercentage, "Percent Operator");
            filterDataTypes vInfo2 = new filterDataTypes(filterValueTypes.Numeric);
            vInfo2.numericType = filterNumericTypes.DoubleFreeForm;
            bPercentOp.dataType = vInfo2;
            base.elementList.Add(bPercentOp);

            elementBase b2 = new elementBase(manager, filterElementMap.Variable, "2nd Variable:");
            b2.loadFromVariable1 = false;
            b2.loadFromVariable2 = true;
            base.elementList.Add(b2);

            elementBase bOp = new elementBase(manager, filterElementMap.OperatorNumeric, "Operator");
            bOp.isOperatorNumeric = true;
            filterDataTypes vInfo = new filterDataTypes(filterValueTypes.Numeric);
            vInfo.numericType = filterNumericTypes.DoubleFreeForm;
            bOp.dataType = vInfo;
            base.elementList.Add(bOp);

        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, true, false);
            formattedText += filterUtility.FormatOperator(options, " " + filterPercentageOperatorTypes.FromIndex(fc.operatorVariablePercentageIndex).symbol);
            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, false, true);
            //formattedText += filterUtility.FormatOperator(options, " " + numericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            //formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompare);
            BuildFormattedTextForNumeric(ref formattedText, options, fc, filterValueTypes.Numeric);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.variable1_Index = filterDataHelper.VariablesGetIndexFromName("Sales Reg");
            fc.variable1_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
            fc.variable1_HN_RID = -1;
            fc.variable1_CDR_RID = Include.UndefinedCalendarDateRange;
            fc.variable1_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
            fc.variable1_TimeTypeIndex = variableTimeTypes.Any.dbIndex;

            fc.operatorVariablePercentageIndex = filterPercentageOperatorTypes.PercentOf;

            fc.variable2_Index = filterDataHelper.VariablesGetIndexFromName("Stock Reg");
            fc.variable2_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
            fc.variable2_HN_RID = -1;
            fc.variable2_CDR_RID = Include.UndefinedCalendarDateRange;
            fc.variable2_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
            //fc.variable2_TimeTypeIndex = variableTimeTypes.Any.dbIndex;
            fc.variable2_TimeTypeIndex = variableTimeTypes.Corresponding.dbIndex; //TT#1551-MD -jsobek -Store Filters - Default 2nd variable time modifier to Corresponding for Var % option

            fc.operatorIndex = filterNumericOperatorTypes.GreaterThan;
            fc.valueTypeIndex = filterValueTypes.Numeric;
            fc.numericTypeIndex = filterNumericTypes.DoubleFreeForm;
            fc.valueToCompareDouble = .25;
        }
    }

    public class entryStoreSortBy : filterDictionaryEntry
    {
        public entryStoreSortBy(filterManager manager)
        {
            base.groupSettings.groupTitle = "Store Sort By";

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
            fc.sortByTypeIndex = filterSortByTypes.StoreFields;
            fc.sortByFieldIndex = filterStoreFieldTypes.StoreName;
            fc.operatorIndex = filterSortByDirectionTypes.Ascending;
        }
    }
}
