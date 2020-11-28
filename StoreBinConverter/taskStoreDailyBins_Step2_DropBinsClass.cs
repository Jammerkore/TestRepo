using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace StoreBinConverter
{
    public class taskStoreDailyBins_Step2_DropBinsClass
    {
        private SubTaskSelectControl _task;
        private SqlCommand _cmd;

        public taskStoreDailyBins_Step2_DropBinsClass(SubTaskSelectControl task, SqlCommand cmd)
        {
            _task = task;
            _cmd = cmd;
        }

        private bool _formClosing;
        public void HandleFormClosing()
        {
            _formClosing = true;
        }



        public void DropBins()
        {
            try
            {
                _task.startTime = System.DateTime.Now;
                _task.UpdateStatus("Starting");
                _task.UpdateResult("Working - please wait.");

                if (!_formClosing)
                {
                    _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STORE_DAY_HISTORY_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) TRUNCATE TABLE STORE_DAY_HISTORY_BIN";
                    _cmd.CommandType = CommandType.Text;
                    _cmd.ExecuteNonQuery();
                    _task.UpdateTime();
                }

                if (!_formClosing)
                {
                    _cmd.CommandText = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STORE_DAY_HISTORY_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) DROP TABLE STORE_DAY_HISTORY_BIN";
                    _cmd.CommandType = CommandType.Text;
                    _cmd.ExecuteNonQuery();
                    _task.UpdateTime();
                }

                if (!_formClosing)
                {
                    _task.ultraProgressBar1.Maximum = 10;
                    _task.ultraProgressBar1.Value = 10;
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
        }
    }
}
