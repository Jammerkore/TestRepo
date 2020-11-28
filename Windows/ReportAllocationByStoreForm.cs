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
    public partial class ReportAllocationByStoreForm : Form
    {
        SessionAddressBlock _SAB;
        public ReportAllocationByStoreForm(SessionAddressBlock sab)
        {
            _SAB = sab;
            InitializeComponent();
        }

        private void LoadForm(object sender, EventArgs e)
        {
            this.reportAllocationByStoreControl.ViewReportEvent += new ReportAllocationByStore.ViewReportEventHandler(ViewReport);
            this.reportAllocationByStoreControl.LoadData(_SAB);
        }
        private void ViewReport(object sender, ReportData.AllocationByStoreEventArgs e)
        {
            //Windows.CrystalReports.AllocationByStore allocationByStoreReport = new Windows.CrystalReports.AllocationByStore();
            System.Data.DataSet allocationByStoreDataSet = MIDEnvironment.CreateDataSet("AllocationByStoreDataSet");
            ReportData reportData = new ReportData();

            string styleLevelName = reportData.AllocationByStore_Report_GetStyleLevelName();
            string parentOfStyleLevelName = reportData.AllocationByStore_Report_GetParentOfStyleLevelName();


            reportData.AllocationByStore_Report(allocationByStoreDataSet, e);

            allocationByStoreDataSet.Tables[0].Columns.Add("HEADER_TYPE");
            allocationByStoreDataSet.Tables[0].Columns.Add("HEADER_STATUS");

            DataTable dtTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));
            DataTable dtStatus = MIDText.GetTextType(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
            foreach (System.Data.DataRow dr in allocationByStoreDataSet.Tables[0].Rows)
            {
                int typeCode = (int)dr["DISPLAY_TYPE"];
                int statusCode = (int)dr["DISPLAY_STATUS"];

                DataRow[] drFindTypeText = dtTypes.Select("TEXT_CODE='" + typeCode + "'");
                if (drFindTypeText.Length > 0)
                {
                    dr["HEADER_TYPE"] = drFindTypeText[0]["TEXT_VALUE"];
                }
                else
                {
                    dr["HEADER_TYPE"] = string.Empty;
                }

                DataRow[] drFindStatusText = dtStatus.Select("TEXT_CODE='" + statusCode + "'");
                if (drFindStatusText.Length > 0)
                {
                    dr["HEADER_STATUS"] = drFindStatusText[0]["TEXT_VALUE"];
                }
                else
                {
                    dr["HEADER_STATUS"] = string.Empty;
                }
            }

            //allocationByStoreReport.SetDataSource(allocationByStoreDataSet);
            //allocationByStoreReport.SetParameterValue("@STORE_SELECTED", e.storeIDandName);  //Must set the parameter AFTER setting the data source
            //allocationByStoreReport.SetParameterValue("@STYLE_COL_HEADER", styleLevelName);  //Must set the parameter AFTER setting the data source
            //allocationByStoreReport.SetParameterValue("@PARENT_COL_HEADER", parentOfStyleLevelName);  //Must set the parameter AFTER setting the data source
            
            //frmReportViewer viewer = new frmReportViewer(_SAB, eReportType.AllocationByStore);
            //frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reportType: eReportType.AllocationByStore, reportName: "AllocationByStore", reportTitle: "Allocation By Store");
            List<ReportInfo> reports = new List<ReportInfo>();
            reports.Add(new ReportInfo(aReportSource: allocationByStoreDataSet,
                reportType: eReportType.AllocationByStore,
                reportName: "AllocationByStore",
                reportTitle: "Logilitity - RO - Allocation By Store",
                reportComment: "",
                reportInformation: "Store: " + e.storeIDandName,
                displayValue: "Allocation By Store"
                ));
            frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reports: reports);
            //viewer.ReportSource = allocationByStoreDataSet;
            viewer.Text = "Allocation By Store Report"; 
            viewer.MdiParent = this.ParentForm;
            viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        
            viewer.Show();
            viewer.BringToFront();
        }
    
    }
}
