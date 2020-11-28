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
    public sealed class filterProductFieldTypes
    {
        public static List<filterProductFieldTypes> fieldTypeList = new List<filterProductFieldTypes>();
        public static readonly filterProductFieldTypes Hierarchy = new filterProductFieldTypes(0, "Hierarchy", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes NodeID = new filterProductFieldTypes(1, "ID", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes NodeName = new filterProductFieldTypes(2, "Name", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes NodeDescrip = new filterProductFieldTypes(3, "Description", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes Level = new filterProductFieldTypes(4, "Level", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes Active = new filterProductFieldTypes(5, "Active", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes Characteristic = new filterProductFieldTypes(6, "Characteristic", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterProductFieldTypes CharacteristicValue = new filterProductFieldTypes(7, "Characteristic Value", new filterDataTypes(filterValueTypes.Text));

        private filterProductFieldTypes(int dbIndex, string name, filterDataTypes dataType)
        {
            //if (textCode == -1) //use Merchandise Style Level Name
            //{
            //    this.Name = FilterCommon.MerchandiseStyleLevelName;
            //}
            //else if (textCode == -2) //use Merchandise Parent of Style Level Name
            //{
            //    this.Name = FilterCommon.MerchandiseParentofStyleLevelName;
            //}
            //else
            //{
            //    string n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
            //    this.Name = n;
            //}
            this.Name = name;
            this.dbIndex = dbIndex;
            this.dataType = dataType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public filterDataTypes dataType { get; private set; }
        public static implicit operator int(filterProductFieldTypes op) { return op.dbIndex; }


        public static filterProductFieldTypes FromIndex(int dbIndex)
        {
            filterProductFieldTypes result = fieldTypeList.Find(
               delegate(filterProductFieldTypes ft)
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
        public static filterProductFieldTypes FromString(string storeFieldTypeName)
        {
            filterProductFieldTypes result = fieldTypeList.Find(
              delegate(filterProductFieldTypes ft)
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

            foreach (filterProductFieldTypes fieldType in fieldTypeList)
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
            filterProductFieldTypes field = filterProductFieldTypes.FromIndex(fieldIndex);
            return field.dataType;
        }
        public static string GetNameFromIndex(int fieldIndex)
        {
            filterProductFieldTypes field = filterProductFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }
        public static filterDataTypes GetValueTypeInfoForField(int fieldIndex)
        {
            filterProductFieldTypes field = filterProductFieldTypes.FromIndex(fieldIndex);
            return field.dataType;
        }
      

    }


}
