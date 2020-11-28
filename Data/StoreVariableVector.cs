//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Runtime.Serialization; 
//using System.Runtime.Serialization.Formatters.Binary; 
//using System.Data;
//using System.IO;  
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    /// <summary>
//    /// Container for store values of a single variable type
//    /// </summary>
//    [Serializable]
//    public struct StoreVariableVector
//    {
//        #region Fields
//        internal int    _baseOffset;
//        internal short  _baseLength; 
//        internal byte   _storageTypeCode;
//        internal byte   _status;
//        internal short  _hiStrRID;
//        internal short  _variableIDX;
//        internal Array  _storeArray;

//        #endregion Fields

//        #region Constructors
//        /// <summary>
//        /// Creates an instance of the StoreVariableVector
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index identifying location within the associated StoreVariableVectorArray where this vector resides</param>
//        public StoreVariableVector(short aVariableIDX)
//        {
//            _variableIDX = aVariableIDX;
//            _storageTypeCode = (byte)eMIDStorageTypeCode.Empty;
//            _hiStrRID = 0;
//            _baseOffset = 0;
//            _baseLength = 0;
//            _status = 0;
//            _storeArray = null;
//        }
//        public StoreVariableVector(short aVariableIDX, Int64[] aStoreVariableValue)
//        {
//            _variableIDX = aVariableIDX;
//            _storageTypeCode = (byte)MIDStorageTypeInfo.FindStorageTypeCode(0, aStoreVariableValue);
//            _hiStrRID = (short)aStoreVariableValue.Length;
//            _baseOffset = 0;
//            _baseLength = 0;
//            _status = 0;
//            Type type = MIDStorageTypeInfo.GetStorageType((eMIDStorageTypeCode)_storageTypeCode);
//            _storeArray = Array.CreateInstance(type, _hiStrRID);
//            for (int i = 0; i < _hiStrRID; i++)
//            {
//                _storeArray.SetValue(Convert.ChangeType(aStoreVariableValue[i], type), i);
//            }
//        }
//        /// <summary>
//        /// Creates an instance of this vector
//        /// </summary>
//        /// <param name="aStoreVariableVector">Store Variable Vector to use to initialize this vector</param>
//        public StoreVariableVector(StoreVariableVector aStoreVariableVector)
//        {
//            _variableIDX = aStoreVariableVector.VariableIDX;
//            _storageTypeCode = (byte)aStoreVariableVector.StorageTypeCode;
//            _hiStrRID = aStoreVariableVector._hiStrRID;
//            _baseOffset = aStoreVariableVector._baseOffset;
//            _baseLength = aStoreVariableVector._baseLength;
//            _status = aStoreVariableVector._status;
//            _storeArray = Array.CreateInstance(MIDStorageTypeInfo.GetStorageType(_storageTypeCode), aStoreVariableVector.StoreCount);
//            Array.Copy(aStoreVariableVector._storeArray, _storeArray, aStoreVariableVector.StoreCount);
//        }
//        /// <summary>
//        /// Creates an instance of this vector
//        /// </summary>
//        /// <param name="aVariableIDX">Variable IDX</param>
//        /// <param name="aStorageTypeCode">eMIDStorageTypeCode cast as a byte</param>
//        /// <param name="aArrayCount">Count of stores</param>
//        /// <param name="aStartByteArray">Starting position of this vector in the byte array of which this vector is a part</param>
//        /// <param name="aEndByteArray">Ending position of this vector in the byte array of which this vector is a part</param>
//        /// <param name="aStoreVariableVector"></param>
//        public StoreVariableVector(short aVariableIDX, byte aStorageTypeCode, byte aStatus, short aHiStoreRID, int aBaseOffset, short aBaseLength, Array aStoreVariableVector)
//        {
//            _variableIDX = aVariableIDX;
//            _storageTypeCode = aStorageTypeCode;
//            _status = aStatus;
//            _hiStrRID = aHiStoreRID;
//            _baseOffset = aBaseOffset;
//            _baseLength = aBaseLength;
//            _storeArray = aStoreVariableVector;
//        }
//        #endregion Constructors

//        #region Properties
//        /// <summary>
//        /// Gets the variable IDX for this vector
//        /// </summary>
//        public short VariableIDX
//        {
//            get { return _variableIDX; }
//            set { _variableIDX = value; }
//        }

//        /// <summary>
//        /// Gets the storage type code for this vector
//        /// </summary>
//        public eMIDStorageTypeCode StorageTypeCode
//        {
//            get { return (eMIDStorageTypeCode)_storageTypeCode; }
//            set { _storageTypeCode = (byte)value; }
//        }
//        /// <summary>
//        /// Gets the count of stores in this vector
//        /// </summary>
//        public short StoreCount
//        {
//            get
//            {
//                return _hiStrRID;
//            }
//            set
//            {
//                _hiStrRID = value;
//            }
//        }
//        /// <summary>
//        /// Gets the Largest Store RID within the vector
//        /// </summary>
//        public int LargestStoreRID
//        {
//            get
//            {
//                return _hiStrRID;
//            }
//        }
//        public bool IsCompressed
//        {
//            get { return (StoreCount > 0 && _storeArray == null); }
//        }
//        public bool IsExpanded
//        {
//            get { return !(_storeArray == null); }
//        }
//        #endregion Properties 

//        #region Methods

//        #region GetStoreVariableValue
//        /// <summary>
//        /// Gets the store's variable value as it exists in memory (byte, sbyte, etc.)
//        /// </summary>
//        /// <param name="aStoreRID">RID identifying the store</param>
//        /// <returns>Store's variable value</returns>
//        private object GetStorageTypeDefaultValue(int aStoreRID)
//        {
//            if (_storeArray == null
//                || aStoreRID > LargestStoreRID
//                || aStoreRID < 1)
//            {
//                return MIDStorageTypeInfo.GetStorageTypeDefaultValue(_storageTypeCode);
//            }
//            return _storeArray.GetValue(aStoreRID - 1);
//        }
//        /// <summary>
//        /// Gets the store's variable value cast as a 64-bit integer.
//        /// </summary>
//        /// <param name="aStoreRID">RID identifying the store</param>
//        /// <returns>Store's variable value cast as a 64-bit integer.</returns>
//        public Int64 GetStoreVariableValue(int aStoreRID)
//        {
//            return Convert.ToInt64(GetStorageTypeDefaultValue(aStoreRID));
//        }
//        #endregion GetStoreVariableValue

//        #region Operator +
//        /// <summary>
//        /// Combines 2 StoreVariableVectors
//        /// </summary>
//        /// <param name="A">1st StoreVariableVector</param>
//        /// <param name="B">2nd StoreVariableVector</param>
//        /// <returns></returns>
//        public static StoreVariableVector operator +(StoreVariableVector A, StoreVariableVector B)
//        {
//            if (((StoreVariableVector)A).VariableIDX != ((StoreVariableVector)B).VariableIDX)
//            {
//                throw new Exception("'+' operator requires StoreVariableVectors have same VaraibleIDX");
//            }
//            StoreVariableVector C = new StoreVariableVector((StoreVariableVector)A);
//            int count = Math.Max(C.StoreCount, B.StoreCount) + 1;  // Convert to STORE RID by adding 1
//            Int64[] intermediateValue = new Int64[count];
//            for (int i = 1; i < count; i++)
//            {
//                intermediateValue[i] = C.GetStoreVariableValue(i) + B.GetStoreVariableValue(i);
//               // C.SetStoreVariableValue(i,C, C.VariableIDX, C.GetStoreVariableValue(i) + B.GetStoreVariableValue(i));
//            }

//            return new StoreVariableVector(A.VariableIDX, intermediateValue);
//        }
//        /// <summary>
//        /// Combines 2 Nullable StoreVariableVectors
//        /// </summary>
//        /// <param name="A">1st StoreVariableVector (can be null)</param>
//        /// <param name="B">2nd StoreVariableVector (can be null)</param>
//        /// <returns>StoreVariableVector with the combined values (will be null if both A and B were null)</returns>
//        public static Nullable<StoreVariableVector> operator +(Nullable<StoreVariableVector> A, Nullable<StoreVariableVector> B)
//        {
//            if (A == null)
//            {
//                if (B == null)
//                {
//                    return null;
//                }
//                return new StoreVariableVector((StoreVariableVector)B);
//            }
//            if (B == null)
//            {
//                return new StoreVariableVector((StoreVariableVector)A);
//            }
//            return (StoreVariableVector)A + (StoreVariableVector)B;
//        }
//        #endregion Operator +

//        #endregion Methods
//    }

//}