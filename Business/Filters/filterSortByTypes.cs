using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public sealed class filterSortByTypes
    {
        public static List<filterSortByTypes> sortByTypeList = new List<filterSortByTypes>();
        public static readonly filterSortByTypes StoreCharacteristics = new filterSortByTypes("Store Characteristics", 0, new List<filterTypes>() { filterTypes.StoreFilter, filterTypes.StoreGroupFilter });
        public static readonly filterSortByTypes HeaderCharacteristics = new filterSortByTypes("Header Characteristics", 1, new List<filterTypes>() { filterTypes.HeaderFilter, filterTypes.AssortmentFilter }); //TT#1468-MD -jsobek -Header Filter Sort Options
        public static readonly filterSortByTypes StoreFields = new filterSortByTypes("Store Fields", 2, new List<filterTypes>() { filterTypes.StoreFilter, filterTypes.StoreGroupFilter });
        public static readonly filterSortByTypes HeaderFields = new filterSortByTypes("Header Fields", 3, new List<filterTypes>() { filterTypes.HeaderFilter, filterTypes.AssortmentFilter }); //TT#1468-MD -jsobek -Header Filter Sort Options
        public static readonly filterSortByTypes Variables = new filterSortByTypes("Variables", 4, new List<filterTypes>() { filterTypes.StoreFilter });
        //public static readonly filterSortByTypes HeaderType = new filterSortByTypes("Header Type", 5, new List<filterTypes>() { filterTypes.HeaderFilter, filterTypes.AssortmentFilter });
        public static readonly filterSortByTypes StoreStatus = new filterSortByTypes("Store Status", 6, new List<filterTypes>() { filterTypes.StoreFilter, filterTypes.StoreGroupFilter });
        public static readonly filterSortByTypes HeaderStatus = new filterSortByTypes("Header Status", 7, new List<filterTypes>() { filterTypes.HeaderFilter, filterTypes.AssortmentFilter }); //TT#1468-MD -jsobek -Header Filter Sort Options
        public static readonly filterSortByTypes HeaderDate = new filterSortByTypes("Header Date", 8, new List<filterTypes>() { filterTypes.HeaderFilter, filterTypes.AssortmentFilter }); //TT#1468-MD -jsobek -Header Filter Sort Options
        //public static readonly filterSortByTypes HeaderReleaseDate = new filterSortByTypes("Release Date", 9, new List<filterTypes>() { filterTypes.HeaderFilter, filterTypes.AssortmentFilter });
        public static readonly filterSortByTypes ProductSearchFields = new filterSortByTypes("Product Search Fields", 10, new List<filterTypes>() { filterTypes.ProductFilter }); //TT#1388-MD -jsobek -Product Filters
        //public static readonly filterSortByTypes ProductCharacteristics = new filterSortByTypes("Product Characteristics", 11, new List<filterTypes>() { filterTypes.ProductFilter }); //TT#1388-MD -jsobek -Product Filters
        public static readonly filterSortByTypes AuditSearchFields = new filterSortByTypes("Audit Search Fields", 12, new List<filterTypes>() { filterTypes.AuditFilter }); 
      
        private filterSortByTypes(string Name, int dbIndex, List<filterTypes> filterTypeList)
        {
            this.Name = Name;
            this.Index = dbIndex;
            this.filterTypeList = filterTypeList;
            sortByTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }
        public List<filterTypes> filterTypeList { get; private set; }
        public static implicit operator int(filterSortByTypes op) { return op.Index; }


        public static filterSortByTypes FromIndex(int dbIndex)
        {
            filterSortByTypes result = sortByTypeList.Find(
               delegate(filterSortByTypes ft)
               {
                   return ft.Index == dbIndex;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                //filter type was not found in the list
                return null;
            }
        }
        public static filterSortByTypes FromString(string filterTypeName)
        {
            filterSortByTypes result = sortByTypeList.Find(
              delegate(filterSortByTypes ft)
              {
                  return ft.Name == filterTypeName;
              }
              );
            if (result != null)
            {
                return result;
            }
            else
            {
                //filter type was not found in the list
                return null;
            }
        }
        public static DataTable ToDataTable(filterTypes filterType)
        {
            DataTable dt = new DataTable("fields");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));

            foreach (filterSortByTypes fieldType in sortByTypeList)
            {
                if (fieldType.filterTypeList.Contains(filterType))
                {
                    DataRow dr = dt.NewRow();
                    dr["FIELD_INDEX"] = fieldType.Index;
                    dr["FIELD_NAME"] = fieldType.Name;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }


    }
}
