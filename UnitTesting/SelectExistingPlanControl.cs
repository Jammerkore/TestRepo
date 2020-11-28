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
    public partial class SelectExistingPlanControl : UserControl
    {
        public SelectExistingPlanControl()
        {
            InitializeComponent();

            DataSet dsPlans = new DataSet();
            dsPlans.Tables.Add("Plans");
            dsPlans.Tables[0].Columns.Add("Plan");
            dsPlans.Tables[0].Columns.Add("Path");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(UnitTests.globalPlanFilePath);
            foreach (System.IO.DirectoryInfo diPlan in di.GetDirectories())
            {
                DataRow drPlan = dsPlans.Tables[0].NewRow();
                drPlan["Plan"] = diPlan.Name;
                drPlan["Path"] = diPlan.FullName;
                dsPlans.Tables[0].Rows.Add(drPlan);
            }
            BindGrid(dsPlans);
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
        private void SelectExistingPlanControl_Load(object sender, EventArgs e)
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
            e.Layout.Bands[0].Columns["Plan"].Header.Caption = "Plan";
            e.Layout.Bands[0].Columns["Path"].Header.Caption = "Path";
       
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {

                case "btnOK":
                    //string environmentName = this.ultraCombo1.Text;
                    //if (UnitTests.DoesEnvironmentExist(environmentName) == false)
                    //{
                    //    MessageBox.Show("Please select a valid DB.");
                    //}
                    //UnitTests.defaultEnvironment = environmentName;
                    doOK();
                    break;
                case "btnCancel":
                    UnitTests.RaiseSelectPlan_Cancel_Event(this);
                    break;

            }
        }



        private void doOK()
        {
            bool OK = false;
            if (this.ultraGrid1.Selected.Rows.Count > 0)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
                if (urFirst.ListObject != null)
                {
                    DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                    string planName = (string)drFirstSelected["Plan"];
                    OK = true;
                    UnitTests.RaiseSelectPlan_OK_Event(this, planName);
                }
            }

            if (OK == false)
            {
                MessageBox.Show("Please select a plan.");
            }
        }
        


    }
    
}
