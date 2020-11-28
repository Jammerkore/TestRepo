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
    public partial class ViewExecutionRowCountForm : Form
    {
        public ViewExecutionRowCountForm()
        {
            InitializeComponent();
        }

        public void SetMessage(string testName, string procedureName, string message, int rowCount)
        {
            this.viewExecutionRowCountControl1.SetMessage(testName, procedureName, message, rowCount);
        }
        public void HideButton_InsertRowCount()
        {
            this.viewExecutionRowCountControl1.HideButton_InsertRowCount();
        }
        public void ShowButton_InsertCompareValue()
        {
            this.viewExecutionRowCountControl1.ShowButton_InsertCompareValue();
        }
        public void SetScalarValue(object obj)
        {
            this.viewExecutionRowCountControl1.SetScalarValue(obj);
        }
        public void HideOutputParameters()
        {
            this.viewExecutionOutputParameters1.Visible = false;
            this.ultraSplitter1.Visible = false;
        }
        public void BindOutputParameters(string testName, string procedureName, DataSet dsOutputParameters)
        {
            this.viewExecutionOutputParameters1.BindGrid(testName, procedureName, dsOutputParameters);
        }
      
        private void ViewExecutionRowCountForm_Load(object sender, EventArgs e)
        {
            this.viewExecutionRowCountControl1.InsertFromResultsEvent += new ViewExecutionRowCountControl.InsertFromResultsEventHandler(Handle_InsertFromResultsEvent);
            this.viewExecutionOutputParameters1.InsertFromResultsEvent += new ViewExecutionOutputParameters.InsertFromResultsEventHandler(Handle_InsertFromResultsEvent2);
        }

        public bool didInsertFromResults = false;
        private void Handle_InsertFromResultsEvent(object sender, ViewExecutionRowCountControl.InsertFromResultsEventArgs e)
        {
            this.didInsertFromResults = true;
            this.Close();
        }
        private void Handle_InsertFromResultsEvent2(object sender, ViewExecutionOutputParameters.InsertFromResultsEventArgs e)
        {
            this.didInsertFromResults = true;
            this.Close();
        }
    }
}
