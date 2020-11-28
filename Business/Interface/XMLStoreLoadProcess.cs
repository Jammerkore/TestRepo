//using System;
//using System.IO;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Collections;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Diagnostics;
//using System.Globalization;
//using System.Data;

//using MIDRetail.Data;
//using MIDRetail.DataCommon;
//using MIDRetail.Common;


//namespace MIDRetail.Business
//{
//    /// <summary>
//    /// This class is called by StoreServerSession to load stores.
//    /// </summary>
//    public class XMLStoreLoadProcess
//    {
//        private string _sourceModule = "XMLStoreLoadProcess.cs";
//        private Stores _stores = null;
//        private SessionAddressBlock _SAB;
//        private int _commitLimit;
//        private string _exceptionFileName;
//        StoreProfile _storeProfile = null;

//        private EditMsgs _editMsgs;
//        private int _storeRecs;
//        private int _recordsWithErrors;
//        // Begin MID Track #4668 - add number added and modified
//        private int _recordsAdded;
//        private int _recordsUpdated;
//        // End MID Track #4668
//        private int _recordsDeleted;		// TT#739-MD - STodd - delete stores
//        private int _recordsRecovered;		// TT#739-MD - STodd - delete stores
//        // Begin TT# 166 - stodd
//        private string _appSetting;
//        private bool _characteristicAutoAdd = false;
//        private string _CharacteristicDelimiter = "\\";
//        // End TT# 166
//        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//        private bool _useCharacteristicTransaction = false;
//        private string _currentStoreId = string.Empty;
//        // END TT#1401 - stodd - add resevation stores (IMO)
        
//        /// <summary>
//        /// Instantiates XMLStoreLoadProccess.
//        /// </summary>
//        /// <param name="SAB">A prepared SessionAddressBlock.</param>
//        /// <param name="commitLimit">The maximum number of records to commit.</param>
//        /// <param name="exceptionFileName">The name of the file to log exceptions to.</param>
//        // Begin TT#166 - JSmith - Store Characteristics auto add
//        //public XMLStoreLoadProcess(SessionAddressBlock SAB, int commitLimit, string exceptionFileName)
//        public XMLStoreLoadProcess(SessionAddressBlock SAB, int commitLimit, string exceptionFileName)
//        // End TT#166
//        {
//            _SAB = SAB;
//            _commitLimit = commitLimit;
//            _exceptionFileName = exceptionFileName;
//            //_storeChar = _SAB.StoreServerSession.GetStoreCharacteristicList();
//            //_storeMaint = new StoreMaintenance(_SAB); //TT#1517-MD -jsobek -Store Service Optimization
//            StoreValidation.SetSAB(SAB); //TT#1517-MD -jsobek -Store Service Optimization
//            _editMsgs = new EditMsgs();
//        }

//        /// <summary>
//        /// Rather than implementing two seperate load functions, we parse the delimited file into 
//        /// the stores deserialization class and use that base function.
//        /// </summary>
//        /// <param name="delimitedFileName">The name of the file to load and parse.</param>
//        /// <param name="delimiter">The delimiter use to seperate fields in the file.</param>
//        /// <returns>A Stores instance that may then be passed to ProcessStores</returns>
//        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//        // Begin TT#166 - JSmith - Store Characteristics auto add
//        //public bool StoresFromDelimitedFile(string delimitedFileName, char[] delimiter)
//        public bool StoresFromDelimitedFile(string delimitedFileName, char[] delimiter,
//            bool autoAddCharacteristics, char[] characteristicDelimiter, bool useCharacteristicTransaction)
//        // End TT#166
//        // END TT#1401 - stodd - add resevation stores (IMO)
//        {
//            int dynamicCharacteristsCount = 0;
//            bool errorFound = false;
//            // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//            string storeId = string.Empty;
//            _useCharacteristicTransaction = useCharacteristicTransaction;
//            // END TT#1401 - stodd - add resevation stores (IMO)
//            if(!File.Exists(delimitedFileName))	// Make sure our file exists...
//            {
//                throw new XMLStoreLoadProcessException(String.Format("Store Service can not find the file located at '{0}'", delimitedFileName));
//            }

//            // Begin TT#166 - JSmith - Store Characteristics auto add
//            _characteristicAutoAdd = autoAddCharacteristics;
//            // End TT#166
			
//            StreamReader sr = new StreamReader(delimitedFileName);
//            string[] items;
			
//            string line = sr.ReadLine();
//            while(line != null)
//            {
//                items = MIDstringTools.Split(line,delimiter[0],true);
//                // Begin TT# 166 - stodd
//                // skip blank line
//                if ((items.Length == 1 && (items[0] == null || items[0] == ""))   
//                    || items.Length == 0)
//                {
//                    // BEGIN TT#493 - stodd - looping
//                    line = sr.ReadLine();
//                    // END TT#493
//                    continue;
//                }
//                // End TT# 166 - stodd

//                // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//                errorFound = ValidateItems(items, delimiter[0]);
//                if (errorFound)
//                {
//                    line = sr.ReadLine();
//                    continue;
//                    //HandleErrorMessages();
//                }
//                // END TT#1401 - stodd - add resevation stores (IMO)

//                // Begin TT# 166 - stodd
//                if (items[0].ToUpper() == "OPTIONS")
//                {
//                    errorFound = ProcessOptions(line, items);
//                    if (errorFound)
//                    {
//                        HandleErrorMessages();
//                        break;
//                    }
//                }
//                // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//                else if (_useCharacteristicTransaction)
//                {
//                    if (items[0].ToUpper() == "S")
//                    {
//                        ++_storeRecs;							
//                        _storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);
//                        //_storeProfile.Characteristics = new ArrayList();
//                        bool success = AddStore(items[1], items[2], items[3], items[4], items[5], items[6],
//                                            items[7], items[8], items[9], items[10], items[11], items[12], items[13], items[14]);

//                        UpdateStore(null);
//                        _currentStoreId = items[1];
//                    }
//                    else if (items[0].ToUpper() == "C")
//                    {
//                        ++_storeRecs;
//                        _storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);

//                        // Check to be sure store exists
//                        if (_storeProfile.Key == Include.UndefinedStoreRID)
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + items[1];
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CharacteristicAddedPriorToStore, msgDetails, _sourceModule);
//                            line = sr.ReadLine();
//                            continue;
//                        }

//                        //_storeProfile.Characteristics = new ArrayList();
//                        dynamicCharacteristsCount = (items.Length - 2) / 2;
//                        List<storeCharInfo> charList = null;
//                        if (((items.Length - 2) % 2) == 0)	// make sure characteristics are paired
//                        {
//                            int c = 2;
//                            for (int i = 0; i < dynamicCharacteristsCount; i++)
//                            {
//                                eStoreCharType newStoreCharType = eStoreCharType.unknown;
//                                // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                                //if (ParseStoreCharName(ref items[c], ref newStoreCharType))
//                                //{
//                                //    if (items[c] != null) AddStoreCharacteristics(items[c], items[c + 1], newStoreCharType);
//                                //    c += 2;	// characteristics are in pairs
//                                //}
//                                string newStoreCharName;
//                                if (ParseStoreCharName(ref items[c], ref newStoreCharType, out newStoreCharName))
//                                {
//                                    if (items[c] != null)
//                                    {
//                                        charList = new List<storeCharInfo>();
//                                        storeCharInfo charInfo = new storeCharInfo();
//                                        AddStoreCharacteristics(newStoreCharName, items[c + 1], newStoreCharType, ref charInfo);
//                                        charList.Add(charInfo);
//                                    }
//                                    c += 2;	// characteristics are in pairs
//                                }
//                                // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                            }
//                            UpdateStore(charList);
//                            _currentStoreId = items[1];
//                        }
//                        else
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + _storeProfile.StoreId;
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharNotPaired, msgDetails, _sourceModule);
//                        }
//                    }
//                    // BEGIN TT#739-MD - STodd - delete stores
//                    else if (items[0].ToUpper() == "D")		// Mark Store For Deletion
//                    {
//                        ++_storeRecs;
//                        _storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);

//                        // Check to be sure store exists
//                        if (_storeProfile.Key == Include.UndefinedStoreRID)
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + items[1];
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, msgDetails, _sourceModule);
//                            line = sr.ReadLine();
//                            continue;
//                        }

//                        if (_storeProfile.ActiveInd)
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + items[1];
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotInactiveForDelete, msgDetails, _sourceModule);
//                            line = sr.ReadLine();
//                            continue;
//                        }

//                        // BEGIN TT#739-MD - STodd - delete stores
//                        if (_storeProfile.Key == _SAB.StoreServerSession.GlobalOptions.ReserveStoreRID)
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + items[1];
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CannotDeleteReserveStore, msgDetails, _sourceModule);
//                            line = sr.ReadLine();
//                            continue;
//                        }
//                        // END TT#739-MD - STodd - delete stores

//                        // BEGIN TT#739-MD - STodd - delete stores
//                        UpdateStore(StoresStoreAction.Delete, null);
//                        _currentStoreId = items[1];
//                        // END TT#739-MD - STodd - delete stores

//                    }
//                    else if (items[0].ToUpper() == "R")		// Revoer store that had been marked for deletion
//                    {
//                        ++_storeRecs;
//                        _storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);

//                        // Check to be sure store exists
//                        if (_storeProfile.Key == Include.UndefinedStoreRID)
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + items[1];
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, msgDetails, _sourceModule);
//                            line = sr.ReadLine();
//                            continue;
//                        }

//                        // BEGIN TT#739-MD - STodd - delete stores
//                        UpdateStore(StoresStoreAction.Recover, null);
//                        _currentStoreId = items[1];
//                        // END TT#739-MD - STodd - delete stores

//                    }
//                    // END TT#739-MD - STodd - delete stores
//                    else
//                    {
//                        errorFound = true;
//                        string msgDetails = "Store: " + _storeProfile.StoreId;
//                        _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_TransactionTypeInvalid, msgDetails, _sourceModule);
//                    }
//                    // BEGIN TT#1401 - stodd - errors not showing uyp in audit
//                    if (errorFound)
//                    {
//                        HandleErrorMessages();
//                    }
//                    // END TT#1401 - stodd - errors not showing uyp in audit
//                }
//                // END TT#1401 - stodd - add resevation stores (IMO)
//                else
//                // End TT# 166 - stodd
//                // END Issue 4667
//                {
//                    // Begin Issue 4301 (1/30/2007)
//                    //_editMsgs.ClearMsgs();
//                    // End Issue 4301
//                    ++_storeRecs;
//                    _storeProfile = StoreMgmt.StoreProfile_Get(items[0]); //_SAB.StoreServerSession.GetStoreProfile(items[0]);
//                    //_storeProfile.Characteristics = new ArrayList();

//                    //BEGIN TT#1401 - stodd - add VSWID
//                    if (AddStore(items[0], items[1], items[2], items[3], items[4], items[5], items[6],
//                        items[7], items[8], items[9], items[10], items[11], items[12], null))
//                    {
//                        List<storeCharInfo> charList = null;
//                        if (((items.Length - 13) % 2) == 0)	// make sure characteristics are paired
//                        {
//                            int c = 13;
//                            dynamicCharacteristsCount = (items.Length - 13) / 2;
//                            //END TT#1401 - stodd - add VSWID
//                            charList = new List<storeCharInfo>();
//                            for (int i = 0; i < dynamicCharacteristsCount; i++)
//                            {
//                                // Begin TT# 166 - stodd
//                                eStoreCharType newStoreCharType = eStoreCharType.unknown;
//                                // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                                //if (ParseStoreCharName(ref items[c], ref newStoreCharType))
//                                //{
//                                //    // BEGIN TT#1246 - gtaylor
//                                //    if (items[c] != null) AddStoreCharacteristics(items[c], items[c + 1], newStoreCharType);
//                                //    // END
//                                //    c += 2;	// characteristics are in pairs
//                                //}
//                                string newStoreCharName;
//                                if (ParseStoreCharName(ref items[c], ref newStoreCharType, out newStoreCharName))
//                                {
//                                    // BEGIN TT#1246 - gtaylor
//                                    if (items[c] != null)
//                                    {
//                                        storeCharInfo charInfo = new storeCharInfo();
//                                        AddStoreCharacteristics(newStoreCharName, items[c + 1], newStoreCharType, ref charInfo);
//                                        charList.Add(charInfo);
//                                    }
//                                    // END
//                                    c += 2;	// characteristics are in pairs
//                                }
//                                // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                                // End TT# 166 - stodd
//                            }
//                        }
//                        else
//                        {
//                            errorFound = true;
//                            string msgDetails = "Store: " + _storeProfile.StoreId;
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharNotPaired, msgDetails, _sourceModule);
//                        }
//                        // BEGIN TT#1401 - stodd - errors not showing uyp in audit
//                        if (errorFound)
//                        {
//                            HandleErrorMessages();
//                        }
//                        // END TT#1401 - stodd - errors not showing uyp in audit
//                        UpdateStore(charList);
//                    }
//                    else
//                    {
//                        HandleErrorMessages();
//                    }
//                }
//                line = sr.ReadLine();
//            }

//            sr.Close();	


//            _SAB.StoreServerSession.Audit.StoreLoadAuditInfo_Add(_storeRecs, _recordsWithErrors, _recordsAdded, _recordsUpdated, _recordsDeleted, _recordsRecovered);	// TT#739-MD - STodd - delete stores


//            // BEGIN Issue 4000 - stodd 5/16/2007
//            RefreshStoreGroups(ref _editMsgs);
//            // END Issue 4000

//            return errorFound;
//        }

//        /// <summary>
//        /// Since new stores have been added it's a good idea to refresh the store groups now.
//        /// This is so the overhead that might be encountered to create new Attr Sets within
//        /// dynamic groups is done now and not during online access. 
//        /// </summary>
//        /// <param name="em"></param>
//        private void RefreshStoreGroups(ref EditMsgs em)
//        {
//            //_SAB.StoreServerSession.RefreshStoresInAllGroups();
//            StoreMgmt.ProgressBarOptions pOpt = new StoreMgmt.ProgressBarOptions();
//            pOpt.useProgressBar = false;
//            StoreMgmt.StoreGroups_Refresh(pOpt);
//        }


//        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//        private bool ValidateItems(string[] items, char delimiter)
//        {
//            bool errorFound = false;
//            try
//            {
//                // There should be a minimum of 15 items due to the requirements of the STORES table.
//                // If there is only 1 item then odds are the delimiter is mismatched between the 
//                // config file and the input file.
//                if ((items[0].ToUpper() == "S" && items.Length < 15) || (items[0].ToUpper() != "S" && items.Length < 13))		// TT#1317-MD - stodd - Store Load errors when transaction file is delimited
//                {
//                    // Begin TT# 166 - stodd
//                    if (items[0].ToUpper() == "OPTIONS")
//                    {
//                        // this is a valid record.
//                    }
//                    // End TT# 166 - stodd
//                    else if (_useCharacteristicTransaction && items[0].ToUpper() == "C")
//                    {
//                        // this is a valid record. Characteriscs may have less items.
//                    }
//                    // BEGIN TT#739-MD - STodd - delete stores
//                    else if (_useCharacteristicTransaction && items.Length == 2 && (items[0].ToUpper() == "D" || items[0].ToUpper() == "R"))
//                    {
//                        // this is a valid record. Delete transactions only need the store id.
//                    }
//                    // END TT#739-MD - STodd - delete stores
//                    else
//                    {
//                        ++_storeRecs;
//                        // Begin TT#1317-MD - stodd - Store Load errors when transaction file is delimited
                        // Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
                        //string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_MismatchedDelimiter, false);
                        // End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
//                        if (items[0].ToUpper() == "S")
//                        {
//                            msgDetails = msgDetails.Replace("{0}", items[1]);
//                        }
//                        else
//                        {
//                            msgDetails = msgDetails.Replace("{0}", items[0]);
//                        }
//                        msgDetails = msgDetails.Replace("{1}", delimiter.ToString());
//                        //string msgDetails = "Delimiter defined as " + delimiter.ToString(CultureInfo.CurrentUICulture) + " in CONFIG file.";
                        // Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
                        //_editMsgs.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, msgDetails, _sourceModule);
						// End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
//                        errorFound = true;
//                        // End TT#1317-MD - stodd - Store Load errors when transaction file is delimited
//                        HandleErrorMessages();
//                    }
//                }
//                return errorFound;
//            }
//            catch
//            {
//                throw;
//            }

//        }
//        // END TT#1401 - stodd - add resevation stores (IMO)

//        // Begin TT# 166 - stodd
//        private bool ProcessOptions(string line, string[] fields)
//        {
//            bool errorFound = false;

//            // Characteristic Auto Add
//            if (fields.Length > 1 && fields[1] != null && fields[1].Length > 0)
//            {
//                try
//                {
//                    _characteristicAutoAdd = Convert.ToBoolean(fields[1]);
//                }
//                catch
//                {
//                    errorFound = true;
//                    string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharAutoAddValue, false);
//                    msgDetails = msgDetails.Replace("{0}", fields[1].ToString(CultureInfo.CurrentUICulture));
//                    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharAutoAddValue, msgDetails, _sourceModule);
//                }
//            }

//            // Characteristic Type Delimiter
//            if (fields.Length > 2 && fields[2] != null && fields[2].Length > 0)
//            {
//                try
//                {
//                    _CharacteristicDelimiter = (Convert.ToString(fields[2])).Trim();
//                    if ((_CharacteristicDelimiter.Trim()).Length != 1)
//                    {
//                        errorFound = true;
//                        string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharTypeDelimiter, false);
//                        msgDetails = msgDetails.Replace("{0}",_CharacteristicDelimiter);
//                        _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharTypeDelimiter, msgDetails, _sourceModule);

//                    }
//                }
//                catch
//                {
//                    errorFound = true;
//                    string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharTypeDelimiter, false);
//                    msgDetails = msgDetails.Replace("{0}", fields[2].ToString(CultureInfo.CurrentUICulture));
//                    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharTypeDelimiter, msgDetails, _sourceModule);
//                }
//            }

//            // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//            // Use Transaction Types
//            if (fields.Length > 3 && fields[3] != null && fields[3].Length > 0)
//            {
//                try
//                {
//                    _useCharacteristicTransaction = bool.Parse((Convert.ToString(fields[3])).Trim());
//                }
//                catch
//                {
//                    errorFound = true;
//                    string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidUseCharacteristicTransaction, false);
//                    msgDetails = msgDetails.Replace("{0}", fields[3].ToString(CultureInfo.CurrentUICulture));
//                    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidUseCharacteristicTransaction, msgDetails, _sourceModule);
//                }
//            }
//            // END TT#1401 - stodd - add resevation stores (IMO)

//            return errorFound;
//        }
//        // End TT# 166 - stodd

//        /// <summary>
//        /// Loads and deserializes an XML file and adds Stores
//        /// </summary>
//        /// <param name="xmlLoadFileName">The name of the file to load and deserialize</param>
//        /// <returns>True on success, false on failure</returns>
//        // Begin TT#166 - JSmith - Store Characteristics auto add
//        //public bool ProcessStores(string xmlLoadFileName)
//        //{
//        //    return _ProcessStores(xmlLoadFileName, null);
//        //}
//        public bool ProcessStores(string xmlLoadFileName, bool autoAddCharacteristics)
//        {
//            return _ProcessStores(xmlLoadFileName, null, autoAddCharacteristics);
//        }
//        // End TT#166
////		/// <summary>
////		/// Creates stores from an already deserialized file.
////		/// </summary>
////		/// <param name="inStores">The deserialized Stores class</param>
////		/// <returns>True on success, false on failure</returns>
////		public bool ProcessStores(Stores inStores)
////		{
////			return _ProcessStores(null, inStores);
////		}
//        /// <summary>
//        /// Internal function that performs actual adding of stores off of a deserialized stores class.
//        /// </summary>
//        /// <param name="xmlLoadFileName">Name of Xml file to load and deserialize. Ignored if inStores is not null.</param>
//        /// <param name="inStores">A deserialized Stores class used to load stores from.</param>
//        /// <returns></returns>
//        // Begin TT#166 - JSmith - Store Characteristics auto add
//        //private bool _ProcessStores(string xmlLoadFileName, Stores inStores)
//        private bool _ProcessStores(string xmlLoadFileName, Stores inStores, bool autoAddCharacteristics)
//        // End TT#166
//        {
//            EditMsgs em = new EditMsgs();

//            if(inStores == null && _stores == null) // If Stores is already deserialize, do not deserialize again.
//            {
//                if(!File.Exists(xmlLoadFileName))	// Make sure our file exists before attempting to deserialize
//                {
//                    throw new XMLStoreLoadProcessException(String.Format("Store Service can not find the file located at '{0}'", xmlLoadFileName));
//                }
//                // Begin Track #4229 - JSmith - API locks .XML input file
//                TextReader r = null;
//                // End Track #4229
//                try 
//                {
//                    /* Follow me here, I created MIDRetail.StoreLoad.StoreLoadSchema.xsd to define and validate
//                        what a StoreLoad XML file should look like. From the Visual Studio command prompt I
//                        run xsd /c StoreLoadSchema.xsd to generate a class file that is a strongly typed
//                        represenation of that schema. The end result is I don't have to parse a loaded XML
//                        document node by node which can result in errors if the Xml document is not formed
//                        perfectly prior to reciept by this function.
//                    */
//                    XmlSerializer s = new XmlSerializer(typeof(Stores));	// Create a Serializer
//                    // Begin Track #4229 - JSmith - API locks .XML input file
//                    //TextReader r = new StreamReader(xmlLoadFileName);		// Load the Xml File
//                    r = new StreamReader(xmlLoadFileName);		// Load the Xml File
//                    // End Track #4229
//                    _stores = (Stores)s.Deserialize(r);						// Deserialize the Xml File to a strongly typed object
//                    // Begin Track #4229 - JSmith - API locks .XML input file
//                    //r.Close();												// Close the input file.
//                    // End Track #4229
//                }
//                catch(Exception ex)
//                {
//                    throw new XMLStoreLoadProcessException(String.Format("Error encountered during deserialization of the file '{0}'", xmlLoadFileName), ex);
//                }
//                // Begin Track #4229 - JSmith - API locks .XML input file
//                finally
//                {
//                    if (r != null)
//                        r.Close();
//                }
//                // End Track #4229
//            } 
//            else
//            {
//                _stores = inStores;
//            }
//            try
//            {
//                bool errorFound = false;	// TT#739-MD - STodd - delete stores
//                // Begin TT# 166 - stodd auto add
//                _characteristicAutoAdd = autoAddCharacteristics;
//                if (_stores.Options != null)
//                {
//                    if (_stores.Options.AutoAddCharacteristicsSpecified)
//                    {
//                        _characteristicAutoAdd = _stores.Options.AutoAddCharacteristics;
//                    }
//                }
//                // End TT# 166 - stodd auto add
//                foreach (StoresStore s in _stores.Store)
//                {
//                    ++_storeRecs;
//                    _editMsgs.ClearMsgs();
//                    _storeProfile = StoreMgmt.StoreProfile_Get(s.ID); // _SAB.StoreServerSession.GetStoreProfile(s.ID);
//                    //_storeProfile.Characteristics = new ArrayList();
//                    string sSellingSquareFeet = null, sSellingOpenDate = null, sSellingCloseDate = null, sStockOpenDate = null,
//                        sStockCloseDate = null, sLeadTime = null, sStoreActive = null;

//                    // BEGIN TT#739-MD - STodd - delete stores
//                    if (s.Action == StoresStoreAction.Delete)
//                    {
//                        if (_storeProfile.Key != Include.UndefinedStoreRID)
//                        {

//                            if (_storeProfile.ActiveInd == false)
//                            {
//                                // BEGIN TT#739-MD - STodd - delete stores
//                                if (_SAB.StoreServerSession.GlobalOptions.ReserveStoreRID == _storeProfile.Key)
//                                {
//                                    errorFound = true;
//                                    string msgDetails = "Store: " + _storeProfile.StoreId;
//                                    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CannotDeleteReserveStore, msgDetails, _sourceModule);
//                                    HandleErrorMessages();
//                                }
//                                // END TT#739-MD - STodd - delete stores

//                                UpdateStore(StoresStoreAction.Delete, null);

//                            }
//                            else
//                            {
//                                // A delete transaction was found, but the store was still active
//                                errorFound = true;
//                                string msgDetails = "Store: " + _storeProfile.StoreId;
//                                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotInactiveForDelete, msgDetails, _sourceModule);
//                                HandleErrorMessages();
//                            }
//                        }
//                    }
//                    else if (s.Action == StoresStoreAction.Recover)
//                    {
//                        if (_storeProfile.Key != Include.UndefinedStoreRID)
//                        {
//                            UpdateStore(StoresStoreAction.Recover, null);
//                        }
//                    }
//                    else
//                    // END TT#739-MD - STodd - delete stores
//                    {
//                        // Begin Issue 4374 stodd
//                        if (s.ActiveIndicatorSpecified)
//                            sStoreActive = Convert.ToString(s.ActiveIndicator, CultureInfo.CurrentUICulture);
//                        else
//                            sStoreActive = null;
//                        // End Issue 4374 stodd
//                        if (s.SellingSquareFeetSpecified)
//                            sSellingSquareFeet = Convert.ToString(s.SellingSquareFeet, CultureInfo.CurrentUICulture);
//                        else
//                            sSellingSquareFeet = null;
//                        if (s.SellingOpenDateSpecified)
//                            sSellingOpenDate = Convert.ToString(s.SellingOpenDate, CultureInfo.CurrentUICulture);
//                        else
//                            sSellingOpenDate = null;
//                        if (s.SellingCloseDateSpecified)
//                            sSellingCloseDate = Convert.ToString(s.SellingCloseDate, CultureInfo.CurrentUICulture);
//                        else
//                            sSellingCloseDate = null;
//                        if (s.StockOpenDateSpecified)
//                            sStockOpenDate = Convert.ToString(s.StockOpenDate, CultureInfo.CurrentUICulture);
//                        else
//                            sStockOpenDate = null;
//                        if (s.StockCloseDateSpecified)
//                            sStockCloseDate = Convert.ToString(s.StockCloseDate, CultureInfo.CurrentUICulture);
//                        else
//                            sStockCloseDate = null;
//                        if (s.LeadTimeSpecified)
//                            sLeadTime = Convert.ToString(s.LeadTime, CultureInfo.CurrentUICulture);
//                        else
//                            sLeadTime = null;

//                        // convert values to string for common editing
//                        if (AddStore(s.ID, s.Name, s.Description, s.City, s.State, sSellingSquareFeet,
//                            sSellingOpenDate, sSellingCloseDate,
//                            sStockOpenDate, sStockCloseDate,
//                            sStoreActive, sLeadTime, // Issue 4374 stodd
//                            Convert.ToString(s.ShipDate, CultureInfo.CurrentUICulture), s.VSWID))	// TT#1401 - stodd - add VSWID
//                        {
//                            List<storeCharInfo> charList = new List<storeCharInfo>();
//                            if (s.Characteristic != null)
//                            {
//                                foreach (StoresStoreCharacteristic c in s.Characteristic) // Now loop through and add each Characteristic
//                                {
//                                    storeCharInfo charInfo = new storeCharInfo();
//                                    AddStoreCharacteristics(c.Name, c.Value, c.CharType.ToString(), ref charInfo);
//                                    charList.Add(charInfo);
//                                }
//                            }
//                            UpdateStore(charList);
//                        }
//                        else
//                        {
//                            HandleErrorMessages();
//                        }
//                    }
//                }

//                RefreshStoreGroups(ref _editMsgs);

//            }
//            catch (Exception ex)
//            {
//                throw new XMLStoreLoadProcessException(String.Format("Error encountered while processing the file '{0}'", xmlLoadFileName), ex);
//            }
//            // Begin MID Track #4668 - add number added and modified
//            _SAB.StoreServerSession.Audit.StoreLoadAuditInfo_Add(_storeRecs, _recordsWithErrors, _recordsAdded, _recordsUpdated, _recordsDeleted, _recordsRecovered);
//            // End MID Track #4668
//            // false = no error found.
//            return false;
//        }

//        public bool AddStore(
//            string StoreID, 
//            string StoreName, 
//            string StoreDescription, 
//            string City, 
//            string State, 
//            string SellingSqFt, 
//            string SellingOpenDate, 
//            string SellingCloseDate, 
//            string StockOpenDate, 
//            string StockCloseDate, 
//            string ActiveIndicator, 
//            string LeadTime, 
//            string ShipDate, 
//            string ImoId)	
//        {
//            bool updateSuccessful = true;

//            //delay profile modification until after validation
//            bool profileActiveInd = false;
//            string profileCity = null;
//            string profileState = null;
//            int? profileLeadTime = null;
//            int? profileSellingSqFt = null;
//            DateTime? profileSellingCloseDt = null;
//            DateTime? profileSellingOpenDt = null;
//            DateTime? profileStockCloseDt = null;
//            DateTime? profileStockOpenDt = null;
//            bool? profileShipOnFriday = null;
//            bool? profileShipOnMonday = null;
//            bool? profileShipOnSaturday = null;
//            bool? profileShipOnSunday = null;
//            bool? profileShipOnThursday = null;
//            bool? profileShipOnTuesday = null;
//            bool? profileShipOnWednesday = null;      
//            string profileStoreName = null;
//            string profileStoreId = null;
//            string profileStoreDescription = null;
//            string profileIMO_ID = null;

//            try
//            {
//                if (ActiveIndicator == null)
//                {
//                    if (_storeProfile.Key == Include.UndefinedStoreRID)
//                    {
//                        //_storeProfile.SetActiveInd(true);
//                        profileActiveInd = true;
//                    }
//                }
//                else
//                {
//                    bool tempActiveInd = Convert.ToBoolean(ActiveIndicator, CultureInfo.CurrentUICulture);
//                    profileActiveInd = tempActiveInd;
//                    if (tempActiveInd && _storeProfile.DeleteStore)
//                    {
//                        // BEGIN TT#739-MD - STodd - delete stores
//                        updateSuccessful = false;
//                        string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_StoreNotInactiveForDelete, false);
//                        msgDetails = msgDetails.Replace("{0}", StoreID);
//                        _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotInactiveForDelete, msgDetails, _sourceModule);
//                        // END TT#739-MD - STodd - delete stores
//                    }

//                    // BEGIN TT#739-MD - STodd - delete stores
//                    if (!tempActiveInd && StoreMgmt.ReserveStoreRID == _storeProfile.Key)
//                    {
//                        updateSuccessful = false;
//                        string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_CannotMarkReserveStoreInactive, false);
//                        _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CannotMarkReserveStoreInactive, msgDetails, _sourceModule);
//                    }
//                    // END TT#739-MD - STodd - delete stores
//                }
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Active Indicator");
//                msgDetails = msgDetails.Replace("{2}",ActiveIndicator);
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);
//            }

//            if (City != null)
//            {
//                profileCity = City.Trim();
//            }
//            if (State != null)
//            {
//                profileState = State.Trim();
//            }

//            try
//            {
//                // Begin MID Issue 3408 stodd
//                // Need to init lead time to zero
//                if (LeadTime != null && LeadTime != string.Empty)  // TT#3651 - JSmith - Store Load Failure
//                {
//                    //_storeProfile.LeadTime = Convert.ToInt32(LeadTime, CultureInfo.CurrentUICulture);
//                    profileLeadTime = Convert.ToInt32(LeadTime, CultureInfo.CurrentUICulture);
//                }
//                // Begin TT#2020 - JSmith - Lead Time in Store Profiles
//                //else
//                //    _storeProfile.LeadTime = 0;
//                // End TT#2020
//                // End MID Issue 3408 stodd
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Lead Time");
//                msgDetails = msgDetails.Replace("{2}",LeadTime.ToString(CultureInfo.CurrentUICulture));
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);
//            }

//            try
//            {
//                if (SellingCloseDate != null && SellingCloseDate != string.Empty)  // TT#3651 - JSmith - Store Load Failure
//                {
//                    //_storeProfile.SellingCloseDt = Convert.ToDateTime(SellingCloseDate, CultureInfo.CurrentUICulture);
//                    profileSellingCloseDt = Convert.ToDateTime(SellingCloseDate, CultureInfo.CurrentUICulture);
//                }
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Selling Close Date");
//                msgDetails = msgDetails.Replace("{2}",SellingCloseDate.ToString(CultureInfo.CurrentUICulture));
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);

//            }

//            try
//            {
//                if (SellingOpenDate != null && SellingOpenDate != string.Empty)  // TT#3651 - JSmith - Store Load Failure
//                {
//                    //_storeProfile.SellingOpenDt = Convert.ToDateTime(SellingOpenDate, CultureInfo.CurrentUICulture);
//                    profileSellingOpenDt = Convert.ToDateTime(SellingOpenDate, CultureInfo.CurrentUICulture);
//                }
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Selling Open Date");
//                msgDetails = msgDetails.Replace("{2}",SellingOpenDate.ToString(CultureInfo.CurrentUICulture));
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);
//            }

//            try
//            {
//                // Begin MID Issue 3408 stodd
//                // Need to init selling square feet to zero
//                if (SellingSqFt != null && SellingSqFt != string.Empty)  // TT#3651 - JSmith - Store Load Failure
//                {
//                    //_storeProfile.SellingSqFt = Convert.ToInt32(SellingSqFt, CultureInfo.CurrentUICulture);
//                    profileSellingSqFt = Convert.ToInt32(SellingSqFt, CultureInfo.CurrentUICulture);
//                }
//                // Begin TT#2020 - JSmith - Lead Time in Store Profiles
//                //else
//                //    _storeProfile.SellingSqFt = 0;
//                // End TT#2020
//                // End MID Issue 3408 stodd
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Selling Sq Ft");
//                msgDetails = msgDetails.Replace("{2}",SellingSqFt.ToString(CultureInfo.CurrentUICulture));
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);
//            }

//            if (ShipDate != null)
//            {
//                //_storeProfile.ShipOnFriday    = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("F") > -1);	// So I just check to see if the letter is in there and return that boolean
//                //_storeProfile.ShipOnMonday    = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("M") > -1);
//                //_storeProfile.ShipOnSaturday  = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("SA") > -1);
//                //_storeProfile.ShipOnSunday    = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("SU") > -1);
//                //_storeProfile.ShipOnThursday  = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("TH") > -1);
//                //_storeProfile.ShipOnTuesday   = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("TU") > -1);
//                //_storeProfile.ShipOnWednesday = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("W") > -1);

//                profileShipOnFriday    = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("F") > -1);	// So I just check to see if the letter is in there and return that boolean
//                profileShipOnMonday    = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("M") > -1);
//                profileShipOnSaturday  = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("SA") > -1);
//                profileShipOnSunday    = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("SU") > -1);
//                profileShipOnThursday  = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("TH") > -1);
//                profileShipOnTuesday   = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("TU") > -1);
//                profileShipOnWednesday = (ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("W") > -1);
//            }

			

//            try
//            {
//                if (StockCloseDate != null && StockCloseDate != string.Empty)  // TT#3651 - JSmith - Store Load Failure
//                {
//                    //_storeProfile.StockCloseDt = Convert.ToDateTime(StockCloseDate, CultureInfo.CurrentUICulture);
//                    profileStockCloseDt = Convert.ToDateTime(StockCloseDate, CultureInfo.CurrentUICulture);
//                }
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Stock Close Date");
//                msgDetails = msgDetails.Replace("{2}",StockCloseDate.ToString(CultureInfo.CurrentUICulture));
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);
//            }

//            try
//            {
//                if (StockOpenDate != null && StockOpenDate != string.Empty)  // TT#3651 - JSmith - Store Load Failure
//                {
//                    //_storeProfile.StockOpenDt = Convert.ToDateTime(StockOpenDate, CultureInfo.CurrentUICulture);
//                    profileStockOpenDt = Convert.ToDateTime(StockOpenDate, CultureInfo.CurrentUICulture);
//                }
//            }
//            catch
//            {
//                updateSuccessful = false;
//                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreFieldDetails, false);
//                msgDetails = msgDetails.Replace("{0}",StoreID);
//                msgDetails = msgDetails.Replace("{1}","Stock Open Date");
//                msgDetails = msgDetails.Replace("{2}",StockOpenDate.ToString(CultureInfo.CurrentUICulture));
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, msgDetails, _sourceModule);
//            }

//            if (StoreName != null)
//            {
//                //_storeProfile.StoreName = StoreName.Trim();
//                profileStoreName = StoreName.Trim();
//            }

//            if (StoreID != null)
//            {
//                //_storeProfile.StoreId = StoreID.Trim();
//                profileStoreId = StoreID.Trim();
//            }

//            if (StoreDescription != null)
//            {
//                //_storeProfile.StoreDescription = StoreDescription.Trim();
//                profileStoreDescription = StoreDescription.Trim();
//            }
//            else
//            {
//                if (StoreName != null)
//                {
//                    //_storeProfile.StoreDescription = StoreName.Trim();
//                    profileStoreDescription = StoreName.Trim();
//                }
//            }

//            // BEGIN TT#1401 - stodd - add VSWID
//            if (ImoId != null)
//            {
//                //_storeProfile.IMO_ID = ImoId.Trim();
//                profileIMO_ID = ImoId.Trim();
//            }
//            // END TT#1401 - stodd - add VSWID


//            _storeProfile.SetFieldsOnProfile(
//                profileActiveInd: profileActiveInd,
//                profileCity: profileCity,
//                profileState: profileState,
//                profileLeadTime: profileLeadTime,
//                profileSellingCloseDt: profileSellingCloseDt,
//                profileSellingOpenDt: profileSellingOpenDt,
//                profileSellingSqFt: profileSellingSqFt,
//                profileShipOnFriday: profileShipOnFriday,
//                profileShipOnMonday: profileShipOnMonday,
//                profileShipOnSaturday: profileShipOnSaturday,
//                profileShipOnSunday: profileShipOnSunday,
//                profileShipOnThursday: profileShipOnThursday,
//                profileShipOnTuesday: profileShipOnTuesday,
//                profileShipOnWednesday: profileShipOnWednesday,
//                profileStockCloseDt: profileStockCloseDt,
//                profileStockOpenDt: profileStockOpenDt,
//                profileStoreName: profileStoreName,
//                profileStoreId: profileStoreId,
//                profileStoreDescription: profileStoreDescription,
//                profileIMO_ID: profileIMO_ID
//                );


//            return updateSuccessful;
//        }

//        // Begin TT# 166 - stodd
//        public void AddStoreCharacteristics(string aStoreCharacteristic, string Value, string aStoreCharType, ref storeCharInfo charInfo)
//        {
//            eStoreCharType storeCharType = eStoreCharType.unknown;
//            switch (aStoreCharType)
//            {
//                case "Date":
//                    storeCharType = eStoreCharType.date;
//                    break;
//                case "Dollar":
//                    storeCharType = eStoreCharType.dollar;
//                    break;
//                case "Number":
//                    storeCharType = eStoreCharType.number;
//                    break;
//                case "Text":
//                    storeCharType = eStoreCharType.text;
//                    break;
//                default:
//                    storeCharType = eStoreCharType.text;
//                    break;
//            }

//            AddStoreCharacteristics(aStoreCharacteristic, Value, storeCharType, ref charInfo);
//        }
//        // End TT# 166 

//        /// <summary>-
//        /// Adds a characteristic to a store
//        /// </summary>
//        /// <param name="StoreCharacteristic">User defined store characteristic</param>
//        /// <param name="Value">Value of the characteristic</param>
//        /// <returns></returns>
//        public void AddStoreCharacteristics(string aStoreCharacteristic, string Value, eStoreCharType aStoreCharType, ref storeCharInfo charInfo)
//        {
          
//            charInfo.isDirty = true;
//            charInfo.action = storeCharInfoAction.InsertValue;
          

//            aStoreCharacteristic = aStoreCharacteristic.Trim();
//            // Begin Issue 3691 - stodd 2/7/2006
//            if (Value != null)
//                Value = Value.Trim();
//            // End issue 3691
    

//            // Begin TT#237 - JSmith - after transaction gets error, subsequent transactions not processed.
//            object val;
//            try
//            {
//                switch (aStoreCharType)
//                {
//                    case eStoreCharType.date:
//                        // Begin TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
//                        //val = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
//                        DateTime dt = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
//                        if (dt != Include.UndefinedDate &&
//                            !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(dt))
//                        {
//                            string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_DateNotWithinMerchandiseCalendar);
//                            msgDetails = msgDetails.Replace("{0}", dt.ToString("d", CultureInfo.CurrentUICulture));
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, msgDetails, _sourceModule);
//                            Value = "0001-01-01";
//                        }
//                        // End TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
//                        break;
//                    case eStoreCharType.dollar:
//                        val = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
//                        break;
//                    case eStoreCharType.number:
//                        val = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
//                        break;
//                }
//            }
//            catch
//            {
//                string msgDetails = "Store: " + _storeProfile.StoreId +
//                            " Characteristic: " + aStoreCharacteristic +
//                            " Value: " + Value;
//                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharateristicValue, msgDetails, _sourceModule);
//                return;
//            }
//            // End TT#237

//            charInfo.anyValue = Value;

    

//            int charGroupRid = StoreMgmt.StoreCharGroup_Find(aStoreCharacteristic); //TT#722 - stodd - store load and mixed case causing duplicates
//            bool characteristicGroupFound;
//            if (charGroupRid == Include.NoRID)
//            {
//                characteristicGroupFound = false;
//            }
//            else
//            {
//                characteristicGroupFound = true;
//            }

    
//            if (!characteristicGroupFound)
//            {
//                if (_characteristicAutoAdd)
//                {
//                    bool didInsertNewDynamicStoreGroup = false;
//                    int filterRID = Include.NoRID;
//                    //charGroupRid = StoreMgmt.StoreCharGroup_Insert(ref _editMsgs, ref didInsertNewDynamicStoreGroup, ref filterRID, true, Include.GlobalUserRID, aStoreCharacteristic, aStoreCharType, hasList: false, storeId: _storeProfile.StoreId);
//                    charInfo.scgRID = charGroupRid;
//                    if (didInsertNewDynamicStoreGroup) //Filter and its conditions have been saved and committed to the db by now.
//                    {
//                        filter f = filterDataHelper.LoadExistingFilter(filterRID);                     
//                        StoreGroupProfile groupProfile = StoreMgmt.StoreGroup_AddOrUpdate(f, isNewGroup: true, loadNewResults: false);  //Adds group and levels and results to the db.  Executes the filter.
//                        //((StoreTreeView)(this._EAB.StoreGroupExplorer.TreeView)).AfterSave(e.filterToSave, groupProfile);  //Adds db entry to the FOLDER table

//                        //save store group to folder join
//                        FolderDataLayer _dlFolder = new FolderDataLayer();

//                        DataTable dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.StoreGroupMainGlobalFolder);

//                        if (dtFolders == null)
//                        {
//                            //throw new Exception("Global Attributes Folder not defined");
//                            string msgDetails = "Store: " + _storeProfile.StoreId +
//                                    " Global Attributes Folder not defined for characteristic: " + aStoreCharacteristic;
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);
//                        }

//                        FolderProfile globalFolderProf = new FolderProfile(dtFolders.Rows[0]);
//                        _dlFolder.OpenUpdateConnection();
//                        try
//                        {
//                            _dlFolder.Folder_Item_Insert(globalFolderProf.Key, groupProfile.Key, eProfileType.StoreGroup);
//                            _dlFolder.CommitData();
//                        }
//                        catch (Exception exc)
//                        {
//                            string message = exc.ToString();
//                            throw;
//                        }
//                        finally
//                        {
//                            _dlFolder.CloseUpdateConnection();
//                        }
//                    }


//                    characteristicGroupFound = true;
//                }
//                else
//                {
                   
//                    string msgDetails = "Store: " + _storeProfile.StoreId +
//                        " Invalid Characteristic Name: " + aStoreCharacteristic;
//                    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);

//                }
//            }

//            if (characteristicGroupFound)
//            {
//                //=========================================================================================
//                // if the value is null or empty
//                // the value for this characterisitic will be removed from the store--that is made NULL--
//                // by the store session
//                //=========================================================================================
//                if (Value == null || Value == string.Empty || Value == "0001-01-01")  // Issue 4659 stodd 9.6.2007
//                {
//                    //isValid = true;
//                }
//                else
//                {
//                    //Determine if the current group is a list
//                    StoreCharMaint storeCharMaintData = new StoreCharMaint();
//                    DataTable dtCharGroup = storeCharMaintData.StoreCharGroup_Read(charGroupRid);

//                    bool isList = false;
//                    if (dtCharGroup.Rows[0]["SCG_LIST_IND"] != DBNull.Value)
//                    {
//                        int listInd = (int)dtCharGroup.Rows[0]["SCG_LIST_IND"];
//                        if (listInd != 0) isList = true;
//                    }

//                    fieldDataTypes charGroupFieldType = fieldDataTypes.FromCharIgnoreLists(Convert.ToInt32(dtCharGroup.Rows[0]["SCG_TYPE"]));


//                    int scRidDuplicate = Include.NoRID;
//                    bool doesCharValueAlreadyExist = StoreMgmt.DoesStoreCharValueAlreadyExist(Value, charGroupFieldType, charGroupRid, Include.NoRID, ref scRidDuplicate);
//                    if (isList)
//                    {
//                        if (doesCharValueAlreadyExist == false)
//                        {
//                            string msgDetails = "Store: " + _storeProfile.StoreId +
//                                " Characteristic: " + aStoreCharacteristic +
//                                " Value: " + Value;
//                            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharListValue, msgDetails, _sourceModule);
//                        }
//                    }
//                    else //no validation list
//                    {
//                        if (Value == null || Value.Length == 0)
//                        {
//                            string msgDetails = "Store: " + _storeProfile.StoreId +
//                                ".  Value not found for Characteristic Name: " + aStoreCharacteristic +
//                                ".  Characteristic not processed";
//                            _editMsgs.AddMsg(eMIDMessageLevel.Warning, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);
//                        }
//                    }

//                }
//            }


          

//            //if (characteristicGroupFound)
//            //{
//            //    //charGroupRID = charGroup.RID;
//            //    //valueDataType = charGroup.DataType;
//            //    //=========================================================================================
//            //    // if the value is null or empty
//            //    // the value for this characterisitic will be removed from the store--that is made NULL--
//            //    // by the store session
//            //    //=========================================================================================
//            //    if (Value == null || Value == string.Empty || Value == "0001-01-01")  // Issue 4659 stodd 9.6.2007
//            //    {
//            //        isValid = true;
//            //        isEmpty = true;
//            //    }
//            //    else if (charGroup.HasList)
//            //    {
//            //        foreach (StoreCharValue charValue in charGroup.Values)
//            //        {
//            //            switch (charValue.StoreCharType)
//            //            {
//            //                case eStoreCharType.date:
//            //                    if (Convert.ToDateTime(charValue.CharValue, CultureInfo.CurrentUICulture) == Convert.ToDateTime(Value, CultureInfo.CurrentUICulture))
//            //                    {
//            //                        isValid = true;
//            //                    }
//            //                    break;
//            //                case eStoreCharType.dollar:
//            //                    if (Convert.ToDecimal(charValue.CharValue, CultureInfo.CurrentUICulture) == Convert.ToDecimal(Value, CultureInfo.CurrentUICulture))
//            //                    {
//            //                        isValid = true;
//            //                    }
//            //                    break;
//            //                case eStoreCharType.number:
//            //                    // Begin Issue 4190 changed from ToInt32 to ToDecimal
//            //                    if (Convert.ToDecimal(charValue.CharValue, CultureInfo.CurrentUICulture) == Convert.ToDecimal(Value, CultureInfo.CurrentUICulture))
//            //                    // End Issue 4190 changed from ToInt32 to ToDecimal
//            //                    {
//            //                        isValid = true;
//            //                    }
//            //                    break;
//            //                case eStoreCharType.text:
//            //                    if (Convert.ToString(charValue.CharValue, CultureInfo.CurrentUICulture) == Value)
//            //                    {
//            //                        isValid = true;
//            //                    }
//            //                    break;
//            //                case eStoreCharType.unknown:
//            //                    if (Convert.ToString(charValue.CharValue, CultureInfo.CurrentUICulture) == Value)
//            //                    {
//            //                        isValid = true;
//            //                    }
//            //                    break;
//            //            }
//            //            if (isValid)
//            //            {
//            //                break;	// stop values loop
//            //            }
//            //        }
//            //        if (!isValid)
//            //        {
//            //            string msgDetails = "Store: " + _storeProfile.StoreId +
//            //                " Characteristic: " + aStoreCharacteristic +
//            //                " Value: " + Value;
//            //            _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharListValue, msgDetails, _sourceModule);
//            //        }
//            //    }
//            //    else	// no validation list
//            //    {
//            //        if (Value == null ||
//            //            Value.Length == 0)
//            //        {
//            //            isValid = false;
//            //            string msgDetails = "Store: " + _storeProfile.StoreId +
//            //                ".  Value not found for Characteristic Name: " + aStoreCharacteristic +
//            //                ".  Characteristic not processed";
//            //            _editMsgs.AddMsg(eMIDMessageLevel.Warning, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);
//            //        }
//            //        // BEGIN Issue 4659 stodd 9.6.2007
//            //        //						else
//            //        //							if (charGroup.DataType == eStoreCharType.date &&
//            //        //							Value == "0001-01-01")
//            //        //						{
//            //        //							isValid = false;
//            //        //							string msgDetails = "Store: " + _storeProfile.StoreId +
//            //        //								".  Undefined date provided for Characteristic Name: " + StoreCharacteristic +
//            //        //								".  Characteristic not processed";
//            //        //							_editMsgs.AddMsg(eMIDMessageLevel.Information, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);
//            //        //						}
//            //        // END Issue 4659
//            //        else
//            //        {
//            //            isValid = true;
//            //        }
//            //    }
//            //    //        break;	// stop characteristic loop
//            //    //    }
//            //    //}
//            //}

//            //if (isValid)
//            //{
//            //    // check for characteristic already assigned to store to update
//            //    foreach (StoreCharGroupProfile storeCharGroupProfile in _storeProfile.Characteristics)
//            //    {
//            //        if (storeCharGroupProfile.Name == aStoreCharacteristic)
//            //        {
//            //            characteristicFound = true;
//            //            scgp = storeCharGroupProfile;
//            //            break;
//            //        }
//            //        ++charIndex;
//            //    }
//            //    if (!characteristicFound)
//            //    {
//            //        scgp = new StoreCharGroupProfile(charGroupRID);
//            //        scgp.Name = aStoreCharacteristic;
//            //        sc = new StoreCharValue();
//            //        scgp.CharacteristicValue = sc;
//            //    }

//            //    sc = scgp.CharacteristicValue;

//            //    if (isEmpty)
//            //    {
//            //        sc.SC_RID = 0;
//            //        sc.CharValue = DBNull.Value;
//            //    }
//            //    else
//            //    {
//            //        switch (valueDataType)
//            //        {
//            //            case eStoreCharType.date:
//            //                sc.CharValue = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
//            //                sc.StoreCharType = eStoreCharType.date;
//            //                break;
//            //            case eStoreCharType.dollar:
//            //                sc.CharValue = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
//            //                sc.StoreCharType = eStoreCharType.dollar;
//            //                break;
//            //            case eStoreCharType.number:
//            //                // Begin Issue 4190 changed from ToInt32 to ToDecimal
//            //                sc.CharValue = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
//            //                // End Issue 4190 changed from ToInt32 to ToDecimal
//            //                sc.StoreCharType = eStoreCharType.number;
//            //                break;
//            //            case eStoreCharType.text:
//            //                sc.CharValue = Value;
//            //                sc.StoreCharType = eStoreCharType.text;
//            //                break;
//            //            case eStoreCharType.unknown:
//            //                sc.CharValue = Value;
//            //                sc.StoreCharType = eStoreCharType.unknown;
//            //                break;
//            //        }

//            //        sc.SC_RID = _SAB.StoreServerSession.StoreCharExists(aStoreCharacteristic, sc.CharValue);
//            //    }

//            //    sc.Name = aStoreCharacteristic;

//            //    scgp.CharacteristicValue = sc;
//            //    if (characteristicFound)
//            //    {
//            //        _storeProfile.Characteristics[charIndex] = scgp;	// Update the characterstic
//            //    }
//            //    else
//            //    {
//            //        _storeProfile.Characteristics.Add(scgp);	// Add the characterstic
//            //    }
//            //}
			
//        }

//        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//        //private bool ParseStoreCharName(ref string storeChar, ref eStoreCharType storeCharType)
//        private bool ParseStoreCharName(ref string storeChar, ref eStoreCharType storeCharType, out string aStoreCharName)
//        // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//        {
//            bool isValid = true;
//            // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//            aStoreCharName = string.Empty;
//            // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name

//            // BEGIN Issue 1246 - gtaylor 4/6/2011
//            string origStoreChar = (storeChar ?? string.Empty.ToString()).Trim();
//            // END

//            // Begin TT#627-MD - JSmith - Store load fails if characteristics uses combinations of including characteristic type and not including type to add new characteristics.
//            aStoreCharName = origStoreChar;
//            // End TT#627-MD - JSmith - Store load fails if characteristics uses combinations of including characteristic type and not including type to add new characteristics.
//            storeCharType = eStoreCharType.text;

//            try
//            {
//                if (_CharacteristicDelimiter == string.Empty)
//                {
//                    storeCharType = eStoreCharType.text;
//                }
//                else if (origStoreChar.Contains(_CharacteristicDelimiter))
//                {
//                    int delimIndex = origStoreChar.IndexOf(_CharacteristicDelimiter);
//                    if (delimIndex > -1)
//                    {
//                        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                        //origStoreChar = origStoreChar.Substring(0, delimIndex);
//                        aStoreCharName = origStoreChar.Substring(0, delimIndex);
//                        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                        string charType = (origStoreChar.Substring(delimIndex + 1)).ToUpper();
//                        switch (charType)
//                        {
//                            case "TEXT":
//                                storeCharType = eStoreCharType.text;
//                                break;
//                            case "DATE":
//                                storeCharType = eStoreCharType.date;
//                                break;
//                            case "NUMBER":
//                                storeCharType = eStoreCharType.number;
//                                break;
//                            case "DOLLAR":
//                                storeCharType = eStoreCharType.dollar;
//                                break;
//                            default:
//                                isValid = false;
//                                string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharType, false);
//                                // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                                //msgDetails = msgDetails.Replace("{0}", origStoreChar);
//                                msgDetails = msgDetails.Replace("{0}", aStoreCharName);
//                                // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                                msgDetails = msgDetails.Replace("{1}", charType);
//                                _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        storeCharType = eStoreCharType.text;
//                        origStoreChar = origStoreChar.Trim();
//                        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                        aStoreCharName = origStoreChar;
//                        // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }

//            return isValid;
//        }

//        // BEGIN TT#739-MD - STodd - delete stores
//        public void UpdateStore(List<storeCharInfo> charList)
//        {
//            UpdateStore(StoresStoreAction.Update, charList);
//        }
//        // END TT#739-MD - STodd - delete stores

//        /// <summary>
//        /// Adds or updates the store
//        /// </summary>
//        /// <returns></returns>
//        public void UpdateStore(StoresStoreAction storeAction, List<storeCharInfo> charList)	// TT#739-MD - STodd - delete stores
//        {

//            //eChangeType changeType = StoreValidation.ProcessStoreData(ref _editMsgs, _storeProfile, charList, storeAction); // Add or update the store
//            // Begin MID Track #4733 - JSmith - Close date preceeds open date
//            //if (changeType == eChangeType.none)
//            //{

//            //}
//            //else
//            //// End MID Track #4733
//            //if (changeType == eChangeType.add)
//            //{
//            //    // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//            //    if (_currentStoreId == string.Empty)
//            //    {
//            //        ++_recordsAdded;
//            //    }
//            //    else
//            //    {
//            //        if (_currentStoreId != _storeProfile.StoreId)
//            //        {
//            //            ++_recordsAdded;
//            //        }
//            //    }
//            //    // END TT#1401 - stodd - add resevation stores (IMO)
//            //}
//            //else
//            //{
//            //    // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//            //    // BEGIN TT#739-MD - STodd - delete stores
//            //    if (storeAction == StoresStoreAction.Delete)
//            //    {
//            //        ++_recordsDeleted;
//            //    } 
//            //    else if (storeAction == StoresStoreAction.Recover)
//            //    {
//            //        ++_recordsRecovered;
//            //    } 
//            //    else if (_currentStoreId == string.Empty)
//            //    // END TT#739-MD - STodd - delete stores
//            //    {
//            //        ++_recordsUpdated;
//            //    }
//            //    else
//            //    {
//            //        if (_currentStoreId != _storeProfile.StoreId)
//            //        {
//            //            ++_recordsUpdated;
//            //        }
//            //    }
//            //    // END TT#1401 - stodd - add resevation stores (IMO)
//            //}
//            //// End MID Track #4668

//            //// Begin Issue 4301 - stodd (1/30/2007)
//            //// This was moved back here after is was discovered that moving it caused the XML files not to get their 
//            //// error messages in the audit log.
//            //HandleErrorMessages();
//            //// End Issue 4301 - stodd
//        }

//        private void HandleErrorMessages()
//        {
//            // Begin TT# 166 - stodd
//            if (_editMsgs.ErrorFound)
//            // End TT# 166
//            {
//                ++_recordsWithErrors;
//            }
//            for (int e=0; e<_editMsgs.EditMessages.Count; e++)
//            {
//                EditMsgs.Message emm = (EditMsgs.Message) _editMsgs.EditMessages[e];
//                // Begin TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
//                //_SAB.StoreServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
//                if (emm.code == eMIDTextCode.Unassigned)
//                {
//                    _SAB.StoreServerSession.Audit.Add_Msg(emm.messageLevel, emm.msg, emm.module);
//                }
//                else
//                {
//                    _SAB.StoreServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
//                }
//                // End TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
//            }
//            // BEGIN Issue 4667 - stodd 9.18.2007 moved this clear to here.
//            _editMsgs.ClearMsgs();
//            // END Issue 4667
//        }
//    }

//    /// <summary>
//    /// Local class's exception type.
//    /// </summary>
//    [Serializable]
//    public class XMLStoreLoadProcessException : Exception
//    {
//        /// <summary>
//        /// Used when throwing exceptions in the XML Store Load Class
//        /// </summary>
//        /// <param name="message">The error message to display</param>
//        public XMLStoreLoadProcessException(string message): base(message)
//        {
//        }
//        public XMLStoreLoadProcessException(string message, Exception innerException): base(message, innerException)
//        {
//        }
//        public XMLStoreLoadProcessException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
//            : base(aInfo, aContext)
//        {
//        }
//        override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
//        {
//            base.GetObjectData(aInfo, aContext);
//        }
//    }
//}
