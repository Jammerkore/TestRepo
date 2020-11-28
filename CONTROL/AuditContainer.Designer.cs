//namespace MIDRetail.Windows.Controls
//{
//    partial class AuditContainer
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }

//            //remove handlers
//            this.ugAudit.AfterRowsDeleted -= new System.EventHandler(this.ugAudit_AfterRowsDeleted);
//            this.ugAudit.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugAudit_BeforeRowsDeleted);
//            this.ugAudit.BeforeRowExpanded -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugAudit_BeforeRowExpanded);
//            this.ugAudit.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAudit_InitializeLayout);

//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
//            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.OptionSet optionSet1 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartDock");
//            Infragistics.Win.UltraWinToolbars.OptionSet optionSet2 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.OptionSet optionSet3 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartType");
//            Infragistics.Win.UltraWinToolbars.OptionSet optionSet4 = new Infragistics.Win.UltraWinToolbars.OptionSet("MessageLevel");
//            Infragistics.Win.UltraWinToolbars.OptionSet optionSet5 = new Infragistics.Win.UltraWinToolbars.OptionSet("ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("My Activity Toolbar");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("messageMenuPopup");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridMenuPopup");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btn_Filter");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btn_Refresh");
//            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Message Level Toolbar");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblMessageLevel");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("messageLevelComboBox");
//            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("gridSearchText");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("messageClear");
//            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditContainer));
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartMenuPopup");
//            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartShowHide", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartDockMenuPopup");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartTypeMenuPopup");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartLegendMenuPopup");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartTitleMenuPopup");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("chartExport");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartShowHide", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool9 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartTypeMenuPopup");
//            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypeBar", "ChartType");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypeLine", "ChartType");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypePie", "ChartType");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypePyramid", "ChartType");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypeHistogram", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool10 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("messageMenuPopup");
//            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool3 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("messageTextMaxLimit");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("messageSettingsSave");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("messageSettingsReset");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool11 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridMenuPopup");
//            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowGroupArea", "");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowFilterRow", "");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridChooseColumns");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool12 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridExportMenuPopup");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool13 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("GridEmailPopupMenu");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypeBar", "ChartType");
//            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypeLine", "ChartType");
//            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypePie", "ChartType");
//            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypePyramid", "ChartType");
//            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool15 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowGroupArea", "");
//            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool4 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("messageTextMaxLimit");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool14 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartDockMenuPopup");
//            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool16 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartDockLeft", "ChartDock");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool17 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartDockBottom", "ChartDock");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool18 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartDockRight", "ChartDock");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool19 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartDockLeft", "ChartDock");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool20 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartDockBottom", "ChartDock");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool21 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartDockRight", "ChartDock");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("messageLevelComboBox");
//            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
//            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
//            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
//            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
//            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
//            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
//            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
//            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridSearchFindButton");
//            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridSearchClearButton");
//            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExportSelected");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExportAll");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("messageSettingsSave");
//            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool23 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("gridShowFilterRow", "");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool24 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartShowLegend", "");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool25 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartShowRowLabels", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool15 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("gridExportMenuPopup");
//            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExportAll");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridExportSelected");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool16 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartLegendMenuPopup");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool26 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartShowLegend", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool17 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartLegendLocationMenuPopup");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool18 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartLegendLocationMenuPopup");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool27 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendTop", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool28 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendLeft", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool29 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendRight", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool30 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendBottom", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool31 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendTop", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool32 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendLeft", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool33 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendRight", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool34 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartLegendBottom", "ChartLegendLocation");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("messageSettingsReset");
//            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridChooseColumns");
//            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool19 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartTitleMenuPopup");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool35 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleShowHide", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool20 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartTitleLocationMenuPopup");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool36 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleShowHide", "");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool21 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("chartTitleLocationMenuPopup");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool37 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationTop", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool38 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationLeft", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool39 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationRight", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool40 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationBottom", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool41 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationTop", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool42 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationLeft", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool43 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationRight", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool44 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTitleLocationBottom", "ChartTitleLocation");
//            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool45 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("chartTypeHistogram", "");
//            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("chartExport");
//            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool22 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("GridEmailPopupMenu");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailAllRows");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailSelectedRows");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailAllRows");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("gridEmailSelectedRows");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblMessageLevel");
//            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblSearch");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCancel");
//            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btn_Delete");
//            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btn_Filter");
//            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btn_Refresh");
//            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
//            this.AuditContainer_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
//            this.ultraChart1 = new Infragistics.Win.UltraWinChart.UltraChart();
//            this.ugAudit = new Infragistics.Win.UltraWinGrid.UltraGrid();
//            this._AuditContainer_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
//            this._AuditContainer_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this._AuditContainer_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this._AuditContainer_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this.chartSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
//            this.AuditContainer_Fill_Panel.ClientArea.SuspendLayout();
//            this.AuditContainer_Fill_Panel.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraChart1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ugAudit)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // AuditContainer_Fill_Panel
//            // 
//            // 
//            // AuditContainer_Fill_Panel.ClientArea
//            // 
//            this.AuditContainer_Fill_Panel.ClientArea.Controls.Add(this.ultraChart1);
//            this.AuditContainer_Fill_Panel.ClientArea.Controls.Add(this.ugAudit);
//            this.AuditContainer_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
//            this.AuditContainer_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.AuditContainer_Fill_Panel.Location = new System.Drawing.Point(0, 75);
//            this.AuditContainer_Fill_Panel.Name = "AuditContainer_Fill_Panel";
//            this.AuditContainer_Fill_Panel.Size = new System.Drawing.Size(992, 75);
//            this.AuditContainer_Fill_Panel.TabIndex = 0;
//            // 
////			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
////			'ChartType' must be persisted ahead of any Axes change made in design time.
////		
//            this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.HistogramChart;
//            // 
//            // ultraChart1
//            // 
//            this.ultraChart1.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
//            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
//            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
//            this.ultraChart1.Axis.PE = paintElement1;
//            this.ultraChart1.Axis.X.Labels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.X.Labels.FontColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
//            this.ultraChart1.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
//            this.ultraChart1.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.FormatString = "";
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
//            this.ultraChart1.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.X.LineThickness = 1;
//            this.ultraChart1.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
//            this.ultraChart1.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.X.MajorGridLines.Visible = true;
//            this.ultraChart1.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
//            this.ultraChart1.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.X.MinorGridLines.Visible = false;
//            this.ultraChart1.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
//            this.ultraChart1.Axis.X.Visible = true;
//            this.ultraChart1.Axis.X2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
//            this.ultraChart1.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
//            this.ultraChart1.Axis.X2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
//            this.ultraChart1.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.FormatString = "";
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
//            this.ultraChart1.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.X2.Labels.Visible = false;
//            this.ultraChart1.Axis.X2.LineThickness = 1;
//            this.ultraChart1.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
//            this.ultraChart1.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.X2.MajorGridLines.Visible = true;
//            this.ultraChart1.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
//            this.ultraChart1.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.X2.MinorGridLines.Visible = false;
//            this.ultraChart1.Axis.X2.TickmarkInterval = 1D;
//            this.ultraChart1.Axis.X2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
//            this.ultraChart1.Axis.X2.Visible = false;
//            this.ultraChart1.Axis.Y.Labels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
//            this.ultraChart1.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
//            this.ultraChart1.Axis.Y.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.FormatString = "";
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Y.LineThickness = 1;
//            this.ultraChart1.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
//            this.ultraChart1.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Y.MajorGridLines.Visible = true;
//            this.ultraChart1.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
//            this.ultraChart1.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Y.MinorGridLines.Visible = false;
//            this.ultraChart1.Axis.Y.TickmarkInterval = 2D;
//            this.ultraChart1.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
//            this.ultraChart1.Axis.Y.Visible = true;
//            this.ultraChart1.Axis.Y2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
//            this.ultraChart1.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
//            this.ultraChart1.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
//            this.ultraChart1.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.FormatString = "";
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Y2.LineThickness = 1;
//            this.ultraChart1.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
//            this.ultraChart1.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Y2.MajorGridLines.Visible = true;
//            this.ultraChart1.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
//            this.ultraChart1.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Y2.MinorGridLines.Visible = false;
//            this.ultraChart1.Axis.Y2.TickmarkInterval = 0.1D;
//            this.ultraChart1.Axis.Y2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
//            this.ultraChart1.Axis.Y2.Visible = true;
//            this.ultraChart1.Axis.Z.Labels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
//            this.ultraChart1.Axis.Z.Labels.ItemFormatString = "";
//            this.ultraChart1.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
//            this.ultraChart1.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Z.LineThickness = 1;
//            this.ultraChart1.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
//            this.ultraChart1.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Z.MajorGridLines.Visible = true;
//            this.ultraChart1.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
//            this.ultraChart1.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Z.MinorGridLines.Visible = false;
//            this.ultraChart1.Axis.Z.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
//            this.ultraChart1.Axis.Z.Visible = false;
//            this.ultraChart1.Axis.Z2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
//            this.ultraChart1.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
//            this.ultraChart1.Axis.Z2.Labels.ItemFormatString = "";
//            this.ultraChart1.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
//            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
//            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
//            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
//            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
//            this.ultraChart1.Axis.Z2.Labels.Visible = false;
//            this.ultraChart1.Axis.Z2.LineThickness = 1;
//            this.ultraChart1.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
//            this.ultraChart1.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Z2.MajorGridLines.Visible = true;
//            this.ultraChart1.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
//            this.ultraChart1.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
//            this.ultraChart1.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
//            this.ultraChart1.Axis.Z2.MinorGridLines.Visible = false;
//            this.ultraChart1.Axis.Z2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
//            this.ultraChart1.Axis.Z2.Visible = false;
//            this.ultraChart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
//            this.ultraChart1.ColorModel.AlphaLevel = ((byte)(150));
//            this.ultraChart1.ColorModel.ColorBegin = System.Drawing.Color.Pink;
//            this.ultraChart1.ColorModel.ColorEnd = System.Drawing.Color.DarkRed;
//            this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
//            this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.ultraChart1.Effects.Effects.Add(gradientEffect1);
//            this.ultraChart1.Location = new System.Drawing.Point(657, 0);
//            this.ultraChart1.Name = "ultraChart1";
//            this.ultraChart1.Size = new System.Drawing.Size(335, 75);
//            this.ultraChart1.TabIndex = 6;
//            this.ultraChart1.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
//            this.ultraChart1.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
//            // 
//            // ugAudit
//            // 
//            appearance1.BackColor = System.Drawing.Color.White;
//            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
//            this.ugAudit.DisplayLayout.Appearance = appearance1;
//            this.ugAudit.DisplayLayout.GroupByBox.Prompt = "Drag here to group by column";
//            this.ugAudit.DisplayLayout.InterBandSpacing = 10;
//            appearance2.BackColor = System.Drawing.Color.Transparent;
//            this.ugAudit.DisplayLayout.Override.CardAreaAppearance = appearance2;
//            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance3.ForeColor = System.Drawing.Color.Black;
//            appearance3.TextHAlignAsString = "Left";
//            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
//            this.ugAudit.DisplayLayout.Override.HeaderAppearance = appearance3;
//            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.ugAudit.DisplayLayout.Override.RowAppearance = appearance4;
//            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            this.ugAudit.DisplayLayout.Override.RowSelectorAppearance = appearance5;
//            this.ugAudit.DisplayLayout.Override.RowSelectorWidth = 12;
//            this.ugAudit.DisplayLayout.Override.RowSpacingBefore = 2;
//            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance6.ForeColor = System.Drawing.Color.Black;
//            this.ugAudit.DisplayLayout.Override.SelectedRowAppearance = appearance6;
//            this.ugAudit.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.ugAudit.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
//            this.ugAudit.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.ugAudit.Location = new System.Drawing.Point(0, 0);
//            this.ugAudit.Name = "ugAudit";
//            this.ugAudit.Size = new System.Drawing.Size(992, 75);
//            this.ugAudit.TabIndex = 2;
//            this.ugAudit.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAudit_InitializeLayout);
//            this.ugAudit.AfterRowsDeleted += new System.EventHandler(this.ugAudit_AfterRowsDeleted);
//            this.ugAudit.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugAudit_BeforeRowExpanded);
//            this.ugAudit.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugAudit_BeforeRowsDeleted);
//            // 
//            // _AuditContainer_Toolbars_Dock_Area_Left
//            // 
//            this._AuditContainer_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._AuditContainer_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
//            this._AuditContainer_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
//            this._AuditContainer_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._AuditContainer_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 75);
//            this._AuditContainer_Toolbars_Dock_Area_Left.Name = "_AuditContainer_Toolbars_Dock_Area_Left";
//            this._AuditContainer_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 75);
//            this._AuditContainer_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // ultraToolbarsManager1
//            // 
//            this.ultraToolbarsManager1.DesignerFlags = 1;
//            this.ultraToolbarsManager1.DockWithinContainer = this;
//            this.ultraToolbarsManager1.MenuAnimationStyle = Infragistics.Win.UltraWinToolbars.MenuAnimationStyle.Slide;
//            this.ultraToolbarsManager1.MenuSettings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
//            optionSet2.AllowAllUp = false;
//            optionSet3.AllowAllUp = false;
//            optionSet5.AllowAllUp = false;
//            this.ultraToolbarsManager1.OptionSets.Add(optionSet1);
//            this.ultraToolbarsManager1.OptionSets.Add(optionSet2);
//            this.ultraToolbarsManager1.OptionSets.Add(optionSet3);
//            this.ultraToolbarsManager1.OptionSets.Add(optionSet4);
//            this.ultraToolbarsManager1.OptionSets.Add(optionSet5);
//            this.ultraToolbarsManager1.RuntimeCustomizationOptions = ((Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions)((Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.AllowCustomizeDialog | Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.AllowAltClickToolDragging)));
//            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
//            ultraToolbar1.DockedColumn = 0;
//            ultraToolbar1.DockedRow = 0;
//            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            popupMenuTool1,
//            popupMenuTool2,
//            buttonTool33,
//            buttonTool29});
//            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
//            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
//            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
//            ultraToolbar1.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
//            ultraToolbar1.Settings.ToolSpacing = 4;
//            ultraToolbar1.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.False;
//            ultraToolbar1.Text = "My Activity Toolbar";
//            ultraToolbar2.DockedColumn = 0;
//            ultraToolbar2.DockedRow = 1;
//            comboBoxTool1.InstanceProps.Width = 111;
//            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            labelTool2,
//            comboBoxTool1});
//            ultraToolbar2.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
//            ultraToolbar2.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.True;
//            ultraToolbar2.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
//            ultraToolbar2.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.True;
//            ultraToolbar2.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
//            ultraToolbar2.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
//            ultraToolbar2.Text = "Message Level Toolbar";
//            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
//            ultraToolbar1,
//            ultraToolbar2});
//            textBoxTool2.SharedPropsInternal.Caption = "Search:";
//            textBoxTool2.SharedPropsInternal.Category = "Grid Search";
//            textBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
//            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
//            buttonTool3.SharedPropsInternal.AppearancesSmall.Appearance = appearance7;
//            buttonTool3.SharedPropsInternal.Caption = "Clear Messages";
//            buttonTool3.SharedPropsInternal.Category = "Messages";
//            buttonTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
//            popupMenuTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
//            popupMenuTool4.SharedPropsInternal.Caption = "Chart";
//            popupMenuTool4.SharedPropsInternal.Category = "Chart";
//            popupMenuTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            popupMenuTool4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool1,
//            popupMenuTool5,
//            popupMenuTool6,
//            popupMenuTool7,
//            popupMenuTool8,
//            buttonTool4});
//            stateButtonTool2.SharedPropsInternal.Caption = "Show Chart";
//            stateButtonTool2.SharedPropsInternal.Category = "Chart";
//            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
//            popupMenuTool9.SharedPropsInternal.AppearancesSmall.Appearance = appearance9;
//            popupMenuTool9.SharedPropsInternal.Caption = "Chart Type";
//            popupMenuTool9.SharedPropsInternal.Category = "Chart Type";
//            popupMenuTool9.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool3,
//            stateButtonTool4,
//            stateButtonTool5,
//            stateButtonTool6,
//            stateButtonTool7});
//            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
//            popupMenuTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
//            popupMenuTool10.SharedPropsInternal.Caption = "Messages";
//            popupMenuTool10.SharedPropsInternal.Category = "Messages";
//            popupMenuTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            popupMenuTool10.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            textBoxTool3,
//            buttonTool6,
//            buttonTool7});
//            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
//            popupMenuTool11.SharedPropsInternal.AppearancesSmall.Appearance = appearance11;
//            popupMenuTool11.SharedPropsInternal.Caption = "Grid";
//            popupMenuTool11.SharedPropsInternal.Category = "Grid";
//            popupMenuTool11.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            stateButtonTool9.Checked = true;
//            popupMenuTool11.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool9,
//            stateButtonTool10,
//            buttonTool8,
//            popupMenuTool12,
//            popupMenuTool13});
//            stateButtonTool11.OptionSetKey = "ChartType";
//            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
//            stateButtonTool11.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
//            stateButtonTool11.SharedPropsInternal.Caption = "Bar";
//            stateButtonTool11.SharedPropsInternal.Category = "Chart Type";
//            stateButtonTool12.OptionSetKey = "ChartType";
//            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
//            stateButtonTool12.SharedPropsInternal.AppearancesSmall.Appearance = appearance13;
//            stateButtonTool12.SharedPropsInternal.Caption = "Line";
//            stateButtonTool12.SharedPropsInternal.Category = "Chart Type";
//            stateButtonTool13.OptionSetKey = "ChartType";
//            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
//            stateButtonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance14;
//            stateButtonTool13.SharedPropsInternal.Caption = "Pie";
//            stateButtonTool13.SharedPropsInternal.Category = "Chart Type";
//            stateButtonTool14.OptionSetKey = "ChartType";
//            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
//            stateButtonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance15;
//            stateButtonTool14.SharedPropsInternal.Caption = "Pyramid";
//            stateButtonTool14.SharedPropsInternal.Category = "Chart Type";
//            stateButtonTool15.Checked = true;
//            stateButtonTool15.SharedPropsInternal.Caption = "Show Group Area";
//            stateButtonTool15.SharedPropsInternal.Category = "Grid";
//            textBoxTool4.SharedPropsInternal.Caption = "Max Messages:";
//            textBoxTool4.SharedPropsInternal.Category = "Messages";
//            textBoxTool4.SharedPropsInternal.ToolTipText = "The maximum number of application messages to be stored in memory.  Older message" +
//    "s will be removed first.";
//            textBoxTool4.SharedPropsInternal.ToolTipTextFormatted = "Determines the maximum number of application messages to be stored in memory.<br/" +
//    ">When the maximum limit is reached, older messages will be removed from memory.";
//            textBoxTool4.Text = "5000";
//            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
//            popupMenuTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance16;
//            popupMenuTool14.SharedPropsInternal.Caption = "Dock";
//            popupMenuTool14.SharedPropsInternal.Category = "Chart Dock";
//            stateButtonTool18.Checked = true;
//            popupMenuTool14.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool16,
//            stateButtonTool17,
//            stateButtonTool18});
//            stateButtonTool19.OptionSetKey = "ChartDock";
//            stateButtonTool19.SharedPropsInternal.Caption = "Left";
//            stateButtonTool19.SharedPropsInternal.Category = "Chart Dock";
//            stateButtonTool20.OptionSetKey = "ChartDock";
//            stateButtonTool20.SharedPropsInternal.Caption = "Bottom";
//            stateButtonTool20.SharedPropsInternal.Category = "Chart Dock";
//            stateButtonTool21.Checked = true;
//            stateButtonTool21.OptionSetKey = "ChartDock";
//            stateButtonTool21.SharedPropsInternal.Caption = "Right";
//            stateButtonTool21.SharedPropsInternal.Category = "Chart Dock";
//            comboBoxTool2.AutoComplete = true;
//            appearance17.BackColor = System.Drawing.SystemColors.Control;
//            comboBoxTool2.EditAppearance = appearance17;
//            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
//            comboBoxTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance18;
//            comboBoxTool2.SharedPropsInternal.Caption = "Message Level:";
//            comboBoxTool2.SharedPropsInternal.Category = "Messages";
//            comboBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
//            comboBoxTool2.Text = "Warning";
//            appearance19.Image = ((object)(resources.GetObject("appearance19.Image")));
//            valueListItem1.Appearance = appearance19;
//            valueListItem1.DataValue = "Debug";
//            valueListItem1.DisplayText = "Debug";
//            appearance20.Image = ((object)(resources.GetObject("appearance20.Image")));
//            valueListItem2.Appearance = appearance20;
//            valueListItem2.DataValue = "Information";
//            valueListItem2.DisplayText = "Information";
//            appearance21.Image = ((object)(resources.GetObject("appearance21.Image")));
//            valueListItem3.Appearance = appearance21;
//            valueListItem3.DataValue = "Edit";
//            valueListItem3.DisplayText = "Edit";
//            appearance22.Image = ((object)(resources.GetObject("appearance22.Image")));
//            valueListItem4.Appearance = appearance22;
//            valueListItem4.DataValue = "Warning";
//            valueListItem4.DisplayText = "Warning";
//            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
//            valueListItem5.Appearance = appearance23;
//            valueListItem5.DataValue = "Error";
//            valueListItem5.DisplayText = "Error";
//            appearance24.Image = ((object)(resources.GetObject("appearance24.Image")));
//            valueListItem6.Appearance = appearance24;
//            valueListItem6.DataValue = "Severe";
//            valueListItem6.DisplayText = "Severe";
//            valueList1.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
//            valueListItem1,
//            valueListItem2,
//            valueListItem3,
//            valueListItem4,
//            valueListItem5,
//            valueListItem6});
//            comboBoxTool2.ValueList = valueList1;
//            appearance25.Image = ((object)(resources.GetObject("appearance25.Image")));
//            buttonTool9.SharedPropsInternal.AppearancesSmall.Appearance = appearance25;
//            buttonTool9.SharedPropsInternal.Caption = "Find";
//            buttonTool9.SharedPropsInternal.Category = "Grid Search";
//            buttonTool9.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
//            buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance26;
//            buttonTool10.SharedPropsInternal.Caption = "Clear";
//            buttonTool10.SharedPropsInternal.Category = "Grid Search";
//            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            buttonTool11.SharedPropsInternal.Caption = "Export Selected Rows to Excel";
//            buttonTool11.SharedPropsInternal.Category = "Grid Export";
//            buttonTool12.SharedPropsInternal.Caption = "Export All Rows to Excel";
//            buttonTool12.SharedPropsInternal.Category = "Grid Export";
//            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
//            buttonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance27;
//            buttonTool13.SharedPropsInternal.Caption = "Save Settings";
//            buttonTool13.SharedPropsInternal.Category = "Messages";
//            stateButtonTool23.SharedPropsInternal.Caption = "Show Filter Row";
//            stateButtonTool23.SharedPropsInternal.Category = "Grid";
//            stateButtonTool24.Checked = true;
//            stateButtonTool24.SharedPropsInternal.Caption = "Show Legend";
//            stateButtonTool24.SharedPropsInternal.Category = "Chart";
//            stateButtonTool25.Checked = true;
//            stateButtonTool25.SharedPropsInternal.Caption = "Show Row Labels";
//            stateButtonTool25.SharedPropsInternal.Category = "Chart";
//            appearance28.Image = ((object)(resources.GetObject("appearance28.Image")));
//            popupMenuTool15.SharedPropsInternal.AppearancesSmall.Appearance = appearance28;
//            popupMenuTool15.SharedPropsInternal.Caption = "Export";
//            popupMenuTool15.SharedPropsInternal.Category = "Grid Export";
//            popupMenuTool15.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            buttonTool14,
//            buttonTool15});
//            popupMenuTool16.SharedPropsInternal.Caption = "Legend";
//            popupMenuTool16.SharedPropsInternal.Category = "Chart Legend";
//            stateButtonTool26.Checked = true;
//            popupMenuTool16.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool26,
//            popupMenuTool17});
//            popupMenuTool18.SharedPropsInternal.Caption = "Location";
//            popupMenuTool18.SharedPropsInternal.Category = "Chart Legend";
//            stateButtonTool29.Checked = true;
//            popupMenuTool18.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool27,
//            stateButtonTool28,
//            stateButtonTool29,
//            stateButtonTool30});
//            stateButtonTool31.OptionSetKey = "ChartLegendLocation";
//            stateButtonTool31.SharedPropsInternal.Caption = "Top";
//            stateButtonTool31.SharedPropsInternal.Category = "Chart Legend";
//            stateButtonTool32.OptionSetKey = "ChartLegendLocation";
//            stateButtonTool32.SharedPropsInternal.Caption = "Left";
//            stateButtonTool32.SharedPropsInternal.Category = "Chart Legend";
//            stateButtonTool33.Checked = true;
//            stateButtonTool33.OptionSetKey = "ChartLegendLocation";
//            stateButtonTool33.SharedPropsInternal.Caption = "Right";
//            stateButtonTool33.SharedPropsInternal.Category = "Chart Legend";
//            stateButtonTool34.OptionSetKey = "ChartLegendLocation";
//            stateButtonTool34.SharedPropsInternal.Caption = "Bottom";
//            stateButtonTool34.SharedPropsInternal.Category = "Chart Legend";
//            appearance29.Image = ((object)(resources.GetObject("appearance29.Image")));
//            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance29;
//            buttonTool16.SharedPropsInternal.Caption = "Reset Settings";
//            buttonTool16.SharedPropsInternal.Category = "Messages";
//            appearance30.Image = ((object)(resources.GetObject("appearance30.Image")));
//            buttonTool17.SharedPropsInternal.AppearancesSmall.Appearance = appearance30;
//            buttonTool17.SharedPropsInternal.Caption = "Choose Columns";
//            buttonTool17.SharedPropsInternal.Category = "Grid";
//            popupMenuTool19.SharedPropsInternal.Caption = "Title";
//            popupMenuTool19.SharedPropsInternal.Category = "Chart Title";
//            popupMenuTool19.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool35,
//            popupMenuTool20});
//            stateButtonTool36.SharedPropsInternal.Caption = "Show Title";
//            stateButtonTool36.SharedPropsInternal.Category = "Chart Title";
//            popupMenuTool21.SharedPropsInternal.Caption = "Location";
//            popupMenuTool21.SharedPropsInternal.Category = "Chart Title";
//            stateButtonTool37.Checked = true;
//            popupMenuTool21.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            stateButtonTool37,
//            stateButtonTool38,
//            stateButtonTool39,
//            stateButtonTool40});
//            stateButtonTool41.Checked = true;
//            stateButtonTool41.OptionSetKey = "ChartTitleLocation";
//            stateButtonTool41.SharedPropsInternal.Caption = "Top";
//            stateButtonTool41.SharedPropsInternal.Category = "Chart Title";
//            stateButtonTool42.OptionSetKey = "ChartTitleLocation";
//            stateButtonTool42.SharedPropsInternal.Caption = "Left";
//            stateButtonTool42.SharedPropsInternal.Category = "Chart Title";
//            stateButtonTool43.OptionSetKey = "ChartTitleLocation";
//            stateButtonTool43.SharedPropsInternal.Caption = "Right";
//            stateButtonTool43.SharedPropsInternal.Category = "Chart Title";
//            stateButtonTool44.OptionSetKey = "ChartTitleLocation";
//            stateButtonTool44.SharedPropsInternal.Caption = "Bottom";
//            stateButtonTool44.SharedPropsInternal.Category = "Chart Title";
//            appearance31.Image = ((object)(resources.GetObject("appearance31.Image")));
//            stateButtonTool45.SharedPropsInternal.AppearancesSmall.Appearance = appearance31;
//            stateButtonTool45.SharedPropsInternal.Caption = "Histogram";
//            stateButtonTool45.SharedPropsInternal.Category = "Chart Type";
//            buttonTool18.SharedPropsInternal.Caption = "Export to PDF";
//            buttonTool18.SharedPropsInternal.Category = "Chart";
//            popupMenuTool22.SharedPropsInternal.AppearancesSmall.Appearance = appearance31;
//            popupMenuTool22.SharedPropsInternal.Caption = "Email";
//            popupMenuTool22.SharedPropsInternal.Category = "Grid Export";
//            popupMenuTool22.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            buttonTool19,
//            buttonTool20});
//            buttonTool21.SharedPropsInternal.Caption = "Email All Rows";
//            buttonTool21.SharedPropsInternal.Category = "Grid Export";
//            buttonTool22.SharedPropsInternal.Caption = "Email Selected Rows";
//            buttonTool22.SharedPropsInternal.Category = "Grid Export";
//            appearance32.Image = ((object)(resources.GetObject("appearance32.Image")));
//            labelTool3.SharedPropsInternal.AppearancesSmall.Appearance = appearance32;
//            labelTool3.SharedPropsInternal.Caption = "Message Level:";
//            labelTool3.SharedPropsInternal.Category = "Messages";
//            labelTool4.SharedPropsInternal.Caption = "Search:";
//            labelTool4.SharedPropsInternal.Category = "Grid Search";
//            appearance33.Image = ((object)(resources.GetObject("appearance33.Image")));
//            buttonTool24.SharedPropsInternal.AppearancesSmall.Appearance = appearance33;
//            buttonTool24.SharedPropsInternal.Caption = "Cancel";
//            buttonTool24.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            appearance34.Image = ((object)(resources.GetObject("appearance34.Image")));
//            buttonTool26.SharedPropsInternal.AppearancesSmall.Appearance = appearance34;
//            buttonTool26.SharedPropsInternal.Caption = "Delete";
//            buttonTool26.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            appearance35.Image = ((object)(resources.GetObject("appearance35.Image")));
//            buttonTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance35;
//            buttonTool28.SharedPropsInternal.Caption = "Filter";
//            buttonTool28.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            appearance36.Image = ((object)(resources.GetObject("appearance36.Image")));
//            buttonTool30.SharedPropsInternal.AppearancesSmall.Appearance = appearance36;
//            buttonTool30.SharedPropsInternal.Caption = "Refresh";
//            buttonTool30.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
//            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//            textBoxTool2,
//            buttonTool3,
//            popupMenuTool4,
//            stateButtonTool2,
//            popupMenuTool9,
//            popupMenuTool10,
//            popupMenuTool11,
//            stateButtonTool11,
//            stateButtonTool12,
//            stateButtonTool13,
//            stateButtonTool14,
//            stateButtonTool15,
//            textBoxTool4,
//            popupMenuTool14,
//            stateButtonTool19,
//            stateButtonTool20,
//            stateButtonTool21,
//            comboBoxTool2,
//            buttonTool9,
//            buttonTool10,
//            buttonTool11,
//            buttonTool12,
//            buttonTool13,
//            stateButtonTool23,
//            stateButtonTool24,
//            stateButtonTool25,
//            popupMenuTool15,
//            popupMenuTool16,
//            popupMenuTool18,
//            stateButtonTool31,
//            stateButtonTool32,
//            stateButtonTool33,
//            stateButtonTool34,
//            buttonTool16,
//            buttonTool17,
//            popupMenuTool19,
//            stateButtonTool36,
//            popupMenuTool21,
//            stateButtonTool41,
//            stateButtonTool42,
//            stateButtonTool43,
//            stateButtonTool44,
//            stateButtonTool45,
//            buttonTool18,
//            popupMenuTool22,
//            buttonTool21,
//            buttonTool22,
//            labelTool3,
//            labelTool4,
//            buttonTool24,
//            buttonTool26,
//            buttonTool28,
//            buttonTool30});
//            // 
//            // _AuditContainer_Toolbars_Dock_Area_Right
//            // 
//            this._AuditContainer_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._AuditContainer_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
//            this._AuditContainer_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
//            this._AuditContainer_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._AuditContainer_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1000, 75);
//            this._AuditContainer_Toolbars_Dock_Area_Right.Name = "_AuditContainer_Toolbars_Dock_Area_Right";
//            this._AuditContainer_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 75);
//            this._AuditContainer_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // _AuditContainer_Toolbars_Dock_Area_Top
//            // 
//            this._AuditContainer_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._AuditContainer_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
//            this._AuditContainer_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
//            this._AuditContainer_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._AuditContainer_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
//            this._AuditContainer_Toolbars_Dock_Area_Top.Name = "_AuditContainer_Toolbars_Dock_Area_Top";
//            this._AuditContainer_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1000, 75);
//            this._AuditContainer_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // _AuditContainer_Toolbars_Dock_Area_Bottom
//            // 
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 150);
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.Name = "_AuditContainer_Toolbars_Dock_Area_Bottom";
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1000, 0);
//            this._AuditContainer_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // chartSplitter1
//            // 
//            this.chartSplitter1.BackColor = System.Drawing.SystemColors.Control;
//            this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.chartSplitter1.Location = new System.Drawing.Point(992, 75);
//            this.chartSplitter1.Name = "chartSplitter1";
//            this.chartSplitter1.RestoreExtent = 150;
//            this.chartSplitter1.Size = new System.Drawing.Size(8, 75);
//            this.chartSplitter1.TabIndex = 6;
//            // 
//            // AuditContainer
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.AuditContainer_Fill_Panel);
//            this.Controls.Add(this.chartSplitter1);
//            this.Controls.Add(this._AuditContainer_Toolbars_Dock_Area_Left);
//            this.Controls.Add(this._AuditContainer_Toolbars_Dock_Area_Right);
//            this.Controls.Add(this._AuditContainer_Toolbars_Dock_Area_Bottom);
//            this.Controls.Add(this._AuditContainer_Toolbars_Dock_Area_Top);
//            this.Name = "AuditContainer";
//            this.Size = new System.Drawing.Size(1000, 150);
//            this.Load += new System.EventHandler(this.AuditContainer_Load);
//            this.AuditContainer_Fill_Panel.ClientArea.ResumeLayout(false);
//            this.AuditContainer_Fill_Panel.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.ultraChart1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ugAudit)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
//        private Infragistics.Win.Misc.UltraPanel AuditContainer_Fill_Panel;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AuditContainer_Toolbars_Dock_Area_Left;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AuditContainer_Toolbars_Dock_Area_Right;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AuditContainer_Toolbars_Dock_Area_Bottom;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AuditContainer_Toolbars_Dock_Area_Top;
//        private Infragistics.Win.UltraWinGrid.UltraGrid ugAudit;
//        private Infragistics.Win.Misc.UltraSplitter chartSplitter1;
//        private Infragistics.Win.UltraWinChart.UltraChart ultraChart1;
//    }
//}
