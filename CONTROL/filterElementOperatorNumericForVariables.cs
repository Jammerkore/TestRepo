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
    public partial class filterElementOperatorNumericForVariables : UserControl, IFilterElement
    {
        public filterElementOperatorNumericForVariables()
        {
            InitializeComponent();
        }


        private elementBase eb;
        private filterEntrySettings groupSettings;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.groupSettings = groupSettings;
            BindComboBox();
            if (eb.manager.readOnly)
            {
                cboOperators.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastOperatorIndexNumericForVariable != null)
            {
                this.cboOperators.Value = (int)eb.manager.currentCondition.lastOperatorIndexNumericForVariable;
            }

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        DataTable dtNumericOp;
        private void BindComboBox()
        {
            dtNumericOp = filterNumericOperatorTypes.ToDataTable().Copy();

            //remove between operator for variables
            DataRow[] drBetween = dtNumericOp.Select("OPERATOR_INDEX=" + filterNumericOperatorTypes.Between.dbIndex.ToString());
            dtNumericOp.Rows.Remove(drBetween[0]);


            this.cboOperators.DataSource = dtNumericOp;
        }

        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboOperators.Value = condition.operatorIndex;
            eb.manager.currentCondition.lastOperatorIndexNumericForVariable = condition.operatorIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (cboOperators.Value != null)
            {
                condition.operatorIndex = (int)cboOperators.Value;
            }
        }

        private void cboOperators_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                if (cboOperators.Value != null)
                {
                    eb.manager.currentCondition.lastOperatorIndexNumericForVariable = (int)cboOperators.Value;
                }
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }

        private void cboOperators_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboOperators, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }


    }
}
