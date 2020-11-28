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
    public static class filterEngineSQLforStores
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

        public static string MakeSqlForFilter(filter f)
        {
            try
            {
                ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq()); //parent seq for conditions
                //string procedureName = FilterCommon.BuildFilterProcedureName(f.filterRID, f.filterType);
                string sSQL = string.Empty;
                FilterData dlFilters = new FilterData();
                //bool doesProcedureExist = dlFilters.DoesFilterProcedureExist(f.filterRID, f.filterType);
                string filterName = f.filterName;
                //escape val2
                filterName = filterName.Replace("'", "''");
                filterName = filterName.Replace("[", "[[]");
                filterName = filterName.Replace("%", "[%]");
                filterName = filterName.Replace("_", "[_]");

    
       
                if (f.isLimited)
                {
                    sSQL += htab1 + "SELECT TOP " + f.resultLimit.ToString() + System.Environment.NewLine;
                }
                else
                {
                    sSQL += htab1 + "SELECT " + System.Environment.NewLine;
                }
                sSQL += htab1 + htab1 + "s.ST_ID AS 'ID'," + System.Environment.NewLine;
                sSQL += htab1 + htab1 + "s.STORE_NAME AS 'Name'" + System.Environment.NewLine;
                sSQL += htab1 + "FROM STORES s WITH (NOLOCK) " + System.Environment.NewLine;

                sSQL += htab1 + "WHERE s.ACTIVE_IND=1" + System.Environment.NewLine;

    

                //Build the sql for the conditions
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeSQLForConditions(cn, ref sSQL);
                }

                sSQL += System.Environment.NewLine;
             

                ConditionNode sortRoot = f.FindConditionNode(f.GetSortByConditionSeq()); //parent seq for conditions
                if (sortRoot.ConditionNodes.Count > 0)
                {
                    sSQL += htab1 + "ORDER BY ";
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

        private static void MakeSQLForSortBy(ConditionNode cnSort, ref string sSQL, bool isFirst)
        {
            filterSortByTypes sortByType = filterSortByTypes.FromIndex(cnSort.condition.sortByTypeIndex);
            //filterDataTypes dataType;
            //if (sortByType == filterSortByTypes.ProductCharacteristics)
            //{
            //    dataType = filterDataHelper.ProductCharacteristicsGetDataType(fieldIndex);
            //}
            //else 
            if (sortByType == filterSortByTypes.StoreFields)
            {
                if (isFirst == false)
                {
                    sSQL += ", ";
                }

                //dataType = filterProductFieldTypes.GetValueTypeInfoForField(fieldIndex);
                filterProductFieldTypes sortByField =  filterProductFieldTypes.FromIndex(cnSort.condition.sortByFieldIndex);


                if (sortByField == filterStoreFieldTypes.StoreID)
                {
                    sSQL += "ID";
                }
                else if (sortByField == filterStoreFieldTypes.StoreName)
                {
                    sSQL += "Name";
                }
               

                filterSortByDirectionTypes sortByDirection = filterSortByDirectionTypes.FromIndex(cnSort.condition.operatorIndex);
                if (sortByDirection == filterSortByDirectionTypes.Descending)
                {
                    sSQL += " DESC";
                }
            }
  
        }

        private static void MakeSQLForCondition(ConditionNode cn, ref string sSQL)
        {
            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);

            if (et == filterDictionary.StoreStatus )
            {
                BuildSqlForStringComparison("s.STORE_NAME", cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductID)
            {
                BuildSqlForProductID(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductName)
            {
                BuildSqlForProductName(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductDescrip)
            {
                BuildSqlForProductDescription(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductChar)
            {
                BuildSqlForCharacteristics(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductActive)
            {
                BuildSqlForProductActive(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductMerchandise)
            {
                BuildSqlForMerchandise(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductLevels)
            {
                BuildSqlForProductLevels(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.ProductHierarchies)
            {
                BuildSqlForProductHierarchies(cn.condition, ref sSQL);
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
   
        private static void BuildSqlForProductID(filterCondition fc, ref string sSQL)
        {
            sSQL += "("; //TT#1431-MD -jsobek -Filter on two description items returns all item that contain either instead of items that only contain both
            BuildSqlForStringComparison("RTRIM(bn.BN_ID)", fc, ref sSQL);
            sSQL += " OR ";
            BuildSqlForStringComparison("RTRIM(cc.COLOR_CODE_ID)", fc, ref sSQL);
            sSQL += " OR ";
            BuildSqlForStringComparison("RTRIM(sc.SIZE_CODE_ID)", fc, ref sSQL);
            sSQL += ")"; //TT#1431-MD -jsobek -Filter on two description items returns all item that contain either instead of items that only contain both
        }
        private static void BuildSqlForProductName(filterCondition fc, ref string sSQL)
        {
            sSQL += "("; //TT#1431-MD -jsobek -Filter on two description items returns all item that contain either instead of items that only contain both
            BuildSqlForStringComparison("RTRIM(bn.BN_NAME)", fc, ref sSQL);
            sSQL += " OR ";
            BuildSqlForStringComparison("RTRIM(cc.COLOR_CODE_NAME)", fc, ref sSQL);
            sSQL += " OR ";
            BuildSqlForStringComparison("RTRIM(sc.SIZE_CODE_PRIMARY)", fc, ref sSQL);
            sSQL += " OR ";
            BuildSqlForStringComparison("RTRIM(sc.SIZE_CODE_SECONDARY)", fc, ref sSQL);
            sSQL += ")"; //TT#1431-MD -jsobek -Filter on two description items returns all item that contain either instead of items that only contain both
        }
        private static void BuildSqlForProductDescription(filterCondition fc, ref string sSQL)
        {
            sSQL += "("; //TT#1431-MD -jsobek -Filter on two description items returns all item that contain either instead of items that only contain both
            BuildSqlForStringComparison("RTRIM(bn.BN_DESCRIPTION)", fc, ref sSQL);
            sSQL += " OR ";
            BuildSqlForStringComparison("RTRIM(cn.COLOR_DESCRIPTION)", fc, ref sSQL);
            sSQL += ")"; //TT#1431-MD -jsobek -Filter on two description items returns all item that contain either instead of items that only contain both
        }
        private static void BuildSqlForCharacteristics(filterCondition fc, ref string sSQL)
        {       
            filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
            //if (valueType == filterValueTypes.Date || valueType == filterValueTypes.Dollar || valueType == filterValueTypes.Numeric || valueType == filterValueTypes.Text)
            //{
            //    int hcg_rid = fc.fieldIndex; //corresponds to HIER_CHAR_GROUP HCG_RID, example 7=Fabric


            //    sSQL += "hnj.HN_RID IN " + System.Environment.NewLine;
            //    sSQL += GetTab() + "( " + System.Environment.NewLine;
            //    tabLevel++;
            //    sSQL += GetTab() + "SELECT HN_RID " + System.Environment.NewLine;
            //    sSQL += GetTab() + "FROM " + System.Environment.NewLine;
            //    sSQL += GetTab() + "HIER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;
            //    sSQL += GetTab() + "WHERE HC_RID IN " + System.Environment.NewLine;
            //    tabLevel++;
            //    sSQL += GetTab() + "( " + System.Environment.NewLine;
            //    sSQL += GetTab() + "SELECT HC_RID " + System.Environment.NewLine;
            //    sSQL += GetTab() + "FROM HIER_CHAR hc WITH (NOLOCK) " + System.Environment.NewLine;
            //    sSQL += GetTab() + "WHERE HCG_RID = " + hcg_rid.ToString() + System.Environment.NewLine;
            //    //if (valueType == filterValueTypes.Date)
            //    //{
            //    //    sSQL += GetTab() + "AND ";
            //    //    //BuildSqlForSmallDateComparison("hc.DATE_VALUE", fc, ref sSQL);
            //    //    sSQL += System.Environment.NewLine;
            //    //}
            //    //else if (valueType == filterValueTypes.Dollar)
            //    //{
            //    //    sSQL += GetTab() + "AND ";
            //    //    BuildSqlForDoubleComparison("hc.DOLLAR_VALUE", fc, ref sSQL);
            //    //    sSQL += System.Environment.NewLine;
            //    //}
            //    //else if (valueType == filterValueTypes.Numeric)
            //    //{
            //    //    sSQL += GetTab() + "AND ";
            //    //    BuildSqlForIntComparison("hc.NUMBER_VALUE", fc, ref sSQL);
            //    //    sSQL += System.Environment.NewLine;
            //    //}
            //    //else if (valueType == filterValueTypes.Text)
            //    //{
            //        sSQL += GetTab() + "AND ";
            //        BuildSqlForStringComparison("hc.HC_ID", fc, ref sSQL);
            //        sSQL += System.Environment.NewLine;
            //    //}
            //    sSQL += GetTab() + ") " + System.Environment.NewLine;
            //    tabLevel--;
            //    tabLevel--;
            //    sSQL += GetTab() + ") " + System.Environment.NewLine;
            //}
            if (valueType == filterValueTypes.List)
            {
                filterListConstantTypes listType = fc.listConstantType;
                if (listType == filterListConstantTypes.All)
                {
                    sSQL += "1=1 --All Characteristic Values";
                }
                else
                {
                    if (listType == filterListConstantTypes.None)
                    {
                        sSQL += "1=2 --No Characteristic Values";
                    }
                    else
                    {
                        sSQL += "hnj.HN_RID IN " + System.Environment.NewLine;
                        sSQL += GetTab() + "( " + System.Environment.NewLine;
                        tabLevel++;
                        sSQL += GetTab() + "SELECT HN_RID " + System.Environment.NewLine;
                        sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                        sSQL += GetTab() + "HIER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;
                        sSQL += GetTab() + "WHERE HC_RID IN (";

                        DataRow[] listValues = fc.GetListValues(filterListValueTypes.ProductCharacteristicRID);
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
                        tabLevel--;
                        sSQL += GetTab() + ") " + System.Environment.NewLine;
                    }
                }
            }
            else
            {
                sSQL += "1=1 --Unknown characteristic value type";
            }
            
        }

        private static void BuildSqlForProductActive(filterCondition fc, ref string sSQL)
        {
            if (fc.valueToCompareBool == true)
            {
                sSQL += " coalesce(hn.ACTIVE_IND, 1)=1 ";
            }
            else
            {
                sSQL += " coalesce(hn.ACTIVE_IND, 1)=0 ";
            }

        }
        
        private static void BuildSqlForProductLevels(filterCondition fc, ref string sSQL)
        {
             filterListConstantTypes listType = fc.listConstantType;
             filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
             if (listType == filterListConstantTypes.All)
             {
                if (listOp == filterListOperatorTypes.Excludes)
                {
                    sSQL += "1=2 --No Product Levels";
                }
                else
                {
                    sSQL += "1=1 --All Product Levels";
                }
             }
             else
             {
                 if (listType == filterListConstantTypes.None)
                 {
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "1=1 --All Product Levels";
                    }
                    else
                    {
                        sSQL += "1=2 --No Product Levels";
                    }
                 }
                 else
                 {
                     sSQL += "hn.HOME_LEVEL ";
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

                     DataRow[] listValues = fc.GetListValues(filterListValueTypes.ProductLevels);
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
        private static void BuildSqlForProductHierarchies(filterCondition fc, ref string sSQL)
        {
            filterListConstantTypes listType = fc.listConstantType;
            filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
             if (listType == filterListConstantTypes.All)
             {
                if (listOp == filterListOperatorTypes.Excludes)
                {
                    sSQL += "1=2 --No Hierarchies";
                }
                else
                {
                    sSQL += "1=1 --All Hierarchies";
                }
             }
             else
             {
                 if (listType == filterListConstantTypes.None)
                 {
                     if (listOp == filterListOperatorTypes.Excludes)
                     {
                         sSQL += "1=1 --All Hierarchies";
                     }
                     else
                     {
                         sSQL += "1=2 --No Hierarchies";
                     }
                 }
                 else
                 {
                     sSQL += "hnj.PH_RID ";
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

                     DataRow[] listValues = fc.GetListValues(filterListValueTypes.ProductHierarchies);
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
            if (et == filterDictionary.ProductMerchandise)
            {
                sSQL += htab1 + "DECLARE @PH_RID_" + GetConditionUID(fc) + " INT = (SELECT HOME_PH_RID FROM HIERARCHY_NODE hn WITH (NOLOCK) WHERE hn.HN_RID=" + cn.condition.headerMerchandise_HN_RID.ToString() + ");" + System.Environment.NewLine;


                //Begin TT#1430-MD -jsobek -Null reference after canceling a product search
                sSQL += htab1 + "DECLARE @HN_RIDS_" + GetConditionUID(fc) + " AS NODE_TYPE;" + System.Environment.NewLine;

                sSQL += htab1 + "INSERT INTO @HN_RIDS_" + GetConditionUID(fc) + System.Environment.NewLine;
                sSQL += htab1 + "SELECT DISTINCT @PH_RID_" + GetConditionUID(fc) + ", HN_RID FROM UDF_HIERARCHY_GET_FROM_NODE(" + cn.condition.headerMerchandise_HN_RID.ToString() + ", @PH_RID_" + GetConditionUID(fc) + ")" + System.Environment.NewLine; //TT#1453-MD -jsobek -Search for Styles in Hierarchies returns a database unique index constraint violation error
                //End TT#1430-MD -jsobek -Null reference after canceling a product search
            }


        }
        private static void BuildSqlForMerchandise(filterCondition fc, ref string sSQL)
        {
            if (fc.headerMerchandise_HN_RID == Include.NoRID)
            {
                sSQL += "1=1 --No Merchandise Specified" + System.Environment.NewLine;
            }
            else
            {
                //Begin TT#1430-MD -jsobek -Null reference after canceling a product search
                //sSQL += "hnj.HN_RID IN" + System.Environment.NewLine;
                //sSQL += GetTab() + "(" + System.Environment.NewLine;
                //tabLevel++;
                //int selectedNodeRID = fc.headerMerchandise_HN_RID;
            
                ////get the product hierarchy rid separately to prevent performance issue
                //string phRID = "@PH_RID_" + GetConditionUID(fc);
                //sSQL += GetTab() + "SELECT HN_RID FROM [dbo].[UDF_HIERARCHY_GET_FROM_NODE] (" + selectedNodeRID.ToString() + "," + phRID + ")  ";

                //sSQL += System.Environment.NewLine;
                //tabLevel--;
                //sSQL += GetTab() + ") " + System.Environment.NewLine;

    
                int selectedNodeRID = fc.headerMerchandise_HN_RID;
            
                //get the product hierarchy rid separately to prevent performance issue
                //string phRID = "@PH_RID_" + GetConditionUID(fc);
                //sSQL += GetTab()  + selectedNodeRID.ToString() + " IN (SELECT HN_RID FROM UDF_HIERARCHY_GET_FROM_NODE(hn.HN_RID, " + phRID + "))";
                //sSQL += System.Environment.NewLine;


                sSQL += GetTab() + "hn.HN_RID IN (SELECT HN_RID FROM @HN_RIDS_" + GetConditionUID(fc) + ")" + System.Environment.NewLine;


                //End TT#1430-MD -jsobek -Null reference after canceling a product search
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
