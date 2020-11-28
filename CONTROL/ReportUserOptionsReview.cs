using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{
    public partial class ReportUserOptionsReview : UserControl
    {
        public ReportUserOptionsReview()
        {
            InitializeComponent();
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "reportView":
                    ViewReport();
                    break;

            }
        }


        private void ViewReport()
        {
            int auditLevelToReport = -1; //will show all audit levels unless the user specifies an audit level
            if (this.chkSpecifyAuditLevel.Checked == true)
            {
                auditLevelToReport = Convert.ToInt32(cbxAuditLoggingLevel.SelectedValue);
            }

            string forecastMonitorToReport = "."; //will show all forcast monitor settings
            if (this.chkForecast.Checked == true)
            {
                if (this.radForecastMonitorOn.Checked == true)
                {
                    forecastMonitorToReport = "1";
                }
                else
                {
                    forecastMonitorToReport = "0";
                }  
            }

            string salesMonitorToReport = "."; //will show all forcast monitor settings
            if (this.chkSales.Checked == true)
            {
                if (this.radModifySalesMonitorOn.Checked == true)
                {
                    salesMonitorToReport = "1";
                }
                else
                {
                    salesMonitorToReport = "0";
                }
            }

            string dcfulfillmentMonitorToReport = "."; //will show all forcast monitor settings
            if (this.chkDCFulfillment.Checked == true)
            {
                if (this.radDCFulfillmentMonitorOn.Checked == true)
                {
                    dcfulfillmentMonitorToReport = "1";
                }
                else
                {
                    dcfulfillmentMonitorToReport = "0";
                }
            }

            RaiseViewReportEvent(auditLevelToReport, forecastMonitorToReport, salesMonitorToReport, dcfulfillmentMonitorToReport);
        }
        public event ViewReportEventHandler ViewReportEvent;
        public virtual void RaiseViewReportEvent(int auditLoggingLevel, string forecastMonitor, string salesMonitor, string dcfulfillmentMonitor)
        {
            if (ViewReportEvent != null)
                ViewReportEvent(this, new ViewReportEventArgs(auditLoggingLevel, forecastMonitor, salesMonitor, dcfulfillmentMonitor));
        }
        public class ViewReportEventArgs
        {
            public ViewReportEventArgs(int auditLoggingLevel, string forecastMonitor, string salesMonitor, string dcfulfillmentMonitor) { this.auditLoggingLevel = auditLoggingLevel; this.forecastMonitor = forecastMonitor; this.salesMonitor = salesMonitor; this.dcfulfillmentMonitor = dcfulfillmentMonitor; }
            public int auditLoggingLevel { get; private set; } // readonly
            public string forecastMonitor { get; private set; } // readonly
            public string salesMonitor { get; private set; } // readonly
            public string dcfulfillmentMonitor { get; private set; } // readonly
        }
        public delegate void ViewReportEventHandler(object sender, ViewReportEventArgs e);

        private void chkSpecifyAuditLevel_CheckedChanged(object sender, EventArgs e)
        {
            this.cbxAuditLoggingLevel.Enabled = this.chkSpecifyAuditLevel.Checked;
        }

        private void ReportUserOptionsReview_Load(object sender, EventArgs e)
        {
            //this.lblAuditLoggingLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AuditLoggingLevel);
            //LoadLoggingLevels();
        }
        public void LoadLoggingLevels()
        {
            this.lblAuditLoggingLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AuditLoggingLevel);
            cbxAuditLoggingLevel.Items.Clear();
            DataTable dt = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);
            cbxAuditLoggingLevel.DataSource = dt;
            cbxAuditLoggingLevel.DisplayMember = "TEXT_VALUE";
            cbxAuditLoggingLevel.ValueMember = "TEXT_CODE";
        }

        private void chkForecast_CheckedChanged(object sender, EventArgs e)
        {
            radForecastMonitorOn.Enabled = chkForecast.Checked;
            radForecastMonitorOff.Enabled = chkForecast.Checked;
        }

        private void chkSales_CheckedChanged(object sender, EventArgs e)
        {
            radModifySalesMonitorOn.Enabled = chkSales.Checked;
            radModifySalesMonitorOff.Enabled = chkSales.Checked;
        }

        private void chkDCFulfillment_CheckedChanged(object sender, EventArgs e)
        {
            radDCFulfillmentMonitorOn.Enabled = chkDCFulfillment.Checked;
            radDCFulfillmentMonitorOff.Enabled = chkDCFulfillment.Checked;
        }


    }
}
