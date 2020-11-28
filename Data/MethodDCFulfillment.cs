using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for DC Fulfillment
	/// </summary>
	public class DCFulfillmentMethodData: MethodBaseData
	{
		private int _methodRid;
        private eDCFulfillmentSplitOption _splitOption = eDCFulfillmentSplitOption.DCFulfillment;
        private bool _applyMinimumsInd = false;
        private char _prioritizeType = 'C';
        private int _headerField = Include.Undefined;
        private int _hcg_RID = Include.NoRID;
        private eDCFulfillmentHeadersOrder _headersOrder = eDCFulfillmentHeadersOrder.Ascending;
        private eDCFulfillmentStoresOrder _storesOrder = eDCFulfillmentStoresOrder.Ascending;
        private eDCFulfillmentSplitByOption _split_By_Option = eDCFulfillmentSplitByOption.SplitByDC;
        private eDCFulfillmentReserve _split_By_Reserve = eDCFulfillmentReserve.ReservePreSplit;
        private eDCFulfillmentMinimums _apply_By = eDCFulfillmentMinimums.ApplyFirst;
        private eDCFulfillmentWithinDC _within_Dc = eDCFulfillmentWithinDC.Proportional;
        private DataTable _dtStoreOrder;
        private eHeaderCharType _fieldDataType = eHeaderCharType.text;
      
		public int MethodRid
		{
			get{return _methodRid;}
			set{_methodRid = value;	}
		}

        public eDCFulfillmentSplitOption SplitOption
        {
            get { return _splitOption; }
            set { _splitOption = value; }
        }

        public bool ApplyMinimumsInd
		{
            get { return _applyMinimumsInd; }
            set { _applyMinimumsInd = value; }
		}

        public char PrioritizeType
        {
            get { return _prioritizeType; }
            set { _prioritizeType = value; }
        }

        public int HeaderField
        {
            get { return _headerField; }
            set { _headerField = value; }
        }

        public int Hcg_RID
        {
            get { return _hcg_RID; }
            set { _hcg_RID = value; }
        }

        public eDCFulfillmentHeadersOrder HeadersOrder
        {
            get { return _headersOrder; }
            set { _headersOrder = value; }
        }

        public eDCFulfillmentStoresOrder StoresOrder
        {
            get { return _storesOrder; }
            set { _storesOrder = value; }
        }

        public eDCFulfillmentSplitByOption Split_By_Option
        {
            get { return _split_By_Option; }
            set { _split_By_Option = value; }
        }

        public eDCFulfillmentReserve Split_By_Reserve
        {
            get { return _split_By_Reserve; }
            set { _split_By_Reserve = value; }
        }

        public eDCFulfillmentMinimums Apply_By
        {
            get { return _apply_By; }
            set { _apply_By = value; }
        }

        public eDCFulfillmentWithinDC Within_Dc
        {
            get { return _within_Dc; }
            set { _within_Dc = value; }
        }

        public DataTable dtStoreOrder
		{
			get{return _dtStoreOrder;}
			set{_dtStoreOrder = value;	}
		}

        public eHeaderCharType FieldDataType
        {
            get { return _fieldDataType; }
            set { _fieldDataType = value; }
        }

     	/// <summary>
        /// Creates an instance of the DCFulfillmentMethodData class
		/// </summary>
		public DCFulfillmentMethodData()
		{
		}

		/// <summary>
        /// Creates an instance of the DCFulfillmentMethodData class
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
        public DCFulfillmentMethodData(int aMethodRID, eChangeType changeType)
		{
            switch (changeType)
            {
                case eChangeType.populate:
                    PopulateDCFulfillment(aMethodRID);
                    break;
            } 
		}

		/// <summary>
        /// Creates an instance of the DCFulfillmentMethodData class
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public DCFulfillmentMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}
        public DCFulfillmentMethodData(TransactionData td)
		{
			_dba = td.DBA;
			_methodRid = Include.NoRID;
		}
		
		public bool PopulateDCFulfillment(int method_RID)
		{
			try
			{
                if (PopulateMethod(method_RID))
                {
                    _methodRid = method_RID;
                    DataSet dsDCFulfillment = StoredProcedures.MID_METHOD_DC_FULFILLMENT_READ.ReadAsDataSet(_dba, METHOD_RID: method_RID);
                    if (dsDCFulfillment.Tables.Count != 0)
                    {
                        DataRow dr = dsDCFulfillment.Tables[0].Rows[0];
                        if (dr["SPLIT_OPTION"] != System.DBNull.Value)
                        {
                            _splitOption = (eDCFulfillmentSplitOption)(Convert.ToInt32(dr["SPLIT_OPTION"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["APPLY_MINIMUMS_IND"] != System.DBNull.Value)
                        {
                            _applyMinimumsInd = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_MINIMUMS_IND"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["PRIORITIZE_TYPE"] != System.DBNull.Value)
                        {
                            _prioritizeType = (Convert.ToChar(dr["PRIORITIZE_TYPE"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["HEADER_FIELD"] != System.DBNull.Value)
                        {
                            _headerField = (Convert.ToInt32(dr["HEADER_FIELD"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["HCG_RID"] != System.DBNull.Value)
                        {
                            _hcg_RID = (Convert.ToInt32(dr["HCG_RID"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["HEADERS_ORDER"] != System.DBNull.Value)
                        {
                            _headersOrder = (eDCFulfillmentHeadersOrder)(Convert.ToInt32(dr["HEADERS_ORDER"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["STORES_ORDER"] != System.DBNull.Value)
                        {
                            _storesOrder = (eDCFulfillmentStoresOrder)(Convert.ToInt32(dr["STORES_ORDER"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["SPLIT_BY_OPTION"] != System.DBNull.Value)
                        {
                            _split_By_Option = (eDCFulfillmentSplitByOption)(Convert.ToInt32(dr["SPLIT_BY_OPTION"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["SPLIT_BY_RESERVE"] != System.DBNull.Value)
                        {
                            _split_By_Reserve = (eDCFulfillmentReserve)(Convert.ToInt32(dr["SPLIT_BY_RESERVE"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["APPLY_BY"] != System.DBNull.Value)
                        {
                            _apply_By = (eDCFulfillmentMinimums)(Convert.ToInt32(dr["APPLY_BY"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["WITHIN_DC"] != System.DBNull.Value)
                        {
                            _within_Dc = (eDCFulfillmentWithinDC)(Convert.ToInt32(dr["WITHIN_DC"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        if (dr["FIELD_DATA_TYPE"] != System.DBNull.Value)
                        {
                            _fieldDataType = (eHeaderCharType)(Convert.ToInt32(dr["FIELD_DATA_TYPE"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        SetDataTables(dsDCFulfillment);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Read database to build data tables
                    DataSet dsDCFulfillment = StoredProcedures.MID_METHOD_DC_FULFILLMENT_READ.ReadAsDataSet(_dba, METHOD_RID: method_RID);
                    _splitOption = eDCFulfillmentSplitOption.DCFulfillment;
                    _applyMinimumsInd = false;
                    _prioritizeType = 'C';
                    _headerField = Include.Undefined;
                    _hcg_RID = Include.NoRID;
                    _headersOrder = eDCFulfillmentHeadersOrder.Ascending;
                    _storesOrder = eDCFulfillmentStoresOrder.Ascending;
                    _fieldDataType = eHeaderCharType.text;
                    SetDataTables(dsDCFulfillment);

                    return false;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        private void SetDataTables(DataSet dsDCFulfillment)
        {
            _dtStoreOrder = dsDCFulfillment.Tables[1];
            _dtStoreOrder.TableName = "StoreOrder";
        }

		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;

            try
            {
                StoredProcedures.MID_METHOD_DC_FULFILLMENT_UPSERT.Insert(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           SPLIT_OPTION: Convert.ToInt32(SplitOption),
                                                                           APPLY_MINIMUMS_IND: Include.ConvertBoolToChar(ApplyMinimumsInd),
                                                                           PRIORITIZE_TYPE: PrioritizeType,
                                                                           HEADER_FIELD: HeaderField,
                                                                           HCG_RID: Hcg_RID,
                                                                           HEADERS_ORDER: Convert.ToInt32(HeadersOrder),
                                                                           STORES_ORDER: Convert.ToInt32(StoresOrder),
                                                                           FIELD_DATA_TYPE: Convert.ToInt32(FieldDataType),
                                                                           SPLIT_BY_OPTION: Convert.ToInt32(Split_By_Option),
                                                                           SPLIT_BY_RESERVE: Convert.ToInt32(Split_By_Reserve),
                                                                           APPLY_BY: Convert.ToInt32(Apply_By),
                                                                           WITHIN_DC: Convert.ToInt32(Within_Dc),
                                                                           STORE_ORDER_TABLE: dtStoreOrder
                                                                           );
            }
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                InsertSuccessful = false;
                throw;
            }
			return InsertSuccessful;
		}
       
		public bool UpdateMethod(int method_RID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			_dba = td.DBA;
			try
			{
				if (UpdateMethod(method_RID))
					UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}

		new public bool UpdateMethod(int method_RID)
		{
			bool UpdateSuccessful = true;
			try
			{
                dtStoreOrder.AcceptChanges();
                StoredProcedures.MID_METHOD_DC_FULFILLMENT_UPSERT.Insert(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           SPLIT_OPTION: Convert.ToInt32(SplitOption),
                                                                           APPLY_MINIMUMS_IND: Include.ConvertBoolToChar(ApplyMinimumsInd),
                                                                           PRIORITIZE_TYPE: PrioritizeType,
                                                                           HEADER_FIELD: HeaderField,
                                                                           HCG_RID: Hcg_RID,
                                                                           HEADERS_ORDER: Convert.ToInt32(HeadersOrder),
                                                                           STORES_ORDER: Convert.ToInt32(StoresOrder),
                                                                           FIELD_DATA_TYPE: Convert.ToInt32(FieldDataType),
                                                                           SPLIT_BY_OPTION: Convert.ToInt32(Split_By_Option),
                                                                           SPLIT_BY_RESERVE: Convert.ToInt32(Split_By_Reserve),
                                                                           APPLY_BY: Convert.ToInt32(Apply_By),
                                                                           WITHIN_DC: Convert.ToInt32(Within_Dc),
                                                                           STORE_ORDER_TABLE: dtStoreOrder
                                                                           );
           		UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}

		public bool DeleteMethod(int method_RID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			_dba = td.DBA;
			try
			{
                StoredProcedures.MID_METHOD_DC_FULFILLMENT_DELETE.Delete(_dba, METHOD_RID: method_RID);
                DeleteSuccessfull = true;
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return DeleteSuccessfull;
		}
	}
}

