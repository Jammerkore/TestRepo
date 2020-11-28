using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTesting
{
    public partial class UnitTestManager : UserControl
    {
        public UnitTestManager()
        {
            InitializeComponent();
        }

        private void unitTestManager_Load(object sender, EventArgs e)
        {
            this.unitTestGridControl1.SelectedUnitTestChangedEvent += new UnitTestGridControl.SelectedUnitTestChangedEventHandler(SelectedUnitTestChanged);
        }

        private void SelectedUnitTestChanged(object sender, UnitTestGridControl.SelectedUnitTestChangedEventArgs e)
        {
            string testName = (string)e.drUnitTest["testName"];
            string procedureName = (string)e.drUnitTest["procedureName"];
            this.unitTestParameterControl1.BindGrid(testName, procedureName, UnitTests.GetParametersForUnitTest(testName, procedureName));
            this.txtProcedureName.Text = procedureName;
            this.unitTestExpectedResultControl1.BindGrid(testName, procedureName);
          
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnPrepareSelectedTests":
                    DataRow[] drSelectedTests = this.unitTestGridControl1.GetSelectedUnitTests();
                    RaiseRunSelectedTests_Event(drSelectedTests);
                    break;
                case "btnPrepareAllTests":
                    DataRow[] drAllTests = this.unitTestGridControl1.GetAllUnitTests();
                    RaiseRunSelectedTests_Event(drAllTests);
                    break;
                case "btnRunExistingPlan":
                    RunExistingPlan();
                    break;
                case "btnSaveTests":
                    //if (UnitTests.defaultFolder == string.Empty)
                    //{
                    //    MessageBox.Show("Please provide a folder/user name in the settings tab first.");
                    //    break;
                    //}
                    UnitTests.SaveTestFiles();
                    
                    break;
                case "btnManageEnvironments":
                    PopUpEnvironmentControl();
                    break;
            }
        }
        private void PopUpEnvironmentControl()
        {
            EnvironmentForm ef = new EnvironmentForm();
            ef.ShowDialog();
        }
        public event RunSelectedTests_EventHandler RunSelectedTests_Event;
        public virtual void RaiseRunSelectedTests_Event(DataRow[] drSelectedTests)
        {

            if (RunSelectedTests_Event != null)
                RunSelectedTests_Event(this, new RunSelectedTests_EventArgs(drSelectedTests));
        }
        public class RunSelectedTests_EventArgs
        {
            public RunSelectedTests_EventArgs(DataRow[] drSelectedTests) { this.drSelectedTests = drSelectedTests; }
            public DataRow[] drSelectedTests
            {
                get; private set;
            }
        }
        public delegate void RunSelectedTests_EventHandler(object sender, RunSelectedTests_EventArgs e);


        private void RunExistingPlan()
        {
            RaiseRunExistingPlan_Event();
        }
        public event RunExistingPlan_EventHandler RunExistingPlan_Event;
        public virtual void RaiseRunExistingPlan_Event()
        {

            if (RunExistingPlan_Event != null)
                RunExistingPlan_Event(this, new RunExistingPlan_EventArgs());
        }
        public class RunExistingPlan_EventArgs
        {
           
        }
        public delegate void RunExistingPlan_EventHandler(object sender, RunExistingPlan_EventArgs e);
    }
}
