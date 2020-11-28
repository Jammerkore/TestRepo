using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class CalendarData : DataLayer
	{
		private DBAdapter _daCalendarModel;
		private DBAdapter _daCalendarModelPeriods;

		public CalendarData() : base()
		{

		}


		public DataTable CalendarModel_Read()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_CALENDAR_MODEL_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable CalendarModelPeriods_Read(int cm_rid, eCalendarModelPeriodType periodType)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE.Read(_dba,
                                                                                              CM_RID: cm_rid,
                                                                                              CMP_TYPE: (int)periodType
                                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataSet CalendarModel_ReadForMaintenance(int CM_RID)
		{
            DataSet ds = MIDEnvironment.CreateDataSet("CALENDAR_MODEL");
            _daCalendarModel = _dba.NewDBAdapter("CALENDAR_MODEL");
            // Select
            string selectCommand = "select CM_RID, CM_ID, START_DATE, FISCAL_YEAR ";
            // begin MID Track # 2354 - removed nolock because it causes concurrency issues
            selectCommand += "FROM CALENDAR_MODEL WHERE CM_RID = " + CM_RID.ToString(CultureInfo.CurrentUICulture); 
            // end MID Track # 2354
            _daCalendarModel.SelectCommand(selectCommand);
			// Insert	
            MIDDbParameter[] inParams  = { new MIDDbParameter("@CM_ID", eDbType.VarChar, 50, "CM_ID"),
                                          new MIDDbParameter("@START_DATE", eDbType.DateTime, 0, "START_DATE"),
                                          new MIDDbParameter("@FISCAL_YEAR", eDbType.Float, 0, "FISCAL_YEAR") };	  
									
            for (int i=0;i<3;i++)
            {
                inParams[i].Direction = eParameterDirection.Input;
            }

            MIDDbParameter[] outParams = { new MIDDbParameter("@CM_RID", eDbType.Int, 0, "@CM_RID") };
            outParams[0].Direction = eParameterDirection.Output;
            _daCalendarModel.InsertCommand("SP_MID_CALENDARMODEL_INSERT", inParams, outParams);
            // Update
            string updateCommand = "UPDATE CALENDAR_MODEL SET CM_ID = @CM_ID, START_DATE = @START_DATE, " +
                " FISCAL_YEAR = @FISCAL_YEAR " +
                "WHERE CM_RID = @CM_RID";
            MIDDbParameter[] inParams2  = { new MIDDbParameter("@CM_ID", eDbType.VarChar, 50, "CM_ID"),
                                           new MIDDbParameter("@START_DATE", eDbType.DateTime, 0, "START_DATE"),
                                           new MIDDbParameter("@FISCAL_YEAR", eDbType.Float, 0, "FISCAL_YEAR"),
                                           new MIDDbParameter("@CM_RID", eDbType.Int, 0, "CM_RID")};	  
									
            for (int i=0;i<4;i++)
            {
                inParams2[i].Direction = eParameterDirection.Input;
            }
            _daCalendarModel.UpdateCommand(updateCommand, inParams2);
            // Delete
            string deleteCommand = "DELETE FROM CALENDAR_MODEL WHERE SM_RID = @SM_RID";
            MIDDbParameter[] inParams3  = { new MIDDbParameter("@CM_RID", eDbType.Int, 0, "CM_RID") };	  
            inParams3[0].Direction = eParameterDirection.Input;
            _daCalendarModel.DeleteCommand(deleteCommand, inParams3);


            _daCalendarModel.Fill( ds );

            return ds;
            //return StoredProcedures.MID_CALENDAR_MODEL_READ.ReadAsDataSet(_dba, CM_RID: CM_RID);
     
		}

		public void CalendarModel_UpdateRowsInTable(DataTable xDataTable)
		{
			try
			{
				_daCalendarModel.UpdateTable(xDataTable);
                //foreach (DataRow dr in xDataTable.Rows)
                //{
                //    int CM_RID = (int)dr["CM_RID"];
                //    string CM_ID = (string)dr["CM_ID"];
                //    DateTime START_DATE = (DateTime)dr["START_DATE"];
                //    int FISCAL_YEAR = (int)dr["FISCAL_YEAR"];
                //    StoredProcedures.MID_CALENDAR_MODEL_UPDATE.Update(_dba,
                //                                                      CM_RID: CM_RID,
                //                                                      CM_ID: CM_ID,
                //                                                      START_DATE: START_DATE,
                //                                                      FISCAL_YEAR: FISCAL_YEAR
                //                                                      );
                //}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

		}

		public DataTable CalendarModel_ReadKey(string modelName, int fiscalYear)
		{
			try
			{
                    //MID Track # 2354 - removed nolock because it causes concurrency issues
                    return StoredProcedures.MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR.Read(_dba, CM_ID: modelName, FISCAL_YEAR: fiscalYear);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public void CalendarModel_Delete(int cm_rid) //TT#846-MD -jsobek -New Stored Procedures for Performance
		{
			try
			{
                    StoredProcedures.MID_CALENDAR_MODEL_DELETE.Delete(_dba, CM_RID: cm_rid);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataSet CalendarModelPeriods_ReadForMaintenance(int CM_RID)
		{
            DataSet ds = MIDEnvironment.CreateDataSet("CALENDAR_MODEL_PERIODS");
            _daCalendarModelPeriods = _dba.NewDBAdapter("CALENDAR_MODEL_PERIODS");
            // Select
            string selectCommand = "select CM_RID, CMP_SEQUENCE, CMP_ID, CMP_ABBREVIATION, NO_OF_TIME_PERIODS, CMP_TYPE ";
            // begin MID Track # 2354 - removed nolock because it causes concurrency issues
            selectCommand += "FROM CALENDAR_MODEL_PERIODS WHERE CM_RID = " + CM_RID.ToString(CultureInfo.CurrentUICulture);
            // end MID Track # 2354
            selectCommand += " ORDER BY CMP_TYPE, CMP_SEQUENCE";
            _daCalendarModelPeriods.SelectCommand(selectCommand);
            // Insert	
            string insertCommand = "INSERT INTO CALENDAR_MODEL_PERIODS(CM_RID, CMP_SEQUENCE, " +
                " CMP_ID, CMP_ABBREVIATION, NO_OF_TIME_PERIODS, CMP_TYPE) " +
                " VALUES (@CM_RID, @CMP_SEQUENCE, @CMP_ID, @CMP_ABBREVIATION, @NO_OF_TIME_PERIODS, @CMP_TYPE)";

            MIDDbParameter[] inParams = { new MIDDbParameter("@CM_RID", eDbType.Single, 0, "CM_RID"),
                                          new MIDDbParameter("@CMP_SEQUENCE", eDbType.Single, 0, "CMP_SEQUENCE"),
                                          new MIDDbParameter("@CMP_ID", eDbType.VarChar, 50, "CMP_ID"),
                                          new MIDDbParameter("@CMP_ABBREVIATION", eDbType.VarChar, 10, "CMP_ABBREVIATION"),
                                          new MIDDbParameter("@NO_OF_TIME_PERIODS", eDbType.Single, 0, "NO_OF_TIME_PERIODS"),
                                          new MIDDbParameter("@CMP_TYPE", eDbType.Int, 0, "CMP_TYPE") 
                                         };
            for (int i = 0; i < 6; i++)
            {
                inParams[i].Direction = eParameterDirection.Input;
            }
            _daCalendarModelPeriods.InsertCommand(insertCommand, inParams);
            // Update
            string updateCommand = "UPDATE CALENDAR_MODEL_PERIODS SET CMP_ID = @CMP_ID, " +
                " CMP_ABBREVIATION = @CMP_ABBREVIATION, NO_OF_TIME_PERIODS = @NO_OF_TIME_PERIODS " +
                "WHERE CM_RID = @CM_RID AND CMP_SEQUENCE = @CMP_SEQUENCE AND CMP_TYPE = @CMP_TYPE";
            MIDDbParameter[] inParams2 = {  new MIDDbParameter("@CMP_ID", eDbType.VarChar, 50, "CMP_ID"),
                                          new MIDDbParameter("@CMP_ABBREVIATION", eDbType.VarChar, 10, "CMP_ABBREVIATION"),
                                          new MIDDbParameter("@NO_OF_TIME_PERIODS", eDbType.Single, 0, "NO_OF_TIME_PERIODS"),
                                          new MIDDbParameter("@CM_RID", eDbType.Single, 0, "CM_RID"),
                                          new MIDDbParameter("@CMP_SEQUENCE", eDbType.Single, 0, "CMP_SEQUENCE"),
                                          new MIDDbParameter("@CMP_TYPE", eDbType.Single, 0, "CMP_TYPE") 
                                          };

            for (int i = 0; i < 6; i++)
            {
                inParams2[i].Direction = eParameterDirection.Input;
            }
            _daCalendarModelPeriods.UpdateCommand(updateCommand, inParams2);
            // Delete
            string deleteCommand = "DELETE FROM CALENDAR_MODEL_PERIODS WHERE CM_RID = @CM_RID AND CMP_SEQUENCE = @CMP_SEQUENCE AND CMP_TYPE = @CMP_TYPE";
            MIDDbParameter[] inParams3 = { new MIDDbParameter("@CM_RID", eDbType.Int, 0, "CM_RID"),
                                         new MIDDbParameter("@CMP_SEQUENCE", eDbType.Single, 0, "CMP_SEQUENCE"),
                                         new MIDDbParameter("@CMP_TYPE", eDbType.Single, 0, "CMP_TYPE")
                                          };
            inParams3[0].Direction = eParameterDirection.Input;
            inParams3[1].Direction = eParameterDirection.Input;
            inParams3[2].Direction = eParameterDirection.Input;
            _daCalendarModelPeriods.DeleteCommand(deleteCommand, inParams3);

            _daCalendarModelPeriods.Fill(ds);

            return ds;
            //return StoredProcedures.MID_CALENDAR_MODEL_PERIODS_READ.ReadAsDataSet(_dba, CM_RID: CM_RID);
		}

		public void CalendarModelPeriods_UpdateRowsInTable(DataTable xDataTable)
		{
			try
			{

				_daCalendarModelPeriods.UpdateTable(xDataTable);
                //foreach (DataRow dr in xDataTable.Rows)
                //{
                //    int CM_RID = (int)dr["CM_RID"];
                //    int CMP_SEQUENCE = (int)dr["CMP_SEQUENCE"];
                //    int CMP_TYPE = (int)dr["CMP_TYPE"];
                //    string CMP_ID = (string)dr["CMP_ID"];
                //    string CMP_ABBREVIATION = (string)dr["CMP_ABBREVIATION"];
                //    int NO_OF_TIME_PERIODS = (int)dr["NO_OF_TIME_PERIODS"];
                //    StoredProcedures.MID_CALENDAR_MODEL_PERIODS_UPDATE.Update(_dba,
                //                                                              CM_RID: CM_RID,
                //                                                              CMP_SEQUENCE: CMP_SEQUENCE,
                //                                                              CMP_TYPE: CMP_TYPE,
                //                                                              CMP_ID: CMP_ID,
                //                                                              CMP_ABBREVIATION: CMP_ABBREVIATION,
                //                                                              NO_OF_TIME_PERIODS: NO_OF_TIME_PERIODS
                //                                                              );
                //}



			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public void CalendarModelPeriods_Delete(int cm_rid) 
		{
			try
			{
                    StoredProcedures.MID_CALENDAR_MODEL_PERIODS_DELETE.Delete(_dba, CM_RID: cm_rid);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable CalendarDateRange_Read(int cdr_rid)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_CALENDAR_DATE_RANGE_READ.Read(_dba, CDR_RID: cdr_rid);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable CalendarDateRange_ReadForNames()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues          
                return StoredProcedures.MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int CalendarDateRange_Insert(int cdr_start, int cdr_end, int cdr_range_type, 
			int cdt_id, int cdr_relative_to, string cdr_name, bool isDynamicSwitchDate, int dynamicSwitchDate) //Issue 5171
		{
			try
			{
				char dynamicSwitch = Include.ConvertBoolToChar(isDynamicSwitchDate);
				int cdr_rid = -1;

                if (cdr_name == null)
                {
                    cdr_name = string.Empty;
                }
                
                cdr_rid = StoredProcedures.SP_MID_CALENDARDTRANGE_INSERT.InsertAndReturnRID(_dba, 
                                                                                            CDR_START: cdr_start,
                                                                                            CDR_END: cdr_end,
                                                                                            CDR_RANGE_TYPE_ID: cdr_range_type,
                                                                                            CDR_DATE_TYPE_ID: cdt_id,
                                                                                            CDR_RELATIVE_TO: cdr_relative_to,
                                                                                            CDR_NAME: cdr_name,
                                                                                            CDR_DYNAMIC_SWITCH: dynamicSwitch,
                                                                                            CDR_DYNAMIC_SWITCH_DATE: dynamicSwitchDate
                                                                                            );
				return cdr_rid;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void CalendarDateRange_Update(int cdr_rid, int cdr_start, int cdr_end, int cdr_range_type, int cdt_id, 
			int cdr_relative_to, string cdr_name, bool isDynamicSwitchDate, int dynamicSwitchDate)
		{
			try
			{
                if (cdr_name == null)
                {
                    cdr_name = string.Empty;
                }

                StoredProcedures.MID_CALENDAR_DATE_RANGE_UPDATE.Update(_dba,
                                                                       CDR_RID: cdr_rid,
                                                                       CDR_START: cdr_start,
                                                                       CDR_END: cdr_end,
                                                                       CDR_RANGE_TYPE_ID: cdr_range_type,
                                                                       CDR_DATE_TYPE_ID: cdt_id,
                                                                       CDR_RELATIVE_TO: cdr_relative_to,
                                                                       CDR_NAME: cdr_name,
                                                                       CDR_DYNAMIC_SWITCH: Include.ConvertBoolToChar(isDynamicSwitchDate),
                                                                       CDR_DYNAMIC_SWITCH_DATE: dynamicSwitchDate
                                                                       );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void CalendarDateRange_UpdateName(int cdr_rid, string cdr_name)
		{
			try
			{
                if (cdr_name == null)
                {
                    cdr_name = string.Empty;
                }

                StoredProcedures.MID_CALENDAR_DATE_RANGE_UPDATE_NAME.Update(_dba, 
                                                                            CDR_RID: cdr_rid,
                                                                            CDR_NAME: cdr_name
                                                                            );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public void CalendarDateRange_Delete(int cdr_rid)  //TT#846-MD -jsobek -New Stored Procedures for Performance
		{
			try
			{
                StoredProcedures.MID_CALENDAR_DATE_RANGE_DELETE.Delete(_dba, CDR_RID: cdr_rid);
			}
			catch ( Exception err )
			{
//				throw new MIDException(eErrorLevel.warning,0,"Requested Calendar Date Range could not be deleted." +
//					" It is being used elsewhere in the system.");
				string message = err.ToString();
				throw new Exception("Delete Failed");
			}
		}

		public DataTable CalendarWeek53Year_Read()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_CALENDAR_WEEK53_YEAR_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// returns all week 53 records for the desired calendar model rid
		/// </summary>
		/// <param name="cm_rid">int</param>
		/// <returns>DataTable</returns>
		public DataTable CalendarWeek53Year_Read(int cm_rid)
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_CALENDAR_WEEK53_YEAR_READ.Read(_dba, CM_RID: cm_rid, CMP_TYPE: (int)eCalendarModelPeriodType.Month);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public void CalendarWeek53Year_Delete(int cm_rid) //TT#846-MD -jsobek -New Stored Procedures for Performance
		{
			try
			{
                StoredProcedures.MID_CALENDAR_WEEK53_YEAR_DELETE.Delete(_dba, CM_RID: cm_rid);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public void CalendarWeek53Year_Delete(int year, int cm_rid, int sequence) 
		{
			try
			{
                StoredProcedures.MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ.Delete(_dba, 
                                                                                          CM_RID: cm_rid,
                                                                                          WEEK53_FISCAL_YEAR: year,
                                                                                          CMP_SEQUENCE: sequence
                                                                                          );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void CalendarWeek53Year_Insert(int week53_fiscal_year, int cm_rid, int cmp_sequence, DataCommon.eWeek53Offset eOffset)
		{
			try
			{
				int offset = (int)eOffset;
                StoredProcedures.SP_MID_CALENDARWEEK53_INSERT.Insert(_dba, 
                                                                     WEEK53_FISCAL_YEAR: week53_fiscal_year,
                                                                     CM_RID: cm_rid,
                                                                     CMP_SEQUENCE: cmp_sequence,
                                                                     OFFSET_ID: offset,
                                                                     CMP_TYPE: (int)eCalendarModelPeriodType.Month
                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void CalendarWeek53Year_Update(int fiscalYear, int modelKey, int periodSeq, DataCommon.eWeek53Offset eOffset)
		{
			try
			{
                StoredProcedures.MID_CALENDAR_WEEK53_YEAR_UPDATE.Update(_dba, 
                                                                        WEEK53_FISCAL_YEAR: fiscalYear,
                                                                        CM_RID: modelKey,
                                                                        CMP_SEQUENCE: periodSeq,
                                                                        OFFSET_ID: (int)eOffset
                                                                        );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable PostingDate_Read()
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_POSTING_DATE_RID_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

     
		public void DeleteAllFiscalWeeks()
		{
			try
			{
                StoredProcedures.MID_FISCAL_WEEKS_DELETE_ALL.Delete(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        
        public void AddFiscalWeek(int aRowSequence, int aFiscalWeek, int aFirstDayOfWeek, int aLastDayOfWeek, int aFirstDayOfWeekOffset, int aLastDayOfWeekOffset)
        {
            try
            {
                StoredProcedures.MID_FISCAL_WEEKS_INSERT.Insert(_dba, 
                                                                ROW_SEQUENCE: aRowSequence,
                                                                FISCAL_WEEK: aFiscalWeek,
                                                                FIRST_DAY_OF_WEEK: aFirstDayOfWeek,
                                                                LAST_DAY_OF_WEEK: aLastDayOfWeek,
                                                                FIRST_DAY_OF_WEEK_OFFSET: aFirstDayOfWeekOffset,
                                                                LAST_DAY_OF_WEEK_OFFSET: aLastDayOfWeekOffset
                                                                );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#988
		// END MID Track #4373

        // Begin TT#1277 - JSmith - Remove old calendar date ranges
        public void PurgeOldCalendarDateRanges()
        {
            try
            {
                StoredProcedures.SP_MID_DELETE_DATE_RANGES.Delete(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1277
	}
}
