using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MIDRetailInstaller
{
    public partial class InstallLogViewer : Form
    {
        //object to pass frame to
        InstallerFrame frame = null;
        ucInstallationLog ucLog = null;

        ToolTip tt = new ToolTip();

        //save file name
        string strFileName = "";
        int currWidth;

        public InstallLogViewer()
        {
            InitializeComponent();
        }

        public InstallLogViewer(InstallerFrame p_frame)
        {
            frame = p_frame;
            InitializeComponent();
            currWidth = this.Width;
        }

        private void InstallLogViewer_Load(object sender, EventArgs e)
        {
            tt.SetToolTip(btnExport, frame.GetToolTipText("log_export"));
            tt.SetToolTip(btnClose, frame.GetToolTipText("log_close"));
        }

        public string ExportLog()
        {
            btnExport_Click(this, new EventArgs());
            return strFileName;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //installation log export file
            strFileName = DateTime.Now.Year.ToString() + 
                DateTime.Now.Month.ToString("00") +
                DateTime.Now.Day.ToString("00") +
                DateTime.Now.Hour.ToString("00") +
                DateTime.Now.Minute.ToString("00") +
                DateTime.Now.Second.ToString("00") +
                DateTime.Now.Millisecond.ToString("00") + 
                "_InstallLog.txt";
            saveInstallLog.FileName = strFileName;

            //show the save dialog
            DialogResult result = saveInstallLog.ShowDialog();

            //positive result...
            if (result == DialogResult.OK)
            {
                //create report file
                StreamWriter sw = new StreamWriter(saveInstallLog.FileName);

                //write lines to report
                for (int intLogEntry = 0; intLogEntry <  gridInstallLog.Rows.Count; intLogEntry++)
                {
                    sw.WriteLine(gridInstallLog.Rows[intLogEntry].Cells[2].Value.ToString().Trim() + ": " + 
                        gridInstallLog.Rows[intLogEntry].Cells[1].Value.ToString().Trim());
                }

                //close writer object
                sw.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //hide log viewer
            this.Hide();

        }

        public void AddLogEntry(string strEntry, eErrorType etError)
        {
            
            //add a row to the install log
            gridInstallLog.Rows.Add();
            
            //add log line to grid
            gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[1].Value = strEntry.Trim();
            switch (etError)
            {
                case eErrorType.error:
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[0].Value = imlErrorTypes.Images[0];
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[2].Value = "Error";
                    break;
                case eErrorType.message:
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[0].Value = imlErrorTypes.Images[2];
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[2].Value = "Message";
                    break;
                case eErrorType.warning:
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[0].Value = imlErrorTypes.Images[1];
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[2].Value = "Warning";
                    break;
                case eErrorType.debug:
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[0].Value = imlErrorTypes.Images[3];
                    gridInstallLog.Rows[gridInstallLog.Rows.Count - 1].Cells[2].Value = "Debug";
                    break;

            }

            // Begin 1668 - JSmith - Install Log
            frame.SetLogMessage(strEntry.Trim(), etError);
            // End 1668
        }

        private void InstallLogViewer_Resize(object sender, EventArgs e)
        {
            if (Width > currWidth)
            {
                ErrorMessage.Width += Width - currWidth;
            }
            else if (Width < currWidth)
            {
                ErrorMessage.Width -= currWidth - Width;
            }

            if (ErrorMessage.Width > ErrorMessage.MinimumWidth)
            {
                currWidth = this.Width;
            }
        }
    }
}
