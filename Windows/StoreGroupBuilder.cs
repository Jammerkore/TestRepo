using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

//todo - search for this string
//Take out error number lines from messagebox captions.
//check all message boxes
//comment all code 


namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for StoreGroupBuilder.
	/// WorkflowMethodFormBase
	/// </summary>
	public class StoreGroupBuilder : MIDFormBase
	{
		#region "Properties"
		private System.Windows.Forms.ListBox lbChar;
		private System.Windows.Forms.ListBox lbEngSQL;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtStoreGroupName;
		private System.Windows.Forms.Button cmdAddAll;
		private System.Windows.Forms.Button cmdRemoveAll;
		private System.Windows.Forms.CheckedListBox clbStoresAll;
		private System.Windows.Forms.ListBox lbStoresSel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button cmdModify;
		private System.Windows.Forms.Button cmdRemove;
		private System.Windows.Forms.Button cmdAnd;
		private System.Windows.Forms.Button cmdOr;
		private System.Windows.Forms.Button cmdNot;
		private System.Windows.Forms.Button cmdLParen;
		private System.Windows.Forms.Button cmdRParen;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.Button cmdCompareDateB1;
		private System.Windows.Forms.Button cmdCompareDateB2;
		private System.Windows.Forms.Button cmdCompareDate;
		private System.Windows.Forms.Panel tmpDateNumbPanel;
		private System.Windows.Forms.TextBox txtBetweenDateNumb2;
		private System.Windows.Forms.TextBox txtBetweenDateNumb1;
		private System.Windows.Forms.TextBox txtCompareDateNumb;
		private System.Windows.Forms.RadioButton dateNumbBetweenButton;
		private System.Windows.Forms.RadioButton dateNumbLThanButton;
		private System.Windows.Forms.RadioButton dateNumbGThanButton;
		private System.Windows.Forms.RadioButton dateNumbEqualButton;
		private System.Windows.Forms.Panel tmpBoolPanel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton boolFalseButton;
		private System.Windows.Forms.RadioButton boolTrueButton;
		private System.Windows.Forms.Panel tmpStringPanel;
		private System.Windows.Forms.RadioButton stringNotEqualButton;
		private System.Windows.Forms.RadioButton stringEqualButton;
		private System.Windows.Forms.TextBox txtString;
		private System.Windows.Forms.Label lblDateNumbAND;
		private System.Windows.Forms.Panel pnlCharacteristics;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button cmdCharAddAll;
		private System.Windows.Forms.Button cmdCharRemoveAll;
		private System.Windows.Forms.CheckedListBox clbCharAll;
		private System.Windows.Forms.ListBox lbCharSel;
		private System.Windows.Forms.RadioButton charNotInButton;
		private System.Windows.Forms.RadioButton charInButton;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button cmdCancelGroupBox;

		private StoreServerSession _storeSession;
		private ArrayList _storeChar;
		private DataTable _dtStoreCharGroup;
		private DataTable _dtAllStores;
		private DataTable _dtAppText;
		private DataTable _dtStoreGroupLevelStatement;
		private DataSet _dsStoreChar;
		private DataView _dvStoreChar;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDStoreNode _SG_Node;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private int _SGL_Seq;
		private int _SG_RID;
		private int _SGL_RID;
		private string _SQLOperator;
		private bool _newGroupLevel;
		private string _origGroupName;
		private string _newGroupLevelName;
		//private Audit _mel;
		private SessionAddressBlock _SAB;
		private int _UserID;
		private bool _SGLS_CHAR_IND;
		private int _SGLS_CHAR_ID;
		private string _SCG_NAME;
		private eENGSQLOperator _SGLS_SQL_OPERATOR;
		private eStoreCharType _SGLS_DT;
		private string _SGLS_VALUE;
		private string _SGLS_VALUE_CHAR_RID;
		private string _SGLS_PREFIX = "";
		private string _SGLS_SUFFIX = "";
		private int _SCG_RID;
		private bool _isModify = false;
		private DataTable _dtChar;
		private bool _isNewStoreChar = false;
		private bool _storeCharHasList = false;
		private eStoreCharType _storeCharType;
		private bool _error = false;
		private int _hzSize = 0;
		private const string STORE_TABLE_COLUMN = "0";
		private const string DYNAMIC_CHARACTERISTIC = "1";
		private DataTable _dtStoreStatus;
		private bool _storeMaintIsActive; // 5431


		private const int addTagArrayLength = 3;
		private System.Windows.Forms.Button btnCancel;

		// add event to update explorer when new group level (set) is added
		public delegate void StoreGroupLevelChangeEventHandler(object source, StoreGroupLevelChangeEventArgs e);
		public event StoreGroupLevelChangeEventHandler OnStoreGroupLevelPropertyChangeHandler;
		// event to refresh store explorer
		public delegate void RefreshStoreGroupEventHandler(object source, StoreGroupLevelChangeEventArgs e);
		public event RefreshStoreGroupEventHandler OnRefreshStoreGroupEvent;

		private enum eAddTagArrayDef
		{
			eStoreCharType = 0,
			eIsStore_Char_Group = 1,
			eStoreTableColumns = 2
		}
		private enum eIsStore_Char_Group
		{
			False = 0,
			True = 1
		}
	 

//		private string [] allOperators = 
//					  {"0","1","=",">","<","BETWEEN", "<>", "LIKE","IN", "NOT IN", "AND"};

		public string NewGroupName 
		{
			get { return _newGroupLevelName ;}
		}

		public int SGL_RID 
		{
			get { return _SGL_RID ;}
		}

		public int SGL_Sequence 
		{
			get { return _SGL_Seq ;}
		}

		#endregion

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public StoreGroupBuilder(int SGL_Seq, int SG_RID, int SGL_RID, SessionAddressBlock SAB, bool storeMaintIsActive)
		public StoreGroupBuilder(int SGL_Seq, MIDStoreNode SG_Node, int SGL_RID, SessionAddressBlock SAB, bool storeMaintIsActive)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(SAB)
		{
			//			_mel = new Audit(eProcesses.storeGroupBuilder, userRID);
			//			_mel.AddHeader();

			Cursor.Current = Cursors.WaitCursor;
			//This is here so the designer doesn't blow up.
			this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreGroupBuilder);

			//NEED from explorer
			_SGL_Seq = SGL_Seq;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_SG_RID = SG_RID;
			_SG_Node = SG_Node;
			_SG_RID = _SG_Node.Profile.Key;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			_UserID = SAB.ClientServerSession.UserRID;
			_storeMaintIsActive = storeMaintIsActive;  // Issue 5431

			//if SGL_RID = 0 then NewGroup
			if (SGL_RID == 0)
				_newGroupLevel = true;
			else
			{
				_newGroupLevel = false;
				_SGL_RID = SGL_RID;
			}

			_dtStoreStatus = MIDText.GetLabels((int) eStoreStatus.New,(int) eStoreStatus.Preopen);

			InitializeComponent();

			_SAB = SAB;
			_storeSession = _SAB.StoreServerSession;

			//populate datatable containing Store_Group_Dyn_Join info
			_dtStoreGroupLevelStatement = _storeSession.GetStoreGroupLevelStatement(_SGL_RID);
			DataColumn[] PrimaryKeyColumn = new DataColumn[2];
			PrimaryKeyColumn[0] = _dtStoreGroupLevelStatement.Columns["SGL_RID"];
			PrimaryKeyColumn[1] = _dtStoreGroupLevelStatement.Columns["SGLS_STATEMENT_SEQ"];
			_dtStoreGroupLevelStatement.PrimaryKey = PrimaryKeyColumn;

			//Populate CheckedListBox to be populated on form load - MUST BE HERE
			PopulateAllStoresListbox();

            // Begin TT#1119 - JSmith - Unable to create User attributes in My Hierarchy
            //FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributes);
            if (SG_Node.OwnerUserRID == Include.GlobalUserRID)
            {
                FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesGlobal);
            }
            else
            {
                FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
            }
            // End TT#1119


		}

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

				this.lbChar.DoubleClick -= new System.EventHandler(this.lbChar_DoubleClick);
				this.lbEngSQL.SelectedIndexChanged -= new System.EventHandler(this.lbEngSQL_SelectedIndexChanged);
				this.cmdSave.Click -= new System.EventHandler(this.cmdSave_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.cmdAdd.Click -= new System.EventHandler(this.cmdAdd_Click);
				this.charInButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.charNotInButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.cmdCharRemoveAll.Click -= new System.EventHandler(this.cmdCharRemoveAll_Click);
				this.cmdCharAddAll.Click -= new System.EventHandler(this.cmdCharAddAll_Click);
				this.clbCharAll.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.clbCharAll_ItemCheck);
				this.boolFalseButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.boolTrueButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.stringNotEqualButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.stringEqualButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.cmdCompareDate.Click -= new System.EventHandler(this.cmdCompareDate_Click);
				this.cmdCompareDateB2.Click -= new System.EventHandler(this.cmdCompareDateB2_Click);
				this.cmdCompareDateB1.Click -= new System.EventHandler(this.cmdCompareDateB1_Click);
				this.dateNumbBetweenButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.dateNumbLThanButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.dateNumbGThanButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.dateNumbEqualButton.CheckedChanged -= new System.EventHandler(this.genericRadioButton_CheckedChanged);
				this.cmdCancelGroupBox.Click -= new System.EventHandler(this.cmdCancelGroupBox_Click);
				this.cmdAddAll.Click -= new System.EventHandler(this.cmdAddAll_Click);
				this.cmdRemoveAll.Click -= new System.EventHandler(this.cmdRemoveAll_Click);
				this.clbStoresAll.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.clbStoresAll_ItemCheck);
				this.cmdModify.Click -= new System.EventHandler(this.cmdModify_Click);
				this.cmdRemove.Click -= new System.EventHandler(this.cmdRemove_Click);
				this.cmdAnd.Click -= new System.EventHandler(this.cmdAnd_Click);
				this.cmdOr.Click -= new System.EventHandler(this.cmdOr_Click);
				this.cmdNot.Click -= new System.EventHandler(this.cmdNot_Click);
				this.cmdLParen.Click -= new System.EventHandler(this.cmdLParen_Click);
				this.cmdRParen.Click -= new System.EventHandler(this.cmdRParen_Click);
				this.Load -= new System.EventHandler(this.StoreGroupBuilder_Load);
			}
			base.Dispose( disposing );
			//_storeSession.Terminate();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreGroupBuilder));
            this.lbChar = new System.Windows.Forms.ListBox();
            this.lbEngSQL = new System.Windows.Forms.ListBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.pnlCharacteristics = new System.Windows.Forms.Panel();
            this.charInButton = new System.Windows.Forms.RadioButton();
            this.charNotInButton = new System.Windows.Forms.RadioButton();
            this.cmdCharRemoveAll = new System.Windows.Forms.Button();
            this.cmdCharAddAll = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lbCharSel = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.clbCharAll = new System.Windows.Forms.CheckedListBox();
            this.tmpBoolPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.boolFalseButton = new System.Windows.Forms.RadioButton();
            this.boolTrueButton = new System.Windows.Forms.RadioButton();
            this.tmpStringPanel = new System.Windows.Forms.Panel();
            this.txtString = new System.Windows.Forms.TextBox();
            this.stringNotEqualButton = new System.Windows.Forms.RadioButton();
            this.stringEqualButton = new System.Windows.Forms.RadioButton();
            this.tmpDateNumbPanel = new System.Windows.Forms.Panel();
            this.cmdCompareDate = new System.Windows.Forms.Button();
            this.txtBetweenDateNumb2 = new System.Windows.Forms.TextBox();
            this.cmdCompareDateB2 = new System.Windows.Forms.Button();
            this.lblDateNumbAND = new System.Windows.Forms.Label();
            this.txtBetweenDateNumb1 = new System.Windows.Forms.TextBox();
            this.txtCompareDateNumb = new System.Windows.Forms.TextBox();
            this.cmdCompareDateB1 = new System.Windows.Forms.Button();
            this.dateNumbBetweenButton = new System.Windows.Forms.RadioButton();
            this.dateNumbLThanButton = new System.Windows.Forms.RadioButton();
            this.dateNumbGThanButton = new System.Windows.Forms.RadioButton();
            this.dateNumbEqualButton = new System.Windows.Forms.RadioButton();
            this.cmdCancelGroupBox = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStoreGroupName = new System.Windows.Forms.TextBox();
            this.cmdAddAll = new System.Windows.Forms.Button();
            this.cmdRemoveAll = new System.Windows.Forms.Button();
            this.clbStoresAll = new System.Windows.Forms.CheckedListBox();
            this.lbStoresSel = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdModify = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.cmdAnd = new System.Windows.Forms.Button();
            this.cmdOr = new System.Windows.Forms.Button();
            this.cmdNot = new System.Windows.Forms.Button();
            this.cmdLParen = new System.Windows.Forms.Button();
            this.cmdRParen = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlCharacteristics.SuspendLayout();
            this.tmpBoolPanel.SuspendLayout();
            this.tmpStringPanel.SuspendLayout();
            this.tmpDateNumbPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lbChar
            // 
            this.lbChar.Location = new System.Drawing.Point(8, 56);
            this.lbChar.Name = "lbChar";
            this.lbChar.Size = new System.Drawing.Size(128, 134);
            this.lbChar.TabIndex = 3;
            this.lbChar.DoubleClick += new System.EventHandler(this.lbChar_DoubleClick);
            // 
            // lbEngSQL
            // 
            this.lbEngSQL.HorizontalExtent = 50;
            this.lbEngSQL.HorizontalScrollbar = true;
            this.lbEngSQL.Location = new System.Drawing.Point(8, 224);
            this.lbEngSQL.Name = "lbEngSQL";
            this.lbEngSQL.Size = new System.Drawing.Size(313, 147);
            this.lbEngSQL.TabIndex = 7;
            this.lbEngSQL.SelectedIndexChanged += new System.EventHandler(this.lbEngSQL_SelectedIndexChanged);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(341, 530);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 21;
            this.cmdSave.Text = "&Save";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 530);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdAdd);
            this.groupBox1.Controls.Add(this.pnlCharacteristics);
            this.groupBox1.Controls.Add(this.tmpBoolPanel);
            this.groupBox1.Controls.Add(this.tmpStringPanel);
            this.groupBox1.Controls.Add(this.tmpDateNumbPanel);
            this.groupBox1.Controls.Add(this.cmdCancelGroupBox);
            this.groupBox1.Location = new System.Drawing.Point(139, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 160);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.groupBox1.Visible = false;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(83, 131);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdAdd.TabIndex = 0;
            this.cmdAdd.Text = "&Add";
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // pnlCharacteristics
            // 
            this.pnlCharacteristics.Controls.Add(this.charInButton);
            this.pnlCharacteristics.Controls.Add(this.charNotInButton);
            this.pnlCharacteristics.Controls.Add(this.cmdCharRemoveAll);
            this.pnlCharacteristics.Controls.Add(this.cmdCharAddAll);
            this.pnlCharacteristics.Controls.Add(this.label8);
            this.pnlCharacteristics.Controls.Add(this.lbCharSel);
            this.pnlCharacteristics.Controls.Add(this.label6);
            this.pnlCharacteristics.Controls.Add(this.clbCharAll);
            this.pnlCharacteristics.Location = new System.Drawing.Point(6, 15);
            this.pnlCharacteristics.Name = "pnlCharacteristics";
            this.pnlCharacteristics.Size = new System.Drawing.Size(334, 112);
            this.pnlCharacteristics.TabIndex = 16;
            // 
            // charInButton
            // 
            this.charInButton.Location = new System.Drawing.Point(139, 8);
            this.charInButton.Name = "charInButton";
            this.charInButton.Size = new System.Drawing.Size(32, 23);
            this.charInButton.TabIndex = 7;
            this.charInButton.Text = "=";
            this.charInButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // charNotInButton
            // 
            this.charNotInButton.Location = new System.Drawing.Point(139, 34);
            this.charNotInButton.Name = "charNotInButton";
            this.charNotInButton.Size = new System.Drawing.Size(32, 23);
            this.charNotInButton.TabIndex = 6;
            this.charNotInButton.Text = "!=";
            this.charNotInButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // cmdCharRemoveAll
            // 
            this.cmdCharRemoveAll.Location = new System.Drawing.Point(139, 86);
            this.cmdCharRemoveAll.Name = "cmdCharRemoveAll";
            this.cmdCharRemoveAll.Size = new System.Drawing.Size(32, 23);
            this.cmdCharRemoveAll.TabIndex = 5;
            this.cmdCharRemoveAll.Text = "<<";
            this.cmdCharRemoveAll.Click += new System.EventHandler(this.cmdCharRemoveAll_Click);
            // 
            // cmdCharAddAll
            // 
            this.cmdCharAddAll.Location = new System.Drawing.Point(139, 61);
            this.cmdCharAddAll.Name = "cmdCharAddAll";
            this.cmdCharAddAll.Size = new System.Drawing.Size(32, 23);
            this.cmdCharAddAll.TabIndex = 4;
            this.cmdCharAddAll.Text = ">>";
            this.cmdCharAddAll.Click += new System.EventHandler(this.cmdCharAddAll_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(188, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "Selected Char:";
            // 
            // lbCharSel
            // 
            this.lbCharSel.HorizontalScrollbar = true;
            this.lbCharSel.Location = new System.Drawing.Point(186, 17);
            this.lbCharSel.Name = "lbCharSel";
            this.lbCharSel.Size = new System.Drawing.Size(128, 95);
            this.lbCharSel.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "All Char:";
            // 
            // clbCharAll
            // 
            this.clbCharAll.CheckOnClick = true;
            this.clbCharAll.HorizontalScrollbar = true;
            this.clbCharAll.Location = new System.Drawing.Point(3, 16);
            this.clbCharAll.Name = "clbCharAll";
            this.clbCharAll.Size = new System.Drawing.Size(122, 94);
            this.clbCharAll.TabIndex = 0;
            this.clbCharAll.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbCharAll_ItemCheck);
            // 
            // tmpBoolPanel
            // 
            this.tmpBoolPanel.Controls.Add(this.label7);
            this.tmpBoolPanel.Controls.Add(this.boolFalseButton);
            this.tmpBoolPanel.Controls.Add(this.boolTrueButton);
            this.tmpBoolPanel.Location = new System.Drawing.Point(6, 15);
            this.tmpBoolPanel.Name = "tmpBoolPanel";
            this.tmpBoolPanel.Size = new System.Drawing.Size(324, 112);
            this.tmpBoolPanel.TabIndex = 14;
            this.tmpBoolPanel.Visible = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(30, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 19);
            this.label7.TabIndex = 10;
            this.label7.Text = "Is";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // boolFalseButton
            // 
            this.boolFalseButton.Location = new System.Drawing.Point(134, 48);
            this.boolFalseButton.Name = "boolFalseButton";
            this.boolFalseButton.Size = new System.Drawing.Size(52, 24);
            this.boolFalseButton.TabIndex = 1;
            this.boolFalseButton.Text = "False";
            this.boolFalseButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // boolTrueButton
            // 
            this.boolTrueButton.Location = new System.Drawing.Point(30, 48);
            this.boolTrueButton.Name = "boolTrueButton";
            this.boolTrueButton.Size = new System.Drawing.Size(52, 24);
            this.boolTrueButton.TabIndex = 0;
            this.boolTrueButton.Text = "True";
            this.boolTrueButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // tmpStringPanel
            // 
            this.tmpStringPanel.Controls.Add(this.txtString);
            this.tmpStringPanel.Controls.Add(this.stringNotEqualButton);
            this.tmpStringPanel.Controls.Add(this.stringEqualButton);
            this.tmpStringPanel.Location = new System.Drawing.Point(6, 15);
            this.tmpStringPanel.Name = "tmpStringPanel";
            this.tmpStringPanel.Size = new System.Drawing.Size(331, 112);
            this.tmpStringPanel.TabIndex = 15;
            this.tmpStringPanel.Visible = false;
            // 
            // txtString
            // 
            this.txtString.Location = new System.Drawing.Point(8, 56);
            this.txtString.Name = "txtString";
            this.txtString.Size = new System.Drawing.Size(200, 20);
            this.txtString.TabIndex = 2;
            // 
            // stringNotEqualButton
            // 
            this.stringNotEqualButton.Location = new System.Drawing.Point(134, 16);
            this.stringNotEqualButton.Name = "stringNotEqualButton";
            this.stringNotEqualButton.Size = new System.Drawing.Size(52, 24);
            this.stringNotEqualButton.TabIndex = 1;
            this.stringNotEqualButton.Text = "<>";
            this.stringNotEqualButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // stringEqualButton
            // 
            this.stringEqualButton.Location = new System.Drawing.Point(30, 16);
            this.stringEqualButton.Name = "stringEqualButton";
            this.stringEqualButton.Size = new System.Drawing.Size(52, 24);
            this.stringEqualButton.TabIndex = 0;
            this.stringEqualButton.Text = "=";
            this.stringEqualButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // tmpDateNumbPanel
            // 
            this.tmpDateNumbPanel.Controls.Add(this.cmdCompareDate);
            this.tmpDateNumbPanel.Controls.Add(this.txtBetweenDateNumb2);
            this.tmpDateNumbPanel.Controls.Add(this.cmdCompareDateB2);
            this.tmpDateNumbPanel.Controls.Add(this.lblDateNumbAND);
            this.tmpDateNumbPanel.Controls.Add(this.txtBetweenDateNumb1);
            this.tmpDateNumbPanel.Controls.Add(this.txtCompareDateNumb);
            this.tmpDateNumbPanel.Controls.Add(this.cmdCompareDateB1);
            this.tmpDateNumbPanel.Controls.Add(this.dateNumbBetweenButton);
            this.tmpDateNumbPanel.Controls.Add(this.dateNumbLThanButton);
            this.tmpDateNumbPanel.Controls.Add(this.dateNumbGThanButton);
            this.tmpDateNumbPanel.Controls.Add(this.dateNumbEqualButton);
            this.tmpDateNumbPanel.Location = new System.Drawing.Point(6, 15);
            this.tmpDateNumbPanel.Name = "tmpDateNumbPanel";
            this.tmpDateNumbPanel.Size = new System.Drawing.Size(325, 112);
            this.tmpDateNumbPanel.TabIndex = 1;
            this.tmpDateNumbPanel.Visible = false;
            // 
            // cmdCompareDate
            // 
            this.cmdCompareDate.Image = ((System.Drawing.Image)(resources.GetObject("cmdCompareDate.Image")));
            this.cmdCompareDate.Location = new System.Drawing.Point(134, 33);
            this.cmdCompareDate.Name = "cmdCompareDate";
            this.cmdCompareDate.Size = new System.Drawing.Size(19, 20);
            this.cmdCompareDate.TabIndex = 13;
            this.cmdCompareDate.Click += new System.EventHandler(this.cmdCompareDate_Click);
            // 
            // txtBetweenDateNumb2
            // 
            this.txtBetweenDateNumb2.Location = new System.Drawing.Point(123, 81);
            this.txtBetweenDateNumb2.Name = "txtBetweenDateNumb2";
            this.txtBetweenDateNumb2.Size = new System.Drawing.Size(72, 20);
            this.txtBetweenDateNumb2.TabIndex = 12;
            // 
            // cmdCompareDateB2
            // 
            this.cmdCompareDateB2.Image = ((System.Drawing.Image)(resources.GetObject("cmdCompareDateB2.Image")));
            this.cmdCompareDateB2.Location = new System.Drawing.Point(195, 81);
            this.cmdCompareDateB2.Name = "cmdCompareDateB2";
            this.cmdCompareDateB2.Size = new System.Drawing.Size(19, 20);
            this.cmdCompareDateB2.TabIndex = 11;
            this.cmdCompareDateB2.Click += new System.EventHandler(this.cmdCompareDateB2_Click);
            // 
            // lblDateNumbAND
            // 
            this.lblDateNumbAND.Location = new System.Drawing.Point(110, 72);
            this.lblDateNumbAND.Name = "lblDateNumbAND";
            this.lblDateNumbAND.Size = new System.Drawing.Size(40, 19);
            this.lblDateNumbAND.TabIndex = 10;
            this.lblDateNumbAND.Text = "And";
            this.lblDateNumbAND.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtBetweenDateNumb1
            // 
            this.txtBetweenDateNumb1.Location = new System.Drawing.Point(3, 81);
            this.txtBetweenDateNumb1.Name = "txtBetweenDateNumb1";
            this.txtBetweenDateNumb1.Size = new System.Drawing.Size(72, 20);
            this.txtBetweenDateNumb1.TabIndex = 9;
            // 
            // txtCompareDateNumb
            // 
            this.txtCompareDateNumb.Location = new System.Drawing.Point(64, 33);
            this.txtCompareDateNumb.Name = "txtCompareDateNumb";
            this.txtCompareDateNumb.Size = new System.Drawing.Size(72, 20);
            this.txtCompareDateNumb.TabIndex = 7;
            // 
            // cmdCompareDateB1
            // 
            this.cmdCompareDateB1.Image = ((System.Drawing.Image)(resources.GetObject("cmdCompareDateB1.Image")));
            this.cmdCompareDateB1.Location = new System.Drawing.Point(75, 81);
            this.cmdCompareDateB1.Name = "cmdCompareDateB1";
            this.cmdCompareDateB1.Size = new System.Drawing.Size(19, 20);
            this.cmdCompareDateB1.TabIndex = 6;
            this.cmdCompareDateB1.Click += new System.EventHandler(this.cmdCompareDateB1_Click);
            // 
            // dateNumbBetweenButton
            // 
            this.dateNumbBetweenButton.Location = new System.Drawing.Point(3, 57);
            this.dateNumbBetweenButton.Name = "dateNumbBetweenButton";
            this.dateNumbBetweenButton.Size = new System.Drawing.Size(88, 24);
            this.dateNumbBetweenButton.TabIndex = 3;
            this.dateNumbBetweenButton.Text = "Between";
            this.dateNumbBetweenButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // dateNumbLThanButton
            // 
            this.dateNumbLThanButton.Location = new System.Drawing.Point(172, 1);
            this.dateNumbLThanButton.Name = "dateNumbLThanButton";
            this.dateNumbLThanButton.Size = new System.Drawing.Size(32, 24);
            this.dateNumbLThanButton.TabIndex = 2;
            this.dateNumbLThanButton.Text = "<";
            this.dateNumbLThanButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // dateNumbGThanButton
            // 
            this.dateNumbGThanButton.Location = new System.Drawing.Point(92, 1);
            this.dateNumbGThanButton.Name = "dateNumbGThanButton";
            this.dateNumbGThanButton.Size = new System.Drawing.Size(32, 24);
            this.dateNumbGThanButton.TabIndex = 1;
            this.dateNumbGThanButton.Text = ">";
            this.dateNumbGThanButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // dateNumbEqualButton
            // 
            this.dateNumbEqualButton.Location = new System.Drawing.Point(12, 1);
            this.dateNumbEqualButton.Name = "dateNumbEqualButton";
            this.dateNumbEqualButton.Size = new System.Drawing.Size(32, 24);
            this.dateNumbEqualButton.TabIndex = 0;
            this.dateNumbEqualButton.Text = "=";
            this.dateNumbEqualButton.CheckedChanged += new System.EventHandler(this.genericRadioButton_CheckedChanged);
            // 
            // cmdCancelGroupBox
            // 
            this.cmdCancelGroupBox.Location = new System.Drawing.Point(164, 131);
            this.cmdCancelGroupBox.Name = "cmdCancelGroupBox";
            this.cmdCancelGroupBox.Size = new System.Drawing.Size(75, 23);
            this.cmdCancelGroupBox.TabIndex = 17;
            this.cmdCancelGroupBox.Text = "Ca&ncel";
            this.cmdCancelGroupBox.Click += new System.EventHandler(this.cmdCancelGroupBox_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Store Set Na&me:";
            // 
            // txtStoreGroupName
            // 
            this.txtStoreGroupName.Location = new System.Drawing.Point(128, 4);
            this.txtStoreGroupName.Name = "txtStoreGroupName";
            this.txtStoreGroupName.Size = new System.Drawing.Size(240, 20);
            this.txtStoreGroupName.TabIndex = 1;
            // 
            // cmdAddAll
            // 
            this.cmdAddAll.Location = new System.Drawing.Point(177, 407);
            this.cmdAddAll.Name = "cmdAddAll";
            this.cmdAddAll.Size = new System.Drawing.Size(91, 23);
            this.cmdAddAll.TabIndex = 17;
            this.cmdAddAll.Text = "&Add All >>";
            this.cmdAddAll.Click += new System.EventHandler(this.cmdAddAll_Click);
            // 
            // cmdRemoveAll
            // 
            this.cmdRemoveAll.Location = new System.Drawing.Point(176, 439);
            this.cmdRemoveAll.Name = "cmdRemoveAll";
            this.cmdRemoveAll.Size = new System.Drawing.Size(91, 23);
            this.cmdRemoveAll.TabIndex = 18;
            this.cmdRemoveAll.Text = "<< &Remove All";
            this.cmdRemoveAll.Click += new System.EventHandler(this.cmdRemoveAll_Click);
            // 
            // clbStoresAll
            // 
            this.clbStoresAll.CheckOnClick = true;
            this.clbStoresAll.Location = new System.Drawing.Point(8, 397);
            this.clbStoresAll.Name = "clbStoresAll";
            this.clbStoresAll.Size = new System.Drawing.Size(157, 154);
            this.clbStoresAll.TabIndex = 16;
            this.clbStoresAll.ThreeDCheckBoxes = true;
            this.clbStoresAll.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbStoresAll_ItemCheck);
            // 
            // lbStoresSel
            // 
            this.lbStoresSel.Location = new System.Drawing.Point(280, 397);
            this.lbStoresSel.Name = "lbStoresSel";
            this.lbStoresSel.Size = new System.Drawing.Size(205, 121);
            this.lbStoresSel.Sorted = true;
            this.lbStoresSel.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 381);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Al&ways include these stores:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(280, 384);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Selected s&tores:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.label4.Location = new System.Drawing.Point(8, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Select St&ores Where:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Store C&haracteristics:";
            // 
            // cmdModify
            // 
            this.cmdModify.BackColor = System.Drawing.SystemColors.Control;
            this.cmdModify.Location = new System.Drawing.Point(352, 32);
            this.cmdModify.Name = "cmdModify";
            this.cmdModify.Size = new System.Drawing.Size(120, 23);
            this.cmdModify.TabIndex = 27;
            this.cmdModify.Text = "&Modify";
            this.cmdModify.UseVisualStyleBackColor = false;
            this.cmdModify.Click += new System.EventHandler(this.cmdModify_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.BackColor = System.Drawing.SystemColors.Control;
            this.cmdRemove.Location = new System.Drawing.Point(352, 64);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(120, 23);
            this.cmdRemove.TabIndex = 9;
            this.cmdRemove.Text = "&Remove";
            this.cmdRemove.UseVisualStyleBackColor = false;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // cmdAnd
            // 
            this.cmdAnd.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAnd.Location = new System.Drawing.Point(352, 96);
            this.cmdAnd.Name = "cmdAnd";
            this.cmdAnd.Size = new System.Drawing.Size(40, 23);
            this.cmdAnd.TabIndex = 10;
            this.cmdAnd.Text = "An&d";
            this.cmdAnd.UseVisualStyleBackColor = false;
            this.cmdAnd.Click += new System.EventHandler(this.cmdAnd_Click);
            // 
            // cmdOr
            // 
            this.cmdOr.BackColor = System.Drawing.SystemColors.Control;
            this.cmdOr.Location = new System.Drawing.Point(392, 96);
            this.cmdOr.Name = "cmdOr";
            this.cmdOr.Size = new System.Drawing.Size(40, 23);
            this.cmdOr.TabIndex = 11;
            this.cmdOr.Text = "&Or";
            this.cmdOr.UseVisualStyleBackColor = false;
            this.cmdOr.Click += new System.EventHandler(this.cmdOr_Click);
            // 
            // cmdNot
            // 
            this.cmdNot.BackColor = System.Drawing.SystemColors.Control;
            this.cmdNot.Location = new System.Drawing.Point(432, 96);
            this.cmdNot.Name = "cmdNot";
            this.cmdNot.Size = new System.Drawing.Size(40, 23);
            this.cmdNot.TabIndex = 12;
            this.cmdNot.Text = "&Not";
            this.cmdNot.UseVisualStyleBackColor = false;
            this.cmdNot.Click += new System.EventHandler(this.cmdNot_Click);
            // 
            // cmdLParen
            // 
            this.cmdLParen.BackColor = System.Drawing.SystemColors.Control;
            this.cmdLParen.Location = new System.Drawing.Point(384, 128);
            this.cmdLParen.Name = "cmdLParen";
            this.cmdLParen.Size = new System.Drawing.Size(24, 23);
            this.cmdLParen.TabIndex = 13;
            this.cmdLParen.Text = "&(";
            this.cmdLParen.UseVisualStyleBackColor = false;
            this.cmdLParen.Click += new System.EventHandler(this.cmdLParen_Click);
            // 
            // cmdRParen
            // 
            this.cmdRParen.BackColor = System.Drawing.SystemColors.Control;
            this.cmdRParen.Location = new System.Drawing.Point(424, 128);
            this.cmdRParen.Name = "cmdRParen";
            this.cmdRParen.Size = new System.Drawing.Size(24, 23);
            this.cmdRParen.TabIndex = 14;
            this.cmdRParen.Text = "&)";
            this.cmdRParen.UseVisualStyleBackColor = false;
            this.cmdRParen.Click += new System.EventHandler(this.cmdRParen_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.WindowText;
            this.panel2.Location = new System.Drawing.Point(4, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(368, 4);
            this.panel2.TabIndex = 26;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.cmdRParen);
            this.panel3.Controls.Add(this.cmdLParen);
            this.panel3.Controls.Add(this.cmdModify);
            this.panel3.Controls.Add(this.cmdOr);
            this.panel3.Controls.Add(this.cmdNot);
            this.panel3.Controls.Add(this.cmdRemove);
            this.panel3.Controls.Add(this.cmdAnd);
            this.panel3.Location = new System.Drawing.Point(0, 207);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(493, 174);
            this.panel3.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Modify";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(252, 255);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "&Remove";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(252, 287);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "An&d";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(295, 291);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(40, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "&Or";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(259, 258);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "&Remove";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(259, 290);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "An&d";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(257, 225);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(120, 23);
            this.button7.TabIndex = 8;
            this.button7.Text = "&Modify";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(249, 282);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(40, 23);
            this.button8.TabIndex = 10;
            this.button8.Text = "An&d";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(252, 259);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(120, 23);
            this.button9.TabIndex = 9;
            this.button9.Text = "&Remove";
            // 
            // StoreGroupBuilder
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(519, 558);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbStoresSel);
            this.Controls.Add(this.clbStoresAll);
            this.Controls.Add(this.cmdRemoveAll);
            this.Controls.Add(this.cmdAddAll);
            this.Controls.Add(this.txtStoreGroupName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.lbEngSQL);
            this.Controls.Add(this.lbChar);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "StoreGroupBuilder";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.StoreGroupBuilder_Closing);
            this.Load += new System.EventHandler(this.StoreGroupBuilder_Load);
            this.Controls.SetChildIndex(this.button9, 0);
            this.Controls.SetChildIndex(this.button8, 0);
            this.Controls.SetChildIndex(this.button7, 0);
            this.Controls.SetChildIndex(this.button6, 0);
            this.Controls.SetChildIndex(this.button5, 0);
            this.Controls.SetChildIndex(this.button4, 0);
            this.Controls.SetChildIndex(this.button3, 0);
            this.Controls.SetChildIndex(this.button2, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.lbChar, 0);
            this.Controls.SetChildIndex(this.lbEngSQL, 0);
            this.Controls.SetChildIndex(this.cmdSave, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtStoreGroupName, 0);
            this.Controls.SetChildIndex(this.cmdAddAll, 0);
            this.Controls.SetChildIndex(this.cmdRemoveAll, 0);
            this.Controls.SetChildIndex(this.clbStoresAll, 0);
            this.Controls.SetChildIndex(this.lbStoresSel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.pnlCharacteristics.ResumeLayout(false);
            this.tmpBoolPanel.ResumeLayout(false);
            this.tmpStringPanel.ResumeLayout(false);
            this.tmpStringPanel.PerformLayout();
            this.tmpDateNumbPanel.ResumeLayout(false);
            this.tmpDateNumbPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void StoreGroupBuilder_Load(object sender, System.EventArgs e)
		{
			_storeChar = new ArrayList();
			_storeChar = _storeSession.GetStoreCharacteristicList();

			//Load Store Characteristics
			PopulateCharacteristicsListbox();

			if (!(_newGroupLevel))
			{
				//Load Store_Group_Dyn_Join
				PopulateEnglishListbox();

				//Load Store Group Name
				StoreGroupLevelProfile lSG = _storeSession.GetStoreGroupLevel(_SGL_RID); //_SAB.StoreServerSession.GetStoreGroupLevel(_SGL_RID);
				_origGroupName = lSG.Name;
				txtStoreGroupName.Text = _origGroupName;
				
				//Load arraylist of StoresInGroup
				ProfileList lGroupLevelJoin = new ProfileList(eProfileType.Store);
				lGroupLevelJoin = _storeSession.GetStaticStoresInGroup(_SGL_RID);

				//Load "Always Include Stores" section - bottom of screen
				checkSelStoresLoad(lGroupLevelJoin);
			}

//			AllowUpdate = true;  Security changes - 1/24/2005 vg
			//Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
			//SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
			if (_SGL_Seq == int.MaxValue)
			{
				SetReadOnly(false);
			}
			else
			{
				SetReadOnly(FunctionSecurity.AllowUpdate);
			}
			//End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems


			if (FunctionSecurity.AllowUpdate)
			{

				Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreAttributes));
			}
			else
			{

				Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreAttributes));
			}

			Cursor.Current = Cursors.Default;
		}
		
		#region "Populate CheckedListBoxes"
		private void PopulateAllStoresListbox()
		{
			try
			{
				// BEGIN issue 4320 - stodd 2.16.2007
				//==========================================================
				// Get Store Profile Keys and build a datatable with them
				//==========================================================
				ArrayList storeKeys = _storeSession.GetAllStoreKeys();
				_dtAllStores = BuildAllStoreTable();
				foreach (int storeRid in storeKeys)
				{
					DataRow newRow = _dtAllStores.NewRow();
					newRow["ST_RID"] = storeRid;
					_dtAllStores.Rows.Add(newRow);
				}
				_dtAllStores.AcceptChanges();
				// END issue 4320 - stodd 2.16.2007

				//===================================================================
				// Remove Unavailable stores from list.
				// That is, remove Static stores from OTHER sets in this attribute
				//===================================================================
				ProfileList groupLevelList = _storeSession.GetStoreGroupLevelListViewList(_SG_RID);
				ArrayList unavailableStoreList = new ArrayList();
				foreach (StoreGroupLevelListViewProfile sglp in groupLevelList.ArrayList)
				{
					// Skip the level we are working on.
					if (sglp.Key == _SGL_RID)
						continue;
					
					object[] key = new object[1];
					ProfileList storeList = _storeSession.GetStaticStoresInGroup(sglp.Key);
					foreach (StoreProfile sp in storeList.ArrayList)
					{
						key[0] = sp.Key;
						DataRow dr = _dtAllStores.Rows.Find(key);
						if (dr != null)
							_dtAllStores.Rows.Remove(dr);
					}
				}
				_dtAllStores.AcceptChanges();

				// figure out and add store display text to Data table
				foreach (DataRow row in _dtAllStores.Rows)
				{
					int storeRid = Convert.ToInt32(row["ST_RID"],CultureInfo.CurrentUICulture);
					row["Text"] = _storeSession.GetStoreDisplayText(storeRid);
				}

				//Stores DataView
				DataView dvStores;
				dvStores = new DataView();

				//Sort Stores by Text
				dvStores.Table = _dtAllStores;
				dvStores = new DataView(_dtAllStores);
				dvStores.Sort = "Text";

				// BEGIN Issue 4440 - stodd 06.08.2007
				// This filter is no longer needed, store list above contains only active stores.
				// Only show ACTIVE stores
				//dvStores.RowFilter = "ACTIVE_IND = '1'";
				// END Issue 4440 - stodd 06.08.2007

				//Populate StoresAll ListBox with ST_IDs
				clbStoresAll.DataSource = dvStores;	
				clbStoresAll.DisplayMember=  "Text";
				clbStoresAll.ValueMember =  "ST_RID";
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AllStoresPopError ) + "\n" +
					ex.ToString(),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
			}
		}

		// BEGIN issue 4320 - stodd 2.16.2007
		private DataTable BuildAllStoreTable()
		{
			DataTable stores = MIDEnvironment.CreateDataTable("Stores");
			try
			{
				// Add ST_RID column
				DataColumn myColumn = new DataColumn();
				myColumn.AllowDBNull = true; 
				myColumn.Caption = "ST_RID"; 
				myColumn.ColumnName = "ST_RID"; 
				myColumn.DefaultValue = null;
				myColumn.ReadOnly = false;
				myColumn.DataType = System.Type.GetType("System.Int32"); 
				stores.Columns.Add(myColumn);

				// Add Text column
				myColumn = new DataColumn();
				myColumn.AllowDBNull = true; 
				myColumn.Caption = "Text"; 
				myColumn.ColumnName = "Text"; 
				myColumn.DefaultValue = null;
				myColumn.ReadOnly = false;
				myColumn.DataType = System.Type.GetType("System.String"); 
				stores.Columns.Add(myColumn);

				//make Store RID column the primary key
				DataColumn[] PrimaryKeyColumn = new DataColumn[1];
				PrimaryKeyColumn[0] = stores.Columns["ST_RID"];
				stores.PrimaryKey = PrimaryKeyColumn;

				return stores;
			}
			catch
			{
				throw;
			}
		}
		// END issue 4320 - stodd 2.16.2007

		private void PopulateCharacteristicsListbox()
		{
            //Begin TT#1244-MD -jsobek -Error Adding Store Attribute Set
			//Don't show listbox activity
			lbChar.BeginUpdate();

			DataTable dtStoreCharGroup = MIDEnvironment.CreateDataTable();
			//This will change to 2 datatables (_dtAppText & _dtStoreCharGroup)
			//then merge Value and Display to bind LB
			//then properties will can be found with select from datatable...no need for DB calls.
			//Steve's DataTable influence has officially rubbed off on me.

			try
			{
				//Get "Soft Labels" for Stores table columns
				_dtAppText = MIDText.GetTextType(eMIDTextType.eStores, eMIDTextOrderBy.TextValue, eMIDMessageLevel.ListBoxItems);
				// Add the Store Status row
				DataTable dt = MIDText.GetMsg(eMIDTextCode.lbl_StoreStatus);
				foreach (DataRow storeStatusRow in dt.Rows)
				{
					object [] rowArray = storeStatusRow.ItemArray;
					_dtAppText.Rows.Add(rowArray);
				}

				// Unfortunately the TEXT_VALUE_TYPE on the APPLICATION_TEXT uses the eMIDTextValueType, where as
				// Store Dynamic Characterics uses the eStoreCharType definitions.  Because of this we must conver this value
				// on the datatable to get the Store Group Builder to display the correct information.
				ConvertDataTypeEnumeration();

				_dtAppText.PrimaryKey = new DataColumn[] {_dtAppText.Columns["TEXT_CODE"]};
				//DataView dvStoreChar;
				//_dvStoreChar = new DataView();

				//Order Labels as they are shown on the Explorer
				//_dvStoreChar.Table = _dtAppText;
				_dvStoreChar = new DataView(_dtAppText);
				_dvStoreChar.Sort = "TEXT_CODE";	
			
				//Get User defined Characteristics (Store_Char)
				//Add items to listbox
				_dsStoreChar = _storeSession.GetAllStoreCharacteristicsData();
				dtStoreCharGroup = _dsStoreChar.Tables["STORE_CHAR_GROUP"];
			
				//DataTable _dtChar = MIDEnvironment.CreateDataTable();
				_dtChar = MIDEnvironment.CreateDataTable();
				_dtChar.Columns.Add("lbDisplay", typeof(string));
				_dtChar.Columns.Add("lbID", typeof(int));
				_dtChar.Columns.Add("lbIsCharGrp", typeof(bool));
				_dtChar.PrimaryKey = new DataColumn[] {_dtChar.Columns["lbID"],_dtChar.Columns["lbIsCharGrp"] };

				foreach (DataRow dr in _dvStoreChar.Table.Rows)
				{
					DataRow row = _dtChar.NewRow();
					row["lbDisplay"] = dr["TEXT_VALUE"];
					row["lbID"] = dr["TEXT_CODE"];
					row["lbIsCharGrp"] = false;
					_dtChar.Rows.Add(row);
				}

				foreach (DataRow dr in dtStoreCharGroup.Rows)
				{
					DataRow row = _dtChar.NewRow();
					row["lbDisplay"] = dr["SCG_ID"];
					row["lbID"] = dr["SCG_RID"];
					row["lbIsCharGrp"] = true;
					_dtChar.Rows.Add(row);
				}

				_dtChar.DefaultView.Sort = "lbDisplay";
				lbChar.DataSource = _dtChar;
				lbChar.DisplayMember = "lbDisplay";
				lbChar.ValueMember = "lbID";
			}
			catch (Exception err)
			{
				string tmp = err.ToString();
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AllCharPopError ),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{
                lbChar.EndUpdate();
            }
            //End TT#1244-MD -jsobek -Error Adding Store Attribute Set
		}
		#endregion

		private void ConvertDataTypeEnumeration()
		{
			foreach (DataRow row in _dtAppText.Rows)
			{
				if (row["TEXT_VALUE_TYPE"] != DBNull.Value)
				{
					int oldDataType = Convert.ToInt32(row["TEXT_VALUE_TYPE"], CultureInfo.CurrentUICulture);
					switch ((eMIDTextValueType)oldDataType)
					{
						case eMIDTextValueType.String:
							row["TEXT_VALUE_TYPE"] = (int)eStoreCharType.text;
							break;
						case eMIDTextValueType.Boolean:
							row["TEXT_VALUE_TYPE"] = (int)eStoreCharType.boolean;
							break;
						case eMIDTextValueType.Date:
							row["TEXT_VALUE_TYPE"] = (int)eStoreCharType.date;
							break;
						case eMIDTextValueType.Numeric:
							row["TEXT_VALUE_TYPE"] = (int)eStoreCharType.number;
							break;
					}
				}
			}
		}

		#region "Add Checks to clbStoresAll and Populate lbEngSQL"
		private void PopulateEnglishListbox()
		{ 
			lbEngSQL.BeginUpdate();
			try
			{
				//bind datatable to pop_lbEngSQL
				//Add column to Store_Group_Dyn_Join to be able to modify (ValueMember = Text_Code)
				//Then lookup datatype and actual column name from enum & table????
				//CreateStatements();
					
				lbEngSQL.DataSource = _dtStoreGroupLevelStatement;
				//lbEngSQL.DisplayMember	= "SGLS_STATEMENT"; //Will be "ENG_SQL"
				lbEngSQL.DisplayMember	= "SGLS_ENG_SQL";
				lbEngSQL.ValueMember = "SGL_RID".ToString(CultureInfo.CurrentUICulture);
			}
			catch (Exception err)
			{
				string errMsg = err.Message;
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AllENGSQLPopError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{lbEngSQL.EndUpdate();
			HorizontalScrollFix();
			}
		}

		private string AllOperators(eENGSQLOperator engSQLOperator)
		{
			switch (engSQLOperator)
			{
				case eENGSQLOperator.False:
					return "0";
				case eENGSQLOperator.True:	
					return "1";
				case eENGSQLOperator.Equals:
					return "=";
				case eENGSQLOperator.GThan:
					return ">";
				case eENGSQLOperator.LThan:	
					return "<";
				case eENGSQLOperator.Between:
					return "BETWEEN";
				case eENGSQLOperator.NotEqual:
					return "<>";
				case eENGSQLOperator.Like:
					return "LIKE";
				case eENGSQLOperator.In:	
					return "IN";
				case eENGSQLOperator.NotIn:
					return "NOT IN";
				case eENGSQLOperator.And:
					return "AND";
				default:
					return "INVALID";
			}	
		}

		//Listbox's Horizontal scrollbar not showing up -- <QuickHack>
		private void HorizontalScrollFix()
		{			
			_hzSize = 0;
			try
			{
				// Make no partial items are displayed vertically.
				lbEngSQL.IntegralHeight = true;

				// Display a horizontal scroll bar.
				lbEngSQL.HorizontalScrollbar = true;

				// Create a Graphics object to use when determining the size of the largest item in the ListBox.
				Graphics g = lbEngSQL.CreateGraphics();

				//Determine the size for HorizontalExtent using the MeasureString method 
				//using the longest item in the list.
				foreach (DataRow dr in _dtStoreGroupLevelStatement.Rows)
				{
					
					if (g.MeasureString(dr["SGLS_ENG_SQL"].ToString(),lbEngSQL.Font).Width
						> _hzSize)
					{_hzSize = (int)g.MeasureString(dr["SGLS_ENG_SQL"].ToString(),lbEngSQL.Font).Width;}
				}

				//Set the HorizontalExtent property.
				lbEngSQL.HorizontalExtent = _hzSize;

				g.Dispose();
			}
			catch{}
		}

		private void HorizontalScrollFix(DataRow dr)
		{			
			try
			{
				// Make no partial items are displayed vertically.
				lbEngSQL.IntegralHeight = true;

				// Display a horizontal scroll bar.
				lbEngSQL.HorizontalScrollbar = true;

				// Create a Graphics object to use when determining the size of the largest item in the ListBox.
				Graphics g = lbEngSQL.CreateGraphics();

				//Determine the size for HorizontalExtent using the MeasureString method 
				//using the new item in the list.
				if (g.MeasureString(dr["SGLS_ENG_SQL"].ToString(),lbEngSQL.Font).Width
						> _hzSize)
					{_hzSize = (int)g.MeasureString(dr["SGLS_ENG_SQL"].ToString(),lbEngSQL.Font).Width;}

				//Set the HorizontalExtent property.
				lbEngSQL.HorizontalExtent = _hzSize;

				g.Dispose();
			}
			catch{}
		}

		private void checkSelStoresLoad(ProfileList groupLevelJoin)
		{
			Cursor.Current = Cursors.WaitCursor;
			clbStoresAll.BeginUpdate();
			foreach(StoreProfile sp in groupLevelJoin)
			{
				int lInt = clbStoresAll.FindStringExact(sp.Text.ToString(CultureInfo.CurrentUICulture));
				// Begin Issue 4136 stodd
				if (lInt >= 0)
				{
					clbStoresAll.SetSelected(lInt, true);
					clbStoresAll.SetItemCheckState(lInt, CheckState.Checked);
				}
				// end issue 4136
			}
			clbStoresAll.EndUpdate();
			Cursor.Current = Cursors.Default;
		}

		private void RemoveUnavailableStores(ArrayList storeList)
		{
			Cursor.Current = Cursors.WaitCursor;
			clbStoresAll.BeginUpdate();
			foreach(StoreProfile sp in storeList)
			{
				int lInt = clbStoresAll.FindStringExact(sp.StoreId.ToString(CultureInfo.CurrentUICulture));
			
				clbStoresAll.Items.RemoveAt(lInt);
			}
			clbStoresAll.EndUpdate();
			Cursor.Current = Cursors.Default;
		}
		#endregion

		private void cmdSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				ISave();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SaveStoreGroupError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
			this.DialogResult = DialogResult.Cancel;}
		}
		
		protected override bool SaveChanges()
		{
			//========================================
			//Validate group name before save
			//========================================
			if (!(ValidGroupNameText(txtStoreGroupName.Text)))
			{
				txtStoreGroupName.Focus();
				return false;
			}
			//=============================================================================
			// String Sql together into a proger 'where' clause
			//=============================================================================
			string whereSql = string.Empty;
			foreach(DataRow dr in _dtStoreGroupLevelStatement.Rows)
			{
				whereSql += dr["SGLS_STATEMENT"].ToString();
				whereSql += " ";
			}
			//=============================================================================
			// temporarily replace any placeholders with the real char group name
			//=============================================================================
			if (whereSql.IndexOf("*SCGRID__") >= 0)
			{
				whereSql = ReplaceStoreCharGroupPlaceholders(whereSql);
			}
			//=============================================================================
			// Validate for matched parentheses
			//=============================================================================
			if (!ValidateParentheses(whereSql))
			{
				lbEngSQL.Focus();
				return false;
			}
			//===============================================================================
			// Test sql to be sure it's built correctly before saving it (later) to the DB
			//===============================================================================
			if (!_SAB.StoreServerSession.ValidateSQLWhereClause(whereSql))
			{
				string msgText = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidWhereSql);
				msgText = msgText.Replace("{0}",whereSql);
				MessageBox.Show(msgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.Text);

				return false;
			}
			//================================================================================
			// handle change in store group level name 
			//================================================================================
			if (!(_newGroupLevel))
			{
				if (txtStoreGroupName.Text.Trim() != _origGroupName)
				{
					_storeSession.RenameGroupLevel(_SGL_RID, txtStoreGroupName.Text.Trim());
					_newGroupLevelName = txtStoreGroupName.Text.Trim();
				}
				else
					_newGroupLevelName = string.Empty;
			}
			else
				_newGroupLevelName = txtStoreGroupName.Text.Trim();
			//==============================================================================
			// If NEW gstore group level...
			// Add Store Group Level to DB and Store Session
			//==============================================================================
			if (_newGroupLevel)
			{
				_SGL_RID = _storeSession.AddGroupLevel(_SG_RID, txtStoreGroupName.Text);
			}
			//=============================================================================
			// If it's not a new group level, delete any Store Group Level Join records
			//==============================================================================
			if (!(_newGroupLevel))
				_storeSession.DeleteGroupLevelJoin(_SGL_RID);
			//==============================================================================
			// Add any new Store Group Level Join records to DB and to Store Session
			//==============================================================================
			DataView dvStoresSel = new DataView(_dtAllStores);
			dvStoresSel.Sort = "Text";
			int index = 0;

			int [] IDs = new int[lbStoresSel.Items.Count];
			for (int i=0; i<=lbStoresSel.Items.Count - 1; i++)
			{
				index = dvStoresSel.Find(lbStoresSel.Items[i].ToString());
				DataRow row = dvStoresSel[index].Row;
				IDs[i] = Convert.ToInt32(row["ST_RID"], CultureInfo.CurrentUICulture);
			}
			if (IDs.Length > 0)
				_storeSession.AddGroupLevelJoin(IDs,_SGL_RID);
			//=============================================================================
			// If it's not a new group level, delete any Store Group Level Statement records
			//=============================================================================
			if (!(_newGroupLevel))
				_storeSession.DeleteGroupLevelStatement(_SGL_RID);
			else
			{
				//==============================================================
				//Replace 0 (SGL_RID) with new SGL_RID for each row in the dt.
				//==============================================================
				foreach(DataRow dr in _dtStoreGroupLevelStatement.Rows)
				{
					dr["SGL_RID"] = _SGL_RID;
				}
			}

			_isNewStoreChar = false;
			//===================================================================	 
			// Add Store Group Level Statement records to DB and Store Session
			//===================================================================
			_storeSession.AddGroupLevelStatement(_dtStoreGroupLevelStatement);
			// ==================================================================
			// Refresh stores in group
			//===================================================================
			_storeSession.RefreshStoresInGroup(_SG_RID);

			//===================================================================
			// Throws event to add new group level (set) to Store Explorer
			//===================================================================
			if (_newGroupLevel)
			{
				StoreGroupLevelProfile sgl = _storeSession.GetStoreGroupLevel(_SGL_RID);
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//StoreGroupLevelChangeEventArgs eventArgs = new StoreGroupLevelChangeEventArgs(_SG_RID, sgl);
				StoreGroupLevelChangeEventArgs eventArgs = new StoreGroupLevelChangeEventArgs(_SG_Node, sgl);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				if (OnStoreGroupLevelPropertyChangeHandler != null)
				{
					OnStoreGroupLevelPropertyChangeHandler(this, eventArgs);
				}
			}
			else
			{
				StoreGroupLevelProfile sgl = _storeSession.GetStoreGroupLevel(_SGL_RID);
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//StoreGroupLevelChangeEventArgs eventArgs = new StoreGroupLevelChangeEventArgs(_SG_RID, sgl);
				StoreGroupLevelChangeEventArgs eventArgs = new StoreGroupLevelChangeEventArgs(_SG_Node, sgl);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				if (OnRefreshStoreGroupEvent != null)
				{
					OnRefreshStoreGroupEvent(this, eventArgs);
				}
			}
			ChangePending = false;
			this.DialogResult = DialogResult.OK;
			return true;
		}

		/// <summary>
		/// Validates that the parentheses are properly matched
		/// </summary>
		/// <param name="whereSql"></param>
		/// <returns></returns>
		private bool ValidateParentheses(string whereSql)
		{
			bool isValid = true;

			if (whereSql != null)
			{
				int leftCount=0;
				int rightCount=0;
				foreach(char s in whereSql)
				{
					if (s=='(')
						++leftCount;
				}
				foreach(char s in whereSql)
				{
					if (s==')')
						++rightCount;
				}
				if (rightCount != leftCount)
				{
					isValid = false;
					MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_MismatchedParentheses), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			return isValid;
		}

		private string ReplaceStoreCharGroupPlaceholders(string sql)
		{
			string newSql = sql;

			try
			{
				string [] aList = sql.Split(' ');
				foreach (string aToken in aList)
				{
					if (aToken.StartsWith("*SCGRID__"))
					{
						string ridPart = aToken.Remove(0,9);
						int scgRid = Convert.ToInt32(ridPart,CultureInfo.CurrentUICulture);
						string charGroupName = _storeSession.GetStoreCharacteristicGroupID(scgRid);
						charGroupName = charGroupName.Replace("]",@"\]");
						charGroupName = "[" + charGroupName + "]";  // done in case name contains comma
						newSql = newSql.Replace(aToken,charGroupName);
					}
				}
			}
			catch (Exception)
			{
				throw new MIDException (eErrorLevel.severe,	0, "Error processing sql " + sql);				
				//throw;
			}

			return newSql;
		}

		#region "Add All/Remove All buttons for CheckedListBoxes (Characteristics and Stores)"

		//Add all from clbStoresAll to lbStoresSel
		private void cmdAddAll_Click(object sender, System.EventArgs e)
		{	
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				clbStoresAll.BeginUpdate();
				for( int i=0 ; i < clbStoresAll.Items.Count; i++ ) 
 
				{ 
					clbStoresAll.SetSelected(i, true); 
					clbStoresAll.SetItemChecked(i, true); 
				} 
				clbStoresAll.EndUpdate();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddAllError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{Cursor.Current = Cursors.Default;}
		}

		//Add all from clbCharAll to lbCharSel
		private void cmdCharAddAll_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				clbCharAll.BeginUpdate();
				for( int i=0 ; i < clbCharAll.Items.Count; i++ ) 
 
				{ 
					clbCharAll.SetSelected(i, true); 
					clbCharAll.SetItemChecked(i, true); 
				} 
				clbCharAll.EndUpdate();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddAllError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{Cursor.Current = Cursors.Default;}
		}

		//Remove all from lbStoresSel to clbStoresAll
		private void cmdRemoveAll_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				clbStoresAll.BeginUpdate();
				for( int i=0 ; i < clbStoresAll.Items.Count; i++ ) 
				{ 
					clbStoresAll.SetSelected(i, true); 
					clbStoresAll.SetItemChecked(i, false); 
				} 

				clbStoresAll.SetSelected(0, true); 

				clbStoresAll.EndUpdate();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_RemoveAllError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{Cursor.Current = Cursors.Default;}
		}

		//Remove all from lbCharSel to clbCharAll
		private void cmdCharRemoveAll_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				clbCharAll.BeginUpdate();
				for( int i=0 ; i < clbCharAll.Items.Count; i++ ) 
 
				{ 
					clbCharAll.SetSelected(i, true); 
					clbCharAll.SetItemChecked(i, false); 
				} 
				clbCharAll.SetSelected(0, true); 

				clbCharAll.EndUpdate();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_RemoveAllError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{Cursor.Current = Cursors.Default;}
		}
		#endregion

		#region "Single Item Check for CheckedListBoxes (Characteristics and Stores)"
		private void clbStoresAll_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			try
			{
				string clbText = clbStoresAll.Text;
			
				//If item checked add to lbStoresSel otherwise remove from lbStoresSel
				if (e.NewValue == CheckState.Checked)
					lbStoresSel.Items.Add(clbText);
				else
					lbStoresSel.Items.Remove(clbText);

                if (FormLoaded)
                {
                    ChangePending = true;
                }
			}
			
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddToListError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
	
		}

		private void clbCharAll_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			try
			{
				//Obtain reference of selected item
				string clbValue = clbCharAll.SelectedValue.ToString();

				string clbText = clbCharAll.Text;

				//If item checked add to lbCharSel otherwise remove from lbCharSel
				if (e.NewValue == CheckState.Checked)
					lbCharSel.Items.Add(clbText);
				else
					lbCharSel.Items.Remove(clbText);

                if (FormLoaded)
                {
                    ChangePending = true;
                }
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddToListError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
		#endregion

		private void lbChar_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				ArrayList al = new ArrayList();

				string lbvm = lbChar.ValueMember.ToString(CultureInfo.CurrentUICulture);

				GenericLoad(Convert.ToInt32(lbChar.SelectedValue,CultureInfo.CurrentUICulture), eENGSQLOperator.In, al);
			}
			catch (Exception err)
			{
				string tmsStr = err.Message;
			}
		}

		private void GenericLoad(int SGLS_CHAR_ID, eENGSQLOperator anOperator, ArrayList al)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				CharacteristicGroup lStoreCharGroup;
				string lWhere = "";	//Where label for group box
				eStoreTableColumns storeColName = 0;
				_storeCharHasList = false;
				_SGLS_SQL_OPERATOR = anOperator;

				//Determine if Store_Char_Group is selected.  If so, display selection panel.
				object[]findTheseVals = new object[2];
				findTheseVals[0] = SGLS_CHAR_ID;
				findTheseVals[1] = true;

				DataRow drlbChar = _dtChar.Rows.Find(findTheseVals);

				if(drlbChar != null)
				{				
					for (int i=0; i <= _storeChar.Count -1; i++)
					{
						lStoreCharGroup = (CharacteristicGroup)_storeChar[i];
						
						if (drlbChar["lbDisplay"].ToString() == lStoreCharGroup.Name.ToString(CultureInfo.CurrentUICulture))
						{
							_SGLS_CHAR_IND = true;
							lWhere = lStoreCharGroup.Name.ToString(CultureInfo.CurrentUICulture);
							if(lStoreCharGroup.HasList)
								//Show List Panel
							{
								_SCG_RID = lStoreCharGroup.RID;
								_SGLS_CHAR_ID = lStoreCharGroup.RID;
								//_SCG_NAME = lStoreCharGroup.Name.Replace(" ","_");
								_SCG_NAME = lStoreCharGroup.Name;
								InitializeCharPanel(lStoreCharGroup, anOperator, al);
								pnlCharacteristics.BringToFront();
								_SGLS_DT = eStoreCharType.list;
								_storeCharHasList = true;
								break;
							}
							else
							{
								//Show panel by data type
								_storeCharType = lStoreCharGroup.DataType;
								_SCG_RID = lStoreCharGroup.RID;
								_SGLS_CHAR_ID = lStoreCharGroup.RID;
								//_SCG_NAME = lStoreCharGroup.Name.Replace(" ","_");
								_SCG_NAME = lStoreCharGroup.Name;
								_dtStoreCharGroup = _storeSession.GetAllStoreCharacteristicsData().Tables["STORE_CHAR_GROUP"];
								_SGLS_DT = lStoreCharGroup.DataType;
							}	
						}
					}
				} 
				else if (SGLS_CHAR_ID == (int)eMIDTextCode.lbl_StoreStatus)
				{
					//Get the DataRow that corresponds with the selected lbChar item
					_SGLS_CHAR_IND = false;
					DataRow dr;
					dr = _dtAppText.Rows.Find(SGLS_CHAR_ID);
					_SGLS_CHAR_ID = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
					//_SGLS_DT = (eStoreCharType)(Convert.ToInt32(dr["TEXT_VALUE_TYPE"], CultureInfo.CurrentUICulture));
					_storeCharHasList = true;
					lWhere = dr["TEXT_VALUE"].ToString();
					DataTable dtValueList = MIDText.GetLabels((int)eStoreStatus.New, (int)eStoreStatus.Preopen);
					InitializeCharPanelForStoreStatus(dtValueList, anOperator, al);
					pnlCharacteristics.BringToFront();
					_SGLS_DT = eStoreCharType.list;
				}
				else
				{		
					//Get the DataRow that corresponds with the selected lbChar item
					_SGLS_CHAR_IND = false;
					DataRow dr;
					dr = _dtAppText.Rows.Find(lbChar.SelectedValue);
					_SGLS_CHAR_ID = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
					storeColName = (eStoreTableColumns)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					_SGLS_DT = (eStoreCharType)(Convert.ToInt32(dr["TEXT_VALUE_TYPE"], CultureInfo.CurrentUICulture));
					lWhere = dr["TEXT_VALUE"].ToString();
				}
		
				//Chg groupBox Where label.
				groupBox1.Text = "Where " + lWhere;

				if (!_storeCharHasList)
				{
					switch ( _SGLS_DT )
					{
						case eStoreCharType.boolean:
							InitializeBoolPanel(storeColName);
							break;
						case eStoreCharType.text:
							InitializeStringPanel(storeColName);
							break;
						case eStoreCharType.number:
							InitializeNumberPanel(storeColName);
							break;
						case eStoreCharType.dollar:
							InitializeNumberPanel(storeColName);
							break;
						case eStoreCharType.date:
							InitializeDatePanel(storeColName);
							break;
					}
				}

				//Show groupBox1
				if (groupBox1.Visible == false)
					groupBox1.Visible = true;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{Cursor.Current = Cursors.Default;}
}
		
		#region "GroupBox Methods"

		private void ShowPanelList(ArrayList rALStoreCharVal, DataCommon.eStoreCharType aStoreCharType,ArrayList al)
		{
			//Bind arraylist to listbox
			try
			{
				// this is all done to get rid of the time part of the datetime when
				// the data type is date.
				if (aStoreCharType == eStoreCharType.date)
				{
					foreach (StoreCharValue scv in rALStoreCharVal)
					{
						if (scv.CharValue.GetType() == typeof(System.DateTime))
						{
							DateTime tempDate = (DateTime)scv.CharValue;
							scv.CharValue = tempDate.ToString("MM/dd/yyyy");
						}
					}
				}
				
				clbCharAll.DataSource = rALStoreCharVal;
				clbCharAll.DisplayMember = "CharValue";
				clbCharAll.ValueMember = "SC_RID";
				
				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)aStoreCharType;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.True;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = 0;

				cmdAdd.Tag = lAddTag;

				if (al.Count > 0)
				{
					for (int i=0; i <= al.Count -1; i++)
					{
						int scRid = Convert.ToInt32(al[i], CultureInfo.CurrentUICulture);
						foreach (StoreCharValue scv in rALStoreCharVal)
						{
							if (scv.SC_RID == scRid)
							{
								int vIndex = clbCharAll.Items.IndexOf(scv);
								clbCharAll.SetSelected(vIndex,true);
								clbCharAll.SetItemChecked(vIndex,true);
								break;
							}
						}
					}
				}
			}	

			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		
		}

		private void ShowPanelListForStoreStatus(DataTable dtValueList, DataCommon.eStoreCharType aStoreCharType,ArrayList al)
		{
			//Bind arraylist to listbox
			try
			{
				clbCharAll.DataSource = dtValueList;
				clbCharAll.DisplayMember = "TEXT_VALUE";
				clbCharAll.ValueMember = "TEXT_CODE";
				
				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)aStoreCharType;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.True;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = 0;

				cmdAdd.Tag = lAddTag;

				if (al.Count > 0)
				{
					for (int i=0; i <= al.Count -1; i++)
					{
						int aStatus = Convert.ToInt32(al[i], CultureInfo.CurrentUICulture);
						foreach (DataRow aRow in dtValueList.Rows)
						{
							int aListStatus = Convert.ToInt32(aRow["TEXT_CODE"],CultureInfo.CurrentUICulture);
							string statusText = aRow["TEXT_VALUE"].ToString();
							if (aListStatus == aStatus)
							{
								int vIndex = clbCharAll.FindStringExact(statusText);
								clbCharAll.SetSelected(vIndex,true);
								clbCharAll.SetItemChecked(vIndex,true);
								break;
							}
						}
					}
				}
			}	

			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		
		}

		#region "Initialize Panels"
		private void InitializeBoolPanel(eStoreTableColumns ColName)
		{
			try
			{
				//Show Panel
				if (tmpBoolPanel.Visible == false)
					tmpBoolPanel.Visible = true;
				clearBoolVals();
				tmpBoolPanel.BringToFront();

				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)eStoreCharType.boolean;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = (int)ColName;

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineBoolCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
		private void InitializeBoolPanel(DataRow dr)
		{
			try
			{
				//Show Panel
				if (tmpBoolPanel.Visible == false)
					tmpBoolPanel.Visible = true;
				clearBoolVals();

				lbChar.SelectedIndex = lbChar.FindStringExact(GetTextValue((eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture))));
				
				if (dr["SGLS_VALUE"].ToString() == "1")																					
					boolTrueButton.Checked = true;
				else
					boolFalseButton.Checked = true;

				tmpBoolPanel.BringToFront();
				
				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)eStoreCharType.boolean;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineBoolCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void InitializeStringPanel(eStoreTableColumns ColName)
		{
			try
			{
				//Show Panel
				if (tmpStringPanel.Visible == false)
					tmpStringPanel.Visible = true;
				clearStringlVals();
				tmpStringPanel.BringToFront();

				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)eStoreCharType.text;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = (int)ColName;

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineStringCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
		private void InitializeStringPanel(DataRow dr)
		{
			try
			{
				//Show Panel
				if (tmpStringPanel.Visible == false)
					tmpStringPanel.Visible = true;
				clearStringlVals();

				if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)
				{
					string CharGroupName = _storeSession.GetStoreCharacteristicGroupID(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
					lbChar.SelectedIndex = lbChar.FindStringExact(CharGroupName);
					this.txtString.Text = ConvertCharacteristicRidToValue(eStoreCharType.text, dr["SGLS_VALUE"].ToString()); 
				}
				else
				{
					lbChar.SelectedIndex = lbChar.FindStringExact(GetTextValue((eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture))));
					this.txtString.Text = dr["SGLS_VALUE"].ToString();
				}

				if ((eENGSQLOperator)(Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture)) == eENGSQLOperator.Equals)																					
					stringEqualButton.Checked = true;
				else
					stringNotEqualButton.Checked = true;

				tmpStringPanel.BringToFront();

				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)eStoreCharType.text;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);

				cmdAdd.Tag = lAddTag;
			}
			catch (Exception err)
			{ string tmpStr = err.Message;
			MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineStringCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}


		private void InitializeNumberPanel(eStoreTableColumns ColName)
		{
			try
			{
				//Don't show calendar buttons
				cmdCompareDate.Visible = false;
				cmdCompareDateB1.Visible = false;
				cmdCompareDateB2.Visible = false;

				// since these are share with the date panel, make sure they are enterable
				txtCompareDateNumb.ReadOnly = false;
				txtBetweenDateNumb1.ReadOnly = false;
				txtBetweenDateNumb2.ReadOnly = false;

				//Clear/Set TextBox Values
				clearDateNumbVals();

				//Resize lblDateNumbAND label to be centered without calendar buttons
                lblDateNumbAND.Location = new Point(72, 79); //TT#684-MD-VStuart-Store Attribute [StoreGroupBuilder]
				//This isn't working!!
				lblDateNumbAND.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			
				//Show Panel
				if (tmpDateNumbPanel.Visible == false)
					tmpDateNumbPanel.Visible = true;
				tmpDateNumbPanel.BringToFront();

				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)_SGLS_DT;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = (int)ColName;

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineNumericCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
		private void InitializeNumberPanel(DataRow dr)
		{
			try
			{
				//Don't show calendar buttons
				cmdCompareDate.Visible = false;
				cmdCompareDateB1.Visible = false;
				cmdCompareDateB2.Visible = false;

				// since these are share with the date panel, make sure they are enterable
				txtCompareDateNumb.ReadOnly = false;
				txtBetweenDateNumb1.ReadOnly = false;
				txtBetweenDateNumb2.ReadOnly = false;

				//Clear/Set TextBox Values
				clearDateNumbVals();

				//Resize lblDateNumbAND label to be centered without calendar buttons
                lblDateNumbAND.Location = new Point(72, 79); //TT#684-MD-VStuart-Store Attribute [StoreGroupBuilder]
				//This isn't working!!
				lblDateNumbAND.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			
				//Show Panel
				if (tmpDateNumbPanel.Visible == false)
					tmpDateNumbPanel.Visible = true;

				object aValue = null;
				eENGSQLOperator sqlOperator = (eENGSQLOperator)(Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
				if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)  
				{
					// this value could be in the form of value,value.
					string[] strVals = Split(dr["SGLS_VALUE"].ToString(), sqlOperator);
					string CharGroupName = _storeSession.GetStoreCharacteristicGroupID(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
					lbChar.SelectedIndex = lbChar.FindStringExact(CharGroupName);
					aValue = ConvertCharacteristicRidToValue((eStoreCharType)(Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture)), strVals[0]); 
					
					if (sqlOperator == eENGSQLOperator.Between)
					{
						if (strVals.Length == 2)
						{
							object secValue = ConvertCharacteristicRidToValue((eStoreCharType)(Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture)), strVals[1]); 
							aValue += "," + secValue;
						}
					}
				}
				else
				{
					lbChar.SelectedIndex = lbChar.FindStringExact(GetTextValue((eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture))));
					aValue = dr["SGLS_VALUE"];
				}
				
				if (sqlOperator == eENGSQLOperator.Equals)																					
				{
					dateNumbEqualButton.Checked = true;
					txtCompareDateNumb.Text = aValue.ToString();
				}
				else if (sqlOperator == eENGSQLOperator.GThan)
				{
					dateNumbGThanButton.Checked = true;
					txtCompareDateNumb.Text = aValue.ToString();
				}
				else if (sqlOperator == eENGSQLOperator.LThan)
				{
					dateNumbLThanButton.Checked = true;
					txtCompareDateNumb.Text = aValue.ToString();
				}
				else if (sqlOperator == eENGSQLOperator.Between)
				{
					string[] strVals = Split(aValue.ToString(), sqlOperator);
					txtBetweenDateNumb1.Text = strVals[0];
					txtBetweenDateNumb2.Text = strVals[1];
					dateNumbBetweenButton.Checked = true;	
				}
				
				tmpDateNumbPanel.BringToFront();

				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture));
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineNumericCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}


		private void InitializeDatePanel(eStoreTableColumns ColName)
		{
			try
			{
				//Show calendar buttons
				cmdCompareDate.Visible = true;
				cmdCompareDateB1.Visible = true;
				cmdCompareDateB2.Visible = true;

				// Can only use date selection button right now
				txtCompareDateNumb.ReadOnly = true;
				txtBetweenDateNumb1.ReadOnly = true;
				txtBetweenDateNumb2.ReadOnly = true;

				//Clear/Set TextBox Values
				clearDateNumbVals();

				//Resize lblDateNumbAND label to be centered without calendar buttons
                lblDateNumbAND.Location = new Point(96, 79); //TT#684-MD-VStuart-Store Attribute [StoreGroupBuilder]
				lblDateNumbAND.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

				//Show Panel
				if (tmpDateNumbPanel.Visible == false)
					tmpDateNumbPanel.Visible = true;
				tmpDateNumbPanel.BringToFront();	
		
				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)eStoreCharType.date;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = (int)ColName;

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineDateCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
		private void InitializeDatePanel(DataRow dr)
		{
			try
			{
				//Show calendar buttons
				cmdCompareDate.Visible = true;
				cmdCompareDateB1.Visible = true;
				cmdCompareDateB2.Visible = true;

				// Can only use date selection button right now
				txtCompareDateNumb.ReadOnly = true;
				txtBetweenDateNumb1.ReadOnly = true;
				txtBetweenDateNumb2.ReadOnly = true;

				//Clear/Set TextBox Values
				clearDateNumbVals();

				//Resize lblDateNumbAND label to be centered without calendar buttons
                lblDateNumbAND.Location = new Point(96, 79); //TT#684-MD-VStuart-Store Attribute [StoreGroupBuilder]
				lblDateNumbAND.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

				//Show Panel
				if (tmpDateNumbPanel.Visible == false)
					tmpDateNumbPanel.Visible = true;

				string SGLS_VALUE = dr["SGLS_VALUE"].ToString();
				eENGSQLOperator sqlOperator = (eENGSQLOperator)(Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
				if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)
				{
					string aName = _storeSession.GetStoreCharacteristicGroupID(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
					lbChar.SelectedIndex = lbChar.FindStringExact(aName);
				}
				else
					lbChar.SelectedIndex = lbChar.FindStringExact(GetTextValue((eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture))));
				//lbChar.SelectedIndex = lbChar.FindStringExact(GetSGLSCharIDString(dr).ToString());
				
				if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)
				{
					string[] strVals = Split(SGLS_VALUE, sqlOperator);
					string tempSglsValue1;

					SGLS_VALUE = ConvertCharacteristicRidToValue(eStoreCharType.date, strVals[0]); 

					if (sqlOperator == eENGSQLOperator.Between)
					{
						if (strVals.Length == 2)
						{
							tempSglsValue1 = ConvertCharacteristicRidToValue(eStoreCharType.date, strVals[1]); 
							SGLS_VALUE = SGLS_VALUE + "," + tempSglsValue1;
						}
					}
				}

				if (sqlOperator == eENGSQLOperator.Equals)																					
				{
					dateNumbEqualButton.Checked = true;
					txtCompareDateNumb.Text = SGLS_VALUE;
				}
				else if (sqlOperator == eENGSQLOperator.GThan)
				{
					dateNumbGThanButton.Checked = true;
					txtCompareDateNumb.Text = SGLS_VALUE;
				}
				else if (sqlOperator == eENGSQLOperator.LThan)
				{
					dateNumbLThanButton.Checked = true;
					txtCompareDateNumb.Text = SGLS_VALUE;
				}
				else if (sqlOperator == eENGSQLOperator.Between)
				{
					string[] strVals = Split(SGLS_VALUE, sqlOperator);
					this.txtBetweenDateNumb1.Text = strVals[0];
					this.txtBetweenDateNumb2.Text = strVals[1];
					dateNumbBetweenButton.Checked = true;	
				}
			
				tmpDateNumbPanel.BringToFront();	
		
				int[] lAddTag = new int[addTagArrayLength];
				lAddTag[(int)eAddTagArrayDef.eStoreCharType] = (int)eStoreCharType.date;
				lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] = (int)eIsStore_Char_Group.False;
				//lAddTag[(int)eAddTagArrayDef.eStoreTableColumns] = (int)ColName;

				cmdAdd.Tag = lAddTag;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineDateCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void InitializeCharPanel(CharacteristicGroup lStoreCharGroup, eENGSQLOperator anOperator, ArrayList al)
		{
			try
			{
				//Clear Panel
				lbCharSel.Items.Clear();
				if (anOperator == eENGSQLOperator.In)
				{
					charInButton.Checked = true;
					charNotInButton.Checked = false;
				}
				else
				{
					charInButton.Checked = false;
					charNotInButton.Checked = true;
				}

				// Set selected characteristic
				lbChar.SelectedIndex = lbChar.FindStringExact(lStoreCharGroup.Name);

				//Show Panel
				ShowPanelList(lStoreCharGroup.Values, lStoreCharGroup.DataType, al);
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineUDCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}	
		}

		private void InitializeCharPanelForStoreStatus(DataTable dtValueList, eENGSQLOperator anOperator, ArrayList al)
		{
			try
			{
				//Clear Panel
				lbCharSel.Items.Clear();
				if (anOperator == eENGSQLOperator.In)
				{
					charInButton.Checked = true;
					charNotInButton.Checked = false;
				}
				else
				{
					charInButton.Checked = false;
					charNotInButton.Checked = true;
				}

				// Set selected characteristic
				lbChar.SelectedIndex = lbChar.FindStringExact(_storeSession.StoreStatusText);

				//Show Panel
				ShowPanelListForStoreStatus(dtValueList, eStoreCharType.text, al);
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DetermineUDCharError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}	
		}

		#endregion

		#region "Clear Panel Controls"
		private void clearDateNumbVals()
		{
			try
			{
				//clear text boxes
				txtCompareDateNumb.Clear();
				txtBetweenDateNumb1.Clear();
				txtBetweenDateNumb2.Clear();

				//clear radio buttons
				dateNumbBetweenButton.Checked = false;
				dateNumbLThanButton.Checked = false;
				dateNumbGThanButton.Checked = false;
				dateNumbEqualButton.Checked = false;
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_ClearValuesError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void clearBoolVals()
		{
			//clear radio buttons
			boolTrueButton.Checked = false;
			boolFalseButton.Checked = false;
		}

		private void clearStringlVals()
		{
			//clear radio buttons
			stringEqualButton.Checked = false;
			stringNotEqualButton.Checked = false;

			//clear text boxes
			txtString.Clear();
		}
		#endregion

		#region "Date Button/Text methods"
		private void cmdCompareDate_Click(object sender, System.EventArgs e)
		{
			DateFill(txtCompareDateNumb);
		}

		private void cmdCompareDateB1_Click(object sender, System.EventArgs e)
		{
			DateFill(txtBetweenDateNumb1);
		}

		private void cmdCompareDateB2_Click(object sender, System.EventArgs e)
		{

			DateFill(txtBetweenDateNumb2);
			validateBetweenDates(eStoreCharType.date);
		}

		private void DateFill(TextBox txtToFill)
		{
			try
			{
				DateTime dtSelectedDate = new DateTime();
				Cursor.Current = Cursors.WaitCursor;
				DateSelectorSingle frm = new DateSelectorSingle();
				frm.SelectedDate = dtSelectedDate;
				frm.ShowDialog();
				Cursor.Current = Cursors.Default;
				dtSelectedDate = frm.SelectedDate;
				txtToFill.Text = dtSelectedDate.ToString("MM/dd/yyyy", CultureInfo.CurrentUICulture);
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DateFillError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
		#endregion

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Add line to lblEngSQL
				int [] lAddTag = new int[addTagArrayLength];
				lAddTag = (int[])cmdAdd.Tag;

			
				if (lAddTag[(int)eAddTagArrayDef.eIsStore_Char_Group] == 1)
				{
					addCharInNotInClause();
				}
				else
				{
					switch (lAddTag[(int)eAddTagArrayDef.eStoreCharType])
					{	
						case (int)eStoreCharType.date: 
							if (ValidateDatePanel())
								addDateNumbWhereClause((eStoreCharType)lAddTag[(int)eAddTagArrayDef.eStoreCharType], (eStoreTableColumns)lAddTag[(int)eAddTagArrayDef.eStoreTableColumns]);
							else
								_error = true;
							break;
						case (int)eStoreCharType.dollar:
							if (ValidateDollarPanel())
								addDateNumbWhereClause((eStoreCharType)lAddTag[(int)eAddTagArrayDef.eStoreCharType], (eStoreTableColumns)lAddTag[(int)eAddTagArrayDef.eStoreTableColumns]);
							else
								_error = true;
							break;
						case (int)eStoreCharType.number:
							if (ValidateNumberPanel())
								addDateNumbWhereClause((eStoreCharType)lAddTag[(int)eAddTagArrayDef.eStoreCharType], (eStoreTableColumns)lAddTag[(int)eAddTagArrayDef.eStoreTableColumns]);
							else
								_error = true;
							break;
						case (int)eStoreCharType.boolean:
							//Validate a radio button is checked
							if (ValidateBooleanPanel())
								addBoolWhereClause((eStoreTableColumns)lAddTag[(int)eAddTagArrayDef.eStoreTableColumns]);
							else
								_error = true;
							break;
						case (int)eStoreCharType.text:
							//Validate Textbox
							if (ValidateStringPanel())
								addStringWhereClause((eStoreTableColumns)lAddTag[(int)eAddTagArrayDef.eStoreTableColumns]);
							else
								_error = true;
							break;
					}
				}

                if (FormLoaded)
                {
                    ChangePending = true;
                }

				//HorizontalScrollFix();
			}
			catch (Exception ex)
			{	string tmp = ex.Message;
			MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
			finally
			{
				// if no errors, make group box disappear.
				if (!_error)
					groupBox1.Visible = false;
				_error = false;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if(_isNewStoreChar)
			{
				//delete SC_RID? or leave it bcuz someone else may have already used it
				//since we created it.??????????
			}
			//Needs are you sure you want to cancel verification.
			this.Close();
		}
 
		private void addDateNumbWhereClause(eStoreCharType DataTypeGeneric, eStoreTableColumns ColName)
		{
			try
			{		
	



				if (_SGLS_SQL_OPERATOR != eENGSQLOperator.Between)
				{
					_SGLS_DT = DataTypeGeneric;
					_SGLS_VALUE = txtCompareDateNumb.Text;
				}
				else
				{
					_SGLS_DT = DataTypeGeneric;
					_SGLS_VALUE = txtBetweenDateNumb1.Text + "," + txtBetweenDateNumb2.Text;
				}
	
				if (!_isModify)
					EnglishListboxStatementAdd();
				else
					EnglishListboxStatementModify();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}

		}

		private void addBoolWhereClause(eStoreTableColumns ColName)
		{
			try
			{
				if (!_isModify)
					EnglishListboxStatementAdd();
				else
					EnglishListboxStatementModify();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void addStringWhereClause(eStoreTableColumns ColName)
		{
			try
			{
				_SGLS_VALUE = txtString.Text.Trim();

				if (!_isModify)
					EnglishListboxStatementAdd();
				else
					EnglishListboxStatementModify();
			}
			catch (Exception err)
			{string tmp = err.Message;
			MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}
 
		private void addCharInNotInClause()
		{
			_error = false;
			try
			{
				if (charNotInButton.Checked == false && charInButton.Checked == false)
				{
					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SQLOperatorError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
					_error = true;
				}
								
				StringBuilder sbSel = new StringBuilder();
				_SGLS_VALUE_CHAR_RID = "";

				//=======================================================
				//loop through clbCharAll gathering all checked items
				//=======================================================
				if (clbCharAll.CheckedItems.Count > 0)
				{
					int i = 0;
					try
					{
						if (_SGLS_CHAR_ID == (int)eMIDTextCode.lbl_StoreStatus)
						{
							foreach(DataRowView dRow in clbCharAll.CheckedItems) 
							{
								if (i != 0)
								{
									sbSel.Append(",");
									_SGLS_VALUE_CHAR_RID += ",";
								}

								sbSel.Append(dRow["TEXT_VALUE"].ToString());
								_SGLS_VALUE_CHAR_RID += Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
								i++;
							}
						}
						else
						{
							foreach(StoreCharValue scv in clbCharAll.CheckedItems) 
							{
								if (i != 0)
								{
									sbSel.Append(",");
									_SGLS_VALUE_CHAR_RID += ",";
								}

								sbSel.Append(scv.CharValue.ToString());
								_SGLS_VALUE_CHAR_RID += scv.SC_RID;
								i++;
							}
						}
					}
					catch{} //blows up ?? "An enumerator can only be used if the list doesn't change"

					_SGLS_VALUE = sbSel.ToString();

					if (!_error)
					{
						if (!_isModify)
							EnglishListboxStatementAdd();
						else
							EnglishListboxStatementModify();
					}

				}
			}
			catch (Exception err)
			{ string tmpStr = err.Message;
				_error = true;
			MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void genericRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (sender == dateNumbEqualButton || sender == stringEqualButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.Equals);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.Equals];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.Equals;
				}
				else if (sender == dateNumbGThanButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.GThan);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.GThan];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.GThan;
				}
				else if (sender == dateNumbLThanButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.LThan);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.LThan];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.LThan;
				}
				else if (sender == dateNumbBetweenButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.Between);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.Between];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.Between;
				}
				else if (sender == boolTrueButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.Equals);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.Equals];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.Equals;
					_SGLS_VALUE = AllOperators(eENGSQLOperator.True);  //eENGSQLOperator.True.ToString();
					_SGLS_DT = eStoreCharType.boolean;
				}
				else if (sender == boolFalseButton)
				{
					//_SGLS_VALUE 
					_SQLOperator = AllOperators(eENGSQLOperator.Equals);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.Equals];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.Equals;
					_SGLS_VALUE =  AllOperators(eENGSQLOperator.False); //eENGSQLOperator.False.ToString();
					_SGLS_DT = eStoreCharType.boolean;
				}
				else if (sender == stringNotEqualButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.NotEqual);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.NotEqual];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.NotEqual;
				}
				else if (sender == charNotInButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.NotIn);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.NotIn];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.NotIn;
				}
				else if (sender == charInButton)
				{
					_SQLOperator = AllOperators(eENGSQLOperator.In);
					//_SQLOperator = allOperators[(int)eENGSQLOperator.In];
					_SGLS_SQL_OPERATOR = eENGSQLOperator.In;
				}
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_RadioButtonChangeError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		/// <summary>
		///  Modify Line in english listbox
		/// </summary>
		private void EnglishListboxStatementModify()
		{
			bool changed = false;
			DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));
			eENGSQLOperator sqlOperator = (eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture);

			if (sqlOperator != _SGLS_SQL_OPERATOR)
			{
				dr["SGLS_SQL_OPERATOR"] = _SGLS_SQL_OPERATOR;
				changed = true;
			}
			

			if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)
			{
				object[]findTheseVals = new object[2];
				findTheseVals[0] = lbChar.SelectedValue;;
				findTheseVals[1] = true;
				DataRow dr2 = this._dtChar.Rows.Find(findTheseVals);
				
				int scgRid = Convert.ToInt32(dr2["lbID"].ToString());
				eStoreCharType groupDataType = _storeSession.GetStoreCharacteristicDataType(scgRid);
				// IF this is a between selection, the value you looks like:  value,value
				// so we split it apart.  If it's not a between valueArray[0] will be null.
				string[] valueArray = Split(_SGLS_VALUE, sqlOperator);

				for (int i=0;i<valueArray.Length;i++)
				{
					if (_SGLS_DT == eStoreCharType.date
						|| (_SGLS_DT == eStoreCharType.list && groupDataType == eStoreCharType.date))
					{
						string toDateCharRid = ConvertDateToCharateristicRid(dr2["lbDisplay"].ToString(), valueArray[i]);
						if (i==0)
							_SGLS_VALUE_CHAR_RID = toDateCharRid;
						else
							_SGLS_VALUE_CHAR_RID += "," + toDateCharRid;
					}
					else
					{
						string toValueCharRid = ConvertValueToCharateristicRid(dr2["lbDisplay"].ToString(), valueArray[i]);
						if (i==0)
							_SGLS_VALUE_CHAR_RID = toValueCharRid;
						else
							_SGLS_VALUE_CHAR_RID += "," + toValueCharRid;
					}
				}

				if (dr["SGLS_VALUE"].ToString() != _SGLS_VALUE_CHAR_RID)
				{
					dr["SGLS_VALUE"] = _SGLS_VALUE_CHAR_RID;
					changed = true;
				}
			}
			else if (dr["SGLS_VALUE"].ToString() != _SGLS_VALUE)
			{
				if (_SGLS_CHAR_ID == (int)eMIDTextCode.lbl_StoreStatus)
				{
					dr["SGLS_VALUE"] = _SGLS_VALUE_CHAR_RID;
				}
				else
				{
					dr["SGLS_VALUE"] = _SGLS_VALUE;
				}
				changed = true;

			}

			if (changed)
			{
				ModifySqlStatement(dr);

				lbEngSQL.Refresh();
			}
			_isModify = false;
			cmdAdd.Text = "&Add";
	}

		private int GetStoreStatusCode(string storeStatusValue)
		{
			int textCode = Include.Undefined;

			DataRow [] rows = _dtStoreStatus.Select("TEXT_VALUE = '" + storeStatusValue + "'");
			if (rows.Length > 0)
			{
				DataRow row = rows[0];
				textCode = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
			}

			return textCode;
		}


		/// <summary>
		/// Add line to english listbox.
		/// also adds new row ro _dtStoreGroupLevelStatement.
		/// </summary>
		private void EnglishListboxStatementAdd()
		{
			try
			{
				//New DataRow
				DataRow row = this._dtStoreGroupLevelStatement.NewRow();

				//If New Group Level, No need for SQL_RID (We'll get it on insert)
				if (_newGroupLevel)
					row["SGL_RID"] = 0;
				else
					//Otherwise get current SGL_RID
					row["SGL_RID"] = _SGL_RID;

				if (_SGLS_CHAR_IND)
				{
					// a List already has the right values in _SGLS_VALUE and _SGLS_VALUE_CHAR_RID.
					// but if its not a list, we need to do this work.
					if (_SGLS_DT != eStoreCharType.list)
					{
						object[]findTheseVals = new object[2];
						findTheseVals[0] = lbChar.SelectedValue;
						findTheseVals[1] = true;
						DataRow dr = this._dtChar.Rows.Find(findTheseVals);

						// IF this is a between selection, the value you looks like:  value,value
						// so we split it apart.  If it's not a between valueArray[0] will be null.
						string[] valueArray = Split(_SGLS_VALUE, _SGLS_SQL_OPERATOR);

						if (_SGLS_DT == eStoreCharType.date)
						{
							_SGLS_VALUE_CHAR_RID = ConvertDateToCharateristicRid(dr["lbDisplay"].ToString(), valueArray[0]);

							if (this._SGLS_SQL_OPERATOR == eENGSQLOperator.Between && valueArray.Length == 2)
							{
								string toDateCharRid = ConvertDateToCharateristicRid(dr["lbDisplay"].ToString(), valueArray[1]);
								_SGLS_VALUE_CHAR_RID += "," + toDateCharRid;
							}
						}
						else
						{
							_SGLS_VALUE_CHAR_RID = ConvertValueToCharateristicRid(dr["lbDisplay"].ToString(), valueArray[0]);

							if (this._SGLS_SQL_OPERATOR == eENGSQLOperator.Between && valueArray.Length == 2)
							{
								string toValueCharRid = ConvertValueToCharateristicRid(dr["lbDisplay"].ToString(), valueArray[1]);
								_SGLS_VALUE_CHAR_RID += "," + toValueCharRid;
							}
						}
					}
				}

				//If 0 rows in ListBox, start count at 1, otherwise start at count
				int lbEngCount = 0;
				if (lbEngSQL.Items.Count == 0)
					lbEngCount = 1;
				else
					lbEngCount = lbEngSQL.Items.Count+1;
				
				//Count = Sequence (For Add)
				row["SGLS_STATEMENT_SEQ"] = lbEngCount;
			
				//If > 1st item, append AND as prefix (could be "soft" in case OR should be default)
				if (lbEngSQL.Items.Count >= 1)
				{
					row["SGLS_PREFIX"] = MIDText.GetTextOnly((int)eENGSQLOperator.And);
					//row["SGLS_PREFIX"] = allOperators[(int)eENGSQLOperator.And];
					row["SGLS_STATEMENT"] = ConfigureSqlStatement(true).Trim();
					row["SGLS_ENG_SQL"] = ConfigureEnglishStatement(true).Trim();
				}
				else
				{
					row["SGLS_STATEMENT"] = ConfigureSqlStatement(false).Trim();
					row["SGLS_ENG_SQL"] = ConfigureEnglishStatement(false).Trim();
				}

				InitializeRow(row);

				_dtStoreGroupLevelStatement.Rows.Add(row);

				//If this is a new group level and the first item to be added to the
				//EngSQL listbox, then we must bind it now.
				if (_newGroupLevel && lbEngCount == 1)
					PopulateEnglishListbox();
				else
					HorizontalScrollFix(row);
			}
			catch (Exception ex)
				{string tmp = ex.Message;
			MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}

		}

		/// <summary>
		/// takes a date string value in the format MM/DD/YYYY and returns a characteristic RID
		/// for it.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private string ConvertDateToCharateristicRid(string charGroup, string aDateString)
		{
			int dYear = Convert.ToInt32( aDateString.Substring(6,4), CultureInfo.CurrentUICulture );
			int dDay = Convert.ToInt32( aDateString.Substring(3,2), CultureInfo.CurrentUICulture );
			int dMonth = Convert.ToInt32( aDateString.Substring(0,2), CultureInfo.CurrentUICulture );
			DateTime aDate = new DateTime(dYear, dMonth, dDay);

			int scRid = _storeSession.StoreCharExists(charGroup, aDate);
			if (scRid == 0)
			{
				_storeSession.OpenUpdateConnection();
				scRid = _storeSession.AddStoreChar(charGroup, aDate);
				_storeSession.CommitData();
				_storeSession.CloseUpdateConnection();
			}

			return scRid.ToString();
		}


		/// <summary>
		/// the SGLS_VALUE field for a store dynamic charactertic contains the RID of the value, not the real value.
		/// This method converts those RID(S) to real values.
		/// </summary>
		/// <param name="valueRid"></param>
		/// <returns></returns>
		private string ConvertCharacteristicRidToValue(eStoreCharType dataType, string valueString)
		{
			string[] strVals = valueString.Split(new char[]{','});
			string aValue = "";
			for (int i=0;i<strVals.Length;i++)
			{
				int scRid = Convert.ToInt32(strVals[i], CultureInfo.CurrentUICulture);
				object aObject = _storeSession.GetStoreCharacteristicValue(scRid);
				if (dataType == eStoreCharType.date)
				{
					// if it's a dateTime we need to change it to mm/dd/yyyyy
					aObject = ((DateTime)aObject).ToString("MM/dd/yyyy");
				}
				if (i==0)
					aValue = aObject.ToString();
				else
					aValue += "," + aObject.ToString();
			}
			return aValue;
		}

		/// <summary>
		/// takes a value and returns a characteristic RID for it.
		/// DOES NOT DO DATES
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private string ConvertValueToCharateristicRid(string charGroup, string aValue)
		{
			int scRid = _storeSession.StoreCharExists(charGroup, aValue);
			if (scRid == 0)
			{
				_storeSession.OpenUpdateConnection();
				scRid = _storeSession.AddStoreChar(charGroup, aValue);
				_storeSession.CommitData();
				_storeSession.CloseUpdateConnection();
			}

			return scRid.ToString();
		}

		/// <summary>
		/// Configures SGLS_STATEMENT; the the true SQL statement
		/// </summary>
		/// <param name="lAND"></param>
		/// <returns></returns>
		private string ConfigureSqlStatement(bool lAND)
		{
			StringBuilder sb = new StringBuilder();
			string hiddenCharName = "*unknown*";

			try
			{
				if (lAND)
					sb.Append(MIDText.GetTextOnly((int)eENGSQLOperator.And));
				sb.Append(_SGLS_PREFIX);
				sb.Append(" ");

				if (_SGLS_CHAR_IND)
				{
					hiddenCharName = "SCGRID__" + this._SCG_RID.ToString(CultureInfo.CurrentUICulture);
					sb.Append(hiddenCharName);
				}
				else if (_SGLS_CHAR_ID == (int)eMIDTextCode.lbl_StoreStatus)
					sb.Append("[Store Status]");
				else
					sb.Append((eStoreTableColumns)_SGLS_CHAR_ID);

				sb.Append(" ");
			
				if(_SGLS_SQL_OPERATOR == eENGSQLOperator.Between)
				{
					if (_SGLS_CHAR_IND)
					{
						string[] strArray = Split(_SGLS_VALUE, _SGLS_SQL_OPERATOR);
						if (strArray.Length != 2)
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_InvalidBetweenValue,
								MIDText.GetTextOnly((int)eMIDTextCode.msg_InvalidBetweenValue));

						sb.Append(" >= ");

						if (_SGLS_DT == eStoreCharType.text || _SGLS_DT == eStoreCharType.date)
						{
							sb.Append("'");
							sb.Append(strArray[0]);
							sb.Append("'");
						}
						else
						{
							sb.Append(strArray[0]);
						}

						sb.Append(" and ");
						sb.Append(hiddenCharName);
						sb.Append(" <= ");

						//================================================================================
						// For GT and LT the name is made a placeholder by placing an * infront of it.
						// during look-up time, it will be converted to the characteristic name.
						//================================================================================
						sb.Replace(hiddenCharName,"*" + hiddenCharName);

						if (_SGLS_DT == eStoreCharType.text || _SGLS_DT == eStoreCharType.date)
						{
							sb.Append("'");
							sb.Append(strArray[1]);
							sb.Append("'");
						}
						else
						{
							sb.Append(strArray[1]);
						}

					}
					else
						sb.Append(GetBetweenStatement(_SGLS_VALUE,(eStoreTableColumns)_SGLS_CHAR_ID, _SGLS_DT, false));
				}
				// If this is a Dynamic Characteristic and > or < was selected we have to jump through some hoops...
				else if (_SGLS_CHAR_IND && 
					(_SGLS_SQL_OPERATOR == eENGSQLOperator.GThan || _SGLS_SQL_OPERATOR == eENGSQLOperator.LThan) )
				{
					//================================================================================
					// For GT and LT the name is made a placeholder by placing an * infront of it.
					// during look-up time, it will be converted to the characteristic name.
					//================================================================================
					sb.Replace(hiddenCharName, "*" + hiddenCharName);

					if (_SGLS_SQL_OPERATOR == eENGSQLOperator.GThan)
						sb.Append(" > ");
					else if (_SGLS_SQL_OPERATOR == eENGSQLOperator.LThan)
						sb.Append(" < ");

					if (_SGLS_DT == eStoreCharType.text || _SGLS_DT == eStoreCharType.date)
					{
						sb.Append("'");
						sb.Append(_SGLS_VALUE);
						sb.Append("'");
					}
					else
					{
						sb.Append(_SGLS_VALUE);
					}

				}
				else
				{
					sb.Append(AllOperators(_SGLS_SQL_OPERATOR));
					sb.Append(" ");

					if (_SGLS_CHAR_IND || _SGLS_CHAR_ID == (int)eMIDTextCode.lbl_StoreStatus)
					{
						if (_SGLS_DT == eStoreCharType.list)
						{
							sb.Append("(");
							sb.Append(_SGLS_VALUE_CHAR_RID);
							sb.Append(")");
						}
						else
						{
							sb.Append(_SGLS_VALUE_CHAR_RID);
						}
					
					}
					else if (_SGLS_DT == eStoreCharType.text || _SGLS_DT == eStoreCharType.date)
					{
						sb.Append("'");
						// Begin Issue 3949 - stodd
						string aValue = _SGLS_VALUE.Replace("'","''");
						// End 
						sb.Append(_SGLS_VALUE);
						sb.Append("'");
					}
					
					else if (_SGLS_DT == eStoreCharType.list)
					{
						sb.Append("(");
						sb.Append(_SGLS_VALUE);
						sb.Append(")");
					}
					else
						sb.Append(_SGLS_VALUE);
				}
				sb.Append(" ");
				sb.Append(_SGLS_SUFFIX);
			}
			catch (Exception err)
			{
				HandleException(err);
			}
			
			return sb.ToString();

		}

		/// <summary>
		/// Configures SGLS_ENG_SQL (English-like sql)
		/// </summary>
		/// <param name="lAND"></param>
		/// <returns></returns>
		private string ConfigureEnglishStatement(bool lAND)
		{

			StringBuilder sb = new StringBuilder();
			if (lAND)
				sb.Append(MIDText.GetTextOnly((int)eENGSQLOperator.And));
				//sb.Append(allOperators[(int)eENGSQLOperator.And]);
			sb.Append(_SGLS_PREFIX);
			sb.Append(" ");

			if (_SGLS_CHAR_IND)
			{
				object[]findTheseVals = new object[2];
				//findTheseVals[0] = _SGLS_CHAR_ID;
				findTheseVals[0] = lbChar.SelectedValue;;
				findTheseVals[1] = true;
				DataRow dr = this._dtChar.Rows.Find(findTheseVals);
				
				sb.Append(dr["lbDisplay"]);
			}
			else
				sb.Append(GetTextValue((eStoreTableColumns)_SGLS_CHAR_ID));

			sb.Append(" ");
			if(_SGLS_SQL_OPERATOR == eENGSQLOperator.Between)
			{
				sb.Append(GetBetweenStatement(_SGLS_VALUE,(eStoreTableColumns)_SGLS_CHAR_ID, _SGLS_DT, true));
			}
			else
			{ 
				sb.Append(MIDText.GetTextOnly((int)_SGLS_SQL_OPERATOR));

				sb.Append(" ");
				if (_SGLS_DT == eStoreCharType.boolean)
				{
					if (_SGLS_VALUE == AllOperators(eENGSQLOperator.True))
						sb.Append(MIDText.GetTextOnly((int)eENGSQLOperator.True));
					else
						sb.Append(MIDText.GetTextOnly((int)eENGSQLOperator.False));
				}
				else if (_SGLS_DT == eStoreCharType.list)
				{
					sb.Append("(");
					sb.Append(_SGLS_VALUE);
					sb.Append(")");
				}
				else
					sb.Append(_SGLS_VALUE.ToString(CultureInfo.CurrentUICulture));
			}

			sb.Append(" ");
			sb.Append(_SGLS_SUFFIX);

			string retString;
			int j = 2;
			if (_SGLS_DT == eStoreCharType.date)
			{
				if (_SGLS_SQL_OPERATOR == eENGSQLOperator.Between)
					j = 0;
				for (int i = j;i<=3; i++)
				{
					try
					// ###
					{sb.Remove(sb.ToString().IndexOf("'"),1);}
					catch{}
				}
				retString = sb.ToString().Trim();
			}
			else
				retString = sb.ToString();

			return retString;
		}

		private string GetEngList(int charGroup, string[] vals)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			
			for (int i=0; i <= _storeChar.Count -1; i++)
			{
				CharacteristicGroup lStoreCharGroup;
				StoreCharValue lStoreCharVal;
				lStoreCharGroup = (CharacteristicGroup)_storeChar[i];
				ArrayList scgAL = lStoreCharGroup.Values;
				if (lStoreCharGroup.RID == charGroup)
				{
					for (int j=0; j<= lStoreCharGroup.Values.Count -1; j++)
					{
						lStoreCharVal = (StoreCharValue)scgAL[j];
						for (int k=0; k <= vals.Length -1; k++)
						{
							if (Convert.ToInt32(vals[k], CultureInfo.CurrentUICulture) == lStoreCharVal.SC_RID)
							{
								if (sb.Length > 1)
									sb.Append(", ");
								sb.Append(lStoreCharVal.CharValue);
							}
						}
					}
				}
			}
			sb.Append(")");
			return sb.ToString();
		}

		private void InitializeRow(DataRow row)
		{		
			switch (_SGLS_CHAR_IND)
			{
				case false:
						row["SGLS_CHAR_IND"] = "0";
						break;
				case true:
						row["SGLS_CHAR_IND"] = "1";
						break;
				default:
						row["SGLS_CHAR_IND"] = "0";	
						break;
			}

				row["SGLS_CHAR_ID"] = (int)_SGLS_CHAR_ID;
				row["SGLS_SQL_OPERATOR"] = (int)_SGLS_SQL_OPERATOR;
				row["SGLS_DT"] = (int)_SGLS_DT;
				
			if (_SGLS_CHAR_IND)
				row["SGLS_VALUE"] = _SGLS_VALUE_CHAR_RID.ToString(CultureInfo.CurrentUICulture);
			else if (_SGLS_CHAR_ID == (int)eMIDTextCode.lbl_StoreStatus)
				row["SGLS_VALUE"] = _SGLS_VALUE_CHAR_RID.ToString(CultureInfo.CurrentUICulture);
			else
				row["SGLS_VALUE"] = _SGLS_VALUE.ToString(CultureInfo.CurrentUICulture);
		
		}
		#endregion

		#region "lbEngSQL Command Buttons"
		
		// Remove Items from SQL Listbox & Trim items when 1st item is replaced.
		private void cmdRemove_Click(object sender, System.EventArgs e)
		{
			if (RemoveListBoxFirstLine())
				RemoveListBoxFirstLineExtras();

			HorizontalScrollFix();

            if (FormLoaded)
            {
                ChangePending = true;
            }
		}

		private bool RemoveListBoxFirstLine()
		{	bool retVal;
			int seq;
			try
			{
				if (lbEngSQL.SelectedIndex == 0)
					retVal = true;
				else
					retVal = false;

				if (lbEngSQL.Items.Count !=0 && lbEngSQL.SelectedIndex != -1)
				{
					DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));
					seq = Convert.ToInt32(dr["SGLS_STATEMENT_SEQ"], CultureInfo.CurrentUICulture);
					dr.Delete();
					_dtStoreGroupLevelStatement.AcceptChanges();
					foreach (DataRow dr2 in this._dtStoreGroupLevelStatement.Rows)
					{
						if (Convert.ToInt32(dr2["SGLS_STATEMENT_SEQ"], CultureInfo.CurrentUICulture) > seq)
							dr2["SGLS_STATEMENT_SEQ"] = Convert.ToInt32(dr2["SGLS_STATEMENT_SEQ"], CultureInfo.CurrentUICulture) - 1;
					}
					lbEngSQL.Refresh();
					int lSelIndex = lbEngSQL.SelectedIndex;
					
					if (lbEngSQL.Items.Count > 0 && (lSelIndex != lbEngSQL.Items.Count))
					{
						lbEngSQL.Focus();
						lbEngSQL.SelectedIndex = lSelIndex;
					}
				}	
				return retVal;
			}
			catch
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_RemoveTextFromWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				return false;}
		}

		private bool RemoveListBoxFirstLineExtras()
		{
			try
			{
				if (lbEngSQL.SelectedIndex == 0)
				{
					DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),1);
					SetClassVariables(dr);
					dr["SGLS_PREFIX"] = "";
                    _SGLS_PREFIX = dr["SGLS_PREFIX"].ToString(); // TT#813 - MD - New Store Atribute Error - RBeck
					dr["SGLS_SUFFIX"] = "";
					ModifySqlStatement(dr);
					lbEngSQL.Refresh();
				}
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_RemoveTextFromWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}

			return true;
		}

		private void cmdOr_Click(object sender, System.EventArgs e)
		{
			if (lbEngSQL.Items.Count !=0 && lbEngSQL.SelectedIndex != -1)
			{
				toggleOrAnd("AND", "OR");
			}

            if (FormLoaded)
            {
                ChangePending = true;
            }
		}
 
		private void cmdAnd_Click(object sender, System.EventArgs e)
		{
			if (lbEngSQL.Items.Count !=0 && lbEngSQL.SelectedIndex != -1)
			{
				toggleOrAnd("OR", "AND");
			}		

            if (FormLoaded)
            {
                ChangePending = true;
            }
		}

		private void toggleOrAnd(string str1, string str2)
		{
			try
			{
				DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));
				SetClassVariables(dr);
				//Toggle OR and AND per line
				StringBuilder sbString = new StringBuilder(dr["SGLS_PREFIX"].ToString());
				StringBuilder sbString2 = new StringBuilder(sbString.ToString().ToUpper(CultureInfo.CurrentUICulture));
				sbString2.Replace(str1, str2);
				dr["SGLS_PREFIX"] = sbString2.ToString();
				ModifySqlStatement(dr);
				lbEngSQL.Refresh();
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void cmdLParen_Click(object sender, System.EventArgs e)
		{
			string [] strArray; 
			try
			{
				if (lbEngSQL.Items.Count !=0 && lbEngSQL.SelectedIndex != -1)
				{
					DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));
					SetClassVariables(dr);
					StringBuilder sbString = new StringBuilder(dr["SGLS_PREFIX"].ToString());
					//Always allow left paren as first character of first line
					if (lbEngSQL.SelectedIndex == 0)
						sbString.Insert(0,"(");
					else
						strArray = new string[Regex.Split(sbString.ToString()," ").Length];
					strArray = Regex.Split(sbString.ToString()," ");
				
					if (strArray[0].ToUpper(CultureInfo.CurrentUICulture) == "AND" || 
							strArray[0].ToUpper(CultureInfo.CurrentUICulture) == "OR")
					{
						if (strArray[strArray.Length-1].ToUpper(CultureInfo.CurrentUICulture) == "NOT" 
								|| strArray[strArray.Length-1].ToUpper(CultureInfo.CurrentUICulture) == "AND" 
								|| strArray[strArray.Length-1].ToUpper(CultureInfo.CurrentUICulture) == "OR")
							sbString.Insert(sbString.Length, " ");
						//Insert left paren after operators
						//if (strArray[1] == "NOT")
							sbString.Insert(sbString.Length, "(");
						//else
							//sbString.Insert(strArray[0].Length + 1,"(");
					}

					dr["SGLS_PREFIX"] = sbString.ToString();
					ModifySqlStatement(dr);
					lbEngSQL.Refresh();

                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }

					//lbEngSQLEdit(sbString);			
				}
			}
			catch (Exception err)
			{ string strTmp = err.Message;
			MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void cmdRParen_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (lbEngSQL.Items.Count !=0 && lbEngSQL.SelectedIndex != -1)
				{
					DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));
					SetClassVariables(dr);
					StringBuilder sbString = new StringBuilder(dr["SGLS_SUFFIX"].ToString());
					//Always allow Right Paren at end of line
					sbString.Insert(sbString.Length,")");
					
					dr["SGLS_SUFFIX"] = sbString.ToString();
					ModifySqlStatement(dr);
					lbEngSQL.Refresh();

                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
					
					//lbEngSQLEdit(sbString);
				}
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void cmdNot_Click(object sender, System.EventArgs e)
		{
			string [] strArray; 
			 
			try
			{
				if (lbEngSQL.Items.Count !=0 && lbEngSQL.SelectedIndex != -1)
				{
					DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));
					// BEGIN Issue 4481 stodd 11.30.2007
					SetClassVariables(dr);
					// END Issue 4481

					StringBuilder sbString = new StringBuilder(dr["SGLS_PREFIX"].ToString());
					
					if (lbEngSQL.SelectedIndex != 0)
						strArray = new string[Regex.Split(sbString.ToString()," ").Length];
					strArray = Regex.Split(sbString.ToString()," ");
					
					if (strArray[0].ToUpper(CultureInfo.CurrentUICulture) == "AND" || strArray[0].ToUpper(CultureInfo.CurrentUICulture) == "OR")
					{
						//insert NOT, if NOT already exists remove it.
						if (strArray.Length == 1)
							sbString.Insert(strArray[0].Length," NOT");
						else
						{
							if (strArray[1] != "NOT")
								sbString.Insert(strArray[0].Length," NOT");
							else 
								sbString.Remove(strArray[0].Length,4);
						}

					}
					dr["SGLS_PREFIX"] = sbString.ToString();
					ModifySqlStatement(dr);
					lbEngSQL.Refresh();

                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
				}
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_AddTextToWhereClause),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

		private void SetClassVariables(DataRow dr)
		{
			try
			{
				_SGLS_CHAR_ID = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);
				if (dr["SGLS_CHAR_IND"].ToString() == "0")
					_SGLS_CHAR_IND = false;
				else
					_SGLS_CHAR_IND = true;
				if (_SGLS_CHAR_IND)
				{
					DataTable dt = _dsStoreChar.Tables["STORE_CHAR_GROUP"];
					dt.PrimaryKey = new DataColumn[] {dt.Columns["SCG_RID"]};
					DataRow dr2 = dt.Rows.Find(_SGLS_CHAR_ID);  //scgRid
					_storeCharType = (eStoreCharType)Convert.ToInt32(dr2["SCG_TYPE"], CultureInfo.CurrentUICulture);
					_SCG_RID = Convert.ToInt32(dr2["SCG_RID"], CultureInfo.CurrentUICulture);
					_SCG_NAME = dr2["SCG_ID"].ToString();
				}
				//_SQLOperator
				_SGLS_SQL_OPERATOR = (eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"],CultureInfo.CurrentUICulture);
				_SGLS_DT = (eStoreCharType)Convert.ToInt32(dr["SGLS_DT"],CultureInfo.CurrentUICulture);
				_SGLS_VALUE = dr["SGLS_VALUE"].ToString();
				_SGLS_VALUE_CHAR_RID = dr["SGLS_VALUE"].ToString();
				if (dr["SGLS_PREFIX"] != DBNull.Value)
					_SGLS_PREFIX = dr["SGLS_PREFIX"].ToString();
				if (dr["SGLS_SUFFIX"] != DBNull.Value)
					_SGLS_SUFFIX = dr["SGLS_SUFFIX"].ToString();
			}
			catch
			{
				throw;
			}
		}


		private void cmdCancelGroupBox_Click(object sender, System.EventArgs e)
		{
			_isModify = false;
			groupBox1.Visible = false;
			cmdAdd.Text = "&Add";
		}
 
		private void cmdModify_Click(object sender, System.EventArgs e)
		{
			//Find SGL_RID and SGLS_STATEMENT_SEQ to get appropriate info from either
			//Store_Char dt or _dtAppText (Stores table columns)

			//Get the DataRow that corresponds with the selected lbChar item
			try
			{
				if (this.lbEngSQL.SelectedIndex != -1)
				{
					DataRow dr = GetSGLS(Convert.ToInt32(this.lbEngSQL.SelectedValue, CultureInfo.CurrentUICulture),Convert.ToInt32(this.lbEngSQL.SelectedIndex + 1, CultureInfo.CurrentUICulture));

					string lWhere;

					if (dr["SGLS_CHAR_IND"].ToString()==DYNAMIC_CHARACTERISTIC) 
					{
						DataTable dt = _dsStoreChar.Tables["STORE_CHAR_GROUP"];
						dt.PrimaryKey = new DataColumn[] {dt.Columns["SCG_RID"]};

						// for a Store Dynamic Characteristic, the Store Char Rid is in the "value" field
						int scgRid = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);
						DataRow dr2 = dt.Rows.Find(scgRid);
						lWhere = _storeSession.GetStoreCharacteristicGroupID(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
						_storeCharType = (eStoreCharType)Convert.ToInt32(dr2["SCG_TYPE"], CultureInfo.CurrentUICulture);
						_SCG_RID = Convert.ToInt32(dr2["SCG_RID"], CultureInfo.CurrentUICulture);
						_SCG_NAME = _SAB.StoreServerSession.GetStoreCharacteristicGroupID(_SCG_RID);
						//_SCG_NAME = _SCG_NAME.Replace(" ","_");
					}
					else if ( Convert.ToInt32(dr["SGLS_CHAR_ID"],CultureInfo.CurrentUICulture) == (int)eMIDTextCode.lbl_StoreStatus )
					{
						lWhere = _storeSession.StoreStatusText;
					}
					else
						lWhere = GetTextValue((eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture)));

					groupBox1.Text = "Where " + lWhere;
					cmdAdd.Text = "&Update";

					//Show groupBox1
					if (groupBox1.Visible == false)
						groupBox1.Visible = true;
				
					//Show appropriate panel
					eStoreCharType lDT = (eStoreCharType)(Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture));
					switch ( lDT )
					{
						case eStoreCharType.boolean:
							InitializeBoolPanel(dr);
							break;
						case eStoreCharType.text:
							InitializeStringPanel(dr);
							break;
						case eStoreCharType.dollar:
						case eStoreCharType.number:
							InitializeNumberPanel(dr);
							break;
						case eStoreCharType.date:
							InitializeDatePanel(dr);
							break;
						case eStoreCharType.list:
							string[] strAR = dr["SGLS_VALUE"].ToString().Split(new char[] {','});
							ArrayList al = new ArrayList(strAR);
							eENGSQLOperator anOperator = (eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture);
							GenericLoad(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture), anOperator, al);
							break;
					}
					_isModify = true;
                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
				}
			}
			catch (Exception err)
			{	
				HandleException(err);
			}
		}

		private DataRow GetSGLS(int SGL_RID, int SGLS_STATEMENT_SEQ)
		{
			object[]pks = new object [2];
			pks[0] = SGL_RID;
			pks[1] = SGLS_STATEMENT_SEQ;

			DataRow dr;
			dr = this._dtStoreGroupLevelStatement.Rows.Find(pks);

			return dr;
		}
		
		private eStoreTableColumns GetSGLSCharIDString(DataRow dr)
		{
			DataRow drSC;
			drSC = _dtAppText.Rows.Find(dr["SGLS_CHAR_ID"]);
			eStoreTableColumns stc = (eStoreTableColumns)(Convert.ToInt32(drSC["TEXT_CODE"], CultureInfo.CurrentUICulture));
			return stc;
		}

		private string GetTextValue(eStoreTableColumns stc)
		{
			DataRow dr;
			dr = _dtAppText.Rows.Find(stc);
			return dr["TEXT_VALUE"].ToString();
		}

		private string GetBetweenStatement(string val, eStoreTableColumns sglsChar, eStoreCharType dataType, bool engSQL)
		{
			
			string[] strArray = Split(val, eENGSQLOperator.Between);
			string retVal;

			if (engSQL)
			{				
				retVal =  MIDText.GetTextOnly((int)eENGSQLOperator.Between) + " " + strArray[0].ToString(CultureInfo.CurrentUICulture) + " "
					+ MIDText.GetTextOnly((int)eENGSQLOperator.And) + " "	+ strArray[1].ToString(CultureInfo.CurrentUICulture);
			}
			else
			{
				if (dataType == eStoreCharType.date)
				{
					retVal = AllOperators(eENGSQLOperator.GThan) + AllOperators(eENGSQLOperator.Equals) + " '" 
						+ strArray[0].ToString(CultureInfo.CurrentUICulture) + "' " + AllOperators(eENGSQLOperator.And) + " " + sglsChar  + " "
						+ AllOperators(eENGSQLOperator.LThan) + AllOperators(eENGSQLOperator.Equals)  + " '"
						+ strArray[1].ToString(CultureInfo.CurrentUICulture) + "'";
				}
				else
				{
					retVal = AllOperators(eENGSQLOperator.GThan) + AllOperators(eENGSQLOperator.Equals) + " " 
						+ strArray[0].ToString(CultureInfo.CurrentUICulture) + " " + AllOperators(eENGSQLOperator.And) + " " + sglsChar  + " "
						+ AllOperators(eENGSQLOperator.LThan) + AllOperators(eENGSQLOperator.Equals)  + " "
						+ strArray[1].ToString(CultureInfo.CurrentUICulture);
				}
			}
			return retVal;
		}
 
		private void CreateStatements()
		{
			try
			{
				foreach(DataRow dr in this._dtStoreGroupLevelStatement.Rows)
				{
					eStoreTableColumns tc;
					string sqlOp;
					string sglsValue;
					string sglsCharValue;

					if (dr["SGLS_CHAR_IND"].ToString() == "0")
					{
						tc = (eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
						sglsCharValue = tc.ToString();
					}
					else
						//Find SotreCharGroupName
						sglsCharValue=  "blah";
				
//					if ((eStoreCharType)Convert.ToInt32(dr["SGLS_DT"]) == eStoreCharType.boolean)
//					{
//						sqlOp = (eENGSQLOperator)Convert.ToInt32(dr["SGLS_VALUE"]);
//						sglsValue = sqlOp.ToString();
//					}
					if ((eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture) == eStoreCharType.text)
					{
						sglsValue = "'" + dr["SGLS_VALUE"].ToString() + "'";
						sqlOp = AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
					}
					else if ((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture) == eENGSQLOperator.Between)
					{
						sglsValue = GetBetweenStatement(dr["SGLS_VALUE"].ToString(),
							(eStoreTableColumns)Convert.ToInt32(dr["SGLS_CHAR_ID"],CultureInfo.CurrentUICulture), 
							(eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture),
							false);
						sqlOp = "";
					}
					else
					{
						sglsValue = dr["SGLS_VALUE"].ToString();
						sqlOp = AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
					}		

					string statement = dr["SGLS_PREFIX"] + " " + sglsCharValue + " " 
						+ sqlOp	+ " " + sglsValue + " " + dr["SGLS_SUFFIX"];

				}
			}
			catch (Exception err)
			{ string tmpStr = err.Message;
			}
		}

		/// <summary>
		/// Configures SGLS_STATEMENT (real SQL)
		/// </summary>
		/// <param name="dr"></param>
		private void ModifySqlStatement(DataRow dr)
		{
			try
			{
				eStoreTableColumns tc;
				string sqlOp = "";
				string sglsValue = "";
				string sglsCharValue;
				string hiddenCharName = "*unknown*";
				eStoreCharType charDataType = (eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture);

				if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)
				{
					hiddenCharName = "SCGRID__" + _SCG_RID.ToString(CultureInfo.CurrentUICulture);
					sglsCharValue = hiddenCharName;
				}
				else
				{
					int charId = (Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
					if (charId == (int)eMIDTextCode.lbl_StoreStatus)
					{
						sglsCharValue = "[Store Status]";
					}
					else
					{
						tc = (eStoreTableColumns)(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
						sglsCharValue = tc.ToString();
					}
				}
			
				// We only want to put single quote around a string data type if it a real store column.  If it's not, then
				// the value is really the dynamic characteristic RID.
				if (charDataType == eStoreCharType.text
					&& dr["SGLS_CHAR_IND"].ToString() == STORE_TABLE_COLUMN)
				{
					sglsValue = "'" + dr["SGLS_VALUE"].ToString() + "'";
					sqlOp =AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));

				}
				else if ((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture) == eENGSQLOperator.Between)
				{
					if (dr["SGLS_CHAR_IND"].ToString() == STORE_TABLE_COLUMN)
					{
						sglsValue = GetBetweenStatement(dr["SGLS_VALUE"].ToString(),(eStoreTableColumns)Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture), 
							(eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture),
							false);
						sqlOp = "";
					}
					else
					{
						sglsCharValue = "*" + hiddenCharName;
						string aValue = ConvertCharacteristicRidToValue((eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture), dr["SGLS_VALUE"].ToString());
						sglsValue = ConstructCharBetweenSqlOp(aValue, sglsCharValue, (eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture));  
						sqlOp = "";
					}
				}
				else if (charDataType == eStoreCharType.date
					&& dr["SGLS_CHAR_IND"].ToString() == STORE_TABLE_COLUMN)
				{
					sglsValue = "'" + dr["SGLS_VALUE"].ToString() + "'";
					sqlOp =AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));

				}
				else if (charDataType == eStoreCharType.list)
				{
					int charId = (Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture));
					if (charId == (int)eMIDTextCode.lbl_StoreStatus)
					{
						sglsValue = "(" + _SGLS_VALUE_CHAR_RID + ")";
						sqlOp = AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
					}
					else
					{
						sglsValue = "(" + dr["SGLS_VALUE"].ToString() + ")";
						sqlOp = AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
					}
				}
				// Since we've already checked for String & List, that leaves Date, Dollar & Number.
				// we have to jump through hopes to do > and <
				else if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC &&
					((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture) == eENGSQLOperator.GThan ||
					(eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture) == eENGSQLOperator.LThan) )
				{
					// need to search on Store char group now
					sglsCharValue = "*" + hiddenCharName;
//					sqlOp = " = ";
//					sqlOp += dr["SGLS_CHAR_ID"].ToString();
//					sqlOp += " AND ";
//					switch (_SGLS_DT)
//					{
//
//						case eStoreCharType.text:
//							sqlOp += "TEXT_VALUE ";
//							break;
//						case eStoreCharType.date:
//							sqlOp += "DATE_VALUE ";
//							break;
//						case eStoreCharType.number:
//							sqlOp += "NUMBER_VALUE ";
//							break;
//						case eStoreCharType.dollar:
//							sqlOp += "DOLLAR_VALUE ";
//							break;
//					}

					if (_SGLS_SQL_OPERATOR == eENGSQLOperator.GThan)
						sqlOp += " > ";
					else if (_SGLS_SQL_OPERATOR == eENGSQLOperator.LThan)
						sqlOp += " < ";

					if (charDataType == eStoreCharType.date)
						sglsValue = "'" + _SGLS_VALUE + "'";
					else
						sglsValue = _SGLS_VALUE;
				}
				else
				{
					sglsValue = dr["SGLS_VALUE"].ToString();
					sqlOp = AllOperators((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture));
				}

				string statement = dr["SGLS_PREFIX"] + " " + sglsCharValue + " " 
						+ sqlOp	+ " " + sglsValue + " " + dr["SGLS_SUFFIX"];

				dr["SGLS_STATEMENT"] = statement.Trim();

				//Add ENG_SQL here---
                ConfigureEnglishStatement(dr);
			}
			catch (Exception err)
			{
					string tmpStr = err.Message;
			}
		}

		private string ConstructCharBetweenSqlOp(string aValue, string charName, eStoreCharType sgls_dt)
		{
			string[] strArray = Split(aValue, eENGSQLOperator.Between);

			string sqlOp = "";
//			sqlOp += charName;
//			sqlOp += " AND ";
//			switch (sgls_dt)
//			{
//				case eStoreCharType.text:
//					sqlOp += "TEXT_VALUE ";
//					break;
//				case eStoreCharType.date:
//					sqlOp += "DATE_VALUE ";
//					break;
//				case eStoreCharType.number:
//					sqlOp += "NUMBER_VALUE ";
//					break;
//				case eStoreCharType.dollar:
//					sqlOp += "DOLLAR_VALUE ";
//					break;
//			}

			sqlOp += " >= ";

			if (sgls_dt == eStoreCharType.text || sgls_dt == eStoreCharType.date)
			{
				sqlOp += "'";
				sqlOp += strArray[0];
				sqlOp += "'";
			}
			else
			{
				sqlOp += strArray[0];
			}

			sqlOp += " and ";

			sqlOp += charName;

						
//			switch (sgls_dt)
//			{
//				case eStoreCharType.text:
//					sqlOp += "TEXT_VALUE ";
//					break;
//				case eStoreCharType.date:
//					sqlOp += "DATE_VALUE ";
//					break;
//				case eStoreCharType.number:
//					sqlOp += "NUMBER_VALUE ";
//					break;
//				case eStoreCharType.dollar:
//					sqlOp += "DOLLAR_VALUE ";
//					break;
//			}
						
			sqlOp += " <= ";

			if (sgls_dt == eStoreCharType.text || sgls_dt == eStoreCharType.date)
			{
				sqlOp += "'";
				sqlOp += strArray[1];
				sqlOp += "'";
			}
			else
			{
				sqlOp += strArray[1];
			}
			
			return sqlOp;
		}

		private bool DBNullError(DataRow dr)
		{
			if (dr["SGLS_CHAR_IND"] == DBNull.Value || 
				dr["SGLS_SQL_OPERATOR"]  == DBNull.Value || 
				dr["SGLS_DT"]  == DBNull.Value || 
				dr["SGLS_VALUE"] == DBNull.Value)
			{
				//MIDText...
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_StoreGroupBuilderDBNullError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				//MessageBox.Show("Error:  Null value in Database.  Store_Group_Level_Statement must be built by Store Group Builder");
				return true;
			}

			return false;
		}

		/// <summary>
		/// Edit SGLS_ENG_SQL (English-like sql)
		/// </summary>
		/// <param name="dr"></param>
		private void ConfigureEnglishStatement(DataRow dr)
		{
			//Check for null values that would blow up this method
			if (DBNullError(dr))
				return;

			StringBuilder sb = new StringBuilder();
			sb.Append(dr["SGLS_PREFIX"].ToString());
			sb.Append(" ");

			if (Include.ConvertCharToBool(Convert.ToChar(dr["SGLS_CHAR_IND"], CultureInfo.CurrentUICulture)))
			{
				object[]findTheseVals = new object[2];
				findTheseVals[0] = dr["SGLS_CHAR_ID"];
				findTheseVals[1] = true;
				DataRow dr2 = this._dtChar.Rows.Find(findTheseVals);
				
				sb.Append(dr2["lbDisplay"]);
			}
			else
				sb.Append(GetTextValue((eStoreTableColumns)Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture)));


			object aValue = null;
			if (dr["SGLS_CHAR_IND"].ToString() == DYNAMIC_CHARACTERISTIC)
			{
				eStoreCharType sct = (eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture);
				int scgRid = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);
				if (sct == eStoreCharType.list)
					sct = _storeSession.GetStoreCharacteristicDataType(scgRid);
				aValue = ConvertCharacteristicRidToValue(sct, dr["SGLS_VALUE"].ToString());
			}
			else if ( Convert.ToInt32(dr["SGLS_CHAR_ID"],CultureInfo.CurrentUICulture) == (int)eMIDTextCode.lbl_StoreStatus )
			{
				aValue = _SGLS_VALUE;

			}
			else
				aValue = dr["SGLS_VALUE"].ToString();

			sb.Append(" ");
			if((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture) == eENGSQLOperator.Between)
			{
				sb.Append(GetBetweenStatement(aValue.ToString(),(eStoreTableColumns)Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture),
					(eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture),
					true));
			}
			else
			{ 

				sb.Append(MIDText.GetTextOnly(Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture)));

				sb.Append(" ");
				if ((eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture) == eStoreCharType.boolean)
				{
					if (aValue.ToString() == AllOperators(eENGSQLOperator.True))
						sb.Append(MIDText.GetTextOnly((int)eENGSQLOperator.True));
					else
						sb.Append(MIDText.GetTextOnly((int)eENGSQLOperator.False));
				}
				else if ((eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture) == eStoreCharType.list)
				{
					//string[] vals = aValue.ToString().Split(new Char[] {','});
					//sb.Append(GetEngList(Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture),vals));
					sb.Append("(");
					sb.Append(aValue.ToString());
					sb.Append(")");
				}
				else
					sb.Append(aValue.ToString());
			}

			sb.Append(" ");
			sb.Append(dr["SGLS_SUFFIX"]);

			string retString;
			int j = 2;
			if ((eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture) == eStoreCharType.date)
			{
				if ((eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture) == eENGSQLOperator.Between)
					j = 0;
				for (int i = j;i<=3; i++)
				{
					try
					//###
					{sb.Remove(sb.ToString().IndexOf("'"),1);}
					catch{}
				}
				retString = sb.ToString().Trim();
			}
			else
				retString = sb.ToString();

			dr["SGLS_ENG_SQL"] = retString.Trim();

			HorizontalScrollFix(dr);
		}

		/// <summary>
		/// Splits string value delimited by a comma(,) -- does special processing for BETWEEN operator.
		/// </summary>
		/// <param name="aValue"></param>
		/// <returns></returns>
		private string[] Split(string aValue, eENGSQLOperator anOperator)
		{
			try
			{
				string[] valueArray = null;
				if (anOperator == eENGSQLOperator.Between)
					valueArray = aValue.Split(new Char[] {','});
				else
				{
					valueArray = new string[1];
					valueArray[0] = aValue;
				}

				return valueArray;
			}
			catch
			{
				throw;
			}
		}


		#endregion
 
		#region "Validation"

		private bool ValidateDollarPanel()
		{
			bool isValid = true;
			int pos = 0;
			// First lets make sure everything is entered and numeric...
			if (ValidateNumberPanel())
			{
				if (dateNumbBetweenButton.Checked)
				{

					pos = txtBetweenDateNumb1.Text.IndexOf(".");
					if(pos != -1)// 2 decimals + "."
					{
						isValid = txtBetweenDateNumb1.Text.Substring(pos).Length <= 3;
						if (!isValid)
						{
							MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidDollarValues),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						}
					}

					if (isValid)
					{
						pos = txtBetweenDateNumb2.Text.IndexOf(".");
						if(pos != -1)// 2 decimals + "."
						{
							isValid = txtBetweenDateNumb2.Text.Substring(pos).Length <= 3;
							if (!isValid)
							{
								MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidDollarValues),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
							}
						}
					}
				}
				else if (dateNumbGThanButton.Checked || dateNumbLThanButton.Checked || dateNumbEqualButton.Checked)
				{
					pos = txtCompareDateNumb.Text.IndexOf(".");
					if(pos != -1)// 2 decimals + "."
					{
						isValid = txtCompareDateNumb.Text.Substring(pos).Length <= 3;
						if (!isValid)
						{
							MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidDollarValue),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						}
					}
				}
			}
			else
				isValid = false;

			return isValid;
		}
		private bool ValidateNumberPanel()
		{
			bool isValid = true;
			try
			{
				if (dateNumbBetweenButton.Checked)
				{
					if (txtBetweenDateNumb1.Text == string.Empty || txtBetweenDateNumb2.Text == string.Empty)
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidNumericValues),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						isValid = false;
					}
					else
					{
						decimal dec1 = decimal.Parse(txtBetweenDateNumb1.Text);
						decimal dec2 = decimal.Parse(txtBetweenDateNumb2.Text);
					}
				}
				else if (dateNumbGThanButton.Checked || dateNumbLThanButton.Checked || dateNumbEqualButton.Checked)
				{
					if (txtCompareDateNumb.Text == string.Empty)
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidNumericValue),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						isValid = false;
					}
					else
					{
						decimal dec = decimal.Parse(txtCompareDateNumb.Text);
					}
				}
			}
			catch
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidNumericValues),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				isValid = false;
			}
			
			return isValid;
		}

		private bool ValidateDatePanel()
		{
			bool isValid = true;
			try
			{
				if (dateNumbBetweenButton.Checked)
				{
					if (txtBetweenDateNumb1.Text == string.Empty || txtBetweenDateNumb2.Text == string.Empty
						|| txtBetweenDateNumb1.Text == "01/01/0001" || txtBetweenDateNumb2.Text == "01/01/0001")
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidDateValues),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						isValid = false;
					}
				}
				else if (dateNumbGThanButton.Checked || dateNumbLThanButton.Checked || dateNumbEqualButton.Checked)
				{
					if (txtCompareDateNumb.Text == string.Empty || txtCompareDateNumb.Text == "01/01/0001")
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidDateValue),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						isValid = false;
					}
				}
			}
			catch
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidDateValues),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				isValid = false;
			}
			
			return isValid;
		}

		private bool ValidateStringPanel()
		{
			if ((stringEqualButton.Checked || stringNotEqualButton.Checked) && (ValidText(txtString.Text)))
				return true;
			else
			{	
				return false;
			}
		}

		private bool ValidateBooleanPanel()
		{
			if (boolTrueButton.Checked || boolFalseButton.Checked)
				return true;
			else
			{	//todo
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidBooleanValue),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				return false;
			}
		}

		private bool ValidGroupNameText(string txtBoxText)
		{
			if (txtBoxText != "")
			{
				if (txtBoxText != _origGroupName)
				{
					bool groupLevelExists = _storeSession.DoesGroupLevelNameExist(_SG_RID, txtBoxText);
					if (groupLevelExists)
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DuplicateName));
						return false;
					}
				}
				return true;
			}
			else
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_StoreGroupNameRequired));
				return false;
			}
		}

		private bool ValidText(string txtBoxText)
		{
			if (txtBoxText == "")
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_MissingTextValue),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				return false;
			}

			return true;
		}

		private void validateBetweenDates(eStoreCharType DataTypeGeneric)
		{
			try
			{
				if (DataTypeGeneric == eStoreCharType.date)
				{
					if (Convert.ToDateTime(txtBetweenDateNumb2.Text, CultureInfo.CurrentUICulture) < Convert.ToDateTime(txtBetweenDateNumb1.Text, CultureInfo.CurrentUICulture))
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_FromDateError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						txtBetweenDateNumb1.Focus();
					}
				}
				else
				{
					if (double.Parse(txtBetweenDateNumb2.Text, CultureInfo.CurrentUICulture) < double.Parse(txtBetweenDateNumb1.Text, CultureInfo.CurrentUICulture))
					{
						MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_FromDateError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
						txtBetweenDateNumb1.Focus();	
					}
				}
			}
			catch
			{MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_ValidateDateError),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));}
		}

#endregion

		private void lbEngSQL_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lbEngSQL.SelectedIndex == 0)
			{
				cmdAnd.Enabled = false;
				cmdOr.Enabled = false;
				cmdNot.Enabled = false;
				cmdRParen.Enabled = true;
				cmdLParen.Enabled = true;
			}
			else
			{
				cmdAnd.Enabled = true;
				cmdOr.Enabled = true;
				cmdNot.Enabled = true;
				cmdRParen.Enabled = true;
				cmdLParen.Enabled = true;
			}
		}

		#region IFormBase Members
		override public void ICut()
		{
			
		}

		override public void ICopy()
		{
			
		}

		override public void IPaste()
		{
			
		}	

//		override public void IClose()
//		{
//			try
//			{
//				this.Close();
//
//			}		
//			catch(Exception ex)
//			{
//				MessageBox.Show(ex.Message);
//			}
//			
//		}

		override public void ISave()
		{
			try
			{
				SaveChanges();
			}		
			catch(Exception ex)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SaveStoreGroupError) + "\n" + ex.ToString(),MIDText.GetText(eMIDTextCode.lbl_StoreGroupBuilder));
				this.DialogResult = DialogResult.Cancel;
			}
		}

		override public void ISaveAs()
		{
			
		}

		override public void IDelete()
		{
			
		}

		override public void IRefresh()
		{
			
		}
		
		#endregion

		private void StoreGroupBuilder_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// BEGIN Issue 5431 stodd 5.9.2008
			//==============================================================
			// CHeck to see if the store profile maint window is open.
			// If it is not, then all store datatable can be cleared.
			//==============================================================
			if (!_storeMaintIsActive)
				_storeSession.ClearAllStoreDataTable();
			// END Issue 5431
		}
	}

	public class StoreGroupLevelChangeEventArgs : EventArgs
	{
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		MIDStoreNode _sg_node;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		StoreGroupLevelProfile _storeGroupLevel;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//int _sg_rid;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public StoreGroupLevelChangeEventArgs(int sg_rid, StoreGroupLevelProfile storeGroupLevel)
		public StoreGroupLevelChangeEventArgs(MIDStoreNode sg_node, StoreGroupLevelProfile storeGroupLevel)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			_sg_node = sg_node;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			_storeGroupLevel = storeGroupLevel;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_sg_rid = sg_rid;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		}
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		public MIDStoreNode SG_NODE
		{
			get { return _sg_node; }
			set { _sg_node = value; }
		}
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		public StoreGroupLevelProfile StoreGroupLevel 
		{
			get { return _storeGroupLevel ; }
			set { _storeGroupLevel = value; }
		}
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public int SG_RID 
		//{
		//    get { return _sg_rid ; }
		//    set { _sg_rid = value; }
		//}
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
	}
}
