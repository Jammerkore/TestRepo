using System;
using System.Collections;
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
    public partial class HeaderCharacteristics : Form, IFormBase
    {
        public HeaderCharacteristics()
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

                this.charControl.groupRidField = "HCG_RID";
                this.charControl.groupIDField = "Characteristics";
                this.charControl.groupTypeField = "HCG_TYPE";
                this.charControl.groupIsListField = "Is List";
                this.charControl.valueRidField = "FIELD_INDEX";
                this.charControl.valueField = "FIELD_VALUE";
                this.charControl.GetValuesForCharGroup = new CharacteristicMaintControl.GetValuesForCharGroupDelegate(GetValuesForThisCharGroup);

                FunctionSecurityProfile FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHeadersCharacteristics);
                this.charControl.DoInitialize(this.Text, FunctionSecurity.AllowUpdate);
                this.charControl.RefreshCharGroups(GetCharGroups());
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private DataTable GetCharGroups()
        {
            DataTable dtCharGroups = new DataTable();
            dtCharGroups.Columns.Add("HCG_RID", typeof(int));
            dtCharGroups.Columns.Add("HCG_TYPE", typeof(int));
            dtCharGroups.Columns.Add("Characteristics");
            dtCharGroups.Columns.Add("Value Type");
            dtCharGroups.Columns.Add("Is List", typeof(bool));
            dtCharGroups.Columns.Add("Protect", typeof(bool));

            try
            {
                HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
                DataTable dt = charData.HeaderCharGroup_Read();
                foreach (DataRow drChar in dt.Rows)
                {
                    DataRow dr = dtCharGroups.NewRow();
                    dr["HCG_RID"] = drChar["HCG_RID"];
                    dr["HCG_TYPE"] = drChar["HCG_TYPE"];
                    dr["Characteristics"] = drChar["HCG_ID"];
                    charTypes storeCharType = charTypes.FromIndex(Convert.ToInt32(drChar["HCG_TYPE"]));
                    dr["Value Type"] = storeCharType.Name;
                    dr["Is List"] = Include.ConvertStringToBool((string)drChar["HCG_LIST_IND"]); 
                    dr["Protect"] = Include.ConvertStringToBool((string)drChar["HCG_PROTECT_IND"]);
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
            DataTable dtGroups = FieldTypeUtility.GetDataTableForFields();
            try
            {
                HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
                DataTable dt = charData.HeaderCharGroup_ReadValues(scgRid);

                foreach (DataRow drChar in dt.Rows)
                {
                    DataRow dr = dtGroups.NewRow();
                    dr["OBJECT_TYPE"] = charObjectTypes.Header.Index;
                    dr["FIELD_INDEX"] = Convert.ToInt32(drChar["HC_RID"]);
                    fieldDataTypes dataType = fieldDataTypes.FromCharIgnoreLists(Convert.ToInt32(drChar["HCG_TYPE"]));
                    dr["FIELD_TYPE"] = dataType.Index;
                    dr["ALLOW_EDIT"] = true;
                    dr["MAX_LENGTH"] = 50;
                    dr["FIELD_NAME"] = drChar["HCG_ID"];
                    dr["FIELD_VALUE"] = drChar["CHAR_VALUE"];
                    dtGroups.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return dtGroups;
        }



        private void Handle_CharAdd(object sender, CharacteristicMaintControl.CharAddEventArgs e)
        {
            try
            {
                CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Header, "HCG_RID");
                frm.charGroupGetDataForInsert = new CharacteristicsEditorForm.CharGroupGetDataForInsert(this.Header_CharGroupGetDataForInsert);
                frm.isCharGroupValid = new CharacteristicsEditorForm.IsCharGroupValid(this.Header_IsCharGroupValid);
                frm.charGroupInsert = new CharacteristicsEditorForm.CharGroupInsert(this.Header_CharGroupInsert);
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
        private void Header_CharGroupGetDataForInsert(charObjectTypes charObjectType, DataTable dtCharGroupFields)
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

                DataRow dr4 = dtCharGroupFields.NewRow();
                dr4["OBJECT_TYPE"] = charObjectType.Index;
                dr4["FIELD_INDEX"] = charObjectGroupFields.GroupProtectInd.Index;
                dr4["FIELD_TYPE"] = fieldDataTypes.Boolean.Index;
                dr4["ALLOW_EDIT"] = true;
                dr4["MAX_LENGTH"] = 50;
                dr4["FIELD_NAME"] = charObjectGroupFields.GroupProtectInd.Name;
                dr4["FIELD_VALUE"] = false;
                dtCharGroupFields.Rows.Add(dr4);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Header_CharGroupGetDataForUpdate(charObjectTypes charObjectType, DataTable dtCharGroupFields, DataRow drCharGroup, bool doesGroupHaveValues)
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
                if (doesGroupHaveValues) //do not allow users to change the header group type if the group already has values defined
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

                DataRow dr4 = dtCharGroupFields.NewRow();
                dr4["OBJECT_TYPE"] = charObjectType.Index;
                dr4["FIELD_INDEX"] = charObjectGroupFields.GroupProtectInd.Index;
                dr4["FIELD_TYPE"] = fieldDataTypes.Boolean.Index;
                dr4["ALLOW_EDIT"] = true;
                dr4["MAX_LENGTH"] = 50;
                dr4["FIELD_NAME"] = charObjectGroupFields.GroupProtectInd.Name;
                dr4["FIELD_VALUE"] = (bool)drCharGroup["Protect"];
                dtCharGroupFields.Rows.Add(dr4);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        // Begin TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
        public delegate void HeaderMaintSaved();
        public event HeaderMaintSaved OnSavedHandler;
        // End TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.

        private int Header_CharGroupInsert(DataTable dtGroupValues)
        {
            try
            {
                string charGroupName = (string)dtGroupValues.Rows[0]["FIELD_VALUE"];
                int charGroupType = (int)dtGroupValues.Rows[1]["FIELD_VALUE"];
                //bool charGroupIsList = (bool)dtGroupValues.Rows[2]["FIELD_VALUE"];
                //bool charGroupProtect = (bool)dtGroupValues.Rows[3]["FIELD_VALUE"];



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

                object protectObj = dtGroupValues.Rows[3]["FIELD_VALUE"];
                bool charGroupProtect = false;
                if (protectObj.GetType() == typeof(bool))
                {
                    charGroupProtect = (bool)protectObj;
                }
                else
                {
                    if ((int)protectObj != 0)
                    {
                        charGroupProtect = true;
                    }
                }



                HeaderCharacteristicsData charMaint = new HeaderCharacteristicsData();
                int hcgRID = charMaint.HeaderCharGroup_Insert(charGroupName, charTypes.FromIndex(charGroupType).eHeaderCharGroupType, charGroupIsList, charGroupProtect);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.HeaderCharacteristic, "Added: " + charGroupName);
                // Begin TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
                // RAISE EVENT FOR ALLOCATION EXPLORER SO HE CAN HANDLE THE NEW COLUMNS.
                if (OnSavedHandler != null)
                {
                    OnSavedHandler();
                }
                // End TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
                return hcgRID;
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
                int charGroupRID = (int)e.drCharGroup["HCG_RID"];
                string charGroupName = (string)e.drCharGroup["Characteristics"];
                if (CharGroupLock(charGroupRID, charGroupName))
                {
                    // Begin TT#1937-MD - JSmith - Removing an Existing Header Characteristic Name Displays and Error
                    //CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "HCG_RID");
                    CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Header, "HCG_RID");
                    // End TT#1937-MD - JSmith - Removing an Existing Header Characteristic Name Displays and Error
                    frm.charGroupGetDataForUpdate = new CharacteristicsEditorForm.CharGroupGetDataForUpdate(this.Header_CharGroupGetDataForUpdate);
                    frm.isCharGroupValid = new CharacteristicsEditorForm.IsCharGroupValid(this.Header_IsCharGroupValid);
                    frm.charGroupUpdate = new CharacteristicsEditorForm.CharGroupUpdate(this.Header_CharGroupUpdate);
                    frm.PopulateFieldsForCharEdit(this.Text, e.drCharGroup);
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

        private void Header_CharGroupUpdate(int hcgRID, DataTable dtGroupValues)
        {
            try
            {
                string charGroupName = (string)dtGroupValues.Rows[0]["FIELD_VALUE"];
                int charGroupType = (int)dtGroupValues.Rows[1]["FIELD_VALUE"];
                bool charGroupIsList = (bool)dtGroupValues.Rows[2]["FIELD_VALUE"];
                bool charGroupProtect = (bool)dtGroupValues.Rows[3]["FIELD_VALUE"];

                HeaderCharacteristicsData charMaint = new HeaderCharacteristicsData();
                charMaint.HeaderCharGroup_Update(hcgRID, charGroupName, charTypes.FromIndex(charGroupType).eHeaderCharGroupType, charGroupIsList, charGroupProtect);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.HeaderCharacteristic, "Updated: " + charGroupName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            //finally
            //{
            //    CharGroupUnlock();
            //}
        }

        private void Handle_CharDelete(object sender, CharacteristicMaintControl.CharDeleteEventArgs e)
        {
            try
            {
                if (IsInUse(eProfileType.HeaderCharGroup, e.charGroupRid) == false)
                {
                    //delete char group
                    HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
                    charData.HeaderCharGroup_Delete(e.charGroupRid);
                    StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.HeaderCharacteristic, "Removed: " + e.charGroupID);
                    this.charControl.RefreshCharGroups(GetCharGroups());
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
                eProfileType etype = eProfileType.HeaderCharGroup;
                ArrayList _ridList = new ArrayList();

                foreach (DataRow drCharGroup in e.charGroupRows)
                {
                    _ridList.Add((int)drCharGroup["HCG_RID"]);
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

        private void Handle_ValueAdd(object sender, CharacteristicMaintControl.ValueAddEventArgs e)
        {
            try
            {
                CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "HCG_RID");
                frm.isCharValueValid = new CharacteristicsEditorForm.IsCharValueValid(this.Header_IsCharValueValid);
                frm.valueInsert = new CharacteristicsEditorForm.ValueInsert(this.Header_ValueInsert);
                frm.PopulateFieldsForValueAdd(this.Text, e.charGroupRid, e.charGroupID, e.fieldDataType);
                frm.ShowDialog();

                charControl.RefreshValues();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private int Header_ValueInsert(int hcgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue, string charGroupID, string valueAsString)
        {
            int hcRID = Include.NoRID;
            try
            {
                HeaderCharacteristicsData charMaint = new HeaderCharacteristicsData();
                hcRID = charMaint.HeaderCharValue_Insert(hcgRID, textValue, dateValue, numberValue, dollarValue);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.HeaderCharacteristic, "Value Added: " + charGroupID + ": " + valueAsString);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            // Begin TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
            // RAISE EVENT FOR ALLOCATION EXPLORER SO HE CAN HANDLE THE NEW COLUMNS.
            if (OnSavedHandler != null)
            {
                OnSavedHandler();
            }
            // End TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
            return hcRID;
        }

        private void Handle_ValueEdit(object sender, CharacteristicMaintControl.ValueEditEventArgs e)
        {
            try
            {
                if (ValueLock(e.valueRid))
                {
                    CharacteristicsEditorForm frm = new CharacteristicsEditorForm(SAB, EAB, charObjectTypes.Store, "HCG_RID");
                    frm.isCharValueValid = new CharacteristicsEditorForm.IsCharValueValid(this.Header_IsCharValueValid);
                    frm.valueUpdate = new CharacteristicsEditorForm.ValueUpdate(this.Header_ValueUpdate);
                    frm.PopulateFieldsForValueEdit(this.Text, e.charGroupRid, e.charGroupID, e.fieldDataType, e.valueRid, e.val);
                    frm.ShowDialog();
                    ValueUnlock();
                    if (frm.hasValueChanged)
                    {
                        charControl.RefreshValues();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Header_ValueUpdate(int hcRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue, string charGroupID, string valueAsString)
        {
            try
            {
                HeaderCharacteristicsData charMaint = new HeaderCharacteristicsData();
                charMaint.HeaderCharValue_Update(hcRID, textValue, dateValue, numberValue, dollarValue);
                StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.HeaderCharacteristic, "Value Updated: " + charGroupID + ": " + valueAsString);
            }
            catch(Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            //finally
            //{
            //    ValueUnlock();
            //}
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

            genericEnqueueCharGroup = new GenericEnqueue(eLockType.HeaderCharacteristicGroup, scgRID, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
            try
            {
                genericEnqueueCharGroup.EnqueueGeneric();
                return true;
            }
            catch (GenericConflictException)
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueCharGroup, "Header Characteristics", scgName);
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
            catch(Exception ex)
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
            genericEnqueueValue = new GenericEnqueue(eLockType.HeaderCharacteristicValue, scRID, this.SAB.ClientServerSession.UserRID, this.SAB.ClientServerSession.ThreadID);
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
            catch(Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void Handle_ValueDelete(object sender, CharacteristicMaintControl.ValueDeleteEventArgs e)
        {
            try
            {
                if (IsInUse(eProfileType.HeaderChar, e.valueRid) == false)
                {
                    //delete char value
                    HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
                    charData.HeaderCharValue_Delete(e.valueRid);
                    StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.HeaderCharacteristic, "Value Removed: " + e.charGroupID + ": " + e.valueAsString);
                    charControl.RefreshValues();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void Handle_ValueInUse(object sender, CharacteristicMaintControl.ValueInUseEventArgs e)
        {
            try
            {
                eProfileType etype = eProfileType.HeaderChar;
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

        public bool Header_IsCharGroupValid(validationKinds validationKind, int objectType, int fieldIndex, int charGroupRID, object orignalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;
            try
            {
                if (objectType == charObjectTypes.Header)
                {
                    if (fieldIndex == charObjectGroupFields.GroupName)
                    {

                        if (proposedValue == DBNull.Value || (string)proposedValue == string.Empty)
                        {
                            isValid = false;
                            //em.ErrorFound = true;
                            //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ValueRequired, String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), charObjectGroupFields.GroupName.Name), this.ParentForm.Text);
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_ValueRequired, msg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired), charObjectGroupFields.GroupName.Name) });
                        }

                        else if (validationKind == validationKinds.UponSaving) //Need to ensure Group Name is unique - delayed until actual saving of all changes
                        {
                            HeaderCharacteristicsData charMaintData = new HeaderCharacteristicsData();
                            if (charMaintData.DoesHeaderCharGroupNameAlreadyExist((string)proposedValue, charGroupRID))
                            {
                                isValid = false;
                                //em.ErrorFound = true;
                                //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateCharGroupName, MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharGroupName), this.ParentForm.Text);
                                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateCharGroupName, msg = MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharGroupName) });
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
        public bool Header_IsCharValueValid(int charGroupRid, fieldDataTypes charGroupFieldType, validationKinds validationKind, int objectType, int fieldIndex, int valueRID, object orignalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
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
                        HeaderCharacteristicsData charMaintData = new HeaderCharacteristicsData();
                        bool doesValueAlreadyExist = false;
                        int hcRidDuplicate = Include.NoRID;
                        if (charGroupFieldType == fieldDataTypes.Text)
                        {
                            doesValueAlreadyExist = charMaintData.DoesHeaderCharValueTextAlreadyExist(Convert.ToString(proposedValue), charGroupRid, valueRID, ref hcRidDuplicate);
                        }
                        else if (charGroupFieldType == fieldDataTypes.DateNoTime)
                        {
                            doesValueAlreadyExist = charMaintData.DoesHeaderCharValueDateAlreadyExist(Convert.ToDateTime(proposedValue), charGroupRid, valueRID, ref hcRidDuplicate);
                        }
                        else if (charGroupFieldType == fieldDataTypes.NumericDouble)
                        {
                            doesValueAlreadyExist = charMaintData.DoesHeaderCharValueNumberAlreadyExist(Convert.ToSingle(proposedValue), charGroupRid, valueRID, ref hcRidDuplicate);
                        }
                        else if (charGroupFieldType == fieldDataTypes.NumericDollar)
                        {
                            doesValueAlreadyExist = charMaintData.DoesHeaderCharValueDollarAlreadyExist(Convert.ToSingle(proposedValue), charGroupRid, valueRID, ref hcRidDuplicate);
                        }

                        if (doesValueAlreadyExist)
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
