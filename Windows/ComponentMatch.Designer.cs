namespace MIDRetail.Windows
{
    partial class ComponentMatch
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDisconnectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ugLinkGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ugHdrGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.cmsMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugLinkGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugHdrGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsConnect,
            this.cmsDisconnect,
            this.cmsDisconnectAll});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.ShowImageMargin = false;
            this.cmsMenu.Size = new System.Drawing.Size(127, 70);
            this.cmsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMenu_Opening);
            // 
            // cmsConnect
            // 
            this.cmsConnect.Name = "cmsConnect";
            this.cmsConnect.Size = new System.Drawing.Size(126, 22);
            this.cmsConnect.Text = "Connect";
            this.cmsConnect.Click += new System.EventHandler(this.cmsConnect_Click);
            // 
            // cmsDisconnect
            // 
            this.cmsDisconnect.Name = "cmsDisconnect";
            this.cmsDisconnect.Size = new System.Drawing.Size(126, 22);
            this.cmsDisconnect.Text = "Disconnect";
            this.cmsDisconnect.Click += new System.EventHandler(this.cmsDisconnect_Click);
            // 
            // cmsDisconnectAll
            // 
            this.cmsDisconnectAll.Name = "cmsDisconnectAll";
            this.cmsDisconnectAll.Size = new System.Drawing.Size(126, 22);
            this.cmsDisconnectAll.Text = "Disconnect All";
            this.cmsDisconnectAll.Click += new System.EventHandler(this.cmsDisconnectAll_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Size = new System.Drawing.Size(762, 396);
            this.splitContainer1.SplitterDistance = 335;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ugLinkGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ugHdrGrid);
            this.splitContainer2.Size = new System.Drawing.Size(762, 335);
            this.splitContainer2.SplitterDistance = 408;
            this.splitContainer2.TabIndex = 0;
            // 
            // ugLinkGrid
            // 
            this.ugLinkGrid.AllowDrop = true;
            this.ugLinkGrid.ContextMenuStrip = this.cmsMenu;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugLinkGrid.DisplayLayout.Appearance = appearance1;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugLinkGrid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugLinkGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ugLinkGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugLinkGrid.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugLinkGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ugLinkGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.ugLinkGrid.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugLinkGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugLinkGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            this.ugLinkGrid.DisplayLayout.Override.CardAreaAppearance = appearance5;
            appearance6.BorderColor = System.Drawing.Color.Silver;
            appearance6.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugLinkGrid.DisplayLayout.Override.CellAppearance = appearance6;
            this.ugLinkGrid.DisplayLayout.Override.CellPadding = 0;
            appearance7.BackColor = System.Drawing.SystemColors.Control;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.BorderColor = System.Drawing.SystemColors.Window;
            this.ugLinkGrid.DisplayLayout.Override.GroupByRowAppearance = appearance7;
            this.ugLinkGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            this.ugLinkGrid.DisplayLayout.Override.RowAppearance = appearance8;
            this.ugLinkGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugLinkGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance9;
            this.ugLinkGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugLinkGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugLinkGrid.Location = new System.Drawing.Point(0, 0);
            this.ugLinkGrid.Name = "ugLinkGrid";
            this.ugLinkGrid.Size = new System.Drawing.Size(408, 335);
            this.ugLinkGrid.TabIndex = 0;
            this.ugLinkGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugLinkGrid_InitializeRow);
            this.ugLinkGrid.DragLeave += new System.EventHandler(this.ugLinkGrid_DragLeave);
            this.ugLinkGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugLinkGrid_MouseDown);
            this.ugLinkGrid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugLinkGrid_AfterSelectChange);
            this.ugLinkGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.ugLinkGrid_DragOver);
            this.ugLinkGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugLinkGrid_InitializeLayout);
            this.ugLinkGrid.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugLinkGrid_CellChange);
            this.ugLinkGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugLinkGrid_DragDrop);
            this.ugLinkGrid.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugLinkGrid_AfterCellUpdate);
            this.ugLinkGrid.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugLinkGrid_DragEnter);
            // 
            // ugHdrGrid
            // 
            this.ugHdrGrid.ContextMenuStrip = this.cmsMenu;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugHdrGrid.DisplayLayout.Appearance = appearance10;
            appearance11.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.ugHdrGrid.DisplayLayout.GroupByBox.Appearance = appearance11;
            appearance12.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugHdrGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance12;
            this.ugHdrGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugHdrGrid.DisplayLayout.GroupByBox.Hidden = true;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugHdrGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance13;
            this.ugHdrGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.ugHdrGrid.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugHdrGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugHdrGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            this.ugHdrGrid.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BorderColor = System.Drawing.Color.Silver;
            appearance15.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugHdrGrid.DisplayLayout.Override.CellAppearance = appearance15;
            this.ugHdrGrid.DisplayLayout.Override.CellPadding = 0;
            appearance16.BackColor = System.Drawing.SystemColors.Control;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.ugHdrGrid.DisplayLayout.Override.GroupByRowAppearance = appearance16;
            this.ugHdrGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.Color.Silver;
            this.ugHdrGrid.DisplayLayout.Override.RowAppearance = appearance17;
            this.ugHdrGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugHdrGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance18;
            this.ugHdrGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugHdrGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugHdrGrid.Location = new System.Drawing.Point(0, 0);
            this.ugHdrGrid.Name = "ugHdrGrid";
            this.ugHdrGrid.Size = new System.Drawing.Size(350, 335);
            this.ugHdrGrid.TabIndex = 0;
            this.ugHdrGrid.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ugHdrGrid_SelectionDrag);
            this.ugHdrGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugHdrGrid_InitializeRow);
            this.ugHdrGrid.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ugHdrGrid_BeforeSelectChange);
            this.ugHdrGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugHdrGrid_MouseDown);
            this.ugHdrGrid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugHdrGrid_AfterSelectChange);
            this.ugHdrGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.ugHdrGrid_DragOver);
            this.ugHdrGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugHdrGrid_InitializeLayout);
            this.ugHdrGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugHdrGrid_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(675, 21);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(542, 21);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ComponentMatch
            // 
            this.AllowDragDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 396);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ComponentMatch";
            this.Text = "ComponentMatch";
            this.Load += new System.EventHandler(this.ComponentMatch_Load);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.cmsMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugLinkGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugHdrGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem cmsConnect;
        private System.Windows.Forms.ToolStripMenuItem cmsDisconnect;
        private System.Windows.Forms.ToolStripMenuItem cmsDisconnectAll;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugLinkGrid;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugHdrGrid;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imageListDrag;
    }
}