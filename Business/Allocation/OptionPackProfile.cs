using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
    #region OptionPackProfile Properties Changed Event
    /// <summary>
    /// Identifies an OptionPackProfile whose properties have changed
    /// </summary>
    [Serializable]
    public class OptionPropertiesChangedEventArgs : EventArgs
    {
        OptionPackProfile _opp;
        public OptionPropertiesChangedEventArgs(OptionPackProfile aOptionPackProfile)
        {
            _opp = aOptionPackProfile;
        }
        /// <summary>
        /// Gets OptionPackProfile whose Properties have changed
        /// </summary>
        public OptionPackProfile OptionPackProfileChanged
        {
            get { return _opp; }
        }
    }
    /// <summary>
    /// OptionPropertiesChangedEventHandler Delegate
    /// </summary>
    /// <param name="source">Object firing the event</param>
    /// <param name="args">Arguments passed by the event</param>
    public delegate void OptionPropertiesChangedEventHandler(object source, OptionPropertiesChangedEventArgs args);

    #endregion OptionPackProfile Properties Changed Event

    #region OptionPackProfileList
    /// <summary>
    /// Profile List of OptionPackProfiles.
    /// </summary>
    [Serializable]
    public class OptionPackProfileList : ProfileList
    {
        /// <summary>
        /// Creates an instance of this profile list
        /// </summary>
        public OptionPackProfileList()
            : base(eProfileType.OptionPackProfile)
        {
        }
    }
    #endregion OptionPackProfileList

    #region OptionPackProfile
    /// <summary>
    /// Option Pack Profile:  Profiles a Work Up Pack Buy
    /// </summary>
    [Serializable]
    public class OptionPackProfile : Profile
    {
        #region Fields
        private ApplicationSessionTransaction _appTran;  // TT#744 Use Orig Pack Fit Logic; Remove Bulk
        private string _optionPackID;
        private BuildPacksMethod _buildPacksMethod;
        private int _maxStoreRID;
        private int _maxPackNeedTolerance;
        private double _avgPackDevTolerance; // correction
        private string _fromPatternComboName;
        private int _fromPatternComboRID;
        private int _version;                  // TT#536 Reserve negative

        private StoreSizeVector[] _storeBulkSizeBuy;
        private StoreVector _storeBulkBuy;
        private List<StorePackVector> _storePackBuy;
        private StoreSizeVector[] _storeTotalSizeBuy;
        private StoreVector _storeTotalBuy;

        private StoreSizeVector[] _storeSizeNeedErrors;
        private StoreVector _storeTotalError;
        private StoreVector _storeSizesInErrorCount;

        private int _reserveStoreRID;
        private Dictionary<int, int> _sizeRIDIdxXref;
        private Dictionary<int, int> _packIDIdxXref;

        // Calaculated Properties
        private int _allPackSizeUnitErrors;
        private int _allStoreTotalNumberOfPacks;
        private int _allStoreTotalPackUnits;
        private int _countAllPacksWithErrors;
        private int _countAllStoresWithBulk;
        private int _countAllStoresWithPacks;
        private int _countAllStoresWithUnits;
        private int _countNonReserveStoresWithBulk;
        private int _countNonReserveStoresWithPacks;
        private int _countNonReserveStoresWithUnits;
        private int _countSizesWithAtLeast1Error;
        private int _countSizesWithUnits;
        private int _nonReserveTotalNumberOfPacks;
        private int _nonReserveTotalPackUnits;
        private int _reserveTotalNumberOfPacks;
        private int _reserveTotalPackUnits;
        private int _storesWithErrorCount;
        private int _totalPackUnitsForPacksInError;
        private int _totalSizeUnitError;
        private List<MIDException> _messageLog;
        private int _increaseBuyQty;            // TT#669 Build Pack Variance Enhancement
        private bool _depleteReserveSelected;   // TT#669 Build Pack Variance Enhancement
        private int _origTotalUnitBuy;          // TT#801 - Build Packs - Add Stats for Best Pack Solution Select

        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the Option Pack Profile
        /// </summary>
        /// <param name="aBPM">Business Build Packs Method for which this profile is an option pack solution.</param>
        /// <param name="aFromPatternComboName">Pack Pattern Combination Name that is the source of this option pack profile</param>
        /// <param name="aFromPatternComboRID">Pack Pattern Combination RID that is the source of this option pack profile</param>
        /// <param name="aOptionPackKey">OptionPackKey that uniquely identifies this option within the generated options from the source Pack Pattern Combination</param>
        /// <param name="aOptionPackID">OptionPackID for this Option Pack Profile</param>
        /// <param name="aStoreBulkSizeBuy">Work Up Size Buy to use in building the packs</param>
        /// <param name="aReserveStoreRID">Reserve store RID; if no reserve store, set this value to Include.NoRID</param>
        /// <param name="aMaxPackNeedTolerance">Maximum Pack Need Tolerance (aka Ship Variance)</param>
        /// <param name="aAvgPackDevTolerance">Average Pack Deviation Tolerance</param>
        /// <param name="aVersion">Version of this pack solution</param>
        /// <param name="aDepleteReserveSelected">True:  Reserve is to be depleted if fitting a pack to a store introduces a variance</param>
        /// <param name="aIncreaseBuyQty">The amount the buy may be increased if a fitting a pack to a store introduces a variance</param>
        public OptionPackProfile(
            ApplicationSessionTransaction aApplicationSessionTransaction,   // TT#744 Use Orig Pack Fit Logic; Remove Bulk
            BuildPacksMethod aBPM,
            string aFromPatternComboName,
            int aFromPatternComboRID,
            int aOptionPackKey,
            string aOptionPackID,
            StoreSizeVector[] aStoreBulkSizeBuy,
            int aReserveStoreRID,
            uint aMaxPackNeedTolerance,
            double aAvgPackDevTolerance,    // Correction  // TT#536 -- negative reserve
            int aVersion,                   // TT#536 -- negative reserve // TT#669 Build Pack Variance Enhancement
            bool aDepleteReserveSelected,   // TT#669 Build Pack Variance Enhancement
            int aIncreaseBuyQty)            // TT#669 Build Pack Variance Enhancement
            : base(aOptionPackKey)
        {
            _appTran = aApplicationSessionTransaction; // TT#744 Use Orig Pack Fit Logic; Remove Bulk

            //Begin TT#739-MD -jsobek -Delete Stores -Max Store RID
            //_maxStoreRID = MIDStorageTypeInfo.GetStoreMaxRID(0);
            _maxStoreRID = MaxStoresHelper.GetStoreMaxRID(0);
            //End TT#739-MD -jsobek -Delete Stores -Max Store RID


            _buildPacksMethod = aBPM;
            _fromPatternComboName = aFromPatternComboName;
            _fromPatternComboRID = aFromPatternComboRID;
            _optionPackID = aOptionPackID;
            _maxPackNeedTolerance = (int)aMaxPackNeedTolerance;
            _avgPackDevTolerance = aAvgPackDevTolerance;    // Correction
            _version = aVersion;   // TT#536 -- negative Reserve
            _reserveStoreRID = aReserveStoreRID;
            _sizeRIDIdxXref = new Dictionary<int, int>();
            //_packIDIdxXref = new Dictionary<int, int>();  TT#744 Use Orig Pack Fit Logic; Remove Bulk
            _storeBulkSizeBuy = new StoreSizeVector[aStoreBulkSizeBuy.Length];
            _storeBulkBuy = new StoreVector();
            _storePackBuy = new List<StorePackVector>();
            _storeTotalSizeBuy = new StoreSizeVector[aStoreBulkSizeBuy.Length];
            _storeTotalBuy = new StoreVector();
            _storeSizeNeedErrors = new StoreSizeVector[aStoreBulkSizeBuy.Length];
            _storeTotalError = new StoreVector();
            _storeSizesInErrorCount = new StoreVector();
            int sizeCodeRID;
            double storeValue;  // TT#744 - JEllis - Use Orig Pack Fit logic; remove bulk
            for (int i = 0; i < aStoreBulkSizeBuy.Length; i++)
            {
                sizeCodeRID = aStoreBulkSizeBuy[i].SizeCodeRID;
                _sizeRIDIdxXref.Add(sizeCodeRID, i);
                _storeBulkSizeBuy[i] = new StoreSizeVector(sizeCodeRID);
                _storeTotalSizeBuy[i] = new StoreSizeVector(sizeCodeRID);
                _storeSizeNeedErrors[i] = new StoreSizeVector(sizeCodeRID);
                for (int j = aStoreBulkSizeBuy[i].MaxStoreRID; j > 0; j--)
                {
                    storeValue = aStoreBulkSizeBuy[i].GetStoreValue(j);
                    _storeBulkSizeBuy[i].SetStoreValue(j, storeValue);
                    _storeTotalSizeBuy[i].SetStoreValue(j, _storeTotalSizeBuy[i].GetStoreValue(j) + storeValue);
                    _storeBulkBuy.SetStoreValue(j, _storeBulkBuy.GetStoreValue(j) + storeValue);
                    _storeTotalBuy.SetStoreValue(j, _storeBulkBuy.GetStoreValue(j));
                }
            }
            _origTotalUnitBuy = (int)_storeTotalBuy.AllStoreTotalValue ;  // TT#801 - Build Packs - Add Stats For Best Pack Solution Select
            ResetProperties(true);
            _messageLog = new List<MIDException>();
            _depleteReserveSelected = aDepleteReserveSelected; // TT#669 Build Pack Variance Enhancement
            _increaseBuyQty = aIncreaseBuyQty;                 // TT#669 Build Pack Variance Enhancement
        }
        #endregion Constructors

        #region Properties
        #region General Properities
        /// <summary>
        /// Gets the profile type of this profile
        /// </summary>
        public override eProfileType ProfileType
        {
            get { return eProfileType.OptionPackProfile; }
        }
        /// <summary>
        /// Gets the ID of this Option Pack Profile
        /// </summary>
        public string OptionPackID
        {
            get { return _optionPackID; }
        }
        /// <summary>
        /// Gets the Key of this Option Pack Profile
        /// </summary>
        public int OptionPackKey
        {
            get { return base.Key; }
        }
        /// <summary>
        /// Gets the Pack Pattern Name that is the source of this option pack profile
        /// </summary>
        public string FromPackPatternComboName
        {
            get { return _fromPatternComboName; }
        }
        /// <summary>
        /// Gets the Pack Pattern Combo RID that is the source of this option pack profile
        /// </summary>
        public int FromPackPatternComboRID
        {
            get { return _fromPatternComboRID; }
        }
        /// <summary>
        /// Gets the Ship Variance (aka Maximum Pack Need Tolerance) for this Option Pack Profile
        /// </summary>
        public int ShipVariance
        {
            get { return _maxPackNeedTolerance; }
        }

        // begin TT#536 negative reserve
        /// <summary>
        /// Gets the version of this option pack profile
        /// </summary>
        public int Version
        {
            get { return _version; }
        }
        // end TT#536 negative reserve

        /// <summary>
        /// Gets the Maximum Pack Need Tolerance (aka Ship Variance) for this Option Pack Profile
        /// </summary>
        public int MaxPackNeedTolerance
        {
            get { return _maxPackNeedTolerance; }
        }
        /// <summary>
        /// Gets the Average Pack Deviation Tolerance for this Option Pack Profile
        /// </summary>
        public double AvgPackDevTolerance   // Correction
        {
            get { return _avgPackDevTolerance; }
        }
        // begin TT#801 - Build Packs - Add Stats for Best Pack Solution Select
        /// <summary>
        /// Total Units in the original buy
        /// </summary>
        public int OriginalTotalUnitBuy
        {
            get { return _origTotalUnitBuy; }
        }
        /// <summary>
        /// Percent of "current" total buy to original buy
        /// </summary>
        public double PercentTotalToOriginalBuy
        {
            get 
            {
                if (_origTotalUnitBuy == 0)
                {
                    return 0;
                }
                return _storeTotalBuy.AllStoreTotalValue * 100 / _origTotalUnitBuy;
            }
        }
        /// <summary>
        /// Percent of "original" buy that is packaged
        /// </summary>
        public double PercentOriginalBuyPackaged
        {
            get
            {
                if (_origTotalUnitBuy == 0)
                {
                    return 0;
                }
                return
                    ((double)OriginalBuyPackUnits)
                    * 100
                    / _origTotalUnitBuy;
            }
        }
        /// <summary>
        /// Original Buy Units in packs
        /// </summary>
        public int OriginalBuyPackUnits
        {
            get { return (int)(AllStoreTotalPackUnits - TotalSizeUnitError); }
        }
        // end TT#801 - Build Packs - Add Stats for Best Pack Solutino Select
        /// <summary>
        /// Gets a list of the messages issued while creating/processing this option pack profile
        /// </summary>
        public List<MIDException> MessageLog
        {
            get
            {
                return _messageLog;
            }
        }
        #endregion General Properties

        #region OptionPacks
        /// <summary>
        /// Gets the OptionPack_Combo that describes the pack options within this Option Pack Profile.  In other words, describes a proposed way to "package" the buy.
        /// </summary>
        public OptionPack_Combo OptionPacks
        {
            get
            {
                List<PackPattern> optionPacks = new List<PackPattern>();
                OptionPack_PackPattern oppp;
                foreach (StorePackVector spv in _storePackBuy)
                {
                    oppp = new OptionPack_PackPattern(
                        spv.PackID.ToString(CultureInfo.CurrentCulture),
                        spv.PackPatternRID,
                        spv.PatternName,
                        spv.PackSizeUnits);
                    optionPacks.Add(oppp);
                }
                OptionPack_Combo opc =
                    new OptionPack_Combo(
                        base.Key,
                        _fromPatternComboRID,
                        _fromPatternComboName,
                        false,
                        optionPacks);
                return opc;
            }
        }
        #endregion OptionPacks

        #region Store Pack Allocation
        /// <summary>
        /// Gets Store Pack Allocation.
        /// </summary>
        public StorePackVector[] StorePackBuy
        {
            get
            {
                StorePackVector[] storePackBuy = new StorePackVector[_storePackBuy.Count];
                int i = 0;
                foreach (StorePackVector spv in _storePackBuy)
                {
                    storePackBuy[i] = (StorePackVector)spv.Clone();
                    i++;
                }
                return storePackBuy;
            }
        }
        #endregion Store Pack Allocation

        #region Total Buy Units
        #region All Stores
        /// <summary>
        /// Gets the All Store Total Buy Units
        /// </summary>
        public int AllStoreTotalBuy
        {
            get { return (int)_storeTotalBuy.AllStoreTotalValue; }     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
        }
        /// <summary>
        /// Gets the All Store Bulk Buy Units
        /// </summary>
        public int AllStoreBulkBuy
        {
            get { return (int)_storeBulkBuy.AllStoreTotalValue; }     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
        }
        #endregion All Stores

        #region Non-Reserve Stores
        /// <summary>
        /// Gets the Non-Reserve Total Buy Units
        /// </summary>
        public int NonReserveTotalBuy
        {
            get { return AllStoreTotalBuy - ReserveTotalBuy; }
        }
        /// <summary>
        /// Gets the Non-Reserve Bulk Buy Units
        /// </summary>
        public int NonReserveBulkBuy
        {
            get { return AllStoreBulkBuy - ReserveBulkBuy; }
        }
        #endregion Non-Reserve Stores

        #region Reserve Store
        /// <summary>
        /// Gets the Reserve Total Buy Units
        /// </summary>
        public int ReserveTotalBuy
        {
            get
            {
                if (_reserveStoreRID > 0)
                {
                    return (int)_storeTotalBuy.GetStoreValue(_reserveStoreRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                }
                else
                {
                    return 0;
                }

            }
        }

        /// <summary>
        /// Gets the Reserve Bulk Buy Units
        /// </summary>
        public int ReserveBulkBuy
        {
            get
            {
                if (_reserveStoreRID > 0)
                {
                    return (int)_storeBulkBuy.GetStoreValue(_reserveStoreRID);     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                }
                else
                {
                    return 0;
                }

            }
        }
        #endregion Reserve Store
        #endregion Total Buy Units

        #region Size Buy Units
        #region All Stores
        /// <summary>
        /// Gets the All Store Bulk Size Buy
        /// </summary>
        public SizeUnits[] AllStoreBulkSizeBuy
        {
            get
            {
                SizeUnits[] bulkSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
                int sizePosition;
                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                {
                    sizePosition = keyValue.Value;
                    bulkSizeBuy[sizePosition] =
                        new SizeUnits(
                            keyValue.Key,
                            (int)_storeBulkSizeBuy[sizePosition].AllStoreTotalValue);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                }
                return bulkSizeBuy;
            }
        }
        /// <summary>
        ///  Gets the All Store Total Size Buy
        /// </summary>
        public SizeUnits[] AllStoreTotalSizeBuy
        {
            get
            {
                SizeUnits[] totalSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
                int sizePosition;
                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                {
                    sizePosition = keyValue.Value;
                    totalSizeBuy[sizePosition] =
                        new SizeUnits(
                            keyValue.Key,
                            (int)_storeTotalSizeBuy[sizePosition].AllStoreTotalValue);   // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                }
                return totalSizeBuy;
            }
        }
        #endregion All Stores

        #region Non-Reserve Stores
        /// <summary>
        /// Gets the Non-Reserve Bulk Size Buy
        /// </summary>
        public SizeUnits[] NonReserveBulkSizeBuy
        {
            get
            {
                SizeUnits[] bulkSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
                int sizePosition;
                if (_reserveStoreRID > 0)
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        bulkSizeBuy[sizePosition] =
                            new SizeUnits(
                                keyValue.Key,
                                (int)_storeBulkSizeBuy[sizePosition].AllStoreTotalValue - (int)_storeBulkSizeBuy[sizePosition].GetStoreValue(_reserveStoreRID));    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        bulkSizeBuy[sizePosition] =
                            new SizeUnits(
                                keyValue.Key,
                                (int)_storeBulkSizeBuy[sizePosition].AllStoreTotalValue);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                return bulkSizeBuy;
            }
        }
        /// <summary>
        /// Gets the Non-Reserve Total Size Buy
        /// </summary>
        public SizeUnits[] NonReserveTotalSizeBuy
        {
            get
            {
                SizeUnits[] totalSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
                int sizePosition;
                if (_reserveStoreRID > 0)
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        totalSizeBuy[sizePosition] =
                            new SizeUnits(
                                keyValue.Key,
                                (int)this._storeTotalSizeBuy[sizePosition].AllStoreTotalValue - (int)_storeTotalSizeBuy[sizePosition].GetStoreValue(_reserveStoreRID));    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        totalSizeBuy[sizePosition] =
                            new SizeUnits(
                                keyValue.Key,
                                (int)this._storeTotalSizeBuy[sizePosition].AllStoreTotalValue);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                return totalSizeBuy;
            }
        }
        #endregion Non-Reserve Stores

        #region Reserve Store
        /// <summary>
        /// Gets the Reserve Bulk Size Buy
        /// </summary>
        public SizeUnits[] ReserveBulkSizeBuy
        {
            get
            {
                SizeUnits[] reserveSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
                int sizePosition;
                if (_reserveStoreRID > 0)
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        reserveSizeBuy[sizePosition] =
                            new SizeUnits(keyValue.Key, (int)this._storeBulkSizeBuy[sizePosition].GetStoreValue(_reserveStoreRID));    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        reserveSizeBuy[sizePosition] = new SizeUnits(keyValue.Key, 0);
                    }
                }
                return reserveSizeBuy;
            }
        }

        /// <summary>
        /// Gets the Reserve Total Size Buy
        /// </summary>
        public SizeUnits[] ReserveTotalSizeBuy
        {
            get
            {
                SizeUnits[] reserveSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
                int sizePosition;
                if (_reserveStoreRID > 0)
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        reserveSizeBuy[sizePosition] =
                            new SizeUnits(keyValue.Key, (int)this._storeTotalSizeBuy[sizePosition].GetStoreValue(_reserveStoreRID));      // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizePosition = keyValue.Value;
                        reserveSizeBuy[sizePosition] = new SizeUnits(keyValue.Key, 0);
                    }
                }
                return reserveSizeBuy;
            }
        }
        #endregion Reserve Store
        #endregion Size Buy Units

        #region Total Pack Units Properties
        #region All Store
        /// <summary>
        /// Gets the All Store Units that are in packs
        /// </summary>
        public int AllStoreTotalPackUnits
        {
            get
            {
                if (_allStoreTotalPackUnits < 0)
                {
                    AccumPackErrors();
                }
                return _allStoreTotalPackUnits;
            }
        }
        #endregion All Store

        #region Non-Reserve Stores
        /// <summary>
        /// Gets the NonReserve Store Number of units in Packs
        /// </summary>
        public int NonReserveTotalPackUnits
        {
            get
            {
                if (_nonReserveTotalPackUnits < 0)
                {
                    if (_reserveStoreRID > 0)
                    {
                        _nonReserveTotalPackUnits = 0;
                        foreach (StorePackVector spv in _storePackBuy)
                        {
                            _nonReserveTotalPackUnits +=
                                (int)spv.AllStorePackTotalUnits                     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                - spv.GetStorePackTotalUnits(_reserveStoreRID);
                        }
                    }
                    else
                    {
                        _nonReserveTotalPackUnits = AllStoreTotalPackUnits;
                    }
                }
                return _nonReserveTotalPackUnits;
            }
        }
        #endregion Non-Reserve Stores

        #region Reserve Store
        /// <summary>
        /// Gets the Reserve Store Number of Units in Packs
        /// </summary>
        public int ReserveTotalPackUnits
        {
            get
            {
                if (_reserveTotalPackUnits < 0)
                {
                    _reserveTotalPackUnits = 0;
                    if (_reserveStoreRID > 0)
                    {
                        _reserveTotalPackUnits = 0;
                        foreach (StorePackVector spv in _storePackBuy)
                        {
                            _reserveTotalPackUnits +=
                                spv.GetStorePackTotalUnits(_reserveStoreRID);
                        }
                    }
                }
                return _reserveTotalPackUnits;
            }
        }
        #endregion Reserve Store
        #endregion Total Pack Units Properties

        #region Total Number of Packs Properties
        #region All Store
        /// <summary>
        /// Gets the All Store Total Number of Packs
        /// </summary>
        public int AllStoreTotalNumberOfPacks
        {
            get
            {
                if (_allStoreTotalNumberOfPacks < 0)
                {
                    _allStoreTotalNumberOfPacks = 0;
                    foreach (StorePackVector spv in _storePackBuy)
                    {
                        _allStoreTotalNumberOfPacks += (int)spv.AllStoreTotalValue;     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                return _allStoreTotalNumberOfPacks;
            }
        }
        #endregion All Store

        #region Non-Reserve Stores
        /// <summary>
        /// Gets the Non-Reserve Total number of Packs
        /// </summary>
        public int NonReserveTotalNumberOfPacks
        {
            get
            {
                if (_nonReserveTotalNumberOfPacks < 0)
                {
                    if (_reserveStoreRID > 0)
                    {
                        _nonReserveTotalNumberOfPacks = 0;
                        foreach (StorePackVector spv in _storePackBuy)
                        {
                            _nonReserveTotalNumberOfPacks +=
                                spv.AllStoreNumberOfPacks
                                - (int)spv.GetStoreValue(_reserveStoreRID);     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                        }
                    }
                    else
                    {
                        _nonReserveTotalNumberOfPacks = AllStoreTotalNumberOfPacks;
                    }
                }
                return _nonReserveTotalNumberOfPacks;
            }
        }
        #endregion Non-Reserve stores

        #region Reserve Store
        /// <summary>
        /// Gets the Reserve total number of packs
        /// </summary>
        public int ReserveTotalNumberOfPacks
        {
            get
            {
                if (_reserveTotalNumberOfPacks < 0)
                {
                    _reserveTotalNumberOfPacks = 0;
                    if (_reserveStoreRID > 0)
                    {
                        foreach (StorePackVector spv in _storePackBuy)
                        {
                            _reserveTotalNumberOfPacks +=
                                (int)spv.GetStoreValue(_reserveStoreRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                        }
                    }
                }
                return _reserveTotalNumberOfPacks;
            }
        }
        #endregion Reserve Store
        #endregion Total Number of Packs Properties

        #region Size Count Properties
        /// <summary>
        /// Gets the count of sizes with units.
        /// </summary>
        public int CountSizesWithUnits
        {
            get
            {
                if (_countSizesWithUnits < 0)
                {
                    _countSizesWithUnits = 0;
                    foreach (StoreSizeVector ssv in _storeTotalSizeBuy)
                    {
                        if (ssv.AllStoreTotalValue > 0)
                        {
                            _countSizesWithUnits++;

                        }
                    }
                }
                return _countSizesWithUnits;
            }
        }
        /// <summary>
        /// Gets the count of sizes having at least a 1 unit error.
        /// </summary>
        public int CountSizesWithAtLeast1Error
        {
            get
            {
                if (_countSizesWithAtLeast1Error < 0)
                {
                    _countSizesWithAtLeast1Error = 0;
                    foreach (StoreSizeVector ssv in _storeSizeNeedErrors)
                    {
                        if (ssv.AllStoreTotalValue > 0)
                        {
                            _countSizesWithAtLeast1Error++;
                        }
                    }
                }
                return _countSizesWithAtLeast1Error;
            }
        }
        #endregion Size Count Properties

        #region Total Size UNIT Error
        /// <summary>
        /// Gets the total size units in error (all stores and all sizes)
        /// </summary>
        public int TotalSizeUnitError
        {
            get
            {
                if (_totalSizeUnitError < 0)
                {
                    _totalSizeUnitError = 0;
                    foreach (StoreSizeVector svv in _storeSizeNeedErrors)
                    {
                        _totalSizeUnitError += (int)svv.AllStoreTotalValue;     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                return _totalSizeUnitError;
            }
        }
        #endregion Total Size UNIT Error

        #region Average Size Error
        /// <summary>
        /// Gets the average error per size relative to ALL sizes with units whether size has error or not.
        /// </summary>
        public double AvgErrorPerSizeWithUnits
        {
            get
            {
                if (CountSizesWithUnits == 0)
                {
                    return 0;
                }
                return (double)TotalSizeUnitError / (double)CountSizesWithUnits;
            }
        }
        /// <summary>
        /// Gets the average error per size having errors.
        /// </summary>
        public double AvgErrorPerSizeInError
        {
            get
            {
                if (CountSizesWithAtLeast1Error == 0)
                {
                    return 0;
                }
                return (double)TotalSizeUnitError / (double)CountSizesWithAtLeast1Error;
            }
        }
        /// <summary>
        /// Gets the average size error per pack
        /// </summary>
        public double AvgErrorPerPack
        {
            get
            {
                if (_allPackSizeUnitErrors < 0)
                {
                    AccumPackErrors();
                }
                if (AllStoreTotalPackUnits > 0)
                {
                    return (double)_allPackSizeUnitErrors * 100
                            / (double)AllStoreTotalPackUnits;
                }
                return 0;
            }
        }
         /// <summary>
        /// Gets the average size error in the packs that had errors
        /// </summary>
        public double AvgErrorPerPackWithError
        {
            get
            {
                if (_totalPackUnitsForPacksInError < 0)
                {
                    AccumPackErrors();
                }
                if (_totalPackUnitsForPacksInError > 0)
                {
                    return (double)_allPackSizeUnitErrors 
                        / (double)_totalPackUnitsForPacksInError;
                }
                return 0;
            }
        }
        /// <summary>
        /// Gets the average error per store having an error
        /// </summary>
        public double AvgErrorPerStoreWithError
        {
            get
            {
                if (StoresWithErrorCount == 0)
                {
                    return 0;
                }
                return (double)TotalSizeUnitError / (double)StoresWithErrorCount;
            }
        }
        /// <summary>
        /// Gets the average error per (non-reserve)store with packs
        /// </summary>
        public double AvgErrorPerStoreWithPacks
        {
            get
            {
                if (CountOfNonReserveStoresWithPacks == 0)
                {
                    return 0;
                }
                return (double)TotalSizeUnitError / (double)CountOfNonReserveStoresWithPacks;
            }
        }
        /// <summary>
        /// Gets the average error per (non-reserve) store with units
        /// </summary>
        public double AvgErrorPerStoreWithUnits
        {
            get
            {
                if (CountOfNonReserveStoresWithUnits == 0)
                {
                    return 0;
                }
                return (double)TotalSizeUnitError / (double) CountOfNonReserveStoresWithUnits;
            }
        }

        #endregion Average Size Error

        #region Percent Units In Packs
        #region All Stores
        /// <summary>
        /// Gets percent of all store units that are in packs
        /// </summary>
        public double PercentAllStoreUnitsInPacks
        {
            get
            {
                int allStoreTotalBuy = AllStoreTotalBuy;
                if (allStoreTotalBuy != 0)
                {
                    return (double)AllStoreTotalPackUnits * 100d
                           / (double)allStoreTotalBuy;
                }
                return 0;
            }
        }
        #endregion All Stores

        #region Non-Reserve Stores
        /// <summary>
        /// Gets percent of non-reserve store units that are in packs.
        /// </summary>
        public double PercentNonReserveUnitsInPacks
        {
            get
            {
                int nonReserveTotalBuy = NonReserveTotalBuy;
                if (nonReserveTotalBuy != 0)
                {
                    return (double)NonReserveTotalPackUnits * 100d
                           / (double)nonReserveTotalBuy;
                }
                return 0;
            }
        }
        #endregion Non-Reserve Stores

        #region Reserve Store
        /// <summary>
        /// Gets percent of reserve units that are in packs
        /// </summary>
        public double PercentReserveUnitsInPacks
        {
            get
            {
                int reserveTotalBuy = ReserveTotalBuy;
                if (reserveTotalBuy != 0)
                {
                    return (double)ReserveTotalPackUnits * 100d
                            / (double)reserveTotalBuy;
                }
                return 0;
            }
        }
        #endregion Reserve Stores
        #endregion Percent Units in Packs

        #region Percent Units to Total Units
        /// <summary>
        /// Gets percent of reserve units to all store total units
        /// </summary>
        public double PercentReserveInTotal
        {
            get
            {
                int allStoreTotalBuy = AllStoreTotalBuy;
                if (allStoreTotalBuy != 0)
                {
                    return (double)ReserveTotalBuy * 100d
                            / (double)allStoreTotalBuy;
                }
                return 0;
            }
        }
        /// <summary>
        /// Gets percent of all store bulk units to all store total units
        /// </summary>
        public double PercentBulkToTotal
        {
            get
            {
                int allStoreTotalBuy = AllStoreTotalBuy;
                if (allStoreTotalBuy != 0)
                {
                    return (double)AllStoreBulkBuy * 100d
                           / (double)allStoreTotalBuy;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets percent of all reserve bulk units relative to all store total buy units
        /// </summary>
        public double PercentBulkInReserve
        {
            get
            {
                int allStoreTotalBuy = AllStoreTotalBuy;
                if (allStoreTotalBuy != 0)
                {
                    return (double)ReserveBulkBuy * 100d
                            / (double)allStoreTotalBuy;
                }
                return 0;
            }
        }
        #endregion Percent Units to Total Units

        #region Percent Bulk Units In Reserve
        /// <summary>
        /// Gets percent of the total bulk units that are in reserve
        /// </summary>
        public double PercentBulkTotalInReserve
        {
            get
            {
                int allStoreBulkBuy = AllStoreBulkBuy;
                if (allStoreBulkBuy != 0)
                {
                    return (double)ReserveBulkBuy * 100d
                            / (double)allStoreBulkBuy;
                }
                return 0;
            }
        }
        #endregion Percent Bulk Units in Reserve

        #region Percent reserve that is bulk
        /// <summary>
        /// Gets percent of reserve units that are bulk units.
        /// </summary>
        public double PercentReserveInBulk
        {
            get
            {
                if (_reserveStoreRID > 0)
                {
                    int reserveTotalBuy = ReserveTotalBuy;
                    if (reserveTotalBuy > 0)
                    {
                        return (double)ReserveBulkBuy * 100d
                                / (double)reserveTotalBuy;
                    }
                }
                return 0;
            }
        }
        #endregion Percent Reserve that is bulk

        #region Store Count Properties
        #region Store Count With Packs
        /// <summary>
        /// Gets the count of "all" stores with at least 1 pack
        /// </summary>
        public int CountOfAllStoresWithPacks
        {
            get
            {
                if (_countAllStoresWithPacks < 0)
                {
                    _countAllStoresWithPacks = 0;
                    for (int i = _maxStoreRID; i > 0; i--)
                    {
                        for (int j = 0; j < _storePackBuy.Count; j++)
                        {
                            if (_storePackBuy[j].GetStoreValue(i) > 0)
                            {
                                _countAllStoresWithPacks++;
                                break;
                            }
                        }
                    }
                }
                return _countAllStoresWithPacks;
            }
        }
        /// <summary>
        /// Gets the count of Non-reserve stores with at least 1 pack
        /// </summary>
        public int CountOfNonReserveStoresWithPacks
        {
            get
            {
                if (_countNonReserveStoresWithPacks < 0)
                {
                    _countNonReserveStoresWithPacks = CountOfAllStoresWithPacks;

                    if (_reserveStoreRID > 0)
                    {
                        for (int j = 0; j < _storePackBuy.Count; j++)
                        {
                            if (_storePackBuy[j].GetStoreValue(_reserveStoreRID) > 0)
                            {
                                _countNonReserveStoresWithPacks--;
                                break;
                            }
                        }
                    }
                }
                return _countNonReserveStoresWithPacks;
            }
        }
        #endregion Store Count With Packs

        #region Store Count With Units
        /// <summary>
        /// Gets count of stores with at least 1 unit allocated
        /// </summary>
        public int CountOfAllStoresWithUnits
        {
            get
            {
                if (_countAllStoresWithUnits < 0)
                {
                    _countAllStoresWithUnits = 0;
                    for (int j = _maxStoreRID; j > 0; j--)
                    {
                        if (_storeTotalBuy.GetStoreValue(j) > 0)
                        {
                            _countAllStoresWithUnits++;
                        }
                    }
                }
                return _countAllStoresWithUnits;
            }
        }
        /// <summary>
        /// Gets count of non-reserve stores with at least 1 unit allocated
        /// </summary>
        public int CountOfNonReserveStoresWithUnits
        {
            get
            {
                if (_countNonReserveStoresWithUnits < 0)
                {
                    _countNonReserveStoresWithUnits = CountOfAllStoresWithUnits;
                    if (_reserveStoreRID > 0)
                    {
                        if (_storeTotalBuy.GetStoreValue(_reserveStoreRID) > 0)
                        {
                            _countNonReserveStoresWithUnits -= 1;
                        }
                    }
                }
                return _countNonReserveStoresWithUnits;
            }
        }
        #endregion Store Count With Units

        #region Store Count with Bulk
        /// <summary>
        /// Gets the count of "all" stores with bulk units
        /// </summary>
        public int CountOfAllStoresWithBulk
        {
            get
            {
                if (_countAllStoresWithBulk < 0)
                {
                    _countAllStoresWithBulk = 0;
                    for (int i = _storeBulkBuy.MaxStoreRID; i > 0; i--)
                    {
                        if (_storeBulkBuy.GetStoreValue(i) > 0)
                        {
                            _countAllStoresWithBulk++;
                        }
                    }
                }
                return _countAllStoresWithBulk;
            }
        }
        /// <summary>
        /// Gets the count of Non-reserve stores with bulk units
        /// </summary>
        public int CountOfNonReserveStoresWithBulk
        {
            get
            {
                if (_countNonReserveStoresWithBulk < 0)
                {
                    _countNonReserveStoresWithBulk = CountOfAllStoresWithBulk;

                    if (_reserveStoreRID > 0)
                    {
                        if (_storeBulkBuy.GetStoreValue(_reserveStoreRID) > 0)
                        {
                            _countNonReserveStoresWithBulk--;
                        }
                    }
                }
                return _countNonReserveStoresWithBulk;
            }
        }
        #endregion Store Count with Bulk

        #region Store Count with Size Errors
        /// <summary>
        /// Gets number of stores having a size error.
        /// </summary>
        public int StoresWithErrorCount
        {
            get
            {
                if (_storesWithErrorCount < 0)
                {
                    _storesWithErrorCount = 0;

                    for (int i = _maxStoreRID; i > 0; i--)
                    {
                        foreach (StoreSizeVector ssv in _storeSizeNeedErrors)
                        {
                            if (ssv.GetStoreValue(i) > 0)
                            {
                                _storesWithErrorCount++;
                                break;
                            }
                        }
                    }
                }
                return _storesWithErrorCount;
            }

        }
        #endregion Store Count with Size Errors
        #endregion Store Count Properties

        #region Percent of Stores Properties
        /// <summary>
        /// Gets percentage of stores having a size error.
        /// </summary>
        public double PercentNonReserveWithUnitsInError
        {
            get
            {
                if (CountOfNonReserveStoresWithUnits == 0)
                {
                    return 0;
                }
                return (double)StoresWithErrorCount * 100d / (double)CountOfNonReserveStoresWithUnits;
            }
        }
        #endregion Percentage of Stores Properties
        #endregion Properties

        #region Methods
        #region ResetProperties
        /// <summary>
        /// Event when Option Pack Properties have changed. EventArgs Class: OptionPropertiesChangedEventArgs
        /// </summary>
        public event OptionPropertiesChangedEventHandler optionPropertiesChangedEvent;

        /// <summary>
        /// Resets properties so they will be recalculated as the result of a change. Fires the OptionPropertiesChangedEvent.
        /// </summary>
        private void ResetProperties(bool aFromConstructor)
        {
            _allPackSizeUnitErrors = -1;
            _allStoreTotalNumberOfPacks = -1;
            _allStoreTotalPackUnits = -1;
            _countAllPacksWithErrors = -1;
            _countAllStoresWithBulk = -1;
            _countAllStoresWithPacks = -1;
            _countAllStoresWithUnits = -1;
            _countNonReserveStoresWithBulk = -1;
            _countNonReserveStoresWithPacks = -1;
            _countNonReserveStoresWithUnits = -1;
            _countSizesWithAtLeast1Error = -1;
            _countSizesWithUnits = -1;
            _nonReserveTotalPackUnits = -1;
            _reserveTotalPackUnits = -1;
            _nonReserveTotalNumberOfPacks = -1;
            _reserveTotalNumberOfPacks = -1;
            _storesWithErrorCount = -1;
            _totalSizeUnitError = -1;
            _totalPackUnitsForPacksInError = -1;
            if (!aFromConstructor
                && optionPropertiesChangedEvent != null)
            {
                optionPropertiesChangedEvent(this, new OptionPropertiesChangedEventArgs(this));
            }
        }
        /// <summary>
        /// Accumulates packs size errors
        /// </summary>
        private void AccumPackErrors()
        {
            _totalPackUnitsForPacksInError = 0;
            _allPackSizeUnitErrors = 0;
            _allStoreTotalPackUnits = 0;
            foreach (StorePackVector spv in _storePackBuy)
            {
                _allStoreTotalPackUnits += spv.AllStorePackTotalUnits; 
                if (spv.TotalPackSizeError > 0)
                {
                    _totalPackUnitsForPacksInError += spv.AllStorePackTotalUnits;
                    _allPackSizeUnitErrors += spv.TotalPackSizeError;
                }
            }
        }
        #endregion ResetProperties
        // begin TT#552 Build Packs Properties for TAB 4

        private Dictionary<string, int> _spvPackIDIndex;
        /// <summary>
        /// Gets the pack property identified by the specified eBuildPackProperty for the given pack
        /// </summary>
        /// <param name="aBuildPackProperty">Desired Pack Property</param>
        /// <param name="aPackID">Pack ID of the property</param>
        /// <returns>Value of the pack property</returns>
        public double GetPackProperty(eBuildPackProperty aBuildPackProperty, string aPackName)
        {
            StorePackVector spv;
            if (_spvPackIDIndex == null)
            {
                _spvPackIDIndex = new Dictionary<string, int>();
                for (int i=0; i<_storePackBuy.Count; i++)
                {
                    spv = _storePackBuy[i];
                    _spvPackIDIndex.Add(spv.PackID.ToString(), i);
                }
            }
            int packIndex;
            if (!_spvPackIDIndex.TryGetValue(aPackName, out packIndex))
            {
                throw new ArgumentException("Pack Name [" + aPackName.ToString() + "] is not a valid Pack Name");
            }
            spv = _storePackBuy[packIndex];
            switch (aBuildPackProperty)
            {
                case eBuildPackProperty.AllStoreTotalPackUnits:
                    {
                        return (double)spv.AllStorePackTotalUnits;
                    }
                case eBuildPackProperty.NonReserveTotalPackUnits:
                    {
                        if (_reserveStoreRID > 0)
                        {
                            return 
                                (double)spv.AllStorePackTotalUnits
                                - (double)spv.GetStorePackTotalUnits(_reserveStoreRID);
                        }
                        return (double)spv.AllStorePackTotalUnits;
                    }
                case eBuildPackProperty.ReserveTotalPackUnits:
                    {
                        if (_reserveStoreRID > 0)
                        {
                            return (double)spv.GetStorePackTotalUnits(_reserveStoreRID);
                        }
                        return 0;
                    }
                case eBuildPackProperty.AllStoreTotalNumberOfPacks:
                    {
                        return (double)spv.AllStoreNumberOfPacks;
                    }
                case eBuildPackProperty.ReserveTotalNumberOfPacks:
                    {
                        if (_reserveStoreRID > 0)
                        {
                            return (double)spv.GetStoreValue(_reserveStoreRID);
                        }
                        return 0;
                    }
                case eBuildPackProperty.NonReserveTotalNumberOfPacks:
                    {
                        if (_reserveStoreRID > 0)
                        {
                            return (double)spv.AllStoreNumberOfPacks
                                    - (double)spv.GetStoreValue(_reserveStoreRID);
                        }
                        return (double)spv.AllStoreNumberOfPacks;
                    }
                case eBuildPackProperty.CountOfAllStoresWithPacks:
                    {
                       return (double)spv.CountOfAllStoresWithPacks;
                    }
                case eBuildPackProperty.CountOfNonReserveStoresWithPacks:
                    {
                        int count = spv.CountOfAllStoresWithPacks;
                        if (_reserveStoreRID > 0)
                        {
                            if (spv.GetStoreValue(_reserveStoreRID) > 0)
                            {
                                count--;
                            }
                        }
                        return count;
                    }
                case eBuildPackProperty.AverageErrorPerSizeWithUnits:
                    {
                        if (spv.CountOfSizesInPack > 0)
                        {
                            return (double)spv.TotalPackSizeError
                                   / (double) spv.CountOfSizesInPack;
                        }
                        return 0;
                    }
                case eBuildPackProperty.AverageErrorPerStoreWithPacks:
                    {
                        int count = spv.CountOfAllStoresWithPacks;
                        if (_reserveStoreRID > 0
                            && spv.GetStoreValue(_reserveStoreRID) > 0)
                        {
                            count--;
                        }
                        if (count > 0)
                        {
                            return (double)spv.TotalPackSizeError 
                                   / (double)count;
                        }
                        return 0;
                    }
                default:
                    {
                        return 0;
                    }
            }

        }
        // end TT#552 Build Packs Properties for TAB 4


        #region Get Size Buy
        /// <summary>
        /// Gets the Bulk Size Buy for the given store
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Bulk Size Buy for the given store</returns>
        public SizeUnits[] GetStoreBulkSizeBuy(int aStoreRID)
        {
            SizeUnits[] storeSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
            int sizePosition;
            foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
            {
                sizePosition = keyValue.Value;
                storeSizeBuy[sizePosition] = new SizeUnits(keyValue.Key, (int)_storeBulkSizeBuy[sizePosition].GetStoreValue(aStoreRID));    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
            }
            return storeSizeBuy;
        }
        /// <summary>
        /// Gets the Total Size Buy for the given store
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Total Size Buy for the given store</returns>
        public SizeUnits[] GetStoreTotalSizeBuy(int aStoreRID)
        {
            SizeUnits[] storeSizeBuy = new SizeUnits[_sizeRIDIdxXref.Count];
            int sizePosition;
            foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
            {
                sizePosition = keyValue.Value;
                storeSizeBuy[sizePosition] = new SizeUnits(keyValue.Key, (int)_storeTotalSizeBuy[sizePosition].GetStoreValue(aStoreRID));    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
            }
            return storeSizeBuy;
        }
        #endregion Get Size Buy

        #region Apply Pack Pattern to Stores
        /// <summary>
        /// Applies a pack pattern to the stores.  If the total pack units is less than the minimum order, the pack will be rejected.
        /// </summary>
        /// <param name="aPackPattern">Pack Pattern to apply</param>
        /// <param name="aMinimumUnitOrder">Minimum Order</param>
        /// <param name="aStatusReason">Exception describing any failures</param>
        /// <returns>True:  pack was applied successfully; False: pack was not applied; in this case, aStatusReason will give a reason why the pack was not applied</returns>
        internal bool ApplyPackToStores(
            OptionPack_PackPattern aPackPattern, 
            int aMinimumUnitOrder, 
            out MIDException aStatusReason)
        {
            bool success = true;
            aStatusReason = null;
            try
            {
                StorePackVector spv = new StorePackVector(_storePackBuy.Count + 1, aPackPattern);

                StoreSizeVector[] storeBulkSizeBuy = new StoreSizeVector[_sizeRIDIdxXref.Count]; 
                StoreSizeVector[] packSizeErrors = new StoreSizeVector[_sizeRIDIdxXref.Count];
                StoreVector packTotalError = new StoreVector();
                StoreVector packSizeErrorCount = new StoreVector();
                double avgPackSizeError;

                StoreSizeVector[] storeSizeNeedErrors = new StoreSizeVector[_sizeRIDIdxXref.Count];
                StoreVector storeTotalError = _storeTotalError.Clone() as StoreVector;
                StoreVector storeSizesInErrorCount = _storeSizesInErrorCount.Clone() as StoreVector;

                int sizeRID;
                int sizeIdx;
                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                {
                    sizeRID = keyValue.Key;
                    sizeIdx = keyValue.Value;
                    packSizeErrors[sizeIdx] = new StoreSizeVector(sizeRID);
                    storeBulkSizeBuy[sizeIdx] = _storeBulkSizeBuy[sizeIdx].Clone() as StoreSizeVector;
                    storeSizeNeedErrors[sizeIdx] = _storeSizeNeedErrors[sizeIdx].Clone() as StoreSizeVector;
                }

                int storeBulkSizeValue;
                int packSizeValue;
                int packSizeError;
                int packErrorCount;
                int reserveSizeValue;
                int storePackError;
                int[] sizeNeedErrors = new int[_sizeRIDIdxXref.Count];
                int[] reserveSizeUnits = new int[_sizeRIDIdxXref.Count];  // TT#669 Build Pack Variance Enhancement
                int sizesInErrorCount;
                int totalError;
                int increaseBuyQty = _increaseBuyQty; // TT#689 Pack Coverage too small
               
                // begin TT#669 Build Pack Variance Enhancement
                if (_reserveStoreRID > 0
                    && _depleteReserveSelected)
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizeRID = keyValue.Key;
                        sizeIdx = keyValue.Value;
                        reserveSizeUnits[sizeIdx] = (int)storeBulkSizeBuy[sizeIdx].GetStoreValue(_reserveStoreRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizeIdx = keyValue.Value;
                        reserveSizeUnits[sizeIdx] = 0;
                    }
                }
                // end TT#669 Build Pack Variance Enhancement

                // begin TT#689 Pack Coverage is too small
                MIDGenericSortItem[] processStore;
                if (_reserveStoreRID != Include.NoRID)
                {
                    processStore = new MIDGenericSortItem[_maxStoreRID - 1];
                }
                else
                {
                    processStore = new MIDGenericSortItem[_maxStoreRID];
                }
                int processStoreRID;
                int storePosition = 0;
                for (processStoreRID = _maxStoreRID; processStoreRID > 0; processStoreRID--)
                {
                    if (processStoreRID != _reserveStoreRID)
                    {
                        processStore[storePosition].Item = processStoreRID;
                        processStore[storePosition].SortKey = new double[2];
                        processStore[storePosition].SortKey[0] = -_storeBulkBuy.GetStoreValue(processStoreRID);  // sort ascending sequence
                        processStore[storePosition].SortKey[1] = _buildPacksMethod.AppTransaction.GetRandomDouble();
                        storePosition++;
                    }
                }
                Array.Sort(processStore, new MIDGenericSortDescendingComparer());  // sort ascending on total store bulk value
                for (int i=0; i<processStore.Length; i++)
                {
                    processStoreRID = processStore[i].Item;
                //for (int storeRID = _maxStoreRID; storeRID > 0; storeRID--)
                //{
                // end TT#689 Pack Coverage is too small

                    foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                    {
                        sizeIdx = keyValue.Value;
                        sizeNeedErrors[sizeIdx] = (int)storeSizeNeedErrors[sizeIdx].GetStoreValue(processStoreRID); // TT#689 Pack Coverage too small    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    }
                    sizesInErrorCount = (int)storeSizesInErrorCount.GetStoreValue(processStoreRID); // TT#689 Pack Coverage too small    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    totalError = (int)storeTotalError.GetStoreValue(processStoreRID); // TT#689 Pack Coverage too small       // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk

                    int packIncreasesBuy;  // TT#669 Build Pack Variance Enhancement
                    avgPackSizeError = 0;
                    bool applyPack = true;
                    int ttlUnitsInPackWithMatchedHdrSizes;  // TT#886 - Distinct Packs have same size runs
                    while (applyPack)
                    {
                        applyPack = false;
                        storePackError = 0;
                        ttlUnitsInPackWithMatchedHdrSizes = 0; // TT#886 - Distinct Packs have same size runs
                        packIncreasesBuy = 0; // TT#669 Build Pack Variance Enhancement
                        foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                        {
                            sizeRID = keyValue.Key;
                            sizeIdx = keyValue.Value;
                            packSizeError = 0;
                            storeBulkSizeValue =
                                (int)storeBulkSizeBuy[sizeIdx].GetStoreValue(processStoreRID); // TT#689 Pack Coverage too small    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                            packSizeValue =
                                aPackPattern.GetSizeUnits(sizeRID);
                            ttlUnitsInPackWithMatchedHdrSizes += packSizeValue;
                            if (packSizeValue > storeBulkSizeValue)
                            {
                                packSizeError = packSizeValue - storeBulkSizeValue;
                                packSizeErrorCount.SetStoreValue(processStoreRID, packSizeErrorCount.GetStoreValue(processStoreRID) + 1);  // TT#689 Pack Coverage too small
                                packSizeErrors[sizeIdx].SetStoreValue(processStoreRID, packSizeError);  // TT#689 Pack Coverage too small
                                packTotalError.SetStoreValue(processStoreRID, packTotalError.GetStoreValue(processStoreRID) + packSizeError);  // TT#689 Pack Coverage too small
                            }
                            if (packSizeError > 0 && sizeNeedErrors[sizeIdx] == 0)
                            {
                                sizesInErrorCount++;
                            }
                            storePackError += packSizeError;
                            sizeNeedErrors[sizeIdx] += packSizeError;
                            totalError += packSizeError;
                            // begin TT#669 Build Pack Variance Enhancement
                            if (packSizeError > reserveSizeUnits[sizeIdx])
                            {
                                packIncreasesBuy += packSizeError - reserveSizeUnits[sizeIdx];
                            }
                            // end TT#669 Build Pack Variance Enhancement
                        }
                        packErrorCount = (int)packSizeErrorCount.GetStoreValue(processStoreRID); // TT#689 Pack Coverage too small      // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                        // begin TT#886 - Distinct Packs have same size runs
                        if (ttlUnitsInPackWithMatchedHdrSizes != aPackPattern.PackMultiple)
                        {
                            // Reject any pack having at least one size on it that does not match a size on header.
                            //  NOTE:  if we allow a pack to have a size not on the header, our audits on size errors
                            //         will be wrong the way it is tracked now; so our statistics would not be correct
                            //  NOTE:  This rejection will apply only to packs with hard-code size runs because dynamic packs
                            //         always have only the sizes on the header being processed.
                        }
                        else if (packErrorCount == 0)
                        {
                            if (packErrorCount == 0)
                            // end TT#886 - Distinct Packs have same size runs
                            {
                                applyPack = true;
                            }
                            else if (totalError > _maxPackNeedTolerance)
                            {
                            }
                            else
                            {
                                avgPackSizeError =
                                    (double)packTotalError.GetStoreValue(processStoreRID) // TT#689 Pack Coverage too small
                                    / (double)packErrorCount;
                                if (avgPackSizeError > _avgPackDevTolerance)
                                {
                                }
                                // begin TT#669 Build Pack Variance Enhancement
                                //else if (packIncreasesBuy > _increaseBuyQty)  // TT#689 Pack Coverage too small
                                else if (packIncreasesBuy > increaseBuyQty)      // TT#689 Pack Coverage too small
                                {
                                    applyPack = false;
                                }
                                // end TT#669 Build Pack Variance Enhancement
                                else
                                {
                                    applyPack = true;
                                }
                            }
                        }  // TT#886 - Distinct Packs have same size runs
                        if (applyPack)
                        {
                            spv.SetStoreValue(processStoreRID, spv.GetStoreValue(processStoreRID) + 1);  // TT#689 Pack Coverage too small
                            spv.TotalPackSizeError += storePackError;
                            foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                            {
                                sizeRID = keyValue.Key;
                                sizeIdx = keyValue.Value;
                                packSizeValue = aPackPattern.GetSizeUnits(sizeRID);
                                storeBulkSizeValue = (int)storeBulkSizeBuy[sizeIdx].GetStoreValue(processStoreRID); // TT#689 Pack Coverage too small    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                if (packSizeValue < storeBulkSizeValue)
                                {
                                    storeBulkSizeBuy[sizeIdx].SetStoreValue(processStoreRID, storeBulkSizeValue - packSizeValue);  // TT#689 Pack Coverage too small
                                }
                                else
                                {
                                    // begin TT#669 Build Pack Variance Enhancement
                                    packSizeError = packSizeValue - storeBulkSizeValue;
                                    if (packSizeError > reserveSizeUnits[sizeIdx])
                                    {
                                        reserveSizeUnits[sizeIdx] = 0;
                                    }
                                    else
                                    {
                                        reserveSizeUnits[sizeIdx] -= packSizeError;
                                    }
                                    // end TT#669 Build Pack Variance Enhancement

                                    storeBulkSizeBuy[sizeIdx].SetStoreValue(processStoreRID, 0);  // TT#689 Pack Coverage too small
                                }
                                storeSizeNeedErrors[sizeIdx].SetStoreValue(processStoreRID, sizeNeedErrors[sizeIdx]);  // TT#689 Pack Coverage too small
                            }
                            storeSizesInErrorCount.SetStoreValue(processStoreRID, sizesInErrorCount);  // TT#689 Pack Coverage too small
                            storeTotalError.SetStoreValue(processStoreRID, totalError);  // TT#689 Pack Coverage too small
                            // begin TT#669 Build Pack Variance Enhancement
                            //_increaseBuyQty -= packIncreasesBuy;  // TT#689 Pack Coverage too small
                            increaseBuyQty -= packIncreasesBuy;      // TT#689 Pack Coverage too small
                            // end TT#669 Build Pack Variance Enhancement
                        }
                    }
                    //}  // TT#689 Pack Coverage is too small
                }
                //if (spv.AllStoreTotalValue < aMinimumUnitOrder)  // TT#599 BP not all options generated or displayed
                if (spv.AllStorePackTotalUnits < aMinimumUnitOrder) // TT#599 BP not all options generated or displayed
                {
                    aStatusReason = 
                        new MIDException(
                            eErrorLevel.information, 
                            (int)eMIDTextCode.msg_al_PackUnitsLessThanMinimum, 
                            string.Format(
                                MIDText.GetTextOnly(eMIDTextCode.msg_al_PackUnitsLessThanMinimum),
                                _buildPacksMethod.WorkUpSizeBuy.HeaderID,
                                _buildPacksMethod.Name,
                                //spv.AllStoreTotalValue.ToString(CultureInfo.CurrentUICulture), // TT#599 BP not all options generated or displayed
                                spv.AllStorePackTotalUnits.ToString(CultureInfo.CurrentUICulture), // TT#599 BP not all options generated or displayed
                                this.OptionPackID,
                                aMinimumUnitOrder.ToString(CultureInfo.CurrentUICulture),
                                aPackPattern.PackMultiple.ToString(CultureInfo.CurrentUICulture)));
                    _messageLog.Add(aStatusReason);
                    success = false;
                }
                else
                {
                    int storePackSizeValue;
                    
                    int storeOldTotalSizeValue;
                    int storeNewTotalSizeValue;
                    int storeNewTotalValue;
                    int storeOldBulkSizeValue;
                    int storeNewBulkSizeValue;
                    int storeNewBulkValue;
                    for (int storeRID = _maxStoreRID; storeRID > 0; storeRID--)
                    {
                        if (storeRID != _reserveStoreRID)   // TT#535 negative reserve qty
                        {                                    // TT#535 negative reserve qty
                            packSizeValue = (int)spv.GetStoreValue(storeRID);                           // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                            if (packSizeValue > 0)  // TT#535 negative reserve qty
                            {                        // TT#535 negative reserve qty 
                                storeNewTotalValue = (int)_storeTotalBuy.GetStoreValue(storeRID);       // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                storeNewBulkValue = (int)_storeBulkBuy.GetStoreValue(storeRID);         // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                                {
                                    sizeRID = keyValue.Key;
                                    sizeIdx = keyValue.Value;
                                    storePackSizeValue =
                                        packSizeValue
                                        * aPackPattern.GetSizeUnits(sizeRID);
                                    storeOldBulkSizeValue = (int)_storeBulkSizeBuy[sizeIdx].GetStoreValue(storeRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                    storeNewBulkSizeValue = (int)storeBulkSizeBuy[sizeIdx].GetStoreValue(storeRID);     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk

                                    storeOldTotalSizeValue = (int)_storeTotalSizeBuy[sizeIdx].GetStoreValue(storeRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk 

                                    storeNewBulkValue =
                                        storeNewBulkValue
                                        - storeOldBulkSizeValue
                                        + storeNewBulkSizeValue;

                                    storeNewTotalSizeValue =
                                        storeOldTotalSizeValue
                                        - storeOldBulkSizeValue
                                        + storeNewBulkSizeValue
                                        + storePackSizeValue;
                                    storeNewTotalValue =
                                        storeNewTotalValue
                                        - storeOldTotalSizeValue
                                        + storeNewTotalSizeValue;

                                    _storeBulkBuy.SetStoreValue(storeRID, storeNewBulkValue);
                                    _storeTotalSizeBuy[sizeIdx].SetStoreValue(storeRID, storeNewTotalSizeValue);
                                    _storeTotalBuy.SetStoreValue(storeRID, storeNewTotalValue);
                                    if (storePackSizeValue > storeOldBulkSizeValue)
                                    {
                                        if (_depleteReserveSelected)  // TT#669 Build Pack Variance Enhancement
                                        {                             // TT#669 Build Pack Variance Enhancement
                                            // reduce reserve store to zero or by amount of pack size error
                                            if (_reserveStoreRID > 0
                                                && storeRID != _reserveStoreRID)
                                            {
                                                packSizeError =
                                                    storePackSizeValue
                                                    - storeOldBulkSizeValue;
                                                reserveSizeValue = (int)storeBulkSizeBuy[sizeIdx].GetStoreValue(_reserveStoreRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                                if (packSizeError > reserveSizeValue)
                                                {
                                                    packSizeError = reserveSizeValue;

                                                }
                                                storeBulkSizeBuy[sizeIdx].SetStoreValue(_reserveStoreRID, reserveSizeValue - packSizeError);
                                                _storeBulkBuy.SetStoreValue(_reserveStoreRID, _storeBulkBuy.GetStoreValue(_reserveStoreRID) - packSizeError);
                                                _storeTotalSizeBuy[sizeIdx].SetStoreValue(_reserveStoreRID, _storeTotalSizeBuy[sizeIdx].GetStoreValue(_reserveStoreRID) - packSizeError);
                                                _storeTotalBuy.SetStoreValue(_reserveStoreRID, _storeTotalBuy.GetStoreValue(_reserveStoreRID) - packSizeError);
                                            }
                                        }   // TT#669 Build Pack Variance Enhancement
                                    }
                                }
                            }  // TT#535 negative reserve qty
                        }      // TT#535 negative reserve qty
                    }
                    _storeBulkSizeBuy = storeBulkSizeBuy;
                    _storeSizeNeedErrors = storeSizeNeedErrors;
                    _storeTotalError = storeTotalError;
                    _storeSizesInErrorCount = storeSizesInErrorCount;
                    _storePackBuy.Add(spv);
                    _increaseBuyQty = increaseBuyQty; // TT#689 Pack Coverage too small
                }
            }
            catch (MIDException e)
            {
                success = false;
                aStatusReason = e;
                _messageLog.Add(aStatusReason);
            }
            catch (Exception e)
            {
                success = false;
                aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
                _messageLog.Add(aStatusReason);
            }
            finally
            {
                ResetProperties(false);
            }
            return success;
        }
        // begin TT#744 - JEllis - Use Orig Pack Fitting Algorithm; Remove Bulk
        int _packCount;
        int _sizeCount;
        int _storeMaxRID;
        int _sizeIDX;
        int _packIDX;
        int _sizeRID;
        int[,] _packSizeContent;
        double[,] _packSizeCurvePct;
        int[] _packMultiple;
        bool[] _packAvailable;
        int _wrkIncreaseBuyQty;
        StoreSizeVector[] _wrkStoreBulkSizeBuy;
        StoreVector _wrkStoreBulkBuy;
        StoreSizeVector[] _wrkStoreTotalSizeBuy;
        StoreVector _wrkStoreTotalBuy;
        StoreSizeVector[] _wrkPackSizeErrors;
        StoreVector _wrkPackTotalError;
        StoreVector _wrkPackSizeErrorCount;

        StoreSizeVector[] _wrkStoreSizeNeedErrors;
        StoreVector _wrkStoreTotalError;
        StoreVector _wrkStoreSizesInErrorCount;

        public bool AllocatePacksToStores(List<OptionPack_PackPattern> aPackPatternList, int aCompMinOrder, out List<MIDException> aStatusReasonList)
        {
            _packCount = aPackPatternList.Count;
            _sizeCount = this._sizeRIDIdxXref.Count;
            
            //Begin TT#739-MD -jsobek -Delete Stores -Max Store RID
            //_storeMaxRID = MIDStorageTypeInfo.GetStoreMaxRID(0);
            _storeMaxRID = MaxStoresHelper.GetStoreMaxRID(0);
            //End TT#739-MD -jsobek -Delete Stores -Max Store RID


            //======================//
            //  Build Pack Curves   //
            //======================//
            _packSizeContent = new int[_packCount, _sizeCount];
            _packSizeContent.Initialize();
            _packSizeCurvePct = new double[_packCount, _sizeCount];
            _packSizeCurvePct.Initialize();
            _packMultiple = new int[_packCount];
            _packAvailable = new bool[_packCount];

            _packIDX = 0;
            foreach (OptionPack_PackPattern pp in aPackPatternList)
            {
                _packAvailable[_packIDX] = false;  // TT#912 Infinte loop when fixed packs size runs do not match any size on header
                 int totalSizeUnits = 0;
                _packMultiple[_packIDX] = pp.PackMultiple;
                SizeUnitRun sur = pp.SizeRun;

                foreach (KeyValuePair<int, int> sizeRIDIdx in _sizeRIDIdxXref)
                {
                    _sizeIDX = sizeRIDIdx.Value;
                    _sizeRID = sizeRIDIdx.Key;
                    _packSizeContent[_packIDX, _sizeIDX] = sur.GetSizeUnits(_sizeRID);
                    totalSizeUnits += _packSizeContent[_packIDX, _sizeIDX];
                }
                // Note:  we are not doing the typical spread to 100 because
                // we just require a reasonable estimate to use as a comparison
                if (totalSizeUnits > 0)
                {
                    _packAvailable[_packIDX] = true;  // TT#912 Infinte loop when fixed packs size runs do not match any size on header
                    foreach (KeyValuePair<int, int> sizeRIDIdx in _sizeRIDIdxXref)
                    {
                        _sizeIDX = sizeRIDIdx.Value;
                        _sizeRID = sizeRIDIdx.Key;
                        _packSizeCurvePct[_packIDX, _sizeIDX] =
                            _packSizeContent[_packIDX, _sizeIDX] * 100 / totalSizeUnits;
                    }
                }
                _storePackBuy.Add(new StorePackVector(_storePackBuy.Count + 1, pp));
                _packIDX++;
            }
            aStatusReasonList = new List<MIDException>();
            bool status = true;

            bool continueFittingPacks = true;
            List<StorePackVector> wrkStorePackBuy = null;
            while (continueFittingPacks)
            {
                continueFittingPacks = false;

                _wrkIncreaseBuyQty = _increaseBuyQty;
                _wrkStoreBulkSizeBuy = new StoreSizeVector[_sizeCount];
                _wrkStoreTotalSizeBuy = new StoreSizeVector[_sizeCount];
                _wrkPackSizeErrors = new StoreSizeVector[_sizeCount];
                _wrkPackTotalError = new StoreVector();
                _wrkPackSizeErrorCount = new StoreVector();

                _wrkStoreSizeNeedErrors = new StoreSizeVector[_sizeCount];
                _wrkStoreTotalError = _storeTotalError.Clone() as StoreVector;
                _wrkStoreSizesInErrorCount = _storeSizesInErrorCount.Clone() as StoreVector;

                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                {
                    _sizeRID = keyValue.Key;
                    _sizeIDX = keyValue.Value;
                    _wrkPackSizeErrors[_sizeIDX] = new StoreSizeVector(_sizeRID);
                    _wrkStoreBulkSizeBuy[_sizeIDX] = _storeBulkSizeBuy[_sizeIDX].Clone() as StoreSizeVector;
                    _wrkStoreTotalSizeBuy[_sizeIDX] = _storeTotalSizeBuy[_sizeIDX].Clone() as StoreSizeVector;
                    _wrkStoreSizeNeedErrors[_sizeIDX] = _storeSizeNeedErrors[_sizeIDX].Clone() as StoreSizeVector;
                }
                _wrkStoreBulkBuy = _storeBulkBuy.Clone() as StoreVector;
                _wrkStoreTotalBuy = _storeTotalBuy.Clone() as StoreVector;
                wrkStorePackBuy = new List<StorePackVector>();
                for (int packIDX = 0; packIDX < _packCount; packIDX++)
                {
                    wrkStorePackBuy.Add(_storePackBuy[packIDX].Clone() as StorePackVector);
                }

                bool continueProcessStore;
                int storeUnitsToPack;
                int bestFitPackIDX;
                for (int storeRID = _storeMaxRID; storeRID > 0; storeRID--)
                {
                    if (_reserveStoreRID != storeRID)
                    {
                        continueProcessStore = true;

                        while (continueProcessStore)
                        {
                            continueProcessStore = false;
                            storeUnitsToPack = (int)_wrkStoreBulkBuy.GetStoreValue(storeRID);
                            if (storeUnitsToPack > 0)
                            {
                                if (FindBestFitPack(storeRID, out bestFitPackIDX))
                                {
                                    continueProcessStore = true;
                                    //ApplyPackToStore
                                    //if (_storePackBuy[bestFitPackIDX] == null)
                                    //{
                                    //    _storePackBuy[bestFitPackIDX] = new StorePackVector(bestFitPackIDX + 1, aPackPatternList[bestFitPackIDX]);
                                    //}
                                    int packAllocated = wrkStorePackBuy[bestFitPackIDX].GetStoreNumberofPacks(storeRID);
                                    wrkStorePackBuy[bestFitPackIDX].SetStoreValue(storeRID, packAllocated + 1);
                                    int storeBulkSizeValue, storeBulkValue;
                                    int storePackSizeValue;
                                    int storeTotalSizeValue, storeTotalValue;
                                    int storeSizeNeedError, storeTotalError;
                                    int storeReserveSizeValue, storeReserveValue;
                                    int storeTotalReserveSizeValue, storeTotalReserveValue;
                                    int sizeError;
                                    storeBulkValue = (int)_wrkStoreBulkBuy.GetStoreValue(storeRID);
                                    storeTotalError = (int)_wrkStoreTotalError.GetStoreValue(storeRID);
                                    storeTotalValue = (int)_wrkStoreTotalBuy.GetStoreValue(storeRID);
                                    if (_depleteReserveSelected
                                        && _reserveStoreRID > -1)
                                    {
                                        storeReserveValue = (int)_wrkStoreBulkBuy.GetStoreValue(_reserveStoreRID);
                                        storeTotalReserveValue = (int)_wrkStoreTotalBuy.GetStoreValue(_reserveStoreRID);
                                    }
                                    else
                                    {
                                        storeReserveValue = 0;
                                        storeTotalReserveValue = 0;
                                    }
                                    int packIncreaseBuy = 0;
                                    int sizesInErrorCount = (int)_wrkStoreSizesInErrorCount.GetStoreValue(storeRID);
                                    foreach (KeyValuePair<int, int> sizeRIDIdx in _sizeRIDIdxXref)
                                    {
                                        _sizeIDX = sizeRIDIdx.Value;
                                        _sizeRID = sizeRIDIdx.Key;
                                        storeBulkSizeValue = (int)_wrkStoreBulkSizeBuy[_sizeIDX].GetStoreValue(storeRID);
                                        storePackSizeValue = _packSizeContent[bestFitPackIDX, _sizeIDX];
                                        storeTotalSizeValue = (int)_wrkStoreTotalSizeBuy[_sizeIDX].GetStoreValue(storeRID);
                                        if (storePackSizeValue > storeBulkSizeValue)
                                        {
                                            sizeError = storePackSizeValue - storeBulkSizeValue;
                                            if (_depleteReserveSelected
                                                && _reserveStoreRID > -1)
                                            {
                                                storeReserveSizeValue = (int)_wrkStoreBulkSizeBuy[_sizeIDX].GetStoreValue(_reserveStoreRID);
                                                storeTotalReserveSizeValue = (int)_wrkStoreTotalSizeBuy[_sizeIDX].GetStoreValue(_reserveStoreRID);
                                                if (storeReserveSizeValue < sizeError)
                                                {
                                                    packIncreaseBuy +=
                                                        sizeError
                                                        - storeReserveSizeValue;
                                                    storeReserveValue -=
                                                        storeReserveSizeValue;
                                                    storeTotalReserveValue -=
                                                        storeReserveSizeValue;
                                                    storeTotalReserveSizeValue -=
                                                        storeReserveSizeValue;
                                                    storeReserveSizeValue = 0;
                                                }
                                                else
                                                {
                                                    storeReserveSizeValue -= sizeError;
                                                    storeReserveValue -= sizeError;
                                                    storeTotalReserveValue -= sizeError;
                                                    storeTotalReserveSizeValue -= sizeError;
                                                }
                                                _wrkStoreBulkSizeBuy[_sizeIDX].SetStoreValue(_reserveStoreRID, storeReserveSizeValue);
                                                _wrkStoreTotalSizeBuy[_sizeIDX].SetStoreValue(_reserveStoreRID, storeTotalReserveSizeValue);
                                            }
                                            else
                                            {
                                                packIncreaseBuy += sizeError;
                                            }
                                            storeSizeNeedError = (int)_wrkStoreSizeNeedErrors[_sizeIDX].GetStoreValue(storeRID);
                                            if (storeSizeNeedError == 0)
                                            {
                                                sizesInErrorCount++; 
                                            }
                                            storeSizeNeedError += sizeError;
                                            _wrkStoreSizeNeedErrors[_sizeIDX].SetStoreValue(storeRID, storeSizeNeedError);
                                            wrkStorePackBuy[bestFitPackIDX].TotalPackSizeError += sizeError;
                                            storeTotalError += sizeError;

                                            storeTotalSizeValue -= storeBulkSizeValue;
                                            storeTotalValue -= storeBulkSizeValue;
                                            storeBulkValue -= storeBulkSizeValue;
                                            storeBulkSizeValue = 0;
                                            storeTotalSizeValue += storePackSizeValue;
                                            storeTotalValue += storePackSizeValue;
                                        }
                                        else
                                        {
                                            storeBulkSizeValue -= storePackSizeValue;
                                            storeBulkValue -= storePackSizeValue;
                                        }
                                        _wrkStoreBulkSizeBuy[_sizeIDX].SetStoreValue(storeRID, storeBulkSizeValue);
                                        _wrkStoreTotalSizeBuy[_sizeIDX].SetStoreValue(storeRID, storeTotalSizeValue);
                                        if (_depleteReserveSelected
                                            && _reserveStoreRID > -1)
                                        {
                                            _wrkStoreBulkBuy.SetStoreValue(_reserveStoreRID, storeReserveValue);
                                            _wrkStoreTotalBuy.SetStoreValue(_reserveStoreRID, storeTotalReserveValue);
                                        }
                                    }
                                    _wrkStoreSizesInErrorCount.SetStoreValue(storeRID, sizesInErrorCount);
                                    _wrkStoreTotalError.SetStoreValue(storeRID, storeTotalError);
                                    _wrkStoreBulkBuy.SetStoreValue(storeRID, storeBulkValue);
                                    _wrkStoreTotalBuy.SetStoreValue(storeRID, storeTotalValue);
                               
                                    _wrkIncreaseBuyQty -= packIncreaseBuy;
                                    if (_wrkIncreaseBuyQty < 0)
                                    {
                                        throw new Exception("Logic Error: Increase Buy Quantity reduced to negative value");
                                    }
                                }
                            }
                        }
                    }
                }
                List<int> removePackIDX = new List<int>();
                for (int packIDX = 0; packIDX < _packCount; packIDX++)
                {
                    if (wrkStorePackBuy[packIDX] == null)
                    {
                        removePackIDX.Add(packIDX);
                    }
                    else if (wrkStorePackBuy[packIDX].AllStorePackTotalUnits < aCompMinOrder)
                    {
                        if (wrkStorePackBuy[packIDX].AllStorePackTotalUnits > 0)
                        {
                            continueFittingPacks = true;
                            _packAvailable[packIDX] = false;
                        }
                        aStatusReasonList.Add(
                            new MIDException(
                                eErrorLevel.information,
                                (int)eMIDTextCode.msg_al_PackUnitsLessThanMinimum,
                                string.Format(
                                    MIDText.GetTextOnly(eMIDTextCode.msg_al_PackUnitsLessThanMinimum),
                                    _buildPacksMethod.WorkUpSizeBuy.HeaderID,
                                    _buildPacksMethod.Name,
                                    _storePackBuy[packIDX].AllStorePackTotalUnits.ToString(CultureInfo.CurrentUICulture),
                                    this.OptionPackID,
                                    aCompMinOrder.ToString(CultureInfo.CurrentUICulture),
                                    aPackPatternList[packIDX].PackMultiple.ToString(CultureInfo.CurrentUICulture)))
                                    );
                        _messageLog.Add(aStatusReasonList[aStatusReasonList.Count - 1]);
                        removePackIDX.Add(packIDX);
                        status = false;
                    }
                    else if (wrkStorePackBuy[packIDX].AllStorePackTotalUnits == 0)
                    {
                        removePackIDX.Add(packIDX);
                    }
                }
                foreach (int packIDX in removePackIDX)
                {
                    wrkStorePackBuy.Remove(_storePackBuy[packIDX]);
                }
            }
            _storePackBuy = wrkStorePackBuy;
            _storeBulkBuy = _wrkStoreBulkBuy;
            _storeBulkSizeBuy = _wrkStoreBulkSizeBuy;
            _storeSizeNeedErrors = _wrkStoreSizeNeedErrors;
            _storeSizesInErrorCount = _wrkStoreSizesInErrorCount;
            _storeTotalBuy = _wrkStoreTotalBuy;
            _storeTotalSizeBuy = _wrkStoreTotalSizeBuy;
            _storeTotalError = _wrkStoreTotalError;
            return status;
        }
        private bool FindBestFitPack(int aStoreRID, out int aBestFitPackIDX)
        {
            int sizeIDX;
            int sizeRID;
            int sortIDX;
            int potentialCandidateCount = 0;
            int storeCandidateCount = 0;
            bool[] storePackCandidate = new bool[_packCount];
            double[] storePackGap = new double[_packCount];
            double[] storePackDeviationError = new double[_packCount];
            int[] storePackDeviationErrorCount = new int[_packCount];
            int[] storePackNeedError = new int[_packCount];
            double[] storePackAvgDeviationError = new double[_packCount];
            double[] storeDesiredAllocationCurve = new double[_sizeCount];
            int[] reserveSizeUnits = new int[_sizeCount];

            if (_reserveStoreRID > 0
                && _depleteReserveSelected)
            {
                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                {
                    sizeRID = keyValue.Key;
                    sizeIDX = keyValue.Value;
                    reserveSizeUnits[sizeIDX] = (int)_wrkStoreBulkSizeBuy[sizeIDX].GetStoreValue(_reserveStoreRID); 
                }
            }
            else
            {
                foreach (KeyValuePair<int, int> keyValue in _sizeRIDIdxXref)
                {
                    sizeIDX = keyValue.Value;
                    reserveSizeUnits[sizeIDX] = 0;
                }
            }
            foreach (KeyValuePair<int, int> sizeRIDIdx in _sizeRIDIdxXref)
            {
                sizeRID = sizeRIDIdx.Key;
                sizeIDX = sizeRIDIdx.Value;
                // NOTE:  we just want a reasonable percent to total to use for comparison purposes
                // to find the best fit packs.
                int storeBulkBuy = (int)_wrkStoreBulkBuy.GetStoreValue(aStoreRID);
                if (storeBulkBuy > 0)
                {
                    storeDesiredAllocationCurve[sizeIDX] =
                        _wrkStoreBulkSizeBuy[sizeIDX].GetStoreValue(aStoreRID)
                        * 100 / storeBulkBuy;
                }
                else
                {
                    storeDesiredAllocationCurve[sizeIDX] = 0;
                }
            } 
            for (int packIDX = 0; packIDX < _packCount; packIDX++)
            {
                storePackCandidate[packIDX] = false;
                if (_packAvailable[packIDX] == false)
                {
                }
                else
                {
                    storePackGap[packIDX] = -1;
                    storePackDeviationError[packIDX] = 0;
                    storePackDeviationErrorCount[packIDX] = 0;
                    storePackNeedError[packIDX] = 0;
                    int packIncreaseBuy = 0;
                    int packSizeNeedError;
                    foreach (KeyValuePair<int, int> sizeRIDIdx in _sizeRIDIdxXref)
                    {
                        sizeRID = sizeRIDIdx.Key;
                        sizeIDX = sizeRIDIdx.Value;
                        int storeBulkSizeValue = (int)_wrkStoreBulkSizeBuy[sizeIDX].GetStoreValue(aStoreRID);
                        if (_packSizeContent[packIDX, sizeIDX] > storeBulkSizeValue)
                        {
                            packSizeNeedError =
                                _packSizeContent[packIDX, sizeIDX]
                                - storeBulkSizeValue;
                            storePackNeedError[packIDX] +=
                                packSizeNeedError;
                            storePackDeviationErrorCount[packIDX]++;
                            if (packSizeNeedError > reserveSizeUnits[sizeIDX])
                            {
                                packIncreaseBuy +=
                                    packSizeNeedError
                                    - reserveSizeUnits[sizeIDX];
                            }
                        }
                    }
                    if (storePackDeviationErrorCount[packIDX] > 0)
                    {
                        storePackAvgDeviationError[packIDX] =
                            (double)storePackNeedError[packIDX]
                            / (double)storePackDeviationErrorCount[packIDX];
                    }
                    else
                    {
                        storePackAvgDeviationError[packIDX] = 0;
                    }
                    if (storePackAvgDeviationError[packIDX] <=
                        _avgPackDevTolerance
                        &&
                        (
                        (storePackNeedError[packIDX]
                        + _wrkStoreTotalError.GetStoreValue(aStoreRID)) <=
                        this.MaxPackNeedTolerance
                        ))
                    {
                        if (packIncreaseBuy > _wrkIncreaseBuyQty)
                        {
                        }
                        else
                        {
                            storePackCandidate[packIDX] = true;
                            potentialCandidateCount++;
                        }
                    }
                }
            }
            if (potentialCandidateCount > 0)
            {
                MIDGenericSortItem[] sortedPack = new MIDGenericSortItem[potentialCandidateCount];
                sortIDX = 0;
                for (int packIDX = 0; packIDX < _packCount; packIDX++)
                {
                    if (storePackCandidate[packIDX])
                    {
                        sortedPack[sortIDX].Item = packIDX;
                        sortedPack[sortIDX].SortKey = new double[2];
                        sortedPack[sortIDX].SortKey[0] = storePackNeedError[packIDX];
                        sortedPack[sortIDX].SortKey[1] = _appTran.GetRandomDouble();
                        sortIDX++;
                    }
                    storePackCandidate[packIDX] = false;
                }
                Array.Sort(sortedPack, new MIDGenericSortAscendingComparer());  // TT#1143  - MD - Jellis - Group ALlocation Min Broken
                _packIDX = sortedPack[0].Item;
                int targetPackNeedError = (int)storePackNeedError[_packIDX];
                double targetPackAvgDeviationError = storePackAvgDeviationError[_packIDX];
                for (sortIDX = 0; sortIDX < potentialCandidateCount; sortIDX++)
                {
                    _packIDX = sortedPack[sortIDX].Item;
                    if (storePackNeedError[_packIDX] == targetPackNeedError
                        && storePackAvgDeviationError[_packIDX] == targetPackAvgDeviationError)
                    {
                        storePackCandidate[_packIDX] = true;
                        storeCandidateCount++;
                    }
                    storePackGap[_packIDX] = 0;
                    for (sizeIDX = 0; sizeIDX < _sizeCount; sizeIDX++)
                    {
                        storePackGap[_packIDX] +=
                            Math.Abs
                            (
                            storeDesiredAllocationCurve[sizeIDX]
                            - _packSizeCurvePct[_packIDX, sizeIDX]
                            );
                    }
                }
                if (storeCandidateCount > 0)
                {
                    MIDGenericSortItem[] candidatePack = new MIDGenericSortItem[storeCandidateCount];
                    sortIDX = 0;
                    for (int packIDX = 0; packIDX < _packCount; packIDX++)
                    {
                        if (storePackCandidate[packIDX])
                        {
                            candidatePack[sortIDX].Item = packIDX;
                            candidatePack[sortIDX].SortKey = new double[4];
                            candidatePack[sortIDX].SortKey[0] = storePackNeedError[packIDX];
                            candidatePack[sortIDX].SortKey[1] = storePackGap[packIDX];
                            candidatePack[sortIDX].SortKey[2] = -_packMultiple[packIDX];  // descending order
                            candidatePack[sortIDX].SortKey[3] = _appTran.GetRandomDouble();
                            sortIDX++;
                        }
                    }
                    Array.Sort(candidatePack, new MIDGenericSortAscendingComparer());  // TT#1143 - MD - Jellis - Group ALlocation Min Broken
                    aBestFitPackIDX = candidatePack[0].Item;
                    return true;
                }
            }
            aBestFitPackIDX = -1;
            return false;
        }
        // end TT#744 - JEllis - Use Orig Pack Fitting Algorithm; Remove Bulk
        #endregion Apply Pack Pattern to Stores

        #region AdjustBulk
        /// <summary>
        /// Forces bulk size order to be in multiple of the BulkSizeMultiple as well as rejects bulk order if it is less than the minimum.
        /// </summary>
        /// <param name="aTrans">Application Session Transaction</param>
        /// <param name="aBulkMinOrder">Minimum order.</param> // TT#787 Vendor Min Order applies only to packs
        /// <param name="aBulkSizeMultiple">Desired size multiple for each bulk size</param>
        /// <param name="aStatusReason">Reason for failure.</param>
        /// <returns>True: adjustments were successful, False: adjustments failed or an information message was issued.</returns>
        internal bool AdjustBulkTotals(
            ApplicationSessionTransaction aTrans,
            int aBulkMinOrder,  // TT#787 Vendor Min Order applies only to packs
            int aBulkSizeMultiple, 
            out MIDException aStatusReason)
        {
            try
            {
                int removeBulkAfterPacksMsgCnt = 0;
                aStatusReason = null;
                if (aBulkSizeMultiple == 0)
                {
                    aStatusReason =
                        new MIDException(
                            eErrorLevel.information,
                            (int)eMIDTextCode.msg_al_SizeMultipleAssumedToBe1,
                            string.Format(
                                MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeMultipleAssumedToBe1),
                                _buildPacksMethod.Name,
                                _buildPacksMethod.WorkUpSizeBuy.HeaderID,
                                _optionPackID));
                    _messageLog.Add(aStatusReason);
                }
                double newSizeTotal;      // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                double oldSizeTotal;      // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                double oldStoreValue;     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                double newStoreValue;     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                int sizeIdx;
                int sizeRID;
                double reserveValue;      // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                double newReserveValue;   // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                double adjReserve;        // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk 
                foreach (StoreSizeVector ssv in _storeBulkSizeBuy)
                {
                    sizeRID = ssv.SizeCodeRID;
                    if (!GetSizeRIDIndex(sizeRID, out sizeIdx, out aStatusReason))
                    {
                        _messageLog.Add(aStatusReason);
                        if (aStatusReason.ErrorLevel != eErrorLevel.information)
                        {
                            return false;
                        }
                    }
                    oldSizeTotal = ssv.AllStoreTotalValue;
                    newSizeTotal =
                        (int)(((double)oldSizeTotal
                              / (double)aBulkSizeMultiple) + .5d);
                    newSizeTotal =
                        newSizeTotal
                        * aBulkSizeMultiple;
                    if (newSizeTotal != oldSizeTotal
                        && _reserveStoreRID > 0)
                    {
                        adjReserve = newSizeTotal - oldSizeTotal;
                        reserveValue = ssv.GetStoreValue(_reserveStoreRID);
                        newReserveValue = reserveValue + adjReserve;
                        if (newReserveValue < 0)
                        {
                            newReserveValue = 0;
                        }
                        ssv.SetStoreValue(_reserveStoreRID, newReserveValue);
                        _storeBulkBuy.SetStoreValue
                            (_reserveStoreRID,
                             _storeBulkBuy.GetStoreValue(_reserveStoreRID) - reserveValue + newReserveValue);
                        _storeTotalSizeBuy[sizeIdx].SetStoreValue
                            (_reserveStoreRID,
                             _storeTotalSizeBuy[sizeIdx].GetStoreValue(_reserveStoreRID) - reserveValue + newReserveValue);
                        _storeTotalBuy.SetStoreValue
                            (_reserveStoreRID,
                             _storeTotalBuy.GetStoreValue(_reserveStoreRID) - reserveValue + newReserveValue);
                    }
                    oldSizeTotal = ssv.AllStoreTotalValue;
                    if (oldSizeTotal != newSizeTotal)
                    {
                        MIDGenericSortItem[] sortStores = new MIDGenericSortItem[_maxStoreRID];
                        int storeRID = 1;
                        for (int i = 0; i < _maxStoreRID; i++)
                        {
                            sortStores[i].Item = storeRID;
                            sortStores[i].SortKey = new double[2];
                            sortStores[i].SortKey[0] = _storeBulkSizeBuy[sizeIdx].GetStoreValue(storeRID);
                            sortStores[i].SortKey[1] = aTrans.GetRandomDouble();
                            storeRID++;
                        }
                        Array.Sort(sortStores, new MIDGenericSortDescendingComparer());
                        for (int i = 0; i < _maxStoreRID; i++)
                        {
                            storeRID = sortStores[i].Item;
                            oldStoreValue = _storeBulkSizeBuy[sizeIdx].GetStoreValue(storeRID);
                            if (oldSizeTotal > 0)
                            {
                                newStoreValue =
                                    (int)(((double)oldStoreValue
                                    * (double)newSizeTotal
                                    / (double)oldSizeTotal) + .5d);
                                if (newStoreValue > newSizeTotal)
                                {
                                    newStoreValue = newSizeTotal;
                                }
                            }
                            else
                            {
                                newStoreValue = 0;
                            }
                            _storeBulkSizeBuy[sizeIdx].SetStoreValue(storeRID, newStoreValue);
                            _storeBulkBuy.SetStoreValue
                                (storeRID,
                                _storeBulkBuy.GetStoreValue(storeRID) - oldStoreValue + newStoreValue);
                            _storeTotalSizeBuy[sizeIdx].SetStoreValue
                                (storeRID,
                                _storeTotalSizeBuy[sizeIdx].GetStoreValue(storeRID) - oldStoreValue + newStoreValue);
                            _storeTotalBuy.SetStoreValue
                                (storeRID,
                                _storeTotalBuy.GetStoreValue(storeRID) - oldStoreValue + newStoreValue);
                            oldSizeTotal -= oldStoreValue;
                            newSizeTotal -= newStoreValue;
                        }
                    }
                }

                // begin TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                bool removeBulkSize = _buildPacksMethod.RemoveBulkAfterFittingPacks;
                if (removeBulkSize)
                {
                    if (removeBulkAfterPacksMsgCnt < 1)
                    {
                        aStatusReason =
                            new MIDException(
                                    eErrorLevel.information,
                                    (int)eMIDTextCode.msg_al_AllRemainingBulkSizeRemovedAfterPacksBuilt,
                                    string.Format(
                                        MIDText.GetTextOnly(eMIDTextCode.msg_al_AllRemainingBulkSizeRemovedAfterPacksBuilt),
                                        _buildPacksMethod.Name,
                                        _buildPacksMethod.WorkUpSizeBuy.HeaderID,
                                        _optionPackID));
                        _messageLog.Add(aStatusReason);
                    }
                    removeBulkAfterPacksMsgCnt++;
                }
                else if (_storeBulkBuy.AllStoreTotalValue < aBulkMinOrder)  // TT#849 - Move BP MID Dots enhancement from 3.2 to ver 4.0
                // end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                {
                    removeBulkSize = true; // TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                    aStatusReason =
                        new MIDException(
                            eErrorLevel.information,
                            (int)eMIDTextCode.msg_al_BulkOrderLessThanMinimum,
                            string.Format(
                                MIDText.GetTextOnly(eMIDTextCode.msg_al_BulkOrderLessThanMinimum),
                                _buildPacksMethod.Name,
                                _buildPacksMethod.WorkUpSizeBuy.HeaderID,
                                _optionPackID,
                                _storeBulkBuy.AllStoreTotalValue,
                                aBulkMinOrder)); // TT#787 Vendor Min Order applies only to packs
                    _messageLog.Add(aStatusReason);
                }
                if (removeBulkSize) // TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                {
                    foreach (StoreSizeVector ssv in _storeBulkSizeBuy)
                    {
                        sizeRID = ssv.SizeCodeRID;
                        if (_sizeRIDIdxXref.TryGetValue(sizeRID, out sizeIdx))
                        {
                            for (int storeRID = _maxStoreRID; storeRID > 0; storeRID--)
                            {
                                oldStoreValue = ssv.GetStoreValue(storeRID);
                                ssv.SetStoreValue(storeRID, 0);
                                _storeBulkBuy.SetStoreValue(
                                    storeRID,
                                    _storeBulkBuy.GetStoreValue(storeRID) - oldStoreValue);
                                _storeTotalBuy.SetStoreValue(
                                    storeRID,
                                    _storeTotalBuy.GetStoreValue(storeRID) - oldStoreValue);
                                _storeTotalSizeBuy[sizeIdx].SetStoreValue(
                                    storeRID,
                                    _storeTotalSizeBuy[sizeIdx].GetStoreValue(storeRID) - oldStoreValue);
                            }
                        }
                    }
                }
            }
            finally
            {
                ResetProperties(false);
            }
            if (aStatusReason == null)
            {
                return true;
            }
            return false;
        }
        #endregion AdjustBulk

        #region CalculatePackReserve
        /// <summary>
        /// Calculates the pack reserve AFTER the store pack allocation has been determined.
        /// Note:  Store Errors, pack errors, size need errors are NOT adjusted during this calculation
        /// </summary>
        /// <param name="aTrans">Application Session Transaction</param>
        /// <param name="aPackUnitsToPutInReserve">Pack units to put in reserve.</param>
        /// <param name="aStatusReason">A Status reason when the calculation fails.</param>
        /// <returns>True: Calculation was successful; False: Calculation failed, reason for failure in aStatusReason.</returns>
        internal bool CalculatePackReserve(ApplicationSessionTransaction aTrans, int aPackUnitsToPutInReserve, out MIDException aStatusReason)
        {
            // Reset properties
            _countAllStoresWithPacks = -1;
            _countNonReserveStoresWithPacks = -1;

            aStatusReason = null;
            if (_reserveStoreRID > 0)
            {
                int oldPackUnitTotal = 0; // NonReserve store total
                int newPackUnitTotal = 0; // NonReserve store total
                MIDGenericSortItem[] sortPacks = new MIDGenericSortItem[this._storePackBuy.Count];
                MIDGenericSortItem[] sortStores = new MIDGenericSortItem[_maxStoreRID];
                MIDGenericSortDescendingComparer midDescendComparer = new MIDGenericSortDescendingComparer();
                StorePackVector spv;
                for (int i = 0; i < _storePackBuy.Count; i++)
                {
                    sortPacks[i].Item = i;
                    spv = _storePackBuy[i];
                    sortPacks[i].SortKey = new double[2];
                    sortPacks[i].SortKey[0] = -spv.AllStorePackTotalUnits;  // want sizes in ascending sequence
                    sortPacks[i].SortKey[1] = aTrans.GetRandomDouble();
                    oldPackUnitTotal += spv.AllStorePackTotalUnits;
                }
                if (aPackUnitsToPutInReserve < oldPackUnitTotal)
                {
                    newPackUnitTotal = oldPackUnitTotal - aPackUnitsToPutInReserve;
                }
                else
                {
                    aStatusReason = new MIDException(
                         eErrorLevel.information,
                         (int)eMIDTextCode.msg_al_PackUnitsToReserveExceedsPackTotal,
                         string.Format(
                            MIDText.GetTextOnly(
                                eMIDTextCode.msg_al_PackUnitsToReserveExceedsPackTotal),
                                this._buildPacksMethod.Name,
                                this._buildPacksMethod.WorkUpSizeBuy.HeaderID,
                                this._optionPackID,
                                oldPackUnitTotal,
                                aPackUnitsToPutInReserve));
                    newPackUnitTotal = 0;
                }

                Array.Sort(sortPacks, midDescendComparer);
                int j = 0;
                int packIdx;
                int newPacksTotal;
                int storeRID;
                int newStoreTotal;
                int oldStoreTotal;
                int newStoreValue;
                int oldStoreValue;
                int reserveValue;
                int sizeIdx;
                for (int i = 0; i < sortPacks.Length; i++)
                {
                    packIdx = sortPacks[i].Item;
                    if (oldPackUnitTotal > 0)
                    {
                        newPacksTotal =
                            (int)(((double)_storePackBuy[packIdx].AllStorePackTotalUnits
                            * (double)newPackUnitTotal
                            / 100d) + .5d);
                        newPacksTotal =
                            (int)(((double)newPacksTotal
                                   / (double)_storePackBuy[packIdx].PackMultiple) + .5d);
                        newPacksTotal =
                            newPacksTotal
                            * _storePackBuy[packIdx].PackMultiple;
                        if (newPacksTotal > newPackUnitTotal)
                        {
                            newPacksTotal = newPackUnitTotal;
                        }
                    }
                    else
                    {
                        newPacksTotal = 0;
                    }
                    newPackUnitTotal =
                        newPackUnitTotal
                        - newPacksTotal;
                    oldPackUnitTotal =
                        oldPackUnitTotal
                        - _storePackBuy[packIdx].AllStorePackTotalUnits;
                    j = 0;
                    for (storeRID = _maxStoreRID; storeRID > 0; storeRID--)
                    {
                        sortStores[j].Item = storeRID;
                        sortStores[j].SortKey = new double[2];
                        sortStores[j].SortKey[0] = -(double)_storePackBuy[packIdx].GetStoreValue(storeRID);  // we want ascending sequence
                        sortStores[j].SortKey[1] = aTrans.GetRandomDouble();
                        j++;
                    }
                    Array.Sort(sortStores, midDescendComparer);
                    // at store level work with packs
                    newStoreTotal =
                        newPacksTotal
                        / _storePackBuy[packIdx].PackMultiple;
                    oldStoreTotal =
                        (int)_storePackBuy[packIdx].AllStoreTotalValue;     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    if (newStoreTotal < oldStoreTotal)
                    {
                        reserveValue =
                            oldStoreTotal
                            - newStoreTotal;
                    }
                    else
                    {
                        reserveValue = 0;
                    }

                    for (j = 0; j < sortStores.Length; j++)
                    {
                        storeRID = sortStores[j].Item;
                        oldStoreValue =
                            (int)_storePackBuy[packIdx].GetStoreValue(storeRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                        if (oldStoreTotal > 0)
                        {
                            newStoreValue =
                                (int)(((double)oldStoreValue
                                     * (double)newStoreTotal
                                     / (double)oldStoreTotal) + .5d);
                            if (newStoreValue > newStoreTotal)
                            {
                                newStoreValue = newStoreTotal;
                            }
                        }
                        else
                        {
                            newStoreValue = 0;
                        }
                        _storePackBuy[packIdx].SetStoreValue(storeRID, newStoreValue);
                        newStoreTotal -= newStoreValue;
                        oldStoreTotal -= oldStoreValue;
                        _storeTotalBuy.SetStoreValue(
                            storeRID,
                            _storeTotalBuy.GetStoreValue(storeRID) - (oldStoreValue - newStoreValue) * _storePackBuy[packIdx].PackMultiple);
                        foreach (SizeUnits su in _storePackBuy[packIdx].PackSizeUnits)
                        {
                            if (!GetSizeRIDIndex(su.RID, out sizeIdx, out aStatusReason))
                            {
                                if (aStatusReason.ErrorLevel != eErrorLevel.information)
                                {
                                    return false;
                                }
                            }
                            _storeTotalSizeBuy[sizeIdx].SetStoreValue(
                                storeRID,
                                _storeTotalSizeBuy[sizeIdx].GetStoreValue(storeRID) - (oldStoreValue - newStoreValue) * su.Units);
                        }
                    }
                    _storePackBuy[packIdx].SetStoreValue(_reserveStoreRID, reserveValue);
                    _storeTotalBuy.SetStoreValue(
                        _reserveStoreRID, 
                        _storeTotalBuy.GetStoreValue(_reserveStoreRID) + reserveValue * _storePackBuy[packIdx].PackMultiple);
                    foreach (SizeUnits su in _storePackBuy[packIdx].PackSizeUnits)
                    {
                        if (!GetSizeRIDIndex(su.RID, out sizeIdx, out aStatusReason))
                        {
                            if (aStatusReason.ErrorLevel != eErrorLevel.information)
                            {
                                return false;
                            }
                        }
                        // Begin TT#732 - JSmith - Build Pack on the Evaluation tab the total bulk units is not correct.
                        //_storeTotalSizeBuy[sizeIdx].SetStoreValue(
                        //    storeRID,
                        //    _storeTotalSizeBuy[sizeIdx].GetStoreValue(storeRID) + (reserveValue) * su.Units);
                        _storeTotalSizeBuy[sizeIdx].SetStoreValue(
                           _reserveStoreRID,
                           _storeTotalSizeBuy[sizeIdx].GetStoreValue(_reserveStoreRID) + (reserveValue) * su.Units);
                        // End TT#732
                    }
                }
            }
            else
            {
                // no reserve store
                string message =
                    MIDText.GetTextOnly((int)eMethodType.BuildPacks) + " [" + _buildPacksMethod.Name + "] "
                    + MIDText.GetTextOnly((int)eMIDTextCode.lbl_HeaderID) + " [" + _buildPacksMethod.WorkUpSizeBuy.HeaderID + "] "
                    + MIDText.GetTextOnly(eMIDTextCode.msg_al_NoReserveStore);
                aStatusReason =
                    new MIDException(
                        eErrorLevel.information,
                        (int)eMIDTextCode.msg_al_NoReserveStore,
                         message);
            }
            if (aStatusReason == null)
            {
                return true;
            }
            return false;
        }

        #endregion CalculatePackReserve

        #region SizeRIDIndex
        /// <summary>
        /// Gets the size index for a given size Code RID
        /// </summary>
        /// <param name="aSizeRID">Size RID</param>
        /// <param name="aSizeIdx">Size index</param>
        /// <param name="aStatusReason">When get fails, this gives a reason for the failure</param>
        /// <returns>True: if Get is successful; False if failure in which case aStatusReason contains the reason for failure</returns>
        private bool GetSizeRIDIndex(int aSizeRID, out int aSizeIdx, out MIDException aStatusReason)
        {
            aStatusReason = null;
            if (_sizeRIDIdxXref.TryGetValue(aSizeRID, out aSizeIdx))
            {
                return true;
            }
             aStatusReason =
                new MIDException(
                    eErrorLevel.severe,
                    (int)eMIDTextCode.msg_al_SizeRID_HasNoIndex,
                    string.Format(
                        MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeRID_HasNoIndex),
                        _buildPacksMethod.Name,
                        _buildPacksMethod.WorkUpSizeBuy.HeaderID,
                        _optionPackID,
                        aSizeRID));
            return false;
        }
        #endregion SizeRIDIndex
        // begin TT#580 build packs creates duplicate solutions
        public bool IsOptionPackSolutionEqual(OptionPackProfile aOptionPackProfile)
        {
            if (this.FromPackPatternComboName != aOptionPackProfile.FromPackPatternComboName)
            {
                return false;
            }
            if (this.ShipVariance != aOptionPackProfile.ShipVariance)
            {
                return false;
            }
            if (this.AvgPackDevTolerance != aOptionPackProfile.AvgPackDevTolerance)
            {
                return false;
            }
            int thatSizeIDX;
            MIDException statusReason;
            if (this._storeBulkSizeBuy.Length != aOptionPackProfile._storeBulkSizeBuy.Length)
            {
                return false;
            }
            foreach (StoreSizeVector ssv in this._storeBulkSizeBuy)
            {
                if (!aOptionPackProfile.GetSizeRIDIndex(ssv.SizeCodeRID, out thatSizeIDX, out statusReason))
                {
                    return false;
                }
                if (!ssv.Equals(aOptionPackProfile._storeBulkSizeBuy[thatSizeIDX]))
                {
                    return false;
                }
            }
            if (this._storePackBuy.Count != aOptionPackProfile._storePackBuy.Count)
            {
                return false;
            }
            List<StorePackVector> thatSpvL = new List<StorePackVector>();
            foreach (StorePackVector spv in aOptionPackProfile._storePackBuy)
            {
                thatSpvL.Add(spv);
            }
            bool isEqual;
            foreach (StorePackVector spv in this._storePackBuy)
            {
                isEqual = false;
                foreach (StorePackVector thatSpv in thatSpvL)
                {
                    if (spv.IsThisPackVectorContentEqualTo(thatSpv))
                    {
                        isEqual = true;
                        thatSpvL.Remove(thatSpv);
                        break;
                    }
                }
                if (!isEqual)
                {
                    return false;
                }
            }
            return true;
        }
        // end TT#580 build packs creates duplicate solutions
        #endregion Methods
    }
    #endregion OptionPackProfile
}
