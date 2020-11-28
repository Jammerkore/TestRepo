namespace MIDRetail.Windows
{
	partial class ColorNodeSearch
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.txtGroupName = new System.Windows.Forms.TextBox();
			this.lblGroupName = new System.Windows.Forms.Label();
			this.gbxOptions = new System.Windows.Forms.GroupBox();
			this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblID = new System.Windows.Forms.Label();
			this.lblCriteria = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.txtEdit = new System.Windows.Forms.TextBox();
			this.lvColors = new System.Windows.Forms.ListView();
			this.colID = new System.Windows.Forms.ColumnHeader();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colGroup = new System.Windows.Forms.ColumnHeader();
			this.lblInstructions = new System.Windows.Forms.Label();
			this.cmsResults = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiRename = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiLocate = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.gbxOptions.SuspendLayout();
			this.cmsResults.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
			this.splitContainer1.Panel1.Controls.Add(this.txtGroupName);
			this.splitContainer1.Panel1.Controls.Add(this.lblGroupName);
			this.splitContainer1.Panel1.Controls.Add(this.gbxOptions);
			this.splitContainer1.Panel1.Controls.Add(this.txtName);
			this.splitContainer1.Panel1.Controls.Add(this.lblName);
			this.splitContainer1.Panel1.Controls.Add(this.txtID);
			this.splitContainer1.Panel1.Controls.Add(this.lblID);
			this.splitContainer1.Panel1.Controls.Add(this.lblCriteria);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragOver);
			this.splitContainer1.Panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragEnter);
			this.splitContainer1.Panel1.DragLeave += new System.EventHandler(this.splitContainer1_Panel1_DragLeave);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtEdit);
			this.splitContainer1.Panel2.Controls.Add(this.lvColors);
			this.splitContainer1.Panel2.Controls.Add(this.lblInstructions);
			this.splitContainer1.Size = new System.Drawing.Size(678, 406);
			this.splitContainer1.SplitterDistance = 225;
			this.splitContainer1.TabIndex = 4;
			this.splitContainer1.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer1_DragOver);
			this.splitContainer1.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainer1_DragEnter);
			this.splitContainer1.DragLeave += new System.EventHandler(this.splitContainer1_DragLeave);
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.ForeColor = System.Drawing.Color.LimeGreen;
			this.progressBar1.Location = new System.Drawing.Point(16, 375);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(100, 12);
			this.progressBar1.TabIndex = 12;
			this.progressBar1.Visible = false;
			// 
			// txtGroupName
			// 
			this.txtGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtGroupName.Location = new System.Drawing.Point(13, 156);
			this.txtGroupName.Name = "txtGroupName";
			this.txtGroupName.Size = new System.Drawing.Size(197, 20);
			this.txtGroupName.TabIndex = 12;
			// 
			// lblGroupName
			// 
			this.lblGroupName.AutoSize = true;
			this.lblGroupName.Location = new System.Drawing.Point(10, 139);
			this.lblGroupName.Name = "lblGroupName";
			this.lblGroupName.Size = new System.Drawing.Size(147, 13);
			this.lblGroupName.TabIndex = 11;
			this.lblGroupName.Text = "All or part of the Group Name:";
			// 
			// gbxOptions
			// 
			this.gbxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbxOptions.Controls.Add(this.chkMatchWholeWord);
			this.gbxOptions.Controls.Add(this.chkMatchCase);
			this.gbxOptions.Location = new System.Drawing.Point(19, 258);
			this.gbxOptions.Name = "gbxOptions";
			this.gbxOptions.Size = new System.Drawing.Size(186, 87);
			this.gbxOptions.TabIndex = 10;
			this.gbxOptions.TabStop = false;
			this.gbxOptions.Text = "Search options";
			// 
			// chkMatchWholeWord
			// 
			this.chkMatchWholeWord.AutoSize = true;
			this.chkMatchWholeWord.Location = new System.Drawing.Point(7, 44);
			this.chkMatchWholeWord.Name = "chkMatchWholeWord";
			this.chkMatchWholeWord.Size = new System.Drawing.Size(113, 17);
			this.chkMatchWholeWord.TabIndex = 1;
			this.chkMatchWholeWord.Text = "Match whole word";
			this.chkMatchWholeWord.UseVisualStyleBackColor = true;
			// 
			// chkMatchCase
			// 
			this.chkMatchCase.AutoSize = true;
			this.chkMatchCase.Location = new System.Drawing.Point(7, 20);
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
			this.chkMatchCase.TabIndex = 0;
			this.chkMatchCase.Text = "Match case";
			this.chkMatchCase.UseVisualStyleBackColor = true;
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(16, 109);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(197, 20);
			this.txtName.TabIndex = 5;
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(13, 92);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(115, 13);
			this.lblName.TabIndex = 4;
			this.lblName.Text = "All or part of the Name:";
			// 
			// txtID
			// 
			this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtID.Location = new System.Drawing.Point(16, 66);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(197, 20);
			this.txtID.TabIndex = 3;
			// 
			// lblID
			// 
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(13, 49);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(98, 13);
			this.lblID.TabIndex = 2;
			this.lblID.Text = "All or part of the ID:";
			// 
			// lblCriteria
			// 
			this.lblCriteria.Location = new System.Drawing.Point(10, 7);
			this.lblCriteria.Name = "lblCriteria";
			this.lblCriteria.Size = new System.Drawing.Size(158, 36);
			this.lblCriteria.TabIndex = 1;
			this.lblCriteria.Text = "Criteria";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(138, 367);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtEdit
			// 
			this.txtEdit.Location = new System.Drawing.Point(281, 13);
			this.txtEdit.Name = "txtEdit";
			this.txtEdit.Size = new System.Drawing.Size(100, 20);
			this.txtEdit.TabIndex = 2;
			this.txtEdit.Visible = false;
			// 
			// lvColors
			// 
			this.lvColors.AllowColumnReorder = true;
			this.lvColors.AllowDrop = true;
			this.lvColors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID,
            this.colName,
            this.colGroup});
			this.lvColors.Location = new System.Drawing.Point(3, 29);
			this.lvColors.Name = "lvColors";
			this.lvColors.Size = new System.Drawing.Size(439, 361);
			this.lvColors.TabIndex = 1;
			this.lvColors.UseCompatibleStateImageBehavior = false;
			this.lvColors.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvColors_DragEnter);
			this.lvColors.DoubleClick += new System.EventHandler(this.lvColors_DoubleClick);
			this.lvColors.DragOver += new System.Windows.Forms.DragEventHandler(this.lvColors_DragOver);
			this.lvColors.DragLeave += new System.EventHandler(this.lvColors_DragLeave);
			this.lvColors.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvColors_MouseUp);
			this.lvColors.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvColors_ColumnClick);
			this.lvColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvColors_MouseMove);
			this.lvColors.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvColors_ItemSelectionChanged);
			this.lvColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvColors_MouseDown);
			// 
			// colID
			// 
			this.colID.Text = "ID";
			this.colID.Width = 120;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 100;
			// 
			// colGroup
			// 
			this.colGroup.Text = "Group";
			this.colGroup.Width = 150;
			// 
			// lblInstructions
			// 
			this.lblInstructions.AutoSize = true;
			this.lblInstructions.Location = new System.Drawing.Point(13, 13);
			this.lblInstructions.Name = "lblInstructions";
			this.lblInstructions.Size = new System.Drawing.Size(61, 13);
			this.lblInstructions.TabIndex = 0;
			this.lblInstructions.Text = "Instructions";
			// 
			// cmsResults
			// 
			this.cmsResults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiCut,
            this.cmiCopy,
            this.toolStripMenuItem1,
            this.cmiDelete,
            this.cmiRename,
            this.toolStripMenuItem2,
            this.cmiSelectAll,
            this.toolStripMenuItem4,
            this.cmiLocate});
			this.cmsResults.Name = "mnuResults";
			this.cmsResults.Size = new System.Drawing.Size(168, 154);
			this.cmsResults.Opening += new System.ComponentModel.CancelEventHandler(this.cmsResults_Opening);
			// 
			// cmiCut
			// 
			this.cmiCut.Name = "cmiCut";
			this.cmiCut.ShortcutKeyDisplayString = "Ctrl+X";
			this.cmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmiCut.Size = new System.Drawing.Size(167, 22);
			this.cmiCut.Text = "&Cut";
			// 
			// cmiCopy
			// 
			this.cmiCopy.Name = "cmiCopy";
			this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmiCopy.Size = new System.Drawing.Size(167, 22);
			this.cmiCopy.Text = "Copy";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 6);
			// 
			// cmiDelete
			// 
			this.cmiDelete.Name = "cmiDelete";
			this.cmiDelete.Size = new System.Drawing.Size(167, 22);
			this.cmiDelete.Text = "Delete";
			// 
			// cmiRename
			// 
			this.cmiRename.Name = "cmiRename";
			this.cmiRename.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.cmiRename.Size = new System.Drawing.Size(167, 22);
			this.cmiRename.Text = "Rename";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 6);
			// 
			// cmiSelectAll
			// 
			this.cmiSelectAll.Name = "cmiSelectAll";
			this.cmiSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.cmiSelectAll.Size = new System.Drawing.Size(167, 22);
			this.cmiSelectAll.Text = "Select All";
			this.cmiSelectAll.Click += new System.EventHandler(this.cmiSelectAll_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(164, 6);
			// 
			// cmiLocate
			// 
			this.cmiLocate.Name = "cmiLocate";
			this.cmiLocate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.cmiLocate.Size = new System.Drawing.Size(167, 22);
			this.cmiLocate.Text = "Locate";
			// 
			// ColorNodeSearch
			// 
			this.AllowDragDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 406);
			this.Controls.Add(this.splitContainer1);
			this.Name = "ColorNodeSearch";
			this.Text = "ColorNodeSearch";
			this.Load += new System.EventHandler(this.ColorNodeSearch_Load);
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.gbxOptions.ResumeLayout(false);
			this.gbxOptions.PerformLayout();
			this.cmsResults.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblCriteria;
		private System.Windows.Forms.Label lblInstructions;
		private System.Windows.Forms.ListView lvColors;
		private System.Windows.Forms.ColumnHeader colID;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colGroup;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label lblID;
		private System.Windows.Forms.ContextMenuStrip cmsResults;
		private System.Windows.Forms.ToolStripMenuItem cmiCut;
		private System.Windows.Forms.ToolStripMenuItem cmiCopy;
		private System.Windows.Forms.ToolStripMenuItem cmiDelete;
		private System.Windows.Forms.ToolStripMenuItem cmiRename;
		private System.Windows.Forms.ToolStripMenuItem cmiLocate;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.GroupBox gbxOptions;
		private System.Windows.Forms.CheckBox chkMatchWholeWord;
		private System.Windows.Forms.CheckBox chkMatchCase;
		private System.Windows.Forms.ToolStripMenuItem cmiSelectAll;
		private System.Windows.Forms.TextBox txtEdit;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.TextBox txtGroupName;
		private System.Windows.Forms.Label lblGroupName;
		private System.Windows.Forms.ProgressBar progressBar1;

	}
}