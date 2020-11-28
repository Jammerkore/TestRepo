using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for Intransit.
	/// </summary>
	public partial class Intransit : DataLayer
	{
//		private int _numberOfStoreDataTables = 1;
		// begin MID Track 5694 MA Enhancement Relieve IT by Hdr ID
		private eMIDTextCode _itUpdateMessageCode;
		private int[]    _itUpdateRowsAffected;
		private int      _itUpdateReturnCode;
		// end   MID Track 5694 MA Enhancement Relieve IT by Hdr ID

		public Intransit()
            : base(false) // TT#1185 - Verify ENQ before Update 
		{
			IntransitInitial();
		}
		//public Intransit(Header aHeader):base(aHeader._dba) // TT#1185 - Verify ENQ before Update
        public Intransit(Header aHeader)        // TT#1185 - Verify ENQ before Update
            : base(aHeader.DBA, false)          // TT#1185 - Verify ENQ before Update
		{
			IntransitInitial();
		}
		public void IntransitInitial()
		{
//			GlobalOptions opts = new GlobalOptions();
//			DataTable dt = opts.GetGlobalOptions();
//			DataRow dr = dt.Rows[0];
//			this._numberOfStoreDataTables = (dr["STORE_TABLE_COUNT"] == DBNull.Value) ? 
//				1 : Convert.ToInt32(dr["STORE_TABLE_COUNT"], CultureInfo.CurrentUICulture);
			// begin MID Track 5694 MA Enhancement Relieve IT by Hdr ID
			_itUpdateReturnCode = 0;
			_itUpdateMessageCode = 0;
			// end   MID Track 5694 MA Enhancement Relieve IT by Hdr ID
		}


//		/// <summary>
//		/// Updates intransit by "adding" the supplied increment to the existing intransit value.
//		/// </summary>
//		/// <param name="hnRID">Hierarchy Node RID whose intransit is to be updated</param>
//		/// <param name="timeId">Time Id to update (yyyyddd)</param>
//		/// <param name="storeRID">Store RID whose intransit is to be updated</param>
//		/// <param name="increment">Positive or Negative incremental update</param>
//		/// <returns>True: id update is successful</returns>
//		public bool Update(int hnRID, int timeId, int storeRID, int increment)
//		{
//			try
//			{
//				//				string SQL_UpdateCommand = "update STORE_INTRANSIT set UNITS = mrsadmin.SF_MID_GREATEST(0, UNITS+" + increment.ToString(CultureInfo.CurrentUICulture) +
//				//					") where HN_RID = " + hnRID.ToString(CultureInfo.CurrentUICulture) + " and TIME_ID = " + timeId.ToString(CultureInfo.CurrentUICulture) + " and ST_RID = " + storeRID.ToString(CultureInfo.CurrentUICulture);
//				//				string SQL_InsertCommand = "insert into STORE_INTRANSIT (HN_RID, ST_RID, TIME_ID, UNITS) VALUES (" + 
//				//					hnRID.ToString(CultureInfo.CurrentUICulture) + ", " +
//				//					storeRID.ToString(CultureInfo.CurrentUICulture) + ", " +
//				//					timeId.ToString(CultureInfo.CurrentUICulture) + ", " + 
//				//					"mrsadmin.SF_MID_GREATEST(0, " + increment.ToString(CultureInfo.CurrentUICulture) + "))";
//				MIDDbParameter[] InParams  = {   new MIDDbParameter("@HN_RID", hnRID, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@TIME_ID", timeId, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@VALUE", increment, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@INCREMENT","Y",eDbType.Char, eParameterDirection.Input)
//										  };
//						
//				_dba.ExecuteStoredProcedure("SP_MID_UPDATE_INTRANSIT", InParams);
//			}
//			catch (Exception e)
//			{
//				throw;
//			}
//			finally
//			{
//			}
//			return true;			
//		}

//		/// <summary>
//		/// Updates intransit by "replacing" the existing intransit value by the supplied value.
//		/// </summary>
//		/// <param name="hnRID">Hierarchy Node RID whose intransit is to be updated</param>
//		/// <param name="timeId">Time Id to update (yyyyddd)</param>
//		/// <param name="storeRID">Store RID whose intransit is to be updated</param>
//		/// <param name="units">NonNegative intransit value (negative values result in 0 intransit)</param>
//		/// <returns>True: update is successful</returns>
//		public bool Set(int hnRID, int timeId, int storeRID, int units)
//		{
//			try
//			{
//				//				string SQL_UpdateCommand = "update STORE_INTRANSIT set UNITS = mrsadmin.SF_MID_GREATEST(0, +" + units.ToString(CultureInfo.CurrentUICulture) +
//				//					") where HN_RID = " + hnRID.ToString(CultureInfo.CurrentUICulture) + " and TIME_ID = " + timeId.ToString(CultureInfo.CurrentUICulture) + " and ST_RID = " + storeRID.ToString(CultureInfo.CurrentUICulture);
//				//				string SQL_InsertCommand = "insert into STORE_INTRANSIT (HN_RID, ST_RID, TIME_ID, UNITS) VALUES (" + 
//				//					hnRID.ToString(CultureInfo.CurrentUICulture) + "," +
//				//					storeRID.ToString(CultureInfo.CurrentUICulture) + "," +
//				//					timeId.ToString(CultureInfo.CurrentUICulture) + "," + 
//				//					"mrsadmin.SF_MID_GREATEST(0, " + units.ToString(CultureInfo.CurrentUICulture) + ")";
//				MIDDbParameter[] InParams  = {   new MIDDbParameter("@HN_RID", hnRID, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@TIME_ID", timeId, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@VALUE", units, eDbType.Int, eParameterDirection.Input),
//											  new MIDDbParameter("@INCREMENT","N",eDbType.Char, eParameterDirection.Input)
//										  };
//						
//				_dba.ExecuteStoredProcedure("SP_MID_UPDATE_INTRANSIT", InParams);
//			}
//			catch (Exception e)
//			{
//				throw;
//			}
//			finally
//			{
//			}
//			return true;			
//		}

		/// <summary>
		/// Updates external intransit by "replacing" the existing intransit value by the supplied value.
		/// </summary>
		/// <param name="hnRID">Hierarchy Node RID whose intransit is to be updated</param>
		/// <param name="timeId">Time Id to update (yyyyddd)</param>
		/// <param name="storeRID">Store RID whose intransit is to be updated</param>
		/// <param name="units">NonNegative intransit value (negative values result in 0 intransit)</param>
		/// <returns>True: update is successful</returns>
		public bool SetExternal(int hnRID, int timeId, int storeRID, int units)
		{
			try
			{
                //MIDDbParameter[] InParams  = {   new MIDDbParameter("@HN_RID", hnRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@TIME_ID", timeId, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@VALUE", units, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@INCREMENT","N",eDbType.Char, eParameterDirection.Input)
                //                          };
						
                //_dba.ExecuteStoredProcedure("SP_MID_UPDATE_EXT_INTRANSIT", InParams);
                StoredProcedures.SP_MID_UPDATE_EXT_INTRANSIT.Insert(_dba,
                                                                    HN_RID: hnRID,
                                                                    TIME_ID: timeId,
                                                                    ST_RID: storeRID,
                                                                    VALUE: units,
                                                                    INCREMENT: 'N'
                                                                    );
			}
			catch (Exception e)
			{
				string message = e.ToString();
				throw;
			}
			finally
			{
			}
			return true;			
		}

//		/// <summary>
//		/// Roll intransit forward from Time ID Start to Time ID Finish
//		/// </summary>
//		/// <param name="timeIdStart"></param>
//		/// <param name="timeIdFinish"></param>
//		public void RollForward(int timeIdStart, int timeIdFinish)
//		{
//			MIDDbParameter[] InParams  = {   new MIDDbParameter("@TIME_ID_A", timeIdStart, eDbType.Int, eParameterDirection.Input),
//										  new MIDDbParameter("@TIME_ID_B", timeIdFinish, eDbType.Int, eParameterDirection.Input)};
//
//			_dba.ExecuteStoredProcedure("SP_MID_ROLL_INTRANSIT", InParams);
//		}



//		public DataTable GetItByTimeByNode(int timeId, int headerRID)
//		{
//			int tableNumber = headerRID%_numberOfStoreDataTables;
//			string SQLCommand = "select ST_RID, COALESCE(UNITS, 0) UNITS from STORE_INTRANSIT" + tableNumber.ToString(CultureInfo.CurrentUICulture) + " " +  
//				" where TIME_ID =" + timeId.ToString(CultureInfo.CurrentUICulture) + " and HN_RID =" + headerRID.ToString(CultureInfo.CurrentUICulture);
//			return _dba.ExecuteSQLQuery( SQLCommand, "SP_MID_GET_INTRANSIT" );
//		}

		// Begin MID Track 5694 MA Enhancement Relieve IT by Hdr ID
		/// <summary>
		/// Gets status message text code from update of internal store intransit
		/// </summary>
		public eMIDTextCode UpdateItMessageCode
		{
			get {return this._itUpdateMessageCode;}
		}
		/// <summary>
		/// Gets return codes from update of internal store intransit
		/// </summary>
		public int UpdateItReturnCode
		{
			get {return this._itUpdateReturnCode;}
		}
		/// <summary>
		/// Gets number of rows affected by successful update of store intransit
		/// </summary>
		public int UpdateItAffectedRows
		{
			get 
			{
				int j = 0;
				foreach (int i in _itUpdateRowsAffected)
				{
					j += i;
				}
				return j;
			}
		}
		// end MID Track 5694 MA Enhancement Relieve IT by Hdr ID 
		public bool UpdateIt(IntransitUpdateRequest aIntransitUpdateRequest)
		{
			aIntransitUpdateRequest.Finish();
			// begin MID Track 5694 MA Enhancement Relieve IT by Hdr ID
			_itUpdateMessageCode = eMIDTextCode.msg_al_StoreIntransitTableRowsUpdatedSuccessfully;
            _itUpdateRowsAffected = new int[aIntransitUpdateRequest.IntransitUpdateText.Count];
			_itUpdateRowsAffected.Initialize();
			_itUpdateReturnCode = 0;
			int i = 0;
			// end MID Track 5694 MA Enhancement Relieve IT by Hdr ID
			foreach (StringBuilder sb in aIntransitUpdateRequest.IntransitUpdateText)
			{
                //MIDDbParameter[] InParams = {
                //                             new MIDDbParameter("@xml", sb, eDbType.Text, eParameterDirection.Input)
                //                         };
                // begin MID Track 5694 MA Enhancement Relieve IT by Hdr ID
				//int returnCode = 0;
				//returnCode = _dba.UpdateStoredProcedure("SP_MID_UPDATE_INTRANSIT", InParams, null);
				//if (returnCode < 0)
				//{
				//	return false;
				//}
				//_itUpdateReturnCode = _dba.UpdateStoredProcedure("SP_MID_UPDATE_INTRANSIT", InParams, null);

                _itUpdateReturnCode = StoredProcedures.SP_MID_UPDATE_INTRANSIT.Update(_dba,
                                                                xml: sb.ToString(),
                                                                debug: 0);
                //_itUpdateReturnCode = (int)StoredProcedures.SP_MID_UPDATE_INTRANSIT.ReturnCode.Value;

				if (_itUpdateReturnCode < 0)
				{
					_itUpdateMessageCode = eMIDTextCode.msg_al_IntransitUpdateStoredProcedureFailed;
					return false;
				}
				_itUpdateRowsAffected[i] = _itUpdateReturnCode;
				i++; 
				// end MID Track 5694 MA Enhancement Relieve IT by Hdr ID
			}
			return true;
		}

		public StoreIntransit GetIt(IntransitReadRequest aIntransitReadRequest, int[] aAllStoreRIDList)
		{
			MIDDbParameter flashBackParm;
			DateTime flashBack = aIntransitReadRequest.FlashBack;
			StoreIntransit storeIntransit = new StoreIntransit(aAllStoreRIDList);

            // Begin TT#827-MD - JSmith - Allocation Reviews Performance
            //foreach (StringBuilder hnSB in aIntransitReadRequest.IntransitHnRIDTextList)
			foreach (DataTable hnSB in aIntransitReadRequest.IntransitHnRIDTextList)
            // End TT#827-MD - JSmith - Allocation Reviews Performance
			{
                // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                //foreach (StringBuilder strSB in aIntransitReadRequest.StoreIntransitFromToList)
                foreach (DataTable strSB in aIntransitReadRequest.StoreIntransitFromToList)
                // End TT#827-MD - JSmith - Allocation Reviews Performance
				{
                    //if (flashBack == Include.UndefinedDate)
                    //{
                    //    flashBackParm = new MIDDbParameter("@FlashBack", DBNull.Value, eDbType.DateTime, eParameterDirection.Output);
                    //}
                    //else
                    //{
                    //    flashBackParm = new MIDDbParameter("@FlashBack",flashBack, eDbType.DateTime, eParameterDirection.InputOutput);
                    //}

                    //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                    ////MIDDbParameter[] InParams = {   
                    ////                              new MIDDbParameter("@HN_List", hnSB, eDbType.Text, eParameterDirection.Input),
                    ////                              new MIDDbParameter("@ST_Day_List", strSB, eDbType.Text, eParameterDirection.Input)
                    ////                          };
                    //MIDDbParameter[] InParams  = {   
                    //                              new MIDDbParameter("@HN_List", hnSB, eDbType.Structured, eParameterDirection.Input),
                    //                              new MIDDbParameter("@ST_Day_List", strSB, eDbType.Structured, eParameterDirection.Input)
                    //                          };
                    //// End TT#827-MD - JSmith - Allocation Reviews Performance
                    //MIDDbParameter[] OutParams = {
                    //                              flashBackParm,
                    //                              new MIDDbParameter("@Return_Code", DBNull.Value, eDbType.Int, eParameterDirection.Output),
                    //};

                    //_dba.OpenReadConnection();
					try
					{
                        //_dba.ReadOnlyStoredProcedure("SP_MID_GET_INTRANSIT", InParams, OutParams);
                        int returnValue = -1;
                        DataTable dt = StoredProcedures.MID_GET_INTRANSIT.ReadValues(_dba,
                                                                               ref returnValue,
                                                                               ref flashBack,
                                                                               HN_List: hnSB,
                                                                               ST_Day_List: strSB,
                                                                               flashBackIn: flashBack
                                                                               );

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            storeIntransit.SetStoreIntransitValue(
                                Convert.ToInt32(dt.Rows[i]["ST_RID"]),
                                Convert.ToInt32(dt.Rows[i]["UNITS"]));
                        }

                        //int storeRID_Ordinal = (int)_dba.GetReadOrdinal("ST_RID");
                        //int units_Ordinal = (int)_dba.GetReadOrdinal("UNITS");
                        //do 
                        //{
                        //while (_dba.Read())
                        //{
                        //    storeIntransit.SetStoreIntransitValue(
                        //        (int)_dba.ReadResultRow(storeRID_Ordinal),
                        //        (int)_dba.ReadResultRow(units_Ordinal));
                        //}
                        //} while (_dba.NextResult());
                        //int returnValue = (int)_dba.ReadOutputParmValue("@Return_Code");
                        //int returnValue = (int)StoredProcedures.MID_GET_INTRANSIT.Return_Code.Value;
                        //flashBack = (DateTime)_dba.ReadOutputParmValue("@FlashBack");
                        //flashBack = (DateTime)StoredProcedures.MID_GET_INTRANSIT.FlashBack.Value;
						storeIntransit.FlashBack = flashBack;
					}
					catch (Exception error)
					{
						string message = error.ToString();
						throw;
					}
					finally
					{
						//_dba.CloseReadConnection();
					}
				}
			}
			return storeIntransit;
		}

		/// <summary>
		/// Executes the command to purge all external intransit that is less than the current date
		/// </summary>
		//Begin Track #4673 - JSmith - Correct database timeout
//		public int Purge_External_Intransit()
		public int Purge_External_Intransit(int aCommitLimit)
		//End Track #4673
		{
			try
			{
                ////Begin TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
                ////string SQLCommand = "DELETE FROM STORE_EXTERNAL_INTRANSIT WHERE TIME_ID < (select CURR_DATE_YYYYDDD from POSTING_DATE)";
                //string SQLCommand = "DELETE top (" + aCommitLimit.ToString() + ") FROM STORE_EXTERNAL_INTRANSIT with (rowlock) WHERE TIME_ID < (select CURR_DATE_YYYYDDD from POSTING_DATE)";
                ////End TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
                //return _dba.ExecuteNonQuery(SQLCommand);
                return StoredProcedures.MID_STORE_EXTERNAL_INTRANSIT_DELETE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//Begin TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
		/// <summary>
		/// Executes the command to purge all intransit review that is less than the current date
		/// </summary>
		public int Purge_Intransit_Review(int aCommitLimit)
		{
			try
			{
                //string SQLCommand = "DELETE top (" + aCommitLimit.ToString() + ") FROM STORE_INTRANSIT_REV with (rowlock) WHERE TIME_ID < (select CURR_DATE_YYYYDDD from POSTING_DATE)";
                //return _dba.ExecuteNonQuery(SQLCommand);
                return StoredProcedures.MID_STORE_INTRANSIT_REV_DELETE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
			}
			catch
			{
				throw;
			}
		}

		//End TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
// Begin Reclass - JSmith - Add method to return time IDs for nodes

		public DataTable External_Intransit_TimeIDs(int aNodeRID)
		{
			try
			{
                //string SQLCommand = "select distinct TIME_ID from STORE_EXTERNAL_INTRANSIT"
                //    + " where HN_RID = @HN_RID"
                //    + " order by TIME_ID";
                //MIDDbParameter[] inParams  = { new MIDDbParameter("@HN_RID", aNodeRID, eDbType.Int, eParameterDirection.Input ) };
                //return _dba.ExecuteSQLQuery( SQLCommand, "TimeIDs", inParams );
                return StoredProcedures.MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable Intransit_TimeIDs(int aNodeRID)
		{
			try
			{
                //string SQLCommand = "select distinct TIME_ID from STORE_INTRANSIT"
                //    + " where HN_RID = @HN_RID"
                //    + " order by TIME_ID";
                //MIDDbParameter[] inParams  = { new MIDDbParameter("@HN_RID", aNodeRID, eDbType.Int, eParameterDirection.Input ) };
                //return _dba.ExecuteSQLQuery( SQLCommand, "TimeIDs", inParams );
                return StoredProcedures.MID_STORE_INTRANSIT_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				throw;
			}
		}
// End Reclass

		// begin TT#1137 (MID Track 4351) Rebuild Intransit Utility
		public bool DeleteIntransit(IntransitDeleteRequest aIntransitDeleteRequest)
		{
			try
			{
                //// Begin TT#1875 - JSmith - Database error when sql command is too long
                ////string SQLCommand = "delete from STORE_INTRANSIT where HN_RID in (" + aIntransitDeleteRequest.HnRIDListAsCommaDelimitedText + ")";
                ////return (_dba.ExecuteNonQuery(SQLCommand) >= 0);
                //bool successful = true;
                //while (aIntransitDeleteRequest.MoreRequests)
                //{
                //    string SQLCommand = "delete from STORE_INTRANSIT where HN_RID in (" + aIntransitDeleteRequest.GetRequests() + ")";

                  

                //    if (_dba.ExecuteNonQuery(SQLCommand) > 0)
                //    {
                //        successful = false;
                //    }
                //}
                //return successful;
                //// End TT#1875
                int rowsDeleted = StoredProcedures.MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST.Delete(_dba, HN_RID_LIST: aIntransitDeleteRequest.GetRequestsAsDataTable());
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}
    
		// end TT#1137 (MID Track 4351) Rebuild Intransit Utility
	}

	public class StoreIntransit
	{
		// Fields
		private DateTime _flashBack;
		private int[] _siv;
		private int[] _allStoreRIDList;
		private Hashtable _storeIdxRID;

		// Constructor
		public StoreIntransit(int[] aAllStoreRIDList)
		{
			_flashBack = Include.UndefinedDate;
			_siv = new int[aAllStoreRIDList.Length];
			_allStoreRIDList = aAllStoreRIDList;
			_storeIdxRID = new Hashtable();
			for(int i=0; i< _allStoreRIDList.Length; i++)
			{
				_storeIdxRID.Add(_allStoreRIDList[i], i);
			}
		}

		// Properties
		public DateTime FlashBack
		{
			get
			{
				return _flashBack;
			}
			set
			{
				_flashBack = value;
			}
		}

		public StoreIntransitValue[] StoreIntransitValues
		{
			get
			{
				StoreIntransitValue[] sivArray = new StoreIntransitValue[_siv.Length];
				foreach (int storeRID in _storeIdxRID.Keys)
				{
					StoreIntransitValue siv = new StoreIntransitValue(storeRID, _siv[(int)_storeIdxRID[storeRID]]);
					sivArray[(int)_storeIdxRID[storeRID]] = siv;
				}
				return sivArray;
			}
		}

		public int StoreDimension
		{
			get 
			{
				return _siv.Length;
			}
		}

		public void SetStoreIntransitValue(int aStoreRID, int aStoreIntransitUnits)
		{
			if (this._storeIdxRID.Contains(aStoreRID))
			{
				_siv[(int)_storeIdxRID[aStoreRID]] += aStoreIntransitUnits;
			}
		}
	}

	public struct StoreIntransitValue
	{
		// Fields
		private int _storeRID;
		private int _storeIntransit;

		// Constructors
		public StoreIntransitValue(int aStoreRID, int aStoreIntransit)
		{
			_storeRID = aStoreRID;
			_storeIntransit = aStoreIntransit;
		}

		// Properties
		/// <summary>
		/// Gets StoreRID
		/// </summary>
		public int StoreRID
		{
			get
			{
				return _storeRID;
			}
		}

		/// <summary>
		/// Gets Intransit Value
		/// </summary>
		public int StoreIntransit
		{
			get
			{
				return _storeIntransit;
			}
		}
	}
	
	public class IntransitReadRequest
	{
		//private int _maxStringLength; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		private DateTime _flashBack;
		private ArrayList _hnRIDTextList;
        // Begin TT#827-MD - JSmith - Allocation Reviews Performance
        private DataTable _HNTable = null;
        private DataTable _STTable = null;
        // End TT#827-MD - JSmith - Allocation Reviews Performance
		private ArrayList _storeIntransitFromToList;
		//private int _currentHnRIDList; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		//private int _currentStoreIntransitDayList; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		// begin MID Track 4341 Performance Issues
		//private int _requestCharIdx; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		//private int _lastFrom_yyyyddd; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		//private int _lastTo_yyyyddd; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		private char[] _lastFromDateCharArray;
		private char[] _lastToDateCharArray;
		// end MID Track 4341 Performance Issues
		//		private StringBuilder _hnRIDIntransitList;
		//		private StringBuilder _storeIntransitFromToList;
		//Constructor
		public IntransitReadRequest()
		{
			//_maxStringLength = int.MaxValue - 400;   // allow room for parameter to grow slightly bigger
			_hnRIDTextList = new ArrayList();
			_storeIntransitFromToList = new ArrayList();
			//_currentHnRIDList = -1; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			//_currentStoreIntransitDayList = -1; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			//			_hnRIDIntransitList = new StringBuilder();
			//			_storeIntransitFromToList = new StringBuilder();
			_flashBack = Include.UndefinedDate;
			// begin MID track 4341 Performance Issues
			//_lastFrom_yyyyddd = 0; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			//_lastTo_yyyyddd = 0; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			_lastFromDateCharArray = new char[7];
			_lastFromDateCharArray.Initialize();
            _lastToDateCharArray = new char[7];
			_lastToDateCharArray.Initialize();
			// end MID track 4341 Performance Issues
		}

		// Properties
		/// <summary>
		/// Gets FlashBack datetime where desired intransit resides.
		/// </summary>
		public DateTime FlashBack
		{
			get
			{
				return _flashBack;
			}
			set
			{
				_flashBack = value;
			}
		}
		/// <summary>
		/// Gets the Intransit HnRID List in Text format
		/// </summary>
		//			public StringBuilder IntransitHnRIDTextList
		public ArrayList IntransitHnRIDTextList
		{
			get
			{
				return this._hnRIDTextList;
			}
		}

		/// <summary>
		/// Gets the Intransit Store From/to List in Text format
		/// </summary>
		//		public StringBuilder StoreIntransitFromToTextList
		public ArrayList StoreIntransitFromToList
		{
			get
			{
				return this._storeIntransitFromToList;
			}
		}

		/// <summary>
		/// Adds an HnRID to the Intransit HnRID List
		/// </summary>
		public int IntransitHnRID
		{
			set
			{
                // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                //StringBuilder sb;
                //if (this._hnRIDTextList.Count == 0 
                //    || ((StringBuilder)this._hnRIDTextList[_currentHnRIDList]).Length > this._maxStringLength)
                //{
                //    sb = new StringBuilder();
                //    _currentHnRIDList++;
                //    _hnRIDTextList.Add(sb);
                //}
                //sb = (StringBuilder)_hnRIDTextList[_currentHnRIDList];
                //sb.Append(value.ToString());
                //sb.Append(",");
                ////				_hnRIDIntransitList.Append(value.ToString());
                ////				_hnRIDIntransitList.Append(",");

                if (this._hnRIDTextList.Count == 0)
                {
                    _HNTable = new DataTable();
                    _HNTable.Columns.Add("HN_RID", typeof(int));
                    _HNTable.PrimaryKey = new DataColumn[] { _HNTable.Columns["HN_RID"] };  // TT#3595 - JSmith - Copy Store Forecasts Take A Long Time to Process
                    _hnRIDTextList.Add(_HNTable);
                }

                
                // Begin TT#3536 - JSmith - Duplicate nodes in alternate hierarchy cause error reading intransit
                DataRow[] drs = _HNTable.Select("HN_RID=" + value);
                if (drs.Length > 0)
                {
                    return;
                }
                // End TT#3536 - JSmith - Duplicate nodes in alternate hierarchy cause error reading intransit

                DataRow dr = _HNTable.NewRow();
                dr["HN_RID"] = value;
                _HNTable.Rows.Add(dr);
                // End TT#827-MD - JSmith - Allocation Reviews Performance
			}
		}

		/// <summary>
		/// Adds the members of an array of HnRIDs to the Intransit HnRID list
		/// </summary>
		public int[] IntransitHnRIDList
		{
			set
			{
				for (int i=0; i < value.Length; i++)
				{
					IntransitHnRID = value[i];
				}
			}
		}

		//Methods
		/// <summary>
		/// Adds a store and its intransit from/to dates to the StoreFromTo Intransit List.
		/// </summary>
		/// <param name="aStoreRID_CharArray">Character array of the RID of the store with leading zeros removed</param>
		/// <param name="aFrom_yyyyddd">From day in YYYYDDD format</param>
		/// <param name="aTo_yyyyddd">To day in YYYYDDD format</param>
		// begin MID Track 4341 Performance Issues
		//public void SetStoreIntransitFromTo(int aStoreRID, int aFrom_yyyyddd, int aTo_yyyyddd)
		public void SetStoreIntransitFromTo(char[] aStoreRID_CharArray, int aFrom_yyyyddd, int aTo_yyyyddd)
		{
            // Begin TT#827-MD - JSmith - Allocation Reviews Performance
            //StringBuilder sb;
            //if (this._storeIntransitFromToList.Count == 0 
            //    || ((StringBuilder)this._storeIntransitFromToList[_currentStoreIntransitDayList]).Length > this._maxStringLength)
            //{
            //    sb = new StringBuilder();
            //    _currentStoreIntransitDayList++;
            //    _storeIntransitFromToList.Add(sb);
            //}
            //sb = (StringBuilder)this._storeIntransitFromToList[_currentStoreIntransitDayList];
            //// begin MID track 4341 Performance Issues
            ////char[] trimChar = "0".ToCharArray();
            ////sb.Append(aStoreRID.ToString().TrimStart().TrimStart(trimChar));
            ////sb.Append(",");
            ////sb.Append(aFrom_yyyyddd.ToString().TrimStart(trimChar));
            ////sb.Append(",");
            ////sb.Append(aTo_yyyyddd.ToString().TrimStart(trimChar));
            ////sb.Append(";");
            //_requestCharIdx = 0;
            //char[] requestCharArray = new char[17 + aStoreRID_CharArray.Length];
            //aStoreRID_CharArray.CopyTo(requestCharArray,_requestCharIdx);
            //_requestCharIdx += aStoreRID_CharArray.Length;
            //requestCharArray[_requestCharIdx] = Include.CommaCharacter;
            //_requestCharIdx++;
            //string dateString;
            //if (aFrom_yyyyddd != _lastFrom_yyyyddd)
            //{
            //    _lastFrom_yyyyddd = aFrom_yyyyddd;
            //    dateString = aFrom_yyyyddd.ToString();
            //    dateString.CopyTo(dateString.Length - _lastFromDateCharArray.Length, _lastFromDateCharArray,0,_lastFromDateCharArray.Length);
            //}
            //_lastFromDateCharArray.CopyTo(requestCharArray,_requestCharIdx);
            //_requestCharIdx += _lastFromDateCharArray.Length;
            //requestCharArray[_requestCharIdx] = Include.CommaCharacter;
            //_requestCharIdx++;
            //if (aTo_yyyyddd != _lastTo_yyyyddd)
            //{
            //    _lastTo_yyyyddd = aTo_yyyyddd;
            //    dateString = aTo_yyyyddd.ToString();
            //    dateString.CopyTo(dateString.Length - _lastToDateCharArray.Length, _lastToDateCharArray,0,_lastToDateCharArray.Length);
            //}
            //_lastToDateCharArray.CopyTo(requestCharArray,_requestCharIdx);
            //_requestCharIdx += _lastToDateCharArray.Length;
            //requestCharArray[_requestCharIdx] = Include.SemiColonCharacter;
            //sb.Append(requestCharArray);
            ////sb.Append(aStoreRID.ToString().TrimStart(Include.StoreTrimChar));
            ////sb.Append(Include.CommaCharacterString);
            ////sb.Append(aFrom_yyyyddd.ToString().TrimStart(Include.DateTrimChar));
            ////sb.Append(Include.CommaCharacterString);
            ////sb.Append(aTo_yyyyddd.ToString().TrimStart(Include.DateTrimChar));
            ////sb.Append(Include.SemiColonCharacterString);
            //// removed unnecessary comments
            //// end MID track 4341 Performance Issues

            if (this._storeIntransitFromToList.Count == 0)
            {
                _STTable = new DataTable();
                _STTable.Columns.Add("ST_RID", typeof(int));
                _STTable.Columns.Add("START_DAY", typeof(int));
                _STTable.Columns.Add("END_DAY", typeof(int));
                _storeIntransitFromToList.Add(_STTable);
            }

            DataRow dr = _STTable.NewRow();
            dr["ST_RID"] = int.Parse(new string(aStoreRID_CharArray));
            dr["START_DAY"] = aFrom_yyyyddd;
            dr["END_DAY"] = aTo_yyyyddd;
            _STTable.Rows.Add(dr);
            // End TT#827-MD - JSmith - Allocation Reviews Performance
		}
        
		/// <summary>
		/// Adds a list of stores and associated from/to dates to the StoreFromTo Intransit List
		/// </summary>
		/// <param name="aStoreRID_CharArray_List">Array of store RIDs Character Arrays with leading zeros removed</param>
		/// <param name="aFrom_yyyyddd">Array of From days in YYYYDDD format (must be in 1-to-1 correspondence with the store RID array.</param>
		/// <param name="aTo_yyyyddd">Array of To days in YYYYDDD format (must be in 1-to-1 correspondence with the store RID array.</param>
		// begin MID Track 4341 Performance Issues
		// public void SetStoreIntransitFromTo(int[] aStoreRID, int[] aFrom_yyyyddd, int[] aTo_yyyyddd)
		public void SetStoreIntransitFromTo(ArrayList aStoreRID_CharArray_List, int[] aFrom_yyyyddd, int[] aTo_yyyyddd)
			// end MID Track 4341 Performance Issues
		{
			if (aStoreRID_CharArray_List.Count != aFrom_yyyyddd.Length // MID Track 4341 Performance Issues
				|| aStoreRID_CharArray_List.Count != aTo_yyyyddd.Length) // MID Track 4341 Performance Issues
			{
				// throw exception
			}
			for(int i=0; i < aStoreRID_CharArray_List.Count; i++)  // MID Track 4341 performance Issues
			{
				SetStoreIntransitFromTo((char[])aStoreRID_CharArray_List[i], aFrom_yyyyddd[i], aTo_yyyyddd[i]); // MID Track 4341 performance Issues
			}
		}

		/// <summary>
		/// Adds a list of stores and associated from/to dates to the StoreFromTo Intransit List
		/// </summary>
		/// <param name="aStoreRID_CharArray_List">Array of store RIDs Character Arrays with leading zeros removed</param>
		/// <param name="aFromTo_yyyyddd_DateRange">A list of "longs" where bits 32-63 represent the from day and bits 0-31 represent the to-day.</param>
		public void SetStoreIntransitFromTo(ArrayList aStoreRID_CharArray_List, long[] aFromTo_yyyyddd_DateRange) // MID Track 4341 Performance issues
		{
			if (aStoreRID_CharArray_List.Count != aFromTo_yyyyddd_DateRange.Length)  // MID track 4341 Performance Issues
			{
				// throw exception
			}
			for (int i=0; i<aStoreRID_CharArray_List.Count; i++) // MID track 4341 Performance Issues
			{
				SetStoreIntransitFromTo((char[])aStoreRID_CharArray_List[i], (int)(aFromTo_yyyyddd_DateRange[i] >> 32), (int)(aFromTo_yyyyddd_DateRange[i])); // MID Track 4341 Performance issues
			}
		}
	}



	public class IntransitUpdateRequest
	{
		private int _maxStringLength;
		private char[] _trimChar;
		private int _styleRID;
		private int _colorHnRID;
		private int _sizeHnRID;
		private int _parentOfStyleRID;
		private int _currentArrayItem;
		private ArrayList _intransitUpdateText;
		private bool _xmlComplete;
		//Constructor
		public IntransitUpdateRequest()
		{
			_trimChar = "0".ToCharArray();
//Begin Track #3984 - JSmith - Charge Intransit failing
//			_maxStringLength = 500000;   // allow room for parameter to grow slightly bigger
			_maxStringLength = 50000;   // reduce document size, more smaller documents work faster
//End Track #3984
			_intransitUpdateText = new ArrayList();
			_currentArrayItem = -1;
			_styleRID = Include.NoRID;
			_colorHnRID = Include.NoRID;
			_sizeHnRID = Include.NoRID;
			_parentOfStyleRID = Include.NoRID;
			_xmlComplete = false;
		}

		// Properties

		/// <summary>
		/// Gets the Intransit Update Text ArrayList
		/// </summary>
		public ArrayList IntransitUpdateText
		{
			get
			{
				return this._intransitUpdateText;
			}
		}

		/// <summary>
		/// Adds store intransit incremental update to ArrayList
		/// </summary>
		public void StoreIntransit(int aStyleHnRID, int aParentOfStyleRID, int aColorHnRID, int aSizeHnRID, int[] aStoreRIDList, int[] aIntransitYYYYDDD, int[] aIncrementValue)
		{
			StartXML(aStyleHnRID, aParentOfStyleRID, aColorHnRID, aSizeHnRID);
			for (int i=0; i<aStoreRIDList.Length ; i++)
			{
				StoreIntransit(aStoreRIDList[i], aIntransitYYYYDDD[i], aIncrementValue[i]);
			}
		}
		public void StoreIntransit(int aStyleHnRID, int aParentOfStyleRID, int aColorHnRID, int aSizeHnRID, int aStoreRID, int aIntransitYYYYDDD, int aIncrementValue)
		{
			StartXML(aStyleHnRID, aParentOfStyleRID, aColorHnRID, aSizeHnRID);
			StoreIntransit(aStoreRID, aIntransitYYYYDDD, aIncrementValue);
		}
		private void StoreIntransit(int aStoreRID, int aIntransitYYYYDDD, int aIncrementValue)
		{
			if (this._xmlComplete)
			{
				throw new Exception("Attempt to update store intransit after intransit update complete");
			}
			StringBuilder sb = (StringBuilder)this._intransitUpdateText[this._currentArrayItem];
			if (sb.Length >= this._maxStringLength)
			{
				ReStartXML();
				sb = (StringBuilder)this._intransitUpdateText[this._currentArrayItem];
			}
			sb.Append("<Str S_RID=\"");
			sb.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
			sb.Append("\" Dy=\"");
			sb.Append(aIntransitYYYYDDD.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
			sb.Append("\" Incr=\"");
			sb.Append(aIncrementValue.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
			sb.Append("\" /> ");			
		}

		private void ReStartXML()
		{
			StartXML(this._styleRID, this._parentOfStyleRID, this._colorHnRID, this._sizeHnRID);
		}
		private void StartXML(int aStyleHnRID, int aParentOfStyleRID, int aColorHnRID, int aSizeHnRID)
		{
			if (this._intransitUpdateText.Count == 0
				|| ((StringBuilder)this._intransitUpdateText[this._currentArrayItem]).Length >= this._maxStringLength)
			{
				FinishXML();
				StringBuilder sb = new StringBuilder();
				sb.Append(Include.XML_Version);
	        	sb.Append(" <UpdateIntransit> ");
				this._intransitUpdateText.Add(sb);
				this._currentArrayItem = this._intransitUpdateText.Count - 1;
			}
			if (aStyleHnRID != this._styleRID
				|| aParentOfStyleRID != this._parentOfStyleRID
				|| aColorHnRID != this._colorHnRID
				|| aSizeHnRID != this._sizeHnRID)
			{
				FinishIntransitRID();
				this._styleRID = aStyleHnRID;
				this._parentOfStyleRID = aParentOfStyleRID;
				this._colorHnRID = aColorHnRID;
				this._sizeHnRID = aSizeHnRID;
				StringBuilder sb = (StringBuilder)this._intransitUpdateText[this._currentArrayItem];
				sb.Append("<Intransit Name=\"IT\" ");
				if (this._styleRID != Include.NoRID)
				{
					sb.Append("Sty_RID=\"");
					sb.Append(this._styleRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
					sb.Append("\" ");
					if (this._parentOfStyleRID != Include.NoRID)
					{
						sb.Append("pSty_RID=\"");
						sb.Append(this._parentOfStyleRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
						sb.Append("\" ");
					}
				}
				if (this._colorHnRID != Include.NoRID)
				{
					sb.Append("C_RID=\"");
					sb.Append(this._colorHnRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
					sb.Append("\" ");
				}
				if (this._sizeHnRID != Include.NoRID)
				{
					sb.Append("Sz_RID=\"");
					sb.Append(this._sizeHnRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
					sb.Append("\" ");
				}
				sb.Append("> ");
			}
		}

		public void Finish()
		{
			FinishXML();
			_xmlComplete = true;
		}
		private void FinishXML()
		{
			if (this._intransitUpdateText.Count > 0 && _xmlComplete == false)
			{
				FinishIntransitRID();
				StringBuilder sb = (StringBuilder)this._intransitUpdateText[this._currentArrayItem];
				sb.Append(" </UpdateIntransit> ");
			}
		}
		private void FinishIntransitRID()
		{
			if (this._styleRID != Include.NoRID
				|| this._parentOfStyleRID != Include.NoRID
				|| this._colorHnRID != Include.NoRID
				|| this._sizeHnRID != Include.NoRID)
			{
				StringBuilder sb = (StringBuilder)this._intransitUpdateText[this._currentArrayItem];
				sb.Append(" </Intransit> ");
				this._styleRID = Include.NoRID;
				this._colorHnRID = Include.NoRID;
				this._parentOfStyleRID = Include.NoRID;
				this._sizeHnRID = Include.NoRID;
			}
		}
	}
	// begin TT#1137 (MID Track 4351) Rebuild Intransit Utility
	public class IntransitDeleteRequest
	{
		//int _maxStringLength;
		//private StringBuilder _sbDeleteIntransitHnRIDs;
        // Begin TT#1875 - JSmith - Database error when sql command is too long
        //private bool _moreRequests;
        //private int _index;
        // End TT#1875
		//Constructor
		public IntransitDeleteRequest()
		{
			//_maxStringLength = 50000;  
			//_sbDeleteIntransitHnRIDs = new StringBuilder();
            // Begin TT#1875 - JSmith - Database error when sql command is too long
            //_moreRequests = false;
            //_index = 0;
            // End TT#1875
		}

		// Properties

        // Begin TT#1875 - JSmith - Database error when sql command is too long
        ///// <summary>
        ///// Gets the HnRID list as a comma delimited text 
        ///// </summary>
        //public string HnRIDListAsCommaDelimitedText
        //{
        //    get
        //    {
        //        return this._sbDeleteIntransitHnRIDs.ToString();
        //    }
        //}

        /// <summary>
        /// Gets the length of the HnRID list as a comma delimited text 
        /// </summary>
        //public int HnRIDListAsCommaDelimitedTextLength
        //{
        //    get
        //    {
        //        return this._sbDeleteIntransitHnRIDs.Length;
        //    }
        //}

        public int GetCountOfRequests()
        {
            return _deleteIntransitHnRIDs.Count;
        }

        /// <summary>
        /// Gets the flag identifying if more requests are to be processed. 
        /// </summary>
        //public bool MoreRequests
        //{
        //    get
        //    {
        //        return this._moreRequests;
        //    }
        //}
        // End TT#1875

        /// <summary>
		/// Clears the contents of the Intransit Delete Request
		/// </summary>
		public void Clear()
		{
			//_sbDeleteIntransitHnRIDs = new StringBuilder();
            _deleteIntransitHnRIDs.Clear();
            // Begin TT#1875 - JSmith - Database error when sql command is too long
            //_moreRequests = false;
            //_index = 0;
            // End TT#1875
		}

        ///// <summary>
        ///// Adds the specified Hierarchy Node RIDs to the Delete Intransit Request
        ///// </summary>
        ///// <param name="aHnRID_ArrayList">ArrayList of Hierarchy Node RIDs whose Intransit is to be deleted</param>
        //public void AddHnRIDToDeleteIntransitList(ArrayList aHnRID_ArrayList)
        //{
        //    int[] hnRID_List = new int[aHnRID_ArrayList.Count];
        //    aHnRID_ArrayList.CopyTo(0,hnRID_List,0,aHnRID_ArrayList.Count);
        //    AddHnRIDToDeleteIntransitList(hnRID_List);
        //}
        ///// <summary>
        ///// Adds the specified Hierarchy Node RIDs to the Delete Intransit Request
        ///// </summary>
        ///// <param name="aHnRID_List">Array of Hierarchy Node RIDs whose intransit is to be deleted</param>
        //public void AddHnRIDToDeleteIntransitList(int[] aHnRID_List)
        //{
        //    foreach (int hnRID in aHnRID_List)
        //    {
        //        AddHnRIDToDeleteIntransitList(hnRID);
        //    }
        //}
		/// <summary>
		/// Adds the Hierarchy Node RIDs of the specified Hierarchy Node Profiles to the Delete Intransit Request
		/// </summary>
		/// <param name="aHierarchyNodeList">Hierarchy Node Profile List of Hierarchy Node Profiles whose intransit is to be deleted</param>
		public void AddHnRIDToDeleteIntransitList(HierarchyNodeList aHierarchyNodeList)
		{
			foreach (HierarchyNodeProfile hnp in aHierarchyNodeList)
			{
				AddHnRIDToDeleteIntransitList(hnp.Key);
			}
		}
		/// <summary>
		/// Adds the Hierarchy Node RID of the specified Hierarchy Node Profile to the Delete Intransit Request
		/// </summary>
		/// <param name="aHierarchyNodeProfile">Hierarchy Node Profile whose intransit is to be deleted</param>
		public void AddHnRIDToDeleteIntransitList(HierarchyNodeProfile aHierarchyNodeProfile)
		{
			AddHnRIDToDeleteIntransitList(aHierarchyNodeProfile.Key);
		}
        private System.Collections.Generic.List<int> _deleteIntransitHnRIDs = new System.Collections.Generic.List<int>();
		/// <summary>
		/// Adds the Hierarchy Node RID to the Delete Intransit Request
		/// </summary>
		/// <param name="aHnRID">Hierarchy Node RID whose intransit is to be deleted</param>
		public void AddHnRIDToDeleteIntransitList(int aHnRID)
		{
            //// Begin TT#1875 - JSmith - Database error when sql command is too long
            //_moreRequests = true;
            //// End TT#1875
            //if (_sbDeleteIntransitHnRIDs.Length > 0)
            //{
            //    _sbDeleteIntransitHnRIDs.Append("," + aHnRID.ToString());
            //}
            //else
            //{
            //    _sbDeleteIntransitHnRIDs.Append(aHnRID.ToString());
            //}
            _deleteIntransitHnRIDs.Add(aHnRID);
		}

        public DataTable GetRequestsAsDataTable()
        {
            DataTable dtNodeList = new DataTable();
            dtNodeList.Columns.Add("HN_RID", typeof(int));
            foreach (int hnRID in _deleteIntransitHnRIDs)
            {
                //ensure hnRIDs are distinct, and only added to the datatable one time
                if (dtNodeList.Select("HN_RID=" + hnRID.ToString()).Length == 0)
                {
                    DataRow dr = dtNodeList.NewRow();
                    dr["HN_RID"] = hnRID;
                    dtNodeList.Rows.Add(dr);
                }
            }
            return dtNodeList;
        }
        // Begin TT#1875 - JSmith - Database error when sql command is too long
        //public string GetRequests()
        //{
        //    int startIndex;
        //    int length = _maxStringLength;

        //    startIndex = _index;
        //    if (_index + _maxStringLength <= _sbDeleteIntransitHnRIDs.Length)
        //    {
        //        _index = _sbDeleteIntransitHnRIDs.ToString().IndexOf(",", _index + _maxStringLength) + 1;
        //    }

        //    // pick put end of request when max length is in middle of last entry
        //    if (startIndex > 0 &&
        //        _index == 0)
        //    {
        //        length = _sbDeleteIntransitHnRIDs.Length - startIndex;
        //        _moreRequests = false;
        //    }
        //    // get rest of requests
        //    else if (startIndex + _maxStringLength >= _sbDeleteIntransitHnRIDs.Length)
        //    {
        //        length = _sbDeleteIntransitHnRIDs.Length - _index;
        //        _moreRequests = false;
        //        _index = 0;
        //    }
        //    else
        //    {
        //        length = _index - startIndex - 1;
        //    }

        //    return _sbDeleteIntransitHnRIDs.ToString().Substring(startIndex, length);
        //}
        // End TT#1875
	}
	// end TT#1137 (MID Track 4351) Rebuild Intransit Utility
}


