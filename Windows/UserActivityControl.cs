using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
    public partial class UserActivityControl : UserControl
    {
        public UserActivityControl()
        {
            InitializeComponent();
        }

        private const string myActivityVersion = "01.001.0006";
        private void UserActivityControl_Load(object sender, EventArgs e)
        {
            //The User Activity Control load event fires BEFORE the the User Activity Explorer.  This is important to know when saving/loading settings.
            //tag the grid and the toolbar with a version# 
            this.ultraToolbarsManager1.ToolbarSettings.Tag = myActivityVersion;
            this.ugUserActivityExplorer.Tag = myActivityVersion;

            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            this.ultraToolbarsManager1.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.ultraToolbarManager1_ToolValueChanged);
            this.ultraToolbarsManager1.BeforeToolExitEditMode += new Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventHandler(this.ultraToolbarsManager1_BeforeToolExitEditMode);
            this.ultraToolbarsManager1.ToolKeyPress += new Infragistics.Win.UltraWinToolbars.ToolKeyPressEventHandler(this.ultraToolbarsManager1_ToolKeyPress);

            

            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer; 
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column."); 
            rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            this.ugUserActivityExplorer.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;

            this.ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["Image"].Header.Caption = "";
            //ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["Time"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Time);
            //ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["Module"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Module);
            //ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["Message Level"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Level);
            //ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["Message"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Message);
            //ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["Details"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Details);
            this.ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["MessageIntLevel"].Hidden = true;
            this.ugUserActivityExplorer.DisplayLayout.Bands[0].Columns["MessageIntLevel"].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            this.ugUserActivityExplorer.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ugUserActivityExplorer.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ugUserActivityExplorer.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ugUserActivityExplorer.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;

            // this.lblDetailMessageLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_HighestDetailLevel);
            //this.btnClearMessages.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Clear);

            //this.ultraChart1.TitleBottom.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Breaddown);
            // this.cbxShowChart.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Show_Chart);

            MessageSettingsSetResetPoint();
            MessageDataTableInitialize();
            InitializeChart();
        }

        private MIDRetail.Business.SessionAddressBlock _SAB;
        public MIDRetail.Business.SessionAddressBlock SAB
        {
            get { return _SAB; }
            set { _SAB = value; }
        }

        private void UserActivityControl_Leave(object sender, EventArgs e)
        {

        }

        private void UserActivityControl_Enter(object sender, EventArgs e)
        {

        }

 
        public Infragistics.Win.UltraWinGrid.UltraGrid myActivityGrid
        {
            get { return ugUserActivityExplorer; }
        }
        public Infragistics.Win.UltraWinToolbars.UltraToolbarsManager myActivityToolbarManager
        {
            get { return ultraToolbarsManager1; }
        }
        public Infragistics.Win.UltraWinChart.UltraChart myActivityChart
        {
            get { return ultraChart1; }
        }

        #region "SaveSettings Event"
        public class SaveSettingsEventArgs
        {
          public SaveSettingsEventArgs(Infragistics.Win.UltraWinGrid.UltraGrid ug, Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utm) { myActivityGrid = ug; myActivityToolbarManager = utm; }
          public Infragistics.Win.UltraWinGrid.UltraGrid myActivityGrid {get; private set;} // readonly
          public Infragistics.Win.UltraWinToolbars.UltraToolbarsManager myActivityToolbarManager { get; private set; } // readonly
  
        }


        // Declare the delegate (if using non-generic pattern)
        public delegate void SaveSettingsEventHandler(object sender, SaveSettingsEventArgs e);

        // Declare the event.
        public event SaveSettingsEventHandler SaveSettingsEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseSaveSettingsEvent()
        {
            // Raise the event by using the () operator.
            if (SaveSettingsEvent != null)
                SaveSettingsEvent(this, new SaveSettingsEventArgs(this.ugUserActivityExplorer, this.ultraToolbarsManager1));
        }
        #endregion

        #region "ResetSettings Event"
        public class ResetSettingsEventArgs
        {
            public ResetSettingsEventArgs() { }
        }


        // Declare the delegate (if using non-generic pattern)
        public delegate void ResetSettingsEventHandler(object sender, ResetSettingsEventArgs e);

        // Declare the event.
        public event ResetSettingsEventHandler ResetSettingsEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseResetSettingsEvent()
        {
            // Raise the event by using the () operator.
            if (ResetSettingsEvent != null)
                ResetSettingsEvent(this, new ResetSettingsEventArgs());
        }
        #endregion


        #region "Toolbar Manager"

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {

                #region "Message Tools"

                case "messageClear":
                    MessagesClear();
                    break;
                case "messageSettingsSave": 
                    MessageSettingsSave();
                    break;
                case "messageSettingsReset":
                    MessageSettingsReset();
                    break;

                #endregion

                #region "Grid Tools"

                case "gridSearchFindButton":
                    MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(ugUserActivityExplorer, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
                    break;
                case "gridSearchClearButton":
                    Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
                    t.Text = "";
                    MIDRetail.Windows.Controls.SharedControlRoutines.ClearGridSearchResults(ugUserActivityExplorer);
                    break;

                case "gridShowSearchToolbar":
                    this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible = !this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible;
                    break;

                case "gridShowGroupArea":
                    this.ugUserActivityExplorer.DisplayLayout.GroupByBox.Hidden = !this.ugUserActivityExplorer.DisplayLayout.GroupByBox.Hidden;
                    break;

                case "gridShowFilterRow":
                    if (this.ugUserActivityExplorer.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
                    {
                        this.ugUserActivityExplorer.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
                    }
                    else
                    {
                        this.ugUserActivityExplorer.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
                    }
                    break;

                case "gridExportSelected":
                    //UltraGridExcelExportWrapper exporter = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                    //exporter.ExportSelectedRowsToExcel();
                    SharedRoutines.GridExport.ExportSelectedRowsToExcel(this.ugUserActivityExplorer);
                    break;

                case "gridExportAll":
                    //UltraGridExcelExportWrapper exporter2 = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                    //exporter2.ExportAllRowsToExcel();
                    SharedRoutines.GridExport.ExportAllRowsToExcel(this.ugUserActivityExplorer);
                    break;

                case "gridEmailSelectedRows":
                    //UltraGridExcelExportWrapper exporter3 = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                    //ShowEmailForm(exporter3.ExportSelectedRowsToExcelAsAttachment());

                    SharedRoutines.GridExport.EmailSelectedRows(SharedRoutines.GridExport.BuildEmailSubjectWithUserName(_SAB, "My Activity"), "MyActivity.xls", this.ugUserActivityExplorer);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    SharedRoutines.GridExport.EmailAllRows(SharedRoutines.GridExport.BuildEmailSubjectWithUserName(_SAB, "My Activity"), "MyActivity.xls", this.ugUserActivityExplorer);
                    break;

                case "gridChooseColumns":
                    this.ugUserActivityExplorer.ShowColumnChooser("Choose Columns");
                    break;
                
                #endregion

                #region "Chart Tools"
                case "chartShowHide":
                    SetChartVisibility();
                    break;

                case "chartDockLeft":
                    SetChartDocking(); 
                    break;

                case "chartDockBottom":
                    SetChartDocking();
                    break;

                case "chartDockRight":
                    SetChartDocking();
                    break;
                case "chartLegendTop":
                    SetChartLegend();
                    break;

                case "chartLegendLeft":
                    SetChartLegend();
                    break;

                case "chartLegendRight":
                    SetChartLegend();
                    break;

                case "chartLegendBottom":
                    SetChartLegend();
                    break;

                case "chartTypeBar":
                    SetChartType(); 
                    break;
                case "chartTypeLine":
                    SetChartType(); 
                    break;
                case "chartTypePie":
                    SetChartType(); 
                    break;
                case "chartTypePyramid":
                    SetChartType(); 
                    break;
                case "chartTypeHistogram":
                    SetChartType(); 
                    break;
                case "chartShowLegend":
                    SetChartLegend();
                    break;
                case "chartTitleShowHide":
                    SetChartTitle();
                    break;
                case "chartTitleLocationTop":
                    SetChartTitle();
                    break;
                case "chartTitleLocationLeft":
                    SetChartTitle();
                    break;
                case "chartTitleLocationRight":
                    SetChartTitle();
                    break;
                case "chartTitleLocationBottom":
                    SetChartTitle();
                    break;
                case "chartExport":
                    ExportChart();
                    break;
                //case "chartShowRowLabels":
                //    this.ultraChart1.Data.UseRowLabelsColumn = !this.ultraChart1.Data.UseRowLabelsColumn;
                //    break;

                #endregion
                

                





            }
        }

        private void ultraToolbarManager1_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "messageLevelComboBox":
                    SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text);
                    break;
            }
        }

        private void ultraToolbarsManager1_BeforeToolExitEditMode(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "messageTextMaxLimit":
                    SetMaxMessageLimit(((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text, e);
                    break;
            }
        }

        private void ultraToolbarsManager1_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "gridSearchText":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(ugUserActivityExplorer, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
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

        private void ultraToolbarsManager1_AfterToolExitEditMode(object sender, Infragistics.Win.UltraWinToolbars.AfterToolExitEditModeEventArgs e)
        {
            //switch (e.Tool.Key)
            //{
            //    case "messageLevelComboBox":
            //        SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text);
            //        break;
            //}
        }

        private void ultraToolbarsManager1_AfterToolCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
        {
            //switch (e.Tool.Key)
            //{
            //    case "messageLevelComboBox":
            //        SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text);
            //        break;
            //}
        }


        #endregion



        #region "Messages"

        private System.IO.MemoryStream toolbarManagerSettingsMemoryStreamForReset = new System.IO.MemoryStream();  //used to undo user settings, and restore the toolbar manager back to the designed settings
        private System.IO.MemoryStream gridManagerSettingsMemoryStreamForReset = new System.IO.MemoryStream();  //used to undo user settings, and restore the grid back to the designed settings
        private DataSet dsUserMessages = new DataSet();
        private DataView dvUserMessages;

        private const int messageLevelDebug = 1; //TT#595-MD -jsobek -Add Debug to My Activity level 
        private const int messageLevelInformation = 2;
        private const int messageLevelEdit = 4;
        private const int messageLevelWarning = 5;
        private const int messageLevelError = 6;
        private const int messageLevelSevere = 7;


        int _currentMessageSelectedLevel = messageLevelWarning;
        int _maxMessageLimit = 5000;

        private void MessageSettingsSetResetPoint()
        {
            //Save the toolbar manager as it designed to serve as a reset point
            this.ultraToolbarsManager1.SaveAsBinary(toolbarManagerSettingsMemoryStreamForReset, true);
            this.ugUserActivityExplorer.DisplayLayout.Save(gridManagerSettingsMemoryStreamForReset);
        }
        private void MessageSettingsSave()
        {
            //string sFile = FindSavePath();
            //this.ultraToolbarsManager1.SaveAsXml(sFile, false);
            RaiseSaveSettingsEvent();

        }
        private void MessageSettingsReset()
        {
            toolbarManagerSettingsMemoryStreamForReset.Position = 0;
            this.ultraToolbarsManager1.LoadFromBinary(toolbarManagerSettingsMemoryStreamForReset);
            gridManagerSettingsMemoryStreamForReset.Position = 0;
            this.ugUserActivityExplorer.DisplayLayout.Load(gridManagerSettingsMemoryStreamForReset);

            LoadChartSettings();

            RaiseResetSettingsEvent();

            //ensure the chart is visible and docked to the right
            //this.ultraChart1.Width = CHART_STARTING_WIDTH;
            //this.ultraChart1.Height = CHART_STARTING_HEIGHT;
            //this.ultraChart1.Visible = true;
            //this.chartSplitter1.Visible = true;
            //this.ultraChart1.Dock = DockStyle.Right;
            //this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            //this.ugUserActivityExplorer.BringToFront();
        }

        private void MessageDataTableInitialize()
        {
            dsUserMessages.Tables.Add("dtUserMessages");
            dsUserMessages.Tables[0].Columns.Add("Image", typeof(Image));
            dsUserMessages.Tables[0].Columns.Add("Time", typeof(String));
            dsUserMessages.Tables[0].Columns.Add("Module", typeof(String));
            dsUserMessages.Tables[0].Columns.Add("Message Level", typeof(String));
            dsUserMessages.Tables[0].Columns.Add("Message", typeof(String));
            dsUserMessages.Tables[0].Columns.Add("Details", typeof(String));
            dsUserMessages.Tables[0].Columns.Add("MessageIntLevel", typeof(int));

           

            RefreshMessageDataView();
        }

        private void RefreshMessageDataView()
        {
            if (dsUserMessages.Tables.Count > 0)
            {
                //dvUserMessages = new DataView(dsUserMessages.Tables[0], "MessageIntLevel >= " + _currentMessageSelectedLevel, "Time DESC", DataViewRowState.CurrentRows);

             

                dvUserMessages = new DataView(dsUserMessages.Tables[0], "MessageIntLevel >= " + _currentMessageSelectedLevel, "", DataViewRowState.CurrentRows);
                ugUserActivityExplorer.DataSource = dvUserMessages;


             

                //ugUserActivityExplorer.ResumeRowSynchronization();
            }
        }
        public void LoadGridDataSettings()
        {
                    SetCurrentMessageLevel(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["messageLevelComboBox"]).Text);   
        }

        public void AddMessage(string aTime, string aModule, string aMessageLevelText, string aMessage, string aMessageDetails, Image aImage, int aMessageLevel)
        {
            DataRow dr = dsUserMessages.Tables[0].NewRow();
            dr["Image"] = aImage;

            DateTime messageDate;
            if (DateTime.TryParse(aTime, out messageDate) == true)
            {
                //Begin TT#538-MD -jsobek -My Activity needs to display as military time
                //aTime = messageDate.ToString("yyyy-MM-dd") + " " + messageDate.ToLongTimeString();
                aTime = messageDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //End TT#538-MD -jsobek -My Activity needs to display as military time
            }

            dr["Time"] = aTime;
            dr["Module"] = aModule;
            dr["Message Level"] = aMessageLevelText;
            dr["Message"] = aMessage;
            dr["Details"] = aMessageDetails;
            dr["MessageIntLevel"] = aMessageLevel;

            this.ugUserActivityExplorer.BeginUpdate();
            this.ugUserActivityExplorer.SuspendRowSynchronization();

            dsUserMessages.Tables[0].Rows.InsertAt(dr, 0);

            if (dsUserMessages.Tables[0].Rows.Count > _maxMessageLimit)
            {
                dsUserMessages.Tables[0].Rows.RemoveAt(dsUserMessages.Tables[0].Rows.Count - 1);
            }

            this.ugUserActivityExplorer.ResumeRowSynchronization();
            this.ugUserActivityExplorer.EndUpdate();

            UpdateChartRowCount(aMessageLevel);
            

            //    ugUserActivityExplorer.ActiveRow = ugUserActivityExplorer.Rows[0]; //set the active row to the first row at the top
        }
        private void UpdateChartRowCount(int aMessageLevel)
        {
            int row = 0;
            //Begin TT#595-MD -jsobek -Add Debug to My Activity level 
            switch (aMessageLevel)
            {
                case messageLevelSevere:
                    row = 5;
                    break;
                case messageLevelError:
                    row = 4;
                    break;
                case messageLevelWarning:
                    row = 3;
                    break;
                case messageLevelEdit:
                    row = 2;
                    break;
                case messageLevelInformation:
                    row = 1;
                    break;
                case messageLevelDebug:
                    row = 0;
                    break;
            }
            //End TT#595-MD -jsobek -Add Debug to My Activity level 
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[row]["Count"] = (int)((DataTable)this.ultraChart1.Data.DataSource).Rows[row]["Count"] + 1;
        }

        private void SetCurrentMessageLevel(string sLevel)
        {
            if (sLevel == "Debug") //TT#595-MD -jsobek -Add Debug to My Activity level  
                _currentMessageSelectedLevel = messageLevelDebug;
            else if (sLevel == "Information")
                _currentMessageSelectedLevel = messageLevelInformation;
            else if (sLevel == "Edit")
                _currentMessageSelectedLevel = messageLevelEdit;
            else if (sLevel == "Warning")
                _currentMessageSelectedLevel = messageLevelWarning;
            else if (sLevel == "Error")
                _currentMessageSelectedLevel = messageLevelError;
            else if (sLevel == "Severe")
                _currentMessageSelectedLevel = messageLevelSevere;

            RefreshMessageDataView();
        }




        private string _messageValidationMaxMessageLimitInvalidNumber = "Please enter a valid number for the max message limit.";
        public string messageValidationMaxMessageLimitInvalidNumber
        {
            get { return _messageValidationMaxMessageLimitInvalidNumber; }
            set { _messageValidationMaxMessageLimitInvalidNumber = value; }
        }
        private string _messageValidationMaxMessageLimitLowestValue = "Message limit cannot be less than 50.";
        public string messageValidationMaxMessageLimitLowestValue
        {
            get { return _messageValidationMaxMessageLimitLowestValue; }
            set { _messageValidationMaxMessageLimitLowestValue = value; }
        }

        private int _messageUpperLimit = 100000;
        public int messageUpperLimit
        {
            get { return _messageUpperLimit; }
            set 
            {
                _messageUpperLimit = value; 

                 //ensure the user is not over the limit
                Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Tools["messageTextMaxLimit"];
                int currentLimit;
                if (int.TryParse(t.Text, out currentLimit) == true)
                {
                    if (currentLimit > _messageUpperLimit)
                    {
                        t.Text = _messageUpperLimit.ToString();
                    }
                }
                
            }
        }

        private string _messageValidationMaxMessageLimitHighestValue = "Message limit cannot be greater than {0}.";
        public string messageValidationMaxMessageLimitHighestValue
        {
            get { return _messageValidationMaxMessageLimitHighestValue; }
            set { _messageValidationMaxMessageLimitHighestValue = value; }
        }
        private string _messageValidationMaxMessageLimitRestoringOriginalMessage = "Restoring original value.";
        public string messageValidationMaxMessageLimitRestoringOriginalMessage
        {
            get { return _messageValidationMaxMessageLimitRestoringOriginalMessage; }
            set { _messageValidationMaxMessageLimitRestoringOriginalMessage = value; }
        }
        private string _messageValidationMaxMessageLimitInvalidNumberTitle = "Invalid Max Message Limit";
        public string messageValidationMaxMessageLimitInvalidNumberTitle
        {
            get { return _messageValidationMaxMessageLimitInvalidNumberTitle; }
            set { _messageValidationMaxMessageLimitInvalidNumberTitle = value; }
        }

        private void SetMaxMessageLimit(string newLimitText, Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventArgs e)
        {
            int tempLimit;
            if (int.TryParse(newLimitText, out tempLimit) == false)
            {
                String sMsg = _messageValidationMaxMessageLimitInvalidNumber;

                if (e.ForceExit)
                {
                    e.RestoreOriginalValue = true;
                    sMsg += "\r\n" + _messageValidationMaxMessageLimitRestoringOriginalMessage;
                }
                else
                {
                    e.Cancel = true;
                }
                MessageBox.Show(sMsg, _messageValidationMaxMessageLimitInvalidNumberTitle);
                return;
                //MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Breaddown);
            }
            if (tempLimit < 50)
            {
                String sMsg = _messageValidationMaxMessageLimitLowestValue;
                if (e.ForceExit)
                {
                    e.RestoreOriginalValue = true;
                    sMsg += "\r\n" + _messageValidationMaxMessageLimitRestoringOriginalMessage;
                }
                else
                {
                    e.Cancel = true;
                }
                MessageBox.Show(sMsg, _messageValidationMaxMessageLimitInvalidNumberTitle);
                return;
            }
            if (tempLimit > _messageUpperLimit)
            {
                String sMsg = _messageValidationMaxMessageLimitHighestValue;
                if (e.ForceExit)
                {
                    e.RestoreOriginalValue = true;
                    sMsg += "\r\n" + _messageValidationMaxMessageLimitRestoringOriginalMessage;
                }
                else
                {
                    e.Cancel = true;
                }
                MessageBox.Show(sMsg, _messageValidationMaxMessageLimitInvalidNumberTitle);
                return;
            }
            _maxMessageLimit = tempLimit;
        }



        private string _messageValidationClearWarningPrompt = "This will permanently remove all messages.\r\n Are you sure you wish to continue??";
        public string messageValidationClearWarningPrompt
        {
            get { return _messageValidationClearWarningPrompt; }
            set { _messageValidationClearWarningPrompt = value; }
        }
        private string _messageValidationClearWarningPromptTitle = "Confirm:";
        public string messageValidationClearWarningPromptTitle
        {
            get { return _messageValidationClearWarningPromptTitle; }
            set { _messageValidationClearWarningPromptTitle = value; }
        }

        /// <summary>
        /// Deletes all messages and clears chart data
        /// </summary>
        private void MessagesClear()
        {
            if (MessageBox.Show(_messageValidationClearWarningPrompt, _messageValidationClearWarningPromptTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dsUserMessages.Tables[0].Rows.Clear();
                ClearChartData();
            }
        }

        private void ClearChartData()
        {
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[0]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[1]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[2]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[3]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[4]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[5]["Count"] = 0;
        }

        //private void ShowEmailForm(System.Net.Mail.Attachment a)
        //{
        //    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        //    EmailMessageForm frm = new EmailMessageForm();
        //    frm.AddAttachment(a);
        //    string subject = "My Activity";
        //    MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
        //    DataTable dt = secAdmin.GetUser(_SAB.ClientServerSession.UserRID);
        //    if (dt.Rows.Count > 0)
        //    {
        //        string userName = String.Empty;
        //        string userFullName = String.Empty;
        //        if (dt.Rows[0].IsNull("USER_NAME") == false)
        //        {
        //            userName = (string)dt.Rows[0]["USER_NAME"];
        //        }
        //        if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
        //        {
        //            userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
        //        }
        //        subject += " - " + userName + " (" + userFullName + ")";
        //    }
            
        //    frm.SetDefaults("", "", "", subject, "Please see attached file."); 
        //    frm.ShowDialog();

        //    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        //}

        #endregion



        #region "Grid"


        ///// <summary>
        ///// Finds all instances of a string in the grid, and highlights cells and rows where the text is found
        ///// </summary>
        ///// <param name="ug">The Infragistics.Win.UltraWinGrid.UltraGrid reference.</param>
        ///// <param name="sTextToFind">The string to search for.</</param>
        //public static void SearchGrid(Infragistics.Win.UltraWinGrid.UltraGrid ug, String sTextToFind)
        //{
        //    if (sTextToFind.Trim() == String.Empty)
        //    {
        //        return;
        //    }

        //    Infragistics.Win.UltraWinGrid.UltraGridRow firstrow = null;

        //    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in ug.Rows)
        //    {
        //        bool blnFoundInRow = false;
        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
        //        {
        //            if (cell.Text.ToLower().Contains(sTextToFind.ToLower()))
        //            {
        //                blnFoundInRow = true;
        //                if (firstrow == null)
        //                    firstrow = row;
        //            }
        //            else
        //            {
        //                cell.Appearance.BackColor = System.Drawing.Color.White;
        //            }
        //        }

        //        if (blnFoundInRow)
        //        {
        //            foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
        //            {
        //                if (cell.Text.ToLower().Contains(sTextToFind.ToLower()))
        //                {
        //                    cell.Appearance.BackColor = System.Drawing.Color.LightGoldenrodYellow;
        //                }
        //                else
        //                {
        //                    cell.Appearance.BackColor = System.Drawing.Color.BlanchedAlmond;
        //                }
        //            }
        //        }
        //    }

        //    if (firstrow != null)
        //    {
        //        ug.DisplayLayout.RowScrollRegions[0].FirstRow = firstrow;
        //    }
        //}

        ///// <summary>
        ///// Clears the search results in the grid.
        ///// </summary>
        ///// <param name="ug"></param>
        //public static void ClearGridSearchResults(Infragistics.Win.UltraWinGrid.UltraGrid ug)
        //{
        //    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in ug.Rows)
        //    {
        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
        //        {
        //            cell.Appearance.BackColor = System.Drawing.Color.White;
        //        }
        //    }
        //}
   
        #endregion



        //#region "Grid Export"

        /// <summary>
        /// Used to provide Excel Export Functionality for the Infragistics UltraWinGrid
        /// Just create an instance of this class, pass in your grid and call Export function.
        /// Saves in xls format - does not support xlsx format
        /// </summary>
        //private class UltraGridExcelExportWrapper
        //{
            //private Infragistics.Win.UltraWinGrid.UltraGrid _ug;
            //private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
            //private bool _checkForExportSelected = false;
            //private string _objectDescriptor;
            //public UltraGridExcelExportWrapper(Infragistics.Win.UltraWinGrid.UltraGrid ug, string objectDescriptor = "rows")
            //{
            //    _ug = ug;
            //    _objectDescriptor = objectDescriptor;

            //    this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter();
            //    this.ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(ultraGridExcelExporter1_RowExporting);
            //}

            //public void ExportAllRowsToExcel()
            //{
            //    string myFilepath = FindSavePath();
            //    string MessBoxText1 = "All " + _objectDescriptor + " sucessfully exported to \r\n";
            //    string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";
            //    try
            //    {
            //        if (myFilepath != null)
            //        {
            //            _checkForExportSelected = false;
            //            this.ultraGridExcelExporter1.Export(_ug, myFilepath);
            //            MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Rows.Count);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //public System.Net.Mail.Attachment ExportAllRowsToExcelAsAttachment()
            //{
            //    try
            //    {   
            //            _checkForExportSelected = false;
            //            return GetEmailAttachment();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //public System.Net.Mail.Attachment ExportSelectedRowsToExcelAsAttachment()
            //{
            //    try
            //    {

            //        _checkForExportSelected = true;
            //        return GetEmailAttachment();

            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //private System.Net.Mail.Attachment GetEmailAttachment()
            //{
            //    Infragistics.Documents.Excel.Workbook wb = new Infragistics.Documents.Excel.Workbook();
            //    this.ultraGridExcelExporter1.Export(_ug, wb);
                 
            //    //Infragistics does not save nicely directly to a memory stream, so saving as a file and reading it back into memory stream
            //    string fileName = System.IO.Path.GetTempPath() + "\\tempActivity_" + Data.EnvironmentInfo.MIDInfo.userName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") +  ".tmp";
            //    wb.Save(fileName); 
            //    byte[] b = System.IO.File.ReadAllBytes(fileName);
            //    System.IO.File.Delete(fileName);
            //    System.IO.MemoryStream streamAttachment = new System.IO.MemoryStream(b);

            //    System.Net.Mail.Attachment attachmentWorkbook;
            //    attachmentWorkbook = new System.Net.Mail.Attachment(streamAttachment, "MyActivity.xls");
            //    return attachmentWorkbook; 
            //}

            //public void ExportSelectedRowsToExcel()
            //{
            //    string myFilepath = FindSavePath();
            //    string MessBoxText1 = "Selected " + _objectDescriptor + " sucessfully exported to \r\n";
            //    string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";
            //    try
            //    {
            //        if (myFilepath != null)
            //        {
            //            _checkForExportSelected = true;
            //            this.ultraGridExcelExporter1.Export(_ug, myFilepath);
            //            MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Selected.Rows.Count);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //private void ultraGridExcelExporter1_RowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventArgs e)
            //{
            //    // The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
            //    //Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;

            //    //  Get the grid
            //    //Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

            //    // Get the real, on-screen row, from the export row. 
            //    Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

            //    // If the on-screen row is not selected, do not export it. 
            //    if (onScreenRow.Selected == false && _checkForExportSelected == true)
            //        e.Cancel = true;
            //}
            //private String FindSavePath()
            //{
            //    System.IO.Stream myStream;
            //    string myFilepath = null;
            //    try
            //    {
            //        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //        saveFileDialog1.Filter = "excel files (*.xls)|*.xls";
            //        saveFileDialog1.FilterIndex = 2;
            //        saveFileDialog1.RestoreDirectory = true;
            //        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //        {
            //            if ((myStream = saveFileDialog1.OpenFile()) != null)
            //            {
            //                myFilepath = saveFileDialog1.FileName;
            //                myStream.Close();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    return myFilepath;
            //}
        //}

        //#endregion



        #region "Chart"

        private const int CHART_STARTING_WIDTH = 335;
        private const int CHART_STARTING_HEIGHT = 150;

        //private void ShowOrHideChart(bool makeVisible)
        //{
           
        //}
        private void SetChartTitle()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTitleShow = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleShowHide"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTop = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationTop"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationLeft"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationRight"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationBottom"];

            this.ultraChart1.TitleTop.Visible = false;
            this.ultraChart1.TitleLeft.Visible = false;
            this.ultraChart1.TitleRight.Visible = false;
            this.ultraChart1.TitleBottom.Visible = false;

            if (sbTitleShow.Checked == true)
            {
                if (sbTop.Checked == true)
                {
                    this.ultraChart1.TitleTop.Visible = true;
                }
                else if (sbLeft.Checked == true)
                {
                    this.ultraChart1.TitleLeft.Visible = true;
                }
                else if (sbRight.Checked == true)
                {
                    this.ultraChart1.TitleRight.Visible = true;
                }
                else if (sbBottom.Checked == true)
                {
                    this.ultraChart1.TitleBottom.Visible = true;
                }
            }
        }
        private void SetChartLegend()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLegendShow = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartShowLegend"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTop = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendTop"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendLeft"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendRight"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendBottom"];

            this.ultraChart1.Legend.Visible = false;

            if (sbLegendShow.Checked == true)
            {
                 this.ultraChart1.Legend.Visible = true;
                if (sbTop.Checked == true)
                {
                      this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Top;
                }
                else if (sbLeft.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Left;
                }
                else if (sbRight.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Right;
                }
                else if (sbBottom.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Bottom;
                }
            }
        }
        private void SetChartVisibility()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sb = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartShowHide"];
            if (sb.Checked == true)
            {
                this.ultraChart1.Visible = true;
                this.chartSplitter1.Visible = true;
                if (this.ultraChart1.Height == 0)
                {
                    this.ultraChart1.Height = CHART_STARTING_HEIGHT;
                }
                if (this.ultraChart1.Width == 0)
                {
                    this.ultraChart1.Width = CHART_STARTING_WIDTH;
                }
            }
            else
            {
                this.ultraChart1.Visible = false;
                this.chartSplitter1.Visible = false;
            }
        }
        private void SetChartDocking()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbDockLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartDockLeft"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbDockRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartDockRight"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbDockBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartDockBottom"];

            if (sbDockLeft.Checked == true)
            {
                this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Left;
                this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Left;
            }
            else if (sbDockRight.Checked == true)
            {
                this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Right;
                this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            }
            else if (sbDockBottom.Checked == true)
            {
                this.ultraChart1.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.chartSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;              
            }
            this.ugUserActivityExplorer.BringToFront();

        }
        private void SetChartType()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBar = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeBar"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLine = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeLine"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbPie = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypePie"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbPyramid = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypePyramid"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbHistogram = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeHistogram"];


            this.ultraChart1.ResetAxis();
            if (sbBar.Checked == true)
            {
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart;
                this.ultraChart1.Data.SwapRowsAndColumns = true;
                this.ultraChart1.Axis.X.Labels.SeriesLabels.Visible = false;
            }
            else if (sbLine.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = false;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnLineChart;
                this.ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
                this.ultraChart1.Axis.X.Labels.ItemFormatString = "";
                this.ultraChart1.Axis.X2.Visible = false; 
            }
            else if (sbPyramid.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = false;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PyramidChart;
            }
            else if (sbHistogram.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = false;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.HistogramChart;
            }
            else //Use Pie Chart as default
            {
                this.ultraChart1.Data.SwapRowsAndColumns = false;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
            }
  
        }
        public void LoadChartSettings()
        {
            SetChartTitle();
            SetChartLegend();
            SetChartType();
            SetChartVisibility();
            SetChartDocking();
        }

        private void InitializeChart()
        {
            this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.ultraChart1.ColorModel.CustomPalette = new Color[] { Color.DarkGreen, Color.LightGreen, Color.Blue, Color.Yellow, Color.OrangeRed, Color.Red }; //TT#595-MD -jsobek -Add Debug to My Activity level 

            this.ultraChart1.TitleTop.Text = "My Activity";
            this.ultraChart1.TitleTop.HorizontalAlign = StringAlignment.Center;
            this.ultraChart1.TitleBottom.Text = "My Activity";
            this.ultraChart1.TitleBottom.HorizontalAlign = StringAlignment.Center;
            this.ultraChart1.TitleLeft.Text = "My Activity";
            this.ultraChart1.TitleRight.Text = "My Activity";

            this.ultraChart1.TitleTop.Visible = false;
            this.ultraChart1.TitleLeft.Visible = false;
            this.ultraChart1.TitleRight.Visible = false;
            this.ultraChart1.TitleBottom.Visible = false;
           

            DataTable dt = new DataTable();
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("Type", typeof(string));
            dt.Rows.Add(new object[] { 0, "Debug" }); //TT#595-MD -jsobek -Add Debug to My Activity level 
            dt.Rows.Add(new object[] { 0, "Info" });
            dt.Rows.Add(new object[] { 0, "Edit" });
            dt.Rows.Add(new object[] { 0, "Warning" });
            dt.Rows.Add(new object[] { 0, "Error" });
            dt.Rows.Add(new object[] { 0, "Severe" });
            this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
            this.ultraChart1.Data.RowLabelsColumn = 1;
            this.ultraChart1.Data.UseRowLabelsColumn = true;
            this.ultraChart1.Legend.Visible = true;
            this.ultraChart1.Data.DataSource = dt;
            this.ultraChart1.Data.DataBind();
        }

        private void ExportChart()
        {

            string PDF_File = FindPDFSavePath();
            if (PDF_File != null)
            {
                Infragistics.Documents.Reports.Report.Report r = new Infragistics.Documents.Reports.Report.Report();

                Graphics g = r.AddSection().AddCanvas().CreateGraphics();
                ultraChart1.RenderPdfFriendlyGraphics(g);

                r.Publish(PDF_File, Infragistics.Documents.Reports.Report.FileFormat.PDF);
            }

        }
        private String FindPDFSavePath()
        {
            System.IO.Stream myStream;
            string myFilepath = null;
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        myFilepath = saveFileDialog1.FileName;
                        myStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myFilepath;
        }

        #endregion

        // Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
        private void ugUserActivityExplorer_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.Both;
            e.Layout.Override.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
        }
        // End TT#3435 - JSmith - Errors messages not complete with encounter database errors.
   

   


    }
}
