using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTesting
{
    public partial class SelectPlanForm : Form
    {
        public SelectPlanForm()
        {
            InitializeComponent();
            radNewPlan.Checked = true;
        }
        public void AllowOnlyExistingPlans()
        {
            radExistingPlan.Checked = true;
            radNewPlan.Enabled = false;
        }
        private void SelectPlanForm_Load(object sender, EventArgs e)
        {
            //this.selectPlanControl1.SelectPlan_OK_Event += new SelectPlanControl.SelectPlan_OK_EventHandler(Handle_OK);
            //this.selectPlanControl1.SelectPlan_Cancel_Event += new SelectPlanControl.SelectPlan_Cancel_EventHandler(Handle_Cancel);
            UnitTests.SelectPlan_OK_Event += new UnitTests.SelectPlan_OK_EventHandler(Handle_OK);
            UnitTests.SelectPlan_Cancel_Event += new UnitTests.SelectPlan_Cancel_EventHandler(Handle_Cancel);
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {

                case "btnOK":
                    //string environmentName = this.ultraCombo1.Text;
                    //if (UnitTests.DoesEnvironmentExist(environmentName) == false)
                    //{
                    //    MessageBox.Show("Please select a valid DB.");
                    //}
                    //UnitTests.defaultEnvironment = environmentName;
                    //doOK();
                    break;
                case "btnCancel":
                    //RaiseSelectPlan_Cancel_Event();
                    break;

            }
        }


        public string planName = string.Empty;
        public bool isOK = false;
        private void Handle_OK(object sender, UnitTests.SelectPlan_OK_EventArgs e)
        {
            planName = e.planName;
            isOK = true;
            this.Close();
        }
        private void Handle_Cancel(object sender, UnitTests.SelectPlan_Cancel_EventArgs e)
        {
            this.Close();
        }

        private void radNewPlan_CheckedChanged(object sender, EventArgs e)
        {
            planSelectionChanged();
        }

        private void radExistingPlan_CheckedChanged(object sender, EventArgs e)
        {
            planSelectionChanged();
        }
        private void planSelectionChanged()
        {
            if (panel2.Controls.Count > 0)
            {
                panel2.Controls.RemoveAt(0);
            }
            if (radNewPlan.Checked == true)
            {
                SelectPlanControl c = new SelectPlanControl();
                panel2.Controls.Add(c);
                c.Dock = DockStyle.Fill;
            }
            else if (radExistingPlan.Checked == true)
            {
                SelectExistingPlanControl c = new SelectExistingPlanControl();
                panel2.Controls.Add(c);
                c.Dock = DockStyle.Fill;
            }
        }
    }
}
