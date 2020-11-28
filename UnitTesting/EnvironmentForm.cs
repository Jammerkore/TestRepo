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
    public partial class EnvironmentForm : Form
    {
        public EnvironmentForm()
        {
            InitializeComponent();
        }

        private void EnvironmentForm_Load(object sender, EventArgs e)
        {
            //this.environment_AddControl1.FireOK_Event += new Environment_AddControl.FireOK_EventHandler(Handle_OK);
            //this.environment_AddControl1.FireCancel_Event += new Environment_AddControl.FireCancel_EventHandler(Handle_Cancel);
        }
        private void Handle_OK(object sender, Environment_AddControl.FireOK_EventArgs e)
        {
            //UnitTests.SaveTestFiles(false);
            this.Close();
        }
        private void Handle_Cancel(object sender, Environment_AddControl.FireCancel_EventArgs e)
        {
            this.Close();
        }
    }
}
