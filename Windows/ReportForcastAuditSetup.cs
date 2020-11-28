using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using CrystalDecisions.Windows.Forms;
//using CrystalDecisions.ReportSource;
//using CrystalDecisions.CrystalReports.ViewerObjectModel;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class frmReportForcastAuditSetup : ReportFormBase
	{
		#region Controls
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.TextBox txtMerchandise;
		private System.Windows.Forms.Label lblLowLevels;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboLowLevels;
		private System.Windows.Forms.Label lblVersion;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboVersion;
		private System.Windows.Forms.Label lblTimePeriod;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsTimePeriod;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		#endregion

		#region Member Vars
		private int _currentLowLevelNode = Include.NoRID;
		private int _longestHighestGuest = Include.NoRID;
		private int _longestBranch = Include.NoRID;
		private SecurityAdmin _secAdmin;
		private CalendarDateSelector _frm;
		private string _reportName;
        SessionAddressBlock _SAB;

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.DateTimePicker dateTimePicker3;
		private System.Windows.Forms.DateTimePicker dateTimePicker4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUsers;
		private System.Windows.Forms.Label lblUsers;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUserGroups;
		private System.Windows.Forms.Label lblUserGroups;
		private System.Windows.Forms.GroupBox gbrTimePeriod;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.Label lblFromDate;
		private System.Windows.Forms.Label lblToDate;
		private System.Windows.Forms.DateTimePicker dtpDate;

        private bool _userGroupChanged = false;     // TT#209
        private bool _userChanged = false;          // TT#209

		#region Properties

		#endregion

		#region Constructors and Initialization Code
		private System.ComponentModel.Container components = null;

        public frmReportForcastAuditSetup(SessionAddressBlock sab, string reportName) : base(sab)
        {
            _SAB = sab;
            InitializeComponent();
            _secAdmin = new SecurityAdmin();
            _reportName = reportName;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
            this.drsTimePeriod.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsTimePeriod_OnSelection);
            this.drsTimePeriod.Load -= new System.EventHandler(this.drsTimePeriod_Load);
            this.drsTimePeriod.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.drsTimePeriod_ClickCellButton);
            this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
            this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
            this.cboUserGroups.SelectionChangeCommitted -= new System.EventHandler(this.cboUserGroups_SelectionChangeCommitted);
            this.cboUsers.SelectionChangeCommitted -= new System.EventHandler(this.cboUsers_SelectionChangeCommitted);
            this.cboUserGroups.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboUserGroups_MIDComboBoxPropertiesChangedEvent);
            this.cboUsers.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboUsers_MIDComboBoxPropertiesChangedEvent);
            this.Load -= new System.EventHandler(this.frmReportForcastAuditSetup_Load);
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
            this.lblLowLevels = new System.Windows.Forms.Label();
            this.cboLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cboVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.drsTimePeriod = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboUserGroups = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblUserGroups = new System.Windows.Forms.Label();
            this.cboUsers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblUsers = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbrTimePeriod = new System.Windows.Forms.GroupBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbrTimePeriod.SuspendLayout();
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
            this.txtMerchandise.Location = new System.Drawing.Point(80, 8);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(208, 20);
            this.txtMerchandise.TabIndex = 1;
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            // 
            // lblLowLevels
            // 
            this.lblLowLevels.Location = new System.Drawing.Point(8, 40);
            this.lblLowLevels.Name = "lblLowLevels";
            this.lblLowLevels.Size = new System.Drawing.Size(64, 16);
            this.lblLowLevels.TabIndex = 2;
            this.lblLowLevels.Text = "Low Levels:";
            // 
            // cboLowLevels
            // 
            this.cboLowLevels.Enabled = false;
            this.cboLowLevels.Location = new System.Drawing.Point(80, 40);
            this.cboLowLevels.Name = "cboLowLevels";
            this.cboLowLevels.Size = new System.Drawing.Size(208, 21);
            this.cboLowLevels.TabIndex = 3;
            this.cboLowLevels.Text = "(Low Levels)";
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(8, 72);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 16);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version:";
            // 
            // cboVersion
            // 
            this.cboVersion.Location = new System.Drawing.Point(80, 72);
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.Size = new System.Drawing.Size(208, 21);
            this.cboVersion.TabIndex = 5;
            this.cboVersion.Text = "(Version)";
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.Location = new System.Drawing.Point(8, 104);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblTimePeriod.TabIndex = 6;
            this.lblTimePeriod.Text = "Time Period:";
            // 
            // drsTimePeriod
            // 
            this.drsTimePeriod.DateRangeForm = null;
            this.drsTimePeriod.DateRangeRID = 0;
            this.drsTimePeriod.Location = new System.Drawing.Point(80, 104);
            this.drsTimePeriod.Name = "drsTimePeriod";
            this.drsTimePeriod.Size = new System.Drawing.Size(208, 26);
            this.drsTimePeriod.TabIndex = 7;
            this.drsTimePeriod.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsTimePeriod_OnSelection);
            this.drsTimePeriod.Load += new System.EventHandler(this.drsTimePeriod_Load);
            this.drsTimePeriod.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.drsTimePeriod_ClickCellButton);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(152, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(72, 304);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboUserGroups);
            this.groupBox2.Controls.Add(this.lblUserGroups);
            this.groupBox2.Controls.Add(this.cboUsers);
            this.groupBox2.Controls.Add(this.lblUsers);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(8, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 80);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // cboUserGroups
            // 
            this.cboUserGroups.Location = new System.Drawing.Point(96, 48);
            this.cboUserGroups.Name = "cboUserGroups";
            this.cboUserGroups.Size = new System.Drawing.Size(184, 21);
            this.cboUserGroups.TabIndex = 14;
            this.cboUserGroups.Text = "(User Groups)";
            this.cboUserGroups.SelectionChangeCommitted += new System.EventHandler(this.cboUserGroups_SelectionChangeCommitted);
            this.cboUserGroups.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboUserGroups_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblUserGroups
            // 
            this.lblUserGroups.Location = new System.Drawing.Point(8, 48);
            this.lblUserGroups.Name = "lblUserGroups";
            this.lblUserGroups.Size = new System.Drawing.Size(88, 16);
            this.lblUserGroups.TabIndex = 13;
            this.lblUserGroups.Text = "Or User Group:";
            // 
            // cboUsers
            // 
            this.cboUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsers.Location = new System.Drawing.Point(96, 16);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(184, 21);
            this.cboUsers.TabIndex = 12;
            this.cboUsers.SelectionChangeCommitted += new System.EventHandler(this.cboUsers_SelectionChangeCommitted);
            this.cboUsers.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboUsers_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblUsers
            // 
            this.lblUsers.Location = new System.Drawing.Point(8, 16);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(40, 16);
            this.lblUsers.TabIndex = 11;
            this.lblUsers.Text = "Users:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dateTimePicker3);
            this.groupBox3.Controls.Add(this.dateTimePicker4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(8, 104);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(256, 80);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CustomFormat = "MM/dd/yyyy";
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(88, 48);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(160, 20);
            this.dateTimePicker3.TabIndex = 4;
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.CustomFormat = "MM/dd/yyyy";
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(88, 16);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.Size = new System.Drawing.Size(160, 20);
            this.dateTimePicker4.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Process To:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Process From:";
            // 
            // gbrTimePeriod
            // 
            this.gbrTimePeriod.Controls.Add(this.dtpFromDate);
            this.gbrTimePeriod.Controls.Add(this.lblFromDate);
            this.gbrTimePeriod.Controls.Add(this.lblToDate);
            this.gbrTimePeriod.Controls.Add(this.dtpDate);
            this.gbrTimePeriod.Location = new System.Drawing.Point(8, 216);
            this.gbrTimePeriod.Name = "gbrTimePeriod";
            this.gbrTimePeriod.Size = new System.Drawing.Size(296, 80);
            this.gbrTimePeriod.TabIndex = 14;
            this.gbrTimePeriod.TabStop = false;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "MM/dd/yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(80, 16);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpFromDate.Size = new System.Drawing.Size(200, 20);
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
            this.dtpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpDate.TabIndex = 5;
            // 
            // frmReportForcastAuditSetup
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(312, 334);
            this.Controls.Add(this.gbrTimePeriod);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.drsTimePeriod);
            this.Controls.Add(this.lblTimePeriod);
            this.Controls.Add(this.cboVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.cboLowLevels);
            this.Controls.Add(this.lblLowLevels);
            this.Controls.Add(this.txtMerchandise);
            this.Controls.Add(this.lblMerchandise);
            this.Name = "frmReportForcastAuditSetup";
            this.Text = "Forecast Audit Selection";
            this.Load += new System.EventHandler(this.frmReportForcastAuditSetup_Load);
            this.Controls.SetChildIndex(this.lblMerchandise, 0);
            this.Controls.SetChildIndex(this.txtMerchandise, 0);
            this.Controls.SetChildIndex(this.lblLowLevels, 0);
            this.Controls.SetChildIndex(this.cboLowLevels, 0);
            this.Controls.SetChildIndex(this.lblVersion, 0);
            this.Controls.SetChildIndex(this.cboVersion, 0);
            this.Controls.SetChildIndex(this.lblTimePeriod, 0);
            this.Controls.SetChildIndex(this.drsTimePeriod, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.gbrTimePeriod, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.gbrTimePeriod.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#endregion

		#region Event Handlers
		private void frmReportForcastAuditSetup_Load(object sender, System.EventArgs e)
		{
            FormLoaded = false;         // TT#209 
			PopulateVersions();
			PopulateUsers();

            if (_reportName == "ForecastAuditMerchandise")
			{
				this.Text = "Forecast Audit by Merchandise";
                // Begin TT#209 - RMatelic - Audit Reports do not allow drag/drop of merchandise into selection
                FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsForecastAuditMerchandise);
                // End TT#209
            }
			else
			{
				this.Text = "Forecast Audit by Method";
                // Begin TT#209 - RMatelic - Audit Reports do not allow drag/drop of merchandise into selection
                FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsForecastAuditMethod);
                // End TT#209
			}

            SetReadOnly(FunctionSecurity.AllowUpdate);   // TT#209 
            FormLoaded = true;                           // TT#209 
		}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		protected void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            txtBaseMerchandise_DragDrop(sender, e);
			PopulateLowLevels();
		}

        protected void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            txtBaseMerchandise_DragEnter(sender, e);
        }

        private void txtMerchandise_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        protected void txtMerchandise_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            txtBaseMerchandise_KeyPress(sender, e);
        }

        protected void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                txtBaseMerchandise_Validating(sender, e);
                if (NodeRID != Include.NoRID)
                {
                    PopulateLowLevels();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void txtMerchandise_Validated(object sender, System.EventArgs e)
        {
            txtBaseMerchandise_Validated(sender, e);
        }

		protected void drsTimePeriod_Load(object sender, System.EventArgs e)
		{
			try
			{
				_frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				drsTimePeriod.DateRangeForm = _frm;
			}
			catch(Exception ex)
			{
				HandleException(ex, "drsTimePeriod_Load");
			}
		}

		protected void drsTimePeriod_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			// tells the date range selector the currently selected date range RID
			if (drsTimePeriod.Tag != null)
				((CalendarDateSelector)drsTimePeriod.DateRangeForm).DateRangeRID = (int)drsTimePeriod.Tag;

			((CalendarDateSelector)drsTimePeriod.DateRangeForm).AllowDynamicToStoreOpen = false;
			((CalendarDateSelector)drsTimePeriod.DateRangeForm).AllowDynamicToPlan = false;
			((CalendarDateSelector)drsTimePeriod.DateRangeForm).RestrictToOnlyWeeks = true;
			((CalendarDateSelector)drsTimePeriod.DateRangeForm).AllowDynamicSwitch = true;

			drsTimePeriod.Enabled = true;
			// tells the control to show the date range selector form
			drsTimePeriod.ShowSelector();
		}

		protected void drsTimePeriod_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				DateRangeProfile dr = e.SelectedDateRange;

				if (dr != null)
				{					
					try
					{
						drsTimePeriod.Text= dr.DisplayDate;
		
						//Add RID to Control's Tag (for later use)
						int lAddTag = dr.Key;
		
						drsTimePeriod.Tag = lAddTag;
						drsTimePeriod.DateRangeRID = lAddTag;

						//Display Dynamic picture or not
						if (dr.DateRangeType == eCalendarRangeType.Dynamic)
							drsTimePeriod.SetImage(this.DynamicToCurrentImage);
						else
							drsTimePeriod.SetImage(null);
						//=========================================================
						// Override the image if this is a dynamic switch date.
						//=========================================================
						if (dr.IsDynamicSwitch)
							drsTimePeriod.SetImage(this.DynamicSwitchImage);
					}
					catch(Exception ex)
					{
						HandleException(ex, "LoadDateRangeText");
					}
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "drsTimePeriod_OnSelection");
			}
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

                //Get theMerchandise node details
                int nodeRid = this.NodeRID;
                String nodeName = txtMerchandise.Text;

                //Get the low level details
                int lowLevelNo = 0;
                String lowLevelStr = "";
                if (cboLowLevels.SelectedIndex >= 0)
                {
                    LowLevelCombo lowLevelComb = (LowLevelCombo)cboLowLevels.SelectedItem;
                    lowLevelNo = lowLevelComb.LowLevelSequence;
                    lowLevelStr = lowLevelComb.LowLevelName;
                }

                //Get Vesion Detals
                int versionRid = 0;
                String versionName = "";
                if (this.cboVersion.SelectedIndex >= 0)
                {
                    System.Data.DataRowView drv = (System.Data.DataRowView)this.cboVersion.SelectedItem;
                    versionRid = Int32.Parse(drv["Key"].ToString());
                    versionName = drv["Description"].ToString();
                }


                //Get User Details
                int userRid = 0;
                String userName = "";
                if (cboUsers.SelectedIndex >= 0)
                {
                    System.Data.DataRowView drv = (System.Data.DataRowView)this.cboUsers.SelectedItem;
					// Begin TT#1571 - stodd - user group not selecting correctly
					int holdUserRid = Int32.Parse(drv["USER_RID"].ToString());
					if (holdUserRid > 0)
					{
						userRid = Int32.Parse(drv["USER_RID"].ToString());
						userName = drv["USER_NAME"].ToString();
					}
					// Begin TT#1571 - stodd
                }
                //Get User Group Details
                int userGroupRid = 0;
                String groupName = "";
                if (cboUserGroups.SelectedIndex >= 0 && userRid == 0)
                {
                    System.Data.DataRowView drv = (System.Data.DataRowView)this.cboUserGroups.SelectedItem;
                    userGroupRid = Int32.Parse(drv["GROUP_RID"].ToString());
                    groupName = drv["GROUP_NAME"].ToString();
                }


                //Get the Date Range info
                String firstWeek = "";
                String lastWeek = "";
                String timePeriod = "";
                int cdrRid = drsTimePeriod.DateRangeRID;
                if (cdrRid > 0)
                {
                    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(cdrRid);
                    ProfileList pList = SAB.ClientServerSession.Calendar.GetWeekRange(drp, null);
                    if (pList.Count > 0)
                    {
                        timePeriod = pList[0].ToString() + "-" + pList[pList.Count - 1].ToString();
                        String fWeek = pList[0].ToString().Replace("Week", "").Replace(" ", "");
                        String lWeek = pList[pList.Count - 1].ToString().Replace("Week", "").Replace(" ", "");
                        firstWeek = fWeek.Substring(fWeek.IndexOf("/") + 1) + fWeek.Substring(0, fWeek.IndexOf("/"));
                        lastWeek = lWeek.Substring(lWeek.IndexOf("/") + 1) + lWeek.Substring(0, lWeek.IndexOf("/"));
                    }
                }

                String processFromDate = dtpFromDate.Value.ToString("yyyyMMdd");
                String processToDate = dtpDate.Value.ToString("yyyyMMdd");

                ReportData reportData = new ReportData();
                //Windows.CrystalReports.ForecastAuditMerchandise forecastAuditMerchandiseReport = new Windows.CrystalReports.ForecastAuditMerchandise();
                //Windows.CrystalReports.ForecastAuditMethod forecastAuditMethodReport = new Windows.CrystalReports.ForecastAuditMethod();
                if (_reportName == "ForecastAuditMerchandise")
                {
                    //Windows.CrystalReports.ForecastAuditMerchandise forecastAuditReport = new Windows.CrystalReports.ForecastAuditMerchandise();
                    System.Data.DataSet ds = MIDEnvironment.CreateDataSet("ForecastAuditMerchandiseDataSet");
                    reportData.ForecastAuditMerchandise_Report(ds,
                                                              this.NodeRID == -1 ? 0 : this.NodeRID,
                                                              lowLevelNo,
                                                              versionRid,
                                                              userRid,
                                                              userGroupRid,
                                                              firstWeek,
                                                              lastWeek,
                                                              processFromDate,
                                                              processToDate);
                    //forecastAuditReport.SetDataSource(ds);

                    //forecastAuditReport.SetParameterValue("@NodeName", nodeName);
                    //forecastAuditReport.SetParameterValue("@LowerLevel", lowLevelStr);
                    //forecastAuditReport.SetParameterValue("@Timeperiod", timePeriod);

                    //frmReportViewer viewer = new frmReportViewer(_SAB, eReportType.ForecastAuditSetup);
                    //frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reportType: eReportType.ForecastAuditSetup, reportName: "ForecastAuditSetup", reportTitle: "Forecast Audit Setup");
                    List<ReportInfo> reports = new List<ReportInfo>();
                    reports.Add(new ReportInfo(aReportSource: ds,
                        reportType: eReportType.ForecastAuditSetup,
                        reportName: "ForecastAuditSetup",
                        reportTitle: "Logilitity - RO - Forecast Audit Setup",
                        reportComment: "",
                        reportInformation: "",
                        displayValue: "Forecast Audit Setup"
                        ));
                    frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reports: reports);
                    viewer.Text = "Forecast Audit Merchandise";
                    viewer.MdiParent = this.ParentForm;
                    viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    //viewer.ReportSource = ds;
                    viewer.Show();
                    viewer.BringToFront();
                }
                else
                {
                    //Windows.CrystalReports.ForecastAuditMethod forecastAuditReport = new Windows.CrystalReports.ForecastAuditMethod();
                    System.Data.DataSet ds1 = MIDEnvironment.CreateDataSet("ForecastReportNamesDataSet");
                    reportData.ForecastReportNames_Report(ds1);
                    //forecastAuditReport.SetDataSource(ds1);

                    System.Data.DataSet ds2 = MIDEnvironment.CreateDataSet("ForecastAuditOTSForecastDataSet");
                    reportData.ForecastAuditOTSForecast_Report(ds2,
                                                              this.NodeRID == -1 ? 0 : this.NodeRID,
                                                              lowLevelNo,
                                                              versionRid,
                                                              userRid,
                                                              userGroupRid,
                                                              firstWeek,
                                                              lastWeek);
                    //forecastAuditReport.Subreports["Forecast Audit OTS Forecast.rpt"].SetDataSource(ds2);

                    System.Data.DataSet ds3 = MIDEnvironment.CreateDataSet("ForecastAuditModifySalesDataSet");
                    reportData.ForecastAuditModifySales_Report(ds3,
                                                              this.NodeRID == -1 ? 0 : this.NodeRID,
                                                              lowLevelNo,
                                                              versionRid,
                                                              userRid,
                                                              userGroupRid,
                                                              firstWeek,
                                                              lastWeek);
                    //forecastAuditReport.Subreports["Fore Audit Modify Sales.rpt"].SetDataSource(ds3);

                    //forecastAuditReport.SetParameterValue("@SELECTED_NODE_RID", this.NodeRID == -1 ? 0 : this.NodeRID);
                    //forecastAuditReport.SetParameterValue("@LOWER_LEVEL", lowLevelNo);
                    //forecastAuditReport.SetParameterValue("@FV_RID", versionRid);
                    //forecastAuditReport.SetParameterValue("@USER_RID", userRid);
                    //forecastAuditReport.SetParameterValue("@USER_GROUP_RID", userGroupRid);

                    //forecastAuditReport.SetParameterValue("@NodeName", nodeName);
                    //forecastAuditReport.SetParameterValue("@LowerLevel", lowLevelStr);
                    //forecastAuditReport.SetParameterValue("@Timeperiod", timePeriod);
                    //forecastAuditReport.SetParameterValue("@TIME_RANGE_BEGIN", firstWeek);
                    //forecastAuditReport.SetParameterValue("@TIME_RANGE_END", lastWeek);

                    //frmReportViewer viewer = new frmReportViewer(_SAB, eReportType.ForecastAuditSetup);
                    //frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reportType: eReportType.ForecastAuditSetup, reportName: "ForecastAuditSetup", reportTitle: "Forecast Audit Setup");
                    List<ReportInfo> reports = new List<ReportInfo>();
                    reports.Add(new ReportInfo(aReportSource: ds1,
                        reportType: eReportType.ForecastAuditSetup,
                        reportName: "ForecastAuditSetup",
                        reportTitle: "Logilitity - RO - Forecast Audit Setup",
                        reportComment: "",
                        reportInformation: "",
                        displayValue: "Forecast Audit Setup"
                        ));
                    frmReportViewer viewer = new frmReportViewer(aSAB: _SAB, reports: reports);
                    viewer.Text = "Forecast Audit Method";
                    viewer.MdiParent = this.ParentForm;
                    viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    //viewer.ReportSource = ds1;
                    viewer.Show();
                    viewer.BringToFront();
                }
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
		private void PopulateLowLevels()
		{
			try
			{
				HierarchyProfile hierProf;
				cboLowLevels.Items.Clear();
				
				if (NodeRID == Include.NoRID) return;

				HierarchyNodeProfile aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(NodeRID, false);
				
				if (aHierarchyNodeProfile != null)
				{
					cboLowLevels.Enabled = true;
					
					hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
					if (hierProf.HierarchyType == eHierarchyType.organizational)
					{
						for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
						{
							HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
							cboLowLevels.Items.Add(
								new LowLevelCombo(eLowLevelsType.HierarchyLevel,
								//Begin Track #5866 - JScott - Matrix Balance does not work
								//0,
								i - aHierarchyNodeProfile.HomeHierarchyLevel,
								//End Track #5866 - JScott - Matrix Balance does not work
								hlp.Key,
								hlp.LevelID));
						}
					}
					else
					{
						HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
							_longestHighestGuest = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
						}
						int highestGuestLevel = _longestHighestGuest;

						// add guest levels to comboBox
						if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate)) // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
						{
							for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
							{
								if (i == 0)
								{
									cboLowLevels.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										0,
										0,
										"Root"));
								}
								else
								{
									HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
									cboLowLevels.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										//Begin Track #5866 - JScott - Matrix Balance does not work
										//0,
										i,
										//End Track #5866 - JScott - Matrix Balance does not work
										hlp.Key,
										hlp.LevelID));
								}
							}
						}

						// add offsets to comboBox
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
                            //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                            //_longestBranch = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                            DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
                            //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
						}
						int longestBranchCount = _longestBranch; 
						int offset = 0;
						for (int i = 0; i < longestBranchCount; i++)
						{
							++offset;
                            // Begin TT#1040 - JSmith - parameter not compatible error
                            //cboLowLevels.Items.Add(
                            //    new LowLevelCombo(eLowLevelsType.LevelOffset,
                            //    offset,
                            //    0,
                            //    null));
                            cboLowLevels.Items.Add(
                                new LowLevelCombo(eLowLevelsType.LevelOffset,
                                offset,
                                0,
                                "+" + offset));
                            // End TT#1040
						}
					}
					if (cboLowLevels.Items.Count > 0)
					{
						cboLowLevels.SelectedIndex = 0;
					}
					
					_currentLowLevelNode = aHierarchyNodeProfile.Key;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PopulateVersions()
		{
			// Setup Versions DataTable for Listboxes
			ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
			DataTable dt = MIDEnvironment.CreateDataTable("Versions");
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("Key", typeof(int));

			dt.Rows.Add(new object[] {"", Include.NoRID});

			foreach (VersionProfile verProf in versionProfList)
			{
				if (verProf.Description != "Actual" &&
					!verProf.StoreSecurity.AccessDenied)
					dt.Rows.Add(new object[] {verProf.Description, verProf.Key});
			}

			this.cboVersion.ValueMember = "Key";
			this.cboVersion.DisplayMember = "Description";
			this.cboVersion.DataSource = dt;
		}

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
			cboUsers.SelectedValue = SAB.ClientServerSession.UserRID;
			
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