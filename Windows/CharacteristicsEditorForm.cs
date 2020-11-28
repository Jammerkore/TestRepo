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
    public partial class CharacteristicsEditorForm : Form
    {
        private SessionAddressBlock SAB;
        private ExplorerAddressBlock EAB;
        private charObjectTypes charObjectType;
        private string charGroupRidField;
        public int charGroupRID = -1;
        private string charGroupID = string.Empty;
        private int valueRID = -1;
        private fieldDataTypes charGroupFieldDataType = null;
        private currentlyEditingType currentlyEditing;
        public bool hasValueChanged = false;
        public bool hasGroupChanged = false;
        public bool hasValueBeenInserted = false;
        public bool hasGroupBeenInserted = false;
        public bool checkForStoreLoadAPI = false;
		// Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
        public string originalValueAsString;  
        public string currentValueAsString;
		// End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.

        private enum currentlyEditingType
        {
            CharGroup = 0,
            Value = 1
        }

        public CharacteristicsEditorForm(SessionAddressBlock SAB, ExplorerAddressBlock EAB, charObjectTypes charObjectType, string charGroupRidField)
        {
            this.SAB = SAB;
            this.EAB = EAB;
            this.charObjectType = charObjectType;
            this.charGroupRidField = charGroupRidField;
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
        }



        private void CharacteristicsEditorForm_Load(object sender, EventArgs e)
        {
            this.gridFields.CloseFormEvent += new MIDGridFieldEditor.CloseFormEventHandler(Handle_CloseForm);
            this.gridFields.SaveEvent += new MIDGridFieldEditor.SaveEventHandler(Handle_Save);
        }
        private void CharacteristicsEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.gridFields.CanClose() == false)
            {
                e.Cancel = true;
            }
        }

        private void Handle_CloseForm(object sender, MIDGridFieldEditor.CloseFormEventArgs e)
        {
            this.Close();
        }

        private void Handle_Save(object sender, MIDGridFieldEditor.SaveEventArgs e)
        {
            //At this point all fields and the object have been validated

            if (checkForStoreLoadAPI)
            {
                //Do not allow saving if Store Load API is running
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                    return;
                }
            }



            DataRow drToSave = e.dsValuesToSave.Tables[0].Rows[0];

            if (this.currentlyEditing == currentlyEditingType.Value)
            {
                if (this.valueRID == -1)
                {
                    //Add new value
                    AddCharValue(this.charGroupRID, this.charGroupFieldDataType, drToSave["FIELD_VALUE"]);
                    hasValueBeenInserted = true;
                }
                else
                {
                    //Update value
                    UpdateCharValue(this.valueRID, this.charGroupRID, this.charGroupFieldDataType, drToSave["FIELD_VALUE"]);
                    hasValueChanged = true;
                }
            }
            else if (this.currentlyEditing == currentlyEditingType.CharGroup)
            {
                if (this.charGroupRID == -1)
                {
                    //Add new char group
                    this.charGroupRID = charGroupInsert(e.dsValuesToSave.Tables[0]);
                    hasGroupBeenInserted = true;
                }
                else
                {
                    //Update char group
                    charGroupUpdate(this.charGroupRID, e.dsValuesToSave.Tables[0]);
                    hasGroupChanged = true;
                }
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


        #region "Characteristic Groups"
        public delegate void CharGroupGetDataForInsert(charObjectTypes charObjectType, DataTable dtCharGroupFields);
        public CharGroupGetDataForInsert charGroupGetDataForInsert;

        public void PopulateFieldsForCharAdd(string formText)
        {
            this.Text = "Characteristic [New]";
            currentlyEditing = currentlyEditingType.CharGroup;
            this.charGroupRID = -1;

            this.gridFields.Initialize(true, formText);
            this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
            this.gridFields.isFieldValueValid = new IsFieldValueValid(isCharGroupValid);
            this.gridFields.isObjectValid = new IsObjectValid(IsCharGroupObjectValid);
            this.gridFields.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);

           


            DataTable dtCharGroupFields = FieldTypeUtility.GetDataTableForFields();


            charGroupGetDataForInsert(this.charObjectType, dtCharGroupFields);


            dtCharGroupFields.AcceptChanges();
            this.gridFields.BindGrid(Include.NoRID, dtCharGroupFields, new GetListValuesForField(charTypes.GetListValuesForCharGroups));

        }

        public delegate void CharGroupGetDataForUpdate(charObjectTypes charObjectType, DataTable dtCharGroupFields, DataRow drCharGroup, bool hasValuesInGroup);
        public CharGroupGetDataForUpdate charGroupGetDataForUpdate;

        public void PopulateFieldsForCharEdit(string formText, DataRow drCharGroup)
        {
            this.Text = "Characteristic [Edit]";
            currentlyEditing = currentlyEditingType.CharGroup;
            this.charGroupRID = (int)drCharGroup[charGroupRidField];

            this.gridFields.Initialize(true, formText);
            this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
            this.gridFields.isFieldValueValid = new IsFieldValueValid(isCharGroupValid);
            this.gridFields.isObjectValid = new IsObjectValid(IsCharGroupObjectValid);
            this.gridFields.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);

            DataTable dtCharGroupFields = FieldTypeUtility.GetDataTableForFields();

            DataTable dtValues = StoreMgmt.StoreCharGroup_ReadValues(this.charGroupRID);

            bool doesGroupHasValuesDefined = false;
            if (dtValues != null && dtValues.Rows.Count > 0)
            {
                doesGroupHasValuesDefined = true;
            }

            charGroupGetDataForUpdate(this.charObjectType, dtCharGroupFields, drCharGroup, doesGroupHasValuesDefined);
            dtCharGroupFields.AcceptChanges();

            this.gridFields.BindGrid(this.charGroupRID, dtCharGroupFields, new GetListValuesForField(charTypes.GetListValuesForCharGroups));

        }

        public delegate bool IsCharGroupValid(validationKinds validationKind, int objectType, int fieldIndex, int charGroupRID, object originalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent);
        public IsCharGroupValid isCharGroupValid;

        public bool IsCharGroupObjectValid(int charGroupRID, ref DataSet dsFields, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;

            if (charGroupRID == Include.NoRID)
            {
                foreach (DataRow cRow in dsFields.Tables[0].Rows)  //here each row is a single field, so we know each of these fields changed
                {
                    int objectType = Convert.ToInt32(cRow["OBJECT_TYPE"]);
                    int fieldIndex = Convert.ToInt32(cRow["FIELD_INDEX"]);
                    object curVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Current];
                    object origVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Original];

                    isValid = isCharGroupValid(validationKinds.UponSaving, objectType, fieldIndex, charGroupRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                    if (isValid == false)
                    {
                        break;
                    }
                }
             
            }
            else
            {
                DataTable dtChanges = dsFields.Tables[0].GetChanges(DataRowState.Modified);  // Process Rows that were CHANGED

                if (dtChanges != null)
                {
                    foreach (DataRow cRow in dtChanges.Rows)  //here each row is a single field, so we know each of these fields changed
                    {
                        int objectType = Convert.ToInt32(cRow["OBJECT_TYPE"]);
                        int fieldIndex = Convert.ToInt32(cRow["FIELD_INDEX"]);
                        object curVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Current];
                        object origVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Original];

                        isValid = isCharGroupValid(validationKinds.UponSaving, objectType, fieldIndex, charGroupRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                        if (isValid == false)
                        {
                            break;
                        }
                    }
                }
            }

            return isValid;
        }

        public delegate int CharGroupInsert(DataTable dtGroupValues);
        public CharGroupInsert charGroupInsert;  //called when saving


        public delegate void CharGroupUpdate(int charGroupRID, DataTable dtGroupValues);
        public CharGroupUpdate charGroupUpdate; //called when saving

        #endregion


        #region "Characteristic Values"
        public void PopulateFieldsForValueAdd(string formText, int charGroupRID, string charGroupID, fieldDataTypes fieldDataType)
        {
            this.Text = "Characteristic Value [New]";
            currentlyEditing = currentlyEditingType.Value;
            this.valueRID = -1;

            this.charGroupRID = charGroupRID;
            this.charGroupID = charGroupID;
            this.charGroupFieldDataType = fieldDataType;

            this.gridFields.Initialize(true, formText);
            this.gridFields.HideColumn("FIELD_NAME");
            this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
            this.gridFields.isFieldValueValid = new IsFieldValueValid(IsCharValueValidWrapper);
            this.gridFields.isObjectValid = new IsObjectValid(IsCharValueObjectValid);

            this.gridFields.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);


            DataTable dtValue = FieldTypeUtility.GetDataTableForFields();

            DataRow dr = dtValue.NewRow();
            dr["OBJECT_TYPE"] = charObjectType.Index;
            dr["FIELD_INDEX"] = Include.NoRID;
            dr["FIELD_TYPE"] = this.charGroupFieldDataType.Index;
            dr["ALLOW_EDIT"] = true;
            dr["MAX_LENGTH"] = 50;
            dr["FIELD_NAME"] = string.Empty;
            dr["FIELD_VALUE"] = DBNull.Value;
            dtValue.Rows.Add(dr);


            dtValue.AcceptChanges();
            this.gridFields.BindGrid(Include.NoRID, dtValue, null);

        }

        public void PopulateFieldsForValueEdit(string formText, int charGroupRID, string charGroupID, fieldDataTypes fieldDataType, int valueRID, object val)
        {
            this.Text = "Characteristic Value [Edit]";
            currentlyEditing = currentlyEditingType.Value;

            this.charGroupRID = charGroupRID;
            this.charGroupID = charGroupID;
            this.charGroupFieldDataType = fieldDataType;
            this.valueRID = valueRID;


            this.gridFields.Initialize(true, formText);
            this.gridFields.HideColumn("FIELD_NAME");
            this.gridFields.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);
            this.gridFields.isFieldValueValid = new IsFieldValueValid(IsCharValueValidWrapper);
            this.gridFields.isObjectValid = new IsObjectValid(IsCharValueObjectValid);

            this.gridFields.pendingChangesMessage = MIDText.GetTextOnly(eMIDTextCode.msg_PendingChanges);

            DataTable dtFields = FieldTypeUtility.GetDataTableForFields();

            DataRow dr = dtFields.NewRow();
            dr["OBJECT_TYPE"] = charObjectType.Index;
            dr["FIELD_INDEX"] = valueRID;
            dr["FIELD_TYPE"] = this.charGroupFieldDataType.Index;
            dr["ALLOW_EDIT"] = true;
            dr["MAX_LENGTH"] = 50;
            dr["FIELD_NAME"] = string.Empty;
            dr["FIELD_VALUE"] = val;
            dtFields.Rows.Add(dr);


            dtFields.AcceptChanges();
            this.gridFields.BindGrid(this.charGroupRID, dtFields, null);

        }




        public delegate bool IsCharValueValid(int charGroupRid, fieldDataTypes charGroupFieldType, validationKinds validationKind, int objectType, int fieldIndex, int valueRID, object originalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent);
        public IsCharValueValid isCharValueValid;
        public bool IsCharValueValidWrapper(validationKinds validationKind, int objectType, int fieldIndex, int objectRID, object originalValue, object newValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            return isCharValueValid(this.charGroupRID, this.charGroupFieldDataType, validationKind, objectType, fieldIndex, objectRID, originalValue, newValue, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
        }


        public bool IsCharValueObjectValid(int valueRID, ref DataSet dsFields, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;

            if (valueRID == Include.NoRID)
            {
                foreach (DataRow cRow in dsFields.Tables[0].Rows)  //here each row is a single field, so we know each of these fields changed
                {
                    int objectType = Convert.ToInt32(cRow["OBJECT_TYPE"]);
                    int fieldIndex = Convert.ToInt32(cRow["FIELD_INDEX"]);
                    object curVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Current];
                    object origVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Original];

                    isValid = isCharValueValid(this.charGroupRID, this.charGroupFieldDataType, validationKinds.UponSaving, objectType, fieldIndex, valueRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                    if (isValid == false)
                    {
                        break;
                    }

                    if (curVal != origVal)
                    {
                        cRow["IS_DIRTY"] = true;
						// Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                        originalValueAsString = Convert.ToString(origVal);
                        currentValueAsString = Convert.ToString(curVal);
						// End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                    }
                    else
                    {
                        cRow["IS_DIRTY"] = false;
                    }
                }
             
            }
            else
            {
                DataTable dtChanges = dsFields.Tables[0].GetChanges(DataRowState.Modified);  // Process Rows that were CHANGED

                if (dtChanges != null)
                {
                    foreach (DataRow cRow in dtChanges.Rows)  //here each row is a single field, so we know each of these fields changed
                    {
                        int objectType = Convert.ToInt32(cRow["OBJECT_TYPE"]);
                        int fieldIndex = Convert.ToInt32(cRow["FIELD_INDEX"]);
                        object curVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Current];
                        object origVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Original];

                        isValid = isCharValueValid(this.charGroupRID, this.charGroupFieldDataType, validationKinds.UponSaving, objectType, fieldIndex, valueRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                        if (isValid == false)
                        {
                            break;
                        }

                        if (curVal != origVal)
                        {
                            cRow["IS_DIRTY"] = true;
                            // Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                            originalValueAsString = Convert.ToString(origVal);
                            currentValueAsString = Convert.ToString(curVal);
                            // End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                        }
                        else
                        {
                            cRow["IS_DIRTY"] = false;
                        }
                    }
                }
            }

            return isValid;
        }
        public delegate int ValueInsert(int scgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue, string groupID, string valueAsString);
        public ValueInsert valueInsert;

        private void AddCharValue(int scgRID, fieldDataTypes dataType, object val)
        {
            string stringVal = Include.NullForStringValue;
            DateTime? dateVal = null;
            float? numericVal = null;
            float? dollarVal = null;
            string valueAsString = string.Empty;
            if (dataType == fieldDataTypes.DateNoTime)
            {
                dateVal = Convert.ToDateTime(val);
                valueAsString = dateVal.ToString();
            }
            else if (dataType == fieldDataTypes.NumericDouble)
            {
                numericVal = Convert.ToSingle(val);
                valueAsString = numericVal.ToString();
            }
            else if (dataType == fieldDataTypes.NumericDollar)
            {
                dollarVal = Convert.ToSingle(val);
                valueAsString = dollarVal.ToString();
            }
            else
            {
                stringVal = val.ToString();
                valueAsString = val.ToString();
            }

            valueInsert(scgRID, stringVal, dateVal, numericVal, dollarVal, this.charGroupID, valueAsString);
        }

        public delegate void ValueUpdate(int scRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue, string groupID, string valueAsString);
        public ValueUpdate valueUpdate;

        private void UpdateCharValue(int scRID, int scgRID, fieldDataTypes dataType, object val)
        {
            string stringVal = Include.NullForStringValue;
            DateTime? dateVal = null;
            float? numericVal = null;
            float? dollarVal = null;
            string valueAsString = string.Empty;
            if (dataType == fieldDataTypes.DateNoTime)
            {
                dateVal = Convert.ToDateTime(val);
                valueAsString = dateVal.ToString();
            }
            else if (dataType == fieldDataTypes.NumericDouble)
            {
                numericVal = Convert.ToSingle(val);
                valueAsString = numericVal.ToString();
            }
            else if (dataType == fieldDataTypes.NumericDollar)
            {
                dollarVal = Convert.ToSingle(val);
                valueAsString = dollarVal.ToString();
            }
            else
            {
                stringVal = val.ToString();
                valueAsString = val.ToString();
            }

            valueUpdate(scRID, stringVal, dateVal, numericVal, dollarVal, this.charGroupID, valueAsString);
        }
        #endregion


    }
}
