using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public static class CharacteristicMaint
    {
        public static DataTable GetCharGroupDataTable()
        {
            DataTable dtCharGroups = new DataTable();
            dtCharGroups.Columns.Add("CHAR_GROUP_RID", typeof(int));
            dtCharGroups.Columns.Add("CHAR_GROUP_TYPE", typeof(int));
            dtCharGroups.Columns.Add("Characteristics");
            dtCharGroups.Columns.Add("Value Type");
            return dtCharGroups;
        }


    }

    public sealed class charTypes
    {
        public static List<charTypes> typeList = new List<charTypes>();
        public static readonly charTypes Text = new charTypes(0, "Text", eStoreCharType.text, eHeaderCharType.text);
        public static readonly charTypes Date = new charTypes(1, "Date", eStoreCharType.date, eHeaderCharType.date);
        public static readonly charTypes Number = new charTypes(2, "Number", eStoreCharType.number, eHeaderCharType.number);
        public static readonly charTypes Dollar = new charTypes(3, "Dollar", eStoreCharType.dollar, eHeaderCharType.dollar);
        private charTypes(int index, string name, eStoreCharType echarGroupType, eHeaderCharType eHeaderCharGroupType)
        {
            this.Index = index;
            this.Name = name;
            this.echarGroupType = echarGroupType;
            this.eHeaderCharGroupType = eHeaderCharGroupType;
            typeList.Add(this);
        }
        public int Index { get; private set; }
        public string Name { get; private set; }
        public eStoreCharType echarGroupType { get; private set; }
        public eHeaderCharType eHeaderCharGroupType { get; private set; }
        public static implicit operator int(charTypes op) { return op.Index; }
        public static charTypes FromIndex(int index)
        {
            return typeList.Find(x => x.Index == index);
        }
        public static charTypes FromName(string name)
        {
            return typeList.Find(x => x.Name == name);
        }
        public static DataTable GetListValuesForCharGroups(int objectType, int fieldIndex, ref string dataField, ref string displayField)
        {
            DataTable dtListValues = FieldTypeUtility.GetDataTableForListDropDowns();
            dataField = "LIST_VALUE_INDEX";
            displayField = "LIST_VALUE";


            foreach(charTypes storeCharType in typeList)
            {
                DataRow dr = dtListValues.NewRow();
                dr[dataField] = storeCharType.Index;
                dr[displayField] = storeCharType.Name;
                dtListValues.Rows.Add(dr);
            }

            return dtListValues;
        }
    }
    public sealed class charObjectTypes
    {
        public static List<charObjectTypes> objectList = new List<charObjectTypes>();
        public static readonly charObjectTypes Store = new charObjectTypes(0, "Store Characteristics");
        public static readonly charObjectTypes Header = new charObjectTypes(1, "Header Characteristics");
        public static readonly charObjectTypes Product = new charObjectTypes(2, "Product Characteristics");

        private charObjectTypes(int index, string name)
        {
            this.Index = index;
            this.Name = name;
            objectList.Add(this);
        }
        public int Index { get; private set; }
        public string Name { get; private set; }
        public static implicit operator int(charObjectTypes op) { return op.Index; }
        public static charObjectTypes FromIndex(int index)
        {
            return objectList.Find(x => x.Index == index);
        }
    }
    public sealed class charObjectGroupFields
    {
        public static List<charObjectGroupFields> fieldList = new List<charObjectGroupFields>();
        public static readonly charObjectGroupFields GroupName = new charObjectGroupFields(0, "Characteristic");
        public static readonly charObjectGroupFields GroupValueType = new charObjectGroupFields(1, "Value Type");
        public static readonly charObjectGroupFields IsList = new charObjectGroupFields(2, "Is List");
        public static readonly charObjectGroupFields GroupProtectInd = new charObjectGroupFields(3, "Protect");

        private charObjectGroupFields(int index, string name)
        {
            this.Index = index;
            this.Name = name;
            fieldList.Add(this);
        }
        public int Index { get; private set; }
        public string Name { get; private set; }
        public static implicit operator int(charObjectGroupFields op) { return op.Index; }
        public static charObjectGroupFields FromIndex(int index)
        {
            return fieldList.Find(x => x.Index == index);
        }
    }
}
