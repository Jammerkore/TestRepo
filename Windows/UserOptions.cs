using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for UserOptions.
	/// </summary>
	public class frmUserOptions : MIDFormBase
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbxForecastMonitor;
		private System.Windows.Forms.RadioButton radForecastMonitorOn;
		private System.Windows.Forms.RadioButton radForecastMonitorOff;
		private System.Windows.Forms.GroupBox gbxModifySalesMonitor;
		private System.Windows.Forms.Label lblAuditLoggingLevel;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxAuditLoggingLevel;
		private System.Windows.Forms.CheckBox cbxShowLogin;
		private System.Windows.Forms.Label lblForecastMonitorDirectory;
		private System.Windows.Forms.Button btnForecastMonitorDirectory;
		private System.Windows.Forms.TextBox txtForecastMonitorDirectory;
		private System.Windows.Forms.Button btnModifySalesMonitor;
		private System.Windows.Forms.TextBox txtModifySalesMonitorDirectory;
		private System.Windows.Forms.Label lblModifySalesMonitorDirectory;
		private System.Windows.Forms.RadioButton radModifySalesMonitorOff;
		private System.Windows.Forms.RadioButton radModifySalesMonitorOn;
		private System.Windows.Forms.FolderBrowserDialog fbdDirectory;
        private CheckBox cbxShowSignOffPrompt;
        private GroupBox gbxDCFulfillmentMonitor;
        private Button btnDCFulfillmentMonitorDirectory;
        private TextBox txtDCFulfillmentMonitorDirectory;
        private Label lblDCFulfillmentMonitorDirectory;
        private RadioButton radDCFulfillmentMonitorOff;
        private RadioButton radDCFulfillmentMonitorOn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmUserOptions(SessionAddressBlock aSAB) : base (aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.radForecastMonitorOff.CheckedChanged -= new System.EventHandler(this.radMonitor_CheckedChanged);
				this.radForecastMonitorOn.CheckedChanged -= new System.EventHandler(this.radMonitor_CheckedChanged);
				this.btnForecastMonitorDirectory.Click -= new System.EventHandler(this.btnForecastMonitorDirectory_Click);
				this.btnModifySalesMonitor.Click -= new System.EventHandler(this.btnModifySalesMonitor_Click);
				this.radModifySalesMonitorOff.CheckedChanged -= new System.EventHandler(this.radMonitor_CheckedChanged);
				this.radModifySalesMonitorOn.CheckedChanged -= new System.EventHandler(this.radMonitor_CheckedChanged);
                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                this.radDCFulfillmentMonitorOff.CheckedChanged -= new System.EventHandler(this.radMonitor_CheckedChanged);
                this.radDCFulfillmentMonitorOn.CheckedChanged -= new System.EventHandler(this.radMonitor_CheckedChanged);
                this.btnDCFulfillmentMonitorDirectory.Click -= new System.EventHandler(this.btnDCFulfillmentMonitorDirectory_Click);
                // END TT#1966-MD - AGallagher - DC Fulfillment
                this.cbxAuditLoggingLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxAuditLoggingLevel_MIDComboBoxPropertiesChangedEvent);
				this.Load -= new System.EventHandler(this.frmUserOptions_Load);
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxForecastMonitor = new System.Windows.Forms.GroupBox();
            this.radForecastMonitorOff = new System.Windows.Forms.RadioButton();
            this.radForecastMonitorOn = new System.Windows.Forms.RadioButton();
            this.btnForecastMonitorDirectory = new System.Windows.Forms.Button();
            this.txtForecastMonitorDirectory = new System.Windows.Forms.TextBox();
            this.lblForecastMonitorDirectory = new System.Windows.Forms.Label();
            this.gbxModifySalesMonitor = new System.Windows.Forms.GroupBox();
            this.btnModifySalesMonitor = new System.Windows.Forms.Button();
            this.txtModifySalesMonitorDirectory = new System.Windows.Forms.TextBox();
            this.lblModifySalesMonitorDirectory = new System.Windows.Forms.Label();
            this.radModifySalesMonitorOff = new System.Windows.Forms.RadioButton();
            this.radModifySalesMonitorOn = new System.Windows.Forms.RadioButton();
            this.lblAuditLoggingLevel = new System.Windows.Forms.Label();
            this.cbxAuditLoggingLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbxShowLogin = new System.Windows.Forms.CheckBox();
            this.fbdDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.cbxShowSignOffPrompt = new System.Windows.Forms.CheckBox();
            this.gbxDCFulfillmentMonitor = new System.Windows.Forms.GroupBox();
            this.btnDCFulfillmentMonitorDirectory = new System.Windows.Forms.Button();
            this.txtDCFulfillmentMonitorDirectory = new System.Windows.Forms.TextBox();
            this.lblDCFulfillmentMonitorDirectory = new System.Windows.Forms.Label();
            this.radDCFulfillmentMonitorOff = new System.Windows.Forms.RadioButton();
            this.radDCFulfillmentMonitorOn = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbxForecastMonitor.SuspendLayout();
            this.gbxModifySalesMonitor.SuspendLayout();
            this.gbxDCFulfillmentMonitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(388, 397);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(476, 397);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbxForecastMonitor
            // 
            this.gbxForecastMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxForecastMonitor.Controls.Add(this.radForecastMonitorOff);
            this.gbxForecastMonitor.Controls.Add(this.radForecastMonitorOn);
            this.gbxForecastMonitor.Controls.Add(this.btnForecastMonitorDirectory);
            this.gbxForecastMonitor.Controls.Add(this.txtForecastMonitorDirectory);
            this.gbxForecastMonitor.Controls.Add(this.lblForecastMonitorDirectory);
            this.gbxForecastMonitor.Location = new System.Drawing.Point(16, 16);
            this.gbxForecastMonitor.Name = "gbxForecastMonitor";
            this.gbxForecastMonitor.Size = new System.Drawing.Size(520, 72);
            this.gbxForecastMonitor.TabIndex = 21;
            this.gbxForecastMonitor.TabStop = false;
            this.gbxForecastMonitor.Text = "Forecast Monitor";
            // 
            // radForecastMonitorOff
            // 
            this.radForecastMonitorOff.Location = new System.Drawing.Point(64, 16);
            this.radForecastMonitorOff.Name = "radForecastMonitorOff";
            this.radForecastMonitorOff.Size = new System.Drawing.Size(40, 24);
            this.radForecastMonitorOff.TabIndex = 1;
            this.radForecastMonitorOff.Text = "Off";
            this.radForecastMonitorOff.CheckedChanged += new System.EventHandler(this.radMonitor_CheckedChanged);
            // 
            // radForecastMonitorOn
            // 
            this.radForecastMonitorOn.Location = new System.Drawing.Point(8, 16);
            this.radForecastMonitorOn.Name = "radForecastMonitorOn";
            this.radForecastMonitorOn.Size = new System.Drawing.Size(40, 24);
            this.radForecastMonitorOn.TabIndex = 0;
            this.radForecastMonitorOn.Text = "On";
            this.radForecastMonitorOn.CheckedChanged += new System.EventHandler(this.radMonitor_CheckedChanged);
            // 
            // btnForecastMonitorDirectory
            // 
            this.btnForecastMonitorDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForecastMonitorDirectory.Location = new System.Drawing.Point(428, 40);
            this.btnForecastMonitorDirectory.Name = "btnForecastMonitorDirectory";
            this.btnForecastMonitorDirectory.Size = new System.Drawing.Size(72, 20);
            this.btnForecastMonitorDirectory.TabIndex = 27;
            this.btnForecastMonitorDirectory.Text = "Directory...";
            this.btnForecastMonitorDirectory.Click += new System.EventHandler(this.btnForecastMonitorDirectory_Click);
            // 
            // txtForecastMonitorDirectory
            // 
            this.txtForecastMonitorDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtForecastMonitorDirectory.Location = new System.Drawing.Point(88, 40);
            this.txtForecastMonitorDirectory.Name = "txtForecastMonitorDirectory";
            this.txtForecastMonitorDirectory.Size = new System.Drawing.Size(328, 20);
            this.txtForecastMonitorDirectory.TabIndex = 26;
            // 
            // lblForecastMonitorDirectory
            // 
            this.lblForecastMonitorDirectory.Location = new System.Drawing.Point(8, 40);
            this.lblForecastMonitorDirectory.Name = "lblForecastMonitorDirectory";
            this.lblForecastMonitorDirectory.Size = new System.Drawing.Size(72, 16);
            this.lblForecastMonitorDirectory.TabIndex = 28;
            this.lblForecastMonitorDirectory.Text = "Directory:";
            this.lblForecastMonitorDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxModifySalesMonitor
            // 
            this.gbxModifySalesMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxModifySalesMonitor.Controls.Add(this.btnModifySalesMonitor);
            this.gbxModifySalesMonitor.Controls.Add(this.txtModifySalesMonitorDirectory);
            this.gbxModifySalesMonitor.Controls.Add(this.lblModifySalesMonitorDirectory);
            this.gbxModifySalesMonitor.Controls.Add(this.radModifySalesMonitorOff);
            this.gbxModifySalesMonitor.Controls.Add(this.radModifySalesMonitorOn);
            this.gbxModifySalesMonitor.Location = new System.Drawing.Point(16, 112);
            this.gbxModifySalesMonitor.Name = "gbxModifySalesMonitor";
            this.gbxModifySalesMonitor.Size = new System.Drawing.Size(520, 80);
            this.gbxModifySalesMonitor.TabIndex = 22;
            this.gbxModifySalesMonitor.TabStop = false;
            this.gbxModifySalesMonitor.Text = "Modify Sales Monitor";
            // 
            // btnModifySalesMonitor
            // 
            this.btnModifySalesMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModifySalesMonitor.Location = new System.Drawing.Point(428, 48);
            this.btnModifySalesMonitor.Name = "btnModifySalesMonitor";
            this.btnModifySalesMonitor.Size = new System.Drawing.Size(72, 20);
            this.btnModifySalesMonitor.TabIndex = 30;
            this.btnModifySalesMonitor.Text = "Directory...";
            this.btnModifySalesMonitor.Click += new System.EventHandler(this.btnModifySalesMonitor_Click);
            // 
            // txtModifySalesMonitorDirectory
            // 
            this.txtModifySalesMonitorDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModifySalesMonitorDirectory.Location = new System.Drawing.Point(88, 48);
            this.txtModifySalesMonitorDirectory.Name = "txtModifySalesMonitorDirectory";
            this.txtModifySalesMonitorDirectory.Size = new System.Drawing.Size(328, 20);
            this.txtModifySalesMonitorDirectory.TabIndex = 29;
            // 
            // lblModifySalesMonitorDirectory
            // 
            this.lblModifySalesMonitorDirectory.Location = new System.Drawing.Point(8, 48);
            this.lblModifySalesMonitorDirectory.Name = "lblModifySalesMonitorDirectory";
            this.lblModifySalesMonitorDirectory.Size = new System.Drawing.Size(72, 16);
            this.lblModifySalesMonitorDirectory.TabIndex = 31;
            this.lblModifySalesMonitorDirectory.Text = "Directory:";
            this.lblModifySalesMonitorDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radModifySalesMonitorOff
            // 
            this.radModifySalesMonitorOff.Location = new System.Drawing.Point(64, 16);
            this.radModifySalesMonitorOff.Name = "radModifySalesMonitorOff";
            this.radModifySalesMonitorOff.Size = new System.Drawing.Size(40, 24);
            this.radModifySalesMonitorOff.TabIndex = 1;
            this.radModifySalesMonitorOff.Text = "Off";
            this.radModifySalesMonitorOff.CheckedChanged += new System.EventHandler(this.radMonitor_CheckedChanged);
            // 
            // radModifySalesMonitorOn
            // 
            this.radModifySalesMonitorOn.Location = new System.Drawing.Point(8, 16);
            this.radModifySalesMonitorOn.Name = "radModifySalesMonitorOn";
            this.radModifySalesMonitorOn.Size = new System.Drawing.Size(40, 24);
            this.radModifySalesMonitorOn.TabIndex = 0;
            this.radModifySalesMonitorOn.Text = "On";
            this.radModifySalesMonitorOn.CheckedChanged += new System.EventHandler(this.radMonitor_CheckedChanged);
            // 
            // lblAuditLoggingLevel
            // 
            this.lblAuditLoggingLevel.Location = new System.Drawing.Point(5, 306);
            this.lblAuditLoggingLevel.Name = "lblAuditLoggingLevel";
            this.lblAuditLoggingLevel.Size = new System.Drawing.Size(112, 23);
            this.lblAuditLoggingLevel.TabIndex = 23;
            this.lblAuditLoggingLevel.Text = "Audit Logging Level";
            this.lblAuditLoggingLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxAuditLoggingLevel
            // 
            this.cbxAuditLoggingLevel.AutoAdjust = true;
            this.cbxAuditLoggingLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxAuditLoggingLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxAuditLoggingLevel.DataSource = null;
            this.cbxAuditLoggingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAuditLoggingLevel.DropDownWidth = 160;
            this.cbxAuditLoggingLevel.FormattingEnabled = false;
            this.cbxAuditLoggingLevel.IgnoreFocusLost = false;
            this.cbxAuditLoggingLevel.ItemHeight = 13;
            this.cbxAuditLoggingLevel.Location = new System.Drawing.Point(125, 306);
            this.cbxAuditLoggingLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cbxAuditLoggingLevel.MaxDropDownItems = 25;
            this.cbxAuditLoggingLevel.Name = "cbxAuditLoggingLevel";
            this.cbxAuditLoggingLevel.SetToolTip = "";
            this.cbxAuditLoggingLevel.Size = new System.Drawing.Size(160, 21);
            this.cbxAuditLoggingLevel.TabIndex = 24;
            this.cbxAuditLoggingLevel.Tag = null;
            this.cbxAuditLoggingLevel.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbxAuditLoggingLevel_MIDComboBoxPropertiesChangedEvent);
            this.cbxAuditLoggingLevel.SelectionChangeCommitted += new System.EventHandler(this.cbxAuditLoggingLevel_SelectionChangeCommitted);
            // 
            // cbxShowLogin
            // 
            this.cbxShowLogin.Location = new System.Drawing.Point(21, 354);
            this.cbxShowLogin.Name = "cbxShowLogin";
            this.cbxShowLogin.Size = new System.Drawing.Size(120, 24);
            this.cbxShowLogin.TabIndex = 25;
            this.cbxShowLogin.Text = "Show Login Panel";
            // 
            // cbxShowSignOffPrompt
            // 
            this.cbxShowSignOffPrompt.Location = new System.Drawing.Point(393, 306);
            this.cbxShowSignOffPrompt.Name = "cbxShowSignOffPrompt";
            this.cbxShowSignOffPrompt.Size = new System.Drawing.Size(156, 24);
            this.cbxShowSignOffPrompt.TabIndex = 26;
            this.cbxShowSignOffPrompt.Text = "Show Sign Off Prompt";
            // 
            // gbxDCFulfillmentMonitor
            // 
            this.gbxDCFulfillmentMonitor.Controls.Add(this.btnDCFulfillmentMonitorDirectory);
            this.gbxDCFulfillmentMonitor.Controls.Add(this.txtDCFulfillmentMonitorDirectory);
            this.gbxDCFulfillmentMonitor.Controls.Add(this.lblDCFulfillmentMonitorDirectory);
            this.gbxDCFulfillmentMonitor.Controls.Add(this.radDCFulfillmentMonitorOff);
            this.gbxDCFulfillmentMonitor.Controls.Add(this.radDCFulfillmentMonitorOn);
            this.gbxDCFulfillmentMonitor.Location = new System.Drawing.Point(17, 212);
            this.gbxDCFulfillmentMonitor.Name = "gbxDCFulfillmentMonitor";
            this.gbxDCFulfillmentMonitor.Size = new System.Drawing.Size(518, 76);
            this.gbxDCFulfillmentMonitor.TabIndex = 27;
            this.gbxDCFulfillmentMonitor.TabStop = false;
            this.gbxDCFulfillmentMonitor.Text = "DC Fulfillment Monitor";
            // 
            // btnDCFulfillmentMonitorDirectory
            // 
            this.btnDCFulfillmentMonitorDirectory.Location = new System.Drawing.Point(427, 48);
            this.btnDCFulfillmentMonitorDirectory.Name = "btnDCFulfillmentMonitorDirectory";
            this.btnDCFulfillmentMonitorDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnDCFulfillmentMonitorDirectory.TabIndex = 4;
            this.btnDCFulfillmentMonitorDirectory.Text = "Directory...";
            this.btnDCFulfillmentMonitorDirectory.UseVisualStyleBackColor = true;
            this.btnDCFulfillmentMonitorDirectory.Click += new System.EventHandler(this.btnDCFulfillmentMonitorDirectory_Click);
            // 
            // txtDCFulfillmentMonitorDirectory
            // 
            this.txtDCFulfillmentMonitorDirectory.Location = new System.Drawing.Point(88, 49);
            this.txtDCFulfillmentMonitorDirectory.Name = "txtDCFulfillmentMonitorDirectory";
            this.txtDCFulfillmentMonitorDirectory.Size = new System.Drawing.Size(326, 20);
            this.txtDCFulfillmentMonitorDirectory.TabIndex = 3;
            // 
            // lblDCFulfillmentMonitorDirectory
            // 
            this.lblDCFulfillmentMonitorDirectory.AutoSize = true;
            this.lblDCFulfillmentMonitorDirectory.Location = new System.Drawing.Point(30, 51);
            this.lblDCFulfillmentMonitorDirectory.Name = "lblDCFulfillmentMonitorDirectory";
            this.lblDCFulfillmentMonitorDirectory.Size = new System.Drawing.Size(52, 13);
            this.lblDCFulfillmentMonitorDirectory.TabIndex = 2;
            this.lblDCFulfillmentMonitorDirectory.Text = "Directory:";
            // 
            // radDCFulfillmentMonitorOff
            // 
            this.radDCFulfillmentMonitorOff.AutoSize = true;
            this.radDCFulfillmentMonitorOff.Location = new System.Drawing.Point(63, 22);
            this.radDCFulfillmentMonitorOff.Name = "radDCFulfillmentMonitorOff";
            this.radDCFulfillmentMonitorOff.Size = new System.Drawing.Size(39, 17);
            this.radDCFulfillmentMonitorOff.TabIndex = 1;
            this.radDCFulfillmentMonitorOff.TabStop = true;
            this.radDCFulfillmentMonitorOff.Text = "Off";
            this.radDCFulfillmentMonitorOff.UseVisualStyleBackColor = true;
            // 
            // radDCFulfillmentMonitorOn
            // 
            this.radDCFulfillmentMonitorOn.AutoSize = true;
            this.radDCFulfillmentMonitorOn.Location = new System.Drawing.Point(6, 22);
            this.radDCFulfillmentMonitorOn.Name = "radDCFulfillmentMonitorOn";
            this.radDCFulfillmentMonitorOn.Size = new System.Drawing.Size(39, 17);
            this.radDCFulfillmentMonitorOn.TabIndex = 0;
            this.radDCFulfillmentMonitorOn.TabStop = true;
            this.radDCFulfillmentMonitorOn.Text = "On";
            this.radDCFulfillmentMonitorOn.UseVisualStyleBackColor = true;
            // 
            // frmUserOptions
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(560, 431);
            this.Controls.Add(this.gbxDCFulfillmentMonitor);
            this.Controls.Add(this.cbxShowSignOffPrompt);
            this.Controls.Add(this.cbxShowLogin);
            this.Controls.Add(this.cbxAuditLoggingLevel);
            this.Controls.Add(this.lblAuditLoggingLevel);
            this.Controls.Add(this.gbxModifySalesMonitor);
            this.Controls.Add(this.gbxForecastMonitor);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmUserOptions";
            this.Text = "UserOptions";
            this.Load += new System.EventHandler(this.frmUserOptions_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.gbxForecastMonitor, 0);
            this.Controls.SetChildIndex(this.gbxModifySalesMonitor, 0);
            this.Controls.SetChildIndex(this.lblAuditLoggingLevel, 0);
            this.Controls.SetChildIndex(this.cbxAuditLoggingLevel, 0);
            this.Controls.SetChildIndex(this.cbxShowLogin, 0);
            this.Controls.SetChildIndex(this.cbxShowSignOffPrompt, 0);
            this.Controls.SetChildIndex(this.gbxDCFulfillmentMonitor, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbxForecastMonitor.ResumeLayout(false);
            this.gbxForecastMonitor.PerformLayout();
            this.gbxModifySalesMonitor.ResumeLayout(false);
            this.gbxModifySalesMonitor.PerformLayout();
            this.gbxDCFulfillmentMonitor.ResumeLayout(false);
            this.gbxDCFulfillmentMonitor.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmUserOptions_Load(object sender, System.EventArgs e)
		{
			FunctionSecurityProfile showLoginSecurity;
			FormLoaded = false;
			Format_Title(eDataState.Updatable, eMIDTextCode.frm_UserOptions, MIDText.GetTextOnly(eMIDTextCode.frm_UserOptions));
			SetReadOnly(true);
			SetText();
			if (SAB.ClientServerSession.UserOptions.ForecastMonitorIsActive)
			{
				this.radForecastMonitorOn.Checked = true;
			}
			else
			{
				this.radForecastMonitorOff.Checked = true;
			}
			this.txtForecastMonitorDirectory.Text = SAB.ClientServerSession.UserOptions.ForecastMonitorDirectory;
			if (SAB.ClientServerSession.UserOptions.ModifySalesMonitorIsActive)
			{
				this.radModifySalesMonitorOn.Checked = true;
			}
			else
			{
				this.radModifySalesMonitorOff.Checked = true;
			}
			this.txtModifySalesMonitorDirectory.Text = SAB.ClientServerSession.UserOptions.ModifySalesMonitorDirectory;
            // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
            if (SAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
            {
                this.radDCFulfillmentMonitorOn.Checked = true;
            }
            else
            {
                this.radDCFulfillmentMonitorOff.Checked = true;
            }
            this.txtDCFulfillmentMonitorDirectory.Text = SAB.ClientServerSession.UserOptions.DCFulfillmentMonitorDirectory;
            // END TT#1966-MD - AGallagher - DC Fulfillment

			showLoginSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsShowLogin);
			if (SAB.ClientServerSession.GlobalOptions.UseWindowsLogin &&
				showLoginSecurity.AllowView)
			{
				if (SAB.ClientServerSession.UserOptions.ShowLogin)
				{
					this.cbxShowLogin.Checked = true;
				}
				else
				{
					this.cbxShowLogin.Checked = false;
				}
			}
			else
			{
				this.cbxShowLogin.Checked = false;
				this.cbxShowLogin.Visible = false;
			}
			LoadLoggingLevels();
			cbxAuditLoggingLevel.SelectedValue = SAB.ClientServerSession.UserOptions.AuditLoggingLevel;
            // Begin TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
            this.cbxShowSignOffPrompt.Checked = SAB.ClientServerSession.UserOptions.ShowSignOffPrompt;
            // End TT#4243
			FormLoaded = true;
		}

		private void SetText()
		{
			try
			{
				this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				this.gbxForecastMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ForecastMonitor);
				this.radForecastMonitorOff.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Off);
				this.radForecastMonitorOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_On);
				this.gbxModifySalesMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ModifySalesMonitor);
				this.radModifySalesMonitorOff.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Off);
				this.radModifySalesMonitorOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_On);
				this.lblAuditLoggingLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AuditLoggingLevel);
				this.btnForecastMonitorDirectory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Directory);
				this.btnModifySalesMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Directory);
                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                this.gbxDCFulfillmentMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMonitor);
                this.radDCFulfillmentMonitorOff.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Off);
                this.radDCFulfillmentMonitorOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_On);
                this.btnDCFulfillmentMonitorDirectory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Directory);
                // END TT#1966-MD - AGallagher - DC Fulfillment
                this.cbxShowSignOffPrompt.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ShowSignOffPrompt); // TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void LoadLoggingLevels()
		{
			cbxAuditLoggingLevel.Items.Clear();
			DataTable dt = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);
			cbxAuditLoggingLevel.DataSource = dt;
			cbxAuditLoggingLevel.DisplayMember = "TEXT_VALUE";
			cbxAuditLoggingLevel.ValueMember = "TEXT_CODE";
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			SaveChanges();
			if (!ErrorFound)
			{
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		override protected bool SaveChanges()
		{
			ErrorFound = false;
			
			if (ValuesValid())
			{
				if (radForecastMonitorOn.Checked)
				{
					SAB.ClientServerSession.UserOptions.ForecastMonitorIsActive = true;
				}
				else
				{
					SAB.ClientServerSession.UserOptions.ForecastMonitorIsActive = false;
				}
				SAB.ClientServerSession.UserOptions.ForecastMonitorDirectory = txtForecastMonitorDirectory.Text;

				if (radModifySalesMonitorOn.Checked)
				{
					SAB.ClientServerSession.UserOptions.ModifySalesMonitorIsActive = true;
				}
				else
				{
					SAB.ClientServerSession.UserOptions.ModifySalesMonitorIsActive = false;
				}
				SAB.ClientServerSession.UserOptions.ModifySalesMonitorDirectory = txtModifySalesMonitorDirectory.Text;

                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                if (radDCFulfillmentMonitorOn.Checked)
                {
                    SAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive = true;
                }
                else
                {
                    SAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive = false;
                }
                SAB.ClientServerSession.UserOptions.DCFulfillmentMonitorDirectory = txtDCFulfillmentMonitorDirectory.Text;
                // END TT#1966-MD - AGallagher - DC Fulfillment

				if (cbxShowLogin.Checked)
				{
					SAB.ClientServerSession.UserOptions.ShowLogin = true;
				}
				else
				{
					SAB.ClientServerSession.UserOptions.ShowLogin = false;
				}

                // Begin TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect
                bool loggingLevelChanged = false;
                if (SAB.ClientServerSession.UserOptions.AuditLoggingLevel != (eMIDMessageLevel)Convert.ToInt32(cbxAuditLoggingLevel.SelectedValue))
                {
                    loggingLevelChanged = true;
                }
                // End TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect
				SAB.ClientServerSession.UserOptions.AuditLoggingLevel = (eMIDMessageLevel)Convert.ToInt32(cbxAuditLoggingLevel.SelectedValue);
                // Begin TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect
                if (loggingLevelChanged)
                {
                    eMIDMessageLevel auditLevel = SAB.ClientServerSession.UserOptions.AuditLoggingLevel;
                    SAB.ClientServerSession.SetAuditLoggingLevel(auditLevel);
                    if (SAB.ApplicationServerSession != null)
                    {
                        SAB.ApplicationServerSession.SetAuditLoggingLevel(auditLevel);
                    }
                    if (SAB.ControlServerSession != null)
                    {
                        SAB.ControlServerSession.SetAuditLoggingLevel(auditLevel);
                    }
                    if (SAB.HeaderServerSession != null)
                    {
                        SAB.HeaderServerSession.SetAuditLoggingLevel(auditLevel);
                    }
                    if (SAB.HierarchyServerSession != null)
                    {
                        SAB.HierarchyServerSession.SetAuditLoggingLevel(auditLevel);
                    }
                    if (SAB.SchedulerServerSession != null)
                    {
                        SAB.SchedulerServerSession.SetAuditLoggingLevel(auditLevel);
                    }
                    if (SAB.StoreServerSession != null)
                    {
                        SAB.StoreServerSession.SetAuditLoggingLevel(auditLevel);
                    }
                }
                // End TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect

                // Begin TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
                SAB.ClientServerSession.UserOptions.ShowSignOffPrompt = cbxShowSignOffPrompt.Checked;
                // End TT#4243
				SAB.ClientServerSession.UpdateUserOptions();
				ChangePending = false;
			}
			else
			{
				ErrorFound = true;
				MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect));
			}

			return true;
		}

		private bool ValuesValid()
		{
			bool valuesValid = true;
			
			if (radForecastMonitorOn.Checked)
			{
				if (txtForecastMonitorDirectory.Text == null ||
					txtForecastMonitorDirectory.Text.Length == 0)
				{
					valuesValid = false;
					ErrorProvider.SetError (txtForecastMonitorDirectory, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
			}

			if (radModifySalesMonitorOn.Checked)
			{
				if (txtModifySalesMonitorDirectory.Text == null ||
					txtModifySalesMonitorDirectory.Text.Length == 0)
				{
					valuesValid = false;
					ErrorProvider.SetError (txtModifySalesMonitorDirectory, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
			}

            // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
            if (radDCFulfillmentMonitorOn.Checked)
            {
                if (txtDCFulfillmentMonitorDirectory.Text == null ||
                    txtDCFulfillmentMonitorDirectory.Text.Length == 0)
                {
                    valuesValid = false;
                    ErrorProvider.SetError(txtDCFulfillmentMonitorDirectory, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
            }
            // END TT#1966-MD - AGallagher - DC Fulfillment

			return valuesValid;
		}

		private void btnForecastMonitorDirectory_Click(object sender, System.EventArgs e)
		{
			try
			{
				fbdDirectory.SelectedPath = txtForecastMonitorDirectory.Text.Trim();
				fbdDirectory.Description = "Select the directory where the forecast monitor log is to be saved.";

				if (fbdDirectory.ShowDialog() == DialogResult.OK)
				{
					txtForecastMonitorDirectory.Text = fbdDirectory.SelectedPath;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void btnModifySalesMonitor_Click(object sender, System.EventArgs e)
		{
			try
			{
				fbdDirectory.SelectedPath = txtModifySalesMonitorDirectory.Text.Trim();
				fbdDirectory.Description = "Select the directory where the modify sales monitor log is to be saved.";

				if (fbdDirectory.ShowDialog() == DialogResult.OK)
				{
					txtModifySalesMonitorDirectory.Text = fbdDirectory.SelectedPath;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        private void btnDCFulfillmentMonitorDirectory_Click(object sender, System.EventArgs e)
        {
            try
            {
                fbdDirectory.SelectedPath = txtDCFulfillmentMonitorDirectory.Text.Trim();
                fbdDirectory.Description = "Select the directory where the DC Fulfillment monitor log is to be saved.";

                if (fbdDirectory.ShowDialog() == DialogResult.OK)
                {
                    txtDCFulfillmentMonitorDirectory.Text = fbdDirectory.SelectedPath;

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment
		private void radMonitor_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void cbxAuditLoggingLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

        private void cbxAuditLoggingLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxAuditLoggingLevel_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
