using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MIDRetail.Data;

namespace UnitTesting
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private const string settingsControlTabName = "Settings";
        private const string unitTestTabName = "Unit Tests";
        //private const string environmentControlTabName = "Environments";
        private const string storedProcTabName = "Stored Procedures";
        private const string storedProcConverterTabName = "Converter";

        private void Form1_Load(object sender, EventArgs e)
        {

            UnitTests.InitializeTestStructures();
            //look for a last used file
            //string lastUsedFolder = UnitTests.defaultFolder;
            //if (System.IO.File.Exists(UnitTests.unitTestFilePath + "\\lastused.txt") == true)
            //{
            //    System.IO.StreamReader sr = new System.IO.StreamReader(UnitTests.unitTestFilePath + "\\lastused.txt");
            //    lastUsedFolder = sr.ReadLine();
            //    sr.Close();
            //}
            //UnitTests.defaultFolder = lastUsedFolder;
            
            Shared_BaseStoredProcedures.PopulateStoredProcedureListFromAssembly();
            UnitTests.LoadTestFiles(UnitTests.unitTestFilePath);
         



            //DataSet dsResults = StoredProcedureTesting.RunUnitTests(StoredProcedureTesting.dsTests.Tables[0].Select(), dba, "Dots");


            //this.ultraTabControl1.Tabs.Add(settingsControlTabName, settingsControlTabName);
            //SettingsControl settingsControl = new SettingsControl();
            //settingsControl.Dock = DockStyle.Fill;
            //this.ultraTabControl1.Tabs[settingsControlTabName].TabPage.Controls.Add(settingsControl);
            //this.ultraTabControl1.Tabs[settingsControlTabName].AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            //this.ultraTabControl1.Tabs[settingsControlTabName].CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;


            //this.ultraTabControl1.Tabs.Add(environmentControlTabName, environmentControlTabName);
            //EnvironmentControl environmentControl = new EnvironmentControl();
            //environmentControl.Dock = DockStyle.Fill;
            //this.ultraTabControl1.Tabs[environmentControlTabName].TabPage.Controls.Add(environmentControl);
            //this.ultraTabControl1.Tabs[environmentControlTabName].AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            //this.ultraTabControl1.Tabs[environmentControlTabName].CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            //environmentControl.BindGrid(UnitTests.GetEnvironmentsForEditing());

            this.ultraTabControl1.Tabs.Add(storedProcConverterTabName, storedProcConverterTabName);
            StoredProcedureConverterControl converterControl = new StoredProcedureConverterControl();
            //viewerControl.ShowUnitTestTab_Event += new StoredProcedureViewerControl.ShowUnitTestTab_EventHandler(Handle_SelectUnitTestTab);
            converterControl.Dock = DockStyle.Fill;
            this.ultraTabControl1.Tabs[storedProcConverterTabName].TabPage.Controls.Add(converterControl);
            this.ultraTabControl1.Tabs[storedProcConverterTabName].AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            this.ultraTabControl1.Tabs[storedProcConverterTabName].CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            
            this.ultraTabControl1.Tabs.Add(storedProcTabName, storedProcTabName);
            StoredProcedureViewerControl viewerControl = new StoredProcedureViewerControl();
            viewerControl.ShowUnitTestTab_Event += new StoredProcedureViewerControl.ShowUnitTestTab_EventHandler(Handle_SelectUnitTestTab);
            viewerControl.MakeReport_Event += new StoredProcedureViewerControl.MakeReport_EventHandler(Handle_MakeReport);
            viewerControl.Dock = DockStyle.Fill;
            this.ultraTabControl1.Tabs[storedProcTabName].TabPage.Controls.Add(viewerControl);
            this.ultraTabControl1.Tabs[storedProcTabName].AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            this.ultraTabControl1.Tabs[storedProcTabName].CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            viewerControl.BindGrid(UnitTests.GetStoredProcedureListAsDataset());


            this.ultraTabControl1.Tabs.Add(unitTestTabName, unitTestTabName);
            UnitTestManager testManager = new UnitTestManager();
            testManager.RunSelectedTests_Event += new UnitTestManager.RunSelectedTests_EventHandler(Handle_RunSelectedTests);
            testManager.RunExistingPlan_Event += new UnitTestManager.RunExistingPlan_EventHandler(Handle_RunExistingPlan);
            testManager.Dock = DockStyle.Fill;
            this.ultraTabControl1.Tabs[unitTestTabName].TabPage.Controls.Add(testManager);
            this.ultraTabControl1.Tabs[unitTestTabName].AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            this.ultraTabControl1.Tabs[unitTestTabName].CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            this.ultraTabControl1.Tabs[unitTestTabName].Selected = true;
            testManager.unitTestGridControl1.BindGrid(UnitTests.GetUnitTests());
            
        }

        private void Handle_SelectUnitTestTab(object sender, StoredProcedureViewerControl.ShowUnitTestTab_EventArgs e)
        {
            this.ultraTabControl1.Tabs[unitTestTabName].Selected = true;
        }
     

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //case "btnPopUpEnvironment":
                //    PopUpEnvironmentControl();
                //    break;
               
            }
        }
   
        private void Handle_MakeReport(object sender, StoredProcedureViewerControl.MakeReport_EventArgs e)
        {
            string reportTabName = string.Empty;

            ReportViewerControl reportViewer = new ReportViewerControl();
         
            if (e.ReportType == UnitTests.ReportTypes.ProceduresNotReferenced)
            {
                reportTabName = "Procedures In Project Not Referenced";
                reportViewer.ReportType = UnitTests.ReportTypes.ProceduresNotReferenced;
                reportViewer.MakeProceduresNotReferencedDataSet();

            }
            if (e.ReportType == UnitTests.ReportTypes.ParameterListing)
            {
                reportTabName = "Parameter Listing";
                reportViewer.ReportType = UnitTests.ReportTypes.ParameterListing;
                reportViewer.MakeParameterListingDataSet();

            }
            if (e.ReportType == UnitTests.ReportTypes.DatabaseComparison)
            {
                reportTabName = "Database Comparison-" + UnitTests.defaultEnvironment;
                reportViewer.ReportType = UnitTests.ReportTypes.DatabaseComparison;
                reportViewer.MakeDatabaseComparisonDataSet();

            }
            if (e.ReportType == UnitTests.ReportTypes.ObjectDBOCheck)
            {
                reportTabName = "Object DBO Check";
                reportViewer.ReportType = UnitTests.ReportTypes.ObjectDBOCheck;
                reportViewer.MakeObjectDBOCheckDataSet();

            }
            //this.ultraTabControl1.Tabs.Add(reportTabName, reportTabName);
            addTab(reportTabName);
            reportViewer.Dock = DockStyle.Fill;
            this.ultraTabControl1.Tabs[reportTabName].TabPage.Controls.Add(reportViewer);
            this.ultraTabControl1.Tabs[reportTabName].AllowClosing = Infragistics.Win.DefaultableBoolean.True;
            this.ultraTabControl1.Tabs[reportTabName].CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Always;
            this.ultraTabControl1.Tabs[reportTabName].Selected = true;
          
        }

        private void Handle_RunSelectedTests(object sender, UnitTestManager.RunSelectedTests_EventArgs e)
        { 
            SelectPlanForm frm = new SelectPlanForm();
            frm.ShowDialog();
            if (frm.isOK)
            {
                string unitTestTabName = frm.planName;
                
                //this.ultraTabControl1.Tabs.Add(unitTestTabName, unitTestTabName);
                addTab(unitTestTabName);
                PlanControl planControl = new PlanControl();
                planControl.Dock = DockStyle.Fill;
                this.ultraTabControl1.Tabs[unitTestTabName].TabPage.Controls.Add(planControl);
                this.ultraTabControl1.Tabs[unitTestTabName].Selected = true;

                planControl.LoadTestsToRun(e.drSelectedTests, frm.planName);
            }
        }
        private void Handle_RunExistingPlan(object sender, UnitTestManager.RunExistingPlan_EventArgs e)
        {
            SelectPlanForm frm = new SelectPlanForm();
            frm.AllowOnlyExistingPlans();
            frm.ShowDialog();
            if (frm.isOK)
            {
                string unitTestTabName = frm.planName;

                //this.ultraTabControl1.Tabs.Add(unitTestTabName, unitTestTabName);
                addTab(unitTestTabName);
                PlanControl planControl = new PlanControl();
                planControl.Dock = DockStyle.Fill;
                this.ultraTabControl1.Tabs[unitTestTabName].TabPage.Controls.Add(planControl);
                this.ultraTabControl1.Tabs[unitTestTabName].Selected = true;

                planControl.LoadTestsFromExistingPlan(frm.planName);
            }
        }

        private void ultraTabControl1_TabClosed(object sender, Infragistics.Win.UltraWinTabControl.TabClosedEventArgs e)
        {
           this.ultraTabControl1.Tabs[e.Tab.Key].Dispose();
            
        }
        private void addTab(string tabName)
        {
            if (this.ultraTabControl1.Tabs.Exists(tabName))
            {
                this.ultraTabControl1.Tabs[tabName].Dispose();
            }
            this.ultraTabControl1.Tabs.Add(tabName, tabName);
        }
    }
}
