using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class StoreGroupMaint : DataLayer
	{	


		public StoreGroupMaint() : base()
		{

		}

        public StoreGroupMaint(string aConnectionString)
			: base(aConnectionString)
		{

		}

        public int StoreGroup_InsertAndAddUserItem(string groupName, bool isDynamic, int aUserRID, int filterRID, int sgVersion)
        {
            try
            {
                int RID = -1;
                char strIsDynamic = Include.ConvertBoolToChar(isDynamic);

                RID = StoredProcedures.MID_STORE_GROUP_INSERT.InsertAndReturnRID(_dba,
                                                                                   SG_ID: groupName,
                                                                                   SG_DYNAMIC_GROUP_IND: strIsDynamic,
                                                                                   USER_RID: aUserRID,
                                                                                   FILTER_RID: filterRID,
                                                                                   SG_VERSION: sgVersion
                                                                                   );

                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(aUserRID, (int)eProfileType.StoreGroup, RID, aUserRID);
                }




                return RID;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool DoesStoreGroupNameAlreadyExist(string sgID, int userRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_GROUP_READ_FOR_DUPLICATE.Read(_dba,
                                                                                    SG_ID: sgID,
                                                                                    USER_RID: userRID);
            int matchCount = (int)dt.Rows[0]["MYCOUNT"];
            if (matchCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoesStoreGroupNameAlreadyExistForStoreCharGroup(string sgID, int userRID, int scgRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP.Read(_dba,
                                                                                    SG_ID: sgID,
                                                                                    USER_RID: userRID, 
                                                                                    SCG_RID: scgRID);
            int matchCount = (int)dt.Rows[0]["MYCOUNT"];
            if (matchCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int StoreGroup_ReadForStoreCharGroupRename(string sgID, int userRID, int scgRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_GROUP_READ_FOR_RENAME.Read(_dba,
                                                                                    SG_ID: sgID,
                                                                                    USER_RID: userRID,
                                                                                    SCG_RID: scgRID);
            if (dt.Rows.Count == 0)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0]["SG_RID"]);
            }
        }

        /// <summary>
        /// Return the number of Store Groups
        /// </summary>
        /// <returns></returns>
        public int StoreGroup_Count(bool bLoadInactiveGroups = false)
        {
            try
            {
                return StoredProcedures.MID_STORE_GROUP_COUNT.ReadRecordCount(_dba, LOAD_INACTIVE_GROUPS_IND: Include.ConvertBoolToChar(bLoadInactiveGroups));

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Reads Rows of Store Group data that is merged with the groups they own
        /// </summary>
        /// <param name="aDataOrderBy"></param>
        /// <returns></returns>
	    // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
		//public DataTable StoreGroup_Read(eDataOrderBy aDataOrderBy)
        public DataTable StoreGroup_Read(eDataOrderBy aDataOrderBy, bool bLoadInactiveGroups = false)
		// End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
        {
            try
            {
                ///MID Track # 2354 - removed nolock because it causes concurrency issues
                if (aDataOrderBy == eDataOrderBy.RID)
                {
				    // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
					//return StoredProcedures.MID_STORE_GROUP_READ_ALL_SORTED_BY_RID.Read(_dba);
                    return StoredProcedures.MID_STORE_GROUP_READ_ALL_SORTED_BY_RID.Read(_dba, LOAD_INACTIVE_GROUPS_IND: Include.ConvertBoolToChar(bLoadInactiveGroups));
					// End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                }
                else
                {
				    // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
					//return StoredProcedures.MID_STORE_GROUP_READ_ALL_SORTED_BY_ID.Read(_dba);
                    return StoredProcedures.MID_STORE_GROUP_READ_ALL_SORTED_BY_ID.Read(_dba, LOAD_INACTIVE_GROUPS_IND: Include.ConvertBoolToChar(bLoadInactiveGroups));
					// End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                }

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable StoreGroupLevel_ReadAll()
        {
            try
            {
                return StoredProcedures.MID_STORE_GROUP_LEVEL_READ_ALL.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataTable StoreGroupLevelResults_ReadAll()
        {
            try
            {
                return StoredProcedures.MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataTable StoreGroupLevelResults_ReadLatest(int sgRID)
        {
            try
            {
                return StoredProcedures.MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP.Read(_dba, SG_RID: sgRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataTable StoreGroupLevelResults_ReadVersion(int sgRID, int sgVersion)
        {
            try
            {
                return StoredProcedures.MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION.Read(_dba, SG_RID: sgRID, SG_VERSION: sgVersion);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void StoreGroup_UpdateIdAndUser(int groupRid, string description, int userRid)
        {
            try
            {
                StoredProcedures.MID_STORE_GROUP_UPDATE_ID_AND_USER.Update(_dba,
                                                                           SG_RID: groupRid,
                                                                           SG_ID: description,
                                                                           USER_RID: userRid
                                                                           );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



        public void StoreGroup_Update(int groupRID, string description)
        {
            try
            {
                _dba.OpenUpdateConnection();
                //TO DO -update filter name
                StoredProcedures.MID_STORE_GROUP_UPDATE_ID.Update(_dba,
                                                                  SG_RID: groupRID,
                                                                  SG_ID: description
                                                                  );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
        //public void StoreGroupLevel_Update(int groupLevelRID, int groupLevelVersion, string description)
        //{
        //    try
        //    {
        //        _dba.OpenUpdateConnection();
          
        //        StoredProcedures.MID_STORE_GROUP_LEVEL_UPDATE_ID.Update(_dba,
        //                                                                SGL_RID: groupLevelRID,
        //                                                                SGL_VERSION: groupLevelRID,
        //                                                                SGL_ID: description
        //                                                                );

        //        _dba.CommitData();
        //        _dba.CloseUpdateConnection();
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        /// <summary>
        /// updates Store Group Level ID (or description)
        /// </summary>
        /// <param name="groupLevelRID"></param>
        /// <param name="description"></param>
        /// <param name="updateStoreGroupLevelInd">Flag identifying if the STORE_GROUP_LEVEL table should be updated</param>
        public void StoreGroupLevel_Update(int groupLevelRID, int groupLevelVersion, string description, bool updateStoreGroupLevelInd = false)
        {
            try
            {
                _dba.OpenUpdateConnection();

                StoredProcedures.MID_STORE_GROUP_LEVEL_UPDATE_ID.Update(_dba,
                                                                        SGL_RID: groupLevelRID,
                                                                        SGL_VERSION: groupLevelVersion,
                                                                        SGL_ID: description,
                                                                        UPDATE_STORE_GROUP_LEVEL_IND: Include.ConvertBoolToChar(updateStoreGroupLevelInd)
                                                                        );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.

        public void StoreGroupFilter_UpdateLevelName(int filterRID, string oldName, string newName)
        {
            try
            {
                _dba.OpenUpdateConnection();

                StoredProcedures.MID_FILTER_CONDITION_UPDATE_LEVEL_NAME.Update(_dba,
                                                                        FILTER_RID: filterRID,
                                                                        OLD_NAME: oldName,
                                                                        NEW_NAME: newName
                                                                        );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public void StoreGroupFilter_UpdateName(int filterRID, string newName)
        {
            try
            {
                _dba.OpenUpdateConnection();

                StoredProcedures.MID_FILTER_UPDATE_NAME_FOR_SET.Update(_dba,
                                                                        FILTER_RID: filterRID,
                                                                        NEW_NAME: newName
                                                                        );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


        public void StoreGroupLevel_UpdateSequence(int sgl_rid, int sgl_version, int sequence)
        {
            try
            {
                _dba.OpenUpdateConnection();
                //TO DO -update filter conditions
                StoredProcedures.MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE.Update(_dba,
                                                                                SGL_RID: sgl_rid,
                                                                                SGL_VERSION: sgl_version,
                                                                                SGL_SEQUENCE: sequence);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



        public int StoreGroupLevel_Insert(int levelSeq, int SG_RID, string SGL_ID, bool isActive, int conditionRID, int levelType)
        {
            try
            {
                int SGL_RID = -1;

                SGL_RID = StoredProcedures.MID_STORE_GROUP_LEVEL_INSERT.InsertAndReturnRID(_dba,
                                                                                            SG_RID: SG_RID,
                                                                                            SGL_SEQUENCE: levelSeq,
                                                                                            SGL_ID: SGL_ID,
                                                                                            IS_ACTIVE: Include.ConvertBoolToInt(isActive),
                                                                                            CONDITION_RID: conditionRID,
                                                                                            LEVEL_TYPE: levelType
                                                                                            );
                return SGL_RID;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int StoreGroupJoin_Insert(int SG_RID, int SG_VERSION, int SGL_RID, int SGL_VERSION, int STORE_COUNT, string SGL_OVERRIDE_ID, int SGL_OVERRIDE_SEQUENCE)
        {
            try
            {
                int SGJ_RID = -1;

                SGJ_RID = StoredProcedures.MID_STORE_GROUP_JOIN_INSERT.InsertAndReturnRID(_dba,
                                                                                            SG_RID: SG_RID,
                                                                                            SG_VERSION: SG_VERSION,
                                                                                            SGL_RID: SGL_RID,
                                                                                            SGL_VERSION: SGL_VERSION,
                                                                                            STORE_COUNT: STORE_COUNT,
                                                                                            SGL_OVERRIDE_ID: SGL_OVERRIDE_ID,
                                                                                            SGL_OVERRIDE_SEQUENCE: SGL_OVERRIDE_SEQUENCE
                                                                                            );
                return SGJ_RID;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		// Begin TT#1517-MD - stodd - new sets not getting added to database
        public void StoreGroupJoin_FirstTimeInit()
        {
            try
            {
                StoredProcedures.MID_STORE_GROUP_JOIN_FIRST_TIME_INIT.FirstTimeInit(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		// End TT#1517-MD - stodd - new sets not getting added to database

        /// <summary>
        /// Clears version history for store group joins and store group level results, as well as inactive store group levels and inactive store groups
        /// </summary>
        /// <returns></returns>
        public int StoreGroupJoinHistory_DeleteAll()
        {
            try
            {

                int inactiveGroupsRemoved = StoredProcedures.MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL.Delete(_dba);


                return inactiveGroupsRemoved;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int StoreGroupResults_Insert(int SGL_RID, int SGL_VERSION, int ST_RID)
        {
            try
            {
                int RESULT_RID = -1;

                RESULT_RID = StoredProcedures.MID_STORE_GROUP_RESULTS_INSERT.InsertAndReturnRID(_dba,
                                                                                            SGL_RID: SGL_RID,
                                                                                            SGL_VERSION: SGL_VERSION,
                                                                                            ST_RID: ST_RID
                                                                                            );
                return RESULT_RID;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

      
        

        

        public void StoreGroup_SetInactive(int groupRID)
        {
            try
            {

                StoredProcedures.MID_STORE_GROUP_UPDATE_SET_INACTIVE.Update(_dba, SG_RID: groupRID);  //Also sets USER_PLAN group references to null
              
            



                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
        public void StoreGroup_Delete(int groupRID)
        {
            try
            {

                StoredProcedures.MID_STORE_GROUP_DELETE.Delete(_dba, SG_RID: groupRID);  //Also sets USER_PLAN group references to null
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

        public DataTable SharedStoreGroups_Read(int aUserRID)
        {
            try
            {
                return StoredProcedures.MID_STORE_GROUP_READ_SHARED.Read(_dba, USER_RID: aUserRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void StoreGroup_InsertAllStoresAttribute(int filterRID)
        {
            try
            {
                StoredProcedures.MID_STORE_GROUP_INSERT_ALL_STORES_SET.Insert(_dba, FILTER_RID: filterRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



	}
}
