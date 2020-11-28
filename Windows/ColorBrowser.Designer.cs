namespace MIDRetail.Windows
{
	partial class ColorBrowser
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tvColors = new MIDRetail.Windows.ColorTreeView();
            this.txtPlaceHolders = new System.Windows.Forms.TextBox();
            this.lblPlaceHolders = new System.Windows.Forms.Label();
            this.cmsColorTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSearch = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.panel1.SuspendLayout();
            this.cmsColorTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(124, 363);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(205, 363);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tvColors);
            this.panel1.Controls.Add(this.txtPlaceHolders);
            this.panel1.Controls.Add(this.lblPlaceHolders);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 357);
            this.panel1.TabIndex = 4;
            // 
            // tvColors
            // 
            this.tvColors.AllowAutoExpand = false;
            this.tvColors.AllowMultiSelect = false;
            this.tvColors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvColors.CurrentEffect = System.Windows.Forms.DragDropEffects.Move;
            this.tvColors.CurrentState = MIDRetail.DataCommon.eDragStates.Idle;
            this.tvColors.DragEffect = System.Windows.Forms.DragDropEffects.None;
            this.tvColors.FavoritesNode = null;
            this.tvColors.Location = new System.Drawing.Point(0, 49);
            this.tvColors.Name = "tvColors";
            this.tvColors.PerformingCopy = false;
            this.tvColors.PerformingCut = false;
            this.tvColors.SelectedNode = null;
            this.tvColors.SimulateMultiSelect = false;
            this.tvColors.Size = new System.Drawing.Size(292, 305);
            this.tvColors.TabIndex = 2;
            this.tvColors.TimerInterval = 2000;
            this.tvColors.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvColors_BeforeExpand);
            this.tvColors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvColors_KeyDown);
            // 
            // txtPlaceHolders
            // 
            this.txtPlaceHolders.Location = new System.Drawing.Point(159, 14);
            this.txtPlaceHolders.Name = "txtPlaceHolders";
            this.txtPlaceHolders.Size = new System.Drawing.Size(65, 20);
            this.txtPlaceHolders.TabIndex = 1;
            this.txtPlaceHolders.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPlaceHolders.TextChanged += new System.EventHandler(this.txtPlaceHolders_TextChanged);
            // 
            // lblPlaceHolders
            // 
            this.lblPlaceHolders.AutoSize = true;
            this.lblPlaceHolders.Location = new System.Drawing.Point(12, 20);
            this.lblPlaceHolders.Name = "lblPlaceHolders";
            this.lblPlaceHolders.Size = new System.Drawing.Size(148, 13);
            this.lblPlaceHolders.TabIndex = 0;
            this.lblPlaceHolders.Text = "Number of placeholder colors:";
            // 
            // cmsColorTreeView
            // 
            this.cmsColorTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiCopy,
            this.toolStripMenuItem1,
            this.cmiSearch});
            this.cmsColorTreeView.Name = "cmsColorTreeView";
            this.cmsColorTreeView.Size = new System.Drawing.Size(150, 54);
            // 
            // cmiCopy
            // 
            this.cmiCopy.Name = "cmiCopy";
            this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.cmiCopy.Size = new System.Drawing.Size(149, 22);
            this.cmiCopy.Text = "Copy";
            this.cmiCopy.Click += new System.EventHandler(this.cmiCopy_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
            // 
            // cmiSearch
            // 
            this.cmiSearch.Name = "cmiSearch";
            this.cmiSearch.Size = new System.Drawing.Size(149, 22);
            this.cmiSearch.Text = "Search";
            this.cmiSearch.Click += new System.EventHandler(this.cmiSearch_Click);
            // 
            // ColorBrowser
            // 
            this.AllowDragDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 398);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ColorBrowser";
            this.Text = "ColorBrowser";
            this.Load += new System.EventHandler(this.ColorBrowser_Load);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.cmsColorTreeView.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblPlaceHolders;
		private System.Windows.Forms.TextBox txtPlaceHolders;
		private System.Windows.Forms.ContextMenuStrip cmsColorTreeView;
		private System.Windows.Forms.ToolStripMenuItem cmiCopy;
		private System.Windows.Forms.ToolStripMenuItem cmiSearch;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private MIDRetail.Windows.ColorTreeView tvColors;
	}
}