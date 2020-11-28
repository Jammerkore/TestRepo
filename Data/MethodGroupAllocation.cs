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
	public class GroupAllocationMethodData: MethodBaseData
	{
		private int _methodRid;
		//private int _Store_Filter_RID;
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
        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        //private char _Exceed_Capacity_Ind;
        //private int _All_Color_Multiple;
        //private int _All_Size_Multiple;
        //private int _All_Color_Maximum;
        //private int _All_Color_Minimum;
        // end TT#488 - MD - Jellis - Group Allocaton - Not used in GA
		private int _Store_Group_RID;
        private int _Tab_Store_Group_RID;    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35
        private bool _useStoreGradeDefault;
		private bool _usePctNeedDefault;
		private bool _useFactorPctDefault;

        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        //private bool _useAllColorsMinDefault;
        //private bool _useAllColorsMaxDefault;
        //// BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        //private int _Pack_Multiple;
        //private double _Pack_Rounding_1st_Pack_Pct;
        //private double _Pack_Rounding_Nth_Pack_Pct;
        // end TT#488 - MD - Jellis - Group Allocation - Not used in GA

        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		private Hashtable _storeGradeHash; // TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35
		private DataSet _dsGroupAllocation;

        //private DataSet _imoDataSet; // TT#1401 - GTaylor - Reservation Stores
        //private IMOMethodOverrideProfileList _imoMethodOverrideProfileList; // TT#1401 - GTaylor - Reservation Stores

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

        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        private eMerchandiseType _hdrmerchandiseType;
        private char _hdrinventory_Ind;
        private int _HDRIB_MERCH_HN_RID;
        private int _HDRIB_MERCH_PH_RID;
        private int _HDRIB_MERCH_PHL_SEQUENCE;
        // ENDN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis

        // begin TT#488 - Jellis - Group Allocation - Not used in GA
        //// BEGIN TT#1401 - GTaylor - Reservation Stores
        //private int _IMO_SG_RID;
        //private bool _applyVSW = true;
        //// END TT#1401 - GTaylor - Reservation Stores
        // end TT#488 - Jellis - Group Allocation - Not used in GA


		private int _beginCdrRid;
		private int _shipToCdrRid;
		private bool _lineItemMinOverrideInd;
		private int _lineItemMinOverride;	// TT#488-MD - STodd - Group Allocation


        //  To Do store grades and colors and capacity by set

        private bool _merchUnspecified;      // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        private bool _onHandUnspecified;     // End TT#709 

		public int MethodRid
		{
			get{return _methodRid;}
			set{_methodRid = value;	}
		}
		//public int Store_Filter_RID
		//{
		//    get{return _Store_Filter_RID;}
		//    set{_Store_Filter_RID = value;	}
		//}
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

        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        //public char Exceed_Capacity_Ind
        //{
        //    get{return _Exceed_Capacity_Ind;}
        //    set{_Exceed_Capacity_Ind = value;	}
        //}
        // end TT#488 - MD - Jellis - Group Allocation - Not used in GA

		public char Percent_Ind
		{
			get{return _Percent_Ind;}
			set{_Percent_Ind = value;	}
		}

        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        //public int All_Color_Multiple
        //{
        //    get{return _All_Color_Multiple;}
        //    set{_All_Color_Multiple = value; }
        //}
        //public int All_Size_Multiple
        //{
        //    get{return _All_Size_Multiple;}
        //    set{_All_Size_Multiple = value;  }
        //}
        //public int All_Color_Maximum
        //{
        //    get{return _All_Color_Maximum;}
        //    set{_All_Color_Maximum = value;  }
        //}
        //public int All_Color_Minimum
        //{
        //    get{return _All_Color_Minimum;}
        //    set{_All_Color_Minimum = value;  }
        //}
        // end TT#488 - MD - Jellis - Group Allocation - Not used in GA 

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

        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        //public bool UseAllColorsMinDefault
        //{
        //    get{return _useAllColorsMinDefault;}
        //    set{_useAllColorsMinDefault = value;  }
        //}
        //public bool UseAllColorsMaxDefault
        //{
        //    get{return _useAllColorsMaxDefault;}
        //    set{_useAllColorsMaxDefault = value;  }
        //}
		// BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        //public int Pack_Multiple
        //{
        //    get { return _Pack_Multiple; }
        //    set { _Pack_Multiple = value; }
        //}
        //public double Pack_Rounding_1st_Pack_Pct
        //{
        //    get { return _Pack_Rounding_1st_Pack_Pct; }
        //    set { _Pack_Rounding_1st_Pack_Pct = value; }
        //}
        //public double Pack_Rounding_Nth_Pack_Pct
        //{
        //    get { return _Pack_Rounding_Nth_Pack_Pct; }
        //    set { _Pack_Rounding_Nth_Pack_Pct = value; }
        //}
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        // end TT#488 - MD - Jellis - Group Allocation - Not used in GA
		
		public DataSet DSGroupAllocation
		{
			get{return _dsGroupAllocation;}
			set{_dsGroupAllocation = value;  }
		}
		// BEGIN TT#667 - Stodd - Pre-allocate Reserve

        // BEGIN TT#1401 - GTaylor - Reservation Stores
		//public DataSet IMODataSet
		//{
		//    get { return _imoDataSet; }
		//    set { _imoDataSet = value; }
		//}
		//public IMOMethodOverrideProfileList IMOMethodOverrideProfileList
		//{
		//    get { return _imoMethodOverrideProfileList; }
		//    set { _imoMethodOverrideProfileList = value; }
		//}
		//public bool ApplyVSW
		//{
		//    get { return _applyVSW; }
		//    set { _applyVSW = value; }
		//}
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

        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        public char HDRInventory_Ind
        {
            get
            {
                return _hdrinventory_Ind;
            }

            set
            {
                _hdrinventory_Ind = value;
            }
        }

        public int HDRIB_MERCH_HN_RID
        {
            get
            {
                return _HDRIB_MERCH_HN_RID;
            }

            set
            {
                _HDRIB_MERCH_HN_RID = value;
            }
        }

        public int HDRIB_MERCH_PH_RID
        {
            get
            {
                return _HDRIB_MERCH_PH_RID;
            }

            set
            {
                _HDRIB_MERCH_PH_RID = value;
            }
        }

        public int HDRIB_MERCH_PHL_SEQ
        {
            get
            {
                return _HDRIB_MERCH_PHL_SEQUENCE;
            }

            set
            {
                _HDRIB_MERCH_PHL_SEQUENCE = value;
            }
        }
        // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis

        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        // BEGIN TT#1401 - GTaylor - Reservation Stores
        //public int IMO_SG_RID
        //{
        //    get
        //    {
        //        return _IMO_SG_RID;
        //    }
        //    set
        //    { 
        //        _IMO_SG_RID = value;
        //    }
        //}
        // END TT#1401 - GTaylor - Reservation Stores
        // end TT#488 - MD - Jellis - Group Allocation - Not used in GA

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

		public int BeginCdrRid
		{
			get { return _beginCdrRid; }
			set { _beginCdrRid = value; }
		}
		public int ShipToCdrRid
		{
			get { return _shipToCdrRid; }
			set { _shipToCdrRid = value; }
		}
		public bool LineItemMinOverrideInd
		{
			get { return _lineItemMinOverrideInd; }
			set { _lineItemMinOverrideInd = value; }
		}
		public int LineItemMinOverride	// TT#488-MD - STodd - Group Allocation - 
		{
			get { return _lineItemMinOverride; }
			set { _lineItemMinOverride = value; }
		}

		
		/// <summary>
		/// Creates an instance of the AllocationOverrideMethodData class.
		/// </summary>
		public GroupAllocationMethodData()
		{
		}

		/// <summary>
		/// Creates an instance of the AllocationOverrideMethodData class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public GroupAllocationMethodData(int aMethodRID)
		{
			_methodRid = Include.NoRID;
		}

		/// <summary>
		/// Creates an instance of the AllocationOverrideMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public GroupAllocationMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = Include.NoRID;
		}
		public GroupAllocationMethodData(TransactionData td)
		{
			_dba = td.DBA;
			_methodRid = Include.NoRID;
		}
		
		public bool PopulateGroupAllocation(int method_RID)
		{
			try
			{
				if (PopulateMethod(method_RID))
				{
					_methodRid = method_RID;

                    //Begin TT#1268-MD -jsobek -5.4 Merge
                    //StringBuilder SQLCommand = new StringBuilder();
                    //SQLCommand.Append("SELECT ");
                    //SQLCommand.Append("STORE_GRADE_TIMEFRAME, EXCEED_MAX_IND, PERCENT_NEED_LIMIT, RESERVE, PERCENT_IND, ");
                    //SQLCommand.Append("MERCH_HN_RID, MERCH_PH_RID, MERCH_PHL_SEQUENCE, ");
                    //SQLCommand.Append("ON_HAND_HN_RID, ON_HAND_PH_RID, ON_HAND_PHL_SEQUENCE, ");
                    //SQLCommand.Append("ON_HAND_FACTOR, ");
                    //SQLCommand.Append("SG_RID, ");
                    //// Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    //SQLCommand.Append("MERCH_UNSPECIFIED, ON_HAND_UNSPECIFIED,  ");
                    //// End TT#709 
                    //// BEGIN TT#667 - Stodd - Pre-allocate Reserve
                    //SQLCommand.Append("COALESCE(RESERVE_AS_BULK,0) as RESERVE_AS_BULK, COALESCE(RESERVE_AS_PACKS,0) as RESERVE_AS_PACKS, ");
                    //// END TT#667 - Stodd - Pre-allocate Reserve
                    //// BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    ////SQLCommand.Append("STORE_GRADES_SG_RID ");    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                    //SQLCommand.Append("STORE_GRADES_SG_RID, ");    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                    //SQLCommand.Append("COALESCE(INVENTORY_IND,'A') as INVENTORY_IND, ");
                    ////SQLCommand.Append("COALESCE(IB_MERCH_TYPE," + Include.NoRID.ToString(CultureInfo.CurrentUICulture) + ") IB_MERCH_TYPE, ");
                    //// BEGIN TT#1401 - GTaylor - Reservation Stores
                    ////SQLCommand.Append("IB_MERCH_HN_RID, IB_MERCH_PH_RID, IB_MERCH_PHL_SEQUENCE ");
                    //// BEGIN TT#1401 - stodd - VSW
                    //SQLCommand.Append("IB_MERCH_HN_RID, IB_MERCH_PH_RID, IB_MERCH_PHL_SEQUENCE, ");
                    //SQLCommand.Append("BEGIN_CDR_RID, SHIP_TO_CDR_RID, LINE_ITEM_MIN_OVERRIDE_IND, LINE_ITEM_MIN_OVERRIDE, ");
                    //SQLCommand.Append("COALESCE(HDRINVENTORY_IND,'A') as HDRINVENTORY_IND, ");
                    //SQLCommand.Append("HDRIB_MERCH_HN_RID, HDRIB_MERCH_PH_RID, HDRIB_MERCH_PHL_SEQUENCE ");

                    //// END TT#1401 - stodd - VSW                    
                    //// END TT#1401 - GTaylor - Reservation Stores
                    //// END TT#1287 - AGallagher - Inventory Min/Max
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand.Append("FROM METHOD_GROUP_ALLOCATION WHERE METHOD_RID = ");
                    //// end MID Track # 2354
                    //SQLCommand.Append(method_RID);

                    //DataTable dtGroupAllocation = MIDEnvironment.CreateDataTable();
                    //dtGroupAllocation = _dba.ExecuteSQLQuery(SQLCommand.ToString(), "Group Allocation");
                    DataTable dtGroupAllocation = StoredProcedures.MID_METHOD_GROUP_ALLOCATION_READ.Read(_dba, METHOD_RID: method_RID);
                    //End TT#1268-MD -jsobek -5.4 Merge

					if (dtGroupAllocation.Rows.Count != 0)
					{
						DataRow dr = dtGroupAllocation.Rows[0];
						
						//if (dr["STORE_FILTER_RID"] != System.DBNull.Value)
						//    _Store_Filter_RID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
						//else
						//    _Store_Filter_RID = Include.NoRID;

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
							_Exceed_Maximums_Ind = Convert.ToChar(dr["EXCEED_MAX_IND"].ToString());

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

						//if (dr["COLOR_MULT"] != System.DBNull.Value)
						//    _All_Color_Multiple = Convert.ToInt32(dr["COLOR_MULT"], CultureInfo.CurrentUICulture);

						//if (dr["SIZE_MULT"] != System.DBNull.Value)
						//    _All_Size_Multiple = Convert.ToInt32(dr["SIZE_MULT"], CultureInfo.CurrentUICulture);

						//if (dr["ALL_COLOR_MIN"] != System.DBNull.Value)
						//{
						//    _All_Color_Minimum = Convert.ToInt32(dr["ALL_COLOR_MIN"], CultureInfo.CurrentUICulture);
						//    _useAllColorsMinDefault = false;
						//}
						//else
						//    _useAllColorsMinDefault = true;

						//if (dr["ALL_COLOR_MAX"] != System.DBNull.Value)
						//{
						//    _All_Color_Maximum = Convert.ToInt32(dr["ALL_COLOR_MAX"], CultureInfo.CurrentUICulture);
						//    _useAllColorsMaxDefault = false;
						//}
						//else
						//    _useAllColorsMaxDefault = true;

						if (dr["SG_RID"] != System.DBNull.Value)
							_Store_Group_RID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
						else
							_Store_Group_RID = Include.NoRID;

						//if (dr["EXCEED_CAPACITY"] != System.DBNull.Value)
						//    _Exceed_Capacity_Ind = Convert.ToChar(dr["EXCEED_CAPACITY"], CultureInfo.CurrentUICulture);

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
						//if (dr["IMO_SG_RID"] != System.DBNull.Value)
						//    _IMO_SG_RID = Convert.ToInt32(dr["IMO_SG_RID"], CultureInfo.CurrentUICulture);
						//else
						//    _IMO_SG_RID = Include.NoRID;
						//// END TT#1401 - GTaylor - Reservation Stores

						//// BEGIN TT#1401 - stodd
						//if (dr["IMO_APPLY_VSW"] != System.DBNull.Value)
						//    _applyVSW = Convert.ToBoolean(dr["IMO_APPLY_VSW"], CultureInfo.CurrentUICulture);
						//else
						//    _applyVSW = true;
						// END TT#1401 - stodd

						_beginCdrRid = Convert.ToInt32(dr["BEGIN_CDR_RID"], CultureInfo.CurrentUICulture);
						_shipToCdrRid = Convert.ToInt32(dr["SHIP_TO_CDR_RID"], CultureInfo.CurrentUICulture);
						if (dr["LINE_ITEM_MIN_OVERRIDE_IND"] != System.DBNull.Value)
							_lineItemMinOverrideInd = Include.ConvertCharToBool(Convert.ToChar(dr["LINE_ITEM_MIN_OVERRIDE_IND"]));
                        else
                            _lineItemMinOverrideInd = false;

                        if (dr["LINE_ITEM_MIN_OVERRIDE"] != System.DBNull.Value)    // TT#986 - MD - stodd - invalid cast exception
						{
							_lineItemMinOverride = Convert.ToInt32(dr["LINE_ITEM_MIN_OVERRIDE"]);	// TT#488-MD - STodd - Group Allocation
						}
						else
						{
							_lineItemMinOverride = Include.Undefined;		// TT#488-MD - STodd - Group Allocation
						}

                        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
                        if (dr["HDRINVENTORY_IND"] != System.DBNull.Value)
                            _hdrinventory_Ind = Convert.ToChar(dr["HDRINVENTORY_IND"], CultureInfo.CurrentUICulture);
                        else
                            _hdrinventory_Ind = 'A';
                        if (dr["HDRIB_MERCH_HN_RID"] != System.DBNull.Value)
                            _HDRIB_MERCH_HN_RID = Convert.ToInt32(dr["HDRIB_MERCH_HN_RID"], CultureInfo.CurrentUICulture);
                        else
                            _HDRIB_MERCH_HN_RID = Include.NoRID;
                        if (dr["HDRIB_MERCH_PH_RID"] != System.DBNull.Value)
                        {
                            _HDRIB_MERCH_PH_RID = Convert.ToInt32(dr["HDRIB_MERCH_PH_RID"], CultureInfo.CurrentUICulture);
                            _HDRIB_MERCH_PHL_SEQUENCE = Convert.ToInt32(dr["HDRIB_MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            _HDRIB_MERCH_PH_RID = Include.NoRID;
                            _HDRIB_MERCH_PHL_SEQUENCE = 0;
                        }
                        // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis

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
		public DataSet GetGroupAllocationChildData()
		{ 
			_dsGroupAllocation = MIDEnvironment.CreateDataSet();

            //DataTable dtColors = SetupColorTable();
			DataTable dtStoreGrades = SetupStoreGradeTable();
			//DataTable dtCapacity = SetupCapacityTable();
            //DataTable dtPackRounding = SetupPackRoundingTable();  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
			DataTable dtStoreGradeBoundary = GetStoreGradesGrades(Method_RID);  // TT#616 - Stodd - Allocation - Add Attribute Sets (#35)

			//_dsGroupAllocation.Tables.Add(dtColors);
			_dsGroupAllocation.Tables.Add(dtStoreGrades);
			//_dsGroupAllocation.Tables.Add(dtCapacity);
            //_dsGroupAllocation.Tables.Add(dtPackRounding);   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
			_dsGroupAllocation.Tables.Add(dtStoreGradeBoundary);   // TT#616 - Stodd - Allocation - Add Attribute Sets (#35)

			return _dsGroupAllocation;
		}
		//private DataTable SetupColorTable()
		//{
		//    DataTable dt = MIDEnvironment.CreateDataTable("Colors");
		//    dt.Columns.Add("RowPosition",System.Type.GetType("System.Int32"));
		//    dt.Columns.Add("ColorCodeRID", System.Type.GetType("System.Int32")); 
		//    dt.Columns.Add("Minimum", System.Type.GetType("System.Int32")); 
		//    dt.Columns.Add("Maximum", System.Type.GetType("System.Int32")); 

		//    StringBuilder SQLCommand = new StringBuilder();
		
		//    SQLCommand.Append("SELECT 0 AS RowPosition, COLOR_CODE_RID AS ColorCodeRID, ");
		//    SQLCommand.Append("COLOR_MIN AS Minimum, ");
		//    SQLCommand.Append("COLOR_MAX AS Maximum ");
		//    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
		//    SQLCommand.Append("FROM METHOD_OVERRIDE_COLOR_MINMAX ");
		//    // end MID Track # 2354
		//    SQLCommand.Append("WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture));
			
		//    dt = _dba.ExecuteQuery(SQLCommand.ToString());
		//    dt.TableName = "Colors";
		//    dt.Columns[0].ColumnName = "RowPosition";
		//    dt.Columns[1].ColumnName = "Color";
		//    dt.Columns[2].ColumnName = "Minimum";
		//    dt.Columns[2].Caption = "Minimum";
		//    dt.Columns[2].ReadOnly = false;
		//    dt.Columns[2].Unique = false;
		//    dt.Columns[2].AllowDBNull = true;
		//    dt.Columns[3].ColumnName = "Maximum";
		//    dt.Columns[3].Caption = "Maximum";
		//    dt.Columns[3].ReadOnly = false;
		//    dt.Columns[3].Unique = false;
		//    dt.Columns[3].AllowDBNull = true;
		//    int rowpos = 0;
		//    foreach (DataRow row in dt.Rows)
		//    {
		//        row["RowPosition"] = rowpos;
		//        rowpos++;
		//    }
		//    return dt;
		//}
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
			dtStoreGrades.Columns.Add("MinGroup", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MaxGroup", System.Type.GetType("System.Int32"));
			//dtStoreGrades.Columns.Add("MinAd", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MinHeader", System.Type.GetType("System.Int32"));
			dtStoreGrades.Columns.Add("MaxHeader", System.Type.GetType("System.Int32"));
			//dtStoreGrades.Columns.Add("ShipUpTo", System.Type.GetType("System.Int32"));  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

			//======================
			// Get Grade Boundaries
			//======================
            //StringBuilder SQLCommand = new StringBuilder(); //TT#1268-MD -jsobek -5.4 Merge
			dtGradeBoundary = GetStoreGradesGrades(Method_RID);

			//======================
			// Get Grade Values
			//======================

            //Begin TT#1268-MD -jsobek -5.4 Merge
            //SQLCommand.Append("SELECT 0 AS RowPosition, ");
            //SQLCommand.Append("SGL_RID, ");
            //SQLCommand.Append("BOUNDARY AS Boundary, ");
            //SQLCommand.Append("MINIMUM_GROUP AS MinGroup, MAXIMUM_GROUP AS MaxGroup, ");
            //SQLCommand.Append("MINIMUM_HEADER AS MinHeader, ");
            //SQLCommand.Append("MAXIMUM_HEADER AS MaxHeader ");
            ////SQLCommand.Append("SHIP_UP_TO AS ShipUpTo ");  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
            //SQLCommand.Append("FROM METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES ");
            //SQLCommand.Append("WHERE METHOD_RID = " + Method_RID.ToString(CultureInfo.CurrentUICulture));
            //SQLCommand.Append(" ORDER BY SGL_RID, BOUNDARY DESC");
            //dtGradeValues = _dba.ExecuteQuery(SQLCommand.ToString());
            dtGradeValues = StoredProcedures.MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ.Read(_dba, METHOD_RID: Method_RID);
            //End TT#1268-MD -jsobek -5.4 Merge

			//==============================
			// Get Store Group Levels (Sets)
			//==============================

            //Begin TT#1268-MD -jsobek -5.4 Merge
            //SQLCommand = new StringBuilder();
            //SQLCommand.Append("SELECT SGL_RID ");
            //SQLCommand.Append("FROM STORE_GROUP_LEVEL ");
            //SQLCommand.Append("WHERE SG_RID = " + _Tab_Store_Group_RID.ToString(CultureInfo.CurrentUICulture));
            //SQLCommand.Append(" ORDER BY SGL_SEQUENCE");
            //dtStoreGroupLevels = _dba.ExecuteQuery(SQLCommand.ToString());
            dtStoreGroupLevels = StoredProcedures.MID_STORE_GROUP_LEVEL_READ_FROM_GROUP.Read(_dba, SG_RID: _Tab_Store_Group_RID);
            //End TT#1268-MD -jsobek -5.4 Merge

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
						newRow["MinGroup"] = valMatch[0]["MinGroup"];
						newRow["MaxGroup"] = valMatch[0]["MaxGroup"];
						//newRow["MinAd"] = valMatch[0]["MinAd"];
						newRow["MinHeader"] = valMatch[0]["MinHeader"];
						newRow["MaxHeader"] = valMatch[0]["MaxHeader"];
						//newRow["ShipUpTo"] = valMatch[0]["ShipUpTo"];
					}
					else
					{
						newRow["MinGroup"] = DBNull.Value;
						newRow["MaxGroup"] = DBNull.Value;
						//newRow["MinAd"] = DBNull.Value;
						newRow["MinHeader"] = DBNull.Value;
						newRow["MaxHeader"] = DBNull.Value;
						//newRow["ShipUpTo"] = DBNull.Value;
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
			dtStoreGrades.Columns["MinGroup"].ColumnName = "Group Min";
			dtStoreGrades.Columns["MaxGroup"].ColumnName = "Group Max";
			//dtStoreGrades.Columns["MinAd"].ColumnName = "Min Ad";
			dtStoreGrades.Columns["MinHeader"].ColumnName = "Header Min";
			dtStoreGrades.Columns["MaxHeader"].ColumnName = "Header Max";
			//dtStoreGrades.Columns["ShipUpTo"].ColumnName = "Ship Up To";   // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

			return dtStoreGrades;

			// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		}

        // begin TT#488 - MD - Jellis - Group Allocation - Not used in GA
        //// BEGIN TT#1401 - GTaylor - Reservation Stores
        //public bool GetApplyVSW()
        //{
        //    // BEGIN TT#1401 - stodd
        //    //bool applyVSW = false;
        //    //int _methodRID = MethodRid;
			
        //    //try
        //    //{
        //    //    StringBuilder SQLCommand = new StringBuilder();
        //    //    SQLCommand.Append("SELECT IMO_APPLY_VSW FROM METHOD_OVERRIDE ");
        //    //    SQLCommand.Append("WHERE METHOD_RID = " + _methodRID.ToString(CultureInfo.CurrentUICulture));

        //    //    DataTable dtTemp = MIDEnvironment.CreateDataTable("GetApplyVSW");

        //    //    dtTemp = _dba.ExecuteQuery(SQLCommand.ToString());
        //    //    if (dtTemp.Rows.Count > 0)
        //    //    {
        //    //       applyVSW = Convert.ToBoolean((dtTemp.Rows[0]["IMO_APPLY_VSW"].Equals(DBNull.Value)) ? true : dtTemp.Rows[0]["IMO_APPLY_VSW"]);
        //    //    }
        //    //    else 
        //    //    {
        //    //        applyVSW = false;
        //    //    }
        //    //    dtTemp.Dispose();
        //    //}
        //    //catch
        //    //{
        //    //    throw;
        //    //}
        //    return _applyVSW;
        //    // END TT#1401 - stodd
        //}
        // end TT#488 - MD - Jellis - Group Allocation - Not used in GA

		//public IMOMethodOverrideProfileList GetMethodOverrideIMO(int _methodRID)
		//{
		//    int Store_RID, Method_RID, Min_Ship_Qty, Max_Value;
		//    double Pct_Pck_Threshold;
		//    bool ApplyVSW;

		//    IMOMethodOverrideProfileList imomopl; 
            
		//    try
		//    {
		//        imomopl = new IMOMethodOverrideProfileList(eProfileType.IMO);

		//        if (_methodRID == Include.NoRID) _methodRID = _methodRid;

		//        DataTable dtMethodOverride = MIDEnvironment.CreateDataTable("VSW");

		//        StringBuilder SQLCommand = new StringBuilder();
		//        SQLCommand.Append("SELECT MOI.METHOD_RID, MOI.ST_RID, MOI.IMO_MIN_SHIP_QTY, MOI.IMO_PCT_PK_THRSHLD, MOI.IMO_MAX_VALUE, MO.IMO_APPLY_VSW ");
		//        SQLCommand.Append("FROM METHOD_OVERRIDE_IMO MOI ");
		//        SQLCommand.Append("LEFT JOIN METHOD_OVERRIDE MO ON MOI.METHOD_RID = MO.METHOD_RID ");
		//        SQLCommand.Append("WHERE MOI.METHOD_RID = " + _methodRID.ToString(CultureInfo.CurrentUICulture));
		//        SQLCommand.Append(" ORDER BY ST_RID");

		//        dtMethodOverride = _dba.ExecuteQuery(SQLCommand.ToString());
		//        foreach (DataRow row in dtMethodOverride.Rows)
		//        {
		//            Store_RID = Convert.ToInt32((row["ST_RID"].Equals(DBNull.Value)) ? Include.NoRID : row["ST_RID"]);
		//            Method_RID = Convert.ToInt32((row["METHOD_RID"].Equals(DBNull.Value)) ? Include.NoRID : row["METHOD_RID"]);
		//            Min_Ship_Qty = Convert.ToInt32((row["IMO_MIN_SHIP_QTY"].Equals(DBNull.Value)) ? Include.NoRID : row["IMO_MIN_SHIP_QTY"]);
		//            Pct_Pck_Threshold = Convert.ToDouble((row["IMO_PCT_PK_THRSHLD"].Equals(DBNull.Value)) ? .50 : row["IMO_PCT_PK_THRSHLD"]);
		//            Max_Value = Convert.ToInt32((row["IMO_MAX_VALUE"].Equals(DBNull.Value)) ? Include.NoRID : row["IMO_MAX_VALUE"]);
		//            ApplyVSW = Convert.ToBoolean((row["IMO_APPLY_VSW"].Equals(DBNull.Value)) ? true : row["IMO_APPLY_VSW"]);

		//            IMOMethodOverrideProfile imomop = new IMOMethodOverrideProfile(Store_RID);
		//            if (!imomopl.Contains(Store_RID))
		//            {
		//                imomop.IMOMaxValue = Max_Value;
		//                imomop.IMOMethodRID = Method_RID;
		//                imomop.IMOMinShipQty = Min_Ship_Qty;
		//                imomop.IMOPackQty = Pct_Pck_Threshold;
		//                imomop.IMOStoreRID = Store_RID;
		//                imomop.IMO_Apply_VSW = ApplyVSW;

		//                imomopl.Add(imomop);
		//            }
		//        }
		//        dtMethodOverride.Dispose();
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//    return imomopl;
		//}
		//// END TT#1401 - GTaylor - Reservation Stores

		private DataTable GetStoreGradesGrades(int methodRid)
		{
			DataTable dtGradeValues = MIDEnvironment.CreateDataTable("GradeBoundary");

            //Begin TT#1268-MD -jsobek -5.4 Merge
            //StringBuilder SQLCommand = new StringBuilder();
            //SQLCommand.Append("SELECT 0 AS RowPosition, GRADE_CODE AS GradeCode, BOUNDARY AS Boundary ");
            //SQLCommand.Append("FROM METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY ");
            //SQLCommand.Append("WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture));
            //SQLCommand.Append(" ORDER BY BOUNDARY DESC");

            //dtGradeValues = _dba.ExecuteQuery(SQLCommand.ToString());
            dtGradeValues = StoredProcedures.MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ.Read(_dba, METHOD_RID: _methodRid);
            //End TT#1268-MD -jsobek -5.4 Merge
			dtGradeValues.TableName = "GradeBoundary";

			return dtGradeValues;
		}


		//private DataTable SetupCapacityTable()
		//{
		//    DataTable dt = MIDEnvironment.CreateDataTable("Capacity");
			
		//    dt.Columns.Add("SglRID",		System.Type.GetType("System.Int32")); 
		//    dt.Columns.Add("SetName",		System.Type.GetType("System.String"));
		//    dt.Columns.Add("ExceedCapacity",System.Type.GetType("System.String"));
		//    dt.Columns.Add("ExceedBy",	System.Type.GetType("System.Double")); 
		//    //dt.Columns.Add("ExceedCapacityBool",System.Type.GetType("System.Boolean"));

		//    StringBuilder SQLCommand = new StringBuilder();
		
		//    SQLCommand.Append("SELECT moc.SGL_RID AS SglRID, ");
		//    SQLCommand.Append("sgl.SGL_ID AS SetName, ");
		//    SQLCommand.Append("moc.EXCEED_CAPACITY AS ExceedCapacity, ");
		//    SQLCommand.Append("moc.EXCEED_BY AS ExceedBy ");
		//    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
		//    SQLCommand.Append("FROM METHOD_OVERRIDE_CAPACITY moc, STORE_GROUP_LEVEL sgl ");
		//    // end MID Track # 2354
		//    SQLCommand.Append("WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture));
		//    SQLCommand.Append(" AND moc.SGL_RID = sgl.SGL_RID");
		//    SQLCommand.Append(" ORDER BY SGL_SEQUENCE");    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35) - correct 1st time display issue   
		
		//    dt = _dba.ExecuteQuery(SQLCommand.ToString());
		//    dt.TableName = "Capacity";
		//    dt.Columns[0].ColumnName = "SglRID";
		//    dt.Columns[1].ColumnName = "Set";
		//    dt.Columns[2].ColumnName = "ExceedChar";
		//    dt.Columns[3].ColumnName = "Exceed by %";

		//    return dt;
		//}

        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		//private DataTable SetupPackRoundingTable()
		//{
		//    DataTable dt = MIDEnvironment.CreateDataTable("PackRounding");
		//    dt.Columns.Add("PackMultiple", System.Type.GetType("System.Int32"));
		//    dt.Columns.Add("FstPack", System.Type.GetType("System.Int32"));
		//    dt.Columns.Add("NthPack", System.Type.GetType("System.Int32"));
		//    StringBuilder SQLCommand = new StringBuilder();
                        
		//    SQLCommand.Append("SELECT CASE ");
		//    SQLCommand.Append("CAST(PACK_MULTIPLE_RID as varchar) ");
		//    SQLCommand.Append("WHEN '-1'  THEN (select TEXT_VALUE from APPLICATION_TEXT where TEXT_CODE = 900678) "); 
		//    SQLCommand.Append("ELSE CAST(PACK_MULTIPLE_RID as varchar) END as PackMultipletext, ");
		//    SQLCommand.Append("PACK_ROUNDING_1ST_PACK_PCT AS FstPack, ");
		//    SQLCommand.Append("PACK_ROUNDING_NTH_PACK_PCT AS NthPack, ");
		//    SQLCommand.Append("PACK_MULTIPLE_RID AS PackMultiple ");
		//    SQLCommand.Append("FROM METHOD_OVERRIDE_PACK_ROUNDING ");
		//    SQLCommand.Append("WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture));

		//    dt = _dba.ExecuteQuery(SQLCommand.ToString());

		//    dt.TableName = "PackRounding";
		//    dt.Columns[0].ColumnName = "PackText";
		//    dt.Columns[0].Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Multiple);
		//    dt.Columns[0].ReadOnly = false;
		//    dt.Columns[0].Unique = true;
		//    dt.Columns[0].AllowDBNull = false;
		//    dt.Columns[1].ColumnName = "FstPack";
		//    dt.Columns[1].DataType = System.Type.GetType("System.Double");
		//    dt.Columns[1].Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_1st_Pack);
		//    dt.Columns[1].ReadOnly = false;
		//    dt.Columns[1].Unique = false;
		//    dt.Columns[1].AllowDBNull = true;
		//    dt.Columns[2].ColumnName = "NthPack";
		//    dt.Columns[2].DataType = System.Type.GetType("System.Double");
		//    dt.Columns[2].Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Nth_Pack);
		//    dt.Columns[2].ReadOnly = false;
		//    dt.Columns[2].Unique = false;
		//    dt.Columns[2].AllowDBNull = true;
		//    dt.Columns[3].ColumnName = "PackMultiple";
		//    dt.Columns[3].Caption = "Real PM";
		//    dt.Columns[3].ReadOnly = false;
		//    dt.Columns[3].Unique = false;
		//    dt.Columns[3].AllowDBNull = false;

		//    if (dt.Rows.Count == 0)
		//    {
		//        DataRow newrow = dt.NewRow();
		//        //newrow["METHOD_RID"] = _methodRid.ToString(CultureInfo.CurrentUICulture);
		//        newrow["PackText"] = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_All_Generic_Packs);
		//        GlobalOptions opts = new GlobalOptions();
		//        DataTable dto = opts.GetGlobalOptions();
		//        DataRow dr = dto.Rows[0];
		//        newrow["FstPack"] = dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"];
		//        newrow["NthPack"] = dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"];
		//        newrow["PackMultiple"] = -1;
		//        dt.Rows.Add(newrow);
		//        dt.AcceptChanges();
		//    }

		//    return dt;
		//}
		//// END TT#616 - AGallagher - Allocation - Pack Rounding (#67)

		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;
			try
			{	
				MethodRid = method_RID;
                //Begin TT#1268-MD -jsobek -5.4 Merge

                int? STORE_GRADE_TIMEFRAME_Nullable = null;
                if (!UseStoreGradeDefault) STORE_GRADE_TIMEFRAME_Nullable = Grade_Week_Count;

                double? PERCENT_NEED_LIMIT_Nullable = null;
                if (!UsePctNeedDefault) PERCENT_NEED_LIMIT_Nullable = Percent_Need_Limit;

                double? RESERVE_Nullable = null;
                char? PERCENT_IND_Nullable = null;
                if (Reserve != Include.UndefinedReserve)
                {
                    RESERVE_Nullable = Reserve;
                    PERCENT_IND_Nullable = Percent_Ind;
                }

                int? MERCH_HN_RID_Nullable = null;
                if (Merch_Plan_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = Merch_Plan_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (Merch_Plan_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = Merch_Plan_PH_RID;

                int? ON_HAND_HN_RID_Nullable = null;
                if (Merch_OnHand_HN_RID != Include.NoRID) ON_HAND_HN_RID_Nullable = Merch_OnHand_HN_RID;

                int? ON_HAND_PH_RID_Nullable = null;
                if (Merch_OnHand_PH_RID != Include.NoRID) ON_HAND_PH_RID_Nullable = Merch_OnHand_PH_RID;

                double? ON_HAND_FACTOR_Nullable = null;
                if (!UseFactorPctDefault) ON_HAND_FACTOR_Nullable = Plan_Factor_Percent;

                int? SG_RID_Nullable = null;
                if (Store_Group_RID != Include.NoRID) SG_RID_Nullable = Store_Group_RID;

                int? STORE_GRADES_SG_RID_Nullable = null;
                if (Tab_Store_Group_RID != Include.NoRID) STORE_GRADES_SG_RID_Nullable = Tab_Store_Group_RID;

                int? IB_MERCH_HN_RID_Nullable = null;
                if (IB_MERCH_HN_RID != Include.NoRID) IB_MERCH_HN_RID_Nullable = IB_MERCH_HN_RID;

                int? IB_MERCH_PH_RID_Nullable = null;
                if (IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PH_RID_Nullable = IB_MERCH_PH_RID;

                int? IB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (IB_MERCH_PHL_SEQ != Include.NoRID) IB_MERCH_PHL_SEQUENCE_Nullable = IB_MERCH_PHL_SEQ;

                int? BEGIN_CDR_RID_Nullable = null;
                if (BeginCdrRid != Include.NoRID) BEGIN_CDR_RID_Nullable = BeginCdrRid;

                int? SHIP_TO_CDR_RID_Nullable = null;
                if (ShipToCdrRid != Include.NoRID) SHIP_TO_CDR_RID_Nullable = ShipToCdrRid; //TT#1300-MD -jsobek -Format Exception when opening a newly created GA Method (New or copied)

                char LINE_ITEM_MIN_OVERRIDE_IND;
                if (LineItemMinOverrideInd)
                {
                    LINE_ITEM_MIN_OVERRIDE_IND = '1';
                }
                else
                {
                    LINE_ITEM_MIN_OVERRIDE_IND = '0';
                }

                int? HDRIB_MERCH_HN_RID_Nullable = null;
                if (HDRIB_MERCH_HN_RID != Include.NoRID) HDRIB_MERCH_HN_RID_Nullable = HDRIB_MERCH_HN_RID;

                int? HDRIB_MERCH_PH_RID_Nullable = null;
                if (HDRIB_MERCH_PH_RID != Include.NoRID) HDRIB_MERCH_PH_RID_Nullable = HDRIB_MERCH_PH_RID;

                int? HDRIB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (HDRIB_MERCH_PHL_SEQ != Include.NoRID) HDRIB_MERCH_PHL_SEQUENCE_Nullable = HDRIB_MERCH_PHL_SEQ;
     
                StoredProcedures.MID_METHOD_GROUP_ALLOCATION_INSERT.Insert(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           STORE_GRADE_TIMEFRAME: STORE_GRADE_TIMEFRAME_Nullable,
                                                                           EXCEED_MAX_IND: Exceed_Maximums_Ind,
                                                                           PERCENT_NEED_LIMIT: PERCENT_NEED_LIMIT_Nullable,
                                                                           RESERVE: RESERVE_Nullable,
                                                                           PERCENT_IND: PERCENT_IND_Nullable,
                                                                           MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                           MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                           MERCH_PHL_SEQUENCE: Merch_Plan_PHL_SEQ,
                                                                           ON_HAND_HN_RID: ON_HAND_HN_RID_Nullable,
                                                                           ON_HAND_PH_RID: ON_HAND_PH_RID_Nullable,
                                                                           ON_HAND_PHL_SEQUENCE: Merch_OnHand_PHL_SEQ,
                                                                           ON_HAND_FACTOR: ON_HAND_FACTOR_Nullable,
                                                                           SG_RID: SG_RID_Nullable,
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
                                                                           BEGIN_CDR_RID: BEGIN_CDR_RID_Nullable,
                                                                           SHIP_TO_CDR_RID: SHIP_TO_CDR_RID_Nullable,
                                                                           LINE_ITEM_MIN_OVERRIDE_IND: LINE_ITEM_MIN_OVERRIDE_IND,
                                                                           LINE_ITEM_MIN_OVERRIDE: LineItemMinOverride,
                                                                           HDRINVENTORY_IND: _hdrinventory_Ind,
                                                                           HDRIB_MERCH_TYPE: 3,
                                                                           HDRIB_MERCH_HN_RID: HDRIB_MERCH_HN_RID_Nullable,
                                                                           HDRIB_MERCH_PH_RID: HDRIB_MERCH_PH_RID_Nullable,
                                                                           HDRIB_MERCH_PHL_SEQUENCE: HDRIB_MERCH_PHL_SEQUENCE_Nullable
                                                                           );
                //End TT#1268-MD -jsobek -5.4 Merge
				
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
				if (_dsGroupAllocation == null)
					return UpdateSuccessful;
				 
				DataView dv = new DataView();
				//dv.Table = _dsGroupAllocation.Tables["Colors"];
				//for (int i = 0; i < dv.Count; i++)
				//{
				//    string addColor = BuildInsertColorStatement(dv[i]);
				//    _dba.ExecuteNonQuery(addColor);
				//}
	
				dv.Table = _dsGroupAllocation.Tables["StoreGrades"];
				//===========================
				// Store Grade Boundaries
				//===========================
				InitGradeBoundaryHash();
				for (int i = 0; i < dv.Count; i++)
				{
                    //string addGrade = BuildInsertGradeBoundaryStatement(dv[i]);  //TT#1268-MD -jsobek -5.4 Merge

                    //===============================================================
                    // If we already have processed the grade, we don't build a row.
                    //===============================================================
                    string gradeCode = dv[i]["Grade"].ToString();
                    if (!_storeGradeHash.ContainsKey(gradeCode))
                    {
                        //Begin TT#1268-MD -jsobek -5.4 Merge
                        //StringBuilder SQLCommand = new StringBuilder();

                        //SQLCommand.Append("INSERT INTO METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY (");
                        //SQLCommand.Append("METHOD_RID, BOUNDARY, GRADE_CODE) ");
                        //SQLCommand.Append("VALUES(");
                        //SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
                        //SQLCommand.Append(", ");
                        //SQLCommand.Append(dv[i]["Boundary"]);
                        //SQLCommand.Append(", ");
                        //if (dv[i]["Grade"] == System.DBNull.Value)
                        //    SQLCommand.Append("null");
                        //else
                        //{
                        //    SQLCommand.Append("'");
                        //    SQLCommand.Append(Convert.ToString(dv[i]["Grade"], CultureInfo.CurrentUICulture));
                        //    SQLCommand.Append("'");
                        //}
                        //SQLCommand.Append(")");

                        _storeGradeHash.Add(gradeCode, gradeCode);

					    //_dba.ExecuteNonQuery(addGrade);


                        string GRADE_CODE_Nullable = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                        if (dv[i]["Grade"] != System.DBNull.Value) GRADE_CODE_Nullable = (string)dv[i]["Grade"];

                        StoredProcedures.MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT.Insert(_dba,
                                                                                                         METHOD_RID: MethodRid,
                                                                                                         BOUNDARY: (int)dv[i]["Boundary"],
                                                                                                         GRADE_CODE: GRADE_CODE_Nullable
                                                                                                         );
                        //End TT#1268-MD -jsobek -5.4 Merge

                    }
				}
				//=====================
				// Store Grade Values
				//=====================
				for (int i = 0; i < dv.Count; i++)
				{
                    //Begin TT#1268-MD -jsobek -5.4 Merge
                    //string addGrade = BuildInsertGradeValueStatement(dv[i]);
                    //if (addGrade != null)
                    //{
                    //    _dba.ExecuteNonQuery(addGrade);
                    //}
                    
                    bool doAddGrade = true;

                    //====================================================
                    // If not values are populated, we don't build a row.
                    //====================================================
                    if (dv[i]["Group Min"] == DBNull.Value
                        && dv[i]["Group Max"] == DBNull.Value
                        && dv[i]["Header Min"] == DBNull.Value
                        && dv[i]["Header Max"] == DBNull.Value)
                    {
                        doAddGrade = false;
                    }

                    if (doAddGrade)
                    {
                        int? MINIMUM_GROUP_Nullable = null;
                        if (dv[i]["Group Min"] != System.DBNull.Value) MINIMUM_GROUP_Nullable = (int)dv[i]["Group Min"];

                        int? MAXIMUM_GROUP_Nullable = null;
                        if (dv[i]["Group Max"] != System.DBNull.Value) MAXIMUM_GROUP_Nullable = (int)dv[i]["Group Max"];

                        int? MINIMUM_HEADER_Nullable = null;
                        if (dv[i]["Header Min"] != System.DBNull.Value) MINIMUM_HEADER_Nullable = (int)dv[i]["Header Min"];

                        int? MAXIMUM_HEADER_Nullable = null;
                        if (dv[i]["Header Max"] != System.DBNull.Value) MAXIMUM_HEADER_Nullable = (int)dv[i]["Header Max"];

                        int? SGL_RID_Nullable = null;
                        if (dv[i]["SGLRID"] != System.DBNull.Value) SGL_RID_Nullable = (int)dv[i]["SGLRID"];

                        StoredProcedures.MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT.Insert(_dba,
                                                                                       METHOD_RID: MethodRid,
                                                                                       BOUNDARY: (int)dv[i]["Boundary"],
                                                                                       MINIMUM_GROUP: MINIMUM_GROUP_Nullable,
                                                                                       MAXIMUM_GROUP: MAXIMUM_GROUP_Nullable,
                                                                                       MINIMUM_HEADER: MINIMUM_HEADER_Nullable,
                                                                                       MAXIMUM_HEADER: MAXIMUM_HEADER_Nullable,
                                                                                       SGL_RID: SGL_RID_Nullable
                                                                                       );
                    }
                    //End TT#1268-MD -jsobek -5.4 Merge
				}
				// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				//dv.Table = _dsGroupAllocation.Tables["Capacity"];
				//for (int i = 0; i < dv.Count; i++)
				//{
				//    string addGrade = BuildInsertCapacityStatement( dv[i]);
				//    _dba.ExecuteNonQuery(addGrade);
				//}
                // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
				//dv.Table = _dsGroupAllocation.Tables["PackRounding"];
				//for (int i = 0; i < dv.Count; i++)
				//{
				//    string addGrade = BuildInsertPackRoundingStatement(dv[i]);
				//    _dba.ExecuteNonQuery(addGrade);
				//}
                // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)

                // BEGIN TT#1401 - GTaylor - Reservation Stores
//                dv.Table = _imoDataSet.Tables["Stores"];
//                string addVSW;
//                for (int i = 0; i < dv.Count; i++)
//                {
////                    if ((dv[i]["Updated"].Equals(true)) && !(dv[i]["Reservation Store"].Equals("")))
//                    if (!(dv[i]["Reservation Store"].Equals("")))
//                    {
//                        addVSW = BuildDeleteVSWIMO(dv[i]);
//                        _dba.ExecuteNonQuery(addVSW);
//                        addVSW = BuildInsertVSWIMO(dv[i]);
//                        // BEGIN TT#1401 - stodd - VSW
//                        if (addVSW != null)
//                        {
//                            _dba.ExecuteNonQuery(addVSW);
//                        }
//                        // END TT#1401 - stodd - VSW
//                    }
//                }
				//addVSW = BuildUpdateMethodOverrideVSWIMO(ApplyVSW);
				//_dba.ExecuteNonQuery(addVSW);
                // END TT#1401 - GTaylor - Reservation Stores
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

        // BEGIN TT#1401 - GTaylor - Reservation Stores
		//private string BuildDeleteVSWIMO(DataRowView rv)
		//{
		//    StringBuilder SQLCommand = new StringBuilder();

		//    SQLCommand.Append("DELETE FROM METHOD_OVERRIDE_IMO WHERE ");
		//    SQLCommand.Append("METHOD_RID = ");
		//    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
		//    SQLCommand.Append(" AND ");
		//    SQLCommand.Append("ST_RID = ");
		//    SQLCommand.Append(rv["Store RID"]);
		//    return SQLCommand.ToString(); 
		//}
		////  update the Method Override table on demand
		//private string BuildUpdateMethodOverrideVSWIMO(bool imoApplySW)
		//{
		//    StringBuilder SQLCommand = new StringBuilder();

		//    SQLCommand.Append("UPDATE METHOD_OVERRIDE SET ");
		//    SQLCommand.Append("IMO_APPLY_VSW = ");
		//    //SQLCommand.Append(imoApplySW.ToString());
		//    SQLCommand.Append((imoApplySW ? "1" : "0"));
		//    SQLCommand.Append(" WHERE METHOD_RID = ");
		//    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
		//    return SQLCommand.ToString(); 
		//}
		//private string BuildInsertVSWIMO(DataRowView rv)
		//{
		//    StringBuilder SQLCommand = new StringBuilder();
		//    double PctPackThreshold = 0.0;

		//    // BEGIN TT#1401 - stodd - VSW
		//    if (
		//        ((rv["Item Max"] == System.DBNull.Value) || (rv["Item Max"].Equals("")))
		//        &&
		//        ((rv["Min Ship Qty"] == System.DBNull.Value) || (rv["Min Ship Qty"].Equals("")))
		//        &&
		//        ((rv["Pct Pack Threshold"] == System.DBNull.Value) || (rv["Pct Pack Threshold"].Equals("")))
		//        )
		//    {
		//        return null;
		//    }
		//    else
		//    {
		//    // END TT#1401 - stodd - VSW

		//        SQLCommand.Append("INSERT INTO METHOD_OVERRIDE_IMO (");
		//        SQLCommand.Append("METHOD_RID, ST_RID, IMO_MIN_SHIP_QTY, IMO_PCT_PK_THRSHLD, IMO_MAX_VALUE) ");
		//        SQLCommand.Append("VALUES(");
		//        SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
		//        SQLCommand.Append(", ");

		//        if ((rv["Store RID"] == System.DBNull.Value) || (rv["Store RID"].Equals("")))
		//            SQLCommand.Append("null");
		//        else
		//            SQLCommand.Append(rv["Store RID"]);
		//        SQLCommand.Append(", ");

		//        if ((rv["Min Ship Qty"] == System.DBNull.Value) || (rv["Min Ship Qty"].Equals("")))
		//            SQLCommand.Append("null");
		//        else
		//            SQLCommand.Append(rv["Min Ship Qty"]);
		//        SQLCommand.Append(", ");

		//        if ((rv["Pct Pack Threshold"] == System.DBNull.Value) ||
		//            (rv["Pct Pack Threshold"].Equals("")) ||
		//            (rv["Pct Pack Threshold"].Equals(Include.PercentPackThresholdDefault)))
		//            SQLCommand.Append("null");
		//        else
		//        {
		//            PctPackThreshold = (Convert.ToDouble(rv["Pct Pack Threshold"]) / 100);
		//            SQLCommand.Append(PctPackThreshold);
		//        }
		//        SQLCommand.Append(", ");

		//        if ((rv["Item Max"] == System.DBNull.Value) || (rv["Item Max"].Equals("")))
		//            SQLCommand.Append("null");
		//        else
		//            SQLCommand.Append(rv["Item Max"]);
		//        SQLCommand.Append(")");

		//        return SQLCommand.ToString();
		//    }	// TT#1401 - stodd - VSW
		//}
		//// END TT#1401 - GTaylor - Reservation Stores

		//private string BuildInsertColorStatement(DataRowView rv)
		//{	
		//    StringBuilder SQLCommand = new StringBuilder();

		//    SQLCommand.Append("INSERT INTO METHOD_OVERRIDE_COLOR_MINMAX (");
		//    SQLCommand.Append("METHOD_RID, COLOR_CODE_RID, COLOR_MIN, COLOR_MAX) ");
		//    SQLCommand.Append("VALUES(");
		//    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
		//    SQLCommand.Append(", ");
		//    SQLCommand.Append(rv["Color"]);
		//    SQLCommand.Append(", ");
		//    if (rv["Minimum"] == System.DBNull.Value)
		//        SQLCommand.Append("null");
		//    else
		//        SQLCommand.Append(rv["Minimum"]);
		//    SQLCommand.Append(", ");
		//    if (rv["Maximum"] == System.DBNull.Value)
		//        SQLCommand.Append("null");
		//    else
		//        SQLCommand.Append(rv["Maximum"]); 
		//    SQLCommand.Append(")");
			
		//    return SQLCommand.ToString();;
		//}
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
		// BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
        //private string BuildInsertGradeBoundaryStatement(DataRowView rv)
        //{			
        //    //===============================================================
        //    // If we already have processed the grade, we don't build a row.
        //    //===============================================================
        //    string gradeCode = rv["Grade"].ToString();
        //    if (_storeGradeHash.ContainsKey(gradeCode))
        //    {
        //        return null;
        //    }

        //    StringBuilder SQLCommand = new StringBuilder();

        //    SQLCommand.Append("INSERT INTO METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY (");
        //    SQLCommand.Append("METHOD_RID, BOUNDARY, GRADE_CODE) ");
        //    SQLCommand.Append("VALUES(");
        //    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
        //    SQLCommand.Append(", ");
        //    SQLCommand.Append(rv["Boundary"]);
        //    SQLCommand.Append(", ");
        //    if (rv["Grade"] == System.DBNull.Value)
        //        SQLCommand.Append("null");
        //    else
        //    {
        //        SQLCommand.Append("'");
        //        SQLCommand.Append(Convert.ToString(rv["Grade"], CultureInfo.CurrentUICulture));
        //        SQLCommand.Append("'");
        //    }
        //    SQLCommand.Append(")");

        //    _storeGradeHash.Add(gradeCode, gradeCode);

        //    return SQLCommand.ToString(); ;
        //}
		// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
        //private string BuildInsertGradeValueStatement(DataRowView rv)
        //{
        //    // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        //    //====================================================
        //    // If not values are populated, we don't build a row.
        //    //====================================================
        //    if (rv["Group Min"] == DBNull.Value
        //        && rv["Group Max"] == DBNull.Value
        //        && rv["Header Min"] == DBNull.Value
        //        && rv["Header Max"] == DBNull.Value)
        //    {
        //        return null;
        //    }
        //    // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

        //    StringBuilder SQLCommand = new StringBuilder();

        //    SQLCommand.Append("INSERT INTO METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES (");
        //    SQLCommand.Append("METHOD_RID, BOUNDARY, MINIMUM_GROUP, ");
        //    SQLCommand.Append("MAXIMUM_GROUP, MINIMUM_HEADER, MAXIMUM_HEADER, SGL_RID) ");  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)  // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)  
        //    SQLCommand.Append("VALUES(");
        //    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
        //    SQLCommand.Append(", ");
        //    SQLCommand.Append(rv["Boundary"]);
        //    SQLCommand.Append(", ");
        //    if (rv["Group Min"] == System.DBNull.Value)
        //        SQLCommand.Append("null");
        //    else
        //        SQLCommand.Append(rv["Group Min"]);
        //    SQLCommand.Append(", ");
        //    if (rv["Group Max"] == System.DBNull.Value)
        //        SQLCommand.Append("null");
        //    else
        //        SQLCommand.Append(rv["Group Max"]);
        //    SQLCommand.Append(", ");
        //    //if (rv["Min Ad"] == System.DBNull.Value)
        //    //    SQLCommand.Append("null");
        //    //else
        //    //    SQLCommand.Append(rv["Min Ad"]);
        //    //SQLCommand.Append(", ");
        //    if (rv["Header Min"] == System.DBNull.Value)
        //        SQLCommand.Append("null");
        //    else
        //        SQLCommand.Append(rv["Header Min"]);
        //    SQLCommand.Append(", ");
        //    if (rv["Header Max"] == System.DBNull.Value)
        //        SQLCommand.Append("null");
        //    else
        //        SQLCommand.Append(rv["Header Max"]);
        //    // BEGIN TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
        //    SQLCommand.Append(", ");
        //    //if (rv["Ship Up To"] == System.DBNull.Value)
        //    //    SQLCommand.Append("null");
        //    //else
        //    //    SQLCommand.Append(rv["Ship Up To"]);
        //    //// END TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
        //    //// BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        //    //SQLCommand.Append(", ");
        //    if (rv["SGLRID"] == System.DBNull.Value)
        //        SQLCommand.Append("null");
        //    else
        //        SQLCommand.Append(rv["SGLRID"]);
        //    // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

        //    SQLCommand.Append(")");

			
        //    return SQLCommand.ToString();;
        //}	
		//private string BuildInsertCapacityStatement(DataRowView rv)
		//{	
		//    StringBuilder SQLCommand = new StringBuilder();

		//    SQLCommand.Append("INSERT INTO METHOD_OVERRIDE_CAPACITY (");
		//    SQLCommand.Append("METHOD_RID, SGL_RID, EXCEED_CAPACITY, EXCEED_BY) ");
		//    SQLCommand.Append("VALUES(");
		//    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
		//    SQLCommand.Append(", ");
		//    SQLCommand.Append(rv["SglRID"]);
		//    SQLCommand.Append(", ");
		//    SQLCommand.Append(rv["ExceedChar"]);
		//    SQLCommand.Append(", ");
		//    if (rv["Exceed by %"] == System.DBNull.Value)
		//        SQLCommand.Append("null");
		//    else
		//        SQLCommand.Append(rv["Exceed by %"]);
		//    SQLCommand.Append(")");
			
		//    return SQLCommand.ToString();;
		//}
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		//private string BuildInsertPackRoundingStatement(DataRowView rv)
		//{
		//    StringBuilder SQLCommand = new StringBuilder();

		//    SQLCommand.Append("INSERT INTO METHOD_OVERRIDE_PACK_ROUNDING (");
		//    SQLCommand.Append("METHOD_RID, PACK_MULTIPLE_RID, PACK_ROUNDING_1ST_PACK_PCT, PACK_ROUNDING_NTH_PACK_PCT) ");
		//    SQLCommand.Append("VALUES(");
		//    SQLCommand.Append(MethodRid.ToString(CultureInfo.CurrentUICulture));
		//    SQLCommand.Append(", ");
		//    SQLCommand.Append(rv["PackMultiple"]);
		//    SQLCommand.Append(", ");
		//    // BEgin TT#616 - stodd - pack rounding
		//    if (rv["FstPack"] == DBNull.Value)
		//    {
		//        SQLCommand.Append("null");
		//    }
		//    else
		//    {
		//        SQLCommand.Append(rv["FstPack"]);
		//    }
		//    SQLCommand.Append(", ");
		//    if (rv["NthPack"] == DBNull.Value)
		//    {
		//        SQLCommand.Append("null");
		//    }
		//    else
		//    {
		//        SQLCommand.Append(rv["NthPack"]);
		//    }
		//    // End TT#616 - stodd - pack rounding

		//    SQLCommand.Append(")");

		//    return SQLCommand.ToString(); ; 
		//}
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		public bool UpdateMethod(int method_RID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			_dba = td.DBA;
			try
			{
				if (   DeleteChildData() 
					&& UpdateMethod(method_RID) 
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


		public bool UpdateMethod(int method_RID)
		{
			bool UpdateSuccessful = true;
			try
			{

                //Begin TT#1268-MD -jsobek -5.4 Merge

                int? STORE_GRADE_TIMEFRAME_Nullable = null;
                if (!UseStoreGradeDefault) STORE_GRADE_TIMEFRAME_Nullable = Grade_Week_Count;

                double? PERCENT_NEED_LIMIT_Nullable = null;
                if (!UsePctNeedDefault) PERCENT_NEED_LIMIT_Nullable = Percent_Need_Limit;

                double? RESERVE_Nullable = null;
                char? PERCENT_IND_Nullable = null;
                if (Reserve != Include.UndefinedReserve)
                {
                    RESERVE_Nullable = Reserve;
                    PERCENT_IND_Nullable = Percent_Ind;
                }

                int? MERCH_HN_RID_Nullable = null;
                if (Merch_Plan_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = Merch_Plan_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (Merch_Plan_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = Merch_Plan_PH_RID;

                int? ON_HAND_HN_RID_Nullable = null;
                if (Merch_OnHand_HN_RID != Include.NoRID) ON_HAND_HN_RID_Nullable = Merch_OnHand_HN_RID;

                int? ON_HAND_PH_RID_Nullable = null;
                if (Merch_OnHand_PH_RID != Include.NoRID) ON_HAND_PH_RID_Nullable = Merch_OnHand_PH_RID;

                double? ON_HAND_FACTOR_Nullable = null;
                if (!UseFactorPctDefault) ON_HAND_FACTOR_Nullable = Plan_Factor_Percent;

                int? SG_RID_Nullable = null;
                if (Store_Group_RID != Include.NoRID) SG_RID_Nullable = Store_Group_RID;

                int? STORE_GRADES_SG_RID_Nullable = null;
                if (Tab_Store_Group_RID != Include.NoRID) STORE_GRADES_SG_RID_Nullable = Tab_Store_Group_RID;

                int? IB_MERCH_HN_RID_Nullable = null;
                if (IB_MERCH_HN_RID != Include.NoRID) IB_MERCH_HN_RID_Nullable = IB_MERCH_HN_RID;

                int? IB_MERCH_PH_RID_Nullable = null;
                if (IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PH_RID_Nullable = IB_MERCH_PH_RID;

                int? IB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (IB_MERCH_PHL_SEQ != Include.NoRID) IB_MERCH_PHL_SEQUENCE_Nullable = IB_MERCH_PHL_SEQ;

                int? BEGIN_CDR_RID_Nullable = null;
                if (BeginCdrRid != Include.NoRID) BEGIN_CDR_RID_Nullable = BeginCdrRid;

                int? SHIP_TO_CDR_RID_Nullable = null;
                if (ShipToCdrRid != Include.NoRID) SHIP_TO_CDR_RID_Nullable = ShipToCdrRid; //TT#1300-MD -jsobek -Format Exception when opening a newly created GA Method (New or copied)

                char LINE_ITEM_MIN_OVERRIDE_IND;
                if (LineItemMinOverrideInd)
                {
                    LINE_ITEM_MIN_OVERRIDE_IND = '1';
                }
                else
                {
                    LINE_ITEM_MIN_OVERRIDE_IND = '0';
                }

                int? HDRIB_MERCH_HN_RID_Nullable = null;
                if (HDRIB_MERCH_HN_RID != Include.NoRID) HDRIB_MERCH_HN_RID_Nullable = HDRIB_MERCH_HN_RID;

                int? HDRIB_MERCH_PH_RID_Nullable = null;
                if (HDRIB_MERCH_PH_RID != Include.NoRID) HDRIB_MERCH_PH_RID_Nullable = HDRIB_MERCH_PH_RID;

                int? HDRIB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (HDRIB_MERCH_PHL_SEQ != Include.NoRID) HDRIB_MERCH_PHL_SEQUENCE_Nullable = HDRIB_MERCH_PHL_SEQ;


                StoredProcedures.MID_METHOD_GROUP_ALLOCATION_UPDATE.Update(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           STORE_GRADE_TIMEFRAME: STORE_GRADE_TIMEFRAME_Nullable,
                                                                           EXCEED_MAX_IND: Exceed_Maximums_Ind,
                                                                           PERCENT_NEED_LIMIT: PERCENT_NEED_LIMIT_Nullable,
                                                                           RESERVE: RESERVE_Nullable,
                                                                           PERCENT_IND: PERCENT_IND_Nullable,
                                                                           MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                           MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                           MERCH_PHL_SEQUENCE: Merch_Plan_PHL_SEQ,
                                                                           ON_HAND_HN_RID: ON_HAND_HN_RID_Nullable,
                                                                           ON_HAND_PH_RID: ON_HAND_PH_RID_Nullable,
                                                                           ON_HAND_PHL_SEQUENCE: Merch_OnHand_PHL_SEQ,
                                                                           ON_HAND_FACTOR: ON_HAND_FACTOR_Nullable,
                                                                           SG_RID: SG_RID_Nullable,
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
                                                                           BEGIN_CDR_RID: BEGIN_CDR_RID_Nullable,
                                                                           SHIP_TO_CDR_RID: SHIP_TO_CDR_RID_Nullable,
                                                                           LINE_ITEM_MIN_OVERRIDE_IND: LINE_ITEM_MIN_OVERRIDE_IND,
                                                                           LINE_ITEM_MIN_OVERRIDE: LineItemMinOverride,
                                                                           HDRINVENTORY_IND: _hdrinventory_Ind,
                                                                           HDRIB_MERCH_TYPE: 3,
                                                                           HDRIB_MERCH_HN_RID: HDRIB_MERCH_HN_RID_Nullable,
                                                                           HDRIB_MERCH_PH_RID: HDRIB_MERCH_PH_RID_Nullable,
                                                                           HDRIB_MERCH_PHL_SEQUENCE: HDRIB_MERCH_PHL_SEQUENCE_Nullable
                                                                           );
                //End TT#1268-MD -jsobek -5.4 Merge
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
                    //Begin TT#1268-MD -jsobek -5.4 Merge
                    //StringBuilder SQLCommand = new StringBuilder();

                    //SQLCommand.Append("DELETE FROM METHOD_GROUP_ALLOCATION ");
                    //SQLCommand.Append("WHERE ");
                    //SQLCommand.Append("METHOD_RID = ");
                    //SQLCommand.Append(method_RID.ToString(CultureInfo.CurrentUICulture));

                    //_dba.ExecuteNonQuery(SQLCommand.ToString());

                    StoredProcedures.MID_METHOD_GROUP_ALLOCATION_DELETE.Delete(_dba, METHOD_RID: method_RID);
                    //End TT#1268-MD -jsobek -5.4 Merge

	
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
                //Begin TT#1268-MD -jsobek -5.4 Merge
                ////string delColors = "DELETE FROM METHOD_GROUP_ALLOCATION_COLOR_MINMAX WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture);
                //// BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                ////string delStoreGrades = "DELETE FROM METHOD_OVERRIDE_STORE_GRADES WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture);
                //string delStoreGradesValues = "DELETE FROM METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture);
                //string delStoreGradesBoundary = "DELETE FROM METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture);
                //// END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                ////string delCapacity = "DELETE FROM METHOD_OVERRIDE_CAPACITY WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture);
                ////string delPackRounding = "DELETE FROM METHOD_OVERRIDE_PACK_ROUNDING WHERE METHOD_RID = " + _methodRid.ToString(CultureInfo.CurrentUICulture);  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)

                ////_dba.ExecuteNonQuery(delColors);
                //// BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                ////_dba.ExecuteNonQuery(delStoreGrades);
                //_dba.ExecuteNonQuery(delStoreGradesValues);
                //_dba.ExecuteNonQuery(delStoreGradesBoundary);
                //// END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                ////_dba.ExecuteNonQuery(delCapacity);
                ////_dba.ExecuteNonQuery(delPackRounding);  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
              
                StoredProcedures.MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE.Delete(_dba, METHOD_RID: _methodRid);
                StoredProcedures.MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE.Delete(_dba, METHOD_RID: _methodRid);
                //End TT#1268-MD -jsobek -5.4 Merge

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
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //StringBuilder SQLCommand = new StringBuilder();		
                //SQLCommand.Append("SELECT mo.METHOD_RID, m.METHOD_NAME, m.METHOD_TYPE_ID, ");
                //SQLCommand.Append("m.USER_RID, au.USER_NAME ");
                //SQLCommand.Append("FROM METHOD_GROUP_ALLOCATION mo, METHOD m, APPLICATION_USER au ");
                //SQLCommand.Append("WHERE ");
                //SQLCommand.Append("mo.MERCH_HN_RID = ");
                //SQLCommand.Append(aNodeRID);
                //SQLCommand.Append(" and mo.METHOD_RID = m.METHOD_RID ");
                //SQLCommand.Append(" and m.USER_RID = au.USER_RID ");

                //return _dba.ExecuteQuery(SQLCommand.ToString());
                return StoredProcedures.MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE.Read(_dba, MERCH_HN_RID: aNodeRID);
                //End TT#1268-MD -jsobek -5.4 Merge
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
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //StringBuilder SQLCommand = new StringBuilder();		
                //SQLCommand.Append("SELECT mo.METHOD_RID, m.METHOD_NAME, m.METHOD_TYPE_ID, ");
                //SQLCommand.Append("m.USER_RID, au.USER_NAME ");
                //SQLCommand.Append("FROM METHOD_GROUP_ALLOCATION mo, METHOD m, APPLICATION_USER au ");
                //SQLCommand.Append("WHERE ");
                //SQLCommand.Append("mo.ON_HAND_HN_RID = ");
                //SQLCommand.Append(aNodeRID);
                //SQLCommand.Append(" and mo.METHOD_RID = m.METHOD_RID ");
                //SQLCommand.Append(" and m.USER_RID = au.USER_RID ");

                //return _dba.ExecuteQuery(SQLCommand.ToString());
                return StoredProcedures.MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE.Read(_dba, ON_HAND_HN_RID: aNodeRID);
                //End TT#1268-MD -jsobek -5.4 Merge
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
	}
}

