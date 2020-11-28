using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows.Controls
{
    public partial class SearchResults : UserControl
    {
        public SearchResults()
        {
            InitializeComponent();
        }

        private void SearchResultsControl_Load(object sender, EventArgs e)
        {
        }

        private SessionAddressBlock SAB;
        private filterTypes filterType;
        public void LoadData(SessionAddressBlock SAB, filterTypes filterType)
        {
            this.SAB = SAB;
            this.filterType = filterType;
            if (filterDataHelper.SAB == null)
            {
                filterDataHelper.SAB = SAB;
            }
            BindFilterComboBox();


            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["useNewTab"]).Checked = filterUtility.showResultsInNewTab; //load from memory

            if (filterType == filterTypes.AuditFilter)
            {
                this.ultraToolbarsManager1.Tools["auditIncludeSummary"].SharedProps.Visible = true;
                this.ultraToolbarsManager1.Tools["auditIncludeDetails"].SharedProps.Visible = true;
                this.ultraToolbarsManager1.Tools["auditDetailLocation"].SharedProps.Visible = true;

                //load from memory
                ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["auditIncludeSummary"]).Checked = filterUtility.auditFilterIncludeSummary;
                ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["auditIncludeDetails"]).Checked = filterUtility.auditFilterIncludeDetails;
                ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["auditMergeDetails"]).Checked = filterUtility.auditFilterMergeDetails;
            }
            else
            {
                this.ultraToolbarsManager1.Tools["auditIncludeSummary"].SharedProps.Visible = false;
                this.ultraToolbarsManager1.Tools["auditIncludeDetails"].SharedProps.Visible = false;
                this.ultraToolbarsManager1.Tools["auditDetailLocation"].SharedProps.Visible = false;
            }
        }

 

        public int currentFilterRID = Include.NoRID;
        public string currentFilterName = string.Empty;
        public void BindFilterComboBox()
        {
            try
            {
                //Begin TT#1417-MD -jsobek -Product Filter - Error when closing search window
                Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.ultraToolbarsManager1.Tools["ControlContainerFilter"];
                MIDComboBoxEnh.MyComboBox cmbFilter = (MIDComboBoxEnh.MyComboBox)cct.Control;
                if (cmbFilter != null) //the combobox will become null if the user closes the search window, but still is creating/editing a filter
                {
                    ArrayList userRIDList = new ArrayList();
                    userRIDList.Add(Include.GlobalUserRID);
                    userRIDList.Add(SAB.ClientServerSession.UserRID);

                    FilterData fd = new FilterData();
                    DataTable dtFilters = fd.FilterReadForUser(filterType, userRIDList);


                    if (filterType == filterTypes.AuditFilter && dtFilters.Rows.Count == 0)     //create default audit filter
                    {
                        int defaultAuditFilteRID = fd.InsertFilterForAuditDefault(SAB.ClientServerSession.UserRID);
                        fd.InsertConditionForAuditDefault(defaultAuditFilteRID);
                        DataRow drDefaultFilter = dtFilters.NewRow();
                        drDefaultFilter["FILTER_RID"] = defaultAuditFilteRID;
		                drDefaultFilter["USER_RID"] = SAB.ClientServerSession.UserRID;
                        drDefaultFilter["FILTER_NAME"] = "Default Audit Filter";
		                drDefaultFilter["OWNER_USER_RID"] =  SAB.ClientServerSession.UserRID;
		                drDefaultFilter["FILTER_USER_RID"] =  SAB.ClientServerSession.UserRID;
                        dtFilters.Rows.Add(drDefaultFilter);

                    }

                    LoadFiltersOnToolbar(dtFilters);


                    if (currentFilterRID == Include.NoRID)
                    {
                        if (dtFilters.Rows.Count > 0)
                        {
                            //set to the first filter
                            this.currentFilterRID = (int)dtFilters.Rows[0]["FILTER_RID"];
                            this.currentFilterName = (string)dtFilters.Rows[0]["FILTER_NAME"];
                            cmbFilter.SelectedValue = this.currentFilterRID;
                        }
                     
                    }
                    else
                    {
                        cmbFilter.SelectedValue = this.currentFilterRID;
                    }
                }
                //End TT#1417-MD -jsobek -Product Filter - Error when closing search window
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void LoadFiltersOnToolbar(DataTable dtFilters)
        {
            DataView dv = new DataView(dtFilters);
            dv.Sort = "FILTER_NAME";

            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.ultraToolbarsManager1.Tools["ControlContainerFilter"];
            MIDComboBoxEnh.MyComboBox cmbFilter = (MIDComboBoxEnh.MyComboBox)cct.Control;
            if (cmbFilter != null)
            {
                cmbFilter.ValueMember = "FILTER_RID";
                cmbFilter.DisplayMember = "FILTER_NAME";
                cmbFilter.DataSource = dv;
            }
        }
        private void cboFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.ultraToolbarsManager1.Tools["ControlContainerFilter"];
            MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox cmbFilter = (MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)cct.Control;
            int filterRid = int.Parse(cmbFilter.SelectedValue.ToString());
            this.currentFilterRID = filterRid;
            this.currentFilterName = cmbFilter.Text;
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnEditFilter":
                    EditFilter();
                    break;
                case "btnShowResults":
                    ShowResults();
                    break;
                case "btnFilterNew":
                    NewFilter();
                    break;
                case "btnFilterEdit":
                    EditFilter();
                    break;
                case "btnFilterDelete":
                    DeleteFilter();
                    break;

                case "useNewTab":
                    _useNewTab = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["useNewTab"]).Checked;
                    break;

         
               
            }
        }

        private void NewFilter()
        {
            RaiseNewFilterEvent();
        }
        private void EditFilter()
        {
            RaiseEditFilterEvent(this.currentFilterRID, this.currentFilterName);
        }
        private void DeleteFilter()
        {
            string msgText = MIDText.GetText(eMIDTextCode.msg_DeleteSearchWarning);
            string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteSearchWarningCaption);
            if (MessageBox.Show(msgText, msgCaption, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                RaiseDeleteFilterEvent(this.currentFilterRID, this.currentFilterName);
            }
        }



        private int resultCount = 0;
        private bool _useNewTab = false;
        private void ShowResults()
        {
            try
            {
                if (currentFilterRID == Include.NoRID)
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_SelectFilterFirst);
                    MessageBox.Show(msgText);
                    return;
                }


                string sTabPage;
                resultCount++;

                if (_useNewTab == false)
                {
                    int tabKeyIndex = this.ultraTabControl1.Tabs.IndexOf("Results");
                    if (tabKeyIndex != -1)
                    {
                        this.ultraTabControl1.Tabs.Remove(this.ultraTabControl1.Tabs["Results"]);
                    }
                    sTabPage = "Results";
                }
                else
                {
                    sTabPage = "Results" + resultCount.ToString();
                }

                this.ultraTabControl1.Tabs.Add(sTabPage, sTabPage);

                filter f = filterDataHelper.LoadExistingFilter(this.currentFilterRID);
                string sql = string.Empty;


                filterUtility.showResultsInNewTab = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["useNewTab"]).Checked; //save menu choices in memory

                if (filterType == filterTypes.ProductFilter)
                {
                    sql = filterEngineSQLforProducts.MakeSqlForFilter(f);
                }
                else if (filterType == filterTypes.AuditFilter)
                {
                    auditMergeDetails = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["auditMergeDetails"]).Checked;
                    auditIncludeSummary = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["auditIncludeSummary"]).Checked;
                    auditIncludeDetails = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["auditIncludeDetails"]).Checked;

                      //save menu choices in memory
                    filterUtility.auditFilterIncludeSummary = auditIncludeSummary;
                    filterUtility.auditFilterIncludeDetails = auditIncludeDetails;
                    filterUtility.auditFilterMergeDetails = auditMergeDetails;

                    bool mergeDetailsInSQL = false;
                    if (auditIncludeDetails && auditMergeDetails)
                    {
                        mergeDetailsInSQL = true;
                    }
                    sql = filterEngineSQLforAudit.MakeSqlForFilter(f, mergeDetailsInSQL);
                }
     

                SearchResultContainer ui = new SearchResultContainer();
                ui.gridControl1.LocateEvent += new MIDGridControl.LocateEventHandler(Handle_Locate);
                ui.gridControl1.ClearSelectedNodeEvent += new MIDGridControl.ClearSelectedNodeEventHandler(Handle_ClearSelectedNode);
                ui.gridControl1.CopyActionEvent += new MIDGridControl.CopyActionEventHandler(Handle_CopyAction);
                ui.gridControl1.SaveLayoutEvent += new MIDGridControl.SaveLayoutEventHandler(Handle_SaveLayout);
                ui.gridControl1.RemoveLayoutEvent += new MIDGridControl.RemoveLayoutEventHandler(Handle_RemoveLayout);
       
                ui.gridControl1.SAB = SAB;

                if (filterType == filterTypes.ProductFilter)
                {
                    ui.gridControl1.HideColumn("HN_RID");
                    ui.gridControl1.HideColumn("PH_RID");

                    ui.gridControl1.ShowButton("btnLocate");
                    ui.gridControl1.ShowButton("btnCopy");
                }
                else if (filterType == filterTypes.AuditFilter)
                {
                    ui.gridControl1.HideColumn("ProcessRID");
                    ui.gridControl1.HideColumn("ProcessID");
                    if (auditMergeDetails == false)
                    {
                        ui.gridControl1.HideColumn("Time");
                        ui.gridControl1.HideColumn("Module");
                        ui.gridControl1.HideColumn("MessageLevel");
                        ui.gridControl1.HideColumn("Message");
                        ui.gridControl1.HideColumn("Message Details");
                    }

                    ui.AfterExecution = new SearchResultContainer.AfterExecutionDelegate(AfterExecutionForAudit);
                    ui.gridControl1.gridInitializeLayoutCallback = new MIDGridControl.gridInitializeLayoutCallbackDelegate(gridInitializeLayoutForAudit);
                    ui.gridControl1.gridBeforeRowExpandedCallback = new MIDGridControl.gridBeforeRowExpandedCallbackDelegate(gridBeforeRowExpandedForAudit);
                }
   



                ui.Dock = DockStyle.Fill;
                this.ultraTabControl1.Tabs[sTabPage].TabPage.Controls.Add(ui);

                this.ultraTabControl1.Tabs[sTabPage].Selected = true;

                ui.gridControl1.filterType = filterType;
                
                if (filterType == filterTypes.AuditFilter) //handle special case where there are multiple grid and menu layouts for the same filter
                {
                    if (auditMergeDetails)
                    {
                        if (customLayoutStream2 == null)  //read the grid layout just once from the database, then maintain it here when changed
                        {
                            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                            InfragisticsLayout gridLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.auditFilterMergedSearchResultsGrid);
                            customLayoutStream2 = gridLayout.LayoutStream;
                            InfragisticsLayout menuLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.auditFilterMergedSearchResultsMenu);
                            customLayoutMenuStream2 = menuLayout.LayoutStream;
                        }
                        ui.gridControl1.layoutID = eLayoutID.auditFilterMergedSearchResultsGrid;
                        ui.gridControl1.layoutMenuID = eLayoutID.auditFilterMergedSearchResultsMenu;
                        ui.gridControl1.customLayoutStream = customLayoutStream2;
                        ui.gridControl1.customLayoutMenuStream = customLayoutMenuStream2;                    
                    }
                    else
                    {
                        if (customLayoutStream1 == null)  //read the grid layout just once from the database, then maintain it here when changed
                        {
                            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                            InfragisticsLayout gridLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.auditFilterNestedSearchResultsGrid);
                            customLayoutStream1 = gridLayout.LayoutStream;
                            InfragisticsLayout menuLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.auditFilterNestedSearchResultsMenu);
                            customLayoutMenuStream1 = menuLayout.LayoutStream;
                        }
                        ui.gridControl1.layoutID = eLayoutID.auditFilterNestedSearchResultsGrid;
                        ui.gridControl1.layoutMenuID = eLayoutID.auditFilterNestedSearchResultsMenu;
                        ui.gridControl1.customLayoutStream = customLayoutStream1;
                        ui.gridControl1.customLayoutMenuStream = customLayoutMenuStream1;  
                  
                    }
                }
                else
                {
                    if (filterType.layoutID != Include.NoRID)
                    {
                        if (customLayoutStream1 == null)  //read the grid layout just once from the database, then maintain it here when changed
                        {
                            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                            InfragisticsLayout gridLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, (eLayoutID)filterType.layoutID);
                            customLayoutStream1 = gridLayout.LayoutStream;
                            //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                            InfragisticsLayout menuLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, (eLayoutID)filterType.layoutMenuID);
                            customLayoutMenuStream1 = menuLayout.LayoutStream;
                            //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                        }
                        ui.gridControl1.layoutID = (eLayoutID)filterType.layoutID;
                        ui.gridControl1.layoutMenuID = (eLayoutID)filterType.layoutMenuID;
                        ui.gridControl1.customLayoutStream = customLayoutStream1;
                        ui.gridControl1.customLayoutMenuStream = customLayoutMenuStream1;  
                    }
                }
              
            

                ui.Start(sql, SAB);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private bool auditMergeDetails;
        private bool auditIncludeSummary;
        private bool auditIncludeDetails;
        private void AfterExecutionForAudit(ref DataSet ds)
        {
            ds.Tables[0].TableName = "Headers";
            if (auditIncludeSummary && (auditIncludeDetails== false || auditMergeDetails == false))
            {
                DataTable auditSummaryRow = ds.Tables.Add("SummaryRow");
                auditSummaryRow.Locale = ds.Tables[0].Locale;
                auditSummaryRow.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                DataColumn dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "NeedsLoaded";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessID";
                dataColumn.Caption = "ProcessID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Text";
                dataColumn.Caption = "Text";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                // add summary counts for load processes
                DataTable auditSummary = ds.Tables.Add("Summary");
                auditSummary.Locale = ds.Tables[0].Locale;
                auditSummary.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummary.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Item";
                dataColumn.Caption = "Item";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummary.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Value";
                dataColumn.Caption = "Value";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummary.Columns.Add(dataColumn);

                 ds.Relations.Add("SummaryRow", ds.Tables["Headers"].Columns["ProcessRID"], ds.Tables["SummaryRow"].Columns["ProcessRID"]);

                 ds.Relations.Add("Summary", ds.Tables["SummaryRow"].Columns["ProcessRID"], ds.Tables["Summary"].Columns["ProcessRID"]);

                 foreach (DataRow dr in ds.Tables[0].Rows)
                 {
                    ds.Tables["SummaryRow"].Rows.Add(new object[] { true, Convert.ToInt32(dr["ProcessRID"]), Convert.ToInt32(dr["ProcessID"],CultureInfo.CurrentUICulture), "Summary"});
                 }
            }

            if (auditIncludeDetails && auditMergeDetails == false)
            {
                // add detail header line
                DataTable auditDetailRow = ds.Tables.Add("DetailRow");
                auditDetailRow.Locale = ds.Tables[0].Locale;
                auditDetailRow.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                DataColumn dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "NeedsLoaded";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessID";
                dataColumn.Caption = "ProcessID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Text";
                dataColumn.Caption = "Text";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                // add detail audit messages
                DataTable auditDetails = ds.Tables.Add("Details");
                auditDetails.Locale = ds.Tables[0].Locale;
                auditDetails.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Time";
                dataColumn.Caption = "Time";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Module";
                dataColumn.Caption = "Module";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "MessageLevel";
                dataColumn.Caption = "MessageLevel";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "MessageLevelText";
                dataColumn.Caption = "Message Level";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "MessageCode";
                dataColumn.Caption = "MessageCode";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Message";
                dataColumn.Caption = "Message";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Message2";
                dataColumn.Caption = "Message Details";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                ds.Relations.Add("DetailRow", ds.Tables["Headers"].Columns["ProcessRID"], ds.Tables["DetailRow"].Columns["ProcessRID"]);
                ds.Relations.Add("Details", ds.Tables["DetailRow"].Columns["ProcessRID"], ds.Tables["Details"].Columns["ProcessRID"]);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ds.Tables["DetailRow"].Rows.Add(new object[] { true, Convert.ToInt32(dr["ProcessRID"], CultureInfo.CurrentUICulture), Convert.ToInt32(dr["ProcessID"], CultureInfo.CurrentUICulture), "Details" });
                }
           
            }
        }

        private void gridInitializeLayoutForAudit(ref Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands["Headers"].Columns["Start Time"].Format = "MM/dd/yyyy hh:mm:ss tt";
            e.Layout.Bands["Headers"].Columns["Stop Time"].Format = "MM/dd/yyyy hh:mm:ss tt";
            e.Layout.Bands["Headers"].Columns["Time"].Format = "MM/dd/yyyy hh:mm:ss tt";
            e.Layout.Override.RowSizingAutoMaxLines = 50;
            e.Layout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFree;
            if ((auditIncludeSummary && (auditIncludeDetails == false || auditMergeDetails == false)) || (auditIncludeDetails && auditMergeDetails == false))
            {
                //e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                e.Layout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.MultiBand;
                e.Layout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                e.Layout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
                e.Layout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
                e.Layout.InterBandSpacing = 10;
                e.Layout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.Both;

                //this.ugAudit.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
                e.Layout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
                e.Layout.AddNewBox.Hidden = true;
                e.Layout.AddNewBox.Prompt = "";
                e.Layout.Bands[0].AddButtonCaption = "";
                //this.ugAudit.DisplayLayout.GroupByBox.Hidden = false;
                //this.ugAudit.DisplayLayout.GroupByBox.Prompt = "Drag here to group by column";
                //this.ugAudit.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                //this.ugAudit.DisplayLayout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.Both;

       
            }
            if (auditIncludeSummary && (auditIncludeDetails == false || auditMergeDetails == false))
            {
                e.Layout.Bands["SummaryRow"].ColHeadersVisible = false;
                e.Layout.Bands["SummaryRow"].Columns["NeedsLoaded"].Hidden = true;
                e.Layout.Bands["SummaryRow"].Columns["ProcessRID"].Hidden = true;
                e.Layout.Bands["SummaryRow"].Columns["ProcessID"].Hidden = true;
                e.Layout.Bands["SummaryRow"].Columns["NeedsLoaded"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["SummaryRow"].Columns["ProcessRID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["SummaryRow"].Columns["ProcessID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["SummaryRow"].Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                e.Layout.Bands["SummaryRow"].Columns["Text"].Header.Caption = "Summary"; 

                e.Layout.Bands["Summary"].Columns["ProcessRID"].Hidden = true;
                e.Layout.Bands["Summary"].Columns["ProcessRID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["Summary"].Columns["Item"].Header.Caption = "Item";
                e.Layout.Bands["Summary"].Columns["Value"].Header.Caption = "Value";
            }
            if (auditIncludeDetails && auditMergeDetails == false)
            {
                e.Layout.Bands["DetailRow"].ColHeadersVisible = false;
                e.Layout.Bands["DetailRow"].Columns["NeedsLoaded"].Hidden = true;
                e.Layout.Bands["DetailRow"].Columns["ProcessRID"].Hidden = true;
                e.Layout.Bands["DetailRow"].Columns["ProcessID"].Hidden = true;
                e.Layout.Bands["DetailRow"].Columns["NeedsLoaded"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["DetailRow"].Columns["ProcessRID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["DetailRow"].Columns["ProcessID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;

                e.Layout.Bands["Details"].Columns["Time"].Format = "MM/dd/yyyy hh:mm:ss tt";
                e.Layout.Bands["Details"].Columns["ProcessRID"].Hidden = true;
                e.Layout.Bands["Details"].Columns["MessageLevel"].Hidden = true;
                e.Layout.Bands["Details"].Columns["ProcessRID"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                e.Layout.Bands["Details"].Columns["MessageLevel"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;


                e.Layout.Bands["Details"].Columns["Message"].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Bands["Details"].Columns["Message"].MaxWidth = 800;
                e.Layout.Bands["Details"].Columns["Message2"].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Bands["Details"].Columns["Message2"].MaxWidth = 800;
                e.Layout.Bands["Details"].Columns["MessageCode"].Hidden = true;
                e.Layout.Bands["Details"].Columns["MessageCode"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            }
            else if (auditIncludeDetails && auditMergeDetails == true)
            {
                e.Layout.Bands["Headers"].Columns["Message"].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Bands["Headers"].Columns["Message"].MaxWidth = 800;
                e.Layout.Bands["Headers"].Columns["Message Details"].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Bands["Headers"].Columns["Message Details"].MaxWidth = 800;
            }
        }
        private void gridBeforeRowExpandedForAudit(ref Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e, DataSet ds)
        {
            try
            {
                // Begin TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
                this.Cursor = Cursors.WaitCursor;
                // End TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
                if (e.Row.Band.Key == "SummaryRow")
                {
                    if (e.Row.IsFilterRow)
                    {
                        e.Row.Hidden = true;
                    }
                    if ((bool)e.Row.Cells["NeedsLoaded"].Value)
                    {
                        eProcesses processType = (eProcesses)e.Row.Cells["ProcessID"].Value;
                        switch (processType)
                        {
                            case eProcesses.hierarchyLoad:
                                AuditUtility.AddHierarchyLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            case eProcesses.storeLoad:
                                AuditUtility.AddStoreLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            case eProcesses.historyPlanLoad:
                                AuditUtility.AddHistoryPlanLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            case eProcesses.HeaderReconcile:
                                AuditUtility.AddHeaderReconcileSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            case eProcesses.headerLoad:
                                AuditUtility.AddHeaderLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //Begin MOD - JScott - Build Pack Criteria Load
                            case eProcesses.buildPackCriteriaLoad:
                                AuditUtility.AddBuildPackCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End MOD - JScott - Build Pack Criteria Load
                            //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                            case eProcesses.ChainSetPercentCriteriaLoad:
                                AuditUtility.AddChainSetPercentCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                            //Begin TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                            case eProcesses.StoreEligibilityCriteraLoad:
                                AuditUtility.AddStoreEligibilityCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                            //Begin TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
                            case eProcesses.VSWCriteriaLoad:
                                AuditUtility.AddVSWCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
                            //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                            case eProcesses.DailyPercentagesCriteraLoad:
                                AuditUtility.AddDailyPercentagesCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#43 - MD - DOConnell - Projected Sales Enhancement

                            case eProcesses.purge:
                                AuditUtility.AddPurgeSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            case eProcesses.colorCodeLoad:
                                AuditUtility.AddColorCodeLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            case eProcesses.sizeCodeLoad:
                                AuditUtility.AddSizeCodeLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                            case eProcesses.sizeCurveLoad:
                                AuditUtility.AddSizeCurveLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

                            //Begin TT#707 - JScott - Size Curve process needs to multi-thread
                            case eProcesses.sizeCurveGenerate:
                                AuditUtility.AddSizeCurveGenerateSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#707 - JScott - Size Curve process needs to multi-thread
                            case eProcesses.rollup:
                                AuditUtility.AddRollupSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
                            case eProcesses.computationDriver:
                                AuditUtility.AddComputationDriverSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End - Abercrombie & Fitch #4411
                            case eProcesses.sizeConstraintsLoad:
                                AuditUtility.AddSizeConstraintsSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //Begin Track #5100 - JSmith - Add counts to audit
                            case eProcesses.relieveIntransit:
                                AuditUtility.AddRelieveIntransitSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            // End Track #5100
                            //BEGIN issue 5117 - stodd - special request
                            case eProcesses.specialRequest:
                                AuditUtility.AddSpecialRequestSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //END issue 5117
                            //BEGIN TT#465 - stodd - Size Day to Week Summary
                            case eProcesses.SizeDayToWeekSummary:
                                AuditUtility.AddSizeDayToWeekSummarySummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#465 - stodd - Size Day to Week Summary
                            // Begin TT#710 - JSmith - Generate relieve intransit
                            case eProcesses.generateRelieveIntransit:
                                AuditUtility.AddGenerateRelieveIntransitSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#710
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            case eProcesses.determineHierarchyActivity:
                                AuditUtility.AddDetermineHierarchyActivitySummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End TT#988
                            // BEGIN TT#1401 - stodd - add resevation stores (IMO)							
                            case eProcesses.pushToBackStock:
                                AuditUtility.AddPushToBackStock(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
                            case eProcesses.hierarchyReclass:
                                AuditUtility.AddHierarchyReclassSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                                break;
                            //End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  

                        }
                        e.Row.Cells["NeedsLoaded"].Value = false;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand childband in e.Row.ChildBands)
                        {
                            childband.Band.PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                        }
                      
                    }
                }
                else
                    if (e.Row.Band.Key == "DetailRow")
                    {
                        if (e.Row.IsFilterRow)
                        {
                            e.Row.Hidden = true;
                        }
                        if ((bool)e.Row.Cells["NeedsLoaded"].Value)
                        {
                            AuditUtility.AddDetail(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture), ds);
                            e.Row.Cells["NeedsLoaded"].Value = false;
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand childband in e.Row.ChildBands)
                            {
                                childband.Band.PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                                //foreach (Infragistics.Win.UltraWinGrid.UltraGridRow childRow in childband.Rows)
                                //{
                                //    childRow.PerformAutoSize();
                                //}
                               
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            // Begin TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
            finally
            {
                this.Cursor = Cursors.Default;
            }
            // End TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
        }

        private System.IO.MemoryStream customLayoutStream1 = null;
        private System.IO.MemoryStream customLayoutMenuStream1 = null;
        private System.IO.MemoryStream customLayoutStream2 = null;
        private System.IO.MemoryStream customLayoutMenuStream2 = null;

        private void Handle_Locate(object sender, MIDGridControl.LocateEventArgs e)
        {
            try
            {
                RaiseLocateEvent(e.drSelected);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Handle_ClearSelectedNode(object sender, MIDGridControl.ClearSelectedNodeEventArgs e)
        {
            try
            {
                RaiseClearSelectedNodeEvent();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }


        private void Handle_CopyAction(object sender, MIDGridControl.CopyActionEventArgs e)
        {
            try
            {
                RaiseCopyActionEvent();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void Handle_SaveLayout(object sender, MIDGridControl.SaveLayoutEventArgs e)
        {
            try
            {
                if (filterType == filterTypes.AuditFilter && e.layoutID == eLayoutID.auditFilterMergedSearchResultsGrid)
                {
                    this.customLayoutStream2 = e.customLayout;
                    this.customLayoutMenuStream2 = e.customMenuLayout;
                }
                else
                {
                    this.customLayoutStream1 = e.customLayout;
                    this.customLayoutMenuStream1 = e.customMenuLayout; //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Handle_RemoveLayout(object sender, MIDGridControl.RemoveLayoutEventArgs e)
        {
            try
            {
                if (filterType == filterTypes.AuditFilter && e.layoutID == eLayoutID.auditFilterMergedSearchResultsGrid)
                {
                    this.customLayoutStream2 = null;
                    this.customLayoutMenuStream2 = null;
                }
                else
                {
                    this.customLayoutStream1 = null;
                    this.customLayoutMenuStream1 = null; //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
     

        private void ultraTabControl1_TabClosing(object sender, Infragistics.Win.UltraWinTabControl.TabClosingEventArgs e)
        {
            SearchResultContainer ui = (SearchResultContainer)e.Tab.TabPage.Controls[0];
            ui.Abort();
        }



        public event NewFilterEventHandler NewFilterEvent;
        public void RaiseNewFilterEvent()
        {
            if (NewFilterEvent != null)
                NewFilterEvent(new object(), new NewFilterEventArgs());
        }
        public class NewFilterEventArgs
        {
            public NewFilterEventArgs() { }
        }
        public delegate void NewFilterEventHandler(object sender, NewFilterEventArgs e);



        public event EditFilterEventHandler EditFilterEvent;
        public void RaiseEditFilterEvent(int filterRID, string filterName)
        {
            if (EditFilterEvent != null)
                EditFilterEvent(new object(), new EditFilterEventArgs(filterRID, filterName));
        }
        public class EditFilterEventArgs
        {
            public EditFilterEventArgs(int filterRID, string filterName) { this.filterRID = filterRID; this.filterName = filterName; }
            public int filterRID { get; private set; }
            public string filterName { get; private set; }
        }
        public delegate void EditFilterEventHandler(object sender, EditFilterEventArgs e);



        public event DeleteFilterEventHandler DeleteFilterEvent;
        public void RaiseDeleteFilterEvent(int filterRID, string filterName)
        {
            if (DeleteFilterEvent != null)
                DeleteFilterEvent(new object(), new DeleteFilterEventArgs(filterRID, filterName));
        }
        public class DeleteFilterEventArgs
        {
            public DeleteFilterEventArgs(int filterRID, string filterName) { this.filterRID = filterRID; this.filterName = filterName; }
            public int filterRID { get; private set; }
            public string filterName { get; private set; }
        }
        public delegate void DeleteFilterEventHandler(object sender, DeleteFilterEventArgs e);



        public delegate void LocateEventHandler(object sender, LocateEventArgs e);
        public event LocateEventHandler LocateEvent;
        public void RaiseLocateEvent(DataRow drSelected)
        {
            if (LocateEvent != null)
                LocateEvent(new object(), new LocateEventArgs(drSelected));
        }
        public class LocateEventArgs
        {
            public LocateEventArgs(DataRow drSelected) { this.drSelected = drSelected; }
            public DataRow drSelected { get; private set; }
        }

        //
        public delegate void ClearSelectedNodeEventHandler(object sender, ClearSelectedNodeEventArgs e);
        public event ClearSelectedNodeEventHandler ClearSelectedNodeEvent;
        public void RaiseClearSelectedNodeEvent()
        {
            if (ClearSelectedNodeEvent != null)
                ClearSelectedNodeEvent(new object(), new ClearSelectedNodeEventArgs());
        }
        public class ClearSelectedNodeEventArgs
        {
            public ClearSelectedNodeEventArgs() { }
        }



        public delegate void CopyActionEventHandler(object sender, CopyActionEventArgs e);
        public event CopyActionEventHandler CopyActionEvent;
        public void RaiseCopyActionEvent()
        {
            if (CopyActionEvent != null)
                CopyActionEvent(new object(), new CopyActionEventArgs());
        }
        public class CopyActionEventArgs
        {
            public CopyActionEventArgs() { }

        }
    }
}
