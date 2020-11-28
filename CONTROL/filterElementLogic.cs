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
    public partial class filterElementLogic : UserControl, IFilterElement
    {
        public filterElementLogic()
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
                cboLogic.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }
        private void BindComboBox()
        {
            this.cboLogic.DataSource = filterLogicTypes.ToDataTable().Copy();
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboLogic.Value = condition.logicIndex;
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (cboLogic.Value != null)
            {
                condition.logicIndex = (int)cboLogic.Value;
            }
        }

        private void cboLogic_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void cboLogic_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboLogic, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }
}
