//using System;
//using System.Data;
//using System.IO;
//using System.Collections;
//using System.Globalization;

//using MIDRetail.Data;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//namespace MIDRetail.Business
//{
//    // class to hold store characteristic values and the RID that goes with them
//    [Serializable()]
//    public class StoreCharValue
//    {
//        private int _SC_RID;
//        private object _value;
//        private eStoreCharType _eType;
//        private string _name;

//        public int SC_RID 
//        {
//            get{return _SC_RID;}
//            set{_SC_RID = value;}
//        }
//        public string Name
//        {
//            get{return _name;}
//            set{_name = value;}
//        }
//        public eStoreCharType StoreCharType
//        {
//            get{return _eType;}
//            set{_eType = value;}
//        }
//        public object CharValue
//        {
//            get{return _value;}
//            set{_value = value;}
//        }

//        public StoreCharValue()
//        {
//        }

//        /// <summary>
//        /// Returns a deep copy
//        /// </summary>
//        /// <returns></returns>
//        public StoreCharValue Clone()
//        {
//            StoreCharValue clone = new StoreCharValue();
//            clone._SC_RID = _SC_RID;
//            clone._value = _value;
//            clone._eType = _eType;
//            clone._name = _name;
//            return clone;
//        }

//    }

//    /// <summary>
//    ///  class to hold store characteristic groups and their vales for a particular store.
//    /// </summary>
//    [Serializable()]
//    public class StoreCharGroupProfile : Profile
//    {
//        //private int _SCG_RID;  Replaced by Key
//        private string _name;
//        private StoreCharValue _characteristicValue; 

////		public int SCG_RID   // Replaced by Key
////		{
////			get{return _SCG_RID;}
////			set{_SCG_RID = value;}
////		}
//        public string Name
//        {
//            get{return _name;}
//            set{_name = value;}
//        }
//        public StoreCharValue CharacteristicValue
//        {
//            get{return _characteristicValue;}
//            set{_characteristicValue = value;}
//        }

//        public StoreCharGroupProfile(int aKey)
//            : base(aKey)
//        {
//            _characteristicValue = new StoreCharValue();
//        }

//        public StoreCharGroupProfile(int aKey, string name, StoreCharValue charValue)
//            : base(aKey)
//        {
//            _name = name;
//            _characteristicValue = charValue;
//        }

//        /// <summary>
//        /// returns deep copy
//        /// </summary>
//        /// <returns></returns>
//        public StoreCharGroupProfile Clone()
//        {
//            StoreCharGroupProfile clone = new StoreCharGroupProfile(this.Key, _name, _characteristicValue.Clone());
//            return clone;
//        }


//        /// <summary>
//        /// Returns the eProfileType of this profile.
//        /// </summary>
//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.Store;
//            }
//        }
//    }
//    /// <summary>
//    /// StoreCharacteristic contain the methods for retrieving and maintaining
//    /// the store characteristic data
//    /// </summary>
//    //[Serializable()]
////    public class StoreCharacteristics
////    {
////        private ArrayList _charGroupList;
////        private StoreData _storeData;
////        private DataTable _dtStoreCharGroups = null;	// from STORE_CHAR_GROUP
////        private DataTable _dtStoreCharValues = null;	// from STORE_CHAR
////        private DataTable _dtStoreCharJoin = null;		// from STORE_CHAR_JOIN
////        private DataSet	  _dsCharacteristicsMaint;		// Use in Maint Window only
//////		private bool	  _updateConnectionOpen = false;
////        private StoreCharacteristicRelations _storeCharRelations;

////        /// <summary>
////        /// returns an ArrayList of CharacteristicGroup objects
////        /// </summary>
////        public ArrayList CharacteristicGroupList
////        {
////            get{return _charGroupList;}
////            //set{_charGroupList = value;}
////        }
////        /// <summary>
////        /// returns the Hashtable of CharacteristicGroup objects by Group Name
////        /// </summary>
////        public Hashtable HashByCharGroupName
////        {
////            get{return _storeCharRelations.HashByCharGroupName;}
////        }
////        /// <summary>
////        /// returns the Hashtable of CharacteristicGroup objects by Group RID
////        /// </summary>
////        public Hashtable HashByCharGroupRID
////        {
////            get{return _storeCharRelations.HashByCharGroupRID;}
////        }
////        /// <summary>
////        /// returns a HashTable of all characteristic values by characteristic RID
////        /// </summary>
////        public Hashtable HashByCharValueRID
////        {
////            get{return _storeCharRelations.HashByCharValueRID;}
////        }
////        /// <summary>
////        /// returns Store Char Relations object
////        /// </summary>
////        public StoreCharacteristicRelations StoreCharRelations
////        {
////            get{return _storeCharRelations;}
////        }

////        public StoreCharacteristics(StoreData storeData)
////        {
////            _charGroupList = new ArrayList();
////            _storeData = storeData;
			
////            BuildStoreCharacteristics();
////            PopulateCharacteristicsMaintData();
////        }

////        public void Refresh()
////        {
////            BuildStoreCharacteristics();
////            PopulateCharacteristicsMaintData();
////        }

////        public void OpenUpdateConnection()
////        {
////            _storeData.OpenUpdateConnection();
//////			_updateConnectionOpen = true;
////        }

////        public void CommitData()
////        {
////            _storeData.CommitData();
////        }

////        public void CloseUpdateConnection()
////        {
////            _storeData.CloseUpdateConnection();
//////			_updateConnectionOpen = false;
////        }


////        /// <summary>
////        /// Using the STORE_CHAR_GROUP, STORE_CHAR, and STORE_CHAR_JOIN
////        /// builds store charateristic list
////        /// </summary>
////        private void BuildStoreCharacteristics()
////        {
////            try
////            {
////                Hashtable hashByCharGroupName = new Hashtable();
////                Hashtable hashByCharGroupRID = new Hashtable();
////                Hashtable hashByCharValueRID = new Hashtable();

////                if (_dtStoreCharGroups !=null)
////                    _dtStoreCharGroups.Clear(); 
////                if (_dtStoreCharValues !=null)
////                    _dtStoreCharValues.Clear(); 
////                if (_dtStoreCharJoin !=null)
////                    _dtStoreCharJoin.Clear();
////                if (_charGroupList !=null)
////                    _charGroupList.Clear();
				
////                _dtStoreCharGroups = _storeData.StoreCharGroup_Read();
////                _dtStoreCharValues = _storeData.StoreChar_Read();
////                _dtStoreCharJoin = _storeData.StoreCharJoin_Read();

////                foreach(DataRow charGroupRow in _dtStoreCharGroups.Rows)
////                {
////                    CharacteristicGroup newCharGroup = new CharacteristicGroup();
////                    newCharGroup.RID = Convert.ToInt32(charGroupRow["SCG_RID"], CultureInfo.CurrentUICulture);
////                    newCharGroup.Id = (string)charGroupRow["SCG_ID"];
////                    newCharGroup.Name = (string)charGroupRow["SCG_ID"];
////                    newCharGroup.HasList = ((string)charGroupRow["SCG_LIST_IND"] == "1")? true: false;
////                    newCharGroup.DataType = (eStoreCharType)Convert.ToInt32(charGroupRow["SCG_TYPE"], CultureInfo.CurrentUICulture);
					
////                    // add values
////                    string filter = "SCG_RID = " + newCharGroup.RID.ToString(CultureInfo.CurrentUICulture);
////                    DataRow [] filteredRows = _dtStoreCharValues.Select(filter);
////                    foreach(DataRow dr in filteredRows)
////                    {
////                        int sc_rid = 0;
////                        sc_rid = Convert.ToInt32(dr["SC_RID"], CultureInfo.CurrentUICulture);

////                        switch(newCharGroup.DataType)
////                        {
////                            case DataCommon.eStoreCharType.text:
////                                newCharGroup.AddValue( sc_rid, dr["TEXT_VALUE"] );
////                                hashByCharValueRID.Add(sc_rid, dr["TEXT_VALUE"] );
////                                break;
////                            case DataCommon.eStoreCharType.date:
////                                newCharGroup.AddValue( sc_rid, dr["DATE_VALUE"] );
////                                hashByCharValueRID.Add(sc_rid, dr["DATE_VALUE"] );
////                                break;
////                            case DataCommon.eStoreCharType.number:
////                                newCharGroup.AddValue( sc_rid, dr["NUMBER_VALUE"] );
////                                hashByCharValueRID.Add(sc_rid, dr["NUMBER_VALUE"] );
////                                break;
////                            case DataCommon.eStoreCharType.dollar:
////                                newCharGroup.AddValue( sc_rid, dr["DOLLAR_VALUE"] );
////                                hashByCharValueRID.Add(sc_rid, dr["DOLLAR_VALUE"] );
////                                break;
////                        }
////                    }
					
////                    _charGroupList.Add(newCharGroup);
////                    hashByCharGroupRID.Add(newCharGroup.RID, newCharGroup);
////                    hashByCharGroupName.Add(newCharGroup.Name, newCharGroup);
////                }
////                _dtStoreCharGroups = null;
////                _dtStoreCharValues = null;

////                _storeCharRelations = new StoreCharacteristicRelations(_charGroupList, hashByCharGroupName, hashByCharGroupRID, hashByCharValueRID);
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

////        // Methods

////        /// <summary>
////        /// Returns a deep copy of the Characterist
////        /// </summary>
////        /// <returns></returns>
////        public ArrayList GetList()
////        {
////            ArrayList charList = new ArrayList();
////            foreach (CharacteristicGroup cg in _charGroupList)
////            {
////                charList.Add(cg.Clone());
////            }

////            return charList;
////        }

////        public DataTable GetStoreValues()
////        {
////            return _dtStoreCharJoin;
////        }

////        public int CharacteristicExists(string charGroupName, object eValue)
////        {
////            return this._storeCharRelations.CharacteristicExists(charGroupName, eValue);
////        }

////        public int CharacteristicGroupExists(string charGroupName)
////        {
////            return this._storeCharRelations.CharacteristicGroupExists(charGroupName);
////        }

////        public eStoreCharType GetCharacteristicDataType(int charGroupRID)
////        {
////            return _storeCharRelations.GetCharacteristicDataType(charGroupRID);
////        }

////        public eStoreCharType GetCharacteristicDataType(string  charGroupName)
////        {
////            return _storeCharRelations.GetCharacteristicDataType(charGroupName);
////        }

////        public int GetCharacteristicGroupRID(string charGroupName)
////        {
////            try
////            {
////                return _storeCharRelations.GetCharacteristicGroupRID(charGroupName);
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

		
////        public string GetCharacteristicGroupID(int charGroupRID)
////        {
////            try
////            {
////                return _storeCharRelations.GetCharacteristicGroupID(charGroupRID);
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

////        public object GetCharacteristicValue(int charValueRID)
////        {
////            try
////            {
////                return _storeCharRelations.GetCharacteristicValue(charValueRID);
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

////        public DataCommon.eStoreCharType DataType(string charGroupName)
////        {
////            return _storeCharRelations.DataType(charGroupName);
////        }

////        /// <summary>
////        /// only adds record to database and returns key
////        /// </summary>
////        /// <param name="charGroupName"></param>
////        /// <param name="aValue"></param>
////        /// <returns></returns>
////        public int InsertStoreChar(string charGroupName, object aValue)
////        {
////            object nullValue = null;	
////            int RID = 0;
////            CharacteristicGroup cg = (CharacteristicGroup)_storeCharRelations.HashByCharGroupName[charGroupName];

////            switch(cg.DataType)
////            {
////                case DataCommon.eStoreCharType.text:
////                    RID = _storeData.StoreChar_Insert(cg.RID, aValue, nullValue, nullValue, nullValue);
////                    break;
////                case DataCommon.eStoreCharType.date:
////                    RID = _storeData.StoreChar_Insert(cg.RID, nullValue, aValue, nullValue, nullValue);
////                    break;
////                case DataCommon.eStoreCharType.number:
////                    RID = _storeData.StoreChar_Insert(cg.RID, nullValue, nullValue, aValue, nullValue);
////                    break;
////                case DataCommon.eStoreCharType.dollar:
////                    RID = _storeData.StoreChar_Insert(cg.RID, nullValue, nullValue, nullValue, aValue);
////                    break;
////            }

////            //_storeCharRelations.AddStoreChar(RID, charGroupName, aValue);

////            return RID;
////        }

////        public void InsertStoreCharJoin(int ST_RID, int SC_RID)
////        {
////            _storeData.StoreCharJoin_Insert(ST_RID,SC_RID);
////            DataRow newRow = this._dtStoreCharJoin.NewRow();
////            newRow["ST_RID"] = ST_RID;
////            newRow["SC_RID"] = SC_RID;
////            _dtStoreCharJoin.Rows.Add(newRow);
////        }

////        public void DeleteStoreCharJoin(int ST_RID, int SC_RID)
////        {
////            _storeData.StoreCharJoin_Delete(ST_RID,SC_RID);
////            DataRow [] delRows = _dtStoreCharJoin.Select("SC_RID = " + SC_RID.ToString() + " AND ST_RID = " + ST_RID.ToString());
////            if (delRows != null)
////                if (delRows.Length > 0)
////                _dtStoreCharJoin.Rows.Remove(delRows[0]);
////        }

////        public void DeleteStoreCharJoin(int SC_RID)
////        {
////            _storeData.StoreCharJoin_Delete(SC_RID);

////            DataRow [] delRows = _dtStoreCharJoin.Select("SC_RID = " + SC_RID.ToString());
////            if (delRows != null)
////            {
////                int dCount = delRows.Length;
////                for (int i=0;i<dCount;i++)
////                {
////                    _dtStoreCharJoin.Rows.Remove(delRows[i]);
////                }
////            }
////        }

////        //************************************************
////        // Characteristic Maintenance Window Methods
////        //************************************************

////        /// <summary>
////        /// Populates a DataSet used by the Store Characteristic Maintenance Window
////        /// </summary>
////        private void PopulateCharacteristicsMaintData()
////        {
////            try
////            {
////                _dsCharacteristicsMaint = _storeData.ReadForStoreCharMaint();
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

////        internal DataSet GetCharacteristicsMaintData()
////        {
////            return _dsCharacteristicsMaint;
////        }

////        internal DataSet RefreshCharacteristicsMaintData()
////        {
////            _dsCharacteristicsMaint.Clear();
////            try
////            {
////                _dsCharacteristicsMaint = _storeData.StoreCharMaint_RefreshUsingAdapter(_dsCharacteristicsMaint);
////                // rebuild Store Characteristic object also;
////                BuildStoreCharacteristics();
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////            return _dsCharacteristicsMaint;
////        }

////        internal DataTable UpdateStoreCharGroup(DataTable xDataTable)
////        {
////            return _storeData.StoreCharGroup_UpdateUsingAdapter(xDataTable);
////        }

////        internal void UpdateStoreCharGroup(string storeCharName, int storeCharRid, bool hasList)
////        {
////            DataRow [] rows = _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].Select("SCG_RID = " + storeCharRid.ToString(CultureInfo.CurrentUICulture));
////            foreach (DataRow row in rows)
////            {
////                row["SCG_ID"] = storeCharName;
////                row["SCG_LIST_IND"] = Include.ConvertBoolToChar(hasList);
////            }
////            _dsCharacteristicsMaint.AcceptChanges();
////            _storeCharRelations.UpdateStoreCharGroup(storeCharName, storeCharRid, hasList);

		
////        }

////        // Begin TT#166 - stodd
////        internal int AddStoreCharGroupDB(string charGroupName, eStoreCharType aCharType, bool hasList)
////        {
////            try
////            {
////                int charGroupRID = _storeData.StoreCharGroup_Insert(charGroupName, aCharType, hasList);
////                return charGroupRID;
////            }
////            catch (Exception err)
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }
////        // End TT# 166


////        internal void AddStoreCharGroup(string storeCharName, int storeCharRid, eStoreCharType aCharType, bool hasList)
////        {
////            DataRow newDataRow = _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].NewRow();
////            newDataRow["SCG_RID"] = storeCharRid;
////            newDataRow["SCG_ID"] = storeCharName;
////            newDataRow["SCG_TYPE"] = (int)aCharType;
////            newDataRow["SCG_LIST_IND"] = Include.ConvertBoolToChar(hasList);
////            _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].Rows.Add(newDataRow);
////            _dsCharacteristicsMaint.AcceptChanges();

////            _storeCharRelations.AddStoreCharGroup(storeCharName, storeCharRid, aCharType, hasList);
////        }

////        internal void DeleteStoreCharGroup(string storeCharName)
////        {
////            string escapedStoreCharName = storeCharName.Replace(@"'",@"''");
////            DataRow [] rows = _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].Select("SCG_ID = '" + escapedStoreCharName + "'");
////            foreach (DataRow row in rows)
////            {
////                _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].Rows.Remove(row);
////            }
////            _dsCharacteristicsMaint.AcceptChanges();

////            _storeCharRelations.DeleteStoreCharGroup(storeCharName);
////        }

////        internal void DeleteStoreCharGroup(int storeCharRid)
////        {
////            DataRow [] rows = _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].Select("SCG_RID = " + storeCharRid.ToString(CultureInfo.CurrentUICulture));
////            foreach (DataRow row in rows)
////            {
////                _dsCharacteristicsMaint.Tables["STORE_CHAR_GROUP"].Rows.Remove(row);
////            }
////            _dsCharacteristicsMaint.AcceptChanges();

////            _storeCharRelations.DeleteStoreCharGroup(storeCharRid);
////        }

////        internal void AddStoreChar(int charGroupRid, int charValueRid, object eValue)
////        {
////            try
////            {
////                CharacteristicGroup scg =  (CharacteristicGroup)HashByCharGroupRID[charGroupRid];
////                // add to dataset
////                DataRow newDataRow = _dsCharacteristicsMaint.Tables["STORE_CHAR"].NewRow();
////                newDataRow["SC_RID"] = charValueRid;
////                newDataRow["SCG_RID"] = charGroupRid;
////                switch(scg.DataType)
////                {
////                    case DataCommon.eStoreCharType.text:
////                        newDataRow["TEXT_VALUE"] = eValue; 
////                        break;
////                    case DataCommon.eStoreCharType.date:
////                        newDataRow["DATE_VALUE"] = eValue; 
////                        break;
////                    case DataCommon.eStoreCharType.number:
////                        newDataRow["NUMBER_VALUE"] = eValue; 
////                        break;
////                    case DataCommon.eStoreCharType.dollar:
////                        newDataRow["DOLLAR_VALUE"] = eValue; 
////                        break;
////                }
////                _dsCharacteristicsMaint.Tables["STORE_CHAR"].Rows.Add(newDataRow);
////                _dsCharacteristicsMaint.AcceptChanges();
				
////                _storeCharRelations.AddStoreChar(charGroupRid, charValueRid, eValue);
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

////        internal DataTable UpdateStoreChar(DataTable xDataTable)
////        {
////            return _storeData.StoreChar_UpdateUsingAdapter(xDataTable);
////        }

////        public void UpdateStoreChar(int charValueRid, object eValue)
////        {
////            try
////            {
////                CharacteristicGroup scg = null;
////                // update dataset
////                DataRow [] rows = _dsCharacteristicsMaint.Tables["STORE_CHAR"].Select("SC_RID = " + charValueRid.ToString(CultureInfo.CurrentUICulture));
////                if (rows != null)
////                {
////                    int scgKey = Convert.ToInt32(rows[0]["SCG_RID"], CultureInfo.CurrentUICulture);
////                    scg =  (CharacteristicGroup)HashByCharGroupRID[scgKey];
////                    foreach(DataRow row in rows)
////                    {
////                        switch(scg.DataType)
////                        {
////                            case DataCommon.eStoreCharType.text:
////                                row["TEXT_VALUE"] = eValue; 
////                                break;
////                            case DataCommon.eStoreCharType.date:
////                                row["DATE_VALUE"] = eValue; 
////                                break;
////                            case DataCommon.eStoreCharType.number:
////                                row["NUMBER_VALUE"] = eValue; 
////                                break;
////                            case DataCommon.eStoreCharType.dollar:
////                                row["DOLLAR_VALUE"] = eValue; 
////                                break;
////                        }
////                    }
////                    _dsCharacteristicsMaint.AcceptChanges();
////                }

////                _storeCharRelations.UpdateStoreChar(scg.RID, charValueRid, eValue);
////            }
////            catch ( Exception err )
////            {
////                string message = err.ToString();
////                throw;
////            }
////        }

////        public void DeleteStoreCharValue(int scRid)
////        {
////            // from DB
////            _storeData.StoreChar_Delete(scRid);
////            // from DataSet
////            DataRow [] rows = _dsCharacteristicsMaint.Tables[1].Select("SC_RID = " + scRid.ToString(CultureInfo.CurrentUICulture));
////            foreach (DataRow row in rows)
////            {
////                _dsCharacteristicsMaint.Tables[1].Rows.Remove(row);
////            }
////            // from list
////            _storeCharRelations.DeleteStoreCharValue(scRid);
////        }


////        // Misc Methods

////        public void DumpToFile()
////        {
////            string rec = "";
////            FileStream fs = new FileStream("StoreCharacteristic.txt", FileMode.Create);
////            StreamWriter sw = new StreamWriter(fs);

////            foreach(CharacteristicGroup cg in _charGroupList)
////            {
////                //rec = cg.RID.ToString(CultureInfo.CurrentUICulture) + " " + cg.Name + " " + cg.DataType.ToString(CultureInfo.CurrentUICulture) + obsolete
////                rec = cg.RID.ToString(CultureInfo.CurrentUICulture) + " " + cg.Name + " " + cg.DataType.ToString() + 
////                    " " + cg.HasList.ToString(CultureInfo.CurrentUICulture) + "\n";
////                sw.Write(rec);
////                if (cg.HasList)
////                {
////                    foreach(object scv in cg.Values)
////                    {
////                        StoreCharValue  cValue = (StoreCharValue)scv;
////                        rec = "---->" + cValue.SC_RID.ToString(CultureInfo.CurrentUICulture) + " " + cValue.CharValue + "\n";
////                        sw.Write(rec);
////                    }
////                }
////            }
////            sw.Close();
////        }

////        //Begin TT#1414-MD -jsobek -Attribute Set Filter -unused function
////        //public bool IsStoreCharUsedAnywhere(int sc_rid)
////        //{
////        //    bool used = false;
////        //    DataRow [] rowsFound = _dtStoreCharJoin.Select("SC_RID = " + sc_rid.ToString(CultureInfo.CurrentUICulture));
////        //    if (rowsFound.Length > 0)
////        //        used = true;

////        //    return used;
////        //}
////        //End TT#1414-MD -jsobek -Attribute Set Filter -unused function
////    }

//    /// <summary>
//    /// Contains a store characteristic group's data, along with is't values
//    /// </summary>
//    [Serializable()]
//    //public class CharacteristicGroup
//    //{
//    //    private int _RID;
//    //    // name & id are always identical.  For Convenience only.
//    //    //private string _name;
//    //    private string _id;
//    //    private bool _hasList;
//    //    private eStoreCharType _dataType;
//    //    private ArrayList _values;

//    //    public int RID 
//    //    {
//    //        get { return _RID ; }
//    //        set { _RID = value; }
//    //    }
//    //    public string Name 
//    //    {
//    //        get { return _id ; }
//    //        set { _id = value; }
//    //    }
//    //    public string Id 
//    //    {
//    //        get { return _id ; }
//    //        set { _id = value; }
//    //    }
//    //    public bool HasList 
//    //    {
//    //        get { return _hasList ; }
//    //        set { _hasList = value; }
//    //    }
//    //    public eStoreCharType DataType 
//    //    {
//    //        get { return _dataType ; }
//    //        set { _dataType = value; }
//    //    }
//    //    public ArrayList Values 
//    //    {
//    //        get { return _values ; }
//    //        set { _values = value; }
//    //    }

//    //    public CharacteristicGroup()
//    //    {
//    //        _RID = -1;
//    //        _id = null;
//    //        _values = new ArrayList();
//    //        _hasList = false;
//    //    }
//    //    public CharacteristicGroup(string name)
//    //    {
//    //        _RID = -1;
//    //        _id = name;
//    //        _values = new ArrayList();
//    //        _hasList = false;
//    //    }
//    //    public CharacteristicGroup(int RID, string name, eStoreCharType dataType)
//    //    {
//    //        _RID = RID;
//    //        _id = name;
//    //        _dataType = dataType;
//    //        _values = new ArrayList();
//    //        _hasList = false;
//    //    }

//    //    public CharacteristicGroup(int RID, string name, eStoreCharType dataType, bool hasList)
//    //    {
//    //        _RID = RID;
//    //        _id = name;
//    //        _dataType = dataType;
//    //        _values = new ArrayList();
//    //        _hasList = hasList;
//    //    }

//    //    // Methods

//    //    public int AddValue(int valueKey, object aValue)
//    //    {
//    //        StoreCharValue newValue = new StoreCharValue();
//    //        newValue.CharValue = aValue;
//    //        newValue.SC_RID = valueKey;
//    //        return _values.Add(newValue);
//    //    }

//    //    public void UpdateValue(int valueKey, object newValue)
//    //    {

//    //        int valCnt = _values.Count;
//    //        for (int i=0;i<valCnt;i++)
//    //        {
//    //            StoreCharValue aValue = (StoreCharValue)_values[i];
//    //            if (aValue.SC_RID == valueKey)
//    //            {
//    //                aValue.CharValue = newValue;
//    //                break;
//    //            }
//    //        }
//    //    }

//    //    public int AddValue(StoreCharValue newValue)
//    //    {
//    //        return _values.Add(newValue);
//    //    }

//    //    public void RemoveValue(object aValue)
//    //    {
//    //        _values.Remove(aValue);
//    //    }

//    //    public CharacteristicGroup Clone()
//    //    {
//    //        CharacteristicGroup clone = new CharacteristicGroup(_RID,_id, this._dataType);
//    //        //if (_hasList)
//    //        //{
//    //            clone._hasList = _hasList;
//    //            foreach (StoreCharValue scv in _values)
//    //            {
//    //                StoreCharValue cloneValue = scv.Clone();
//    //                clone.AddValue(cloneValue);
//    //            }
//    //        //}
//    //        return clone;
//    //    }
//    //}

//    /// <summary>
//    /// holds an arraylist and hashtable of store characteristic groups
//    /// </summary>
//    [Serializable()]
//    public class StoreCharacteristicRelations
//    {
//        private ArrayList _charGroupList;
//        private Hashtable _hashByCharGroupName = null;
//        private Hashtable _hashByCharGroupRID = null;
//        private Hashtable _hashByCharValueRID = null;

//        #region Properties
//        public ArrayList CharacteristicGroupList 
//        {
//            get { return _charGroupList ; }
//            set { _charGroupList = value; }
//        }
//        //public Hashtable HashByCharGroupName 
//        //{
//        //    get 
//        //    { 
//        //        if (_hashByCharGroupName == null)
//        //        {
//        //            _hashByCharGroupName = new Hashtable();
//        //            int cCount = _charGroupList.Count;
//        //            for (int i=0;i<cCount;i++)
//        //            {
//        //                CharacteristicGroup cg = (CharacteristicGroup)_charGroupList[i];
//        //                _hashByCharGroupName.Add(cg.Name, cg);
//        //            }
//        //            return _hashByCharGroupName;
//        //        }
//        //        else
//        //            return _hashByCharGroupName; 
//        //    }
//        //}
//        //public Hashtable HashByCharGroupRID 
//        //{
//        //    get 
//        //    { 
//        //        if (_hashByCharGroupRID == null)
//        //        {
//        //            _hashByCharGroupRID = new Hashtable();
//        //            int cCount = _charGroupList.Count;
//        //            for (int i=0;i<cCount;i++)
//        //            {
//        //                CharacteristicGroup cg = (CharacteristicGroup)_charGroupList[i];
//        //                _hashByCharGroupRID.Add(cg.RID, cg);
//        //            }
//        //            return _hashByCharGroupRID;
//        //        }
//        //        else
//        //            return _hashByCharGroupRID; 
//        //    }
//        //}
//        public Hashtable HashByCharValueRID 
//        {
//            get 
//            { 
//                if (_hashByCharValueRID == null)
//                {
//                    _hashByCharValueRID = new Hashtable();
//                    int cCount = _charGroupList.Count;
//                    for (int i=0;i<cCount;i++)
//                    {
//                        CharacteristicGroup cg = (CharacteristicGroup)_charGroupList[i];
//                        int vCount = cg.Values.Count;
//                        for (int j=0;j<vCount;j++)
//                        {
//                            StoreCharValue scv = (StoreCharValue)cg.Values[j];
//                            if (!_hashByCharValueRID.ContainsKey(scv.SC_RID))
//                                _hashByCharValueRID.Add(scv.SC_RID, scv.CharValue);
//                        }
//                    }
//                    return _hashByCharValueRID;
//                }
//                else
//                    return _hashByCharValueRID; 
//            }
//        }
//        #endregion 

//        /// <summary>
//        /// normal constructor
//        /// </summary>
//        /// <param name="charList"></param>
//        public StoreCharacteristicRelations(ArrayList charList)
//        {
//            _charGroupList = charList;
//        }
		
//        /// <summary>
//        /// special constructor used by Store Global Area
//        /// </summary>
//        /// <param name="charList"></param>
//        public StoreCharacteristicRelations(ArrayList charList, Hashtable hashByName, Hashtable hashByRid, Hashtable hashByValueRid)
//        {
//            _charGroupList = charList;
//            _hashByCharGroupName = hashByName;
//            _hashByCharGroupRID = hashByRid;
//            _hashByCharValueRID = hashByValueRid;
//        }
//        // Methods

//        /// <summary>
//        /// Looks for a specific value within a characteristic group and returns
//        /// the value's RID.
//        /// </summary>
//        /// <param name="charGroupRid"></param>
//        /// <param name="eValue"></param>
//        /// <returns></returns>
//        //public int CharacteristicExists(int charGroupRid, object eValue)
//        //{
//        //    int RID = 0;

//        //    if (eValue != System.DBNull.Value)
//        //    {
//        //        CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupRID[charGroupRid];
//        //        if (cg == null)
//        //            throw new MIDException(eErrorLevel.severe,0,"Could not find Store Char Group with key of: " 
//        //                + charGroupRid.ToString(CultureInfo.CurrentUICulture) );
//        //        RID = CharacteristicValueSearch(cg, eValue);
//        //    }
//        //    return RID;
//        //}

//        /// <summary>
//        /// Looks for a specific value within a characteristic group and returns
//        /// the value's RID.
//        /// </summary>
//        /// <param name="charGroupName"></param>
//        /// <param name="eValue"></param>
//        /// <returns></returns>
//        public int CharacteristicExists(string charGroupName, object eValue)
//        {
//            int RID = 0;

//            if (eValue != System.DBNull.Value)
//            {
//                CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupName[charGroupName];
//                if (cg == null)
//                    throw new MIDException(eErrorLevel.severe,0,"Could not find Store Char Group called: " + charGroupName );
//                RID = CharacteristicValueSearch(cg, eValue);
//            }
//            return RID;
//        }

//        public int CharacteristicValueSearch(CharacteristicGroup cg, object eValue)
//        {
//            try
//            {
//                int RID = 0;

//                StoreCharValue scv;
//                switch (cg.DataType)
//                {
//                    case (DataCommon.eStoreCharType.text):
//                    {
//                        string stringValue = (string)eValue;
//                        for (int v=0;v<cg.Values.Count;v++)
//                        {
//                            scv = (StoreCharValue)cg.Values[v];

//                            if (scv.CharValue != null && scv.CharValue != System.DBNull.Value) //TT#1310-MD -jsobek -Error when adding a new Store
//                            { 
                           
//                                if (stringValue == (string)scv.CharValue )
//                                {
//                                    RID = scv.SC_RID;
//                                    return RID;
//                                }
//                            }
//                        }
//                        break;
//                    }
//                    case (DataCommon.eStoreCharType.date):
//                    {
//                        DateTime dateTimeValue = Convert.ToDateTime(eValue); // Issue 4959/4960
//                        for (int v=0;v<cg.Values.Count;v++)
//                        {
//                            scv = (StoreCharValue)cg.Values[v];
//                            if (scv.CharValue != null && scv.CharValue != System.DBNull.Value) //TT#1310-MD -jsobek -Error when adding a new Store
//                            {
//                                if (dateTimeValue == (DateTime)scv.CharValue)
//                                {
//                                    RID = scv.SC_RID;
//                                    return RID;
//                                }
//                            }
//                        }
//                        break;
//                    }
//                    case (DataCommon.eStoreCharType.number):
//                    {
//                        float floatValue1 = Convert.ToSingle(eValue, CultureInfo.CurrentUICulture);
//                        for (int v=0;v<cg.Values.Count;v++)
//                        {
//                            scv = (StoreCharValue)cg.Values[v];
//                            if (scv.CharValue != null && scv.CharValue != System.DBNull.Value) //TT#1310-MD -jsobek -Error when adding a new Store
//                            {
//                                if (floatValue1 == Convert.ToSingle(scv.CharValue, CultureInfo.CurrentUICulture))
//                                {
//                                    RID = scv.SC_RID;
//                                    return RID;
//                                }
//                            }
//                        }
//                        break;
//                    }
//                    case (DataCommon.eStoreCharType.dollar):
//                    {
//                        //BEGIN Issue 4959/4960
//                        //BEGIN Issue 5065
//                        float floatValue2 = 0.0f;
//                        string intermVal = eValue.ToString();
//                        int pos = intermVal.IndexOf("$");
//                        if (pos > 0)
//                        {
//                            intermVal = intermVal.Replace("$","");
//                            floatValue2 = Convert.ToSingle(intermVal, CultureInfo.CurrentUICulture);
//                        }
//                        else
//                        {
//                            floatValue2 = Convert.ToSingle(eValue, CultureInfo.CurrentUICulture);
//                        }
//                        //END Issue 5065
//                        //END Issue 4959/4960
//                        for (int v=0;v<cg.Values.Count;v++)
//                        {
//                            scv = (StoreCharValue)cg.Values[v];
//                            if (scv.CharValue != null && scv.CharValue != System.DBNull.Value) //TT#1310-MD -jsobek -Error when adding a new Store
//                            {
//                                if (floatValue2 == Convert.ToSingle(scv.CharValue, CultureInfo.CurrentUICulture))
//                                {
//                                    RID = scv.SC_RID;
//                                    return RID;
//                                }
//                            }
//                        }
//                        break;
//                    }
//                }
//                return RID;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public int CharacteristicGroupExists(string charGroupName)
//        {
//            int RID = 0;

//            foreach(CharacteristicGroup cg in _charGroupList)
//            {
//                if (cg.Name.ToUpper(CultureInfo.CurrentUICulture) == charGroupName.ToUpper(CultureInfo.CurrentUICulture))
//                {
//                    RID = cg.RID;
//                    break;
//                }
//            }

//            return RID;
//        }

//        public eStoreCharType GetCharacteristicDataType(int charGroupRID)
//        {
//            eStoreCharType dataType = eStoreCharType.unknown;
//            CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupRID[charGroupRID];
//            if (cg != null)
//                dataType = cg.DataType;
			
//            return dataType;
//        }

//        public eStoreCharType GetCharacteristicDataType(string charGroupName)
//        {
//            eStoreCharType dataType = eStoreCharType.unknown;
//            CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupName[charGroupName];
//            if (cg != null)
//                dataType = cg.DataType;
			
//            return dataType;
//        }

//        public int GetCharacteristicGroupRID(string charGroupName)
//        {
//            try
//            {
//                int cgRid = Include.NoRID;
//                CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupName[charGroupName];
//                if (cg != null)
//                    cgRid = cg.RID;
//                return cgRid;
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
		
//        public string GetCharacteristicGroupID(int charGroupRID)
//        {
//            try
//            {
//                CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupRID[charGroupRID];
//                return cg.Id;
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public string GetCharacteristicGroupIDFromValue(int charValueRID)
//        {
//            try
//            {	
//                string name = string.Empty;
//                foreach (CharacteristicGroup cg in this._charGroupList)
//                {
//                    foreach (StoreCharValue scv in cg.Values)
//                    {
//                        if (scv.SC_RID == charValueRID)
//                        {
//                            name = cg.Name;
//                            break;
//                        }
//                    }
//                }
//                return name;
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public object GetCharacteristicValue(int charValueRID)
//        {
//            try
//            {
//                return HashByCharValueRID[charValueRID];
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public DataCommon.eStoreCharType DataType(string charGroupName)
//        {
//            DataCommon.eStoreCharType dataType = DataCommon.eStoreCharType.unknown;
//            CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupName[charGroupName];

//            if (cg == null)
//                dataType = DataCommon.eStoreCharType.text;
//            else
//            {
//                switch(cg.DataType)
//                {
//                    case DataCommon.eStoreCharType.text:
//                        dataType = DataCommon.eStoreCharType.text;
//                        break;
//                    case DataCommon.eStoreCharType.date:
//                        dataType = DataCommon.eStoreCharType.date;
//                        break;
//                    case DataCommon.eStoreCharType.number:
//                        dataType = DataCommon.eStoreCharType.number;
//                        break;
//                    case DataCommon.eStoreCharType.dollar:
//                        dataType = DataCommon.eStoreCharType.dollar;
//                        break;
//                }
//            }

//            return dataType;
//        }


//        // Begin TT# 166 - stodd
//        public CharacteristicGroup AddStoreCharGroup(string charGroupName, int charGroupRID, eStoreCharType aCharType, bool hasList)
//        {
//            CharacteristicGroup cg = new CharacteristicGroup(charGroupRID, charGroupName, aCharType, hasList);
//            this._charGroupList.Add(cg);
//            if (!HashByCharGroupName.ContainsKey(cg.Name))
//                this.HashByCharGroupName.Add(cg.Name, cg);
//            if (!HashByCharGroupRID.ContainsKey(cg.RID))
//                this.HashByCharGroupRID.Add(cg.RID, cg);
//            return cg;
//        }
//        // Ebd TT# 166 - stodd

//        public void UpdateStoreCharGroup(string charGroupName, int charGroupRID, bool hasList)
//        {
//            CharacteristicGroup cg = (CharacteristicGroup)HashByCharGroupRID[charGroupRID];
//            // BEGIN MID issue #### Stodd 09/07/2005
//            //==========================
//            // Group name has changed
//            //==========================
//            if (cg.Name != charGroupName)
//            {
//                if (HashByCharGroupName.ContainsKey(cg.Name))
//                {
//                    HashByCharGroupName.Remove(cg.Name);
//                }
//                cg.Name = charGroupName;
//                cg.HasList = hasList;
//                HashByCharGroupName.Add(cg.Name, cg);
//            }
//            else
//            {
//                cg.Name = charGroupName;
//                cg.HasList = hasList;
//            }
//            // END MID issue ####
//        }

//        /// <summary>
//        /// deletes the Store Characteristic group from the list and
//        /// from all hash tables;
//        /// </summary>
//        /// <param name="scg_rid"></param>
//        public void DeleteStoreCharGroup(int scg_rid)
//        {
//            CharacteristicGroup cg = (CharacteristicGroup)HashByCharGroupRID[scg_rid];
//            if (cg != null)
//            {
//                foreach (StoreCharValue scv in cg.Values)
//                {
//                    // remove from value hash
//                    if (HashByCharValueRID.ContainsKey(scv.SC_RID))
//                        HashByCharValueRID.Remove(scv.SC_RID);
//                }

//                this._charGroupList.Remove(cg);
//                this.HashByCharGroupName.Remove(cg.Name);
//                this.HashByCharGroupRID.Remove(cg.RID);
//            }
//        }

//        /// <summary>
//        /// deletes the Store Characteristic group from the list and
//        /// from all hash tables;
//        /// </summary>
//        /// <param name="scgName"></param>
//        public void DeleteStoreCharGroup(string scgName)
//        {
//            CharacteristicGroup cg = (CharacteristicGroup)HashByCharGroupName[scgName];
//            if (cg != null)
//            {
//                DeleteStoreCharGroup(cg.RID);
//            }
//        }
		
//        /// <summary>
//        /// deletes a store characteristic value from the value hash and the store char group list
//        /// </summary>
//        /// <param name="sc_rid"></param>
//        public void DeleteStoreCharValue(int sc_rid)
//        {
//            // remove from value hash
//            if (HashByCharValueRID.ContainsKey(sc_rid))
//                HashByCharValueRID.Remove(sc_rid);

//            // remove from char group list
//            int count = _charGroupList.Count;
//            int valueIdx = 0;
//            for (int i=0;i<count;i++)
//            {
//                CharacteristicGroup cg = (CharacteristicGroup)_charGroupList[i];
//                int vCount = cg.Values.Count;
//                for (int j=0;j<vCount;j++)
//                {
//                    StoreCharValue scv = (StoreCharValue)cg.Values[j];
//                    if (scv.SC_RID == sc_rid)
//                        valueIdx = j;
//                }

//                if (valueIdx > 0)
//                {
//                    cg.Values.RemoveAt(valueIdx);
//                    break;
//                }
//            }
//        }

//        /// <summary>
//        /// adds store char value to Store Char Relations AND DB
//        /// </summary>
//        /// <param name="charGroupName"></param>
//        /// <param name="aValue"></param>
//        /// <returns></returns>
//        public void AddStoreChar(int RID, string charGroupName, object aValue)
//        {
//            CharacteristicGroup cg = (CharacteristicGroup)HashByCharGroupName[charGroupName];

//            AddStoreChar(cg.RID, RID, aValue);
//        }

//        /// <summary>
//        /// adds store char value to Store Char Relations
//        /// </summary>
//        /// <param name="charGroupRid"></param>
//        /// <param name="charValueRid"></param>
//        /// <param name="aValue"></param>
//        /// <returns></returns>
//        public void AddStoreChar(int charGroupRid, int charValueRid, object aValue)
//        {
//            CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupRID[charGroupRid];

//            cg.AddValue( charValueRid, aValue );
//            if (!HashByCharValueRID.ContainsKey(charValueRid))
//                HashByCharValueRID.Add(charValueRid, aValue );
//        }

//        public void UpdateStoreChar(int charGroupRid, int charValueRid, object aValue)
//        {
//            CharacteristicGroup cg = (CharacteristicGroup)this.HashByCharGroupRID[charGroupRid];
//            cg.UpdateValue( charValueRid, aValue );

//            if (HashByCharValueRID.ContainsKey(charValueRid))
//            {
//                object oldValue = HashByCharValueRID[charValueRid];
//                oldValue = aValue;
//            }

//        }
		
//    }
//}
