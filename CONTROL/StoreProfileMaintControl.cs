using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class StoreProfileMaintControl : UserControl
    {
        public StoreProfileMaintControl()
        {
            InitializeComponent();
        }

        private SessionAddressBlock SAB;

        public void LoadStores(SessionAddressBlock SAB, string formText)
        {
            try
            {
                StoreValidation.SetCharListValuesLists(); //reads data for charactersitics drop downs where type=list  (Would be nice not to have to read this everytime they change stores.)


                this.SAB = SAB;
                this.gridStores.SelectedRowChangedEvent += new MIDGridControl.SelectedRowChangedEventHandler(Handle_StoreRowChanged);
                this.gridStores.AfterRowFilterChangedEvent += new MIDGridControl.AfterRowFilterChangedEventHandler(Handle_RowFilterChanged);  // TT#1807-MD - JSmith - Store Profiles - Filtering - Field does not change to selected field when filtering. Custom drop down has a selection for ((DB Null))
                this.gridStores.exportObjectName = "Stores";
                this.gridStores.HideLayoutOnMenu();
                this.gridStores.HideColumn("ST_RID");
                this.gridStores.doIncludeHeaderWhenResizing = true;
                this.gridStores.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;



                this.gridFields.HideButtons();
                this.gridFields.Initialize(false, formText);
                this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
                //this.gridFields.HideLayoutOnMenu();
                //this.gridFields.SkipAutoSelect();
                //this.gridFields.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
                //this.gridFields.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;

                //StoreMaint storeMaint = new StoreMaint();
                //DataTable dt = storeMaint.ReadStoresForMaint();
                //DataSet ds = new DataSet();
                //ds.Tables.Add(dt);
                //this.gridStores.BindGrid(ds);

                BindStoreList();


                //Read Security
                FunctionSecurityProfile functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresProfiles);
                FunctionSecurityProfile storeAttributeSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesGlobal);
                FunctionSecurityProfile userStoreAttributeSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);

                if (functionSecurity.AllowUpdate)
                {
                    this.ultraToolbarsManager1.Tools["btnStoreEdit"].SharedProps.Enabled = true;
                    this.ultraToolbarsManager1.Tools["btnEditFields"].SharedProps.Enabled = true;
                    this.ultraToolbarsManager1.Tools["btnStoreAdd"].SharedProps.Enabled = true;                    
                }
                else
                {
                    this.ParentForm.Text += " [Read Only]";
                }

                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public void ShowStore(int storeRID)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in gridStores.ultraGrid1.Rows)
            {
                LocateStartingStore(row, storeRID);
            }
        }
        private void LocateStartingStore(Infragistics.Win.UltraWinGrid.UltraGridRow row, int storeRID)
        {
            if (row.HasChild(null))
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand cb in row.ChildBands)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow childRow in cb.Rows)
                    {
                        LocateStartingStore(childRow, storeRID);
                    }
                }
            }
            else
            {
                if (Convert.ToInt32(row.Cells["ST_RID"].Value) == storeRID)
                {
         
                    gridStores.ultraGrid1.Selected.Rows.Clear();
                    gridStores.ultraGrid1.ActiveRow = row;
                    row.Selected = true;
                    row.Expanded = true;
                }
            }
        }

        private int currentStoreRID = -1;
        private string currentStoreName = string.Empty;
        private void Handle_StoreRowChanged(object sender, MIDGridControl.SelectedRowChangedEventArgs e)
        {
            try
            {
                currentStoreRID = Convert.ToInt32(e.drSelected["ST_RID"]);
                currentStoreName = Convert.ToString(e.drSelected["Id"]) + ": " + Convert.ToString(e.drSelected["Name"]);
                RefreshFieldsForStore();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        // Begin TT#1807-MD - JSmith - Store Profiles - Filtering - Field does not change to selected field when filtering. Custom drop down has a selection for ((DB Null))
        private void Handle_RowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in gridStores.ultraGrid1.Rows)
            {
                if (!row.IsFilteredOut)
                {
                    row.Selected = true;
                    currentStoreRID = Convert.ToInt32(row.Cells["ST_RID"].Value);
                    currentStoreName = Convert.ToString(row.Cells["Id"].Value) + ": " + Convert.ToString(row.Cells["Name"].Value);
                    RefreshFieldsForStore();
                    break;
                }
            }
        }
        // End TT#1807-MD - JSmith - Store Profiles - Filtering - Field does not change to selected field when filtering. Custom drop down has a selection for ((DB Null))
		
        public void RefreshFieldsForStore()
        {
            try
            {

                this.gridFields.BindGrid(currentStoreRID, StoreValidation.ReadFieldAndCharacteristicValuesForStore(SAB, currentStoreRID), new GetListValuesForField(StoreValidation.GetListValuesForStoreField));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public void FormatCellValueForField(int objectType, int fieldIndex, fieldDataTypes dataType, ref Infragistics.Win.UltraWinGrid.UltraGridCell cell)
        {
            try
            {
                if (dataType == fieldDataTypes.DateNoTime) //Display min dates as null
                {
                    DateTime dateValue;
                    if (DateTime.TryParse(cell.Value.ToString(), out dateValue))
                    {
                        if (dateValue == DateTime.MinValue)
                        {
                            cell.Value = DBNull.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (e.Tool.Key == "btnStoreAdd")
            {
                AddStore();
            }
            else if (e.Tool.Key == "btnStoreEdit")
            {
                EditStore();
            }
            else if (e.Tool.Key == "btnEditFields")
            {
                EditFields();
            }
        }

        public void BindStoreList()
        {
            StoreMaint storeMaint = new StoreMaint();
            DataTable dt = storeMaint.ReadStoresForMaint();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            this.gridStores.BindGrid(ds);
        }
    

        private void AddStore()
        {
            if (CanAddNewStore())
            {
                RaiseAddStoreEvent();
            }     
        }
        private void EditStore()
        {
            if (currentStoreRID == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a store to edit.");
            }
            else
            {
                if (StoreLock(this.currentStoreRID, this.currentStoreName))
                {
                    RaiseEditStoreEvent(this.currentStoreRID);
                }
            }
        }
        private void EditFields()
        {
            if (FieldsLock())
            {
                RaiseEditFieldsEvent(this.gridFields.GetSelectedRows());
            }
        }

        private GenericEnqueue genericEnqueueStore;
        private GenericEnqueue genericEnqueueStoreFields;
        private GenericEnqueue genericEnqueueStoreAddNew;
        /// <summary>
        /// Returns true if able to lock the group.
        /// Otherwise it will display a message and return false.
        /// </summary>
        /// <param name="storeRID"></param>
        /// <returns></returns>
        private bool StoreLock(int storeRID, string storeName)
        {
            //First check fields
            if (genericEnqueueStoreFields == null)
            {
                genericEnqueueStoreFields = new GenericEnqueue(eLockType.StoreFields, -1, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
            }

            if (genericEnqueueStoreFields.DoesHaveConflicts() == false)
            {
                //Now check individual store
                genericEnqueueStore = new GenericEnqueue(eLockType.Store, storeRID, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
                try
                {
                    genericEnqueueStore.EnqueueGeneric();
                    return true;
                }
                catch (GenericConflictException)
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStore, "Store", storeName);
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                    return false;
                }
            }
            else
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreFields, "Store Fields");
                return false;
            }

        }

        private bool CanAddNewStore()
        {
    
            if (genericEnqueueStoreAddNew == null)
            {
                genericEnqueueStoreAddNew = new GenericEnqueue(eLockType.StoreAddNew, -1, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
            }

            if (genericEnqueueStoreAddNew.DoesHaveConflicts() == false)
            {
                try
                {
                    genericEnqueueStoreAddNew.EnqueueGeneric();
                    return true;
                }
                catch (GenericConflictException)
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreAddNew, "Add New Store");
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                    return false;
                }
            }
            else
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreAddNew, "Add New Store");
                return false;
            }

        }


        private bool FieldsLock()
        {
            //First check fields
            genericEnqueueStoreFields = new GenericEnqueue(eLockType.StoreFields, -1, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
            try
            {
                genericEnqueueStoreFields.EnqueueGeneric();
                
                //Now check for ANY individual store that might be locked
                MIDEnqueue MIDEnqueueData = new MIDEnqueue();
                DataTable lockTable = MIDEnqueueData.Enqueue_Read(eLockType.Store);

                if (lockTable.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    //Display the list of store conflicts
                    string errMsg = MIDText.GetTextOnly(eMIDTextCode.msg_StandardInUseMsg);

                    string msg = ((int)eMIDTextCode.msg_StandardInUseMsg).ToString() + ": "; 
                    foreach (DataRow dataRow in lockTable.Rows)
                    {
                        int storeRID = Convert.ToInt32(dataRow["RID"]);
                        string storeName = StoreMgmt.GetStoreDisplayText(storeRID); //this.SAB.StoreServerSession.GetStoreDisplayText(storeRID);
                        string userName = Convert.ToString(dataRow["USER_FULLNAME"]);

                        msg += String.Format(errMsg, "Store", storeName, userName) + System.Environment.NewLine;
                    }
                    System.Windows.Forms.MessageBox.Show(msg, "Store Conflict", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    return false;
                }
            }
            catch (GenericConflictException)
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreFields, "Store Fields");
                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return false;
            }

        }

        public void StoreUnlock()
        {
            try
            {
                genericEnqueueStore.DequeueGeneric();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        public void StoreAddNewUnlock()
        {
            try
            {
                genericEnqueueStoreAddNew.DequeueGeneric();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        public void FieldsUnlock()
        {
            try
            {
                genericEnqueueStoreFields.DequeueGeneric();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }



        public delegate void AddStoreEventHandler(object sender, AddStoreEventArgs e);
        public event AddStoreEventHandler AddStoreEvent;
        public void RaiseAddStoreEvent()
        {
            if (AddStoreEvent != null)
                AddStoreEvent(new object(), new AddStoreEventArgs());
        }
        public class AddStoreEventArgs
        {
            public AddStoreEventArgs() { }
        }

        public delegate void EditStoreEventHandler(object sender, EditStoreEventArgs e);
        public event EditStoreEventHandler EditStoreEvent;
        public void RaiseEditStoreEvent(int storeRID)
        {
            if (EditStoreEvent != null)
                EditStoreEvent(new object(), new EditStoreEventArgs(storeRID));
        }
        public class EditStoreEventArgs
        {
            public EditStoreEventArgs(int storeRID) { this.storeRID = storeRID; }
            public int storeRID { get; private set; }
        }

        public delegate void EditFieldsEventHandler(object sender, EditFieldsEventArgs e);
        public event EditFieldsEventHandler EditFieldsEvent;
        public void RaiseEditFieldsEvent(List<DataRow> selectedFields)
        {
            if (EditFieldsEvent != null)
                EditFieldsEvent(new object(), new EditFieldsEventArgs(selectedFields));
        }
        public class EditFieldsEventArgs
        {
            public EditFieldsEventArgs(List<DataRow> selectedFields) { this.selectedFields = selectedFields; }

            public List<DataRow> selectedFields { get; private set; }
        }
    }
}
