using System;
using System.Data;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for RuleMethodData.
	/// </summary>
	public class RuleMethodData: MethodBaseData
	{
		private int				_store_Filter_RID = Include.NoRID;
		private int				_HDR_RID = Include.NoRID;
		private eSortDirection	_store_Order = eSortDirection.Descending;
		private eComponentType	_header_Component;
		private int				_header_Pack_RID = Include.NoRID;
		private int				_color_Code_RID = Include.NoRID;
		private eRuleMethod		_included_Stores = eRuleMethod.None;
		private double			_included_Quantity;
		private eRuleMethod		_excluded_Stores = eRuleMethod.None;
		private double			_excluded_Quantity;
		private int				_SGL_RID = Include.NoRID;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		private char			_is_Header_Master;
// (CSMITH) - END MID Track #3219
        private int              _hdr_BC_RID = Include.NoRID;       // Assortment
        private char            _includeReserveInd;

		public int Store_Filter_RID
		{
			get{return _store_Filter_RID;}
			set{_store_Filter_RID = value;	}
		}
		public int HDR_RID
		{
			get{return _HDR_RID;}
			set{_HDR_RID = value;	}
		}
		public eSortDirection Store_Order
		{
			get{return _store_Order;}
			set{_store_Order = value;	}
		}
		public eComponentType Header_Component
		{
			get{return _header_Component;}
			set{_header_Component = value;	}
		}
		public int Header_Pack_RID
		{
			get{return _header_Pack_RID;}
			set{_header_Pack_RID = value;	}
		}
		public int Color_Code_RID
		{
			get{return _color_Code_RID;}
			set{_color_Code_RID = value;	}
		}
        // Assortment BEGIN
        public int Hdr_BC_RID
        {
            get { return _hdr_BC_RID; }
            set { _hdr_BC_RID = value; }
        }
        // Assortment END
		public eRuleMethod Included_Stores
		{
			get{return _included_Stores;}
			set{_included_Stores = value;	}
		}
		public double Included_Quantity
		{
			get{return _included_Quantity;}
			set{_included_Quantity = value;	}
		}
		public eRuleMethod Excluded_Stores
		{
			get{return _excluded_Stores;}
			set{_excluded_Stores = value;	}
		}
		public double Excluded_Quantity
		{
			get{return _excluded_Quantity;}
			set{_excluded_Quantity = value;	}
		}
		public int SGL_RID
		{
			get{return _SGL_RID;}
			set{_SGL_RID = value;	}
		}
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement

		public char Is_Header_Master
		{
			get
			{
				return _is_Header_Master;
			}

			set
			{
				_is_Header_Master = value;
			}
		}
// (CSMITH) - END MID Track #3219

        public char Include_Reserve_Ind
        {
            get { return _includeReserveInd; }
            set { _includeReserveInd = value; }
        }

		/// <summary>
		/// Creates an instance of the MethodRule class.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		public RuleMethodData(int method_RID, eChangeType changeType)
		{
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateRule(method_RID);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the MethodRule class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="method_RID">The record ID of the method</param>
		public RuleMethodData(TransactionData td, int method_RID)
		{
			_dba = td.DBA;
		}

		/// <summary>
		/// Creates an instance of the MethodRule class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		public RuleMethodData(TransactionData td)
		{
			//
			// TODO: Add constructor logic here
			//
			_dba = td.DBA;
//			_Gen_Alloc_HDR_RID = Include.NoRID;
		}


		public bool PopulateRule(int method_RID)
		{
			try
			{

				if (PopulateMethod(method_RID))
				{
					// MID Track # 2354 - removed nolock because it causes concurrency issues
					DataTable dtRuleMethod = MIDEnvironment.CreateDataTable();
                    dtRuleMethod = StoredProcedures.MID_METHOD_RULE_READ.Read(_dba, METHOD_RID: method_RID);
					if(dtRuleMethod.Rows.Count != 0)
					{
						DataRow dr = dtRuleMethod.Rows[0];
						_store_Filter_RID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture );
						_HDR_RID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
						_store_Order = (eSortDirection)(Convert.ToInt32(dr["STORE_ORDER"], CultureInfo.CurrentUICulture));
						_header_Component = (eComponentType)(Convert.ToInt32(dr["HEADER_COMPONENT"], CultureInfo.CurrentUICulture));
						_header_Pack_RID = Convert.ToInt32(dr["HDR_PACK_RID"], CultureInfo.CurrentUICulture);
                        // Assortment BEGIN
                        //_color_Code_RID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                        _hdr_BC_RID = Convert.ToInt32(dr["HDR_BC_RID"], CultureInfo.CurrentUICulture);
                        if (_hdr_BC_RID == Include.NoRID)
                            _color_Code_RID = Include.NoRID;
                        else
                        {
                            //string SQL2 = "SELECT COLOR_CODE_RID FROM HEADER_BULK_COLOR WHERE HDR_BC_RID = "
                            //            + Convert.ToString(_hdr_BC_RID, CultureInfo.CurrentUICulture);
                            //_color_Code_RID = Convert.ToInt32(_dba.ExecuteScalar(SQL2), CultureInfo.CurrentUICulture);
                            _color_Code_RID = (int)StoredProcedures.MID_HEADER_BULK_COLOR_READ_COLOR_CODE.ReadValue(_dba, HDR_BC_RID: _hdr_BC_RID);
                        }
                        // Assortment END
						_included_Stores = (eRuleMethod)(Convert.ToInt32(dr["INCLUDED_STORES"], CultureInfo.CurrentUICulture));
						_included_Quantity = Convert.ToDouble(dr["INCLUDED_QUANTITY"], CultureInfo.CurrentUICulture);
						_excluded_Stores = (eRuleMethod)(Convert.ToInt32(dr["EXCLUDED_STORES"], CultureInfo.CurrentUICulture));
						_excluded_Quantity = Convert.ToDouble(dr["EXCLUDED_QUANTITY"], CultureInfo.CurrentUICulture);;
						_SGL_RID = Convert.ToInt32(dr["SGL_RID"], CultureInfo.CurrentUICulture);
                        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
						_is_Header_Master = Convert.ToChar(dr["IS_HEADER_MASTER"], CultureInfo.CurrentUICulture);				
                        // (CSMITH) - END MID Track #3219
                        _includeReserveInd = Convert.ToChar(dr["INCLUDE_RESERVE_IND"]);				

						return true;
					}
					else
						return false;
				}
				else
					return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#1237-MD -jsobek -Size Rule Error
		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessfull = true;
			try
			{	
                //// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
                //string SQLCommand = "INSERT INTO METHOD_RULE(METHOD_RID, STORE_FILTER_RID, HDR_RID, STORE_ORDER,"
                //    // Assortment BEGIN
                //    // " HEADER_COMPONENT, HDR_PACK_RID, COLOR_CODE_RID, INCLUDED_STORES, INCLUDED_QUANTITY,"
                //    + " HEADER_COMPONENT, HDR_PACK_RID, HDR_BC_RID, INCLUDED_STORES, INCLUDED_QUANTITY,"
                //    // Assortment END
                //    + " EXCLUDED_STORES, EXCLUDED_QUANTITY, SGL_RID, IS_HEADER_MASTER)"
                //    + " VALUES ("
                //    + method_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                //// (CSMITH) - END MID Track #3219
                //if (Store_Filter_RID == Include.NoRID)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += Store_Filter_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                //}
                //if (this.HDR_RID == Include.NoRID)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += HDR_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                //}
                //SQLCommand += ((int)Store_Order).ToString(CultureInfo.CurrentUICulture) + ","
                //    + ((int)Header_Component).ToString(CultureInfo.CurrentUICulture) + ",";
                //if (Header_Pack_RID == Include.NoRID)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += Header_Pack_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                //}
                //// Assortment BEGIN
                ////if (Color_Code_RID == Include.NoRID)
                ////{
                ////	SQLCommand +=  "null,";
                ////}
                ////else
                ////{
                ////	SQLCommand += Color_Code_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                ////}
                //if (Hdr_BC_RID == Include.NoRID)
                //{
                //    SQLCommand += "null,";
                //}
                //else
                //{
                //    SQLCommand += Hdr_BC_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                //}
                //// Assortment END
                //SQLCommand += ((int)this.Included_Stores).ToString(CultureInfo.CurrentUICulture) + ","
                //    + this.Included_Quantity.ToString(CultureInfo.CurrentUICulture) + ","
                //    + ((int)this.Excluded_Stores).ToString(CultureInfo.CurrentUICulture) + ","
                //    + this.Excluded_Quantity.ToString(CultureInfo.CurrentUICulture) + ",";
                //// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
                //if (this.SGL_RID == Include.NoRID)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += SGL_RID.ToString(CultureInfo.CurrentUICulture) + ",";
                //}
                //SQLCommand += _is_Header_Master.ToString(CultureInfo.CurrentUICulture);
                //// (CSMITH) - END MID Track #3219
                //SQLCommand += ")";

                //td.DBA.ExecuteNonQuery(SQLCommand);
                
                int? STORE_FILTER_RID_Nullable = null;
                if (Store_Filter_RID != Include.NoRID) STORE_FILTER_RID_Nullable = Store_Filter_RID;

                int? HDR_RID_Nullable = null;
                if (this.HDR_RID != Include.NoRID) HDR_RID_Nullable = this.HDR_RID;

                int? HDR_PACK_RID_Nullable = null;
                if (Header_Pack_RID != Include.NoRID) HDR_PACK_RID_Nullable = Header_Pack_RID;

                int? HDR_BC_RID_Nullable = null;
                if (Hdr_BC_RID != Include.NoRID) HDR_BC_RID_Nullable = Hdr_BC_RID;

                int? SGL_RID_Nullable = null;
                if (this.SGL_RID != Include.NoRID) SGL_RID_Nullable = this.SGL_RID;

                int INCLUDED_QUANTITY_Int = Convert.ToInt32(this.Included_Quantity);
                int EXCLUDED_QUANTITY_Int = Convert.ToInt32(this.Excluded_Quantity);

                StoredProcedures.MID_METHOD_RULE_INSERT.Insert(td.DBA,
                                                               METHOD_RID: method_RID,
                                                               STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                               HDR_RID: HDR_RID_Nullable,
                                                               STORE_ORDER: (int)Store_Order,
                                                               HEADER_COMPONENT: (int)Header_Component,
                                                               HDR_PACK_RID: HDR_PACK_RID_Nullable,
                                                               HDR_BC_RID: HDR_BC_RID_Nullable,
                                                               INCLUDED_STORES: (int)this.Included_Stores,
                                                               INCLUDED_QUANTITY: INCLUDED_QUANTITY_Int,
                                                               EXCLUDED_STORES: (int)this.Excluded_Stores,
                                                               EXCLUDED_QUANTITY: EXCLUDED_QUANTITY_Int,
                                                               SGL_RID: SGL_RID_Nullable,
                                                               IS_HEADER_MASTER: _is_Header_Master,
                                                               INCLUDE_RESERVE_IND: _includeReserveInd
                                                               );
	
				InsertSuccessfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				InsertSuccessfull = false;
				throw;
			}
			return InsertSuccessfull;
		}

		public bool UpdateMethod(int method_RID, TransactionData td)
		{
			bool UpdateSuccessfull = true;
			try
			{
//                string SQLCommand = "UPDATE METHOD_RULE SET ";
//                if (Store_Filter_RID == Include.NoRID)
//                {
//                    SQLCommand += "STORE_FILTER_RID = null,";
//                }
//                else
//                {
//                    SQLCommand += "STORE_FILTER_RID = " + Store_Filter_RID.ToString(CultureInfo.CurrentUICulture) + ",";
//                }
//                if (HDR_RID == Include.NoRID)
//                {
//                    SQLCommand += "HDR_RID = null,";
//                }
//                else
//                {
//                    SQLCommand += "HDR_RID = " + HDR_RID.ToString(CultureInfo.CurrentUICulture) + ",";
//                }
//                SQLCommand += " STORE_ORDER = " + ((int)Store_Order).ToString(CultureInfo.CurrentUICulture) + ","
//                    + " HEADER_COMPONENT = " + ((int)Header_Component).ToString(CultureInfo.CurrentUICulture) + ",";
//                if (Header_Pack_RID == Include.NoRID)
//                {
//                    SQLCommand += "HDR_PACK_RID = null,";
//                }
//                else
//                {
//                    SQLCommand += "HDR_PACK_RID = " + Header_Pack_RID.ToString(CultureInfo.CurrentUICulture) + ",";
//                }
//                // Assortment BEGIN 
//                //if (Color_Code_RID == Include.NoRID)
//                //{
//                //	SQLCommand += "COLOR_CODE_RID = null,";
//                //}
//                //else
//                //{
//                //	SQLCommand += "COLOR_CODE_RID = " + Color_Code_RID.ToString(CultureInfo.CurrentUICulture) + ",";
//                //}
//                if (Hdr_BC_RID == Include.NoRID)
//                {
//                    SQLCommand += "HDR_BC_RID = null,";
//                }
//                else
//                {
//                    SQLCommand += "HDR_BC_RID = " + Hdr_BC_RID.ToString(CultureInfo.CurrentUICulture) + ",";
//                }
//                // Assortment END
//                SQLCommand += " INCLUDED_STORES = " + ((int)Included_Stores).ToString(CultureInfo.CurrentUICulture) + ","
//                    + " INCLUDED_QUANTITY = " + Included_Quantity.ToString(CultureInfo.CurrentUICulture) + ","
//                    + " EXCLUDED_STORES = " + ((int)Excluded_Stores).ToString(CultureInfo.CurrentUICulture) + ","
//                    + " EXCLUDED_QUANTITY = " + Excluded_Quantity.ToString(CultureInfo.CurrentUICulture) + ",";
//// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
//                if (SGL_RID == Include.NoRID)
//                {
//                    SQLCommand += "SGL_RID = null,";
//                }
//                else
//                {
//                    SQLCommand += "SGL_RID = " + SGL_RID.ToString(CultureInfo.CurrentUICulture) + ",";
//                }
//                SQLCommand += "IS_HEADER_MASTER = " + _is_Header_Master.ToString(CultureInfo.CurrentUICulture);
//// (CSMITH) - END MID Track #3219
				
//                SQLCommand += " WHERE METHOD_RID = " +  method_RID.ToString(CultureInfo.CurrentUICulture);

//                td.DBA.ExecuteNonQuery(SQLCommand);
                int? STORE_FILTER_RID_Nullable = null;
                if (Store_Filter_RID != Include.NoRID) STORE_FILTER_RID_Nullable = Store_Filter_RID;

                int? HDR_RID_Nullable = null;
                if (this.HDR_RID != Include.NoRID) HDR_RID_Nullable = this.HDR_RID;

                int? HDR_PACK_RID_Nullable = null;
                if (Header_Pack_RID != Include.NoRID) HDR_PACK_RID_Nullable = Header_Pack_RID;

                int? HDR_BC_RID_Nullable = null;
                if (Hdr_BC_RID != Include.NoRID) HDR_BC_RID_Nullable = Hdr_BC_RID;

                int? SGL_RID_Nullable = null;
                if (this.SGL_RID != Include.NoRID) SGL_RID_Nullable = this.SGL_RID;

                int INCLUDED_QUANTITY_Int = Convert.ToInt32(this.Included_Quantity);
                int EXCLUDED_QUANTITY_Int = Convert.ToInt32(this.Excluded_Quantity);

                StoredProcedures.MID_METHOD_RULE_UPDATE.Update(td.DBA,
                                                               METHOD_RID: method_RID,
                                                               STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                               HDR_RID: HDR_RID_Nullable,
                                                               STORE_ORDER: (int)Store_Order,
                                                               HEADER_COMPONENT: (int)Header_Component,
                                                               HDR_PACK_RID: HDR_PACK_RID_Nullable,
                                                               HDR_BC_RID: HDR_BC_RID_Nullable,
                                                               INCLUDED_STORES: (int)this.Included_Stores,
                                                               INCLUDED_QUANTITY: INCLUDED_QUANTITY_Int,
                                                               EXCLUDED_STORES: (int)this.Excluded_Stores,
                                                               EXCLUDED_QUANTITY: EXCLUDED_QUANTITY_Int,
                                                               SGL_RID: SGL_RID_Nullable,
                                                               IS_HEADER_MASTER: _is_Header_Master,
                                                               INCLUDE_RESERVE_IND: _includeReserveInd
                                                               );

	
				UpdateSuccessfull = false;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				UpdateSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessfull;
		}

		public bool DeleteMethod(int method_RID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                //string SQLCommand = "DELETE FROM METHOD_RULE WHERE METHOD_RID = ";
                //SQLCommand += method_RID.ToString(CultureInfo.CurrentUICulture);

                //td.DBA.ExecuteNonQuery(SQLCommand);
                StoredProcedures.MID_METHOD_RULE_DELETE.Delete(td.DBA, method_RID);
	
				DeleteSuccessfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				DeleteSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return DeleteSuccessfull;
		}
        //End TT#1237-MD -jsobek -Size Rule Error
	}
	
}
