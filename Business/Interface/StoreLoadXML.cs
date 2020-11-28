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
    public static class StoreLoadXML
    {
   
        /// <summary>
        /// Internal function that performs actual adding of stores off of a deserialized stores class.
        /// </summary>
        /// <param name="xmlLoadFileName">Name of Xml file to load and deserialize. Ignored if inStores is not null.</param>
        /// <param name="inStores">A deserialized Stores class used to load stores from.</param>
        /// <returns></returns>
        public static List<StoreLoadRecord> GetRecordsFromXmlFile(string xmlLoadFileName, bool autoAddCharacteristics, out StoreLoadProcessingOptions processingOptions)
        {
            //make store processing options and set initial options
            processingOptions = new StoreLoadProcessingOptions();
            processingOptions.autoAddCharacteristics = autoAddCharacteristics;
            processingOptions.characteristicDelimiter = string.Empty;
            processingOptions.useCharacteristicTransaction = false;


            Stores _stores = null;


            if (!File.Exists(xmlLoadFileName))	// Make sure our file exists before attempting to deserialize
            {
                throw new XMLStoreLoadProcessException(String.Format("Store Service can not find the file located at '{0}'", xmlLoadFileName));
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
            try
            {
                /* Follow me here, I created MIDRetail.StoreLoad.StoreLoadSchema.xsd to define and validate
                    what a StoreLoad XML file should look like. From the Visual Studio command prompt I
                    run xsd /c StoreLoadSchema.xsd to generate a class file that is a strongly typed
                    represenation of that schema. The end result is I don't have to parse a loaded XML
                    document node by node which can result in errors if the Xml document is not formed
                    perfectly prior to reciept by this function.
                */
                XmlSerializer s = new XmlSerializer(typeof(Stores));	// Create a Serializer
                // Begin Track #4229 - JSmith - API locks .XML input file
                //TextReader r = new StreamReader(xmlLoadFileName);		// Load the Xml File
                r = new StreamReader(xmlLoadFileName);		// Load the Xml File
                // End Track #4229
                _stores = (Stores)s.Deserialize(r);						// Deserialize the Xml File to a strongly typed object
                // Begin Track #4229 - JSmith - API locks .XML input file
                //r.Close();												// Close the input file.
                // End Track #4229
            }
            catch (Exception ex)
            {
                throw new XMLStoreLoadProcessException(String.Format("Error encountered during deserialization of the file '{0}'", xmlLoadFileName), ex);
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            finally
            {
                if (r != null)
                    r.Close();
            }
            // End Track #4229

            List<StoreLoadRecord> recordList = new List<StoreLoadRecord>();

            try
            {
                if (_stores.Options != null)
                {
                    if (_stores.Options.AutoAddCharacteristicsSpecified)
                    {
                        processingOptions.autoAddCharacteristics = _stores.Options.AutoAddCharacteristics;
                    }
                }
                foreach (StoresStore s in _stores.Store)
                {
                    //make a new store record and add the record to the list
                    StoreLoadRecord storeRecord = new StoreLoadRecord();
                    recordList.Add(storeRecord);

                    if (s.Action == StoresStoreAction.Delete)
                    { 
                        storeRecord.recordAction = StoreLoadRecordActions.processMarkStoreForDeletion;
                        storeRecord.StoreID = s.ID;
                    }
                    else if (s.Action == StoresStoreAction.Recover)
                    {
                        storeRecord.recordAction = StoreLoadRecordActions.processRecoverStoreFromDeletion;
                        storeRecord.StoreID = s.ID;
                    }
                    else
                    {
                        storeRecord.recordAction = StoreLoadRecordActions.processStore;
                        storeRecord.StoreID = s.ID;
                        storeRecord.StoreName = s.Name;
                        storeRecord.StoreDescription = s.Description; 
                        storeRecord.City = s.City;
                        storeRecord.State = s.State;
                        storeRecord.ShipDate = Convert.ToString(s.ShipDate, CultureInfo.CurrentUICulture);
                        storeRecord.ImoId = s.VSWID;

                        if (s.ActiveIndicatorSpecified)
                        {
                            storeRecord.ActiveIndicator = Convert.ToString(s.ActiveIndicator, CultureInfo.CurrentUICulture);
                        }

                        if (s.SellingSquareFeetSpecified)
                        {
                            storeRecord.SellingSqFt = Convert.ToString(s.SellingSquareFeet, CultureInfo.CurrentUICulture);
                        }

                        if (s.SellingOpenDateSpecified)
                        {
                            storeRecord.SellingOpenDate = Convert.ToString(s.SellingOpenDate, CultureInfo.CurrentUICulture);
                        }
                       
                        if (s.SellingCloseDateSpecified)
                        {
                            storeRecord.SellingCloseDate = Convert.ToString(s.SellingCloseDate, CultureInfo.CurrentUICulture);
                        }

                        if (s.StockOpenDateSpecified)
                        {
                            storeRecord.StockOpenDate = Convert.ToString(s.StockOpenDate, CultureInfo.CurrentUICulture);
                        }

                        if (s.StockCloseDateSpecified)
                        {
                            storeRecord.StockCloseDate = Convert.ToString(s.StockCloseDate, CultureInfo.CurrentUICulture);
                        }

                        if (s.LeadTimeSpecified)
                        {
                            storeRecord.LeadTime = Convert.ToString(s.LeadTime, CultureInfo.CurrentUICulture);
                        }
                                                                  
                        if (s.Characteristic != null)
                        {
                            foreach (StoresStoreCharacteristic c in s.Characteristic) // Now loop through and add each characteristic and the corresponding value for this store
                            {
                                eStoreCharType newStoreCharType = eStoreCharType.unknown;
                                switch (c.CharType.ToString())
                                {
                                    case "Date":
                                        newStoreCharType = eStoreCharType.date;
                                        break;
                                    case "Dollar":
                                        newStoreCharType = eStoreCharType.dollar;
                                        break;
                                    case "Number":
                                        newStoreCharType = eStoreCharType.number;
                                        break;
                                    case "Text":
                                        newStoreCharType = eStoreCharType.text;
                                        break;
                                    default:
                                        newStoreCharType = eStoreCharType.text;
                                        break;
                                }
                                storeRecord.characteristicRecordList.Add(new StoreLoadCharacteristicRecord { storeCharType = newStoreCharType, storeCharName = c.Name, storeCharValue = c.Value });                   
                            }
                        }                  
                    }
                } 
            }
            catch (Exception ex)
            {
                throw new XMLStoreLoadProcessException(String.Format("Error encountered while processing the file '{0}'", xmlLoadFileName), ex);
            }

            return recordList;
        }
       
    }
}
