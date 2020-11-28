using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Windows.Controls;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows
{
    public partial class ReportUserOptionsReviewForm : Form
    {
        SessionAddressBlock _SAB;
        public ReportUserOptionsReviewForm(SessionAddressBlock sab)
        {
            _SAB = sab;
            InitializeComponent();
        }

        private void LoadForm(object sender, EventArgs e)
        {
            this.reportUserOptionsReview1.ViewReportEvent += new ReportUserOptionsReview.ViewReportEventHandler(ViewReport);
            this.reportUserOptionsReview1.LoadLoggingLevels(); 
        }
        private void ViewReport(object sender, ReportUserOptionsReview.ViewReportEventArgs e)
        {
            //Windows.CrystalReports.UserOptionsReview userOptionsReviewReport = new Windows.CrystalReports.UserOptionsReview();
            System.Data.DataSet userOptionsReviewDataSet = MIDEnvironment.CreateDataSet("UserOptionsReviewDataSet");
            ReportData reportData = new ReportData();
            reportData.UserOptionsReview_Report(userOptionsReviewDataSet, e.auditLoggingLevel, e.forecastMonitor, e.salesMonitor, e.dcfulfillmentMonitor);

            userOptionsReviewDataSet.Tables[0].Columns.Add("AUDIT_LOGGING_TEXT");
            userOptionsReviewDataSet.Tables[0].Columns.Add("FORECAST_MONITOR_TEXT");
            userOptionsReviewDataSet.Tables[0].Columns.Add("MODIFY_SALES_MONITOR_TEXT");
            userOptionsReviewDataSet.Tables[0].Columns.Add("DCFULFILLMENT_MONITOR_TEXT");

            foreach (System.Data.DataRow dr in userOptionsReviewDataSet.Tables[0].Rows)
            {


                int auditLoggingLevel = -1;
                if (dr["AUDIT_LOGGING_LEVEL"] != DBNull.Value)
                {
                    auditLoggingLevel = (int)dr["AUDIT_LOGGING_LEVEL"];
                }

                string forecastMonitorActive = "";
                if (dr["FORECAST_MONITOR_ACTIVE"] != DBNull.Value)
                {
                   forecastMonitorActive = (string)dr["FORECAST_MONITOR_ACTIVE"];
                }

                string salesMonitorActive = "";
                if (dr["MODIFY_SALES_MONITOR_ACTIVE"] != DBNull.Value)
                {
                    salesMonitorActive = (string)dr["MODIFY_SALES_MONITOR_ACTIVE"];
                }

                string dcfulfillmentMonitorActive = "";
                if (dr["DCFULFILLMENT_MONITOR_ACTIVE"] != DBNull.Value)
                {
                    dcfulfillmentMonitorActive = (string)dr["DCFULFILLMENT_MONITOR_ACTIVE"];
                }

                string auditLoggingLevelText = String.Empty;
                string forecastMonitorActiveText = String.Empty;
                string salesMonitorActiveText = String.Empty;
                string dcfulfillmentMonitorActiveText = String.Empty;

                string msgLevelText = "Not Set";

                try
                {
                    eMIDMessageLevel msgLevel = (eMIDMessageLevel)auditLoggingLevel;
                    msgLevelText =msgLevel.ToString();
                }
                catch
                {
                    msgLevelText = "Not Set";
                }
                if (msgLevelText == "-1")
                {
                    msgLevelText = "Not Set";
                }

                if (forecastMonitorActive == "0")
                {
                    forecastMonitorActiveText = "Off";
                }
                else if (forecastMonitorActive == "1")
                {
                    forecastMonitorActiveText = "On";
                }
                else
                {
                    forecastMonitorActiveText = "";
                }

                if (salesMonitorActive == "0")
                {
                    salesMonitorActiveText = "Off";
                }
                else if (salesMonitorActive == "1")
                {
                    salesMonitorActiveText = "On";
                }
                 else
                {
                    salesMonitorActiveText = "";
                }

                if (dcfulfillmentMonitorActive == "0")
                {
                    dcfulfillmentMonitorActiveText = "Off";
                }
                else if (dcfulfillmentMonitorActive == "1")
                {
                    dcfulfillmentMonitorActiveText = "On";
                }
                else
                {
                    dcfulfillmentMonitorActiveText = "";
                }

                dr["AUDIT_LOGGING_TEXT"] = msgLevelText;
                dr["FORECAST_MONITOR_TEXT"] = forecastMonitorActiveText;
                dr["MODIFY_SALES_MONITOR_TEXT"] = salesMonitorActiveText;
                dr["DCFULFILLMENT_MONITOR_TEXT"] = dcfulfillmentMonitorActiveText;
            }

            //userOptionsReviewReport.SetDataSource(userOptionsReviewDataSet);
            //userOptionsReviewReport.SetParameterValue("@DEFAULT_AUDIT_LEVEL", _SAB.ClientServerSession.Audit.GetDefaultLoggingLevel().ToString());  //Must set the parameter AFTER setting the data source

            //frmReportViewer viewer = new frmReportViewer(_SAB, eReportType.UserOptionsReview);
            List<ReportInfo> reports = new List<ReportInfo>();
            reports.Add(new ReportInfo(aReportSource: userOptionsReviewDataSet,
                reportType: eReportType.UserOptionsReview,
                reportName: "UserOptions",
                reportTitle: "Logility - RO - User Options Review" + "                               System Audit Logging Level:" + _SAB.ClientServerSession.Audit.GetDefaultLoggingLevel().ToString(),
                reportComment: "",
                reportInformation: "",
                displayValue: "User Options Review"
                ));
            frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reports: reports);
            //frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reportType: eReportType.UserOptionsReview, reportName: "UserOptions", reportTitle: "Logility - RO - User Options Review" + "                               System Audit Logging Level:" + _SAB.ClientServerSession.Audit.GetDefaultLoggingLevel().ToString());
            //viewer.ReportSource = userOptionsReviewReport;
            //viewer.ReportSource = userOptionsReviewDataSet;

            viewer.Text = "User Options Review Report"; 
            viewer.MdiParent = this.ParentForm;
            viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        
            viewer.Show();
            viewer.BringToFront();
        }
    
    }
}
