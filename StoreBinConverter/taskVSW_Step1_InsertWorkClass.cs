using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace StoreBinConverter
{
    public class taskVSW_Step1_InsertWorkClass
    {
        private SubTaskSelectControl _task;
        private SqlCommand _cmd;
        private int[] _storeRIDs;
        private SqlBulkCopy bulkCopyVSW;
        private string _databaseConnectionString;
        private long totalVSWRows = 0;
     

        public taskVSW_Step1_InsertWorkClass(SubTaskSelectControl task, 
            SqlCommand cmd, 
            string databaseConnectionString,
            int[] storeRIDs
            )
        {
            _task = task;
            _cmd = cmd;
            _storeRIDs = storeRIDs;
            _databaseConnectionString = databaseConnectionString;
        }

        private bool _formClosing;
        public void HandleFormClosing()
        {
            _formClosing = true;
        }

        public void InsertWork()
        {
            try
            {
                _task.startTime = System.DateTime.Now;
                _task.UpdateStatus("Initializing");
                _task.UpdateResult("Obtaining Count - Wait up to 5 minutes.");



                // Get the total row count 
                _cmd.CommandText = "SELECT COUNT(*) FROM VSW_REVERSE_ONHAND";
                _cmd.CommandType = CommandType.Text;


                _task.UpdateTime();

                totalVSWRows = -1;
                object totVSWRows = _cmd.ExecuteScalar();
                if (totVSWRows != null && totVSWRows != DBNull.Value)
                {
                    string stemp = totVSWRows.ToString();
                    long.TryParse(stemp, out totalVSWRows);
                }

                if (totalVSWRows == -1)
                {
                    _task.UpdateStatus("Failed");
                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - Error: Could not obtain total row count.");
                    return;
                }
                _task.UpdateTime();

                if (totalVSWRows == 0)
                {
                    _task.UpdateLog(System.Environment.NewLine + "Zero VSW Reverse On Hand rows to process");
                    _task.UpdateStatus("Complete");
                    _task.UpdateTime();
                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - completed.");
                    return;
                }


                //Create Work Table
                _task.UpdateStatus("Creating work table.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VSW_REVERSE_ON_HAND_WORK]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[VSW_REVERSE_ON_HAND_WORK]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                _cmd.CommandText = MakeWorkTableSQL();
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                _task.UpdateTime();

                //Create Upsert Stored Procedure
                _task.UpdateStatus("Creating work procedure.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CONVERT_VSW_BINS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[CONVERT_VSW_BINS]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                _cmd.CommandText = MakeWorkProcedureSQL();
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                _task.UpdateTime();

                
                 // Create a datatable that corresponds to the temporary work table
                DataTable dtWork = new DataTable("VSW_REVERSE_ON_HAND_WORK");
                dtWork.Columns.Add("HDR_RID", typeof(int));
                dtWork.Columns.Add("HN_RID", typeof(int));
                dtWork.Columns.Add("ST_RID", typeof(int));
                dtWork.Columns.Add("USTAT", typeof(int));
                dtWork.Columns.Add("VSW_REVERSE_ON_HAND_UNITS", typeof(int));
              

                //Set the progress bar maximum
                long totalRowsToProcess = totalVSWRows * _storeRIDs.LongLength;
                _task.ultraProgressBar1.Maximum = (int)(totalRowsToProcess / 1000) + 1000; //adding an extra 1000 for rounding

                _task.UpdateLog(System.Environment.NewLine + "Processing " + totalRowsToProcess.ToString("###,###,###,###,###,###,##0") + " total VSW Reverse On Hand rows.");



                _cmd.CommandText = "SELECT HDR_RID FROM VSW_REVERSE_ONHAND";
                _cmd.CommandType = CommandType.Text;
                SqlDataAdapter sda = new SqlDataAdapter(_cmd);
                DataTable dtHeaderRIDS = new DataTable("HEADER_RIDS");
                sda.Fill(dtHeaderRIDS);




                _task.UpdateStatus("Processing");


                // Setup the SqlBulkCopy
                bulkCopyVSW = new SqlBulkCopy(_databaseConnectionString, SqlBulkCopyOptions.TableLock);
                int idealBatchSizeFactor = (5000 / _storeRIDs.Length) + 1; //we want to process about 5000 rows at a time
                bulkCopyVSW.BatchSize = (_storeRIDs.Length * idealBatchSizeFactor); // 5000;
                bulkCopyVSW.NotifyAfter = 0;  //_storeRIDs.Length; // 1000;
                //bulkCopyVSW.SqlRowsCopied += new SqlRowsCopiedEventHandler(_task.bulkCopy_SqlRowsCopied);
                bulkCopyVSW.DestinationTableName = "VSW_REVERSE_ON_HAND_WORK";



                StoreVswReverseOnhand _storeVswReverseOnhand = new StoreVswReverseOnhand();


                Header header = new Header();

                int headerCount = 0;
                foreach (DataRow drHeaderRID in dtHeaderRIDS.Rows)
                {
                    int aHdrRID = (int)drHeaderRID["HDR_RID"];
                    //ReadBinDataAndWriteToTempWorkTable(false, mimimumTime, maximumTime, dtWork, bulkCopy);
                    VswReverseOnhandDatabaseBinKey vrodbk = new VswReverseOnhandDatabaseBinKey(aHdrRID);  // Key for ALL HnRIDs associated with this header
                    List<StoreVariableData<VswReverseOnhandDatabaseBinKey>> vswReverseOnhandList = new List<StoreVariableData<VswReverseOnhandDatabaseBinKey>>();

                    //vswReverseOnhandList = _storeVswReverseOnhand.GetVswReverseOnhandDatabaseBin().ReadStoreVariables(header._dba, vrodbk);

                    if (vswReverseOnhandList.Count > 0)
                    {
                        foreach (StoreVariableData<VswReverseOnhandDatabaseBinKey> svd in vswReverseOnhandList)
                        {
                            VswReverseOnhandDatabaseBinKey tempKey = svd.DatabaseBinKey;
                            for (int i = 0; i < _storeRIDs.Length; i++)
                            {
                                DataRow newRow = dtWork.NewRow();
                                newRow["HDR_RID"] = aHdrRID;
                                newRow["HN_RID"] = tempKey.HnRID;
                                newRow["ST_RID"] = _storeRIDs[i];
                                newRow["USTAT"] = svd.Status;
                                newRow["VSW_REVERSE_ON_HAND_UNITS"] = svd.StoreVariableVectorContainer.GetStoreVariableValue(_storeRIDs[i], 0);
                                dtWork.Rows.Add(newRow);
                            }

                        }
                    }
                    headerCount++;
                    if (headerCount == 5000)
                    {
                        bulkCopyVSW.WriteToServer(dtWork);
                        _task.bulkCopy_SqlRowsCopied(headerCount);
                        headerCount = 0;
                    }

                    if (_formClosing)
                    {
                        return;
                    }
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
                    _task.UpdateLog(System.Environment.NewLine + _task.ultraCheckEditor1.Text + " - Error: " + ex.ToString());
                }
            }

        } //end InsertWork

        private string MakeWorkTableSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE [dbo].[VSW_REVERSE_ON_HAND_WORK](");
	        sb.AppendLine("[HDR_RID] [int] NOT NULL,");
	        sb.AppendLine("[HN_RID] [int] NOT NULL,");
	        sb.AppendLine("[ST_RID] [int] NOT NULL,");
	        sb.AppendLine("[USTAT] [int] NOT NULL,");
	        sb.AppendLine("[VSW_REVERSE_ON_HAND_UNITS] [int] NULL");
            sb.AppendLine(") ON [PRIMARY]");

            return sb.ToString();
        }

        private string MakeWorkProcedureSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-- =============================================");
            sb.AppendLine("-- Description:	Moves data from work table");
            sb.AppendLine("-- =============================================");
            sb.AppendLine("CREATE PROCEDURE [dbo].[CONVERT_VSW_BINS] ");
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
            sb.AppendLine("--Begin Process Step 1: Get data from VSW Reverse On Hand work table");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("");
            sb.AppendLine("DECLARE @SUMMARY_VIEW TABLE	");
            sb.AppendLine("(");
            sb.AppendLine("	HDR_RID int, ");
            sb.AppendLine("	HN_RID int,  ");
            sb.AppendLine("	ST_RID int, ");
            sb.AppendLine("");
            sb.AppendLine("	--values to update in VSW Reverse On Hand table");
            sb.AppendLine("	INSERT_OR_UPDATE_FLAG char(1), -- I to Insert, U to Update");
            sb.AppendLine("	USTAT int,");
            sb.AppendLine("	VSW_REVERSE_ON_HAND_UNITS int");
   
            sb.AppendLine("");
            sb.AppendLine("	PRIMARY KEY (HDR_RID, HN_RID, ST_RID)");
            sb.AppendLine(")");
            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @SUMMARY_VIEW ");
            sb.AppendLine(" SELECT		");
            sb.AppendLine("		wt.HDR_RID, ");
            sb.AppendLine("		wt.HN_RID, ");
            sb.AppendLine("		wt.ST_RID, ");
            sb.AppendLine("");
            sb.AppendLine("		'I' AS INSERT_OR_UPDATE_FLAG, -- I to Insert, U to Update, setting default to insert");
            sb.AppendLine("		wt.USTAT,");

            sb.AppendLine("		wt.VSW_REVERSE_ON_HAND_UNITS ");
            sb.AppendLine("	");
            sb.AppendLine("FROM [dbo].[VSW_REVERSE_ON_HAND_WORK]  wt ");
            sb.AppendLine("INNER JOIN [dbo].[HIERARCHY_NODE] hn on hn.HN_RID=wt.HN_RID ");
            sb.AppendLine("INNER JOIN [dbo].[HEADER] hd on hd.HDR_RID=wt.HDR_RID ");
            sb.AppendLine("GROUP BY wt.HDR_RID, wt.HN_RID, wt.ST_RID ");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 1: Get data from VSW Reverse On Hand work table',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Row Count:',(SELECT COUNT(*) FROM @SUMMARY_VIEW) ");
            sb.AppendLine("--End Process Step 1: Get data from VSW Reverse On Hand work table");
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

          
            sb.AppendLine("UPDATE sv ");
            sb.AppendLine("SET sv.INSERT_OR_UPDATE_FLAG='U' ");
            sb.AppendLine("FROM @SUMMARY_VIEW sv ");
            sb.AppendLine("INNER JOIN [dbo].[VSW_REVERSE_ON_HAND] vsw ON vsw.HDR_RID=sv.HDR_RID AND vsw.HN_RID=sv.HN_RID AND vsw.ST_RID=sv.ST_RID ");
            sb.AppendLine("");
            

            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 2: Determine which rows to update',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,null, null ");
            sb.AppendLine("--End Process Step 2: Determine which rows to update");
            sb.AppendLine("");
            sb.AppendLine("--Begin Process Step 3: Update VSW Reverse On Hand table");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("UPDATE vsw ");
            sb.AppendLine("SET ");
            sb.AppendLine("vsw.USTAT = sv.USTAT,");
            sb.AppendLine("vsw.VSW_REVERSE_ON_HAND_UNITS = sv.VSW_REVERSE_ON_HAND_UNITS ");
            sb.AppendLine("FROM [dbo].[VSW_REVERSE_ON_HAND] vsw ");
            sb.AppendLine("INNER JOIN @SUMMARY_VIEW sv ON vsw.HDR_RID=sv.HDR_RID AND vsw.HN_RID=sv.HN_RID AND vsw.ST_RID=sv.ST_RID ");
            sb.AppendLine("WHERE sv.INSERT_OR_UPDATE_FLAG='U' ");
            sb.AppendLine("SET @UPDATED_ROWS = @@ROWCOUNT ");
            sb.AppendLine("SET @TOTAL_UPDATED_ROWS = @TOTAL_UPDATED_ROWS + @UPDATED_ROWS ");
        
            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 3: Update VSW Reverse On Hand table',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Total Updated Rows:', (SELECT @TOTAL_UPDATED_ROWS) ");
            sb.AppendLine("--End Process Step 3: Update VSW Reverse On Hand table");

            sb.AppendLine("--Begin Process Step 4: Add rows to VSW Reverse On Hand table");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("DECLARE @INSERTED_ROWS INT");
            sb.AppendLine("DECLARE @TOTAL_INSERTED_ROWS INT");
            sb.AppendLine("SET @TOTAL_INSERTED_ROWS=0");
            sb.AppendLine("");

            sb.AppendLine("INSERT INTO [dbo].[VSW_REVERSE_ON_HAND] ");
            sb.AppendLine("(");
            sb.AppendLine("	HDR_RID,");
            sb.AppendLine("	HN_RID,");
            sb.AppendLine("	ST_RID,");
            sb.AppendLine("	USTAT,");
            sb.AppendLine("	VSW_REVERSE_ON_HAND_UNITS ");
            sb.AppendLine(") ");
            sb.AppendLine("SELECT ");
            sb.AppendLine("	HDR_RID,");
            sb.AppendLine("	HN_RID,");
            sb.AppendLine("	ST_RID,");
            sb.AppendLine("	USTAT,");
            sb.AppendLine("	VSW_REVERSE_ON_HAND_UNITS ");
            sb.AppendLine("FROM ");
            sb.AppendLine("@SUMMARY_VIEW sv ");
            sb.AppendLine("WHERE sv.INSERT_OR_UPDATE_FLAG='I' ");
            sb.AppendLine("SET @INSERTED_ROWS = @@ROWCOUNT ");
            sb.AppendLine("SET @TOTAL_INSERTED_ROWS = @TOTAL_INSERTED_ROWS + @INSERTED_ROWS ");

            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 4: Add rows to VSW Reverse On Hand table',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Total Inserted Rows:', (SELECT @TOTAL_INSERTED_ROWS)");
            sb.AppendLine("--End Process Step 4: Add rows to VSW Reverse On Hand table");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("SELECT * FROM @PROCESS_SUMMARY");
            sb.AppendLine("");
            sb.AppendLine("END");

            return sb.ToString();
        }

    }
}
