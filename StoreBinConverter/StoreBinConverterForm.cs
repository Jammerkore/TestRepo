using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Threading;


using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace StoreBinConverter
{
    public partial class StoreBinConverterForm : Form
    {
        public StoreBinConverterForm()
        {
            InitializeComponent();


            if (ServicesStart() && DatabaseConnectionOpen() && DatabaseVerifyVersion() && GetStoreRIDs())
            {
                _canProcessTasks = true;
                this.taskControlContainer1.SetVisibleSteps(this._numberOfStoreHistoryTables);
            }

            this.FormClosing += new FormClosingEventHandler(StoreBinConverterForm_FormClosing);
        }

        private bool _canProcessTasks = false; 
        private bool _formClosing = false;
        private bool _processingTasks = false;
        private DateTime _startTotalTime;
        private List<Thread> _threadList = new List<Thread>();
        private Thread _totalTimeThread;

        private ArrayList _varKeyList;
        private bool _needVariableKeyList = true;
        private ProfileList _dailyWeekList;
        private bool _needDailyTimes = true;
        private ProfileList _weeklyWeekList;
        private bool _needWeeklyTimes = true;
        private int[] _storeRIDs;
        private bool _needStoreData = true;
        private int _numberOfStoreHistoryTables;
        private SqlCommand _cmd;
        private SqlConnection _databaseCommandConnection;
        private string _databaseConnectionString;
        private SessionAddressBlock _SAB;
        private bool _needToStartServices = true;

        private void StoreBinConverterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //prompt the user if they want to close the form while processing
            if (_processingTasks)
            {
                if (MessageBox.Show("Tasks are processing.  Are you sure you want to close the application??", "Confirm:", MessageBoxButtons.YesNoCancel) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }


            _formClosing = true;
            Abort();

            ServicesStop();
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnProcess":
                    ProcessTasks();
                    break;
                case "btnStop":
                    Abort();
                    break;
     
            }
        }

        private void Abort()
        {
            foreach (Thread t in this._threadList)
            {
                if (t != null) t.Abort();
            }
        }

        private void ProcessTasks()
        {
            if (CheckCanProcessTasks() == false || this.taskControlContainer1.IsOneOrMoreTasksChecked() == false)
            {
                return;
            }

            try
            {
                ProcessTasksInitialize();

                if (!_formClosing && this.taskControlContainer1.ctlTaskStoreDayBins_InsertWork.ultraCheckEditor1.Checked == true)
                {
                    if (GetDailyMinAndMaxTimes() == false || GetVariableKeyList() == false)
                    {
                        return;
                    }
                }
                if (!_formClosing && this.taskControlContainer1.ctlTaskStoreWeeklyBins_InsertWork.ultraCheckEditor1.Checked == true)
                {
                    if (GetWeeklyMinAndMaxTimes() == false || GetVariableKeyList() == false)
                    {
                        return;
                    }
                }

                //Instantiate the total time thread
                ThreadStart totalTimeStart = new ThreadStart(ShowTotalTime);
                _totalTimeThread = new Thread(totalTimeStart);
                _totalTimeThread.Name = "TotalTime";


                Handle_VSW_Step1_InsertToWorkTable();
                Handle_VSW_Step2_DropBinTable();
                Handle_VSW_Step3_Upsert();
                Handle_VSW_Step4_DropWorkTable();

                Handle_Daily_Step1_InsertToWorkTable();
                Handle_Daily_Step2_DropBinTable();
                Handle_Daily_Step3_Upsert();
                Handle_Daily_Step4_DropWorkTable();

                Handle_Weekly_Step1_InsertToWorkTable();
                Handle_Weekly_Step2_DropBinTable();
                Handle_Weekly_Step3_Upsert();
                Handle_Weekly_Step4_DropWorkTable();
            }
            finally
            {
                ProcessTasksFinalize();
                DatabaseConnectionClose();
            }
        }

        private bool CheckCanProcessTasks()
        {
            if (this._canProcessTasks == false)
            {
                MessageBox.Show("Can not process tasks - please fix database/network connection issues and restart the application.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Handle_VSW_Step1_InsertToWorkTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskVSW_InsertWork.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskVSW.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskVSW_InsertWork.StepText;
                threadParameters.SAB = _SAB;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.VSW;
                threadParameters.storeRIDs = _storeRIDs;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskVSW_InsertWork.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart insertThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskVSW_InsertWork.Step1_InsertToWorkTable_VSW);

                //Add the thread to the thread list
                Thread insertThread = new Thread(insertThreadStart);
                _threadList.Add(insertThread);
              
                //Before moving on - wait for the insert to work table threads to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_VSW_Step2_DropBinTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskVSW_DropBins.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskVSW.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskVSW_DropBins.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.VSW;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskVSW_DropBins.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart dropBinTableThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskVSW_DropBins.Step2_DropBinTable);

                //Add the thread to the thread list
                Thread dropBinTableThread = new Thread(dropBinTableThreadStart);
                _threadList.Add(dropBinTableThread);

                //Before moving on - wait for the drop bin thread to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_VSW_Step3_Upsert()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskVSW_Upsert.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskVSW.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskVSW_Upsert.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.VSW;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskVSW_Upsert.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart upsertThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskVSW_Upsert.Step3_Upsert);
                  
                //Add the thread to the thread list
                Thread upsertThread = new Thread(upsertThreadStart);
                _threadList.Add(upsertThread);

                //Before moving on - wait for the upsert thread to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_VSW_Step4_DropWorkTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskVSW_DropWork.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskVSW.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskVSW_DropWork.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.VSW;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskVSW_DropWork.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart dropWorkThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskVSW_DropWork.Step4_DropWorkTableAndProcedure);

                //Add the thread to the thread list
                Thread dropWorkThread = new Thread(dropWorkThreadStart);
                _threadList.Add(dropWorkThread);

                //Before moving on - wait for the drop work thread to finish
                StartAndWaitForThreads();
            }
        }

        private void Handle_Daily_Step1_InsertToWorkTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreDayBins_InsertWork.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreDayBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreDayBins_InsertWork.StepText;
                threadParameters.SAB = _SAB;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.weekList = _dailyWeekList;
                threadParameters.varKeyList = _varKeyList;
                threadParameters.taskType = TaskCommon.TaskType.Daily;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.storeRIDs = _storeRIDs;
                threadParameters.UpdateLog = UpdateLog;


                for (int i = 0; i < _numberOfStoreHistoryTables; i++)
                {
                    //Instantiate the thread start object
                    ThreadStart insertThreadStart = null;

                    switch (i)
                    {
                        case 0:
                            this.taskControlContainer1.stepDailyInsertTable0.SetParameters(threadParameters, 0);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable0.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 1:
                            this.taskControlContainer1.stepDailyInsertTable1.SetParameters(threadParameters, 1);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable1.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 2:
                            this.taskControlContainer1.stepDailyInsertTable2.SetParameters(threadParameters, 2);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable2.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 3:
                            this.taskControlContainer1.stepDailyInsertTable3.SetParameters(threadParameters, 3);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable3.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 4:
                            this.taskControlContainer1.stepDailyInsertTable4.SetParameters(threadParameters, 4);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable4.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 5:
                            this.taskControlContainer1.stepDailyInsertTable5.SetParameters(threadParameters, 5);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable5.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 6:
                            this.taskControlContainer1.stepDailyInsertTable6.SetParameters(threadParameters, 6);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable6.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 7:
                            this.taskControlContainer1.stepDailyInsertTable7.SetParameters(threadParameters, 7);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable7.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 8:
                            this.taskControlContainer1.stepDailyInsertTable8.SetParameters(threadParameters, 8);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable8.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 9:
                            this.taskControlContainer1.stepDailyInsertTable9.SetParameters(threadParameters, 9);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyInsertTable9.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                    }

                    //Add the thread to the thread list
                    Thread insertThread = new Thread(insertThreadStart);
                    _threadList.Add(insertThread);
                } //end for NumberOfStoreHistoryTables

                //Before moving on - wait for the insert to work table threads to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_Daily_Step2_DropBinTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreDayBins_DropBins.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreDayBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreDayBins_DropBins.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.Daily;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskStoreDayBins_DropBins.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart dropBinTableThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskStoreDayBins_DropBins.Step2_DropBinTable);

                //Add the thread to the thread list
                Thread dropBinTableThread = new Thread(dropBinTableThreadStart);
                _threadList.Add(dropBinTableThread);

                //Before moving on - wait for the drop bin thread to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_Daily_Step3_Upsert()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreDayBins_Upsert.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreDayBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreDayBins_Upsert.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.Daily;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;


                for (int i = 0; i < _numberOfStoreHistoryTables; i++)
                {
                    //Instantiate the thread start object
                    ThreadStart upsertThreadStart = null;

                    switch (i)
                    {
                        case 0:
                            this.taskControlContainer1.stepDailyUpsertTable0.SetParameters(threadParameters, 0);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable0.Step3_Upsert);
                            break;
                        case 1:
                            this.taskControlContainer1.stepDailyUpsertTable1.SetParameters(threadParameters, 1);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable1.Step3_Upsert);
                            break;
                        case 2:
                            this.taskControlContainer1.stepDailyUpsertTable2.SetParameters(threadParameters, 2);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable2.Step3_Upsert);
                            break;
                        case 3:
                            this.taskControlContainer1.stepDailyUpsertTable3.SetParameters(threadParameters, 3);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable3.Step3_Upsert);
                            break;
                        case 4:
                            this.taskControlContainer1.stepDailyUpsertTable4.SetParameters(threadParameters, 4);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable4.Step3_Upsert);
                            break;
                        case 5:
                            this.taskControlContainer1.stepDailyUpsertTable5.SetParameters(threadParameters, 5);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable5.Step3_Upsert);
                            break;
                        case 6:
                            this.taskControlContainer1.stepDailyUpsertTable6.SetParameters(threadParameters, 6);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable6.Step3_Upsert);
                            break;
                        case 7:
                            this.taskControlContainer1.stepDailyUpsertTable7.SetParameters(threadParameters, 7);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable7.Step3_Upsert);
                            break;
                        case 8:
                            this.taskControlContainer1.stepDailyUpsertTable8.SetParameters(threadParameters, 8);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable8.Step3_Upsert);
                            break;
                        case 9:
                            this.taskControlContainer1.stepDailyUpsertTable9.SetParameters(threadParameters, 9);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepDailyUpsertTable9.Step3_Upsert);
                            break;
                    }
                    //Add the thread to the thread list
                    Thread upsertThread = new Thread(upsertThreadStart);
                    _threadList.Add(upsertThread);
                } //end for NumberOfStoreHistoryTables

                //Before moving on - wait for the upsert threads to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_Daily_Step4_DropWorkTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreDayBins_DropWork.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreDayBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreDayBins_DropWork.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.Daily;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskStoreDayBins_DropWork.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart dropWorkThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskStoreDayBins_DropWork.Step4_DropWorkTableAndProcedure);

                //Add the thread to the thread list
                Thread dropWorkThread = new Thread(dropWorkThreadStart);
                _threadList.Add(dropWorkThread);

                //Before moving on - wait for the drop work thread to finish
                StartAndWaitForThreads();
            }
        }

        private void Handle_Weekly_Step1_InsertToWorkTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreWeeklyBins_InsertWork.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreWeeklyBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreWeeklyBins_InsertWork.StepText;
                threadParameters.SAB = _SAB;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.weekList = _weeklyWeekList;
                threadParameters.varKeyList = _varKeyList;
                threadParameters.taskType = TaskCommon.TaskType.Weekly;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.storeRIDs = _storeRIDs;
                threadParameters.UpdateLog = UpdateLog;


                for (int i = 0; i < _numberOfStoreHistoryTables; i++)
                {
                    //Instantiate the thread start object
                    ThreadStart insertThreadStart = null;

                    switch (i)
                    {
                        case 0:
                            this.taskControlContainer1.stepWeeklyInsertTable0.SetParameters(threadParameters, 0);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable0.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 1:
                            this.taskControlContainer1.stepWeeklyInsertTable1.SetParameters(threadParameters, 1);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable1.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 2:
                            this.taskControlContainer1.stepWeeklyInsertTable2.SetParameters(threadParameters, 2);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable2.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 3:
                            this.taskControlContainer1.stepWeeklyInsertTable3.SetParameters(threadParameters, 3);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable3.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 4:
                            this.taskControlContainer1.stepWeeklyInsertTable4.SetParameters(threadParameters, 4);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable4.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 5:
                            this.taskControlContainer1.stepWeeklyInsertTable5.SetParameters(threadParameters, 5);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable5.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 6:
                            this.taskControlContainer1.stepWeeklyInsertTable6.SetParameters(threadParameters, 6);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable6.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 7:
                            this.taskControlContainer1.stepWeeklyInsertTable7.SetParameters(threadParameters, 7);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable7.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 8:
                            this.taskControlContainer1.stepWeeklyInsertTable8.SetParameters(threadParameters, 8);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable8.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                        case 9:
                            this.taskControlContainer1.stepWeeklyInsertTable9.SetParameters(threadParameters, 9);
                            insertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyInsertTable9.Step1_InsertToWorkTable_DailyAndWeekly);
                            break;
                    }

                    //Add the thread to the thread list
                    Thread insertThread = new Thread(insertThreadStart);
                    _threadList.Add(insertThread);
                } //end for NumberOfStoreHistoryTables

                //Before moving on - wait for the insert to work table threads to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_Weekly_Step2_DropBinTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreWeeklyBins_DropBins.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreWeeklyBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreWeeklyBins_DropBins.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.Weekly;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskStoreWeeklyBins_DropBins.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart dropBinTableThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskStoreWeeklyBins_DropBins.Step2_DropBinTable);

                //Add the thread to the thread list
                Thread dropBinTableThread = new Thread(dropBinTableThreadStart);
                _threadList.Add(dropBinTableThread);

                //Before moving on - wait for the drop bin thread to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_Weekly_Step3_Upsert()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreWeeklyBins_Upsert.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreWeeklyBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreWeeklyBins_Upsert.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.Weekly;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;


                for (int i = 0; i < _numberOfStoreHistoryTables; i++)
                {
                    //Instantiate the thread start object
                    ThreadStart upsertThreadStart = null;

                    switch (i)
                    {
                        case 0:
                            this.taskControlContainer1.stepWeeklyUpsertTable0.SetParameters(threadParameters, 0);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable0.Step3_Upsert);
                            break;
                        case 1:
                            this.taskControlContainer1.stepWeeklyUpsertTable1.SetParameters(threadParameters, 1);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable1.Step3_Upsert);
                            break;
                        case 2:
                            this.taskControlContainer1.stepWeeklyUpsertTable2.SetParameters(threadParameters, 2);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable2.Step3_Upsert);
                            break;
                        case 3:
                            this.taskControlContainer1.stepWeeklyUpsertTable3.SetParameters(threadParameters, 3);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable3.Step3_Upsert);
                            break;
                        case 4:
                            this.taskControlContainer1.stepWeeklyUpsertTable4.SetParameters(threadParameters, 4);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable4.Step3_Upsert);
                            break;
                        case 5:
                            this.taskControlContainer1.stepWeeklyUpsertTable5.SetParameters(threadParameters, 5);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable5.Step3_Upsert);
                            break;
                        case 6:
                            this.taskControlContainer1.stepWeeklyUpsertTable6.SetParameters(threadParameters, 6);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable6.Step3_Upsert);
                            break;
                        case 7:
                            this.taskControlContainer1.stepWeeklyUpsertTable7.SetParameters(threadParameters, 7);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable7.Step3_Upsert);
                            break;
                        case 8:
                            this.taskControlContainer1.stepWeeklyUpsertTable8.SetParameters(threadParameters, 8);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable8.Step3_Upsert);
                            break;
                        case 9:
                            this.taskControlContainer1.stepWeeklyUpsertTable9.SetParameters(threadParameters, 9);
                            upsertThreadStart = new ThreadStart(this.taskControlContainer1.stepWeeklyUpsertTable9.Step3_Upsert);
                            break;
                    }
                    //Add the thread to the thread list
                    Thread upsertThread = new Thread(upsertThreadStart);
                    _threadList.Add(upsertThread);
                } //end for NumberOfStoreHistoryTables

                //Before moving on - wait for the upsert threads to finish
                StartAndWaitForThreads();
            }
        }
        private void Handle_Weekly_Step4_DropWorkTable()
        {
            if (!_formClosing && this.taskControlContainer1.ctlTaskStoreWeeklyBins_DropWork.ultraCheckEditor1.Checked == true)
            {
                //Clear the thread list
                _threadList.Clear();

                //Add the total time thread to the thread list
                _threadList.Add(_totalTimeThread);

                //Setup the thread parameters
                TaskCommon.stepParameters threadParameters = new TaskCommon.stepParameters();
                threadParameters.taskName = taskControlContainer1.taskStoreWeeklyBins.StepText;
                threadParameters.stepName = taskControlContainer1.ctlTaskStoreWeeklyBins_DropWork.StepText;
                threadParameters.databaseConnectionString = _databaseConnectionString;
                threadParameters.taskType = TaskCommon.TaskType.Weekly;
                threadParameters.numberOfHistoryTables = _numberOfStoreHistoryTables;
                threadParameters.UpdateLog = UpdateLog;
                this.taskControlContainer1.ctlTaskStoreWeeklyBins_DropWork.SetParameters(threadParameters, 0);

                //Instantiate the thread start object
                ThreadStart dropWorkThreadStart = new ThreadStart(this.taskControlContainer1.ctlTaskStoreWeeklyBins_DropWork.Step4_DropWorkTableAndProcedure);

                //Add the thread to the thread list
                Thread dropWorkThread = new Thread(dropWorkThreadStart);
                _threadList.Add(dropWorkThread);

                //Before moving on - wait for the drop work thread to finish
                StartAndWaitForThreads();
            }
        }

        private void StartAndWaitForThreads()
        {
            //Start all threads we added
            foreach (Thread t in this._threadList)
            {
                t.Start();
            }

            //Wait for all threads we added to finish
            bool keepWaitingForThreadsToFinish = true;
            while (keepWaitingForThreadsToFinish)
            {
                bool allThreadsFinished = true;
                foreach (Thread t in this._threadList)
                {
                    if (t.Name != "TotalTime")
                    {
                        if (t.ThreadState != System.Threading.ThreadState.Aborted && t.ThreadState != System.Threading.ThreadState.Stopped)
                        {
                            allThreadsFinished = false;
                        }
                    }
                }

                if (allThreadsFinished == true)
                {
                    keepWaitingForThreadsToFinish = false;
                    _threadList[0].Abort(); //abort the total time thread
                }

                Application.DoEvents();
            }
        }

        private void ProcessTasksInitialize()
        {
            _startTotalTime = System.DateTime.Now;
            _processingTasks = true;



            this.taskControlContainer1.SetCheckBoxes(false);

            this.Cursor = Cursors.WaitCursor;
            this.ultraToolbarsManager1.Tools["btnProcess"].SharedProps.Enabled = false;
            this.ultraToolbarsManager1.Tools["btnStop"].SharedProps.Enabled = true;

        }
        private void ProcessTasksFinalize()
        {
            _processingTasks = false;
            this.Cursor = Cursors.Default;
            this.ultraToolbarsManager1.Tools["btnProcess"].SharedProps.Enabled = true;
            this.ultraToolbarsManager1.Tools["btnStop"].SharedProps.Enabled = false;

            this.taskControlContainer1.SetCheckBoxes(true);

            UpdateLog(System.Environment.NewLine + "Tasks have been processed.");
        }

        private bool GetVariableKeyList()
        {
            try
            {
                if (_needVariableKeyList)
                {
                    _varKeyList = new ArrayList();
                    DataTable dtVar = MIDText.GetLabels((int)eForecastBaseDatabaseStoreVariables.SalesTotal, (int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
                    foreach (DataRow aRow in dtVar.Rows)
                    {
                        string textValue = aRow["TEXT_VALUE"].ToString();
                        _varKeyList.Add(textValue);
                    }

                    _needVariableKeyList = false;
                }
                return true;
             }
            catch (Exception ex)
            {
                UpdateLog(System.Environment.NewLine + "Error: Could not obtain variable key list..." + ex.ToString());
                return false;
            }
        }
      
        private bool GetDailyMinAndMaxTimes()
        {
            int dailyMinimumTime;
            int dailyMaximumTime;
            try
            {
                if (_needDailyTimes)
                {
                    // Get the minimum time
                    dailyMinimumTime = 0;
                    _cmd.CommandText = "SELECT [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME](MIN([TIME_ID])) FROM STORE_DAY_HISTORY_BIN";
                    _cmd.CommandType = CommandType.Text;
                    object minTime = _cmd.ExecuteScalar();
                    if (minTime != null && minTime != DBNull.Value)
                    {
                        string stemp = minTime.ToString();
                        int.TryParse(stemp, out dailyMinimumTime);
                    }
                    if (dailyMinimumTime == 0)
                    {
                        UpdateLog(System.Environment.NewLine + "Error: Could not obtain minimum time for daily history.");
                        return false;
                    }

                    // Get the maximum time
                    dailyMaximumTime = 0;
                    _cmd.CommandText = "SELECT [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME](MAX([TIME_ID])) FROM STORE_DAY_HISTORY_BIN";
                    _cmd.CommandType = CommandType.Text;
                    object maxTime = _cmd.ExecuteScalar();
                    if (maxTime != null && maxTime != DBNull.Value)
                    {
                        string stemp = maxTime.ToString();
                        int.TryParse(stemp, out dailyMaximumTime);
                    }
                    if (dailyMaximumTime == 0)
                    {
                        UpdateLog(System.Environment.NewLine + "Error: Could not obtain maximum time for daily history.");
                        return false;
                    }
                    




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
                    overrideDateRange.StartDateKey = dailyMinimumTime;
                    overrideDateRange.EndDateKey = dailyMaximumTime;
                    overrideDateRange.DateRangeType = eCalendarRangeType.Static;

                    //if (isDaily)
                    //{
                        overrideDateRange.SelectedDateType = eCalendarDateType.Day;
                    //}
                    //else
                    //{
                    //    overrideDateRange.SelectedDateType = eCalendarDateType.Week;
                    //}


                    overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;
                    
                    int dailyDRP_RID = _SAB.ApplicationServerSession.Calendar.AddDateRange(overrideDateRange);
                    DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(dailyDRP_RID);
                    _dailyWeekList = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);



                    _needDailyTimes = false;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                UpdateLog(System.Environment.NewLine + "Error: Could not obtain times for daily history..." + ex.ToString());
                return false;
            }
        }

        private bool GetWeeklyMinAndMaxTimes()
        {
            int weeklyMinimumTime;
            int weeklyMaximumTime;
            try
            {
                if (_needWeeklyTimes)
                {
                    // Get the minimum time
                    weeklyMinimumTime = 0;
                    _cmd.CommandText = "SELECT [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME](MIN([TIME_ID])) FROM STORE_WEEK_HISTORY_BIN";
                    _cmd.CommandType = CommandType.Text;
                    object minTime = _cmd.ExecuteScalar();
                    if (minTime != null && minTime != DBNull.Value)
                    {
                        string stemp = minTime.ToString();
                        int.TryParse(stemp, out weeklyMinimumTime);
                    }
                    if (weeklyMinimumTime == 0)
                    {
                        UpdateLog(System.Environment.NewLine + "Error: Could not obtain minimum time for weekly history.");
                        return false;
                    }

                    // Get the maximum time
                    weeklyMaximumTime = 0;
                    _cmd.CommandText = "SELECT [dbo].[UDF_DATE_GET_JULIAN_FROM_SQLTIME](MAX([TIME_ID])) FROM STORE_WEEK_HISTORY_BIN";
                    _cmd.CommandType = CommandType.Text;
                    object maxTime = _cmd.ExecuteScalar();
                    if (maxTime != null && maxTime != DBNull.Value)
                    {
                        string stemp = maxTime.ToString();
                        int.TryParse(stemp, out weeklyMaximumTime);
                    }
                    if (weeklyMaximumTime == 0)
                    {
                        UpdateLog(System.Environment.NewLine + "Error: Could not obtain maximum time for weekly history.");
                        return false;
                    }

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
                    overrideDateRange.StartDateKey = weeklyMinimumTime;
                    overrideDateRange.EndDateKey = weeklyMaximumTime;
                    overrideDateRange.DateRangeType = eCalendarRangeType.Static;

                    //if (isDaily)
                    //{
                    //overrideDateRange.SelectedDateType = eCalendarDateType.Day;
                    //}
                    //else
                    //{
                        overrideDateRange.SelectedDateType = eCalendarDateType.Week;
                    //}


                    overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;

                    int dailyDRP_RID = _SAB.ApplicationServerSession.Calendar.AddDateRange(overrideDateRange);
                    DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(dailyDRP_RID);
                    _weeklyWeekList = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);


                    _needWeeklyTimes = false;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                UpdateLog(System.Environment.NewLine + "Error: Could not obtain times for weekly history..." + ex.ToString());
                return false;
            }
        }

        private bool GetStoreRIDs()
        {
            if (_needStoreData)
            {
                try
                {
                    // Get the store rids
                    ProfileList storeList = _SAB.StoreServerSession.GetAllStoresList();
                    _storeRIDs = new int[MIDStorageTypeInfo.GetStoreMaxRID(0)];
                    for (int i = 0; i < _storeRIDs.Length; i++)
                    {
                        StoreProfile sp = (StoreProfile)storeList.FindKey(i + 1);
                        // Begin TT#646 - stodd - inactive stores causing viewer to abend.
                        // This catches inactive stores that have not been retured from the store service.
                        //if (sp != null)
                        //{
                        //    _storeIdHash.Add(i, sp.StoreId);
                        //}
                        //else
                        //{
                        //    _storeIdHash.Add(i, "Inactive");
                        //}
                        // End TT#646 - stodd - inactive stores causing viewer to abend.
                        //_storeRidHash.Add(i, i + 1);
                        _storeRIDs[i] = i + 1;
                    }

                    _cmd.CommandText = "SELECT STORE_TABLE_COUNT FROM SYSTEM_OPTIONS";
                    _cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(_cmd);
                    DataTable dtNumTables = new DataTable("NUM_TABLES");
                    sda.Fill(dtNumTables);
                    _numberOfStoreHistoryTables = (int)dtNumTables.Rows[0]["STORE_TABLE_COUNT"];


                    _needStoreData = false;
                    return true;
                }
                catch (Exception ex)
                {
                    UpdateLog(System.Environment.NewLine + "Error: Could not obtain times for weekly history..." + ex.ToString());
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
   
        private bool DatabaseConnectionOpen()
        {
            UpdateDBNameInStatusBar();
            UpdateLog(System.Environment.NewLine + "Connecting to database...");
            try
            {
                _databaseConnectionString = _SAB.ConnectionString;
                _databaseCommandConnection = new SqlConnection(_databaseConnectionString);
                _databaseCommandConnection.Open();

                _cmd = new SqlCommand();
                _cmd.Connection = _databaseCommandConnection;
                _cmd.CommandTimeout = 0;
            }
            catch (Exception exception)
            {
                UpdateLog("Error:" + exception.Message);
                return false;
            }

            UpdateLog("complete.");
            return true;
        }

        private bool DatabaseVerifyVersion()
        {
            UpdateLog(System.Environment.NewLine + "Verifying database version...");
            try
            {
                _cmd.CommandText = "SELECT TOP 1 * FROM STORE_HISTORY_DAY0";
                _cmd.CommandType = CommandType.Text;
                SqlDataAdapter sda = new SqlDataAdapter(_cmd);
                DataTable dtVerify = new DataTable();
                sda.Fill(dtVerify);

                if (!dtVerify.Columns.Contains("IN_STOCK_SALES"))
                {
                    UpdateLog("Database version out of date.  Please update the database before processing tasks.");
                    return false;
                }
            }
            catch (Exception exception)
            {
                UpdateLog("Error:" + exception.Message);
                return false;
            }

            UpdateLog("complete.");
            return true;
        }

        private void DatabaseConnectionClose()
        {
            try
            {
                //Close the cmd connection string
                _databaseCommandConnection.Close();
            }
            catch
            {
            }
        }


        private void UpdateDBNameInStatusBar()
        {
            this.ultraStatusBar1.Panels["pnlDB"].Text = EnvironmentInfo.MIDInfo.databaseName;
        }

        private void UpdateLog(string logMsg)
        {
            this.txtLog.Text += logMsg;
            Application.DoEvents();
        }
        private void UpdateTotalTime()
        {
            if (!_formClosing)
            {
                TimeSpan ts = System.DateTime.Now.Subtract(_startTotalTime);
                this.ultraStatusBar1.Panels["pnlTime"].Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                Application.DoEvents();
            }
        }



        private bool ServicesStart()
        {
            if (_needToStartServices == true)
            {
                UpdateLog("Starting Services...");
                try
                {
                    IMessageCallback _messageCallback = new BatchMessageCallback();
                    SessionSponsor _sponsor = new SessionSponsor();
                    _SAB = new SessionAddressBlock(_messageCallback, _sponsor);

                    string eventLogID = "StoreBinViewer";
                    if (!EventLog.SourceExists(eventLogID))
                    {
                        EventLog.CreateEventSource(eventLogID, null);
                    }

                    // Register callback channel

                    try
                    {
                        System.Runtime.Remoting.Channels.IChannel channel = _SAB.OpenCallbackChannel();
                    }
                    catch (Exception exception)
                    {
                        UpdateLog("Error opening port #0 - " + exception.Message);
                        //EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + exception.Message, EventLogEntryType.Error);
                        return false;
                    }

                    // Create Sessions

                    try
                    {
                        _SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Application); 
                    }
                    catch (Exception exception)
                    {
                   
                        Exception innerE = exception;
                        while (innerE.InnerException != null)
                        {
                            innerE = innerE.InnerException;
                        }
                        UpdateLog("Error creating sessions - " + innerE.Message);
                        //EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
                        return false;
                    }

                    ScheduleData _scheduleData = new ScheduleData();
          

                    eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                    MIDConfigurationManager.AppSettings["Password"], eProcesses.StoreBinViewer);
                    if (authentication != eSecurityAuthenticate.UserAuthenticated)
                    {
                        UpdateLog("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"]);
                        //EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
                        //System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
                        return false;
                    }

           
                    _SAB.ClientServerSession.Initialize();
                    _SAB.HierarchyServerSession.Initialize();
                    _SAB.ApplicationServerSession.Initialize();
                    _SAB.StoreServerSession.Initialize();
                    _needToStartServices = false;
                }

                catch (Exception exception)
                {
                    UpdateLog("Error starting services:" + exception.Message);
                    return false;
                }
           

                UpdateLog("complete.");
            }

            return true;
           
        }

        private void ServicesStop()
        {
            if (_SAB != null)
            {
                try
                {
                    _SAB.CloseSessions();
                }
                catch
                {
                }
            }
        }


        private void ShowTotalTime()
        {
            try
            {
                DateTime lastUpdated = _startTotalTime;
                bool keepGoing = true;

                while (keepGoing)
                {
                    DateTime timeNow = System.DateTime.Now;

                    //update every second
                    if (timeNow.Subtract(lastUpdated).TotalMilliseconds > 1000)
                    {
                        lastUpdated = timeNow;
                        TimeSpan ts = timeNow.Subtract(_startTotalTime);


                        this.Invoke((MethodInvoker)delegate
                        {
                            this.ultraStatusBar1.Panels["pnlTime"].Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                        });


                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                string s = ex.ToString();  //prevent warning
            }
        }

       
    }
}
