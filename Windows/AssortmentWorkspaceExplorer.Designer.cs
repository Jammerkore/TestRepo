using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    partial class AssortmentWorkspaceExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Dispose
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            CheckSaveLayout();

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                //this.cboView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboView_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            }
            base.Dispose(disposing);
        }
        #endregion	
        #region Designer generated code
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
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet1 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartType");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet2 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartDock");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet3 = new Infragistics.Win.UltraWinToolbars.OptionSet("MessageLevel");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet4 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartLegendLocation");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet5 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartTitleLocation");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Allocation WorkspaceToolbar");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool19 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("reviewMenuPopup");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool20 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridMenuPopup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("searchButton");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("saveView");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("showDetails");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("headerCreateToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool7 = new Infragistics.Win.UltraWinToolbars.LabelTool("Header:");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("headerAdd");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool5 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("groupAllocationCreateTextBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("groupAllocationCreate");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar3 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Action Toolbar");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool3 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("actionComboBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAction");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar4 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("headerToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("headerLabel");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool3 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerTotalTextBox");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar5 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("quantityAllocateToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("quantityAllocateLabel");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool9 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool11 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateTotalTextBox");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar6 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("View Toolbar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAsrtView");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar7 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("headerFilterToolbar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAsrtFilter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("filter");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool6 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("groupAllocationCreateTextBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSelectionScreen");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool27 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("reviewMenuPopup");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssortmentWorkspaceExplorer));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSelectionScreen");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewAssortment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewProperties");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool28 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridMenuPopup");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridChooseColumns");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailPopupMenu");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool49 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowGroupArea", "");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool4 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("actionComboBox");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem16 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem17 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem18 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem19 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem20 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem21 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem22 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem23 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem24 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem25 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem26 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("searchButton");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("multiRemove");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewStyle");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool57 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowFilterRow", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSize");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridChooseColumns");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailAllRows");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailSelectedRows");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAction");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("headerLabel");
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool4 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerTotalTextBox");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("quantityAllocateLabel");
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool10 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool12 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateTotalTextBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("saveView");
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("filter");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("showDetails");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("groupAllocationCreate");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSummary");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool("multiHeaderLabel");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("multiCreate");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("multiAddTo");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("headerAdd");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("viewComboBox");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool8 = new Infragistics.Win.UltraWinToolbars.LabelTool("Header:");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("autoSelectGroup", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExport");
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailPopupMenu");
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewAssortment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewProperties");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool6 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("headerFilterComboBox");
            Infragistics.Win.ValueList valueList3 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAsrtView");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAsrtFilter");
            this.panel1 = new System.Windows.Forms.Panel();
            this.ugAssortments = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmsGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsReview = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewAssortment = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSaveView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.cmsColumnChooser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsColSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsColClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this._UserActivityControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.headerToolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._UserActivityControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._UserActivityControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._UserActivityControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.midComboBoxAsrtFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midComboBoxAsrtView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            ((System.ComponentModel.ISupportInitialize)(this.ugAssortments)).BeginInit();
            this.cmsGrid.SuspendLayout();
            this.cmsColumnChooser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerToolbarsManager)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 125);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 0);
            this.panel1.TabIndex = 4;
            // 
            // ugAssortments
            // 
            this.ugAssortments.AllowDrop = true;
            this.ugAssortments.ContextMenuStrip = this.cmsGrid;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugAssortments.DisplayLayout.Appearance = appearance1;
            this.ugAssortments.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugAssortments.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugAssortments.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugAssortments.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ugAssortments.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugAssortments.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ugAssortments.DisplayLayout.MaxColScrollRegions = 1;
            this.ugAssortments.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugAssortments.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            this.ugAssortments.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugAssortments.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.ugAssortments.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BorderColor = System.Drawing.Color.Silver;
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugAssortments.DisplayLayout.Override.CellAppearance = appearance7;
            this.ugAssortments.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugAssortments.DisplayLayout.Override.CellPadding = 0;
            appearance8.BackColor = System.Drawing.SystemColors.Control;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            this.ugAssortments.DisplayLayout.Override.GroupByRowAppearance = appearance8;
            this.ugAssortments.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            this.ugAssortments.DisplayLayout.Override.RowAppearance = appearance9;
            this.ugAssortments.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ugAssortments.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugAssortments.DisplayLayout.Override.TemplateAddRowAppearance = appearance10;
            this.ugAssortments.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugAssortments.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugAssortments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugAssortments.Location = new System.Drawing.Point(0, 125);
            this.ugAssortments.Name = "ugAssortments";
            this.ugAssortments.Size = new System.Drawing.Size(992, 475);
            this.ugAssortments.TabIndex = 5;
            this.ugAssortments.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAssortments_InitializeLayout);
            this.ugAssortments.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugAssortments_InitializeRow);
            this.ugAssortments.AfterRowsDeleted += new System.EventHandler(this.ugAssortments_AfterRowsDeleted);
            this.ugAssortments.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugAssortments_ClickCellButton);
            this.ugAssortments.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugAssortments_AfterSelectChange);
            this.ugAssortments.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ugAssortments_BeforeSelectChange);
            this.ugAssortments.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ugAssortments_SelectionDrag);
            this.ugAssortments.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugAssortments_BeforeRowsDeleted);
            this.ugAssortments.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.ugAssortments_DoubleClickRow);
            this.ugAssortments.BeforeSortChange += new Infragistics.Win.UltraWinGrid.BeforeSortChangeEventHandler(this.ugAssortments_BeforeSortChange);
            this.ugAssortments.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugAssortments_AfterSortChange);
            this.ugAssortments.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ugAssortments_BeforeColumnChooserDisplayed);
            this.ugAssortments.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugAssortments_DragEnter);
            this.ugAssortments.DragOver += new System.Windows.Forms.DragEventHandler(this.ugAssortments_DragOver);
            this.ugAssortments.DragLeave += new System.EventHandler(this.ugAssortments_DragLeave);
            this.ugAssortments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugAssortments_KeyDown);
            this.ugAssortments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugAssortments_MouseDown);
            // 
            // cmsGrid
            // 
            this.cmsGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.cmsGrid.AllowMerge = false;
            this.cmsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsReview,
            this.cmsFilter,
            this.cmsSearch,
            this.cmsSeparator1,
            this.cmsDelete,
            this.cmsCopy,
            this.cmsSaveView});
            this.cmsGrid.Name = "cmsGrid";
            this.cmsGrid.ShowCheckMargin = true;
            this.cmsGrid.ShowImageMargin = false;
            this.cmsGrid.Size = new System.Drawing.Size(127, 142);
            this.cmsGrid.Opening += new System.ComponentModel.CancelEventHandler(this.cmsGrid_Opening);
            // 
            // cmsReview
            // 
            this.cmsReview.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsReview.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsReviewSelect,
            //this.cmsReviewAssortment,  //TT#2016-MD - AGallagher - Assortment Review Navigation - remove right click Assoerment
            this.cmsReviewProperties});
            this.cmsReview.Name = "cmsReview";
            this.cmsReview.Size = new System.Drawing.Size(126, 22);
            this.cmsReview.Text = "Review";
            // 
            // cmsReviewSelect
            // 
            this.cmsReviewSelect.Name = "cmsReviewSelect";
            this.cmsReviewSelect.Size = new System.Drawing.Size(135, 22);
            this.cmsReviewSelect.Text = "Select";
            this.cmsReviewSelect.Click += new System.EventHandler(this.cmsReviewSelect_Click);
            // 
            // cmsReviewAssortment
            // 
            this.cmsReviewAssortment.Name = "cmsReviewAssortment";
            this.cmsReviewAssortment.Size = new System.Drawing.Size(135, 22);
            this.cmsReviewAssortment.Text = "Assortment";
            this.cmsReviewAssortment.Click += new System.EventHandler(this.cmsReviewAssortment_Click);
            // 
            // cmsReviewProperties
            // 
            this.cmsReviewProperties.Name = "cmsReviewProperties";
            this.cmsReviewProperties.Size = new System.Drawing.Size(135, 22);
            this.cmsReviewProperties.Text = "Properties";
            this.cmsReviewProperties.Click += new System.EventHandler(this.cmsReviewProperties_Click);
            // 
            // cmsFilter
            // 
            this.cmsFilter.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(126, 22);
            this.cmsFilter.Text = "Filter";
            this.cmsFilter.Click += new System.EventHandler(this.cmsFilter_Click);
            // 
            // cmsSearch
            // 
            this.cmsSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsSearch.Name = "cmsSearch";
            this.cmsSearch.Size = new System.Drawing.Size(126, 22);
            this.cmsSearch.Text = "Search";
            this.cmsSearch.Click += new System.EventHandler(this.cmsSearch_Click);
            // 
            // cmsSeparator1
            // 
            this.cmsSeparator1.Name = "cmsSeparator1";
            this.cmsSeparator1.Size = new System.Drawing.Size(123, 6);
            // 
            // cmsDelete
            // 
            this.cmsDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.Size = new System.Drawing.Size(126, 22);
            this.cmsDelete.Text = "Delete";
            this.cmsDelete.Click += new System.EventHandler(this.cmsDelete_Click);
            // 
            // cmsCopy
            // 
            this.cmsCopy.Name = "cmsCopy";
            this.cmsCopy.Size = new System.Drawing.Size(126, 22);
            this.cmsCopy.Text = "Copy";
            // 
            // cmsSaveView
            // 
            this.cmsSaveView.Name = "cmsSaveView";
            this.cmsSaveView.Size = new System.Drawing.Size(126, 22);
            this.cmsSaveView.Text = "Save View";
            this.cmsSaveView.Click += new System.EventHandler(this.cmsSaveView_Click);  // TT#1990-MD - JSmith - Assortment - Save View 
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmsColumnChooser
            // 
            this.cmsColumnChooser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsColSelectAll,
            this.cmsColClearAll});
            this.cmsColumnChooser.Name = "cmsColumnChooser";
            this.cmsColumnChooser.Size = new System.Drawing.Size(123, 48);
            // 
            // cmsColSelectAll
            // 
            this.cmsColSelectAll.Name = "cmsColSelectAll";
            this.cmsColSelectAll.Size = new System.Drawing.Size(122, 22);
            this.cmsColSelectAll.Text = "Select All";
            this.cmsColSelectAll.Click += new System.EventHandler(this.cmsColSelectAll_Click);
            // 
            // cmsColClearAll
            // 
            this.cmsColClearAll.Name = "cmsColClearAll";
            this.cmsColClearAll.Size = new System.Drawing.Size(122, 22);
            this.cmsColClearAll.Text = "Clear All";
            this.cmsColClearAll.Click += new System.EventHandler(this.cmsColClearAll_Click);
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Top
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._UserActivityControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._UserActivityControl_Toolbars_Dock_Area_Top.Name = "_UserActivityControl_Toolbars_Dock_Area_Top";
            this._UserActivityControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(992, 125);
            this._UserActivityControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.headerToolbarsManager;
            // 
            // headerToolbarsManager
            // 
            this.headerToolbarsManager.DesignerFlags = 1;
            this.headerToolbarsManager.DockWithinContainer = this.panel1;
            this.headerToolbarsManager.MenuAnimationStyle = Infragistics.Win.UltraWinToolbars.MenuAnimationStyle.Slide;
            this.headerToolbarsManager.MenuSettings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
            optionSet1.AllowAllUp = false;
            optionSet4.AllowAllUp = false;
            optionSet5.AllowAllUp = false;
            this.headerToolbarsManager.OptionSets.Add(optionSet1);
            this.headerToolbarsManager.OptionSets.Add(optionSet2);
            this.headerToolbarsManager.OptionSets.Add(optionSet3);
            this.headerToolbarsManager.OptionSets.Add(optionSet4);
            this.headerToolbarsManager.OptionSets.Add(optionSet5);
            this.headerToolbarsManager.RuntimeCustomizationOptions = ((Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions)((Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.AllowCustomizeDialog | Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.AllowAltClickToolDragging)));
            this.headerToolbarsManager.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 2;
            popupMenuTool20.InstanceProps.IsFirstInGroup = true;
            buttonTool43.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool19,
            popupMenuTool20,
            buttonTool42,
            buttonTool43,
            buttonTool44});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar1.Settings.ToolSpacing = 4;
            ultraToolbar1.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Text = "Allocation Workspace Toolbar";
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 3;
            ultraToolbar2.FloatingSize = new System.Drawing.Size(365, 24);
            ultraToolbar2.IsStockToolbar = false;
            textBoxTool5.InstanceProps.IsFirstInGroup = true;
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool7,
            buttonTool46,
            textBoxTool5,
            buttonTool15});
            ultraToolbar2.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar2.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar2.ShowInToolbarList = false;
            ultraToolbar2.Text = "Header Create Toolbar";
            ultraToolbar2.Visible = false;
            ultraToolbar3.DockedColumn = 0;
            ultraToolbar3.DockedRow = 1;
            ultraToolbar3.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            comboBoxTool3,
            buttonTool7});
            ultraToolbar3.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar3.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar3.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar3.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar3.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar3.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar3.ShowInToolbarList = false;
            ultraToolbar3.Text = "Action Toolbar";
            ultraToolbar3.Visible = false;
            ultraToolbar4.DockedColumn = 0;
            ultraToolbar4.DockedRow = 0;
            ultraToolbar4.FloatingSize = new System.Drawing.Size(534, 24);
            textBoxTool3.InstanceProps.Width = 133;
            ultraToolbar4.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool1,
            textBoxTool1,
            textBoxTool3});
            ultraToolbar4.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar4.Text = "Header Toolbar";
            ultraToolbar5.DockedColumn = 0;
            ultraToolbar5.DockedRow = 1;
            textBoxTool11.InstanceProps.Width = 133;
            ultraToolbar5.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool3,
            textBoxTool9,
            textBoxTool11});
            ultraToolbar5.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar5.Text = "Qty to Allocate Toolbar";
            ultraToolbar6.DockedColumn = 1;
            ultraToolbar6.DockedRow = 2;
            controlContainerTool1.ControlName = "midComboBoxAsrtView";
            ultraToolbar6.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool1});
            ultraToolbar6.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar6.Text = "View Toolbar";
            ultraToolbar7.DockedColumn = 0;
            ultraToolbar7.DockedRow = 3;
            controlContainerTool3.ControlName = "midComboBoxAsrtFilter";
            ultraToolbar7.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool3,
            buttonTool18});
            ultraToolbar7.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar7.Text = "Filter Toolbar";
            this.headerToolbarsManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1,
            ultraToolbar2,
            ultraToolbar3,
            ultraToolbar4,
            ultraToolbar5,
            ultraToolbar6,
            ultraToolbar7});
            textBoxTool6.SharedPropsInternal.Caption = "Group Allocation:";
            textBoxTool6.SharedPropsInternal.Category = "Grid Search";
            textBoxTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool6.SharedPropsInternal.Width = 250;
            buttonTool19.SharedPropsInternal.Caption = "Selection Screen";
            buttonTool19.SharedPropsInternal.Category = "Review";
            buttonTool19.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            popupMenuTool27.SharedPropsInternal.AppearancesSmall.Appearance = appearance11;
            popupMenuTool27.SharedPropsInternal.Caption = "Review";
            popupMenuTool27.SharedPropsInternal.Category = "Review";
            popupMenuTool27.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool27.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool21,
            buttonTool9,
            buttonTool17});
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            popupMenuTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            popupMenuTool28.SharedPropsInternal.Caption = "Grid";
            popupMenuTool28.SharedPropsInternal.Category = "Grid";
            popupMenuTool28.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool25.InstanceProps.IsFirstInGroup = true;
            popupMenuTool28.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool25,
            buttonTool1,
            buttonTool11});
            stateButtonTool49.Checked = true;
            stateButtonTool49.SharedPropsInternal.Caption = "Show Group Area";
            stateButtonTool49.SharedPropsInternal.Category = "Grid";
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            comboBoxTool4.EditAppearance = appearance13;
            comboBoxTool4.SharedPropsInternal.Caption = "Action:";
            comboBoxTool4.SharedPropsInternal.Category = "Action";
            comboBoxTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool4.SharedPropsInternal.Width = 275;
            valueList1.DropDownListMinWidth = 250;
            valueList1.DropDownListWidth = 250;
            valueList1.MaxDropDownItems = 30;
            valueListItem2.DataValue = "Select an action...";
            valueListItem2.DisplayText = "Select an action...";
            valueListItem3.DataValue = "Reapply Total Allocation";
            valueListItem3.DisplayText = "Reapply Total Allocation";
            valueListItem4.DataValue = "Need";
            valueListItem4.DisplayText = "Need";
            valueListItem5.DataValue = "Size Proportional";
            valueListItem5.DisplayText = "Size Proportional";
            valueListItem11.DataValue = "Size Proportional with Constraints";
            valueListItem11.DisplayText = "Size Proportional with Constraints";
            valueListItem12.DataValue = "Balance Proportional";
            valueListItem12.DisplayText = "Balance Proportional";
            valueListItem13.DataValue = "Balance to Reserve";
            valueListItem13.DisplayText = "Balance to Reserve";
            valueListItem14.DataValue = "Balance Size Proportional";
            valueListItem14.DisplayText = "Balance Size Proportional";
            valueListItem15.DataValue = "Balance Size to Reserve";
            valueListItem15.DisplayText = "Balance Size to Reserve";
            valueListItem16.DataValue = "Balance Size with Constraints";
            valueListItem16.DisplayText = "Balance Size with Constraints";
            valueListItem17.DataValue = "Balance Size Bilaterally";
            valueListItem17.DisplayText = "Balance Size Bilaterally";
            valueListItem18.DataValue = "Charge Intransit";
            valueListItem18.DisplayText = "Charge Intransit";
            valueListItem19.DataValue = "Release";
            valueListItem19.DisplayText = "Release";
            valueListItem20.DataValue = "Reset";
            valueListItem20.DisplayText = "Reset";
            valueListItem21.DataValue = "Cancel Size Intransit";
            valueListItem21.DisplayText = "Cancel Size Intransit";
            valueListItem22.DataValue = "Cancel Intransit";
            valueListItem22.DisplayText = "Cancel Intransit";
            valueListItem23.DataValue = "Cancel Size Allocation";
            valueListItem23.DisplayText = "Cancel Size Allocation";
            valueListItem24.DataValue = "Cancel Allocation";
            valueListItem24.DisplayText = "Cancel Allocation";
            valueListItem25.DataValue = "Remove API Workflow";
            valueListItem25.DisplayText = "Remove API Workflow";
            valueListItem26.DataValue = "Apply API Workflow";
            valueListItem26.DisplayText = "Apply API Workflow";
            valueList1.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5,
            valueListItem11,
            valueListItem12,
            valueListItem13,
            valueListItem14,
            valueListItem15,
            valueListItem16,
            valueListItem17,
            valueListItem18,
            valueListItem19,
            valueListItem20,
            valueListItem21,
            valueListItem22,
            valueListItem23,
            valueListItem24,
            valueListItem25,
            valueListItem26});
            comboBoxTool4.ValueList = valueList1;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            buttonTool26.SharedPropsInternal.AppearancesSmall.Appearance = appearance14;
            buttonTool26.SharedPropsInternal.Caption = "Search";
            buttonTool26.SharedPropsInternal.Category = "Misc";
            buttonTool26.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            buttonTool27.SharedPropsInternal.AppearancesSmall.Appearance = appearance15;
            buttonTool27.SharedPropsInternal.Caption = "Remove from";
            buttonTool27.SharedPropsInternal.Category = "Grid Search";
            buttonTool27.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool30.SharedPropsInternal.Caption = "Style";
            buttonTool30.SharedPropsInternal.Category = "Review";
            stateButtonTool57.Checked = true;
            stateButtonTool57.SharedPropsInternal.Caption = "Show Filter Row";
            stateButtonTool57.SharedPropsInternal.Category = "Grid";
            buttonTool34.SharedPropsInternal.Caption = "Size";
            buttonTool34.SharedPropsInternal.Category = "Review";
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            buttonTool35.SharedPropsInternal.AppearancesSmall.Appearance = appearance16;
            buttonTool35.SharedPropsInternal.Caption = "Choose Columns";
            buttonTool35.SharedPropsInternal.Category = "Grid";
            buttonTool4.SharedPropsInternal.Caption = "Email All Rows";
            buttonTool4.SharedPropsInternal.Category = "Grid Export";
            buttonTool6.SharedPropsInternal.Caption = "Email Selected Rows";
            buttonTool6.SharedPropsInternal.Category = "Grid Export";
            appearance17.Image = ((object)(resources.GetObject("appearance17.Image")));
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance17;
            buttonTool8.SharedPropsInternal.Caption = "Process";
            buttonTool8.SharedPropsInternal.Category = "Action";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance18.FontData.BoldAsString = "True";
            appearance18.FontData.ItalicAsString = "True";
            appearance18.TextHAlignAsString = "Left";
            labelTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance18;
            labelTool2.SharedPropsInternal.Caption = "Headers";
            labelTool2.SharedPropsInternal.Category = "Headers";
            labelTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            labelTool2.SharedPropsInternal.Enabled = false;
            labelTool2.SharedPropsInternal.MinWidth = 160;
            labelTool2.SharedPropsInternal.Spring = true;
            labelTool2.SharedPropsInternal.Width = 160;
            textBoxTool2.SharedPropsInternal.Caption = "Selected:";
            textBoxTool2.SharedPropsInternal.Category = "Headers";
            textBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool2.SharedPropsInternal.Enabled = false;
            textBoxTool4.SharedPropsInternal.Caption = "Total:";
            textBoxTool4.SharedPropsInternal.Category = "Headers";
            textBoxTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool4.SharedPropsInternal.Enabled = false;
            appearance19.FontData.BoldAsString = "True";
            appearance19.FontData.ItalicAsString = "True";
            appearance19.TextHAlignAsString = "Left";
            labelTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance19;
            labelTool4.SharedPropsInternal.Caption = "Quantity To Allocate";
            labelTool4.SharedPropsInternal.Category = "QuantityAllocate";
            labelTool4.SharedPropsInternal.Enabled = false;
            labelTool4.SharedPropsInternal.MinWidth = 160;
            labelTool4.SharedPropsInternal.Spring = true;
            labelTool4.SharedPropsInternal.Width = 160;
            textBoxTool10.SharedPropsInternal.Caption = "Selected:";
            textBoxTool10.SharedPropsInternal.Category = "QuantityAllocate";
            textBoxTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool10.SharedPropsInternal.Enabled = false;
            textBoxTool12.SharedPropsInternal.Caption = "Total:";
            textBoxTool12.SharedPropsInternal.Category = "QuantityAllocate";
            textBoxTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool12.SharedPropsInternal.Enabled = false;
            appearance20.Image = ((object)(resources.GetObject("appearance20.Image")));
            buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance20;
            buttonTool10.SharedPropsInternal.Caption = "Save View";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance21.Image = ((object)(resources.GetObject("appearance21.Image")));
            buttonTool12.SharedPropsInternal.AppearancesSmall.Appearance = appearance21;
            buttonTool12.SharedPropsInternal.Caption = "Edit Filter";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance22.Image = ((object)(resources.GetObject("appearance22.Image")));
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance22;
            buttonTool14.SharedPropsInternal.Caption = "Show Details";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool14.SharedPropsInternal.Enabled = false;
            buttonTool14.SharedPropsInternal.Visible = false;
            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance23;
            buttonTool16.SharedPropsInternal.Caption = "Create";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool20.SharedPropsInternal.Caption = "Summary";
            buttonTool20.SharedPropsInternal.Category = "Review";
            appearance24.TextHAlignAsString = "Left";
            labelTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance24;
            labelTool6.SharedPropsInternal.Caption = "Multi-Header:";
            labelTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            appearance25.Image = ((object)(resources.GetObject("appearance25.Image")));
            buttonTool38.SharedPropsInternal.AppearancesSmall.Appearance = appearance25;
            buttonTool38.SharedPropsInternal.Caption = "Create";
            buttonTool38.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            buttonTool40.SharedPropsInternal.AppearancesSmall.Appearance = appearance26;
            buttonTool40.SharedPropsInternal.Caption = "Add to";
            buttonTool40.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            buttonTool47.SharedPropsInternal.AppearancesSmall.Appearance = appearance27;
            buttonTool47.SharedPropsInternal.Caption = "Add";
            buttonTool47.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance28.BackColor = System.Drawing.SystemColors.Control;
            comboBoxTool2.EditAppearance = appearance28;
            appearance29.BackColor = System.Drawing.SystemColors.Control;
            comboBoxTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance29;
            comboBoxTool2.SharedPropsInternal.Caption = "View:";
            comboBoxTool2.SharedPropsInternal.Category = "View";
            comboBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool2.SharedPropsInternal.Width = 250;
            comboBoxTool2.ValueList = valueList2;
            labelTool8.SharedPropsInternal.Caption = "Header:";
            labelTool8.SharedPropsInternal.Category = "Add";
            labelTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            stateButtonTool2.SharedPropsInternal.Caption = "Auto Select Group";
            stateButtonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance30.Image = ((object)(resources.GetObject("appearance30.Image")));
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance30;
            buttonTool2.SharedPropsInternal.Caption = "Export";
            buttonTool2.SharedPropsInternal.Category = "Grid Export";
            appearance31.Image = ((object)(resources.GetObject("appearance31.Image")));
            buttonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance31;
            buttonTool13.SharedPropsInternal.Caption = "Email";
            buttonTool13.SharedPropsInternal.Category = "Grid Export";
            buttonTool3.SharedPropsInternal.Caption = "Assortment";
            buttonTool3.SharedPropsInternal.Category = "Review";
            buttonTool5.SharedPropsInternal.Caption = "Properties";
            buttonTool5.SharedPropsInternal.Category = "Review";
            comboBoxTool6.SharedPropsInternal.Caption = "Filter:";
            comboBoxTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool6.SharedPropsInternal.Width = 250;
            comboBoxTool6.ValueList = valueList3;
            controlContainerTool2.ControlName = "midComboBoxAsrtView";
            controlContainerTool2.SharedPropsInternal.Caption = "View:";
            controlContainerTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool4.ControlName = "midComboBoxAsrtFilter";
            controlContainerTool4.SharedPropsInternal.Caption = "Filter:";
            controlContainerTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.headerToolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            textBoxTool6,
            buttonTool19,
            popupMenuTool27,
            popupMenuTool28,
            stateButtonTool49,
            comboBoxTool4,
            buttonTool26,
            buttonTool27,
            buttonTool30,
            stateButtonTool57,
            buttonTool34,
            buttonTool35,
            buttonTool4,
            buttonTool6,
            buttonTool8,
            labelTool2,
            textBoxTool2,
            textBoxTool4,
            labelTool4,
            textBoxTool10,
            textBoxTool12,
            buttonTool10,
            buttonTool12,
            buttonTool14,
            buttonTool16,
            buttonTool20,
            labelTool6,
            buttonTool38,
            buttonTool40,
            buttonTool47,
            comboBoxTool2,
            labelTool8,
            stateButtonTool2,
            buttonTool2,
            buttonTool13,
            buttonTool3,
            buttonTool5,
            comboBoxTool6,
            controlContainerTool2,
            controlContainerTool4});
            this.headerToolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.headerToolbarsManager_ToolClick);
            this.headerToolbarsManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.headerToolbarsManager_ValueChanged);
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Bottom
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 600);
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.Name = "_UserActivityControl_Toolbars_Dock_Area_Bottom";
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(992, 0);
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.headerToolbarsManager;
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Left
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._UserActivityControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 125);
            this._UserActivityControl_Toolbars_Dock_Area_Left.Name = "_UserActivityControl_Toolbars_Dock_Area_Left";
            this._UserActivityControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 475);
            this._UserActivityControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.headerToolbarsManager;
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Right
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._UserActivityControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(992, 125);
            this._UserActivityControl_Toolbars_Dock_Area_Right.Name = "_UserActivityControl_Toolbars_Dock_Area_Right";
            this._UserActivityControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 475);
            this._UserActivityControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.headerToolbarsManager;
            // 
            // midComboBoxAsrtFilter
            // 
            this.midComboBoxAsrtFilter.AutoAdjust = true;
            this.midComboBoxAsrtFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxAsrtFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxAsrtFilter.DataSource = null;
            this.midComboBoxAsrtFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxAsrtFilter.DropDownWidth = 240;
            this.midComboBoxAsrtFilter.FormattingEnabled = false;
            this.midComboBoxAsrtFilter.IgnoreFocusLost = false;
            this.midComboBoxAsrtFilter.ItemHeight = 13;
            this.midComboBoxAsrtFilter.Location = new System.Drawing.Point(752, 4);
            this.midComboBoxAsrtFilter.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxAsrtFilter.MaxDropDownItems = 25;
            this.midComboBoxAsrtFilter.Name = "midComboBoxAsrtFilter";
            this.midComboBoxAsrtFilter.SetToolTip = "";
            this.midComboBoxAsrtFilter.Size = new System.Drawing.Size(240, 23);
            this.midComboBoxAsrtFilter.TabIndex = 10;
            this.midComboBoxAsrtFilter.Tag = null;
            this.midComboBoxAsrtFilter.Visible = false;
            this.midComboBoxAsrtFilter.SelectionChangeCommitted += new System.EventHandler(this.assortmentMidComboBoxFilter_SelectionChangeCommitted);
            // 
            // midComboBoxAsrtView
            // 
            this.midComboBoxAsrtView.AutoAdjust = true;
            this.midComboBoxAsrtView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxAsrtView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxAsrtView.DataSource = null;
            this.midComboBoxAsrtView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxAsrtView.DropDownWidth = 240;
            this.midComboBoxAsrtView.FormattingEnabled = false;
            this.midComboBoxAsrtView.IgnoreFocusLost = false;
            this.midComboBoxAsrtView.ItemHeight = 13;
            this.midComboBoxAsrtView.Location = new System.Drawing.Point(752, 27);
            this.midComboBoxAsrtView.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxAsrtView.MaxDropDownItems = 25;
            this.midComboBoxAsrtView.Name = "midComboBoxAsrtView";
            this.midComboBoxAsrtView.SetToolTip = "";
            this.midComboBoxAsrtView.Size = new System.Drawing.Size(240, 23);
            this.midComboBoxAsrtView.TabIndex = 11;
            this.midComboBoxAsrtView.Tag = null;
            this.midComboBoxAsrtView.Visible = false;
            this.midComboBoxAsrtView.SelectionChangeCommitted += new System.EventHandler(this.assortmentMidComboBoxEnhView_SelectionChangeCommitted);
            // 
            // AssortmentWorkspaceExplorer
            // 
            this.Controls.Add(this.midComboBoxAsrtView);
            this.Controls.Add(this.midComboBoxAsrtFilter);
            this.Controls.Add(this.ugAssortments);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Top);
            this.Name = "AssortmentWorkspaceExplorer";
            this.Size = new System.Drawing.Size(992, 600);
            this.Load += new System.EventHandler(this.AssortmentWorkspaceExplorer_Load);
            this.Enter += new System.EventHandler(this.AssortmentWorkspaceExplorer_Enter);
            this.Leave += new System.EventHandler(this.AssortmentWorkspaceExplorer_Leave);
            this.Controls.SetChildIndex(this._UserActivityControl_Toolbars_Dock_Area_Top, 0);
            this.Controls.SetChildIndex(this._UserActivityControl_Toolbars_Dock_Area_Bottom, 0);
            this.Controls.SetChildIndex(this._UserActivityControl_Toolbars_Dock_Area_Right, 0);
            this.Controls.SetChildIndex(this._UserActivityControl_Toolbars_Dock_Area_Left, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.ugAssortments, 0);
            this.Controls.SetChildIndex(this.midComboBoxAsrtFilter, 0);
            this.Controls.SetChildIndex(this.midComboBoxAsrtView, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ugAssortments)).EndInit();
            this.cmsGrid.ResumeLayout(false);
            this.cmsColumnChooser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerToolbarsManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

		private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugAssortments;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList imageListDrag;
        private System.Windows.Forms.ContextMenuStrip cmsGrid;
        private System.Windows.Forms.ToolStripMenuItem cmsReview;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem cmsSearch;
        private System.Windows.Forms.ToolStripMenuItem cmsSaveView;
        private System.Windows.Forms.ToolStripMenuItem cmsDelete;
        private System.Windows.Forms.ToolStripMenuItem cmsCopy;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewSelect;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewAssortment;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewProperties;
        private System.Windows.Forms.ContextMenuStrip cmsColumnChooser;
        private System.Windows.Forms.ToolStripMenuItem cmsColSelectAll;
        private System.Windows.Forms.ToolStripMenuItem cmsColClearAll;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager headerToolbarsManager;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Bottom;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Right;
        private MIDComboBoxEnh midComboBoxAsrtFilter;
        private MIDComboBoxEnh midComboBoxAsrtView;
    }
}
