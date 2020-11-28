using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementOperatorNumeric : UserControl, IFilterElement
    {
        public filterElementOperatorNumeric()
        {
            InitializeComponent();
        }


        private elementBase eb;
        private filterEntrySettings groupSettings;

        //delegates to access the container
        public FilterMakeElementInGroupDelegate makeElementInGroupDelegate;
        public FilterRemoveDynamicElementsForFieldDelegate removeDynamicElementsForFieldDelegate;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.groupSettings = groupSettings;
            BindComboBox();
            if (eb.manager.readOnly)
            {
                cboOperators.Enabled = false;
            }
        }
        public void ClearControls()
        {
            //clear newly adding groups from the list and from the container
            if (this.removeDynamicElementsForFieldDelegate != null)
            {
                this.removeDynamicElementsForFieldDelegate(keyListToRemove);
            }
        }
        DataTable dtNumericOp;
        private void BindComboBox()
        {
            dtNumericOp = filterNumericOperatorTypes.ToDataTable().Copy();
            this.cboOperators.DataSource = dtNumericOp;

        }
        public void SetDefault()
        {
            eb.isLoading = true;

            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.manager.currentCondition.lastOperatorIndexNumeric != null)
            {
                this.cboOperators.Value = (int)eb.manager.currentCondition.lastOperatorIndexNumeric;
            }
            else
            {
                this.cboOperators.Value = filterNumericOperatorTypes.DoesEqual.dbIndex;
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
    
            eb.isLoading = false;
        }
        //private valueTypes valueType;
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboOperators.Value = condition.operatorIndex;
            eb.manager.currentCondition.lastOperatorIndexNumeric = condition.operatorIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            foreach (elementBase b in fieldElementList)
            {
                b.LoadFromCondition(condition);
            }
            //valueType = valueTypes.FromIndex(condition.valueTypeIndex);
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            eb.SaveDataTypeToCondition(ref condition, eb.dataType);
            condition.operatorIndex = (int)cboOperators.Value;
       
            foreach (elementBase b in fieldElementList)
            {
                b.SaveToCondition(ref condition);
            }
        }

        private void cboOperators_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();


            if (cboOperators.Value != null)
            {
                int opIndex = (int)cboOperators.Value;

                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    eb.manager.currentCondition.lastOperatorIndexNumeric = opIndex;
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                Handle_OpTypeChanged(filterNumericOperatorTypes.FromIndex(opIndex));
            }
        }

        private filterNumericOperatorTypes currentOpType = null;
        private List<elementBase> fieldElementList = new List<elementBase>();
        private List<string> keyListToRemove = new List<string>();
        private void Handle_OpTypeChanged(filterNumericOperatorTypes newOpType)
        {
            if (currentOpType == null || (newOpType == filterNumericOperatorTypes.Between && currentOpType != filterNumericOperatorTypes.Between) || (newOpType != filterNumericOperatorTypes.Between && currentOpType == filterNumericOperatorTypes.Between))
            {
                MakeGroupsForOpType(newOpType);
                currentOpType = newOpType;
            }
        }
        private void MakeGroupsForOpType(filterNumericOperatorTypes opType)
        {
            ClearControls();

            fieldElementList.Clear();

            //if (eb.numericType == null)
            //{
            //    //TODO - Fix for variables.
            //    eb.numericType = numericTypes.Integer;
            //}

            //add new groups based on type
            if (opType == filterNumericOperatorTypes.Between)
            {
                //add between value element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareNumericBetween, "Numeric Value");
                b.dataType = eb.dataType;
                fieldElementList.Add(b);
            }
            else
            {
                //add single value element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareNumeric, "Numeric Value");
                b.dataType = eb.dataType;
                fieldElementList.Add(b);

            }

            keyListToRemove.Clear();
            int index = 0;
            foreach (elementBase b in fieldElementList)
            {
                string key = "ne" + index.ToString();
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                }
                keyListToRemove.Add(key);
                index++;
            }
        }

        private void cboOperators_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboOperators, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }
}
