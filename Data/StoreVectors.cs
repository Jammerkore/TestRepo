using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    #region Store Value Vector
    /// <summary>
    /// Vector of store values
    /// </summary>
    [Serializable]
    public class StoreVector : ICloneable
    {
        private double[] _storeVector;     // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        private double _totalQty;          // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        /// <summary>
        /// Create instance of this class
        /// </summary>
        public StoreVector()
        {
            //Begin TT#739-MD -jsobek -Delete Stores -Max Store RID
            //_storeVector = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)]; // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
            _storeVector = new double[MaxStoresHelper.GetStoreMaxRID(0)];
            //End TT#739-MD -jsobek -Delete Stores -Max Store RID
            _totalQty = 0;
        }
        // begin TT#801 BP Need more Pack Selection Criteria (performance)
        public StoreVector(StoreVector aStoreVector)
        {
            _storeVector = new double[aStoreVector.Count];
            for (int storeIndex = 0; storeIndex < _storeVector.Length; storeIndex++)
            {
                _storeVector[storeIndex] = aStoreVector._storeVector[storeIndex];
            }
            _totalQty = aStoreVector._totalQty;
        }
        // end TT#801 BP Need more Pack Selection Criteria (performance)
        /// <summary>
        /// Count of stores in vector
        /// </summary>
        public int Count
        {
            get { return _storeVector.Length; }
        }
        /// <summary>
        /// Max Store RID in vector
        /// </summary>
        public int MaxStoreRID
        {
            get { return _storeVector.Length; }
        }
        /// <summary>
        /// All store total value
        /// </summary>
        public double AllStoreTotalValue    // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        {
            get { return _totalQty; }
        }
        /// <summary>
        /// Gets Store Value for given store RID
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Value in vector for the given store RID</returns>
        public double GetStoreValue(int aStoreRID)    // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        {
            // begin TT#801 BP Need more Pack Selection Criteria (performance)
            try
            {
                int storeIndex = aStoreRID - 1;
                return _storeVector[storeIndex];
            }
            catch (IndexOutOfRangeException)
            {
                if (aStoreRID < 1)
                {
                    throw new ArgumentException("GetStoreValue:  StoreRID must be greater than 0");
                }
                return 0;
            }

            //int storeIndex = aStoreRID - 1;
            //if (storeIndex < 0)
            //{
            //    throw new ArgumentException("GetStoreValue:  StoreRID must be greater than 0");
            //}
            //if (storeIndex < Count)
            //{
            //    return _storeVector[storeIndex];
            //}
            //return 0;
            // end TT#801 BP Need more Pack Selection Criteria
        }
        /// <summary>
        /// Sets Store Value for given store RID
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <param name="aValue">Desired value for store</param>
        public void SetStoreValue(int aStoreRID, double aValue)   // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        {
            // begin TT#801 BP Need more Pack Selection Criteria (performance)
            int storeIndex = aStoreRID - 1;
            try
            {
                _totalQty -= _storeVector[storeIndex];
            }
            catch (IndexOutOfRangeException)
            {
                if (aStoreRID < 1)
                {
                    throw new ArgumentException("SetStoreValue: StoreRID must be greater than 0");
                }
                double[] storeVector = new double[aStoreRID];    // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
                for (int i = 0; i < _storeVector.Length; i++)
                {
                    storeVector[i] = _storeVector[i];
                }
            }
            _storeVector[storeIndex] = aValue;
            _totalQty += aValue;

            //if (aStoreRID > Count)
            //{
            //    double[] storeVector = new double[aStoreRID];    // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
            //    for (int i = 0; i < _storeVector.Length; i++)
            //    {
            //        storeVector[i] = _storeVector[i];
            //    }
            //}
            //int storeIndex = aStoreRID - 1;
            //if (storeIndex < 0)
            //{
            //    throw new ArgumentException("SetStoreValue: StoreRID must be greater than 0");
            //}
            //_totalQty -= _storeVector[storeIndex];
            //_storeVector[storeIndex] = aValue;
            //_totalQty += aValue;
            // end TT#801 BP Need more Pack Selection Criteria (performance)
        }
        public Object Clone()
        {
            // begin TT#801 BP Need more Pack Selection Criteria (performance)
            StoreVector sv = new StoreVector(this);
            return sv;

            //StoreVector sv = new StoreVector();
            //for (int storeRID = this.MaxStoreRID; storeRID > 0; storeRID--)
            //{
            //    sv.SetStoreValue(storeRID, GetStoreValue(storeRID));
            //}
            //return sv;
            // end TT#801 BP Need more Pack Selection Criteria (performance)
        }
        // begin TT#580 Build Packs creates duplicate results
        /// <summary>
        /// Determines if this vector equals another
        /// </summary>
        /// <param name="obj">Store Vector to compare to this vector</param>
        /// <returns>True: vectors are equal; False: vectors are not equal</returns>
        public override bool Equals(object obj)
        {
            StoreVector sv = obj as StoreVector;
            if (sv == null)
            {
                return false;
            }
            if (this._totalQty != sv._totalQty)
            {
                return false;
            }
            StoreVector sv1;
            StoreVector sv2;
            if (sv.MaxStoreRID < this.MaxStoreRID)
            {
                sv1 = this;
                sv2 = sv;
            }
            else
            {
                sv1 = sv;
                sv2 = this;
            }
            for (int storeRID = sv1.MaxStoreRID; storeRID < 1; storeRID--)
            {
                if (sv1.GetStoreValue(storeRID) != sv2.GetStoreValue(storeRID))
                {
                    return false;
                }
            }
            return true;
        }
        // end TT#580 Build Packs creates duplicate results
		//Begin TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
		//End TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
        // begin TT#1391 - TMW New Action
        public void Clear()
        {
            _storeVector.Initialize();
            _totalQty = _storeVector[0]; // this entry should have zero in it!
        }
        // end TT#1391 - TMW New Action
    }
    #endregion Store Value Vector

    #region Store Size Value Vector
    /// <summary>
    /// Vector of store size values.
    /// </summary>
    [Serializable]
    public class StoreSizeVector : StoreVector, ICloneable
    {
        private int _sizeCodeRID;
        /// <summary>
        /// Creates an instance of this vector
        /// </summary>
        /// <param name="aSizeCodeRID">Size Code RID for the size represented by this vector</param>
        public StoreSizeVector(int aSizeCodeRID)
            : base()
        {
            _sizeCodeRID = aSizeCodeRID;
        }
        // begin TT#801 - BP Need more Pack Selection Criteria (performance)
        public StoreSizeVector(StoreSizeVector aStoreSizeVector)
            : base(aStoreSizeVector)
        {
            _sizeCodeRID = aStoreSizeVector._sizeCodeRID;
        }
        // end TT#801 - BP Need more Pack Selection Criteria (performance)
        /// <summary>
        /// Gets the Size Code RID of the size whose store values are in this vector
        /// </summary>
        public int SizeCodeRID
        {
            get { return _sizeCodeRID; }
        }
        public new Object Clone()
        {
            // begin TT#801 - BP Need more Pack Selection Criteria (performance)
            StoreSizeVector ssv = new StoreSizeVector(this);
            return ssv;
            //StoreSizeVector ssv = new StoreSizeVector(_sizeCodeRID);
            //for (int storeRID = this.MaxStoreRID; storeRID > 0; storeRID--)
            //{
            //    ssv.SetStoreValue(storeRID, this.GetStoreValue(storeRID));
            //}
            //return ssv;
            // end TT#801 - BP Need more Pack Selection Criteria (performance)
        }
        public override bool Equals(object obj)
        {
            StoreSizeVector ssv = obj as StoreSizeVector;
            if (ssv == null)
            {
                return false;
            }
            if (this._sizeCodeRID != ssv._sizeCodeRID)
            {
                return false;
            }
            return base.Equals(obj);
        }
		//Begin TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
		//End TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
     }
    #endregion Store Size Value Vector

    #region Store Pack Value Vector
    /// <summary>
    /// Vector of store pack values (values are number of packs)
    /// </summary>
    [Serializable]
    public class StorePackVector : StoreVector, ICloneable
    {
        private int _packID;
        private OptionPack_PackPattern _packPattern;
        private int _totalPackSizeError;

        private int _countAllStoresWithPacks;

        /// <summary>
        /// Creates an instance of this vector
        /// </summary>
        /// <param name="aPackID">ID of the pack represented in this vector</param>
        public StorePackVector(int aPackID, OptionPack_PackPattern aPackPattern)
            : base()
        {
            _packID = aPackID;
            _packPattern = aPackPattern;
            _totalPackSizeError = 0;

            _countAllStoresWithPacks = -1;
        }
        // begin TT#801 - BP Need more Pack Selection Criteria (performance)
        public StorePackVector(StorePackVector aStorePackVector)
            : base (aStorePackVector)
        {
            _packID = aStorePackVector._packID;
            _packPattern = aStorePackVector._packPattern;
            _totalPackSizeError = aStorePackVector._totalPackSizeError;
            _countAllStoresWithPacks = aStorePackVector._countAllStoresWithPacks;
        }
        // end TT#801 - BP Need more Pack Selection Criteria (performance)

        /// <summary>
        /// Gets the Pack ID whose store values are in this vector
        /// </summary>
        public int PackID
        {
            get { return _packID; }
        }
        /// <summary>
        /// Gets the Pack Multiple for this pack
        /// </summary>
        public int PackMultiple
        {
            get { return _packPattern.PackMultiple; }
        }
        /// <summary>
        /// Gets the source Pattern Name for this pack
        /// </summary>
        public string PatternName
        {
            get { return _packPattern.PatternName; }
        }
        /// <summary>
        /// Gets the source Pack Pattern RID
        /// </summary>
        public int PackPatternRID
        {
            get { return _packPattern.PackPatternRID; }
        }
        /// <summary>
        /// Gets a copy of the List of SizeUnits for this pack
        /// </summary>
        public List<SizeUnits> PackSizeUnits
        {
            get { return _packPattern.SizeUnitsList; }
        }
        /// <summary>
        /// Gets the All Store Pack Total Units
        /// </summary>
        public int AllStorePackTotalUnits
        {
            get
            {
                return _packPattern.PackMultiple * (int)base.AllStoreTotalValue;    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
            }
        }
        /// <summary>
        /// Gets the total number of packs allocated
        /// </summary>
        public int AllStoreNumberOfPacks
        {
            get { return (int)base.AllStoreTotalValue; }    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
        }
        /// <summary>
        /// Gets the number of stores having at least 1 pack allocatged
        /// </summary>
        public int CountOfAllStoresWithPacks
        {
            get
            {
                if (_countAllStoresWithPacks < 0)
                {
                    _countAllStoresWithPacks = 0;
                    for (int storeRID = MaxStoreRID; storeRID > 0; storeRID--)
                    {
                        if (GetStoreValue(storeRID) > 0)
                        {
                            _countAllStoresWithPacks++;
                        }
                    }
                }
                return _countAllStoresWithPacks;
            }                        
        }
        /// <summary>
        /// Gets the count of sizes in this pack pattern with units
        /// </summary>
        public int CountOfSizesInPack
        {
            get
            {
                return _packPattern.CountOfSizesWithUnits;
            }
        }
        /// <summary>
        /// Gets or sets total size unit error introduced by this pack
        /// </summary>
        public int TotalPackSizeError
        {
            get
            {
                return _totalPackSizeError;
            }
            set
            {
                _totalPackSizeError = value;
            }
        }

        /// <summary>
        /// Gets the number of packs allocated to store
        /// </summary>
        /// <param name="aStoreRID"></param>
        /// <returns></returns>
        public int GetStoreNumberofPacks(int aStoreRID)
        {
            return (int)GetStoreValue(aStoreRID); // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        }
        /// <summary>
        /// Gets the Pack Total Units for the given stores
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Pack Total Units for the given store</returns>
        public int GetStorePackTotalUnits(int aStoreRID)
        {
            return _packPattern.PackMultiple * (int)base.GetStoreValue(aStoreRID); // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        }
        /// <summary>
        /// Sets the number of packs allocated to store
        /// </summary>
        /// <param name="aStoreRID"></param>
        /// <param name="aValue"></param>
        new public void SetStoreValue(int aStoreRID, double aValue) //  TT#801 - BP Need more Pack Selection Criteria (unrelated)  
        {
            _countAllStoresWithPacks = -1;
            base.SetStoreValue(aStoreRID, (double)aValue); // TT#744 - JEllis - Use Orig Pack Fit Logic - Remove Bulk
        }
        public new Object Clone()
        {
            // begin TT#801 - BP Need more Pack Selection Criteria (performance)
            StorePackVector spv = new StorePackVector(this);
            return spv;
            //StorePackVector spv = new StorePackVector(_packID, _packPattern);
            //spv.TotalPackSizeError = _totalPackSizeError;
            //for (int storeRID = this.MaxStoreRID; storeRID > 0; storeRID--)
            //{
            //    spv.SetStoreValue(storeRID, GetStoreValue(storeRID));
            //}
            //return spv;
            // end TT#801 - BP Need more Pack Selection Criteria (performance)
        }
        // begin TT#580 build packs created duplicates
        /// <summary>
        /// Determine if another StorePackVector content equals content of this vector (pack ID may be different)
        /// </summary>
        /// <param name="aStorePackVector">Store Pack Vector to compare</param>
        /// <returns>True: if content other than pack ID are the same; False otherwise</returns>
        public bool IsThisPackVectorContentEqualTo(StorePackVector aStorePackVector)
        {
            if (aStorePackVector == null)
            {
                return false;
            }
            if (this._totalPackSizeError != aStorePackVector._totalPackSizeError)
            {
                return false;
            }
            SizeUnitRun sur = this._packPattern.SizeRun;
            if (!sur.Equals(aStorePackVector._packPattern.SizeRun))
            {
                return false;
            }
            return this.Equals(aStorePackVector);
        }
        // end TT#580 build packs created duplicates
    }
    #endregion Store Pack Value Vector
}
