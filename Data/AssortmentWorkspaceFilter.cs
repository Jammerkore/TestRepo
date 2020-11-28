//using System;

//using System.Data;
//using System.Collections;
//using System.Globalization;
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    /// <summary>
//    /// Summary description for AssortmentWorkspaceFilterData.
//    /// </summary>
//    public partial class AssortmentWorkspaceFilterData : DataLayer
//    {
//        public AssortmentWorkspaceFilterData() : base()
//        {
            
//        }

//        public DataTable AssortmentWorkspaceFilter_Read(int aUserRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_ASRT_WRKSP_FILTER_READ_ALL.Read(_dba, USER_RID: aUserRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AssortmentWorkspaceFilter_Insert(int aUserRID, int aHnRID, eFilterDateType aHeaderDateType, 
//            int aHeaderDateBetweenFrom, int aHeaderDateBetweenTo, DateTime aHeaderDateFrom, DateTime aHeaderDateTo,
//            eFilterDateType aReleaseDateType, int aReleaseDateBetweenFrom, int aReleaseDateBetweenTo, 
//            DateTime aReleaseDateFrom, DateTime aReleaseDateTo, int aMaximumHeaders)
//        {
//            try
//            {
//                object hnRID = null;
//                if (aHnRID > Include.NoRID)
//                {
//                    hnRID = aHnRID;
//                }

//                StoredProcedures.MID_ASRT_WRKSP_FILTER_INSERT.Insert(_dba,
//                                                                     USER_RID: aUserRID,
//                                                                     HN_RID: Include.ConvertObjectToNullableInt(hnRID),
//                                                                     HEADER_DATE_TYPE: Convert.ToInt32(aHeaderDateType, CultureInfo.CurrentUICulture),
//                                                                     HEADER_DATE_BETWEEN_FROM: aHeaderDateBetweenFrom,
//                                                                     HEADER_DATE_BETWEEN_TO: aHeaderDateBetweenTo,
//                                                                     HEADER_DATE_FROM: aHeaderDateFrom,
//                                                                     HEADER_DATE_TO: aHeaderDateTo,
//                                                                     RELEASE_DATE_TYPE: Convert.ToInt32(aReleaseDateType, CultureInfo.CurrentUICulture),
//                                                                     RELEASE_DATE_BETWEEN_FROM: aReleaseDateBetweenFrom,
//                                                                     RELEASE_DATE_BETWEEN_TO: aReleaseDateBetweenTo,
//                                                                     RELEASE_DATE_FROM: aReleaseDateFrom,
//                                                                     RELEASE_DATE_TO: aReleaseDateTo,
//                                                                     MAXIMUM_HEADERS: aMaximumHeaders
//                                                                     );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AssortmentWorkspaceFilter_Update(int aUserRID, int aHnRID, eFilterDateType aHeaderDateType, 
//            int aHeaderDateBetweenFrom, int aHeaderDateBetweenTo, DateTime aHeaderDateFrom, DateTime aHeaderDateTo,
//            eFilterDateType aReleaseDateType, int aReleaseDateBetweenFrom, int aReleaseDateBetweenTo, 
//            DateTime aReleaseDateFrom, DateTime aReleaseDateTo, int aMaximumHeaders)
//        {
//            try
//            {
//                object hnRID = null;
//                if (aHnRID > Include.NoRID)
//                {
//                    hnRID = aHnRID;
//                }

//                StoredProcedures.MID_ASRT_WRKSP_FILTER_UPDATE.Update(_dba,
//                                                                     USER_RID: aUserRID,
//                                                                     HN_RID: Include.ConvertObjectToNullableInt(hnRID),
//                                                                     HEADER_DATE_TYPE: Convert.ToInt32(aHeaderDateType, CultureInfo.CurrentUICulture),
//                                                                     HEADER_DATE_BETWEEN_FROM: aHeaderDateBetweenFrom,
//                                                                     HEADER_DATE_BETWEEN_TO: aHeaderDateBetweenTo,
//                                                                     HEADER_DATE_FROM: aHeaderDateFrom,
//                                                                     HEADER_DATE_TO: aHeaderDateTo,
//                                                                     RELEASE_DATE_TYPE: Convert.ToInt32(aReleaseDateType, CultureInfo.CurrentUICulture),
//                                                                     RELEASE_DATE_BETWEEN_FROM: aReleaseDateBetweenFrom,
//                                                                     RELEASE_DATE_BETWEEN_TO: aReleaseDateBetweenTo,
//                                                                     RELEASE_DATE_FROM: aReleaseDateFrom,
//                                                                     RELEASE_DATE_TO: aReleaseDateTo,
//                                                                     MAXIMUM_HEADERS: aMaximumHeaders
//                                                                     );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public DataTable AssortmentWorkspaceFilterStatus_Read(int aUserRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_ASRT_WRKSP_FILTER_STATUS_READ_ALL.Read(_dba, USER_RID: aUserRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AssortmentWorkspaceFilterStatus_Insert(int aUserRID, eHeaderAllocationStatus aHeaderStatus, bool aSelected)
//        {
//            try
//            {
//                StoredProcedures.MID_ASRT_WRKSP_FILTER_STATUS_INSERT.Insert(_dba,
//                                                                            USER_RID: aUserRID,
//                                                                            HEADER_STATUS: Convert.ToInt32(aHeaderStatus, CultureInfo.CurrentCulture),
//                                                                            STATUS_SELECTED: Include.ConvertBoolToChar(aSelected)
//                                                                            );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AssortmentWorkspaceFilterStatus_DeleteAll(int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_ASRT_WRKSP_FILTER_STATUS_DELETE.Delete(_dba, USER_RID: aUserRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public DataTable AssortmentWorkspaceFilter_NodeRead(int aNodeRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_ASRT_WRKSP_FILTER_READ_NODE.Read(_dba, HN_RID: aNodeRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
       
//        #region Apply View Filter to User Filter

//        public void ApplyViewFilterToUserFilter(int aViewRID, int aUserRID)
//        {
//            try
//            {
//                WorkspaceFilter_Delete(aUserRID);
//                ViewToWorkspaceFilter_Add(aViewRID, aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void WorkspaceFilter_Delete(int aUserRID)
//        {
//            try
//            {
//                AssortmentWorkspaceFilterStatus_DeleteAll(aUserRID);
//                AssortmentWorkspaceFilter_Delete(aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void AssortmentWorkspaceFilter_Delete(int aUserRID)
//        {
//            try
//            {
//                //string SQLCommand = "DELETE FROM ASRT_WRKSP_FILTER "
//                //    + "WHERE USER_RID = @USER_RID";

//                //MIDDbParameter[] InParams = {   new MIDDbParameter("@USER_RID", aUserRID, eDbType.Int, eParameterDirection.Input)
//                //                          };

//                //_dba.ExecuteNonQuery(SQLCommand, InParams);

//                StoredProcedures.MID_ASRT_WRKSP_FILTER_DELETE.Delete(_dba, USER_RID: aUserRID);
//            }
//            catch (Exception err)
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void ViewToWorkspaceFilter_Add(int aViewRID, int aUserRID)
//        {
//            try
//            {
//                ViewToWorkspaceFilter_Insert(aViewRID, aUserRID);
//                ViewToWorkspaceFilterStatus_Insert(aViewRID, aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void ViewToWorkspaceFilter_Insert(int aViewRID, int aUserRID)
//        {
//            try
//            {
//                //string SQLCommand = "INSERT INTO ASRT_WRKSP_FILTER SELECT " + Convert.ToString(aUserRID, CultureInfo.CurrentUICulture)
//                //  + ",HN_RID,HEADER_DATE_TYPE,HEADER_DATE_BETWEEN_FROM,HEADER_DATE_BETWEEN_TO,"
//                //  + "HEADER_DATE_FROM,HEADER_DATE_TO,"
//                //  + "RELEASE_DATE_TYPE,RELEASE_DATE_BETWEEN_FROM,RELEASE_DATE_BETWEEN_TO,"
//                //  + "RELEASE_DATE_FROM,RELEASE_DATE_TO,MAXIMUM_HEADERS "
//                //  + "FROM GRID_VIEW_FILTER WHERE VIEW_RID = " + Convert.ToString(aViewRID, CultureInfo.CurrentUICulture);

//                //_dba.ExecuteNonQuery(SQLCommand);

//                StoredProcedures.MID_ASRT_WRKSP_FILTER_INSERT_VIEW_TO_WORKSPACE.InsertandReturnRID(_dba,
//                                                                                        USER_RID: aUserRID,
//                                                                                        VIEW_RID: aViewRID
//                                                                                        );
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//         private void ViewToWorkspaceFilterStatus_Insert(int aViewRID, int aUserRID)
//        {
//            try
//            {
//                //string SQLCommand = "INSERT INTO ASRT_WRKSP_FILTER_STATUS SELECT " + Convert.ToString(aUserRID, CultureInfo.CurrentUICulture)
//                //     + ",HEADER_STATUS,STATUS_SELECTED"
//                //     + " FROM GRID_VIEW_FILTER_STATUS WHERE VIEW_RID = " + Convert.ToString(aViewRID, CultureInfo.CurrentUICulture);

//                //_dba.ExecuteNonQuery(SQLCommand);

//                StoredProcedures.MID_ASRT_WRKSP_FILTER_STATUS_INSERT_VIEW_TO_WORKSPACE.Insert(_dba,
//                                                                                            USER_RID: aUserRID,
//                                                                                            VIEW_RID: aViewRID
//                                                                                            );
//            }
//            catch (Exception err)
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
//        #endregion
//    }
//}
