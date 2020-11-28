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
    public sealed class filterHeaderFieldTypes
    {
        public static List<filterHeaderFieldTypes> fieldTypeList = new List<filterHeaderFieldTypes>();
        public static readonly filterHeaderFieldTypes HeaderID = new filterHeaderFieldTypes(0, (int)eMIDTextCode.lbl_HeaderID, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes QuantityToAllocate = new filterHeaderFieldTypes(20, (int)eMIDTextCode.lbl_Quantity, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        //public static readonly filterHeaderFieldTypes MultiHeaderID = new filterHeaderFieldTypes(1, (int)eMIDTextCode.lbl_MultiHeaderID, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes SubClass = new filterHeaderFieldTypes(2, -2, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes Style = new filterHeaderFieldTypes(3, -1, new filterDataTypes(filterValueTypes.Text));
        //public static readonly headerFieldTypes Description = new headerFieldTypes(4, "Description", valueTypes.Text);
        //public static readonly headerFieldTypes NumStores = new headerFieldTypes(5, "# of Stores", valueTypes.Numeric);
        public static readonly filterHeaderFieldTypes NumPacks = new filterHeaderFieldTypes(6, (int)eMIDTextCode.lbl_NumPacks, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterHeaderFieldTypes NumBulkColors = new filterHeaderFieldTypes(7, (int)eMIDTextCode.lbl_NumBulkColors, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterHeaderFieldTypes NumBulkSizes = new filterHeaderFieldTypes(8, (int)eMIDTextCode.lbl_NumBulkSizes, new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));

        //public static readonly headerFieldTypes UnitRetail = new headerFieldTypes(9, "Unit Retail", valueTypes.Numeric);
        //public static readonly headerFieldTypes UnitCost = new headerFieldTypes(10, "Unit Cost", valueTypes.Dollar);
        public static readonly filterHeaderFieldTypes SizeGroup = new filterHeaderFieldTypes(11, (int)eMIDTextCode.lbl_SizeGroup, new filterDataTypes(filterValueTypes.List));
        //public static readonly headerFieldTypes Multiple = new headerFieldTypes(12, "Multiple", valueTypes.Numeric);
        public static readonly filterHeaderFieldTypes PO = new filterHeaderFieldTypes(13, (int)eMIDTextCode.lbl_PurchaseOrder, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes Vendor = new filterHeaderFieldTypes(14, (int)eMIDTextCode.lbl_Vendor, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes DC = new filterHeaderFieldTypes(15, (int)eMIDTextCode.lbl_DistCenter, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes IntransitStatus = new filterHeaderFieldTypes(16, (int)eMIDTextCode.lbl_Intransit, new filterDataTypes(filterValueTypes.List));
        public static readonly filterHeaderFieldTypes ShipStatus = new filterHeaderFieldTypes(17, (int)eMIDTextCode.lbl_ShipStatus, new filterDataTypes(filterValueTypes.List));
        public static readonly filterHeaderFieldTypes VswID = new filterHeaderFieldTypes(18, (int)eMIDTextCode.lbl_IMO_ID, new filterDataTypes(filterValueTypes.Text));
        public static readonly filterHeaderFieldTypes VswProcess = new filterHeaderFieldTypes(19, (int)eMIDTextCode.lbl_AdjustVSW_OnHand, new filterDataTypes(filterValueTypes.List));
        public static readonly filterHeaderFieldTypes MasterHeaderID = new filterHeaderFieldTypes(21, (int)eMIDTextCode.lbl_MasterSubord, new filterDataTypes(filterValueTypes.Text));   // TT#1966-MD - JSmith - DC Fulfillment



        private filterHeaderFieldTypes(int dbIndex, int textCode, filterDataTypes dataType)
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
        public static implicit operator int(filterHeaderFieldTypes op) { return op.dbIndex; }


        public static filterHeaderFieldTypes FromIndex(int dbIndex)
        {
            filterHeaderFieldTypes result = fieldTypeList.Find(
               delegate(filterHeaderFieldTypes ft)
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
        public static filterHeaderFieldTypes FromString(string storeFieldTypeName)
        {
            filterHeaderFieldTypes result = fieldTypeList.Find(
              delegate(filterHeaderFieldTypes ft)
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

            foreach (filterHeaderFieldTypes fieldType in fieldTypeList.OrderBy(x => x.Name))
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
            filterHeaderFieldTypes field = filterHeaderFieldTypes.FromIndex(fieldIndex);
            return field.dataType;
        }
        public static string GetNameFromIndex(int fieldIndex)
        {
            filterHeaderFieldTypes field = filterHeaderFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }
        private static DataTable dtIntransitStatus = null;
        //private static DataTable dtShipStatus = null;
        private static DataTable dtVswProcess = null;
        public static DataTable GetValueListForHeaderFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            try
            {
                filterHeaderFieldTypes headerFieldType = filterHeaderFieldTypes.FromIndex(fieldIndex);
                if (headerFieldType == filterHeaderFieldTypes.IntransitStatus)
                {
                    if (dtIntransitStatus == null)
                    {
                        dtIntransitStatus = MIDText.GetTextType(eMIDTextType.eHeaderIntransitStatus, eMIDTextOrderBy.TextCode);
                    }
                    foreach (DataRow drText in dtIntransitStatus.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE_INDEX"] = Convert.ToInt32(drText["TEXT_CODE"], CultureInfo.CurrentUICulture);
                        dr["VALUE_NAME"] = drText["TEXT_VALUE"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
                else if (headerFieldType == filterHeaderFieldTypes.ShipStatus)
                {
                    //if (dtShipStatus == null)
                    //{
                    //    dtShipStatus = MIDText.GetTextType(eMIDTextType.eHeaderShipStatus, eMIDTextOrderBy.TextCode);
                    //}
                    //foreach (DataRow drText in dtShipStatus.Rows)
                    //{
                    //    DataRow dr = dt.NewRow();
                    //    dr["VALUE_INDEX"] = Convert.ToInt32(drText["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    //    dr["VALUE_NAME"] = drText["TEXT_VALUE"].ToString();
                    //    dt.Rows.Add(dr);
                    //}
                    foreach (filterHeaderFieldShipStatusType statusType in filterHeaderFieldShipStatusType.statusList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE_INDEX"] = statusType.Index;
                        dr["VALUE_NAME"] = statusType.Name;
                        dt.Rows.Add(dr);
                    }
                }
                else if (headerFieldType == filterHeaderFieldTypes.VswProcess)
                {

                    if (dtVswProcess == null)
                    {
                        dtVswProcess = MIDText.GetTextType(eMIDTextType.eAdjustVSW, eMIDTextOrderBy.TextCode);
                    }
                    DataRow drNone = dt.NewRow();
                    drNone["VALUE_INDEX"] = Include.NoRID;
                    //drNone["VALUE_NAME"] = "None";
                    // Begin TT#1378-MD- RMatelic -Add soft text label for Unspecified Size Group >>> also VSW Process
                    //drNone["VALUE_NAME"] = "Unspecified";
                    drNone["VALUE_NAME"] = MIDText.GetTextOnly(eMIDTextCode.lbl_Unspecified);
                    // End TT#1378
                    dt.Rows.Add(drNone);
                    foreach (DataRow drText in dtVswProcess.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE_INDEX"] = Convert.ToInt32(drText["TEXT_CODE"], CultureInfo.CurrentUICulture);
                        dr["VALUE_NAME"] = drText["TEXT_VALUE"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
                else if (headerFieldType == filterHeaderFieldTypes.SizeGroup)
                {
                    //if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                    //{

                        SizeGroupList sgl = new SizeGroupList(eProfileType.SizeGroup);
                        //sgl.LoadAll(true);
                        sgl.LoadAll(IncludeUndefinedGroup: true, doReadSizeCodeListFromDatabase: false); //TT#1313-MD -jsobek -Header Filters -performance
                        foreach (SizeGroupProfile sgp in sgl.ArrayList)
                        {
                             DataRow dr = dt.NewRow();
                            dr["VALUE_INDEX"] = sgp.Key;
                            dr["VALUE_NAME"] = sgp.SizeGroupName;
                            dt.Rows.Add(dr);
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return dt;
        }

    }


    //public sealed class filterHeaderFieldInstransitStatusType
    //{
    //    public static List<filterHeaderFieldInstransitStatusType> statusList = new List<filterHeaderFieldInstransitStatusType>();

    //    public static readonly filterHeaderFieldInstransitStatusType NotIntransit = new filterHeaderFieldInstransitStatusType(eHeaderIntransitStatus.NotIntransit);
    //    public static readonly filterHeaderFieldInstransitStatusType IntransitBySKU = new filterHeaderFieldInstransitStatusType(eHeaderIntransitStatus.IntransitBySKU); //aka SKU
    //    public static readonly filterHeaderFieldInstransitStatusType IntransitByStyle = new filterHeaderFieldInstransitStatusType(eHeaderIntransitStatus.IntransitByStyle); //aka Style
    //    public static readonly filterHeaderFieldInstransitStatusType IntransitByBulkSize = new filterHeaderFieldInstransitStatusType(eHeaderIntransitStatus.IntransitByBulkSize); //aka Style and Size

    //    private filterHeaderFieldInstransitStatusType(eHeaderIntransitStatus textCode)
    //    {
    //        string n = MIDText.GetTextFromCode((int)textCode);
    //        this.Name = n;
    //        this.Index = (int)textCode;
    //        statusList.Add(this);
    //    }

    //    public string Name { get; private set; }
    //    public int Index { get; private set; }

    //    public static implicit operator int(filterHeaderFieldInstransitStatusType op) { return op.Index; }


    //    public static filterHeaderFieldInstransitStatusType FromIndex(int dbIndex)
    //    {
    //        filterHeaderFieldInstransitStatusType result = statusList.Find(
    //           delegate(filterHeaderFieldInstransitStatusType ft)
    //           {
    //               return ft.Index == dbIndex;
    //           }
    //           );

    //        return result;

    //    }
    //    public static filterHeaderFieldInstransitStatusType FromString(string storeFieldTypeName)
    //    {
    //        filterHeaderFieldInstransitStatusType result = statusList.Find(
    //          delegate(filterHeaderFieldInstransitStatusType ft)
    //          {
    //              return ft.Name == storeFieldTypeName;
    //          }
    //          );

    //        return result;

    //    }


    //}

    public sealed class filterHeaderFieldShipStatusType
    {
        public static List<filterHeaderFieldShipStatusType> statusList = new List<filterHeaderFieldShipStatusType>();
        public static readonly filterHeaderFieldShipStatusType NotShipped = new filterHeaderFieldShipStatusType(eHeaderShipStatus.NotShipped);
        public static readonly filterHeaderFieldShipStatusType Partial = new filterHeaderFieldShipStatusType(eHeaderShipStatus.Partial);
        public static readonly filterHeaderFieldShipStatusType OnHold = new filterHeaderFieldShipStatusType(eHeaderShipStatus.OnHold);
        public static readonly filterHeaderFieldShipStatusType Shipped = new filterHeaderFieldShipStatusType(eHeaderShipStatus.Shipped);
      
        private filterHeaderFieldShipStatusType(eHeaderShipStatus textCode)
        {
            string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = (int)textCode;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterHeaderFieldShipStatusType op) { return op.Index; }


        public static filterHeaderFieldShipStatusType FromIndex(int dbIndex)
        {
            filterHeaderFieldShipStatusType result = statusList.Find(
               delegate(filterHeaderFieldShipStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterHeaderFieldShipStatusType FromString(string storeFieldTypeName)
        {
            filterHeaderFieldShipStatusType result = statusList.Find(
              delegate(filterHeaderFieldShipStatusType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }

    }

    //public sealed class filterHeaderFieldVSWProcessStatusType
    //{
    //    public static List<filterHeaderFieldVSWProcessStatusType> statusList = new List<filterHeaderFieldVSWProcessStatusType>();
    //    public static readonly filterHeaderFieldVSWProcessStatusType Replace = new filterHeaderFieldVSWProcessStatusType(0, "Replace");
    //    public static readonly filterHeaderFieldVSWProcessStatusType Adjust = new filterHeaderFieldVSWProcessStatusType(1, "Adjust");

    //    private filterHeaderFieldVSWProcessStatusType(int Index, string Name)
    //    {
    //        //string n = filterData.GetTextFromCode(textCode);
    //        this.Name = Name;
    //        this.Index = Index;
    //        statusList.Add(this);
    //    }
    //    public string Name { get; private set; }
    //    public int Index { get; private set; }

    //    public static implicit operator int(filterHeaderFieldVSWProcessStatusType op) { return op.Index; }


    //    public static filterHeaderFieldVSWProcessStatusType FromIndex(int dbIndex)
    //    {
    //        filterHeaderFieldVSWProcessStatusType result = statusList.Find(
    //           delegate(filterHeaderFieldVSWProcessStatusType ft)
    //           {
    //               return ft.Index == dbIndex;
    //           }
    //           );

    //        return result;

    //    }
    //    public static filterHeaderFieldVSWProcessStatusType FromString(string storeFieldTypeName)
    //    {
    //        filterHeaderFieldVSWProcessStatusType result = statusList.Find(
    //          delegate(filterHeaderFieldVSWProcessStatusType ft)
    //          {
    //              return ft.Name == storeFieldTypeName;
    //          }
    //          );

    //        return result;

    //    }


    //}
}
