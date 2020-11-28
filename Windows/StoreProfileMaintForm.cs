using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class StoreProfileMaintForm : Form, IFormBase
    {

        private SessionAddressBlock SAB;
        private ExplorerAddressBlock EAB;


        public StoreProfileMaintForm(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            this.SAB = SAB;
            this.EAB = EAB;
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
            this.storeProfileMaintControl1.AddStoreEvent += new StoreProfileMaintControl.AddStoreEventHandler(Handle_AddStore);
            this.storeProfileMaintControl1.EditStoreEvent += new StoreProfileMaintControl.EditStoreEventHandler(Handle_EditStore);
            this.storeProfileMaintControl1.EditFieldsEvent += new StoreProfileMaintControl.EditFieldsEventHandler(Handle_EditFields);
        }

        public void LoadStores()
        {
            this.storeProfileMaintControl1.LoadStores(this.SAB, this.Text);
        }
        public void ShowStore(int storeRID)
        {
            this.storeProfileMaintControl1.ShowStore(storeRID);
        }
        private void Handle_AddStore(object sender, StoreProfileMaintControl.AddStoreEventArgs e)
        {
            try
            {
                StoreProfileMaintSingleStore frmAddStore = new StoreProfileMaintSingleStore(this.SAB, this.EAB);
                frmAddStore.PopulateFieldsForAdd(this.Text);
                frmAddStore.ShowDialog();
                if (frmAddStore.storeAdded)
                {
                    //this.storeProfileMaintControl1.StoreAddNewUnlock();
                    EAB.StoreGroupExplorer.ReloadNodes();
                    this.storeProfileMaintControl1.BindStoreList();
                    this.storeProfileMaintControl1.ShowStore(frmAddStore.storeAddedRID);
             
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            finally
            {
                this.storeProfileMaintControl1.StoreAddNewUnlock();
            }
        }
        private void Handle_EditStore(object sender, StoreProfileMaintControl.EditStoreEventArgs e)
        {
            try
            {
                StoreProfileMaintSingleStore frmAddStore = new StoreProfileMaintSingleStore(this.SAB, this.EAB);
                //frmAddStore.storeUnlockDelegate = new StoreProfileMaintControl.StoreUnlockDelegate(this.storeProfileMaintControl1.StoreUnlock);
                frmAddStore.PopulateFieldsForEdit(this.Text, e.storeRID);
                frmAddStore.ShowDialog();
                this.storeProfileMaintControl1.StoreUnlock();
                this.storeProfileMaintControl1.BindStoreList(); //refresh Active Ind and Marked for delete in the main store list
                this.storeProfileMaintControl1.ShowStore(e.storeRID);
               // this.storeProfileMaintControl1.RefreshFieldsForStore();

                if (frmAddStore.storeUpdated)
                {
                    // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                    //EAB.StoreGroupExplorer.ReloadNodes();
                    EAB.StoreGroupExplorer.AddStoreExplorerPendingRefresh();
                    // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

      

        private void Handle_EditFields(object sender, StoreProfileMaintControl.EditFieldsEventArgs e)
        {
            try
            {
                if (e.selectedFields.Count > 0)
                {
                    StoreProfileMaintFieldsByCol frmFieldsByCol = new StoreProfileMaintFieldsByCol(this.SAB, this.EAB);
                    //frmFieldsByCol.fieldsUnlockDelegate = new StoreProfileMaintControl.StoreFieldsUnlockDelegate(this.storeProfileMaintControl1.FieldsUnlock);
                    frmFieldsByCol.PopulateFieldsForEdit(this.SAB, this.Text, e.selectedFields);
                    frmFieldsByCol.ShowDialog();
                    this.storeProfileMaintControl1.FieldsUnlock();
                    this.storeProfileMaintControl1.BindStoreList(); //refresh Active Ind and Marked for delete in the main store list
                    //this.storeProfileMaintControl1.RefreshFieldsForStore();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }



        #region IFormBase Members
        public void ICut()
        {

        }

        public void ICopy()
        {
        }

        public void IPaste()
        {
        }


        public void ISave()
        {
        }

        public void ISaveAs()
        {
        }

        public void IDelete()
        {
        }

        public void IRefresh()
        {
        }

        public void IFind()
        {
        }

        public void IClose()
        {
            this.Close();
        }

        #endregion
    }
}
