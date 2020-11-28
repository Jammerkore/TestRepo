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
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class StoreProfileMaintFieldsByCol : Form
    {

        private SessionAddressBlock SAB;
        private ExplorerAddressBlock EAB;
        public bool storeFieldsOrCharsUpdated = false;

        public StoreProfileMaintFieldsByCol(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            this.SAB = SAB;
            this.EAB = EAB;
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
        }

        private void StoreProfileMaintSingleStore_Load(object sender, EventArgs e)
        {
            this.gridFieldsByCol.CloseFormEvent += new MIDGridFieldEditorColumns.CloseFormEventHandler(Handle_CloseForm);
            this.gridFieldsByCol.SaveEvent += new MIDGridFieldEditorColumns.SaveEventHandler(Handle_Save);
        }

        public void PopulateFieldsForEdit(SessionAddressBlock SAB, string formText, List<DataRow> selectedFieldsAndChars)
        {
            try
            {
                this.Text = "Store Profile Fields [Edit]";
                StoreValidation.SetCharListValuesLists(); //reads data for charactersitics drop downs where type=list  
                this.gridFieldsByCol.Initialize(true, formText);
                this.gridFieldsByCol.formatCellValueForField = new MIDGridFieldEditorColumns.FormatCellValueForField(this.FormatCellValueForField);
                this.gridFieldsByCol.isFieldValueValid = new IsFieldValueValid(StoreValidation.IsStoreFieldValid);
                this.gridFieldsByCol.isObjectValid = new IsObjectValidForFields(StoreValidation.IsStoreObjectValidForFieldEditing);
                //StoreValidation.ReserveStoreRID = SAB.StoreServerSession.GlobalOptions.ReserveStoreRID;
                StoreValidation.hasActiveFieldChanged = false;
                StoreValidation.SetSAB(SAB);
                this.gridFieldsByCol.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);
                List<fieldColumnMap> columnMapList = null;
                DataTable dtAllStoreFields = null;
                DataTable dt = StoreValidation.ReadFieldAndCharacteristicValuesForSelectedFields(SAB, selectedFieldsAndChars, ref columnMapList, ref dtAllStoreFields);
                dt.AcceptChanges();
                this.gridFieldsByCol.BindGrid(dt, new GetListValuesForField(StoreValidation.GetListValuesForStoreField), columnMapList);
                this.gridFieldsByCol.dtAllValues = dtAllStoreFields;
                this.gridFieldsByCol.objectRidColumnName = "ST_RID";

                //this.gridFieldsByCol.BindList(StoreMaintenance.MakeStoreObjectsForSelectedFields(SAB, selectedFieldsAndChars));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

      

        public void FormatCellValueForField(int objectType, int fieldIndex, fieldDataTypes dataType, ref Infragistics.Win.UltraWinGrid.UltraGridCell cell)
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

        private void Handle_CloseForm(object sender, MIDGridFieldEditorColumns.CloseFormEventArgs e)
        {
            this.Close();
        }

        private void Handle_Save(object sender, MIDGridFieldEditorColumns.SaveEventArgs e)
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
                ////also fill char structure while we are looping through values
                List<storeCharInfo> charList = new List<storeCharInfo>();
                List<storeCharInfo> charListForAllStores = new List<storeCharInfo>();
                List<int> fieldChangedList = new List<int>();
                List<string> fieldNameList = new List<string>();  // TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                foreach (DataRow dr in e.dsValuesToSave.Tables[0].Rows)
                {
                    int storeRID = (int)dr["OBJECT_RID"];
                    charList.Clear();
                    StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeRID);
                    bool profileActiveInd = storeProfile.ActiveInd;
                    bool? profileSimilarStoreModelInd = null;  // TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
                    string profileCity = storeProfile.City;
                    string profileState = storeProfile.State;
                    int? profileLeadTime = storeProfile.LeadTime;
                    int? profileSellingSqFt = storeProfile.SellingSqFt;
                    DateTime? profileSellingCloseDt = storeProfile.SellingCloseDt;
                    DateTime? profileSellingOpenDt = storeProfile.SellingOpenDt;
                    DateTime? profileStockCloseDt = storeProfile.StockCloseDt;
                    DateTime? profileStockOpenDt = storeProfile.StockOpenDt;
                    bool? profileShipOnFriday = storeProfile.ShipOnFriday;
                    bool? profileShipOnMonday = storeProfile.ShipOnMonday;
                    bool? profileShipOnSaturday = storeProfile.ShipOnSaturday;
                    bool? profileShipOnSunday = storeProfile.ShipOnSunday;
                    bool? profileShipOnThursday = storeProfile.ShipOnThursday;
                    bool? profileShipOnTuesday = storeProfile.ShipOnTuesday;
                    bool? profileShipOnWednesday = storeProfile.ShipOnWednesday;
                    string profileStoreName = storeProfile.StoreName;
                    string profileStoreId = storeProfile.StoreId;
                    string profileStoreDescription = storeProfile.StoreDescription;
                    string profileIMO_ID = storeProfile.IMO_ID;

                    foreach (fieldColumnMap map in e.columnMapList)
                    {
                        if (map.isDirty)
                        {
                            string fieldValue = dr[map.columnIndex].ToString();
                            if (map.objectType == storeObjectTypes.StoreFields)
                            {
                                storeFieldTypes fieldType = storeFieldTypes.FromIndex(map.fieldIndex);
                                filterStoreFieldTypes filterFieldType = filterStoreFieldTypes.FromField(fieldType);
                                if (filterFieldType != null)
                                {
                                    if (fieldChangedList.Exists(x => x == filterFieldType.dbIndex) == false)
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




                            } //end of store fields
                            else if (map.objectType == storeObjectTypes.StoreCharacteristics)
                            {

                                object curVal = dr[map.columnIndex, System.Data.DataRowVersion.Current];
                                object origVal = dr[map.columnIndex, System.Data.DataRowVersion.Original];


                                if (curVal.ToString() != origVal.ToString())
                                {
                                    int scgRID = map.fieldIndex;
                                    storeCharInfo charInfo = new storeCharInfo();
                                    charInfo.stRID = storeRID;
                                    charInfo.scgRID = scgRID;
                                    charInfo.isDirty = true;
                                    charInfo.anyValue = fieldValue;
                                    //int fieldType = map.fieldDataType   (int)dr["FIELD_TYPE"];
                                    if (map.fieldDataType == fieldDataTypes.List)  //list
                                    {
                                        if (dr[map.columnIndex] == DBNull.Value || dr[map.columnIndex].ToString() == string.Empty)
                                        {
                                            charInfo.action = storeCharInfoAction.Skip;
                                        }
                                        else
                                        {
                                            int scRID;
                                            string val = dr[map.columnIndex].ToString();
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
                                        if (dr[map.columnIndex].ToString() == string.Empty)
                                        {
                                            charInfo.action = storeCharInfoAction.Skip;
                                        }
                                        else
                                        {
                                            charInfo.action = storeCharInfoAction.InsertValue;
                                            charInfo.dataType = map.fieldDataType;//fieldDataTypes.FromIndex(fieldType); //see fieldDataTypes.FromChar
                                            charInfo.anyValue = dr[map.columnIndex].ToString();
                                        }
                                    }
                                    charList.Add(charInfo);
                                    charListForAllStores.Add(charInfo);
                                }
                            } //end of store char
                        } //end of map columns

                    
                    } //end of isDirty
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
                                                profileIMO_ID: profileIMO_ID);

                    if (fieldChangedList.Count > 0 || charList.Count > 0)
                    {
                        //Update store
                        bool doRefreshNodes = false;
                        StoreMgmt.StoreProfile_Update(storeProfile, false, ref doRefreshNodes, SharedRoutines.ProgressBarOptions_Get(), charList, fieldChangedList);
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreProfile, "Updated: " + storeProfile.Text);
                        if (doRefreshNodes)
                        {
                            //delay the refresh of store groups until we have finished updating all the store rows
                        }
                    }


                
                } //end of store rows loop

                //Refresh all the store groups
                if ((charListForAllStores != null && charListForAllStores.Count > 0) || fieldChangedList.Count > 0)
                {
                    this.storeFieldsOrCharsUpdated = true;
                    StoreMgmt.StoreGroups_RefreshForChangedFieldsAndChars(charListForAllStores, fieldChangedList, SharedRoutines.ProgressBarOptions_Get());
                    // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                    //this.EAB.StoreGroupExplorer.ReloadNodes();
                    this.EAB.StoreGroupExplorer.AddStoreExplorerPendingRefresh();
                    // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                }






   



          
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }           
        }
      
        private void StoreProfileMaintSingleStore_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.gridFieldsByCol.GridExitEditMode();  // TT#1831-MD - JSmith - Edit Store or Fields, make change select black X to Close window and do not receive mssg on pending changes.
            if (this.gridFieldsByCol.CanClose() == false)
            {
                e.Cancel = true;
            }
        }

 

    }
}
