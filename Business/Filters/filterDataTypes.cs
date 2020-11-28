using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Business
{
    /// <summary>
    /// Class to hold information about the data type.
    /// Holds type data as well as formatting data.
    /// </summary>
    public class filterDataTypes
    {
        public filterValueTypes valueType;
        public filterDateTypes dateType;
        public filterNumericTypes numericType;

        public filterDataTypes(filterValueTypes valueType)
        {
            this.valueType = valueType;
        }
        public filterDataTypes(filterValueTypes valueType, filterDateTypes dateType)
        {
            this.valueType = valueType;
            this.dateType = dateType;
        }
        public filterDataTypes(filterValueTypes valueType, filterNumericTypes numericType)
        {
            this.valueType = valueType;
            this.numericType = numericType;
        }
    }

    public sealed class filterValueTypes
    {
        public static List<filterValueTypes> valueTypeList = new List<filterValueTypes>();
        public static readonly filterValueTypes Text = new filterValueTypes(0);
        public static readonly filterValueTypes Date = new filterValueTypes(1);
        public static readonly filterValueTypes Numeric = new filterValueTypes(2);
        public static readonly filterValueTypes Dollar = new filterValueTypes(3);
        public static readonly filterValueTypes List = new filterValueTypes(5);
        public static readonly filterValueTypes Boolean = new filterValueTypes(6);
        public static readonly filterValueTypes Calendar = new filterValueTypes(7);   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only


        private filterValueTypes(int dbIndex)
        {
            this.dbIndex = dbIndex;
            valueTypeList.Add(this);
        }
        public int dbIndex { get; private set; }

        public static implicit operator int(filterValueTypes op) { return op.dbIndex; }


        public static filterValueTypes FromIndex(int dbIndex)
        {
            filterValueTypes result = valueTypeList.Find(
               delegate(filterValueTypes ft)
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
                //value type was not found in the list so just return text type
                return Text;
            }
        }

    }

    public sealed class filterDateTypes
    {
        public static List<filterDateTypes> navTypeList = new List<filterDateTypes>();
        public static readonly filterDateTypes DateAndTime = new filterDateTypes(1);
        public static readonly filterDateTypes DateOnly = new filterDateTypes(2);
        public static readonly filterDateTypes TimeOnly = new filterDateTypes(3);


        private filterDateTypes(int Index)
        {
            this.Index = Index;
            navTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(filterDateTypes op) { return op.Index; }


        public static filterDateTypes FromIndex(int Index)
        {
            filterDateTypes result = navTypeList.Find(
               delegate(filterDateTypes ft)
               {
                   return ft.Index == Index;
               }
               );
            return result;
        }

    }

    public sealed class filterNumericTypes
    {
        public static List<filterNumericTypes> navTypeList = new List<filterNumericTypes>();
        public static readonly filterNumericTypes DoubleFreeForm = new filterNumericTypes(1);
        public static readonly filterNumericTypes Dollar = new filterNumericTypes(2);
        public static readonly filterNumericTypes Integer = new filterNumericTypes(3);


        private filterNumericTypes(int Index)
        {
            this.Index = Index;
            navTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(filterNumericTypes op) { return op.Index; }


        public static filterNumericTypes FromIndex(int Index)
        {
            filterNumericTypes result = navTypeList.Find(
               delegate(filterNumericTypes ft)
               {
                   return ft.Index == Index;
               }
               );
            return result;
        }

    }

    //Support Types
    public sealed class filterNavigationTypes
    {
        public static List<filterNavigationTypes> navTypeList = new List<filterNavigationTypes>();
        public static readonly filterNavigationTypes Info = new filterNavigationTypes(0);
        public static readonly filterNavigationTypes Condition = new filterNavigationTypes(1);
        public static readonly filterNavigationTypes SortBy = new filterNavigationTypes(2);


        private filterNavigationTypes(int Index)
        {
            this.Index = Index;
            navTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(filterNavigationTypes op) { return op.Index; }


        public static filterNavigationTypes FromIndex(int Index)
        {
            filterNavigationTypes result = navTypeList.Find(
               delegate(filterNavigationTypes ft)
               {
                   return ft.Index == Index;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

    }

    public sealed class filterListValueTypes
    {
        public static List<filterListValueTypes> listValueTypeList = new List<filterListValueTypes>();
        public static readonly filterListValueTypes None = new filterListValueTypes(0);
        public static readonly filterListValueTypes StoreStatus = new filterListValueTypes(1);
        public static readonly filterListValueTypes HeaderStatus = new filterListValueTypes(2);
        public static readonly filterListValueTypes HeaderTypes = new filterListValueTypes(3);
        public static readonly filterListValueTypes StoreRID = new filterListValueTypes(4);
        public static readonly filterListValueTypes StoreCharacteristicRID = new filterListValueTypes(5);
        public static readonly filterListValueTypes HeaderCharacteristicRID = new filterListValueTypes(6);
        public static readonly filterListValueTypes StoreField = new filterListValueTypes(7);
        public static readonly filterListValueTypes HeaderField = new filterListValueTypes(8);
        public static readonly filterListValueTypes StoreGroupLevel = new filterListValueTypes(9);
        public static readonly filterListValueTypes ProductCharacteristicRID = new filterListValueTypes(10);
        public static readonly filterListValueTypes ProductLevels = new filterListValueTypes(11);
        public static readonly filterListValueTypes ProductHierarchies = new filterListValueTypes(12);
        public static readonly filterListValueTypes AuditField = new filterListValueTypes(13);
        public static readonly filterListValueTypes AuditExecutionStatus = new filterListValueTypes(14);
        public static readonly filterListValueTypes AuditCompletionStatus = new filterListValueTypes(15);
        public static readonly filterListValueTypes AuditMessageLevel = new filterListValueTypes(16);
        public static readonly filterListValueTypes Users = new filterListValueTypes(17);
        public static readonly filterListValueTypes AssortmentField = new filterListValueTypes(18);  // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        public static readonly filterListValueTypes AssortmentTypes = new filterListValueTypes(19);  // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        public static readonly filterListValueTypes AssortmentStatus = new filterListValueTypes(20);  // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

        private filterListValueTypes(int Index)
        {
            this.Index = Index;
            listValueTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(filterListValueTypes op) { return op.Index; }


        public static filterListValueTypes FromIndex(int Index)
        {
            filterListValueTypes result = listValueTypeList.Find(
               delegate(filterListValueTypes ft)
               {
                   return ft.Index == Index;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

    }

    public sealed class filterListConstantTypes
    {
        public static List<filterListConstantTypes> constantList = new List<filterListConstantTypes>();
        public static readonly filterListConstantTypes None = new filterListConstantTypes("None", 0);
        public static readonly filterListConstantTypes All = new filterListConstantTypes("All", 1);
        public static readonly filterListConstantTypes HasValues = new filterListConstantTypes("Has Values", 2);

        private filterListConstantTypes(string Name, int dbIndex)
        {
            this.Name = Name;
            this.dbIndex = dbIndex;
            constantList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex;
        public static implicit operator string(filterListConstantTypes op) { return op.Name; }


        public static filterListConstantTypes FromIndex(int dbIndex)
        {
            filterListConstantTypes result = constantList.Find(
               delegate(filterListConstantTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;

        }
        public static filterListConstantTypes FromString(string filterTypeName)
        {
            filterListConstantTypes result = constantList.Find(
              delegate(filterListConstantTypes ft)
              {
                  return ft.Name == filterTypeName;
              }
              );

            return result;

        }
    }

    public sealed class filterLogicTypes
    {
        public static List<filterLogicTypes> fieldTypeList = new List<filterLogicTypes>();
        public static readonly filterLogicTypes And = new filterLogicTypes(0, "And");
        public static readonly filterLogicTypes Or = new filterLogicTypes(1, "Or");

        private filterLogicTypes(int dbIndex, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.Index = dbIndex;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterLogicTypes op) { return op.Index; }


        public static filterLogicTypes FromIndex(int dbIndex)
        {
            filterLogicTypes result = fieldTypeList.Find(
               delegate(filterLogicTypes ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterLogicTypes FromString(string storeFieldTypeName)
        {
            filterLogicTypes result = fieldTypeList.Find(
              delegate(filterLogicTypes ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("fields");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));

            foreach (filterLogicTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.Index;
                dr["FIELD_NAME"] = fieldType.Name;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }

    public sealed class filterSortByDirectionTypes
    {
        public static List<filterSortByDirectionTypes> fieldTypeList = new List<filterSortByDirectionTypes>();
        public static readonly filterSortByDirectionTypes Ascending = new filterSortByDirectionTypes(0, "Ascending");
        public static readonly filterSortByDirectionTypes Descending = new filterSortByDirectionTypes(1, "Descending");

        private filterSortByDirectionTypes(int dbIndex, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }

        public static implicit operator int(filterSortByDirectionTypes op) { return op.dbIndex; }


        public static filterSortByDirectionTypes FromIndex(int dbIndex)
        {
            filterSortByDirectionTypes result = fieldTypeList.Find(
               delegate(filterSortByDirectionTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;

        }
        public static filterSortByDirectionTypes FromString(string storeFieldTypeName)
        {
            filterSortByDirectionTypes result = fieldTypeList.Find(
              delegate(filterSortByDirectionTypes ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("fields");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));

            foreach (filterSortByDirectionTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }

    //Operator Types
    public sealed class filterNumericOperatorTypes
    {
        public static List<filterNumericOperatorTypes> opList = new List<filterNumericOperatorTypes>();
        public static readonly filterNumericOperatorTypes DoesEqual = new filterNumericOperatorTypes(0, "=", "equals");
        public static readonly filterNumericOperatorTypes DoesNotEqual = new filterNumericOperatorTypes(1, "!=", "does not equal");
        public static readonly filterNumericOperatorTypes GreaterThan = new filterNumericOperatorTypes(2, ">", "greater than");
        public static readonly filterNumericOperatorTypes GreaterThanOrEqualTo = new filterNumericOperatorTypes(3, ">=", "greater than or equal to");
        public static readonly filterNumericOperatorTypes LessThan = new filterNumericOperatorTypes(4, "<", "less than");
        public static readonly filterNumericOperatorTypes LessThanOrEqualTo = new filterNumericOperatorTypes(5, "<=", "less than or equal to");
        public static readonly filterNumericOperatorTypes Between = new filterNumericOperatorTypes(6, "bt", "between");

        private filterNumericOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(filterNumericOperatorTypes op) { return op.dbIndex; }


        public static filterNumericOperatorTypes FromIndex(int dbIndex)
        {
            filterNumericOperatorTypes result = opList.Find(
               delegate(filterNumericOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static filterNumericOperatorTypes FromSymbol(string symbol)
        {
            filterNumericOperatorTypes result = opList.Find(
              delegate(filterNumericOperatorTypes ft)
              {
                  return ft.symbol == symbol;
              }
              );

            return result;
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("operators");
            dt.Columns.Add("OPERATOR_SYMBOL");
            dt.Columns.Add("OPERATOR_DESCRIPTION");
            dt.Columns.Add("OPERATOR_INDEX", typeof(int));


            foreach (filterNumericOperatorTypes opType in opList)
            {
                DataRow dr = dt.NewRow();
                dr["OPERATOR_INDEX"] = opType.dbIndex;
                dr["OPERATOR_SYMBOL"] = opType.symbol;
                dr["OPERATOR_DESCRIPTION"] = opType.description;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    public sealed class filterDateOperatorTypes
    {
        public static List<filterDateOperatorTypes> opList = new List<filterDateOperatorTypes>();
        public static readonly filterDateOperatorTypes Unrestricted = new filterDateOperatorTypes(0, "u", "Unrestricted");
        public static readonly filterDateOperatorTypes Last24Hours = new filterDateOperatorTypes(1, "last24hours", "Last 24 Hours");
        public static readonly filterDateOperatorTypes Last7Days = new filterDateOperatorTypes(2, "last7days", "Last 7 days");
        public static readonly filterDateOperatorTypes Between = new filterDateOperatorTypes(3, "bt", "Between");
        public static readonly filterDateOperatorTypes Specify = new filterDateOperatorTypes(4, "s", "Specify a date range");


        private filterDateOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(filterDateOperatorTypes op) { return op.dbIndex; }


        public static filterDateOperatorTypes FromIndex(int dbIndex)
        {
            filterDateOperatorTypes result = opList.Find(
               delegate(filterDateOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static filterDateOperatorTypes FromSymbol(string symbol)
        {
            filterDateOperatorTypes result = opList.Find(
              delegate(filterDateOperatorTypes ft)
              {
                  return ft.symbol == symbol;
              }
              );

            return result;
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("operators");
            dt.Columns.Add("OPERATOR_SYMBOL");
            dt.Columns.Add("OPERATOR_DESCRIPTION");
            dt.Columns.Add("OPERATOR_INDEX", typeof(int));


            foreach (filterDateOperatorTypes opType in opList)
            {
                DataRow dr = dt.NewRow();
                dr["OPERATOR_INDEX"] = opType.dbIndex;
                dr["OPERATOR_SYMBOL"] = opType.symbol;
                dr["OPERATOR_DESCRIPTION"] = opType.description;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
	public sealed class filterCalendarDateOperatorTypes
    {
        public static List<filterCalendarDateOperatorTypes> opList = new List<filterCalendarDateOperatorTypes>();
        public static readonly filterCalendarDateOperatorTypes Unrestricted = new filterCalendarDateOperatorTypes(0, "Unrestricted", "Unrestricted");
        public static readonly filterCalendarDateOperatorTypes Last1Week = new filterCalendarDateOperatorTypes(1, "last1week", "Last 1 Week");
        public static readonly filterCalendarDateOperatorTypes Next1Week = new filterCalendarDateOperatorTypes(2, "next1week", "Next 1 Week");
        public static readonly filterCalendarDateOperatorTypes Next4Weeks = new filterCalendarDateOperatorTypes(3, "next4weeks", "Next 4 Weeks");
        public static readonly filterCalendarDateOperatorTypes Between = new filterCalendarDateOperatorTypes(4, "bt", "between");
        public static readonly filterCalendarDateOperatorTypes Specify = new filterCalendarDateOperatorTypes(5, "s", "Specify a date range");

        private filterCalendarDateOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(filterCalendarDateOperatorTypes op) { return op.dbIndex; }


        public static filterCalendarDateOperatorTypes FromIndex(int dbIndex)
        {
            filterCalendarDateOperatorTypes result = opList.Find(
               delegate(filterCalendarDateOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static filterCalendarDateOperatorTypes FromSymbol(string symbol)
        {
            filterCalendarDateOperatorTypes result = opList.Find(
              delegate(filterCalendarDateOperatorTypes ft)
              {
                  return ft.symbol == symbol;
              }
              );

            return result;
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("operators");
            dt.Columns.Add("OPERATOR_SYMBOL");
            dt.Columns.Add("OPERATOR_DESCRIPTION");
            dt.Columns.Add("OPERATOR_INDEX", typeof(int));


            foreach (filterCalendarDateOperatorTypes opType in opList)
            {
                DataRow dr = dt.NewRow();
                dr["OPERATOR_INDEX"] = opType.dbIndex;
                dr["OPERATOR_SYMBOL"] = opType.symbol;
                dr["OPERATOR_DESCRIPTION"] = opType.description;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
	// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

    public sealed class filterStringOperatorTypes
    {
        public static List<filterStringOperatorTypes> opList = new List<filterStringOperatorTypes>();

        public static readonly filterStringOperatorTypes DoesEqual = new filterStringOperatorTypes(0, "e", "equals"); //case insensitive
        public static readonly filterStringOperatorTypes StartsWith = new filterStringOperatorTypes(1, "sw", "starts with"); //case insensitive
        public static readonly filterStringOperatorTypes EndsWith = new filterStringOperatorTypes(2, "ew", "ends with"); //case insensitive
        public static readonly filterStringOperatorTypes Contains = new filterStringOperatorTypes(3, "c", "contains"); //case insensitive

        public static readonly filterStringOperatorTypes DoesEqualExactly = new filterStringOperatorTypes(4, "ee", "equals exactly"); //case sensitive
        public static readonly filterStringOperatorTypes StartsExactlyWith = new filterStringOperatorTypes(5, "sew", "starts exactly with"); //case sensitive
        public static readonly filterStringOperatorTypes EndsExactlyWith = new filterStringOperatorTypes(6, "eew", "ends exactly with"); //case sensitive
        public static readonly filterStringOperatorTypes ContainsExactly = new filterStringOperatorTypes(7, "ce", "contains exactly"); //case sensitive


        private filterStringOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(filterStringOperatorTypes op) { return op.dbIndex; }


        public static filterStringOperatorTypes FromIndex(int dbIndex)
        {
            filterStringOperatorTypes result = opList.Find(
               delegate(filterStringOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static filterStringOperatorTypes FromSymbol(string symbol)
        {
            filterStringOperatorTypes result = opList.Find(
              delegate(filterStringOperatorTypes ft)
              {
                  return ft.symbol == symbol;
              }
              );

            return result;
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("operators");
            dt.Columns.Add("OPERATOR_INDEX", typeof(int));
            dt.Columns.Add("OPERATOR_SYMBOL");
            dt.Columns.Add("OPERATOR_DESCRIPTION");

            foreach (filterStringOperatorTypes opType in opList)
            {
                DataRow dr = dt.NewRow();
                dr["OPERATOR_INDEX"] = opType.dbIndex;
                dr["OPERATOR_SYMBOL"] = opType.symbol;
                dr["OPERATOR_DESCRIPTION"] = opType.description;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    public sealed class filterListOperatorTypes
    {
        public static List<filterListOperatorTypes> opList = new List<filterListOperatorTypes>();

        public static readonly filterListOperatorTypes Includes = new filterListOperatorTypes(0, "In", "includes");
        public static readonly filterListOperatorTypes Excludes = new filterListOperatorTypes(1, "Not In", "excludes");


        private filterListOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(filterListOperatorTypes op) { return op.dbIndex; }


        public static filterListOperatorTypes FromIndex(int dbIndex)
        {
            filterListOperatorTypes result = opList.Find(
               delegate(filterListOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static filterListOperatorTypes FromSymbol(string symbol)
        {
            filterListOperatorTypes result = opList.Find(
              delegate(filterListOperatorTypes ft)
              {
                  return ft.symbol == symbol;
              }
              );

            return result;
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("operators");
            dt.Columns.Add("OPERATOR_INDEX", typeof(int));
            dt.Columns.Add("OPERATOR_SYMBOL");
            dt.Columns.Add("OPERATOR_DESCRIPTION");

            foreach (filterListOperatorTypes opType in opList)
            {
                DataRow dr = dt.NewRow();
                dr["OPERATOR_INDEX"] = opType.dbIndex;
                dr["OPERATOR_SYMBOL"] = opType.symbol;
                dr["OPERATOR_DESCRIPTION"] = opType.description;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    public sealed class filterPercentageOperatorTypes
    {
        public static List<filterPercentageOperatorTypes> opList = new List<filterPercentageOperatorTypes>();

        public static readonly filterPercentageOperatorTypes PercentOf = new filterPercentageOperatorTypes(0, "% of", "percent of");
        public static readonly filterPercentageOperatorTypes PercentChange = new filterPercentageOperatorTypes(1, "% change", "percent change");


        private filterPercentageOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(filterPercentageOperatorTypes op) { return op.dbIndex; }


        public static filterPercentageOperatorTypes FromIndex(int dbIndex)
        {
            filterPercentageOperatorTypes result = opList.Find(
               delegate(filterPercentageOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static filterPercentageOperatorTypes FromSymbol(string symbol)
        {
            filterPercentageOperatorTypes result = opList.Find(
              delegate(filterPercentageOperatorTypes ft)
              {
                  return ft.symbol == symbol;
              }
              );

            return result;
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("operators");
            dt.Columns.Add("OPERATOR_INDEX", typeof(int));
            dt.Columns.Add("OPERATOR_SYMBOL");
            dt.Columns.Add("OPERATOR_DESCRIPTION");

            foreach (filterPercentageOperatorTypes opType in opList)
            {
                DataRow dr = dt.NewRow();
                dr["OPERATOR_INDEX"] = opType.dbIndex;
                dr["OPERATOR_SYMBOL"] = opType.symbol;
                dr["OPERATOR_DESCRIPTION"] = opType.description;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
