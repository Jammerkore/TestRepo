using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace MIDRetail.DataCommon
{
    [Serializable()]
    public class StoreProfile : Profile, IComparable, ICloneable
    {
        private string _storeId;
        private string _storeName;
        private string _storeDescription;
        private bool _activeInd = false; // 1 byte //default to false
        private string _city;
        private string _state;
        private int _sellingSqFt;  // 4 bytes
        private DateTime _sellingOpenDt; // 8 bytes
        private DateTime _sellingCloseDt; // 8 bytes
        private DateTime _stockOpenDt; // 8 bytes
        private DateTime _stockCloseDt; // 8 bytes
        private int _leadTime; // 4 bytes
        private bool _shipOnMonday; // 1 byte
        private bool _shipOnTuesday; // 1 byte
        private bool _shipOnWednesday; // 1 byte
        private bool _shipOnThursday; // 1 byte
        private bool _shipOnFriday; // 1 byte
        private bool _shipOnSaturday; // 1 byte
        private bool _shipOnSunday; // 1 byte
        //private bool _dynamicStore = false;	 // 1 byte			// used in store explorer
        private string _text;
        private eStoreStatus _status; // 4 bytes
        private eStoreStatus _stockStatus; // 4 bytes
        private bool _similarStoreModel; // 1 byte
        private string _IMO_ID;
        private bool _deleteStore = false; // 1 byte // TT#739-MD - STodd - delete stores //default to false

        //public int GetSize()
        //{
        //    int totalBytes = 59; //start with 59
        //    if (_storeId != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_storeId).Length;
        //    if (_storeName != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_storeName).Length;
        //    if (_storeDescription != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_storeDescription).Length;
        //    if (_city != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_city).Length;
        //    if (_state != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_state).Length;
        //    if (_text != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_text).Length;
        //    if (_IMO_ID != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_IMO_ID).Length;
        //    return totalBytes;
        //}

        #region Properties
        public string StoreId
        {
            get { return _storeId; }
            //set { _storeId = value; }
        }
        public string StoreName
        {
            get { return _storeName; }
            //set { _storeName = value; }
        }
        public string StoreDescription
        {
            get { return _storeDescription; }
            //set { _storeDescription = value; }
        }

        public bool ActiveInd
        {
            get { return _activeInd; }
            //set { _activeInd = value; }
        }


        public string City
        {
            get { return _city; }
            //set { _city = value; }
        }

        public string State
        {
            get { return _state; }
            //set { _state = value; }
        }
        public int SellingSqFt
        {
            get { return _sellingSqFt; }
            //set { _sellingSqFt = value; }
        }
        public DateTime SellingOpenDt
        {
            get { return _sellingOpenDt; }
            //set { _sellingOpenDt = value; }
        }

        public void ResetSellingOpenDt()
        {
            _sellingOpenDt = Include.UndefinedDate;
        }


        public DateTime SellingCloseDt
        {
            get { return _sellingCloseDt; }
            //set { _sellingCloseDt = value; }
        }
        public void ResetSellingCloseDt()
        {
            _sellingCloseDt = Include.UndefinedDate;
        }



        public DateTime StockOpenDt
        {
            get { return _stockOpenDt; }
            //set { _stockOpenDt = value; }
        }
        public void ResetStockOpenDt()
        {
            _stockOpenDt = Include.UndefinedDate;
        }


        public DateTime StockCloseDt
        {
            get { return _stockCloseDt; }
            //set { _stockCloseDt = value; }
        }
        public void ResetStockCloseDt()
        {
            _stockCloseDt = Include.UndefinedDate;
        }


        public int LeadTime
        {
            get { return _leadTime; }
            //set { _leadTime = value; }
        }
        public bool ShipOnMonday
        {
            get { return _shipOnMonday; }
            //set { _shipOnMonday = value; }
        }
        public bool ShipOnTuesday
        {
            get { return _shipOnTuesday; }
            //set { _shipOnTuesday = value; }
        }
        public bool ShipOnWednesday
        {
            get { return _shipOnWednesday; }
            //set { _shipOnWednesday = value; }
        }
        public bool ShipOnThursday
        {
            get { return _shipOnThursday; }
            //set { _shipOnThursday = value; }
        }
        public bool ShipOnFriday
        {
            get { return _shipOnFriday; }
            //set { _shipOnFriday = value; }
        }
        public bool ShipOnSaturday
        {
            get { return _shipOnSaturday; }
            //set { _shipOnSaturday = value; }
        }
        public bool ShipOnSunday
        {
            get { return _shipOnSunday; }
            //set { _shipOnSunday = value; }
        }
        //public ArrayList Characteristics 
        //{
        //    get { return _characteristics ; }
        //    set { _characteristics = value; }
        //}
        //public bool DynamicStore
        //{
        //    get { return _dynamicStore; }
        //    set { _dynamicStore = value; }
        //}


        public string Text
        {
            get { return _text; }
            //set { _text = value; }
        }
        public void SetText(string value)
        {
            _text = value; 
        }


        public eStoreStatus Status
        {
            get { return _status; }
            //set { _status = value; }
        }

        public void SetStatus(eStoreStatus value)
        {
            _status = value;
        }


        public eStoreStatus StockStatus
        {
            get { return _stockStatus; }
            //set { _stockStatus = value; }
        }
        public void SetStockStatus(eStoreStatus value)
        {
            _stockStatus = value;
        }

 
        public bool SimilarStoreModel
        {
            get { return _similarStoreModel; }
            //set { _similarStoreModel = value; }
        }
  

        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        public string IMO_ID
        {
            get { return _IMO_ID; }
            //set { _IMO_ID = value; }
        }
        // END TT#1401 - stodd - add resevation stores
        // BEGIN TT#739-MD - STodd - delete stores
        public bool DeleteStore
        {
            get { return _deleteStore; }
            //set { _deleteStore = value; }
        }

        public void SetDeleteStore(bool value)
        {
            _deleteStore = value;
        }

        // END TT#739-MD - STodd - delete stores
        #endregion

        /// <summary>
        /// overrided Equals
        /// </summary>
        /// <param name="obj">StoreProfile</param>
        /// <returns>Bool</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || obj.GetType() == typeof(System.DBNull) || ((Profile)obj).ProfileType != ProfileType)
            {
                return Key == Include.NoRID;
            }
            else
            {
                return (Key == ((Profile)obj).Key);
            }
        }

        public override int GetHashCode()
        {
            try
            {
                return base.GetHashCode();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// override ToString
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.Text;
        }

        //public int IComparable.CompareTo(object obj)
        public int CompareTo(object obj)
        {
            return Key - ((StoreProfile)obj).Key;
        }

        public static bool operator <(StoreProfile lhs, StoreProfile rhs)
        {
            return ((IComparable)lhs).CompareTo(rhs) < 0;
        }

        public static bool operator <=(StoreProfile lhs, StoreProfile rhs)
        {
            return ((IComparable)lhs).CompareTo(rhs) <= 0;
        }

        public static bool operator >(StoreProfile lhs, StoreProfile rhs)
        {
            return ((IComparable)lhs).CompareTo(rhs) > 0;
        }

        public static bool operator >=(StoreProfile lhs, StoreProfile rhs)
        {
            return ((IComparable)lhs).CompareTo(rhs) >= 0;
        }

        /// <summary>
        /// returns a deep copy
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            StoreProfile sp = new StoreProfile(_key);

            sp._activeInd = this._activeInd;
            sp._city = this._city;
            sp._leadTime = this._leadTime;
            sp._shipOnFriday = this._shipOnFriday;
            sp._shipOnMonday = this._shipOnMonday;
            sp._shipOnSaturday = this._shipOnSaturday;
            sp._shipOnSunday = this._shipOnSunday;
            sp._shipOnThursday = this._shipOnThursday;
            sp._shipOnTuesday = this._shipOnTuesday;
            sp._shipOnWednesday = this._shipOnWednesday;
            sp._sellingCloseDt = this._sellingCloseDt;
            sp._sellingOpenDt = this._sellingOpenDt;
            sp._sellingSqFt = this._sellingSqFt;
            sp._state = this._state;
            sp._stockCloseDt = this._stockCloseDt;
            sp._stockOpenDt = this._stockOpenDt;
            sp._storeId = this._storeId;
            sp._storeName = this._storeName;
            sp._storeDescription = this._storeDescription;
            //sp._dynamicStore = this._dynamicStore;
            sp._text = this._text;
            sp._status = this._status;
            sp._stockStatus = this._stockStatus;
            sp._similarStoreModel = this._similarStoreModel; 	// Issue 3557 - stodd
            // BEGIN TT#1401 - stodd - add resevation stores
            sp._IMO_ID = this._IMO_ID;
            // END TT#1401 - stodd - add resevation stores
            sp._deleteStore = this._deleteStore;	// TT#739-MD - STodd - delete stores

            //if (_characteristics != null)
            //{
            //    // BEGIN Issue 4656 stodd 9.6.07
            //    sp._characteristics = new ArrayList();
            //    // END Issue 4656 stodd 9.6.07
            //    foreach (StoreCharGroupProfile scgp in this._characteristics)
            //    {
            //        StoreCharGroupProfile scgpClone = scgp.Clone();
            //        sp._characteristics.Add(scgpClone);
            //    }
            //}
            return sp;
        }


        public void LoadFieldsFromDataRow(System.Data.DataRow dr)
        {
            try
            {
                int key = -1;

                if (dr["ST_RID"] != DBNull.Value)
                    key = Convert.ToInt32(dr["ST_RID"]);

                base.Key = key;

                if (dr["ST_ID"] != DBNull.Value)
                {
                    this._storeId = (string)dr["ST_ID"];
                }

                if (dr["STORE_NAME"] != DBNull.Value)
                {
                    this._storeName = (string)dr["STORE_NAME"];
                }

                if (dr["STORE_DESC"] != DBNull.Value)
                {
                    this._storeDescription = (string)dr["STORE_DESC"];
                }

                if (dr["ACTIVE_IND"] != DBNull.Value)
                {
                    this._activeInd = ((string)dr["ACTIVE_IND"] == "1") ? true : false;
                }

                if (dr["CITY"] != DBNull.Value)
                {
                    this._city = (string)dr["CITY"];
                }

                if (dr["STATE"] != DBNull.Value)
                {
                    this._state = (string)dr["STATE"];
                }

                if (dr["SELLING_SQ_FT"] != DBNull.Value)
                {
                    this._sellingSqFt = Convert.ToInt32(dr["SELLING_SQ_FT"]);
                }

                if (dr["SELLING_OPEN_DATE"] != DBNull.Value)
                {
                    this._sellingOpenDt = (DateTime)dr["SELLING_OPEN_DATE"];
                }

                if (dr["SELLING_CLOSE_DATE"] != DBNull.Value)
                {
                    this._sellingCloseDt = (DateTime)dr["SELLING_CLOSE_DATE"];
                }

                if (dr["STOCK_OPEN_DATE"] != DBNull.Value)
                {
                    this._stockOpenDt = (DateTime)dr["STOCK_OPEN_DATE"];
                }

                if (dr["STOCK_CLOSE_DATE"] != DBNull.Value)
                {
                    this._stockCloseDt = (DateTime)dr["STOCK_CLOSE_DATE"];
                }

                if (dr["LEAD_TIME"] != DBNull.Value)
                {
                    this._leadTime = Convert.ToInt32(dr["LEAD_TIME"]);
                }

                if (dr["SHIP_ON_MONDAY"] != DBNull.Value)
                {
                    this._shipOnMonday = ((string)dr["SHIP_ON_MONDAY"] == "1") ? true : false;
                }

                if (dr["SHIP_ON_TUESDAY"] != DBNull.Value)
                {
                    this._shipOnTuesday = ((string)dr["SHIP_ON_TUESDAY"] == "1") ? true : false;
                }

                if (dr["SHIP_ON_WEDNESDAY"] != DBNull.Value)
                {
                    this._shipOnWednesday = ((string)dr["SHIP_ON_WEDNESDAY"] == "1") ? true : false;
                }

                if (dr["SHIP_ON_THURSDAY"] != DBNull.Value)
                {
                    this._shipOnThursday = ((string)dr["SHIP_ON_THURSDAY"] == "1") ? true : false;
                }

                if (dr["SHIP_ON_FRIDAY"] != DBNull.Value)
                {
                    this._shipOnFriday = ((string)dr["SHIP_ON_FRIDAY"] == "1") ? true : false;
                }

                if (dr["SHIP_ON_SATURDAY"] != DBNull.Value)
                {
                    this._shipOnSaturday = ((string)dr["SHIP_ON_SATURDAY"] == "1") ? true : false;
                }

                if (dr["SHIP_ON_SUNDAY"] != DBNull.Value)
                {
                    this._shipOnSunday = ((string)dr["SHIP_ON_SUNDAY"] == "1") ? true : false;
                }


                //this._dynamicStore = dynamicStore;

                if (dr["SIMILAR_STORE_MODEL"] != DBNull.Value)
                {
                    this._similarStoreModel = ((string)dr["SIMILAR_STORE_MODEL"] == "1") ? true : false;
                }


                if (dr["IMO_ID"] != DBNull.Value)
                {
                    this._IMO_ID = dr["IMO_ID"].ToString();
                }

                // BEGIN TT#739-MD - STodd - delete stores
                if (dr["STORE_DELETE_IND"] != DBNull.Value)
                    this._deleteStore = ((string)dr["STORE_DELETE_IND"] == "1") ? true : false;
                // END TT#739-MD - STodd - delete stores

                // Get statues
                //WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
                //sp.Status = StoreServerGlobal.GetStoreStatus(currentWeek, sp.SellingOpenDt, sp.SellingCloseDt);
                //sp.StockStatus = StoreServerGlobal.GetStoreStatus(currentWeek, sp.StockOpenDt, sp.StockCloseDt);

                //sp.Text = Include.GetStoreDisplay(_globalStoreDisplayOption, sp.StoreId, sp.StoreName, sp.StoreDescription);
                //_SAB.StoreServerSession.UpdateStatusAndTextOnStoreProfile(ref sp);



                //sp.Status = _SAB.StoreServerSession.GetStoreStatusForCurrentWeek(sp.SellingOpenDt, sp.SellingCloseDt);
                //sp.StockStatus = _SAB.StoreServerSession.GetStoreStatusForCurrentWeek(sp.StockOpenDt, sp.StockCloseDt);
                //sp.Text = Include.GetStoreDisplay(_gop.StoreDisplay, sp.StoreId, sp.StoreName, sp.StoreDescription);


                //sp.Characteristics = null;
                //sp.Characteristics = new ArrayList();

                //			int maxCols = _dtAllStores.Columns.Count;
                //			int startCol = StoreServerGlobal.NumberOfStoreProfileColumns;
                //			for (int col=startCol;col<maxCols;col++)
                //			{
                //				StoreCharGroupProfile ch = new StoreCharGroupProfile(-1);
                //				ch.Name = _dtAllStores.Columns[col].Caption;
                //				ch.Key = _characteristics.GetCharacteristicGroupRID(ch.Name);
                //				ch.CharacteristicValue.StoreCharType = _characteristics.GetCharacteristicDataType(ch.Key);
                //				if (Convert.ToString(dr[col], CultureInfo.CurrentCulture) == "<none>")
                //				{
                //					ch.CharacteristicValue.CharValue = System.DBNull.Value;
                //				}
                //				else
                //				{
                //					ch.CharacteristicValue.CharValue = dr[col];
                //				}
                //				ch.CharacteristicValue.SC_RID = _characteristics.CharacteristicExists(ch.Name, ch.CharacteristicValue.CharValue);
                //						
                //				sp.Characteristics.Add( ch );
                //			}

               // return this;
            }
            catch (Exception err)
            {
                //string msg = "ConvertToStoreProfile(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }

        // Begin TT#2095-MD - JSmith - Discrepancy in Store Status
        public void LoadDataRowFromFields(ref System.Data.DataRow dr)
        {
            try
            {
                dr["ACTIVE_IND"] = _activeInd ? "1" : "0";
                dr["ST_RID"] = Key;
                dr["ST_ID"] = _storeId;
                dr["STORE_NAME"] = _storeName;
                dr["STORE_DESC"] = _storeDescription;
                dr["CITY"] = _city;
                dr["STATE"] = _state;
                dr["SELLING_SQ_FT"] = _sellingSqFt;

                if (_sellingOpenDt == Include.UndefinedDate)
                {
                    dr["SELLING_OPEN_DATE"] = DBNull.Value;
                }
                else
                {
                    dr["SELLING_OPEN_DATE"] = _sellingOpenDt;
                }

                if (_sellingCloseDt == Include.UndefinedDate)
                {
                    dr["SELLING_CLOSE_DATE"] = DBNull.Value;
                }
                else
                {
                    dr["SELLING_CLOSE_DATE"] = _sellingCloseDt;
                }

                if (_stockOpenDt == Include.UndefinedDate)
                {
                    dr["STOCK_OPEN_DATE"] = DBNull.Value;
                }
                else
                {
                    dr["STOCK_OPEN_DATE"] = _stockOpenDt;
                }

                if (_stockCloseDt == Include.UndefinedDate)
                {
                    dr["STOCK_CLOSE_DATE"] = DBNull.Value;
                }
                else
                {
                    dr["STOCK_CLOSE_DATE"] = _stockCloseDt;
                }

                dr["Store Status"] = _status;
                dr["LEAD_TIME"] = _leadTime;
                dr["SHIP_ON_MONDAY"] = _shipOnMonday ? "1" : "0";
                dr["SHIP_ON_TUESDAY"] = _shipOnTuesday ? "1" : "0";
                dr["SHIP_ON_WEDNESDAY"] = _shipOnWednesday ? "1" : "0";
                dr["SHIP_ON_THURSDAY"] = _shipOnThursday ? "1" : "0";
                dr["SHIP_ON_FRIDAY"] = _shipOnFriday ? "1" : "0";
                dr["SHIP_ON_SATURDAY"] = _shipOnSaturday ? "1" : "0";
                dr["SHIP_ON_SUNDAY"] = _shipOnSunday ? "1" : "0";
                dr["SIMILAR_STORE_MODEL"] = _similarStoreModel ? "1" : "0";
                if (string.IsNullOrWhiteSpace(_IMO_ID))
                {
                    dr["IMO_ID"] = DBNull.Value;
                }
                else
                {
                    dr["IMO_ID"] = _IMO_ID;
                }
                dr["STORE_DELETE_IND"] = _deleteStore ? "1" : "0";
            }
            catch (Exception err)
            {
                throw;
            }
        }
        // End TT#2095-MD - JSmith - Discrepancy in Store Status

        public void SetFieldsOnProfile(
            bool profileActiveInd,
            bool? profileSimilarStoreModelInd,  // TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
            string profileCity = null,
            string profileState = null,
            int? profileLeadTime = null,
            DateTime? profileSellingCloseDt = null,
            DateTime? profileSellingOpenDt  = null,
            int? profileSellingSqFt = null,
            bool? profileShipOnFriday  = null,
            bool? profileShipOnMonday  = null,
            bool? profileShipOnSaturday  = null,
            bool? profileShipOnSunday  = null,
            bool? profileShipOnThursday  = null,
            bool? profileShipOnTuesday = null,
            bool? profileShipOnWednesday = null,
            DateTime? profileStockCloseDt = null,
            DateTime? profileStockOpenDt  = null,
            string profileStoreName = null,
            string profileStoreId = null,
            string profileStoreDescription = null,
            string profileIMO_ID = null
            )
    {
        this._activeInd = profileActiveInd;
        // Begin TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
        if (profileSimilarStoreModelInd != null)
        {
            this._similarStoreModel = (bool)profileSimilarStoreModelInd;
        }
        // End TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
        this._city = profileCity;
        this._state = profileState;
        this._storeId = profileStoreId;
        this._storeName = profileStoreName;
        this._storeDescription = profileStoreDescription;
        // Begin TT#1874-MD - JSmith - VSW ID is cleared when field not included in transaction
        //this._IMO_ID = profileIMO_ID;
        if (profileIMO_ID != null)
        {
            this._IMO_ID = profileIMO_ID;
        }
        // End TT#1874-MD - JSmith - VSW ID is cleared when field not included in transaction

        if (profileLeadTime != null)
        {
            this._leadTime = (int)profileLeadTime;
        }
        if (profileSellingSqFt != null)
        {
            this._sellingSqFt = (int)profileSellingSqFt;
        }

        if (profileShipOnFriday != null)
        {
            this._shipOnFriday = (bool)profileShipOnFriday;
            this._shipOnMonday = (bool)profileShipOnMonday;
            this._shipOnSaturday = (bool)profileShipOnSaturday;
            this._shipOnSunday = (bool)profileShipOnSunday;
            this._shipOnThursday = (bool)profileShipOnThursday;
            this._shipOnTuesday = (bool)profileShipOnTuesday;
            this._shipOnWednesday = (bool)profileShipOnWednesday;
        }

        if (profileSellingCloseDt != null)
        {
            this._sellingCloseDt = (DateTime)profileSellingCloseDt;
        }
        if (profileSellingOpenDt != null)
        {
            this._sellingOpenDt = (DateTime)profileSellingOpenDt;
        }


        if (profileStockCloseDt != null)
        {
            this._stockCloseDt = (DateTime)profileStockCloseDt;
        }
        if (profileStockOpenDt != null)
        {
            this._stockOpenDt = (DateTime)profileStockOpenDt;
        }

        
    }


        public void SetFieldsToUnknownStore()
        {
            this._storeId = "UNKNOWN";
            this._storeName = "UNKNOWN STORE";
            this._activeInd = false;
        }



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aKey">int</param>
        public StoreProfile(int aKey)
            : base(aKey)
        {
            //_characteristics = null;
        }

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.Store;
            }
        }

  

    }
}
