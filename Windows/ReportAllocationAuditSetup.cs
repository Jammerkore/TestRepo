using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class frmReportAllocationAuditSetup : ReportFormBase
	{
		#region Controls
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.TextBox txtMerchandise;
		private System.Windows.Forms.Label lblPlanLevel;
		private System.Windows.Forms.TextBox txtPlanLevel;
		private System.Windows.Forms.Label lblUser;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUsers;
		private System.Windows.Forms.GroupBox gbrTimePeriod;
		private System.Windows.Forms.DateTimePicker dtpDate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		#endregion

		#region Member Vars
		private SecurityAdmin _secAdmin;
        //private int _planLevelRid = Include.Undefined;  // TT#350 - RMatelic - Report does not show any allocations
        private bool _userGroupChanged = false;     // TT#209
        private bool _userChanged = false;          // TT#209
        #endregion

		private System.Windows.Forms.Label lblToDate;
		private System.Windows.Forms.Label lblFromDate;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblUserGroups;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUserGroups;
        SessionAddressBlock _SAB;                  // TT#209

		#region Properties

		#endregion

		#region Constructors and Initialization Code
		private System.ComponentModel.Container components = null;

		public frmReportAllocationAuditSetup(SessionAddressBlock sab) : base(sab)
		{
			InitializeComponent();
			_secAdmin = new SecurityAdmin();
            _SAB = sab;     // TT#209
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
            this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.Validated -= new System.EventHandler(this.txtMerchandise_Validated);
            this.txtMerchandise.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
            this.txtMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtPlanLevel.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtPlanLevel_DragDrop);
            this.txtPlanLevel.Validated -= new System.EventHandler(this.txtPlanLevel_Validated);
            this.txtPlanLevel.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtPlanLevel_KeyPress);
            this.txtPlanLevel.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPlanLevel_Validating);
            this.txtPlanLevel.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtPlanLevel_DragEnter);
            this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
            this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
            this.cboUserGroups.SelectionChangeCommitted -= new System.EventHandler(this.cboUserGroups_SelectionChangeCommitted);
            this.cboUsers.SelectionChangeCommitted -= new System.EventHandler(this.cboUsers_SelectionChangeCommitted);
            this.cboUserGroups.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboUserGroups_MIDComboBoxPropertiesChangedEvent);
            this.cboUsers.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboUsers_MIDComboBoxPropertiesChangedEvent);
            this.Load -= new System.EventHandler(this.frmReportAllocationAuditSetup_Load);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblMerchandise = new System.Windows.Forms.Label();
			this.txtMerchandise = new System.Windows.Forms.TextBox();
			this.lblPlanLevel = new System.Windows.Forms.Label();
			this.txtPlanLevel = new System.Windows.Forms.TextBox();
			this.lblUser = new System.Windows.Forms.Label();
			this.cboUsers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.gbrTimePeriod = new System.Windows.Forms.GroupBox();
			this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
			this.lblFromDate = new System.Windows.Forms.Label();
			this.lblToDate = new System.Windows.Forms.Label();
			this.dtpDate = new System.Windows.Forms.DateTimePicker();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblUserGroups = new System.Windows.Forms.Label();
			this.cboUserGroups = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.gbrTimePeriod.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// lblMerchandise
			// 
			this.lblMerchandise.Location = new System.Drawing.Point(8, 8);
			this.lblMerchandise.Name = "lblMerchandise";
			this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
			this.lblMerchandise.TabIndex = 0;
			this.lblMerchandise.Text = "Merchandise:";
			// 
			// txtMerchandise
			// 
			this.txtMerchandise.AllowDrop = true;
			this.txtMerchandise.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMerchandise.Location = new System.Drawing.Point(80, 8);
			this.txtMerchandise.Name = "txtMerchandise";
			this.txtMerchandise.Size = new System.Drawing.Size(200, 20);
			this.txtMerchandise.TabIndex = 1;
			this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
			this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
			this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
			this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
			this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
			this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
			// 
			// lblPlanLevel
			// 
			this.lblPlanLevel.Location = new System.Drawing.Point(8, 40);
			this.lblPlanLevel.Name = "lblPlanLevel";
			this.lblPlanLevel.Size = new System.Drawing.Size(64, 16);
			this.lblPlanLevel.TabIndex = 0;
			this.lblPlanLevel.Text = "Plan Level:";
			// 
			// txtPlanLevel
			// 
			this.txtPlanLevel.AllowDrop = true;
			this.txtPlanLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPlanLevel.Location = new System.Drawing.Point(80, 40);
			this.txtPlanLevel.Name = "txtPlanLevel";
			this.txtPlanLevel.Size = new System.Drawing.Size(200, 20);
			this.txtPlanLevel.TabIndex = 2;
			this.txtPlanLevel.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtPlanLevel_DragDrop);
			this.txtPlanLevel.Validated += new System.EventHandler(this.txtPlanLevel_Validated);
			this.txtPlanLevel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPlanLevel_KeyPress);
			this.txtPlanLevel.Validating += new System.ComponentModel.CancelEventHandler(this.txtPlanLevel_Validating);
			this.txtPlanLevel.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtPlanLevel_DragEnter);
			this.txtPlanLevel.DragOver += new System.Windows.Forms.DragEventHandler(this.txtPlanLevel_DragOver);
			// 
			// lblUser
			// 
			this.lblUser.Location = new System.Drawing.Point(16, 16);
			this.lblUser.Name = "lblUser";
			this.lblUser.Size = new System.Drawing.Size(32, 16);
			this.lblUser.TabIndex = 0;
			this.lblUser.Text = "User:";
			// 
			// cboUsers
			// 
			this.cboUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboUsers.Location = new System.Drawing.Point(96, 16);
			this.cboUsers.Name = "cboUsers";
			this.cboUsers.Size = new System.Drawing.Size(168, 21);
			this.cboUsers.TabIndex = 3;
			this.cboUsers.SelectionChangeCommitted += new System.EventHandler(this.cboUsers_SelectionChangeCommitted);
            this.cboUsers.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboUsers_MIDComboBoxPropertiesChangedEvent);
			// 
			// gbrTimePeriod
			// 
			this.gbrTimePeriod.Controls.Add(this.dtpFromDate);
			this.gbrTimePeriod.Controls.Add(this.lblFromDate);
			this.gbrTimePeriod.Controls.Add(this.lblToDate);
			this.gbrTimePeriod.Controls.Add(this.dtpDate);
			this.gbrTimePeriod.Location = new System.Drawing.Point(8, 160);
			this.gbrTimePeriod.Name = "gbrTimePeriod";
			this.gbrTimePeriod.Size = new System.Drawing.Size(272, 80);
			this.gbrTimePeriod.TabIndex = 6;
			this.gbrTimePeriod.TabStop = false;
			// 
			// dtpFromDate
			// 
			this.dtpFromDate.CustomFormat = "MM/dd/yyyy";
			this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpFromDate.Location = new System.Drawing.Point(80, 16);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.dtpFromDate.Size = new System.Drawing.Size(184, 20);
			this.dtpFromDate.TabIndex = 9;
			// 
			// lblFromDate
			// 
			this.lblFromDate.Location = new System.Drawing.Point(8, 24);
			this.lblFromDate.Name = "lblFromDate";
			this.lblFromDate.Size = new System.Drawing.Size(64, 16);
			this.lblFromDate.TabIndex = 8;
			this.lblFromDate.Text = "From Date:";
			// 
			// lblToDate
			// 
			this.lblToDate.Location = new System.Drawing.Point(8, 48);
			this.lblToDate.Name = "lblToDate";
			this.lblToDate.Size = new System.Drawing.Size(48, 16);
			this.lblToDate.TabIndex = 7;
			this.lblToDate.Text = "To Date:";
			// 
			// dtpDate
			// 
			this.dtpDate.CustomFormat = "MM/dd/yyyy";
			this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpDate.Location = new System.Drawing.Point(80, 48);
			this.dtpDate.Name = "dtpDate";
			this.dtpDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.dtpDate.Size = new System.Drawing.Size(184, 20);
			this.dtpDate.TabIndex = 5;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.Location = new System.Drawing.Point(160, 256);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.Location = new System.Drawing.Point(72, 256);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblUserGroups);
			this.groupBox1.Controls.Add(this.cboUserGroups);
			this.groupBox1.Controls.Add(this.lblUser);
			this.groupBox1.Controls.Add(this.cboUsers);
			this.groupBox1.Location = new System.Drawing.Point(8, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 80);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			// 
			// lblUserGroups
			// 
			this.lblUserGroups.Location = new System.Drawing.Point(8, 48);
			this.lblUserGroups.Name = "lblUserGroups";
			this.lblUserGroups.Size = new System.Drawing.Size(88, 16);
			this.lblUserGroups.TabIndex = 4;
			this.lblUserGroups.Text = "Or User Group:";
			// 
			// cboUserGroups
			// 
			this.cboUserGroups.Location = new System.Drawing.Point(96, 48);
			this.cboUserGroups.Name = "cboUserGroups";
			this.cboUserGroups.Size = new System.Drawing.Size(168, 21);
			this.cboUserGroups.TabIndex = 5;
			this.cboUserGroups.Text = "(User Groups)";
			this.cboUserGroups.SelectionChangeCommitted += new System.EventHandler(this.cboUserGroups_SelectionChangeCommitted);
            this.cboUserGroups.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboUserGroups_MIDComboBoxPropertiesChangedEvent);
			// 
			// frmReportAllocationAuditSetup
			// 
			this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 286);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.gbrTimePeriod);
			this.Controls.Add(this.txtPlanLevel);
			this.Controls.Add(this.txtMerchandise);
			this.Controls.Add(this.lblPlanLevel);
			this.Controls.Add(this.lblMerchandise);
			this.Name = "frmReportAllocationAuditSetup";
			this.Text = "Allocation Audit Selection";
			this.Load += new System.EventHandler(this.frmReportAllocationAuditSetup_Load);
			this.Controls.SetChildIndex(this.lblMerchandise, 0);
			this.Controls.SetChildIndex(this.lblPlanLevel, 0);
			this.Controls.SetChildIndex(this.txtMerchandise, 0);
			this.Controls.SetChildIndex(this.txtPlanLevel, 0);
			this.Controls.SetChildIndex(this.gbrTimePeriod, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.btnOK, 0);
			this.Controls.SetChildIndex(this.groupBox1, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.gbrTimePeriod.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		#endregion

		#region Event Handlers
		private void frmReportAllocationAuditSetup_Load(object sender, System.EventArgs e)
		{
            FormLoaded = false;         // TT#209 
			PopulateUsers();
            // Begin TT#209 - RMatelic - Audit Reports do not allow drag/drop of merchandise into selection
            FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsAllocationAudit);
            SetReadOnly(FunctionSecurity.AllowUpdate);
            FormLoaded = true;         
            // End TT#209 
		}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		private void txtPlanLevel_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            txtBasePlanLevel_DragDrop(sender, e);          // TT#350 - RMatelic - Report does not show any allocations
		}

		private void txtPlanLevel_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            txtBasePlanLevel_DragEnter(sender, e);         // TT#350 - RMatelic - Report does not show any allocations
		}

        private void txtPlanLevel_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        protected void txtPlanLevel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtBasePlanLevel_Validating(sender, e);        // TT#350 - RMatelic - Report does not show any allocations
        }

        protected void txtPlanLevel_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            txtBasePlanLevel_KeyPress(sender, e);          // TT#350 - RMatelic - Report does not show any allocations
        }

        protected void txtPlanLevel_Validated(object sender, System.EventArgs e)
        {
            txtBasePlanLevel_Validated(sender, e);         // TT#350 - RMatelic - Report does not show any allocations
        }

        protected void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            txtBaseMerchandise_DragDrop(sender, e);
        }

        protected void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            txtBaseMerchandise_DragEnter(sender, e);
        }

        private void txtMerchandise_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        protected void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtBaseMerchandise_Validating(sender, e);
        }

        protected void txtMerchandise_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            txtBaseMerchandise_KeyPress(sender, e);
        }

        protected void txtMerchandise_Validated(object sender, System.EventArgs e)
        {
            txtBaseMerchandise_Validated(sender, e);
        }

        // Begin TT#209 - RMatelic - Unrelated to specific issue - unselect other drop down 
        private void cboUsers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                if (_userGroupChanged)
                {
                    _userGroupChanged = false;
                }
                else if (Convert.ToInt32(cboUserGroups.SelectedValue, CultureInfo.CurrentUICulture)!= Include.NoRID)
                {
                    _userChanged = true;
                    cboUserGroups.SelectedValue = Include.NoRID;
                }
            }
        }

        private void cboUserGroups_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                if (_userChanged)
                {
                    _userChanged = false;
                }
                else if (Convert.ToInt32(cboUsers.SelectedValue, CultureInfo.CurrentUICulture) != Include.NoRID)
                {
                    _userGroupChanged = true;
                    cboUsers.SelectedValue = Include.NoRID;
                }
            }
        }
        // End TT#209  

        // Begin TT#316-MD - RMatelic - Replace all Windows Combobox controls with new enhanced control 
        void cboUserGroups_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboUserGroups_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboUsers_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboUsers_SelectionChangeCommitted(source, new EventArgs());
        }
        // End TT#316

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //Get User Details
                int userRid = 0;
                // Begin TT#350 - RMatelic - Report does not show any allocations; replaced previous code; do DIFF for differences  
                String userName = string.Empty;
                if (Convert.ToInt32(cboUsers.SelectedValue, CultureInfo.CurrentUICulture) != Include.NoRID)
                {
                    userRid = Convert.ToInt32(cboUsers.SelectedValue, CultureInfo.CurrentUICulture);
                    userName = cboUsers.Text;
                }
                
                //Get User Group Details
                int userGroupRid = 0;
                if (Convert.ToInt32(cboUserGroups.SelectedValue, CultureInfo.CurrentUICulture) != Include.NoRID)
                {
                    userGroupRid = Convert.ToInt32(cboUserGroups.SelectedValue, CultureInfo.CurrentUICulture);
                    userName = cboUserGroups.Text;
                }
                // End TT#350

                String processFromDate = dtpFromDate.Value.ToString("yyyyMMdd");
                String processToDate = dtpDate.Value.ToString("yyyyMMdd");

                string headerRIDTextList = GetHeaderRIDList();  // TT#397 - RMatelic - Allocation Audit Report not selecting headers when drag/drop a style/ color node

                ReportData reportData = new ReportData();
                System.Data.DataSet ds = MIDEnvironment.CreateDataSet("AllocationAuditDataSet");
                reportData.AuditAllocation_Report(ds,
                                                 this.NodeRID == -1 ? 0 : this.NodeRID,
                                                 // Begin TT#350 - RMatelic - Report does not show any allocations
                                                 //this._planLevelRid == -1 ? 0 : this._planLevelRid,
                                                 this.PlanLevelRID == -1 ? 0 : this.PlanLevelRID,
                                                 // End TT#350  
                                                 userRid,
                                                 userGroupRid,
                                                 processFromDate,
                                                 processToDate,
                                                 headerRIDTextList);    // TT#397

                Windows.CrystalReports.AllocationAudit allocationAuditReport = new Windows.CrystalReports.AllocationAudit();
                allocationAuditReport.Subreports["HeaderAuditStyleAllocationSub.rpt"].SetDataSource(ds);
                allocationAuditReport.Subreports["HeaderAuditSizeAllocationSub.rpt"].SetDataSource(ds);

                allocationAuditReport.SetParameterValue("@SELECTED_NODE_RID", this.NodeRID == -1 ? 0 : this.NodeRID);
                //allocationAuditReport.SetParameterValue("@PLAN_HNRID", this._planLevelRid);
                // Begin TT#350 - RMatelic - Report does not show any allocations
                //allocationAuditReport.SetParameterValue("@PLAN_HNRID", this._planLevelRid == -1 ? 0 : this._planLevelRid);
                allocationAuditReport.SetParameterValue("@PLAN_HNRID", this.PlanLevelRID == -1 ? 0 : this.PlanLevelRID);
                // End TT#350 
                allocationAuditReport.SetParameterValue("@USER_RID", userRid);
                allocationAuditReport.SetParameterValue("@USER_GROUP_RID", userGroupRid);
                allocationAuditReport.SetParameterValue("@PROCESS_FROM_DATE", processFromDate);
                allocationAuditReport.SetParameterValue("@PROCESS_TO_DATE", processToDate);
                // Begin TT#350 - RMatelic - Report does not show any allocations - unrelated to specific issue; use main hierarchy top node if none specified
                //allocationAuditReport.SetParameterValue("@Node", txtMerchandise.Text);
                if (this.NodeRID == -1)
                {
                    HierarchyProfile hp = _SAB.HierarchyServerSession.GetMainHierarchyData();
                    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(hp.HierarchyRootNodeRID);
                    allocationAuditReport.SetParameterValue("@Node", hnp.LevelText);
                }
                else
                {
                    allocationAuditReport.SetParameterValue("@Node", txtMerchandise.Text);
                }
                // End TT#350
                allocationAuditReport.SetParameterValue("@PlanLevel", txtPlanLevel.Text);
                allocationAuditReport.SetParameterValue("@User", userName);
                allocationAuditReport.SetParameterValue("@TimePeriod", dtpFromDate.Text + " - " + dtpDate.Text);

                frmReportViewer viewer = new frmReportViewer(_SAB);
                viewer.Text = "Allocation Audit";
                viewer.MdiParent = this.ParentForm;
                viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                viewer.ReportSource = allocationAuditReport;
                viewer.Show();
                viewer.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

		#endregion

		#region Private Methods
		private void PopulateUsers()
		{
			cboUsers.ValueMember = "USER_RID";
            cboUsers.DisplayMember = "USER_NAME";
            // Begin TT#209 - RMatelic - Unrelated to specific issue - add empty row
            //cboUsers.DataSource = _secAdmin.GetUsers();
            DataTable dtUsers = _secAdmin.GetUsers();
            DataRow emptyRow = dtUsers.NewRow();
            emptyRow["USER_RID"] = Include.NoRID;
            emptyRow["USER_NAME"] = string.Empty;
            dtUsers.Rows.Add(emptyRow);
            dtUsers.DefaultView.Sort = "USER_NAME ASC";
            dtUsers.AcceptChanges();
            // End TT#209
            cboUsers.DataSource = dtUsers;
            cboUsers.SelectedValue = _SAB.ClientServerSession.UserRID;

            cboUserGroups.ValueMember = "GROUP_RID";
            cboUserGroups.DisplayMember = "GROUP_NAME";
            // Begin TT#209 - RMatelic - Unrelated to specific issue - add empty row
            //cboUserGroups.DataSource = _secAdmin.GetActiveGroups();
            DataTable dtGroups = _secAdmin.GetActiveGroups();
            DataRow emptyRowGroup = dtGroups.NewRow();
            emptyRowGroup["GROUP_RID"] = Include.NoRID;
            emptyRowGroup["GROUP_NAME"] = string.Empty;
            dtGroups.Rows.Add(emptyRowGroup);
            dtGroups.DefaultView.Sort = "GROUP_NAME ASC";
            dtGroups.AcceptChanges();
            cboUserGroups.DataSource = dtGroups;
            //cboUserGroups.Text = "";
            cboUserGroups.SelectedValue = Include.NoRID;
            // End TT#209 
		}

		#endregion

       
	}
}
