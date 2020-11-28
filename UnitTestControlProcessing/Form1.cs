using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Globalization;

namespace MIDRetail.UnitTestingControlProcessing
{
    public partial class Form1 : Form
    {
        SessionAddressBlock SAB;
        List<int> apiCodes = new List<int>();
        int _waitPeriod = 2000;
        int _RollupWaitPeriod = 10000;
        string _user, _line, _password, _waitPeriodstr, _jobsList, _optDelimiter, _APICodesfileLocation, _RollupAPIWaitPeriod;
        string processJobName = null, testProcessJobName = null, line = null, jobMessage = null;
        StreamReader _reader = null;
        List<string> _lines = new List<string>();
        List<string> _lines2 = new List<string>();
        int code = 0;
        int textCode = 0;

        DataTable _dtProcesses = new DataTable();
        public Form1(SessionAddressBlock aSAB)
        {
            InitializeComponent();
            SAB = aSAB;
            listBox1.Items.Clear();
            this.Text = "Control Processing Rules Test";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _user = MIDConfigurationManager.AppSettings["User"];
            _password = MIDConfigurationManager.AppSettings["Password"];
            _waitPeriodstr = MIDConfigurationManager.AppSettings["WaitPeriod"];
            _jobsList = MIDConfigurationManager.AppSettings["JobsList"];
            _optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
            _APICodesfileLocation = MIDConfigurationManager.AppSettings["TextCodeList"];
            _RollupAPIWaitPeriod = MIDConfigurationManager.AppSettings["RollupWait"];

            //---------------//
            //Set Wait Periods
            //---------------//
            if (_waitPeriodstr != string.Empty)
            {
                _waitPeriod = Convert.ToInt32(_waitPeriodstr);
            }

            if (_RollupAPIWaitPeriod != string.Empty)
            {
                _RollupWaitPeriod = Convert.ToInt32(_RollupAPIWaitPeriod);
            }

            //-------------------//
            //Get the list of jobs
            //-------------------//

            _reader = new StreamReader(_APICodesfileLocation);  //opens the file
            while ((_line = _reader.ReadLine()) != null)
            {
                string[] fields = MIDstringTools.Split(_line, _optDelimiter[0], true);
                if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                {
                    continue;
                }
                _line = Regex.Replace(_line, ",", string.Empty);
                _lines.Add(_line);
            }
            _reader.Close();
            _reader.Dispose();
            if (_lines != null && _lines.Count > 0)
            {
                for (int s = 0; s < _lines.Count; s++)
                {
                    code = Convert.ToInt32(_lines[s]);
                    apiCodes.Add(code);
                }
            }
            _lines.Clear();
            _line = null;
            _reader = new StreamReader(_jobsList);  //opens the file
            while ((_line = _reader.ReadLine()) != null)
            {
                string[] fields = MIDstringTools.Split(_line, _optDelimiter[0], true);
                if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                {
                    continue;
                }
                _lines.Add(_line);
            }
            _reader.Close();
            _reader.Dispose();
            _lines2 = new List<string>(_lines);

            InitializeCheckedListBox();
        }

        private void InitializeCheckedListBox()
        {
            _lines.Clear();
            _line = null;
            List<string[]> _jobs = new List<string[]>();
            _reader = new StreamReader(_jobsList);  //opens the file
            while ((_line = _reader.ReadLine()) != null)
            {
                string[] fields = MIDstringTools.Split(_line, _optDelimiter[0], true);
                if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                {
                    continue;
                }
                _lines.Add(_line);
            }
            _reader.Close();
            _reader.Dispose();

            for (int s = 0; s < _lines.Count; s++)
            {
                checkedListBox1.Items.Add(_lines[s]);
                checkedListBox1.SetItemCheckState(s, CheckState.Checked);
                checkedListBox2.Items.Add(_lines[s]);
                checkedListBox2.SetItemCheckState(s, CheckState.Checked);
            }
            //SelectAllCheckBoxes(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            listBox1.Items.Clear();
            button2.Enabled = false;
            eProcesses process, testProcess;
            string processName, testProcessName, result;
            eSecurityAuthenticate authentication, testAuthentication;
            int processID = 0, testProcessID = 0;
            eMIDMessageLevel aProcessMessageLevel, aTestProcessMessageLevel;
            DateTime aProcessCompletionTime, aTestProcessCompletionTime;
            
            try
            {
                if (!checkBox2.Checked)
                {

                    //------------//
                    //Process Jobs
                    //------------//

                    foreach (object itemChecked in checkedListBox1.CheckedItems)
                    {
                        processJobName = Convert.ToString(itemChecked);

                        foreach (object testItemChecked in checkedListBox2.CheckedItems)
                        {
                            testProcessJobName = Convert.ToString(testItemChecked);
                            result = "OK";

                            testAuthentication = ProcessJobs(processJobName, testProcessJobName, out aProcessMessageLevel, out aTestProcessMessageLevel, out aProcessCompletionTime, out aTestProcessCompletionTime);

                            //------------------------//
                            //Post message to list box
                            //------------------------//
                            if (aTestProcessMessageLevel == eMIDMessageLevel.None || aTestProcessMessageLevel == eMIDMessageLevel.Severe)
                            {
                                result = "Severe";
                            }
                            jobMessage = "with job name " + testProcessJobName;

                            if (testAuthentication == eSecurityAuthenticate.Unavailable)
                            {
                                result = "Unavailable";
                            }

                            listBox1.Items.Add("Checking Process:" + processJobName + " " + jobMessage + " ---- " + result + " Time: " + aTestProcessCompletionTime);
                            Application.DoEvents();
                        }
                    }
                    
                }
                else
                {
                    processNormal();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                button2.Enabled = true;
                MessageBox.Show("Test Complete");
            }
        }


        private void processNormal()
        {
            this.Cursor = Cursors.WaitCursor;
            listBox1.Items.Clear();
            button2.Enabled = false;
            eProcesses process, testProcess;
            string processName, testProcessName, result;
            eSecurityAuthenticate authentication, testAuthentication;
            int processID = 0, testProcessID = 0;
            eMIDMessageLevel aProcessMessageLevel, aTestProcessMessageLevel;
            DateTime aProcessCompletionTime, aTestProcessCompletionTime;
            //------------//
            //Process Rules
            //------------//
            DataTable dtProcesses = MIDText.GetTextType(eMIDTextType.eProcesses, eMIDTextOrderBy.TextCode);

            foreach (DataRow dr in dtProcesses.Rows)
            {
                textCode = Convert.ToInt32(dr["TEXT_CODE"]);
                if (!apiCodes.Contains(textCode))
                {
                    continue;
                }
                process = (eProcesses)Convert.ToInt32(dr["TEXT_CODE"]);
                if (process == eProcesses.unknown)
                {
                    continue;
                }
                processName = Convert.ToString(dr["TEXT_VALUE"]);

                authentication = SAB.ClientServerSession.UserLogin(_user, _password, process);
                processID = SAB.ControlServerSession.ControlServerSessionRemote.CurrentProcessID;
                listBox1.Items.Add("Testing Process:" + processName + "(" + Convert.ToString(process.GetHashCode()) + ")");

                Application.DoEvents();
                foreach (DataRow dr2 in dtProcesses.Rows)
                {
                    textCode = Convert.ToInt32(dr2["TEXT_CODE"]);
                    if (!apiCodes.Contains(textCode))
                    {
                        continue;
                    }

                    testProcess = (eProcesses)Convert.ToInt32(dr2["TEXT_CODE"]);
                    if (testProcess == eProcesses.unknown)
                    {
                        continue;
                    }
                    if (testProcess == eProcesses.purge && process == eProcesses.purge)
                    {
                        bool stop = true;
                    }
                    result = "OK";
                    testProcessName = Convert.ToString(dr2["TEXT_VALUE"]);
                    if (checkBox2.Checked)  // processing rules only
                    {
                        testAuthentication = SAB.ClientServerSession.UserLogin(_user, _password, testProcess);
                        testProcessID = SAB.ControlServerSession.ControlServerSessionRemote.CurrentProcessID;
                    }
                    else
                    {
                        testProcessJobName = "Purge";
                        testAuthentication = ProcessJobs(processJobName, testProcessJobName, out aProcessMessageLevel, out aTestProcessMessageLevel, out aProcessCompletionTime, out aTestProcessCompletionTime);
                        if (aTestProcessMessageLevel == eMIDMessageLevel.None)
                        {
                            result = "Error Processing";
                        }
                        jobMessage = "with job name " + testProcessJobName;
                    }
                    if (testAuthentication == eSecurityAuthenticate.Unavailable)
                    {
                        result = "Unavailable";
                    }
                    if (!checkBox1.Checked || (checkBox1.Checked && result == "Unavailable"))
                    {
                        listBox1.Items.Add("     Checking Process:" + testProcessName + "(" + Convert.ToString(testProcess.GetHashCode()) + ") " + jobMessage + " ---- " + result);
                        Application.DoEvents();
                    }
                    if (checkBox2.Checked)  // processing rules only
                    {
                        SAB.ControlServerSession.SetProcessState(testProcess, testProcessID, false);
                        SAB.ControlServerSession.SetProcessState(process, processID, true);
                    }
                }
                if (checkBox2.Checked)  // processing rules only
                {
                    SAB.ControlServerSession.SetProcessState(process, processID, false);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();

            f.ShowDialog();

            StreamWriter s = new StreamWriter(f.FileName);

            foreach (string li in listBox1.Items)
            {
                s.WriteLine(li);
            }

            s.Close();

            MessageBox.Show("Export Complete");
 
        }

        private eSecurityAuthenticate ProcessJobs(string aProcessJobName, string aTestProcessJobName, out eMIDMessageLevel aProcessMessageLevel, out eMIDMessageLevel aTestProcessMessageLevel, 
                                                    out DateTime aProcessCompletionTime, out DateTime aTestProcessCompletionTime)
        {
            eSecurityAuthenticate retCode = eSecurityAuthenticate.UserAuthenticated;
            aProcessMessageLevel = eMIDMessageLevel.None;
            aTestProcessMessageLevel = eMIDMessageLevel.None;
            ControlProcessJob jProcess;
            ControlProcessJob jTestProcess;
            bool processesRunning = true;

            jProcess = new ControlProcessJob(aProcessJobName);
            jTestProcess = new ControlProcessJob(aTestProcessJobName);
            
            jProcess.ExecuteJobInThread();
            if ((jTestProcess.JobName == "RollupAPITest") || (jTestProcess.JobName == "GenerateRelieveIntransitAPITest"))
            {
                Thread.Sleep(_RollupWaitPeriod);
            }
            else
            {
                //Thread.Sleep(2000);
                Thread.Sleep(_waitPeriod);
            }
            jTestProcess.ExecuteJobInThread();

            while (processesRunning)
            {
                if (!jProcess.isRunning &&
                    !jTestProcess.isRunning)
                {
                    if (jTestProcess.RetCode == eMIDMessageLevel.ProcessUnavailable)
                    {
                        retCode = eSecurityAuthenticate.Unavailable;
                    }
                    processesRunning = false;
                }
                else
                {
                    //Thread.Sleep(2000);
                    Thread.Sleep(_waitPeriod);
                }
            }

            aProcessMessageLevel = jProcess.RetCode;
            aTestProcessMessageLevel = jTestProcess.RetCode;
            aProcessCompletionTime = jProcess.CompletionDateTime;
            aTestProcessCompletionTime = jTestProcess.CompletionDateTime;

            return retCode;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int box = 1;
            if (checkBox3.Checked)
            {
                SelectAllCheckBoxes(box, true);
            }
            else
            {
                SelectAllCheckBoxes(box, false);
            }
        }
        private void SelectAllCheckBoxes(int box, bool CheckThem)
        {
            if (box == 1)
            {
                for (int i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
                {
                    if (CheckThem)
                    {
                        checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    }
                    else
                    {
                        checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
            }
            else if (box == 2)
            {
                for (int i = 0; i <= (checkedListBox2.Items.Count - 1); i++)
                {
                    if (CheckThem)
                    {
                        checkedListBox2.SetItemCheckState(i, CheckState.Checked);
                    }
                    else
                    {
                        checkedListBox2.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int box = 2;
            if (checkBox4.Checked)
            {
                SelectAllCheckBoxes(box, true);
            }
            else
            {
                SelectAllCheckBoxes(box, false);
            }
        }

    }

    public class ControlProcessJob
    {
        //=======
        // FIELDS
        //=======

        private string _jobName;
        private string _schedulerInterfacePath;
        private bool _isRunning;
        private eMIDMessageLevel _retCode = eMIDMessageLevel.None;  
        private DateTime _completionDateTime;
        private Thread _thread;

        //=============
        // CONSTRUCTORS
        //=============

        public ControlProcessJob(string aJobName)
        {
            try
            {
                _jobName = aJobName;
                _schedulerInterfacePath = @"C:\Logility\RO\Batch\ScheduleInterface.exe";


#if (DEBUG)
                _schedulerInterfacePath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\ScheduleInterface\bin\Debug\" + "ScheduleInterface.exe";
#else
                object value = MIDConfigurationManager.AppSettings["SchedulerInterfacePath"];
                if (value != null)
                {
                    _schedulerInterfacePath = @"C:\Logility\RO\Batch\ScheduleInterface.exe";
                }
#endif
                //_schedulerInterfacePath = @"C:\Logility\RO\Batch\ScheduleInterface.exe";

            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public bool isRunning
        {
            get
            {
                try
                {
                    return _isRunning;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }

        public DateTime CompletionDateTime
        {
            get
            {
                try
                {
                    return _completionDateTime;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }

        //public eProcessCompletionStatus CompletionStatus
        //{
        //    get
        //    {
        //        try
        //        {
        //            return _completionStatus;
        //        }
        //        catch (ThreadAbortException exc)
        //        {
        //            string message = exc.ToString();
        //            throw;
        //        }
        //        catch (Exception exc)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public eMIDMessageLevel RetCode
        {
            get
            {
                try
                {
                    return _retCode;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }

        public string JobName
        {
            get
            {
                try
                {
                    return _jobName;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }

        //========
        // METHODS
        //========

        public void AbortThread()
        {
            try
            {
                _thread.Abort();
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public void WaitForThreadExit()
        {
            try
            {
                _thread.Join();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public void ExecuteJobInThread()
        {
            try
            {
                //ExecuteJob();
                _thread = new Thread(new ThreadStart(ExecuteJob));
                _thread.Start();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public void ExecuteJob()
        {
            try
            {
                _isRunning = true;
                _retCode = eMIDMessageLevel.Information;

                Process job = new Process();

                job.StartInfo.FileName = _schedulerInterfacePath;

                job.StartInfo.Arguments = JobName;

                job.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                job.Start();
                job.WaitForExit();
                if (job.ExitCode > 0)
                {
                    _retCode = (eMIDMessageLevel)job.ExitCode;
                }
                job.Close();
            }
            catch (ThreadAbortException)
            {
                _retCode = eMIDMessageLevel.Error;
            }
            catch (Exception exc)
            {
                _retCode = eMIDMessageLevel.Error;
            }
            finally
            {
                _isRunning = false;
                _completionDateTime = DateTime.Now;
            }
        }
    }
}
