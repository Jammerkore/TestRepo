using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
	/// <summary>
	/// Reader for IMO
	/// </summary>
	[Serializable()]
	public class IMO_Reader:Profile
	{
		//========
		// FIELDS 
		//========
		ApplicationSessionTransaction _transaction;
		ProfileList _allStoreList;
		Data.IMO_Data _dbIMO;
		DateTime _flashBack;
		Hashtable _storeIMOValueHash;
		Hashtable _storeIMONodeHash;	
		int _lastPlanLevelRID;
		long _lastIMOTypeKey;
		int[] _lastStoreIMOArray;

		//==============
		// CONSTRUCTORS
		//==============
		public IMO_Reader(ApplicationSessionTransaction aTransaction):base(-1)
		{
			_transaction = aTransaction;
			_allStoreList = _transaction.GetMasterProfileList(eProfileType.Store);
			_flashBack = Include.UndefinedDate;
			_storeIMOValueHash = new Hashtable();
			_storeIMONodeHash = new Hashtable();
			_dbIMO = new Data.IMO_Data();
			_lastPlanLevelRID = Include.NoRID;
			_lastIMOTypeKey = -1;
			_lastStoreIMOArray = null;
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
				return eProfileType.IMO;
			}
		}

		//=========
		// METHODS
		//=========
		/// <summary>
		/// Resets IMO flash back identifier to trigger a re-read of IMO.  Does not destroy store begin-end dates.
		/// </summary>
		public void ResetIMO_Reader()
		{
			_flashBack = Include.UndefinedDate;
			_storeIMOValueHash.Clear();
			_storeIMONodeHash.Clear();
			this._lastIMOTypeKey = -1;
			this._lastPlanLevelRID = Include.NoRID;
			this._lastStoreIMOArray = null;
		}

		/// <summary>
		/// Gets the store array containing IMO values.
		/// </summary>
		/// <param name="aPlanLevelRID">Plan level RID</param>
		/// <param name="ikt">IMO Key Type describing the desired IMO type</param>
		/// <returns>Store Array containing the plan level's store IMO values for the given key type</returns>
	    internal int[] GetStoreIMOArray(int aPlanLevelRID, IntransitKeyType ikt)
		{
			if (aPlanLevelRID == _lastPlanLevelRID
				&& ikt.IntransitTypeKey == _lastIMOTypeKey)
			{
				return _lastStoreIMOArray;
			}
			else
			{
				_lastPlanLevelRID = aPlanLevelRID;
				_lastIMOTypeKey = ikt.IntransitTypeKey;
				Hashtable planLevelValueHash;

				planLevelValueHash = (Hashtable)this._storeIMOValueHash[aPlanLevelRID];
				if (planLevelValueHash == null)
				{
					planLevelValueHash = new Hashtable();
					_storeIMOValueHash.Add(aPlanLevelRID, planLevelValueHash);
				}

				_lastStoreIMOArray = (int[])planLevelValueHash[ikt.IntransitTypeKey];
				if (_lastStoreIMOArray == null)
				{
					_lastStoreIMOArray = this.LoadStoreIMO(aPlanLevelRID, ikt);
					planLevelValueHash.Add(ikt.IntransitTypeKey, _lastStoreIMOArray);
				}
			}
			return _lastStoreIMOArray;
		}

		/// <summary>
		/// Gets the given store's IMO value for the given plan level and IMO type.
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <param name="aPlanLevelRID">RID of the Plan Level</param>
		/// <param name="aIntransitType">Identifies the IMO key type</param>
		/// <returns>IMO value for the store in the given plan level and intransit type</returns>
		public int GetStoreIMO(int aStoreRID, int aPlanLevelRID, IntransitKeyType aIntransitType)
		{
			return GetStoreIMO(_transaction.GetStoreIndexRID(aStoreRID), aPlanLevelRID, aIntransitType);
		}

		/// <summary>
		/// Gets the given store's IMO value for the given plan level and IMO type.
		/// </summary>
		/// <param name="aStore">Index_RID of the store</param>
		/// <param name="aPlanLevelRID">RID of the Plan Level</param>
		/// <param name="aIntransitType">Identifies the Intransit key type</param>
		/// <returns>Intransit value for the store in the given plan level and intransit type</returns>
		public int GetStoreIMO(Index_RID aStore, int aPlanLevelRID, IntransitKeyType aIntransitType)
		{
			int[] storeIMOArray = this.GetStoreIMOArray(aPlanLevelRID, aIntransitType);
			return storeIMOArray[aStore.Index];
		}

		/// <summary>
		/// Gets the given store's IMO value for the given plan level and IMO types.
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <param name="aPlanLevelRID">RID of the Plan Level</param>
		/// <param name="aIntransitType">Array that identifies the IMO key types to be added together for IMO</param>
		/// <returns>IMO value for the store in the given plan level and IMO types</returns>
		public int GetStoreIMO(int aStoreRID, int aPlanLevelRID, IntransitKeyType[] aIntransitType)
		{
			int storeIMOValue = 0;
			Index_RID storeIndexRID = _transaction.GetStoreIndexRID(aStoreRID);
			foreach (IntransitKeyType ikt in aIntransitType)
			{
                storeIMOValue += GetStoreIMO(storeIndexRID, aPlanLevelRID, ikt);
			}
			return storeIMOValue;
		}

		/// <summary>
		/// Loads IMO values from the database.
		/// </summary>
		/// <param name="aPlanLevelRID">Plan Level RID</param>
		/// <param name="aIntransitType">Intransit Key Type</param>
		/// <returns>IMO values for all stores in "_allStoreList" array sequence</returns>
		private int[] LoadStoreIMO(int aPlanLevelRID, IntransitKeyType aIntransitType)
		{
			IMO_ReadRequest irr = new IMO_ReadRequest();
			irr.FlashBack = this._flashBack;
			//irr.SetStoreIMOFromTo(this._transaction.GetStoreRID_CharArray_List(), aStoreIMODays);
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
						irr.IMOHnRID = hnp.Key;
					}
					else
					{
						// BEGIN TT#1401 - stodd - add resevation stores (IMO)
						//ArrayList alTotal = _transaction.GetIMOReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.ParentOfStyle);
						ArrayList alTotal = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.ParentOfStyle);
						// END TT#1401 - stodd - add resevation stores (IMO)
						foreach(IntransitReadNodeProfile irnpTotal in alTotal)
						{
							irr.IMOHnRID = irnpTotal.Key;
						}
					}
					break;
				}
				case (eIntransitBy.Color):
				{
					if (hnp.LevelType == eHierarchyLevelType.Color)
					{
						// node is at "color", so don't have to worry about descendants
						irr.IMOHnRID = hnp.Key;
					}
					else if (hnp.LevelType == eHierarchyLevelType.Size)
					{
						// node is at "color", get color parent
						irr.IMOHnRID = (int)hnp.Parents[0];
					}
					else
					{
						// BEGIN TT#1401 - stodd - add resevation stores (IMO)
						//ArrayList alColor = _transaction.GetIMOReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Color);
						ArrayList alColor = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Color);
						// END TT#1401 - stodd - add resevation stores (IMO)

						foreach(IntransitReadNodeProfile irnpColor in alColor)
						{
							if (irnpColor.LevelType == eHierarchyLevelMasterType.Color
								|| irnpColor.LevelType == eHierarchyLevelMasterType.Size)
							{
								if (irnpColor.ColorCodeRID == aIntransitType.ColorRID)
								{
									// color match -- add to this bucket, for store list
									irr.IMOHnRID = irnpColor.Key;
								}
								else if (aIntransitType.ColorRID == Include.DummyColorRID
									&& !irnpColor.StyleDefinedInHierarchy)
								{
									// handle alternate hierarchy where style of color not in alternate but color is
									irr.IMOHnRID = irnpColor.Key;
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
						irr.IMOHnRID = hnp.Key;
					}
					else
					{
						// BEGIN TT#1401 - stodd - add resevation stores (IMO)
						//ArrayList alSize = _transaction.GetIMOReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Size);
						ArrayList alSize = _transaction.GetIntransitReadNodes(aPlanLevelRID, eHierarchyLevelMasterType.Size);
						// END TT#1401 - stodd - add resevation stores (IMO)
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
										irr.IMOHnRID = irnpSize.Key;
									}
								}
								else if (!irnpSize.StyleDefinedInHierarchy
									&& aIntransitType.ColorRID == Include.DummyColorRID
									&& irnpSize.SizeCodeRID == aIntransitType.SizeRID)    
								{
									// Handle alternate hierarchy where style of size not in alternate but size is
									irr.IMOHnRID = irnpSize.Key;
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
    		StoreIMO si = _dbIMO.GetIMO(irr, this._transaction.AllStoreRIDList());
			this._flashBack = si.FlashBack;
			int[] storeIMO = new int[this._allStoreList.Count];
			storeIMO.Initialize();
			foreach (StoreIMOValue siv in si.StoreIMOValues)
			{
				storeIMO[this._transaction.GetStoreIndexRID(siv.StoreRID).Index] = siv.StoreIMO;
			}
			return storeIMO;
		}

		/// <summary>
		/// Gets Store Total IMO 
		/// </summary>
		/// <param name="aHnRID">Hierarchy node RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns></returns>
		public double GetStoreTotalIMO(
			int aHnRID,
			int aStoreRID)
		{
			Index_RID storeIndexRID = this._transaction.GetStoreIndexRID(aStoreRID);
			Hashtable storePlanIMONodeHash;

			storePlanIMONodeHash = (Hashtable)this._storeIMONodeHash[aHnRID];
			if (storePlanIMONodeHash == null)
			{
				storePlanIMONodeHash = new Hashtable();
				this._storeIMONodeHash.Add(aHnRID, storePlanIMONodeHash);
			}
			int[] storeIMOValue;

			storeIMOValue = (int[])storePlanIMONodeHash[aHnRID];
			if (storeIMOValue == null)
			{
				storeIMOValue = LoadStoreIMO(aHnRID, new IntransitKeyType(0, 0));
				storePlanIMONodeHash.Add(aHnRID, storeIMOValue);
			}
			return storeIMOValue[storeIndexRID.Index];
		}
	}
}
