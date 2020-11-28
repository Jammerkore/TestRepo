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
    public partial class ProcessControlForm : MIDFormBase
    {
        //private SessionAddressBlock _SAB;
        private ExplorerAddressBlock _EAB;
        private Dictionary<eProcesses, List<ProcessControlRule>> _processRules;
        private int _currentProcessID;
        private bool _changesMade = false;

        private string _strProcess;
        private string _strProcessControl;
        private string _strProcessID;
        private string _strIsRunning;
        private string _strLastModifiedOn;
        private string _strLastModifiedBy;
        private string _strSaveChanges;
        private string _strMustBeRunning;
        private string _strCannotBeRunning;
        private string _strServicesRestartRequired;
        private string _strWindowsUser;
        private const string _colUpdated = "Updated";
        private const string _colProcessID = "ProcessID";
        private const string _colTextValue = "TEXT_VALUE";
        private const string _colTextCode = "TEXT_CODE";
        private const string _strTrue = "True";
        private const string _strFalse = "False";

        public ProcessControlForm(SessionAddressBlock SAB, ExplorerAddressBlock EAB) : base(SAB)
        {
            //_SAB = SAB;
            _EAB = EAB;
            InitializeComponent();
            _processRules = new Dictionary<eProcesses, List<ProcessControlRule>>();
        }

        private void ProcessControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (_SAB.ClientServerSession.UserRID == Include.AdministratorUserRID)
                {
                    SetReadOnly(true);
                }
                else
                {
                    SetReadOnly(false);
                }
                _strWindowsUser = GetUser();
                cboProcesses.Enabled = true;
                SetText();
                LoadRunningProcesses();
                LoadProcessesCombo();
                FormLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), _strProcessControl);
            }
        }

        private string GetUser()
        {
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            return StripOutDomain(currentUser.Name);
        }

        private string StripOutDomain(string domainAndUser)
        {
            string userName = domainAndUser;

            int backslashPosition = domainAndUser.IndexOf("\\");
            if (backslashPosition > 0)
            {
                userName = domainAndUser.Substring(backslashPosition + 1);
            }

            return userName;
        }

        private void SetText()
        {
            this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
            this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
            _strProcess = MIDText.GetTextOnly(eMIDTextCode.lbl_Process);
            _strProcessControl = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessControl);
            _strProcessID = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessID);
            _strIsRunning = MIDText.GetTextOnly(eMIDTextCode.lbl_IsRunning);
            _strLastModifiedOn = MIDText.GetTextOnly(eMIDTextCode.lbl_LastModifiedOn);
            _strLastModifiedBy = MIDText.GetTextOnly(eMIDTextCode.lbl_LastModifiedBy);
            _strSaveChanges = MIDText.GetTextOnly(eMIDTextCode.lbl_SaveChanges);
            _strMustBeRunning = MIDText.GetTextOnly(eMIDTextCode.lbl_MustBeRunning);
            _strCannotBeRunning = MIDText.GetTextOnly(eMIDTextCode.lbl_CannotBeRunning);
            _strServicesRestartRequired = MIDText.GetTextOnly(eMIDTextCode.lbl_ServicesRestartRequired);
        }

        private void LoadRunningProcesses()
        {
            DataTable dt = new DataTable("Running");
            DataColumn column1 = new DataColumn(_strProcess, typeof(string));
            DataColumn column2 = new DataColumn(_strProcessID, typeof(int));
            DataColumn column3 = new DataColumn(_strIsRunning, typeof(bool));
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
            this.cboProcesses.DisplayMember = _colTextValue;
            this.cboProcesses.ValueMember = _colTextCode;

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
            removeEntry.Add((int)eProcesses.assortmentAPI);


            int codeValue;
            for (int i = dtProcesses.Rows.Count - 1; i >= 0; i--)
            {
                dr = dtProcesses.Rows[i];
                codeValue = Convert.ToInt32(dr[_colTextCode]);
                if (removeEntry.Contains(codeValue))
                {
                    dtProcesses.Rows.Remove(dr);
                }
            }
        }


        private void ugProcessRunning_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            e.Layout.Bands[0].Columns[_strProcess].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            foreach (UltraGridRow row in ugProcessRunning.Rows)
            {
                row.Activation = Activation.ActivateOnly;
            }

        }

        private void ugProcessRules_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            e.Layout.Bands[0].Columns[_strProcess].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            e.Layout.Bands[0].Columns[_strLastModifiedOn].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            e.Layout.Bands[0].Columns[_strLastModifiedBy].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            foreach (UltraGridRow row in ugProcessRules.Rows)
            {
                row.Cells[_strProcess].Activation = Activation.NoEdit;
                row.Cells[_strLastModifiedOn].Activation = Activation.NoEdit;
                row.Cells[_strLastModifiedBy].Activation = Activation.NoEdit;
            }

            this.ugProcessRules.DisplayLayout.AddNewBox.Hidden = true;
            this.ugProcessRules.DisplayLayout.GroupByBox.Hidden = true;
            this.ugProcessRules.DisplayLayout.GroupByBox.Prompt = string.Empty;
            this.ugProcessRules.DisplayLayout.Bands[0].Columns[_colUpdated].Hidden = true;
            this.ugProcessRules.DisplayLayout.Bands[0].Columns[_colProcessID].Hidden = true;
        }

        private void cboProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboProcesses.SelectedIndex > -1)
            {
                if (ChangePending)
                {
                    ResultSaveChanges = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(GetPendingMessage()), _strSaveChanges,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (ResultSaveChanges == DialogResult.Yes)
                    {
                        SaveChanges();
                    }
                    ChangePending = false;
                }
                DataRowView drv = (DataRowView)cboProcesses.SelectedItem;
                DataRow dr = drv.Row;
                int aProcess = Convert.ToInt32(dr[_colTextCode]);
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
                DataColumn column1 = new DataColumn(_strProcess, typeof(string));
                DataColumn column2 = new DataColumn(_strMustBeRunning, typeof(bool));
                DataColumn column3 = new DataColumn(_strCannotBeRunning, typeof(bool));
                DataColumn column4 = new DataColumn(_colUpdated, typeof(bool));
                DataColumn column5 = new DataColumn(_colProcessID, typeof(int));
                DataColumn column6 = new DataColumn(_strLastModifiedOn, typeof(string));
                DataColumn column7 = new DataColumn(_strLastModifiedBy, typeof(string));
                dt.Columns.AddRange(new DataColumn[] { column1, column2, column3, column4, column5, column6, column7 });

                foreach (ProcessControlRule aRule in rules)
                {
                    string processText = MIDText.GetTextOnly(aRule.ProcessID);
                    string lastModifiedDateTime = string.Empty;
                    if (aRule.LastModifiedDateTime != Include.UndefinedDate)
                    {
                        lastModifiedDateTime = aRule.LastModifiedDateTime.ToString();
                    }
                    dt.Rows.Add(new object[] { processText, aRule.MustBeRunning, aRule.CannotBeRunning, false, aRule.ProcessID, lastModifiedDateTime, aRule.LastModifiedBy });
                }
                dt.DefaultView.Sort = _strProcess; //TT#1644 - MD - Doconnell - Process Control
                ugProcessRules.DataSource = dt;
            }
            _currentProcessID = aProcess;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel_Click();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void ugProcessRules_CellChange(object sender, CellEventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _changesMade = true;
                // disable the event so it does not fire when updating the cell
                this.ugProcessRules.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugProcessRules_CellChange);
                if (e.Cell.Column.Key == _strMustBeRunning
                    && e.Cell.Row.Cells[_strMustBeRunning].Text == _strTrue)
                {
                    e.Cell.Row.Cells[_strCannotBeRunning].Value = _strFalse;
                }
                else if (e.Cell.Column.Key == _strCannotBeRunning
                    && e.Cell.Row.Cells[_strCannotBeRunning].Text == _strTrue)
                {
                    e.Cell.Row.Cells[_strMustBeRunning].Value = _strFalse;
                }
                e.Cell.Row.Cells[_colUpdated].Value = _strTrue;
                // enable the event
                this.ugProcessRules.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugProcessRules_CellChange);
            }
        }

        override protected bool SaveChanges()
        {
            bool blCannotBeRunning, blMustBeRunning;
            int intProcessID;
            this.Cursor = Cursors.WaitCursor;
            if (ChangePending)
            {
                SystemData sd = new SystemData();
                List<ProcessControlRule> rules = null;
                if (_processRules.ContainsKey((eProcesses)_currentProcessID))
                {
                    rules = _processRules[(eProcesses)_currentProcessID];
                }

                try
                {
                    DateTime lastModifiedDateTime = DateTime.Now;
                    sd.OpenUpdateConnection();
                    foreach (UltraGridRow row in ugProcessRules.Rows)
                    {
                        if (row.Cells[_colUpdated].Text == _strTrue)
                        {
                            blCannotBeRunning = row.Cells[_strCannotBeRunning].Text == _strTrue ? true : false;
                            blMustBeRunning = row.Cells[_strMustBeRunning].Text == _strTrue ? true : false;
                            intProcessID = Convert.ToInt32(row.Cells[_colProcessID].Value);
                            // update database
                            sd.UpdateAPIProcessControlRules(
                                _currentProcessID,
                                blCannotBeRunning,
                                blMustBeRunning,
                                intProcessID,
                                lastModifiedDateTime,
                                _strWindowsUser
                                );
                            // update collection used for processing
                            foreach (ProcessControlRule aRule in rules)
                            {
                                if (aRule.ProcessID == intProcessID)
                                {
                                    aRule.MustBeRunning = blMustBeRunning;
                                    aRule.CannotBeRunning = blCannotBeRunning;
                                    aRule.LastModifiedDateTime = lastModifiedDateTime;
                                    aRule.LastModifiedBy = _strWindowsUser;
                                }
                            }
                        }
                    }

                    sd.CommitData();
                    ChangePending = false;
                }
                catch (Exception exception)
                {
                    HandleException(exception);
                }
                finally
                {
                    sd.CloseUpdateConnection();
                    this.Cursor = Cursors.Default;
                }
                
            }

            this.Cursor = Cursors.Default;
            return false;
        }

        override protected void BeforeClosing()
        {
            try
            {
                if (_changesMade)
                {
                    ResultSaveChanges = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustRestartServices), _strServicesRestartRequired,
                    MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        override public void ISave()
        {
            try
            {
                SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
