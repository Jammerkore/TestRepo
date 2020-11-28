namespace MIDRetail.Windows.Controls
{
    partial class PlanChainLadderGridControl
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
            this.ugGrid.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugGrid_InitializeLayout);
            this.ugGrid.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugGrid_InitializeRow);
            this.ugGrid.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugGrid_BeforeExitEditMode);
            this.ugGrid.BeforePerformAction -= new Infragistics.Win.UltraWinGrid.BeforeUltraGridPerformActionEventHandler(this.ugGrid_BeforePerformAction);
            this.ugGrid.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugGrid_KeyDown);
            this.ugGrid.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.ugGrid_KeyPress);
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.ugGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.ugTotal = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panelTop = new System.Windows.Forms.Panel();
            this.txtBoxTitle = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ugGrid)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugTotal)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // ugGrid
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugGrid.DisplayLayout.Appearance = appearance1;
            this.ugGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugGrid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ugGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugGrid.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ugGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.ugGrid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugGrid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ugGrid.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ugGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ugGrid.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugGrid.DisplayLayout.Override.CellAppearance = appearance8;
            this.ugGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugGrid.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ugGrid.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ugGrid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ugGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ugGrid.DisplayLayout.Override.RowAppearance = appearance11;
            this.ugGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ugGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ugGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugGrid.Location = new System.Drawing.Point(0, 0);
            this.ugGrid.Name = "ugGrid";
            this.ugGrid.Size = new System.Drawing.Size(687, 187);
            this.ugGrid.TabIndex = 0;
            this.ugGrid.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.ugGrid.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ugGrid.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ugGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugGrid_InitializeLayout);
            this.ugGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugGrid_InitializeRow);
            this.ugGrid.AfterGroupPosChanged += new Infragistics.Win.UltraWinGrid.AfterGroupPosChangedEventHandler(this.ugGrid_AfterGroupPosChanged);
            this.ugGrid.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugGrid_BeforeExitEditMode);
            this.ugGrid.BeforeGroupPosChanged += new Infragistics.Win.UltraWinGrid.BeforeGroupPosChangedEventHandler(this.ugGrid_BeforeGroupPosChanged);
            this.ugGrid.BeforePerformAction += new Infragistics.Win.UltraWinGrid.BeforeUltraGridPerformActionEventHandler(this.ugGrid_BeforePerformAction);
            this.ugGrid.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.ugGrid_AfterColPosChanged);
            this.ugGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugGrid_KeyDown);
            this.ugGrid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ugGrid_KeyPress);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.ugGrid);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.ugTotal);
            this.splitContainer.Size = new System.Drawing.Size(687, 375);
            this.splitContainer.SplitterDistance = 187;
            this.splitContainer.TabIndex = 1;
            this.splitContainer.DoubleClick += new System.EventHandler(this.splitContainer_DoubleClick);
            // 
            // ugTotal
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugTotal.DisplayLayout.Appearance = appearance13;
            this.ugTotal.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugTotal.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.ugTotal.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugTotal.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.ugTotal.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugTotal.DisplayLayout.GroupByBox.Hidden = true;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugTotal.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.ugTotal.DisplayLayout.MaxColScrollRegions = 1;
            this.ugTotal.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugTotal.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ugTotal.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.ugTotal.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugTotal.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.ugTotal.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugTotal.DisplayLayout.Override.CellAppearance = appearance20;
            this.ugTotal.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugTotal.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.ugTotal.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.ugTotal.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.ugTotal.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugTotal.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.ugTotal.DisplayLayout.Override.RowAppearance = appearance23;
            this.ugTotal.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ugTotal.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugTotal.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.ugTotal.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugTotal.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugTotal.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugTotal.Location = new System.Drawing.Point(0, 0);
            this.ugTotal.Name = "ugTotal";
            this.ugTotal.Size = new System.Drawing.Size(687, 184);
            this.ugTotal.TabIndex = 1;
            this.ugTotal.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.ugTotal.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ugTotal.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ugTotal.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugTotal_InitializeLayout);
            this.ugTotal.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugTotal_InitializeRow);
            this.ugTotal.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.ugTotal_AfterColRegionScroll);
            this.ugTotal.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugTotal_BeforeExitEditMode);
            this.ugTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ugTotal_KeyPress);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.txtBoxTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(687, 24);
            this.panelTop.TabIndex = 2;
            // 
            // txtBoxTitle
            // 
            this.txtBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTitle.Location = new System.Drawing.Point(133, 2);
            this.txtBoxTitle.Name = "txtBoxTitle";
            this.txtBoxTitle.Size = new System.Drawing.Size(221, 20);
            this.txtBoxTitle.TabIndex = 0;
            this.txtBoxTitle.TabStop = false;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.splitContainer);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 24);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(687, 375);
            this.panelBottom.TabIndex = 3;
            // 
            // PlanChainLadderGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "PlanChainLadderGridControl";
            this.Size = new System.Drawing.Size(687, 399);
            ((System.ComponentModel.ISupportInitialize)(this.ugGrid)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugTotal)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid ugGrid;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugTotal;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtBoxTitle;
        private System.Windows.Forms.Panel panelBottom;
    }
}
