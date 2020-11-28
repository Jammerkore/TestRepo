using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Business
{




    public sealed class navigationTypes
    {
        public static List<navigationTypes> navTypeList = new List<navigationTypes>();
        public static readonly navigationTypes Info = new navigationTypes(0);
        public static readonly navigationTypes Condition = new navigationTypes(1);
        public static readonly navigationTypes SortBy = new navigationTypes(2);


        private navigationTypes(int Index)
        {
            this.Index = Index;
            navTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(navigationTypes op) { return op.Index; }


        public static navigationTypes FromIndex(int Index)
        {
            navigationTypes result = navTypeList.Find(
               delegate(navigationTypes ft)
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


    public delegate bool CompareStoreValue(StoreEnhancedProfile prof, filterCondition fc);
    public delegate object GetStoreValue(StoreEnhancedProfile prof);


    /// <summary>
    /// simple wrapper class so we are able to compare store related things that reside outside of the store profile
    /// </summary>
    public class StoreEnhancedProfile
    {
        public StoreProfile prof;
        //room for other stuff, like Store Eligibility Model
    }

    public sealed class storeFieldTypes
    {
        public static List<storeFieldTypes> storeFieldTypeList = new List<storeFieldTypes>();
        public static readonly storeFieldTypes StoreID = new storeFieldTypes(0, 900000, new valueInfoTypes(valueTypes.Text), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.StoreId, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.StoreId; });
        public static readonly storeFieldTypes StoreName = new storeFieldTypes(1, 900001, new valueInfoTypes(valueTypes.Text), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.StoreName, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.StoreName; });
        public static readonly storeFieldTypes ActiveInd = new storeFieldTypes(2, 900002, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ActiveInd, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ActiveInd; });
        public static readonly storeFieldTypes City = new storeFieldTypes(3, 900003, new valueInfoTypes(valueTypes.Text), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.City, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.City; });
        public static readonly storeFieldTypes State = new storeFieldTypes(4, 900004, new valueInfoTypes(valueTypes.Text), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.State, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.State; });
        public static readonly storeFieldTypes SellingSqFt = new storeFieldTypes(5, 900005, new valueInfoTypes(valueTypes.Numeric, numericTypes.Integer), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToInt(e.prof.SellingSqFt, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.SellingSqFt; });
        public static readonly storeFieldTypes SellingOpenDate = new storeFieldTypes(6, 900006, new valueInfoTypes(valueTypes.Date, dateTypes.DateOnly), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.SellingOpenDt, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.SellingOpenDt; });
        public static readonly storeFieldTypes SellingCloseDate = new storeFieldTypes(7, 900007, new valueInfoTypes(valueTypes.Date, dateTypes.DateOnly), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.SellingCloseDt, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.SellingCloseDt; });
        public static readonly storeFieldTypes StockOpenDate = new storeFieldTypes(8, 900008, new valueInfoTypes(valueTypes.Date, dateTypes.DateOnly), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.StockOpenDt, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.StockOpenDt; });
        public static readonly storeFieldTypes StockCloseDate = new storeFieldTypes(9, 900009, new valueInfoTypes(valueTypes.Date, dateTypes.DateOnly), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToDate(e.prof.StockCloseDt, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.StockCloseDt; });
        public static readonly storeFieldTypes LeadTime = new storeFieldTypes(10, 900010, new valueInfoTypes(valueTypes.Numeric, numericTypes.Integer), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToInt(e.prof.LeadTime, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.LeadTime; });
        public static readonly storeFieldTypes ShipOnMonday = new storeFieldTypes(11, 900011, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnMonday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnMonday; });
        public static readonly storeFieldTypes ShipOnTuesday = new storeFieldTypes(12, 900012, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnTuesday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnTuesday; });
        public static readonly storeFieldTypes ShipOnWednesday = new storeFieldTypes(13, 900013, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnWednesday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnWednesday; });
        public static readonly storeFieldTypes ShipOnThursday = new storeFieldTypes(14, 900014, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnThursday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnThursday; });
        public static readonly storeFieldTypes ShipOnFriday = new storeFieldTypes(15, 900015, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnFriday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnFriday; });
        public static readonly storeFieldTypes ShipOnSaturday = new storeFieldTypes(16, 900016, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnSaturday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnSaturday; });
        public static readonly storeFieldTypes ShipOnSunday = new storeFieldTypes(17, 900017, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.ShipOnSunday, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.ShipOnSunday; });
        public static readonly storeFieldTypes StoreDescription = new storeFieldTypes(18, 900024, new valueInfoTypes(valueTypes.Text), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.StoreDescription, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.StoreDescription; });
        public static readonly storeFieldTypes SimiliarStoreModel = new storeFieldTypes(19, 900352, new valueInfoTypes(valueTypes.Boolean), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToBool(e.prof.SimilarStoreModel, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.SimilarStoreModel; });
        public static readonly storeFieldTypes ImoID = new storeFieldTypes(20, 900805, new valueInfoTypes(valueTypes.Text), delegate(StoreEnhancedProfile e, filterCondition fc) { return filterDataHelper.CompareToString(e.prof.IMO_ID, fc); }, delegate(StoreEnhancedProfile e) { return e.prof.IMO_ID; });

        //Stock Lead Weeks is part of the Store Eligibility Model - which we dont get yet as part of the store enhanced profile
        //public static readonly storeFieldTypes StockLeadWeeks = new storeFieldTypes(21, 900820, new valueInfoTypes(valueTypes.Numeric, numericTypes.Integer), delegate(StoreProfile prof, filterCondition fc) { return CompareToString(prof.StockStatus, fc); });



        private storeFieldTypes(int dbIndex, int textCode, valueInfoTypes valueInfoType, CompareStoreValue compareStoreValue, GetStoreValue getStoreValue)
        {
            string n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
            this.Name = n;
            this.dbIndex = dbIndex;
            this.valueInfoType = valueInfoType;
            this.compareStoreValue = compareStoreValue;
            this.getStoreValue = getStoreValue;
            storeFieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        //public valueTypes valueType { get; private set; }
        //public dateTypes dateType  { get; private set; }
        public valueInfoTypes valueInfoType { get; private set; }
        private CompareStoreValue compareStoreValue;
        private GetStoreValue getStoreValue;
        // public static implicit operator string(storeFieldTypes op) { return op.Name; }
        public static implicit operator int(storeFieldTypes op) { return op.dbIndex; }


        public bool CompareStoreValue(StoreEnhancedProfile prof, filterCondition fc)
        {
            return compareStoreValue(prof, fc);
        }
        public object GetStoreValue(StoreEnhancedProfile prof)
        {
            return getStoreValue(prof);
        }

       

        public static storeFieldTypes FromIndex(int dbIndex)
        {
            storeFieldTypes result = storeFieldTypeList.Find(
               delegate(storeFieldTypes ft)
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
        public static storeFieldTypes FromString(string storeFieldTypeName)
        {
            storeFieldTypes result = storeFieldTypeList.Find(
              delegate(storeFieldTypes ft)
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
                //storeField type was not found in the list
                return null;
            }
        }
        public static DataTable ToDataTable()
        {
            DataTable dt = new DataTable("storeFields");
            dt.Columns.Add("FIELD_NAME");
            dt.Columns.Add("FIELD_INDEX", typeof(int));
            dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            foreach (storeFieldTypes fieldType in storeFieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueInfoType.valueType.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static valueInfoTypes GetValueTypeInfoForField(int fieldIndex)
        {
            storeFieldTypes storeField = storeFieldTypes.FromIndex(fieldIndex);
            return storeField.valueInfoType;
        }
        public static string GetNameFromIndex(int fieldIndex)
        {
            storeFieldTypes field = storeFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }
        public static DataTable GetValueListForStoreFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            //dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            storeFieldTypes storeFieldType = storeFieldTypes.FromIndex(fieldIndex);

            //if (storeFieldType == storeFieldTypes.IntransitStatus)
            //{
            //    foreach (headerFieldInstransitStatusType statusType in headerFieldInstransitStatusType.statusList)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dr["VALUE_INDEX"] = statusType.Index;
            //        dr["VALUE_NAME"] = statusType.Name;
            //        //dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueInfoType.valueType.dbIndex;
            //        dt.Rows.Add(dr);
            //    }
            //}

            return dt;
        }
    }

    public sealed class headerFieldTypes
    {
        public static List<headerFieldTypes> fieldTypeList = new List<headerFieldTypes>();
        public static readonly headerFieldTypes HeaderID = new headerFieldTypes(0, "Header ID", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes MultiHeaderID = new headerFieldTypes(1, "Multi-Header ID", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes SubClass = new headerFieldTypes(2, "Sub-Class", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes Style = new headerFieldTypes(3, "Style", new valueInfoTypes(valueTypes.Text));
        //public static readonly headerFieldTypes Description = new headerFieldTypes(4, "Description", valueTypes.Text);
        //public static readonly headerFieldTypes NumStores = new headerFieldTypes(5, "# of Stores", valueTypes.Numeric);
        public static readonly headerFieldTypes NumPacks = new headerFieldTypes(6, "# Packs", new valueInfoTypes(valueTypes.Numeric, numericTypes.Integer));
        public static readonly headerFieldTypes NumBulkColors = new headerFieldTypes(7, "# Bulk Colors", new valueInfoTypes(valueTypes.Numeric, numericTypes.Integer));
        public static readonly headerFieldTypes NumBulkSizes = new headerFieldTypes(8, "# Bulk Sizes", new valueInfoTypes(valueTypes.Numeric, numericTypes.Integer));

        //public static readonly headerFieldTypes UnitRetail = new headerFieldTypes(9, "Unit Retail", valueTypes.Numeric);
        //public static readonly headerFieldTypes UnitCost = new headerFieldTypes(10, "Unit Cost", valueTypes.Dollar);
        public static readonly headerFieldTypes SizeGroup = new headerFieldTypes(11, "Size Group", new valueInfoTypes(valueTypes.Text));
        //public static readonly headerFieldTypes Multiple = new headerFieldTypes(12, "Multiple", valueTypes.Numeric);
        public static readonly headerFieldTypes PO = new headerFieldTypes(13, "PO", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes Vendor = new headerFieldTypes(14, "Vendor", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes DC = new headerFieldTypes(15, "DC", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes IntransitStatus = new headerFieldTypes(16, "Intransit Status", new valueInfoTypes(valueTypes.List));
        public static readonly headerFieldTypes ShipStatus = new headerFieldTypes(17, "Ship Status", new valueInfoTypes(valueTypes.List));
        public static readonly headerFieldTypes VswID = new headerFieldTypes(18, "VSW", new valueInfoTypes(valueTypes.Text));
        public static readonly headerFieldTypes VswProcess = new headerFieldTypes(19, "VSW Process", new valueInfoTypes(valueTypes.List));




        private headerFieldTypes(int dbIndex, string Name, valueInfoTypes valueInfoType)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            this.valueInfoType = valueInfoType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public valueInfoTypes valueInfoType { get; private set; }
        public static implicit operator int(headerFieldTypes op) { return op.dbIndex; }


        public static headerFieldTypes FromIndex(int dbIndex)
        {
            headerFieldTypes result = fieldTypeList.Find(
               delegate(headerFieldTypes ft)
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
        public static headerFieldTypes FromString(string storeFieldTypeName)
        {
            headerFieldTypes result = fieldTypeList.Find(
              delegate(headerFieldTypes ft)
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

            foreach (headerFieldTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueInfoType.valueType.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static valueInfoTypes GetValueTypeForField(int fieldIndex)
        {
            headerFieldTypes field = headerFieldTypes.FromIndex(fieldIndex);
            return field.valueInfoType;
        }
        public static string GetNameFromIndex(int fieldIndex)
        {
            headerFieldTypes field = headerFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }
        public static DataTable GetValueListForHeaderFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            //dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

            headerFieldTypes headerFieldType = headerFieldTypes.FromIndex(fieldIndex);

            if (headerFieldType == headerFieldTypes.IntransitStatus)
            {
                foreach (headerFieldInstransitStatusType statusType in headerFieldInstransitStatusType.statusList)
                {
                    DataRow dr = dt.NewRow();
                    dr["VALUE_INDEX"] = statusType.Index;
                    dr["VALUE_NAME"] = statusType.Name;
                    //dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueInfoType.valueType.dbIndex;
                    dt.Rows.Add(dr);
                }
            }
            else if (headerFieldType == headerFieldTypes.ShipStatus)
            {
                foreach (headerFieldShipStatusType statusType in headerFieldShipStatusType.statusList)
                {
                    DataRow dr = dt.NewRow();
                    dr["VALUE_INDEX"] = statusType.Index;
                    dr["VALUE_NAME"] = statusType.Name;
                    //dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueInfoType.valueType.dbIndex;
                    dt.Rows.Add(dr);
                }
            }
            else if (headerFieldType == headerFieldTypes.VswProcess)
            {
                foreach (headerFieldVSWProcessStatusType statusType in headerFieldVSWProcessStatusType.statusList)
                {
                    DataRow dr = dt.NewRow();
                    dr["VALUE_INDEX"] = statusType.Index;
                    dr["VALUE_NAME"] = statusType.Name;
                    //dr["FIELD_VALUE_TYPE_INDEX"] = fieldType.valueInfoType.valueType.dbIndex;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }

    public sealed class headerFieldInstransitStatusType
    {
        public static List<headerFieldInstransitStatusType> statusList = new List<headerFieldInstransitStatusType>();
        public static readonly headerFieldInstransitStatusType Style = new headerFieldInstransitStatusType(0, "Style");
        public static readonly headerFieldInstransitStatusType StyleAndSize = new headerFieldInstransitStatusType(1, "Style and Size");
        public static readonly headerFieldInstransitStatusType NotIntransit = new headerFieldInstransitStatusType(2, "Not Intransit");

        private headerFieldInstransitStatusType(int Index, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.Index = Index;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(headerFieldInstransitStatusType op) { return op.Index; }


        public static headerFieldInstransitStatusType FromIndex(int dbIndex)
        {
            headerFieldInstransitStatusType result = statusList.Find(
               delegate(headerFieldInstransitStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static headerFieldInstransitStatusType FromString(string storeFieldTypeName)
        {
            headerFieldInstransitStatusType result = statusList.Find(
              delegate(headerFieldInstransitStatusType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }


    }

    public sealed class headerFieldShipStatusType
    {
        public static List<headerFieldShipStatusType> statusList = new List<headerFieldShipStatusType>();
        public static readonly headerFieldShipStatusType Shipped = new headerFieldShipStatusType(0, "Shipped");
        public static readonly headerFieldShipStatusType NotShipped = new headerFieldShipStatusType(1, "Not Shipped");

        private headerFieldShipStatusType(int Index, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.Index = Index;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(headerFieldShipStatusType op) { return op.Index; }


        public static headerFieldShipStatusType FromIndex(int dbIndex)
        {
            headerFieldShipStatusType result = statusList.Find(
               delegate(headerFieldShipStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static headerFieldShipStatusType FromString(string storeFieldTypeName)
        {
            headerFieldShipStatusType result = statusList.Find(
              delegate(headerFieldShipStatusType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }


    }

    public sealed class headerFieldVSWProcessStatusType
    {
        public static List<headerFieldVSWProcessStatusType> statusList = new List<headerFieldVSWProcessStatusType>();
        public static readonly headerFieldVSWProcessStatusType Replace = new headerFieldVSWProcessStatusType(0, "Replace");
        public static readonly headerFieldVSWProcessStatusType Adjust = new headerFieldVSWProcessStatusType(1, "Adjust");

        private headerFieldVSWProcessStatusType(int Index, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.Index = Index;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(headerFieldVSWProcessStatusType op) { return op.Index; }


        public static headerFieldVSWProcessStatusType FromIndex(int dbIndex)
        {
            headerFieldVSWProcessStatusType result = statusList.Find(
               delegate(headerFieldVSWProcessStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static headerFieldVSWProcessStatusType FromString(string storeFieldTypeName)
        {
            headerFieldVSWProcessStatusType result = statusList.Find(
              delegate(headerFieldVSWProcessStatusType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }


    }

    public sealed class storeStatusTypes
    {
        public static List<storeStatusTypes> storeStatusTypeList = new List<storeStatusTypes>();
        public static readonly storeStatusTypes None = new storeStatusTypes(0, -1, valueTypes.Text);
        public static readonly storeStatusTypes New = new storeStatusTypes(1, 804000, valueTypes.Text);
        public static readonly storeStatusTypes Comp = new storeStatusTypes(2, 804001, valueTypes.Text);
        public static readonly storeStatusTypes NonComp = new storeStatusTypes(3, 804002, valueTypes.Text);
        public static readonly storeStatusTypes Closed = new storeStatusTypes(4, 804003, valueTypes.Text);
        public static readonly storeStatusTypes Preopen = new storeStatusTypes(5, 804004, valueTypes.Text);

        private storeStatusTypes(int dbIndex, int textCode, valueTypes valueType)
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
            this.valueType = valueType;
            storeStatusTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public valueTypes valueType { get; private set; }
        public static implicit operator int(storeStatusTypes op) { return op.dbIndex; }


        public static storeStatusTypes FromIndex(int dbIndex)
        {
            storeStatusTypes result = storeStatusTypeList.Find(
               delegate(storeStatusTypes ft)
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
        public static storeStatusTypes FromString(string storeFieldTypeName)
        {
            storeStatusTypes result = storeStatusTypeList.Find(
              delegate(storeStatusTypes ft)
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

            foreach (storeStatusTypes fieldType in storeStatusTypeList)
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

    public sealed class listValueTypes
    {
        public static List<listValueTypes> listValueTypeList = new List<listValueTypes>();
        public static readonly listValueTypes None = new listValueTypes(0);
        public static readonly listValueTypes StoreStatus = new listValueTypes(1);
        public static readonly listValueTypes HeaderStatus = new listValueTypes(2);
        public static readonly listValueTypes HeaderTypes = new listValueTypes(3);
        public static readonly listValueTypes StoreRID = new listValueTypes(4);
        public static readonly listValueTypes StoreCharacteristicRID = new listValueTypes(5);
        public static readonly listValueTypes HeaderCharacteristicRID = new listValueTypes(6);
        public static readonly listValueTypes StoreField = new listValueTypes(7);
        public static readonly listValueTypes HeaderField = new listValueTypes(8);
        public static readonly listValueTypes StoreGroupLevel = new listValueTypes(9);

        private listValueTypes(int Index)
        {
            this.Index = Index;
            listValueTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(listValueTypes op) { return op.Index; }


        public static listValueTypes FromIndex(int Index)
        {
            listValueTypes result = listValueTypeList.Find(
               delegate(listValueTypes ft)
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

    public sealed class listConstantTypes
    {
        public static List<listConstantTypes> constantList = new List<listConstantTypes>();
        public static readonly listConstantTypes None = new listConstantTypes("None", 0);
        public static readonly listConstantTypes All = new listConstantTypes("All", 1);
        public static readonly listConstantTypes HasValues = new listConstantTypes("Has Values", 2);

        private listConstantTypes(string Name, int dbIndex)
        {
            this.Name = Name;
            this.dbIndex = dbIndex;
            constantList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex;
        public static implicit operator string(listConstantTypes op) { return op.Name; }


        public static listConstantTypes FromIndex(int dbIndex)
        {
            listConstantTypes result = constantList.Find(
               delegate(listConstantTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;

        }
        public static listConstantTypes FromString(string filterTypeName)
        {
            listConstantTypes result = constantList.Find(
              delegate(listConstantTypes ft)
              {
                  return ft.Name == filterTypeName;
              }
              );

            return result;

        }
    }

    /// <summary>
    /// Corresponds to eFilterCubeModifyer
    /// </summary>
    public sealed class variableValueTypes
    {
        public static List<variableValueTypes> fieldTypeList = new List<variableValueTypes>();
        public static readonly variableValueTypes StoreDetail = new variableValueTypes(0, "Store Detail", valueTypes.Text);
        public static readonly variableValueTypes StoreTotal = new variableValueTypes(1, "Store Total", valueTypes.Text);
        public static readonly variableValueTypes StoreAverage = new variableValueTypes(2, "Store Average", valueTypes.Text);
        public static readonly variableValueTypes ChainDetail = new variableValueTypes(3, "Chain Detail", valueTypes.Text);



        private variableValueTypes(int dbIndex, string Name, valueTypes valueType)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            this.valueType = valueType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public valueTypes valueType { get; private set; }
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
        public static valueTypes GetValueTypeForField(int fieldIndex)
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
        public static readonly variableTimeTypes Any = new variableTimeTypes(0, "Any", valueTypes.Text);
        public static readonly variableTimeTypes All = new variableTimeTypes(1, "All", valueTypes.Text);
        public static readonly variableTimeTypes Join = new variableTimeTypes(2, "Join", valueTypes.Text);
        public static readonly variableTimeTypes Average = new variableTimeTypes(3, "Average", valueTypes.Text);
        public static readonly variableTimeTypes Total = new variableTimeTypes(4, "Total", valueTypes.Text);
        public static readonly variableTimeTypes Corresponding = new variableTimeTypes(5, "Corresponding", valueTypes.Text);


        private variableTimeTypes(int dbIndex, string Name, valueTypes valueType)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            this.valueType = valueType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public valueTypes valueType { get; private set; }
        public static implicit operator int(variableTimeTypes op) { return op.dbIndex; }


        public static variableTimeTypes FromIndex(int dbIndex)
        {
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
        public static valueTypes GetValueTypeForField(int fieldIndex)
        {
            variableTimeTypes storeField = variableTimeTypes.FromIndex(fieldIndex);
            return storeField.valueType;
        }
    }


    public sealed class logicTypes
    {
        public static List<logicTypes> fieldTypeList = new List<logicTypes>();
        public static readonly logicTypes And = new logicTypes(0, "And");
        public static readonly logicTypes Or = new logicTypes(1, "Or");

        private logicTypes(int dbIndex, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.Index = dbIndex;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(logicTypes op) { return op.Index; }


        public static logicTypes FromIndex(int dbIndex)
        {
            logicTypes result = fieldTypeList.Find(
               delegate(logicTypes ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static logicTypes FromString(string storeFieldTypeName)
        {
            logicTypes result = fieldTypeList.Find(
              delegate(logicTypes ft)
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

            foreach (logicTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.Index;
                dr["FIELD_NAME"] = fieldType.Name;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }

    public sealed class sortByTypes
    {
        public static List<sortByTypes> sortByTypeList = new List<sortByTypes>();
        public static readonly sortByTypes StoreCharacteristics = new sortByTypes("Store Characteristics", 0, new List<filterTypes>() { filterTypes.StoreFilter, filterTypes.AttributeSetFilter });
        public static readonly sortByTypes HeaderCharacteristics = new sortByTypes("Header Characteristics", 1, new List<filterTypes>() { filterTypes.HeaderFilter });
        public static readonly sortByTypes StoreFields = new sortByTypes("Store Fields", 2, new List<filterTypes>() { filterTypes.StoreFilter, filterTypes.AttributeSetFilter });
        public static readonly sortByTypes HeaderFields = new sortByTypes("Header Fields", 3, new List<filterTypes>() { filterTypes.HeaderFilter });
        public static readonly sortByTypes Variables = new sortByTypes("Variables", 4, new List<filterTypes>() { filterTypes.StoreFilter });
        public static readonly sortByTypes HeaderType = new sortByTypes("Header Type", 5, new List<filterTypes>() { filterTypes.HeaderFilter });
        public static readonly sortByTypes StoreStatus = new sortByTypes("Store Status", 6, new List<filterTypes>() { filterTypes.StoreFilter, filterTypes.AttributeSetFilter });
        public static readonly sortByTypes HeaderStatus = new sortByTypes("Header Status", 7, new List<filterTypes>() { filterTypes.HeaderFilter });
        public static readonly sortByTypes HeaderDate = new sortByTypes("Header Date", 8, new List<filterTypes>() { filterTypes.HeaderFilter });
        public static readonly sortByTypes HeaderReleaseDate = new sortByTypes("Release Date", 9, new List<filterTypes>() { filterTypes.HeaderFilter });

        private sortByTypes(string Name, int dbIndex, List<filterTypes> filterTypeList)
        {
            this.Name = Name;
            this.Index = dbIndex;
            this.filterTypeList = filterTypeList;
            sortByTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }
        public List<filterTypes> filterTypeList { get; private set; }
        public static implicit operator int(sortByTypes op) { return op.Index; }


        public static sortByTypes FromIndex(int dbIndex)
        {
            sortByTypes result = sortByTypeList.Find(
               delegate(sortByTypes ft)
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
        public static sortByTypes FromString(string filterTypeName)
        {
            sortByTypes result = sortByTypeList.Find(
              delegate(sortByTypes ft)
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

            foreach (sortByTypes fieldType in sortByTypeList)
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
        //public static DataTable ToDataTableFromSingleString(string fieldName)
        //{
        //    DataTable dt = new DataTable("fields");
        //    dt.Columns.Add("FIELD_NAME");
        //    dt.Columns.Add("FIELD_INDEX", typeof(int));


        //            DataRow dr = dt.NewRow();
        //            dr["FIELD_INDEX"] = 0;
        //            dr["FIELD_NAME"] = fieldName;
        //            dt.Rows.Add(dr);

        //    return dt;
        //}

    }

    public sealed class sortByDirectionTypes
    {
        public static List<sortByDirectionTypes> fieldTypeList = new List<sortByDirectionTypes>();
        public static readonly sortByDirectionTypes Ascending = new sortByDirectionTypes(0, "Ascending");
        public static readonly sortByDirectionTypes Descending = new sortByDirectionTypes(1, "Descending");

        private sortByDirectionTypes(int dbIndex, string Name)
        {
            //string n = filterData.GetTextFromCode(textCode);
            this.Name = Name;
            this.dbIndex = dbIndex;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }

        public static implicit operator int(sortByDirectionTypes op) { return op.dbIndex; }


        public static sortByDirectionTypes FromIndex(int dbIndex)
        {
            sortByDirectionTypes result = fieldTypeList.Find(
               delegate(sortByDirectionTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;

        }
        public static sortByDirectionTypes FromString(string storeFieldTypeName)
        {
            sortByDirectionTypes result = fieldTypeList.Find(
              delegate(sortByDirectionTypes ft)
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

            foreach (sortByDirectionTypes fieldType in fieldTypeList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = fieldType.dbIndex;
                dr["FIELD_NAME"] = fieldType.Name;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }



    public sealed class numericOperatorTypes
    {
        public static List<numericOperatorTypes> opList = new List<numericOperatorTypes>();
        public static readonly numericOperatorTypes DoesEqual = new numericOperatorTypes(0, "=", "equals");
        public static readonly numericOperatorTypes DoesNotEqual = new numericOperatorTypes(1, "!=", "does not equal");
        public static readonly numericOperatorTypes GreaterThan = new numericOperatorTypes(2, ">", "greater than");
        public static readonly numericOperatorTypes GreaterThanOrEqualTo = new numericOperatorTypes(3, ">=", "greater than or equal to");
        public static readonly numericOperatorTypes LessThan = new numericOperatorTypes(4, "<", "less than");
        public static readonly numericOperatorTypes LessThanOrEqualTo = new numericOperatorTypes(5, "<=", "less than or equal to");
        public static readonly numericOperatorTypes Between = new numericOperatorTypes(6, "bt", "between");

        private numericOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(numericOperatorTypes op) { return op.dbIndex; }


        public static numericOperatorTypes FromIndex(int dbIndex)
        {
            numericOperatorTypes result = opList.Find(
               delegate(numericOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static numericOperatorTypes FromSymbol(string symbol)
        {
            numericOperatorTypes result = opList.Find(
              delegate(numericOperatorTypes ft)
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


            foreach (numericOperatorTypes opType in opList)
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

    public sealed class dateOperatorTypes
    {
        public static List<dateOperatorTypes> opList = new List<dateOperatorTypes>();
        public static readonly dateOperatorTypes Unrestricted = new dateOperatorTypes(0, "u", "Unrestricted");
        public static readonly dateOperatorTypes Last24Hours = new dateOperatorTypes(1, "last24hours", "Last 24 Hours");
        public static readonly dateOperatorTypes Last7Days = new dateOperatorTypes(2, "last7days", "Last 7 days");
        public static readonly dateOperatorTypes Between = new dateOperatorTypes(3, "bt", "Between");
        public static readonly dateOperatorTypes Specify = new dateOperatorTypes(4, "s", "Specify a date range");


        private dateOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(dateOperatorTypes op) { return op.dbIndex; }


        public static dateOperatorTypes FromIndex(int dbIndex)
        {
            dateOperatorTypes result = opList.Find(
               delegate(dateOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static dateOperatorTypes FromSymbol(string symbol)
        {
            dateOperatorTypes result = opList.Find(
              delegate(dateOperatorTypes ft)
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


            foreach (dateOperatorTypes opType in opList)
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

    public sealed class stringOperatorTypes
    {
        public static List<stringOperatorTypes> opList = new List<stringOperatorTypes>();

        public static readonly stringOperatorTypes Equals = new stringOperatorTypes(0, "e", "equals"); //case insensitive
        public static readonly stringOperatorTypes StartsWith = new stringOperatorTypes(1, "sw", "starts with"); //case insensitive
        public static readonly stringOperatorTypes EndsWith = new stringOperatorTypes(2, "ew", "ends with"); //case insensitive
        public static readonly stringOperatorTypes Contains = new stringOperatorTypes(3, "c", "contains"); //case insensitive

        public static readonly stringOperatorTypes EqualsExactly = new stringOperatorTypes(4, "ee", "equals exactly"); //case sensitive
        public static readonly stringOperatorTypes StartsExactlyWith = new stringOperatorTypes(5, "sew", "starts exactly with"); //case sensitive
        public static readonly stringOperatorTypes EndsExactlyWith = new stringOperatorTypes(6, "eew", "ends exactly with"); //case sensitive
        public static readonly stringOperatorTypes ContainsExactly = new stringOperatorTypes(7, "ce", "contains exactly"); //case sensitive


        private stringOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(stringOperatorTypes op) { return op.dbIndex; }


        public static stringOperatorTypes FromIndex(int dbIndex)
        {
            stringOperatorTypes result = opList.Find(
               delegate(stringOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static stringOperatorTypes FromSymbol(string symbol)
        {
            stringOperatorTypes result = opList.Find(
              delegate(stringOperatorTypes ft)
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

            foreach (stringOperatorTypes opType in opList)
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

    public sealed class listOperatorTypes
    {
        public static List<listOperatorTypes> opList = new List<listOperatorTypes>();

        public static readonly listOperatorTypes Includes = new listOperatorTypes(0, "In", "includes");
        public static readonly listOperatorTypes Excludes = new listOperatorTypes(1, "Not In", "excludes");


        private listOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(listOperatorTypes op) { return op.dbIndex; }


        public static listOperatorTypes FromIndex(int dbIndex)
        {
            listOperatorTypes result = opList.Find(
               delegate(listOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static listOperatorTypes FromSymbol(string symbol)
        {
            listOperatorTypes result = opList.Find(
              delegate(listOperatorTypes ft)
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

            foreach (listOperatorTypes opType in opList)
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

    public sealed class percentageOperatorTypes
    {
        public static List<percentageOperatorTypes> opList = new List<percentageOperatorTypes>();

        public static readonly percentageOperatorTypes PercentOf = new percentageOperatorTypes(0, "% of", "percent of");
        public static readonly percentageOperatorTypes PercentChange = new percentageOperatorTypes(1, "% change", "percent change");


        private percentageOperatorTypes(int dbIndex, string symbol, string description)
        {
            this.dbIndex = dbIndex;
            this.symbol = symbol;
            this.description = description;

            opList.Add(this);
        }
        public int dbIndex { get; private set; }
        public string symbol { get; private set; }
        public string description { get; private set; }

        public static implicit operator int(percentageOperatorTypes op) { return op.dbIndex; }


        public static percentageOperatorTypes FromIndex(int dbIndex)
        {
            percentageOperatorTypes result = opList.Find(
               delegate(percentageOperatorTypes ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );

            return result;
        }
        public static percentageOperatorTypes FromSymbol(string symbol)
        {
            percentageOperatorTypes result = opList.Find(
              delegate(percentageOperatorTypes ft)
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

            foreach (percentageOperatorTypes opType in opList)
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
