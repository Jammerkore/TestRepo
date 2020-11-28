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
    public partial class PlanOptionDB : UserControl
    {
        public PlanOptionDB()
        {
            InitializeComponent();

            DataSet dsEnvironment = UnitTests.GetEnvironmentsForSelection();
            this.cboEnvironment.SetDataBinding(dsEnvironment, "Environments");
            this.cboEnvironment.Text = UnitTests.defaultEnvironment;
            
        }

        private void ultraCombo1_RowSelected(object sender, Infragistics.Win.UltraWinGrid.RowSelectedEventArgs e)
        {
            string environmentName = this.cboEnvironment.Text;
            if (UnitTests.DoesEnvironmentExist(environmentName) == true)
            {
               DataRow drEnv = UnitTests.GetEnvironmentFromName(environmentName);
               this.txtConnectionString.Text = (string)drEnv["connectionString"];
               if (drEnv.Table.Columns.Contains("server"))
               {
                   //this.txtServer.Text = (string)drEnv["server"];
                   //this.txtDatabaseName.Text = (string)drEnv["databaseName"];
                   this.txtBAKFile.Text = (string)drEnv["bakFilePath"];
               }
            }
        }
       
    }
}
