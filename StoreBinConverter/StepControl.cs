using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace StoreBinConverter
{
    public partial class StepControl : UserControl
    {

        public StepControl()
        {
            InitializeComponent();
        }

        public bool ShowProgressBar 
        { 
            get 
            { 
                return this.ultraProgressBar1.Visible; 
            } 
            set 
            {
                this.lblProgress.Visible = !value;
                this.ultraProgressBar1.Visible = value; 
            } 
        }
  
        public string DefaultStatus { get { return this.lblStatus.Text; } set { this.lblStatus.Text = value; } }
        public bool ShowCheckBox 
        { 
            get 
            { 
                return this.ultraCheckEditor1.Visible; 
            } 
            set 
            {
                this.lblStep.Visible = !value;
                this.ultraCheckEditor1.Visible = value; 
            } 
        }

        public string StepText { get { return this.lblStep.Text; } set { this.ultraCheckEditor1.Text = value;  this.lblStep.Text = value; } }
        public Infragistics.Win.HAlign StepTextHAlign { get { return this.lblStep.Appearance.TextHAlign; } set { this.lblStep.Appearance.TextHAlign = value; } }
        public Infragistics.Win.DefaultableBoolean StepTextBold { get { return this.lblStep.Appearance.FontData.Bold; } set { this.lblStep.Appearance.FontData.Bold = value; } }

        private int progressBarValue
        {

            set
            {
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.ultraProgressBar1.Value = value;
                        });
                    }
                    else
                    {
                        this.ultraProgressBar1.Value = value;
                    }
                }
                catch
                {
                }
            }
        }

        private int progressBarMax
        {

            set
            {
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.ultraProgressBar1.Maximum = value;
                        });
                    }
                    else
                    {
                        this.ultraProgressBar1.Maximum = value;
                    }
                }
                catch
                {
                }
            }
        }

        private DateTime _startTime;
        private SqlCommand _cmd;
        private SqlConnection _databaseCommandConnection;

        private long _totalRowsToRead;
        private string _taskName;
        private string _stepName;
        private string _binTableName;  //STORE_DAY_HISTORY_BIN or STORE_WEEK_HISTORY_BIN
        private string _workTableName; //STORE_HISTORY_WORK_DAY[x] or STORE_HISTORY_WORK_WEEK[x]
        private string _workProcedureName; //CONVERT_STORE_DAILY_BINS[x] or CONVERT_STORE_WEEKLY_BINS[x]
        private string _normalizedTableName; //STORE_HISTORY_DAY[x] or STORE_HISTORY_WEEK[x]

        private long _totalRowsRead = 0;
        private long _totalRowsCopied = 0;
        //private int _updateUIcounter = 0;

        private SqlBulkCopy _bulkCopy;

        private DataTable _dtWork;

        private SessionAddressBlock _SAB;
        private TaskCommon.TaskType _taskType;
        private int[] _storeRIDs;
        private string _databaseConnectionString;
       
        private int _numberOfHistoryTables;
        private ProfileList _weekList;
        private TaskCommon.UpdateLogDelegate _UpdateLog;
        private int _currentTable;
        private ArrayList _varKeyList;

        public void SetParameters(TaskCommon.stepParameters p, int currentTable)
        {
            this._taskName = p.taskName;
            this._stepName = p.stepName;
            this._SAB = p.SAB;
            this._taskType = p.taskType;
            this._storeRIDs = p.storeRIDs;
            this._databaseConnectionString = p.databaseConnectionString;
            this._numberOfHistoryTables = p.numberOfHistoryTables;
            //this.minimumTime = p.minimumTime;
            //this.maximumTime = p.maximumTime;
            this._weekList = p.weekList;
            this._UpdateLog = p.UpdateLog;
            this._currentTable = currentTable;
            this._varKeyList = p.varKeyList;
        }

        public void Step1_InsertToWorkTable_VSW()
        {
            try
            {
                if (DatabaseConnection_Initialize() == false) return;
                UpdateStatusToStarting();


                // Get the total row count 
                _cmd.CommandText = "SELECT COUNT(*) FROM VSW_REVERSE_ONHAND";
                _cmd.CommandType = CommandType.Text;
                this.UpdateTime();
                _totalRowsToRead = -1;
                object totVSWRows = _cmd.ExecuteScalar();
                if (totVSWRows != null && totVSWRows != DBNull.Value)
                {
                    string stemp = totVSWRows.ToString();
                    long.TryParse(stemp, out _totalRowsToRead);
                }

                if (_totalRowsToRead == -1)
                {
                    this.UpdateStatus("Failed");
                    this.UpdateLog(System.Environment.NewLine + this.ultraCheckEditor1.Text + " - Error: Could not obtain total row count.");
                    return;
                }
                this.UpdateTime();

                if (_totalRowsToRead == 0)
                {
                    this.UpdateLog(System.Environment.NewLine + "Zero VSW Reverse On Hand rows to process");
                    this.UpdateStatus("Complete");
                    this.UpdateTime();
                    this.UpdateLog(System.Environment.NewLine + this.ultraCheckEditor1.Text + " - completed.");
                    return;
                }


                //Create Work Table
                this.UpdateStatus("Creating work table.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VSW_REVERSE_ON_HAND_WORK]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[VSW_REVERSE_ON_HAND_WORK]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                _cmd.CommandText = MakeWorkTableSQL_VSW();
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                this.UpdateTime();

                ////Create Upsert Stored Procedure
                //this.UpdateStatus("Creating work procedure.");
                //_cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CONVERT_VSW_BINS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[CONVERT_VSW_BINS]";
                //_cmd.CommandType = CommandType.Text;
                //_cmd.ExecuteNonQuery();

                //_cmd.CommandText = VSW_MakeWorkProcedureSQL();
                //_cmd.CommandType = CommandType.Text;
                //_cmd.ExecuteNonQuery();
                //_task.UpdateTime();


                // Create a datatable that corresponds to the temporary work table
                DataTable dtWork = new DataTable("VSW_REVERSE_ON_HAND_WORK");
                dtWork.Columns.Add("HDR_RID", typeof(int));
                dtWork.Columns.Add("HN_RID", typeof(int));
                dtWork.Columns.Add("ST_RID", typeof(int));
                dtWork.Columns.Add("USTAT", typeof(int));
                dtWork.Columns.Add("VSW_REVERSE_ON_HAND_UNITS", typeof(int));


                //Set the progress bar maximum
                long totalRowsToProcess = this._totalRowsToRead * _storeRIDs.LongLength;
                this.ultraProgressBar1.Maximum = (int)(totalRowsToProcess / 1000) + 1000; //adding an extra 1000 for rounding

                this.UpdateLog(System.Environment.NewLine + "Processing " + totalRowsToProcess.ToString("###,###,###,###,###,###,##0") + " total VSW Reverse On Hand rows.");



                _cmd.CommandText = "SELECT HDR_RID FROM VSW_REVERSE_ONHAND";
                _cmd.CommandType = CommandType.Text;
                SqlDataAdapter sda = new SqlDataAdapter(_cmd);
                DataTable dtHeaderRIDS = new DataTable("HEADER_RIDS");
                sda.Fill(dtHeaderRIDS);




                this.UpdateStatus("Processing");


                // Setup the SqlBulkCopy
                _bulkCopy = new SqlBulkCopy(_databaseConnectionString, SqlBulkCopyOptions.TableLock);
                int idealBatchSizeFactor = (5000 / _storeRIDs.Length) + 1; //we want to process about 5000 rows at a time
                _bulkCopy.BatchSize = (_storeRIDs.Length * idealBatchSizeFactor); // 5000;
                _bulkCopy.NotifyAfter = 0;  //_storeRIDs.Length; // 1000;
                //bulkCopyVSW.SqlRowsCopied += new SqlRowsCopiedEventHandler(_task.bulkCopy_SqlRowsCopied);
                _bulkCopy.DestinationTableName = "VSW_REVERSE_ON_HAND_WORK";



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
                        _bulkCopy.WriteToServer(dtWork);
                        bulkCopy_SqlRowsCopied(headerCount, headerCount);
                        headerCount = 0;
                    }

                }


                this.UpdateStatus("Complete");
                this.UpdateTime();




            }

            catch (ThreadAbortException ex)
            {
                HandleAbort(ex);
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }
            finally
            {
                DatabaseConnection_Close();
            }
        }

        public void Step1_InsertToWorkTable_DailyAndWeekly()
        {
            try
            {
                if (DatabaseConnection_Initialize() == false) return;
                UpdateStatusToStarting();
                SetTableNames();

            


                this.UpdateResult("Obtaining row count");



                // Get the total row count (each row contains data for all the stores which we have to expand out)

                _cmd.CommandText = "SELECT COUNT(*) FROM " + _binTableName + " WHERE HN_RID % " + _numberOfHistoryTables.ToString() + "=" + _currentTable.ToString();
                _cmd.CommandType = CommandType.Text;



                this._totalRowsToRead = -1;
                object totDailyRows = _cmd.ExecuteScalar();
                if (totDailyRows != null && totDailyRows != DBNull.Value)
                {
                    string stemp = totDailyRows.ToString();
                    long.TryParse(stemp, out _totalRowsToRead);
                }

                if (_totalRowsToRead == -1)
                {
                    this.UpdateStatus("Failed");
                    this.UpdateResult("Error: Could not obtain total row count.");
                    return;
                }
                this.UpdateTime();

                if (_totalRowsToRead == 0)
                {
                    this.UpdateResult("Table " + _currentTable.ToString() + ": Zero rows to process");
                    this.UpdateStatus("Complete");
                    this.UpdateTime();
                    return;
                }


                //Create Work Table
                this.UpdateStatus("Creating work table.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + _workTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[" + _workTableName + "]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                _cmd.CommandText = MakeWorkTableSQL_DailyAndWeekly();
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                this.UpdateTime();



                // Create a datatable that corresponds to the temporary work table
                _dtWork = new DataTable(this._workTableName);
                _dtWork.Columns.Add("HN_RID", typeof(int));
                _dtWork.Columns.Add("JULIAN_TIME_ID", typeof(int));
                _dtWork.Columns.Add("ST_RID", typeof(int));
                _dtWork.Columns.Add("SALES", typeof(int));
                _dtWork.Columns.Add("SALES_REG", typeof(int));
                _dtWork.Columns.Add("SALES_PROMO", typeof(int));
                _dtWork.Columns.Add("SALES_MKDN", typeof(int));
                _dtWork.Columns.Add("STOCK", typeof(int));
                _dtWork.Columns.Add("STOCK_REG", typeof(int));
                _dtWork.Columns.Add("STOCK_MKDN", typeof(int));
                _dtWork.Columns.Add("IN_STOCK_SALES", typeof(int));
                _dtWork.Columns.Add("IN_STOCK_SALES_REG", typeof(int));
                _dtWork.Columns.Add("IN_STOCK_SALES_PROMO", typeof(int));
                _dtWork.Columns.Add("IN_STOCK_SALES_MKDN", typeof(int));
                _dtWork.Columns.Add("ACCUM_SELL_THRU_SALES", typeof(int));
                _dtWork.Columns.Add("ACCUM_SELL_THRU_STOCK", typeof(int));
                _dtWork.Columns.Add("DAYS_IN_STOCK", typeof(int));
                _dtWork.Columns.Add("RECEIVED_STOCK", typeof(int));



                //Set the progress bar maximum
                long totalRowsToProcess = _totalRowsToRead * _storeRIDs.LongLength;
                //this.ultraProgressBar1.Maximum = (int)(totalRowsToProcess / 1000) + 1000; //adding an extra 1000 for rounding
                progressBarMax = (int)(totalRowsToProcess / 1000) + 1000; //adding an extra 1000 for rounding

                this.UpdateLog(System.Environment.NewLine + "Processing " + totalRowsToProcess.ToString("###,###,###,###,###,###,##0") + " total store day bin rows for table " + this._currentTable.ToString() );

                this.UpdateStatus("Processing");


                // Setup the SqlBulkCopy
                _bulkCopy = new SqlBulkCopy(_databaseConnectionString, SqlBulkCopyOptions.TableLock);
                int idealBatchSizeFactor = (5000 / _storeRIDs.Length) + 1; //we want to process about 5000 rows at a time
                _bulkCopy.BatchSize = (_storeRIDs.Length * idealBatchSizeFactor); // 5000;
                _bulkCopy.NotifyAfter = 0; // _storeRIDs.Length; // 1000;
                //bulkCopyDay.SqlRowsCopied += new SqlRowsCopiedEventHandler(_task.bulkCopy_SqlRowsCopied);
                _bulkCopy.DestinationTableName = this._workTableName;

                ReadBinDataAndWriteToTempWorkTable();

              
                this.UpdateStatus("Complete");
                this.UpdateTime();
                    
                
            }

            catch (ThreadAbortException ex)
            {
                HandleAbort(ex); 
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }
            finally
            {
                DatabaseConnection_Close();
            }
        }

        public void Step2_DropBinTable()
        {
            try
            {
                if (DatabaseConnection_Initialize() == false) return;
                UpdateStatusToStarting();
                SetTableNames();

                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + _binTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) DROP TABLE " + _binTableName;
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                this.UpdateTime();

                this.UpdateStatusAndProgressBarToComplete();
            }
            catch (ThreadAbortException ex)
            {
                HandleAbort(ex);
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }
            finally
            {
                DatabaseConnection_Close();
            }
        }

        public void Step3_Upsert()
        {
            try
            {
                if (DatabaseConnection_Initialize() == false) return;
                UpdateStatusToStarting();
                SetTableNames();



                //Create Work Table
                this.UpdateStatus("Creating procedure.");
                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + _workProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[" + _workProcedureName + "]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();

                if (this._taskType == TaskCommon.TaskType.VSW)
                {
                    _cmd.CommandText = MakeWorkProcedureSQL_VSW();
                }
                else
                {
                    _cmd.CommandText = MakeWorkProcedureSQL_DailyAndWeekly(_numberOfHistoryTables);
                }

                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                this.UpdateTime();


                _cmd.CommandText = "[dbo].[" + _workProcedureName + "]";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.ExecuteNonQuery();

                this.UpdateTime();

                DataTable dtResult = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(_cmd);
                sqlDataAdapter.Fill(dtResult);

                UpdateLogWithProcessSummary(dtResult);

                UpdateStatusAndProgressBarToComplete();

            }

            catch (ThreadAbortException ex)
            {
                HandleAbort(ex);
            }
            catch (Exception ex)
            {
                this.UpdateStatus("Failed");
                this.UpdateTime();
                this.UpdateResult("Error: " + ex.ToString());
            }
            finally
            {
                DatabaseConnection_Close();
            }
        }

        public void Step4_DropWorkTableAndProcedure()
        {
            try
            {
                if (DatabaseConnection_Initialize() == false) return;
                UpdateStatusToStarting();
                SetTableNames();

                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + _workTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) DROP TABLE " + _workTableName;
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                this.UpdateTime();

                _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + _workProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[" + _workProcedureName + "]";
                _cmd.CommandType = CommandType.Text;
                _cmd.ExecuteNonQuery();
                this.UpdateTime();

                this.UpdateStatusAndProgressBarToComplete();
            }
            catch (ThreadAbortException ex)
            {
                HandleAbort(ex);
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }
            finally
            {
                DatabaseConnection_Close();
            }
        }


        private void SetTableNames()
        {
            if (this._taskType == TaskCommon.TaskType.Daily)
            {
                _stepName = _stepName + " Table " + _currentTable.ToString();
                _binTableName = "STORE_DAY_HISTORY_BIN";
                _workTableName = "STORE_HISTORY_WORK_DAY" + _currentTable.ToString();
                _workProcedureName = "CONVERT_STORE_DAILY_BINS" + _currentTable.ToString();
                _normalizedTableName = "STORE_HISTORY_DAY" + _currentTable.ToString();
            }
            else if (this._taskType == TaskCommon.TaskType.Weekly)
            {
                _stepName = _stepName + " Table " + _currentTable.ToString();
                _binTableName = "STORE_WEEK_HISTORY_BIN";
                _workTableName = "STORE_HISTORY_WORK_WEEK" + _currentTable.ToString();
                _workProcedureName = "CONVERT_STORE_WEEKLY_BINS" + _currentTable.ToString(); ;
                _normalizedTableName = "STORE_HISTORY_WEEK" + _currentTable.ToString();
            }
            else if (this._taskType == TaskCommon.TaskType.VSW)
            {
                _binTableName = "VSW_REVERSE_ONHAND";
                _workTableName = "VSW_REVERSE_ON_HAND_WORK";
                _workProcedureName = "CONVERT_VSW_BIN";
                _normalizedTableName = "VSW_REVERSE_ON_HAND";
            }
            
        }
        private void HandleAbort(ThreadAbortException ex)
        {
            this.UpdateStatus("Aborted");
            this.UpdateTime();
        }
        private void HandleErrors(Exception ex)
        {
            this.UpdateStatus("Failed");
            this.UpdateTime();
            this.UpdateLog(System.Environment.NewLine + _taskName + _stepName + ": Error: " + ex.ToString());
        }

      

        /// <summary>
        /// Creates and opens a new database connection
        /// </summary>
        /// <returns>returns true if successful</returns>
        private bool DatabaseConnection_Initialize()
        {
            this.UpdateStatus("Initializing");
            this.UpdateResult(System.Environment.NewLine + "Connecting to database...");
            try
            {
                //databaseConnectionString = _SAB.ConnectionString;
                _databaseCommandConnection = new SqlConnection(_databaseConnectionString);
                _databaseCommandConnection.Open();

                _cmd = new SqlCommand();
                _cmd.Connection = _databaseCommandConnection;
                _cmd.CommandTimeout = 0;
                return true;
            }
            catch (Exception exception)
            {
                UpdateResult("Error:" + exception.Message);
                return false;
            }
        }

        private void DatabaseConnection_Close()
        {
            if (_databaseCommandConnection != null)
            {
                try
                {
                    _databaseCommandConnection.Close();
                }
                catch  //ignore errors
                {
                }
            }
        }


   

        private void ReadBinDataAndWriteToTempWorkTable()
        {



            int styleRid = -1;
            int colorCodeRid = -1;
            int sizeCodeRid = -1;


            string varName = string.Empty;



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









            this.UpdateResult("Obtaining styles.");
            this.UpdateTime();



            //==============
            // Get Styles
            //==============
            DataTable dtAllStyles = null;

            dtAllStyles = nodeData.GetAllStyles();







            this.UpdateResult("Preparing to process rows.");
            this.UpdateTime();



            int storeCount = _storeRIDs.Length;

            foreach (DataRow row in dtAllStyles.Rows) // by STYLE
            {
                styleRid = int.Parse(row["HN_RID"].ToString());
                HierarchyNodeList colorNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(styleLevelProf.Level, mainHier.Key, mainHier.Key, styleRid, false, eNodeSelectType.NoVirtual);

                foreach (HierarchyNodeProfile colorNode in colorNodeList.ArrayList) // by COLOR
                {
                    colorCodeRid = colorNode.ColorOrSizeCodeRID;

                    foreach (WeekProfile weekProf in _weekList) // TIME (each week)
                    {
                        timeKeyList = new ArrayList();
                        //==========================
                        // Build day list for week
                        //==========================

                        if (this._taskType == TaskCommon.TaskType.Daily)
                        {
                            foreach (DayProfile dayProf in weekProf.Days.ArrayList)
                            {
                                timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, dayProf.Key));
                            }
                        }
                        else
                        {
                            timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsWeekly, weekProf.Key));
                        }



                        HierarchyNodeList sizeNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(colorLevelProf.Level, mainHier.Key, mainHier.Key, colorNode.Key, false, eNodeSelectType.NoVirtual);


                        foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList) // by SIZES
                        {
                            int sizeRID = sizeNode.Key;
                            if (sizeRID % 10 == this._currentTable)
                            {
                                sizeCodeRid = sizeNode.ColorOrSizeCodeRID;
                                double[] valueList = null;

                                for (int t = 0; t < timeKeyList.Count; t++) // by TIME
                                {
                                    SQL_TimeID timeID = (SQL_TimeID)timeKeyList[t];
                                    _dtWork.Rows.Clear();



                                    //We are adding one row for each store for this size and time
                                    for (int i = 0; i < storeCount; i++) // by STORE
                                    {
                                        DataRow newRow = _dtWork.NewRow();
                                        newRow["HN_RID"] = sizeRID;
                                        newRow["JULIAN_TIME_ID"] = timeID.TimeID;
                                        newRow["ST_RID"] = _storeRIDs[i];
                                        _dtWork.Rows.Add(newRow);
                                    }

                                    for (int v = 0; v < _varKeyList.Count; v++) // by VARIABLE
                                    {
                                        varName = _varKeyList[v].ToString();

                                        if (this._taskType == TaskCommon.TaskType.Daily)
                                        {
                                            valueList = _dlStoreVarHist.GetStoreVariableDayValue(varName, styleRid, timeID, colorCodeRid, sizeCodeRid, _storeRIDs); // get values by STORE
                                        }
                                        else
                                        {
                                            valueList = _dlStoreVarHist.GetStoreVariableWeekValue(varName, styleRid, timeID, colorCodeRid, sizeCodeRid, _storeRIDs); // get values by STORE
                                        }



                                        for (int i = 0; i < storeCount; i++)
                                        {
                                            _dtWork.Rows[i][varName] = valueList[i];
                                        }

                                    } // end by VARIABLE

                                    //remove rows that contain all zeros, so they will not be inserted on the database
                                    for (int i = storeCount - 1; i >= 0; i--)
                                    {
                                        DataRow storeRow = _dtWork.Rows[i];
                                        bool nonZeros = false;
                                        int v = 0;
                                        while (nonZeros == false && v < _varKeyList.Count)
                                        {
                                            varName = _varKeyList[v].ToString();
                                            if ((int)storeRow[varName] != 0)
                                            {
                                                nonZeros = true;
                                            }
                                            v++;
                                        }
                                        if (nonZeros == false) //remove this row
                                        {
                                            _dtWork.Rows.RemoveAt(i);
                                        }
                                    }


                                    _bulkCopy.WriteToServer(_dtWork);
                                    bulkCopy_SqlRowsCopied(storeCount, _dtWork.Rows.Count);


                                    //if (_formClosing)
                                    //{
                                    //    return;
                                    //}

                                } // end by TIME
                            } //end if sizeRID % 10
                        } // end by SIZES
                    } // end TIME (each week)
                } // end by COLOR
            } // end by STYLE



        }


        private string MakeWorkTableSQL_DailyAndWeekly()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE [dbo].[" + this._workTableName + "](");
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

        private string MakeWorkTableSQL_VSW()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE [dbo].[" + this._workTableName + "](");
            sb.AppendLine("[HDR_RID] [int] NOT NULL,");
            sb.AppendLine("[HN_RID] [int] NOT NULL,");
            sb.AppendLine("[ST_RID] [int] NOT NULL,");
            sb.AppendLine("[USTAT] [int] NOT NULL,");
            sb.AppendLine("[VSW_REVERSE_ON_HAND_UNITS] [int] NULL");
            sb.AppendLine(") ON [PRIMARY]");

            return sb.ToString();
        }

        private string MakeWorkProcedureSQL_DailyAndWeekly(int numberOfHistoryTables)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-- =============================================");
            sb.AppendLine("-- Description:	Moves data from work table");
            sb.AppendLine("-- =============================================");
            sb.AppendLine("CREATE PROCEDURE [dbo].[" + this._workProcedureName + "] ");
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
            sb.AppendLine("--Begin Process Step 1: Get data from work table");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("");
            sb.AppendLine("DECLARE @SUMMARY_VIEW TABLE	");
            sb.AppendLine("(");
            sb.AppendLine("	HN_RID int, ");
            sb.AppendLine("	TIME_ID int,  ");
            sb.AppendLine("	ST_RID int, ");
            sb.AppendLine("	HN_MOD int,");
            sb.AppendLine("");
            sb.AppendLine("	--values to update in bin tables");
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
            sb.AppendLine(" SELECT 		");
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
            sb.AppendLine("FROM [dbo].[" + this._workTableName + "]  wd ");
            sb.AppendLine("INNER JOIN [dbo].[HIERARCHY_NODE] hn on hn.HN_RID=wd.HN_RID ");
            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 1: Get data from work table',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Row Count:',(SELECT COUNT(*) FROM @SUMMARY_VIEW) ");
            sb.AppendLine("--End Process Step 1: Get data from work table");
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

            //for (int i = 0; i < numberOfHistoryTables; i++)
            //{
            sb.AppendLine("UPDATE sv ");
            sb.AppendLine("SET sv.INSERT_OR_UPDATE_FLAG='U' ");
            sb.AppendLine("FROM @SUMMARY_VIEW sv ");
            sb.AppendLine("INNER JOIN [dbo].[" + _normalizedTableName + "] d ON sv.HN_MOD=" + this._currentTable.ToString() + " AND d.HN_RID=sv.HN_RID AND d.TIME_ID=sv.TIME_ID AND d.ST_RID=sv.ST_RID ");
            sb.AppendLine("");
            //}

            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 2: Determine which rows to update',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,null, null ");
            sb.AppendLine("--End Process Step 2: Determine which rows to update");
            sb.AppendLine("");
            sb.AppendLine("--Begin Process Step 3: Update history tables");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");

            //for (int i = 0; i < numberOfHistoryTables; i++)
            //{
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
            sb.AppendLine("FROM [dbo].[" + _normalizedTableName + "] d ");
            sb.AppendLine("INNER JOIN @SUMMARY_VIEW sv ON d.HN_RID=sv.HN_RID AND d.TIME_ID=sv.TIME_ID AND d.ST_RID=sv.ST_RID ");
            sb.AppendLine("WHERE sv.HN_MOD=" + this._currentTable.ToString() + " AND sv.INSERT_OR_UPDATE_FLAG='U' ");
            sb.AppendLine("SET @UPDATED_ROWS = @@ROWCOUNT ");
            sb.AppendLine("SET @TOTAL_UPDATED_ROWS = @TOTAL_UPDATED_ROWS + @UPDATED_ROWS ");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 3: Update history tables - Table " + this._currentTable.ToString() + "',0,DATEDIFF(millisecond, @TIME_TO_EXECUTE, getDate()),'Updated Rows:', (SELECT @UPDATED_ROWS) ");
            sb.AppendLine("");
            //}

            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 3: Update history tables',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Total Updated Rows:', (SELECT @TOTAL_UPDATED_ROWS) ");
            sb.AppendLine("--End Process Step 3: Update history tables");

            sb.AppendLine("--Begin Process Step 4: Add rows to history tables");
            sb.AppendLine("SET @TIME_TO_EXECUTE_PROCESS_STEP = getDate();");
            sb.AppendLine("DECLARE @INSERTED_ROWS INT");
            sb.AppendLine("DECLARE @TOTAL_INSERTED_ROWS INT");
            sb.AppendLine("SET @TOTAL_INSERTED_ROWS=0");
            sb.AppendLine("");

            //for (int i = 0; i < numberOfHistoryTables; i++)
            //{

            sb.AppendLine("SET @TIME_TO_EXECUTE = getDate();");
            sb.AppendLine("INSERT INTO [dbo].[" + _normalizedTableName + "] ");
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
            sb.AppendLine("WHERE sv.HN_MOD=" + this._currentTable.ToString() + " AND sv.INSERT_OR_UPDATE_FLAG='I' ");
            sb.AppendLine("SET @INSERTED_ROWS = @@ROWCOUNT ");
            sb.AppendLine("SET @TOTAL_INSERTED_ROWS = @TOTAL_INSERTED_ROWS + @INSERTED_ROWS ");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 4: Add rows to history tables - Table " + this._currentTable.ToString() + "',0,DATEDIFF(millisecond, @TIME_TO_EXECUTE, getDate()),'Inserted Rows:', (SELECT @INSERTED_ROWS) ");
            sb.AppendLine("");
            //}

            sb.AppendLine("");
            sb.AppendLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Step 4: Add rows to history tables',DATEDIFF(millisecond, @TIME_TO_EXECUTE_PROCESS_STEP, getDate()),0,'Total Inserted Rows:', (SELECT @TOTAL_INSERTED_ROWS)");
            sb.AppendLine("--End Process Step 4: Add rows to history tables");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("SELECT * FROM @PROCESS_SUMMARY");
            sb.AppendLine("");
            sb.AppendLine("END");

            return sb.ToString();
        }

        private string MakeWorkProcedureSQL_VSW()
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
            sb.AppendLine("FROM [dbo].[" + this._workTableName + "]  wt ");
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
            sb.AppendLine("INNER JOIN [dbo].[" + this._normalizedTableName + "] vsw ON vsw.HDR_RID=sv.HDR_RID AND vsw.HN_RID=sv.HN_RID AND vsw.ST_RID=sv.ST_RID ");
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
            sb.AppendLine("FROM [dbo].[" + this._normalizedTableName + "] vsw ");
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

            sb.AppendLine("INSERT INTO [dbo].[" + this._normalizedTableName + "] ");
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


        private void UpdateStatus(string status)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.lblStatus.Text = status;
                    });
                }
                else
                {
                    this.lblStatus.Text = status;
                }
            }
            catch
            {
            }
        }
        private void UpdateResult(string result)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.lblResult.Text = result;
                    });
                }
                else
                {
                    this.lblResult.Text = result;
                }
            }
            catch
            {
            }
        }
        private void UpdateTime()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        TimeSpan ts = System.DateTime.Now.Subtract(_startTime);
                        this.lblTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                    });
                }
                else
                {
                    TimeSpan ts = System.DateTime.Now.Subtract(_startTime);
                    this.lblTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                }
            }
            catch
            {
            }
        }
        private void UpdateEstimatedTime(string estTime)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.lblEstTime.Text = estTime;
                    });
                }
                else
                {
                    this.lblEstTime.Text = estTime;
                }
            }
            catch
            {
            }
        }
        private void UpdateTime(TimeSpan ts)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.lblTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                    });
                }
                else
                {
                    this.lblTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                }
            }
            catch
            {
            }
        }



 

        private void UpdateStatusAndProgressBarToComplete()
        {
            progressBarMax = 10;
            progressBarValue = 10;
            this.UpdateStatus("Complete");
            this.UpdateTime();
        }
        private void UpdateStatusToStarting()
        {
            this._startTime = System.DateTime.Now;
            this.UpdateStatus("Starting");
            this.UpdateResult("Working - please wait.");
        }


        private void bulkCopy_SqlRowsCopied(int rowsRead, int rowsCopied)
        {
            _totalRowsRead += rowsRead;
            _totalRowsCopied += rowsCopied;
            //_updateUIcounter++;
            //if ((_updateUIcounter % 10) == 0)  //update the screen every once in awhile
            //{
            //   _updateUIcounter = 0;


           UpdateResult("Read: " + _totalRowsRead.ToString("###,###,###,###,###,#00") + " Copied: " + _totalRowsCopied.ToString("###,###,###,###,###,#00"));

            int curval = (int)(_totalRowsRead / 1000);
            if (curval < this.ultraProgressBar1.Maximum)
            {

                progressBarValue = curval;
            }
            else
            {

                progressBarValue = this.ultraProgressBar1.Maximum; ;
            }

            TimeSpan ts = System.DateTime.Now.Subtract(this._startTime);

     

            double tempVal = ((double)this.ultraProgressBar1.Maximum - (double)this.ultraProgressBar1.Value) / (double)this.ultraProgressBar1.Value;

            int minutesLeft = (int)(ts.TotalMinutes * tempVal);

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                     UpdateEstimatedTime((minutesLeft / 60).ToString("##00") + ":" + (minutesLeft % 60).ToString("00") + ":00");
                });
            }
            else
            {
                UpdateEstimatedTime((minutesLeft / 60).ToString("##00") + ":" + (minutesLeft % 60).ToString("00") + ":00");
            }

            this.UpdateTime(ts);
            // }
        }

        private void UpdateLog(string logMsg)
        {
            _UpdateLog(logMsg);
        }

        private void UpdateLogWithProcessSummary(DataTable dtResult)
        {
            //UpdateLog(System.Environment.NewLine + "Process Summary");
            //string scol = System.Environment.NewLine + "Step".PadRight(55, ' ');
            //scol += "Time to Execute".PadRight(25, ' ');
            //scol += "Time to Execute Substep".PadRight(25, ' ');
            //scol += "Description".PadRight(25, ' ');
            //scol += "Results".PadRight(25, ' ');
            //UpdateLog(scol);
            //UpdateLog(System.Environment.NewLine); 


            //for (int i = 0; i < dtResult.Columns.Count; i++)
            //{
            //    string s = dtResult.Columns[i].ColumnName.PadRight(50, ' ');
            //    UpdateLog(s);
            //}
            string updated = "";
            string inserted = "";
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                if (dtResult.Rows[i]["PROCESS_STEP_DESCRIPTION"] != DBNull.Value && dtResult.Rows[i]["PROCESS_STEP_RESULTS"] != DBNull.Value)
                {
                    string sResult = (string)dtResult.Rows[i]["PROCESS_STEP_RESULTS"];
                    string sDescrip = (string)dtResult.Rows[i]["PROCESS_STEP_DESCRIPTION"];
                    if (sDescrip == "Total Updated Rows:")
                    {
                        updated = sResult;
                    }
                    if (sDescrip == "Total Inserted Rows:")
                    {
                        inserted = sResult;
                    }

                }
                //for (int j = 0; j < dtResult.Columns.Count; j++)
                //{
                //    string s;
                //    if (dtResult.Rows[i][j] != DBNull.Value)
                //    {

                //        //if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE")
                //        //{
                //        //    int milli = (int)dtResult.Rows[i]["PROCESS_STEP_TIME_TO_EXECUTE"];
                //        //    int hours = (int)System.Math.Floor(((double)milli / 60000) / 60);
                //        //    int minutes = (int)System.Math.Floor((double)milli / 60000) % 60;
                //        //    int seconds = (int)System.Math.Floor((double)milli / 1000) % 60;

                //        //    s = hours.ToString("##00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                //        //}
                //        //else if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS")
                //        //{
                //        //    int milli = (int)dtResult.Rows[i]["PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS"];
                //        //    int hours = (int)System.Math.Floor(((double)milli / 60000) / 60);
                //        //    int minutes = (int)System.Math.Floor((double)milli / 60000) % 60;
                //        //    int seconds = (int)System.Math.Floor((double)milli / 1000) % 60;

                //        //    s = hours.ToString("##00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                //        //}
                //        //else
                //        //{
                //        //    s = dtResult.Rows[i][j].ToString();
                //        //}
                //          if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE")
                //        {

                //          }
                //    }
                //    else
                //    {
                //        s = "";
                //    }

                //    int padding = 25;
                //    if (j == 0) padding=55;
                //    UpdateLog(s.PadRight(padding, ' '));
                //}
                //UpdateLog(System.Environment.NewLine); 
                UpdateResult("Inserted: " + inserted + " Updated: " + updated);
            }
        }

        //public void UpdateLogWithProcessSummary2(DataTable dtResult)
        //{
        //    UpdateLog(System.Environment.NewLine + "Process Summary");
        //    string scol = System.Environment.NewLine + "Step".PadRight(55, ' ');
        //    scol += "Time to Execute".PadRight(25, ' ');
        //    scol += "Time to Execute Substep".PadRight(25, ' ');
        //    scol += "Description".PadRight(25, ' ');
        //    scol += "Results".PadRight(25, ' ');
        //    UpdateLog(scol);
        //    UpdateLog(System.Environment.NewLine);
        //    //for (int i = 0; i < dtResult.Columns.Count; i++)
        //    //{
        //    //    string s = dtResult.Columns[i].ColumnName.PadRight(50, ' ');
        //    //    UpdateLog(s);
        //    //}

        //    for (int i = 0; i < dtResult.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dtResult.Columns.Count; j++)
        //        {
        //            string s;
        //            if (dtResult.Rows[i][j] != DBNull.Value)
        //            {

        //                if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE")
        //                {
        //                    int milli = (int)dtResult.Rows[i]["PROCESS_STEP_TIME_TO_EXECUTE"];
        //                    int hours = (int)System.Math.Floor(((double)milli / 60000) / 60);
        //                    int minutes = (int)System.Math.Floor((double)milli / 60000) % 60;
        //                    int seconds = (int)System.Math.Floor((double)milli / 1000) % 60;

        //                    s = hours.ToString("##00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        //                }
        //                else if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS")
        //                {
        //                    int milli = (int)dtResult.Rows[i]["PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS"];
        //                    int hours = (int)System.Math.Floor(((double)milli / 60000) / 60);
        //                    int minutes = (int)System.Math.Floor((double)milli / 60000) % 60;
        //                    int seconds = (int)System.Math.Floor((double)milli / 1000) % 60;

        //                    s = hours.ToString("##00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        //                }
        //                else
        //                {
        //                    s = dtResult.Rows[i][j].ToString();
        //                }
        //            }
        //            else
        //            {
        //                s = "";
        //            }

        //            int padding = 25;
        //            if (j == 0) padding = 55;
        //            UpdateLog(s.PadRight(padding, ' '));
        //        }
        //        UpdateLog(System.Environment.NewLine);
        //    }
        //}
       
    }
}
