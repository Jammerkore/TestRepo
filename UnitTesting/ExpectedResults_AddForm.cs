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
    public partial class ExpectedResults_AddForm : Form
    {
        public ExpectedResults_AddForm()
        {
            InitializeComponent();
        }

        private void ExpectedResults_AddForm_Load(object sender, EventArgs e)
        {
            this.expectedResult_AddControl1.FireOK_Event += new ExpectedResult_AddControl.FireOK_EventHandler(Handle_OK);
            this.expectedResult_AddControl1.FireCancel_Event += new ExpectedResult_AddControl.FireCancel_EventHandler(Handle_Cancel);
        }
        public bool OK = false;
        private void Handle_OK(object sender, ExpectedResult_AddControl.FireOK_EventArgs e)
        {
            OK = true;
            this.Close();
        }
        private void Handle_Cancel(object sender, ExpectedResult_AddControl.FireCancel_EventArgs e)
        {
            this.Close();
        }
       
    }
}
