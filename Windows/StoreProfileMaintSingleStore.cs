using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class StoreProfileMaintSingleStore : Form
    {

        private SessionAddressBlock SAB;
        private ExplorerAddressBlock EAB;
        private int storeRID = -1;

        public bool storeAdded = false;
        public int storeAddedRID = -1;

        public bool storeUpdated = false;

        public StoreProfileMaintSingleStore(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            this.SAB = SAB;
            this.EAB = EAB;
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
        }

       

        private void StoreProfileMaintSingleStore_Load(object sender, EventArgs e)
        {
            this.gridFields.CloseFormEvent += new MIDGridFieldEditor.CloseFormEventHandler(Handle_CloseForm);
            this.gridFields.SaveEvent += new MIDGridFieldEditor.SaveEventHandler(Handle_Save);
        }

        public void PopulateFieldsForEdit(string formText, int storeRID)
        {
            try
            {
                this.Text = "Store Profiles [Edit]";
                this.storeRID = storeRID;
                StoreValidation.SetCharListValuesLists(); //reads data for charactersitics drop downs where type=list  
                this.gridFields.Initialize(true, formText);
                this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
                this.gridFields.isFieldValueValid = new IsFieldValueValid(StoreValidation.IsStoreFieldValid);
                this.gridFields.isObjectValid = new IsObjectValid(StoreValidation.IsStoreObjectValid);

                StoreValidation.hasActiveFieldChanged = false;
                StoreValidation.SetSAB(SAB);
                this.gridFields.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);
                DataTable dt = StoreValidation.ReadFieldAndCharacteristicValuesForStore(SAB, this.storeRID);
                dt.AcceptChanges();
                this.gridFields.BindGrid(this.storeRID, dt, new GetListValuesForField(StoreValidation.GetListValuesForStoreField));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
 
        public void PopulateFieldsForAdd(string formText)
        {
            try
            {
                this.Text = "Store Profiles [New]";
                this.storeRID = -1;
                StoreValidation.SetCharListValuesLists(); //reads data for charactersitics drop downs where type=list  
                this.gridFields.Initialize(true, formText);
                this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
                this.gridFields.isFieldValueValid = new IsFieldValueValid(StoreValidation.IsStoreFieldValid);
                this.gridFields.isObjectValid = new IsObjectValid(StoreValidation.IsStoreObjectValid);
                //StoreValidation.ReserveStoreRID = SAB.StoreServerSession.GlobalOptions.ReserveStoreRID;
                StoreValidation.hasActiveFieldChanged = false;
                StoreValidation.SetSAB(SAB);
                this.gridFields.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);
                DataTable dt = StoreValidation.ReadFieldAndCharacteristicValuesForNewStore(SAB);
                dt.AcceptChanges();
                this.gridFields.BindGrid(Include.NoRID, dt, new GetListValuesForField(StoreValidation.GetListValuesForStoreField));
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

        private void Handle_CloseForm(object sender, MIDGridFieldEditor.CloseFormEventArgs e)
        {
            this.Close();
        }

        private void Handle_Save(object sender, MIDGridFieldEditor.SaveEventArgs e)
        {
            //At this point all fields and the store object have been validated


            //Do not allow saving if Store Load API is running
            GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            if (genericEnqueueStoreLoad.DoesHaveConflicts())
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                return;
            }

            try
            {
                StoreProfile storeProfile = new StoreProfile(this.storeRID);
                bool profileActiveInd = false;
                bool? profileSimilarStoreModelInd = null;  // TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
                string profileCity = null;
                string profileState = null;
                int? profileLeadTime = null;
                int? profileSellingSqFt = null;
                DateTime? profileSellingCloseDt = null;
                DateTime? profileSellingOpenDt = null;
                DateTime? profileStockCloseDt = null;
                DateTime? profileStockOpenDt = null;
                bool? profileShipOnFriday = null;
                bool? profileShipOnMonday = null;
                bool? profileShipOnSaturday = null;
                bool? profileShipOnSunday = null;
                bool? profileShipOnThursday = null;
                bool? profileShipOnTuesday = null;
                bool? profileShipOnWednesday = null;
                string profileStoreName = null;
                string profileStoreId = null;
                string profileStoreDescription = null;
                string profileIMO_ID = null;


                //also fill char structure while we are looping through values
                List<storeCharInfo> charList = new List<storeCharInfo>();
                List<int> fieldChangedList = new List<int>();
                List<string> fieldNameList = new List<string>();  // TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields

                foreach (DataRow dr in e.dsValuesToSave.Tables[0].Rows)
                {
                    int objectType = (int)dr["OBJECT_TYPE"];
                    if (objectType == storeObjectTypes.StoreFields)
                    {
                        int fieldIndex = (int)dr["FIELD_INDEX"];
                        storeFieldTypes fieldType = storeFieldTypes.FromIndex(fieldIndex);
                        if (dr["IS_DIRTY"] != DBNull.Value)
                        {
                            if ( (bool)dr["IS_DIRTY"] == true)
                            {
                                filterStoreFieldTypes filterFieldType = filterStoreFieldTypes.FromField(fieldType);
                                if (filterFieldType != null)
                                {
                                    fieldChangedList.Add(filterFieldType.dbIndex);
                                    // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                                    if (!fieldNameList.Contains(filterFieldType.Name))
                                    {
                                        fieldNameList.Add(filterFieldType.Name);
                                    }
                                    // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                                }
                            }
                        }
                        



                        string fieldValue = dr["FIELD_VALUE"].ToString();
                        if (fieldType == storeFieldTypes.ActiveInd)
                        {
                            profileActiveInd = Convert.ToBoolean(fieldValue);
                        }
                        // Begin TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
                        if (fieldType == storeFieldTypes.SimilarStoreModel)
                        {
                            profileSimilarStoreModelInd = Convert.ToBoolean(fieldValue);
                        }
                        // End TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
                        if (fieldType == storeFieldTypes.City)
                        {
                            profileCity = fieldValue;
                        }
                        if (fieldType == storeFieldTypes.State && fieldValue != String.Empty)
                        {
                            //we have the state index (dataField), but on the database states are stored by display, so we need to get the display value
                            int stateIndex;
                            if (int.TryParse(fieldValue, out stateIndex) == false)
                            {
                                profileState = fieldValue;
                            }
                            else
                            {
                                if (stateIndex != -1)
                                {
                                    string dataField = string.Empty;
                                    string displayField = string.Empty;
                                    DataTable dtStates = StoreValidation.GetListValuesForStoreField(storeObjectTypes.StoreFields.Index, storeFieldTypes.State.fieldIndex, ref dataField, ref displayField);

                                    DataRow[] drState = dtStates.Select(dataField + "=" + stateIndex.ToString());
                                    if (drState.Length > 0)
                                    {
                                        string stateAbbrev = (string)drState[0][displayField];
                                        profileState = stateAbbrev;
                                    }
                                }
                            }
                        }
                        if (fieldType == storeFieldTypes.LeadTime)
                        {
                            if (fieldValue != string.Empty)
                            {
                                profileLeadTime = Convert.ToInt32(fieldValue);
                            }
                        }
                        if (fieldType == storeFieldTypes.SellingSqFt)
                        {
                            if (fieldValue != string.Empty)
                            {
                                profileSellingSqFt = Convert.ToInt32(fieldValue);
                            }
                        }
                        if (fieldType == storeFieldTypes.SellingOpenDate)
                        {
                            if (fieldValue != string.Empty)
                            {
                                profileSellingOpenDt = Convert.ToDateTime(fieldValue);
                            }
                        }
                        if (fieldType == storeFieldTypes.SellingCloseDate)
                        {
                            if (fieldValue != string.Empty)
                            {
                                profileSellingCloseDt = Convert.ToDateTime(fieldValue);
                            }
                        }
                        if (fieldType == storeFieldTypes.StockOpenDate)
                        {
                            if (fieldValue != string.Empty)
                            {
                                profileStockOpenDt = Convert.ToDateTime(fieldValue);
                            }
                        }
                        if (fieldType == storeFieldTypes.StockCloseDate)
                        {
                            if (fieldValue != string.Empty)
                            {
                                profileStockCloseDt = Convert.ToDateTime(fieldValue);
                            }
                        }
                        if (fieldType == storeFieldTypes.ShipOnMonday)
                        {
                            profileShipOnMonday = Convert.ToBoolean(fieldValue);      
                        }
                        if (fieldType == storeFieldTypes.ShipOnTuesday)
                        {
                            profileShipOnTuesday = Convert.ToBoolean(fieldValue);
                        }
                        if (fieldType == storeFieldTypes.ShipOnWednesday)
                        {
                            profileShipOnWednesday = Convert.ToBoolean(fieldValue);
                        }
                        if (fieldType == storeFieldTypes.ShipOnThursday)
                        {
                            profileShipOnThursday = Convert.ToBoolean(fieldValue);
                        }
                        if (fieldType == storeFieldTypes.ShipOnFriday)
                        {
                            profileShipOnFriday = Convert.ToBoolean(fieldValue);
                        }
                        if (fieldType == storeFieldTypes.ShipOnSaturday)
                        {
                            profileShipOnSaturday = Convert.ToBoolean(fieldValue);
                        }
                        if (fieldType == storeFieldTypes.ShipOnSunday)
                        {
                            profileShipOnSunday = Convert.ToBoolean(fieldValue);
                        }
                        if (fieldType == storeFieldTypes.StoreName)
                        {
                            profileStoreName = fieldValue;
                        }
                        if (fieldType == storeFieldTypes.StoreID)
                        {
                            profileStoreId = fieldValue;
                        }
                        if (fieldType == storeFieldTypes.StoreDescription)
                        {
                            profileStoreDescription = fieldValue;
                        }
                        if (fieldType == storeFieldTypes.ImoID)
                        {
                            profileIMO_ID = fieldValue;
                        }
                    }
                    else if (objectType == storeObjectTypes.StoreCharacteristics)
                    {
                        storeCharInfo charInfo = new storeCharInfo();
                        charInfo.stRID = this.storeRID;
                        charInfo.scgRID =  (int)dr["FIELD_INDEX"];
                        if (dr["IS_DIRTY"] != DBNull.Value)
                        {
                            charInfo.isDirty = (bool)dr["IS_DIRTY"];
                        }
                        else
                        {
                            charInfo.isDirty = false;
                        }
                        if (charInfo.isDirty)
                        {
                            int fieldType = (int)dr["FIELD_TYPE"];
                            if (fieldType == 1)  //list
                            {
                                if (dr["FIELD_VALUE"] == DBNull.Value || dr["FIELD_VALUE"].ToString() == string.Empty)
                                {
                                    charInfo.action = storeCharInfoAction.Skip;
                                }
                                else
                                {
                                    int scRID;
                                    string val = dr["FIELD_VALUE"].ToString();
                                    if (int.TryParse(val, out scRID) == true)
                                    {
                                        charInfo.scRID = scRID;
                                        // Begin TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
                                        if (StoreValidation.ClearStoreCharValue(charInfo.scgRID, scRID))
                                        {
                                            charInfo.scRID = 0;
                                            charInfo.action = storeCharInfoAction.Skip;
                                        }
                                        // End TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
                                    }
                                    else
                                    {
                                        //get the scrid from the list for this char
                                        string dataField = string.Empty;
                                        string displayField = string.Empty;
                                        DataTable dtCharList = StoreValidation.GetListValuesForStoreField(storeObjectTypes.StoreCharacteristics.Index, charInfo.scgRID, ref dataField, ref displayField);
                                        DataRow[] drCharTemp = dtCharList.Select(displayField + "='" + val + "'");
                                        if (drCharTemp.Length > 0)
                                        {
                                            scRID = (int)drCharTemp[0][dataField];
                                            charInfo.scRID = scRID;
                                        }
                                    }
                                    // Begin TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
                                    //charInfo.dataType = fieldDataTypes.List;
                                    //charInfo.action = storeCharInfoAction.UseValue;
                                    if (charInfo.action != storeCharInfoAction.Skip)
                                    {
                                        charInfo.dataType = fieldDataTypes.List;
                                        charInfo.action = storeCharInfoAction.UseValue;
                                    }
                                    // End TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
                                }
                            }
                            else
                            {
                                if (dr["FIELD_VALUE"].ToString() == string.Empty)
                                {
                                    charInfo.action = storeCharInfoAction.Skip;
                                }
                                else
                                {
                                    charInfo.action = storeCharInfoAction.InsertValue;
                                    charInfo.dataType = fieldDataTypes.FromIndex(fieldType); //see fieldDataTypes.FromChar
                                    charInfo.anyValue = dr["FIELD_VALUE"].ToString();
                                }
                            }
                            charList.Add(charInfo);
                        }
                    }
                }

                storeProfile.SetFieldsOnProfile(
                profileActiveInd: profileActiveInd,
                profileSimilarStoreModelInd: profileSimilarStoreModelInd,  // TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
                profileCity: profileCity,
                profileState: profileState,
                profileLeadTime: profileLeadTime,
                profileSellingCloseDt: profileSellingCloseDt,
                profileSellingOpenDt: profileSellingOpenDt,
                profileSellingSqFt: profileSellingSqFt,
                profileShipOnFriday: profileShipOnFriday,
                profileShipOnMonday: profileShipOnMonday,
                profileShipOnSaturday: profileShipOnSaturday,
                profileShipOnSunday: profileShipOnSunday,
                profileShipOnThursday: profileShipOnThursday,
                profileShipOnTuesday: profileShipOnTuesday,
                profileShipOnWednesday: profileShipOnWednesday,
                profileStockCloseDt: profileStockCloseDt,
                profileStockOpenDt: profileStockOpenDt,
                profileStoreName: profileStoreName,
                profileStoreId: profileStoreId,
                profileStoreDescription: profileStoreDescription,
                profileIMO_ID: profileIMO_ID
                );

           

                if (this.storeRID == Include.NoRID)
                {
                    //Insert new store
                    DataSet dsValues = e.dsValuesToSave;

                    //for new stores - only refresh groups if the store active indicator is set to true
                    bool refreshStoreGroups;
                    if (storeProfile.ActiveInd)
                    {
                        refreshStoreGroups = true;
                    }
                    else
                    {
                        refreshStoreGroups = false;
                    }

                    this.storeAddedRID = StoreMgmt.StoreProfile_Add(storeProfile, charList, refreshStoreGroups: refreshStoreGroups, progressbarOptions: SharedRoutines.ProgressBarOptions_Get());
                    StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreProfile, "Added: " + storeProfile.Text);
                    this.storeAdded = true;
                }
                else
                {
                    //Update store (and refresh groups)
                    bool doRefreshNodes = false;

                    if (StoreValidation.hasActiveFieldChanged)
                    {
                        StoreMgmt.StoreProfile_Update(storeProfile, false, ref doRefreshNodes, SharedRoutines.ProgressBarOptions_Get(), charList, fieldChangedList);
                        //Since the store results will now change for EVERY store group, refresh ALL store groups
                        StoreMgmt.StoreGroups_Refresh(SharedRoutines.ProgressBarOptions_Get());
                        doRefreshNodes = true;  //this will cause a reload of the nodes in the store group explorer
                    }
                    else
                    {
                        StoreMgmt.StoreProfile_Update(storeProfile, true, ref doRefreshNodes, SharedRoutines.ProgressBarOptions_Get(), charList, fieldChangedList);
                        // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                        StoreMgmt.StoreGroups_Refresh(SharedRoutines.ProgressBarOptions_Get(), fieldNameList);
                        // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                    }
                    StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreProfile, "Updated: " + storeProfile.Text);

                    if (doRefreshNodes)
                    {
                        this.storeUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }           
        }



        private void StoreProfileMaintSingleStore_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.gridFields.GridExitEditMode();  // TT#1831-MD - JSmith - Edit Store or Fields, make change select black X to Close window and do not receive mssg on pending changes.
            if (this.gridFields.CanClose() == false)
            {
                e.Cancel = true;
            }
        }

 

    }
}
