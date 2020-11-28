using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{

    /// <summary>
    /// Used for header filters
    /// </summary>
    public static class filterEngineSQL
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

        public static void CreateOrUpdateSqlForFilter(filter f)
        {
            try
            {
                ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq()); //parent seq for conditions
                string procedureName = FilterCommon.BuildFilterProcedureName(f.filterRID, f.filterType);
                string sSQL = string.Empty;
                FilterData dlFilters = new FilterData();
                bool doesProcedureExist = dlFilters.DoesFilterProcedureExist(f.filterRID, f.filterType);
                string filterName = f.filterName;
                //escape val2
                filterName = filterName.Replace("'", "''");
                filterName = filterName.Replace("[", "[[]");
                filterName = filterName.Replace("%", "[%]");
                filterName = filterName.Replace("_", "[_]");

                sSQL += "--dv =============================================" + System.Environment.NewLine;
                sSQL += "--dv Generated date:   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + System.Environment.NewLine;             
                sSQL += "--dv Description:      Returns sorted, limited headers that meet the header filter conditions" + System.Environment.NewLine;
                sSQL += "--dv Flags:            GENERATED_PROCEDURE" + System.Environment.NewLine;
                sSQL += "--dv Filter Name:      " + filterName + System.Environment.NewLine;
                sSQL += "--dv =============================================" + System.Environment.NewLine;


                if (doesProcedureExist)
                {
                    sSQL += "ALTER PROCEDURE [dbo].[" + procedureName + "]" + System.Environment.NewLine;
                }
                else
                {
                    sSQL += "CREATE PROCEDURE [dbo].[" + procedureName + "]" + System.Environment.NewLine;
                }
                //add parameters
                sSQL += htab1 + "@HN_RID_OVERRIDE int = -1," + System.Environment.NewLine;
                sSQL += htab1 + "@USE_WORKSPACE_FIELDS bit = 0" + System.Environment.NewLine;
                sSQL += "AS" + System.Environment.NewLine;
                sSQL += "BEGIN" + System.Environment.NewLine;
                sSQL += htab1 + "SET NOCOUNT ON;" + System.Environment.NewLine;
                sSQL += htab1 + System.Environment.NewLine;

                sSQL += htab1 + "DECLARE @eHeaderType_MultiHeader INT = 800734;" + System.Environment.NewLine; //Multi header parent
                sSQL += htab1 + "DECLARE @eHeaderType_Assortment INT = 800739;" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @eHeaderType_Placeholder INT = 800740;" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @eAssortmentType_GroupAllocation INT = 3;" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @eHeaderAllocationStatus_Released INT = 802709;" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @eHeaderAllocationStatus_ReleasedApproved INT = 802710;" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @eHeaderAllocationStatus_ReceivedOutofBalance INT = 802700;" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @eHeaderAllocationStatus_InUseByMultiHeader INT = 802702;" + System.Environment.NewLine; //Multi header children
                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @PH_RID_OVERRIDE INT = -1;" + System.Environment.NewLine;

                if (f.filterType == filterTypes.HeaderFilter) //Allocation Workspace (AWS) and allocate tasks
                {
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "IF (@USE_WORKSPACE_FIELDS = 0) --Set the product hierarchry in allocate tasks for merchandise override" + System.Environment.NewLine;
                    sSQL += htab1 + "BEGIN" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "SELECT @PH_RID_OVERRIDE = HOME_PH_RID FROM HIERARCHY_NODE hn WITH (NOLOCK) WHERE hn.HN_RID=@HN_RID_OVERRIDE " + System.Environment.NewLine;
                    sSQL += htab1 + "END" + System.Environment.NewLine;
                }


                //Build date variables for conditions so SQL does not perform these calcs per row
                bool firstDynamicDate = false;
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeDateVariableSQLForConditions(cn, ref sSQL, ref firstDynamicDate);
                }

                //Get the product hierarchy rid separately for any child merchandise condition to prevent performance issue
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeProductHierarchyVariableSQLForConditions(cn, ref sSQL);
                }


                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @HDR_RIDS AS HDR_RID_TYPE;" + System.Environment.NewLine;
                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO @HDR_RIDS" + System.Environment.NewLine;
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                //if (f.isLimited)
                //{
                //    sSQL += htab1 + "SELECT TOP " + f.resultLimit.ToString() + System.Environment.NewLine;
                //}
                //else
                //{
                //    sSQL += htab1 + "SELECT " + System.Environment.NewLine;
                //}
                //sSQL += htab1 + htab1 + "HDR_RID" + System.Environment.NewLine;
                //sSQL += htab1 + "FROM [dbo].[HEADER] h WITH (NOLOCK)" + System.Environment.NewLine;
                //sSQL += htab1 + "WHERE HDR_RID <> 1" + System.Environment.NewLine;
                if (f.isLimited)
                {
                    sSQL += htab1 + "SELECT DISTINCT TOP " + f.resultLimit.ToString() + System.Environment.NewLine;
                }
                else
                {
                    sSQL += htab1 + "SELECT DISTINCT" + System.Environment.NewLine;
                }
                sSQL += htab1 + htab1 + "h.HDR_RID" + System.Environment.NewLine;
                sSQL += htab1 + "FROM [dbo].[HEADER] h WITH (NOLOCK)" + System.Environment.NewLine;
                bool bCheckMasterID = false;
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);
                    if (et == filterDictionary.HeaderFields)
                    {
                        filterHeaderFieldTypes fieldType = filterHeaderFieldTypes.FromIndex(cn.condition.fieldIndex);
                        if (fieldType == filterHeaderFieldTypes.MasterHeaderID)
                        {
                            bCheckMasterID = true;
                        }
                    }
                }
                if (bCheckMasterID)
                {
                    sSQL += htab1 + "LEFT OUTER JOIN [dbo].[MASTER_HEADER] mh WITH (NOLOCK) on mh.MASTER_HDR_RID = h.HDR_RID" + System.Environment.NewLine;
                    sSQL += htab1 + "LEFT OUTER JOIN [dbo].[HEADER] m WITH (NOLOCK) on m.HDR_RID = mh.MASTER_HDR_RID" + System.Environment.NewLine;
                    sSQL += htab1 + "LEFT OUTER JOIN [dbo].[MASTER_HEADER] sh WITH (NOLOCK) on sh.SUBORD_HDR_RID = h.HDR_RID" + System.Environment.NewLine;
                    sSQL += htab1 + "LEFT OUTER JOIN [dbo].[HEADER] s WITH (NOLOCK) on s.HDR_RID = sh.MASTER_HDR_RID" + System.Environment.NewLine;
                }
                sSQL += htab1 + "WHERE h.HDR_RID <> 1" + System.Environment.NewLine;
                // End TT#1966-MD - JSmith - DC Fulfillment
                if (f.filterType == filterTypes.HeaderFilter) //Allocation Workspace (AWS) and allocate tasks
                {
                    sSQL += htab1 + htab1 + "AND h.DISPLAY_TYPE NOT IN (@eHeaderType_Assortment, @eHeaderType_Placeholder) --Never show assortments or place holders in the AWS or allocate tasks" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "AND h.DISPLAY_STATUS NOT IN (@eHeaderAllocationStatus_InUseByMultiHeader) --Never consider multi-header children when applying filter conditions - children are added later" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "AND (@USE_WORKSPACE_FIELDS = 1 OR h.DISPLAY_STATUS NOT IN (@eHeaderAllocationStatus_Released, @eHeaderAllocationStatus_ReleasedApproved, @eHeaderAllocationStatus_ReceivedOutofBalance, @eHeaderAllocationStatus_InUseByMultiHeader)) --Never include released, released approved, out of balance, or multi-header children headers in allocate tasks" + System.Environment.NewLine;
                    // Begin TT#5033 - JSmith - Database Error During Auto Allocation
                    ////Add additional where clause for the override merchandise node if it was supplied for an allocate task
                    //sSQL += htab1 + htab1 + "AND (@USE_WORKSPACE_FIELDS = 1 OR @HN_RID_OVERRIDE = -1 OR h.HDR_RID IN" + System.Environment.NewLine;
                    //sSQL += GetTab() + "(" + System.Environment.NewLine;
                    //tabLevel++;
                    //if (organizationalPhRID == null)
                    //{
                    //    MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                    //    organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                    //}
                    //sSQL += GetTab() + "SELECT HDR_RID FROM [dbo].[UDF_HIERARCHY_GET_HEADERS_FROM_NODE] (@HN_RID_OVERRIDE, @PH_RID_OVERRIDE," + organizationalPhRID.ToString() + ")  ";
                    //sSQL += System.Environment.NewLine;
                    //tabLevel--;
                    //sSQL += GetTab() + ")) " + System.Environment.NewLine;
                    // End TT#5033 - JSmith - Database Error During Auto Allocation
                }
                else if (f.filterType == filterTypes.AssortmentFilter) //Assortment Filter for Assortment Workspace (SWS)
                {
                    sSQL += htab1 + htab1 + "AND h.ASRT_TYPE <> @eAssortmentType_GroupAllocation AND h.DISPLAY_TYPE IN (@eHeaderType_Assortment) --Only show assortments" + System.Environment.NewLine;
                }

            
                
                //Build the sql for the conditions
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeSQLForConditions(cn, ref sSQL);
                }

                if (f.filterType == filterTypes.HeaderFilter) //Allocation Workspace (AWS) and allocate tasks
                {
                    // Begin TT#5033 - JSmith - Database Error During Auto Allocation
                    //Add additional logic for the override merchandise node if it was supplied for an allocate task
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "IF (@HN_RID_OVERRIDE != -1) " + System.Environment.NewLine;
                    sSQL += htab1 + "BEGIN " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "DECLARE @NODE_HDR_RIDS AS HDR_RID_TYPE" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "DECLARE @HDR_RIDS2 AS HDR_RID_TYPE" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "INSERT INTO @HDR_RIDS2 SELECT HDR_RID FROM @HDR_RIDS" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "DELETE FROM @HDR_RIDS" + System.Environment.NewLine;
                    if (organizationalPhRID == null)
                    {
                        MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                        organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                    }
                    sSQL += htab1 + htab1 + "INSERT INTO @NODE_HDR_RIDS SELECT HDR_RID FROM [dbo].[UDF_HIERARCHY_GET_HEADERS_FROM_NODE] (@HN_RID_OVERRIDE, @PH_RID_OVERRIDE," + organizationalPhRID.ToString() + ")  " + System.Environment.NewLine; ;
                    sSQL += htab1 + htab1 + "INSERT INTO @HDR_RIDS  " + System.Environment.NewLine; ;
                    sSQL += htab1 + htab1 + "SELECT hr.HDR_RID FROM @HDR_RIDS2 hr  " + System.Environment.NewLine; ;
                    sSQL += htab1 + htab1 + "  JOIN @NODE_HDR_RIDS nhr ON nhr.HDR_RID = hr.HDR_RID  " + System.Environment.NewLine; ;
                    sSQL += htab1 + "END " + System.Environment.NewLine;
                    // End TT#5033 - JSmith - Database Error During Auto Allocation

                    //Add remaining multi-header children from multi-headers that passed the conditions 
                    //Assuming multi-header children are not filtered by conditions
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "IF (@USE_WORKSPACE_FIELDS = 1) --Add remaining multi-header children from multi-headers that passed the conditions" + System.Environment.NewLine;
                    sSQL += htab1 + "BEGIN" + System.Environment.NewLine;
                    //Begin TT#1368-MD -jsobek -Header Filters - Remove InUsebyMulti status option
                    //sSQL += htab1 + htab1 + "INSERT INTO @HDR_RIDS" + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + "SELECT HDR_RID FROM HEADER WITH (NOLOCK) " + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + "WHERE HDR_GROUP_RID IN " + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + "( " + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + htab1 + "SELECT HDR_GROUP_RID FROM HEADER WITH (NOLOCK) WHERE HDR_GROUP_RID <> 1 AND HDR_RID IN (SELECT HDR_RID FROM @HDR_RIDS) " + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + ") " + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + "AND HDR_RID NOT IN (SELECT HDR_RID FROM @HDR_RIDS) " + System.Environment.NewLine;
                    //sSQL += htab1 + htab1 + "AND DISPLAY_TYPE NOT IN (@eHeaderType_Assortment, @eHeaderType_Placeholder) --Never show assortments or place holders in the AWS or allocate tasks" + System.Environment.NewLine;

                    sSQL += htab1 + htab1 + "INSERT INTO @HDR_RIDS" + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "SELECT HDR_RID FROM HEADER WITH (NOLOCK) " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "WHERE HDR_GROUP_RID <> 1 AND HDR_GROUP_RID IN " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "( " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + htab1 + "SELECT HDR_RID FROM @HDR_RIDS " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + ") " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "AND HDR_RID NOT IN (SELECT HDR_RID FROM @HDR_RIDS) " + System.Environment.NewLine;
                    sSQL += htab1 + htab1 + "AND DISPLAY_TYPE NOT IN (@eHeaderType_Assortment, @eHeaderType_Placeholder) --Never show assortments or place holders in the AWS or allocate tasks" + System.Environment.NewLine;
                    //End TT#1368-MD -jsobek -Header Filters - Remove InUsebyMulti status option

                    sSQL += htab1 + "END" + System.Environment.NewLine;
                }

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "IF (@USE_WORKSPACE_FIELDS = 1)" + System.Environment.NewLine;
                sSQL += htab1 + "BEGIN" + System.Environment.NewLine;
                sSQL += htab1 + htab1 + "SELECT * FROM (SELECT * FROM [dbo].[UDF_HEADER_READ_FOR_WORKSPACE] (@HDR_RIDS) ) AS h" + System.Environment.NewLine;       //TT#1477-MD -jsobek -Header Filter Sort on Workspace

                //Begin TT#1477-MD -jsobek -Header Filter Sort on Workspace
                ConditionNode sortRoot = f.FindConditionNode(f.GetSortByConditionSeq()); //parent seq for conditions
                if (sortRoot.ConditionNodes.Count > 0)
                {
                    sSQL += htab1 + htab1 + "ORDER BY ";
                }
                else
                {
                    //always provide a default order by for workspace
                    sSQL += htab1 + htab1 + "ORDER BY h.HDR_ID ";
                }

                bool isFirst = true;
                foreach (ConditionNode cn in sortRoot.ConditionNodes)
                {
                    MakeSQLForSortBy(cn, ref sSQL, isFirst);
                    isFirst = false;
                }
                sSQL += System.Environment.NewLine;
                //End TT#1477-MD -jsobek -Header Filter Sort on Workspace

                sSQL += htab1 + "END" + System.Environment.NewLine;
                sSQL += htab1 + "ELSE" + System.Environment.NewLine;
                sSQL += htab1 + "BEGIN" + System.Environment.NewLine;
                //Begin TT#1468-MD -jsobek -Header Filter Sort Options
                //sSQL += htab1 + htab1 + "SELECT * FROM [dbo].[UDF_HEADER_READ_FOR_TASKLIST] (@HDR_RIDS)" + System.Environment.NewLine;

                sSQL += htab1 + htab1 + "SELECT " + System.Environment.NewLine;
                sSQL += htab1 + htab1 + "HDR_RID, " + System.Environment.NewLine;
                sSQL += htab1 + htab1 + "HDR_ID " + System.Environment.NewLine;
                sSQL += htab1 + htab1 + "FROM HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                sSQL += htab1 + htab1 + "WHERE h.HDR_RID IN (SELECT HDR_RID FROM @HDR_RIDS) " + System.Environment.NewLine;
                sortRoot = f.FindConditionNode(f.GetSortByConditionSeq()); //parent seq for conditions
                if (sortRoot.ConditionNodes.Count > 0)
                {
                    sSQL += htab1 + htab1 + "ORDER BY ";
                }

                isFirst = true;
                foreach (ConditionNode cn in sortRoot.ConditionNodes)
                {
                    MakeSQLForSortBy(cn, ref sSQL, isFirst);
                    isFirst = false;
                }
                sSQL += System.Environment.NewLine;
                //End TT#1468-MD -jsobek -Header Filter Sort Options
                sSQL += htab1 + "END" + System.Environment.NewLine;
               
                sSQL += htab1 + System.Environment.NewLine;
                sSQL += "END" + System.Environment.NewLine;

                //Debug.WriteLine(" ");
                //Debug.WriteLine("ExecuteFilter() SQL: " + sSQL); 

                dlFilters.ExecuteCreateOrAlterFilterProcedure(sSQL);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        //Begin TT#1468-MD -jsobek -Header Filter Sort Options
        private static void MakeSQLForSortBy(ConditionNode cnSort, ref string sSQL, bool isFirst)
        {
            if (isFirst == false)
            {
                sSQL += ", ";
            }


            filterSortByTypes sortByType = filterSortByTypes.FromIndex(cnSort.condition.sortByTypeIndex);
            
            if (sortByType == filterSortByTypes.HeaderDate)
            {
                sSQL += "h.HDR_DAY";
            }
            else if (sortByType == filterSortByTypes.HeaderCharacteristics)
            {
                MakeSQLForSortByHeaderCharacteristics(cnSort.condition.sortByFieldIndex, ref sSQL);
            }
            else if (sortByType == filterSortByTypes.HeaderStatus)
            {
                sSQL += "(SELECT at.TEXT_VALUE FROM APPLICATION_TEXT at WITH (NOLOCK) WHERE at.TEXT_CODE= h.DISPLAY_STATUS)";
            }
            else if (sortByType == filterSortByTypes.HeaderFields)
            {

                

                filterHeaderFieldTypes sortByField = filterHeaderFieldTypes.FromIndex(cnSort.condition.sortByFieldIndex);
                MakeSQLForSortByHeaderFields(sortByField, ref sSQL);

                
            }

            filterSortByDirectionTypes sortByDirection = filterSortByDirectionTypes.FromIndex(cnSort.condition.operatorIndex);
            if (sortByDirection == filterSortByDirectionTypes.Descending)
            {
                sSQL += " DESC";
            }
        }
        private static void MakeSQLForSortByHeaderCharacteristics(int fieldIndex, ref string sSQL)
        {
            filterDataTypes filterDataType = filterDataHelper.HeaderCharacteristicsGetDataType(fieldIndex);
            filterValueTypes valueType = filterDataType.valueType;
            if (valueType == filterValueTypes.Date || valueType == filterValueTypes.Dollar || valueType == filterValueTypes.Numeric || valueType == filterValueTypes.Text)
            {
                int hcg_rid = fieldIndex; //corresponds to HEADER_CHAR_GROUP HCG_RID, example 7=Fabric
                sSQL += "(SELECT ";
                if (valueType == filterValueTypes.Date)
                {
                    sSQL += "hc.DATE_VALUE ";
                }
                else if (valueType == filterValueTypes.Dollar)
                {
                    sSQL += "hc.DOLLAR_VALUE ";
                }
                else if (valueType == filterValueTypes.Numeric)
                {
                    sSQL += "hc.NUMBER_VALUE ";
                }
                else if (valueType == filterValueTypes.Text)
                {
                    sSQL += "hc.TEXT_VALUE ";
                }
                sSQL += "FROM HEADER_CHAR_JOIN hcj WITH (NOLOCK) INNER JOIN HEADER_CHAR hc WITH (NOLOCK) ON hc.HC_RID=hcj.HC_RID ";
                sSQL += "WHERE hcj.HDR_RID=h.HDR_RID AND hc.HCG_RID=" + hcg_rid.ToString() + ")";

            }
            else if (valueType == filterValueTypes.List)
            {
                int hcg_rid = fieldIndex;
                sSQL += "(SELECT ";
                sSQL += "CASE ";
                sSQL += "WHEN (SELECT HCG_TYPE FROM HEADER_CHAR_GROUP WITH (NOLOCK) WHERE HCG_RID=" + hcg_rid.ToString() + ")=1 THEN CONVERT(varchar(50), hc.DATE_VALUE) ";
                sSQL += "WHEN (SELECT HCG_TYPE FROM HEADER_CHAR_GROUP WITH (NOLOCK) WHERE HCG_RID=" + hcg_rid.ToString() + ")=2 THEN CONVERT(varchar(50), hc.NUMBER_VALUE) ";
                sSQL += "WHEN (SELECT HCG_TYPE FROM HEADER_CHAR_GROUP WITH (NOLOCK) WHERE HCG_RID=" + hcg_rid.ToString() + ")=3 THEN CONVERT(varchar(50), hc.DOLLAR_VALUE) ";
                sSQL += "ELSE hc.TEXT_VALUE ";
                sSQL += "END ";
                sSQL += "FROM HEADER_CHAR_JOIN hcj WITH (NOLOCK) INNER JOIN HEADER_CHAR hc WITH (NOLOCK) ON hc.HC_RID=hcj.HC_RID ";
                sSQL += "WHERE hcj.HDR_RID=h.HDR_RID AND hc.HCG_RID=" + hcg_rid.ToString() + ")";
            }
        }
        private static void MakeSQLForSortByHeaderFields(filterHeaderFieldTypes fieldType,  ref string sSQL)
        {
  
            if (fieldType == filterHeaderFieldTypes.HeaderID)
            {
                sSQL += "h.HDR_ID";
            }
            else if (fieldType == filterHeaderFieldTypes.PO) //purchase order
            {
                sSQL += "h.PURCHASE_ORDER";
            }
            else if (fieldType == filterHeaderFieldTypes.Vendor)
            {
                sSQL += "h.VENDOR";
            }
            else if (fieldType == filterHeaderFieldTypes.DC) //distribution center
            {
                sSQL += "h.DIST_CENTER";
            }
            else if (fieldType == filterHeaderFieldTypes.VswID) //aka IMO ID
            {
                sSQL += "h.IMO_ID";
            }
            else if (fieldType == filterHeaderFieldTypes.ShipStatus)
            {
                sSQL += "(SELECT at.TEXT_VALUE FROM APPLICATION_TEXT at WITH (NOLOCK) WHERE at.TEXT_CODE= h.DISPLAY_SHIP_STATUS)";
            }
            else if (fieldType == filterHeaderFieldTypes.IntransitStatus)
            {
                sSQL += "(SELECT at.TEXT_VALUE FROM APPLICATION_TEXT at WITH (NOLOCK) WHERE at.TEXT_CODE= h.DISPLAY_INTRANSIT)";
            }
            //else if (fieldType == filterHeaderFieldTypes.MultiHeaderID)
            //{
            //    //The multi-header ID is just the HDR_ID for the master header in multi-header group
            //    sSQL += "(SELECT h2.HDR_ID FROM HEADER h2 WITH (NOLOCK) WHERE DISPLAY_TYPE=800734 AND h2.HDR_RID=h.HDR_RID)";
            //}
            else if (fieldType == filterHeaderFieldTypes.QuantityToAllocate)
            {
                //The multi-header ID is just the HDR_ID for the master header in multi-header group
                sSQL += "h.UNITS_RECEIVED";
            }
            else if (fieldType == filterHeaderFieldTypes.SubClass) //Parent of style base node
            {
                if (organizationalPhRID == null)
                {
                    MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                    organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                }
                string nodeDisplay = "(SELECT [dbo].[UDF_MID_GET_NODE_DISPLAY] ((SELECT PARENT_HN_RID FROM HIER_NODE_JOIN hnj WITH (NOLOCK) WHERE hnj.HN_RID=h.STYLE_HNRID AND hnj.PH_RID=" + organizationalPhRID + ")))";
                sSQL += nodeDisplay;
            }
            else if (fieldType == filterHeaderFieldTypes.Style)
            {
                string nodeDisplay = "(SELECT [dbo].[UDF_MID_GET_NODE_DISPLAY] (h.STYLE_HNRID))";
                sSQL += nodeDisplay;
            }
            else if (fieldType == filterHeaderFieldTypes.NumPacks)
            {
                sSQL += "(SELECT COUNT(*) FROM HEADER_PACK hp WITH (NOLOCK) WHERE hp.HDR_RID = h.HDR_RID)";
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkColors)
            {
                sSQL += "(SELECT COUNT(*) FROM HEADER_BULK_COLOR hc WITH (NOLOCK) WHERE hc.HDR_RID = h.HDR_RID)";
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkSizes)
            {
                sSQL += "(SELECT COUNT(DISTINCT SIZE_CODE_RID) FROM HEADER_BULK_COLOR_SIZE hs WITH (NOLOCK) WHERE hs.HDR_RID = h.HDR_RID AND UNITS > 0)";
            }
            else if (fieldType == filterHeaderFieldTypes.SizeGroup)
            {
                sSQL += "(SELECT sg.SIZE_GROUP_NAME FROM SIZE_GROUP sg WITH (NOLOCK) WHERE sg.SIZE_GROUP_RID = h.SIZE_GROUP_RID)";
            }
            else if (fieldType == filterHeaderFieldTypes.VswProcess)
            {
                sSQL += "(CASE WHEN ([dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,19) = 1 AND [dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,20) = 1) THEN 'Adjust' ";
                sSQL += "WHEN ([dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,19) = 1 AND [dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,20) = 0) THEN 'Replace' ";
	            sSQL += "WHEN [dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,19) = 0 THEN 'None' ";
	            sSQL += "ELSE null ";
                sSQL += "END) ";
            }
        }

        //End TT#1468-MD -jsobek -Header Filter Sort Options

        private static void MakeSQLForConditions(ConditionNode cn, ref string sSQL)
        {
            if (cn.ConditionNodes.Count > 0)
            {
                BuildSqlForLogic(cn.condition, ref sSQL);
                sSQL += "(";
                MakeSQLForCondition(cn, ref sSQL);
                sSQL += System.Environment.NewLine;
                tabLevel += 1;
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    MakeSQLForConditions(cChild, ref sSQL);
                }
                sSQL += GetTab() + ")" + System.Environment.NewLine;
                tabLevel -= 1;
            }
            else
            {
                BuildSqlForLogic(cn.condition, ref sSQL);
                MakeSQLForCondition(cn, ref sSQL);
                sSQL += System.Environment.NewLine;
            }
        }

        private static void MakeSQLForCondition(ConditionNode cn, ref string sSQL)
        {
            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);
            if (et == filterDictionary.HeaderFields)
            {
                BuildSqlForHeaderFields(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.HeaderCharacteristics)
            {
                BuildSqlForHeaderCharacteristics(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.HeaderDate)
            {
                BuildSqlForHeaderDate(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.HeaderReleaseDate)
            {
                BuildSqlForHeaderReleaseDate(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.HeaderStatus)
            {
                BuildSqlForHeaderStatus(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.HeaderTypes)
            {
                BuildSqlForHeaderTypes(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.HeaderMerchandise)
            {
                BuildSqlForHeaderMerchandise(cn.condition, ref sSQL);
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
                    // Begin TT#4608 - JSmith - Syntax error when creating filter with nested conditions
                    //MakeDateVariableSQLForCondition(cChild, ref sSQL, ref firstDynamicDate);
                    MakeDateVariableSQLForConditions(cChild, ref sSQL, ref firstDynamicDate);
                    // End TT#4608 - JSmith - Syntax error when creating filter with nested conditions
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
            if (et == filterDictionary.HeaderDate) //date only
            {
                BuildSqlDateVariablesForSmallDate(fc, ref sSQL, ref firstDynamicDate);
            }
            else if (et == filterDictionary.HeaderReleaseDate) //date and time
            {
                BuildSqlDateVariablesForDateTime(fc, ref sSQL, ref firstDynamicDate);
            }
            else if (et == filterDictionary.HeaderCharacteristics)
            {
                  filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
                  if (valueType == filterValueTypes.Date)
                  {
                      BuildSqlDateVariablesForSmallDate(fc, ref sSQL, ref firstDynamicDate);
                  }
            }
    
        }



        private static void MakeProductHierarchyVariableSQLForConditions(ConditionNode cn, ref string sSQL)
        {
            if (cn.ConditionNodes.Count > 0)
            {
                //BuildSqlForLogic(cn.condition, ref sSQL);
                //sSQL += "(";
                MakeProductHierarchyForCondition(cn, ref sSQL);
                //sSQL += System.Environment.NewLine;
                //tabLevel += 1;
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    MakeProductHierarchyForCondition(cChild, ref sSQL);
                }
                //sSQL += GetTab() + ")" + System.Environment.NewLine;
                //tabLevel -= 1;
            }
            else
            {
                //BuildSqlForLogic(cn.condition, ref sSQL);
                MakeProductHierarchyForCondition(cn, ref sSQL);
                //sSQL += System.Environment.NewLine;
            }
        }
        private static void MakeProductHierarchyForCondition(ConditionNode cn, ref string sSQL)
        {
            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);
            filterCondition fc = cn.condition;
            if (et == filterDictionary.HeaderMerchandise) 
            {
                sSQL += htab1 + "DECLARE @PH_RID_" + GetConditionUID(fc) + " INT = (SELECT HOME_PH_RID FROM HIERARCHY_NODE hn WITH (NOLOCK) WHERE hn.HN_RID=" + cn.condition.headerMerchandise_HN_RID.ToString() + ");" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @HDR_RIDS_FROM_HIERARCHY_" + GetConditionUID(fc) + " AS HDR_RID_TYPE;" + System.Environment.NewLine;
                if (organizationalPhRID == null)
                {
                    MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                    organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                }
                sSQL += htab1 + "INSERT INTO @HDR_RIDS_FROM_HIERARCHY_" + GetConditionUID(fc) + " SELECT * FROM UDF_HIERARCHY_GET_HEADERS_FROM_NODE (" + fc.headerMerchandise_HN_RID.ToString() + ", @PH_RID_" + GetConditionUID(fc) + ", " + organizationalPhRID.ToString() + ")" + System.Environment.NewLine;
            }
      

        }


        private static void MakeDateDynamicSQL(ref string sSQL)
        {
            sSQL += htab1 + "DECLARE @DT_NOW_WITH_TIME DATETIME = GETDATE();" + System.Environment.NewLine;
            sSQL += htab1 + "DECLARE @SDT_NOW SMALLDATETIME = CONVERT(SMALLDATETIME, CONVERT(CHAR(8), @DT_NOW_WITH_TIME, 112), 112);" + System.Environment.NewLine;
            sSQL += System.Environment.NewLine;
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
        private static void BuildSqlForHeaderFields(filterCondition fc, ref string sSQL)
        {
            filterHeaderFieldTypes fieldType = filterHeaderFieldTypes.FromIndex(fc.fieldIndex); 
            if (fieldType == filterHeaderFieldTypes.HeaderID)
            {
                BuildSqlForStringComparison("h.HDR_ID", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.PO) //purchase order
            {
                BuildSqlForStringComparison("h.PURCHASE_ORDER", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.Vendor)
            {
                BuildSqlForStringComparison("h.VENDOR", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.DC) //distribution center
            {
                BuildSqlForStringComparison("h.DIST_CENTER", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.VswID) //aka IMO ID
            {
                BuildSqlForStringComparison("h.IMO_ID", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.ShipStatus) 
            {
                //sSQL += "h.HDR_RID ";
                //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                //if (listOp == filterListOperatorTypes.Excludes)
                //{
                //    sSQL += "NOT IN ";
                //}
                //else 
                //{
                //    sSQL += "IN ";
                //}
                 
                //sSQL += "( "+ System.Environment.NewLine;
                //tabLevel++;
                
                //sSQL += GetTab() + "SELECT HDR_RID FROM " + System.Environment.NewLine;
                //sSQL += GetTab() + "( " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "SELECT HDR_RID, " + System.Environment.NewLine;
                //sSQL += GetTab() + "CASE " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "WHEN (cast((SHIPPING_STATUS_FLAGS & 0x0002) as bit) = 1) THEN 802733 --Shipped " + System.Environment.NewLine;
                //sSQL += GetTab() + "WHEN (cast((SHIPPING_STATUS_FLAGS & 0x0004) as bit) = 1) THEN 802732 --OnHold " + System.Environment.NewLine;
                //sSQL += GetTab() + "WHEN (cast((SHIPPING_STATUS_FLAGS & 0x0001) as bit) = 1) THEN 802731 --Partial " + System.Environment.NewLine;
                //sSQL += GetTab() + "ELSE 802730 --Not Shipped " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "END AS SHIP_STATUS " + System.Environment.NewLine;
                //sSQL += GetTab() + "FROM HEADER WITH (NOLOCK) " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + ") as ship1 WHERE ship1.SHIP_STATUS IN (";
                //DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderField);
                //bool firstStatus = true;
                //foreach (DataRow dr in listValues)
                //{
                //    int listValueIndex = (int)dr["LIST_VALUE_INDEX"];
         
                //    if (firstStatus == false)
                //    {
                //        sSQL += ",";
                //    }
                //    else
                //    {
                //        firstStatus = false;
                //    }
                //    sSQL += listValueIndex.ToString();
                //}
                //sSQL += ")" + System.Environment.NewLine;

                //tabLevel--;
                //sSQL +=GetTab() + ") "+ System.Environment.NewLine;
                    filterListConstantTypes listType = fc.listConstantType;
                    filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                    if (listType == filterListConstantTypes.All)
                    {
                        if (listOp == filterListOperatorTypes.Excludes)
                        {
                            sSQL += "1=2 --No Ship Statuses";
                        }
                        else
                        {
                            sSQL += "1=1 --All Ship Statuses";
                        }
                    }
                    else
                    {
                        if (listType == filterListConstantTypes.None)
                        {
                            if (listOp == filterListOperatorTypes.Excludes)
                            {
                                sSQL += "1=1 --All Ship Statuses";
                            }
                            else
                            {
                                sSQL += "1=2 --No Ship Statuses";
                            }
                        }
                        else
                        {
                            sSQL += "h.DISPLAY_SHIP_STATUS ";
                            //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                            if (listOp == filterListOperatorTypes.Excludes)
                            {
                                sSQL += "NOT IN ";
                            }
                            else
                            {
                                sSQL += "IN ";
                            }

                            sSQL += "( ";


                            DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderField);
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
            else if (fieldType == filterHeaderFieldTypes.IntransitStatus)
            {
                //sSQL += "h.HDR_RID ";
                //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                //if (listOp == filterListOperatorTypes.Excludes)
                //{
                //    sSQL += "NOT IN ";
                //}
                //else
                //{
                //    sSQL += "IN ";
                //}

                //sSQL += "( " + System.Environment.NewLine;
                //tabLevel++;

                //sSQL += GetTab() + "SELECT HDR_RID FROM " + System.Environment.NewLine;
                //sSQL += GetTab() + "( " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "SELECT " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "HDR_RID, " + System.Environment.NewLine;
                //sSQL += GetTab() + "CASE WHEN (cast((INTRANSIT_STATUS_FLAGS & 0x0001) as bit) = 1) THEN --StyleIntransitUpdated " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "CASE WHEN (PackCount = 0 AND ColorCount = 0) THEN " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "802751 --Intransit By Sku aka SKU " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "ELSE " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "CASE WHEN (cast((INTRANSIT_STATUS_FLAGS & 0x0004) as bit) = 1) THEN --BulkSizeIntransitUpdated " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "802753 --IntransitByBulkSize aka Style and Size " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "ELSE " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "802752 --IntransitByStyle aka Style " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "END " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "END " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "ELSE " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "802750 --Not Intransit " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "END AS INTRANSIT_STATUS " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                //sSQL += GetTab() + "( " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "SELECT " + System.Environment.NewLine;
                //tabLevel++;
                //sSQL += GetTab() + "HDR_RID, " + System.Environment.NewLine;
                //sSQL += GetTab() + "INTRANSIT_STATUS_FLAGS, " + System.Environment.NewLine;
                //sSQL += GetTab() + "(SELECT COUNT(*) FROM HEADER_PACK hp WHERE hp.HDR_RID = h.HDR_RID) AS PackCount, " + System.Environment.NewLine;
                //sSQL += GetTab() + "(SELECT COUNT(*) FROM HEADER_BULK_COLOR hc WHERE hc.HDR_RID = h.HDR_RID) AS ColorCount " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + "FROM HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + ") as instransit1 " + System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + ") as intransit2 WHERE intransit2.INTRANSIT_STATUS IN (";
                //DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderField);
                //bool firstStatus = true;
                //foreach (DataRow dr in listValues)
                //{
                //    int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

                //    if (firstStatus == false)
                //    {
                //        sSQL += ",";
                //    }
                //    else
                //    {
                //        firstStatus = false;
                //    }
                //    sSQL += listValueIndex.ToString();
                //}
                //sSQL += ")" + System.Environment.NewLine;

                //tabLevel--;
                //sSQL += GetTab() + ") " + System.Environment.NewLine;
                 filterListConstantTypes listType = fc.listConstantType;
                 filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                 if (listType == filterListConstantTypes.All)
                 {
                     if (listOp == filterListOperatorTypes.Excludes)
                     {
                         sSQL += "1=2 --No Intransit Statuses";
                     }
                     else
                     {
                         sSQL += "1=1 --All Intransit Statuses";
                     }
                 }
                 else
                 {
                     if (listType == filterListConstantTypes.None)
                     {
                         if (listOp == filterListOperatorTypes.Excludes)
                         {
                             sSQL += "1=1 --All Intransit Statuses";
                         }
                         else
                         {
                             sSQL += "1=2 --No Intransit Statuses";
                         }
                     }
                     else
                     {
                         sSQL += "h.DISPLAY_INTRANSIT ";
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

                         DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderField);
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
            //else if (fieldType == filterHeaderFieldTypes.MultiHeaderID)
            //{
            //    //The multi-header ID is just the HDR_ID for the master header in multi-header group
            //    sSQL += "h.HDR_RID IN" + System.Environment.NewLine;
            //    sSQL += GetTab() + "(" + System.Environment.NewLine;
            //    tabLevel++;
            //    sSQL += GetTab() + "SELECT HDR_RID FROM HEADER WITH (NOLOCK) WHERE DISPLAY_TYPE=800734 AND ";
            //    BuildSqlForStringComparison("HDR_ID", fc, ref sSQL);
            //    sSQL += System.Environment.NewLine;
            //    tabLevel--;
            //    sSQL += GetTab() + ") " + System.Environment.NewLine;
            //}
            else if (fieldType == filterHeaderFieldTypes.QuantityToAllocate)
            {
                BuildSqlForIntComparison("h.UNITS_RECEIVED", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.SubClass) //Parent of style base node
            {
                if (organizationalPhRID == null)
                {
                    MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                    organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                }
                string nodeDisplay = "(SELECT [dbo].[UDF_MID_GET_NODE_DISPLAY] ((SELECT PARENT_HN_RID FROM HIER_NODE_JOIN hnj WITH (NOLOCK) WHERE hnj.HN_RID=h.STYLE_HNRID AND hnj.PH_RID=" + organizationalPhRID + ")))";
                BuildSqlForStringComparison(nodeDisplay, fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.Style)
            {
                string nodeDisplay = "(SELECT [dbo].[UDF_MID_GET_NODE_DISPLAY] (h.STYLE_HNRID))";
                BuildSqlForStringComparison(nodeDisplay, fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.NumPacks)
            {
                BuildSqlForIntComparison("(SELECT COUNT(*) FROM HEADER_PACK hp WITH (NOLOCK) WHERE hp.HDR_RID = h.HDR_RID)", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkColors)
            {
                BuildSqlForIntComparison("(SELECT COUNT(*) FROM HEADER_BULK_COLOR hc WITH (NOLOCK) WHERE hc.HDR_RID = h.HDR_RID)", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkSizes)
            {
                BuildSqlForIntComparison("(SELECT COUNT(DISTINCT SIZE_CODE_RID) FROM HEADER_BULK_COLOR_SIZE hs WITH (NOLOCK) WHERE hs.HDR_RID = h.HDR_RID AND UNITS > 0)", fc, ref sSQL);
            }
            else if (fieldType == filterHeaderFieldTypes.SizeGroup)
            {
                filterListConstantTypes listType = fc.listConstantType;
                filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                if (listType == filterListConstantTypes.All)
                {
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "1=2 --No Size Groups";
                    }
                    else
                    {
                        sSQL += "1=1 --All Size Groups";
                    }
                }
                else
                {
                    if (listType == filterListConstantTypes.None)
                    {
                        if (listOp == filterListOperatorTypes.Excludes)
                        {
                            sSQL += "1=1 --All Size Groups";
                        }
                        else
                        {
                            sSQL += "1=2 --No Size Groups";
                        }
                    }
                    else
                    {
                        sSQL += "h.SIZE_GROUP_RID ";
                        //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                        if (listOp == filterListOperatorTypes.Excludes)
                        {
                            sSQL += "NOT IN ";
                        }
                        else
                        {
                            sSQL += "IN ";
                        }

                        sSQL += "( ";


                        DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderField);
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
            else if (fieldType == filterHeaderFieldTypes.VswProcess)
            {
                filterListConstantTypes listType = fc.listConstantType;
                filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                if (listType == filterListConstantTypes.All)
                {
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "1=2 --No VSW Processes";
                    }
                    else
                    {
                        sSQL += "1=1 --All VSW Processes";
                    }
                }
                else
                {
                    if (listType == filterListConstantTypes.None)
                    {
                        if (listOp == filterListOperatorTypes.Excludes)
                        {
                            sSQL += "1=1 --All VSW Processes";
                        }
                        else
                        {
                            sSQL += "1=2 --No VSW Processes";
                        }
                    }
                    else
                    {
                        //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                        if (listOp == filterListOperatorTypes.Excludes)
                        {
                            sSQL += " ( NOT ";
                        }
                        else
                        {
                            //sSQL += "IN ";
                            sSQL += "( ";
                        }

                        //sSQL += "( ";


                        DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderField);
                        bool firstStatus = true;
                        foreach (DataRow dr in listValues)
                        {
                            int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

                            if (firstStatus == false)
                            { 
                                if (listOp == filterListOperatorTypes.Excludes)
                                {
                                    sSQL += " AND NOT ";
                                }
                                else
                                {
                                    sSQL += " OR "; ;
                                }
                            }
                            else
                            {
                                firstStatus = false;
                            }
                            switch ((eAdjustVSW)listValueIndex)
                            {
                                case eAdjustVSW.Adjust:
                                    sSQL += "([dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,19) = 1 AND [dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,20) = 1)";
                                    break;
                                
                                case eAdjustVSW.Replace:
                                    sSQL += "([dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,19) = 1 AND [dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,20) = 0)";
                                    break;

                                default: // None
                                    sSQL += "[dbo].[UDF_MID_FLAGTOBIT](h.ALLOCATION_TYPE_FLAGS,19) = 0";
                                    break;
                            }
                             
                            //sSQL += listValueIndex.ToString();
                        }

                        tabLevel--;
                        sSQL += GetTab() + ") " + System.Environment.NewLine;
                    }
                }
            }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            else if (fieldType == filterHeaderFieldTypes.MasterHeaderID) //Master Header ID
            {
                BuildSqlForMasterIDComparison(fc, ref sSQL);
            }
            // ENd TT#1966-MD - JSmith - DC Fulfillment
          
        }
        private static void BuildSqlForHeaderCharacteristics(filterCondition fc, ref string sSQL)
        {
           
            filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
            if (valueType == filterValueTypes.Date || valueType == filterValueTypes.Dollar || valueType == filterValueTypes.Numeric || valueType == filterValueTypes.Text)
            {
                int hcg_rid = fc.fieldIndex; //corresponds to HEADER_CHAR_GROUP HCG_RID, example 7=Fabric


                sSQL += "h.HDR_RID IN " + System.Environment.NewLine;
                sSQL += GetTab() + "( " + System.Environment.NewLine;
                tabLevel++;
                sSQL += GetTab() + "SELECT HDR_RID " + System.Environment.NewLine;
                sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                sSQL += GetTab() + "HEADER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;
                sSQL += GetTab() + "WHERE HC_RID IN " + System.Environment.NewLine;
                tabLevel++;
                sSQL += GetTab() + "( " + System.Environment.NewLine;
                sSQL += GetTab() + "SELECT HC_RID " + System.Environment.NewLine;
                sSQL += GetTab() + "FROM HEADER_CHAR hc WITH (NOLOCK) " + System.Environment.NewLine;
                sSQL += GetTab() + "WHERE HCG_RID = " + hcg_rid.ToString() + System.Environment.NewLine;
                if (valueType == filterValueTypes.Date)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForSmallDateComparison("hc.DATE_VALUE", fc, ref sSQL);
                    sSQL += System.Environment.NewLine;
                }
                else if (valueType == filterValueTypes.Dollar)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForDoubleComparison("hc.DOLLAR_VALUE", fc, ref sSQL);
                    sSQL += System.Environment.NewLine;
                }
                else if (valueType == filterValueTypes.Numeric)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForDoubleComparison("hc.NUMBER_VALUE", fc, ref sSQL); //TT#4286 -jsobek -Error when filtering on VSW sell through characteristic 
                    sSQL += System.Environment.NewLine;
                }
                else if (valueType == filterValueTypes.Text)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForStringComparison("hc.TEXT_VALUE", fc, ref sSQL);
                    sSQL += System.Environment.NewLine;
                }
                sSQL += GetTab() + ") " + System.Environment.NewLine;
                tabLevel--;
                tabLevel--;
                sSQL += GetTab() + ") " + System.Environment.NewLine;
            }
            else if (valueType == filterValueTypes.List)
            {
                filterListConstantTypes listType = fc.listConstantType;
                filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);

                //Header characteristics are not applied to all headers - only a subset of headers will have a given characteristic value

                //Begin TT#1420-MD -jsobek -Header Filter using a Header Characteristice when selected results in headers that have the header characteristic and headers that DO NO have the header characteristic.
                //if (listType == filterListConstantTypes.All)
                //{
                //    sSQL += "1=1 --All Characteristic Values";
                //}
                //else
                //{
                    if (listType == filterListConstantTypes.None)
                    {
                        sSQL += "1=2 --No Characteristic Values";
                    }
                    else if (listType == filterListConstantTypes.All) //TT#1541-MD -jsobek -Incorrect Syntax error saving a Header Filter
                    {
                        sSQL += "h.HDR_RID IN " + System.Environment.NewLine;
                        sSQL += GetTab() + "( " + System.Environment.NewLine;
                        tabLevel++;
                        sSQL += GetTab() + "SELECT HDR_RID " + System.Environment.NewLine;
                        sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                        sSQL += GetTab() + "HEADER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;

                        sSQL += GetTab() + "WHERE HC_RID IN (";
                        int hcg_rid = fc.fieldIndex;
                        sSQL += "SELECT DISTINCT HC_RID FROM HEADER_CHAR hc WITH (NOLOCK) WHERE hc.HCG_RID=" + hcg_rid.ToString() + ") " + System.Environment.NewLine;

                        tabLevel--;
                        sSQL += GetTab() + ") " + System.Environment.NewLine;
                    }
                    else
                    {
                        DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderCharacteristicRID);

                        if (listValues.Length == 0)
                        {
                            sSQL += "1=2 --No Selected Values Saved";
                        }
                        else
                        {
                            sSQL += "h.HDR_RID " + System.Environment.NewLine;

                            if (listOp == filterListOperatorTypes.Excludes)
                            {
                                sSQL += "NOT IN ";
                            }
                            else
                            {
                                sSQL += "IN ";
                            }

                            sSQL += GetTab() + "( " + System.Environment.NewLine;
                            tabLevel++;
                            // Begin TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                            if (fc.operatorIndex == 1) // Not In
                            {
                                sSQL += GetTab() + "SELECT HDR_RID " + System.Environment.NewLine;
                                sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                                sSQL += GetTab() + "HEADER WITH (NOLOCK) " + System.Environment.NewLine;
                                sSQL += GetTab() + "WHERE HDR_RID NOT IN " + System.Environment.NewLine;
                                sSQL += GetTab() + "( " + System.Environment.NewLine;
                                
                            }
                            // End TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                            sSQL += GetTab() + "SELECT HDR_RID " + System.Environment.NewLine;
                            sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                            sSQL += GetTab() + "HEADER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;
                            sSQL += GetTab() + "WHERE HC_RID IN (";


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
                            sSQL += ") " + System.Environment.NewLine;
                            // Begin TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                            if (fc.operatorIndex == 1) // Not In
                            {
                                sSQL += GetTab() + ") " + System.Environment.NewLine;
                            }
                            // End TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                            tabLevel--;
                            sSQL += GetTab() + ") " + System.Environment.NewLine;
                        }
                    }
               // }
               //End TT#1420-MD -jsobek -Header Filter using a Header Characteristice when selected results in headers that have the header characteristic and headers that DO NO have the header characteristic.
            }
            else
            {
                sSQL += "1=1 --Unknown characteristic value type";
            }
            
        }
        private static void BuildSqlForHeaderDate(filterCondition fc, ref string sSQL)
        {

            BuildSqlForSmallDateComparison("h.HDR_DAY", fc, ref sSQL);
        }
        private static void BuildSqlForHeaderReleaseDate(filterCondition fc, ref string sSQL)
        {
            //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
            if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
            {
                BuildSqlForDateTimeComparison("h.RELEASE_DATETIME", fc, ref sSQL); //date and time
            }
            else
            {
                BuildSqlForDateTimeComparison("CAST(h.RELEASE_DATETIME AS DATE)", fc, ref sSQL); //date only
            }
            //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
        }
        private static void BuildSqlForHeaderStatus(filterCondition fc, ref string sSQL)
        {
             filterListConstantTypes listType = fc.listConstantType;
             filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
             if (listType == filterListConstantTypes.All)
             {
                if (listOp == filterListOperatorTypes.Excludes)
                {
                    sSQL += "1=2 --No Header Statuses";
                }
                else
                {
                    sSQL += "1=1 --All Header Statuses";
                }
             }
             else
             {
                 if (listType == filterListConstantTypes.None)
                 {
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "1=1 --All Header Statuses";
                    }
                    else
                    {
                        sSQL += "1=2 --No Header Statuses";
                    }
                 }
                 else
                 {
                     sSQL += "h.DISPLAY_STATUS ";
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

                     DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderStatus);
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
        private static void BuildSqlForHeaderTypes(filterCondition fc, ref string sSQL)
        {
            filterListConstantTypes listType = fc.listConstantType;
            filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
             if (listType == filterListConstantTypes.All)
             {
                if (listOp == filterListOperatorTypes.Excludes)
                {
                    sSQL += "1=2 --No Header Types";
                }
                else
                {
                    sSQL += "1=1 --All Header Types";
                }
             }
             else
             {
                 if (listType == filterListConstantTypes.None)
                 {
                     if (listOp == filterListOperatorTypes.Excludes)
                     {
                         sSQL += "1=1 --All Header Types";
                     }
                     else
                     {
                         sSQL += "1=2 --No Header Types";
                     }
                 }
                 else
                 {
                     // Begin TT#1966-MD - JSmith - DC Fulfillment
                     //sSQL += "h.DISPLAY_TYPE ";
                     ////filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                     //if (listOp == filterListOperatorTypes.Excludes)
                     //{
                     //    sSQL += "NOT IN ";
                     //}
                     //else
                     //{
                     //    sSQL += "IN ";
                     //}

                     //sSQL += "(";
                     //tabLevel++;

                     //DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderTypes);
                     //bool firstStatus = true;
                     //foreach (DataRow dr in listValues)
                     //{
                     //    int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

                     //    if (firstStatus == false)
                     //    {
                     //        sSQL += ",";
                     //    }
                     //    else
                     //    {
                     //        firstStatus = false;
                     //    }
                     //    sSQL += listValueIndex.ToString();
                     //}

                     //tabLevel--;
                     //sSQL += GetTab() + ") " + System.Environment.NewLine;

                     // Determine types so will know how to construct SQL
                     bool containsMaster = false;
                     bool containsOtherTypes = false;
                     DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderTypes);
                     foreach (DataRow dr in listValues)
                     {
                         int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

                         if (listValueIndex == eHeaderType.Master.GetHashCode())
                         {
                             containsMaster = true;
                         }
                         else
                         {
                             containsOtherTypes = true;
                         }
                     }
                     

                     if (containsOtherTypes
                         && containsMaster)
                     {
                         sSQL += "( ";
                     }

                     if (containsOtherTypes)
                     {
                         sSQL += "h.DISPLAY_TYPE ";
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
                         sSQL += GetTab() + ") ";
                     }

                     if (containsOtherTypes
                         && containsMaster)
                     {
                         if (listOp == filterListOperatorTypes.Excludes)
                         {
                             sSQL += " AND ( h.HDR_RID NOT IN (SELECT MASTER_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) AND h.HDR_RID NOT IN (SELECT SUBORD_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) ) ) ";
                         }
                         else
                         {
                             sSQL += " OR ( h.HDR_RID IN (SELECT MASTER_HDR_RID FROM MASTER_HEADER WITH (NOLOCK))  OR h.HDR_RID IN (SELECT SUBORD_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) ) ) ";
                         }
                     }
                     else if (containsMaster)
                     {
                         if (listOp == filterListOperatorTypes.Excludes)
                         {
                             sSQL += " ( h.HDR_RID NOT IN (SELECT MASTER_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) AND h.HDR_RID NOT IN (SELECT SUBORD_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) ) ";
                         }
                         else
                         {
                             sSQL += "( h.HDR_RID IN (SELECT MASTER_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) OR h.HDR_RID IN (SELECT SUBORD_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) )";
                         }
                     }

                     sSQL += System.Environment.NewLine;
                     // End TT#1966-MD - JSmith - DC Fulfillment
                 }
            }
        }

        private static int? organizationalPhRID = null;
        private static void BuildSqlForHeaderMerchandise(filterCondition fc, ref string sSQL)
        {
            if (fc.headerMerchandise_HN_RID == Include.NoRID)
            {
                sSQL += "1=1 --No Merchandise Specified" + System.Environment.NewLine;
            }
            else
            {
                sSQL += "h.HDR_RID IN" + System.Environment.NewLine;
                sSQL += GetTab() + "(" + System.Environment.NewLine;
                tabLevel++;
                int selectedNodeRID = fc.headerMerchandise_HN_RID;

                ////int selectedPhRID = fc.headerMerchandise_PH_RID;
                //if (organizationalPhRID == null)
                //{
                //    MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                //    organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                //}
                ////sSQL += GetTab() + "SELECT HDR_RID FROM [dbo].[UDF_HIERARCHY_GET_HEADERS_FROM_NODE] (" + selectedNodeRID.ToString() + ",(SELECT HOME_PH_RID FROM HIERARCHY_NODE hn WITH (NOLOCK) WHERE hn.HN_RID=" + selectedNodeRID.ToString() + ")," + organizationalPhRID.ToString() + ")  ";
                ////get the product hierarchy rid separately to prevent performance issue
                //string phRID = "@PH_RID_" + GetConditionUID(fc);
                //sSQL += GetTab() + "SELECT HDR_RID FROM [dbo].[UDF_HIERARCHY_GET_HEADERS_FROM_NODE] (" + selectedNodeRID.ToString() + "," + phRID + "," + organizationalPhRID.ToString() + ")  ";
                //sSQL += System.Environment.NewLine;
                sSQL += GetTab() + "SELECT HDR_RID FROM @HDR_RIDS_FROM_HIERARCHY_" + GetConditionUID(fc) + System.Environment.NewLine;

                tabLevel--;
                sSQL += GetTab() + ") " + System.Environment.NewLine;
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

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        private static void BuildSqlForMasterIDComparison(filterCondition fc, ref string sSQL)
        {
            string val2 = fc.valueToCompare;
            //escape val2
            val2 = val2.Replace("'", "''");
            val2 = val2.Replace("[", "[[]");
            val2 = val2.Replace("%", "[%]");
            val2 = val2.Replace("_", "[_]");
            filterStringOperatorTypes stringOp = filterStringOperatorTypes.FromIndex(fc.operatorIndex);
            sSQL += "(";
            if (stringOp == filterStringOperatorTypes.Contains)
            {
                sSQL += "UPPER(m.HDR_ID) LIKE '%" + val2.ToUpper() + "%'";
                sSQL += " OR UPPER(s.HDR_ID) LIKE '%" + val2.ToUpper() + "%'";
            }
            else if (stringOp == filterStringOperatorTypes.ContainsExactly)
            {
                sSQL += "m.HDR_ID LIKE '%" + val2 + "%'";
                sSQL += " OR s.HDR_ID LIKE '%" + val2 + "%'";
            }
            else if (stringOp == filterStringOperatorTypes.StartsWith)
            {
                sSQL += "UPPER(m.HDR_ID) LIKE '" + val2.ToUpper() + "%'";
                sSQL += " OR UPPER(s.HDR_ID) LIKE '" + val2.ToUpper() + "%'";
            }
            else if (stringOp == filterStringOperatorTypes.StartsExactlyWith)
            {
                sSQL += "m.HDR_ID LIKE '" + val2 + "%'";
                sSQL += " OR s.HDR_ID LIKE '" + val2 + "%'";
            }
            else if (stringOp == filterStringOperatorTypes.EndsWith)
            {
                sSQL += "UPPER(m.HDR_ID) LIKE '%" + val2.ToUpper() + "'";
                sSQL += " OR UPPER(s.HDR_ID) LIKE '%" + val2.ToUpper() + "'";
            }
            else if (stringOp == filterStringOperatorTypes.EndsExactlyWith)
            {
                sSQL += "m.HDR_ID LIKE '%" + val2 + "'";
                sSQL += " OR s.HDR_ID LIKE '%" + val2 + "'";
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqual)
            {
                sSQL += "UPPER(m.HDR_ID) = '" + val2.ToUpper() + "'";
                sSQL += " OR UPPER(s.HDR_ID) = '" + val2.ToUpper() + "'";
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqualExactly)
            {
                sSQL += "m.HDR_ID = '" + val2 + "'";
                sSQL += " OR s.HDR_ID = '" + val2 + "'";
            }
            else
            {
                sSQL += "m.HDR_ID = '" + val2 + "'";
                sSQL += " OR s.HDR_ID = '" + val2 + "'";
            }
            sSQL += ")";

        }
        // End TT#1966-MD - JSmith - DC Fulfillment

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
        private static void BuildSqlForSmallDateComparison(string val1, filterCondition fc, ref string sSQL)
        {
            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Unrestricted)
            {
                sSQL += "1=1 --Unrestricted date range";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
            {
                sSQL += "(" + val1 + " >= @SDT_LAST_24HR_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_NOW)";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
            {
                sSQL += "(" + val1 + " >= @SDT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_NOW)";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
            {
                sSQL += "(" + val1 + " >= @SDT_BTWN_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_BTWN_TO_" + GetConditionUID(fc) + ")";
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
            {
                sSQL += "(" + val1 + " >= @SDT_SPCFY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_SPCFY_TO_" + GetConditionUID(fc) + ")";
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

        private static void BuildSqlDateVariablesForSmallDate(filterCondition fc, ref string sSQL, ref bool firstDynamicDate)
        {
            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
            {
                if (firstDynamicDate == false)
                {
                    firstDynamicDate = true;
                    MakeDateDynamicSQL(ref sSQL);
                }
                sSQL += htab1 + "DECLARE @SDT_LAST_24HR_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(hh, -24, @SDT_NOW), 112);" + System.Environment.NewLine;
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
            {
                if (firstDynamicDate == false)
                {
                    firstDynamicDate = true;
                    MakeDateDynamicSQL(ref sSQL);
                }

                //go from midnight to midnight
                sSQL += htab1 + "DECLARE @SDT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(dd, -7, @SDT_NOW), 112);" + System.Environment.NewLine;
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

                sSQL += htab1 + "DECLARE @SDT_BTWN_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(dd, " + daysFrom.ToString() + ", @SDT_NOW), 112);" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @SDT_BTWN_TO_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(dd, " + daysTo.ToString() + ", @SDT_NOW), 112);" + System.Environment.NewLine;
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
            {
                DateTime dateFrom = (DateTime)fc.valueToCompareDateFrom;    
                DateTime dateTo = (DateTime)fc.valueToCompareDateTo;
                sSQL += htab1 + "DECLARE @SDT_SPCFY_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, '" + dateFrom.ToString("yyyyMMdd") + "', 112);" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @SDT_SPCFY_TO_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, '" + dateTo.ToString("yyyyMMdd") + "', 112);" + System.Environment.NewLine;
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
