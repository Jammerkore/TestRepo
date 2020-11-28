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

namespace MIDRetail.Windows.Controls
{
    public partial class SelectMultipleHeaderStatusesControl : UserControl
    {
        public SelectMultipleHeaderStatusesControl()
        {
            InitializeComponent();
        }

        private void SelectMultipleHeaderStatusesControl_Load(object sender, EventArgs e)
        {
        }

        DataTable dtStatus;
        public void LoadAllocationData(SessionAddressBlock SAB)
        {
            // load header status
            eHeaderAllocationStatus status;
            dtStatus = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
            for (int i = dtStatus.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dRow = dtStatus.Rows[i];

                status = (eHeaderAllocationStatus)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
                //text = Convert.ToString(dRow["TEXT_VALUE"], CultureInfo.CurrentUICulture);

                if (status == eHeaderAllocationStatus.InUseByMultiHeader)
                {
                    //selected = multiChecked;
                }

                // if size, use all statuses
                if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                   // lstStatus.Items.Add(new MIDListBoxItem(text, status), selected);
                }
                else
                {
                    // remove all size statuses
                    if (Enum.IsDefined(typeof(eNonSizeHeaderAllocationStatus), Convert.ToInt32(status, CultureInfo.CurrentUICulture)))
                    {
                        // lstStatus.Items.Add(new MIDListBoxItem(text, status), selected);
                    }
                    else
                    {
                        dtStatus.Rows.Remove(dRow);
                    }

                }


            }
      

            DataSet dsHeaderStatuses = new DataSet();

            dsHeaderStatuses.Tables.Add(dtStatus);
       

            this.midSelectMultiNodeControl1.BindDataSet(dsHeaderStatuses);

           
        }

       




        public void GetSelectedStatuses(ref ReportData.AllocationAnalysisEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode n in this.midSelectMultiNodeControl1.ultraTree1.Nodes)
            {
                string s = n.Text;
                bool isChecked;
                  if (n.CheckedState == CheckState.Checked)
                  {
                        isChecked = true;
                  }
                  else
                  {
                        isChecked = false;
                  }


                DataRow[] drFind = dtStatus.Select("TEXT_VALUE='" + s + "'");
                if (drFind.Length > 0)
                {
                    eHeaderAllocationStatus status = (eHeaderAllocationStatus)Convert.ToInt32(drFind[0]["TEXT_CODE"], CultureInfo.CurrentUICulture);

                    if (status == eHeaderAllocationStatus.ReceivedOutOfBalance)
                    {
                        e.includeHeaderStatus_ReceivedOutOfBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.ReceivedInBalance)
                    {
                        e.includeHeaderStatus_ReceivedInBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.InUseByMultiHeader)
                    {
                        e.includeHeaderStatus_InUseByMultiHeader = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.PartialSizeOutOfBalance)
                    {
                        e.includeHeaderStatus_PartialSizeOutOfBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.PartialSizeInBalance)
                    {
                        e.includeHeaderStatus_PartialSizeInBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.AllocatedOutOfBalance)
                    {
                        e.includeHeaderStatus_AllocatedOutOfBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.AllocatedInBalance)
                    {
                        e.includeHeaderStatus_AllocatedInBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.SizesOutOfBalance)
                    {
                        e.includeHeaderStatus_SizesOutOfBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.AllInBalance)
                    {
                        e.includeHeaderStatus_AllInBalance = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.Released)
                    {
                        e.includeHeaderStatus_Released = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.ReleaseApproved)
                    {
                        e.includeHeaderStatus_ReleaseApproved = isChecked;
                    }
                    else if (status == eHeaderAllocationStatus.AllocationStarted)
                    {
                        e.includeHeaderStatus_AllocationStarted = isChecked;
                    }
                }
            }



        }

       
    }
}
