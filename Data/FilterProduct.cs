//using System;
//using System.Collections;
//using System.Data;
//using System.Data.Common;
//using System.Text;
//using System.Globalization;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    public partial class ProductFilterData : DataLayer
//    {
//        public ProductFilterData() : base()
//        {

//        }

//        public DataTable ProductSearchObject_Read(int aUserRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_PRODUCT_SEARCH_OBJECT_READ.Read(_dba, USER_RID: aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
//        public DataTable ProductSearchObject_ReadIDs()
//        {
//            try
//            {
//                return StoredProcedures.MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS.Read(_dba);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

       

//        public void ProductSearchObject_Insert(int aUserRID, object aCharacteristicQueues)
//        {
//            try
//            {
//                if (aCharacteristicQueues != null)
//                {

//                    byte[] PRODUCT_SEARCH_OBJECT = (byte[])aCharacteristicQueues;

//                    StoredProcedures.MID_PRODUCT_SEARCH_OBJECT_INSERT.Insert(_dba,
//                                                                             USER_RID: aUserRID,
//                                                                             PRODUCT_SEARCH_OBJECT_TYPE: (int)eFilterObjectType.Characteristic,
//                                                                             PRODUCT_SEARCH_OBJECT: PRODUCT_SEARCH_OBJECT
//                                                                             );
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        /// <summary>
//        /// Insert In-Use profile types and their RIDs for the filter in the cross reference table
//        /// </summary>
//        /// <remarks>
//        /// TT#252 - jsobek - Filter Cross-Reference (for In Use Tool)
//        /// </remarks>
//        public void ProductSearchObject_XRef_Insert(ArrayList aFilterXRefList)
//        {
//            try
//            {
//                foreach (FilterXRef xrefData in aFilterXRefList)
//                {
//                    StoredProcedures.MID_IN_USE_FILTER_XREF_INSERT.Insert(_dba,
//                                                                      FILTER_RID: xrefData.filterRID,
//                                                                      FILTER_XREF_TYPE: (int)xrefData.filterXRefType,
//                                                                      PROFILE_TYPE: (int)xrefData.profileType,
//                                                                      PROFILE_TYPE_RID: xrefData.profileTypeRID
//                                                                      );
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void ProductSearchObject_XRef_Delete(int aFilterRID)
//        {
//            try
//            {
//                StoredProcedures.MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT.Delete(_dba, FILTER_RID: aFilterRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
       

//        public void ProductSearchObject_Delete(int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_PRODUCT_SEARCH_OBJECT_DELETE.Delete(_dba, USER_RID: aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public DataTable ProductSearchView_Read(int aUserRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_PRODUCT_SEARCH_VIEW_READ.Read(_dba, USER_RID: aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void ProductSearchView_Insert(int aUserRID, int aSequence, int aDataType, int aDataKey, int aColumnWidth)
//        {
//            try
//            {  
//                StoredProcedures.MID_PRODUCT_SEARCH_VIEW_INSERT.Insert(_dba,
//                                                                       USER_RID: aUserRID,
//                                                                       VIEW_SEQUENCE: aSequence,
//                                                                       DATA_TYPE: aDataType,
//                                                                       DATA_KEY: aDataKey,
//                                                                       COLUMN_WIDTH: aColumnWidth
//                                                                       );
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void ProductSearchView_Delete(int aUserRID)
//        {
//            try
//            {
//                StoredProcedures.MID_PRODUCT_SEARCH_VIEW_DELETE.Delete(_dba, USER_RID: aUserRID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//    }


//}
