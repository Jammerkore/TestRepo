using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
    public class CalendarDateSelectorManager
    {

        #region Fields
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private MRSCalendar _calendar;
        private int _currentYear = 0;
        private CalendarDateSelector _calendarDateSelector;
        private Profile _anchorDate = null;
        private DayProfile _anchorDay = null;
        private WeekProfile _anchorWeek = null;
        private PeriodProfile _anchorPeriod = null;
        private WeekProfile _btnDynamicSwitchTag = null;  // TODO:  FIX WHEN NEED TO HANDLE DYNAMIC DATES!
        private DateTime _nullDate = new DateTime(1, 1, 1);


        #endregion Fields
        public CalendarDateSelectorManager(SessionAddressBlock SAB)
        {
            _SAB = SAB;
            _calendar = _SAB.ClientServerSession.Calendar;
            _currentYear = _calendar.CurrentWeek.FiscalYear;
            _calendarDateSelector = new CalendarDateSelector(_SAB);
            _calendarDateSelector.PopulateForm = false;
        }

        public void SetCalendarDateSelectorProperties(int iDateRangeRID, int iAnchorDateRangeRID, eDateRangeRelativeTo anchorDateRelativeTo,
            bool bAllowDynamicToCurrent, bool bAllowDynamicToPlan,
            bool bAllowDynamicToStoreOpen, bool bAllowReoccurring, bool bRestrictToOnlyWeeks, bool bRestrictToOnlyPeriods, bool bAllowDynamicSwitch)
        {
            _calendarDateSelector.DateRangeRID = iDateRangeRID;
			_calendarDateSelector.AnchorDateRelativeTo = anchorDateRelativeTo;
            if (anchorDateRelativeTo == eDateRangeRelativeTo.StoreOpen
                && iAnchorDateRangeRID > 1)  // must be store key
            {
                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(iAnchorDateRangeRID);
                if (storeProfile != null
                    && storeProfile.SellingOpenDt != _nullDate)
                {
                    DayProfile dayProfile = _SAB.ClientServerSession.Calendar.GetDay(storeProfile.SellingOpenDt);
                    _calendarDateSelector.AnchorDate = dayProfile;
                }
            }
            else if (iAnchorDateRangeRID != Include.UndefinedCalendarDateRange)
            {
                _calendarDateSelector.AnchorDateRangeRID = iAnchorDateRangeRID;
            }
            else
            {
                _calendarDateSelector.AnchorDate = _SAB.ClientServerSession.Calendar.CurrentDate;
            }
            _calendarDateSelector.AllowDynamicToCurrent = bAllowDynamicToCurrent;
            _calendarDateSelector.AllowDynamicToPlan = bAllowDynamicToPlan;
            _calendarDateSelector.AllowDynamicToStoreOpen = bAllowDynamicToStoreOpen;
            _calendarDateSelector.AllowReoccurring = bAllowReoccurring;
            _calendarDateSelector.RestrictToOnlyWeeks = bRestrictToOnlyWeeks;
            _calendarDateSelector.RestrictToOnlyPeriods = bRestrictToOnlyPeriods;
            _calendarDateSelector.AllowDynamicSwitch = bAllowDynamicSwitch;

            _calendarDateSelector.SetUp(iDateRangeRID);
        }

        public DataView GetDateRangesWithNames()
        {
            return _calendarDateSelector.GetPredefinedDateRanges();
        }

        public DataTable GetCurrentDateTable()
        {
            WeekProfile selectedStartWeek = new WeekProfile(Include.NoRID);
            WeekProfile selectedEndWeek = new WeekProfile(Include.NoRID);
            string currentSalesDate, currentSalesDay, currentSalesWeek, sCurrentDate, sPlanDate, sStoreOpenDate;

            DataTable dt = new DataTable("Current Date");

            DataColumn myDataColumn;

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.String");
            myDataColumn.ColumnName = "Current Sales Week Label";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "Current Sales Week TimeID";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "Current Sales Week";
            dt.Columns.Add(myDataColumn);

             myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.String");
            myDataColumn.ColumnName = "Current Sales Day Label";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "Current Sales Day TimeID";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.String");
            myDataColumn.ColumnName = "Current Sales Date Label";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.String");
            myDataColumn.ColumnName = "Current Date Label";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.String");
            myDataColumn.ColumnName = "Current Plan Date Label";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.String");
            myDataColumn.ColumnName = "Store Open Date Label";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "Selected Start Week";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "Selected End Week";
            dt.Columns.Add(myDataColumn);

            _calendarDateSelector.GetSelectedDates(out selectedStartWeek, out selectedEndWeek);
            _calendarDateSelector.GetCurrentDateInformation(out currentSalesDate, out currentSalesDay, out currentSalesWeek, out sCurrentDate, out sPlanDate, out sStoreOpenDate);

            DataRow dr;
            dr = dt.NewRow();
            dr["Current Sales Week Label"] = currentSalesWeek;
            dr["Current Sales Week TimeID"] = _calendar.CurrentWeek.Key;
            dr["Current Sales Week"] = _calendar.CurrentWeek.YearWeek;
            dr["Current Sales Day Label"] = currentSalesDay;
            dr["Current Sales Day TimeID"] = _calendar.CurrentDate.Key;
            dr["Current Sales Date Label"] = currentSalesDate;
            dr["Current Date Label"] = sCurrentDate;
            dr["Current Plan Date Label"] = sPlanDate;
            dr["Store Open Date Label"] = sPlanDate;
            dr["Selected Start Week"] = selectedStartWeek.YearWeek;
            dr["Selected End Week"] = selectedEndWeek.YearWeek;
            dt.Rows.Add(dr);

            return dt;
        }

        public void GetSelectedDates(int iDateRangeRID, out WeekProfile selectedStartWeek, out WeekProfile selectedEndWeek)
        {
            if (_calendarDateSelector.DateRangeRID != iDateRangeRID)
            {
                _calendarDateSelector.DateRangeRID = iDateRangeRID;
                if (_anchorWeek != null)
                {
                    _calendarDateSelector.AnchorDate = _anchorWeek;
                }
                _calendarDateSelector.SetUp(iDateRangeRID);
            }

            _calendarDateSelector.GetSelectedDates(out selectedStartWeek, out selectedEndWeek);
        }

        public DataTable GetDateSelectionTable()
        {
            return _calendar.DateSmallSelectionDataTable;
        }

        public DateRangeProfile GetDateRangeProfile(int iDateRangeRID, int anchorDateKey = Include.Undefined)
        {
            DateRangeProfile dateRangeProfile = null;

            if (iDateRangeRID == Include.NoRID)
            {
                dateRangeProfile = new DateRangeProfile(Include.NoRID);
            }
            else
            {
                dateRangeProfile = _calendar.GetDateRange(iDateRangeRID);
                if (dateRangeProfile.RelativeTo == eDateRangeRelativeTo.Plan
                    && anchorDateKey != Include.UndefinedCalendarDateRange)
                {
                    dateRangeProfile = _calendar.GetDateRange(iDateRangeRID, anchorDateKey);
                }
                else if (dateRangeProfile.RelativeTo == eDateRangeRelativeTo.StoreOpen
                    && anchorDateKey != Include.UndefinedCalendarDateRange)
                {
                    StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(anchorDateKey);
                    DayProfile dayProfile = null;
                    if (storeProfile != null
                        && storeProfile.SellingOpenDt != _nullDate)
                    {
                        dayProfile = _SAB.ClientServerSession.Calendar.GetDay(storeProfile.SellingOpenDt);

                    }
                    dateRangeProfile = _calendar.GetDateRange(iDateRangeRID, dayProfile);
                }
            }
            return dateRangeProfile;
        }

        public DataTable DefineDateCriteriaTable()
        {
            DataTable dtDateCriteriaDataTable = new DataTable("DateCriteriaTable");

            return dtDateCriteriaDataTable;
        }

        //TODO: rename
        public DateRangeProfile OkButtonClicked(
            int cdrRID, 
            int startDate, 
            int endDate, 
            eCalendarDateType dateType, 
            eCalendarRangeType dateRangeType, 
            eDateRangeRelativeTo relativeTo,
            int anchorDateKey
            )
        {
            if (relativeTo == eDateRangeRelativeTo.StoreOpen
                && anchorDateKey > 1)  // must be store key
            {
                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(anchorDateKey);
                if (storeProfile != null
                    && storeProfile.SellingOpenDt != _nullDate)
                {
                    DayProfile dayProfile = _SAB.ClientServerSession.Calendar.GetDay(storeProfile.SellingOpenDt);
                    _anchorWeek = dayProfile.Week;
                    _anchorPeriod = dayProfile.Period;
                }
            }

            DateRangeProfile selectedDateRange = BuildDateRange(cdrRID, startDate, endDate, dateType, dateRangeType, relativeTo, anchorDateKey);
            bool anchorDateOverriden = (dateRangeType == eCalendarRangeType.Dynamic) && (relativeTo != eDateRangeRelativeTo.Current);

            // TODO: remove the side effect in GetDisplayDate 
            _calendar.GetDisplayDate(selectedDateRange, anchorDateOverriden);
            // TODO: remove the if since the name will ALWAYS be null
            if (selectedDateRange.Name == "" || selectedDateRange.Name == null)
            {
                SaveDateRange(selectedDateRange);
            }

            //=========================================================================
            // If this is a dynamic switch date, we want to return the date range
            // (either static or dynamic) determined by the dynamic switch logic.
            //=========================================================================
            if (selectedDateRange.DateRangeType == eCalendarRangeType.DynamicSwitch)
            {
                selectedDateRange = _calendar.GetDateRange(selectedDateRange.Key, true);
            }
            else if (selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
            {
                if (anchorDateOverriden
                    && anchorDateKey != Include.UndefinedCalendarDateRange)
                {
                    if (relativeTo == eDateRangeRelativeTo.StoreOpen
					    && anchorDateKey > 1)  // must be store key
                    {
                        StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(anchorDateKey);
                        if (storeProfile != null
                            && storeProfile.SellingOpenDt != _nullDate)
                        {
                            DayProfile dayProfile = _SAB.ClientServerSession.Calendar.GetDay(storeProfile.SellingOpenDt);
                            selectedDateRange = _calendar.GetDateRange(selectedDateRange.Key, dayProfile);
                        }
                        else
                        {
                            selectedDateRange = _calendar.GetDateRange(selectedDateRange.Key, anchorDateKey);
                        }
                    }
                    else
                    {
                        selectedDateRange = _calendar.GetDateRange(selectedDateRange.Key, anchorDateKey);
                    }
                }
            }

            return selectedDateRange;
        }
        public string RenameDateRange(int cdrRID, string rangeName)
        {
            try
            {
                //todo: rename the method in calendar to renamedaterange
                _calendar.UpdateDateRange(cdrRID, rangeName);
            }
            catch (Exception)
            {
                throw new Exception("Rename failed with rangeName=" + rangeName);
            }
            return "OK";

        }
        public string DeleteDateRange(int cdrRID)
        {
            try { 
                _calendar.DeleteDateRange(cdrRID);
            }
            catch (Exception)
            {
                throw new Exception("Delete failed with rangeId=" + cdrRID.ToString());
            } 
            return "OK";

        }

        //TODO: rename
        public DateRangeProfile SaveRangeButtonClicked(
            int cdrRID, 
            string dateRangeName, 
            int startDate, 
            int endDate, 
            eCalendarDateType dateType, 
            eCalendarRangeType dateRangeType, 
            eDateRangeRelativeTo relativeTo,
            int anchorDateKey
            )
        {
            if (relativeTo == eDateRangeRelativeTo.StoreOpen
			    && anchorDateKey > 1)  // must be store key
            {
                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(anchorDateKey);
                if (storeProfile != null
                    && storeProfile.SellingOpenDt != _nullDate)
                {
                    DayProfile dayProfile = _SAB.ClientServerSession.Calendar.GetDay(storeProfile.SellingOpenDt);
                    _anchorWeek = dayProfile.Week;
                    _anchorPeriod = dayProfile.Period;
                }
            }

            DateRangeProfile selectedDateRange = BuildDateRange(cdrRID, startDate, endDate, dateType, dateRangeType, relativeTo, anchorDateKey);

            //TODO: shouldn't be an "update" - rather this should always create a new date range

            //*****************
            // Update new name
            //*****************
            selectedDateRange.Name = dateRangeName;
            selectedDateRange.Key = cdrRID;
            SaveDateRange(selectedDateRange);	//updates/adds on DB

            return selectedDateRange;
        }

        /// <summary>
        /// This expects the startDate and endDate to be in FISCAL format.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="aCalendarRangeType"></param>
        /// <returns></returns>
        private DateRangeProfile BuildDateRange(
            int key, 
            int startDate, 
            int endDate, 
            eCalendarDateType dateType, 
            eCalendarRangeType dateRangeType, 
            eDateRangeRelativeTo relativeTo,
            int anchorDateKey
            )
        {
            DateRangeProfile selectedDateRange = new DateRangeProfile(key);
            int oldStartDate = startDate;
            int oldEndDate = endDate;

            selectedDateRange.SelectedDateType = dateType;
            if (dateType == DataCommon.eCalendarDateType.Period)
            {
                //COnvert startDate and endDate to thier key values
                startDate = _calendar.GetPeriodKey(startDate);
                endDate = _calendar.GetPeriodKey(endDate);

                if (dateRangeType == eCalendarRangeType.Dynamic)
                {
                    if (relativeTo == eDateRangeRelativeTo.Current)
                    {
                        selectedDateRange.StartDateKey = _calendar.ConvertToDynamicPeriod(startDate);
                        selectedDateRange.EndDateKey = _calendar.ConvertToDynamicPeriod(endDate);
                    }
                    else
                    {
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorPeriod == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                        }
                        // End Track #5833

                        selectedDateRange.InternalAnchorDate = _anchorPeriod;
                        selectedDateRange.StartDateKey = _calendar.ConvertToDynamicPeriod(_anchorPeriod, startDate);
                        selectedDateRange.EndDateKey = _calendar.ConvertToDynamicPeriod(_anchorPeriod, endDate);
                    }
                }
                else if (dateRangeType == eCalendarRangeType.Static)
                {
                    selectedDateRange.StartDateKey = startDate;
                    selectedDateRange.EndDateKey = endDate;
                }
                else if (dateRangeType == eCalendarRangeType.Reoccurring)
                {
                    selectedDateRange.StartDateKey = StripYear(oldStartDate);
                    selectedDateRange.EndDateKey = StripYear(oldEndDate);
                }
                else if (dateRangeType == eCalendarRangeType.DynamicSwitch)
                {
                    selectedDateRange.StartDateKey = startDate;
                    selectedDateRange.EndDateKey = endDate;
                    selectedDateRange.IsDynamicSwitch = true;
                    if (_btnDynamicSwitchTag == null)
                    {
                        selectedDateRange.DynamicSwitchDate = startDate;
                    }
                    else
                    {
                        WeekProfile wp = (WeekProfile)_btnDynamicSwitchTag;
                        selectedDateRange.DynamicSwitchDate = wp.Key;
                    }
                }
            }
            else if (dateType == DataCommon.eCalendarDateType.Week)
            {
                //COnvert startDate and endDate to thier key values
                startDate = _calendar.GetWeekKey(startDate);
                endDate = _calendar.GetWeekKey(endDate);

                if (dateRangeType == eCalendarRangeType.Dynamic)
                {
                    if (relativeTo == eDateRangeRelativeTo.Current)
                    {
                        selectedDateRange.StartDateKey = _calendar.ConvertToDynamicWeek(startDate);
                        selectedDateRange.EndDateKey = _calendar.ConvertToDynamicWeek(endDate);
                    }
                    else if (relativeTo == eDateRangeRelativeTo.Plan)
                    {
                        _anchorWeek = _calendar.GetFirstWeekOfRange(anchorDateKey);
                        selectedDateRange.StartDateKey = _calendar.ConvertToDynamicWeek(_anchorWeek, startDate);
                        selectedDateRange.EndDateKey = _calendar.ConvertToDynamicWeek(_anchorWeek, endDate);
                    }
                    else
                    {
                        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                        if (_anchorWeek == null)
                        {
                            SetUpAnchorDate(_calendar.PostDate);
                        }
                        // End Track #5833

                        selectedDateRange.InternalAnchorDate = _anchorWeek;
                        selectedDateRange.StartDateKey = _calendar.ConvertToDynamicWeek(_anchorWeek, startDate);
                        selectedDateRange.EndDateKey = _calendar.ConvertToDynamicWeek(_anchorWeek, endDate);
                    }
                }
                else if (dateRangeType == eCalendarRangeType.Static)
                {
                    selectedDateRange.StartDateKey = startDate;
                    selectedDateRange.EndDateKey = endDate;
                }
                else if (dateRangeType == eCalendarRangeType.Reoccurring)
                {
                    selectedDateRange.StartDateKey = StripYear(oldStartDate);
                    selectedDateRange.EndDateKey = StripYear(oldEndDate);
                }
                else if (dateRangeType == eCalendarRangeType.DynamicSwitch)
                {
                    selectedDateRange.StartDateKey = startDate;
                    selectedDateRange.EndDateKey = endDate;
                    selectedDateRange.IsDynamicSwitch = true;
                    if (_btnDynamicSwitchTag == null)
                    {
                        selectedDateRange.DynamicSwitchDate = startDate;
                    }
                    else
                    {
                        WeekProfile wp = (WeekProfile)_btnDynamicSwitchTag;

                        selectedDateRange.DynamicSwitchDate = wp.Key;
                    }
                }
            }

            selectedDateRange.DateRangeType = dateRangeType;
            if (dateRangeType == eCalendarRangeType.Dynamic)
            {
                selectedDateRange.RelativeTo = relativeTo;
            }

            return selectedDateRange;
        }

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

        private int StripYear(int date)
        {
            string dateString = date.ToString("000000", CultureInfo.CurrentUICulture);
            string dateSubString = dateString.Substring(4, 2);
            int dateLessYear = Convert.ToInt32(dateSubString, CultureInfo.CurrentUICulture);

            return dateLessYear;
        }

        private DateRangeProfile SaveDateRange(DateRangeProfile dateRangeProfile)
        {
            if (profileNameInUse(dateRangeProfile.Key, dateRangeProfile.Name))
            {
                throw new Exception("Duplicate date range profile name");
            }

            if (dateRangeProfile.Key == Include.UndefinedCalendarDateRange)
            {
                _calendar.AddDateRange(dateRangeProfile);
            }
            else
            {
                _calendar.UpdateDateRange(dateRangeProfile);
            }

            return dateRangeProfile;
        }

        private bool profileNameInUse(int cdrRID, string sName)
        {
            int foundCDRRID = getCDRRIDForRangeName(sName);
            bool inUse = false;

            if (foundCDRRID != Include.UndefinedCalendarDateRange)
            {
                inUse = foundCDRRID != cdrRID;
            }

            return inUse;
        }

        public int getCDRRIDForRangeName(string sRangeName)
        {
            CalendarData cd = new CalendarData();
            DataTable dtNames = cd.CalendarDateRange_ReadForNames();
            DataColumn[] primaryKeyColumn = new DataColumn[1];
            DataRow foundRow;
            int cdrRID = Include.UndefinedCalendarDateRange;

            primaryKeyColumn[0] = dtNames.Columns["CDR_NAME"];
            dtNames.PrimaryKey = primaryKeyColumn;
            foundRow = dtNames.Rows.Find(sRangeName);

            if (foundRow != null)
            {
                cdrRID = Convert.ToInt32(foundRow["CDR_RID"], CultureInfo.CurrentUICulture);
            }

            return cdrRID;
        }
    }


}
