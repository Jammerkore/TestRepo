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
    public partial class filterElementSortBy : UserControl, IFilterElement
    {
        public filterElementSortBy()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            BindComboBox(groupSettings);
            if (eb.manager.readOnly)
            {
                cboField.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }
        private void BindComboBox(filterEntrySettings groupSettings)
        {
            DataTable dt = groupSettings.loadFieldList().Copy();
            this.cboField.DataSource = dt;
            eb.isLoading = true;
            this.cboField.Value = dt.Rows[0]["FIELD_INDEX"];
            eb.isLoading = false;
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {
            SharedControlRoutines.SetUltraComboValue(this.cboField, condition.sortByFieldIndex);
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (cboField.Value != null)
            {
                condition.sortByFieldIndex = (int)cboField.Value;
            }
        }

        private void cboField_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void cboField_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboField, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }
}
