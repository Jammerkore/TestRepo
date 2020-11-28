using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementVariable : UserControl, IFilterElement
    {
        public filterElementVariable()
        {
            InitializeComponent();

        }

        private elementBase eb;
        filterEntrySettings groupSettings;

        //delegates to access the container
        public FilterMakeElementInGroupDelegate makeElementInGroupDelegate;
        public FilterRemoveDynamicElementsForFieldDelegate removeDynamicElementsForFieldDelegate;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.groupSettings = groupSettings;
            this.filterElementMerchandise1.SetElementBase(eb, groupSettings);
            this.filterElementCalendar1.SetElementBase(eb, groupSettings);

            BindVariableComboBox();
            BindVersionComboBox();
            BindValueTypeComboBox();
            BindTimeComboBox();
            if (eb.manager.readOnly)
            {
                cboVariables.Enabled = false;
                cboVersions.Enabled = false;
                cboValueTypes.Enabled = false;
                cboTime.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            this.useVariable1 = eb.loadFromVariable1;
            this.useVariable2 = eb.loadFromVariable2;
            if (this.useVariable1)
            {
                if (eb.manager.currentCondition.lastVariable1_VersionIndex != null)
                {
                    SharedControlRoutines.SetUltraComboValue(this.cboVersions, (int)eb.manager.currentCondition.lastVariable1_VersionIndex);
                }
                if (eb.manager.currentCondition.lastVariable1_VariableValueTypeIndex != null)
                {
                    SharedControlRoutines.SetUltraComboValue(this.cboValueTypes, (int)eb.manager.currentCondition.lastVariable1_VariableValueTypeIndex);
                }
                if (eb.manager.currentCondition.lastVariable1_TimeTypeIndex != null)
                {
                    SharedControlRoutines.SetUltraComboValue(this.cboTime, (int)eb.manager.currentCondition.lastVariable1_TimeTypeIndex);
                }
            }
            else
            {
                if (eb.manager.currentCondition.lastVariable2_VersionIndex != null)
                {
                    SharedControlRoutines.SetUltraComboValue(this.cboVersions, (int)eb.manager.currentCondition.lastVariable2_VersionIndex);
                }
                if (eb.manager.currentCondition.lastVariable2_VariableValueTypeIndex != null)
                {
                    SharedControlRoutines.SetUltraComboValue(this.cboValueTypes, (int)eb.manager.currentCondition.lastVariable2_VariableValueTypeIndex);
                }
                if (eb.manager.currentCondition.lastVariable2_TimeTypeIndex != null)
                {
                    SharedControlRoutines.SetUltraComboValue(this.cboTime, (int)eb.manager.currentCondition.lastVariable2_TimeTypeIndex);
                }
               
            }
         


            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
            //clear groups created by child elements first
            foreach (elementBase b in fieldElementList)
            {
                b.elementInterface.ClearControls();
            }
            //clear newly adding groups from the list and from the container
            if (this.removeDynamicElementsForFieldDelegate != null)
            {
                this.removeDynamicElementsForFieldDelegate(keyListToRemove);
            }
        }

        public void BindVariableComboBox()
        {
            DataTable dt = groupSettings.loadFieldList().Copy();
            this.cboVariables.DataSource = dt;
        }

        public void BindVersionComboBox()
        {
            this.cboVersions.DataSource = filterDataHelper.VersionsGetDataTable().Copy();
        }
        public void BindValueTypeComboBox()
        {
            this.cboValueTypes.DataSource = variableValueTypes.ToDataTable().Copy();
        }
        private DataTable dtTimeTypes;
        public void BindTimeComboBox()
        {
            dtTimeTypes = variableTimeTypes.ToDataTable().Copy();
            this.cboTime.DataSource = dtTimeTypes;
        }


        private bool useVariable1;
        private bool useVariable2;

        public void LoadFromCondition(filter f, filterCondition condition)
        {

            this.useVariable1 = eb.loadFromVariable1;
            this.useVariable2 = eb.loadFromVariable2;
            this.filterElementMerchandise1.LoadFromCondition(f, condition);
            this.filterElementCalendar1.LoadFromCondition(f, condition);


            this.lblVariable.Text = eb.groupHeading;

            if (useVariable1)
            {
                SharedControlRoutines.SetUltraComboValue(this.cboVariables, condition.variable1_Index);
                SharedControlRoutines.SetUltraComboValue(this.cboVersions, condition.variable1_VersionIndex);
                SharedControlRoutines.SetUltraComboValue(this.cboValueTypes, condition.variable1_VariableValueTypeIndex);
                eb.manager.currentCondition.lastVariable1_VersionIndex = condition.variable1_VersionIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                eb.manager.currentCondition.lastVariable1_VariableValueTypeIndex = condition.variable1_VariableValueTypeIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables


                //Remove Corresponding time type
                DataRow[] drCorresponding = dtTimeTypes.Select("FIELD_INDEX=" + variableTimeTypes.Corresponding.dbIndex.ToString());
                if (drCorresponding.Length > 0)
                {
                    dtTimeTypes.Rows.Remove(drCorresponding[0]);
                }

                SharedControlRoutines.SetUltraComboValue(this.cboTime, condition.variable1_TimeTypeIndex);
                eb.manager.currentCondition.lastVariable1_TimeTypeIndex = condition.variable1_TimeTypeIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables


                if (eb.useDynamicOperator)
                {

                    foreach (elementBase b in fieldElementList)
                    {
                        b.LoadFromCondition(condition);
                    }
                }

            }
            else if (useVariable2)
            {
                SharedControlRoutines.SetUltraComboValue(this.cboVariables, condition.variable2_Index);
                SharedControlRoutines.SetUltraComboValue(this.cboVersions, condition.variable2_VersionIndex);
                SharedControlRoutines.SetUltraComboValue(this.cboValueTypes, condition.variable2_VariableValueTypeIndex);
                SharedControlRoutines.SetUltraComboValue(this.cboTime, condition.variable2_TimeTypeIndex);
                eb.manager.currentCondition.lastVariable2_VersionIndex = condition.variable2_VersionIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                eb.manager.currentCondition.lastVariable2_VariableValueTypeIndex = condition.variable2_VariableValueTypeIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                eb.manager.currentCondition.lastVariable2_TimeTypeIndex = condition.variable2_TimeTypeIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables

            }

        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return this.filterElementMerchandise1.IsValid(f, condition);
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            this.filterElementMerchandise1.SaveToCondition(ref f, ref condition);
            this.filterElementCalendar1.SaveToCondition(ref f, ref condition);
            if (useVariable1)
            {
                condition.variable1_Index = (int)this.cboVariables.Value;
                //condition.variable1_IsTimeTotal = filterDataHelper.VariablesGetIsTimeTotal(condition.variable1_Index);

                filterDataTypes dataType = groupSettings.GetDataTypeFromFieldIndex(condition.variable1_Index);
                condition.valueTypeIndex = dataType.valueType;
                condition.variable1_VersionIndex = (int)this.cboVersions.Value;
                //condition.variable1_HN_RID = this.txtMerchandise.Text;
                //condition.variable1_CDR_RID = this.txtDateRange.Text;
                condition.variable1_VariableValueTypeIndex = (int)this.cboValueTypes.Value;
                condition.variable1_TimeTypeIndex = (int)this.cboTime.Value;

                if (eb.useDynamicOperator)
                {
                    foreach (elementBase b in fieldElementList)
                    {
                        b.SaveToCondition(ref condition);
                    }
                }
            }
            else if (useVariable2)
            {
                condition.variable2_Index = (int)this.cboVariables.Value;
                //condition.variable2_IsTimeTotal = filterDataHelper.VariablesGetIsTimeTotal(condition.variable2_Index);
                filterDataTypes dataType = groupSettings.GetDataTypeFromFieldIndex(condition.variable2_Index);
                condition.valueTypeIndex = dataType.valueType;

                condition.variable2_VersionIndex = (int)this.cboVersions.Value;
                //condition.variable2_HN_RID = this.txtMerchandise.Text;
                //condition.variable2_CDR_RID = this.txtDateRange.Text;
                condition.variable2_VariableValueTypeIndex = (int)this.cboValueTypes.Value;
                condition.variable2_TimeTypeIndex = (int)this.cboTime.Value;
            }
        }

        private void cboVariable_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            if (useVariable1 && cboVariables.Value != null) //Handle numeric types for variable to constant
            {
                int variableIndex = (int)cboVariables.Value;

            
                filterDataTypes vInfo = groupSettings.GetDataTypeFromFieldIndex(variableIndex);
                //eb.numericType = vInfo.numericType;
                if (eb.useDynamicOperator)
                {
                    Handle_DataTypeChanged(vInfo, variableIndex);
                }
        
            }


        }

        private filterDataTypes currentDataType = null;
        private List<elementBase> fieldElementList = new List<elementBase>();
        private List<string> keyListToRemove = new List<string>();
        private void Handle_DataTypeChanged(filterDataTypes newDataType, int fieldIndex)
        {
            if (currentDataType == null || newDataType != currentDataType)
            {
                MakeGroupsForDataType(newDataType, fieldIndex);
                currentDataType = newDataType;
            }
        }
        private void MakeGroupsForDataType(filterDataTypes dataType, int fieldIndex)
        {
            ClearControls();

            fieldElementList.Clear();
            keyListToRemove.Clear();
            int index = 0;
            //add new groups based on type
            if (dataType.valueType == filterValueTypes.Text)
            {
                //add text operator
                elementBase bOperator = new elementBase(eb.manager, filterElementMap.OperatorString, "String Operator");
                bOperator.dataType = dataType;
                fieldElementList.Add(bOperator);

                string keyOperator = "ve" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(keyOperator, bOperator, false, fieldIndex);
                }
                keyListToRemove.Add(keyOperator);
                index++;


                //add text value element
                elementBase bStringValue = new elementBase(eb.manager, filterElementMap.ValueToCompareString, "String Value");
                bStringValue.dataType = dataType;
                fieldElementList.Add(bStringValue);

                string keyStringValue = "vecs" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(keyStringValue, bStringValue, false, fieldIndex);
                }
                keyListToRemove.Add(keyStringValue);
                index++;
            }
            else if (dataType.valueType == filterValueTypes.Numeric || dataType.valueType == filterValueTypes.Dollar)
            {
                //add numeric operator
                elementBase b = new elementBase(eb.manager, filterElementMap.OperatorNumeric, "Numeric Operator");
                b.isOperatorNumeric = true;
                b.dataType = dataType;
                fieldElementList.Add(b);

                string key = "veno" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, fieldIndex);
                }
                keyListToRemove.Add(key);
                index++;
            }
            else if (dataType.valueType == filterValueTypes.Boolean)
            {
                //add boolean element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareBool, "Boolean Value");
                b.dataType = dataType;
                fieldElementList.Add(b);

                string key = "vebv" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, fieldIndex);
                }
                keyListToRemove.Add(key);
                index++;
            }
            else if (dataType.valueType == filterValueTypes.Date)
            {
                //add date operator
                elementBase b = new elementBase(eb.manager, filterElementMap.OperatorDate, "Operator:");
                b.isOperatorDate = true;
                b.dataType = dataType;
                fieldElementList.Add(b);

                string key = "vedo" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, fieldIndex);
                }
                keyListToRemove.Add(key);
                index++;
            }
            else if (dataType.valueType == filterValueTypes.List)
            {
                //add list operator
                elementBase bOperator = new elementBase(eb.manager, filterElementMap.OperatorIn, "In / Not In");
                bOperator.dataType = dataType;
                fieldElementList.Add(bOperator);

                string keyOperator = "velo" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(keyOperator, bOperator, false, fieldIndex);
                }
                keyListToRemove.Add(keyOperator);
                index++;

                //add list element
                elementBase bList = new elementBase(eb.manager, filterElementMap.List, groupSettings.GetNameFromField(fieldIndex));
                bList.isList = true;
                bList.dataType = dataType;
                fieldElementList.Add(bList);

                string key = "vel" + index.ToString(); //ve=>variable element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, bList, true, fieldIndex);
                }
                keyListToRemove.Add(key);
                index++;
            }

        }


        private void cboVersion_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false && this.cboVersions.Value != null)
            {
                if (this.useVariable1)
                {
                    eb.manager.currentCondition.lastVariable1_VersionIndex = (int)this.cboVersions.Value;
                }
                else
                {
                    eb.manager.currentCondition.lastVariable2_VersionIndex = (int)this.cboVersions.Value;
                }
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        private void txtMerchandise_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
       
        }
        private void txtDateRange_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }
        private void cboValueType_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false && this.cboValueTypes.Value != null)
            {
                if (this.useVariable1)
                {
                    eb.manager.currentCondition.lastVariable1_VariableValueTypeIndex = (int)this.cboValueTypes.Value;
                }
                else
                {
                    eb.manager.currentCondition.lastVariable2_VariableValueTypeIndex = (int)this.cboValueTypes.Value;
                }
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        private void cboTime_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false && this.cboTime.Value != null)
            {
                if (this.useVariable1)
                { 
                    eb.manager.currentCondition.lastVariable1_TimeTypeIndex = (int)this.cboTime.Value;
                }
                else
                {
                    eb.manager.currentCondition.lastVariable2_TimeTypeIndex = (int)this.cboTime.Value;
                }
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }

        private void cboVariables_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboVariables, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }

        private void cboVersions_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboVersions, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }

        private void cboValueTypes_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboValueTypes, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }

        private void cboTime_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboTime, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }

      
    }
}
