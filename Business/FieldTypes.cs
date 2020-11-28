using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public static class FieldTypeUtility
    {
        public static DataTable GetDataTableForListDropDowns()
        {
            DataTable dtListValues = new DataTable("ListValues");
            dtListValues.Columns.Add("LIST_VALUE_INDEX", typeof(int)); //data
            dtListValues.Columns.Add("LIST_VALUE"); //display
            return dtListValues;
        }

        public static DataTable GetDataTableForFields()
        {
            DataTable dtFields = new DataTable("Fields");
            dtFields.Columns.Add("OBJECT_TYPE", typeof(int));
            dtFields.Columns.Add("FIELD_INDEX", typeof(int));
            dtFields.Columns.Add("FIELD_TYPE", typeof(int));
            dtFields.Columns.Add("ALLOW_EDIT", typeof(bool));
            //dtFields.Columns.Add("HAS_CHANGED", typeof(bool));
            dtFields.Columns.Add("MAX_LENGTH", typeof(int));
            dtFields.Columns.Add("FIELD_NAME"); //visible on grid
            dtFields.Columns.Add("FIELD_VALUE", typeof(object)); //visible grid
            //dtFields.Columns.Add("ORIG_VALUE", typeof(object));
            dtFields.Columns.Add("IS_DIRTY", typeof(bool));
            return dtFields;
        }
        public static DataTable GetDataTableForFieldsByCol()
        {
            DataTable dtFieldsByCol = new DataTable("FieldsByCol");
            dtFieldsByCol.Columns.Add("OBJECT_RID", typeof(int));
            dtFieldsByCol.Columns.Add("OBJECT_NAME"); //visible grid
            //The remaining columns are created dynamically.

            return dtFieldsByCol;
        }

        public static void ShowValidationMessage(List<MIDMsg> msgList, string title)
        {
            //EditMsgs.Message msg = (EditMsgs.Message)msgs.EditMessages[0];
            MIDMsg mMsg = msgList[0];
            System.Windows.Forms.MessageBox.Show("[" + ((int)mMsg.textCode).ToString() + "] " + mMsg.msg, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
      
    }
        public class fieldTypeBase
        {
            public string Name { get; set; }
            public int fieldIndex { get; set; }
            public string dbFieldName { get; set; }
            public bool allowEdit { get; set; }
            public int maxLength { get; set; }
            public fieldDataTypes dataType { get; set; }

        }

        public class fieldColumnMap
        {
            public int objectType;
            public int fieldIndex;
            public int columnIndex;
            public fieldDataTypes fieldDataType;
            public fieldDataTypes fieldActualDataType = fieldDataTypes.Text;  // TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
            public bool isVisible = true;
            public string columnName;  //Used to hide the column
            public bool isDirty = false;
            public fieldColumnMap(int objectType, int fieldIndex, int columnIndex)
            {
                this.objectType = objectType;
                this.fieldIndex = fieldIndex;
                this.columnIndex = columnIndex;
            }
        }
        public sealed class validationKinds
        {
            public static List<validationKinds> timeList = new List<validationKinds>();
            public static readonly validationKinds BeforeExitEditMode = new validationKinds(0);
            public static readonly validationKinds UponSaving = new validationKinds(1);

            private validationKinds(int Index)
            {
                this.Index = Index;
                timeList.Add(this);
            }

            public int Index { get; private set; }
            public static implicit operator int(validationKinds op) { return op.Index; }

            public static validationKinds FromIndex(int Index)
            {
                return timeList.Find(x => x.Index == Index);
            }
        }

        public enum storeCharInfoAction
        {
            UseValue,
            InsertValue,
            Skip
        }
        public class storeCharInfo
        {
            public int scgRID; //store char group rid
            public int scRID; //store char rid (for the value)
            public int stRID; //store rid

            public storeCharInfoAction action;
            public bool isDirty;

            public fieldDataTypes dataType;
            public string anyValue;
            public string textValue;
            public DateTime? dateValue;
            public float? numberValue;
            public float? dollarValue;
        }

        public delegate DataTable GetListValuesForField(int objectType, int fieldIndex, ref string dataField, ref string displayField);
        public delegate bool IsFieldValueValid(validationKinds validationKind, int objectType, int fieldIndex, int objectRID, object origValue, object newValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent);
        public delegate bool IsObjectValid(int objectRID, ref DataSet dsSingleObjectFields, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent);
        public delegate bool IsObjectValidForFields(ref DataSet dsFields, List<MIDMsg> msgList, List<fieldColumnMap> columnMapList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent);
        public delegate object FieldValueGetForCurrentField(int objectRID, int objectType, int fieldIndex);
        public delegate void FieldValueSetForCurrentField(int objectRID, int objectType, int fieldIndex, object val);
        public sealed class fieldDataTypes
        {
            public static List<fieldDataTypes> dataTypeList = new List<fieldDataTypes>();
            public static readonly fieldDataTypes Text = new fieldDataTypes(0);
            public static readonly fieldDataTypes List = new fieldDataTypes(1);
            public static readonly fieldDataTypes Boolean = new fieldDataTypes(2);
            public static readonly fieldDataTypes DateWithTime = new fieldDataTypes(3);
            public static readonly fieldDataTypes DateNoTime = new fieldDataTypes(4);
            public static readonly fieldDataTypes DateOnlyTime = new fieldDataTypes(5);
            public static readonly fieldDataTypes NumericDollar = new fieldDataTypes(6);
            public static readonly fieldDataTypes NumericDouble = new fieldDataTypes(7);
            public static readonly fieldDataTypes NumericInteger = new fieldDataTypes(8);

            private fieldDataTypes(int Index)
            {
                this.Index = Index;
                dataTypeList.Add(this);
            }

            public int Index { get; private set; }
            public static implicit operator int(fieldDataTypes op) { return op.Index; }

            public static fieldDataTypes FromIndex(int Index)
            {
                return dataTypeList.Find(x => x.Index == Index);
            }

            public static fieldDataTypes FromChar(int charGroupType, string charGroupListInd)
            {
                fieldDataTypes dataType;
                if (charGroupListInd == "1")
                {
                    dataType = fieldDataTypes.List;
                }
                else
                {
                    if (charGroupType == 1)
                    {
                        dataType = fieldDataTypes.DateNoTime;
                    }
                    else if (charGroupType == 2)
                    {
                        dataType = fieldDataTypes.NumericDouble;
                    }
                    else if (charGroupType == 3)
                    {
                        dataType = fieldDataTypes.NumericDollar;
                    }
                    else
                    {
                        dataType = fieldDataTypes.Text;
                    }
                }
                return dataType;
            }
            public static fieldDataTypes FromCharIgnoreLists(int charGroupType)
            {
                fieldDataTypes dataType;
              
                if (charGroupType == 1)
                {
                    dataType = fieldDataTypes.DateNoTime;
                }
                else if (charGroupType == 2)
                {
                    dataType = fieldDataTypes.NumericDouble;
                }
                else if (charGroupType == 3)
                {
                    dataType = fieldDataTypes.NumericDollar;
                }
                else
                {
                    dataType = fieldDataTypes.Text;
                }

                return dataType;
            }

            public static Type GetTypeForField(int Index)
            {
                Type fieldType;
                fieldDataTypes dataType = dataTypeList.Find(x => x.Index == Index);
                if (dataType == fieldDataTypes.Boolean)
                {
                    fieldType = typeof(bool);
                }
                else if (dataType == fieldDataTypes.DateNoTime || dataType == fieldDataTypes.DateOnlyTime || dataType == fieldDataTypes.DateWithTime)
                {
                    fieldType = typeof(DateTime);
                }
                else if (dataType == fieldDataTypes.NumericDollar || dataType == fieldDataTypes.NumericDouble)
                {
                    fieldType = typeof(double);
                }
                else if (dataType == fieldDataTypes.NumericInteger)
                {
                    fieldType = typeof(int);
                }
                else
                {
                    fieldType = typeof(string);
                }
                return fieldType;
            }
            public static void AssignValueToDataRow(ref DataRow dr, string fieldName, int fieldDataTypeIndex, object val)
            {
                fieldDataTypes dataType = dataTypeList.Find(x => x.Index == fieldDataTypeIndex);
                if (val == DBNull.Value)
                {
                    dr[fieldName] = DBNull.Value;
                }
                else if (dataType == fieldDataTypes.Boolean)
                {
                    // Begin TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
                    //dr[fieldName] = Include.ConvertCharToBool(Convert.ToChar(val));
                    if (val.ToString().ToUpper() == "FALSE"
                        || val.ToString().ToUpper() == "TRUE")
                    {
                        dr[fieldName] = Convert.ToBoolean(val);
                    }
                    else
                    {
                        dr[fieldName] = Include.ConvertCharToBool(Convert.ToChar(val));
                    }
                    // Begin TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
                }
                else if (dataType == fieldDataTypes.DateNoTime || dataType == fieldDataTypes.DateOnlyTime || dataType == fieldDataTypes.DateWithTime)
                {
                    dr[fieldName] = Convert.ToDateTime(val);
                }
                else if (dataType == fieldDataTypes.NumericDollar || dataType == fieldDataTypes.NumericDouble)
                {
                    dr[fieldName] = Convert.ToDouble(val);
                }
                else if (dataType == fieldDataTypes.NumericInteger)
                {
                    dr[fieldName] = Convert.ToInt32(val);
                }
                else
                {
                    dr[fieldName] = Convert.ToString(val);
                }
            }
        }
}
