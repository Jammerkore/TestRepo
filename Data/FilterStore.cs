//using System;
//using System.Collections;
//using System.Data;
//using System.Data.Common;
//using System.Text;
//using System.Globalization;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    public enum eFilterObjectType
//    {
//        Attribute,
//        Data,
//        Characteristic
//    }
//    //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
//    public enum eFilterXRefType
//    {
//        StoreFilterXRef,
//        ProductSearchXRef
//    }
//    //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)


//    public partial class StoreFilterData : DataLayer
//    {
//        public StoreFilterData() : base()
//        {

//        }
//        // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
//        //public DataTable StoreFilter_Read(int aFilterRID)
//        //{
//        //    try
//        //    {
//        //        return StoredProcedures.MID_STORE_FILTER_READ.Read(_dba, STORE_FILTER_RID: aFilterRID);
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}
//        // End Track #6302

//        //public DataTable StoreFilter_Read(ArrayList aUserRIDList)
//        //{
//        //    try
//        //    {
//        //        DataTable dtUserList = new DataTable();
//        //        dtUserList.Columns.Add("USER_RID", typeof(int));
//        //        foreach (int userRID in aUserRIDList)
//        //        {
//        //            //ensure userRIDs are distinct, and only added to the datatable one time
//        //            if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
//        //            {
//        //                DataRow dr = dtUserList.NewRow();
//        //                dr["USER_RID"] = userRID;
//        //                dtUserList.Rows.Add(dr);
//        //            }
//        //        }

//        //        return StoredProcedures.MID_STORE_FILTER_READ_FROM_USERS.Read(_dba, USER_RID_LIST: dtUserList);
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public DataTable StoreFilter_ReadParent(ArrayList aUserRIDList)
//        //{
//        //    try
//        //    {
//        //        DataTable dtUserList = new DataTable();
//        //        dtUserList.Columns.Add("USER_RID", typeof(int));
//        //        foreach (int userRID in aUserRIDList)
//        //        {
//        //            //ensure userRIDs are distinct, and only added to the datatable one time
//        //            if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
//        //            {
//        //                DataRow dr = dtUserList.NewRow();
//        //                dr["USER_RID"] = userRID;
//        //                dtUserList.Rows.Add(dr);
//        //            }
//        //        }
//        //        return StoredProcedures.MID_STORE_FILTER_READ_PARENT_FROM_USERS.Read(_dba, USER_RID_LIST: dtUserList);
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
//        public DataTable StoreFilter_ReadAll()
//        {
//            try
//            {
//                return StoredProcedures.MID_STORE_FILTER_READ_ALL.Read(_dba);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

//        //public void StoreFilter_Update(int aFilterRID, int aUserRID, string aFilterName)
//        //{
//        //    try
//        //    {
//        //        StoredProcedures.MID_STORE_FILTER_UPDATE.Update(_dba,
//        //                                                        STORE_FILTER_RID: aFilterRID,
//        //                                                        USER_RID: aUserRID,
//        //                                                        STORE_FILTER_NAME: aFilterName
//        //                                                        );
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public void StoreFilter_Update(StoreFilterProfile aStoreFilterProfile)
//        //{
//        //    try
//        //    {
//        //        StoreFilter_Update(aStoreFilterProfile.UnloadToDataRow(NewStoreFilterRow()));
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public void StoreFilter_Update(DataRow aStoreFilterRow)
//        //{
//        //    try
//        //    {

//        //        StoredProcedures.MID_STORE_FILTER_UPDATE.Update(_dba,
//        //                                                        STORE_FILTER_RID: (int)aStoreFilterRow["STORE_FILTER_RID"],
//        //                                                        USER_RID: (int)aStoreFilterRow["USER_RID"],
//        //                                                        STORE_FILTER_NAME: (string)aStoreFilterRow["STORE_FILTER_NAME"]
//        //                                                        );

                
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public void StoreFilter_Delete(int aFilterRID)
//        //{
//        //    try
//        //    {
//        //        StoreFilterObject_Delete(aFilterRID);
//        //        StoredProcedures.MID_STORE_FILTER_DELETE.Delete(_dba, STORE_FILTER_RID: aFilterRID);
//        //        //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login

//        //        if (ConnectionIsOpen)
//        //        {
//        //            SecurityAdmin sa = new SecurityAdmin(_dba);
//        //            sa.DeleteUserItemByTypeAndRID((int)eProfileType.StoreFilter, aFilterRID);
//        //        }
//        //        ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//        //        //SecurityAdmin sa = new SecurityAdmin();
//        //        //try
//        //        //{
//        //        //    sa.OpenUpdateConnection();
//        //        //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        //        //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.Filter, aFilterRID);
//        //        //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.StoreFilter, aFilterRID);
//        //        //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        //        //    sa.CommitData();
//        //        //}
//        //        //catch
//        //        //{
//        //        //    throw;
//        //        //}
//        //        //finally
//        //        //{
//        //        //    sa.CloseUpdateConnection();
//        //        //}
//        //        ////End Track #4815


//        //        //End TT#1564 - DOConnell - Missing Tasklist record prevents Login




//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public int StoreFilter_Insert(int aUserRID, string aFilterName)
//        //{
//        //    try
//        //    {
//        //        int filterRID = StoredProcedures.SP_MID_STORE_FILTER_INSERT.InsertAndReturnRID(_dba,
//        //                                                                                    USER_RID: aUserRID,
//        //                                                                                    STORE_FILTER_NAME: aFilterName
//        //                                                                                    );
//        //        //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
//        //        if (ConnectionIsOpen)
//        //        {
//        //            SecurityAdmin sa = new SecurityAdmin(_dba);
//        //            sa.AddUserItem(aUserRID, (int)eProfileType.StoreFilter, filterRID, aUserRID);
//        //        }
//        //        //SecurityAdmin sa = new SecurityAdmin();
//        //        //try
//        //        //{
//        //        //    sa.OpenUpdateConnection();
//        //        //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        //        //    //sa.AddUserItem(aUserRID, (int)eSharedDataType.Filter, filterRID, aUserRID);
//        //        //    sa.AddUserItem(aUserRID, (int)eProfileType.StoreFilter, filterRID, aUserRID);
//        //        //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        //        //    sa.CommitData();
//        //        //}
//        //        //catch
//        //        //{
//        //        //    throw;
//        //        //}
//        //        //finally
//        //        //{
//        //        //    sa.CloseUpdateConnection();
//        //        //}
//        //        //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

//        //        return filterRID;
//        //        //End Track #4815
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public int StoreFilter_GetKey(int aUserRID, string aFilterName)
//        //{
//        //    try
//        //    {
//        //        // MID Track # 2354 - removed nolock because it causes concurrency issues

//        //        DataTable dt = StoredProcedures.MID_STORE_FILTER_READ_FROM_USER_AND_NAME.Read(_dba,
//        //                                                                              USER_RID: aUserRID,
//        //                                                                              STORE_FILTER_NAME: aFilterName
//        //                                                                              );

//        //        if (dt.Rows.Count == 1)
//        //        {
//        //            return (Convert.ToInt32(dt.Rows[0]["STORE_FILTER_RID"], CultureInfo.CurrentUICulture));
//        //        }
//        //        else
//        //        {
//        //            return -1;
//        //        }
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public string StoreFilter_GetName(int filterRid)
//        //{
//        //    try
//        //    {
//        //        DataTable dt = StoredProcedures.MID_STORE_FILTER_READ.Read(_dba, STORE_FILTER_RID: filterRid);

//        //        if (dt.Rows.Count == 1)
//        //        {
//        //            return dt.Rows[0]["STORE_FILTER_NAME"].ToString();
//        //        }
//        //        else
//        //        {
//        //            return "unknown";
//        //        }
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public DataTable StoreFilterObject_Read(int aFilterRID)
//        //{
//        //    try
//        //    {
//        //        //MID Track # 2354 - removed nolock because it causes concurrency issues
//        //        return StoredProcedures.MID_STORE_FILTER_OBJECT_READ.Read(_dba, STORE_FILTER_RID: aFilterRID);
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}


	
//        //public void StoreFilterObject_Insert(DataTable aFilterObjectTable, int aFilterRID)
//        //{
//        //    try
//        //    {
//        //        foreach (DataRow row in aFilterObjectTable.Rows)
//        //        {
//        //            if (row["STORE_FILTER_OBJECT"] != DBNull.Value)
//        //            {
//        //                StoredProcedures.MID_STORE_FILTER_OBJECT_INSERT.Insert(_dba,
//        //                                                                   STORE_FILTER_RID: aFilterRID,
//        //                                                                   STORE_FILTER_OBJECT_TYPE: (int)row["STORE_FILTER_OBJECT_TYPE"],
//        //                                                                   STORE_FILTER_OBJECT: (byte[])row["STORE_FILTER_OBJECT"]
//        //                                                                   );
//        //            }
//        //        }
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}


//        //public void StoreFilterObject_Insert(int aFilterRID, object aAttrQueues, object aDataQueues)
//        //{
//        //    try
//        //    {
//        //        StoredProcedures.MID_STORE_FILTER_OBJECT_INSERT.Insert(_dba,
//        //                                                        STORE_FILTER_RID: aFilterRID,
//        //                                                        STORE_FILTER_OBJECT_TYPE: (int)eFilterObjectType.Attribute,
//        //                                                        STORE_FILTER_OBJECT: (byte[])aAttrQueues
//        //                                                        );

    
//        //        StoredProcedures.MID_STORE_FILTER_OBJECT_INSERT.Insert(_dba,
//        //                                                        STORE_FILTER_RID: aFilterRID,
//        //                                                        STORE_FILTER_OBJECT_TYPE: (int)eFilterObjectType.Data,
//        //                                                        STORE_FILTER_OBJECT: (byte[])aDataQueues
//        //                                                        );
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}

//        //public void StoreFilterObject_Delete(int aFilterRID)
//        //{
//        //    try
//        //    {
//        //         StoredProcedures.MID_STORE_FILTER_OBJECT_DELETE.Delete(_dba, STORE_FILTER_RID: aFilterRID);
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}


//        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
//        public void StoreFilterObject_XRef_Insert(ArrayList aFilterXRefList)
//        {
//            try
//            {
//                foreach (FilterXRef xrefData in aFilterXRefList)
//                {
//                    StoredProcedures.MID_IN_USE_FILTER_XREF_INSERT.Insert(_dba,
//                                                               FILTER_RID: xrefData.filterRID,
//                                                               FILTER_XREF_TYPE: (int)xrefData.filterXRefType,
//                                                               PROFILE_TYPE: (int)xrefData.profileType,
//                                                               PROFILE_TYPE_RID: xrefData.profileTypeRID
//                                                               );
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//        public void StoreFilterObject_XRef_Delete(int aFilterRID)
//        {
//            try
//            {
//                StoredProcedures.MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE.Delete(_dba, FILTER_RID: aFilterRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
        
//        }
//        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)




       


//        //private DataRow NewStoreFilterRow()
//        //{
//        //    try
//        //    {
//        //        //MID Track # 2354 - removed nolock because it causes concurrency issues
//        //        return StoredProcedures.MID_STORE_FILTER_READ_DEFAULT.Read(_dba).NewRow();
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}
//    }

	

//        /// <summary>
//        /// Class to store filter cross reference data
//        /// </summary>
//        /// <remarks>
//        /// TT#252 - jsobek - Identify and save components of the filter in a cross reference table for In Use Tool
//        /// </remarks>
//        public class FilterXRef
//        {
//            public eFilterXRefType filterXRefType;
//            public int filterRID;  // For Product Search, this equals the user RID for that product search.  For Store Filter, this equals the Store Filter RID.  
//            public eProfileType profileType;
//            public int profileTypeRID;

//            public FilterXRef(eFilterXRefType paramfilterXRefType, int paramfilterRID, eProfileType paramprofileType, int paramprofileTypeRID)
//            {
//                filterXRefType = paramfilterXRefType;
//                filterRID = paramfilterRID;
//                profileType = paramprofileType;
//                profileTypeRID = paramprofileTypeRID;
//            }

           

//        }

       
      
   
//}
