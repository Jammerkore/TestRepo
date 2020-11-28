using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class ReportData : DataLayer
    {
        public ReportData()
            : base()
        {
        }

        //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
        public DataSet AuditAllocation_Report(DataSet ds,
                                              int aNodeRID,
                                              int aPlanLevelRid,
                                              int aUserRid,
                                              int aUserGroupRid,
                                              string aProcessFromDate,
                                              string aProcessToDate,
                                              string aHeaderRIDList) // TT#397 - RMatelic - Allocation Audit Report not selecting headers when drag/drop a style/ color node
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@PLAN_HNRID", aPlanLevelRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_RID", aUserRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_GROUP_RID", aUserGroupRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@PROCESS_FROM_DATE", aProcessFromDate, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@PROCESS_TO_DATE", aProcessToDate, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@HEADER_RID_LIST", aHeaderRIDList, eDbType.Text, eParameterDirection.Input)};     // TT#397

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_HEADER_AUDIT_REPORT", "AllocationAuditDataTable", InParams));

                ds.Tables.Add(StoredProcedures.HEADER_AUDIT_REPORT_READ.Read(_dba,
                                                                      SELECTED_NODE_RID: aNodeRID,
                                                                      PLAN_HNRID: aPlanLevelRid,
                                                                      USER_RID: aUserRid,
                                                                      USER_GROUP_RID: aUserGroupRid,
                                                                      PROCESS_FROM_DATE: aProcessFromDate,
                                                                      PROCESS_TO_DATE: aProcessToDate,
                                                                      HEADER_RID_LIST: aHeaderRIDList
                                                                      ));
                ds.Tables[0].TableName = "AllocationAuditDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet ForecastAuditMerchandise_Report(DataSet ds,
                                                       int aNodeRID,
                                                       int aLowLevelNo,
                                                       int aVersionRid,
                                                       int aUserRid,
                                                       int aUserGroupRid,
                                                       string aTimeRangeBegin,
                                                       string aTimeRangeEnd,
                                                       string aProcessFromDate,
                                                       string aProcessToDate)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@FV_RID", aVersionRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_RID", aUserRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_GROUP_RID", aUserGroupRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_RANGE_BEGIN", aTimeRangeBegin, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_RANGE_END", aTimeRangeEnd, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@PROCESS_FROM_DATE", aProcessFromDate, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@PROCESS_TO_DATE", aProcessToDate, eDbType.VarChar, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_FORECAST_AUDIT_BY_MERCHANDISE", "ForecastAuditMerchandiseDataTable", InParams));
                //Begin TT#1289-MD -jsobek -Report request database login info and errors 
                ds.Tables.Add(StoredProcedures.SP_GET_FORECAST_AUDIT_BY_MERCHANDISE.Read(_dba,
                                                                                SELECTED_NODE_RID: aNodeRID,
                                                                                LOWER_LEVEL: aLowLevelNo,
                                                                                FV_RID: aVersionRid,
                                                                                USER_RID: aUserRid,
                                                                                TIME_RANGE_BEGIN: aTimeRangeBegin,
                                                                                TIME_RANGE_END: aTimeRangeEnd,
                                                                                USER_GROUP_RID: aUserGroupRid,
                                                                                PROCESS_FROM_DATE: aProcessFromDate,
                                                                                PROCESS_TO_DATE: aProcessToDate
                                                                                ));
                //End TT#1289-MD -jsobek -Report request database login info and errors 
                if (ds.Tables.Count > 0) ds.Tables[0].TableName = "ForecastAuditMerchandiseDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet ForecastReportNames_Report(DataSet ds)
        {
            try
            {
                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_FORECAST_REPORT_NAMES", "ForecastReportNamesDataTable"));
                ds.Tables.Add("Modify Sales", "ForecastReportNamesDataTable");
                ds.Tables.Add("OTS Forecast", "ForecastReportNamesDataTable");
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet ForecastAuditOTSForecast_Report(DataSet ds,
                                                       int aNodeRID,
                                                       int aLowLevelNo,
                                                       int aVersionRid,
                                                       int aUserRid,
                                                       int aUserGroupRid,
                                                       string aTimeRangeBegin,
                                                       string aTimeRangeEnd)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@FV_RID", aVersionRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_RID", aUserRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_GROUP_RID", aUserGroupRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_RANGE_BEGIN", aTimeRangeBegin, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_RANGE_END", aTimeRangeEnd, eDbType.VarChar, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT", "ForecastAuditOTSForecastDataTable", InParams));

                ds.Tables.Add(StoredProcedures.SP_GET_FORECAST_AUDIT_OTSFORECAST_REPORT.Read(_dba,
                                                                                    SELECTED_NODE_RID: aNodeRID,
                                                                                    LOWER_LEVEL: aLowLevelNo,
                                                                                    FV_RID: aVersionRid,
                                                                                    USER_RID: aUserRid,
                                                                                    TIME_RANGE_BEGIN: aTimeRangeBegin,
                                                                                    TIME_RANGE_END: aTimeRangeEnd,
                                                                                    USER_GROUP_RID: aUserGroupRid
                                                                                    ));
                ds.Tables[0].TableName = "ForecastAuditOTSForecastDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet ForecastAuditModifySales_Report(DataSet ds,
                                                       int aNodeRID,
                                                       int aLowLevelNo,
                                                       int aVersionRid,
                                                       int aUserRid,
                                                       int aUserGroupRid,
                                                       string aTimeRangeBegin,
                                                       string aTimeRangeEnd)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@FV_RID", aVersionRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_RID", aUserRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_GROUP_RID", aUserGroupRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_RANGE_BEGIN", aTimeRangeBegin, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_RANGE_END", aTimeRangeEnd, eDbType.VarChar, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_FORECAST_AUDIT_MODIFYSALES_REPORT", "ForecastAuditModifySalesDataTable", InParams));
                ds.Tables.Add(StoredProcedures.FORECAST_AUDIT_MODIFYSALES_REPORT_READ.Read(_dba,
                                                                                    SELECTED_NODE_RID: aNodeRID,
                                                                                    LOWER_LEVEL: aLowLevelNo,
                                                                                    FV_RID: aVersionRid,
                                                                                    USER_RID: aUserRid,
                                                                                    TIME_RANGE_BEGIN: aTimeRangeBegin,
                                                                                    TIME_RANGE_END: aTimeRangeEnd,
                                                                                    USER_GROUP_RID: aUserGroupRid
                                                                                    ));
                ds.Tables[0].TableName = "ForecastAuditModifySalesDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet GetReports_Report(DataSet ds,
                                         int aShowEligibility,
                                         int aShowModifiers,
                                         int aShowSimilarStore,
                                         int aShowStoreGrades,
                                         int aShowAllocationMinMax,
                                         int aShowVelocityGrades,
                                         int aShowCapacity,
                                         int aShowDailypercentages,
                                         int aShowPurgeCriteria,
                                         int aShowForecastLevel,
                                         int aShowForecastType,
                                         int aShowStockMinMax,
            //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                                         int aShowCharacteristics,
                                         int aShowChainSetPercent,
                                         int aShowVSW,
                                         int aShowSizeCurveCriteria,
                                         int aShowSizeCurveTolerance,
                                         int aShowSizeCurveSimilarStores
                                            )
        //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@ShowEligibility", aShowEligibility, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowModifiers", aShowModifiers, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowSimilarStore", aShowSimilarStore, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowStoreGrades", aShowStoreGrades, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowAllocationMinMax", aShowAllocationMinMax, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowVelocityGrades", aShowVelocityGrades, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowCapacity", aShowCapacity, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowDailypercentages", aShowDailypercentages, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowPurgeCriteria", aShowPurgeCriteria, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowForecastLevel", aShowForecastLevel, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowForecastType", aShowForecastType, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowStockMinMax", aShowStockMinMax, eDbType.Int, eParameterDirection.Input),
                //                            //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                //                            new MIDDbParameter("@ShowCharacteristics", aShowCharacteristics, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@ShowChainSetPercent", aShowChainSetPercent, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@ShowVSW", aShowVSW, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@ShowSizeCurvesCriteria", aShowSizeCurveCriteria, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@ShowSizeCurvesTolerance", aShowSizeCurveTolerance, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@ShowSizeCurvesSimilarStores", aShowSizeCurveSimilarStores, eDbType.Int, eParameterDirection.Input)
                //                            //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                //                            };

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_REPORTS", "GetReportsDataTable", InParams));
                ds.Tables.Add(StoredProcedures.SP_GET_REPORTS.Read(_dba,
                                                                  ShowEligibility: aShowEligibility,
                                                                  ShowModifiers: aShowModifiers,
                                                                  ShowSimilarStore: aShowSimilarStore,
                                                                  ShowStoreGrades: aShowStoreGrades,
                                                                  ShowAllocationMinMax: aShowAllocationMinMax,
                                                                  ShowVelocityGrades: aShowVelocityGrades,
                                                                  ShowCapacity: aShowCapacity,
                                                                  ShowDailypercentages: aShowDailypercentages,
                                                                  ShowPurgeCriteria: aShowPurgeCriteria,
                                                                  ShowForecastLevel: aShowForecastLevel,
                                                                  ShowForecastType: aShowForecastType,
                                                                  ShowStockMinMax: aShowStockMinMax,
                                                                  ShowCharacteristics: aShowCharacteristics,
                                                                  ShowChainSetPercent: aShowChainSetPercent,
                                                                  ShowVSW: aShowVSW,
                                                                  ShowSizeCurvesCriteria: aShowSizeCurveCriteria,
                                                                  ShowSizeCurvesTolerance: aShowSizeCurveTolerance,
                                                                  ShowSizeCurvesSimilarStores: aShowSizeCurveSimilarStores
                                                                  ));
                ds.Tables[0].TableName = "GetReportsDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet DailyPercentages_Report(DataSet ds,
                                               int aNodeRID,
                                               int aLowLevelNo,
                                               string aStoreID,
            //string aStoreCharGroup,   // Begin TT#265 - RMAtelic -Eligibility Report not showing low level nodes and formatting problems  
            //string aStoreChar)       
                                                string aStoreRIDList)       // End TT#265
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_ID", aStoreID, eDbType.VarChar, eParameterDirection.Input),
                //                              // Begin TT#265-Eligibility Report not showing low level nodes and formatting problems - changed parm names
                //                              //new MIDDbParameter("@STORE_CHAR_GROUP", aStoreCharGroup, eDbType.VarChar, eParameterDirection.Input),
                //                              //new MIDDbParameter("@STORE_CHAR", aStoreChar, eDbType.VarChar, eParameterDirection.Input)};
                //                               new MIDDbParameter("@STORE_RID_LIST", aStoreRIDList, eDbType.Text, eParameterDirection.Input)};
                //                              // End TT#265 

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_DAILYPERCENTAGES_REPORT", "DailyPercentagesDataTable", InParams));
                ds.Tables.Add(StoredProcedures.DAILYPERCENTAGES_REPORT_READ.Read(_dba,
                                                                          SELECTED_NODE_RID: aNodeRID,
                                                                          LOWER_LEVEL: aLowLevelNo,
                                                                          STORE_ID: aStoreID,
                                                                          STORE_RID_LIST: aStoreRIDList
                                                                          ));
                ds.Tables[0].TableName = "DailyPercentagesDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet ForecastLevel_Report(DataSet ds,
                                            int aNodeRID,
                                            int aLowLevelNo,
                                            int aForecastLevel)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowForecastLevel", aForecastLevel, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_FORECAST_LEVEL_REPORT", "ForecastLevelDataTable", InParams));
                ds.Tables.Add(StoredProcedures.SP_GET_FORECAST_LEVEL_REPORT.Read(_dba,
                                                                                SELECTED_NODE_RID: aNodeRID,
                                                                                LOWER_LEVEL: aLowLevelNo,
                                                                                ShowForecastLevel: aForecastLevel
                                                                                ));
                ds.Tables[0].TableName = "ForecastLevelDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet ForecastType_Report(DataSet ds,
                                           int aNodeRID,
                                           int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_FORECAST_TYPE_REPORT", "ForecastTypeDataTable", InParams));
                ds.Tables.Add(StoredProcedures.SP_GET_FORECAST_TYPE_REPORT.Read(_dba,
                                                                               SELECTED_NODE_RID: aNodeRID,
                                                                               LOWER_LEVEL: aLowLevelNo
                                                                               ));
                ds.Tables[0].TableName = "ForecastTypeDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet PurgeDates_Report(DataSet ds,
                                         int aNodeRID,
                                         int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_PURGE_DATES_REPORT", "PurgeDatesDataTable", InParams));
                ds.Tables.Add(StoredProcedures.PURGE_DATES_REPORT_READ.Read(_dba,
                                                                     SELECTED_NODE_RID: aNodeRID,
                                                                     LOWER_LEVEL: aLowLevelNo
                                                                     ));
                ds.Tables[0].TableName = "PurgeDatesDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet StoreCapacity_Report(DataSet ds,
                                            int aNodeRID,
                                            int aLowLevelNo,
                                            string aStoreID,
            //string aStoreCharGroup,    // Begin TT#265 - RMatelic - Eligibility Report not showing low level nodes and formatting problems  
            //string aStoreChar)       
                                             string aStoreRIDList)       // End TT#265
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_ID", aStoreID, eDbType.VarChar, eParameterDirection.Input),
                //                              // Begin TT#265-Eligibility Report not showing low level nodes and formatting problems - changed parm names
                //                              //new MIDDbParameter("@STORE_CHAR_GROUP", aStoreCharGroup, eDbType.VarChar, eParameterDirection.Input),
                //                              //new MIDDbParameter("@STORE_CHAR", aStoreChar, eDbType.VarChar, eParameterDirection.Input)};
                //                              new MIDDbParameter("@STORE_RID_LIST", aStoreRIDList, eDbType.Text, eParameterDirection.Input)};
                //                              // End TT#265

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_STORE_CAPACITY_REPORT", "StoreCapacityDataTable", InParams));
                ds.Tables.Add(StoredProcedures.STORE_CAPACITY_REPORT_READ.Read(_dba,
                                                                        SELECTED_NODE_RID: aNodeRID,
                                                                        LOWER_LEVEL: aLowLevelNo,
                                                                        STORE_ID: aStoreID,
                                                                        STORE_RID_LIST: aStoreRIDList
                                                                        ));
                ds.Tables[0].TableName = "StoreCapacityDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet StoreEligibility_Report(DataSet ds,
                                               int aNodeRID,
                                               int aLowLevelNo,
                                               string aStoreID,
            //string aStoreGroup,              // Begin TT#265 - Store Eligibility issues - changed parm names 
            //string aStoreSet,                
                                               string aStoreRIDList,              // End TT#265
                                               int aShowEligibility,
                                               int aShowModifiers,
                                               int aShowSimilarStore)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_ID", aStoreID, eDbType.VarChar, eParameterDirection.Input),
                //                              // Begin TT#265 - Store Eligibility issues - changed parm names 
                //                              //new MIDDbParameter("@STORE_GROUP", aStoreGroup, eDbType.VarChar, eParameterDirection.Input),
                //                              //new MIDDbParameter("@STORE_SET", aStoreSet, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_RID_LIST", aStoreRIDList, eDbType.Text, eParameterDirection.Input),
                //                              // End TT#265
                //                              new MIDDbParameter("@ShowEligibility", aShowEligibility, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowModifiers", aShowModifiers, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowSimilarStore", aShowSimilarStore, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_STORE_ELIGIBILITY_REPORT", "StoreEligibilityDataTable", InParams));
                ds.Tables.Add(StoredProcedures.SP_GET_STORE_ELIGIBILITY_REPORT.Read(_dba,
                                                                           SELECTED_NODE_RID: aNodeRID,
                                                                           LOWER_LEVEL: aLowLevelNo,
                                                                           STORE_ID: aStoreID,
                                                                           STORE_RID_LIST: aStoreRIDList,
                                                                           ShowEligibility: aShowEligibility,
                                                                           ShowModifiers: aShowModifiers,
                                                                           ShowSimilarStore: aShowSimilarStore
                                                                           ));
                ds.Tables[0].TableName = "StoreEligibilityDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet StoreGrades_Report(DataSet ds,
                                          int aNodeRID,
                                          int aLowLevelNo,
                                          int aShowStoreGrades,
                                          int aShowAllocationMinMax)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowStoreGrades", aShowStoreGrades, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ShowAllocationMinMax", aShowAllocationMinMax, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_STORE_GRADES_REPORT", "StoreGradesDataTable", InParams));
                ds.Tables.Add(StoredProcedures.SP_GET_STORE_GRADES_REPORT.Read(_dba,
                                                                               SELECTED_NODE_RID: aNodeRID,
                                                                               LOWER_LEVEL: aLowLevelNo,
                                                                               ShowStoreGrades: aShowStoreGrades,
                                                                               ShowAllocationMinMax: aShowAllocationMinMax
                                                                               ));
                ds.Tables[0].TableName = "StoreGradesDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet VelocityGrades_Report(DataSet ds,
                                             int aNodeRID,
                                             int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_VELOCITYGRADES_REPORT", "VelocityGradesDataTable", InParams));
                ds.Tables.Add(StoredProcedures.VELOCITYGRADES_REPORT_READ.Read(_dba,
                                                                        SELECTED_NODE_RID: aNodeRID,
                                                                        LOWER_LEVEL: aLowLevelNo
                                                                        ));
                ds.Tables[0].TableName = "VelocityGradesDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet StockMinMax_Report(DataSet ds,
                                          int aNodeRID,
                                          int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_STOCK_MIN_MAX_REPORT", "StockMinMaxDataTable", InParams));
                ds.Tables.Add(StoredProcedures.STOCK_MIN_MAX_REPORT_READ.Read(_dba,
                                                                       SELECTED_NODE_RID: aNodeRID,
                                                                       LOWER_LEVEL: aLowLevelNo
                                                                       ));
                ds.Tables[0].TableName = "StockMinMaxDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End Track #6232 - KJohnson

        //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
        public DataSet Characteristic_Report(DataSet ds,
                                         int aNodeRID,
                                         int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_CHARACTERISTICS_REPORT", "CharacteristicDataTable", InParams));
                ds.Tables.Add(StoredProcedures.CHARACTERISTICS_REPORT_READ.Read(_dba,
                                                                         SELECTED_NODE_RID: aNodeRID,
                                                                         LOWER_LEVEL: aLowLevelNo
                                                                         ));
                ds.Tables[0].TableName = "CharacteristicDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet ChainSetPercent_Report(DataSet ds,
                                              int aNodeRID,
                                              int aLowLevelNo,
                                              string storeID,
                                              string storeRIDList,
                                              int beginWeekKey,
                                              int endWeekKey
                                             )
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_ID", storeID, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_RID_LIST", storeRIDList, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@BEG_WEEK", beginWeekKey, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@END_WEEK", endWeekKey, eDbType.Int, eParameterDirection.Input),
                //                            };

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_CHAIN_SET_PERCENT_REPORT", "ChainSetPercentagesDataTable", InParams));
                ds.Tables.Add(StoredProcedures.CHAIN_SET_PERCENT_REPORT_READ.Read(_dba,
                                                                           SELECTED_NODE_RID: aNodeRID,
                                                                           LOWER_LEVEL: aLowLevelNo,
                                                                           STORE_ID: storeID,
                                                                           STORE_RID_LIST: storeRIDList,
                                                                           BEG_WEEK: beginWeekKey,
                                                                           END_WEEK: endWeekKey
                                                                           ));
                ds.Tables[0].TableName = "ChainSetPercentagesDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet VSW_Report(DataSet ds,
                                        int aNodeRID,
                                        int aLowLevelNo,
                                        string storeID,
                                        string storeRIDList
                                        )
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_ID", storeID, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@STORE_RID_LIST", storeRIDList, eDbType.VarChar, eParameterDirection.Input)
                //                            };

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_VSW_REPORT", "VSWDataTable", InParams));
                //Begin TT#4041 -jsobek -Node Properties Override report doesn't show decimals or FWOS Max Models
                //ds.Tables.Add(StoredProcedures.SP_GET_VSW_REPORT.Read(_dba,
                //                                                     SELECTED_NODE_RID: aNodeRID,
                //                                                     LOWER_LEVEL: aLowLevelNo,
                //                                                     STORE_ID: storeID,
                //                                                     STORE_RID_LIST: storeRIDList
                //                                                     ));
                ds.Tables.Add(StoredProcedures.MID_REPORT_READ_VSW_OVERRIDE.Read(_dba,
                                                                     SELECTED_NODE_RID: aNodeRID,
                                                                     LOWER_LEVEL: aLowLevelNo,
                                                                     STORE_ID: storeID,
                                                                     STORE_RID_LIST: storeRIDList
                                                                     ));
                //End TT#4041 -jsobek -Node Properties Override report doesn't show decimals or FWOS Max Models
                ds.Tables[0].TableName = "VSWDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet SizeCurveCriteria_Report(DataSet ds, int aNodeRID, int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_SIZE_CURVE_CRITERIA_REPORT", "SizeCurveCriteriaDataTable", InParams));
                ds.Tables.Add(StoredProcedures.SP_GET_SIZE_CURVE_CRITERIA_REPORT.Read(_dba,
                                                                                     SELECTED_NODE_RID: aNodeRID,
                                                                                     LOWER_LEVEL: aLowLevelNo
                                                                                     ));
                ds.Tables[0].TableName = "SizeCurveCriteriaDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet SizeCurveTolerance_Report(DataSet ds, int aNodeRID, int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_SIZE_CURVE_TOLERANCE_REPORT", "SizeCurveToleranceDataTable", InParams));

                ds.Tables.Add(StoredProcedures.SP_GET_SIZE_CURVE_TOLERANCE_REPORT.Read(_dba,
                                                                                      SELECTED_NODE_RID: aNodeRID,
                                                                                      LOWER_LEVEL: aLowLevelNo
                                                                                      ));
                ds.Tables[0].TableName = "SizeCurveToleranceDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataSet SizeCurveSimilarStores_Report(DataSet ds,
                                        int aNodeRID,
                                        int aLowLevelNo)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_NODE_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@LOWER_LEVEL", aLowLevelNo, eDbType.Int, eParameterDirection.Input)};

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_SIZE_CURVE_SIMILAR_STORES_REPORT", "SizeCurveSimilarStoresDataTable", InParams));

                ds.Tables.Add(StoredProcedures.SIZE_CURVE_SIMILAR_STORES_REPORT_READ.Read(_dba,
                                                                                   SELECTED_NODE_RID: aNodeRID,
                                                                                   LOWER_LEVEL: aLowLevelNo
                                                                                   ));
                ds.Tables[0].TableName = "SizeCurveSimilarStoresDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
        //BEGIN TT#554-MD -jsobek -User Log Level Report

        public DataSet UserOptionsReview_Report(DataSet ds, int selectedAuditLoggingLevel, string selectedForecastMonitor, string seletedSalesMonitor, string seletedDCFulfillmentMonitor)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SELECTED_AUDIT_LOGGING_LEVEL", selectedAuditLoggingLevel, eDbType.Int, eParameterDirection.Input),
                //                                new MIDDbParameter("@SELECTED_FORECAST_MONITOR", selectedForecastMonitor, eDbType.Char, eParameterDirection.Input),
                //                                new MIDDbParameter("@SELECTED_SALES_MONITOR", seletedSalesMonitor, eDbType.Char, eParameterDirection.Input)
                //                             };


                //ds.Tables.Add(_dba.ExecuteQuery("dbo.SP_GET_USER_OPTIONS_REVIEW_REPORT", "UserOptionsReviewDataTable", InParams));

                ds.Tables.Add(StoredProcedures.SP_GET_USER_OPTIONS_REVIEW_REPORT.Read(_dba,
                                                                                     SELECTED_AUDIT_LOGGING_LEVEL: selectedAuditLoggingLevel,
                                                                                     SELECTED_FORECAST_MONITOR: Convert.ToChar(selectedForecastMonitor),
                                                                                     SELECTED_SALES_MONITOR: Convert.ToChar(seletedSalesMonitor),
                                                                                     SELECTED_DCFULFILLMENT_MONITOR: Convert.ToChar(seletedDCFulfillmentMonitor)
                                                                                     ));
                ds.Tables[0].TableName = "UserOptionsReviewDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#554-MD -jsobek -User Log Level Report


        //BEGIN TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
        public class AllocationByStoreEventArgs
        {
            public AllocationByStoreEventArgs(
                string storeIDandName,
                int storeRID,
                bool includeHeaderType_ASN,
                bool includeHeaderType_Assortment,
                bool includeHeaderType_DropShip,
                bool includeHeaderType_Dummy,
                bool includeHeaderType_IMO,
                bool includeHeaderType_MultiHeader,
                bool includeHeaderType_Placeholder,
                bool includeHeaderType_PurchaseOrder,
                bool includeHeaderType_Receipt,
                bool includeHeaderType_Reserve,
                bool includeHeaderType_WorkupTotalBuy,
                bool includeHeaderType_Master,    // TT#1966-MD - JSmith - DC Fulfillment
                bool includeHeaderStatus_ReceivedOutOfBalance,
                bool includeHeaderStatus_ReceivedInBalance,
                bool includeHeaderStatus_InUseByMultiHeader,
                bool includeHeaderStatus_PartialSizeOutOfBalance,
                bool includeHeaderStatus_PartialSizeInBalance,
                bool includeHeaderStatus_AllocatedOutOfBalance,
                bool includeHeaderStatus_AllocatedInBalance,
                bool includeHeaderStatus_SizesOutOfBalance,
                bool includeHeaderStatus_AllInBalance,
                bool includeHeaderStatus_Released,
                bool includeHeaderStatus_ReleaseApproved,
                bool includeHeaderStatus_AllocationStarted
                                      )
            {
                this.storeIDandName = storeIDandName;
                this.storeRID = storeRID;
                this.includeHeaderType_ASN = includeHeaderType_ASN;
                this.includeHeaderType_Assortment = includeHeaderType_Assortment;
                this.includeHeaderType_DropShip = includeHeaderType_DropShip;
                this.includeHeaderType_Dummy = includeHeaderType_Dummy;
                this.includeHeaderType_IMO = includeHeaderType_IMO;
                this.includeHeaderType_MultiHeader = includeHeaderType_MultiHeader;
                this.includeHeaderType_Placeholder = includeHeaderType_Placeholder;
                this.includeHeaderType_PurchaseOrder = includeHeaderType_PurchaseOrder;
                this.includeHeaderType_Receipt = includeHeaderType_Receipt;
                this.includeHeaderType_Reserve = includeHeaderType_Reserve;
                this.includeHeaderType_WorkupTotalBuy = includeHeaderType_WorkupTotalBuy;
                this.includeHeaderType_Master = includeHeaderType_Master;    // TT#1966-MD - JSmith - DC Fulfillment
                this.includeHeaderStatus_ReceivedOutOfBalance = includeHeaderStatus_ReceivedOutOfBalance;
                this.includeHeaderStatus_ReceivedInBalance = includeHeaderStatus_ReceivedInBalance;
                this.includeHeaderStatus_InUseByMultiHeader = includeHeaderStatus_InUseByMultiHeader;
                this.includeHeaderStatus_PartialSizeOutOfBalance = includeHeaderStatus_PartialSizeOutOfBalance;
                this.includeHeaderStatus_PartialSizeInBalance = includeHeaderStatus_PartialSizeInBalance;
                this.includeHeaderStatus_AllocatedOutOfBalance = includeHeaderStatus_AllocatedOutOfBalance;
                this.includeHeaderStatus_AllocatedInBalance = includeHeaderStatus_AllocatedInBalance;
                this.includeHeaderStatus_SizesOutOfBalance = includeHeaderStatus_SizesOutOfBalance;
                this.includeHeaderStatus_AllInBalance = includeHeaderStatus_AllInBalance;
                this.includeHeaderStatus_Released = includeHeaderStatus_Released;
                this.includeHeaderStatus_ReleaseApproved = includeHeaderStatus_ReleaseApproved;
                this.includeHeaderStatus_AllocationStarted = includeHeaderStatus_AllocationStarted;
            }
            public string storeIDandName { get; private set; } // readonly 
            public int storeRID { get; private set; } // readonly 
            public bool includeHeaderType_ASN { get; private set; } // readonly            
            public bool includeHeaderType_Assortment { get; private set; } // readonly
            public bool includeHeaderType_DropShip { get; private set; } // readonly    
            public bool includeHeaderType_Dummy { get; private set; } // readonly      
            public bool includeHeaderType_IMO { get; private set; } // readonly         
            public bool includeHeaderType_MultiHeader { get; private set; } // readonly
            public bool includeHeaderType_Placeholder { get; private set; } // readonly   
            public bool includeHeaderType_PurchaseOrder { get; private set; } // readonly  
            public bool includeHeaderType_Receipt { get; private set; } // readonly 
            public bool includeHeaderType_Reserve { get; private set; } // readonly       
            public bool includeHeaderType_WorkupTotalBuy { get; private set; } // readonly
            public bool includeHeaderType_Master { get; private set; } // readonly// TT#1966-MD - JSmith - DC Fulfillment
            public bool includeHeaderStatus_ReceivedOutOfBalance { get; private set; } // readonly
            public bool includeHeaderStatus_ReceivedInBalance { get; private set; } // readonly  
            public bool includeHeaderStatus_InUseByMultiHeader { get; private set; } // readonly     
            public bool includeHeaderStatus_PartialSizeOutOfBalance { get; private set; } // readonly
            public bool includeHeaderStatus_PartialSizeInBalance { get; private set; } // readonly
            public bool includeHeaderStatus_AllocatedOutOfBalance { get; private set; } // readonly  
            public bool includeHeaderStatus_AllocatedInBalance { get; private set; } // readonly 
            public bool includeHeaderStatus_SizesOutOfBalance { get; private set; } // readonly    
            public bool includeHeaderStatus_AllInBalance { get; private set; } // readonly     
            public bool includeHeaderStatus_Released { get; private set; } // readonly          
            public bool includeHeaderStatus_ReleaseApproved { get; private set; } // readonly
            public bool includeHeaderStatus_AllocationStarted { get; private set; } // readonly    
        }
        public DataSet AllocationByStore_Report(DataSet ds, AllocationByStoreEventArgs e)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@ST_RID", e.storeRID, eDbType.Int, eParameterDirection.Input),
                //                              //header types
                //                              new MIDDbParameter("@Include_Receipt", e.includeHeaderType_Receipt, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_ASN", e.includeHeaderType_ASN, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Dummy", e.includeHeaderType_Dummy, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_DropShip", e.includeHeaderType_DropShip, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_MultiHeader", e.includeHeaderType_MultiHeader, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Reserve", e.includeHeaderType_Reserve, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_WorkupTotalBuy", e.includeHeaderType_WorkupTotalBuy, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_PurchaseOrder", e.includeHeaderType_PurchaseOrder, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Assortment", e.includeHeaderType_Assortment, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Placeholder", e.includeHeaderType_Placeholder, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_IMO", e.includeHeaderType_IMO, eDbType.Bit, eParameterDirection.Input),
                //                              //header statues
                //                              new MIDDbParameter("@Include_ReceivedOutOfBalance", e.includeHeaderStatus_ReceivedOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_ReceivedInBalance", e.includeHeaderStatus_ReceivedInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_InUseByMultiHeader", e.includeHeaderStatus_InUseByMultiHeader, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_PartialSizeOutOfBalance", e.includeHeaderStatus_PartialSizeOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_PartialSizeInBalance", e.includeHeaderStatus_PartialSizeInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllocatedOutOfBalance", e.includeHeaderStatus_AllocatedOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllocatedInBalance", e.includeHeaderStatus_AllocatedInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_SizesOutOfBalance", e.includeHeaderStatus_SizesOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllInBalance", e.includeHeaderStatus_AllInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Released", e.includeHeaderStatus_Released, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_ReleaseApproved", e.includeHeaderStatus_ReleaseApproved, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllocationStarted", e.includeHeaderStatus_AllocationStarted, eDbType.Bit, eParameterDirection.Input)

                //                             };


                //ds.Tables.Add(_dba.ExecuteQuery("dbo.MID_GET_ALLOCATION_BY_STORE", "AllocationByStoreDataTable", InParams));

                ds.Tables.Add(StoredProcedures.MID_GET_ALLOCATION_BY_STORE.Read(_dba,
                                                                              ST_RID: e.storeRID,
                                                                              Include_Receipt: Convert.ToInt32(e.includeHeaderType_Receipt),
                                                                              Include_ASN: Convert.ToInt32(e.includeHeaderType_ASN),
                                                                              Include_Dummy: Convert.ToInt32(e.includeHeaderType_Dummy),
                                                                              Include_DropShip: Convert.ToInt32(e.includeHeaderType_DropShip),
                                                                              Include_MultiHeader: Convert.ToInt32(e.includeHeaderType_MultiHeader),
                                                                              Include_Reserve: Convert.ToInt32(e.includeHeaderType_Reserve),
                                                                              Include_WorkupTotalBuy: Convert.ToInt32(e.includeHeaderType_WorkupTotalBuy),
                                                                              Include_PurchaseOrder: Convert.ToInt32(e.includeHeaderType_PurchaseOrder),
                                                                              Include_Assortment: Convert.ToInt32(e.includeHeaderType_Assortment),
                                                                              Include_Placeholder: Convert.ToInt32(e.includeHeaderType_Placeholder),
                                                                              Include_IMO: Convert.ToInt32(e.includeHeaderType_IMO),
                                                                              Include_Master: Convert.ToInt32(e.includeHeaderType_Master),  // TT#1966-MD - JSmith - DC Fulfillment
                                                                              Include_ReceivedOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_ReceivedOutOfBalance),
                                                                              Include_ReceivedInBalance: Convert.ToInt32(e.includeHeaderStatus_ReceivedInBalance),
                                                                              Include_InUseByMultiHeader: Convert.ToInt32(e.includeHeaderStatus_InUseByMultiHeader),
                                                                              Include_PartialSizeOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_PartialSizeOutOfBalance),
                                                                              Include_PartialSizeInBalance: Convert.ToInt32(e.includeHeaderStatus_PartialSizeInBalance),
                                                                              Include_AllocatedOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_AllocatedOutOfBalance),
                                                                              Include_AllocatedInBalance: Convert.ToInt32(e.includeHeaderStatus_AllocatedInBalance),
                                                                              Include_SizesOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_SizesOutOfBalance),
                                                                              Include_AllInBalance: Convert.ToInt32(e.includeHeaderStatus_AllInBalance),
                                                                              Include_Released: Convert.ToInt32(e.includeHeaderStatus_Released),
                                                                              Include_ReleaseApproved: Convert.ToInt32(e.includeHeaderStatus_ReleaseApproved),
                                                                              Include_AllocationStarted: Convert.ToInt32(e.includeHeaderStatus_AllocationStarted)
                                                                              ));
                ds.Tables[0].TableName = "AllocationByStoreDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public string AllocationByStore_Report_GetParentOfStyleLevelName()
        {
            try
            {
                //return (string)_dba.ExecuteScalar("SELECT [dbo].[UDF_HIERARCHY_GET_PARENT_OF_STYLE_LEVEL_NAME] () AS PARENT_OF_STYLE_LEVEL_NAME");
                return (string)StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME.ReadValues(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public string AllocationByStore_Report_GetStyleLevelName()
        {
            try
            {
                //return (string)_dba.ExecuteScalar("SELECT [dbo].[UDF_HIERARCHY_GET_STYLE_LEVEL_NAME] () AS STYLE_LEVEL_NAME");
                return (string)StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME.ReadValues(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
        //BEGIN TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public class AllocationAnalysisEventArgs
        {
            public AllocationAnalysisEventArgs()
            {
                this.restrictToStyle_HN_RID = -1;
                this.resultLimit = -1;
                this.useDateRange = false;
                this.restrictMethods = false;
                this.restrictUsers = false;
                this.restrictHeaders = false;
            }

            public int restrictToStyle_HN_RID { get; set; }
            public bool useDateRange { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public int resultLimit { get; set; }
            public bool restrictMethods { get; set; }
            public string methodRIDsToInclude { get; set; }
            public bool restrictUsers { get; set; }
            public string userRIDsToInclude { get; set; }
            public bool restrictHeaders { get; set; }
            public string headerRIDsToInclude { get; set; }

            public bool includeHeaderType_ASN { get; set; }
            public bool includeHeaderType_Assortment { get; set; }
            public bool includeHeaderType_DropShip { get; set; }
            public bool includeHeaderType_Dummy { get; set; }
            public bool includeHeaderType_IMO { get; set; }
            public bool includeHeaderType_MultiHeader { get; set; }
            public bool includeHeaderType_Placeholder { get; set; }
            public bool includeHeaderType_PurchaseOrder { get; set; }
            public bool includeHeaderType_Receipt { get; set; }
            public bool includeHeaderType_Reserve { get; set; }
            public bool includeHeaderType_WorkupTotalBuy { get; set; }
            public bool includeHeaderType_Master { get; set; }// Begin TT#1966-MD - JSmith - DC Fulfillment
            public bool includeHeaderStatus_ReceivedOutOfBalance { get; set; }
            public bool includeHeaderStatus_ReceivedInBalance { get; set; }
            public bool includeHeaderStatus_InUseByMultiHeader { get; set; }
            public bool includeHeaderStatus_PartialSizeOutOfBalance { get; set; }
            public bool includeHeaderStatus_PartialSizeInBalance { get; set; }
            public bool includeHeaderStatus_AllocatedOutOfBalance { get; set; }
            public bool includeHeaderStatus_AllocatedInBalance { get; set; }
            public bool includeHeaderStatus_SizesOutOfBalance { get; set; }
            public bool includeHeaderStatus_AllInBalance { get; set; }
            public bool includeHeaderStatus_Released { get; set; }
            public bool includeHeaderStatus_ReleaseApproved { get; set; }
            public bool includeHeaderStatus_AllocationStarted { get; set; }
            //Action Types
            public bool includeActionType_ActionUnassigned { get; set; }
            public bool includeActionType_ApplyAPI_Workflow { get; set; }
            public bool includeActionType_BackoutAllocation { get; set; }
            public bool includeActionType_BackoutDetailPackAllocation { get; set; }
            public bool includeActionType_BackoutSizeAllocation { get; set; }
            public bool includeActionType_BackoutSizeIntransit { get; set; }
            public bool includeActionType_BackoutStyleIntransit { get; set; }
            public bool includeActionType_BalanceSizeBilaterally { get; set; }
            public bool includeActionType_BalanceSizeNoSubs { get; set; }
            public bool includeActionType_BalanceSizeWithConstraints { get; set; }
            public bool includeActionType_BalanceSizeWithSubs { get; set; }
            public bool includeActionType_BalanceStyleProportional { get; set; }
            public bool includeActionType_BalanceToDC { get; set; }
            public bool includeActionType_BreakoutSizesAsReceived { get; set; }
            public bool includeActionType_BreakoutSizesAsReceivedWithConstraints { get; set; }
            public bool includeActionType_ChargeIntransit { get; set; }
            public bool includeActionType_ChargeSizeIntransit { get; set; }
            public bool includeActionType_DeleteHeader { get; set; }
            public bool includeActionType_ReapplyTotalAllocation { get; set; }
            public bool includeActionType_Release { get; set; }
            public bool includeActionType_RemoveAPI_Workflow { get; set; }
            public bool includeActionType_Reset { get; set; }
            public bool includeActionType_StyleNeed { get; set; }
            public bool includeActionType_BalanceToVSW { get; set; } // TT#1334-MD - stodd - Balance to VSW Action
        }
        public DataSet AllocationAnalysis_GetData(DataSet ds, AllocationAnalysisEventArgs e)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@RestrictToStyle_HN_RID", e.restrictToStyle_HN_RID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@UseDateRange", e.useDateRange, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@StartDate", e.startDate, eDbType.DateTime, eParameterDirection.Input),
                //                              new MIDDbParameter("@EndDate", e.endDate, eDbType.DateTime, eParameterDirection.Input),
                //                              new MIDDbParameter("@ResultLimit", e.resultLimit, eDbType.Int, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictMethods", e.restrictMethods, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@MethodRIDsToInclude", e.methodRIDsToInclude, eDbType.VarChar, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictUsers", e.restrictUsers, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@UserRIDsToInclude", e.userRIDsToInclude, eDbType.VarChar, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictHeaders", e.restrictHeaders, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@HeaderRIDsToInclude", e.headerRIDsToInclude, eDbType.VarChar, eParameterDirection.Input),
                //                              //header types
                //                              new MIDDbParameter("@Include_Receipt", e.includeHeaderType_Receipt, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_ASN", e.includeHeaderType_ASN, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Dummy", e.includeHeaderType_Dummy, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_DropShip", e.includeHeaderType_DropShip, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_MultiHeader", e.includeHeaderType_MultiHeader, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Reserve", e.includeHeaderType_Reserve, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_WorkupTotalBuy", e.includeHeaderType_WorkupTotalBuy, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_PurchaseOrder", e.includeHeaderType_PurchaseOrder, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Assortment", e.includeHeaderType_Assortment, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Placeholder", e.includeHeaderType_Placeholder, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_IMO", e.includeHeaderType_IMO, eDbType.Bit, eParameterDirection.Input),
                //                              //header statuses
                //                              new MIDDbParameter("@Include_ReceivedOutOfBalance", e.includeHeaderStatus_ReceivedOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_ReceivedInBalance", e.includeHeaderStatus_ReceivedInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_InUseByMultiHeader", e.includeHeaderStatus_InUseByMultiHeader, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_PartialSizeOutOfBalance", e.includeHeaderStatus_PartialSizeOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_PartialSizeInBalance", e.includeHeaderStatus_PartialSizeInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllocatedOutOfBalance", e.includeHeaderStatus_AllocatedOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllocatedInBalance", e.includeHeaderStatus_AllocatedInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_SizesOutOfBalance", e.includeHeaderStatus_SizesOutOfBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllInBalance", e.includeHeaderStatus_AllInBalance, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_Released", e.includeHeaderStatus_Released, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_ReleaseApproved", e.includeHeaderStatus_ReleaseApproved, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@Include_AllocationStarted", e.includeHeaderStatus_AllocationStarted, eDbType.Bit, eParameterDirection.Input),
                //                              //action types
                //                              new MIDDbParameter("@IncludeActionType_ActionUnassigned", e.includeActionType_ActionUnassigned, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_ApplyAPI_Workflow", e.includeActionType_ApplyAPI_Workflow, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BackoutAllocation", e.includeActionType_BackoutAllocation, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BackoutDetailPackAllocation", e.includeActionType_BackoutDetailPackAllocation, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BackoutSizeAllocation", e.includeActionType_BackoutSizeAllocation, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BackoutSizeIntransit", e.includeActionType_BackoutSizeIntransit, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BackoutStyleIntransit", e.includeActionType_BackoutStyleIntransit, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BalanceSizeBilaterally", e.includeActionType_BalanceSizeBilaterally, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BalanceSizeNoSubs", e.includeActionType_BalanceSizeNoSubs, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BalanceSizeWithConstraints", e.includeActionType_BalanceSizeWithConstraints, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BalanceSizeWithSubs", e.includeActionType_BalanceSizeWithSubs, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BalanceStyleProportional", e.includeActionType_BalanceStyleProportional, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BalanceToDC", e.includeActionType_BalanceToDC, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BreakoutSizesAsReceived", e.includeActionType_BreakoutSizesAsReceived, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_BreakoutSizesAsReceivedWithConstraints", e.includeActionType_BreakoutSizesAsReceivedWithConstraints, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_ChargeIntransit", e.includeActionType_ChargeIntransit, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_ChargeSizeIntransit", e.includeActionType_ChargeSizeIntransit, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_DeleteHeader", e.includeActionType_DeleteHeader, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_ReapplyTotalAllocation", e.includeActionType_ReapplyTotalAllocation, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_Release", e.includeActionType_Release, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_RemoveAPI_Workflow", e.includeActionType_RemoveAPI_Workflow, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_Reset", e.includeActionType_Reset, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@IncludeActionType_StyleNeed", e.includeActionType_StyleNeed, eDbType.Bit, eParameterDirection.Input)

                //                             };


                //ds.Tables.Add(_dba.ExecuteQuery("dbo.MID_GET_ALLOCATION_ANALYSIS", "AllocationAnalysisDataTable", InParams));

                //BEGIN TT#3893-VStuart-Unhandled Exception Error-Allocation Analysis-MID
                ds.Tables.Add(StoredProcedures.MID_GET_ALLOCATION_ANALYSIS.Read(_dba,
                                                                              RestrictToStyle_HN_RID: e.restrictToStyle_HN_RID,
                                                                              UseDateRange: Convert.ToInt32(e.useDateRange),
                                                                              StartDate: e.startDate,
                                                                              EndDate: e.endDate,
                                                                              ResultLimit: e.resultLimit,
                                                                              RestrictMethods: Convert.ToInt32(e.restrictMethods),
                                                                              MethodRIDsToInclude: e.methodRIDsToInclude,
                                                                              RestrictUsers: Convert.ToInt32(e.restrictUsers),
                                                                              UserRIDsToInclude: e.userRIDsToInclude,
                                                                              RestrictHeaders: Convert.ToInt32(e.restrictHeaders),
                                                                              HeaderRIDsToInclude: e.headerRIDsToInclude,
                                                                              Include_Receipt: Convert.ToInt32(e.includeHeaderType_Receipt),
                                                                              Include_ASN: Convert.ToInt32(e.includeHeaderType_ASN),
                                                                              Include_Dummy: Convert.ToInt32(e.includeHeaderType_Dummy),
                                                                              Include_DropShip: Convert.ToInt32(e.includeHeaderType_DropShip),
                                                                              Include_MultiHeader: Convert.ToInt32(e.includeHeaderType_MultiHeader),
                                                                              Include_Reserve: Convert.ToInt32(e.includeHeaderType_Reserve),
                                                                              Include_WorkupTotalBuy: Convert.ToInt32(e.includeHeaderType_WorkupTotalBuy),
                                                                              Include_PurchaseOrder: Convert.ToInt32(e.includeHeaderType_PurchaseOrder),
                                                                              Include_Assortment: Convert.ToInt32(e.includeHeaderType_Assortment),
                                                                              Include_Placeholder: Convert.ToInt32(e.includeHeaderType_Placeholder),
                                                                              Include_IMO: Convert.ToInt32(e.includeHeaderType_IMO),
                                                                              Include_Master: Convert.ToInt32(e.includeHeaderType_Master),  // TT#1966-MD - JSmith - DC Fulfillment
                                                                              Include_ReceivedOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_ReceivedOutOfBalance),
                                                                              Include_ReceivedInBalance: Convert.ToInt32(e.includeHeaderStatus_ReceivedInBalance),
                                                                              Include_InUseByMultiHeader: Convert.ToInt32(e.includeHeaderStatus_InUseByMultiHeader),
                                                                              Include_PartialSizeOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_PartialSizeOutOfBalance),
                                                                              Include_PartialSizeInBalance: Convert.ToInt32(e.includeHeaderStatus_PartialSizeInBalance),
                                                                              Include_AllocatedOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_AllocatedOutOfBalance),
                                                                              Include_AllocatedInBalance: Convert.ToInt32(e.includeHeaderStatus_AllocatedInBalance),
                                                                              Include_SizesOutOfBalance: Convert.ToInt32(e.includeHeaderStatus_SizesOutOfBalance),
                                                                              Include_AllInBalance: Convert.ToInt32(e.includeHeaderStatus_AllInBalance),
                                                                              Include_Released: Convert.ToInt32(e.includeHeaderStatus_Released),
                                                                              Include_ReleaseApproved: Convert.ToInt32(e.includeHeaderStatus_ReleaseApproved),
                                                                              Include_AllocationStarted: Convert.ToInt32(e.includeHeaderStatus_AllocationStarted),
                                                                              IncludeActionType_ActionUnassigned: Convert.ToInt32(e.includeActionType_ActionUnassigned),
                                                                              IncludeActionType_ApplyAPI_Workflow: Convert.ToInt32(e.includeActionType_ApplyAPI_Workflow),
                                                                              IncludeActionType_BackoutAllocation: Convert.ToInt32(e.includeActionType_BackoutAllocation),
                                                                              IncludeActionType_BackoutDetailPackAllocation: Convert.ToInt32(e.includeActionType_BackoutDetailPackAllocation),
                                                                              IncludeActionType_BackoutSizeAllocation: Convert.ToInt32(e.includeActionType_BackoutSizeAllocation),
                                                                              IncludeActionType_BackoutSizeIntransit: Convert.ToInt32(e.includeActionType_BackoutSizeIntransit),
                                                                              IncludeActionType_BackoutStyleIntransit: Convert.ToInt32(e.includeActionType_BackoutStyleIntransit),
                                                                              IncludeActionType_BalanceSizeBilaterally: Convert.ToInt32(e.includeActionType_BalanceSizeBilaterally),
                                                                              IncludeActionType_BalanceSizeNoSubs: Convert.ToInt32(e.includeActionType_BalanceSizeNoSubs),
                                                                              IncludeActionType_BalanceSizeWithConstraints: Convert.ToInt32(e.includeActionType_BalanceSizeWithConstraints),
                                                                              IncludeActionType_BalanceSizeWithSubs: Convert.ToInt32(e.includeActionType_BalanceSizeWithSubs),
                                                                              IncludeActionType_BalanceStyleProportional: Convert.ToInt32(e.includeActionType_BalanceStyleProportional),
                                                                              IncludeActionType_BalanceToDC: Convert.ToInt32(e.includeActionType_BalanceToDC),
                                                                              IncludeActionType_BreakoutSizesAsReceived: Convert.ToInt32(e.includeActionType_BreakoutSizesAsReceived),
                                                                              IncludeActionType_BreakoutSizesAsReceivedWithConstraints: Convert.ToInt32(e.includeActionType_BreakoutSizesAsReceivedWithConstraints),
                                                                              IncludeActionType_ChargeIntransit: Convert.ToInt32(e.includeActionType_ChargeIntransit),
                                                                              IncludeActionType_ChargeSizeIntransit: Convert.ToInt32(e.includeActionType_ChargeSizeIntransit),
                                                                              IncludeActionType_DeleteHeader: Convert.ToInt32(e.includeActionType_DeleteHeader),
                                                                              IncludeActionType_ReapplyTotalAllocation: Convert.ToInt32(e.includeActionType_ReapplyTotalAllocation),
                                                                              IncludeActionType_Release: Convert.ToInt32(e.includeActionType_Release),
                                                                              IncludeActionType_RemoveAPI_Workflow: Convert.ToInt32(e.includeActionType_RemoveAPI_Workflow),
                                                                              IncludeActionType_Reset: Convert.ToInt32(e.includeActionType_Reset),
                                                                              IncludeActionType_StyleNeed: Convert.ToInt32(e.includeActionType_StyleNeed), // TT#1334-MD - stodd - Balance to VSW Action
                                                                              IncludeActionType_BalanceToVSW: Convert.ToInt32(e.includeActionType_BalanceToVSW) // TT#1334-MD - stodd - Balance to VSW Action
                                                                              ));

                ds.Tables[0].TableName = "AllocationAnalysisDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                ds.Tables[0].Columns["AUDIT_DATETIME"].Caption = "Date/Time";
                ds.Tables[0].Columns["USER_ID"].Caption = "User ID";
                ds.Tables[0].Columns["HEADER_ID"].Caption = "Header ID";
                ds.Tables[0].Columns["HEADER_TYPE"].Caption = "Header Type";
                ds.Tables[0].Columns["HEADER_STATUS"].Caption = "Header Status";
                ds.Tables[0].Columns["STYLE"].Caption = "Style";
                ds.Tables[0].Columns["ACTION"].Caption = "Action";
                ds.Tables[0].Columns["METHOD"].Caption = "Method";
                ds.Tables[0].Columns["ALLOCATED_BY_PROCESS"].Caption = "Allocated by Process";
                ds.Tables[0].Columns["STORES_BY_PROCESS"].Caption = "Stores by Process";
                ds.Tables[0].Columns["AVAILABLE_UNITS"].Caption = "Available Units";
                ds.Tables[0].Columns["TOTAL_ALLOCATED"].Caption = "Total Allocated";
                ds.Tables[0].Columns["TOTAL_STORES"].Caption = "Total Stores";
                ds.Tables[0].Columns["RESERVE_UNITS"].Caption = "Reserve Units";
                ds.Tables[0].Columns["WORKFLOW"].Caption = "Workflow";
                ds.Tables[0].Columns["TASKLIST"].Caption = "Task List";
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public class ForecastAnalysisEventArgs
        {
            public ForecastAnalysisEventArgs()
            {
                this.restrictToDescendantsOf_HN_RID = -1;
                this.restrictToLowerLevelSequence = -1;
                this.resultLimit = -1;
                this.useDateRange = false;
                this.useForecastDateRange = false;
                this.restrictMethods = false;
                this.restrictUsers = false;
                this.restrictStoreForecastVersions = false;
                this.restrictChainForecastVersions = false;
            }

            public int restrictToDescendantsOf_HN_RID { get; set; }
            public int restrictToLowerLevelSequence { get; set; }

            public bool useDateRange { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }

            public bool useForecastDateRange { get; set; }
            public DateTime ForecastStartDate { get; set; }
            public DateTime ForecastEndDate { get; set; }

            public int resultLimit { get; set; }

            public bool restrictMethods { get; set; }
            public string methodRIDsToInclude { get; set; }

            public bool restrictUsers { get; set; }
            public string userRIDsToInclude { get; set; }

            public bool restrictStoreForecastVersions { get; set; }
            public string storeForecastVersionRIDsToInclude { get; set; }

            public bool restrictChainForecastVersions { get; set; }
            public string chainForecastVersionRIDsToInclude { get; set; }

        }
        public DataSet ForecastAnalysis_GetData(DataSet ds, ForecastAnalysisEventArgs e)
        {
            try
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@RestrictToDescendantsOf_HN_RID", e.restrictToDescendantsOf_HN_RID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@RestrictToLowerLevelSequence", e.restrictToLowerLevelSequence, eDbType.Int, eParameterDirection.Input),

                //                              new MIDDbParameter("@UseDateRange", e.useDateRange, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@StartDate", e.startDate, eDbType.DateTime, eParameterDirection.Input),
                //                              new MIDDbParameter("@EndDate", e.endDate, eDbType.DateTime, eParameterDirection.Input),

                //                              new MIDDbParameter("@UseForecastDateRange", e.useForecastDateRange, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@ForecastStartDate", e.ForecastStartDate, eDbType.DateTime, eParameterDirection.Input),
                //                              new MIDDbParameter("@ForecastEndDate", e.ForecastEndDate, eDbType.DateTime, eParameterDirection.Input),

                //                              new MIDDbParameter("@ResultLimit", e.resultLimit, eDbType.Int, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictMethods", e.restrictMethods, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@MethodRIDsToInclude", e.methodRIDsToInclude, eDbType.VarChar, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictUsers", e.restrictUsers, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@UserRIDsToInclude", e.userRIDsToInclude, eDbType.VarChar, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictStoreForecastVersions", e.restrictStoreForecastVersions, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@StoreForecastVersionRIDsToInclude", e.storeForecastVersionRIDsToInclude, eDbType.VarChar, eParameterDirection.Input),

                //                              new MIDDbParameter("@RestrictChainForecastVersions", e.restrictChainForecastVersions, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@ChainForecastVersionRIDsToInclude", e.chainForecastVersionRIDsToInclude, eDbType.VarChar, eParameterDirection.Input)

                //                             };

                //ds.Tables.Add(_dba.ExecuteQuery("dbo.MID_GET_FORECAST_ANALYSIS", "ForecastAnalysisDataTable", InParams));

                //BEGIN TT#3892-VStuart-Unhandled Exception Error-Forecast Analysis-MID
                ds.Tables.Add(StoredProcedures.MID_GET_FORECAST_ANALYSIS.Read(_dba,
                                                                            RestrictToDescendantsOf_HN_RID: e.restrictToDescendantsOf_HN_RID,
                                                                            RestrictToLowerLevelSequence: e.restrictToLowerLevelSequence,
                                                                            UseDateRange: Convert.ToInt32(e.useDateRange),
                                                                            StartDate: e.startDate,
                                                                            EndDate: e.endDate,
                                                                            UseForecastDateRange: Convert.ToInt32(e.useForecastDateRange),
                                                                            ForecastStartDate: e.ForecastStartDate,
                                                                            ForecastEndDate: e.ForecastEndDate,
                                                                            ResultLimit: e.resultLimit,
                                                                            RestrictMethods: Convert.ToInt32(e.restrictMethods),
                                                                            MethodRIDsToInclude: e.methodRIDsToInclude,
                                                                            RestrictUsers: Convert.ToInt32(e.restrictUsers),
                                                                            UserRIDsToInclude: e.userRIDsToInclude,
                                                                            RestrictStoreForecastVersions: Convert.ToInt32(e.restrictStoreForecastVersions),
                                                                            StoreForecastVersionRIDsToInclude: e.storeForecastVersionRIDsToInclude,
                                                                            RestrictChainForecastVersions: Convert.ToInt32(e.restrictChainForecastVersions),
                                                                            ChainForecastVersionRIDsToInclude: e.chainForecastVersionRIDsToInclude
                                                                            ));
                ds.Tables[0].TableName = "ForecastAnalysisDataTable"; //keep the table name so Crystal Reports can match it to the predefined dataset and table
                return ds;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


        public DataTable GetAllocationMethodTypesUsed(int userRID)
        {
            //MIDDbParameter[] InParams = { new MIDDbParameter("@USER_RID", userRID, eDbType.Int, eParameterDirection.Input) };

            //return _dba.ExecuteQuery("dbo.MID_GET_ALLOCATION_METHOD_TYPES_USED", "AllocationMethodTypesUsed", InParams);

            return StoredProcedures.MID_GET_ALLOCATION_METHOD_TYPES_USED.Read(_dba, USER_RID: userRID);
        }

        public DataTable GetAllocationMethodsUsed(int userRID, bool includeDescription)
        {
            //MIDDbParameter[] InParams = { new MIDDbParameter("@USER_RID", userRID, eDbType.Int, eParameterDirection.Input),
            //                                new MIDDbParameter("@INCLUDE_DESCRIPTION", includeDescription, eDbType.Bit, eParameterDirection.Input)
            //                            };

            //return _dba.ExecuteQuery("dbo.MID_GET_ALLOCATION_METHODS_USED", "AllocationMethodsUsed", InParams);

            return StoredProcedures.MID_GET_ALLOCATION_METHODS_USED.Read(_dba,
                                                                                  USER_RID: userRID,
                                                                                  INCLUDE_DESCRIPTION: Convert.ToInt32(includeDescription)
                                                                                  );
        }

        public DataTable GetForecastMethodTypesUsed(int userRID)
        {
            //MIDDbParameter[] InParams = { new MIDDbParameter("@USER_RID", userRID, eDbType.Int, eParameterDirection.Input) };

            //return _dba.ExecuteQuery("dbo.MID_GET_FORECAST_METHOD_TYPES_USED", "ForecastMethodTypesUsed", InParams);

            return StoredProcedures.MID_GET_FORECAST_METHOD_TYPES_USED.Read(_dba, USER_RID: userRID);
        }

        public DataTable GetForecastMethodsUsed(int userRID, bool includeDescription)
        {
            //MIDDbParameter[] InParams = { new MIDDbParameter("@USER_RID", userRID, eDbType.Int, eParameterDirection.Input),
            //                                new MIDDbParameter("@INCLUDE_DESCRIPTION", includeDescription, eDbType.Bit, eParameterDirection.Input)
            //                            };

            //return _dba.ExecuteQuery("dbo.MID_GET_FORECAST_METHODS_USED", "ForecastMethodsUsed", InParams);

            return StoredProcedures.MID_GET_FORECAST_METHODS_USED.Read(_dba,
                                                                                USER_RID: userRID,
                                                                                INCLUDE_DESCRIPTION: Convert.ToInt32(includeDescription)
                                                                                );
        }

        //END TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
    }
}
