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
    public partial class filterElementName : UserControl, IFilterElement
    {
        public filterElementName()
        {
            InitializeComponent();

        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                txtName.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.txtName.Text = condition.valueToCompare;
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            condition.valueToCompare = txtName.Text;
        }

        private void txtName_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }
    }
}
