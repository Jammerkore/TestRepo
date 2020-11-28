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
    public partial class filterElementValueToCompareString : UserControl, IFilterElement
    {
        public filterElementValueToCompareString()
        {
            InitializeComponent();

        }

        private elementBase eb;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                txtValueToCompare.Enabled = false;
            }


            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastValueToCompare != string.Empty)
            {
                this.txtValueToCompare.Text = eb.manager.currentCondition.lastValueToCompare;
            }     
            
            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables

        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.txtValueToCompare.Text = condition.valueToCompare;
            eb.manager.currentCondition.lastValueToCompare = condition.valueToCompare;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            condition.valueToCompare = txtValueToCompare.Text;
        }

        private void txtValueToCompare_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                eb.manager.currentCondition.lastValueToCompare = txtValueToCompare.Text;
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
    }
}
