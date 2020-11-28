using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinScrollBar;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for CalendarDateSelector.
	/// </summary>
	public class CalendarDateSelector : MIDFormBase, Infragistics.Win.ISelectionStrategyFilter
	{
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
		private System.Windows.Forms.Button btnReset;

		private DateRangeProfile _selectedDateRange = null;
		//Begin Issue 4677 - JSmith - saves invalid date range
		private DateRangeProfile _origDateRange = null;
		//End Issue 4677

		private int _dateRangeRID = 0;
		private ArrayList _selectedDates = null;

		private Profile _anchorDate = null;
		private DayProfile _anchorDay = null;
		private WeekProfile _anchorWeek = null;
		private PeriodProfile _anchorPeriod = null;
		private int _anchorDateRangeRID = 0;

		private bool _restrictToSingleDate;
		private bool _restrictToOnlyWeeks;
		private bool _restrictToOnlyPeriods;

		//======================================================================================
		// These two switdches are mutually exclusive because of how they display on the form.
		private bool _allowReoccurring;
		private bool _allowDynamicSwitch;
		//======================================================================================
		private bool _allowDynamic;
		private bool _allowDynamicToCurrent;
		private bool _allowDynamicToPlan;
		private bool _allowDynamicToStoreOpen;
		private bool _defaultToStatic;

		private bool _newSelection;
		private bool _reset;

        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
        private bool _anchorDateWasOverridden = false;
        private bool _overrideNullAnchorDateDefaults = false;
        // End Track #5833
		// Begin Track #6220 - JSmith - Can not call base method
		private bool _selectRow = false;
		// End Track #6220

		private Color _bColor;
		private Color _fColor;

		private Point _point;

		Infragistics.Win.UltraWinGrid.UltraGridCell _grid1FirstCell = null;

		private DataCommon.eDateRangeRelativeTo _anchorDateRelativeTo = eDateRangeRelativeTo.Current;

		private DataView _dvDateRangesWithNames;
		private int _startYearAtLoad = 0;
		private int _currentYear = 0;

		MenuItem _mnuItemDelete = new MenuItem();
		MenuItem _mnuItemRename = new MenuItem();
		
		private MRSCalendar _calendar;
		private System.Windows.Forms.Label lblCurrentDay;
		private System.Windows.Forms.Label lblCurrentWeek;
		private System.Windows.Forms.RadioButton rbPeriod;
		private System.Windows.Forms.RadioButton rbWeek;
		private System.Windows.Forms.RadioButton rbDay;
		private System.Windows.Forms.GroupBox gbSelectBy;
		private System.Windows.Forms.GroupBox gbRelativeTo;
		private System.Windows.Forms.RadioButton rbRelativeToCurrent;
		private System.Windows.Forms.RadioButton rbRelativeToPlan;
		private System.Windows.Forms.RadioButton rbRelativeToStore;
		private System.Windows.Forms.Label lblPlan;
		private System.Windows.Forms.Label lblStoreOpen;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbDynamic;
		private System.Windows.Forms.RadioButton rbStatic;
		private System.Windows.Forms.Button btnSaveRange;
		private System.Windows.Forms.Button btnOk;

		private bool _formLoaded = false;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.RadioButton rbRecurring;

		private MyCustomSelectionStrategy mySelectionStrategy;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.Button btnCancel;
		// Begin MID Issue #229 - stodd
		private bool _scrollBarClick = false;
		private System.Windows.Forms.RadioButton rbDynamicSwitch;
		private System.Windows.Forms.Label lblCurrent;
		private System.Windows.Forms.Label lblCurrSalesWeek;
		private System.Windows.Forms.Label lblDailySalesThru;
		private System.Windows.Forms.Label lblDynamicSwitch;
		private System.Windows.Forms.ToolTip toolTip;
		private System.ComponentModel.IContainer components;
		// End MID Issue #229 - stodd

		private WeekProfile _selectedStartWeek;
		private WeekProfile _selectedEndWeek;
		private System.Windows.Forms.Label lblDay;
		private System.Windows.Forms.Button btnDynamicSwitch;	// Added - Issue 5171

		private bool _changedByProgram = false;

        private bool _clearDateRange = false;	// TT#207 MID Track #6451 
        private bool _bPopulateForm = true;  // TT#1953-MD - RO Web


		#region Properties
        /// <summary>
        /// Flag to identify if the form should be populated.
        /// </summary>
        public bool PopulateForm
        {
            get { return _bPopulateForm; }
            set { _bPopulateForm = value; }
        }
		/// <summary>
		/// if this is set when form opens, it will preset the values found in the DB.
		/// </summary>
		public int DateRangeRID 
		{
			get{return _dateRangeRID;}
			set{_dateRangeRID = value;}
		}
		/// <summary>
		/// This date is used as the anchor date for calculating Relative To Plan and 
		/// Relateive TO Store Open dates.  If this is set DO NOT AnchorDateRangeRID
		/// </summary>
		public Profile AnchorDate
		{
			get { return _anchorDate ; }
			set { _anchorDate = value; }
		}

        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
        /// <summary>
        /// This flag is used to override the screen defaults if an anchor date is not
        /// provided and allow the calling program to set the enabled radio buttons.
        /// </summary>
        public bool OverrideNullAnchorDateDefaults
        {
            get { return _overrideNullAnchorDateDefaults; }
            set { _overrideNullAnchorDateDefaults = value; }
        }
        // End Track #5833

		/// <summary>
		/// This DateRangeProfile RID is used to calculate the anchor date 
		/// that is then used to alculate the Relative To Plan and 
		/// Relateive TO Store Open dates.  If this is set DO NOT AnchorDate.
		/// </summary>
		public int AnchorDateRangeRID
		{
			get { return _anchorDateRangeRID ; }
			set { _anchorDateRangeRID = value; }
		}
		/// <summary>
		/// when specified this restricts the user to the selection of a single date only.
		/// </summary>
		public bool RestrictToSingleDate
		{
			get { return _restrictToSingleDate ; }
			set { _restrictToSingleDate = value; }
		}
		/// <summary>
		/// when specified restricts date range selections to weeks only
		/// </summary>
		public bool RestrictToOnlyWeeks
		{
			get { return _restrictToOnlyWeeks ; }
			set { _restrictToOnlyWeeks = value; }
		}
		/// <summary>
		/// when specified restricts date range selections to periods only
		/// </summary>
		public bool RestrictToOnlyPeriods
		{
			get { return _restrictToOnlyPeriods ; }
			set { _restrictToOnlyPeriods = value; }
		}
		/// <summary>
		/// when specified enables 'reoccurring' as a date range type. This is mutually
		/// exclusive with 'AllowDynamicSwitch'.
		/// </summary>
		public bool AllowReoccurring
		{
			get { return _allowReoccurring ; }
			set 
			{ 
				_allowReoccurring = value; 
				if (_allowReoccurring)
					_allowDynamicSwitch = false;
			}
		}
		/// <summary>
		/// when specified enables 'Dynamic Switch (Set Date)' as a date range type. This is mutually
		/// exclusive with 'reoccuring'.
		/// </summary>
		public bool AllowDynamicSwitch
		{
			get { return _allowDynamicSwitch ; }
			set 
			{ 
				_allowDynamicSwitch = value; 
				if (_allowDynamicSwitch)
					_allowReoccurring = false;
			}
		}
		/// <summary>
		/// when specified enables 'dynamic' as a date range type
		/// </summary>
		public bool AllowDynamic
		{
			get { return _allowDynamic ; }
            set { _allowDynamic = value; }
		}
		/// <summary>
		/// when specified enables 'dynamic to current' as a date range type
		/// </summary>
		/// <remarks>The default is true</remarks>
		public bool AllowDynamicToCurrent
		{
			get { return _allowDynamicToCurrent ; }
			set { _allowDynamicToCurrent = value; }
		}
		/// <summary>
		/// when specified enables 'dynamic to plan' as a date range type
		/// </summary>
		/// <remarks>The default is true</remarks>
		public bool AllowDynamicToPlan
		{
			get { return _allowDynamicToPlan ; }
			set { _allowDynamicToPlan = value; }
		}
		/// <summary>
		/// when specified enables 'dynamic to store open' as a date range type
		/// </summary>
		/// <remarks>The default is true</remarks>
		public bool AllowDynamicToStoreOpen
		{
			get { return _allowDynamicToStoreOpen ; }
			set { _allowDynamicToStoreOpen = value; }
		}
		/// <summary>
		/// When specified, forces the 'static' radio button to be the selected default.
		/// </summary>
		public bool DefaultToStatic
		{
			get { return _defaultToStatic ; }
			set { _defaultToStatic = value; }
		}
		/// <summary>
		/// Tells which date the anchor date is.  You never need to send the Current Date.
		/// </summary>
		public DataCommon.eDateRangeRelativeTo AnchorDateRelativeTo
		{
			get { return _anchorDateRelativeTo ; }
			set { _anchorDateRelativeTo = value; }
		}
		#endregion 


		public CalendarDateSelector(SessionAddressBlock _SAB) : base(_SAB)
		{
			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminCalendarRange);

			InitializeComponent();
			_allowDynamic = true;
			_allowDynamicToCurrent = true;
			_allowDynamicToPlan = true;
			_allowDynamicToStoreOpen = true;

			_calendar = _SAB.ClientServerSession.Calendar;

			_currentYear = _calendar.CurrentWeek.FiscalYear;

			_selectedDates = new ArrayList();
			_selectedDateRange = new DateRangeProfile(Include.UndefinedCalendarDateRange);
		
			_dateRangeRID = Include.UndefinedCalendarDateRange;

			_selectedStartWeek = new WeekProfile(Include.NoRID);
			_selectedEndWeek = new WeekProfile(Include.NoRID);
            // Begin TT#2972 - RMatelic - Select Date Range highlights months at a time
            ultraGrid1.StyleLibraryName = "Windows7NoHotTrackLibraryName";
            //End TT#2972 
			// assign data views to grids
			ultraGrid1.DataSource = _calendar.DateSelectionDataTable;

			BuildContextmenu();
			listBox1.ContextMenu = this.contextMenu1;
		}


		private void CalendarDateSelector_Load(object sender, System.EventArgs e)
		{
            // Begin TT#1953-MD - RO Web
            //SetReadOnly(true);  // Issue 5119
            if (PopulateForm)
            {
                SetReadOnly(true);
            }
            // End TT#1953-MD - RO Web
			_formLoaded = false;
			_reset = false;
			LoadSoftText();

			// load list box
            // Begin TT#1953-MD - RO Web
            //listBox1.BeginUpdate();
            if (PopulateForm)
            {
                listBox1.BeginUpdate();
            }
            // End TT#1953-MD - RO Web

			if (this._anchorDateRangeRID != Include.UndefinedCalendarDateRange)
			{	
				DateRangeProfile AnchorDrp = _calendar.GetDateRange(_anchorDateRangeRID);
				switch (AnchorDrp.SelectedDateType)
				{
					case eCalendarDateType.Day:
						//_anchorDate = _calendar.GetFirstDayOfRange(_anchorDateRangeRID);
						_anchorDate = _calendar.GetFirstWeekOfRange(_anchorDateRangeRID);
						break;
					case eCalendarDateType.Week:
						_anchorDate = _calendar.GetFirstWeekOfRange(_anchorDateRangeRID);
						break;
					case eCalendarDateType.Period:
						//_anchorDate = _calendar.GetFirstPeriodOfRange(_anchorDateRangeRID);
						_anchorDate = _calendar.GetFirstWeekOfRange(_anchorDateRangeRID);
						break;
				}
			}

			if (_anchorDate != null)
			{
				// converts anchor date to week date
				if (_anchorDate.ProfileType == eProfileType.Day)
					_anchorDate = ((DayProfile)_anchorDate).Week;
				else if (_anchorDate.ProfileType == eProfileType.Period)
					_anchorDate = ((PeriodProfile)_anchorDate).Weeks[0];
		
				_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
					this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);
			}
			else
				_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
					this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);

            // Begin TT#1953-MD - RO Web
            if (PopulateForm)
            {
                listBox1.DataSource = _dvDateRangesWithNames;
                listBox1.DisplayMember = "CDR_NAME";
                listBox1.ValueMember = "CDR_RID";
                listBox1.EndUpdate();
            }
            // End TT#1953-MD - RO Web

            // Create the custom selection strategy
            this.mySelectionStrategy = new MyCustomSelectionStrategy(this.ultraGrid1);
            this.ultraGrid1.SelectionStrategyFilter = this;

			InitializeSelectionCriteria();

            // Begin TT#1953-MD - RO Web
            if (!PopulateForm)
            {
                return;
            }
            // End TT#1953-MD - RO Web

			ApplyFormSettings();

			if (FunctionSecurity.AllowUpdate)
			{
				btnSaveRange.Enabled = true; 
			}
			else
			{
				btnSaveRange.Enabled = false; 
			}

			if (FunctionSecurity.AccessDenied)
			{
				listBox1.Enabled = false;
			}


			if (_selectedDateRange.StartDateKey == 0)
				ScrollToYear(_calendar.CurrentDate.FiscalYear);
			else
				ScrollToYear(_startYearAtLoad);

            //// Create the custom selection strategy
            //this.mySelectionStrategy = new MyCustomSelectionStrategy( this.ultraGrid1 );
            //this.ultraGrid1.SelectionStrategyFilter = this;
			_formLoaded = true;
		}

		private void LoadSoftText()
		{
			lblCurrSalesWeek.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CurrentSalesWeek);
			lblDailySalesThru.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DailySalesThru);
			rbPeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Period);
			rbWeek.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Week);
			rbDay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Day);
			rbDynamic.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Dynamic);
			rbStatic.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Static);
			rbRecurring.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Reoccurring);
			rbDynamicSwitch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DynamicSwitch);
			rbRelativeToCurrent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RelativeToCurrent);
			rbRelativeToPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RelativeToPlan);
			rbRelativeToStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RelativeToStore);

			toolTip.SetToolTip(rbDynamic, MIDText.GetTextOnly(eMIDTextCode.msg_DynamicToolTip));
			toolTip.SetToolTip(rbStatic, MIDText.GetTextOnly(eMIDTextCode.msg_StaticToolTip));
			toolTip.SetToolTip(rbRecurring, MIDText.GetTextOnly(eMIDTextCode.msg_RecurringToolTip));
			toolTip.SetToolTip(rbDynamicSwitch, MIDText.GetTextOnly(eMIDTextCode.msg_DynamicSwitchToolTip));
		}

		/// <summary>
		/// Does all of the initialization of data views and data tables along with setting
		/// Starting values for the window. 
		/// </summary>
		protected void InitializeSelectionCriteria()
		{
			try
			{
				ClearSelections();

				this.rbDay.Enabled = false;

                // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                _anchorDateWasOverridden = false;
                SetUpAnchorDate(null);
                //// setup anchor date if it was set
                //if (_anchorDate != null)
                //{
                //    switch (_anchorDate.ProfileType)
                //    {
                //        case eProfileType.Day:
                //            _anchorDay = (DayProfile)_anchorDate;
                //            _anchorWeek = _calendar.GetWeek(_anchorDay.Date);
                //            _anchorPeriod = _calendar.GetPeriod(_anchorDay.Date);
                //            break;
                //        case eProfileType.Week:
                //            _anchorWeek = (WeekProfile)_anchorDate;
                //            _anchorDay = _calendar.GetDay(_anchorWeek.Date);
                //            _anchorPeriod = _calendar.GetPeriod(_anchorWeek.Date);
                //            break;
                //        case eProfileType.Period:
                //            _anchorPeriod = (PeriodProfile)_anchorDate;
                //            _anchorDay = _calendar.GetDay(_anchorPeriod.Date);
                //            _anchorWeek = _calendar.GetWeek(_anchorPeriod.Date);
                //            break;
                //    }
                //}
                // End Track #5833

				// Begin TT#1445-MD - stodd - Calendar Date Range display is incorrect - 
                if (_anchorDateRelativeTo == eDateRangeRelativeTo.Plan
                    && _anchorDateRangeRID != Include.UndefinedCalendarDateRange)
                {
                    this.lblPlan.Text = FormatAnchorDateDisplay(_anchorDate);
                    this.lblPlan.ForeColor = System.Drawing.Color.Blue;
                }
				// End TT#1445-MD - stodd - Calendar Date Range display is incorrect - 

				if (_dateRangeRID == Include.UndefinedCalendarDateRange
					|| _dateRangeRID == 0)
				{
					PopulateFromScreenDefaults();
				}					
				else
				{
                    // Begin Track #6432 - JSmith - Changing date range changes other date ranges
                    //PopulateFromDataBase(_dateRangeRID);
                    PopulateFromDataBase(_dateRangeRID, true);
                    // End Track #6432
				}

				lblCurrentWeek.Text = _calendar.CurrentWeek.WeekInYear.ToString(CultureInfo.CurrentUICulture) + "/" + 
					_calendar.CurrentWeek.FiscalYear.ToString(CultureInfo.CurrentUICulture);

                // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
                //lblCurrentDay.Text = _calendar.PostDate.Date.ToString("MM/dd/yyyy", CultureInfo.CurrentUICulture);
                lblCurrentDay.Text = _calendar.PostDate.Date.ToString("d");
                // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
				lblDay.Text = _calendar.PostDate.Date.ToString("ddd",CultureInfo.CurrentUICulture);

                // Begin TT#1953-MD - RO Web
                if (!PopulateForm)
                {
                    return;
                }
                // End TT#1953-MD - RO Web

				this.btnOk.Select();

			}
			catch (Exception err)
			{
				MessageBox.Show(err.ToString());

			}
		}

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        //public void SetDateForPlan(int DateRangeRID)
        //{
        //    this.DateRangeRID = DateRangeRID;
        //    this.AnchorDate = base.SAB.ClientServerSession.Calendar.CurrentDate;
        //    this.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;
        //    this.AllowDynamicToStoreOpen = true;
        //}
		public void SetDateForPlan(int DateRangeRID, bool RestrictToOnlyWeeks, bool RestrictToSingleDate, bool AllowDynamic, bool AllowDynamicToPlan, bool AllowDynamicToStoreOpen)
        {
            this.DateRangeRID = DateRangeRID;
            this.AnchorDate = base.SAB.ClientServerSession.Calendar.CurrentDate;
            this.RestrictToSingleDate = RestrictToSingleDate;
            this.RestrictToOnlyWeeks = RestrictToOnlyWeeks;
            this.AllowDynamic = AllowDynamic;
            this.AllowDynamicToPlan = AllowDynamicToPlan;
            if (AllowDynamicToPlan)
            {
                this.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;
            }
            else
            {
                this.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
            }
            this.AllowDynamicToStoreOpen = AllowDynamicToStoreOpen;
        }
		// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        public string GetDateRangeTextForPlan(int DateRangeRID)
        {
             if (DateRangeRID != Include.UndefinedCalendarDateRange)
                {
                    DateRangeProfile drp = base.SAB.ApplicationServerSession.Calendar.GetDateRange(DateRangeRID, base.SAB.ClientServerSession.Calendar.CurrentDate);
                    return drp.DisplayDate;     
                }
             else
             {
                 return string.Empty; // "Default To Plan";
             }

        }
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        

        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
        private void SetUpAnchorDate(Profile aAnchorDate)
        {
            // setup anchor date if it was set
            if (aAnchorDate != null)
            {
                _anchorDate = aAnchorDate;
            }

            if (_anchorDate != null)
            {
                switch (_anchorDate.ProfileType)
                {
                    case eProfileType.Day:
                        _anchorDay = (DayProfile)_anchorDate;
                        _anchorWeek = _calendar.GetWeek(_anchorDay.Date);
                        _anchorPeriod = _calendar.GetPeriod(_anchorDay.Date);
                        break;
                    case eProfileType.Week:
                        _anchorWeek = (WeekProfile)_anchorDate;
                        _anchorDay = _calendar.GetDay(_anchorWeek.Date);
                        _anchorPeriod = _calendar.GetPeriod(_anchorWeek.Date);
                        break;
                    case eProfileType.Period:
                        _anchorPeriod = (PeriodProfile)_anchorDate;
                        _anchorDay = _calendar.GetDay(_anchorPeriod.Date);
                        _anchorWeek = _calendar.GetWeek(_anchorPeriod.Date);
                        break;
                }
            }
        }
        // End Track #5833

		private void BuildContextmenu()
		{
//			MenuItem _mnuItemDelete = new MenuItem();
//			MenuItem _mnuItemRename = new MenuItem();
		
			_mnuItemDelete.Text = "Delete Date Range";
			_mnuItemRename.Text = "Rename Date Range";
			
			if (FunctionSecurity.AllowDelete)
			{
				this.contextMenu1.MenuItems.Add(_mnuItemDelete);
			}
			if (FunctionSecurity.AllowUpdate)
			{
				contextMenu1.MenuItems.Add(_mnuItemRename);
			}

			_mnuItemRename.Click += new System.EventHandler(this._mnuItemRename_Click);
			_mnuItemDelete.Click += new System.EventHandler(this._mnuItemDelete_Click);

		}

		private void _mnuItemRename_Click(object sender, System.EventArgs e)
		{
			if (listBox1.SelectedIndex > -1)
			{
				NameDialog dateRangeNameForm = new NameDialog("Date Range Name");
				dateRangeNameForm.StartPosition = FormStartPosition.CenterScreen;
				dateRangeNameForm.TextValue = listBox1.Text;

				bool nameOk = false;
				bool cancelAction = false;
				while (!(nameOk || cancelAction))
				{
					DialogResult theResult = dateRangeNameForm.ShowDialog();
					if (theResult == DialogResult.OK)
					{
						if (dateRangeNameForm.TextValue.Length > 50)
							MessageBox.Show("Date Range Name exceeds maximum of 50 characters.  Please correct.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
						else
						{
							DataRow dr = _dvDateRangesWithNames.Table.Rows.Find(dateRangeNameForm.TextValue);
							if (dr != null)
								MessageBox.Show("A Date Range already exists with the name - " + dateRangeNameForm.TextValue,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
								// Begin Issue 3829 - stodd
							else if (this._calendar.DateRangeSelector_NameExists(dateRangeNameForm.TextValue))
								MessageBox.Show("A Date Range already exists with the name - " + dateRangeNameForm.TextValue,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
								// End Issue 3829 - stodd
							else
								nameOk = true;
						}
					}
					else
					{
						cancelAction = true;
					}
				}

				if (nameOk)
				{
					string oldName = listBox1.Text;
					_selectedDateRange.Name = dateRangeNameForm.TextValue;
					string newName = dateRangeNameForm.TextValue;

					_calendar.UpdateDateRange(Convert.ToInt32(listBox1.SelectedValue, CultureInfo.CurrentUICulture), dateRangeNameForm.TextValue);
						
					listBox1.BeginUpdate();
					DataRow row = _dvDateRangesWithNames.Table.Rows.Find(oldName);
					row["CDR_NAME"] = dateRangeNameForm.TextValue;
					listBox1.EndUpdate();
				}
			}
			else
				MessageBox.Show("Select a Date Range to Rename.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);						

		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
//			if (!FunctionSecurity.AllowUpdate)
//			{
//				contextMenu1.MenuItems[0].Enabled = false; 
//				contextMenu1.MenuItems[1].Enabled = false; 
//			}
//			else
//			{
//				contextMenu1.MenuItems[0].Enabled = true; 
//				contextMenu1.MenuItems[1].Enabled = true; 
//			}

		}

		private void _mnuItemDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				int dateRangeRid;
				if (listBox1.SelectedIndex > -1)
				{
					dateRangeRid = Convert.ToInt32(listBox1.SelectedValue, CultureInfo.CurrentUICulture);
					DialogResult questionResult = MessageBox.Show("Are you sure you want to Delete Calendar Date Range: " +
						listBox1.Text + "?","Delete Date Range", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (questionResult == DialogResult.Yes)	
					{
						try
						{
							_calendar.DeleteDateRange(dateRangeRid);
						}
						catch (Exception)
						{
							MessageBox.Show("Requested Calendar Date Range could not be deleted." +
								" It is being used elsewhere in the system.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);						
							return;
						}
						//============================================================================================
						// If the date range we deleted just happens to be the one currently selected by the window,
						// we need to update some of the selected info so a new RID and name are assigned.
						// We ALWAYS empty out the date range name in the selected date range.
						//============================================================================================
						if (this._selectedDateRange.Key == dateRangeRid)
						{
							_selectedDateRange.Key = Include.UndefinedCalendarDateRange;
							_selectedDateRange.Name = string.Empty;
						}
						if (_anchorDate != null)
							_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
								this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);
						else
							_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
								this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);
						listBox1.DataSource = _dvDateRangesWithNames;
						listBox1.SelectedIndex = -1;
					}
				}
				else
				{
					MessageBox.Show("No Date Range selected to Delete.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);						
				}
			}
			catch( Exception error)
			{
				MessageBox.Show(error.Message);
			}

			finally
			{
			
			}
		}

		private void PopulateFromScreenDefaults()
		{
			_selectedDateRange = new DateRangeProfile(Include.UndefinedCalendarDateRange);
			_selectedDateRange.InternalAnchorDate = this.AnchorDate;
			_selectedDateRange.DateRangeType = eCalendarRangeType.Dynamic;

			// defualts to Current
			_selectedDateRange.RelativeTo = this.AnchorDateRelativeTo;
			
			//&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
			//   scroll to current year  TO DO
			//&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&*&&&&

			//ultraGrid1.Text = _displayedYears[0].ToString();
			
			this.rbWeek.Checked = true;
			if (_allowDynamic)
			{
				if (_defaultToStatic)
				{
					this.rbStatic.Checked = true;
					 //BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    this.rbRelativeToCurrent.Checked = false;
                    this.rbRelativeToPlan.Checked = false;
                    this.rbRelativeToStore.Checked = false;
					 //END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    _selectedDateRange.DateRangeType = eCalendarRangeType.Static;	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
				}
				else
				{
					this.rbDynamic.Checked = true;
					if (_allowDynamicToCurrent)
					{
						this.rbRelativeToCurrent.Checked = true;
					}
					else
						if (_allowDynamicToPlan)
					{
						this.rbRelativeToPlan.Checked = true;
						 //BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                        this.rbRelativeToCurrent.Checked = false;
                        _selectedDateRange.RelativeTo = DataCommon.eDateRangeRelativeTo.Plan;
						 //END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
					}
					else
						if (_allowDynamicToStoreOpen)
					{
						this.rbRelativeToStore.Checked = true;
						 //BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                        this.rbRelativeToCurrent.Checked = false;
                        this.rbRelativeToPlan.Checked = false;
                        _selectedDateRange.RelativeTo = DataCommon.eDateRangeRelativeTo.StoreOpen;
						 //END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
					}
				}
			}
			else
			{
				this.rbStatic.Checked = true;
			}

            // Begin TT#1953-MD - RO Web
            if (!PopulateForm)
            {
                return;
            }
            // End TT#1953-MD - RO Web

			this.gbRelativeTo.Enabled = true;

			this.lblCurrentWeek.ForeColor = System.Drawing.Color.DarkGreen;

			//ScrollToYear(_calendar.CurrentDate.FiscalYear);

			DoDynamicSelectionSetup();

			listBox1_ClearSelection();

			// If the form is loaded, lets scroll to the right year.
			if (this._formLoaded)
			{
				// scroll to start year
				ScrollToYear(_calendar.CurrentDate.FiscalYear);
			}
		}

        // Begin Track #6432 - JSmith - Changing date range changes other date ranges
        //private void PopulateFromDataBase(int RID)
        private void PopulateFromDataBase(int RID, bool aSetOriginalDateRange)
        // End Track #6432
		{
			// these are used to figure out the visually selected dates;
			//WeekProfile selectedStartWeek = null;
			//WeekProfile selectedEndWeek = null;
			//int endDateKey = 0;

			_selectedDateRange = _calendar.GetDateRange(RID, false);
			_selectedDateRange.Debug("IN ");

			// BEGIN Issue 5119
			if (_selectedDateRange.IsDynamicSwitch)
			{
				// BEGIN Issue 5171
				WeekProfile startWeek;
				if (_selectedDateRange.DynamicSwitchDate == Include.UndefinedDynamicSwitchDate)
				{
					startWeek = GetSelectedStartWeek();
				}
				else
				{
					startWeek = _calendar.GetWeek(_selectedDateRange.DynamicSwitchDate);
				}
				// END Issue 5171
				if (startWeek.Key != Include.NoRID)
				{
					this.btnDynamicSwitch.Text = FormatAnchorDateDisplay(startWeek);
					btnDynamicSwitch.Tag = startWeek;
				}

				_calendar.ResolveDynamicSwitchDateForSelector(_selectedDateRange);				
			}
			// End issue 5119

			//Begin Issue 4677 - JSmith - saves invalid date range
            // Begin Track #6432 - JSmith - Changing date range changes other date ranges
            //_origDateRange = _selectedDateRange.Clone();
            if (aSetOriginalDateRange)
            {
                _origDateRange = _selectedDateRange.Clone();
            }
            // End Track #6432
			//End Issue 4677

			if (_anchorDate != null)
				_selectedDateRange.InternalAnchorDate = _anchorDate;

			// BEGIN Issue 5119
			//=================================================
			// Sets the...
			// _selectedStartWeek and the _selectedEndWeek
			//=================================================
			if (_selectedDateRange.IsDynamicSwitch)
				_changedByProgram = true;
			GetSelectedDates();
			if (_selectedDateRange.IsDynamicSwitch)
				_changedByProgram = false;
			// END Issue 5119

            // Begin TT#1953-MD - RO Web
            if (!PopulateForm)
            {
                return;
            }
            // End TT#1953-MD - RO Web

			if (_selectedDateRange.SelectedDateType == eCalendarDateType.Day)
				this.rbDay.Checked = true;
			else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Period)
				this.rbPeriod.Checked = true;
			else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Week)
				this.rbWeek.Checked = true;


			if (_selectedDateRange.Name == "" || _selectedDateRange.Name == null)
			{
				listBox1_ClearSelection();
			}
			else // they selected a "named" date range from the listbox
			{
				if (!_formLoaded || _reset)
				{	
					string name = _selectedDateRange.Name;
					int sIndex = listBox1.FindStringExact(name);
					if (sIndex != -1)
					{
						listBox1.SetSelected(sIndex,true);
					}
				}
			}
			
			// set selection
			ClearSelections();

			// BEGIN Issue 5119
			if (_selectedDateRange.IsDynamicSwitch)
				_changedByProgram = true;
			SetDateRangeSelection(_selectedStartWeek, _selectedEndWeek);
			if (_selectedDateRange.IsDynamicSwitch)
				_changedByProgram = false;

//			if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
//			{
//				if (_selectedStartWeek.Key != Include.NoRID)
//					this.btnDynamicSwitch.Text = FormatAnchorDateDisplay(_selectedStartWeek);
//			}
			// END Issue 5119

			// if the form isn't loaded yet, the scrollTo event in the grid doesn't work.
			// so...we set the start date and do the scrollTo in the form_load event.
			if (!this._formLoaded)
				_startYearAtLoad = _selectedStartWeek.FiscalYear;
			else
			{
				// scroll to start year
				//ScrollToYear(selectedStartWeek.FiscalYear);
			}

			
//			if (_selectedDateRange.IsDynamicSwitch)
//			{	
//				_selectedDateRange.DateRangeType = eCalendarRangeType.DynamicSwitch;
//			}
		}

		// BEGIN Issue 5119
		/// <summary>
		///  Using the _selectedDateRange, resolves the _selectedStartWeek & _selectedEndWeek.
		/// </summary>
		private void GetSelectedDates()
		{
			int endDateKey = 0;
			if (_selectedDateRange.SelectedDateType == eCalendarDateType.Day)
			{
				this.rbDay.Enabled = true;
				rbDay.Checked = true;
				DayProfile startDt = null;
				DayProfile endDt = null;
				if (_selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
				{
					if (_selectedDateRange.RelativeTo == eDateRangeRelativeTo.Current)
					{
						startDt = _calendar.ConvertToStaticDay(_selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticDay(_selectedDateRange.EndDateKey);
					}
					else
					{
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorDay == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                            _anchorDateWasOverridden = true;
                        }
                        // End Track #5833

						startDt = _calendar.ConvertToStaticDay(_anchorDay, _selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticDay(_anchorDay, _selectedDateRange.EndDateKey);
					}
					this.rbDynamic.Checked = true;
					DoDynamicSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Static)
				{
					startDt = _calendar.GetDay(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetDay(_selectedDateRange.EndDateKey);
					this.rbStatic.Checked = true;
					DoStaticSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
				{
					endDateKey = CheckReoccurringEndDate(); 
					startDt = _calendar.GetDay(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetDay(endDateKey);
					this.rbRecurring.Checked = true;
					DoReoccurringSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					startDt = _calendar.GetDay(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetDay(_selectedDateRange.EndDateKey);
					this.rbDynamicSwitch.Checked = true;
					DoDynamicSwitchSelectionSetup();
				}
                // Begin Track #5833 - KJohnson - Null reference when dynamic to plan selected
                if (startDt != null)
                {
                    _selectedStartWeek = _calendar.GetWeek(startDt.Date);
                }
                if (endDt != null)
                {
                    _selectedEndWeek = _calendar.GetWeek(endDt.Date);
                }
                // End Track #5833
			}
			else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Week)
			{
				rbWeek.Checked = true;
				WeekProfile startDt = null;
				WeekProfile endDt = null;
				if (_selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
				{
					if (_selectedDateRange.RelativeTo == eDateRangeRelativeTo.Current)
					{
						startDt = _calendar.ConvertToStaticWeek(_selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticWeek(_selectedDateRange.EndDateKey);
					}
					else
					{
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorWeek == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                            _anchorDateWasOverridden = true;
                        }
                        // End Track #5833

						startDt = _calendar.ConvertToStaticWeek(_anchorWeek, _selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticWeek(_anchorWeek, _selectedDateRange.EndDateKey);
					}
					this.rbDynamic.Checked = true;
					DoDynamicSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Static)
				{
					startDt = _calendar.GetWeek(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetWeek(_selectedDateRange.EndDateKey);
					this.rbStatic.Checked = true;
					DoStaticSelectionSetup();			
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
				{
					endDateKey = CheckReoccurringEndDate(); 
					startDt = _calendar.GetWeek(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetWeek(endDateKey);
					this.rbRecurring.Checked = true;
					DoReoccurringSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					startDt = _calendar.GetWeek(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetWeek(_selectedDateRange.EndDateKey);
					this.rbDynamicSwitch.Checked = true;
					DoDynamicSwitchSelectionSetup();
				}
                // Begin Track #5833 - KJohnson - Null reference when dynamic to plan selected
                if (startDt != null)
                {
                    _selectedStartWeek = startDt;
                }
                if (endDt != null)
                {
                    _selectedEndWeek = endDt;
                }
                // End Track #5833
			}
			else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Period)
			{
				rbPeriod.Checked = true;
				PeriodProfile startDt = null;
				PeriodProfile endDt = null;
				if (_selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
				{
					if (_selectedDateRange.RelativeTo == eDateRangeRelativeTo.Current)
					{
						startDt = _calendar.ConvertToStaticPeriod(_selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticPeriod(_selectedDateRange.EndDateKey);
					}
					else
					{
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorPeriod == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                            _anchorDateWasOverridden = true;
                        }
                        // End Track #5833

						startDt = _calendar.ConvertToStaticPeriod(_anchorPeriod, _selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticPeriod(_anchorPeriod, _selectedDateRange.EndDateKey);
					}
					this.rbDynamic.Checked = true;
					DoDynamicSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Static)
				{
					startDt = _calendar.GetPeriod(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetPeriod(_selectedDateRange.EndDateKey);
					this.rbStatic.Checked = true;
					DoStaticSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
				{
					endDateKey = CheckReoccurringEndDate(); 
					startDt = _calendar.GetPeriod(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetPeriod(endDateKey);
					this.rbRecurring.Checked = true;
					DoReoccurringSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					startDt = _calendar.GetPeriod(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetPeriod(_selectedDateRange.EndDateKey);
					this.rbDynamicSwitch.Checked = true;
					DoDynamicSwitchSelectionSetup();
				}
                // Begin Track #5833 - KJohnson - Null reference when dynamic to plan selected
                if (startDt != null)
                {
                    _selectedStartWeek = _calendar.GetWeek(startDt.Date);
                }
                if (endDt != null)
                {
                    _selectedEndWeek = _calendar.GetWeek(endDt.Date);
                }
                // End Track #5833
			}
		}

		/// <summary>
		/// Returns only the starting week of the range.
		/// </summary>
		/// <returns></returns>
		private WeekProfile GetSelectedStartWeek()
		{
			WeekProfile startWeek = null; 
			int endDateKey = 0;
			if (_selectedDateRange.SelectedDateType == eCalendarDateType.Day)
			{
				//this.rbDay.Enabled = true;
				//rbDay.Checked = true;
				DayProfile startDt = null;
                //DayProfile endDt = null;
				if (_selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
				{
					if (_selectedDateRange.RelativeTo == eDateRangeRelativeTo.Current)
					{
						startDt = _calendar.ConvertToStaticDay(_selectedDateRange.StartDateKey);
						//endDt = _calendar.ConvertToStaticDay(_selectedDateRange.EndDateKey);
					}
					else
					{
						startDt = _calendar.ConvertToStaticDay(_anchorDay, _selectedDateRange.StartDateKey);
						//endDt = _calendar.ConvertToStaticDay(_anchorDay, _selectedDateRange.EndDateKey);
					}
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Static)
				{
					startDt = _calendar.GetDay(_selectedDateRange.StartDateKey);
					//endDt = _calendar.GetDay(_selectedDateRange.EndDateKey);
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
				{
					endDateKey = CheckReoccurringEndDate(); 
					startDt = _calendar.GetDay(_selectedDateRange.StartDateKey);
					//endDt = _calendar.GetDay(endDateKey);
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					startDt = _calendar.GetDay(_selectedDateRange.StartDateKey);
					//endDt = _calendar.GetDay(_selectedDateRange.EndDateKey);
				}
				startWeek = _calendar.GetWeek(startDt.Date);
				//_selectedStartWeek = _calendar.GetWeek(startDt.Date);
				//_selectedEndWeek = _calendar.GetWeek(endDt.Date);
			}
			else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Week)
			{
				//rbWeek.Checked = true;
				WeekProfile startDt = null;
				WeekProfile endDt = null;
				if (_selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
				{
					if (_selectedDateRange.RelativeTo == eDateRangeRelativeTo.Current)
					{
						startDt = _calendar.ConvertToStaticWeek(_selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticWeek(_selectedDateRange.EndDateKey);
					}
					else
					{
						startDt = _calendar.ConvertToStaticWeek(_anchorWeek, _selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticWeek(_anchorWeek, _selectedDateRange.EndDateKey);
					}
					this.rbDynamic.Checked = true;
					DoDynamicSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Static)
				{
					startDt = _calendar.GetWeek(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetWeek(_selectedDateRange.EndDateKey);
					this.rbStatic.Checked = true;
					DoStaticSelectionSetup();			
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
				{
					endDateKey = CheckReoccurringEndDate(); 
					startDt = _calendar.GetWeek(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetWeek(endDateKey);
					this.rbRecurring.Checked = true;
					DoReoccurringSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					startDt = _calendar.GetWeek(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetWeek(_selectedDateRange.EndDateKey);
					this.rbDynamicSwitch.Checked = true;
					DoDynamicSwitchSelectionSetup();
				}
				startWeek = startDt;
				//_selectedStartWeek = startDt;
				//_selectedEndWeek = endDt;
			}
			else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Period)
			{
				rbPeriod.Checked = true;
				PeriodProfile startDt = null;
				PeriodProfile endDt = null;
				if (_selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
				{
					if (_selectedDateRange.RelativeTo == eDateRangeRelativeTo.Current)
					{
						startDt = _calendar.ConvertToStaticPeriod(_selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticPeriod(_selectedDateRange.EndDateKey);
					}
					else
					{
						startDt = _calendar.ConvertToStaticPeriod(_anchorPeriod, _selectedDateRange.StartDateKey);
						endDt = _calendar.ConvertToStaticPeriod(_anchorPeriod, _selectedDateRange.EndDateKey);
					}
					this.rbDynamic.Checked = true;
					DoDynamicSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Static)
				{
					startDt = _calendar.GetPeriod(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetPeriod(_selectedDateRange.EndDateKey);
					this.rbStatic.Checked = true;
					DoStaticSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
				{
					endDateKey = CheckReoccurringEndDate(); 
					startDt = _calendar.GetPeriod(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetPeriod(endDateKey);
					this.rbRecurring.Checked = true;
					DoReoccurringSelectionSetup();
				}
				else if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					startDt = _calendar.GetPeriod(_selectedDateRange.StartDateKey);
					endDt = _calendar.GetPeriod(_selectedDateRange.EndDateKey);
					this.rbDynamicSwitch.Checked = true;
					DoDynamicSwitchSelectionSetup();
				}
				startWeek = _calendar.GetWeek(startDt.Date);
				//_selectedStartWeek = _calendar.GetWeek(startDt.Date);
				//_selectedEndWeek = _calendar.GetWeek(endDt.Date);
			}
			return startWeek;
		}
		// END Issue 5119

		/// <summary>
		/// Tests to see if the Reoccurring key for the end date is less than the start date.
		/// This means we have crossed a year boundary and need to bump up the year on the
		/// end date.
		/// </summary>
		/// <returns></returns>
		private int CheckReoccurringEndDate()
		{
			int endDateKey = _selectedDateRange.EndDateKey;
			if (_selectedDateRange.EndDateKey < _selectedDateRange.StartDateKey)
			{
				int currYear = _calendar.CurrentDate.FiscalYear;
				endDateKey = ((currYear + 1) * 100) + endDateKey;
			}
			return endDateKey;
		}

		private void ApplyFormSettings()
		{
			if (_restrictToOnlyPeriods)
			{
				this.rbPeriod.Enabled = true;
				rbPeriod.Checked = true;
				this.rbDay.Enabled = false;
				this.rbWeek.Enabled = false;
			}
			if (_restrictToOnlyWeeks)
			{
				this.rbWeek.Enabled = true;
				rbWeek.Checked = true;
				this.rbDay.Enabled = false;
				this.rbPeriod.Enabled = false;
			}
			if (_allowReoccurring)
			{
				this.rbRecurring.Enabled = true;
				//this.rbRecurring.Visible = true;
				this.rbDynamicSwitch.Enabled = false;
				//this.rbDynamicSwitch.Visible = false;
			}
			else
			{
				this.rbRecurring.Enabled = false;
				//this.rbRecurring.Visible = true;
			}
			if (_allowDynamicSwitch)
			{
				this.rbDynamicSwitch.Enabled = true;
				//this.rbDynamicSwitch.Visible = true;
				//rbDynamicSwitch.Location = rbRecurring.Location;
				this.rbRecurring.Enabled = false;
				//this.rbRecurring.Visible = false;
				this.btnDynamicSwitch.ForeColor = System.Drawing.Color.Blue;
			}
			else
			{
				this.rbDynamicSwitch.Enabled = false;
				//this.rbDynamicSwitch.Visible = false;
			}
			if (_allowDynamic)
			{
			 	//BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
				// Begin TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static" - 
                //if (_defaultToStatic)
                //{
                //    this.rbStatic.Checked = true;
                //}
                //else
                {
				 //END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    this.rbDynamic.Enabled = true;

                    this.rbRelativeToCurrent.Enabled = false;
                    if (_allowDynamicToCurrent)
                    {
                        if (rbDynamic.Checked)
                        {
                            this.rbRelativeToCurrent.Enabled = true;
                        }
                    }
                    //else
                    //{
                    //    this.rbRelativeToCurrent.Enabled = false;
                    //    //this.rbRelativeToCurrent.Checked = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    //}
                    this.rbRelativeToPlan.Enabled = false;
                    if (_allowDynamicToPlan)
                    {
                        if (rbDynamic.Checked)
                        {
                            this.rbRelativeToPlan.Enabled = true;
                        }
                    }
                    //else
                    //{
                    //    this.rbRelativeToPlan.Enabled = false;
                    //    //this.rbRelativeToPlan.Checked = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    //}
                    this.rbRelativeToStore.Enabled = false;
                    if (_allowDynamicToStoreOpen)
                    {
                        if (rbDynamic.Checked)
                        {
                            this.rbRelativeToStore.Enabled = true;
                        }
                    }
                    //else
                    //{
                    //    this.rbRelativeToStore.Enabled = false;
                    //    //this.rbRelativeToStore.Checked = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    //}
				// End TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                }
			}
			else
			{
				this.rbDynamic.Enabled = false;
				this.gbRelativeTo.Enabled = false;
			}

			initializeGridShading();
		}

		private void DoDynamicSelectionSetup()
		{
			_selectedDateRange.IsDynamicSwitch = false;
			this.gbRelativeTo.Enabled = true;
			// BEGIN Issue 5232 stodd 4.25.2008
			this.rbRelativeToPlan.Enabled = true;
			this.rbRelativeToStore.Enabled = true;
			// END Issue 5232 stodd 4.25.2008
	
			this.lblCurrent.Text = FormatAnchorDateDisplay(_calendar.CurrentWeek);
			this.lblCurrent.ForeColor = System.Drawing.Color.Blue;

			// if no anchor date, can't select ToPlan or ToStore
			if (_anchorDate == null)
			{
                // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                //this.rbRelativeToPlan.Enabled = false;
                //this.rbRelativeToStore.Enabled = false;
                //this.rbRelativeToCurrent.Checked = true;
                if (!_overrideNullAnchorDateDefaults)
                {
                    this.rbRelativeToPlan.Enabled = false;
                    this.rbRelativeToStore.Enabled = false;
                    // Begin TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                    if (rbDynamic.Checked)
                    {
                        this.rbRelativeToCurrent.Enabled = true;
                        this.rbRelativeToCurrent.Checked = true;
                    }
                    // End TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                }
                else
                {
                    if (_allowDynamicToCurrent)
                    {
						// Begin TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static" 
                        if (rbDynamic.Checked)
                        {
                            this.rbRelativeToCurrent.Enabled = true;
                            //this.rbRelativeToCurrent.Checked = true;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                        }
						// End TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                    }
                    else
                    {
                        this.rbRelativeToCurrent.Enabled = false;
                    }

                    if (_allowDynamicToPlan)
                    {
						// Begin TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                        if (rbDynamic.Checked)
                        {
                            this.rbRelativeToPlan.Enabled = true;
                            //BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                            if (!this.rbRelativeToCurrent.Checked)
                            {
                                this.rbRelativeToPlan.Checked = true;
                            }
                            //END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                        }
						// End TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                    }
                    else
                    {
                        this.rbRelativeToPlan.Enabled = false;
                    }

                    if (_allowDynamicToStoreOpen)
                    {
						// Begin TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                        if (rbDynamic.Checked)
                        {
                            this.rbRelativeToStore.Enabled = true;
                            //BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                            if (!this.rbRelativeToCurrent.Checked && !this.rbRelativeToPlan.Checked)
                            {
                                this.rbRelativeToStore.Checked = true;
                            }
                            //END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                        }
						// End TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
                    }
                    else
                    {
                        this.rbRelativeToStore.Enabled = false;
                    }

                    
                }
                // End Track #5833
			}
			else
			{
				// enable the right radio buttons
                // Begin TT#1287-MD - stodd - When changing from a static calendar date to a dynamic calendar date in the date range selector "Current" is disabled. It should not be. - 
                if (rbDynamic.Checked)
                {
                    this.rbRelativeToCurrent.Enabled = true;
                }
                // End TT#1287-MD - stodd - When changing from a static calendar date to a dynamic calendar date in the date range selector "Current" is disabled. It should not be. - 

				// Begin TT#1445-MD - stodd - Calendar Date Range display is incorrect - 
                if (_selectedDateRange.DateRangeType != eCalendarRangeType.Static)
                {
                    if (this.AnchorDateRelativeTo == DataCommon.eDateRangeRelativeTo.Plan)
                    {
                        this.rbRelativeToPlan.Enabled = true;
                        this.lblPlan.Text = FormatAnchorDateDisplay(_anchorDate);
                        this.lblPlan.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (this.AnchorDateRelativeTo == DataCommon.eDateRangeRelativeTo.StoreOpen)
                    {
                        this.rbRelativeToStore.Enabled = true;
                        this.lblStoreOpen.Text = FormatAnchorDateDisplay(_anchorDate);
                        this.lblStoreOpen.ForeColor = System.Drawing.Color.Blue;
                    }

                    // PLAN
                    // Begin TT#1287-MD - stodd - When changing from a static calendar date to a dynamic calendar date in the date range selector "Current" is disabled. It should not be. - 
                    if (_selectedDateRange.RelativeTo == DataCommon.eDateRangeRelativeTo.Plan)
                    {
                        this.rbRelativeToPlan.Checked = true;
                    }
                    // STORE OPEN
                    else if (_selectedDateRange.RelativeTo == DataCommon.eDateRangeRelativeTo.StoreOpen)
                    {
                        this.rbRelativeToStore.Checked = true;
                    }
                    // CURRENT
                    else if (_selectedDateRange.RelativeTo == DataCommon.eDateRangeRelativeTo.Current)
                    {
                        this.rbRelativeToCurrent.Checked = true;
                    }
                    else
                    {
                        this.rbRelativeToCurrent.Checked = true;
                    }
                    // End TT#1287-MD - stodd - When changing from a static calendar date to a dynamic calendar date in the date range selector "Current" is disabled. It should not be. - 
                }
				// End TT#1445-MD - stodd - Calendar Date Range display is incorrect - 
			}

			// override setting if type not allowed
			if (!_allowDynamicToCurrent)
			{
				this.rbRelativeToCurrent.Enabled = false;
			}
				
			if (!_allowDynamicToPlan)
			{
				this.rbRelativeToPlan.Enabled = false;
			}

			if (!_allowDynamicToStoreOpen)
			{
				this.rbRelativeToStore.Enabled = false;
			}
			this.btnDynamicSwitch.Enabled = false;		// Issue 5171
			//BEGIN TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
			// Begin TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
            //if (!_formLoaded)
            //{
            //    if (_defaultToStatic)
            //    {
            //        this.rbRelativeToCurrent.Checked = false;
            //        this.rbRelativeToCurrent.Enabled = false;
            //        this.rbRelativeToPlan.Checked = false;
            //        this.rbRelativeToPlan.Enabled = false;
            //        this.rbRelativeToStore.Checked = false;
            //        this.rbRelativeToStore.Enabled = false;
            //    }
            //}
			// End TT#1284 - stodd - Time Period display indicates "Dynamic". Date Range selector opens as "Static"
			//END TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
		}

		private void DoStaticSelectionSetup()
		{
			_selectedDateRange.IsDynamicSwitch = false;
			this.rbRelativeToCurrent.Checked = false;
			this.rbRelativeToPlan.Checked = false;
			this.rbRelativeToStore.Checked = false;
			this.gbRelativeTo.Enabled = false;
			this.btnDynamicSwitch.Enabled = false;		// Issue 5171
		}

		private void DoReoccurringSelectionSetup()
		{
			_selectedDateRange.IsDynamicSwitch = false;
			this.rbRelativeToCurrent.Checked = false;
			this.rbRelativeToPlan.Checked = false;
			this.rbRelativeToStore.Checked = false;
			this.gbRelativeTo.Enabled = false;
			this.btnDynamicSwitch.Enabled = false;		// Issue 5171
		}

		private void DoDynamicSwitchSelectionSetup()
		{
			_selectedDateRange.IsDynamicSwitch = true;
			this.lblCurrent.Text = FormatAnchorDateDisplay(_calendar.CurrentWeek);
			this.lblCurrent.ForeColor = System.Drawing.Color.Blue;
			// BEGIN Issue 5119
			if (this._formLoaded)
			{
				if (_selectedStartWeek.Key != Include.NoRID)
				{
					this.btnDynamicSwitch.Text = FormatAnchorDateDisplay(_selectedStartWeek);
					btnDynamicSwitch.Tag = _selectedStartWeek;
					_selectedDateRange.DynamicSwitchDate = _selectedStartWeek.Key;
				}
			}
			// END Issue 5119
			this.btnDynamicSwitch.ForeColor = System.Drawing.Color.Blue;
			this.gbRelativeTo.Enabled = true;
			this.rbRelativeToCurrent.Checked = true;
			this.rbRelativeToPlan.Checked = false;
			this.rbRelativeToStore.Checked = false;
			this.rbRelativeToCurrent.Enabled = true;
			this.rbRelativeToPlan.Enabled = false;
			this.rbRelativeToStore.Enabled = false;
			this.rbWeek.Checked = true;
			this.btnDynamicSwitch.Enabled = true;	// Issue 5171
		}

		private string FormatAnchorDateDisplay(Profile anchorDate)
		{
			string dateDisplay = "";

			switch (anchorDate.ProfileType)
			{
				case eProfileType.Day:
					DayProfile anchorDay = (DayProfile)anchorDate;
					dateDisplay = anchorDay.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture) + "/" + 
						anchorDay.DayInYear.ToString("000", CultureInfo.CurrentUICulture);
					break;
				case eProfileType.Week:
					WeekProfile anchorWeek = (WeekProfile)anchorDate;
					dateDisplay = "Week " + anchorWeek.WeekInYear.ToString("00", CultureInfo.CurrentUICulture) + "/" +
						anchorWeek.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
					break;
				case eProfileType.Period:
					PeriodProfile anchorPeriod = (PeriodProfile)anchorDate;
					dateDisplay = anchorPeriod.Abbreviation + " " +
						anchorPeriod.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
					break;
			}

			return dateDisplay;
		}

		private void SetDateRangeSelection(WeekProfile startWeek, WeekProfile endWeek)
		{
			int year, period;
			int week = 0;
			int rowCount = ultraGrid1.Rows.Count;
			int colCount = 8;
			bool prevWeekselected = false;
			ultraGrid1.BeginUpdate();

			PeriodProfile startPeriod = startWeek.Period;
			PeriodProfile endPeriod = endWeek.Period;
			int yearPeriod = 0;
			UltraGridRow firstRow = null;

			for (int r=0;r<rowCount;r++)
			{
				year = Convert.ToInt32(ultraGrid1.Rows[r].Cells["year"].Value, CultureInfo.CurrentUICulture);

				if (_selectedDateRange.SelectedDateType == eCalendarDateType.Period)
				{
					if (ultraGrid1.Rows[r].Cells["period"].Value != DBNull.Value)
					{
						period = Convert.ToInt32(ultraGrid1.Rows[r].Cells["period"].Value, CultureInfo.CurrentUICulture);
						yearPeriod = Convert.ToInt32( year.ToString("0000", CultureInfo.CurrentUICulture) + period.ToString("00", CultureInfo.CurrentUICulture) );
						if (yearPeriod >= startPeriod.YearPeriod && yearPeriod <= endPeriod.YearPeriod)
						{
							SelectRow(ultraGrid1.Rows[r]);
							// catch first row selected
							if (firstRow == null)
								firstRow = ultraGrid1.Rows[r];
						}
					}
				}
				else if (_selectedDateRange.SelectedDateType == eCalendarDateType.Week)
				{
					year = Convert.ToInt32(ultraGrid1.Rows[r].Cells["year"].Value, CultureInfo.CurrentUICulture);
					prevWeekselected = false;

					for (int c=3;c<colCount;c++)
					{
						if (ultraGrid1.Rows[r].Cells[c].Value != System.DBNull.Value)
						{
							week = Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture);
							prevWeekselected = false;

							if (year == startWeek.FiscalYear && week >= startWeek.WeekInYear) 
							{
								if (year == endWeek.FiscalYear && week <= endWeek.WeekInYear)
								{
									ultraGrid1.Rows[r].Cells[c].Selected = true;
									prevWeekselected = true;

									// catch first row selected
									if (firstRow == null)
										firstRow = ultraGrid1.Rows[r];
								}
								else if (year < endWeek.FiscalYear)
								{
									ultraGrid1.Rows[r].Cells[c].Selected = true;
									prevWeekselected = true;

									// catch first row selected
									if (firstRow == null)
										firstRow = ultraGrid1.Rows[r];
								}
							}
							else if (year > startWeek.FiscalYear && year < endWeek.FiscalYear)
							{
								ultraGrid1.Rows[r].Cells[c].Selected = true;
								prevWeekselected = true;

								// catch first row selected
								if (firstRow == null)
									firstRow = ultraGrid1.Rows[r];
							}
							else if (startWeek.FiscalYear != endWeek.FiscalYear
								&& year == endWeek.FiscalYear && week <= endWeek.WeekInYear)
							{
								ultraGrid1.Rows[r].Cells[c].Selected = true;
								prevWeekselected = true;

								// catch first row selected
								if (firstRow == null)
									firstRow = ultraGrid1.Rows[r];
							}
							
						}
						else
						{
							if (_selectedDateRange.SelectedDateType == eCalendarDateType.Week && prevWeekselected)
							{
								ultraGrid1.Rows[r].Cells[c].Selected = true;
								prevWeekselected = true;

								// catch first row selected
								if (firstRow == null)
									firstRow = ultraGrid1.Rows[r];
							}
						}
					}
				}
			}

			// scroll row into view
			firstRow = firstRow.GetSibling(SiblingRow.Previous);
			ultraGrid1.DisplayLayout.RowScrollRegions[0].ScrollRowIntoView(firstRow);

			// make sure it's the top row.
			int limit = 0;
			do
			{
				limit++;
				if (limit > 14)
					break;
				if (ultraGrid1.DisplayLayout.RowScrollRegions[0].VisibleRows[0].Row == firstRow)
					break;

				ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineDown);
			} while (true);

			
			ultraGrid1.EndUpdate();
		

		}

		private void initializeGridShading()
		{
            // Begin Track #6227 - JSmith - simililar store time period not marked clearly on calendar
            Color lightGray = Color.FromArgb(237, 237, 237);
            // End Track #6227

			int rowCount = ultraGrid1.Rows.Count;
			int colCount = 8;
			for (int r=0;r<rowCount;r++)
			{
				// applies coloring to year break headers
				int year = Convert.ToInt32(ultraGrid1.Rows[r].Cells["year"].Value, CultureInfo.CurrentUICulture);
				if (ultraGrid1.Rows[r].Cells["period"].Value == DBNull.Value)
				{
					ultraGrid1.Rows[r].Cells["week1"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week1"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week2"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week2"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week3"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week3"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week4"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week4"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week5"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week5"].Appearance.ForeColorDisabled = _fColor;
				}

				for (int c=3;c<colCount;c++)
				{
					if (ultraGrid1.Rows[r].Cells[c].Value != System.DBNull.Value)
					{
						ultraGrid1.Rows[r].Cells[c].Appearance.ResetBackColor();
						ultraGrid1.Rows[r].Cells[c].Appearance.ResetForeColor();


						if ((year < _calendar.CurrentPeriod.FiscalYear) 
							|| (year == _calendar.CurrentPeriod.FiscalYear 
							&& Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture) < _calendar.CurrentWeek.WeekInYear))
                            // Begin Track #6227 - JSmith - simililar store time period not marked clearly on calendar
                            //ultraGrid1.Rows[r].Cells[c].Appearance.BackColor = System.Drawing.Color.LightGray;
                            ultraGrid1.Rows[r].Cells[c].Appearance.BackColor = lightGray;
                            // End Track #6227

						if (year == _calendar.CurrentPeriod.FiscalYear
							&& Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture) == _calendar.CurrentWeek.WeekInYear)
						{
							ultraGrid1.Rows[r].Cells[c].Appearance.BackColor = System.Drawing.Color.SeaGreen;
							ultraGrid1.Rows[r].Cells[c].Appearance.ForeColor = System.Drawing.Color.White;
                            ultraGrid1.Rows[r].Cells[c].Appearance.FontData.Bold = DefaultableBoolean.True;
                            ultraGrid1.Rows[r].Cells[c].Appearance.FontData.Italic = DefaultableBoolean.False;
                            ultraGrid1.Rows[r].Cells[c].Appearance.FontData.Underline = DefaultableBoolean.False;
                            ultraGrid1.Rows[r].Cells[c].Appearance.FontData.SizeInPoints = 7.30F;

                        }

						// set anchor date color (if present)
						if (_anchorWeek != null)
						{
							if (year == this._anchorWeek.FiscalYear
								&& Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture) == this._anchorWeek.WeekInYear)
							{
								ultraGrid1.Rows[r].Cells[c].Appearance.BackColor = System.Drawing.Color.CornflowerBlue;
								ultraGrid1.Rows[r].Cells[c].Appearance.ForeColor = System.Drawing.Color.White;
							}
						}
					}
				}
			}
		}

		private void ResetSelectedDateRangeName()
		{
			//listBox1.ClearSelected();
			_selectedDateRange.Name = "";
			if (this.DateRangeRID > 0)
				this._selectedDateRange.Key = DateRangeRID;
			else
				this._selectedDateRange.Key = 0;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				this.ultraGrid1.Click -= new System.EventHandler(this.ultraGrid1_Click);
				this.ultraGrid1.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_SelectionDrag);
				this.ultraGrid1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseDown);
				this.ultraGrid1.AfterSelectChange -= new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ultraGrid1_AfterSelectChange);
				this.ultraGrid1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseUp);
				this.ultraGrid1.BeforeSelectChange -= new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ultraGrid1_BeforeSelectChange);
				this.ultraGrid1.AfterRowActivate -= new System.EventHandler(this.ultraGrid1_AfterRowActivate);
				this.ultraGrid1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseMove);
				this.ultraGrid1.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ultraGrid1);
                //End TT#169
				this.ultraGrid1.AfterRowRegionScroll -= new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.ultraGrid1_AfterRowRegionScroll);
				this.ultraGrid1.BeforeRowRegionScroll -= new Infragistics.Win.UltraWinGrid.BeforeRowRegionScrollEventHandler(this.ultraGrid1_BeforeRowRegionScroll);
				this.ultraGrid1.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeEnterEditMode);
				this.btnReset.Click -= new System.EventHandler(this.btnReset_Click);
				this.btnOk.Click -= new System.EventHandler(this.btnOk_Click);
				this.rbPeriod.Click -= new System.EventHandler(this.rbPeriod_Click);
				this.rbWeek.Click -= new System.EventHandler(this.rbWeek_Click);
				this.rbDay.Click -= new System.EventHandler(this.rbDay_Click);
				this.rbRelativeToPlan.Click -= new System.EventHandler(this.rbRelativeToPlan_Click);
				this.rbRelativeToStore.Click -= new System.EventHandler(this.rbRelativeToStore_Click);
				this.rbRelativeToCurrent.CheckedChanged -= new System.EventHandler(this.rbRelativeToCurrent_CheckedChanged);
				this.listBox1.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
				this.listBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
				this.listBox1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.listBox1_KeyPress);
				this.listBox1.SelectedValueChanged -= new System.EventHandler(this.listBox1_SelectedValueChanged);
				this.listBox1.Leave -= new System.EventHandler(this.listBox1_Leave);
				this.listBox1.SelectedIndexChanged -= new System.EventHandler(this.listBox1_SelectedIndexChanged);
				this.rbRecurring.Click -= new System.EventHandler(this.rbRecurring_Click);
				this.rbStatic.Click -= new System.EventHandler(this.rbStatic_Click);
				this.rbDynamic.Click -= new System.EventHandler(this.rbDynamic_Click);
				this.btnSaveRange.Click -= new System.EventHandler(this.btnSaveRange_Click);
				this.btnClear.Click -= new System.EventHandler(this.btnClear_Click);
				this.contextMenu1.Popup -= new System.EventHandler(this.contextMenu1_Popup);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Load -= new System.EventHandler(this.CalendarDateSelector_Load);
				this.Activated -= new System.EventHandler(this.CalendarDateSelector_Activated);

				_mnuItemRename.Click -= new System.EventHandler(this._mnuItemRename_Click);
				_mnuItemDelete.Click -= new System.EventHandler(this._mnuItemDelete_Click);

				ultraGrid1.DataSource = null;
				ultraGrid1.SelectionStrategyFilter = null;
				listBox1.DataSource = null;
				mySelectionStrategy = null;

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

//		/// <summary>
//		/// Clean up any resources being used.
//		/// </summary>
//		public void Remove()
//		{
//			RemoveBase();
//			this.ultraGrid1.Click -= new System.EventHandler(this.ultraGrid1_Click);
//			this.ultraGrid1.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_SelectionDrag);
//			this.ultraGrid1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseDown);
//			this.ultraGrid1.AfterSelectChange -= new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ultraGrid1_AfterSelectChange);
//			this.ultraGrid1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseUp);
//			this.ultraGrid1.BeforeSelectChange -= new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ultraGrid1_BeforeSelectChange);
//			this.ultraGrid1.AfterRowActivate -= new System.EventHandler(this.ultraGrid1_AfterRowActivate);
//			this.ultraGrid1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseMove);
//			this.ultraGrid1.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
//			this.ultraGrid1.AfterRowRegionScroll -= new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.ultraGrid1_AfterRowRegionScroll);
//			this.ultraGrid1.BeforeRowRegionScroll -= new Infragistics.Win.UltraWinGrid.BeforeRowRegionScrollEventHandler(this.ultraGrid1_BeforeRowRegionScroll);
//			this.ultraGrid1.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeEnterEditMode);
//			this.btnReset.Click -= new System.EventHandler(this.btnReset_Click);
//			this.btnOk.Click -= new System.EventHandler(this.btnOk_Click);
//			this.rbPeriod.Click -= new System.EventHandler(this.rbPeriod_Click);
//			this.rbWeek.Click -= new System.EventHandler(this.rbWeek_Click);
//			this.rbDay.Click -= new System.EventHandler(this.rbDay_Click);
//			this.rbRelativeToPlan.Click -= new System.EventHandler(this.rbRelativeToPlan_Click);
//			this.rbRelativeToStore.Click -= new System.EventHandler(this.rbRelativeToStore_Click);
//			this.rbRelativeToCurrent.CheckedChanged -= new System.EventHandler(this.rbRelativeToCurrent_CheckedChanged);
//			this.listBox1.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
//			this.listBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
//			this.listBox1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.listBox1_KeyPress);
//			this.listBox1.SelectedValueChanged -= new System.EventHandler(this.listBox1_SelectedValueChanged);
//			this.listBox1.Leave -= new System.EventHandler(this.listBox1_Leave);
//			this.listBox1.SelectedIndexChanged -= new System.EventHandler(this.listBox1_SelectedIndexChanged);
//			this.rbRecurring.Click -= new System.EventHandler(this.rbRecurring_Click);
//			this.rbStatic.Click -= new System.EventHandler(this.rbStatic_Click);
//			this.rbDynamic.Click -= new System.EventHandler(this.rbDynamic_Click);
//			this.btnSaveRange.Click -= new System.EventHandler(this.btnSaveRange_Click);
//			this.btnClear.Click -= new System.EventHandler(this.btnClear_Click);
//			this.contextMenu1.Popup -= new System.EventHandler(this.contextMenu1_Popup);
//			this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
//			this.Load -= new System.EventHandler(this.CalendarDateSelector_Load);
//			this.Activated -= new System.EventHandler(this.CalendarDateSelector_Activated);
//
//
//			if (contextMenu1 != null)
//			{
//				contextMenu1.Dispose();
//			}
//
//			ultraGrid1.DataSource = null;
//			ultraGrid1.SelectionStrategyFilter = null;
//			listBox1.DataSource = null;
//			mySelectionStrategy = null;
//		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblCurrSalesWeek = new System.Windows.Forms.Label();
            this.lblDailySalesThru = new System.Windows.Forms.Label();
            this.lblCurrentDay = new System.Windows.Forms.Label();
            this.lblCurrentWeek = new System.Windows.Forms.Label();
            this.rbPeriod = new System.Windows.Forms.RadioButton();
            this.rbWeek = new System.Windows.Forms.RadioButton();
            this.rbDay = new System.Windows.Forms.RadioButton();
            this.gbSelectBy = new System.Windows.Forms.GroupBox();
            this.gbRelativeTo = new System.Windows.Forms.GroupBox();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.rbRelativeToPlan = new System.Windows.Forms.RadioButton();
            this.rbRelativeToStore = new System.Windows.Forms.RadioButton();
            this.rbRelativeToCurrent = new System.Windows.Forms.RadioButton();
            this.lblPlan = new System.Windows.Forms.Label();
            this.lblStoreOpen = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDynamicSwitch = new System.Windows.Forms.Button();
            this.rbDynamicSwitch = new System.Windows.Forms.RadioButton();
            this.rbRecurring = new System.Windows.Forms.RadioButton();
            this.rbStatic = new System.Windows.Forms.RadioButton();
            this.rbDynamic = new System.Windows.Forms.RadioButton();
            this.lblDynamicSwitch = new System.Windows.Forms.Label();
            this.btnSaveRange = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblDay = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            this.gbSelectBy.SuspendLayout();
            this.gbRelativeTo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // ultraGrid1
            // 
            this.ultraGrid1.AllowDrop = true;
            this.ultraGrid1.Location = new System.Drawing.Point(16, 0);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.Size = new System.Drawing.Size(194, 272);
            this.ultraGrid1.TabIndex = 1;
            this.ultraGrid1.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ultraGrid1_SelectionDrag);
            this.ultraGrid1.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeEnterEditMode);
            this.ultraGrid1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseDown);
            this.ultraGrid1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseUp);
            this.ultraGrid1.BeforeRowRegionScroll += new Infragistics.Win.UltraWinGrid.BeforeRowRegionScrollEventHandler(this.ultraGrid1_BeforeRowRegionScroll);
            this.ultraGrid1.Click += new System.EventHandler(this.ultraGrid1_Click);
            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
            this.ultraGrid1.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ultraGrid1_AfterSelectChange);
            this.ultraGrid1.MouseLeaveElement += new Infragistics.Win.UIElementEventHandler(this.ultraGrid1_MouseLeaveElement);
            this.ultraGrid1.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.ultraGrid1_BeforeSelectChange);
            this.ultraGrid1.AfterRowActivate += new System.EventHandler(this.ultraGrid1_AfterRowActivate);
            this.ultraGrid1.DoubleClick += new System.EventHandler(this.ultraGrid1_DoubleClick);
            this.ultraGrid1.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ultraGrid1_MouseEnterElement);
            this.ultraGrid1.AfterRowRegionScroll += new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.ultraGrid1_AfterRowRegionScroll);
            this.ultraGrid1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseMove);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(90, 508);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(58, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "&Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(222, 508);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(58, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblCurrSalesWeek
            // 
            this.lblCurrSalesWeek.Location = new System.Drawing.Point(13, 282);
            this.lblCurrSalesWeek.Name = "lblCurrSalesWeek";
            this.lblCurrSalesWeek.Size = new System.Drawing.Size(108, 19);
            this.lblCurrSalesWeek.TabIndex = 10;
            this.lblCurrSalesWeek.Text = "Current Sales Week:";
            // 
            // lblDailySalesThru
            // 
            this.lblDailySalesThru.Location = new System.Drawing.Point(176, 282);
            this.lblDailySalesThru.Name = "lblDailySalesThru";
            this.lblDailySalesThru.Size = new System.Drawing.Size(91, 16);
            this.lblDailySalesThru.TabIndex = 11;
            this.lblDailySalesThru.Text = "Daily Sales Thru:";
            // 
            // lblCurrentDay
            // 
            this.lblCurrentDay.Location = new System.Drawing.Point(289, 282);
            this.lblCurrentDay.Name = "lblCurrentDay";
            this.lblCurrentDay.Size = new System.Drawing.Size(67, 19);
            this.lblCurrentDay.TabIndex = 12;
            this.lblCurrentDay.Text = "05/19/2003";
            // 
            // lblCurrentWeek
            // 
            this.lblCurrentWeek.Location = new System.Drawing.Point(120, 282);
            this.lblCurrentWeek.Name = "lblCurrentWeek";
            this.lblCurrentWeek.Size = new System.Drawing.Size(50, 19);
            this.lblCurrentWeek.TabIndex = 13;
            this.lblCurrentWeek.Text = "22/2003";
            // 
            // rbPeriod
            // 
            this.rbPeriod.Location = new System.Drawing.Point(11, 16);
            this.rbPeriod.Name = "rbPeriod";
            this.rbPeriod.Size = new System.Drawing.Size(59, 24);
            this.rbPeriod.TabIndex = 14;
            this.rbPeriod.Text = "Period";
            this.rbPeriod.Click += new System.EventHandler(this.rbPeriod_Click);
            // 
            // rbWeek
            // 
            this.rbWeek.Location = new System.Drawing.Point(142, 16);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(57, 24);
            this.rbWeek.TabIndex = 15;
            this.rbWeek.Text = "Week";
            this.rbWeek.Click += new System.EventHandler(this.rbWeek_Click);
            this.rbWeek.CheckedChanged += new System.EventHandler(this.rbWeek_CheckedChanged);
            // 
            // rbDay
            // 
            this.rbDay.Location = new System.Drawing.Point(258, 16);
            this.rbDay.Name = "rbDay";
            this.rbDay.Size = new System.Drawing.Size(45, 24);
            this.rbDay.TabIndex = 16;
            this.rbDay.Text = "Day";
            this.rbDay.Click += new System.EventHandler(this.rbDay_Click);
            // 
            // gbSelectBy
            // 
            this.gbSelectBy.Controls.Add(this.rbPeriod);
            this.gbSelectBy.Controls.Add(this.rbWeek);
            this.gbSelectBy.Controls.Add(this.rbDay);
            this.gbSelectBy.Location = new System.Drawing.Point(8, 307);
            this.gbSelectBy.Name = "gbSelectBy";
            this.gbSelectBy.Size = new System.Drawing.Size(351, 51);
            this.gbSelectBy.TabIndex = 17;
            this.gbSelectBy.TabStop = false;
            this.gbSelectBy.Text = "Select By";
            // 
            // gbRelativeTo
            // 
            this.gbRelativeTo.Controls.Add(this.lblCurrent);
            this.gbRelativeTo.Controls.Add(this.rbRelativeToPlan);
            this.gbRelativeTo.Controls.Add(this.rbRelativeToStore);
            this.gbRelativeTo.Controls.Add(this.rbRelativeToCurrent);
            this.gbRelativeTo.Controls.Add(this.lblPlan);
            this.gbRelativeTo.Controls.Add(this.lblStoreOpen);
            this.gbRelativeTo.Location = new System.Drawing.Point(8, 435);
            this.gbRelativeTo.Name = "gbRelativeTo";
            this.gbRelativeTo.Size = new System.Drawing.Size(351, 61);
            this.gbRelativeTo.TabIndex = 20;
            this.gbRelativeTo.TabStop = false;
            this.gbRelativeTo.Text = "Dynamic Date Relative To";
            // 
            // lblCurrent
            // 
            this.lblCurrent.Location = new System.Drawing.Point(29, 39);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(89, 13);
            this.lblCurrent.TabIndex = 26;
            // 
            // rbRelativeToPlan
            // 
            this.rbRelativeToPlan.Location = new System.Drawing.Point(142, 17);
            this.rbRelativeToPlan.Name = "rbRelativeToPlan";
            this.rbRelativeToPlan.Size = new System.Drawing.Size(45, 22);
            this.rbRelativeToPlan.TabIndex = 22;
            this.rbRelativeToPlan.Text = "Plan";
            this.rbRelativeToPlan.Click += new System.EventHandler(this.rbRelativeToPlan_Click);
            // 
            // rbRelativeToStore
            // 
            this.rbRelativeToStore.Location = new System.Drawing.Point(258, 17);
            this.rbRelativeToStore.Name = "rbRelativeToStore";
            this.rbRelativeToStore.Size = new System.Drawing.Size(80, 24);
            this.rbRelativeToStore.TabIndex = 23;
            this.rbRelativeToStore.Text = "Store Open";
            this.rbRelativeToStore.Click += new System.EventHandler(this.rbRelativeToStore_Click);
            // 
            // rbRelativeToCurrent
            // 
            this.rbRelativeToCurrent.Location = new System.Drawing.Point(11, 17);
            this.rbRelativeToCurrent.Name = "rbRelativeToCurrent";
            this.rbRelativeToCurrent.Size = new System.Drawing.Size(61, 22);
            this.rbRelativeToCurrent.TabIndex = 21;
            this.rbRelativeToCurrent.Text = "Current";
            this.rbRelativeToCurrent.CheckedChanged += new System.EventHandler(this.rbRelativeToCurrent_CheckedChanged);
            // 
            // lblPlan
            // 
            this.lblPlan.Location = new System.Drawing.Point(145, 39);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(89, 13);
            this.lblPlan.TabIndex = 24;
            // 
            // lblStoreOpen
            // 
            this.lblStoreOpen.Location = new System.Drawing.Point(252, 39);
            this.lblStoreOpen.Name = "lblStoreOpen";
            this.lblStoreOpen.Size = new System.Drawing.Size(93, 13);
            this.lblStoreOpen.TabIndex = 25;
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(216, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(143, 251);
            this.listBox1.TabIndex = 21;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.Leave += new System.EventHandler(this.listBox1_Leave);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            this.listBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listBox1_KeyPress);
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(214, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "Pre-Defined Date Ranges";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDynamicSwitch);
            this.groupBox1.Controls.Add(this.rbDynamicSwitch);
            this.groupBox1.Controls.Add(this.rbRecurring);
            this.groupBox1.Controls.Add(this.rbStatic);
            this.groupBox1.Controls.Add(this.rbDynamic);
            this.groupBox1.Location = new System.Drawing.Point(8, 364);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 61);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date Type";
            // 
            // btnDynamicSwitch
            // 
            this.btnDynamicSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDynamicSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDynamicSwitch.ForeColor = System.Drawing.Color.Blue;
            this.btnDynamicSwitch.Location = new System.Drawing.Point(170, 38);
            this.btnDynamicSwitch.Name = "btnDynamicSwitch";
            this.btnDynamicSwitch.Size = new System.Drawing.Size(80, 19);
            this.btnDynamicSwitch.TabIndex = 4;
            this.btnDynamicSwitch.Text = "Week 20/2008";
            this.btnDynamicSwitch.Click += new System.EventHandler(this.btnDynamicSwitch_Click);
            // 
            // rbDynamicSwitch
            // 
            this.rbDynamicSwitch.Location = new System.Drawing.Point(154, 17);
            this.rbDynamicSwitch.Name = "rbDynamicSwitch";
            this.rbDynamicSwitch.Size = new System.Drawing.Size(103, 24);
            this.rbDynamicSwitch.TabIndex = 3;
            this.rbDynamicSwitch.Text = "Dynamic Switch";
            this.rbDynamicSwitch.Click += new System.EventHandler(this.rbDynamicSwitch_Click);
            // 
            // rbRecurring
            // 
            this.rbRecurring.Location = new System.Drawing.Point(258, 17);
            this.rbRecurring.Name = "rbRecurring";
            this.rbRecurring.Size = new System.Drawing.Size(71, 24);
            this.rbRecurring.TabIndex = 2;
            this.rbRecurring.Text = "Recurring";
            this.rbRecurring.Click += new System.EventHandler(this.rbRecurring_Click);
            // 
            // rbStatic
            // 
            this.rbStatic.Location = new System.Drawing.Point(89, 17);
            this.rbStatic.Name = "rbStatic";
            this.rbStatic.Size = new System.Drawing.Size(51, 24);
            this.rbStatic.TabIndex = 1;
            this.rbStatic.Text = "Static";
            this.rbStatic.Click += new System.EventHandler(this.rbStatic_Click);
            // 
            // rbDynamic
            // 
            this.rbDynamic.Location = new System.Drawing.Point(11, 17);
            this.rbDynamic.Name = "rbDynamic";
            this.rbDynamic.Size = new System.Drawing.Size(66, 24);
            this.rbDynamic.TabIndex = 0;
            this.rbDynamic.Text = "Dynamic";
            this.rbDynamic.Click += new System.EventHandler(this.rbDynamic_Click);
            // 
            // lblDynamicSwitch
            // 
            this.lblDynamicSwitch.Location = new System.Drawing.Point(0, 0);
            this.lblDynamicSwitch.Name = "lblDynamicSwitch";
            this.lblDynamicSwitch.Size = new System.Drawing.Size(100, 23);
            this.lblDynamicSwitch.TabIndex = 0;
            // 
            // btnSaveRange
            // 
            this.btnSaveRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveRange.Location = new System.Drawing.Point(8, 508);
            this.btnSaveRange.Name = "btnSaveRange";
            this.btnSaveRange.Size = new System.Drawing.Size(74, 23);
            this.btnSaveRange.TabIndex = 27;
            this.btnSaveRange.Text = "&Save Range";
            this.btnSaveRange.Click += new System.EventHandler(this.btnSaveRange_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(156, 508);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(58, 23);
            this.btnClear.TabIndex = 28;
            this.btnClear.Text = "C&lear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(288, 508);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 23);
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(257, 282);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(27, 19);
            this.lblDay.TabIndex = 32;
            this.lblDay.Text = "Mon";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CalendarDateSelector
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(371, 542);
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSaveRange);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lblCurrentWeek);
            this.Controls.Add(this.lblCurrentDay);
            this.Controls.Add(this.lblDailySalesThru);
            this.Controls.Add(this.lblCurrSalesWeek);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.ultraGrid1);
            this.Controls.Add(this.gbSelectBy);
            this.Controls.Add(this.gbRelativeTo);
            this.MaximizeBox = false;
            this.Name = "CalendarDateSelector";
            this.Text = "Select Date Range";
            this.Load += new System.EventHandler(this.CalendarDateSelector_Load);
            this.Activated += new System.EventHandler(this.CalendarDateSelector_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalendarDateSelector_KeyDown);
            this.Controls.SetChildIndex(this.gbRelativeTo, 0);
            this.Controls.SetChildIndex(this.gbSelectBy, 0);
            this.Controls.SetChildIndex(this.ultraGrid1, 0);
            this.Controls.SetChildIndex(this.btnReset, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.lblCurrSalesWeek, 0);
            this.Controls.SetChildIndex(this.lblDailySalesThru, 0);
            this.Controls.SetChildIndex(this.lblCurrentDay, 0);
            this.Controls.SetChildIndex(this.lblCurrentWeek, 0);
            this.Controls.SetChildIndex(this.listBox1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.btnSaveRange, 0);
            this.Controls.SetChildIndex(this.btnClear, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblDay, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            this.gbSelectBy.ResumeLayout(false);
            this.gbRelativeTo.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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

			_bColor = this.BackColor;
			_fColor = this.ForeColor;

			ultraGrid1.DisplayLayout.Override.SelectTypeRow = SelectType.Extended;
			ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
			ultraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
			//ultraGrid1.DisplayLayout.ScrollStyle = ScrollStyle.Immediate; 

			this.ultraGrid1.DisplayLayout.Bands[0].AutoPreviewEnabled = true; 

			ultraGrid1.DisplayLayout.Bands[0].ColHeadersVisible = false;
			ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
			ultraGrid1.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
			ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Year"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Period"].Hidden = true;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].Width = 75;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].Width = 20;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellAppearance.FontData.SizeInPoints = 7;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellActivation = Activation.Disabled;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellActivation = Activation.ActivateOnly;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.BackColor = _bColor;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.ForeColorDisabled = _fColor;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

			//ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.ResetForeColor();
			//ultraGrid1.DisplayLayout.Bands[0].Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Raised;

			// BorderStyleCell applies to cells in the cards.
			//ultraGrid1.DisplayLayout.Bands[0].Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.RaisedSoft;

			// BorderStyleHeader applies to column captions in the card.
			//ultraGrid1.DisplayLayout.Bands[0].Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Raised;

			ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;

            // Begin TT#668-MD - JSmith - Windows 8 - Installer issues
            foreach (UltraGridBand band in e.Layout.Bands)
            {
                foreach (UltraGridColumn col in band.Columns)
                {
                    if (!col.Hidden)
                    {
                        col.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                    }
                }
            }
            // End TT#668-MD - JSmith - Windows 8 - Installer issues
		}
		private bool DetermineSelectedDates(ref int startDate, ref int endDate)
		{
			bool successful = true;

			GetSelectedCells();
			
			foreach (UltraGridCell aCell in _selectedDates)
			{
				if ((int)aCell.Tag > endDate)
					endDate = (int)aCell.Tag;

				if ((int)aCell.Tag < startDate)
					startDate = (int)aCell.Tag;
			}

			if (this.RestrictToSingleDate && startDate != endDate)
			{
				MessageBox.Show("Cannot select a range of dates. \nDate selection has been restricted to a single date." +
					"\n\n Please select a single time period.");

				ClearSelections();
				successful = false;
			}

			return successful;
		}

		private int StripYear(int date)
		{
			string dateString = date.ToString("000000", CultureInfo.CurrentUICulture);
			string dateSubString = dateString.Substring(4,2);
			int dateLessYear = Convert.ToInt32(dateSubString, CultureInfo.CurrentUICulture);
			return dateLessYear;
		}


		/// <summary>
		/// This expects the startDate and endDate to be in FISCAL format.
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="aCalendarRangeType"></param>
		/// <returns></returns>
		private DateRangeProfile BuildDateRange(int startDate, int endDate, eCalendarRangeType aCalendarRangeType)
		{
			int oldStartDate = startDate;
			int oldEndDate = endDate;
			if (this.rbPeriod.Checked)
			{
				//COnvert startDate and endDate to thier key values
				startDate = _calendar.GetPeriodKey(startDate);
				endDate = _calendar.GetPeriodKey(endDate);
		
				_selectedDateRange.SelectedDateType = DataCommon.eCalendarDateType.Period;
				if (aCalendarRangeType == eCalendarRangeType.Dynamic)
				{
					if (this.rbRelativeToCurrent.Checked)
					{
						_selectedDateRange.StartDateKey = _calendar.ConvertToDynamicPeriod(startDate);
						_selectedDateRange.EndDateKey =_calendar.ConvertToDynamicPeriod(endDate);
					}
					else
					{
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorPeriod == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                            _anchorDateWasOverridden = true;
                        }
                        // End Track #5833

						_selectedDateRange.InternalAnchorDate = _anchorPeriod;
						_selectedDateRange.StartDateKey = _calendar.ConvertToDynamicPeriod(_anchorPeriod, startDate);
						_selectedDateRange.EndDateKey =_calendar.ConvertToDynamicPeriod(_anchorPeriod, endDate);
					}
				}
				else if (aCalendarRangeType == eCalendarRangeType.Static)
				{
					_selectedDateRange.StartDateKey = startDate;
					_selectedDateRange.EndDateKey = endDate;
				}
				else if (aCalendarRangeType == eCalendarRangeType.Reoccurring)
				{
					_selectedDateRange.StartDateKey = StripYear(oldStartDate);
					_selectedDateRange.EndDateKey = StripYear(oldEndDate);
				}
				else if (aCalendarRangeType == eCalendarRangeType.DynamicSwitch)
				{
					_selectedDateRange.StartDateKey = startDate;
					_selectedDateRange.EndDateKey = endDate;
					_selectedDateRange.IsDynamicSwitch = true;
					if (btnDynamicSwitch.Tag == null)
					{
						_selectedDateRange.DynamicSwitchDate = startDate;
					}
					else
					{
						WeekProfile wp = (WeekProfile)btnDynamicSwitch.Tag;
						_selectedDateRange.DynamicSwitchDate = wp.Key;
					}
				}
			}
			else if (this.rbWeek.Checked)
			{
				//COnvert startDate and endDate to thier key values
				startDate = _calendar.GetWeekKey(startDate);
				endDate = _calendar.GetWeekKey(endDate);

				_selectedDateRange.SelectedDateType = DataCommon.eCalendarDateType.Week;
				
				if (aCalendarRangeType == eCalendarRangeType.Dynamic)
				{
					if (this.rbRelativeToCurrent.Checked)
					{
						_selectedDateRange.StartDateKey = _calendar.ConvertToDynamicWeek(startDate);
						_selectedDateRange.EndDateKey =_calendar.ConvertToDynamicWeek(endDate);
					}
					else
					{
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorWeek == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                            _anchorDateWasOverridden = true;
                        }
                        // End Track #5833

						_selectedDateRange.InternalAnchorDate = _anchorWeek;
						_selectedDateRange.StartDateKey = _calendar.ConvertToDynamicWeek(_anchorWeek, startDate);
						_selectedDateRange.EndDateKey =_calendar.ConvertToDynamicWeek(_anchorWeek, endDate);
					}
				}
				else if (aCalendarRangeType == eCalendarRangeType.Static)
				{
					_selectedDateRange.StartDateKey = startDate;
					_selectedDateRange.EndDateKey = endDate;
				}
				else if (aCalendarRangeType == eCalendarRangeType.Reoccurring)
				{
					_selectedDateRange.StartDateKey = StripYear(oldStartDate);
					_selectedDateRange.EndDateKey = StripYear(oldEndDate);
				}
				else if (aCalendarRangeType == eCalendarRangeType.DynamicSwitch)
				{
					_selectedDateRange.StartDateKey = startDate;
					_selectedDateRange.EndDateKey = endDate;
					_selectedDateRange.IsDynamicSwitch = true;
					if (btnDynamicSwitch.Tag == null)
					{
						_selectedDateRange.DynamicSwitchDate = startDate;
					}
					else
					{
						WeekProfile wp = (WeekProfile)btnDynamicSwitch.Tag;
						_selectedDateRange.DynamicSwitchDate = wp.Key;
					}
				}
			}
			else if (this.rbDay.Checked)
			{
				_selectedDateRange.SelectedDateType = DataCommon.eCalendarDateType.Day;
				if (aCalendarRangeType == eCalendarRangeType.Dynamic)
				{
					if (this.rbRelativeToCurrent.Checked)
					{
						_selectedDateRange.StartDateKey = _calendar.ConvertToDynamicDay(startDate);
						_selectedDateRange.EndDateKey =_calendar.ConvertToDynamicDay(endDate);
					}
					else
					{
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorDay == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                            _anchorDateWasOverridden = true;
                        }
                        // End Track #5833

						_selectedDateRange.InternalAnchorDate = _anchorDay;
						_selectedDateRange.StartDateKey = _calendar.ConvertToDynamicDay(_anchorDay, startDate);
						_selectedDateRange.EndDateKey =_calendar.ConvertToDynamicDay(_anchorDay, endDate);
					}
				}
				else if (aCalendarRangeType == eCalendarRangeType.Static)
				{
					_selectedDateRange.StartDateKey = startDate;
					_selectedDateRange.EndDateKey = endDate;
				}
				else if (aCalendarRangeType == eCalendarRangeType.Reoccurring)
				{
					_selectedDateRange.StartDateKey = StripYear(oldStartDate);
					_selectedDateRange.EndDateKey = StripYear(oldEndDate);
				}
				else if (aCalendarRangeType == eCalendarRangeType.DynamicSwitch)
				{
					_selectedDateRange.StartDateKey = startDate;
					_selectedDateRange.EndDateKey = endDate;
					_selectedDateRange.IsDynamicSwitch = true;
					if (btnDynamicSwitch.Tag == null)
					{
						_selectedDateRange.DynamicSwitchDate = startDate;
					}
					else
					{
						WeekProfile wp = (WeekProfile)btnDynamicSwitch.Tag;
						_selectedDateRange.DynamicSwitchDate = wp.Key;
					}
				}
			}

			_selectedDateRange.DateRangeType = aCalendarRangeType;

			if (aCalendarRangeType == eCalendarRangeType.Dynamic)
			{
				if (this.rbRelativeToPlan.Checked)
					_selectedDateRange.RelativeTo = eDateRangeRelativeTo.Plan;
				else if (this.rbRelativeToCurrent.Checked)
					_selectedDateRange.RelativeTo = eDateRangeRelativeTo.Current;
				else if (this.rbRelativeToStore.Checked)
					_selectedDateRange.RelativeTo = eDateRangeRelativeTo.StoreOpen;
			}

			return _selectedDateRange;
		}

		private void GetSelectedCells()
		{
			// Begin Issue 4566 Stodd 08.09.07 - extra cells getting added to _selectedDates. 
			_selectedDates.Clear();
			// End Issue 

			if (this.rbWeek.Checked)
			{
				foreach (UltraGridCell aCell in ultraGrid1.Selected.Cells)
				{
					if (aCell.Text != "" && aCell.Column.Index > 2)
					{
						string ww = aCell.Text.PadLeft(2,'0');
						string yyyyww = aCell.Row.Cells["year"].Value.ToString() + ww;
						aCell.Tag = Convert.ToInt32(yyyyww, CultureInfo.CurrentUICulture);

						_selectedDates.Add(aCell);
					}
				}
			}
			else if (rbPeriod.Checked)
			{
				UltraGridRow currRow = null;
				foreach (UltraGridCell aCell in ultraGrid1.Selected.Cells)
				{
					if (aCell.Text != "" && aCell.Column.Index > 2)
					{
						UltraGridRow aRow = aCell.Row;
						if (aRow == currRow)
							continue;
						currRow = aRow;
						string per = aRow.Cells["Period"].Text;
						string yyyypp = aCell.Row.Cells["year"].Value.ToString() + per.PadLeft(2,'0');
						aCell.Tag = Convert.ToInt32(yyyypp, CultureInfo.CurrentUICulture);

						_selectedDates.Add(aCell);
					}
				}
			}
			else if (rbDay.Checked)
			{
		
			}
		}

		private void SaveDateRange()
		{
			if (_selectedDateRange.Key == Include.UndefinedCalendarDateRange)
			{
				_calendar.AddDateRange(_selectedDateRange);
				this._dateRangeRID = _selectedDateRange.Key;
			}
			else
			{
				//Begin Issue 4677 - JSmith - saves invalid date range
//				_calendar.UpdateDateRange(_selectedDateRange);
				// if not named and something changed, create new date range
				if (_origDateRange != null
					&& (_origDateRange.Name == null || _origDateRange.Name.Trim().Length == 0)
					&& _origDateRange != _selectedDateRange)
				{
					_calendar.AddDateRange(_selectedDateRange);
					this._dateRangeRID = _selectedDateRange.Key;
				}
				else
				{
					_calendar.UpdateDateRange(_selectedDateRange);
				}
				//End Track #4677
				_selectedDateRange.Debug("OUT");
			}
		}

		private void SelectRow(UltraGridCell aCell)
		{
			UltraGridRow aRow = aCell.Row;

			//ultraGrid1.BeginUpdate();
			ultraGrid1.Selected.Cells.Clear();

			aRow.Cells[3].Selected = true;
			aRow.Cells[4].Selected = true;
			aRow.Cells[5].Selected = true;
			aRow.Cells[6].Selected = true;
			aRow.Cells[7].Selected = true;
			//ultraGrid1.EndUpdate();
		}

		private void SelectRow(UltraGridRow aRow)
		{
			aRow.Cells[3].Selected = true;
			aRow.Cells[4].Selected = true;
			aRow.Cells[5].Selected = true;
			aRow.Cells[6].Selected = true;
			aRow.Cells[7].Selected = true;
		}

		private void ScrollToYear(int year)
		{
			for (int i=0;i<ultraGrid1.Rows.Count;i++)
			{
				UltraGridRow row = ultraGrid1.Rows[i];
				if (Convert.ToInt32(row.Cells["year"].Value, CultureInfo.CurrentUICulture) == year
					  && row.Cells["week1"].Value == DBNull.Value)
				{
					// Begin Issue 3904 - stodd
					//ultraGrid1.ActiveRowScrollRegion.ScrollRowIntoView(row);
					ultraGrid1.ActiveRowScrollRegion.FirstRow = row;
					// End Issue 3904 - stodd

					ultraGrid1.ActiveRow = row;
					break;
				}

			}
		}

//		private void SelectToTop(Infragistics.Win.UltraWinGrid.UltraGrid aGrid, UltraGridCell aCell)
//		{
//			UltraGridRow aRow = aCell.Row;
//			UltraGridRow siblingRow = aRow.GetSibling(SiblingRow.Previous);
//			
//			aGrid.BeginUpdate();
//			aGrid.Selected.Cells.Clear();
//
//			while ( null != siblingRow )
//			{         
//				SelectRow(siblingRow);
//				siblingRow = siblingRow.GetSibling( SiblingRow.Previous );
//			}
//
//			aGrid.EndUpdate();
//		}
//
//		private void SelectToBottom(Infragistics.Win.UltraWinGrid.UltraGrid aGrid, UltraGridCell aCell)
//		{
//			UltraGridRow aRow = aCell.Row;
//			UltraGridRow siblingRow = aRow.GetSibling(SiblingRow.Next);
//			
//			aGrid.BeginUpdate();
//			aGrid.Selected.Cells.Clear();
//
//			while ( null != siblingRow )
//			{         
//				SelectRow(siblingRow);
//				siblingRow = siblingRow.GetSibling( SiblingRow.Next );
//			}
//
//			aGrid.EndUpdate();
//		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			_reset = true;
			ClearSelections();
			listBox1_ClearSelection();
			if (this.DateRangeRID > 0 && DateRangeRID != Include.UndefinedCalendarDateRange)
                // Begin Track #6432 - JSmith - Changing date range changes other date ranges
                //PopulateFromDataBase(DateRangeRID);
                PopulateFromDataBase(DateRangeRID, true);
                // End Track #6432
			else
				PopulateFromScreenDefaults();
			_reset = false;
			ChangePending = false;  // Issue 5119
		}

		private void ClearSelections()
		{
			_selectedDates.Clear();
			_grid1FirstCell = null;
		}

		private void rbDynamic_Click(object sender, System.EventArgs e)
		{
			listBox1_ClearSelection();
			//ResetSelectedDateRangeName();
			DoDynamicSelectionSetup();
			ChangePending = true;  // Issue 5119
		}

		private void rbStatic_Click(object sender, System.EventArgs e)
		{
			listBox1_ClearSelection();
			//ResetSelectedDateRangeName();
			DoStaticSelectionSetup();
			ChangePending = true;  // Issue 5119
		}

		private void rbRecurring_Click(object sender, System.EventArgs e)
		{
			listBox1_ClearSelection();
			//ResetSelectedDateRangeName();
			DoStaticSelectionSetup();
			ChangePending = true;  // Issue 5119
		}

		private void rbDynamicSwitch_Click(object sender, System.EventArgs e)
		{
			listBox1_ClearSelection();
			DoDynamicSwitchSelectionSetup();
			ChangePending = true;  // Issue 5119
		}

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            SaveChanges();
        }

        override protected bool SaveChanges()
        {
            // Begin TT#207 - MID Track #6451 - RMatelic - Clear button not working correctly 
            if (_clearDateRange)
            {
                _selectedDateRange = new DateRangeProfile(Include.UndefinedCalendarDateRange);
                this.Tag = _selectedDateRange;
                _clearDateRange = false;
                return true;

            }
            // End TT#207  

			// BEGIN Issue 5119
			if (ChangePending)
			{
				int startDate = 9999999, endDate = 0;
				if (DetermineSelectedDates(ref startDate, ref endDate))
				{
                    if (startDate < 9999999 && endDate > 0)
                    {
                        if (this.rbStatic.Checked)
                        {
                            _selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.Static);
                        }
                        else if (this.rbDynamic.Checked)
                        {
                            _selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.Dynamic);
                        }
                        else if (this.rbRecurring.Checked)
                        {
                            _selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.Reoccurring);
                        }
                        else if (this.rbDynamicSwitch.Checked)
                        {
                            _selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.DynamicSwitch);
                        }
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        //_calendar.GetDisplayDate(_selectedDateRange);
                        _calendar.GetDisplayDate(_selectedDateRange, _anchorDateWasOverridden);
                        // End Track #5833
                        //========================================================================
                        //We only need to add those that do not have a name associated with them.
                        // SaveDateRange -> updates/adds to DB
                        //========================================================================
                        if (_selectedDateRange.Name == "" || _selectedDateRange.Name == null)
                            SaveDateRange();

                        //=========================================================================
                        // If this is a dynamic switch date, we want to return the date range
                        // (either static or dynamic) determined by the dynamic switch logic.
                        //=========================================================================
                        if (_selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
                        {
                            _selectedDateRange = _calendar.GetDateRange(_selectedDateRange.Key, true);
                        }

                        this.Tag = _selectedDateRange;
                        DialogResult = DialogResult.OK;

                        ChangePending = false;
                    }
                //TT#691 Begin - MD - Validation issue on Administration-Models-FWOS Override - RBeck
                    else
                    {
                        ChangePending = false;
                        ErrorFound = true;
                    }
                //TT#691 End  - MD - Validation issue on Administration-Models-FWOS Override - RBeck
				}
			}
			// END Issue 5119
			Close();
            return ErrorFound;
		}

		private void btnSaveRange_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			int startDate = 9999999, endDate = 0;
			if (DetermineSelectedDates(ref startDate, ref endDate))
			{
				if (startDate < 9999999 && endDate > 0)
				{
					if (this.rbStatic.Checked)
					{
						_selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.Static);
					}
					else if (this.rbDynamic.Checked)
					{
						_selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.Dynamic);
					}
					else if (this.rbRecurring.Checked)
					{
						_selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.Reoccurring);
					}
					else if (this.rbDynamicSwitch.Checked)
					{
						// Begin Issue 4522 stodd 8.9.7
						_selectedDateRange = BuildDateRange(startDate, endDate, eCalendarRangeType.DynamicSwitch);
						// End Issue 4522
					}

					//**********************************
					// Enter new name and validate it
					//**********************************
					NameDialog dateRangeNameForm = new NameDialog("Date Range Name");
					dateRangeNameForm.StartPosition = FormStartPosition.CenterScreen;
					dateRangeNameForm.TextValue = _selectedDateRange.Name;
					bool nameOk = false;
					bool cancelAction = false;
					while (!(nameOk || cancelAction))
					{
						DialogResult theResult = dateRangeNameForm.ShowDialog();
						if (theResult == DialogResult.OK)
						{
							// Begin Track # 6425 - stodd
							dateRangeNameForm.TextValue = dateRangeNameForm.TextValue.Trim();
							// End Track # 6425 - stodd
							if (dateRangeNameForm.TextValue.Length > 50)
								MessageBox.Show("Date Range Name exceeds maximum of 50 characters.  Please correct.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
							else
							{
								DataRow dr = _dvDateRangesWithNames.Table.Rows.Find(dateRangeNameForm.TextValue);
								if (dr != null)
									MessageBox.Show("A Date Range already exists with the name - " + dateRangeNameForm.TextValue,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
									// Begin Issue 3829 - stodd
								else if (this._calendar.DateRangeSelector_NameExists(dateRangeNameForm.TextValue))
									MessageBox.Show("A Date Range already exists with the name - " + dateRangeNameForm.TextValue,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
									// End Issue 3829 - stodd
								else
									nameOk = true;
							}
						}
						else
						{
							cancelAction = true;
						}
					}

					//*****************
					// Update new name
					//*****************
					if (nameOk)
					{
						_selectedDateRange.Name = dateRangeNameForm.TextValue;
						_selectedDateRange.Key = Include.UndefinedCalendarDateRange;  //will force an add
						SaveDateRange();	//updates/adds on DB
						
						listBox1.BeginUpdate();
						if (_anchorDate != null)
							_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
								this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);
						else
							_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
								this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);

						this.listBox1.DataSource = _dvDateRangesWithNames;
						listBox1.Refresh();
						listBox1.Text = _selectedDateRange.Name;
						listBox1.EndUpdate();
					}
				}
			}
			Cursor.Current = Cursors.Default;
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
            // Begin TT#207 - MID Track #6451 - RMatelic - Clear button not working correctly 
            //_selectedDateRange = new DateRangeProfile(Include.UndefinedCalendarDateRange);
            // End TT#207
			this.Tag = _selectedDateRange;
			DialogResult = DialogResult.OK;
			ChangePending = true;  // Issue 5119
            _clearDateRange = true; // TT#207 MID Track #6451
		}

		private void rbPeriod_Click(object sender, System.EventArgs e)
		{
            listBox1_ClearSelection();  // Issue 5672
			//ResetSelectedDateRangeName();
			ChangePending = true;  // Issue 5119
		}

		private void rbWeek_Click(object sender, System.EventArgs e)
		{
            listBox1_ClearSelection();  // Issue 5672
			//ResetSelectedDateRangeName();
			ChangePending = true;  // Issue 5119
		}

		private void rbDay_Click(object sender, System.EventArgs e)
		{
			//ResetSelectedDateRangeName();
			ChangePending = true;  // Issue 5119
		}

		private void rbRelativeToCurrent_CheckedChanged(object sender, System.EventArgs e)
		{
			//ResetSelectedDateRangeName();
			if (this._formLoaded)
				ChangePending = true;  // Issue 5119
		}

		private void rbRelativeToPlan_Click(object sender, System.EventArgs e)
		{
			//ResetSelectedDateRangeName();
			ChangePending = true;  // Issue 5119
		}

		private void rbRelativeToStore_Click(object sender, System.EventArgs e)
		{
			//ResetSelectedDateRangeName();
			ChangePending = true;  // Issue 5119
		}

		private void listBox1_SelectedValueChanged(object sender, System.EventArgs e)
		{
		}

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_newSelection)
			{
				if (listBox1.ContainsFocus)
				{
					int index = listBox1.SelectedIndex;
					if (index > -1 && _formLoaded)  // a date range is selected
					{
                        ultraGrid1.Selected.Cells.Clear();
                        // Begin Track #6432 - JSmith - Changing date range changes other date ranges
                        //PopulateFromDataBase(Convert.ToInt32(listBox1.SelectedValue, CultureInfo.CurrentUICulture));
                        PopulateFromDataBase(Convert.ToInt32(listBox1.SelectedValue, CultureInfo.CurrentUICulture), false);
                        // End Track #6432
						//initializeGridShading(_displayedYears[0], ultraGrid1);
					}
					if (this._formLoaded)
						ChangePending = true;  // Issue 5119
				}
			}
			_newSelection = false;
		}

		private void listBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void listBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.Delete ) //Delete
			{
				try
				{
					DialogResult questionResult = MessageBox.Show("Are you sure you want to Delete Calendar Date Range: " +
						listBox1.Text + "?","Delete Date Range", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (questionResult == DialogResult.Yes)	
					{
						_calendar.DeleteDateRange(Convert.ToInt32(listBox1.SelectedValue, CultureInfo.CurrentUICulture));
						if (_anchorDate != null)
							_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
								this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);
						else
							_dvDateRangesWithNames = _calendar.GetDateRangesWithNames(this._allowDynamicToCurrent, this._allowDynamicToPlan, this._allowDynamicToStoreOpen,
								this._allowReoccurring, this._restrictToOnlyWeeks, this._restrictToOnlyPeriods, this._allowDynamicSwitch);
						listBox1.DataSource = _dvDateRangesWithNames;
					}
				}
				catch ( Exception )
				{
					MessageBox.Show("Requested Calendar Date Range could not be deleted." +
						" It is being used elsewhere in the system.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);						
				}
			}

			if (e.KeyCode == Keys.K && (e.Control && e.Shift))
			{	
				MessageBox.Show(_selectedDateRange.Key.ToString(CultureInfo.CurrentUICulture));
			}
		}

		private void listBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_newSelection = true;
		}

		//this is NOT an event call.  Just a method to do some clearing.
		private void listBox1_ClearSelection()
		{
			if (this.DateRangeRID > 0 && DateRangeRID != Include.UndefinedCalendarDateRange)
			{
				// hopefully this catches where the original date range RID sent WAS the pre-defined
				// date range.  We want to then reset it to undefined, else it's OK to use the 
				// original RID
				DateRangeProfile cdr = _calendar.GetDateRange(DateRangeRID);
				if (cdr.Name != string.Empty)
				{
					_selectedDateRange.Key = Include.UndefinedCalendarDateRange;
					_selectedDateRange.Name = string.Empty;
				}
				else
				{
					_selectedDateRange.Key = DateRangeRID;
					_selectedDateRange.Name = string.Empty;
				}

			}
			else
			{
				_selectedDateRange.Key = Include.UndefinedCalendarDateRange;
				_selectedDateRange.Name = string.Empty;
			}
			listBox1.ClearSelected();
		}

		private void CalendarDateSelector_Activated(object sender, System.EventArgs e)
		{
			if (_selectedDateRange.Name == "" || _selectedDateRange.Name == null)
			{
				listBox1_ClearSelection();
			}
			else // they selected a "named" date range from the listbox
			{
				if (!_formLoaded )
				{	
					string name = _selectedDateRange.Name;
					int sIndex = listBox1.FindStringExact(name);
					if (sIndex != -1)
					{
						listBox1.SetSelected(sIndex,true);
					}
				}
			}

			if (_selectedDateRange.StartDateKey == 0)
				ScrollToYear(_calendar.CurrentDate.FiscalYear);
			else
				ScrollToYear(_startYearAtLoad);
		}

		private void ultraGrid1_Click(object sender, System.EventArgs e)
		{
			// Begin MID Issue #229 - stodd
			if (!_scrollBarClick)
			// End MID Issue #229 - stodd
			{				
				listBox1_ClearSelection();
			}	
		}

		private void ultraGrid1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Begin MID Issue #229 - stodd
			if (!_scrollBarClick)
			// End MID Issue #229 - stodd
			{
				ClearSelections();
			
				// get first selected cell
				Infragistics.Win.UIElement mouseUIElement;
				_point = new Point(e.X,e.Y);
				mouseUIElement = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(_point);
				if ( mouseUIElement != null )
				{
					_grid1FirstCell = (UltraGridCell)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));
                    // Begin TT#220 - RMatelic - Selecting date on calendar in general method - application abends
                    if (_grid1FirstCell != null)
                    {
                        //if (rbPeriod.Checked && _grid1FirstCell != null)  // insures entire period gets selected
                        if (rbPeriod.Checked)
                        {
                            SelectRow(_grid1FirstCell);
                        }
                        // Begin Track #6220 - JSmith - Can not call base method 
                        else if (rbWeek.Checked && _grid1FirstCell.Column.Header.Caption == "Name")
                        {
                            _selectRow = true;
                            SelectRow(_grid1FirstCell);
                        }
                        // End Track #6220
                    }
                }   // End TT#220
			}
		}

		private void ultraGrid1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Begin Track #6220 - JSmith - Can not call base method 
			_selectRow = false;
			// End Track #6220
        }

		private void ultraGrid1_BeforeRowRegionScroll(object sender, Infragistics.Win.UltraWinGrid.BeforeRowRegionScrollEventArgs e)
		{

		}

		private void ultraGrid1_AfterRowRegionScroll(object sender, Infragistics.Win.UltraWinGrid.RowScrollRegionEventArgs e)
		{
		
		}

		private void ultraGrid1_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
		{	
			if (rbPeriod.Checked)
			{
				Infragistics.Win.UIElement mouseUIElement = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(_point);
				if ( mouseUIElement != null )
				{
					UltraGridRow aRow = (UltraGridRow)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow)); 

					if (aRow != null)
						SelectRow(aRow);			
				}
			}
		}

		private void ultraGrid1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_point = new Point(e.X,e.Y);
			if (rbPeriod.Checked)
			{
				if (e.Button == MouseButtons.Left)
				{
					//_point = new Point(e.X,e.Y);
					Infragistics.Win.UIElement mouseUIElement = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(_point);
					if ( mouseUIElement != null )
					{
						UltraGridRow aRow = (UltraGridRow)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow)); 

						if (aRow != null)
							SelectRow(aRow);			
					}
				}
			}
		}

		private void ultraGrid1_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
			// Begin Track #6220 - JSmith - Can not call base method
//			if (rbPeriod.Checked)
			if (rbPeriod.Checked ||
				_selectRow)
			// End Track #6220
			{
                Infragistics.Win.UIElement mouseUIElement = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(_point);
                if (mouseUIElement != null)
                {
                    UltraGridRow aRow = (UltraGridRow)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));

                    if (aRow != null)
                        SelectRow(aRow);
                }
            }
            else if (rbWeek.Checked)
            {
                //--------Used To Unselect Name Columns-------------- 
                SelectedCellsCollection selectedCells = ((UltraGrid)sender).Selected.Cells;
                foreach (UltraGridCell cell in selectedCells)
                {
                    if (cell.Column.Header.Caption == "Name")
                    {
                        cell.Selected = false;
                    }
                }
            }

			if (!_changedByProgram)
			{
				if (this.AllowDynamicSwitch)
				{
					int startDate = 9999999, endDate = 0;
					if (DetermineSelectedDates(ref startDate, ref endDate))
					{
						if (startDate < 9999999)
						{
							int startDateKey = _calendar.GetWeekKey(startDate);
							WeekProfile wp = _calendar.GetWeek(startDateKey);
							this.btnDynamicSwitch.Text = FormatAnchorDateDisplay(wp);
							btnDynamicSwitch.Tag = wp; 
						}
					}
				}
			}
			
			if (this._formLoaded)
				ChangePending = true;
		}

		private void ultraGrid1_BeforeSelectChange(object sender, Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventArgs e)
		{

        }

		private void ultraGrid1_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void ultraGrid1_AfterRowActivate(object sender, System.EventArgs e)
		{

		}

		public Infragistics.Win.ISelectionStrategy GetSelectionStrategy(Infragistics.Shared.ISelectableItem item)
		{
			if(item is UltraGridCell)
			{
				return this.mySelectionStrategy;
			}
			return null;
		}

		private void listBox1_Leave(object sender, System.EventArgs e)
		{
			
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if (this.DateRangeRID > 0 && DateRangeRID != Include.UndefinedCalendarDateRange)
				_selectedDateRange = new DateRangeProfile(DateRangeRID);
			else
				_selectedDateRange = new DateRangeProfile(Include.UndefinedCalendarDateRange);

			this.Tag = _selectedDateRange;
			DialogResult = DialogResult.Cancel;
			Close();
		}

		// Begin MID Issue #229 - stodd
		private void ultraGrid1_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			if ( e.Element.GetType() == typeof(ScrollTrackSubAreaUIElement)
				|| e.Element.GetType() == typeof(ScrollArrowUIElement)
				|| e.Element.GetType() == typeof(RowScrollbarUIElement)
				|| e.Element.GetType() == typeof(ScrollTrackUIElement)
				|| e.Element.GetType() == typeof(ScrollThumbUIElement))
				_scrollBarClick = true;
		}

		private void ultraGrid1_MouseLeaveElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			if ( e.Element.GetType() == typeof(ScrollTrackSubAreaUIElement)
				|| e.Element.GetType() == typeof(ScrollArrowUIElement)
				|| e.Element.GetType() == typeof(RowScrollbarUIElement)
				|| e.Element.GetType() == typeof(ScrollTrackUIElement)
				|| e.Element.GetType() == typeof(ScrollThumbUIElement))
				_scrollBarClick = false;
		}

		// End MID Issue #229 - stodd

		private void ultraGrid1_DoubleClick(object sender, System.EventArgs e)
		{
			
		}

		private void CalendarDateSelector_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.K && (e.Control && e.Shift))
			{	
				MessageBox.Show(_selectedDateRange.Key.ToString(CultureInfo.CurrentUICulture));
			}
		}

		// BEGIN Issue 5171
		private void btnDynamicSwitch_Click(object sender, System.EventArgs e)
		{
			DynamicSwitchDateSelector DynSwitchSelector;
			DynSwitchSelector = new DynamicSwitchDateSelector(base.SAB);
			if (btnDynamicSwitch.Tag != null)
				DynSwitchSelector.SelectedWeek = (WeekProfile)btnDynamicSwitch.Tag;
			DynSwitchSelector.Location = new Point(this.Location.X + this.btnDynamicSwitch.Location.X + 
				this.groupBox1.Location.X + btnDynamicSwitch.Size.Width + 3,
				this.Location.Y + btnDynamicSwitch.Location.Y + (this.groupBox1.Location.Y / 2)); 
			DialogResult aResult = DynSwitchSelector.ShowDialog();
			if (aResult == DialogResult.OK)
			{
				_selectedDateRange.DynamicSwitchDate = DynSwitchSelector.SelectedWeek.Key;
				btnDynamicSwitch.Tag = DynSwitchSelector.SelectedWeek;
				btnDynamicSwitch.Text = FormatAnchorDateDisplay(DynSwitchSelector.SelectedWeek);
				ChangePending = true;
			}
			DynSwitchSelector.Dispose();
		}
		// END Issue 5171

		/// <summary>
		/// MyCustomSelectionStrategy Class used to turn on snaking
		/// </summary>
		internal class MyCustomSelectionStrategy : Infragistics.Win.SelectionStrategyExtended
		{
            private bool _isWeek;
            public bool IsWeek
            {
                get { return _isWeek; }
                set { _isWeek = value; }
            }

			private Infragistics.Win.ISelectionManager manager;

			public MyCustomSelectionStrategy(Infragistics.Win.ISelectionManager manager):base(manager)
			{
				this.manager = manager;
			}
	
			public override bool OnMouseMessage(Infragistics.Shared.ISelectableItem item, ref Infragistics.Win.MouseMessageInfo msginfo)
			{
				try
				{
					if (item != null && msginfo.MouseMessageType == Infragistics.Win.MouseMessageType.Move)
					{
                        //if (_isWeek)
                        //{
                            manager.SetPivotItem(item, true); //<--Must set pivot to get it later on
                        //}

						Infragistics.Shared.ISelectableItem pivotItem = manager.GetPivotItem(item);

						Infragistics.Win.UltraWinGrid.UltraGridCell itemCell = item as Infragistics.Win.UltraWinGrid.UltraGridCell;
						Infragistics.Win.UltraWinGrid.UltraGridCell pivotCell = pivotItem as Infragistics.Win.UltraWinGrid.UltraGridCell;

						if (pivotCell != null && itemCell != null && itemCell.Row != pivotCell.Row)
							manager.EnterSnakingMode(item);

						if (itemCell != null)
						{
							if (itemCell.Column.Header.Caption == "Name")
								itemCell.Selected = false;

						}
					}

					return base.OnMouseMessage(item, ref msginfo);
				}
				finally
				{
			
				}
			}

		}

        private void rbWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWeek.Checked)
            {
                mySelectionStrategy.IsWeek = true;

                ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
                ultraGrid1.DisplayLayout.Override.SelectTypeRow = SelectType.Extended;
            }
            else
            {
                mySelectionStrategy.IsWeek = false;

                ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                ultraGrid1.DisplayLayout.Override.SelectTypeRow = SelectType.None;
            }
        }	

        // Begin TT#1953-MD - RO Web
        // Methods to support RO Web
        public void SetUp(int iDateRangeRID)
        {
            _dateRangeRID = iDateRangeRID;
            CalendarDateSelector_Load(this, new System.EventArgs());
        }

        public DataView GetPredefinedDateRanges()
        {
            return _dvDateRangesWithNames;
        }

        public void GetSelectedDates(out WeekProfile selectedStartWeek, out WeekProfile selectedEndWeek)
        {
            selectedStartWeek = _selectedStartWeek;
            selectedEndWeek = _selectedEndWeek;
        }

        public void GetCurrentDateInformation(out string sCurrentSalesDate, out string sCurrentSalesDay, out string sCurrentSalesWeek,
            out string sCurrentDate, out string sPlanDate, out string sStoreOpenDate)
        {
            sCurrentSalesDate = lblCurrentDay.Text;
            sCurrentSalesDay = lblDay.Text;
            sCurrentSalesWeek = lblCurrentWeek.Text;
            sCurrentDate = lblCurrent.Text;
            sPlanDate = lblPlan.Text;
            sStoreOpenDate = lblStoreOpen.Text;
        }
        // End TT#1953-MD - RO Web
	}
}
