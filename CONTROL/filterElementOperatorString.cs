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
    public partial class filterElementOperatorString : UserControl, IFilterElement
    {
        public filterElementOperatorString()
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

            if (eb.manager.currentCondition.lastOperatorIndexString != null)
            {
                this.cboOperators.Value = (int)eb.manager.currentCondition.lastOperatorIndexString;
            }

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        private void BindComboBox()
        {
            this.cboOperators.DataSource = filterStringOperatorTypes.ToDataTable().Copy();
            eb.isLoading = true;
            this.cboOperators.Value = filterStringOperatorTypes.Contains.dbIndex;
            eb.isLoading = false;
        }

        public void LoadFromCondition(filter f, filterCondition condition)
        {

            this.cboOperators.Value = (int)condition.operatorIndex;
            eb.manager.currentCondition.lastOperatorIndexString = condition.operatorIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
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
                    eb.manager.currentCondition.lastOperatorIndexString = (int)cboOperators.Value;
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
