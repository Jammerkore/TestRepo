using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for MethodForecastModifySales.
	/// </summary>
	public class ForecastModifySalesMethodData : MethodBaseData
	{
		private int				_methodRid;
		private int				_hnRID;
		private int				_cdrRID;
		private int				_filterRid;
		private eStoreAverageBy	_averageBy;
		private DataTable		_dtGrades;
		private DataTable		_dtSellThru;
		private DataTable		_dtMatrix;

		private TransactionData _td;


		#region Properties
		public int MethodRid
		{
			get	{return _methodRid;}
			set	{_methodRid = value;}
		}
		public int HierNodeRID
		{
			get {return _hnRID;	}
			set	{_hnRID = value;}
		}
		public int CDR_RID
		{
			get{return _cdrRID;}
			set{_cdrRID = value;}
		}
		public int Filter
		{
			get{return _filterRid;}
			set{_filterRid = value;}
		}
		public eStoreAverageBy AverageBy
		{
			get{return _averageBy;}
			set{_averageBy = value;}
		}
		public DataTable GradeDataTable
		{
			get{return _dtGrades;}
			set{_dtGrades = value;}
		}
		public DataTable SellThruDataTable
		{
			get{return _dtSellThru;}
			set{_dtSellThru = value;}
		}
		public DataTable MatrixDataTable
		{
			get{return _dtMatrix;}
			set{_dtMatrix = value;}
		}
		#endregion

		public ForecastModifySalesMethodData()
		{
		}

		public ForecastModifySalesMethodData(int aMethodRid, eChangeType changeType)
		{
			_methodRid = aMethodRid;
			switch (changeType)
			{
				case eChangeType.populate:
					Populate(aMethodRid);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the ForecastModifySalesMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public ForecastModifySalesMethodData(TransactionData td, int aMethodRid)
		{
			_dba = td.DBA;
			_methodRid = aMethodRid;
		}

		public bool Populate(int aMethodRid)
		{
			try
			{
				if (PopulateMethod(aMethodRid))
				{
					_methodRid =  aMethodRid; 
					
				
                    DataTable dtModifySales = MIDEnvironment.CreateDataTable();
                    dtModifySales = StoredProcedures.MID_METHOD_MOD_SALES_READ.Read(_dba, METHOD_RID: aMethodRid);
					if (dtModifySales.Rows.Count != 0)
					{
						DataRow dr = dtModifySales.Rows[0];
						_hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						_filterRid = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
						_averageBy = (eStoreAverageBy)Convert.ToInt32(dr["AVERAGE_STORE"], CultureInfo.CurrentUICulture);

						_dtGrades = GetGrades(aMethodRid);
						_dtSellThru = GetSellThru(aMethodRid);
						_dtMatrix = GetMatrix(aMethodRid);
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception Ex)
			{
				string message = Ex.ToString();
				throw;
			}
		}

		public DataTable GetGrades(int methodRid)
		{
			try
			{
				DataTable dtGrade = MIDEnvironment.CreateDataTable("Grade");
                dtGrade = StoredProcedures.MID_METHOD_MOD_SALES_GRADE_READ.Read(_dba, METHOD_RID: methodRid);
				return dtGrade;
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetSellThru(int methodRid)
		{
			try
			{
				DataTable dtSellThru = MIDEnvironment.CreateDataTable("Sell Thru");
                dtSellThru = StoredProcedures.MID_METHOD_MOD_SALES_SELL_THRU_READ.Read(_dba, METHOD_RID: methodRid);
				return dtSellThru;
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetMatrix(int methodRid)
		{
			try
			{
				DataTable dtMatrix = MIDEnvironment.CreateDataTable("Matrix");
                dtMatrix = StoredProcedures.MID_METHOD_MOD_SALES_MATRIX_READ.Read(_dba, METHOD_RID: methodRid);
				return dtMatrix;
			}
			catch
			{
				throw;
			}
		}

		public bool InsertMethod(int aMethodRID, TransactionData td)
		{
			bool InsertSuccessful = true;

			try
			{	
				_methodRid = aMethodRID;
				_td = td;

				int averageBy = (int)this._averageBy;


                int? STORE_FILTER_RID_Nullable = null;
                if (_filterRid > Include.UndefinedStoreFilter) STORE_FILTER_RID_Nullable = _filterRid;
                StoredProcedures.MID_METHOD_MOD_SALES_INSERT.Insert(_dba,
                                                                    METHOD_RID: aMethodRID,
                                                                    HN_RID: _hnRID,
                                                                    CDR_RID: _cdrRID,
                                                                    STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                    AVERAGE_STORE: averageBy
                                                                    );
					
				InsertSuccessful = InsertChildData();
			}
			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}

		private bool InsertChildData()
		{
			bool InsertSuccessful = true;

			try
			{	
				if (InsertGrades())
				{
					if (InsertSellThru())
					{
						if (InsertMatrix())
							InsertSuccessful = true;
						else
							InsertSuccessful = false;
					}
					else
						InsertSuccessful = false;
				}
				else
					InsertSuccessful = false;
					
			}
			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}
		
		private bool InsertGrades()
		{
			bool InsertSuccessful = true;

			try
			{	
				foreach (DataRow aRow in this._dtGrades.Rows)
				{
					int boundary = Convert.ToInt32(aRow["BOUNDARY"], CultureInfo.CurrentUICulture);

					string grade = aRow["GRADE_CODE"].ToString();


                    StoredProcedures.MID_METHOD_MOD_SALES_GRADE_INSERT.Insert(_dba,
                                                                              METHOD_RID: _methodRid,
                                                                              BOUNDARY: boundary,
                                                                              GRADE_CODE: grade
                                                                              );
				}
					
			}
			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}

		private bool InsertSellThru()
		{
			bool InsertSuccessful = true;

			try
			{	
				foreach (DataRow aRow in this._dtSellThru.Rows)
				{
					int sellThru = Convert.ToInt32(aRow["SELL_THRU"], CultureInfo.CurrentUICulture);


                    StoredProcedures.MID_METHOD_MOD_SALES_SELL_THRU_INSERT.Insert(_dba,
                                                                                  METHOD_RID: _methodRid,
                                                                                  SELL_THRU: sellThru
                                                                                  );
				}
					
			}
			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}

		private bool InsertMatrix()
		{
			bool InsertSuccessful = true;

			try
			{	
				foreach (DataRow aRow in this._dtMatrix.Rows)
				{
					if (aRow["MATRIX_RULE"] != DBNull.Value)
					{
           
						int boundary = Convert.ToInt32(aRow["BOUNDARY"], CultureInfo.CurrentUICulture);
						int sellThru = Convert.ToInt32(aRow["SELL_THRU"], CultureInfo.CurrentUICulture);
						int rule = Convert.ToInt32(aRow["MATRIX_RULE"], CultureInfo.CurrentUICulture);
						decimal qty = 0;
						if (aRow["MATRIX_RULE_QUANTITY"] != DBNull.Value)
							qty = Convert.ToDecimal(aRow["MATRIX_RULE_QUANTITY"], CultureInfo.CurrentUICulture);

                
                        int? SGL_RID_Nullable = null;
                        if (aRow["SGL_RID"] != DBNull.Value) SGL_RID_Nullable = Convert.ToInt32(aRow["SGL_RID"], CultureInfo.CurrentUICulture);
                        StoredProcedures.MID_METHOD_MOD_SALES_MATRIX_INSERT.Insert(_dba,
                                                                           METHOD_RID: _methodRid,
                                                                           SGL_RID: SGL_RID_Nullable,
                                                                           BOUNDARY: boundary,
                                                                           SELL_THRU: sellThru,
                                                                           MATRIX_RULE: rule,
                                                                           MATRIX_RULE_QUANTITY: (double)qty
                                                                           );
					}
				}
					
			}
			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}

		public bool UpdateMethod(int aMethodRID, TransactionData td)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
			bool UpdateSuccessful = true;

			try
			{
				if (DeleteChildData())
				{
					_methodRid = aMethodRID;
				
					int averageBy = (int)_averageBy;

            
                    int? STORE_FILTER_RID_Nullable = null;
                    if (_filterRid > Include.UndefinedStoreFilter) STORE_FILTER_RID_Nullable = _filterRid;
                    StoredProcedures.MID_METHOD_MOD_SALES_UPDATE.Update(_dba,
                                                                        METHOD_RID: aMethodRID,
                                                                        HN_RID: _hnRID,
                                                                        CDR_RID: _cdrRID,
                                                                        STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                        AVERAGE_STORE: averageBy
                                                                        );

					UpdateSuccessful = InsertChildData();
				}

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

		public bool DeleteMethod(int aMethodRID, TransactionData td)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
			bool DeleteSuccessfull = true;

			try
			{
				if (DeleteChildData())
				{

                    StoredProcedures.MID_METHOD_MOD_SALES_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
					DeleteSuccessfull = true;
				}
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
		
		private bool DeleteChildData()
		{
			bool DeleteSuccessfull = true;

			try
			{
                StoredProcedures.MID_METHOD_MOD_SALES_DELETE_CHILD_DATA.Delete(_dba, METHOD_RID: _methodRid);
				
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
