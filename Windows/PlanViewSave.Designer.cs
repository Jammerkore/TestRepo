namespace MIDRetail.Windows
{
	partial class PlanViewSave
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			if (disposing)
			{
				this.drsStoreHighLevelTime.Click -= new System.EventHandler(this.drsStoreHighLevelTime_Click);
				this.drsStoreHighLevelTime.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsStoreHighLevelTime_OnSelection);
				this.txtStoreHighLevelMerch.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerch_DragDrop);
				this.txtStoreHighLevelMerch.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerch_DragEnter);
				this.txtStoreHighLevelMerch.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtMerch_DragOver);
				this.drsChainHighLevelTime.Click -= new System.EventHandler(this.drsChainHighLevelTime_Click);
				this.drsChainHighLevelTime.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsChainHighLevelTime_OnSelection);
				this.txtChainHighLevelMerch.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerch_DragDrop);
				this.txtChainHighLevelMerch.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerch_DragEnter);
				this.txtChainHighLevelMerch.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtMerch_DragOver);
				this.drsStoreLowLevelTime.Click -= new System.EventHandler(this.drsStoreLowLevelTime_Click);
				this.drsStoreLowLevelTime.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsStoreLowLevelTime_OnSelection);
				this.drsChainLowLevelTime.Click -= new System.EventHandler(this.drsChainLowLevelTime_Click);
				this.drsChainLowLevelTime.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsChainLowLevelTime_OnSelection);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.chkStoreHighLevel.CheckedChanged -= new System.EventHandler(this.chkStoreHighLevel_CheckedChanged);
				this.chkChainHighLevel.CheckedChanged -= new System.EventHandler(this.chkChainHighLevel_CheckedChanged);
				this.chkStoreLowLevel.CheckedChanged -= new System.EventHandler(this.chkStoreLowLevel_CheckedChanged);
				this.chkChainLowLevel.CheckedChanged -= new System.EventHandler(this.chkChainLowLevel_CheckedChanged);
				this.chkStoreLowLevelOverride.CheckedChanged -= new System.EventHandler(this.chkStoreLowLevelOverride_CheckedChanged);
				this.chkChainLowLevelOverride.CheckedChanged -= new System.EventHandler(this.chkChainLowLevelOverride_CheckedChanged);
				this.chkView.CheckedChanged -= new System.EventHandler(this.chkView_CheckedChanged);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.PlanViewSave_Closing);
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cboStoreHighLevelVers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.drsStoreHighLevelTime = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
			this.txtStoreHighLevelMerch = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.chkChainHighLevelAllStore = new System.Windows.Forms.CheckBox();
			this.cboChainHighLevelVers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.drsChainHighLevelTime = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
			this.txtChainHighLevelMerch = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpStoreHighLevel = new System.Windows.Forms.GroupBox();
			this.chkStoreHighLevel = new System.Windows.Forms.CheckBox();
			this.grpChainHighLevel = new System.Windows.Forms.GroupBox();
			this.chkChainHighLevel = new System.Windows.Forms.CheckBox();
			this.grpView = new System.Windows.Forms.GroupBox();
			this.rdoUser = new System.Windows.Forms.RadioButton();
			this.rdoGlobal = new System.Windows.Forms.RadioButton();
			this.txtViewName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkView = new System.Windows.Forms.CheckBox();
			this.chkStoreLowLevel = new System.Windows.Forms.CheckBox();
			this.chkChainLowLevel = new System.Windows.Forms.CheckBox();
			this.grpStoreLowLevel = new System.Windows.Forms.GroupBox();
			this.chkStoreLowLevelOverride = new System.Windows.Forms.CheckBox();
			this.pnlStoreLowLevelOverride = new System.Windows.Forms.Panel();
			this.drsStoreLowLevelTime = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
			this.label9 = new System.Windows.Forms.Label();
			this.cboStoreLowLevelVers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.label10 = new System.Windows.Forms.Label();
			this.grpChainLowLevel = new System.Windows.Forms.GroupBox();
			this.chkChainLowLevelOverride = new System.Windows.Forms.CheckBox();
			this.chkChainLowLevelAllStore = new System.Windows.Forms.CheckBox();
			this.pnlChainLowLevelOverride = new System.Windows.Forms.Panel();
			this.label11 = new System.Windows.Forms.Label();
			this.drsChainLowLevelTime = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
			this.cboChainLowLevelVers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.label13 = new System.Windows.Forms.Label();
			this.pnlStoreHighLevel = new System.Windows.Forms.Panel();
			this.pnlChainHighLevel = new System.Windows.Forms.Panel();
			this.pnlStoreLowLevel = new System.Windows.Forms.Panel();
			this.pnlChainLowLevel = new System.Windows.Forms.Panel();
			this.pnlView = new System.Windows.Forms.Panel();
			this.pnlChain = new System.Windows.Forms.Panel();
			this.pnlStore = new System.Windows.Forms.Panel();
			this.chkSaveLowToHighLevel = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.grpStoreHighLevel.SuspendLayout();
			this.grpChainHighLevel.SuspendLayout();
			this.grpView.SuspendLayout();
			this.grpStoreLowLevel.SuspendLayout();
			this.pnlStoreLowLevelOverride.SuspendLayout();
			this.grpChainLowLevel.SuspendLayout();
			this.pnlChainLowLevelOverride.SuspendLayout();
			this.pnlStoreHighLevel.SuspendLayout();
			this.pnlChainHighLevel.SuspendLayout();
			this.pnlStoreLowLevel.SuspendLayout();
			this.pnlChainLowLevel.SuspendLayout();
			this.pnlView.SuspendLayout();
			this.pnlChain.SuspendLayout();
			this.pnlStore.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// cboStoreHighLevelVers
			// 
			this.cboStoreHighLevelVers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStoreHighLevelVers.Location = new System.Drawing.Point(104, 72);
			this.cboStoreHighLevelVers.Name = "cboStoreHighLevelVers";
			this.cboStoreHighLevelVers.Size = new System.Drawing.Size(120, 21);
			this.cboStoreHighLevelVers.TabIndex = 5;
			// 
			// drsStoreHighLevelTime
			// 
			this.drsStoreHighLevelTime.DateRangeForm = null;
			this.drsStoreHighLevelTime.DateRangeRID = 0;
			this.drsStoreHighLevelTime.Enabled = false;
			this.drsStoreHighLevelTime.Location = new System.Drawing.Point(104, 48);
			this.drsStoreHighLevelTime.Name = "drsStoreHighLevelTime";
			this.drsStoreHighLevelTime.Size = new System.Drawing.Size(175, 25);
			this.drsStoreHighLevelTime.TabIndex = 4;
			this.drsStoreHighLevelTime.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsStoreHighLevelTime_OnSelection);
			this.drsStoreHighLevelTime.Click += new System.EventHandler(this.drsStoreHighLevelTime_Click);
			// 
			// txtStoreHighLevelMerch
			// 
			this.txtStoreHighLevelMerch.AllowDrop = true;
			this.txtStoreHighLevelMerch.Location = new System.Drawing.Point(104, 24);
			this.txtStoreHighLevelMerch.Name = "txtStoreHighLevelMerch";
			this.txtStoreHighLevelMerch.Size = new System.Drawing.Size(175, 20);
			this.txtStoreHighLevelMerch.TabIndex = 3;
			this.txtStoreHighLevelMerch.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerch_DragDrop);
			this.txtStoreHighLevelMerch.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerch_DragEnter);
			this.txtStoreHighLevelMerch.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerch_DragOver);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(73, 21);
			this.label5.TabIndex = 2;
			this.label5.Text = "Version:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(73, 21);
			this.label4.TabIndex = 1;
			this.label4.Text = "Time Period:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Merchandise:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkChainHighLevelAllStore
			// 
			this.chkChainHighLevelAllStore.Location = new System.Drawing.Point(104, 16);
			this.chkChainHighLevelAllStore.Name = "chkChainHighLevelAllStore";
			this.chkChainHighLevelAllStore.Size = new System.Drawing.Size(160, 14);
			this.chkChainHighLevelAllStore.TabIndex = 9;
			this.chkChainHighLevelAllStore.Text = "Save All Store as Chain";
			// 
			// cboChainHighLevelVers
			// 
			this.cboChainHighLevelVers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChainHighLevelVers.Location = new System.Drawing.Point(104, 88);
			this.cboChainHighLevelVers.Name = "cboChainHighLevelVers";
			this.cboChainHighLevelVers.Size = new System.Drawing.Size(120, 21);
			this.cboChainHighLevelVers.TabIndex = 8;
			// 
			// drsChainHighLevelTime
			// 
			this.drsChainHighLevelTime.DateRangeForm = null;
			this.drsChainHighLevelTime.DateRangeRID = 0;
			this.drsChainHighLevelTime.Enabled = false;
			this.drsChainHighLevelTime.Location = new System.Drawing.Point(104, 64);
			this.drsChainHighLevelTime.Name = "drsChainHighLevelTime";
			this.drsChainHighLevelTime.Size = new System.Drawing.Size(175, 25);
			this.drsChainHighLevelTime.TabIndex = 7;
			this.drsChainHighLevelTime.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsChainHighLevelTime_OnSelection);
			this.drsChainHighLevelTime.Click += new System.EventHandler(this.drsChainHighLevelTime_Click);
			// 
			// txtChainHighLevelMerch
			// 
			this.txtChainHighLevelMerch.AllowDrop = true;
			this.txtChainHighLevelMerch.Location = new System.Drawing.Point(104, 40);
			this.txtChainHighLevelMerch.Name = "txtChainHighLevelMerch";
			this.txtChainHighLevelMerch.Size = new System.Drawing.Size(175, 20);
			this.txtChainHighLevelMerch.TabIndex = 6;
			this.txtChainHighLevelMerch.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerch_DragDrop);
			this.txtChainHighLevelMerch.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerch_DragEnter);
			this.txtChainHighLevelMerch.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerch_DragOver);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 88);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(73, 21);
			this.label6.TabIndex = 11;
			this.label6.Text = "Version:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(73, 21);
			this.label7.TabIndex = 10;
			this.label7.Text = "Time Period:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 40);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(73, 20);
			this.label8.TabIndex = 9;
			this.label8.Text = "Merchandise:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.Location = new System.Drawing.Point(248, 352);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 0;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(336, 352);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// grpStoreHighLevel
			// 
			this.grpStoreHighLevel.Controls.Add(this.label3);
			this.grpStoreHighLevel.Controls.Add(this.txtStoreHighLevelMerch);
			this.grpStoreHighLevel.Controls.Add(this.drsStoreHighLevelTime);
			this.grpStoreHighLevel.Controls.Add(this.label4);
			this.grpStoreHighLevel.Controls.Add(this.cboStoreHighLevelVers);
			this.grpStoreHighLevel.Controls.Add(this.label5);
			this.grpStoreHighLevel.Location = new System.Drawing.Point(32, 8);
			this.grpStoreHighLevel.Name = "grpStoreHighLevel";
			this.grpStoreHighLevel.Size = new System.Drawing.Size(288, 104);
			this.grpStoreHighLevel.TabIndex = 5;
			this.grpStoreHighLevel.TabStop = false;
			this.grpStoreHighLevel.Text = "Store Plan";
			// 
			// chkStoreHighLevel
			// 
			this.chkStoreHighLevel.Location = new System.Drawing.Point(8, 8);
			this.chkStoreHighLevel.Name = "chkStoreHighLevel";
			this.chkStoreHighLevel.Size = new System.Drawing.Size(16, 24);
			this.chkStoreHighLevel.TabIndex = 6;
			this.chkStoreHighLevel.CheckedChanged += new System.EventHandler(this.chkStoreHighLevel_CheckedChanged);
			// 
			// grpChainHighLevel
			// 
			this.grpChainHighLevel.Controls.Add(this.chkSaveLowToHighLevel);
			this.grpChainHighLevel.Controls.Add(this.label7);
			this.grpChainHighLevel.Controls.Add(this.label8);
			this.grpChainHighLevel.Controls.Add(this.drsChainHighLevelTime);
			this.grpChainHighLevel.Controls.Add(this.cboChainHighLevelVers);
			this.grpChainHighLevel.Controls.Add(this.label6);
			this.grpChainHighLevel.Controls.Add(this.txtChainHighLevelMerch);
			this.grpChainHighLevel.Controls.Add(this.chkChainHighLevelAllStore);
			this.grpChainHighLevel.Location = new System.Drawing.Point(32, 8);
			this.grpChainHighLevel.Name = "grpChainHighLevel";
			this.grpChainHighLevel.Size = new System.Drawing.Size(288, 120);
			this.grpChainHighLevel.TabIndex = 7;
			this.grpChainHighLevel.TabStop = false;
			this.grpChainHighLevel.Text = "Chain Plan";
			// 
			// chkChainHighLevel
			// 
			this.chkChainHighLevel.Location = new System.Drawing.Point(8, 8);
			this.chkChainHighLevel.Name = "chkChainHighLevel";
			this.chkChainHighLevel.Size = new System.Drawing.Size(16, 24);
			this.chkChainHighLevel.TabIndex = 8;
			this.chkChainHighLevel.CheckedChanged += new System.EventHandler(this.chkChainHighLevel_CheckedChanged);
			// 
			// grpView
			// 
			this.grpView.Controls.Add(this.rdoUser);
			this.grpView.Controls.Add(this.rdoGlobal);
			this.grpView.Controls.Add(this.txtViewName);
			this.grpView.Controls.Add(this.label1);
			this.grpView.Location = new System.Drawing.Point(32, 8);
			this.grpView.Name = "grpView";
			this.grpView.Size = new System.Drawing.Size(288, 72);
			this.grpView.TabIndex = 9;
			this.grpView.TabStop = false;
			this.grpView.Text = "View";
			// 
			// rdoUser
			// 
			this.rdoUser.Location = new System.Drawing.Point(104, 48);
			this.rdoUser.Name = "rdoUser";
			this.rdoUser.Size = new System.Drawing.Size(48, 16);
			this.rdoUser.TabIndex = 13;
			this.rdoUser.Text = "User";
			// 
			// rdoGlobal
			// 
			this.rdoGlobal.Location = new System.Drawing.Point(160, 48);
			this.rdoGlobal.Name = "rdoGlobal";
			this.rdoGlobal.Size = new System.Drawing.Size(56, 16);
			this.rdoGlobal.TabIndex = 12;
			this.rdoGlobal.Text = "Global";
			// 
			// txtViewName
			// 
			this.txtViewName.AllowDrop = true;
			this.txtViewName.Location = new System.Drawing.Point(104, 16);
			this.txtViewName.Name = "txtViewName";
			this.txtViewName.Size = new System.Drawing.Size(175, 20);
			this.txtViewName.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 20);
			this.label1.TabIndex = 10;
			this.label1.Text = "View Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkView
			// 
			this.chkView.Location = new System.Drawing.Point(8, 8);
			this.chkView.Name = "chkView";
			this.chkView.Size = new System.Drawing.Size(16, 24);
			this.chkView.TabIndex = 10;
			this.chkView.CheckedChanged += new System.EventHandler(this.chkView_CheckedChanged);
			// 
			// chkStoreLowLevel
			// 
			this.chkStoreLowLevel.Location = new System.Drawing.Point(8, 8);
			this.chkStoreLowLevel.Name = "chkStoreLowLevel";
			this.chkStoreLowLevel.Size = new System.Drawing.Size(16, 24);
			this.chkStoreLowLevel.TabIndex = 11;
			this.chkStoreLowLevel.CheckedChanged += new System.EventHandler(this.chkStoreLowLevel_CheckedChanged);
			// 
			// chkChainLowLevel
			// 
			this.chkChainLowLevel.Location = new System.Drawing.Point(8, 8);
			this.chkChainLowLevel.Name = "chkChainLowLevel";
			this.chkChainLowLevel.Size = new System.Drawing.Size(16, 24);
			this.chkChainLowLevel.TabIndex = 12;
			this.chkChainLowLevel.CheckedChanged += new System.EventHandler(this.chkChainLowLevel_CheckedChanged);
			// 
			// grpStoreLowLevel
			// 
			this.grpStoreLowLevel.Controls.Add(this.chkStoreLowLevelOverride);
			this.grpStoreLowLevel.Controls.Add(this.pnlStoreLowLevelOverride);
			this.grpStoreLowLevel.Location = new System.Drawing.Point(32, 8);
			this.grpStoreLowLevel.Name = "grpStoreLowLevel";
			this.grpStoreLowLevel.Size = new System.Drawing.Size(288, 104);
			this.grpStoreLowLevel.TabIndex = 13;
			this.grpStoreLowLevel.TabStop = false;
			this.grpStoreLowLevel.Text = "Store Low-level Plans";
			// 
			// chkStoreLowLevelOverride
			// 
			this.chkStoreLowLevelOverride.Location = new System.Drawing.Point(96, 24);
			this.chkStoreLowLevelOverride.Name = "chkStoreLowLevelOverride";
			this.chkStoreLowLevelOverride.Size = new System.Drawing.Size(160, 14);
			this.chkStoreLowLevelOverride.TabIndex = 10;
			this.chkStoreLowLevelOverride.Text = "Override";
			this.chkStoreLowLevelOverride.CheckedChanged += new System.EventHandler(this.chkStoreLowLevelOverride_CheckedChanged);
			// 
			// pnlStoreLowLevelOverride
			// 
			this.pnlStoreLowLevelOverride.Controls.Add(this.drsStoreLowLevelTime);
			this.pnlStoreLowLevelOverride.Controls.Add(this.label9);
			this.pnlStoreLowLevelOverride.Controls.Add(this.cboStoreLowLevelVers);
			this.pnlStoreLowLevelOverride.Controls.Add(this.label10);
			this.pnlStoreLowLevelOverride.Location = new System.Drawing.Point(8, 40);
			this.pnlStoreLowLevelOverride.Name = "pnlStoreLowLevelOverride";
			this.pnlStoreLowLevelOverride.Size = new System.Drawing.Size(264, 56);
			this.pnlStoreLowLevelOverride.TabIndex = 22;
			// 
			// drsStoreLowLevelTime
			// 
			this.drsStoreLowLevelTime.DateRangeForm = null;
			this.drsStoreLowLevelTime.DateRangeRID = 0;
			this.drsStoreLowLevelTime.Enabled = false;
			this.drsStoreLowLevelTime.Location = new System.Drawing.Point(88, 8);
			this.drsStoreLowLevelTime.Name = "drsStoreLowLevelTime";
			this.drsStoreLowLevelTime.Size = new System.Drawing.Size(175, 25);
			this.drsStoreLowLevelTime.TabIndex = 4;
			this.drsStoreLowLevelTime.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsStoreLowLevelTime_OnSelection);
			this.drsStoreLowLevelTime.Click += new System.EventHandler(this.drsStoreLowLevelTime_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(0, 8);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(73, 21);
			this.label9.TabIndex = 1;
			this.label9.Text = "Time Period:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cboStoreLowLevelVers
			// 
			this.cboStoreLowLevelVers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStoreLowLevelVers.Location = new System.Drawing.Point(88, 32);
			this.cboStoreLowLevelVers.Name = "cboStoreLowLevelVers";
			this.cboStoreLowLevelVers.Size = new System.Drawing.Size(120, 21);
			this.cboStoreLowLevelVers.TabIndex = 5;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(0, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(73, 21);
			this.label10.TabIndex = 2;
			this.label10.Text = "Version:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpChainLowLevel
			// 
			this.grpChainLowLevel.Controls.Add(this.chkChainLowLevelOverride);
			this.grpChainLowLevel.Controls.Add(this.chkChainLowLevelAllStore);
			this.grpChainLowLevel.Controls.Add(this.pnlChainLowLevelOverride);
			this.grpChainLowLevel.Location = new System.Drawing.Point(32, 8);
			this.grpChainLowLevel.Name = "grpChainLowLevel";
			this.grpChainLowLevel.Size = new System.Drawing.Size(288, 120);
			this.grpChainLowLevel.TabIndex = 14;
			this.grpChainLowLevel.TabStop = false;
			this.grpChainLowLevel.Text = "Chain Low-level Plans";
			// 
			// chkChainLowLevelOverride
			// 
			this.chkChainLowLevelOverride.Location = new System.Drawing.Point(96, 40);
			this.chkChainLowLevelOverride.Name = "chkChainLowLevelOverride";
			this.chkChainLowLevelOverride.Size = new System.Drawing.Size(160, 14);
			this.chkChainLowLevelOverride.TabIndex = 12;
			this.chkChainLowLevelOverride.Text = "Override";
			this.chkChainLowLevelOverride.CheckedChanged += new System.EventHandler(this.chkChainLowLevelOverride_CheckedChanged);
			// 
			// chkChainLowLevelAllStore
			// 
			this.chkChainLowLevelAllStore.Location = new System.Drawing.Point(96, 16);
			this.chkChainLowLevelAllStore.Name = "chkChainLowLevelAllStore";
			this.chkChainLowLevelAllStore.Size = new System.Drawing.Size(160, 14);
			this.chkChainLowLevelAllStore.TabIndex = 9;
			this.chkChainLowLevelAllStore.Text = "Save All Store as Chain";
			// 
			// pnlChainLowLevelOverride
			// 
			this.pnlChainLowLevelOverride.Controls.Add(this.label11);
			this.pnlChainLowLevelOverride.Controls.Add(this.drsChainLowLevelTime);
			this.pnlChainLowLevelOverride.Controls.Add(this.cboChainLowLevelVers);
			this.pnlChainLowLevelOverride.Controls.Add(this.label13);
			this.pnlChainLowLevelOverride.Location = new System.Drawing.Point(8, 56);
			this.pnlChainLowLevelOverride.Name = "pnlChainLowLevelOverride";
			this.pnlChainLowLevelOverride.Size = new System.Drawing.Size(264, 56);
			this.pnlChainLowLevelOverride.TabIndex = 23;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(0, 8);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(72, 21);
			this.label11.TabIndex = 10;
			this.label11.Text = "Time Period:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// drsChainLowLevelTime
			// 
			this.drsChainLowLevelTime.DateRangeForm = null;
			this.drsChainLowLevelTime.DateRangeRID = 0;
			this.drsChainLowLevelTime.Enabled = false;
			this.drsChainLowLevelTime.Location = new System.Drawing.Point(88, 8);
			this.drsChainLowLevelTime.Name = "drsChainLowLevelTime";
			this.drsChainLowLevelTime.Size = new System.Drawing.Size(175, 25);
			this.drsChainLowLevelTime.TabIndex = 7;
			this.drsChainLowLevelTime.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsChainLowLevelTime_OnSelection);
			this.drsChainLowLevelTime.Click += new System.EventHandler(this.drsChainLowLevelTime_Click);
			// 
			// cboChainLowLevelVers
			// 
			this.cboChainLowLevelVers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChainLowLevelVers.Location = new System.Drawing.Point(88, 32);
			this.cboChainLowLevelVers.Name = "cboChainLowLevelVers";
			this.cboChainLowLevelVers.Size = new System.Drawing.Size(120, 21);
			this.cboChainLowLevelVers.TabIndex = 8;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(0, 32);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(73, 21);
			this.label13.TabIndex = 11;
			this.label13.Text = "Version:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnlStoreHighLevel
			// 
			this.pnlStoreHighLevel.Controls.Add(this.chkStoreHighLevel);
			this.pnlStoreHighLevel.Controls.Add(this.grpStoreHighLevel);
			this.pnlStoreHighLevel.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlStoreHighLevel.Location = new System.Drawing.Point(0, 0);
			this.pnlStoreHighLevel.Name = "pnlStoreHighLevel";
			this.pnlStoreHighLevel.Size = new System.Drawing.Size(328, 120);
			this.pnlStoreHighLevel.TabIndex = 15;
			// 
			// pnlChainHighLevel
			// 
			this.pnlChainHighLevel.Controls.Add(this.grpChainHighLevel);
			this.pnlChainHighLevel.Controls.Add(this.chkChainHighLevel);
			this.pnlChainHighLevel.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlChainHighLevel.Location = new System.Drawing.Point(0, 0);
			this.pnlChainHighLevel.Name = "pnlChainHighLevel";
			this.pnlChainHighLevel.Size = new System.Drawing.Size(328, 136);
			this.pnlChainHighLevel.TabIndex = 16;
			// 
			// pnlStoreLowLevel
			// 
			this.pnlStoreLowLevel.Controls.Add(this.grpStoreLowLevel);
			this.pnlStoreLowLevel.Controls.Add(this.chkStoreLowLevel);
			this.pnlStoreLowLevel.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlStoreLowLevel.Location = new System.Drawing.Point(328, 0);
			this.pnlStoreLowLevel.Name = "pnlStoreLowLevel";
			this.pnlStoreLowLevel.Size = new System.Drawing.Size(328, 120);
			this.pnlStoreLowLevel.TabIndex = 17;
			// 
			// pnlChainLowLevel
			// 
			this.pnlChainLowLevel.Controls.Add(this.grpChainLowLevel);
			this.pnlChainLowLevel.Controls.Add(this.chkChainLowLevel);
			this.pnlChainLowLevel.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlChainLowLevel.Location = new System.Drawing.Point(328, 0);
			this.pnlChainLowLevel.Name = "pnlChainLowLevel";
			this.pnlChainLowLevel.Size = new System.Drawing.Size(328, 136);
			this.pnlChainLowLevel.TabIndex = 18;
			// 
			// pnlView
			// 
			this.pnlView.Controls.Add(this.grpView);
			this.pnlView.Controls.Add(this.chkView);
			this.pnlView.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlView.Location = new System.Drawing.Point(0, 256);
			this.pnlView.Name = "pnlView";
			this.pnlView.Size = new System.Drawing.Size(658, 88);
			this.pnlView.TabIndex = 19;
			// 
			// pnlChain
			// 
			this.pnlChain.Controls.Add(this.pnlChainLowLevel);
			this.pnlChain.Controls.Add(this.pnlChainHighLevel);
			this.pnlChain.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlChain.Location = new System.Drawing.Point(0, 120);
			this.pnlChain.Name = "pnlChain";
			this.pnlChain.Size = new System.Drawing.Size(658, 136);
			this.pnlChain.TabIndex = 20;
			// 
			// pnlStore
			// 
			this.pnlStore.Controls.Add(this.pnlStoreLowLevel);
			this.pnlStore.Controls.Add(this.pnlStoreHighLevel);
			this.pnlStore.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlStore.Location = new System.Drawing.Point(0, 0);
			this.pnlStore.Name = "pnlStore";
			this.pnlStore.Size = new System.Drawing.Size(658, 120);
			this.pnlStore.TabIndex = 21;
			// 
			// chkSaveLowToHighLevel
			// 
			this.chkSaveLowToHighLevel.Location = new System.Drawing.Point(104, 16);
			this.chkSaveLowToHighLevel.Name = "chkSaveLowToHighLevel";
			this.chkSaveLowToHighLevel.Size = new System.Drawing.Size(160, 14);
			this.chkSaveLowToHighLevel.TabIndex = 12;
			this.chkSaveLowToHighLevel.Text = "Save Low to High Level";
			// 
			// PlanViewSave
			// 
			this.AcceptButton = this.btnSave;
			this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(658, 384);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.pnlView);
			this.Controls.Add(this.pnlChain);
			this.Controls.Add(this.pnlStore);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "PlanViewSave";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PlanView Save";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.PlanViewSave_Closing);
			this.Controls.SetChildIndex(this.pnlStore, 0);
			this.Controls.SetChildIndex(this.pnlChain, 0);
			this.Controls.SetChildIndex(this.pnlView, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.grpStoreHighLevel.ResumeLayout(false);
			this.grpStoreHighLevel.PerformLayout();
			this.grpChainHighLevel.ResumeLayout(false);
			this.grpChainHighLevel.PerformLayout();
			this.grpView.ResumeLayout(false);
			this.grpView.PerformLayout();
			this.grpStoreLowLevel.ResumeLayout(false);
			this.pnlStoreLowLevelOverride.ResumeLayout(false);
			this.grpChainLowLevel.ResumeLayout(false);
			this.pnlChainLowLevelOverride.ResumeLayout(false);
			this.pnlStoreHighLevel.ResumeLayout(false);
			this.pnlChainHighLevel.ResumeLayout(false);
			this.pnlStoreLowLevel.ResumeLayout(false);
			this.pnlChainLowLevel.ResumeLayout(false);
			this.pnlView.ResumeLayout(false);
			this.pnlChain.ResumeLayout(false);
			this.pnlStore.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStoreHighLevelVers;
		private System.Windows.Forms.TextBox txtStoreHighLevelMerch;
		private System.Windows.Forms.CheckBox chkChainHighLevelAllStore;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboChainHighLevelVers;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsChainHighLevelTime;
		private System.Windows.Forms.TextBox txtChainHighLevelMerch;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsStoreHighLevelTime;
		private System.Windows.Forms.GroupBox grpStoreHighLevel;
		private System.Windows.Forms.CheckBox chkStoreHighLevel;
		private System.Windows.Forms.GroupBox grpChainHighLevel;
		private System.Windows.Forms.CheckBox chkChainHighLevel;
		private System.Windows.Forms.GroupBox grpView;
		private System.Windows.Forms.CheckBox chkView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtViewName;
		private System.Windows.Forms.RadioButton rdoGlobal;
		private System.Windows.Forms.RadioButton rdoUser;
		private System.Windows.Forms.CheckBox chkStoreLowLevel;
		private System.Windows.Forms.CheckBox chkChainLowLevel;
		private System.Windows.Forms.GroupBox grpStoreLowLevel;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsStoreLowLevelTime;
		private System.Windows.Forms.Label label9;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStoreLowLevelVers;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.GroupBox grpChainLowLevel;
		private System.Windows.Forms.Label label11;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsChainLowLevelTime;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboChainLowLevelVers;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox chkChainLowLevelAllStore;
		private System.Windows.Forms.CheckBox chkStoreLowLevelOverride;
		private System.Windows.Forms.CheckBox chkChainLowLevelOverride;
		private System.Windows.Forms.Panel pnlStoreHighLevel;
		private System.Windows.Forms.Panel pnlChainHighLevel;
		private System.Windows.Forms.Panel pnlStoreLowLevel;
		private System.Windows.Forms.Panel pnlChainLowLevel;
		private System.Windows.Forms.Panel pnlView;
		private System.Windows.Forms.Panel pnlChain;
		private System.Windows.Forms.Panel pnlStore;
		private System.Windows.Forms.Panel pnlStoreLowLevelOverride;
		private System.Windows.Forms.Panel pnlChainLowLevelOverride;
		private System.Windows.Forms.CheckBox chkSaveLowToHighLevel;
	}
}