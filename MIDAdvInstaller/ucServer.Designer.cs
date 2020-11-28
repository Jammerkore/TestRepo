namespace MIDRetailInstaller
{
    partial class ucServer
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
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.gbxConfigureUsing = new System.Windows.Forms.GroupBox();
            this.rdoUseIPAddress = new System.Windows.Forms.RadioButton();
            this.rdoUseMachineName = new System.Windows.Forms.RadioButton();
            this.gbxStartType = new System.Windows.Forms.GroupBox();
            this.rdoStartTypeManual = new System.Windows.Forms.RadioButton();
            this.rdoStartTypeAuto = new System.Windows.Forms.RadioButton();
            this.lblInstallDir = new System.Windows.Forms.Label();
            this.btnInstallFolder = new System.Windows.Forms.Button();
            this.txtInstallFolder = new System.Windows.Forms.TextBox();
            this.clstServices = new System.Windows.Forms.CheckedListBox();
            this.rdoInstallServer = new System.Windows.Forms.RadioButton();
            this.folderInstallFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.grpInstalledServerComponents = new System.Windows.Forms.GroupBox();
            this.lblTasks = new System.Windows.Forms.Label();
            this.cboTasks = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lstInstalledServices = new System.Windows.Forms.ListBox();
            this.rdoInstalledServerTasks = new System.Windows.Forms.RadioButton();
            this.grpClientConfiguration = new System.Windows.Forms.GroupBox();
            this.btnConfigurationInstallFolder = new System.Windows.Forms.Button();
            this.txtConfigurationInstallFolder = new System.Windows.Forms.TextBox();
            this.lblInstallDir2 = new System.Windows.Forms.Label();
            this.rdoInstallConfiguration = new System.Windows.Forms.RadioButton();
            this.rdoInstallTypical = new System.Windows.Forms.RadioButton();
            this.grpServer.SuspendLayout();
            this.gbxConfigureUsing.SuspendLayout();
            this.gbxStartType.SuspendLayout();
            this.grpInstalledServerComponents.SuspendLayout();
            this.grpClientConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpServer
            // 
            this.grpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpServer.Controls.Add(this.gbxConfigureUsing);
            this.grpServer.Controls.Add(this.gbxStartType);
            this.grpServer.Controls.Add(this.lblInstallDir);
            this.grpServer.Controls.Add(this.btnInstallFolder);
            this.grpServer.Controls.Add(this.txtInstallFolder);
            this.grpServer.Controls.Add(this.clstServices);
            this.grpServer.Location = new System.Drawing.Point(7, 44);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(665, 137);
            this.grpServer.TabIndex = 0;
            this.grpServer.TabStop = false;
            // 
            // gbxConfigureUsing
            // 
            this.gbxConfigureUsing.Controls.Add(this.rdoUseIPAddress);
            this.gbxConfigureUsing.Controls.Add(this.rdoUseMachineName);
            this.gbxConfigureUsing.Location = new System.Drawing.Point(320, 54);
            this.gbxConfigureUsing.Name = "gbxConfigureUsing";
            this.gbxConfigureUsing.Size = new System.Drawing.Size(125, 76);
            this.gbxConfigureUsing.TabIndex = 39;
            this.gbxConfigureUsing.TabStop = false;
            this.gbxConfigureUsing.Text = "Configure Using";
            // 
            // rdoUseIPAddress
            // 
            this.rdoUseIPAddress.AutoSize = true;
            this.rdoUseIPAddress.Location = new System.Drawing.Point(12, 43);
            this.rdoUseIPAddress.Name = "rdoUseIPAddress";
            this.rdoUseIPAddress.Size = new System.Drawing.Size(76, 17);
            this.rdoUseIPAddress.TabIndex = 1;
            this.rdoUseIPAddress.Text = "IP Address";
            this.rdoUseIPAddress.UseVisualStyleBackColor = true;
            this.rdoUseIPAddress.CheckedChanged += new System.EventHandler(this.rdoUseIPAddress_CheckedChanged);
            // 
            // rdoUseMachineName
            // 
            this.rdoUseMachineName.AutoSize = true;
            this.rdoUseMachineName.Checked = true;
            this.rdoUseMachineName.Location = new System.Drawing.Point(12, 20);
            this.rdoUseMachineName.Name = "rdoUseMachineName";
            this.rdoUseMachineName.Size = new System.Drawing.Size(97, 17);
            this.rdoUseMachineName.TabIndex = 0;
            this.rdoUseMachineName.TabStop = true;
            this.rdoUseMachineName.Text = "Machine Name";
            this.rdoUseMachineName.UseVisualStyleBackColor = true;
            this.rdoUseMachineName.CheckedChanged += new System.EventHandler(this.rdoUseMachineName_CheckedChanged);
            // 
            // gbxStartType
            // 
            this.gbxStartType.Controls.Add(this.rdoStartTypeManual);
            this.gbxStartType.Controls.Add(this.rdoStartTypeAuto);
            this.gbxStartType.Location = new System.Drawing.Point(200, 54);
            this.gbxStartType.Name = "gbxStartType";
            this.gbxStartType.Size = new System.Drawing.Size(103, 76);
            this.gbxStartType.TabIndex = 38;
            this.gbxStartType.TabStop = false;
            this.gbxStartType.Text = "Start Type";
            // 
            // rdoStartTypeManual
            // 
            this.rdoStartTypeManual.AutoSize = true;
            this.rdoStartTypeManual.Location = new System.Drawing.Point(15, 43);
            this.rdoStartTypeManual.Name = "rdoStartTypeManual";
            this.rdoStartTypeManual.Size = new System.Drawing.Size(60, 17);
            this.rdoStartTypeManual.TabIndex = 1;
            this.rdoStartTypeManual.Text = "Manual";
            this.rdoStartTypeManual.UseVisualStyleBackColor = true;
            // 
            // rdoStartTypeAuto
            // 
            this.rdoStartTypeAuto.AutoSize = true;
            this.rdoStartTypeAuto.Checked = true;
            this.rdoStartTypeAuto.Location = new System.Drawing.Point(15, 20);
            this.rdoStartTypeAuto.Name = "rdoStartTypeAuto";
            this.rdoStartTypeAuto.Size = new System.Drawing.Size(72, 17);
            this.rdoStartTypeAuto.TabIndex = 0;
            this.rdoStartTypeAuto.TabStop = true;
            this.rdoStartTypeAuto.Text = "Automatic";
            this.rdoStartTypeAuto.UseVisualStyleBackColor = true;
            // 
            // lblInstallDir
            // 
            this.lblInstallDir.AutoSize = true;
            this.lblInstallDir.Location = new System.Drawing.Point(45, 28);
            this.lblInstallDir.Name = "lblInstallDir";
            this.lblInstallDir.Size = new System.Drawing.Size(118, 13);
            this.lblInstallDir.TabIndex = 13;
            this.lblInstallDir.Text = "Choose install directory:";
            // 
            // btnInstallFolder
            // 
            this.btnInstallFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstallFolder.Location = new System.Drawing.Point(552, 25);
            this.btnInstallFolder.Name = "btnInstallFolder";
            this.btnInstallFolder.Size = new System.Drawing.Size(102, 23);
            this.btnInstallFolder.TabIndex = 12;
            this.btnInstallFolder.Text = "Change Directory";
            this.btnInstallFolder.UseVisualStyleBackColor = true;
            this.btnInstallFolder.Click += new System.EventHandler(this.btnInstallFolder_Click);
            // 
            // txtInstallFolder
            // 
            this.txtInstallFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstallFolder.Location = new System.Drawing.Point(178, 26);
            this.txtInstallFolder.Name = "txtInstallFolder";
            this.txtInstallFolder.ReadOnly = true;
            this.txtInstallFolder.Size = new System.Drawing.Size(368, 20);
            this.txtInstallFolder.TabIndex = 11;
            this.txtInstallFolder.Text = "C:\\MIDRetail";
            // 
            // clstServices
            // 
            this.clstServices.BackColor = System.Drawing.SystemColors.Control;
            this.clstServices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clstServices.CheckOnClick = true;
            this.clstServices.ColumnWidth = 115;
            this.clstServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clstServices.FormattingEnabled = true;
            this.clstServices.IntegralHeight = false;
            this.clstServices.Items.AddRange(new object[] {
            "API",
            "Control Service",
            "Hierarchy Service",
            "Scheduler Service",
            "Store Service"});
            this.clstServices.Location = new System.Drawing.Point(47, 52);
            this.clstServices.MultiColumn = true;
            this.clstServices.Name = "clstServices";
            this.clstServices.Size = new System.Drawing.Size(155, 77);
            this.clstServices.TabIndex = 6;
            // 
            // rdoInstallServer
            // 
            this.rdoInstallServer.AutoSize = true;
            this.rdoInstallServer.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstallServer.Location = new System.Drawing.Point(16, 41);
            this.rdoInstallServer.Name = "rdoInstallServer";
            this.rdoInstallServer.Size = new System.Drawing.Size(124, 17);
            this.rdoInstallServer.TabIndex = 2;
            this.rdoInstallServer.Text = "Custom Server Install";
            this.rdoInstallServer.UseVisualStyleBackColor = true;
            this.rdoInstallServer.CheckedChanged += new System.EventHandler(this.rdoInstallServer_CheckedChanged);
            // 
            // grpInstalledServerComponents
            // 
            this.grpInstalledServerComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInstalledServerComponents.Controls.Add(this.lblTasks);
            this.grpInstalledServerComponents.Controls.Add(this.cboTasks);
            this.grpInstalledServerComponents.Controls.Add(this.lstInstalledServices);
            this.grpInstalledServerComponents.Enabled = false;
            this.grpInstalledServerComponents.Location = new System.Drawing.Point(7, 258);
            this.grpInstalledServerComponents.Name = "grpInstalledServerComponents";
            this.grpInstalledServerComponents.Size = new System.Drawing.Size(665, 172);
            this.grpInstalledServerComponents.TabIndex = 1;
            this.grpInstalledServerComponents.TabStop = false;
            // 
            // lblTasks
            // 
            this.lblTasks.AutoSize = true;
            this.lblTasks.Location = new System.Drawing.Point(407, 21);
            this.lblTasks.Name = "lblTasks";
            this.lblTasks.Size = new System.Drawing.Size(34, 13);
            this.lblTasks.TabIndex = 9;
            this.lblTasks.Text = "Task:";
            // 
            // cboTasks
            // 
            this.cboTasks.FormattingEnabled = true;
            this.cboTasks.Items.AddRange(new object[] {
            "Upgrade",
            "Uninstall",
            "Configure"});
            this.cboTasks.Location = new System.Drawing.Point(447, 17);
            this.cboTasks.Name = "cboTasks";
            this.cboTasks.Size = new System.Drawing.Size(206, 21);
            this.cboTasks.TabIndex = 8;
            this.cboTasks.Text = "Upgrade";
            this.cboTasks.SelectedIndexChanged += new System.EventHandler(this.cboTasks_SelectedIndexChanged);
            // 
            // lstInstalledServices
            // 
            this.lstInstalledServices.AllowDrop = true;
            this.lstInstalledServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInstalledServices.FormattingEnabled = true;
            this.lstInstalledServices.IntegralHeight = false;
            this.lstInstalledServices.Location = new System.Drawing.Point(47, 44);
            this.lstInstalledServices.Name = "lstInstalledServices";
            this.lstInstalledServices.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstInstalledServices.Size = new System.Drawing.Size(606, 122);
            this.lstInstalledServices.TabIndex = 7;
            this.lstInstalledServices.SelectedIndexChanged += new System.EventHandler(this.lstInstalledServices_SelectedIndexChanged);
            this.lstInstalledServices.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstInstalledServices_KeyDown);
            // 
            // rdoInstalledServerTasks
            // 
            this.rdoInstalledServerTasks.AutoSize = true;
            this.rdoInstalledServerTasks.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstalledServerTasks.Location = new System.Drawing.Point(16, 257);
            this.rdoInstalledServerTasks.Name = "rdoInstalledServerTasks";
            this.rdoInstalledServerTasks.Size = new System.Drawing.Size(187, 17);
            this.rdoInstalledServerTasks.TabIndex = 8;
            this.rdoInstalledServerTasks.Text = "Installed Server Component Tasks";
            this.rdoInstalledServerTasks.UseVisualStyleBackColor = true;
            this.rdoInstalledServerTasks.CheckedChanged += new System.EventHandler(this.rdoInstalledServerTasks_CheckedChanged);
            // 
            // grpClientConfiguration
            // 
            this.grpClientConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpClientConfiguration.Controls.Add(this.btnConfigurationInstallFolder);
            this.grpClientConfiguration.Controls.Add(this.txtConfigurationInstallFolder);
            this.grpClientConfiguration.Controls.Add(this.lblInstallDir2);
            this.grpClientConfiguration.Enabled = false;
            this.grpClientConfiguration.Location = new System.Drawing.Point(7, 188);
            this.grpClientConfiguration.Name = "grpClientConfiguration";
            this.grpClientConfiguration.Size = new System.Drawing.Size(665, 61);
            this.grpClientConfiguration.TabIndex = 3;
            this.grpClientConfiguration.TabStop = false;
            // 
            // btnConfigurationInstallFolder
            // 
            this.btnConfigurationInstallFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigurationInstallFolder.Location = new System.Drawing.Point(551, 23);
            this.btnConfigurationInstallFolder.Name = "btnConfigurationInstallFolder";
            this.btnConfigurationInstallFolder.Size = new System.Drawing.Size(102, 23);
            this.btnConfigurationInstallFolder.TabIndex = 16;
            this.btnConfigurationInstallFolder.Text = "Change Directory";
            this.btnConfigurationInstallFolder.UseVisualStyleBackColor = true;
            this.btnConfigurationInstallFolder.Click += new System.EventHandler(this.btnConfigurationInstallFolder_Click);
            // 
            // txtConfigurationInstallFolder
            // 
            this.txtConfigurationInstallFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConfigurationInstallFolder.Location = new System.Drawing.Point(178, 25);
            this.txtConfigurationInstallFolder.Name = "txtConfigurationInstallFolder";
            this.txtConfigurationInstallFolder.ReadOnly = true;
            this.txtConfigurationInstallFolder.Size = new System.Drawing.Size(368, 20);
            this.txtConfigurationInstallFolder.TabIndex = 15;
            this.txtConfigurationInstallFolder.Text = "C:\\MIDClientConfiguration";
            // 
            // lblInstallDir2
            // 
            this.lblInstallDir2.AutoSize = true;
            this.lblInstallDir2.Location = new System.Drawing.Point(44, 28);
            this.lblInstallDir2.Name = "lblInstallDir2";
            this.lblInstallDir2.Size = new System.Drawing.Size(118, 13);
            this.lblInstallDir2.TabIndex = 14;
            this.lblInstallDir2.Text = "Choose install directory:";
            // 
            // rdoInstallConfiguration
            // 
            this.rdoInstallConfiguration.AutoSize = true;
            this.rdoInstallConfiguration.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstallConfiguration.Location = new System.Drawing.Point(15, 186);
            this.rdoInstallConfiguration.Name = "rdoInstallConfiguration";
            this.rdoInstallConfiguration.Size = new System.Drawing.Size(117, 17);
            this.rdoInstallConfiguration.TabIndex = 17;
            this.rdoInstallConfiguration.Text = "Install Configuration";
            this.rdoInstallConfiguration.UseVisualStyleBackColor = true;
            this.rdoInstallConfiguration.CheckedChanged += new System.EventHandler(this.rdoInstallConfiguration_CheckedChanged);
            // 
            // rdoInstallTypical
            // 
            this.rdoInstallTypical.AutoSize = true;
            this.rdoInstallTypical.ForeColor = System.Drawing.Color.MediumBlue;
            this.rdoInstallTypical.Location = new System.Drawing.Point(16, 12);
            this.rdoInstallTypical.Name = "rdoInstallTypical";
            this.rdoInstallTypical.Size = new System.Drawing.Size(123, 17);
            this.rdoInstallTypical.TabIndex = 4;
            this.rdoInstallTypical.Text = "Typical Server Install";
            this.rdoInstallTypical.UseVisualStyleBackColor = true;
            this.rdoInstallTypical.CheckedChanged += new System.EventHandler(this.rdoInstallTypical_CheckedChanged);
            // 
            // ucServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoInstalledServerTasks);
            this.Controls.Add(this.rdoInstallConfiguration);
            this.Controls.Add(this.rdoInstallTypical);
            this.Controls.Add(this.rdoInstallServer);
            this.Controls.Add(this.grpClientConfiguration);
            this.Controls.Add(this.grpInstalledServerComponents);
            this.Controls.Add(this.grpServer);
            this.Name = "ucServer";
            this.Size = new System.Drawing.Size(680, 435);
            this.Load += new System.EventHandler(this.ucServer_Load);
            this.VisibleChanged += new System.EventHandler(this.ucServer_VisibleChanged);
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            this.gbxConfigureUsing.ResumeLayout(false);
            this.gbxConfigureUsing.PerformLayout();
            this.gbxStartType.ResumeLayout(false);
            this.gbxStartType.PerformLayout();
            this.grpInstalledServerComponents.ResumeLayout(false);
            this.grpInstalledServerComponents.PerformLayout();
            this.grpClientConfiguration.ResumeLayout(false);
            this.grpClientConfiguration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.CheckedListBox clstServices;
        private System.Windows.Forms.Label lblInstallDir;
        private System.Windows.Forms.Button btnInstallFolder;
        public System.Windows.Forms.TextBox txtInstallFolder;
        private System.Windows.Forms.FolderBrowserDialog folderInstallFolder;
        private System.Windows.Forms.GroupBox grpInstalledServerComponents;
        private System.Windows.Forms.Label lblTasks;
        public MIDRetail.Windows.Controls.MIDComboBoxEnh cboTasks;
        public System.Windows.Forms.ListBox lstInstalledServices;
        private System.Windows.Forms.RadioButton rdoInstallServer;
        public System.Windows.Forms.RadioButton rdoInstalledServerTasks;
        private System.Windows.Forms.GroupBox gbxStartType;
        private System.Windows.Forms.RadioButton rdoStartTypeAuto;
        private System.Windows.Forms.RadioButton rdoStartTypeManual;
        private System.Windows.Forms.GroupBox grpClientConfiguration;
        private System.Windows.Forms.Button btnConfigurationInstallFolder;
        public System.Windows.Forms.TextBox txtConfigurationInstallFolder;
        private System.Windows.Forms.Label lblInstallDir2;
        private System.Windows.Forms.RadioButton rdoInstallConfiguration;
        private System.Windows.Forms.GroupBox gbxConfigureUsing;
        private System.Windows.Forms.RadioButton rdoUseIPAddress;
        private System.Windows.Forms.RadioButton rdoUseMachineName;
        private System.Windows.Forms.RadioButton rdoInstallTypical;
    }
}
