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
    public partial class filterElementOperatorVariablePercentage : UserControl, IFilterElement
    {
        public filterElementOperatorVariablePercentage()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            BindComboBox();
            if (eb.manager.readOnly)
            {
                cboOperators.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastOperatorIndexVariablePercentage != null)
            {
                this.cboOperators.Value = (int)eb.manager.currentCondition.lastOperatorIndexVariablePercentage;
            }

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        private void BindComboBox()
        {
            this.cboOperators.DataSource = filterPercentageOperatorTypes.ToDataTable().Copy();
        }

        public void LoadFromCondition(filter f, filterCondition condition)
        {

            this.cboOperators.Value = condition.operatorVariablePercentageIndex;
            eb.manager.currentCondition.lastOperatorIndexVariablePercentage = condition.operatorVariablePercentageIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            eb.SaveDataTypeToCondition(ref condition, eb.dataType);
            if (cboOperators.Value != null)
            {
                condition.operatorVariablePercentageIndex = (int)cboOperators.Value;
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
                    eb.manager.currentCondition.lastOperatorIndexVariablePercentage = (int)cboOperators.Value;
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
