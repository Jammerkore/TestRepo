namespace MIDRetail.Windows
{
	partial class ProductCharProperties
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
			this.btnApply = new System.Windows.Forms.Button();
			this.lblCharacteristic = new System.Windows.Forms.Label();
			this.lvAssigned = new System.Windows.Forms.ListView();
			this.colProduct = new System.Windows.Forms.ColumnHeader();
			this.colHierarchy = new System.Windows.Forms.ColumnHeader();
			this.colLevel = new System.Windows.Forms.ColumnHeader();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.cmsAssigned = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.gbxAssigned = new System.Windows.Forms.GroupBox();
			this.cmsAssigned.SuspendLayout();
			this.gbxAssigned.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Location = new System.Drawing.Point(358, 367);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// lblCharacteristic
			// 
			this.lblCharacteristic.AutoSize = true;
			this.lblCharacteristic.Location = new System.Drawing.Point(15, 12);
			this.lblCharacteristic.Name = "lblCharacteristic";
			this.lblCharacteristic.Size = new System.Drawing.Size(74, 13);
			this.lblCharacteristic.TabIndex = 0;
			this.lblCharacteristic.Text = "Characteristic:";
			// 
			// lvAssigned
			// 
			this.lvAssigned.AllowColumnReorder = true;
			this.lvAssigned.AllowDrop = true;
			this.lvAssigned.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvAssigned.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colProduct,
            this.colHierarchy,
            this.colLevel});
			this.lvAssigned.Location = new System.Drawing.Point(10, 19);
			this.lvAssigned.Name = "lvAssigned";
			this.lvAssigned.Size = new System.Drawing.Size(405, 281);
			this.lvAssigned.TabIndex = 0;
			this.lvAssigned.UseCompatibleStateImageBehavior = false;
			this.lvAssigned.View = System.Windows.Forms.View.Details;
			this.lvAssigned.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvAssigned_DragEnter);
			this.lvAssigned.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvAssigned_DragDrop);
			this.lvAssigned.DragOver += new System.Windows.Forms.DragEventHandler(this.lvAssigned_DragOver);
			this.lvAssigned.DragLeave += new System.EventHandler(this.lvAssigned_DragLeave);
			this.lvAssigned.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvAssigned_KeyDown);
			// 
			// colProduct
			// 
			this.colProduct.Text = "Product";
			this.colProduct.Width = 150;
			// 
			// colHierarchy
			// 
			this.colHierarchy.Text = "Hierarchy";
			this.colHierarchy.Width = 150;
			// 
			// colLevel
			// 
			this.colLevel.Text = "Level";
			this.colLevel.Width = 100;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(276, 367);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(196, 367);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// cmsAssigned
			// 
			this.cmsAssigned.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiCut,
            this.cmiCopy,
            this.cmiSeparator1,
            this.cmiPaste,
            this.cmiDelete});
			this.cmsAssigned.Name = "cmsAssigned";
			this.cmsAssigned.Size = new System.Drawing.Size(153, 120);
			this.cmsAssigned.Opening += new System.ComponentModel.CancelEventHandler(this.cmsAssigned_Opening);
			// 
			// cmiCut
			// 
			this.cmiCut.Name = "cmiCut";
			this.cmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmiCut.Size = new System.Drawing.Size(152, 22);
			this.cmiCut.Text = "Cut";
			this.cmiCut.Click += new System.EventHandler(this.cmiCut_Click);
			// 
			// cmiCopy
			// 
			this.cmiCopy.Name = "cmiCopy";
			this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmiCopy.Size = new System.Drawing.Size(152, 22);
			this.cmiCopy.Text = "Copy";
			this.cmiCopy.Click += new System.EventHandler(this.cmiCopy_Click);
			// 
			// cmiSeparator1
			// 
			this.cmiSeparator1.Name = "cmiSeparator1";
			this.cmiSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// cmiPaste
			// 
			this.cmiPaste.Name = "cmiPaste";
			this.cmiPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.cmiPaste.Size = new System.Drawing.Size(152, 22);
			this.cmiPaste.Text = "Paste";
			this.cmiPaste.Click += new System.EventHandler(this.cmiPaste_Click);
			// 
			// cmiDelete
			// 
			this.cmiDelete.Name = "cmiDelete";
			this.cmiDelete.Size = new System.Drawing.Size(152, 22);
			this.cmiDelete.Text = "Delete";
			this.cmiDelete.Click += new System.EventHandler(this.cmiDelete_Click);
			// 
			// gbxAssigned
			// 
			this.gbxAssigned.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbxAssigned.Controls.Add(this.lvAssigned);
			this.gbxAssigned.Location = new System.Drawing.Point(12, 38);
			this.gbxAssigned.Name = "gbxAssigned";
			this.gbxAssigned.Size = new System.Drawing.Size(425, 315);
			this.gbxAssigned.TabIndex = 5;
			this.gbxAssigned.TabStop = false;
			this.gbxAssigned.Text = "Assigned";
			this.gbxAssigned.DragOver += new System.Windows.Forms.DragEventHandler(this.gbxAssigned_DragOver);
			this.gbxAssigned.DragEnter += new System.Windows.Forms.DragEventHandler(this.gbxAssigned_DragEnter);
			this.gbxAssigned.DragLeave += new System.EventHandler(this.gbxAssigned_DragLeave);
			// 
			// ProductCharProperties
			// 
			this.AllowDragDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(449, 402);
			this.Controls.Add(this.gbxAssigned);
			this.Controls.Add(this.lblCharacteristic);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnApply);
			this.Name = "ProductCharProperties";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ProductCharProperties";
			this.Load += new System.EventHandler(this.ProductCharProperties_Load);
			this.Controls.SetChildIndex(this.btnApply, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.btnOK, 0);
			this.Controls.SetChildIndex(this.lblCharacteristic, 0);
			this.Controls.SetChildIndex(this.gbxAssigned, 0);
			this.cmsAssigned.ResumeLayout(false);
			this.gbxAssigned.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ListView lvAssigned;
		private System.Windows.Forms.ContextMenuStrip cmsAssigned;
		private System.Windows.Forms.ToolStripMenuItem cmiCut;
		private System.Windows.Forms.ToolStripMenuItem cmiCopy;
		private System.Windows.Forms.ToolStripSeparator cmiSeparator1;
		private System.Windows.Forms.ToolStripMenuItem cmiPaste;
		private System.Windows.Forms.ToolStripMenuItem cmiDelete;
		private System.Windows.Forms.ColumnHeader colProduct;
		private System.Windows.Forms.ColumnHeader colHierarchy;
		private System.Windows.Forms.ColumnHeader colLevel;
		private System.Windows.Forms.Label lblCharacteristic;
		private System.Windows.Forms.GroupBox gbxAssigned;
	}
}