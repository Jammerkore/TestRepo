using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class OncClickUpdateConfirmation : Form
    {

        //  installer frame
        InstallerFrame _frame;

        //  installer data
        public DataSet installer_data;

        DataTable dtText = null;
        string _serverName;
        string _databaseName;
        bool _isAppServer;
        StreamWriter _installLog;

        public OncClickUpdateConfirmation(bool aIsAppServer, string aServerName, string aDatabaseName, InstallerFrame p_frm)
        {
            InitializeComponent();
            _frame = p_frm;
            this.Load += new EventHandler(OncClickUpdateConfirmation_Load);
            _serverName = aServerName;
            _databaseName = aDatabaseName;
            _isAppServer = aIsAppServer;
        }

        private void OncClickUpdateConfirmation_Load(object sender, System.EventArgs e)
        {
            //  onload
            //      disable the Continue button
            //      clear the checkbox
            //      enable Terminate button
            cbConfirmN.Checked = false;
            cbConfirmN.CheckState = CheckState.Unchecked;
            cbConfirmN.Update();
            btnContinueN.Enabled = false;
            btnTerminateN.Enabled = true;
            // get installer data
            installer_data = _frame.GetInstallerData();
            dtText = installer_data.Tables["install_text"];
            // GetTexts
            string warningMsg;
            string confirmMsg;
            if (_isAppServer)
            {
                warningMsg = GetText("lblWarningMessageOC-S");
                warningMsg = warningMsg.Replace("{0}", System.Environment.MachineName.Trim());
                warningMsg = warningMsg.Replace("{1}", _serverName.Trim());
                warningMsg = warningMsg.Replace("{2}", _databaseName.Trim());
                confirmMsg = GetText("cbConfirmOC-S");
            }
            else
            {
                warningMsg = GetText("lblWarningMessageOC-C");
                warningMsg = warningMsg.Replace("{0}", System.Environment.MachineName.Trim());
                confirmMsg = GetText("cbConfirmOC-C");
            }
            this.lblWarningMessage.Text = GetText("lblWarningMessageOC");
            this.lblWarningMessageN2.Text = warningMsg;
            this.cbConfirmN.Text = confirmMsg;
            this.btnContinueN.Text = GetText("btnContinueN");
            this.btnTerminateN.Text = GetText("btnTerminateN");
        }

        private void cbConfirmN_CheckedChanged(object sender, EventArgs e)
        {
            //   relase the continue button if the check box has been checked
            if (((CheckBox)sender).CheckState == CheckState.Checked)
            {
                btnContinueN.Enabled = true;
            }
            else
            {
                btnContinueN.Enabled = false;
            }
        }

        //  Continue the Installation
        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            // Begin TT#1668 - JSmith - Install Log
            _frame.SetLogMessage(GetText("OCConfirmed"), eErrorType.message);
            // End TT#1668
        }

        //  Terminate the Installation
        private void btnTerminate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            // Begin TT#1668 - JSmith - Install Log
            _frame.SetLogMessage(GetText("OCTerminated"), eErrorType.message);
            // End TT#1668
        }

        public string GetText(string id)
        {
            string text = null;
            DataRow[] drText = null;
            drText = dtText.Select("id = '" + id + "'");
            if (drText.Length > 0)
            {
                text = drText[0].Field<string>("text");
                text = text.Replace(@"\n", Environment.NewLine);
            }
            return text;
        }
    }
}
