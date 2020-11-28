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
    public partial class SelectMultipleHeaderTypesControl : UserControl
    {
        public SelectMultipleHeaderTypesControl()
        {
            InitializeComponent();
        }

        private void SelectMultipleHeaderTypesControl_Load(object sender, EventArgs e)
        {
        }

        private DataTable dtTypes;
        public void LoadData(SessionAddressBlock SAB)
        {
            // load header type
            eHeaderType type;
            //string text = string.Empty;
            //bool selected = false;
            //bool multiChecked = false;
            dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));

         
                //bool phRemoved = false, asrtRemoved = false;
                for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dRow = dtTypes.Rows[i];

                    if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                    {
                    if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
                    {
                        dtTypes.Rows.Remove(dRow);
                        //asrtRemoved = true;
                    }
                    else if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
                    {
                        dtTypes.Rows.Remove(dRow);
                        //phRemoved = true;
                    }
                    }

                    type = (eHeaderType)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    //text = Convert.ToString(dRow["TEXT_VALUE"], CultureInfo.CurrentUICulture);
                    // if size, use all statuses
                    if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                    {
                        //lstTypes.Items.Add(new MIDListBoxItem(text, type), selected);
                    }
                    else
                    {
                        // remove all size statuses
                        if (Enum.IsDefined(typeof(eNonSizeHeaderType), Convert.ToInt32(type, CultureInfo.CurrentUICulture)))
                        {
                            //lstTypes.Items.Add(new MIDListBoxItem(text, type), selected);
                        }
                        else
                        {
                            dtTypes.Rows.Remove(dRow);
                        }
                    }
                    //if (asrtRemoved && phRemoved)
                    //{
                    //    break;
                    //}
                }
      

            DataSet dsHeaderTypes = new DataSet();

            dsHeaderTypes.Tables.Add(dtTypes);
       

            this.midSelectMultiNodeControl1.BindDataSet(dsHeaderTypes);

           
        }


        public void GetSelectedTypes(ref ReportData.AllocationAnalysisEventArgs e)
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


                DataRow[] drFind = dtTypes.Select("TEXT_VALUE='" + s + "'");
                if (drFind.Length > 0)
                {
                    eHeaderType type = (eHeaderType)Convert.ToInt32(drFind[0]["TEXT_CODE"], CultureInfo.CurrentUICulture);

                    if (type == eHeaderType.ASN)
                    {
                        e.includeHeaderType_ASN = isChecked;
                    }
                    else if (type == eHeaderType.Assortment)
                    {
                        e.includeHeaderType_Assortment = isChecked;
                    }
                    else if (type == eHeaderType.DropShip)
                    {
                        e.includeHeaderType_DropShip = isChecked;
                    }
                    else if (type == eHeaderType.Dummy)
                    {
                        e.includeHeaderType_Dummy = isChecked;
                    }
                    else if (type == eHeaderType.IMO)
                    {
                        e.includeHeaderType_IMO = isChecked;
                    }
                    else if (type == eHeaderType.MultiHeader)
                    {
                        e.includeHeaderType_MultiHeader = isChecked;
                    }
                    else if (type == eHeaderType.Placeholder)
                    {
                        e.includeHeaderType_Placeholder = isChecked;
                    }
                    else if (type == eHeaderType.PurchaseOrder)
                    {
                        e.includeHeaderType_PurchaseOrder = isChecked;
                    }
                    else if (type == eHeaderType.Receipt)
                    {
                        e.includeHeaderType_Receipt = isChecked;
                    }
                    else if (type == eHeaderType.Reserve)
                    {
                        e.includeHeaderType_Reserve = isChecked;
                    }
                    else if (type == eHeaderType.WorkupTotalBuy)
                    {
                        e.includeHeaderType_WorkupTotalBuy = isChecked;
                    }
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    else if (type == eHeaderType.Master)
                    {
                        e.includeHeaderType_Master = isChecked;
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment


                }






            }



        }
    }
}
