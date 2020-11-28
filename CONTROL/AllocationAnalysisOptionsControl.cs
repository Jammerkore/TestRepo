using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class AllocationAnalysisOptionsControl : UserControl
    {
        public AllocationAnalysisOptionsControl()
        {
            InitializeComponent();
        }
        public void LoadData(SessionAddressBlock SAB)
        {
            this.selectSingleHierarchyNodeControl1.LoadData(SAB, SelectSingleHierarchyNodeControl.AllowedNodeLevel.Style); 
        }

        public void GetOptions(ref ReportData.AllocationAnalysisEventArgs e)
        {
            if ((string)osHeaders.CheckedItem.DataValue == "Selected")
            {
                e.restrictHeaders = true;
            }
            else
            {
                e.restrictHeaders = false;
            }

            if ((string)osMerchandise.CheckedItem.DataValue == "Restrict")
            {
                e.restrictToStyle_HN_RID = this.selectSingleHierarchyNodeControl1.GetNode();
            }

            if ((string)osAuditDates.CheckedItem.DataValue == "DateRange")
            {
                DateTime startDate = (DateTime)this.dteFromDate.Value;
                DateTime startTime = (DateTime)this.dteFromTime.Value;
                e.startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);


                DateTime endDate = (DateTime)this.dteToDate.Value;
                DateTime endTime = (DateTime)this.dteToTime.Value;
                e.endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                e.useDateRange = true;
            }
            else if ((string)osAuditDates.CheckedItem.DataValue == "Last_7_Days")
            {
                e.endDate = DateTime.Now;
                e.startDate = e.endDate.AddDays(-7);
                e.useDateRange = true;
            }
            else if ((string)osAuditDates.CheckedItem.DataValue == "Last_24_Hours")
            {
                e.endDate = DateTime.Now;
                e.startDate = e.endDate.AddHours(-24);
                e.useDateRange = true;
            }

            if ((string)osResultLimit.CheckedItem.DataValue == "Limit")
            {
                int resultLimit = -1;
                int.TryParse(this.txtResultLimit.Text, out resultLimit);
                e.resultLimit = resultLimit;
            }

        }

        private void osAuditDates_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osAuditDates.CheckedItem.DataValue == "DateRange")
            {
                this.dteFromDate.Enabled = true;
                this.dteFromTime.Enabled = true;
                this.dteToDate.Enabled = true;
                this.dteToTime.Enabled = true;
            }
            else
            {
                this.dteFromDate.Enabled = false;
                this.dteFromTime.Enabled = false;
                this.dteToDate.Enabled = false;
                this.dteToTime.Enabled = false;
            }
        }

        private void osMerchandise_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osMerchandise.CheckedItem.DataValue == "Restrict")
            {
                this.selectSingleHierarchyNodeControl1.Enabled = true;
            }
            else
            {
                this.selectSingleHierarchyNodeControl1.Enabled = false;
            }
        }

        private void osResultLimit_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osResultLimit.CheckedItem.DataValue == "Limit")
            {
                this.txtResultLimit.Enabled = true;
            }
            else
            {
                this.txtResultLimit.Enabled = false;
            }
        }
    }
}
