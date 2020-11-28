using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace StoreBinConverter
{
    public class taskVSW_Step3_UpsertClass
    {

        private SubTaskSelectControl _task;
        private SqlCommand _cmd;

        public taskVSW_Step3_UpsertClass(SubTaskSelectControl task, SqlCommand cmd)
        {
            _task = task;
            _cmd = cmd;
        }

        private bool _formClosing;
        public void HandleFormClosing()
        {
            _formClosing = true;
        }

        public void Upsert()
        {
            try
            {
                _task.startTime = System.DateTime.Now;
                _task.UpdateStatus("Starting");
                _task.UpdateResult("Working - please wait.");

                if (!_formClosing)
                {
                    // Now that the rows are in the temporary work table - use the work table and update/insert into the VSW Reverse On Hand normalized table via a stored procedure call
                    _task.UpdateStatus("Updating");
                    _cmd.CommandText = "[dbo].[CONVERT_VSW_BINS]";
                    _cmd.CommandType = CommandType.StoredProcedure;
                    _cmd.ExecuteNonQuery();

                    DataTable dtResult = new DataTable();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(_cmd);
                    sqlDataAdapter.Fill(dtResult);

                    _task.UpdateLogWithProcessSummary(dtResult);

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
