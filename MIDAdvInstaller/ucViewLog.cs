using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class ucInstallationLog : UserControl
    {
        //object to pass frame to
        InstallerFrame frame = null;

        ToolTip tt = new ToolTip();

        private InstallLogViewer log_view = null;
        public int errors = 0;
        public int warnings = 0;
        public int messages = 0;

        public ucInstallationLog()
        {
            InitializeComponent();
        }

        public InstallerFrame Frame
        {
            set
            {
                frame = value;
            }
        }

        public void ViewLog()
        {
            btnViewLog_Click(this, new EventArgs());
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            //show log viewer
            log_view.ShowDialog();
        }

        private void ucInstallationLog_Load(object sender, EventArgs e)
        {
            //create the log viewer object
            if (log_view == null)
            {
                if (frame != null)
                {
                    log_view = new InstallLogViewer(frame);
                }
                else
                {
                    log_view = new InstallLogViewer();
                }
            }

            ////add a start entry to the log
            //AddLogEntry("Installation Began:" + DateTime.Now.ToString().Trim(), eErrorType.message);

            if (frame != null)
            {
                tt.SetToolTip(picErrors, frame.GetToolTipText("viewlog_errors"));
                tt.SetToolTip(lblErrors, frame.GetToolTipText("viewlog_errors"));
                tt.SetToolTip(picWarnings, frame.GetToolTipText("viewlog_warnings"));
                tt.SetToolTip(lblWarnings, frame.GetToolTipText("viewlog_warnings"));
                tt.SetToolTip(picMessages, frame.GetToolTipText("viewlog_messages"));
                tt.SetToolTip(lblMessages, frame.GetToolTipText("viewlog_messages"));
                tt.SetToolTip(btnViewLog, frame.GetToolTipText("viewlog_btnViewLog"));
            }
        }

        public void AddLogEntry(string strLogEntry, eErrorType etError)
        {
            if (log_view == null)
            {
                if (frame != null)
                {
                    log_view = new InstallLogViewer(frame);
                }
                else
                {
                    log_view = new InstallLogViewer();
                }
            }

            //add the log entry to the log
            switch(etError)
            {
                case eErrorType.error:
                    log_view.AddLogEntry(strLogEntry, eErrorType.error);
                    errors++;
                    break;
                case eErrorType.message:
                    log_view.AddLogEntry(strLogEntry, eErrorType.message);
                    messages++;
                    break;
                case eErrorType.warning:
                    log_view.AddLogEntry(strLogEntry, eErrorType.warning);
                    warnings++;
                    break;
                case eErrorType.debug:
                    log_view.AddLogEntry(strLogEntry, eErrorType.debug);
                    break;
            }

            //iterate the error type counters
            lblErrors.Text = errors.ToString() + " Errors";
            lblMessages.Text = messages.ToString() + " Messages";
            lblWarnings.Text = warnings.ToString() + " Warnings";

        }

        public string ExportLog()
        {
            return log_view.ExportLog();
        }

    }
}
