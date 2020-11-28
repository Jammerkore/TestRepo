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
    public partial class EnvironmentControl : UserControl
    {
        public EnvironmentControl()
        {
            InitializeComponent();
            this.BindGrid(UnitTests.GetEnvironmentsForEditing());
        }

        private void EnvironmentControl_Load(object sender, EventArgs e)
        {
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
            //rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            //rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            //this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;


            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["environmentName"].Header.Caption = "Environment";
            e.Layout.Bands[0].Columns["connectionString"].Hidden = true;
            e.Layout.Bands[0].Columns["server"].Header.Caption = "Server";
            e.Layout.Bands[0].Columns["databaseName"].Header.Caption = "DB";
            e.Layout.Bands[0].Columns["bakFilePath"].Hidden = true;
        }

        public void BindGrid(DataSet aDataSet)
        {

            ultraGrid1.DataSource = null;

            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);

            SelectFirstRow();
        }

        private void SelectFirstRow()
        {
            if (this.ultraGrid1.Rows.Count > 0)
            {
                this.ultraGrid1.Rows[0].Selected = true;

            }
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnAdd":
                    AddEnvironment();
                    break;
                case "btnEdit":
                    EditEnvironment();
                    break;
                case "btnDelete":
                    DeleteEnvironment();
                    break;
            }
        }

        private void AddEnvironment()
        {
            Environment_AddForm frm = new Environment_AddForm();
            frm.environment_AddControl1.SetEditMode(Environment_AddControl.EditModes.Add);
            frm.ShowDialog();
        }

        private void EditEnvironment()
        {
            if (this.ultraGrid1.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Please first select an environment to edit.");
                return;
            }

            Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
            if (urFirst.ListObject != null)
            {
                DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                Environment_AddForm frm = new Environment_AddForm();
                frm.Text = "Edit Environment";
                frm.environment_AddControl1.SetEditMode(Environment_AddControl.EditModes.Edit);
                frm.environment_AddControl1.LoadEnvironment((string)drFirstSelected["environmentName"]);
                frm.ShowDialog();
               
            }
        }

        private void DeleteEnvironment()
        {            
            if (this.ultraGrid1.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Please first select an environment to delete.");
                return;
            }
            
            Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
            if (urFirst.ListObject != null)
            {
                DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                string environmentName = (string)drFirstSelected["environmentName"];
                int testCount = UnitTests.CountTestsForEnvironment(environmentName);
                if (MessageBox.Show("Warning!" + System.Environment.NewLine + testCount.ToString() +  " associated tests will also be deleted." + System.Environment.NewLine + "Do you wish to proceed??", "Warning - Please Confirm:", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    UnitTests.DeleteEnvironment(environmentName);
                }
            }
        }

       
    }
}
