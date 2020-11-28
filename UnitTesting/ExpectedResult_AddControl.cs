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
    public partial class ExpectedResult_AddControl : UserControl
    {
        public ExpectedResult_AddControl()
        {
            InitializeComponent();
        }
      
        public enum EditModes
        {
            Add,
            Edit
        }
        private EditModes editMode;
        public void SetEditMode(EditModes editMode)
        {
            this.editMode = editMode;
        }
        private string testName;
        private string procedureName;
        private UnitTests.ExpectedResultKinds resultKind;
        public void SetExpectedResultKind(string testName, string procedureName, UnitTests.ExpectedResultKinds resultKind)
        {
            this.testName = testName;
            this.procedureName = procedureName;
            this.resultKind = resultKind;

            if (resultKind == UnitTests.ExpectedResultKinds.FieldEquals)
            {
                this.txtExpectedField.Enabled = true;
            }
            else
            {
                this.txtExpectedField.Enabled = false;
            }
        }

        public void LoadExpectedResult(DataRow dr)
        {
            if (dr != null)
            {
                this.txtExpectedField.Text = (string)dr["fieldName"];
                this.txtExpectedValue.Text = (string)dr["expectedValue"];
            }
        }


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnOK":
                    if (IsValid())
                    {
                        SaveExpectedResult();
                        RaiseFireOK_Event();
                    }
                    break;
                case "btnCancel":
                    RaiseFireCancel_Event();
                    break;

            }
        }

        private bool IsValid()
        {
            bool valid = true;
            if (resultKind == UnitTests.ExpectedResultKinds.FieldEquals)
            {
                string tempField = this.txtExpectedField.Text.Trim();
                if (tempField == string.Empty)
                {
                    valid = false;
                    MessageBox.Show("Please provide a field name.");
                }
            }
           
            string tempValue = this.txtExpectedValue.Text.Trim();
            if (tempValue == string.Empty)
            {
                valid = false;
                MessageBox.Show("Please provide an expected value.");
            }

            if (resultKind == UnitTests.ExpectedResultKinds.RowCountEqualsX)
            {
                int rowCount;
                if (int.TryParse(tempValue, out rowCount) == false)
                {
                    valid = false;
                    MessageBox.Show("Please provide a valid integer row count.");
                }
            }

            return valid;
        }
        public string expectedField = string.Empty;
        public string expectedValue = string.Empty;
        private void SaveExpectedResult()
        {
            expectedField = this.txtExpectedField.Text.Trim();
            expectedValue = this.txtExpectedValue.Text.Trim();
            //if (editMode == EditModes.Add)
            //{
            //    UnitTests.Expec
            //}
            //else if (editMode == EditModes.Edit)
            //{
            //    UnitTests.EditEnvironment(oldEnvironmentName, newEnvironmentName, connectionString);
            //}
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
