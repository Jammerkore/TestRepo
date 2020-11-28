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
    public partial class filterElementSortByDirection : UserControl, IFilterElement
    {
        public filterElementSortByDirection()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            BindComboBox();
            //Begin TT#1388-MD -jsobek -Product Filters
            eb.isLoading = true;
            this.cboDirection.Value = filterSortByDirectionTypes.Ascending.dbIndex;
            eb.isLoading = false;
            //End TT#1388-MD -jsobek -Product Filters
            if (eb.manager.readOnly)
            {
                cboDirection.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }
        private void BindComboBox()
        {
            this.cboDirection.DataSource = filterSortByDirectionTypes.ToDataTable().Copy();
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboDirection.Value = condition.operatorIndex;
            
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (cboDirection.Value != null)
            {
                condition.operatorIndex = (int)cboDirection.Value;
            }
        }

        private void cboOperators_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void cboDirection_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboDirection, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }

}
