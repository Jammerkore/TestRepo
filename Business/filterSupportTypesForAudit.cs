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
    public sealed class filterAuditFieldTypes
    {
        public static List<filterAuditFieldTypes> fieldTypeList = new List<filterAuditFieldTypes>();
        public static readonly filterAuditFieldTypes DetailModule = new filterAuditFieldTypes(0, "Detail - Module", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditFieldTypes DetailMessage = new filterAuditFieldTypes(1, "Detail - Message", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditFieldTypes DetailMessageDetails = new filterAuditFieldTypes(2, "Detail - Message Details", new filterDataTypes(filterValueTypes.Text));

        public static readonly filterAuditFieldTypes ProcessDescription = new filterAuditFieldTypes(3, "Process - Description", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditFieldTypes ProcessDuration = new filterAuditFieldTypes(4, "Process - Duration", new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterAuditFieldTypes ProcessName = new filterAuditFieldTypes(5, "Process - Name", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditFieldTypes ProcessStatus = new filterAuditFieldTypes(6, "Process - Status", new filterDataTypes(filterValueTypes.List));
        public static readonly filterAuditFieldTypes ProcessSummary = new filterAuditFieldTypes(7, "Process - Summary", new filterDataTypes(filterValueTypes.List));


        private filterAuditFieldTypes(int dbIndex, string n, filterDataTypes dataType)
        {
            //string n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
            this.Name = n;
            this.dbIndex = dbIndex;
            this.dataType = dataType;
            fieldTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int dbIndex { get; private set; }
        public filterDataTypes dataType { get; private set; }
        public static implicit operator int(filterAuditFieldTypes op) { return op.dbIndex; }


        public static filterAuditFieldTypes FromIndex(int dbIndex)
        {
            filterAuditFieldTypes result = fieldTypeList.Find(
               delegate(filterAuditFieldTypes ft)
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
        public static filterAuditFieldTypes FromString(string storeFieldTypeName)
        {
            filterAuditFieldTypes result = fieldTypeList.Find(
              delegate(filterAuditFieldTypes ft)
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

            foreach (filterAuditFieldTypes fieldType in fieldTypeList)
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
            filterAuditFieldTypes field = filterAuditFieldTypes.FromIndex(fieldIndex);
            return field.dataType;
        }
  


        public static string GetNameFromIndex(int fieldIndex)
        {
            filterAuditFieldTypes field = filterAuditFieldTypes.FromIndex(fieldIndex);
            return field.Name;
        }
        private static DataTable dtProcessStatus = null;
        private static DataTable dtProcessSummary = null;
        public static DataTable GetValueListForAuditFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            try
            {
                filterAuditFieldTypes fieldType = filterAuditFieldTypes.FromIndex(fieldIndex);
                if (fieldType == filterAuditFieldTypes.ProcessStatus)
                {
                    //if (dtProcessStatus == null)
                    //{
                    //    dtProcessStatus = MIDText.GetTextType(eMIDTextType.eProcessCompletionStatus, eMIDTextOrderBy.TextCode);
                    //}
                    //foreach (DataRow drText in dtProcessStatus.Rows)
                    //{
                    //    DataRow dr = dt.NewRow();
                    //    dr["VALUE_INDEX"] = Convert.ToInt32(drText["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    //    dr["VALUE_NAME"] = drText["TEXT_VALUE"].ToString();
                    //    dt.Rows.Add(dr);
                    //}

                    foreach (filterAuditProcessStatusType statusType in filterAuditProcessStatusType.statusList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE_INDEX"] = statusType.Index;
                        dr["VALUE_NAME"] = statusType.Name;
                        dt.Rows.Add(dr);
                    }
                }
                else if (fieldType == filterAuditFieldTypes.ProcessSummary)
                {

                    //if (dtProcessSummary == null)
                    //{
                    //    dtProcessSummary = MIDText.GetTextType(eMIDTextType.eProcessExecutionStatus, eMIDTextOrderBy.TextCode);
                    //}
                    //DataRow drNone = dt.NewRow();
                    //drNone["VALUE_INDEX"] = Include.NoRID;
                    ////drNone["VALUE_NAME"] = "None";
                    //// Begin TT#1378-MD- RMatelic -Add soft text label for Unspecified Size Group >>> also VSW Process
                    ////drNone["VALUE_NAME"] = "Unspecified";
                    //drNone["VALUE_NAME"] = MIDText.GetTextOnly(eMIDTextCode.lbl_Unspecified);
                    //// End TT#1378
                    //dt.Rows.Add(drNone);
                    //foreach (DataRow drText in dtProcessSummary.Rows)
                    //{
                    //    DataRow dr = dt.NewRow();
                    //    dr["VALUE_INDEX"] = Convert.ToInt32(drText["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    //    dr["VALUE_NAME"] = drText["TEXT_VALUE"].ToString();
                    //    dt.Rows.Add(dr);
                    //}

                    foreach (filterAuditProcessSummaryType statusType in filterAuditProcessSummaryType.statusList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE_INDEX"] = statusType.Index;
                        dr["VALUE_NAME"] = statusType.Name;
                        dt.Rows.Add(dr);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return dt;
        }

    }


    public sealed class filterAuditProcessStatusType
    {
        public static List<filterAuditProcessStatusType> statusList = new List<filterAuditProcessStatusType>();

        public static readonly filterAuditProcessStatusType None = new filterAuditProcessStatusType(eProcessCompletionStatus.None);
        public static readonly filterAuditProcessStatusType Successful = new filterAuditProcessStatusType(eProcessCompletionStatus.Successful);
        public static readonly filterAuditProcessStatusType Failed = new filterAuditProcessStatusType(eProcessCompletionStatus.Failed);
        public static readonly filterAuditProcessStatusType ConditionFailed = new filterAuditProcessStatusType(eProcessCompletionStatus.ConditionFailed);
        public static readonly filterAuditProcessStatusType Cancelled = new filterAuditProcessStatusType(eProcessCompletionStatus.Cancelled);
        public static readonly filterAuditProcessStatusType Unexpected = new filterAuditProcessStatusType(eProcessCompletionStatus.Unexpected); 

        private filterAuditProcessStatusType(eProcessCompletionStatus textCode)
        {
            string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = (int)textCode;
            statusList.Add(this);
        }

        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditProcessStatusType op) { return op.Index; }


        public static filterAuditProcessStatusType FromIndex(int dbIndex)
        {
            filterAuditProcessStatusType result = statusList.Find(
               delegate(filterAuditProcessStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditProcessStatusType FromString(string storeFieldTypeName)
        {
            filterAuditProcessStatusType result = statusList.Find(
              delegate(filterAuditProcessStatusType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }


    }

    public sealed class filterAuditProcessSummaryType
    {
        public static List<filterAuditProcessSummaryType> statusList = new List<filterAuditProcessSummaryType>();
        public static readonly filterAuditProcessSummaryType None = new filterAuditProcessSummaryType(eProcessExecutionStatus.None);
        public static readonly filterAuditProcessSummaryType Waiting = new filterAuditProcessSummaryType(eProcessExecutionStatus.Waiting);
        public static readonly filterAuditProcessSummaryType Running = new filterAuditProcessSummaryType(eProcessExecutionStatus.Running);
        public static readonly filterAuditProcessSummaryType OnHold = new filterAuditProcessSummaryType(eProcessExecutionStatus.OnHold);
        public static readonly filterAuditProcessSummaryType Completed = new filterAuditProcessSummaryType(eProcessExecutionStatus.Completed);
        public static readonly filterAuditProcessSummaryType Cancelled = new filterAuditProcessSummaryType(eProcessExecutionStatus.Cancelled);
        public static readonly filterAuditProcessSummaryType Executed = new filterAuditProcessSummaryType(eProcessExecutionStatus.Executed);
        public static readonly filterAuditProcessSummaryType Failed = new filterAuditProcessSummaryType(eProcessExecutionStatus.Failed);
        public static readonly filterAuditProcessSummaryType InError = new filterAuditProcessSummaryType(eProcessExecutionStatus.InError);
        public static readonly filterAuditProcessSummaryType Unexpected = new filterAuditProcessSummaryType(eProcessExecutionStatus.Unexpected);
        public static readonly filterAuditProcessSummaryType Queued = new filterAuditProcessSummaryType(eProcessExecutionStatus.Queued);

        private filterAuditProcessSummaryType(eProcessExecutionStatus textCode)
        {
            string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = (int)textCode;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditProcessSummaryType op) { return op.Index; }


        public static filterAuditProcessSummaryType FromIndex(int dbIndex)
        {
            filterAuditProcessSummaryType result = statusList.Find(
               delegate(filterAuditProcessSummaryType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditProcessSummaryType FromString(string storeFieldTypeName)
        {
            filterAuditProcessSummaryType result = statusList.Find(
              delegate(filterAuditProcessSummaryType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }

    }

    public sealed class filterAuditStatusType
    {
        public static List<filterAuditStatusType> statusList = new List<filterAuditStatusType>();
        public static readonly filterAuditStatusType Running = new filterAuditStatusType(0, "Running");
        public static readonly filterAuditStatusType Completed = new filterAuditStatusType(1, "Completed");


        private filterAuditStatusType(int seq, string n)
        {
            //string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = seq;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditStatusType op) { return op.Index; }


        public static filterAuditStatusType FromIndex(int dbIndex)
        {
            filterAuditStatusType result = statusList.Find(
               delegate(filterAuditStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditStatusType FromString(string storeFieldTypeName)
        {
            filterAuditStatusType result = statusList.Find(
              delegate(filterAuditStatusType ft)
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

            foreach (filterAuditStatusType statusType in statusList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = statusType.Index;
                dr["FIELD_NAME"] = statusType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = filterValueTypes.Text.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

    public sealed class filterAuditMessageLevelType
    {
        public static List<filterAuditMessageLevelType> statusList = new List<filterAuditMessageLevelType>();
        public static readonly filterAuditMessageLevelType Debug = new filterAuditMessageLevelType(0, "Debug");
        public static readonly filterAuditMessageLevelType Information = new filterAuditMessageLevelType(1, "Information");
        public static readonly filterAuditMessageLevelType Edit = new filterAuditMessageLevelType(2, "Edit");
        public static readonly filterAuditMessageLevelType Warning = new filterAuditMessageLevelType(3, "Warning");
        public static readonly filterAuditMessageLevelType Error = new filterAuditMessageLevelType(4, "Error");
        public static readonly filterAuditMessageLevelType Severe = new filterAuditMessageLevelType(5, "Severe");

        private filterAuditMessageLevelType(int seq, string n)
        {
            //string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = seq;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditMessageLevelType op) { return op.Index; }


        public static filterAuditMessageLevelType FromIndex(int dbIndex)
        {
            filterAuditMessageLevelType result = statusList.Find(
               delegate(filterAuditMessageLevelType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditMessageLevelType FromString(string storeFieldTypeName)
        {
            filterAuditMessageLevelType result = statusList.Find(
              delegate(filterAuditMessageLevelType ft)
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

            foreach (filterAuditMessageLevelType statusType in statusList)
            {
                DataRow dr = dt.NewRow();
                dr["FIELD_INDEX"] = statusType.Index;
                dr["FIELD_NAME"] = statusType.Name;
                dr["FIELD_VALUE_TYPE_INDEX"] = filterValueTypes.Text.dbIndex;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
