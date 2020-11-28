//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;

//using MIDRetail.Data;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Business
//{

//    /// <summary>
//    /// Used for dynamic store groups (aka attributes)
//    /// </summary>
//    public static class filterEngineSQLForStoreGroupDynamic
//    {
//        private static string htab1 = "\t";
//        private static int tabLevel = 2;

//        private static string GetTab()
//        {
//            string htab = string.Empty;
//            for (int i = 1; i <= tabLevel; i++)
//            {
//                htab += "\t";
//            }
//            return htab;
//        }

//        public static DataTable ExecuteFilter(ref List<levelInfo> tempLevelList, filter f, StoreGroupMaint groupData, int groupRID, int groupVersion)
//        {
//            try
//            {
//                ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq()); //parent seq for conditions


//                string sSQL = string.Empty;

//                //Build date variables for conditions so SQL does not perform these calcs per row
//                bool firstDynamicDate = false;
//                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
//                {
//                    MakeDateVariableSQLForConditions(cn, ref sSQL, ref firstDynamicDate);
//                }
//                ConditionNode cnExclusionList = null;
//                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
//                {
//                    FindExclusionListCondition(cn, ref cnExclusionList);
//                }

//                sSQL += htab1 + "DECLARE @ALL_STORES AS TABLE (ST_RID INT, LEVEL_SEQ INT)" + System.Environment.NewLine;
//                sSQL += htab1 + "INSERT INTO @ALL_STORES SELECT ST_RID, -1 FROM STORES WITH (NOLOCK) " + System.Environment.NewLine;
                

               
//                sSQL += htab1 + System.Environment.NewLine;

//                foreach (levelInfo li in tempLevelList)
//                {
//                    if (li.levelType == eGroupLevelTypes.DynamicSet)
//                    {
//                        //if (li.levelType == eGroupLevelTypes.DynamicSet && li.storeObjectType == StoreValidation.storeObjectTypes.StoreCharacteristics)
//                        //{
//                        //    sSQL += htab1 + "--Level " + li.levelName + System.Environment.NewLine;
//                        //    sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = " + li.levelSeq + System.Environment.NewLine;
//                        //    sSQL += htab1 + "WHERE LEVEL_SEQ = -1 AND ST_RID IN (SELECT ST_RID FROM STORE_CHAR_JOIN scj WITH (NOLOCK) WHERE 1=1 " + System.Environment.NewLine;
//                        //    //ConditionNode cn = f.FindConditionNodeByRid(li.levelConditionRID);
//                        //    string[] valuesSplit = li.levelValues.Split('~');

//                        //    foreach (string val in valuesSplit)
//                        //    {
//                        //        int scRID = Convert.ToInt32(val);
//                        //        sSQL += htab1 + "AND SC_RID=" + scRID.ToString() + System.Environment.NewLine;
//                        //    }


//                        //    sSQL += htab1 + ") " + System.Environment.NewLine;
//                        //    sSQL += htab1 + System.Environment.NewLine;
//                        //}
//                        //else if (li.levelType == eGroupLevelTypes.DynamicSet && li.storeObjectType == StoreValidation.storeObjectTypes.StoreFields)
//                        //{
//                        //    sSQL += htab1 + "--Level " + li.levelName + System.Environment.NewLine;
//                        //    sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = " + li.levelSeq + System.Environment.NewLine;

//                        //    sSQL += htab1 + "WHERE LEVEL_SEQ = -1 AND ST_RID IN (SELECT ST_RID FROM STORES WITH (NOLOCK) WHERE 1=1 " + System.Environment.NewLine;
//                        //    //ConditionNode cn = f.FindConditionNodeByRid(li.levelConditionRID);
//                        //    string[] valuesSplit = li.levelValues.Split('~');
//                        //    string[] fieldsSplit = li.levelFields.Split('~');
//                        //    int myIndex = 0;
//                        //    foreach (string val in valuesSplit)
//                        //    {
//                        //        int filterFieldIndex = Convert.ToInt32(fieldsSplit[myIndex]);
//                        //        filterStoreFieldTypes filterStoreFieldType = filterStoreFieldTypes.FromIndex(filterFieldIndex);
//                        //        if (filterStoreFieldType.storeFieldType.dataType == fieldDataTypes.NumericInteger)
//                        //        {
//                        //            sSQL += htab1 + "AND " + filterStoreFieldType.storeFieldType.dbFieldName + "=" + val + System.Environment.NewLine;
//                        //        }
//                        //        else
//                        //        {
//                        //            sSQL += htab1 + "AND " + filterStoreFieldType.storeFieldType.dbFieldName + "='" + val + "'" + System.Environment.NewLine;
//                        //        }
//                        //        myIndex++;
//                        //    }


//                        //    sSQL += htab1 + ") " + System.Environment.NewLine;
//                        //    sSQL += htab1 + System.Environment.NewLine;
//                        //}

//                        sSQL += htab1 + "--Level " + li.levelName + System.Environment.NewLine;
//                        sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = " + li.levelSeq + System.Environment.NewLine;
//                        //sSQL += htab1 + "WHERE LEVEL_SEQ = -1 AND ST_RID IN (SELECT s.ST_RID FROM STORE_CHAR_JOIN scj WITH (NOLOCK) INNER JOIN STORES s WITH (NOLOCK) ON s.ST_RID=scj.ST_RID WHERE 1=1 " + System.Environment.NewLine;
//                        sSQL += htab1 + "WHERE LEVEL_SEQ = -1 " + System.Environment.NewLine;
//                        //ConditionNode cn = f.FindConditionNodeByRid(li.levelConditionRID);

//                        string[] valuesSplit = li.levelValues.Split('~');
//                        string[] fieldsSplit = li.levelFields.Split('~');
//                        int myIndex = 0;
//                        foreach (string val in valuesSplit)
//                        {
//                            int filterFieldIndex = Convert.ToInt32(fieldsSplit[myIndex]);
//                            if (filterFieldIndex == -99)
//                            {
//                                int scRID = Convert.ToInt32(val);
//                                sSQL += htab1 + "AND ST_RID IN (SELECT scj.ST_RID FROM STORE_CHAR_JOIN scj WITH (NOLOCK) WHERE SC_RID=" + scRID.ToString() + ") " + System.Environment.NewLine;
//                            }
//                            else 
//                            {

//                                filterStoreFieldTypes filterStoreFieldType = filterStoreFieldTypes.FromIndex(filterFieldIndex);
//                                if (filterStoreFieldType.storeFieldType.dataType == fieldDataTypes.NumericInteger)
//                                {
//                                    sSQL += htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s." + filterStoreFieldType.storeFieldType.dbFieldName + "=" + val + ") " + System.Environment.NewLine;
//                                }
//                                else
//                                {
//                                    sSQL += htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s." + filterStoreFieldType.storeFieldType.dbFieldName + "='" + val + "')" + System.Environment.NewLine;
//                                }
//                            }
//                            myIndex++;
//                        }


//                        //sSQL += htab1 + ") " + System.Environment.NewLine;
//                        sSQL += htab1 + System.Environment.NewLine;

//                    }
           
//                }

           
//                //Exclude stores in the exclusion list
//                DataRow[] listValues = cnExclusionList.condition.GetListValues(filterListValueTypes.StoreRID);
//                if (listValues.Length > 0)
//                {
//                    sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = -1 WHERE ST_RID IN (";

//                    bool firstOne = true;
//                    foreach (DataRow dr in listValues)
//                    {
//                        int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

//                        if (firstOne == false)
//                        {
//                            sSQL += ",";
//                        }
//                        else
//                        {
//                            firstOne = false;
//                        }
//                        sSQL += listValueIndex.ToString();
//                    }
//                    sSQL += ") " + System.Environment.NewLine;
//                    sSQL += htab1 + System.Environment.NewLine;
//                }

//                sSQL += htab1 + "DECLARE @SG_RID INT" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SG_VERSION INT" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SGL_RID INT" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SGL_VERSION INT" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SGL_OVERRIDE_SEQUENCE INT" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SGL_OVERRIDE_ID VARCHAR(50)" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @LEVEL_TYPE INT" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @CONDITION_RID INT" + System.Environment.NewLine;

//                sSQL += htab1 + "SET @SG_RID = " + groupRID.ToString() + System.Environment.NewLine;
//                sSQL += htab1 + "SET @SG_VERSION = " + groupVersion.ToString() + System.Environment.NewLine;

//                foreach (levelInfo li in tempLevelList)
//                {
//                    if (li.levelType == eGroupLevelTypes.DynamicSet)
//                    {
//                        sSQL += htab1 + "SET @SGL_OVERRIDE_SEQUENCE = " + li.levelSeq.ToString() + System.Environment.NewLine;
//                        sSQL += htab1 + "SET @SGL_OVERRIDE_ID = '" + li.levelName + "';" + System.Environment.NewLine;
//                        sSQL += htab1 + "SET @LEVEL_TYPE = " + ((int)li.levelType).ToString()  + System.Environment.NewLine; // --Normal=0,AvailableStoreSet=1,DynamicSet=2
//                        sSQL += htab1 + "SET @CONDITION_RID = " + li.levelConditionRID.ToString() + System.Environment.NewLine;
//                        sSQL += htab1 + "SELECT @SGL_RID=SGL_RID FROM STORE_GROUP_LEVEL WITH (NOLOCK) WHERE SG_RID = @SG_RID AND SGL_ID = @SGL_OVERRIDE_ID" + System.Environment.NewLine;


//                        sSQL += htab1 + "IF NOT EXISTS (SELECT 1 FROM STORE_GROUP_LEVEL WHERE SGL_ID = @SGL_OVERRIDE_ID)" + System.Environment.NewLine;
//                        sSQL += htab1 + "BEGIN" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + "IF EXISTS (SELECT 1 FROM @ALL_STORES WHERE LEVEL_SEQ = @SGL_OVERRIDE_SEQUENCE)" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + "BEGIN" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + htab1 + "--Insert the group level" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + htab1 + "INSERT INTO STORE_GROUP_LEVEL (SGL_SEQUENCE,SG_RID,SGL_ID,IS_ACTIVE,CONDITION_RID,LEVEL_TYPE)" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + htab1 + "VALUES (@SGL_OVERRIDE_SEQUENCE,@SG_RID,@SGL_OVERRIDE_ID,1,@CONDITION_RID,@LEVEL_TYPE)" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + htab1 + "SET @SGL_RID = SCOPE_IDENTITY();" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + "END" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + "ELSE" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + "BEGIN " + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + htab1 + "SET @SGL_RID  = -1;" + System.Environment.NewLine;
//                        sSQL += htab1 + htab1 + "END" + System.Environment.NewLine;
//                        sSQL += htab1 + "END" + System.Environment.NewLine;

//                        sSQL += htab1 + "IF (@SGL_RID != -1)" + System.Environment.NewLine;
//                        sSQL += htab1 + "BEGIN" + System.Environment.NewLine;
//                        sSQL += htab1 + "--Insert new entry into the group level join" + System.Environment.NewLine;
//                        sSQL += htab1 + "SELECT @SGL_VERSION= COALESCE(MAX(SGL_VERSION), 0) FROM STORE_GROUP_JOIN WHERE SG_RID=258 AND SG_VERSION=1 AND SGL_RID=@SGL_RID" + System.Environment.NewLine;
//                        sSQL += htab1 + "SET @SGL_VERSION = @SGL_VERSION + 1;" + System.Environment.NewLine;

//                        sSQL += htab1 + "INSERT INTO STORE_GROUP_JOIN (SG_RID,SG_VERSION,SGL_RID,SGL_VERSION,STORE_COUNT,SGL_OVERRIDE_ID,SGL_OVERRIDE_SEQUENCE)" + System.Environment.NewLine;
//                        sSQL += htab1 + "VALUES (@SG_RID,@SG_VERSION,@SGL_RID,@SGL_VERSION,NULL,@SGL_OVERRIDE_ID,@SGL_OVERRIDE_SEQUENCE)" + System.Environment.NewLine;

//                        sSQL += htab1 + "--Insert new results" + System.Environment.NewLine;
//                        sSQL += htab1 + "INSERT INTO STORE_GROUP_LEVEL_RESULTS (SGL_RID,SGL_VERSION,ST_RID)" + System.Environment.NewLine;
//                        sSQL += htab1 + "SELECT @SGL_RID, @SGL_VERSION, ST_RID FROM @ALL_STORES WHERE LEVEL_SEQ = @SGL_OVERRIDE_SEQUENCE" + System.Environment.NewLine;
//                        sSQL += htab1 + "END" + System.Environment.NewLine;
//                    }
//                }


//                sSQL += htab1 + "SELECT * FROM @ALL_STORES" + System.Environment.NewLine;

//                FilterData dlFilters = new FilterData();
//                DataTable dt = dlFilters.ExecuteSqlForResultSet(sSQL);

//                return dt;
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.HandleException(ex);
//                return null;
//            }
//        }

     

//        private static void MakeSQLForConditions(ConditionNode cn, ref string sSQL)
//        {
//            if (cn.ConditionNodes.Count > 0)
//            {
//                BuildSqlForLogic(cn.condition, ref sSQL);
//                sSQL += "(";
//                MakeSQLForCondition(cn, ref sSQL);
//                sSQL += System.Environment.NewLine;
//                tabLevel += 1;
//                foreach (ConditionNode cChild in cn.ConditionNodes)
//                {
//                    MakeSQLForConditions(cChild, ref sSQL);
//                }
//                sSQL += GetTab() + ")" + System.Environment.NewLine;
//                tabLevel -= 1;
//            }
//            else
//            {
//                BuildSqlForLogic(cn.condition, ref sSQL);
//                MakeSQLForCondition(cn, ref sSQL);
//                sSQL += System.Environment.NewLine;
//            }
//        }

//        private static void FindExclusionListCondition(ConditionNode cn, ref ConditionNode cnExclusionList)
//        {

//            if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList)
//            {
//                cnExclusionList = cn;
//            }
//            else
//            {
//                foreach (ConditionNode cChild in cn.ConditionNodes)
//                {
//                    FindExclusionListCondition(cChild, ref cnExclusionList);
//                }
//            }
//        }
//        private static void MakeDateVariableSQLForConditions(ConditionNode cn, ref string sSQL, ref bool firstDynamicDate)
//        {
//            if (cn.ConditionNodes.Count > 0)
//            {
//                //BuildSqlForLogic(cn.condition, ref sSQL);
//                //sSQL += "(";
//                MakeDateVariableSQLForCondition(cn, ref sSQL, ref firstDynamicDate);
//                //sSQL += System.Environment.NewLine;
//                //tabLevel += 1;
//                foreach (ConditionNode cChild in cn.ConditionNodes)
//                {
//                    MakeDateVariableSQLForCondition(cChild, ref sSQL, ref firstDynamicDate);
//                }
//                //sSQL += GetTab() + ")" + System.Environment.NewLine;
//                //tabLevel -= 1;
//            }
//            else
//            {
//                //BuildSqlForLogic(cn.condition, ref sSQL);
//                MakeDateVariableSQLForCondition(cn, ref sSQL, ref firstDynamicDate);
//                //sSQL += System.Environment.NewLine;
//            }
//        }
//        private static void MakeDateVariableSQLForCondition(ConditionNode cn, ref string sSQL, ref bool firstDynamicDate)
//        {
//            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);
//            filterCondition fc = cn.condition;
//            if (et == filterDictionary.StoreFields)
//            {
//                filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
//                if (valueType == filterValueTypes.Date)
//                {
//                    BuildSqlDateVariablesForSmallDate(fc, ref sSQL, ref firstDynamicDate);
//                }
//            }

//        }


//        private static void MakeSQLForCondition(ConditionNode cn, ref string sSQL)
//        {
//            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);
//            if (et == filterDictionary.StoreFields)
//            {
//                BuildSqlForStoreFields(cn.condition, ref sSQL);
//            }
//            else if (et == filterDictionary.StoreCharacteristics)
//            {
//                BuildSqlForStoreCharacteristics(cn.condition, ref sSQL);
//            }
//            else if (et == filterDictionary.StoreList)
//            {
//                BuildSqlForListComparison("s.ST_RID", cn.condition, ref sSQL, filterListValueTypes.StoreRID);
//            }
//            else if (et == filterDictionary.StoreStatus)
//            {
//                BuildSqlForStoreStatus(cn.condition, ref sSQL);
//            }
//            else if (et == filterDictionary.StoreGroupName)
//            {
//                sSQL = "1=1";
//            }
//            else if (et == filterDictionary.StoreGroupExclusionList)
//            {
//                sSQL = "1=1";
//            }
           
//        }



//        private static void BuildSqlForLogic(filterCondition fc, ref string sSQL)
//        {
//            if (filterLogicTypes.FromIndex(fc.logicIndex) == filterLogicTypes.And)
//            {
//                sSQL += GetTab() + "AND ";
//            }
//            else
//            {
//                sSQL += GetTab() + "OR ";
//            }
//        }
//        private static void BuildSqlForStoreFields(filterCondition fc, ref string sSQL)
//        {
//            filterStoreFieldTypes fieldType = filterStoreFieldTypes.FromIndex(fc.fieldIndex);
//            if (fieldType == filterStoreFieldTypes.StoreID)
//            {
//                BuildSqlForStringComparison("s.ST_ID", fc, ref sSQL);
//            }
//            else if (fieldType == filterStoreFieldTypes.StoreName) 
//            {
//                BuildSqlForStringComparison("s.STORE_NAME", fc, ref sSQL);
//            }
//            else if (fieldType == filterStoreFieldTypes.City)
//            {
//                BuildSqlForStringComparison("s.CITY", fc, ref sSQL);
//            }
//            else if (fieldType == filterStoreFieldTypes.State) 
//            {
//                BuildSqlForStringComparison("s.STATE", fc, ref sSQL);
//            }
//            else if (fieldType == filterStoreFieldTypes.SellingSqFt) 
//            {
//                BuildSqlForIntComparison("s.SELLING_SQ_FT", fc, ref sSQL);
//            }
//            else if (fieldType == filterStoreFieldTypes.SellingOpenDate)
//            {
//                BuildSqlForSmallDateComparison("s.SELLING_OPEN_DATE", fc, ref sSQL);
//            }
           
            

//        }
//        private static void BuildSqlForStoreCharacteristics(filterCondition fc, ref string sSQL)
//        {

//            filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
//            if (valueType == filterValueTypes.Date || valueType == filterValueTypes.Dollar || valueType == filterValueTypes.Numeric || valueType == filterValueTypes.Text)
//            {
//                int scg_rid = fc.fieldIndex; //corresponds to STORE_CHAR_GROUP SCG_RID, example 3=district


//                sSQL += "s.ST_RID IN " + System.Environment.NewLine;
//                sSQL += GetTab() + "( " + System.Environment.NewLine;
//                tabLevel++;
//                sSQL += GetTab() + "SELECT ST_RID " + System.Environment.NewLine;
//                sSQL += GetTab() + "FROM " + System.Environment.NewLine;
//                sSQL += GetTab() + "STORE_CHAR_JOIN scj WITH (NOLOCK) " + System.Environment.NewLine;
//                sSQL += GetTab() + "WHERE SC_RID IN " + System.Environment.NewLine;
//                tabLevel++;
//                sSQL += GetTab() + "( " + System.Environment.NewLine;
//                sSQL += GetTab() + "SELECT SC_RID " + System.Environment.NewLine;
//                sSQL += GetTab() + "FROM STORE_CHAR sc WITH (NOLOCK) " + System.Environment.NewLine;
//                sSQL += GetTab() + "WHERE SCG_RID = " + scg_rid.ToString() + System.Environment.NewLine;
//                if (valueType == filterValueTypes.Date)
//                {
//                    sSQL += GetTab() + "AND ";
//                    BuildSqlForSmallDateComparison("sc.DATE_VALUE", fc, ref sSQL);
//                    sSQL += System.Environment.NewLine;
//                }
//                else if (valueType == filterValueTypes.Dollar)
//                {
//                    sSQL += GetTab() + "AND ";
//                    BuildSqlForDoubleComparison("sc.DOLLAR_VALUE", fc, ref sSQL);
//                    sSQL += System.Environment.NewLine;
//                }
//                else if (valueType == filterValueTypes.Numeric)
//                {
//                    sSQL += GetTab() + "AND ";
//                    BuildSqlForDoubleComparison("sc.NUMBER_VALUE", fc, ref sSQL); 
//                    sSQL += System.Environment.NewLine;
//                }
//                else if (valueType == filterValueTypes.Text)
//                {
//                    sSQL += GetTab() + "AND ";
//                    BuildSqlForStringComparison("sc.TEXT_VALUE", fc, ref sSQL);
//                    sSQL += System.Environment.NewLine;
//                }
//                sSQL += GetTab() + ") " + System.Environment.NewLine;
//                tabLevel--;
//                tabLevel--;
//                sSQL += GetTab() + ") " + System.Environment.NewLine;
//            }
//            else if (valueType == filterValueTypes.List)
//            {
//                filterListConstantTypes listType = fc.listConstantType;

//                if (listType == filterListConstantTypes.All)
//                {
//                    sSQL += "1=1 --All Characteristic Values";
//                }
//                else
//                {
//                    if (listType == filterListConstantTypes.None)
//                    {
//                        sSQL += "1=2 --No Characteristic Values";
//                    }
//                    else
//                    {
//                        DataRow[] listValues = fc.GetListValues(filterListValueTypes.HeaderCharacteristicRID);
//                        if (listValues.Length == 0)
//                        {
//                            sSQL += "1=2 --No Selected Values Saved";
//                        }
//                        else
//                        {
//                            sSQL += "s.ST_RID IN " + System.Environment.NewLine;
//                            sSQL += GetTab() + "( " + System.Environment.NewLine;
//                            tabLevel++;
//                            sSQL += GetTab() + "SELECT ST_RID " + System.Environment.NewLine;
//                            sSQL += GetTab() + "FROM " + System.Environment.NewLine;
//                            sSQL += GetTab() + "STORE_CHAR_JOIN scj WITH (NOLOCK) " + System.Environment.NewLine;
//                            sSQL += GetTab() + "WHERE SC_RID IN (";
//                            bool firstStatus = true;
//                            foreach (DataRow dr in listValues)
//                            {
//                                int listValueIndex = (int)dr["LIST_VALUE_INDEX"];
//                                if (firstStatus == false)
//                                {
//                                    sSQL += ",";
//                                }
//                                else
//                                {
//                                    firstStatus = false;
//                                }
//                                sSQL += listValueIndex.ToString();
//                            }
//                            sSQL += ") " + System.Environment.NewLine;
//                            tabLevel--;
//                            sSQL += GetTab() + ") " + System.Environment.NewLine;
//                        }
//                    }
//                }
//            }
//            else
//            {
//                sSQL += "1=1 --Unknown characteristic value type";
//            }

//        }
  
//        private static void BuildSqlForStoreStatus(filterCondition fc, ref string sSQL)
//        {
//            sSQL += "1=1 --All Store Statuses";

//            //Need to get store store status at the database level  //TODO -Attribute Set Filter
//            //Search for this in the solution: static public eStoreStatus GetStoreStatus(WeekProfile baseWeek, DateTime sellingOpenDt, DateTime sellingCloseDt)

//        }
      
//        private static void BuildSqlForStringComparison(string val1, filterCondition fc, ref string sSQL)
//        {
//            string val2 = fc.valueToCompare;
//            //escape val2
//            val2 = val2.Replace("'", "''");
//            val2 = val2.Replace("[", "[[]");
//            val2 = val2.Replace("%", "[%]");
//            val2 = val2.Replace("_", "[_]");
//            filterStringOperatorTypes stringOp = filterStringOperatorTypes.FromIndex(fc.operatorIndex);
//            if (stringOp == filterStringOperatorTypes.Contains)
//            {
//                sSQL += "UPPER(" + val1 + ") LIKE '%" + val2.ToUpper() + "%'";
//            }
//            else if (stringOp == filterStringOperatorTypes.ContainsExactly)
//            {
//                sSQL += val1 + " LIKE '%" + val2 + "%'";
//            }
//            else if (stringOp == filterStringOperatorTypes.StartsWith)
//            {
//                sSQL += "UPPER(" + val1 + ") LIKE '" + val2.ToUpper() + "%'";
//            }
//            else if (stringOp == filterStringOperatorTypes.StartsExactlyWith)
//            {
//                sSQL += val1 + " LIKE '" + val2 + "%'";
//            }
//            else if (stringOp == filterStringOperatorTypes.EndsWith)
//            {
//                sSQL += "UPPER(" + val1 + ") LIKE '%" + val2.ToUpper() + "'";
//            }
//            else if (stringOp == filterStringOperatorTypes.EndsExactlyWith)
//            {
//                sSQL += val1 + " LIKE '%" + val2 + "'";
//            }
//            else if (stringOp == filterStringOperatorTypes.DoesEqual)
//            {
//                sSQL += "UPPER(" + val1 + ") = '" + val2.ToUpper() + "'";
//            }
//            else if (stringOp == filterStringOperatorTypes.DoesEqualExactly)
//            {
//                sSQL += val1 + " = '" + val2 + "'";
//            }
//            else
//            {
//                sSQL += val1 + " = '" + val2 + "'";
//            }


//        }

//        private static void BuildSqlForDoubleComparison(string val1, filterCondition fc, ref string sSQL)
//        {
//            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
//            //filterNumericTypes nType = filterNumericTypes.FromIndex(fc.numericTypeIndex);
//            double val2 = (double)fc.valueToCompareDouble;

//            //escape val2
//            if (numericOp == filterNumericOperatorTypes.DoesEqual)
//            {
//                sSQL += val1 + " = " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
//            {
//                sSQL += val1 + " <> " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.GreaterThan)
//            {
//                sSQL += val1 + " > " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
//            {
//                sSQL += val1 + " >= " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.LessThan)
//            {
//                sSQL += val1 + " < " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
//            {
//                sSQL += val1 + " <= " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.Between)
//            {
//                double val3 = (double)fc.valueToCompareDouble2;
//                sSQL += "(" + val1 + " >= " + val2.ToString() + " AND " + val1 + " <= " + val3.ToString() + ") ";
//            }


//        }
//        private static void BuildSqlForIntComparison(string val1, filterCondition fc, ref string sSQL)
//        {
//            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
//            //filterNumericTypes nType = filterNumericTypes.FromIndex(fc.numericTypeIndex);
//            int val2 = (int)fc.valueToCompareInt;

//            //escape val2
//            if (numericOp == filterNumericOperatorTypes.DoesEqual)
//            {
//                sSQL += val1 + " = " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
//            {
//                sSQL += val1 + " <> " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.GreaterThan)
//            {
//                sSQL += val1 + " > " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
//            {
//                sSQL += val1 + " >= " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.LessThan)
//            {
//                sSQL += val1 + " < " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
//            {
//                sSQL += val1 + " <= " + val2.ToString();
//            }
//            else if (numericOp == filterNumericOperatorTypes.Between)
//            {
//                int val3 = (int)fc.valueToCompareInt2;
//                sSQL += "(" + val1 + " >= " + val2.ToString() + " AND " + val1 + " <= " + val3.ToString() + ") ";
//            }


//        }
//        private static void BuildSqlForSmallDateComparison(string val1, filterCondition fc, ref string sSQL)
//        {
//            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Unrestricted)
//            {
//                sSQL += "1=1 --Unrestricted date range";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
//            {
//                sSQL += "(" + val1 + " >= @SDT_LAST_24HR_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_NOW)";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
//            {
//                sSQL += "(" + val1 + " >= @SDT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_NOW)";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
//            {
//                sSQL += "(" + val1 + " >= @SDT_BTWN_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_BTWN_TO_" + GetConditionUID(fc) + ")";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
//            {
//                sSQL += "(" + val1 + " >= @SDT_SPCFY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @SDT_SPCFY_TO_" + GetConditionUID(fc) + ")";
//            }
//        }
//        private static void BuildSqlForDateTimeComparison(string val1, filterCondition fc, ref string sSQL)
//        {
//            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Unrestricted)
//            {
//                sSQL += "1=1 --Unrestricted date range";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
//            {
//                sSQL += "(" + val1 + " >= @DT_LAST_24HR_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_NOW_WITH_TIME)";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
//            {
//                sSQL += "(" + val1 + " >= @DT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_NOW_WITH_TIME)";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
//            {
//                sSQL += "(" + val1 + " >= @DT_BTWN_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_BTWN_TO_" + GetConditionUID(fc) + ")";
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
//            {
//                sSQL += "(" + val1 + " >= @DT_SPCFY_FROM_" + GetConditionUID(fc) + " AND " + val1 + " <= @DT_SPCFY_TO_" + GetConditionUID(fc) + ")";
//            }
//        }

//        private static void BuildSqlDateVariablesForSmallDate(filterCondition fc, ref string sSQL, ref bool firstDynamicDate)
//        {
//            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
//            {
//                if (firstDynamicDate == false)
//                {
//                    firstDynamicDate = true;
//                    MakeDateDynamicSQL(ref sSQL);
//                }
//                sSQL += htab1 + "DECLARE @SDT_LAST_24HR_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(hh, -24, @SDT_NOW), 112);" + System.Environment.NewLine;
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
//            {
//                if (firstDynamicDate == false)
//                {
//                    firstDynamicDate = true;
//                    MakeDateDynamicSQL(ref sSQL);
//                }

//                //go from midnight to midnight
//                sSQL += htab1 + "DECLARE @SDT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(dd, -7, @SDT_NOW), 112);" + System.Environment.NewLine;
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
//            {
//                if (firstDynamicDate == false)
//                {
//                    firstDynamicDate = true;
//                    MakeDateDynamicSQL(ref sSQL);
//                }
//                int daysFrom = fc.valueToCompareDateBetweenFromDays;
//                int daysTo = fc.valueToCompareDateBetweenToDays;

//                sSQL += htab1 + "DECLARE @SDT_BTWN_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(dd, " + daysFrom.ToString() + ", @SDT_NOW), 112);" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SDT_BTWN_TO_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, DATEADD(dd, " + daysTo.ToString() + ", @SDT_NOW), 112);" + System.Environment.NewLine;
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
//            {
//                DateTime dateFrom = (DateTime)fc.valueToCompareDateFrom;
//                DateTime dateTo = (DateTime)fc.valueToCompareDateTo;
//                sSQL += htab1 + "DECLARE @SDT_SPCFY_FROM_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, '" + dateFrom.ToString("yyyyMMdd") + "', 112);" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @SDT_SPCFY_TO_" + GetConditionUID(fc) + " SMALLDATETIME = CONVERT(SMALLDATETIME, '" + dateTo.ToString("yyyyMMdd") + "', 112);" + System.Environment.NewLine;
//            }
//        }
//        private static void MakeDateDynamicSQL(ref string sSQL)
//        {
//            sSQL += htab1 + "DECLARE @DT_NOW_WITH_TIME DATETIME = GETDATE();" + System.Environment.NewLine;
//            sSQL += htab1 + "DECLARE @SDT_NOW SMALLDATETIME = CONVERT(SMALLDATETIME, CONVERT(CHAR(8), @DT_NOW_WITH_TIME, 112), 112);" + System.Environment.NewLine;
//            sSQL += System.Environment.NewLine;
//        }
//        private static void BuildSqlDateVariablesForDateTime(filterCondition fc, ref string sSQL, ref bool firstDynamicDate)
//        {
//            if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last24Hours)
//            {
//                if (firstDynamicDate == false)
//                {
//                    firstDynamicDate = true;
//                    MakeDateDynamicSQL(ref sSQL);
//                }
//                sSQL += htab1 + "DECLARE @DT_LAST_24HR_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(hh, -24, @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Last7Days)
//            {
//                if (firstDynamicDate == false)
//                {
//                    firstDynamicDate = true;
//                    MakeDateDynamicSQL(ref sSQL);
//                }

//                //go from midnight to midnight
//                sSQL += htab1 + "DECLARE @DT_LAST_7DAY_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, -7, @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Between)
//            {
//                if (firstDynamicDate == false)
//                {
//                    firstDynamicDate = true;
//                    MakeDateDynamicSQL(ref sSQL);
//                }
//                int daysFrom = fc.valueToCompareDateBetweenFromDays;
//                int daysTo = fc.valueToCompareDateBetweenToDays;

//                sSQL += htab1 + "DECLARE @DT_BTWN_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysFrom.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @DT_BTWN_TO_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysTo.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
//            }
//            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
//            {
//                DateTime dateFrom = (DateTime)fc.valueToCompareDateFrom;
//                DateTime dateTo = (DateTime)fc.valueToCompareDateTo;
//                sSQL += htab1 + "DECLARE @DT_SPCFY_FROM_" + GetConditionUID(fc) + " DATETIME = CONVERT(DATETIME, '" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 120);" + System.Environment.NewLine;
//                sSQL += htab1 + "DECLARE @DT_SPCFY_TO_" + GetConditionUID(fc) + " DATETIME = CONVERT(DATETIME, '" + dateTo.ToString("yyyy-MM-dd HH:mm:ss") + "', 120);" + System.Environment.NewLine;
//            }
//        }

//        private static void BuildSqlForListComparison(string field, filterCondition fc, ref string sSQL, filterListValueTypes listValueType)
//        {
//            filterListConstantTypes listType = fc.listConstantType;
//            filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
//            if (listType == filterListConstantTypes.All)
//            {
//                if (listOp == filterListOperatorTypes.Excludes)
//                {
//                    sSQL += "1=2 --None";
//                }
//                else
//                {
//                    sSQL += "1=1 --All";
//                }
//            }
//            else
//            {
//                if (listType == filterListConstantTypes.None)
//                {
//                    if (listOp == filterListOperatorTypes.Excludes)
//                    {
//                        sSQL += "1=1 --All ";
//                    }
//                    else
//                    {
//                        sSQL += "1=2 --None";
//                    }
//                }
//                else
//                {
//                    sSQL += field + " ";
//                    //filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
//                    if (listOp == filterListOperatorTypes.Excludes)
//                    {
//                        sSQL += "NOT IN ";
//                    }
//                    else
//                    {
//                        sSQL += "IN ";
//                    }

//                    sSQL += "(";
//                    tabLevel++;

//                    DataRow[] listValues = fc.GetListValues(listValueType);
//                    bool firstStatus = true;
//                    foreach (DataRow dr in listValues)
//                    {
//                        int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

//                        if (firstStatus == false)
//                        {
//                            sSQL += ",";
//                        }
//                        else
//                        {
//                            firstStatus = false;
//                        }
//                        sSQL += listValueIndex.ToString();
//                    }

//                    tabLevel--;
//                    sSQL += GetTab() + ") " + System.Environment.NewLine;
//                }
//            }
//        }



//        /// <summary>
//        /// Returns a unique identifier for this condition.
//        /// Not using RIDs in case the filter is not saved yet on the database, and so stored procedures can be copied more easily, if the need ever arises.
//        /// </summary>
//        /// <param name="fc"></param>
//        /// <returns></returns>
//        private static string GetConditionUID(filterCondition fc)
//        {
//            return fc.Seq.ToString();
//        }
//    }
//}
