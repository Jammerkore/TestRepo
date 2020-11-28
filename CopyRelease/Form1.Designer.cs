namespace MIDRetail.CopyRelease
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fbdFilePath = new System.Windows.Forms.FolderBrowserDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnQABuildPath = new System.Windows.Forms.Button();
            this.btnQAReleasePath = new System.Windows.Forms.Button();
            this.txtQABuildPath = new System.Windows.Forms.TextBox();
            this.txtQAReleasePath = new System.Windows.Forms.TextBox();
            this.txtQAFolderName = new System.Windows.Forms.TextBox();
            this.lblQAFolderName = new System.Windows.Forms.Label();
            this.radCreateRelease = new System.Windows.Forms.RadioButton();
            this.radQA = new System.Windows.Forms.RadioButton();
            this.txtQADocumentationBranch = new System.Windows.Forms.TextBox();
            this.lblClient = new System.Windows.Forms.Label();
            this.cboClient = new System.Windows.Forms.ComboBox();
            this.lblProdVersion = new System.Windows.Forms.Label();
            this.radPackageToFTP = new System.Windows.Forms.RadioButton();
            this.pnlQABuild = new System.Windows.Forms.Panel();
            this.lblQASMBBranchName = new System.Windows.Forms.Label();
            this.lblQASMBBranch = new System.Windows.Forms.Label();
            this.lblQASCMRepositoryName = new System.Windows.Forms.Label();
            this.lblQASCMRepository = new System.Windows.Forms.Label();
            this.btnQACalcFile = new System.Windows.Forms.Button();
            this.txtQACalcFile = new System.Windows.Forms.TextBox();
            this.lblQADocumentationBranch = new System.Windows.Forms.Label();
            this.cbxQACalcOnly = new System.Windows.Forms.CheckBox();
            this.pnlFTP = new System.Windows.Forms.Panel();
            this.txtFTPZipFileName = new System.Windows.Forms.TextBox();
            this.lblFTPZipFileName = new System.Windows.Forms.Label();
            this.btnFTPReleasePath = new System.Windows.Forms.Button();
            this.btnFTPPath = new System.Windows.Forms.Button();
            this.txtFTPReleasePath = new System.Windows.Forms.TextBox();
            this.txtFTPPath = new System.Windows.Forms.TextBox();
            this.pnlRelease = new System.Windows.Forms.Panel();
            this.btnRLQAPath = new System.Windows.Forms.Button();
            this.btnRLReleasePath = new System.Windows.Forms.Button();
            this.txtRLQAPath = new System.Windows.Forms.TextBox();
            this.txtRLReleasePath = new System.Windows.Forms.TextBox();
            this.txtRLFolderName = new System.Windows.Forms.TextBox();
            this.lblRLFolderName = new System.Windows.Forms.Label();
            this.ofdFileName = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblProdVersionNumber = new System.Windows.Forms.Label();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.cbxAllocation = new System.Windows.Forms.CheckBox();
            this.cbxPlanning = new System.Windows.Forms.CheckBox();
            this.cbxSize = new System.Windows.Forms.CheckBox();
            this.cbxMaster = new System.Windows.Forms.CheckBox();
            this.cbxAssortment = new System.Windows.Forms.CheckBox();
            this.cbxGroupAllocation = new System.Windows.Forms.CheckBox();
            this.gbKeys = new System.Windows.Forms.GroupBox();
            this.cbxAnalytics = new System.Windows.Forms.CheckBox();
            this.radDevelopment = new System.Windows.Forms.RadioButton();
            this.pnlQABuild.SuspendLayout();
            this.pnlFTP.SuspendLayout();
            this.pnlRelease.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.gbKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(514, 414);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(433, 414);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnQABuildPath
            // 
            this.btnQABuildPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQABuildPath.Location = new System.Drawing.Point(453, 47);
            this.btnQABuildPath.Name = "btnQABuildPath";
            this.btnQABuildPath.Size = new System.Drawing.Size(94, 23);
            this.btnQABuildPath.TabIndex = 2;
            this.btnQABuildPath.Text = "Build Path";
            this.btnQABuildPath.UseVisualStyleBackColor = true;
            this.btnQABuildPath.Click += new System.EventHandler(this.btnQABuildPath_Click);
            this.btnQABuildPath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // btnQAReleasePath
            // 
            this.btnQAReleasePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQAReleasePath.Location = new System.Drawing.Point(453, 86);
            this.btnQAReleasePath.Name = "btnQAReleasePath";
            this.btnQAReleasePath.Size = new System.Drawing.Size(94, 23);
            this.btnQAReleasePath.TabIndex = 3;
            this.btnQAReleasePath.Text = "QA Path";
            this.btnQAReleasePath.UseVisualStyleBackColor = true;
            this.btnQAReleasePath.Click += new System.EventHandler(this.btnQAReleasePath_Click);
            this.btnQAReleasePath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtQABuildPath
            // 
            this.txtQABuildPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQABuildPath.Location = new System.Drawing.Point(17, 50);
            this.txtQABuildPath.Name = "txtQABuildPath";
            this.txtQABuildPath.Size = new System.Drawing.Size(430, 20);
            this.txtQABuildPath.TabIndex = 4;
            this.txtQABuildPath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtQAReleasePath
            // 
            this.txtQAReleasePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQAReleasePath.Location = new System.Drawing.Point(17, 87);
            this.txtQAReleasePath.Name = "txtQAReleasePath";
            this.txtQAReleasePath.Size = new System.Drawing.Size(430, 20);
            this.txtQAReleasePath.TabIndex = 5;
            this.txtQAReleasePath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtQAFolderName
            // 
            this.txtQAFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQAFolderName.Location = new System.Drawing.Point(90, 201);
            this.txtQAFolderName.Name = "txtQAFolderName";
            this.txtQAFolderName.Size = new System.Drawing.Size(457, 20);
            this.txtQAFolderName.TabIndex = 6;
            this.txtQAFolderName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // lblQAFolderName
            // 
            this.lblQAFolderName.AutoSize = true;
            this.lblQAFolderName.Location = new System.Drawing.Point(14, 202);
            this.lblQAFolderName.Name = "lblQAFolderName";
            this.lblQAFolderName.Size = new System.Drawing.Size(70, 13);
            this.lblQAFolderName.TabIndex = 7;
            this.lblQAFolderName.Text = "Folder Name:";
            this.lblQAFolderName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // radCreateRelease
            // 
            this.radCreateRelease.AutoSize = true;
            this.radCreateRelease.Location = new System.Drawing.Point(34, 85);
            this.radCreateRelease.Name = "radCreateRelease";
            this.radCreateRelease.Size = new System.Drawing.Size(98, 17);
            this.radCreateRelease.TabIndex = 8;
            this.radCreateRelease.Text = "Create Release";
            this.radCreateRelease.UseVisualStyleBackColor = true;
            this.radCreateRelease.CheckedChanged += new System.EventHandler(this.radCreateRelease_CheckedChanged);
            this.radCreateRelease.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // radQA
            // 
            this.radQA.AutoSize = true;
            this.radQA.Location = new System.Drawing.Point(196, 61);
            this.radQA.Name = "radQA";
            this.radQA.Size = new System.Drawing.Size(66, 17);
            this.radQA.TabIndex = 9;
            this.radQA.Text = "QA Build";
            this.radQA.UseVisualStyleBackColor = true;
            this.radQA.CheckedChanged += new System.EventHandler(this.radQA_CheckedChanged);
            this.radQA.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtQADocumentationBranch
            // 
            this.txtQADocumentationBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQADocumentationBranch.Location = new System.Drawing.Point(90, 161);
            this.txtQADocumentationBranch.Name = "txtQADocumentationBranch";
            this.txtQADocumentationBranch.Size = new System.Drawing.Size(457, 20);
            this.txtQADocumentationBranch.TabIndex = 10;
            this.txtQADocumentationBranch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(12, 26);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(36, 13);
            this.lblClient.TabIndex = 12;
            this.lblClient.Text = "Client:";
            // 
            // cboClient
            // 
            this.cboClient.FormattingEnabled = true;
            this.cboClient.Location = new System.Drawing.Point(54, 23);
            this.cboClient.Name = "cboClient";
            this.cboClient.Size = new System.Drawing.Size(205, 21);
            this.cboClient.TabIndex = 13;
            this.cboClient.SelectedIndexChanged += new System.EventHandler(this.cboClient_SelectedIndexChanged);
            this.cboClient.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // lblProdVersion
            // 
            this.lblProdVersion.AutoSize = true;
            this.lblProdVersion.Location = new System.Drawing.Point(272, 30);
            this.lblProdVersion.Name = "lblProdVersion";
            this.lblProdVersion.Size = new System.Drawing.Size(102, 13);
            this.lblProdVersion.TabIndex = 14;
            this.lblProdVersion.Text = "Production Version: ";
            // 
            // radPackageToFTP
            // 
            this.radPackageToFTP.AutoSize = true;
            this.radPackageToFTP.Location = new System.Drawing.Point(196, 85);
            this.radPackageToFTP.Name = "radPackageToFTP";
            this.radPackageToFTP.Size = new System.Drawing.Size(128, 17);
            this.radPackageToFTP.TabIndex = 15;
            this.radPackageToFTP.TabStop = true;
            this.radPackageToFTP.Text = "Package To FTP Site";
            this.radPackageToFTP.UseVisualStyleBackColor = true;
            this.radPackageToFTP.CheckedChanged += new System.EventHandler(this.radPackageToFTP_CheckedChanged);
            this.radPackageToFTP.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // pnlQABuild
            // 
            this.pnlQABuild.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlQABuild.Controls.Add(this.lblQASMBBranchName);
            this.pnlQABuild.Controls.Add(this.lblQASMBBranch);
            this.pnlQABuild.Controls.Add(this.lblQASCMRepositoryName);
            this.pnlQABuild.Controls.Add(this.lblQASCMRepository);
            this.pnlQABuild.Controls.Add(this.btnQACalcFile);
            this.pnlQABuild.Controls.Add(this.txtQACalcFile);
            this.pnlQABuild.Controls.Add(this.lblQADocumentationBranch);
            this.pnlQABuild.Controls.Add(this.cbxQACalcOnly);
            this.pnlQABuild.Controls.Add(this.btnQABuildPath);
            this.pnlQABuild.Controls.Add(this.btnQAReleasePath);
            this.pnlQABuild.Controls.Add(this.txtQABuildPath);
            this.pnlQABuild.Controls.Add(this.txtQAReleasePath);
            this.pnlQABuild.Controls.Add(this.txtQADocumentationBranch);
            this.pnlQABuild.Controls.Add(this.txtQAFolderName);
            this.pnlQABuild.Controls.Add(this.lblQAFolderName);
            this.pnlQABuild.Location = new System.Drawing.Point(12, 151);
            this.pnlQABuild.Name = "pnlQABuild";
            this.pnlQABuild.Size = new System.Drawing.Size(564, 245);
            this.pnlQABuild.TabIndex = 16;
            // 
            // lblQASMBBranchName
            // 
            this.lblQASMBBranchName.AutoSize = true;
            this.lblQASMBBranchName.Location = new System.Drawing.Point(372, 17);
            this.lblQASMBBranchName.Name = "lblQASMBBranchName";
            this.lblQASMBBranchName.Size = new System.Drawing.Size(64, 13);
            this.lblQASMBBranchName.TabIndex = 23;
            this.lblQASMBBranchName.Text = "SMBBranch";
            // 
            // lblQASMBBranch
            // 
            this.lblQASMBBranch.AutoSize = true;
            this.lblQASMBBranch.Location = new System.Drawing.Point(328, 17);
            this.lblQASMBBranch.Name = "lblQASMBBranch";
            this.lblQASMBBranch.Size = new System.Drawing.Size(44, 13);
            this.lblQASMBBranch.TabIndex = 22;
            this.lblQASMBBranch.Text = "Branch:";
            // 
            // lblQASCMRepositoryName
            // 
            this.lblQASCMRepositoryName.AutoSize = true;
            this.lblQASCMRepositoryName.Location = new System.Drawing.Point(232, 17);
            this.lblQASCMRepositoryName.Name = "lblQASCMRepositoryName";
            this.lblQASCMRepositoryName.Size = new System.Drawing.Size(80, 13);
            this.lblQASCMRepositoryName.TabIndex = 21;
            this.lblQASCMRepositoryName.Text = "SCMRepository";
            // 
            // lblQASCMRepository
            // 
            this.lblQASCMRepository.AutoSize = true;
            this.lblQASCMRepository.Location = new System.Drawing.Point(145, 17);
            this.lblQASCMRepository.Name = "lblQASCMRepository";
            this.lblQASCMRepository.Size = new System.Drawing.Size(86, 13);
            this.lblQASCMRepository.TabIndex = 20;
            this.lblQASCMRepository.Text = "SCM Repository:";
            // 
            // btnQACalcFile
            // 
            this.btnQACalcFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQACalcFile.Location = new System.Drawing.Point(453, 125);
            this.btnQACalcFile.Name = "btnQACalcFile";
            this.btnQACalcFile.Size = new System.Drawing.Size(94, 23);
            this.btnQACalcFile.TabIndex = 15;
            this.btnQACalcFile.Text = "Calc File";
            this.btnQACalcFile.UseVisualStyleBackColor = true;
            this.btnQACalcFile.Click += new System.EventHandler(this.btnQACalcFile_Click);
            this.btnQACalcFile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtQACalcFile
            // 
            this.txtQACalcFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQACalcFile.Location = new System.Drawing.Point(17, 125);
            this.txtQACalcFile.Name = "txtQACalcFile";
            this.txtQACalcFile.Size = new System.Drawing.Size(430, 20);
            this.txtQACalcFile.TabIndex = 14;
            this.txtQACalcFile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // lblQADocumentationBranch
            // 
            this.lblQADocumentationBranch.AutoSize = true;
            this.lblQADocumentationBranch.Location = new System.Drawing.Point(14, 165);
            this.lblQADocumentationBranch.Name = "lblQADocumentationBranch";
            this.lblQADocumentationBranch.Size = new System.Drawing.Size(67, 13);
            this.lblQADocumentationBranch.TabIndex = 13;
            this.lblQADocumentationBranch.Text = "Doc Branch:";
            this.lblQADocumentationBranch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // cbxQACalcOnly
            // 
            this.cbxQACalcOnly.AutoSize = true;
            this.cbxQACalcOnly.Location = new System.Drawing.Point(17, 15);
            this.cbxQACalcOnly.Name = "cbxQACalcOnly";
            this.cbxQACalcOnly.Size = new System.Drawing.Size(102, 17);
            this.cbxQACalcOnly.TabIndex = 12;
            this.cbxQACalcOnly.Text = "Calcs Only Build";
            this.cbxQACalcOnly.UseVisualStyleBackColor = true;
            this.cbxQACalcOnly.CheckedChanged += new System.EventHandler(this.cbxQACalcOnly_CheckedChanged);
            this.cbxQACalcOnly.MouseHover += new System.EventHandler(this.object_MouseHover);
            this.cbxQACalcOnly.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // pnlFTP
            // 
            this.pnlFTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFTP.Controls.Add(this.txtFTPZipFileName);
            this.pnlFTP.Controls.Add(this.lblFTPZipFileName);
            this.pnlFTP.Controls.Add(this.btnFTPReleasePath);
            this.pnlFTP.Controls.Add(this.btnFTPPath);
            this.pnlFTP.Controls.Add(this.txtFTPReleasePath);
            this.pnlFTP.Controls.Add(this.txtFTPPath);
            this.pnlFTP.Location = new System.Drawing.Point(11, 150);
            this.pnlFTP.Name = "pnlFTP";
            this.pnlFTP.Size = new System.Drawing.Size(564, 242);
            this.pnlFTP.TabIndex = 18;
            // 
            // txtFTPZipFileName
            // 
            this.txtFTPZipFileName.Location = new System.Drawing.Point(98, 142);
            this.txtFTPZipFileName.Name = "txtFTPZipFileName";
            this.txtFTPZipFileName.Size = new System.Drawing.Size(349, 20);
            this.txtFTPZipFileName.TabIndex = 7;
            // 
            // lblFTPZipFileName
            // 
            this.lblFTPZipFileName.AutoSize = true;
            this.lblFTPZipFileName.Location = new System.Drawing.Point(17, 145);
            this.lblFTPZipFileName.Name = "lblFTPZipFileName";
            this.lblFTPZipFileName.Size = new System.Drawing.Size(75, 13);
            this.lblFTPZipFileName.TabIndex = 6;
            this.lblFTPZipFileName.Text = "Zip File Name:";
            // 
            // btnFTPReleasePath
            // 
            this.btnFTPReleasePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFTPReleasePath.Location = new System.Drawing.Point(453, 61);
            this.btnFTPReleasePath.Name = "btnFTPReleasePath";
            this.btnFTPReleasePath.Size = new System.Drawing.Size(94, 23);
            this.btnFTPReleasePath.TabIndex = 2;
            this.btnFTPReleasePath.Text = "Release Path";
            this.btnFTPReleasePath.UseVisualStyleBackColor = true;
            this.btnFTPReleasePath.Click += new System.EventHandler(this.btnFTPReleasePath_Click);
            this.btnFTPReleasePath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // btnFTPPath
            // 
            this.btnFTPPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFTPPath.Location = new System.Drawing.Point(453, 100);
            this.btnFTPPath.Name = "btnFTPPath";
            this.btnFTPPath.Size = new System.Drawing.Size(94, 23);
            this.btnFTPPath.TabIndex = 3;
            this.btnFTPPath.Text = "FTP Path";
            this.btnFTPPath.UseVisualStyleBackColor = true;
            this.btnFTPPath.Click += new System.EventHandler(this.btnFTPPath_Click);
            this.btnFTPPath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtFTPReleasePath
            // 
            this.txtFTPReleasePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFTPReleasePath.Location = new System.Drawing.Point(17, 61);
            this.txtFTPReleasePath.Name = "txtFTPReleasePath";
            this.txtFTPReleasePath.Size = new System.Drawing.Size(430, 20);
            this.txtFTPReleasePath.TabIndex = 4;
            this.txtFTPReleasePath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtFTPPath
            // 
            this.txtFTPPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFTPPath.Location = new System.Drawing.Point(17, 100);
            this.txtFTPPath.Name = "txtFTPPath";
            this.txtFTPPath.Size = new System.Drawing.Size(430, 20);
            this.txtFTPPath.TabIndex = 5;
            this.txtFTPPath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // pnlRelease
            // 
            this.pnlRelease.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRelease.Controls.Add(this.btnRLQAPath);
            this.pnlRelease.Controls.Add(this.btnRLReleasePath);
            this.pnlRelease.Controls.Add(this.txtRLQAPath);
            this.pnlRelease.Controls.Add(this.txtRLReleasePath);
            this.pnlRelease.Controls.Add(this.txtRLFolderName);
            this.pnlRelease.Controls.Add(this.lblRLFolderName);
            this.pnlRelease.Location = new System.Drawing.Point(15, 150);
            this.pnlRelease.Name = "pnlRelease";
            this.pnlRelease.Size = new System.Drawing.Size(564, 242);
            this.pnlRelease.TabIndex = 17;
            // 
            // btnRLQAPath
            // 
            this.btnRLQAPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRLQAPath.Location = new System.Drawing.Point(450, 61);
            this.btnRLQAPath.Name = "btnRLQAPath";
            this.btnRLQAPath.Size = new System.Drawing.Size(94, 23);
            this.btnRLQAPath.TabIndex = 2;
            this.btnRLQAPath.Text = "QA Path";
            this.btnRLQAPath.UseVisualStyleBackColor = true;
            this.btnRLQAPath.Click += new System.EventHandler(this.btnRLQAPath_Click);
            this.btnRLQAPath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // btnRLReleasePath
            // 
            this.btnRLReleasePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRLReleasePath.Location = new System.Drawing.Point(450, 101);
            this.btnRLReleasePath.Name = "btnRLReleasePath";
            this.btnRLReleasePath.Size = new System.Drawing.Size(94, 23);
            this.btnRLReleasePath.TabIndex = 3;
            this.btnRLReleasePath.Text = "Release Path";
            this.btnRLReleasePath.UseVisualStyleBackColor = true;
            this.btnRLReleasePath.Click += new System.EventHandler(this.btnRLReleasePath_Click);
            this.btnRLReleasePath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtRLQAPath
            // 
            this.txtRLQAPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRLQAPath.Location = new System.Drawing.Point(17, 61);
            this.txtRLQAPath.Name = "txtRLQAPath";
            this.txtRLQAPath.Size = new System.Drawing.Size(427, 20);
            this.txtRLQAPath.TabIndex = 4;
            this.txtRLQAPath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtRLReleasePath
            // 
            this.txtRLReleasePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRLReleasePath.Location = new System.Drawing.Point(17, 100);
            this.txtRLReleasePath.Name = "txtRLReleasePath";
            this.txtRLReleasePath.Size = new System.Drawing.Size(427, 20);
            this.txtRLReleasePath.TabIndex = 5;
            this.txtRLReleasePath.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // txtRLFolderName
            // 
            this.txtRLFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRLFolderName.Location = new System.Drawing.Point(100, 199);
            this.txtRLFolderName.Name = "txtRLFolderName";
            this.txtRLFolderName.Size = new System.Drawing.Size(444, 20);
            this.txtRLFolderName.TabIndex = 6;
            this.txtRLFolderName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // lblRLFolderName
            // 
            this.lblRLFolderName.AutoSize = true;
            this.lblRLFolderName.Location = new System.Drawing.Point(14, 202);
            this.lblRLFolderName.Name = "lblRLFolderName";
            this.lblRLFolderName.Size = new System.Drawing.Size(70, 13);
            this.lblRLFolderName.TabIndex = 7;
            this.lblRLFolderName.Text = "Folder Name:";
            this.lblRLFolderName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.object_MouseMove);
            // 
            // ofdFileName
            // 
            this.ofdFileName.FileName = "ofdFileName";
            // 
            // lblProdVersionNumber
            // 
            this.lblProdVersionNumber.AutoSize = true;
            this.lblProdVersionNumber.Location = new System.Drawing.Point(373, 30);
            this.lblProdVersionNumber.Name = "lblProdVersionNumber";
            this.lblProdVersionNumber.Size = new System.Drawing.Size(43, 13);
            this.lblProdVersionNumber.TabIndex = 19;
            this.lblProdVersionNumber.Text = "x.x.xxxx";
            // 
            // ssStatus
            // 
            this.ssStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ssStatus.AutoSize = false;
            this.ssStatus.Dock = System.Windows.Forms.DockStyle.None;
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.ssStatus.Location = new System.Drawing.Point(8, 414);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(409, 22);
            this.ssStatus.TabIndex = 20;
            this.ssStatus.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(112, 17);
            this.toolStripStatusLabel.Text = "toolStripStatusLabel";
            // 
            // cbxAllocation
            // 
            this.cbxAllocation.AutoSize = true;
            this.cbxAllocation.Location = new System.Drawing.Point(421, 32);
            this.cbxAllocation.Name = "cbxAllocation";
            this.cbxAllocation.Size = new System.Drawing.Size(72, 17);
            this.cbxAllocation.TabIndex = 21;
            this.cbxAllocation.Text = "Allocation";
            this.cbxAllocation.UseVisualStyleBackColor = true;
            this.cbxAllocation.CheckedChanged += new System.EventHandler(this.cbxAllocation_CheckedChanged);
            // 
            // cbxPlanning
            // 
            this.cbxPlanning.AutoSize = true;
            this.cbxPlanning.Location = new System.Drawing.Point(421, 55);
            this.cbxPlanning.Name = "cbxPlanning";
            this.cbxPlanning.Size = new System.Drawing.Size(67, 17);
            this.cbxPlanning.TabIndex = 22;
            this.cbxPlanning.Text = "Planning";
            this.cbxPlanning.UseVisualStyleBackColor = true;
            this.cbxPlanning.CheckedChanged += new System.EventHandler(this.cbxPlanning_CheckedChanged);
            // 
            // cbxSize
            // 
            this.cbxSize.AutoSize = true;
            this.cbxSize.Location = new System.Drawing.Point(421, 78);
            this.cbxSize.Name = "cbxSize";
            this.cbxSize.Size = new System.Drawing.Size(46, 17);
            this.cbxSize.TabIndex = 23;
            this.cbxSize.Text = "Size";
            this.cbxSize.UseVisualStyleBackColor = true;
            this.cbxSize.CheckedChanged += new System.EventHandler(this.cbxSize_CheckedChanged);
            // 
            // cbxMaster
            // 
            this.cbxMaster.AutoSize = true;
            this.cbxMaster.Location = new System.Drawing.Point(508, 32);
            this.cbxMaster.Name = "cbxMaster";
            this.cbxMaster.Size = new System.Drawing.Size(58, 17);
            this.cbxMaster.TabIndex = 24;
            this.cbxMaster.Text = "Master";
            this.cbxMaster.UseVisualStyleBackColor = true;
            this.cbxMaster.CheckedChanged += new System.EventHandler(this.cbxMaster_CheckedChanged);
            // 
            // cbxAssortment
            // 
            this.cbxAssortment.AutoSize = true;
            this.cbxAssortment.Location = new System.Drawing.Point(508, 55);
            this.cbxAssortment.Name = "cbxAssortment";
            this.cbxAssortment.Size = new System.Drawing.Size(78, 17);
            this.cbxAssortment.TabIndex = 25;
            this.cbxAssortment.Text = "Assortment";
            this.cbxAssortment.UseVisualStyleBackColor = true;
            this.cbxAssortment.CheckedChanged += new System.EventHandler(this.cbxAssortment_CheckedChanged);
            // 
            // cbxGroupAllocation
            // 
            this.cbxGroupAllocation.AutoSize = true;
            this.cbxGroupAllocation.Location = new System.Drawing.Point(482, 78);
            this.cbxGroupAllocation.Name = "cbxGroupAllocation";
            this.cbxGroupAllocation.Size = new System.Drawing.Size(104, 17);
            this.cbxGroupAllocation.TabIndex = 26;
            this.cbxGroupAllocation.Text = "Group Allocation";
            this.cbxGroupAllocation.UseVisualStyleBackColor = true;
            this.cbxGroupAllocation.CheckedChanged += new System.EventHandler(this.cbxGroupAllocation_CheckedChanged);
            // 
            // gbKeys
            // 
            this.gbKeys.Controls.Add(this.cbxAnalytics);
            this.gbKeys.Location = new System.Drawing.Point(412, 7);
            this.gbKeys.Name = "gbKeys";
            this.gbKeys.Size = new System.Drawing.Size(177, 125);
            this.gbKeys.TabIndex = 27;
            this.gbKeys.TabStop = false;
            this.gbKeys.Text = "License Keys";
            // 
            // cbxAnalytics
            // 
            this.cbxAnalytics.AutoSize = true;
            this.cbxAnalytics.Location = new System.Drawing.Point(10, 95);
            this.cbxAnalytics.Name = "cbxAnalytics";
            this.cbxAnalytics.Size = new System.Drawing.Size(68, 17);
            this.cbxAnalytics.TabIndex = 27;
            this.cbxAnalytics.Text = "Analytics";
            this.cbxAnalytics.UseVisualStyleBackColor = true;
            this.cbxAnalytics.CheckedChanged += new System.EventHandler(this.cbxAnalytics_CheckedChanged);
            // 
            // radDevelopment
            // 
            this.radDevelopment.AutoSize = true;
            this.radDevelopment.Location = new System.Drawing.Point(34, 62);
            this.radDevelopment.Name = "radDevelopment";
            this.radDevelopment.Size = new System.Drawing.Size(114, 17);
            this.radDevelopment.TabIndex = 28;
            this.radDevelopment.TabStop = true;
            this.radDevelopment.Text = "Development Build";
            this.radDevelopment.UseVisualStyleBackColor = true;
            this.radDevelopment.CheckedChanged += new System.EventHandler(this.radDevelopment_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 441);
            this.Controls.Add(this.radDevelopment);
            this.Controls.Add(this.cbxGroupAllocation);
            this.Controls.Add(this.cbxAssortment);
            this.Controls.Add(this.cbxMaster);
            this.Controls.Add(this.cbxSize);
            this.Controls.Add(this.cbxPlanning);
            this.Controls.Add(this.cbxAllocation);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.lblProdVersionNumber);
            this.Controls.Add(this.radPackageToFTP);
            this.Controls.Add(this.lblProdVersion);
            this.Controls.Add(this.cboClient);
            this.Controls.Add(this.lblClient);
            this.Controls.Add(this.radQA);
            this.Controls.Add(this.radCreateRelease);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlFTP);
            this.Controls.Add(this.pnlQABuild);
            this.Controls.Add(this.pnlRelease);
            this.Controls.Add(this.gbKeys);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy Release";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlQABuild.ResumeLayout(false);
            this.pnlQABuild.PerformLayout();
            this.pnlFTP.ResumeLayout(false);
            this.pnlFTP.PerformLayout();
            this.pnlRelease.ResumeLayout(false);
            this.pnlRelease.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.gbKeys.ResumeLayout(false);
            this.gbKeys.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbdFilePath;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnQABuildPath;
        private System.Windows.Forms.Button btnQAReleasePath;
        private System.Windows.Forms.TextBox txtQABuildPath;
        private System.Windows.Forms.TextBox txtQAReleasePath;
        private System.Windows.Forms.TextBox txtQAFolderName;
        private System.Windows.Forms.Label lblQAFolderName;
        private System.Windows.Forms.RadioButton radCreateRelease;
        private System.Windows.Forms.RadioButton radQA;
        private System.Windows.Forms.TextBox txtQADocumentationBranch;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cboClient;
        private System.Windows.Forms.Label lblProdVersion;
        private System.Windows.Forms.RadioButton radPackageToFTP;
        private System.Windows.Forms.Panel pnlQABuild;
        private System.Windows.Forms.CheckBox cbxQACalcOnly;
        private System.Windows.Forms.Panel pnlRelease;
        private System.Windows.Forms.Button btnRLQAPath;
        private System.Windows.Forms.Button btnRLReleasePath;
        private System.Windows.Forms.TextBox txtRLQAPath;
        private System.Windows.Forms.TextBox txtRLReleasePath;
        private System.Windows.Forms.TextBox txtRLFolderName;
        private System.Windows.Forms.Label lblRLFolderName;
        private System.Windows.Forms.Panel pnlFTP;
        private System.Windows.Forms.Button btnFTPReleasePath;
        private System.Windows.Forms.Button btnFTPPath;
        private System.Windows.Forms.TextBox txtFTPReleasePath;
        private System.Windows.Forms.TextBox txtFTPPath;
        private System.Windows.Forms.Label lblQADocumentationBranch;
        private System.Windows.Forms.Button btnQACalcFile;
        private System.Windows.Forms.TextBox txtQACalcFile;
        private System.Windows.Forms.OpenFileDialog ofdFileName;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblProdVersionNumber;
        private System.Windows.Forms.Label lblQASMBBranch;
        private System.Windows.Forms.Label lblQASCMRepositoryName;
        private System.Windows.Forms.Label lblQASCMRepository;
        private System.Windows.Forms.Label lblQASMBBranchName;
        private System.Windows.Forms.TextBox txtFTPZipFileName;
        private System.Windows.Forms.Label lblFTPZipFileName;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.CheckBox cbxAllocation;
        private System.Windows.Forms.CheckBox cbxPlanning;
        private System.Windows.Forms.CheckBox cbxSize;
        private System.Windows.Forms.CheckBox cbxMaster;
        private System.Windows.Forms.CheckBox cbxAssortment;
        private System.Windows.Forms.CheckBox cbxGroupAllocation;
        private System.Windows.Forms.GroupBox gbKeys;
        private System.Windows.Forms.RadioButton radDevelopment;
        private System.Windows.Forms.CheckBox cbxAnalytics;
    }
}

