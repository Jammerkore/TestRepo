using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.Misc;


using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class SchedulerJobManager : MIDFormBase
    {
        //private SessionAddressBlock _SAB;
        private ExplorerAddressBlock _EAB;
        private ScheduleData _scheduleData;
        public SchedulerJobManager(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            _SAB = SAB;
            _EAB = EAB;
            InitializeComponent();
        }

        private void SchedulerJobManager_Load(object sender, EventArgs e)
        {
            try
            {
                InitToolbars();
                LoadUsers();
                LoadJobs();

                SetReadOnly(true);
                FormLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Process Control");
            }
        }

        private void LoadUsers()
        {
            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.ultraToolbarsManager1.Tools["ControlContainerToolUsers"];
            MIDComboBoxEnh.MyComboBox cmbUsers = (MIDComboBoxEnh.MyComboBox)cct.Control;

            SecurityAdmin sa = new SecurityAdmin();
            DataTable dtUsers = sa.GetActiveUsers();

            //List<DataRow> rowsToDelete = new List<DataRow>();
            //foreach (DataRow aRow in dtUsers.Rows)
            //{
            //    int user = int.Parse(aRow["TEXT_CODE"].ToString());
            //    if (user == 1)
            //    {
            //        rowsToDelete.Add(aRow);
            //    }
            //}
            //foreach (DataRow aRow in rowsToDelete)
            //{
            //    dtUsers.Rows.Remove(aRow);
            //}

            foreach (DataRow aRow in dtUsers.Rows)
            {
                string userName = aRow["USER_NAME"].ToString();
                string userFullName = aRow["USER_NAME"].ToString();
                if (!string.IsNullOrEmpty(userFullName) && userName != userFullName)
                {
                    aRow["USER_NAME"] = userName + " [" + userFullName + "]";
                }
            }
            dtUsers.AcceptChanges();


            // INfragistic Valuelist
            Infragistics.Win.ValueList valList;
            Infragistics.Win.ValueListItem valListItem;

            // Create Grid Value List
            valList = ultraGrid1.DisplayLayout.ValueLists.Add("Users");
            foreach (DataRow row in dtUsers.Rows)
            {
                valListItem = new Infragistics.Win.ValueListItem();
                valListItem.DataValue = Convert.ToInt32(row["USER_RID"]);
                valListItem.DisplayText = Convert.ToString(row["USER_NAME"]);
                valList.ValueListItems.Add(valListItem);
            }


            //DataRow selectRow = dtActions.NewRow();
            //selectRow["TEXT_CODE"] = Include.NoRID;
            //selectRow["TEXT_VALUE"] = "Select action...";
            //dtActions.Rows.InsertAt(selectRow, 0);
            dtUsers.PrimaryKey = new DataColumn[] { dtUsers.Columns["USER_NAME"] };

            DataView dv = new DataView(dtUsers);
            dv.Sort = "USER_NAME";

            midComboBoxEnhUsers.DisplayMember = "USER_NAME";
            midComboBoxEnhUsers.ValueMember = "USER_RID";
            this.midComboBoxEnhUsers.DataSource = dv;       // TT#1398-MD - stodd - Clean up order of Allocation Actions in the dropdowns
            if (dtUsers.Rows.Count > 0)
            {
                midComboBoxEnhUsers.SelectedIndex = 0;
            }
        }

        private void InitToolbars()
        {
            this.ultraToolbarsManager1.ToolbarSettings.AllowCustomize = DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowFloating = DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowHiding = DefaultableBoolean.False;
            Infragistics.Win.UltraWinToolbars.UltraToolbar tb = this.ultraToolbarsManager1.Toolbars["uToolsAllJobs"];
            tb.ShowInToolbarList = false;
            tb = this.ultraToolbarsManager1.Toolbars["uToolsUserJobs"];
            tb.ShowInToolbarList = false;
        }

        private void ddOnHold_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            // Set the scroll style to immediate so when the user scrolls the drop down 
            // using scroll thumb, rows get scrolled imediately.
            e.Layout.ScrollStyle = ScrollStyle.Immediate;

            // Change the order in which columns get displayed in the UltraDropDown.
            e.Layout.Bands[0].Columns["USER_RID"].Hidden = true;
            e.Layout.Bands[0].Columns["USER_FULLNAME"].Hidden = true;
            e.Layout.Bands[0].Columns["USER_DESCRIPTION"].Hidden = true;
            e.Layout.Bands[0].Columns["USER_ACTIVE_IND"].Hidden = true;
            e.Layout.Bands[0].Columns["USER_DELETE_IND"].Hidden = true;
            //e.Layout.Bands[0].Columns["ProductName"].Header.VisiblePosition = 1;

            // Sort the items in the drop down by ProductName column.
            e.Layout.Bands[0].SortedColumns.Clear();
            e.Layout.Bands[0].SortedColumns.Add("USER_NAME", false);

            // Set the border style of the drop down.
            e.Layout.BorderStyle = UIElementBorderStyle.Solid;

            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            e.Layout.Grid.DrawFilter = new NoFocusRect();
            e.Layout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            e.Layout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
        }

        private void LoadJobs()
        {
            DataTable dtExecutionStatus;
            Infragistics.Win.ValueList valList = null;
            Infragistics.Win.ValueListItem valListItem;

            dtExecutionStatus = MIDText.GetTextType(eMIDTextType.eProcessExecutionStatus, eMIDTextOrderBy.TextCode);
            if (ultraGrid1.DisplayLayout.ValueLists.Exists("ExecutionStatus"))
            {
                valList = ultraGrid1.DisplayLayout.ValueLists["ExecutionStatus"];
                valList.ResetValueListItems();
            }
            else
            {
                // Create Grid Value List
                valList = ultraGrid1.DisplayLayout.ValueLists.Add("ExecutionStatus");
            }

            foreach (DataRow row in dtExecutionStatus.Rows)
            {
                valListItem = new Infragistics.Win.ValueListItem();
                valListItem.DataValue = Convert.ToInt32(row["TEXT_CODE"]);
                valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
                valList.ValueListItems.Add(valListItem);
            }

            _scheduleData = new ScheduleData();
			// Begin TT#1386-MD - stodd - Scheduler Job Manager
            //DataTable dt = _scheduleData.ReadScheduledJobsForJobManager("all jobs", -1);
            DataTable dt = _SAB.SchedulerServerSession.GetScheduledJobsForJobManager("all jobs", "all jobs", -1);	
			// End TT#1386-MD - stodd - Scheduler Job Manager
			
            DataColumn dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Boolean");
            dataColumn.ColumnName = "OnHold";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            dt.Columns.Add(dataColumn);

            SetOnHoldStatus(dt);

            ultraGrid1.DataSource = dt;
        }

        private void SetOnHoldStatus(DataTable dt)
        {
            foreach (DataRow aRow in dt.Rows)
            {
                DateTime onHoldDate = Include.UndefinedDate;
                DateTime releasedDate = Include.UndefinedDate;
                eProcessExecutionStatus execStatus = eProcessExecutionStatus.None;
                if (aRow["EXECUTION_STATUS"] != DBNull.Value)
                {
                    execStatus = (eProcessExecutionStatus)int.Parse(aRow["EXECUTION_STATUS"].ToString());
                }
                if (aRow["HOLD_BY_DATETIME"] != DBNull.Value)
                {
                    onHoldDate = DateTime.Parse(aRow["HOLD_BY_DATETIME"].ToString());
                }
                if (aRow["RELEASED_BY_DATETIME"] != DBNull.Value)
                {
                    releasedDate = DateTime.Parse(aRow["RELEASED_BY_DATETIME"].ToString());
                }

                if (onHoldDate > releasedDate || execStatus == eProcessExecutionStatus.OnHold)
                {
                    aRow["OnHold"] = true;
                }
                else
                {
                    aRow["OnHold"] = false;
                }
            }

            dt.AcceptChanges();
        }


        private void ultraGrid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            // Hide Columns
            e.Layout.Bands[0].Columns["SCHED_RID"].Hidden = true;
            e.Layout.Bands[0].Columns["JOB_RID"].Hidden = true;
            e.Layout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].Hidden = true;
            e.Layout.Bands[0].Columns["LAST_COMPLETION_STATUS"].Hidden = true;
            e.Layout.Bands[0].Columns["LAST_RUN_DATETIME"].Hidden = true;
            e.Layout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].Hidden = true;
            e.Layout.Bands[0].Columns["CURRENT_TASKLIST_RID"].Hidden = true;
            e.Layout.Bands[0].Columns["CURRENT_TASKLIST_SEQUENCE"].Hidden = true;
            //e.Layout.Bands[0].Columns["USER_RID"].Hidden = true;
            //e.Layout.Bands[0].Columns["EXECUTION_STATUS"].Hidden = true;
            //e.Layout.Bands[0].Columns["HOLD_BY_USER_RID"].Hidden = true;
            //e.Layout.Bands[0].Columns["RELEASED_BY_USER_RID"].Hidden = true;
            //e.Layout.Bands[0].Columns["SCHED_NAME"].Hidden = true;
            //e.Layout.Bands[0].Columns["NEXT_RUN_DATETIME"].Hidden = true;

            // Column Positions
            e.Layout.Bands[0].Columns["OnHold"].Header.VisiblePosition = 0;
            e.Layout.Bands[0].Columns["SCHED_NAME"].Header.VisiblePosition = 1;
            e.Layout.Bands[0].Columns["JOB_NAME"].Header.VisiblePosition = 2;
            e.Layout.Bands[0].Columns["USER_RID"].Header.VisiblePosition = 3;
            e.Layout.Bands[0].Columns["NEXT_RUN_DATETIME"].Header.VisiblePosition = 4;
            e.Layout.Bands[0].Columns["EXECUTION_STATUS"].Header.VisiblePosition = 5;
            e.Layout.Bands[0].Columns["HOLD_BY_USER_RID"].Header.VisiblePosition = 6;
            e.Layout.Bands[0].Columns["HOLD_BY_DATETIME"].Header.VisiblePosition = 7;
            e.Layout.Bands[0].Columns["RELEASED_BY_USER_RID"].Header.VisiblePosition = 8;
            e.Layout.Bands[0].Columns["RELEASED_BY_DATETIME"].Header.VisiblePosition = 9;

            // Column Captions
            e.Layout.Bands[0].Columns["OnHold"].Header.Caption = "On Hold";
            e.Layout.Bands[0].Columns["SCHED_NAME"].Header.Caption = "Schedule Name";
            e.Layout.Bands[0].Columns["JOB_NAME"].Header.Caption = "Job Name";
            e.Layout.Bands[0].Columns["USER_RID"].Header.Caption = "User Name";
            e.Layout.Bands[0].Columns["NEXT_RUN_DATETIME"].Header.Caption = "Next Run Date";
            e.Layout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].Header.Caption = "Repeat Until";
            e.Layout.Bands[0].Columns["EXECUTION_STATUS"].Header.Caption = "Execution Status";
            e.Layout.Bands[0].Columns["HOLD_BY_USER_RID"].Header.Caption = "On Hold By";
            e.Layout.Bands[0].Columns["HOLD_BY_DATETIME"].Header.Caption = "On Hold Date";
            e.Layout.Bands[0].Columns["RELEASED_BY_USER_RID"].Header.Caption = "Released By";
            e.Layout.Bands[0].Columns["RELEASED_BY_DATETIME"].Header.Caption = "Released Date";

            // No Edit
            e.Layout.Bands[0].Columns["SCHED_NAME"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["JOB_NAME"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["USER_RID"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["NEXT_RUN_DATETIME"].CellActivation = Activation.NoEdit;
            //e.Layout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["EXECUTION_STATUS"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["HOLD_BY_USER_RID"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["HOLD_BY_DATETIME"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["RELEASED_BY_USER_RID"].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns["RELEASED_BY_DATETIME"].CellActivation = Activation.NoEdit;


            e.Layout.Bands[0].Columns["OnHold"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            e.Layout.Bands[0].Columns["OnHold"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            e.Layout.Bands[0].Columns["OnHold"].CellClickAction = CellClickAction.Edit;
            //this.ultraGrid1.Rows[0].Cells["Date Range"].Appearance.Image = ;

            e.Layout.Bands[0].Columns["NEXT_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
            e.Layout.Bands[0].Columns["HOLD_BY_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
            e.Layout.Bands[0].Columns["RELEASED_BY_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";

            // Sort the items in the drop down by ProductName column.
            e.Layout.Bands[0].SortedColumns.Clear();
            e.Layout.Bands[0].SortedColumns.Add("JOB_NAME", false);

            // Auto Resize Columns
            e.Layout.Bands[0].Columns["SCHED_NAME"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            e.Layout.Bands[0].Columns["JOB_NAME"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            e.Layout.Bands[0].Columns["NEXT_RUN_DATETIME"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            e.Layout.Bands[0].Columns["HOLD_BY_DATETIME"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            e.Layout.Bands[0].Columns["RELEASED_BY_DATETIME"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);


            // Value lists
            ultraGrid1.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].ValueList = ultraGrid1.DisplayLayout.ValueLists["ExecutionStatus"];
            ultraGrid1.DisplayLayout.Bands[0].Columns["USER_RID"].ValueList = ultraGrid1.DisplayLayout.ValueLists["Users"];
            ultraGrid1.DisplayLayout.Bands[0].Columns["HOLD_BY_USER_RID"].ValueList = ultraGrid1.DisplayLayout.ValueLists["Users"];
            ultraGrid1.DisplayLayout.Bands[0].Columns["RELEASED_BY_USER_RID"].ValueList = ultraGrid1.DisplayLayout.ValueLists["Users"];


        }


        private void ultraGrid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
        }

        private void ultraGrid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
        }

        private void ultraGrid1_ClickCell(object sender, ClickCellEventArgs e)
        {
        }

        private void ultraGrid1_CellChange(object sender, CellEventArgs e)
        {
            bool onHold = false;
            if (FormLoaded)
            {
                if (e.Cell.Column.Key == "OnHold")
                {
                    onHold = bool.Parse(e.Cell.Text);
                    UpdateGridWithStatus(e.Cell, onHold);
                }
                ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, false, false);
            }

        }

        private bool UpdateGridWithStatus(UltraGridCell aCell, bool onHold)
        {
            if (onHold)
            {
                if ((int)aCell.Row.Cells["EXECUTION_STATUS"].Value != 801603)  //801603 = "On Hold"
                {
                    ChangePending = true;
                }
            }
            else
            {
                if ((int)aCell.Row.Cells["EXECUTION_STATUS"].Value != 801601)  //801601 = "Waiting"
                {

                    ChangePending = true;
                }
            }
            return onHold;
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = null;
            MIDComboBoxEnh.MyComboBox cmbUsers = null;

            switch (e.Tool.Key)
            {
                case "ButtonToolHoldUserJobs":
                    cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.ultraToolbarsManager1.Tools["ControlContainerToolUsers"];
                    cmbUsers = (MIDComboBoxEnh.MyComboBox)cct.Control;
                    HoldUserJobs(cmbUsers);
                    break;

                case "ButtonToolReleaseUserJobs":
                    cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.ultraToolbarsManager1.Tools["ControlContainerToolUsers"];
                    cmbUsers = (MIDComboBoxEnh.MyComboBox)cct.Control;
                    ReleaseUserJobs(cmbUsers);
                    break;

                case "ButtonToolHoldAllJobs":
                    HoldAllJobs();
                    break;

                case "ButtonToolReleaseAllJobs":
                    ReleaseAllJobs();
                    break;

                case "ButtonToolApply":
                    ApplyChanges(true);
                    break;

                case "ButtonToolRefresh":
                    RefreshJobs();
                    break;
            }
        }

        private void HoldUserJobs(MIDComboBoxEnh.MyComboBox cmbUsers)
        {
            this.ultraGrid1.BeginUpdate();
            try
            {
                foreach (UltraGridRow row in this.ultraGrid1.Rows)
                {
                    if (Convert.ToInt32(cmbUsers.SelectedValue) == Convert.ToInt32(row.Cells["USER_RID"].Value))
                    {
                        row.Cells["OnHold"].Value = true;
                        UpdateGridWithStatus(row.Cells["OnHold"], true);
                    }
                }
            }
            finally
            {
                this.ultraGrid1.EndUpdate();
            }

        }

        private void HoldAllJobs()
        {
            this.ultraGrid1.BeginUpdate();
            try
            {
                foreach (UltraGridRow row in this.ultraGrid1.Rows)
                {
                    row.Cells["OnHold"].Value = true;
                    UpdateGridWithStatus(row.Cells["OnHold"], true);
                }
            }
            finally
            {
                this.ultraGrid1.EndUpdate();
            }
        }

        private void ReleaseUserJobs(MIDComboBoxEnh.MyComboBox cmbUsers)
        {
            this.ultraGrid1.BeginUpdate();
            try
            {
                foreach (UltraGridRow row in this.ultraGrid1.Rows)
                {
                    if (Convert.ToInt32(cmbUsers.SelectedValue) == Convert.ToInt32(row.Cells["USER_RID"].Value))
                    {
                        row.Cells["OnHold"].Value = false;
                        UpdateGridWithStatus(row.Cells["OnHold"], false);
                    }
                }
            }
            finally
            {
                this.ultraGrid1.EndUpdate();
            }
        }

        private void ReleaseAllJobs()
        {
            this.ultraGrid1.BeginUpdate();
            try
            {
                foreach (UltraGridRow row in this.ultraGrid1.Rows)
                {
                    row.Cells["OnHold"].Value = false;
                    UpdateGridWithStatus(row.Cells["OnHold"], false);
                }
            }
            finally
            {
                this.ultraGrid1.EndUpdate();
            }
        }

        private void ApplyChanges(bool refreshJobs)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                foreach (UltraGridRow aRow in this.ultraGrid1.Rows.GetRowEnumerator(GridRowType.DataRow, null, null))
                {
                    bool onHold = bool.Parse(aRow.Cells["OnHold"].Value.ToString());
                    int schedRid = int.Parse(aRow.Cells["SCHED_RID"].Value.ToString());
                    int jobRid = int.Parse(aRow.Cells["JOB_RID"].Value.ToString());
                    eProcessExecutionStatus execStatus = (eProcessExecutionStatus)int.Parse(aRow.Cells["EXECUTION_STATUS"].Value.ToString());

                    if (onHold)
                    {
                        if (execStatus == eProcessExecutionStatus.Waiting)
                        {
#if (DEBUG)
#else
                            _SAB.SchedulerServerSession.HoldJob(schedRid, jobRid, _SAB.ClientServerSession.UserRID);
#endif
                        }
                    }
                    else
                    {
                        if (execStatus == eProcessExecutionStatus.OnHold)
                        {
#if (DEBUG)
#else
                            _SAB.SchedulerServerSession.ResumeJob(schedRid, jobRid, _SAB.ClientServerSession.UserRID);
#endif
                        }
                    }

                }

                // Need so refreshJobs() won't ask the "Save?" question.
                ChangePending = false;

                if (refreshJobs)
                {
#if (DEBUG)
#else
                    RefreshJobs();
#endif
                }


                ChangePending = false;
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void RefreshJobs()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (ChangePending)
                {
                    ResultSaveChanges = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges), "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (ResultSaveChanges == DialogResult.Yes)
                    {
                        ApplyChanges(false);
                    }
                    ChangePending = false;
                }

                LoadJobs();
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Catches close() to ask for save changes.
        /// (This should be handeled by the base class, but for some reason was never calling the Form_Closing() method.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchedulerJobManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangePending)
            {
                ResultSaveChanges = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges), "Save Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (ResultSaveChanges)
                {
                    case DialogResult.Yes:
                        ApplyChanges(false);
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}
