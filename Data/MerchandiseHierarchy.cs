// Begin TT#2073 - JSmith - Timeout period elapsed during Alt Hierarchy Load
// Added with (rowlock) hint to all update and delete statements.  Too many rows to mark.
// End TT#2073
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Data
{
	public partial class MerchandiseHierarchyData : DataLayer
	{	
		private int _nodeIDs = 0;
		private int _colorIDs = 0;
		private int _sizeIDs = 0;
		private XmlDocument _nodeDoc;
		private XmlDocument _colorDoc;
		private XmlDocument _sizeDoc;
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        //private ChainSetPercentCriteriaData _cspcd = null;
        //private string sourceModule = "MerchandiseHierarchy.cs"; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
		public MerchandiseHierarchyData() : base()
		{

		}
       

     
		public DataTable MyHierarchies_Read(int userRID)
		{
			try
			{
                return StoredProcedures.MID_PRODUCT_HIERARCHY_READ_FROM_OWNER.Read(_dba, PH_OWNER: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //BEGIN TT#5245-Purge error number 2-BonTon
        public DataTable MyHierarchies_InUse_Read(int userRID)
        {
            try
            {
                return StoredProcedures.MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER.Read(_dba, PH_OWNER: userRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#5245-Purge error number 2-BonTon

		public DataTable PostingDate_Read()
		{
			try
			{
                return StoredProcedures.MID_POSTING_DATE_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool PostingDate_Exists(int hierarchyRID)
		{
			try
			{
                int count = StoredProcedures.MID_POSTING_DATE_READ_COUNT.ReadRecordCount(_dba, PH_RID: hierarchyRID);

				if (count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

    

        public bool PostingDate_Add(int hierarchyRID, DateTime postingDate, int postingDateYYYYDDD, int postingDateOffset, DateTime currentDate, int currentDateYYYYDDD, int currentDateOffset)
        {
            try
            {
                int rowsInserted = StoredProcedures.MID_POSTING_DATE_INSERT.Insert(_dba, 
                                                                                   PH_RID: hierarchyRID,
                                                                                   POSTINGDATE: postingDate,
                                                                                   POSTINGDATEYYYYDDD: postingDateYYYYDDD,
                                                                                   POSTINGDATEOFFSET: postingDateOffset,
                                                                                   CURRENTDATE: currentDate,
                                                                                   CURRENTDATEYYYYDDD: currentDateYYYYDDD,
                                                                                   CURRENTDATEOFFSET: currentDateOffset
                                                                                   );
                return (rowsInserted > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool PostingDate_Update(int hierarchyRID, DateTime postingDate, int postingDateYYYYDDD, int postingDateOffset, DateTime currentDate, int currentDateYYYYDDD, int currentDateOffset)
        {
            try
            {
                int rowsUpdated = StoredProcedures.MID_POSTING_DATE_UPDATE.Update(_dba, 
                                                                                  PH_RID: hierarchyRID,
                                                                                  POSTINGDATE: postingDate,
                                                                                  POSTINGDATEYYYYDDD: postingDateYYYYDDD,
                                                                                  POSTINGDATEOFFSET: postingDateOffset,
                                                                                  CURRENTDATE: currentDate,
                                                                                  CURRENTDATEYYYYDDD: currentDateYYYYDDD,
                                                                                  CURRENTDATEOFFSET: currentDateOffset
                                                                                  );
                return (rowsUpdated > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#460

		public DataTable Hierarchy_Read()
		{
			try
			{
                return StoredProcedures.MID_PRODUCT_HIERARCHY_READ.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        //Begin TT#1313-MD -jsobek -Header Filters
        public int Hierarchy_Read_Organizational_RID()
        {
            try
            {
                DataTable dt = StoredProcedures.MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID.Read(_dba);

                int orgRID = (int)dt.Rows[0]["PH_RID"];
                return orgRID;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters
		public int Hierarchy_Add(string hierarchyID, eHierarchyType hierarchyType, eHierarchyRollupOption hierarchyRollupOption, string hierarchyColor, int owner, eOTSPlanLevelType planLevelType)
		{
			try
			{             
                int hierarchyRID = StoredProcedures.SP_MID_HIER_INSERT.InsertAndReturnRID(_dba, 
                                                                                           PH_ID: hierarchyID,
				                                                                           PH_TYPE: (int)hierarchyType,
				                                                                           PH_COLOR: hierarchyColor,
				                                                                           PH_OWNER: owner,
				                                                                           OTS_PLANLEVEL_TYPE: (int)planLevelType,
				                                                                           HISTORY_ROLL_OPTION: (int)hierarchyRollupOption
				                                                                           );

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(owner, (int)eProfileType.Hierarchy, hierarchyRID, owner);
                }

                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.AddUserItem(owner, (int)eSharedDataType.Hierarchy, hierarchyRID, owner);
                //    sa.AddUserItem(owner, (int)eProfileType.Hierarchy, hierarchyRID, owner);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login


				return hierarchyRID;

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Update(int hierarchyRID, string hierarchyID, int aOwner, eHierarchyType hierarchyType, eHierarchyRollupOption hierarchyRollupOption, string hierarchyColor, eOTSPlanLevelType planLevelType)
		{
			try
			{
                StoredProcedures.MID_PRODUCT_HIERARCHY_UPDATE.Update(_dba, 
                                                                     PH_RID: hierarchyRID,
                                                                     PH_ID: hierarchyID,
                                                                     PH_TYPE: (int)hierarchyType,
                                                                     PH_COLOR: hierarchyColor,
                                                                     PH_OWNER: aOwner,
                                                                     OTS_PLANLEVEL_TYPE: (int)planLevelType,
                                                                     HISTORY_ROLL_OPTION: (int)hierarchyRollupOption
                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void Hierarchy_Delete(int hierarchyRID)
		{
			try
			{
                StoredProcedures.MID_PRODUCT_HIERARCHY_DELETE.Delete(_dba, PH_RID: hierarchyRID);
              

                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.Hierarchy, hierarchyRID);
                //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.Hierarchy, hierarchyRID);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable HierarchyLevels_Read(int hierarchyRID)
		{
			try
			{
                return StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_READ.Read(_dba, PH_RID: hierarchyRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
        public DataTable HierarchyLevels_Read_HeaderPurge(int aHierarchyRID, int aLevel)
        {
            try
            {
                return StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ.Read(_dba, 
                                                                                            PH_RID: aHierarchyRID,
                                                                                            PHL_SEQUENCE: aLevel
                                                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

       

        public void Hierarchy_Level_Add(int hierarchyRID, int level, string description,
            string levelColor, eLevelLengthType levelLengthType,
            int requiredSize, int sizeRangeFrom, int sizeRangeTo,
            eHierarchyLevelType hierarchyLevelType,
            eOTSPlanLevelType OTSPlanLevelType,
            eHierarchyDisplayOptions displayOption,
            eHierarchyIDFormat IDFormat,
            ePurgeTimeframe purgeDailyHistoryTimeframe, int purgeDailyHistory,
            ePurgeTimeframe purgeWeeklyHistoryTimeframe, int purgeWeeklyHistory,
            ePurgeTimeframe purgePlansTimeframe, int purgePlans)
        {
            try
            {
                int? purgeDailyHistory_Nullable = null;
                if (purgeDailyHistory != Include.Undefined) purgeDailyHistory_Nullable = purgeDailyHistory;

                int? purgeWeeklyHistory_Nullable = null;
                if (purgeWeeklyHistory != Include.Undefined) purgeWeeklyHistory_Nullable = purgeWeeklyHistory;

                int? purgePlans_Nullable = null;
                if (purgePlans != Include.Undefined) purgePlans_Nullable = purgePlans;

                StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_INSERT.Insert(_dba, 
                                                                            PH_RID: hierarchyRID,
                                                                            PHL_SEQUENCE: level,
                                                                            PHL_ID: description,
                                                                            PHL_COLOR: levelColor,
                                                                            PHL_TYPE: (int)hierarchyLevelType,
                                                                            LENGTH_TYPE: (int)levelLengthType,
                                                                            REQUIRED_SIZE: requiredSize,
                                                                            SIZE_RANGE_FROM: sizeRangeFrom,
                                                                            SIZE_RANGE_TO: sizeRangeTo,
                                                                            OTS_PLANLEVEL_TYPE: (int)OTSPlanLevelType,
                                                                            PHL_DISPLAY_OPTION_ID: (int)displayOption,
                                                                            PHL_ID_FORMAT: (int)IDFormat,
                                                                            PURGE_DAILY_HISTORY_TIMEFRAME: (int)purgeDailyHistoryTimeframe,
                                                                            PURGE_DAILY_HISTORY: purgeDailyHistory_Nullable,
                                                                            PURGE_WEEKLY_HISTORY_TIMEFRAME: (int)purgeWeeklyHistoryTimeframe,
                                                                            PURGE_WEEKLY_HISTORY: purgeWeeklyHistory_Nullable,
                                                                            PURGE_PLANS_TIMEFRAME: (int)purgePlansTimeframe,
                                                                            PURGE_PLANS: purgePlans_Nullable
                                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Hierarchy_Level_Add_HeaderPurge(int hierarchyRID, int level, eHeaderType aHeaderType, ePurgeTimeframe purgeHeadersTimeframe, int purgeHeaders)
        {
            try
            {
                int? purgeHeaders_Nullable = null;
                if (purgeHeaders != Include.Undefined) purgeHeaders_Nullable = purgeHeaders;

                StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT.Insert(_dba, 
                                                                                         PH_RID: hierarchyRID,
                                                                                         PHL_SEQUENCE: level,
                                                                                         HEADER_TYPE: (int)aHeaderType,
                                                                                         PURGE_HEADERS_TIMEFRAME: (int)purgeHeadersTimeframe,
                                                                                         PURGE_HEADERS: purgeHeaders_Nullable
                                                                                         );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
 
        public void Hierarchy_Level_Update(int hierarchyRID, int level, string description,
                                            string levelColor, eLevelLengthType levelLengthType,
                                            int requiredSize, int sizeRangeFrom, int sizeRangeTo,
                                            eHierarchyLevelType hierarchyLevelType,
                                            eOTSPlanLevelType OTSPlanLevelType,
                                            eHierarchyDisplayOptions displayOption,
                                            eHierarchyIDFormat IDFormat,
                                            ePurgeTimeframe purgeDailyHistoryTimeframe, int purgeDailyHistory,
                                            ePurgeTimeframe purgeWeeklyHistoryTimeframe, int purgeWeeklyHistory,
                                            ePurgeTimeframe purgePlansTimeframe, int purgePlans)
        {
            try
            {
                int? purgeDailyHistory_Nullable = null;
                if (purgeDailyHistory != Include.Undefined) purgeDailyHistory_Nullable = purgeDailyHistory;

                int? purgeWeeklyHistory_Nullable = null;
                if (purgeWeeklyHistory != Include.Undefined) purgeWeeklyHistory_Nullable = purgeWeeklyHistory;

                int? purgePlans_Nullable = null;
                if (purgePlans != Include.Undefined) purgePlans_Nullable = purgePlans;

                StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_UPDATE.Update(_dba, 
                                                                            PH_RID: hierarchyRID,
                                                                            PHL_SEQUENCE: level,
                                                                            PHL_ID: description,
                                                                            PHL_COLOR: levelColor,
                                                                            PHL_TYPE: (int)hierarchyLevelType,
                                                                            LENGTH_TYPE: (int)levelLengthType,
                                                                            REQUIRED_SIZE: requiredSize,
                                                                            SIZE_RANGE_FROM: sizeRangeFrom,
                                                                            SIZE_RANGE_TO: sizeRangeTo,
                                                                            OTS_PLANLEVEL_TYPE: (int)OTSPlanLevelType,
                                                                            PHL_DISPLAY_OPTION_ID: (int)displayOption,
                                                                            PHL_ID_FORMAT: (int)IDFormat,
                                                                            PURGE_DAILY_HISTORY_TIMEFRAME: (int)purgeDailyHistoryTimeframe,
                                                                            PURGE_DAILY_HISTORY: purgeDailyHistory_Nullable,
                                                                            PURGE_WEEKLY_HISTORY_TIMEFRAME: (int)purgeWeeklyHistoryTimeframe,
                                                                            PURGE_WEEKLY_HISTORY: purgeWeeklyHistory_Nullable,
                                                                            PURGE_PLANS_TIMEFRAME: (int)purgePlansTimeframe,
                                                                            PURGE_PLANS: purgePlans_Nullable
                                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Hierarchy_Level_DeleteAll(int hierarchyRID)
        {
            try
            {
                StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_DELETE.Delete(_dba, PH_RID: hierarchyRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Hierarchy_Level_Delete(int aHierarchyRID, int aLevel)
        {
            try
            {
                StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ.Delete(_dba, 
                                                                                     PH_RID: aHierarchyRID,
                                                                                     PHL_SEQUENCE: aLevel
                                                                                     );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Hierarchy_Level_Delete_HeaderPurge(int hierarchyRID, int level)
        {
            try
            {
                StoredProcedures.MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE.Delete(_dba, 
                                                                                         PH_RID: hierarchyRID,
                                                                                         PHL_SEQUENCE: level
                                                                                         );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type

		public eHierarchyLevelType Hierarchy_Node_Type(int aNodeRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_HIERARCHY_READ_TYPE.Read(_dba, HN_RID: aNodeRID);

				if (dt.Rows.Count == 0)
				{
					return eHierarchyLevelType.Undefined;
				}
				DataRow dr = dt.Rows[0];

				return (eHierarchyLevelType)(Convert.ToInt32(dr["HN_TYPE"], CultureInfo.CurrentUICulture));
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_Node_Read()
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_READ_ALL_BASE.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable Hierarchy_Node_Read(Dictionary<int, int> includedHierarchies)
		{
			try
			{
                DataTable dtIncludedHierarchyList = new DataTable();
                dtIncludedHierarchyList.Columns.Add("HOME_PH_RID", typeof(int));
                foreach (KeyValuePair<int, int> hierarchy in includedHierarchies)
                {
                    int hierarchyRID = (int)hierarchy.Key;
                    //ensure hierarchyRID are distinct, and only added to the datatable one time
                    if (dtIncludedHierarchyList.Select("HOME_PH_RID=" + hierarchyRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtIncludedHierarchyList.NewRow();
                        dr["HOME_PH_RID"] = hierarchyRID;
                        dtIncludedHierarchyList.Rows.Add(dr);
                    }
                }

                return StoredProcedures.MID_HIERARCHY_READ_BASE_FROM_HOME_PH.Read(_dba, HOME_PH_RID_LIST: dtIncludedHierarchyList);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_Node_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_GET_INFO_FROM_BASE_NODE.Read(_dba, HN_RID: nodeRID);	
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_NodeRID_Read(string nodeID)
		{
			try
			{
                return StoredProcedures.MID_BASE_NODE_READ_FROM_ID.Read(_dba, BN_ID: nodeID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public DataTable Hierarchy_NodeRID_Read_From_Base_Search_String(string searchString)
        {
            try
            {
                return StoredProcedures.MID_BASE_NODE_SEARCH_READ.Read(_dba, BN_SEARCH_STRING: searchString);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }       
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

		public DataTable Hierarchy_ChildNodes_Read(int hierarchyRID, int nodeRID)
		{
			try
			{   
                return StoredProcedures.MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE.Read(_dba, 
                                                                                         hierarchyRID: hierarchyRID,
                                                                                         HN_RID: nodeRID
                                                                                         );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_Node_PurgeCriteria_Read(int nodeRID)
		{
			try
			{  
                return StoredProcedures.MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_NodeRID_Read_For_Purge(int aHierarchyRID)
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_READ_FROM_HOME_PH.Read(_dba, HOME_PH_RID: aHierarchyRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Node_Update(int nodeRID, int homeHierarchyRID, int homeHierarchyLevel, 
			eHierarchyLevelType levelType, bool OTSPlanLevelTypeIsOverridden, 
			eOTSPlanLevelType OTSPlanLevelTypeOverride, bool useBasicReplenishment,
			bool OTSPlanLevelIsOverridden, ePlanLevelSelectType PlanLevelSelectType, ePlanLevelLevelType OTSPlanLevelLevelType, 
			int OTSPlanLevelRID, int OTSPlanLevelSequence,
			int OTSPlanLevelAnchorNode, eMaskField OTSPlanLevelMaskField, 
            string OTSPlanLevelMask, bool virtualInd,
            ePurpose aPurpose)
		{
			try
			{         
                int OTS_PLANLEVEL_TYPE;
                if (OTSPlanLevelTypeIsOverridden)
                {
                    OTS_PLANLEVEL_TYPE = (int)OTSPlanLevelTypeOverride;
                }
                else
                {
                    OTS_PLANLEVEL_TYPE = (int)eOTSPlanLevelType.Undefined;
                }
                int? OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = (int)PlanLevelSelectType;
                }
                int? OTS_FORECAST_LEVEL_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_TYPE_Nullable = (int)OTSPlanLevelLevelType;
                }
                int? OTS_FORECAST_LEVEL_PH_RID_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelRID != Include.NoRID)
                {
                    OTS_FORECAST_LEVEL_PH_RID_Nullable = (int)OTSPlanLevelRID;
                }
                int? OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelSequence != Include.Undefined && OTSPlanLevelSequence != 0)
                {
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = (int)OTSPlanLevelSequence;
                }
                int? OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelAnchorNode != Include.Undefined)
                {
                    OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = (int)OTSPlanLevelAnchorNode;
                }
                int? OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMaskField != eMaskField.Undefined)
                {
                    OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = (int)OTSPlanLevelMaskField;
                }
                string OTS_FORECAST_LEVEL_MASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMask != null && OTSPlanLevelMask.Trim().Length > 0)
                {
                    OTS_FORECAST_LEVEL_MASK = OTSPlanLevelMask;
                }

                StoredProcedures.MID_HIERARCHY_UPDATE.Update(_dba, 
                                                             HN_RID: nodeRID,
                                                             HOME_PH_RID: homeHierarchyRID,
                                                             HOME_LEVEL: homeHierarchyLevel,
                                                             HN_TYPE: (int)levelType,
                                                             OTS_PLANLEVEL_TYPE: OTS_PLANLEVEL_TYPE,
                                                             USE_BASIC_REPLENISHMENT: Include.ConvertBoolToChar(useBasicReplenishment),
                                                             OTS_FORECAST_LEVEL_SELECT_TYPE: OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable,
                                                             OTS_FORECAST_LEVEL_TYPE: OTS_FORECAST_LEVEL_TYPE_Nullable,
                                                             OTS_FORECAST_LEVEL_PH_RID: OTS_FORECAST_LEVEL_PH_RID_Nullable,
                                                             OTS_FORECAST_LEVEL_PHL_SEQUENCE: OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable,
                                                             OTS_FORECAST_LEVEL_ANCHOR_NODE: OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable,
                                                             OTS_FORECAST_LEVEL_MASK_FIELD: OTS_FORECAST_LEVEL_MASK_FIELD_Nullable,
                                                             OTS_FORECAST_LEVEL_MASK: OTS_FORECAST_LEVEL_MASK,
                                                             VIRTUAL_IND: Include.ConvertBoolToChar(virtualInd),
                                                             PURPOSE: (int)aPurpose
                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
				
		public void Hierarchy_Node_Delete(int nodeRID)
		{
			try
			{
                StoredProcedures.SP_MID_HIERNODE_DELETE.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Node_Replace(int aNodeRID, int aReplaceWithNodeRID)
		{
			try
			{
                StoredProcedures.SP_MID_HIERNODE_REPLACE.Update(_dba, 
                                                                HN_RID: aNodeRID,
                                                                REPLACE_HN_RID: aReplaceWithNodeRID
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public int Hierarchy_BasicNode_Add(int hierarchyRID, int parentRID, int homeHierarchyRID, int homeHierarchyLevel,
            eHierarchyLevelType levelType, bool OTSPlanLevelTypeIsOverridden,
            eOTSPlanLevelType OTSPlanLevelTypeOverride, bool useBasicReplenishment,
            bool OTSPlanLevelIsOverridden, ePlanLevelSelectType PlanLevelSelectType, ePlanLevelLevelType OTSPlanLevelLevelType,
            int OTSPlanLevelRID, int OTSPlanLevelSequence,
            int OTSPlanLevelAnchorNode, eMaskField OTSPlanLevelMaskField, string OTSPlanLevelMask, bool virtualInd,
            ePurpose aPurpose, string nodeID, string nodeName, string description,
            bool productTypeIsOverridden, eProductType productType)
        {
            try
            {
                int OTS_PLANLEVEL_TYPE;
                if (OTSPlanLevelTypeIsOverridden)
                {
                    OTS_PLANLEVEL_TYPE = (int)OTSPlanLevelTypeOverride;
                }
                else
                {
                    OTS_PLANLEVEL_TYPE = (int)eOTSPlanLevelType.Undefined;
                }
                int? OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = (int)PlanLevelSelectType;
                }
                int? OTS_FORECAST_LEVEL_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_TYPE_Nullable = (int)OTSPlanLevelLevelType;
                }
                int? OTS_FORECAST_LEVEL_PH_RID_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelRID != Include.NoRID)
                {
                    OTS_FORECAST_LEVEL_PH_RID_Nullable = (int)OTSPlanLevelRID;
                }
                int? OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelSequence != Include.Undefined && OTSPlanLevelSequence != 0)
                {
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = (int)OTSPlanLevelSequence;
                }
                int? OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelAnchorNode != Include.Undefined)
                {
                    OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = (int)OTSPlanLevelAnchorNode;
                }
                int? OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMaskField != eMaskField.Undefined)
                {
                    OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = (int)OTSPlanLevelMaskField;
                }
                string OTS_FORECAST_LEVEL_MASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMask != null && OTSPlanLevelMask.Trim().Length > 0)
                {
                    OTS_FORECAST_LEVEL_MASK = OTSPlanLevelMask;
                }
                int PRODUCT_TYPE;
                if (productTypeIsOverridden)
                {
                    PRODUCT_TYPE = (int)productType;
                }
                else
                {
                    PRODUCT_TYPE = (int)eProductType.Undefined;
                }

                return StoredProcedures.SP_MID_HIERNODE_BASE_INSERT.InsertAndReturnRID(_dba, 
                                                                                        PH_RID: hierarchyRID,
                                                                                        PARENT_RID: parentRID,
                                                                                        HOME_PH_RID: homeHierarchyRID,
                                                                                        HOME_LEVEL: homeHierarchyLevel,
                                                                                        HN_TYPE: (int)levelType,
                                                                                        OTS_PLANLEVEL_TYPE: OTS_PLANLEVEL_TYPE,
                                                                                        USE_BASIC_REPLENISHMENT: Include.ConvertBoolToChar(useBasicReplenishment),
                                                                                        OTS_FORECAST_LEVEL_SELECT_TYPE: OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable,
                                                                                        OTS_FORECAST_LEVEL_TYPE: OTS_FORECAST_LEVEL_TYPE_Nullable,
                                                                                        OTS_FORECAST_LEVEL_PH_RID: OTS_FORECAST_LEVEL_PH_RID_Nullable,
                                                                                        OTS_FORECAST_LEVEL_PHL_SEQUENCE: OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable,
                                                                                        OTS_FORECAST_LEVEL_ANCHOR_NODE: OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable,
                                                                                        OTS_FORECAST_LEVEL_MASK_FIELD: OTS_FORECAST_LEVEL_MASK_FIELD_Nullable,
                                                                                        OTS_FORECAST_LEVEL_MASK: OTS_FORECAST_LEVEL_MASK,
                                                                                        VIRTUAL_IND: Include.ConvertBoolToChar(virtualInd),
                                                                                        PURPOSE: (int)aPurpose,
                                                                                        NODE_ID: nodeID,
                                                                                        NODE_NAME: nodeName,
                                                                                        NODE_DESCRIPTION: description,
                                                                                        PRODUCT_TYPE: PRODUCT_TYPE
                                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1453

		public void Hierarchy_BasicNode_Update(int nodeRID, string nodeID, string nodeName, string description, 
			bool productTypeIsOverridden, eProductType productType)
		{
			try
			{
                int newProductType;
                if (productTypeIsOverridden)
                {
                    newProductType = (int)productType;
                }
                else
                {
                    newProductType = (int)eProductType.Undefined;
                }

                StoredProcedures.MID_BASE_NODE_UPDATE.Update(_dba, 
                                                             HN_RID: nodeRID,
                                                             BN_ID: nodeID,
                                                             BN_NAME: nodeName,
                                                             BN_DESCRIPTION: description,
                                                             BN_PRODUCT_TYPE: newProductType
                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_BasicNode_Delete(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_BASE_NODE_DELETE.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#634 - JSmith - Color rename
        public DataTable GetStylesForColor(int aColorCodeRID)
        {
            try
            {
                return StoredProcedures.MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE.Read(_dba, COLOR_CODE_RID: aColorCodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#634

		public DataTable Hierarchy_ColorNode_Read(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_ColorNode_Read(int hierarchyRID, int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE.Read(_dba, 
                                                                                   hierarchyRID: hierarchyRID,
                                                                                   HN_RID: nodeRID
                                                                                   );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


        public int Hierarchy_ColorNode_Add(int hierarchyRID, int parentRID, int homeHierarchyRID, int homeHierarchyLevel,
            eHierarchyLevelType levelType, bool OTSPlanLevelTypeIsOverridden,
            eOTSPlanLevelType OTSPlanLevelTypeOverride, bool useBasicReplenishment,
            bool OTSPlanLevelIsOverridden, ePlanLevelSelectType PlanLevelSelectType, ePlanLevelLevelType OTSPlanLevelLevelType,
            int OTSPlanLevelRID, int OTSPlanLevelSequence,
            int OTSPlanLevelAnchorNode, eMaskField OTSPlanLevelMaskField, string OTSPlanLevelMask, bool virtualInd,
            ePurpose aPurpose, int colorCodeRID, string colorDescription,
            string aStyleNodeID)
        {
            try
            {
                int OTS_PLANLEVEL_TYPE;
                if (OTSPlanLevelTypeIsOverridden)
                {
                    OTS_PLANLEVEL_TYPE = (int)OTSPlanLevelTypeOverride;
                }
                else
                {
                    OTS_PLANLEVEL_TYPE = (int)eOTSPlanLevelType.Undefined;
                }
                int? OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = (int)PlanLevelSelectType;
                }
                int? OTS_FORECAST_LEVEL_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_TYPE_Nullable = (int)OTSPlanLevelLevelType;
                }
                int? OTS_FORECAST_LEVEL_PH_RID_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelRID != Include.NoRID)
                {
                    OTS_FORECAST_LEVEL_PH_RID_Nullable = (int)OTSPlanLevelRID;
                }
                int? OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelSequence != Include.Undefined && OTSPlanLevelSequence != 0)
                {
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = (int)OTSPlanLevelSequence;
                }
                int? OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelAnchorNode != Include.Undefined)
                {
                    OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = (int)OTSPlanLevelAnchorNode;
                }
                int? OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMaskField != eMaskField.Undefined)
                {
                    OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = (int)OTSPlanLevelMaskField;
                }
                string OTS_FORECAST_LEVEL_MASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMask != null && OTSPlanLevelMask.Trim().Length > 0)
                {
                    OTS_FORECAST_LEVEL_MASK = OTSPlanLevelMask;
                }
                
                return StoredProcedures.SP_MID_HIERNODE_COLOR_INSERT.InsertAndReturnRID(_dba, 
                                                                                         PH_RID: hierarchyRID,
                                                                                         PARENT_RID: parentRID,
                                                                                         HOME_PH_RID: homeHierarchyRID,
                                                                                         HOME_LEVEL: homeHierarchyLevel,
                                                                                         HN_TYPE: (int)levelType,
                                                                                         OTS_PLANLEVEL_TYPE: OTS_PLANLEVEL_TYPE,
                                                                                         USE_BASIC_REPLENISHMENT: Include.ConvertBoolToChar(useBasicReplenishment),
                                                                                         OTS_FORECAST_LEVEL_SELECT_TYPE: OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable,
                                                                                         OTS_FORECAST_LEVEL_TYPE: OTS_FORECAST_LEVEL_TYPE_Nullable,
                                                                                         OTS_FORECAST_LEVEL_PH_RID: OTS_FORECAST_LEVEL_PH_RID_Nullable,
                                                                                         OTS_FORECAST_LEVEL_PHL_SEQUENCE: OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable,
                                                                                         OTS_FORECAST_LEVEL_ANCHOR_NODE: OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable,
                                                                                         OTS_FORECAST_LEVEL_MASK_FIELD: OTS_FORECAST_LEVEL_MASK_FIELD_Nullable,
                                                                                         OTS_FORECAST_LEVEL_MASK: OTS_FORECAST_LEVEL_MASK,
                                                                                         VIRTUAL_IND: Include.ConvertBoolToChar(virtualInd),
                                                                                         PURPOSE: (int)aPurpose,
                                                                                         COLOR_CODE_RID: colorCodeRID,
                                                                                         COLOR_DESCRIPTION: colorDescription,
                                                                                         STYLE_NODE_ID: aStyleNodeID
                                                                                         );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
		}

        //  this needs to be updated to allow for the db when it is not populated
        public void Hierarchy_Properties_Update(int nodeRID, int applyHNRIDFrom)
        {
            try
            {
                StoredProcedures.MID_HIER_NODE_PROPERTIES_DELETE.Delete(_dba, HN_RID: nodeRID);
                if (applyHNRIDFrom != Include.NoRID)
                {
                    StoredProcedures.MID_HIER_NODE_PROPERTIES_INSERT.Insert(_dba, 
                                                                            HN_RID: nodeRID,
                                                                            APPLY_HN_RID_FROM: applyHNRIDFrom
                                                                            );
                }

            }
            catch
            {
                throw;
            }
        }


		public void Hierarchy_ColorNode_Update(int nodeRID, int colorCodeRID, string colorDescription, string aStyleNodeID)
		{
			try
			{
                StoredProcedures.MID_COLOR_NODE_UPDATE.Update(_dba, 
                                                              HN_RID: colorCodeRID,
                                                              COLOR_CODE_RID: colorCodeRID,
                                                              COLOR_DESCRIPTION: colorDescription
                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_ColorNode_Delete(int nodeRID, int colorCodeRID)
		{
			try
			{
                StoredProcedures.MID_COLOR_NODE_DELETE.Delete(_dba, 
                                                              HN_RID: colorCodeRID,
                                                              COLOR_CODE_RID: colorCodeRID
                                                              );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Hierarchy_ColorNodeRID_For_Style(int aHierarchyRID, int aStyleNodeRID, string aColorID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_COLOR_NODE_READ_FOR_STYLE.Read(_dba, 
                                                                    PH_RID: aHierarchyRID,
                                                                    STYLE_HN_RID: aStyleNodeRID,
                                                                    COLOR_CODE_ID: aColorID
                                                                    );

				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Include.NoRID;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#2763 - JSmith - Hierarchy Color descriptions not updating
        public int Hierarchy_ColorNodeRID_For_Style(int aHierarchyRID, int aStyleNodeRID, string aColorID,
            out string aColorDescription)
		{
			try
			{
                aColorDescription = null;
                DataTable dt = StoredProcedures.MID_COLOR_NODE_READ_FOR_STYLE.Read(_dba, 
                                                                                   PH_RID: aHierarchyRID,
                                                                                   STYLE_HN_RID: aStyleNodeRID,
                                                                                   COLOR_CODE_ID: aColorID
                                                                                   );

				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
                    aColorDescription = Convert.ToString(dr["COLOR_DESCRIPTION"], CultureInfo.CurrentUICulture);
					return Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Include.NoRID;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // End TT#2763 - JSmith - Hierarchy Color descriptions not updating

		public DataTable Hierarchy_SizeNode_Read(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE.Read(_dba, HN_RID: aNodeRID);		
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_SizeNode_Read(int hierarchyRID, int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE.Read(_dba, 
                                                                                         hierarchyRID: hierarchyRID,
                                                                                         HN_RID: nodeRID
                                                                                         );			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public int Hierarchy_SizeNode_Add(int hierarchyRID, int parentRID, int homeHierarchyRID, int homeHierarchyLevel,
            eHierarchyLevelType levelType, bool OTSPlanLevelTypeIsOverridden,
            eOTSPlanLevelType OTSPlanLevelTypeOverride, bool useBasicReplenishment,
            bool OTSPlanLevelIsOverridden, ePlanLevelSelectType PlanLevelSelectType, ePlanLevelLevelType OTSPlanLevelLevelType,
            int OTSPlanLevelRID, int OTSPlanLevelSequence,
            int OTSPlanLevelAnchorNode, eMaskField OTSPlanLevelMaskField, string OTSPlanLevelMask, bool virtualInd,
            ePurpose aPurpose, int sizeCodeRID,
            string aStyleNodeID, string aColorNodeID)
        {
            try
            {
                int OTS_PLANLEVEL_TYPE;
                if (OTSPlanLevelTypeIsOverridden)
                {
                    OTS_PLANLEVEL_TYPE = (int)OTSPlanLevelTypeOverride;
                }
                else
                {
                    OTS_PLANLEVEL_TYPE = (int)eOTSPlanLevelType.Undefined;
                }
                int? OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable = (int)PlanLevelSelectType;
                }
                int? OTS_FORECAST_LEVEL_TYPE_Nullable = null;
                if (OTSPlanLevelIsOverridden)
                {
                    OTS_FORECAST_LEVEL_TYPE_Nullable = (int)OTSPlanLevelLevelType;
                }
                int? OTS_FORECAST_LEVEL_PH_RID_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelRID != Include.NoRID)
                {
                    OTS_FORECAST_LEVEL_PH_RID_Nullable = (int)OTSPlanLevelRID;
                }
                int? OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelSequence != Include.Undefined && OTSPlanLevelSequence != 0)
                {
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable = (int)OTSPlanLevelSequence;
                }
                int? OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelAnchorNode != Include.Undefined)
                {
                    OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable = (int)OTSPlanLevelAnchorNode;
                }
                int? OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = null;
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMaskField != eMaskField.Undefined)
                {
                    OTS_FORECAST_LEVEL_MASK_FIELD_Nullable = (int)OTSPlanLevelMaskField;
                }
                string OTS_FORECAST_LEVEL_MASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (OTSPlanLevelIsOverridden && OTSPlanLevelMask != null && OTSPlanLevelMask.Trim().Length > 0)
                {
                    OTS_FORECAST_LEVEL_MASK = OTSPlanLevelMask;
                }


                return StoredProcedures.SP_MID_HIERNODE_SIZE_INSERT.InsertAndReturnRID(_dba, 
                                                                                       PH_RID: hierarchyRID,
                                                                                       PARENT_RID: parentRID,
                                                                                       HOME_PH_RID: homeHierarchyRID,
                                                                                       HOME_LEVEL: homeHierarchyLevel,
                                                                                       HN_TYPE: (int)levelType,
                                                                                       OTS_PLANLEVEL_TYPE: OTS_PLANLEVEL_TYPE,
                                                                                       USE_BASIC_REPLENISHMENT: Include.ConvertBoolToChar(useBasicReplenishment),
                                                                                       OTS_FORECAST_LEVEL_SELECT_TYPE: OTS_FORECAST_LEVEL_SELECT_TYPE_Nullable,
                                                                                       OTS_FORECAST_LEVEL_TYPE: OTS_FORECAST_LEVEL_TYPE_Nullable,
                                                                                       OTS_FORECAST_LEVEL_PH_RID: OTS_FORECAST_LEVEL_PH_RID_Nullable,
                                                                                       OTS_FORECAST_LEVEL_PHL_SEQUENCE: OTS_FORECAST_LEVEL_PHL_SEQUENCE_Nullable,
                                                                                       OTS_FORECAST_LEVEL_ANCHOR_NODE: OTS_FORECAST_LEVEL_ANCHOR_NODE_Nullable,
                                                                                       OTS_FORECAST_LEVEL_MASK_FIELD: OTS_FORECAST_LEVEL_MASK_FIELD_Nullable,
                                                                                       OTS_FORECAST_LEVEL_MASK: OTS_FORECAST_LEVEL_MASK,
                                                                                       VIRTUAL_IND: Include.ConvertBoolToChar(virtualInd),
                                                                                       PURPOSE: (int)aPurpose,
                                                                                       SIZE_CODE_RID: sizeCodeRID,
                                                                                       STYLE_NODE_ID: aStyleNodeID,
                                                                                       COLOR_NODE_ID: aColorNodeID
                                                                                       );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1453

		public void Hierarchy_SizeNode_Delete(int nodeRID, int sizeCodeRID)
		{
			try
			{
                StoredProcedures.MID_SIZE_NODE_DELETE.Delete(_dba, 
                                                             HN_RID: nodeRID,
                                                             SIZE_CODE_RID: sizeCodeRID
                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Hierarchy_SizeNodeRID_For_Color(int aHierarchyRID, int aColorNodeRID, string aSizeID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SIZE_NODE_READ_RID_FOR_COLOR.Read(_dba, 
                                                                       PH_RID: aHierarchyRID,
                                                                       COLOR_HN_RID: aColorNodeRID,
                                                                       SIZE_CODE_ID: aSizeID
                                                                       );

				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Include.NoRID;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_Join_Read(int hierarchyRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_HIER_NODE_JOIN_READ.Read(_dba, PH_RID: hierarchyRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_Join_Read(int aHierarchyRID, int aParentRID, int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT.Read(_dba, 
                                                                                    PH_RID: aHierarchyRID,
                                                                                    PARENT_HN_RID: aParentRID,
                                                                                    HN_RID: aNodeRID
                                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Join_Add(int hierarchyRID, int parentRID, int nodeRID)
		{
			try
			{
                StoredProcedures.MID_HIER_NODE_JOIN_INSERT.Insert(_dba, 
                                                                  PH_RID: hierarchyRID,
                                                                  PARENT_HN_RID: parentRID,
                                                                  HN_RID: nodeRID
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Join_Update(int nodeRID, int oldHierarchy_RID,  int oldParent_RID, int newHierarchy_RID,  int newParent_RID)
		{
			try
			{
                StoredProcedures.SP_MID_HIERJOIN_UPDATE.Update(_dba, 
                                                               HN_RID: nodeRID,
                                                               OLD_HIERARCHY_RID: oldHierarchy_RID,
                                                               OLD_PARENT_RID: oldParent_RID,
                                                               NEW_HIERARCHY_RID: newHierarchy_RID,
                                                               NEW_PARENT_RID: newParent_RID
                                                               );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void Hierarchy_Join_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_HIER_NODE_JOIN_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Join_Delete(int hierarchyRID, int parentRID, int nodeRID)
		{
			try
			{
                // Begin TT#1339-MD - stodd - Cannot Delete Hiearchy Shortcuts
                //StoredProcedures.MID_HIER_NODE_JOIN_DELETE.Delete(_dba, 
                //                                                  PH_RID: nodeRID,
                //                                                  PARENT_HN_RID: nodeRID,
                //                                                  HN_RID: nodeRID
                //                                                  );

                StoredProcedures.MID_HIER_NODE_JOIN_DELETE.Delete(_dba,
                                                  PH_RID: hierarchyRID,
                                                  PARENT_HN_RID: parentRID,
                                                  HN_RID: nodeRID
                                                  );
                // End TT#1339-MD - stodd - Cannot Delete Hiearchy Shortcuts
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_CharGroup_Read()
		{
			try
			{
                return StoredProcedures.MID_HIER_CHAR_GROUP_READ_ALL.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
        //Begin TT#1388-MD -jsobek -Product Filters
        public DataTable Hierarchy_CharGetValuesForGroup(int aHCG_RID)
        {
            try
            {
                return StoredProcedures.MID_HIER_CHAR_READ_FOR_GROUP.Read(_dba, HCG_RID: aHCG_RID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1388-MD -jsobek -Product Filters

		public int Hierarchy_CharGroup_Add(string description)
		{
			try
			{
                return StoredProcedures.SP_MID_HIERCHARGROUP_INSERT.InsertAndReturnRID(_dba, HCG_ID: description);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_CharGroup_Update(int hierarchyCharGroup_RID, string description)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_GROUP_UPDATE.Update(_dba, 
                                                                   HCG_RID: hierarchyCharGroup_RID,
                                                                   HCG_ID: description
                                                                   );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void Hierarchy_CharGroup_Delete(int hierarchyCharGroup_RID)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_GROUP_DELETE.Delete(_dba, HCG_RID: hierarchyCharGroup_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hierarchy_Char_Read()
		{
			try
			{
                return StoredProcedures.MID_HIER_CHAR_GROUP_VALUES_READ_ALL.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Hierarchy_Char_Add(int hierarchyCharGroup_RID, string description)
		{
			try
			{
                return StoredProcedures.SP_MID_HIERCHAR_INSERT.InsertAndReturnRID(_dba, 
                                                                                  HCG_RID: hierarchyCharGroup_RID,
                                                                                  HC_ID: description
                                                                                 );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hierarchy_Char_Update(int hierarchyChar_RID, string description)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_UPDATE.Update(_dba, 
                                                             HC_RID: hierarchyChar_RID,
                                                             HC_ID: description
                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void Hierarchy_Char_Delete(int hierarchyChar_RID)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_DELETE.Delete(_dba, HC_RID: hierarchyChar_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
        public DataTable Hier_Char_Join_Read_All()
        {
            try
            {
                return StoredProcedures.MID_HIER_CHAR_JOIN_READ_ALL.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#3523 - JSmith - Performance of Anthro morning processing jobs

        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
		public DataTable Hier_Char_Join_Read()
		{
			try
			{
				string SQLCommand = "SELECT * FROM HIER_CHAR_JOIN hcj with (nolock)";

				return _dba.ExecuteSQLQuery(SQLCommand, "HIER_CHAR_JOIN");
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
        // End TT#3523 - JSmith - Performance of Anthro morning processing jobs

		public DataTable Hier_Char_Join_Read(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_HIER_CHAR_JOIN_READ.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable Hier_Char_Join_ReadByValue(int aValueRID)
		{
			try
			{
                return StoredProcedures.MID_HIER_CHAR_JOIN_READ_FROM_VALUE.Read(_dba, HC_RID: aValueRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hier_Char_Join_Add(int nodeRID, int hierarchyChar_RID)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_JOIN_INSERT.Insert(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  HC_RID: hierarchyChar_RID
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hier_Char_Join_Delete(int nodeRID, int hierarchyChar_RID)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_JOIN_DELETE.Delete(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  HC_RID: hierarchyChar_RID
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Hier_Char_Join_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_HIER_CHAR_JOIN_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Read All Method Override IMO Data
        public DataTable Method_Override_IMO_ReadAll()
        {
            try
            {
                return StoredProcedures.MID_METHOD_OVERRIDE_IMO_READ_ALL.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Delete NODE_IMO Data
        public void Node_IMO_Delete(int nodeRID)
        {
            try
            {
                StoredProcedures.MID_NODE_IMO_DELETE.Delete(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Delete NODE_IMO Data
        public void Node_IMO_Delete(int nodeRID, int storeRID)
        {
            try
            {
                StoredProcedures.MID_NODE_IMO_DELETE_FROM_STORE.Delete(_dba, 
                                                                       HN_RID: nodeRID,
                                                                       ST_RID: storeRID
                                                                       );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Read NODE_IMO Data by HN_RID
        public DataTable Node_IMO_Read(int nodeRID)
        {
            try
            {
                return StoredProcedures.MID_NODE_IMO_READ_FOR_STORES.Read(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Insert NODE_IMO Data
        public void Node_IMO_Insert(int HN_RID, int ST_RID, int IMO_MIN_SHIP_QTY, double IMO_PCT_PK_THRSHLD, int IMO_MAX_VALUE, double IMO_FWOS_MAX, int IMO_FWOS_MAX_RID, eModifierType IMO_FWOS_MAX_TYPE, int IMO_PSH_BK_STK) //TT#108 - MD - DOConnell - FWOS Max Model 
        {
            try
            {
                double? IMO_PCT_PK_THRSHLD_Nullable = null;
                if (IMO_PCT_PK_THRSHLD != Include.PercentPackThresholdDefault) IMO_PCT_PK_THRSHLD_Nullable = IMO_PCT_PK_THRSHLD;

                double? IMO_FWOS_MAX_Nullable = null;
                if (IMO_FWOS_MAX != int.MaxValue) IMO_FWOS_MAX_Nullable = IMO_FWOS_MAX;

                int? IMO_FWOS_MAX_RID_Nullable;
                if (IMO_FWOS_MAX_RID == int.MaxValue || IMO_FWOS_MAX_RID == Include.NoRID)
                {
                    IMO_FWOS_MAX_RID_Nullable = null;
                }
                else
                {
                    IMO_FWOS_MAX_RID_Nullable = IMO_FWOS_MAX_RID;
                }

                int? IMO_FWOS_MAX_TYPE_Nullable = null;
                if (IMO_FWOS_MAX_TYPE != eModifierType.None) IMO_FWOS_MAX_TYPE_Nullable = Convert.ToInt32(IMO_FWOS_MAX_TYPE, CultureInfo.CurrentUICulture);

                StoredProcedures.MID_NODE_IMO_INSERT.Insert(_dba, 
                                                            HN_RID: HN_RID,
                                                            ST_RID: ST_RID,
                                                            IMO_MIN_SHIP_QTY: IMO_MIN_SHIP_QTY,
                                                            IMO_PCT_PK_THRSHLD: IMO_PCT_PK_THRSHLD_Nullable,
                                                            IMO_FWOS_MAX: IMO_FWOS_MAX_Nullable,
                                                            IMO_MAX_VALUE: IMO_MAX_VALUE,
                                                            IMO_PSH_BK_STK: IMO_PSH_BK_STK,
                                                            IMO_FWOS_MAX_RID: IMO_FWOS_MAX_RID_Nullable,
                                                            IMO_FWOS_MAX_TYPE: IMO_FWOS_MAX_TYPE_Nullable
                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // END TT#1401 - GTaylor - Reservation Stores
        //

		public void PurgeCriteria_Add(int nodeRID, int purgeDailyHistory, int purgeWeeklyHistory, int purgePlans)
		{
			try
			{
                int? purgeDailyHistory_Nullable = null;
                if (purgeDailyHistory != Include.Undefined) purgeDailyHistory_Nullable = purgeDailyHistory;

                int? purgeWeeklyHistory_Nullable = null;
                if (purgeWeeklyHistory != Include.Undefined) purgeWeeklyHistory_Nullable = purgeWeeklyHistory;

                int? purgePlans_Nullable = null;
                if (purgePlans != Include.Undefined) purgePlans_Nullable = purgePlans;

                StoredProcedures.MID_PURGE_CRITERIA_INSERT.Insert(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  PURGE_DAILY_HISTORY: purgeDailyHistory_Nullable,
                                                                  PURGE_WEEKLY_HISTORY: purgeWeeklyHistory_Nullable,
                                                                  PURGE_PLANS: purgePlans_Nullable
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        public void PurgeCriteria_Add_HeaderType(int nodeRID, eHeaderType purgeHeaderType, int purgeHeaderTimeframe,
        int purgeHeaders)
        {
	        try
	        {
                int? purgeHeaders_Nullable = null;
                if (purgeHeaders != Include.Undefined) purgeHeaders_Nullable = purgeHeaders;

                int? purgeHeaderTimeframe_Nullable = null;
                if (purgeHeaderTimeframe != Include.Undefined) purgeHeaderTimeframe_Nullable = purgeHeaderTimeframe;

                StoredProcedures.MID_PURGE_CRITERIA_HEADER_PURGE_INSERT.Insert(_dba, 
                                                                               HN_RID: nodeRID,
                                                                               HEADER_TYPE: (int)purgeHeaderType,
                                                                               PURGE_HEADERS_TIMEFRAME: purgeHeaderTimeframe_Nullable,
                                                                               PURGE_HEADERS: purgeHeaders_Nullable
                                                                               );
	        }
	        catch ( Exception err )
	        {
		        string message = err.ToString();
		        throw;
	        }
        }
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

        public void PurgeCriteria_DeleteAll(int nodeRID)
        {
            try
            {
                StoredProcedures.MID_PURGE_CRITERIA_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type


		public DataTable StoreGrades_Read(int nodeRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STORE_GRADES_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreGrade_Add(int nodeRID, string gradeCode, int boundary, int WOSIndex)
		{
			try
			{
                StoredProcedures.MID_STORE_GRADES_INSERT.Insert(_dba, 
                                                                HN_RID: nodeRID,
                                                                GRADE_CODE: gradeCode,
                                                                BOUNDARY: boundary,
                                                                WOS_INDEX: (double)WOSIndex
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreGrade_Update(int nodeRID, string gradeCode, int boundary, int WOSIndex)
		{
			try
			{
                StoredProcedures.MID_STORE_GRADES_UPDATE.Update(_dba, 
                                                                HN_RID: nodeRID,
                                                                GRADE_CODE: gradeCode,
                                                                BOUNDARY: boundary,
                                                                WOS_INDEX: (double)WOSIndex
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void StoreGrade_Delete(int nodeRID, int boundary)
		{
			try
			{
                StoredProcedures.MID_STORE_GRADES_DELETE.Delete(_dba, 
                                                                HN_RID: nodeRID,
                                                                BOUNDARY: boundary
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreGrade_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_STORE_GRADES_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
        public void StoreGrade_UpdateBoundary(int nodeRID, int boundary, int originalBoundary)
        {
            try
            {
                StoredProcedures.MID_STORE_GRADES_UPDATE_BOUNDARY.Update(_dba,
                                                                         HN_RID: nodeRID,
                                                                         BOUNDARY: boundary,
                                                                         ORIGINAL_BOUNDARY: originalBoundary
                                                                         );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review



		public void MinMax_Add(int nodeRID, int boundary,
			int minimumStock, int maximumStock, int minimumAd,
            int minimumColor, int maximumColor, int shipUpTo)  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
		{
			try
			{      
                StoredProcedures.MID_STORE_GRADES_INSERT_WITH_MINMAX.Insert(_dba, 
                                                                            HN_RID: nodeRID,
                                                                            BOUNDARY: boundary,
                                                                            MINIMUM_STOCK: minimumStock,
                                                                            MAXIMUM_STOCK: maximumStock,
                                                                            MINIMUM_AD: minimumAd,
                                                                            MINIMUM_COLOR: minimumColor,
                                                                            MAXIMUM_COLOR: maximumColor,
                                                                            SHIP_UP_TO: shipUpTo
                                                                            );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void MinMax_Update(int nodeRID, 	int boundary,  
			int minimumStock, int maximumStock, int minimumAd,
            int minimumColor, int maximumColor, int shipUpTo)  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
		{
			try
			{
                StoredProcedures.MID_STORE_GRADES_UPDATE_WITH_MINMAX.Update(_dba, 
                                                                            HN_RID: nodeRID,
                                                                            BOUNDARY: boundary,
                                                                            MINIMUM_STOCK: minimumStock,
                                                                            MAXIMUM_STOCK: maximumStock,
                                                                            MINIMUM_AD: minimumAd,
                                                                            MINIMUM_COLOR: minimumColor,
                                                                            MAXIMUM_COLOR: maximumColor,
                                                                            SHIP_UP_TO: shipUpTo
                                                                            );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable VelocityGrades_Read(int nodeRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_VELOCITY_GRADE_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void VelocityGrade_Add(int nodeRID, string gradeCode, int boundary)
		{
			try
			{
                StoredProcedures.MID_VELOCITY_GRADE_INSERT.Insert(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  BOUNDARY: boundary,
                                                                  GRADE_CODE: gradeCode
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void VelocityGrade_Update(int nodeRID, string gradeCode,
			int boundary)
		{
			try
			{
                StoredProcedures.MID_VELOCITY_GRADE_UPDATE.Update(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  BOUNDARY: boundary,
                                                                  GRADE_CODE: gradeCode
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void VelocityGrade_Delete(int nodeRID, int boundary)
		{
			try
			{
                StoredProcedures.MID_VELOCITY_GRADE_DELETE.Delete(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  BOUNDARY: boundary
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void VelocityGrade_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_VELOCITY_GRADE_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		public void VelocityMinMax_Add(int nodeRID, int boundary, int minimumStock, int maximumStock, int minimumAd)
		{
			try
			{
                StoredProcedures.MID_VELOCITY_GRADE_INSERT_WITH_MINMAX.Insert(_dba, 
                                                                              HN_RID: nodeRID,
                                                                              BOUNDARY: boundary,
                                                                              MINIMUM_STOCK: minimumStock,
                                                                              MAXIMUM_STOCK: maximumStock,
                                                                              MINIMUM_AD: minimumAd
                                                                              );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void VelocityMinMax_Update(int nodeRID, int boundary, int minimumStock, int maximumStock, int minimumAd)
		{
			try
			{
                StoredProcedures.MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX.Update(_dba, 
                                                                              HN_RID: nodeRID,
                                                                              BOUNDARY: boundary,
                                                                              MINIMUM_STOCK: minimumStock,
                                                                              MAXIMUM_STOCK: maximumStock,
                                                                              MINIMUM_AD: minimumAd
                                                                              );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		//End TT#505 - JScott - Velocity - Apply Min/Max
		public DataTable SellThruPcts_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_SELL_THRU_READ.Read(_dba, HN_RID: nodeRID);     
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SellThruPct_Add(int nodeRID, int sellThruPct)
		{
			try
			{
                StoredProcedures.MID_SELL_THRU_INSERT.Insert(_dba, 
                                                             HN_RID: nodeRID,
                                                             SELL_THRU_PCT: sellThruPct
                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SellThruPcts_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_SELL_THRU_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public DataTable EligModel_Read()
		{
			try
			{
                return StoredProcedures.MID_ELIGIBILITY_MODEL_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int EligModel_Add(string EligModelID)
		{
			try
			{
                return StoredProcedures.SP_MID_ELIG_MODEL_INSERT.InsertAndReturnRID(_dba, EM_ID: EligModelID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModel_Delete(int EligModelRID)
		{
			try
			{
                StoredProcedures.MID_ELIGIBILITY_MODEL_DELETE.Delete(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable EligModelSalesEntry_Read(int EligModelRID)
		{
			try
			{
                return StoredProcedures.MID_SALES_ELIGIBILITY_ENTRY_READ.Read(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModelSalesEntry_Add(int EligModelRID, int EligModelEntrySeq, int EligModelEntryDateRange)
		{
			try
			{
                StoredProcedures.MID_SALES_ELIGIBILITY_ENTRY_INSERT.Insert(_dba, 
                                                                           EM_RID: EligModelRID,
                                                                           SALES_EM_SEQUENCE: EligModelEntrySeq,
                                                                           CDR_RID: EligModelEntryDateRange
                                                                           );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModelSalesEntry_Delete(int EligModelRID)
		{
			try
			{
                StoredProcedures.MID_SALES_ELIGIBILITY_ENTRY_DELETE.Delete(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable EligModelStockEntry_Read(int EligModelRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STOCK_ELIGIBILITY_ENTRY_READ.Read(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModelStockEntry_Add(int EligModelRID, int EligModelEntrySeq, int EligModelEntryDateRange)
		{
			try
			{
                StoredProcedures.MID_STOCK_ELIGIBILITY_ENTRY_INSERT.Insert(_dba, 
                                                                           EM_RID: EligModelRID,
                                                                           STOCK_EM_SEQUENCE: EligModelEntrySeq,
                                                                           CDR_RID: EligModelEntryDateRange
                                                                           );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModelStockEntry_Delete(int EligModelRID)
		{
			try
			{
                StoredProcedures.MID_STOCK_ELIGIBILITY_ENTRY_DELETE.Delete(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable EligModelPriShipEntry_Read(int EligModelRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_PRIORITY_SHIPPING_ENTRY_READ.Read(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModelPriShipEntry_Add(int EligModelRID, int EligModelEntrySeq, int EligModelEntryDateRange)
		{
			try
			{
                StoredProcedures.MID_PRIORITY_SHIPPING_ENTRY_INSERT.Insert(_dba, 
                                                                           EM_RID: EligModelRID,
                                                                           PRI_SHIP_EM_SEQUENCE: EligModelEntrySeq,
                                                                           CDR_RID: EligModelEntryDateRange
                                                                           );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void EligModelPriShipEntry_Delete(int EligModelRID)
		{
			try
			{
                StoredProcedures.MID_PRIORITY_SHIPPING_ENTRY_DELETE.Delete(_dba, EM_RID: EligModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable GetFilteredOverrideLowLevelModels(string overrideLowLevelModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED.Read(_dba, NAME_FILTER: overrideLowLevelModelNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, NAME_FILTER: overrideLowLevelModelNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable GetFilteredForcastBalModels(string forecastBalanceModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_FORECAST_BAL_MODEL_READ_FILTERED.Read(_dba, FBMOD_ID_FILTER: forecastBalanceModelNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, FBMOD_ID_FILTER: forecastBalanceModelNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }

        // Begin TT#1638 - JSmith - Revised Model Save
        /// <summary>
        /// Returns a datatable containing the requested models based on the name filter
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredEligibilityModels(string eligibilityModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_ELIGIBILITY_MODEL_READ_FILTERED.Read(_dba, EM_ID_FILTER: eligibilityModelNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, EM_ID_FILTER: eligibilityModelNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }
        // End TT#1638 - JSmith - Revised Model Save


		public DataTable StkModModel_Read()
		{
			try
			{
                return StoredProcedures.MID_STOCK_MODIFIER_MODEL_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int StkModModel_Add(string stkModModelID, double stkModModelDefault)
		{
			try
			{
                return StoredProcedures.SP_MID_STKMOD_MODEL_INSERT.InsertAndReturnRID(_dba, 
                                                                          STKMOD_ID: stkModModelID,
                                                                          STKMOD_DEFAULT: stkModModelDefault
                                                                          );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StkModModel_Update(int stkModModelRID, string stkModModelID, double stkModModelDefault)
		{
			try
			{
                StoredProcedures.MID_STOCK_MODIFIER_MODEL_UPDATE.Update(_dba,  STKMOD_RID: stkModModelRID,
                                                                               STKMOD_ID: stkModModelID,
                                                                               STKMOD_DEFAULT: stkModModelDefault
                                                                               );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		public void StkModModel_Delete(int StkModModelRID)
		{
			try
			{
                StoredProcedures.MID_STOCK_MODIFIER_MODEL_DELETE.Delete(_dba, STKMOD_RID: StkModModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable StkModModelEntry_Read(int StkModModelRID)
		{
			try
			{
                return StoredProcedures.MID_STOCK_MODIFIER_MODEL_ENTRY_READ.Read(_dba, STKMOD_RID: StkModModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StkModModelEntry_Add(int StkModModelRID, int StkModModelEntrySeq, double StkModModelEntryValue, int StkModModelEntryDateRange)
		{
			try
			{
                StoredProcedures.MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT.Insert(_dba, 
                                                                              STKMOD_RID: StkModModelRID,
                                                                              STKMOD_SEQUENCE: StkModModelEntrySeq,
                                                                              STKMOD_VALUE: StkModModelEntryValue,
                                                                              CDR_RID: StkModModelEntryDateRange
                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StkModModelEntry_Delete(int StkModModelRID)
		{
			try
			{
                StoredProcedures.MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE.Delete(_dba, STKMOD_RID: StkModModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1638 - JSmith - Revised Model Save
        /// <summary>
        /// Returns a datatable containing the requested models based on a name filter
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredStockModifierModels(string stockModifierNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_STOCK_MODIFIER_MODEL_READ_FILTERED.Read(_dba, STKMOD_ID_FILTER: stockModifierNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, STKMOD_ID_FILTER: stockModifierNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }
        // End TT#1638 - JSmith - Revised Model Save

		public DataTable SlsModModel_Read()
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //string SQLCommand = "SELECT * FROM SALES_MODIFIER_MODEL ";
                //// end MID Track # 2354
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "SALES_MODIFIER_MODEL" );
                //return _dba.ExecuteStoredProcedureForRead("MID_SALES_MODIFIER_MODEL_READ_ALL");
                return StoredProcedures.MID_SALES_MODIFIER_MODEL_READ_ALL.Read(_dba);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int SlsModModel_Add(string slsModModelID, double slsModModelDefault)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@SLSMOD_ID", slsModModelID),
                //                              new MIDDbParameter("@SLSMOD_DEFAULT", slsModModelDefault) } ;
                //InParams[0].DbType = eDbType.VarChar;
                //InParams[0].Direction = eParameterDirection.Input;
                //InParams[1].DbType = eDbType.Float;
                //InParams[1].Direction = eParameterDirection.Input;
								
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@SLSMOD_RID", DBNull.Value) };
                //OutParams[0].DbType = eDbType.Int;
                //OutParams[0].Direction = eParameterDirection.Output;

                //return _dba.ExecuteStoredProcedure("SP_MID_SLSMOD_MODEL_INSERT", InParams, OutParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SLSMOD_ID", slsModModelID, eDbType.VarChar),
                //                              new MIDDbParameter("@SLSMOD_DEFAULT", slsModModelDefault, eDbType.Float) 
                //                            };
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@SLSMOD_RID", DBNull.Value, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForInsert("SP_MID_SLSMOD_MODEL_INSERT", InParams, OutParams);
                return StoredProcedures.SP_MID_SLSMOD_MODEL_INSERT.InsertAndReturnRID(_dba, 
                                                                                      SLSMOD_ID: slsModModelID,
                                                                                      SLSMOD_DEFAULT: slsModModelDefault
                                                                                     );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SlsModModel_Update(int slsModModelRID, string slsModModelID, double slsModModelDefault)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "UPDATE SALES_MODIFIER_MODEL with (rowlock) SET "
                //    + " SLSMOD_ID = @SLSMOD_ID,"
                //    + " SLSMOD_DEFAULT = " + slsModModelDefault.ToString(CultureInfo.CurrentUICulture)
                //    + " WHERE SLSMOD_RID = " + slsModModelRID.ToString(CultureInfo.CurrentUICulture);
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@SLSMOD_ID", slsModModelID, eDbType.VarChar) } ;

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@STKMOD_RID", slsModModelRID, eDbType.Int),
                //                              new MIDDbParameter("@STKMOD_ID", slsModModelID, eDbType.VarChar),
                //                              new MIDDbParameter("@STKMOD_DEFAULT", slsModModelDefault, eDbType.Float) 
                //                            };
                //_dba.ExecuteStoredProcedureForUpdate("MID_SALES_MODIFIER_MODEL_UPDATE", InParams);
                StoredProcedures.MID_SALES_MODIFIER_MODEL_UPDATE.Update(_dba,
                                                                        SLSMOD_RID: slsModModelRID,
                                                                        SLSMOD_ID: slsModModelID,
                                                                        SLSMOD_DEFAULT: slsModModelDefault
                                                                        );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SlsModModel_Delete(int SlsModModelRID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand;
                //SQLCommand = "DELETE FROM SALES_MODIFIER_MODEL_ENTRY with (rowlock) "
                //    + " WHERE SLSMOD_RID = " +  SlsModModelRID.ToString(CultureInfo.CurrentUICulture);

                //_dba.ExecuteNonQuery(SQLCommand);

                //SQLCommand = "DELETE FROM SALES_MODIFIER_MODEL with (rowlock) "
                //    + " WHERE SLSMOD_RID = " +  SlsModModelRID.ToString(CultureInfo.CurrentUICulture);

                //_dba.ExecuteNonQuery(SQLCommand);

                //MIDDbParameter[] InParams = { new MIDDbParameter("@SLSMOD_RID", SlsModModelRID, eDbType.Int) };
                //_dba.ExecuteStoredProcedureForUpdate("MID_SALES_MODIFIER_MODEL_DELETE", InParams);
                StoredProcedures.MID_SALES_MODIFIER_MODEL_DELETE.Delete(_dba, SLSMOD_RID: SlsModModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SlsModModelEntry_Read(int SlsModModelRID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //string SQLCommand = @"SELECT smme.SLSMOD_RID, smme.SLSMOD_SEQUENCE,"
                //    + @"smme.SLSMOD_VALUE, smme.CDR_RID,"
                //    + @"cdr.CDR_START, cdr.CDR_END, cdr.CDR_DATE_TYPE_ID, cdr.CDR_RELATIVE_TO, cdr.CDR_RANGE_TYPE_ID "
                //    + @" FROM SALES_MODIFIER_MODEL_ENTRY smme, CALENDAR_DATE_RANGE cdr"
                //    + @" WHERE SLSMOD_RID=" + SlsModModelRID.ToString(CultureInfo.CurrentUICulture)
                //    + @" AND smme.CDR_RID = cdr.CDR_RID "
                //    + @" ORDER BY SLSMOD_SEQUENCE ";
                //// end MID Track # 2354
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "SALES_MODIFIER_MODEL_ENTRY" );
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SLSMOD_RID", SlsModModelRID, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForRead("MID_SALES_MODIFIER_MODEL_ENTRY_READ", InParams);
                return StoredProcedures.MID_SALES_MODIFIER_MODEL_ENTRY_READ.Read(_dba, SLSMOD_RID: SlsModModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance 
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SlsModModelEntry_Add(int SlsModModelRID, int SlsModModelEntrySeq, double StkModModelEntryValue, int SlsModModelEntryDateRange)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "INSERT INTO SALES_MODIFIER_MODEL_ENTRY(SLSMOD_RID, SLSMOD_SEQUENCE, SLSMOD_VALUE, CDR_RID)"
                //    + " VALUES ("
                //    + SlsModModelRID.ToString(CultureInfo.CurrentUICulture) + "," 
                //    + SlsModModelEntrySeq.ToString(CultureInfo.CurrentUICulture) + ","
                //    + StkModModelEntryValue.ToString(CultureInfo.CurrentUICulture) + ","
                //    + SlsModModelEntryDateRange.ToString(CultureInfo.CurrentUICulture) + ")";
                //_dba.ExecuteNonQuery(SQLCommand);

                //MIDDbParameter[] InParams = { new MIDDbParameter("@SLSMOD_RID", SlsModModelRID, eDbType.Int),
                //                              new MIDDbParameter("@SLSMOD_SEQUENCE", SlsModModelEntrySeq, eDbType.Int),
                //                              new MIDDbParameter("@SLSMOD_VALUE", StkModModelEntryValue, eDbType.Float),
                //                              new MIDDbParameter("@CDR_RID", SlsModModelEntryDateRange, eDbType.Int)
                //                            };
                //_dba.ExecuteStoredProcedureForInsert("MID_SALES_MODIFIER_MODEL_ENTRY_INSERT", InParams);
                StoredProcedures.MID_SALES_MODIFIER_MODEL_ENTRY_INSERT.Insert(_dba, 
                                                                              SLSMOD_RID: SlsModModelRID,
                                                                              SLSMOD_SEQUENCE: SlsModModelEntrySeq,
                                                                              SLSMOD_VALUE: StkModModelEntryValue,
                                                                              CDR_RID: SlsModModelEntryDateRange
                                                                              );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SlsModModelEntry_Delete(int SlsModModelRID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "DELETE FROM SALES_MODIFIER_MODEL_ENTRY with (rowlock) WHERE SLSMOD_RID="
                //    + SlsModModelRID.ToString(CultureInfo.CurrentUICulture);
                //_dba.ExecuteNonQuery(SQLCommand);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@SLSMOD_RID", SlsModModelRID, eDbType.Int) };
                //_dba.ExecuteStoredProcedureForDelete("MID_SALES_MODIFIER_MODEL_ENTRY_DELETE", InParams);
                StoredProcedures.MID_SALES_MODIFIER_MODEL_ENTRY_DELETE.Delete(_dba, SLSMOD_RID: SlsModModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1638 - JSmith - Revised Model Save
        /// <summary>
        /// Returns a datatable containing the requested models based on a name filter
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredSalesModifierModels(string salesModifierNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_SALES_MODIFIER_MODEL_READ_FILTERED.Read(_dba, SLSMOD_ID_FILTER: salesModifierNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, SLSMOD_ID_FILTER: salesModifierNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }
        // End TT#1638 - JSmith - Revised Model Save

		// BEGIN MID Track #4370 - John Smith - FWOS Models
		public DataTable FWOSModModel_Read()
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "SELECT * FROM FWOS_MODIFIER_MODEL ";
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "FWOS_MODIFIER_MODEL" );
                //return _dba.ExecuteStoredProcedureForRead("MID_FWOS_MODIFIER_MODEL_READ_ALL");
                return StoredProcedures.MID_FWOS_MODIFIER_MODEL_READ_ALL.Read(_dba);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance 
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int FWOSModModel_Add(string FWOSModModelID, double FWOSModModelDefault)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_ID", FWOSModModelID, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@FWOSMOD_DEFAULT", FWOSModModelDefault, eDbType.Float, eParameterDirection.Input) } ;
								
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@FWOSMOD_RID", DBNull.Value) };
                //OutParams[0].DbType = eDbType.Int;
                //OutParams[0].Direction = eParameterDirection.Output;

                //return _dba.ExecuteStoredProcedure("SP_MID_FWOSMOD_MODEL_INSERT", InParams, OutParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMOD_ID", FWOSModModelID, eDbType.VarChar),
                //                              new MIDDbParameter("@FWOSMOD_DEFAULT", FWOSModModelDefault, eDbType.Float) 
                //                            };
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@FWOSMOD_RID", DBNull.Value, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForInsert("SP_MID_FWOSMOD_MODEL_INSERT", InParams, OutParams);
                return StoredProcedures.SP_MID_FWOSMOD_MODEL_INSERT.InsertAndReturnRID(_dba, 
                                                                                       FWOSMOD_ID: FWOSModModelID,
                                                                                       FWOSMOD_DEFAULT: FWOSModModelDefault
                                                                                       );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void FWOSModModel_Update(int FWOSModModelRID, string FWOSModModelID, double FWOSModModelDefault)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "UPDATE FWOS_MODIFIER_MODEL with (rowlock) SET "
                //    + " FWOSMOD_ID = @FWOSMOD_ID,"
                //    + " FWOSMOD_DEFAULT = @FWOSMOD_DEFAULT"
                //    + " WHERE FWOSMOD_RID = @FWOSMOD_RID";
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_ID", FWOSModModelID, eDbType.VarChar, eParameterDirection.Input),
                //                            new MIDDbParameter("@FWOSMOD_DEFAULT", FWOSModModelDefault, eDbType.Float, eParameterDirection.Input),
                //                            new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int, eParameterDirection.Input) } ;

                //_dba.ExecuteNonQuery(SQLCommand, InParams);

                //MIDDbParameter[] InParams = {  new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int),
                //                               new MIDDbParameter("@FWOSMOD_ID", FWOSModModelID, eDbType.VarChar),
                //                               new MIDDbParameter("@FWOSMOD_DEFAULT", FWOSModModelDefault, eDbType.Float)
                //                            };
                //_dba.ExecuteStoredProcedureForUpdate("MID_FWOS_MODIFIER_MODEL_UPDATE", InParams);
                StoredProcedures.MID_FWOS_MODIFIER_MODEL_UPDATE.Update(_dba, 
                                                                       FWOSMOD_RID: FWOSModModelRID,
                                                                       FWOSMOD_ID: FWOSModModelID,
                                                                       FWOSMOD_DEFAULT: FWOSModModelDefault
                                                                       );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void FWOSModModel_Delete(int FWOSModModelRID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand;
                //SQLCommand = "DELETE FROM FWOS_MODIFIER_MODEL_ENTRY with (rowlock) "
                //    + " WHERE FWOSMOD_RID = @FWOSMOD_RID";
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int, eParameterDirection.Input) } ;

                //_dba.ExecuteNonQuery(SQLCommand, InParams);

                //SQLCommand = "DELETE FROM FWOS_MODIFIER_MODEL with (rowlock) "
                //    + " WHERE FWOSMOD_RID = @FWOSMOD_RID";

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams = {  new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int) };
                //_dba.ExecuteStoredProcedureForDelete("MID_FWOS_MODIFIER_MODEL_DELETE", InParams);
                StoredProcedures.MID_FWOS_MODIFIER_MODEL_DELETE.Delete(_dba, FWOSMOD_RID: FWOSModModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable FWOSModModelEntry_Read(int FWOSModModelRID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = @"SELECT smme.FWOSMOD_RID, smme.FWOSMOD_SEQUENCE,"
                //    + @"smme.FWOSMOD_VALUE, smme.CDR_RID,"
                //    + @"cdr.CDR_START, cdr.CDR_END, cdr.CDR_DATE_TYPE_ID, cdr.CDR_RELATIVE_TO, cdr.CDR_RANGE_TYPE_ID "
                //    + @" FROM FWOS_MODIFIER_MODEL_ENTRY smme, CALENDAR_DATE_RANGE cdr"
                //    + @" WHERE FWOSMOD_RID = @FWOSMOD_RID"
                //    + @" AND smme.CDR_RID = cdr.CDR_RID "
                //    + @" ORDER BY FWOSMOD_SEQUENCE ";

                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int, eParameterDirection.Input) } ;
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "FWOS_MODIFIER_MODEL_ENTRY", InParams );
                //MIDDbParameter[] InParams = {  new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForRead("MID_FWOS_MODIFIER_MODEL_ENTRY_READ", InParams);
                return StoredProcedures.MID_FWOS_MODIFIER_MODEL_ENTRY_READ.Read(_dba, FWOSMOD_RID: FWOSModModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void FWOSModModelEntry_Add(int FWOSModModelRID, int FWOSModModelEntrySeq, double FWOSModModelEntryValue, int FWOSModModelEntryDateRange)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "INSERT INTO FWOS_MODIFIER_MODEL_ENTRY(FWOSMOD_RID, FWOSMOD_SEQUENCE, FWOSMOD_VALUE, CDR_RID)"
                //    + " VALUES (@FWOSMOD_RID, @FWOSMOD_ENTRYSEQ, @FWOSMOD_ENTRYVALUE, @FWOSMOD_ENTRYDATE)";
					
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@FWOSMOD_ENTRYSEQ", FWOSModModelEntrySeq, eDbType.Int, eParameterDirection.Input),
                //// Begin Track #6104 stodd
                //                            new MIDDbParameter("@FWOSMOD_ENTRYVALUE", FWOSModModelEntryValue, eDbType.Float, eParameterDirection.Input),
                //// End Track #6104 stodd
                //                            new MIDDbParameter("@FWOSMOD_ENTRYDATE", FWOSModModelEntryDateRange, eDbType.Int, eParameterDirection.Input)} ;

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int),
                //                              new MIDDbParameter("@FWOSMOD_SEQUENCE", FWOSModModelEntrySeq, eDbType.Int),
                //                              new MIDDbParameter("@FWOSMOD_VALUE", FWOSModModelEntryValue, eDbType.Float),
                //                              new MIDDbParameter("@CDR_RID", FWOSModModelEntryDateRange, eDbType.Int)
                //                            };
                //_dba.ExecuteStoredProcedureForInsert("MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT", InParams);
                StoredProcedures.MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT.Insert(_dba, 
                                                                             FWOSMOD_RID: FWOSModModelRID,
                                                                             FWOSMOD_SEQUENCE: FWOSModModelEntrySeq,
                                                                             FWOSMOD_VALUE: FWOSModModelEntryValue,
                                                                             CDR_RID: FWOSModModelEntryDateRange
                                                                             );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void FWOSModModelEntry_Delete(int FWOSModModelRID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "DELETE FROM FWOS_MODIFIER_MODEL_ENTRY with (rowlock) WHERE FWOSMOD_RID = @FWOSMOD_RID";
					
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int, eParameterDirection.Input) } ;

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@FWOSMOD_RID", FWOSModModelRID, eDbType.Int) } ;
                //_dba.ExecuteStoredProcedureForDelete("MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE", InParams);
                StoredProcedures.MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE.Delete(_dba, FWOSMOD_RID: FWOSModModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// END MID Track #4370

        // Begin TT#1638 - JSmith - Revised Model Save
        /// <summary>
        /// Returns a datatable containing the requested models based on the name filter
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredFWOSModifierModels(string FWOSModifierNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_FWOS_MODIFIER_MODEL_READ_FILTERED.Read(_dba, FWOSMOD_ID_FILTER: FWOSModifierNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, FWOSMOD_ID_FILTER: FWOSModifierNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1638 - JSmith - Revised Model Save

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        public DataTable FWOSMaxModel_Read()
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "SELECT * FROM FWOS_MAX_MODEL ";

                //return _dba.ExecuteSQLQuery(SQLCommand, "FWOS_MAX_MODEL");
                //return _dba.ExecuteStoredProcedureForRead("MID_FWOS_MAX_MODEL_READ_ALL");
                return StoredProcedures.MID_FWOS_MAX_MODEL_READ_ALL.Read(_dba);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int FWOSMaxModel_Add(string FWOSMaxModelID, double FWOSMaxModelDefault)
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_ID", FWOSMaxModelID, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@FWOSMAX_DEFAULT", FWOSMaxModelDefault, eDbType.Float, eParameterDirection.Input) };

                //MIDDbParameter[] OutParams = { new MIDDbParameter("@FWOSMAX_RID", DBNull.Value) };
                //OutParams[0].DbType = eDbType.Int;
                //OutParams[0].Direction = eParameterDirection.Output;

                //return _dba.ExecuteStoredProcedure("SP_MID_FWOSMAX_MODEL_INSERT", InParams, OutParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_ID", FWOSMaxModelID, eDbType.VarChar),
                //                              new MIDDbParameter("@FWOSMAX_DEFAULT", FWOSMaxModelDefault, eDbType.Float) 
                //                            };
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@FWOSMAX_RID", DBNull.Value, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForInsert("SP_MID_FWOSMAX_MODEL_INSERT", InParams, OutParams);
                return StoredProcedures.SP_MID_FWOSMAX_MODEL_INSERT.InsertAndReturnRID(_dba, 
                                                                                       FWOSMAX_ID: FWOSMaxModelID,
                                                                                       FWOSMAX_DEFAULT: FWOSMaxModelDefault
                                                                                       );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void FWOSMaxModel_Update(int FWOSMaxModelRID, string FWOSMaxModelID, double FWOSMaxModelDefault)
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "UPDATE FWOS_MAX_MODEL with (rowlock) SET "
                //    + " FWOSMAX_ID = @FWOSMAX_ID,"
                //    + " FWOSMAX_DEFAULT = @FWOSMAX_DEFAULT"
                //    + " WHERE FWOSMAX_RID = @FWOSMAX_RID";
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_ID", FWOSMaxModelID, eDbType.VarChar, eParameterDirection.Input),
                //                            new MIDDbParameter("@FWOSMAX_DEFAULT", FWOSMaxModelDefault, eDbType.Float, eParameterDirection.Input),
                //                            new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int, eParameterDirection.Input) };

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams = {  new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int),
                //                               new MIDDbParameter("@FWOSMAX_ID", FWOSMaxModelID, eDbType.VarChar),
                //                               new MIDDbParameter("@FWOSMAX_DEFAULT", FWOSMaxModelDefault, eDbType.Float)
                //                            };
                // _dba.ExecuteStoredProcedureForUpdate("MID_FWOS_MAX_MODEL_UPDATE", InParams);
                StoredProcedures.MID_FWOS_MAX_MODEL_UPDATE.Update(_dba, 
                                                                  FWOSMAX_RID: FWOSMaxModelRID,
                                                                  FWOSMAX_ID: FWOSMaxModelID,
                                                                  FWOSMAX_DEFAULT: FWOSMaxModelDefault
                                                                  );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void FWOSMaxModel_Delete(int FWOSMaxModelRID)
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand;
                //SQLCommand = "DELETE FROM FWOS_MAX_MODEL_ENTRY with (rowlock) "
                //    + " WHERE FWOSMAX_RID = @FWOSMAX_RID";
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int, eParameterDirection.Input) };

                //_dba.ExecuteNonQuery(SQLCommand, InParams);

                //SQLCommand = "DELETE FROM FWOS_MAX_MODEL with (rowlock) "
                //    + " WHERE FWOSMAX_RID = @FWOSMAX_RID";

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int) };
                // _dba.ExecuteStoredProcedureForDelete("MID_FWOS_MAX_MODEL_DELETE", InParams);
                StoredProcedures.MID_FWOS_MAX_MODEL_DELETE.Delete(_dba, FWOSMAX_RID: FWOSMaxModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable FWOSMaxModelEntry_Read(int FWOSMaxModelRID)
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = @"SELECT smme.FWOSMAX_RID, smme.FWOSMAX_SEQUENCE,"
                //    + @"smme.FWOSMAX_VALUE, smme.CDR_RID,"
                //    + @"cdr.CDR_START, cdr.CDR_END, cdr.CDR_DATE_TYPE_ID, cdr.CDR_RELATIVE_TO, cdr.CDR_RANGE_TYPE_ID "
                //    + @" FROM FWOS_MAX_MODEL_ENTRY smme, CALENDAR_DATE_RANGE cdr"
                //    + @" WHERE FWOSMAX_RID = @FWOSMAX_RID"
                //    + @" AND smme.CDR_RID = cdr.CDR_RID "
                //    + @" ORDER BY FWOSMAX_SEQUENCE ";

                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int, eParameterDirection.Input) };

                //return _dba.ExecuteSQLQuery(SQLCommand, "FWOS_MAX_MODEL_ENTRY", InParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForRead("MID_FWOS_MAX_MODEL_ENTRY_READ", InParams);
                return StoredProcedures.MID_FWOS_MAX_MODEL_ENTRY_READ.Read(_dba, FWOSMAX_RID: FWOSMaxModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
                
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void FWOSMaxModelEntry_Add(int FWOSMaxModelRID, int FWOSMaxModelEntrySeq, double FWOSMaxModelEntryValue, int FWOSMaxModelEntryDateRange)
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "INSERT INTO FWOS_MAX_MODEL_ENTRY(FWOSMAX_RID, FWOSMAX_SEQUENCE, FWOSMAX_VALUE, CDR_RID)"
                //    + " VALUES (@FWOSMAX_RID, @FWOSMAX_ENTRYSEQ, @FWOSMAX_ENTRYVALUE, @FWOSMAX_ENTRYDATE)";

                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int, eParameterDirection.Input),
                //                            new MIDDbParameter("@FWOSMAX_ENTRYSEQ", FWOSMaxModelEntrySeq, eDbType.Int, eParameterDirection.Input),
                //// Begin Track #6104 stodd
                //                            new MIDDbParameter("@FWOSMAX_ENTRYVALUE", FWOSMaxModelEntryValue, eDbType.Float, eParameterDirection.Input),
                //// End Track #6104 stodd
                //                            new MIDDbParameter("@FWOSMAX_ENTRYDATE", FWOSMaxModelEntryDateRange, eDbType.Int, eParameterDirection.Input)};

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int),
                //                              new MIDDbParameter("@FWOSMAX_SEQUENCE", FWOSMaxModelEntrySeq, eDbType.Int),
                //                              new MIDDbParameter("@FWOSMAX_VALUE", FWOSMaxModelEntryValue, eDbType.Float),
                //                              new MIDDbParameter("@CDR_RID", FWOSMaxModelEntryDateRange, eDbType.Int) 
                //                            };
                //_dba.ExecuteStoredProcedureForInsert("MID_FWOS_MAX_MODEL_ENTRY_INSERT", InParams);
                StoredProcedures.MID_FWOS_MAX_MODEL_ENTRY_INSERT.Insert(_dba, 
                                                                        FWOSMAX_RID: FWOSMaxModelRID,
                                                                        FWOSMAX_SEQUENCE: FWOSMaxModelEntrySeq,
                                                                        FWOSMAX_VALUE: FWOSMaxModelEntryValue,
                                                                        CDR_RID: FWOSMaxModelEntryDateRange
                                                                        );
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void FWOSMaxModelEntry_Delete(int FWOSMaxModelRID)
        {
            try
            {
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "DELETE FROM FWOS_MAX_MODEL_ENTRY with (rowlock) WHERE FWOSMAX_RID = @FWOSMAX_RID";

                //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int, eParameterDirection.Input) };

                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                 //MIDDbParameter[] InParams = { new MIDDbParameter("@FWOSMAX_RID", FWOSMaxModelRID, eDbType.Int) };
                 //_dba.ExecuteStoredProcedureForDelete("MID_FWOS_MAX_MODEL_ENTRY_DELETE", InParams);
                StoredProcedures.MID_FWOS_MAX_MODEL_ENTRY_DELETE.Delete(_dba, FWOSMAX_RID: FWOSMaxModelRID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#108 - MD - DOConnell - FWOS Max Model Enhancement

        /// <summary>
        /// Returns a datatable containing the requested models based on the name filter
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredFWOSMaxModels(string FWOSMaxModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                if (isCaseSensitive)
                {
                    return StoredProcedures.MID_FWOS_MAX_MODEL_READ_FILTERED.Read(_dba, FWOSMAX_ID_FILTER: FWOSMaxModelNameFilter);
                }
                else
                {
                    return StoredProcedures.MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE.Read(_dba, FWOSMAX_ID_FILTER: FWOSMaxModelNameFilter);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }
        // End TT#1638 - JSmith - Revised Model Save
		
     

        


		public DataTable DailyPercentagesDefaults_Read(int nodeRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_DAILY_PERCENTAGES_DEFAULTS_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DailyPercentagesDefaults_Add(int nodeRID, int storeRID, double day1, double day2, double day3, double day4, double day5, double day6, double day7)
		{
			try
			{
                StoredProcedures.MID_DAILY_PERCENTAGES_DEFAULTS_INSERT.Insert(_dba, 
                                                                              HN_RID: nodeRID,
                                                                              ST_RID: storeRID,
                                                                              DAY1: day1,
                                                                              DAY2: day2,
                                                                              DAY3: day3,
                                                                              DAY4: day4,
                                                                              DAY5: day5,
                                                                              DAY6: day6,
                                                                              DAY7: day7
                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DailyPercentagesDefaults_Delete(int nodeRID, int storeRID)
		{
			try
			{
                StoredProcedures.MID_DAILY_PERCENTAGES_DEFAULTS_DELETE.Delete(_dba, 
                                                                              HN_RID: nodeRID,
                                                                              ST_RID: storeRID
                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DailyPercentagesDefaults_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable DailyPercentages_Read(int nodeRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_DAILY_PERCENTAGES_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

      

        public void DailyPercentages_Add(int nodeRID, int storeRID, int dateRange, double day1, double day2, double day3, double day4, double day5, double day6, double day7)
        {
            try
            {
                StoredProcedures.MID_DAILY_PERCENTAGES_INSERT.Insert(_dba, 
                                                                     HN_RID: nodeRID,
                                                                     ST_RID: storeRID,
                                                                     CDR_RID: dateRange,
                                                                     DAY1: day1,
                                                                     DAY2: day2,
                                                                     DAY3: day3,
                                                                     DAY4: day4,
                                                                     DAY5: day5,
                                                                     DAY6: day6,
                                                                     DAY7: day7
                                                                     );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		public void DailyPercentages_Delete(int nodeRID, int storeRID, int dateRange)
		{
			try
			{
                StoredProcedures.MID_DAILY_PERCENTAGES_DELETE.Delete(_dba, 
                                                                     HN_RID: nodeRID,
                                                                     ST_RID: storeRID,
                                                                     CDR_RID: dateRange
                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DailyPercentages_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_DAILY_PERCENTAGES_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		public DataTable StoreEligibility_Read(int nodeRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STORE_ELIGIBILITY_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreEligibility_Add(int nodeRID, int storeRID, bool eligibilityInherited,
			int emRID, bool ineligible, 
			eModifierType stkModType, int stkModRID, double stkModPct,
			eModifierType slsModType, int slsModRID, double slsModPct, 
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			eModifierType FWOSModType, int FWOSModRID, double FWOSModPct,
			// END MID Track #4370
			// BEGIN MID Track #4827 - John Smith - Presentation plus sales
			eSimilarStoreType simStoreType, double simStoreRatio, int untilDate,
			bool aPresPlusSalesIsSet, bool aPresPlusSales
			//BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
			, int stkLeadWeeks)
			//END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
			// END MID Track #4827
		{
			try
			{
                int? emRID_Nullable = null;
                if (emRID != Include.NoRID) emRID_Nullable = emRID;

                char useEligibility;
                if (eligibilityInherited)
				{
					useEligibility = '0';
				}
				else
				{
					useEligibility = '1';
				}

                int? stkModRID_Nullable = null;
                if (stkModRID != Include.NoRID) stkModRID_Nullable = stkModRID;

                int? slsModRID_Nullable = null;
                if (slsModRID != Include.NoRID) slsModRID_Nullable = slsModRID;

                int? FWOSModRID_Nullable = null;
                if (FWOSModRID != Include.NoRID) FWOSModRID_Nullable = FWOSModRID;

                int? untilDate_Nullable = null;
                if (untilDate != Include.Undefined) untilDate_Nullable = untilDate;
                
                char? presentationPlusSalesChar_Nullable = null;
                if (aPresPlusSalesIsSet)
                {
                    presentationPlusSalesChar_Nullable = Include.ConvertBoolToChar(aPresPlusSales);
                }

                int? stkLeadWeeks_Nullable = null;
                if (stkLeadWeeks != Include.NoRID) stkLeadWeeks_Nullable = stkLeadWeeks;

                StoredProcedures.MID_STORE_ELIGIBILITY_INSERT.Insert(_dba, 
                                                                     HN_RID: nodeRID,
                                                                     ST_RID: storeRID,
                                                                     EM_RID: emRID_Nullable,
                                                                     USE_ELIGIBILITY: useEligibility,
                                                                     INELIGIBLE: Include.ConvertBoolToChar(ineligible),
                                                                     STKMOD_TYPE: (int)stkModType,
                                                                     STKMOD_RID: stkModRID_Nullable,
                                                                     STKMOD_PCT: stkModPct,
                                                                     SLSMOD_TYPE: (int)slsModType,
                                                                     SLSMOD_RID: slsModRID_Nullable,
                                                                     SLSMOD_PCT: slsModPct,
                                                                     FWOSMOD_TYPE: (int)FWOSModType,
                                                                     FWOSMOD_RID: FWOSModRID_Nullable,
                                                                     FWOSMOD_PCT: FWOSModPct,
                                                                     SIMILAR_STORE_TYPE: (int)simStoreType,
                                                                     SIMILAR_STORE_RATIO: simStoreRatio,
                                                                     UNTIL_DATE: untilDate_Nullable,
                                                                     PRESENTATION_PLUS_SALES_IND: presentationPlusSalesChar_Nullable,
                                                                     STOCK_LEAD_WEEKS: stkLeadWeeks
                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreEligibility_Update(int nodeRID, int storeRID, bool eligibilityInherited,
			int emRID, bool ineligible, 
			eModifierType stkModType, int stkModRID, double stkModPct,
			eModifierType slsModType, int slsModRID, double slsModPct, 
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			eModifierType FWOSModType, int FWOSModRID, double FWOSModPct,
			// END MID Track #4370
			// BEGIN MID Track #4827 - John Smith - Presentation plus sales
			eSimilarStoreType simStoreType, double simStoreRatio, int untilDate,
			bool aPresPlusSalesIsSet, bool aPresPlusSales
			//BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
			, int stkLeadWeeks)
			//END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
			// END MID Track #4827
		{
			try
			{
                
                int? emRID_Nullable = null;
                if (emRID != Include.NoRID) emRID_Nullable = emRID;

                char useEligibility;
                if (eligibilityInherited)
                {
                    useEligibility = '0';
                }
                else
                {
                    useEligibility = '1';
                }

                int? stkModRID_Nullable = null;
                if (stkModRID != Include.NoRID) stkModRID_Nullable = stkModRID;

                int? slsModRID_Nullable = null;
                if (slsModRID != Include.NoRID) slsModRID_Nullable = slsModRID;

                int? FWOSModRID_Nullable = null;
                if (FWOSModRID != Include.NoRID) FWOSModRID_Nullable = FWOSModRID;

                int? untilDate_Nullable = null;
                if (untilDate != Include.Undefined) untilDate_Nullable = untilDate;

                char? presentationPlusSalesChar_Nullable = null;
                if (aPresPlusSalesIsSet)
                {
                    presentationPlusSalesChar_Nullable = Include.ConvertBoolToChar(aPresPlusSales);
                }

                int? stkLeadWeeks_Nullable = null;
                if (stkLeadWeeks != Include.NoRID) stkLeadWeeks_Nullable = stkLeadWeeks;

                StoredProcedures.MID_STORE_ELIGIBILITY_UPDATE.Update(_dba, 
                                                                     HN_RID: nodeRID,
                                                                     ST_RID: storeRID,
                                                                     EM_RID: emRID_Nullable,
                                                                     USE_ELIGIBILITY: useEligibility,
                                                                     INELIGIBLE: Include.ConvertBoolToChar(ineligible),
                                                                     STKMOD_TYPE: (int)stkModType,
                                                                     STKMOD_RID: stkModRID_Nullable,
                                                                     STKMOD_PCT: stkModPct,
                                                                     SLSMOD_TYPE: (int)slsModType,
                                                                     SLSMOD_RID: slsModRID_Nullable,
                                                                     SLSMOD_PCT: slsModPct,
                                                                     FWOSMOD_TYPE: (int)FWOSModType,
                                                                     FWOSMOD_RID: FWOSModRID_Nullable,
                                                                     FWOSMOD_PCT: FWOSModPct,
                                                                     SIMILAR_STORE_TYPE: (int)simStoreType,
                                                                     SIMILAR_STORE_RATIO: simStoreRatio,
                                                                     UNTIL_DATE: untilDate_Nullable,
                                                                     PRESENTATION_PLUS_SALES_IND: presentationPlusSalesChar_Nullable,
                                                                     STOCK_LEAD_WEEKS: stkLeadWeeks
                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreEligibility_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_STORE_ELIGIBILITY_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreEligibility_Delete(int nodeRID, int storeRID)
		{
			try
			{
                StoredProcedures.MID_STORE_ELIGIBILITY_DELETE.Delete(_dba, 
                                                                     HN_RID: nodeRID,
                                                                     ST_RID: storeRID
                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable SimilarStore_GetStoreRIDs(int nodeRID)
        {
            try
            {
                return StoredProcedures.MID_GET_SIMILAR_STORE_RIDS.Read(_dba, HN_RID: nodeRID);
            }
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
        }

        public DataTable MakeStoreRidParameterDataTable(DataTable dt, string sourceStoreRidColumnName)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ST_RID", typeof(int));
            foreach (DataRow dr in dt.Rows)
            {
                table.Rows.Add(dr[sourceStoreRidColumnName]);
            }
            return table;
        }

		public void SimilarStore_Add(int nodeRID, int storeRID, ArrayList simStores)
		{
			try
			{
				if (simStores != null)
				{
					foreach(int SimStore in simStores)
					{
                        StoredProcedures.MID_SIMILAR_STORES_INSERT.Insert(_dba, 
                                                                          HN_RID: nodeRID,
                                                                          ST_RID: storeRID,
                                                                          SS_RID: SimStore
                                                                          );
					}
				}
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SimilarStore_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_SIMILAR_STORES_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SimilarStore_Delete(int nodeRID, int storeRID)
		{
			try
			{
                StoredProcedures.MID_SIMILAR_STORES_DELETE.Delete(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  ST_RID: storeRID
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		public DataTable SizeCurveCriteria_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_NODE_SIZE_CURVE_CRITERIA_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SizeCurveDefaultCriteria_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public int SizeCurveCriteria_Add(int nodeRID, bool criteriaIsInherited,
			eLowLevelsType levelType, int hierarchyRID, int levelSeq, int levelOffset,
			int dateRID, bool applyLostSales, int OLLRID, int customOLLRID, int sizeGroupRID, string curveName, int strGrpRID)
		{
			try
			{
				if (!criteriaIsInherited)
				{ 
                    int? hierarchyRID_Nullable = null;
                    if (hierarchyRID != Include.NoRID) hierarchyRID_Nullable = hierarchyRID;

                    int? levelSeq_Nullable = null;
                    if (levelSeq != Include.Undefined && levelSeq != 0) levelSeq_Nullable = levelSeq;

                    int? levelOffset_Nullable = null;
                    if (levelOffset != Include.Undefined) levelOffset_Nullable =levelOffset;

                    int? dateRID_Nullable = null;
                    if (dateRID != Include.NoRID) dateRID_Nullable = dateRID;

                    int? OLLRID_Nullable = null;
                    if (OLLRID != Include.NoRID) OLLRID_Nullable = OLLRID;

                    int? customOLLRID_Nullable = null;
                    if (customOLLRID != Include.NoRID) customOLLRID_Nullable = customOLLRID;

                    int? sizeGroupRID_Nullable = null;
                    if (sizeGroupRID != Include.NoRID) sizeGroupRID_Nullable = sizeGroupRID;

                    int? strGrpRID_Nullable = null;
                    if (strGrpRID != Include.NoRID) strGrpRID_Nullable = strGrpRID;

                    return StoredProcedures.SP_MID_ND_SZ_CRV_CRIT_INS.InsertAndReturnRID(_dba, 
                                                                                         HN_RID: nodeRID,
                                                                                         PH_OFFSET_IND: (int)levelType,
                                                                                         PH_RID: hierarchyRID_Nullable,
                                                                                         PHL_SEQUENCE: levelSeq_Nullable,
                                                                                         PHL_OFFSET: levelOffset_Nullable,
                                                                                         CDR_RID: dateRID_Nullable,
                                                                                         APPLY_LOST_SALES_IND: (applyLostSales ? '1' : '0'),
                                                                                         OLL_RID: OLLRID_Nullable,
                                                                                         CUSTOM_OLL_RID: customOLLRID_Nullable,
                                                                                         SIZE_GROUP_RID: sizeGroupRID_Nullable,
                                                                                         CURVE_NAME: curveName,
                                                                                         SG_RID: strGrpRID_Nullable
                                                                                         );
				}

				return -1;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveCriteria_Update(int nodeRID, int criteriaRID, bool criteriaIsInherited,
			eLowLevelsType levelType, int hierarchyRID, int levelSeq, int levelOffset,
			int dateRID, bool applyLostSales, int OLLRID, int customOLLRID, int sizeGroupRID, string curveName, int strGrpRID)
		{
			try
			{
				if (!criteriaIsInherited)
				{
                    int? hierarchyRID_Nullable = null;
                    if (hierarchyRID != Include.NoRID) hierarchyRID_Nullable = hierarchyRID;

                    int? levelSeq_Nullable = null;
                    if (levelSeq != Include.Undefined && levelSeq != 0) levelSeq_Nullable = levelSeq;

                    int? levelOffset_Nullable = null;
                    if (levelOffset != Include.Undefined) levelOffset_Nullable = levelOffset;

                    int? dateRID_Nullable = null;
                    if (dateRID != Include.NoRID) dateRID_Nullable = dateRID;

                    int? OLLRID_Nullable = null;
                    if (OLLRID != Include.NoRID) OLLRID_Nullable = OLLRID;

                    int? customOLLRID_Nullable = null;
                    if (customOLLRID != Include.NoRID) customOLLRID_Nullable = customOLLRID;

                    int? sizeGroupRID_Nullable = null;
                    if (sizeGroupRID != Include.NoRID) sizeGroupRID_Nullable = sizeGroupRID;

                    int? strGrpRID_Nullable = null;
                    if (strGrpRID != Include.NoRID) strGrpRID_Nullable = strGrpRID;

                    StoredProcedures.SP_MID_ND_SZ_CRV_CRIT_UPD.Update(_dba, 
                                                                      HN_RID: nodeRID,
                                                                      NSCCD_RID: criteriaRID,
                                                                      PH_OFFSET_IND: (int)levelType,
                                                                      PH_RID: hierarchyRID_Nullable,
                                                                      PHL_SEQUENCE: levelSeq_Nullable,
                                                                      PHL_OFFSET: levelOffset_Nullable,
                                                                      CDR_RID: dateRID_Nullable,
                                                                      APPLY_LOST_SALES_IND: (applyLostSales ? '1' : '0'),
                                                                      OLL_RID: OLLRID_Nullable,
                                                                      CUSTOM_OLL_RID: customOLLRID_Nullable,
                                                                      SIZE_GROUP_RID: sizeGroupRID_Nullable,
                                                                      CURVE_NAME: curveName,
                                                                      SG_RID: strGrpRID_Nullable
                                                                      );
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveDefaultCriteria_Update(int nodeRID, int defaultRID)
		{
			try
			{
                int? defaultRID_Nullable = null;
                if (defaultRID != Include.NoRID) defaultRID_Nullable = defaultRID;

                StoredProcedures.SP_MID_ND_SZ_CRV_CRIT_DEF_UPD.Update(_dba, 
                                                                      HN_RID: nodeRID,
                                                                      NSCCD_RID: defaultRID_Nullable
                                                                      );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveCriteria_Delete(int nodeRID, int criteriaRID)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_CRV_CRIT_DEL.Delete(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  NSCCD_RID: criteriaRID
                                                                  );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveCriteria_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_CRV_CRIT_DELALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SizeCurveTolerance_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_NODE_SIZE_CURVE_TOLERANCE_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveTolerance_Add(int nodeRID, double minimumAvg, eLowLevelsType levelType, int hierarchyRID,
            int levelSeq, int levelOffset, double salesTolerance, eNodeChainSalesType indexUnitsInd, double minTolerance, double maxTolerance, bool applyMinToZeroTolerance)
		{
			try
			{
               
                int? hierarchyRID_Nullable = null;
                if (hierarchyRID != Include.NoRID) hierarchyRID_Nullable = hierarchyRID;

                int? levelSeq_Nullable = null;
                if (levelSeq != Include.Undefined) levelSeq_Nullable = levelSeq;

                int? levelOffset_Nullable = null;
                if (levelOffset != Include.Undefined) levelOffset_Nullable = levelOffset;

                StoredProcedures.SP_MID_ND_SZ_CRV_TOLER_INS.Insert(_dba, 
                                                                   HN_RID: nodeRID,
                                                                   MINIMUM_AVERAGE: minimumAvg,
                                                                   PH_OFFSET_IND: (int)levelType,
                                                                   PH_RID: hierarchyRID_Nullable,
                                                                   PHL_SEQUENCE: levelSeq_Nullable,
                                                                   PHL_OFFSET: levelOffset_Nullable,
                                                                   SALES_TOLERANCE: salesTolerance,
                                                                   INDEX_UNITS_TYPE: (int)indexUnitsInd,
                                                                   MIN_TOLERANCE: minTolerance,
                                                                   MAX_TOLERANCE: maxTolerance,
                                                                   APPLY_MIN_TO_ZERO_TOLERANCE_IND: Include.ConvertBoolToChar(applyMinToZeroTolerance)
                                                                   );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveTolerance_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SizeCurveSimilarStore_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveSimilarStore_Add(int nodeRID, int storeRID, int simStoreRID, int untilDate)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_CRV_SIMSTR_INS.Insert(_dba, 
                                                                    HN_RID: nodeRID,
                                                                    ST_RID: storeRID,
                                                                    SS_RID: simStoreRID,
                                                                    UNTIL_DATE: untilDate
                                                                    );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveSimilarStore_Update(int nodeRID, int storeRID, int simStoreRID, int untilDate)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_CRV_SIMSTR_UPD.Update(_dba, 
                                                                    HN_RID: nodeRID,
                                                                    ST_RID: storeRID,
                                                                    SS_RID: simStoreRID,
                                                                    UNTIL_DATE: untilDate
                                                                    );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveSimilarStore_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveSimilarStore_Delete(int nodeRID, int storeRID)
		{
			try
			{
                StoredProcedures.MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE.Delete(_dba, 
                                                                                 HN_RID: nodeRID,
                                                                                 ST_RID: storeRID
                                                                                 );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeOutOfStock_DeleteAll(int aNodeRID)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_OOS_DEL_ALL.Delete(_dba, HN_RID: aNodeRID);
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}

		public DataTable SizeOutOfStockHeader_Get(int aNodeRID)
		{
			try
			{
                return StoredProcedures.SP_MID_ND_SZ_OOS_HEADER_GET.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}

        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //public DataSet SizeOutOfStockValuesForNodeProperties_Get(int aNodeRID, int aStrGrpRID, int aSzGrpRID)
        public DataSet SizeOutOfStockValuesForNodeProperties_Get(int aNodeRID, int aStrGrpRID, int aSzGrpRID, int sg_Version)
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		{
			try
			{
                return StoredProcedures.SP_MID_ND_SZ_OOS_VALUES_GET.ReadAsDataSet(_dba, 
                                                                                   HN_RID: aNodeRID,
                                                                                   SG_RID: aStrGrpRID,
                                                                                   SIZE_GROUP_RID: aSzGrpRID,
                                                                                   FOR_NODE_PROPERTIES: 1,
                                                                                   SG_VERSION: sg_Version  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                                                                   );
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}

		public void SizeOutOfStock_Add(int aNodeRID, int aStrGrpRID, int aSzGrpRID)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_OOS_INS.Insert(_dba, 
                                                             HN_RID: aNodeRID,
                                                             SG_RID: aStrGrpRID,
                                                             SIZE_GROUP_RID: aSzGrpRID
                                                             );
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}

		public void SizeOutOfStockGroupLevel_Add(int aNodeRID, DataTable dtOOSGrpLvl)
		{
			try
			{
				foreach (DataRow row in dtOOSGrpLvl.Rows)
				{
					if (row["OOS_QUANTITY"] != DBNull.Value && Convert.ToInt32(row["OOS_QUANTITY"]) != 0 && Convert.ToInt32(row["IS_INHERITED"]) == 0)
					{
                        StoredProcedures.SP_MID_ND_SZ_OOS_GRPLVL_INS.Insert(_dba, 
                                                                    HN_RID: aNodeRID,
                                                                    SGL_RID: Convert.ToInt32(row["SGL_RID"]),
                                                                    ROW_TYPE_ID: Convert.ToInt32(row["ROW_TYPE_ID"]),
                                                                    OOS_QUANTITY: Convert.ToInt32(row["OOS_QUANTITY"])
                                                                    );
					}
				}
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}

		public void SizeOutOfStockQuantity_Add(int aNodeRID, DataTable dtOOSQuantity)
		{
			try
			{
				foreach (DataRow row in dtOOSQuantity.Rows)
				{
					if (row["OOS_QUANTITY"] != DBNull.Value && Convert.ToInt32(row["OOS_QUANTITY"]) != 0 && Convert.ToInt32(row["IS_INHERITED"]) == 0)
					{
                        StoredProcedures.SP_MID_ND_SZ_OOS_QUANTITY_INS.Insert(_dba, 
                                                                              HN_RID: aNodeRID,
                                                                              SGL_RID: Convert.ToInt32(row["SGL_RID"]),
                                                                              COLOR_CODE_RID: Convert.ToInt32(row["COLOR_CODE_RID"]),
                                                                              SIZES_RID: Convert.ToInt32(row["SIZES_RID"]),
                                                                              DIMENSIONS_RID: Convert.ToInt32(row["DIMENSIONS_RID"]),
                                                                              ROW_TYPE_ID: Convert.ToInt32(row["ROW_TYPE_ID"]),
                                                                              SIZE_CODE_RID: Convert.ToInt32(row["SIZE_CODE_RID"]),
                                                                              OOS_QUANTITY: Convert.ToInt32(row["OOS_QUANTITY"])
                                                                              );
					}
				}
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}

		public void SizeSellThru_DeleteAll(int aNodeKey)
		{
			try
			{
                StoredProcedures.MID_NODE_SIZE_SELLTHRU_DELETE_ALL.Delete(_dba, HN_RID: aNodeKey);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SizeSellThru_Get(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_NODE_SIZE_SELLTHRU_READ.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataSet SizeDayToWeekSummary(int aNodeRID, int startSQLTimeID, int endSQLTimeID, int storeRID=-1)
        {
            return StoredProcedures.MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE.ReadAsDataSet(_dba, 
                                                                                                              START_TIME: startSQLTimeID,
                                                                                                              END_TIME: endSQLTimeID,
                                                                                                              SELECTED_NODE_RID: aNodeRID,
                                                                                                              STORE_RID: storeRID
                                                                                                              );
        }

		public void SizeSellThru_Add(int aNodeRID, double aSellThruLimit)
		{
			try
			{
                StoredProcedures.SP_MID_ND_SZ_ST_INS.Insert(_dba, 
                                                            HN_RID: aNodeRID,
                                                            SELLTHRU_LIMIT: aSellThruLimit
                                                            );
			}
			catch (Exception e)
			{
				string exceptionMessage = e.Message;
				throw;
			}
		}



        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings
        public DataTable SizeCurveNames_Read()
        {
            try
            {
                return StoredProcedures.MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable SizeCurveName_Read(int aNsccdRID)
        {
            try
            {
                return StoredProcedures.MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME.Read(_dba, NSCCD_RID: aNsccdRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#413 

		public DataTable StoreCapacity_Read(int nodeRID)
		{
			try
			{
                return StoredProcedures.MID_STORE_CAPACITY_READ.Read(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreCapacity_Add(int nodeRID, int storeRID, int capacity)
		{
			try
			{
                int? capacity_Nullable = null;
                if (capacity != Include.Undefined) capacity_Nullable = capacity;

                StoredProcedures.MID_STORE_CAPACITY_INSERT.Insert(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  ST_RID: storeRID,
                                                                  ST_CAPACITY: capacity_Nullable
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreCapacity_DeleteAll(int nodeRID)
		{
			try
			{
                StoredProcedures.MID_STORE_CAPACITY_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreCapacity_Delete(int nodeRID, int storeRID)
		{
			try
			{
                StoredProcedures.MID_STORE_CAPACITY_DELETE.Delete(_dba, 
                                                                  HN_RID: nodeRID,
                                                                  ST_RID: storeRID
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public ArrayList GetHierarchyXref(int aHierarchyRID)
		{
			try
			{
				ArrayList xref = new ArrayList();
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_HIERARCHY_READ_XREF.Read(_dba, PH_RID: aHierarchyRID);

				foreach(DataRow dr in dt.Rows)
				{
					xref.Add(Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture));
				}
				return xref;
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetColorIDs(int aRowsToRead)
		{
			try
			{
                return StoredProcedures.MID_COLOR_NODE_READ_TOP.Read(_dba, ROWS_TO_RETURN: aRowsToRead);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetSizeIDs(int aRowsToRead)
		{
			try
			{
                return StoredProcedures.MID_SIZE_NODE_READ_TOP.Read(_dba, ROWS_TO_RETURN: aRowsToRead);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetColorIDs(string aStyleNodeID)
		{
			try
			{
                return StoredProcedures.MID_COLOR_NODE_READ_FROM_STYLE.Read(_dba, STYLE_NODE_ID: aStyleNodeID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetSizeIDs(string aStyleNodeID, string aColorNodeID)
		{
			try
			{
                return StoredProcedures.MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR.Read(_dba, 
                                                                                         STYLE_NODE_ID: aStyleNodeID,
                                                                                         COLOR_NODE_ID: aColorNodeID
                                                                                         );
			}
			catch
			{
				throw;
			}
		}

		public void RenameStyleIDOnIDs(string aOldID, string aNewID)
		{
			try
			{
                StoredProcedures.MID_COLOR_NODE_UPDATE_STYLE_ID.Update(_dba, 
                                                                       NEW_STYLE_NODE_ID: aNewID,
                                                                       OLD_STYLE_NODE_ID: aOldID
                                                                       );

                StoredProcedures.MID_SIZE_NODE_UPDATE_STYLE_ID.Update(_dba, 
                                                                      NEW_STYLE_NODE_ID: aNewID,
                                                                      OLD_STYLE_NODE_ID: aOldID
                                                                      );
			}
			catch (Exception ex)
			{
                string message = ex.ToString();
				throw;
			}
		}
      
		public void DropColorSizeIndexes()
		{
			try
			{
                StoredProcedures.MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX.Execute(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void BuildColorSizeIDs()
		{
			try
			{
                StoredProcedures.SP_MID_BUILD_COLOR_SIZE_IDS.Update(_dba);
			}
			catch (Exception ex)
			{
                string message = ex.ToString();
				throw;
			}
		}

		public void BuildColorSizeIndexes()
		{
			try
			{
                StoredProcedures.MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX.Execute(_dba);
			}
			catch (Exception ex)
			{
                string message = ex.ToString();
				throw;
			}
		}

		public void NodeLookup_XMLInit()
		{
			try
			{
				_nodeIDs = 0;
				_colorIDs = 0;
				_sizeIDs = 0;
				
				XmlDeclaration xmlDeclaration;
				XmlNode rootNode;
				
				_nodeDoc = new XmlDocument();
				// XML declaration
				xmlDeclaration = _nodeDoc.CreateXmlDeclaration("1.0",null,null); 
				// Create the root element
				rootNode  = _nodeDoc.CreateElement("root");
				_nodeDoc.InsertBefore(xmlDeclaration, _nodeDoc.DocumentElement); 
				_nodeDoc.AppendChild(rootNode);

				_colorDoc = new XmlDocument();
				// XML declaration
				xmlDeclaration = _colorDoc.CreateXmlDeclaration("1.0",null,null); 
				// Create the root element
				rootNode  = _colorDoc.CreateElement("root");
				_colorDoc.InsertBefore(xmlDeclaration, _colorDoc.DocumentElement); 
				_colorDoc.AppendChild(rootNode);

				_sizeDoc = new XmlDocument();
				// XML declaration
				xmlDeclaration = _sizeDoc.CreateXmlDeclaration("1.0",null,null); 
				// Create the root element
				rootNode  = _sizeDoc.CreateElement("root");
				_sizeDoc.InsertBefore(xmlDeclaration, _sizeDoc.DocumentElement); 
				_sizeDoc.AppendChild(rootNode);
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		public void Node_XMLInsert(string aID, char aDelimiter)
		{
			try
			{
				string[] fields = MIDstringTools.Split(aID,aDelimiter,true);
				if (fields.Length == 2)
				{
					ColorNode_XMLInsert(aID, fields[0], fields[1]);
				}
				else if (fields.Length == 3)
				{
					SizeNode_XMLInsert(aID, fields[0], fields[1], fields[2]);
				}
				else 
				{
					Node_XMLInsert(aID);
				}

				return;
			}
			catch 
			{
				throw;
			}
		}

		private void Node_XMLInsert(string aID)
		{
			try
			{
				++_nodeIDs;

				XmlElement newNode = _nodeDoc.CreateElement("node");
				XmlAttribute idAttribute = _nodeDoc.CreateAttribute("ID");
				idAttribute.Value = aID;
				newNode.SetAttributeNode(idAttribute);
				_nodeDoc.DocumentElement.AppendChild(newNode);

				return;
			}
			catch 
			{
				throw;
			}
		}

		private void ColorNode_XMLInsert(string aID, string aStyleID, string aColorID)
		{
			try
			{
				++_colorIDs;

				XmlElement newNode = _colorDoc.CreateElement("node");
				XmlAttribute idAttribute = _colorDoc.CreateAttribute("ID");
				idAttribute.Value = aID;
				newNode.SetAttributeNode(idAttribute);
				idAttribute = _colorDoc.CreateAttribute("STYLE_ID");
				idAttribute.Value = aStyleID;
				newNode.SetAttributeNode(idAttribute);
				idAttribute = _colorDoc.CreateAttribute("COLOR_ID");
				idAttribute.Value = aColorID;
				newNode.SetAttributeNode(idAttribute);
				_colorDoc.DocumentElement.AppendChild(newNode);

				return;
			}
			catch 
			{
				throw;
			}
		}

		private void SizeNode_XMLInsert(string aID, string aStyleID, string aColorID, string aSizeID)
		{
			try
			{
				++_sizeIDs;

				XmlElement newNode = _sizeDoc.CreateElement("node");
				XmlAttribute idAttribute = _sizeDoc.CreateAttribute("ID");
				idAttribute.Value = aID;
				newNode.SetAttributeNode(idAttribute);
				idAttribute = _sizeDoc.CreateAttribute("STYLE_ID");
				idAttribute.Value = aStyleID;
				newNode.SetAttributeNode(idAttribute);
				idAttribute = _sizeDoc.CreateAttribute("COLOR_ID");
				idAttribute.Value = aColorID;
				newNode.SetAttributeNode(idAttribute);
				idAttribute = _sizeDoc.CreateAttribute("SIZE_ID");
				idAttribute.Value = aSizeID;
				newNode.SetAttributeNode(idAttribute);
				_sizeDoc.DocumentElement.InsertAfter(newNode, _sizeDoc.DocumentElement.LastChild);

				return;
			}
			catch 
			{
				throw;
			}
		}

		public DataTable GetNodeRIDs()
		{
			try
			{
				DataTable dt = MIDEnvironment.CreateDataTable();

				if (_nodeIDs > 0)
				{
					StringWriter sw = new StringWriter();
					XmlTextWriter xw = new XmlTextWriter(sw);
					_nodeDoc.WriteTo(xw);
                    dt = StoredProcedures.SP_MID_GET_NODE_RIDS.Read(_dba, xmlDoc: sw.ToString());
				}
				return dt;
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		public DataTable GetColorRIDs()
		{
			try
			{
				DataTable dt = MIDEnvironment.CreateDataTable();

				if (_colorIDs > 0)
				{
					StringWriter sw = new StringWriter();
					XmlTextWriter xw = new XmlTextWriter(sw);
					_colorDoc.WriteTo(xw);
                    dt = StoredProcedures.SP_MID_GET_COLOR_RIDS.Read(_dba, xmlDoc: sw.ToString());
				}
				return dt;
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		public DataTable GetSizeRIDs()
		{
			try
			{
				DataTable dt = MIDEnvironment.CreateDataTable();

				if (_sizeIDs > 0)
				{
					StringWriter sw = new StringWriter();
					XmlTextWriter xw = new XmlTextWriter(sw);
					_sizeDoc.WriteTo(xw);
                    dt = StoredProcedures.SP_MID_GET_SIZE_RIDS.Read(_dba, xmlDoc: sw.ToString());
				}
				return dt;
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
        }

        // Begin TT#1549, TT#1399 - Urban Hierarchy - added a value to the method call
		public DataTable GetAncestors(int aNodeRID, int aHierarchyRID, eHierarchySearchType aHierarchySearchType, bool UseApplyFrom)
		{
			try
			{
                if (UseApplyFrom == true)
                {
                    return StoredProcedures.SP_MID_GET_ANCESTORS_WITH_APPLY_FROM.Read(_dba, 
                                                                                      HN_RID: aNodeRID,
                                                                                      PH_RID: aHierarchyRID,
                                                                                      HierarchySearchType: Convert.ToInt32(aHierarchySearchType)
                                                                                      );
                }
                else
                {
                    return StoredProcedures.SP_MID_GET_ANCESTORS.Read(_dba, 
                                                                      HN_RID: aNodeRID,
                                                                      PH_RID: aHierarchyRID,
                                                                      HierarchySearchType: Convert.ToInt32(aHierarchySearchType)
                                                                      );
                }
            }
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		//Begin Track #4358 - JSmith - Performance opening alternate node
		public int GetHighestGuestLevel(int aNodeRID)
		{
			//_dba.OpenReadConnection();
			try
			{
                int highestGuestLevel = -1;
                int homePathLength = -1;
                StoredProcedures.SP_MID_GET_HIGHEST_GUEST_LEVEL.GetOutput(_dba, ref highestGuestLevel, ref homePathLength, HN_RID: aNodeRID);
                //int highestGuestLevel = Convert.ToInt32(StoredProcedures.SP_MID_GET_HIGHEST_GUEST_LEVEL.GUEST_LEVEL.Value);
                //int homePathLength = Convert.ToInt32(StoredProcedures.SP_MID_GET_HIGHEST_GUEST_LEVEL.HOME_PATH_LENGTH.Value);

				return highestGuestLevel;
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
			finally
			{
				//_dba.CloseReadConnection();
			}
		}
		
		//BEGIN TT#4650 - DOConnell - Changes do not hold
        public DataTable GetHierarchyDescendantLevels(int aNodeRID)
        {
            //_dba.OpenReadConnection();
            try
            {
                return StoredProcedures.SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS.Read(_dba, HN_RID: aNodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                // _dba.CloseReadConnection();
            }
        }
		//END TT#4650 - DOConnell - Changes do not hold
		
        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
        public DataTable GetAllGuestLevels(int aNodeRID)
        {
            //_dba.OpenReadConnection();
            try
            {
                return StoredProcedures.SP_MID_GET_ALL_GUEST_LEVELS.Read(_dba, HN_RID: aNodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
               // _dba.CloseReadConnection();
            }
        }
        //End Track #5960

        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
		public int GetBranchSize(int aNodeRID, bool aHomeHierarchyOnly)
        //End Track #5960
		{
			//_dba.OpenReadConnection();
			try
			{
                return StoredProcedures.SP_MID_GET_BRANCH_SIZE.GetOutput(_dba, HN_RID: aNodeRID, HOME_HIERARCHY_ONLY: Include.ConvertBoolToChar(aHomeHierarchyOnly));
                //int branchSize = Convert.ToInt32(StoredProcedures.SP_MID_GET_BRANCH_SIZE.BRANCH_SIZE.Value);
                //return branchSize;
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
			finally
			{
				//_dba.CloseReadConnection();
			}
		}
		//End Track #4358

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		/// <summary>
		/// Retrieves a list of nodes to be used to read IMO
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// A DataTable containing the node information
		/// </returns>
		public DataTable GetReadIMONodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
                return StoredProcedures.SP_MID_GET_IMO_READ_NODES.Read(_dba, 
                                                                       HN_RID: aNodeRID,
                                                                       LEVEL_TYPE: (int)aLevelType
                                                                       );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

		//Begin Track #4362 - JSmith - Intransit read performance
		/// <summary>
		/// Retrieves a list of nodes to be used to read intransit
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// A DataTable containing the node information
		/// </returns>
		public DataTable GetReadIntransitNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
                return StoredProcedures.SP_MID_GET_INTRANSIT_READ_NODES.Read(_dba, 
                                                                             HN_RID: aNodeRID,
                                                                             LEVEL_TYPE: (int)aLevelType
                                                                             );
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of descendant nodes of a given type
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// A DataTable containing the node information
		/// </returns>
		public DataTable GetDescendantsByType(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
                return StoredProcedures.SP_MID_GET_DESCENDANTS_BY_TYPE.Read(_dba, 
                                                                            HN_RID: aNodeRID,
                                                                            LEVEL_TYPE: (int)aLevelType
                                                                            );
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of descendant nodes at a given offset
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aOffset">The level offset of the node to retrieve</param>
		/// <returns>
		/// A DataTable containing the node information
		/// </returns>
		public DataTable GetDescendantsByOffset(int aNodeRID, int aOffset)
		{
			try
			{
                return StoredProcedures.SP_MID_GET_DESCENDANTS_BY_OFFSET.Read(_dba, 
                                                                              HN_RID: aNodeRID,
                                                                              LEVEL_OFFSET: aOffset
                                                                              );
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of descendant nodes at a given level in the hierarchy
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelSequence">The level sequence of the hierarchy to retrieve</param>
		/// <returns>
		/// A DataTable containing the node information
		/// </returns>
		public DataTable GetDescendantsByLevel(int aNodeRID, int aLevelSequence)
		{
			try
			{
                return StoredProcedures.SP_MID_GET_DESCENDANTS_BY_LEVEL.Read(_dba, 
                                                                             HN_RID: aNodeRID,
                                                                             LEVEL_SEQ: aLevelSequence
                                                                             );
			}
			catch ( Exception err )
			{
                string message = err.ToString();
				throw;
			}
		}
		//End Track #4362

        //Begin Track #5004 - JSmith - Global Unlock
        /// <summary>
        /// Retrieves a list of descendant nodes at a given level in the hierarchy
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aFromOffset">The from level offset of the node to retrieve</param>
        /// <param name="aToOffset">The from level offset of the node to retrieve</param>
        /// <returns>
        /// A DataTable containing the node information
        /// </returns>
        public DataTable GetDescendantsByRange(int aNodeRID, int aFromOffset, int aToOffset)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_DESCENDANTS_BY_RANGE.Read(_dba, 
                                                                             HN_RID: aNodeRID,
                                                                             LEVEL_FROM_OFFSET: aFromOffset,
                                                                             LEVEL_TO_OFFSET: aToOffset
                                                                             );
            }
            catch
            {
                throw;
            }
        }
        //End Track #5004

		public bool StockMinMaxes_DeleteAll(int aNodeRID)
		{
			bool DeleteSuccessfull = true;
			try
			{
                StoredProcedures.MID_NODE_STOCK_MIN_MAX_DELETE_ALL.Delete(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			return DeleteSuccessfull;
		}

		/// <summary>
		/// Save Stock Min Max based on METHOD_RID, SGL_RID, and BOUNDARY
		/// </summary>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool StockMinMaxes_Add(int aNodeRID, int aGroupLevelRID, int aBoundary, int aCalendarDateRange,
			int aMinimum, int aMaximum)
		{
			bool InsertSuccessfull = true;
			try
			{
                int? aMinimum_Nullable = null;
                if (aMinimum != int.MinValue) aMinimum_Nullable = aMinimum;
                int? aMaximum_Nullable = null;
                if (aMaximum != int.MaxValue) aMaximum_Nullable = aMaximum;

                StoredProcedures.MID_NODE_STOCK_MIN_MAX_INSERT.Insert(_dba, 
                                                                      HN_RID: aNodeRID,
                                                                      SGL_RID: aGroupLevelRID,
                                                                      BOUNDARY: aBoundary,
                                                                      CDR_RID: aCalendarDateRange,
                                                                      MIN_STOCK: aMinimum_Nullable,
                                                                      MAX_STOCK: aMaximum_Nullable
                                                                      );
                
				InsertSuccessfull = true;
			}
			catch
			{
				InsertSuccessfull = false;
				throw;
			}
			return InsertSuccessfull;
		}

		public DataTable StockMinMaxes_Read(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_NODE_STOCK_MIN_MAX_READ.Read(_dba, HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

       

		/// <summary>
		/// Update the Group RID used for stock min/maxes
		/// </summary>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool StockMinMaxes_GroupRIDUpdate(int aNodeRID, int aStoreGroupRID)
		{
			bool InsertSuccessfull = true;
			try
			{
                int? aStoreGroupRID_Nullable = null;
                if (aStoreGroupRID != Include.NoRID) aStoreGroupRID_Nullable = aStoreGroupRID;

                StoredProcedures.MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP.Update(_dba, 
                                                                                      HN_RID: aNodeRID,
                                                                                      SG_RID: aStoreGroupRID_Nullable
                                                                                      );

				InsertSuccessfull = true;
			}
			catch
			{
				InsertSuccessfull = false;
				throw;
			}
			return InsertSuccessfull;
		}


        public int DetermineHierarchyActivity(int aFromOffset, int aToOffset, int aForecastOffset, int aIntransitOffset,int aTable)
        {
            try
            {
                int returnCode = StoredProcedures.SP_MID_SET_NODE_ACTIVITY.UpdateWithReturnCode(_dba, 
                                                                                                fromOffset: aFromOffset,
                                                                                                toOffset: (int)aToOffset,
                                                                                                intransitOffset: aIntransitOffset,
                                                                                                forecastOffset: aForecastOffset,
                                                                                                table: aTable
                                                                                                );

                return returnCode;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


        public void SetHierarchyActivityFalse()
        {
            try
            {
                StoredProcedures.MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR.Update(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int GetTotalNodeCount()
        {
            try
            {
                return StoredProcedures.MID_HIERARCHY_READ_COUNT.ReadRecordCount(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int GetActiveNodeCount()
        {
            try
            {
                return StoredProcedures.MID_HIERARCHY_READ_ACTIVE_COUNT.ReadRecordCount(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int GetInactiveNodeCount()
        {
            try
            {
                return StoredProcedures.MID_HIERARCHY_READ_INACTIVE_COUNT.ReadRecordCount(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#1873 - JSmith - Purge Failed with Severe Error
        public bool HierarchyInUseByHeader(int aPhRID)
        {
            bool openedConnection = false;
            if (!ConnectionIsOpen)
            {
                _dba.OpenUpdateConnection();
            }
            try
            {
                int inUseByHeader = StoredProcedures.SP_MID_HIERARCHY_IN_USE_BY_HEADER.UpdateWithReturnCode(_dba, PH_RID: aPhRID);
                return Include.ConvertIntToBool(inUseByHeader);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (openedConnection)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }
        // End TT#1873
		
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        public DataTable ChainSetPercentSet_Read(int nodeRID, int beg_Week, int end_Week)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_SET_PERCENT_SET_READ.Read(_dba, 
                                                                            HN_RID: nodeRID,
                                                                            BEGIN_WEEK: beg_Week,
                                                                            END_WEEK: end_Week
                                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //public int ChainSetPercentSet_Add(int nodeRID, string storeAttribute, string storeAttributeSet, int timeID, decimal percent, ChainSetPercentCriteriaData _cspcd)
        public int ChainSetPercentSet_Add(int nodeRID, string storeAttribute, string storeAttributeSet, int timeID, decimal percent, ChainSetPercentCriteriaData _cspcd, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        {
            int insertReturn =  0;
            
            try
            {
                // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                //insertReturn = _cspcd.ChainSetPercentCriteria_Insert(timeID, nodeRID, storeAttribute, storeAttributeSet, percent);
                insertReturn = _cspcd.ChainSetPercentCriteria_Insert(timeID, nodeRID, storeAttribute, storeAttributeSet, percent, sg_Version);
                // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.

                return insertReturn;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void ChainSetPercentSet_DeleteAll(int nodeRID)
        {
            try
            {
                StoredProcedures.MID_CHAIN_SET_PERCENT_SET_DELETE_ALL.Delete(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int ChainSetPercentSet_DeleteAll(int nodeRID, int timeID)
        {
            int deleteReturn = -1;
            try
            {
                StoredProcedures.MID_CHAIN_SET_PERCENT_SET_DELETE.Delete(_dba, 
                                                                         HN_RID: nodeRID,
                                                                         FISCAL_WEEK: timeID
                                                                         );
            }
            catch (Exception err)
            {
                deleteReturn = 5;
                string message = err.ToString();
                throw;
            }
            return deleteReturn;
        }

        //Begin TT#1671 - DOConnell - Percentages not updating correctly 
        public int ChainSetPercentSet_Delete(int nodeRID, int timeID, int SGL_RID)
        {
            int deleteReturn = -1;
            try
            {
                StoredProcedures.MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP.Delete(_dba, 
                                                                                         HN_RID: nodeRID,
                                                                                         SGL_RID: SGL_RID,
                                                                                         FISCAL_WEEK: timeID
                                                                                         );
            }
            catch (Exception err)
            {
                deleteReturn = 1;
                string message = err.ToString();
                throw;
                
            }

            return deleteReturn;
        }
        //End TT#1671 - DOConnell - Percentages not updating correctly 

        // Begin TT#1700 - JSmith - Chain Set Percentages need to retain last settings in Node Properties
        public void ChainSetPercentUser_Update(int aUserRID, int aCDR_RID, int aSG_RID)
        {
            try
            {
                StoredProcedures.MID_CHAIN_SET_PERCENT_USER_UPSERT.Update(_dba, 
                                                                          USER_RID: aUserRID,
                                                                          SG_RID: aSG_RID,
                                                                          CDR_RID: aCDR_RID
                                                                          );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable ChainSetPercentUser_Read(int aUserRID)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_SET_PERCENT_USER_READ.Read(_dba, USER_RID: aUserRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1700

     

        // BEGIN TT#2015 - gtaylor - Apply Changes To Lower Levels
        //  get total number of descendants based on nodeRID
        public int GetTotalNumberDescendants(int nodeRID)
        {
            try
            {
                return ((DataTable)GetAllDescendants(nodeRID)).Rows.Count;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //  get all descendants for a nodeRID
        public DataTable GetAllDescendants(int nodeRID)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_ALL_DESCENDANTS.Read(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int GetTotalAffectedDescendants(int nodeRID, int IMO, int SE, int CHR, int SC, int DP, int PC, int CSP)
        {
            try
            {
                return ((DataTable)GetAllAffectedDescendants(nodeRID, IMO, SE, CHR, SC, DP, PC, CSP)).Rows.Count;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable GetAllAffectedDescendants(int nodeRID, int IMO, int SE, int CHR, int SC, int DP, int PC, int CSP)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_ALL_AFFECTED_DESCENDANTS.Read(_dba,
                                                                                   HN_RID: nodeRID,
                                                                                   IMO: IMO,
                                                                                   SE: SE,
                                                                                   CHAR: CHR,
                                                                                   SC: SC,
                                                                                   DP: DP,
                                                                                   PC: PC,
                                                                                   CSP: CSP
                                                                                   );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //  get locked ancestors for a nodeRID
        public DataTable GetLockedAncestors(int nodeRID)
        {
            try
            {
                return StoredProcedures.SP_MID_ANY_ANCESTORS_LOCKED.Read(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //  get locked descendants for a nodeRID
        public DataTable GetLockedDescendants(int nodeRID)
        {
            try
            {
                return StoredProcedures.SP_MID_ANY_DESCENDANTS_LOCKED.Read(_dba, HN_RID: nodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //  are any ancestors locked?
        public bool AnyAncestorsLocked(int nodeRID)
        {
            try
            {
                return (((DataTable)GetLockedAncestors(nodeRID)).Rows.Count > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //  are any descendants locked?
        public bool AnyDescendantsLocked(int nodeRID)
        {
            try
            {
                return (((DataTable)GetLockedDescendants(nodeRID)).Rows.Count > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int UpdateColumnByTable(int HN_RID, int ST_RID, string COLUMNNAME, string TABLENAME, int COLVALUE)
        {
            try
            {
                return StoredProcedures.SP_MID_UPDATE_COLUMN_BY_TABLE.UpdateWithReturnCode(_dba,
                                                                     HN_RID: HN_RID,
                                                                     ST_RID: ST_RID,
                                                                     COLUMNNAME: COLUMNNAME,
                                                                     TABLENAME: TABLENAME,
                                                                     COLVALUE: COLVALUE
                                                                     );

                //int err = (int)StoredProcedures.SP_MID_UPDATE_COLUMN_BY_TABLE.ReturnCode.Value;
                //return err;
            }
            catch
            {
                throw;
            }
        }

        public int DeleteFromTableWithColumnName(int RID, string COLUMNAME, string TABLENAME)
        {
            try
            {
                //  delete rows in database to force inheritence
                return StoredProcedures.SP_MID_DELETE_HIER_BY_COL.DeleteWithReturnCode(_dba,
                                                                                        RID: RID,
                                                                                        COLUMNAME: COLUMNAME,
                                                                                        TABLENAME: TABLENAME
                                                                                        );
                //int err = (int)StoredProcedures.SP_MID_DELETE_HIER_BY_COL.ReturnCode.Value;
                //return err;
            }
            catch
            {
                throw;
            }
        }

        public int DeleteFromTableWithStoreRID(int HN_RID, int ST_RID, string TABLENAME)
        {
            try
            {
                //  delete rows in database to force inheritence
                return StoredProcedures.SP_MID_DELETE_HIER_FROM_TABLE.DeleteWithReturnCode(_dba,
                                                                     HN_RID: HN_RID,
                                                                     ST_RID: ST_RID,
                                                                     TABLENAME: TABLENAME
                                                                     );
                //int err = (int)StoredProcedures.SP_MID_DELETE_HIER_FROM_TABLE.ReturnCode.Value;
                //return err;
            }
            catch
            {
                throw;
            }
        }

        //  this accepts timeID as the value similar to what is in CHAIN_SET_PERCENT_SET not FISCAL_WEEK
        public int DeleteCSPSet(int nodeRID, int SGL_RID, int timeID)
        {
            int deleteReturn = -1;
            try
            {
                deleteReturn = StoredProcedures.SP_MID_CHAIN_SET_PCT_SET_WK_DELETE.DeleteWithReturnCode(_dba,
                                                                        NODE_RID: nodeRID,
                                                                        YEAR_WEEK: timeID
                                                                        );
                //deleteReturn = (int)StoredProcedures.SP_MID_CHAIN_SET_PCT_SET_WK_DELETE.RETURN.Value;
            }
            catch (Exception err)
            {
                deleteReturn = 1;
                string message = err.ToString();
                throw;

            }
            return deleteReturn;
        }
        // END TT#2015
		
        // Begin TT#2231 - JSmith - Size curve build failing
        public DataTable GetStyleColorSizes(int aNodeRID)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_STYLE_COLOR_SIZES.Read(_dba, HN_RID: aNodeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2231

        public void Hierarchy_Node_MarkForDelete(int aNodeRID, bool aDeleteInd, string aNodeID)
        {
            try
            {
                StoredProcedures.MID_MARK_NODE_DELETED.Update(_dba,
                                                                     HN_RID: aNodeRID,
                                                                     NODE_DELETE_IND: Include.ConvertBoolToChar(aDeleteInd),
                                                                     NODE_ID: aNodeID
                                                                     );

            }
            catch
            {
                throw;
            }
        }

        public DataTable Hierarchy_Node_GetDeletedNodes()
        {
            try
            {
                return StoredProcedures.MID_GET_DELETED_HIERARCHY_NODES.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int GetHierarchyMaxNodeLevel(int aHierarchyRID)
        {
            try
            {
                return (int)StoredProcedures.MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL.ReadValue(_dba, HOME_PH_RID: aHierarchyRID); //TT#1403-MD -jsobek -Data Layer Request - Need access to stored procedure
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels

        public int GetHierarchyRootNodeRID(int aHierarchyRID)
        {
            try
            {
                return (int)StoredProcedures.MID_HIERARCHY_GET_ROOT_NODE_RID.ReadValue(_dba, PH_RID: aHierarchyRID); //TT#1403-MD -jsobek -Data Layer Request - Need access to stored procedure
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public int AddOrganizationalHierarchyLevel(string aLevelID, string aAfterLevel)
        {
            try
            {
                int returnCode = StoredProcedures.MID_HIERARCHY_ADD_ORG_LEVEL.UpdateWithReturnCode(_dba,
                                                                                                levelID: aLevelID,
                                                                                                afterLevel: aAfterLevel
                                                                                                );

                return returnCode;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int RemoveOrganizationalHierarchyLevel(string aLevelID)
        {
            try
            {
                int returnCode = StoredProcedures.MID_HIERARCHY_REMOVE_ORG_LEVEL.UpdateWithReturnCode(_dba,
                                                                                                levelID: aLevelID
                                                                                                );

                return returnCode;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int AddAlternateHierarchyLevel(int aHierarchyRID, int aAfterLevel)
        {
            try
            {
                int returnCode = StoredProcedures.MID_HIERARCHY_ADD_ALT_LEVEL.UpdateWithReturnCode(_dba,
                                                                                                PH_RID: aHierarchyRID,
                                                                                                afterLevel: aAfterLevel
                                                                                                );

                return returnCode;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int RemoveAlternateHierarchyLevel(int aHierarchyRID, int aLevel)
        {
            try
            {
                int returnCode = StoredProcedures.MID_HIERARCHY_REMOVE_ALT_LEVEL.UpdateWithReturnCode(_dba,
                                                                                                PH_RID: aHierarchyRID,
                                                                                                level: aLevel
                                                                                                );

                return returnCode;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void RemoveTemporaryNodes()
        {
            try
            {
                StoredProcedures.MID_HIERARCHY_TEMP_NODES_DELETE.Delete(_dba);

                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
	}
}
