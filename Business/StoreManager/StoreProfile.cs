//using System;
//using System.IO;
//using System.Collections;
//using System.Data;
//using System.Globalization;
//using System.Runtime.InteropServices;

//using MIDRetail.Data;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//using System.Reflection.Emit;

//namespace MIDRetail.Business
//{
//    [Serializable()]
//    public class StoreProfile : Profile, IComparable , ICloneable
//    {
//        private string _storeId; 
//        private string _storeName; 
//        private string _storeDescription; 
//        private bool _activeInd; // 1 byte
//        private string _city; 
//        private string _state;
//        private int _sellingSqFt;  // 4 bytes
//        private DateTime _sellingOpenDt; // 8 bytes
//        private DateTime _sellingCloseDt; // 8 bytes
//        private DateTime _stockOpenDt; // 8 bytes
//        private DateTime _stockCloseDt; // 8 bytes
//        private int _leadTime; // 4 bytes
//        private bool _shipOnMonday; // 1 byte
//        private bool _shipOnTuesday; // 1 byte
//        private bool _shipOnWednesday; // 1 byte
//        private bool _shipOnThursday; // 1 byte
//        private bool _shipOnFriday; // 1 byte
//        private bool _shipOnSaturday; // 1 byte
//        private bool _shipOnSunday; // 1 byte
//        private bool _dynamicStore;	 // 1 byte			// used in store explorer
//        private string _text;
//        private eStoreStatus _status; // 4 bytes
//        private eStoreStatus _stockStatus; // 4 bytes
//        private bool _similarStoreModel; // 1 byte
//        private string _IMO_ID;
//        private bool _deleteStore;// 1 byte // TT#739-MD - STodd - delete stores

//        //public int GetSize()
//        //{
//        //    int totalBytes = 59; //start with 59
//        //    if (_storeId != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_storeId).Length;
//        //    if (_storeName != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_storeName).Length;
//        //    if (_storeDescription != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_storeDescription).Length;
//        //    if (_city != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_city).Length;
//        //    if (_state != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_state).Length;
//        //    if (_text != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_text).Length;
//        //    if (_IMO_ID != null) totalBytes += System.Text.Encoding.ASCII.GetBytes(_IMO_ID).Length;
//        //    return totalBytes;
//        //}

//        #region Properties
//        public string StoreId 
//        {
//            get { return _storeId ; }
//            set { _storeId = value; }
//        }
//        public string StoreName 
//        {
//            get { return _storeName ; }
//            set { _storeName = value; }
//        }
//        public string StoreDescription 
//        {
//            get { return _storeDescription ; }
//            set { _storeDescription = value; }
//        }
//        public bool ActiveInd 
//        {
//            get { return _activeInd ; }
//            set { _activeInd = value; }
//        }
//        public string City 
//        {
//            get { return _city ; }
//            set { _city = value; }
//        }
//        public string State 
//        {
//            get { return _state ; }
//            set { _state = value; }
//        }
//        public int SellingSqFt 
//        {
//            get { return _sellingSqFt ; }
//            set { _sellingSqFt = value; }
//        }
//        public DateTime SellingOpenDt 
//        {
//            get { return _sellingOpenDt ; }
//            set { _sellingOpenDt = value; }
//        }
//        public DateTime SellingCloseDt 
//        {
//            get { return _sellingCloseDt ; }
//            set { _sellingCloseDt = value; }
//        }
//        public DateTime StockOpenDt 
//        {
//            get { return _stockOpenDt ; }
//            set { _stockOpenDt = value; }
//        }
//        public DateTime StockCloseDt 
//        {
//            get { return _stockCloseDt ; }
//            set { _stockCloseDt = value; }
//        }
//        public int LeadTime 
//        {
//            get { return _leadTime ; }
//            set { _leadTime = value; }
//        }
//        public bool ShipOnMonday 
//        {
//            get { return _shipOnMonday ; }
//            set { _shipOnMonday = value; }
//        }
//        public bool ShipOnTuesday 
//        {
//            get { return _shipOnTuesday ; }
//            set { _shipOnTuesday = value; }
//        }
//        public bool ShipOnWednesday 
//        {
//            get { return _shipOnWednesday ; }
//            set { _shipOnWednesday = value; }
//        }
//        public bool ShipOnThursday 
//        {
//            get { return _shipOnThursday ; }
//            set { _shipOnThursday = value; }
//        }
//        public bool ShipOnFriday 
//        {
//            get { return _shipOnFriday ; }
//            set { _shipOnFriday = value; }
//        }
//        public bool ShipOnSaturday 
//        {
//            get { return _shipOnSaturday ; }
//            set { _shipOnSaturday = value; }
//        }
//        public bool ShipOnSunday 
//        {
//            get { return _shipOnSunday ; }
//            set { _shipOnSunday = value; }
//        }
//        //public ArrayList Characteristics 
//        //{
//        //    get { return _characteristics ; }
//        //    set { _characteristics = value; }
//        //}
//        public bool DynamicStore 
//        {
//            get { return _dynamicStore ; }
//            set { _dynamicStore = value; }
//        }
//        public string Text 
//        {
//            get { return _text ; }
//            set { _text = value; }
//        }
//        public eStoreStatus Status 
//        {
//            get { return _status ; }
//            set { _status = value; }
//        }
//        public eStoreStatus StockStatus 
//        {
//            get { return _stockStatus ; }
//            set { _stockStatus = value; }
//        }
//        // Begin Issue 3557 - stodd
//        public bool SimilarStoreModel 
//        {
//            get { return _similarStoreModel ; }
//            set { _similarStoreModel = value; }
//        }
//        // End Issue 3557 - stodd

//        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//        public string IMO_ID
//        {
//            get { return _IMO_ID; }
//            set { _IMO_ID = value; }
//        }
//        // END TT#1401 - stodd - add resevation stores
//        // BEGIN TT#739-MD - STodd - delete stores
//        public bool DeleteStore
//        {
//            get { return _deleteStore; }
//            set { _deleteStore = value; }
//        }
//        // END TT#739-MD - STodd - delete stores
//        #endregion

//        /// <summary>
//        /// overrided Equals
//        /// </summary>
//        /// <param name="obj">StoreProfile</param>
//        /// <returns>Bool</returns>
//        public override bool Equals(Object obj) 
//        {
//            if (obj == null || obj.GetType() == typeof(System.DBNull) || ((Profile)obj).ProfileType != ProfileType)
//            {
//                return Key == Include.NoRID;
//            }
//            else
//            {
//                return (Key == ((Profile)obj).Key);
//            }
//        }

//        public override int GetHashCode() 
//        {
//            try
//            {
//                return base.GetHashCode();
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        /// <summary>
//        /// override ToString
//        /// </summary>
//        /// <returns>string</returns>
//        public override string ToString()
//        {
//            return this.Text;
//        }

//        //public int IComparable.CompareTo(object obj)
//        public int CompareTo(object obj)
//        { 
//            return Key - ((StoreProfile)obj).Key; 
//        } 
	 
//        public static bool operator<(StoreProfile lhs, StoreProfile rhs)
//        { 
//            return ((IComparable)lhs).CompareTo(rhs) < 0;
//        }

//        public static bool operator<=(StoreProfile lhs, StoreProfile rhs)
//        { 
//            return ((IComparable)lhs).CompareTo(rhs) <= 0;
//        }

//        public static bool operator>(StoreProfile lhs, StoreProfile rhs)
//        { 
//            return ((IComparable)lhs).CompareTo(rhs) > 0;
//        }

//        public static bool operator>=(StoreProfile lhs, StoreProfile rhs)
//        { 
//            return ((IComparable)lhs).CompareTo(rhs) >= 0;
//        }

//        /// <summary>
//        /// returns a deep copy
//        /// </summary>
//        /// <returns></returns>
//        public object Clone()
//        {
//            StoreProfile sp = new StoreProfile(_key);
			
//            sp._activeInd = this._activeInd;
//            sp._city = this._city;
//            sp._leadTime = this._leadTime;
//            sp._shipOnFriday = this._shipOnFriday;
//            sp._shipOnMonday = this._shipOnMonday;
//            sp._shipOnSaturday = this._shipOnSaturday;
//            sp._shipOnSunday = this._shipOnSunday;
//            sp._shipOnThursday = this._shipOnThursday;
//            sp._shipOnTuesday = this._shipOnTuesday;
//            sp._shipOnWednesday = this._shipOnWednesday;
//            sp._sellingCloseDt = this._sellingCloseDt;
//            sp._sellingOpenDt = this._sellingOpenDt;
//            sp._sellingSqFt = this._sellingSqFt;
//            sp._state = this._state;
//            sp._stockCloseDt = this._stockCloseDt;
//            sp._stockOpenDt = this._stockOpenDt;
//            sp._storeId = this._storeId;
//            sp._storeName = this._storeName;
//            sp._storeDescription = this._storeDescription;
//            sp._dynamicStore = this._dynamicStore;
//            sp._text = this._text;
//            sp._status = this._status;
//            sp._stockStatus = this._stockStatus;
//            sp._similarStoreModel = this._similarStoreModel; 	// Issue 3557 - stodd
//            // BEGIN TT#1401 - stodd - add resevation stores
//            sp._IMO_ID = this._IMO_ID;
//            // END TT#1401 - stodd - add resevation stores
//            sp._deleteStore = this._deleteStore;	// TT#739-MD - STodd - delete stores

//            //if (_characteristics != null)
//            //{
//            //    // BEGIN Issue 4656 stodd 9.6.07
//            //    sp._characteristics = new ArrayList();
//            //    // END Issue 4656 stodd 9.6.07
//            //    foreach (StoreCharGroupProfile scgp in this._characteristics)
//            //    {
//            //        StoreCharGroupProfile scgpClone = scgp.Clone();
//            //        sp._characteristics.Add(scgpClone);
//            //    }
//            //}
//            return sp;
//        }
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="aKey">int</param>
//        public StoreProfile(int aKey)
//            : base(aKey)
//        {
//            //_characteristics = null;
//        }

//        /// <summary>
//        /// Returns the eProfileType of this profile.
//        /// </summary>
//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.Store;
//            }
//        }


////		/// <summary>
////		/// Returns this store's value for the characteristic named.
////		/// </summary>
////		/// <param name="CharacteristicName"></param>
////		/// <returns></returns>
////		public object GetCharacteristicValue(string CharacteristicName)
////		{
////			object returnValue = null;
////
////			for (int i=0;i<_characteristics.Count;i++)
////			{
////				StoreCharGroupProfile scgp = (StoreCharGroupProfile)this._characteristics[i];
////				if (scgp.Name == CharacteristicName)
////				{
////					returnValue = scgp.CharacteristicValue.CharValue;
////					break;
////				}
////			}
////
////			return returnValue;
////		}
////
////		/// <summary>
////		/// Returns this store's value for the characteristic group RID sent.
////		/// </summary>
////		/// <param name="CharacteristicGroupRID"></param>
////		/// <returns></returns>
////		public object GetCharacteristicValue(int CharacteristicGroupRID)
////		{
////			object returnValue = null;
////
////			for (int i=0;i<_characteristics.Count;i++)
////			{
////				StoreCharGroupProfile scgp = (StoreCharGroupProfile)this._characteristics[i];
////				if (scgp.Key == CharacteristicGroupRID)
////				{
////					returnValue = scgp.CharacteristicValue.CharValue;
////					break;
////				}
////			}
////
////			return returnValue;
////		}
////
////		/// <summary>
////		/// Returns this store's characteristic value's RID for the characteristic group RID sent.
////		/// </summary>
////		/// <param name="CharacteristicGroupRID"></param>
////		/// <returns></returns>
////		public int GetCharacteristicRID(int CharacteristicGroupRID)
////		{
////			int returnValue=0;
////
////			for (int i=0;i<_characteristics.Count;i++)
////			{
////				StoreCharGroupProfile scgp = (StoreCharGroupProfile)this._characteristics[i];
////				if (scgp.Key == CharacteristicGroupRID)
////				{
////					returnValue = scgp.CharacteristicValue.SC_RID;
////					break;
////				}
////			}
////
////			return returnValue;
////		}

//        /// <summary>
//        /// Unloads StoreProfile in to field by field object array.
//        /// </summary>
//        /// <returns>Object array</returns>
//        public object [] ItemArray()
//        {
//            object[] ar = new object[23];	// TT#1401 - stodd - add resevation stores	// TT#739-MD - STodd - delete stores
//            ar[0] = this.Key;
//            ar[1] = this.StoreId;
//            ar[2] = this.StoreName;
//            ar[3] = this.StoreDescription;
				
//            ar[4] = Convert.ToInt32(this.ActiveInd, CultureInfo.CurrentUICulture);
				
//            ar[5] = this.City;
//            ar[6] = this.State;
//            ar[7] = this.SellingSqFt;
//            ar[8] = checkDateNull(this.SellingOpenDt);
//            ar[9] = checkDateNull(this.SellingCloseDt);
//            ar[10] = checkDateNull(this.StockOpenDt);
//            ar[11] = checkDateNull(this.StockCloseDt);
//            ar[12] = this.LeadTime;
				
//            ar[13] = Convert.ToInt32(this.ShipOnMonday, CultureInfo.CurrentUICulture);
//            ar[14] = Convert.ToInt32(this.ShipOnTuesday, CultureInfo.CurrentUICulture);
//            ar[15] = Convert.ToInt32(this.ShipOnWednesday, CultureInfo.CurrentUICulture);
//            ar[16] = Convert.ToInt32(this.ShipOnThursday, CultureInfo.CurrentUICulture);
//            ar[17] = Convert.ToInt32(this.ShipOnFriday, CultureInfo.CurrentUICulture);
//            ar[18] = Convert.ToInt32(this.ShipOnSaturday, CultureInfo.CurrentUICulture);
//            ar[19] = Convert.ToInt32(this.ShipOnSunday, CultureInfo.CurrentUICulture);
//            // Issue 3557 - stodd
//            ar[20] = Convert.ToInt32(this.SimilarStoreModel, CultureInfo.CurrentUICulture);
//            // BEGIN TT#1401 - stodd - add resevation stores
//            ar[21] = this._IMO_ID;
//            // END TT#1401 - stodd - add resevation stores
//            ar[22] = Convert.ToInt32(this._deleteStore);	// TT#739-MD - STodd - delete stores

//            return ar;
//        }

//        public static object checkDateNull(DateTime tdate)
//        {
//            if (tdate.Year == 1)
//            {
//                return System.DBNull.Value;
//            }
//            else
//            {
//                return tdate;
//            }
//        }

//    }
//    //[Serializable()]
//    //public class StoreExplorerViewProfile : Profile, IComparable , ICloneable
//    //{
//    //    private string _storeId;
//    //    private bool _activeInd;
//    //    private bool _dynamicStore;				// used in store explorer
//    //    private string _text;

//    //    #region Properties
//    //    public string StoreId 
//    //    {
//    //        get { return _storeId ; }
//    //        set { _storeId = value; }
//    //    }
//    //    public bool ActiveInd 
//    //    {
//    //        get { return _activeInd ; }
//    //        set { _activeInd = value; }
//    //    }
//    //    public bool DynamicStore 
//    //    {
//    //        get { return _dynamicStore ; }
//    //        set { _dynamicStore = value; }
//    //    }
//    //    public string Text 
//    //    {
//    //        get { return _text ; }
//    //        set { _text = value; }
//    //    }
//    //    #endregion

//    //    public StoreExplorerViewProfile(int aKey)
//    //        : base(aKey)
//    //    {

//    //    }

	
//    //    override public eProfileType ProfileType
//    //    {
//    //        get
//    //        {
//    //            return eProfileType.Store;
//    //        }
//    //    }

//    //    public override bool Equals(Object obj) 
//    //    {
//    //        if (obj == null || obj.GetType() == typeof(System.DBNull) || ((Profile)obj).ProfileType != ProfileType)
//    //        {
//    //            return Key == Include.NoRID;
//    //        }
//    //        else
//    //        {
//    //            return (Key == ((Profile)obj).Key);
//    //        }
//    //    }

//    //    public override int GetHashCode() 
//    //    {
//    //        try
//    //        {
//    //            return base.GetHashCode();
//    //        }
//    //        catch (Exception exc)
//    //        {
//    //            string message = exc.ToString();
//    //            throw;
//    //        }
//    //    }

//    //    //public int IComparable.CompareTo(object obj)
//    //    public int CompareTo(object obj)
//    //    { 
//    //        return Key - ((StoreExplorerViewProfile)obj).Key; 
//    //    } 
	 
//    //    public static bool operator<(StoreExplorerViewProfile lhs, StoreExplorerViewProfile rhs)
//    //    { 
//    //        return ((IComparable)lhs).CompareTo(rhs) < 0;
//    //    }

//    //    public static bool operator<=(StoreExplorerViewProfile lhs, StoreExplorerViewProfile rhs)
//    //    { 
//    //        return ((IComparable)lhs).CompareTo(rhs) <= 0;
//    //    }

//    //    public static bool operator>(StoreExplorerViewProfile lhs, StoreExplorerViewProfile rhs)
//    //    { 
//    //        return ((IComparable)lhs).CompareTo(rhs) > 0;
//    //    }

//    //    public static bool operator>=(StoreExplorerViewProfile lhs, StoreExplorerViewProfile rhs)
//    //    { 
//    //        return ((IComparable)lhs).CompareTo(rhs) >= 0;
//    //    }

//    //    public object Clone()
//    //    {
//    //        return this;
//    //    }  
//    //}
//}