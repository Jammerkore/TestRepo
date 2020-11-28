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
    public partial class ReportAllocationByStore : UserControl
    {
        public ReportAllocationByStore()
        {
            InitializeComponent();
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "reportView":
                    ViewReport();
                    break;

            }
        }


        private void ViewReport()
        {
            if (lstTypes.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a header type.", string.Empty);
                return;
            }
            if (lstStatus.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a header status.", string.Empty);
                return;
            }
            int storeRID = -1;
            int storeSelectedIndex = this.cboStore.SelectedIndex;

            if (storeSelectedIndex == -1)
            {
                MessageBox.Show("Please select a store.", string.Empty);
                return;
            }


            storeRID = this.dictionaryStoreRIDs[storeSelectedIndex];


            bool includeHeaderType_ASN              = false;
            bool includeHeaderType_Assortment       = false;
            bool includeHeaderType_DropShip         = false;
            bool includeHeaderType_Dummy            = false;
            bool includeHeaderType_IMO              = false;
            bool includeHeaderType_MultiHeader      = false;
            bool includeHeaderType_Placeholder      = false;
            bool includeHeaderType_PurchaseOrder    = false;
            bool includeHeaderType_Receipt          = false;
            bool includeHeaderType_Reserve          = false;
            bool includeHeaderType_WorkupTotalBuy   = false;
            bool includeHeaderType_Master           = false;   // TT#1966-MD - JSmith - DC Fulfillment
            
         

            for (int i = 0; i < lstTypes.CheckedItems.Count; i++)
            {
             
                MIDListBoxItem itm = (MIDListBoxItem)lstTypes.CheckedItems[i]; 
                eHeaderType type = (eHeaderType)itm.Tag;

                if (type == eHeaderType.ASN)
                {
                    includeHeaderType_ASN = true;
                }
                else if (type == eHeaderType.Assortment)
                {
                    includeHeaderType_Assortment = true;
                }
                else if (type == eHeaderType.DropShip)
                {
                    includeHeaderType_DropShip = true;
                }
                else if (type == eHeaderType.Dummy)
                {
                    includeHeaderType_Dummy = true;
                }
                else if (type == eHeaderType.IMO)
                {
                    includeHeaderType_IMO = true;
                }
                else if (type == eHeaderType.MultiHeader)
                {
                    includeHeaderType_MultiHeader = true;
                }
                else if (type == eHeaderType.Placeholder)
                {
                    includeHeaderType_Placeholder = true;
                }
                else if (type == eHeaderType.PurchaseOrder)
                {
                    includeHeaderType_PurchaseOrder = true;
                }
                else if (type == eHeaderType.Receipt)
                {
                    includeHeaderType_Receipt = true;
                }
                else if (type == eHeaderType.Reserve)
                {
                    includeHeaderType_Reserve = true;
                }
                else if (type == eHeaderType.WorkupTotalBuy)
                {
                    includeHeaderType_WorkupTotalBuy = true;
                }
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                else if (type == eHeaderType.Master)
                {
                    includeHeaderType_Master = true;
                }
                // End TT#1966-MD - JSmith - DC Fulfillment
            }


            bool includeHeaderStatus_ReceivedOutOfBalance    = false;
            bool includeHeaderStatus_ReceivedInBalance       = false;
            bool includeHeaderStatus_InUseByMultiHeader      = false;
            bool includeHeaderStatus_PartialSizeOutOfBalance = false;
            bool includeHeaderStatus_PartialSizeInBalance    = false;
            bool includeHeaderStatus_AllocatedOutOfBalance   = false;
            bool includeHeaderStatus_AllocatedInBalance      = false;
            bool includeHeaderStatus_SizesOutOfBalance       = false;
            bool includeHeaderStatus_AllInBalance            = false;
            bool includeHeaderStatus_Released                = false;
            bool includeHeaderStatus_ReleaseApproved         = false;
            bool includeHeaderStatus_AllocationStarted       = false;


            for (int i = 0; i < lstStatus.CheckedItems.Count; i++)
            {
                MIDListBoxItem itm = (MIDListBoxItem)lstStatus.CheckedItems[i];
                eHeaderAllocationStatus status = (eHeaderAllocationStatus)itm.Tag;

                if (status == eHeaderAllocationStatus.ReceivedOutOfBalance)
                {
                    includeHeaderStatus_ReceivedOutOfBalance = true;
                }
                else if (status == eHeaderAllocationStatus.ReceivedInBalance)
                {
                    includeHeaderStatus_ReceivedInBalance = true;
                }
                else if (status == eHeaderAllocationStatus.InUseByMultiHeader)
                {
                    includeHeaderStatus_InUseByMultiHeader = true;
                }
                else if (status == eHeaderAllocationStatus.PartialSizeOutOfBalance)
                {
                    includeHeaderStatus_PartialSizeOutOfBalance = true;
                }
                else if (status == eHeaderAllocationStatus.PartialSizeInBalance)
                {
                    includeHeaderStatus_PartialSizeInBalance = true;
                }
                else if (status == eHeaderAllocationStatus.AllocatedOutOfBalance)
                {
                    includeHeaderStatus_AllocatedOutOfBalance = true;
                }
                else if (status == eHeaderAllocationStatus.AllocatedInBalance)
                {
                    includeHeaderStatus_AllocatedInBalance = true;
                }
                else if (status == eHeaderAllocationStatus.SizesOutOfBalance)
                {
                    includeHeaderStatus_SizesOutOfBalance = true;
                }
                else if (status == eHeaderAllocationStatus.AllInBalance)
                {
                    includeHeaderStatus_AllInBalance = true;
                }
                else if (status == eHeaderAllocationStatus.Released)
                {
                    includeHeaderStatus_Released = true;
                }
                else if (status == eHeaderAllocationStatus.ReleaseApproved)
                {
                    includeHeaderStatus_ReleaseApproved = true;
                }
                else if (status == eHeaderAllocationStatus.AllocationStarted)
                {
                    includeHeaderStatus_AllocationStarted = true;
                }
            }



          


            RaiseViewReportEvent(this.cboStore.Text, storeRID,
                                    includeHeaderType_ASN,
                                    includeHeaderType_Assortment,
                                    includeHeaderType_DropShip,
                                    includeHeaderType_Dummy,
                                    includeHeaderType_IMO,
                                    includeHeaderType_MultiHeader,
                                    includeHeaderType_Placeholder,
                                    includeHeaderType_PurchaseOrder,
                                    includeHeaderType_Receipt,
                                    includeHeaderType_Reserve,
                                    includeHeaderType_WorkupTotalBuy,
                                    includeHeaderType_Master,   // TT#1966-MD - JSmith - DC Fulfillment
                                    includeHeaderStatus_ReceivedOutOfBalance,
                                    includeHeaderStatus_ReceivedInBalance,
                                    includeHeaderStatus_InUseByMultiHeader,
                                    includeHeaderStatus_PartialSizeOutOfBalance,
                                    includeHeaderStatus_PartialSizeInBalance,
                                    includeHeaderStatus_AllocatedOutOfBalance,
                                    includeHeaderStatus_AllocatedInBalance,
                                    includeHeaderStatus_SizesOutOfBalance,
                                    includeHeaderStatus_AllInBalance,
                                    includeHeaderStatus_Released,
                                    includeHeaderStatus_ReleaseApproved,
                                    includeHeaderStatus_AllocationStarted      
                                );
        }
        public event ViewReportEventHandler ViewReportEvent;
        public virtual void RaiseViewReportEvent(   string storeIDandName, int storeRID,
                                                    bool includeHeaderType_ASN,            
                                                    bool includeHeaderType_Assortment,
                                                    bool includeHeaderType_DropShip,    
                                                    bool includeHeaderType_Dummy,      
                                                    bool includeHeaderType_IMO,         
                                                    bool includeHeaderType_MultiHeader,
                                                    bool includeHeaderType_Placeholder,   
                                                    bool includeHeaderType_PurchaseOrder,  
                                                    bool includeHeaderType_Receipt, 
                                                    bool includeHeaderType_Reserve,       
                                                    bool includeHeaderType_WorkupTotalBuy,
                                                    bool includeHeaderType_Master,   // TT#1966-MD - JSmith - DC Fulfillment
                                                    bool includeHeaderStatus_ReceivedOutOfBalance,
                                                    bool includeHeaderStatus_ReceivedInBalance,  
                                                    bool includeHeaderStatus_InUseByMultiHeader,     
                                                    bool includeHeaderStatus_PartialSizeOutOfBalance,
                                                    bool includeHeaderStatus_PartialSizeInBalance,
                                                    bool includeHeaderStatus_AllocatedOutOfBalance,  
                                                    bool includeHeaderStatus_AllocatedInBalance, 
                                                    bool includeHeaderStatus_SizesOutOfBalance,    
                                                    bool includeHeaderStatus_AllInBalance,     
                                                    bool includeHeaderStatus_Released,          
                                                    bool includeHeaderStatus_ReleaseApproved,
                                                    bool includeHeaderStatus_AllocationStarted      
                                                )
        {
            if (ViewReportEvent != null)
                ViewReportEvent(this, new ReportData.AllocationByStoreEventArgs(
                   storeIDandName,
                   storeRID,
                   includeHeaderType_ASN,
                   includeHeaderType_Assortment,
                   includeHeaderType_DropShip,
                   includeHeaderType_Dummy,
                   includeHeaderType_IMO,
                   includeHeaderType_MultiHeader,
                   includeHeaderType_Placeholder,
                   includeHeaderType_PurchaseOrder,
                   includeHeaderType_Receipt,
                   includeHeaderType_Reserve,
                   includeHeaderType_WorkupTotalBuy,
                   includeHeaderType_Master,   // TT#1966-MD - JSmith - DC Fulfillment
                   includeHeaderStatus_ReceivedOutOfBalance,
                   includeHeaderStatus_ReceivedInBalance,
                   includeHeaderStatus_InUseByMultiHeader,
                   includeHeaderStatus_PartialSizeOutOfBalance,
                   includeHeaderStatus_PartialSizeInBalance,
                   includeHeaderStatus_AllocatedOutOfBalance,
                   includeHeaderStatus_AllocatedInBalance,
                   includeHeaderStatus_SizesOutOfBalance,
                   includeHeaderStatus_AllInBalance,
                   includeHeaderStatus_Released,
                   includeHeaderStatus_ReleaseApproved,
                   includeHeaderStatus_AllocationStarted      
                                                             ));
        }

        public delegate void ViewReportEventHandler(object sender, ReportData.AllocationByStoreEventArgs e);

        

        private void ReportAllocationByStore_Load(object sender, EventArgs e)
        {
        }
        private Dictionary<int, int> dictionaryStoreRIDs = new Dictionary<int,int>();
        public void LoadData(SessionAddressBlock SAB)
        {
            //load all stores
            cboStore.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboStore.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            int storeIndex = 0;
            foreach (StoreProfile sp in StoreMgmt.StoreProfiles_GetActiveStoresList()) //SAB.StoreServerSession.GetActiveStoresList())
            {
                cboStore.Items.Add(sp.Text);
                dictionaryStoreRIDs.Add(storeIndex, sp.Key);
                storeIndex++;
            }

            // load header type
            eHeaderType type;
            string text = string.Empty;
            bool selected = false;
            bool multiChecked = false; 
            DataTable dtTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));
      
            if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                bool phRemoved = false, asrtRemoved = false;
                for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dRow = dtTypes.Rows[i];
                    if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
                    {
                        dtTypes.Rows.Remove(dRow);
                        asrtRemoved = true;
                    }
                    else if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
                    {
                        dtTypes.Rows.Remove(dRow);
                        phRemoved = true;
                    }
                    if (asrtRemoved && phRemoved)
                    {
                        break;
                    }
                }
            }

            foreach (DataRow dr in dtTypes.Rows)
            {
                type = (eHeaderType)Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
                text = Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);

                // if size, use all statuses
                if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    lstTypes.Items.Add(new MIDListBoxItem(text, type), selected);
                }
                else
                {
                    // remove all size statuses
                    if (Enum.IsDefined(typeof(eNonSizeHeaderType), Convert.ToInt32(type, CultureInfo.CurrentUICulture)))
                    {
                        lstTypes.Items.Add(new MIDListBoxItem(text, type), selected);
                    }
                }

                switch (type)
                {
                    case eHeaderType.MultiHeader:

                        multiChecked = selected;
                        break;

                    case eHeaderType.Assortment:

                        break;

                    case eHeaderType.Placeholder:
                        break;

                    default:
                        break;

                }
            }

            // load header status
            eHeaderAllocationStatus status;
            DataTable dtStatus = MIDText.GetTextType(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
            foreach (DataRow dr in dtStatus.Rows)
            {
                status = (eHeaderAllocationStatus)Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
                text = Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);

                if (status == eHeaderAllocationStatus.InUseByMultiHeader)
                {
                    selected = multiChecked;
                }
               
                // if size, use all statuses
                if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    lstStatus.Items.Add(new MIDListBoxItem(text, status), selected);
                }
                else
                {
                    // remove all size statuses
                    if (Enum.IsDefined(typeof(eNonSizeHeaderAllocationStatus), Convert.ToInt32(status, CultureInfo.CurrentUICulture)))
                    {
                        lstStatus.Items.Add(new MIDListBoxItem(text, status), selected);
                    }
                }
      

            }
        }

        private void lstStatus_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (sender is CheckedListBox)
                {
                    _rightMouseListBox = (CheckedListBox)sender;
                }
            }
        }

        private void lstTypes_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (sender is CheckedListBox)
                {
                    _rightMouseListBox = (CheckedListBox)sender;
                }
            }
        }
        private System.Windows.Forms.CheckedListBox _rightMouseListBox;
        private void mniClearAll_Click(object sender, System.EventArgs e)
        {
            int i;
                if (_rightMouseListBox.Name == "lstTypes")
                {
                    for (i = 0; i < lstTypes.Items.Count; i++)
                    {
                        lstTypes.SetSelected(i, true);      // MID Track #5974 - null reference from context menu
                        lstTypes.SetItemChecked(i, false);
                    }
                }
                else
                    if (_rightMouseListBox.Name == "lstStatus")
                    {
                        for (i = 0; i < lstStatus.Items.Count; i++)
                        {
                            lstStatus.SetSelected(i, true);      // MID Track #5974 - null reference from context menu
                            lstStatus.SetItemChecked(i, false);
                        }
                    }
          
        }

        private void mniSelectAll_Click(object sender, System.EventArgs e)
        {
            int i;

                if (_rightMouseListBox.Name == "lstTypes")
                {
                    for (i = 0; i < lstTypes.Items.Count; i++)
                    {
                        lstTypes.SetSelected(i, true);      // MID Track #5974 - null reference from context menu
                        lstTypes.SetItemChecked(i, true);
                    }
                }
                else
                    if (_rightMouseListBox.Name == "lstStatus")
                    {
                        for (i = 0; i < lstStatus.Items.Count; i++)
                        {
                            lstStatus.SetSelected(i, true);     // MID Track #5974 - null reference from context menu
                            lstStatus.SetItemChecked(i, true);
                        }
                    }
        
        }


    }
}
