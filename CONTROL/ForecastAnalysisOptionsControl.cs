using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class ForecastAnalysisOptionsControl : UserControl
    {
        public ForecastAnalysisOptionsControl()
        {
            InitializeComponent();
        }
        private SessionAddressBlock _SAB;
        public void LoadData(SessionAddressBlock SAB)
        {
            _SAB = SAB;
            this.selectSingleHierarchyNodeControl1.LoadData(SAB, SelectSingleHierarchyNodeControl.AllowedNodeLevel.Any);
            this.selectSingleHierarchyNodeControl1.NodeChangedEvent += new SelectSingleHierarchyNodeControl.NodeChangedEventHandler(HierarchyNodeChanged);
        }
        private void HierarchyNodeChanged(object sender, SelectSingleHierarchyNodeControl.NodeChangedEventArgs e)
        {
            this.selectHierarchyLowLevelControl1.LoadData(_SAB, e.NodeRID);
        }
        public void GetOptions(ref ReportData.ForecastAnalysisEventArgs e)
        {
         
            if ((string)osMerchandise.CheckedItem.DataValue == "Restrict")
            {
                e.restrictToDescendantsOf_HN_RID = this.selectSingleHierarchyNodeControl1.GetNode();
            }
            if ((string)osLowLevel.CheckedItem.DataValue == "Restrict")
            {
                e.restrictToLowerLevelSequence = this.selectHierarchyLowLevelControl1.GetLowLevelSequence();
            }
          

           

            if ((string)osResultLimit.CheckedItem.DataValue == "Limit")
            {
                int resultLimit = -1;
                int.TryParse(this.txtResultLimit.Text, out resultLimit);
                e.resultLimit = resultLimit;
            }

        }

     


        private void osMerchandise_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osMerchandise.CheckedItem.DataValue == "Restrict")
            {
                this.selectSingleHierarchyNodeControl1.Enabled = true;
                this.osLowLevel.Enabled = true;
            }
            else
            {
                this.selectSingleHierarchyNodeControl1.Enabled = false;
                this.osLowLevel.CheckedIndex = 0;
                this.osLowLevel.Enabled = false;
            }
        }

        private void osResultLimit_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osResultLimit.CheckedItem.DataValue == "Limit")
            {
                this.txtResultLimit.Enabled = true;
            }
            else
            {
                this.txtResultLimit.Enabled = false;
            }
        }

        private void osLowLevel_ValueChanged(object sender, EventArgs e)
        {
            if ((string)osLowLevel.CheckedItem.DataValue == "Restrict")
            {
                this.selectHierarchyLowLevelControl1.Enabled = true;
            }
            else
            {
                this.selectHierarchyLowLevelControl1.Enabled = false;
            }
        }
    }
}
