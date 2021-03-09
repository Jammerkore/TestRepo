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
	/// Adds properties for OTS_Plan
	/// </summary>
	public class AllocationOverrideMethodData: MethodBaseData
	{
		private int _OverRide_HDR_RID;
		private int _Store_Filter_RID;
		private int _Grade_Week_Count;
		private char _Exceed_Maximums_Ind;
		private double _Percent_Need_Limit;
		private int  _Merch_Plan_HN_RID;
		private int _Merch_Plan_PH_RID;
		private int _Merch_Plan_PHL_SEQ;
		private int _Merch_OnHand_HN_RID;
		private int _Merch_OnHand_PH_RID;
        private int _Merch_OnHand_PHL_SEQ;
		private double _Plan_Factor_Percent;
		private double _Reserve;
		private char _Percent_Ind;
		private char _Exceed_Capacity_Ind;
		private int _All_Color_Multiple;
		private int _All_Size_Multiple;
		private int _All_Color_Maximum;
		private int _All_Color_Minimum;
		private int _Store_Group_RID;
        private int _Tab_Store_Group_RID;    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35
        private bool _useStoreGradeDefault;
		private bool _usePctNeedDefault;
		private bool _useFactorPctDefault;
		private bool _useAllColorsMinDefault;
		private bool _useAllColorsMaxDefault;
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private int _Pack_Multiple;
        private double _Pack_Rounding_1st_Pack_Pct;
        private double _Pack_Rounding_Nth_Pack_Pct;
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		private Hashtable _storeGradeHash; // TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35
		private DataSet _dsOverRide;

        private DataSet _imoDataSet; // TT#1401 - GTaylor - Reservation Stores
        private IMOMethodOverrideProfileList _imoMethodOverrideProfileList; // TT#1401 - GTaylor - Reservation Stores

		// BEGIN TT#667 - Stodd - Pre-allocate Reserve
		private double _reserveAsBulk;
		private double _reserveAsPacks;
        // END TT#667 - Stodd - Pre-allocate Reserve
        
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private eMerchandiseType _merchandiseType;
        private char _inventory_Ind;
        private int _IB_MERCH_HN_RID;
        private int _IB_MERCH_PH_RID;
        private int _IB_MERCH_PHL_SEQUENCE;
        // END TT#1287 - AGallagher - Inventory Min/Max

        // BEGIN TT#1401 - GTaylor - Reservation Stores
        private int _IMO_SG_RID;
        private bool _applyVSW = true;
        // END TT#1401 - GTaylor - Reservation Stores

        //  To Do store grades and colors and capacity by set

        private bool _merchUnspecified;      // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        private bool _onHandUnspecified;     // End TT#709 

		public int OverRide_HDR_RID
		{
			get{return _OverRide_HDR_RID;}
			set{_OverRide_HDR_RID = value;	}
		}
		public int Store_Filter_RID
		{
			get{return _Store_Filter_RID;}
			set{_Store_Filter_RID = value;	}
		}
		public int Grade_Week_Count
		{
			get{return _Grade_Week_Count;}
			set{_Grade_Week_Count = value;	}
		}
		public int Merch_Plan_HN_RID
		{
			get{return _Merch_Plan_HN_RID;}
			set{_Merch_Plan_HN_RID = value;	}
		}
		public int Merch_Plan_PH_RID
		{
			get{return _Merch_Plan_PH_RID;}
			set{_Merch_Plan_PH_RID = value;	}
		}
		public int Merch_Plan_PHL_SEQ
		{
			get{return _Merch_Plan_PHL_SEQ;}
			set{_Merch_Plan_PHL_SEQ = value; }
		}
		public int Merch_OnHand_HN_RID
		{
			get{return _Merch_OnHand_HN_RID;}
			set{_Merch_OnHand_HN_RID = value;	}
		}
		public int Merch_OnHand_PH_RID
		{
			get{return _Merch_OnHand_PH_RID;}
			set{_Merch_OnHand_PH_RID = value;	}
		}
		public int Merch_OnHand_PHL_SEQ
		{
			get{return _Merch_OnHand_PHL_SEQ;}
			set{_Merch_OnHand_PHL_SEQ = value; }
		}
		public double Plan_Factor_Percent
		{
			get{return _Plan_Factor_Percent;}
			set{_Plan_Factor_Percent = value;	}
		}
		public double Percent_Need_Limit
		{
			get{return _Percent_Need_Limit;}
			set{_Percent_Need_Limit = value;	}
		}
		public double Reserve
		{
			get{return _Reserve;}
			set{_Reserve = value;	}
		}
		public char Exceed_Maximums_Ind
		{
			get{return _Exceed_Maximums_Ind;}
			set{_Exceed_Maximums_Ind = value;	}
		}
		public char Exceed_Capacity_Ind
		{
			get{return _Exceed_Capacity_Ind;}
			set{_Exceed_Capacity_Ind = value;	}
		}
		public char Percent_Ind
		{
			get{return _Percent_Ind;}
			set{_Percent_Ind = value;	}
		}
		public int All_Color_Multiple
		{
			get{return _All_Color_Multiple;}
			set{_All_Color_Multiple = value; }
		}
		public int All_Size_Multiple
		{
			get{return _All_Size_Multiple;}
			set{_All_Size_Multiple = value;  }
		}
		public int All_Color_Maximum
		{
			get{return _All_Color_Maximum;}
			set{_All_Color_Maximum = value;  }
		}
		public int All_Color_Minimum
		{
			get{return _All_Color_Minimum;}
			set{_All_Color_Minimum = value;  }
		}
		public int Store_Group_RID
		{
			get{return _Store_Group_RID;}
			set{_Store_Group_RID = value;  }
		}
        // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        public int Tab_Store_Group_RID
        {
            get { return _Tab_Store_Group_RID; }
            set { _Tab_Store_Group_RID = value; }
        }
        // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)						 
		public bool UseStoreGradeDefault
		{
			get{return _useStoreGradeDefault;}
			set{_useStoreGradeDefault = value;  }
		}	
		public bool UsePctNeedDefault
		{
			get{return _usePctNeedDefault;}
			set{_usePctNeedDefault = value;  }
		}
		public bool UseFactorPctDefault
		{
			get{return _useFactorPctDefault;}
			set{_useFactorPctDefault = value;  }
		}
		public bool UseAllColorsMinDefault
		{
			get{return _useAllColorsMinDefault;}
			set{_useAllColorsMinDefault = value;  }
		}
		public bool UseAllColorsMaxDefault
		{
			get{return _useAllColorsMaxDefault;}
			set{_useAllColorsMaxDefault = value;  }
		}
		// BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        public int Pack_Multiple
        {
            get { return _Pack_Multiple; }
            set { _Pack_Multiple = value; }
        }
        public double Pack_Rounding_1st_Pack_Pct
        {
            get { return _Pack_Rounding_1st_Pack_Pct; }
            set { _Pack_Rounding_1st_Pack_Pct = value; }
        }
        public double Pack_Rounding_Nth_Pack_Pct
        {
            get { return _Pack_Rounding_Nth_Pack_Pct; }
            set { _Pack_Rounding_Nth_Pack_Pct = value; }
        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		
		public DataSet DSOverRide
		{
			get{return _dsOverRide;}
			set{_dsOverRide = value;  }
		}
		// BEGIN TT#667 - Stodd - Pre-allocate Reserve

        // BEGIN TT#1401 - GTaylor - Reservation Stores
        public DataSet IMODataSet
        {
            get { return _imoDataSet; }
            set { _imoDataSet = value; }
        }
        public IMOMethodOverrideProfileList IMOMethodOverrideProfileList
        {
            get { return _imoMethodOverrideProfileList; }
            set { _imoMethodOverrideProfileList = value; }
        }
        public bool ApplyVSW
        {
            get { return _applyVSW; }
            set { _applyVSW = value; }
        }
        // END TT#1401 - GTaylor - Reservation Stores

		// Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        public bool Merch_Plan_Unspecified
        {
            get { return _merchUnspecified; }
            set { _merchUnspecified = value; }
        }
        public bool Merch_OnHand_Unspecified
        {
            get { return _onHandUnspecified; }
            set { _onHandUnspecified = value; }
        }
        // End TT#709  

		public double ReserveAsBulk
		{
			get { return _reserveAsBulk; }
			set { _reserveAsBulk = value; }
		}
		public double ReserveAsPacks
		{
			get { return _reserveAsPacks; }
			set { _reserveAsPacks = value; }
		}
        		// END TT#667 - Stodd - Pre-allocate Reserve

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

        public int IB_MERCH_HN_RID
        {
            get
            {
                return _IB_MERCH_HN_RID;
            }

            set
            {
                _IB_MERCH_HN_RID = value;
            }
        }

        public int IB_MERCH_PH_RID
        {
            get
            {
                return _IB_MERCH_PH_RID;
            }

            set
            {
                _IB_MERCH_PH_RID = value;
            }
        }

        public int IB_MERCH_PHL_SEQ
        {
            get
            {
                return _IB_MERCH_PHL_SEQUENCE;
            }

            set
            {
                _IB_MERCH_PHL_SEQUENCE = value;
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

        // BEGIN TT#1401 - GTaylor - Reservation Stores
        public int IMO_SG_RID
        {
            get
            {
                return _IMO_SG_RID;
            }
            set
            { 
                _IMO_SG_RID = value;
            }
        }
        // END TT#1401 - GTaylor - Reservation Stores

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

		
		/// <summary>
		/// Creates an instance of the AllocationOverrideMethodData class.
		/// </summary>
		public AllocationOverrideMethodData()
		{
		}

		/// <summary>
		/// Creates an instance of the AllocationOverrideMethodData class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public AllocationOverrideMethodData(int aMethodRID)
		{
			_OverRide_HDR_RID = Include.NoRID;
		}

		/// <summary>
		/// Creates an instance of the AllocationOverrideMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public AllocationOverrideMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_OverRide_HDR_RID = Include.NoRID;
		}
		public AllocationOverrideMethodData(TransactionData td)
		{
			_dba = td.DBA;
			_OverRide_HDR_RID = Include.NoRID;
		}
		
		public bool PopulateAllocationOverride(int method_RID)
		{
			try
			{
				if (PopulateMethod(method_RID))
				{
					_OverRide_HDR_RID = method_RID;
                   

					DataTable dtOverRide = MIDEnvironment.CreateDataTable();
                    dtOverRide = StoredProcedures.MID_METHOD_OVERRIDE_READ.Read(_dba, METHOD_RID: method_RID);
					if(dtOverRide.Rows.Count != 0)
					{
						DataRow dr = dtOverRide.Rows[0];
						
						if (dr["STORE_FILTER_RID"] != System.DBNull.Value)
							_Store_Filter_RID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
						else
							_Store_Filter_RID = Include.NoRID;
						
						if (dr["STORE_GRADE_TIMEFRAME"] != System.DBNull.Value)
						{
							_Grade_Week_Count = Convert.ToInt32(dr["STORE_GRADE_TIMEFRAME"], CultureInfo.CurrentUICulture);
							_useStoreGradeDefault = false;
						}
						else
						{
							_useStoreGradeDefault = true;
						}
						if (dr["EXCEED_MAX_IND"] != System.DBNull.Value)
							_Exceed_Maximums_Ind = Convert.ToChar(dr["EXCEED_MAX_IND"], CultureInfo.CurrentUICulture);	

						if (dr["PERCENT_NEED_LIMIT"] != System.DBNull.Value)
						{
							_Percent_Need_Limit = Convert.ToDouble(dr["PERCENT_NEED_LIMIT"], CultureInfo.CurrentUICulture);	
							_usePctNeedDefault = false;
						}
						else
							_usePctNeedDefault = true;

						if (dr["RESERVE"] != System.DBNull.Value)
							_Reserve = Convert.ToDouble(dr["RESERVE"], CultureInfo.CurrentUICulture);
						else
							_Reserve = Include.UndefinedReserve;
						
						if (dr["PERCENT_IND"] != System.DBNull.Value)
							_Percent_Ind = Convert.ToChar(dr["PERCENT_IND"], CultureInfo.CurrentUICulture);				

						if (dr["MERCH_HN_RID"] != System.DBNull.Value)
							_Merch_Plan_HN_RID = Convert.ToInt32(dr["MERCH_HN_RID"], CultureInfo.CurrentUICulture);
						else
							_Merch_Plan_HN_RID = Include.NoRID;
						if (dr["MERCH_PH_RID"] != System.DBNull.Value)
						{
							_Merch_Plan_PH_RID = Convert.ToInt32(dr["MERCH_PH_RID"], CultureInfo.CurrentUICulture);
							_Merch_Plan_PHL_SEQ = Convert.ToInt32(dr["MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
						}
						else
						{
							_Merch_Plan_PH_RID = Include.NoRID;
							_Merch_Plan_PHL_SEQ = 0;		
						}	
						if (dr["ON_HAND_HN_RID"] != System.DBNull.Value)
							_Merch_OnHand_HN_RID = Convert.ToInt32(dr["ON_HAND_HN_RID"], CultureInfo.CurrentUICulture);
						else
							_Merch_OnHand_HN_RID = Include.NoRID;
						if (dr["ON_HAND_PH_RID"] != System.DBNull.Value)
						{
							_Merch_OnHand_PH_RID = Convert.ToInt32(dr["ON_HAND_PH_RID"], CultureInfo.CurrentUICulture);
							_Merch_OnHand_PHL_SEQ = Convert.ToInt32(dr["ON_HAND_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
						}
						else
						{
							_Merch_OnHand_PH_RID = Include.NoRID;
							_Merch_OnHand_PHL_SEQ = 0;		
						}	
						if (dr["ON_HAND_FACTOR"] != System.DBNull.Value)
						{
							_Plan_Factor_Percent = Convert.ToDouble(dr["ON_HAND_FACTOR"], CultureInfo.CurrentUICulture);
							_useFactorPctDefault = false;
						}
						else
							_useFactorPctDefault = true;

						if (dr["COLOR_MULT"] != System.DBNull.Value)
							_All_Color_Multiple = Convert.ToInt32(dr["COLOR_MULT"], CultureInfo.CurrentUICulture);

						if (dr["SIZE_MULT"] != System.DBNull.Value)
							_All_Size_Multiple = Convert.ToInt32(dr["SIZE_MULT"], CultureInfo.CurrentUICulture);

						if (dr["ALL_COLOR_MIN"] != System.DBNull.Value)
						{
							_All_Color_Minimum = Convert.ToInt32(dr["ALL_COLOR_MIN"], CultureInfo.CurrentUICulture);
							_useAllColorsMinDefault = false;
						}
						else
							_useAllColorsMinDefault = true;

						if (dr["ALL_COLOR_MAX"] != System.DBNull.Value)
						{
							_All_Color_Maximum = Convert.ToInt32(dr["ALL_COLOR_MAX"], CultureInfo.CurrentUICulture);
							_useAllColorsMaxDefault = false;
						}
						else
							_useAllColorsMaxDefault = true;

						if (dr["SG_RID"] != System.DBNull.Value)
							_Store_Group_RID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
						else
							_Store_Group_RID = Include.NoRID;

						if (dr["EXCEED_CAPACITY"] != System.DBNull.Value)
							_Exceed_Capacity_Ind = Convert.ToChar(dr["EXCEED_CAPACITY"], CultureInfo.CurrentUICulture);

						// BEGIN TT#667 - Stodd - Pre-allocate Reserve
						_reserveAsBulk = double.Parse(dr["RESERVE_AS_BULK"].ToString());
						_reserveAsPacks = double.Parse(dr["RESERVE_AS_PACKS"].ToString());
						// BEGIN TT#667 - Stodd - Pre-allocate Reserve


                        // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                        if (dr["MERCH_UNSPECIFIED"] != System.DBNull.Value)
                        {
                            _merchUnspecified = Include.ConvertCharToBool(Convert.ToChar(dr["MERCH_UNSPECIFIED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            _merchUnspecified = false;
                        }
                        if (dr["ON_HAND_UNSPECIFIED"] != System.DBNull.Value)
                        {
                            _onHandUnspecified = Include.ConvertCharToBool(Convert.ToChar(dr["ON_HAND_UNSPECIFIED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            _onHandUnspecified = false;
                        }
                        // End TT#709 
						
						// BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                        if (dr["STORE_GRADES_SG_RID"] != System.DBNull.Value)
                            _Tab_Store_Group_RID = Convert.ToInt32(dr["STORE_GRADES_SG_RID"], CultureInfo.CurrentUICulture);
                        else
                            _Tab_Store_Group_RID = Include.NoRID;
                        // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                        if (dr["INVENTORY_IND"] != System.DBNull.Value)
                            _inventory_Ind = Convert.ToChar(dr["INVENTORY_IND"], CultureInfo.CurrentUICulture);
                        else
                            _inventory_Ind = 'A';
                        if (dr["IB_MERCH_HN_RID"] != System.DBNull.Value)
                            _IB_MERCH_HN_RID = Convert.ToInt32(dr["IB_MERCH_HN_RID"], CultureInfo.CurrentUICulture);
                        else
                            _IB_MERCH_HN_RID = Include.NoRID;
                        if (dr["IB_MERCH_PH_RID"] != System.DBNull.Value)
                        {
                            _IB_MERCH_PH_RID = Convert.ToInt32(dr["IB_MERCH_PH_RID"], CultureInfo.CurrentUICulture);
                            _IB_MERCH_PHL_SEQUENCE = Convert.ToInt32(dr["IB_MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            _IB_MERCH_PH_RID = Include.NoRID;
                            _IB_MERCH_PHL_SEQUENCE = 0;
                        }
                        // END TT#1287 - AGallagher - Inventory Min/Max
				        // BEGIN TT#1401 - GTaylor - Reservation Stores
                        if (dr["IMO_SG_RID"] != System.DBNull.Value)
                            _IMO_SG_RID = Convert.ToInt32(dr["IMO_SG_RID"], CultureInfo.CurrentUICulture);
                        else
                            _IMO_SG_RID = Include.NoRID;
                        // END TT#1401 - GTaylor - Reservation Stores

						// BEGIN TT#1401 - stodd
						if (dr["IMO_APPLY_VSW"] != System.DBNull.Value)
							_applyVSW = Convert.ToBoolean(dr["IMO_APPLY_VSW"], CultureInfo.CurrentUICulture);
						else
							_applyVSW = true;
						// END TT#1401 - stodd
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
		public DataSet GetOverRideChildData()
		{ 
			_dsOverRide = MIDEnvironment.CreateDataSet();

            DataTable dtColors = SetupColorTable();
			DataTable dtStoreGrades = SetupStoreGradeTable();
			DataTable dtCapacity = SetupCapacityTable();
            DataTable dtPackRounding = SetupPackRoundingTable();  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
			DataTable dtStoreGradeBoundary = GetStoreGradesGrades(Method_RID);  // TT#616 - Stodd - Allocation - Add Attribute Sets (#35)

			_dsOverRide.Tables.Add(dtColors);
			_dsOverRide.Tables.Add(dtStoreGrades);
			_dsOverRide.Tables.Add(dtCapacity);
            _dsOverRide.Tables.Add(dtPackRounding);   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
			_dsOverRide.Tables.Add(dtStoreGradeBoundary);   // TT#616 - Stodd - Allocation - Add Attribute Sets (#35)

			return _dsOverRide;
		}
		private DataTable SetupColorTable()
		{
			DataTable dt = MIDEnvironment.CreateDataTable("Colors");
			dt.Columns.Add("RowPosition",System.Type.GetType("System.Int32"));
			dt.Columns.Add("ColorCodeRID", System.Type.GetType("System.Int32")); 
			dt.Columns.Add("Minimum", System.Type.GetType("System.Int32")); 
			dt.Columns.Add("Maximum", System.Type.GetType("System.Int32")); 

           
            dt = StoredProcedures.MID_METHOD_OVERRIDE_COLOR_MINMAX_READ.Read(_dba, METHOD_RID: _OverRide_HDR_RID);
			dt.TableName = "Colors";
			dt.Columns[0].ColumnName = "RowPosition";
			dt.Columns[1].ColumnName = "Color";
			dt.Columns[2].ColumnName = "Minimum";
			dt.Columns[2].Caption = "Minimum";
			dt.Columns[2].ReadOnly = false;
			dt.Columns[2].Unique = false;
			dt.Columns[2].AllowDBNull = true;
			dt.Columns[3].ColumnName = "Maximum";
			dt.Columns[3].Caption = "Maximum";
			dt.Columns[3].ReadOnly = false;
			dt.Columns[3].Unique = false;
			dt.Columns[3].AllowDBNull = true;
			int rowpos = 0;
			foreach (DataRow row in dt.Rows)
			{
				row["RowPosition"] = rowpos;
				rowpos++;
			}
			return dt;
		}
		private DataTable SetupStoreGradeTable()
		{
			// BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

			DataTable dtStoreGrades = MIDEnvironment.CreateDataTable("StoreGrades");
			DataTable dtGradeBoundary = MIDEnvironment.CreateDataTable("GradeBoundary");
			DataTable dtGradeValues = MIDEnvironment.CreateDataTable("GradeValues");
			DataTable dtStoreGroupLevels = MIDEnvironment.CreateDataTable("StoreGroupLevels");

			dtStoreGrades.Columns.Add("RowPosition", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("SGLRID", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("Boundary", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("GradeCode", System.Type.GetType("System.String"));
			dtStoreGrades.Columns.Add("MinStock", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MaxStock", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MinAd", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MinColor", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MaxColor", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("ShipUpTo", System.Type.GetType("System.Int32"));  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

			//======================
			// Get Grade Boundaries
			//======================
			dtGradeBoundary = GetStoreGradesGrades(Method_RID);

			//======================
			// Get Grade Values
			//======================        
            dtGradeValues = StoredProcedures.MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ.Read(_dba, METHOD_RID: Method_RID);

			//==============================
			// Get Store Group Levels (Sets)
			//==============================
            dtStoreGroupLevels = StoredProcedures.MID_STORE_GROUP_LEVEL_READ_FROM_GROUP.Read(_dba, SG_RID: _Tab_Store_Group_RID);

			//===========================
			// Build Store Grades Table
			//===========================
			int rowpos = 0;
			foreach (DataRow sglRow in dtStoreGroupLevels.Rows)
			{
				int sglRid = int.Parse(sglRow["SGL_RID"].ToString());
				foreach (DataRow boundaryRow in dtGradeBoundary.Rows)
				{
					string gradeCode = boundaryRow["GradeCode"].ToString();
					int boundary = int.Parse(boundaryRow["Boundary"].ToString());

					DataRow newRow = dtStoreGrades.NewRow();
					newRow["RowPosition"] = rowpos++;
					newRow["Boundary"] = boundary;
					newRow["GradeCode"] = gradeCode;
					newRow["SGLRID"] = sglRid;

					DataRow [] valMatch = dtGradeValues.Select("SGL_RID = " + sglRid.ToString() + " and Boundary = " + boundary);
					if (valMatch.Length > 0)
					{
						newRow["MinStock"] = valMatch[0]["MinStock"];
						newRow["MaxStock"] = valMatch[0]["MaxStock"];
						newRow["MinAd"] = valMatch[0]["MinAd"];
						newRow["MinColor"] = valMatch[0]["MinColor"];
						newRow["MaxColor"] = valMatch[0]["MaxColor"];
						newRow["ShipUpTo"] = valMatch[0]["ShipUpTo"];
					}
					else
					{
						newRow["MinStock"] = DBNull.Value;
						newRow["MaxStock"] = DBNull.Value;
						newRow["MinAd"] = DBNull.Value;
						newRow["MinColor"] = DBNull.Value;
						newRow["MaxColor"] = DBNull.Value;
						newRow["ShipUpTo"] = DBNull.Value;
					}
					dtStoreGrades.Rows.Add(newRow);
				}
			}

			//======================
			// Change Column Names
			//======================
			dtStoreGrades.Columns["RowPosition"].ColumnName = "RowPosition";
			dtStoreGrades.Columns["SGLRID"].ColumnName = "SGLRID";  // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
			dtStoreGrades.Columns["GradeCode"].ColumnName = "Grade";
			dtStoreGrades.Columns["Boundary"].ColumnName = "Boundary";
			dtStoreGrades.Columns["MinStock"].ColumnName = "Allocation Min";
			dtStoreGrades.Columns["MaxStock"].ColumnName = "Allocation Max";
			dtStoreGrades.Columns["MinAd"].ColumnName = "Min Ad";
			dtStoreGrades.Columns["MinColor"].ColumnName = "Color Min";
			dtStoreGrades.Columns["MaxColor"].ColumnName = "Color Max";
			dtStoreGrades.Columns["ShipUpTo"].ColumnName = "Ship Up To";   // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

			return dtStoreGrades;

			// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		}

        // BEGIN TT#1401 - GTaylor - Reservation Stores
        public bool GetApplyVSW()
        {
			// BEGIN TT#1401 - stodd
			//bool applyVSW = false;
			//int _methodRID = OverRide_HDR_RID;
			
			//try
			//{
			//    StringBuilder SQLCommand = new StringBuilder();
			//    SQLCommand.Append("SELECT IMO_APPLY_VSW FROM METHOD_OVERRIDE ");
			//    SQLCommand.Append("WHERE METHOD_RID = " + _methodRID.ToString(CultureInfo.CurrentUICulture));

			//    DataTable dtTemp = MIDEnvironment.CreateDataTable("GetApplyVSW");

			//    dtTemp = _dba.ExecuteQuery(SQLCommand.ToString());
			//    if (dtTemp.Rows.Count > 0)
			//    {
			//       applyVSW = Convert.ToBoolean((dtTemp.Rows[0]["IMO_APPLY_VSW"].Equals(DBNull.Value)) ? true : dtTemp.Rows[0]["IMO_APPLY_VSW"]);
			//    }
			//    else 
			//    {
			//        applyVSW = false;
			//    }
			//    dtTemp.Dispose();
			//}
			//catch
			//{
			//    throw;
			//}
			return _applyVSW;
			// END TT#1401 - stodd
        }

        public IMOMethodOverrideProfileList GetMethodOverrideIMO(int _methodRID)
        {
            int Store_RID, Method_RID, Min_Ship_Qty, Max_Value;
            double Pct_Pck_Threshold;
            bool ApplyVSW;

            IMOMethodOverrideProfileList imomopl; 
            
            try
            {
                imomopl = new IMOMethodOverrideProfileList(eProfileType.IMO);

                if (_methodRID == Include.NoRID) _methodRID = _OverRide_HDR_RID;

                DataTable dtMethodOverride = MIDEnvironment.CreateDataTable("VSW");
                dtMethodOverride = StoredProcedures.MID_METHOD_OVERRIDE_IMO_READ.Read(_dba, METHOD_RID: _methodRID);
                foreach (DataRow row in dtMethodOverride.Rows)
                {
                    Store_RID = Convert.ToInt32((row["ST_RID"].Equals(DBNull.Value)) ? Include.NoRID : row["ST_RID"]);
                    Method_RID = Convert.ToInt32((row["METHOD_RID"].Equals(DBNull.Value)) ? Include.NoRID : row["METHOD_RID"]);
                    Min_Ship_Qty = Convert.ToInt32((row["IMO_MIN_SHIP_QTY"].Equals(DBNull.Value)) ? Include.NoRID : row["IMO_MIN_SHIP_QTY"]);
                    Pct_Pck_Threshold = Convert.ToDouble((row["IMO_PCT_PK_THRSHLD"].Equals(DBNull.Value)) ? .50 : row["IMO_PCT_PK_THRSHLD"]);
                    Max_Value = Convert.ToInt32((row["IMO_MAX_VALUE"].Equals(DBNull.Value)) ? Include.NoRID : row["IMO_MAX_VALUE"]);
                    ApplyVSW = Convert.ToBoolean((row["IMO_APPLY_VSW"].Equals(DBNull.Value)) ? true : row["IMO_APPLY_VSW"]);

                    IMOMethodOverrideProfile imomop = new IMOMethodOverrideProfile(Store_RID);
                    if (!imomopl.Contains(Store_RID))
                    {
                        imomop.IMOMaxValue = Max_Value;
                        imomop.IMOMethodRID = Method_RID;
                        imomop.IMOMinShipQty = Min_Ship_Qty;
                        imomop.IMOPackQty = Pct_Pck_Threshold;
                        imomop.IMOStoreRID = Store_RID;
                        imomop.IMO_Apply_VSW = ApplyVSW;

                        imomopl.Add(imomop);
                    }
                }
                dtMethodOverride.Dispose();
            }
            catch
            {
                throw;
            }
            return imomopl;
        }
        // END TT#1401 - GTaylor - Reservation Stores

		private DataTable GetStoreGradesGrades(int methodRid)
		{
            //Begin TT#1248-MD -jsobek -Error when processing Allocation Override Method
			//DataTable dtGradeValues = MIDEnvironment.CreateDataTable("GradeBoundary");
            DataTable dtGradeValues = StoredProcedures.MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ.Read(_dba, METHOD_RID: _OverRide_HDR_RID);
            dtGradeValues.TableName = "GradeBoundary";
            //End TT#1248-MD -jsobek -Error when processing Allocation Override Method
			return dtGradeValues;
		}


		private DataTable SetupCapacityTable()
		{
			DataTable dt = MIDEnvironment.CreateDataTable("Capacity");
			
			dt.Columns.Add("SglRID",		System.Type.GetType("System.Int32")); 
			dt.Columns.Add("SetName",		System.Type.GetType("System.String"));
			dt.Columns.Add("ExceedCapacity",System.Type.GetType("System.String"));
			dt.Columns.Add("ExceedBy",	System.Type.GetType("System.Double")); 
			//dt.Columns.Add("ExceedCapacityBool",System.Type.GetType("System.Boolean"));

           
            dt = StoredProcedures.MID_METHOD_OVERRIDE_CAPACITY_READ.Read(_dba, METHOD_RID: _OverRide_HDR_RID);
			dt.TableName = "Capacity";
			dt.Columns[0].ColumnName = "SglRID";
			dt.Columns[1].ColumnName = "Set";
			dt.Columns[2].ColumnName = "ExceedChar";
			dt.Columns[3].ColumnName = "Exceed by %";

			return dt;
		}

        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private DataTable SetupPackRoundingTable()
        {
            DataTable dt = MIDEnvironment.CreateDataTable("PackRounding");
            dt.Columns.Add("PackMultiple", System.Type.GetType("System.Int32"));
            dt.Columns.Add("FstPack", System.Type.GetType("System.Int32"));
            dt.Columns.Add("NthPack", System.Type.GetType("System.Int32"));
           
            dt = StoredProcedures.MID_METHOD_OVERRIDE_PACK_ROUNDING_READ.Read(_dba, METHOD_RID: _OverRide_HDR_RID);

            dt.TableName = "PackRounding";
            dt.Columns[0].ColumnName = "PackText";
            dt.Columns[0].Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Multiple);
            dt.Columns[0].ReadOnly = false;
            dt.Columns[0].Unique = true;
            dt.Columns[0].AllowDBNull = false;
            dt.Columns[1].ColumnName = "FstPack";
            dt.Columns[1].DataType = System.Type.GetType("System.Double");
            dt.Columns[1].Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_1st_Pack);
            dt.Columns[1].ReadOnly = false;
            dt.Columns[1].Unique = false;
            dt.Columns[1].AllowDBNull = true;
            dt.Columns[2].ColumnName = "NthPack";
            dt.Columns[2].DataType = System.Type.GetType("System.Double");
            dt.Columns[2].Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Nth_Pack);
            dt.Columns[2].ReadOnly = false;
            dt.Columns[2].Unique = false;
            dt.Columns[2].AllowDBNull = true;
            dt.Columns[3].ColumnName = "PackMultiple";
            dt.Columns[3].Caption = "Real PM";
            dt.Columns[3].ReadOnly = false;
            dt.Columns[3].Unique = false;
            dt.Columns[3].AllowDBNull = false;

            if (dt.Rows.Count == 0)
            {
                DataRow newrow = dt.NewRow();
                //newrow["METHOD_RID"] = _OverRide_HDR_RID.ToString(CultureInfo.CurrentUICulture);
                newrow["PackText"] = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_All_Generic_Packs);
                GlobalOptions opts = new GlobalOptions();
                DataTable dto = opts.GetGlobalOptions();
                DataRow dr = dto.Rows[0];
                newrow["FstPack"] = dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"];
                newrow["NthPack"] = dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"];
                newrow["PackMultiple"] = -1;
                dt.Rows.Add(newrow);
                dt.AcceptChanges();
            }

            return dt;
        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)

		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;
			try
			{	
				OverRide_HDR_RID = method_RID;
              

                int? STORE_FILTER_RID_Nullable = null;
                if (Store_Filter_RID != Include.NoRID) STORE_FILTER_RID_Nullable = Store_Filter_RID;

                int? STORE_GRADE_TIMEFRAME_Nullable = null;
                if (UseStoreGradeDefault == false) STORE_GRADE_TIMEFRAME_Nullable = Grade_Week_Count;

                double? PERCENT_NEED_LIMIT_Nullable = null;
                if (UsePctNeedDefault == false) PERCENT_NEED_LIMIT_Nullable = Percent_Need_Limit;

                double? RESERVE_Nullable = null;
                if (Reserve != Include.UndefinedReserve) RESERVE_Nullable = Reserve;

                char? PERCENT_IND_Nullable = null;
                if (Reserve != Include.UndefinedReserve) PERCENT_IND_Nullable = Percent_Ind;

                int? MERCH_HN_RID_Nullable = null;
                if (Merch_Plan_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = Merch_Plan_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (Merch_Plan_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = Merch_Plan_PH_RID;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (Merch_Plan_PH_RID != Include.NoRID) MERCH_PHL_SEQUENCE_Nullable = Merch_Plan_PHL_SEQ;

                int? ON_HAND_HN_RID_Nullable = null;
                if (Merch_OnHand_HN_RID != Include.NoRID) ON_HAND_HN_RID_Nullable = Merch_OnHand_HN_RID;

                int? ON_HAND_PH_RID_Nullable = null;
                if (Merch_OnHand_PH_RID != Include.NoRID) ON_HAND_PH_RID_Nullable = Merch_OnHand_PH_RID;

                int? ON_HAND_PHL_SEQUENCE_Nullable = null;
                if (Merch_OnHand_PH_RID != Include.NoRID) ON_HAND_PHL_SEQUENCE_Nullable = Merch_OnHand_PHL_SEQ;

                double? ON_HAND_FACTOR_Nullable = null;
                if (UseFactorPctDefault == false) ON_HAND_FACTOR_Nullable = Plan_Factor_Percent;

                int? COLOR_MULT_Nullable = null;
                if (All_Color_Multiple != Include.Undefined) COLOR_MULT_Nullable = All_Color_Multiple;

                int? SIZE_MULT_Nullable = null;
                if (All_Size_Multiple != Include.Undefined) SIZE_MULT_Nullable = All_Size_Multiple;

                int? ALL_COLOR_MIN_Nullable = null;
                if (UseAllColorsMinDefault == false) ALL_COLOR_MIN_Nullable = All_Color_Minimum;

                int? ALL_COLOR_MAX_Nullable = null;
                if (UseAllColorsMaxDefault == false) ALL_COLOR_MAX_Nullable = All_Color_Maximum;

                int? SG_RID_Nullable = null;
                if (Tab_Store_Group_RID != Include.NoRID) SG_RID_Nullable = Tab_Store_Group_RID;

                int? STORE_GRADES_SG_RID_Nullable = null;
                if (Tab_Store_Group_RID != Include.NoRID) STORE_GRADES_SG_RID_Nullable = Tab_Store_Group_RID;

                int? IB_MERCH_HN_RID_Nullable = null;
                if (IB_MERCH_HN_RID != Include.NoRID) IB_MERCH_HN_RID_Nullable = IB_MERCH_HN_RID;

                int? IB_MERCH_PH_RID_Nullable = null;
                if (IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PH_RID_Nullable = IB_MERCH_PH_RID;

                int? IB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PHL_SEQUENCE_Nullable = IB_MERCH_PHL_SEQ;

                int? IMO_SG_RID_Nullable = null;
                if (IMO_SG_RID != Include.NoRID) IMO_SG_RID_Nullable = IMO_SG_RID;

                    StoredProcedures.MID_METHOD_OVERRIDE_INSERT.Insert(_dba,
                                                                       METHOD_RID: method_RID,
                                                                       STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                       STORE_GRADE_TIMEFRAME: STORE_GRADE_TIMEFRAME_Nullable,
                                                                       EXCEED_MAX_IND: Exceed_Maximums_Ind,
                                                                       PERCENT_NEED_LIMIT: PERCENT_NEED_LIMIT_Nullable,
                                                                       RESERVE: RESERVE_Nullable,
                                                                       PERCENT_IND: PERCENT_IND_Nullable,
                                                                       MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                       MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                       MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                       ON_HAND_HN_RID: ON_HAND_HN_RID_Nullable,
                                                                       ON_HAND_PH_RID: ON_HAND_PH_RID_Nullable,
                                                                       ON_HAND_PHL_SEQUENCE: ON_HAND_PHL_SEQUENCE_Nullable,
                                                                       ON_HAND_FACTOR: ON_HAND_FACTOR_Nullable,
                                                                       COLOR_MULT: COLOR_MULT_Nullable,
                                                                       SIZE_MULT: SIZE_MULT_Nullable,
                                                                       ALL_COLOR_MIN: ALL_COLOR_MIN_Nullable,
                                                                       ALL_COLOR_MAX: ALL_COLOR_MAX_Nullable,
                                                                       SG_RID: SG_RID_Nullable,
                                                                       EXCEED_CAPACITY: Exceed_Capacity_Ind,
                                                                       MERCH_UNSPECIFIED: Include.ConvertBoolToChar(Merch_Plan_Unspecified),
                                                                       ON_HAND_UNSPECIFIED: Include.ConvertBoolToChar(Merch_OnHand_Unspecified),
                                                                       RESERVE_AS_BULK: ReserveAsBulk,
                                                                       RESERVE_AS_PACKS: ReserveAsPacks,
                                                                       STORE_GRADES_SG_RID: STORE_GRADES_SG_RID_Nullable,
                                                                       INVENTORY_IND: _inventory_Ind,
                                                                       IB_MERCH_TYPE: 3,
                                                                       IB_MERCH_HN_RID: IB_MERCH_HN_RID_Nullable,
                                                                       IB_MERCH_PH_RID: IB_MERCH_PH_RID_Nullable,
                                                                       IB_MERCH_PHL_SEQUENCE: IB_MERCH_PHL_SEQUENCE_Nullable,
                                                                       IMO_SG_RID: IMO_SG_RID_Nullable
                                                                       );
				
				if (UpdateChildData())
                    InsertSuccessful = true;
				else
					InsertSuccessful = false;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}
		
		private bool UpdateChildData()
		{
			bool UpdateSuccessful = true; 
			try
			{
				if (_dsOverRide == null)
					return UpdateSuccessful;
				 
				DataView dv = new DataView();
				dv.Table = _dsOverRide.Tables["Colors"];
				for (int i = 0; i < dv.Count; i++)
				{
					//string addColor = BuildInsertColorStatement(dv[i]);
					//_dba.ExecuteNonQuery(addColor);
                    int? COLOR_MIN_Nullable = null;
                    if (dv[i]["Minimum"] != System.DBNull.Value) COLOR_MIN_Nullable = Convert.ToInt32(dv[i]["Minimum"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                    int? COLOR_MAX_Nullable = null;
                    if (dv[i]["Maximum"] != System.DBNull.Value) COLOR_MAX_Nullable = Convert.ToInt32(dv[i]["Maximum"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                    StoredProcedures.MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT.Insert(_dba,
                                                                                METHOD_RID: OverRide_HDR_RID,
                                                                                COLOR_CODE_RID: (int)dv[i]["Color"],
                                                                                COLOR_MIN: COLOR_MIN_Nullable,
                                                                                COLOR_MAX: COLOR_MAX_Nullable
                                                                                );
				}
	
				dv.Table = _dsOverRide.Tables["StoreGrades"];
				// BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				//===========================
				// Store Grade Boundaries
				//===========================
				InitGradeBoundaryHash();
				for (int i = 0; i < dv.Count; i++)
				{
                    //string addGrade = BuildInsertGradeBoundaryStatement(dv[i]);
                    //if (addGrade != null)
                    //{
                    //    _dba.ExecuteNonQuery(addGrade);
                    //}
                    bool doAddGrade = true;
                    //===============================================================
                    // If we already have processed the grade, we don't build a row.
                    //===============================================================
                    string gradeCode = dv[i]["Grade"].ToString();
                    if (_storeGradeHash.ContainsKey(gradeCode))
                    {
                        doAddGrade = false;
                    }

                    if (doAddGrade)
                    {
                        _storeGradeHash.Add(gradeCode, gradeCode); //TT#1248-MD -jsobek -Error when processing Allocation Override Method

                        string GRADE_CODE_Nullable = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                        if (dv[i]["Grade"] != System.DBNull.Value) GRADE_CODE_Nullable = (string)dv[i]["Grade"];
                        StoredProcedures.MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT.Insert(_dba,
                                                                                             METHOD_RID: OverRide_HDR_RID,
                                                                                             BOUNDARY: Convert.ToInt32(dv[i]["Boundary"]), //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message
                                                                                             GRADE_CODE: GRADE_CODE_Nullable
                                                                                             );
                    }
				}
				//=====================
				// Store Grade Values
				//=====================
				for (int i = 0; i < dv.Count; i++)
				{
                    //string addGrade = BuildInsertGradeValueStatement(dv[i]);
                    //if (addGrade != null)
                    //{
                    //    _dba.ExecuteNonQuery(addGrade);
                    //}

                    bool doAddGrade = true;
                    //====================================================
                    // If not values are populated, we don't build a row.
                    //====================================================
                    if (dv[i]["Allocation Min"] == DBNull.Value
                        && dv[i]["Allocation Max"] == DBNull.Value
                        && dv[i]["Min Ad"] == DBNull.Value
                        && dv[i]["Color Min"] == DBNull.Value
                        && dv[i]["Color Max"] == DBNull.Value
                        && dv[i]["Ship Up To"] == DBNull.Value)
                    {
                        doAddGrade = false;
                    }

                    if (doAddGrade)
                    {
                        int? MINIMUM_STOCK_Nullable = null;
                        if (dv[i]["Allocation Min"] != System.DBNull.Value) MINIMUM_STOCK_Nullable = Convert.ToInt32(dv[i]["Allocation Min"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                        int? MAXIMUM_STOCK_Nullable = null;
                        if (dv[i]["Allocation Max"] != System.DBNull.Value) MAXIMUM_STOCK_Nullable = Convert.ToInt32(dv[i]["Allocation Max"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                        int? MINIMUM_AD_Nullable = null;
                        if (dv[i]["Min Ad"] != System.DBNull.Value) MINIMUM_AD_Nullable = Convert.ToInt32(dv[i]["Min Ad"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                        int? MINIMUM_COLOR_Nullable = null;
                        if (dv[i]["Color Min"] != System.DBNull.Value) MINIMUM_COLOR_Nullable = Convert.ToInt32(dv[i]["Color Min"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                        int? MAXIMUM_COLOR_Nullable = null;
                        if (dv[i]["Color Max"] != System.DBNull.Value) MAXIMUM_COLOR_Nullable = Convert.ToInt32(dv[i]["Color Max"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                        int? SHIP_UP_TO_Nullable = null;
                        if (dv[i]["Ship Up To"] != System.DBNull.Value) SHIP_UP_TO_Nullable = Convert.ToInt32(dv[i]["Ship Up To"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                        int? SGL_RID_Nullable = null;
                        if (dv[i]["SGLRID"] != System.DBNull.Value) SGL_RID_Nullable = (int)dv[i]["SGLRID"];
                        
                        StoredProcedures.MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT.Insert(_dba,
                                                                                       METHOD_RID: OverRide_HDR_RID,
                                                                                       BOUNDARY: (int)dv[i]["Boundary"],
                                                                                       MINIMUM_STOCK: MINIMUM_STOCK_Nullable,
                                                                                       MAXIMUM_STOCK: MAXIMUM_STOCK_Nullable,
                                                                                       MINIMUM_AD: MINIMUM_AD_Nullable,
                                                                                       MINIMUM_COLOR: MINIMUM_COLOR_Nullable,
                                                                                       MAXIMUM_COLOR: MAXIMUM_COLOR_Nullable,
                                                                                       SHIP_UP_TO: SHIP_UP_TO_Nullable,
                                                                                       SGL_RID: SGL_RID_Nullable
                                                                                       );
                    }
				}
				// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				dv.Table = _dsOverRide.Tables["Capacity"];
				for (int i = 0; i < dv.Count; i++)
				{
                    //string addGrade = BuildInsertCapacityStatement( dv[i]);
                    //_dba.ExecuteNonQuery(addGrade);
                    double? EXCEED_BY_Nullable = null;
                    if (dv[i]["Exceed by %"] != System.DBNull.Value) EXCEED_BY_Nullable = Convert.ToDouble(dv[i]["Exceed by %"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message
                    StoredProcedures.MID_METHOD_OVERRIDE_CAPACITY_INSERT.Insert(_dba,
                                                                            METHOD_RID: OverRide_HDR_RID,
                                                                            SGL_RID: (int)dv[i]["SglRID"],
                                                                            EXCEED_CAPACITY: Include.ConvertStringToChar((string)dv[i]["ExceedChar"]),
                                                                            EXCEED_BY: EXCEED_BY_Nullable
                                                                            );
				}
                // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                dv.Table = _dsOverRide.Tables["PackRounding"];
                for (int i = 0; i < dv.Count; i++)
                {
                    //string addGrade = BuildInsertPackRoundingStatement(dv[i]);
                    //_dba.ExecuteNonQuery(addGrade);
                    double? PACK_ROUNDING_1ST_PACK_PCT_Nullable = null;
                    if (dv[i]["FstPack"] != System.DBNull.Value) PACK_ROUNDING_1ST_PACK_PCT_Nullable = Convert.ToDouble(dv[i]["FstPack"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                    double? PACK_ROUNDING_NTH_PACK_PCT_Nullable = null;
                    if (dv[i]["NthPack"] != System.DBNull.Value) PACK_ROUNDING_NTH_PACK_PCT_Nullable = Convert.ToDouble(dv[i]["NthPack"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                    StoredProcedures.MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT.Insert(_dba,
                                                                                 METHOD_RID: OverRide_HDR_RID,
                                                                                 PACK_MULTIPLE_RID: Convert.ToInt32(dv[i]["PackMultiple"]), //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message
                                                                                 PACK_ROUNDING_1ST_PACK_PCT: PACK_ROUNDING_1ST_PACK_PCT_Nullable,
                                                                                 PACK_ROUNDING_NTH_PACK_PCT: PACK_ROUNDING_NTH_PACK_PCT_Nullable
                                                                                 );
                }
                // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)

                // BEGIN TT#1401 - GTaylor - Reservation Stores
                dv.Table = _imoDataSet.Tables["Stores"];
                //string addVSW;
                for (int i = 0; i < dv.Count; i++)
                {
                    //                    if ((dv[i]["Updated"].Equals(true)) && !(dv[i]["Reservation Store"].Equals("")))
                    if (!(dv[i]["Reservation Store"].Equals("")))
                    {
                        //addVSW = BuildDeleteVSWIMO(dv[i]);
                        //_dba.ExecuteNonQuery(addVSW);
                        StoredProcedures.MID_METHOD_OVERRIDE_IMO_DELETE.Delete(_dba,
                                                                       METHOD_RID: OverRide_HDR_RID,
                                                                       ST_RID: (int)dv[i]["Store RID"]
                                                                       );

                        //addVSW = BuildInsertVSWIMO(dv[i]);
                        //// BEGIN TT#1401 - stodd - VSW
                        //if (addVSW != null)
                        //{
                        //    _dba.ExecuteNonQuery(addVSW);
                        //}
                        //// END TT#1401 - stodd - VSW

                        bool doAddVSW = true;
                        if (
                            ((dv[i]["Item Max"] == System.DBNull.Value) || (dv[i]["Item Max"].Equals("")))
                            &&
                            ((dv[i]["Min Ship Qty"] == System.DBNull.Value) || (dv[i]["Min Ship Qty"].Equals("")))
                            &&
                            ((dv[i]["Pct Pack Threshold"] == System.DBNull.Value) || (dv[i]["Pct Pack Threshold"].Equals("")))
                            )
                        {
                            doAddVSW = false;
                        }



                        if (doAddVSW)
                        {
                            int? ST_RID_Nullable = null;
                            if ((dv[i]["Store RID"] != System.DBNull.Value) && Convert.ToString(dv[i]["Store RID"]) != string.Empty) ST_RID_Nullable = Convert.ToInt32(dv[i]["Store RID"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                            int? IMO_MIN_SHIP_QTY_Nullable = null;
                            if ((dv[i]["Min Ship Qty"] != System.DBNull.Value) && Convert.ToString(dv[i]["Min Ship Qty"]) != string.Empty) IMO_MIN_SHIP_QTY_Nullable = Convert.ToInt32(dv[i]["Min Ship Qty"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                            double? IMO_PCT_PK_THRSHLD_Nullable = null;
                            if ((dv[i]["Pct Pack Threshold"] != System.DBNull.Value) && Convert.ToString(dv[i]["Pct Pack Threshold"]) != string.Empty && Convert.ToDouble(dv[i]["Pct Pack Threshold"]) != Include.PercentPackThresholdDefault) IMO_PCT_PK_THRSHLD_Nullable = Convert.ToDouble(dv[i]["Pct Pack Threshold"]) / Convert.ToDouble(100); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                            int? IMO_MAX_VALUE_Nullable = null;
                            if ((dv[i]["Item Max"] != System.DBNull.Value) && Convert.ToString(dv[i]["Item Max"]) != string.Empty) IMO_MAX_VALUE_Nullable = Convert.ToInt32(dv[i]["Item Max"]); //TT#1331-MD -jsobek -In Allocation Override Method-> vsw Tab-> add minimum ship Qty and Item-> application throws System Exception error message

                            StoredProcedures.MID_METHOD_OVERRIDE_IMO_INSERT.Insert(_dba,
                                                                                   METHOD_RID: OverRide_HDR_RID,
                                                                                   ST_RID: ST_RID_Nullable,
                                                                                   IMO_MIN_SHIP_QTY: IMO_MIN_SHIP_QTY_Nullable,
                                                                                   IMO_PCT_PK_THRSHLD: IMO_PCT_PK_THRSHLD_Nullable,
                                                                                   IMO_MAX_VALUE: IMO_MAX_VALUE_Nullable
                                                                                   );
                        }
                       
                    }
                }
                //addVSW = BuildUpdateMethodOverrideVSWIMO(ApplyVSW);
                //_dba.ExecuteNonQuery(addVSW);
                // END TT#1401 - GTaylor - Reservation Stores
                StoredProcedures.MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG.Update(_dba,
                                                                    METHOD_RID: OverRide_HDR_RID,
                                                                    IMO_APPLY_VSW: (ApplyVSW ? 1 : 0)
                                                                    );
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

      
		private void InitGradeBoundaryHash()
		{
			if (_storeGradeHash == null)
			{
				_storeGradeHash = Hashtable.Synchronized(new Hashtable());
			}
			else
			{
				_storeGradeHash.Clear();
			}
		}
		
		public bool UpdateMethod(int method_RID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			_dba = td.DBA;
			try
			{
				if (   DeleteChildData() 
					&& UpdateMethodOverride(method_RID) 
				    && UpdateChildData())
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


		public bool UpdateMethodOverride(int method_RID)
		{
			bool UpdateSuccessful = true;
			try
			{
                int? STORE_FILTER_RID_Nullable = null;
                if (Store_Filter_RID != Include.NoRID) STORE_FILTER_RID_Nullable = Store_Filter_RID;

                int? STORE_GRADE_TIMEFRAME_Nullable = null;
                if (_useStoreGradeDefault == false) STORE_GRADE_TIMEFRAME_Nullable = Grade_Week_Count;

                double? PERCENT_NEED_LIMIT_Nullable = null;
                if (_usePctNeedDefault == false) PERCENT_NEED_LIMIT_Nullable = Percent_Need_Limit;

                double? RESERVE_Nullable = null;
                if (Reserve != Include.UndefinedReserve) RESERVE_Nullable = Reserve;

                char? PERCENT_IND_Nullable = null;
                if (Reserve != Include.UndefinedReserve) PERCENT_IND_Nullable = _Percent_Ind;

                int? MERCH_HN_RID_Nullable = null;
                if (_Merch_Plan_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = _Merch_Plan_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (_Merch_Plan_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = _Merch_Plan_PH_RID;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (_Merch_Plan_PH_RID != Include.NoRID) MERCH_PHL_SEQUENCE_Nullable=_Merch_Plan_PHL_SEQ;

                int? ON_HAND_HN_RID_Nullable = null;
                if (_Merch_OnHand_HN_RID != Include.NoRID) ON_HAND_HN_RID_Nullable = _Merch_OnHand_HN_RID;

                int? ON_HAND_PH_RID_Nullable = null;
                if (_Merch_OnHand_PH_RID != Include.NoRID) ON_HAND_PH_RID_Nullable = _Merch_OnHand_PH_RID;

                int? ON_HAND_PHL_SEQUENCE_Nullable = null;
                if (_Merch_OnHand_PH_RID != Include.NoRID) ON_HAND_PHL_SEQUENCE_Nullable = _Merch_OnHand_PHL_SEQ;

                double? ON_HAND_FACTOR_Nullable = null;
                if (UseFactorPctDefault == false) ON_HAND_FACTOR_Nullable = _Plan_Factor_Percent;

                int? COLOR_MULT_Nullable = null;
                if (_All_Color_Multiple != Include.Undefined) COLOR_MULT_Nullable = _All_Color_Multiple;

                int? SIZE_MULT_Nullable = null;
                if (_All_Size_Multiple != Include.Undefined) SIZE_MULT_Nullable = _All_Size_Multiple;

                int? ALL_COLOR_MIN_Nullable = null;
                if (_useAllColorsMinDefault == false) ALL_COLOR_MIN_Nullable = _All_Color_Minimum;

                int? ALL_COLOR_MAX_Nullable = null;
                if (_useAllColorsMaxDefault == false) ALL_COLOR_MAX_Nullable = _All_Color_Maximum;

                int? SG_RID_Nullable = null;
                if (_Store_Group_RID != Include.NoRID) SG_RID_Nullable = _Store_Group_RID;

                int? STORE_GRADES_SG_RID_Nullable = null;
                if (_Tab_Store_Group_RID != Include.NoRID) STORE_GRADES_SG_RID_Nullable = _Tab_Store_Group_RID;

                int? IB_MERCH_HN_RID_Nullable = null;
                if (IB_MERCH_HN_RID != Include.NoRID) IB_MERCH_HN_RID_Nullable = IB_MERCH_HN_RID;

                int? IB_MERCH_PH_RID_Nullable = null;
                if (IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PH_RID_Nullable = IB_MERCH_PH_RID;

                int? IB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PHL_SEQUENCE_Nullable = IB_MERCH_PHL_SEQ;  //fixes previous bug - PHL_SEQ should not be compared to NoRID

                int? IMO_SG_RID_Nullable = null;
                if (IMO_SG_RID != Include.NoRID) IMO_SG_RID_Nullable = IMO_SG_RID;

                StoredProcedures.MID_METHOD_OVERRIDE_UPDATE.Update(_dba,
                                                                   METHOD_RID: method_RID,
                                                                   STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                   STORE_GRADE_TIMEFRAME: STORE_GRADE_TIMEFRAME_Nullable,
                                                                   EXCEED_MAX_IND: _Exceed_Maximums_Ind,
                                                                   PERCENT_NEED_LIMIT: PERCENT_NEED_LIMIT_Nullable,
                                                                   RESERVE: RESERVE_Nullable,
                                                                   PERCENT_IND: PERCENT_IND_Nullable,
                                                                   MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                   MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                   MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                   ON_HAND_HN_RID: ON_HAND_HN_RID_Nullable,
                                                                   ON_HAND_PH_RID: ON_HAND_PH_RID_Nullable,
                                                                   ON_HAND_PHL_SEQUENCE: ON_HAND_PHL_SEQUENCE_Nullable,
                                                                   ON_HAND_FACTOR: ON_HAND_FACTOR_Nullable,
                                                                   COLOR_MULT: COLOR_MULT_Nullable,
                                                                   SIZE_MULT: SIZE_MULT_Nullable,
                                                                   ALL_COLOR_MIN: ALL_COLOR_MIN_Nullable,
                                                                   ALL_COLOR_MAX: ALL_COLOR_MAX_Nullable,
                                                                   SG_RID: SG_RID_Nullable,
                                                                   EXCEED_CAPACITY: _Exceed_Capacity_Ind,
                                                                   MERCH_UNSPECIFIED: Include.ConvertBoolToChar(Merch_Plan_Unspecified),
                                                                   ON_HAND_UNSPECIFIED: Include.ConvertBoolToChar(Merch_OnHand_Unspecified),
                                                                   RESERVE_AS_BULK: ReserveAsBulk,
                                                                   RESERVE_AS_PACKS: ReserveAsPacks,
                                                                   STORE_GRADES_SG_RID: STORE_GRADES_SG_RID_Nullable,
                                                                   INVENTORY_IND: _inventory_Ind,
                                                                   IB_MERCH_HN_RID: IB_MERCH_HN_RID_Nullable,
                                                                   IB_MERCH_PH_RID: IB_MERCH_PH_RID_Nullable,
                                                                   IB_MERCH_PHL_SEQUENCE: IB_MERCH_PHL_SEQUENCE_Nullable,
                                                                   IMO_SG_RID: IMO_SG_RID_Nullable
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
				if ( DeleteChildData() )
				{
                    StoredProcedures.MID_METHOD_OVERRIDE_DELETE.Delete(_dba, METHOD_RID: method_RID);
	
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
                
                StoredProcedures.MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE.Delete(_dba, METHOD_RID: _OverRide_HDR_RID);
                StoredProcedures.MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE.Delete(_dba, METHOD_RID: _OverRide_HDR_RID);
                StoredProcedures.MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE.Delete(_dba, METHOD_RID: _OverRide_HDR_RID);
                StoredProcedures.MID_METHOD_OVERRIDE_CAPACITY_DELETE.Delete(_dba, METHOD_RID: _OverRide_HDR_RID);
                StoredProcedures.MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE.Delete(_dba, METHOD_RID: _OverRide_HDR_RID);

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
                return StoredProcedures.MID_METHOD_OVERRIDE_READ_FROM_NODE.Read(_dba, MERCH_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetMethodsByOnHandNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE.Read(_dba, ON_HAND_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
	}
}

