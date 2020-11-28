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
    public partial class filterElementDynamicSet : UserControl, IFilterElement
    {
        public filterElementDynamicSet()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.cboSet.DataSource = groupSettings.loadValueList();
            this.cboDirection.DataSource = filterSortByDirectionTypes.ToDataTable().Copy();
   
            eb.isLoading = true;
            this.cboDirection.Value = filterSortByDirectionTypes.Ascending.dbIndex;
            eb.isLoading = false;
    
            if (eb.manager.readOnly)
            {
                cboSet.Enabled = false;
                cboDirection.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }



        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboSet.Value = condition.operatorIndex;
            this.cboDirection.Value = condition.valueToCompareInt2;
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (cboSet.Value != null)
            {
                condition.operatorIndex = (int)cboSet.Value;             
                if (cboSet.SelectedRow.ListObject != null)
                {
                    DataRow drSelected = ((DataRowView)cboSet.SelectedRow.ListObject).Row;
                    condition.valueToCompareInt = (int)drSelected["OBJECT_TYPE"];  
                }
              
            }
            if (cboDirection.Value != null)
            {
                condition.valueToCompareInt2 = (int)cboDirection.Value;
            }
        }

        private void cboSet_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void cboSet_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboSet, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }

        private void cboDirection_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void cboDirection_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboDirection, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }

}
