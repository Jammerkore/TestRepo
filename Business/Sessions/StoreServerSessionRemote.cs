using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    /// <summary>
    /// StoreServerSession is a class that contains fields, properties, and methods that are available to other sessions
    /// of the system.
    /// </summary>
    /// <remarks>
    /// The StoreServerSession class is the interface to the StoreServer functionality.  All requests for functionality
    /// or information in the StoreServer should be made through methods and properties in this class.
    /// </remarks>
    public class StoreServerSessionRemote : SessionRemote, IBatchLoadData
    {
       
        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of StoreSessionGlobal as either local or remote, depending on the value of aLocal
        /// </summary>
        /// <param name="aLocal">
        /// A boolean that indicates whether this class is being instantiated in the Client or in a remote service.
        /// </param>
        public StoreServerSessionRemote(bool aLocal)
            : base(aLocal)
        {
            try
            {
                //_storeData = new StoreData();
            }
            catch (Exception err)
            {
                string msg = "Constructor(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========
        public MRSCalendar CalendarGlobal
        {
            get
            {
                return StoreServerGlobal.Calendar;
            }
        }


  

        //========
        // METHODS
        //========
        /// <summary>
        /// Initializes the session.
        /// </summary>
        public override void Initialize()
        {
            try
            {

                //DateTime beginTime = System.DateTime.Now;
                base.Initialize();

                Calendar = StoreServerGlobal.Calendar;

                CreateAudit();
                // Setting Posting Date
                DateTime postingDate = SessionAddressBlock.HierarchyServerSession.GetPostingDate();
                Calendar.SetPostingDate(postingDate);

                // Begin TT#1861-MD - JSmith - Serialization error accessing the Audit
                //StoreMgmt.LoadInitialStoresAndGroups(SessionAddressBlock);  // TT#1859-MD - JSmith - Object Reference Error
                StoreMgmt.LoadInitialStoresAndGroups(SessionAddressBlock, SessionAddressBlock.StoreServerSession);  // TT#1859-MD - JSmith - Object Reference Error
                // End TT#1861-MD - JSmith - Serialization error accessing the Audit
                // Begin TT#1808-MD - JSmith - Store Load Error
                ExceptionHandler.Initialize(SessionAddressBlock.StoreServerSession, false);
                // End TT#1808-MD - JSmith - Store Load Error

                //DateTime endTime = System.DateTime.Now;
                //Debug.WriteLine("Store Session Init -- " + System.Convert.ToString(endTime.Subtract(beginTime), CultureInfo.CurrentUICulture));
            }
            catch (Exception err)
            {
                string msg = "Initialize(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                StoreServerGlobal.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public void CleanUpGlobal()
        {
            StoreServerGlobal.CleanUp();
        }

        /// <summary>
        /// Clears all cached areas in the session.
        /// </summary>
        public void Refresh()
        {
            try
            {
                StoreServerGlobal.AcquireCompleteWriterLock();
                try
                {
                    StoreServerGlobal.RefreshStoreText();
                    StoreServerGlobal.CalculateStatusForStores();
                    //RefreshSession();
                    //BuildRemainingItems();
                }
                finally
                {
                    StoreServerGlobal.ReleaseCompleteWriterLock();
                }
            }
            catch (Exception err)
            {
                string msg = "Refresh(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }

        /// <summary>
        /// Dump memory to event viewer
        /// </summary>
        public string ShowMemory()
        {
            try
            {
                return StoreServerGlobal.ShowMemory();
            }
            catch
            {
                throw;
            }
        }

    



        public void RefreshCalendar(DateTime refreshDate)
        {
            try
            {
                if (refreshDate != StoreServerGlobal.CalendarRefreshDate)
                {
                    StoreServerGlobal.Calendar.Refresh();
                    StoreServerGlobal.CalendarRefreshDate = refreshDate;
                }

                // Refresh the Calendar of THIS session
                Calendar.Refresh();
            }
            catch (Exception err)
            {
                string msg = "RefreshCalendar(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }

       

        /// <summary>
        /// Updates the posting date.
        /// </summary>
        /// <param name="postingDate">The posting date</param>
        /// <remarks>Currently updates the posting date for the organizational hierarchy</remarks>
        public void PostingDateUpdate(DateTime postingDate)
        {
            try
            {
                Calendar.SetPostingDate(postingDate);
                StoreServerGlobal.SetPostingDate(postingDate);
            }
            catch (Exception err)
            {
                string msg = "PostingDateUpdate(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }

        /// <summary>
        /// Identifies resources to release as the session expires.
        /// </summary>
        protected override void ExpiredCleanup()
        {
            try
            {
                // Begin TT#1243 - JSmith - Audit Performance
                base.ExpiredCleanup();
                // End TT#1243

            
            }
            catch (Exception err)
            {
                string msg = "ExpiredCleanup(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
            finally
            {
                StoreServerGlobal.GarbageCollect();
            }
        }

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public override void CloseSession()
        {
            try
            {
                base.CloseSession();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flush buffer and close audit
        /// </summary>
        public override void CloseAudit()
        {
            try
            {
                base.CloseAudit();
                StoreServerGlobal.CloseAudit();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

        // Begin TT#1440 - JSmith - Memory Issues
        override public void CleanUpSession()
        {
       

            base.CleanUpSession();
        }
        // End TT#1440


        // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
        public int GetAllStoresListCount()
        {
            return StoreServerGlobal.GetAllStoresList().Count;
        }
        // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
     

        //Begin TT#1517-MD -jsobek -Store Service Optimization
        public ProfileList GetAllStoresList()
        {
            return StoreServerGlobal.GetAllStoresList();
        }
        public int GetReaderLockTimeOut()
        {
            return StoreServerGlobal.GetReaderLockTimeOut();
        }
        public int GetWriterLockTimeOut()
        {
            return StoreServerGlobal.GetWriterLockTimeOut();
        }

        public eStoreStatus GetStoreStatusForCurrentWeek(DateTime openDt, DateTime closeDt)
        {
            try
            {
                return StoreServerGlobal.GetStoreStatusForCurrentWeek(openDt, closeDt);
            }
            catch (Exception err)
            {
                string msg = "GetStoreStatusForCurrentWeek(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }
        public void AddStoreProfileToList(StoreProfile sp)
        {
            try
            {
                StoreServerGlobal.AddStoreProfileToList(sp);
            }
            catch (Exception err)
            {
                string msg = "AddStoreProfileToList(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }
        public void UpdateStoreProfileInList(StoreProfile sp)
        {
            try
            {
                StoreServerGlobal.UpdateStoreProfileInList(sp);
            }
            catch (Exception err)
            {
                string msg = "UpdateStoreProfileInList(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }
        //End TT#1517-MD -jsobek -Store Service Optimization

      
     


       

        /// <summary>
        /// Gets either the store sales or stock status depending on the dates provided 
        /// </summary>
        /// <param name="baseWeek">The week for which you want to determine the stores status</param>
        /// <param name="openDt">The store sales or stock open date</param>
        /// <param name="closeDt">The store sales or stock close date</param>
        /// <returns></returns>
        public eStoreStatus GetStoreStatus(WeekProfile baseWeek, DateTime openDt, DateTime closeDt)
        {
            try
            {
                return StoreServerGlobal.GetStoreStatus(baseWeek, openDt, closeDt);
            }
            catch (Exception err)
            {
                string msg = "GetStoreStatus(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }

        // Begin TT#2090-MD - JSmith - Performance
        /// <summary>
        /// Gets the store sales status for a list of stores 
        /// </summary>
        /// <param name="baseWeek">The week for which you want to determine the stores status</param>
        /// <param name="stores">The list of store profiles</param>
        /// <returns></returns>
        public Dictionary<int, eStoreStatus> GetAllStoresSalesStatus(WeekProfile baseWeek, ArrayList stores)
        {
            try
            {
                Dictionary<int, eStoreStatus> storeStatusHash = new Dictionary<int, eStoreStatus>();
                foreach (StoreProfile sp in stores)
                {
                    eStoreStatus storeStatus = GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt);
                    storeStatusHash.Add(sp.Key, storeStatus);
                }
                return storeStatusHash;
            }
            catch (Exception err)
            {
                string msg = "GetAllStoresSalesStatus(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }

        /// <summary>
        /// Gets the store stock status for a list of stores 
        /// </summary>
        /// <param name="baseWeek">The week for which you want to determine the stores status</param>
        /// <param name="stores">The list of store profiles</param>
        /// <returns></returns>
        public Dictionary<int, eStoreStatus> GetAllStoresStockStatus(WeekProfile baseWeek, ArrayList stores)
        {
            try
            {
                Dictionary<int, eStoreStatus> storeStatusHash = new Dictionary<int, eStoreStatus>();
                foreach (StoreProfile sp in stores)
                {
                    eStoreStatus storeStatus = GetStoreStatus(baseWeek, sp.StockOpenDt, sp.StockCloseDt);
                    storeStatusHash.Add(sp.Key, storeStatus);
                }
                return storeStatusHash;
            }
            catch (Exception err)
            {
                string msg = "GetAllStoresStockStatus(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }
        // End TT#2090-MD - JSmith - Performance


        /// <summary>
        /// Implements the BatchLoadData interface and will load and create stores based on the passed in data
        /// </summary>
        /// <param name="SAB">SAB to use for loading the batch file</param>
        /// <param name="fileLocation">Location of the XML file to parse</param>
        /// <param name="commitLimit">Maximum number of records to add</param>
        /// <param name="exceptionFile">Location of file to store exceptions (error log)</param>
        /// <returns>True on success, false on failure</returns>
        // Begin TT#166 - JSmith - Store Characteristics auto add
        //public bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile)
        public bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile,
            bool autoAddCharacteristics)
        // End TT#166
        {
            try
            {
                //Begin TT#1517-MD -jsobek -Store Service Optimization
                //XMLStoreLoadProcess load = new XMLStoreLoadProcess(SAB, commitLimit, exceptionFile);
                //// Begin TT#166 - JSmith - Store Characteristics auto add
                ////return load.ProcessStores(fileLocation);
                //return load.ProcessStores(fileLocation, autoAddCharacteristics);
                //// End TT#166

                StoreMgmt.LoadInitialStoresAndGroups(SAB: SAB, session: SAB.StoreServerSession, bDoingRefresh: true);
                StoreLoadProcessManager load = new StoreLoadProcessManager(SAB);
                return load.ProcessXmlFile(fileLocation, autoAddCharacteristics);
                //End TT#1517-MD -jsobek -Store Service Optimization
            }
            catch (Exception err)
            {
                string msg = "LoadXMLTransFile(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }
        /// <summary>
        /// Implements the BatchLoadData interface and will load and create stores based on the passed in data
        /// </summary>
        /// <param name="SAB">SAB to use for loading the batch file</param>
        /// <param name="delimiter">Delimiter used in the file to seperate fields</param>
        /// <param name="fileLocation">Location of the delimited file to parse</param>
        /// <param name="commitLimit">Maximum number of records to add</param>
        /// <param name="exceptionFile">Location of file to store exceptions (error log)</param>
        /// <returns>True on success, false on failure</returns>
        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        // Begin TT#166 - JSmith - Store Characteristics auto add
        //public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile)
        public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
            bool autoAddCharacteristics, char[] characteristicDelimiter, bool useCharacteristicTransaction)
        // End TT#166
        // END TT#1401 - stodd - add resevation stores (IMO)
        {
            try
            {
                //Begin TT#1517-MD -jsobek -Store Service Optimization
                //XMLStoreLoadProcess load = new XMLStoreLoadProcess(SAB, commitLimit, exceptionFile);
                //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                //// Begin TT#166 - JSmith - Store Characteristics auto add
                ////return load.StoresFromDelimitedFile(fileLocation, delimiter);
                //return load.StoresFromDelimitedFile(fileLocation, delimiter, autoAddCharacteristics, characteristicDelimiter, useCharacteristicTransaction);
                //// End TT#166
                //// END TT#1401 - stodd - add resevation stores (IMO)

                StoreMgmt.LoadInitialStoresAndGroups(SAB: SAB, session: SAB.StoreServerSession, bDoingRefresh: true);
                StoreLoadProcessManager load = new StoreLoadProcessManager(SAB);
                return load.ProcessDelimitedFile(fileLocation, delimiter, autoAddCharacteristics, characteristicDelimiter, useCharacteristicTransaction);
                //End TT#1517-MD -jsobek -Store Service Optimization
            }
            catch (Exception err)
            {
                string msg = "LoadDelimitedTransFile(): " + err.ToString();
                EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(err, GetType().Name);
                }
                throw;
            }
        }
        // End TT#166

        // BEGIN TT#739-MD - STodd - delete stores	
        public bool DeleteStoreBatchProcess(SessionAddressBlock SAB)
        {
            bool errorFound = false;

            //if (_storeData.NumStoresMarkedForDelete() > 0)
            //{
            //    // STORE DELETE STORED PROCEDURE GOES HERE!!
            //}
            return errorFound;
        }
        // END TT#739-MD - STodd - delete stores

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return StoreServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#195 MD

       

   
    }
}
