using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public static class filterEngineSQLforAudit
    {
        private static string htab1 = "\t";
        private static int tabLevel = 2;

        private static string GetTab()
        {
            string htab = string.Empty;
            for (int i = 1; i <= tabLevel; i++)
            {
                htab += "\t";
            }
            return htab;
        }

        public static string MakeSqlForFilter(filter f, bool mergeDetails)
        {
            try
            {
                ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq()); //parent seq for conditions
                string sSQL = string.Empty;


                bool firstDynamicDate = false;
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeDateVariableSQLForConditions(cn, ref sSQL, ref firstDynamicDate);
                }

           
                sSQL += htab1 + System.Environment.NewLine;

                if (f.isLimited)
                {
                    sSQL += htab1 + "SELECT TOP " + f.resultLimit.ToString() + " * " + System.Environment.NewLine;
                }
                else
                {
                    sSQL += htab1 + "SELECT * " + System.Environment.NewLine;
                }

                sSQL += htab1 + "FROM (" + System.Environment.NewLine;
                sSQL += htab1 + "SELECT " + System.Environment.NewLine; 
                sSQL += htab1 + "ph.PROCESS_RID AS 'ProcessRID'," + System.Environment.NewLine;  //hidden
                sSQL += htab1 + "ph.PROCESS_ID AS 'ProcessID'," + System.Environment.NewLine;  //hidden       		
		        sSQL += htab1 + "at1.TEXT_VALUE AS 'Process'," + System.Environment.NewLine;
                //sSQL += htab1 + "ph.PROC_DESC AS 'Description'," + System.Environment.NewLine;
                //sSQL += htab1 + "ph.USER_RID," + System.Environment.NewLine; //hidden
                sSQL += htab1 + "au.USER_NAME AS 'User', " + System.Environment.NewLine;
                //sSQL += htab1 + "ph.COMPLETION_STATUS_CODE, " + System.Environment.NewLine; //hidden
                //sSQL += htab1 + "ph.EXECUTION_STATUS_CODE, " + System.Environment.NewLine; //hidden
                //sSQL += htab1 + "(CASE WHEN ph.COMPLETION_STATUS_CODE != 800500 THEN at2.TEXT_VALUE ELSE at3.TEXT_VALUE END ) AS 'Status'," + System.Environment.NewLine; //If completed, use completion code, else use execution code
                sSQL += htab1 + "at3.TEXT_VALUE AS 'Execution Status'," + System.Environment.NewLine;
                sSQL += htab1 + "COALESCE(at2.TEXT_VALUE, 'None') AS 'Completion Status'," + System.Environment.NewLine;
		        sSQL += htab1 + "ph.START_TIME AS 'Start Time'," + System.Environment.NewLine;
		        sSQL += htab1 + "ph.STOP_TIME AS 'Stop Time'," + System.Environment.NewLine;
                //sSQL += htab1 + "(CASE WHEN ph.STOP_TIME IS NOT NULL THEN DATEDIFF(minute, ph.START_TIME, ph.STOP_TIME)  ELSE 0 END ) AS 'DurationMinutes'," + System.Environment.NewLine;
                //Begin TT#4299 -jsobek -Audit Duration is incorrect
                //sSQL += htab1 + "(CASE WHEN ph.STOP_TIME IS NOT NULL THEN RIGHT('00'+ CONVERT(VARCHAR, DATEDIFF(day, ph.START_TIME, ph.STOP_TIME)),2) + ':' + RIGHT('00'+ CONVERT(VARCHAR, DATEDIFF(hour, ph.START_TIME, ph.STOP_TIME)),2) + ':' + RIGHT('00'+ CONVERT(VARCHAR, DATEDIFF(minute, ph.START_TIME, ph.STOP_TIME)),2) + ':' + RIGHT('00'+ CONVERT(VARCHAR, DATEDIFF(second, ph.START_TIME, ph.STOP_TIME)),2) ELSE null END ) AS 'Duration'," + System.Environment.NewLine; //dd.hh:mm:ss
                sSQL += htab1 + "(CASE WHEN ph.STOP_TIME IS NOT NULL THEN [dbo].[UDF_DATE_GET_DURATION_AS_STRING] (ph.START_TIME, ph.STOP_TIME) ELSE null END ) AS 'Duration'," + System.Environment.NewLine; //dd.hh:mm:ss
                //End TT#4299 -jsobek -Audit Duration is incorrect
                //sSQL += htab1 + "at4.TEXT_VALUE AS 'Summary'," + System.Environment.NewLine;
		        sSQL += htab1 + "at5.TEXT_VALUE AS 'Highest Message Level'," + System.Environment.NewLine;
                if (mergeDetails)
                {
                    sSQL += htab1 + "pr.TIME_STAMP AS 'Time'," + System.Environment.NewLine;
		            sSQL += htab1 + "pr.REPORTING_MODULE AS 'Module'," + System.Environment.NewLine;
		            //sSQL += htab1 + "pr.LINE_NUMBER AS 'LineNumber'," + System.Environment.NewLine;
		            //sSQL += htab1 + "pr.MESSAGE_LEVEL AS 'MessageLevelCode'," + System.Environment.NewLine;
                    //sSQL += htab1 + "pr.MESSAGE_LEVEL, " + System.Environment.NewLine; //hidden
		            sSQL += htab1 + "at7.TEXT_VALUE AS 'MessageLevel'," + System.Environment.NewLine;
		            //sSQL += htab1 + "pr.MESSAGE_CODE AS 'MessageCode'," + System.Environment.NewLine;
                    sSQL += htab1 + "(CASE WHEN at6.TEXT_CODE IS NOT NULL THEN CONVERT(VARCHAR, at6.TEXT_CODE) + ': ' + at6.TEXT_VALUE ELSE at6.TEXT_VALUE END) AS 'Message'," + System.Environment.NewLine;
                    sSQL += htab1 + "COALESCE(pr.REPORT_MESSAGE,' ') AS 'Message Details' " + System.Environment.NewLine;
                }
                else //add blank columns here so the result set is the same when saving/applying layouts
                {
                    sSQL += htab1 + "null AS 'Time'," + System.Environment.NewLine;
                    sSQL += htab1 + "null AS 'Module'," + System.Environment.NewLine;
                    sSQL += htab1 + "null AS 'MessageLevel'," + System.Environment.NewLine;
                    sSQL += htab1 + "null AS 'Message'," + System.Environment.NewLine;
                    sSQL += htab1 + "null AS 'Message Details' " + System.Environment.NewLine;
                }

	            sSQL += htab1 + "FROM PROC_HDR ph WITH (NOLOCK) " + System.Environment.NewLine;
                if (mergeDetails)
                {
                    sSQL += htab1 + "INNER JOIN PROC_RPT pr WITH (NOLOCK) ON pr.PROCESS_RID = ph.PROCESS_RID " + System.Environment.NewLine;
		            sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at6 WITH (NOLOCK) ON pr.MESSAGE_CODE = at6.TEXT_CODE " + System.Environment.NewLine;
                    sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at7 WITH (NOLOCK) ON pr.MESSAGE_LEVEL = at7.TEXT_CODE " + System.Environment.NewLine;
                }
		        sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at1 WITH (NOLOCK) ON ph.PROCESS_ID = at1.TEXT_CODE " + System.Environment.NewLine;
		        sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at2 WITH (NOLOCK) ON ph.COMPLETION_STATUS_CODE = at2.TEXT_CODE " + System.Environment.NewLine;
		        sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at3 WITH (NOLOCK) ON ph.EXECUTION_STATUS_CODE = at3.TEXT_CODE " + System.Environment.NewLine;
		        sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at4 WITH (NOLOCK) ON ph.SUMMARY_CODE = at4.TEXT_CODE " + System.Environment.NewLine;
		        sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_TEXT at5 WITH (NOLOCK) ON ph.HIGHEST_LEVEL = at5.TEXT_CODE " + System.Environment.NewLine;
                sSQL += htab1 + "LEFT OUTER JOIN APPLICATION_USER au WITH (NOLOCK) ON ph.USER_RID = au.USER_RID " + System.Environment.NewLine;
                sSQL += htab1 + "WHERE 1=1 " + System.Environment.NewLine;
                //Build the sql for the conditions (inner)
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeSQLForConditions(cn, ref sSQL, mergeDetails);
                }
                sSQL += htab1 + ") AS A1 " + System.Environment.NewLine;
             
              

            

                ConditionNode sortRoot = f.FindConditionNode(f.GetSortByConditionSeq()); //parent seq for conditions
                if (sortRoot.ConditionNodes.Count > 0)
                {
                    sSQL += htab1 + "ORDER BY ";
                }
                else
                {
                    if (mergeDetails)
                    {
                        sSQL += htab1 + htab1 + "ORDER BY [Time] DESC";
                    }
                    else
                    {
                        sSQL += htab1 + htab1 + "ORDER BY [Start Time] DESC";
                    }
                    	
                }
                bool isFirst = true;
                foreach (ConditionNode cn in sortRoot.ConditionNodes)
                {
                    MakeSQLForSortBy(cn, ref sSQL, isFirst);
                    isFirst = false;
                }

                return sSQL;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return string.Empty;
            }
        }
        private static void MakeSQLForConditions(ConditionNode cn, ref string sSQL, bool mergeDetails)
        {
            if (cn.ConditionNodes.Count > 0)
            {
                BuildSqlForLogic(cn.condition, ref sSQL);
                sSQL += "(";
                MakeSQLForCondition(cn, ref sSQL, mergeDetails);
                sSQL += System.Environment.NewLine;
                tabLevel += 1;
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    MakeSQLForConditions(cChild, ref sSQL, mergeDetails);
                }
                sSQL += GetTab() + ")" + System.Environment.NewLine;
                tabLevel -= 1;
            }
            else
            {
                BuildSqlForLogic(cn.condition, ref sSQL);
                MakeSQLForCondition(cn, ref sSQL, mergeDetails);
                sSQL += System.Environment.NewLine;
            }
        }

        private static void MakeSQLForSortBy(ConditionNode cnSort, ref string sSQL, bool isFirst)
        {
            filterSortByTypes sortByType = filterSortByTypes.FromIndex(cnSort.condition.sortByTypeIndex);
            //filterDataTypes dataType;
            //if (sortByType == filterSortByTypes.ProductCharacteristics)
            //{
            //    dataType = filterDataHelper.ProductCharacteristicsGetDataType(fieldIndex);
            //}
            //else 
            if (sortByType == filterSortByTypes.AuditSearchFields)
            {
                if (isFirst == false)
                {
                    sSQL += ", ";
                }

                //dataType = filterProductFieldTypes.GetValueTypeInfoForField(fieldIndex);
                filterAuditSearchTypes sortByField = filterAuditSearchTypes.FromIndex(cnSort.condition.sortByFieldIndex);


                if (sortByField == filterAuditSearchTypes.Duration)
                {
                    sSQL += "DurationMinutes";
                }
                else if (sortByField == filterAuditSearchTypes.HighestMessageLevel)
                {
                    sSQL += "[Highest Message Level]";
                }
                else if (sortByField == filterAuditSearchTypes.ProcessName)
                {
                    sSQL += "Process";
                }
                else if (sortByField == filterAuditSearchTypes.StartTime)
                {
                    sSQL += "[Start Time]";
                }
                else if (sortByField == filterAuditSearchTypes.ExecutionStatus)
                {
                    sSQL += "[Execution Status]";
                }
                else if (sortByField == filterAuditSearchTypes.CompletionStatus)
                {
                    sSQL += "[Completion Status]";
                }
                else if (sortByField == filterAuditSearchTypes.User)
                {
                    sSQL += "User";
                }

                filterSortByDirectionTypes sortByDirection = filterSortByDirectionTypes.FromIndex(cnSort.condition.operatorIndex);
                if (sortByDirection == filterSortByDirectionTypes.Descending)
                {
                    sSQL += " DESC";
                }
            }
  
        }

        private static void MakeSQLForCondition(ConditionNode cn, ref string sSQL, bool mergeDetails)
        {
            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);

            if (et == filterDictionary.AuditStartTime)
            {
                BuildSqlForAuditStartTime(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.AuditDetailTime)
            {
                BuildSqlForAuditDetailTime(cn.condition, ref sSQL, mergeDetails);
            }
            else if (et == filterDictionary.AuditFields)
            {
                BuildSqlForAuditFields(cn.condition, ref sSQL, mergeDetails);
            }
            else if (et == filterDictionary.AuditExecutionStatus)
            {
                BuildSqlForProcessExecutionStatus(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.AuditCompletionStatus)
            {
                BuildSqlForProcessCompletionStatus(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.Users)
            {
                BuildSqlForAuditUser(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.AuditProcessMessageLevel)
            {
                BuildSqlForAuditHighestProcessMessageLevel(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.AuditDetailMessageLevel)
            {
                BuildSqlForAuditHighestDetailMessageLevel(cn.condition, ref sSQL, mergeDetails);
            }
        
          
        }


        private static void BuildSqlForLogic(filterCondition fc, ref string sSQL)
        {
            if (filterLogicTypes.FromIndex(fc.logicIndex) == filterLogicTypes.And)
            {
                sSQL += GetTab() + "AND ";
            }
            else
            {
                sSQL += GetTab() + "OR ";
            }
        }
        private static void BuildSqlForAuditStartTime(filterCondition fc, ref string sSQL)
        {          
            //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            //BuildSqlForDateTimeComparison("ph.START_TIME", fc, ref sSQL);
            if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
            {
                BuildSqlForDateTimeComparison("ph.START_TIME", fc, ref sSQL); //date and time
            }
            else
            {
                BuildSqlForDateTimeComparison("CAST(ph.START_TIME AS DATE)", fc, ref sSQL); //date only
            }
            //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
        }
        private static void BuildSqlForAuditDetailTime(filterCondition fc, ref string sSQL, bool mergeDetails)
        {
            if (mergeDetails)
            {
                //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                //BuildSqlForDateTimeComparison("pr.TIME_STAMP", fc, ref sSQL);
                if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
                {
                    BuildSqlForDateTimeComparison("pr.TIME_STAMP", fc, ref sSQL); //date and time
                }
                else
                {
                    BuildSqlForDateTimeComparison("CAST(pr.TIME_STAMP AS DATE)", fc, ref sSQL); //date only
                }
                //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            }
            else
            {
                sSQL += GetTab() + "ph.PROCESS_RID IN " + System.Environment.NewLine;
                sSQL += GetTab() + "(" + System.Environment.NewLine;
                sSQL += GetTab() + "SELECT pr2.PROCESS_RID FROM PROC_RPT pr2 WITH (NOLOCK) WHERE ";
                //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                //BuildSqlForDateTimeComparison("pr2.TIME_STAMP", fc, ref sSQL);
                if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
                {
                    BuildSqlForDateTimeComparison("pr2.TIME_STAMP", fc, ref sSQL); //date and time
                }
                else
                {
                    BuildSqlForDateTimeComparison("CAST(pr2.TIME_STAMP AS DATE)", fc, ref sSQL); //date only
                    sSQL += Environment.NewLine;  // TT#4602 - JSmith - Syntax error applying Audit filter
                }
                //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                sSQL += GetTab() + ")" + System.Environment.NewLine;
            }
            
        }

        private static void BuildSqlForDateTimeComparison(string val1, filterCondition fc, ref string sSQL)
        {
            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Unrestricted)
            {
                sSQL += "1=1 --Unrestricted date range";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
            {
                sSQL += "(" + val1 + " >= @DT_LAST_24HR_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_NOW_WITH_TIME)";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
            {
                sSQL += "(" + val1 + " >= @DT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_NOW_WITH_TIME)";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
            {
                sSQL += "(" + val1 + " >= @DT_BTWN_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_BTWN_TO_" + GetConditionUID(fc) + ")";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
            {
                sSQL += "(" + val1 + " >= @DT_SPCFY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_SPCFY_TO_" + GetConditionUID(fc) + ")";
            }
        }
        private static void BuildSqlForAuditFields(filterCondition fc, ref string sSQL, bool mergeDetails)
        {
            filterAuditFieldTypes fieldType = filterAuditFieldTypes.FromIndex(fc.fieldIndex);
            if (fieldType == filterAuditFieldTypes.DetailModule)
            {
                if (mergeDetails)
                {
                    BuildSqlForStringComparison("pr.REPORTING_MODULE", fc, ref sSQL);
                }
                else
                {
                    sSQL += GetTab() + "ph.PROCESS_RID IN " + System.Environment.NewLine;
                    sSQL += GetTab() + "(" + System.Environment.NewLine;
                    sSQL += GetTab() + "SELECT pr2.PROCESS_RID FROM PROC_RPT pr2 WITH (NOLOCK) WHERE ";
                    BuildSqlForStringComparison("pr2.REPORTING_MODULE", fc, ref sSQL);
                    sSQL += GetTab() + ")" + System.Environment.NewLine;
                }
                
            }
            else if (fieldType == filterAuditFieldTypes.DetailMessage) 
            {
                if (mergeDetails)
                {
                    BuildSqlForStringComparison("(CASE WHEN at6.TEXT_CODE IS NOT NULL THEN CONVERT(VARCHAR, at6.TEXT_CODE) + ': ' + at6.TEXT_VALUE ELSE at6.TEXT_VALUE END)", fc, ref sSQL);
                }
                else
                {
                    sSQL += GetTab() + "ph.PROCESS_RID IN " + System.Environment.NewLine;
                    sSQL += GetTab() + "(" + System.Environment.NewLine;
                    sSQL += GetTab() + "SELECT pr2.PROCESS_RID FROM PROC_RPT pr2 WITH (NOLOCK) LEFT OUTER JOIN APPLICATION_TEXT at6 WITH (NOLOCK) ON pr2.MESSAGE_CODE = at6.TEXT_CODE WHERE ";
                    BuildSqlForStringComparison("(CASE WHEN at6.TEXT_CODE IS NOT NULL THEN CONVERT(VARCHAR, at6.TEXT_CODE) + ': ' + at6.TEXT_VALUE ELSE at6.TEXT_VALUE END)", fc, ref sSQL);
                    sSQL += GetTab() + ")" + System.Environment.NewLine;
                }
            }
            else if (fieldType == filterAuditFieldTypes.DetailMessageDetails)
            {
                if (mergeDetails)
                {
                    BuildSqlForStringComparison("COALESCE(pr.REPORT_MESSAGE,' ')", fc, ref sSQL);
                }
                else
                {
                    sSQL += GetTab() + "ph.PROCESS_RID IN " + System.Environment.NewLine;
                    sSQL += GetTab() + "(" + System.Environment.NewLine;
                    sSQL += GetTab() + "SELECT pr2.PROCESS_RID FROM PROC_RPT pr2 WITH (NOLOCK) WHERE ";
                    BuildSqlForStringComparison("COALESCE(pr2.REPORT_MESSAGE,' ')", fc, ref sSQL);
                    sSQL += GetTab() + ")" + System.Environment.NewLine;
                }
            }
            //else if (fieldType == filterAuditFieldTypes.ProcessDescription)
            //{
            //    BuildSqlForStringComparison("ph.PROC_DESC", fc, ref sSQL);
            //}
            else if (fieldType == filterAuditFieldTypes.ProcessDuration)
            {
                BuildSqlForIntComparison("(CASE WHEN ph.STOP_TIME IS NOT NULL THEN DATEDIFF(minute, ph.START_TIME, ph.STOP_TIME)  ELSE 0 END )", fc, ref sSQL);
            }
            else if (fieldType == filterAuditFieldTypes.ProcessName)
            {
               // BuildSqlForStringComparison("at1.TEXT_VALUE", fc, ref sSQL);
                BuildSqlForListComparison("ph.PROCESS_ID", fc, ref sSQL, filterListValueTypes.AuditField);
            }
            //else if (fieldType == filterAuditFieldTypes.ProcessStatus)
            //{
            //    BuildSqlForListComparison("ph.COMPLETION_STATUS_CODE", fc, ref sSQL, filterListValueTypes.AuditField);
            //}
            //else if (fieldType == filterAuditFieldTypes.ProcessStatusExecution)
            //{
            //    BuildSqlForListComparison("ph.EXECUTION_STATUS_CODE", fc, ref sSQL, filterListValueTypes.AuditField);
            //}
        }

        private static void BuildSqlForProcessExecutionStatus(filterCondition fc, ref string sSQL)
        {
            //BuildSqlForListComparison("CASE WHEN ph.STOP_TIME IS NULL THEN 0 ELSE 1 END", fc, ref sSQL, filterListValueTypes.AuditExecutionStatus);
            BuildSqlForListComparison("ph.EXECUTION_STATUS_CODE", fc, ref sSQL, filterListValueTypes.AuditExecutionStatus);
        }

        private static void BuildSqlForProcessCompletionStatus(filterCondition fc, ref string sSQL)
        {
            //BuildSqlForListComparison("CASE WHEN ph.STOP_TIME IS NULL THEN 0 ELSE 1 END", fc, ref sSQL, filterListValueTypes.AuditExecutionStatus);
            BuildSqlForListComparison("ph.COMPLETION_STATUS_CODE", fc, ref sSQL, filterListValueTypes.AuditCompletionStatus);
        }

        private static void BuildSqlForAuditUser(filterCondition fc, ref string sSQL)
        {
            BuildSqlForListComparison("ph.USER_RID", fc, ref sSQL, filterListValueTypes.Users);
        }
        private static void BuildSqlForAuditHighestProcessMessageLevel(filterCondition fc, ref string sSQL)
        {
            BuildSqlForListComparison("ph.HIGHEST_LEVEL", fc, ref sSQL, filterListValueTypes.AuditMessageLevel);        
        }

        private static void BuildSqlForAuditHighestDetailMessageLevel(filterCondition fc, ref string sSQL, bool mergeDetails)
        {
            if (mergeDetails)
            {
                BuildSqlForListComparison("pr.MESSAGE_LEVEL", fc, ref sSQL, filterListValueTypes.AuditMessageLevel);
            }
            else
            {
                sSQL += GetTab() + "ph.PROCESS_RID IN " + System.Environment.NewLine;
                sSQL += GetTab() + "(" + System.Environment.NewLine;
                sSQL += GetTab() + "SELECT pr2.PROCESS_RID FROM PROC_RPT pr2 WITH (NOLOCK) WHERE ";
                BuildSqlForListComparison("pr2.MESSAGE_LEVEL", fc, ref sSQL, filterListValueTypes.AuditMessageLevel);
                sSQL += Environment.NewLine;  // TT#4602 - JSmith - Syntax error applying Audit filter
                sSQL += GetTab() + ")" + System.Environment.NewLine;
            }
        }
       
        private static void BuildSqlForListComparison(string field, filterCondition fc, ref string sSQL, filterListValueTypes listValueType)
        {
            filterListConstantTypes listType = fc.listConstantType;
            filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
            if (listType == filterListConstantTypes.All)
            {
                if (listOp == filterListOperatorTypes.Excludes)
                {
                    sSQL += "1=2 --No Users";
                }
                else
                {
                    sSQL += "1=1 --All Users";
                }
            }
            else
            {
                if (listType == filterListConstantTypes.None)
                {
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "1=1 --All Users";
                    }
                    else
                    {
                        sSQL += "1=2 --No Users";
                    }
                }
                else
                {
                    sSQL += field + " ";
                    //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "NOT IN ";
                    }
                    else
                    {
                        sSQL += "IN ";
                    }

                    sSQL += "(";
                    tabLevel++;

                    DataRow[] listValues = fc.GetListValues(listValueType);
                    bool firstStatus = true;
                    foreach (DataRow dr in listValues)
                    {
                        int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

                        if (firstStatus == false)
                        {
                            sSQL += ",";
                        }
                        else
                        {
                            firstStatus = false;
                        }
                        sSQL += listValueIndex.ToString();
                    }

                    tabLevel--;
                    sSQL += GetTab() + ") " + System.Environment.NewLine;
                }
            }
        }

        private static void BuildSqlForStringComparison(string val1, filterCondition fc, ref string sSQL)
        {
            string val2 = fc.valueToCompare;
            //escape val2
            val2 = val2.Replace("'", "''");
            val2 = val2.Replace("[", "[[]");
            val2 = val2.Replace("%", "[%]");
            val2 = val2.Replace("_", "[_]");
            filterStringOperatorTypes stringOp = filterStringOperatorTypes.FromIndex(fc.operatorIndex);
            if (stringOp == filterStringOperatorTypes.Contains)
            {
                sSQL += "UPPER(" + val1 + ") LIKE '%" + val2.ToUpper() + "%'"; 
            }
            else if (stringOp == filterStringOperatorTypes.ContainsExactly)
            {
                sSQL += val1 + " LIKE '%" + val2 + "%'"; 
            }
            else if (stringOp == filterStringOperatorTypes.StartsWith)
            {
                sSQL += "UPPER(" + val1 + ") LIKE '" + val2.ToUpper() + "%'"; 
            }
            else if (stringOp == filterStringOperatorTypes.StartsExactlyWith)
            {
                sSQL += val1 + " LIKE '" + val2 + "%'"; 
            }
            else if (stringOp == filterStringOperatorTypes.EndsWith)
            {
                sSQL += "UPPER(" + val1 + ") LIKE '%" + val2.ToUpper() + "'";
            }
            else if (stringOp == filterStringOperatorTypes.EndsExactlyWith)
            {
                sSQL += val1 + " LIKE '%" + val2 + "'"; 
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqual)
            {
                sSQL += "UPPER(" + val1 + ") = '" + val2.ToUpper() + "'"; 
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqualExactly)
            {
                sSQL += val1 + " = '" + val2 + "'"; 
            }
            else
            {
                sSQL += val1 + " = '" + val2 + "'"; 
            }
            

        }

        private static void BuildSqlForDoubleComparison(string val1, filterCondition fc, ref string sSQL)
        {
            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
            //filterNumericTypes nType = filterNumericTypes.FromIndex(fc.numericTypeIndex);
            double val2 = (double)fc.valueToCompareDouble;
           
            //escape val2
            if (numericOp == filterNumericOperatorTypes.DoesEqual)
            {
                sSQL += val1 + " = " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
            {
                sSQL += val1 + " <> " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThan)
            {
                sSQL += val1 + " > " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
            {
                sSQL += val1 + " >= " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.LessThan)
            {
                sSQL += val1 + " < " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
            {
                sSQL += val1 + " <= " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.Between)
            {
                double val3 = (double)fc.valueToCompareDouble2;
                sSQL += "(" + val1 + " >= " + val2.ToString() + " AND " + val1 + " <= " + val3.ToString() + ") ";
            }


        }
        private static void BuildSqlForIntComparison(string val1, filterCondition fc, ref string sSQL)
        {
            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
            //filterNumericTypes nType = filterNumericTypes.FromIndex(fc.numericTypeIndex);
            int val2 = (int)fc.valueToCompareInt;

            //escape val2
            if (numericOp == filterNumericOperatorTypes.DoesEqual)
            {
                sSQL += val1 + " = " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
            {
                sSQL += val1 + " <> " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThan)
            {
                sSQL += val1 + " > " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
            {
                sSQL += val1 + " >= " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.LessThan)
            {
                sSQL += val1 + " < " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
            {
                sSQL += val1 + " <= " + val2.ToString();
            }
            else if (numericOp == filterNumericOperatorTypes.Between)
            {
                int val3 = (int)fc.valueToCompareInt2;
                sSQL += "(" + val1 + " >= " + val2.ToString() + " AND " + val1 + " <= " + val3.ToString() + ") ";
            }


        }

        private static void MakeDateVariableSQLForConditions(ConditionNode cn, ref string sSQL, ref bool firstDynamicDate)
        {
            if (cn.ConditionNodes.Count > 0)
            {
                //BuildSqlForLogic(cn.condition, ref sSQL);
                //sSQL += "(";
                MakeDateVariableSQLForCondition(cn, ref sSQL, ref firstDynamicDate);
                //sSQL += System.Environment.NewLine;
                //tabLevel += 1;
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    MakeDateVariableSQLForCondition(cChild, ref sSQL, ref firstDynamicDate);
                }
                //sSQL += GetTab() + ")" + System.Environment.NewLine;
                //tabLevel -= 1;
            }
            else
            {
                //BuildSqlForLogic(cn.condition, ref sSQL);
                MakeDateVariableSQLForCondition(cn, ref sSQL, ref firstDynamicDate);
                //sSQL += System.Environment.NewLine;
            }
        }

        private static void MakeDateVariableSQLForCondition(ConditionNode cn, ref string sSQL, ref bool firstDynamicDate)
        {
            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);
            filterCondition fc = cn.condition;
            if (et == filterDictionary.AuditStartTime) //date and time
            {
                BuildSqlDateVariablesForDateTime(fc, ref sSQL, ref firstDynamicDate);
            }
            else if (et == filterDictionary.AuditDetailTime) //date and time
            {
                BuildSqlDateVariablesForDateTime(fc, ref sSQL, ref firstDynamicDate);
            }
        

        }
        private static void BuildSqlDateVariablesForDateTime(filterCondition fc, ref string sSQL, ref bool firstDynamicDate)
        {
            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
            {
                if (firstDynamicDate == false)
                {
                    firstDynamicDate = true;
                    MakeDateDynamicSQL(ref sSQL);
                }
                sSQL += htab1 + "DECLARE @DT_LAST_24HR_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(hh, -24, @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
            {
                if (firstDynamicDate == false)
                {
                    firstDynamicDate = true;
                    MakeDateDynamicSQL(ref sSQL);
                }

                //go from midnight to midnight
                sSQL += htab1 + "DECLARE @DT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, -7, @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
            {
                if (firstDynamicDate == false)
                {
                    firstDynamicDate = true;
                    MakeDateDynamicSQL(ref sSQL);
                }
                int daysFrom = fc.valueToCompareDateBetweenFromDays;
                int daysTo = fc.valueToCompareDateBetweenToDays;

                //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                //sSQL += htab1 + "DECLARE @DT_BTWN_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysFrom.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
                //sSQL += htab1 + "DECLARE @DT_BTWN_TO_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysTo.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
          
                if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
                {
                    sSQL += htab1 + "DECLARE @DT_BTWN_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysFrom.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
                    sSQL += htab1 + "DECLARE @DT_BTWN_TO_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysTo.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
                }
                else
                {
                    sSQL += htab1 + "DECLARE @DT_BTWN_FROM_" + GetConditionUID(fc) + " DATETIME = CAST(DATEADD(dd, " + daysFrom.ToString() + ", CAST(@DT_NOW_WITH_TIME AS DATE)) AS DATE);" + System.Environment.NewLine;
                    sSQL += htab1 + "DECLARE @DT_BTWN_TO_" + GetConditionUID(fc) + " DATETIME = CAST(DATEADD(dd, " + daysTo.ToString() + ", CAST(@DT_NOW_WITH_TIME AS DATE)) AS DATE);" + System.Environment.NewLine;
                }
                //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
            {
                DateTime dateFrom = (DateTime)fc.valueToCompareDateFrom;
                DateTime dateTo = (DateTime)fc.valueToCompareDateTo;
                sSQL += htab1 + "DECLARE @DT_SPCFY_FROM_" + GetConditionUID(fc) + " DATETIME = CONVERT(DATETIME, '" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 120);" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @DT_SPCFY_TO_" + GetConditionUID(fc) + " DATETIME = CONVERT(DATETIME, '" + dateTo.ToString("yyyy-MM-dd HH:mm:ss") + "', 120);" + System.Environment.NewLine;
            }
        }
        private static void MakeDateDynamicSQL(ref string sSQL)
        {
            sSQL += htab1 + "DECLARE @DT_NOW_WITH_TIME DATETIME = GETDATE();" + System.Environment.NewLine;
            //sSQL += htab1 + "DECLARE @SDT_NOW SMALLDATETIME = CONVERT(SMALLDATETIME, CONVERT(CHAR(8), @DT_NOW_WITH_TIME, 112), 112);" + System.Environment.NewLine;
            sSQL += System.Environment.NewLine;
        }

        /// <summary>
        /// Returns a unique identifier for this condition.
        /// Not using RIDs in case the filter is not saved yet on the database, and so stored procedures can be copied more easily, if the need ever arises.
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        private static string GetConditionUID(filterCondition fc)
        {
            return fc.Seq.ToString(); 
        }
    }
}
