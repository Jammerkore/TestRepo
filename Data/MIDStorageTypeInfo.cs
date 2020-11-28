//using System;
//using System.Globalization;
//using System.Data;
//using System.IO;  
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    #region MIDStorageTypeDataAccess
//    /// <summary>
//    /// Used to access database from static class MIDStorageTypeInfo
//    /// </summary>
//    public class MIDStorageTypeDataAccess : DataLayer
//    {
//        public MIDStorageTypeDataAccess()
//        {
//        }
//    }
//    #endregion MIDStorageTypeDataAccess

//    #region MIDStorageTypeInfo (a static class)
//    /// <summary>
//    /// Static class to hold MID Storage Type information
//    /// </summary>
//    [Serializable]
//    public static class MIDStorageTypeInfo
//    {
//        #region Fields
//        private static int      _count = 0;  // TT#707 - JEllis - Container not thread safe (Part 2)
//        private static int      _storeMAX_RID;
//        private static Int64[]  _storageTypeMinimum;
//        private static Int64[]  _storageTypeMaximum;
//        private static byte[]   _storageTypeSize;
//        private static Type[]   _storageType;
//        private static object[] _storageDefaultValue;
//        private static int[]    _variableMaxIDX;
//        private static object _MIDStorageTypeInfoLock = new Object();  // TT#707 - JEllis - Container not thread safe
//        #endregion Fields

//        #region Static Constructor
//        static MIDStorageTypeInfo()
//        {
//            lock (_MIDStorageTypeInfoLock)        // TT#707 - JEllis - Container not thread safe
//            {                                     // TT#707 - JEllis - Container not thread safe 
//                if (_count == 0)                  // TT#707 - JEllis - Container not thread safe (part 2)
//                {                                 // TT#707 - JEllis - Container not thread safe (part 2)
//                    _count = Enum.GetValues(typeof(eMIDStorageTypeCode)).Length;
//                    _storageTypeMinimum = new Int64[_count];
//                    _storageTypeMaximum = new Int64[_count];
//                    _storageTypeSize = new byte[_count];
//                    _storageType = new Type[_count];
//                    _storageDefaultValue = new Object[_count];

//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.Empty, typeof(Nullable), 0, 0, 0, 0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeSByte, typeof(sbyte), sbyte.MinValue, (Int64)sbyte.MaxValue, 1, (sbyte)0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeByte, typeof(byte), byte.MinValue, (Int64)byte.MaxValue, 1, (byte)0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeShort, typeof(Int16), Int16.MinValue, (Int64)Int16.MaxValue, 2, (Int16)0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeUshort, typeof(UInt16), UInt16.MinValue, (Int64)UInt16.MaxValue, 2, (UInt16)0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeInt, typeof(Int32), Int32.MinValue, (Int64)Int32.MaxValue, 4, (Int32)0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeUint, typeof(UInt32), UInt32.MinValue, (Int64)UInt32.MaxValue, 4, (UInt32)0);
//                    StorageTypeInfoInitialization((int)eMIDStorageTypeCode.typeLong, typeof(Int64), Int64.MinValue, (Int64)Int64.MaxValue, 8, (Int64)0);
//                    _storeMAX_RID = 0;
//                    _variableMaxIDX = new int[(Enum.GetValues(typeof(eMIDVariableModelType)).Length)];
//                    for (int i = 0; i < _variableMaxIDX.Length; i++)
//                    {
//                        _variableMaxIDX[i] = -1;
//                    }
//                }                            // TT#707 - JEllis - Container not thread safe (part 2)
//            }                                // TT#707 - JEllis - Container not thread safe
//        }
//        /// <summary>
//        /// Initializes 
//        /// </summary>
//        /// <param name="aTypeIndex"></param>
//        /// <param name="aType"></param>
//        /// <param name="aMinimumValue"></param>
//        /// <param name="aMaximumValue"></param>
//        /// <param name="aTypeFieldSize"></param>
//        /// <param name="typeZeroValue"></param>
//        private static void StorageTypeInfoInitialization(int aTypeIndex, Type aType, Int64 aMinimumValue, Int64 aMaximumValue, byte aTypeFieldSize, object typeDefaultValue)
//        {
//            _storageType[aTypeIndex] = aType;
//            _storageTypeMinimum[aTypeIndex] = aMinimumValue;
//            _storageTypeMaximum[aTypeIndex] = aMaximumValue;
//            _storageTypeSize[aTypeIndex] = aTypeFieldSize;
//            _storageDefaultValue[aTypeIndex] = typeDefaultValue;
//        }
//        #endregion Static Constructor

//        #region Properties
//        /// <summary>
//        /// Gets the count of MID Storage types
//        /// </summary>
//        public static int Count
//        {
//            get { return _count; }
//        }
//        #endregion Properties

//        #region Methods

//        #region Type
//        /// <summary>
//        /// Gets the type of the MID storage type
//        /// </summary>
//        /// <param name="aTypeCode">MIDStorageTypeCode for which the Type is desired</param>
//        /// <returns>Type of the MIDStorageType</returns>
//        public static Type GetStorageType(eMIDStorageTypeCode aTypeCode)
//        {
//            return GetStorageType((int)aTypeCode);
//        }
//        /// <summary>
//        /// Gets the type of the MID storage type
//        /// </summary>
//        /// <param name="aTypeCode">eMIDStorageTypeCode cast as an integer for which the Type is desired</param>
//        /// <returns>Type of the MID Storage Type</returns>
//        public static Type GetStorageType(int aTypeCode)
//        {
//            return _storageType[aTypeCode];
//        }
//        #endregion Type

//        #region StorageTypeMinimumValue
//        /// <summary>
//        /// Get the minimum value for a variable storage type
//        /// </summary>
//        /// <param name="aTypeCode">eMIDStorageTypeCode cast as an integer for which the minimum value is desired</param>
//        /// <returns>Minimum value that the storage type can hold</returns>
//        public static Int64 GetStorageMinimumValue(eMIDStorageTypeCode aTypeCode)
//        {
//            return GetStorageMinimumValue((int)aTypeCode);
//        }
//        public static Int64 GetStorageMinimumValue(int aTypeCode)
//        {
//            return _storageTypeMinimum[aTypeCode];
//        }
//        #endregion StorageTypeMinimumValue

//        #region StorageTypeMaximumValue
//        /// <summary>
//        /// Get the maximum value for a variable storage type
//        /// </summary>
//        /// <param name="aTypeCode">Storage type code for which the maximum value is desired</param>
//        /// <returns>Maximum value that the storage type can hold</returns>
//        public static Int64 GetStorageMaximumValue(eMIDStorageTypeCode aTypeCode)
//        {
//            return GetStorageMaximumValue((int)aTypeCode);
//        }
//        /// <summary>
//        /// Get the maximum value for a variable storage type
//        /// </summary>
//        /// <param name="aTypeCode">eMIDStorageTypeCode cast as an integer for which the maximum value is desired</param>
//        /// <returns>Maximum value that the storage type can hold</returns>
//        public static Int64 GetStorageMaximumValue(int aTypeCode)
//        {
//            return _storageTypeMaximum[(int)aTypeCode];
//        }
//        #endregion StorageTypeMaximumValue

//        #region StorageTypeFieldSize
//        /// <summary>
//        /// Get the field size of the variable storage type
//        /// </summary>
//        /// <param name="aTypeCode">Type code for which the field size is desired</param>
//        /// <returns>Field size of the storage area</returns>
//        public static int GetStorageTypeFieldSize(eMIDStorageTypeCode aTypeCode)
//        {
//            return GetStorageTypeFieldSize((int)aTypeCode);
//        }
//        /// <summary>
//        /// Get the field size of the variable storage type
//        /// </summary>
//        /// <param name="aTypeCode">eMIDStorageTypeCode cast as an integer for which the field size is desired</param>
//        /// <returns>Field size of the storage area</returns>
//        public static int GetStorageTypeFieldSize(int aTypeCode)
//        {
//            return _storageTypeSize[aTypeCode];
//        }
//        #endregion StorageTypeFieldSize

//        #region StorageTypeDefaultValue
//        /// <summary>
//        /// Get the default value for the given storage type
//        /// </summary>
//        /// <param name="aTypeCode">eMIDStorageTypeCode cast as an integer for which the default value is desired</param>
//        /// <returns></returns>
//        public static object GetStorageTypeDefaultValue(int aTypeCode)
//        {
//            return _storageDefaultValue[aTypeCode];
//        }
//        /// <summary>
//        /// Get the default value for the given storage type
//        /// </summary>
//        /// <param name="aTypeCode">TypeCode for which the default value is desired</param>
//        /// <returns>The default value object for the given storage type</returns>
//        public static object GetStorageTypeDefaultValue(eMIDStorageTypeCode aTypeCode)
//        {
//            return GetStorageTypeDefaultValue((int)aTypeCode);
//        }
//        #endregion StorageTypeDefaultValue

//        #region Find StorageTypeCode
//        // begin TT#707 - JEllis - Container not thread safe (Part 2)
//        // this method IS thread safe because it only references local variables and passed in variables
//        //private static object _storageTypeCodeLock = new object();        // TT#707 - JEllis - Container not thread safe // part 2
//        /// <summary>
//        /// Find the best single storage type code that will hold the given storages values
//        /// </summary>
//        /// <param name="aNewStoreValue">A new store value that will be added to the existing array</param>
//        /// <param name="aStoreArray">An array of storage values for which a single storage type is desired</param>
//        /// <returns>A storage type that will hold each of the given values in the array</returns>
//        public static eMIDStorageTypeCode FindStorageTypeCode(Int64 aNewStoreValue, Array aStoreArray)
//            {                             // TT#707 - JEllis - Container not thread safe
//                Int64 maxValue = aNewStoreValue;
//                Int64 minValue = aNewStoreValue;
//                Int64 storeValue;
//                if (aStoreArray != null)
//                {
//                        for (int i = 0; i < aStoreArray.Length; i++)
//                        {
//                            storeValue = Convert.ToInt64(aStoreArray.GetValue(i));
//                            if (storeValue > maxValue)
//                            {
//                                maxValue = storeValue;
//                            }
//                            if (storeValue < minValue)
//                            {
//                                minValue = storeValue;
//                            }
//                        }
//                 }
//                return FindStorageTypeCode(minValue, maxValue);
//        }
//        //public static eMIDStorageTypeCode FindStorageTypeCode(Int64 aNewStoreValue, Array aStoreArray)
//        //{
//        //    lock (_storageTypeCodeLock)   // TT#707 - JEllis - Container not thread safe
//        //    {                             // TT#707 - JEllis - Container not thread safe
//        //        Int64 maxValue = aNewStoreValue;
//        //        Int64 minValue = aNewStoreValue;
//        //        Int64 storeValue;
//        //        if (aStoreArray != null)
//        //        {
//        //                for (int i = 0; i < aStoreArray.Length; i++)
//        //                {
//        //                    storeValue = Convert.ToInt64(aStoreArray.GetValue(i));
//        //                    if (storeValue > maxValue)
//        //                    {
//        //                        maxValue = storeValue;
//        //                    }
//        //                    if (storeValue < minValue)
//        //                    {
//        //                        minValue = storeValue;
//        //                    }
//        //                }
//        //         }
//        //        return FindStorageTypeCode(minValue, maxValue);
//        //    }                            // TT#707 - JEllis - Container not thread safe         
//        //}
//        // this method IS thread safe because it only uses local variables and references passed in variables
//        /// <summary>
//        /// Find the best storage type code that will hold the given storage value
//        /// </summary>
//        /// <param name="aMaxStorageValue">The largest value that requires storage</param>
//        /// <param name="aMinStorageValue">The smallest value that requires storage</param>
//        /// <returns>Returns the best storage type code that will hold the specified value</returns>
//        public static eMIDStorageTypeCode FindStorageTypeCode(Int64 aMinStorageValue, Int64 aMaxStorageValue)
//        {
//            eMIDStorageTypeCode typeCode = eMIDStorageTypeCode.Empty;
//            for (byte i = 1; i < MIDStorageTypeInfo.Count; i++)
//            {
//                if (aMinStorageValue >= 0
//                    && MIDStorageTypeInfo.GetStorageMinimumValue(i) < 0)
//                {
//                    continue;
//                }
//                if (aMaxStorageValue <= MIDStorageTypeInfo.GetStorageMaximumValue(i)
//                   && aMinStorageValue >= MIDStorageTypeInfo.GetStorageMinimumValue(i))
//                {
//                    typeCode = (eMIDStorageTypeCode)i;
//                    break;
//                }
//            }
//            if (typeCode == eMIDStorageTypeCode.Empty)
//            {
//                throw new Exception(
//                    "Cannot find a type code for store variable value where min='"
//                    + aMinStorageValue.ToString()
//                    + "' and max='"
//                    + aMaxStorageValue.ToString()
//                    + "'");
//            }
//            return typeCode;
//        }
//        //public static eMIDStorageTypeCode FindStorageTypeCode(Int64 aMinStorageValue, Int64 aMaxStorageValue)
//        //{
//        //    lock (_storageTypeCodeLock)   // TT#707 - JEllis - Container not thread safe
//        //    {                             // TT#707 - JEllis - Container not thread safe
//        //        eMIDStorageTypeCode typeCode = eMIDStorageTypeCode.Empty;
//        //        for (byte i = 1; i < MIDStorageTypeInfo.Count; i++)
//        //        {
//        //            if (aMinStorageValue >= 0
//        //                && MIDStorageTypeInfo.GetStorageMinimumValue(i) < 0)
//        //            {
//        //                continue;
//        //            }
//        //            if (aMaxStorageValue <= MIDStorageTypeInfo.GetStorageMaximumValue(i)
//        //               && aMinStorageValue >= MIDStorageTypeInfo.GetStorageMinimumValue(i))
//        //            {
//        //                typeCode = (eMIDStorageTypeCode)i;
//        //                break;
//        //            }
//        //        }
//        //        if (typeCode == eMIDStorageTypeCode.Empty)
//        //        {
//        //            throw new Exception(
//        //                "Cannot find a type code for store variable value where min='"
//        //                + aMinStorageValue.ToString()
//        //                + "' and max='"
//        //                + aMaxStorageValue.ToString()
//        //                + "'");
//        //        }
//        //        return typeCode;
//        //    }
//        //}
//        // end TT#707 - JEllis - Container not thread safe (Part 2)

//        #endregion Find StorageTypeCode

//        #region StoreMaxRID
//        /// <summary>
//        /// Gets the smaller of the largest Store RID defined on the database or the largest valid store RID yet encountered that is greater or equal to the specified store RID
//        /// </summary>
//        /// <param name="aStoreRID">Specified Store RID</param>
//        /// <returns>A vaild store RID that is larger than the specified store RID</returns>
//        /// <remarks>A store RID is valid if its value is less than the largest store RID defined on the database.</remarks>
//        public static int GetStoreMaxRID(int aStoreRID)
//        {
//            if (aStoreRID > _storeMAX_RID
//                || _storeMAX_RID == 0)
//            {
//                ReadStoreMaxRID();
//                if (aStoreRID > _storeMAX_RID)
//                {
//                    throw new Exception("Store RID = '" + _storeMAX_RID.ToString() + "' is larger than MAX Store RID '" + _storeMAX_RID.ToString());
//                }

//            }
//            return Math.Max(_storeMAX_RID, aStoreRID);
//        }
//        private static object _readStoreMaxRIDLock = new object();        // TT#707 - JEllis - Container not thread safe
//        /// <summary>
//        /// Get the largest store RID defined on the database
//        /// </summary>
//        private static void ReadStoreMaxRID()
//        {
//            lock (_readStoreMaxRIDLock)        // TT#707 - JEllis - Container not thread safe
//            {
//                MIDStorageTypeDataAccess dataAccess = new MIDStorageTypeDataAccess();
//                string SQLCommand = "select MAX(ST_RID) as MAX_STORE_RID from STORES";
//                DataTable dt = dataAccess._dba.ExecuteSQLQuery(SQLCommand, "GET_MAX_STORE_RID");
//                if (dt.Rows.Count > 0)
//                {
//                    _storeMAX_RID = Convert.ToInt32(dt.Rows[0]["MAX_STORE_RID"]);
//                }
//            }        // TT#707 - JEllis - Container not thread safe
//        }
//        #endregion StoreMaxRID

//        #region VariableMaxIDX
//        /// <summary>
//        /// Get the smaller of the largest variable IDX defined for the specified variable model type or the largest variable IDX yet encountered that is greater or equal to the specified variable ID
//        /// </summary>
//        /// <param name="aVariableModelType">Variable Model Type</param>
//        /// <param name="aVariableIDX">Variable ID to test for range</param>
//        /// <returns>Largest Variable IDX for the givein Variable Model Type that is greater or equal to the given variable IDX</returns>
//        public static int GetVariableMaxIDX(eMIDVariableModelType aVariableModelType, int aVariableIDX)
//        {
//            int variableTypeIDX = (int)aVariableModelType - 1;
//            if (aVariableIDX > _variableMaxIDX[variableTypeIDX])
//            {
//                if (_variableMaxIDX[variableTypeIDX] < 0)
//                {
//                    ReadVariableMaxIDX(aVariableModelType);
//                }
//                if (aVariableIDX > _variableMaxIDX[variableTypeIDX]
//                    || aVariableIDX < 0)
//                {
//                    throw new Exception(
//                        Enum.GetName(typeof(eMIDVariableModelType),
//                        aVariableModelType) + " variable IDX='"
//                        + aVariableIDX.ToString()
//                        + "' is not defined in the VARIABLE_MODEL table");
//                }
//            }
//            return _variableMaxIDX[variableTypeIDX];
//        }
//        private static object _variableMaxIDXLock = new object();        // TT#707 - JEllis - Container not thread safe
//        /// <summary>
//        /// Get maximum variable IDX code for the given variable model type
//        /// </summary>
//        /// <param name="aVariableModelType">Variable Model Type for which the maximum varible ID code is required</param>
//        private static void ReadVariableMaxIDX(eMIDVariableModelType aVariableModelType)
//        {
//            lock (_variableMaxIDXLock)        // TT#707 - JEllis - Container not thread safe
//            {
//                if (_variableMaxIDX[(int)aVariableModelType - 1] < 0) // TT#707 - JEllis - Container not thread safe (Part 2)
//                {                                                     // TT#707 - JEllis - Container not thread safe (Part 2)
//                    MIDStorageTypeDataAccess dataAccess = new MIDStorageTypeDataAccess();
//                    string SQLCommand = "select MAX(VARIABLE_IDX) as MAX_VARIABLE_IDX from VARIABLE_MODEL_ENTRY WHERE VARIABLE_MODEL_RID=" + ((int)aVariableModelType).ToString();
//                    DataTable dt = dataAccess._dba.ExecuteSQLQuery(SQLCommand, "GET_MAX_VARIABLE_IDX");
//                    if (dt.Rows.Count > 0)
//                    {
//                        _variableMaxIDX[(int)aVariableModelType - 1] = Convert.ToInt32(dt.Rows[0]["MAX_VARIABLE_IDX"]);
//                    }
//                }                                                     // TT#707 - JEllis - Container not thread safe (Part 2)
//            }        // TT#707 - JEllis - Container not thread safe
//        }
//        #endregion VariableMaxIDX

//        #endregion Methods
//    }
//    #endregion MIDStorageTypeInfo (a static class)
//}