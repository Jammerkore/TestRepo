using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StoreBinConverter
{
    public partial class SubTaskSelectControl : UserControl
    {
      
        public SubTaskSelectControl()
        {
            InitializeComponent();
        }

        taskCommon.UpdateLogDelegate _UpdateLog;
        taskCommon.UpdateTotalTimeDelegate _UpdateTotalTime;
        public void SetCommonMethods(taskCommon.UpdateLogDelegate UpdateLog, taskCommon.UpdateTotalTimeDelegate UpdateTotalTime)
        {
           _UpdateLog = UpdateLog;
           _UpdateTotalTime = UpdateTotalTime;
        }

        public DateTime startTime;

       

        public void UpdateStatus(string status)
        {
            this.lblStatus.Text = status;
            Application.DoEvents();
        }
        public void UpdateResult(string result)
        {
            this.txtResult.Text = result;
            Application.DoEvents();
        }
        public void UpdateTime()
        {
            TimeSpan ts = System.DateTime.Now.Subtract(startTime);
            this.lblTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            Application.DoEvents();
        }
        public void UpdateTime(TimeSpan ts)
        {
            this.lblTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            Application.DoEvents();
        }


        long _totalRowsCopied = 0;
        int _updateUIcounter = 0;
        public void bulkCopy_SqlRowsCopied(int rowsCopied)
        {
            _totalRowsCopied += rowsCopied;
            _updateUIcounter++;
            if ((_updateUIcounter % 10) == 0)  //update the screen every once in awhile
            {
                _updateUIcounter = 0;


                this.txtResult.Text = "Rows Processed: " + _totalRowsCopied.ToString("###,###,###,###,###,#00");

                int curval = (int)(_totalRowsCopied / 1000);
                if (curval < this.ultraProgressBar1.Maximum)
                    this.ultraProgressBar1.Value = curval;
                else
                    this.ultraProgressBar1.Value = this.ultraProgressBar1.Maximum;

                TimeSpan ts = System.DateTime.Now.Subtract(this.startTime);

                //double percentleft = 100 - ((double)this.ultraProgressBar1.Value / (double)this.ultraProgressBar1.Maximum);
                //int minutesLeft = (int)(ts.TotalMinutes * percentleft);

                double tempVal = ((double)this.ultraProgressBar1.Maximum - (double)this.ultraProgressBar1.Value) / (double)this.ultraProgressBar1.Value;

                int minutesLeft = (int)(ts.TotalMinutes * tempVal);

                this.lblEstTime.Text = (minutesLeft / 60).ToString("##00") + ":" + (minutesLeft % 60).ToString("00") + ":00";
            
                this.UpdateTime(ts);
                UpdateTotalTime();
            }
        }
        public void UpdateTotalTime()
        {
            _UpdateTotalTime();
        }
        public void UpdateLog(string logMsg)
        {
            _UpdateLog(logMsg);
        }
        public void UpdateLogWithProcessSummary(DataTable dtResult)
        {
            UpdateLog(System.Environment.NewLine + "Process Summary");
            string scol = System.Environment.NewLine + "Step".PadRight(55, ' ');
            scol += "Time to Execute".PadRight(25, ' ');
            scol += "Time to Execute Substep".PadRight(25, ' ');
            scol += "Description".PadRight(25, ' ');
            scol += "Results".PadRight(25, ' ');
            UpdateLog(scol);
            UpdateLog(System.Environment.NewLine); 
            //for (int i = 0; i < dtResult.Columns.Count; i++)
            //{
            //    string s = dtResult.Columns[i].ColumnName.PadRight(50, ' ');
            //    UpdateLog(s);
            //}
           
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                for (int j = 0; j < dtResult.Columns.Count; j++)
                {
                    string s;
                    if (dtResult.Rows[i][j] != DBNull.Value)
                    {

                        if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE")
                        {
                            int milli = (int)dtResult.Rows[i]["PROCESS_STEP_TIME_TO_EXECUTE"];
                            int hours = (int)System.Math.Floor(((double)milli / 60000) / 60);
                            int minutes = (int)System.Math.Floor((double)milli / 60000) % 60;
                            int seconds = (int)System.Math.Floor((double)milli / 1000) % 60;

                            s = hours.ToString("##00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                        }
                        else if (dtResult.Columns[j].ColumnName == "PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS")
                        {
                            int milli = (int)dtResult.Rows[i]["PROCESS_STEP_TIME_TO_EXECUTE_SUBPROCESS"];
                            int hours = (int)System.Math.Floor(((double)milli / 60000) / 60);
                            int minutes = (int)System.Math.Floor((double)milli / 60000) % 60;
                            int seconds = (int)System.Math.Floor((double)milli / 1000) % 60;

                            s = hours.ToString("##00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                        }
                        else
                        {
                            s = dtResult.Rows[i][j].ToString();
                        }
                    }
                    else
                    {
                        s = "";
                    }

                    int padding = 25;
                    if (j == 0) padding=55;
                    UpdateLog(s.PadRight(padding, ' '));
                }
                UpdateLog(System.Environment.NewLine); 
            }
        }


    }
}
