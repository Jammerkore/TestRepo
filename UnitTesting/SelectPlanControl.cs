using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace UnitTesting
{
    public partial class SelectPlanControl : UserControl
    {
        public SelectPlanControl()
        {
            InitializeComponent();
            
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnOK":
                    doOK();
                    break;
                case "btnCancel":
                    UnitTests.RaiseSelectPlan_Cancel_Event(this);
                    break;

            }
        }
        private bool IsValid()
        {
            bool valid = true;

            if (IsValidFolderName(this.txtPlan.Text) == false)
            {
                valid = false;
                MessageBox.Show("Please enter a valid plan folder name.");
            }
            else
            {
                if (System.IO.Directory.Exists(UnitTests.globalPlanFilePath + this.txtPlan.Text) == true)
                {
                    if (MessageBox.Show("This plan already exists, do you wish to override?", "Override?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    valid = false;
                }
            }


            return valid;
        }
        bool IsValidFolderName(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(System.IO.Path.GetInvalidPathChars().ToString()) + "]");
            if (containsABadCharacter.IsMatch(testName))
            { 
                return false; 
            };

            return true;
        }

    


        private void doOK()
        {
            if (IsValid() == true)
            {
                UnitTests.RaiseSelectPlan_OK_Event(this, this.txtPlan.Text);
            }
        }
        private void txtPlan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                doOK();
            }
        }
    }
    
}
