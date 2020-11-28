using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public sealed class filterAssortmentFieldTypes
    {
        public static List<filterAssortmentFieldTypes> fieldTypeList = new List<filterAssortmentFieldTypes>();
        public static readonly filterAssortmentFieldTypes AssortmentID = new filterAssortmentFieldTypes(0, (int)eMIDTextCode.lbl_AssortmentID, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAssortmentFieldTypes Delivery = new filterAssortmentFieldTypes(1, (int)eMIDTextCode.lbl_Delivery, new filterDataTypes(filterValueTypes.Calendar, filterDateTypes.DateOnly));
        public static readonly filterAssortmentFieldTypes Description = new filterAssortmentFieldTypes(2, (int)eMIDTextCode.lbl_Description, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAssortmentFieldTypes Quantity = new filterAssortmentFieldTypes(3, (int)eMIDTextCode.lbl_Quantity, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterAssortmentFieldTypes AllocatedUnits = new filterAssortmentFieldTypes(4, (int)eMIDTextCode.lbl_AllocatedUnits, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterAssortmentFieldTypes OriginalAllocatedUnits = new filterAssortmentFieldTypes(5, (int)eMIDTextCode.lbl_OrigAllocatedUnits, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterAssortmentFieldTypes ReserveAllocatedUnits = new filterAssortmentFieldTypes(6, (int)eMIDTextCode.lbl_RsvAllocatedUnits, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));




        private filterAssortmentFieldTypes(int dbIndex, int textCode, filterDataTypes dataType)
        {
            if (textCode == -1) //use Merchandise Style Level Name
            {
                this.Name = FilterCommon.MerchandiseStyleLevelName;
            }
            else if (textCode == -2) //use Merchandise Parent of Style Level Name
            {
                this.Name = FilterCommon.MerchandiseParentofStyleLevelName;
            }
            else
            {
                string n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
                this.Name = n;
            }
            this.dbIndex = dbIndex;
            this.dataType = dataType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public filterDataTypes dataType { get; private set; }
        public static implicit operator int(filterAssortmentFieldTypes op) { return op.dbIndex; }


        public static filterAssortmentFieldTypes FromIndex(int dbIndex)
        {
            filterAssortmentFieldTypes result = fieldTypeList.Find(
               delegate(filterAssortmentFieldTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                //field type was not found in the list
                return null;
            }
        }
        public static filterAssortmentFieldTypes FromString(string storeFieldTypeName)
        {
            filterAssortmentFieldTypes result = fieldTypeList.Find(
              delegate(filterAssortmentFieldTypes ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );
            if (result != null)
            {
                return result;
            }
            else
            {
                //field type was not found in the list
                return null;
            }
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("fields");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));
            dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            foreach (filterAssortmentFieldTypes fieldType in fieldTypeList.OrderBy(x => x.Name))
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.dataType.valueType.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static filterDataTypes GetValueTypeForField(int fieldIndex)
        {
            filterAssortmentFieldTypes field = filterAssortmentFieldTypes.FromIndex(fieldIndex);
            return field.dataType;
        }
        public static string GetNameFromIndex(int fieldIndex)
        {
            filterAssortmentFieldTypes field = filterAssortmentFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }

        public static DataTable GetValueListForAssortmentFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            try
            {
                filterAssortmentFieldTypes assortmentFieldType = filterAssortmentFieldTypes.FromIndex(fieldIndex);
                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return dt;
        }

    }
}
