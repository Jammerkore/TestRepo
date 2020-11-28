namespace MIDRetailInstaller
{
    partial class ucClient
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpClient = new System.Windows.Forms.GroupBox();
            this.lblShortcutName = new System.Windows.Forms.Label();
            this.txtShortcutName = new System.Windows.Forms.TextBox();
            this.lblSelectGlobalConfig = new System.Windows.Forms.Label();
            this.btnGlobalConfigurationBrowse = new System.Windows.Forms.Button();
            this.txtGlobalConfigurationFile = new System.Windows.Forms.TextBox();
            this.lblShareName = new System.Windows.Forms.Label();
            this.txtShareName = new System.Windows.Forms.TextBox();
            this.chxShareClient = new System.Windows.Forms.CheckBox();
            this.chkAutoUpgradeClient = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAccess = new System.Windows.Forms.Label();
            this.rdoMe = new System.Windows.Forms.RadioButton();
            this.rdoEveryone = new System.Windows.Forms.RadioButton();
            this.chkQuickLaunch = new System.Windows.Forms.CheckBox();
            this.chkDesktop = new System.Windows.Forms.CheckBox();
            this.lblInstallDir = new System.Windows.Forms.Label();
            this.btnInstallFolder = new System.Windows.Forms.Button();
            this.txtInstallFolder = new System.Windows.Forms.TextBox();
            this.rdoInstallClient = new System.Windows.Forms.RadioButton();
            this.folderInstallFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.grpInstalledClients = new System.Windows.Forms.GroupBox();
            this.lstInstalledClients = new System.Windows.Forms.ListBox();
            this.lblTasks = new System.Windows.Forms.Label();
            this.cboTasks = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.rdoInstallClientTasks = new System.Windows.Forms.RadioButton();
            this.rdoInstallTypical = new System.Windows.Forms.RadioButton();
            this.ofdBrowser = new System.Windows.Forms.OpenFileDialog();
            this.gbxTypical = new System.Windows.Forms.GroupBox();
            this.lblSelectGlobalConfigTypical = new System.Windows.Forms.Label();
            this.btnGlobalConfigurationBrowseTypical = new System.Windows.Forms.Button();
            this.txtGlobalConfigurationFileTypical = new System.Windows.Forms.TextBox();
            this.grpClient.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpInstalledClients.SuspendLayout();
            this.gbxTypical.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpClient
            // 
            this.grpClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpClient.Controls.Add(this.lblShortcutName);
            this.grpClient.Controls.Add(this.txtShortcutName);
            this.grpClient.Controls.Add(this.lblSelectGlobalConfig);
            this.grpClient.Controls.Add(this.btnGlobalConfigurationBrowse);
            this.grpClient.Controls.Add(this.txtGlobalConfigurationFile);
            this.grpClient.Controls.Add(this.lblShareName);
            this.grpClient.Controls.Add(this.txtShareName);
            this.grpClient.Controls.Add(this.chxShareClient);
            this.grpClient.Controls.Add(this.chkAutoUpgradeClient);
            this.grpClient.Controls.Add(this.panel1);
            this.grpClient.Controls.Add(this.chkQuickLaunch);
            this.grpClient.Controls.Add(this.chkDesktop);
            this.grpClient.Controls.Add(this.lblInstallDir);
            this.grpClient.Controls.Add(this.btnInstallFolder);
            this.grpClient.Controls.Add(this.txtInstallFolder);
            this.grpClient.Location = new System.Drawing.Point(7, 92);
            this.grpClient.Name = "grpClient";
            this.grpClient.Size = new System.Drawing.Size(665, 169);
            this.grpClient.TabIndex = 0;
            this.grpClient.TabStop = false;
            // 
            // lblShortcutName
            // 
            this.lblShortcutName.AutoSize = true;
            this.lblShortcutName.Location = new System.Drawing.Point(42, 130);
            this.lblShortcutName.Name = "lblShortcutName";
            this.lblShortcutName.Size = new System.Drawing.Size(81, 13);
            this.lblShortcutName.TabIndex = 34;
            this.lblShortcutName.Text = "Shortcut Name:";
            // 
            // txtShortcutName
            // 
            this.txtShortcutName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShortcutName.Location = new System.Drawing.Point(123, 126);
            this.txtShortcutName.Name = "txtShortcutName";
            this.txtShortcutName.Size = new System.Drawing.Size(177, 20);
            this.txtShortcutName.TabIndex = 33;
            // 
            // lblSelectGlobalConfig
            // 
            this.lblSelectGlobalConfig.AutoSize = true;
            this.lblSelectGlobalConfig.Location = new System.Drawing.Point(44, 66);
            this.lblSelectGlobalConfig.Name = "lblSelectGlobalConfig";
            this.lblSelectGlobalConfig.Size = new System.Drawing.Size(105, 13);
            this.lblSelectGlobalConfig.TabIndex = 32;
            this.lblSelectGlobalConfig.Text = "Global Configuration:";
            // 
            // btnGlobalConfigurationBrowse
            // 
            this.btnGlobalConfigurationBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGlobalConfigurationBrowse.Location = new System.Drawing.Point(552, 61);
            this.btnGlobalConfigurationBrowse.Name = "btnGlobalConfigurationBrowse";
            this.btnGlobalConfigurationBrowse.Size = new System.Drawing.Size(102, 23);
            this.btnGlobalConfigurationBrowse.TabIndex = 31;
            this.btnGlobalConfigurationBrowse.Text = "Browse";
            this.btnGlobalConfigurationBrowse.UseVisualStyleBackColor = true;
            this.btnGlobalConfigurationBrowse.Click += new System.EventHandler(this.btnGlobalConfigurationBrowse_Click);
            // 
            // txtGlobalConfigurationFile
            // 
            this.txtGlobalConfigurationFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGlobalConfigurationFile.Location = new System.Drawing.Point(164, 63);
            this.txtGlobalConfigurationFile.Name = "txtGlobalConfigurationFile";
            this.txtGlobalConfigurationFile.Size = new System.Drawing.Size(382, 20);
            this.txtGlobalConfigurationFile.TabIndex = 30;
            // 
            // lblShareName
            // 
            this.lblShareName.AutoSize = true;
            this.lblShareName.Location = new System.Drawing.Point(429, 130);
            this.lblShareName.Name = "lblShareName";
            this.lblShareName.Size = new System.Drawing.Size(69, 13);
            this.lblShareName.TabIndex = 29;
            this.lblShareName.Text = "Share Name:";
            // 
            // txtShareName
            // 
            this.txtShareName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShareName.Location = new System.Drawing.Point(502, 126);
            this.txtShareName.Name = "txtShareName";
            this.txtShareName.Size = new System.Drawing.Size(150, 20);
            this.txtShareName.TabIndex = 28;
            // 
            // chxShareClient
            // 
            this.chxShareClient.AutoSize = true;
            this.chxShareClient.Location = new System.Drawing.Point(328, 128);
            this.chxShareClient.Name = "chxShareClient";
            this.chxShareClient.Size = new System.Drawing.Size(83, 17);
            this.chxShareClient.TabIndex = 27;
            this.chxShareClient.Text = "Share Client";
            this.chxShareClient.UseVisualStyleBackColor = true;
            this.chxShareClient.CheckedChanged += new System.EventHandler(this.chxShareClient_CheckedChanged);
            // 
            // chkAutoUpgradeClient
            // 
            this.chkAutoUpgradeClient.AutoSize = true;
            this.chkAutoUpgradeClient.Location = new System.Drawing.Point(328, 97);
            this.chkAutoUpgradeClient.Name = "chkAutoUpgradeClient";
            this.chkAutoUpgradeClient.Size = new System.Drawing.Size(92, 17);
            this.chkAutoUpgradeClient.TabIndex = 26;
            this.chkAutoUpgradeClient.Text = "Auto Upgrade";
            this.chkAutoUpgradeClient.UseVisualStyleBackColor = true;
            this.chkAutoUpgradeClient.CheckedChanged += new System.EventHandler(this.chkAutoUpgradeClient_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAccess);
            this.panel1.Controls.Add(this.rdoMe);
            this.panel1.Controls.Add(this.rdoEveryone);
            this.panel1.Location = new System.Drawing.Point(458, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 26);
            this.panel1.TabIndex = 25;
            // 
            // lblAccess
            // 
            this.lblAccess.AutoSize = true;
            this.lblAccess.Location = new System.Drawing.Point(7, 7);
            this.lblAccess.Name = "lblAccess";
            this.lblAccess.Size = new System.Drawing.Size(45, 13);
            this.lblAccess.TabIndex = 22;
            this.lblAccess.Text = "Access:";
            // 
            // rdoMe
            // 
            this.rdoMe.AutoSize = true;
            this.rdoMe.Location = new System.Drawing.Point(130, 5);
            this.rdoMe.Name = "rdoMe";
            this.rdoMe.Size = new System.Drawing.Size(62, 17);
            this.rdoMe.TabIndex = 21;
            this.rdoMe.Text = "Just Me";
            this.rdoMe.UseVisualStyleBackColor = true;
            // 
            // rdoEveryone
            // 
            this.rdoEveryone.AutoSize = true;
            this.rdoEveryone.Checked = true;
            this.rdoEveryone.Location = new System.Drawing.Point(58, 5);
            this.rdoEveryone.Name = "rdoEveryone";
            this.rdoEveryone.Size = new System.Drawing.Size(70, 17);
            this.rdoEveryone.TabIndex = 20;
            this.rdoEveryone.TabStop = true;
            this.rdoEveryone.Text = "Everyone";
            this.rdoEveryone.UseVisualStyleBackColor = true;
            // 
            // chkQuickLaunch
            // 
            this.chkQuickLaunch.AutoSize = true;
            this.chkQuickLaunch.Location = new System.Drawing.Point(175, 97);
            this.chkQuickLaunch.Name = "chkQuickLaunch";
            this.chkQuickLaunch.Size = new System.Drawing.Size(136, 17);
            this.chkQuickLaunch.TabIndex = 16;
            this.chkQuickLaunch.Text = "Pin to Taskbar";
            this.chkQuickLaunch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkQuickLaunch.UseVisualStyleBackColor = true;
            this.chkQuickLaunch.Visible = false;
            this.chkQuickLaunch.CheckedChanged += new System.EventHandler(this.chkQuickLaunch_CheckedChanged);
            // 
            // chkDesktop
            // 
            this.chkDesktop.AutoSize = true;
            this.chkDesktop.Checked = true;
            this.chkDesktop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDesktop.Location = new System.Drawing.Point(45, 97);
            this.chkDesktop.Name = "chkDesktop";
            this.chkDesktop.Size = new System.Drawing.Size(109, 17);
            this.chkDesktop.TabIndex = 15;
            this.chkDesktop.Text = "Desktop Shortcut";
            this.chkDesktop.UseVisualStyleBackColor = true;
            this.chkDesktop.CheckedChanged += new System.EventHandler(this.chkDesktop_CheckedChanged);
            // 
            // lblInstallDir
            // 
            this.lblInstallDir.AutoSize = true;
            this.lblInstallDir.Location = new System.Drawing.Point(44, 32);
            this.lblInstallDir.Name = "lblInstallDir";
            this.lblInstallDir.Size = new System.Drawing.Size(118, 13);
            this.lblInstallDir.TabIndex = 10;
            this.lblInstallDir.Text = "Choose install directory:";
            // 
            // btnInstallFolder
            // 
            this.btnInstallFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstallFolder.Location = new System.Drawing.Point(552, 27);
            this.btnInstallFolder.Name = "btnInstallFolder";
            this.btnInstallFolder.Size = new System.Drawing.Size(102, 23);
            this.btnInstallFolder.TabIndex = 8;
            this.btnInstallFolder.Text = "Change Directory";
            this.btnInstallFolder.UseVisualStyleBackColor = true;
            this.btnInstallFolder.Click += new System.EventHandler(this.btnInstallFolder_Click);
            // 
            // txtInstallFolder
            // 
            this.txtInstallFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstallFolder.Location = new System.Drawing.Point(164, 29);
            this.txtInstallFolder.Name = "txtInstallFolder";
            this.txtInstallFolder.Size = new System.Drawing.Size(382, 20);
            this.txtInstallFolder.TabIndex = 7;
            this.txtInstallFolder.Text = "C:\\MIDRetail";
            // 
            // rdoInstallClient
            // 
            this.rdoInstallClient.AutoSize = true;
            this.rdoInstallClient.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstallClient.Location = new System.Drawing.Point(16, 88);
            this.rdoInstallClient.Name = "rdoInstallClient";
            this.rdoInstallClient.Size = new System.Drawing.Size(119, 17);
            this.rdoInstallClient.TabIndex = 2;
            this.rdoInstallClient.Text = "Custom Client Install";
            this.rdoInstallClient.UseVisualStyleBackColor = true;
            this.rdoInstallClient.CheckedChanged += new System.EventHandler(this.rdoInstallClient_CheckedChanged);
            // 
            // grpInstalledClients
            // 
            this.grpInstalledClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInstalledClients.Controls.Add(this.lstInstalledClients);
            this.grpInstalledClients.Controls.Add(this.lblTasks);
            this.grpInstalledClients.Controls.Add(this.cboTasks);
            this.grpInstalledClients.Enabled = false;
            this.grpInstalledClients.Location = new System.Drawing.Point(7, 270);
            this.grpInstalledClients.Name = "grpInstalledClients";
            this.grpInstalledClients.Size = new System.Drawing.Size(665, 160);
            this.grpInstalledClients.TabIndex = 1;
            this.grpInstalledClients.TabStop = false;
            // 
            // lstInstalledClients
            // 
            this.lstInstalledClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInstalledClients.FormattingEnabled = true;
            this.lstInstalledClients.IntegralHeight = false;
            this.lstInstalledClients.Location = new System.Drawing.Point(45, 46);
            this.lstInstalledClients.Name = "lstInstalledClients";
            this.lstInstalledClients.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstInstalledClients.Size = new System.Drawing.Size(608, 104);
            this.lstInstalledClients.TabIndex = 24;
            this.lstInstalledClients.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstInstalledClients_KeyDown);
            // 
            // lblTasks
            // 
            this.lblTasks.AutoSize = true;
            this.lblTasks.Location = new System.Drawing.Point(407, 20);
            this.lblTasks.Name = "lblTasks";
            this.lblTasks.Size = new System.Drawing.Size(34, 13);
            this.lblTasks.TabIndex = 23;
            this.lblTasks.Text = "Task:";
            // 
            // cboTasks
            // 
            this.cboTasks.AutoAdjust = true;
            this.cboTasks.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTasks.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTasks.DataSource = null;
            this.cboTasks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTasks.DropDownWidth = 206;
            this.cboTasks.FormattingEnabled = true;
            this.cboTasks.IgnoreFocusLost = false;
            this.cboTasks.ItemHeight = 13;
            this.cboTasks.Items.AddRange(new object[] {
            "Upgrade",
            "Uninstall",
            //"Auto Upgrade Client",
            "Configure"});
            this.cboTasks.Location = new System.Drawing.Point(447, 16);
            this.cboTasks.Margin = new System.Windows.Forms.Padding(0);
            this.cboTasks.MaxDropDownItems = 25;
            this.cboTasks.Name = "cboTasks";
            this.cboTasks.SetToolTip = "";
            this.cboTasks.Size = new System.Drawing.Size(206, 21);
            this.cboTasks.TabIndex = 22;
            this.cboTasks.Text = "Upgrade";
            this.cboTasks.Tag = null;
            this.cboTasks.SelectedIndexChanged += new System.EventHandler(this.cboTasks_SelectedIndexChanged);
            // 
            // rdoInstallClientTasks
            // 
            this.rdoInstallClientTasks.AutoSize = true;
            this.rdoInstallClientTasks.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstallClientTasks.Location = new System.Drawing.Point(16, 266);
            this.rdoInstallClientTasks.Name = "rdoInstallClientTasks";
            this.rdoInstallClientTasks.Size = new System.Drawing.Size(125, 17);
            this.rdoInstallClientTasks.TabIndex = 21;
            this.rdoInstallClientTasks.Text = "Installed Client Tasks";
            this.rdoInstallClientTasks.UseVisualStyleBackColor = true;
            this.rdoInstallClientTasks.CheckedChanged += new System.EventHandler(this.rdoInstallClientTasks_CheckedChanged);
            // 
            // rdoInstallTypical
            // 
            this.rdoInstallTypical.AutoSize = true;
            this.rdoInstallTypical.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstallTypical.Location = new System.Drawing.Point(16, 24);
            this.rdoInstallTypical.Name = "rdoInstallTypical";
            this.rdoInstallTypical.Size = new System.Drawing.Size(118, 17);
            this.rdoInstallTypical.TabIndex = 22;
            this.rdoInstallTypical.Text = "Typical Client Install";
            this.rdoInstallTypical.UseVisualStyleBackColor = true;
            this.rdoInstallTypical.CheckedChanged += new System.EventHandler(this.rdoInstallTypical_CheckedChanged);
            // 
            // ofdBrowser
            // 
            this.ofdBrowser.FileName = "openFileDialog1";
            // 
            // gbxTypical
            // 
            this.gbxTypical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxTypical.Controls.Add(this.lblSelectGlobalConfigTypical);
            this.gbxTypical.Controls.Add(this.btnGlobalConfigurationBrowseTypical);
            this.gbxTypical.Controls.Add(this.txtGlobalConfigurationFileTypical);
            this.gbxTypical.Location = new System.Drawing.Point(7, 28);
            this.gbxTypical.Name = "gbxTypical";
            this.gbxTypical.Size = new System.Drawing.Size(665, 54);
            this.gbxTypical.TabIndex = 23;
            this.gbxTypical.TabStop = false;
            // 
            // lblSelectGlobalConfigTypical
            // 
            this.lblSelectGlobalConfigTypical.AutoSize = true;
            this.lblSelectGlobalConfigTypical.Location = new System.Drawing.Point(44, 21);
            this.lblSelectGlobalConfigTypical.Name = "lblSelectGlobalConfigTypical";
            this.lblSelectGlobalConfigTypical.Size = new System.Drawing.Size(105, 13);
            this.lblSelectGlobalConfigTypical.TabIndex = 35;
            this.lblSelectGlobalConfigTypical.Text = "Global Configuration:";
            // 
            // btnGlobalConfigurationBrowseTypical
            // 
            this.btnGlobalConfigurationBrowseTypical.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGlobalConfigurationBrowseTypical.Location = new System.Drawing.Point(552, 16);
            this.btnGlobalConfigurationBrowseTypical.Name = "btnGlobalConfigurationBrowseTypical";
            this.btnGlobalConfigurationBrowseTypical.Size = new System.Drawing.Size(102, 23);
            this.btnGlobalConfigurationBrowseTypical.TabIndex = 34;
            this.btnGlobalConfigurationBrowseTypical.Text = "Browse";
            this.btnGlobalConfigurationBrowseTypical.UseVisualStyleBackColor = true;
            this.btnGlobalConfigurationBrowseTypical.Click += new System.EventHandler(this.btnGlobalConfigurationBrowseTypical_Click);
            // 
            // txtGlobalConfigurationFileTypical
            // 
            this.txtGlobalConfigurationFileTypical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGlobalConfigurationFileTypical.Location = new System.Drawing.Point(164, 18);
            this.txtGlobalConfigurationFileTypical.Name = "txtGlobalConfigurationFileTypical";
            this.txtGlobalConfigurationFileTypical.Size = new System.Drawing.Size(382, 20);
            this.txtGlobalConfigurationFileTypical.TabIndex = 33;
            // 
            // ucClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoInstallTypical);
            this.Controls.Add(this.gbxTypical);
            this.Controls.Add(this.rdoInstallClientTasks);
            this.Controls.Add(this.rdoInstallClient);
            this.Controls.Add(this.grpInstalledClients);
            this.Controls.Add(this.grpClient);
            this.Name = "ucClient";
            this.Size = new System.Drawing.Size(680, 435);
            this.Load += new System.EventHandler(this.ucClient_Load);
            this.VisibleChanged += new System.EventHandler(this.ucClient_VisibleChanged);
            this.grpClient.ResumeLayout(false);
            this.grpClient.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpInstalledClients.ResumeLayout(false);
            this.grpInstalledClients.PerformLayout();
            this.gbxTypical.ResumeLayout(false);
            this.gbxTypical.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpClient;
        private System.Windows.Forms.RadioButton rdoInstallClient;
        private System.Windows.Forms.Button btnInstallFolder;
        private System.Windows.Forms.TextBox txtInstallFolder;
        private System.Windows.Forms.FolderBrowserDialog folderInstallFolder;
        private System.Windows.Forms.Label lblInstallDir;
        private System.Windows.Forms.CheckBox chkQuickLaunch;
        private System.Windows.Forms.CheckBox chkDesktop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAccess;
        private System.Windows.Forms.RadioButton rdoMe;
        private System.Windows.Forms.RadioButton rdoEveryone;
        private System.Windows.Forms.GroupBox grpInstalledClients;
        public System.Windows.Forms.ListBox lstInstalledClients;
        private System.Windows.Forms.Label lblTasks;
        public MIDRetail.Windows.Controls.MIDComboBoxEnh cboTasks;
        public System.Windows.Forms.RadioButton rdoInstallClientTasks;
        private System.Windows.Forms.CheckBox chkAutoUpgradeClient;
        private System.Windows.Forms.RadioButton rdoInstallTypical;
        private System.Windows.Forms.TextBox txtShareName;
        private System.Windows.Forms.CheckBox chxShareClient;
        private System.Windows.Forms.Label lblShareName;
        private System.Windows.Forms.Label lblSelectGlobalConfig;
        private System.Windows.Forms.Button btnGlobalConfigurationBrowse;
        private System.Windows.Forms.TextBox txtGlobalConfigurationFile;
        private System.Windows.Forms.OpenFileDialog ofdBrowser;
        private System.Windows.Forms.GroupBox gbxTypical;
        private System.Windows.Forms.Label lblSelectGlobalConfigTypical;
        private System.Windows.Forms.Button btnGlobalConfigurationBrowseTypical;
        private System.Windows.Forms.TextBox txtGlobalConfigurationFileTypical;
        private System.Windows.Forms.Label lblShortcutName;
        private System.Windows.Forms.TextBox txtShortcutName;
    }
}
