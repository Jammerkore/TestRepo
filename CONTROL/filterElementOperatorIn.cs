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
    public partial class filterElementOperatorIn : UserControl, IFilterElement
    {
        public filterElementOperatorIn()
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
        }
        public void ClearControls()
        {
        }
        private void BindComboBox()
        {
            this.cboOperators.DataSource = filterListOperatorTypes.ToDataTable().Copy();
            eb.isLoading = true;
            this.cboOperators.Value = filterListOperatorTypes.Includes.dbIndex;
            eb.isLoading = false;
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboOperators.Value = condition.operatorIndex;
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (eb.dataType != null)
            {
                eb.SaveDataTypeToCondition(ref condition, eb.dataType);
            }
            if (cboOperators.Value != null)
            {
                condition.operatorIndex = (int)cboOperators.Value;
            }
        }

        private void cboOperators_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void cboOperators_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboOperators, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }
}
