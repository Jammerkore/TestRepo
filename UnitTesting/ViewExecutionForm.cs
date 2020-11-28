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
    public partial class ViewExecutionForm : Form
    {
        public ViewExecutionForm()
        {
            InitializeComponent();
        }

        public void BindGrid(string testName, string procedureName, DataSet aDataSet)
        {
            this.viewExecutionGridControl1.BindGrid(testName, procedureName, aDataSet);
        }
        public void BindGrid(string testName, string procedureName, DataTable aDataTable)
        {
            this.viewExecutionGridControl1.BindGrid(testName, procedureName, aDataTable);
        }
        public void BindOutputParameters(string testName, string procedureName, DataSet dsOutputParameters)
        {
            this.viewExecutionOutputParameters1.BindGrid(testName, procedureName, dsOutputParameters);
        }

        private void ViewExecutionForm_Load(object sender, EventArgs e)
        {
            this.viewExecutionGridControl1.InsertFromResultsEvent +=new ViewExecutionGridControl.InsertFromResultsEventHandler(Handle_InsertFromResultsEvent);
            this.viewExecutionOutputParameters1.InsertFromResultsEvent += new ViewExecutionOutputParameters.InsertFromResultsEventHandler(Handle_InsertFromResultsEvent2);
        }

        public bool didInsertFromResults = false;
        private void Handle_InsertFromResultsEvent(object sender, ViewExecutionGridControl.InsertFromResultsEventArgs e)
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
