using System;
using System.Collections;  //TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for OTS_Plan
	/// </summary>
	public class VelocityMethodData: MethodBaseData
	{
		private int _sg_RID;
		private int _OTS_Plan_HN_RID;
		private int _OTS_Plan_PH_RID;
		private int _OTS_Plan_PHL_SEQ;
		private int _OTS_Begin_CDR_RID;
		private int _OTS_Ship_To_CDR_RID;
		private int _velocity_Method_RID;

		private char _sim_Store_Ind;
		private char _trend_Percent;
		private char _avg_Using_Chain_Ind;
		private char _ship_Using_Basis_Ind;
        
		// Begin TT # 91 - stodd
		//private char _gradesByBasisInd;	// Track #6074 stodd
		// End TT # 91

        // Begin TT#313 - JSmith -  balance does not remain checked
        private char _balance_Ind;
        // End TT#313

        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        private char _apply_Min_Max_Ind;  
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        private char _reconcile_Ind;
        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private eMerchandiseType _merchandiseType;
        private char _inventory_Ind;
        private int _MERCH_HN_RID;
        private int _MERCH_PH_RID;
        private int _MERCH_PHL_SEQ;

        //Begin TT#855-MD -jsobek -Velocity Enhancements
        private eVelocityMethodGradeVariableType _gradeVariableType;
        public eVelocityMethodGradeVariableType GradeVariableType
        {
            get { return _gradeVariableType; }
            set { _gradeVariableType = value; }
        }

        private char _balanceToHeaderInd;
        public char BalanceToHeaderInd
        {
            get { return _balanceToHeaderInd; }
            set { _balanceToHeaderInd = value; }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

        // END TT#1287 - AGallagher - Inventory Min/Max

                
		private DataSet _dsVelocity;
        private ArrayList SGLRIDList;  //TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
		
		public int Velocity_Method_RID
		{
			get
			{
				return _velocity_Method_RID;
			}

			set
			{
				_velocity_Method_RID = value;
			}
		}

		public int StoreGroup_RID
		{
			get
			{
				return _sg_RID;
			}

			set
			{
				_sg_RID = value;
			}
		}

		public int OTS_Plan_HN_RID
		{
			get
			{
				return _OTS_Plan_HN_RID;
			}

			set
			{
				_OTS_Plan_HN_RID = value;
			}
		}

		public int OTS_Plan_PH_RID
		{
			get
			{
				return _OTS_Plan_PH_RID;
			}

			set
			{
				_OTS_Plan_PH_RID = value;
			}
		}

		public int OTS_Plan_PHL_SEQ
		{
			get
			{
				return _OTS_Plan_PHL_SEQ;
			}

			set
			{
				_OTS_Plan_PHL_SEQ = value;
			}
		}

		public char Sim_Store_Ind
		{
			get
			{
				return _sim_Store_Ind;
			}

			set
			{
				_sim_Store_Ind = value;
			}
		}

		public char Avg_Using_Chain_Ind
		{
			get
			{
				return _avg_Using_Chain_Ind;
			}

			set
			{
				_avg_Using_Chain_Ind = value;
			}
		}
		
		public char Ship_Using_Basis_Ind
		{
			get
			{
				return _ship_Using_Basis_Ind;
			}

			set
			{
				_ship_Using_Basis_Ind = value;
			}
        }
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        public char Apply_Min_Max_Ind
		{
			get
			{
				return _apply_Min_Max_Ind;
			}

			set
			{
                _apply_Min_Max_Ind = value;
			}
		}
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public char Inventory_Ind
        {
            get
            {
                return _inventory_Ind;
            }

            set
            {
                _inventory_Ind = value;
            }
        }

        public int MERCH_HN_RID
        {
            get
            {
                return _MERCH_HN_RID;
            }

            set
            {
                _MERCH_HN_RID = value;
            }
        }

        public int MERCH_PH_RID
        {
            get
            {
                return _MERCH_PH_RID;
            }

            set
            {
                _MERCH_PH_RID = value;
            }
        }

        public int MERCH_PHL_SEQ
        {
            get
            {
                return _MERCH_PHL_SEQ;
            }

            set
            {
                _MERCH_PHL_SEQ = value;
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

		
		public char Trend_Percent
		{
			get
			{
				return _trend_Percent;
			}

			set
			{
				_trend_Percent = value;
			}
		}
		
		public int OTS_Begin_CDR_RID
		{
			get
			{
				return _OTS_Begin_CDR_RID;
			}

			set
			{
				_OTS_Begin_CDR_RID = value;
			}
		}

		public int OTS_Ship_To_CDR_RID
		{
			get
			{
				return _OTS_Ship_To_CDR_RID;
			}

			set
			{
				_OTS_Ship_To_CDR_RID = value;
			}
		}
		
		public DataSet DSVelocity
		{
			get
			{
				return _dsVelocity;
			}

			set
			{
				_dsVelocity = value;
			}
		}

        // Begin TT#313 - JSmith -  balance does not remain checked
        /// <summary>
        /// Gets or sets the flag that will be analyzed to balance the velocity results or not
        /// </summary>
        public char Balance_Ind
        {
            get
            {
                return _balance_Ind;
            }
            set
            {
                _balance_Ind = value;
            }
        }
        // End TT#313

        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        public char Reconcile_Ind
        {
            get
            {
                return _reconcile_Ind;
            }
            set
            {
                _reconcile_Ind = value;
            }
        }
        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

        // Begin Track #6074 stodd
		// Begin TT # 91 - stodd
		//public bool GradesByBasisInd
		//{
		//    get
		//    {
		//        return Include.ConvertCharToBool(_gradesByBasisInd);
		//    }

		//    set
		//    {
		//        _gradesByBasisInd = Include.ConvertBoolToChar(value);
		//    }
		//}
		// End TT # 91 - stodd
		// End Track #6074 stodd
		/// <summary>
		/// Creates an instance of the VelocityMethodData class.
		/// </summary>
		public VelocityMethodData()
		{
		}

		/// <summary>
		/// Creates an instance of the VelocityMethodData class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public VelocityMethodData(int aMethodRID)
		{
			_velocity_Method_RID = Include.NoRID;
		}

		/// <summary>
		/// Creates an instance of the VelocityMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public VelocityMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_velocity_Method_RID = Include.NoRID;
		}

		public bool PopulateVelocity(int method_RID)
		{
			try
			{
				if (PopulateMethod(method_RID))
				{
					_velocity_Method_RID =  method_RID; 
					//MID Track # 2354 - removed nolock because it causes concurrency issues        
					DataTable dtVelocityMethod = MIDEnvironment.CreateDataTable();
                    dtVelocityMethod = StoredProcedures.MID_METHOD_VELOCITY_READ.Read(_dba, METHOD_RID: _velocity_Method_RID);

					if (dtVelocityMethod.Rows.Count != 0)
					{
						DataRow dr = dtVelocityMethod.Rows[0];
						_sg_RID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
						_OTS_Plan_HN_RID = Convert.ToInt32(dr["OTS_PLAN_HN_RID"], CultureInfo.CurrentUICulture);
						_OTS_Plan_PH_RID = Convert.ToInt32(dr["OTS_PLAN_PH_RID"], CultureInfo.CurrentUICulture);
						_OTS_Plan_PHL_SEQ = Convert.ToInt32(dr["OTS_PLAN_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
						_sim_Store_Ind = Convert.ToChar(dr["SIM_STORE_IND"], CultureInfo.CurrentUICulture);				
						_avg_Using_Chain_Ind = Convert.ToChar(dr["AVG_USING_CHAIN_IND"], CultureInfo.CurrentUICulture);	
						_ship_Using_Basis_Ind = Convert.ToChar(dr["SHIP_USING_BASIS_IND"], CultureInfo.CurrentUICulture);		
						_trend_Percent = Convert.ToChar(dr["TREND_PERCENT"], CultureInfo.CurrentUICulture);
						_OTS_Begin_CDR_RID = Convert.ToInt32(dr["OTS_BEGIN_CDR_RID"], CultureInfo.CurrentUICulture);
						_OTS_Ship_To_CDR_RID = Convert.ToInt32(dr["OTS_SHIP_TO_CDR_RID"], CultureInfo.CurrentUICulture);
                        // Begin TT#313 - JSmith -  balance does not remain checked
                        _balance_Ind = Convert.ToChar(dr["BALANCE_IND"], CultureInfo.CurrentUICulture);	
                        // End TT#313
                        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        _apply_Min_Max_Ind = Convert.ToChar(dr["APPLY_MIN_MAX_IND"], CultureInfo.CurrentUICulture);	
                        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                        _reconcile_Ind = Convert.ToChar(dr["RECONCILE_IND"], CultureInfo.CurrentUICulture);	
                        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                        _inventory_Ind = Convert.ToChar(dr["INVENTORY_IND"], CultureInfo.CurrentUICulture);
                        //_merchandiseType = (eMerchandiseType)(Convert.ToInt32(dr["MERCH_TYPE"], CultureInfo.CurrentUICulture));
                        _MERCH_HN_RID = Convert.ToInt32(dr["MERCH_HN_RID"], CultureInfo.CurrentUICulture);
                        _MERCH_PH_RID = Convert.ToInt32(dr["MERCH_PH_RID"], CultureInfo.CurrentUICulture);
                        _MERCH_PHL_SEQ = Convert.ToInt32(dr["MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);

                        //Begin TT#855-MD -jsobek -Velocity Enhancements
                        if (dr["GRADE_VARIABLE_TYPE"] == DBNull.Value)
                        {
                            _gradeVariableType = eVelocityMethodGradeVariableType.Sales;
                        }
                        else
                        {
                            _gradeVariableType = (eVelocityMethodGradeVariableType)Convert.ToInt32(dr["GRADE_VARIABLE_TYPE"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["BALANCE_TO_HEADER_IND"] == DBNull.Value)
                        {
                            _balanceToHeaderInd = '0';
                        }
                        else
                        {
                            _balanceToHeaderInd = Convert.ToChar(dr["BALANCE_TO_HEADER_IND"], CultureInfo.CurrentUICulture);
                        }
                        //End TT#855-MD -jsobek -Velocity Enhancements

                        // END TT#1287 - AGallagher - Inventory Min/Max
    					// Begin Track #6074 stodd
						// Begin TT # 91 - stodd
						//_gradesByBasisInd = Convert.ToChar(dr["GRADES_BY_BASIS_IND"], CultureInfo.CurrentUICulture);
						// End TT # 91 - stodd
						// End Track #6074
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

		public DataSet GetVelocityChildData()
		{ 
			try
			{
				_dsVelocity = MIDEnvironment.CreateDataSet();

				DataTable dtBasis = SetupBasisTable();
                // Begin TT#216 - JSmith - inconsistancies between 2.7 and 3.1
                //DataTable dtSalesPeriod = SetupSalesPeriodTable();
                // End TT#216
				DataTable dtVelocityGrade = SetupVelocityGradeTable();
				DataTable dtSellThru = SetupSellThruTable();
				DataTable dtGroupLevel = SetupGroupLevelTable();
				DataTable dtMatrix = SetupMatrixTable();
      
				_dsVelocity.Tables.Add(dtBasis);
                // Begin TT#216 - JSmith - inconsistancies between 2.7 and 3.1
                //_dsVelocity.Tables.Add(dtSalesPeriod);
                // End TT#216
				_dsVelocity.Tables.Add(dtVelocityGrade);
				_dsVelocity.Tables.Add(dtSellThru);
				_dsVelocity.Tables.Add(dtGroupLevel);
				_dsVelocity.Tables.Add(dtMatrix);

				return _dsVelocity;
			}

			catch (Exception Ex)
			{
				string message = Ex.ToString();
				throw;
			}
		}

		private DataTable SetupBasisTable()
		{
            DataTable dt = StoredProcedures.MID_METHOD_VELOCITY_BASIS_READ.Read(_dba, METHOD_RID: _velocity_Method_RID);
            dt.TableName = "Basis";

            dt.Columns[0].ColumnName = "BasisSequence";
            dt.Columns[1].ColumnName = "BasisHNRID";
            dt.Columns[2].ColumnName = "BasisPHRID";
            dt.Columns[3].ColumnName = "BasisPHLSequence";
            dt.Columns[4].ColumnName = "BasisFVRID";
            dt.Columns[5].ColumnName = "cdrRID";
            dt.Columns[6].ColumnName = "Weight";

            return dt;

           
           
		}

		private DataTable SetupVelocityGradeTable()
		{
            DataTable dt = StoredProcedures.MID_METHOD_VELOCITY_GRADE_READ.Read(_dba, METHOD_RID: _velocity_Method_RID);;
            dt.TableName = "VelocityGrade";

            dt.Columns[0].ColumnName = "RowPosition";
            dt.Columns[1].ColumnName = "Grade";

            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            dt.Columns[3].ColumnName = "MinStock";
            dt.Columns[4].ColumnName = "MaxStock";
            dt.Columns[5].ColumnName = "MinAd";
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

            int rowpos = 0;

            foreach (DataRow row in dt.Rows)
            {
                row["RowPosition"] = rowpos;
                rowpos++;
            }

            return dt;
		}

		private DataTable SetupSellThruTable()
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            DataTable dt = StoredProcedures.MID_METHOD_VELOCITY_SELL_THRU_READ.Read(_dba, METHOD_RID: _velocity_Method_RID);
            dt.TableName = "SellThru";

            dt.Columns[0].ColumnName = "RowPosition";
            dt.Columns[1].ColumnName = "SellThruIndex";

            int rowpos = 0;

            foreach (DataRow row in dt.Rows)
            {
                row["RowPosition"] = rowpos;
                rowpos++;
            }

            return dt;
            
		}

		private DataTable SetupGroupLevelTable()
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            DataTable dt = StoredProcedures.MID_METHOD_VELOCITY_GROUP_LEVEL_READ.Read(_dba, METHOD_RID: _velocity_Method_RID);
            dt.TableName = "GroupLevel";

            dt.Columns[0].ColumnName = "SglRID";
            dt.Columns[1].ColumnName = "NoOnHandRule";
            dt.Columns[2].ColumnName = "NoOnHandQty";
            // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
            dt.Columns[3].ColumnName = "ModeInd";
            dt.Columns[4].ColumnName = "AverageRule";
            dt.Columns[5].ColumnName = "AverageQty";
            dt.Columns[6].ColumnName = "SpreadInd";
            // END TT#637 - AGallagher - Velocity - Spread Average (#7) 

            return dt;

            
		}

		private DataTable SetupMatrixTable()
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            DataTable dt = StoredProcedures.MID_METHOD_VELOCITY_MATRIX_READ.Read(_dba, METHOD_RID: _velocity_Method_RID);
            dt.TableName = "VelocityMatrix";

            dt.Columns[0].ColumnName = "SglRID";
            dt.Columns[1].ColumnName = "Boundary";
            dt.Columns[2].ColumnName = "SellThruIndex";
            dt.Columns[3].ColumnName = "VelocityRule";
            dt.Columns[4].ColumnName = "VelocityQty";
            dt.Columns[5].ColumnName = "Stores";
            dt.Columns[6].ColumnName = "AvgWOS";

            return dt;
            
		}

		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;

			try
			{	
                _velocity_Method_RID = method_RID;
               

                int? OTS_PLAN_HN_RID_Nullable = null;
                if (_OTS_Plan_HN_RID != Include.NoRID) OTS_PLAN_HN_RID_Nullable = _OTS_Plan_HN_RID;

                int? OTS_PLAN_PH_RID_Nullable = null;
                if (_OTS_Plan_PH_RID != Include.NoRID) OTS_PLAN_PH_RID_Nullable = _OTS_Plan_PH_RID;

                int? OTS_PLAN_PHL_SEQUENCE_Nullable = null;
                if (_OTS_Plan_PH_RID != Include.NoRID) OTS_PLAN_PHL_SEQUENCE_Nullable = _OTS_Plan_PHL_SEQ;

                int? OTS_BEGIN_CDR_RID_Nullable = null;
                if (_OTS_Begin_CDR_RID != Include.NoRID && _OTS_Begin_CDR_RID != Include.UndefinedCalendarDateRange) OTS_BEGIN_CDR_RID_Nullable = _OTS_Begin_CDR_RID;

                int? MERCH_HN_RID_Nullable = null;
                if (_MERCH_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = _MERCH_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (_MERCH_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = _MERCH_PH_RID;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (_MERCH_PH_RID != Include.NoRID) MERCH_PHL_SEQUENCE_Nullable = _MERCH_PHL_SEQ;  //fix for older code

                int? OTS_SHIP_TO_CDR_RID_Nullable = null;
                if (_OTS_Ship_To_CDR_RID != Include.NoRID && _OTS_Ship_To_CDR_RID != Include.UndefinedCalendarDateRange) OTS_SHIP_TO_CDR_RID_Nullable = _OTS_Ship_To_CDR_RID;

                int MERCH_TYPE = Convert.ToInt32(_merchandiseType);

                StoredProcedures.MID_METHOD_VELOCITY_INSERT.Insert(_dba,
                                                                   METHOD_RID: method_RID,
                                                                   SG_RID: StoreGroup_RID,
                                                                   OTS_PLAN_HN_RID: OTS_PLAN_HN_RID_Nullable,
                                                                   OTS_PLAN_PH_RID: OTS_PLAN_PH_RID_Nullable,
                                                                   OTS_PLAN_PHL_SEQUENCE: OTS_PLAN_PHL_SEQUENCE_Nullable,
                                                                   SIM_STORE_IND: _sim_Store_Ind,
                                                                   AVG_USING_CHAIN_IND: _avg_Using_Chain_Ind,
                                                                   SHIP_USING_BASIS_IND: _ship_Using_Basis_Ind,
                                                                   TREND_PERCENT: _trend_Percent,
                                                                   OTS_BEGIN_CDR_RID: OTS_BEGIN_CDR_RID_Nullable,
                                                                   OTS_SHIP_TO_CDR_RID: OTS_SHIP_TO_CDR_RID_Nullable,
                                                                   BALANCE_IND: _balance_Ind,
                                                                   APPLY_MIN_MAX_IND: _apply_Min_Max_Ind,
                                                                   RECONCILE_IND: _reconcile_Ind,
                                                                   INVENTORY_IND: _inventory_Ind,
                                                                   MERCH_TYPE: MERCH_TYPE,
                                                                   MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                   MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                   MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                   GRADE_VARIABLE_TYPE: (int)_gradeVariableType,
                                                                   BALANCE_TO_HEADER_IND: _balanceToHeaderInd
                                                                   );
				
				if (UpdateChildData())
				{
					InsertSuccessful = true;
				}
				else
				{
					InsertSuccessful = false;
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
		
		public bool UpdateMethod(int method_RID, TransactionData td)
		{
			_dba = td.DBA;

			bool UpdateSuccessful = true;

			try
			{
				if (DeleteChildData() && UpdateVelocityMethod(method_RID) && UpdateChildData())
				{
					UpdateSuccessful = true;
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

		public bool UpdateVelocityMethod(int method_RID)
		{
			bool UpdateSuccessful = true;

			try
			{
				_velocity_Method_RID = method_RID;
				

                int? OTS_PLAN_HN_RID_Nullable = null;
                if (_OTS_Plan_HN_RID != Include.NoRID) OTS_PLAN_HN_RID_Nullable = _OTS_Plan_HN_RID;

                int? OTS_PLAN_PH_RID_Nullable = null;
                if (_OTS_Plan_PH_RID != Include.NoRID) OTS_PLAN_PH_RID_Nullable = _OTS_Plan_PH_RID;

                int? OTS_PLAN_PHL_SEQUENCE_Nullable = null;
                if (_OTS_Plan_PH_RID != Include.NoRID) OTS_PLAN_PHL_SEQUENCE_Nullable = _OTS_Plan_PHL_SEQ;

                int? OTS_BEGIN_CDR_RID_Nullable = null;
                if (_OTS_Begin_CDR_RID != Include.NoRID && _OTS_Begin_CDR_RID != Include.UndefinedCalendarDateRange) OTS_BEGIN_CDR_RID_Nullable = _OTS_Begin_CDR_RID;

                int? MERCH_HN_RID_Nullable = null;
                if (_MERCH_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = _MERCH_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (_MERCH_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = _MERCH_PH_RID;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (_MERCH_PH_RID != Include.NoRID) MERCH_PHL_SEQUENCE_Nullable = _MERCH_PHL_SEQ;  //fix for older code

                int? OTS_SHIP_TO_CDR_RID_Nullable = null;
                if (_OTS_Ship_To_CDR_RID != Include.NoRID && _OTS_Ship_To_CDR_RID != Include.UndefinedCalendarDateRange) OTS_SHIP_TO_CDR_RID_Nullable = _OTS_Ship_To_CDR_RID;

                int MERCH_TYPE = Convert.ToInt32(_merchandiseType);

                StoredProcedures.MID_METHOD_VELOCITY_UPDATE.Update(_dba,
                                                                   METHOD_RID: method_RID,
                                                                   SG_RID: StoreGroup_RID,
                                                                   OTS_PLAN_HN_RID: OTS_PLAN_HN_RID_Nullable,
                                                                   OTS_PLAN_PH_RID: OTS_PLAN_PH_RID_Nullable,
                                                                   OTS_PLAN_PHL_SEQUENCE: OTS_PLAN_PHL_SEQUENCE_Nullable,
                                                                   SIM_STORE_IND: _sim_Store_Ind,
                                                                   AVG_USING_CHAIN_IND: _avg_Using_Chain_Ind,
                                                                   SHIP_USING_BASIS_IND: _ship_Using_Basis_Ind,
                                                                   TREND_PERCENT: _trend_Percent,
                                                                   OTS_BEGIN_CDR_RID: OTS_BEGIN_CDR_RID_Nullable,
                                                                   BALANCE_IND: _balance_Ind,
                                                                   APPLY_MIN_MAX_IND: _apply_Min_Max_Ind,
                                                                   RECONCILE_IND: _reconcile_Ind,
                                                                   INVENTORY_IND: _inventory_Ind,
                                                                   MERCH_TYPE: MERCH_TYPE,
                                                                   MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                   MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                   MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                   OTS_SHIP_TO_CDR_RID: OTS_SHIP_TO_CDR_RID_Nullable,
                                                                   GRADE_VARIABLE_TYPE: (int)this._gradeVariableType,
                                                                   BALANCE_TO_HEADER_IND: this._balanceToHeaderInd 
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

		private bool UpdateChildData()
		{
			bool UpdateSuccessful = true; 

			try
			{
				if (_dsVelocity == null)
				{
					return UpdateSuccessful;
				}
				 
				DataView dv = new DataView();
				dv.Table = _dsVelocity.Tables["Basis"];
				for (int i = 0; i < dv.Count; i++)
				{
			
                    int? BASIS_HN_RID_Nullable = null;
                    if ((int)dv[i]["BasisHNRID"] != Include.NoRID) BASIS_HN_RID_Nullable = (int)dv[i]["BasisHNRID"];

                    int? BASIS_FV_RID_Nullable = null;
                    if ((int)dv[i]["BasisFVRID"] != Include.NoRID) BASIS_FV_RID_Nullable = (int)dv[i]["BasisFVRID"];

                    int? BASIS_PH_RID_Nullable = null;
                    if ((int)dv[i]["BasisPHRID"] != Include.NoRID) BASIS_PH_RID_Nullable = (int)dv[i]["BasisPHRID"];

                    int? BASIS_PHL_SEQUENCE_Nullable = null;
                    if ((int)dv[i]["BasisPHRID"] != Include.NoRID) BASIS_PHL_SEQUENCE_Nullable = (int)dv[i]["BasisPHLSequence"];

                    StoredProcedures.MID_METHOD_VELOCITY_BASIS_INSERT.Insert(_dba,
                                                                         METHOD_RID: _velocity_Method_RID,
                                                                         BASIS_SEQUENCE: (int)dv[i]["BasisSequence"],
                                                                         BASIS_HN_RID: BASIS_HN_RID_Nullable,
                                                                         BASIS_FV_RID: BASIS_FV_RID_Nullable,
                                                                         CDR_RID: (int)dv[i]["cdrRID"],
                                                                         SALES_WEIGHT: (double)dv[i]["Weight"],
                                                                         BASIS_PH_RID: BASIS_PH_RID_Nullable,
                                                                         BASIS_PHL_SEQUENCE: BASIS_PHL_SEQUENCE_Nullable
                                                                         );
				}
	
				// BEGIN Issue 4818
//				dv.Table = _dsVelocity.Tables["SalesPeriod"];
//				for (int i = 0; i < dv.Count; i++)
//				{
//					string addSales = BuildInsertSalesPeriodStatement(dv[i]);
//					_dba.ExecuteNonQuery(addSales);
//				}
				// END Issue 4818

				dv.Table = _dsVelocity.Tables["VelocityGrade"];
				for (int i = 0; i < dv.Count; i++)
				{


                    int? MINIMUM_STOCK_Nullable = null;
                    if (dv[i]["MinStock"] != DBNull.Value) MINIMUM_STOCK_Nullable = (int)dv[i]["MinStock"];

                    int? MAXIMUM_STOCK_Nullable = null;
                    if (dv[i]["MaxStock"] != DBNull.Value) MAXIMUM_STOCK_Nullable = (int)dv[i]["MaxStock"];

                    int? MINIMUM_AD_Nullable = null;
                    if (dv[i]["MinAd"] != DBNull.Value) MINIMUM_AD_Nullable = (int)dv[i]["MinAd"];

                    StoredProcedures.MID_METHOD_VELOCITY_GRADE_INSERT.Insert(_dba,
                                                                         METHOD_RID: _velocity_Method_RID,
                                                                         BOUNDARY: (int)dv[i]["Boundary"],
                                                                         GRADE_CODE: (string)dv[i]["Grade"],
                                                                         MINIMUM_STOCK: MINIMUM_STOCK_Nullable,
                                                                         MAXIMUM_STOCK: MAXIMUM_STOCK_Nullable,
                                                                         MINIMUM_AD: MINIMUM_AD_Nullable
                                                                         );
				}

				dv.Table = _dsVelocity.Tables["SellThru"];
				for (int i = 0; i < dv.Count; i++)
				{

                    StoredProcedures.MID_METHOD_VELOCITY_SELL_THRU_INSERT.Insert(_dba,
                                                                             METHOD_RID: _velocity_Method_RID,
                                                                             VELOCITY_SELL_THRU_INDEX: (int)dv[i]["SellThruIndex"]
                                                                             );
				}

                SGLRIDList = new ArrayList();  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing

				dv.Table = _dsVelocity.Tables["GroupLevel"];
				for (int i = 0; i < dv.Count; i++)
				{
               
                    int SGL_RID;
                    int.TryParse(Convert.ToString(dv[i]["SglRID"], CultureInfo.CurrentUICulture), out SGL_RID);
                    int NO_ONHAND_RULE;
                    int.TryParse(Convert.ToString(dv[i]["NoOnHandRule"], CultureInfo.CurrentUICulture), out NO_ONHAND_RULE);
                    double? NO_ONHAND_QUANTITY_Nullable = null;
                    if (dv[i]["NoOnHandQty"] != System.DBNull.Value)
                    {
                        double NO_ONHAND_QUANTITY;
                        double.TryParse(Convert.ToString(dv[i]["NoOnHandQty"], CultureInfo.CurrentUICulture), out NO_ONHAND_QUANTITY);
                        NO_ONHAND_QUANTITY_Nullable = NO_ONHAND_QUANTITY;
                    }
                    char MATRIX_MODE_IND;
                    char.TryParse(Convert.ToString(dv[i]["ModeInd"], CultureInfo.CurrentUICulture), out MATRIX_MODE_IND);
                    int? MATRIX_MODE_AVERAGE_RULE_Nullable = null;
                    if (dv[i]["AverageRule"] != System.DBNull.Value)
                    {
                        int MATRIX_MODE_AVERAGE_RULE;
                        int.TryParse(Convert.ToString(dv[i]["AverageRule"], CultureInfo.CurrentUICulture), out MATRIX_MODE_AVERAGE_RULE);
                        MATRIX_MODE_AVERAGE_RULE_Nullable = MATRIX_MODE_AVERAGE_RULE;
                    }
                    double? MATRIX_MODE_AVERAGE_QUANTITY_Nullable = null;
                    if (dv[i]["AverageQty"] != System.DBNull.Value)
                    {
                        double MATRIX_MODE_AVERAGE_QUANTITY;
                        double.TryParse(Convert.ToString(dv[i]["AverageQty"], CultureInfo.CurrentUICulture), out MATRIX_MODE_AVERAGE_QUANTITY);
                        MATRIX_MODE_AVERAGE_QUANTITY_Nullable = MATRIX_MODE_AVERAGE_QUANTITY;
                    }
                    char MATRIX_SPREAD_IND;
                    char.TryParse(Convert.ToString(dv[i]["SpreadInd"], CultureInfo.CurrentUICulture), out MATRIX_SPREAD_IND);

                    StoredProcedures.MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT.Insert(_dba,
                                                                               METHOD_RID: _velocity_Method_RID,
                                                                               SGL_RID: SGL_RID,
                                                                               NO_ONHAND_RULE: NO_ONHAND_RULE,
                                                                               NO_ONHAND_QUANTITY: NO_ONHAND_QUANTITY_Nullable,
                                                                               MATRIX_MODE_IND: MATRIX_MODE_IND,
                                                                               MATRIX_MODE_AVERAGE_RULE: MATRIX_MODE_AVERAGE_RULE_Nullable,
                                                                               MATRIX_MODE_AVERAGE_QUANTITY: MATRIX_MODE_AVERAGE_QUANTITY_Nullable,
                                                                               MATRIX_SPREAD_IND: MATRIX_SPREAD_IND
                                                                               );
				}

				dv.Table = _dsVelocity.Tables["VelocityMatrix"];
				for (int i = 0; i < dv.Count; i++)
				{


                    bool doInsert = true;

                    // ======================================
                    // Don't insert a row if there is no data 
                    // ======================================
                    if (dv[i]["VelocityRule"] == System.DBNull.Value)
                    {
                        doInsert = false;
                    }

                    if (doInsert)
                    {
                        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                        if (SGLRIDList.Contains((Convert.ToInt32(dv[i]["SglRID"], CultureInfo.CurrentUICulture))))
                        {
                            doInsert = false;
                        }
                        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

                        if (doInsert)
                        {
            
                            int SGL_RID;
                            int.TryParse(Convert.ToString(dv[i]["SglRID"], CultureInfo.CurrentUICulture), out SGL_RID);
                            int BOUNDARY;
                            int.TryParse(Convert.ToString(dv[i]["Boundary"], CultureInfo.CurrentUICulture), out BOUNDARY);
                            int VELOCITY_SELL_THRU_INDEX;
                            int.TryParse(Convert.ToString(dv[i]["SellThruIndex"], CultureInfo.CurrentUICulture), out VELOCITY_SELL_THRU_INDEX);
                            int VELOCITY_RULE;
                            int.TryParse(Convert.ToString(dv[i]["VelocityRule"], CultureInfo.CurrentUICulture), out VELOCITY_RULE);

                            double? VELOCITY_QUANTITY_Nullable = null;
                            if (dv[i]["VelocityQty"] != System.DBNull.Value && Convert.ToDouble(dv[i]["VelocityQty"], CultureInfo.CurrentUICulture) != Include.UndefinedDouble) VELOCITY_QUANTITY_Nullable = Convert.ToDouble(dv[i]["VelocityQty"], CultureInfo.CurrentUICulture);


                            StoredProcedures.MID_METHOD_VELOCITY_MATRIX_INSERT.Insert(_dba,
                                                                                      METHOD_RID: _velocity_Method_RID,
                                                                                      SGL_RID: SGL_RID,
                                                                                      BOUNDARY: BOUNDARY,
                                                                                      VELOCITY_SELL_THRU_INDEX: VELOCITY_SELL_THRU_INDEX,
                                                                                      VELOCITY_RULE: VELOCITY_RULE,
                                                                                      VELOCITY_QUANTITY: VELOCITY_QUANTITY_Nullable
                                                                                      );
                        }
                    }
				}
			}

			catch(Exception Ex)
			{
				string exceptionMessage = Ex.Message;

				UpdateSuccessful = false;

				throw;
			}

			return UpdateSuccessful;
		}	

    	 
		public bool DeleteMethod(int method_RID, TransactionData td)
		{
			_dba = td.DBA;

			bool DeleteSuccessfull = true;

            // Begin Track #5908 – Unable to delete method
            //try
            //{
                //if (DeleteChildData())
                //{
                    //StringBuilder SQLCommand = new StringBuilder();
                    //SQLCommand.Append("DELETE FROM METHOD_VELOCITY ");
                    //SQLCommand.Append("WHERE ");
                    //SQLCommand.Append("METHOD_RID = ");
                    //SQLCommand.Append(method_RID.ToString(CultureInfo.CurrentUICulture));
                    //_dba.ExecuteNonQuery(SQLCommand.ToString());
                //    DeleteSuccessfull = true;
                //}
            //}

            //catch
            //{
            //    DeleteSuccessfull = false;

            //    throw;
            //}

            //finally
            //{
            //}
            // End Track #5908

			return DeleteSuccessfull;
		}
		
		private bool DeleteChildData()
		{
			bool DeleteSuccessfull = true;

			try
			{
        
                StoredProcedures.MID_METHOD_VELOCITY_MATRIX_DELETE.Delete(_dba, METHOD_RID: _velocity_Method_RID);
                StoredProcedures.MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE.Delete(_dba, METHOD_RID: _velocity_Method_RID);
                StoredProcedures.MID_METHOD_VELOCITY_SELL_THRU_DELETE.Delete(_dba, METHOD_RID: _velocity_Method_RID);
                StoredProcedures.MID_METHOD_VELOCITY_GRADE_DELETE.Delete(_dba, METHOD_RID: _velocity_Method_RID);
                StoredProcedures.MID_METHOD_VELOCITY_BASIS_DELETE.Delete(_dba, METHOD_RID: _velocity_Method_RID);
	
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

		public DataTable GetMethodsByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_VELOCITY_READ_FROM_NODE.Read(_dba, OTS_PLAN_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetMethodsBasisByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE.Read(_dba, BASIS_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
