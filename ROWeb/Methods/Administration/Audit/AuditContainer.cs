using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logility.ROWeb.Methods.Administration.Audit
{
    public class ROAuditContainer
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private long _ROInstanceID;
        //private CharacteristicBase _characteristicsClass;

        private filterTypes filterTypes;

        //=============
        // CONSTRUCTORS
        //=============
        public ROAuditContainer(SessionAddressBlock SAB, ROWebTools ROWebTools, long ROInstanceID)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
        }

        //===========
        // PROPERTIES
        //===========

        /// <summary>
        /// Gets the SessionAddressBlock
        /// </summary>
        protected SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        /// <summary>
        /// Gets the unique function ID
        /// </summary>
        protected long ROInstanceID
        {
            get { return _ROInstanceID; }
        }

        //===========
        // METHODS
        //===========

        public void CleanUp()
        {


        }
        public ROOut GetAuditFilterOption(ROProfileKeyParms parms)
        {


            if (parms.ProfileType == eProfileType.AuditFilter)
            {
                filterType = filterTypes.AuditFilter;
            }
            else
            {
                return new ROOut(eROReturnCode.Failure, "Profile Type or Filter Type Error", ROInstanceID);
            }

            try
            {
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                userRIDList.Add(SAB.ClientServerSession.UserRID);

                FilterData fd = new FilterData();
                DataTable dtFilters = fd.FilterReadForUser(filterType, userRIDList);


                if (filterType == filterTypes.AuditFilter && dtFilters.Rows.Count == 0)     //create default audit filter
                {
                    int defaultAuditFilteRID = fd.InsertFilterForAuditDefault(SAB.ClientServerSession.UserRID);
                    fd.InsertConditionForAuditDefault(defaultAuditFilteRID);
                    DataRow drDefaultFilter = dtFilters.NewRow();
                    drDefaultFilter["FILTER_RID"] = defaultAuditFilteRID;
                    drDefaultFilter["USER_RID"] = SAB.ClientServerSession.UserRID;
                    drDefaultFilter["FILTER_NAME"] = "Default Audit Filter";
                    drDefaultFilter["OWNER_USER_RID"] = SAB.ClientServerSession.UserRID;
                    drDefaultFilter["FILTER_USER_RID"] = SAB.ClientServerSession.UserRID;
                    dtFilters.Rows.Add(drDefaultFilter);
                }
                int key = 0;
                string value = string.Empty;

                if (dtFilters.Rows.Count > 0)
                {
                    foreach (DataRow item in dtFilters.Rows)
                    {
                        key = Convert.ToInt32(item["FILTER_RID"]);
                        value = item["FILTER_NAME"].ToString();
                    }
                }
                eROReturnCode returnCode = eROReturnCode.Successful;
                string message = null;
                return new ROAuditFilterOption(returnCode, message, ROInstanceID,
                        auditFilterOptions: new KeyValuePair<int, string>(key, value));

            }
            catch (Exception ex)
            {

                return new ROAuditFilterOption(eROReturnCode.Failure, ex.Message, ROInstanceID,
                      auditFilterOptions: new KeyValuePair<int, string>());
            }


            //  return auditFilterOption;

        }


        private filterTypes filterType;
        public int currentFilterRID = Include.NoRID;
        private bool auditMergeDetails;
        private bool auditIncludeSummary;
        private bool auditIncludeDetails;
        public ROOut GenerateAuditReport(ROProfileKeyParms parms)
        {
            // Add product filter to profile type
            if (parms.ProfileType == eProfileType.AuditFilter || parms.ProfileType == eProfileType.AuditFilter)
            {
                this.currentFilterRID = parms.Key;
                filterType = filterTypes.AuditFilter;
            }
            else
            {
                return new ROOut(eROReturnCode.Failure, "Profile Type or Filter Type Error", ROInstanceID);

            }

            var AuditResults = new List<AuditResult>();
            try
            {
                filter f = filterDataHelper.LoadExistingFilter(this.currentFilterRID);
                string sql = string.Empty;

                if (filterType == filterTypes.ProductFilter)
                {
                    sql = filterEngineSQLforProducts.MakeSqlForFilter(f);
                }
                else if (filterType == filterTypes.AuditFilter)
                {
                    auditMergeDetails = false;
                    auditIncludeSummary = true;
                    auditIncludeDetails = true;

                    bool mergeDetailsInSQL = false;
                    if (auditIncludeDetails && auditMergeDetails)
                    {
                        mergeDetailsInSQL = true;
                    }
                    sql = filterEngineSQLforAudit.MakeSqlForFilter(f, mergeDetailsInSQL);

                    AuditResults = StartProcess(sql);
                }

                if (AuditResults.Count > 0)
                {
                    return new ROAuditResult(eROReturnCode.Successful, "", ROInstanceID, AuditResults);
                }
                else
                {
                    return new ROAuditResult(eROReturnCode.Failure, "", ROInstanceID, null);
                }

            }
            catch (Exception ex)
            {

                return new ROAuditResult(eROReturnCode.Failure, ex.Message, ROInstanceID, null);
            }



        }

        private DataSet dsResults;
        //
        private List<AuditResult> StartProcess(string sql)
        {
            FilterData fd = new FilterData();
            DataTable dt = fd.ExecuteSQLQuery(sql, "searchResults", 0); //TT#1430-MD -jsobek -Null reference after canceling a product search
            dsResults = new DataSet();
            dsResults.Tables.Add(dt);
            dsResults = AfterExecutionForAudit(dsResults);
            return ConvertDataSetTOModel(dsResults);
        }


        public List<AuditResult> ConvertDataSetTOModel(DataSet dataSet)
        {
            List<AuditResult> auditresultList = new List<AuditResult>();

            // Assigning DataSet to Indivisual table
            DataTable result = dataSet.Tables[0];

            foreach (DataRow item in result.Rows)
            {
                AuditResult resultDetail = new AuditResult();
                resultDetail.ProcessRID = Convert.ToInt32(item["ProcessRID"]);
                resultDetail.ProcessID = Convert.ToInt32(item["ProcessID"]);
                resultDetail.Process = item["Process"].ToString();
                resultDetail.User = item["User"].ToString();
                resultDetail.ExecutionStatus = item["Execution Status"].ToString();
                resultDetail.CompletionStatus = item["Completion Status"].ToString();
                resultDetail.StartTime = item["Start Time"] != DBNull.Value ? item["Start Time"].ToString() : null;
                resultDetail.StopTime = item["Stop Time"] != DBNull.Value ? item["Stop Time"].ToString() : null;
                resultDetail.Duration = item["Duration"] != DBNull.Value ? item["Duration"].ToString() : null;
                resultDetail.HighestMessageLevel = item["Highest Message Level"].ToString();
                resultDetail.Time = item["Time"] != DBNull.Value ? item["Time"].ToString() : null;
                resultDetail.Module = item["Module"].ToString();
                resultDetail.MessageLevel = item["MessageLevel"] != DBNull.Value ? item["MessageLevel"].ToString() : null;
                resultDetail.Message = item["Message"] != DBNull.Value ? item["Message"].ToString() : null;
                resultDetail.MessageDetails = item["Message Details"] != DBNull.Value ? item["Message Details"].ToString() : null;
                resultDetail.AuditSummaryList = GetAuditSummary(Convert.ToInt32(item["ProcessRID"]), dataSet);
                resultDetail.AuditDetailsList = GetAuditDetails(Convert.ToInt32(item["ProcessRID"]), dataSet);
              
                auditresultList.Add(resultDetail);

            }

            return auditresultList;
        }

        private List<AuditDetails> GetAuditDetails(int processRID, DataSet dataSet)
        {
            List<AuditDetails> detailsList = new List<AuditDetails>();

            DataTable detailsRow = dataSet.Tables["DetailRow"];
            var detailsRowResult = GetAuditDetailsRow(processRID, detailsRow);
            DataSet ds = GetAuditDetailsByProcessType(detailsRowResult, dataSet);
            DataTable detailsTable = ds.Tables["Details"];

            foreach (DataRow item in detailsTable.Rows)
            {
                AuditDetails details = new AuditDetails();
                if (Convert.ToInt32(item["ProcessRID"]) == processRID)
                {
                    details.ProcessRID = Convert.ToInt32(item["ProcessRID"]);
                    details.Time = Convert.ToDateTime(item["Time"]).ToString(Include.AuditDateTimeFormat);
                    details.Module = item["Module"].ToString();
                    details.MessageLevel = item["MessageLevel"].ToString();
                    details.MessageLevelText = item["MessageLevelText"].ToString();
                    details.MessageCode = item["MessageCode"].ToString();
                    details.Message = item["Message"].ToString();
                    details.Message2 = item["Message2"].ToString();
                    detailsList.Add(details);
                }
            }
            return detailsList;
        }

        private DataSet GetAuditDetailsByProcessType(AuditDetailsRow detailsRowResult, DataSet ds)
        {
          
                if (detailsRowResult.NeedsLoaded)
                {
                    AuditUtility.AddDetail(Convert.ToInt32(detailsRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                }
            return ds;
            
        }

        private AuditSummaryRow GetAuditSummaryRow(int processRID, DataTable summaryRowTable)
        {

            AuditSummaryRow summaryRow = new AuditSummaryRow();
            foreach (DataRow item in summaryRowTable.Rows)
            {
               
                if (Convert.ToInt32(item["ProcessRID"]) == processRID)
                {
                    summaryRow.ProcessRID = Convert.ToInt32(item["ProcessRID"]);
                    summaryRow.ProcessID = Convert.ToInt32(item["ProcessID"]);
                    summaryRow.NeedsLoaded = Convert.ToBoolean(item["NeedsLoaded"]);
                    summaryRow.Text = item["Text"].ToString();
                    
                }
            }
            return summaryRow;
        }

        private AuditDetailsRow GetAuditDetailsRow(int processRID, DataTable detailsRowTable)
        {
            AuditDetailsRow detailsRow = new AuditDetailsRow();

            foreach (DataRow item in detailsRowTable.Rows)
            {
               
                if (Convert.ToInt32(item["ProcessRID"]) == processRID)
                {
                    detailsRow.ProcessRID = Convert.ToInt32(item["ProcessRID"]);
                    detailsRow.ProcessID = Convert.ToInt32(item["ProcessID"]);
                    detailsRow.NeedsLoaded = Convert.ToBoolean(item["NeedsLoaded"]);
                    detailsRow.Text = item["Text"].ToString();
                }
            }
            return detailsRow;
        }

        public List<AuditSummary> GetAuditSummary(int processRID, DataSet dataSet)
        {
            DataTable summaryRow = dataSet.Tables["SummaryRow"];
            var summaryRowResult = GetAuditSummaryRow(processRID, summaryRow);
            DataSet ds= GetAuditSummaryByProcessType(summaryRowResult, dataSet);
            DataTable summaryTable = ds.Tables["Summary"];

            List<AuditSummary> summaryList = new List<AuditSummary>();

            foreach (DataRow item in summaryTable.Rows)
            {
                AuditSummary summary = new AuditSummary();
                if (Convert.ToInt32(item["ProcessRID"]) == processRID)
                {
                    summary.ProcessRID = Convert.ToInt32(item["ProcessRID"]);
                    summary.Item = item["Item"].ToString();
                    summary.Value = item["Value"].ToString();
                    summaryList.Add(summary);
                }
            }
            return summaryList;
        }

        private DataSet GetAuditSummaryByProcessType(AuditSummaryRow summaryRowResult, DataSet ds)
        {
            if (summaryRowResult.NeedsLoaded)
            {
                eProcesses processType = (eProcesses)summaryRowResult.ProcessID;
                switch (processType)
                {
                    case eProcesses.hierarchyLoad:
                        AuditUtility.AddHierarchyLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    case eProcesses.storeLoad:
                        AuditUtility.AddStoreLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    case eProcesses.historyPlanLoad:
                        AuditUtility.AddHistoryPlanLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    case eProcesses.HeaderReconcile:
                        AuditUtility.AddHeaderReconcileSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    case eProcesses.headerLoad:
                        AuditUtility.AddHeaderLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //Begin MOD - JScott - Build Pack Criteria Load
                    case eProcesses.buildPackCriteriaLoad:
                        AuditUtility.AddBuildPackCriteriaLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End MOD - JScott - Build Pack Criteria Load
                    //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                    case eProcesses.ChainSetPercentCriteriaLoad:
                        AuditUtility.AddChainSetPercentCriteriaLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                    //Begin TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                    case eProcesses.StoreEligibilityCriteraLoad:
                        AuditUtility.AddStoreEligibilityCriteriaLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                    //Begin TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
                    case eProcesses.VSWCriteriaLoad:
                        AuditUtility.AddVSWCriteriaLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eProcesses.DailyPercentagesCriteraLoad:
                        AuditUtility.AddDailyPercentagesCriteriaLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement

                    case eProcesses.purge:
                        AuditUtility.AddPurgeSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    case eProcesses.colorCodeLoad:
                        AuditUtility.AddColorCodeLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    case eProcesses.sizeCodeLoad:
                        AuditUtility.AddSizeCodeLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    case eProcesses.sizeCurveLoad:
                        AuditUtility.AddSizeCurveLoadSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

                    //Begin TT#707 - JScott - Size Curve process needs to multi-thread
                    case eProcesses.sizeCurveGenerate:
                        AuditUtility.AddSizeCurveGenerateSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#707 - JScott - Size Curve process needs to multi-thread
                    case eProcesses.rollup:
                        AuditUtility.AddRollupSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
                    case eProcesses.computationDriver:
                        AuditUtility.AddComputationDriverSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End - Abercrombie & Fitch #4411
                    case eProcesses.sizeConstraintsLoad:
                        AuditUtility.AddSizeConstraintsSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //Begin Track #5100 - JSmith - Add counts to audit
                    case eProcesses.relieveIntransit:
                        AuditUtility.AddRelieveIntransitSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    // End Track #5100
                    //BEGIN issue 5117 - stodd - special request
                    case eProcesses.specialRequest:
                        AuditUtility.AddSpecialRequestSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //END issue 5117
                    //BEGIN TT#465 - stodd - Size Day to Week Summary
                    case eProcesses.SizeDayToWeekSummary:
                        AuditUtility.AddSizeDayToWeekSummarySummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#465 - stodd - Size Day to Week Summary
                    // Begin TT#710 - JSmith - Generate relieve intransit
                    case eProcesses.generateRelieveIntransit:
                        AuditUtility.AddGenerateRelieveIntransitSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#710
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    case eProcesses.determineHierarchyActivity:
                        AuditUtility.AddDetermineHierarchyActivitySummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //End TT#988
                    // BEGIN TT#1401 - stodd - add resevation stores (IMO)							
                    case eProcesses.pushToBackStock:
                        AuditUtility.AddPushToBackStock(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                    //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
                    case eProcesses.hierarchyReclass:
                        AuditUtility.AddHierarchyReclassSummary(Convert.ToInt32(summaryRowResult.ProcessRID, CultureInfo.CurrentUICulture), ds);
                        break;
                        //End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  

                }
            }
            return ds;
        }

        private DataSet AfterExecutionForAudit(DataSet ds)
        {
            ds.Tables[0].TableName = "Headers";
            if (auditIncludeSummary && (auditIncludeDetails == false || auditMergeDetails == false))
            {
                DataTable auditSummaryRow = ds.Tables.Add("SummaryRow");
                auditSummaryRow.Locale = ds.Tables[0].Locale;
                auditSummaryRow.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                DataColumn dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "NeedsLoaded";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessID";
                dataColumn.Caption = "ProcessID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Text";
                dataColumn.Caption = "Text";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummaryRow.Columns.Add(dataColumn);

                // add summary counts for load processes
                DataTable auditSummary = ds.Tables.Add("Summary");
                auditSummary.Locale = ds.Tables[0].Locale;
                auditSummary.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummary.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Item";
                dataColumn.Caption = "Item";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummary.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Value";
                dataColumn.Caption = "Value";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditSummary.Columns.Add(dataColumn);

                ds.Relations.Add("SummaryRow", ds.Tables["Headers"].Columns["ProcessRID"], ds.Tables["SummaryRow"].Columns["ProcessRID"]);

                ds.Relations.Add("Summary", ds.Tables["SummaryRow"].Columns["ProcessRID"], ds.Tables["Summary"].Columns["ProcessRID"]);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ds.Tables["SummaryRow"].Rows.Add(new object[] { true, Convert.ToInt32(dr["ProcessRID"]), Convert.ToInt32(dr["ProcessID"], CultureInfo.CurrentUICulture), "Summary" });
                }
            }

            if (auditIncludeDetails && auditMergeDetails == false)
            {
                // add detail header line
                DataTable auditDetailRow = ds.Tables.Add("DetailRow");
                auditDetailRow.Locale = ds.Tables[0].Locale;
                auditDetailRow.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                DataColumn dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "NeedsLoaded";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessID";
                dataColumn.Caption = "ProcessID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Text";
                dataColumn.Caption = "Text";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetailRow.Columns.Add(dataColumn);

                // add detail audit messages
                DataTable auditDetails = ds.Tables.Add("Details");
                auditDetails.Locale = ds.Tables[0].Locale;
                auditDetails.CaseSensitive = ds.Tables[0].CaseSensitive;

                //Create Columns and rows for datatable
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "ProcessRID";
                dataColumn.Caption = "ProcessRID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Time";
                dataColumn.Caption = "Time";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Module";
                dataColumn.Caption = "Module";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "MessageLevel";
                dataColumn.Caption = "MessageLevel";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "MessageLevelText";
                dataColumn.Caption = "Message Level";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "MessageCode";
                dataColumn.Caption = "MessageCode";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Message";
                dataColumn.Caption = "Message";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Message2";
                dataColumn.Caption = "Message Details";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                auditDetails.Columns.Add(dataColumn);

                ds.Relations.Add("DetailRow", ds.Tables["Headers"].Columns["ProcessRID"], ds.Tables["DetailRow"].Columns["ProcessRID"]);
                ds.Relations.Add("Details", ds.Tables["DetailRow"].Columns["ProcessRID"], ds.Tables["Details"].Columns["ProcessRID"]);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ds.Tables["DetailRow"].Rows.Add(new object[] { true, Convert.ToInt32(dr["ProcessRID"], CultureInfo.CurrentUICulture), Convert.ToInt32(dr["ProcessID"], CultureInfo.CurrentUICulture), "Details" });
                }


            }

            return ds;

        }

     
    }
}
