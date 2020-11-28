using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;  // TT#1185 - Verify ENQ before Update
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Documents.Excel;
using Infragistics.Win.UltraWinGrid.ExcelExport;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    partial class AllocationWorkspaceExplorer
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
                DequeueHeaders();  // TT#1185 - Verify ENQ before Update (remove any Header ENQ that may be lingering around) 
                //_headerEnqueue = null; // TT#1185 - Verify ENQ before Update
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
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet6 = new Infragistics.Win.UltraWinToolbars.OptionSet("MessageLevel");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet7 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartType");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet8 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartLegendLocation");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet9 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartDock");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet10 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartTitleLocation");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar9 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Allocation WorkspaceToolbar");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool19 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("reviewMenuPopup");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool20 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridMenuPopup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("searchButton");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("saveView");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("showDetails");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar10 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("headerCreateToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool7 = new Infragistics.Win.UltraWinToolbars.LabelTool("Header:");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("headerAdd");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool5 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("groupAllocationCreateTextBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("groupAllocationCreate");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnDelete");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar11 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Action Toolbar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAction");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar12 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("headerToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("headerLabel");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool3 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerTotalTextBox");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar13 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("quantityAllocateToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("quantityAllocateLabel");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool9 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool11 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateTotalTextBox");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar14 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("View Toolbar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerView");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar15 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("headerFilterToolbar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool5 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerFilter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("filter");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool6 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("groupAllocationCreateTextBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSelectionScreen");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool27 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("reviewMenuPopup");
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationWorkspaceExplorer));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSelectionScreen");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewStyle");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSize");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSummary");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool28 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridMenuPopup");
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("autoSelectGroup", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool43 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowGroupArea", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool44 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowFilterRow", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridChooseColumns");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailPopupMenu");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool49 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowGroupArea", "");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool4 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("actionComboBox");
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList4 = new Infragistics.Win.ValueList(0);
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
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("multiRemove");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewStyle");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool57 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowFilterRow", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSize");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridChooseColumns");
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailAllRows");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailSelectedRows");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAction");
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("headerLabel");
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool4 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("headerTotalTextBox");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("quantityAllocateLabel");
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool10 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateSelectedTextBox");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool12 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("quantityAllocateTotalTextBox");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("saveView");
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("filter");
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("showDetails");
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("groupAllocationCreate");
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reviewSummary");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool("multiHeaderLabel");
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("multiCreate");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("multiAddTo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("headerAdd");
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("viewComboBox");
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList5 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool8 = new Infragistics.Win.UltraWinToolbars.LabelTool("Header:");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("autoSelectGroup", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExport");
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailPopupMenu");
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnDelete");
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool6 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("headerFilterComboBox");
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList6 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAction");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerView");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool6 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerFilter");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Allocation Workspace Details Toolbar");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("GridPopup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEditSave");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCancel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnHideDetails");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("GridPopup");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCollapseAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExpandAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEditSave");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCancel");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnHideDetails");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCollapseAll");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExpandAll");
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.midComboBoxFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midComboBoxView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midComboBoxAction = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this._UserActivityControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.headerToolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._UserActivityControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._UserActivityControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._UserActivityControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.ugHeaders = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmsGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsReview = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewSize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewSummary = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsReviewGroupAllocation = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSaveView = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssortment = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssrt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAssrtCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssrtAddPlaceholder = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssrtAddPhTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.cmsAssrtAddTo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssrtRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMulti = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMultiMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsMultiCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMultiAddTo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMultiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAddHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddPack = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddPackColor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddColorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAddMTColor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsChooseColor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddBulkColor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddPackSize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddBulkSize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSave = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAutoSelectGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerLower = new System.Windows.Forms.SplitContainer();
            this.Panel1_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.btnHideDetails = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEditSave = new System.Windows.Forms.Button();
            this.btnExpandCollapse = new System.Windows.Forms.Button();
            this._Panel1_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.detailToolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._Panel1_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Panel1_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Panel1_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ugDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.cmsColumnChooser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsColSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsColClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerToolbarsManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugHeaders)).BeginInit();
            this.cmsGrid.SuspendLayout();
            this.cmsAssrt.SuspendLayout();
            this.cmsMultiMenu.SuspendLayout();
            this.cmsAddMenu.SuspendLayout();
            this.cmsAddColorMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLower)).BeginInit();
            this.splitContainerLower.Panel1.SuspendLayout();
            this.splitContainerLower.Panel2.SuspendLayout();
            this.splitContainerLower.SuspendLayout();
            this.Panel1_Fill_Panel.ClientArea.SuspendLayout();
            this.Panel1_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.detailToolbarsManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugDetails)).BeginInit();
            this.cmsColumnChooser.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.midComboBoxFilter);
            this.panel1.Controls.Add(this.midComboBoxView);
            this.panel1.Controls.Add(this.midComboBoxAction);
            this.panel1.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Left);
            this.panel1.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Right);
            this.panel1.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Bottom);
            this.panel1.Controls.Add(this._UserActivityControl_Toolbars_Dock_Area_Top);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1115, 100);
            this.panel1.TabIndex = 0;
            // 
            // midComboBoxFilter
            // 
            this.midComboBoxFilter.AutoAdjust = true;
            this.midComboBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxFilter.DataSource = null;
            this.midComboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxFilter.DropDownWidth = 240;
            this.midComboBoxFilter.FormattingEnabled = false;
            this.midComboBoxFilter.IgnoreFocusLost = false;
            this.midComboBoxFilter.ItemHeight = 13;
            this.midComboBoxFilter.Location = new System.Drawing.Point(1022, 10);
            this.midComboBoxFilter.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxFilter.MaxDropDownItems = 25;
            this.midComboBoxFilter.Name = "midComboBoxFilter";
            this.midComboBoxFilter.SetToolTip = "";
            this.midComboBoxFilter.Size = new System.Drawing.Size(240, 23);
            this.midComboBoxFilter.TabIndex = 6;
            this.midComboBoxFilter.Tag = null;
            this.midComboBoxFilter.Visible = false;
            this.midComboBoxFilter.SelectionChangeCommitted += new System.EventHandler(this.midComboBoxFilter_SelectionChangeCommitted);
            // 
            // midComboBoxView
            // 
            this.midComboBoxView.AutoAdjust = true;
            this.midComboBoxView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxView.DataSource = null;
            this.midComboBoxView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxView.DropDownWidth = 240;
            this.midComboBoxView.FormattingEnabled = false;
            this.midComboBoxView.IgnoreFocusLost = false;
            this.midComboBoxView.ItemHeight = 13;
            this.midComboBoxView.Location = new System.Drawing.Point(997, 33);
            this.midComboBoxView.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxView.MaxDropDownItems = 25;
            this.midComboBoxView.Name = "midComboBoxView";
            this.midComboBoxView.SetToolTip = "";
            this.midComboBoxView.Size = new System.Drawing.Size(240, 23);
            this.midComboBoxView.TabIndex = 5;
            this.midComboBoxView.Tag = null;
            this.midComboBoxView.Visible = false;
            this.midComboBoxView.SelectionChangeCommitted += new System.EventHandler(this.midComboBoxView_SelectionChangeCommitted);
            // 
            // midComboBoxAction
            // 
            this.midComboBoxAction.AutoAdjust = true;
            this.midComboBoxAction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxAction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxAction.DataSource = null;
            this.midComboBoxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxAction.DropDownWidth = 240;
            this.midComboBoxAction.FormattingEnabled = false;
            this.midComboBoxAction.IgnoreFocusLost = false;
            this.midComboBoxAction.ItemHeight = 13;
            this.midComboBoxAction.Location = new System.Drawing.Point(997, 56);
            this.midComboBoxAction.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxAction.MaxDropDownItems = 25;
            this.midComboBoxAction.Name = "midComboBoxAction";
            this.midComboBoxAction.SetToolTip = "";
            this.midComboBoxAction.Size = new System.Drawing.Size(240, 23);
            this.midComboBoxAction.TabIndex = 4;
            this.midComboBoxAction.Tag = null;
            this.midComboBoxAction.Visible = false;
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Left
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._UserActivityControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 100);
            this._UserActivityControl_Toolbars_Dock_Area_Left.Name = "_UserActivityControl_Toolbars_Dock_Area_Left";
            this._UserActivityControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 0);
            this._UserActivityControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.headerToolbarsManager;
            // 
            // headerToolbarsManager
            // 
            this.headerToolbarsManager.DesignerFlags = 1;
            this.headerToolbarsManager.DockWithinContainer = this.panel1;
            this.headerToolbarsManager.MenuAnimationStyle = Infragistics.Win.UltraWinToolbars.MenuAnimationStyle.Slide;
            this.headerToolbarsManager.MenuSettings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
            optionSet7.AllowAllUp = false;
            optionSet8.AllowAllUp = false;
            optionSet10.AllowAllUp = false;
            this.headerToolbarsManager.OptionSets.Add(optionSet6);
            this.headerToolbarsManager.OptionSets.Add(optionSet7);
            this.headerToolbarsManager.OptionSets.Add(optionSet8);
            this.headerToolbarsManager.OptionSets.Add(optionSet9);
            this.headerToolbarsManager.OptionSets.Add(optionSet10);
            this.headerToolbarsManager.RuntimeCustomizationOptions = ((Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions)((Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.AllowCustomizeDialog | Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.AllowAltClickToolDragging)));
            this.headerToolbarsManager.ShowFullMenusDelay = 500;
            ultraToolbar9.DockedColumn = 0;
            ultraToolbar9.DockedRow = 3;
            ultraToolbar9.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool19,
            popupMenuTool20,
            buttonTool42,
            buttonTool43,
            buttonTool44});
            ultraToolbar9.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar9.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar9.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar9.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar9.Settings.ToolSpacing = 4;
            ultraToolbar9.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar9.Text = "Allocation Workspace Toolbar";
            ultraToolbar10.DockedColumn = 0;
            ultraToolbar10.DockedRow = 2;
            ultraToolbar10.FloatingSize = new System.Drawing.Size(365, 24);
            ultraToolbar10.IsStockToolbar = false;
            textBoxTool5.InstanceProps.IsFirstInGroup = true;
            ultraToolbar10.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool7,
            buttonTool46,
            textBoxTool5,
            buttonTool15,
            buttonTool3});
            ultraToolbar10.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar10.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar10.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar10.Text = "Header Create Toolbar";
            ultraToolbar11.DockedColumn = 1;
            ultraToolbar11.DockedRow = 0;
            ultraToolbar11.FloatingSize = new System.Drawing.Size(468, 24);
            controlContainerTool1.ControlName = "midComboBoxAction";
            ultraToolbar11.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool1,
            buttonTool7});
            ultraToolbar11.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar11.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar11.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar11.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar11.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar11.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar11.Text = "Action Toolbar";
            ultraToolbar12.DockedColumn = 0;
            ultraToolbar12.DockedRow = 0;
            ultraToolbar12.FloatingSize = new System.Drawing.Size(534, 24);
            textBoxTool3.InstanceProps.Width = 133;
            ultraToolbar12.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool1,
            textBoxTool1,
            textBoxTool3});
            ultraToolbar12.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar12.Text = "Header Toolbar";
            ultraToolbar13.DockedColumn = 0;
            ultraToolbar13.DockedRow = 1;
            textBoxTool11.InstanceProps.Width = 133;
            ultraToolbar13.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool3,
            textBoxTool9,
            textBoxTool11});
            ultraToolbar13.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar13.Text = "Qty to Allocate Toolbar";
            ultraToolbar14.DockedColumn = 1;
            ultraToolbar14.DockedRow = 1;
            controlContainerTool3.ControlName = "midComboBoxView";
            ultraToolbar14.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool3});
            ultraToolbar14.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar14.Text = "View Toolbar";
            ultraToolbar15.DockedColumn = 1;
            ultraToolbar15.DockedRow = 2;
            controlContainerTool5.ControlName = "midComboBoxFilter";
            ultraToolbar15.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool5,
            buttonTool39});
            ultraToolbar15.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar15.Text = "Filter Toolbar";
            this.headerToolbarsManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar9,
            ultraToolbar10,
            ultraToolbar11,
            ultraToolbar12,
            ultraToolbar13,
            ultraToolbar14,
            ultraToolbar15});
            textBoxTool6.SharedPropsInternal.Caption = "Group Allocation:";
            textBoxTool6.SharedPropsInternal.Category = "Grid Search";
            textBoxTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool6.SharedPropsInternal.Width = 250;
            buttonTool19.SharedPropsInternal.Caption = "Selection Screen";
            buttonTool19.SharedPropsInternal.Category = "Review";
            buttonTool19.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance44.Image = ((object)(resources.GetObject("appearance44.Image")));
            popupMenuTool27.SharedPropsInternal.AppearancesSmall.Appearance = appearance44;
            popupMenuTool27.SharedPropsInternal.Caption = "Review";
            popupMenuTool27.SharedPropsInternal.Category = "Review";
            popupMenuTool27.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool27.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool21,
            buttonTool22,
            buttonTool24,
            buttonTool18});
            appearance45.Image = ((object)(resources.GetObject("appearance45.Image")));
            popupMenuTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance45;
            popupMenuTool28.SharedPropsInternal.Caption = "Grid";
            popupMenuTool28.SharedPropsInternal.Category = "Grid";
            popupMenuTool28.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            stateButtonTool43.Checked = true;
            stateButtonTool44.Checked = true;
            buttonTool25.InstanceProps.IsFirstInGroup = true;
            popupMenuTool28.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            stateButtonTool43,
            stateButtonTool44,
            buttonTool25,
            buttonTool1,
            buttonTool11});
            stateButtonTool49.Checked = true;
            stateButtonTool49.SharedPropsInternal.Caption = "Show Group Area";
            stateButtonTool49.SharedPropsInternal.Category = "Grid";
            appearance46.BackColor = System.Drawing.SystemColors.Control;
            comboBoxTool4.EditAppearance = appearance46;
            comboBoxTool4.SharedPropsInternal.Caption = "Action:";
            comboBoxTool4.SharedPropsInternal.Category = "Action";
            comboBoxTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool4.SharedPropsInternal.Width = 275;
            valueList4.DropDownListMinWidth = 250;
            valueList4.DropDownListWidth = 250;
            valueList4.MaxDropDownItems = 30;
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
            valueList4.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
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
            comboBoxTool4.ValueList = valueList4;
            appearance47.Image = ((object)(resources.GetObject("appearance47.Image")));
            buttonTool26.SharedPropsInternal.AppearancesSmall.Appearance = appearance47;
            buttonTool26.SharedPropsInternal.Caption = "Search";
            buttonTool26.SharedPropsInternal.Category = "Misc";
            buttonTool26.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
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
            appearance48.Image = ((object)(resources.GetObject("appearance48.Image")));
            buttonTool35.SharedPropsInternal.AppearancesSmall.Appearance = appearance48;
            buttonTool35.SharedPropsInternal.Caption = "Choose Columns";
            buttonTool35.SharedPropsInternal.Category = "Grid";
            buttonTool4.SharedPropsInternal.Caption = "Email All Rows";
            buttonTool4.SharedPropsInternal.Category = "Grid Export";
            buttonTool6.SharedPropsInternal.Caption = "Email Selected Rows";
            buttonTool6.SharedPropsInternal.Category = "Grid Export";
            appearance49.Image = ((object)(resources.GetObject("appearance49.Image")));
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance49;
            buttonTool8.SharedPropsInternal.Caption = "Process";
            buttonTool8.SharedPropsInternal.Category = "Action";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance50.FontData.BoldAsString = "True";
            appearance50.FontData.ItalicAsString = "True";
            appearance50.TextHAlignAsString = "Left";
            labelTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance50;
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
            appearance51.FontData.BoldAsString = "True";
            appearance51.FontData.ItalicAsString = "True";
            appearance51.TextHAlignAsString = "Left";
            labelTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance51;
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
            appearance52.Image = ((object)(resources.GetObject("appearance52.Image")));
            buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance52;
            buttonTool10.SharedPropsInternal.Caption = "Save View";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance53.Image = ((object)(resources.GetObject("appearance53.Image")));
            buttonTool12.SharedPropsInternal.AppearancesSmall.Appearance = appearance53;
            buttonTool12.SharedPropsInternal.Caption = "Edit Filter";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance54.Image = ((object)(resources.GetObject("appearance54.Image")));
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance54;
            buttonTool14.SharedPropsInternal.Caption = "Show Details";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance55.Image = ((object)(resources.GetObject("appearance55.Image")));
            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance55;
            buttonTool16.SharedPropsInternal.Caption = "Create";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool20.SharedPropsInternal.Caption = "Summary";
            buttonTool20.SharedPropsInternal.Category = "Review";
            appearance56.TextHAlignAsString = "Left";
            labelTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance56;
            labelTool6.SharedPropsInternal.Caption = "Multi-Header:";
            labelTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool38.SharedPropsInternal.Caption = "Create";
            buttonTool38.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool40.SharedPropsInternal.Caption = "Add to";
            buttonTool40.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance57.Image = ((object)(resources.GetObject("appearance57.Image")));
            buttonTool47.SharedPropsInternal.AppearancesSmall.Appearance = appearance57;
            buttonTool47.SharedPropsInternal.Caption = "Add";
            buttonTool47.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance58.BackColor = System.Drawing.SystemColors.Control;
            comboBoxTool2.EditAppearance = appearance58;
            comboBoxTool2.SharedPropsInternal.Caption = "View:";
            comboBoxTool2.SharedPropsInternal.Category = "View";
            comboBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool2.SharedPropsInternal.Width = 250;
            comboBoxTool2.ValueList = valueList5;
            labelTool8.SharedPropsInternal.Caption = "Header:";
            labelTool8.SharedPropsInternal.Category = "Add";
            labelTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            stateButtonTool2.SharedPropsInternal.Caption = "Auto Select Group";
            stateButtonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance59.Image = ((object)(resources.GetObject("appearance59.Image")));
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance59;
            buttonTool2.SharedPropsInternal.Caption = "Export";
            buttonTool2.SharedPropsInternal.Category = "Grid Export";
            appearance60.Image = ((object)(resources.GetObject("appearance60.Image")));
            buttonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance60;
            buttonTool13.SharedPropsInternal.Caption = "Email";
            buttonTool13.SharedPropsInternal.Category = "Grid Export";
            appearance61.Image = ((object)(resources.GetObject("appearance61.Image")));
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance61;
            buttonTool5.SharedPropsInternal.Caption = "Delete";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance62.BackColor = System.Drawing.SystemColors.Control;
            comboBoxTool6.EditAppearance = appearance62;
            comboBoxTool6.SharedPropsInternal.Caption = "Filter:";
            comboBoxTool6.SharedPropsInternal.Category = "Headers";
            comboBoxTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool6.SharedPropsInternal.Width = 250;
            comboBoxTool6.ValueList = valueList6;
            controlContainerTool2.ControlName = "midComboBoxAction";
            controlContainerTool2.SharedPropsInternal.Caption = "Action:";
            controlContainerTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool4.ControlName = "midComboBoxView";
            controlContainerTool4.SharedPropsInternal.Caption = "  View:";
            controlContainerTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool6.ControlName = "midComboBoxFilter";
            controlContainerTool6.SharedPropsInternal.Caption = "Filter:";
            controlContainerTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
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
            buttonTool5,
            comboBoxTool6,
            controlContainerTool2,
            controlContainerTool4,
            controlContainerTool6});
            this.headerToolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.headerToolbarsManager_ToolClick);
            this.headerToolbarsManager.ToolKeyPress += new Infragistics.Win.UltraWinToolbars.ToolKeyPressEventHandler(this.headerToolbarsManager_ToolKeyPress);
            this.headerToolbarsManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.headerToolbarsManager_ValueChanged);
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Right
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._UserActivityControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1115, 100);
            this._UserActivityControl_Toolbars_Dock_Area_Right.Name = "_UserActivityControl_Toolbars_Dock_Area_Right";
            this._UserActivityControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 0);
            this._UserActivityControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.headerToolbarsManager;
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Bottom
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 100);
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.Name = "_UserActivityControl_Toolbars_Dock_Area_Bottom";
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1115, 0);
            this._UserActivityControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.headerToolbarsManager;
            // 
            // _UserActivityControl_Toolbars_Dock_Area_Top
            // 
            this._UserActivityControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UserActivityControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._UserActivityControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._UserActivityControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UserActivityControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._UserActivityControl_Toolbars_Dock_Area_Top.Name = "_UserActivityControl_Toolbars_Dock_Area_Top";
            this._UserActivityControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1115, 100);
            this._UserActivityControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.headerToolbarsManager;
            // 
            // ultraGridExcelExporter1
            // 
            this.ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(this.ultraGridExcelExporter1_RowExporting);
            // 
            // splitContainer
            // 
            this.splitContainer.AllowDrop = true;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 100);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.ugHeaders);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AllowDrop = true;
            this.splitContainer.Panel2.Controls.Add(this.splitContainerLower);
            this.splitContainer.Panel2MinSize = 0;
            this.splitContainer.Size = new System.Drawing.Size(1115, 500);
            this.splitContainer.SplitterDistance = 327;
            this.splitContainer.TabIndex = 4;
            // 
            // ugHeaders
            // 
            this.ugHeaders.AllowDrop = true;
            this.ugHeaders.ContextMenuStrip = this.cmsGrid;
            this.ugHeaders.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugHeaders.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.BorderColor = System.Drawing.SystemColors.Window;
            this.ugHeaders.DisplayLayout.GroupByBox.Appearance = appearance1;
            appearance2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugHeaders.DisplayLayout.GroupByBox.BandLabelAppearance = appearance2;
            this.ugHeaders.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugHeaders.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.ugHeaders.DisplayLayout.MaxColScrollRegions = 1;
            this.ugHeaders.DisplayLayout.MaxRowScrollRegions = 1;
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugHeaders.DisplayLayout.Override.ActiveCellAppearance = appearance4;
            this.ugHeaders.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugHeaders.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            this.ugHeaders.DisplayLayout.Override.CardAreaAppearance = appearance5;
            appearance6.BorderColor = System.Drawing.Color.Silver;
            appearance6.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugHeaders.DisplayLayout.Override.CellAppearance = appearance6;
            this.ugHeaders.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugHeaders.DisplayLayout.Override.CellPadding = 0;
            appearance7.BackColor = System.Drawing.SystemColors.Control;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.BorderColor = System.Drawing.SystemColors.Window;
            this.ugHeaders.DisplayLayout.Override.GroupByRowAppearance = appearance7;
            this.ugHeaders.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            this.ugHeaders.DisplayLayout.Override.RowAppearance = appearance8;
            this.ugHeaders.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ugHeaders.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugHeaders.DisplayLayout.Override.TemplateAddRowAppearance = appearance9;
            this.ugHeaders.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugHeaders.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugHeaders.Location = new System.Drawing.Point(0, 0);
            this.ugHeaders.Name = "ugHeaders";
            this.ugHeaders.Size = new System.Drawing.Size(1115, 327);
            this.ugHeaders.TabIndex = 0;
            this.ugHeaders.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugHeaders_InitializeLayout);
            this.ugHeaders.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugHeaders_InitializeRow);
            this.ugHeaders.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.ugHeaders_AfterColRegionScroll);
            this.ugHeaders.BeforeRowCollapsed += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugHeaders_BeforeRowCollapsed);
            this.ugHeaders.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugHeaders_BeforeRowExpanded);
            this.ugHeaders.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugHeaders_ClickCellButton);
            this.ugHeaders.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugHeaders_AfterSelectChange);
            this.ugHeaders.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ugHeaders_BeforeSelectChange);
            this.ugHeaders.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ugHeaders_SelectionDrag);
            this.ugHeaders.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugHeaders_BeforeRowsDeleted);
            this.ugHeaders.DoubleClickCell += new Infragistics.Win.UltraWinGrid.DoubleClickCellEventHandler(this.ugHeaders_DoubleClickCell);
            this.ugHeaders.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.ugHeaders_DoubleClickRow);
            this.ugHeaders.BeforeSortChange += new Infragistics.Win.UltraWinGrid.BeforeSortChangeEventHandler(this.ugHeaders_BeforeSortChange);
            this.ugHeaders.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugHeaders_AfterSortChange);
            this.ugHeaders.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.ugHeaders_AfterColPosChanged);
            this.ugHeaders.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ugHeaders_BeforeColumnChooserDisplayed);
            this.ugHeaders.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugHeaders_DragEnter);
            this.ugHeaders.DragOver += new System.Windows.Forms.DragEventHandler(this.ugHeaders_DragOver);
            this.ugHeaders.DragLeave += new System.EventHandler(this.ugHeaders_DragLeave);
            this.ugHeaders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugHeaders_KeyDown);
            this.ugHeaders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grid_MouseDown);
            this.ugHeaders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugHeaders_MouseUp);
            // 
            // cmsGrid
            // 
            this.cmsGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.cmsGrid.AllowMerge = false;
            this.cmsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsReview,
            this.cmsSeparator1,
            this.cmsFilter,
            this.cmsSearch,
            this.cmsSaveView,
            this.cmsDetails,
            this.cmsRemove,
            this.cmsExpand,
            this.cmsCollapse,
            this.cmsAssortment,
            this.cmsMulti,
            this.cmsAdd,
            this.cmsDelete,
            this.cmsSave,
            this.cmsSaveAs,
            this.cmsCancel,
            this.cmsAutoSelectGroup});
            this.cmsGrid.Name = "cmsGrid";
            this.cmsGrid.ShowCheckMargin = true;
            this.cmsGrid.ShowImageMargin = false;
            this.cmsGrid.Size = new System.Drawing.Size(171, 362);
            this.cmsGrid.Opening += new System.ComponentModel.CancelEventHandler(this.cmsGrid_Opening);
            // 
            // cmsReview
            // 
            this.cmsReview.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsReview.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsReviewSelect,
            this.cmsReviewStyle,
            this.cmsReviewSize,
            this.cmsReviewSummary,
            this.cmsReviewGroupAllocation});
            this.cmsReview.Name = "cmsReview";
            this.cmsReview.Size = new System.Drawing.Size(170, 22);
            this.cmsReview.Text = "Review";
            // 
            // cmsReviewSelect
            // 
            this.cmsReviewSelect.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsReviewSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsReviewSelect.Name = "cmsReviewSelect";
            this.cmsReviewSelect.Size = new System.Drawing.Size(164, 22);
            this.cmsReviewSelect.Text = "Select";
            this.cmsReviewSelect.Click += new System.EventHandler(this.cmsReviewSelect_Click);
            // 
            // cmsReviewStyle
            // 
            this.cmsReviewStyle.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsReviewStyle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsReviewStyle.Name = "cmsReviewStyle";
            this.cmsReviewStyle.Size = new System.Drawing.Size(164, 22);
            this.cmsReviewStyle.Text = "Style";
            this.cmsReviewStyle.Click += new System.EventHandler(this.cmsReviewStyle_Click);
            // 
            // cmsReviewSize
            // 
            this.cmsReviewSize.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsReviewSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsReviewSize.Name = "cmsReviewSize";
            this.cmsReviewSize.Size = new System.Drawing.Size(164, 22);
            this.cmsReviewSize.Text = "Size";
            this.cmsReviewSize.Click += new System.EventHandler(this.cmsReviewSize_Click);
            // 
            // cmsReviewSummary
            // 
            this.cmsReviewSummary.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsReviewSummary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsReviewSummary.Name = "cmsReviewSummary";
            this.cmsReviewSummary.Size = new System.Drawing.Size(164, 22);
            this.cmsReviewSummary.Text = "Summary";
            this.cmsReviewSummary.Click += new System.EventHandler(this.cmsReviewSummary_Click);
            // 
            // cmsReviewGroupAllocation
            // 
            this.cmsReviewGroupAllocation.Name = "cmsReviewGroupAllocation";
            this.cmsReviewGroupAllocation.Size = new System.Drawing.Size(164, 22);
            this.cmsReviewGroupAllocation.Text = "Group Allocation";
            this.cmsReviewGroupAllocation.Click += new System.EventHandler(this.cmsGroupAllocation_Click);
            // 
            // cmsSeparator1
            // 
            this.cmsSeparator1.Name = "cmsSeparator1";
            this.cmsSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // cmsFilter
            // 
            this.cmsFilter.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(170, 22);
            this.cmsFilter.Text = "Filter";
            this.cmsFilter.Click += new System.EventHandler(this.cmsFilter_Click);
            // 
            // cmsSearch
            // 
            this.cmsSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsSearch.Name = "cmsSearch";
            this.cmsSearch.Size = new System.Drawing.Size(170, 22);
            this.cmsSearch.Text = "Search";
            this.cmsSearch.Click += new System.EventHandler(this.cmsSearch_Click);
            // 
            // cmsSaveView
            // 
            this.cmsSaveView.Name = "cmsSaveView";
            this.cmsSaveView.Size = new System.Drawing.Size(170, 22);
            this.cmsSaveView.Text = "Save View";
            this.cmsSaveView.Click += new System.EventHandler(this.cmsSaveView_Click);
            // 
            // cmsDetails
            // 
            this.cmsDetails.Name = "cmsDetails";
            this.cmsDetails.Size = new System.Drawing.Size(170, 22);
            this.cmsDetails.Text = "Show Details";
            this.cmsDetails.Click += new System.EventHandler(this.cmsDetails_Click);
            // 
            // cmsRemove
            // 
            this.cmsRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsRemove.Name = "cmsRemove";
            this.cmsRemove.Size = new System.Drawing.Size(170, 22);
            this.cmsRemove.Text = "Remove";
            this.cmsRemove.Click += new System.EventHandler(this.cmsRemove_Click);
            // 
            // cmsExpand
            // 
            this.cmsExpand.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsExpand.Name = "cmsExpand";
            this.cmsExpand.Size = new System.Drawing.Size(170, 22);
            this.cmsExpand.Text = "Expand All";
            this.cmsExpand.Click += new System.EventHandler(this.cmsExpand_Click);
            // 
            // cmsCollapse
            // 
            this.cmsCollapse.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsCollapse.Name = "cmsCollapse";
            this.cmsCollapse.Size = new System.Drawing.Size(170, 22);
            this.cmsCollapse.Text = "Collapse All";
            this.cmsCollapse.Click += new System.EventHandler(this.cmsCollapse_Click);
            // 
            // cmsAssortment
            // 
            this.cmsAssortment.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsAssortment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsAssortment.DropDown = this.cmsAssrt;
            this.cmsAssortment.Name = "cmsAssortment";
            this.cmsAssortment.Size = new System.Drawing.Size(170, 22);
            this.cmsAssortment.Text = "Assortment";
            // 
            // cmsAssrt
            // 
            this.cmsAssrt.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.cmsAssrt.AllowMerge = false;
            this.cmsAssrt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAssrtCreate,
            this.cmsAssrtAddPlaceholder,
            this.cmsAssrtAddTo,
            this.cmsAssrtRemove});
            this.cmsAssrt.Name = "cmsAssrt";
            this.cmsAssrt.OwnerItem = this.cmsAssortment;
            this.cmsAssrt.ShowImageMargin = false;
            this.cmsAssrt.Size = new System.Drawing.Size(165, 92);
            this.cmsAssrt.Opening += new System.ComponentModel.CancelEventHandler(this.cmsAssrt_Opening);
            // 
            // cmsAssrtCreate
            // 
            this.cmsAssrtCreate.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsAssrtCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsAssrtCreate.Name = "cmsAssrtCreate";
            this.cmsAssrtCreate.Size = new System.Drawing.Size(164, 22);
            this.cmsAssrtCreate.Text = "Create";
            this.cmsAssrtCreate.Click += new System.EventHandler(this.cmsAssrtCreate_Click);
            // 
            // cmsAssrtAddPlaceholder
            // 
            this.cmsAssrtAddPlaceholder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAssrtAddPhTextBox});
            this.cmsAssrtAddPlaceholder.Name = "cmsAssrtAddPlaceholder";
            this.cmsAssrtAddPlaceholder.Size = new System.Drawing.Size(164, 22);
            this.cmsAssrtAddPlaceholder.Text = "Add Placeholder Style";
            this.cmsAssrtAddPlaceholder.DropDownOpening += new System.EventHandler(this.cmsAssrtAddPlaceholder_DropDownOpening);
            this.cmsAssrtAddPlaceholder.DropDownOpened += new System.EventHandler(this.cmsAssrtAddPlaceholder_DropDownOpened);
            // 
            // cmsAssrtAddPhTextBox
            // 
            this.cmsAssrtAddPhTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.cmsAssrtAddPhTextBox.MaxLength = 2;
            this.cmsAssrtAddPhTextBox.Name = "cmsAssrtAddPhTextBox";
            this.cmsAssrtAddPhTextBox.Size = new System.Drawing.Size(21, 23);
            this.cmsAssrtAddPhTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmsAssrtAddPhTextBox_KeyUp);
            this.cmsAssrtAddPhTextBox.DoubleClick += new System.EventHandler(this.cmsAssrtAddPhTextBox_DoubleClick);
            // 
            // cmsAssrtAddTo
            // 
            this.cmsAssrtAddTo.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsAssrtAddTo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsAssrtAddTo.Name = "cmsAssrtAddTo";
            this.cmsAssrtAddTo.Size = new System.Drawing.Size(164, 22);
            this.cmsAssrtAddTo.Text = "Add to";
            this.cmsAssrtAddTo.Click += new System.EventHandler(this.cmsAssrtAddTo_Click);
            // 
            // cmsAssrtRemove
            // 
            this.cmsAssrtRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsAssrtRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmsAssrtRemove.Name = "cmsAssrtRemove";
            this.cmsAssrtRemove.Size = new System.Drawing.Size(164, 22);
            this.cmsAssrtRemove.Text = "Remove";
            this.cmsAssrtRemove.Click += new System.EventHandler(this.cmsAssrtRemove_Click);
            // 
            // cmsMulti
            // 
            this.cmsMulti.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsMulti.DropDown = this.cmsMultiMenu;
            this.cmsMulti.Name = "cmsMulti";
            this.cmsMulti.Size = new System.Drawing.Size(170, 22);
            this.cmsMulti.Text = "Multi-header";
            // 
            // cmsMultiMenu
            // 
            this.cmsMultiMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsMultiCreate,
            this.cmsMultiAddTo,
            this.cmsMultiRemove});
            this.cmsMultiMenu.Name = "cmsMultiMenu";
            this.cmsMultiMenu.OwnerItem = this.cmsMulti;
            this.cmsMultiMenu.ShowImageMargin = false;
            this.cmsMultiMenu.Size = new System.Drawing.Size(93, 70);
            this.cmsMultiMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMultiMenu_Opening);
            // 
            // cmsMultiCreate
            // 
            this.cmsMultiCreate.Name = "cmsMultiCreate";
            this.cmsMultiCreate.Size = new System.Drawing.Size(92, 22);
            this.cmsMultiCreate.Text = "Create";
            this.cmsMultiCreate.Click += new System.EventHandler(this.cmsMultiCreate_Click);
            // 
            // cmsMultiAddTo
            // 
            this.cmsMultiAddTo.Name = "cmsMultiAddTo";
            this.cmsMultiAddTo.Size = new System.Drawing.Size(92, 22);
            this.cmsMultiAddTo.Text = "Add to";
            this.cmsMultiAddTo.Click += new System.EventHandler(this.cmsMultiAddTo_Click);
            // 
            // cmsMultiRemove
            // 
            this.cmsMultiRemove.Name = "cmsMultiRemove";
            this.cmsMultiRemove.Size = new System.Drawing.Size(92, 22);
            this.cmsMultiRemove.Text = "Remove";
            this.cmsMultiRemove.Click += new System.EventHandler(this.cmsMultiRemove_Click);
            // 
            // cmsAdd
            // 
            this.cmsAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsAdd.DropDown = this.cmsAddMenu;
            this.cmsAdd.Name = "cmsAdd";
            this.cmsAdd.Size = new System.Drawing.Size(170, 22);
            this.cmsAdd.Text = "Add";
            // 
            // cmsAddMenu
            // 
            this.cmsAddMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAddHeader,
            this.cmsAddPack,
            this.cmsAddPackColor,
            this.cmsAddPackSize,
            this.cmsAddBulkColor,
            this.cmsAddBulkSize});
            this.cmsAddMenu.Name = "cmsAddMenu";
            this.cmsAddMenu.OwnerItem = this.cmsAdd;
            this.cmsAddMenu.ShowImageMargin = false;
            this.cmsAddMenu.Size = new System.Drawing.Size(104, 136);
            // 
            // cmsAddHeader
            // 
            this.cmsAddHeader.Name = "cmsAddHeader";
            this.cmsAddHeader.Size = new System.Drawing.Size(103, 22);
            this.cmsAddHeader.Text = "Header";
            this.cmsAddHeader.Click += new System.EventHandler(this.cmsAddHeader_Click);
            // 
            // cmsAddPack
            // 
            this.cmsAddPack.Name = "cmsAddPack";
            this.cmsAddPack.Size = new System.Drawing.Size(103, 22);
            this.cmsAddPack.Text = "Pack";
            this.cmsAddPack.Click += new System.EventHandler(this.cmsAddPack_Click);
            // 
            // cmsAddPackColor
            // 
            this.cmsAddPackColor.DropDown = this.cmsAddColorMenu;
            this.cmsAddPackColor.Name = "cmsAddPackColor";
            this.cmsAddPackColor.Size = new System.Drawing.Size(103, 22);
            this.cmsAddPackColor.Text = "PackColor";
            // 
            // cmsAddColorMenu
            // 
            this.cmsAddColorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAddMTColor,
            this.cmsChooseColor});
            this.cmsAddColorMenu.Name = "cmsAddColor";
            this.cmsAddColorMenu.OwnerItem = this.cmsAddPackColor;
            this.cmsAddColorMenu.ShowImageMargin = false;
            this.cmsAddColorMenu.Size = new System.Drawing.Size(124, 48);
            // 
            // cmsAddMTColor
            // 
            this.cmsAddMTColor.Name = "cmsAddMTColor";
            this.cmsAddMTColor.Size = new System.Drawing.Size(123, 22);
            this.cmsAddMTColor.Text = "Empty Row";
            this.cmsAddMTColor.Click += new System.EventHandler(this.cmsAddMTColor_Click);
            // 
            // cmsChooseColor
            // 
            this.cmsChooseColor.Name = "cmsChooseColor";
            this.cmsChooseColor.Size = new System.Drawing.Size(123, 22);
            this.cmsChooseColor.Text = "Color Browser";
            this.cmsChooseColor.Click += new System.EventHandler(this.cmsChooseColor_Click);
            // 
            // cmsAddBulkColor
            // 
            this.cmsAddBulkColor.DropDown = this.cmsAddColorMenu;
            this.cmsAddBulkColor.Name = "cmsAddBulkColor";
            this.cmsAddBulkColor.Size = new System.Drawing.Size(103, 22);
            this.cmsAddBulkColor.Text = "BulkColor";
            // 
            // cmsAddPackSize
            // 
            this.cmsAddPackSize.Name = "cmsAddPackSize";
            this.cmsAddPackSize.Size = new System.Drawing.Size(103, 22);
            this.cmsAddPackSize.Text = "PackSize";
            this.cmsAddPackSize.Click += new System.EventHandler(this.cmsAddPackSize_Click);
            // 
            // cmsAddBulkSize
            // 
            this.cmsAddBulkSize.Name = "cmsAddBulkSize";
            this.cmsAddBulkSize.Size = new System.Drawing.Size(103, 22);
            this.cmsAddBulkSize.Text = "BulkSize";
            this.cmsAddBulkSize.Click += new System.EventHandler(this.cmsAddBulkSize_Click);
            // 
            // cmsDelete
            // 
            this.cmsDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.Size = new System.Drawing.Size(170, 22);
            this.cmsDelete.Text = "Delete";
            this.cmsDelete.Click += new System.EventHandler(this.cmsDelete_Click);
            // 
            // cmsSave
            // 
            this.cmsSave.Name = "cmsSave";
            this.cmsSave.Size = new System.Drawing.Size(170, 22);
            this.cmsSave.Text = "Save";
            this.cmsSave.Click += new System.EventHandler(this.cmsSave_Click);
            // 
            // cmsSaveAs
            // 
            this.cmsSaveAs.Name = "cmsSaveAs";
            this.cmsSaveAs.Size = new System.Drawing.Size(170, 22);
            this.cmsSaveAs.Text = "Save As...";
            this.cmsSaveAs.Click += new System.EventHandler(this.cmsSaveAs_Click);
            // 
            // cmsCancel
            // 
            this.cmsCancel.Name = "cmsCancel";
            this.cmsCancel.Size = new System.Drawing.Size(170, 22);
            this.cmsCancel.Text = "Cancel";
            this.cmsCancel.Click += new System.EventHandler(this.cmsCancel_Click);
            // 
            // cmsAutoSelectGroup
            // 
            this.cmsAutoSelectGroup.Name = "cmsAutoSelectGroup";
            this.cmsAutoSelectGroup.Size = new System.Drawing.Size(170, 22);
            this.cmsAutoSelectGroup.Text = "Auto Select Group";
            this.cmsAutoSelectGroup.Click += new System.EventHandler(this.cmsAutoSelectGroup_Click);
            // 
            // splitContainerLower
            // 
            this.splitContainerLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLower.IsSplitterFixed = true;
            this.splitContainerLower.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLower.Name = "splitContainerLower";
            this.splitContainerLower.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLower.Panel1
            // 
            this.splitContainerLower.Panel1.Controls.Add(this.Panel1_Fill_Panel);
            this.splitContainerLower.Panel1.Controls.Add(this._Panel1_Toolbars_Dock_Area_Left);
            this.splitContainerLower.Panel1.Controls.Add(this._Panel1_Toolbars_Dock_Area_Right);
            this.splitContainerLower.Panel1.Controls.Add(this._Panel1_Toolbars_Dock_Area_Bottom);
            this.splitContainerLower.Panel1.Controls.Add(this._Panel1_Toolbars_Dock_Area_Top);
            this.splitContainerLower.Panel1MinSize = 23;
            // 
            // splitContainerLower.Panel2
            // 
            this.splitContainerLower.Panel2.Controls.Add(this.ugDetails);
            this.splitContainerLower.Size = new System.Drawing.Size(1115, 169);
            this.splitContainerLower.SplitterDistance = 26;
            this.splitContainerLower.TabIndex = 1;
            // 
            // Panel1_Fill_Panel
            // 
            // 
            // Panel1_Fill_Panel.ClientArea
            // 
            this.Panel1_Fill_Panel.ClientArea.Controls.Add(this.btnHideDetails);
            this.Panel1_Fill_Panel.ClientArea.Controls.Add(this.btnCancel);
            this.Panel1_Fill_Panel.ClientArea.Controls.Add(this.btnEditSave);
            this.Panel1_Fill_Panel.ClientArea.Controls.Add(this.btnExpandCollapse);
            this.Panel1_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Panel1_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1_Fill_Panel.Location = new System.Drawing.Point(0, 25);
            this.Panel1_Fill_Panel.Name = "Panel1_Fill_Panel";
            this.Panel1_Fill_Panel.Size = new System.Drawing.Size(1115, 1);
            this.Panel1_Fill_Panel.TabIndex = 0;
            // 
            // btnHideDetails
            // 
            this.btnHideDetails.Location = new System.Drawing.Point(313, 0);
            this.btnHideDetails.Name = "btnHideDetails";
            this.btnHideDetails.Size = new System.Drawing.Size(80, 23);
            this.btnHideDetails.TabIndex = 3;
            this.btnHideDetails.Text = "Hide Details";
            this.btnHideDetails.UseVisualStyleBackColor = true;
            this.btnHideDetails.Click += new System.EventHandler(this.btnHideDetails_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(213, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEditSave
            // 
            this.btnEditSave.Location = new System.Drawing.Point(132, 0);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(75, 23);
            this.btnEditSave.TabIndex = 1;
            this.btnEditSave.Text = "Edit";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.btnEditSave_Click);
            // 
            // btnExpandCollapse
            // 
            this.btnExpandCollapse.Location = new System.Drawing.Point(30, 0);
            this.btnExpandCollapse.Name = "btnExpandCollapse";
            this.btnExpandCollapse.Size = new System.Drawing.Size(76, 23);
            this.btnExpandCollapse.TabIndex = 0;
            this.btnExpandCollapse.Text = "Collapse All";
            this.btnExpandCollapse.UseVisualStyleBackColor = true;
            this.btnExpandCollapse.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // _Panel1_Toolbars_Dock_Area_Left
            // 
            this._Panel1_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Panel1_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._Panel1_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._Panel1_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Panel1_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._Panel1_Toolbars_Dock_Area_Left.Name = "_Panel1_Toolbars_Dock_Area_Left";
            this._Panel1_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 1);
            this._Panel1_Toolbars_Dock_Area_Left.ToolbarsManager = this.detailToolbarsManager;
            // 
            // detailToolbarsManager
            // 
            this.detailToolbarsManager.DesignerFlags = 1;
            this.detailToolbarsManager.DockWithinContainer = this.splitContainerLower.Panel1;
            this.detailToolbarsManager.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool2,
            buttonTool9,
            buttonTool23,
            buttonTool28});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
            ultraToolbar1.Settings.ToolSpacing = 4;
            ultraToolbar1.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Text = "Allocation Workspace Details Toolbar";
            this.detailToolbarsManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            popupMenuTool3.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            popupMenuTool3.SharedPropsInternal.Caption = "Grid";
            popupMenuTool3.SharedPropsInternal.Category = "GridPopup";
            popupMenuTool3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool36,
            buttonTool37});
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            buttonTool17.SharedPropsInternal.AppearancesSmall.Appearance = appearance11;
            buttonTool17.SharedPropsInternal.Caption = "Edit";
            buttonTool17.SharedPropsInternal.Category = "EditSave";
            buttonTool17.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            buttonTool29.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            buttonTool29.SharedPropsInternal.Caption = "Cancel";
            buttonTool29.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            buttonTool31.SharedPropsInternal.AppearancesSmall.Appearance = appearance13;
            buttonTool31.SharedPropsInternal.Caption = "Hide Details";
            buttonTool31.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            buttonTool32.SharedPropsInternal.AppearancesSmall.Appearance = appearance14;
            buttonTool32.SharedPropsInternal.Caption = "Collapse All";
            buttonTool32.SharedPropsInternal.Category = "gridiPopup";
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            buttonTool33.SharedPropsInternal.AppearancesSmall.Appearance = appearance15;
            buttonTool33.SharedPropsInternal.Caption = "Expand All";
            buttonTool33.SharedPropsInternal.Category = "gridiPopup";
            this.detailToolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool3,
            buttonTool17,
            buttonTool29,
            buttonTool31,
            buttonTool32,
            buttonTool33});
            this.detailToolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.detailToolbarsManager_ToolClick);
            // 
            // _Panel1_Toolbars_Dock_Area_Right
            // 
            this._Panel1_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Panel1_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._Panel1_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._Panel1_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Panel1_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1115, 25);
            this._Panel1_Toolbars_Dock_Area_Right.Name = "_Panel1_Toolbars_Dock_Area_Right";
            this._Panel1_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 1);
            this._Panel1_Toolbars_Dock_Area_Right.ToolbarsManager = this.detailToolbarsManager;
            // 
            // _Panel1_Toolbars_Dock_Area_Bottom
            // 
            this._Panel1_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Panel1_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._Panel1_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._Panel1_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Panel1_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 26);
            this._Panel1_Toolbars_Dock_Area_Bottom.Name = "_Panel1_Toolbars_Dock_Area_Bottom";
            this._Panel1_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1115, 0);
            this._Panel1_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.detailToolbarsManager;
            // 
            // _Panel1_Toolbars_Dock_Area_Top
            // 
            this._Panel1_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Panel1_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._Panel1_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._Panel1_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Panel1_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._Panel1_Toolbars_Dock_Area_Top.Name = "_Panel1_Toolbars_Dock_Area_Top";
            this._Panel1_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1115, 25);
            this._Panel1_Toolbars_Dock_Area_Top.ToolbarsManager = this.detailToolbarsManager;
            // 
            // ugDetails
            // 
            this.ugDetails.AllowDrop = true;
            this.ugDetails.ContextMenuStrip = this.cmsGrid;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugDetails.DisplayLayout.Appearance = appearance16;
            this.ugDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderColor = System.Drawing.SystemColors.Window;
            this.ugDetails.DisplayLayout.GroupByBox.Appearance = appearance17;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugDetails.DisplayLayout.GroupByBox.BandLabelAppearance = appearance18;
            this.ugDetails.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugDetails.DisplayLayout.GroupByBox.Hidden = true;
            appearance19.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance19.BackColor2 = System.Drawing.SystemColors.Control;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugDetails.DisplayLayout.GroupByBox.PromptAppearance = appearance19;
            this.ugDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.ugDetails.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugDetails.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugDetails.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.ugDetails.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugDetails.DisplayLayout.Override.CellAppearance = appearance21;
            this.ugDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugDetails.DisplayLayout.Override.CellPadding = 0;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.ugDetails.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            this.ugDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.ugDetails.DisplayLayout.Override.RowAppearance = appearance23;
            this.ugDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.ugDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugDetails.Location = new System.Drawing.Point(0, 0);
            this.ugDetails.Name = "ugDetails";
            this.ugDetails.Size = new System.Drawing.Size(1115, 139);
            this.ugDetails.TabIndex = 0;
            this.ugDetails.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_AfterCellUpdate);
            this.ugDetails.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugDetails_InitializeLayout);
            this.ugDetails.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugDetails_InitializeRow);
            this.ugDetails.AfterRowsDeleted += new System.EventHandler(this.ugDetails_AfterRowsDeleted);
            this.ugDetails.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.ugDetails_AfterColRegionScroll);
            this.ugDetails.AfterRowRegionScroll += new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.ugDetails_AfterRowRegionScroll);
            this.ugDetails.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_ClickCellButton);
            this.ugDetails.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugDetails_AfterSelectChange);
            this.ugDetails.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ugDetails_BeforeSelectChange);
            this.ugDetails.BeforeCellActivate += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugDetails_BeforeCellActivate);
            this.ugDetails.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugDetails_BeforeCellDeactivate);
            this.ugDetails.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ugDetails_BeforeRowDeactivate);
            this.ugDetails.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugDetails_BeforeCellUpdate);
            this.ugDetails.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugDetails_BeforeRowsDeleted);
            this.ugDetails.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.ugDetails_Error);
            this.ugDetails.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugDetails_AfterSortChange);
            this.ugDetails.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugDetails_MouseEnterElement);
            this.ugDetails.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugDetails_DragDrop);
            this.ugDetails.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugDetails_DragEnter);
            this.ugDetails.DragOver += new System.Windows.Forms.DragEventHandler(this.ugDetails_DragOver);
            this.ugDetails.DragLeave += new System.EventHandler(this.ugDetails_DragLeave);
            this.ugDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugDetails_KeyDown);
            this.ugDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grid_MouseDown);
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
            // AllocationWorkspaceExplorer
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panel1);
            this.Name = "AllocationWorkspaceExplorer";
            this.Size = new System.Drawing.Size(1115, 600);
            this.Load += new System.EventHandler(this.AllocationWorkspaceExplorer_Load);
            this.SizeChanged += new System.EventHandler(this.AllocationWorkspaceExplorer_SizeChanged);
            this.Enter += new System.EventHandler(this.AllocationWorkspaceExplorer_Enter);
            this.Leave += new System.EventHandler(this.AllocationWorkspaceExplorer_Leave);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.splitContainer, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerToolbarsManager)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugHeaders)).EndInit();
            this.cmsGrid.ResumeLayout(false);
            this.cmsAssrt.ResumeLayout(false);
            this.cmsMultiMenu.ResumeLayout(false);
            this.cmsAddMenu.ResumeLayout(false);
            this.cmsAddColorMenu.ResumeLayout(false);
            this.splitContainerLower.Panel1.ResumeLayout(false);
            this.splitContainerLower.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLower)).EndInit();
            this.splitContainerLower.ResumeLayout(false);
            this.Panel1_Fill_Panel.ClientArea.ResumeLayout(false);
            this.Panel1_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.detailToolbarsManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugDetails)).EndInit();
            this.cmsColumnChooser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private System.Windows.Forms.ToolStripMenuItem cmsDetails;
        private System.Windows.Forms.ToolStripMenuItem cmsSave;
        private System.Windows.Forms.ToolStripMenuItem cmsCancel;
        private System.Windows.Forms.ContextMenuStrip cmsAddMenu;
        private System.Windows.Forms.ToolStripMenuItem cmsAddHeader;
        private System.Windows.Forms.ToolStripMenuItem cmsAddPack;
        private System.Windows.Forms.ToolStripMenuItem cmsAddPackColor;
        private System.Windows.Forms.ToolStripMenuItem cmsAddPackSize;
        private System.Windows.Forms.ToolStripMenuItem cmsAddBulkColor;
        private System.Windows.Forms.ToolStripMenuItem cmsAddBulkSize;
        private System.Windows.Forms.ContextMenuStrip cmsMultiMenu;
        private System.Windows.Forms.ToolStripMenuItem cmsMultiCreate;
        private System.Windows.Forms.ToolStripMenuItem cmsMultiAddTo;
        private System.Windows.Forms.ToolStripMenuItem cmsMultiRemove;

        private System.Windows.Forms.SplitContainer splitContainer;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugHeaders;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugDetails;
        private System.Windows.Forms.ContextMenuStrip cmsGrid;
        private System.Windows.Forms.ToolStripMenuItem cmsReview;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem cmsSearch;
        private System.Windows.Forms.ToolStripMenuItem cmsAssortment;
        private System.Windows.Forms.ToolStripMenuItem cmsMulti;
        private System.Windows.Forms.ToolStripMenuItem cmsAdd;
        private System.Windows.Forms.ToolStripMenuItem cmsDelete;
        private System.Windows.Forms.ToolStripMenuItem cmsRemove;
        private System.Windows.Forms.ToolStripMenuItem cmsExpand;
        private System.Windows.Forms.ToolStripMenuItem cmsCollapse;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewSelect;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewStyle;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewSize;
        private System.Windows.Forms.ToolStripMenuItem cmsReviewSummary;
        private System.Windows.Forms.ContextMenuStrip cmsAssrt;
        private System.Windows.Forms.ToolStripMenuItem cmsAssrtCreate;
        private System.Windows.Forms.ToolStripMenuItem cmsAssrtAddTo;
        private System.Windows.Forms.ToolStripMenuItem cmsAssrtRemove; 
        #endregion
        private System.Windows.Forms.ContextMenuStrip cmsAddColorMenu;
        private System.Windows.Forms.ToolStripMenuItem cmsAddMTColor;
        private System.Windows.Forms.ToolStripMenuItem cmsChooseColor;
        private System.Windows.Forms.ToolStripMenuItem cmsAssrtAddPlaceholder;
        private System.Windows.Forms.ToolStripMenuItem cmsAutoSelectGroup;
        private System.Windows.Forms.ToolStripMenuItem cmsSaveView;
        private System.Windows.Forms.ImageList imageListDrag;
        private System.Windows.Forms.ToolStripMenuItem cmsSaveAs;
        private System.Windows.Forms.ToolStripTextBox cmsAssrtAddPhTextBox;
        private System.Windows.Forms.SplitContainer splitContainerLower;
        private System.Windows.Forms.Button btnExpandCollapse;
        private System.Windows.Forms.Button btnEditSave;
        private System.Windows.Forms.Button btnHideDetails;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip cmsColumnChooser;
        private System.Windows.Forms.ToolStripMenuItem cmsColSelectAll;
		private System.Windows.Forms.ToolStripMenuItem cmsColClearAll;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager headerToolbarsManager;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _UserActivityControl_Toolbars_Dock_Area_Top;
		private ToolStripMenuItem cmsReviewGroupAllocation;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager detailToolbarsManager;
        private Infragistics.Win.Misc.UltraPanel Panel1_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Panel1_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Panel1_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Panel1_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Panel1_Toolbars_Dock_Area_Top;
        private MIDComboBoxEnh midComboBoxFilter;
        private MIDComboBoxEnh midComboBoxView;
        private MIDComboBoxEnh midComboBoxAction;
       
    }
}
