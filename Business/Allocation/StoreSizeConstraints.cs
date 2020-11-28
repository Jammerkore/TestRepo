using System;
using System.Collections;
using System.Collections.Generic; // TT#1391 - TMW New Action (Unrelated Performance)
using System.Globalization;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;    // TT#1432 - Size Dimension Constraints not working

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// The class sets and hols the Stoe / Size min, max, and mult.
	/// It merges the Size Constraints model defined and constraints defined in the allocation profile.
	/// </summary>
	public class StoreSizeConstraints
	{
		private SessionAddressBlock SAB;
		private SizeConstraintModelProfile _constraintModel;
		private CollectionDecoder _constraintDecoder;
        private Hashtable _constraintStoreGroupLevelHash;
        // begin TT#1391 - TMW New Action (Unrelated - Performance)
        //private Hashtable _storeMinHash;
        //private Hashtable _storeMaxHash;
        //private Hashtable _storeMultHash;
        private Dictionary<int, StoreSizeVector> _storeMin;
        private Dictionary<int, StoreSizeVector> _storeMax;
        private Dictionary<int, StoreSizeVector> _storeCapMax;  // TT#2155 - JEllis - Fill Size Null Reference
        private Dictionary<int, StoreSizeVector> _storeMulti;
        // end TT#1391 - TMW New Action (Unrelated - Performance)
		private AllocationProfile _allocProfile;
		private int _colorRid;
        //private bool _useStoreCapacity;  // TT#2155 - JEllis - Fill Size Null Reference

        //public StoreSizeConstraints(SessionAddressBlock sab, int constraintModelRid, AllocationProfile allocProfile, int colorRid, bool useStoreCapacity) // TT#2155 - JEllis - Fill Size Null Reference
        public StoreSizeConstraints(SessionAddressBlock sab, int constraintModelRid, AllocationProfile allocProfile, int colorRid) // TT#2155 - JEllis - Fill Size Null Reference
		{
			SAB = sab;
			_colorRid = colorRid;
			_allocProfile = allocProfile;
            //_useStoreCapacity = useStoreCapacity;  // TT#2155 - JEllis - Fill Size Null Reference

			if (constraintModelRid != Include.NoRID)
			{
				_constraintModel = new SizeConstraintModelProfile(constraintModelRid);
                _constraintStoreGroupLevelHash = StoreMgmt.GetStoreGroupLevelHashTable(_constraintModel.StoreGroupRid); //SAB.StoreServerSession.GetStoreGroupLevelHashTable(_constraintModel.StoreGroupRid);
                // Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
				//_constraintDecoder = new CollectionDecoder(this._allocProfile.AppSessionTransaction, _constraintModel.CollectionSets, _constraintStoreGroupLevelHash);  // TT#1432 - Size Dimension Constraints not working
                _constraintDecoder = new CollectionDecoder(this._allocProfile.AppSessionTransaction, _constraintModel.CollectionSets(StoreMgmt.StoreGroup_GetVersion(_constraintModel.StoreGroupRid)), _constraintStoreGroupLevelHash);  // TT#1432 - Size Dimension Constraints not working
                // End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
                //_constraintDecoder = new CollectionDecoder(_constraintModel.CollectionSets, _constraintStoreGroupLevelHash); // TT#1432 - Size Dimension Constraints not working
			}
			else
			{
				_constraintModel = null;
				_constraintDecoder = null;
				_constraintStoreGroupLevelHash = null;
			}

            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //_storeMinHash = new Hashtable();
            //_storeMaxHash = new Hashtable();
            //_storeMultHash = new Hashtable();
            _storeMin = new Dictionary<int, StoreSizeVector>();
            _storeMax = new Dictionary<int, StoreSizeVector>();
            _storeCapMax = new Dictionary<int, StoreSizeVector>(); // TT#2155 - Jellis - Fill Size Null Reference
            _storeMulti = new Dictionary<int, StoreSizeVector>();
            // end TT#1391 - TMW New Action (Unrelated - Performance)
		}

        // begin TT#1391 - TMW New Action (Unrelated - Performance)
        //private void SetStoreMin(int storeRid, int sizeRid, int min)
        //{
        //    SetValue(storeRid, sizeRid, min, _storeMinHash);
        //}
        // end TT#1391 - TMW New Action (Unrelated - Performance)
        
        //public int GetStoreMin(int storeRid, int sizeRid) // MID Track 3492 Size Need with constraints not allocating correctly
        //public int GetStoreMin(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
        public int GetStoreMin(int storeRid, int sizeRid)      // TT#1391 - TMW New Action
		{
			//int min = GetValue(storeRid, sizeRid, _storeMinHash); // MID Track 3492 Size Need with constraints not allocating correctly
            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //int min = GetValue(storeRid, aSizeCodeProfile, _storeMinHash); // MID Track 3492 Size Need with constraints not allocating correctly
            int min = GetValue(storeRid, sizeRid, _storeMin);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
            return min;
		}

        // begin TT#1391 - TMW New Action (Unrelated - Performance)
        //private void SetStoreMax(int storeRid, int sizeRid, int max)
        //{
        //    SetValue(storeRid, sizeRid, max, _storeMaxHash);
        //}
        // end TT#1391 - TMW New Action (Unrelated - Performance)
        
        //public int GetStoreMax(int storeRid, int sizeRid) // MID Track 3492 Size Need with constraints not allocating correctly
        //public int GetStoreMax(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
        /// <summary>
        /// Gets the store size maximum (without considering capacity) -- use when breaking out color allocation to size (ie. capacity was considered when allocating the color)
        /// </summary>
        /// <param name="storeRid">RID identifying the store</param>
        /// <param name="sizeRid">RID identifying the size</param>
        /// <returns>"Inventory" maximum</returns>
        public int GetStoreMax(int storeRid, int sizeRid) // TT#1391 - TMW New Action
		{
			//int max = GetValue(storeRid, sizeRid, _storeMaxHash); // MID Track 3492 Size Need with constraints not allocating correctly
            //int max = GetValue(storeRid, aSizeCodeProfile, _storeMaxHash); // MID Track 3492 Size Need with constraints not allocating correctly
            int max = GetValue(storeRid, sizeRid, _storeMax);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
            return max;
		}

        // begin TT#2155 - JEllis - Fill Size Hole Null Reference
        /// <summary>
        /// Gets the store size capacity maximum -- use when building allocation from size UP (ie. capacity needs to be considered in the decision).
        /// </summary>
        /// <param name="storeRid">RID identifying the store</param>
        /// <param name="sizeRid">RID identifying the size</param>
        /// <returns>"Inventory" capacity maximum</returns>
        public int GetStoreCapacityMax(int storeRid, int sizeRid)
        {
            return GetValue(storeRid, sizeRid, _storeCapMax);
        }
        // end TT#2155 - JEllis - Fill Size Hole Null Reference

        // begin TT#1391 - TMW New Action (Unrelated - Performance)
        //private void SetStoreMult(int storeRid, int sizeRid, int mult)
        //{
        //    ////SetValue(storeRid, sizeRid, mult, _storeMultHash);
        //}
        // end TT#1391 - TMW New Action (Unrelated - Performance)
        
        //public int GetStoreMult(int storeRid, int sizeRid) // MID Track 3492 Size Need with constraints not allocating correctly
        //public int GetStoreMult(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Traack 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
        public int GetStoreMult(int storeRid, int sizeRid) // TT#1391 - TMW New Action
		{
			//int mult = GetValue(storeRid, sizeRid, _storeMultHash); // MID Track 3492 Size Need with constraints not allocating correctly
            //int mult = GetValue(storeRid, aSizeCodeProfile, _storeMultHash); // MID Track 3492 Size Need with Constraints not allocating correctly
            int mult = GetValue(storeRid, sizeRid, _storeMulti);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
            return mult;
		}

        // begin TT#1391 - TMW New Action (Unrelated - Performance)
        ///// <summary>
        ///// Sets the value in the desired hashtable
        ///// </summary>
        ///// <param name="storeRid"></param>
        ///// <param name="sizeRid"></param>
        ///// <param name="units"></param>
        ///// <param name="aHashtable"></param>
        //private void SetValue(int storeRid, int sizeRid, int units, Hashtable aHashtable)
        //{
        //    try
        //    {
        //        if (!aHashtable.ContainsKey(storeRid))
        //        {
        //            Hashtable ht = new Hashtable();
        //            ht.Add(sizeRid, units);
        //            aHashtable.Add(storeRid, ht);
        //        }
        //        else
        //        {
        //            Hashtable ht = (Hashtable)aHashtable[storeRid];
        //            if (ht.ContainsKey(sizeRid))
        //            {
        //                //int prevUnits = (int)ht[sizeRid];
        //                ht.Remove(sizeRid);
        //                //units += prevUnits;
        //            }
        //            ht.Add(sizeRid, units);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        // end TT#1391 - TMW New Action (Unrelated - Performance)

		//private int GetValue(int storeRid, int sizeRid, Hashtable aHashtable) // MID Track 3492 Size Need with constraints not allocating correctly
		// begin TT#1391 - TMW New Action (Unrelated Performance
        private int GetValue(int storeRid, int aSizeCodeRID, Dictionary<int, StoreSizeVector> aMinMaxMultiDict)
        {
            StoreSizeVector aMinMaxMultiVector;
            //int sizeRid = aSizeCodeProfile.Key;
            if (!aMinMaxMultiDict.TryGetValue(aSizeCodeRID, out aMinMaxMultiVector))
            {
                SetStoreMinMaxMult(aSizeCodeRID);
                aMinMaxMultiVector = aMinMaxMultiDict[aSizeCodeRID];
            }
            return (int)aMinMaxMultiVector.GetStoreValue(storeRid);
        }
        //private int GetValue(int storeRid, SizeCodeProfile aSizeCodeProfile, Hashtable aHashtable) // MID Track 3492 Size Need with constraints not allocating correctly
        //{
        //    try
        //    {
        //        int aValue = 0;
        //        if (aHashtable.ContainsKey(storeRid))
        //        {
        //            int sizeRid = aSizeCodeProfile.Key;
        //            Hashtable ht = (Hashtable)aHashtable[storeRid];
        //            if (ht.ContainsKey(sizeRid))
        //            {
        //                aValue = (int)ht[sizeRid];
        //            }
        //            else
        //            {
        //                //SetStoreMinMaxMult(sizeRid, storeRid); // MID Track 3492 Size Need with constraints not allocating correctly
        //                //aValue = GetValue(storeRid, sizeRid, aHashtable); // MID Track 3492 Size Need with Constraints not allocating correctly
        //                SetStoreMinMaxMult(aSizeCodeProfile, storeRid); // MID Track 3492 Size Need with constraints not allocating correctly
        //                aValue = GetValue(storeRid, aSizeCodeProfile, aHashtable); // MID Track 3492 Size Need with constraints not allocating correctly
        //            }
        //        }
        //        else
        //        {
        //            //SetStoreMinMaxMult(sizeRid, storeRid); // MID Track 3492 Size Need with constraints not allocating correctly
        //            //aValue = GetValue(storeRid, sizeRid, aHashtable); // MID Track 3492 Size Need with Constraints not allocating correctly
        //            SetStoreMinMaxMult(aSizeCodeProfile, storeRid); // MID Track 3492 Size Need with constraints not allocating correctly
        //            aValue = GetValue(storeRid, aSizeCodeProfile, aHashtable); // MID Track 3492 Size Need with constraints not allocating correctly
        //        }
        //        return aValue;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        // end TT#1391 - TMW New Action - (Unrelated Performance)

		/// <summary>
		/// Sets the min, max and mult for the store / size.
		/// If a rule min/max was declared, it overrides anything we get from the allocation profile.
		/// </summary>
		/// <param name="aSizeCodeProfile">Profile that describes the size</param>
		/// <param name="storeRid">RID that identifies the store</param>
		//private void SetStoreMinMaxMult(int sizeRid, int storeRid) // MID Track 3492 Size Need with constraints Not allocating correctly
        //private void SetStoreMinMaxMult(SizeCodeProfile aSizeCodeProfile, int storeRid)	 // MID Track 3492 Size Need with constraints Not allocating correctly
        private void SetStoreMinMaxMult(int aSizeCodeRID)	 // TT#1391 - TMW New Action
        {
			int mult = 1;
			int min=0;
			int max = int.MaxValue;

			//==========================================================
			// we do not do this lookup for the dummy color (packs).
			// (allocProfile calls can't find color in bulk.)
			//==========================================================
            //int sizeRid = aSizeCodeProfile.Key; // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
            if (_colorRid != Include.DummyColorRID)
            { 
                // begin TT#1391 - TMW New Action (Max should be SET by this object NOT retrieved from Allocation Profile)
                StoreSizeVector storeMinVector = new StoreSizeVector(aSizeCodeRID);
                StoreSizeVector storeMaxVector = new StoreSizeVector(aSizeCodeRID);
                StoreSizeVector storeMultiVector = new StoreSizeVector(aSizeCodeRID);
                StoreSizeVector storeCapMaxVector = new StoreSizeVector(aSizeCodeRID); // TT#2155 - JEllis - Fill Size Holes Null Reference
                HdrColorBin hcb = _allocProfile.GetHdrColorBin(_colorRid);
                //HdrSizeBin hsb = hcb.GetSizeBin(aSizeCodeRID);  // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                HdrSizeBin hsb = (HdrSizeBin)hcb.ColorSizes[aSizeCodeRID];    // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                Index_RID[] storeIdxRIDArray = _allocProfile.AppSessionTransaction.StoreIndexRIDArray();
                AllocationColorOrSizeComponent colorComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, _colorRid); // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
                if (_constraintDecoder != null)
                {
                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                    {
                        int sglRid = (int)_constraintStoreGroupLevelHash[storeIdxRID.RID];
                        MinMaxItemBase minMax = (MinMaxItemBase)_constraintDecoder.GetItem(sglRid, _colorRid, aSizeCodeRID);
                        min = minMax.Min;
                        max = minMax.Max;
                        mult = 1;           // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                        if (hsb != null)    // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                        {                   // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                            // begin TT#1478 - Size Multiple Broken
                            //mult = Math.Max(1, minMax.Mult); 
                            if (minMax.Mult > 0)
                            {
                                mult = minMax.Mult;
                            }
                            else if (hsb.SizeMultiple > 0)
                            {
                                mult = hsb.SizeMultiple;
                            }
                            else
                            {
                                mult = 1;
                            }
                            // end TT#1478 - Size Multiple Broken
                            int primMax = _allocProfile.GetStorePrimaryMaximum(hsb, storeIdxRID);
                            max = Math.Min(max, primMax);
                            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                            //if (_useStoreCapacity)
                            //{
                            //    int cap = _allocProfile.GetStoreCapacityMaximum(storeIdxRID);
                            //    max = Math.Min(max, cap);
                            //}
                            // end TT#2155 - JEllis - Fill Size Holes Null Reference
                        }    // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                        storeMinVector.SetStoreValue(storeIdxRID.RID, min);
                        storeMaxVector.SetStoreValue(storeIdxRID.RID, max);
                        storeMultiVector.SetStoreValue(storeIdxRID.RID, mult);
                        storeCapMaxVector.SetStoreValue(storeIdxRID.RID, Math.Min(max, _allocProfile.GetStoreCapacityMaximum(colorComponent, storeIdxRID, true))); // TT#2155 - JEllis - Fill Size Holes Null Reference // TT#1074 - MD - Jellis - Group Allocation Inventory Min max broken
                        //storeCapMaxVector.SetStoreValue(storeIdxRID.RID, Math.Min(max, _allocProfile.GetStoreCapacityMaximum(storeIdxRID))); // TT#2155 - JEllis - Fill Size Holes Null Reference
                    }
                }
                else
                {
                    // begin TT#1478 - Size Multiple Broken
                    //if (hsb.SizeMultiple > 0)  // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                    if (hsb != null && hsb.SizeMultiple > 0)  // TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                    {
                        mult = hsb.SizeMultiple;
                    }
                    else
                    {
                        mult = 1;
                    }
                    // end TT#1478 - Size Multiple Broken
                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                    {
                        // Begin  TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                        //int primMax = _allocProfile.GetStorePrimaryMaximum(hsb, storeIdxRID);  
                        int primMax = int.MaxValue;  
                        if (hsb != null)  
                        {
                            primMax = _allocProfile.GetStorePrimaryMaximum(hsb, storeIdxRID);
                        }
                        // End TT#3539 - JSmith - Action Failed When Using Fill to Size Plan + Size Mins, Full Size-Run is Not Present
                        //max = Math.Min(max, primMax);  // TT#1478 - Size Multiple Broken
                        max = Math.Min(int.MaxValue, primMax); // TT#1478 - Size Multiple Broken
                        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                        //if (_useStoreCapacity)
                        //{
                        //    int cap = _allocProfile.GetStoreCapacityMaximum(storeIdxRID);
                        //    max = Math.Min(max, cap);
                        //}
                        // end TT#2155 - JEllis - Fill Size Holes Null Reference
                        storeMinVector.SetStoreValue(storeIdxRID.RID, min);
                        storeMaxVector.SetStoreValue(storeIdxRID.RID, max);
                        storeMultiVector.SetStoreValue(storeIdxRID.RID, mult);
                        storeCapMaxVector.SetStoreValue(storeIdxRID.RID, Math.Min(max, _allocProfile.GetStoreCapacityMaximum(colorComponent, storeIdxRID, true))); // TT#2155 - JEllis - Fill Size Holes Null Reference // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
                        //storeCapMaxVector.SetStoreValue(storeIdxRID.RID, Math.Min(max, _allocProfile.GetStoreCapacityMaximum(storeIdxRID))); // TT#2155 - JEllis - Fill Size Holes Null Reference // TT#1074 -  MD - Jellis - Group Allocation Inventory Min max broken
                    }
                }
                _storeMin.Add(storeMinVector.SizeCodeRID, storeMinVector);
                _storeMax.Add(storeMaxVector.SizeCodeRID, storeMaxVector);
                _storeMulti.Add(storeMaxVector.SizeCodeRID, storeMultiVector);
                _storeCapMax.Add(storeCapMaxVector.SizeCodeRID, storeCapMaxVector); // TT#2155 - JEllis - Fill Size Holes Null Reference
            }
        }
    }
} 
                //if (_constraintDecoder != null)
                //{
                //    //int sglRid = (int)_constraintStoreGroupLevelHash[storeRid];
                //    //MinMaxItemBase minMax = (MinMaxItemBase)_constraintDecoder.GetItem(sglRid, _colorRid ,sizeRid); // MID Track 3492 Size Need with constraints Not allocating correctly
                //    // begin MID Track 5593 Size Maximum not observed by Size Need
                //    //MinMaxItemBase minMax = (MinMaxItemBase) _constraintDecoder.GetItem(storeRid, _colorRid, aSizeCodeProfile); // MID Track 3492 Size Need with Constraints Not allocating correctly
                //    int sglRid = (int)_constraintStoreGroupLevelHash[storeRid];
                //    MinMaxItemBase minMax = (MinMaxItemBase)_constraintDecoder.GetItem(sglRid, _colorRid, aSizeCodeProfile); // MID Track 3492 Size Need with Constraints Not allocating correctly
                //    // end MID Track 5593 Size Maximum not observed by Size Need
                //    min = minMax.Min;
                //    max = minMax.Max;
                //    mult = Math.Max(1, minMax.Mult);  // TT#1042 Size Constraint Balance MUST preserve minimums
                //}
                //if (min == 0)
                //{
                //    min = _allocProfile.GetStoreMinimum(_colorRid, sizeRid, storeRid);
                //}
                //if (max == int.MaxValue)
                //{
                //    int allocMax = _allocProfile.GetStoreMaximum(_colorRid, sizeRid, storeRid); 
                //    //int primMax = _allocProfile.GetStorePrimaryMaximum(_colorRid, sizeRid, storeRid); // MID Track 3492 Size Need with constraint not allocating correctly
                //    max = Math.Min(allocMax, primMax); 
                //    if (_useStoreCapacity)
                //    {
                //        int cap = _allocProfile.GetStoreCapacityMaximum(storeRid);
                //        max = Math.Min(max, cap);
                //    }
                //}
                //}
                //SetStoreMult(storeRid, sizeRid, mult);
                //SetStoreMin(storeRid, sizeRid, min);
                //SetStoreMax(storeRid, sizeRid, max);
//        }
//    }
//}
            // end TT#1391 - TMW New Action 

