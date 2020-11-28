using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;
using MIDRetail.DataCommon;     // TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementValueToCompareDateSpecify : UserControl, IFilterElement
    {
        public filterElementValueToCompareDateSpecify()
        {
            InitializeComponent();


        }
        private bool isInitializing = false;
        private void filterElementValueToCompareDateSpecify_Load(object sender, EventArgs e)
        {
            isInitializing = true;
            DateTime dtNow = DateTime.Now;
            this.dteFromDate.Value = dtNow;
            this.dteFromTime.Value = dtNow;
            this.dteToDate.Value = dtNow;
            this.dteToTime.Value = dtNow;
            // Begin TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
            eb.manager.currentCondition.lastValueToCompareDateFrom = dtNow;
            eb.manager.currentCondition.lastValueToCompareDateTo = dtNow;
            // End TT#1407-MD
            isInitializing = false;
        }

        private elementBase eb;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                dteFromDate.Enabled = false;
                dteToDate.Enabled = false;
                dteFromTime.Enabled = false;
                dteToTime.Enabled = false;
            }
            if (eb.dataType.dateType == filterDateTypes.DateOnly)
            {
                this.dteFromTime.Visible = false;
                this.dteToTime.Visible = false;
                //this.dteFromDate.Left += 8;
                //this.dteToDate.Left += 8;
                //lblFrom.Left += 8;
                //lblTo.Left += 8;
            }
            else if (eb.dataType.dateType == filterDateTypes.TimeOnly)
            {
                this.dteFromDate.Visible = false;
                this.dteToDate.Visible = false;
                this.dteFromTime.Left = 75;
                this.dteToTime.Left = 75;
                //lblFrom.Left += 8;
                //lblTo.Left += 8;
            }

        }
        //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        public void SetDefault()
        {
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastValueToCompareDateFrom != null)
            {
                DateTime startDate = (DateTime)eb.manager.currentCondition.lastValueToCompareDateFrom;
                DateTime startTime = (DateTime)eb.manager.currentCondition.lastValueToCompareDateFrom;

                this.dteFromDate.Value = startDate;
                this.dteFromTime.Value = startTime;
            }
            if (eb.manager.currentCondition.lastValueToCompareDateTo != null)
            {
                DateTime endDate = (DateTime)eb.manager.currentCondition.lastValueToCompareDateTo;
                DateTime endTime = (DateTime)eb.manager.currentCondition.lastValueToCompareDateTo;

                this.dteToDate.Value = endDate;
                this.dteToTime.Value = endTime;
            }


            eb.isLoading = false;
        }
        //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables

        public void ClearControls()
        {

        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            DateTime dtNow = DateTime.Now;
            if (condition.valueToCompareDateFrom != null)
            {
                DateTime startDate = (DateTime)condition.valueToCompareDateFrom;
                DateTime startTime = (DateTime)condition.valueToCompareDateFrom;

                this.dteFromDate.Value = startDate;
                this.dteFromTime.Value = startTime;
                eb.manager.currentCondition.lastValueToCompareDateFrom = startDate;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
            else
            {
                this.dteFromDate.Value = dtNow;
                this.dteFromTime.Value = dtNow;
            }

            if (condition.valueToCompareDateTo != null)
            {
                DateTime endDate = (DateTime)condition.valueToCompareDateTo;
                DateTime endTime = (DateTime)condition.valueToCompareDateTo;

                this.dteToDate.Value = endDate;
                this.dteToTime.Value = endTime;
                eb.manager.currentCondition.lastValueToCompareDateTo = endDate;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
            else
            {
                this.dteToDate.Value = dtNow;
                this.dteToTime.Value = dtNow;
            }

            //if (dateTypes.FromIndex(condition.dateTypeIndex) == dateTypes.DateOnly)
            //{
            //    this.dteFromTime.Visible = false;
            //    this.dteToTime.Visible = false;
            //}
            //else if (dateTypes.FromIndex(condition.dateTypeIndex) == dateTypes.TimeOnly)
            //{
            //    this.dteFromDate.Visible = false;
            //    this.dteToDate.Visible = false;
            //}
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            // Begin TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
            if (condition.lastValueToCompareDateFrom > condition.lastValueToCompareDateTo)
            {
                MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_FromDateError), f.filterName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // End TT#1407-MD
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            DateTime startDate = (DateTime)this.dteFromDate.Value;
            DateTime startTime = (DateTime)this.dteFromTime.Value;

            condition.valueToCompareDateFrom = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);

            DateTime endDate = (DateTime)this.dteToDate.Value;
            DateTime endTime = (DateTime)this.dteToTime.Value;
            condition.valueToCompareDateTo = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);

            // Begin TT#4607 - JSmith - Specify date range not selecting headers released on from date
            if (eb.dataType.dateType == filterDateTypes.DateAndTime)
            {
                condition.valueToCompareBool = true;
            }
            // End TT#4607 - JSmith - Specify date range not selecting headers released on from date
        }

        private void dteFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (isInitializing == false)
            {
                eb.MakeConditionDirty();
                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    DateTime startDate = (DateTime)this.dteFromDate.Value;
                    DateTime startTime = (DateTime)this.dteFromTime.Value;

                    eb.manager.currentCondition.lastValueToCompareDateFrom = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
        }

        private void dteFromTime_ValueChanged(object sender, EventArgs e)
        {
            if (isInitializing == false)
            {
                eb.MakeConditionDirty();
                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    DateTime startDate = (DateTime)this.dteFromDate.Value;
                    DateTime startTime = (DateTime)this.dteFromTime.Value;

                    eb.manager.currentCondition.lastValueToCompareDateFrom = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
        }

        private void dteToDate_ValueChanged(object sender, EventArgs e)
        {
            if (isInitializing == false)
            {
                eb.MakeConditionDirty();
                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    DateTime endDate = (DateTime)this.dteToDate.Value;
                    DateTime endTime = (DateTime)this.dteToTime.Value;
                    eb.manager.currentCondition.lastValueToCompareDateTo = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
        }

        private void dteToTime_ValueChanged(object sender, EventArgs e)
        {
            if (isInitializing == false)
            {
                eb.MakeConditionDirty();
                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    DateTime endDate = (DateTime)this.dteToDate.Value;
                    DateTime endTime = (DateTime)this.dteToTime.Value;
                    eb.manager.currentCondition.lastValueToCompareDateTo = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
        }


    }
}
