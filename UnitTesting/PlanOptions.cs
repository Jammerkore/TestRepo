using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTesting
{
    public partial class PlanOptions : UserControl
    {
        public PlanOptions()
        {
            InitializeComponent();
        }
        public void SetPlanName(string planName)
        {
            this.txtPlan.Text = planName;
            this.txtFolderPath.Text = UnitTests.globalPlanFilePath + planName + "\\";
        }

        private void chkUpgradeStandardDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkUpgradeStandardDatabase.Checked == true)
            {
                this.chkUseUpgradedDatabaseForUnitTests.Checked = true;
                this.chkUseUpgradedDatabaseForUnitTests.Enabled = true;
                this.chkRemoveUpgradeDBOnFailure.Enabled = true;
                this.chkRemoveUpgradeDBOnSuccess.Enabled = true;
            }
            else
            {
                this.chkUseUpgradedDatabaseForUnitTests.Checked = false;
                this.chkUseUpgradedDatabaseForUnitTests.Enabled = false;
                this.chkRemoveUpgradeDBOnFailure.Enabled = false;
                this.chkRemoveUpgradeDBOnSuccess.Enabled = false;
            }
        }
        private void chkNewDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkCreateNewDatabase.Checked == true)
            {

                this.chkRemoveNewDBOnFailure.Enabled = true;
                this.chkRemoveNewDBOnSuccess.Enabled = true;
            }
            else
            {
                this.chkRemoveNewDBOnFailure.Enabled = false;
                this.chkRemoveNewDBOnSuccess.Enabled = false;
            }
        }

        private void chkRunUnitTests_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkRunUnitTests.Checked == true)
            {
                this.chkStopOnFirstUnitTestFailure.Enabled = true;
                this.chkDeletePriorResultFolders.Enabled = true;
            }
            else
            {
                this.chkStopOnFirstUnitTestFailure.Enabled = false;
                this.chkDeletePriorResultFolders.Enabled = false;
            }
        }
    }
}
