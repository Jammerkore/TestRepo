using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MIDRetail.DataCommon
{

    public delegate void SetCalendarDateRangeForPlanDelegate(int DateRangeRID);
    public delegate string GetDateRangeTextForPlanDelegate(int DateRangeRID);
    public delegate string GetTextFromHierarchyNodeDelegate(int hnRID);
    public delegate bool IsStoreGroupOrLevelInUse(eProfileType etype, int aRID);
    //public delegate bool IsStoreGroupLevelInUse(ArrayList levelRIDs);
    public sealed class filterTypes
    {
        public static List<filterTypes> filterTypeList = new List<filterTypes>();
        public static readonly filterTypes StoreFilter = new filterTypes("Store Filter", 0, "StoreFilters.html", "Stores", (int)eLayoutID.storeFilterSearchResultsGrid, (int)eLayoutID.storeFilterSearchResultsMenu);
        public static readonly filterTypes HeaderFilter = new filterTypes("Header Filter", 1, "HeaderFilters.html", "Headers"); //TT#1313-MD -jsobek -Header Filters
        public static readonly filterTypes AssortmentFilter = new filterTypes("Assortment Filter", 2, "AssortmentFilters.html", "Assortments"); //TT#1313-MD -jsobek -Header Filters
        public static readonly filterTypes StoreGroupFilter = new filterTypes("Attribute Filter", 3, "AttributeSetFilters.html", "Attribute Sets");
        public static readonly filterTypes ProductFilter = new filterTypes("Product Filter", 4, "ProductFilters.html", "Merchandise Search Results", (int)eLayoutID.productFilterSearchResultsGrid, (int)eLayoutID.productFilterSearchResultsMenu); //TT#1388-MD -jsobek -Product Filters
        public static readonly filterTypes AuditFilter = new filterTypes("Audit Filter", 5, "AuditFilters.html", "Audit Viewer"); //TT#1443-MD -jsobek -Audit Filters
        public static readonly filterTypes StoreGroupDynamicFilter = new filterTypes("Dynamic Attribute Filter", 6, "AttributeSetFilters.html", "Attribute Sets");

        private filterTypes(string Name, int dbIndex, string helpFileName, string exportFileName, int layoutID = -1, int layoutMenuID = -1)
        {
            this.Name = Name;
            this.dbIndex = dbIndex;
            this.helpFileName = helpFileName;
            this.exportFileName = exportFileName;
            this.layoutID = layoutID;
            this.layoutMenuID = layoutMenuID; //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
            filterTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex;
        public int layoutID; //TT#1313-MD -jsobek -Product Filters
        public int layoutMenuID; //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
        public string helpFileName { get; private set; }
        public string exportFileName { get; private set; } //TT#4280 -jsobek -Exporting rows in Audit to Excel 
        public static implicit operator int(filterTypes op) { return op.dbIndex; }


        public static filterTypes FromIndex(int dbIndex)
        {
            filterTypes result = filterTypeList.Find(
               delegate(filterTypes ft)
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
                //filter type was not found in the list
                return null;
            }
        }
        public static filterTypes FromString(string filterTypeName)
        {
            filterTypes result = filterTypeList.Find(
              delegate(filterTypes ft)
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
    }

    public static class FilterCommon
    {
        public static string MerchandiseStyleLevelName = "Style";
        public static string MerchandiseParentofStyleLevelName = "Sub-Class";

        public static string BuildFilterProcedureName(int filterRID, filterTypes filterType)
        {
            if (filterType == filterTypes.HeaderFilter)
            {
                if (filterRID == -1)
                {
                    return "MID_FILTER_DEFAULT_AWS";
                }
                else 
                {
                    return "WF_AWS_" + filterRID.ToString();
                }
                
            }
            else if (filterType == filterTypes.AssortmentFilter)
            {
                if (filterRID == -1)
                {
                    return "MID_FILTER_DEFAULT_SWS";
                }
                else
                {
                    return "WF_SWS_" + filterRID.ToString();
                }
            }
            //else if (filterType == filterTypes.StoreGroupFilter) //TT#1414-MD -jsobek -Attribute Set Filter
            //{
            //    if (filterRID == -1)
            //    {
            //        return "MID_FILTER_DEFAULT_XF";
            //    }
            //    else
            //    {
            //        return "XF_" + filterRID.ToString();
            //    }
            //}
            else
            {
                return string.Empty;
            }
        }
    }

   


}
