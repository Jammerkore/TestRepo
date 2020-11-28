using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace MIDRetail.DataCommon
{
    public static class SQLMonitorList
    {
        public static bool showFormAtStartup = true; //change this to true to show the SQL Monitor form when the application loads



        public static DataTable dtSQLMonitor = null;
        public static bool doMonitor = false;
        public static bool includeApplicationText = true;
        public static bool includeStartAndStopTimes = true;
        public static bool includeStackTrace = true;
        public static bool includeMessageListening = false;


        public static void ClearSQLMonitorEntries()
        {
            dtSQLMonitor.Rows.Clear();
        }

        public class SQLMonitorEntry
        {
            private DateTime _startTime;
            private string _command;
            private MIDDbParameter[] _InputParameters;
            public SQLMonitorEntry(string command, MIDDbParameter[] InputParameters = null)
            {
                if (includeStartAndStopTimes)
                {
                    this._startTime = System.DateTime.Now;
                }
                _command = command;
                _InputParameters = InputParameters;
            }

            public void AddEntryToSQLMonitor()
            {
                bool doAddEntry = true;

                if (includeMessageListening == false)
                {
                    if (_command == "SELECT MESSAGE_RID, MESSAGE_DATETIME, MESSAGE_TO,  MESSAGE_FROM, MESSAGE_CODE, MESSAGE_PROCESSING_PRIORITY, MESSAGE_DETAILS  FROM MESSAGE_QUEUE  WHERE MESSAGE_TO = @MESSAGE_TO  ORDER BY MESSAGE_PROCESSING_PRIORITY ")
                    {
                        doAddEntry = false;
                    }
                }


                if (doAddEntry)
                {
                    DataRow dr = dtSQLMonitor.NewRow();
                    dr["Command"] = _command;

                    if (_InputParameters != null && _InputParameters.Length > 0)
                    {
                        string sInputParams = string.Empty;

                        for (int i = 0; i < _InputParameters.Length; i++)
                        {

                            sInputParams += _InputParameters[i].ParameterName + "=";
                            if (_InputParameters[i].Value != null)
                            {
                                sInputParams += _InputParameters[i].Value.ToString();
                            }

                            sInputParams += System.Environment.NewLine;
                        }


                        dr["Input Parameters"] = sInputParams;
                    }
                    else
                    {
                        dr["Input Parameters"] = string.Empty;
                    }

                    if (includeStartAndStopTimes)
                    {
                        TimeSpan ts = System.DateTime.Now.Subtract(this._startTime);
                        string duration = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + "." + ts.Milliseconds.ToString("0000");
                        dr["Time Started"] = _startTime;
                        dr["Duration"] = duration;
                    }

                    //dr["Time Ended"] = command;

                    if (includeStackTrace)
                    {
                        //StackFrame frame = new StackFrame(1);
                        //var method = frame.
                        //var type = method.DeclaringType;
                        //var name = method.Name;

                        StackTrace st = new StackTrace(2, true);
                        //string stackIndent = "";
                        //string traceString = st.ToString();
                        //for (int i = 0; i < st.FrameCount; i++)
                        //{
                        //    // Note that at this level, there are four 
                        //    // stack frames, one for each method invocation.
                        //    StackFrame sf = st.GetFrame(i);
                        //    traceString = st.ToString();
                        //    Console.WriteLine(stackIndent + " Method: {0}",
                        //        sf.GetMethod());
                        //    Console.WriteLine(stackIndent + " File: {0}",
                        //        sf.GetFileName());
                        //    Console.WriteLine(stackIndent + " Line Number: {0}",
                        //        sf.GetFileLineNumber());
                        //    stackIndent += "  ";
                        //}
                        dr["Trace"] = st.ToString();
                    }
                    dtSQLMonitor.Rows.Add(dr);
                }
            }

        }
        public static void BeforeExecution(ref SQLMonitorEntry sme, string cmd, MIDDbParameter[] InputParameters=null)
        {
            if (doMonitor)
            {
                sme = new SQLMonitorEntry(cmd, InputParameters);
            }
        }
        public static void AfterExecution(SQLMonitorEntry sme)
        {
            if (doMonitor)
            {
                sme.AddEntryToSQLMonitor();
            }
        }

        public static void PrepareDataTableForBinding()
        {
            dtSQLMonitor = new DataTable();
            dtSQLMonitor.Columns.Add("Command");
            dtSQLMonitor.Columns.Add("Input Parameters");
            dtSQLMonitor.Columns.Add("Time Started", typeof(DateTime));
            //dtSQLMonitor.Columns.Add("Time Ended");
            dtSQLMonitor.Columns.Add("Duration");
            dtSQLMonitor.Columns.Add("Trace");
        }

        //public static void AddEntryToSQLMonitor(string command)
        //{
        //    if (dtSQLMonitor == null)
        //    {
        //        dtSQLMonitor = new DataTable();
        //        dtSQLMonitor.Columns.Add("Command");
        //        //dtSQLMonitor.Columns.Add("Time Started");
        //        //dtSQLMonitor.Columns.Add("Time Ended");
        //        dtSQLMonitor.Columns.Add("Trace");
        //    }
          
        //}
    }
}
