using System;
using System.Collections.Generic;
using System.Collections;
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
    public partial class StoreCharacteristics : Form, IFormBase
    {
      

        public StoreCharacteristics()
        {
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
        }

        private SessionAddressBlock SAB;
        private ExplorerAddressBlock EAB;

        public void LoadCharacteristics(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            try
            {
                this.SAB = SAB;
                this.EAB = EAB;

                this.charControl.CharAddEvent += new CharacteristicMaintControl.CharAddEventHandler(Handle_CharAdd);
                this.charControl.CharEditEvent += new CharacteristicMaintControl.CharEditEventHandler(Handle_CharEdit);
                this.charControl.CharDeleteEvent += new CharacteristicMaintControl.CharDeleteEventHandler(Handle_CharDelete);
                this.charControl.CharInUseEvent += new CharacteristicMaintControl.CharInUseEventHandler(Handle_CharInUse);
                this.charControl.ValueAddEvent += new CharacteristicMaintControl.ValueAddEventHandler(Handle_ValueAdd);
                this.charControl.ValueEditEvent += new CharacteristicMaintControl.ValueEditEventHandler(Handle_ValueEdit);
                this.charControl.ValueDeleteEvent += new CharacteristicMaintControl.ValueDeleteEventHandler(Handle_ValueDelete);
                this.charControl.ValueInUseEvent += new CharacteristicMaintControl.ValueInUseEventHandler(Handle_ValueInUse);

                this.charControl.groupRidField = "SCG_RID";
                this.charControl.groupIDField = "Characteristics";
                this.charControl.groupTypeField = "SCG_TYPE";
                this.charControl.groupIsListField = "Is List";
                this.charControl.valueRidField = "FIELD_INDEX";
                this.charControl.valueField = "FIELD_VALUE";
                this.charControl.GetValuesForCharGroup = new CharacteristicMaintControl.GetValuesForCharGroupDelegate(GetValuesForThisCharGroup);

                FunctionSecurityProfile FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresCharacteristics);
                this.charControl.DoInitialize(this.Text, FunctionSecurity.AllowUpdate);
                this.charControl.RefreshCharGroups(GetCharGroups());
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        /// <summary>
        /// Gets charactertics groups
        /// </summary>
        /// <returns></returns>
        private DataTable GetCharGroups()
        {
            DataTable dtCharGroups = new DataTable();
            dtCharGroups.Columns.Add("SCG_RID", typeof(int));
            dtCharGroups.Columns.Add("SCG_TYPE", typeof(int));
            dtCharGroups.Columns.Add("Characteristics");
            dtCharGroups.Columns.Add("Value Type");
            dtCharGroups.Columns.Add("Is List", typeof(bool));

            try
            { 
                StoreData storeCharMaint = new StoreData();
                DataTable dt = storeCharMaint.StoreCharGroup_Read();
                foreach (DataRow drChar in dt.Rows)
                {
                    DataRow dr = dtCharGroups.NewRow();
                    dr["SCG_RID"] = drChar["SCG_RID"];
                    dr["SCG_TYPE"] = drChar["SCG_TYPE"];
                    dr["Characteristics"] = drChar["SCG_ID"];
                    charTypes storeCharType = charTypes.FromIndex(Convert.ToInt32(drChar["SCG_TYPE"]));
                    dr["Value Type"] = storeCharType.Name;
                    dr["Is List"] = Include.ConvertStringToBool((string)drChar["SCG_LIST_IND"]); 
                    dtCharGroups.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return dtCharGroups;
        }
        private DataTable GetValuesForThisCharGroup(int scgRid)
        {
            DataTable dtStoreFieldsAndChars = FieldTypeUtility.GetDataTableForFields();
            try
            {
                DataTable dt = StoreMgmt.StoreCharGroup_ReadValues(scgRid);
  
                foreach (DataRow drChar in dt.Rows)
                {
                    DataRow dr = dtStoreFieldsAndChars.NewRow();
                    dr["OBJECT_TYPE"] = charObjectTypes.Store.Index;
                    dr["FIELD_INDEX"] = Convert.ToInt32(drChar["SC_RID"]); ;
                    fieldDataTypes dataType = fieldDataTypes.FromCharIgnoreLists(Convert.ToInt32(drChar["SCG_TYPE"]));
                    dr["FIELD_TYPE"] = dataType.Index;
                    dr["ALLOW_EDIT"] = true;
                    dr["MAX_LENGTH"] = 50;
                    dr["FIELD_NAME"] = drChar["SCG_ID"];
                    dr["FIELD_VALUE"] = drChar["CHAR_VALUE"];
                    dtStoreFieldsAndChars.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return dtStoreFieldsAndChars;
        }

  

        private void Handle_CharAdd(object sender, CharacteristicMaintControl.CharAddEventArgs e)
        {
            try
            {
                CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "SCG_RID");
                frm.charGroupGetDataForInsert = new CharacteristicsEditorForm.CharGroupGetDataForInsert(this.Store_CharGroupGetDataForInsert);
                frm.isCharGroupValid = new CharacteristicsEditorForm.IsCharGroupValid(this.Store_IsCharGroupValid);
                frm.charGroupInsert = new CharacteristicsEditorForm.CharGroupInsert(this.Store_CharGroupInsert);
                frm.checkForStoreLoadAPI = true;
                frm.PopulateFieldsForCharAdd(this.Text);
                frm.ShowDialog();
                if (frm.hasGroupBeenInserted)
                {
                    this.charControl.RefreshCharGroups(GetCharGroups());
                    this.charControl.ShowCharGroup(frm.charGroupRID);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void Store_CharGroupGetDataForInsert(charObjectTypes charObjectType, DataTable dtCharGroupFields)
        {
            try
            {
                DataRow dr = dtCharGroupFields.NewRow();
                dr["OBJECT_TYPE"] = charObjectType.Index;
                dr["FIELD_INDEX"] = charObjectGroupFields.GroupName.Index;
                dr["FIELD_TYPE"] = fieldDataTypes.Text.Index;
                dr["ALLOW_EDIT"] = true;
                dr["MAX_LENGTH"] = 50;
                dr["FIELD_NAME"] = charObjectGroupFields.GroupName.Name;
                dr["FIELD_VALUE"] = DBNull.Value;
                dtCharGroupFields.Rows.Add(dr);


                DataRow dr2 = dtCharGroupFields.NewRow();
                dr2["OBJECT_TYPE"] = charObjectType.Index;
                dr2["FIELD_INDEX"] = charObjectGroupFields.GroupValueType.Index;
                dr2["FIELD_TYPE"] = fieldDataTypes.List.Index;
                dr2["ALLOW_EDIT"] = true;
                dr2["MAX_LENGTH"] = 50;
                dr2["FIELD_NAME"] = charObjectGroupFields.GroupValueType.Name;
                dr2["FIELD_VALUE"] = 0;
                dtCharGroupFields.Rows.Add(dr2);

                DataRow dr3 = dtCharGroupFields.NewRow();
                dr3["OBJECT_TYPE"] = charObjectType.Index;
                dr3["FIELD_INDEX"] = charObjectGroupFields.IsList.Index;
                dr3["FIELD_TYPE"] = fieldDataTypes.Boolean.Index;
                dr3["ALLOW_EDIT"] = true;
                dr3["MAX_LENGTH"] = 50;
                dr3["FIELD_NAME"] = charObjectGroupFields.IsList.Name;
                dr3["FIELD_VALUE"] = 0;
                dtCharGroupFields.Rows.Add(dr3);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Store_CharGroupGetDataForUpdate(charObjectTypes charObjectType, DataTable dtCharGroupFields, DataRow drCharGroup, bool hasValuesInGroup)
        {
            try
            {
                DataRow dr = dtCharGroupFields.NewRow();
                dr["OBJECT_TYPE"] = charObjectType.Index;
                dr["FIELD_INDEX"] = charObjectGroupFields.GroupName.Index;
                dr["FIELD_TYPE"] = fieldDataTypes.Text.Index;
                dr["ALLOW_EDIT"] = true;
                dr["MAX_LENGTH"] = 50;
                dr["FIELD_NAME"] = charObjectGroupFields.GroupName.Name;
                dr["FIELD_VALUE"] = (string)drCharGroup["Characteristics"];
                dtCharGroupFields.Rows.Add(dr);

                DataRow dr2 = dtCharGroupFields.NewRow();
                dr2["OBJECT_TYPE"] = charObjectType.Index;
                dr2["FIELD_INDEX"] = charObjectGroupFields.GroupValueType.Index;
                dr2["FIELD_TYPE"] = fieldDataTypes.List.Index;
                if (hasValuesInGroup) //do not allow users to change the type of a store characteristic if it has values defined
                {
                    dr2["ALLOW_EDIT"] = false;
                }
                else
                {
                    dr2["ALLOW_EDIT"] = true;
                }
                
                dr2["MAX_LENGTH"] = 50;
                dr2["FIELD_NAME"] = charObjectGroupFields.GroupValueType.Name;
                dr2["FIELD_VALUE"] = charTypes.FromName((string)drCharGroup["Value Type"]).Index;
                dtCharGroupFields.Rows.Add(dr2);

                DataRow dr3 = dtCharGroupFields.NewRow();
                dr3["OBJECT_TYPE"] = charObjectType.Index;
                dr3["FIELD_INDEX"] = charObjectGroupFields.IsList.Index;
                dr3["FIELD_TYPE"] = fieldDataTypes.Boolean.Index;
                dr3["ALLOW_EDIT"] = true;
                dr3["MAX_LENGTH"] = 50;
                dr3["FIELD_NAME"] = charObjectGroupFields.IsList.Name;
                dr3["FIELD_VALUE"] = (bool)drCharGroup["Is List"];
                dtCharGroupFields.Rows.Add(dr3);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private int Store_CharGroupInsert(DataTable dtGroupValues)
        {
            try
            {
                string charGroupName = (string)dtGroupValues.Rows[0]["FIELD_VALUE"];
                int charGroupType = (int)dtGroupValues.Rows[1]["FIELD_VALUE"];
                object isListObj = dtGroupValues.Rows[2]["FIELD_VALUE"];

                bool charGroupIsList = false;
                if (isListObj.GetType() == typeof(bool))
                {
                    charGroupIsList = (bool)isListObj;
                }
                else
                {
                    if ((int)isListObj != 0)
                    {
                        charGroupIsList = true;
                    }
                }
                

                //EditMsgs em = new EditMsgs();
                List<MIDMsg> em = new List<MIDMsg>();
                bool didInsertNewDynamicStoreGroup = false;
                int filterRID = Include.NoRID;
                int scgRID = StoreMgmt.StoreCharGroup_Insert(ref em, ref didInsertNewDynamicStoreGroup, ref filterRID, false, SAB.ClientServerSession.UserRID, charGroupName, charTypes.FromIndex(charGroupType).echarGroupType, charGroupIsList, string.Empty);//, this.ParentForm.Text);

                if (didInsertNewDynamicStoreGroup) //Filter and its conditions have been saved and committed to the db by now.
                {
                    filter f = filterDataHelper.LoadExistingFilter(filterRID);
                    StoreGroupProfile groupProfile = StoreMgmt.StoreGroup_AddOrUpdate(f, isNewGroup: true, loadNewResults: true);  //Adds group and levels and results to the db.  Executes the filter.
                    ((StoreTreeView)(this.EAB.StoreGroupExplorer.TreeView)).AfterSave(f, groupProfile);  //Adds db entry to the FOLDER table
                }
                return scgRID;

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return Include.NoRID;
            }
        }


        private void Handle_CharEdit(object sender, CharacteristicMaintControl.CharEditEventArgs e)
        {
            try
            {
                int charGroupRID = (int)e.drCharGroup["SCG_RID"];
                string charGroupName = (string)e.drCharGroup["Characteristics"];
                if (CharGroupLock(charGroupRID, charGroupName))
                {
                    CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "SCG_RID");
                    frm.charGroupGetDataForUpdate = new CharacteristicsEditorForm.CharGroupGetDataForUpdate(this.Store_CharGroupGetDataForUpdate);
                    frm.isCharGroupValid = new CharacteristicsEditorForm.IsCharGroupValid(this.Store_IsCharGroupValid);
                    frm.charGroupUpdate = new CharacteristicsEditorForm.CharGroupUpdate(this.Store_CharGroupUpdate);
                    frm.PopulateFieldsForCharEdit(this.Text, e.drCharGroup);
                    frm.checkForStoreLoadAPI = true;
                    frm.ShowDialog();
                    CharGroupUnlock();
                    if (frm.hasGroupChanged)
                    {
                        this.charControl.RefreshCharGroups(GetCharGroups());
                        this.charControl.ShowCharGroup(charGroupRID);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void Store_CharGroupUpdate(int scgRID, DataTable dtGroupValues)
        {
            try
            {
                string charGroupName = (string)dtGroupValues.Rows[0]["FIELD_VALUE"];
                int charGroupType = (int)dtGroupValues.Rows[1]["FIELD_VALUE"];
                bool charGroupIsList = (bool)dtGroupValues.Rows[2]["FIELD_VALUE"];

                StoreMgmt.StoreCharGroup_Update(scgRID, charGroupName, charTypes.FromIndex(charGroupType).echarGroupType, charGroupIsList);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreCharacteristic, "Updated: " + charGroupName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

        }

        private void Handle_CharDelete(object sender, CharacteristicMaintControl.CharDeleteEventArgs e)
        {
            try
            {

                //Do not allow deleting if Store Load API is running
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                    return;
                }



                if (IsInUse(eProfileType.StoreCharacteristics, e.charGroupRid) == false)   //This will also check store groups
                {
                  
                    //DataTable dtStoreGroupsInUseByChar = StoreMgmt.StoreGroup_GetFiltersForStoreCharGroup(e.charGroupRid);
                    //ArrayList ridList = new ArrayList();
                    //foreach (DataRow drStoreGroup in dtStoreGroupsInUseByChar.Rows)
                    //{
                    //    int sgRID = (int)drStoreGroup["SG_RID"];
                    //    ridList.Add(sgRID);
                    //}

                    //if (IsInUse(eProfileType.StoreGroup, ridList) == false) 
                    //{
                        //delete char group
                        StoreMgmt.StoreCharGroup_Delete(e.charGroupRid); //Also sets dynamic groups inactive
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreCharacteristic, "Removed: " + e.charGroupID);
                        EAB.StoreGroupExplorer.ReloadNodes(); //dynamic store groups will have been set inactive now, so reload nodes

                        this.charControl.RefreshCharGroups(GetCharGroups());
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Handle_CharInUse(object sender, CharacteristicMaintControl.CharInUseEventArgs e)
        {
            try
            {
                eProfileType etype = eProfileType.StoreCharacteristics;
                ArrayList _ridList = new ArrayList();

                foreach (DataRow drCharGroup in e.charGroupRows)
                {
                    _ridList.Add((int)drCharGroup["SCG_RID"]);
                }

                if (_ridList.Count > 0)
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    bool display = false;
                    MIDFormBase fb = new MIDFormBase();
                    fb.DisplayInUseForm(_ridList, etype, inUseTitle, ref display, true);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private bool IsInUse(eProfileType etype, int aRID)
        {
            bool isInUse = false;
            try
            {
                ArrayList ridList = new ArrayList();
                ridList.Add(aRID);
                if (ridList.Count > 0) //If no RID is selected do nothing.
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    MIDFormBase fb = new MIDFormBase();
                    fb.DisplayInUseFormAndShowProgress(ridList, etype, inUseTitle, false, out isInUse);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return isInUse;
        }
        private bool IsInUse(eProfileType etype, ArrayList ridList)
        {
            bool isInUse = false;
            try
            {
              
                if (ridList.Count > 0) //If no RID is selected do nothing.
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    MIDFormBase fb = new MIDFormBase();
                    fb.DisplayInUseFormAndShowProgress(ridList, etype, inUseTitle, false, out isInUse);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return isInUse;
        }

        private void Handle_ValueAdd(object sender, CharacteristicMaintControl.ValueAddEventArgs e)
        {
            try
            {
                CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "SCG_RID");
                frm.isCharValueValid = new CharacteristicsEditorForm.IsCharValueValid(this.Store_IsCharValueValid);
                frm.valueInsert = new CharacteristicsEditorForm.ValueInsert(this.StoreChar_ValueInsert);
                frm.PopulateFieldsForValueAdd(this.Text, e.charGroupRid, e.charGroupID, e.fieldDataType);
                frm.checkForStoreLoadAPI = true;
                frm.ShowDialog();
                charControl.RefreshValues();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private int StoreChar_ValueInsert(int scgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue, string charGroupID, string valueAsString)
        {
            int scRID = Include.NoRID;
            try
            {
                scRID = StoreMgmt.StoreCharValue_Insert(scgRID, textValue, dateValue, numberValue, dollarValue);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreCharacteristic, "Value Added: " + charGroupID + ": " + valueAsString);
                //Refresh results for dynamic groups
                StoreMgmt.StoreGroups_RefreshFiltersForStoreCharGroup(scgRID, SharedRoutines.ProgressBarOptions_Get());
                EAB.StoreGroupExplorer.ReloadNodes();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return scRID;
        }


        private void Handle_ValueEdit(object sender, CharacteristicMaintControl.ValueEditEventArgs e)
        {
            try
            {
                if (ValueLock(e.valueRid))
                {
                    CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "SCG_RID");
                    frm.isCharValueValid = new CharacteristicsEditorForm.IsCharValueValid(this.Store_IsCharValueValid);
                    frm.valueUpdate = new CharacteristicsEditorForm.ValueUpdate(this.StoreCharValue_Update);
                    frm.PopulateFieldsForValueEdit(this.Text, e.charGroupRid, e.charGroupID, e.fieldDataType, e.valueRid, e.val);
                    frm.checkForStoreLoadAPI = true;
                    frm.ShowDialog();
                    ValueUnlock();

               

                    if (frm.hasValueChanged)
                    {
                        charControl.RefreshValues();
                        // Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                        RenameGroupLevelsForCharacteristicValue(e.charGroupRid, frm.originalValueAsString, frm.currentValueAsString);
                        // End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                        //Refresh results for dynamic groups
                        StoreMgmt.StoreGroups_RefreshFiltersForStoreCharGroup(e.charGroupRid, SharedRoutines.ProgressBarOptions_Get());
                        EAB.StoreGroupExplorer.ReloadNodes();
                    }
      
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        // Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
        private void RenameGroupLevelsForCharacteristicValue(int charGroupRid, string originalValue, string currentValue)
        {
            try
            {
                StoreGroupProfile sgp;
                string newName;
                DataTable dt = StoreMgmt.StoreGroup_GetFiltersForStoreCharGroup(charGroupRid);
                // Begin TT#1912-MD - JSmith - Str Versioning - Change Value name as user arobinson, go to user pam open the allcocation override method - the set/value name that is change in the method the grid goes blank for that set.  Other sets are fine.
                // Add brackets to name to match attribute set names.
                //originalValue = originalValue.Replace("_", "[_]");
                //currentValue = currentValue.Replace("_", "[_]");
                // End TT#1912-MD - JSmith - Str Versioning - Change Value name as user arobinson, go to user pam open the allcocation override method - the set/value name that is change in the method the grid goes blank for that set.  Other sets are fine.
                foreach (DataRow dr in dt.Rows)
                {
                     sgp = StoreMgmt.StoreGroup_Get(Convert.ToInt32(dr["SG_RID"]));
                    foreach(StoreGroupLevelProfile sglp in sgp.GetGroupLevelList(false))
                    {
                        bool needToRename = false;
                        string[] levelNameSections = sglp.Name.Split(':');
                        for (int i = 0; i < levelNameSections.Length; i++)
                        {
                            if (levelNameSections[i].Trim() == originalValue.Trim())
                            {
                                levelNameSections[i] = currentValue;
                                needToRename = true;
                            }
                        }
                        if (needToRename)
                        {
                            newName = string.Empty;
                            for (int i = 0; i < levelNameSections.Length; i++)
                            {
                                if (i > 0)
                                {
                                    newName = newName + " : ";
                                    needToRename = true;
                                }
                                newName = newName + levelNameSections[i];
                            }
                            StoreMgmt.StoreGroupLevel_Rename(sglp.Key, sglp.LevelVersion, newName, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        // End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.

        private void StoreCharValue_Update(int scRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue, string charGroupID, string valueAsString)
        {
            try
            {
                StoreMgmt.StoreCharValue_Update(scRID, textValue, dateValue, numberValue, dollarValue);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreCharacteristic, "Value Updated: " + charGroupID + ": " + valueAsString);
            }
            catch(Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }


        private GenericEnqueue genericEnqueueCharGroup;
        /// <summary>
        /// Returns true if able to lock the group.
        /// Otherwise it will display a message and return false.
        /// </summary>
        /// <param name="scgRID"></param>
        /// <returns></returns>
        private bool CharGroupLock(int scgRID, string scgName)
        {

            genericEnqueueCharGroup = new GenericEnqueue(eLockType.StoreCharacteristicGroup, scgRID, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
            try
            {
                genericEnqueueCharGroup.EnqueueGeneric();
                return true;
            }
            catch (GenericConflictException)
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueCharGroup, "Store Characteristics", scgName);
                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return false;
            }

        }
        private void CharGroupUnlock()
        {
            try
            {
                genericEnqueueCharGroup.DequeueGeneric();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private GenericEnqueue genericEnqueueValue;
        /// <summary>
        /// Returns true if able to lock the value.
        /// Other it will display a message and return false.
        /// </summary>
        /// <param name="scRID"></param>
        /// <returns></returns>
        private bool ValueLock(int scRID)
        {
            genericEnqueueValue = new GenericEnqueue(eLockType.StoreCharacteristicValue, scRID, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
            try
            {
                genericEnqueueValue.EnqueueGeneric();
                return true;
            }
            catch (GenericConflictException)
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueValue, "Characteristic Value");
                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return false;
            }
        }
        private void ValueUnlock()
        {
            try
            {
                genericEnqueueValue.DequeueGeneric();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void Handle_ValueDelete(object sender, CharacteristicMaintControl.ValueDeleteEventArgs e)
        {
            try
            {
                //Do not allow deleting if Store Load API is running
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                    return;
                }

                if (IsInUse(eProfileType.StoreCharacteristicValues, e.valueRid) == false)
                {
                    // Begin TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed
                    ArrayList ridList = GetAttributeSetsForCharacteristicValue(e.charGroupRid, e.valueAsString);
                    if (IsInUse(eProfileType.StoreGroupLevel, ridList) == false)
                    {
                    // End TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed
                        //delete char value
                        StoreMgmt.StoreCharValue_Delete(e.valueRid);
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreCharacteristic, "Value Removed: " + e.charGroupID + ": " + e.valueAsString);
                        charControl.RefreshValues();

                        //Refresh results for dynamic groups
                        StoreMgmt.StoreGroups_RefreshFiltersForStoreCharGroup(e.charGroupRid, SharedRoutines.ProgressBarOptions_Get());
                        EAB.StoreGroupExplorer.ReloadNodes();
                    // Begin TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed
                    }
                    // End TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        // Begin TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed
        private ArrayList GetAttributeSetsForCharacteristicValue(int charGroupRid, string charID)
        {
            DataTable dtStoreGroupsInUseByChar = StoreMgmt.StoreGroup_GetFiltersForStoreCharGroup(charGroupRid);
            ArrayList ridList = new ArrayList();
            StoreGroupProfile storeGroupProf;
            foreach (DataRow drStoreGroup in dtStoreGroupsInUseByChar.Rows)
            {
                int sgRID = (int)drStoreGroup["SG_RID"];
                storeGroupProf = StoreMgmt.StoreGroup_Get(sgRID);
                foreach (StoreGroupLevelProfile sglp in storeGroupProf.GroupLevels)
                {
                    if (sglp.Name == charID)
                    {
                        ridList.Add(sglp.Key);
                        break;
                    }
                }
            }
            return ridList;
        }
        // End TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed

        private void Handle_ValueInUse(object sender, CharacteristicMaintControl.ValueInUseEventArgs e)
        {
            try
            {
                eProfileType etype = eProfileType.StoreCharacteristicValues;
                ArrayList _ridList = new ArrayList();

                foreach (DataRow drValue in e.valueRows)
                {
                    _ridList.Add((int)drValue["FIELD_INDEX"]);
                }

                if (_ridList.Count > 0)
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    bool display = false;
                    MIDFormBase fb = new MIDFormBase();
                    fb.DisplayInUseForm(_ridList, etype, inUseTitle, ref display, true);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        public bool Store_IsCharGroupValid(validationKinds validationKind, int objectType, int fieldIndex, int charGroupRID, object originalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;
            try
            {
                if (objectType == charObjectTypes.Store)
                {
                    if (fieldIndex == charObjectGroupFields.GroupName)
                    {

                        if (proposedValue == DBNull.Value || (string)proposedValue == string.Empty)
                        {
                            isValid = false;
                            //em.ErrorFound = true;
                            //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ValueRequired, String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), charObjectGroupFields.GroupName.Name), this.ParentForm.Text);
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_ValueRequired, msg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), charObjectGroupFields.GroupName.Name) });
                            return false;  // TT#1852-MD - JSmith - Store Characteristic- Select New-Select OK(did not make any changes) - receive Error
                        }

                        if (validationKind == validationKinds.UponSaving) //Need to ensure Group Name is unique - delayed until actual saving of all changes
                        {
                            if (StoreMgmt.DoesStoreCharGroupNameAlreadyExist((string)proposedValue, charGroupRID)) 
                            {
                                isValid = false;
                                //em.ErrorFound = true;
                                //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateCharGroupName, MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharGroupName), this.ParentForm.Text);
                                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateCharGroupName, msg = MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharGroupName) });
                            }
                            else if (charGroupRID != Include.NoRID) //editing characteristic
                            {
                                StoreGroupMaint groupMaint = new StoreGroupMaint();
                                if (groupMaint.DoesStoreGroupNameAlreadyExistForStoreCharGroup((string)proposedValue, Include.GlobalUserRID, charGroupRID) == true)
                                {
                                    isValid = false;
                                    //em.ErrorFound = true;
                                    string msgDetails = string.Empty;   
                                    msgDetails += " Duplicate Attribute Exists: " + (string)proposedValue;
                                    //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msgDetails, this.ParentForm.Text);
                                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msg = msgDetails });
                                }
                            }
                            else if (charGroupRID == Include.NoRID) //new characteristic
                            {
                                StoreGroupMaint groupMaint = new StoreGroupMaint();
                                if (groupMaint.DoesStoreGroupNameAlreadyExist((string)proposedValue, Include.GlobalUserRID) == true)  
                                {
                                    isValid = false;
                                    //em.ErrorFound = true;
                                    string msgDetails = string.Empty;   
                                    msgDetails += " Duplicate Attribute Exists: " + (string)proposedValue;
                                    //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msgDetails, this.ParentForm.Text);
                                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msg = msgDetails });
                                }
                            }
                        }


                        //update the store group name if the char group name changed and is valid
                        if (validationKind == validationKinds.UponSaving && isValid && (originalValue.ToString() != proposedValue.ToString()) && (charGroupRID != Include.NoRID))
                        {
                            StoreGroupMaint groupMaint = new StoreGroupMaint();
                            int sgRID = groupMaint.StoreGroup_ReadForStoreCharGroupRename((string)proposedValue, Include.GlobalUserRID, charGroupRID);
                            if (sgRID != Include.NoRID)
                            {
                                StoreMgmt.StoreGroup_Rename(sgRID, (string)proposedValue);

                                //Update the node on the store group explorer with the new name
                                EAB.StoreGroupExplorer.RenameAndSelectStoreGroupNode(sgRID, (string)proposedValue);
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return isValid;
        }
        public bool Store_IsCharValueValid(int charGroupRid, fieldDataTypes charGroupFieldType, validationKinds validationKind, int objectType, int fieldIndex, int valueRID, object originalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;

            try
            {
                if (proposedValue == DBNull.Value)
                {
                    isValid = false;
                    //em.ErrorFound = true;
                    //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ValueRequired, String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), "Value"), this.ParentForm.Text);
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_ValueRequired, msg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), "Value") });
                }
                else if (charGroupFieldType == fieldDataTypes.Text && (string)proposedValue == string.Empty)
                {
                    isValid = false;
                    //em.ErrorFound = true;
                    //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ValueRequired, String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), "Value"), this.ParentForm.Text);
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_ValueRequired, msg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), "Value") });
                }
                else
                {
                    if (validationKind == validationKinds.UponSaving)
                    {
                        int scRidDuplicate = Include.NoRID;
                        if (StoreMgmt.DoesStoreCharValueAlreadyExist(proposedValue, charGroupFieldType, charGroupRid, valueRID, ref scRidDuplicate))
                        {
                            isValid = false;
                            //em.ErrorFound = true;
                            //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateCharValue, MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharValue), this.ParentForm.Text);
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateCharValue, msg = MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharValue) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return isValid;
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
