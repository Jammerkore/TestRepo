using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.IO;  
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    #region ABSTRACT DatabaseBinKey
    /// <summary>
    /// Abstract description of a database bin key
    /// </summary>
    [Serializable]
    public abstract class DatabaseBinKey
    {
        private eDatabaseBinKeyType _databaseKeyType;
        private int[] _databaseKeys;

        /// <summary>
        /// Creates an instance of a database bin key having a single key item
        /// </summary>
        /// <param name="aDatabaseKeyType">Key Type</param>
        /// <param name="aKeyValue">Key  item value</param>
        public DatabaseBinKey(eDatabaseBinKeyType aDatabaseKeyType, int aKeyValue)
        {
            _databaseKeyType = aDatabaseKeyType;
            _databaseKeys = new int[1];
            _databaseKeys[0] = aKeyValue;
        }
        /// <summary>
        /// Creates an instance of a database bin key having 2 key items
        /// </summary>
        /// <param name="aDatabaseKeyType">Key Type</param>
        /// <param name="aKeyValue1">First key item value</param>
        /// <param name="aKeyValue2">Second key item value</param>
        public DatabaseBinKey(eDatabaseBinKeyType aDatabaseKeyType, int aKeyValue1, int aKeyValue2)
        {
            _databaseKeyType = aDatabaseKeyType;
            _databaseKeys = new int[2];
            _databaseKeys[0] = aKeyValue1;
            _databaseKeys[1] = aKeyValue2;
        }
        /// <summary>
        /// Creates an instance of a database bin key having 3 key items
        /// </summary>
        /// <param name="aDatabaseKeyType">Key Type</param>
        /// <param name="aKeyValue1">First key item value</param>
        /// <param name="aKeyValue2">Second key item value</param>
        /// <param name="aKeyValue3">Third key item value</param>
        public DatabaseBinKey(eDatabaseBinKeyType aDatabaseKeyType, int aKeyValue1, int aKeyValue2, int aKeyValue3)
        {
            _databaseKeyType = aDatabaseKeyType;
            _databaseKeys = new int[3];
            _databaseKeys[0] = aKeyValue1;
            _databaseKeys[1] = aKeyValue2;
            _databaseKeys[2] = aKeyValue3;
        }
        /// <summary>
        /// Creates an instance of a database bin key having 4 key items
        /// </summary>
        /// <param name="aDatabaseKeyType">Key Type</param>
        /// <param name="aKeyValue1">First key item value</param>
        /// <param name="aKeyValue2">Second key item value</param>
        /// <param name="aKeyValue3">Third key item value</param>
        /// <param name="aKeyValue4">Fourth key item value</param>
        public DatabaseBinKey(eDatabaseBinKeyType aDatabaseKeyType, int aKeyValue1, int aKeyValue2, int aKeyValue3, int aKeyValue4)
        {
            _databaseKeyType = aDatabaseKeyType;
            _databaseKeys = new int[4];
            _databaseKeys[0] = aKeyValue1;
            _databaseKeys[1] = aKeyValue2;
            _databaseKeys[2] = aKeyValue3;
            _databaseKeys[3] = aKeyValue4;
        }
        /// <summary>
        /// Creates an instance of a database bin key having 5 key items
        /// </summary>
        /// <param name="aDatabaseKeyType">Key Type</param>
        /// <param name="aKeyValue1">First key item value</param>
        /// <param name="aKeyValue2">Second key item value</param>
        /// <param name="aKeyValue3">Third key item value</param>
        /// <param name="aKeyValue4">Fourth key item value</param>
        /// <param name="aKeyValue5">Fifth key item value</param>
        public DatabaseBinKey(eDatabaseBinKeyType aDatabaseKeyType, int aKeyValue1, int aKeyValue2, int aKeyValue3, int aKeyValue4, int aKeyValue5)
        {
            _databaseKeyType = aDatabaseKeyType;
            _databaseKeys = new int[5];
            _databaseKeys[0] = aKeyValue1;
            _databaseKeys[1] = aKeyValue2;
            _databaseKeys[2] = aKeyValue3;
            _databaseKeys[3] = aKeyValue4;
            _databaseKeys[4] = aKeyValue5;
        }
        /// <summary>
        /// Gets the Database Bin Key Type for this key
        /// </summary>
        public eDatabaseBinKeyType DatabaseBinKeyType
        {
            get { return _databaseKeyType; }
        }
        /// <summary>
        /// Gets the number of key items in this DatabaseBinKey
        /// </summary>
        public int Count
        {
            get { return _databaseKeys.Length; }
        }
        /// <summary>
        /// Gets an SQL "where" select clause segment corresponding to this key
        /// </summary>
        public string SQL_WhereClauseSegment
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                string[] connector = { " ", " and " };
                int connectIdx = 0;
                for (int i = 0; i < Count; i++)
                {
                    if (_databaseKeys[i] != Include.NoRID)
                    {
                        sb.Append(connector[connectIdx] + GetKeyWord(i) + "=");
                        sb.Append(_databaseKeys[i].ToString());
                        connectIdx = 1;
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets an array of MIDDbParameters corresponding to this key
        /// </summary>
        public MIDDbParameter[] SQL_UpdtProcedureInputParms
        {
            get
            {
                MIDDbParameter[] parameter = new MIDDbParameter[Count];
                for (int i = 0; i < Count; i++)
                {
                    parameter[i] = new MIDDbParameter(GetKeyWord(i), _databaseKeys[i], GetDbType(i), eParameterDirection.Input);
                }
                return parameter;
            }
        }
        /// <summary>
        /// Gets the key item value within this DatabaseBinKey associated with the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">The Index of the key item</param>
        /// <returns>Key Value associated with the index</returns>
        public int GetKeyValue(int aKeyIndex)
        {
            return _databaseKeys[aKeyIndex];
        }
        /// <summary>
        /// Gets the key item value within this DatabaseBinKey associated with the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">The Index of the key item</param>
        /// <param name="aKeyValue">Returned Key Value associated with the index</param>
        /// <returns>TRUE: aKeyValue is the value associated with the given KeyIndex; FALSE: KeyIndex is not valid</returns>
        public bool TryGetKeyValue(int aKeyIndex, out int aKeyValue)
        {
            if (aKeyIndex < Count)
            {
                if (aKeyIndex < 0)
                {
                }
                else
                {
                    aKeyValue = GetKeyValue(aKeyIndex);
                    return true;
                }
            }
            aKeyValue = 0;
            return false;
        }

        /// <summary>
        /// Gets the SQL key word for this DatabaseBinKey associated with the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">The index of the key item</param>
        /// <returns>The keyword associated with the given index</returns>
        public abstract string GetKeyWord(int aKeyIndex);
        /// <summary>
        /// Gets the key item index for this DatabaseBinKey assoicated with the gien KeyWord
        /// </summary>
        /// <param name="aKeyWord">Key word for which the index is desired</param>
        /// <returns>Key index associated with the key word</returns>
        public abstract int GetKeyWordIndex(string aKeyWord);
        /// <summary>
        /// Creates a new instance of the DatabaseBinKey using the supplied keyvalue array
        /// </summary>
        /// <param name="aKeyValueArray">Key item array of values</param>
        /// <returns>DatabbaseBinKey instance for the given key</returns>
        public abstract DatabaseBinKey CreateNewInstance(int[] aKeyValueArray);
        /// <summary>
        /// Getst the DbType of the key
        /// </summary>
        /// <param name="aKeyIndex">Key Index</param>
        /// <returns>DbType associated with the key</returns>
        public abstract eDbType GetDbType(int aKeyIndex);
        /// <summary>
        /// Compares the supplied object to this DatabaseBinKey for equality
        /// </summary>
        /// <param name="obj">Object to compare (Must be the same DatabaseKeyType as this)</param>
        /// <returns>TRUE: if equal; FALSE if not equal</returns>
        public override bool Equals(object obj)
        {
            DatabaseBinKey dbk = (obj as DatabaseBinKey);
            if (dbk == null
                || Count != dbk.Count
                || _databaseKeyType != dbk._databaseKeyType)
            {
                return false;
            }
            for (int i = 0; i < Count; i++)
            {
                if (_databaseKeys[i] != dbk._databaseKeys[i])
                {
                    return false;
                }
            }
            return true;
        }
         /// <summary>
        /// Gets the hash code for this DatabaseBinKey
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            int hashCode;
            if (4 < Count)
            {
                hashCode =
                    _databaseKeys[0]
                    ^ _databaseKeys[1]
                    ^ _databaseKeys[2]
                    ^ _databaseKeys[3]
                    ^ _databaseKeys[4];
            }
            else if (3 < Count)
            {
                hashCode =
                    _databaseKeys[0]
                    ^ _databaseKeys[1]
                    ^ _databaseKeys[2]
                    ^ _databaseKeys[3];
            }
                // begin TT#1504 - JEllis - Duplicate Key when Posting Daily
            //else if (3 < Count)
            //{
            //    hashCode =
            //        _databaseKeys[0]
            //        ^ _databaseKeys[1]
            //        ^ _databaseKeys[2]
            //        ^ _databaseKeys[3];
            //}
                // end TT#1504 - JEllis - Duplicate Key when Posting Daily
            else if (2 < Count)
            {
                hashCode =
                    _databaseKeys[0]
                    ^ _databaseKeys[1]
                    ^ _databaseKeys[2];
            }
            else if (1 < Count)
            {
                hashCode =
                    _databaseKeys[0]
                    ^ _databaseKeys[1];
            }
            else
            {
                hashCode = _databaseKeys[0];
            }
            return hashCode ^ (int)_databaseKeyType;
        }
    }
    #endregion ABSTRACT DatabaseBinKey

    #region History DatabaseBinKey
    /// <summary>
    /// Describe a history database Bin Key
    /// </summary>
    [Serializable]
    public class HistoryDatabaseBinKey : DatabaseBinKey
    {
        /// <summary>
        /// Creates an instance of a HistoryDatabaseBinKey
        /// </summary>
        /// <param name="aTimeID">Identifies the time ID item of this key</param>
        /// <param name="aHnRID">Identifies the Hierarchy Node RID item of this key</param>
        /// <param name="aColorCodeRID">Identifies the Color Code RID item of this key</param>
        /// <param name="aSizeCodeRID">Identifies the Size Code RID item of this key</param>
        public HistoryDatabaseBinKey(Int16 aTimeID, int aHnRID, int aColorCodeRID, int aSizeCodeRID) :
            base(eDatabaseBinKeyType.HistoryDatabaseBinKey, aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID)
        {
        }
        /// <summary>
        /// Gets the TimeID item of this key
        /// </summary>
        public Int16 TimeID
        {
            get { return (Int16)GetKeyValue(0); }
        }
        /// <summary>
        /// Gets the Hierarchy Node RID item of this key
        /// </summary>
        public int HnRID
        {
            get { return GetKeyValue(1); }
        }
        /// <summary>
        /// Gets the Color Code RID item of this key
        /// </summary>
        public int ColorCodeRID
        {
            get { return (Int16)GetKeyValue(2); }
        }
        /// <summary>
        /// Gets the Size Code RID item of this key
        /// </summary>
        public int SizeCodeRID
        {
            get { return (Int16)GetKeyValue(3); }
        }

        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">KeyIndex</param>
        /// <returns>KeyWord</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_TimeID;
                    }
                case 1:
                    {
                        return Include.DbKeyname_HnRID;
                    }
                case 2:
                    {
                        return Include.DbKeyname_ColorCodeRID;
                    }
                case 3:
                    {
                        return Include.DbKeyname_SizeCodeRID;
                    }
            }
            throw new IndexOutOfRangeException("HistoryDatabaseBinKey index must be 0, 1, 2, or 3");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_TimeID:
                    {
                        return 0;
                    }
                case Include.DbKeyname_HnRID:
                    {
                        return 1;
                    }
                case Include.DbKeyname_ColorCodeRID:
                    {
                        return 2;
                    }
                case Include.DbKeyname_SizeCodeRID:
                    {
                        return 3;
                    }
            }
            throw new IndexOutOfRangeException("HistoryDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.smallint;
                    }
                case 1:
                    {
                        return eDbType.Int;
                    }
                case 2:
                    {
                        return eDbType.Int;
                    }
                case 3:
                    {
                        return eDbType.Int;
                    }
            }
            throw new IndexOutOfRangeException("HistoryDatabaseBinKey index must be 0, 1, 2, or 3");
        }
        /// <summary>
        /// Creates a new instance of the HistoryDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HistoryDatabaseBinKey< for the given key value array/returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("HistoryDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());

            }
            // Begin TT#1504 - JEllis - Duplicate Key on Daily Post
            return new HistoryDatabaseBinKey((Int16)aKeyValueArray[0], aKeyValueArray[1], aKeyValueArray[2], aKeyValueArray[3]);
            //return new HistoryDatabaseBinKey((Int16)aKeyValueArray[0], aKeyValueArray[1], (Int16)aKeyValueArray[2], (Int16)aKeyValueArray[3]);
            // end TT#1504 - JEllis - Duplicate Key on Daily Post
        }
    }
    #endregion History DatabaseBinKey

    #region HeaderDatabaseBinKey
    /// <summary>
    /// Describes a HeaderDatabaseBin Key
    /// </summary>
    [Serializable]
    public class HeaderDatabaseBinKey : DatabaseBinKey
    {
        /// <summary>
        /// Creates an instance of the HeaderDatabaseBinKey
        /// </summary>
        /// <param name="aHdrRID">RID that identifies the header</param>
        public HeaderDatabaseBinKey(eDatabaseHeaderBinKeyType aDatabaseHeaderBinKey, int aHdrRID)
            : base((eDatabaseBinKeyType)aDatabaseHeaderBinKey, aHdrRID)
        {
        }
        /// <summary>
        /// Get the Header RID
        /// </summary>
        public int HdrRID
        {
            get { return GetKeyValue(0); }
        }
        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">KeyIndex</param>
        /// <returns>KeyWord</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_HdrRID;
                    }
            }
            throw new IndexOutOfRangeException("HeaderDatabaseBinKey index must be 0");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_HdrRID:
                    {
                        return 0;
                    }
            }
            throw new IndexOutOfRangeException("HeaderDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.Int;
                    }
            }
            throw new IndexOutOfRangeException("HeaderDatabaseBinKey index must be 0; found '" + aKeyIndex.ToString() + "'");
        }
        /// <summary>
        /// Creates a new instance of the HeaderDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HeaderDatabaseBinKey for the given key value array</returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("HeaderDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());

            }
            return new HeaderDatabaseBinKey((eDatabaseHeaderBinKeyType)DatabaseBinKeyType, aKeyValueArray[0]);
        }
    }
    #endregion Header DatabaseBinKey

    // begin TT#370 Build Packs Enhancement
    #region Header Summary DatabaseBinKey
    /// <summary>
    /// Describes a HeaderSummaryDatabaseBin Key
    /// </summary>
    [Serializable]
    public class HeaderSummaryDatabaseBinKey : DatabaseBinKey
    {
        /// <summary>
        /// Creates an instance of the HeaderSummaryDatabaseBinKey that can be used to read all summary nodes
        /// </summary>
        /// <param name="aHdrRID">RID that identifies the header</param>
        public HeaderSummaryDatabaseBinKey(int aHdrRID)
            : base(eDatabaseBinKeyType.HeaderSummaryDatabaseBinKey, aHdrRID, Include.NoRID)
        {
        }
        /// <summary>
        /// Creates an instance of the HeaderSummaryDatabaseBinKey
        /// </summary>
        /// <param name="aHdrRID">RID that identifies the header</param>
        public HeaderSummaryDatabaseBinKey(int aHdrRID, eAllocationSummaryNode aAllocationSummaryNode)
            : base(eDatabaseBinKeyType.HeaderSummaryDatabaseBinKey, aHdrRID, (int)aAllocationSummaryNode)
        {
        }
        /// <summary>
        /// Get the Header RID
        /// </summary>
        public int HdrRID
        {
            get { return GetKeyValue(0); }
        }
        public eAllocationSummaryNode AllocationSummaryNode
        {
            get { return (eAllocationSummaryNode)GetKeyValue(1); }
        }
        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">KeyIndex</param>
        /// <returns>KeyWord</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_HdrRID;
                    }
                case 1:
                    {
                        return Include.DbKeyname_AllocationSummaryNode;
                    }
            }
            throw new IndexOutOfRangeException("HeaderSummaryDatabaseBinKey index must be 0 or 1");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_HdrRID:
                    {
                        return 0;
                    }
                case Include.DbKeyname_AllocationSummaryNode:
                    {
                        return 1;
                    }
            }
            throw new IndexOutOfRangeException("HeaderSummaryDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.Int;
                    }
                case 1:
                    {
                        return eDbType.tinyint;
                    }
            }
            throw new IndexOutOfRangeException("HeaderDatabaseBinKey index must be 0 or 1; found '" + aKeyIndex.ToString() + "'");
        }
        /// <summary>
        /// Creates a new instance of the HeaderSummaryDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HeaderSummaryDatabaseBinKey for the given key value array</returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("HeaderSummaryDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());
            }
            return new HeaderSummaryDatabaseBinKey(aKeyValueArray[0], (eAllocationSummaryNode)aKeyValueArray[1]);
        }
    }
    #endregion Header Summary DatabaseBinKey

    // end TT#370 Build Packs Enhancement

    #region Header Pack DatabaseBinKey
    /// <summary>
    /// Describes a HeaderPackDatabaseBin key
    /// </summary>
    [Serializable]
    public class HeaderPackDatabaseBinKey : DatabaseBinKey
    {
        /// <summary>
        /// Creates an instance of the HeaderPackDatabaseBinKey
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aHdrPackRID">Header Pack RID</param>
        public HeaderPackDatabaseBinKey(int aHdrRID, int aHdrPackRID)
            : base(eDatabaseBinKeyType.HeaderPackDatabaseBinKey, aHdrRID, aHdrPackRID)
        {
        }
        /// <summary>
        /// Gets the Header RID for this key
        /// </summary>
        public int HdrRID
        {
            get { return GetKeyValue(0); }
        }
        /// <summary>
        /// Gets the Header Pack RID for this key
        /// </summary>
        public int HdrPackRID
        {
            get { return GetKeyValue(1); }
        }
        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">Key Index</param>
        /// <returns>SQL keyword associated with the given key index</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_HdrRID;
                    }
                case 1:
                    {
                        return Include.DbKeyname_HdrPackRID;
                    }
            }
            throw new IndexOutOfRangeException("HeaderPackDatabaseBinKey index must be 0 or 1");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_HdrRID:
                    {
                        return 0;
                    }
                case Include.DbKeyname_HdrPackRID:
                    {
                        return 1;
                    }
            }
            throw new IndexOutOfRangeException("HeaderPackDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.Int;
                    }
                case 1:
                    {
                        return eDbType.Int;
                    }
            }
            throw new IndexOutOfRangeException("HeaderPackDatabaseBinKey index must be 0 or 1; found '" + aKeyIndex.ToString() + "'");
        }
        /// <summary>
        /// Creates a new instance of the HeaderPackDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HeaderPackDatabaseBinKey for the given key value array</returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("HeaderPackDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());

            }
            return new HeaderPackDatabaseBinKey(aKeyValueArray[0], aKeyValueArray[1]);
        }
    }
    #endregion Header Pack DatabaseBinKey

    #region Header Color DatabaseBinKey
    /// <summary>
    /// Describes a HeaderColorDatabaseBin Key
    /// </summary>
    [Serializable]
    public class HeaderColorDatabaseBinKey : DatabaseBinKey
    {
        /// <summary>
        /// Creates an instance of the HeaderColorDatabaseBinKey
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aHdr_BC_RID">Header Bulk Color RID for this header</param>
        public HeaderColorDatabaseBinKey(int aHdrRID, int aHdr_BC_RID)
            : base(eDatabaseBinKeyType.HeaderColorDatabaseBinKey, aHdrRID, aHdr_BC_RID)
        {
        }
        /// <summary>
        /// Gets the Header RID 
        /// </summary>
        public int HdrRID
        {
            get { return GetKeyValue(0); }
        }
        /// <summary>
        /// Gets the Header Bulk Color RID
        /// </summary>
        public int Hdr_BC_RID
        {
            get { return GetKeyValue(1); }
        }
        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">Key Index</param>
        /// <returns>SQL keyword associated with the given key index</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_HdrRID;
                    }
                case 1:
                    {
                        return Include.DbKeyname_Hdr_BC_RID;
                    }
            }
            throw new IndexOutOfRangeException("HeaderColorDatabaseBinKey index must be 0 or 1");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_HdrRID:
                    {
                        return 0;
                    }
                case Include.DbKeyname_Hdr_BC_RID:
                    {
                        return 1;
                    }
            }
            throw new IndexOutOfRangeException("HeaderColorDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.Int;
                    }
                case 1:
                    {
                        return eDbType.Int;
                    }
            }
            throw new IndexOutOfRangeException("HeaderColorDatabaseBinKey index must be 0 or 1; found '" + aKeyIndex.ToString() + "'");
        }
        /// <summary>
        /// Creates a new instance of the HeaderColorDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HeaderColorDatabaseBinKey for the given key value array</returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("HeaderColorDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());

            }
            return new HeaderColorDatabaseBinKey(aKeyValueArray[0], aKeyValueArray[1]);
        }
    }
    #endregion Header Color DatabaseBinKey

    #region Header Color Size DatabaseBinKey
    /// <summary>
    /// Describes a HeaderColorSizeDatabaseBin Key
    /// </summary>
    [Serializable]
    public class HeaderColorSizeDatabaseBinKey : DatabaseBinKey
    {
        public HeaderColorSizeDatabaseBinKey(int aHdrRID, int aHdr_BC_RID, int aHdr_BCSZ_Key)
            : base(eDatabaseBinKeyType.HeaderColorSizeDatabaseBinKey, aHdrRID, aHdr_BC_RID, aHdr_BCSZ_Key)
        {
        }
        /// <summary>
        /// Gets the Header RID 
        /// </summary>
        public int HdrRID
        {
            get { return GetKeyValue(0); }
        }
        /// <summary>
        /// Gets the Header Bulk Color RID
        /// </summary>
        public int Hdr_BC_RID
        {
            get { return GetKeyValue(1); }
        }
        /// <summary>
        /// Gets the Header Bulk Color Size Key 
        /// </summary>
        public int Hdr_BCSZ_Key
        {
            get { return GetKeyValue(2); }
        }
        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">Key Index</param>
        /// <returns>SQL keyword associated with the given key index</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_HdrRID;
                    }
                case 1:
                    {
                        return Include.DbKeyname_Hdr_BC_RID;
                    }
                case 2:
                    {
                        return Include.DbKeyname_Hdr_BCSZ_Key;
                    }
            }
            throw new IndexOutOfRangeException("HeaderColorSizeDatabaseBinKey index must be 0, 1 or 2");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_HdrRID:
                    {
                        return 0;
                    }
                case Include.DbKeyname_Hdr_BC_RID:
                    {
                        return 1;
                    }
                case Include.DbKeyname_Hdr_BCSZ_Key:
                    {
                        return 2;
                    }
            }
            throw new IndexOutOfRangeException("HeaderColorSizeDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.Int;
                    }
                case 1:
                    {
                        return eDbType.Int;
                    }
                case 2:
                    {
                        return eDbType.Int;
                    }
            }
            throw new IndexOutOfRangeException("HeaderColorSizeDatabaseBinKey index must be 0 or 1; found '" + aKeyIndex.ToString() + "'");
        }
        /// <summary>
        /// Creates a new instance of the HeaderColorSizeDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HeaderColorSizeDatabaseBinKey for the given key value array</returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("HeaderColorSizeDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());

            }
            return new HeaderColorSizeDatabaseBinKey(aKeyValueArray[0], aKeyValueArray[1], aKeyValueArray[2]);
        }
    }
    #endregion Header Color Size DatabaseBinKey

    // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
    #region VSW Reverse OnHand DatabaseBinKey
    /// <summary>
    /// Describes a HeaderSummaryDatabaseBin Key
    /// </summary>
    [Serializable]
    public class VswReverseOnhandDatabaseBinKey : DatabaseBinKey
    {
        /// <summary>
        /// Creates an instance of the VswReverseOnhandDatabaseBinKey that can be used to read/Update VSW Reverse Onhand
        /// </summary>
        /// <param name="aHdrRID">RID that identifies the header</param>
        public VswReverseOnhandDatabaseBinKey(int aHdrRID)
            : base(eDatabaseBinKeyType.VswReverseOnhandDatabaseBinKey, aHdrRID, Include.NoRID)
        {
        }
        /// <summary>
        /// Creates an instance of the HeaderSummaryDatabaseBinKey
        /// </summary>
        /// <param name="aHdrRID">RID that identifies the header</param>
        public VswReverseOnhandDatabaseBinKey(int aHdrRID, int aHnRID)
            : base(eDatabaseBinKeyType.VswReverseOnhandDatabaseBinKey, aHdrRID, aHnRID)
        {
        }
        /// <summary>
        /// Get the Header RID
        /// </summary>
        public int HdrRID
        {
            get { return GetKeyValue(0); }
        }
        public int HnRID
        {
            get { return GetKeyValue(1); }
        }
        /// <summary>
        /// Gets the database KeyWord for the given KeyIndex
        /// </summary>
        /// <param name="aKeyIndex">KeyIndex</param>
        /// <returns>KeyWord</returns>
        public override string GetKeyWord(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return Include.DbKeyname_HdrRID;
                    }
                case 1:
                    {
                        return Include.DbKeyname_HnRID;
                    }
            }
            throw new IndexOutOfRangeException("VswReverseOnhandDatabaseBinKey index must be 0 or 1");
        }
        /// <summary>
        /// Gets the KeyWordIndex assoiated with the given KeyWord
        /// </summary>
        /// <param name="aKeyWord">KeyWord</param>
        /// <returns>KeyIndex for the given KeyWord</returns>
        public override int GetKeyWordIndex(string aKeyWord)
        {
            switch (aKeyWord)
            {
                case Include.DbKeyname_HdrRID:
                    {
                        return 0;
                    }
                case Include.DbKeyname_HnRID:
                    {
                        return 1;
                    }
            }
            throw new IndexOutOfRangeException("VswReverseOnhandDatabaseBinKey keyword = '" + aKeyWord + "' not valid");
        }
        /// <summary>
        /// Gets the DbType for the given key item index
        /// </summary>
        /// <param name="aKeyIndex">Key item index</param>
        /// <returns>DbType for the given key item in this DatabaseBinKey</returns>
        public override eDbType GetDbType(int aKeyIndex)
        {
            switch (aKeyIndex)
            {
                case 0:
                    {
                        return eDbType.Int;
                    }
                case 1:
                    {
                        return eDbType.Int;
                    }
            }
            throw new IndexOutOfRangeException("VswReverseOnhandDatabaseBinKey index must be 0 or 1; found '" + aKeyIndex.ToString() + "'");
        }
        /// <summary>
        /// Creates a new instance of the VswReverseOnhandDatabaseBinKey using the given key value array
        /// </summary>
        /// <param name="aKeyValueArray">Key value array</param>
        /// <returns>New instance of the HeaderSummaryDatabaseBinKey for the given key value array</returns>
        public override DatabaseBinKey CreateNewInstance(int[] aKeyValueArray)
        {
            if (aKeyValueArray.Length != Count)
            {
                StringBuilder sb = new StringBuilder("VswReverseOnhandDatabaseBinKey must have ");
                sb.Append(Count.ToString());
                sb.Append(" keys: ");
                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(GetKeyWord(0));
                }
                throw new IndexOutOfRangeException(sb.ToString());
            }
            return new VswReverseOnhandDatabaseBinKey(aKeyValueArray[0], aKeyValueArray[1]);
        }
    }
    #endregion VSW Reverse OnHand DatabaseBinKey
    // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
}
