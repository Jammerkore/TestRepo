using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public delegate bool CompareStoreValue(filterStoreEnhancedProfile prof, filterCondition fc);
    public delegate object GetStoreValue(filterStoreEnhancedProfile prof);
    /// <summary>
    /// simple wrapper class so we are able to compare store related things that reside outside of the store profile
    /// </summary>
    public class filterStoreEnhancedProfile
    {
        public StoreProfile prof;
        //room for other stuff, like Store Eligibility Model
    }

    public sealed class filterStoreStatusTypes
    {
        public static List<filterStoreStatusTypes> storeStatusTypeList = new List<filterStoreStatusTypes>();
        public static readonly filterStoreStatusTypes None = new filterStoreStatusTypes(0, -1, filterValueTypes.Text);
        public static readonly filterStoreStatusTypes New = new filterStoreStatusTypes(1, 804000, filterValueTypes.Text);
        public static readonly filterStoreStatusTypes Comp = new filterStoreStatusTypes(2, 804001, filterValueTypes.Text);
        public static readonly filterStoreStatusTypes NonComp = new filterStoreStatusTypes(3, 804002, filterValueTypes.Text);
        public static readonly filterStoreStatusTypes Closed = new filterStoreStatusTypes(4, 804003, filterValueTypes.Text);
        public static readonly filterStoreStatusTypes Preopen = new filterStoreStatusTypes(5, 804004, filterValueTypes.Text);

        private filterStoreStatusTypes(int dbIndex, int textCode, filterValueTypes valueType)
        {
            string n;
            if (textCode == -1)
            {
                n = "No Status";
            }
            else
            {
                n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
            }
            this.Name = n;
            this.dbIndex = dbIndex;
            this.textCode = textCode; //TT#4466 -jsobek -Store Filter -Store Status
            this.valueType = valueType;
            storeStatusTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public int textCode { get; private set; } //TT#4466 -jsobek -Store Filter -Store Status
        public filterValueTypes valueType { get; private set; }
        public static implicit operator int(filterStoreStatusTypes op) { return op.dbIndex; }


        public static filterStoreStatusTypes FromIndex(int dbIndex)
        {
            filterStoreStatusTypes result = storeStatusTypeList.Find(
               delegate(filterStoreStatusTypes ft)
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
                //status type was not found in the list
                return null;
            }
        }

        //Begin TT#4466 -jsobek -Store Filter -Store Status
        public static filterStoreStatusTypes FromTextCode(int textCode)
        {
            return storeStatusTypeList.Find(x => x.textCode == textCode);

        }
        //End TT#4466 -jsobek -Store Filter -Store Status

        public static filterStoreStatusTypes FromString(string storeFieldTypeName)
        {
            filterStoreStatusTypes result = storeStatusTypeList.Find(
              delegate(filterStoreStatusTypes ft)
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
                //status type was not found in the list
                return null;
            }
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("storeStatuses");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));
            dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            foreach (filterStoreStatusTypes fieldType in storeStatusTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueType.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    public sealed class filterStoreFieldTypes
    {
        public static List<filterStoreFieldTypes> storeFieldTypeList = new List<filterStoreFieldTypes>();
        public static readonly filterStoreFieldTypes StoreID = new filterStoreFieldTypes(0, 900000, storeFieldTypes.StoreID, new filterDataTypes(filterValueTypes.Text), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.StoreId, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.StoreId; });
        public static readonly filterStoreFieldTypes StoreName = new filterStoreFieldTypes(1, 900001, storeFieldTypes.StoreName, new filterDataTypes(filterValueTypes.Text), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.StoreName, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.StoreName; });
        // Begin TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
        // Begin TT#1796-MD - JSmith - Store Profiles - Store Field Marked for Delete
        //public static readonly filterStoreFieldTypes ActiveInd = new filterStoreFieldTypes(2, 900002, storeFieldTypes.ActiveInd, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ActiveInd, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ActiveInd; });
        // End TT#1796-MD - JSmith - Store Profiles - Store Field Marked for Delete
        public static readonly filterStoreFieldTypes ActiveInd = new filterStoreFieldTypes(2, 900002, storeFieldTypes.ActiveInd, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ActiveInd, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ActiveInd; }, false);
        // End TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
        public static readonly filterStoreFieldTypes City = new filterStoreFieldTypes(3, 900003, storeFieldTypes.City, new filterDataTypes(filterValueTypes.Text), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.City, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.City; });
        public static readonly filterStoreFieldTypes State = new filterStoreFieldTypes(4, 900004, storeFieldTypes.State, new filterDataTypes(filterValueTypes.Text), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.State, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.State; });
        public static readonly filterStoreFieldTypes SellingSqFt = new filterStoreFieldTypes(5, 900005, storeFieldTypes.SellingSqFt, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToInt(e.prof.SellingSqFt, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.SellingSqFt; });
        public static readonly filterStoreFieldTypes SellingOpenDate = new filterStoreFieldTypes(6, 900006, storeFieldTypes.SellingOpenDate, new filterDataTypes(filterValueTypes.Date, filterDateTypes.DateOnly), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.SellingOpenDt, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.SellingOpenDt; });
        public static readonly filterStoreFieldTypes SellingCloseDate = new filterStoreFieldTypes(7, 900007, storeFieldTypes.SellingCloseDate, new filterDataTypes(filterValueTypes.Date, filterDateTypes.DateOnly), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.SellingCloseDt, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.SellingCloseDt; });
        public static readonly filterStoreFieldTypes StockOpenDate = new filterStoreFieldTypes(8, 900008, storeFieldTypes.StockOpenDate, new filterDataTypes(filterValueTypes.Date, filterDateTypes.DateOnly), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.StockOpenDt, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.StockOpenDt; });
        public static readonly filterStoreFieldTypes StockCloseDate = new filterStoreFieldTypes(9, 900009, storeFieldTypes.StockCloseDate, new filterDataTypes(filterValueTypes.Date, filterDateTypes.DateOnly), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.StockCloseDt, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.StockCloseDt; });
        public static readonly filterStoreFieldTypes LeadTime = new filterStoreFieldTypes(10, 900010, storeFieldTypes.LeadTime, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToInt(e.prof.LeadTime, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.LeadTime; });
        public static readonly filterStoreFieldTypes ShipOnMonday = new filterStoreFieldTypes(11, 900011, storeFieldTypes.ShipOnMonday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnMonday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnMonday; });
        public static readonly filterStoreFieldTypes ShipOnTuesday = new filterStoreFieldTypes(12, 900012, storeFieldTypes.ShipOnTuesday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnTuesday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnTuesday; });
        public static readonly filterStoreFieldTypes ShipOnWednesday = new filterStoreFieldTypes(13, 900013, storeFieldTypes.ShipOnWednesday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnWednesday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnWednesday; });
        public static readonly filterStoreFieldTypes ShipOnThursday = new filterStoreFieldTypes(14, 900014, storeFieldTypes.ShipOnThursday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnThursday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnThursday; });
        public static readonly filterStoreFieldTypes ShipOnFriday = new filterStoreFieldTypes(15, 900015, storeFieldTypes.ShipOnFriday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnFriday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnFriday; });
        public static readonly filterStoreFieldTypes ShipOnSaturday = new filterStoreFieldTypes(16, 900016, storeFieldTypes.ShipOnSaturday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnSaturday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnSaturday; });
        public static readonly filterStoreFieldTypes ShipOnSunday = new filterStoreFieldTypes(17, 900017, storeFieldTypes.ShipOnSunday, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnSunday, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.ShipOnSunday; });
        public static readonly filterStoreFieldTypes StoreDescription = new filterStoreFieldTypes(18, 900024, storeFieldTypes.StoreDescription, new filterDataTypes(filterValueTypes.Text), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.StoreDescription, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.StoreDescription; });
        public static readonly filterStoreFieldTypes SimilarStoreModel = new filterStoreFieldTypes(19, 900352, storeFieldTypes.SimilarStoreModel, new filterDataTypes(filterValueTypes.Boolean), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.SimilarStoreModel, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.SimilarStoreModel; });
        public static readonly filterStoreFieldTypes ImoID = new filterStoreFieldTypes(20, 900805, storeFieldTypes.ImoID, new filterDataTypes(filterValueTypes.Text), delegate(filterStoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.IMO_ID, fc); }, delegate(filterStoreEnhancedProfile e) { return e.prof.IMO_ID; });

        //Stock Lead Weeks is part of the Store Eligibility Model - which we dont get yet as part of the store enhanced profile


        // Begin TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
        //private filterStoreFieldTypes(int dbIndex, int textCode, storeFieldTypes storeFieldType, filterDataTypes dataType, CompareStoreValue compareStoreValue, GetStoreValue getStoreValue)
        private filterStoreFieldTypes(int dbIndex, int textCode, storeFieldTypes storeFieldType, filterDataTypes dataType, CompareStoreValue compareStoreValue, GetStoreValue getStoreValue, bool allowFiltering = true)
        // End TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
        {
            string n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
            this.Name = n;
            this.dbIndex = dbIndex;
            this.dataType = dataType;
            this.compareStoreValue = compareStoreValue;
            this.getStoreValue = getStoreValue;
            this.storeFieldType = storeFieldType;
            this.allowFiltering = allowFiltering;  // TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
            storeFieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }

        public filterDataTypes dataType { get; private set; }
        private CompareStoreValue compareStoreValue;
        private GetStoreValue getStoreValue;
        public storeFieldTypes storeFieldType;
        public bool allowFiltering;  // TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
        // public static implicit operator string(storeFieldTypes op) { return op.Name; }
        public static implicit operator int(filterStoreFieldTypes op) { return op.dbIndex; }


        public bool CompareStoreValue(filterStoreEnhancedProfile prof, filterCondition fc)
        {
            return compareStoreValue(prof, fc);
        }
        public object GetStoreValue(filterStoreEnhancedProfile prof)
        {
            return getStoreValue(prof);
        }



        public static filterStoreFieldTypes FromIndex(int dbIndex)
        {
            filterStoreFieldTypes result = storeFieldTypeList.Find(
               delegate(filterStoreFieldTypes ft)
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
                //storeField type was not found in the list
                return null;
            }
        }

        public static filterStoreFieldTypes FromField(storeFieldTypes storeFieldType)
        {
            filterStoreFieldTypes result = storeFieldTypeList.Find(x => x.storeFieldType.fieldIndex == storeFieldType.fieldIndex);
              
                return result;
            
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("storeFields");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));
            dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            foreach (filterStoreFieldTypes fieldType in storeFieldTypeList)
            {
                // Begin TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
                if (fieldType.allowFiltering)
                {
                // End TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
                    DataRow dr = dt.NewRow();
                    dr["FIELD_INDEX"] = fieldType.dbIndex;
                    dr["FIELD_NAME"] = fieldType.Name;
                    dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.dataType.valueType.dbIndex;
                    dt.Rows.Add(dr);
                // Begin TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
                }
                // End TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
            }
            return dt;
        }
        public static filterDataTypes GetValueTypeInfoForField(int fieldIndex)
        {
            filterStoreFieldTypes storeField = filterStoreFieldTypes.FromIndex(fieldIndex);
            return storeField.dataType;
        }
        public static string GetNameFromIndex(int fieldIndex)
        {
            filterStoreFieldTypes field = filterStoreFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }
        public static DataTable GetValueListForStoreFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            //dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            filterStoreFieldTypes storeFieldType = filterStoreFieldTypes.FromIndex(fieldIndex);



            return dt;
        }
    }


    /// <summary>
    /// Corresponds to eFilterCubeModifyer
    /// </summary>
    public sealed class variableValueTypes
    {
        public static List<variableValueTypes> fieldTypeList = new List<variableValueTypes>();
        public static readonly variableValueTypes StoreDetail = new variableValueTypes(0, "Store Detail", filterValueTypes.Text);
        public static readonly variableValueTypes StoreTotal = new variableValueTypes(1, "Store Total", filterValueTypes.Text);
        public static readonly variableValueTypes StoreAverage = new variableValueTypes(2, "Store Average", filterValueTypes.Text);
        public static readonly variableValueTypes ChainDetail = new variableValueTypes(3, "Chain Detail", filterValueTypes.Text);



        private variableValueTypes(int dbIndex, string Name, filterValueTypes valueType)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            this.valueType = valueType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public filterValueTypes valueType { get; private set; }
        public static implicit operator int(variableValueTypes op) { return op.dbIndex; }


        public static variableValueTypes FromIndex(int dbIndex)
        {
            variableValueTypes result = fieldTypeList.Find(
               delegate(variableValueTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;

        }
        public static variableValueTypes FromString(string storeFieldTypeName)
        {
            variableValueTypes result = fieldTypeList.Find(
              delegate(variableValueTypes ft)
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
            dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            foreach (variableValueTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueType.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static filterValueTypes GetValueTypeForField(int fieldIndex)
        {
            variableValueTypes storeField = variableValueTypes.FromIndex(fieldIndex);
            return storeField.valueType;
        }
        public static eFilterCubeModifyer ToFilterCubeModifier(variableValueTypes variableValueType)
        {
            if (variableValueType == variableValueTypes.StoreDetail)
            {
                return eFilterCubeModifyer.StoreDetail;
            }
            else if (variableValueType == variableValueTypes.ChainDetail)
            {
                return eFilterCubeModifyer.ChainDetail;
            }
            else if (variableValueType == variableValueTypes.StoreAverage)
            {
                return eFilterCubeModifyer.StoreAverage;
            }
            else
            {
                return eFilterCubeModifyer.StoreTotal;
            }
        }
    }

    public sealed class variableTimeTypes
    {
        public static List<variableTimeTypes> fieldTypeList = new List<variableTimeTypes>();
        public static readonly variableTimeTypes Any = new variableTimeTypes(0, "Any", filterValueTypes.Text);
        public static readonly variableTimeTypes All = new variableTimeTypes(1, "All", filterValueTypes.Text);
        //public static readonly variableTimeTypes Join = new variableTimeTypes(2, "Join", filterValueTypes.Text); //TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        public static readonly variableTimeTypes Average = new variableTimeTypes(3, "Average", filterValueTypes.Text);
        public static readonly variableTimeTypes Total = new variableTimeTypes(4, "Total", filterValueTypes.Text);
        public static readonly variableTimeTypes Corresponding = new variableTimeTypes(5, "Corresponding", filterValueTypes.Text);


        private variableTimeTypes(int dbIndex, string Name, filterValueTypes valueType)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            this.valueType = valueType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public filterValueTypes valueType { get; private set; }

        public static implicit operator int(variableTimeTypes op) 
        {
            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            if (op.dbIndex == 2)
            {
                return 0;
            }
            else
            {
                return op.dbIndex;
            }
            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        }


        public static variableTimeTypes FromIndex(int dbIndex)
        {
            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            //Change Join to Any
            if (dbIndex == 2)
            {
                dbIndex = 0;
            }
            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            variableTimeTypes result = fieldTypeList.Find(
               delegate(variableTimeTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;

        }
        public static variableTimeTypes FromString(string storeFieldTypeName)
        {
            variableTimeTypes result = fieldTypeList.Find(
              delegate(variableTimeTypes ft)
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
            dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            foreach (variableTimeTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueType.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static filterValueTypes GetValueTypeForField(int fieldIndex)
        {
            variableTimeTypes storeField = variableTimeTypes.FromIndex(fieldIndex);
            return storeField.valueType;
        }
    }
}
