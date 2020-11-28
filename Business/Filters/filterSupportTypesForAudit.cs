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

        //public static readonly filterAuditFieldTypes ProcessDescription = new filterAuditFieldTypes(3, "Process - Description", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditFieldTypes ProcessDuration = new filterAuditFieldTypes(4, "Process - Duration (in minutes)", new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterAuditFieldTypes ProcessName = new filterAuditFieldTypes(5, "Process - Name", new filterDataTypes(filterValueTypes.List));
        //public static readonly filterAuditFieldTypes ProcessStatus = new filterAuditFieldTypes(6, "Process - Completion Status", new filterDataTypes(filterValueTypes.List));
        //public static readonly filterAuditFieldTypes ProcessStatusExecution = new filterAuditFieldTypes(7, "Process - Execution Status", new filterDataTypes(filterValueTypes.List));
        //public static readonly filterAuditFieldTypes ProcessSummary = new filterAuditFieldTypes(8, "Process - Summary", new filterDataTypes(filterValueTypes.List));


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

        public static DataTable GetValueListForAuditFields(int fieldIndex)
        {
            DataTable dt = new DataTable("values");
            dt.Columns.Add("VALUE_NAME");
            dt.Columns.Add("VALUE_INDEX", typeof(int));
            try
            {
                filterAuditFieldTypes fieldType = filterAuditFieldTypes.FromIndex(fieldIndex);
                if (fieldType == filterAuditFieldTypes.ProcessName)
                {
                    foreach (filterAuditProcessNameType nameType in filterAuditProcessNameType.statusList.OrderBy(x=>x.Name))
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE_INDEX"] = nameType.Index;
                        dr["VALUE_NAME"] = nameType.Name;
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

    public sealed class filterAuditSearchTypes
    {
        public static List<filterAuditSearchTypes> fieldTypeList = new List<filterAuditSearchTypes>();
        //public static readonly filterAuditSearchTypes DetailModule = new filterAuditSearchTypes(0, "Detail - Module", new filterDataTypes(filterValueTypes.Text));
        //public static readonly filterAuditSearchTypes DetailMessage = new filterAuditSearchTypes(1, "Detail - Message", new filterDataTypes(filterValueTypes.Text));
        //public static readonly filterAuditSearchTypes DetailMessageDetails = new filterAuditSearchTypes(2, "Detail - Message Details", new filterDataTypes(filterValueTypes.Text));


        //public static readonly filterAuditSearchTypes Description = new filterAuditSearchTypes(1, "Description", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditSearchTypes Duration = new filterAuditSearchTypes(2, "Process - Duration (in minutes)", new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer));
        public static readonly filterAuditSearchTypes HighestMessageLevel = new filterAuditSearchTypes(4, "Process - Highest Message Level", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditSearchTypes ProcessName = new filterAuditSearchTypes(5, "Process - Name", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditSearchTypes StartTime = new filterAuditSearchTypes(6, "Process - Start Time", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditSearchTypes ExecutionStatus = new filterAuditSearchTypes(7, "Process - Execution Status", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditSearchTypes CompletionStatus = new filterAuditSearchTypes(8, "Process - Completion Status", new filterDataTypes(filterValueTypes.Text));
        public static readonly filterAuditSearchTypes User = new filterAuditSearchTypes(9, "Process - User", new filterDataTypes(filterValueTypes.Text));

        private filterAuditSearchTypes(int dbIndex, string n, filterDataTypes dataType)
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
        public static implicit operator int(filterAuditSearchTypes op) { return op.dbIndex; }


        public static filterAuditSearchTypes FromIndex(int dbIndex)
        {
            filterAuditSearchTypes result = fieldTypeList.Find(
               delegate(filterAuditSearchTypes ft)
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
        public static filterAuditSearchTypes FromString(string storeFieldTypeName)
        {
            filterAuditSearchTypes result = fieldTypeList.Find(
              delegate(filterAuditSearchTypes ft)
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

            foreach (filterAuditSearchTypes fieldType in fieldTypeList)
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
            filterAuditSearchTypes field = filterAuditSearchTypes.FromIndex(fieldIndex);
            return field.dataType;
        }



        public static string GetNameFromIndex(int fieldIndex)
        {
            filterAuditSearchTypes field = filterAuditSearchTypes.FromIndex(fieldIndex);
            return field.Name;
        }



    }
    public sealed class filterAuditProcessCompletionStatusType
    {
        public static List<filterAuditProcessCompletionStatusType> statusList = new List<filterAuditProcessCompletionStatusType>();

        public static readonly filterAuditProcessCompletionStatusType None = new filterAuditProcessCompletionStatusType(eProcessCompletionStatus.None);
        public static readonly filterAuditProcessCompletionStatusType Successful = new filterAuditProcessCompletionStatusType(eProcessCompletionStatus.Successful);
        public static readonly filterAuditProcessCompletionStatusType Failed = new filterAuditProcessCompletionStatusType(eProcessCompletionStatus.Failed);
        public static readonly filterAuditProcessCompletionStatusType ConditionFailed = new filterAuditProcessCompletionStatusType(eProcessCompletionStatus.ConditionFailed);
        public static readonly filterAuditProcessCompletionStatusType Cancelled = new filterAuditProcessCompletionStatusType(eProcessCompletionStatus.Cancelled);
        public static readonly filterAuditProcessCompletionStatusType Unexpected = new filterAuditProcessCompletionStatusType(eProcessCompletionStatus.Unexpected); 

        private filterAuditProcessCompletionStatusType(eProcessCompletionStatus textCode)
        {
            string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = (int)textCode;
            statusList.Add(this);
        }

        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditProcessCompletionStatusType op) { return op.Index; }


        public static filterAuditProcessCompletionStatusType FromIndex(int dbIndex)
        {
            filterAuditProcessCompletionStatusType result = statusList.Find(
               delegate(filterAuditProcessCompletionStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditProcessCompletionStatusType FromString(string storeFieldTypeName)
        {
            filterAuditProcessCompletionStatusType result = statusList.Find(
              delegate(filterAuditProcessCompletionStatusType ft)
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

            foreach (filterAuditProcessCompletionStatusType statusType in statusList.OrderBy(x => x.Name))
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

    public sealed class filterAuditProcessExecutionStatusType
    {
        public static List<filterAuditProcessExecutionStatusType> statusList = new List<filterAuditProcessExecutionStatusType>();
        public static readonly filterAuditProcessExecutionStatusType None = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.None);
        public static readonly filterAuditProcessExecutionStatusType Waiting = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Waiting);
        public static readonly filterAuditProcessExecutionStatusType Running = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Running);
        public static readonly filterAuditProcessExecutionStatusType OnHold = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.OnHold);
        public static readonly filterAuditProcessExecutionStatusType Completed = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Completed);
        public static readonly filterAuditProcessExecutionStatusType Cancelled = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Cancelled);
        public static readonly filterAuditProcessExecutionStatusType Executed = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Executed);
        public static readonly filterAuditProcessExecutionStatusType Failed = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Failed);
        public static readonly filterAuditProcessExecutionStatusType InError = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.InError);
        public static readonly filterAuditProcessExecutionStatusType Unexpected = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Unexpected);
        public static readonly filterAuditProcessExecutionStatusType Queued = new filterAuditProcessExecutionStatusType(eProcessExecutionStatus.Queued);

        private filterAuditProcessExecutionStatusType(eProcessExecutionStatus textCode)
        {
            string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = (int)textCode;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditProcessExecutionStatusType op) { return op.Index; }


        public static filterAuditProcessExecutionStatusType FromIndex(int dbIndex)
        {
            filterAuditProcessExecutionStatusType result = statusList.Find(
               delegate(filterAuditProcessExecutionStatusType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditProcessExecutionStatusType FromString(string storeFieldTypeName)
        {
            filterAuditProcessExecutionStatusType result = statusList.Find(
              delegate(filterAuditProcessExecutionStatusType ft)
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

            foreach (filterAuditProcessExecutionStatusType statusType in statusList.OrderBy(x => x.Name))
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

    public sealed class filterAuditProcessNameType
    {
        public static List<filterAuditProcessNameType> statusList = new List<filterAuditProcessNameType>();
        public static readonly filterAuditProcessNameType unknown = new filterAuditProcessNameType(eProcesses.unknown);
        public static readonly filterAuditProcessNameType hierarchyLoad = new filterAuditProcessNameType(eProcesses.hierarchyLoad);
        public static readonly filterAuditProcessNameType storeLoad = new filterAuditProcessNameType(eProcesses.storeLoad);
        public static readonly filterAuditProcessNameType historyPlanLoad = new filterAuditProcessNameType(eProcesses.historyPlanLoad);
        public static readonly filterAuditProcessNameType clientApplication = new filterAuditProcessNameType(eProcesses.clientApplication);
        public static readonly filterAuditProcessNameType storeGroupBuilder = new filterAuditProcessNameType(eProcesses.storeGroupBuilder);
        public static readonly filterAuditProcessNameType hierarchyWebService = new filterAuditProcessNameType(eProcesses.hierarchyWebService);
        public static readonly filterAuditProcessNameType colorCodeLoad = new filterAuditProcessNameType(eProcesses.colorCodeLoad);
        public static readonly filterAuditProcessNameType sizeCodeLoad = new filterAuditProcessNameType(eProcesses.sizeCodeLoad);
        public static readonly filterAuditProcessNameType headerLoad = new filterAuditProcessNameType(eProcesses.headerLoad);
        public static readonly filterAuditProcessNameType forecasting = new filterAuditProcessNameType(eProcesses.forecasting);
        public static readonly filterAuditProcessNameType rollup = new filterAuditProcessNameType(eProcesses.rollup);
        public static readonly filterAuditProcessNameType controlService = new filterAuditProcessNameType(eProcesses.controlService);
        public static readonly filterAuditProcessNameType applicationService = new filterAuditProcessNameType(eProcesses.applicationService);
        public static readonly filterAuditProcessNameType hierarchyService = new filterAuditProcessNameType(eProcesses.hierarchyService);
        public static readonly filterAuditProcessNameType storeService = new filterAuditProcessNameType(eProcesses.storeService);
        public static readonly filterAuditProcessNameType relieveIntransit = new filterAuditProcessNameType(eProcesses.relieveIntransit);
        public static readonly filterAuditProcessNameType purge = new filterAuditProcessNameType(eProcesses.purge);
        public static readonly filterAuditProcessNameType allocate = new filterAuditProcessNameType(eProcesses.allocate);
        public static readonly filterAuditProcessNameType schedulerService = new filterAuditProcessNameType(eProcesses.schedulerService);
        public static readonly filterAuditProcessNameType executeJob = new filterAuditProcessNameType(eProcesses.executeJob);
        public static readonly filterAuditProcessNameType headerService = new filterAuditProcessNameType(eProcesses.headerService);
        public static readonly filterAuditProcessNameType sizeCurveLoad = new filterAuditProcessNameType(eProcesses.sizeCurveLoad);
        public static readonly filterAuditProcessNameType sqlScript = new filterAuditProcessNameType(eProcesses.sqlScript);
        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
        //public static readonly filterAuditProcessNameType computationsLoad = new filterAuditProcessNameType(eProcesses.computationsLoad);
        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
        public static readonly filterAuditProcessNameType forecastBalancing = new filterAuditProcessNameType(eProcesses.forecastBalancing);
        public static readonly filterAuditProcessNameType databaseConversionUtility = new filterAuditProcessNameType(eProcesses.databaseConversionUtility);
        public static readonly filterAuditProcessNameType reBuildIntransit = new filterAuditProcessNameType(eProcesses.reBuildIntransit);
        public static readonly filterAuditProcessNameType sizeConstraintsLoad = new filterAuditProcessNameType(eProcesses.sizeConstraintsLoad);
        public static readonly filterAuditProcessNameType computationDriver = new filterAuditProcessNameType(eProcesses.computationDriver);
        public static readonly filterAuditProcessNameType specialRequest = new filterAuditProcessNameType(eProcesses.specialRequest);
        public static readonly filterAuditProcessNameType forecastExportThread = new filterAuditProcessNameType(eProcesses.forecastExportThread);
        public static readonly filterAuditProcessNameType sizeCurveGenerate = new filterAuditProcessNameType(eProcesses.sizeCurveGenerate);
        public static readonly filterAuditProcessNameType SizeDayToWeekSummary = new filterAuditProcessNameType(eProcesses.SizeDayToWeekSummary);
        public static readonly filterAuditProcessNameType buildPackCriteriaLoad = new filterAuditProcessNameType(eProcesses.buildPackCriteriaLoad);
        public static readonly filterAuditProcessNameType StoreBinViewer = new filterAuditProcessNameType(eProcesses.StoreBinViewer);
        public static readonly filterAuditProcessNameType generateRelieveIntransit = new filterAuditProcessNameType(eProcesses.generateRelieveIntransit);
        public static readonly filterAuditProcessNameType sizeCurveGenerateThread = new filterAuditProcessNameType(eProcesses.sizeCurveGenerateThread);
        public static readonly filterAuditProcessNameType determineHierarchyActivity = new filterAuditProcessNameType(eProcesses.determineHierarchyActivity);
        public static readonly filterAuditProcessNameType ChainSetPercentCriteriaLoad = new filterAuditProcessNameType(eProcesses.ChainSetPercentCriteriaLoad);
        public static readonly filterAuditProcessNameType hierarchyReclass = new filterAuditProcessNameType(eProcesses.hierarchyReclass);
        public static readonly filterAuditProcessNameType pushToBackStock = new filterAuditProcessNameType(eProcesses.pushToBackStock);
        public static readonly filterAuditProcessNameType headerAllocationLoad = new filterAuditProcessNameType(eProcesses.headerAllocationLoad);
        public static readonly filterAuditProcessNameType DailyPercentagesCriteraLoad = new filterAuditProcessNameType(eProcesses.DailyPercentagesCriteraLoad);
        public static readonly filterAuditProcessNameType scheduleInterface = new filterAuditProcessNameType(eProcesses.scheduleInterface);
        public static readonly filterAuditProcessNameType convertFilters = new filterAuditProcessNameType(eProcesses.convertFilters);
        public static readonly filterAuditProcessNameType storeDelete = new filterAuditProcessNameType(eProcesses.storeDelete);
        public static readonly filterAuditProcessNameType StoreEligibilityCriteraLoad = new filterAuditProcessNameType(eProcesses.StoreEligibilityCriteraLoad);
        public static readonly filterAuditProcessNameType VSWCriteriaLoad = new filterAuditProcessNameType(eProcesses.VSWCriteriaLoad);
        public static readonly filterAuditProcessNameType HeaderReconcile = new filterAuditProcessNameType(eProcesses.HeaderReconcile);
        public static readonly filterAuditProcessNameType BatchComp = new filterAuditProcessNameType(eProcesses.BatchComp); // TT#1595-MD - stodd - batch comp


        private filterAuditProcessNameType(eProcesses textCode)
        {
            string n = MIDText.GetTextFromCode((int)textCode);
            this.Name = n;
            this.Index = (int)textCode;
            statusList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }

        public static implicit operator int(filterAuditProcessNameType op) { return op.Index; }


        public static filterAuditProcessNameType FromIndex(int dbIndex)
        {
            filterAuditProcessNameType result = statusList.Find(
               delegate(filterAuditProcessNameType ft)
               {
                   return ft.Index == dbIndex;
               }
               );

            return result;

        }
        public static filterAuditProcessNameType FromString(string storeFieldTypeName)
        {
            filterAuditProcessNameType result = statusList.Find(
              delegate(filterAuditProcessNameType ft)
              {
                  return ft.Name == storeFieldTypeName;
              }
              );

            return result;

        }



    }


    //public sealed class filterAuditStatusType
    //{
    //    public static List<filterAuditStatusType> statusList = new List<filterAuditStatusType>();
    //    public static readonly filterAuditStatusType Running = new filterAuditStatusType(0, "Running");
    //    public static readonly filterAuditStatusType Completed = new filterAuditStatusType(1, "Completed");


    //    private filterAuditStatusType(int seq, string n)
    //    {
    //        //string n = MIDText.GetTextFromCode((int)textCode);
    //        this.Name = n;
    //        this.Index = seq;
    //        statusList.Add(this);
    //    }
    //    public string Name { get; private set; }
    //    public int Index { get; private set; }

    //    public static implicit operator int(filterAuditStatusType op) { return op.Index; }


    //    public static filterAuditStatusType FromIndex(int dbIndex)
    //    {
    //        filterAuditStatusType result = statusList.Find(
    //           delegate(filterAuditStatusType ft)
    //           {
    //               return ft.Index == dbIndex;
    //           }
    //           );

    //        return result;

    //    }
    //    public static filterAuditStatusType FromString(string storeFieldTypeName)
    //    {
    //        filterAuditStatusType result = statusList.Find(
    //          delegate(filterAuditStatusType ft)
    //          {
    //              return ft.Name == storeFieldTypeName;
    //          }
    //          );

    //        return result;

    //    }
    //    public static DataTable ToDataTable()
    //    {
    //        DataTable dt = new DataTable("fields");
    //        dt.Columns.Add("FIELD_NAME");
    //        dt.Columns.Add("FIELD_INDEX", typeof(int));
    //        dt.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

    //        foreach (filterAuditStatusType statusType in statusList)
    //        {
    //            DataRow dr = dt.NewRow();
    //            dr["FIELD_INDEX"] = statusType.Index;
    //            dr["FIELD_NAME"] = statusType.Name;
    //            dr["FIELD_VALUE_TYPE_INDEX"] = filterValueTypes.Text.dbIndex;
    //            dt.Rows.Add(dr);
    //        }
    //        return dt;
    //    }
    //}

    public sealed class filterAuditMessageLevelType
    {
        public static List<filterAuditMessageLevelType> statusList = new List<filterAuditMessageLevelType>();
        public static readonly filterAuditMessageLevelType UnAssigned = new filterAuditMessageLevelType(0, "UnAssigned");
        public static readonly filterAuditMessageLevelType Debug = new filterAuditMessageLevelType(1, "Debug");
        public static readonly filterAuditMessageLevelType Information = new filterAuditMessageLevelType(2, "Information");
        public static readonly filterAuditMessageLevelType Edit = new filterAuditMessageLevelType(4, "Edit");
        public static readonly filterAuditMessageLevelType Warning = new filterAuditMessageLevelType(5, "Warning");
        public static readonly filterAuditMessageLevelType Error = new filterAuditMessageLevelType(6, "Error");
        public static readonly filterAuditMessageLevelType Severe = new filterAuditMessageLevelType(7, "Severe");

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


    public static class AuditUtility
    {
        #region Audit Summary Info

        public static void AddHierarchyLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.HierarchyLoadAuditInfoAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Hierarchy Records",
																			  Convert.ToInt32(dr["HIER_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Level Records",
																			  Convert.ToInt32(dr["LEVEL_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Product Records",
																			  Convert.ToInt32(dr["MERCH_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Product Records Added",
                                                                              (dr["MERCH_ADDED"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["MERCH_ADDED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Product Records Updated",
                                                                              (dr["MERCH_UPDATED"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["MERCH_UPDATED"],CultureInfo.CurrentUICulture)
																		  });
                    //End TT#106 MD
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Move Records",
																			  (dr["MOVE_RECS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["MOVE_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Rename Records",
																			  (dr["RENAME_RECS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["RENAME_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Delete Records",
																			  (dr["DELETE_RECS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["DELETE_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });

                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }


        //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
        public static void AddHierarchyReclassSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.HierarchyReclassAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Hierarchy Output Records",
																			  Convert.ToInt32(dr["HEIR_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Add Change Output Records",
																			  Convert.ToInt32(dr["ADDCHG_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Deleted Output Records",
																			  Convert.ToInt32(dr["DELETE_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Moved Output Records",
																			  Convert.ToInt32(dr["MOVE_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Rejected Output Records",
																			  Convert.ToInt32(dr["TRANS_REJECTED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
        public static void AddStoreLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.StoreLoadAuditInfoAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Records",
																			  Convert.ToInt32(dr["STORE_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    // Begin MID Track #4668 - add number added and modified
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Stores Added",
																			  (dr["STORES_CREATED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_CREATED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Stores Updated",
																			  (dr["STORES_MODIFIED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_MODIFIED"],CultureInfo.CurrentUICulture)
																		  });
                    // End MID Track #4668
                    // BEGIN TT#739-MD - STodd - delete stores
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Stores Marked for Delete",
																			  (dr["STORES_DELETED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_DELETED"],CultureInfo.CurrentUICulture)
																		  });

                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Stores Recovered",
																			  (dr["STORES_RECOVERED"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dr["STORES_RECOVERED"],CultureInfo.CurrentUICulture)
																		  });
                    // END TT#739-MD - STodd - delete stores
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });

                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        // BEGIN Issue 5117 stodd 4.17.2008
        public static void AddSpecialRequestSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.SpecialRequestAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Jobs in Special Request",
																			  Convert.ToInt32(dr["TOTAL_JOBS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Jobs Processed",
																			  Convert.ToInt32(dr["JOBS_PROCESSED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Successful Jobs",
																			  Convert.ToInt32(dr["SUCCESSFUL_JOBS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Jobs with Errors",
																			  Convert.ToInt32(dr["JOBS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        // END Issue 5117

        public static void AddHistoryPlanLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.PostingAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Chain History Records",
																			  Convert.ToInt32(dr["CH_WK_HIS_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Chain Forecast Records",
																			  Convert.ToInt32(dr["CH_WK_FOR_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Daily History Records",
																			  Convert.ToInt32(dr["ST_DAY_HIS_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Weekly History Records",
																			  Convert.ToInt32(dr["ST_WK_HIS_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Forecast Records",
																			  Convert.ToInt32(dr["ST_WK_FOR_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Intransit Records",
																			  Convert.ToInt32(dr["INTRANSIT_RECS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Nodes Added",
																			  Convert.ToInt32(dr["NODES_ADDED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });

                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        public static void AddHeaderReconcileSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.HeaderReconcileAuditInfo_Read(aProcessRID);

                string filesReadText = MIDText.GetTextOnly(eMIDTextCode.lbl_TransactionFilesRead);
                string recordsReadText = MIDText.GetTextOnly(eMIDTextCode.lbl_RecordsRead);
                string duplicateText = MIDText.GetTextOnly(eMIDTextCode.lbl_DuplicateRecordsFound);
                string skippedText = MIDText.GetTextOnly(eMIDTextCode.lbl_RecordsSkipped);
                string recsWrittenText = MIDText.GetTextOnly(eMIDTextCode.lbl_RecordsWritten);
                string filesWrittenText = MIDText.GetTextOnly(eMIDTextCode.lbl_TransactionFilesWritten);
                string removeRecsText = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoveRecordsWritten);
                string removeFilesText = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoveFilesWritten);  

                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  filesReadText,
																			  Convert.ToInt32(dr["HEADER_FILES_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  recordsReadText,
																			  Convert.ToInt32(dr["HEADER_TRANS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  duplicateText,
																			  Convert.ToInt32(dr["HEADER_TRANS_DUPLICATES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  skippedText,
																			  Convert.ToInt32(dr["HEADER_TRANS_SKIPPED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  recsWrittenText,
																			  Convert.ToInt32(dr["HEADER_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  filesWrittenText,
																			  Convert.ToInt32(dr["HEADER_FILES_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  removeRecsText,
																			  Convert.ToInt32(dr["REMOVE_TRANS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  removeFilesText,
																			  Convert.ToInt32(dr["REMOVE_FILES_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });

                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }


        public static void AddHeaderLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.HeaderLoadAuditInfoAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read",
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Headers Created",
																			  Convert.ToInt32(dr["HDRS_CREATED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Headers Modified",
																			  Convert.ToInt32(dr["HDRS_MODIFIED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Headers Removed",
																			  Convert.ToInt32(dr["HDRS_REMOVED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Headers Reset",
																			  Convert.ToInt32(dr["HDRS_RESET"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        //Begin MOD - JScott - Build Pack Criteria Load
        public static void AddBuildPackCriteriaLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.BuildPackCriteriaLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read",
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Criteria Added/Updated",
																			  Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End MOD - JScott - Build Pack Criteria Load

        //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
        public static void AddChainSetPercentCriteriaLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.ChainSetPercentCriteriaLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read",
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Criteria Added/Updated",
																			  Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2

        //Begin TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        public static void AddStoreEligibilityCriteriaLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.StoreEligibilityCriteriaLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read",
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Criteria Added/Updated",
																			  Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

        //Begin TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
        public static void AddVSWCriteriaLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.VSWCriteriaLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read",
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Criteria Added/Updated",
																			  Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API

        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        public static void AddDailyPercentagesCriteriaLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.DailyPercentagesCriteriaLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read",
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Criteria Added/Updated",
																			  Convert.ToInt32(dr["CRITERIA_ADDED_UPDATED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement


        public static void AddPurgeSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.PurgeAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Daily History Records",
																			  Convert.ToInt32(dr["STORE_DAILY_HISTORY"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Chain Weekly History Records",
																			  Convert.ToInt32(dr["CHAIN_WEEKLY_HISTORY"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Weekly History Records",
																			  Convert.ToInt32(dr["STORE_WEEKLY_HISTORY"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Chain Forecast Records",
																			  Convert.ToInt32(dr["CHAIN_WEEKLY_FORECAST"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Store Forecast Records",
																			  Convert.ToInt32(dr["STORE_WEEKLY_FORECAST"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Header Records",
																			  Convert.ToInt32(dr["HEADERS"],CultureInfo.CurrentUICulture)
																		  });
                    //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Intransit Records",
																			  Convert.ToInt32(dr["INTRANSIT"],CultureInfo.CurrentUICulture)
																		  });
                    //Begin TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Intransit Revision Records",
																			  Convert.ToInt32(dr["INTRANSIT_REV"],CultureInfo.CurrentUICulture)
																		  });
                    //End TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
                    // Begin TT#4352 - JSmith - VSW Review records not getting purged
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "VSW OH Revision Records",
																			  Convert.ToInt32(dr["IMO_REV"],CultureInfo.CurrentUICulture)
																		  });
                    // End TT#4352 - JSmith - VSW Review records not getting purged
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "User Records",
																			  Convert.ToInt32(dr["USERS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Group Records",
																			  Convert.ToInt32(dr["GROUPS"],CultureInfo.CurrentUICulture)
																		  });
                    //End Track #4815
                    // Begin TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Daily Percentages Records",
                                                                              (dr["DAILY_PERCENTAGES"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["DAILY_PERCENTAGES"],CultureInfo.CurrentUICulture)
																		  });
                    // End TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
                    // Begin TT#767 - JSmith - Purge Performance
                    // BEGIN TT#739-MD - STodd - delete stores
                    // Begin RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets
                    //_auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                    //                                                          "Empty Attribute Sets",
                    //                                                          (dr["EMPTY_ATTRIBUTE_SETS"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["EMPTY_ATTRIBUTE_SETS"],CultureInfo.CurrentUICulture)
                    //                                                      });
                    // End RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets
                    // END TT#739-MD - STodd - delete stores
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Audit Records",
																			  Convert.ToInt32(dr["AUDITS"],CultureInfo.CurrentUICulture)
																		  });
                    // Begin TT#767
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        public static void AddColorCodeLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.ColorCodeLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read" ,
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Added",
																			  Convert.ToInt32(dr["CODES_CREATED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Updated",
																			  Convert.ToInt32(dr["CODES_MODIFIED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        public static void AddSizeCodeLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.SizeCodeLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read" ,
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Added",
																			  Convert.ToInt32(dr["CODES_CREATED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Updated",
																			  Convert.ToInt32(dr["CODES_MODIFIED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
        public static void AddSizeCurveLoadSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.SizeCurveLoadAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curves Read" ,
                                                                              Convert.ToInt32(dr["CURVES_READ"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curves Loaded",
                                                                              Convert.ToInt32(dr["CURVES_CREATED"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curves In Error",
                                                                              Convert.ToInt32(dr["CURVES_WITH_ERRORS"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curve Groups Read",
                                                                              Convert.ToInt32(dr["GROUPS_READ"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curve Groups In Error",
                                                                              Convert.ToInt32(dr["GROUPS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curve Groups Created",
                                                                              Convert.ToInt32(dr["GROUPS_CREATED"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curve Groups Modified",
                                                                              Convert.ToInt32(dr["GROUPS_MODIFIED"],CultureInfo.CurrentUICulture)
                                                                          });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
                                                                              "Curve Groups Removed",
                                                                              Convert.ToInt32(dr["GROUPS_REMOVED"],CultureInfo.CurrentUICulture)
                                                                          });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

        public static void AddSizeConstraintsSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.SizeConstraintsLoadAuditInfo_Read(aProcessRID);
                DataRow dr = summary.Rows[0];
                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Records Read" ,
																		  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																	  });
                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Models Added",
																		  Convert.ToInt32(dr["MODELS_CREATED"],CultureInfo.CurrentUICulture)
																	  });
                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Models Updated",
																		  Convert.ToInt32(dr["MODELS_MODIFIED"],CultureInfo.CurrentUICulture)
																	  });

                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Models UpRemoved",
																		  Convert.ToInt32(dr["MODELS_REMOVED"],CultureInfo.CurrentUICulture)
																	  });

                _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Errors",
																		  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																	  });
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        //Begin TT#707 - JScott - Size Curve process needs to multi-thread
        public static void AddSizeCurveGenerateSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.SizeCurveGenerateAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Methods Executed" ,
																		  Convert.ToInt32(dr["MTHDS_EXECUTED"],CultureInfo.CurrentUICulture)
																	  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Methods Successful",
																		  Convert.ToInt32(dr["MTHDS_SUCCESSFUL"],CultureInfo.CurrentUICulture)
																	  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Methods Failed",
																		  Convert.ToInt32(dr["MTHDS_FAILED"],CultureInfo.CurrentUICulture)
																	  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																		  "Methods with No Action Performed",
																		  Convert.ToInt32(dr["MTHDS_NO_ACTION"],CultureInfo.CurrentUICulture)
																	  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        //End TT#707 - JScott - Size Curve process needs to multi-thread
        public static void AddRollupSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.RollupAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Items" ,
																			  Convert.ToInt32(dr["TOTAL_ITEMS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Batch Size",
																			  Convert.ToInt32(dr["BATCH_SIZE"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Batches",
																			  Convert.ToInt32(dr["TOTAL_BATCHES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Concurrent Processes",
																			  Convert.ToInt32(dr["CONCURRENT_PROCESSES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }

        //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
        public static void AddComputationDriverSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.ComputationDriverAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Items" ,
																			  Convert.ToInt32(dr["TOTAL_ITEMS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Concurrent Processes",
																			  Convert.ToInt32(dr["CONCURRENT_PROCESSES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End - Abercrombie & Fitch #4411

        //Begin Track #5100 - JSmith - Add counts to audit
        public static void AddRelieveIntransitSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.RelieveIntransitAuditInfo_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Read" ,
																			  Convert.ToInt32(dr["RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Records Accepted",
																			  Convert.ToInt32(dr["RECS_ACCEPTED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Errors",
																			  Convert.ToInt32(dr["RECS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End Track #5100

        // Begin TT#465 - stodd - sizeDayToweekSummary
        public static void AddSizeDayToWeekSummarySummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.SizeDayToWeekSummary_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Styles Processed",
																			  Convert.ToInt32(dr["TOTAL_STYLES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Colors Processed",
																			  Convert.ToInt32(dr["TOTAL_COLORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Sizes Processed",
																			  Convert.ToInt32(dr["TOTAL_SIZES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Daily Records Read",
																			  Convert.ToInt32(dr["TOTAL_RECS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Daily Values Processed",
																			  Convert.ToInt32(dr["TOTAL_VALUES_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Weekly Records Written",
																			  Convert.ToInt32(dr["TOTAL_RECS_WRITTEN"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Errors",
																			  Convert.ToInt32(dr["TOTAL_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        // End TT#465 - stodd - sizeDayToweekSummary

        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        public static void AddPushToBackStock(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.PushToBackStock_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Headers Read",
																			  Convert.ToInt32(dr["HDRS_READ"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Headers with Errors",
																			  Convert.ToInt32(dr["HDRS_WITH_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Headers Processed",
																			  Convert.ToInt32(dr["HDRS_PROCESSED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Headers Skipped",
																			  Convert.ToInt32(dr["HDRS_SKIPPED"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        // END TT#1401 - stodd - add resevation stores (IMO)

        // Begin TT#710 - JSmith - Generate relieve intransit
        public static void AddGenerateRelieveIntransitSummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.GenerateRelieveIntransitSummary_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Headers to relieve",
																			  Convert.ToInt32(dr["HEADERS_TO_RELIEVE"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Files generated",
																			  Convert.ToInt32(dr["FILES_GENERATED"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Errors",
																			  Convert.ToInt32(dr["TOTAL_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End TT#710

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        public static void AddDetermineHierarchyActivitySummary(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable summary = _auditData.DetermineHierarchyActivitySummary_Read(aProcessRID);
                if (summary.Rows.Count == 1)
                {
                    DataRow dr = summary.Rows[0];
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Nodes",
																			  Convert.ToInt32(dr["TOTAL_NODES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Active Nodes",
																			  Convert.ToInt32(dr["ACTIVE_NODES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Inactive Nodes",
																			  Convert.ToInt32(dr["INACTIVE_NODES"],CultureInfo.CurrentUICulture)
																		  });
                    _auditDataSet.Tables["Summary"].Rows.Add(new object[] { Convert.ToInt32(dr["PROCESS_RID"],CultureInfo.CurrentUICulture),
																			  "Total Errors",
																			  Convert.ToInt32(dr["TOTAL_ERRORS"],CultureInfo.CurrentUICulture)
																		  });
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        //End TT#988

        public static void AddDetail(int aProcessRID, DataSet _auditDataSet)
        {
            try
            {
                AuditData _auditData = new AuditData();
                DataTable ar = _auditData.AuditReport_Read(aProcessRID);
                DateTime time;
                int messageLevel;

                foreach (DataRow dr in ar.Rows)
                {
                    messageLevel = Convert.ToInt32(dr["MessageLevelCode"], CultureInfo.CurrentUICulture);
                    //if (messageLevel < _afp.HighestDetailMessageLevel)
                    //{
                    //    continue;
                    //}

                    string message = string.Empty;
                    //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
                    string reportMessage = string.Empty;
                    string rptMsg = string.Empty;
                    System.Collections.ArrayList myStringArray = new System.Collections.ArrayList();
                    int rptLen = 500;
                    //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
                    int messageCode = -1;
                    if (dr["MessageCode"] != System.DBNull.Value)
                    {
                        messageCode = Convert.ToInt32(dr["MessageCode"], CultureInfo.CurrentUICulture);
                        message = Convert.ToString(dr["MessageCode"], CultureInfo.CurrentUICulture) + ": " + Convert.ToString(dr["Message"], CultureInfo.CurrentUICulture);
                    }
                    time = Convert.ToDateTime(dr["Time"], CultureInfo.CurrentUICulture);
                    //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
                    if (dr["ReportMessage"] != System.DBNull.Value)
                    {
                        rptMsg = Convert.ToString(dr["ReportMessage"], CultureInfo.CurrentUICulture);

                        // Begin TT#2102 - JSmith - Header Load Errors
                        // add dummy string so line displays.
                        if (rptMsg.Length > 0)
                        {
                            // End TT#2102
                            while (rptMsg.Length > 0)
                            {
                                if (rptMsg.Length < 500) rptLen = rptMsg.Length;
                                reportMessage = rptMsg.Substring(0, rptLen);
                                rptMsg = rptMsg.Remove(0, rptLen);
                                reportMessage.Trim();
                                if (myStringArray.Count > 0) reportMessage = "(Continued from previous Message)" + "\r\n" + reportMessage;
                                myStringArray.Add(reportMessage);
                            }
                            // Begin TT#2102 - JSmith - Header Load Errors
                        }
                        // add dummy string so line displays.
                        else
                        {
                            myStringArray.Add(" ");
                        }
                        // End TT#2102
                    }
                    // Begin TT#2102 - JSmith - Header Load Errors
                    // add dummy string so line displays.
                    else
                    {
                        myStringArray.Add(" ");
                    }
                    // End TT#2102

                    foreach (string reportMsg in myStringArray)
                    {
                        _auditDataSet.Tables["Details"].Rows.Add(new object[] {	Convert.ToInt32(dr["ProcessRID"],CultureInfo.CurrentUICulture),
                                                                            time.ToString(Include.AuditDateTimeFormat),
																			Convert.ToString(dr["Module"],CultureInfo.CurrentUICulture),
																			Convert.ToInt32(dr["MessageLevelCode"],CultureInfo.CurrentUICulture),
																			Convert.ToString(dr["MessageLevel"],CultureInfo.CurrentUICulture),
																			messageCode,
																			message,
																			reportMsg
																		  });
                    }
                    //                    _auditDataSet.Tables["Details"].Rows.Add(new object[] {	Convert.ToInt32(dr["ProcessRID"],CultureInfo.CurrentUICulture),
                    ////																			Convert.ToString(dr["Time"],CultureInfo.CurrentUICulture),
                    //                                                                            time.ToString(Include.AuditDateTimeFormat),
                    //                                                                            Convert.ToString(dr["Module"],CultureInfo.CurrentUICulture),
                    //                                                                            Convert.ToInt32(dr["MessageLevelCode"],CultureInfo.CurrentUICulture),
                    //                                                                            Convert.ToString(dr["MessageLevel"],CultureInfo.CurrentUICulture),
                    //                                                                            messageCode,
                    //                                                                            message,
                    //                                                                            Convert.ToString(dr["ReportMessage"],CultureInfo.CurrentUICulture)
                    //                                                                          });
                    //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
                }
            }
            catch (Exception exception)
            {
                throw exception;
                //HandleException(exception);
            }
        }
        #endregion
    }
}
