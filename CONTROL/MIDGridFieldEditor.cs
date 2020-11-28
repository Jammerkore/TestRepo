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
    public partial class MIDGridFieldEditor : UserControl
    {
        public MIDGridFieldEditor()
        {
            InitializeComponent();
        }

        private bool allowEdit;
        private bool forcingExitEditModeOnClose = false;  // TT#1821-MD - JSmith - Str Profile - Deselect Active and Cancel and receive the wrong mssg.
        public void Initialize(bool allowEdit, string formText)
        {

            this.midGridControl1.HideLayoutOnMenu();


            this.allowEdit = allowEdit;
            this.Text = formText;
            
            if (allowEdit)
            {
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.EditAndSelectText;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
                this.midGridControl1.ultraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;

         
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
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.SelectTypeCol = SelectType.None;
          
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
            this.midGridControl1.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
       
            this.midGridControl1.ultraGrid1.DisplayLayout.Override.HeaderAppearance.TextHAlign = HAlign.Center;

            this.midGridControl1.gridInitializeLayoutCallback = new MIDGridControl.gridInitializeLayoutCallbackDelegate(Handle_InitializeLayout);
            this.midGridControl1.gridInitializeRowCallback = new MIDGridControl.gridInitializeRowCallbackDelegate(Handle_InitializeRow);
            this.midGridControl1.gridBeforeExitEditModeCallback = new MIDGridControl.gridBeforeExitEditModeCallbackDelegate(Handle_BeforeExitEditMode);
            this.midGridControl1.ExitEditModeOnReturnKeyPress = true;
        }





        private GetListValuesForField getListValuesCallback;
        private DataSet dsFields;
        private int objectRID;
        private bool hasPendingChanges = false;

        public void BindGrid(int objectRID, DataTable dtFields, GetListValuesForField getListValuesCallback)
        {
            this.objectRID = objectRID;
            this.getListValuesCallback = getListValuesCallback;

            this.midGridControl1.HideColumn("OBJECT_TYPE");
            this.midGridControl1.HideColumn("FIELD_INDEX");
            this.midGridControl1.HideColumn("FIELD_TYPE");
            this.midGridControl1.HideColumn("ALLOW_EDIT");
            this.midGridControl1.HideColumn("MAX_LENGTH");
            this.midGridControl1.HideColumn("IS_DIRTY");
            
            dsFields = new DataSet();
            dsFields.Tables.Add(dtFields);
           
            this.midGridControl1.BindGrid(dsFields);

        }
  
        public List<DataRow> GetSelectedRows()
        {
            List<DataRow> selectedFieldList = new List<DataRow>();

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow urSelected in this.midGridControl1.ultraGrid1.Selected.Rows)
            {
                if (urSelected.ListObject != null)
                {
                    DataRow drSelected = ((DataRowView)urSelected.ListObject).Row;
                    selectedFieldList.Add(drSelected);
                }         
            }

            return selectedFieldList;
        }
        public DataRow GetFirstSelectedRow()
        {
            if (this.midGridControl1.ultraGrid1.Selected.Rows.Count > 0)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.midGridControl1.ultraGrid1.Selected.Rows[0];
                if (urFirst.ListObject != null)
                {
                    return ((DataRowView)urFirst.ListObject).Row;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public void HideButtons()
        {
            this.ultraToolbarsManager1.Tools["btnOK"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnCancel"].SharedProps.Visible = false;
        }
        public void HideColumn(string dataFieldName)
        {
            this.midGridControl1.HideColumn(dataFieldName);
        }

       
        public delegate void FormatCellValueForField(int objectType, int fieldIndex, fieldDataTypes dataType, ref Infragistics.Win.UltraWinGrid.UltraGridCell cell);
        public FormatCellValueForField formatCellValueForField;

        public IsFieldValueValid isFieldValueValid; //validates a specific field, before leaving editing mode on the cell
        public IsObjectValid isObjectValid; //validates the entire object, upon clicking OK
        public string pendingChangesMessage = "There are pending changes.  Do you wish to exit?";


       


        private void Handle_InitializeLayout(ref Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["FIELD_NAME"].Format = "MM/dd/yyyy HH:mm:ss";
            e.Layout.Bands[0].Columns["FIELD_NAME"].CellActivation = Activation.NoEdit;
            //e.Layout.Bands[0].Columns["FIELD_NAME"].CellClickAction = CellClickAction.CellSelect;
            e.Layout.Bands[0].Columns["FIELD_NAME"].Header.Caption = "Field";
            e.Layout.Bands[0].Columns["FIELD_NAME"].AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Bands[0].Columns["FIELD_VALUE"].CellActivation = Activation.AllowEdit;
            //e.Layout.Bands[0].Columns["FIELD_VALUE"].CellClickAction = CellClickAction.EditAndSelectText;
            e.Layout.Bands[0].Columns["FIELD_VALUE"].Header.Caption = "Value";
            e.Layout.Bands[0].Columns["FIELD_VALUE"].Width = 200;
            e.Layout.Bands[0].Columns["FIELD_VALUE"].AllowRowFiltering = DefaultableBoolean.False;
        }
        private void Handle_InitializeRow(ref Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e, DataSet ds)
        {
            if (e.Row.ListObject != null)
            {
                DataRow dr = ((DataRowView)e.Row.ListObject).Row;
                int fieldType = Convert.ToInt32(dr["FIELD_TYPE"]);
                fieldDataTypes dataType = fieldDataTypes.FromIndex(fieldType);

                bool allowEdit = (bool)dr["ALLOW_EDIT"];
                if (allowEdit == false)
                {
                    UltraGridCell cell = e.Row.Cells["FIELD_VALUE"];
                    cell.Activation = Activation.NoEdit;
                }

                if (formatCellValueForField != null)
                {
                    int objectType = Convert.ToInt32(dr["OBJECT_TYPE"]);
                    int fieldIndex = Convert.ToInt32(dr["FIELD_INDEX"]);
                    Infragistics.Win.UltraWinGrid.UltraGridCell cell = e.Row.Cells["FIELD_VALUE"];
                    formatCellValueForField(objectType, fieldIndex, dataType, ref cell);
                }


                if (dataType == fieldDataTypes.Boolean)
                {
                    Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBox = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
                    chkBox.UseAppStyling = false;
                    e.Row.Cells["FIELD_VALUE"].EditorComponent = chkBox;
                }
                if (dataType == fieldDataTypes.List)
                {
                    int objectType = Convert.ToInt32(dr["OBJECT_TYPE"]);
                    int fieldIndex = Convert.ToInt32(dr["FIELD_INDEX"]);
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

                    e.Row.Cells["FIELD_VALUE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    e.Row.Cells["FIELD_VALUE"].ValueList = this.midGridControl1.ultraGrid1.DisplayLayout.ValueLists[valueListName];
                }
                else if (dataType == fieldDataTypes.DateNoTime)
                {
                    Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dte = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
                    dte.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
                    dte.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
                    dte.MaskInput = "{date}";
                    dte.UseAppStyling = false;
                    //dte.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
                    e.Row.Cells["FIELD_VALUE"].EditorComponent = dte;
                }
                else if (dataType == fieldDataTypes.DateWithTime)
                {
                    Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dte = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
                    dte.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
                    dte.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
                    dte.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    dte.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    dte.MaskInput = "{LOC}mm/dd/yyyy hh:mm tt"; 
                    dte.UseAppStyling = false;
                    //dte.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
                    e.Row.Cells["FIELD_VALUE"].EditorComponent = dte;
                }
                else if (dataType == fieldDataTypes.DateOnlyTime)
                {
                    Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dte = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
                    dte.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
                    dte.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
                    dte.MaskInput = "{time}"; 
                    dte.UseAppStyling = false;
                    //dte.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
                    e.Row.Cells["FIELD_VALUE"].EditorComponent = dte;
                }
                else if (dataType == fieldDataTypes.NumericDollar || dataType == fieldDataTypes.NumericDouble || dataType == fieldDataTypes.NumericInteger)
                {
                    Infragistics.Win.UltraWinEditors.UltraNumericEditor une = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
                
                    string val = e.Row.Cells["FIELD_VALUE"].Value.ToString();




                    if (val == string.Empty)
                    {
                        e.Row.Cells["FIELD_VALUE"].Value = DBNull.Value;

                        if (dataType == fieldDataTypes.NumericInteger)
                        {
                            une.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Integer;
                        }
                        else
                        {
                            une.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
                        }
                    }
                    else
                    {
                        if (dataType == fieldDataTypes.NumericInteger)
                        {
                            une.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Integer;
                            int valInt;
                            if (int.TryParse(val, out valInt) == false)
                            {
                                string valTrimmed = val.Trim();
                                if (int.TryParse(valTrimmed, out valInt) == false)
                                {
                                    MessageBox.Show("Value " + val + " is not a valid integer.  Value will be changed to zero.");
                                    e.Row.Cells["FIELD_VALUE"].Value = "0";
                                }
                                else
                                {
                                    e.Row.Cells["FIELD_VALUE"].Value = valTrimmed;
                                }
                            }
                        }
                        else
                        {
                            une.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
                            double valDouble;
                            if (double.TryParse(val, out valDouble) == false)
                            {
                                string valTrimmed = val.Trim();
                                if (double.TryParse(valTrimmed, out valDouble) == false)
                                {
                                    MessageBox.Show("Value " + val + " is not a valid number.  Value will be changed to zero.");
                                    e.Row.Cells["FIELD_VALUE"].Value = "0.0";
                                }
                                else
                                {
                                    e.Row.Cells["FIELD_VALUE"].Value = valTrimmed;
                                }
                            }
                        }
                    }
                 
                    SharedControlRoutines.SetMaskForNumericEditor(dataType, une);
                    une.UseAppStyling = false;

                    

                    //une.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
                    e.Row.Cells["FIELD_VALUE"].EditorComponent = une;

                }
                else if (dataType == fieldDataTypes.Text)
                {
                    Infragistics.Win.UltraWinEditors.UltraTextEditor ute = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
                    ute.UseAppStyling = false;
                    int maxLength = Convert.ToInt32(dr["MAX_LENGTH"]);
                    ute.MaxLength = maxLength;
                    //ute.Width = 100;
                    //ute.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
                    //EditorWithText textEditor = new EditorWithText( );
                    e.Row.Cells["FIELD_VALUE"].EditorComponent = ute;
                }

            }
        }



        private object FieldValueGetCurrent(int objectRID, int objectType, int fieldIndex)
        {
                DataRow[] drField = dsFields.Tables[0].Select("OBJECT_TYPE = " + objectType.ToString() + " AND FIELD_INDEX=" + fieldIndex.ToString());
                return drField[0]["FIELD_VALUE"];
        }
        private void FieldValueSetCurrent(int objectRID, int objectType, int fieldIndex, object val)
        {
            DataRow[] drField = dsFields.Tables[0].Select("OBJECT_TYPE = " + objectType.ToString() + " AND FIELD_INDEX=" + fieldIndex.ToString());
            drField[0]["FIELD_VALUE"] = val;
        }

        private void Handle_BeforeExitEditMode(ref Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e, Infragistics.Win.UltraWinGrid.UltraGridRow activeRow, Infragistics.Win.UltraWinGrid.UltraGridCell activeCell, DataSet ds)
        {
            // If the user is canceling the modifications (for example by hitting Escape 
            // key, then just return because the cell will revert to its original value
            // in this case and not commit the user's input.
            if (e.CancellingEditOperation)
                return;

            if (activeRow.ListObject != null)
            {
                DataRow currentRow = ((DataRowView)activeRow.ListObject).Row;
                int objectType = Convert.ToInt32(currentRow["OBJECT_TYPE"]);
                int fieldIndex = Convert.ToInt32(currentRow["FIELD_INDEX"]);
                //string proposedValue = activeCell.Text;

                List<MIDMsg> msgList = new List<MIDMsg>();

                int fieldType = Convert.ToInt32(currentRow["FIELD_TYPE"]);
                fieldDataTypes dataType = fieldDataTypes.FromIndex(fieldType);

                bool isValid = SharedControlRoutines.IsDateInValidFormat(dataType, activeCell, msgList);

                if (isValid)
                {
                    if (activeCell.EditorResolved.IsValid == false)
                    {
                        //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CellValueNotValid, MIDText.GetTextOnly(eMIDTextCode.msg_CellValueNotValid), this.ParentForm.Text);
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_CellValueNotValid, msg = MIDText.GetTextOnly(eMIDTextCode.msg_CellValueNotValid) });
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    
                    object proposedValue = activeCell.EditorResolved.Value;
                    object origValue = activeCell.Value;
                    isValid = isFieldValueValid(validationKinds.BeforeExitEditMode, objectType, fieldIndex, this.objectRID, origValue, proposedValue, msgList, new FieldValueGetForCurrentField(this.FieldValueGetCurrent), new FieldValueSetForCurrentField(this.FieldValueSetCurrent));
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

                bool isValid = this.isObjectValid(this.objectRID, ref this.dsFields, msgList, new FieldValueGetForCurrentField(this.FieldValueGetCurrent), new FieldValueSetForCurrentField(this.FieldValueSetCurrent));
                int warningMsgCount = msgList.FindAll(x => x.msgLevel == eMIDMessageLevel.Warning).Count;

                if (isValid == false && warningMsgCount > 0) //using DisplayPrompt as a flag for warnings
                {
                    //Display a Warning Prompt to allow users to proceed
                    //EditMsgs.Message msg = (EditMsgs.Message)msgs.EditMessages[0];
                    MIDMsg mMsg = msgList.Find(x => x.msgLevel == eMIDMessageLevel.Warning);
                    //string promptText = MIDText.GetTextOnly(mMsg.textCode);
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
                SaveEvent(new object(), new SaveEventArgs(this.dsFields));
        }
        public class SaveEventArgs
        {
            public SaveEventArgs(DataSet dsValuesToSave) { this.dsValuesToSave = dsValuesToSave; }
            public DataSet dsValuesToSave { get; private set; }
        }


        public delegate void CloseFormEventHandler(object sender, CloseFormEventArgs e);
        public event CloseFormEventHandler CloseFormEvent;
        public void RaiseCloseFormEvent()
        {
            if (CloseFormEvent != null)
            // Begin TT#1811-MD - JSmith - Str Profile - Edit Store - make a change and select X and no mssg received. Msg="There are pending changes.  Do you wish to exit?"  Code=060098 
                //CloseFormEvent(new object(), new CloseFormEventArgs());
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
            // End TT#1811-MD - JSmith - Str Profile - Edit Store - make a change and select X and no mssg received. Msg="There are pending changes.  Do you wish to exit?"  Code=060098 
        }
        public class CloseFormEventArgs
        {
            public CloseFormEventArgs() { }
        }
    } //MIDGridFieldEditor
}
