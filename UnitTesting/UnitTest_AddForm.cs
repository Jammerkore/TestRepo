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
    public partial class UnitTest_AddForm : Form
    {
        public UnitTest_AddForm()
        {
            InitializeComponent();
        }

        private void UnitTest_AddForm_Load(object sender, EventArgs e)
        {
            this.unitTest_AddControl1.FireOK_Event += new UnitTest_AddControl.FireOK_EventHandler(Handle_OK);
            this.unitTest_AddControl1.FireCancel_Event += new UnitTest_AddControl.FireCancel_EventHandler(Handle_Cancel);
        }

        private void Handle_OK(object sender, UnitTest_AddControl.FireOK_EventArgs e)
        {
            this.Close();
        }
        private void Handle_Cancel(object sender, UnitTest_AddControl.FireCancel_EventArgs e)
        {
            this.Close();
        }

        //public void FillParametersFromTest(string testName, string procedureName)
        //{
        //    this.unitTest_AddControl1.FillParametersFromTest(testName, procedureName);
        //}
        //public void FillParametersFromProcedure(string procedureName)
        //{
        //    this.unitTest_AddControl1.FillParametersFromProcedure(procedureName);
        //}
        //public void SetProcedureName(string procedureName)
        //{
        //    this.unitTest_AddControl1.SetProcedureName(procedureName);
        //}
        //public void LoadTest(string testName, string procedureName)
        //{
        //    this.unitTest_AddControl1.LoadTest(testName, procedureName);
        //}
    }
}
