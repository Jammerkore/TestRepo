using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml;
using System.Xml.XPath;     // TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
using System.Text;
using System.Reflection;
using System.Data.SqlClient; //TT#1205 - Verify version of sql server during install - APicchetti - 3/23/2011
using System.Runtime.InteropServices;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using Microsoft.Data.ConnectionUI;



namespace MIDRetail.DatabaseUpdate
{
	/// <summary>
	/// Summary description for frmDatabaseUpdate.
	/// </summary>
	public class frmDatabaseUpdate : System.Windows.Forms.Form
	{
        //// Begin TT#195 MD - JSmith - Add environment authentication
        //[DllImport("Wtsapi32.dll")] 
        //static extern bool WTSQuerySessionInformation( 
        //    System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned); 
 
        //public enum WTS_INFO_CLASS 
        //{ 
        //  WTSInitialProgram, 
        //  WTSApplicationName, 
        //  WTSWorkingDirectory, 
        //  WTSOEMId, 
        //  WTSSessionId, 
        //  WTSUserName, 
        //  WTSWinStationName, 
        //  WTSDomainName, 
        //  WTSConnectState, 
        //  WTSClientBuildNumber, 
        //  WTSClientName, 
        //  WTSClientDirectory, 
        //  WTSClientProductId, 
        //  WTSClientHardwareId, 
        //  WTSClientAddress, 
        //  WTSClientDisplay, 
        //  WTSClientProtocolType 
        //}
        //// End TT#195 MD

		SQLLocator SQLLocator;
		DatabaseUpdateInfo DatabaseUpdateInfo;
        ArrayList _alIncludedDatabases;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDatabase;
        private System.Windows.Forms.CheckBox cbxUpdateDatabase;
		private System.Windows.Forms.CheckedListBox cbxDatabases;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboServers;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox cbxLicenceKey;
		private System.Windows.Forms.ContextMenu mnuDatabases;
		private System.Windows.Forms.MenuItem mniClearAll;
		private System.Windows.Forms.MenuItem mniSelectAll;
		private System.Windows.Forms.ContextMenu mnuReport;
		private System.Windows.Forms.MenuItem menuItemCopy;
		private System.Windows.Forms.MenuItem menuItemCopyAll;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemPrint;
		private System.Windows.Forms.PrintDialog printDialog1;
		private System.Windows.Forms.MenuItem menuItemPrintAll;
		private System.Drawing.Printing.PrintDocument printDocument1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		// for printing
        private StringReader _stringToPrint;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.CheckBox cbxInstallNew;
		private System.Windows.Forms.CheckBox cbxIgnoreErrors;
		private System.Windows.Forms.TabPage tabMessages;
		private System.Windows.Forms.ListBox lstMessages;
		private System.Windows.Forms.TabPage tabProcessed;
		private System.Windows.Forms.ListBox lstProcessed;
		private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.GroupBox grpLogin;
		private System.Windows.Forms.GroupBox grpSelect;
		private System.Windows.Forms.Button btnKey;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripProgressBar prgInstall;
        private Button btnConnectionDialog;
        private Label label7;
        private Label label8;
        private TextBox txtServer;
        private TextBox txtDatabase;
        private Label label9;
		private Font _printFont;
        private string _connectionStringArg;	// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

        //  Begin TT#1468 - GRT - Notification Update
        #region Queues

        private Queue _messageErrorQueue;       
        public Queue MessageErrorQueue
        {
            get { return _messageErrorQueue; }
            set { _messageErrorQueue = value; }
        }
        private Queue _messageQueue;
        public Queue MessageQueue
        {
            get { return _messageQueue; }
            set { _messageQueue = value; }
        }
        private Queue _processedQueue;
        public Queue ProcessedQueue
        {
            get { return _processedQueue; }
            set { _processedQueue = value; }
        }
        private Queue _processedErrorQueue;
        public Queue ProcessedErrorQueue
        {
            get { return _processedErrorQueue; }
            set { _processedErrorQueue = value; }
        }        
        
        #endregion Queues
        #region Switches

        public bool LoadSuccessful
        {
            get
            {
                // Begin TT#195 MD - JSmith - Add environment authentication
                //return (bprocessDatabase && bProcessKeyed && bProcessList && bProcessScripts && bcheckloadComputations &&
                //  bbuildDatabases && bgeneratetableScript && bLoadLicenseKey);
                return (bValidConfiguration && bprocessDatabase && bProcessKeyed && bProcessList && bProcessScripts && bcheckloadComputations &&
                  bbuildDatabases && bgeneratetableScript && bLoadLicenseKey);
                // End TT#195 MD
            }
        }

        [DefaultValue(true)]
        private bool _bprocessList;
        public bool bProcessList
        {
            get { return _bprocessList; }
            set { _bprocessList = value; }
        }

        [DefaultValue(true)]
        private bool _bprocessKeyed;
        public bool bProcessKeyed
        {
            get { return _bprocessKeyed; }
            set { _bprocessKeyed = value; }
        }

        [DefaultValue(true)]
        private bool _bprocessScripts;
        public bool bProcessScripts
        {
            get { return _bprocessScripts; }
            set { _bprocessScripts = value; }
        }

        [DefaultValue(true)]
        private bool _bprocessDatabase;
        public bool bprocessDatabase
        {
            get { return _bprocessDatabase; }
            set { _bprocessDatabase = value; }
        }

        [DefaultValue(true)]
        private bool _bcheckloadComputations;
        public bool bcheckloadComputations
        {
            get { return _bcheckloadComputations; }
            set { _bcheckloadComputations = value; }
        }

        [DefaultValue(true)]
        private bool _bbuildDatabases;
        public bool bbuildDatabases
        {
            get { return _bbuildDatabases; }
            set { _bbuildDatabases = value; }
        }

        [DefaultValue(true)]
        private bool _bgeneratetableScript;
        public bool bgeneratetableScript
        {
            get { return _bgeneratetableScript; }
            set { _bgeneratetableScript = value; }
        }

        [DefaultValue(true)]
        private bool _bloadLicenseKey;
        public bool bLoadLicenseKey
        {
            get { return _bloadLicenseKey; }
            set { _bloadLicenseKey = value; }
        }

        // Begin TT#195 MD - JSmith - Add environment authentication
        [DefaultValue(true)]
        private bool _bvalidConfiguration;
        public bool bValidConfiguration
        {
            get { return _bvalidConfiguration; }
            set { _bvalidConfiguration = value; }
        }
        // End TT#195 MD

        //[DefaultValue(false)]
        //private bool _bverifysqlversion_Edition;
        //public bool bverifysqlversion_Edition
        //{
        //    get { return _bverifysqlversion_Edition; }
        //    set { _bverifysqlversion_Edition = value; }
        //}

        // Begin TT#74 MD - JSmith - One-button Upgrade
        [DefaultValue(false)]
        private bool _bIsOneClickUpgrade;
        public bool IsOneClickUpgrade
        {
            get { return _bIsOneClickUpgrade; }
            set { _bIsOneClickUpgrade = value; }
        }

        StreamWriter installLog;
        private TabPage tabQuery;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugResults;
        private RichTextBox rtbSQL;
        private CheckBox cbModify;
        private CheckBox cbxInstallNewROExtract;
        System.Security.Principal.WindowsIdentity currentUser = null;
        // End TT#74 MD

        #endregion Switches

        //  End TT#1468 - GRT - Notification Update

		// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        public frmDatabaseUpdate(string aConnectionString, bool oneClick, System.IO.StreamWriter aInstallLog)
        {
            try
            {
                string command;
                currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                installLog = aInstallLog;
                _connectionStringArg = aConnectionString;	

                SetLogMessage("********************************************************************", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("**                  Database Utility Started                      **", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("********************************************************************", eErrorType.message);


                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();

                MessageErrorQueue = new Queue();
                MessageQueue = new Queue();
                ProcessedErrorQueue = new Queue();
                ProcessedQueue = new Queue();

                if (oneClick)
                {
                    InitDatabaseUpgradeForOneClick(aConnectionString);
                }
                else
                {
                    InitDatabaseUpgrade(aConnectionString);
                }
            }
            catch
            {
                throw;
            }
        }
		// End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
		
        // Begin TT#1668 - JSmith - Install Log
		//public frmDatabaseUpdate()
		public frmDatabaseUpdate(System.IO.StreamWriter aInstallLog)
		// End TT#1668
		{
            try
            {
			    string command;
				// Begin TT#1668 - JSmith - Install Log
                currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                installLog = aInstallLog;
                _connectionStringArg = null;	// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

                SetLogMessage("********************************************************************", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("**                  Database Utility Started                      **", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("********************************************************************", eErrorType.message);
				// End TT#1668
			    //
			    // Required for Windows Form Designer support
			    //
			    InitializeComponent();

                InitDatabaseUpgrade(null);	// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
        // Begin TT#74 MD - JSmith - One-button Upgrade
        public frmDatabaseUpdate(string aConnectionString, System.IO.StreamWriter aInstallLog)
        {
            try
            {
                string command;
                currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                installLog = aInstallLog;
                _connectionStringArg = aConnectionString;	// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

                SetLogMessage("********************************************************************", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("**                  Database Utility Started                      **", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("********************************************************************", eErrorType.message);


                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();

                MessageErrorQueue = new Queue();
                MessageQueue = new Queue();
                ProcessedErrorQueue = new Queue();
                ProcessedQueue = new Queue();

                InitDatabaseUpgradeForOneClick(aConnectionString);	// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SetLogMessage("********************************************************************", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("**                   Database Utility Ended                       **", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("********************************************************************", eErrorType.message);
            }
        }

        // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        private void InitDatabaseUpgrade(string aConnectionString)
        {
            string command;
            //  BEGIN TT#1468 - GRT - setup the message queues
            //      seperate the error messages from the normal messages to improve parsing and readability.

            MessageErrorQueue = new Queue();
            MessageQueue = new Queue();
            ProcessedErrorQueue = new Queue();
            ProcessedQueue = new Queue();

            //  END TT#1468

            _separators = new char[System.Environment.NewLine.Length];
            for (int i = 0; i < System.Environment.NewLine.Length; i++)
            {
                _separators[i] = System.Environment.NewLine[i];
            }

            command = System.Environment.CommandLine;
            command = command.Trim('"');

                UpdateRoutines.SetProgressBar(lblStatus, prgInstall);
        }
        // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

		// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        private void InitDatabaseUpgradeForOneClick(string aConnectionString)
        {
            string command;
            strConnString = aConnectionString;
            IsOneClickUpgrade = true;

            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(aConnectionString);

            _separators = new char[System.Environment.NewLine.Length];
            for (int i = 0; i < System.Environment.NewLine.Length; i++)
            {
                _separators[i] = System.Environment.NewLine[i];
            }

            command = System.Environment.CommandLine;
            command = command.Trim('"');

                UpdateRoutines.SetProgressBar(lblStatus, prgInstall);

            cbxUpdateDatabase.Checked = true;
            bprocessDatabase = true;
            bProcessKeyed = true;
            bProcessList = true;
            bProcessScripts = true;
            bcheckloadComputations = true;
            bbuildDatabases = true;
            bgeneratetableScript = true;
            bLoadLicenseKey = true;

            // Begin TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
            //ProcessDatabase(builder.DataSource, builder.InitialCatalog, eDatabaseType.SQLServer2005, builder.UserID, builder.Password, false);
            eDatabaseType aDatabaseType;
            if (IsValidDatabase(aConnectionString, builder.InitialCatalog, out aDatabaseType))
            {
                ProcessDatabase(builder.DataSource, builder.InitialCatalog, aDatabaseType, builder.UserID, builder.Password, false);
            }
            else
            {
                bprocessDatabase = false;
            }
            // End TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
        }
		// End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        // End TT#74 MD

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatabaseUpdate));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.cbxDatabases = new System.Windows.Forms.CheckedListBox();
            this.mnuDatabases = new System.Windows.Forms.ContextMenu();
            this.mniClearAll = new System.Windows.Forms.MenuItem();
            this.mniSelectAll = new System.Windows.Forms.MenuItem();
            this.cboServers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpLogin = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnConnectionDialog = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbxInstallNewROExtract = new System.Windows.Forms.CheckBox();
            this.cbxUpdateDatabase = new System.Windows.Forms.CheckBox();
            this.cbxLicenceKey = new System.Windows.Forms.CheckBox();
            this.cbxIgnoreErrors = new System.Windows.Forms.CheckBox();
            this.cbxInstallNew = new System.Windows.Forms.CheckBox();
            this.tabMessages = new System.Windows.Forms.TabPage();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.mnuReport = new System.Windows.Forms.ContextMenu();
            this.menuItemCopy = new System.Windows.Forms.MenuItem();
            this.menuItemCopyAll = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemPrint = new System.Windows.Forms.MenuItem();
            this.menuItemPrintAll = new System.Windows.Forms.MenuItem();
            this.tabProcessed = new System.Windows.Forms.TabPage();
            this.lstProcessed = new System.Windows.Forms.ListBox();
            this.tabQuery = new System.Windows.Forms.TabPage();
            this.cbModify = new System.Windows.Forms.CheckBox();
            this.ugResults = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.rtbSQL = new System.Windows.Forms.RichTextBox();
            this.btnKey = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgInstall = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1.SuspendLayout();
            this.tabDatabase.SuspendLayout();
            this.grpSelect.SuspendLayout();
            this.grpLogin.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.tabMessages.SuspendLayout();
            this.tabProcessed.SuspendLayout();
            this.tabQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugResults)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabDatabase);
            this.tabControl1.Controls.Add(this.tabMessages);
            this.tabControl1.Controls.Add(this.tabProcessed);
            this.tabControl1.Controls.Add(this.tabQuery);
            this.tabControl1.Location = new System.Drawing.Point(8, 72);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(550, 247);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabDatabase
            // 
            this.tabDatabase.Controls.Add(this.grpSelect);
            this.tabDatabase.Controls.Add(this.grpLogin);
            this.tabDatabase.Controls.Add(this.grpOptions);
            this.tabDatabase.Location = new System.Drawing.Point(4, 22);
            this.tabDatabase.Name = "tabDatabase";
            this.tabDatabase.Size = new System.Drawing.Size(542, 221);
            this.tabDatabase.TabIndex = 0;
            this.tabDatabase.Text = "Database";
            // 
            // grpSelect
            // 
            this.grpSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpSelect.Controls.Add(this.cbxDatabases);
            this.grpSelect.Controls.Add(this.cboServers);
            this.grpSelect.Controls.Add(this.label1);
            this.grpSelect.Controls.Add(this.label2);
            this.grpSelect.Location = new System.Drawing.Point(536, 19);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(280, 199);
            this.grpSelect.TabIndex = 16;
            this.grpSelect.TabStop = false;
            this.grpSelect.Text = "Database(s)";
            this.grpSelect.Visible = false;
            // 
            // cbxDatabases
            // 
            this.cbxDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxDatabases.CheckOnClick = true;
            this.cbxDatabases.ContextMenu = this.mnuDatabases;
            this.cbxDatabases.Location = new System.Drawing.Point(16, 80);
            this.cbxDatabases.Name = "cbxDatabases";
            this.cbxDatabases.Size = new System.Drawing.Size(248, 109);
            this.cbxDatabases.TabIndex = 4;
            // 
            // mnuDatabases
            // 
            this.mnuDatabases.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniClearAll,
            this.mniSelectAll});
            // 
            // mniClearAll
            // 
            this.mniClearAll.Index = 0;
            this.mniClearAll.Text = "Clear All";
            this.mniClearAll.Click += new System.EventHandler(this.mniClearAll_Click);
            // 
            // mniSelectAll
            // 
            this.mniSelectAll.Index = 1;
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new System.EventHandler(this.mniSelectAll_Click);
            // 
            // cboServers
            // 
            this.cboServers.AutoAdjust = true;
            this.cboServers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboServers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboServers.DataSource = null;
            this.cboServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServers.DropDownWidth = 184;
            this.cboServers.FormattingEnabled = false;
            this.cboServers.IgnoreFocusLost = false;
            this.cboServers.ItemHeight = 13;
            this.cboServers.Location = new System.Drawing.Point(80, 24);
            this.cboServers.Margin = new System.Windows.Forms.Padding(0);
            this.cboServers.MaxDropDownItems = 25;
            this.cboServers.Name = "cboServers";
            this.cboServers.SetToolTip = "";
            this.cboServers.Size = new System.Drawing.Size(184, 21);
            this.cboServers.TabIndex = 3;
            this.cboServers.Tag = null;
            this.cboServers.SelectedIndexChanged += new System.EventHandler(this.cboServers_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Database(s)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpLogin
            // 
            this.grpLogin.Controls.Add(this.label9);
            this.grpLogin.Controls.Add(this.btnConnectionDialog);
            this.grpLogin.Controls.Add(this.label7);
            this.grpLogin.Controls.Add(this.label8);
            this.grpLogin.Controls.Add(this.txtServer);
            this.grpLogin.Controls.Add(this.txtDatabase);
            this.grpLogin.Controls.Add(this.label3);
            this.grpLogin.Controls.Add(this.label4);
            this.grpLogin.Controls.Add(this.txtUser);
            this.grpLogin.Controls.Add(this.txtPassword);
            this.grpLogin.Location = new System.Drawing.Point(8, 16);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(256, 192);
            this.grpLogin.TabIndex = 14;
            this.grpLogin.TabStop = false;
            this.grpLogin.Text = "Login Information";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(9, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 23);
            this.label9.TabIndex = 19;
            this.label9.Text = "SQL Server";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnConnectionDialog
            // 
            this.btnConnectionDialog.Location = new System.Drawing.Point(86, 20);
            this.btnConnectionDialog.Name = "btnConnectionDialog";
            this.btnConnectionDialog.Size = new System.Drawing.Size(147, 23);
            this.btnConnectionDialog.TabIndex = 18;
            this.btnConnectionDialog.Text = "Build Connection...";
            this.btnConnectionDialog.UseVisualStyleBackColor = true;
            this.btnConnectionDialog.Click += new System.EventHandler(this.btnConnectionDialog_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 23);
            this.label7.TabIndex = 10;
            this.label7.Text = "Database";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 23);
            this.label8.TabIndex = 9;
            this.label8.Text = "Server";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtServer
            // 
            this.txtServer.Enabled = false;
            this.txtServer.Location = new System.Drawing.Point(86, 119);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(147, 20);
            this.txtServer.TabIndex = 7;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Enabled = false;
            this.txtDatabase.Location = new System.Drawing.Point(86, 151);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(147, 20);
            this.txtDatabase.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "User";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Password";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(86, 55);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(147, 20);
            this.txtUser.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(86, 87);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(147, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbxInstallNewROExtract);
            this.grpOptions.Controls.Add(this.cbxUpdateDatabase);
            this.grpOptions.Controls.Add(this.cbxLicenceKey);
            this.grpOptions.Controls.Add(this.cbxIgnoreErrors);
            this.grpOptions.Controls.Add(this.cbxInstallNew);
            this.grpOptions.Location = new System.Drawing.Point(274, 16);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(256, 192);
            this.grpOptions.TabIndex = 13;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbxInstallNewROExtract
            // 
            this.cbxInstallNewROExtract.Location = new System.Drawing.Point(16, 121);
            this.cbxInstallNewROExtract.Name = "cbxInstallNewROExtract";
            this.cbxInstallNewROExtract.Size = new System.Drawing.Size(234, 24);
            this.cbxInstallNewROExtract.TabIndex = 13;
            this.cbxInstallNewROExtract.Text = "Setup New RO Extract Database";
            this.cbxInstallNewROExtract.CheckedChanged += new System.EventHandler(this.cbxInstallNewROExtract_CheckedChanged);
            // 
            // cbxUpdateDatabase
            // 
            this.cbxUpdateDatabase.Checked = true;
            this.cbxUpdateDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxUpdateDatabase.Location = new System.Drawing.Point(16, 22);
            this.cbxUpdateDatabase.Name = "cbxUpdateDatabase";
            this.cbxUpdateDatabase.Size = new System.Drawing.Size(120, 24);
            this.cbxUpdateDatabase.TabIndex = 7;
            this.cbxUpdateDatabase.Text = "Update Database";
            this.cbxUpdateDatabase.CheckedChanged += new System.EventHandler(this.cbxUpdateDatabase_CheckedChanged);
            // 
            // cbxLicenceKey
            // 
            this.cbxLicenceKey.Location = new System.Drawing.Point(16, 85);
            this.cbxLicenceKey.Name = "cbxLicenceKey";
            this.cbxLicenceKey.Size = new System.Drawing.Size(128, 24);
            this.cbxLicenceKey.TabIndex = 9;
            this.cbxLicenceKey.Text = "Install License Key";
            // 
            // cbxIgnoreErrors
            // 
            this.cbxIgnoreErrors.Location = new System.Drawing.Point(16, 151);
            this.cbxIgnoreErrors.Name = "cbxIgnoreErrors";
            this.cbxIgnoreErrors.Size = new System.Drawing.Size(96, 24);
            this.cbxIgnoreErrors.TabIndex = 12;
            this.cbxIgnoreErrors.Text = "Ignore Errors";
            // 
            // cbxInstallNew
            // 
            this.cbxInstallNew.Location = new System.Drawing.Point(16, 52);
            this.cbxInstallNew.Name = "cbxInstallNew";
            this.cbxInstallNew.Size = new System.Drawing.Size(152, 24);
            this.cbxInstallNew.TabIndex = 11;
            this.cbxInstallNew.Text = "Setup New Database";
            this.cbxInstallNew.CheckedChanged += new System.EventHandler(this.cbxInstallNew_CheckedChanged);
            // 
            // tabMessages
            // 
            this.tabMessages.Controls.Add(this.lstMessages);
            this.tabMessages.Location = new System.Drawing.Point(4, 22);
            this.tabMessages.Name = "tabMessages";
            this.tabMessages.Size = new System.Drawing.Size(542, 221);
            this.tabMessages.TabIndex = 1;
            this.tabMessages.Text = "Messages";
            this.tabMessages.Visible = false;
            // 
            // lstMessages
            // 
            this.lstMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMessages.ContextMenu = this.mnuReport;
            this.lstMessages.HorizontalScrollbar = true;
            this.lstMessages.Location = new System.Drawing.Point(8, 8);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMessages.Size = new System.Drawing.Size(526, 186);
            this.lstMessages.TabIndex = 0;
            this.lstMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstMessages_KeyDown);
            // 
            // mnuReport
            // 
            this.mnuReport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopy,
            this.menuItemCopyAll,
            this.menuItem2,
            this.menuItemPrint,
            this.menuItemPrintAll});
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Index = 0;
            this.menuItemCopy.Text = "&Copy";
            this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
            // 
            // menuItemCopyAll
            // 
            this.menuItemCopyAll.Index = 1;
            this.menuItemCopyAll.Text = "Copy &All";
            this.menuItemCopyAll.Click += new System.EventHandler(this.menuItemCopyAll_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "-";
            // 
            // menuItemPrint
            // 
            this.menuItemPrint.Index = 3;
            this.menuItemPrint.Text = "&Print";
            this.menuItemPrint.Click += new System.EventHandler(this.menuItemPrint_Click);
            // 
            // menuItemPrintAll
            // 
            this.menuItemPrintAll.Index = 4;
            this.menuItemPrintAll.Text = "P&rint All";
            this.menuItemPrintAll.Click += new System.EventHandler(this.menuItemPrintAll_Click);
            // 
            // tabProcessed
            // 
            this.tabProcessed.Controls.Add(this.lstProcessed);
            this.tabProcessed.Location = new System.Drawing.Point(4, 22);
            this.tabProcessed.Name = "tabProcessed";
            this.tabProcessed.Size = new System.Drawing.Size(542, 221);
            this.tabProcessed.TabIndex = 2;
            this.tabProcessed.Text = "Processed";
            // 
            // lstProcessed
            // 
            this.lstProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstProcessed.ContextMenu = this.mnuReport;
            this.lstProcessed.HorizontalScrollbar = true;
            this.lstProcessed.Location = new System.Drawing.Point(8, 8);
            this.lstProcessed.Name = "lstProcessed";
            this.lstProcessed.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstProcessed.Size = new System.Drawing.Size(526, 186);
            this.lstProcessed.TabIndex = 1;
            this.lstProcessed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstProcessed_KeyDown);
            // 
            // tabQuery
            // 
            this.tabQuery.Controls.Add(this.cbModify);
            this.tabQuery.Controls.Add(this.ugResults);
            this.tabQuery.Controls.Add(this.rtbSQL);
            this.tabQuery.Location = new System.Drawing.Point(4, 22);
            this.tabQuery.Name = "tabQuery";
            this.tabQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabQuery.Size = new System.Drawing.Size(542, 221);
            this.tabQuery.TabIndex = 3;
            this.tabQuery.Text = "Query";
            this.tabQuery.UseVisualStyleBackColor = true;
            // 
            // cbModify
            // 
            this.cbModify.AutoSize = true;
            this.cbModify.Location = new System.Drawing.Point(8, 97);
            this.cbModify.Name = "cbModify";
            this.cbModify.Size = new System.Drawing.Size(57, 17);
            this.cbModify.TabIndex = 18;
            this.cbModify.Text = "Modify";
            this.cbModify.UseVisualStyleBackColor = true;
            // 
            // ugResults
            // 
            this.ugResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugResults.Location = new System.Drawing.Point(7, 117);
            this.ugResults.Name = "ugResults";
            this.ugResults.Size = new System.Drawing.Size(530, 97);
            this.ugResults.TabIndex = 2;
            this.ugResults.Text = "Query Results";
            this.ugResults.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugResults_InitializeLayout);
            // 
            // rtbSQL
            // 
            this.rtbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSQL.Location = new System.Drawing.Point(7, 6);
            this.rtbSQL.Name = "rtbSQL";
            this.rtbSQL.Size = new System.Drawing.Size(529, 88);
            this.rtbSQL.TabIndex = 1;
            this.rtbSQL.Text = "";
            this.rtbSQL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbSQL_KeyDown);
            this.rtbSQL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtbSQL_MouseUp);
            // 
            // btnKey
            // 
            this.btnKey.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnKey.Location = new System.Drawing.Point(12, 330);
            this.btnKey.Name = "btnKey";
            this.btnKey.Size = new System.Drawing.Size(65, 23);
            this.btnKey.TabIndex = 16;
            this.btnKey.Text = "Key";
            this.btnKey.Click += new System.EventHandler(this.btnKey_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(467, 330);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(158, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 24);
            this.label6.TabIndex = 13;
            this.label6.Text = "Database Utility";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProcess
            // 
            this.btnProcess.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnProcess.BackColor = System.Drawing.SystemColors.Control;
            this.btnProcess.Location = new System.Drawing.Point(381, 330);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(80, 23);
            this.btnProcess.TabIndex = 11;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(158, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(256, 24);
            this.label5.TabIndex = 15;
            this.label5.Text = "Logility - RO";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.prgInstall});
            this.statusStrip.Location = new System.Drawing.Point(0, 372);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(564, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 17;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(450, 17);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prgInstall
            // 
            this.prgInstall.Name = "prgInstall";
            this.prgInstall.Size = new System.Drawing.Size(125, 16);
            // 
            // frmDatabaseUpdate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(564, 394);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnKey);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.label5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(580, 432);
            this.Name = "frmDatabaseUpdate";
            this.Text = "Logility - RO";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDatabaseUpdate_Closing);
            this.Load += new System.EventHandler(this.DatabaseUpdate_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabDatabase.ResumeLayout(false);
            this.grpSelect.ResumeLayout(false);
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.tabMessages.ResumeLayout(false);
            this.tabProcessed.ResumeLayout(false);
            this.tabQuery.ResumeLayout(false);
            this.tabQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugResults)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private char[] _separators;

        //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
        private string strConnString = "";
        private string strMasterConnString = "";
        private string strServer = "";
        private string strDatabase = "";
        private string strUserID = "";
        private string strPassword = "";
        //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

		protected void HandleException(Exception exc)
		{
			string message = "";

			while (MessageQueue.Count > 0)
			{
				message += (string)MessageQueue.Dequeue() + System.Environment.NewLine;
			}

			message += exc.ToString();

			MessageBox.Show(message, "Database Update", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
		}

		private void DatabaseUpdate_Load(object sender, System.EventArgs e)
		{
            string[] sIncluded;
            ArrayList alIncludedServers;
            bool bAddServer;
			try
			{
                bprocessDatabase = true;
                bProcessKeyed = true;
                bProcessList = true;
                bProcessScripts = true;
                bcheckloadComputations = true;
                bbuildDatabases = true;
                bgeneratetableScript =true;
                bLoadLicenseKey = true;

                alIncludedServers = new ArrayList();
                _alIncludedDatabases = new ArrayList();
				bool allowIgnoreErrors = false;
				string strParm = MIDConfigurationManager.AppSettings["AllowIgnoreErrors"];
				if (strParm != null)
				{
					try
					{
						allowIgnoreErrors = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}
#if (DEBUG)
			{
				allowIgnoreErrors = true;
			}
#endif
				if (allowIgnoreErrors)
				{
					cbxIgnoreErrors.Visible = true;
				}
				else
				{
					cbxIgnoreErrors.Visible = false;
				}

                strParm = MIDConfigurationManager.AppSettings["IncludeServers"];
                if (strParm != null)
                {
                    try
                    {
                        sIncluded = MIDstringTools.Split(strParm, ',', true);
                        for (int i = 0; i < sIncluded.Length; i++)
                        {
                            if (sIncluded[i] != null)
                            {
                                alIncludedServers.Add(sIncluded[i].ToLower());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                strParm = MIDConfigurationManager.AppSettings["IncludeDatabases"];
                if (strParm != null)
                {
                    try
                    {
                        sIncluded = MIDstringTools.Split(strParm, ',', true);
                        for (int i = 0; i < sIncluded.Length; i++)
                        {
                            if (sIncluded[i] != null)
                            {
                                _alIncludedDatabases.Add(sIncluded[i].ToLower());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

				DatabaseUpdateInfo = new DatabaseUpdateInfo();

				txtUser.Text = DatabaseUpdateInfo.User;
				cboServers.Items.Clear();
				lstMessages.Items.Clear();
				lstProcessed.Items.Clear();
			}
			catch
			{
				throw;
			}

            grpSelect.Visible = false;
            btnKey.Visible = Convert.ToBoolean(MIDConfigurationManager.AppSettings["AllowUserKeying"]);
            txtServer.Text = "";
            txtUser.Text = "";
            txtPassword.Text = "";
            txtDatabase.Text = "";
            btnProcess.Enabled = false;
            EnableQueryTab(false);	// TT#3781 - stodd - add query tab to Database Utilty - 
            //btnExecute.Enabled = false;
            // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            string strConnString = PopulateDataConnection();
            // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            if (txtDatabase.Text.Trim().Length > 0)
            {
                bool isMIDDatabase = UpdateRoutines.IsMIDDatabase(MessageQueue, strConnString);
                if (isMIDDatabase)
                {
                    cbxInstallNew.Enabled = false;
                    cbxInstallNewROExtract.Enabled = false;
                }
                else
                {
                    bool isROExtractDatabase = UpdateRoutines.IsROExtractDatabase(MessageQueue, strConnString);
                    if (isROExtractDatabase)
                    {
                        cbxInstallNew.Enabled = false;
                        cbxInstallNewROExtract.Enabled = false;
                    }
                }
            }
        }

		private void btnProcess_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			// Begin TT#3781 - stodd - add query tab to Database Utilty - 
            if (tabControl1.SelectedIndex == 3)
            {
                try
                {
                    ExecuteSql();
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else
			// End TT#3781 - stodd - add query tab to Database Utilty - 
            {
            //begin TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                //Confirm that connection information has been entered
                bool blConnectInfo = true;
                if ((txtServer.Text.Trim() == "") ||
                    (txtDatabase.Text.Trim() == "") ||
                    (txtUser.Text.Trim() == "") ||
                    (txtPassword.Text.Trim() == ""))
                {
                    blConnectInfo = false;
                }

                //Confirm that there is an option chosen
                bool blOptions = false;
            if ((cbxUpdateDatabase.Checked == true) ||
                    (cbxLicenceKey.Checked == true) ||
                    (cbxInstallNew.Checked == true) ||
                    (cbxInstallNewROExtract.Checked == true)
                    )
                {
                    blOptions = true;
                }

                if (blConnectInfo == true)
                {
                    if (blOptions == true)
                    {

                        // end TT#1130
                        try
                        {
                            if (txtUser.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("User is required", "DatabaseUpdate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                return;
                            }
                            if (txtPassword.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("Password is required", "DatabaseUpdate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                return;
                            }

                            //UpdateRoutines.SetMaxDegreeOfParallelismToOne(MessageQueue, strMasterConnString);  // TT#4318 - JSmith - Remove setting max degree of parallelism during database upgrade

                            if (grpSelect.Visible == true)
                            {
                                processList();
                            }
                            else
                            {
                                processKeyed();
                            }

                            // switch to report tab
                            this.tabControl1.SelectedTab = this.tabMessages;
                        }
                        catch (Exception exc)
                        {
                            HandleException(exc);
                        }
                        finally
                        {
                            //UpdateRoutines.ResetMaxDegreeOfParallelism(MessageQueue, strMasterConnString);  // TT#4318 - JSmith - Remove setting max degree of parallelism during database upgrade
                            Cursor.Current = Cursors.Default;
                        }
                    }

                    //begin TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    else
                    {
                        Exception option_exc = new Exception("No database maintanence options have been chosen");
                        HandleException(option_exc);
                    }
                }
                else
                {
                    Exception option_exc = new Exception("The database connection information is not complete");
                    HandleException(option_exc);
                }
                //end TT#1130

                // Begin TT#757-MD - JSmith - Fails if click Process twice
                btnProcess.Enabled = false;
                // End TT#757-MD - JSmith - Fails if click Process twice
                EnableQueryTab(false);	// TT#3781 - stodd - add query tab to Database Utilty - 
            }
		}

		private void processList()
		{
			try
			{
				//Begin Track #4616 - JSmith - SQL Server 2005
				eDatabaseType databaseType = eDatabaseType.None;
				//End Track #4616

				if (cboServers.SelectedIndex < 0)
				{
					MessageBox.Show("Server is required", "DatabaseUpdate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					return;
				}
				string server = cboServers.Text;
				string message = string.Empty;

				bool ignoreErrors = false;
				if (cbxIgnoreErrors.Checked)
				{
					ignoreErrors = true;
				}

				//Begin Track #4616 - JSmith - SQL Server 2005
				if (cbxDatabases.Items.Count > 0)
				{
					// use the first database to check server type
					MIDListBoxItem mlbi = (MIDListBoxItem)cbxDatabases.Items[0];
					databaseType = UpdateRoutines.GetDatabaseType(MessageQueue, strConnString);
				}
				//End Track #4616

				for (int i = 0; i < cbxDatabases.Items.Count; i++)
				{
					if (cbxDatabases.GetItemChecked(i))
					{
						MIDListBoxItem mlbi = (MIDListBoxItem)cbxDatabases.Items[i];
						server += ";" + mlbi.Value;

                        SetStatusMessage("Processing database:" + mlbi.Value);

						//Begin Track #4616 - JSmith - SQL Server 2005
//						ProcessDatabase(cboServers.Text, mlbi.Value, txtUser.Text, txtPassword.Text, ignoreErrors);
						ProcessDatabase(cboServers.Text, mlbi.Value, databaseType, txtUser.Text, txtPassword.Text, ignoreErrors);
						//End Track #4616
                        SetStatusMessage("Completed processing database:" + mlbi.Value);
					}
				}

				// update user database information
				DatabaseUpdateInfo.UpdateDatabaseUpdateInfo(server);
                SetStatusMessage("All databases processed");
                bProcessList = true; // TT#1468 - GRT - Reporting incorrect completion status when error occurs
			}
			catch (Exception exc)
			{
                // Begin TT#1468 - GRT - Reporting incorrect completion status when error occurs
                bProcessList = false; 
                MessageErrorQueue.Enqueue(exc.ToString());
				//throw;
                // End TT#1468 - GRT - Reporting incorrect completion status when error occurs
			}
            finally
            {
                MessageQueue.Enqueue("Process List = " + bProcessList.ToString());
                //  Begin TT#1468 - GRT
                string sMessage = "Complete";
                if (LoadSuccessful)
                    sMessage += " - Successful";
                else
                    sMessage += " - Failed";

                SetStatusMessage(sMessage);
                //  End TT#1468 - GRT
            }
		}

		private void processKeyed()
		{
            try
            {
                //Begin Track #4616 - JSmith - SQL Server 2005
                eDatabaseType databaseType = eDatabaseType.None;
                //End Track #4616

                if (txtServer.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Server is required", "DatabaseUpdate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                if (txtDatabase.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Database is required", "DatabaseUpdate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }

                //string server = cboServers.Text;
                //string message = string.Empty;

                bool ignoreErrors = false;
                ignoreErrors = cbxIgnoreErrors.Checked;

                //Begin Track #4616 - JSmith - SQL Server 2005
                databaseType = UpdateRoutines.GetDatabaseType(MessageQueue, strConnString);
                //End Track #4616

                //Begin Track #4616 - JSmith - SQL Server 2005
                //				ProcessDatabase(txtServer.Text, txtDatabase.Text, txtUser.Text, txtPassword.Text, ignoreErrors);
                ProcessDatabase(txtServer.Text, txtDatabase.Text, databaseType, txtUser.Text, txtPassword.Text, ignoreErrors);
                //End Track #4616
                bProcessKeyed = true; // TT#1468 - GRT - Reporting incorrect completion status when error occurs
            }
            catch (Exception exc)
            {
                // Begin TT#1468 - GRT - Reporting incorrect completion status when error occurs
                bProcessKeyed = false; // TT#1468 - GRT - Reporting incorrect completion status when error occurs
                MessageErrorQueue.Enqueue(exc.ToString());
                //throw;
                // End TT#1468 - GRT - Reporting incorrect completion status when error occurs
            }
            finally
            {
                MessageQueue.Enqueue("Process Keyed = " + bProcessKeyed.ToString());
                //  Begin TT#1468 - GRT
                string sMessage = "Complete";
                if (LoadSuccessful)
                    sMessage += " - Successful";
                else
                    sMessage += " - Failed";

                SetStatusMessage(sMessage);
                //  End TT#1468 - GRT
            }
		}

		//Begin Track #4616 - JSmith - SQL Server 2005
//		private void ProcessDatabase(string aServer, string aDatabase, string aUser, string aPassword, bool aIgnoreErrors)
		private void ProcessDatabase(string aServer, string aDatabase, eDatabaseType aDatabaseType, string aUser, string aPassword, bool aIgnoreErrors)
		//End Track #4616
		{
            try
            {
                SetLogMessage("Database " + aDatabase + " on Database Server " + aServer + " will be upgraded logged on with user " + aUser, eErrorType.message);
                bool successful;
                bool isValidConfiguration;
                MIDRetail.DatabaseUpdate.UpdateRoutines.ProcessDatabase(MessageQueue, _processedQueue, cbxInstallNew.Checked, cbxInstallNewROExtract.Checked, cbxUpdateDatabase.Checked, true, IsOneClickUpgrade, cbxLicenceKey.Checked, strConnString, aDatabase, new UpdateRoutines.SetMessageDelegate(SetStatusMessage), new UpdateRoutines.SetMessageToInstallerLog(SetLogInformationalMessage), out successful, out isValidConfiguration);
                bValidConfiguration = isValidConfiguration;
                bprocessDatabase = successful;
                FillListBoxes(aDatabase);
                //bprocessDatabase = successful;
            }
            catch (Exception exc)
            {
                // Begin TT#1468 - GRT - Reporting incorrect completion status when error occurs
                MessageErrorQueue.Enqueue(exc.ToString());
                bprocessDatabase = false;
                throw;
                // End TT#1468 - GRT - Reporting incorrect completion status when error occurs
            }

//            //bool loadSuccessful; 
//            // Begin Track #6117 - JSmith - Database utility will not load computations only
//            bool computationsLoaded = false;
//            // End Track #6117
//            string key = string.Empty;
//            string fileName = string.Empty;
//            //int noDataTables = 10;
			
//            string strValue = string.Empty;
//            Queue tmpMessageQueue = null;
//            //// begin TT#173 Provide Database container for large data collections
//            //string weekArchiveFileGroup = "WEEKARCHIVE";
//            //string dayArchiveFileGroup = "DAYARCHIVE";
//            //// end TT#173 Provide Database container for large data collections

//            try
//            {
//                // Begin TT#1668 - JSmith - Install Log
//                SetLogMessage("Database " + aDatabase + " on Database Server " + aServer + " will be upgraded logged on with user " + aUser, eErrorType.message);
//                // End TT#1668

//                UpdateRoutines.SetRecoveryModelSimple(MessageQueue, strConnString, strDatabase);
//                UpdateRoutines.DisableTextConstraints(MessageQueue, strConnString);

//                //LoadSuccessful = true;  

              

//                string connectionString = strConnString;
                
//                MIDConnectionString.ConnectionString = connectionString;

//                bValidConfiguration = true;
//                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
//                string assemblyConfiguration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;
//                string databaseConfiguration = UpdateRoutines.GetConfiguration(MessageQueue, strConnString);
//                if (databaseConfiguration != null &&
//                    databaseConfiguration != assemblyConfiguration)
//                {
//                    string msg = "Installer configuration of " + assemblyConfiguration + " does not match database configuration of " + databaseConfiguration + ".";
//                    MessageQueue.Enqueue(msg);
//                    if (!IsOneClickUpgrade)
//                    {
//                        if (MessageBox.Show(msg + Environment.NewLine + "Do you want to continue with the upgrade?", "",
//                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                        {
//                            bValidConfiguration = false;
//                            MessageQueue.Enqueue("User cancelled upgrade.");
//                        }
//                    }
//                }


              
   

//                if (cbxInstallNew.Checked || cbxUpdateDatabase.Checked)
//                {
//                    //SetStatusMessage("Building Database Objects");
//                    //tmpMessageQueue = new Queue();
//                    bool successful;
//                    UpdateRoutines.PerformDatabaseMaintenance(MessageQueue, _processedQueue, cbxUpdateDatabase.Checked, strConnString, new UpdateRoutines.SetMessageDelegate(SetStatusMessage), out successful);


//                    //if (LoadSuccessful)
//                    //{
//                    //    SetStatusMessage("Loading Computations");
//                    //    if (cbxInstallNew.Checked)
//                    //    {
//                    //        fileName = Path.GetTempPath() + "\\database.sql";
//                    //        GenerateTableScript(fileName, true, true,
//                    //            noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                    //            noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                    //            weekArchiveFileGroup, dayArchiveFileGroup,
//                    //            aDatabaseType);
//                    //        key = "TableFile";
//                    //        tmpMessageQueue = new Queue();
//                    //        if (!UpdateRoutines.ProcessScripts(fileName, key, tmpMessageQueue, _processedQueue,
//                    //            false, false, strConnString, aDatabaseType, aIgnoreErrors,
//                    //            noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                    //            noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                    //            weekArchiveFileGroup, dayArchiveFileGroup))
//                    //        {
//                    //            bProcessScripts = false;
//                    //            // transfer messages to main queue
//                    //            while (tmpMessageQueue.Count > 0)
//                    //            {
//                    //                MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
//                    //            }
//                    //        }
//                    //    }
//                    //    else
//                    //    {
//                    //        computationsLoaded = true;
//                    //        bcheckloadComputations = CheckLoadComputations(aServer, aDatabase, aDatabaseType, aUser,
//                    //            aPassword, aIgnoreErrors, noDataTables, allocationFileGroup,
//                    //            forecastFileGroup, historyFileGroup,
//                    //            noHistoryFileGroup, dailyHistoryFileGroup,
//                    //            noDailyHistoryFileGroup, auditFileGroup,
//                    //            weekArchiveFileGroup,
//                    //            dayArchiveFileGroup);
//                    //    }
//                    //}

//                    //if (LoadSuccessful)
//                    //{
//                    //    tmpMessageQueue = new Queue();
//                    //    if (!UpdateRoutines.ProcessDBObjects(tmpMessageQueue, _processedQueue,
//                    //            cbxUpdateDatabase.Checked, strConnString, aDatabaseType, aIgnoreErrors,
//                    //            noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                    //            noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                    //            weekArchiveFileGroup, dayArchiveFileGroup, DatabaseObjectsSQLObjectPhase.PostGenerate))
//                    //    {
//                    //        bProcessScripts = false;
//                    //        while (tmpMessageQueue.Count > 0)
//                    //        {
//                    //            MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
//                    //        }
//                    //    }
//                    //}

//                    //if (LoadSuccessful)
//                    //{
//                    //    key = "TextFile";
//                    //    tmpMessageQueue = new Queue();
//                    //    fileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + MIDConfigurationManager.AppSettings[key];
//                    //    if (!UpdateRoutines.ProcessScripts(fileName, key, tmpMessageQueue, _processedQueue,
//                    //        false, false, strConnString, aDatabaseType, aIgnoreErrors,
//                    //        noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                    //        noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                    //        weekArchiveFileGroup, dayArchiveFileGroup))
//                    //    {
//                    //        bProcessScripts = false;
//                    //        // transfer messages to main queue
//                    //        while (tmpMessageQueue.Count > 0)
//                    //        {
//                    //            MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
//                    //        }
//                    //    }
//                    //}

//                    //if (LoadSuccessful)
//                    //{
//                    //    SetStatusMessage("Upgrading Database");
//                    //    key = "UpgradeFile";
//                    //    fileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + MIDConfigurationManager.AppSettings[key];
//                    //    tmpMessageQueue = new Queue();
//                    //    if (!UpdateRoutines.ProcessScripts(fileName, key, tmpMessageQueue, _processedQueue,
//                    //        cbxUpdateDatabase.Checked, false, strConnString, aDatabaseType, aIgnoreErrors,
//                    //        noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                    //        noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                    //        weekArchiveFileGroup, dayArchiveFileGroup))
//                    //    {
//                    //        bProcessScripts = false;
//                    //        while (tmpMessageQueue.Count > 0)
//                    //        {
//                    //            MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
//                    //        }
//                    //    }
//                    //}

//                    if (cbxInstallNew.Checked)
//                    {
//                        if (LoadSuccessful)
//                        {
//                            //key = "DefaultsFile";
//                            //tmpMessageQueue = new Queue();
//                            //fileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + MIDConfigurationManager.AppSettings[key];
//                            //if (!UpdateRoutines.ProcessScripts(fileName, key, tmpMessageQueue, _processedQueue,
//                            //    false, false, strConnString, aDatabaseType, aIgnoreErrors,
//                            //    noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                            //    noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                            //    weekArchiveFileGroup, dayArchiveFileGroup))
//                            //{
//                            //    bProcessScripts = false;
//                            //    // transfer messages to main queue
//                            //    while (tmpMessageQueue.Count > 0)
//                            //    {
//                            //        MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
//                            //    }
//                            //}
//                        }

//                        if (LoadSuccessful)
//                        {
//                            //key = "FillFactorFile";
//                            //tmpMessageQueue = new Queue();
//                            //fileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + MIDConfigurationManager.AppSettings[key];
//                            //if (!UpdateRoutines.ProcessScripts(fileName, key, tmpMessageQueue, _processedQueue,
//                            //    false, false, strConnString, aDatabaseType, aIgnoreErrors,
//                            //    noDataTables, allocationFileGroup, forecastFileGroup, historyFileGroup,
//                            //    noHistoryFileGroup, dailyHistoryFileGroup, noDailyHistoryFileGroup, auditFileGroup,
//                            //    weekArchiveFileGroup, dayArchiveFileGroup))
//                            //{
//                            //    bProcessScripts = false;
//                            //    // transfer messages to main queue
//                            //    while (tmpMessageQueue.Count > 0)
//                            //    {
//                            //        MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
//                            //    }
//                            //}
//                        }
//                    }

//                    if (LoadSuccessful == false)
//                    {
//                        MessageQueue.Enqueue("Database installation failed");
//                    }
//                    else
//                    {
//                        MessageQueue.Enqueue("Database installation was successful");
//                    }
//                }
                
//                if (LoadSuccessful) 
//                {
//                    if (cbxLicenceKey.Checked)
//                    {
//                        bLoadLicenseKey = LicenseKeyRoutines.LoadLicenseKey(MessageQueue, strConnString); 
//                    }
//                }

//                // Begin TT#195 MD - JSmith - Add environment authentication
//                if (LoadSuccessful) 
//                {

//                    string assemblyName = string.Empty;

//#if (DEBUG)
//                    assemblyName = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\Windows\bin\Debug\" + "MIDRetail.Windows.dll"; ;
//#else
//                    assemblyName = Directory.GetParent(Application.StartupPath).ToString().Trim() + @"\Installer\Install Files\Client\MIDRetail.Windows.dll";
//#endif

//                    try
//                    {
//                        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyName);
//                        string remoteComputerName = null;
//                        if (System.Windows.Forms.SystemInformation.TerminalServerSession)
//                        {
//                            remoteComputerName = GetTerminalServerClientNameWTSAPI();
//                        }

//                        if (UpdateRoutines.StampDatabase(MessageQueue, strConnString, strDatabase, fvi.FileVersion, remoteComputerName))
//                        {
//                            MessageQueue.Enqueue("Database stamping was successful");
//                        }
//                        else
//                        {
//                            bprocessDatabase = false;
//                            MessageQueue.Enqueue("Database stamping was not successful");
//                        }
//                    }
//                    catch
//                    {
//                        MessageQueue.Enqueue("Unable to stamp database using information from " + assemblyName);
//                    }

//                }
//                // End TT#195 MD

//                FillListBoxes(aDatabase);   
//                bprocessDatabase = true;
//            }
//            catch (Exception exc)
//            {
//                // Begin TT#1468 - GRT - Reporting incorrect completion status when error occurs
//                MessageErrorQueue.Enqueue(exc.ToString());
//                bprocessDatabase = false;
//                throw;
//                // End TT#1468 - GRT - Reporting incorrect completion status when error occurs
//            }
//            finally
//            {
//                UpdateRoutines.RestoreRecoveryModel(MessageQueue, strConnString, strDatabase);
//                UpdateRoutines.EnableTextConstraints(MessageQueue, strConnString);
//                if (LoadSuccessful)
//                {
//                    ProgressBarSetToMaximum();
//                }
//            }
		}

        //// Begin TT#195 MD - JSmith - Add environment authentication
        //private string GetTerminalServerClientNameWTSAPI()
        //{

        //    const int WTS_CURRENT_SERVER_HANDLE = -1;

        //    IntPtr buffer = IntPtr.Zero;
        //    uint bytesReturned;

        //    string strReturnValue = "";
        //    try
        //    {
        //        WTSQuerySessionInformation(IntPtr.Zero, WTS_CURRENT_SERVER_HANDLE, WTS_INFO_CLASS.WTSClientName, out buffer, out bytesReturned);
        //        strReturnValue = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(buffer);
        //    }

        //    finally
        //    {
        //        buffer = IntPtr.Zero;
        //    }

        //    return strReturnValue;
        //}
        //// End TT#195 MD

		// Begin Track #6016 - JSmith - Database Upgrade Fails
        //private bool CheckLoadComputations(string aServer, string aDatabase, eDatabaseType aDatabaseType, string aUser,
        //    string aPassword, bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
        //    string aForecastFileGroup, string aHistoryFileGroup,
        //    int aNoHistoryFileGroup, string aDailyHistoryFileGroup,
        //    int aNoDailyHistoryFileGroup, string aAuditFileGroup,
        //    string aWeekArchiveFileGroup, string aDayArchiveFileGroup)
        //{
        //    bool loadSuccessful;
        //    string key = string.Empty;
        //    string fileName = string.Empty;
        //    Queue tmpMessageQueue = null;
        //    try
        //    {
        //        loadSuccessful = true;
        //        if (!LoadRoutines.UpdateTables(MessageQueue, strConnString))
        //        {
        //            loadSuccessful = false;
        //            MessageQueue.Enqueue("Update Tables Failed.");
        //        }
        //        else
        //        {
        //            MessageQueue.Enqueue("Update Tables Succeeded.");
        //        }
        //        // rebuild the stored procedures if not a new install
        //        if (loadSuccessful && !cbxInstallNew.Checked)
        //        {
        //            fileName = Path.GetTempPath() + "\\database.sql";
        //            GenerateTableScript(fileName, false, true,
        //                aNoDataTables, aAllocationFileGroup, aForecastFileGroup, aHistoryFileGroup,
        //                aNoHistoryFileGroup, aDailyHistoryFileGroup, aNoDailyHistoryFileGroup, aAuditFileGroup,
        //                aWeekArchiveFileGroup, aDayArchiveFileGroup,
        //                aDatabaseType);

        //            SetStatusMessage("Rebuilding Stored Procedures");
        //            key = "TableFile";
        //            tmpMessageQueue = new Queue();
        //            if (!UpdateRoutines.ProcessScripts(fileName, key, tmpMessageQueue, _processedQueue,
        //                false, false, strConnString, aDatabaseType, aIgnoreErrors,
        //                aNoDataTables, aAllocationFileGroup, aForecastFileGroup, aHistoryFileGroup,
        //                aNoHistoryFileGroup, aDailyHistoryFileGroup, aNoDailyHistoryFileGroup, aAuditFileGroup,
        //                aWeekArchiveFileGroup, aDayArchiveFileGroup))
        //            {
        //                loadSuccessful = false;
        //                // transfer messages to main queue
        //                while (tmpMessageQueue.Count > 0)
        //                {
        //                    MessageQueue.Enqueue(tmpMessageQueue.Dequeue());
        //                }
        //                MessageQueue.Enqueue("Process Scripts Failed.");
        //            }
        //            else
        //            {
        //                MessageQueue.Enqueue("Process Scripts Succeeded.");
        //            }
        //        }
        //        if (loadSuccessful)
        //        {
        //            SetStatusMessage("Loading Computations");
        //            if (!LoadRoutines.LoadComputations(MessageQueue, aServer, aDatabase, aUser, aPassword, strConnString))
        //            {
        //                loadSuccessful = false;
        //                MessageQueue.Enqueue("Load Computations Failed.");
        //            }
        //            else
        //            {
        //                MessageQueue.Enqueue("Load Computations Succeeded.");
        //            }
        //            SetStatusMessage("Computations Executed.");
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        MessageErrorQueue.Enqueue(exc.ToString());
        //        return false;
        //    }
        //    return loadSuccessful;
        //}

        private void FillListBoxes(string aDatabaseName)
		{
			int count;
			string message;

			try
			{
				this.lstMessages.Items.Add(" ");
				this.lstMessages.Items.Add("===============================================");
				this.lstMessages.Items.Add(aDatabaseName);
				this.lstMessages.Items.Add("===============================================");
				this.lstMessages.Items.Add(" ");

                if (LoadSuccessful)
				{
					this.lstMessages.Items.Add("The database has been processed successfully.");
				}
				else
				{
					this.lstMessages.Items.Add("Processing of the database has failed.");
				}

				this.lstMessages.Items.Add(" "); 

				if (MessageQueue.Count > 0)
				{
					this.lstMessages.Items.Add("-----------------------------------------------");
					this.lstMessages.Items.Add("Messages");
					this.lstMessages.Items.Add("-----------------------------------------------");
					this.lstMessages.Items.Add(" ");

					count = 0;
					while (MessageQueue.Count > 0)
					{
						if (count > 0)
						{
							lstMessages.Items.Add("----");
						}

						message = (string)MessageQueue.Dequeue();
						string[] lines = message.Split(_separators);
						foreach (string line in lines)
						{
							if (line.Trim().Length > 0)
							{
								lstMessages.Items.Add(line);
							}
						}
						++count;
					}
				}

                this.lstMessages.Items.Add(" ");

                if (MessageErrorQueue.Count > 0)
                {
                    this.lstMessages.Items.Add("-----------------------------------------------");
                    this.lstMessages.Items.Add("Messages");
                    this.lstMessages.Items.Add("-----------------------------------------------");
                    this.lstMessages.Items.Add(" ");

                    count = 0;
                    while (MessageErrorQueue.Count > 0)
                    {
                        if (count > 0)
                        {
                            lstMessages.Items.Add("----");
                        }

                        message = (string)MessageErrorQueue.Dequeue();
                        string[] lines = message.Split(_separators);
                        foreach (string line in lines)
                        {
                            if (line.Trim().Length > 0)
                            {
                                lstMessages.Items.Add(line);
                            }
                        }
                        ++count;
                    }
                }

				this.lstProcessed.Items.Add(" ");
				this.lstProcessed.Items.Add("===============================================");
				this.lstProcessed.Items.Add("Processed in " + aDatabaseName);
				this.lstProcessed.Items.Add("===============================================");
				this.lstProcessed.Items.Add(" ");

                bool blItemsProcessed = false;
				if (_processedQueue.Count > 0)
				{
                    blItemsProcessed = true;
					count = 0;
					while (_processedQueue.Count > 0)
					{
						if (count > 0)
						{
							lstProcessed.Items.Add("----");
						}

						lstProcessed.Items.Add((string)_processedQueue.Dequeue());
						++count;
					}
				}

                foreach (string line in lstMessages.Items)
                {
                    SetLogMessage(line.Trim(), eErrorType.message);
                }

                if (blItemsProcessed)
                {
                    foreach (string line in lstProcessed.Items)
                    {
                        SetLogMessage(line.Trim(), eErrorType.message);
                    }
                }
                else
                {
                    SetLogMessage("No items processed for this database", eErrorType.message);
                }
			}
			catch
			{
				throw;
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void EnableManualEntry(bool Enable)
        {
            txtUser.Enabled = Enable;
            txtPassword.Enabled = Enable;
            txtServer.Enabled = Enable;
            txtDatabase.Enabled = Enable;

        }

		private void btnKey_Click(object sender, System.EventArgs e)
		{
			grpSelect.Visible = false;

            EnableManualEntry(true);

		}

		private void cboServers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (cboServers.SelectedIndex > -1)
				{
					BuildDatabases();
				}
			}
			catch
			{
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void BuildDatabases()
		{
            bool bAddDatabase;
			try
			{
				if (cboServers.Text.Trim().Length == 0 ||
					txtUser.Text.Trim().Length == 0 || 
					txtPassword.Text.Trim().Length == 0)
				{
					return;
				}
				bool restorePrevious = false;
				string strParm = MIDConfigurationManager.AppSettings["RestorePriorEntries"];
				if (strParm != null)
				{
					try
					{
						restorePrevious = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

				Cursor.Current = Cursors.WaitCursor;
				SortedList db = new SortedList();
				this.cbxDatabases.Items.Clear();
				string[] databases = SQLLocator.GetCatalogs(cboServers.Text, txtUser.Text, txtPassword.Text);
				foreach (string database in databases)
				{
					if (database.ToLower() == "master" ||
						database.ToLower() == "tempdb" ||
						database.ToLower() == "model" ||
						database.ToLower() == "msdb" ||
						database.ToLower() == "pubs" ||
						database.ToLower() == "northwind" ||
						database.ToLower() == "data_dictionary")
					{
						continue;
					}

                    bAddDatabase = true;
                    if (_alIncludedDatabases.Count > 0)
                    {
                        bAddDatabase = false;
                        foreach (string databaseMask in _alIncludedDatabases)
                        {
                            if (Microsoft.VisualBasic.CompilerServices.Operators.LikeString(database.ToLower(), databaseMask, Microsoft.VisualBasic.CompareMethod.Text))
                            {
                                bAddDatabase = true;
                                break;
                            }
                        }
                    }

                    if (!bAddDatabase)
                    {
                        continue;
                    }

					bool isMIDDatabase = UpdateRoutines.IsMIDDatabase(MessageQueue, strConnString);
					if (cbxInstallNew.Checked)
					{
						if (!isMIDDatabase)
						{
							db.Add(database, null);
						}
					}
					else
					{
						if (isMIDDatabase)
						{
							db.Add(database, null);
						}
					}
				}

				int count = 0;
				foreach (string database in db.Keys)
				{
					bool setChecked = false;
					if (restorePrevious)
					{
						setChecked = DatabaseUpdateInfo.IsDatabaseSelected(cboServers.Text, database);
					}
					cbxDatabases.Items.Add(new MIDListBoxItem(count, database, database), setChecked);
					++count;
				}

				if (cbxDatabases.Items.Count == 0)
				{
					MessageBox.Show("There are no valid databases for the selected options");
				}
			}
			catch
			{
			}
		}

		private void frmDatabaseUpdate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				DatabaseUpdateInfo.WriteDatabaseUpdateInfo(txtUser.Text, cboServers.Text);

                SetLogMessage("********************************************************************", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("**                   Database Utility Ended                       **", eErrorType.message);
                SetLogMessage("**                                                                **", eErrorType.message);
                SetLogMessage("********************************************************************", eErrorType.message);
			}
			catch
			{
			}
		}

		private void mniClearAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				for (int i = 0; i < cbxDatabases.Items.Count; i++)
				{
					cbxDatabases.SetItemChecked(i, false);
				}
			}
			catch
			{
				throw;
			}
		}

		private void mniSelectAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				for (int i = 0; i < cbxDatabases.Items.Count; i++)
				{
					cbxDatabases.SetItemChecked(i, true);
				}
			}
			catch
			{
				throw;
			}
		}

		private void menuItemPrint_Click(object sender, System.EventArgs e)
		{
			string text = GetReportSelectedLines(true);
			Print(text);
		}

		private void menuItemPrintAll_Click(object sender, System.EventArgs e)
		{
			if (this.tabControl1.SelectedIndex == 1)
				lstMessages_SelectAll();
			else if (this.tabControl1.SelectedIndex == 2)
				lstProcessed_SelectAll();

			string text = GetReportSelectedLines(true);
			Print(text);
		}	

		private void menuItemCopyAll_Click(object sender, System.EventArgs e)
		{
			if (this.tabControl1.SelectedIndex == 1)
				lstMessages_SelectAll();
			else if (this.tabControl1.SelectedIndex == 2)
				lstProcessed_SelectAll();

			string text = GetReportSelectedLines(false);
			Clipboard.SetDataObject(text ,true);
		}

		private void menuItemCopy_Click(object sender, System.EventArgs e)
		{
			string text = GetReportSelectedLines(false);
			Clipboard.SetDataObject(text ,true);
		}

		/// <summary>
		/// gathers all of the selected lines into a string.
		/// also adds new line and can do word wrapping for printing.
		/// </summary>
		/// <param name="wordWrap"></param>
		/// <returns></returns>
		private string GetReportSelectedLines(bool wordWrap)
		{
			try
			{
				ListBox listBox = null;
				if (this.tabControl1.SelectedIndex == 1)
					listBox = lstMessages;
				else if (this.tabControl1.SelectedIndex == 2)
					listBox = lstProcessed;

				int maxChar = 100;
				string selectedText = string.Empty;
				foreach (object aLineObj in listBox.SelectedItems)
				{
					string aLine = (string)aLineObj;
					if (wordWrap)
					{
						string newLine = string.Empty;
						string delimit = "\r\n";
						aLine = aLine.Replace(delimit, "");
						string [] words = aLine.Split(' ');
						foreach (string word in words)
						{
							int newLineLen = newLine.Length;
							int wordLen = word.Length;
							if (wordLen > maxChar)
							{
								selectedText += (word + System.Environment.NewLine);
							}
							else if (newLineLen + wordLen < maxChar)
							{
								newLine += word + " ";
							}
							else
							{
								selectedText += newLine + System.Environment.NewLine;
								newLine = word + " ";
							}
						}
						if (newLine.Length > 0)
						{
							selectedText += newLine + System.Environment.NewLine;
						}
					}
					else
					{
						selectedText += aLine + System.Environment.NewLine;
					}
				}
	
				return selectedText;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// selects all items in the report listbox.
		/// </summary>
		private void lstMessages_SelectAll()
		{
			for (int i=0;i<lstMessages.Items.Count;i++)
			{
				lstMessages.SetSelected(i,true);
			}
		}
		/// <summary>
		/// selects all items in the processed listbox.
		/// </summary>
		private void lstProcessed_SelectAll()
		{
			for (int i=0;i<lstProcessed.Items.Count;i++)
			{
				lstProcessed.SetSelected(i,true);
			}
		}
		/// <summary>
		/// catches ctrl-C and ctrl-A
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstMessages_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control) 
			{
				lstMessages_SelectAll();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control) 
			{
				string text = GetReportSelectedLines(false);
				Clipboard.SetDataObject(text ,true);
				e.Handled = true;
			}
		}

		
		private void lstProcessed_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control) 
			{
				lstProcessed_SelectAll();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control) 
			{
				string text = GetReportSelectedLines(false);
				Clipboard.SetDataObject(text ,true);
				e.Handled = true;
			}
		}
		
		private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			float linesPerPage = 0;
			float yPos = 0;
			int count = 0;
			float leftMargin = e.MarginBounds.Left;
			float topMargin = e.MarginBounds.Top;
			string line = null;

			// Calculate the number of lines per page.
			linesPerPage = e.MarginBounds.Height / 
				_printFont.GetHeight(e.Graphics);

			// Print each line of the file.
			while(count < linesPerPage && 
				((line=_stringToPrint.ReadLine()) != null)) 
			{
				yPos = topMargin + (count * 
					_printFont.GetHeight(e.Graphics));
				e.Graphics.DrawString(line, _printFont, Brushes.Black, 
					leftMargin, yPos, new StringFormat());
				count++;
			}

			// If more lines exist, print another page.
			if(line != null)
				e.HasMorePages = true;
			else
				e.HasMorePages = false;
		}

		private void Print(string text)
		{	
			_stringToPrint = new StringReader(text);

			try
			{
				_printFont = new Font("Lucida Sans", 9);
				printDialog1.AllowPrintToFile =true;
				printDocument1.DocumentName=lstMessages.Text;
				printDialog1.Document=printDocument1;			
				if(printDialog1.ShowDialog()==DialogResult.OK)
				{
					try
					{
						printDocument1.Print();
					}
					catch
					{
						MessageBox.Show ("Error While Printing", "Print Error");
					}
				}
			}
			catch
			{
					throw;
			}
			finally
			{
				_stringToPrint.Close();
			}
		}

		private void cbxUpdateDatabase_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (cbxUpdateDatabase.Checked)
				{
					cbxInstallNew.Checked = false;
					cbxInstallNew.Enabled = false;
					BuildDatabases();
				}
				else
				{
					cbxInstallNew.Enabled = true;
				}
			}
			catch
			{
				throw;
			}
		}

		private void cbxInstallNew_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (cbxInstallNew.Checked)
				{
					cbxUpdateDatabase.Checked = false;
					cbxUpdateDatabase.Enabled = false;
					cbxLicenceKey.Checked = true;
					cbxLicenceKey.Enabled = false;
					BuildDatabases();
				}
				else if (!cbxInstallNewROExtract.Checked)
				{
                    cbxUpdateDatabase.Enabled = true;
                    cbxLicenceKey.Enabled = true;

                    cbxUpdateDatabase.Checked = true;
                    cbxLicenceKey.Checked = false;
                    cbxIgnoreErrors.Enabled = true;
                    cbxIgnoreErrors.Checked = false;

					if (cbxUpdateDatabase.Checked ||
						cbxLicenceKey.Checked)
					{
						BuildDatabases();
					}
				}
			}
			catch
			{
				throw;
			}
		}

        //private void GenerateTableScript(string aFileName, bool aGenerateTables, bool aGenerateStoredProcedures,
        //    int aNoDataTables, string aAllocationFileGroup, string aForecastFileGroup, string aHistoryFileGroup,
        //    int aNoHistoryFileGroup, string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
        //    string aWeekArchiveFileGroup, string aDayArchiveFileGroup,  
        //    eDatabaseType aDatabaseType)
        //{
        //    try
        //    {
        //        DatabaseObjectGen dog = new DatabaseObjectGen(aDatabaseType);
        //        dog.GenFile(aFileName, aGenerateTables, aGenerateStoredProcedures,
        //            aNoDataTables, aAllocationFileGroup, aForecastFileGroup, aHistoryFileGroup,
        //            aNoHistoryFileGroup, aDailyHistoryFileGroup, aNoDailyHistoryFileGroup, aAuditFileGroup, aWeekArchiveFileGroup, aDayArchiveFileGroup);  
        //        bgeneratetableScript = true;
        //    }
        //    catch (Exception exc)
        //    {
        //        bgeneratetableScript = false;
        //        MessageErrorQueue.Enqueue(exc.ToString());
        //    }
        //}

        public void SetStatusMessage(string message)
        {
            lblStatus.Text = message;
            Application.DoEvents();
            if (message != null &&
                message.Trim().Length > 0)
            {
                SetLogMessage(message, eErrorType.message);
            }
        }
        public void SetLogInformationalMessage(string message)
        {
            SetLogMessage(message, eErrorType.message);
        }
        public void SetLogMessage(string message, eErrorType aErrorType)
        {
            string msgLevel = null;
            switch (aErrorType)
            {
                case eErrorType.error:
                    msgLevel = "  Error";
                    break;
                case eErrorType.message:
                    msgLevel = "Message";
                    break;
                case eErrorType.warning:
                    msgLevel = "Warning";
                    break;
                case eErrorType.debug:
                    msgLevel = "  Debug";
                    break;

            }

            if (installLog != null &&
                IsOpen(installLog))
            {
                installLog.WriteLine(DateTime.Now.ToString("s") + " - " + currentUser.Name + " - " + msgLevel + " - " + message);
            }
        }

        public static bool IsOpen(StreamWriter sw)
        {
            try
            {
                sw.BaseStream.Seek(0, SeekOrigin.Current);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void LbLStatusEnabled(bool Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                lblStatus.Enabled = Value;
            }
        }

        public void ProgressBarEnabled(bool Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Enabled = Value;
            }
        }

        public void ProgressBarIncrementValue(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Increment(Value);
            }
        }

        public void ProgressBarSetValue(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Value = Value;
            }
        }

        public void ProgressBarSetStep(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Step = Value;
            }
        }

        public void ProgressBarPerformStep()
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.PerformStep();
            }
        }

        public void ProgressBarSetMinimum(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Minimum = Value;
            }
        }

        public void ProgressBarSetMaximum(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Maximum = Value;
            }
        }

        public void ProgressBarSetToMaximum()
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                ProgressBarSetValue(prgInstall.Maximum);
            }
        }

        // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        private string PopulateDataConnection()
        {
            //disable entry into the connection controls so that the returned info is protected
            EnableManualEntry(false);
            DataConnectionDialog dCon = BuildDataConnectionDialog(true);	// TT#1346-MD - stodd - Database Upgrade does not handle a bad connection string correctly -
            DataConnectionDialog(dCon, true);
            return dCon.ConnectionString;
        }
        // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

        private void btnConnectionDialog_Click(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
            EnableQueryTab(false);	//TT#3781 - stodd - add query tab to Database Utilty - 

            //disable entry into the connection controls so that the returned info is protected
            EnableManualEntry(false);

            // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            DataConnectionDialog dCon = BuildDataConnectionDialog(false);	// TT#1346-MD - stodd - Database Upgrade does not handle a bad connection string correctly -
            // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

            //enter the datasource text to the text box
			// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            DataConnectionDialog(dCon, false);	 

#if (DEBUG)
            if (txtDatabase.Text.Trim().Length > 0)
            {
                btnProcess.Enabled = true;
            }
#endif
			// End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        }

		// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        private void DataConnectionDialog(DataConnectionDialog dCon, bool runSilent)
        {
            DialogResult dialogResult = DialogResult.OK;
            try
            {
                //=====================================================================================================
                // In "Silent" mode, the dialog is not shown. The dCon.ConnectionString has been filled in already.
                //=====================================================================================================
                //if (runSilent && dCon.ConnectionString != null && dCon.ConnectionString != string.Empty)
                if (runSilent)
                {
                    if (dCon.ConnectionString != null && dCon.ConnectionString != string.Empty)
                    {
                        dialogResult = DialogResult.OK;
                    }
                    else
                    {
                        // if connection string is null or empty, don't continue.
                        dialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
                else
                {
                    dialogResult = Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(dCon);
                }

                if (dialogResult == DialogResult.OK)
                {
                    //set global connection string
                    strConnString = dCon.ConnectionString.Trim();
                    if (strConnString.Contains("Integrated Security=True"))
                    {
                        Exception conn_exc = new Exception("You are not authorized to create a connection using Window's Authentication.");
                        HandleException(conn_exc);
                    }
                    else
                    {
                        //variables to build master connection string
                        string MasterSource = "";
                        string MasterInitialCatalog = "";
                        string MasterUserID = "";
                        string MasterPassword = "";

                    // Begin TT#1164-MD - JSmith - Database compatibility issue when installing
                    //parse connection string for display
                    //char[] connDelim = ";".ToCharArray();
                    //string[] connParts = strConnString.Split(connDelim);
                    //foreach (string connPart in connParts)
                    //{
                    //    if (connPart.StartsWith("Data Source=") == true)
                    //    {
                    //        txtServer.Text = connPart.Substring("Data Source=".Length);
                    //        MasterSource = txtServer.Text.Trim();
                    //        strServer = txtServer.Text.Trim();
                    //    }
                    //    else if (connPart.StartsWith("Initial Catalog=") == true)
                    //    {
                    //        txtDatabase.Text = connPart.Substring("Initial Catalog=".Length);
                    //        MasterInitialCatalog = txtDatabase.Text.Trim();
                    //    }
                    //    else if (connPart.StartsWith("User ID=") == true)
                    //    {
                    //        txtUser.Text = connPart.Substring("User ID=".Length);
                    //        MasterUserID = txtUser.Text.Trim();
                    //        strUserID = txtUser.Text.Trim();
                    //    }
                    //    else if (connPart.StartsWith("Password=") == true)
                    //    {
                    //        txtPassword.Text = connPart.Substring("Password=".Length);
                    //        MasterPassword = txtPassword.Text.Trim();
                    //        strPassword = txtPassword.Text.Trim();
                    //    }
                    //}

                    Microsoft.Data.ConnectionUI.SqlConnectionProperties connProp = new Microsoft.Data.ConnectionUI.SqlConnectionProperties();
                    connProp.ConnectionStringBuilder.ConnectionString = dCon.ConnectionString;

                    txtServer.Text = Convert.ToString(connProp.ConnectionStringBuilder["Data Source"]);
                    MasterSource = txtServer.Text.Trim();
                    strServer = txtServer.Text.Trim();

                    txtDatabase.Text = Convert.ToString(connProp.ConnectionStringBuilder["Initial Catalog"]);
                    MasterInitialCatalog = txtDatabase.Text.Trim();

                    txtUser.Text = Convert.ToString(connProp.ConnectionStringBuilder["User ID"]);
                    MasterUserID = txtUser.Text.Trim();
                    strUserID = txtUser.Text.Trim();

                    txtPassword.Text = Convert.ToString(connProp.ConnectionStringBuilder["Password"]);
                    MasterPassword = txtPassword.Text.Trim();
                    strPassword = txtPassword.Text.Trim();
                    // End TT#1164-MD - JSmith - Database compatibility issue when installing

                    //set global master connection string
                    strMasterConnString = "Data Source=" + MasterSource + ";Initial Catalog=master;User ID=" + MasterUserID + ";Password=" + MasterPassword;

                    //Verify the database version and edition
                    string productversion = "";
                    string productlevel = "";
                    string edition = "";
                    string server = "";
                    string database = "";
                    if (VerifySQLVersion_Edition(dCon.ConnectionString.Trim(), out productversion, out productlevel, out edition, out server, out database) != true)
                    {
                        MessageBox.Show(this, "The target database does not meet MID installation requirements: " + Environment.NewLine +
                            "   Server:       " + server + Environment.NewLine +
                            "   Database:     " + database + Environment.NewLine +
                            "   SQL Version:  " + productversion + Environment.NewLine +
                            "   SQL Level:    " + productlevel + Environment.NewLine +
                            "   SQL Edition:  " + edition + Environment.NewLine, "SQL Compatibility", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtServer.Text = "";
                        txtDatabase.Text = "";
                        txtUser.Text = "";
                        txtPassword.Text = "";
                    }

                    // Begin TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
                    UpdateRoutines.CompatibilityLevel compatibilityLevel = UpdateRoutines.GetCompatibilityLevel(MessageQueue, strConnString, txtDatabase.Text);
                    if (compatibilityLevel < UpdateRoutines.CompatibilityLevel.SQL2008)
                    {
                        MessageBox.Show(this, "The target database compatibility level does not meet MID installation requirements: " + Environment.NewLine +
                           "   Server:              " + server + Environment.NewLine +
                           "   Database:            " + database + Environment.NewLine +
                           "   Compatibility Level: " + compatibilityLevel + Environment.NewLine +
                           "   SQL Version:         " + productversion + Environment.NewLine +
                           "   SQL Level:           " + productlevel + Environment.NewLine +
                           "   SQL Edition:         " + edition + Environment.NewLine, "SQL Compatibility", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        txtServer.Text = "";
                        txtDatabase.Text = "";
                        txtUser.Text = "";
                        txtPassword.Text = "";
                    }
                    // End TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process

                    if (!UpdateRoutines.IsDatabaseCaseSensitive(MessageQueue, strConnString))
                    {
                        MessageBox.Show(this, "The target database is not case sensitive does not meet MID installation requirements: " + Environment.NewLine +
                           "   Server:       " + server + Environment.NewLine +
                           "   Database:     " + database + Environment.NewLine +
                           "   Collation:    " + UpdateRoutines.GetCollation(MessageQueue, strConnString, txtDatabase.Text) + Environment.NewLine + 
                           "   SQL Version:  " + productversion + Environment.NewLine +
                           "   SQL Level:    " + productlevel + Environment.NewLine +
                           "   SQL Edition:  " + edition + Environment.NewLine, "SQL Compatibility", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        

                        txtServer.Text = "";
                        txtDatabase.Text = "";
                        txtUser.Text = "";
                        txtPassword.Text = "";
                    }

                    string db = txtDatabase.Text.ToLower();
                    if (db == "master" || db == "model" || db == "msdb" || db == "tempdb")
                    {
                        MessageBox.Show(this, "Changes may NOT be applied to any SQL Server System Database: " + Environment.NewLine +
                           "   Server:       " + server + Environment.NewLine +
                           "   Database:     " + database + Environment.NewLine +
                           "   Collation:    " + UpdateRoutines.GetCollation(MessageQueue, strConnString, txtDatabase.Text) + Environment.NewLine +
                           "   SQL Version:  " + productversion + Environment.NewLine +
                           "   SQL Level:    " + productlevel + Environment.NewLine +
                           "   SQL Edition:  " + edition + Environment.NewLine, "SQL Compatibility", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        txtServer.Text = "";
                        txtDatabase.Text = "";
                        txtUser.Text = "";
                        txtPassword.Text = "";
                    }

                    bool isMIDDatabase = UpdateRoutines.IsMIDDatabase(MessageQueue, strConnString);
                    bool isROExtractDatabase = UpdateRoutines.IsROExtractDatabase(MessageQueue, strConnString);

                    if (!isMIDDatabase
                        && !isROExtractDatabase)
                    {
                        cbxInstallNew.Checked = true;
                        cbxInstallNewROExtract.Enabled = true;
                    }
                    else
                    {
                            if (cbxInstallNew.Checked)
                            {
                                cbxInstallNew.Checked = false;
                            }
                            if (cbxInstallNewROExtract.Checked)
                            {
                                cbxInstallNewROExtract.Checked = false;
                            }
                        }

                    //End TT#1955 - DOConnell - Database Utility changes options incorrectly based on Build Connection setting
                    // Begin TT#1343
                    // Begin TT#757-MD - JSmith - Fails if click Process twice
                        if (txtDatabase.Text.Trim().Length > 0)
                        {
                            btnProcess.Enabled = true;
							EnableQueryTab(true);	// TT#3781 - stodd - add query tab to Database Utilty - 
                        }
                    // End TT#757-MD - JSmith - Fails if click Process twice
                    }
                }
            //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
            }
            catch
            {
                throw;
            }
        }
		// End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

        // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        private DataConnectionDialog BuildDataConnectionDialog(bool runSilent)	// TT#1346-MD - stodd - Database Upgrade does not handle a bad connection string correctly -
        {
            //invoke the data connection dialog
            DataConnectionDialog dCon = new DataConnectionDialog();
            
            
            //add the datasource to the dialog
            DataSource.AddStandardDataSources(dCon);

            //choose the sql datasource
            dCon.SelectedDataSource = DataSource.SqlDataSource;
            dCon.SelectedDataProvider = DataProvider.SqlDataProvider;

            //position the dialog box
            dCon.StartPosition = FormStartPosition.CenterScreen;

            //control the dialog to only allow sql connection with no advanced settings
            foreach (Control dConControl in dCon.Controls)
            {
                //MessageBox.Show(dConControl.Name);

                if (dConControl.Text == "Ad&vanced...")
                {
                    dConControl.Visible = false;
                }

                if (dConControl.Controls.Count > 0)
                {
                    foreach (Control subControl in dConControl.Controls)
                    {
                        if (subControl.Text == "&Change...")
                        {
                            subControl.Visible = false;
                        }
                    }
                }
            }
            // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
#if (DEBUG)
            //===============================================================================================
            // Get Connection info from MIDSettings.config (for DEBUG only)
            //===============================================================================================
            if (_connectionStringArg == null || _connectionStringArg == string.Empty)
            {
                // Begin TT#1335-MD - stodd - Protect the database upgrade against an invalid path to the Midsettings.config
                try
                {
                    DirectoryInfo dir = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim());
                    _connectionStringArg = GetMIDSettingsDBConnectionString(dir + @"\DataCommon\MIDSettings.config");
                    //_connectionStringArg = GetMIDSettingsDBConnectionString(Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DataCommon\MIDSettings.config");
					// Begin TT#1346-MD - stodd - Database Upgrade does not handle a bad connection string correctly -
                    // Tests DB Connection information 
                    SqlConnection sql = new SqlConnection(_connectionStringArg);
                    try
                    {
                        sql.Open();
                    }
                    catch
                    {
                        if (runSilent)
                        {
                            MessageBox.Show("Database Upgrade will not be pre-populated. \nCheck to be sure the server, database, uid, & pwd are all correct. \nConnection String = " + _connectionStringArg, "Database connection string is not valid.");
                        }
                        _connectionStringArg = null;
                    }
                    finally
                    {
                        sql.Close();
                    }
					// End TT#1346-MD - stodd - Database Upgrade does not handle a bad connection string correctly -
                }
                catch   // Will go here if MIDSettings cannot be resolved.
                {
                    _connectionStringArg = null;
                }
                // End TT#1335-MD - stodd - Protect the database upgrade against an invalid path to the Midsettings.config
                if (_connectionStringArg != null && _connectionStringArg != string.Empty)
                {
                    //===============================================================================================
                    // creates a consistantly formatted connection string and sets it in the DataConnectionDialog
                    //===============================================================================================
                    Microsoft.Data.ConnectionUI.SqlConnectionProperties connProp = new Microsoft.Data.ConnectionUI.SqlConnectionProperties();
                    connProp.ConnectionStringBuilder.ConnectionString = _connectionStringArg;
                    _connectionStringArg = connProp.ConnectionStringBuilder.ConnectionString;
                    if (_connectionStringArg.Trim().Contains("Integrated Security=True") || _connectionStringArg.Trim().Contains("servername"))
                    {
                        Exception conn_exc = new Exception("The MIDSettings.config found in \\DataCommmon does not contain a proper database connection.\n Connection String = " + _connectionStringArg);
                        _connectionStringArg = null;
                        HandleException(conn_exc);
                    }
                }
            }
            else
            {
                if (_connectionStringArg.Trim().Contains("servername"))
                {
                    Exception conn_exc = new Exception("The MIDSettings.config found at \\DataCommmon does not contain a proper database connection.\n Connection String = " + _connectionStringArg);
                    _connectionStringArg = null;
                    HandleException(conn_exc);
                }
            }
#endif

            dCon.ConnectionString = _connectionStringArg;

            // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
            return dCon;
        }

        // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        private string GetMIDSettingsDBConnectionString(string MIDSettings_Location)
        {
            string DBConnString = "";

            XPathDocument doc = new XPathDocument(MIDSettings_Location);

            MIDRetail.Encryption.MIDEncryption crypt = new MIDRetail.Encryption.MIDEncryption();

            foreach (XPathNavigator child in doc.CreateNavigator().Select("appSettings/*"))
            {
                if (child.LocalName == "add")
                {
                    child.MoveToFirstAttribute();           //move to the key attribute
                    if (child.Value == "ConnectionString")
                    {
                        child.MoveToNextAttribute();        //move to the value attribute

                        DBConnString = crypt.Decrypt(child.Value);
                    }
                }
            }

            return DBConnString;
        }
        // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

       // Begin TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
        private bool IsValidDatabase(string aConnectionString, string aDatabase, out eDatabaseType aDatabaseType)
        {
            bool validDatabase = true;

            aDatabaseType = eDatabaseType.None;

            //Verify the database version and edition
            string productversion = "";
            string productlevel = "";
            string edition = "";
            string server = "";
            string database = "";
            if (VerifySQLVersion_Edition(aConnectionString, out productversion, out productlevel, out edition, out server, out database) != true)
            {
                SetLogMessage("The target database does not meet MID installation requirements: ", eErrorType.error);
                SetLogMessage("   Server:       " + server, eErrorType.error);
                SetLogMessage("   Database:     " + database, eErrorType.error);
                SetLogMessage("   SQL Version:  " + productversion, eErrorType.error);
                SetLogMessage("   SQL Level:    " + productlevel, eErrorType.error);
                SetLogMessage("   SQL Edition:  " + edition, eErrorType.error);

                validDatabase = false;
            }

            UpdateRoutines.CompatibilityLevel compatibilityLevel = UpdateRoutines.GetCompatibilityLevel(MessageQueue, aConnectionString, aDatabase);
            if (compatibilityLevel < UpdateRoutines.CompatibilityLevel.SQL2008)
            {
                SetLogMessage("The target database compatibility level does not meet MID installation requirements: ", eErrorType.error);
                SetLogMessage("   Server:              " + server, eErrorType.error);
                SetLogMessage("   Database:            " + database, eErrorType.error);
                SetLogMessage("   Compatibility Level: " + compatibilityLevel, eErrorType.error);
                SetLogMessage("   SQL Version:         " + productversion, eErrorType.error);
                SetLogMessage("   SQL Level:           " + productlevel, eErrorType.error);
                SetLogMessage("   SQL Edition:         " + edition, eErrorType.error);

                validDatabase = false;
            }
            else
            {
                switch (compatibilityLevel)
                {
                    case UpdateRoutines.CompatibilityLevel.SQL2005:
                        aDatabaseType = eDatabaseType.SQLServer2005;
                        break;
                    case UpdateRoutines.CompatibilityLevel.SQL2008:
                        aDatabaseType = eDatabaseType.SQLServer2008;
                        break;
                    case UpdateRoutines.CompatibilityLevel.SQL2012:
                        aDatabaseType = eDatabaseType.SQLServer2012;
                        break;
                    // Begin TT#1795-MD - JSmith - Support 2014
                    case UpdateRoutines.CompatibilityLevel.SQL2014:
                        aDatabaseType = eDatabaseType.SQLServer2014;
                        break;
                    // End TT#1795-MD - JSmith - Support 2014
                    // Begin TT#2130-MD - AGallagher - Support 2016
                    case UpdateRoutines.CompatibilityLevel.SQL2016:
                        aDatabaseType = eDatabaseType.SQLServer2016;
                        break;
                    // End TT#2130-MD - AGallagher - Support 2016
                    // Begin TT#1952-MD - AGallagher - Support 2017
                    case UpdateRoutines.CompatibilityLevel.SQL2017:
                        aDatabaseType = eDatabaseType.SQLServer2017;
                        break;
                    // End TT#1952-MD - AGallagher - Support 2017
                    // Begin - AGallagher - Support 2019
                    case UpdateRoutines.CompatibilityLevel.SQL2019:
                        aDatabaseType = eDatabaseType.SQLServer2019;
                        break;
                    // End - AGallagher - Support 2019
                    default:
                        aDatabaseType = eDatabaseType.None;
                        break;
                }
            }

            if (!UpdateRoutines.IsDatabaseCaseSensitive(MessageQueue, aConnectionString))
            {
                SetLogMessage("The target database is not case sensitive does not meet MID installation requirements: ", eErrorType.error);
                SetLogMessage("   Server:       " + server, eErrorType.error);
                SetLogMessage("   Database:     " + database, eErrorType.error);
                SetLogMessage("   Collation:    " + UpdateRoutines.GetCollation(MessageQueue, strConnString, aDatabase), eErrorType.error);
                SetLogMessage("   SQL Version:  " + productversion, eErrorType.error);
                SetLogMessage("   SQL Level:    " + productlevel, eErrorType.error);
                SetLogMessage("   SQL Edition:  " + edition, eErrorType.error);

                validDatabase = false;
            }

            // Begin TT#1795-MD - JSmith - Support 2014
            if (validDatabase)
            {
                SetLogMessage("Database Properties: ", eErrorType.message);
                SetLogMessage("   Server:              " + server, eErrorType.message);
                SetLogMessage("   Database:            " + database, eErrorType.message);
                SetLogMessage("   Compatibility Level: " + compatibilityLevel, eErrorType.message);
                SetLogMessage("   Collation:           " + UpdateRoutines.GetCollation(MessageQueue, strConnString, aDatabase), eErrorType.message);
                SetLogMessage("   SQL Version:         " + productversion, eErrorType.message);
                SetLogMessage("   SQL Level:           " + productlevel, eErrorType.message);
                SetLogMessage("   SQL Edition:         " + edition, eErrorType.message);
            }
            // End TT#1795-MD - JSmith - Support 2014

            return validDatabase;
        }
        // End TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process

        public bool VerifySQLVersion_Edition(string strConn, out string productversion, out string productlevel, out string edition, out string server, out string database)
        {
            //return value init
            bool blReturn = false;
            //get the version, level and edition from the target database
            SqlConnection sql = new SqlConnection(strConn);
            try
            {

                sql.Open();
                string sql_query = "SELECT  SERVERPROPERTY('productversion'), SERVERPROPERTY ('productlevel'), SERVERPROPERTY ('edition')";
                SqlCommand cmd = new SqlCommand(sql_query, sql);
                SqlDataReader read = cmd.ExecuteReader();
                read.Read();
                productversion = read[0].ToString().Trim(); //return sql version to user
                productlevel = read[1].ToString().Trim();   //return sql level to user
                edition = read[2].ToString().Trim();        //return sql edition to user
                server = sql.DataSource;
                database = sql.Database;
                cmd.Dispose();
                read.Close();
                sql.Close();

                //check value inits
                bool productversion_check = false;
                bool edition_check = false;

                //verify the target database version to the required information (stored in the installer config)
                char[] delim = ";".ToCharArray();
                string[] CompatibleSQLVersions = MIDConfigurationManager.AppSettings["SupportedSQLVersions"].ToString().Split(delim);
                foreach (string CompatibleSQLVersion in CompatibleSQLVersions)
                {
                    if (productversion.StartsWith(CompatibleSQLVersion) == true)
                    {
                        productversion_check = true;
                    }
                }

                //verify the target database edition to the required information (stored in the installer config)
                string[] CompatibleSQLEditions = MIDConfigurationManager.AppSettings["SupportedSQLEditions"].ToString().Split(delim);
                foreach (string CompatibleSQLEdition in CompatibleSQLEditions)
                {
                    if (productversion.Contains(CompatibleSQLEdition) == true)
                    {
                        edition_check = true;
                    }
                }

                //if the version and edition check out return true else leave the initial vale or false
                if (productversion_check == true || edition_check == true)
                {
                    blReturn = true;
                }

                //return method value
                return blReturn;
            }
            catch (Exception exc)
            {
                MessageErrorQueue.Enqueue(exc.ToString());
                productlevel = "";
                productversion = "";
                edition = "";
                server = "";
                database = "";
                return false;
            }
        }

        // Begin TT#3781 - stodd - add query tab to Database Utilty
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnExecute.Enabled = false;
            if (tabControl1.SelectedIndex == 3)
            {
                cbModify.Checked = false;
                if (strConnString.Length > 0)
                {
                    EnableQueryTab(true);
                    btnProcess.Text = "Execute";
                    //btnExecute.Enabled = true;
                    //btnProcess.Enabled = false;
                }
            }
            else
            {
                btnProcess.Text = "Process";
                //if (strConnString.Length > 0)
                //{
                //    btnProcess.Enabled = true;
                //}
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
        }

        private static void CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteSql()
        {
            string commandUpper = rtbSQL.Text.ToUpper();
            if (cbModify.Checked)
            {
                ExecuteNonQuery();
            }
            else
            {
                ExecuteQuery();
            }
        }


        private void ExecuteQuery()
        {
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;
            bool connectionOpen = false;

            string command = rtbSQL.Text.Trim();

            try
            {
                try
                {
                    string connectionString = strConnString;

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (SqlException sqlex)
                {
                    string message = sqlex.ToString();
                    MessageQueue.Enqueue("FATAL DB Error: Error encountered during open of database");
                    return;
                }
                catch (Exception ex)
                {
                    string message = "ERROR: " + ex.ToString();
                    MessageQueue.Enqueue(message);
                    return;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = command;
                dt = MIDEnvironment.CreateDataTable("config");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                ugResults.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlCommand.Connection.Close();
            }

        }

        private void ExecuteNonQuery()
        {
            DataTable dt;
            //SqlDataAdapter sda;
            SqlCommand sqlCommand = null;

            string command = rtbSQL.Text.Trim();

            try
            {
                try
                {
                    string connectionString = strConnString;

                    sqlCommand = new SqlCommand(command, new SqlConnection(connectionString));
                    sqlCommand.Connection.Open();
                    sqlCommand.Transaction = sqlCommand.Connection.BeginTransaction();
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand.Transaction.Commit();

                }
                catch (SqlException sqlex)
                {
                    MessageBox.Show(sqlex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //string message = sqlex.ToString();
                    //MessageQueue.Enqueue("FATAL DB Error: Error encountered during open of database");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //string message = "ERROR: " + ex.ToString();
                    //MessageQueue.Enqueue(message);
                    return;
                }
                finally
                {
                    sqlCommand.Connection.Close();

                    dt = new DataTable();
                    DataColumn dataColumn = new DataColumn();
                    dataColumn.DataType = System.Type.GetType("System.String");
                    dataColumn.ColumnName = "Status";
                    dataColumn.ReadOnly = true;
                    //dataColumn.Unique = false;
                    dt.Columns.Add(dataColumn);

                    DataRow aRow = dt.NewRow();
                    aRow["Status"] = "Modify Completed Successfully";
                    dt.Rows.Add(aRow);
                    ugResults.DataSource = dt;
                    ugResults.DisplayLayout.Bands[0].Columns["Status"].Width = 200;
                }

                //sqlCommand.CommandType = CommandType.Text;
                //sqlCommand.CommandText = command;
                //dt = MIDEnvironment.CreateDataTable("config");
                //sda = new SqlDataAdapter(sqlCommand);
                //sda.Fill(dt);

                //ugResults.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void EnableQueryTab(bool enable)
        {
            rtbSQL.Text = string.Empty;
            if (!enable)
            {
                rtbSQL.Text = "Database connection needed to enable Query functionality.";
            }
            TabPage atabPage = tabControl1.TabPages[3];
            foreach (Control ctl in atabPage.Controls) ctl.Enabled = enable;
        }


       private void rtbSQL_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            if (e.KeyCode == Keys.P && e.Modifiers == Keys.Control) 
			{
                PasteAction(this, null);
			}
		}

       private void rtbSQL_MouseUp(object sender, MouseEventArgs e)
       {
           if (e.Button == System.Windows.Forms.MouseButtons.Right)
           {   
               ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
               MenuItem menuItem = new MenuItem("Cut");
               //menuItem.Click += new EventHandler(CutAction);
               //contextMenu.MenuItems.Add(menuItem);
               //menuItem = new MenuItem("Copy");
               //contextMenu.MenuItems.Add(menuItem);
               menuItem = new MenuItem("Paste");
               menuItem.Click += new EventHandler(PasteAction);
               contextMenu.MenuItems.Add(menuItem);

               rtbSQL.ContextMenu = contextMenu;
           }
       }

       void PasteAction(object sender, EventArgs e)
       {
           if (Clipboard.ContainsText())
           {
               rtbSQL.Text += Clipboard.GetText(TextDataFormat.Text).ToString();
           }
       }

       private void ugResults_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
       {

       }

        private void cbxInstallNewROExtract_CheckedChanged(object sender, EventArgs e)
        {
            cbxInstallNew.Checked = false;
            cbxLicenceKey.Checked = false;
            cbxLicenceKey.Enabled = false;
        }

        // End TT#3781 - stodd - add query tab to Database Utilty
    }

    public enum eErrorType
    {
        error,
        warning,
        message,
        debug
    }


}

