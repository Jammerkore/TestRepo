using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class PurgeData : DataLayer
	{	
		private StringBuilder _documentXML;
		private int _recordsWritten = 0;

		public PurgeData() : base()
		{
		}

		public bool DeletePurgeDates()
		{
			try
			{
                int deletedRows = StoredProcedures.MID_PURGE_DATES_DELETE_ALL.Delete(_dba);
                return (deletedRows > 0);
			}
			catch
			{
				throw;
			}
		}

		public void Purge_XMLInit()
		{
			try
			{
				_documentXML = new StringBuilder();
				// add root element
				_documentXML.Append("<root> ");
				_recordsWritten = 0;
				OpenUpdateConnection();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void Purge_XMLInsert(int aNodeRID, int aPurgeDailyHistory,
			int aPurgeWeeklyHistory, int aPurgeOTSPlans, DateTime aPurgeHeaders)
		{
			try
			{
				++_recordsWritten;
				// add node element with attributes
				_documentXML.Append(" <node HN_RID=\"");
				_documentXML.Append(aNodeRID.ToString());
				_documentXML.Append("\" DAY_HIS=\"");
				_documentXML.Append(aPurgeDailyHistory.ToString());
				_documentXML.Append("\" WK_HIS=\"");
				_documentXML.Append(aPurgeWeeklyHistory.ToString());
				_documentXML.Append("\" PLANS=\"");
				_documentXML.Append(aPurgeOTSPlans.ToString());
				_documentXML.Append("\" HEADERS=\"");
				_documentXML.Append(aPurgeHeaders.ToShortDateString());
//				_documentXML.Append(formatDateStr(aPurgeHeaders.ToShortDateString()));
				_documentXML.Append("\"> ");

				// terminate node element
				_documentXML.Append(" </node>");

				return;
			}
			catch 
			{
				throw;
			}
		}

		private string formatDateStr(String date)
		{
			string formated = null;
			string day = null;
			string month = null;
			string year = null;
			int slashIndex = 0;
			int lastIndex = 0;
			if (date.Length == 10)  // full padded date
			{
				formated = date.Substring(6,4) + "-" + date.Substring(0,2) + "-" + date.Substring(3,2);
			}
			else
			{
				slashIndex = date.IndexOf("/",slashIndex);
				if (slashIndex == 2)
				{
					month = date.Substring(0,2);
				}
				else
				{
					month = "0" + date.Substring(0,1);
				}
				lastIndex = slashIndex;
				slashIndex = date.IndexOf("/",slashIndex+1);
				if (slashIndex-1 - lastIndex == 2)
				{
					day = date.Substring(lastIndex+1,2);
				}
				else
				{
					day = "0" + date.Substring(lastIndex+1,1);
				}
				lastIndex = slashIndex;
				slashIndex = date.IndexOf("/",slashIndex+1);
				year = date.Substring(lastIndex+1,4);
				formated = year + "-" + month + "-" + day;
			}
			return formated;
		}

      

		public void BuildPurgeDates(int aHierarchyRID)
		{
			try
			{
                // Begin TT#400-MD - JSmith - Add Header Purge Criteria by Header Type

                //Begin TT#1403-MD -jsobek -Data Layer Request - Need access to stored procedure
                //int levels = (int)StoredProcedures.MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL.ReadValue(_dba, HOME_PH_RID: aHierarchyRID);
                MerchandiseHierarchyData md = new MerchandiseHierarchyData();
                int levels = md.GetHierarchyMaxNodeLevel(aHierarchyRID);
                //End TT#1403-MD -jsobek -Data Layer Request - Need access to stored procedure

                for (int level = 0; level <= levels; level++)
                {
                    StoredProcedures.SP_MID_BUILD_PURGE_DATES.Insert(_dba,
                                                                     PH_RID: aHierarchyRID,
                                                                     PHL_SEQUENCE: level
                                                                     );
                }
                // End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

      

		public DataTable DistinctDailyHistoryWeeks_Read()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public void DailyHistoryDates_Update(int aWeeks, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_DAILY_HISTORY.Update(_dba,
                                                                             PURGE_DAILY_HISTORY: aTimeID,
                                                                             PURGE_DAILY_HISTORY_WEEKS: aWeeks
                                                                             );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        
       


        public void DailyNonSizeHistoryDates_Update(int aTimeID)
        {
            try
            {
                StoredProcedures.SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD.Update(_dba, TIME_ID: aTimeID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


		public DataTable DistinctWeeklyHistoryWeeks_Read()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


        public void WeeklyHistoryDates_Update(int aWeeks, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY.Update(_dba,
                                                                              PURGE_WEEKLY_HISTORY: aTimeID,
                                                                              PURGE_WEEKLY_HISTORY_WEEKS: aWeeks
                                                                              );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

    

		public DataTable DistinctPlanWeeks_Read()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void PlanDates_Update(int aWeeks, int aTimeID)
		{
			try
			{
                StoredProcedures.MID_PURGE_DATES_UPDATE_PLAN_DATES.Update(_dba,
                                                                          PURGE_PLANS: aTimeID,
                                                                          PURGE_PLANS_WEEKS: aWeeks
                                                                          );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

  

        public DataTable DistinctReceiptHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctASNHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctDummyHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctDropShipHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctReserveHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctWorkupTotalBuyHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctPOHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable DistinctVSWHeaderWeeks_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type

        // Begin TT#5210 - JSmith - Purge Performance
        public DataTable DistinctHierarchiesWithPurgeCriteria_Read()
        {
            try
            {
                return StoredProcedures.MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#5210 - JSmith - Purge Performance

        public void HeaderReceiptDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_RECEIPT_DATES.Update(_dba,
                                                                             PURGE_HEADERS_RECEIPT_DATETIME: aDateTime,
                                                                             PURGE_HEADERS_RECEIPT: aTimeID,
                                                                             PURGE_HEADERS_WEEKS_RECEIPT: aWeeks
                                                                             );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderASNDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_ASN_DATES.Update(_dba,
                                                                         PURGE_HEADERS_ASN_DATETIME: aDateTime,
                                                                         PURGE_HEADERS_ASN: aTimeID,
                                                                         PURGE_HEADERS_WEEKS_ASN: aWeeks
                                                                         );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderDummyDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_DUMMY_DATES.Update(_dba,
                                                                           PURGE_HEADERS_DUMMY_DATETIME: aDateTime,
                                                                           PURGE_HEADERS_DUMMY: aTimeID,
                                                                           PURGE_HEADERS_WEEKS_DUMMY: aWeeks
                                                                           );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderDropShipDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_DROPSHIP_DATES.Update(_dba,
                                                                              PURGE_HEADERS_DROPSHIP_DATETIME: aDateTime,
                                                                              PURGE_HEADERS_DROPSHIP: aTimeID,
                                                                              PURGE_HEADERS_WEEKS_DROPSHIP: aWeeks
                                                                              );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderReserveDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_RESERVE_DATES.Update(_dba,
                                                                             PURGE_HEADERS_RESERVE_DATETIME: aDateTime,
                                                                             PURGE_HEADERS_RESERVE: aTimeID,
                                                                             PURGE_HEADERS_WEEKS_RESERVE: aWeeks
                                                                             );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderWorkupTotalBuyDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES.Update(_dba,
                                                                                    PURGE_HEADERS_WORKUPTOTALBUY_DATETIME: aDateTime,
                                                                                    PURGE_HEADERS_WORKUPTOTALBUY: aTimeID,
                                                                                    PURGE_HEADERS_WEEKS_WORKUPTOTALBUY: aWeeks
                                                                                    );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderPODates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_PO_DATES.Update(_dba,
                                                                        PURGE_HEADERS_PO_DATETIME: aDateTime,
                                                                        PURGE_HEADERS_PO: aTimeID,
                                                                        PURGE_HEADERS_WEEKS_PO: aWeeks
                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void HeaderVSWDates_Update(int aWeeks, DateTime aDateTime, int aTimeID)
        {
            try
            {
                StoredProcedures.MID_PURGE_DATES_UPDATE_VSW_DATES.Update(_dba,
                                                                         PURGE_HEADERS_VSW_DATETIME: aDateTime,
                                                                         PURGE_HEADERS_VSW: aTimeID,
                                                                         PURGE_HEADERS_WEEKS_VSW: aWeeks
                                                                         );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
        // End TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages


        // Begin TT#3822 - JSmith - Add Stop Time to Purge
        //public int Purge_Audit(int aAuditDays, int aCommitLimit)
        public int Purge_Audit(int aAuditDays, int aCommitLimit, DateTime aShutdownTime, out bool aAutomaticStopExceeded)
        // End TT#3822 - JSmith - Add Stop Time to Purge
		{
			int recordsDeleted;
            int totalDeleted = 0;
            aAutomaticStopExceeded = false;  // TT#3822 - JSmith - Add Stop Time to Purge
			try
			{ 
				recordsDeleted = 1;
				while (recordsDeleted > 0)
				{
				    // Begin TT#3822 - JSmith - Add Stop Time to Purge
                    if (DateTime.Now.TimeOfDay > aShutdownTime.TimeOfDay)
                    {
                        aAutomaticStopExceeded = true;
						break;
                    }
                    // End TT#3822 - JSmith - Add Stop Time to Purge
                    recordsDeleted = StoredProcedures.SP_MID_PURGE_AUDIT.Delete(_dba,
                                                               PURGE_DAYS: aAuditDays,
                                                               COMMIT_LIMIT: aCommitLimit
                                                               );
                    //recordsDeleted = (int)StoredProcedures.SP_MID_PURGE_AUDIT.RECORDS_DELETED.Value;
					_dba.CommitData();
                    totalDeleted += recordsDeleted;
				}
                return totalDeleted;

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable PurgeDates_ReadDailyHistoryDates()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public DataTable PurgeDates_ReadWeeklyHistoryDates()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

   

		public DataTable PurgeDates_ReadPlanDates()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable PurgeDates_ReadAuditForecasts()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_AUDIT_FORECAST.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable PurgeDates_ReadAuditModifiedSales()
		{
			try
			{
                return StoredProcedures.MID_PURGE_DATES_READ_MODIFIED_SALES.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
        }
        // Begin TT#2170 - JSmith - Daily Size Purge Not Working
        public void Perform_Onetime_Purge()
        {
            try
            {
                OpenUpdateConnection();
                StoredProcedures.SP_MID_ONETIME_PURGE.Delete(_dba);
                StoredProcedures.MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE.Update(_dba, PERFORM_ONETIME_SPECIAL_PURGE_IND: '0');
                _dba.CommitData();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    CloseUpdateConnection();
                }
            }
        }
        // End TT#2170

        ////BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        //public DataTable PurgeDates_ReadDailyPercentages(int current_week)
        //{
        //    try
        //    {
        //        string SQLCommand = "SELECT dp.CDR_RID FROM DAILY_PERCENTAGES dp with (nolock) " +
        //            " INNER JOIN CALENDAR_DATE_RANGE cdr on af.HN_RID = pd.HN_RID " +
        //            " inner join dbo.FISCAL_WEEKS fw ON cdr.CDR_RID = dp.CDR_RID and cdr.CDR_END < " + current_week;

        //        //string SQLCommand = "SELECT AUDIT_FORECAST_RID FROM AUDIT_FORECAST af with (nolock) " +
        //        //    " inner join PURGE_DATES pd on af.HN_RID = pd.HN_RID " +
        //        //    " inner join dbo.FISCAL_WEEKS fw on af.TIME_RANGE_END = fw.FISCAL_WEEK " +
        //        //    " where af.METHOD_TYPE = " + (int)eMethodType.ForecastModifySales +
        //        //    "   and fw.LAST_DAY_OF_WEEK <= pd.PURGE_WEEKLY_HISTORY";

        //        return _dba.ExecuteSQLQuery(SQLCommand, "READ DAILY PERCENTAGES");
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        ////END TT#43 - MD - DOConnell - Projected Sales Enhancement
	}
}
