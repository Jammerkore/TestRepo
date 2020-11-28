using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.DataCommon
{
    // Begin #TT173 Provide Database Container for large data collections
    /// <summary>
    /// Describes a key for the basis of a size aggregate calculation 
    /// </summary>
	[Serializable]
    public struct SizeAggregateBasisKey
    {
        private int _HnRID;
        private int _colorNodeRID;  // TT#2257 - JSmith - Size Day to Week Summary Performance
        private int _colorCodeRID;
        private int _sizeNodeRID;   // TT#2257 - JSmith - Size Day to Week Summary Performance
        private int _sizeCodeRID;
        private bool _sizeActiveInd; // TT#2257 - JSmith - Size Day to Week Summary Performance
        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
        ///// <summary>
        ///// Creates an instance of this structure
        ///// </summary>
        ///// <param name="aHnRID">Hierarchy Node RID where the color-size resides</param>
        ///// <param name="aColorCodeRID">Color Code RID that identifies the color where the size resides</param>
        ///// <param name="aSizeCodeRID">Size Code RID that identifies the size</param>
        //public SizeAggregateBasisKey(int aHnRID, int aColorCodeRID, int aSizeCodeRID)
        //{
        //    _HnRID = aHnRID;
        //    _colorCodeRID = aColorCodeRID;
        //    _sizeCodeRID = aSizeCodeRID;
        //}
        /// <summary>
        /// Creates an instance of this structure
        /// </summary>
        /// <param name="aHnRID">Hierarchy Node RID where the color-size resides</param>
        /// <param name="aColorNodeRID">Color Node RID that identifies the color where the size resides</param>
        /// <param name="aColorCodeRID">Color Code RID that identifies the color where the size resides</param>
        /// <param name="aSizeNodeRID">Size Node RID that identifies the size</param>
        /// <param name="aSizeCodeRID">Size Code RID that identifies the size</param>
        public SizeAggregateBasisKey(int aHnRID, int aColorNodeRID, int aColorCodeRID, int aSizeNodeRID, int aSizeCodeRID, bool aSizeActiveInd)
        {
            _HnRID = aHnRID;
            _colorNodeRID = aColorNodeRID;
            _colorCodeRID = aColorCodeRID;
            _sizeNodeRID = aSizeNodeRID;
            _sizeCodeRID = aSizeCodeRID;
            _sizeActiveInd = aSizeActiveInd;
        }
        // End TT#2257
        /// <summary>
        /// Gets the Hierarchy Node RID
        /// </summary>
        public int HierarchyNodeRID
        {
            get { return _HnRID; }
        }
        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
        /// <summary>
        /// Gets the Color Node RID
        /// </summary>
        public int ColorNodeRID
        {
            get { return _colorNodeRID; }
        }
        // End TT#2257
        /// <summary>
        /// Gets the Color Code RID
        /// </summary>
        public int ColorCodeRID
        {
            get { return _colorCodeRID; }
        }
        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
        /// <summary>
        /// Gets the Size Node RID
        /// </summary>
        public int SizeNodeRID
        {
            get { return _sizeNodeRID; }
        }
        // End TT#2257
        /// <summary>
        /// Gets the Size Code RID
        /// </summary>
        public int SizeCodeRID
        {
            get { return _sizeCodeRID; }
        }
        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
        /// <summary>
        /// Gets the flag identifying if the size is active
        /// </summary>
        public bool SizeIsActive
        {
            get { return _sizeActiveInd; }
        }
        // End TT#2257
    }
    // end #TT173 Provide Database Container for large data collections
	public class StoreSizeValueGroup
	{
		//Begin TT#1076 - JScott - Size Curves by Set
		private ArrayList _storeList;
		private Hashtable _setStoreList;
		private Hashtable _storeToSetHash;

		//End TT#1076 - JScott - Size Curves by Set
		private Hashtable _chnSzValHash;
		private Hashtable _chnStrCountBySize;
		//Begin TT#1076 - JScott - Size Curves by Set
		//private decimal _chnSzTotVal;
		private Hashtable _setSzValHash;
		private Hashtable _setSzTotValHash;
		private Hashtable _setStrCountBySize;
		//End TT#1076 - JScott - Size Curves by Set
		private Hashtable _strSzValHash;
		private Hashtable _strSzTotValHash;

		public decimal ChainSizeTotalValue
		{
			get
			{
				return (decimal)_strSzTotValHash[Include.NoRID];
			}
		}

		public StoreSizeValueGroup(ArrayList aValidStoreList)
		{
			try
			{
				//Begin TT#1076 - JScott - Size Curves by Set
				_storeList = aValidStoreList;
				_setStoreList = new Hashtable();
				//End TT#1076 - JScott - Size Curves by Set
				_chnSzValHash = new Hashtable();
				_chnStrCountBySize = new Hashtable();
				//Begin TT#1076 - JScott - Size Curves by Set
				//_chnSzTotVal = 0;
				_setSzValHash = new Hashtable();
				_setSzTotValHash = new Hashtable();
				_setStrCountBySize = new Hashtable();
				//End TT#1076 - JScott - Size Curves by Set
				_strSzValHash = new Hashtable();
				_strSzTotValHash = new Hashtable();

				_strSzValHash[Include.NoRID] = null;
				_strSzTotValHash[Include.NoRID] = (decimal)0;

				foreach (int storeRID in aValidStoreList)
				{
					_strSzValHash[storeRID] = null;
					_strSzTotValHash[storeRID] = (decimal)0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ArrayList GetStoreList()
		{
			//Begin TT#1076 - JScott - Size Curves by Set
			//Hashtable storeHash;
			//IDictionaryEnumerator iEnum;

			//try
			//{
			//    storeHash = new Hashtable();

			//    iEnum = _strSzValHash.GetEnumerator();

			//    while (iEnum.MoveNext())
			//    {
			//        if ((int)iEnum.Key != Include.NoRID)
			//        {
			//            storeHash[(int)iEnum.Key] = null;
			//        }
			//    }

			//    return new ArrayList(storeHash.Keys);
			//}
			//catch (Exception exc)
			//{
			//    string message = exc.ToString();
			//    throw;
			//}
			try
			{
				return _storeList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			//End TT#1076 - JScott - Size Curves by Set
		}

		//Begin TT#1076 - JScott - Size Curves by Set
		public ArrayList GetSetStoreList(int aSetRID)
		{
			ArrayList setStoreList;

			try
			{
				setStoreList = (ArrayList)_setStoreList[aSetRID];

				if (setStoreList == null)
				{
					setStoreList = new ArrayList();

					foreach (int storeRID in _storeList)
					{
						if ((int)_storeToSetHash[storeRID] == aSetRID)
						{
							setStoreList.Add(storeRID);
						}
					}

					_setStoreList[aSetRID] = setStoreList;
				}

				return setStoreList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetStoreGroup(Hashtable aStoreToSetHash)
		{
			int setRID;
			Hashtable storeHash;
			Hashtable setSzValHash;
			Hashtable setStrCountBySize;
			IDictionaryEnumerator iEnum;
			StoreSizeValue strSzVal;
			StoreSizeValue setSzVal;
			StoreSizeValue currSetSzVal;

			try
			{
				_storeToSetHash = aStoreToSetHash;

				storeHash = (Hashtable)_strSzValHash[Include.NoRID];

				if (storeHash != null)
				{
					iEnum = storeHash.GetEnumerator();

					while (iEnum.MoveNext())
					{
						strSzVal = (StoreSizeValue)iEnum.Value;
						setRID = (int)_storeToSetHash[strSzVal.StoreRID];
						setSzVal = new StoreSizeValue(setRID, strSzVal.SizeCodeRID, strSzVal.SalesValue);

						setSzValHash = (Hashtable)_setSzValHash[setRID];

						if (setSzValHash == null)
						{
							setSzValHash = new Hashtable();
							_setSzValHash[setRID] = setSzValHash;
							_setSzTotValHash[setRID] = (decimal)0;
						}

						setStrCountBySize = (Hashtable)_setStrCountBySize[setRID];

						if (setStrCountBySize == null)
						{
							setStrCountBySize = new Hashtable();
							_setStrCountBySize[setRID] = setStrCountBySize;
						}

						currSetSzVal = (StoreSizeValue)setSzValHash[setSzVal];

						if (currSetSzVal == null)
						{
							setSzValHash[setSzVal] = setSzVal;

							if (!setStrCountBySize.Contains(setSzVal.SizeCodeRID))
							{
								setStrCountBySize[setSzVal.SizeCodeRID] = 1;
							}
							else
							{
								setStrCountBySize[setSzVal.SizeCodeRID] = (int)setStrCountBySize[setSzVal.SizeCodeRID] + 1;
							}
						}
						else
						{
							currSetSzVal.SalesValue += strSzVal.SalesValue;
						}

						_setSzTotValHash[setRID] = (decimal)_setSzTotValHash[setRID] + strSzVal.SalesValue;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#1076 - JScott - Size Curves by Set
		public int GetChainStoreCountBySize(int aSizeCodeRID)
		{
			try
			{
				if (_chnStrCountBySize.Contains(aSizeCodeRID))
				{
					return (int)_chnStrCountBySize[aSizeCodeRID];
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public Hashtable GetChainSizeValues()
		{
			try
			{
				return _chnSzValHash;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#1076 - JScott - Size Curves by Set
		public int GetSetStoreCountBySize(int aStoreRID, int aSizeCodeRID)
		{
			Hashtable setStrCount;

			try
			{
				if (_storeToSetHash != null)
				{
					setStrCount = (Hashtable)_setStrCountBySize[(int)_storeToSetHash[aStoreRID]];

					if (setStrCount != null)
					{
						if (setStrCount.Contains(aSizeCodeRID))
						{
							return (int)setStrCount[aSizeCodeRID];
						}
						else
						{
							return 0;
						}
					}
					else
					{
						return 0;
					}
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public Hashtable GetSetSizeValues(int aSetRID)
		{
			try
			{
				return (Hashtable)_setSzValHash[aSetRID];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public decimal GetSetSizeTotalValue(int aSetRID)
		{
			object setSzTotVal;

			try
			{
				setSzTotVal = _setSzTotValHash[aSetRID];

				if (setSzTotVal != null)
				{
					return (decimal)setSzTotVal;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#1076 - JScott - Size Curves by Set
		public Hashtable GetAllStoreSizeValues()
		{
			try
			{
				return (Hashtable)_strSzValHash[Include.NoRID];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public Hashtable GetStoreSizeValues(int aStoreRID)
		{
			try
			{
				return (Hashtable)_strSzValHash[aStoreRID];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public decimal GetStoreSizeTotalValue(int aStoreRID)
		{
			object strSzTotVal;

			try
			{
				strSzTotVal = _strSzTotValHash[aStoreRID];

				if (strSzTotVal != null)
				{
					return (decimal)strSzTotVal;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void ReplaceStoreSizeValues(int aStoreRID, Hashtable aValueHash)
		{
			try
			{
				_strSzValHash[aStoreRID] = aValueHash;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void Add(int aStoreRID, int aSizeCode, decimal aSales)
		{
			StoreSizeValue strSzVal;
			StoreSizeValue currStrSzVal;
			Hashtable allStoreHash;
			Hashtable storeHash;

			try
			{
				if (_strSzValHash.Contains(aStoreRID))
				{
					// Add to Chain

					strSzVal = new StoreSizeValue(Include.NoRID, aSizeCode, aSales);

					currStrSzVal = (StoreSizeValue)_chnSzValHash[strSzVal];

					if (currStrSzVal == null)
					{
						_chnSzValHash[strSzVal] = strSzVal;
					}
					else
					{
						currStrSzVal.SalesValue += aSales;
					}

					//Begin TT#1076 - JScott - Size Curves by Set
					//_chnSzTotVal += aSales;

					//End TT#1076 - JScott - Size Curves by Set
					// Get All-store and Store value hashtables

					strSzVal = new StoreSizeValue(aStoreRID, aSizeCode, aSales);

					allStoreHash = (Hashtable)_strSzValHash[Include.NoRID];

					if (allStoreHash == null)
					{
						allStoreHash = new Hashtable();
						_strSzValHash[Include.NoRID] = allStoreHash;
						_strSzTotValHash[Include.NoRID] = (decimal)0;
					}

					storeHash = (Hashtable)_strSzValHash[aStoreRID];

					if (storeHash == null)
					{
						storeHash = new Hashtable();
						_strSzValHash[aStoreRID] = storeHash;
						_strSzTotValHash[aStoreRID] = (decimal)0;
					}

					// Add value to All-store and Store hashtables if it does not exist, or update if it does

					currStrSzVal = (StoreSizeValue)allStoreHash[strSzVal];

					if (currStrSzVal == null)
					{
						allStoreHash[strSzVal] = strSzVal;
						storeHash[strSzVal] = strSzVal;

						if (!_chnStrCountBySize.Contains(strSzVal.SizeCodeRID))
						{
							_chnStrCountBySize[strSzVal.SizeCodeRID] = 1;
						}
						else
						{
							_chnStrCountBySize[strSzVal.SizeCodeRID] = (int)_chnStrCountBySize[strSzVal.SizeCodeRID] + 1;
						}
					}
					else
					{
						currStrSzVal.SalesValue += aSales;
					}

					_strSzTotValHash[Include.NoRID] = (decimal)_strSzTotValHash[Include.NoRID] + aSales;
					_strSzTotValHash[aStoreRID] = (decimal)_strSzTotValHash[aStoreRID] + aSales;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	public class StoreSizeValue : IComparable
	{
		private int _storeRID;
		private int _sizeCodeRID;
		private decimal _salesValue;

		public int StoreRID
		{
			get
			{
				return _storeRID;
			}
		}

		public int SizeCodeRID
		{
			get
			{
				return _sizeCodeRID;
			}
		}

		public decimal SalesValue
		{
			get
			{
				return _salesValue;
			}
			set
			{
				_salesValue = value;
			}
		}

		public StoreSizeValue(int aStoreRID, int aSizeCodeRID, decimal aSalesValue)
		{
			_storeRID = aStoreRID;
			_sizeCodeRID = aSizeCodeRID;
			_salesValue = aSalesValue;
		}

		public StoreSizeValue Copy()
		{
			StoreSizeValue strSzVal;

			try
			{
				strSzVal = new StoreSizeValue(_storeRID, _sizeCodeRID, _salesValue);
				return strSzVal;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public override bool Equals(object obj)
		{
			try
			{
				if (obj.GetType() == typeof(StoreSizeValue))
				{
					if (((StoreSizeValue)obj)._storeRID == _storeRID && ((StoreSizeValue)obj)._sizeCodeRID == _sizeCodeRID)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public override int GetHashCode()
		{
			try
			{
				return _storeRID + _sizeCodeRID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#region IComparable Members
		public int CompareTo(object obj)
		{
			StoreSizeValue entry;

			try
			{
				if (obj.GetType() != typeof(StoreSizeValue))
				{
					throw new Exception("Invalid comparison to StoreSizeValue object");
				}

				entry = (StoreSizeValue)obj;

				if (_storeRID < entry.StoreRID)
				{
					return -1;
				}
				else if (_storeRID > entry.StoreRID)
				{
					return 1;
				}
				else
				{
					if (_sizeCodeRID < entry.SizeCodeRID)
					{
						return -1;
					}
					else if (_sizeCodeRID > entry.SizeCodeRID)
					{
						return 1;
					}
					else
					{
						return 0;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion
	}

	public class SizeCurveBalanceItem : IComparable
	{
		int _sizeCode;
		double _value;
		bool _locked;

		public int SizeCode
		{
			get
			{
				return _sizeCode;
			}
		}

		public double Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public bool Locked
		{
			get
			{
				return _locked;
			}
			set
			{
				_locked = value;
			}
		}

		public SizeCurveBalanceItem(int aSizeCode, double aValue, bool aLocked)
		{
			try
			{
				_sizeCode = aSizeCode;
				_value = aValue;
				_locked = aLocked;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int CompareTo(object obj)
		{
			SizeCurveBalanceItem inItem;

			try
			{
				if (obj.GetType() == typeof(SizeCurveBalanceItem))
				{
					inItem = (SizeCurveBalanceItem)obj;

					if (inItem.Value > _value)
					{
						return -1;
					}
					else if (inItem.Value < _value)
					{
						return 1;
					}
					else
					{
						if (inItem.SizeCode > _sizeCode)
						{
							return -1;
						}
						else
						{
							return 1;
						}
					}
				}
				else
				{
					return 1;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
