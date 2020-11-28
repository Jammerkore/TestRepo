using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Data;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for CalendarModelMaint.
	/// </summary>
	public class CalendarModelMaint : MIDFormBase
	{
		private System.Data.DataSet _dsCalendarModel;
		private System.Data.DataSet _dsCalendarModelPeriods;
		private System.Data.DataTable _dtWeek53;
		//private DataSet _dsWeek53;

		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblModelName;
		private System.Windows.Forms.Label lblStartDate;
		private System.Windows.Forms.TextBox txtStartDate;
		private System.Windows.Forms.Label lblFiscalYear;
		private System.Windows.Forms.TextBox txtFiscalYr;
		//private System.Windows.Forms.Label lbl53WkOffset;
		//private System.Windows.Forms.TextBox text53WkOffset;
		//private System.Windows.Forms.Label lbl53WkOffsetPeriod;
		//private System.Windows.Forms.TextBox txt53WkOffsetPeriod;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblTotalWks;
		private System.Windows.Forms.Button btnStartDate;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugridPeriods;
		private System.ComponentModel.Container components = null;
		//private System.Windows.Forms.Label label2;
		//private System.Windows.Forms.TextBox txt53weekCycle;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemNew;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.TextBox txtCalendarModelName;
		private bool _refreshCalendar;
//		private bool _formLoading = false;
		private DataTable _dtPeriodNames;
		private DataTable _dtPeriodAbbreviations;
		private DataTable _dtQuarterNames;
		private DataTable _dtQuarterAbbreviations;
		private DataTable _dtSeasonNames;
		private DataTable _dtSeasonAbbreviations;


		private CalendarData _cd; // = new CalendarData();
		private DataRow _currRow;
//		private bool _newModel;
		private DateTime _nullDate = new DateTime(1,1,1);
		private SessionAddressBlock _SAB;
		private MRSCalendar _cal;
		private int _numWeeks = 0;
		private Point _point;
		private ArrayList _errorMessages = new ArrayList();
		private int _quartersInSeason;
		private int _monthsInQuarter;
		private int _totalQuarters;
		private int _totalMonths;

		private string _monthLabel;
		private string _quarterLabel;
		private string _seasonLabel;

		private int _calendarModelRID;
		public int CalendarModelRID 
		{
			get { return _calendarModelRID ; }
			set { _calendarModelRID = value; }
		}
		private string _calendarModelName;
		public string CalendarModelName 
		{
			get { return _calendarModelName ; }
			set { _calendarModelName = value; }
		}
		private int _fiscalYear;
		private Infragistics.Win.UltraWinGrid.UltraGrid uGridWeek53;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.Button btn454;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboPeriodType;
		private Button btn2Seasons;
		private Button btn4Quarters;
	
		public int FiscalYear 
		{
			get { return _fiscalYear ; }
			//set { _calendarModelName = value; }
		}
		private DateTime _startDate;
		public DateTime StartDate 
		{
			get { return _startDate ; }
			//set { _calendarModelName = value; }
		}

		public CalendarModelMaint(SessionAddressBlock SAB, int cmRID) : base(SAB)
		{
			InitializeComponent();

			_SAB = SAB;
			_cal = _SAB.ClientServerSession.Calendar;
			_cd = _cal.CalendarData;

			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminCalendarDefine);
			Common_Load ();


			_calendarModelRID = cmRID;
			_calendarModelName = "";
			_fiscalYear = 0;
			_startDate = _nullDate;
			
			DateTime dt = new DateTime(1,1,1);
			object [] initRow = {-1,"",dt,0};

			_dsCalendarModel = _cd.CalendarModel_ReadForMaintenance(_calendarModelRID);
			_dsCalendarModelPeriods = _cd.CalendarModelPeriods_ReadForMaintenance(_calendarModelRID);
			ugridPeriods.DataSource = _dsCalendarModelPeriods.Tables[0].DefaultView;

			LoadPeriodTypeCombo();

			_dtWeek53 = _cal.Week53_Read(_calendarModelRID);
			this.uGridWeek53.DataSource = _dtWeek53;

			int rCount = _dsCalendarModel.Tables["CALENDAR_MODEL"].Rows.Count;
			if (rCount == 0)
			{
				_currRow = _dsCalendarModel.Tables["CALENDAR_MODEL"].Rows.Add(initRow);
//				_newModel = true;
			}
			else
			{
				_currRow = _dsCalendarModel.Tables["CALENDAR_MODEL"].Rows[0];
				loadForm();
//				_newModel = false;
			}

			if (!_cal.IsFirstModel(_calendarModelRID))
			{
				this.btnStartDate.Enabled = false;
			}
			this.txtStartDate.ReadOnly = true;
			txtStartDate.BackColor = System.Drawing.SystemColors.ControlLight;

			btnCancel.DialogResult = DialogResult.Cancel;
			btnSave.DialogResult = DialogResult.OK;
		}
	
		private void loadForm()
		{
//			_formLoading = true;
			txtCalendarModelName.Text = _currRow["CM_ID"].ToString();
			_calendarModelName = _currRow["CM_ID"].ToString();
			DateTime dt = Convert.ToDateTime(_currRow["START_DATE"], CultureInfo.CurrentUICulture);
			txtStartDate.Text = dt.ToString("D", CultureInfo.CurrentUICulture);
			_startDate = dt;
			txtFiscalYr.Text = _currRow["FISCAL_YEAR"].ToString();
			_fiscalYear = Convert.ToInt32(_currRow["FISCAL_YEAR"], CultureInfo.CurrentUICulture);

			// Can't update start year in model
			this.txtFiscalYr.ReadOnly = true;
			txtFiscalYr.BackColor = System.Drawing.SystemColors.ControlLight;

			btn2Seasons.Visible = false;
			btn4Quarters.Visible = false;

			DataTable dtMonth = MIDText.GetLabels((int)eCalendarModelPeriodType.Month, (int)eCalendarModelPeriodType.Month);
			if (dtMonth.Rows.Count > 0)
			{
				DataRow aRow = dtMonth.Rows[0];
				_monthLabel = aRow["TEXT_VALUE"].ToString();
			}
			DataTable dtQuarter = MIDText.GetLabels((int)eCalendarModelPeriodType.Quarter, (int)eCalendarModelPeriodType.Quarter);
			if (dtQuarter.Rows.Count > 0)
			{
				DataRow aRow = dtQuarter.Rows[0];
				_quarterLabel = aRow["TEXT_VALUE"].ToString();
			}
			DataTable dtSeason = MIDText.GetLabels((int)eCalendarModelPeriodType.Season, (int)eCalendarModelPeriodType.Season);
			if (dtSeason.Rows.Count > 0)
			{
				DataRow aRow = dtSeason.Rows[0];
				_seasonLabel = aRow["TEXT_VALUE"].ToString();
			}

			BuildContextMenus();

//			_formLoading = false;
		}

		private void Common_Load ()
		{
			try
			{
				SetText();
				
				if (FunctionSecurity.AllowUpdate)
				{

					Format_Title(eDataState.Updatable, eMIDTextCode.frm_CalendarModelMaint, null);
				}
				else
				{

					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_CalendarModelMaint, null);
				}

				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

			}
			catch ( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void BuildContextMenus()
		{
			//===============
			// Week 53 menu
			//===============
			menuItemEdit = new MenuItem("&Edit 53 Week Selections");

			// Clear all previously added MenuItems.
			contextMenu1.MenuItems.Clear();
	 
			// Add MenuItems to display for the TextBox.
			contextMenu1.MenuItems.Add(menuItemEdit);
			menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);

			this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);

			//================
			// Periods menu
			//================
			BuildPeriodContextMenu();
		}

		private void BuildPeriodContextMenu()
		{
			eCalendarModelPeriodType ePeriodType = eCalendarModelPeriodType.Month;
			if (cboPeriodType.SelectedItem != null)
			{
				int periodType = ((ComboObject)cboPeriodType.SelectedItem).Key;
				ePeriodType = (eCalendarModelPeriodType)periodType;
			}
			switch (ePeriodType)
			{
				case eCalendarModelPeriodType.None:
					break;
				case eCalendarModelPeriodType.Month:
					menuItemNew = new MenuItem("&Add " + _monthLabel);
					menuItemDelete = new MenuItem("&Delete " + _monthLabel);
					break;
				case eCalendarModelPeriodType.Quarter:
					menuItemNew = new MenuItem("&Add " + _quarterLabel);
					menuItemDelete = new MenuItem("&Delete " + _quarterLabel);
					break;
				case eCalendarModelPeriodType.Season:
					menuItemNew = new MenuItem("&Add " + _seasonLabel);
					menuItemDelete = new MenuItem("&Delete " + _seasonLabel);
					break;
				case eCalendarModelPeriodType.Year:
					break;
				default:
					break;
			}

			// Clear all previously added MenuItems.
			contextMenu2.MenuItems.Clear();

			// Add MenuItems to display for the TextBox.
            // Begin Track #5901 - JSmith - Set view only for the Calendar and received and unhandled exception
            //contextMenu2.MenuItems.Add(menuItemNew);
            //contextMenu2.MenuItems.Add(menuItemDelete);
            if (FunctionSecurity.AllowUpdate)
            {
                contextMenu2.MenuItems.Add(menuItemNew);
            }
            if (FunctionSecurity.AllowDelete)
            {
                contextMenu2.MenuItems.Add(menuItemDelete);
            }
            // End Track #5901

			menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
			menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
		}

		private void LoadPeriodTypeCombo()
		{
			DataTable dt = MIDText.GetLabels((int)eCalendarModelPeriodType.Month, (int)eCalendarModelPeriodType.Season);

			foreach (DataRow row in dt.Rows)
			{
				string text = row["TEXT_VALUE"].ToString();
				int key = Convert.ToInt32(row["TEXT_CODE"]);
				cboPeriodType.Items.Add(new ComboObject(key, text));
			}
			if (cboPeriodType.Items.Count > 0)
				cboPeriodType.SelectedIndex = 0;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				this.contextMenu1.Popup -= new System.EventHandler(this.contextMenu1_Popup);
				this.txtStartDate.TextChanged -= new System.EventHandler(this.txtStartDate_TextChanged);
				this.btnStartDate.Click -= new System.EventHandler(this.btnStartDate_Click);
				this.txtFiscalYr.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtFiscalYr_KeyPress);
				this.txtFiscalYr.TextChanged -= new System.EventHandler(this.txtFiscalYr_TextChanged);
				this.txtFiscalYr.Leave -= new System.EventHandler(this.txtFiscalYr_Leave);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.groupBox1.Enter -= new System.EventHandler(this.groupBox1_Enter);
				this.ugridPeriods.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugridPeriods_AfterRowInsert);
				this.ugridPeriods.AfterExitEditMode -= new System.EventHandler(this.ugridPeriods_AfterExitEditMode);
				this.ugridPeriods.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugridPeriods_AfterCellUpdate);
				this.ugridPeriods.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugridPeriods_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugridPeriods);
                //End TT#169
				this.contextMenu2.Popup -= new System.EventHandler(this.contextMenu2_Popup);
				this.uGridWeek53.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.uGridWeek53_MouseDown);
				this.uGridWeek53.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.uGridWeek53_BeforeCellUpdate);
				this.uGridWeek53.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uGridWeek53_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(uGridWeek53);
                //End TT#169
				this.uGridWeek53.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.uGridWeek53_BeforeEnterEditMode);
				this.txtCalendarModelName.TextChanged -= new System.EventHandler(this.txtCalendarModelName_TextChanged);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboPeriodType.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboPeriodType_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
			}
			base.Dispose( disposing );
		}
		private void uGridWeek53_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169

			uGridWeek53.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False; 

			this.uGridWeek53.DisplayLayout.Bands[0].Columns["CM_RID"].Hidden = true; // calendar model RID
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["CMP_SEQUENCE"].Hidden = true; // calendar model RID\

			this.uGridWeek53.DisplayLayout.Bands[0].Columns["WEEK53_FISCAL_YEAR"].Header.Caption = "Fiscal\nYear";
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["CMP_ID"].Header.Caption = "Period With 53rd\nWeek";
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["OFFSET_ID"].Header.Caption = "Offset";

			this.uGridWeek53.DisplayLayout.Bands[0].Columns["WEEK53_FISCAL_YEAR"].Width = 50;
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["OFFSET_ID"].Width = 115;
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["CMP_ID"].Width = 110;

			this.uGridWeek53.DisplayLayout.Bands[0].ColHeaderLines = 2;	
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["WEEK53_FISCAL_YEAR"].CellAppearance.TextHAlign =
				Infragistics.Win.HAlign.Center;
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["CMP_ID"].CellAppearance.TextHAlign =
				Infragistics.Win.HAlign.Center;
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["OFFSET_ID"].Header.VisiblePosition = 4;

            //Begin TT#15 - RBeck- Disable the dropdown selection "DropWeek53"
            this.uGridWeek53.DisplayLayout.Bands[0].Columns["OFFSET_ID"].CellActivation = Activation.NoEdit;
            // End TT#15

			this.uGridWeek53.DisplayLayout.Bands[0].Columns["WEEK53_FISCAL_YEAR"].CellAppearance.BackColor = SystemColors.ControlLight;
			this.uGridWeek53.DisplayLayout.Bands[0].Columns["CMP_ID"].CellAppearance.BackColor = SystemColors.ControlLight;

			// Take care of offset drop down
			DataTable _dtOffset = MIDText.GetLabels((int)DataCommon.eWeek53Offset.DropWeek53, (int)DataCommon.eWeek53Offset.Offset1Week);
			uGridWeek53.DisplayLayout.ValueLists.Clear();
			ValueList objValueList = uGridWeek53.DisplayLayout.ValueLists.Add(("Offset"));

			foreach(DataRow dr in _dtOffset.Rows)
			{
				int code = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
				string text = (string)dr["TEXT_VALUE"];

				objValueList.ValueListItems.Add(code, text);
			}

			uGridWeek53.DisplayLayout.Bands[0].Columns["OFFSET_ID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
			uGridWeek53.DisplayLayout.Bands[0].Columns["OFFSET_ID"].ValueList = uGridWeek53.DisplayLayout.ValueLists["Offset"];

            // Begin TT#668-MD - JSmith - Windows 8 - Installer issues
            foreach (UltraGridColumn cColumn in this.uGridWeek53.DisplayLayout.Bands[0].Columns)
            {
                if (!cColumn.Hidden)
                {
                    cColumn.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                }
            }
            // End TT#668-MD - JSmith - Windows 8 - Installer issues

		}
		private void ugridPeriods_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169

			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CM_RID"].Hidden = true; // calendar model RID
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_TYPE"].Hidden = true;
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_SEQUENCE"].Width = 60;
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_SEQUENCE"].Header.Caption = "Sequence";
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_SEQUENCE"].CellAppearance.TextHAlign =
				Infragistics.Win.HAlign.Center;
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_ID"].Width = 97;
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_ID"].Header.Caption = "Name";
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_ABBREVIATION"].Width = 50;
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["CMP_ABBREVIATION"].Header.Caption = "Abbrv.";
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["NO_OF_TIME_PERIODS"].Width = 65;
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["NO_OF_TIME_PERIODS"].Header.Caption = "Weeks in\nPeriod";
			this.ugridPeriods.DisplayLayout.Bands[0].Columns["NO_OF_TIME_PERIODS"].CellAppearance.TextHAlign =
				Infragistics.Win.HAlign.Center;
			this.ugridPeriods.DisplayLayout.Bands[0].ColHeaderLines = 2;

			this.ugridPeriods.DisplayLayout.Bands[0].AddButtonCaption = "Calendar Model Period";
			//this.ugridPeriods.Rows[0].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True; 
			//this.ugridPeriods.DisplayLayout.Override.HeaderAppearance.FontData.Bold = 
			//	Infragistics.Win.DefaultableBoolean.True;

			foreach (UltraGridColumn cColumn in this.ugridPeriods.DisplayLayout.Bands[0].Columns)
			{
				cColumn.Layout.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
				//cColumn.CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
				//cColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                // Begin TT#668-MD - JSmith - Windows 8 - Installer issues
                if (!cColumn.Hidden)
                {
                    cColumn.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                }
                // End TT#668-MD - JSmith - Windows 8 - Installer issues
			}

			AccumWeeks();
		}
		
		private int AccumWeeks()
		{
			_numWeeks = 0;
			foreach(UltraGridRow gRow in ugridPeriods.Rows)
			{
				if (gRow.Cells["NO_OF_TIME_PERIODS"].Value != DBNull.Value &&
					(int)gRow.Cells["CMP_TYPE"].Value == (int)eCalendarModelPeriodType.Month)
					_numWeeks += Convert.ToInt32(gRow.Cells["NO_OF_TIME_PERIODS"].Value, CultureInfo.CurrentUICulture); 
			}

			lblTotalWks.Text = _numWeeks.ToString(CultureInfo.CurrentUICulture);

			return _numWeeks;
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarModelMaint));
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
            this.lblModelName = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.btnStartDate = new System.Windows.Forms.Button();
            this.lblFiscalYear = new System.Windows.Forms.Label();
            this.txtFiscalYr = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotalWks = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn2Seasons = new System.Windows.Forms.Button();
            this.btn4Quarters = new System.Windows.Forms.Button();
            this.cboPeriodType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btn454 = new System.Windows.Forms.Button();
            this.ugridPeriods = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenu2 = new System.Windows.Forms.ContextMenu();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uGridWeek53 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtCalendarModelName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugridPeriods)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uGridWeek53)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblModelName
            // 
            this.lblModelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelName.Location = new System.Drawing.Point(18, 8);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(88, 15);
            this.lblModelName.TabIndex = 0;
            this.lblModelName.Text = "Model Name:";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Location = new System.Drawing.Point(18, 60);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(88, 15);
            this.lblStartDate.TabIndex = 2;
            this.lblStartDate.Text = "Start Date:";
            // 
            // txtStartDate
            // 
            this.txtStartDate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtStartDate.Location = new System.Drawing.Point(118, 57);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(166, 20);
            this.txtStartDate.TabIndex = 3;
            this.txtStartDate.TextChanged += new System.EventHandler(this.txtStartDate_TextChanged);
            // 
            // btnStartDate
            // 
            this.btnStartDate.Image = ((System.Drawing.Image)(resources.GetObject("btnStartDate.Image")));
            this.btnStartDate.Location = new System.Drawing.Point(284, 56);
            this.btnStartDate.Name = "btnStartDate";
            this.btnStartDate.Size = new System.Drawing.Size(19, 21);
            this.btnStartDate.TabIndex = 4;
            this.btnStartDate.Click += new System.EventHandler(this.btnStartDate_Click);
            // 
            // lblFiscalYear
            // 
            this.lblFiscalYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalYear.Location = new System.Drawing.Point(18, 35);
            this.lblFiscalYear.Name = "lblFiscalYear";
            this.lblFiscalYear.Size = new System.Drawing.Size(90, 15);
            this.lblFiscalYear.TabIndex = 5;
            this.lblFiscalYear.Text = "Fiscal Year:";
            // 
            // txtFiscalYr
            // 
            this.txtFiscalYr.Location = new System.Drawing.Point(118, 31);
            this.txtFiscalYr.Name = "txtFiscalYr";
            this.txtFiscalYr.Size = new System.Drawing.Size(35, 20);
            this.txtFiscalYr.TabIndex = 2;
            this.txtFiscalYr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFiscalYr.TextChanged += new System.EventHandler(this.txtFiscalYr_TextChanged);
            this.txtFiscalYr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiscalYr_KeyPress);
            this.txtFiscalYr.Leave += new System.EventHandler(this.txtFiscalYr_Leave);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(150, 589);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(242, 589);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 351);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Total Weeks in Year...";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalWks
            // 
            this.lblTotalWks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalWks.Location = new System.Drawing.Point(130, 351);
            this.lblTotalWks.Name = "lblTotalWks";
            this.lblTotalWks.Size = new System.Drawing.Size(37, 15);
            this.lblTotalWks.TabIndex = 16;
            this.lblTotalWks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTotalWks);
            this.groupBox1.Controls.Add(this.btn2Seasons);
            this.groupBox1.Controls.Add(this.btn4Quarters);
            this.groupBox1.Controls.Add(this.cboPeriodType);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btn454);
            this.groupBox1.Controls.Add(this.ugridPeriods);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 217);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 369);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Period Definitions";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btn2Seasons
            // 
            this.btn2Seasons.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2Seasons.Location = new System.Drawing.Point(133, 11);
            this.btn2Seasons.Name = "btn2Seasons";
            this.btn2Seasons.Size = new System.Drawing.Size(118, 26);
            this.btn2Seasons.TabIndex = 24;
            this.btn2Seasons.Text = "Define 2 Seasons";
            this.btn2Seasons.Click += new System.EventHandler(this.btn2Seasons_Click);
            // 
            // btn4Quarters
            // 
            this.btn4Quarters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4Quarters.Location = new System.Drawing.Point(156, 10);
            this.btn4Quarters.Name = "btn4Quarters";
            this.btn4Quarters.Size = new System.Drawing.Size(118, 26);
            this.btn4Quarters.TabIndex = 23;
            this.btn4Quarters.Text = "Define 4 Quarters";
            this.btn4Quarters.Click += new System.EventHandler(this.btn4Quarters_Click);
            // 
            // cboPeriodType
            // 
            this.cboPeriodType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPeriodType.Location = new System.Drawing.Point(6, 15);
            this.cboPeriodType.Name = "cboPeriodType";
            this.cboPeriodType.Size = new System.Drawing.Size(121, 21);
            this.cboPeriodType.TabIndex = 22;
            this.cboPeriodType.SelectionChangeCommitted += new System.EventHandler(this.cboPeriodType_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboPeriodType.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboPeriodType_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // btn454
            // 
            this.btn454.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn454.Location = new System.Drawing.Point(175, 11);
            this.btn454.Name = "btn454";
            this.btn454.Size = new System.Drawing.Size(118, 26);
            this.btn454.TabIndex = 21;
            this.btn454.Text = "Define 4-5-4 Year";
            this.btn454.Click += new System.EventHandler(this.btn454_Click);
            // 
            // ugridPeriods
            // 
            this.ugridPeriods.ContextMenu = this.contextMenu2;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugridPeriods.DisplayLayout.Appearance = appearance1;
            this.ugridPeriods.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugridPeriods.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugridPeriods.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugridPeriods.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugridPeriods.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugridPeriods.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugridPeriods.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugridPeriods.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugridPeriods.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugridPeriods.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugridPeriods.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugridPeriods.Location = new System.Drawing.Point(6, 41);
            this.ugridPeriods.Name = "ugridPeriods";
            this.ugridPeriods.Size = new System.Drawing.Size(294, 304);
            this.ugridPeriods.TabIndex = 20;
            this.ugridPeriods.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugridPeriods_AfterCellUpdate);
            this.ugridPeriods.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugridPeriods_InitializeLayout);
            this.ugridPeriods.AfterExitEditMode += new System.EventHandler(this.ugridPeriods_AfterExitEditMode);
            this.ugridPeriods.AfterRowsDeleted += new System.EventHandler(this.ugridPeriods_AfterRowsDeleted);
            this.ugridPeriods.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugridPeriods_AfterRowInsert);
            // 
            // contextMenu2
            // 
            this.contextMenu2.Popup += new System.EventHandler(this.contextMenu2_Popup);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uGridWeek53);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 134);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "53 Week Year Definitions";
            // 
            // uGridWeek53
            // 
            this.uGridWeek53.ContextMenu = this.contextMenu1;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uGridWeek53.DisplayLayout.Appearance = appearance7;
            this.uGridWeek53.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.uGridWeek53.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uGridWeek53.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uGridWeek53.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uGridWeek53.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.uGridWeek53.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uGridWeek53.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.uGridWeek53.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.uGridWeek53.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uGridWeek53.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uGridWeek53.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uGridWeek53.Location = new System.Drawing.Point(6, 17);
            this.uGridWeek53.Name = "uGridWeek53";
            this.uGridWeek53.Size = new System.Drawing.Size(294, 112);
            this.uGridWeek53.TabIndex = 0;
            this.uGridWeek53.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uGridWeek53_InitializeLayout);
            this.uGridWeek53.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.uGridWeek53_BeforeEnterEditMode);
            this.uGridWeek53.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.uGridWeek53_BeforeCellUpdate);
            this.uGridWeek53.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uGridWeek53_MouseDown);
            // 
            // txtCalendarModelName
            // 
            this.txtCalendarModelName.Location = new System.Drawing.Point(118, 6);
            this.txtCalendarModelName.Name = "txtCalendarModelName";
            this.txtCalendarModelName.Size = new System.Drawing.Size(144, 20);
            this.txtCalendarModelName.TabIndex = 1;
            this.txtCalendarModelName.TextChanged += new System.EventHandler(this.txtCalendarModelName_TextChanged);
            // 
            // CalendarModelMaint
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(329, 621);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtFiscalYr);
            this.Controls.Add(this.txtStartDate);
            this.Controls.Add(this.txtCalendarModelName);
            this.Controls.Add(this.lblFiscalYear);
            this.Controls.Add(this.btnStartDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.lblModelName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(345, 659);
            this.Name = "CalendarModelMaint";
            this.Load += new System.EventHandler(this.CalendarModelMaint_Load);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            this.Controls.SetChildIndex(this.lblStartDate, 0);
            this.Controls.SetChildIndex(this.btnStartDate, 0);
            this.Controls.SetChildIndex(this.lblFiscalYear, 0);
            this.Controls.SetChildIndex(this.txtCalendarModelName, 0);
            this.Controls.SetChildIndex(this.txtStartDate, 0);
            this.Controls.SetChildIndex(this.txtFiscalYr, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugridPeriods)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uGridWeek53)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void SetText()
			   {
				   try
				   {
					   this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
					   this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				   }
				   catch( Exception exception )
				   {
					   HandleException(exception);
				   }
			   }
		private void dataView1_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
		{
		
		}

		private void dataGrid1_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{
		
		}


		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			
			ISave();			
			
		}

		protected override bool SaveChanges()
		{
			bool newModel = false;

			if ( IsValidForm() )
			{
				_cd.OpenUpdateConnection();

				// Calendar Model changes
				if (_dsCalendarModel.HasChanges())
				{
					//						_cd.OpenUpdateConnection();
					DataTable xDataTable = this._dsCalendarModel.Tables["CALENDAR_MODEL"].GetChanges();
					if (xDataTable != null)
						_cd.CalendarModel_UpdateRowsInTable(xDataTable);
					//						_cd.CommitData();
					//						_cd.CloseUpdateConnection();
					xDataTable.Dispose();
				}
				_dsCalendarModel.AcceptChanges();

				_calendarModelName = txtCalendarModelName.Text;
				_fiscalYear = Convert.ToInt32(txtFiscalYr.Text, CultureInfo.CurrentUICulture);
				_startDate = Convert.ToDateTime(txtStartDate.Text, CultureInfo.CurrentUICulture);

				if (_calendarModelRID == 0)  // new row
				{
					newModel = true;
					DataTable dt = _cd.CalendarModel_ReadKey(_calendarModelName, _fiscalYear);
					if (dt.Rows.Count == 1)
					{
						_calendarModelRID = Convert.ToInt32( dt.Rows[0]["CM_RID"], CultureInfo.CurrentUICulture );
					}
				}

				// update any new periods with calendar model RID
				foreach(DataRow dr in _dsCalendarModelPeriods.Tables["CALENDAR_MODEL_PERIODS"].Rows)
				{
					if (dr.RowState != DataRowState.Deleted)
						dr["CM_RID"] = _calendarModelRID;
				}

				// Calendar Model Periods Changes
				if (_dsCalendarModelPeriods.HasChanges())
				{
					DataTable xDataTable = this._dsCalendarModelPeriods.Tables["CALENDAR_MODEL_PERIODS"].GetChanges();
					if (xDataTable != null)
					{
						_cd.CalendarModelPeriods_UpdateRowsInTable(xDataTable);
					}
				
					xDataTable.Dispose();
				}

				_dsCalendarModelPeriods.AcceptChanges();

				// 53rd week changes
				_cal.Week53_Delete(_calendarModelRID);

				foreach(DataRow row in this._dtWeek53.Rows)
				{
					int year = Convert.ToInt32(row["WEEK53_FISCAL_YEAR"], CultureInfo.CurrentUICulture);
					int modelRID = Convert.ToInt32(row["CM_RID"], CultureInfo.CurrentUICulture);
					int seq = Convert.ToInt32(row["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);
					DataCommon.eWeek53Offset offset = (eWeek53Offset)Convert.ToInt32(row["OFFSET_ID"], CultureInfo.CurrentUICulture);
					
					_cal.Week53_Insert(year, _calendarModelRID, seq, offset); 
				}

				if (newModel)
				{
					_cal.Realign53WeekSelections();
				}
				

				_cd.CommitData();
				_cd.CloseUpdateConnection();

				//_cal.Week53_Populate(_calendarModelRID);

				if (_refreshCalendar)
					RefreshCalendar();
			}
			else
				DialogResult = DialogResult.None;
	
			return true;
		}

		private void btnStartDate_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			DateSelectorSingle frm = new DateSelectorSingle();
			//frm.WindowState = FormWindowState.Maximized;
			frm.SelectedDate = _startDate;
			Point pt = this.btnStartDate.Location;
			int wd = this.btnStartDate.Size.Width;
			pt.X += wd;
			frm.StartPosition = FormStartPosition.Manual;
			frm.SetDesktopLocation(pt.X, pt.Y);

			frm.ShowDialog(this);
			Cursor.Current = Cursors.Default;
			//if (!_formLoading)
			//{
				if (frm.SelectedDate.ToShortDateString() != _startDate.ToShortDateString())
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CalendarStartDateChange), 
						"Changing Calendar Start Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			//}
			_startDate = frm.SelectedDate;
			this.txtStartDate.ReadOnly = false;
			txtStartDate.Text = _startDate.ToString("D", CultureInfo.CurrentUICulture);
			this.txtStartDate.ReadOnly = true;
		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			if (_calendarModelRID == 0)  // new model
			{
				contextMenu1.MenuItems[0].Enabled = false; // New Group
				MessageBox.Show("Unable to define 53 week years. \nNew Models must be Saved first.");
			}
		}

		private void contextMenu2_Popup(object sender, System.EventArgs e)
		{
			// see defines and definition up under InitializeComponent
		}

		private void menuItemNew_Click(object sender, System.EventArgs e)
		{
			ugridPeriods.DisplayLayout.Bands[0].AddNew();
		}
		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			ugridPeriods.DeleteSelectedRows();
		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;

				CalendarWeek53Selector frm = new CalendarWeek53Selector(_SAB, _calendarModelRID);

				Cursor.Current = Cursors.Default;

				frm.StartPosition = FormStartPosition.CenterScreen;
				DialogResult calendarModelResult = frm.ShowDialog(this);

				if (calendarModelResult == DialogResult.OK)
				{
					DataRow drMoldelRID = frm.DtWeek53.Rows[0];
					DataRow drPeriodSeq = frm.DtWeek53.Rows[1];
					object[] key = new object[1];

					bool found = false;
					int fiscalYear = 0;
					int year = 0;
					int periodSeq = 0;
					ArrayList deletedYearList = new ArrayList();
					foreach (DataRow row in _dtWeek53.Rows)
					{
						found = false;
						year = Convert.ToInt32(row["WEEK53_FISCAL_YEAR"]);
						for (int col=0;col<drPeriodSeq.ItemArray.Length; col++)
						{
							periodSeq = Convert.ToInt32( drPeriodSeq[col], CultureInfo.CurrentUICulture );
							if (periodSeq > 0)
							{
								fiscalYear = Convert.ToInt32( frm.DtWeek53.Columns[col].Caption.ToString(CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture );
								if (year == fiscalYear)
								{
									found = true;
									break;
								}
							}
						}
				
						if (!found)
						{
							deletedYearList.Add(year);
						}

					}

					foreach (int dYear in deletedYearList)
					{
						key[0] = dYear;
						DataRow dr = _dtWeek53.Rows.Find(key);
						if (dr != null)
							_dtWeek53.Rows.Remove(dr);

					}


					int rowPosition = 0;
					for (int col=0;col<drPeriodSeq.ItemArray.Length; col++)
					{
						found = true;
						periodSeq = Convert.ToInt32( drPeriodSeq[col], CultureInfo.CurrentUICulture );
						if (periodSeq > 0)
						{
							found = false;
							fiscalYear = Convert.ToInt32( frm.DtWeek53.Columns[col].Caption.ToString(CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture );
							key[0] = fiscalYear;
							DataRow dr = _dtWeek53.Rows.Find(key);
							if (dr != null)
							{
								rowPosition++;
								found = true;									
							}
						}


						if (!found)
						{
	

							int cm_rid = Convert.ToInt32( drMoldelRID[col], CultureInfo.CurrentUICulture );
							DataRow newRow = _dtWeek53.NewRow();
							newRow["WEEK53_FISCAL_YEAR"] = fiscalYear;
							newRow["CM_RID"] = cm_rid;
							newRow["CMP_SEQUENCE"] = periodSeq;

							foreach (DataRow dr in _dsCalendarModelPeriods.Tables[0].Rows)
							{
								int sequence = Convert.ToInt32( dr["CMP_SEQUENCE"], CultureInfo.CurrentUICulture );
								if (sequence == periodSeq)
									newRow["CMP_ID"] = dr["CMP_ID"];
							}
							newRow["OFFSET_ID"] = eWeek53Offset.Offset1Week;
							_dtWeek53.Rows.InsertAt(newRow, rowPosition);
						}
					}

					this.uGridWeek53.DataSource = _dtWeek53;
					this.uGridWeek53.Refresh();
					_refreshCalendar = true;

				}
				frm.Dispose();
			}
			catch ( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		private void RefreshCalendar()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				DateTime refreshTime = DateTime.Now;

				if (_SAB.ApplicationServerSession != null)
					_SAB.ApplicationServerSession.RefreshCalendar(refreshTime);
				if (_SAB.ClientServerSession != null)
					_SAB.ClientServerSession.RefreshCalendar(refreshTime);
//				if (_SAB.ControlServerSession != null)
//					_SAB.ControlServerSession.RefreshCalendar(refreshTime);
				if (_SAB.HierarchyServerSession != null)
					_SAB.HierarchyServerSession.RefreshCalendar(refreshTime);
				if (_SAB.StoreServerSession != null)
					_SAB.StoreServerSession.RefreshCalendar(refreshTime);

				Cursor.Current = Cursors.Default;
			}
			catch ( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		private void txtCalendarModelName_TextChanged(object sender, System.EventArgs e)
		{
			_currRow["CM_ID"] = txtCalendarModelName.Text;
		}
		private void txtStartDate_TextChanged(object sender, System.EventArgs e)
		{
			_currRow["START_DATE"] = txtStartDate.Text;
			_refreshCalendar = true;
		}
		private void txtFiscalYr_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				try
				{
					int year = Convert.ToInt32(txtFiscalYr.Text, CultureInfo.CurrentUICulture);
					_currRow["FISCAL_YEAR"] = year;
				}
				catch
				{
					// swallow error
				}
			}
			catch ( System.FormatException )
			{
				MessageBox.Show("Invalid format entered for:  Fiscal Year");
			}
			catch ( System.ArgumentException )
			{
				MessageBox.Show("Invalid format entered for:  Fiscal Year");
			}
		}

		private void ugridPeriods_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			e.Row.Cells["CM_RID"].Value = _calendarModelRID;
			int periodType = ((ComboObject)cboPeriodType.SelectedItem).Key;
			eCalendarModelPeriodType ePeriodType = (eCalendarModelPeriodType)periodType;
			if (ePeriodType == eCalendarModelPeriodType.Month)
				AccumWeeks();
		}

		private void ugridPeriods_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			int periodType = ((ComboObject)cboPeriodType.SelectedItem).Key;
			eCalendarModelPeriodType ePeriodType = (eCalendarModelPeriodType)periodType;
			if (ePeriodType == eCalendarModelPeriodType.Month)
			{
				if (e.Cell.Column.Key == "NO_OF_TIME_PERIODS")
					AccumWeeks();
			}
		}

		private void txtFiscalYr_Leave(object sender, System.EventArgs e)
		{
			
		}

		private void txtFiscalYr_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}

		private bool IsValidForm()
		{
			bool proceed = true;

			if (this.txtCalendarModelName.ToString().Trim().Length == 0)
			{
				_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameRequiredToSave));
				AttachErrors(this.txtCalendarModelName);
				proceed = false;
			}

			int year = 0;
			if (proceed)
			{
				try
				{
					year = Convert.ToInt32(txtFiscalYr.Text, CultureInfo.CurrentUICulture);
					if (!_cal.IsFirstModel(_calendarModelRID))
					{
						YearProfile yp = _cal.GetYear(year);
						_startDate = yp.Date;
						txtStartDate.Text = yp.Date.ToString("D", CultureInfo.CurrentUICulture);
						_refreshCalendar = true;
					}
				}
				catch
				{
					_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
					AttachErrors(this.txtFiscalYr);
					proceed = false;
				}
			}

			if (proceed)
				proceed = IsValidYear(year);
			if (proceed)
				proceed = IsValidNumberOfWeeks();
			if (proceed)
				proceed = IsValidPeriods();
			
			return proceed;
		}

		private bool IsValidYear(int year)
		{
			try
			{
				bool proceed = true;

				DateTime now = DateTime.Now;
				int lowLimit = now.Year - 100;
				int highLimit = now.Year + 100;
				if (year < lowLimit || year > highLimit)
				{
					_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ExtremeStartYear));
					AttachErrors(this.txtFiscalYr);
					proceed = false;
				}

				if (proceed)
				{
					ArrayList calModels = _cal.GetCalendarModels();
					foreach (CalendarModel calModel in calModels)
					{
						if (year == calModel.FiscalYear && calModel.RID != _calendarModelRID)
						{
							_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStartYear));
							AttachErrors(this.txtFiscalYr);
							proceed = false;
						}
					}
				}

				if (proceed)
				{
					if (_cal.FirstCalendarFiscalYear > 0)
					{
						if (year < _cal.FirstCalendarFiscalYear || year > _cal.LastCalendarFiscalYear)
						{
							_cal.AddYear(year);
						}
					}
				}
				return proceed;
			}
			catch
			{
				throw;
			}
		}

		private bool IsValidNumberOfWeeks()
		{
			if (_numWeeks != 52)
			{
				_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidNumberOfWeeks));
				AttachErrors(this.ugridPeriods);
				ugridPeriods.Focus();
				return false;
			}
			return true;
			
		}

		private bool IsValidPeriods()
		{
			bool proceed = SetPeriodCounts();

			if (proceed)
			{
				if (_quartersInSeason != _totalQuarters)
				{
					_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MismatchedSeasonQuarters));
					AttachErrors(this.ugridPeriods);
					ugridPeriods.Focus();
					return false;
				}
				if (_monthsInQuarter != _totalMonths)
				{
					_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MismatchedQuarterMonths));
					AttachErrors(this.ugridPeriods);
					ugridPeriods.Focus();
					return false;
				}
			}
			return proceed;
		}

		private bool SetPeriodCounts()
		{
		    _quartersInSeason = 0;
			_monthsInQuarter = 0;
			_totalQuarters = 0;
			_totalMonths = 0;
			int totalSeasons = 0;
	
			foreach (DataRow row in _dsCalendarModelPeriods.Tables[0].Rows)
			{
				if (row.RowState != DataRowState.Deleted)
				{
					eCalendarModelPeriodType cmpType = (eCalendarModelPeriodType)(Convert.ToInt32(row["CMP_TYPE"]));
					if (cmpType == eCalendarModelPeriodType.Quarter)
					{
						int months = Convert.ToInt32(row["NO_OF_TIME_PERIODS"]);
						_monthsInQuarter += months;
						_totalQuarters++;
					}
					if (cmpType == eCalendarModelPeriodType.Season)
					{
						int quarters = Convert.ToInt32(row["NO_OF_TIME_PERIODS"]);
						_quartersInSeason += quarters;
						totalSeasons++;
					}
					if (cmpType == eCalendarModelPeriodType.Month)
					{
						_totalMonths++;
					}
				}
			}
			if (_totalQuarters == 0)
			{
				_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MissingQuarterDefinition));
				AttachErrors(this.ugridPeriods);
				ugridPeriods.Focus();
				return false;
			}

			if (totalSeasons == 0)
			{
				_errorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MissingSeasonDefinition));
				AttachErrors(this.ugridPeriods);
				ugridPeriods.Focus();
				return false;
			}

			return true;
		}


		private void ugridPeriods_AfterExitEditMode(object sender, System.EventArgs e)
		{
			_refreshCalendar = true;
		}

		private void uGridWeek53_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
//			Infragistics.Win.UIElement mouseUIElement;
//			Infragistics.Win.UltraWinGrid.UltraGridCell aCell;
//	
//			// retrieve the UIElement from the location of the mouse 
//			mouseUIElement = uGridWeek53.DisplayLayout.UIElement.ElementFromPoint(_point);
//			if ( mouseUIElement == null ) { e.Cancel = true; }
//
//			aCell = (UltraGridCell)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)); 
//			if ( aCell == null ) 
//			{ 
//				e.Cancel = true; 
//			}
//			else if (aCell.Column.Key != "OFFSET_ID") 
//			{ 
//				e.Cancel = true; 
//			}
		}

		private void uGridWeek53_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Infragistics.Win.UIElement mouseUIElement;
			Infragistics.Win.UltraWinGrid.UltraGridCell aCell;
	
			// retrieve the UIElement from the location of the mouse 
			mouseUIElement = uGridWeek53.DisplayLayout.UIElement.ElementFromPoint(_point);
			if ( mouseUIElement == null ) { e.Cancel = true; }

			aCell = (UltraGridCell)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)); 
			if ( aCell == null ) 
			{ 
				e.Cancel = true; 
			}
			else if (aCell.Column.Key != "OFFSET_ID") 
			{ 
				e.Cancel = true; 
			}
			
		}

		private void uGridWeek53_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_point = new Point(e.X, e.Y);
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
//			
//		}

		override public void ISave()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor; // issue 3702 - stodd

				SaveChanges();

				Cursor.Current = Cursors.Default; // issue 3702 - stodd
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
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

		private void CalendarModelMaint_Load(object sender, System.EventArgs e)
		{
		
		}

		private void btn454_Click(object sender, System.EventArgs e)
		{
			try
			{
				bool proceed = true;
				if (ugridPeriods.Rows.Count > 0)
				{
					string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MonthDefinitionBeingReplaced);
					if (ShowWarningPrompt(msg) == DialogResult.Yes)
						proceed = true;
					else
						proceed = false;
				}

				if (proceed)
				{
					Set454Months();
				}
			}
			catch
			{
				throw;
			}
		}

		private void Set454Months()
		{
			try
			{
				// Removes any previous rows
				List<DataRow> rowsToDelete = new List<DataRow>(); 
				foreach (DataRow row in _dsCalendarModelPeriods.Tables[0].Rows)
				{
					if (row.RowState != DataRowState.Deleted)
					{
						eCalendarModelPeriodType cmpType = (eCalendarModelPeriodType)(Convert.ToInt32(row["CMP_TYPE"]));
						if (cmpType == eCalendarModelPeriodType.Month)
						{
							rowsToDelete.Add(row);
						}
					}
				}
				foreach (DataRow row in rowsToDelete)
				{
					row.Delete();
				}

				// Adds new 4-5-4 periods
				for (int i=0;i<12;i++)
				{
					DataRow newRow = _dsCalendarModelPeriods.Tables[0].NewRow();
					int seq = i + 1;
					newRow["CMP_SEQUENCE"] = seq;
					newRow["CMP_ID"] = GetPeriodName(i);;
					newRow["CMP_ABBREVIATION"] = GetPeriodAbbreviation(i);
					if (seq == 2 || seq == 5 || seq ==8 || seq == 11)
						newRow["NO_OF_TIME_PERIODS"] = 5;
					else
						newRow["NO_OF_TIME_PERIODS"] = 4;
					newRow["CMP_TYPE"] = (int)eCalendarModelPeriodType.Month;
					_dsCalendarModelPeriods.Tables[0].Rows.Add(newRow);
				}
				AccumWeeks();
			}
			catch
			{
				throw;
			}
		}

		private void btn4Quarters_Click(object sender, EventArgs e)
		{
			try
			{
				bool proceed = true;
				if (ugridPeriods.Rows.Count > 0)
				{
					string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QuarterDefinitionBeingReplaced);
					if (ShowWarningPrompt(msg) == DialogResult.Yes)
						proceed = true;
					else
						proceed = false;
				}

				if (proceed)
				{
					Set4Quarters();
				}
			}
			catch
			{
				throw;
			}
		}

		private void Set4Quarters()
		{
			try
			{
				// Removes any previous rows
				List<DataRow> rowsToDelete = new List<DataRow>();  
				foreach (DataRow row in _dsCalendarModelPeriods.Tables[0].Rows)
				{
					if (row.RowState != DataRowState.Deleted)
					{
						eCalendarModelPeriodType cmpType = (eCalendarModelPeriodType)(Convert.ToInt32(row["CMP_TYPE"]));
						if (cmpType == eCalendarModelPeriodType.Quarter)
						{
							rowsToDelete.Add(row);
						}
					}
				}
				foreach (DataRow row in rowsToDelete)
				{
					row.Delete();
				}

				for (int i=0;i<4;i++)
				{
					DataRow newRow = _dsCalendarModelPeriods.Tables[0].NewRow();
					int seq = i + 1;
					newRow["CMP_SEQUENCE"] = seq;
					newRow["CMP_ID"] = GetQuarterName(i);;
					newRow["CMP_ABBREVIATION"] = GetQuarterAbbreviation(i);
					newRow["NO_OF_TIME_PERIODS"] = 3;
					newRow["CMP_TYPE"] = (int)eCalendarModelPeriodType.Quarter;
					_dsCalendarModelPeriods.Tables[0].Rows.Add(newRow);
				}
			}
			catch
			{
				throw;
			}
		}

		private void btn2Seasons_Click(object sender, EventArgs e)
		{
			try
			{
				bool proceed = true;
				if (ugridPeriods.Rows.Count > 0)
				{
					string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SeasonDefinitionBeingReplaced);
					if (ShowWarningPrompt(msg) == DialogResult.Yes)
						proceed = true;
					else
						proceed = false;
				}

				if (proceed)
				{
					Set2Seasons();
				}
			}
			catch
			{
				throw;
			}
		}

		private void Set2Seasons()
		{
			try
			{
				// Removes any previous rows
				List<DataRow> rowsToDelete = new List<DataRow>();  
				foreach (DataRow row in _dsCalendarModelPeriods.Tables[0].Rows)
				{
					if (row.RowState != DataRowState.Deleted)
					{
						eCalendarModelPeriodType cmpType = (eCalendarModelPeriodType)(Convert.ToInt32(row["CMP_TYPE"]));
						if (cmpType == eCalendarModelPeriodType.Season)
						{
							rowsToDelete.Add(row);
						}
					}
				}
				foreach (DataRow row in rowsToDelete)
				{
					row.Delete();
				}

				
				for (int i=0;i<2;i++)
				{
					DataRow newRow = _dsCalendarModelPeriods.Tables[0].NewRow();
					int seq = i + 1;
					newRow["CMP_SEQUENCE"] = seq;
					newRow["CMP_ID"] = GetSeasonName(i);;
					newRow["CMP_ABBREVIATION"] = GetSeasonAbbreviation(i);
					newRow["NO_OF_TIME_PERIODS"] = 2;
					newRow["CMP_TYPE"] = (int)eCalendarModelPeriodType.Season;
					_dsCalendarModelPeriods.Tables[0].Rows.Add(newRow);
				}
			}
			catch
			{
				throw;
			}
		}
		private string GetPeriodName(int index)
		{
			try
			{
				string periodName = "Unknown";
				if (_dtPeriodNames == null)
					_dtPeriodNames = MIDText.GetTextType(eMIDTextType.ePeriodName, eMIDTextOrderBy.TextCode);
		
				DataRow aRow = _dtPeriodNames.Rows[index];
				periodName = aRow["TEXT_VALUE"].ToString();
				return periodName;
			}
			catch
			{
				throw;
			}
		}

		private string GetPeriodAbbreviation(int index)
		{
			try
			{
				string periondAbbreviation = "Unknown";
				if (_dtPeriodAbbreviations == null)
					_dtPeriodAbbreviations = MIDText.GetTextType(eMIDTextType.ePeriodAbbreviation, eMIDTextOrderBy.TextCode);
		
				DataRow aRow = _dtPeriodAbbreviations.Rows[index];
				periondAbbreviation = aRow["TEXT_VALUE"].ToString();
				return periondAbbreviation;
			}
			catch
			{
				throw;
			}
		}

		private string GetQuarterName(int index)
		{
			try
			{
				string quarterName = "Unknown";
				if (_dtQuarterNames == null)
					_dtQuarterNames = MIDText.GetTextType(eMIDTextType.eQuarterName, eMIDTextOrderBy.TextCode);

				DataRow aRow = _dtQuarterNames.Rows[index];
				quarterName = aRow["TEXT_VALUE"].ToString();
				return quarterName;
			}
			catch
			{
				throw;
			}
		}

		private string GetQuarterAbbreviation(int index)
		{
			try
			{
				string quarterAbbreviation = "Unknown";
				if (_dtQuarterAbbreviations == null)
					_dtQuarterAbbreviations = MIDText.GetTextType(eMIDTextType.eQuarterAbbreviation, eMIDTextOrderBy.TextCode);

				DataRow aRow = _dtQuarterAbbreviations.Rows[index];
				quarterAbbreviation = aRow["TEXT_VALUE"].ToString();
				return quarterAbbreviation;
			}
			catch
			{
				throw;
			}
		}

		private string GetSeasonName(int index)
		{
			try
			{
				string seasonName = "Unknown";
				if (_dtSeasonNames == null)
					_dtSeasonNames = MIDText.GetTextType(eMIDTextType.eSeasonName, eMIDTextOrderBy.TextCode);

				DataRow aRow = _dtSeasonNames.Rows[index];
				seasonName = aRow["TEXT_VALUE"].ToString();
				return seasonName;
			}
			catch
			{
				throw;
			}
		}

		private string GetSeasonAbbreviation(int index)
		{
			try
			{
				string seasonAbbreviation = "Unknown";
				if (_dtSeasonAbbreviations == null)
					_dtSeasonAbbreviations = MIDText.GetTextType(eMIDTextType.eSeasonAbbreviation, eMIDTextOrderBy.TextCode);

				DataRow aRow = _dtSeasonAbbreviations.Rows[index];
				seasonAbbreviation = aRow["TEXT_VALUE"].ToString();
				return seasonAbbreviation;
			}
			catch
			{
				throw;
			}
		}





		private DialogResult ShowWarningPrompt(string msg)
		{
			try
			{
				DialogResult drResult;
				drResult = DialogResult.Yes;

				drResult = MessageBox.Show(msg,	"Confirmation",	MessageBoxButtons.YesNo);

				return drResult;
			}
			catch (Exception ex)
			{
				HandleException(ex, "ShowWarningPrompt");
				return DialogResult.No;
			}	
		}

		/// <summary>
		/// Used to attach an error provider with messages to a Windows form control.
		/// </summary>
		/// <param name="pControl"></param>
		private void AttachErrors(Control pControl)
		{
			try
			{
				string Msg = "";

				for (int errIdx=0; errIdx <= _errorMessages.Count - 1; errIdx++)
				{
					Msg = (errIdx == 0) ? _errorMessages[errIdx].ToString() : Msg + "\n" + _errorMessages[errIdx].ToString();
				}

				ErrorProvider.SetError(pControl, Msg);
				_errorMessages.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "CalendarModelMaint.AttachErrors(Control pControl)");
			}	
		}

		private void cboPeriodType_SelectionChangeCommitted(object sender, EventArgs e)
		{
			int periodType = ((ComboObject)cboPeriodType.SelectedItem).Key;
			eCalendarModelPeriodType ePeriodType = (eCalendarModelPeriodType)periodType;
			if (_dsCalendarModelPeriods != null)
			{
				_dsCalendarModelPeriods.Tables[0].DefaultView.RowFilter = "CMP_TYPE = " + periodType.ToString();
			}
			switch (ePeriodType)
			{
				case eCalendarModelPeriodType.None:
					break;
				case eCalendarModelPeriodType.Month:
					btn454.Visible = true;
					btn4Quarters.Visible = false;
					btn2Seasons.Visible = false;
					this.ugridPeriods.DisplayLayout.Bands[0].Columns["NO_OF_TIME_PERIODS"].Header.Caption = "Weeks in\nPeriod";
					break;
				case eCalendarModelPeriodType.Quarter:
					btn454.Visible = false;
					btn4Quarters.Visible = true;
					btn4Quarters.Location = btn454.Location;
					btn2Seasons.Visible = false;
					this.ugridPeriods.DisplayLayout.Bands[0].Columns["NO_OF_TIME_PERIODS"].Header.Caption = "Months in\nQuarter";
					break;
				case eCalendarModelPeriodType.Season:
					btn454.Visible = false;
					btn4Quarters.Visible = false;
					btn2Seasons.Visible = true;
					btn2Seasons.Location = btn454.Location;
					this.ugridPeriods.DisplayLayout.Bands[0].Columns["NO_OF_TIME_PERIODS"].Header.Caption = "Quarters in\nSeason";
					break;
				case eCalendarModelPeriodType.Year:
					break;
				default:
					break;
			}
			BuildPeriodContextMenu();
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboPeriodType_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboPeriodType_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		private void ugridPeriods_AfterRowsDeleted(object sender, EventArgs e)
		{
			int periodType = ((ComboObject)cboPeriodType.SelectedItem).Key;
			eCalendarModelPeriodType ePeriodType = (eCalendarModelPeriodType)periodType;
			if (ePeriodType == eCalendarModelPeriodType.Month)
				AccumWeeks();
		}


	}
	
}
