//using System;

//using System.Data;
//using System.Collections;
//using System.Globalization;
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{

//    public partial class AllocationWorkspaceFilterData : DataLayer
//    {
//        public AllocationWorkspaceFilterData() : base()
//        {
//        }
        
//        public DataTable AllocationWorkspaceFilter_Read(int aUserRID)
//        {
//            try
//            {
//                //MID Track # 2354 - removed nolock because it causes concurrency issues
//                return StoredProcedures.MID_AL_WRKSP_FILTER_READ.Read(_dba, USER_RID: aUserRID);  
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilter_Insert(int aUserRID, int aHnRID, eFilterDateType aHeaderDateType, 
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

//               StoredProcedures.MID_AL_WRKSP_FILTER_INSERT.Insert(_dba, 
//                                                                  USER_RID: aUserRID,
//                                                                  HN_RID: Include.ConvertObjectToNullableInt(hnRID),
//                                                                  HEADER_DATE_TYPE: Convert.ToInt32(aHeaderDateType, CultureInfo.CurrentCulture),
//                                                                  HEADER_DATE_BETWEEN_FROM: aHeaderDateBetweenFrom,
//                                                                  HEADER_DATE_BETWEEN_TO: aHeaderDateBetweenTo,
//                                                                  HEADER_DATE_FROM: aHeaderDateFrom,
//                                                                  HEADER_DATE_TO: aHeaderDateTo,
//                                                                  RELEASE_DATE_TYPE: (int)aReleaseDateType,
//                                                                  RELEASE_DATE_BETWEEN_FROM: aReleaseDateBetweenFrom,
//                                                                  RELEASE_DATE_BETWEEN_TO: aReleaseDateBetweenTo,
//                                                                  RELEASE_DATE_FROM: aReleaseDateFrom,
//                                                                  RELEASE_DATE_TO: aReleaseDateTo,
//                                                                  MAXIMUM_HEADERS: aMaximumHeaders
//                                                                  );       
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilter_Update(int aUserRID, int aHnRID, eFilterDateType aHeaderDateType, 
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

//                StoredProcedures.MID_AL_WRKSP_FILTER_UPDATE.Update(_dba, 
//                                                                   USER_RID: aUserRID,
//                                                                   HN_RID: Include.ConvertObjectToNullableInt(hnRID),
//                                                                   HEADER_DATE_TYPE: Convert.ToInt32(aHeaderDateType, CultureInfo.CurrentCulture),
//                                                                   HEADER_DATE_BETWEEN_FROM: aHeaderDateBetweenFrom,
//                                                                   HEADER_DATE_BETWEEN_TO: aHeaderDateBetweenTo,
//                                                                   HEADER_DATE_FROM: aHeaderDateFrom,
//                                                                   HEADER_DATE_TO: aHeaderDateTo,
//                                                                   RELEASE_DATE_TYPE: (int)aReleaseDateType,
//                                                                   RELEASE_DATE_BETWEEN_FROM: aReleaseDateBetweenFrom,
//                                                                   RELEASE_DATE_BETWEEN_TO: aReleaseDateBetweenTo,
//                                                                   RELEASE_DATE_FROM: aReleaseDateFrom,
//                                                                   RELEASE_DATE_TO: aReleaseDateTo,
//                                                                   MAXIMUM_HEADERS: aMaximumHeaders
//                                                                   );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public DataTable AllocationWorkspaceFilterTypes_Read(int aUserRID)
//        {
//            try
//            {
//                //MID Track # 2354 - removed nolock because it causes concurrency issues
//                return StoredProcedures.MID_AL_WRKSP_FILTER_TYPES_READ.Read(_dba, USER_RID: aUserRID); 
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilterTypes_Insert(int aUserRID, eHeaderType aHeaderType, 
//            bool aSelected)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_TYPES_INSERT.Insert(_dba, 
//                                                                         USER_RID: aUserRID,
//                                                                         HEADER_TYPE: Convert.ToInt32(aHeaderType, CultureInfo.CurrentCulture),
//                                                                         TYPE_SELECTED: Include.ConvertBoolToChar(aSelected)
//                                                                         );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilterTypes_DeleteAll(int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_TYPES_DELETE.Delete(_dba, USER_RID: aUserRID);   
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public DataTable AllocationWorkspaceFilterStatus_Read(int aUserRID)
//        {
//            try
//            {
//                // begin MID Track # 2354 - removed nolock because it causes concurrency issues
//                return StoredProcedures.MID_AL_WRKSP_FILTER_STATUS_READ.Read(_dba, USER_RID: aUserRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilterStatus_Insert(int aUserRID, eHeaderAllocationStatus aHeaderStatus, bool aSelected)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_STATUS_INSERT.Insert(_dba, 
//                                                                          USER_RID: aUserRID,
//                                                                          HEADER_STATUS: Convert.ToInt32(aHeaderStatus, CultureInfo.CurrentCulture),
//                                                                          STATUS_SELECTED: Include.ConvertBoolToChar(aSelected)
//                                                                          );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilterStatus_DeleteAll(int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_STATUS_DELETE.Delete(_dba, USER_RID: aUserRID); 
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
  
//        public DataTable AllocationWorkspaceFilter_NodeRead(int aNodeRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_AL_WRKSP_FILTER_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
//        // Assortment Change
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
//                AllocationWorkspaceFilterStatus_DeleteAll(aUserRID);
//                AllocationWorkspaceFilterTypes_DeleteAll(aUserRID);
//                AllocationWorkspaceFilter_Delete(aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void AllocationWorkspaceFilter_Delete(int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_DELETE.Delete(_dba, USER_RID: aUserRID);
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
//                ViewToWorkspaceFilterTypes_Insert(aViewRID, aUserRID);
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
//                StoredProcedures.MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW.Insert(_dba, USER_RID: aUserRID,
//                                                                                  VIEW_RID: aViewRID
//                                                                                  );
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void ViewToWorkspaceFilterTypes_Insert(int aViewRID, int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW.Insert(_dba, USER_RID: aUserRID,
//                                                                                        VIEW_RID: aViewRID
//                                                                                        );                                                                       
//            }
//            catch (Exception err)
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        private void ViewToWorkspaceFilterStatus_Insert(int aViewRID, int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW.Insert(_dba, USER_RID: aUserRID,
//                                                                                         VIEW_RID: aViewRID
//                                                                                         );
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
