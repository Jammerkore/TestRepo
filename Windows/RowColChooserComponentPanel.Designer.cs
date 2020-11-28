using System.Timers;

namespace MIDRetail.Windows
{
	partial class RowColChooserComponentPanel
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
				this.lstComponentOrder.DrawItem -= new System.Windows.Forms.DrawItemEventHandler(this.ListBox_DrawItem);
				this.lstComponentOrder.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseUp);
				this.lstComponentOrder.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseMove);
				this.lstComponentOrder.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseDown);
				this.lstComponentOrder.DragOver -= new System.Windows.Forms.DragEventHandler(this.ListBox_DragOver);
				this.lstComponentHeaders.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.CheckListBox_ItemCheck);
				this.cmiComponentHeadersRestoreDefaults.Click -= new System.EventHandler(this.cmiComponentHeadersRestoreDefaults_Click);
				this.cmiComponentHeadersSelectAll.Click -= new System.EventHandler(this.cmiComponentHeadersSelectAll_Click);
				this.cmiComponentHeadersClearAll.Click -= new System.EventHandler(this.cmiComponentHeadersClearAll_Click);
				this.lstOtherHeaders.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.CheckListBox_ItemCheck);
				this.lstOtherOrder.DrawItem -= new System.Windows.Forms.DrawItemEventHandler(this.ListBox_DrawItem);
				this.lstOtherOrder.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseUp);
				this.lstOtherOrder.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseMove);
				this.lstOtherOrder.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseDown);
				this.lstOtherOrder.DragOver -= new System.Windows.Forms.DragEventHandler(this.ListBox_DragOver);
				this.cmiOtherHeadersRestoreDefaults.Click -= new System.EventHandler(this.cmiOtherHeadersRestoreDefaults_Click);
				this.cmiOtherHeadersSelectAll.Click -= new System.EventHandler(this.cmiOtherHeadersSelectAll_Click);
				this.cmiOtherHeadersClearAll.Click -= new System.EventHandler(this.cmiOtherHeadersClearAll_Click);
				this.Load -= new System.EventHandler(this.RowColChooserComponentPanel_Load);

				if (_scrollUpTimer != null)
				{
					_scrollUpTimer.Elapsed -= new ElapsedEventHandler(ScrollUpTimer_Elapsed);
				}

				if (_scrollDownTimer != null)
				{
					_scrollDownTimer.Elapsed -= new ElapsedEventHandler(ScrollDownTimer_Elapsed);
				}
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
			this.components = new System.ComponentModel.Container();
			this.lstComponentOrder = new System.Windows.Forms.ListBox();
			this.lblComponentOrder = new System.Windows.Forms.Label();
			this.lstComponentHeaders = new System.Windows.Forms.CheckedListBox();
			this.cmsComponentHeaders = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiComponentHeadersRestoreDefaults = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiComponentHeadersSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiComponentHeadersClearAll = new System.Windows.Forms.ToolStripMenuItem();
			this.lblComponentHeaders = new System.Windows.Forms.Label();
			this.spcChooser = new System.Windows.Forms.SplitContainer();
			this.lstOtherHeaders = new System.Windows.Forms.CheckedListBox();
			this.cmsOtherHeaders = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiOtherHeadersRestoreDefaults = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiOtherHeadersSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiOtherHeadersClearAll = new System.Windows.Forms.ToolStripMenuItem();
			this.lblOtherHeaders = new System.Windows.Forms.Label();
			this.lstOtherOrder = new System.Windows.Forms.ListBox();
			this.lblOtherOrder = new System.Windows.Forms.Label();
			this.cmsComponentHeaders.SuspendLayout();
			this.spcChooser.Panel1.SuspendLayout();
			this.spcChooser.Panel2.SuspendLayout();
			this.spcChooser.SuspendLayout();
			this.cmsOtherHeaders.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstComponentOrder
			// 
			this.lstComponentOrder.AllowDrop = true;
			this.lstComponentOrder.Dock = System.Windows.Forms.DockStyle.Top;
			this.lstComponentOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstComponentOrder.ItemHeight = 15;
			this.lstComponentOrder.Location = new System.Drawing.Point(0, 16);
			this.lstComponentOrder.Name = "lstComponentOrder";
			this.lstComponentOrder.Size = new System.Drawing.Size(164, 79);
			this.lstComponentOrder.TabIndex = 8;
			this.lstComponentOrder.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBox_DrawItem);
			this.lstComponentOrder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseUp);
			this.lstComponentOrder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseMove);
			this.lstComponentOrder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseDown);
			this.lstComponentOrder.DragOver += new System.Windows.Forms.DragEventHandler(this.ListBox_DragOver);
			// 
			// lblComponentOrder
			// 
			this.lblComponentOrder.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblComponentOrder.Location = new System.Drawing.Point(0, 0);
			this.lblComponentOrder.Name = "lblComponentOrder";
			this.lblComponentOrder.Size = new System.Drawing.Size(164, 16);
			this.lblComponentOrder.TabIndex = 9;
			this.lblComponentOrder.Text = "Order:";
			this.lblComponentOrder.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lstComponentHeaders
			// 
			this.lstComponentHeaders.CheckOnClick = true;
			this.lstComponentHeaders.ContextMenuStrip = this.cmsComponentHeaders;
			this.lstComponentHeaders.Dock = System.Windows.Forms.DockStyle.Top;
			this.lstComponentHeaders.Location = new System.Drawing.Point(0, 16);
			this.lstComponentHeaders.Name = "lstComponentHeaders";
			this.lstComponentHeaders.Size = new System.Drawing.Size(160, 79);
			this.lstComponentHeaders.TabIndex = 7;
			this.lstComponentHeaders.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckListBox_ItemCheck);
			// 
			// cmsComponentHeaders
			// 
			this.cmsComponentHeaders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiComponentHeadersRestoreDefaults,
            this.cmiComponentHeadersSelectAll,
            this.cmiComponentHeadersClearAll});
			this.cmsComponentHeaders.Name = "cmsHeaders";
			this.cmsComponentHeaders.Size = new System.Drawing.Size(167, 70);
			// 
			// cmiComponentHeadersRestoreDefaults
			// 
			this.cmiComponentHeadersRestoreDefaults.Name = "cmiComponentHeadersRestoreDefaults";
			this.cmiComponentHeadersRestoreDefaults.Size = new System.Drawing.Size(166, 22);
			this.cmiComponentHeadersRestoreDefaults.Text = "Restore Defaults";
			this.cmiComponentHeadersRestoreDefaults.Click += new System.EventHandler(this.cmiComponentHeadersRestoreDefaults_Click);
			// 
			// cmiComponentHeadersSelectAll
			// 
			this.cmiComponentHeadersSelectAll.Name = "cmiComponentHeadersSelectAll";
			this.cmiComponentHeadersSelectAll.Size = new System.Drawing.Size(166, 22);
			this.cmiComponentHeadersSelectAll.Text = "Select All";
			this.cmiComponentHeadersSelectAll.Click += new System.EventHandler(this.cmiComponentHeadersSelectAll_Click);
			// 
			// cmiComponentHeadersClearAll
			// 
			this.cmiComponentHeadersClearAll.Name = "cmiComponentHeadersClearAll";
			this.cmiComponentHeadersClearAll.Size = new System.Drawing.Size(166, 22);
			this.cmiComponentHeadersClearAll.Text = "Clear All";
			this.cmiComponentHeadersClearAll.Click += new System.EventHandler(this.cmiComponentHeadersClearAll_Click);
			// 
			// lblComponentHeaders
			// 
			this.lblComponentHeaders.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblComponentHeaders.Location = new System.Drawing.Point(0, 0);
			this.lblComponentHeaders.Name = "lblComponentHeaders";
			this.lblComponentHeaders.Size = new System.Drawing.Size(160, 16);
			this.lblComponentHeaders.TabIndex = 6;
			this.lblComponentHeaders.Text = "Components:";
			this.lblComponentHeaders.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// spcChooser
			// 
			this.spcChooser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spcChooser.Location = new System.Drawing.Point(3, 3);
			this.spcChooser.Name = "spcChooser";
			// 
			// spcChooser.Panel1
			// 
			this.spcChooser.Panel1.Controls.Add(this.lstOtherHeaders);
			this.spcChooser.Panel1.Controls.Add(this.lblOtherHeaders);
			this.spcChooser.Panel1.Controls.Add(this.lstComponentHeaders);
			this.spcChooser.Panel1.Controls.Add(this.lblComponentHeaders);
			// 
			// spcChooser.Panel2
			// 
			this.spcChooser.Panel2.Controls.Add(this.lstOtherOrder);
			this.spcChooser.Panel2.Controls.Add(this.lblOtherOrder);
			this.spcChooser.Panel2.Controls.Add(this.lstComponentOrder);
			this.spcChooser.Panel2.Controls.Add(this.lblComponentOrder);
			this.spcChooser.Size = new System.Drawing.Size(328, 296);
			this.spcChooser.SplitterDistance = 160;
			this.spcChooser.TabIndex = 10;
			// 
			// lstOtherHeaders
			// 
			this.lstOtherHeaders.CheckOnClick = true;
			this.lstOtherHeaders.ContextMenuStrip = this.cmsOtherHeaders;
			this.lstOtherHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstOtherHeaders.Location = new System.Drawing.Point(0, 111);
			this.lstOtherHeaders.Name = "lstOtherHeaders";
			this.lstOtherHeaders.Size = new System.Drawing.Size(160, 184);
			this.lstOtherHeaders.TabIndex = 8;
			this.lstOtherHeaders.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckListBox_ItemCheck);
			// 
			// cmsOtherHeaders
			// 
			this.cmsOtherHeaders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiOtherHeadersRestoreDefaults,
            this.cmiOtherHeadersSelectAll,
            this.cmiOtherHeadersClearAll});
			this.cmsOtherHeaders.Name = "cmsHeaders";
			this.cmsOtherHeaders.Size = new System.Drawing.Size(167, 70);
			// 
			// cmiOtherHeadersRestoreDefaults
			// 
			this.cmiOtherHeadersRestoreDefaults.Name = "cmiOtherHeadersRestoreDefaults";
			this.cmiOtherHeadersRestoreDefaults.Size = new System.Drawing.Size(166, 22);
			this.cmiOtherHeadersRestoreDefaults.Text = "Restore Defaults";
			this.cmiOtherHeadersRestoreDefaults.Click += new System.EventHandler(this.cmiOtherHeadersRestoreDefaults_Click);
			// 
			// cmiOtherHeadersSelectAll
			// 
			this.cmiOtherHeadersSelectAll.Name = "cmiOtherHeadersSelectAll";
			this.cmiOtherHeadersSelectAll.Size = new System.Drawing.Size(166, 22);
			this.cmiOtherHeadersSelectAll.Text = "Select All";
			this.cmiOtherHeadersSelectAll.Click += new System.EventHandler(this.cmiOtherHeadersSelectAll_Click);
			// 
			// cmiOtherHeadersClearAll
			// 
			this.cmiOtherHeadersClearAll.Name = "cmiOtherHeadersClearAll";
			this.cmiOtherHeadersClearAll.Size = new System.Drawing.Size(166, 22);
			this.cmiOtherHeadersClearAll.Text = "Clear All";
			this.cmiOtherHeadersClearAll.Click += new System.EventHandler(this.cmiOtherHeadersClearAll_Click);
			// 
			// lblOtherHeaders
			// 
			this.lblOtherHeaders.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblOtherHeaders.Location = new System.Drawing.Point(0, 95);
			this.lblOtherHeaders.Name = "lblOtherHeaders";
			this.lblOtherHeaders.Size = new System.Drawing.Size(160, 16);
			this.lblOtherHeaders.TabIndex = 9;
			this.lblOtherHeaders.Text = "Other Displayable Items:";
			this.lblOtherHeaders.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lstOtherOrder
			// 
			this.lstOtherOrder.AllowDrop = true;
			this.lstOtherOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstOtherOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstOtherOrder.ItemHeight = 15;
			this.lstOtherOrder.Location = new System.Drawing.Point(0, 111);
			this.lstOtherOrder.Name = "lstOtherOrder";
			this.lstOtherOrder.Size = new System.Drawing.Size(164, 184);
			this.lstOtherOrder.TabIndex = 10;
			this.lstOtherOrder.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBox_DrawItem);
			this.lstOtherOrder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseUp);
			this.lstOtherOrder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseMove);
			this.lstOtherOrder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseDown);
			this.lstOtherOrder.DragOver += new System.Windows.Forms.DragEventHandler(this.ListBox_DragOver);
			// 
			// lblOtherOrder
			// 
			this.lblOtherOrder.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblOtherOrder.Location = new System.Drawing.Point(0, 95);
			this.lblOtherOrder.Name = "lblOtherOrder";
			this.lblOtherOrder.Size = new System.Drawing.Size(164, 16);
			this.lblOtherOrder.TabIndex = 11;
			this.lblOtherOrder.Text = "Order:";
			this.lblOtherOrder.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// RowColChooserComponentPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.spcChooser);
			this.Name = "RowColChooserComponentPanel";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.Size = new System.Drawing.Size(334, 302);
			this.Load += new System.EventHandler(this.RowColChooserComponentPanel_Load);
			this.cmsComponentHeaders.ResumeLayout(false);
			this.spcChooser.Panel1.ResumeLayout(false);
			this.spcChooser.Panel2.ResumeLayout(false);
			this.spcChooser.ResumeLayout(false);
			this.cmsOtherHeaders.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstComponentOrder;
		private System.Windows.Forms.Label lblComponentOrder;
		private System.Windows.Forms.CheckedListBox lstComponentHeaders;
		private System.Windows.Forms.Label lblComponentHeaders;
		private System.Windows.Forms.SplitContainer spcChooser;
		private System.Windows.Forms.ContextMenuStrip cmsComponentHeaders;
		private System.Windows.Forms.ToolStripMenuItem cmiComponentHeadersRestoreDefaults;
		private System.Windows.Forms.ToolStripMenuItem cmiComponentHeadersSelectAll;
		private System.Windows.Forms.ToolStripMenuItem cmiComponentHeadersClearAll;
		private System.Windows.Forms.CheckedListBox lstOtherHeaders;
		private System.Windows.Forms.ListBox lstOtherOrder;
		private System.Windows.Forms.Label lblOtherHeaders;
		private System.Windows.Forms.Label lblOtherOrder;
		private System.Windows.Forms.ContextMenuStrip cmsOtherHeaders;
		private System.Windows.Forms.ToolStripMenuItem cmiOtherHeadersRestoreDefaults;
		private System.Windows.Forms.ToolStripMenuItem cmiOtherHeadersSelectAll;
		private System.Windows.Forms.ToolStripMenuItem cmiOtherHeadersClearAll;

	}
}
