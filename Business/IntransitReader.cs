using System;
using System.Collections;
using System.Collections.Generic; // TT#4345 -
using System.Data;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
    #region HorizonID
    // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    [Serializable]
    public struct Horizon_ID
    {
        private int _hashCode;
        private int _beginDay;
        private int _shipDay;
        /// <summary>
        /// Uniquely identifies the initial build criteria for StoreSalesITHorizon
        /// </summary>
        /// <param name="aBeginDay">Begin Day for the Sales/Intransit Horizon</param>
        /// <param name="aShipDayBasisNodeRID">For dynamic horizons, the Merchandise Node RID used to determine Store Pick Days and Lead Times; otherwise, use negative 1 (Include.NoRID) because ship day will be static and same for all stores</param>
        /// <param name="aShipDay">For dynamic horizons, use Undefined date (Include.UndefinedDate); otherwise, set to the Ship Day shared by all stores.</param>
        public Horizon_ID(DateTime aBeginDay, DateTime aShipDay)
        {
            if (aBeginDay == Include.UndefinedDate)
            {
                throw
                    new ArgumentException(MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate), "aBeginDay");
            }
            _beginDay =
                aBeginDay.Year * 1000
                + aBeginDay.DayOfYear;
            _shipDay =
                aShipDay.Year * 1000
                + aShipDay.DayOfYear;
            Object[] hashObject = { _beginDay, _shipDay };
            _hashCode = MID_HashCode.CalculateHashCode(hashObject);
        }
        /// <summary>
        /// Gets the Begin Day associated with this Horizon (Format is YYYYDDD)
        /// </summary>
        public int BeginDayID
        {
            get { return _beginDay; }
        }
        /// <summary>
        /// Gets the ShipDay associcated this this Horizon in format YYYYDDD (when ShipDayBasisNodeRID is greater than 0, the ship day will not be a valid date). 
        /// </summary>
        public int ShipDay
        {
            get { return _shipDay; }
        }
        /// <summary>
        /// True: "object" is equal to this structure; False: "object" is not equal to this structure
        /// </summary>
        /// <param name="obj">Object to compare to this object</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetHashCode() != GetHashCode())
            {
                return false;
            }
            if (obj.GetType() != typeof(Horizon_ID))
            {
                return false;
            }
            Horizon_ID horizonID = (Horizon_ID)obj;
            if (_beginDay != horizonID.BeginDayID
                || _shipDay != horizonID.ShipDay)
            {
                return false;
            }
            return true;
        }
        public static bool operator ==(Horizon_ID rHorizon, Horizon_ID lHorizon)
        {
            // Null check 
            if (Object.ReferenceEquals(rHorizon, null))
            {
                if (Object.ReferenceEquals(lHorizon, null))
                {
                    // Both are null.  They do equal each other 
                    return true;
                }

                // Only 1 is null the other is not so they do not equal 
                return false;
            }
            if (Object.ReferenceEquals(lHorizon, null))
            {
                return false;
            }
            return rHorizon.Equals(lHorizon);
        }

        public static bool operator !=(Horizon_ID rHorizon, Horizon_ID lHorizon)
        {
            // Null check 

            return !(rHorizon == lHorizon);
        }
        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
    // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    #endregion HorizonID

    #region StoreSalesITHorizon
    // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    // moved from 
    [Serializable]
    public struct StoreSalesITHorizon
    {
        private int _hashcode;
        private ApplicationSessionTransaction _appTransaction;
        private Horizon_ID _horizon_ID; // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
        private bool _allDatesSame; // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
        private int[] _storeRIDList;
        private int[] _horizonStart;
        private int[] _horizonEnd;
        private long[] _storeIntransitDays;
        /// <summary>
        /// Instantiates same Horizon for every store.
        /// </summary>
        /// <param name="aStoreRIDList">Store RID list</param>
        /// <param name="aHorizonStart">Horizon Start Date</param>
        /// <param name="aHorizonEnd">Horizon End Date</param>       
        public StoreSalesITHorizon(ApplicationSessionTransaction aTransaction, DateTime aHorizonStart, DateTime aHorizonEnd)
        {
            if (aHorizonStart == Include.UndefinedDate)
            {
                throw new ArgumentException(MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate), "aHorizonStart");
            }
            if (aHorizonEnd == Include.UndefinedDate)
            {
                throw new ArgumentException(MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate), "aHorizonEnd");
            }
            _appTransaction = aTransaction;
            _storeRIDList = aTransaction.AllStoreRIDList();
            _horizonStart = new int[_storeRIDList.Length];
            _horizonEnd = new int[_storeRIDList.Length];
            _storeIntransitDays = new long[_storeRIDList.Length];
            int horizonStart = aHorizonStart.Year * 1000 + aHorizonStart.DayOfYear;
            DateTime horizonEndIT = aHorizonEnd.AddDays(-1);
            int horizonEnd = (horizonEndIT.Year * 1000 + horizonEndIT.DayOfYear);
            _horizon_ID = new Horizon_ID(aHorizonStart, aHorizonEnd);
            _hashcode = _horizon_ID.GetHashCode();
            _allDatesSame = true;
            for (int i = 0; i < aTransaction.AllStoreRIDList().Length; i++)
            {
                SetStoreHorizon(i, _storeRIDList[i], horizonStart, horizonEnd);
            }
        }
        /// <summary>
        /// Instantiates different horizons for each store
        /// </summary>
        /// <param name="aHorizon_ID"></param>
        /// <param name="aStoreRIDList"></param>
        /// <param name="aHorizonStart"></param>
        /// <param name="aHorizonEnd"></param>
        public StoreSalesITHorizon(ApplicationSessionTransaction aTransaction, Horizon_ID aHorizon_ID, int[] aHorizonStart, int[] aHorizonEnd)  
        {
            _appTransaction = aTransaction;
            _storeRIDList = aTransaction.AllStoreRIDList();
            if (_storeRIDList.Length != aHorizonStart.Length)
            {
                throw new ArgumentException(MIDText.GetTextOnly(eMIDTextCode.msg_al_StoreDateListsOutOfSync, "aHorizonStart"));
            }
            if (aHorizonStart.Length != aHorizonEnd.Length)
            {
                throw new ArgumentException(MIDText.GetTextOnly(eMIDTextCode.msg_al_StoreDateListsOutOfSync, "aHorizonEnd"));
            }
            _horizonStart = new int[_storeRIDList.Length];
            _horizonEnd = new int[_storeRIDList.Length];
            _storeIntransitDays = new long[_storeRIDList.Length];
            _horizon_ID = aHorizon_ID;
            _hashcode = _horizon_ID.GetHashCode();
            _allDatesSame = true;
            int j = 0;
            for (int i = 0; i < _storeRIDList.Length; i++)
            {
                SetStoreHorizon(i, _storeRIDList[i], aHorizonStart[i], aHorizonEnd[i]);
                if (aHorizonStart[j] != aHorizonStart[i]
                    || aHorizonEnd[j] != aHorizonEnd[i])
                {
                    _allDatesSame = false;
                }
                j = i;
            }
        }

        private void SetStoreHorizon(int aStoreIndex, int aStoreRID, int aHorizonStartDay, int aHorizonEndDay)
        {
            _storeRIDList[aStoreIndex] = aStoreRID;
            _horizonStart[aStoreIndex] = aHorizonStartDay;
            _horizonEnd[aStoreIndex] = aHorizonEndDay;
            _storeIntransitDays[aStoreIndex] = aHorizonStartDay;
            _storeIntransitDays[aStoreIndex] = _storeIntransitDays[aStoreIndex] << 32;
            _storeIntransitDays[aStoreIndex] = _storeIntransitDays[aStoreIndex] + aHorizonEndDay;
            if (aHorizonEndDay != _horizon_ID.ShipDay)
            {
                object[] hash = {_hashcode, aHorizonEndDay};
                _hashcode = MID_HashCode.CalculateHashCode(hash);
            }
        }
        /// <summary>
        /// Gets ID associated with this StoreSalesITHorizon
        /// </summary>
        public Horizon_ID HorizonID
        {
            get { return _horizon_ID; }
        }
        public int[] StoreRIDList
        {
            get { return _storeRIDList; }
        }
        public int[] StoreHorizonStart
        {
            get { return _horizonStart; }
        }
        public int[] StoreHorizonEnd
        {
            get { return _horizonEnd; }
        }
        public long[] StoreIntransitDays
        {
            get { return _storeIntransitDays;  }
        }

        /// <summary>
        /// True: "object" is equal to this structure; False: "object" is not equal to this structure
        /// </summary>
        /// <param name="obj">Object to compare to this object</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetHashCode() != GetHashCode())
            {
                return false;
            }
            if (obj.GetType() != typeof(StoreSalesITHorizon))
            {
                return false;
            }
            StoreSalesITHorizon salesHorizon = (StoreSalesITHorizon)obj;
            if (HorizonID != salesHorizon.HorizonID)
            {
                return false;
            }
            if (StoreIntransitDays.Length != salesHorizon.StoreIntransitDays.Length)
            {
                return false;
            }

            if (_allDatesSame)
            {
                if (salesHorizon._allDatesSame)
                {
                    return (StoreIntransitDays[0] == salesHorizon.StoreIntransitDays[0]);
                }
                return false;
            }
            for (int i = 0; i < StoreIntransitDays.Length; i++)
            {
                if (StoreIntransitDays[i] != salesHorizon.StoreIntransitDays[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool operator ==(StoreSalesITHorizon rHorizon, StoreSalesITHorizon lHorizon)
        {
            // Null check 
            if (Object.ReferenceEquals(rHorizon, null))
            {
                if (Object.ReferenceEquals(lHorizon, null))
                {
                    // Both are null.  They do equal each other 
                    return true;
                }

                // Only 1 is null the other is not so they do not equal 
                return false;
            }
            if (Object.ReferenceEquals(lHorizon, null))
            {
                return false;
            }
            return rHorizon.Equals(lHorizon);
        }

        public static bool operator !=(StoreSalesITHorizon rHorizon, StoreSalesITHorizon lHorizon)
        {
            // Null check 

            return !(rHorizon == lHorizon);
        }
        public override int GetHashCode()
        {
            return _hashcode;
        }

    }
    // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    #endregion StoreSalesITHorizon

    // begin TT#1586 - MD - Jellis - Size Intransit Invalid
    public class IntransitStoreValueCache:Dictionary<long, int[]>
    { }
    public class HorizonIntransitCache:Dictionary<Horizon_ID, IntransitStoreValueCache>
    { }
    // end TT#1586 - MD - Jellis - Size Intransit Invalid

    #region IntransitReader
    // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //
    //   Added Horizon_ID to signature of most methods in this object
    //
    /// <summary>
	/// Reader for intransit
	/// </summary>
	[Serializable()]
	public class IntransitReader:Profile
	{
		//========
		// FIELDS 
		//========
		ApplicationSessionTransaction _transaction;
		ProfileList _allStoreList;
		Data.Intransit _dbIntransit;
		DateTime _flashBack;
        Dictionary<int, Dictionary<Horizon_ID, long[]>> _storeIntransitDayCache;  
        Dictionary<int, HorizonIntransitCache> _storeIntransitValueCache; // TT#1586 - MD - Jellis - Size Intransit Invalid
		Hashtable _storeIntransitWeekHash;
		int _lastPlanLevelRID;
        StoreSalesITHorizon _lastSalesHorizon; 
		long _lastIntransitTypeKey;
		int[] _lastStoreIntransitArray;

		//==============
		// CONSTRUCTORS
		//==============
		public IntransitReader(ApplicationSessionTransaction aTransaction):base(-1)
		{
			_transaction = aTransaction;
			_allStoreList = _transaction.GetMasterProfileList(eProfileType.Store);
			_flashBack = Include.UndefinedDate;
            _storeIntransitDayCache = new Dictionary<int, Dictionary<Horizon_ID, long[]>>();
            _storeIntransitValueCache = new Dictionary<int, HorizonIntransitCache>(); // TT#1586 - MD - Jellis - Size Intransit Invalid
			_storeIntransitWeekHash = new Hashtable();
			_dbIntransit = new Data.Intransit();
			_lastPlanLevelRID = Include.NoRID;
            _lastSalesHorizon = new StoreSalesITHorizon(); 
			_lastIntransitTypeKey = -1;
			_lastStoreIntransitArray = null;
		}

		//============
		// PROPERTIES
		//============
		/// <summary>
		/// Abstract.  Gets the ProfileType of the Profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Intransit;
			}
		}



		//=========
		// METHODS
		//=========
		/// <summary>
		/// Resets intransit flash back identifier to trigger a re-read of intransit.  Does not destroy store begin-end dates.
		/// </summary>
		public void ResetIntransitReader()
		{
			_flashBack = Include.UndefinedDate;
            _storeIntransitValueCache.Clear(); 
            _storeIntransitDayCache.Clear();   
            _storeIntransitWeekHash.Clear();
			this._lastIntransitTypeKey = -1;
			this._lastPlanLevelRID = Include.NoRID;
            _lastSalesHorizon = new StoreSalesITHorizon();  
            this._lastStoreIntransitArray = null;
		}


 		/// <summary>
		/// Gets the store array containing intransit values.
		/// </summary>
		/// <param name="aPlanLevelRID">Plan level RID</param>
		/// <param name="ikt">Intransit Key Type describing the desired intransit type</param>
		/// <returns>Store Array containing the plan level's store intransit values for the given key type</returns>
		// begin MID Track 4341 Performance Issues
		//private int[] GetStoreIntransitArray(int aPlanLevelRID, IntransitKeyType ikt)
        // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
        internal int[] GetStoreIntransitArray(int aPlanLevelRID, StoreSalesITHorizon aStoreSalesITHorizon, IntransitKeyType ikt)
        // end MID Track 4341 Performance Issues
        {
            bool loadIntransit = false;
            if (aPlanLevelRID != _lastPlanLevelRID
                || ikt.IntransitTypeKey != _lastIntransitTypeKey)
            {
                _lastPlanLevelRID = aPlanLevelRID;
                _lastIntransitTypeKey = ikt.IntransitTypeKey;
                _lastSalesHorizon = aStoreSalesITHorizon;
                loadIntransit = true;
            }
            else if (aStoreSalesITHorizon != _lastSalesHorizon)
            {
                _lastSalesHorizon = aStoreSalesITHorizon;
                loadIntransit = true;
            }
            if (loadIntransit)
            {
                HorizonIntransitCache storeHorizonIntransitValues; // TT#1586 - MD - Jellis - Size Intransit Invalid
                if (!_storeIntransitValueCache.TryGetValue(aPlanLevelRID, out storeHorizonIntransitValues))
                {
                    storeHorizonIntransitValues = new HorizonIntransitCache(); // TT#1586 - MD - Jellis - Size Intransit Invalid
                    _storeIntransitValueCache.Add(aPlanLevelRID, storeHorizonIntransitValues);
                }
                IntransitStoreValueCache intransitStoreValueCache; // TT#1586 - MD - Jellis - Size Intransit Invalid
                if (!storeHorizonIntransitValues.TryGetValue(aStoreSalesITHorizon.HorizonID, out intransitStoreValueCache)) // TT#1586 - MD - Jellis - Size Intransit Invalid
                {
                     // begin TT#1586 - MD - Jellis - Size Intransit Invalid 
                    intransitStoreValueCache = new IntransitStoreValueCache();
                    storeHorizonIntransitValues.Add(aStoreSalesITHorizon.HorizonID, intransitStoreValueCache);
                }
                if (!intransitStoreValueCache.TryGetValue(ikt.IntransitTypeKey, out _lastStoreIntransitArray))
                {
                     // end TT#1586 - MD - Jellis - Size Intransit Invalid
                    _lastStoreIntransitArray = this.LoadStoreIntransit(aPlanLevelRID, aStoreSalesITHorizon, ikt);
                    intransitStoreValueCache.Add(ikt.IntransitTypeKey, _lastStoreIntransitArray); // TT#1586 - MD - Jellis - Size Intransit Invalid
                }
            }
            return _lastStoreIntransitArray;
        }

		/// <summary>
		/// Gets the given store's intransit value for the given plan level and intransit type.
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <param name="aPlanLevelRID">RID of the Plan Level</param>
		/// <param name="aIntransitType">Identifies the Intransit key type</param>
		/// <returns>Intransit value for the store in the given plan level and intransit type</returns>
        public int GetStoreIntransit(int aStoreRID, int aPlanLevelRID, StoreSalesITHorizon aStoreSalesITHorizon, IntransitKeyType aIntransitType)
		{
            return GetStoreIntransit(_transaction.GetStoreIndexRID(aStoreRID), aPlanLevelRID, aStoreSalesITHorizon, aIntransitType);
		}

		/// <summary>
		/// Gets the given store's intransit value for the given plan level and intransit type.
		/// </summary>
		/// <param name="aStore">Index_RID of the store</param>
		/// <param name="aPlanLevelRID">RID of the Plan Level</param>
		/// <param name="aIntransitType">Identifies the Intransit key type</param>
		/// <returns>Intransit value for the store in the given plan level and intransit type</returns>
        public int GetStoreIntransit(Index_RID aStore, int aPlanLevelRID, StoreSalesITHorizon aStoreSalesITHorizon, IntransitKeyType aIntransitType)
		{
            int[] storeIntransitArray = GetStoreIntransitArray(aPlanLevelRID, aStoreSalesITHorizon, aIntransitType);
			return storeIntransitArray[aStore.Index];
		}

		/// <summary>
		/// Gets the given store's intransit value for the given plan level and intransit types.
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <param name="aPlanLevelRID">RID of the Plan Level</param>
		/// <param name="aIntransitType">Array that identifies the Intransit key types to be added together for intransit</param>
		/// <returns>Intransit value for the store in the given plan level and intransit types</returns>
        public int GetStoreIntransit(int aStoreRID, int aPlanLevelRID, StoreSalesITHorizon aStoreSalesITHorizon, IntransitKeyType[] aIntransitType)
		{
			int storeIntransitValue = 0;
			Index_RID storeIndexRID = _transaction.GetStoreIndexRID(aStoreRID);
			foreach (IntransitKeyType ikt in aIntransitType)
			{
                storeIntransitValue += GetStoreIntransit(storeIndexRID, aPlanLevelRID, aStoreSalesITHorizon, ikt);
			}
			return storeIntransitValue;
		}

		/// <summary>
		/// Loads intransit values from the database.
		/// </summary>
		/// <param name="aPlanLevelRID">Plan Level RID</param>
		/// <param name="aIntransitType">Intransit Key Type</param>
		/// <param name="aStoreIntransitDays">Array of "longs" representing store Intransit From/to ranges (left integer portion of long is "from" date; right integer portion long is "to" date) </param>
		/// <returns>Intransit values for all stores in "_allStoreList" array sequence</returns>
		private int[] LoadStoreIntransit(int aPlanLevelRID, StoreSalesITHorizon aStoreSalesITHorizon, IntransitKeyType aIntransitType)
        {
            Dictionary<Horizon_ID, long[]> storeHorizonITDays;
            if(!_storeIntransitDayCache.TryGetValue(aPlanLevelRID, out storeHorizonITDays))
            {
                storeHorizonITDays = new Dictionary<Horizon_ID,long[]>();
                _storeIntransitDayCache.Add(aPlanLevelRID, storeHorizonITDays);
            }
            storeHorizonITDays.Remove(aStoreSalesITHorizon.HorizonID);
            storeHorizonITDays.Add(aStoreSalesITHorizon.HorizonID, aStoreSalesITHorizon.StoreIntransitDays);
            return LoadStoreIntransit(aPlanLevelRID, aIntransitType, aStoreSalesITHorizon.StoreIntransitDays);
        }
        private int[] LoadStoreIntransit(int aPlanLevelRID, IntransitKeyType aIntransitType, long[] aStoreIntransitDays)
		{
			IntransitReadRequest irr = new IntransitReadRequest();
			irr.FlashBack = this._flashBack;
			irr.SetStoreIntransitFromTo(this._transaction.GetStoreRID_CharArray_List(), aStoreIntransitDays);
			HierarchyNodeProfile hnp = null;
			hnp = _transaction.GetNodeData(aPlanLevelRID);
			switch (aIntransitType.IntransitType)
			{
				case(eIntransitBy.Total):
				{
					if (hnp.LevelType == eHierarchyLevelType.Style ||
						hnp.LevelType == eHierarchyLevelType.Color ||
						hnp.LevelType == eHierarchyLevelType.Size ||
						hnp.IsParentOfStyle)
					{
						// node is at style or "parent of style" level, so don't have to worry about descendants
						irr.IntransitHnRID = hnp.Key;
					}
					else
					{
						ArrayList alTotal = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.ParentOfStyle);
						foreach(IntransitReadNodeProfile irnpTotal in alTotal)
						{
							irr.IntransitHnRID = irnpTotal.Key;
						}
					}
					break;
				}
				case (eIntransitBy.Color):
				{
					if (hnp.LevelType == eHierarchyLevelType.Color)
					{
						// node is at "color", so don't have to worry about descendants
						irr.IntransitHnRID = hnp.Key;
					}
					else if (hnp.LevelType == eHierarchyLevelType.Size)
					{
						// node is at "color", get color parent
						irr.IntransitHnRID = (int)hnp.Parents[0];
					}
					else
					{
						ArrayList alColor = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Color);
						foreach(IntransitReadNodeProfile irnpColor in alColor)
						{
							if (irnpColor.LevelType == eHierarchyLevelMasterType.Color
								|| irnpColor.LevelType == eHierarchyLevelMasterType.Size)
							{
								if (irnpColor.ColorCodeRID == aIntransitType.ColorRID)
								{
									// color match -- add to this bucket, for store list
									irr.IntransitHnRID = irnpColor.Key;
								}
								else if (aIntransitType.ColorRID == Include.DummyColorRID
									&& !irnpColor.StyleDefinedInHierarchy)
								{
									// handle alternate hierarchy where style of color not in alternate but color is
									irr.IntransitHnRID = irnpColor.Key;
								}
							}
						}
					}
					break;
				}
				case (eIntransitBy.SizeWithinColors):
				case (eIntransitBy.Size):
				{
					if (hnp.LevelType == eHierarchyLevelType.Size)
					{
						// node is at "color", get color parent
						irr.IntransitHnRID = hnp.Key;
					}
					else
					{
						ArrayList alSize = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Size);
						foreach(IntransitReadNodeProfile irnpSize in alSize)
						{
							if (irnpSize.LevelType == eHierarchyLevelMasterType.Size)
							{
								if (irnpSize.ColorCodeRID == aIntransitType.ColorRID)
								{
									// color match
									if (irnpSize.SizeCodeRID == aIntransitType.SizeRID)
									{
										// size match -- add to this bucket, for all store list
										irr.IntransitHnRID = irnpSize.Key;
									}
								}
								else if (!irnpSize.StyleDefinedInHierarchy
									&& aIntransitType.ColorRID == Include.DummyColorRID
									&& irnpSize.SizeCodeRID == aIntransitType.SizeRID)   
								{
									// Handle alternate hierarchy where style of size not in alternate but size is
									irr.IntransitHnRID = irnpSize.Key;
								}
							}
						}
					} 
					break;
				}
				default:
				{
					break;
				}
			}
    		StoreIntransit si = _dbIntransit.GetIt(irr, this._transaction.AllStoreRIDList());
			this._flashBack = si.FlashBack;
			int[] storeIntransit = new int[this._allStoreList.Count];
			storeIntransit.Initialize();
			foreach (StoreIntransitValue siv in si.StoreIntransitValues)
			{
				storeIntransit[this._transaction.GetStoreIndexRID(siv.StoreRID).Index] = siv.StoreIntransit;
			}
			return storeIntransit;
		}

		/// <summary>
		/// Gets Store Total Week Intransit (adds up the daily intransit for the days in the week)
		/// </summary>
		/// <param name="aWeekProfile">Week Profile of the week</param>
		/// <param name="aHnRID">Hierarchy node RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns></returns>
		public double GetStoreTotalWeekIntransit(
			WeekProfile aWeekProfile,
			int aHnRID,
			int aStoreRID)
		{
			Index_RID storeIndexRID = this._transaction.GetStoreIndexRID(aStoreRID);
			Hashtable storePlanIntransitWeekHash;
			storePlanIntransitWeekHash = (Hashtable)this._storeIntransitWeekHash[aHnRID];
			if (storePlanIntransitWeekHash == null)
			{
				storePlanIntransitWeekHash = new Hashtable();
				this._storeIntransitWeekHash.Add(aHnRID, storePlanIntransitWeekHash);
			}
			DateTime endDay = (aWeekProfile.ProfileStartDate.Date.AddDays(aWeekProfile.DaysInWeek - 1));
			long weekKey = (long)((aWeekProfile.ProfileStartDate.Year)*1000 + aWeekProfile.ProfileStartDate.DayOfYear);
			weekKey = (weekKey << 32);
			weekKey += (long)(endDay.Year*1000 + endDay.DayOfYear);
			int[] storeIntransitValue;
			storeIntransitValue = (int[]) storePlanIntransitWeekHash[weekKey];
			if (storeIntransitValue == null)
				// end MID Track 4341 Performance Issues
			{
				long[] weekKeys = new long[this._allStoreList.Count];
				for (int i=0; i<this._allStoreList.Count; i++)
				{
					weekKeys[i] = weekKey;
				}
				storeIntransitValue = LoadStoreIntransit(aHnRID, new IntransitKeyType(0,0), weekKeys);
				storePlanIntransitWeekHash.Add(weekKey, storeIntransitValue);
			}
			return storeIntransitValue[storeIndexRID.Index];
		}
	}
    #endregion IntransitReader
}
    #region Obsolete Code
//    /// <summary>
//    /// Reader for intransit
//    /// </summary>
//    [Serializable()]
//    public class IntransitReader:Profile
//    {
//        //========
//        // FIELDS 
//        //========
//        ApplicationSessionTransaction _transaction;
//        ProfileList _allStoreList;
//        Data.Intransit _dbIntransit;
//        DateTime _flashBack;
//        Hashtable _storeIntransitDayHash;
//        Hashtable _storeIntransitValueHash;
//        Hashtable _storeIntransitWeekHash;
//        int _lastPlanLevelRID;
//        long _lastIntransitTypeKey;
//        int[] _lastStoreIntransitArray;

//        //==============
//        // CONSTRUCTORS
//        //==============
//        public IntransitReader(ApplicationSessionTransaction aTransaction):base(-1)
//        {
//            _transaction = aTransaction;
//            _allStoreList = _transaction.GetMasterProfileList(eProfileType.Store);
//            _flashBack = Include.UndefinedDate;
//            _storeIntransitDayHash = new Hashtable();
//            _storeIntransitValueHash = new Hashtable();
//            _storeIntransitWeekHash = new Hashtable();
//            _dbIntransit = new Data.Intransit();
//            _lastPlanLevelRID = Include.NoRID;
//            _lastIntransitTypeKey = -1;
//            _lastStoreIntransitArray = null;
//        }

//        //============
//        // PROPERTIES
//        //============
//        /// <summary>
//        /// Abstract.  Gets the ProfileType of the Profile.
//        /// </summary>

//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.Intransit;
//            }
//        }



//        //=========
//        // METHODS
//        //=========
//        /// <summary>
//        /// Resets intransit flash back identifier to trigger a re-read of intransit.  Does not destroy store begin-end dates.
//        /// </summary>
//        public void ResetIntransitReader()
//        {
//            _flashBack = Include.UndefinedDate;
//            _storeIntransitValueHash.Clear();
//            _storeIntransitDayHash.Clear();  // TT#3019 - Jellis - AnF Workflow VSW Results different from step by step
//            _storeIntransitWeekHash.Clear();
//            this._lastIntransitTypeKey = -1;
//            this._lastPlanLevelRID = Include.NoRID;
//            this._lastStoreIntransitArray = null;
//        }

//        // BEGIN MID Change j.ellis added method to check if a day range is set
//        /// <summary>
//        /// Determines if the day range for intransit must be set
//        /// </summary>
//        /// <param name="aPlanLevelRID">Plan level for which intransit is desirred</param>
//        /// <returns>True: Day range must be set before reading intransit; False: day range already set</returns>
//        public bool SetDayRangeForRID(int aPlanLevelRID)
//        {
//            if (_storeIntransitDayHash.Contains(aPlanLevelRID))
//            {
//                return false;
//            }
//            return true;
//        }
//        // END MID Change

//        /// <summary>
//        /// Gets the Intransit Store begin and end days for the given plan level.
//        /// </summary>
//        /// <param name="aPlanLevelRID">RID of the PlanLevel</param>
//        /// <returns>Array of longs where bits 0-31 represent the end day and bits 32-63 represent the begin day.</returns>
//        private long[] GetPlanLevelIntransitStoreDays(int aPlanLevelRID)
//        {
//            long[] planLevelIntransitStoreDays;
//            if (_storeIntransitDayHash.Contains(aPlanLevelRID))
//            {
//                return (long[])_storeIntransitDayHash[aPlanLevelRID];
//            }
//            planLevelIntransitStoreDays = new long[this._allStoreList.Count];
//            planLevelIntransitStoreDays.Initialize();
//            _storeIntransitDayHash.Add(aPlanLevelRID, planLevelIntransitStoreDays);
//            return planLevelIntransitStoreDays;
//        }
        
//        /// <summary>
//        /// Sets the store intransit day range for "all" stores in the given planlevel.
//        /// </summary>
//        /// <param name="aPlanLevelRID">RID of the Plan Level</param>
//        /// <param name="aBeginDay">Begin day in format yyyyddd</param>
//        /// <param name="aEndDay">End day in format yyyyddd</param>
//        public void SetStoreIT_DayRange(int aPlanLevelRID, int aBeginDay, int aEndDay)
//        {
//            long[] planLevelIntransitStoreDays = GetPlanLevelIntransitStoreDays(aPlanLevelRID);
//            SetStoreIT_DayRange(aPlanLevelRID, planLevelIntransitStoreDays, aBeginDay, aEndDay);
//        }

//        /// <summary>
//        /// Sets the store intransit day range for "all" stores in the given plan level.
//        /// </summary>
//        /// <param name="aPlanLevelRID">RID of the plan level</param>
//        /// <param name="aPlanLevelIntransitStoreDays">Array of longs identifying the begin (bits 32-63) and end days (bits 0-31) for all stores.</param>
//        /// <param name="aBeginDay">Begin day in format yyyyddd</param>
//        /// <param name="aEndDay">End day in format yyyyddd</param>
//        private void SetStoreIT_DayRange(int aPlanLevelRID, long[] aPlanLevelIntransitStoreDays, int aBeginDay, int aEndDay)
//        {
//            foreach (StoreProfile sp in this._allStoreList)
//            {
//                SetStoreIT_DayRange(sp.Key, aPlanLevelRID, aPlanLevelIntransitStoreDays, aBeginDay, aEndDay);
//            }
//            // begin MID Track 4341 Performance Issues
//            this._storeIntransitValueHash.Remove(aPlanLevelRID);
//            _lastPlanLevelRID = Include.NoRID;
//            // end MID Track 4341 Performance Issues
//        }

//        /// <summary>
//        /// Sets store intransit day range for a given store, plan level.
//        /// </summary>
//        /// <param name="aStoreRID">RID of the store</param>
//        /// <param name="aPlanLevelRID">Plan Level RID</param>
//        /// <param name="aPlanLevelIntransitStoreDays">Array of longs identifying the begin (bits 32-63) and end days (bits 0-31) for each store</param>
//        /// <param name="aBeginDay">Begin day in format yyyyddd</param>
//        /// <param name="aEndDay">End day in format yyyyddd</param>
//        private void SetStoreIT_DayRange(int aStoreRID, int aPlanLevelRID, long[] aPlanLevelIntransitStoreDays, int aBeginDay, int aEndDay)
//        {
//            // begin MID Track 5953 Null REference when executing General Method
//            //SetStoreIT_DayRange(this._transaction.StoreIndexRID(aStoreRID), aPlanLevelRID, aPlanLevelIntransitStoreDays, aBeginDay, aEndDay);
//            SetStoreIT_DayRange(this._transaction.GetStoreIndexRID(aStoreRID), aPlanLevelRID, aPlanLevelIntransitStoreDays, aBeginDay, aEndDay);
//            // end MID Track 5953 Null REference when executing General Method
//        }

//        /// <summary>
//        /// Sets store intransit day range for a given store, plan level
//        /// </summary>
//        /// <param name="aStoreRID">RID for the store</param>
//        /// <param name="aPlanLevelRID">RID for the plan level</param>
//        /// <param name="aBeginDay">Begin day in format yyyyddd</param>
//        /// <param name="aEndDay">End day in format yyyyddd</param>
//        public void SetStoreIT_DayRange(int aStoreRID, int aPlanLevelRID, int aBeginDay, int aEndDay)
//        {
//            // begin MID Track 5953 Null REference when executing General Method
//            //SetStoreIT_DayRange(this._transaction.StoreIndexRID(aStoreRID), aPlanLevelRID, aBeginDay, aEndDay);
//            SetStoreIT_DayRange(this._transaction.GetStoreIndexRID(aStoreRID), aPlanLevelRID, aBeginDay, aEndDay);
//            // end MID Track 5953 Null REference when executing General Method
//        }

//        /// <summary>
//        /// Sets store intransit day range for a given store, plan level
//        /// </summary>
//        /// <param name="aStore">Index_RID that identifies the store</param>
//        /// <param name="aPlanLevelRID">Plan Level RID</param>
//        /// <param name="aBeginDay">begin Day in format yyyyddd</param>
//        /// <param name="aEndDay">End day in format yyyyddd</param>
//        public void SetStoreIT_DayRange(Index_RID aStore, int aPlanLevelRID, int aBeginDay, int aEndDay)
//        {
//            SetStoreIT_DayRange(aStore, aPlanLevelRID, this.GetPlanLevelIntransitStoreDays(aPlanLevelRID), aBeginDay, aEndDay);
//        }

//        /// <summary>
//        /// Sets store intransit day range for a coordinated list of stores, begin and end days.
//        /// </summary>
//        /// <param name="aStoreRIDs">Array of Store RIDs</param>
//        /// <param name="aPlanLevelRID">Plan Level RID</param>
//        /// <param name="aBeginDay">Array of Begin days in format yyyyddd</param>
//        /// <param name="aEndDay">Array of End days in format yyyyddd</param>
//        public void SetStoreIT_DayRange(int[] aStoreRIDs, int aPlanLevelRID, int[] aBeginDay, int[] aEndDay)
//        {
//            if (aStoreRIDs.Length != aBeginDay.Length
//                || aStoreRIDs.Length != aBeginDay.Length)
//            {
//                throw new MIDException (eErrorLevel.fatal,
//                    (int)eMIDTextCode.msg_al_StoreDateListsOutOfSync,
//                    MIDText.GetText(eMIDTextCode.msg_al_StoreDateListsOutOfSync));
//            }
//            long[] planLevelIntransitStoreDays = this.GetPlanLevelIntransitStoreDays(aPlanLevelRID);
			
//            for (int i=0; i<aStoreRIDs.Length; i++)
//            {
//                // begin MID Track 5953 Null REference when executing General Method
//                //SetStoreIT_DayRange(_transaction.StoreIndexRID(aStoreRIDs[i]), aPlanLevelRID, planLevelIntransitStoreDays, aBeginDay[i], aEndDay[i]);
//                SetStoreIT_DayRange(_transaction.GetStoreIndexRID(aStoreRIDs[i]), aPlanLevelRID, planLevelIntransitStoreDays, aBeginDay[i], aEndDay[i]);
//                // end MID Track 5953 Null REference when executing General Method
//            }
//            // begin MID Track 4341 Performance Issues
//            this._storeIntransitValueHash.Remove(aPlanLevelRID);
//            _lastPlanLevelRID = Include.NoRID;
//            // end MID Track 4341 Performance Issues
//        }

//        /// <summary>
//        /// Sets store intransit day range for a coordinated list of stores, begin and end days.
//        /// </summary>
//        /// <param name="aStore">Store Profile List</param>
//        /// <param name="aPlanLevelRID">Plan Level RID</param>
//        /// <param name="aBeginDay">Array of Begin days in format yyyyddd</param>
//        /// <param name="aEndDay">Array of End days in format yyyyddd</param>
//        public void SetStoreIT_DayRange(ProfileList aStore, int aPlanLevelRID, int[] aBeginDay, int[] aEndDay)
//        {
//            if (aStore.Count != aBeginDay.Length
//                || aStore.Count != aEndDay.Length)
//            {
//                throw new MIDException (eErrorLevel.fatal,
//                    (int)eMIDTextCode.msg_al_StoreDateListsOutOfSync,
//                    MIDText.GetText(eMIDTextCode.msg_al_StoreDateListsOutOfSync));
//            }
//            long[] planLevelIntransitStoreDays = this.GetPlanLevelIntransitStoreDays(aPlanLevelRID);
//            int storeRID;
//            for (int i=0; i<aStore.Count; i++)
//            {
//                storeRID = ((StoreProfile)aStore.ArrayList[i]).Key;
//                SetStoreIT_DayRange(storeRID, aPlanLevelRID, planLevelIntransitStoreDays, aBeginDay[i], aEndDay[i]);
//            }
//            // Begin MID Track 4341 Performance Issues
//            this._storeIntransitValueHash.Remove(aPlanLevelRID);
//            _lastPlanLevelRID = Include.NoRID;
//            // End MID track 4341 Performance Issues
//        }

//        /// <summary>
//        /// Sets store intransit begin and end days for a given plan level
//        /// </summary>
//        /// <param name="aStore">Index_RID for the store</param>
//        /// <param name="aPlanLevelRID">RID of the plan level</param>
//        /// <param name="aPlanLevelIntransitStoreDays">Array of longs that describe the begin (bits 32-63) and end (bits 0-31) days.</param>
//        /// <param name="aBeginDay">Begin Day in format yyyyddd</param>
//        /// <param name="aEndDay">End Day in format yyyyddd</param>
//        private void SetStoreIT_DayRange(Index_RID aStore, int aPlanLevelRID, long[] aPlanLevelIntransitStoreDays, int aBeginDay, int aEndDay)
//        {
//            // begin MID Track 4341 Performance Issues
//            //long dayRange;
//            //dayRange = aBeginDay;
//            //dayRange = dayRange << 32;
//            //dayRange = dayRange + aEndDay;
            
//            //aPlanLevelIntransitStoreDays[aStore.Index] = dayRange;
//            //if (this._storeIntransitValueHash.Contains(aPlanLevelRID))
//            //{
//            //	this._storeIntransitValueHash.Remove(aPlanLevelRID);
//            //}
//            int storeIndex = aStore.Index;
//            aPlanLevelIntransitStoreDays[storeIndex] = aBeginDay;
//            aPlanLevelIntransitStoreDays[storeIndex] = aPlanLevelIntransitStoreDays[storeIndex] << 32;
//            aPlanLevelIntransitStoreDays[storeIndex] = aPlanLevelIntransitStoreDays[storeIndex] + aEndDay;
//            // end MID Traack 4341 Performance Issues
//        }

//        /// <summary>
//        /// Gets the store array containing intransit values.
//        /// </summary>
//        /// <param name="aPlanLevelRID">Plan level RID</param>
//        /// <param name="ikt">Intransit Key Type describing the desired intransit type</param>
//        /// <returns>Store Array containing the plan level's store intransit values for the given key type</returns>
//        // begin MID Track 4341 Performance Issues
//        //private int[] GetStoreIntransitArray(int aPlanLevelRID, IntransitKeyType ikt)
//        internal int[] GetStoreIntransitArray(int aPlanLevelRID, IntransitKeyType ikt)
//            // end MID Track 4341 Performance Issues
//        {
//            if (aPlanLevelRID == _lastPlanLevelRID
//                && ikt.IntransitTypeKey == _lastIntransitTypeKey)
//            {
//                return _lastStoreIntransitArray;
//            }
//            else
//            {
//                _lastPlanLevelRID = aPlanLevelRID;
//                _lastIntransitTypeKey = ikt.IntransitTypeKey;
//                Hashtable planLevelValueHash;
//                // begin MID Track 4341 Performance Issues
//                //if (this._storeIntransitValueHash.Contains(aPlanLevelRID))
//                //{
//                //	planLevelValueHash = (Hashtable)this._storeIntransitValueHash[aPlanLevelRID];
//                //}
//                //else
//                planLevelValueHash = (Hashtable)this._storeIntransitValueHash[aPlanLevelRID];
//                if (planLevelValueHash == null)
//                    // end MID Track 4341 Performance Issues
//                {
//                    planLevelValueHash = new Hashtable();
//                    _storeIntransitValueHash.Add(aPlanLevelRID, planLevelValueHash);
//                }
//                // begin MID Track 4341 Performance Issues
//                //if (planLevelValueHash.Contains(ikt.IntransitTypeKey))
//                //{
//                //	_lastStoreIntransitArray = (int[])planLevelValueHash[ikt.IntransitTypeKey];
//                //}
//                //else
//                _lastStoreIntransitArray = (int[])planLevelValueHash[ikt.IntransitTypeKey];
//                if (_lastStoreIntransitArray == null)
//                    // end MID Track 4341 Performance Issues
//                {
//                    _lastStoreIntransitArray = this.LoadStoreIntransit(aPlanLevelRID, ikt);
//                    planLevelValueHash.Add(ikt.IntransitTypeKey, _lastStoreIntransitArray);
//                }
//            }
//            return _lastStoreIntransitArray;
//        }

//        /// <summary>
//        /// Gets the given store's intransit value for the given plan level and intransit type.
//        /// </summary>
//        /// <param name="aStoreRID">RID of the store</param>
//        /// <param name="aPlanLevelRID">RID of the Plan Level</param>
//        /// <param name="aIntransitType">Identifies the Intransit key type</param>
//        /// <returns>Intransit value for the store in the given plan level and intransit type</returns>
//        public int GetStoreIntransit(int aStoreRID, int aPlanLevelRID, IntransitKeyType aIntransitType)
//        {
//            // begin MID Track 5953 Null REference when executing General Method
//            //return GetStoreIntransit(_transaction.StoreIndexRID(aStoreRID), aPlanLevelRID, aIntransitType);
//            return GetStoreIntransit(_transaction.GetStoreIndexRID(aStoreRID), aPlanLevelRID, aIntransitType);
//            // end MID Track 5953 Null REference when executing General Method
//        }

//        /// <summary>
//        /// Gets the given store's intransit value for the given plan level and intransit type.
//        /// </summary>
//        /// <param name="aStore">Index_RID of the store</param>
//        /// <param name="aPlanLevelRID">RID of the Plan Level</param>
//        /// <param name="aIntransitType">Identifies the Intransit key type</param>
//        /// <returns>Intransit value for the store in the given plan level and intransit type</returns>
//        public int GetStoreIntransit(Index_RID aStore, int aPlanLevelRID, IntransitKeyType aIntransitType)
//        {
//            int[] storeIntransitArray = this.GetStoreIntransitArray(aPlanLevelRID, aIntransitType);
//            return storeIntransitArray[aStore.Index];
//        }

//        /// <summary>
//        /// Gets the given store's intransit value for the given plan level and intransit types.
//        /// </summary>
//        /// <param name="aStoreRID">RID of the store</param>
//        /// <param name="aPlanLevelRID">RID of the Plan Level</param>
//        /// <param name="aIntransitType">Array that identifies the Intransit key types to be added together for intransit</param>
//        /// <returns>Intransit value for the store in the given plan level and intransit types</returns>
//        public int GetStoreIntransit(int aStoreRID, int aPlanLevelRID, IntransitKeyType[] aIntransitType)
//        {
//            int storeIntransitValue = 0;
//            // begin MID Track 5953 Null reference when executing General Method
//            //Index_RID storeIndexRID = _transaction.StoreIndexRID(aStoreRID);
//            Index_RID storeIndexRID = _transaction.GetStoreIndexRID(aStoreRID);
//            // end MID Track 5953 Null reference when executing General Method
//            foreach (IntransitKeyType ikt in aIntransitType)
//            {
//                // BEGIN MID Change j.ellis wrong operator to accum intransit
////				storeIntransitValue =+ GetStoreIntransit(storeIndexRID, aPlanLevelRID, ikt);
//                storeIntransitValue += GetStoreIntransit(storeIndexRID, aPlanLevelRID, ikt);
//                // END MID Change
//            }
//            return storeIntransitValue;
//        }

//        /// <summary>
//        /// Loads intransit values from the database.
//        /// </summary>
//        /// <param name="aPlanLevelRID">Plan Level RID</param>
//        /// <param name="aIntransitType">Intransit Key Type</param>
//        /// <returns>Intransit values for all stores in "_allStoreList" array sequence</returns>
//        private int[] LoadStoreIntransit(int aPlanLevelRID, IntransitKeyType aIntransitType)
//        {
//            return LoadStoreIntransit(aPlanLevelRID, aIntransitType, this.GetPlanLevelIntransitStoreDays(aPlanLevelRID));
//        }

//        /// <summary>
//        /// Loads intransit values from the database.
//        /// </summary>
//        /// <param name="aPlanLevelRID">Plan Level RID</param>
//        /// <param name="aIntransitType">Intransit Key Type</param>
//        /// <param name="aStoreIntransitDays">Array of "longs" representing store Intransit From/to ranges (left integer portion of long is "from" date; right integer portion long is "to" date) </param>
//        /// <returns>Intransit values for all stores in "_allStoreList" array sequence</returns>
//        private int[] LoadStoreIntransit(int aPlanLevelRID, IntransitKeyType aIntransitType, long[] aStoreIntransitDays)
//        {
//            // begin MID Track 4362 Alternate Intransit Performance
//            IntransitReadRequest irr = new IntransitReadRequest();
//            irr.FlashBack = this._flashBack;
//            irr.SetStoreIntransitFromTo(this._transaction.GetStoreRID_CharArray_List(), aStoreIntransitDays);
//            HierarchyNodeProfile hnp = null;
//            hnp = _transaction.GetNodeData(aPlanLevelRID);
//            switch (aIntransitType.IntransitType)
//            {
//                case(eIntransitBy.Total):
//                {
//                    if (hnp.LevelType == eHierarchyLevelType.Style ||
//                        hnp.LevelType == eHierarchyLevelType.Color ||
//                        hnp.LevelType == eHierarchyLevelType.Size ||
//                        hnp.IsParentOfStyle)
//                    {
//                        // node is at style or "parent of style" level, so don't have to worry about descendants
//                        irr.IntransitHnRID = hnp.Key;
//                    }
//                    else
//                    {
//                        ArrayList alTotal = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.ParentOfStyle);
//                        foreach(IntransitReadNodeProfile irnpTotal in alTotal)
//                        {
//                            irr.IntransitHnRID = irnpTotal.Key;
//                        }
//                    }
//                    break;
//                }
//                case (eIntransitBy.Color):
//                {
//                    // begin MID Track 5338 Color Onhands zero (% Need Limit Not observed)
//                    if (hnp.LevelType == eHierarchyLevelType.Color)
//                    {
//                        // node is at "color", so don't have to worry about descendants
//                        irr.IntransitHnRID = hnp.Key;
//                    }
//                    else if (hnp.LevelType == eHierarchyLevelType.Size)
//                    {
//                        // node is at "color", get color parent
//                        irr.IntransitHnRID = (int)hnp.Parents[0];
//                    }
//                    else
//                    {
//                        // end MID Track 5338 Color Onhands zero (% Need Limit Not observed)
//                        ArrayList alColor = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Color);
//                        foreach(IntransitReadNodeProfile irnpColor in alColor)
//                        {
//                            if (irnpColor.LevelType == eHierarchyLevelMasterType.Color
//                                || irnpColor.LevelType == eHierarchyLevelMasterType.Size)
//                            {
//                                if (irnpColor.ColorCodeRID == aIntransitType.ColorRID)
//                                {
//                                    // color match -- add to this bucket, for store list
//                                    irr.IntransitHnRID = irnpColor.Key;
//                                }
//                                else if (aIntransitType.ColorRID == Include.DummyColorRID
//                                    && !irnpColor.StyleDefinedInHierarchy)
//                                {
//                                    // handle alternate hierarchy where style of color not in alternate but color is
//                                    irr.IntransitHnRID = irnpColor.Key;
//                                }
//                            }
//                        }
//                    }
//                    break;
//                }
//                case (eIntransitBy.SizeWithinColors):
//                case (eIntransitBy.Size):
//                {
//                    // begin MID Track 5338 Color Onhands zero (% Need Limit Not observed)
//                    if (hnp.LevelType == eHierarchyLevelType.Size)
//                    {
//                        // node is at "color", get color parent
//                        irr.IntransitHnRID = hnp.Key;
//                    }
//                    else
//                    {
//                        // end MID Track 5338 Color Onhands zero (% Need Limit Not observed)
//                        ArrayList alSize = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Size);
//                        foreach(IntransitReadNodeProfile irnpSize in alSize)
//                        {
//                            if (irnpSize.LevelType == eHierarchyLevelMasterType.Size)
//                            {
//                                if (irnpSize.ColorCodeRID == aIntransitType.ColorRID)
//                                {
//                                    // color match
//                                    if (irnpSize.SizeCodeRID == aIntransitType.SizeRID)
//                                    {
//                                        // size match -- add to this bucket, for all store list
//                                        irr.IntransitHnRID = irnpSize.Key;
//                                    }
//                                }
//                                else if (!irnpSize.StyleDefinedInHierarchy
//                                    && aIntransitType.ColorRID == Include.DummyColorRID
//                                    && irnpSize.SizeCodeRID == aIntransitType.SizeRID)     // MID Track 5504 Fill Size Holes does not recognize Intransit
//                                    //&& irnpSize.SizeCodeRID == aIntransitType.ColorRID)  // MID Track 5504 Fill Size Holes does not recognize Intransit
//                                {
//                                    // Handle alternate hierarchy where style of size not in alternate but size is
//                                    irr.IntransitHnRID = irnpSize.Key;
//                                }
//                            }
//                        }
//                    } // MID Track 5338 Color onhands zero (% Need Limit Not observed)
//                    break;
//                }
//                default:
//                {
//                    break;
//                }
//            }
//            StoreIntransit si = _dbIntransit.GetIt(irr, this._transaction.AllStoreRIDList());
//            this._flashBack = si.FlashBack;
//            int[] storeIntransit = new int[this._allStoreList.Count];
//            storeIntransit.Initialize();
//            foreach (StoreIntransitValue siv in si.StoreIntransitValues)
//            {
//                // begin MID Track 5953 Null Reference when executing General Method
//                //storeIntransit[this._transaction.StoreIndexRID(siv.StoreRID).Index] = siv.StoreIntransit;
//                storeIntransit[this._transaction.GetStoreIndexRID(siv.StoreRID).Index] = siv.StoreIntransit;
//                // end MID Track 5953 Null Reference when executing General Method
//            }
//            return storeIntransit;

//            //IntransitReadRequest irr = new IntransitReadRequest();
//            //irr.FlashBack = this._flashBack;
//            // // begin MID Track 4341 Performance Issues
//            // //int[] storeRIDList = new int[this._allStoreList.Count];  
//            // //ArrayList storeRID_CharArray_List = new ArrayList(this._allStoreList.Count); 
//            // //int i = 0;
//            // //foreach(StoreProfile sp in this._allStoreList.ArrayList)
//            // //{
//            // 	//storeRIDList[i] = sp.Key;  // MID Track 4341 Performance Issues
//            //	//storeRID_CharArray_List.Add(this._transaction.GetStoreRID_ToCharArray(sp.Key)); 
//            //	//i++;
//            // //}
//            // //irr.SetStoreIntransitFromTo(storeRID_CharArray_List, aStoreIntransitDays); 
//            //irr.SetStoreIntransitFromTo(this._transaction.GetStoreRID_CharArray_List(), aStoreIntransitDays);

//            //HierarchyNodeProfile hnp = null;
//            //hnp = _transaction.GetNodeData(aPlanLevelRID);
//            //switch (aIntransitType.IntransitType)
//            //{
//            //	case(eIntransitBy.Total):
//            //	{
//            //		if (hnp.LevelType == eHierarchyLevelType.Style ||
//            //			hnp.LevelType == eHierarchyLevelType.Color ||
//            //			hnp.LevelType == eHierarchyLevelType.Size ||
//            //			hnp.IsParentOfStyle)
//            //		{
//            //			// node is at style or "parent of style" level, so don't have to worry about descendants
//            //			irr.IntransitHnRID = hnp.Key;
//            //		}
//            //		else
//            //		{
//            //			HierarchyNodeList hnlTotal = _transaction.GetDescendantData(aPlanLevelRID, eHierarchyLevelMasterType.ParentOfStyle); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //			//HierarchyNodeList hnlTotal = _transaction.SAB.HierarchyServerSession.GetDescendantData(aPlanLevelRID, eHierarchyLevelMasterType.ParentOfStyle); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //			foreach(HierarchyNodeProfile hnpTotal in hnlTotal)
//            //			{
//            //				irr.IntransitHnRID = hnpTotal.Key;
//            //			}
//            //		}
//            //		break;
//            //	}
//            //	case (eIntransitBy.Color):
//            //	{
//            //		HierarchyNodeList hnlColor = _transaction.GetDescendantData(aPlanLevelRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //		//HierarchyNodeList hnlColor = _transaction.SAB.HierarchyServerSession.GetDescendantData(aPlanLevelRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //		foreach(HierarchyNodeProfile hnpColor in hnlColor)
//            //		{
//            //			if (hnpColor.ColorOrSizeCodeRID == aIntransitType.ColorRID)
//            //			{
//            //				// color match -- add to this bucket, for store list
//            //				irr.IntransitHnRID = hnpColor.Key;
//            //			}								
//            //		}
//            //		break;
//            //	}
//            //	case (eIntransitBy.SizeWithinColors):
//            //  case (eIntransitBy.Size):
//            //	{
//            //		HierarchyNodeList hnlColor = _transaction.GetDescendantData(aPlanLevelRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //		//HierarchyNodeList hnlColor = _transaction.SAB.HierarchyServerSession.GetDescendantData(aPlanLevelRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //		foreach(HierarchyNodeProfile hnpColor in hnlColor)
//            //		{
//            //			if (hnpColor.ColorOrSizeCodeRID == aIntransitType.ColorRID)
//            //			{
//            //				// color match
//            //				HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //				//HierarchyNodeList hnlSize = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size); // MID Change j.ellis Performance--cache ancestor and descendant data
//            //				foreach(HierarchyNodeProfile hnpSize in hnlSize)
//            //				{
//            //					if (hnpSize.ColorOrSizeCodeRID == aIntransitType.SizeRID)
//            //					{
//            //						// size match -- add to this bucket, for all store list
//            //						irr.IntransitHnRID = hnpSize.Key;
//            //					}
//            //				}
//            //			}
//            //		}
//            //		break;
//            //	}
//            // removed unnecessary comments
//            //	default:
//            //	{
//            //		break;
//            //	}
//            //}

//            // //StoreIntransit si = _dbIntransit.GetIt(irr, storeRIDList);  // MID Track 4341 Performance Issues
//            //StoreIntransit si = _dbIntransit.GetIt(irr, this._transaction.AllStoreRIDList()); // MID Track 4341 Performance Issues
//            //this._flashBack = si.FlashBack;
//            //int[] storeIntransit = new int[this._allStoreList.Count];
//            //storeIntransit.Initialize();
//            //foreach (StoreIntransitValue siv in si.StoreIntransitValues)
//            //{
//            //	storeIntransit[this._transaction.StoreIndexRID(siv.StoreRID).Index] = siv.StoreIntransit;
//            //}
//            //return storeIntransit;
//            // end MID Track 4362 Alternate Intransit Performance
//        }

//        /// <summary>
//        /// Gets Store Total Week Intransit (adds up the daily intransit for the days in the week)
//        /// </summary>
//        /// <param name="aWeekProfile">Week Profile of the week</param>
//        /// <param name="aHnRID">Hierarchy node RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns></returns>
//        public double GetStoreTotalWeekIntransit(
//            WeekProfile aWeekProfile,
//            int aHnRID,
//            int aStoreRID)
//        {
//            // begin MID Track 5953 Null Reference when executing General Method
//            //Index_RID storeIndexRID = this._transaction.StoreIndexRID(aStoreRID);
//            Index_RID storeIndexRID = this._transaction.GetStoreIndexRID(aStoreRID);
//            // end MID Track 5953 Null Reference when executing General Method
//            Hashtable storePlanIntransitWeekHash;
//            // begin MID Track 4341 Performance Issues
//            //if (this._storeIntransitWeekHash.Contains(aHnRID))
//            //{
//            //	storePlanIntransitWeekHash = (Hashtable)this._storeIntransitWeekHash[aHnRID];
//            //}
//            //else
//            storePlanIntransitWeekHash = (Hashtable)this._storeIntransitWeekHash[aHnRID];
//            if (storePlanIntransitWeekHash == null)
//                // end MID Track 4341 Performance Issues
//            {
//                storePlanIntransitWeekHash = new Hashtable();
//                this._storeIntransitWeekHash.Add(aHnRID, storePlanIntransitWeekHash);
//            }
//            DateTime endDay = (aWeekProfile.ProfileStartDate.Date.AddDays(aWeekProfile.DaysInWeek - 1));
//            long weekKey = (long)((aWeekProfile.ProfileStartDate.Year)*1000 + aWeekProfile.ProfileStartDate.DayOfYear);
//            weekKey = (weekKey << 32);
//            weekKey += (long)(endDay.Year*1000 + endDay.DayOfYear);
//            int[] storeIntransitValue;
//            // begin MID Track 4341 Performance Issues
//            //if (storePlanIntransitWeekHash.Contains(weekKey))
//            //{
//            //	storeIntransitValue = (int[]) storePlanIntransitWeekHash[weekKey];
//            //}
//            //else
//            storeIntransitValue = (int[]) storePlanIntransitWeekHash[weekKey];
//            if (storeIntransitValue == null)
//                // end MID Track 4341 Performance Issues
//            {
//                long[] weekKeys = new long[this._allStoreList.Count];
//                for (int i=0; i<this._allStoreList.Count; i++)
//                {
//                    weekKeys[i] = weekKey;
//                }
//                storeIntransitValue = LoadStoreIntransit(aHnRID, new IntransitKeyType(0,0), weekKeys);
//                storePlanIntransitWeekHash.Add(weekKey, storeIntransitValue);
//            }
//            return storeIntransitValue[storeIndexRID.Index];
//        }
//    }
//}
    // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    #endregion Obsolete Code