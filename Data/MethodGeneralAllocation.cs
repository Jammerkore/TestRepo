using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for METHOD_GENERAL_ALLOCATION
	/// </summary>
	public class GeneralAllocationMethodData: MethodBaseData
	{
		private int _Begin_CDR_RID;
		private int _Ship_To_CDR_RID;
		private int _Merch_HN_RID;
		private int _Merch_PH_RID;
		private int _Merch_PHL_SEQ;
		private int _Gen_Alloc_HDR_RID;
		private double _Reserve;
		private char _Percent_Ind;
		private eMerchandiseType _merchandiseType;
		// BEGIN TT#667 - Stodd - Pre-allocate Reserve
		private double _reserveAsBulk;
		private double _reserveAsPacks;
		// END TT#667 - Stodd - Pre-allocate Reserve

		/// <summary>
		/// Gets or sets Merchandise Type. 
		/// </summary>
		public eMerchandiseType MerchandiseType
		{
			get	{return _merchandiseType;}
			set	{_merchandiseType = value;	}
		}
		public int Begin_CDR_RID
		{
			get{return _Begin_CDR_RID;}
			set{_Begin_CDR_RID = value;	}
		}
		public int Ship_To_CDR_RID
		{
			get{return _Ship_To_CDR_RID;}
			set{_Ship_To_CDR_RID = value;	}
		}
		public int Merch_HN_RID
		{
			get{return _Merch_HN_RID;}
			set{_Merch_HN_RID = value;	}
		}
		public int Merch_PH_RID
		{
			get{return _Merch_PH_RID;}
			set{_Merch_PH_RID = value;	}
		}
		public int Merch_PHL_SEQ
		{
			get{return _Merch_PHL_SEQ;}
			set{_Merch_PHL_SEQ = value;	}
		}
		public int Gen_Alloc_HDR_RID
		{
			get{return _Gen_Alloc_HDR_RID;}
			set{_Gen_Alloc_HDR_RID = value;	}
		}
		public double Reserve
		{
			get{return _Reserve;}
			set{_Reserve = value;	}
		}
		public char Percent_Ind
		{
			get{return _Percent_Ind;}
			set{_Percent_Ind = value;	}
		}

		// BEGIN TT#667 - Stodd - Pre-allocate Reserve
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

		/// <summary>
		/// Creates an instance of the GeneralAllocationMethodData class.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		public GeneralAllocationMethodData(int method_RID, eChangeType changeType)
		{
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateGeneralAllocation(method_RID);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the GeneralAllocationMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="method_RID">The record ID of the method</param>
		public GeneralAllocationMethodData(TransactionData td, int method_RID)
		{
			//
			// TODO: Add constructor logic here
			//
			_dba = td.DBA;
			_Gen_Alloc_HDR_RID = Include.NoRID;
		}

		/// <summary>
		/// Creates an instance of the GeneralAllocationMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		public GeneralAllocationMethodData(TransactionData td)
		{
			//
			// TODO: Add constructor logic here
			//
			_dba = td.DBA;
			_Gen_Alloc_HDR_RID = Include.NoRID;
		}

		/// <summary>
		/// Creates an instance of the GeneralAllocationMethodData class.
		/// </summary>
		public GeneralAllocationMethodData()
		{
			_Gen_Alloc_HDR_RID = Include.NoRID;
		}
		public bool PopulateGeneralAllocation(int method_RID)
		{
			try
			{
				if (PopulateMethod(method_RID))
				{
                    //StringBuilder SQLCommand = new StringBuilder();
                    //SQLCommand.Append("SELECT BEGIN_CDR_RID, SHIP_TO_CDR_RID, MERCH_HN_RID, ");
                    //SQLCommand.Append("MERCH_PH_RID, MERCH_PHL_SEQUENCE, MERCH_TYPE, ");
                    //// BEGIN TT#667 - Stodd - Pre-allocate Reserve
                    //SQLCommand.Append("GEN_ALLOC_HDR_RID, RESERVE, PERCENT_IND, COALESCE(RESERVE_AS_BULK,0) as RESERVE_AS_BULK, COALESCE(RESERVE_AS_PACKS,0) as RESERVE_AS_PACKS ");
                    //// END TT#667 - Stodd - Pre-allocate Reserve
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand.Append("FROM METHOD_GENERAL_ALLOCATION WHERE METHOD_RID = ");
                    //// end MID Track # 2354
                    //SQLCommand.Append(method_RID);

					DataTable dtGenAlloc = MIDEnvironment.CreateDataTable();
					//dtGenAlloc = _dba.ExecuteSQLQuery( SQLCommand.ToString(), "GenAlloc" );
                    dtGenAlloc = StoredProcedures.MID_METHOD_GENERAL_ALLOCATION_READ.Read(_dba, METHOD_RID: method_RID);

					if(dtGenAlloc.Rows.Count != 0)
					{
						DataRow dr = dtGenAlloc.Rows[0];
						_Begin_CDR_RID = Convert.ToInt32(dr["BEGIN_CDR_RID"], CultureInfo.CurrentUICulture);
						_Ship_To_CDR_RID = Convert.ToInt32(dr["SHIP_TO_CDR_RID"], CultureInfo.CurrentUICulture);
						_merchandiseType = (eMerchandiseType)(Convert.ToInt32(dr["MERCH_TYPE"], CultureInfo.CurrentUICulture));

						if (dr["MERCH_HN_RID"] != System.DBNull.Value)
							_Merch_HN_RID = Convert.ToInt32(dr["MERCH_HN_RID"], CultureInfo.CurrentUICulture);
						else
							_Merch_HN_RID = Include.NoRID;
						if (dr["MERCH_PH_RID"] != System.DBNull.Value)
						{
							_Merch_PH_RID = Convert.ToInt32(dr["MERCH_PH_RID"], CultureInfo.CurrentUICulture);
							_Merch_PHL_SEQ = Convert.ToInt32(dr["MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
						}
						else
						{
							_Merch_PH_RID = Include.NoRID;
							_Merch_PHL_SEQ = 0;		
						}	
						if (dr["GEN_ALLOC_HDR_RID"] != System.DBNull.Value)
							_Gen_Alloc_HDR_RID = Convert.ToInt32(dr["GEN_ALLOC_HDR_RID"], CultureInfo.CurrentUICulture);
						else
							_Gen_Alloc_HDR_RID = Include.NoRID;
						if (dr["RESERVE"] != System.DBNull.Value)
							_Reserve = Convert.ToDouble(dr["RESERVE"], CultureInfo.CurrentUICulture);
						else
							_Reserve = Include.UndefinedReserve;
						if (dr["PERCENT_IND"] != System.DBNull.Value)
							_Percent_Ind = Convert.ToChar(dr["PERCENT_IND"], CultureInfo.CurrentUICulture);				
						//						else
						//							_Percent_Ind = Convert.ToChar(" ");

						// BEGIN TT#667 - Stodd - Pre-allocate Reserve
						_reserveAsBulk = double.Parse(dr["RESERVE_AS_BULK"].ToString());
						_reserveAsPacks = double.Parse(dr["RESERVE_AS_PACKS"].ToString());
						// BEGIN TT#667 - Stodd - Pre-allocate Reserve

						return true;
					}
					else
					{
						_Begin_CDR_RID = Include.UndefinedCalendarDateRange;
						_Ship_To_CDR_RID = Include.UndefinedCalendarDateRange;
						_Merch_PH_RID = Include.NoRID;
						_Merch_PHL_SEQ = 0;	
						_Gen_Alloc_HDR_RID = Include.NoRID;
						_Reserve = Include.UndefinedReserve;
						// BEGIN TT#667 - Stodd - Pre-allocate Reserve
						_reserveAsBulk = 0;
						_reserveAsPacks = 0;
						// BEGIN TT#667 - Stodd - Pre-allocate Reserve

						return false;
					}
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
//		public bool InsertGeneralAllocation(Object [] values, DatabaseAccess dba)
		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessfull = true;
			try
			{	
                
                int? MERCH_HN_RID_Nullable = null;
                if (Merch_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = Merch_HN_RID;

                int? MERCH_PH_RID_Nullable = null;
                if (Merch_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = Merch_PH_RID;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (Merch_PH_RID != Include.NoRID) MERCH_PHL_SEQUENCE_Nullable = Merch_PHL_SEQ;

                int? GEN_ALLOC_HDR_RID_Nullable = null;
                if (Gen_Alloc_HDR_RID != Include.NoRID) GEN_ALLOC_HDR_RID_Nullable = Gen_Alloc_HDR_RID;

                double? RESERVE_Nullable = null;
                if (Reserve != Include.UndefinedReserve) RESERVE_Nullable = Reserve;

                char? PERCENT_IND_Nullable = null;
                if (Reserve != Include.UndefinedReserve) PERCENT_IND_Nullable = Percent_Ind;

                StoredProcedures.MID_METHOD_GENERAL_ALLOCATION_INSERT.Insert(td.DBA,
                                                                             METHOD_RID: method_RID,
                                                                             BEGIN_CDR_RID: Begin_CDR_RID,
                                                                             SHIP_TO_CDR_RID: Ship_To_CDR_RID,
                                                                             MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                             MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                             MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                             GEN_ALLOC_HDR_RID: GEN_ALLOC_HDR_RID_Nullable,
                                                                             RESERVE: RESERVE_Nullable,
                                                                             PERCENT_IND: PERCENT_IND_Nullable,
                                                                             MERCH_TYPE: (int)MerchandiseType,
                                                                             RESERVE_AS_BULK: ReserveAsBulk,
                                                                             RESERVE_AS_PACKS: ReserveAsPacks
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
			bool UpdateSuccessful = true;
			try
			{
              
                int? MERCH_HN_RID_Nullable = null;
                if (Merch_HN_RID != Include.NoRID) MERCH_HN_RID_Nullable = Merch_HN_RID;
                
                int? MERCH_PH_RID_Nullable = null;
                if (Merch_PH_RID != Include.NoRID) MERCH_PH_RID_Nullable = Merch_PH_RID;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (Merch_PH_RID != Include.NoRID) MERCH_PHL_SEQUENCE_Nullable = _Merch_PHL_SEQ;

                int? GEN_ALLOC_HDR_RID_Nullable = null;
                if (Gen_Alloc_HDR_RID != Include.NoRID) GEN_ALLOC_HDR_RID_Nullable = Gen_Alloc_HDR_RID;

                StoredProcedures.MID_METHOD_GENERAL_ALLOCATION_UPDATE.Update(td.DBA,
                                                                                METHOD_RID: method_RID,
                                                                                BEGIN_CDR_RID: _Begin_CDR_RID,
                                                                                SHIP_TO_CDR_RID: _Ship_To_CDR_RID,
                                                                                MERCH_HN_RID: MERCH_HN_RID_Nullable,
                                                                                MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                                MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                                GEN_ALLOC_HDR_RID: GEN_ALLOC_HDR_RID_Nullable,
                                                                                RESERVE: _Reserve,
                                                                                PERCENT_IND: _Percent_Ind,
                                                                                MERCH_TYPE: (int)MerchandiseType,
                                                                                RESERVE_AS_BULK: ReserveAsBulk,
                                                                                RESERVE_AS_PACKS: ReserveAsPacks
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
			try
			{
                StoredProcedures.MID_METHOD_GENERAL_ALLOCATION_DELETE.Delete(td.DBA, METHOD_RID: method_RID);
	
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
                return StoredProcedures.MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE.Read(_dba, MERCH_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
	}
}
