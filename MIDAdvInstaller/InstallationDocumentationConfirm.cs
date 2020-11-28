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
    public partial class InstallationDocumentationConfirm : Form
    {

        //  installer frame
        // Begin TT#74 MD - JSmith - One-button Upgrade
        //InstallerFrame _frame = new InstallerFrame();
        InstallerFrame _frame;
        StreamWriter _installLog;
        // End TT#74 MD

        //  installer data
        public DataSet installer_data;

        DataTable dtText = null;

        // Begin TT#74 MD - JSmith - One-button Upgrade
        //public InstallationDocumentationConfirm()
        //{
        //    InitializeComponent();
        //    this.Load += new EventHandler(InstallationDocumentationConfirm_Load);
        //    // add texts here
        //}
        public InstallationDocumentationConfirm(InstallerFrame p_frm)
        {
            InitializeComponent();
            _frame = p_frm;
            this.Load += new EventHandler(InstallationDocumentationConfirm_Load);
            // add texts here
        }
        // End TT#74 MD

        private void InstallationDocumentationConfirm_Load(object sender, System.EventArgs e)
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
            this.lblWarningMessage.Text = GetText("lblWarningMessageN");
            this.lblWarningMessageN2.Text = GetText("lblWarningMessageN2");
            this.cbConfirmN.Text = GetText("cbConfirmN");
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
            _frame.SetLogMessage(GetText("docConfirmed"), eErrorType.message);
            // End TT#1668
        }

        //  Terminate the Installation
        private void btnTerminate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            // Begin TT#1668 - JSmith - Install Log
            _frame.SetLogMessage(GetText("docTerminated"), eErrorType.message);
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
                // BEGIN TT#1267 - gtaylor
                text = text.Replace(@"\n", Environment.NewLine);
                // END TT#1267
            }
            return text;
        }
    }
}
