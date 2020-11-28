using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using MIDRetail.Data;           // Begin TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
using MIDRetail.DataCommon;     // End TT#1407-MD
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementValueToCompareDateBetween : UserControl, IFilterElement
    {
        public filterElementValueToCompareDateBetween()
        {
            InitializeComponent();
        }

        private elementBase eb;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                daysFrom.Enabled = false;
                daysTo.Enabled = false;
                this.chkTimeSensitive.Enabled = false; //TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastValueToCompareDateBetweenFromDays != null)
            {
                int daysFrom = eb.manager.currentCondition.lastValueToCompareDateBetweenFromDays;
                this.daysFrom.Value = daysFrom;
            }
            if (eb.manager.currentCondition.lastValueToCompareDateBetweenToDays != null)
            {
                int daysTo = eb.manager.currentCondition.lastValueToCompareDateBetweenToDays;
                this.daysTo.Value = daysTo;
            }
            if (eb.manager.currentCondition.lastValueToCompareBool != null) //TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            {
                this.chkTimeSensitive.Checked = (bool)eb.manager.currentCondition.lastValueToCompareBool;
            }

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            int daysFrom = condition.valueToCompareDateBetweenFromDays;
            int daysTo = condition.valueToCompareDateBetweenToDays;
            this.daysFrom.Value = daysFrom;
            this.daysTo.Value = daysTo;
            eb.manager.currentCondition.lastValueToCompareDateBetweenFromDays = daysFrom;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.manager.currentCondition.lastValueToCompareDateBetweenToDays = daysTo;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables

            //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            if (condition.valueToCompareBool != null && (bool)condition.valueToCompareBool)
            {
                this.chkTimeSensitive.Checked = true;
                eb.manager.currentCondition.lastValueToCompareBool = true;
            }
            else
            {
                this.chkTimeSensitive.Checked = false;
                eb.manager.currentCondition.lastValueToCompareBool = false;
            }
            //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            // Begin TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
            if (condition.lastValueToCompareDateBetweenFromDays > condition.lastValueToCompareDateBetweenToDays)
            {
                MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_FromDateError), f.filterName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // End TT#1407-MD
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            DateTime dtNow = DateTime.Now;
            int daysFrom = (int)this.daysFrom.Value;
            int daysTo = (int)this.daysTo.Value;
            condition.valueToCompareDateBetweenFromDays = daysFrom;
            condition.valueToCompareDateBetweenToDays = daysTo;
            condition.valueToCompareDateFrom = dtNow.AddDays(daysFrom);
            condition.valueToCompareDateTo = dtNow.AddDays(daysTo);
            //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            if (this.chkTimeSensitive.Checked)
            {
                condition.valueToCompareBool = true;
            }
            else
            {
                condition.valueToCompareBool = false;
            }
            //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
        }

        private void daysFrom_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                DateTime dtNow = DateTime.Now;
                int daysFrom = (int)this.daysFrom.Value;
                eb.manager.currentCondition.lastValueToCompareDateBetweenFromDays = daysFrom;
                eb.manager.currentCondition.lastValueToCompareDateFrom = dtNow.AddDays(daysFrom);
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        private void daysTo_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                DateTime dtNow = DateTime.Now;   
                int daysTo = (int)this.daysTo.Value;
                eb.manager.currentCondition.lastValueToCompareDateBetweenToDays = daysTo;
                eb.manager.currentCondition.lastValueToCompareDateTo = dtNow.AddDays(daysTo);
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }

        private void chkTimeSensitive_CheckedChanged(object sender, EventArgs e)
        {
            //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            eb.MakeConditionDirty();
            if (eb.isLoading == false)
            {
                eb.manager.currentCondition.lastValueToCompareBool = this.chkTimeSensitive.Checked;
            }
            //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
        }
    }
}
