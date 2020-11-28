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
    public partial class AllocationAnalysisControl : UserControl
    {
        public AllocationAnalysisControl()
        {
            InitializeComponent();
        }

        private void AllocationAnalysisControl_Load(object sender, EventArgs e)
        {
         
        }

        private GetSelectedHeaderRIDsFromWorkspace _getSelectedHeaderRIDsFromWorkspace;
        private SessionAddressBlock _SAB;       //TT#3892 & TT#3893-VStuart-Unhandled Exception Error-Forecast Analysis-MID
        public void LoadData(SessionAddressBlock SAB, GetSelectedHeaderRIDsFromWorkspace getSelectedHeaderRIDsFromWorkspace)
        {
            _SAB = SAB;//TT#3892 & TT#3893-VStuart-Unhandled Exception Error-Forecast Analysis-MID
            this._getSelectedHeaderRIDsFromWorkspace = getSelectedHeaderRIDsFromWorkspace;
            this.allocationAnalysisOptionsControl1.LoadData(SAB);
            this.selectMultipleMethodTypesControl1.LoadAllocationData(SAB);
            this.selectMultipleActionTypesControl1.LoadData(SAB);
            this.selectMultipleUsersAndGroupsControl1.LoadData();
            this.selectMultipleHeaderTypesControl1.LoadData(SAB);
            this.selectMultipleHeaderStatusesControl1.LoadAllocationData(SAB);
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
                AllocationAnalysisResultGridControl rg = new AllocationAnalysisResultGridControl();
                rg.Dock = DockStyle.Fill;
                this.ultraTabControl1.Tabs[sTabPage].TabPage.Controls.Add(rg);
                this.ultraTabControl1.Tabs[sTabPage].Selected = true;
                //FillResultGridWithSampleData(rg);
                ReportData reportData = new ReportData();
                System.Data.DataSet AllocationAnalysisDataSet = MIDEnvironment.CreateDataSet("AllocationAnalysisDataSet");
                reportData.AllocationAnalysis_GetData(AllocationAnalysisDataSet, GetAnalysisEventArgs());

                rg.BindGrid(AllocationAnalysisDataSet);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            //END TT#3892-VStuart-Unhandled Exception Error-Forecast Analysis-MID
        }

        private ReportData.AllocationAnalysisEventArgs GetAnalysisEventArgs()
        {


            ReportData.AllocationAnalysisEventArgs e = new ReportData.AllocationAnalysisEventArgs();


            this.allocationAnalysisOptionsControl1.GetOptions(ref e);

            //get selected headers if necessary
            if (e.restrictHeaders == true)
            {
                List<int> selectedHeaderRids = _getSelectedHeaderRIDsFromWorkspace();
                string sList = string.Empty;
                foreach (int headerRID in selectedHeaderRids)
                {
                    if (sList == string.Empty)
                    {
                        sList += headerRID.ToString();
                    }
                    else
                    {
                        sList += "," + headerRID.ToString();
                    }
                }
                e.headerRIDsToInclude = sList;
            }


            
            this.selectMultipleHeaderStatusesControl1.GetSelectedStatuses(ref e);
            this.selectMultipleHeaderTypesControl1.GetSelectedTypes(ref e);
            this.selectMultipleActionTypesControl1.GetSelectedActions(ref e);
            this.selectMultipleMethodTypesControl1.GetSelectedMethods(ref e);
            this.selectMultipleUsersAndGroupsControl1.GetSelectedUsers(ref e);
            return e;
        }

    }
}
