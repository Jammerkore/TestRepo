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

namespace MIDRetail.Windows.Controls
{
    public partial class CharacteristicMaintControl : UserControl
    {
        public CharacteristicMaintControl()
        {
            InitializeComponent();
        }

        private int currentCharGroupRID = -1;
        private string currentCharGroupID = string.Empty;
        private fieldDataTypes currentFieldDataType = null;
        public string groupRidField;
        public string groupIDField;
        public string groupTypeField;
        public string groupIsListField;
        public string valueRidField;
        public string valueField;
        public eProfileType charGroupProfileType;
        public eProfileType valueProfileType;
        private bool allowUpdate;
        public void DoInitialize(string formText, bool allowUpdate)
        {
            try
            {
                this.allowUpdate = allowUpdate;
                this.gridChars.SelectedRowChangedEvent += new MIDGridControl.SelectedRowChangedEventHandler(Handle_CharGroupRowChanged);
                this.gridChars.exportObjectName = "Characteristics";
                this.gridChars.HideLayoutOnMenu();
                this.gridChars.HideColumn(groupRidField);
                this.gridChars.HideColumn(groupTypeField);
                this.gridChars.doIncludeHeaderWhenResizing = true;
                this.gridChars.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;



                this.gridValues.HideButtons();
                this.gridValues.HideColumn("FIELD_NAME");
                this.gridValues.Initialize(false, formText);
                this.gridValues.formatCellValueForField = new MIDGridFieldEditor.FormatCellValueForField(this.FormatCellValueForField);



                if (allowUpdate)
                {
                    this.utmCharGroup.Tools["btnGroupAdd"].SharedProps.Enabled = true;
                    this.utmCharGroup.Tools["btnGroupEdit"].SharedProps.Enabled = true;
                    this.utmCharGroup.Tools["btnGroupDelete"].SharedProps.Enabled = true;

                    this.utmValue.Tools["btnValueAdd"].SharedProps.Enabled = true;
                    this.utmValue.Tools["btnValueEdit"].SharedProps.Enabled = true;
                    this.utmValue.Tools["btnValueDelete"].SharedProps.Enabled = true;
                }
                else
                {
                    this.ParentForm.Text += " [Read Only]";
                    // Begin TT#1849-MD - JSmith - Security - Store Characteristic set to View - Able to create a Store characteristic
                    this.utmCharGroup.Tools["btnGroupAdd"].SharedProps.Enabled = false;
                    this.utmCharGroup.Tools["btnGroupEdit"].SharedProps.Enabled = false;
                    this.utmCharGroup.Tools["btnGroupDelete"].SharedProps.Enabled = false;

                    this.utmValue.Tools["btnValueAdd"].SharedProps.Enabled = false;
                    this.utmValue.Tools["btnValueEdit"].SharedProps.Enabled = false;
                    this.utmValue.Tools["btnValueDelete"].SharedProps.Enabled = false;
                    // End TT#1849-MD - JSmith - Security - Store Characteristic set to View - Able to create a Store characteristic
                }


            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public void RefreshCharGroups(DataTable dtCharGroups)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dtCharGroups);
            this.gridChars.BindGrid(ds);
        }
 
        public void ShowCharGroup(int charGroupRID)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in gridChars.ultraGrid1.Rows)
            {
                LocateCharGroup(row, charGroupRID);
            }
        }
        private void LocateCharGroup(Infragistics.Win.UltraWinGrid.UltraGridRow row, int charGroupRID)
        {
            if (row.HasChild(null))
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand cb in row.ChildBands)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow childRow in cb.Rows)
                    {
                        LocateCharGroup(childRow, charGroupRID);
                    }
                }
            }
            else
            {
                if (Convert.ToInt32(row.Cells[this.groupRidField].Value) == charGroupRID)
                {

                    gridChars.ultraGrid1.Selected.Rows.Clear();
                    gridChars.ultraGrid1.ActiveRow = row;
                    row.Selected = true;
                    row.Expanded = true;
                }
            }
        }

        public delegate DataTable GetValuesForCharGroupDelegate(int charGroupRid);
        public GetValuesForCharGroupDelegate GetValuesForCharGroup;
        //public GetListValuesForField GetListValuesForTheValues;
       
        private void Handle_CharGroupRowChanged(object sender, MIDGridControl.SelectedRowChangedEventArgs e)
        {
            try
            {
                currentCharGroupRID = Convert.ToInt32(e.drSelected[groupRidField]);
                currentCharGroupID = e.drSelected[groupIDField].ToString();
                int charGroupType = Convert.ToInt32(e.drSelected[groupTypeField]);
                bool isGroupList = Convert.ToBoolean(e.drSelected[groupIsListField]);
                this.currentFieldDataType = fieldDataTypes.FromCharIgnoreLists(charGroupType);
                RefreshValues();

                if (allowUpdate && isGroupList)
                {
                    this.utmValue.Tools["btnValueAdd"].SharedProps.Enabled = true;
                    this.utmValue.Tools["btnValueEdit"].SharedProps.Enabled = true;
                    this.utmValue.Tools["btnValueDelete"].SharedProps.Enabled = true;
                }
                else
                {
                    this.utmValue.Tools["btnValueAdd"].SharedProps.Enabled = false;
                    this.utmValue.Tools["btnValueEdit"].SharedProps.Enabled = false;
                    this.utmValue.Tools["btnValueDelete"].SharedProps.Enabled = false;
                }

                if (isGroupList)
                {
                    this.utmValue.Tools["btnValueInUse"].SharedProps.Enabled = true;
                }
                else
                {
                    this.utmValue.Tools["btnValueInUse"].SharedProps.Enabled = false;
                }
                

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        public void RefreshValues()
        {
            try
            {
                DataTable dt = GetValuesForCharGroup(currentCharGroupRID);
                this.gridValues.BindGrid(currentCharGroupRID, dt, null);
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


        private void utmCharGroups_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (e.Tool.Key == "btnGroupAdd")
            {
                CharAdd();
            }
            else if (e.Tool.Key == "btnGroupEdit")
            {
                CharEdit();
            }
            else if (e.Tool.Key == "btnGroupDelete")
            {
                CharDelete();
            }
            else if (e.Tool.Key == "btnGroupInUse")
            {
                CharInUse();
            }
        }


        private void utmValues_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (e.Tool.Key == "btnValueAdd")
            {
                ValueAdd();
            }
            else if (e.Tool.Key == "btnValueEdit")
            {
                ValueEdit();
            }
            else if (e.Tool.Key == "btnValueDelete")
            {
                ValueDelete();
            }
            else if (e.Tool.Key == "btnValueInUse")
            {
                ValueInUse();
            }
        }




        private void CharAdd()
        {
            RaiseCharAddEvent();
        }
        private void CharEdit()
        {
            if (this.currentCharGroupRID == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a characteristic to edit.");
            }
            else
            {
                RaiseCharEditEvent(this.gridChars.GetFirstSelectedRow());
            }
        }
        private void CharDelete()
        {
            if (this.currentCharGroupRID == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a characteristic to remove.");
            }
            else
            {
                string msg = ((int)eMIDTextCode.msg_ConfirmRemove).ToString() + ": " + String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmRemove), "this characteristic.");
                if (MessageBox.Show(msg, this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
                RaiseCharDeleteEvent(this.currentCharGroupRID, this.currentCharGroupID);
            }
        }
        private void CharInUse()
        {
            List<DataRow> drSelected = this.gridChars.GetSelectedRows();
            if (drSelected.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a characteristic for In Use.");
            }
            else
            {
                RaiseCharInUseEvent(drSelected);
            }
        }

        private void ValueAdd()
        {
            if (this.currentCharGroupRID == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a characteristic to add a new value to.");
            }
            else
            {
                RaiseValueAddEvent(this.currentCharGroupRID, this.currentFieldDataType, currentCharGroupID);
            }
        }
        private void ValueEdit()
        {
            DataRow drSelected = this.gridValues.GetFirstSelectedRow();
            if (drSelected == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a value to edit.");
            }
            else
            {
                int valueRID = (int)drSelected[valueRidField];
                object val = drSelected["FIELD_VALUE"];
                RaiseValueEditEvent(valueRID, this.currentCharGroupRID, this.currentFieldDataType, val, currentCharGroupID);
            }
        }
        private void ValueDelete()
        {
            DataRow drSelected = this.gridValues.GetFirstSelectedRow();
            if (drSelected == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a value to remove.");
            }
            else
            {
                string msg = ((int)eMIDTextCode.msg_ConfirmRemove).ToString() + ": " + String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmRemove), "this value.");
                if (MessageBox.Show(msg, this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
                int valueRID = (int)drSelected[valueRidField];
                object valueAsObject = drSelected[valueField];
                RaiseValueDeleteEvent(valueRID, this.currentCharGroupRID, currentCharGroupID, valueAsObject.ToString());
            }
        }
        private void ValueInUse()
        {
            List<DataRow> drSelected = this.gridValues.GetSelectedRows();
            if (drSelected.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a value for In Use.");
            }
            else
            {
                RaiseValueInUseEvent(drSelected);
            }
        }

        #region "Events"
        public delegate void CharAddEventHandler(object sender, CharAddEventArgs e);
        public event CharAddEventHandler CharAddEvent;
        public void RaiseCharAddEvent()
        {
            if (CharAddEvent != null)
                CharAddEvent(new object(), new CharAddEventArgs());
        }
        public class CharAddEventArgs
        {
            public CharAddEventArgs() { }
        }

        public delegate void CharEditEventHandler(object sender, CharEditEventArgs e);
        public event CharEditEventHandler CharEditEvent;
        public void RaiseCharEditEvent(DataRow drCharGroup)
        {
            if (CharEditEvent != null)
                CharEditEvent(new object(), new CharEditEventArgs(drCharGroup));
        }
        public class CharEditEventArgs
        {
            public CharEditEventArgs(DataRow drCharGroup) { this.drCharGroup = drCharGroup; }
            public DataRow drCharGroup { get; private set; }
        }


        public delegate void CharDeleteEventHandler(object sender, CharDeleteEventArgs e);
        public event CharDeleteEventHandler CharDeleteEvent;
        public void RaiseCharDeleteEvent(int charGroupRid, string charGroupID)
        {
            if (CharDeleteEvent != null)
                CharDeleteEvent(new object(), new CharDeleteEventArgs(charGroupRid, charGroupID));
        }
        public class CharDeleteEventArgs
        {
            public CharDeleteEventArgs(int charGroupRid, string charGroupID) { this.charGroupRid = charGroupRid; this.charGroupID = charGroupID; }
            public int charGroupRid { get; private set; }

            public string charGroupID { get; private set; }
        }


        public delegate void CharInUseEventHandler(object sender, CharInUseEventArgs e);
        public event CharInUseEventHandler CharInUseEvent;
        public void RaiseCharInUseEvent(List<DataRow> charGroupRows)
        {
            if (CharInUseEvent != null)
                CharInUseEvent(new object(), new CharInUseEventArgs(charGroupRows));
        }
        public class CharInUseEventArgs
        {
            public CharInUseEventArgs(List<DataRow> charGroupRows) { this.charGroupRows = charGroupRows; }

            public List<DataRow> charGroupRows { get; private set; }
        }

        public delegate void ValueAddEventHandler(object sender, ValueAddEventArgs e);
        public event ValueAddEventHandler ValueAddEvent;
        public void RaiseValueAddEvent(int charGroupRid, fieldDataTypes fieldDataType, string charGroupID)
        {
            if (ValueAddEvent != null)
                ValueAddEvent(new object(), new ValueAddEventArgs(charGroupRid, fieldDataType, charGroupID));
        }
        public class ValueAddEventArgs
        {
            public ValueAddEventArgs(int charGroupRid, fieldDataTypes fieldDataType, string charGroupID) { this.charGroupRid = charGroupRid; this.fieldDataType = fieldDataType; this.charGroupID = charGroupID; }
            public int charGroupRid { get; private set; }
            public fieldDataTypes fieldDataType { get; private set; }

            public string charGroupID { get; private set; }
        }


        public delegate void ValueEditEventHandler(object sender, ValueEditEventArgs e);
        public event ValueEditEventHandler ValueEditEvent;
        public void RaiseValueEditEvent(int valueRid, int charGroupRid, fieldDataTypes fieldDataType, object val, string charGroupID)
        {
            if (ValueEditEvent != null)
                ValueEditEvent(new object(), new ValueEditEventArgs(valueRid, charGroupRid, fieldDataType, val, charGroupID));
        }
        public class ValueEditEventArgs
        {
            public ValueEditEventArgs(int valueRid, int charGroupRid, fieldDataTypes fieldDataType, object val, string charGroupID) { this.valueRid = valueRid; this.charGroupRid = charGroupRid; this.fieldDataType = fieldDataType; this.val = val; this.charGroupID = charGroupID;  }
            public int valueRid { get; private set; }
            public int charGroupRid { get; private set; }
            public fieldDataTypes fieldDataType { get; private set; }
            public object val { get; private set; }

            public string charGroupID { get; private set; }
        }


        public delegate void ValueDeleteEventHandler(object sender, ValueDeleteEventArgs e);
        public event ValueDeleteEventHandler ValueDeleteEvent;
        public void RaiseValueDeleteEvent(int valueRid, int charGroupRid, string charGroupID, string valueAsString)
        {
            if (ValueDeleteEvent != null)
                ValueDeleteEvent(new object(), new ValueDeleteEventArgs(valueRid, charGroupRid, charGroupID, valueAsString));
        }
        public class ValueDeleteEventArgs
        {
            public ValueDeleteEventArgs(int valueRid, int charGroupRid, string charGroupID, string valueAsString) { this.valueRid = valueRid; this.charGroupRid = charGroupRid; this.charGroupID = charGroupID; this.valueAsString = valueAsString; }
            public int valueRid { get; private set; }
            public int charGroupRid { get; private set; }

            public string charGroupID { get; private set; }
            public string valueAsString { get; private set; }
        }



        public delegate void ValueInUseEventHandler(object sender, ValueInUseEventArgs e);
        public event ValueInUseEventHandler ValueInUseEvent;
        public void RaiseValueInUseEvent(List<DataRow> valueRows)
        {
            if (ValueInUseEvent != null)
                ValueInUseEvent(new object(), new ValueInUseEventArgs(valueRows));
        }
        public class ValueInUseEventArgs
        {
            public ValueInUseEventArgs(List<DataRow> valueRows) { this.valueRows = valueRows; }

            public List<DataRow> valueRows { get; private set; }
        }
        #endregion
    }
}
