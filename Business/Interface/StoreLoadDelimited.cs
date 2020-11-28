using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
    public static class StoreLoadDelimited
    {
        /// <summary>
        /// Rather than implementing two seperate load functions, we parse the delimited file into 
        /// the stores deserialization class and use that base function.
        /// </summary>
        /// <param name="delimitedFileName">The name of the file to load and parse.</param>
        /// <param name="delimiter">The delimiter use to seperate fields in the file.</param>
        /// <returns>A Stores instance that may then be passed to ProcessStores</returns>
        public static List<StoreLoadRecord> GetRecordsFromDelimitedFile(string delimitedFileName, char[] delimiter, bool autoAddCharacteristics, bool useCharacteristicTransaction, out StoreLoadProcessingOptions processingOptions)
        {
            bool errorFound = false;

            //make store processing options and set initial options
            processingOptions = new StoreLoadProcessingOptions();
            processingOptions.autoAddCharacteristics = autoAddCharacteristics;
            processingOptions.characteristicDelimiter = string.Empty;
            processingOptions.useCharacteristicTransaction = useCharacteristicTransaction;

        
            if (!File.Exists(delimitedFileName))	// Make sure our file exists...
            {
                throw new XMLStoreLoadProcessException(String.Format("Store Service can not find the file located at '{0}'", delimitedFileName));
            }

            List<StoreLoadRecord> recordList = new List<StoreLoadRecord>();

            StreamReader sr = new StreamReader(delimitedFileName);
            string[] items;

            string line = sr.ReadLine();
            while (line != null)
            {
                items = MIDstringTools.Split(line, delimiter[0], true);

                // skip blank line
                if ((items.Length == 1 && (items[0] == null || items[0] == "")) || items.Length == 0)
                {
                    line = sr.ReadLine();
                    continue;
                }

                //make a new store record and add the record to the list
                StoreLoadRecord storeRecord = new StoreLoadRecord();
                recordList.Add(storeRecord);


            


                // There should be a minimum of 15 items due to the requirements of the STORES table.
                // If there is only 1 item then odds are the delimiter is mismatched between the 
                // config file and the input file.
                if ((items[0].ToUpper() == "S" && items.Length < 15) || (items[0].ToUpper() != "S" && items.Length < 13))		
                {
                    if (items[0].ToUpper() == "OPTIONS")
                    {
                        // this is a valid record.
                    }
                    else if (useCharacteristicTransaction && items[0].ToUpper() == "C")
                    {
                        // this is a valid record. Characteriscs may have less items.
                    }
                    else if (useCharacteristicTransaction && items.Length == 2 && (items[0].ToUpper() == "D" || items[0].ToUpper() == "R"))
                    {
                        // this is a valid record. Delete transactions only need the store id.
                    }
                    else
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_StoreMismatchedDelimiter); 
                        if (items[0].ToUpper() == "S")
                        {
                            msgDetails = msgDetails.Replace("{0}", items[1]);
                        }
                        else
                        {
                            msgDetails = msgDetails.Replace("{0}", items[0]);
                        }
                        msgDetails = msgDetails.Replace("{1}", delimiter.ToString());
                        storeRecord.hasError = true;
                        storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreMismatchedDelimiter, msg = msgDetails });
                        errorFound = true;                       
                    }
                }


                if (errorFound)
                {
                    line = sr.ReadLine();
                    continue;
                }

                // Begin TT# 166 - stodd
                if (items[0].ToUpper() == "OPTIONS")
                {
                    storeRecord.recordAction = StoreLoadRecordActions.processOptions;
                    //errorFound = ProcessOptions(line, items);
                    // Characteristic Auto Add
                    if (items.Length > 1 && items[1] != null && items[1].Length > 0)
                    {
                        try
                        {
                            processingOptions.autoAddCharacteristics = Convert.ToBoolean(items[1]);
                        }
                        catch
                        {
                            errorFound = true;
                            string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidCharAutoAddValue); 
                            msgDetails = msgDetails.Replace("{0}", items[1].ToString(CultureInfo.CurrentUICulture));
                            storeRecord.hasError = true;
                            storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidCharAutoAddValue, msg = msgDetails });
                        }
                    }

                    // Characteristic Type Delimiter
                    if (items.Length > 2 && items[2] != null && items[2].Length > 0)
                    {
                        try
                        {
                            processingOptions.characteristicDelimiter = (Convert.ToString(items[2])).Trim();
                            if ((processingOptions.characteristicDelimiter.Trim()).Length != 1)
                            {
                                errorFound = true;
                                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidCharTypeDelimiter); 
                                msgDetails = msgDetails.Replace("{0}", processingOptions.characteristicDelimiter);
                                storeRecord.hasError = true;
                                storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidCharTypeDelimiter, msg = msgDetails });
                            }
                        }
                        catch
                        {
                            errorFound = true;
                            string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidCharTypeDelimiter); 
                            msgDetails = msgDetails.Replace("{0}", items[2].ToString(CultureInfo.CurrentUICulture));                         
                            storeRecord.hasError = true;
                            storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidCharTypeDelimiter, msg = msgDetails });
                        }
                    }

                    // BEGIN TT#1401 - stodd - add resevation stores (IMO)
                    // Use Transaction Types
                    if (items.Length > 3 && items[3] != null && items[3].Length > 0)
                    {
                        try
                        {
                            useCharacteristicTransaction = bool.Parse((Convert.ToString(items[3])).Trim());
                        }
                        catch
                        {
                            errorFound = true;
                            string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidUseCharacteristicTransaction); 
                            msgDetails = msgDetails.Replace("{0}", items[3].ToString(CultureInfo.CurrentUICulture));
                            storeRecord.hasError = true;
                            storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidUseCharacteristicTransaction, msg = msgDetails });
                        }
                    }

                    if (errorFound)
                    {
                        //HandleErrorMessages();
                        break;
                    }
                }
                // BEGIN TT#1401 - stodd - add resevation stores (IMO)
                else if (useCharacteristicTransaction)
                {
    
                    if (items[0].ToUpper() == "S")
                    {
                        storeRecord.recordAction = StoreLoadRecordActions.processStore;

                        storeRecord.StoreID = items[1];
                        storeRecord.StoreName = items[2];
                        storeRecord.StoreDescription = items[3];
                        storeRecord.City = items[4];
                        storeRecord.State = items[5];
                        storeRecord.SellingSqFt = items[6];
                        storeRecord.SellingOpenDate = items[7];
                        storeRecord.SellingCloseDate = items[8];
                        storeRecord.StockOpenDate = items[9];
                        storeRecord.StockCloseDate = items[10];
                        storeRecord.ActiveIndicator = items[11];
                        storeRecord.LeadTime = items[12];
                        storeRecord.ShipDate = items[13];
                        storeRecord.ImoId = items[14];



                        //++_storeRecs;
                        //StoreProfile _storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);
                        //_storeProfile.Characteristics = new ArrayList();
                       // bool success = AddStore(items[1], items[2], items[3], items[4], items[5], items[6], items[7], items[8], items[9], items[10], items[11], items[12], items[13], items[14]);

                       // UpdateStore(null);
                        //_currentStoreId = items[1];
                    }
                    else if (items[0].ToUpper() == "C")
                    {
                        storeRecord.recordAction = StoreLoadRecordActions.processCharacteristic;
                        storeRecord.StoreID = items[1];


                        //++_storeRecs;
                        //StoreProfile _storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);

                        // Check to be sure store exists
                        //if (_storeProfile.Key == Include.UndefinedStoreRID)
                        //{
                        //    errorFound = true;
                        //    string msgDetails = "Store: " + items[1];
                        //    //_editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CharacteristicAddedPriorToStore, msgDetails, _sourceModule);
                        //    storeRecord.hasError = true;
                        //    storeRecord.errorMsgLevel = eMIDMessageLevel.Edit;
                        //    storeRecord.errorTextCode = eMIDTextCode.msg_CharacteristicAddedPriorToStore;
                        //    storeRecord.errorMsg = msgDetails;


                        //    line = sr.ReadLine();
                        //    continue;
                        //}

                        //_storeProfile.Characteristics = new ArrayList();
                        GetCharacteristicValuesForThisStore(items, 2, processingOptions, ref storeRecord, ref errorFound);
                    }
                    // BEGIN TT#739-MD - STodd - delete stores
                    else if (items[0].ToUpper() == "D")		// Mark Store For Deletion
                    {
                        storeRecord.recordAction = StoreLoadRecordActions.processMarkStoreForDeletion;
                        storeRecord.StoreID = items[1];


                        ////++_storeRecs;
                        //_storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);

                        //// Check to be sure store exists
                        //if (_storeProfile.Key == Include.UndefinedStoreRID)
                        //{
                        //    errorFound = true;
                        //    string msgDetails = "Store: " + items[1];
                        //    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, msgDetails, _sourceModule);
                        //    line = sr.ReadLine();
                        //    continue;
                        //}

                        //if (_storeProfile.ActiveInd)
                        //{
                        //    errorFound = true;
                        //    string msgDetails = "Store: " + items[1];
                        //    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotInactiveForDelete, msgDetails, _sourceModule);
                        //    line = sr.ReadLine();
                        //    continue;
                        //}

                
                        //if (_storeProfile.Key == _SAB.StoreServerSession.GlobalOptions.ReserveStoreRID)
                        //{
                        //    errorFound = true;
                        //    string msgDetails = "Store: " + items[1];
                        //    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CannotDeleteReserveStore, msgDetails, _sourceModule);
                        //    line = sr.ReadLine();
                        //    continue;
                        //}
                      
                    
                        //UpdateStore(StoresStoreAction.Delete, null);
                        //_currentStoreId = items[1];
                    

                    }
                    else if (items[0].ToUpper() == "R")		// Recover store that had been marked for deletion
                    {
                        storeRecord.recordAction = StoreLoadRecordActions.processRecoverStoreFromDeletion;
                        storeRecord.StoreID = items[1];
                        //++_storeRecs;
                        //_storeProfile = StoreMgmt.StoreProfile_Get(items[1]); //_SAB.StoreServerSession.GetStoreProfile(items[1]);

                        //// Check to be sure store exists
                        //if (_storeProfile.Key == Include.UndefinedStoreRID)
                        //{
                        //    errorFound = true;
                        //    string msgDetails = "Store: " + items[1];
                        //    _editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, msgDetails, _sourceModule);
                        //    line = sr.ReadLine();
                        //    continue;
                        //}

                        //// BEGIN TT#739-MD - STodd - delete stores
                        //UpdateStore(StoresStoreAction.Recover, null);
                        //_currentStoreId = items[1];
                        //// END TT#739-MD - STodd - delete stores

                    }
                    else
                    {
                        errorFound = true;
                        string msgDetails = "Store: " + items[1];
                        storeRecord.hasError = true;
                        storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_TransactionTypeInvalid, msg = msgDetails });
                    }
                }
                else
                {
                    storeRecord.recordAction = StoreLoadRecordActions.processStore;
                      
                    storeRecord.StoreID = items[0];
                    storeRecord.StoreName = items[1];
                    storeRecord.StoreDescription = items[2];
                    storeRecord.City = items[3];
                    storeRecord.State = items[4];
                    storeRecord.SellingSqFt = items[5];
                    storeRecord.SellingOpenDate = items[6];
                    storeRecord.SellingCloseDate = items[7];
                    storeRecord.StockOpenDate = items[8];
                    storeRecord.StockCloseDate = items[9];
                    storeRecord.ActiveIndicator = items[10];
                    storeRecord.LeadTime = items[11];
                    storeRecord.ShipDate = items[12];
                    storeRecord.ImoId = null;


                    GetCharacteristicValuesForThisStore(items, 13, processingOptions, ref storeRecord, ref errorFound);
                 
                }
                line = sr.ReadLine();
            }

            sr.Close();

      

            return recordList;
        }

        private static void GetCharacteristicValuesForThisStore(string[] items, int startItem, StoreLoadProcessingOptions processingOptions, ref StoreLoadRecord storeRecord, ref bool errorFound)
        {
            try
            {
                int dynamicCharacteristsCount = (items.Length - startItem) / 2;
                if (((items.Length - startItem) % 2) == 0)	// make sure characteristics are paired
                {
                    int c = startItem;
                    for (int i = 0; i < dynamicCharacteristsCount; i++)
                    {
                        eStoreCharType newStoreCharType = eStoreCharType.unknown;
                        string newStoreCharName;
                        if (ParseStoreCharName(items[c], processingOptions, ref storeRecord, out newStoreCharType, out newStoreCharName))
                        {
                            if (items[c] != null)
                            {
                                storeRecord.characteristicRecordList.Add(new StoreLoadCharacteristicRecord { storeCharType = newStoreCharType, storeCharName = newStoreCharName, storeCharValue = items[c+1] });
                            }
                            c += 2;	// characteristics are in pairs
                        }
                        else
                        {
                            errorFound = true;
                        }
                    }
                }
                else
                {
                    errorFound = true;
                    string msgDetails = "Store: " + storeRecord.StoreID;
                    storeRecord.hasError = true;
                    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidCharNotPaired, msg = msgDetails });
                }
            }
            catch
            {
                throw;
            }
        }
        private static bool ParseStoreCharName(string storeChar, StoreLoadProcessingOptions processingOptions, ref StoreLoadRecord storeRecord, out eStoreCharType storeCharType, out string aStoreCharName)
        {
            bool isValid = true;
            // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
            aStoreCharName = string.Empty;
            // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name

            // BEGIN Issue 1246 - gtaylor 4/6/2011
            string origStoreChar = (storeChar ?? string.Empty.ToString()).Trim();
            // END

            // Begin TT#627-MD - JSmith - Store load fails if characteristics uses combinations of including characteristic type and not including type to add new characteristics.
            aStoreCharName = origStoreChar;
            // End TT#627-MD - JSmith - Store load fails if characteristics uses combinations of including characteristic type and not including type to add new characteristics.
            storeCharType = eStoreCharType.text;

            try
            {
                if (processingOptions.characteristicDelimiter == string.Empty)
                {
                    storeCharType = eStoreCharType.text;
                }
                else if (origStoreChar.Contains(processingOptions.characteristicDelimiter))
                {
                    int delimIndex = origStoreChar.IndexOf(processingOptions.characteristicDelimiter);
                    if (delimIndex > -1)
                    {
                        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
                        //origStoreChar = origStoreChar.Substring(0, delimIndex);
                        aStoreCharName = origStoreChar.Substring(0, delimIndex);
                        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
                        string charType = (origStoreChar.Substring(delimIndex + 1)).ToUpper();
                        switch (charType)
                        {
                            case "TEXT":
                                storeCharType = eStoreCharType.text;
                                break;
                            case "DATE":
                                storeCharType = eStoreCharType.date;
                                break;
                            case "NUMBER":
                                storeCharType = eStoreCharType.number;
                                break;
                            case "DOLLAR":
                                storeCharType = eStoreCharType.dollar;
                                break;
                            default:
                                isValid = false;
                                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidCharType); 
                                msgDetails = msgDetails.Replace("{0}", aStoreCharName);
                                msgDetails = msgDetails.Replace("{1}", charType);
                                storeRecord.hasError = true;
                                storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharateristic, msg = msgDetails });
                                break;
                        }
                    }
                    else
                    {
                        storeCharType = eStoreCharType.text;
                        origStoreChar = origStoreChar.Trim();
                        // Begin TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
                        aStoreCharName = origStoreChar;
                        // End TT#611-MD - JSmith - Store Load gets Argument out of range error when parsing store characteristic name
                    }
                }
            }
            catch
            {
                throw;
            }

            return isValid;
        }
        
    }
}
