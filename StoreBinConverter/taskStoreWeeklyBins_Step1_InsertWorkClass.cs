using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace StoreBinConverter
{
    public class taskStoreWeeklyBins_Step1_InsertWorkClass
    {
        private SubTaskSelectControl _task;
        private SqlCommand _cmd;
        private int[] _storeRIDs;
        private SqlBulkCopy bulkCopyWeek;
        private string _databaseConnectionString;
        private long totalWeeklyRows = 0;
        //private taskCommon.ReadBinDataAndWriteToTempWorkTableDelegate _readBinDataAndWriteToTempWorkTable;
        private int _numberOfStoreHistoryTables;
        private SessionAddressBlock _SAB;

        public taskStoreWeeklyBins_Step1_InsertWorkClass(SubTaskSelectControl task, 
            SqlCommand cmd, 
            string databaseConnectionString,
            int[] storeRIDs,
            //taskCommon.ReadBinDataAndWriteToTempWorkTableDelegate readBinDataAndWriteToTempWorkTable,
            int numberOfStoreHistoryTables,
            SessionAddressBlock SAB
            )
        {
            _task = task;
            _cmd = cmd;
            _databaseConnectionString = databaseConnectionString;
            _storeRIDs = storeRIDs;
           // _readBinDataAndWriteToTempWorkTable = readBinDataAndWriteToTempWorkTable;
            _numberOfStoreHistoryTables = numberOfStoreHistoryTables;
            _SAB = SAB;
        }

        private bool _formClosing;
        public void HandleFormClosing()
        {
            _formClosing = true;
        }


        //long _totalRowsCopied = 0;
        //int _updateUIcounter = 0;
        //private void bulkCopy_SqlRowsCopied(int rowsCopied)
        //{
        //    _totalRowsCopied += rowsCopied;
        //    _updateUIcounter++;
        //    if ((_updateUIcounter % 50) == 0)  //update the screen every once in awhile
        //    {
        //        _updateUIcounter = 0;


        //        _task.txtResult.Text = "Rows Processed: " + _totalRowsCopied.ToString("###,###,###,###,###,#00");

        //        int curval = (int)(_totalRowsCopied / 1000);
        //        if (curval < _task.ultraProgressBar1.Maximum)
        //            _task.ultraProgressBar1.Value = curval;
        //        else
        //            _task.ultraProgressBar1.Value = _task.ultraProgressBar1.Maximum;

        //        TimeSpan ts = System.DateTime.Now.Subtract(_task.startTime);

        //        double percentleft = 100 - (_task.ultraProgressBar1.Value / _task.ultraProgressBar1.Maximum);
        //        double minutesLeft = ts.TotalMinutes * percentleft;
        //       // this.lblEstTime.Text = (minutesLeft / 60).ToString("##00") + ":" + (minutesLeft % 60).ToString("00") + ":00";


        //        _task.UpdateTime(ts);
        //        //RaiseUpdateTotalTimeEvent();
        //    }
        //}


        public void InsertWork()
        {
            try
            {
                _task.startTime = System.DateTime.Now;
                _task.UpdateStatus("Initializing");
                _task.UpdateResult("Obtaining Count - Wait up to 5 minutes.");



                // Get the total row count (each row contains data for all the stores which we have to expand out)
                _cmd.CommandText = "SELECT COUNT(*) FROM STORE_WEEK_HISTORY_BIN";
                _cmd.CommandType = CommandType.Text;

                _task.UpdateTime();

                totalWeeklyRows = -1;
                object totWeeklyRows = _cmd.ExecuteScalar();
                if (totWeeklyRows != null && totWeeklyRows != DBNull.Value)
                {
                    string stemp = totWeeklyRows.ToString();
                    long.TryParse(stemp, out totalWeeklyRows);
                }

                if (totalWeeklyRows == -1)
                {
                    _task.UpdateStatus("Failed");
                    _task.UpdateTime();
                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - Error: Could not obtain total row count.");
                    return;
                }
                _task.UpdateTime();

                if (totalWeeklyRows == 0)
                {
                    _task.UpdateLog(System.Environment.NewLine + "Zero store week bin rows to process");
                    _task.UpdateStatus("Complete");
                    _task.UpdateTime();
                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - completed.");
                    return;
                }

                //Create Work Table
                _task.UpdateStatus("Creating work table.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STORE_HISTORY_WORK_WEEK]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[STORE_HISTORY_WORK_WEEK]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                _cmd.CommandText = MakeWorkTableSQL();
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                _task.UpdateTime();

                //Create Upsert Stored Procedure
                _task.UpdateStatus("Creating work procedure.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CONVERT_STORE_WEEKLY_BINS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[CONVERT_STORE_WEEKLY_BINS]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                _cmd.CommandText = MakeWorkProcedureSQL(_numberOfStoreHistoryTables);
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                _task.UpdateTime();




                // Get the minimum time
                _task.UpdateStatus("Obtaining minimum time.");


                int mimimumTime = 0;
                _cmd.CommandText = "SELECT [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME](MIN([TIME_ID])) FROM STORE_WEEK_HISTORY_BIN";
                _cmd.CommandType = CommandType.Text;
                object minTime = _cmd.ExecuteScalar();
                if (minTime != null && minTime != DBNull.Value)
                {
                    string stemp = minTime.ToString();
                    int.TryParse(stemp, out mimimumTime);
                }

                if (mimimumTime == 0)
                {
                    _task.UpdateStatus("Failed");
                    _task.UpdateTime();
                    _task.UpdateLog(_task.ultraCheckEditor1.Text + " - Error: Could not obtain minimum time.");
                    return;
                }
                _task.UpdateTime();


                // Get the maximum time
                _task.UpdateStatus("Obtaining maximum time.");


                int maximumTime = 0;
                _cmd.CommandText = "SELECT [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME](MAX([TIME_ID])) FROM STORE_WEEK_HISTORY_BIN";
                _cmd.CommandType = CommandType.Text;
                object maxTime = _cmd.ExecuteScalar();
                if (maxTime != null && maxTime != DBNull.Value)
                {
                    string stemp = maxTime.ToString();
                    int.TryParse(stemp, out maximumTime);
                }

                if (maximumTime == 0)
                {
                    _task.UpdateStatus("Failed");
                    _task.UpdateTime();
                    _task.UpdateLog(_task.ultraCheckEditor1.Text + " - Error: Could not obtain maximum time.");
                    return;
                }
                _task.UpdateTime();

                

                // Create a datatable that corresponds to the temporary work table
                DataTable dtWork = new DataTable("STORE_HISTORY_WORK_WEEK");
                dtWork.Columns.Add("HN_RID", typeof(int));
                dtWork.Columns.Add("JULIAN_TIME_ID", typeof(int));
                dtWork.Columns.Add("ST_RID", typeof(int));
                dtWork.Columns.Add("SALES", typeof(int));
                dtWork.Columns.Add("SALES_REG", typeof(int));
                dtWork.Columns.Add("SALES_PROMO", typeof(int));
                dtWork.Columns.Add("SALES_MKDN", typeof(int));
                dtWork.Columns.Add("STOCK", typeof(int));
                dtWork.Columns.Add("STOCK_REG", typeof(int));
                dtWork.Columns.Add("STOCK_MKDN", typeof(int));
                dtWork.Columns.Add("IN_STOCK_SALES", typeof(int));
                dtWork.Columns.Add("IN_STOCK_SALES_REG", typeof(int));
                dtWork.Columns.Add("IN_STOCK_SALES_PROMO", typeof(int));
                dtWork.Columns.Add("IN_STOCK_SALES_MKDN", typeof(int));
                dtWork.Columns.Add("ACCUM_SELL_THRU_SALES", typeof(int));
                dtWork.Columns.Add("ACCUM_SELL_THRU_STOCK", typeof(int));
                dtWork.Columns.Add("DAYS_IN_STOCK", typeof(int));
                dtWork.Columns.Add("RECEIVED_STOCK", typeof(int));





           

                //Set the progress bar maximum
                long totalRowsToProcess = totalWeeklyRows * _storeRIDs.LongLength;
                _task.ultraProgressBar1.Maximum = (int)(totalRowsToProcess / 1000) + 1000; //adding an extra 1000 for rounding

                _task.UpdateLog(System.Environment.NewLine + "Processing " + totalRowsToProcess.ToString("###,###,###,###,###,###,##0") + " total store week bin rows.");

                _task.UpdateStatus("Processing");


                // Setup the SqlBulkCopy
                bulkCopyWeek = new SqlBulkCopy(_databaseConnectionString, SqlBulkCopyOptions.TableLock);
                int idealBatchSizeFactor = (5000 / _storeRIDs.Length) + 1; //we want to process about 5000 rows at a time
                bulkCopyWeek.BatchSize = (_storeRIDs.Length * idealBatchSizeFactor); // 5000;
                bulkCopyWeek.NotifyAfter = 0; // _storeRIDs.Length; // 1000;
                //bulkCopyWeek.SqlRowsCopied += new SqlRowsCopiedEventHandler(bulkCopy_SqlRowsCopied);
                bulkCopyWeek.DestinationTableName = "STORE_HISTORY_WORK_WEEK";

                ReadBinDataAndWriteToTempWorkTable(mimimumTime, maximumTime, dtWork, ref bulkCopyWeek);
                if (!_formClosing)
                {
                    // Now that the rows are in the temporary work table - use the work table and update/insert into the weekly history normalized tables via a stored procedure call
                    _task.UpdateStatus("Updating");

                    _cmd.CommandText = "[dbo].[CONVERT_STORE_WEEKLY_BINS]";
                    _cmd.CommandType = CommandType.StoredProcedure;
                    _cmd.ExecuteNonQuery();
                    _task.UpdateTime();
                }



                if (!_formClosing)
                {
                    _task.UpdateStatus("Complete");
                    _task.UpdateTime();

                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - completed.");
                }
            }
            catch (Exception ex)
            {
                if (!_formClosing)
                {
                    _task.UpdateStatus("Failed");
                    _task.UpdateTime();
                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - Error: " + ex.ToString());
                }
            }

        } //end InsertWork

        public void ReadBinDataAndWriteToTempWorkTable(int from, int to, DataTable dtWork, ref SqlBulkCopy bulkcopy)
        {

            int drpRid = Include.NoRID;

            int styleRid = -1;
            int colorCodeRid = -1;
            int sizeCodeRid = -1;


            string varName = string.Empty;


            ArrayList varKeyList;
            ArrayList timeKeyList;
            MerchandiseHierarchyData nodeData = new MerchandiseHierarchyData();
            HierarchyLevelProfile styleLevelProf = null;
            HierarchyLevelProfile colorLevelProf = null;
            StoreVariableHistoryBin _dlStoreVarHist = new StoreVariableHistoryBin(false, 0);





            //=======================================================================================
            // Get Main Hierarchy information and specifically info for the Style and Color levels.
            //=======================================================================================
            HierarchyProfile mainHier = _SAB.HierarchyServerSession.GetMainHierarchyData();
            for (int levelIndex = 1; levelIndex <= mainHier.HierarchyLevels.Count; levelIndex++)
            {
                HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHier.HierarchyLevels[levelIndex];
                //hlp.LevelID is level name 
                //hlp.Level is level number 
                //hlp.LevelType is level type 
                if (hlp.LevelType == eHierarchyLevelType.Style)
                {
                    styleLevelProf = hlp;
                }
                if (hlp.LevelType == eHierarchyLevelType.Color)
                {
                    colorLevelProf = hlp;
                }
            }



           
            _task.UpdateResult("Obtaining times.");
            _task.UpdateTime();
    
            _task.UpdateTotalTime();

            //==========================
            // gather up weeklist from 
            //==========================
            DateRangeProfile overrideDateRange = new DateRangeProfile(Include.NoRID);
            //if (sDate.Length == 13)
            //{
            //    try
            //    {
            //string[] sep = new string[] { "-" };
            //string[] fromTo = sDate.Split(sep, StringSplitOptions.None);
            //int from = int.Parse(fromTo[0]);
            //int to = int.Parse(fromTo[1]);
            //cdrRid = _SAB.ApplicationServerSession.Calendar.AddDateRange(from, to, eCalendarRangeType.Static, eCalendarDateType.Week, eDateRangeRelativeTo.None, "", false, 0);
            overrideDateRange.StartDateKey = from;
            overrideDateRange.EndDateKey = to;
            overrideDateRange.DateRangeType = eCalendarRangeType.Static;
            
                overrideDateRange.SelectedDateType = eCalendarDateType.Week;
        

            overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;
            //    }
            //    catch
            //    {
            //        throw;
            //    }
            //}
            //================================================
            // Date format "YYYYWW" for a static week
            //================================================
            //if (sDate.Length == 6)
            //{
            //    try
            //    {
            //        int fromTo = int.Parse(sDate);
            //        overrideDateRange.StartDateKey = fromTo;
            //        overrideDateRange.EndDateKey = fromTo;
            //        overrideDateRange.DateRangeType = eCalendarRangeType.Static;
            //        overrideDateRange.SelectedDateType = eCalendarDateType.Week;
            //        overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;

            //    }
            //    catch
            //    {
            //        throw;
            //    }

            //}
            drpRid = _SAB.ApplicationServerSession.Calendar.AddDateRange(overrideDateRange);

            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(drpRid);
            ProfileList weekList = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);



           
            _task.UpdateResult("Obtaining styles.");
            _task.UpdateTime();

            _task.UpdateTotalTime();

            //==============
            // Get Styles
            //==============
            DataTable dtAllStyles = null;
            //if (txtNode.Text.Trim() == string.Empty)
            //{
            dtAllStyles = nodeData.GetAllStyles();
            //}
            //else
            //{
            //    _nodeRid = _SAB.HierarchyServerSession.GetNodeRID(txtNode.Text);
            //    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(_nodeRid);
            //    dtAllStyles = BuildAllStylesTable();

            //    if (hnp.LevelType == eHierarchyLevelType.Style)
            //    {
            //        DataRow aRow = dtAllStyles.NewRow();
            //        aRow[0] = _nodeRid;
            //        dtAllStyles.Rows.Add(aRow);

            //    }
            //    else
            //    {
            //        NodeDescendantList nodeDescList = _SAB.HierarchyServerSession.GetNodeDescendantList(_nodeRid, eHierarchyLevelType.Style, eNodeSelectType.All);
            //        foreach (NodeDescendantProfile ndp in nodeDescList.ArrayList)
            //        {
            //            object[] objs = new object[] { ndp.Key.ToString() };

            //            dtAllStyles.LoadDataRow(objs, false);
            //        }
            //    }
            //}
            //dtAllStyles.AcceptChanges();


            varKeyList = new ArrayList();
            DataTable dtVar = MIDText.GetLabels((int)eForecastBaseDatabaseStoreVariables.SalesTotal, (int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
            foreach (DataRow aRow in dtVar.Rows)
            {
                string textValue = aRow["TEXT_VALUE"].ToString();
                varKeyList.Add(textValue);
            }


            
            _task.UpdateResult("Preparing to process rows.");
            _task.UpdateTime();

            _task.UpdateTotalTime();

            int storeCount = _storeRIDs.Length;

            foreach (DataRow row in dtAllStyles.Rows) // by STYLE
            {
                styleRid = int.Parse(row["HN_RID"].ToString());
                HierarchyNodeList colorNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(styleLevelProf.Level, mainHier.Key, mainHier.Key, styleRid, false, eNodeSelectType.NoVirtual);

                foreach (HierarchyNodeProfile colorNode in colorNodeList.ArrayList) // by COLOR
                {
                    colorCodeRid = colorNode.ColorOrSizeCodeRID;

                    foreach (WeekProfile weekProf in weekList) // TIME (each week)
                    {
                        timeKeyList = new ArrayList();
                        
                        timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsWeekly, weekProf.Key));
                        

                        HierarchyNodeList sizeNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(colorLevelProf.Level, mainHier.Key, mainHier.Key, colorNode.Key, false, eNodeSelectType.NoVirtual);


                        foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList) // by SIZES
                        {
                            sizeCodeRid = sizeNode.ColorOrSizeCodeRID;
                            int sizeRID = sizeNode.Key;
                            double[] valueList = null;

                            for (int t = 0; t < timeKeyList.Count; t++) // by TIME
                            {
                                SQL_TimeID timeID = (SQL_TimeID)timeKeyList[t];
                                dtWork.Rows.Clear();



                                //We are adding one row for each store for this size and time
                                for (int i = 0; i < storeCount; i++) // by STORE
                                {
                                    DataRow newRow = dtWork.NewRow();
                                    newRow["HN_RID"] = sizeRID;
                                    newRow["JULIAN_TIME_ID"] = timeID.TimeID;
                                    newRow["ST_RID"] = _storeRIDs[i];
                                    dtWork.Rows.Add(newRow);
                                }

                                for (int v = 0; v < varKeyList.Count; v++) // by VARIABLE
                                {
                                    varName = varKeyList[v].ToString();
                                    
                                    valueList = _dlStoreVarHist.GetStoreVariableWeekValue(varName, styleRid, timeID, colorCodeRid, sizeCodeRid, _storeRIDs); // get values by STORE
                                    


                                    for (int i = 0; i < storeCount; i++)
                                    {
                                        dtWork.Rows[i][varName] = valueList[i];
                                    }

                                } // end by VARIABLE

                                //remove rows that contain all zeros, so they will not be inserted on the database
                                for (int i = storeCount - 1; i >= 0; i--)
                                {
                                    DataRow storeRow = dtWork.Rows[i];
                                    bool nonZeros = false;
                                    int v = 0;
                                    while (nonZeros == false && v < varKeyList.Count)
                                    {
                                        varName = varKeyList[v].ToString();
                                        if ((int)storeRow[varName] != 0)
                                        {
                                            nonZeros = true;
                                        }
                                        v++;
                                    }
                                    if (nonZeros == false) //remove this row
                                    {
                                        dtWork.Rows.RemoveAt(i);
                                    }
                                }

                                bulkcopy.WriteToServer(dtWork);
                                _task.bulkCopy_SqlRowsCopied(storeCount);

                                if (_formClosing)
                                {
                                    return;
                                }

                            } // end by TIME
                        } // end by SIZES
                    } // end TIME (each week)
                } // end by COLOR
            } // end by STYLE



        }
 

        private string MakeWorkTableSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE [dbo].[STORE_HISTORY_WORK_WEEK](");
            sb.AppendLine("[HN_RID] [int] NOT NULL,");
            sb.AppendLine("[JULIAN_TIME_ID] [int] NOT NULL,");
            sb.AppendLine("[ST_RID] [int] NOT NULL,");
            sb.AppendLine("[SALES] [int] NULL,");
            sb.AppendLine("[SALES_REG] [int] NULL,");
            sb.AppendLine("[SALES_PROMO] [int] NULL,");
            sb.AppendLine("[SALES_MKDN] [int] NULL,");
            sb.AppendLine("[STOCK] [int] NULL,");
            sb.AppendLine("[STOCK_REG] [int] NULL,");
            sb.AppendLine("[STOCK_MKDN] [int] NULL,");
            sb.AppendLine("[IN_STOCK_SALES] [int] NULL,");
            sb.AppendLine("[IN_STOCK_SALES_REG] [int] NULL,");
            sb.AppendLine("[IN_STOCK_SALES_PROMO] [int] NULL,");
            sb.AppendLine("[IN_STOCK_SALES_MKDN] [int] NULL,");
            sb.AppendLine("[ACCUM_SELL_THRU_SALES] [int] NULL,");
            sb.AppendLine("[ACCUM_SELL_THRU_STOCK] [int] NULL,");
            sb.AppendLine("[DAYS_IN_STOCK] [int] NULL,");
            sb.AppendLine("[RECEIVED_STOCK] [int] NULL");
            sb.AppendLine(") ON [PRIMARY]");

            return sb.ToString();
        }

        private string MakeWorkProcedureSQL(int numberOfHistoryTables)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-- =============================================");
            sb.AppendLine("-- Description:	Moves data from work table");
            sb.AppendLine("-- =============================================");
            sb.AppendLine("CREATE PROCEDURE [dbo].[CONVERT_STORE_WEEKLY_BINS] ");
            sb.AppendLine("AS ");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("");
            sb.AppendLine("DECLARE @TIME_TO_EXECUTE_PROCESS_STEP DATETIME ");
            sb.AppendLine("DECLARE @TIME_TO_EXECUTE DATETIME ");
            sb.AppendLine("DECLARE @PROCESS_SUMMARY TABLE	");
            sb.AppendLine("( ");
            sb.AppendLine("	PROCESS_STEP varchar(500), ");
            sb.AppendLine("	PROCESS_STEP_TIME_TO_EXECUTE int, ");
            sb.AppendLine("	PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS int,");
            sb.AppendLine("	PROCESS_STEP_DESCRIPTION varchar(500),");
            sb.AppendLine("	PROCESS_STEP_RESULTS varchar(500) ");
            sb.AppendLine(") ");
            sb.AppendLine("");
            sb.AppendLine("--Begin Process Step 1: Get data from work tables");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("");
            sb.AppendLine("DECLARE @SUMMARY_VIEW TABLE	");
            sb.AppendLine("(");
            sb.AppendLine("	HN_RID int, ");
            sb.AppendLine("	TIME_ID int,  ");
            sb.AppendLine("	ST_RID int, ");
            sb.AppendLine("	HN_MOD int,");
            sb.AppendLine("");
            sb.AppendLine("	--values to update in weekly tables");
            sb.AppendLine("	INSERT_OR_UPDATE_FLAG char(1), -- I to Insert, U to Update");
            sb.AppendLine("	SALES int,");
            sb.AppendLine("	SALES_REG int,");
            sb.AppendLine("	SALES_PROMO int,");
            sb.AppendLine("	SALES_MKDN int,");
            sb.AppendLine("	STOCK int,");
            sb.AppendLine("	STOCK_REG int,");
            sb.AppendLine("	STOCK_MKDN int,");
            sb.AppendLine("	IN_STOCK_SALES int,");
            sb.AppendLine("	IN_STOCK_SALES_REG int,");
            sb.AppendLine("	IN_STOCK_SALES_PROMO int,");
            sb.AppendLine("	IN_STOCK_SALES_MKDN int,");
            sb.AppendLine("	ACCUM_SELL_THRU_SALES int,");
            sb.AppendLine("	ACCUM_SELL_THRU_STOCK int,");
            sb.AppendLine("	DAYS_IN_STOCK int,");
            sb.AppendLine("	RECEIVED_STOCK int, --received stock during the week");
            sb.AppendLine("");
            sb.AppendLine("	PRIMARY KEY (HN_RID, ST_RID, TIME_ID)");
            sb.AppendLine(")");
            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @SUMMARY_VIEW ");
            sb.AppendLine(" SELECT	DISTINCT		");
            sb.AppendLine("		wd.HN_RID, ");
            sb.AppendLine("		JULIAN_TIME_ID AS TIME_ID,");
            sb.AppendLine("		ST_RID, ");
            sb.AppendLine("		wd.HN_RID % 10 AS HN_MOD,");
            sb.AppendLine("");
            sb.AppendLine("		'I' AS INSERT_OR_UPDATE_FLAG, -- I to Insert, U to Update, setting default to insert");
            sb.AppendLine("		SALES,");
            sb.AppendLine("		SALES_REG,");
            sb.AppendLine("		SALES_PROMO,");
            sb.AppendLine("		SALES_MKDN,");
            sb.AppendLine("		STOCK,");
            sb.AppendLine("		STOCK_REG,");
            sb.AppendLine("		STOCK_MKDN,");
            sb.AppendLine("");
            sb.AppendLine("		IN_STOCK_SALES,");
            sb.AppendLine("		IN_STOCK_SALES_REG,");
            sb.AppendLine("		IN_STOCK_SALES_PROMO,");
            sb.AppendLine("		IN_STOCK_SALES_MKDN,");
            sb.AppendLine("		ACCUM_SELL_THRU_SALES,");
            sb.AppendLine("		ACCUM_SELL_THRU_STOCK,");
            sb.AppendLine("		DAYS_IN_STOCK,");
            sb.AppendLine("		RECEIVED_STOCK");
            sb.AppendLine("	");
            sb.AppendLine("FROM [dbo].[STORE_HISTORY_WORK_WEEK]  wd ");
            sb.AppendLine("INNER JOIN [dbo].[HIERARCHY_NODE] hn on hn.HN_RID=wd.HN_RID ");
            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 1: Get data from work tables',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Row Count:',(SELECT COUNT(*) FROM @SUMMARY_VIEW) ");
            sb.AppendLine("--End Process Step 1: Get data from work tables");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("--Begin Process Step 2: Determine which rows to update");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("DECLARE @UPDATED_ROWS INT ");
            sb.AppendLine("DECLARE @TOTAL_UPDATED_ROWS INT ");
            sb.AppendLine("SET @TOTAL_UPDATED_ROWS = 0 ");
            sb.AppendLine("--Determine which rows to update and which rows to insert  (By default, the flag is set to insert)");
            sb.AppendLine("SET @TIME_TO_EXECUTE = getDate();");
            sb.AppendLine("");

            for (int i = 0; i < numberOfHistoryTables; i++)
            {
                sb.AppendLine("UPDATE sv ");
                sb.AppendLine("SET sv.INSERT_OR_UPDATE_FLAG='U' ");
                sb.AppendLine("FROM @SUMMARY_VIEW sv ");
                sb.AppendLine("INNER JOIN [dbo].[STORE_HISTORY_WEEK" + i.ToString() + "] d ON sv.HN_MOD=" + i.ToString() + " AND d.HN_RID=sv.HN_RID AND d.TIME_ID=sv.TIME_ID AND d.ST_RID=sv.ST_RID ");
                sb.AppendLine("");
            }

            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 2: Determine which rows to update',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,null, null ");
            sb.AppendLine("--End Process Step 2: Determine which rows to update");
            sb.AppendLine("");
            sb.AppendLine("--Begin Process Step 3: Update weekly history tables");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");

            for (int i = 0; i < numberOfHistoryTables; i++)
            {
                sb.AppendLine("SET @TIME_TO_EXECUTE = getDate();");
                sb.AppendLine("UPDATE d ");
                sb.AppendLine("SET ");
                sb.AppendLine("d.SALES = sv.SALES,");
                sb.AppendLine("d.SALES_REG = sv.SALES_REG,");
                sb.AppendLine("d.SALES_PROMO = sv.SALES_PROMO,");
                sb.AppendLine("d.SALES_MKDN = sv.SALES_MKDN,");
                sb.AppendLine("d.STOCK = sv.STOCK,");
                sb.AppendLine("d.STOCK_REG = sv.STOCK_REG,");
                sb.AppendLine("d.STOCK_MKDN = sv.STOCK_MKDN,");
                sb.AppendLine("d.IN_STOCK_SALES = sv.IN_STOCK_SALES,");
                sb.AppendLine("d.IN_STOCK_SALES_REG = sv.IN_STOCK_SALES_REG,");
                sb.AppendLine("d.IN_STOCK_SALES_PROMO = sv.IN_STOCK_SALES_PROMO,");
                sb.AppendLine("d.IN_STOCK_SALES_MKDN = sv.IN_STOCK_SALES_MKDN,");
                sb.AppendLine("d.ACCUM_SELL_THRU_SALES = sv.ACCUM_SELL_THRU_SALES,");
                sb.AppendLine("d.ACCUM_SELL_THRU_STOCK = sv.ACCUM_SELL_THRU_STOCK,");
                sb.AppendLine("d.DAYS_IN_STOCK = sv.DAYS_IN_STOCK,");
                sb.AppendLine("d.RECEIVED_STOCK = sv.RECEIVED_STOCK ");
                sb.AppendLine("FROM [dbo].[STORE_HISTORY_WEEK" + i.ToString() + "] d ");
                sb.AppendLine("INNER JOIN @SUMMARY_VIEW sv ON d.HN_RID=sv.HN_RID AND d.TIME_ID=sv.TIME_ID AND d.ST_RID=sv.ST_RID ");
                sb.AppendLine("WHERE sv.HN_MOD=" + i.ToString() + " AND sv.INSERT_OR_UPDATE_FLAG='U' ");
                sb.AppendLine("SET @UPDATED_ROWS = @@ROWCOUNT ");
                sb.AppendLine("SET @TOTAL_UPDATED_ROWS = @TOTAL_UPDATED_ROWS + @UPDATED_ROWS ");
                sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 3: Update weekly history tables - Table " + i.ToString() + "',0,DATEDIFF(millisecond, @TIME_TO_EXECUTE, getDate()),'Updated Rows:', (SELECT @UPDATED_ROWS) ");
                sb.AppendLine("");
            }

            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 3: Update weekly history tables',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Total Updated Rows:', (SELECT @TOTAL_UPDATED_ROWS) ");
            sb.AppendLine("--End Process Step 3: Update weekly history tables");

            sb.AppendLine("--Begin Process Step 4: Add rows to weekly history tables");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("DECLARE @INSERTED_ROWS INT");
            sb.AppendLine("DECLARE @TOTAL_INSERTED_ROWS INT");
            sb.AppendLine("SET @TOTAL_INSERTED_ROWS=0");
            sb.AppendLine("");

            for (int i = 0; i < numberOfHistoryTables; i++)
            {

                sb.AppendLine("SET @TIME_TO_EXECUTE = getDate();");
                sb.AppendLine("INSERT INTO [dbo].[STORE_HISTORY_WEEK" + i.ToString() + "] ");
                sb.AppendLine("(");
                sb.AppendLine("	HN_MOD,");
                sb.AppendLine("	HN_RID,");
                sb.AppendLine("	TIME_ID,");
                sb.AppendLine("	ST_RID,");
                sb.AppendLine("	SALES,");
                sb.AppendLine("	SALES_REG,");
                sb.AppendLine("	SALES_PROMO,");
                sb.AppendLine("	SALES_MKDN,");
                sb.AppendLine("	STOCK,");
                sb.AppendLine("	STOCK_REG,");
                sb.AppendLine("	STOCK_MKDN,");
                sb.AppendLine("	IN_STOCK_SALES,");
                sb.AppendLine("	IN_STOCK_SALES_REG,");
                sb.AppendLine("	IN_STOCK_SALES_PROMO,");
                sb.AppendLine("	IN_STOCK_SALES_MKDN,");
                sb.AppendLine("	ACCUM_SELL_THRU_SALES,");
                sb.AppendLine("	ACCUM_SELL_THRU_STOCK,");
                sb.AppendLine("	DAYS_IN_STOCK,");
                sb.AppendLine("	RECEIVED_STOCK");
                sb.AppendLine(") ");
                sb.AppendLine("SELECT ");
                sb.AppendLine("	HN_MOD,");
                sb.AppendLine("	HN_RID,");
                sb.AppendLine("	TIME_ID,");
                sb.AppendLine("	ST_RID,");
                sb.AppendLine("	SALES,");
                sb.AppendLine("	SALES_REG,");
                sb.AppendLine("	SALES_PROMO,");
                sb.AppendLine("	SALES_MKDN,");
                sb.AppendLine("	STOCK,");
                sb.AppendLine("	STOCK_REG,");
                sb.AppendLine("	STOCK_MKDN,");
                sb.AppendLine("	IN_STOCK_SALES,");
                sb.AppendLine("	IN_STOCK_SALES_REG,");
                sb.AppendLine("	IN_STOCK_SALES_PROMO,");
                sb.AppendLine("	IN_STOCK_SALES_MKDN,");
                sb.AppendLine("	ACCUM_SELL_THRU_SALES,");
                sb.AppendLine("	ACCUM_SELL_THRU_STOCK,");
                sb.AppendLine("	DAYS_IN_STOCK,");
                sb.AppendLine("	RECEIVED_STOCK ");
                sb.AppendLine("FROM ");
                sb.AppendLine("@SUMMARY_VIEW sv ");
                sb.AppendLine("WHERE sv.HN_MOD=" + i.ToString() + " AND sv.INSERT_OR_UPDATE_FLAG='I' ");
                sb.AppendLine("SET @INSERTED_ROWS = @@ROWCOUNT ");
                sb.AppendLine("SET @TOTAL_INSERTED_ROWS = @TOTAL_INSERTED_ROWS + @INSERTED_ROWS ");
                sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 4: Add rows to weekly history tables - Table " + i.ToString() + "',0,DATEDIFF(millisecond, @TIME_TO_EXECUTE, getDate()),'Inserted Rows:', (SELECT @INSERTED_ROWS) ");
                sb.AppendLine("");
            }

            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 4: Add rows to weekly history tables',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Total Inserted Rows:', (SELECT @TOTAL_INSERTED_ROWS)");
            sb.AppendLine("--End Process Step 4: Add rows to weekly history tables");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("SELECT * FROM @PROCESS_SUMMARY");
            sb.AppendLine("");
            sb.AppendLine("END");

            return sb.ToString();
        } //end MakeWorkProcedureSQL

    }
}
