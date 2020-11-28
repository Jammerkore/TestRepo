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
    public partial class ForecastAnalysisDateOptionsControl : UserControl
    {
        public ForecastAnalysisDateOptionsControl()
        {
            InitializeComponent();
        }
        public void LoadData()
        {
           
        }

        public void GetOptions(ref ReportData.ForecastAnalysisEventArgs e)
        {
         
           

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

            if ((string)osForecastDates.CheckedItem.DataValue == "DateRange")
            {
                DateTime startDate = (DateTime)this.dteForecastFromDate.Value;
                //DateTime startTime = (DateTime)this.dteFromTime.Value;
                e.ForecastStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);


                DateTime endDate = (DateTime)this.dteForecastToDate.Value;
                //DateTime endTime = (DateTime)this.dteToTime.Value;
                e.ForecastEndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);
                e.useForecastDateRange = true;
            }
            else if ((string)osForecastDates.CheckedItem.DataValue == "Last_7_Days")
            {
                e.ForecastEndDate = DateTime.Now;
                //BEGIN TT#3581 - DOConnell - Forecast Analysis - Filter Error
                //e.ForecastStartDate = e.endDate.AddDays(-7);
                e.ForecastStartDate = e.ForecastEndDate.AddDays(-7);
                //END TT#3581 - DOConnell - Forecast Analysis - Filter Error
                e.useForecastDateRange = true;
            }
            else if ((string)osForecastDates.CheckedItem.DataValue == "Last_24_Hours")
            {
                e.ForecastEndDate = DateTime.Now;
                //BEGIN TT#3581 - DOConnell - Forecast Analysis - Filter Error
                //e.ForecastStartDate = e.endDate.AddHours(-24);
                e.ForecastStartDate = e.ForecastEndDate.AddHours(-24);
                //END TT#3581 - DOConnell - Forecast Analysis - Filter Error
                e.useForecastDateRange = true;
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

        private void osForecastDates_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osForecastDates.CheckedItem.DataValue == "DateRange")
            {
                this.dteForecastFromDate.Enabled = true;
                
                this.dteForecastToDate.Enabled = true;
                
            }
            else
            {
                this.dteForecastFromDate.Enabled = false;

                this.dteForecastToDate.Enabled = false;
                
            }
        }

       
    }
}
