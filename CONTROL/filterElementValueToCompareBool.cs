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
    public partial class filterElementValueToCompareBool : UserControl, IFilterElement
    {
        public filterElementValueToCompareBool()
        {
            InitializeComponent();
        }

        private elementBase eb;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                cboBoolean.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastValueToCompareBool != null)
            {
                if (eb.manager.currentCondition.lastValueToCompareBool == true)
                {
                    this.cboBoolean.Text = "True";
                }
                else
                {
                    this.cboBoolean.Text = "False";
                }
            }

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            if (condition.valueToCompareBool == true)
            {
                this.cboBoolean.Text = "True";
                eb.manager.currentCondition.lastValueToCompareBool = true;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
            else
            {
                this.cboBoolean.Text = "False";
                eb.manager.currentCondition.lastValueToCompareBool = false;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (this.cboBoolean.Text == "True")
            {
                condition.valueToCompareBool = true;
            }
            else
            {
                condition.valueToCompareBool = false;
            }
            condition.valueToCompare = this.cboBoolean.Text;
            condition.operatorIndex = filterNumericOperatorTypes.DoesEqual;
        }

        private void cboBoolean_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                if (this.cboBoolean.Text == "True")
                {
                    eb.manager.currentCondition.lastValueToCompareBool = true;
                }
                else
                {
                    eb.manager.currentCondition.lastValueToCompareBool = false;
                }

            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }


    }
}
