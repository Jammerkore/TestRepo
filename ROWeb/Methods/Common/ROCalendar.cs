using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class ROCalendar : ROWebFunction
    {
        #region Fields
        //=======
        // FIELDS
        //=======
        private CalendarDateSelectorManager _calendarDateSelectorManager = null;
        private DateRangeProfile _dateRangeProfile;

        #endregion Fields

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROCalendar(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {

        }

        public CalendarDateSelectorManager CalendarDateSelectorManager
        {
            get
            {
                if (_calendarDateSelectorManager == null)
                {
                    _calendarDateSelectorManager = new CalendarDateSelectorManager(SAB);
                }
                return _calendarDateSelectorManager;
            }
        }


        override public void CleanUp()
        {
            _calendarDateSelectorManager = null;
            _dateRangeProfile = null;
        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            //ROCalendarSaveParms parms = (ROCalendarSaveParms)Parms;
            switch (Parms.RORequest) 
            {
                case eRORequest.GetCalendarSelector:
                    return GetCalendarSelector(Parms);
                case eRORequest.GetCalendarModel:
                    return GetCalendarModel(Parms);
                //case eRORequest.UpdateCalendarSelector:
                //    return UpdateCalendarDateRange(parms);
                //case eRORequest.RenameDateRange:
                //    return RenameCalendarDateRange(parms);
                //case eRORequest.DeleteDateRange:
                //    return DeleteCalendarDateRange(parms);
                case eRORequest.UpdateCalendarSelector:
                    return UpdateCalendarDateRange((ROCalendarSaveParms)Parms);
                case eRORequest.RenameDateRange:
                    return RenameCalendarDateRange((ROCalendarSaveParms)Parms);
                case eRORequest.DeleteDateRange:
                    return DeleteCalendarDateRange((ROCalendarSaveParms)Parms);
                case eRORequest.CalculateCalendarDate:
                    return CalculateCalendarDate((ROCalendarDateCalculationParms)Parms);

            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        private ROOut CalculateCalendarDate(ROCalendarDateCalculationParms parms)
        {
            switch (parms.DateCalculationType)
            {
                case eDateCalculationType.GetLY:
                    return GetLYDate(parms);
                case eDateCalculationType.GetApplyTrendTo:
                    return GetApplyTrendToDate(parms);
                default:
                    return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
            }
        }

        private ROOut GetLYDate(ROCalendarDateCalculationParms parms)
        {
            int dateKey = Include.UndefinedCalendarDateRange;
            string displayDate = null;
            string message = null;

            DateRangeProfile dateRange = SAB.ClientServerSession.Calendar.GetDateRange(parms.DateKey);
            DateRangeProfile lastYearDateRange = null;
            if (parms.BaseDateKey > 0)  // contains the key to the plan date
            {
                int planDateRangeKey = parms.BaseDateKey;
                if (planDateRangeKey != Include.UndefinedCalendarDateRange)
                {
                    WeekProfile FirstWeekOfPlan = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange(planDateRangeKey);
                    lastYearDateRange = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dateRange, FirstWeekOfPlan);
                }
                else
                {
                    WeekProfile FirstWeekOfTY = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange(parms.DateKey);
                    lastYearDateRange = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dateRange, FirstWeekOfTY);
                }
            }
            else
            {
                WeekProfile FirstWeekOfTY = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange(parms.DateKey);
                lastYearDateRange = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dateRange, FirstWeekOfTY);
            }

            if (parms.BaseDateKey > 0)
            {
                lastYearDateRange = SAB.ClientServerSession.Calendar.GetDateRange(lastYearDateRange.Key, parms.BaseDateKey);
            }
            else
            {
                lastYearDateRange = SAB.ClientServerSession.Calendar.GetDateRange(lastYearDateRange.Key);
            }

            dateKey = lastYearDateRange.Key;
            displayDate = lastYearDateRange.DisplayDate;

            return new ROCalendarDate(
                ROReturnCode: eROReturnCode.Successful,
                sROMessage: message, 
                ROInstanceID: ROInstanceID, 
                dateKey: dateKey, 
                displayDate: displayDate
                );
        }

        private ROOut GetApplyTrendToDate(ROCalendarDateCalculationParms parms)
        {
            int dateKey = Include.UndefinedCalendarDateRange;
            string displayDate = null;
            string message = null;

            DateRangeProfile dateRange = null;
            if (parms.BaseDateKey > 0)  // contains the key to the plan date
            {
                int planDateRangeKey = parms.BaseDateKey;
                if (planDateRangeKey != Include.UndefinedCalendarDateRange)
                {
                    DateRangeProfile planDateRange = SAB.ClientServerSession.Calendar.GetDateRange(planDateRangeKey);
                    dateRange = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(planDateRange);
                    dateRange = SAB.ClientServerSession.Calendar.ConvertToDynamicToPlan(dateRange, planDateRange);
                }
                else
                {
                    dateRange = SAB.ClientServerSession.Calendar.GetDateRange(parms.DateKey);
                }
            }
            else
            {
                dateRange = SAB.ClientServerSession.Calendar.GetDateRange(parms.DateKey);
            }

            DateRangeProfile trendDateRange = SAB.ClientServerSession.Calendar.GetRangeAsFirstWeekOfRange(dateRange);
            dateKey = trendDateRange.Key;
            displayDate = trendDateRange.DisplayDate;

            return new ROCalendarDate(
                ROReturnCode: eROReturnCode.Successful,
                sROMessage: message,
                ROInstanceID: ROInstanceID,
                dateKey: dateKey,
                displayDate: displayDate
                );
        }

        private ROOut GetCalendarModel(ROParms parms)
        {
            List<ROCalendarTimePeriodModel> modelsList = new List<ROCalendarTimePeriodModel>();
            CalendarModels calendarData = new CalendarModels();

            foreach( object obj in calendarData.Models)
            {
                CalendarModel calendarModel = obj as CalendarModel;

                modelsList.Add(BuildROCalendarModel(calendarModel));
            }

            return new ROCalendarTimePeriodModels(eROReturnCode.Successful, null, ROInstanceID, modelsList);
        }

        private ROCalendarTimePeriodModel BuildROCalendarModel(CalendarModel calendarModel)
        {
            List<ROCalendarTimePeriod> months = BuildCalendarModelPeriodsList(calendarModel.Months);
            List<ROCalendarTimePeriod> quarters = BuildCalendarModelPeriodsList(calendarModel.Quarters);
            List<ROCalendarTimePeriod> seasons = BuildCalendarModelPeriodsList(calendarModel.Seasons);
            ROCalendarTimePeriodModel roCalendarModel = new ROCalendarTimePeriodModel(calendarModel.ModelName, calendarModel.StartDate, calendarModel.FiscalYear, calendarModel.LastModelYear, months, quarters, seasons);

            return roCalendarModel;
        }

        private List<ROCalendarTimePeriod> BuildCalendarModelPeriodsList(ArrayList periods)
        {
            List<ROCalendarTimePeriod> roCalendarModelPeriodsList = new List<ROCalendarTimePeriod>();

            foreach(object obj in periods)
            {
                CalendarModelPeriod modelPeriod = obj as CalendarModelPeriod;
                eROCalendarModelPeriodType roPeriodType = (eROCalendarModelPeriodType) modelPeriod.ModelPeriodType;
                ROCalendarTimePeriod roPeriod = new ROCalendarTimePeriod(modelPeriod.Sequence, modelPeriod.Name, modelPeriod.Abbreviation, modelPeriod.NoOfTimePeriods, roPeriodType);

                roCalendarModelPeriodsList.Add(roPeriod);
            }

            return roCalendarModelPeriodsList;
        }

        private ROOut GetCalendarSelector(ROParms parms)
        {
            ROCalendarSelectorParms calendarParms = (ROCalendarSelectorParms)parms;

            return GetCalendarSelector(calendarParms.DateRangeRID, calendarParms.AnchorDateRangeRID, (eDateRangeRelativeTo)calendarParms.AnchorDateRelativeTo,
                calendarParms.AllowDynamicToCurrent, calendarParms.AllowDynamicToPlan,
                calendarParms.AllowDynamicToStoreOpen, calendarParms.AllowReoccurring,
                calendarParms.RestrictToOnlyWeeks, calendarParms.RestrictToOnlyPeriods, calendarParms.AllowDynamicSwitch);
        }

        private DataTable GetCalendarDateTypeList()
        {
            MIDTextDataHandler textDataHandler = new MIDTextDataHandler("eCalendarDateType", "ID", "Name");

            return textDataHandler.GetUITextTable(eMIDTextType.eCalendarDateType, eMIDTextOrderBy.TextCode);
        }

        private DataTable GetCalendarRangeTypeList(
            bool bAllowDynamic,
            bool bAllowReoccurring,
            bool bAllowDynamicSwitch
            )
        {
            DataTable calendarRangeTypes;
            List<DataRow> RowsToDelete = new List<DataRow>();
            eCalendarRangeType calendarRangeType;

            calendarRangeTypes = eNumConverter.AddEnumsToTable<eCalendarRangeType>("eCalendarRangeType");

            // remove rows based on options
            for (int i = 0; i < calendarRangeTypes.Rows.Count; i++)
            {
                calendarRangeType = (eCalendarRangeType)Convert.ToInt32(calendarRangeTypes.Rows[i]["ID"]);
                if (calendarRangeType == eCalendarRangeType.Dynamic
                    && !bAllowDynamic)
                {
                    RowsToDelete.Add(calendarRangeTypes.Rows[i]);
                }
                else if (calendarRangeType == eCalendarRangeType.Reoccurring
                    && !bAllowReoccurring)
                {
                    RowsToDelete.Add(calendarRangeTypes.Rows[i]);
                }
                else if (calendarRangeType == eCalendarRangeType.DynamicSwitch
                    && !bAllowDynamicSwitch)
                {
                    RowsToDelete.Add(calendarRangeTypes.Rows[i]);
                }
            }

            foreach (var dataRow in RowsToDelete)
            {
                calendarRangeTypes.Rows.Remove(dataRow);
            }

            calendarRangeTypes.AcceptChanges();

            return calendarRangeTypes;
        }

        private DataTable GetDateRangeRelativeToList(
            bool bAllowDynamicToCurrent,
            bool bAllowDynamicToPlan,
            bool bAllowDynamicToStoreOpen)
        {
            DataTable calendarRangeRelativeTo;
            List<DataRow> RowsToDelete = new List<DataRow>();
            eDateRangeRelativeTo dateRangeRelativeTo;

            calendarRangeRelativeTo =  eNumConverter.AddEnumsToTable<eDateRangeRelativeTo>("eDateRangeRelativeTo");

            // remove rows based on options
            for (int i = 0; i < calendarRangeRelativeTo.Rows.Count; i++)
            {
                dateRangeRelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(calendarRangeRelativeTo.Rows[i]["ID"]);
                if (dateRangeRelativeTo == eDateRangeRelativeTo.Current
                    && !bAllowDynamicToCurrent)
                {
                    RowsToDelete.Add(calendarRangeRelativeTo.Rows[i]);
                }
                else if (dateRangeRelativeTo == eDateRangeRelativeTo.Plan
                    && !bAllowDynamicToPlan)
                {
                    RowsToDelete.Add(calendarRangeRelativeTo.Rows[i]);
                }
                else if (dateRangeRelativeTo == eDateRangeRelativeTo.StoreOpen
                    && !bAllowDynamicToStoreOpen)
                {
                    RowsToDelete.Add(calendarRangeRelativeTo.Rows[i]);
                }
            }

            foreach (var dataRow in RowsToDelete)
            {
                calendarRangeRelativeTo.Rows.Remove(dataRow);
            }

            calendarRangeRelativeTo.AcceptChanges();

            return calendarRangeRelativeTo;
        }

        /// <summary>
        /// Returns the list of predefined date ranges
        /// </summary>
        /// <returns>A datatable</returns>
        private ROOut GetCalendarSelector(int iDateRangeRID, int iAnchorDateRangeRID, eDateRangeRelativeTo anchorDateRelativeTo,
            bool bAllowDynamicToCurrent, bool bAllowDynamicToPlan, bool bAllowDynamicToStoreOpen,
                bool bAllowReoccurring, bool bRestrictToOnlyWeeks, bool bRestrictToOnlyPeriods, bool bAllowDynamicSwitch)
        {
            try
            {
                CalendarDateSelectorManager.SetCalendarDateSelectorProperties(iDateRangeRID, iAnchorDateRangeRID, anchorDateRelativeTo,
                    bAllowDynamicToCurrent, bAllowDynamicToPlan, bAllowDynamicToStoreOpen,
                bAllowReoccurring, bRestrictToOnlyWeeks, bRestrictToOnlyPeriods, bAllowDynamicSwitch);

                DataSet dsCalendarSelector = new DataSet("Calendar Selector");

                // Add tables containing enum values
                dsCalendarSelector.Tables.Add(GetCalendarDateTypeList());
                dsCalendarSelector.Tables.Add(GetCalendarRangeTypeList(
				    bAllowDynamic: bAllowDynamicToCurrent || bAllowDynamicToPlan || bAllowDynamicToStoreOpen,
                    bAllowReoccurring: bAllowReoccurring,
                    bAllowDynamicSwitch: bAllowDynamicSwitch));
                dsCalendarSelector.Tables.Add(GetDateRangeRelativeToList(
				    bAllowDynamicToCurrent: bAllowDynamicToCurrent,
                    bAllowDynamicToPlan: bAllowDynamicToPlan,
                    bAllowDynamicToStoreOpen: bAllowDynamicToStoreOpen));

                // Add Predefined Date Ranges
                DataView dv = CalendarDateSelectorManager.GetDateRangesWithNames();

                if (string.IsNullOrEmpty(dv.Table.TableName))
                {
                    dv.Table.TableName = "Predefined Date Ranges";
                }

                dsCalendarSelector.Tables.Add(dv.Table);

                // Add Current Date Table
                dsCalendarSelector.Tables.Add(CalendarDateSelectorManager.GetCurrentDateTable());

                // Add Calendar Selection Table
                dsCalendarSelector.Tables.Add(CalendarDateSelectorManager.GetDateSelectionTable());

                // Add Date Criteria
                dsCalendarSelector.Tables.Add(GetDateRange(iDateRangeRID));

                return new RODataSetOut(eROReturnCode.Successful, null, ROInstanceID, dsCalendarSelector);

            }
            catch
            {
                throw;
            }
        }

        private DataTable GetDateRange(int iDateRangeRID)
        {
            _dateRangeProfile = CalendarDateSelectorManager.GetDateRangeProfile(iDateRangeRID);
            CalendarDateRangeRowHandler rowHandler = CalendarDateRangeRowHandler.GetInstance(_dateRangeProfile, CalendarDateSelectorManager);
            DataTable dt = BuildDateRangeDataTable(rowHandler);

            AddCalendarDateRange(dt, rowHandler);

            return dt;
        }

        private DataTable BuildDateRangeDataTable(CalendarDateRangeRowHandler rowHandler)
        {
            DataTable dt = new DataTable("Calendar Date Range");

            rowHandler.AddUITableColumns(dt);

            return dt;
        }

        private void AddCalendarDateRange(DataTable dt, CalendarDateRangeRowHandler rowHandler)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);

            dt.Rows.Add(dr);
        }

        private ROOut RenameCalendarDateRange(ROCalendarSaveParms parms)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string succ = "Successfully renamed";
            try
            {
                CalendarDateSelectorManager.RenameDateRange(parms.iCDRRID, parms.sDateRangeName);
            }
            catch
            {
                returnCode = eROReturnCode.Failure;
                succ = "Failed to rename";
            }
            return new RONoDataOut(returnCode, succ + " " + parms.sDateRangeName, ROInstanceID);

        }
        private ROOut DeleteCalendarDateRange(ROCalendarSaveParms parms)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string succ = "Successfully deleted";
            try
            {
                CalendarDateSelectorManager.DeleteDateRange(parms.iCDRRID);
            }
            catch
            {
                returnCode = eROReturnCode.Failure;
                succ = "Failed to delete";
            }
            return new RONoDataOut(returnCode, succ + " " + parms.sDateRangeName, ROInstanceID);
        }

        private ROOut UpdateCalendarDateRange(ROCalendarSaveParms saveParms)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            CalendarDateRangeRowHandler rowHandler = CalendarDateRangeRowHandler.GetInstance(_dateRangeProfile, CalendarDateSelectorManager);
            DataTable dt = BuildDateRangeDataTable(rowHandler);

            try 
            {
                eCalendarDateType dateType = (eCalendarDateType) saveParms.iDateType;
                eCalendarRangeType rangeType = (eCalendarRangeType) saveParms.iDateRangeType;
                eDateRangeRelativeTo relativeTo = (eDateRangeRelativeTo) saveParms.iRelativeTo;

                if (saveParms.bSaveDateRange)
                {
                    //until we have update in the UI, the DateRangeRID will always be Include.UndefinedCalendarDateRange in this case
                    _dateRangeProfile = CalendarDateSelectorManager.SaveRangeButtonClicked(saveParms.DateRangeRID, saveParms.sDateRangeName, saveParms.iStartDate, saveParms.iEndDate,
                                                                                            dateType, rangeType, relativeTo);
                }
                else
                {
                    _dateRangeProfile = CalendarDateSelectorManager.OkButtonClicked(saveParms.DateRangeRID, saveParms.iStartDate, saveParms.iEndDate,
                                                                                    dateType, rangeType, relativeTo);
                }
                rowHandler = CalendarDateRangeRowHandler.GetInstance(_dateRangeProfile, CalendarDateSelectorManager);
                AddCalendarDateRange(dt, rowHandler);
            }
            catch(Exception exc)
            {
                returnCode = eROReturnCode.Failure;
                ROWebTools.LogMessage(eROMessageLevel.Error, "save of calendar date range failed: " + exc.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

            return new RODataTableOut(returnCode, null, ROInstanceID, dt);
        }


        public class CalendarDateRangeRowHandler : RowHandler
        {
            private static CalendarDateRangeRowHandler _Instance;

            public static CalendarDateRangeRowHandler GetInstance(DateRangeProfile DateRangeProfile, CalendarDateSelectorManager CalendarDateSelectorManager)
            {
                if (_Instance == null)
                {
                    _Instance = new CalendarDateRangeRowHandler(DateRangeProfile, CalendarDateSelectorManager);
                }
                else
                {
                    _Instance._DateRangeProfile = DateRangeProfile;
                }

                return _Instance;
            }

            private DateRangeProfile _DateRangeProfile;
            private CalendarDateSelectorManager _calendarDateSelectorManager;

            private TypedColumnHandler<int> _Key = new TypedColumnHandler<int>("Key", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDR_START = new TypedColumnHandler<int>("Starting Date", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDR_END = new TypedColumnHandler<int>("Ending Date", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDR_RELATIVE_TO = new TypedColumnHandler<int>("Date Relative To", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDT_ID = new TypedColumnHandler<int>("Date Type", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDR_RANGE_TYPE_ID = new TypedColumnHandler<int>("Range Type ID", eMIDTextCode.Unassigned, false, 100000);
            private TypedColumnHandler<string> _anchorDate = new TypedColumnHandler<string>("Anchor Date", eMIDTextCode.Unassigned, false, string.Empty);
            private TypedColumnHandler<string> _displayDate = new TypedColumnHandler<string>("Display Date", eMIDTextCode.Unassigned, false, string.Empty);
            private TypedColumnHandler<string> _name = new TypedColumnHandler<string>("Name", eMIDTextCode.Unassigned, false, string.Empty);
            private TypedColumnHandler<bool> _isDynamicSwitch = new TypedColumnHandler<bool>("Is Dynamic Switch", eMIDTextCode.Unassigned, false, false);
            private TypedColumnHandler<int> _dynamicSwitchDate = new TypedColumnHandler<int>("Dynamic Switch Date", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDR_SELECTED_START = new TypedColumnHandler<int>("Selected Start Week", eMIDTextCode.Unassigned, false, 0);
            private TypedColumnHandler<int> _CDR_SELECTED_END = new TypedColumnHandler<int>("Selected End Week", eMIDTextCode.Unassigned, false, 0);

            protected CalendarDateRangeRowHandler(DateRangeProfile DateRangeProfile, CalendarDateSelectorManager CalendarDateSelectorManager)
            {
                _DateRangeProfile = DateRangeProfile;
                _calendarDateSelectorManager = CalendarDateSelectorManager;

                _aColumnHandlers = new ColumnHandler[] { _Key, _CDR_START, _CDR_END, _CDR_RELATIVE_TO, _CDT_ID, _CDR_RANGE_TYPE_ID, _anchorDate,
            _displayDate, _name, _isDynamicSwitch, _dynamicSwitchDate, _CDR_SELECTED_START, _CDR_SELECTED_END};
            }

            public override void ParseUIRow(DataRow dr)
            {
                _DateRangeProfile.Key = _Key.ParseUIColumn(dr);
                _DateRangeProfile.StartDateKey = _CDR_START.ParseUIColumn(dr);
                _DateRangeProfile.EndDateKey = _CDR_END.ParseUIColumn(dr);
                _DateRangeProfile.RelativeTo = (eDateRangeRelativeTo)_CDR_RELATIVE_TO.ParseUIColumn(dr);
                _DateRangeProfile.SelectedDateType = (eCalendarDateType)_CDT_ID.ParseUIColumn(dr);
                _DateRangeProfile.DateRangeType = (eCalendarRangeType)_CDR_RANGE_TYPE_ID.ParseUIColumn(dr);
                _DateRangeProfile.DisplayDate = _displayDate.ParseUIColumn(dr);
                _DateRangeProfile.Name = _name.ParseUIColumn(dr);
                _DateRangeProfile.IsDynamicSwitch = _isDynamicSwitch.ParseUIColumn(dr);
                _DateRangeProfile.DynamicSwitchDate = _dynamicSwitchDate.ParseUIColumn(dr);
            }

            public override void FillUIRow(DataRow dr)
            {
                // update selected date information with new key
                WeekProfile selectedStartWeek = new WeekProfile(Include.NoRID);
                WeekProfile selectedEndWeek = new WeekProfile(Include.NoRID);
                _calendarDateSelectorManager.GetSelectedDates(_DateRangeProfile.Key, out selectedStartWeek, out selectedEndWeek);

                _Key.SetUIColumn(dr, _DateRangeProfile.Key);
                _CDR_START.SetUIColumn(dr, _DateRangeProfile.StartDateKey);
                _CDR_END.SetUIColumn(dr, _DateRangeProfile.EndDateKey);
                _CDR_RELATIVE_TO.SetUIColumn(dr, (int)_DateRangeProfile.RelativeTo);
                _CDT_ID.SetUIColumn(dr, (int)_DateRangeProfile.SelectedDateType);
                _CDR_RANGE_TYPE_ID.SetUIColumn(dr, (int)_DateRangeProfile.DateRangeType);
                _displayDate.SetUIColumn(dr, _DateRangeProfile.DisplayDate != null ? _DateRangeProfile.DisplayDate : string.Empty);
                _name.SetUIColumn(dr, _DateRangeProfile.Name != null ? _DateRangeProfile.Name : string.Empty);
                _isDynamicSwitch.SetUIColumn(dr, _DateRangeProfile.IsDynamicSwitch);
                _dynamicSwitchDate.SetUIColumn(dr, _DateRangeProfile.DynamicSwitchDate);
                _CDR_SELECTED_START.SetUIColumn(dr, selectedStartWeek.YearWeek);
                _CDR_SELECTED_END.SetUIColumn(dr, selectedEndWeek.YearWeek);
            }
        }
    }
}
