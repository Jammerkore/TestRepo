namespace MIDRetail.Windows
{
	partial class ProductCharSearch
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
			this.txtCharacteristic = new System.Windows.Forms.TextBox();
			this.lblCharacteristic = new System.Windows.Forms.Label();
			this.gbxOptions = new System.Windows.Forms.GroupBox();
			this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.lblCriteria = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.txtEdit = new System.Windows.Forms.TextBox();
			this.lvProductChars = new System.Windows.Forms.ListView();
			this.colValue = new System.Windows.Forms.ColumnHeader();
			this.colCharacteristic = new System.Windows.Forms.ColumnHeader();
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
			this.splitContainer1.Panel1.Controls.Add(this.txtCharacteristic);
			this.splitContainer1.Panel1.Controls.Add(this.lblCharacteristic);
			this.splitContainer1.Panel1.Controls.Add(this.gbxOptions);
			this.splitContainer1.Panel1.Controls.Add(this.txtValue);
			this.splitContainer1.Panel1.Controls.Add(this.lblValue);
			this.splitContainer1.Panel1.Controls.Add(this.lblCriteria);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragOver);
			this.splitContainer1.Panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainer1_Panel1_DragEnter);
			this.splitContainer1.Panel1.DragLeave += new System.EventHandler(this.splitContainer1_Panel1_DragLeave);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtEdit);
			this.splitContainer1.Panel2.Controls.Add(this.lvProductChars);
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
			// txtCharacteristic
			// 
			this.txtCharacteristic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCharacteristic.Location = new System.Drawing.Point(13, 110);
			this.txtCharacteristic.Name = "txtCharacteristic";
			this.txtCharacteristic.Size = new System.Drawing.Size(197, 20);
			this.txtCharacteristic.TabIndex = 12;
			// 
			// lblCharacteristic
			// 
			this.lblCharacteristic.AutoSize = true;
			this.lblCharacteristic.Location = new System.Drawing.Point(10, 93);
			this.lblCharacteristic.Name = "lblCharacteristic";
			this.lblCharacteristic.Size = new System.Drawing.Size(151, 13);
			this.lblCharacteristic.TabIndex = 11;
			this.lblCharacteristic.Text = "All or part of the Characteristic:";
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
			// txtValue
			// 
			this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue.Location = new System.Drawing.Point(16, 66);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(197, 20);
			this.txtValue.TabIndex = 3;
			// 
			// lblValue
			// 
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(13, 49);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(114, 13);
			this.lblValue.TabIndex = 2;
			this.lblValue.Text = "All or part of the Value:";
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
			// lvProductChars
			// 
			this.lvProductChars.AllowColumnReorder = true;
			this.lvProductChars.AllowDrop = true;
			this.lvProductChars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvProductChars.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colValue,
            this.colCharacteristic});
			this.lvProductChars.Location = new System.Drawing.Point(3, 29);
			this.lvProductChars.Name = "lvProductChars";
			this.lvProductChars.Size = new System.Drawing.Size(439, 361);
			this.lvProductChars.TabIndex = 1;
			this.lvProductChars.UseCompatibleStateImageBehavior = false;
			this.lvProductChars.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvProductChars_DragEnter);
			this.lvProductChars.DoubleClick += new System.EventHandler(this.lvProductChars_DoubleClick);
			this.lvProductChars.DragOver += new System.Windows.Forms.DragEventHandler(this.lvProductChars_DragOver);
			this.lvProductChars.DragLeave += new System.EventHandler(this.lvProductChars_DragLeave);
			this.lvProductChars.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvProductChars_MouseUp);
			this.lvProductChars.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvProductChars_ColumnClick);
			this.lvProductChars.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvProductChars_MouseMove);
			this.lvProductChars.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvProductChars_ItemSelectionChanged);
			this.lvProductChars.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvProductChars_MouseDown);
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			this.colValue.Width = 120;
			// 
			// colCharacteristic
			// 
			this.colCharacteristic.Text = "Characteristic";
			this.colCharacteristic.Width = 100;
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
			// ProductCharSearch
			// 
			this.AllowDragDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 406);
			this.Controls.Add(this.splitContainer1);
			this.Name = "ProductCharSearch";
			this.Text = "ProductCharSearch";
			this.Load += new System.EventHandler(this.ProductCharSearch_Load);
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
		private System.Windows.Forms.ListView lvProductChars;
		private System.Windows.Forms.ColumnHeader colValue;
		private System.Windows.Forms.ColumnHeader colCharacteristic;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.Label lblValue;
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
		private System.Windows.Forms.TextBox txtCharacteristic;
		private System.Windows.Forms.Label lblCharacteristic;
		private System.Windows.Forms.ProgressBar progressBar1;

	}
}