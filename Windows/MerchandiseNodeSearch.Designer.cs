//namespace MIDRetail.Windows
//{
//    partial class MerchandiseNodeSearch
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }

//            this.splitContainer1.Panel1.DragOver -= new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragOver);
//            this.splitContainer1.Panel1.DragEnter -= new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragEnter);
//            this.splitContainer1.Panel1.DragLeave -= new System.EventHandler(this.splitContainer1_Panel1_DragLeave);
//            this.splitContainer1.Panel2.DragOver -= new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel2_DragOver);
//            this.splitContainer1.Panel2.DragEnter -= new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel2_DragEnter);
//            this.splitContainer1.Panel2.DragLeave -= new System.EventHandler(this.splitContainer1_Panel2_DragLeave);
//            this.splitContainer1.DragOver -= new System.Windows.Forms.DragEventHandler(this.splitContainer1_DragOver);
//            this.splitContainer1.DragEnter -= new System.Windows.Forms.DragEventHandler(this.splitContainer1_DragEnter);
//            this.splitContainer1.DragLeave -= new System.EventHandler(this.splitContainer1_DragLeave);
//            this.btnCharacteristics.Click -= new System.EventHandler(this.btnCharacteristics_Click);
//            this.btnSearch.Click -= new System.EventHandler(this.btnSearch_Click);
//            this.pnlCharacteristicQuery.DragOver -= new System.Windows.Forms.DragEventHandler(this.panel_DragOver);
//            this.pnlCharacteristicQuery.DragDrop -= new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//            this.pnlCharacteristicQuery.DragEnter -= new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//            this.pnlCharacteristicQuery.DragLeave -= new System.EventHandler(this.panel_DragLeave);
//            this.btnRParen.Click -= new System.EventHandler(this.toolButton_Click);
//            this.btnRParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnRParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnRParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnRParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            this.btnLParen.Click -= new System.EventHandler(this.toolButton_Click);
//            this.btnLParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnLParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnLParen.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//            this.btnLParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnOr.Click -= new System.EventHandler(this.toolButton_Click);
//            this.btnOr.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnOr.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnOr.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnOr.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            this.btnNot.Click -= new System.EventHandler(this.toolButton_Click);
//            this.btnNot.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnNot.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnNot.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//            this.btnNot.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnAnd.Click -= new System.EventHandler(this.toolButton_Click);
//            this.btnAnd.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnAnd.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnAnd.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnAnd.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//            this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
//            this.lvNodes.DragEnter -= new System.Windows.Forms.DragEventHandler(this.lvNodes_DragEnter);
//            this.lvNodes.DoubleClick -= new System.EventHandler(this.lvNodes_DoubleClick);
//            this.lvNodes.DragOver -= new System.Windows.Forms.DragEventHandler(this.lvNodes_DragOver);
//            this.lvNodes.DragLeave -= new System.EventHandler(this.lvNodes_DragLeave);
//            this.lvNodes.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.lvNodes_MouseUp);
//            this.lvNodes.ColumnClick -= new System.Windows.Forms.ColumnClickEventHandler(this.lvNodes_ColumnClick);
//            this.lvNodes.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.lvNodes_MouseMove);
//            this.lvNodes.ItemSelectionChanged -= new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvNodes_ItemSelectionChanged);
//            this.lvNodes.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.lvNodes_MouseDown);
//            this.cmsResults.Opening -= new System.ComponentModel.CancelEventHandler(this.cmsResults_Opening);
//            this.cmiSelectAll.Click -= new System.EventHandler(this.cmiSelectAll_Click);
//            this.cmiLevelSelectAll.Click -= new System.EventHandler(this.cmiLevelSelectAll_Click);
//            this.cmiLevelClearAll.Click -= new System.EventHandler(this.cmiLevelClearAll_Click);
//            this.Load -= new System.EventHandler(this.MerchandiseNodeSearch_Load);

//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
//            this.lblSearching = new System.Windows.Forms.Label();
//            this.lvLookIn = new System.Windows.Forms.ListView();
//            this.colLookInNode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
//            this.lblLookIn = new System.Windows.Forms.Label();
//            this.btnCharacteristics = new System.Windows.Forms.Button();
//            this.chkCharacteristics = new System.Windows.Forms.CheckBox();
//            this.progressBar1 = new System.Windows.Forms.ProgressBar();
//            this.gbxOptions = new System.Windows.Forms.GroupBox();
//            this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
//            this.chkMatchCase = new System.Windows.Forms.CheckBox();
//            this.clbLevel = new System.Windows.Forms.CheckedListBox();
//            this.lblLevels = new System.Windows.Forms.Label();
//            this.txtDescription = new System.Windows.Forms.TextBox();
//            this.lblDescription = new System.Windows.Forms.Label();
//            this.txtName = new System.Windows.Forms.TextBox();
//            this.lblName = new System.Windows.Forms.Label();
//            this.txtID = new System.Windows.Forms.TextBox();
//            this.lblID = new System.Windows.Forms.Label();
//            this.lblCriteria = new System.Windows.Forms.Label();
//            this.btnSearch = new System.Windows.Forms.Button();
//            this.pnlCharacteristics = new System.Windows.Forms.Panel();
//            this.pnlCharacteristicQuery = new System.Windows.Forms.Panel();
//            this.btnRParen = new System.Windows.Forms.Button();
//            this.btnLParen = new System.Windows.Forms.Button();
//            this.btnOr = new System.Windows.Forms.Button();
//            this.btnNot = new System.Windows.Forms.Button();
//            this.btnAnd = new System.Windows.Forms.Button();
//            this.btnClose = new System.Windows.Forms.Button();
//            this.btnOK = new System.Windows.Forms.Button();
//            this.pnlSearch = new System.Windows.Forms.Panel();
//            this.txtEdit = new System.Windows.Forms.TextBox();
//            this.lvNodes = new System.Windows.Forms.ListView();
//            this.colMerchandise = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
//            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
//            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
//            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
//            this.colLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
//            this.lblInstructions = new System.Windows.Forms.Label();
//            this.cmsResults = new System.Windows.Forms.ContextMenuStrip(this.components);
//            this.cmiColChooser = new System.Windows.Forms.ToolStripMenuItem();
//            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
//            this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
//            this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
//            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
//            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
//            this.cmiRename = new System.Windows.Forms.ToolStripMenuItem();
//            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
//            this.cmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
//            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
//            this.cmiLocate = new System.Windows.Forms.ToolStripMenuItem();
//            this.cmsLevels = new System.Windows.Forms.ContextMenuStrip(this.components);
//            this.cmiLevelSelectAll = new System.Windows.Forms.ToolStripMenuItem();
//            this.cmiLevelClearAll = new System.Windows.Forms.ToolStripMenuItem();
//            this.cmsCharPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
//            this.cmiCharClearAll = new System.Windows.Forms.ToolStripMenuItem();
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
//            this.splitContainer1.Panel1.SuspendLayout();
//            this.splitContainer1.Panel2.SuspendLayout();
//            this.splitContainer1.SuspendLayout();
//            this.gbxOptions.SuspendLayout();
//            this.pnlCharacteristics.SuspendLayout();
//            this.pnlSearch.SuspendLayout();
//            this.cmsResults.SuspendLayout();
//            this.cmsLevels.SuspendLayout();
//            this.cmsCharPanel.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // utmMain
//            // 
//            this.utmMain.MenuSettings.ForceSerialization = true;
//            this.utmMain.ToolbarSettings.ForceSerialization = true;
//            // 
//            // splitContainer1
//            // 
//            this.splitContainer1.AllowDrop = true;
//            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
//            this.splitContainer1.Name = "splitContainer1";
//            // 
//            // splitContainer1.Panel1
//            // 
//            this.splitContainer1.Panel1.AllowDrop = true;
//            this.splitContainer1.Panel1.Controls.Add(this.lblSearching);
//            this.splitContainer1.Panel1.Controls.Add(this.lvLookIn);
//            this.splitContainer1.Panel1.Controls.Add(this.lblLookIn);
//            this.splitContainer1.Panel1.Controls.Add(this.btnCharacteristics);
//            this.splitContainer1.Panel1.Controls.Add(this.chkCharacteristics);
//            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
//            this.splitContainer1.Panel1.Controls.Add(this.gbxOptions);
//            this.splitContainer1.Panel1.Controls.Add(this.clbLevel);
//            this.splitContainer1.Panel1.Controls.Add(this.lblLevels);
//            this.splitContainer1.Panel1.Controls.Add(this.txtDescription);
//            this.splitContainer1.Panel1.Controls.Add(this.lblDescription);
//            this.splitContainer1.Panel1.Controls.Add(this.txtName);
//            this.splitContainer1.Panel1.Controls.Add(this.lblName);
//            this.splitContainer1.Panel1.Controls.Add(this.txtID);
//            this.splitContainer1.Panel1.Controls.Add(this.lblID);
//            this.splitContainer1.Panel1.Controls.Add(this.lblCriteria);
//            this.splitContainer1.Panel1.Controls.Add(this.btnSearch);
//            this.splitContainer1.Panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragEnter);
//            this.splitContainer1.Panel1.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragOver);
//            this.splitContainer1.Panel1.DragLeave += new System.EventHandler(this.splitContainer1_Panel1_DragLeave);
//            // 
//            // splitContainer1.Panel2
//            // 
//            this.splitContainer1.Panel2.Controls.Add(this.pnlCharacteristics);
//            this.splitContainer1.Panel2.Controls.Add(this.pnlSearch);
//            this.splitContainer1.Panel2.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel2_DragEnter);
//            this.splitContainer1.Panel2.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel2_DragOver);
//            this.splitContainer1.Panel2.DragLeave += new System.EventHandler(this.splitContainer1_Panel2_DragLeave);
//            this.splitContainer1.Size = new System.Drawing.Size(878, 548);
//            this.splitContainer1.SplitterDistance = 309;
//            this.splitContainer1.TabIndex = 4;
//            this.splitContainer1.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainer1_DragEnter);
//            this.splitContainer1.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer1_DragOver);
//            this.splitContainer1.DragLeave += new System.EventHandler(this.splitContainer1_DragLeave);
//            // 
//            // lblSearching
//            // 
//            this.lblSearching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.lblSearching.Location = new System.Drawing.Point(16, 519);
//            this.lblSearching.Name = "lblSearching";
//            this.lblSearching.Size = new System.Drawing.Size(285, 23);
//            this.lblSearching.TabIndex = 16;
//            this.lblSearching.Text = "label1";
//            this.lblSearching.Visible = false;
//            // 
//            // lvLookIn
//            // 
//            this.lvLookIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.lvLookIn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//            this.colLookInNode});
//            this.lvLookIn.Location = new System.Drawing.Point(16, 59);
//            this.lvLookIn.Name = "lvLookIn";
//            this.lvLookIn.Size = new System.Drawing.Size(282, 36);
//            this.lvLookIn.TabIndex = 15;
//            this.lvLookIn.UseCompatibleStateImageBehavior = false;
//            this.lvLookIn.View = System.Windows.Forms.View.SmallIcon;
//            // 
//            // colLookInNode
//            // 
//            this.colLookInNode.Width = 160;
//            // 
//            // lblLookIn
//            // 
//            this.lblLookIn.AutoSize = true;
//            this.lblLookIn.Location = new System.Drawing.Point(13, 42);
//            this.lblLookIn.Name = "lblLookIn";
//            this.lblLookIn.Size = new System.Drawing.Size(45, 13);
//            this.lblLookIn.TabIndex = 14;
//            this.lblLookIn.Text = "Look in:";
//            // 
//            // btnCharacteristics
//            // 
//            this.btnCharacteristics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
//            this.btnCharacteristics.Location = new System.Drawing.Point(138, 373);
//            this.btnCharacteristics.Name = "btnCharacteristics";
//            this.btnCharacteristics.Size = new System.Drawing.Size(75, 23);
//            this.btnCharacteristics.TabIndex = 13;
//            this.btnCharacteristics.Text = "Edit";
//            this.btnCharacteristics.UseVisualStyleBackColor = true;
//            this.btnCharacteristics.Visible = false;
//            this.btnCharacteristics.Click += new System.EventHandler(this.btnCharacteristics_Click);
//            // 
//            // chkCharacteristics
//            // 
//            this.chkCharacteristics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
//            this.chkCharacteristics.AutoSize = true;
//            this.chkCharacteristics.Location = new System.Drawing.Point(19, 375);
//            this.chkCharacteristics.Name = "chkCharacteristics";
//            this.chkCharacteristics.Size = new System.Drawing.Size(117, 17);
//            this.chkCharacteristics.TabIndex = 12;
//            this.chkCharacteristics.Text = "Use Characteristics";
//            this.chkCharacteristics.UseVisualStyleBackColor = true;
//            this.chkCharacteristics.Visible = false;
//            // 
//            // progressBar1
//            // 
//            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.progressBar1.ForeColor = System.Drawing.Color.LimeGreen;
//            this.progressBar1.Location = new System.Drawing.Point(16, 501);
//            this.progressBar1.Name = "progressBar1";
//            this.progressBar1.Size = new System.Drawing.Size(185, 12);
//            this.progressBar1.TabIndex = 11;
//            this.progressBar1.Visible = false;
//            // 
//            // gbxOptions
//            // 
//            this.gbxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.gbxOptions.Controls.Add(this.chkMatchWholeWord);
//            this.gbxOptions.Controls.Add(this.chkMatchCase);
//            this.gbxOptions.Location = new System.Drawing.Point(19, 402);
//            this.gbxOptions.Name = "gbxOptions";
//            this.gbxOptions.Size = new System.Drawing.Size(271, 76);
//            this.gbxOptions.TabIndex = 10;
//            this.gbxOptions.TabStop = false;
//            this.gbxOptions.Text = "Search options";
//            // 
//            // chkMatchWholeWord
//            // 
//            this.chkMatchWholeWord.AutoSize = true;
//            this.chkMatchWholeWord.Location = new System.Drawing.Point(7, 44);
//            this.chkMatchWholeWord.Name = "chkMatchWholeWord";
//            this.chkMatchWholeWord.Size = new System.Drawing.Size(113, 17);
//            this.chkMatchWholeWord.TabIndex = 1;
//            this.chkMatchWholeWord.Text = "Match whole word";
//            this.chkMatchWholeWord.UseVisualStyleBackColor = true;
//            // 
//            // chkMatchCase
//            // 
//            this.chkMatchCase.AutoSize = true;
//            this.chkMatchCase.Location = new System.Drawing.Point(7, 20);
//            this.chkMatchCase.Name = "chkMatchCase";
//            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
//            this.chkMatchCase.TabIndex = 0;
//            this.chkMatchCase.Text = "Match case";
//            this.chkMatchCase.UseVisualStyleBackColor = true;
//            // 
//            // clbLevel
//            // 
//            this.clbLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//            | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.clbLevel.CheckOnClick = true;
//            this.clbLevel.Location = new System.Drawing.Point(19, 253);
//            this.clbLevel.Name = "clbLevel";
//            this.clbLevel.Size = new System.Drawing.Size(277, 109);
//            this.clbLevel.TabIndex = 9;
//            // 
//            // lblLevels
//            // 
//            this.lblLevels.AutoSize = true;
//            this.lblLevels.Location = new System.Drawing.Point(16, 234);
//            this.lblLevels.Name = "lblLevels";
//            this.lblLevels.Size = new System.Drawing.Size(56, 13);
//            this.lblLevels.TabIndex = 8;
//            this.lblLevels.Text = "At level(s):";
//            // 
//            // txtDescription
//            // 
//            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.txtDescription.Location = new System.Drawing.Point(16, 207);
//            this.txtDescription.Name = "txtDescription";
//            this.txtDescription.Size = new System.Drawing.Size(282, 20);
//            this.txtDescription.TabIndex = 7;
//            // 
//            // lblDescription
//            // 
//            this.lblDescription.AutoSize = true;
//            this.lblDescription.Location = new System.Drawing.Point(13, 190);
//            this.lblDescription.Name = "lblDescription";
//            this.lblDescription.Size = new System.Drawing.Size(140, 13);
//            this.lblDescription.TabIndex = 6;
//            this.lblDescription.Text = "All or part of the Description:";
//            // 
//            // txtName
//            // 
//            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.txtName.Location = new System.Drawing.Point(16, 163);
//            this.txtName.Name = "txtName";
//            this.txtName.Size = new System.Drawing.Size(282, 20);
//            this.txtName.TabIndex = 5;
//            // 
//            // lblName
//            // 
//            this.lblName.AutoSize = true;
//            this.lblName.Location = new System.Drawing.Point(13, 146);
//            this.lblName.Name = "lblName";
//            this.lblName.Size = new System.Drawing.Size(115, 13);
//            this.lblName.TabIndex = 4;
//            this.lblName.Text = "All or part of the Name:";
//            // 
//            // txtID
//            // 
//            this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.txtID.Location = new System.Drawing.Point(16, 120);
//            this.txtID.Name = "txtID";
//            this.txtID.Size = new System.Drawing.Size(282, 20);
//            this.txtID.TabIndex = 3;
//            // 
//            // lblID
//            // 
//            this.lblID.AutoSize = true;
//            this.lblID.Location = new System.Drawing.Point(13, 103);
//            this.lblID.Name = "lblID";
//            this.lblID.Size = new System.Drawing.Size(98, 13);
//            this.lblID.TabIndex = 2;
//            this.lblID.Text = "All or part of the ID:";
//            // 
//            // lblCriteria
//            // 
//            this.lblCriteria.Location = new System.Drawing.Point(10, 7);
//            this.lblCriteria.Name = "lblCriteria";
//            this.lblCriteria.Size = new System.Drawing.Size(203, 30);
//            this.lblCriteria.TabIndex = 1;
//            this.lblCriteria.Text = "Criteria";
//            // 
//            // btnSearch
//            // 
//            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSearch.Location = new System.Drawing.Point(221, 493);
//            this.btnSearch.Name = "btnSearch";
//            this.btnSearch.Size = new System.Drawing.Size(75, 23);
//            this.btnSearch.TabIndex = 0;
//            this.btnSearch.Text = "btnSearch";
//            this.btnSearch.UseVisualStyleBackColor = true;
//            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
//            // 
//            // pnlCharacteristics
//            // 
//            this.pnlCharacteristics.Controls.Add(this.pnlCharacteristicQuery);
//            this.pnlCharacteristics.Controls.Add(this.btnRParen);
//            this.pnlCharacteristics.Controls.Add(this.btnLParen);
//            this.pnlCharacteristics.Controls.Add(this.btnOr);
//            this.pnlCharacteristics.Controls.Add(this.btnNot);
//            this.pnlCharacteristics.Controls.Add(this.btnAnd);
//            this.pnlCharacteristics.Controls.Add(this.btnClose);
//            this.pnlCharacteristics.Controls.Add(this.btnOK);
//            this.pnlCharacteristics.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.pnlCharacteristics.Location = new System.Drawing.Point(0, 0);
//            this.pnlCharacteristics.Name = "pnlCharacteristics";
//            this.pnlCharacteristics.Size = new System.Drawing.Size(561, 544);
//            this.pnlCharacteristics.TabIndex = 4;
//            // 
//            // pnlCharacteristicQuery
//            // 
//            this.pnlCharacteristicQuery.AllowDrop = true;
//            this.pnlCharacteristicQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//            | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.pnlCharacteristicQuery.AutoScroll = true;
//            this.pnlCharacteristicQuery.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            this.pnlCharacteristicQuery.Location = new System.Drawing.Point(14, 68);
//            this.pnlCharacteristicQuery.Name = "pnlCharacteristicQuery";
//            this.pnlCharacteristicQuery.Size = new System.Drawing.Size(538, 403);
//            this.pnlCharacteristicQuery.TabIndex = 36;
//            this.pnlCharacteristicQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//            this.pnlCharacteristicQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//            this.pnlCharacteristicQuery.DragOver += new System.Windows.Forms.DragEventHandler(this.panel_DragOver);
//            this.pnlCharacteristicQuery.DragLeave += new System.EventHandler(this.panel_DragLeave);
//            // 
//            // btnRParen
//            // 
//            this.btnRParen.BackColor = System.Drawing.SystemColors.Control;
//            this.btnRParen.Location = new System.Drawing.Point(285, 23);
//            this.btnRParen.Name = "btnRParen";
//            this.btnRParen.Size = new System.Drawing.Size(27, 23);
//            this.btnRParen.TabIndex = 34;
//            this.btnRParen.Text = ")";
//            this.btnRParen.UseVisualStyleBackColor = false;
//            this.btnRParen.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnRParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            this.btnRParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnRParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnRParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            // 
//            // btnLParen
//            // 
//            this.btnLParen.BackColor = System.Drawing.SystemColors.Control;
//            this.btnLParen.Location = new System.Drawing.Point(259, 23);
//            this.btnLParen.Name = "btnLParen";
//            this.btnLParen.Size = new System.Drawing.Size(27, 23);
//            this.btnLParen.TabIndex = 33;
//            this.btnLParen.Text = "(";
//            this.btnLParen.UseVisualStyleBackColor = false;
//            this.btnLParen.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnLParen.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//            this.btnLParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnLParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnLParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            // 
//            // btnOr
//            // 
//            this.btnOr.BackColor = System.Drawing.SystemColors.Control;
//            this.btnOr.Location = new System.Drawing.Point(175, 23);
//            this.btnOr.Name = "btnOr";
//            this.btnOr.Size = new System.Drawing.Size(43, 23);
//            this.btnOr.TabIndex = 31;
//            this.btnOr.Text = "Or";
//            this.btnOr.UseVisualStyleBackColor = false;
//            this.btnOr.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnOr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            this.btnOr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnOr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnOr.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            // 
//            // btnNot
//            // 
//            this.btnNot.BackColor = System.Drawing.SystemColors.Control;
//            this.btnNot.Location = new System.Drawing.Point(217, 23);
//            this.btnNot.Name = "btnNot";
//            this.btnNot.Size = new System.Drawing.Size(43, 23);
//            this.btnNot.TabIndex = 32;
//            this.btnNot.Text = "Not";
//            this.btnNot.UseVisualStyleBackColor = false;
//            this.btnNot.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnNot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//            this.btnNot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnNot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnNot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            // 
//            // btnAnd
//            // 
//            this.btnAnd.BackColor = System.Drawing.SystemColors.Control;
//            this.btnAnd.Location = new System.Drawing.Point(133, 23);
//            this.btnAnd.Name = "btnAnd";
//            this.btnAnd.Size = new System.Drawing.Size(43, 23);
//            this.btnAnd.TabIndex = 30;
//            this.btnAnd.Text = "And";
//            this.btnAnd.UseVisualStyleBackColor = false;
//            this.btnAnd.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnAnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            this.btnAnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnAnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnAnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            // 
//            // btnClose
//            // 
//            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnClose.Location = new System.Drawing.Point(396, 490);
//            this.btnClose.Name = "btnClose";
//            this.btnClose.Size = new System.Drawing.Size(75, 23);
//            this.btnClose.TabIndex = 1;
//            this.btnClose.Text = "Close";
//            this.btnClose.UseVisualStyleBackColor = true;
//            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
//            // 
//            // btnOK
//            // 
//            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnOK.Location = new System.Drawing.Point(477, 490);
//            this.btnOK.Name = "btnOK";
//            this.btnOK.Size = new System.Drawing.Size(75, 23);
//            this.btnOK.TabIndex = 0;
//            this.btnOK.Text = "OK";
//            this.btnOK.UseVisualStyleBackColor = true;
//            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
//            // 
//            // pnlSearch
//            // 
//            this.pnlSearch.Controls.Add(this.txtEdit);
//            this.pnlSearch.Controls.Add(this.lvNodes);
//            this.pnlSearch.Controls.Add(this.lblInstructions);
//            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
//            this.pnlSearch.Name = "pnlSearch";
//            this.pnlSearch.Size = new System.Drawing.Size(561, 544);
//            this.pnlSearch.TabIndex = 3;
//            // 
//            // txtEdit
//            // 
//            this.txtEdit.Location = new System.Drawing.Point(144, 3);
//            this.txtEdit.Name = "txtEdit";
//            this.txtEdit.Size = new System.Drawing.Size(100, 20);
//            this.txtEdit.TabIndex = 2;
//            this.txtEdit.Visible = false;
//            // 
//            // lvNodes
//            // 
//            this.lvNodes.AllowColumnReorder = true;
//            this.lvNodes.AllowDrop = true;
//            this.lvNodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//            | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.lvNodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//            this.colMerchandise,
//            this.colID,
//            this.colName,
//            this.colDescription,
//            this.colLevel});
//            this.lvNodes.FullRowSelect = true;
//            this.lvNodes.Location = new System.Drawing.Point(14, 24);
//            this.lvNodes.Name = "lvNodes";
//            this.lvNodes.Size = new System.Drawing.Size(541, 497);
//            this.lvNodes.TabIndex = 1;
//            this.lvNodes.UseCompatibleStateImageBehavior = false;
//            this.lvNodes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvNodes_ColumnClick);
//            this.lvNodes.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvNodes_ItemSelectionChanged);
//            this.lvNodes.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvNodes_DragEnter);
//            this.lvNodes.DragOver += new System.Windows.Forms.DragEventHandler(this.lvNodes_DragOver);
//            this.lvNodes.DragLeave += new System.EventHandler(this.lvNodes_DragLeave);
//            this.lvNodes.DoubleClick += new System.EventHandler(this.lvNodes_DoubleClick);
//            this.lvNodes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvNodes_MouseDown);
//            this.lvNodes.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvNodes_MouseMove);
//            this.lvNodes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvNodes_MouseUp);
//            // 
//            // colMerchandise
//            // 
//            this.colMerchandise.DisplayIndex = 4;
//            this.colMerchandise.Text = "Merchandise";
//            this.colMerchandise.Width = 150;
//            // 
//            // colID
//            // 
//            this.colID.DisplayIndex = 0;
//            this.colID.Text = "ID";
//            this.colID.Width = 120;
//            // 
//            // colName
//            // 
//            this.colName.DisplayIndex = 1;
//            this.colName.Text = "Name";
//            this.colName.Width = 100;
//            // 
//            // colDescription
//            // 
//            this.colDescription.DisplayIndex = 2;
//            this.colDescription.Text = "Description";
//            this.colDescription.Width = 150;
//            // 
//            // colLevel
//            // 
//            this.colLevel.DisplayIndex = 3;
//            this.colLevel.Text = "Level";
//            this.colLevel.Width = 100;
//            // 
//            // lblInstructions
//            // 
//            this.lblInstructions.AutoSize = true;
//            this.lblInstructions.Location = new System.Drawing.Point(23, 6);
//            this.lblInstructions.Name = "lblInstructions";
//            this.lblInstructions.Size = new System.Drawing.Size(61, 13);
//            this.lblInstructions.TabIndex = 0;
//            this.lblInstructions.Text = "Instructions";
//            // 
//            // cmsResults
//            // 
//            this.cmsResults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.cmiColChooser,
//            this.toolStripMenuItem3,
//            this.cmiCut,
//            this.cmiCopy,
//            this.toolStripMenuItem1,
//            this.cmiDelete,
//            this.cmiRename,
//            this.toolStripMenuItem2,
//            this.cmiSelectAll,
//            this.toolStripMenuItem4,
//            this.cmiLocate});
//            this.cmsResults.Name = "mnuResults";
//            this.cmsResults.Size = new System.Drawing.Size(174, 182);
//            this.cmsResults.Opening += new System.ComponentModel.CancelEventHandler(this.cmsResults_Opening);
//            // 
//            // cmiColChooser
//            // 
//            this.cmiColChooser.Name = "cmiColChooser";
//            this.cmiColChooser.Size = new System.Drawing.Size(173, 22);
//            this.cmiColChooser.Text = "Column Chooser...";
//            this.cmiColChooser.Click += new System.EventHandler(this.cmiColChooser_Click);
//            // 
//            // toolStripMenuItem3
//            // 
//            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
//            this.toolStripMenuItem3.Size = new System.Drawing.Size(170, 6);
//            // 
//            // cmiCut
//            // 
//            this.cmiCut.Name = "cmiCut";
//            this.cmiCut.ShortcutKeyDisplayString = "Ctrl+X";
//            this.cmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
//            this.cmiCut.Size = new System.Drawing.Size(173, 22);
//            this.cmiCut.Text = "&Cut";
//            // 
//            // cmiCopy
//            // 
//            this.cmiCopy.Name = "cmiCopy";
//            this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
//            this.cmiCopy.Size = new System.Drawing.Size(173, 22);
//            this.cmiCopy.Text = "Copy";
//            // 
//            // toolStripMenuItem1
//            // 
//            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
//            this.toolStripMenuItem1.Size = new System.Drawing.Size(170, 6);
//            // 
//            // cmiDelete
//            // 
//            this.cmiDelete.Name = "cmiDelete";
//            this.cmiDelete.Size = new System.Drawing.Size(173, 22);
//            this.cmiDelete.Text = "Delete";
//            // 
//            // cmiRename
//            // 
//            this.cmiRename.Name = "cmiRename";
//            this.cmiRename.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
//            this.cmiRename.Size = new System.Drawing.Size(173, 22);
//            this.cmiRename.Text = "Rename";
//            // 
//            // toolStripMenuItem2
//            // 
//            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
//            this.toolStripMenuItem2.Size = new System.Drawing.Size(170, 6);
//            // 
//            // cmiSelectAll
//            // 
//            this.cmiSelectAll.Name = "cmiSelectAll";
//            this.cmiSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
//            this.cmiSelectAll.Size = new System.Drawing.Size(173, 22);
//            this.cmiSelectAll.Text = "Select All";
//            this.cmiSelectAll.Click += new System.EventHandler(this.cmiSelectAll_Click);
//            // 
//            // toolStripMenuItem4
//            // 
//            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
//            this.toolStripMenuItem4.Size = new System.Drawing.Size(170, 6);
//            // 
//            // cmiLocate
//            // 
//            this.cmiLocate.Name = "cmiLocate";
//            this.cmiLocate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
//            this.cmiLocate.Size = new System.Drawing.Size(173, 22);
//            this.cmiLocate.Text = "Locate";
//            // 
//            // cmsLevels
//            // 
//            this.cmsLevels.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.cmiLevelSelectAll,
//            this.cmiLevelClearAll});
//            this.cmsLevels.Name = "cmsLevels";
//            this.cmsLevels.Size = new System.Drawing.Size(123, 48);
//            // 
//            // cmiLevelSelectAll
//            // 
//            this.cmiLevelSelectAll.Name = "cmiLevelSelectAll";
//            this.cmiLevelSelectAll.Size = new System.Drawing.Size(122, 22);
//            this.cmiLevelSelectAll.Text = "Select All";
//            this.cmiLevelSelectAll.Click += new System.EventHandler(this.cmiLevelSelectAll_Click);
//            // 
//            // cmiLevelClearAll
//            // 
//            this.cmiLevelClearAll.Name = "cmiLevelClearAll";
//            this.cmiLevelClearAll.Size = new System.Drawing.Size(122, 22);
//            this.cmiLevelClearAll.Text = "Clear All";
//            this.cmiLevelClearAll.Click += new System.EventHandler(this.cmiLevelClearAll_Click);
//            // 
//            // cmsCharPanel
//            // 
//            this.cmsCharPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.cmiCharClearAll});
//            this.cmsCharPanel.Name = "cmsCharPanel";
//            this.cmsCharPanel.Size = new System.Drawing.Size(102, 26);
//            // 
//            // cmiCharClearAll
//            // 
//            this.cmiCharClearAll.Name = "cmiCharClearAll";
//            this.cmiCharClearAll.Size = new System.Drawing.Size(101, 22);
//            this.cmiCharClearAll.Text = "Clear";
//            this.cmiCharClearAll.Click += new System.EventHandler(this.cmiCharClearAll_Click);
//            // 
//            // MerchandiseNodeSearch
//            // 
//            this.AllowDragDrop = true;
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(878, 548);
//            this.Controls.Add(this.splitContainer1);
//            this.Name = "MerchandiseNodeSearch";
//            this.Text = "MerchandiseNodeSearch";
//            this.Load += new System.EventHandler(this.MerchandiseNodeSearch_Load);
//            this.Controls.SetChildIndex(this.splitContainer1, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            this.splitContainer1.Panel1.ResumeLayout(false);
//            this.splitContainer1.Panel1.PerformLayout();
//            this.splitContainer1.Panel2.ResumeLayout(false);
//            this.splitContainer1.ResumeLayout(false);
//            this.gbxOptions.ResumeLayout(false);
//            this.gbxOptions.PerformLayout();
//            this.pnlCharacteristics.ResumeLayout(false);
//            this.pnlSearch.ResumeLayout(false);
//            this.pnlSearch.PerformLayout();
//            this.cmsResults.ResumeLayout(false);
//            this.cmsLevels.ResumeLayout(false);
//            this.cmsCharPanel.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.SplitContainer splitContainer1;
//        private System.Windows.Forms.Button btnSearch;
//        private System.Windows.Forms.Label lblCriteria;
//        private System.Windows.Forms.Label lblInstructions;
//        private System.Windows.Forms.ListView lvNodes;
//        private System.Windows.Forms.ColumnHeader colID;
//        private System.Windows.Forms.ColumnHeader colName;
//        private System.Windows.Forms.ColumnHeader colDescription;
//        private System.Windows.Forms.ColumnHeader colLevel;
//        private System.Windows.Forms.TextBox txtDescription;
//        private System.Windows.Forms.Label lblDescription;
//        private System.Windows.Forms.TextBox txtName;
//        private System.Windows.Forms.Label lblName;
//        private System.Windows.Forms.TextBox txtID;
//        private System.Windows.Forms.Label lblID;
//        private System.Windows.Forms.CheckedListBox clbLevel;
//        private System.Windows.Forms.Label lblLevels;
//        private System.Windows.Forms.ContextMenuStrip cmsResults;
//        private System.Windows.Forms.ToolStripMenuItem cmiCut;
//        private System.Windows.Forms.ToolStripMenuItem cmiCopy;
//        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
//        private System.Windows.Forms.ToolStripMenuItem cmiRename;
//        private System.Windows.Forms.ToolStripMenuItem cmiLocate;
//        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
//        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
//        private System.Windows.Forms.GroupBox gbxOptions;
//        private System.Windows.Forms.CheckBox chkMatchWholeWord;
//        private System.Windows.Forms.CheckBox chkMatchCase;
//        private System.Windows.Forms.ToolStripMenuItem cmiSelectAll;
//        private System.Windows.Forms.ContextMenuStrip cmsLevels;
//        private System.Windows.Forms.ToolStripMenuItem cmiLevelSelectAll;
//        private System.Windows.Forms.ToolStripMenuItem cmiLevelClearAll;
//        private System.Windows.Forms.TextBox txtEdit;
//        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
//        private System.Windows.Forms.ProgressBar progressBar1;
//        private System.Windows.Forms.Panel pnlSearch;
//        private System.Windows.Forms.Panel pnlCharacteristics;
//        private System.Windows.Forms.Button btnCharacteristics;
//        private System.Windows.Forms.CheckBox chkCharacteristics;
//        private System.Windows.Forms.Button btnClose;
//        private System.Windows.Forms.Button btnOK;
//        private System.Windows.Forms.Button btnRParen;
//        private System.Windows.Forms.Button btnLParen;
//        private System.Windows.Forms.Button btnOr;
//        private System.Windows.Forms.Button btnNot;
//        private System.Windows.Forms.Button btnAnd;
//        private System.Windows.Forms.Panel pnlCharacteristicQuery;
//        private System.Windows.Forms.ToolStripMenuItem cmiColChooser;
//        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
//        private System.Windows.Forms.ContextMenuStrip cmsCharPanel;
//        private System.Windows.Forms.ToolStripMenuItem cmiCharClearAll;
//        private System.Windows.Forms.Label lblLookIn;
//        private System.Windows.Forms.ListView lvLookIn;
//        private System.Windows.Forms.ColumnHeader colLookInNode;
//        private System.Windows.Forms.ColumnHeader colMerchandise;
//        private System.Windows.Forms.Label lblSearching;

//    }
//}