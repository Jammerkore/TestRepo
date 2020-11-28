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
using MIDRetail.Business;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace MIDRetail.Windows.Controls
{
    public partial class MIDGridFieldEditorColumns : UserControl
    {
        public MIDGridFieldEditorColumns()
        {
            InitializeComponent();
        }

        private bool allowEdit;
        private bool forcingExitEditModeOnClose = false;  // TT#1821-MD - JSmith - Str Profile - Deselect Active and Cancel and receive the wrong mssg.
        
        public void Initialize(bool allowEdit, string formText)
        {
            this.midGridControl1.HideLayoutOnMenu();
            this.midGridControl1.doResize = true; 
            this.midGridControl1.doIncludeHeaderWhenResizing = true; 

            this.allowEdit = allowEdit;
            this.Text = formText;
            if (allowEdit)
            {
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.EditAndSelectText;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;

                this.midGridControl1.ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortSingle;
            }
            else
            {
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.ActiveAppearancesEnabled = DefaultableBoolean.False;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                //this.midGridControl1.ultraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.

                this.midGridControl1.ultraGrid1.DisplayLayout.RowSelectorImages.DataChangedImage = null;
                this.midGridControl1.ultraGrid1.DisplayLayout.RowSelectorImages.ActiveAndDataChangedImage = null;

                this.midGridControl1.ShowColumnChooser = false;
            }

            this.midGridControl1.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            //this.midGridControl1.ultraGrid1.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.RowFilterAction = RowFilterAction.None;

            //Disable column sorting
            //this.midGridControl1.ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
            //this.midGridControl1.ultraGrid1.DisplayLayout.Override.SelectTypeCol = SelectType.None;
          
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
            //this.midGridControl1.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
       
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.HeaderAppearance.TextHAlign = HAlign.Center;

            this.midGridControl1.gridInitializeLayoutCallback = new MIDGridControl.gridInitializeLayoutCallbackDelegate(Handle_InitializeLayout);
            this.midGridControl1.gridInitializeRowCallback = new MIDGridControl.gridInitializeRowCallbackDelegate(Handle_InitializeRow);
            this.midGridControl1.gridBeforeExitEditModeCallback = new MIDGridControl.gridBeforeExitEditModeCallbackDelegate(Handle_BeforeExitEditMode);
            this.midGridControl1.ExitEditModeOnReturnKeyPress = true;
            

        }


        public string objectNameText;


        private GetListValuesForField getListValuesCallback;
        private DataSet dsFields;
        private bool hasPendingChanges = false;
        List<fieldColumnMap> columnMapList;

        public void BindGrid(DataTable dtFields, GetListValuesForField getListValuesCallback, List<fieldColumnMap> columnMapList)
        {
            this.getListValuesCallback = getListValuesCallback;
            this.columnMapList = columnMapList;

            this.midGridControl1.HideColumn("OBJECT_RID");
            foreach (fieldColumnMap map in columnMapList)
            {
                if (map.isVisible == false)
                {
                    this.midGridControl1.HideColumn(map.columnName);
                }
            }

            dsFields = new DataSet();
            dsFields.Tables.Add(dtFields);

            this.midGridControl1.BindGrid(dsFields);
            this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns["OBJECT_NAME"].CellActivation = Activation.NoEdit;
            foreach (fieldColumnMap map in columnMapList)
            {
             

                if (map.fieldDataType == fieldDataTypes.List)
                {
                    int objectType = map.objectType;
                    int fieldIndex = map.fieldIndex; 
                    string valueListName = "vl" + objectType.ToString() + "-" + fieldIndex.ToString();
                    if (this.midGridControl1.ultraGrid1.DisplayLayout.ValueLists.Exists(valueListName) == false)
                    {
                        this.midGridControl1.ultraGrid1.DisplayLayout.ValueLists.Add(valueListName);
                        string dataField = string.Empty;
                        string displayField = string.Empty;
                        DataTable dtListValues = this.getListValuesCallback(objectType, fieldIndex, ref dataField, ref displayField);
                        foreach (DataRow drListValue in dtListValues.Rows)
                        {
                            int valueListData = Convert.ToInt32(drListValue[dataField]);
                            string valueListDisplay = drListValue[displayField].ToString();
                            this.midGridControl1.ultraGrid1.DisplayLayout.ValueLists[valueListName].ValueListItems.Add(valueListData, valueListDisplay);
                        }
                    }
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].ValueList = this.midGridControl1.ultraGrid1.DisplayLayout.ValueLists[valueListName];
                }

                // Begin TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
                if (map.fieldActualDataType == fieldDataTypes.Boolean)
                {
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].SortComparer = new UltraGridStringComparer();
                }
                else if (map.fieldActualDataType == fieldDataTypes.DateNoTime || map.fieldActualDataType == fieldDataTypes.DateOnlyTime || map.fieldActualDataType == fieldDataTypes.DateWithTime)
                {
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].SortComparer = new UltraGridDateComparer();
                }
                else if (map.fieldActualDataType == fieldDataTypes.NumericDollar || map.fieldActualDataType == fieldDataTypes.NumericDouble)
                {
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].SortComparer = new UltraGridDoubleComparer();
                }
                else if (map.fieldActualDataType == fieldDataTypes.NumericInteger)
                {
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].SortComparer = new UltraGridIntegerComparer();
                }
                else
                {
                    this.midGridControl1.ultraGrid1.DisplayLayout.Bands[0].Columns[map.columnIndex].SortComparer = new UltraGridStringComparer();
                }
                // End TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
            }

        }
        //public void BindList(List<StoreMaintenance.StoreObject> storeList)
        //{
        //    this.midGridControl1.ultraGrid1.DataSource = storeList;
        //}
       


        public void HideButtons()
        {
            this.ultraToolbarsManager1.Tools["btnOK"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnCancel"].SharedProps.Visible = false;
        }

        //public delegate filterDataTypes GetDataTypeFromFieldIndexDelegate(int fieldType, int fieldIndex);
        //public GetDataTypeFromFieldIndexDelegate getDataTypeFromFieldIndexDelegate;

        public delegate void FormatCellValueForField(int objectType, int fieldIndex, fieldDataTypes dataType, ref Infragistics.Win.UltraWinGrid.UltraGridCell cell);
        public FormatCellValueForField formatCellValueForField;

        public IsFieldValueValid isFieldValueValid; //validates a specific field, before leaving editing mode on the cell
        public IsObjectValidForFields isObjectValid; //validates the entire object, upon clicking OK
        public string pendingChangesMessage = "There are pending changes.  Do you wish to exit?";

        private void Handle_InitializeLayout(ref Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            

            //e.Layout.Bands[0].Columns["FIELD_NAME"].Format = "MM/dd/yyyy HH:mm:ss";
            //e.Layout.Bands[0].Columns["FIELD_NAME"].CellActivation = Activation.NoEdit;
            //e.Layout.Bands[0].Columns["FIELD_NAME"].Header.Caption = "Field";
            //e.Layout.Bands[0].Columns["FIELD_NAME"].AllowRowFiltering = DefaultableBoolean.False;

            //e.Layout.Bands[0].Columns["FIELD_VALUE"].CellActivation = Activation.AllowEdit;
            //e.Layout.Bands[0].Columns["FIELD_VALUE"].Header.Caption = "Value";
            //e.Layout.Bands[0].Columns["FIELD_VALUE"].Width = 200;
            //e.Layout.Bands[0].Columns["FIELD_VALUE"].AllowRowFiltering = DefaultableBoolean.False;
        }
        private void Handle_InitializeRow(ref Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e, DataSet ds)
        {

        }

        private DataRow currentRow;
        public DataTable dtAllValues;
        public string objectRidColumnName;
        private object FieldValueGetCurrent(int objectRID, int objectType, int fieldIndex)
        {
            //DataRow[] drField = dsFields.Tables[0].Select("OBJECT_INDEX = " + objectType.ToString() + " AND FIELD_INDEX=" + fieldIndex.ToString());
            //return drField[0]["FIELD_VALUE"];
            fieldColumnMap currentMap = this.columnMapList.Find(x => x.objectType == objectType && x.fieldIndex == fieldIndex);
            if (currentMap == null)
            {
                //the field is not in the currently selected datatable so use the all field table
                storeFieldTypes storeField = storeFieldTypes.FromIndex(fieldIndex);
                //int objectRID = (int)currentRow["OBJECT_RID"];
                DataRow[] drField = dtAllValues.Select(objectRidColumnName + "=" + objectRID.ToString());
                return drField[0][storeField.dbFieldName];
            }
            else
            {
                DataRow[] drField = this.dsFields.Tables[0].Select("OBJECT_RID=" + objectRID.ToString());
                return drField[0][currentMap.columnIndex];
            }
        }
        private void FieldValueSetCurrent(int objectRID, int objectType, int fieldIndex, object val)
        {
            //DataRow[] drField = dsFields.Tables[0].Select("OBJECT_INDEX = " + objectType.ToString() + " AND FIELD_INDEX=" + fieldIndex.ToString());
            //drField[0]["FIELD_VALUE"] = val;
            //currentMap.columnIndex
            fieldColumnMap currentMap = this.columnMapList.Find(x => x.objectType == objectType && x.fieldIndex == fieldIndex);
            if (currentMap != null)
            {
                DataRow[] drField = this.dsFields.Tables[0].Select("OBJECT_RID=" + objectRID.ToString());
                drField[0][currentMap.columnIndex] = val;
            }
        }



        private void Handle_BeforeExitEditMode(ref Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e, Infragistics.Win.UltraWinGrid.UltraGridRow activeRow, Infragistics.Win.UltraWinGrid.UltraGridCell activeCell, DataSet ds)
        {
            // If the user is canceling the modifications (for example by hitting Escape 
            // key, then just return because the cell will revert to its original value
            // in this case and not commit the user's input.
            if (e.CancellingEditOperation)
                return;

            // Begin TT#1806-MD - JSmith - Store Profiles - Receive Unhandled Exception when viewing drop down of Store Field / Str Characteristic and selecting the OK icon.  No changes were made.
            if (!activeCell.EditorResolved.IsValid)
            {
                return;
            }
            // End TT#1806-MD - JSmith - Store Profiles - Receive Unhandled Exception when viewing drop down of Store Field / Str Characteristic and selecting the OK icon.  No changes were made.

            if (activeRow.ListObject != null)
            {
                currentRow = ((DataRowView)activeRow.ListObject).Row;
                int objectRID = (int)currentRow["OBJECT_RID"];

                int columnIndex = activeCell.Column.Index;
                //EditMsgs em = new EditMsgs();
                List<MIDMsg> msgList = new List<MIDMsg>();
                fieldColumnMap currentMap = this.columnMapList.Find(x => x.columnIndex == columnIndex); 
                fieldDataTypes dataType = currentMap.fieldDataType;

                bool isValid = SharedControlRoutines.IsDateInValidFormat(dataType, activeCell, msgList);

                if (isValid)
                {
                    object proposedValue = activeCell.EditorResolved.Value;
                    object originalValue = activeCell.Value;
                    isValid = isFieldValueValid(validationKinds.BeforeExitEditMode, currentMap.objectType, currentMap.fieldIndex, objectRID, originalValue, proposedValue, msgList, new FieldValueGetForCurrentField(this.FieldValueGetCurrent), new FieldValueSetForCurrentField(this.FieldValueSetCurrent));
                }
                
                // Begin TT#1821-MD - JSmith - Str Profile - Deselect Active and Cancel and receive the wrong mssg.
                //if (isValid == false)
                if (isValid == false
                    && !forcingExitEditModeOnClose)
                // End TT#1821-MD - JSmith - Str Profile - Deselect Active and Cancel and receive the wrong mssg.
                {
                    FieldTypeUtility.ShowValidationMessage(msgList, this.Text);

                    // If ForceExit is true, then the UltraGrid will exit the edit mode
                    // regardless of whether you cancel this event or not. ForceExit would
                    // be true for example when the UltraGrid is being disposed of and thus
                    // it can't stay in edit mode. In which case setting Cancel won't do
                    // any good so just cancel the update to revert the cell's value back
                    // to its original value.
                    if (e.ForceExit)
                    {
                        // If the UltraGrid must exit the edit mode, then cancel the
                        // cell update so the original value gets restored in the cell.
                        activeCell.CancelUpdate();
                        return;
                    }

                    // In normal circumstances where ForceExit is false, set Cancel to 
                    // true so the UltraGrid doesn't exit the edit mode.
                    e.Cancel = true;
                    return;
                }
                hasPendingChanges = true;
            }
        }

   

        //private void SetFieldAsChanged(int objectType, int fieldIndex)
        //{
        //    DataRow[] drfound = this.dsFields.Tables[0].Select("OBJECT_TYPE=" + objectType.ToString() + " AND FIELD_INDEX=" + fieldIndex.ToString());
        //    if (drfound.Length > 0)
        //    {
        //        drfound[0]["HAS_CHANGED"] = true;
        //    }
        //}


   


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (e.Tool.Key == "btnOK")
            {
                if (this.midGridControl1.ultraGrid1.ActiveCell != null && this.midGridControl1.ultraGrid1.ActiveCell.IsInEditMode)
                {
                    this.midGridControl1.ultraGrid1.PerformAction(UltraGridAction.ExitEditMode);
                    if (this.midGridControl1.ultraGrid1.ActiveCell.IsInEditMode == false)
                    {
                        this.midGridControl1.ultraGrid1.PerformAction(UltraGridAction.CommitRow);
                        this.midGridControl1.ultraGrid1.Update();
                   
                        DoSave();
                    }
                }
                else
                {
                    this.midGridControl1.ultraGrid1.PerformAction(UltraGridAction.CommitRow);
                    this.midGridControl1.ultraGrid1.Update();
                    DoSave();
                }
            }
            else if (e.Tool.Key == "btnCancel")
            {
                RaiseCloseFormEvent();
            }
        }

        private void DoSave()
        {
            bool canSave = true;

            //Valudate the entire object - allows for both warnings and object level validaiton errors
            if (this.isObjectValid != null)
            {
                //EditMsgs msgs = new EditMsgs();
                List<MIDMsg> msgList = new List<MIDMsg>();
                bool isValid = this.isObjectValid(ref this.dsFields, msgList, this.columnMapList, new FieldValueGetForCurrentField(this.FieldValueGetCurrent), new FieldValueSetForCurrentField(this.FieldValueSetCurrent));
                int warningMsgCount = msgList.FindAll(x => x.msgLevel == eMIDMessageLevel.Warning).Count;

                if (isValid == false && warningMsgCount > 0) //using ErrorToDisplay as a flag for warnings
                {
                    //Display a Warning Prompt to allow users to proceed
                    //EditMsgs.Message msg = (EditMsgs.Message)msgs.EditMessages[0];
                    MIDMsg mMsg = msgList.Find(x => x.msgLevel == eMIDMessageLevel.Warning);
                    //string promptText = MIDText.GetTextOnly(mMsg.msg);
                    if (MessageBox.Show(mMsg.msg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        canSave = false;
                    }
                }
                else if (isValid == false)
                {
                    //Show the Error message
                    FieldTypeUtility.ShowValidationMessage(msgList, this.Text);
                     canSave = false;
                }
            }

            if (canSave)
            {
                hasPendingChanges = false;
                RaiseSaveEvent();
                RaiseCloseFormEvent();
            }

        }

        public bool CanClose()
        {
            if (hasPendingChanges) //Display a prompt if they have pending changes and did not save
            {
                if (MessageBox.Show(pendingChangesMessage, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        // Begin TT#1831-MD - JSmith - Edit Store or Fields, make change select black X to Close window and do not receive mssg on pending changes.
        public void GridExitEditMode()
        {
            // Take the cell out of edit mode so the pending flag gets set
            if (this.midGridControl1 != null
                && this.midGridControl1.ultraGrid1 != null)
            {
                forcingExitEditModeOnClose = true;
                this.midGridControl1.ultraGrid1.PerformAction(UltraGridAction.ExitEditMode);
                forcingExitEditModeOnClose = false;
            }
        }
        // End TT#1831-MD - JSmith - Edit Store or Fields, make change select black X to Close window and do not receive mssg on pending changes.

        public delegate void SaveEventHandler(object sender, SaveEventArgs e);
        public event SaveEventHandler SaveEvent;
        public void RaiseSaveEvent()
        {
            if (SaveEvent != null)
                SaveEvent(new object(), new SaveEventArgs(this.dsFields, this.columnMapList));
        }
        public class SaveEventArgs
        {
            public SaveEventArgs(DataSet dsValuesToSave, List<fieldColumnMap> columnMapList) { this.dsValuesToSave = dsValuesToSave; this.columnMapList = columnMapList; }
            public DataSet dsValuesToSave { get; private set; }
            public List<fieldColumnMap> columnMapList { get; private set; }
        }


        public delegate void CloseFormEventHandler(object sender, CloseFormEventArgs e);
        public event CloseFormEventHandler CloseFormEvent;
        public void RaiseCloseFormEvent()
        {
            if (CloseFormEvent != null)
                //CloseFormEvent(new object(), new CloseFormEventArgs());
            // Begin TT#1813-MD - JSmith - Str Profiles - Select Edit Field - Change State from CA to AR- select the Cancel and do not receive mssg on pending changes.  Msg="There are pending changes.  Do you wish to exit?"  Code=060098 
            {
                // Take the cell out of edit mode so the pending flag gets set
                // Begin TT#1831-MD - JSmith - Edit Store or Fields, make change select black X to Close window and do not receive mssg on pending changes.
                //if (this.midGridControl1 != null
                //    && this.midGridControl1.ultraGrid1 != null)
                //{
                //    forcingExitEditModeOnClose = true;  // TT#1821-MD - JSmith - Str Profile - Deselect Active and Cancel and receive the wrong mssg.
                //    this.midGridControl1.ultraGrid1.PerformAction(UltraGridAction.ExitEditMode);
                //    forcingExitEditModeOnClose = false;  // TT#1821-MD - JSmith - Str Profile - Deselect Active and Cancel and receive the wrong mssg.
                //}
                GridExitEditMode();
                // End TT#1831-MD - JSmith - Edit Store or Fields, make change select black X to Close window and do not receive mssg on pending changes.
                CloseFormEvent(new object(), new CloseFormEventArgs());
            }
            // End TT#1831-MD - JSmith - Str Profiles - Select Edit Field - Change State from CA to AR- select the Cancel and do not receive mssg on pending changes.  Msg="There are pending changes.  Do you wish to exit?"  Code=060098 
        }
        public class CloseFormEventArgs
        {
            public CloseFormEventArgs() { }
        }
    } //MIDGridFieldEditor
}
