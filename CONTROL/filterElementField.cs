using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementField : UserControl, IFilterElement
    {
        public filterElementField()
        {
            InitializeComponent();
        }
        private elementBase eb;
        private filterEntrySettings groupSettings;

        //delegates to access the container
        public FilterMakeElementInGroupDelegate makeElementInGroupDelegate;
        public FilterRemoveDynamicElementsForFieldDelegate removeDynamicElementsForFieldDelegate;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.groupSettings = groupSettings;
            BindComboBox();
            if (eb.manager.readOnly)
            {
                cboField.Enabled = false;
            }
            if (eb.groupHeading != string.Empty)
            {
                this.ultraLabel1.Text = eb.groupHeading;
            }
        }
        public void ClearControls()
        {
            //clear groups created by child elements first
            foreach (elementBase b in fieldElementList)
            {
                b.elementInterface.ClearControls();
            }
            //clear newly adding groups from the list and from the container
            if (this.removeDynamicElementsForFieldDelegate != null)
            {
                this.removeDynamicElementsForFieldDelegate(keyListToRemove);
            }
        }
        private DataTable dtCbo;
        public void BindComboBox()
        {
            dtCbo = groupSettings.loadFieldList().Copy();
            this.cboField.DataSource = dtCbo;
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboField.Value = condition.fieldIndex;
            if (cboField.Value == null && dtCbo.Rows.Count > 0)
            {
                DataRow drZero = dtCbo.Rows[0];
                cboField.Value = drZero[groupSettings.fieldForData];
            }

            foreach (elementBase b in fieldElementList)
            {
                b.LoadFromCondition(condition);
            }
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            // Begin TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
            foreach (elementBase b in fieldElementList)
            {
                if (!b.elementInterface.IsValid(f, condition))
                {
                    return false;
                }
            }
            // End TT#1407-MD
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            condition.fieldIndex = (int)cboField.Value;

            filterDataTypes dataType = groupSettings.GetDataTypeFromFieldIndex(condition.fieldIndex);
            condition.valueTypeIndex = dataType.valueType.dbIndex;
            // condition.dateTypeIndex = dataType.dateType.Index;
            // condition.numericTypeIndex = dataType.numericType.Index;

            foreach (elementBase b in fieldElementList)
            {
                b.SaveToCondition(ref condition);
            }
        }

        private void cboField_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();

            if (cboField.Value != null)
            {
                int fieldIndex = (int)cboField.Value;
                Handle_DataTypeChanged(groupSettings.GetDataTypeFromFieldIndex(fieldIndex), fieldIndex);
            }
        }

        private filterDataTypes currentDataType = null;
        private List<elementBase> fieldElementList = new List<elementBase>();
        private List<string> keyListToRemove = new List<string>();
        private void Handle_DataTypeChanged(filterDataTypes newDataType, int fieldIndex)
        {
            if (currentDataType == null || newDataType != currentDataType)
            {
                MakeGroupsForDataType(newDataType, fieldIndex);
                currentDataType = newDataType;
            }
        }

        private void MakeGroupsForDataType(filterDataTypes dataType, int fieldIndex)
        {
            ClearControls();

            fieldElementList.Clear();
            keyListToRemove.Clear();
            int index = 0;
            //add new groups based on type
            if (dataType.valueType == filterValueTypes.Text)
            {
                //add text operator
                elementBase bOperator = new elementBase(eb.manager, filterElementMap.OperatorString, "String Operator");
                bOperator.dataType = dataType;
                fieldElementList.Add(bOperator);

                string keyOperator = "nbso" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(keyOperator, bOperator, false, -1);
                    keyListToRemove.Add(keyOperator);
                }
                index++;


                //add text value element
                elementBase bStringValue = new elementBase(eb.manager, filterElementMap.ValueToCompareString, "String Value");
                bStringValue.dataType = dataType;
                fieldElementList.Add(bStringValue);

                string keyStringValue = "nbsv" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(keyStringValue, bStringValue, false, -1);
                    keyListToRemove.Add(keyStringValue);
                }
          
                index++;
            }
            else if (dataType.valueType == filterValueTypes.Numeric || dataType.valueType == filterValueTypes.Dollar)
            {
                //add numeric operator
                elementBase b = new elementBase(eb.manager, filterElementMap.OperatorNumeric, "Numeric Operator");
                b.isOperatorNumeric = true;
                b.dataType = dataType;
                fieldElementList.Add(b);

                string key = "nbno" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                    keyListToRemove.Add(key);
                }
            
                index++;
            }
            else if (dataType.valueType == filterValueTypes.Boolean)
            {
                //add boolean element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareBool, "Boolean Value");
                b.dataType = dataType;
                fieldElementList.Add(b);

                string key = "nbbv" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                    keyListToRemove.Add(key);
                }
            
                index++;
            }
            else if (dataType.valueType == filterValueTypes.Date)
            {
                //add date operator
                elementBase b = new elementBase(eb.manager, filterElementMap.OperatorDate, "Operator:");
                b.isOperatorDate = true;
                b.dataType = dataType;

                fieldElementList.Add(b);

                string key = "nbdo" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                    keyListToRemove.Add(key);
                }     
                index++;
            }
			// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            else if (dataType.valueType == filterValueTypes.Calendar)
            {
                //add calendar date operator
                elementBase b = new elementBase(eb.manager, filterElementMap.OperatorCalendarDate, "Operator:");
                b.isOperatorCalendarDate = true;
                b.dataType = dataType;

                fieldElementList.Add(b);

                string key = "nbco" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                    keyListToRemove.Add(key);
                }
                index++;
            }
			// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            else if (dataType.valueType == filterValueTypes.List)
            {
                //add list operator
                elementBase bOperator = new elementBase(eb.manager, filterElementMap.OperatorIn, "In / Not In");
                bOperator.dataType = dataType;
                fieldElementList.Add(bOperator);

                string keyOperator = "nblo" + index.ToString(); //nb=>new base element
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(keyOperator, bOperator, false, -1);
                    keyListToRemove.Add(keyOperator);
                }         
                index++;

                //add list element
                elementBase bList = new elementBase(eb.manager, filterElementMap.List, groupSettings.GetNameFromField(fieldIndex));
                bList.isList = true;
                bList.dataType = dataType;

                fieldElementList.Add(bList);

                string key = "listnbl" + index.ToString(); //nb=>new base element // TT#1381-MD -jsobek -Excessive white space in Filter Details section
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, bList, true, fieldIndex);
                    keyListToRemove.Add(key);
                }       
                index++;
            }



            //foreach (elementBase b in fieldElementList)
            //{
            //    string key = "nb" + index.ToString(); //nb=>new base element
            //    makeElementInGroupDelegate(key, b);
            //    keyListToRemove.Add(key);
            //    index++;
            //}
        }

        private void cboField_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboField, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }

        private void cboField_BeforeDropDown(object sender, CancelEventArgs e)
        {
            //AdjustTextWidthComboBox_DropDown(this.cboField);
        }

        private void cboField_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                TreeNodeClipboardList cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                if (eb.dragDropTypesAllowed.Any(x => x == cbList.ClipboardDataType))    
                {
                    //if (!_allowUserAttributes &&
                    //        cbList.ClipboardProfile.OwnerUserRID != Include.GlobalUserRID)
                    //{
                    //    e.Effect = DragDropEffects.None;
                    //}
                    //else if (FunctionSecurity.AllowUpdate ||
                    //FunctionSecurity.AllowView)
                    //{
                    cboField.Value = cbList.ClipboardProfile.Key;
                    //Invalidate();
                    //}
                }
            }
        }

        private void cboField_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList;

            try
            {
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (eb.dragDropTypesAllowed.Any(x => x == cbList.ClipboardDataType))
                    {
                        //if (!_allowUserAttributes &&
                        //    cbList.ClipboardProfile.OwnerUserRID != Include.GlobalUserRID)
                        //{
                        //    e.Effect = DragDropEffects.None;
                        //}
                        //else if (FunctionSecurity.AllowUpdate ||
                        //    FunctionSecurity.AllowView)
                        //{
                            e.Effect = DragDropEffects.All;
                        //}
                        //else
                        //{
                        //    e.Effect = DragDropEffects.None;
                        //}
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void cboField_DragLeave(object sender, EventArgs e)
        {

        }

        private void cboField_DragOver(object sender, DragEventArgs e)
        {

        }

       
    }
}
