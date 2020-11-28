using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

using Infragistics.Win.UltraWinTree;


namespace MIDRetail.Windows.Controls
{
    public partial class SelectMultipleMethodTypesControl : UserControl
    {
        public SelectMultipleMethodTypesControl()
        {
            InitializeComponent();
        }

        private void SelectMultipleMethodTypesControl_Load(object sender, EventArgs e)
        {
        }

        DataTable dtMethods;
        public void LoadAllocationData(SessionAddressBlock SAB)
        {
            // load method types
            ReportData reportData = new ReportData();

            DataTable dtMethodTypes = reportData.GetAllocationMethodTypesUsed(SAB.ClientServerSession.UserRID);
            dtMethods = reportData.GetAllocationMethodsUsed(SAB.ClientServerSession.UserRID, true);

            DataSet dsMethodsWithTypes = new DataSet();

            dsMethodsWithTypes.Tables.Add(dtMethodTypes);
            dsMethodsWithTypes.Tables.Add(dtMethods);

            dsMethodsWithTypes.Relations.Add("TypesToMethods", dtMethodTypes.Columns["METHOD_TYPE_ID"], dtMethods.Columns["METHOD_TYPE_ID"], false);

            this.midSelectMultiNodeControl1.ShowRootLines = true;
            this.midSelectMultiNodeControl1.MappingRelationshipColumnKey = "TypesToMethods";

            this.midSelectMultiNodeControl1.FieldToTag = "METHOD_RID";
            this.midSelectMultiNodeControl1.BindDataSet(dsMethodsWithTypes);       
        }

        public void LoadForecastData(SessionAddressBlock SAB)
        {
            // load method types
            ReportData reportData = new ReportData();

            DataTable dtMethodTypes = reportData.GetForecastMethodTypesUsed(SAB.ClientServerSession.UserRID);
            dtMethods = reportData.GetForecastMethodsUsed(SAB.ClientServerSession.UserRID, true);

            DataSet dsMethodsWithTypes = new DataSet();

            dsMethodsWithTypes.Tables.Add(dtMethodTypes);
            dsMethodsWithTypes.Tables.Add(dtMethods);

            dsMethodsWithTypes.Relations.Add("TypesToMethods", dtMethodTypes.Columns["METHOD_TYPE_ID"], dtMethods.Columns["METHOD_TYPE_ID"], false);

            this.midSelectMultiNodeControl1.ShowRootLines = true;
            this.midSelectMultiNodeControl1.MappingRelationshipColumnKey = "TypesToMethods";

            this.midSelectMultiNodeControl1.FieldToTag = "METHOD_RID";
            this.midSelectMultiNodeControl1.BindDataSet(dsMethodsWithTypes);
        }


        public void GetSelectedMethods(ref ReportData.AllocationAnalysisEventArgs e)
        {
            if (this.midSelectMultiNodeControl1.IsEveryNodeSelected() == true)
            {
                e.restrictMethods = false;
            }
            else
            {
                e.restrictMethods = true;
                e.methodRIDsToInclude = this.midSelectMultiNodeControl1.GetSelectedListFromTags(false);
            }
        }

        public void GetSelectedMethods(ref ReportData.ForecastAnalysisEventArgs e)
        {
            if (this.midSelectMultiNodeControl1.IsEveryNodeSelected() == true)
            {
                e.restrictMethods = false;
            }
            else
            {
                e.restrictMethods = true;
                e.methodRIDsToInclude = this.midSelectMultiNodeControl1.GetSelectedListFromTags(false);
            }
        }

      
     

    }
}
