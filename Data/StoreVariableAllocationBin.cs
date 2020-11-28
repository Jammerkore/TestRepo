//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    #region StoreAllocation
//    /// <summary>
//    /// Container for Store Allocation Variables.
//    /// </summary>
//    [Serializable]
//    public class StoreVariableAllocationBin : IDisposable
//    {
//        private bool _isDisposed;
//        private Dictionary<int, bool> _isHeaderCached;
//        // begin   TT#707 - Container not thread safe (part 2) JEllis
//        private TotalAllocationDatabaseBin _totalAllocationDatabaseBin;
//        private DetailAllocationDatabaseBin _detailAllocationDatabaseBin;
//        private PackAllocationDatabaseBin _packAllocationDatabaseBin;
//        private BulkAllocationDatabaseBin _bulkAllocationDatabaseBin;
//        private ColorAllocationDatabaseBin _colorAllocationDatabaseBin;
//        private SizeAllocationDatabaseBin _sizeAllocationDatabaseBin;
//        // end     TT#707 - Container not thread safe (part 2) JEllis
//        private StoreVariableDataDictionary<HeaderDatabaseBinKey> _totalAllocationStoreVariableContainer;
//        private StoreVariableDataDictionary<HeaderPackDatabaseBinKey> _packAllocationStoreVariableContainer;
//        private StoreVariableDataDictionary<HeaderColorDatabaseBinKey> _colorAllocationStoreVariableContainer;
//        private StoreVariableDataDictionary<HeaderColorSizeDatabaseBinKey> _sizeAllocationStoreVariableContainer;
//        private HeaderDatabaseBinKey _lastHeaderTotalDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStoreTotalVectorContainer;
//        private HeaderDatabaseBinKey _lastHeaderDetailDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStoreDetailVectorContainer;
//        private HeaderDatabaseBinKey _lastHeaderBulkDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStoreBulkVectorContainer;
//        private HeaderPackDatabaseBinKey _lastPackDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStorePackVectorContainer;
//        private HeaderColorDatabaseBinKey _lastColorDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStoreColorVectorContainer;
//        private HeaderColorSizeDatabaseBinKey _lastSizeDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStoreSizeVectorContainer;

//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        public StoreVariableAllocationBin()
//        {
//            _isDisposed = false;
//            _isHeaderCached = new Dictionary<int, bool>();
//            _totalAllocationStoreVariableContainer = new StoreVariableDataDictionary<HeaderDatabaseBinKey>();
//            _packAllocationStoreVariableContainer = new StoreVariableDataDictionary<HeaderPackDatabaseBinKey>();
//            _colorAllocationStoreVariableContainer = new StoreVariableDataDictionary<HeaderColorDatabaseBinKey>();
//            _sizeAllocationStoreVariableContainer = new StoreVariableDataDictionary<HeaderColorSizeDatabaseBinKey>();
//        }

//        #region StoreVariableValue

//        #region GetStoreVariableValue
//        #region GetStoreTotalValue
//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's total level value.</returns>
//        public double GetStoreTotalValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreTotalValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's total level value.</returns>
//        public double GetStoreTotalValue
//            (int aVariableID,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreTotalValue
//                (aVariableID,
//                aHdrRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's total level value.</returns>
//        public double GetStoreTotalValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreTotalValue
//                (aVariableIDX,
//                aHdrRID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's total level value.</returns>
//        public double[] GetStoreTotalValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRID)
//        {
//            return GetStoreTotalValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's total level value.</returns>
//        public double[] GetStoreTotalValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetTotalAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreTotalValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetTotalAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's total level value.</returns>
//        public double[] GetStoreTotalValue
//          (int aVariableID,
//           int aHdrRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetTotalAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreTotalValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetTotalAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's Total level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's total level value.</returns>
//        public double[] GetStoreTotalValue
//           (short aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreTotalVectorContainer(aHdrRID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetTotalAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }
//        #endregion GetStoreTotalValue

//        #region GetStoreDetailValue
//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double GetStoreDetailValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreDetailValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double GetStoreDetailValue
//            (int aVariableID,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreDetailValue
//                (aVariableID,
//                aHdrRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double GetStoreDetailValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreDetailValue
//                (aVariableIDX,
//                aHdrRID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double[] GetStoreDetailValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRID)
//        {
//            return GetStoreDetailValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double[] GetStoreDetailValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetDetailAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreDetailValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetDetailAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double[] GetStoreDetailValue
//          (int aVariableID,
//           int aHdrRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetDetailAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreDetailValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetDetailAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's Detail level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Detail level value.</returns>
//        public double[] GetStoreDetailValue
//           (short aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreDetailVectorContainer(aHdrRID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetDetailAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }
//        #endregion GetStoreDetailValue

//        #region GetStoreBulkValue
//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double GetStoreBulkValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreBulkValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double GetStoreBulkValue
//            (int aVariableID,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreBulkValue
//                (aVariableID,
//                aHdrRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double GetStoreBulkValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreBulkValue
//                (aVariableIDX,
//                aHdrRID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double[] GetStoreBulkValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRID)
//        {
//            return GetStoreBulkValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double[] GetStoreBulkValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetBulkAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreBulkValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetBulkAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double[] GetStoreBulkValue
//          (int aVariableID,
//           int aHdrRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetBulkAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreBulkValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetBulkAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's Bulk level value for the given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Bulk level value.</returns>
//        public double[] GetStoreBulkValue
//           (short aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreBulkVectorContainer(aHdrRID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetBulkAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }
//        #endregion GetStoreBulkValue

//        #region GetStorePackValue
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable pack value.</returns>
//        public double GetStorePackValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdrPackRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStorePackValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHdrPackRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Pack value.</returns>
//        public double GetStorePackValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdrPackRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStorePackValue
//                (aVariableID,
//                aHdrRID,
//                aHdrPackRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Pack value.</returns>
//        public double GetStorePackValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdrPackRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStorePackValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdrPackRID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Pack value.</returns>
//        public double[] GetStorePackValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRID)
//        {
//            return GetStorePackValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHdrPackRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Pack value.</returns>
//        public double[] GetStorePackValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHdrPackRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetPackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetPackAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and pack        
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Pack value.</returns>
//        public double[] GetStorePackValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHdrPackRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetPackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetPackAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and pack        
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Pack value.</returns>
//        public double[] GetStorePackValue
//           (short aVariableIDX,
//            int aHdrRID,
//           int aHdrPackRID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStorePackVectorContainer(aHdrRID, aHdrPackRID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetPackAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }
//        #endregion GetStorePackValue

//        #region GetStoreColorValue
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable color value.</returns>
//        public double GetStoreColorValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHdr_BC_RID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color value.</returns>
//        public double GetStoreColorValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorValue
//                (aVariableID,
//                aHdrRID,
//                aHdr_BC_RID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color value.</returns>
//        public double GetStoreColorValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color value.</returns>
//        public double[] GetStoreColorValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRID)
//        {
//            return GetStoreColorValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color value.</returns>
//        public double[] GetStoreColorValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetColorAllocationDatabaseBin().BinTableName + "]");   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color       
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color value.</returns>
//        public double[] GetStoreColorValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetColorAllocationDatabaseBin().BinTableName + "]");  // TT#707 - Container not thread safe (part 2) JEllis
//        } 
//        /// <summary>
//        /// Gets the store's value for the given header and Color        
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color value.</returns>
//        public double[] GetStoreColorValue
//           (short aVariableIDX,
//            int aHdrRID,
//           int aHdr_BC_RID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreColorVectorContainer(aHdrRID, aHdr_BC_RID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetColorAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }

//        #endregion GetStoreColorValue

//        #region GetStoreColorSizeValue
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable color-size value.</returns>
//        public double GetStoreColorSizeValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorSizeValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color-Size value.</returns>
//        public double GetStoreColorSizeValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorSizeValue
//                (aVariableID,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color-Size value.</returns>
//        public double GetStoreColorSizeValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorSizeValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color-size value.</returns>
//        public double[] GetStoreColorSizeValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRID)
//        {
//            return GetStoreColorSizeValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color-Size value.</returns>
//        public double[] GetStoreColorSizeValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int aHdr_BCSZ_Key,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetSizeAllocationDatabaseBin().BinTableName + "]");  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color-Size       
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color-Size value.</returns>
//        public double[] GetStoreColorSizeValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int aHdr_BCSZ_Key,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetSizeAllocationDatabaseBin().BinTableName + "]");  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color-Size        
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color-Size value.</returns>
//        public double[] GetStoreColorSizeValue
//           (short aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreColorSizeVectorContainer(aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetSizeAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }
//        #endregion GetStoreColorSizeValue
//        #endregion GetStoreVariableValue

//        #region SetStoreVariableValue
//        #region SetStoreTotalValue
//        /// <summary>
//        /// Sets the store total variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreTotalValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreTotalValue
//                (aVariableIDX,
//                aHdrRID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store total variable value for a given header
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreTotalValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetTotalAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreTotalValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetTotalAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store total variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreTotalValue
//            (int aVariableID,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetTotalAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreTotalValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetTotalAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store total variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreTotalValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreTotalVectorContainer(aHdrRID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetTotalAllocationDatabaseBin().VariableModel.VariableModelID;   // TT#707 - Container not thread safe (part 2) JEllis
//            double multiplier = Math.Pow(10, GetTotalAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreTotalValue

//        #region SetStoreDetailValue
//        /// <summary>
//        /// Sets the store Detail variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreDetailValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreDetailValue
//                (aVariableIDX,
//                aHdrRID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store Detail variable value for a given header
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreDetailValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetDetailAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreDetailValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetDetailAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store Detail variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreDetailValue
//            (int aVariableID,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetDetailAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreDetailValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetDetailAllocationDatabaseBin().BinTableName);   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store Detail variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreDetailValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreDetailVectorContainer(aHdrRID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetDetailAllocationDatabaseBin().VariableModel.VariableModelID;   // TT#707 - Container not thread safe (part 2) JEllis
//            double multiplier = Math.Pow(10, GetDetailAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);   // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreDetailValue

//        #region SetStoreBulkValue
//        /// <summary>
//        /// Sets the store Bulk variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreBulkValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreBulkValue
//                (aVariableIDX,
//                aHdrRID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store Bulk variable value for a given header
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreBulkValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetBulkAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreBulkValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetBulkAllocationDatabaseBin().BinTableName);   // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store Bulk variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreBulkValue
//            (int aVariableID,
//            int aHdrRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetBulkAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreBulkValue
//                    (variableIdx,
//                     aHdrRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetBulkAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store Bulk variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreBulkValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreBulkVectorContainer(aHdrRID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetBulkAllocationDatabaseBin().VariableModel.VariableModelID;  // TT#707 - Container not thread safe (part 2) JEllis
//            double multiplier = Math.Pow(10, GetBulkAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreBulkValue

//        #region SetStorePackValue
//        /// <summary>
//        /// Sets the store pack variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStorePackValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdrPackRID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store pack variable value for a given header
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetPackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetPackAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store pack variable value for a given header and pack 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetPackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetPackAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store pack variable value for a given header and pack 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHdrPackRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStorePackVectorContainer(aHdrRID, aHdrPackRID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetPackAllocationDatabaseBin().VariableModel.VariableModelID;  // TT#707 - Container not thread safe (part 2) JEllis
//            double multiplier = Math.Pow(10, GetPackAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStorePackValue

//        #region SetStoreColorValue
//        /// <summary>
//        /// Sets the store color variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreColorValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store color variable value for a given header
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetColorAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store color variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetColorAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store color variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHdr_BC_RID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreColorVectorContainer(aHdrRID, aHdr_BC_RID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetColorAllocationDatabaseBin().VariableModel.VariableModelID;  // TT#707 - Container not thread safe (part 2) JEllis
//            double multiplier = Math.Pow(10, GetColorAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreColorValue

//        #region SetStoreColorSizeValue
//        /// <summary>
//        /// Sets the store color-size variable value for a given header
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreColorSizeValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store color-size variable value for a given header
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetSizeAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store color-size variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))  // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                SetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetSizeAllocationDatabaseBin().BinTableName);  // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Sets the store color-size variable value for a given header 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHdr_BC_RID,
//             int aHdr_BCSZ_Key,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreColorSizeVectorContainer(aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetSizeAllocationDatabaseBin().VariableModel.VariableModelID;  // TT#707 - Container not thread safe (part 2) JEllis
//            double multiplier = Math.Pow(10, GetSizeAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  // TT#707 - Container not thread safe (part 2) JEllis
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreColorSizeValue
//        #endregion SetStoreVariableValue

//        #endregion StoreVariableValue

//        #region GetStoreVariableVectorContainer

//        #region GetStoreSummaryVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <returns>StoreVariableVectorContainer for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreTotalVectorContainer(int aHdrRID)
//        {
//            if (_lastHeaderTotalDatabaseBinKey == null
//                || _lastHeaderTotalDatabaseBinKey.HdrRID != aHdrRID)
//            {
//                HeaderDatabaseBinKey htbk = new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderTotalDatabaseBinKey, aHdrRID);
//                StoreVariableData<HeaderDatabaseBinKey> svd;
//                if (!_totalAllocationStoreVariableContainer.TryGetValue(htbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocation(aHdrRID);
//                    }
//                    if (!_totalAllocationStoreVariableContainer.TryGetValue(htbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderDatabaseBinKey>(0, htbk, new StoreVariableVectorContainer());
//                        _totalAllocationStoreVariableContainer.Add(htbk, svd);
//                    }
//                }
//                _lastHeaderTotalDatabaseBinKey = htbk;
//                _lastStoreTotalVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreTotalVectorContainer;
//        }

//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <returns>StoreVariableVectorContainery for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreDetailVectorContainer(int aHdrRID)
//        {
//            if (_lastHeaderDetailDatabaseBinKey == null
//                || _lastHeaderDetailDatabaseBinKey.HdrRID != aHdrRID)
//            {
//                HeaderDatabaseBinKey htbk = new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderDetailDatabaseBinKey, aHdrRID);
//                StoreVariableData<HeaderDatabaseBinKey> svd;
//                if (!this._totalAllocationStoreVariableContainer.TryGetValue(htbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocation(aHdrRID);
//                    }
//                    if (!_totalAllocationStoreVariableContainer.TryGetValue(htbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderDatabaseBinKey>(0, htbk, new StoreVariableVectorContainer());
//                        _totalAllocationStoreVariableContainer.Add(htbk, svd);
//                    }
//                }
//                _lastHeaderDetailDatabaseBinKey = htbk;
//                _lastStoreDetailVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreDetailVectorContainer;
//        }

//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <returns>StoreVariableVectorContainer for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreBulkVectorContainer(int aHdrRID)
//        {
//            if (_lastHeaderBulkDatabaseBinKey == null
//                || _lastHeaderBulkDatabaseBinKey.HdrRID != aHdrRID)
//            {
//                HeaderDatabaseBinKey htbk = new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderBulkDatabaseBinKey, aHdrRID);
//                StoreVariableData<HeaderDatabaseBinKey> svd;
//                if (!this._totalAllocationStoreVariableContainer.TryGetValue(htbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocation(aHdrRID);
//                    }
//                    if (!_totalAllocationStoreVariableContainer.TryGetValue(htbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderDatabaseBinKey>(0, htbk, new StoreVariableVectorContainer());
//                        _totalAllocationStoreVariableContainer.Add(htbk, svd);
//                    }
//                }
//                _lastHeaderBulkDatabaseBinKey = htbk;
//                _lastStoreBulkVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreBulkVectorContainer;
//        }
//        #endregion GetStoreSummaryVectorContainer

//        #region GetStorePackVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <returns>PackVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStorePackVectorContainer(int aHdrRID, int aHdrPackRID)
//        {
//            if (_lastPackDatabaseBinKey == null
//                || _lastPackDatabaseBinKey.HdrRID != aHdrRID
//                || _lastPackDatabaseBinKey.HdrPackRID != aHdrPackRID)
//            {
//                HeaderPackDatabaseBinKey hpbk = new HeaderPackDatabaseBinKey(aHdrRID, aHdrPackRID);
//                StoreVariableData<HeaderPackDatabaseBinKey> svd;
//                if (!_packAllocationStoreVariableContainer.TryGetValue(hpbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocation(aHdrRID);
//                    }
//                    if (!_packAllocationStoreVariableContainer.TryGetValue(hpbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderPackDatabaseBinKey>(0, hpbk, new StoreVariableVectorContainer());
//                        _packAllocationStoreVariableContainer.Add(hpbk, svd);
//                    }
//                }
//                _lastPackDatabaseBinKey = hpbk;
//                _lastStorePackVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStorePackVectorContainer;
//        }
//        #endregion GetStorePackVectorContainer

//        #region GetStoreColorVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Color RID (not Color Code RID)</param>
//        /// <returns>ColorVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreColorVectorContainer(int aHdrRID, int aHdr_BC_RID)
//        {
//            if (_lastColorDatabaseBinKey == null
//                || _lastColorDatabaseBinKey.HdrRID != aHdrRID
//                || _lastColorDatabaseBinKey.Hdr_BC_RID != aHdr_BC_RID)
//            {
//                HeaderColorDatabaseBinKey hcbk = new HeaderColorDatabaseBinKey(aHdrRID, aHdr_BC_RID);
//                StoreVariableData<HeaderColorDatabaseBinKey> svd;
//                if (!_colorAllocationStoreVariableContainer.TryGetValue(hcbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocation(aHdrRID);
//                    }
//                    if (!_colorAllocationStoreVariableContainer.TryGetValue(hcbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderColorDatabaseBinKey>(0, hcbk, new StoreVariableVectorContainer());
//                        _colorAllocationStoreVariableContainer.Add(hcbk, svd);
//                    }
//                }
//                _lastColorDatabaseBinKey = hcbk;
//                _lastStoreColorVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreColorVectorContainer;
//        }
//        #endregion GetStoreColorVectorContainer

//        #region GetStoreColorSizeVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Color RID (not Color Code RID)</param>
//        /// <returns>ColorVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreColorSizeVectorContainer(int aHdrRID, int aHdr_BC_RID, int aHdr_BCSZ_Key)
//        {
//            if (_lastSizeDatabaseBinKey == null
//                || _lastSizeDatabaseBinKey.HdrRID != aHdrRID
//                || _lastSizeDatabaseBinKey.Hdr_BC_RID != aHdr_BC_RID
//                || _lastSizeDatabaseBinKey.Hdr_BCSZ_Key != aHdr_BCSZ_Key)
//            {
//                HeaderColorSizeDatabaseBinKey hsbk = new HeaderColorSizeDatabaseBinKey(aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key);
//                StoreVariableData<HeaderColorSizeDatabaseBinKey> svd;
//                if (!_sizeAllocationStoreVariableContainer.TryGetValue(hsbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocation(aHdrRID);
//                    }
//                    if (!_sizeAllocationStoreVariableContainer.TryGetValue(hsbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderColorSizeDatabaseBinKey>(0, hsbk, new StoreVariableVectorContainer());
//                        _sizeAllocationStoreVariableContainer.Add(hsbk, svd);
//                    }
//                }
//                _lastSizeDatabaseBinKey = hsbk;
//                _lastStoreSizeVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreSizeVectorContainer;
//        }
//        #endregion GetStoreSizeVectorContainer

//        #region ReadStoreAllocation
//        /// <summary>
//        /// Get Store Allocation for the given header RID
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        public void ReadStoreAllocation(int aHdrRID)
//        {
//            HeaderDatabaseBinKey hdbkTotal = new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderTotalDatabaseBinKey, aHdrRID);
//            HeaderDatabaseBinKey hdbkDetail = new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderDetailDatabaseBinKey, aHdrRID);
//            HeaderDatabaseBinKey hdbkBulk = new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderBulkDatabaseBinKey, aHdrRID);
//            HeaderPackDatabaseBinKey hpdbk = new HeaderPackDatabaseBinKey(aHdrRID, Include.NoRID);
//            HeaderColorDatabaseBinKey hcdbk = new HeaderColorDatabaseBinKey(aHdrRID, Include.NoRID);
//            HeaderColorSizeDatabaseBinKey hcsdbk = new HeaderColorSizeDatabaseBinKey(aHdrRID, Include.NoRID, Include.NoRID);
//            List<StoreVariableData<HeaderDatabaseBinKey>> totalList = new List<StoreVariableData<HeaderDatabaseBinKey>>();
//            List<StoreVariableData<HeaderPackDatabaseBinKey>> packList = new List<StoreVariableData<HeaderPackDatabaseBinKey>>();
//            List<StoreVariableData<HeaderColorDatabaseBinKey>> colorList = new List<StoreVariableData<HeaderColorDatabaseBinKey>>();
//            List<StoreVariableData<HeaderColorSizeDatabaseBinKey>> sizeList = new List<StoreVariableData<HeaderColorSizeDatabaseBinKey>>();

//            try
//            {
//                Header header = new Header();
//                _totalAllocationStoreVariableContainer.Remove(hdbkTotal);
//                _totalAllocationStoreVariableContainer.Remove(hdbkDetail);
//                _totalAllocationStoreVariableContainer.Remove(hdbkBulk);
//                totalList =
//                    GetTotalAllocationDatabaseBin().ReadStoreVariables(header._dba, hdbkTotal);   // TT#707 - Container not thread safe (part 2) JEllis

//                if (totalList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderDatabaseBinKey> svd in totalList)
//                    {
//                        _totalAllocationStoreVariableContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                    StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                    _totalAllocationStoreVariableContainer.Add(hdbkTotal, new StoreVariableData<HeaderDatabaseBinKey>(0, hdbkTotal, svvc));
//                }
//                totalList.Clear();
//                totalList =
//                    GetDetailAllocationDatabaseBin().ReadStoreVariables(header._dba, hdbkDetail);   // TT#707 - Container not thread safe (part 2) JEllis
//                if (totalList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderDatabaseBinKey> svd in totalList)
//                    {
//                        _totalAllocationStoreVariableContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                    StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                    _totalAllocationStoreVariableContainer.Add(hdbkDetail, new StoreVariableData<HeaderDatabaseBinKey>(0, hdbkDetail, svvc));
//                }
//                totalList.Clear();
//                totalList =
//                    GetBulkAllocationDatabaseBin().ReadStoreVariables(header._dba, hdbkBulk);   // TT#707 - Container not thread safe (part 2) JEllis
//                if (totalList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderDatabaseBinKey> svd in totalList)
//                    {
//                        _totalAllocationStoreVariableContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                    StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                    _totalAllocationStoreVariableContainer.Add(hdbkBulk, new StoreVariableData<HeaderDatabaseBinKey>(0, hdbkBulk, svvc));
//                }

//                foreach (HeaderPackDatabaseBinKey packKey in _packAllocationStoreVariableContainer.Keys)
//                {
//                    if (aHdrRID == packKey.HdrRID)
//                    {
//                        _packAllocationStoreVariableContainer.Remove(packKey);
//                    }
//                }
//                packList =
//                    GetPackAllocationDatabaseBin().ReadStoreVariables(header._dba, hpdbk);  // TT#707 - Container not thread safe (part 2) JEllis
//                if (packList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderPackDatabaseBinKey> svd in packList)
//                    {
//                        _packAllocationStoreVariableContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                }
//                foreach (HeaderColorDatabaseBinKey colorKey in _colorAllocationStoreVariableContainer.Keys)
//                {
//                    if (aHdrRID == colorKey.HdrRID)
//                    {
//                        _colorAllocationStoreVariableContainer.Remove(colorKey);
//                    }
//                }
//                colorList =
//                    GetColorAllocationDatabaseBin().ReadStoreVariables(header._dba, hcdbk);   // TT#707 - Container not thread safe (part 2) JEllis
//                if (colorList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderColorDatabaseBinKey> svd in colorList)
//                    {
//                        _colorAllocationStoreVariableContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                }
//                foreach (HeaderColorSizeDatabaseBinKey sizeKey in _sizeAllocationStoreVariableContainer.Keys)
//                {
//                    if (aHdrRID == sizeKey.HdrRID)
//                    {
//                        _sizeAllocationStoreVariableContainer.Remove(sizeKey);
//                    }
//                }
//                sizeList =
//                    GetSizeAllocationDatabaseBin().ReadStoreVariables(header._dba, hcsdbk);  // TT#707 - Container not thread safe (part 2) JEllis
//                if (sizeList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderColorSizeDatabaseBinKey> svd in sizeList)
//                    {
//                        _sizeAllocationStoreVariableContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                }
//                try
//                {
//                    _isHeaderCached.Add(aHdrRID, true);
//                }
//                catch (ArgumentException)
//                {
//                    //  Key already exists!
//                }
//            }
//            catch
//            {
//                _isHeaderCached.Remove(aHdrRID);
//                _totalAllocationStoreVariableContainer.Remove(hdbkTotal);
//                _totalAllocationStoreVariableContainer.Remove(hdbkDetail);
//                _totalAllocationStoreVariableContainer.Remove(hdbkBulk);

//                foreach (HeaderPackDatabaseBinKey packKey in _packAllocationStoreVariableContainer.Keys)
//                {
//                    if (aHdrRID == packKey.HdrRID)
//                    {
//                        _packAllocationStoreVariableContainer.Remove(packKey);
//                    }
//                }
//                foreach (HeaderColorDatabaseBinKey colorKey in _colorAllocationStoreVariableContainer.Keys)
//                {
//                    if (aHdrRID == colorKey.HdrRID)
//                    {
//                        _colorAllocationStoreVariableContainer.Remove(colorKey);
//                    }
//                }
//                foreach (HeaderColorSizeDatabaseBinKey sizeKey in _sizeAllocationStoreVariableContainer.Keys)
//                {
//                    if (aHdrRID == sizeKey.HdrRID)
//                    {
//                        _sizeAllocationStoreVariableContainer.Remove(sizeKey);
//                    }
//                }
//                throw;
//            }
//            finally
//            {
//            }
//        }
//        #endregion ReadStoreAllocation

//        #endregion GetStoreVariableVectorContainer

//        #region WriteStoreAllocation

//        /// <summary>
//        /// Writes the store allocation for each header represented in the container
//        /// </summary>
//        /// <param name="aHeaderDataRecord">Header data record containing an open database connection</param>
//        /// <param name="aHdrRIDsToWrite">Header Keys (RIDs) to write.</param>
//        public void WriteStoreAllocation(Header aHeaderDataRecord, int[] aHdrKeysToWrite) // TT#467 Store Container Enqueue changes
//        {
//            // begin TT#467 Store Container Enqueue changes
//            HeaderDatabaseBinKey[] totalKeysToWrite = 
//                new HeaderDatabaseBinKey[aHdrKeysToWrite.Length];
//            HeaderDatabaseBinKey[] detailKeysToWrite = 
//                new HeaderDatabaseBinKey[aHdrKeysToWrite.Length];
//            HeaderDatabaseBinKey[] bulkKeysToWrite = 
//                new HeaderDatabaseBinKey[aHdrKeysToWrite.Length];
//            List<HeaderPackDatabaseBinKey> packKeysToWrite = 
//                new List<HeaderPackDatabaseBinKey>();
//            List<HeaderColorDatabaseBinKey> colorKeysToWrite = 
//                new List<HeaderColorDatabaseBinKey>();
//            List<HeaderColorSizeDatabaseBinKey> colorSizeKeysToWrite = 
//                new List<HeaderColorSizeDatabaseBinKey>();
//            for (int i=0; i<aHdrKeysToWrite.Length; i++)
//            {
//                totalKeysToWrite[i] = 
//                    new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderTotalDatabaseBinKey,aHdrKeysToWrite[i]);
//                detailKeysToWrite[i] = 
//                    new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderDetailDatabaseBinKey,aHdrKeysToWrite[i]);
//                bulkKeysToWrite[i] = 
//                    new HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType.HeaderBulkDatabaseBinKey,aHdrKeysToWrite[i]);
//                foreach (HeaderPackDatabaseBinKey hpdk in _packAllocationStoreVariableContainer.Keys)
//                {
//                    if (hpdk.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        packKeysToWrite.Add(hpdk);
//                    }
//                }
//                foreach (HeaderColorDatabaseBinKey hcdk in _colorAllocationStoreVariableContainer.Keys)
//                {
//                    if (hcdk.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        colorKeysToWrite.Add(hcdk);
//                    }
//                }
//                foreach (HeaderColorSizeDatabaseBinKey hcsdk in _sizeAllocationStoreVariableContainer.Keys)
//                {
//                    if (hcsdk.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        colorSizeKeysToWrite.Add(hcsdk);
//                    }
//                }
//            }
//            GetTotalAllocationDatabaseBin().WriteStoreVariables(   // TT#707 - Container not thread safe (part 2) JEllis
//                aHeaderDataRecord._dba, 
//                _totalAllocationStoreVariableContainer, 
//                totalKeysToWrite);
//            GetDetailAllocationDatabaseBin().WriteStoreVariables(   // TT#707 - Container not thread safe (part 2) JEllis
//                aHeaderDataRecord._dba, 
//                _totalAllocationStoreVariableContainer, 
//                detailKeysToWrite);
//            GetBulkAllocationDatabaseBin().WriteStoreVariables(   // TT#707 - Container not thread safe (part 2) JEllis
//                aHeaderDataRecord._dba, 
//                _totalAllocationStoreVariableContainer, 
//                bulkKeysToWrite);
//            if (packKeysToWrite.Count > 0)
//            {
//                HeaderPackDatabaseBinKey[] packKeys = new HeaderPackDatabaseBinKey[packKeysToWrite.Count];
//                packKeysToWrite.CopyTo(packKeys);
//                GetPackAllocationDatabaseBin().WriteStoreVariables(   // TT#707 - Container not thread safe (part 2) JEllis
//                    aHeaderDataRecord._dba,
//                    _packAllocationStoreVariableContainer,
//                    packKeys);
//            }
//            if (colorKeysToWrite.Count > 0)
//            {
//                HeaderColorDatabaseBinKey[] colorKeys = new HeaderColorDatabaseBinKey[colorKeysToWrite.Count];
//                colorKeysToWrite.CopyTo(colorKeys);
//                GetColorAllocationDatabaseBin().WriteStoreVariables(   // TT#707 - Container not thread safe (part 2) JEllis
//                    aHeaderDataRecord._dba,
//                    _colorAllocationStoreVariableContainer,
//                    colorKeys);
//            }
//            if (colorSizeKeysToWrite.Count > 0)
//            {
//                HeaderColorSizeDatabaseBinKey[] sizeKeys = new HeaderColorSizeDatabaseBinKey[colorSizeKeysToWrite.Count];
//                colorSizeKeysToWrite.CopyTo(sizeKeys);
//                GetSizeAllocationDatabaseBin().WriteStoreVariables(   // TT#707 - Container not thread safe (part 2) JEllis
//                    aHeaderDataRecord._dba,
//                    _sizeAllocationStoreVariableContainer,
//                    sizeKeys);
//            }
//            // end TT#467 Store Container Enqueue changes
//        }

//        #endregion WriteStoreAllocation

//        //public void DeleteStoreAllocation()
//        //{
//        //    
//        //}
//        // begin   TT#707 - Container not thread safe (part 2) JEllis
//        private TotalAllocationDatabaseBin GetTotalAllocationDatabaseBin()
//        {
//            if (_totalAllocationDatabaseBin == null)
//            {
//                _totalAllocationDatabaseBin = new TotalAllocationDatabaseBin();
//            }
//            return _totalAllocationDatabaseBin;
//        }
//        private DetailAllocationDatabaseBin GetDetailAllocationDatabaseBin()
//        {
//            if (_detailAllocationDatabaseBin == null)
//            {
//                _detailAllocationDatabaseBin = new DetailAllocationDatabaseBin();
//            }
//            return _detailAllocationDatabaseBin;
//        }
//        private PackAllocationDatabaseBin GetPackAllocationDatabaseBin()
//        {
//            if (_packAllocationDatabaseBin == null)
//            {
//                _packAllocationDatabaseBin = new PackAllocationDatabaseBin();
//            }
//            return _packAllocationDatabaseBin;
//        }
//        private BulkAllocationDatabaseBin GetBulkAllocationDatabaseBin()
//        {
//            if (_bulkAllocationDatabaseBin == null)
//            {
//                _bulkAllocationDatabaseBin = new BulkAllocationDatabaseBin();
//            }
//            return _bulkAllocationDatabaseBin;
//        }
//        private ColorAllocationDatabaseBin GetColorAllocationDatabaseBin()
//        {
//            if (_colorAllocationDatabaseBin == null)
//            {
//                _colorAllocationDatabaseBin = new ColorAllocationDatabaseBin();
//            }
//            return _colorAllocationDatabaseBin;
//        }
//        private SizeAllocationDatabaseBin GetSizeAllocationDatabaseBin()
//        {
//            if (_sizeAllocationDatabaseBin == null)
//            {
//                _sizeAllocationDatabaseBin = new SizeAllocationDatabaseBin();
//            }
//            return _sizeAllocationDatabaseBin;
//        }
//        // end     TT#707 - Container not thread safe (part 2) JEllis

//        #region Flush
//        /// <summary>
//        /// Flushes the containers associated with the given header RIDs from memory
//        /// </summary>
//        /// <param name="aHnRIDs">Array of the header RIDs to flush</param>
//        public void Flush(int[] aHdrRIDs)
//        {
//            List<HeaderDatabaseBinKey> totalKeys = new List<HeaderDatabaseBinKey>();
//            List<HeaderPackDatabaseBinKey> packKeys = new List<HeaderPackDatabaseBinKey>();
//            List<HeaderColorDatabaseBinKey> colorKeys = new List<HeaderColorDatabaseBinKey>();
//            List<HeaderColorSizeDatabaseBinKey> sizeKeys = new List<HeaderColorSizeDatabaseBinKey>();
//            for (int i = 0; i < aHdrRIDs.Length; i++)
//            {
//                foreach (HeaderDatabaseBinKey htbk in _totalAllocationStoreVariableContainer.Keys)
//                {
//                    if (htbk.HdrRID == aHdrRIDs[i])
//                    {
//                        totalKeys.Add(htbk);
//                    }
//                }
//                foreach (HeaderPackDatabaseBinKey hpbk in _packAllocationStoreVariableContainer.Keys)
//                {
//                    if (hpbk.HdrRID == aHdrRIDs[i])
//                    {
//                        packKeys.Add(hpbk);
//                    }
//                }
//                foreach (HeaderColorDatabaseBinKey hcbk in _colorAllocationStoreVariableContainer.Keys)
//                {
//                    if (hcbk.HdrRID == aHdrRIDs[i])
//                    {
//                        colorKeys.Add(hcbk);
//                    }
//                }
//                foreach (HeaderColorSizeDatabaseBinKey hsbk in _sizeAllocationStoreVariableContainer.Keys)
//                {
//                    if (hsbk.HdrRID == aHdrRIDs[i])
//                    {
//                        sizeKeys.Add(hsbk);
//                    }
//                }
//                _isHeaderCached.Remove(aHdrRIDs[i]);  // TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//            }
//            foreach (HeaderDatabaseBinKey htbk in totalKeys)
//            {
//                _totalAllocationStoreVariableContainer.Remove(htbk);
//            }
//            foreach (HeaderPackDatabaseBinKey hpbk in packKeys)
//            {
//                _packAllocationStoreVariableContainer.Remove(hpbk);
//            }
//            foreach (HeaderColorDatabaseBinKey hcbk in colorKeys)
//            {
//                _colorAllocationStoreVariableContainer.Remove(hcbk);
//            }
//            foreach (HeaderColorSizeDatabaseBinKey hsbk in sizeKeys)
//            {
//                _sizeAllocationStoreVariableContainer.Remove(hsbk);
//            }
//            _lastColorDatabaseBinKey = null;
//            _lastPackDatabaseBinKey = null;
//            _lastSizeDatabaseBinKey = null;
//            _lastHeaderTotalDatabaseBinKey = null;
//            _lastHeaderDetailDatabaseBinKey = null;
//            _lastHeaderBulkDatabaseBinKey = null;
//            _lastStoreColorVectorContainer = null;
//            _lastStorePackVectorContainer = null;
//            _lastStoreSizeVectorContainer = null;
//            _lastStoreTotalVectorContainer = null;
//            _lastStoreDetailVectorContainer = null;
//            _lastStoreBulkVectorContainer = null;
//        }

//        /// <summary>
//        /// Flushes the all containers from memory
//        /// </summary>
//        public void FlushAll()
//        {
//            _lastColorDatabaseBinKey = null;
//            _lastPackDatabaseBinKey = null;
//            _lastSizeDatabaseBinKey = null;
//            _lastHeaderTotalDatabaseBinKey = null;
//            _lastHeaderDetailDatabaseBinKey = null;
//            _lastHeaderBulkDatabaseBinKey = null;
//            _lastStoreColorVectorContainer = null;
//            _lastStorePackVectorContainer = null;
//            _lastStoreSizeVectorContainer = null;
//            _lastStoreTotalVectorContainer = null;
//            _lastStoreDetailVectorContainer = null;
//            _lastStoreBulkVectorContainer = null;
//            _isHeaderCached.Clear(); // TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//        }
//        #endregion Flush

//        #region Dispose
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        public void Dispose()
//        {
//            Dispose(true);
//            System.GC.SuppressFinalize(this);
//        }
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        /// <param name="disposing">
//        /// True:  dispose all managed and unmanaged resources.
//        /// False: dispose only unmanaged resources
//        /// </param>
//        protected void Dispose(bool disposing)
//        {
//            if (!_isDisposed)
//            {
//                if (disposing)
//                {
//                    //if (ConnectionIsOpen)
//                    //{
//                    //    CloseUpdateConnection();
//                    //}
//                    _colorAllocationStoreVariableContainer = null;
//                    _isHeaderCached = null;
//                    _packAllocationStoreVariableContainer = null;
//                    _sizeAllocationStoreVariableContainer = null;
//                    _totalAllocationStoreVariableContainer = null;
//                }
//                _isDisposed = true;
//            }
//        }
//        ~StoreVariableAllocationBin()
//        {
//            Dispose(false);
//        }
//        #endregion Dispose
//    }
//    #endregion StoreAllocation

//    // begin TT#370 Build Packs Enhancement
//    #region ArchiveAllocation
//    /// <summary>
//    /// Container for Store Allocation Archived Variables.
//    /// </summary>
//    [Serializable]
//    public class StoreAllocationArchive : IDisposable
//    {
//        private bool _isDisposed;
//        private Dictionary<int, bool> _isHeaderCached;
//        // begin   TT#707 - Container not thread safe (part 2) JEllis
//        private ArchiveSummaryAllocationDatabaseBin _archiveSummaryAllocationDatabaseBin;
//        private ArchivePackAllocationDatabaseBin _archivePackAllocationDatabaseBin;
//        private ArchiveColorAllocationDatabaseBin _archiveColorAllocationDatabaseBin;
//        private ArchiveColorSizeAllocationDatabaseBin _archiveColorSizeAllocationDatabaseBin;
//        // end     TT#707 - Container not thread safe (part 2) JEllis
//        private StoreVariableDataDictionary<HeaderSummaryDatabaseBinKey>   _archiveSummaryAllocationContainer;
//        private StoreVariableDataDictionary<HeaderPackDatabaseBinKey>      _archivePackAllocationContainer;
//        private StoreVariableDataDictionary<HeaderColorDatabaseBinKey>     _archiveColorAllocationContainer;
//        private StoreVariableDataDictionary<HeaderColorSizeDatabaseBinKey> _archiveSizeAllocationContainer;
//        private HeaderSummaryDatabaseBinKey   _lastSummaryDatabaseBinKey;
//        private StoreVariableVectorContainer  _lastStoreSummaryVectorContainer;
//        private HeaderPackDatabaseBinKey      _lastPackDatabaseBinKey;
//        private StoreVariableVectorContainer  _lastStorePackVectorContainer;
//        private HeaderColorDatabaseBinKey     _lastColorDatabaseBinKey;
//        private StoreVariableVectorContainer  _lastStoreColorVectorContainer;
//        private HeaderColorSizeDatabaseBinKey _lastSizeDatabaseBinKey;
//        private StoreVariableVectorContainer  _lastStoreSizeVectorContainer;

//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        public StoreAllocationArchive()
//        {
//            _isDisposed = false;
//            _isHeaderCached = new Dictionary<int, bool>();
//            _archiveSummaryAllocationContainer = new StoreVariableDataDictionary<HeaderSummaryDatabaseBinKey>();
//            _archivePackAllocationContainer    = new StoreVariableDataDictionary<HeaderPackDatabaseBinKey>();
//            _archiveColorAllocationContainer   = new StoreVariableDataDictionary<HeaderColorDatabaseBinKey>();
//            _archiveSizeAllocationContainer    = new StoreVariableDataDictionary<HeaderColorSizeDatabaseBinKey>();
//        }


//        #region StoreVariableValue
//        #region GetStoreVariableValue
//        #region GetStoreSummaryValue
//        /// <summary>
//        /// Gets the store's summary level value for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's summary level value.</returns>
//        public double GetStoreSummaryValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreSummaryValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aAllocationSummaryNode,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's summary level value for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's summary level value.</returns>
//        public double GetStoreSummaryValue
//            (int aVariableID,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreSummaryValue
//                (aVariableID,
//                aHdrRID,
//                aAllocationSummaryNode,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's summary level value for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's summary level value.</returns>
//        public double GetStoreSummaryValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreSummaryValue
//                (aVariableIDX,
//                aHdrRID,
//                aAllocationSummaryNode,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the stores' summary level values for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' summary level values in same order as provided store list.</returns>
//        public double[] GetStoreSummaryValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int[] aStoreRID)
//        {
//            return GetStoreSummaryValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aAllocationSummaryNode,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the stores' summary level values for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' summary level values in same order as provided store list.</returns>
//        public double[] GetStoreSummaryValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           eAllocationSummaryNode aAllocationSummaryNode,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchiveSummaryAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   // TT#707 - Container not thread safe (part 2) JEllis
//            {
//                return GetStoreSummaryValue
//                    (variableIdx,
//                     aHdrRID,
//                     aAllocationSummaryNode,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetArchiveSummaryAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis
//        }
//        /// <summary>
//        /// Gets the stores' summary level values for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' summary level values in same order as provided store list.</returns>
//        public double[] GetStoreSummaryValue
//          (int aVariableID,
//           int aHdrRID,
//           eAllocationSummaryNode aAllocationSummaryNode,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchiveSummaryAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStoreSummaryValue
//                    (variableIdx,
//                     aHdrRID,
//                     aAllocationSummaryNode,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetArchiveSummaryAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the stores' summary level values for the given header and allocation summary node (TOTAL, DETAIL or BULK)
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' summary level values in same order as provided store list.</returns>
//        public double[] GetStoreSummaryValue
//           (short aVariableIDX,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreSummaryVectorContainer(aHdrRID, aAllocationSummaryNode);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetArchiveSummaryAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }

//        #endregion GetStoreSummaryValue

//        #region GetStorePackValue
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable pack value.</returns>
//        public double GetStorePackValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdrPackRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStorePackValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHdrPackRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Pack value.</returns>
//        public double GetStorePackValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdrPackRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStorePackValue
//                (aVariableID,
//                aHdrRID,
//                aHdrPackRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Pack value.</returns>
//        public double GetStorePackValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdrPackRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStorePackValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdrPackRID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Pack value.</returns>
//        public double[] GetStorePackValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRID)
//        {
//            return GetStorePackValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHdrPackRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and pack
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Pack value.</returns>
//        public double[] GetStorePackValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHdrPackRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchivePackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetArchivePackAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and pack        
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Pack value.</returns>
//        public double[] GetStorePackValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHdrPackRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchivePackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetArchivePackAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and pack        
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Pack value.</returns>
//        public double[] GetStorePackValue
//           (short aVariableIDX,
//            int aHdrRID,
//           int aHdrPackRID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStorePackVectorContainer(aHdrRID, aHdrPackRID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetArchivePackAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }

//        #endregion GetStorePackValue

//        #region GetStoreColorValue
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable color value.</returns>
//        public double GetStoreColorValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHdr_BC_RID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color value.</returns>
//        public double GetStoreColorValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorValue
//                (aVariableID,
//                aHdrRID,
//                aHdr_BC_RID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color value.</returns>
//        public double GetStoreColorValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color value.</returns>
//        public double[] GetStoreColorValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRID)
//        {
//            return GetStoreColorValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color value.</returns>
//        public double[] GetStoreColorValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchiveColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetArchiveColorAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color       
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color value.</returns>
//        public double[] GetStoreColorValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchiveColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetArchiveColorAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color        
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color value.</returns>
//        public double[] GetStoreColorValue
//           (short aVariableIDX,
//            int aHdrRID,
//           int aHdr_BC_RID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreColorVectorContainer(aHdrRID, aHdr_BC_RID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetArchiveColorAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }

//        #endregion GetStoreColorValue

//        #region GetStoreColorSizeValue
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable color-size value.</returns>
//        public double GetStoreColorSizeValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorSizeValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color-Size value.</returns>
//        public double GetStoreColorSizeValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorSizeValue
//                (aVariableID,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's variable Color-Size value.</returns>
//        public double GetStoreColorSizeValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreColorSizeValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aVariableIDX"><Variable Index (position of variable in the container/param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color-size value.</returns>
//        public double[] GetStoreColorSizeValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRID)
//        {
//            return GetStoreColorSizeValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's variable value for the given header and color-size
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Stores' variable Color-Size value.</returns>
//        public double[] GetStoreColorSizeValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int aHdr_BCSZ_Key,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchiveColorSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetArchiveColorSizeAllocationDatabaseBin().BinTableName + "]"); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color-Size       
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color-Size value.</returns>
//        public double[] GetStoreColorSizeValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHdr_BC_RID,
//           int aHdr_BCSZ_Key,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetArchiveColorSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                return GetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetArchiveColorSizeAllocationDatabaseBin().BinTableName + "]");// TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Gets the store's value for the given header and Color-Size        
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not the Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Bulk Color Size Key within color</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Color-Size value.</returns>
//        public double[] GetStoreColorSizeValue
//           (short aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreColorSizeVectorContainer(aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetArchiveColorSizeAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }

//        #endregion GetStoreColorSizeValue
//        #endregion GetStoreVariableValue

//        #region SetStoreVariableValue
//        #region SetStoreSummaryValue
//        /// <summary>
//        /// Sets the store archive variable value for a given header and summary level (TOTAL, DETAIL, BULK) 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreSummaryValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreSummaryValue
//                (aVariableIDX,
//                aHdrRID,
//                aAllocationSummaryNode,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and summary level (TOTAL, DETAIL, BULK) 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreSummaryValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchiveSummaryAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStoreSummaryValue
//                    (variableIdx,
//                     aHdrRID,
//                     aAllocationSummaryNode,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetArchiveSummaryAllocationDatabaseBin().BinTableName); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and summary level (TOTAL, DETAIL, BULK) 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreSummaryValue
//            (int aVariableID,
//            int aHdrRID,
//            eAllocationSummaryNode aAllocationSummaryNode,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchiveSummaryAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStoreSummaryValue
//                    (variableIdx,
//                     aHdrRID,
//                     aAllocationSummaryNode,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetArchiveSummaryAllocationDatabaseBin().BinTableName);// TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and summary level (TOTAL, DETAIL, BULK) 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreSummaryValue
//            (short aVariableIDX,
//             int aHdrRID,
//             eAllocationSummaryNode aAllocationSummaryNode,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreSummaryVectorContainer(aHdrRID, aAllocationSummaryNode);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetArchiveSummaryAllocationDatabaseBin().VariableModel.VariableModelID; // TT#707 - Container not thread safe (part 2) JEllis 
//            double multiplier = Math.Pow(10, GetArchiveSummaryAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);// TT#707 - Container not thread safe (part 2) JEllis  
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreSummaryValue

//        #region SetStorePackValue
//        /// <summary>
//        /// Sets the store archive variable value for a given header and pack 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStorePackValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdrPackRID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and pack 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchivePackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetArchivePackAllocationDatabaseBin().BinTableName); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and pack 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdrPackRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchivePackAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStorePackValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdrPackRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetArchivePackAllocationDatabaseBin().BinTableName);// TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and pack 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStorePackValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHdrPackRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStorePackVectorContainer(aHdrRID, aHdrPackRID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetArchivePackAllocationDatabaseBin().VariableModel.VariableModelID; // TT#707 - Container not thread safe (part 2) JEllis 
//            double multiplier = Math.Pow(10, GetArchivePackAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStorePackValue

//        #region SetStoreColorValue
//        /// <summary>
//        /// Sets the store archive variable value for a given header and color 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreColorValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and Color 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchiveColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetArchiveColorAllocationDatabaseBin().BinTableName); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and Color 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchiveColorAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStoreColorValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetArchiveColorAllocationDatabaseBin().BinTableName); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and Color 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHdr_BC_RID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreColorVectorContainer(aHdrRID, aHdr_BC_RID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetArchiveColorAllocationDatabaseBin().VariableModel.VariableModelID; // TT#707 - Container not thread safe (part 2) JEllis 
//            double multiplier = Math.Pow(10, GetArchiveColorAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreColorValue

//        #region SetStoreColorSizeValue
//        /// <summary>
//        /// Sets the store archive variable value for a given header and color-size 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreColorSizeValue
//                (aVariableIDX,
//                aHdrRID,
//                aHdr_BC_RID,
//                aHdr_BCSZ_Key,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and Color-Size 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchiveColorSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetArchiveColorSizeAllocationDatabaseBin().BinTableName); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and Color-Size 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHdr_BC_RID,
//            int aHdr_BCSZ_Key,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetArchiveColorSizeAllocationDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - Container not thread safe (part 2) JEllis 
//            {
//                SetStoreColorSizeValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHdr_BC_RID,
//                     aHdr_BCSZ_Key,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetArchiveColorSizeAllocationDatabaseBin().BinTableName); // TT#707 - Container not thread safe (part 2) JEllis 
//        }
//        /// <summary>
//        /// Sets the store archive variable value for a given header and Color-Size 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Bulk Color RID (not Color Code RID)</param>
//        /// <param name="aHdr_BCSZ_Key">Header Size Key within color</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreColorSizeValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHdr_BC_RID,
//             int aHdr_BCSZ_Key,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreColorSizeVectorContainer(aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetArchiveColorSizeAllocationDatabaseBin().VariableModel.VariableModelID; // TT#707 - Container not thread safe (part 2) JEllis 
//            double multiplier = Math.Pow(10, GetArchiveColorSizeAllocationDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - Container not thread safe (part 2) JEllis 
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreColorSizeValue
//        #endregion SetStoreVariableValue

//        #endregion StoreVariableValue

//        #region GetStoreVariableVectorContainer

//        #region GetStoreSummaryVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aAllocationSummaryNode">Allocation Summary Node</param>
//        /// <returns>SummaryVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreSummaryVectorContainer(int aHdrRID, eAllocationSummaryNode aAllocationSummaryNode )
//        {
//            if (_lastSummaryDatabaseBinKey == null
//                || _lastSummaryDatabaseBinKey.HdrRID != aHdrRID
//                || _lastSummaryDatabaseBinKey.AllocationSummaryNode != aAllocationSummaryNode)
//            {
//                HeaderSummaryDatabaseBinKey hsbk = new HeaderSummaryDatabaseBinKey(aHdrRID, aAllocationSummaryNode);
//                StoreVariableData<HeaderSummaryDatabaseBinKey> svd;
//                if (!_archiveSummaryAllocationContainer.TryGetValue(hsbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocationArchive(aHdrRID);
//                    }
//                    if (!_archiveSummaryAllocationContainer.TryGetValue(hsbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderSummaryDatabaseBinKey>(0, hsbk, new StoreVariableVectorContainer());
//                        _archiveSummaryAllocationContainer.Add(hsbk, svd);
//                    }
//                }
//                _lastSummaryDatabaseBinKey = hsbk;
//                _lastStoreSummaryVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreSummaryVectorContainer;
//        }
//        #endregion GetStoreSummaryVectorContainer

//        #region GetStorePackVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdrPackRID">Header Pack RID</param>
//        /// <returns>PackVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStorePackVectorContainer(int aHdrRID, int aHdrPackRID)
//        {
//            if (_lastPackDatabaseBinKey == null
//                || _lastPackDatabaseBinKey.HdrRID != aHdrRID
//                || _lastPackDatabaseBinKey.HdrPackRID != aHdrPackRID)
//            {
//                HeaderPackDatabaseBinKey hpbk = new HeaderPackDatabaseBinKey(aHdrRID, aHdrPackRID);
//                StoreVariableData<HeaderPackDatabaseBinKey> svd;
//                if (!_archivePackAllocationContainer.TryGetValue(hpbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocationArchive(aHdrRID);
//                    }
//                    if (!_archivePackAllocationContainer.TryGetValue(hpbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderPackDatabaseBinKey>(0, hpbk, new StoreVariableVectorContainer());
//                        _archivePackAllocationContainer.Add(hpbk, svd);
//                    }
//                }
//                _lastPackDatabaseBinKey = hpbk;
//                _lastStorePackVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStorePackVectorContainer;
//        }
//        #endregion GetStorePackVectorContainer

//        #region GetStoreColorVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Color RID (not Color Code RID)</param>
//        /// <returns>ColorVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreColorVectorContainer(int aHdrRID, int aHdr_BC_RID)
//        {
//            if (_lastColorDatabaseBinKey == null
//                || _lastColorDatabaseBinKey.HdrRID != aHdrRID
//                || _lastColorDatabaseBinKey.Hdr_BC_RID != aHdr_BC_RID)
//            {
//                HeaderColorDatabaseBinKey hcbk = new HeaderColorDatabaseBinKey(aHdrRID, aHdr_BC_RID);
//                StoreVariableData<HeaderColorDatabaseBinKey> svd;
//                if (!_archiveColorAllocationContainer.TryGetValue(hcbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocationArchive(aHdrRID);
//                    }
//                    if (!_archiveColorAllocationContainer.TryGetValue(hcbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderColorDatabaseBinKey>(0, hcbk, new StoreVariableVectorContainer());
//                        _archiveColorAllocationContainer.Add(hcbk, svd);
//                    }
//                }
//                _lastColorDatabaseBinKey = hcbk;
//                _lastStoreColorVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreColorVectorContainer;
//        }
//        #endregion GetStoreColorVectorContainer

//        #region GetStoreColorSizeVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHdr_BC_RID">Header Color RID (not Color Code RID)</param>
//        /// <returns>ColorVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreColorSizeVectorContainer(int aHdrRID, int aHdr_BC_RID, int aHdr_BCSZ_Key)
//        {
//            if (_lastSizeDatabaseBinKey == null
//                || _lastSizeDatabaseBinKey.HdrRID != aHdrRID
//                || _lastSizeDatabaseBinKey.Hdr_BC_RID != aHdr_BC_RID
//                || _lastSizeDatabaseBinKey.Hdr_BCSZ_Key != aHdr_BCSZ_Key)
//            {
//                HeaderColorSizeDatabaseBinKey hsbk = new HeaderColorSizeDatabaseBinKey(aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key);
//                StoreVariableData<HeaderColorSizeDatabaseBinKey> svd;
//                if (!_archiveSizeAllocationContainer.TryGetValue(hsbk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreAllocationArchive(aHdrRID);
//                    }
//                    if (!_archiveSizeAllocationContainer.TryGetValue(hsbk, out svd))
//                    {
//                        svd = new StoreVariableData<HeaderColorSizeDatabaseBinKey>(0, hsbk, new StoreVariableVectorContainer());
//                        _archiveSizeAllocationContainer.Add(hsbk, svd);
//                    }
//                }
//                _lastSizeDatabaseBinKey = hsbk;
//                _lastStoreSizeVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreSizeVectorContainer;
//        }
//        #endregion GetStoreSizeVectorContainer

//        #region ReadStoreAllocationArchive
//        /// <summary>
//        /// Get Store Allocation for the given header RID
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        public void ReadStoreAllocationArchive(int aHdrRID)
//        {
//            HeaderSummaryDatabaseBinKey hdbkTotal = new HeaderSummaryDatabaseBinKey(aHdrRID, eAllocationSummaryNode.Total);
//            HeaderSummaryDatabaseBinKey hdbkDetail = new HeaderSummaryDatabaseBinKey(aHdrRID, eAllocationSummaryNode.DetailSubType);
//            HeaderSummaryDatabaseBinKey hdbkBulk = new HeaderSummaryDatabaseBinKey(aHdrRID, eAllocationSummaryNode.Bulk);
//            HeaderSummaryDatabaseBinKey hdbkSummary = new HeaderSummaryDatabaseBinKey(aHdrRID);
//            HeaderPackDatabaseBinKey hpdbk = new HeaderPackDatabaseBinKey(aHdrRID, Include.NoRID);
//            HeaderColorDatabaseBinKey hcdbk = new HeaderColorDatabaseBinKey(aHdrRID, Include.NoRID);
//            HeaderColorSizeDatabaseBinKey hcsdbk = new HeaderColorSizeDatabaseBinKey(aHdrRID, Include.NoRID, Include.NoRID);
//            List<StoreVariableData<HeaderSummaryDatabaseBinKey>> totalList = new List<StoreVariableData<HeaderSummaryDatabaseBinKey>>();
//            List<StoreVariableData<HeaderPackDatabaseBinKey>> packList = new List<StoreVariableData<HeaderPackDatabaseBinKey>>();
//            List<StoreVariableData<HeaderColorDatabaseBinKey>> colorList = new List<StoreVariableData<HeaderColorDatabaseBinKey>>();
//            List<StoreVariableData<HeaderColorSizeDatabaseBinKey>> sizeList = new List<StoreVariableData<HeaderColorSizeDatabaseBinKey>>();

//            try
//            {
//                Header header = new Header();
//                _archiveSummaryAllocationContainer.Remove(hdbkTotal);
//                _archiveSummaryAllocationContainer.Remove(hdbkDetail);
//                _archiveSummaryAllocationContainer.Remove(hdbkBulk);
//                totalList =
//                    GetArchiveSummaryAllocationDatabaseBin().ReadStoreVariables(header._dba, hdbkSummary); // TT#707 - Container not thread safe (part 2) JEllis 

//                if (totalList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderSummaryDatabaseBinKey> svd in totalList)
//                    {
//                        _archiveSummaryAllocationContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                foreach (HeaderPackDatabaseBinKey packKey in _archivePackAllocationContainer.Keys)
//                {
//                    if (aHdrRID == packKey.HdrRID)
//                    {
//                        _archivePackAllocationContainer.Remove(packKey);
//                    }
//                }
//                packList =
//                    GetArchivePackAllocationDatabaseBin().ReadStoreVariables(header._dba, hpdbk); // TT#707 - Container not thread safe (part 2) JEllis 
//                if (packList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderPackDatabaseBinKey> svd in packList)
//                    {
//                        _archivePackAllocationContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                }
//                foreach (HeaderColorDatabaseBinKey colorKey in _archiveColorAllocationContainer.Keys)
//                {
//                    if (aHdrRID == colorKey.HdrRID)
//                    {
//                        _archiveColorAllocationContainer.Remove(colorKey);
//                    }
//                }
//                colorList =
//                    GetArchiveColorAllocationDatabaseBin().ReadStoreVariables(header._dba, hcdbk); // TT#707 - Container not thread safe (part 2) JEllis 
//                if (colorList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderColorDatabaseBinKey> svd in colorList)
//                    {
//                        _archiveColorAllocationContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                }
//                foreach (HeaderColorSizeDatabaseBinKey sizeKey in _archiveSizeAllocationContainer.Keys)
//                {
//                    if (aHdrRID == sizeKey.HdrRID)
//                    {
//                        _archiveSizeAllocationContainer.Remove(sizeKey);
//                    }
//                }
//                sizeList =
//                    GetArchiveColorSizeAllocationDatabaseBin().ReadStoreVariables(header._dba, hcsdbk); // TT#707 - Container not thread safe (part 2) JEllis 
//                if (sizeList.Count > 0)
//                {
//                    foreach (StoreVariableData<HeaderColorSizeDatabaseBinKey> svd in sizeList)
//                    {
//                        _archiveSizeAllocationContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                else
//                {
//                }
//                try
//                {
//                    _isHeaderCached.Add(aHdrRID, true);
//                }
//                catch (ArgumentException)
//                {
//                    //  Key already exists!
//                }
//            }
//            catch
//            {
//                _isHeaderCached.Remove(aHdrRID);
//                _archiveSummaryAllocationContainer.Remove(hdbkTotal);
//                _archiveSummaryAllocationContainer.Remove(hdbkDetail);
//                _archiveSummaryAllocationContainer.Remove(hdbkBulk);

//                foreach (HeaderPackDatabaseBinKey packKey in _archivePackAllocationContainer.Keys)
//                {
//                    if (aHdrRID == packKey.HdrRID)
//                    {
//                        _archivePackAllocationContainer.Remove(packKey);
//                    }
//                }
//                foreach (HeaderColorDatabaseBinKey colorKey in _archiveColorAllocationContainer.Keys)
//                {
//                    if (aHdrRID == colorKey.HdrRID)
//                    {
//                        _archiveColorAllocationContainer.Remove(colorKey);
//                    }
//                }
//                foreach (HeaderColorSizeDatabaseBinKey sizeKey in _archiveSizeAllocationContainer.Keys)
//                {
//                    if (aHdrRID == sizeKey.HdrRID)
//                    {
//                        _archiveSizeAllocationContainer.Remove(sizeKey);
//                    }
//                }
//                throw;
//            }
//            finally
//            {
//            }
//        }
//        /// <summary>
//        /// Creates new archive by setting the "isHeaderCached" flag to true and removing any existing entries from container
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        public void CreateNewArchive(int aHdrRID)
//        {
//            HeaderSummaryDatabaseBinKey hdbkTotal = new HeaderSummaryDatabaseBinKey(aHdrRID, eAllocationSummaryNode.Total);
//            HeaderSummaryDatabaseBinKey hdbkDetail = new HeaderSummaryDatabaseBinKey(aHdrRID, eAllocationSummaryNode.DetailSubType);
//            HeaderSummaryDatabaseBinKey hdbkBulk = new HeaderSummaryDatabaseBinKey(aHdrRID, eAllocationSummaryNode.Bulk);
//            _archiveSummaryAllocationContainer.Remove(hdbkTotal);
//            _archiveSummaryAllocationContainer.Remove(hdbkDetail);
//            _archiveSummaryAllocationContainer.Remove(hdbkBulk);
//            foreach (HeaderPackDatabaseBinKey packKey in _archivePackAllocationContainer.Keys)
//            {
//                if (aHdrRID == packKey.HdrRID)
//                {
//                    _archivePackAllocationContainer.Remove(packKey);
//                }
//            }
//            foreach (HeaderColorDatabaseBinKey colorKey in _archiveColorAllocationContainer.Keys)
//            {
//                if (aHdrRID == colorKey.HdrRID)
//                {
//                    _archiveColorAllocationContainer.Remove(colorKey);
//                }
//            }
//            foreach (HeaderColorSizeDatabaseBinKey sizeKey in _archiveSizeAllocationContainer.Keys)
//            {
//                if (aHdrRID == sizeKey.HdrRID)
//                {
//                    _archiveSizeAllocationContainer.Remove(sizeKey);
//                }
//            }
//            _isHeaderCached.Remove(aHdrRID);
//            _isHeaderCached.Add(aHdrRID, true);
//        }
//        #endregion ReadStoreAllocationArchive

//        #endregion GetStoreVariableVectorContainer

//        #region WriteStoreAllocationArchive

//        /// <summary>
//        /// Writes the store allocation archive for each header represented in the container
//        /// </summary>
//        /// <param name="aHeaderDataRecord">Header data record containing an open connection</param>
//        /// <param name="aHdrKeysToWrite">Array of header keys to write</param>
//        public void WriteStoreAllocationArchive(Header aHeaderDataRecord, int[] aHdrKeysToWrite) // TT#467 Store Container Enqueue changes
//        {
//            // begin TT#467 Store Container Enqueue changes
//            HeaderSummaryDatabaseBinKey[] summaryKeysToWrite = 
//                new HeaderSummaryDatabaseBinKey[aHdrKeysToWrite.Length * 3];
//            List<HeaderPackDatabaseBinKey> packKeysToWrite = 
//                new List<HeaderPackDatabaseBinKey>();
//            List<HeaderColorDatabaseBinKey> colorKeysToWrite = 
//                new List<HeaderColorDatabaseBinKey>();
//            List<HeaderColorSizeDatabaseBinKey> sizeKeysToWrite = 
//                new List<HeaderColorSizeDatabaseBinKey>();
//            int summaryIndex;
//            for (int i = 0; i < aHdrKeysToWrite.Length; i++)
//            {
//                summaryIndex = i * 3;
//                summaryKeysToWrite[summaryIndex] = 
//                    new HeaderSummaryDatabaseBinKey(aHdrKeysToWrite[i], eAllocationSummaryNode.Total);
//                summaryKeysToWrite[summaryIndex + 1] = 
//                    new HeaderSummaryDatabaseBinKey(aHdrKeysToWrite[i], eAllocationSummaryNode.DetailType);
//                summaryKeysToWrite[summaryIndex + 2] = 
//                    new HeaderSummaryDatabaseBinKey(aHdrKeysToWrite[i], eAllocationSummaryNode.Bulk);
//                foreach (HeaderPackDatabaseBinKey hpdk in _archivePackAllocationContainer.Keys)
//                {
//                    if (hpdk.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        packKeysToWrite.Add(hpdk);
//                    }
//                }
//                foreach (HeaderColorDatabaseBinKey hcdk in _archiveColorAllocationContainer.Keys)
//                {
//                    if (hcdk.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        colorKeysToWrite.Add(hcdk);
//                    }
//                }
//                foreach (HeaderColorSizeDatabaseBinKey hcsdk in _archiveSizeAllocationContainer.Keys)
//                {
//                    if (hcsdk.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        sizeKeysToWrite.Add(hcsdk);
//                    }
//                }
//            }
//            GetArchiveSummaryAllocationDatabaseBin().WriteStoreVariables( // TT#707 - Container not thread safe (part 2) JEllis 
//                aHeaderDataRecord._dba, 
//                _archiveSummaryAllocationContainer,
//                summaryKeysToWrite);
//            if (packKeysToWrite.Count > 0)
//            {
//                HeaderPackDatabaseBinKey[] packKeys =
//                    new HeaderPackDatabaseBinKey[packKeysToWrite.Count];
//                packKeysToWrite.CopyTo(packKeys);
//                GetArchivePackAllocationDatabaseBin().WriteStoreVariables( // TT#707 - Container not thread safe (part 2) JEllis 
//                    aHeaderDataRecord._dba,
//                    _archivePackAllocationContainer,
//                    packKeys);
//            }
//            if (colorKeysToWrite.Count > 0)
//            {
//                HeaderColorDatabaseBinKey[] colorKeys =
//                    new HeaderColorDatabaseBinKey[colorKeysToWrite.Count];
//                colorKeysToWrite.CopyTo(colorKeys);
//                GetArchiveColorAllocationDatabaseBin().WriteStoreVariables( // TT#707 - Container not thread safe (part 2) JEllis 
//                    aHeaderDataRecord._dba,
//                    _archiveColorAllocationContainer,
//                    colorKeys);
//            }
//            if (sizeKeysToWrite.Count > 0)
//            {
//                HeaderColorSizeDatabaseBinKey[] sizeKeys =
//                    new HeaderColorSizeDatabaseBinKey[sizeKeysToWrite.Count];
//                sizeKeysToWrite.CopyTo(sizeKeys);
//                GetArchiveColorSizeAllocationDatabaseBin().WriteStoreVariables( // TT#707 - Container not thread safe (part 2) JEllis 
//                    aHeaderDataRecord._dba, 
//                    _archiveSizeAllocationContainer,
//                    sizeKeys);
//            }
//            // end TT#467 Store Container Enqueue changes
//        }

//        #endregion WriteStoreAllocationArchive

//        //public void DeleteStoreAllocationArchive()
//        //{
//        //    //ArchiveSummaryAllocationDatabaseBin.
//        //}
//        private ArchiveSummaryAllocationDatabaseBin GetArchiveSummaryAllocationDatabaseBin()
//        {
//            if (_archiveSummaryAllocationDatabaseBin == null)
//            {
//                _archiveSummaryAllocationDatabaseBin = new ArchiveSummaryAllocationDatabaseBin();
//            }
//            return _archiveSummaryAllocationDatabaseBin;
//        }
//        private ArchivePackAllocationDatabaseBin GetArchivePackAllocationDatabaseBin()
//        {
//            if (_archivePackAllocationDatabaseBin == null)
//            {
//                _archivePackAllocationDatabaseBin = new ArchivePackAllocationDatabaseBin();
//            }
//            return _archivePackAllocationDatabaseBin;
//        }
//        private ArchiveColorAllocationDatabaseBin GetArchiveColorAllocationDatabaseBin()
//        {
//            if (_archiveColorAllocationDatabaseBin == null)
//            {
//                _archiveColorAllocationDatabaseBin = new ArchiveColorAllocationDatabaseBin();
//            }
//            return _archiveColorAllocationDatabaseBin;
//        }
//        private ArchiveColorSizeAllocationDatabaseBin GetArchiveColorSizeAllocationDatabaseBin()
//        {
//            if (_archiveColorSizeAllocationDatabaseBin == null)
//            {
//                _archiveColorSizeAllocationDatabaseBin = new ArchiveColorSizeAllocationDatabaseBin();
//            }
//            return _archiveColorSizeAllocationDatabaseBin;
//        }

//        #region Flush
//        /// <summary>
//        /// Flushes the containers associated with the given header RIDs from memory
//        /// </summary>
//        /// <param name="aHnRIDs">Array of the header RIDs to flush</param>
//        public void Flush(int[] aHdrRIDs)
//        {
//            List<HeaderSummaryDatabaseBinKey> summaryKeys = new List<HeaderSummaryDatabaseBinKey>();
//            List<HeaderPackDatabaseBinKey> packKeys = new List<HeaderPackDatabaseBinKey>();
//            List<HeaderColorDatabaseBinKey> colorKeys = new List<HeaderColorDatabaseBinKey>();
//            List<HeaderColorSizeDatabaseBinKey> sizeKeys = new List<HeaderColorSizeDatabaseBinKey>();
//            for (int i = 0; i < aHdrRIDs.Length; i++)
//            {
//                foreach (HeaderSummaryDatabaseBinKey hsbk in _archiveSummaryAllocationContainer.Keys)
//                {
//                    if (hsbk.HdrRID == aHdrRIDs[i])
//                    {
//                        summaryKeys.Add(hsbk);
//                    }
//                }
//                foreach (HeaderPackDatabaseBinKey hpbk in _archivePackAllocationContainer.Keys)
//                {
//                    if (hpbk.HdrRID == aHdrRIDs[i])
//                    {
//                        packKeys.Add(hpbk);
//                    }
//                }
//                foreach (HeaderColorDatabaseBinKey hcbk in _archiveColorAllocationContainer.Keys)
//                {
//                    if (hcbk.HdrRID == aHdrRIDs[i])
//                    {
//                        colorKeys.Add(hcbk);
//                    }
//                }
//                foreach (HeaderColorSizeDatabaseBinKey hsbk in _archiveSizeAllocationContainer.Keys)
//                {
//                    if (hsbk.HdrRID == aHdrRIDs[i])
//                    {
//                        sizeKeys.Add(hsbk);
//                    }
//                }
//                _isHeaderCached.Remove(aHdrRIDs[i]); // TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//            }
//            foreach (HeaderSummaryDatabaseBinKey hsbk in summaryKeys)
//            {
//                _archiveSummaryAllocationContainer.Remove(hsbk);
//            }
//            foreach (HeaderPackDatabaseBinKey hpbk in packKeys)
//            {
//                _archivePackAllocationContainer.Remove(hpbk);
//            }
//            foreach (HeaderColorDatabaseBinKey hcbk in colorKeys)
//            {
//                _archiveColorAllocationContainer.Remove(hcbk);
//            }
//            foreach (HeaderColorSizeDatabaseBinKey hsbk in sizeKeys)
//            {
//                _archiveSizeAllocationContainer.Remove(hsbk);
//            }
//            _lastColorDatabaseBinKey = null;
//            _lastPackDatabaseBinKey = null;
//            _lastSizeDatabaseBinKey = null;
//            _lastSummaryDatabaseBinKey = null;
//            _lastStoreColorVectorContainer = null;
//            _lastStorePackVectorContainer = null;
//            _lastStoreSizeVectorContainer = null;
//            _lastStoreSummaryVectorContainer = null;
//        }
//        /// <summary>
//        /// Flushes the all containers from memory
//        /// </summary>
//        public void FlushAll()
//        {
//            _archiveSummaryAllocationContainer.Clear();
//            _archivePackAllocationContainer.Clear();
//            _archiveColorAllocationContainer.Clear();
//            _archiveSizeAllocationContainer.Clear();
//            _lastColorDatabaseBinKey = null;
//            _lastPackDatabaseBinKey = null;
//            _lastSizeDatabaseBinKey = null;
//            _lastSummaryDatabaseBinKey = null;
//            _lastStoreColorVectorContainer = null;
//            _lastStorePackVectorContainer = null;
//            _lastStoreSizeVectorContainer = null;
//            _lastStoreSummaryVectorContainer = null;
//            _isHeaderCached.Clear();  // TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//        }
//        #endregion Flush

//        #region Dispose
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        public void Dispose()
//        {
//            Dispose(true);
//            System.GC.SuppressFinalize(this);
//        }
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        /// <param name="disposing">
//        /// True:  dispose all managed and unmanaged resources.
//        /// False: dispose only unmanaged resources
//        /// </param>
//        protected void Dispose(bool disposing)
//        {
//            if (!_isDisposed)
//            {
//                if (disposing)
//                {
//                    //if (ConnectionIsOpen)
//                    //{
//                    //    CloseUpdateConnection();
//                    //}
//                    _archiveColorAllocationContainer = null;
//                    _isHeaderCached = null;
//                    _archivePackAllocationContainer = null;
//                    _archiveSizeAllocationContainer = null;
//                    _archiveSummaryAllocationContainer = null;
//                }
//                _isDisposed = true;
//            }
//        }
//        ~StoreAllocationArchive()
//        {
//            Dispose(false);
//        }
//        #endregion Dispose
//    }
//    #endregion ArchiveAllocation

//    // end TT#370 Build Packs Enhancement

//    // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//    #region Vsw Reverse Onhand
//    /// <summary>
//    /// Container for Store VSW Reverse Onhand Variables.
//    /// </summary>
//    [Serializable]
//    public class StoreVswReverseOnhand : IDisposable
//    {
//        private bool _isDisposed;
//        private Dictionary<int, bool> _isHeaderCached; 
//        private VswReverseOnhandDatabaseBin _vswReverseOnhandDatabaseBin;
//        private StoreVariableDataDictionary<VswReverseOnhandDatabaseBinKey> _vswReverseOnhandContainer;
//        private VswReverseOnhandDatabaseBinKey _lastVswReverseOnhandDatabaseBinKey;
//        private StoreVariableVectorContainer _lastStoreVswReverseOnhandVectorContainer;

//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        public StoreVswReverseOnhand()
//        {
//            _isDisposed = false;
//            _isHeaderCached = new Dictionary<int, bool>();
//            _vswReverseOnhandContainer = new StoreVariableDataDictionary<VswReverseOnhandDatabaseBinKey>();
//        }


//        #region StoreVariableValue
//        #region GetStoreVariableValue
//        #region GetStoreVswReverseOnhandValue
//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Vsw Reverse Onhand value.</returns>
//        public double GetStoreVswReverseOnhandValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHnRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreVswReverseOnhandValue
//                (aDatabaseVariableName,
//                aHdrRID,
//                aHnRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Vsw Reverse Onhand value.</returns>
//        public double GetStoreVswReverseOnhandValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHnRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreVswReverseOnhandValue
//                (aVariableID,
//                aHdrRID,
//                aHnRID,
//                storeRID)[0];
//        }
//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RID</param>
//        /// <returns>Store's Vsw Reverse Onhand value.</returns>
//        public double GetStoreVswReverseOnhandValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHnRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreVswReverseOnhandValue
//                (aVariableIDX,
//                aHdrRID,
//                aHnRID,
//                storeRID)[0];
//        }

//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of variable in the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Vsw Reverse Onhand values in same order as provided store list.</returns>
//        public double[] GetStoreVswReverseOnhandValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHnRID,
//            int[] aStoreRID)
//        {
//            return GetStoreVswReverseOnhandValue(
//                (short)aVariableIDX,
//                aHdrRID,
//                aHnRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Vsw Reverse Onhand values in same order as provided store list.</returns>
//        public double[] GetStoreVswReverseOnhandValue
//          (string aDatabaseVariableName,
//           int aHdrRID,
//           int aHnRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetVswReverseOnhandDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))   
//            {
//                return GetStoreVswReverseOnhandValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHnRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=[" + aDatabaseVariableName + "] is not a valid variable for table [= " + GetVswReverseOnhandDatabaseBin().BinTableName + "]"); 
//        }
//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Vsw Reverse Onhand values in same order as provided store list.</returns>
//        public double[] GetStoreVswReverseOnhandValue
//          (int aVariableID,
//           int aHdrRID,
//           int aHnRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetVswReverseOnhandDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))
//            {
//                return GetStoreVswReverseOnhandValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHnRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=[" + aVariableID.ToString() + "] is not a valid variable for table [= " + GetVswReverseOnhandDatabaseBin().BinTableName + "]"); 
//        }
//        /// <summary>
//        /// Gets the store's VSW Reverse Onhand value for the given header, Hierarchy Node
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container.</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
//        /// <param name="aStoreRID">Store RIDs</param>
//        /// <returns>Store's Vsw Reverse Onhand values in same order as provided store list.</returns>
//        public double[] GetStoreVswReverseOnhandValue
//           (short aVariableIDX,
//            int aHdrRID,
//            int aHnRID,
//            int[] aStoreRID)
//        {
//            StoreVariableVectorContainer svvc = GetStoreVswReverseOnhandVariableVectorContainer(aHdrRID, aHnRID);
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetVswReverseOnhandDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); 
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }

//        #endregion GetStoreVswReverseOnhandValue
//        #endregion GetStoreVariableValue

//        #region SetStoreVariableValue
//        #region SetStoreVswReverseOnhandValue
//        /// <summary>
//        /// Sets the store VSW Reverse Onhand variable value for a given header, Hierarchy Node 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">Hierarchy RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreVswReverseOnhandValue
//            (UInt16 aVariableIDX,
//            int aHdrRID,
//            int aHnRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            SetStoreVswReverseOnhandValue
//                (aVariableIDX,
//                aHdrRID,
//                aHnRID,
//                aStoreRIDs,
//                aVariableValues
//                );
//        }
//        /// <summary>
//        /// Sets the store VSW Reverse Onhand variable value for a given header, Hierarchy Node 
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">Variable Index (Position of the variable within the container)</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">Hierarchy RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreVswReverseOnhandValue
//            (string aDatabaseVariableName,
//            int aHdrRID,
//            int aHnRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetVswReverseOnhandDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))
//            {
//                SetStoreVswReverseOnhandValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHnRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetVswReverseOnhandDatabaseBin().BinTableName); 
//        }
//        /// <summary>
//        /// Sets the store VSW Reverse Onhand variable value for a given header, Hierarchy Node 
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">Hierarchy RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreVswReverseOnhandValue
//            (int aVariableID,
//            int aHdrRID,
//            int aHnRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetVswReverseOnhandDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx))
//            {
//                SetStoreVswReverseOnhandValue
//                    (variableIdx,
//                     aHdrRID,
//                     aHnRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetVswReverseOnhandDatabaseBin().BinTableName);
//        }
//        /// <summary>
//        /// Sets the store VSW Reverse Onhand variable value for a given header, Hierarchy Node 
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">Hierarchy RID</param>
//        /// <param name="aStoreRIDs">Store RID Array</param>
//        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
//        public void SetStoreVswReverseOnhandValue
//            (short aVariableIDX,
//             int aHdrRID,
//             int aHnRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            int storeRID;
//            Int64 variableValue;
//            StoreVariableVectorContainer svvc = GetStoreVswReverseOnhandVariableVectorContainer(aHdrRID, aHnRID);
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetVswReverseOnhandDatabaseBin().VariableModel.VariableModelID; 
//            double multiplier = Math.Pow(10, GetVswReverseOnhandDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  
//            for (int i = 0; i < aStoreRIDs.Length; i++)
//            {
//                storeRID = aStoreRIDs[i];
//                if (aVariableValues[i] < 0)
//                {
//                    variableValue =
//                       (Int64)(aVariableValues[i]
//                               * multiplier
//                               - .5);   // rounding should use same sign as original value
//                }
//                else
//                {
//                    variableValue =
//                        (Int64)(aVariableValues[i]
//                                * multiplier
//                                + .5);
//                }
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//            }
//            return;
//        }
//        #endregion SetStoreVswReverseOnhandValue
//        #endregion SetStoreVariableValue
//        #endregion StoreVariableValue

//        #region GetStoreVariableVectorContainer

//        #region GetStoreVswReverseOnhandVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        /// <param name="aHnRID">Hierarchy Node RID</param>
//        /// <returns>VswReverseOnhandVariableVectorArray for the specified key values</returns>
//        private StoreVariableVectorContainer GetStoreVswReverseOnhandVariableVectorContainer(int aHdrRID, int aHnRID)
//        {
//            if (_lastVswReverseOnhandDatabaseBinKey == null
//                || _lastVswReverseOnhandDatabaseBinKey.HdrRID != aHdrRID
//                || _lastVswReverseOnhandDatabaseBinKey.HnRID != aHnRID)
//            {
//                VswReverseOnhandDatabaseBinKey vrobk = new VswReverseOnhandDatabaseBinKey(aHdrRID, aHnRID);
//                StoreVariableData<VswReverseOnhandDatabaseBinKey> svd;
//                if (!_vswReverseOnhandContainer.TryGetValue(vrobk, out svd))
//                {
//                    bool headerCached = false;
//                    if (!_isHeaderCached.TryGetValue(aHdrRID, out headerCached)
//                        || !headerCached)
//                    {
//                        this.ReadStoreVswReverseOnhand(aHdrRID);
//                    }
//                    if (!_vswReverseOnhandContainer.TryGetValue(vrobk, out svd))
//                    {
//                        svd = new StoreVariableData<VswReverseOnhandDatabaseBinKey>(0, vrobk, new StoreVariableVectorContainer());
//                        _vswReverseOnhandContainer.Add(vrobk, svd);
//                    }
//                }
//                _lastVswReverseOnhandDatabaseBinKey = vrobk;
//                _lastStoreVswReverseOnhandVectorContainer = svd.StoreVariableVectorContainer;
//            }
//            return _lastStoreVswReverseOnhandVectorContainer;
//        }
//        #endregion GetStoreVswReverseOnhandVectorContainer
        
//        #region IsHeaderCached
//        public bool IsHeaderCached(int aHdrRID)
//        {
//            bool cached = false;
//            if (_isHeaderCached.TryGetValue(aHdrRID,out cached))
//            {
//                return cached;
//            }
//            return false;
//        }
//        #endregion IsHeaderCached
        
//        #region ReadStoreVswReverseOnhand
//        /// <summary>
//        /// Get VSW Reverse Onhand for the given header RID
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        public void ReadStoreVswReverseOnhand(int aHdrRID)
//        {
//            VswReverseOnhandDatabaseBinKey vrodbk = new VswReverseOnhandDatabaseBinKey(aHdrRID);  // Key for ALL HnRIDs associated with this header
//            List<StoreVariableData<VswReverseOnhandDatabaseBinKey>> vswReverseOnhandList = new List<StoreVariableData<VswReverseOnhandDatabaseBinKey>>();

//            try
//            {
//                Header header = new Header();
//                vswReverseOnhandList =
//                    GetVswReverseOnhandDatabaseBin().ReadStoreVariables(header._dba, vrodbk);

//                if (vswReverseOnhandList.Count > 0)
//                {
//                    foreach (StoreVariableData<VswReverseOnhandDatabaseBinKey> svd in vswReverseOnhandList)
//                    {
//                        _vswReverseOnhandContainer.Add(svd.DatabaseBinKey, svd);
//                    }
//                }
//                try
//                {
//                    _isHeaderCached.Add(aHdrRID, true);
//                }
//                catch (ArgumentException)
//                {
//                    //  Key already exists!
//                }
//            }
//            catch
//            {
//                _isHeaderCached.Remove(aHdrRID);
//                _vswReverseOnhandContainer.Remove(vrodbk);
//                throw;
//            }
//            finally
//            {
//            }
//        }
//        /// <summary>
//        /// Creates new Vsw Reverse Onhand by setting the "isHeaderCached" flag to true and removing any existing entries from container
//        /// </summary>
//        /// <param name="aHdrRID">Header RID</param>
//        public void CreateNewVswReverseOnhand(int aHdrRID)
//        {
//            List<VswReverseOnhandDatabaseBinKey> deleteList = new List<VswReverseOnhandDatabaseBinKey>();
//            foreach (VswReverseOnhandDatabaseBinKey vroKey in _vswReverseOnhandContainer.Keys)
//            {
//                if (aHdrRID == vroKey.HdrRID)
//                {
//                    deleteList.Add(vroKey);
//                }
//            }
//            foreach (VswReverseOnhandDatabaseBinKey vroKey in deleteList)
//            {
//                _vswReverseOnhandContainer.Remove(vroKey);
//            }
//            _isHeaderCached.Remove(aHdrRID);
//            _isHeaderCached.Add(aHdrRID, true);
//        }
//        #endregion ReadStoreAllocationArchive

//        #endregion GetStoreVariableVectorContainer

//        #region WriteStoreVswReverseOnhand

//        /// <summary>
//        /// Writes the store VSW Reverse Onhand for each header represented in the container
//        /// </summary>
//        /// <param name="aHeaderDataRecord">Header data record containing an open connection</param>
//        /// <param name="aHdrKeysToWrite">Array of header keys to write</param>
//        public void WriteStoreVswReverseOnhand(Header aHeaderDataRecord, int[] aHdrKeysToWrite)
//        {
//            List<VswReverseOnhandDatabaseBinKey> keysToWrite =
//                new List<VswReverseOnhandDatabaseBinKey>();
//            for (int i = 0; i < aHdrKeysToWrite.Length; i++)
//            {
//                foreach (VswReverseOnhandDatabaseBinKey vroKey in _vswReverseOnhandContainer.Keys)
//                {
//                    if (vroKey.HdrRID == aHdrKeysToWrite[i])
//                    {
//                        keysToWrite.Add(vroKey);                        
//                    }
//                }
//            }
//            VswReverseOnhandDatabaseBinKey[] vroKeysToWrite = new VswReverseOnhandDatabaseBinKey[keysToWrite.Count];
//            keysToWrite.CopyTo(vroKeysToWrite,0);
//            GetVswReverseOnhandDatabaseBin().WriteStoreVariables(
//                            aHeaderDataRecord._dba,
//                            _vswReverseOnhandContainer,
//                            vroKeysToWrite);
//        }
//        #endregion WriteStoreVswReverseOnhand

       
//        #region DeleteStoreVswReverseOnhand
//        /// <summary>
//        /// Deletes StoreVswReverseOnhand from database for given keys
//        /// </summary>
//        /// <param name="aHeaderDataRecord">Database Access</param>
//        /// <param name="aHdrKeysToDelete">Header whose VSW Reverse Onhands is to be deleted</param>
//        public void DeleteStoreVswReverseOnhand(Header aHeaderDataRecord, int[] aHdrKeysToDelete)
//        {
//            Flush(aHdrKeysToDelete);
//            for (int i = 0; i < aHdrKeysToDelete.Length; i++)
//            {
//                VswReverseOnhandDatabaseBinKey vroKey = new VswReverseOnhandDatabaseBinKey(aHdrKeysToDelete[i]);
//                GetVswReverseOnhandDatabaseBin().DeleteStoreVariables(
//                    aHeaderDataRecord._dba,
//                    vroKey);
//            }
//        }
//        #endregion DeleteStoreVswReverseOnhand

//        public VswReverseOnhandDatabaseBin GetVswReverseOnhandDatabaseBin()
//        {
//            if (_vswReverseOnhandDatabaseBin == null)
//            {
//                _vswReverseOnhandDatabaseBin = new VswReverseOnhandDatabaseBin();
//            }
//            return _vswReverseOnhandDatabaseBin;
//        }
//        #region Flush
//        /// <summary>
//        /// Flushes the containers associated with the given header RIDs from memory
//        /// </summary>
//        /// <param name="aHnRIDs">Array of the header RIDs to flush</param>
//        public void Flush(int[] aHdrRIDs)
//        {
//            List<VswReverseOnhandDatabaseBinKey> vroKeys = new List<VswReverseOnhandDatabaseBinKey>();
//            for (int i = 0; i < aHdrRIDs.Length; i++)
//            {
//                foreach (VswReverseOnhandDatabaseBinKey vrobk in _vswReverseOnhandContainer.Keys)
//                {
//                    if (vrobk.HdrRID == aHdrRIDs[i])
//                    {
//                        vroKeys.Add(vrobk);
//                    }
//                }
//                _isHeaderCached.Remove(aHdrRIDs[i]);
//            }
//            foreach (VswReverseOnhandDatabaseBinKey vrobk in vroKeys)
//            {
//                _vswReverseOnhandContainer.Remove(vrobk);
//            }
//            _lastVswReverseOnhandDatabaseBinKey = null;
//            _lastStoreVswReverseOnhandVectorContainer = null;
//        }
//        /// <summary>
//        /// Flushes the all containers from memory
//        /// </summary>
//        public void FlushAll()
//        {
//            _vswReverseOnhandContainer.Clear();
//            _isHeaderCached.Clear();
//            _lastVswReverseOnhandDatabaseBinKey = null;
//            _lastStoreVswReverseOnhandVectorContainer = null;
//        }
//        #endregion Flush

//        #region Dispose
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        public void Dispose()
//        {
//            Dispose(true);
//            System.GC.SuppressFinalize(this);
//        }
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        /// <param name="disposing">
//        /// True:  dispose all managed and unmanaged resources.
//        /// False: dispose only unmanaged resources
//        /// </param>
//        protected void Dispose(bool disposing)
//        {
//            if (!_isDisposed)
//            {
//                if (disposing)
//                {
//                    //if (ConnectionIsOpen)
//                    //{
//                    //    CloseUpdateConnection();
//                    //}
//                    _isHeaderCached = null;
//                    _vswReverseOnhandContainer = null;
//                }
//                _isDisposed = true;
//            }
//        }
//        ~StoreVswReverseOnhand()
//        {
//            Dispose(false);
//        }
//        #endregion Dispose
//    }
//    #endregion VswReverseOnhand

//    // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//}