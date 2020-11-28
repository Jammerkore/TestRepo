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
//    /// A container of StoreVariableVectors indexed by variable ID
//    /// </summary>
//    [Serializable]
//    public class StoreVariableVectorContainer
//    {
//        #region Fields
//        private byte _version;
//        private short _hiStrRID;
//        private short _cntVarIDX;
//        private short _hiVarIDX;
//        private StoreVariableVector[] _variableArray;
//        [NonSerialized]
//        private byte[] _baseArray;
//        [NonSerialized]
//        private MemoryStream _memStream;
//        [NonSerialized]
//        private BinaryReader _binReader;
//        #endregion Fields

//        #region Constructors
//        public StoreVariableVectorContainer()
//        {
//            _hiStrRID = 0;
//            _cntVarIDX = 0;
//            _hiVarIDX = 0;
//            _variableArray = null;
//            _baseArray = null;
//        }

//        /// <summary>
//        /// Creates an instance of this structure
//        /// </summary>
//        /// <param name="aStoreVariableVectorContainer">A null or existing StoreVariableVectorContainer to use to initialize this instance</param>
//        public StoreVariableVectorContainer(StoreVariableVectorContainer aStoreVariableVectorContainer)
//        {
//            if (aStoreVariableVectorContainer == null
//                || ((StoreVariableVectorContainer)aStoreVariableVectorContainer)._variableArray == null)
//            {
//                _hiStrRID = 0;
//                _cntVarIDX = 0;
//                _hiVarIDX = 0;
//                _variableArray = null;
//                _baseArray = null;
//            }
//            else
//            {
//                StoreVariableVectorContainer svvc = ((StoreVariableVectorContainer)aStoreVariableVectorContainer);
//                _variableArray = new StoreVariableVector[svvc._variableArray.Length];
//                svvc._variableArray.CopyTo(_variableArray, 0);
//                _hiStrRID = svvc._hiStrRID;
//                _cntVarIDX = svvc._cntVarIDX;
//                _hiVarIDX = svvc._hiVarIDX;
//                if (svvc._baseArray != null)
//                {
//                    _baseArray = svvc._baseArray;
//                    _memStream = new MemoryStream(_baseArray);
//                    _binReader = new BinaryReader(_memStream);
//                }
				
//            }
//        }
//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        /// <param name="aByteArray">A byte array used to initialize this instance</param>
//        public StoreVariableVectorContainer(byte[] aByteArray)
//        {
//            _baseArray = aByteArray;
//            _memStream = new MemoryStream(aByteArray);
//            _binReader = new BinaryReader(_memStream);

//            _version = _binReader.ReadByte();
//            _hiVarIDX = _binReader.ReadInt16();
//            _cntVarIDX = _binReader.ReadInt16();
//            _hiStrRID = _binReader.ReadInt16();
//            _variableArray = new StoreVariableVector[_hiVarIDX+1];

//            byte _typ;
//            short _len;
//            short _var;
//            int _pos;
//            _pos = (int)_binReader.BaseStream.Position;
//            for (int i = 0; i < _cntVarIDX; i++)
//            {
//                _binReader.BaseStream.Position = _pos;
//                _len = _binReader.ReadInt16();
//                _typ = _binReader.ReadByte();
//                _var = _binReader.ReadInt16();
//                _binReader.BaseStream.Position = _pos;
//                ExpandVector(this, _var);
//                _pos += _len;
//            }
//        }


//        private void ExpandVector(StoreVariableVectorContainer CN, short _var)
//        {
//            CN._variableArray[_var]._baseOffset = (int)CN._binReader.BaseStream.Position;
//            CN._variableArray[_var]._baseLength = CN._binReader.ReadInt16();
//            CN._variableArray[_var]._storageTypeCode = CN._binReader.ReadByte();
//            CN._variableArray[_var]._status = 1;
//            CN._variableArray[_var]._variableIDX = CN._binReader.ReadInt16();
//            CN._variableArray[_var]._hiStrRID = CN._binReader.ReadInt16();

//            switch (CN._variableArray[_var].StorageTypeCode)
//            {
//                case eMIDStorageTypeCode.typeByte:
//                    ExpandVectorByte(CN, _var);
//                    break;
//                case eMIDStorageTypeCode.typeSByte:
//                    ExpandVectorSbyte(CN, _var);
//                    break;
//                case eMIDStorageTypeCode.typeShort:
//                    ExpandVectorShort(CN, _var);
//                    break;
//                case eMIDStorageTypeCode.typeUshort:
//                    ExpandVectorUshort(CN, _var);
//                    break;
//                case eMIDStorageTypeCode.typeInt:
//                    ExpandVectorInt(CN, _var);
//                    break;
//                case eMIDStorageTypeCode.typeUint:
//                    ExpandVectorUint(CN, _var);
//                    break;
//                case eMIDStorageTypeCode.typeLong:
//                    ExpandVectorLong(CN, _var);
//                    break;
//            }
//        }

//        private void ExpandVectorByte(StoreVariableVectorContainer CN, short _var)
//        {
//            byte[] _strArray = new byte[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadByte();
//                }
//            }
//        }

//        private void ExpandVectorSbyte(StoreVariableVectorContainer CN, short _var)
//        {
//            sbyte[] _strArray = new sbyte[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadSByte();
//                }
//            }
//        }

//        private void ExpandVectorUshort(StoreVariableVectorContainer CN, short _var)
//        {
//            ushort[] _strArray = new ushort[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadUInt16();
//                }
//            }
//        }

//        private void ExpandVectorShort(StoreVariableVectorContainer CN, short _var)
//        {
//            short[] _strArray = new short[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadInt16();
//                }
//            }
//        }

//        private void ExpandVectorUint(StoreVariableVectorContainer CN, short _var)
//        {
//            uint[] _strArray = new uint[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadUInt32();
//                }
//            }
//        }

//        private void ExpandVectorInt(StoreVariableVectorContainer CN, short _var)
//        {
//            int[] _strArray = new int[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadInt32();
//                }
//            }
//        }

//        private void ExpandVectorLong(StoreVariableVectorContainer CN, short _var)
//        {
//            long[] _strArray = new long[CN._variableArray[_var]._hiStrRID];
//            CN._variableArray[_var]._storeArray = _strArray;
//            int _end = CN._variableArray[_var]._baseOffset + CN._variableArray[_var]._baseLength;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _str = 0;
//            while (CN._binReader.BaseStream.Position < _end)
//            {
//                _ncnt = CN._binReader.ReadByte();
//                if ((_ncnt & (int)0x80) != 0)
//                {
//                    _ncnt &= ~(int)0x80;
//                    _nncnt = _ncnt & (int)0x0f;
//                    _ncnt >>= 4;
//                }
//                else
//                {
//                    _nncnt = CN._binReader.ReadByte();
//                }
//                _str += _ncnt;
//                for (int i = 0; i < _nncnt; i++, _str++)
//                {
//                    _strArray[_str] = CN._binReader.ReadInt64();
//                }
//            }
//        }
//        #endregion Constructors

//        #region Properties
//        /// <summary>
//        /// Count of the StoreNumericVariableVectors in the array (alternatively, count of the number of sizes in array
//        /// </summary>
//        internal int Count
//        {
//            get
//            {
//                if (_variableArray == null)
//                {
//                    return 0;
//                }
//                return _variableArray.Length;
//            }
//        }
//        #endregion Properties

//        #region Methods

//        #region GetStoreVariableValue
//        /// <summary>
//        /// Gets the 64-bit integer variable value for the given store RID and variable ID
//        /// </summary>
//        /// <param name="aStoreRID">RID that identifies the store</param>
//        /// <param name="aVariableIDX">Code identifying the variable within the structure model type</param>
//        /// <returns>Variable's unit value for the given color size index and store RID</returns>
//        public Int64 GetStoreVariableValue(int aStoreRID, short aVariableIDX)
//        {
//            if (_variableArray != null
//                && aVariableIDX < _variableArray.Length
//                && _variableArray[aVariableIDX].StorageTypeCode != eMIDStorageTypeCode.Empty)
//            {
//                if (aStoreRID > (_variableArray[aVariableIDX]).LargestStoreRID)
//                {
//                    return 0;
//                }
//                if ((_variableArray[aVariableIDX]).IsCompressed)
//                {
//                    //Currently all vectors will be expanded
//                    //ExpandVector(this, aVariableIDX);
//                }
//                return _variableArray[aVariableIDX].GetStoreVariableValue(aStoreRID);
//            }
//            return 0;
//        }
//        #endregion GetStoreVariableValue

//        #region SetStoreVariableValue
//        /// <summary>
//        /// Sets the variable value for a given store RID and variable code
//        /// </summary>
//        /// <param name="aStoreRID">RID identifying the store</param>
//        /// <param name="aVariableIDX">Variable Index identifying the variable to be updated</param>
//        /// <param name="aSVVC">The StoreVariableVectorContainer where the StoreVariableVector resides at aVariableIDX</param>
//        /// <param name="aVariableValue">Variable's value for the given store RID and color-size RID</param>
//        public void SetStoreVariableValue(int aStoreRID, eMIDVariableModelType aVariableModelType, ref StoreVariableVectorContainer aSVVC, short aVariableIDX, long aVariableValue)
//        {
//            if (aSVVC._variableArray == null)
//            {
//                aSVVC.CreateVectorContainer(aVariableModelType, aVariableIDX);
//            }
//            if (aVariableIDX >= aSVVC._variableArray.Length)
//            {
//                if (aVariableValue == 0)
//                {
//                    return;
//                }
//                aSVVC.ExtendVectorContainer(aVariableModelType, aVariableIDX);
//            }
//            if (aSVVC._variableArray[aVariableIDX].IsCompressed)
//            {
//                //Currently all vectors will be expanded
//                //ExpandVector(aSVVC, aVariableIDX);
//            }

//            if (aSVVC._variableArray[aVariableIDX]._storeArray == null)
//            {
//                eMIDStorageTypeCode storageTypeCode = MIDStorageTypeInfo.FindStorageTypeCode(aVariableValue, aVariableValue);
//                aSVVC._variableArray[aVariableIDX]._variableIDX = aVariableIDX;
//                aSVVC._variableArray[aVariableIDX].StorageTypeCode = storageTypeCode;
//                aSVVC._variableArray[aVariableIDX]._storeArray = Array.CreateInstance(MIDStorageTypeInfo.GetStorageType(storageTypeCode), MIDStorageTypeInfo.GetStoreMaxRID(aStoreRID));
//                aSVVC._variableArray[aVariableIDX].StoreCount = (short)(aSVVC._variableArray[aVariableIDX])._storeArray.Length;
//                aSVVC._variableArray[aVariableIDX]._status = 2;
//                if (aSVVC._hiStrRID < aSVVC._variableArray[aVariableIDX].StoreCount)
//                {
//                    aSVVC._hiStrRID = aSVVC._variableArray[aVariableIDX].StoreCount;
//                }
//            }
//            else if (aVariableValue > MIDStorageTypeInfo.GetStorageMaximumValue(((aSVVC._variableArray[aVariableIDX])).StorageTypeCode))
//            {
//                eMIDStorageTypeCode newTypeCode;
//                if (MIDStorageTypeInfo.GetStorageMinimumValue(aSVVC._variableArray[aVariableIDX].StorageTypeCode) == 0)
//                {
//                    newTypeCode = MIDStorageTypeInfo.FindStorageTypeCode(0, aVariableValue);
//                }
//                else
//                {
//                    newTypeCode = MIDStorageTypeInfo.FindStorageTypeCode(aVariableValue, aSVVC._variableArray[aVariableIDX]._storeArray);
//                }
//                ExtendVector(ref aSVVC, aVariableIDX, newTypeCode, Math.Max(aStoreRID, aSVVC._variableArray[aVariableIDX]._storeArray.Length));
//            }
//            else if (aVariableValue < MIDStorageTypeInfo.GetStorageMinimumValue(aSVVC._variableArray[aVariableIDX].StorageTypeCode))
//            {
//                eMIDStorageTypeCode newTypeCode = MIDStorageTypeInfo.FindStorageTypeCode(aVariableValue, aSVVC._variableArray[aVariableIDX]._storeArray);
//                ExtendVector(ref aSVVC, aVariableIDX, newTypeCode, Math.Max(aStoreRID, aSVVC._variableArray[aVariableIDX]._storeArray.Length));
//            }
//            else if (aStoreRID > aSVVC._variableArray[aVariableIDX]._storeArray.Length)
//            {
//                ExtendVector(ref aSVVC, aVariableIDX,aSVVC._variableArray[aVariableIDX].StorageTypeCode, aStoreRID);
//            }
//            aSVVC._variableArray[aVariableIDX]._storeArray.SetValue(Convert.ChangeType(aVariableValue, MIDStorageTypeInfo.GetStorageType(aSVVC._variableArray[aVariableIDX].StorageTypeCode)), aStoreRID - 1);
        
        
//        }
//        #endregion SetStoreVariableValue

//        #region ExtendVector
//        /// <summary>
//        /// Extends the size of the store vector to include at least the specified, required Minimum Store RID as well as re-type the array if necessary
//        /// </summary>
//        /// <param name="aTypeCode">Storage Type Code to use to resize the vector</param>
//        /// <param name="aRequiredMinStoreRID">RID of the desired minimum store RID (expansion will not shrink the current array if this minimum is smaller than the largest existing Store RID in the array</param>
//        private void ExtendVector(ref StoreVariableVectorContainer aSVVC, short aVariableIDX, eMIDStorageTypeCode aTypeCode, int aRequiredMaxStoreRID)
//        {
//            RebuildVector(ref aSVVC, aVariableIDX, aTypeCode, MIDStorageTypeInfo.GetStoreMaxRID(Math.Max(aSVVC._variableArray[aVariableIDX].LargestStoreRID, aRequiredMaxStoreRID)));
//        }
//        #endregion ExtendVector

//        #region TrimVector
//        /// <summary>
//        /// Trims the size of the store vector to the best fitting storage type code and largest necessary store RID
//        /// </summary>
//        internal void TrimVector(StoreVariableVectorContainer aSVVC, short aVariableIDX)
//        {
//            if (aSVVC._variableArray[aVariableIDX].StorageTypeCode != eMIDStorageTypeCode.Empty)
//            {
//                eMIDStorageTypeCode typeCode = aSVVC._variableArray[aVariableIDX].StorageTypeCode;
//                if (aSVVC._variableArray[aVariableIDX]._storeArray != null)
//                {
//                    typeCode = MIDStorageTypeInfo.FindStorageTypeCode(0, aSVVC._variableArray[aVariableIDX]._storeArray);
//                }
//                int trimStoreRID = 0;
//                for (int i = ((aSVVC._variableArray[aVariableIDX])).LargestStoreRID; i > 0; i--)
//                {
//                    if (aSVVC._variableArray[aVariableIDX].GetStoreVariableValue(i) != 0)
//                    {
//                        trimStoreRID = i;
//                        break;
//                    }
//                }
//                if ((byte)typeCode != (byte)aSVVC._variableArray[aVariableIDX].StorageTypeCode
//                    || trimStoreRID != aSVVC._variableArray[aVariableIDX].LargestStoreRID)
//                {
//                    RebuildVector(ref aSVVC, aVariableIDX, typeCode, trimStoreRID);
//                }
//            }
//        }
//        #endregion TrimVector

//        #region RebuildVector
//        /// <summary>
//        /// Rebuilds the vector to the specified type code and MaxStoreRID
//        /// </summary>
//        /// <param name="aTypeCode">Storage type code to which the current vector is to be converted</param>
//        /// <param name="aMaxStoreRID">Maximum Store RID that the converted vector should contain</param>
//        private void RebuildVector(ref StoreVariableVectorContainer aSVVC, short aVariableIDX, eMIDStorageTypeCode aTypeCode, int aMaxStoreRID)
//        {
//            if (aMaxStoreRID > 0)
//            {
//                Array storeVariableVector = Array.CreateInstance(MIDStorageTypeInfo.GetStorageType(aTypeCode), aMaxStoreRID);
//                int commonLength = Math.Min(storeVariableVector.Length, aSVVC._variableArray[aVariableIDX]._storeArray.Length);
//                if (aSVVC._variableArray[aVariableIDX].StorageTypeCode == aTypeCode)
//                {
//                    for (int i = 0; i < commonLength; i++)
//                    {
//                        storeVariableVector.SetValue(aSVVC._variableArray[aVariableIDX]._storeArray.GetValue(i), i);
//                    }
//                }
//                else
//                {
//                    Type convertToType = MIDStorageTypeInfo.GetStorageType(aTypeCode);
//                    for (int i = 0; i < commonLength; i++)
//                    {
//                        object objectValue = Convert.ChangeType(aSVVC._variableArray[aVariableIDX]._storeArray.GetValue(i), convertToType);
//                        storeVariableVector.SetValue(objectValue, i);
//                    }
//                }
//                aSVVC._variableArray[aVariableIDX].StorageTypeCode = aTypeCode;
//                aSVVC._variableArray[aVariableIDX]._storeArray = storeVariableVector;
//                aSVVC._variableArray[aVariableIDX].StoreCount = (short)aSVVC._variableArray[aVariableIDX]._storeArray.Length;
//            }
//            else
//            {
//                aSVVC._variableArray[aVariableIDX].StorageTypeCode = (byte)eMIDStorageTypeCode.Empty;
//                aSVVC._variableArray[aVariableIDX].StoreCount = 0;
//                aSVVC._variableArray[aVariableIDX]._storeArray = null;
//            }
//            if (aSVVC._variableArray[aVariableIDX]._hiStrRID < aSVVC._hiStrRID)
//            {
//                aSVVC._hiStrRID = aSVVC._variableArray[aVariableIDX]._hiStrRID;
//            }
//            aSVVC._hiVarIDX = (short)(aSVVC._variableArray.Length - 1);
//        }
//        #endregion RebuildVector

//        #region Create
//        private void CreateVectorContainer(eMIDVariableModelType aVariableModelType, int aVariableIDX)
//        {
//            _variableArray = new StoreVariableVector[MIDStorageTypeInfo.GetVariableMaxIDX(aVariableModelType, aVariableIDX) + 1];
//        }
//        #endregion Create

//        #region Rebuild
//        private void ExtendVectorContainer(eMIDVariableModelType aVariableModelType, short aVariableIDX)
//        {
//            RebuildVectorArray(MIDStorageTypeInfo.GetVariableMaxIDX(aVariableModelType, aVariableIDX));
//        }
//        /// <summary>
//        /// Rebuilds the vector array to the specified size
//        /// </summary>
//        /// <param name="aVariableIXD">The desired size of the array</param>
//        private void RebuildVectorArray(int aLargestVariableIDX)
//        {
//            StoreVariableVector[] workVectorContainer = new StoreVariableVector[aLargestVariableIDX + 1];
//            int commonLength = Math.Min(workVectorContainer.Length, _variableArray.Length);
//            for (int i = 0; i < commonLength; i++)
//            {
//                workVectorContainer[i] = _variableArray[i];
//            }

//            _variableArray = workVectorContainer;
//        }
//        #endregion Rebuild

//        #region Trim
//        /// <summary>
//        /// Trims the store variable vector container to only those vectors with store data
//        /// </summary>
//        internal void TrimVectorContainer()
//        {
//            if (_variableArray != null)
//            {
//                int trimIndex = _variableArray.Length;
//                bool continueLooking = true;
//                while (continueLooking
//                       && trimIndex > 0)
//                {
//                    trimIndex--;
//                    if (_variableArray[trimIndex].StorageTypeCode != eMIDStorageTypeCode.Empty)
//                    {
//                        if ((_variableArray[trimIndex]).IsCompressed)
//                        {
//                            // Assume that a Serialized vector HAS store Data!
//                            continueLooking = false;
//                            break;
//                        }
//                        TrimVector(this, (short)trimIndex);
//                        if ((_variableArray[trimIndex]).StoreCount > 0)
//                        {
//                            continueLooking = false;
//                            break;
//                        }
//                    }
//                }
//                RebuildVectorArray(trimIndex);
//            }
//        }
//        #endregion Trim

//        #region Compression
//        #region Remove this code when Hugh's code is activated
//        ///// <summary>
//        ///// Creates an Expanded instance of the StoreVariableVector for the given variable
//        ///// </summary>
//        ///// <param name="aVariableIDX">The index of the Variable for which the StoreVariableVector is to be deserialized</param> 
//        ///// <param name="aSVVC">The StoreVariableVectorContainer where the StoreVariableVector resides at position aVariableIDX</param>
//        //private void ExpandVector(StoreVariableVectorContainer aSVVC, UInt16 aVariableIDX)
//        //{
//        //    // uses the startByteArray and endByteArray within the specified StoreVariableVector to expand the store variable vector values from the byte array
//        //}
//        ///// <summary>
//        ///// Compresses the Vector Container 
//        ///// </summary>
//        //public byte[] CompressVectorContainer()
//        //{
//        //    byte[] binaryObject = null;
//        //    using (MemoryStream ms = new MemoryStream())
//        //    {
//        //        BinaryFormatter bf = new BinaryFormatter();
//        //        bf.Serialize(ms, this._variableArray);
//        //        ms.Flush();
//        //        binaryObject = ms.ToArray();
//        //    }
//        //    return binaryObject;
//        //}
//        ///// <summary>
//        ///// Expand a compressed store vector container
//        ///// </summary>
//        ///// <param name="aSerializedStoreVectorArray"></param>
//        //public void ExpandVectorContainer(byte[] aCompressedStoreVectorContainer)
//        //{
//        //    using (MemoryStream ms = new MemoryStream(aCompressedStoreVectorContainer))
//        //    {
//        //        BinaryFormatter bf = new BinaryFormatter();
//        //        //_storeVariableVectorContainer = (Nullable<StoreVariableVector>[])bf.Deserialize(ms);
//        //        _variableArray = (StoreVariableVector[])bf.Deserialize(ms);
//        //    }
//        //}
//        #endregion Remove this code when Hugh's code is activated

//        #region Hugh's ToArray Code (remove this comment when Hugh's code is activated
//        public byte[] ToArray()             //hjf
//        {
//            StoreVariableVectorContainer CN = this; //hjf
//            int _mslen;
//            if (CN._memStream == null)
//            {
//                _mslen = 4096;
//            }
//            else 
//            {
//                _mslen = (int)CN._memStream.Length;
//            }
//            MemoryStream MS = new MemoryStream(_mslen);
//            BinaryWriter BW = new BinaryWriter(MS);

//            int _hiVar = 0;
//            int _cntVar = 0;

//            for (int i = 0; i < CN._variableArray.Length; i++)
//            {
//                if (CN._variableArray != null && CN._variableArray[i]._status != 0)
//                {
//                    _cntVar++;
//                    _hiVar = CN._variableArray[i]._variableIDX = (short)i; 
//                }
//            }

//            BW.Write((byte)CN._version);
//            BW.Write((short)_hiVar);
//            BW.Write((short)_cntVar);
//            BW.Write((short)CN._hiStrRID);

//            for (int i = 0; i <= _hiVar; i++)
//            {
//                if (CN._variableArray != null && CN._variableArray[i]._status != 0)
//                {
//                    int _pos = (int)MS.Position;
//                    int _nxt;
//                    short _len = 0;
//                    BW.Write((short)_len);
//                    BW.Write((byte)CN._variableArray[i]._storageTypeCode);
//                    BW.Write((short)CN._variableArray[i]._variableIDX);
//                    BW.Write((short)CN._variableArray[i]._hiStrRID);

//                    switch (CN._variableArray[i].StorageTypeCode)
//                    {
//                        case eMIDStorageTypeCode.typeByte:
//                            WriteStrArrayByte(CN, BW, i);
//                            break;
//                        case eMIDStorageTypeCode.typeSByte:
//                            WriteStrArraySbyte(CN, BW, i);
//                            break;
//                        case eMIDStorageTypeCode.typeShort:
//                            WriteStrArrayShort(CN, BW, i);
//                            break;
//                        case eMIDStorageTypeCode.typeUshort:
//                            WriteStrArrayUshort(CN, BW, i);
//                            break;
//                        case eMIDStorageTypeCode.typeInt:
//                            WriteStrArrayInt(CN, BW, i);
//                            break;
//                        case eMIDStorageTypeCode.typeUint:
//                            WriteStrArrayUint(CN, BW, i);
//                            break;
//                        case eMIDStorageTypeCode.typeLong:
//                            WriteStrArrayLong(CN, BW, i);
//                            break;
//                    }
//                    _nxt = (int)BW.BaseStream.Position;
//                    _len = (short)(_nxt - _pos);
//                    BW.BaseStream.Position = _pos;
//                    BW.Write((short)_len);
//                    BW.BaseStream.Position = _nxt;
//                }
//            }
//            BW.Close();
//            return MS.ToArray();
//        }

//        private void WriteStrArrayByte(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            byte[] _strArray = CN._variableArray[i]._storeArray as byte[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {                    
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }

//        private void WriteStrArraySbyte(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            sbyte[] _strArray = CN._variableArray[i]._storeArray as sbyte[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }

//        private void WriteStrArrayShort(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            short[] _strArray = CN._variableArray[i]._storeArray as short[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }

//        private void WriteStrArrayUshort(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            ushort[] _strArray = CN._variableArray[i]._storeArray as ushort[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }

//        private void WriteStrArrayInt(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            int[] _strArray = CN._variableArray[i]._storeArray as int[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }

//        private void WriteStrArrayUint(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            uint[] _strArray = CN._variableArray[i]._storeArray as uint[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }

//        private void WriteStrArrayLong(StoreVariableVectorContainer CN, BinaryWriter BW, int i)
//        {
//            long[] _strArray = CN._variableArray[i]._storeArray as long[];
//            int _str = 0;
//            int _ncnt = 0;
//            int _nncnt = 0;
//            int _hiStr = _strArray.Length;

//            while (_str < _hiStr)
//            {
//                int _holdstr = _str;
//                for (_ncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] == 0));
//                        _str++, _ncnt++)
//                { continue; }

//                for (_nncnt = 0;
//                        ((_str < _hiStr) && (_strArray[_str] != 0));
//                        _str++, _nncnt++)
//                { continue; }

//                _str = _holdstr;

//                while (_ncnt > 127)
//                {
//                    BW.Write((byte)127);
//                    BW.Write((byte)0);
//                    _ncnt -= 127;
//                    _str += 127;
//                }

//                while (_nncnt > 255)
//                {
//                    BW.Write((byte)_ncnt);
//                    BW.Write((byte)255);
//                    _str += _ncnt;
//                    _ncnt = 0;
//                    _nncnt -= 255;
//                    for (int j = 0; j < 255; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//                _str += _ncnt;
//                if (_nncnt > 0)
//                {
//                    if ((_ncnt <= 7) && (_nncnt <= 15))
//                    {
//                        _ncnt <<= 4;
//                        _ncnt = _ncnt | _nncnt | (int)0x80;
//                        BW.Write((byte)_ncnt);
//                    }
//                    else
//                    {
//                        BW.Write((byte)_ncnt);
//                        BW.Write((byte)_nncnt);

//                    }
//                    for (int j = 0; j < _nncnt; j++, _str++)
//                    {
//                        BW.Write(_strArray[_str]);
//                    }
//                }
//            }
//            return;
//        }
//        #endregion Hugh's code --REmove this comment when Hugh's code is activated

//        #endregion Compression

//        #region Operator +
//        /// <summary>
//        /// Combines 2 non-null StoreVariableVector Arrays.
//        /// </summary>
//        /// <param name="A">1st non-null StoreVariableVectorContainer</param>
//        /// <param name="B">2nd non-null StoreVariableVectorContainer</param>
//        /// <returns></returns>
//        public static StoreVariableVectorContainer operator +(StoreVariableVectorContainer A, StoreVariableVectorContainer B)
//        {
//            StoreVariableVectorContainer C;
//            StoreVariableVectorContainer D;
//            if (A._variableArray.Length >
//                B._variableArray.Length)
//            {
//                C = new StoreVariableVectorContainer(A);
//                D = B;
//            }
//            else
//            {
//                C = new StoreVariableVectorContainer(B);
//                D = A;
//            }
//            for (int i = 0; i < D._variableArray.Length; i++)
//            {
//                C._variableArray[i] += D._variableArray[i];
//            }
//            return C;
//        }
//        #endregion Operator +
        
//        #endregion Methods 
//    }
//}