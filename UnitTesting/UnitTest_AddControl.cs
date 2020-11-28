using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace UnitTesting
{
    public partial class UnitTest_AddControl : UserControl
    {
        public UnitTest_AddControl()
        {
            InitializeComponent();
            //this.txtTestName.Text = UnitTests.defaultTestName;

            this.unitTestExpectedResultControl1.HideEditButtons();

            //DataSet dsEnvironment = UnitTests.GetEnvironmentsForSelection();
            //this.cboEnvironment.SetDataBinding(dsEnvironment, "Environments");
            //this.cboEnvironment.Text = UnitTests.defaultEnvironment;

        
        }

        private void UnitTest_AddControl_Load(object sender, EventArgs e)
        {
            this.unitTestParameterControl1.InsertFromResultsEvent += new UnitTestParameterControl.InsertFromResultsEventHandler(Handle_InsertFromResultsEvent);

        }


        private string procedureName;
        public void SetProcedureName(string procedureName)
        {
            this.procedureName = procedureName;

         
        }
        public void GenerateTestName()
        {
            string environmentName = UnitTests.defaultEnvironment; //this.cboEnvironment.Text;
            this.txtTestName.Text = UnitTests.GenerateTestNameFromProcedure(environmentName, procedureName);
        }
     
        public void FillParametersFromTest(string testName, string procedureName)
        {
            dsParms = UnitTests.GetParametersForUnitTest(testName, procedureName);
            this.unitTestParameterControl1.BindGrid(testName, procedureName, dsParms);
        }
        private DataSet dsParms;
        public enum EditModes
        {
            Add,
            Edit,
            Copy
        }
        private EditModes editMode;
        public void SetEditMode(EditModes editMode)
        {
            this.editMode = editMode;
        }
        private string oldTestName;
        public void LoadTest(string testName, string procedureName)
        {
            this.dsParms = UnitTests.GetParametersForUnitTest(testName, procedureName);
            this.unitTestParameterControl1.BindGrid(testName, procedureName, dsParms);
            this.unitTestExpectedResultControl1.BindGrid(testName, procedureName);
            this.unitTestParameterControl1.HideSaveButton();
            this.txtTestName.Text = testName;


        

            oldTestName = testName;
            DataRow dr = UnitTests.GetUnitTestFromName(testName, procedureName);
            if (dr != null)
            {
                this.txtTestName.Text = testName;
                //this.cboEnvironment.Text = (string)dr["environmentName"];
                if (dr["sequence"] != DBNull.Value)
                {
                    this.txtSequence.Text = (string)dr["sequence"];
                }
                //string restoreState = (string)dr["restoreState"];
                //if (restoreState == "Y")
                //{
                //    this.chkRestoreState.Checked = true;
                //}
                //else
                //{
                //    this.chkRestoreState.Checked = false;
                //}
            }
        }
        private void Handle_InsertFromResultsEvent(object sender, UnitTestParameterControl.InsertFromResultsEventArgs e)
        {
            this.unitTestExpectedResultControl1.BindGrid(this.txtTestName.Text, procedureName);
        }
        public void FillParametersFromProcedure(string procedureName)
        {
            dsParms = UnitTests.GetTestParametersFromProcedure(procedureName);
            this.unitTestParameterControl1.BindGrid(this.txtTestName.Text, procedureName, dsParms);  
            this.txtTestName.Enabled = false;
            this.unitTestParameterControl1.HideSaveButton();
        }
      

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnOK":
                    if (IsValid())
                    {
                        SaveTest();
                        RaiseFireOK_Event();
                    }
                    break;
                case "btnCancel":
                    RaiseFireCancel_Event();
                    break;
                case "btnScriptParameters":
                    ScriptParameters();
                    break;
                case "btnEditProcedure":
                    EditProcedure();
                    break;

            }
        }
        private void EditProcedure()
        {
            string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\" + procedureName + ".SQL";

            if (System.IO.File.Exists(sPath) == true)
            {
                System.Diagnostics.Process.Start(sPath);
            }
        }
        private void ScriptParameters()
        {
            string paramScriptFileName = procedureName + "_paramScript.SQL";
            string fullpath = string.Empty;

            DataRow drEnv = UnitTests.GetEnvironmentFromName(UnitTests.defaultEnvironment);
            string sbody = "USE [" + (string)drEnv["databaseName"] + "]" + System.Environment.NewLine + System.Environment.NewLine;
            sbody += "EXEC " + procedureName + System.Environment.NewLine;
 
            foreach (DataRow drParam in dsParms.Tables[0].Rows)
            {
                string parameterName = (string)drParam["parameterName"];
                string parameterType = (string)drParam["parameterType"];

                string parameterValue = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (drParam["parameterValue"] != DBNull.Value)
                {
                    parameterValue = (string)drParam["parameterValue"];
                }
                if ((parameterType == "VarChar" || parameterType == "Char" || parameterType=="DateTime") && parameterValue != Include.NullForStringValue)  //TT#1310-MD -jsobek -Error when adding a new Store
                {
                    parameterValue = "'" + parameterValue + "'";
                }
                //else if (parameterType == "DateTime")
                //{
                //    parameterValue = "Convert(datetime, '" + parameterValue + "')";
                //}
                sbody += parameterName + " = " + parameterValue + "," + System.Environment.NewLine;
            }
            baseStoredProcedure bp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
            if (bp.OutputParameters != null)
            {
                foreach (MIDDbParameter p in bp.OutputParameters)
                {
                    sbody += p.ParameterName + " = null," + System.Environment.NewLine;
                }
            }
            sbody = sbody.Substring(0, sbody.Length - 3) + ";";
            

            UnitTests.SaveTempSQLFile(sbody, paramScriptFileName, out fullpath);
            if (System.IO.File.Exists(fullpath) == true)
            {
                System.Diagnostics.Process.Start(fullpath);
            }
        }
        private bool IsValid()
        {
            bool valid = true;
            string newTestName = this.txtTestName.Text.Trim();
            if (this.txtTestName.Text == "")
            {
                valid = false;
                MessageBox.Show("Please provide a test name.");
            }

            if (editMode == EditModes.Add || editMode == EditModes.Copy)
            {
                if (UnitTests.DoesTestAlreadyExistForProcedure(newTestName, this.procedureName))
                {
                    valid = false;
                    MessageBox.Show("Test Name is already defined for this procedure.  Please provide a different name.");
                }
            }
            else if (editMode == EditModes.Edit)
            {
                if (newTestName != oldTestName)
                {
                    if (UnitTests.DoesTestAlreadyExistForProcedure(newTestName, this.procedureName))
                    {
                        valid = false;
                        MessageBox.Show("Test Name is already defined for this procedure.  Please provide a different name.");
                    }
                }
            }
            //string environmentName = this.cboEnvironment.Text;
            //if (UnitTests.DoesEnvironmentExist(environmentName) == false)
            //{
            //    valid = false;
            //    MessageBox.Show("Please select a valid DB.");
            //}
            return valid;
        }
        private void SaveTest()
        {
            string newTestName = this.txtTestName.Text.Trim();
            string environmentName = UnitTests.defaultEnvironment; //this.cboEnvironment.Text;
            string sequence = this.txtSequence.Text;
            string isSuspended;
            if (this.chkSuspend.Checked == true)
            {
                isSuspended = "Y";
            }
            else
            {
                isSuspended = "N";
            }
            this.unitTestParameterControl1.SaveParameters();
            if (editMode == EditModes.Add || editMode == EditModes.Copy)
            {
                UnitTests.AddUnitTest(environmentName, newTestName, this.procedureName, sequence, isSuspended, this.dsParms);
            }
            else if (editMode == EditModes.Edit)
            {


                UnitTests.EditUnitTest(procedureName, oldTestName, newTestName, environmentName, sequence, isSuspended, this.dsParms);
            }

            //UnitTests.defaultTestName = newTestName;
            UnitTests.defaultEnvironment = environmentName;
            //Auto Save
            UnitTests.AutoSave();
           
        }


        public event FireOK_EventHandler FireOK_Event;
        public virtual void RaiseFireOK_Event()
        {

            if (FireOK_Event != null)
                FireOK_Event(this, new FireOK_EventArgs());
        }
        public class FireOK_EventArgs
        {
            public FireOK_EventArgs() {}
        }
        public delegate void FireOK_EventHandler(object sender, FireOK_EventArgs e);
        public event FireCancel_EventHandler FireCancel_Event;
        public virtual void RaiseFireCancel_Event()
        {

            if (FireCancel_Event != null)
                FireCancel_Event(this, new FireCancel_EventArgs());
        }
        public class FireCancel_EventArgs
        {
            public FireCancel_EventArgs() { }
        }
        public delegate void FireCancel_EventHandler(object sender, FireCancel_EventArgs e);

    }
}
