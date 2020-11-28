using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;

namespace UnitTesting
{
    public partial class ViewExecutionRowCountControl : UserControl
    {
        public ViewExecutionRowCountControl()
        {
            InitializeComponent();

            DataSet dsEnvironment = UnitTests.GetEnvironmentsForSelection();
         
        }
        public void HideButton_InsertRowCount()
        {
            this.ultraToolbarsManager1.Tools["btnInsertRowCount"].SharedProps.Visible = false;
        }
        public void ShowButton_InsertCompareValue()
        {
            this.ultraToolbarsManager1.Tools["btnInsertCompareValue"].SharedProps.Visible = true;
        }
        object scalarValue = null;
        public void SetScalarValue(object obj)
        {
            scalarValue = obj;
        }
     
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
         
                case "btnInsertRowCount":
                    InsertRowCount();
                    break;
                case "btnInsertCompareValue":
                    InsertCompareValue();
                    break;
      
            }
        }
 
        private string testName;
        private string procedureName;
        int rowCount;
        public void SetMessage(string testName, string procedureName, string message, int rowCount)
        {
            this.testName = testName;
            this.procedureName = procedureName;
            this.lblMessage.Text = message;
            this.rowCount = rowCount;
        }
      

        private void InsertRowCount()
        {
            UnitTests.ExpectedResults_AddRowCountEqualsX(testName, procedureName, rowCount);
            RaiseInsertFromResultsEvent();   
        }
        private void InsertCompareValue()
        {
            UnitTests.ExpectedResults_AddCompareValue(testName, procedureName, scalarValue);
            RaiseInsertFromResultsEvent();
        }

        public event InsertFromResultsEventHandler InsertFromResultsEvent;
        public virtual void RaiseInsertFromResultsEvent()
        {

            if (InsertFromResultsEvent != null)
                InsertFromResultsEvent(this, new InsertFromResultsEventArgs());
        }
        public class InsertFromResultsEventArgs
        {
            public InsertFromResultsEventArgs() { }
        }
        public delegate void InsertFromResultsEventHandler(object sender, InsertFromResultsEventArgs e);
    }
}
