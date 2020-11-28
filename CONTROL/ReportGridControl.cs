using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;

namespace MIDRetail.Windows.Controls
{
    public enum eReportType
    {
        AuditReclassViewer,
        AllocationAuditSetup,
        AllocationByStore,
        ForecastAuditSetup,
        NodePropertiesOverride_EligibilityModifiersSimilarStore,
        NodePropertiesOverride_StoreGradesAllocationMinMax,
        NodePropertiesOverride_VelocityGrades,
        NodePropertiesOverride_PurgeCriteria,
        NodePropertiesOverride_SizeCurveCriteria,
        NodePropertiesOverride_Characteristics,
        NodePropertiesOverride_ChainSetPct,
        //NodePropertiesOverride_Modifiers,
        NodePropertiesOverride_/*AllocationMinMax*/,
        NodePropertiesOverride_Capacity,
        NodePropertiesOverride_ForecastLevel,
        NodePropertiesOverride_SizeCurveTolerance,
        NodePropertiesOverride_VSW,
        //NodePropertiesOverride_SimilarStore,
        NodePropertiesOverride_StockMinMax,
        NodePropertiesOverride_DailyPcts,
        NodePropertiesOverride_ForecastType,
        NodePropertiesOverride_SizeCurveSimilarStore,
        UserOptionsReview
    }

    public partial class ReportGridControl : UserControl
    {
        eReportType _reportType;
        string _reportName;
        string _reportTitle;
        string _reportInformation;

        public ReportGridControl()
        {
            InitializeComponent();
        }

        public void BindGrid(DataSet aDataSet, eReportType reportType, string reportName, string reportTitle, string reportInformation)
        {
            _reportType = reportType;
            _reportName = reportName;
            _reportTitle = reportTitle;
            _reportInformation = reportInformation;

            ultraGrid1.DataSource = null;

            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;

        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {


                #region "Grid Tools"

                case "gridSearchFindButton":
                    SharedControlRoutines.SearchGrid(ultraGrid1, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
                    break;
                case "gridSearchClearButton":
                    Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
                    t.Text = "";
                    SharedControlRoutines.ClearGridSearchResults(ultraGrid1);
                    break;

                case "gridShowSearchToolbar":
                    this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible = !this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible;
                    break;

                case "gridShowGroupArea":
                    this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = !this.ultraGrid1.DisplayLayout.GroupByBox.Hidden;
                    break;

                case "gridShowFilterRow":
                    if (this.ultraGrid1.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
                    {
                        this.ultraGrid1.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
                    }
                    else
                    {
                        this.ultraGrid1.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
                    }
                    break;

                case "gridExportSelected":
                    SharedControlRoutines.exportHelper.ExportSelectedRowsToExcel(ug: this.ultraGrid1, titleOnFirstRow: ultraGrid1.Text);
                    break;

                case "gridExportAll":
                    SharedControlRoutines.exportHelper.ExportAllRowsToExcel(ug: this.ultraGrid1, titleOnFirstRow: ultraGrid1.Text);
                    break;

                case "gridEmailSelectedRows":
                    SharedControlRoutines.exportHelper.EmailSelectedRows(subject: _reportName, fileName: _reportName + ".xls", ug: this.ultraGrid1, titleOnFirstRow: ultraGrid1.Text);
                    break;

                case "gridEmailAllRows":
                    SharedControlRoutines.exportHelper.EmailAllRows(subject: _reportName, fileName: _reportName + ".xls", ug: this.ultraGrid1, titleOnFirstRow: ultraGrid1.Text);
                    break;

                case "gridChooseColumns":
                    this.ultraGrid1.ShowColumnChooser("Choose Columns");
                    break;

                    #endregion

            }
        }

        private void ultraToolbarsManager1_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "gridSearchText":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(this.ultraGrid1, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
                    }
                    break;
                case "messageLevelComboBox":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                    }
                    break;
            }
        }

        private void ReportGrid_Load(object sender, EventArgs e)
        {
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
            rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            //this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.Default;


            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;

            this.ultraGrid1.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["Process Date/Time"].Format = "MM/dd/yyyy HH:mm:ss";
            //e.Layout.Bands[0].Columns["OTS Begining"].Format = "MM/dd/yyyy";
            //e.Layout.Bands[0].Columns["OTS Ending"].Format = "MM/dd/yyyy";

            //e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
            //e.Layout.Bands[0].Columns["HN_RID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            switch (_reportType)
            {
                case eReportType.AuditReclassViewer:
                    FormatAuditReclassReport();
                    break;
                case eReportType.UserOptionsReview:
                    FormatUserOptionReviewReport();
                    break;
                case eReportType.NodePropertiesOverride_ChainSetPct:
                    FormatNodePropertyChainSetPctReport();
                    break;
                case eReportType.NodePropertiesOverride_Characteristics:
                    FormatNodePropertyCharacteristicsReport();
                    break;
                case eReportType.NodePropertiesOverride_DailyPcts:
                    FormatNodePropertyDailyPctsReport();
                    break;
                case eReportType.NodePropertiesOverride_ForecastLevel:
                    FormatNodePropertyForecastLevelReport();
                    break;
                case eReportType.NodePropertiesOverride_ForecastType:
                    FormatNodePropertyForecastTypeReport();
                    break;
                case eReportType.NodePropertiesOverride_PurgeCriteria:
                    FormatNodePropertyPurgeCriteriaReport();
                    break;
                case eReportType.NodePropertiesOverride_SizeCurveCriteria:
                    FormatNodePropertySizeCurveCriteriaReport();
                    break;
                case eReportType.NodePropertiesOverride_SizeCurveSimilarStore:
                    FormatNodePropertySizeCurveSimilarStoresReport();
                    break;
                case eReportType.NodePropertiesOverride_SizeCurveTolerance:
                    FormatNodePropertySizeCurveToleranceReport();
                    break;
                case eReportType.NodePropertiesOverride_StockMinMax:
                    FormatNodePropertyStockMinMaxReport();
                    break;
                case eReportType.NodePropertiesOverride_Capacity:
                    FormatNodePropertyStoreCapacityReport();
                    break;
                case eReportType.NodePropertiesOverride_EligibilityModifiersSimilarStore:
                    FormatNodePropertyStoreEligibilityReport();
                    break;
                case eReportType.NodePropertiesOverride_StoreGradesAllocationMinMax:
                    FormatNodePropertyStoreGradesReport();
                    break;
                case eReportType.NodePropertiesOverride_VelocityGrades:
                    FormatNodePropertyVelocityGradesReport();
                    break;
                case eReportType.NodePropertiesOverride_VSW:
                    FormatNodePropertyVSWReport();
                    break;
                case eReportType.AllocationByStore:
                    FormatAllocationByStoreReport();
                    break;
                default:
                    MessageBox.Show("Need formatting method for " + _reportType.ToString());
                    break;
            }

            ultraGrid1.Text = String.Format("{0}" + Environment.NewLine + "{1}  {2}      {3}", _reportTitle, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), _reportInformation);
            ultraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.True;
            ultraGrid1.DisplayLayout.CaptionAppearance.FontData.Bold = DefaultableBoolean.True;
            ultraGrid1.DisplayLayout.CaptionAppearance.FontData.SizeInPoints = 14;
            ultraGrid1.DisplayLayout.CaptionAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            this.ultraGrid1.DisplayLayout.Bands[0].Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = true;

            //this.ultraGrid1.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            this.ultraGrid1.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);

            if (_reportType != eReportType.AllocationByStore)
            {
                this.ultraGrid1.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            }

            this.ultraGrid1.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
        }

        private void ultraGrid1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            // Force rows to expand
            e.Row.ExpandAll();
        }

        private void FormatAuditReclassReport()
        {
            RemoveColumn(columnName: "PROCESS_ID");

            SetColumn(columnName: "START_TIME", caption: "Date", format: "MMMM dd, yyyy HH:mm tt");
            SetColumn(columnName: "PROCESS_VALUE", caption: "Process");
            SetColumn(columnName: "RECLASS_SEQ", caption: "#");
            SetColumn(columnName: "RECLASS_ACTION", caption: "Action");
            SetColumn(columnName: "RECLASS_ITEM_TYPE", caption: "Type");
            SetColumn(columnName: "RECLASS_ITEM", caption: "Item");
            SetColumn(columnName: "RECLASS_COMMENT", caption: "Comment");
            
        }

        private void FormatUserOptionReviewReport()
        {
            RemoveColumn(columnName: "AUDIT_LOGGING_LEVEL");
            RemoveColumn(columnName: "FORECAST_MONITOR_ACTIVE");
            RemoveColumn(columnName: "MODIFY_SALES_MONITOR_ACTIVE");
            RemoveColumn(columnName: "DCFULFILLMENT_MONITOR_ACTIVE");

            SetColumn(columnName: "USER_NAME", caption: "UserName");
            SetColumn(columnName: "USER_FULLNAME", caption: "Full Name");
            SetColumn(columnName: "AUDIT_LOGGING_TEXT", caption: "Audit Logging Level", hAlign: Infragistics.Win.HAlign.Center);
            SetColumn(columnName: "FORECAST_MONITOR_TEXT", caption: "Forecast Monitor", hAlign: Infragistics.Win.HAlign.Center);
            SetColumn(columnName: "MODIFY_SALES_MONITOR_TEXT", caption: "Modify Sales", hAlign: Infragistics.Win.HAlign.Center);
            SetColumn(columnName: "DCFULFILLMENT_MONITOR_TEXT", caption: "DC Fulfillment Monitor", hAlign: Infragistics.Win.HAlign.Center);

        }

        private void FormatNodePropertyChainSetPctReport()
        {
            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HOME_LEVEL");

            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "ST_ID");
            RemoveColumn(columnName: "STORE_NAME");
            RemoveColumn(columnName: "SG_RID");
            RemoveColumn(columnName: "SG_ID");
            RemoveColumn(columnName: "SGL_RID");
            RemoveColumn(columnName: "SGL_SEQUENCE");
            RemoveColumn(columnName: "TIME_ID");
            RemoveColumn(columnName: "PERCENTAGE");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "SGL_ID", caption: "Attribute Set");
            
            ExpandAllRows();
        }

        private void FormatNodePropertyCharacteristicsReport()
        {
            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HOME_LEVEL");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "CHARACTERISTIC", caption: "Characteristic");
            SetColumn(columnName: "CHARACTERISTIC_VALUE", caption: "Values");
            
            ExpandAllRows();
        }

        private void FormatNodePropertyDailyPctsReport()
        {
            GroupByColumn(columnName: "STORE_TEXT");

            RemoveColumn(columnName: "Column1");
            RemoveColumn(columnName: "PARENT_HN_RID");
            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "ST_ID");
            RemoveColumn(columnName: "STORE_NAME");
            RemoveColumn(columnName: "BN_ID");
            RemoveColumn(columnName: "BN_NAME");

            SetColumn(columnName: "STORE_TEXT", caption: "Store ID");
            SetColumn(columnName: "DISPLAY_TEXT", caption: "Merchandise Node");
            SetColumn(columnName: "DateRange", caption: "Date Range");
            SetColumn(columnName: "TOTALPCT", caption: "Total %");
            SetColumn(columnName: "DAY1", caption: "DAY1");
            SetColumn(columnName: "DAY2", caption: "DAY2");
            SetColumn(columnName: "DAY3", caption: "DAY3");
            SetColumn(columnName: "DAY4", caption: "DAY4");
            SetColumn(columnName: "DAY5", caption: "DAY5");
            SetColumn(columnName: "DAY6", caption: "DAY6");
            SetColumn(columnName: "DAY7", caption: "DAY7");
            
            ExpandAllRows();
        }

        private void FormatNodePropertyForecastLevelReport()
        {
            RemoveColumn(columnName: "BN_ID");
            RemoveColumn(columnName: "BN_NAME");

            SetColumn(columnName: "DISPLAY_TEXT", caption: "Merchandise Node");
            SetColumn(columnName: "HIERARCHY", caption: "Hierarchy");
            SetColumn(columnName: "LEVEL", caption: "Level");
            SetColumn(columnName: "MASK", caption: "ID/Name/Desc");
            SetColumn(columnName: "MASK_VALUE", caption: "Starts With");

        }

        private void FormatNodePropertyForecastTypeReport()
        {
            RemoveColumn(columnName: "Column1");
            RemoveColumn(columnName: "HN_RID");

            SetColumn(columnName: "PHL_ID", caption: "Levels");
            SetColumn(columnName: "TEXT_VALUE", caption: "OTS Type");

        }

        private void FormatNodePropertyPurgeCriteriaReport()
        {
            SetNumberColumnHeaderLines(2);

            RemoveColumn(columnName: "SORTSEQ");
            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "BN_ID");
            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "PURGE_DAILY_HISTORY_WEEKS");
            RemoveColumn(columnName: "PURGE_WEEKLY_HISTORY_WEEKS");
            RemoveColumn(columnName: "PURGE_PLANS_WEEKS");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_RECEIPT");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_ASN");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_DUMMY");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_DROPSHIP");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_RESERVE");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_WORKUPTOTALBUY");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_PO");
            RemoveColumn(columnName: "PURGE_HEADERS_WEEKS_VSW");

            SetColumn(columnName: "BN_NAME", caption: "Level");
            SetColumn(columnName: "DAILY_INH", caption: "Daily" + Environment.NewLine + "History");
            SetColumn(columnName: "WEEKLY_INH", caption: "Weekly" + Environment.NewLine + "History");
            SetColumn(columnName: "PLANS_INH", caption: "OTS" + Environment.NewLine + "Forecast");
            SetColumn(columnName: "HEADERS_ASN_INH", caption: " " + Environment.NewLine + "ASN");
            SetColumn(columnName: "HEADERS_DROPSHIP_INH", caption: "Drop" + Environment.NewLine + "Ship");
            SetColumn(columnName: "HEADERS_DUMMY_INH", caption: " " + Environment.NewLine + "Dummy");
            SetColumn(columnName: "HEADERS_RECEIPT_INH", caption: " " + Environment.NewLine + "Receipt");
            SetColumn(columnName: "HEADERS_PO_INH", caption: "Purchase" + Environment.NewLine + "Order");
            SetColumn(columnName: "HEADERS_RESERVE_INH", caption: " " + Environment.NewLine + "Reserve");
            SetColumn(columnName: "HEADERS_VSW_INH", caption: " " + Environment.NewLine + "VSW");
            SetColumn(columnName: "HEADERS_WORKUPTOTALBUY_INH", caption: "Workup" + Environment.NewLine + "Total Buy");
        }

        private void FormatNodePropertySizeCurveCriteriaReport()
        {
            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "HOME_LEVEL");
            RemoveColumn(columnName: "NSCCD_RID");
            RemoveColumn(columnName: "PH_OFFSET_IND");
            RemoveColumn(columnName: "PH_RID");
            RemoveColumn(columnName: "PHL_SEQUENCE");
            RemoveColumn(columnName: "PHL_OFFSET");
            RemoveColumn(columnName: "CDR_RID");
            RemoveColumn(columnName: "APPLY_LOST_SALES_IND");
            RemoveColumn(columnName: "OLL_RID");
            RemoveColumn(columnName: "CUSTOM_OLL_RID");
            RemoveColumn(columnName: "SIZE_GROUP_RID");
            RemoveColumn(columnName: "SG_RID");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "PHL_ID", caption: "Level");
            SetColumn(columnName: "IS_DEFAULT", caption: "Default", hAlign: HAlign.Center);
            SetColumn(columnName: "DISPLAY_DATE", caption: "Display Date");
            SetColumn(columnName: "INCLUDE_EXCLUDE", caption: "Include/Exclude");
            SetColumn(columnName: "OVERRIDE_LOW_LEVEL_NODE", caption: "Override Node");
            SetColumn(columnName: "SIZE_GROUP_NAME", caption: "Size Group");
            SetColumn(columnName: "CURVE_NAME", caption: "Curve Name");
            SetColumn(columnName: "STORE_GROUP_ATTRIBUTE", caption: "Attribute");

            ExpandAllRows();

        }

        private void FormatNodePropertySizeCurveSimilarStoresReport()
        {
            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "HOME_LEVEL");
            RemoveColumn(columnName: "ST_RID");
            RemoveColumn(columnName: "SS_RID");
            RemoveColumn(columnName: "UNTIL_DATE");
            RemoveColumn(columnName: "SELLING_OPEN_DATE");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "STORE_TEXT", caption: "Store");
            SetColumn(columnName: "SIMILAR_STORE_TEXT", caption: "Similar Store");
            SetColumn(columnName: "UNTIL_DATE_TEXT", caption: "Until Date");

            ExpandAllRows();

        }

        private void FormatNodePropertySizeCurveToleranceReport()
        {
            SetNumberColumnHeaderLines(2);

            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "HOME_LEVEL");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "MINIMUM_AVERAGE_PER_SIZE", caption: "Highest Level Sales Tolerance" + Environment.NewLine + "Min Ave per Size", hAlign: HAlign.Right, format: "###,##0.00");
            SetColumn(columnName: "HIGHEST_LEVEL_PHL_ID", caption: "Highest Level Sales Tolerance" + Environment.NewLine + "Highest Level", hAlign: HAlign.Center);
            SetColumn(columnName: "SALES_TOLERANCE", caption: "Apply Chain/Set Sales" + Environment.NewLine + "Sales Tolerance", hAlign: HAlign.Right, format: "###,##0.00");
            SetColumn(columnName: "INDEX_UNITS_TYPE_DESCRIPTION", caption: "Apply Chain/Set Sales" + Environment.NewLine + "Index Unit Type", hAlign: HAlign.Center);
            SetColumn(columnName: "MIN_TOLERANCE", caption: "Tolerance" + Environment.NewLine + "Minimum %", hAlign: HAlign.Right, format: "###,##0.00");
            SetColumn(columnName: "MAX_TOLERANCE", caption: "Tolerance" + Environment.NewLine + "Maximum %", hAlign: HAlign.Right, format: "###,##0.00");
            SetColumn(columnName: "APPLY_MIN_TO_ZERO_TOLERANCE", caption: "Apply Min to Zero Tolerance", hAlign: HAlign.Center);

            ExpandAllRows();

        }

        private void FormatNodePropertyStockMinMaxReport()
        {
            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "Column1");
            RemoveColumn(columnName: "HOME_LEVEL");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "SGL_ID", caption: "Attribute Set");
            SetColumn(columnName: "BOUNDARY", caption: "Grade");
            SetColumn(columnName: "DateRange", caption: "Date Range");
            SetColumn(columnName: "MIN_STOCK", caption: "Min Stock", hAlign: HAlign.Right);
            SetColumn(columnName: "MAX_STOCK", caption: "Max Stock", hAlign: HAlign.Right);

            ExpandAllRows();
        }

        private void FormatNodePropertyStoreCapacityReport()
        {
            GroupByColumn(columnName: "STORE_TEXT");

            SortByColumn(columnName: "HOME_LEVEL");
            SortByColumn(columnName: "DISPLAY_TEXT");

            RemoveColumn(columnName: "Column1");
            RemoveColumn(columnName: "PARENT_HN_RID");
            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "ST_ID");
            RemoveColumn(columnName: "STORE_NAME");
            RemoveColumn(columnName: "BN_ID");
            RemoveColumn(columnName: "BN_NAME");
            RemoveColumn(columnName: "HOME_LEVEL");

            SetColumn(columnName: "STORE_TEXT", caption: "Store");
            SetColumn(columnName: "DISPLAY_TEXT", caption: "Merchandise Node");
            SetColumn(columnName: "ST_CAPACITY", caption: "Max Capacity", hAlign: HAlign.Right, format: "###,###,##0");

            ExpandAllRows();
        }

        private void FormatNodePropertyStoreEligibilityReport()
        {
            GroupByColumn(columnName: "STORE_TEXT");

            SortByColumn(columnName: "HOME_LEVEL");
            SortByColumn(columnName: "PARENT_HN_RID");
            SortByColumn(columnName: "DISPLAY_TEXT");

            RemoveColumn(columnName: "Column1");
            RemoveColumn(columnName: "HOME_LEVEL");
            RemoveColumn(columnName: "HOME_PH_RID");
            RemoveColumn(columnName: "PARENT_HN_RID");
            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "ST_ID");
            RemoveColumn(columnName: "STORE_NAME");
            RemoveColumn(columnName: "BN_ID");
            RemoveColumn(columnName: "BN_NAME");
            RemoveColumn(columnName: "CDR_RANGE_TYPE_ID");
            RemoveColumn(columnName: "CDR_RID");
            RemoveColumn(columnName: "STORE_RID");

            SetColumn(columnName: "STORE_TEXT", caption: "Store");
            SetColumn(columnName: "DISPLAY_TEXT", caption: "Merchandise Node");
            SetColumn(columnName: "EM_ID", caption: "Eligibility");
            SetColumn(columnName: "INELIGIBLE", caption: "Ineligible");
            SetColumn(columnName: "STKMOD_ID", caption: "Stock Modifier");
            SetColumn(columnName: "SLSMOD_ID", caption: "Sales Modifier");
            SetColumn(columnName: "FWOSMOD_ID", caption: "FWOS Override");
            SetColumn(columnName: "SimilarStore", caption: "Similar Store");
            SetColumn(columnName: "SIMILAR_STORE_RATIO", caption: "Index", hAlign: HAlign.Right, format: "##0.00");
            SetColumn(columnName: "PERIOD", caption: "Time Period");

            ExpandAllRows();
        }

        private void FormatNodePropertyStoreGradesReport()
        {
            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "HOME_LEVEL");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "GRADE_CODE", caption: "Grade");
            SetColumn(columnName: "BOUNDARY", caption: "Boundary", hAlign: HAlign.Right);
            SetColumn(columnName: "WOS_INDEX", caption: "WOS Index", hAlign: HAlign.Right);
            SetColumn(columnName: "MINIMUM_STOCK", caption: "Allocation Min", hAlign: HAlign.Right);
            SetColumn(columnName: "MAXIMUM_STOCK", caption: "Allocation Max", hAlign: HAlign.Right);
            SetColumn(columnName: "MINIMUM_AD", caption: "Min Ad", hAlign: HAlign.Right);
            SetColumn(columnName: "MINIMUM_COLOR", caption: "Color Min", hAlign: HAlign.Right);
            SetColumn(columnName: "MAXIMUM_COLOR", caption: "Max Color", hAlign: HAlign.Right);

            ExpandAllRows();
        }

        private void FormatNodePropertyVelocityGradesReport()
        {
            GroupByColumn(columnName: "DISPLAY_TEXT", sortByHomeLevel: true);

            RemoveColumn(columnName: "HOME_LEVEL");

            SetColumn(columnName: "DISPLAY_TEXT", caption: "Merchandise Node");
            SetColumn(columnName: "GRADE_CODE", caption: "Grade");
            SetColumn(columnName: "BOUNDARY", caption: "Boundary", hAlign: HAlign.Right);
            SetColumn(columnName: "MINIMUM_STOCK", caption: "Aloc Min", hAlign: HAlign.Right);
            SetColumn(columnName: "MAXIMUM_STOCK", caption: "Aloc Max", hAlign: HAlign.Right);
            SetColumn(columnName: "MINIMUM_AD", caption: "Min Ad", hAlign: HAlign.Right);
            SetColumn(columnName: "SELL_THRU_PCT", caption: "Sell Thru %", hAlign: HAlign.Right);

            ExpandAllRows();
        }

        private void FormatNodePropertyVSWReport()
        {
            SetNumberColumnHeaderLines(2);

            GroupByColumn(columnName: "BN_ID", sortByHomeLevel: true);

            RemoveColumn(columnName: "HN_RID");
            RemoveColumn(columnName: "HOME_LEVEL");
            RemoveColumn(columnName: "ST_ID");
            RemoveColumn(columnName: "STORE_NAME");
            RemoveColumn(columnName: "SG_RID");
            RemoveColumn(columnName: "SG_ID");
            RemoveColumn(columnName: "SGL_RID");
            RemoveColumn(columnName: "SGL_ID");
            RemoveColumn(columnName: "FWOS_MAX_TYPE");

            SetColumn(columnName: "BN_ID", caption: "Merchandise Node");
            SetColumn(columnName: "STORE_TEXT", caption: "Store");
            SetColumn(columnName: "VSW", caption: "VSW");
            SetColumn(columnName: "MIN_SHIP_QTY", caption: "Min Ship" + Environment.NewLine + "Qty", hAlign: HAlign.Right);
            SetColumn(columnName: "PERCENT_PACK_THRESHOLD", caption: "% Pack" + Environment.NewLine + "Threshold", hAlign: HAlign.Right, format: "##0.00");
            SetColumn(columnName: "ITEM_MAX_VALUE", caption: "Item Max", hAlign: HAlign.Right);
            SetColumn(columnName: "FWOS_MAX_MODEL_NAME_OR_FLOAT", caption: "FWOS Max", hAlign: HAlign.Center);
            SetColumn(columnName: "PUSH_TO_BACKSTOCK", caption: "Push to" + Environment.NewLine + "Backstock", hAlign: HAlign.Center);

            ExpandAllRows();
        }

        private void FormatAllocationByStoreReport()
        {
            GroupByColumn(columnName: "HEADER_TYPE");
            GroupByColumn(columnName: "HEADER_STATUS");

            RemoveColumn(columnName: "HDR_RID");
            RemoveColumn(columnName: "UNITS_SHIPPED");
            RemoveColumn(columnName: "ITEM_UNITS_ALLOCATED");
            RemoveColumn(columnName: "DISPLAY_STATUS");
            RemoveColumn(columnName: "DISPLAY_TYPE");
            RemoveColumn(columnName: "DISPLAY_INTRANSIT");
            RemoveColumn(columnName: "DISPLAY_SHIP_STATUS");

            SetColumn(columnName: "HEADER_TYPE", caption: "Header Type");
            SetColumn(columnName: "HEADER_STATUS", caption: "Header Status");
            SetColumn(columnName: "HDR_ID", caption: "Header ID");
            SetColumn(columnName: "STYLE_ID", caption: "Style/SKU");
            SetColumn(columnName: "PARENT_ID", caption: "Sub_Class");
            SetColumn(columnName: "TOTAL_LABEL", caption: " ");
            SetColumn(columnName: "UNITS_ALLOCATED", caption: "Units Allocated", hAlign: HAlign.Right);

            UltraGridBand band = this.ultraGrid1.DisplayLayout.Bands[0];

            band.Override.AllowRowSummaries = AllowRowSummaries.BasedOnDataType;


            SummarySettings sumSummary = band.Summaries.Add("Sum", SummaryType.Sum, band.Columns["UNITS_ALLOCATED"]);

            // Set the format of the summary text
            sumSummary.DisplayFormat = "Total Units {0:###,###,##0}";

            // Change the appearance settings for summaries.
            sumSummary.Appearance.TextHAlign = HAlign.Right;

            // Set the DisplayInGroupBy property of both summaries to false so they don't
            // show up in group-by rows.
            sumSummary.SummaryDisplayArea = SummaryDisplayAreas.GroupByRowsFooter | SummaryDisplayAreas.Bottom;

            // Set the caption that shows up on the header of the summary footer.

            //band.SummaryFooterCaption = "Total Units for [value]";
            band.Override.SummaryFooterCaptionAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Override.SummaryFooterCaptionAppearance.BackColor = Color.LightGray;
            band.Override.SummaryFooterCaptionAppearance.ForeColor = Color.Black;

            band.Override.SummaryValueAppearance.FontData.Bold = DefaultableBoolean.True;

            ExpandAllRows();
        }

        private void SetNumberColumnHeaderLines(int numberLines)
        {
            UltraGridBand band = this.ultraGrid1.DisplayLayout.Bands[0];
            band.ColHeaderLines = numberLines;
        }

        private void RemoveColumn(string columnName)
        {
            UltraGridBand band = this.ultraGrid1.DisplayLayout.Bands[0];
            if (band.Columns.Exists(columnName))
            {
                band.Columns[columnName].Hidden = true;
                band.Columns[columnName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        private void SortByColumn(string columnName, bool descending = false)
        {
            UltraGridBand band = this.ultraGrid1.DisplayLayout.Bands[0];
            if (band.Columns.Exists(columnName))
            {
                band.SortedColumns.Add(columnName, descending);
            }
        }

        private void GroupByColumn(string columnName, bool sortByHomeLevel = false)
        {
            UltraGridBand band = this.ultraGrid1.DisplayLayout.Bands[0];
            if (band.Columns.Exists(columnName))
            {
                this.ultraGrid1.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
                band.SortedColumns.Add(columnName, false, true);
                if (sortByHomeLevel
                    && band.Columns.Exists("HOME_LEVEL"))
                {
                    band.Columns[columnName].GroupByComparer = new CustomGroupByRowsHomeLevelSorter();
                }
            }
        }

        private int visiblePosition = 0;
        private void SetColumn(string columnName, string caption, Infragistics.Win.HAlign hAlign = Infragistics.Win.HAlign.Default, string format = null)
        {
            UltraGridBand band = this.ultraGrid1.DisplayLayout.Bands[0];
            if (band.Columns.Exists(columnName))
            {
                band.Columns[columnName].Header.Caption = caption;
                band.Columns[columnName].Header.VisiblePosition = visiblePosition;
                band.Columns[columnName].CellAppearance.TextHAlign = hAlign;
                band.Columns[columnName].Format = format;
                ++visiblePosition;
            }
        }

        private void ExpandAllRows()
        {
            this.ultraGrid1.Rows.ExpandAll(true);
        }

        private class CustomGroupByRowsHomeLevelSorter : System.Collections.IComparer
        {
            public int Compare(object xObj, object yObj)
            {
                UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;

                int xHomeLevel = Convert.ToInt32(x.Rows[0].Cells["HOME_LEVEL"].Value);
                int yHomeLevel = Convert.ToInt32(y.Rows[0].Cells["HOME_LEVEL"].Value);

                // Compare the group rows by the number of items they contain.
                //return x.Rows.Count.CompareTo(y.Rows.Count);
                return xHomeLevel.CompareTo(yHomeLevel);
            }
        }


    }
}
