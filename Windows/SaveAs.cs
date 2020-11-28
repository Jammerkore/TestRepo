using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SaveAs.
	/// </summary>
	public class frmSaveAs : MIDFormBase
	{
		//		// add event to update explorer when hierarchy is changed
		//		public delegate void SaveAsEventHandler(object source, SaveAsEventArgs e);
		//		public event SaveAsEventHandler OnSaveAsHandler;

        private bool _enableGlobal;
        private bool _enableUser;
        private bool _enableCustom;
        private bool _showUserGlobal;
        private bool _showCustom;
        private bool _saveCanceled ;
        private string _saveAsName;
        private bool _enableGlobalView;         // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private bool _enableUserView;
        private bool _enableGlobalDetail;
        private bool _enableUserDetail;
        private bool _showViewOption;           
        private bool _showDetailViewOption;
        private bool _saveView;
        private bool _saveDetailView;
        private bool _saveMethod;
        private bool _allowMethodSaveAs;
        private string _saveAsMethod;            
        private string _saveAsViewName;
        private string _saveAsDetailViewName;    // End TT#231  
        private int _saveAsNameMaxLength;        // Begin TT#445 - RMatelic - allocation workspace error after save as selected on a Save As header
        private int _saveAsDefaultLength = 50;   // End TT#445 
        
        		
		public bool SaveCanceled 
		{
			get { return _saveCanceled ; }
			set { _saveCanceled = value; }
		}
		public string SaveAsName 
		{
			get { return _saveAsName ; }
			set { _saveAsName = value; }
		}
        public bool EnableGlobal
        {
            get { return _enableGlobal; }
            set { _enableGlobal = value; }
        }
        public bool EnableUser
        {
            get { return _enableUser; }
            set { _enableUser = value; }
        }
        public bool EnableCustom
        {
            get { return _enableCustom; }
            set { _enableCustom = value; }
        }
        public bool ShowUserGlobal
		{
			get { return _showUserGlobal ; }
			set { _showUserGlobal = value; }
		}
        public bool ShowCustom
        {
            get { return _showCustom; }
            set { _showCustom = value; }
        }
        public bool isGlobalChecked
		{
			get { return rdoGlobal.Checked; }
			set { rdoGlobal.Checked = value; }
		}
        public bool isUserChecked
        {
            get { return rdoUser.Checked; }
            set { rdoUser.Checked = value; }
        }
        public bool isCustomChecked
        {
            get { return rdoCustom.Checked; }
            set { rdoCustom.Checked = value; }
        }
        public bool isGlobalEnabled
		{
			get { return rdoGlobal.Enabled; }
			set { rdoGlobal.Enabled = value; }
		}
		public bool isUserEnabled
		{
			get { return rdoUser.Enabled; }
			set { rdoUser.Enabled = value; }
		}
        public bool isCustomEnabled
        {
            get { return rdoCustom.Enabled; }
            set { rdoCustom.Enabled = value; }
        }

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        public bool EnableGlobalView
        {
            get { return _enableGlobalView; }
            set { _enableGlobalView = value; }
        }
        public bool EnableUserView
        {
            get { return _enableUserView; }
            set { _enableUserView = value; }
        }
        public bool EnableGlobalDetail
        {
            get { return _enableGlobalDetail; }
            set { _enableGlobalDetail = value; }
        }
        public bool EnableUserDetail
        {
            get { return _enableUserDetail; }
            set { _enableUserDetail = value; }
        }
        public bool ShowViewOption
        {
            get { return _showViewOption; }
            set { _showViewOption = value; }
        }
        public bool ShowDetailViewOption
        {
            get { return _showDetailViewOption; }
            set { _showDetailViewOption = value; }
        }
        public bool SaveMethod
        {
            get { return _saveMethod; }
            set { _saveMethod = value; }
        }
        public bool SaveView
        {
            get { return _saveView; }
            set { _saveView = value; }
        }
        public bool SaveDetailView
        {
            get { return _saveDetailView; }
            set { _saveDetailView = value; }
        }
        public bool AllowMethodSaveAs
        {
            get { return _allowMethodSaveAs; }
            set { _allowMethodSaveAs = value; }
        }
        public bool isGlobalViewChecked
        {
            get { return rdoGlobalView.Checked; }
            set { rdoGlobalView.Checked = value; }
        }
        public bool isUserViewChecked
        {
            get { return rdoUserView.Checked; }
            set { rdoUserView.Checked = value; }
        }
        public bool isGlobalDetailViewChecked
        {
            get { return rdoGlobalDetailView.Checked; }
            set { rdoGlobalDetailView.Checked = value; }
        }
        public bool isUserDetailViewChecked
        {
            get { return rdoUserDetailView.Checked; }
            set { rdoUserDetailView.Checked = value; }
        }
        public string SaveAsMethod
        {
            get { return _saveAsMethod; }
            set { _saveAsMethod = value; }
        }
        public string SaveAsViewName
        {
            get { return _saveAsViewName; }
            set { _saveAsViewName = value; }
        }
        public string SaveAsDetailViewName
        {
            get { return _saveAsDetailViewName; }
            set { _saveAsDetailViewName = value; }
        }
        // End TT#231  

        // Begin TT#445 - RMatelic - allocation workspace error after save as selected on a Save As header
        public int SaveAsNameMaxLength
        {
            get { return _saveAsNameMaxLength; }
            set { _saveAsNameMaxLength = value; }
        }
        // End TT#445  

		private SessionAddressBlock _SAB;
		private System.Windows.Forms.Label lblSaveAsName;
		private System.Windows.Forms.TextBox txtSaveAsName;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.RadioButton rdoUser;
		private System.Windows.Forms.RadioButton rdoGlobal;
        private RadioButton rdoCustom;
        private GroupBox gbxSaveAsMethod;
        private CheckBox cbxSaveAsMethod;
        private GroupBox gbxSaveAsView;
        private Label lblSaveAsViewName;
        private TextBox txtSaveAsViewName;
        private RadioButton rdoUserView;
        private RadioButton rdoGlobalView;
        private CheckBox cbxSaveAsView;
        private GroupBox gbxSaveAsDetailView;
        private Label label1;
        private TextBox txtSaveAsDetailViewName;
        private RadioButton rdoUserDetailView;
        private RadioButton rdoGlobalDetailView;
        private CheckBox cbxSaveAsDetailView;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSaveAs(SessionAddressBlock aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_SAB = aSAB; 
			_saveAsName = "";
            _showUserGlobal = false;
            _showCustom = false;
            _enableGlobal = true;
            _enableUser = true;
            _enableCustom = true;
            _saveCanceled = true;// TT#393 -MD - RBeck
            this.Icon = new System.Drawing.Icon(MIDGraphics.ImageDir + "\\" + MIDGraphics.SaveIcon);

            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            cbxSaveAsMethod.Checked = true;     
            gbxSaveAsView.Enabled= false;
            gbxSaveAsDetailView.Enabled = false; 
            _saveMethod = true;
            _saveView = false;
            _saveDetailView = false;
            _allowMethodSaveAs = true;
            // End TT#231
            _saveAsNameMaxLength = _saveAsDefaultLength;     // TT#445 - RMatelic - allocation workspace error after save as selected on a Save As header
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

				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.Load -= new System.EventHandler(this.frmSaveAs_Load);
                this.Shown -= new System.EventHandler(this.frmSaveAs_Shown);
                this.rdoGlobal.CheckedChanged -= new System.EventHandler(this.rdoGlobal_CheckedChanged);
                this.rdoUser.CheckedChanged -= new System.EventHandler(this.rdoUser_CheckedChanged);
                this.rdoCustom.CheckedChanged -= new System.EventHandler(this.rdoCustom_CheckedChanged);
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
			this.lblSaveAsName = new System.Windows.Forms.Label();
			this.txtSaveAsName = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.rdoCustom = new System.Windows.Forms.RadioButton();
			this.rdoUser = new System.Windows.Forms.RadioButton();
			this.rdoGlobal = new System.Windows.Forms.RadioButton();
			this.gbxSaveAsMethod = new System.Windows.Forms.GroupBox();
			this.cbxSaveAsMethod = new System.Windows.Forms.CheckBox();
			this.gbxSaveAsView = new System.Windows.Forms.GroupBox();
			this.lblSaveAsViewName = new System.Windows.Forms.Label();
			this.txtSaveAsViewName = new System.Windows.Forms.TextBox();
			this.rdoUserView = new System.Windows.Forms.RadioButton();
			this.rdoGlobalView = new System.Windows.Forms.RadioButton();
			this.cbxSaveAsView = new System.Windows.Forms.CheckBox();
			this.gbxSaveAsDetailView = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSaveAsDetailViewName = new System.Windows.Forms.TextBox();
			this.rdoUserDetailView = new System.Windows.Forms.RadioButton();
			this.rdoGlobalDetailView = new System.Windows.Forms.RadioButton();
			this.cbxSaveAsDetailView = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.gbxSaveAsMethod.SuspendLayout();
			this.gbxSaveAsView.SuspendLayout();
			this.gbxSaveAsDetailView.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// lblSaveAsName
			// 
			this.lblSaveAsName.Location = new System.Drawing.Point(6, 19);
			this.lblSaveAsName.Name = "lblSaveAsName";
			this.lblSaveAsName.Size = new System.Drawing.Size(51, 23);
			this.lblSaveAsName.TabIndex = 0;
			this.lblSaveAsName.Text = "Name:";
			this.lblSaveAsName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtSaveAsName
			// 
			this.txtSaveAsName.Location = new System.Drawing.Point(76, 22);
			this.txtSaveAsName.Name = "txtSaveAsName";
			this.txtSaveAsName.Size = new System.Drawing.Size(240, 20);
			this.txtSaveAsName.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(300, 338);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(197, 338);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 28;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// rdoCustom
			// 
			this.rdoCustom.Location = new System.Drawing.Point(243, 62);
			this.rdoCustom.Name = "rdoCustom";
			this.rdoCustom.Size = new System.Drawing.Size(61, 19);
			this.rdoCustom.TabIndex = 16;
			this.rdoCustom.Text = "Custom";
			this.rdoCustom.Visible = false;
			this.rdoCustom.CheckedChanged += new System.EventHandler(this.rdoCustom_CheckedChanged);
			// 
			// rdoUser
			// 
			this.rdoUser.Location = new System.Drawing.Point(163, 62);
			this.rdoUser.Name = "rdoUser";
			this.rdoUser.Size = new System.Drawing.Size(48, 19);
			this.rdoUser.TabIndex = 15;
			this.rdoUser.Text = "User";
			this.rdoUser.Visible = false;
			this.rdoUser.CheckedChanged += new System.EventHandler(this.rdoUser_CheckedChanged);
			// 
			// rdoGlobal
			// 
			this.rdoGlobal.Location = new System.Drawing.Point(76, 62);
			this.rdoGlobal.Name = "rdoGlobal";
			this.rdoGlobal.Size = new System.Drawing.Size(56, 19);
			this.rdoGlobal.TabIndex = 14;
			this.rdoGlobal.Text = "Global";
			this.rdoGlobal.Visible = false;
			this.rdoGlobal.CheckedChanged += new System.EventHandler(this.rdoGlobal_CheckedChanged);
			// 
			// gbxSaveAsMethod
			// 
			this.gbxSaveAsMethod.Controls.Add(this.lblSaveAsName);
			this.gbxSaveAsMethod.Controls.Add(this.rdoCustom);
			this.gbxSaveAsMethod.Controls.Add(this.txtSaveAsName);
			this.gbxSaveAsMethod.Controls.Add(this.rdoUser);
			this.gbxSaveAsMethod.Controls.Add(this.rdoGlobal);
			this.gbxSaveAsMethod.Location = new System.Drawing.Point(34, 12);
			this.gbxSaveAsMethod.Name = "gbxSaveAsMethod";
			this.gbxSaveAsMethod.Size = new System.Drawing.Size(341, 93);
			this.gbxSaveAsMethod.TabIndex = 29;
			this.gbxSaveAsMethod.TabStop = false;
			// 
			// cbxSaveAsMethod
			// 
			this.cbxSaveAsMethod.AutoSize = true;
			this.cbxSaveAsMethod.Location = new System.Drawing.Point(12, 20);
			this.cbxSaveAsMethod.Name = "cbxSaveAsMethod";
			this.cbxSaveAsMethod.Size = new System.Drawing.Size(15, 14);
			this.cbxSaveAsMethod.TabIndex = 30;
			this.cbxSaveAsMethod.UseVisualStyleBackColor = true;
			this.cbxSaveAsMethod.CheckedChanged += new System.EventHandler(this.cbxSaveAsMethod_CheckedChanged);
			// 
			// gbxSaveAsView
			// 
			this.gbxSaveAsView.Controls.Add(this.lblSaveAsViewName);
			this.gbxSaveAsView.Controls.Add(this.txtSaveAsViewName);
			this.gbxSaveAsView.Controls.Add(this.rdoUserView);
			this.gbxSaveAsView.Controls.Add(this.rdoGlobalView);
			this.gbxSaveAsView.Location = new System.Drawing.Point(34, 121);
			this.gbxSaveAsView.Name = "gbxSaveAsView";
			this.gbxSaveAsView.Size = new System.Drawing.Size(341, 93);
			this.gbxSaveAsView.TabIndex = 31;
			this.gbxSaveAsView.TabStop = false;
			// 
			// lblSaveAsViewName
			// 
			this.lblSaveAsViewName.Location = new System.Drawing.Point(6, 19);
			this.lblSaveAsViewName.Name = "lblSaveAsViewName";
			this.lblSaveAsViewName.Size = new System.Drawing.Size(51, 23);
			this.lblSaveAsViewName.TabIndex = 0;
			this.lblSaveAsViewName.Text = "Name:";
			this.lblSaveAsViewName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtSaveAsViewName
			// 
			this.txtSaveAsViewName.Location = new System.Drawing.Point(76, 22);
			this.txtSaveAsViewName.Name = "txtSaveAsViewName";
			this.txtSaveAsViewName.Size = new System.Drawing.Size(240, 20);
			this.txtSaveAsViewName.TabIndex = 1;
			// 
			// rdoUserView
			// 
			this.rdoUserView.Location = new System.Drawing.Point(163, 62);
			this.rdoUserView.Name = "rdoUserView";
			this.rdoUserView.Size = new System.Drawing.Size(48, 19);
			this.rdoUserView.TabIndex = 15;
			this.rdoUserView.Text = "User";
			this.rdoUserView.Visible = false;
			this.rdoUserView.CheckedChanged += new System.EventHandler(this.rdoView_CheckedChanged);
			// 
			// rdoGlobalView
			// 
			this.rdoGlobalView.Location = new System.Drawing.Point(76, 62);
			this.rdoGlobalView.Name = "rdoGlobalView";
			this.rdoGlobalView.Size = new System.Drawing.Size(56, 19);
			this.rdoGlobalView.TabIndex = 14;
			this.rdoGlobalView.Text = "Global";
			this.rdoGlobalView.Visible = false;
			this.rdoGlobalView.CheckedChanged += new System.EventHandler(this.rdoView_CheckedChanged);
			// 
			// cbxSaveAsView
			// 
			this.cbxSaveAsView.AutoSize = true;
			this.cbxSaveAsView.Location = new System.Drawing.Point(12, 131);
			this.cbxSaveAsView.Name = "cbxSaveAsView";
			this.cbxSaveAsView.Size = new System.Drawing.Size(15, 14);
			this.cbxSaveAsView.TabIndex = 32;
			this.cbxSaveAsView.UseVisualStyleBackColor = true;
			this.cbxSaveAsView.CheckedChanged += new System.EventHandler(this.cbxSaveAsView_CheckedChanged);
			// 
			// gbxSaveAsDetailView
			// 
			this.gbxSaveAsDetailView.Controls.Add(this.label1);
			this.gbxSaveAsDetailView.Controls.Add(this.txtSaveAsDetailViewName);
			this.gbxSaveAsDetailView.Controls.Add(this.rdoUserDetailView);
			this.gbxSaveAsDetailView.Controls.Add(this.rdoGlobalDetailView);
			this.gbxSaveAsDetailView.Location = new System.Drawing.Point(34, 230);
			this.gbxSaveAsDetailView.Name = "gbxSaveAsDetailView";
			this.gbxSaveAsDetailView.Size = new System.Drawing.Size(341, 93);
			this.gbxSaveAsDetailView.TabIndex = 33;
			this.gbxSaveAsDetailView.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtSaveAsDetailViewName
			// 
			this.txtSaveAsDetailViewName.Location = new System.Drawing.Point(76, 22);
			this.txtSaveAsDetailViewName.Name = "txtSaveAsDetailViewName";
			this.txtSaveAsDetailViewName.Size = new System.Drawing.Size(240, 20);
			this.txtSaveAsDetailViewName.TabIndex = 1;
			// 
			// rdoUserDetailView
			// 
			this.rdoUserDetailView.Location = new System.Drawing.Point(163, 62);
			this.rdoUserDetailView.Name = "rdoUserDetailView";
			this.rdoUserDetailView.Size = new System.Drawing.Size(48, 19);
			this.rdoUserDetailView.TabIndex = 15;
			this.rdoUserDetailView.Text = "User";
			this.rdoUserDetailView.Visible = false;
			this.rdoUserDetailView.CheckedChanged += new System.EventHandler(this.rdoDetailView_CheckedChanged);
			// 
			// rdoGlobalDetailView
			// 
			this.rdoGlobalDetailView.Location = new System.Drawing.Point(76, 62);
			this.rdoGlobalDetailView.Name = "rdoGlobalDetailView";
			this.rdoGlobalDetailView.Size = new System.Drawing.Size(56, 19);
			this.rdoGlobalDetailView.TabIndex = 14;
			this.rdoGlobalDetailView.Text = "Global";
			this.rdoGlobalDetailView.Visible = false;
			// 
			// cbxSaveAsDetailView
			// 
			this.cbxSaveAsDetailView.AutoSize = true;
			this.cbxSaveAsDetailView.Location = new System.Drawing.Point(12, 239);
			this.cbxSaveAsDetailView.Name = "cbxSaveAsDetailView";
			this.cbxSaveAsDetailView.Size = new System.Drawing.Size(15, 14);
			this.cbxSaveAsDetailView.TabIndex = 34;
			this.cbxSaveAsDetailView.UseVisualStyleBackColor = true;
			this.cbxSaveAsDetailView.CheckedChanged += new System.EventHandler(this.cbxSaveAsDetailView_CheckedChanged);
			// 
			// frmSaveAs
			// 
			this.AcceptButton = this.btnSave;
			this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 376);
			this.Controls.Add(this.cbxSaveAsDetailView);
			this.Controls.Add(this.gbxSaveAsDetailView);
			this.Controls.Add(this.cbxSaveAsView);
			this.Controls.Add(this.gbxSaveAsView);
			this.Controls.Add(this.cbxSaveAsMethod);
			this.Controls.Add(this.gbxSaveAsMethod);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSaveAs";
			this.Text = "Save As";
			this.Load += new System.EventHandler(this.frmSaveAs_Load);
			this.Shown += new System.EventHandler(this.frmSaveAs_Shown);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.gbxSaveAsMethod, 0);
			this.Controls.SetChildIndex(this.cbxSaveAsMethod, 0);
			this.Controls.SetChildIndex(this.gbxSaveAsView, 0);
			this.Controls.SetChildIndex(this.cbxSaveAsView, 0);
			this.Controls.SetChildIndex(this.gbxSaveAsDetailView, 0);
			this.Controls.SetChildIndex(this.cbxSaveAsDetailView, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.gbxSaveAsMethod.ResumeLayout(false);
			this.gbxSaveAsMethod.PerformLayout();
			this.gbxSaveAsView.ResumeLayout(false);
			this.gbxSaveAsView.PerformLayout();
			this.gbxSaveAsDetailView.ResumeLayout(false);
			this.gbxSaveAsDetailView.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void frmSaveAs_Load(object sender, System.EventArgs e)
		{
			// Begin Track #5909 stodd
			if (_saveAsName != string.Empty)
				txtSaveAsName.Text = _saveAsName;
			// End Track #5909
			if (_showUserGlobal)
            {
                rdoGlobal.Visible = true;
                rdoUser.Visible = true;
            }
            if (_showCustom)
            {
                rdoGlobal.Visible = true;
                rdoUser.Visible = true;
                rdoCustom.Visible = true;
            }
            if (!_showUserGlobal && !_showCustom)
			{
                this.Height -= rdoGlobal.Height;
			}
            else if (!rdoGlobal.Checked && !rdoUser.Checked && !rdoCustom.Checked)
            {
                rdoGlobal.Checked = true;
            }

            if (!_enableGlobal) 
            {
                rdoGlobal.Enabled = false;
            }
            if (!_enableUser)
            {
                rdoUser.Enabled = false;
            }
            if (!_enableCustom)
            {
                rdoCustom.Enabled = false;
            }
            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            if (_saveAsMethod != null && _saveAsMethod.ToString().Trim() != string.Empty) // TT#323 - RMatelic - Models when doing a Save As the word Method appears above the Name and should not be appearing
            {
                gbxSaveAsMethod.Text = _saveAsMethod + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
            }
            
            txtSaveAsViewName.Text = SaveAsViewName;
            cbxSaveAsMethod.Checked = _saveMethod;
            CheckViewOptions();
            // End TT#231  

            // Begin TT#445 - RMatelic - allocation workspace error after save as selected on a Save As header
            txtSaveAsName.MaxLength = SaveAsNameMaxLength;
            // End TT#445  

			// BEGIN TT#62 - MD - stodd - ESC processing
			SetCatchKeyDown();
			// END TT#62 - MD - stodd 
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void CheckViewOptions()
        {
            if (_showViewOption)
            {
                cbxSaveAsView.Visible = true;
                //cbxSaveAsDetailView.Visible = true;
                //gbxSaveAsView.Visible = true;
                gbxSaveAsDetailView.Visible = true;
                gbxSaveAsView.Text = gbxSaveAsMethod.Text + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_View);
                gbxSaveAsDetailView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityStoreDetail) + " "
                                         + MIDText.GetTextOnly(eMIDTextCode.lbl_View);

                if (_showDetailViewOption)
                {
                    cbxSaveAsDetailView.Visible = true;
                    gbxSaveAsDetailView.Visible = true;
                    gbxSaveAsDetailView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityStoreDetail) + " "
                                       + MIDText.GetTextOnly(eMIDTextCode.lbl_View);
                    txtSaveAsDetailViewName.Text = SaveAsDetailViewName;
                    cbxSaveAsDetailView.Checked = _saveDetailView;
                    cbxSaveAsView.Checked = _saveView;
                    if (!_allowMethodSaveAs)
                    {
                        cbxSaveAsMethod.Visible = false;
                    }
                    txtSaveAsDetailViewName.MaxLength = _saveAsDefaultLength; // TT#445 - need to change at some point
                    this.Height = 410;
                }
                else
                {
                    cbxSaveAsDetailView.Visible = false;
                    gbxSaveAsDetailView.Visible = false;
                    cbxSaveAsView.Checked = _saveView;
                    this.Height = 310;
                }
                if (_showUserGlobal)
                {
                    SetUpMethodViewSave();
                    if (_showDetailViewOption)
                    {
                        SetUpDetailViewSave();
                    }
                }
            }
            else
            {
                cbxSaveAsMethod.Visible = false;
                cbxSaveAsView.Visible = false;
                cbxSaveAsDetailView.Visible = false;
                gbxSaveAsView.Visible = false;
                gbxSaveAsDetailView.Visible = false;
                this.Height = 192;
            }
        }

        private void SetUpMethodViewSave() 
        {
            rdoGlobalView.Visible = true;
            rdoUserView.Visible = true;

            if (!_enableGlobalView && !_enableUserView)
            {
                cbxSaveAsView.Visible = false;
                gbxSaveAsView.Enabled = false;
                return;
            }
            txtSaveAsViewName.MaxLength = _saveAsDefaultLength; // TT#445 - need to change at some point
            if (!rdoGlobalView.Checked && !rdoUserView.Checked)
            {
                if (_enableGlobalView)
                {
                    rdoGlobalView.Checked = true;
                }
                else
                {
                    rdoUserView.Checked = true;
                }    
            }
            else if (rdoGlobalView.Checked)
            {
                if (!_enableGlobalView)
                {
                    rdoGlobalView.Enabled = false;
                    if (_enableUserView)
                    {
                        txtSaveAsViewName.Text = null;
                        rdoUserView.Checked = true;
                    }
                }
                else if (!_enableUserView)
                {
                    rdoUserView.Enabled = false;
                }
            }
            else
            {
                if (!_enableUserView)
                {
                    rdoUserView.Enabled = false;
                    if (_enableGlobalView)
                    {
                        txtSaveAsViewName.Text = null;
                        rdoGlobalView.Checked = true;
                    }
                }
                else if (!_enableGlobalView)
                {
                    rdoGlobalView.Enabled = false;
                }
            }
        }

        private void SetUpDetailViewSave()
        {
            rdoGlobalDetailView.Visible = true;
            rdoUserDetailView.Visible = true;

            if (!_enableGlobalDetail && !_enableUserDetail)
            {
                cbxSaveAsDetailView.Visible = false;
                gbxSaveAsDetailView.Enabled = false;
                return;
            }

            if (!rdoGlobalDetailView.Checked && !rdoUserDetailView.Checked)
            {
                if (_enableGlobalDetail)
                {
                    rdoGlobalDetailView.Checked = true;
                }
                else
                {
                    rdoUserDetailView.Checked = true;
                }
            }
            else if (rdoGlobalDetailView.Checked)
            {
                if (!_enableGlobalDetail)
                {
                    rdoGlobalDetailView.Enabled = false;
                    if (_enableUserDetail)
                    {
                        txtSaveAsDetailViewName.Text = null;
                        rdoUserDetailView.Checked = true;
                    }
                }
                else if (!_enableUserDetail)
                {
                    rdoUserDetailView.Enabled = false;
                }
            }
            else
            {
                if (!_enableUserDetail)
                {
                    rdoUserDetailView.Enabled = false;
                    if (_enableGlobalDetail)
                    {
                        txtSaveAsDetailViewName.Text = null;
                        rdoGlobalDetailView.Checked = true;
                    }
                }
                else if (!_enableGlobalDetail)
                {
                    rdoGlobalDetailView.Enabled = false;
                }
            }
        }

        // End TT#231  

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			SaveCanceled = true;
//			SaveAsEventArgs ea = new SaveAsEventArgs();
//			if (OnSaveAsHandler != null)
//			{
//				ea.SaveCanceled = true;
//				OnSaveAsHandler(this, ea);
//			}
			this.Close();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            if (ValidInput())
            // End TT#231
			{
				SaveCanceled = false;
				SaveAsName = txtSaveAsName.Text;
//				SaveAsEventArgs ea = new SaveAsEventArgs();
//				if (OnSaveAsHandler != null)
//				{
//					ea.SaveCanceled = false;
//					ea.SaveAsName = txtSaveAsName.Text;
//					OnSaveAsHandler(this, ea);
//				}
                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                SaveAsViewName = txtSaveAsViewName.Text;
                SaveAsDetailViewName = txtSaveAsDetailViewName.Text;
                // End TT#231
				this.Close();
			}
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private bool ValidInput()
        {
            bool validInput = true;
            ErrorProvider.SetError(txtSaveAsName, string.Empty);
            ErrorProvider.SetError(txtSaveAsViewName, string.Empty);
            ErrorProvider.SetError(txtSaveAsDetailViewName, string.Empty);
            string errMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameRequiredToSave); 

            if (cbxSaveAsMethod.Checked)
            {
                if (txtSaveAsName.Text == null || txtSaveAsName.Text.Trim().Length == 0)
                {
                    validInput = false;
                    ErrorProvider.SetError(txtSaveAsName, errMessage);
                }
            }

            if (cbxSaveAsView.Checked)
            {
                if (txtSaveAsViewName.Text == null || txtSaveAsViewName.Text.Trim().Length == 0)
                {
                    validInput = false;
                    ErrorProvider.SetError(txtSaveAsViewName, errMessage);
                }
            }

            if (cbxSaveAsDetailView.Checked)
            {
                if (txtSaveAsDetailViewName.Text == null || txtSaveAsDetailViewName.Text.Trim().Length == 0)
                {
                    validInput = false;
                    ErrorProvider.SetError(txtSaveAsDetailViewName, errMessage);
                }
            }

            if (!validInput)
            {
                MessageBox.Show(errMessage, "Save As",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return validInput;
        }
        // End TT#231  

        private void frmSaveAs_Shown(object sender, EventArgs e)
        {
            txtSaveAsName.SelectAll();
            txtSaveAsName.Focus();
        }

        private void rdoGlobal_CheckedChanged(object sender, EventArgs e)
        {
            if (txtSaveAsName.Text == "Custom")
            {
                txtSaveAsName.Text = "";
            }
            txtSaveAsName.Enabled = true;
            txtSaveAsName.Focus();
        }

        private void rdoUser_CheckedChanged(object sender, EventArgs e)
        {
            if (txtSaveAsName.Text == "Custom")
            {
                txtSaveAsName.Text = "";
            }
            txtSaveAsName.Enabled = true;
            txtSaveAsName.Focus();
        }

        private void rdoCustom_CheckedChanged(object sender, EventArgs e)
        {
            txtSaveAsName.Text = "Custom";
            txtSaveAsName.Enabled = false;
        }

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void cbxSaveAsMethod_CheckedChanged(object sender, EventArgs e)
        {
            gbxSaveAsMethod.Enabled = cbxSaveAsMethod.Checked;
            _saveMethod = cbxSaveAsMethod.Checked;
            if (cbxSaveAsMethod.Checked)
            {
                txtSaveAsName.Focus();
            }
        }

        private void cbxSaveAsView_CheckedChanged(object sender, EventArgs e)
        {
            gbxSaveAsView.Enabled = cbxSaveAsView.Checked;
            if (cbxSaveAsView.Checked)
            {
                txtSaveAsViewName.Focus();
                _saveView = true;
            }
        }

        private void cbxSaveAsDetailView_CheckedChanged(object sender, EventArgs e)
        {
            gbxSaveAsDetailView.Enabled = cbxSaveAsDetailView.Checked;
            if (cbxSaveAsDetailView.Checked)
            {
                txtSaveAsDetailViewName.Focus();
                _saveDetailView = true;
            }
        }

        private void rdoView_CheckedChanged(object sender, EventArgs e)
        {
            txtSaveAsViewName.Focus();
        }

        private void rdoDetailView_CheckedChanged(object sender, EventArgs e)
        {
            txtSaveAsDetailViewName.Focus();
        }
        // End TT#231  
	}
}
