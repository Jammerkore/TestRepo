namespace MIDRetail.Windows
{
	partial class ProductCharacteristicMaint
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
			this.btnClose = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.cmsProductCharTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiRename = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiSearch = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiInUse = new System.Windows.Forms.ToolStripMenuItem();
			this.btnOK = new System.Windows.Forms.Button();
			this.tvProdChars = new MIDRetail.Windows.ProductCharTreeView();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.cmsProductCharTreeView.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(145, 326);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Location = new System.Drawing.Point(226, 326);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 4;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// cmsProductCharTreeView
			// 
			this.cmsProductCharTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAdd,
            this.cmiSeparator1,
            this.cmiCut,
            this.cmiCopy,
            this.cmiPaste,
            this.cmiSeparator2,
            this.cmiDelete,
            this.cmiRename,
            this.cmiSearch,
            this.cmiSeparator3,
            this.cmiProperties,
            this.cmiInUse,
            });
			this.cmsProductCharTreeView.Name = "cmsProductCharTreeView";
			this.cmsProductCharTreeView.Size = new System.Drawing.Size(151, 198);
			this.cmsProductCharTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.cmsProductCharTreeView_Opening);
			// 
			// cmiAdd
			// 
			this.cmiAdd.Name = "cmiAdd";
			this.cmiAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.cmiAdd.Size = new System.Drawing.Size(150, 22);
			this.cmiAdd.Text = "Add";
			this.cmiAdd.Click += new System.EventHandler(this.cmiAdd_Click);
			// 
			// cmiSeparator1
			// 
			this.cmiSeparator1.Name = "cmiSeparator1";
			this.cmiSeparator1.Size = new System.Drawing.Size(147, 6);
			// 
			// cmiCut
			// 
			this.cmiCut.Name = "cmiCut";
			this.cmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmiCut.Size = new System.Drawing.Size(150, 22);
			this.cmiCut.Text = "Cut";
			this.cmiCut.Click += new System.EventHandler(this.cmiCut_Click);
			// 
			// cmiCopy
			// 
			this.cmiCopy.Name = "cmiCopy";
			this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmiCopy.Size = new System.Drawing.Size(150, 22);
			this.cmiCopy.Text = "Copy";
			this.cmiCopy.Click += new System.EventHandler(this.cmiCopy_Click);
			// 
			// cmiPaste
			// 
			this.cmiPaste.Name = "cmiPaste";
			this.cmiPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.cmiPaste.Size = new System.Drawing.Size(150, 22);
			this.cmiPaste.Text = "Paste";
			this.cmiPaste.Click += new System.EventHandler(this.cmiPaste_Click);
			// 
			// cmiSeparator2
			// 
			this.cmiSeparator2.Name = "cmiSeparator2";
			this.cmiSeparator2.Size = new System.Drawing.Size(147, 6);
			// 
			// cmiDelete
			// 
			this.cmiDelete.Name = "cmiDelete";
			this.cmiDelete.Size = new System.Drawing.Size(150, 22);
			this.cmiDelete.Text = "Delete";
			this.cmiDelete.Click += new System.EventHandler(this.cmiDelete_Click);
			// 
			// cmiRename
			// 
			this.cmiRename.Name = "cmiRename";
			this.cmiRename.Size = new System.Drawing.Size(150, 22);
			this.cmiRename.Text = "Rename";
			this.cmiRename.Click += new System.EventHandler(this.cmiRename_Click);
			// 
			// cmiSearch
			// 
			this.cmiSearch.Name = "cmiSearch";
			this.cmiSearch.Size = new System.Drawing.Size(150, 22);
			this.cmiSearch.Text = "Search";
			this.cmiSearch.Click += new System.EventHandler(this.cmiSearch_Click);
			// 
			// cmiSeparator3
			// 
			this.cmiSeparator3.Name = "cmiSeparator3";
			this.cmiSeparator3.Size = new System.Drawing.Size(147, 6);
			// 
			// cmiProperties
			// 
			this.cmiProperties.Name = "cmiProperties";
			this.cmiProperties.Size = new System.Drawing.Size(150, 22);
			this.cmiProperties.Text = "Properties";
			this.cmiProperties.Click += new System.EventHandler(this.cmiProperties_Click);
            // 
            // cmiInUse
            // 
            this.cmiInUse.Name = "cmiInUse";
            this.cmiInUse.Size = new System.Drawing.Size(150, 22);
            this.cmiInUse.Text = "In Use";
            this.cmiInUse.Click += new System.EventHandler(this.cmiInUse_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(64, 326);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// tvProdChars
			// 
			this.tvProdChars.AllowAutoExpand = true;
			this.tvProdChars.AllowDrop = true;
			this.tvProdChars.AllowMultiSelect = true;
			this.tvProdChars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tvProdChars.CurrentEffect = System.Windows.Forms.DragDropEffects.Move;
			this.tvProdChars.CurrentState = MIDRetail.DataCommon.eDragStates.Idle;
			this.tvProdChars.DragEffect = System.Windows.Forms.DragDropEffects.None;
			this.tvProdChars.FavoritesNode = null;
			this.tvProdChars.LabelEdit = true;
			this.tvProdChars.Location = new System.Drawing.Point(0, -1);
			this.tvProdChars.Name = "tvProdChars";
			this.tvProdChars.PerformingCopy = false;
			this.tvProdChars.PerformingCut = false;
			this.tvProdChars.SelectedNode = null;
			this.tvProdChars.SimulateMultiSelect = false;
			this.tvProdChars.Size = new System.Drawing.Size(313, 321);
			this.tvProdChars.TabIndex = 6;
			this.tvProdChars.TimerInterval = 2000;
			this.tvProdChars.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvProdChars_AfterCollapse);
			this.tvProdChars.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProdChars_BeforeExpand);
			this.tvProdChars.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvProdChars_DragDrop);
			this.tvProdChars.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvProdChars_BeforeLabelEdit);
			this.tvProdChars.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvProdChars_KeyDown);
			this.tvProdChars.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvProdChars_AfterExpand);
			// 
			// ProductCharacteristicMaint
			// 
			this.AcceptButton = this.btnOK;
			this.AllowDragDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(313, 361);
			this.Controls.Add(this.tvProdChars);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnApply);
			this.Name = "ProductCharacteristicMaint";
			this.Text = "ProductCharacteristicMaint";
			this.Load += new System.EventHandler(this.ProductCharacteristicMaint_Load);
			this.Controls.SetChildIndex(this.btnApply, 0);
			this.Controls.SetChildIndex(this.btnClose, 0);
			this.Controls.SetChildIndex(this.btnOK, 0);
			this.Controls.SetChildIndex(this.tvProdChars, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.cmsProductCharTreeView.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.ContextMenuStrip cmsProductCharTreeView;
		private System.Windows.Forms.ToolStripMenuItem cmiAdd;
		private System.Windows.Forms.ToolStripSeparator cmiSeparator1;
		private System.Windows.Forms.ToolStripMenuItem cmiCut;
		private System.Windows.Forms.ToolStripMenuItem cmiCopy;
		private System.Windows.Forms.ToolStripMenuItem cmiPaste;
		private System.Windows.Forms.ToolStripMenuItem cmiDelete;
		private System.Windows.Forms.ToolStripSeparator cmiSeparator2;
		private System.Windows.Forms.ToolStripMenuItem cmiSearch;
		private System.Windows.Forms.ToolStripMenuItem cmiRename;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ToolStripSeparator cmiSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ToolStripMenuItem cmiInUse;
		private MIDRetail.Windows.ProductCharTreeView tvProdChars;

	}
}