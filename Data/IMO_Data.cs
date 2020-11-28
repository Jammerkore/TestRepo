using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
    public partial class IMO_Data : DataLayer
    {
        private eMIDTextCode _IMO_UpdateMessageCode;
        private int[] _IMO_UpdateRowsAffected;
        private int _IMO_UpdateReturnCode;

        public IMO_Data()
            : base()
        {
            IMOInitial();
        }
        public IMO_Data(Header aHeader)
            : base(aHeader._dba)
        {
            IMOInitial();
        }
        public void IMOInitial()
        {
            _IMO_UpdateReturnCode = 0;
            _IMO_UpdateMessageCode = 0;
        }

        /// <summary>
        /// Gets status message text code from update of store IMO
        /// </summary>
        public eMIDTextCode UpdateIMOMessageCode
        {
            get { return this._IMO_UpdateMessageCode; }
        }
        /// <summary>
        /// Gets return codes from update of store IMO
        /// </summary>
        public int UpdateIMOReturnCode
        {
            get { return this._IMO_UpdateReturnCode; }
        }
        /// <summary>
        /// Gets number of rows affected by successful update of store IMO
        /// </summary>
        public int UpdateIMOAffectedRows
        {
            get
            {
                int j = 0;
                foreach (int i in _IMO_UpdateRowsAffected)
                {
                    j += i;
                }
                return j;
            }
        }

        public bool UpdateIMO(IMO_UpdateRequest aIMOUpdateRequest)
        {
            aIMOUpdateRequest.Finish();
            _IMO_UpdateMessageCode = eMIDTextCode.msg_al_StoreIntransitTableRowsUpdatedSuccessfully;
            _IMO_UpdateRowsAffected = new int[aIMOUpdateRequest.IMOUpdateText.Count];
            _IMO_UpdateRowsAffected.Initialize();
            _IMO_UpdateReturnCode = 0;
            int i = 0;
            foreach (StringBuilder sb in aIMOUpdateRequest.IMOUpdateText)
            {
                //MIDDbParameter[] InParams = { new MIDDbParameter("@xml", sb, eDbType.Text, eParameterDirection.Input)
                //};
                //_IMO_UpdateReturnCode = _dba.UpdateStoredProcedure("SP_MID_UPDATE_IMO", InParams, null);
                //_dba.ExecuteStoredProcedureForUpdate("SP_MID_UPDATE_IMO", InParams, null);
                string s = sb.ToString();
                _IMO_UpdateReturnCode = StoredProcedures.SP_MID_UPDATE_IMO.UpdateWithReturnCode(_dba,
                                                                                  xml: sb.ToString(),
                                                                                  debug: 1
                                                                                  );
                if (_IMO_UpdateReturnCode < 0)
                {
                    _IMO_UpdateMessageCode = eMIDTextCode.msg_al_IMOUpdateStoredProcedureFailed;
                    return false;
                }
                _IMO_UpdateRowsAffected[i] = _IMO_UpdateReturnCode;
                i++;
            }
            return true;
        }

        public StoreIMO GetIMO(IMO_ReadRequest aIMOReadRequest, int[] aAllStoreRIDList)
        {
            //MIDDbParameter flashBackParm;
            DateTime flashBack = aIMOReadRequest.FlashBack;
            StoreIMO storeIMO = new StoreIMO(aAllStoreRIDList);

            // Begin TT#827-MD - JSmith - Allocation Reviews Performance
            //foreach (StringBuilder hnSB in aIMOReadRequest.IMOHnRIDTextList)
            foreach (DataTable hnSB in aIMOReadRequest.IMOHnRIDTextList)
            // End TT#827-MD - JSmith - Allocation Reviews Performance
            {
                //foreach (StringBuilder strSB in aIMOReadRequest.StoreIMOFromToList)
                //{
                //if (flashBack == Include.UndefinedDate)
                //{
                //    flashBackParm = new MIDDbParameter("@FlashBack", DBNull.Value, eDbType.DateTime, eParameterDirection.Output);
                //}
                //else
                //{
                //    flashBackParm = new MIDDbParameter("@FlashBack",flashBack, eDbType.DateTime, eParameterDirection.InputOutput);
                //}

                //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                ////MIDDbParameter[] InParams  = {   
                ////                              new MIDDbParameter("@HN_List", hnSB, eDbType.Text, eParameterDirection.Input)
                ////                          };
                //MIDDbParameter[] InParams = {   
                //                              new MIDDbParameter("@HN_List", hnSB, eDbType.Structured, eParameterDirection.Input)
                //                          };
                //// End TT#827-MD - JSmith - Allocation Reviews Performance
                //MIDDbParameter[] OutParams = {
                //                              flashBackParm,
                //                              new MIDDbParameter("@Return_Code", DBNull.Value, eDbType.Int, eParameterDirection.Output),
                //};
                DateTime? flashBackDefaultValue = null;
                if (flashBack != Include.UndefinedDate) flashBackDefaultValue = flashBack;


                //_dba.OpenReadConnection();
                try
                {
                    int returnValue = -1;
                    //_dba.ReadOnlyStoredProcedure("SP_MID_GET_IMO", InParams, OutParams);
                    DataTable dt = StoredProcedures.SP_MID_GET_IMO.ReadValues(_dba,
                                                          ref returnValue,
                                                          ref flashBack,
                                                          HN_List: hnSB,
                                                          flashBackDefaultValue: flashBackDefaultValue
                                                          );

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        storeIMO.SetStoreIMOValue(
                            Convert.ToInt32(dt.Rows[i]["ST_RID"]),
                            Convert.ToInt32(dt.Rows[i]["UNITS"]));
                    }

                    //int storeRID_Ordinal = (int)_dba.GetReadOrdinal("ST_RID");
                    //int units_Ordinal = (int)_dba.GetReadOrdinal("UNITS");
                    //do
                    //{
                    //    while (_dba.Read())
                    //    {
                    //        storeIMO.SetStoreIMOValue(
                    //            (int)_dba.ReadResultRow(storeRID_Ordinal),
                    //            (int)_dba.ReadResultRow(units_Ordinal));
                    //    }
                    //} while (_dba.NextResult());
                    //int returnValue = (int)_dba.ReadOutputParmValue("@Return_Code");
                    //flashBack = (DateTime)_dba.ReadOutputParmValue("@FlashBack");

                    //int returnValue = (int)StoredProcedures.SP_MID_GET_IMO.Return_Code.Value;

                    //flashBack = (DateTime)StoredProcedures.SP_MID_GET_IMO.FlashBack.Value;
                    storeIMO.FlashBack = flashBack;
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                finally
                {
                   // _dba.CloseReadConnection();
                }
                //	}
            }
            return storeIMO;
        }

        /// <summary>
        /// Executes the command to purge all IMO review that is less than the current date
        /// </summary>
        public int Purge_IMO_Review(int aCommitLimit)
        {
            try
            {

                //string SQLCommand = "DELETE top (" + aCommitLimit.ToString() + ") FROM STORE_IMO_REV with (rowlock) WHERE FLASHBACK < (select POSTING_DATE from POSTING_DATE)";
                //return _dba.ExecuteNonQuery(SQLCommand);
                return StoredProcedures.MID_STORE_IMO_REV_DELETE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public bool DeleteIMO(IMO_DeleteRequest aIMODeleteRequest)
        //{
        //    try
        //    {
        //        string SQLCommand = "delete from STORE_IMO where HN_RID in (" + aIMODeleteRequest.HnRIDListAsCommaDelimitedText + ")";
        //        return (_dba.ExecuteNonQuery(SQLCommand) >= 0);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
    }

	public class StoreIMO
	{
		// Fields
		private DateTime _flashBack;
		private int[] _siv;
		private int[] _allStoreRIDList;
		private Hashtable _storeIdxRID;

		// Constructor
		public StoreIMO(int[] aAllStoreRIDList)
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

		public StoreIMOValue[] StoreIMOValues
		{
			get
			{
				StoreIMOValue[] sivArray = new StoreIMOValue[_siv.Length];
				foreach (int storeRID in _storeIdxRID.Keys)
				{
					StoreIMOValue siv = new StoreIMOValue(storeRID, _siv[(int)_storeIdxRID[storeRID]]);
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

		public void SetStoreIMOValue(int aStoreRID, int aStoreIMOUnits)
		{
			if (this._storeIdxRID.Contains(aStoreRID))
			{
				_siv[(int)_storeIdxRID[aStoreRID]] += aStoreIMOUnits;
			}
		}
	}

	public struct StoreIMOValue
	{
		// Fields
		private int _storeRID;
		private int _storeIMO;

		// Constructors
		public StoreIMOValue(int aStoreRID, int aStoreIMO)
		{
			_storeRID = aStoreRID;
			_storeIMO = aStoreIMO;
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
		/// Gets IMO Value
		/// </summary>
		public int StoreIMO
		{
			get
			{
				return _storeIMO;
			}
		}
	}
	
	public class IMO_ReadRequest
	{
		//private int _maxStringLength; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		private DateTime _flashBack;
		private ArrayList _hnRIDTextList;
        // Begin TT#827-MD - JSmith - Allocation Reviews Performance
        private DataTable _HNTable = null;
        // End TT#827-MD - JSmith - Allocation Reviews Performance
		private ArrayList _storeIMOFromToList;
		//private int _currentHnRIDList; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		//private int _currentStoreIMODayList; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		//private int _requestCharIdx; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		//private int _lastFrom_yyyyddd;
		//private int _lastTo_yyyyddd;
		//private char[] _lastFromDateCharArray;
		//private char[] _lastToDateCharArray;

		//Constructor
		public IMO_ReadRequest()
		{
			//_maxStringLength = int.MaxValue - 400;   // allow room for parameter to grow slightly bigger //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			_hnRIDTextList = new ArrayList();
			_storeIMOFromToList = new ArrayList();
			//_currentHnRIDList = -1; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			//_currentStoreIMODayList = -1;
			_flashBack = Include.UndefinedDate;
			//_lastFrom_yyyyddd = 0;
			//_lastTo_yyyyddd = 0;
			//_lastFromDateCharArray = new char[7];
			//_lastFromDateCharArray.Initialize();
			//_lastToDateCharArray = new char[7];
			//_lastToDateCharArray.Initialize();
		}

		// Properties
		/// <summary>
		/// Gets FlashBack datetime where desired IMO resides.
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
		/// Gets the IMO HnRID List in Text format
		/// </summary>
		public ArrayList IMOHnRIDTextList
		{
			get
			{
				return this._hnRIDTextList;
			}
		}

		///// <summary>
		///// Gets the IMO Store From/to List in Text format
		///// </summary>
		//public ArrayList StoreIMOFromToList
		//{
		//    get
		//    {
		//        return this._storeIMOFromToList;
		//    }
		//}

		/// <summary>
		/// Adds an HnRID to the IMO HnRID List
		/// </summary>
		public int IMOHnRID
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
		/// Adds the members of an array of HnRIDs to the IMO HnRID list
		/// </summary>
		public int[] IMOHnRIDList
		{
			set
			{
				for (int i=0; i < value.Length; i++)
				{
					IMOHnRID = value[i];
				}
			}
		}

		//Methods

		///// <summary>
		///// Adds a store and its IMO from/to dates to the StoreFromTo IMO List.
		///// </summary>
		///// <param name="aStoreRID_CharArray">Character array of the RID of the store with leading zeros removed</param>
		///// <param name="aFrom_yyyyddd">From day in YYYYDDD format</param>
		///// <param name="aTo_yyyyddd">To day in YYYYDDD format</param>
		//public void SetStoreIMOFromTo(char[] aStoreRID_CharArray, int aFrom_yyyyddd, int aTo_yyyyddd)
		//{
		//    StringBuilder sb;
		//    if (this._storeIMOFromToList.Count == 0 
		//        || ((StringBuilder)this._storeIMOFromToList[_currentStoreIMODayList]).Length > this._maxStringLength)
		//    {
		//        sb = new StringBuilder();
		//        _currentStoreIMODayList++;
		//        _storeIMOFromToList.Add(sb);
		//    }
		//    sb = (StringBuilder)this._storeIMOFromToList[_currentStoreIMODayList];
		//    // begin MID track 4341 Performance Issues
		//    //char[] trimChar = "0".ToCharArray();
		//    //sb.Append(aStoreRID.ToString().TrimStart().TrimStart(trimChar));
		//    //sb.Append(",");
		//    //sb.Append(aFrom_yyyyddd.ToString().TrimStart(trimChar));
		//    //sb.Append(",");
		//    //sb.Append(aTo_yyyyddd.ToString().TrimStart(trimChar));
		//    //sb.Append(";");
		//    _requestCharIdx = 0;
		//    char[] requestCharArray = new char[17 + aStoreRID_CharArray.Length];
		//    aStoreRID_CharArray.CopyTo(requestCharArray,_requestCharIdx);
		//    _requestCharIdx += aStoreRID_CharArray.Length;
		//    requestCharArray[_requestCharIdx] = Include.CommaCharacter;
		//    _requestCharIdx++;
		//    string dateString;
		//    if (aFrom_yyyyddd != _lastFrom_yyyyddd)
		//    {
		//        _lastFrom_yyyyddd = aFrom_yyyyddd;
		//        dateString = aFrom_yyyyddd.ToString();
		//        dateString.CopyTo(dateString.Length - _lastFromDateCharArray.Length, _lastFromDateCharArray,0,_lastFromDateCharArray.Length);
		//    }
		//    _lastFromDateCharArray.CopyTo(requestCharArray,_requestCharIdx);
		//    _requestCharIdx += _lastFromDateCharArray.Length;
		//    requestCharArray[_requestCharIdx] = Include.CommaCharacter;
		//    _requestCharIdx++;
		//    if (aTo_yyyyddd != _lastTo_yyyyddd)
		//    {
		//        _lastTo_yyyyddd = aTo_yyyyddd;
		//        dateString = aTo_yyyyddd.ToString();
		//        dateString.CopyTo(dateString.Length - _lastToDateCharArray.Length, _lastToDateCharArray,0,_lastToDateCharArray.Length);
		//    }
		//    _lastToDateCharArray.CopyTo(requestCharArray,_requestCharIdx);
		//    _requestCharIdx += _lastToDateCharArray.Length;
		//    requestCharArray[_requestCharIdx] = Include.SemiColonCharacter;
		//    sb.Append(requestCharArray);
		//}
        
		///// <summary>
		///// Adds a list of stores and associated from/to dates to the StoreFromTo IMO List
		///// </summary>
		///// <param name="aStoreRID_CharArray_List">Array of store RIDs Character Arrays with leading zeros removed</param>
		///// <param name="aFrom_yyyyddd">Array of From days in YYYYDDD format (must be in 1-to-1 correspondence with the store RID array.</param>
		///// <param name="aTo_yyyyddd">Array of To days in YYYYDDD format (must be in 1-to-1 correspondence with the store RID array.</param>
		//public void SetStoreIMOFromTo(ArrayList aStoreRID_CharArray_List, int[] aFrom_yyyyddd, int[] aTo_yyyyddd)
		//{
		//    if (aStoreRID_CharArray_List.Count != aFrom_yyyyddd.Length 
		//        || aStoreRID_CharArray_List.Count != aTo_yyyyddd.Length) 
		//    {
		//        // throw exception
		//    }
		//    for(int i=0; i < aStoreRID_CharArray_List.Count; i++)  
		//    {
		//        SetStoreIMOFromTo((char[])aStoreRID_CharArray_List[i], aFrom_yyyyddd[i], aTo_yyyyddd[i]); 
		//    }
		//}

		///// <summary>
		///// Adds a list of stores and associated from/to dates to the StoreFromTo IMO List
		///// </summary>
		///// <param name="aStoreRID_CharArray_List">Array of store RIDs Character Arrays with leading zeros removed</param>
		///// <param name="aFromTo_yyyyddd_DateRange">A list of "longs" where bits 32-63 represent the from day and bits 0-31 represent the to-day.</param>
		//public void SetStoreIMOFromTo(ArrayList aStoreRID_CharArray_List, long[] aFromTo_yyyyddd_DateRange) 
		//{
		//    if (aStoreRID_CharArray_List.Count != aFromTo_yyyyddd_DateRange.Length)  // MID track 4341 Performance Issues
		//    {
		//        // throw exception
		//    }
		//    for (int i=0; i<aStoreRID_CharArray_List.Count; i++) // MID track 4341 Performance Issues
		//    {
		//        SetStoreIMOFromTo((char[])aStoreRID_CharArray_List[i], (int)(aFromTo_yyyyddd_DateRange[i] >> 32), (int)(aFromTo_yyyyddd_DateRange[i])); // MID Track 4341 Performance issues
		//    }
		//}
	}



	public class IMO_UpdateRequest
	{
		private int _maxStringLength;
		private char[] _trimChar;
		private int _styleRID;
		private int _colorHnRID;
		private int _sizeHnRID;
		private int _parentOfStyleRID;
		private int _currentArrayItem;
		private ArrayList _IMOUpdateText;
		private bool _xmlComplete;
		//Constructor
		public IMO_UpdateRequest()
		{
			_trimChar = "0".ToCharArray();
			_maxStringLength = 50000;   // reduce document size, more smaller documents work faster
			_IMOUpdateText = new ArrayList();
			_currentArrayItem = -1;
			_styleRID = Include.NoRID;
			_colorHnRID = Include.NoRID;
			_sizeHnRID = Include.NoRID;
			_parentOfStyleRID = Include.NoRID;
			_xmlComplete = false;
		}

		// Properties

		/// <summary>
		/// Gets the IMO Update Text ArrayList
		/// </summary>
		public ArrayList IMOUpdateText
		{
			get
			{
				return this._IMOUpdateText;
			}
		}

		/// <summary>
		/// Adds store IMO incremental update to ArrayList
		/// </summary>
		public void StoreIMO(int aStyleHnRID, int aParentOfStyleRID, int aColorHnRID, int aSizeHnRID, int[] aStoreRIDList, int[] aIncrementValue)
		{
			StartXML(aStyleHnRID, aParentOfStyleRID, aColorHnRID, aSizeHnRID);
			for (int i=0; i<aStoreRIDList.Length ; i++)
			{
				StoreIMO(aStoreRIDList[i], aIncrementValue[i]);
			}
		}
		public void StoreIMO(int aStyleHnRID, int aParentOfStyleRID, int aColorHnRID, int aSizeHnRID, int aStoreRID, int aIncrementValue)
		{
			StartXML(aStyleHnRID, aParentOfStyleRID, aColorHnRID, aSizeHnRID);
			StoreIMO(aStoreRID, aIncrementValue);
		}
		private void StoreIMO(int aStoreRID, int aIncrementValue)
		{
			if (this._xmlComplete)
			{
				throw new Exception("Attempt to update store IMO after IMO update complete");
			}
			StringBuilder sb = (StringBuilder)this._IMOUpdateText[this._currentArrayItem];
			if (sb.Length >= this._maxStringLength)
			{
				ReStartXML();
				sb = (StringBuilder)this._IMOUpdateText[this._currentArrayItem];
			}
			sb.Append("<Str S_RID=\"");
			sb.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
			//sb.Append("\" Dy=\"");
			//sb.Append(aIMOYYYYDDD.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
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
			if (this._IMOUpdateText.Count == 0
				|| ((StringBuilder)this._IMOUpdateText[this._currentArrayItem]).Length >= this._maxStringLength)
			{
				FinishXML();
				StringBuilder sb = new StringBuilder();
				sb.Append(Include.XML_Version);
	        	sb.Append(" <UpdateIMO> ");
				this._IMOUpdateText.Add(sb);
				this._currentArrayItem = this._IMOUpdateText.Count - 1;
			}
			if (aStyleHnRID != this._styleRID
				|| aParentOfStyleRID != this._parentOfStyleRID
				|| aColorHnRID != this._colorHnRID
				|| aSizeHnRID != this._sizeHnRID)
			{
				FinishIMORID();
				this._styleRID = aStyleHnRID;
				this._parentOfStyleRID = aParentOfStyleRID;
				this._colorHnRID = aColorHnRID;
				this._sizeHnRID = aSizeHnRID;
				StringBuilder sb = (StringBuilder)this._IMOUpdateText[this._currentArrayItem];
				sb.Append("<IMO Name=\"IMO\" ");
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
			if (this._IMOUpdateText.Count > 0 && _xmlComplete == false)
			{
				FinishIMORID();
				StringBuilder sb = (StringBuilder)this._IMOUpdateText[this._currentArrayItem];
				sb.Append(" </UpdateIMO> ");
			}
		}
		private void FinishIMORID()
		{
			if (this._styleRID != Include.NoRID
				|| this._parentOfStyleRID != Include.NoRID
				|| this._colorHnRID != Include.NoRID
				|| this._sizeHnRID != Include.NoRID)
			{
				StringBuilder sb = (StringBuilder)this._IMOUpdateText[this._currentArrayItem];
				sb.Append(" </IMO> ");
				this._styleRID = Include.NoRID;
				this._colorHnRID = Include.NoRID;
				this._parentOfStyleRID = Include.NoRID;
				this._sizeHnRID = Include.NoRID;
			}
		}
	}
	public class IMO_DeleteRequest
	{
		//int _maxStringLength; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
		private StringBuilder _sbDeleteIMOHnRIDs;
		//Constructor
		public IMO_DeleteRequest()
		{
			//_maxStringLength = 50000; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			_sbDeleteIMOHnRIDs = new StringBuilder();
		}

		// Properties

		/// <summary>
		/// Gets the HnRID list as a comma delimited text 
		/// </summary>
		public string HnRIDListAsCommaDelimitedText
		{
			get
			{
				return this._sbDeleteIMOHnRIDs.ToString();
			}
		}
		/// <summary>
		/// Clears the contents of the IMO Delete Request
		/// </summary>
		public void Clear()
		{
			_sbDeleteIMOHnRIDs = new StringBuilder();
		}

		/// <summary>
		/// Adds the specified Hierarchy Node RIDs to the Delete IMO Request
		/// </summary>
		/// <param name="aHnRID_ArrayList">ArrayList of Hierarchy Node RIDs whose IMO is to be deleted</param>
		public void AddHnRIDToDeleteIMOList(ArrayList aHnRID_ArrayList)
		{
			int[] hnRID_List = new int[aHnRID_ArrayList.Count];
			aHnRID_ArrayList.CopyTo(0,hnRID_List,0,aHnRID_ArrayList.Count);
			AddHnRIDToDeleteIMOList(hnRID_List);
		}
		/// <summary>
		/// Adds the specified Hierarchy Node RIDs to the Delete IMO Request
		/// </summary>
		/// <param name="aHnRID_List">Array of Hierarchy Node RIDs whose IMO is to be deleted</param>
		public void AddHnRIDToDeleteIMOList(int[] aHnRID_List)
		{
			foreach (int hnRID in aHnRID_List)
			{
				AddHnRIDToDeleteIMOList(hnRID);
			}
		}
		/// <summary>
		/// Adds the Hierarchy Node RIDs of the specified Hierarchy Node Profiles to the Delete IMO Request
		/// </summary>
		/// <param name="aHierarchyNodeList">Hierarchy Node Profile List of Hierarchy Node Profiles whose IMO is to be deleted</param>
		public void AddHnRIDToDeleteIMOList(HierarchyNodeList aHierarchyNodeList)
		{
			foreach (HierarchyNodeProfile hnp in aHierarchyNodeList)
			{
				AddHnRIDToDeleteIMOList(hnp.Key);
			}
		}
		/// <summary>
		/// Adds the Hierarchy Node RID of the specified Hierarchy Node Profile to the Delete IMO Request
		/// </summary>
		/// <param name="aHierarchyNodeProfile">Hierarchy Node Profile whose IMO is to be deleted</param>
		public void AddHnRIDToDeleteIMOList(HierarchyNodeProfile aHierarchyNodeProfile)
		{
			AddHnRIDToDeleteIMOList(aHierarchyNodeProfile.Key);
		}
		/// <summary>
		/// Adds the Hierarchy Node RID to the Delete IMO Request
		/// </summary>
		/// <param name="aHnRID">Hierarchy Node RID whose IMO is to be deleted</param>
		public void AddHnRIDToDeleteIMOList(int aHnRID)
		{
			if (_sbDeleteIMOHnRIDs.Length > 0)
			{
				_sbDeleteIMOHnRIDs.Append("," + aHnRID.ToString());
			}
			else
			{
				_sbDeleteIMOHnRIDs.Append(aHnRID.ToString());
			}
		}
	}
}


