using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Windows
{
    public partial class ProcessControlForm : Form
    {
        private SessionAddressBlock _SAB;
        private ExplorerAddressBlock _EAB;
        private Dictionary<eProcesses, List<ProcessControlRule>> _processRules;
        public ProcessControlForm(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            _SAB = SAB;
            _EAB = EAB;
            InitializeComponent();
            _processRules = new Dictionary<eProcesses, List<ProcessControlRule>>();
        }

        private void ProcessControl_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRunningProcesses();
                LoadProcessesCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Process Control");
            }
        }

        private void LoadRunningProcesses()
        {
            DataTable dt = new DataTable("Running");
            DataColumn column1 = new DataColumn("Process", typeof(string));
            DataColumn column2 = new DataColumn("Process ID", typeof(int));
            DataColumn column3 = new DataColumn("Is Running", typeof(bool));
            dt.Columns.AddRange(new DataColumn[] { column1, column2, column3 });

            List<ProcessStateEntry> runningProc = _SAB.ControlServerSession.GetRunningProcesses();
            foreach (var ProcessStateEntry in runningProc)
            {
                string processText = MIDText.GetTextOnly((int)ProcessStateEntry.Process);
                dt.Rows.Add(new object[] { processText, ProcessStateEntry.ProcessId, ProcessStateEntry.IsRunning });
            }

            ugProcessRunning.DataSource = dt;
        }

        private void LoadProcessesCombo()
        {
            //BEGIN TT#1644 - MD - Doconnell - Process Control
            DataTable dtProcesses = MIDText.GetLabelValuesInRange((int)eProcesses.unknown, (int)eProcesses.BatchComp);
            //DataTable dtProcesses = MIDText.GetLabelValuesInRange((int)eProcesses.unknown, (int)eProcesses.AllocationTasklist);
            //END TT#1644 - MD - Doconnell - Process Control

            RemoveInvalidProcesses(dtProcesses);

            cboProcesses.DataSource = dtProcesses;
            this.cboProcesses.DisplayMember = "TEXT_VALUE";
            this.cboProcesses.ValueMember = "TEXT_CODE";

        }

        private static void RemoveInvalidProcesses(DataTable dtProcesses)
        {
            DataRow dr;
            List<int> removeEntry = new List<int>();
            removeEntry.Add((int)eProcesses.unknown);
            removeEntry.Add((int)eProcesses.controlService);
            removeEntry.Add((int)eProcesses.storeService);
            removeEntry.Add((int)eProcesses.hierarchyService);
            removeEntry.Add((int)eProcesses.clientApplication);
            removeEntry.Add((int)eProcesses.storeGroupBuilder);
            removeEntry.Add((int)eProcesses.hierarchyWebService);
            //removeEntry.Add((int)eProcesses.forecasting);
            removeEntry.Add((int)eProcesses.applicationService);
            //removeEntry.Add((int)eProcesses.allocate);
            removeEntry.Add((int)eProcesses.schedulerService);
            removeEntry.Add((int)eProcesses.executeJob);
            removeEntry.Add((int)eProcesses.headerService);
            removeEntry.Add((int)eProcesses.sqlScript);
            //removeEntry.Add((int)eProcesses.computationsLoad);
            removeEntry.Add((int)eProcesses.databaseConversionUtility);
            //BEGIN TT#1644-VSuart-Process Control-MID
            //removeEntry.Add((int)eProcesses.computationDriver); // <--This is actually shows up as Chain Forecasting.
            removeEntry.Add((int)eProcesses.storeDelete);
            removeEntry.Add((int)eProcesses.AllocationTasklist);
            //END TT#1644-VSuart-Process Control-MID
            removeEntry.Add((int)eProcesses.forecastExportThread);
            removeEntry.Add((int)eProcesses.StoreBinViewer);
            removeEntry.Add((int)eProcesses.sizeCurveGenerateThread);
            removeEntry.Add((int)eProcesses.scheduleInterface);
            removeEntry.Add((int)eProcesses.convertFilters);
            removeEntry.Add((int)eProcesses.forecastBalancing);
            //BEGIN TT#1644 - MD - Doconnell - Process Control
            //removeEntry.Add((int)eTaskType.SizeCurveMethod);
            removeEntry.Add((int)eProcesses.computationDriver);
            removeEntry.Add((int)eProcesses.specialRequest); 
            //END TT#1644 - MD - Doconnell - Process Control


            int codeValue;
            for (int i = dtProcesses.Rows.Count - 1; i >= 0; i--)
            {
                dr = dtProcesses.Rows[i];
                codeValue = Convert.ToInt32(dr["TEXT_CODE"]);
                if (removeEntry.Contains(codeValue))
                {
                    dtProcesses.Rows.Remove(dr);
                }
            }
        }


        private void ugProcessRunning_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["Process"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            foreach (UltraGridRow row in ugProcessRunning.Rows)
            {
                row.Activation = Activation.ActivateOnly;
            }

        }

        private void ugProcessRules_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["Process"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            foreach (UltraGridRow row in ugProcessRules.Rows)
            {
                row.Activation = Activation.NoEdit;
            }
        }

        private void cboProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboProcesses.SelectedIndex > -1)
            {
                DataRowView drv = (DataRowView)cboProcesses.SelectedItem;
                DataRow dr = drv.Row;
                int aProcess = Convert.ToInt32(dr["TEXT_CODE"]);
                LoadProcessRules(aProcess);
            }
        }

        private void LoadProcessRules(int aProcess)
        {
            List<ProcessControlRule> rules = null;
            if (_processRules.ContainsKey((eProcesses)aProcess))
            {
                rules = _processRules[(eProcesses)aProcess];
            }
            else
            {
                rules = _SAB.ControlServerSession.GetProcessingRules(aProcess);
                if (rules != null)
                {
                    _processRules.Add((eProcesses)aProcess, rules);
                }
            }

            ugProcessRules.DataSource = null;
            if (rules != null)
            {
                DataTable dt = new DataTable("Rules");
                DataColumn column1 = new DataColumn("Process", typeof(string));
                DataColumn column2 = new DataColumn("Must Be Running", typeof(bool));
                DataColumn column3 = new DataColumn("Cannot Be Running", typeof(bool));
                dt.Columns.AddRange(new DataColumn[] { column1, column2, column3 });

                foreach (ProcessControlRule aRule in rules)
                {
                    string processText = MIDText.GetTextOnly(aRule.ProcessID);
                    dt.Rows.Add(new object[] { processText, aRule.MustBeRunning, aRule.CannotBeRunning });
                }
                dt.DefaultView.Sort = "Process"; //TT#1644 - MD - Doconnell - Process Control
                ugProcessRules.DataSource = dt;
            }
        }
    }
}
