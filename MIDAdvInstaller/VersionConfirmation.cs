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
    public partial class VersionConfirmation : Form
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
        //public VersionConfirmation()
        //{
        //    InitializeComponent();
        //    this.Load += new EventHandler(VersionConfirmation_Load);
        //    // add texts here
        //}
        public VersionConfirmation(InstallerFrame p_frm)
        {
            InitializeComponent();
            _frame = p_frm;
            this.Load += new EventHandler(VersionConfirmation_Load);
            // add texts here
        }
        // End TT#74 MD

        private void VersionConfirmation_Load(object sender, System.EventArgs e)
        {
            //  onload
            //      disable the Continue button
            //      clear the checkbox
            //      enable Terminate button
            cbConfirm.Checked = false;
            cbConfirm.CheckState = CheckState.Unchecked;
            cbConfirm.Update();
            btnContinue.Enabled = false;
            btnTerminate.Enabled = true;
            // get installer data
            installer_data = _frame.GetInstallerData();
            dtText = installer_data.Tables["install_text"];
            // GetTexts
            this.lblWarningMessage.Text = GetText("lblWarningMessage");
            this.lblWarning.Text = GetText("lblWarning");
            this.cbConfirm.Text = GetText("cbConfirm");
            this.btnContinue.Text = GetText("btnContinue");
            this.btnTerminate.Text = GetText("btnTerminate");
        }

        private void cbConfirm_CheckedChanged(object sender, EventArgs e)
        {
            //   relase the continue button if the check box has been checked
            if (((CheckBox)sender).CheckState == CheckState.Checked)
            {
                btnContinue.Enabled = true;              
            }
            else
            {
                btnContinue.Enabled = false;
            }
        }
    
        //  Continue the Installation
        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            // Begin TT#1668 - JSmith - Install Log
            _frame.SetLogMessage(GetText("verConfirmed"), eErrorType.message);
            // End TT#1668
        }

        //  Terminate the Installation
        private void btnTerminate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            // Begin TT#1668 - JSmith - Install Log
            _frame.SetLogMessage(GetText("verTerminated"), eErrorType.message);
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
