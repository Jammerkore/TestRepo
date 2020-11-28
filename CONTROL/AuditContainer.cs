//using System;
//using System.Data;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Globalization;
//using System.IO;
//using System.Windows.Forms;
//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;

//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.Business;
//using MIDRetail.Windows;

//namespace MIDRetail.Windows.Controls
//{
//    public partial class AuditContainer : UserControl
//    {
//        private System.Data.DataSet _auditDataSet;
//        private AuditData _auditData;
//        private bool _displayDeletePrompt = false;
//        private AuditFilterProfile _afp;
//        SessionAddressBlock SAB;
//        private FunctionSecurityProfile _functionSecurity;
//        private bool _formLoaded = false;



//        public Include.AuditFilterFormDelegate containerddfunctionDelegate;
//        //public SharedControlRoutines.ExportAllRowsToExcelDelegate exportAllDelegate;
//        //public SharedControlRoutines.ExportSelectedRowsToExcelDelegate exportSelectedDelegate;
//        //public SharedControlRoutines.EmailAllRowsDelegate emailAllDelegate;
//        //public SharedControlRoutines.EmailSelectedRowsDelegate emailSelectedDelegate;

//        #region Properties
//        public FunctionSecurityProfile FunctionSecurity
//        {
//            get { return _functionSecurity; }
//            set { _functionSecurity = value; }
//        }

//        public bool FormLoaded
//        {
//            get { return _formLoaded; }
//            set { _formLoaded = value; }
//        }

//        private string _messageValidationClearWarningPrompt = "This will permanently remove all messages.\r\n Are you sure you wish to continue??";
//        public string messageValidationClearWarningPrompt
//        {
//            get { return _messageValidationClearWarningPrompt; }
//            set { _messageValidationClearWarningPrompt = value; }
//        }
//        private string _messageValidationClearWarningPromptTitle = "Confirm:";
//        public string messageValidationClearWarningPromptTitle
//        {
//            get { return _messageValidationClearWarningPromptTitle; }
//            set { _messageValidationClearWarningPromptTitle = value; }
//        }

//        private string _messageValidationMaxMessageLimitInvalidNumber = "Please enter a valid number for the max message limit.";
//        public string messageValidationMaxMessageLimitInvalidNumber
//        {
//            get { return _messageValidationMaxMessageLimitInvalidNumber; }
//            set { _messageValidationMaxMessageLimitInvalidNumber = value; }
//        }
//        private string _messageValidationMaxMessageLimitLowestValue = "Message limit cannot be less than 50.";
//        public string messageValidationMaxMessageLimitLowestValue
//        {
//            get { return _messageValidationMaxMessageLimitLowestValue; }
//            set { _messageValidationMaxMessageLimitLowestValue = value; }
//        }

//        private int _messageUpperLimit = 100000;
//        public int messageUpperLimit
//        {
//            get { return _messageUpperLimit; }
//            set
//            {
//                _messageUpperLimit = value;

//                //ensure the user is not over the limit
//                Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Tools["messageTextMaxLimit"];
//                int currentLimit;
//                if (int.TryParse(t.Text, out currentLimit) == true)
//                {
//                    if (currentLimit > _messageUpperLimit)
//                    {
//                        t.Text = _messageUpperLimit.ToString();
//                    }
//                }

//            }
//        }

//        private string _messageValidationMaxMessageLimitHighestValue = "Message limit cannot be greater than {0}.";
//        public string messageValidationMaxMessageLimitHighestValue
//        {
//            get { return _messageValidationMaxMessageLimitHighestValue; }
//            set { _messageValidationMaxMessageLimitHighestValue = value; }
//        }
//        private string _messageValidationMaxMessageLimitRestoringOriginalMessage = "Restoring original value.";
//        public string messageValidationMaxMessageLimitRestoringOriginalMessage
//        {
//            get { return _messageValidationMaxMessageLimitRestoringOriginalMessage; }
//            set { _messageValidationMaxMessageLimitRestoringOriginalMessage = value; }
//        }
//        private string _messageValidationMaxMessageLimitInvalidNumberTitle = "Invalid Max Message Limit";
//        public string messageValidationMaxMessageLimitInvalidNumberTitle
//        {
//            get { return _messageValidationMaxMessageLimitInvalidNumberTitle; }
//            set { _messageValidationMaxMessageLimitInvalidNumberTitle = value; }
//        }

//        #endregion

//        public AuditContainer(SessionAddressBlock aSAB)
//        {
//            InitializeComponent();
//            _auditData = new AuditData();
//            SAB = aSAB;
//            _afp = new AuditFilterProfile(SAB.ClientServerSession.UserRID);
//        }
//        private const string myAuditVersion = "01.001.0001";

//        private void AuditContainer_Load(object sender, EventArgs e)
//        {
//            this.ultraToolbarsManager1.ToolbarSettings.Tag = myAuditVersion;
//            this.Tag = myAuditVersion;

//            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarManager_click);
//            this.ultraToolbarsManager1.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.ultraToolbarManager1_ToolValueChanged);
//            this.ultraToolbarsManager1.BeforeToolExitEditMode += new Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventHandler(this.ultraToolbarsManager1_BeforeToolExitEditMode);
//            this.ultraToolbarsManager1.ToolKeyPress += new Infragistics.Win.UltraWinToolbars.ToolKeyPressEventHandler(this.ultraToolbarsManager1_ToolKeyPress);

//            this.ultraChart1.Visible = false;

//            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
//            rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
//            rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
//            this.ugAudit.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;

//            this.SaveSettingsEvent += new Windows.Controls.AuditContainer.SaveSettingsEventHandler(doSaveSettings);
//            this.ResetSettingsEvent += new Windows.Controls.AuditContainer.ResetSettingsEventHandler(doResetSettings);

            
//            MessageSettingsSetResetPoint();
//            InitializeChart();

//            // check for saved grid layout
//            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();

//            // check for saved toolbar manager layout
//            InfragisticsLayout toolbarManagerLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.auditViewerToolbars);
//            if (toolbarManagerLayout.LayoutLength > 0)
//            {
//                string currentVersion = (string)this.myAuditToolbarManager.ToolbarSettings.Tag;

//                //This would be faster if we could save the version in the database so we would not have to load the layout to get the version
//                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tempToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager();
//                tempToolbarManager.LoadFromBinary(toolbarManagerLayout.LayoutStream);
//                string savedVersion = (string)tempToolbarManager.ToolbarSettings.Tag;

//                if (savedVersion == currentVersion)
//                {
//                    toolbarManagerLayout.LayoutStream.Position = 0;
//                    this.myAuditToolbarManager.LoadFromBinary(toolbarManagerLayout.LayoutStream);
//                    this.LoadChartSettings();


//                    InfragisticsLayout gridLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.userActivityGrid);
//                    if (gridLayout.LayoutLength > 0)
//                    {
//                        //string currentVersion = (string)this.ucUserActivityControl.myActivityGrid.Tag;

//                        ////This would be faster if we could save the version in the database so we would not have to load the layout to get the version
//                        //Infragistics.Win.UltraWinGrid.UltraGrid tempGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
//                        //tempGrid.DisplayLayout.Load(gridLayout.LayoutStream);
//                        //string savedVersion = (string)tempGrid.Tag;

//                        //if (savedVersion == currentVersion)
//                        //{

//                        //using the toolbar to determine whether or not the load the grid 
//                        this.myAuditGrid.DisplayLayout.Load(gridLayout.LayoutStream);
//                        this.LoadGridDataSettings();

//                        //}

//                    }
//                }
//            }
//            //Populate_Audit();
//        }

//        private void BeforeClosing()
//        {
//            try
//            {
//                // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
//                //if (!ugAudit.IsDisposed)
//                if (FormLoaded &&
//                    !ugAudit.IsDisposed)
//                // End TT#2012
//                {
//                    InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
//                    layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, eLayoutID.auditViewerGrid, ugAudit);
//                }

//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void AfterClosing()
//        {
//            try
//            {
//                // define empty dataset to attach to grid to release previous memory
//                Audit_Define();
//                ugAudit.DataSource = null;
//                ugAudit.DataSource = _auditDataSet;
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        #region Toolbar functions

//        private void ultraToolbarManager_click(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
//        {
//            string date = Convert.ToString(System.DateTime.Now);
//            switch (e.Tool.Key)
//            {
//                case "btn_Refresh":
//                    btn_Refresh_Click(sender, e);
//                    break;
//                case "btn_Delete":
//                    btnDelete_Click(sender, e);
//                    break;
//                case "btn_Filter":
//                    btnFilter_Click(sender, e);
//                    break;
//                case "btn_Cancel":
//                    break;

//                #region "Grid Tools"

//                case "gridSearchFindButton":
//                    MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(ugAudit, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
//                    break;
//                case "gridSearchClearButton":
//                    Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
//                    t.Text = "";
//                    MIDRetail.Windows.Controls.SharedControlRoutines.ClearGridSearchResults(ugAudit);
//                    break;

//                case "gridShowSearchToolbar":
//                    this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible = !this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible;
//                    break;

//                case "gridShowGroupArea":
//                    this.ugAudit.DisplayLayout.GroupByBox.Hidden = !this.ugAudit.DisplayLayout.GroupByBox.Hidden;
//                    break;

//                case "gridShowFilterRow":
//                    if (this.ugAudit.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
//                    {
//                        this.ugAudit.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
//                    }
//                    else
//                    {
//                        this.ugAudit.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
//                    }
//                    break;

//                case "gridExportSelected":
//                    SharedControlRoutines.exportHelper.ExportSelectedRowsToExcel(this.ugAudit, string.Empty, string.Empty, string.Empty, true);
//                    //SharedRoutines.GridExport.ExportSelectedRowsToExcel(this.ugAudit);
//                    break;

//                case "gridExportAll":
//                    SharedControlRoutines.exportHelper.ExportAllRowsToExcel(this.ugAudit);
//                    //SharedRoutines.GridExport.ExportAllRowsToExcel(this.ugAudit);
//                    break;

//                case "gridEmailSelectedRows":
//                    SharedControlRoutines.exportHelper.EmailSelectedRows("Audit Viewer", "Audit Viewer " + date + ".xls", this.ugAudit, string.Empty, string.Empty, string.Empty, true); //TT#1280-MD -jsobek -Audit Grid does not export correctly
//                    //SharedRoutines.GridExport.EmailSelectedRows(SharedRoutines.GridExport.BuildEmailSubjectWithUserName(SAB, "My Activity"), "MyActivity.xls", this.ugAudit);
//                    break;

//                case "gridEmailAllRows":
//                    SharedControlRoutines.exportHelper.EmailAllRows("Audit Viewer", "Audit Viewer " + date + ".xls", this.ugAudit);
//                    //SharedRoutines.GridExport.EmailAllRows(SharedRoutines.GridExport.BuildEmailSubjectWithUserName(SAB, "My Activity"), "MyActivity.xls", this.ugAudit);
//                    break;

//                case "gridChooseColumns":
//                    this.ugAudit.ShowColumnChooser("Choose Columns");
//                    break;

//                #endregion

//                #region "Chart Tools"
//                case "chartShowHide":
//                    SetChartVisibility();
//                    break;

//                case "chartDockLeft":
//                    SetChartDocking();
//                    break;

//                case "chartDockBottom":
//                    SetChartDocking();
//                    break;

//                case "chartDockRight":
//                    SetChartDocking();
//                    break;
//                case "chartLegendTop":
//                    SetChartLegend();
//                    break;

//                case "chartLegendLeft":
//                    SetChartLegend();
//                    break;

//                case "chartLegendRight":
//                    SetChartLegend();
//                    break;

//                case "chartLegendBottom":
//                    SetChartLegend();
//                    break;

//                case "chartTypeBar":
//                    SetChartType();
//                    break;
//                case "chartTypeLine":
//                    SetChartType();
//                    break;
//                case "chartTypePie":
//                    SetChartType();
//                    break;
//                case "chartTypePyramid":
//                    SetChartType();
//                    break;
//                case "chartTypeHistogram":
//                    SetChartType();
//                    break;
//                case "chartShowLegend":
//                    SetChartLegend();
//                    break;
//                case "chartTitleShowHide":
//                    SetChartTitle();
//                    break;
//                case "chartTitleLocationTop":
//                    SetChartTitle();
//                    break;
//                case "chartTitleLocationLeft":
//                    SetChartTitle();
//                    break;
//                case "chartTitleLocationRight":
//                    SetChartTitle();
//                    break;
//                case "chartTitleLocationBottom":
//                    SetChartTitle();
//                    break;
//                case "chartExport":
//                    ExportChart();
//                    break;
//                //case "chartShowRowLabels":
//                //    this.ultraChart1.Data.UseRowLabelsColumn = !this.ultraChart1.Data.UseRowLabelsColumn;
//                //    break;

//                #endregion

//                #region "Message Tools"

//                case "messageClear":
//                    MessagesClear();
//                    break;
//                case "messageSettingsSave":
//                    MessageSettingsSave();
//                    break;
//                case "messageSettingsReset":
//                    MessageSettingsReset();
//                    break;

//                #endregion
//            }
//        }

//        private void ultraToolbarsManager1_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
//        {
//            switch (e.Tool.Key)
//            {
//                case "gridSearchText":
//                    if (e.KeyChar == (char)Keys.Return)
//                    {
//                        MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(ugAudit, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
//                    }
//                    break;
//                case "messageLevelComboBox":
//                    if (e.KeyChar == (char)Keys.Return)
//                    {
//                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
//                    }
//                    break;
//            }
//        }

//        private void ultraToolbarManager1_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
//        {
//            switch (e.Tool.Key)
//            {
//                case "messageLevelComboBox":
//                    SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text);
//                    break;
//            }
//        }

//        private void ultraToolbarsManager1_BeforeToolExitEditMode(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventArgs e)
//        {
//            switch (e.Tool.Key)
//            {
//                case "messageTextMaxLimit":
//                    SetMaxMessageLimit(((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text, e);
//                    break;
//            }
//        }

//        private void btnDelete_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                //BEGIN TT#435-MD-DOConnell-Add new features to Audit
//                //Delete button had been removed from screen
//                //if (ugAudit.Selected.Rows.Count > 0)
//                //{
//                //    string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
//                //    text = text.Replace("{0}", "the selected Audit Rows");
//                //    if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                //        == DialogResult.Yes)
//                //    {
//                //        this.Cursor = Cursors.WaitCursor;
//                //        ugAudit.BeginUpdate();
//                //        ugAudit.SuspendRowSynchronization();
//                //        int selected = ugAudit.Selected.Rows.Count;
//                //        //						foreach (UltraGridRow row in ugAudit.Selected.Rows)
//                //        for (int i = 0; i < selected; i++)
//                //        {
//                //            // always use 0 because the row.delete removes the delete row and shifts row 1 to 0
//                //            UltraGridRow row = ugAudit.Selected.Rows[0];
//                //            if (row.Band.Key == "Headers")
//                //            {
//                //                _auditData.Audit_Delete(Convert.ToInt32(row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                //                row.Delete();
//                //            }
//                //            // COmmented out by stodd. The delete right above will get rid of these records.
//                //            // ANd the need to just delete the summary rows isn't something very usefull.
//                //            // This will improve delete response time, when deleting a lot of rows.
//                //            //else
//                //            //    if (row.Band.Key == "SummaryRow")
//                //            //{
//                //            //    _auditData.AuditSummary_Delete(Convert.ToInt32(row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                //            //    row.Delete();
//                //            //}
//                //            else
//                //                if (row.Band.Key == "DetailRow")
//                //                {
//                //                    _auditData.AuditDetail_Delete(Convert.ToInt32(row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                //                    row.Delete();
//                //                }
//                //        }
//                //        ugAudit.ResumeRowSynchronization();
//                //        ugAudit.EndUpdate();
//                //    }
//                //}
//                //END TT#435-MD-DOConnell-Add new features to Audit
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//            finally
//            {
//                this.Cursor = Cursors.Default;
//            }
//        }


//        private void btn_Refresh_Click(object sender, EventArgs e)
//        {
//            // Begin TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//            this.Cursor = Cursors.WaitCursor;
//            try
//            {
//                // End TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//                // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
//                InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
//                layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, eLayoutID.auditViewerGrid, ugAudit);
//                // End TT#2012

//                //InitializeComponent();
//                //Populate_Audit(SAB);
//                InitializeForm();
//                // Begin TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//            }
//            finally
//            {
//                this.Cursor = Cursors.Default;
//            }
//            // End TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//        }

//        private void btnFilter_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                containerddfunctionDelegate.Invoke(sender, e);

//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        public void LoadGridDataSettings()
//        {
//            SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["messageLevelComboBox"]).Text);
//        }

//        private void ClearChartData()
//        {
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[0]["Count"] = 0;
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[1]["Count"] = 0;
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[2]["Count"] = 0;
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[3]["Count"] = 0;
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[4]["Count"] = 0;
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[5]["Count"] = 0;
//        }

//        #endregion 

//        #region Messages
//        private const int messageLevelDebug = 1; //TT#595-MD -jsobek -Add Debug to My Activity level 
//        private const int messageLevelInformation = 2;
//        private const int messageLevelEdit = 4;
//        private const int messageLevelWarning = 5;
//        private const int messageLevelError = 6;
//        private const int messageLevelSevere = 7;

//        int _currentMessageSelectedLevel = messageLevelWarning;
//        int _maxMessageLimit = 5000;


//        private void MessagesClear()
//        {
//            if (MessageBox.Show(_messageValidationClearWarningPrompt, _messageValidationClearWarningPromptTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
//            {
//                _auditDataSet.Tables[0].Rows.Clear();
//                ClearChartData();
//            }
//        }

//        private void SetMaxMessageLimit(string newLimitText, Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventArgs e)
//        {
//            int tempLimit;
//            if (int.TryParse(newLimitText, out tempLimit) == false)
//            {
//                String sMsg = _messageValidationMaxMessageLimitInvalidNumber;

//                if (e.ForceExit)
//                {
//                    e.RestoreOriginalValue = true;
//                    sMsg += "\r\n" + _messageValidationMaxMessageLimitRestoringOriginalMessage;
//                }
//                else
//                {
//                    e.Cancel = true;
//                }
//                MessageBox.Show(sMsg, _messageValidationMaxMessageLimitInvalidNumberTitle);
//                return;
//                //MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Breaddown);
//            }
//            if (tempLimit < 50)
//            {
//                String sMsg = _messageValidationMaxMessageLimitLowestValue;
//                if (e.ForceExit)
//                {
//                    e.RestoreOriginalValue = true;
//                    sMsg += "\r\n" + _messageValidationMaxMessageLimitRestoringOriginalMessage;
//                }
//                else
//                {
//                    e.Cancel = true;
//                }
//                MessageBox.Show(sMsg, _messageValidationMaxMessageLimitInvalidNumberTitle);
//                return;
//            }
//            if (tempLimit > _messageUpperLimit)
//            {
//                String sMsg = _messageValidationMaxMessageLimitHighestValue;
//                if (e.ForceExit)
//                {
//                    e.RestoreOriginalValue = true;
//                    sMsg += "\r\n" + _messageValidationMaxMessageLimitRestoringOriginalMessage;
//                }
//                else
//                {
//                    e.Cancel = true;
//                }
//                MessageBox.Show(sMsg, _messageValidationMaxMessageLimitInvalidNumberTitle);
//                return;
//            }

//            _maxMessageLimit = tempLimit;

//            Populate_Audit();
//        }

//        private void MessageSettingsSetResetPoint()
//        {
//            //Save the toolbar manager as it designed to serve as a reset point
//            this.ultraToolbarsManager1.SaveAsBinary(toolbarManagerSettingsMemoryStreamForReset, true);
//            this.ugAudit.DisplayLayout.Save(gridManagerSettingsMemoryStreamForReset);
//        }

//        private void MessageSettingsSave()
//        {
//            //string sFile = FindSavePath();
//            //this.ultraToolbarsManager1.SaveAsXml(sFile, false);
//            RaiseSaveSettingsEvent();

//        }

//        private void MessageSettingsReset()
//        {
//            toolbarManagerSettingsMemoryStreamForReset.Position = 0;
//            this.ultraToolbarsManager1.LoadFromBinary(toolbarManagerSettingsMemoryStreamForReset);
//            gridManagerSettingsMemoryStreamForReset.Position = 0;
//            this.ugAudit.DisplayLayout.Load(gridManagerSettingsMemoryStreamForReset);

//            LoadChartSettings();

//            RaiseResetSettingsEvent();
//        }

//        private void SetCurrentMessageLevel(string sLevel)
//        {
//            if (sLevel == "Debug") //TT#595-MD -jsobek -Add Debug to My Activity level  
//                _currentMessageSelectedLevel = messageLevelDebug;
//            else if (sLevel == "Information")
//                _currentMessageSelectedLevel = messageLevelInformation;
//            else if (sLevel == "Edit")
//                _currentMessageSelectedLevel = messageLevelEdit;
//            else if (sLevel == "Warning")
//                _currentMessageSelectedLevel = messageLevelWarning;
//            else if (sLevel == "Error")
//                _currentMessageSelectedLevel = messageLevelError;
//            else if (sLevel == "Severe")
//                _currentMessageSelectedLevel = messageLevelSevere;

//            _afp.HighestProcessMessageLevel = _currentMessageSelectedLevel;

//            Populate_Audit();
//        }

//        #endregion

//        private void DefaultGridLayout()
//        {
//            try
//            {
//                this.ugAudit.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
//                this.ugAudit.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
//                this.ugAudit.DisplayLayout.AddNewBox.Hidden = true;
//                this.ugAudit.DisplayLayout.AddNewBox.Prompt = "";
//                this.ugAudit.DisplayLayout.Bands[0].AddButtonCaption = "";
//                this.ugAudit.DisplayLayout.GroupByBox.Hidden = false;
//                this.ugAudit.DisplayLayout.GroupByBox.Prompt = "Drag here to group by column";
//                this.ugAudit.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["ProcessRID"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["ProcessID"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["UserRID"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["SummaryCode"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["HighestLevel"].Hidden = true;

//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].ColHeadersVisible = false;
//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["NeedsLoaded"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["ProcessRID"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["ProcessID"].Hidden = true;

//                this.ugAudit.DisplayLayout.Bands["Summary"].Columns["ProcessRID"].Hidden = true;

//                this.ugAudit.DisplayLayout.Bands["DetailRow"].ColHeadersVisible = false;
//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["NeedsLoaded"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["ProcessRID"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["ProcessID"].Hidden = true;

//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["ProcessRID"].Hidden = true;
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["MessageLevel"].Hidden = true;

//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["Message"].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["Message2"].CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["MessageCode"].Hidden = true;

//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["ProcessRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["ProcessID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["UserRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["SummaryCode"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["HighestLevel"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["NeedsLoaded"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["ProcessRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["ProcessID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

//                this.ugAudit.DisplayLayout.Bands["Summary"].Columns["ProcessRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["NeedsLoaded"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["ProcessRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["ProcessID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["ProcessRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["MessageLevel"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["MessageCode"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

//                this.ugAudit.DisplayLayout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.Both;
//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Override.AllowRowFiltering = DefaultableBoolean.False;
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(this, exception.Message);
//            }
//        }

//        public void InitializeForm()
//        {
//            try
//            {
//                // Begin TT#1243 - JSmith - Audit Performance
//                // close audit connection for commit pending writes
//                SAB.ClientServerSession.CloseAudit();
//                SAB.ApplicationServerSession.CloseAudit();
//                if (SAB.SchedulerServerSession != null)
//                {
//                    SAB.SchedulerServerSession.CloseAudit();
//                }
//                SAB.HeaderServerSession.CloseAudit();
//                SAB.HierarchyServerSession.CloseAudit();
//                SAB.StoreServerSession.CloseAudit();
//                // End TT#1243

//                //_afp = new AuditFilterProfile(SAB.ClientServerSession.UserRID);

//                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsAuditViewer);
//                eDataState dataState;
//                if (!FunctionSecurity.AllowUpdate)
//                {
//                    dataState = eDataState.ReadOnly;
//                }
//                else
//                {
//                    dataState = eDataState.Updatable;
//                }
//                //Format_Title(dataState, eMIDTextCode.frm_AuditViewer, null);
//                Populate_Audit();
//                //SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
//                if (!FunctionSecurity.AllowDelete)
//                {
//                    //btnDelete.Visible = false;
//                    ////TT#603 - MD - Audit Filter and Refresh buttons not available if read only. RBeck
//                    //btnFilter.Enabled = true;
//                    //btnRefresh.Enabled = true;
//                    //TT#603 - MD - Audit Filter and Refresh buttons not available if read only. RBeck
//                }
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(this, exception.Message);
//            }
//        }

//        private void ugAudit_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            InitializeLayout(); //TT#3845 - DOConnell - Reset Settings shows hidden columns
//        }
//        //TT#3845 - DOConnell - Reset Settings shows hidden columns
//        private void InitializeLayout()
//        {
//            // check for saved layout
//            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
//            InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.auditViewerGrid);
//            if (layout.LayoutLength > 0)
//            {
//                ugAudit.DisplayLayout.Load(layout.LayoutStream);
//            }
//            else
//            {	// DEFAULT grid layout
//                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
//                //ugld.ApplyDefaults(e);
//                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
//                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
//                //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
//                //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
//                // End TT#1164
//                //End TT#169
//                DefaultGridLayout();
//            }
//            CommonGridLayout();
//        }
//        //TT#3845 - DOConnell - Reset Settings shows hidden columns

//        private void CommonGridLayout()
//        {
//            try
//            {
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["Process"].Header.Caption = "Process";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["ProcessDesc"].Header.Caption = "Description";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["User"].Header.Caption = "User";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["StatusCodeText"].Header.Caption = "Status";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["StartTime"].Header.Caption = "Start Time";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["StopTime"].Header.Caption = "Stop Time";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["Duration"].Header.Caption = "Duration";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["SummaryCodeText"].Header.Caption = "Summary";
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["HighestLevelText"].Header.Caption = "Highest Message Level";

//                this.ugAudit.DisplayLayout.Bands["SummaryRow"].Columns["Text"].Header.Caption = "Summary";

//                this.ugAudit.DisplayLayout.Bands["Summary"].Columns["Item"].Header.Caption = "Item";
//                this.ugAudit.DisplayLayout.Bands["Summary"].Columns["Value"].Header.Caption = "Value";

//                this.ugAudit.DisplayLayout.Bands["DetailRow"].Columns["Text"].Header.Caption = "Detail";

//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["Time"].Header.Caption = "Time";
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["Module"].Header.Caption = "Module";
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["MessageLevelText"].Header.Caption = "Message Level";
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["Message"].Header.Caption = "Message";
//                this.ugAudit.DisplayLayout.Bands["Details"].Columns["Message2"].Header.Caption = "Message Details";

//                this.ugAudit.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
//                this.ugAudit.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(this, exception.Message);
//            }
//        }

//        private void Audit_Define()
//        {
//            try
//            {
//                _auditDataSet = MIDEnvironment.CreateDataSet("AuditDataSet");

//                DataTable auditHeaders = _auditDataSet.Tables.Add("Headers");

//                DataColumn dataColumn;

//                //Create Columns and rows for datatable
//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "StartTime";
//                dataColumn.Caption = "Start Time";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessRID";
//                dataColumn.Caption = "ProcessRID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessID";
//                dataColumn.Caption = "ProcessID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Process";
//                dataColumn.Caption = "Process";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                //				dataColumn = new DataColumn();
//                //				dataColumn.DataType = System.Type.GetType("System.Int32");
//                //				dataColumn.ColumnName = "StatusCode";
//                //				dataColumn.Caption = "StatusCode";
//                //				dataColumn.ReadOnly = true;
//                //				dataColumn.Unique = false;
//                //				auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "StatusCodeText";
//                dataColumn.Caption = "Status";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "UserRID";
//                dataColumn.Caption = "UserRID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "User";
//                dataColumn.Caption = "User";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "StopTime";
//                dataColumn.Caption = "Stop Time";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Duration";
//                dataColumn.Caption = "Duration";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "ProcessDesc";
//                dataColumn.Caption = "Description";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "SummaryCode";
//                dataColumn.Caption = "SummaryCode";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "SummaryCodeText";
//                dataColumn.Caption = "Summary";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "HighestLevel";
//                dataColumn.Caption = "HighestLevel";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "HighestLevelText";
//                dataColumn.Caption = "Highest Level";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditHeaders.Columns.Add(dataColumn);

//                // add summary header line
//                DataTable auditSummaryRow = _auditDataSet.Tables.Add("SummaryRow");

//                //Create Columns and rows for datatable
//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Boolean");
//                dataColumn.ColumnName = "NeedsLoaded";
//                dataColumn.ReadOnly = false;
//                dataColumn.Unique = false;
//                auditSummaryRow.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessRID";
//                dataColumn.Caption = "ProcessRID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditSummaryRow.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessID";
//                dataColumn.Caption = "ProcessID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditSummaryRow.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Text";
//                dataColumn.Caption = "Text";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditSummaryRow.Columns.Add(dataColumn);

//                // add summary counts for load processes
//                DataTable auditSummary = _auditDataSet.Tables.Add("Summary");

//                //Create Columns and rows for datatable
//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessRID";
//                dataColumn.Caption = "ProcessRID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditSummary.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Item";
//                dataColumn.Caption = "Item";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditSummary.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "Value";
//                dataColumn.Caption = "Value";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditSummary.Columns.Add(dataColumn);

//                // add detail header line
//                DataTable auditDetailRow = _auditDataSet.Tables.Add("DetailRow");

//                //Create Columns and rows for datatable
//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Boolean");
//                dataColumn.ColumnName = "NeedsLoaded";
//                dataColumn.ReadOnly = false;
//                dataColumn.Unique = false;
//                auditDetailRow.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessRID";
//                dataColumn.Caption = "ProcessRID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetailRow.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessID";
//                dataColumn.Caption = "ProcessID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetailRow.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Text";
//                dataColumn.Caption = "Text";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetailRow.Columns.Add(dataColumn);

//                // add detail audit messages
//                DataTable auditDetails = _auditDataSet.Tables.Add("Details");

//                //Create Columns and rows for datatable
//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "ProcessRID";
//                dataColumn.Caption = "ProcessRID";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Time";
//                dataColumn.Caption = "Time";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Module";
//                dataColumn.Caption = "Module";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "MessageLevel";
//                dataColumn.Caption = "MessageLevel";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "MessageLevelText";
//                dataColumn.Caption = "Message Level";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.Int32");
//                dataColumn.ColumnName = "MessageCode";
//                dataColumn.Caption = "MessageCode";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Message";
//                dataColumn.Caption = "Message";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);

//                dataColumn = new DataColumn();
//                dataColumn.DataType = System.Type.GetType("System.String");
//                dataColumn.ColumnName = "Message2";
//                dataColumn.Caption = "Message2";
//                dataColumn.ReadOnly = true;
//                dataColumn.Unique = false;
//                auditDetails.Columns.Add(dataColumn);



//                // add relationship between headers, summary and details
//                _auditDataSet.Relations.Add("SummaryRow",
//                    _auditDataSet.Tables["Headers"].Columns["ProcessRID"],
//                    _auditDataSet.Tables["SummaryRow"].Columns["ProcessRID"]);
//                _auditDataSet.Relations.Add("DetailRow",
//                    _auditDataSet.Tables["Headers"].Columns["ProcessRID"],
//                    _auditDataSet.Tables["DetailRow"].Columns["ProcessRID"]);
//                _auditDataSet.Relations.Add("Summary",
//                    _auditDataSet.Tables["SummaryRow"].Columns["ProcessRID"],
//                    _auditDataSet.Tables["Summary"].Columns["ProcessRID"]);
//                _auditDataSet.Relations.Add("Details",
//                    _auditDataSet.Tables["DetailRow"].Columns["ProcessRID"],
//                    _auditDataSet.Tables["Details"].Columns["ProcessRID"]);
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void UpdateChartRowCount(int aMessageLevel)
//        {
//            int row = 0;
//            //Begin TT#595-MD -jsobek -Add Debug to My Activity level 
//            switch (aMessageLevel)
//            {
//                case messageLevelSevere:
//                    row = 5;
//                    break;
//                case messageLevelError:
//                    row = 4;
//                    break;
//                case messageLevelWarning:
//                    row = 3;
//                    break;
//                case messageLevelEdit:
//                    row = 2;
//                    break;
//                case messageLevelInformation:
//                    row = 1;
//                    break;
//                case messageLevelDebug:
//                    row = 0;
//                    break;
//            }
//            //End TT#595-MD -jsobek -Add Debug to My Activity level 
//            ((DataTable)this.ultraChart1.Data.DataSource).Rows[row]["Count"] = (int)((DataTable)this.ultraChart1.Data.DataSource).Rows[row]["Count"] + 1;
//        }

//        public void resetFilterProfile()
//        {
//            _afp = null;
//        }

//        public void Populate_Audit()
//        {
//            string statusText;
//            int userRID;
//            DateTime runDateBetweenFrom = DateTime.MinValue;
//            DateTime runDateBetweenTo = DateTime.MinValue;

//            try
//            {
//                this.Cursor = Cursors.WaitCursor;
//                int messageLevel;
                

//                if (_afp == null)
//                {
//                    _afp = new AuditFilterProfile(SAB.ClientServerSession.UserRID);
//                    SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["messageLevelComboBox"]).Text); 
//                }

//                Audit_Define();
//                ugAudit.DataSource = null;
//                ugAudit.DataSource = _auditDataSet;
//                DataTable ah = _auditData.ProcessAuditHeader_Read();
//                DateTime stopTime;

//                foreach (DataRow dr in ah.Rows)
//                {
//                    if (_afp.ShowMyTasksOnly)
//                    {
//                        userRID = Convert.ToInt32(dr["User RID"], CultureInfo.CurrentUICulture);
//                        if (userRID != SAB.ClientServerSession.UserRID)
//                        {
//                            continue;
//                        }
//                    }

//                    DateTime startTime = Convert.ToDateTime(dr["Start Time"], CultureInfo.CurrentUICulture);
//                    // check run date
//                    switch (_afp.RunDateType)
//                    {
//                        case eFilterDateType.today:
//                            if (startTime.Date != DateTime.Now.Date)
//                            {
//                                continue;
//                            }
//                            break;
//                        case eFilterDateType.between:
//                            runDateBetweenFrom = DateTime.Now.Add(new TimeSpan(_afp.RunDateBetweenFrom, 0, 0, 0, 0));
//                            runDateBetweenTo = DateTime.Now.Add(new TimeSpan(_afp.RunDateBetweenTo, 0, 0, 0, 0));
//                            if (startTime.Date < runDateBetweenFrom.Date ||
//                                startTime.Date > runDateBetweenTo.Date)
//                            {
//                                continue;
//                            }
//                            break;
//                        case eFilterDateType.specify:
//                            if (startTime.Date < _afp.RunDateFrom.Date ||
//                                startTime.Date > _afp.RunDateTo.Date)
//                            {
//                                continue;
//                            }
//                            break;
//                    }

//                    string stringduration = "";
//                    string stringStopTime;
//                    if (dr["Stop Time"] != System.DBNull.Value)
//                    {
//                        if (!_afp.ShowCompletedProcesses)
//                        {
//                            continue;
//                        }

//                        stopTime = Convert.ToDateTime(dr["Stop Time"], CultureInfo.CurrentUICulture);
//                        TimeSpan duration = stopTime.Subtract(startTime);
//                        stringduration = Convert.ToString(duration, CultureInfo.CurrentUICulture);
//                        stringStopTime = stopTime.ToString(Include.AuditDateTimeFormat);

//                        messageLevel = Convert.ToInt32(dr["Highest Message Code"], CultureInfo.CurrentUICulture);
//                        if (messageLevel < _afp.HighestProcessMessageLevel)
//                        {
//                            continue;
//                        }

//                        if (duration.Milliseconds > 0 &&
//                            duration.Minutes < _afp.Duration)
//                        {
//                            continue;
//                        }
//                    }
//                    else
//                    {
//                        if (!_afp.ShowRunningProcesses)
//                        {
//                            continue;
//                        }

//                        stopTime = Include.UndefinedDate;
//                        stringStopTime = "";
//                    }

//                    if ((eProcessCompletionStatus)Convert.ToInt32(dr["Completion Status Code"], CultureInfo.CurrentUICulture) != eProcessCompletionStatus.None)
//                    {
//                        statusText = Convert.ToString(dr["Completion Status"], CultureInfo.CurrentUICulture);
//                    }
//                    else
//                    {
//                        statusText = Convert.ToString(dr["Execution Status"], CultureInfo.CurrentUICulture);
//                    }

//                    _auditDataSet.Tables["Headers"].Rows.Add(new object[] { // Convert.ToString(dr["Start Time"],CultureInfo.CurrentUICulture),
//                                                                            startTime.ToString(Include.AuditDateTimeFormat),
//                                                                            Convert.ToInt32(dr["Process RID"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToInt32(dr["Process ID"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToString(dr["Process"],CultureInfo.CurrentUICulture),
////																			Convert.ToInt32(dr["Status Code"],CultureInfo.CurrentUICulture),
////																			Convert.ToString(dr["Status"],CultureInfo.CurrentUICulture),
//                                                                            statusText,
//                                                                            Convert.ToInt32(dr["User RID"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToString(dr["User Name"],CultureInfo.CurrentUICulture),
////																			Convert.ToString(dr["Stop Time"],CultureInfo.CurrentUICulture),
//                                                                            stringStopTime,
//                                                                             stringduration,
//                                                                            Convert.ToString(dr["Description"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToInt32(dr["Summary Code"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToString(dr["Summary"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToInt32(dr["Highest Message Code"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToString(dr["Highest Message Level"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // add summary/detail rows
//                    _auditDataSet.Tables["SummaryRow"].Rows.Add(new object[] { true,
//                                                                                Convert.ToInt32(dr["Process RID"],CultureInfo.CurrentUICulture),
//                                                                                Convert.ToInt32(dr["Process ID"],CultureInfo.CurrentUICulture),
//                                                                                "Summary"	  
//                    });
//                    _auditDataSet.Tables["DetailRow"].Rows.Add(new object[] { true,
//                                                                                Convert.ToInt32(dr["Process RID"],CultureInfo.CurrentUICulture),
//                                                                                Convert.ToInt32(dr["Process ID"],CultureInfo.CurrentUICulture),
//                                                                                "Details"	  
//                    });

//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//            finally
//            {
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["Process"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
//                this.ugAudit.DisplayLayout.Bands["Headers"].Columns["StartTime"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
//                this.Cursor = Cursors.Default;
//            }
//        }

//        #region Audit Summary Info

//        private void AddHierarchyLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.HierarchyLoadAuditInfoAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Hierarchy Records",
//                                                                              Convert.ToInt32(dr["HIER_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Level Records",
//                                                                              Convert.ToInt32(dr["LEVEL_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Product Records",
//                                                                              Convert.ToInt32(dr["MERCH_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Product Records Added",
//                                                                              (dr["MERCH_ADDED"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["MERCH_ADDED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Product Records Updated",
//                                                                              (dr["MERCH_UPDATED"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["MERCH_UPDATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    //End TT#106 MD
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Move Records",
//                                                                              (dr["MOVE_RECS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["MOVE_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Rename Records",
//                                                                              (dr["RENAME_RECS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["RENAME_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Delete Records",
//                                                                              (dr["DELETE_RECS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["DELETE_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });

//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }


//        //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
//        private void AddHierarchyReclassSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.HierarchyReclassAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Hierarchy Output Records",
//                                                                              Convert.ToInt32(dr["HEIR_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Add Change Output Records",
//                                                                              Convert.ToInt32(dr["ADDCHG_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Deleted Output Records",
//                                                                              Convert.ToInt32(dr["DELETE_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Moved Output Records",
//                                                                              Convert.ToInt32(dr["MOVE_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Rejected Output Records",
//                                                                              Convert.ToInt32(dr["TRANS_REJECTED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
//        private void AddStoreLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.StoreLoadAuditInfoAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Records",
//                                                                              Convert.ToInt32(dr["STORE_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // Begin MID Track #4668 - add number added and modified
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Stores Added",
//                                                                              (dr["STORES_CREATED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_CREATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Stores Updated",
//                                                                              (dr["STORES_MODIFIED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_MODIFIED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // End MID Track #4668
//                    // BEGIN TT#739-MD - STodd - delete stores
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Stores Marked for Delete",
//                                                                              (dr["STORES_DELETED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_DELETED"],CultureInfo.CurrentUICulture)
//                                                                          });

//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Stores Recovered",
//                                                                              (dr["STORES_RECOVERED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_RECOVERED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // END TT#739-MD - STodd - delete stores
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });

//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        // BEGIN Issue 5117 stodd 4.17.2008
//        private void AddSpecialRequestSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.SpecialRequestAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Jobs in Special Request",
//                                                                              Convert.ToInt32(dr["TOTAL_JOBS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Jobs Processed",
//                                                                              Convert.ToInt32(dr["JOBS_PROCESSED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Successful Jobs",
//                                                                              Convert.ToInt32(dr["SUCCESSFUL_JOBS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Jobs with Errors",
//                                                                              Convert.ToInt32(dr["JOBS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        // END Issue 5117

//        private void AddHistoryPlanLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.PostingAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Chain History Records",
//                                                                              Convert.ToInt32(dr["CH_WK_HIS_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Chain Forecast Records",
//                                                                              Convert.ToInt32(dr["CH_WK_FOR_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Daily History Records",
//                                                                              Convert.ToInt32(dr["ST_DAY_HIS_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Weekly History Records",
//                                                                              Convert.ToInt32(dr["ST_WK_HIS_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Forecast Records",
//                                                                              Convert.ToInt32(dr["ST_WK_FOR_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Intransit Records",
//                                                                              Convert.ToInt32(dr["INTRANSIT_RECS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Nodes Added",
//                                                                              Convert.ToInt32(dr["NODES_ADDED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });

//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void AddHeaderLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.HeaderLoadAuditInfoAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read",
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Headers Created",
//                                                                              Convert.ToInt32(dr["HDRS_CREATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Headers Modified",
//                                                                              Convert.ToInt32(dr["HDRS_MODIFIED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Headers Removed",
//                                                                              Convert.ToInt32(dr["HDRS_REMOVED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Headers Reset",
//                                                                              Convert.ToInt32(dr["HDRS_RESET"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        //Begin MOD - JScott - Build Pack Criteria Load
//        private void AddBuildPackCriteriaLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.BuildPackCriteriaLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read",
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Criteria Added/Updated",
//                                                                              Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End MOD - JScott - Build Pack Criteria Load

//        //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
//        private void AddChainSetPercentCriteriaLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.ChainSetPercentCriteriaLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read",
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Criteria Added/Updated",
//                                                                              Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2

//        //Begin TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
//        private void AddStoreEligibilityCriteriaLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.StoreEligibilityCriteriaLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read",
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Criteria Added/Updated",
//                                                                              Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

//        //Begin TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
//        private void AddVSWCriteriaLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.VSWCriteriaLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read",
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Criteria Added/Updated",
//                                                                              Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API

//        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
//        private void AddDailyPercentagesCriteriaLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.DailyPercentagesCriteriaLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read",
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Criteria Added/Updated",
//                                                                              Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End TT#43 - MD - DOConnell - Projected Sales Enhancement


//        private void AddPurgeSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.PurgeAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Daily History Records",
//                                                                              Convert.ToInt32(dr["STORE_DAILY_HISTORY"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Chain Weekly History Records",
//                                                                              Convert.ToInt32(dr["CHAIN_WEEKLY_HISTORY"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Weekly History Records",
//                                                                              Convert.ToInt32(dr["STORE_WEEKLY_HISTORY"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Chain Forecast Records",
//                                                                              Convert.ToInt32(dr["CHAIN_WEEKLY_FORECAST"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Store Forecast Records",
//                                                                              Convert.ToInt32(dr["STORE_WEEKLY_FORECAST"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Header Records",
//                                                                              Convert.ToInt32(dr["HEADERS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Intransit Records",
//                                                                              Convert.ToInt32(dr["INTRANSIT"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    //Begin TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Intransit Review Records",
//                                                                              Convert.ToInt32(dr["INTRANSIT_REV"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    //End TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "User Records",
//                                                                              Convert.ToInt32(dr["USERS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Group Records",
//                                                                              Convert.ToInt32(dr["GROUPS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    //End Track #4815
//                    // Begin TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Daily Percentages Records",
//                                                                              (dr["DAILY_PERCENTAGES"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["DAILY_PERCENTAGES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // End TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
//                    // Begin TT#767 - JSmith - Purge Performance
//                    // BEGIN TT#739-MD - STodd - delete stores
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Empty Attribute Sets",
//                                                                              (dr["EMPTY_ATTRIBUTE_SETS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["EMPTY_ATTRIBUTE_SETS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // END TT#739-MD - STodd - delete stores
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Audit Records",
//                                                                              Convert.ToInt32(dr["AUDITS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    // Begin TT#767
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void AddColorCodeLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.ColorCodeLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read" ,
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Added",
//                                                                              Convert.ToInt32(dr["CODES_CREATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Updated",
//                                                                              Convert.ToInt32(dr["CODES_MODIFIED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void AddSizeCodeLoadSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.SizeCodeLoadAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read" ,
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Added",
//                                                                              Convert.ToInt32(dr["CODES_CREATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Updated",
//                                                                              Convert.ToInt32(dr["CODES_MODIFIED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void AddSizeConstraintsSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.SizeConstraintsLoadAuditInfo_Read(aProcessRID);
//                DataRow dr = summary.Rows[0];
//                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Records Read" ,
//                                                                          Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                      });
//                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Models Added",
//                                                                          Convert.ToInt32(dr["MODELS_CREATED"],CultureInfo.CurrentUICulture)
//                                                                      });
//                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Models Updated",
//                                                                          Convert.ToInt32(dr["MODELS_MODIFIED"],CultureInfo.CurrentUICulture)
//                                                                      });

//                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Models UpRemoved",
//                                                                          Convert.ToInt32(dr["MODELS_REMOVED"],CultureInfo.CurrentUICulture)
//                                                                      });

//                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Errors",
//                                                                          Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                      });
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        //Begin TT#707 - JScott - Size Curve process needs to multi-thread
//        private void AddSizeCurveGenerateSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.SizeCurveGenerateAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Methods Executed" ,
//                                                                          Convert.ToInt32(dr["MTHDS_EXECUTED"],CultureInfo.CurrentUICulture)
//                                                                      });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Methods Successful",
//                                                                          Convert.ToInt32(dr["MTHDS_SUCCESSFUL"],CultureInfo.CurrentUICulture)
//                                                                      });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Methods Failed",
//                                                                          Convert.ToInt32(dr["MTHDS_FAILED"],CultureInfo.CurrentUICulture)
//                                                                      });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                          "Methods with No Action Performed",
//                                                                          Convert.ToInt32(dr["MTHDS_NO_ACTION"],CultureInfo.CurrentUICulture)
//                                                                      });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        //End TT#707 - JScott - Size Curve process needs to multi-thread
//        private void AddRollupSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.RollupAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Items" ,
//                                                                              Convert.ToInt32(dr["TOTAL_ITEMS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Batch Size",
//                                                                              Convert.ToInt32(dr["BATCH_SIZE"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Batches",
//                                                                              Convert.ToInt32(dr["TOTAL_BATCHES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Concurrent Processes",
//                                                                              Convert.ToInt32(dr["CONCURRENT_PROCESSES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
//        private void AddComputationDriverSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.ComputationDriverAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Items" ,
//                                                                              Convert.ToInt32(dr["TOTAL_ITEMS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Concurrent Processes",
//                                                                              Convert.ToInt32(dr["CONCURRENT_PROCESSES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End - Abercrombie & Fitch #4411

//        //Begin Track #5100 - JSmith - Add counts to audit
//        private void AddRelieveIntransitSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.RelieveIntransitAuditInfo_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Read" ,
//                                                                              Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Records Accepted",
//                                                                              Convert.ToInt32(dr["RECS_ACCEPTED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Errors",
//                                                                              Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End Track #5100

//        // Begin TT#465 - stodd - sizeDayToweekSummary
//        private void AddSizeDayToWeekSummarySummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.SizeDayToWeekSummary_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Styles Processed",
//                                                                              Convert.ToInt32(dr["TOTAL_STYLES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Colors Processed",
//                                                                              Convert.ToInt32(dr["TOTAL_COLORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Sizes Processed",
//                                                                              Convert.ToInt32(dr["TOTAL_SIZES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Daily Records Read",
//                                                                              Convert.ToInt32(dr["TOTAL_RECS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Daily Values Processed",
//                                                                              Convert.ToInt32(dr["TOTAL_VALUES_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Weekly Records Written",
//                                                                              Convert.ToInt32(dr["TOTAL_RECS_WRITTEN"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Errors",
//                                                                              Convert.ToInt32(dr["TOTAL_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        // End TT#465 - stodd - sizeDayToweekSummary

//        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//        private void AddPushToBackStock(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.PushToBackStock_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Headers Read",
//                                                                              Convert.ToInt32(dr["HDRS_READ"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Headers with Errors",
//                                                                              Convert.ToInt32(dr["HDRS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Headers Processed",
//                                                                              Convert.ToInt32(dr["HDRS_PROCESSED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Headers Skipped",
//                                                                              Convert.ToInt32(dr["HDRS_SKIPPED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        // END TT#1401 - stodd - add resevation stores (IMO)

//        // Begin TT#710 - JSmith - Generate relieve intransit
//        private void AddGenerateRelieveIntransitSummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.GenerateRelieveIntransitSummary_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Headers to relieve",
//                                                                              Convert.ToInt32(dr["HEADERS_TO_RELIEVE"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Files generated",
//                                                                              Convert.ToInt32(dr["FILES_GENERATED"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Errors",
//                                                                              Convert.ToInt32(dr["TOTAL_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End TT#710

//        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
//        private void AddDetermineHierarchyActivitySummary(int aProcessRID)
//        {
//            try
//            {
//                DataTable summary = _auditData.DetermineHierarchyActivitySummary_Read(aProcessRID);
//                if (summary.Rows.Count == 1)
//                {
//                    DataRow dr = summary.Rows[0];
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Nodes",
//                                                                              Convert.ToInt32(dr["TOTAL_NODES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Active Nodes",
//                                                                              Convert.ToInt32(dr["ACTIVE_NODES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Inactive Nodes",
//                                                                              Convert.ToInt32(dr["INACTIVE_NODES"],CultureInfo.CurrentUICulture)
//                                                                          });
//                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
//                                                                              "Total Errors",
//                                                                              Convert.ToInt32(dr["TOTAL_ERRORS"],CultureInfo.CurrentUICulture)
//                                                                          });
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        //End TT#988

//        private void AddDetail(int aProcessRID)
//        {
//            try
//            {
//                DataTable ar = _auditData.AuditReport_Read(aProcessRID);
//                DateTime time;
//                int messageLevel;

//                foreach (DataRow dr in ar.Rows)
//                {
//                    messageLevel = Convert.ToInt32(dr["MessageLevelCode"], CultureInfo.CurrentUICulture);
//                    if (messageLevel < _afp.HighestDetailMessageLevel)
//                    {
//                        continue;
//                    }

//                    string message = string.Empty;
//                    //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
//                    string reportMessage = string.Empty;
//                    string rptMsg = string.Empty;
//                    ArrayList myStringArray = new ArrayList();
//                    int rptLen = 500;
//                    //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
//                    int messageCode = -1;
//                    if (dr["MessageCode"] != System.DBNull.Value)
//                    {
//                        messageCode = Convert.ToInt32(dr["MessageCode"], CultureInfo.CurrentUICulture);
//                        message = Convert.ToString(dr["MessageCode"], CultureInfo.CurrentUICulture) + ":" + Convert.ToString(dr["Message"], CultureInfo.CurrentUICulture);
//                    }
//                    time = Convert.ToDateTime(dr["Time"], CultureInfo.CurrentUICulture);
//                    //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
//                    if (dr["ReportMessage"] != System.DBNull.Value)
//                    {
//                        rptMsg = Convert.ToString(dr["ReportMessage"], CultureInfo.CurrentUICulture);

//                        // Begin TT#2102 - JSmith - Header Load Errors
//                        // add dummy string so line displays.
//                        if (rptMsg.Length > 0)
//                        {
//                            // End TT#2102
//                            while (rptMsg.Length > 0)
//                            {
//                                if (rptMsg.Length < 500) rptLen = rptMsg.Length;
//                                reportMessage = rptMsg.Substring(0, rptLen);
//                                rptMsg = rptMsg.Remove(0, rptLen);
//                                reportMessage.Trim();
//                                if (myStringArray.Count > 0) reportMessage = "(Continued from previous Message)" + "\r\n" + reportMessage;
//                                myStringArray.Add(reportMessage);
//                            }
//                            // Begin TT#2102 - JSmith - Header Load Errors
//                        }
//                        // add dummy string so line displays.
//                        else
//                        {
//                            myStringArray.Add(" ");
//                        }
//                        // End TT#2102
//                    }
//                    // Begin TT#2102 - JSmith - Header Load Errors
//                    // add dummy string so line displays.
//                    else
//                    {
//                        myStringArray.Add(" ");
//                    }
//                    // End TT#2102

//                    foreach (string reportMsg in myStringArray)
//                    {
//                        _auditDataSet.Tables["Details"].Rows.Add(new object[] {	Convert.ToInt32(dr["ProcessRID"],CultureInfo.CurrentUICulture),
//                                                                            time.ToString(Include.AuditDateTimeFormat),
//                                                                            Convert.ToString(dr["Module"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToInt32(dr["MessageLevelCode"],CultureInfo.CurrentUICulture),
//                                                                            Convert.ToString(dr["MessageLevel"],CultureInfo.CurrentUICulture),
//                                                                            messageCode,
//                                                                            message,
//                                                                            reportMsg
//                                                                          });
//                    }
//                    //                    _auditDataSet.Tables["Details"].Rows.Add(new object[] {	Convert.ToInt32(dr["ProcessRID"],CultureInfo.CurrentUICulture),
//                    ////																			Convert.ToString(dr["Time"],CultureInfo.CurrentUICulture),
//                    //                                                                            time.ToString(Include.AuditDateTimeFormat),
//                    //                                                                            Convert.ToString(dr["Module"],CultureInfo.CurrentUICulture),
//                    //                                                                            Convert.ToInt32(dr["MessageLevelCode"],CultureInfo.CurrentUICulture),
//                    //                                                                            Convert.ToString(dr["MessageLevel"],CultureInfo.CurrentUICulture),
//                    //                                                                            messageCode,
//                    //                                                                            message,
//                    //                                                                            Convert.ToString(dr["ReportMessage"],CultureInfo.CurrentUICulture)
//                    //                                                                          });
//                    //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
//                }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }
//        #endregion

//        #region "Chart"

//        private const int CHART_STARTING_WIDTH = 335;
//        private const int CHART_STARTING_HEIGHT = 150;

//        //private void ShowOrHideChart(bool makeVisible)
//        //{

//        //}
//        private void SetChartTitle()
//        {
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTitleShow = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleShowHide"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTop = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationTop"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationLeft"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationRight"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationBottom"];

//            this.ultraChart1.TitleTop.Visible = false;
//            this.ultraChart1.TitleLeft.Visible = false;
//            this.ultraChart1.TitleRight.Visible = false;
//            this.ultraChart1.TitleBottom.Visible = false;

//            if (sbTitleShow.Checked == true)
//            {
//                if (sbTop.Checked == true)
//                {
//                    this.ultraChart1.TitleTop.Visible = true;
//                }
//                else if (sbLeft.Checked == true)
//                {
//                    this.ultraChart1.TitleLeft.Visible = true;
//                }
//                else if (sbRight.Checked == true)
//                {
//                    this.ultraChart1.TitleRight.Visible = true;
//                }
//                else if (sbBottom.Checked == true)
//                {
//                    this.ultraChart1.TitleBottom.Visible = true;
//                }
//            }
//        }
//        private void SetChartLegend()
//        {
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLegendShow = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartShowLegend"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTop = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendTop"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendLeft"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendRight"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendBottom"];

//            this.ultraChart1.Legend.Visible = false;

//            if (sbLegendShow.Checked == true)
//            {
//                this.ultraChart1.Legend.Visible = true;
//                if (sbTop.Checked == true)
//                {
//                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Top;
//                }
//                else if (sbLeft.Checked == true)
//                {
//                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Left;
//                }
//                else if (sbRight.Checked == true)
//                {
//                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Right;
//                }
//                else if (sbBottom.Checked == true)
//                {
//                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Bottom;
//                }
//            }
//        }
//        private void SetChartVisibility()
//        {
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sb = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartShowHide"];
//            if (sb.Checked == true)
//            {
//                this.ultraChart1.Visible = true;
//                this.chartSplitter1.Visible = true;
//                if (this.ultraChart1.Height == 0)
//                {
//                    this.ultraChart1.Height = CHART_STARTING_HEIGHT;
//                }
//                if (this.ultraChart1.Width == 0)
//                {
//                    this.ultraChart1.Width = CHART_STARTING_WIDTH;
//                }
//            }
//            else
//            {
//                this.ultraChart1.Visible = false;
//                this.chartSplitter1.Visible = false;
//            }
//        }
//        private void SetChartDocking()
//        {
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbDockLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartDockLeft"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbDockRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartDockRight"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbDockBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartDockBottom"];

//            if (sbDockLeft.Checked == true)
//            {
//                this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Left;
//                this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Left;
//            }
//            else if (sbDockRight.Checked == true)
//            {
//                this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Right;
//                this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
//            }
//            else if (sbDockBottom.Checked == true)
//            {
//                this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Bottom;
//                this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
//            }
//            this.ugAudit.BringToFront();

//        }
//        private void SetChartType()
//        {
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBar = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeBar"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLine = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeLine"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbPie = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypePie"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbPyramid = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypePyramid"];
//            Infragistics.Win.UltraWinToolbars.StateButtonTool sbHistogram = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeHistogram"];


//            this.ultraChart1.ResetAxis();
//            if (sbBar.Checked == true)
//            {
//                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart;
//                this.ultraChart1.Data.SwapRowsAndColumns = true;
//                this.ultraChart1.Axis.X.Labels.SeriesLabels.Visible = false;
//            }
//            else if (sbLine.Checked == true)
//            {
//                this.ultraChart1.Data.SwapRowsAndColumns = false;
//                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnLineChart;
//                this.ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
//                this.ultraChart1.Axis.X.Labels.ItemFormatString = "";
//                this.ultraChart1.Axis.X2.Visible = false;
//            }
//            else if (sbPyramid.Checked == true)
//            {
//                this.ultraChart1.Data.SwapRowsAndColumns = false;
//                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PyramidChart;
//            }
//            else if (sbHistogram.Checked == true)
//            {
//                this.ultraChart1.Data.SwapRowsAndColumns = false;
//                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.HistogramChart;
//            }
//            else //Use Pie Chart as default
//            {
//                this.ultraChart1.Data.SwapRowsAndColumns = false;
//                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
//            }

//        }
//        public void LoadChartSettings()
//        {
//            SetChartTitle();
//            SetChartLegend();
//            SetChartType();
//            SetChartVisibility();
//            SetChartDocking();
//        }

//        private void InitializeChart()
//        {
//            this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
//            this.ultraChart1.ColorModel.CustomPalette = new Color[] { Color.DarkGreen, Color.LightGreen, Color.Blue, Color.Yellow, Color.OrangeRed, Color.Red }; //TT#595-MD -jsobek -Add Debug to My Activity level 

//            this.ultraChart1.TitleTop.Text = "Audit";
//            this.ultraChart1.TitleTop.HorizontalAlign = StringAlignment.Center;
//            this.ultraChart1.TitleBottom.Text = "Audit";
//            this.ultraChart1.TitleBottom.HorizontalAlign = StringAlignment.Center;
//            this.ultraChart1.TitleLeft.Text = "Audit";
//            this.ultraChart1.TitleRight.Text = "Audit";

//            this.ultraChart1.TitleTop.Visible = false;
//            this.ultraChart1.TitleLeft.Visible = false;
//            this.ultraChart1.TitleRight.Visible = false;
//            this.ultraChart1.TitleBottom.Visible = false;


//            DataTable dt = new DataTable();
//            dt.Columns.Add("Count", typeof(int));
//            dt.Columns.Add("Type", typeof(string));
//            dt.Rows.Add(new object[] { 0, "Debug" }); //TT#595-MD -jsobek -Add Debug to My Activity level 
//            dt.Rows.Add(new object[] { 0, "Info" });
//            dt.Rows.Add(new object[] { 0, "Edit" });
//            dt.Rows.Add(new object[] { 0, "Warning" });
//            dt.Rows.Add(new object[] { 0, "Error" });
//            dt.Rows.Add(new object[] { 0, "Severe" });
//            this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
//            this.ultraChart1.Data.RowLabelsColumn = 1;
//            this.ultraChart1.Data.UseRowLabelsColumn = true;
//            this.ultraChart1.Legend.Visible = true;
//            this.ultraChart1.Data.DataSource = dt;
//            this.ultraChart1.Data.DataBind();
//        }

//        private void ExportChart()
//        {

//            string PDF_File = FindPDFSavePath();
//            if (PDF_File != null)
//            {
//                Infragistics.Documents.Reports.Report.Report r = new Infragistics.Documents.Reports.Report.Report();

//                Graphics g = r.AddSection().AddCanvas().CreateGraphics();
//                ultraChart1.RenderPdfFriendlyGraphics(g);

//                r.Publish(PDF_File, Infragistics.Documents.Reports.Report.FileFormat.PDF);
//            }

//        }
//        private String FindPDFSavePath()
//        {
//            System.IO.Stream myStream;
//            string myFilepath = null;
//            try
//            {
//                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
//                saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
//                saveFileDialog1.FilterIndex = 2;
//                saveFileDialog1.RestoreDirectory = true;
//                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
//                {
//                    if ((myStream = saveFileDialog1.OpenFile()) != null)
//                    {
//                        myFilepath = saveFileDialog1.FileName;
//                        myStream.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return myFilepath;
//        }

//        #endregion

//        private void ugAudit_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
//        {
//            try
//            {
//                if (!FunctionSecurity.AllowDelete)
//                {
//                    e.Cancel = true;
//                }
//                else
//                    if (_displayDeletePrompt)
//                    {
//                        e.DisplayPromptMsg = true;
//                    }
//                    else
//                    {
//                        e.DisplayPromptMsg = false;
//                    }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        private void ugAudit_AfterRowsDeleted(object sender, System.EventArgs e)
//        {

//        }

//        private void ugAudit_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
//        {
//            try
//            {
//                // Begin TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//                this.Cursor = Cursors.WaitCursor;
//                // End TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//                if (e.Row.Band.Key == "SummaryRow")
//                {
//                    if ((bool)e.Row.Cells["NeedsLoaded"].Value)
//                    {
//                        eProcesses processType = (eProcesses)e.Row.Cells["ProcessID"].Value;
//                        switch (processType)
//                        {
//                            case eProcesses.hierarchyLoad:
//                                AddHierarchyLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            case eProcesses.storeLoad:
//                                AddStoreLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            case eProcesses.historyPlanLoad:
//                                AddHistoryPlanLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            case eProcesses.headerLoad:
//                                AddHeaderLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //Begin MOD - JScott - Build Pack Criteria Load
//                            case eProcesses.buildPackCriteriaLoad:
//                                AddBuildPackCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End MOD - JScott - Build Pack Criteria Load
//                            //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
//                            case eProcesses.ChainSetPercentCriteriaLoad:
//                                AddChainSetPercentCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
//                            //Begin TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
//                            case eProcesses.StoreEligibilityCriteraLoad:
//                                AddStoreEligibilityCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
//                            //Begin TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
//                            case eProcesses.VSWCriteriaLoad:
//                                AddVSWCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
//                            //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
//                            case eProcesses.DailyPercentagesCriteraLoad:
//                                AddDailyPercentagesCriteriaLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#43 - MD - DOConnell - Projected Sales Enhancement

//                            case eProcesses.purge:
//                                AddPurgeSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            case eProcesses.colorCodeLoad:
//                                AddColorCodeLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            case eProcesses.sizeCodeLoad:
//                                AddSizeCodeLoadSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //Begin TT#707 - JScott - Size Curve process needs to multi-thread
//                            case eProcesses.sizeCurveGenerate:
//                                AddSizeCurveGenerateSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#707 - JScott - Size Curve process needs to multi-thread
//                            case eProcesses.rollup:
//                                AddRollupSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
//                            case eProcesses.computationDriver:
//                                AddComputationDriverSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End - Abercrombie & Fitch #4411
//                            case eProcesses.sizeConstraintsLoad:
//                                AddSizeConstraintsSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //Begin Track #5100 - JSmith - Add counts to audit
//                            case eProcesses.relieveIntransit:
//                                AddRelieveIntransitSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            // End Track #5100
//                            //BEGIN issue 5117 - stodd - special request
//                            case eProcesses.specialRequest:
//                                AddSpecialRequestSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //END issue 5117
//                            //BEGIN TT#465 - stodd - Size Day to Week Summary
//                            case eProcesses.SizeDayToWeekSummary:
//                                AddSizeDayToWeekSummarySummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#465 - stodd - Size Day to Week Summary
//                            // Begin TT#710 - JSmith - Generate relieve intransit
//                            case eProcesses.generateRelieveIntransit:
//                                AddGenerateRelieveIntransitSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#710
//                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
//                            case eProcesses.determineHierarchyActivity:
//                                AddDetermineHierarchyActivitySummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End TT#988
//                            // BEGIN TT#1401 - stodd - add resevation stores (IMO)							
//                            case eProcesses.pushToBackStock:
//                                AddPushToBackStock(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
//                            case eProcesses.hierarchyReclass:
//                                AddHierarchyReclassSummary(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                                break;
//                            //End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  

//                        }
//                        e.Row.Cells["NeedsLoaded"].Value = false;
//                    }
//                }
//                else
//                    if (e.Row.Band.Key == "DetailRow")
//                    {
//                        if ((bool)e.Row.Cells["NeedsLoaded"].Value)
//                        {
//                            AddDetail(Convert.ToInt32(e.Row.Cells["ProcessRID"].Value, CultureInfo.CurrentUICulture));
//                            e.Row.Cells["NeedsLoaded"].Value = false;
//                        }
//                    }
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//            // Begin TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//            finally
//            {
//                this.Cursor = Cursors.Default;
//            }
//            // End TT#427-MD - JSmith - Cursor does not show as busy when expanding rows.
//        }

//        public Infragistics.Win.UltraWinGrid.UltraGrid myAuditGrid
//        {
//            get { return ugAudit; }
//        }
//        public Infragistics.Win.UltraWinToolbars.UltraToolbarsManager myAuditToolbarManager
//        {
//            get { return ultraToolbarsManager1; }
//        }
//        public Infragistics.Win.UltraWinChart.UltraChart myActivityChart
//        {
//            get { return ultraChart1; }
//        }
//        private System.IO.MemoryStream toolbarManagerSettingsMemoryStreamForReset = new System.IO.MemoryStream();  //used to undo user settings, and restore the toolbar manager back to the designed settings
//        private System.IO.MemoryStream gridManagerSettingsMemoryStreamForReset = new System.IO.MemoryStream();

//        #region "SaveSettings Event"
//        public class SaveSettingsEventArgs
//        {
//            public SaveSettingsEventArgs(Infragistics.Win.UltraWinGrid.UltraGrid ug, Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utm) { myAuditGrid = ug; myAuditToolbarManager = utm; }
//            public Infragistics.Win.UltraWinGrid.UltraGrid myAuditGrid { get; private set; } // readonly
//            public Infragistics.Win.UltraWinToolbars.UltraToolbarsManager myAuditToolbarManager { get; private set; } // readonly

//        }


//        // Declare the delegate (if using non-generic pattern)
//        public delegate void SaveSettingsEventHandler(object sender, SaveSettingsEventArgs e);

//        // Declare the event.
//        public event SaveSettingsEventHandler SaveSettingsEvent;

//        // Wrap the event in a protected virtual method
//        // to enable derived classes to raise the event.
//        protected virtual void RaiseSaveSettingsEvent()
//        {
//            // Raise the event by using the () operator.
//            if (SaveSettingsEvent != null)
//                SaveSettingsEvent(this, new SaveSettingsEventArgs(this.ugAudit, this.ultraToolbarsManager1));
//        }

//        private void doSaveSettings(object sender, Windows.Controls.AuditContainer.SaveSettingsEventArgs e)
//        {
//            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();

//            System.IO.MemoryStream gridMemoryStream = new System.IO.MemoryStream();

//            e.myAuditGrid.DisplayLayout.Save(gridMemoryStream);
//            layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.auditViewerGrid, gridMemoryStream);

//            System.IO.MemoryStream toolbarManagerMemoryStream = new System.IO.MemoryStream();
//            e.myAuditToolbarManager.SaveAsBinary(toolbarManagerMemoryStream, true);
//            layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.auditViewerToolbars, toolbarManagerMemoryStream);
//        }

//        #endregion

//        #region "ResetSettings Event"
//        public class ResetSettingsEventArgs
//        {
//            public ResetSettingsEventArgs() { }
//        }


//        // Declare the delegate (if using non-generic pattern)
//        public delegate void ResetSettingsEventHandler(object sender, ResetSettingsEventArgs e);

//        // Declare the event.
//        public event ResetSettingsEventHandler ResetSettingsEvent;

//        // Wrap the event in a protected virtual method
//        // to enable derived classes to raise the event.
//        protected virtual void RaiseResetSettingsEvent()
//        {
//            // Raise the event by using the () operator.
//            if (ResetSettingsEvent != null)
//                ResetSettingsEvent(this, new ResetSettingsEventArgs());
//        }

//        private void doResetSettings(object sender, Windows.Controls.AuditContainer.ResetSettingsEventArgs e)
//        {
//            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
//            layoutData.InfragisticsLayout_Delete(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.auditViewerGrid);
//            layoutData.InfragisticsLayout_Delete(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.auditViewerGrid);
//            InitializeLayout(); //TT#3845 - DOConnell - Reset Settings shows hidden columns
//        }
//        #endregion
//    }
//}
