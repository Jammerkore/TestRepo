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
    /// used to temporary hold level info while making profiles and inserting everything for groups onto the database
    /// </summary>
    public class levelInfo
    {
        public int levelRID;
        public string levelName;
        public int levelSeq;
        public int levelConditionRID;
        public int levelVersion;
        public eGroupLevelTypes levelType;
        public string levelValues;
        public storeObjectTypes storeObjectType;
        public string levelFields;
        public levelInfo(int levelRID, string levelName, int levelSeq, int levelConditionRID, eGroupLevelTypes levelType, int levelVersion)
        {
            this.levelRID = levelRID;
            this.levelName = levelName;
            this.levelSeq = levelSeq;
            this.levelConditionRID = levelConditionRID;
            this.levelType = levelType;
            this.levelVersion = levelVersion;
        }
    }

    /// <summary>
    /// Used for store groups (aka attributes)
    /// </summary>
    public static class filterEngineSQLForStoreGroup
    {
        private static string htab1 = "\t";
        private static string htab2 = "\t\t";
        private static string htab3 = "\t\t\t";
        private static int tabLevel = 2;
        private static int maxGroupLevelLength = 50;  // TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.

        // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        static filterEngineSQLForStoreGroup()
        {
            FilterData dlFilters = new FilterData();
            maxGroupLevelLength = dlFilters.GetColumnSize("STORE_GROUP_LEVEL", "SGL_ID");
        }
        // End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.

        private static string GetTab()
        {
            string htab = string.Empty;
            for (int i = 1; i <= tabLevel; i++)
            {
                htab += "\t";
            }
            return htab;
        }

        public static DataSet ExecuteFilter(ref List<levelInfo> tempLevelList, filter f, int groupRID, int groupVersion)
        {
            //bool AllowDelete = true;
            //string sqlText;
            //SystemData sd = new SystemData();

            //sd.GetInUseData(groupRID, (int)eProfileType.StoreGroup, out AllowDelete);

            ////TT1627-MD Store Attribute Conversion - SRisch 
            //if (!AllowDelete )
            //{
            //   DataSet ds = null;
            //   FilterData dlFilters = new FilterData();
            //   dlFilters.OpenUpdateConnection();
            //   try
            //    {
            //        DataTable dt =  dlFilters.ExecuteSQLQuery("Select * from STORE_GROUP_LEVEL where SG_RID = " + groupRID,"STORE_GROUP_LEVEL");
            //        foreach(DataRow dr in dt.Rows)
            //        {
            //            sqlText = "INSERT INTO STORE_GROUP_JOIN (SG_RID, SG_VERSION, SGL_RID, SGL_VERSION, STORE_COUNT) VALUES( " + groupRID + ", 1, " + (int)dr["SGL_RID"] + ", " + (int)dr["SGL_VERSION"] + ",0)";
                          
            //            dlFilters.ExecuteNonQuery(sqlText);

                       
            //        }
            //    dlFilters.CommitData();
            //    }       
            //  catch (Exception ex)
            //   {  
            //    //UpdateLog(System.Environment.NewLine + "Error: Could not obtain store filter count..." + ex.ToString());
                
            //  }
                


            //}

            //TT1627-MD Store Attribute Conversion - SRisch 
            try
            {
                ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq()); //parent seq for conditions
                string sSQL = string.Empty;
                fieldDataTypes StoreCharType;


                //Build date variables for conditions so SQL does not perform these calcs per row
                bool firstDynamicDate = false;
      
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    MakeDateVariableSQLForConditions(cn, ref sSQL, ref firstDynamicDate);
                }
                ConditionNode cnExclusionList = null;
                foreach (ConditionNode cn in conditionRoot.ConditionNodes)
                {
                    FindExclusionListCondition(cn, ref cnExclusionList);
                }
                sSQL += htab1 + "SET NOCOUNT ON;" + System.Environment.NewLine;

                bool isDynamic = false;
                if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    isDynamic = true;
                }
                char strIsDynamic = Include.ConvertBoolToChar(isDynamic);
                sSQL += htab1 + "DECLARE @SG_RID INT" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @SG_VERSION INT" + System.Environment.NewLine;
                //sSQL += htab1 + "DECLARE @SC_RID INT" + System.Environment.NewLine;
                //sSQL += htab1 + "DECLARE @SCG_RID INT" + System.Environment.NewLine;

                bool isNewGroup;
                if (groupRID == -1)
                {
                    isNewGroup = true;
                }
                else
                {
                    isNewGroup = false;
                }

                // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                string filterName = f.filterName.Replace("'", "''");  //escape single quotes
                // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.

                if (isNewGroup)
                {
                    //insert new group
                    sSQL += htab1 + "SET @SG_VERSION = 1;" + System.Environment.NewLine;
                    sSQL += htab1 + "INSERT INTO STORE_GROUP (SG_ID,SG_DYNAMIC_GROUP_IND,USER_RID,FILTER_RID,IS_ACTIVE,SG_VERSION)" + System.Environment.NewLine;
                    sSQL += htab1 + "VALUES (" + System.Environment.NewLine;
                    // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                    //sSQL += htab1 + "'" + f.filterName + "'," + System.Environment.NewLine;
                    sSQL += htab1 + "'" + filterName + "'," + System.Environment.NewLine;
                    // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                    sSQL += htab1 + "'" + strIsDynamic + "'," + System.Environment.NewLine;
                    sSQL += htab1 + f.ownerUserRID.ToString() + "," + System.Environment.NewLine;
                    sSQL += htab1 + f.filterRID.ToString() + "," + System.Environment.NewLine;
                    sSQL += htab1 + "1," + System.Environment.NewLine;
                    sSQL += htab1 + "@SG_VERSION" + System.Environment.NewLine;
                    sSQL += htab1 + ")" + System.Environment.NewLine;

                    sSQL += htab1 + "SET @SG_RID = SCOPE_IDENTITY()" + System.Environment.NewLine;
                }
                else
                {
                   

                    //set the group rid and group version
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "SET @SG_RID = " + groupRID.ToString() + System.Environment.NewLine;
                    sSQL += htab1 + "SET @SG_VERSION  = " + groupVersion.ToString() + System.Environment.NewLine;
                }

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @FINAL_LEVELS AS TABLE (SGL_RID INT, SGL_VERSION INT, SGL_SEQUENCE INT, SGL_ID VARCHAR(50), STORE_COUNT INT, CONDITION_RID INT, LEVEL_TYPE INT, IS_NEW BIT)" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @ALL_STORES AS TABLE (ST_RID INT, LEVEL_SEQ INT)" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO @ALL_STORES SELECT ST_RID, -1 FROM STORES WITH (NOLOCK) WHERE ACTIVE_IND='1' " + System.Environment.NewLine;
               

                sSQL += htab1 + System.Environment.NewLine;

                StringBuilder sb = new StringBuilder();
                foreach (levelInfo li in tempLevelList)
                {
                    // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    if (li.levelName.Length > maxGroupLevelLength)
                    {
                        li.levelName = li.levelName.Substring(0, maxGroupLevelLength);
                    }
                    // End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                   // if (Convert.ToInt32(li.levelFields) == -99)
                    //{
                       // StoreCharType = GetCharDataType(li);
                    //} 
                    if (li.levelType == eGroupLevelTypes.Normal)
                    {
                        sSQL += htab1 + "--Level " + li.levelName + System.Environment.NewLine;
						// Begin TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
                        //sSQL += htab1 + "-- Process non list conditions " + System.Environment.NewLine;
                        sSQL += htab1 + "-- Process conditions " + System.Environment.NewLine;
						// End TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
                        sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = " + li.levelSeq + System.Environment.NewLine;
                        sSQL += htab1 + "WHERE LEVEL_SEQ = -1 AND ST_RID IN (SELECT ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' " + System.Environment.NewLine;
                        ConditionNode cn = f.FindConditionNodeByRid(li.levelConditionRID);

                        // Begin TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
						// Begin TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
						//MakeSQLForConditions(cn, ref sSQL); //Build the sql for the conditions for this level
                        int leftParenCount = 0;
                        MakeSQLForConditions(cn, ref sSQL, ref leftParenCount); //Build the sql for the conditions for this level
						// End TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
                        sSQL += htab1 + ") " + System.Environment.NewLine;
                        sSQL += htab1 + System.Environment.NewLine;

                        //ConditionNode cnStoreList = null;
                        //foreach (ConditionNode scn in cn.ConditionNodes)
                        //{
                        //    FindStoreListCondition(scn, ref cnStoreList);
                        //}

                        //MakeSQLForConditions(cn, ref sSQL, false); //Build the sql for the conditions for this level
                        //sSQL += htab1 + ") " + System.Environment.NewLine;
                        //sSQL += htab1 + System.Environment.NewLine;

                        //if (cnStoreList != null)
                        //{
                        //    sSQL += htab1 + "-- Process list conditions " + System.Environment.NewLine;
                        //    sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = " + li.levelSeq + System.Environment.NewLine;
                        //    sSQL += htab1 + "WHERE ST_RID IN (SELECT ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' " + System.Environment.NewLine;
                            
                        //    MakeSQLForConditions(cn, ref sSQL, true); //Build the sql for the conditions for this level
                        //    sSQL += htab1 + ") " + System.Environment.NewLine;
                        //    sSQL += htab1 + System.Environment.NewLine;
                        //}
                        // End TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
                    }
                    else if (li.levelType == eGroupLevelTypes.DynamicSet)
                    {
                        string[] valuesSplit = li.levelValues.Split('~');
                        string[] fieldsSplit = li.levelFields.Split('~');

                        // Disregard if all values are null
                        bool foundNonNull = false;
                        foreach (string val in valuesSplit)
                        {
                            if (!val.ToLower().StartsWith("null:"))
                            {
                                foundNonNull = true;
                                break;
                            }
                        }

                        if (!foundNonNull)
                        {
                            continue;
                        }

                        sb.Append(htab1 + "--Level " + li.levelName + System.Environment.NewLine);
                        sb.Append(htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = " + li.levelSeq + System.Environment.NewLine);
                        //sSQL += htab1 + "WHERE LEVEL_SEQ = -1 AND ST_RID IN (SELECT s.ST_RID FROM STORE_CHAR_JOIN scj WITH (NOLOCK) INNER JOIN STORES s WITH (NOLOCK) ON s.ST_RID=scj.ST_RID WHERE 1=1 " + System.Environment.NewLine;
                        sb.Append(htab1 + "WHERE LEVEL_SEQ = -1 " + System.Environment.NewLine);
                        //ConditionNode cn = f.FindConditionNodeByRid(li.levelConditionRID);

                        //string[] valuesSplit = li.levelValues.Split('~');
                        //string[] fieldsSplit = li.levelFields.Split('~');

                     
                        int myIndex = 0;
                        foreach (string val in valuesSplit)
                        {
                            int filterFieldIndex = Convert.ToInt32(fieldsSplit[myIndex]);
                            if (filterFieldIndex == -99)
                            {
							    // For null characteristic need to check against all stores in characteristic group
                                if (val.ToLower().StartsWith("null:"))
                                {
                                    // split value to get characteristic group RID in value to be used in query
                                    // select all stores that do not have a join record for the characteristic group
                                    string[] valSplit = val.Split(':');
                                    sb.Append(htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' AND s.ST_RID NOT IN (SELECT scj.ST_RID FROM STORE_CHAR_JOIN scj WITH (NOLOCK) INNER JOIN STORE_CHAR sc WITH (NOLOCK) on sc.SC_RID = scj.SC_RID WHERE sc.SCG_RID = " + valSplit [1].ToString() + "))  " + System.Environment.NewLine);
                                }
                                else
                                {
                                    int scRID = Convert.ToInt32(val);
                                    sb.Append(htab1 + "AND ST_RID IN (SELECT scj.ST_RID FROM STORE_CHAR_JOIN scj WITH (NOLOCK) INNER JOIN STORES s ON s.ST_RID=scj.ST_RID WHERE s.ACTIVE_IND='1' AND SC_RID=" + scRID.ToString() + ") " + System.Environment.NewLine);
                                }
                            }
                            else
                            {

                                filterStoreFieldTypes filterStoreFieldType = filterStoreFieldTypes.FromIndex(filterFieldIndex);
                                if (val.ToLower().StartsWith("null:"))
                                {
                                    sb.Append(htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' AND s." + filterStoreFieldType.storeFieldType.dbFieldName + " is null) " + System.Environment.NewLine);
                                }
                                else
                                {
                                    if (filterStoreFieldType.storeFieldType.dataType == fieldDataTypes.NumericInteger)
                                    {
                                        sb.Append(htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' AND s." + filterStoreFieldType.storeFieldType.dbFieldName + "=" + val + ") " + System.Environment.NewLine);
                                    }
                                    // Begin TT#1802-MD - JSmith - Some Store Field Attributes not converting as expected
                                    else if (filterStoreFieldType.storeFieldType.dataType == fieldDataTypes.Boolean)
                                    {
                                        string booleanVal = "1";
                                        if (val.ToUpper() == "FALSE")
                                        {
                                            booleanVal = "0";
                                        }

                                        sb.Append(htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' AND s." + filterStoreFieldType.storeFieldType.dbFieldName + "='" + booleanVal + "')" + System.Environment.NewLine);
                                    }
                                    // End TT#1802-MD - JSmith - Some Store Field Attributes not converting as expected
                                    else
                                    {
                                        sb.Append(htab1 + "AND ST_RID IN (SELECT s.ST_RID FROM STORES s WITH (NOLOCK) WHERE s.ACTIVE_IND='1' AND s." + filterStoreFieldType.storeFieldType.dbFieldName + "='" + val + "')" + System.Environment.NewLine);
                                    }
                                }
                            }
                            myIndex++;
                        }
                    }
                }

                sSQL += sb.ToString();

                //Exclude stores in the exclusion list
                DataRow[] listValues = cnExclusionList.condition.GetListValues(filterListValueTypes.StoreRID);
                if (listValues.Length > 0)
                {
                    sSQL += htab1 + "UPDATE @ALL_STORES SET LEVEL_SEQ = -1 WHERE ST_RID IN (";

                    bool firstOne = true;
                    foreach (DataRow dr in listValues)
                    {
                        int listValueIndex = (int)dr["LIST_VALUE_INDEX"];

                        if (firstOne == false)
                        {
                            sSQL += ",";
                        }
                        else
                        {
                            firstOne = false;
                        }
                        sSQL += listValueIndex.ToString();
                    }
                    sSQL += ") " + System.Environment.NewLine;
                    sSQL += htab1 + System.Environment.NewLine;
                }


                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @CUR_LEVELS TABLE (SGL_RID int, SGL_VERSION int, SQL_SEQUENCE int, SGL_ID varchar(50));" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO @CUR_LEVELS" + System.Environment.NewLine;
                sSQL += htab1 + "SELECT sgj.SGL_RID, sgj.SGL_VERSION, sgj.SGL_OVERRIDE_SEQUENCE, sgj.SGL_OVERRIDE_ID" + System.Environment.NewLine;
                sSQL += htab1 + "FROM STORE_GROUP_JOIN sgj WITH (NOLOCK)" + System.Environment.NewLine;
                sSQL += htab1 + "INNER JOIN STORE_GROUP_LEVEL sgl WITH (NOLOCK) ON sgl.SGL_RID=sgj.SGL_RID" + System.Environment.NewLine;
                sSQL += htab1 + "WHERE sgj.SG_RID=@SG_RID AND sgj.SG_VERSION=@SG_VERSION AND sgj.SGL_VERSION=sgl.SGL_VERSION" + System.Environment.NewLine;
                // Begin TT#1885-MD - JSmith - Stores missing from Str Attribute after re- adding sets
                // Add active attribute sets to @CUR_LEVELS that do not have a corresponding row in STORE_GROUP_JOIN so all levels are built
                // Must get the highest version of each set
                sSQL += htab1 + "UNION" + System.Environment.NewLine;
	            sSQL += htab1 + "SELECT DISTINCT x.SGL_RID, x.SGL_VERSION, x.SGL_SEQUENCE, x.SGL_ID" + System.Environment.NewLine;
                sSQL += htab1 + "FROM STORE_GROUP_LEVEL sgl WITH (NOLOCK)" + System.Environment.NewLine;
                sSQL += htab1 + "INNER JOIN" + System.Environment.NewLine;
                sSQL += htab1 + "(SELECT SG_RID, SGL_ID, max(SGL_RID) as SGL_RID, max(SGL_VERSION) as SGL_VERSION, max(SGL_SEQUENCE) as SGL_SEQUENCE" + System.Environment.NewLine;
                sSQL += htab1 + "FROM STORE_GROUP_LEVEL WITH (NOLOCK)" + System.Environment.NewLine;
                sSQL += htab1 + "GROUP BY SG_RID, SGL_ID) x " + System.Environment.NewLine;
                sSQL += htab1 + "ON sgl.SG_RID = x.SG_RID and sgl.SGL_ID = x.SGL_ID" + System.Environment.NewLine;
                sSQL += htab1 + "LEFT OUTER JOIN STORE_GROUP_JOIN sgj WITH (NOLOCK) ON sgj.SGL_RID=sgl.SGL_RID AND sgj.SGL_VERSION=sgl.SGL_VERSION" + System.Environment.NewLine;
                sSQL += htab1 + "WHERE sgl.SG_RID = @SG_RID AND sgl.IS_ACTIVE = 1 AND sgj.SGL_VERSION is null" + System.Environment.NewLine;
                sSQL += htab1 + "AND x.SGL_ID NOT IN (select SGL_ID from @CUR_LEVELS)" + System.Environment.NewLine;
                // End TT#1885-MD - JSmith - Stores missing from Str Attribute after re- adding sets
                sSQL += htab1 + System.Environment.NewLine;


                sSQL += htab1 + "--Insert final levels" + System.Environment.NewLine;

                 
                sb.Clear();
                foreach (levelInfo li in tempLevelList)
                {
                    // Begin TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
                    // Skip this set.  Stores should go to Available Stores
                    if (li.levelName == "None")
                    {
                        continue;
                    }
                    // End TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set

                    string levelName = li.levelName.Replace("'", "''");  //escape single quotes
                    levelName = levelName.Replace("[", "[[]");  
                    levelName = levelName.Replace("%", "[%]");
                    levelName = levelName.Replace("_", "[_]");

                    // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                    string levelNameForEqual = li.levelName.Replace("'", "''");  //escape single quotes
                    // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.

                    //if(Convert.ToInt32(li.levelFields) == -99)
                    //{
                    //    int scRID = Convert.ToInt32(li.levelValues.Split('~'));
                    //    //s
                    //}

                    // sSQL += htab1 + "select @SCG_TYPE = SCG_TYPE FROM STORE_CHAR_GROUP where SC_RID = " + scRID;
                    if (f.filterType == filterTypes.StoreGroupFilter)
                    {
                        if (li.levelRID != -1)
                        {
                            // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                            //sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_RID = " + li.levelRID.ToString() + "),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_RID = " + li.levelRID.ToString() + ")," + li.levelSeq.ToString() + "," + "'" + levelName + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = " + li.levelSeq.ToString() + ")," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                            sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_RID = " + li.levelRID.ToString() + "),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_RID = " + li.levelRID.ToString() + ")," + li.levelSeq.ToString() + "," + "'" + levelNameForEqual + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = " + li.levelSeq.ToString() + ")," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                            // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                        }
                        else
                        {
                            if (li.levelType == eGroupLevelTypes.AvailableStoreSet)
                            {
                                // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                                //sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "')," + li.levelSeq.ToString() + "," + "'" + levelName + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = -1)," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                                sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "')," + li.levelSeq.ToString() + "," + "'" + levelNameForEqual + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = -1)," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                                // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                            }
                            else
                            {
                                // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                                //sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "')," + li.levelSeq.ToString() + "," + "'" + levelName + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = " + li.levelSeq.ToString() + ")," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");       
                                sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "')," + li.levelSeq.ToString() + "," + "'" + levelNameForEqual + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = " + li.levelSeq.ToString() + ")," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                                // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                            }
                        }
                    }
                    else //dynamic
                    {
                        if (li.levelType == eGroupLevelTypes.AvailableStoreSet)
                        {
                            // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                            //sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "')," + li.levelSeq.ToString() + "," + "'" + levelName + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = -1)," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                            sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "')," + li.levelSeq.ToString() + "," + "'" + levelNameForEqual + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = -1)," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                            // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                        }
                        else
                        {
                            // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                            //sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelName + "')," + li.levelSeq.ToString() + "," + "'" + levelName + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = " + li.levelSeq.ToString() + ")," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                            sb.AppendLine(htab1 + "INSERT INTO @FINAL_LEVELS (SGL_RID,SGL_VERSION,SGL_SEQUENCE,SGL_ID,STORE_COUNT,CONDITION_RID,LEVEL_TYPE, IS_NEW) SELECT (SELECT c.SGL_RID FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "'),(SELECT CASE WHEN (c.SGL_VERSION  + 1 > 99999) THEN 1 ELSE (c.SGL_VERSION  + 1) END FROM @CUR_LEVELS c WHERE c.SGL_ID = '" + levelNameForEqual + "')," + li.levelSeq.ToString() + "," + "'" + levelNameForEqual + "',(SELECT COUNT(*) FROM @ALL_STORES WHERE LEVEL_SEQ = " + li.levelSeq.ToString() + ")," + li.levelConditionRID.ToString() + "," + ((int)li.levelType).ToString() + ", 0");
                            // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                        }
                    }
                   

                }
                sSQL += sb.ToString();

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "UPDATE @FINAL_LEVELS SET SGL_RID=-1 WHERE SGL_RID IS NULL;" + System.Environment.NewLine;
                sSQL += htab1 + "UPDATE @FINAL_LEVELS SET SGL_VERSION=1 WHERE SGL_VERSION IS NULL;" + System.Environment.NewLine;

                // Begin TT#1517-MD - stodd - new sets not getting added to database
                //if (isNewGroup)
                //{
				// End TT#1517-MD - stodd - new sets not getting added to database
                // Begin TT#1868-MD - JSmith - Control Service gets database errors during startup
                if (isNewGroup)
                {
                // End TT#1868-MD - JSmith - Control Service gets database errors during startup
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "--set all levels to inactive for this group" + System.Environment.NewLine;
                    sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL set IS_ACTIVE = 0 " + System.Environment.NewLine;
                    sSQL += htab1 + "WHERE SG_RID = " + groupRID.ToString() + System.Environment.NewLine;
                // Begin TT#1868-MD - JSmith - Control Service gets database errors during startup
                }
                // End TT#1868-MD - JSmith - Control Service gets database errors during startup

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--update level versions for existing levels only" + System.Environment.NewLine;
                sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL set SGL_VERSION = f.SGL_VERSION, IS_ACTIVE = 1 " + System.Environment.NewLine;
                sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                sSQL += htab1 + "WHERE f.SGL_RID != -1 AND STORE_GROUP_LEVEL.SGL_RID=f.SGL_RID" + System.Environment.NewLine;

				// Begin TT#1517-MD - stodd - new sets not getting added to database
                if (isNewGroup == false)
                {
                    sSQL += htab1 + "DECLARE @SGL_VERSION int, @LEVEL_TYPE INT" + System.Environment.NewLine;
                    sSQL += htab1 + "SELECT @SGL_VERSION = f.SGL_VERSION, @LEVEL_TYPE = f.LEVEL_TYPE FROM @FINAL_LEVELS f" + System.Environment.NewLine;
                    if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                    {
                        sSQL += htab1 + "WHERE f.SGL_RID = -1" + System.Environment.NewLine;
                    }
                    else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                    {
                        sSQL += htab1 + "WHERE f.SGL_RID = -1 AND (f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0)" + System.Environment.NewLine;
                    }
                }
				// End TT#1517-MD - stodd - new sets not getting added to database

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--insert new group levels" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO STORE_GROUP_LEVEL (SGL_SEQUENCE,SG_RID,SGL_ID,IS_ACTIVE,SGL_VERSION,LEVEL_TYPE) " + System.Environment.NewLine;
                sSQL += htab1 + "SELECT SGL_SEQUENCE, @SG_RID, f.SGL_ID, 1, f.SGL_VERSION, f.LEVEL_TYPE " + System.Environment.NewLine;
                sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;

                if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                {
                    sSQL += htab1 + "WHERE f.SGL_RID = -1" + System.Environment.NewLine;
                }
                else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    sSQL += htab1 + "WHERE f.SGL_RID = -1 AND (f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0)" + System.Environment.NewLine;
                }
                // Begin TT#1877-MD - JSmith - Error When Manually Adding a Store
                sSQL += htab1 + "  AND f.SGL_ID NOT IN (SELECT SGL_ID FROM STORE_GROUP_LEVEL WHERE SG_RID = @SG_RID)";
                // End TT#1877-MD - JSmith - Error When Manually Adding a Store

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--update the temp table with the newly inserted ids" + System.Environment.NewLine;
                sSQL += htab1 + "UPDATE f " + System.Environment.NewLine;
				// Begin TT#1880-MD - JSmith - Str Characteristic Value None get an error.
                //sSQL += htab2 + "SET f.IS_NEW = 1, f.SGL_RID = (SELECT SGL_RID FROM STORE_GROUP_LEVEL sgl WITH (NOLOCK) WHERE sgl.IS_ACTIVE=1 AND sgl.SG_RID=@SG_RID AND iif(isDate(sgl.SGL_ID)=1,format(convert(datetime,sgl.SGL_ID,110),'MM/dd/yyyy'),dbo.UDF_REMOVE_SPECIAL_CHARS(sgl.SGL_ID)) = iif(isDate(f.SGL_ID)=1,format(convert(datetime,f.SGL_ID,110),'MM/dd/yyyy'), dbo.UDF_REMOVE_SPECIAL_CHARS(f.SGL_ID))) " + System.Environment.NewLine;
                sSQL += htab2 + "SET f.IS_NEW = 1, f.SGL_RID = (SELECT SGL_RID FROM STORE_GROUP_LEVEL sgl WITH (NOLOCK) WHERE sgl.IS_ACTIVE=1 AND sgl.SG_RID=@SG_RID AND iif(isDate(sgl.SGL_ID)=1,format(convert(datetime,sgl.SGL_ID,110),'MM/dd/yyyy'),sgl.SGL_ID) = iif(isDate(f.SGL_ID)=1,format(convert(datetime,f.SGL_ID,110),'MM/dd/yyyy'), f.SGL_ID)) " + System.Environment.NewLine;
				// End TT#1880-MD - JSmith - Str Characteristic Value None get an error.
                sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                {
                    sSQL += htab1 + "WHERE f.SGL_RID = -1" + System.Environment.NewLine;
                }
                else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    sSQL += htab1 + "WHERE f.SGL_RID = -1 AND (f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0)" + System.Environment.NewLine;
                }

                // stodd
                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "UPDATE @FINAL_LEVELS SET SGL_RID=-1 WHERE SGL_RID IS NULL;" + System.Environment.NewLine;
                // stodd

                if (f.filterType == filterTypes.StoreGroupFilter)
                {
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "--update the filter conditions with the new level rids" + System.Environment.NewLine;
                    sSQL += htab1 + "UPDATE FILTER_CONDITION " + System.Environment.NewLine;
                    sSQL += htab2 + "SET FIELD_INDEX = f.SGL_RID " + System.Environment.NewLine;
                    sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                    sSQL += htab1 + "WHERE FILTER_CONDITION.FILTER_RID=" + f.filterRID.ToString() + " AND FILTER_CONDITION.CONDITION_RID=f.CONDITION_RID AND f.IS_NEW = 1" + System.Environment.NewLine;
                }
				// Begin TT#1517-MD - stodd - new sets not getting added to database
                //}
                //else
                //{
                //    sSQL += htab1 + System.Environment.NewLine;
                //    sSQL += htab1 + "--set all levels to inactive for this group" + System.Environment.NewLine;
                //    sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL set IS_ACTIVE = 0 " + System.Environment.NewLine;
                //    sSQL += htab1 + "WHERE SG_RID = " + groupRID.ToString() + System.Environment.NewLine;



                //    sSQL += htab1 + System.Environment.NewLine;
                //    sSQL += htab1 + "--update level versions for existing levels only" + System.Environment.NewLine;
                //    sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL set SGL_VERSION = f.SGL_VERSION, IS_ACTIVE = 1 " + System.Environment.NewLine;
                //    sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                //    sSQL += htab1 + "WHERE f.SGL_RID != -1 AND STORE_GROUP_LEVEL.SGL_RID=f.SGL_RID" + System.Environment.NewLine;

                //    sSQL += htab1 + "DECLARE @SGL_VERSION int, @LEVEL_TYPE INT" + System.Environment.NewLine;
                //    sSQL += htab1 + "SELECT @SGL_VERSION = f.SGL_VERSION, @LEVEL_TYPE = f.LEVEL_TYPE FROM @FINAL_LEVELS f" + System.Environment.NewLine;


                //    if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                //    {
                //        sSQL += htab1 + "WHERE f.SGL_RID = -1" + System.Environment.NewLine;
                //    }
                //    else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                //    {
                //        sSQL += htab1 + "WHERE f.SGL_RID = -1 AND (f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0)" + System.Environment.NewLine;
                //    }

                //    // Begin TT#1517-MD - stodd - new sets not getting added to database
                //    sSQL += htab1 + System.Environment.NewLine;
                //    sSQL += htab1 + "--insert new group levels" + System.Environment.NewLine;
                //    sSQL += htab1 + "INSERT INTO STORE_GROUP_LEVEL (SGL_SEQUENCE,SG_RID,SGL_ID,IS_ACTIVE,SGL_VERSION,LEVEL_TYPE) " + System.Environment.NewLine;
                //    sSQL += htab1 + "SELECT SGL_SEQUENCE, @SG_RID, f.SGL_ID, 1, f.SGL_VERSION, f.LEVEL_TYPE " + System.Environment.NewLine;
                //    sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;

                //    if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                //    {
                //        sSQL += htab1 + "WHERE f.SGL_RID = -1" + System.Environment.NewLine;
                //    }
                //    else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                //    {
                //        sSQL += htab1 + "WHERE f.SGL_RID = -1 AND (f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0)" + System.Environment.NewLine;
                //    }

                //    //sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL set IS_ACTIVE = 1,SGL_VERSION = @SGL_VERSION ,LEVEL_TYPE = @LEVEL_TYPE " + System.Environment.NewLine;
                //    //sSQL += htab1 + "WHERE SG_RID = @SG_RID  " + System.Environment.NewLine;
                //    ////sSQL += htab1 + "SELECT SGL_SEQUENCE, @SG_RID, f.SGL_ID, 1, f.SGL_VERSION, f.LEVEL_TYPE " + System.Environment.NewLine;
                //    ////sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                //    // End TT#1517-MD - stodd - new sets not getting added to database

                //    sSQL += htab1 + System.Environment.NewLine;
                //    sSQL += htab1 + "--update the temp table with the newly inserted ids" + System.Environment.NewLine;
                //    sSQL += htab1 + "UPDATE f " + System.Environment.NewLine;
                //    sSQL += htab2 + "SET f.IS_NEW = 1, f.SGL_RID = (SELECT SGL_RID FROM STORE_GROUP_LEVEL sgl WITH (NOLOCK) WHERE sgl.IS_ACTIVE=1 AND sgl.SG_RID=@SG_RID AND iif(isDate(sgl.SGL_ID)=1,format(convert(datetime,sgl.SGL_ID,110),'MM/dd/yyyy'),dbo.UDF_REMOVE_SPECIAL_CHARS(sgl.SGL_ID)) = iif(isDate(f.SGL_ID)=1,format(convert(datetime,f.SGL_ID,110),'MM/dd/yyyy'), dbo.UDF_REMOVE_SPECIAL_CHARS(f.SGL_ID))) " + System.Environment.NewLine;
                //    sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                //    if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                //    {
                //        sSQL += htab1 + "WHERE f.SGL_RID = -1" + System.Environment.NewLine;
                //    }
                //    else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                //    {
                //        sSQL += htab1 + "WHERE f.SGL_RID = -1 AND (f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0)" + System.Environment.NewLine;
                //    }

                //    if (f.filterType == filterTypes.StoreGroupFilter)
                //    {
                //        sSQL += htab1 + System.Environment.NewLine;
                //        sSQL += htab1 + "--update the filter conditions with the new level rids" + System.Environment.NewLine;
                //        sSQL += htab1 + "UPDATE FILTER_CONDITION " + System.Environment.NewLine;
                //        sSQL += htab2 + "SET FIELD_INDEX = f.SGL_RID " + System.Environment.NewLine;
                //        sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                //        sSQL += htab1 + "WHERE FILTER_CONDITION.FILTER_RID=" + f.filterRID.ToString() + " AND FILTER_CONDITION.CONDITION_RID=f.CONDITION_RID AND f.IS_NEW = 1" + System.Environment.NewLine;
                //    }
                //}
                // End TT#1517-MD - stodd - new sets not getting added to database

                if (isNewGroup == false)
                {
                    //update group version
                    groupVersion += 1;
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "--update group version" + System.Environment.NewLine;
                    sSQL += htab1 + "UPDATE STORE_GROUP set SG_VERSION = " + groupVersion.ToString() + System.Environment.NewLine;
                    sSQL += htab1 + "WHERE SG_RID = " + groupRID.ToString() + System.Environment.NewLine;
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "SET @SG_VERSION  = " + groupVersion.ToString() + System.Environment.NewLine;
                }

                // Begin TT#1887-MD - JSmith - Version Testing - change str set by user 'a' when user 'b' goes back to the attribute the store attribute value for # of strs in a set is 0 for every set.
                //sSQL += htab1 + System.Environment.NewLine;
                //sSQL += htab1 + "--delete older result entries" + System.Environment.NewLine;
                //sSQL += htab1 + "DELETE FROM STORE_GROUP_LEVEL_RESULTS " + System.Environment.NewLine;
                //sSQL += htab1 + "FROM STORE_GROUP_LEVEL sgl  " + System.Environment.NewLine;
                //sSQL += htab1 + "WHERE STORE_GROUP_LEVEL_RESULTS.SGL_RID=sgl.SGL_RID AND sgl.SG_RID = " + groupRID.ToString() + System.Environment.NewLine;

                //sSQL += htab1 + System.Environment.NewLine;
                //sSQL += htab1 + "--delete older join entries" + System.Environment.NewLine;
                //sSQL += htab1 + "DELETE FROM STORE_GROUP_JOIN " + System.Environment.NewLine;
                //sSQL += htab1 + "WHERE SG_RID = " + groupRID.ToString() + System.Environment.NewLine;
                // End TT#1887-MD - JSmith - Version Testing - change str set by user 'a' when user 'b' goes back to the attribute the store attribute value for # of strs in a set is 0 for every set.

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--insert the join entries" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO STORE_GROUP_JOIN (SG_RID,SG_VERSION,SGL_RID,SGL_VERSION,STORE_COUNT,SGL_OVERRIDE_ID,SGL_OVERRIDE_SEQUENCE) " + System.Environment.NewLine;
                sSQL += htab1 + "SELECT " + System.Environment.NewLine;
                sSQL += htab2 + "@SG_RID," + System.Environment.NewLine;
                sSQL += htab2 + "@SG_VERSION," + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_RID," + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_VERSION," + System.Environment.NewLine;
                sSQL += htab2 + "f.STORE_COUNT," + System.Environment.NewLine;
                // BEGIN TT#5803 - AGallagher - Create attribute set with the 4 digit store number as name and it converts to a date.
                sSQL += htab2 + "f.SGL_ID," + System.Environment.NewLine;
                //sSQL += htab2 + "iif(isDate(f.SGL_ID)=1,format(convert(datetime,f.SGL_ID,110),'MM/dd/yyyy'),f.SGL_ID)," + System.Environment.NewLine;
                // END TT#5803 - AGallagher - Create attribute set with the 4 digit store number as name and it converts to a date.
                sSQL += htab2 + "f.SGL_SEQUENCE " + System.Environment.NewLine;
                sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                sSQL += htab1 + "WHERE f.SGL_RID != -1 " + System.Environment.NewLine;

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--insert the join history entries" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO STORE_GROUP_JOIN_HISTORY (SG_RID,SG_VERSION,SGL_RID,SGL_VERSION,STORE_COUNT,SGL_OVERRIDE_ID,SGL_OVERRIDE_SEQUENCE) " + System.Environment.NewLine;
                sSQL += htab1 + "SELECT " + System.Environment.NewLine;
                sSQL += htab2 + "@SG_RID," + System.Environment.NewLine;
                sSQL += htab2 + "@SG_VERSION," + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_RID," + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_VERSION," + System.Environment.NewLine;
                sSQL += htab2 + "f.STORE_COUNT," + System.Environment.NewLine;
                // BEGIN TT#5803 - AGallagher - Create attribute set with the 4 digit store number as name and it converts to a date.
                sSQL += htab2 + "f.SGL_ID," + System.Environment.NewLine;
                //sSQL += htab2 + "iif(isDate(f.SGL_ID)=1,format(convert(datetime,f.SGL_ID,110),'MM/dd/yyyy'),f.SGL_ID)," + System.Environment.NewLine;
                // END TT#5803 - AGallagher - Create attribute set with the 4 digit store number as name and it converts to a date.
                sSQL += htab2 + "f.SGL_SEQUENCE " + System.Environment.NewLine;
                sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                sSQL += htab1 + "WHERE f.SGL_RID != -1 " + System.Environment.NewLine;

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--insert the result entries" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO STORE_GROUP_LEVEL_RESULTS (SGL_RID,SGL_VERSION,ST_RID) " + System.Environment.NewLine;
                sSQL += htab1 + "SELECT " + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_RID," + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_VERSION," + System.Environment.NewLine;
                sSQL += htab2 + "ST_RID" + System.Environment.NewLine;
                sSQL += htab1 + "FROM @ALL_STORES r INNER JOIN @FINAL_LEVELS f on (f.SGL_SEQUENCE = r.LEVEL_SEQ) OR (f.LEVEL_TYPE = 1 AND r.LEVEL_SEQ =-1) " + System.Environment.NewLine;
                sSQL += htab1 + "WHERE f.SGL_RID != -1 " + System.Environment.NewLine;

                sSQL += htab1 + System.Environment.NewLine;
                sSQL += htab1 + "--insert the result history entries" + System.Environment.NewLine;
                sSQL += htab1 + "INSERT INTO STORE_GROUP_LEVEL_RESULTS_HISTORY (SGL_RID,SGL_VERSION,ST_RID) " + System.Environment.NewLine;
                sSQL += htab1 + "SELECT " + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_RID," + System.Environment.NewLine;
                sSQL += htab2 + "f.SGL_VERSION," + System.Environment.NewLine;
                sSQL += htab2 + "ST_RID" + System.Environment.NewLine;
                sSQL += htab1 + "FROM @ALL_STORES r INNER JOIN @FINAL_LEVELS f on (f.SGL_SEQUENCE = r.LEVEL_SEQ) OR (f.LEVEL_TYPE = 1 AND r.LEVEL_SEQ =-1) " + System.Environment.NewLine;
                sSQL += htab1 + "WHERE f.SGL_RID != -1 " + System.Environment.NewLine;


         
                //Returning two datatables...

                //Return the final store group levels (attribute sets)
                if (f.filterType == filterTypes.StoreGroupFilter) //always create group levels for static filters, even if they have no store results in the level
                {
                    sSQL += htab1 + "SELECT * FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                }
                else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    sSQL += htab1 + "SELECT * FROM @FINAL_LEVELS f WHERE f.LEVEL_TYPE = 1 OR f.STORE_COUNT > 0" + System.Environment.NewLine;
                }

                //Return the new store group RID and the store group level version
                sSQL += htab1 + "SELECT @SG_RID AS SG_RID, @SG_VERSION AS SG_VERSION" + System.Environment.NewLine;


                bool AllowDelete = true;
                SystemData sd = new SystemData();

                sd.GetInUseData(groupRID, (int)eProfileType.StoreGroup, out AllowDelete);

                //TT1627-MD Store Attribute Conversion - SRisch 
                if (!AllowDelete )
                {
                    sSQL += htab1 + "Update STORE_GROUP_LEVEL SET IS_ACTIVE = 1 WHERE SG_RID = " + groupRID;
                }

                // Begin RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets
                if (!isNewGroup
                    && isDynamic)
                {
                    sSQL += htab1 + System.Environment.NewLine;
                    sSQL += htab1 + "--update sets inactive that have no stores" + System.Environment.NewLine;
                    DataTable dt;
                    ProfileList attributeSets = StoreMgmt.StoreGroup_GetLevelListViewList(groupRID);
                    foreach (StoreGroupLevelListViewProfile sgl in attributeSets)
                    {
                        if (sgl.Sequence != 2147483647)
                        {
                            dt = sd.GetInUseData(sgl.Key, (int)eProfileType.StoreGroupLevel, out AllowDelete);
                            if (dt.Rows.Count == 0)
                            {
                                sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL set IS_ACTIVE = 0 " + System.Environment.NewLine;
                                sSQL += htab1 + "FROM @FINAL_LEVELS f " + System.Environment.NewLine;
                                sSQL += htab1 + "WHERE STORE_GROUP_LEVEL.SGL_RID = " + sgl.Key.ToString() + " AND STORE_GROUP_LEVEL.SGL_RID=f.SGL_RID AND f.STORE_COUNT = 0" + System.Environment.NewLine;
                            }
                        }
                    }
                } 
                // End RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets

                // Begin TT#1868-MD - JSmith - Control Service gets database errors during startup
                // Check is slow
                //if (isNewGroup == false)
                //{
                //    sSQL += htab1 + System.Environment.NewLine;
                //    sSQL += htab1 + "--activate attribute sets that are in use" + System.Environment.NewLine;

                //    sSQL += htab1 + "DECLARE @SGL_RID int," + System.Environment.NewLine;
                //    sSQL += htab1 + "@return_value int," + System.Environment.NewLine;
                //    sSQL += htab1 + "@outAllowDelete int" + System.Environment.NewLine;

                //    sSQL += htab1 + "DECLARE CSGL CURSOR FORWARD_ONLY" + System.Environment.NewLine;
                //    sSQL += htab1 + "FOR" + System.Environment.NewLine;
                //    sSQL += htab1 + "SELECT SGL_RID" + System.Environment.NewLine;
                //    sSQL += htab1 + "FROM STORE_GROUP_LEVEL with (nolock)" + System.Environment.NewLine;
                //    sSQL += htab1 + "WHERE SG_RID = " + groupRID + System.Environment.NewLine;

                //    sSQL += htab1 + "OPEN CSGL" + System.Environment.NewLine;

                //    sSQL += htab1 + "FETCH NEXT" + System.Environment.NewLine;
                //    sSQL += htab1 + "FROM CSGL" + System.Environment.NewLine;
                //    sSQL += htab1 + "INTO @SGL_RID" + System.Environment.NewLine;

                //    sSQL += htab1 + "WHILE @@FETCH_STATUS = 0" + System.Environment.NewLine;
                //    sSQL += htab1 + "BEGIN" + System.Environment.NewLine;

                //    sSQL += htab1 + "EXEC	@return_value = [dbo].[SP_MID_DETAIL_ACCESS]" + System.Environment.NewLine;
                //    sSQL += htab1 + "@inUseType = 39," + System.Environment.NewLine;
                //    sSQL += htab1 + "@inUseRID = @SGL_RID," + System.Environment.NewLine;
                //    sSQL += htab1 + "@outAllowDelete = @outAllowDelete OUTPUT" + System.Environment.NewLine;

                //    sSQL += htab1 + "SELECT	@outAllowDelete as N'@outAllowDelete'" + System.Environment.NewLine;

                //    sSQL += htab1 + "UPDATE STORE_GROUP_LEVEL SET IS_ACTIVE = 1 where SGL_RID = @SGL_RID AND @outAllowDelete = 0" + System.Environment.NewLine;

                //    sSQL += htab1 + "FETCH NEXT" + System.Environment.NewLine;
                //    sSQL += htab1 + "FROM CSGL" + System.Environment.NewLine;
                //    sSQL += htab1 + "INTO @SGL_RID" + System.Environment.NewLine;

                //    sSQL += htab1 + "END" + System.Environment.NewLine;

                //    sSQL += htab1 + "CLOSE CSGL " + System.Environment.NewLine;

                //    sSQL += htab1 + "DEALLOCATE CSGL" + System.Environment.NewLine;
                //}
                // End TT#1868-MD - JSmith - Control Service gets database errors during startup

                //Debug.WriteLine(" ");
                //Debug.WriteLine("ExecuteFilter() SQL: " + sSQL);  
                
                DataSet ds = null;
                FilterData dlFilters = new FilterData();
                try
                {
                    dlFilters.OpenUpdateConnection();
                    ds = dlFilters.ExecuteSqlForAttributeFilters(sSQL, 0);
                    dlFilters.CommitData();
                }
                catch (Exception ex)
                {
                    string err = "Error executing filter: " + f.filterName + " (RID=" + f.filterRID.ToString() + "): " + ex.ToString();
                    ExceptionHandler.HandleException(err);
                }
                finally
                {
                    dlFilters.CloseUpdateConnection();
                }
                return ds;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return null;
            }
        }

     
        // Begin TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
        //private static void MakeSQLForConditions(ConditionNode cn, ref string sSQL, bool blProcessListConditions)
        //{
        //    if (cn.ConditionNodes.Count > 0)
        //    {
        //        BuildSqlForLogic(cn.condition, ref sSQL);
        //        sSQL += "( 1=1 ";
        //        MakeSQLForCondition(cn, ref sSQL, blProcessListConditions);
        //        sSQL += System.Environment.NewLine;
        //        tabLevel += 1;
        //        foreach (ConditionNode cChild in cn.ConditionNodes)
        //        {
        //            MakeSQLForConditions(cChild, ref sSQL, blProcessListConditions);
        //        }
        //        sSQL += GetTab() + ")" + System.Environment.NewLine;
        //        tabLevel -= 1;
        //    }
        //    else
        //    {
        //        BuildSqlForLogic(cn.condition, ref sSQL);
        //        string sCond = string.Empty;
        //        MakeSQLForCondition(cn, ref sCond, blProcessListConditions);
        //        if (sCond == string.Empty)
        //        {
        //            sCond = " 1=2 "; //No conditions were supplied for this group level
        //        }
        //        sSQL += sCond;
        //        sSQL += System.Environment.NewLine;
        //    }
        //}
        private static void MakeSQLForConditions(ConditionNode cn, ref string sSQL, ref int leftParenCount, bool blFirstSubCondition = false)
		
        {
            if (cn.ConditionNodes.Count > 0)
            {
                BuildSqlForLogic(cn.condition, ref sSQL);
                if (cn.NodeLevel <= 2)
                {
                    sSQL += "( 1=1 ";
                    ++leftParenCount;
                }
                else
                {
                    sSQL += "( " + System.Environment.NewLine;
                    ++leftParenCount;
                }
                MakeSQLForCondition(cn, ref sSQL);
                sSQL += System.Environment.NewLine;
                tabLevel += 1;
                bool firstSubCondition = true;
                int conditionCount = 0;
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    MakeSQLForConditions(cChild, ref sSQL, ref leftParenCount, firstSubCondition);
                    firstSubCondition = false;
                    ++conditionCount;
                    if (conditionCount == cn.ConditionNodes.Count)
                    {
                        if (leftParenCount > 0)
                        {
                            sSQL += GetTab() + ") " + System.Environment.NewLine;
                            --leftParenCount;
                        }
                    }
                }
                if (leftParenCount > 0)
                {
                    sSQL += GetTab() + ") " + System.Environment.NewLine;
                    --leftParenCount;
                }
                tabLevel -= 1;
            }
            else
            {
                BuildSqlForLogic(cn.condition, ref sSQL);
                if (blFirstSubCondition)
                {
                    sSQL += System.Environment.NewLine + GetTab() + "( ";
                    ++leftParenCount;
                }
                else
                {
                    sSQL += System.Environment.NewLine + GetTab();
                }
                string sCond = string.Empty;
                MakeSQLForCondition(cn, ref sCond);
                if (sCond == string.Empty)
                {
                    sCond = " 1=2 "; //No conditions were supplied for this group level
                }
                sSQL += sCond;
                sSQL += System.Environment.NewLine;
            }
        }
		// End TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
		
        private static void FindExclusionListCondition(ConditionNode cn, ref ConditionNode cnExclusionList)
        {

            if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList)
            {
                cnExclusionList = cn;
            }
            else
            {
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    FindExclusionListCondition(cChild, ref cnExclusionList);
                }
            }
        }

        private static void FindStoreListCondition(ConditionNode cn, ref ConditionNode cnExclusionList)
        {

            if (cn.condition.dictionaryIndex == filterDictionary.StoreList)
            {
                cnExclusionList = cn;
            }
            else
            {
                foreach (ConditionNode cChild in cn.ConditionNodes)
                {
                    FindStoreListCondition(cChild, ref cnExclusionList);
                }
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
            if (et == filterDictionary.StoreFields)
            {
                filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
                if (valueType == filterValueTypes.Date)
                {
                    BuildSqlDateVariablesForSmallDate(fc, ref sSQL, ref firstDynamicDate);
                }
            }

        }


        // Begin TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
        //private static void MakeSQLForCondition(ConditionNode cn, ref string sSQL, bool blProcessListConditions)
        private static void MakeSQLForCondition(ConditionNode cn, ref string sSQL)
        // End TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
        {
            filterDictionary et = filterDictionary.FromIndex(cn.condition.dictionaryIndex);

            // Begin TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error
			//if (blProcessListConditions && et != filterDictionary.StoreList)
            //{
            //    return;
            //}
            //else if (!blProcessListConditions && et == filterDictionary.StoreList)
            //{
            //    return;
            //}
			// End TT#1920-MD - JSmith - Nested Condition when create and select OK receive a Database Error

            if (et == filterDictionary.StoreFields)
            {
                BuildSqlForStoreFields(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.StoreCharacteristics)
            {
                BuildSqlForStoreCharacteristics(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.StoreList)
            {
                BuildSqlForListComparison("s.ST_RID", cn.condition, ref sSQL, filterListValueTypes.StoreRID);
            }
            else if (et == filterDictionary.StoreStatus)
            {
                BuildSqlForStoreStatus(cn.condition, ref sSQL);
            }
            else if (et == filterDictionary.StoreGroupName)
            {
                //sSQL = "1=1";
            }
            else if (et == filterDictionary.StoreGroupExclusionList)
            {
                //sSQL = "1=1";
            }
           
        }



        private static void BuildSqlForLogic(filterCondition fc, ref string sSQL)
        {
            if (filterLogicTypes.FromIndex(fc.logicIndex) == filterLogicTypes.Or)
            {
                sSQL += GetTab() + "OR ";
            }
            else
            {
                sSQL += GetTab() + "AND ";
            }
        }
        private static void BuildSqlForStoreFields(filterCondition fc, ref string sSQL)
        {
            filterStoreFieldTypes fieldType = filterStoreFieldTypes.FromIndex(fc.fieldIndex);
            if (fieldType == filterStoreFieldTypes.StoreID)
            {
                BuildSqlForStringComparison("s.ST_ID", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.StoreName) 
            {
                BuildSqlForStringComparison("s.STORE_NAME", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.City)
            {
                BuildSqlForStringComparison("s.CITY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.State) 
            {
                BuildSqlForStringComparison("s.STATE", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.SellingSqFt) 
            {
                BuildSqlForIntComparison("s.SELLING_SQ_FT", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.SellingOpenDate)
            {
                BuildSqlForSmallDateComparison("s.SELLING_OPEN_DATE", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.LeadTime)
            {
                BuildSqlForIntComparison("s.LEAD_TIME", fc, ref sSQL);
            }
            // Begin TT#1802-MD - JSmith - Some Store Field Attributes not converting as expected
            else if (fieldType == filterStoreFieldTypes.SellingCloseDate)
            {
                BuildSqlForSmallDateComparison("s.SELLING_CLOSE_DATE", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.StockOpenDate)
            {
                BuildSqlForSmallDateComparison("s.STOCK_OPEN_DATE", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.StockCloseDate)
            {
                BuildSqlForSmallDateComparison("s.STOCK_CLOSE_DATE", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnMonday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_MONDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnTuesday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_TUESDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnWednesday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_WEDNESDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnThursday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_THURSDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnFriday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_FRIDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnSaturday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_SATURDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.ShipOnSunday)
            {
                BuildSqlForBooleanComparison("s.SHIP_ON_SUNDAY", fc, ref sSQL);
            }
            else if (fieldType == filterStoreFieldTypes.SimilarStoreModel)
            {
                BuildSqlForBooleanComparison("s.SIMILAR_STORE_MODEL", fc, ref sSQL);
            }
            // End TT#1802-MD - JSmith - Some Store Field Attributes not converting as expected
            

        }
        private static void BuildSqlForStoreCharacteristics(filterCondition fc, ref string sSQL)
        {

            filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
            filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
            if (valueType == filterValueTypes.Date || valueType == filterValueTypes.Dollar || valueType == filterValueTypes.Numeric || valueType == filterValueTypes.Text)
            {
                int scg_rid = fc.fieldIndex; //corresponds to STORE_CHAR_GROUP SCG_RID, example 3=district


                sSQL += GetTab() + "s.ST_RID IN " + System.Environment.NewLine;
                sSQL += GetTab() + "( " + System.Environment.NewLine;
                tabLevel++;
                sSQL += GetTab() + "SELECT ST_RID " + System.Environment.NewLine;
                sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                sSQL += GetTab() + "STORE_CHAR_JOIN scj WITH (NOLOCK) " + System.Environment.NewLine;
                sSQL += GetTab() + "WHERE SC_RID IN " + System.Environment.NewLine;
                tabLevel++;
                sSQL += GetTab() + "( " + System.Environment.NewLine;
                sSQL += GetTab() + "SELECT SC_RID " + System.Environment.NewLine;
                sSQL += GetTab() + "FROM STORE_CHAR sc WITH (NOLOCK) " + System.Environment.NewLine;
                sSQL += GetTab() + "WHERE SCG_RID = " + scg_rid.ToString() + System.Environment.NewLine;
                if (valueType == filterValueTypes.Date)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForSmallDateComparison("sc.DATE_VALUE", fc, ref sSQL);
                    sSQL += System.Environment.NewLine;
                }
                else if (valueType == filterValueTypes.Dollar)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForDoubleComparison("sc.DOLLAR_VALUE", fc, ref sSQL);
                    sSQL += System.Environment.NewLine;
                }
                else if (valueType == filterValueTypes.Numeric)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForDoubleComparison("sc.NUMBER_VALUE", fc, ref sSQL); 
                    sSQL += System.Environment.NewLine;
                }
                else if (valueType == filterValueTypes.Text)
                {
                    sSQL += GetTab() + "AND ";
                    BuildSqlForStringComparison("sc.TEXT_VALUE", fc, ref sSQL);
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

                if (listType == filterListConstantTypes.All)
                {
                    // Begin TT#1926-MD - JSmith - Store Attributes - when select All Valuesin a Set,  the Set that is created is of ALL stores not just the stores with the Values.  This happens for Date, Dollar, Text, and Number
                    //sSQL += "1=1 --All Characteristic Values";
                    int scg_rid = fc.fieldIndex;
                    sSQL += GetTab() + "--All Characteristic Values " + System.Environment.NewLine;
                    sSQL += GetTab() + "s.ST_RID " + System.Environment.NewLine;
                    sSQL += GetTab() + "IN ";
                    sSQL += GetTab() + " ( " + System.Environment.NewLine;
                    sSQL += GetTab() + "  SELECT ST_RID " + System.Environment.NewLine;
                    sSQL += GetTab() + "  FROM " + System.Environment.NewLine;
                    sSQL += GetTab() + "  STORE_CHAR_JOIN scj WITH (NOLOCK) " + System.Environment.NewLine;
                    sSQL += GetTab() + "  JOIN STORE_CHAR sc WITH (NOLOCK) on sc.SC_RID = scj.SC_RID " + System.Environment.NewLine;
                    sSQL += GetTab() + "  WHERE SCG_RID = " + scg_rid + System.Environment.NewLine;
                    sSQL += GetTab() + " ) " + System.Environment.NewLine;
                    // End TT#1926-MD - JSmith - Store Attributes - when select All Valuesin a Set,  the Set that is created is of ALL stores not just the stores with the Values.  This happens for Date, Dollar, Text, and Number
                }
                else
                {
                    if (listType == filterListConstantTypes.None)
                    {
                        sSQL += "1=2 --No Characteristic Values";
                    }
                    else
                    {
                        DataRow[] listValues = fc.GetListValues(filterListValueTypes.StoreCharacteristicRID);
                        if (listValues.Length == 0)
                        {
                            sSQL += "1=2 --No Selected Values Saved";
                        }
                        else
                        {
                            sSQL += GetTab() + "s.ST_RID " + System.Environment.NewLine;

                            if (listOp == filterListOperatorTypes.Excludes)
                            {
                                sSQL += GetTab() + "NOT IN ";
                            }
                            else
                            {
                                sSQL += GetTab() + "IN ";
                            }


                            sSQL += " ( " + System.Environment.NewLine;
                            tabLevel++;
                            sSQL += GetTab() + "SELECT ST_RID " + System.Environment.NewLine;
                            sSQL += GetTab() + "FROM " + System.Environment.NewLine;
                            sSQL += GetTab() + "STORE_CHAR_JOIN scj WITH (NOLOCK) " + System.Environment.NewLine;
                            sSQL += GetTab() + "WHERE SC_RID IN (";
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
            }
            else
            {
                sSQL += "1=1 --Unknown characteristic value type";
            }

        }
  
        private static void BuildSqlForStoreStatus(filterCondition fc, ref string sSQL)
        {
            //Assumes store profiles have been refreshed from the service, giving the latest statuses for all profiles.
            bool firstStore = true;
            bool hasAtLeastOneStoreMatching = false;
            string storeRIDs = string.Empty;
            foreach (StoreProfile prof in StoreMgmt.StoreProfiles_GetActiveStoresList())
            {
                int storeStatus = (int)prof.Status;
                filterStoreStatusTypes filterStatus = filterStoreStatusTypes.FromTextCode(storeStatus);
                if (filterDataHelper.CompareInList(filterListValueTypes.StoreStatus, filterStatus.dbIndex, fc))
                {
                    //Add store RID to SQL
                    if (firstStore == false)
                    {
                        storeRIDs += ",";
                    }
                    storeRIDs += prof.Key.ToString();
                    firstStore = false;
                    hasAtLeastOneStoreMatching = true;
                }
            }

            if (hasAtLeastOneStoreMatching)
            {
                sSQL += "s.ST_RID IN (" + storeRIDs + ")";
            }
            else
            {
                sSQL += "1=2"; //No stores for this group
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
            // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
            string val2ForEqual = fc.valueToCompare;
            //escape val3
            val2ForEqual = val2ForEqual.Replace("'", "''");
            // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
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
                // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                //sSQL += "UPPER(" + val1 + ") = '" + val2.ToUpper() + "'";
                sSQL += "UPPER(" + val1 + ") = '" + val2ForEqual.ToUpper() + "'";
                // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqualExactly)
            {
                // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                //sSQL += val1 + " = '" + val2 + "'";
                sSQL += val1 + " = '" + val2ForEqual + "'";
                // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
            }
            else
            {
                // Begin TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
                //sSQL += val1 + " = '" + val2 + "'";
                sSQL += val1 + " = '" + val2ForEqual + "'";
                // End TT#1875-MD - JSmith - Store Attribute Set name is different format than Store characteristic Value.
            }


        }

        private static void BuildSqlForDoubleComparison(string val1, filterCondition fc, ref string sSQL)
        {
            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
            //filterNumericTypes nType = filterNumericTypes.FromIndex(fc.numericTypeIndex);
            // Begin TT#1517-MD - stodd - object reference when value was null
            //double val2 = (double)fc.valueToCompareDouble;
            double val2 = 0;
            if (fc.valueToCompareInt != null)
            {
                val2 = (double)fc.valueToCompareDouble;
            }
            // End TT#1517-MD - stodd - object reference when value was null

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
            // Begin TT#1517-MD - stodd - object reference when value was null
            int val2 = 0;
            if (fc.valueToCompareInt != null)
            {
                val2 = (int)fc.valueToCompareInt;
            }
            // End TT#1517-MD - stodd - object reference when value was null

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

        // Begin TT#1802-MD - JSmith - Some Store Field Attributes not converting as expected
        private static void BuildSqlForBooleanComparison(string val1, filterCondition fc, ref string sSQL)
        {
            string val2 = "1";
            if (fc.valueToCompare.ToUpper() == "FALSE")
            {
                val2 = "0";
            }

            sSQL += val1 + " = " + val2.ToString();
        }
        // End TT#1802-MD - JSmith - Some Store Field Attributes not converting as expected

        private static void MakeDateDynamicSQL(ref string sSQL)
        {
            sSQL += htab1 + "DECLARE @DT_NOW_WITH_TIME DATETIME = GETDATE();" + System.Environment.NewLine;
            sSQL += htab1 + "DECLARE @SDT_NOW SMALLDATETIME = CONVERT(SMALLDATETIME, CONVERT(CHAR(8), @DT_NOW_WITH_TIME, 112), 112);" + System.Environment.NewLine;
            sSQL += System.Environment.NewLine;
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

                sSQL += htab1 + "DECLARE @DT_BTWN_FROM_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysFrom.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @DT_BTWN_TO_" + GetConditionUID(fc) + " DATETIME = DATEADD(dd, " + daysTo.ToString() + ", @DT_NOW_WITH_TIME);" + System.Environment.NewLine;
            }
            else if (filterDateOperatorTypes.FromIndex(fc.operatorIndex) == filterDateOperatorTypes.Specify)
            {
                DateTime dateFrom = (DateTime)fc.valueToCompareDateFrom;
                DateTime dateTo = (DateTime)fc.valueToCompareDateTo;
                sSQL += htab1 + "DECLARE @DT_SPCFY_FROM_" + GetConditionUID(fc) + " DATETIME = CONVERT(DATETIME, '" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 120);" + System.Environment.NewLine;
                sSQL += htab1 + "DECLARE @DT_SPCFY_TO_" + GetConditionUID(fc) + " DATETIME = CONVERT(DATETIME, '" + dateTo.ToString("yyyy-MM-dd HH:mm:ss") + "', 120);" + System.Environment.NewLine;
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
                    sSQL += "1=2 --None";
                }
                else
                {
                    sSQL += "1=1 --All";
                }
            }
            else
            {
                if (listType == filterListConstantTypes.None)
                {
                    if (listOp == filterListOperatorTypes.Excludes)
                    {
                        sSQL += "1=1 --All ";
                    }
                    else
                    {
                        sSQL += "1=2 --None";
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
                    sSQL += ") " + System.Environment.NewLine;
                }
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
