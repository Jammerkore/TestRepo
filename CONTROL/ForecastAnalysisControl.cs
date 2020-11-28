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
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class ForecastAnalysisControl : UserControl
    {
        public ForecastAnalysisControl()
        {
            InitializeComponent();
        }

        private void ForecastAnalysisControl_Load(object sender, EventArgs e)
        {
         
        }

        //BEGIN TT#3892-VStuart-Unhandled Exception Error-Forecast Analysis-MID
        private SessionAddressBlock _SAB;
        public void LoadData(SessionAddressBlock SAB)
        {
            _SAB = SAB;
            //END TT#3892-VStuart-Unhandled Exception Error-Forecast Analysis-MID
            this.ForecastAnalysisOptionsControl1.LoadData(SAB);
            this.selectMultipleMethodTypesControl1.LoadForecastData(SAB);
            //this.selectMultipleActionTypesControl1.LoadData(SAB);
            this.selectMultipleUsersAndGroupsControl1.LoadData();
            this.storeForecastVersionsControl1.LoadData(SAB);
            this.chainForecastVersionsControl1.LoadData(SAB);
        }

       


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnShowResults":
                    ShowResults();
                    break;
                case "Outlook Style":
                    this.ultraExplorerBar1.Style = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarStyle.OutlookNavigationPane;
                    break;
                case "Group Box Style":
                    this.ultraExplorerBar1.Style = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarStyle.ExplorerBar;
                    break;
                case "Listbar Style":
                    this.ultraExplorerBar1.Style = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarStyle.Listbar;
                    break;
                case "Toolbox Style":
                    this.ultraExplorerBar1.Style = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarStyle.Toolbox;
                    break;
                case "Studio Style":
                    this.ultraExplorerBar1.Style = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarStyle.VisualStudio2005Toolbox;
                    break;
                case "Default Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Default;
                    break;
                case "Office 2000 Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2000;
                    break;
                case "Office 2003 Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2003;
                    break;
                case "Office 2007 Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2007;
                    break;
                case "Studio 2005 Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.VisualStudio2005;
                    break;
                case "XP Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.XP;
                    break;
                case "XP Explorer Format":
                    this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.XPExplorerBar;
                    break;
                case "ShowCriteria":
                    this.ultraExplorerBar1.Visible = !this.ultraExplorerBar1.Visible;
                    break;

                case "useNewTab":
                    _useNewTab = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["useNewTab"]).Checked;
                    break;
                case "autoHideCriteria":
                    _autoHideCriteria = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["autoHideCriteria"]).Checked;
                    break;
         
               
            }
        }
        private int resultCount = 0;
        private bool _useNewTab = false;
        private bool _autoHideCriteria = false;
        private void ShowResults()
        {
            //BEGIN TT#3892-VStuart-Unhandled Exception Error-Forecast Analysis-MID
            try
            {
                if (_autoHideCriteria == true)
                {
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool) this.ultraToolbarsManager1.Tools["ShowCriteria"]).Checked = false;
                    //this.ultraExplorerBar1.Visible = false;
                }

                string sTabPage;
                resultCount++;

                if (_useNewTab == false)
                {
                    int tabKeyIndex = this.ultraTabControl1.Tabs.IndexOf("Results");
                    if (tabKeyIndex != -1)
                    {
                        this.ultraTabControl1.Tabs.Remove(this.ultraTabControl1.Tabs["Results"]);
                    }
                    sTabPage = "Results";
                }
                else
                {
                    sTabPage = "Results" + resultCount.ToString();
                }

                this.ultraTabControl1.Tabs.Add(sTabPage, sTabPage);
                ForecastAnalysisResultGridControl rg = new ForecastAnalysisResultGridControl();
                rg.Dock = DockStyle.Fill;
                this.ultraTabControl1.Tabs[sTabPage].TabPage.Controls.Add(rg);
                this.ultraTabControl1.Tabs[sTabPage].Selected = true;
                ReportData reportData = new ReportData();
                System.Data.DataSet ForecastAnalysisDataSet = MIDEnvironment.CreateDataSet("ForecastAnalysisDataSet");
                reportData.ForecastAnalysis_GetData(ForecastAnalysisDataSet, GetAnalysisEventArgs());

                rg.BindGrid(ForecastAnalysisDataSet);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            //END TT#3892-VStuart-Unhandled Exception Error-Forecast Analysis-MID
        }

        private ReportData.ForecastAnalysisEventArgs GetAnalysisEventArgs()
        {
            ReportData.ForecastAnalysisEventArgs e = new ReportData.ForecastAnalysisEventArgs();
            this.ForecastAnalysisOptionsControl1.GetOptions(ref e);
            this.forecastAnalysisDateOptionsControl1.GetOptions(ref e);

            this.selectMultipleMethodTypesControl1.GetSelectedMethods(ref e);
            this.selectMultipleUsersAndGroupsControl1.GetSelectedUsers(ref e);

            e.restrictStoreForecastVersions = !this.storeForecastVersionsControl1.IsEveryNodeSelected();
            if (e.restrictStoreForecastVersions) e.storeForecastVersionRIDsToInclude = this.storeForecastVersionsControl1.GetSelectedVersions();

            e.restrictChainForecastVersions = !this.chainForecastVersionsControl1.IsEveryNodeSelected();
            if (e.restrictChainForecastVersions) e.chainForecastVersionRIDsToInclude = this.storeForecastVersionsControl1.GetSelectedVersions();

            return e;
        }

    }
}
