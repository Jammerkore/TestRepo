using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
//Begin TT#708 - JScott - Services need a Retry availalbe.
using System.Threading;
//End TT#708 - JScott - Services need a Retry availalbe.

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public class StoreServerGlobal : Global
    {
        //=======
        // FIELDS
        //=======
      
        static private ArrayList _loadLock;
        static private bool _loaded;
        static private Audit _audit;



        static private MIDReaderWriterLock stores_rwl = new MIDReaderWriterLock();
        static private MIDReaderWriterLock calendar_rwl = new MIDReaderWriterLock();

        static private DataTable _dtAllStores;
        static private DataSet _dsAllStores;
        static private ProfileList _allStoreList;
        static private eStoreDisplayOptions _globalStoreDisplayOption;
        static private int _noncompStorePeriodEnd;
        static private int _noncompStorePeriodBegin;
        static private int _newStorePeriodEnd;
        static private int _newStorePeriodBegin;
        static private string _module = "StoreServerGlobal";

        //=============
        // CONSTRUCTORS
        //=============


        static StoreServerGlobal()
        {
            try
            {
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                _loadLock = new ArrayList();
                _loaded = false;

                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                if (!EventLog.SourceExists("MIDStoreService"))
                {
                    EventLog.CreateEventSource("MIDStoreService", null);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService: StoreServerGlobal encountered error - " + ex.Message);
            }
        }

        //===========
        // PROPERTIES
        //===========
   
        static private Audit Audit
        {
            get
            {
                return _audit;
            }
        }

        static public bool Loaded
        {
            get
            {
                return _loaded;
            }
        }


        //========
        // METHODS
        //========
        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        static public void CloseAudit()
        {
            try
            {
                // Begin TT#1303 - stodd - null ref
                if (Audit != null)
                {
                    Audit.CloseUpdateConnection();
                }
                // End TT#1303 - stodd - null ref
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

        static public void BuildStoreServerGlobalArea()
        {
            try
            {
                AcquireCompleteWriterLock();
                try
                {
                    StoreData _storeData = new StoreData();
                    _dtAllStores = _storeData.StoreProfile_Read();

                    PopulateStores();

                    PopulateAllStoreList();
                    CalculateStatusForStores();
                    _allStoreList.ArrayList.Sort(new StoreTextSort());
                }
                finally
                {
                    ReleaseCompleteWriterLock();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

      

        static public void SetPostingDate(DateTime postingDate)
        {
            try
            {
                if (Calendar.PostDate == null ||
                    postingDate.Date != Calendar.PostDate.Date.Date)
                {

                    calendar_rwl.AcquireWriterLock(WriterLockTimeOut);
                    try
                    {
                        Calendar.SetPostingDate(postingDate);
                    }
                    finally
                    {
                        calendar_rwl.ReleaseWriterLock();
                    }

                    CalculateStatusForStores();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.Message, EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

       

        

        static private void PopulateStores()
        {
            try
            {
                GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                gop.LoadOptions();
                _globalStoreDisplayOption = gop.StoreDisplay;
                _noncompStorePeriodEnd = gop.NonCompStorePeriodEnd;
                _noncompStorePeriodBegin = gop.NonCompStorePeriodBegin;
                _newStorePeriodEnd = gop.NewStorePeriodEnd;
                _newStorePeriodBegin = gop.NewStorePeriodBegin;

                _dsAllStores = MIDEnvironment.CreateDataSet("Stores");
                _dsAllStores.Tables.Add(_dtAllStores);

                //make Store RID column the primary key
                DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                PrimaryKeyColumn[0] = _dtAllStores.Columns["ST_RID"];
                _dtAllStores.PrimaryKey = PrimaryKeyColumn;

                // for insert purposes
                _dtAllStores.Columns["ST_RID"].AllowDBNull = true;
                _dtAllStores.Columns["ACTIVE_IND"].DefaultValue = "1";
                _dtAllStores.Columns["SHIP_ON_MONDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SHIP_ON_TUESDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SHIP_ON_WEDNESDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SHIP_ON_THURSDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SHIP_ON_FRIDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SHIP_ON_SATURDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SHIP_ON_SUNDAY"].DefaultValue = "0";
                _dtAllStores.Columns["SIMILAR_STORE_MODEL"].DefaultValue = "0";  // Issue 3557 stodd

                DataColumn colStoreStatus = new DataColumn();
                colStoreStatus.AllowDBNull = true;
                colStoreStatus.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreStatus);
                colStoreStatus.ColumnName = "Store Status";
                colStoreStatus.DefaultValue = null;
                colStoreStatus.ReadOnly = false;
                colStoreStatus.DataType = System.Type.GetType("System.Single");
                _dtAllStores.Columns.Add(colStoreStatus);
                _dsAllStores.AcceptChanges();

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }


        static private void PopulateAllStoreList()
        {
            try
            {
                if (_allStoreList == null)
                {
                    _allStoreList = new ProfileList(eProfileType.Store);
                }
                else
                {
                    _allStoreList.Clear();
                }

               

                try
                {
                    foreach (DataRow storeDataRow in _dtAllStores.Rows)
                    {
                        // unload dataTable row to a StoreProfile
                        StoreProfile currStore = ConvertToStoreProfile(storeDataRow);
                        _allStoreList.Add(currStore);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        private static StoreProfile ConvertToStoreProfile(DataRow dr)
        {
            try
            {
                int key = -1;

                if (dr["ST_RID"] != DBNull.Value)
                    key = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);

                StoreProfile sp = new StoreProfile(key);
                sp.LoadFieldsFromDataRow(dr);
                

                // Get status
                // BEGIN TT#190 - MD - stodd - store service looping
                //WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
                WeekProfile currentWeek = Calendar.CurrentWeek;
                // END TT#190 - MD - stodd - store service looping
                sp.SetStatus(GetStoreStatus(currentWeek, sp.SellingOpenDt, sp.SellingCloseDt));
                sp.SetStockStatus(GetStoreStatus(currentWeek, sp.StockOpenDt, sp.StockCloseDt));

                sp.SetText(Include.GetStoreDisplay(_globalStoreDisplayOption, sp.StoreId, sp.StoreName, sp.StoreDescription));

               

                return sp;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        

        static public void CalculateStatusForStores()
        {
            try
            {
                stores_rwl.AcquireWriterLock(WriterLockTimeOut);

                try
                {
                    GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                    gop.LoadOptions();
                    _globalStoreDisplayOption = gop.StoreDisplay;
                    _noncompStorePeriodEnd = gop.NonCompStorePeriodEnd;
                    _noncompStorePeriodBegin = gop.NonCompStorePeriodBegin;
                    _newStorePeriodEnd = gop.NewStorePeriodEnd;
                    _newStorePeriodBegin = gop.NewStorePeriodBegin;

                    // recalc store comp/non-comp values
                    DateTime sellingOpenDt;
                    DateTime sellingCloseDt;
                    DateTime stockOpenDt;
                    DateTime stockCloseDt;
                    int storeRID = 0;

                    //**************************************
                    // insert new COMP value for each store
                    //**************************************
                    foreach (DataRow dr in _dtAllStores.Rows)
                    {
                        storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
                        //ArrayList storeCharList = (ArrayList)_allStoreCharacteristicsHash[sp.Key];

                        if (dr["SELLING_OPEN_DATE"] == DBNull.Value)
                        {
                            sellingOpenDt = Include.UndefinedDate;
                        }
                        else
                        {
                            sellingOpenDt = (DateTime)dr["SELLING_OPEN_DATE"];
                        }
                        if (dr["SELLING_CLOSE_DATE"] == DBNull.Value)
                        {
                            sellingCloseDt = Include.UndefinedDate;
                        }
                        else
                        {
                            sellingCloseDt = (DateTime)dr["SELLING_CLOSE_DATE"];
                        }

                        if (dr["STOCK_OPEN_DATE"] == DBNull.Value)
                        {
                            stockOpenDt = Include.UndefinedDate;
                        }
                        else
                        {
                            stockOpenDt = (DateTime)dr["STOCK_OPEN_DATE"];
                        }
                        if (dr["STOCK_CLOSE_DATE"] == DBNull.Value)
                        {
                            stockCloseDt = Include.UndefinedDate;
                        }
                        else
                        {
                            stockCloseDt = (DateTime)dr["STOCK_CLOSE_DATE"];
                        }

                        bool storeDone = false;
                        // BEGIN TT#190 - MD - stodd - store service looping
                        //WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
                        WeekProfile currentWeek = Calendar.CurrentWeek;
                        // END TT#190 - MD - stodd - store service looping
                        if (!storeDone)
                        {
                            eStoreStatus storeStatus = GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
                            sp.SetStatus(storeStatus);
                            sp.SetStockStatus(GetStoreStatus(currentWeek, stockOpenDt, stockCloseDt));
                            DataRow storeRow;
                            switch (storeStatus)
                            {
                                case eStoreStatus.Closed:
                                    // update Data Table
                                    storeRow = _dtAllStores.Rows.Find(storeRID);
                                    if (storeRow != null)
                                    {
                                        storeRow["Store Status"] = eStoreStatus.Closed;
                                    }
                                    break;
                                case eStoreStatus.Preopen:
                                    storeRow = _dtAllStores.Rows.Find(storeRID);
                                    if (storeRow != null)
                                    {
                                        storeRow["Store Status"] = eStoreStatus.Preopen;
                                    }
                                    break;
                                case eStoreStatus.New:
                                    storeRow = _dtAllStores.Rows.Find(storeRID);
                                    if (storeRow != null)
                                    {
                                        storeRow["Store Status"] = eStoreStatus.New;
                                    }
                                    break;
                                case eStoreStatus.NonComp:
                                    storeRow = _dtAllStores.Rows.Find(storeRID);
                                    if (storeRow != null)
                                    {
                                        storeRow["Store Status"] = eStoreStatus.NonComp;
                                    }
                                    break;
                                case eStoreStatus.Comp:
                                    storeRow = _dtAllStores.Rows.Find(storeRID);
                                    if (storeRow != null)
                                    {
                                        storeRow["Store Status"] = eStoreStatus.Comp;
                                    }
                                    break;
                            }
                        }
                    }
                }
                finally
                {
                    _dtAllStores.AcceptChanges();
                    stores_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

    

        public static eStoreStatus GetStoreStatusForCurrentWeek(DateTime sellingOpenDt, DateTime sellingCloseDt)
        {
            try
            {
                WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
                return GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        static public eStoreStatus GetStoreStatus(WeekProfile baseWeek, DateTime sellingOpenDt, DateTime sellingCloseDt)
        {
            try
            {
                eStoreStatus storeStatus = eStoreStatus.Comp;

                bool storeDone = false;
                WeekProfile storeOpenDate = null;
                WeekProfile storeCloseDate = null;
                if (sellingOpenDt != Include.UndefinedDate)
                {
                    storeOpenDate = Calendar.GetWeek(sellingOpenDt);
                }

                if (sellingCloseDt != Include.UndefinedDate)
                {
                    storeCloseDate = Calendar.GetWeek(sellingCloseDt);
                    if (baseWeek.Key > storeCloseDate.Key)
                    {
                        storeStatus = eStoreStatus.Closed;
                        storeDone = true;
                    }
                }


                if (!storeDone)
                {

                    if (sellingOpenDt == Include.UndefinedDate)
                    {
                        storeStatus = eStoreStatus.Comp;
                    }
                    else
                    {
                        //WeekProfile storeOpenDate = Calendar.GetWeek(sp.SellingOpenDt);	

                        //********************************************
                        // convert weeks time frames to Week Profiles
                        //********************************************
                        WeekProfile noncompEnd = null;
                        if (_noncompStorePeriodEnd != Include.UndefinedNonCompStorePeriodEnd)
                            noncompEnd = Calendar.Add(storeOpenDate, _noncompStorePeriodEnd);

                        WeekProfile noncompBegin = null;
                        if (_noncompStorePeriodBegin != Include.UndefinedNonCompStorePeriodBegin)
                            noncompBegin = Calendar.Add(storeOpenDate, _noncompStorePeriodBegin);

                        WeekProfile newEnd = null;
                        if (_newStorePeriodEnd != Include.UndefinedNewStorePeriodEnd)
                            newEnd = Calendar.Add(storeOpenDate, _newStorePeriodEnd);

                        WeekProfile newBegin = null;
                        if (_newStorePeriodBegin != Include.UndefinedNewStorePeriodBegin)
                            newBegin = Calendar.Add(storeOpenDate, _newStorePeriodBegin);

                        //*****************************************************************************
                        // If the New Store Begin Date is filled in and the Non-COmp End Date
                        // is filled in, we check to see if the New Store End Date and the Non-COmp
                        // Store Begin date is filled in.  If they aren't we must do some substitution.
                        //*****************************************************************************
                        if (newBegin != null && noncompEnd != null)
                        {
                            if (newEnd == null && noncompBegin == null)
                            {
                                newEnd = storeOpenDate;
                                noncompBegin = storeOpenDate;
                            }
                            else if (newEnd == null && noncompBegin != null)
                            {
                                newEnd = noncompBegin;
                            }
                            else if (newEnd != null && noncompBegin == null)
                            {
                                noncompBegin = newEnd;
                            }

                        }

                        //********************************************
                        // if there is a non-comp end date, but
                        // still no non-comp begin date...
                        //********************************************
                        if (noncompBegin == null && noncompEnd != null)
                        {
                            noncompBegin = storeOpenDate;
                        }

                        //********************************************
                        // if there is a new begin date, but
                        // still no new end date...
                        //********************************************
                        if (newEnd == null && newBegin != null)
                        {
                            newEnd = storeOpenDate;
                        }

                        // START NEW/NON-COMP/COMP/PREOPEN store calculation
                        //******************************************************
                        // If current week falls prior to opening date... 
                        //******************************************************
                        if (newBegin != null)
                        {
                            if (newBegin.Key > baseWeek.Key)
                            {
                                storeStatus = eStoreStatus.Preopen;
                                storeDone = true;
                            }
                        }

                        //******************************************************
                        // If current week falls within the NEW store range... 
                        //******************************************************
                        if (!storeDone)
                        {
                            if (newBegin != null)
                            {
                                if (newBegin.Key <= baseWeek.Key &&
                                    newEnd.Key >= baseWeek.Key)
                                {
                                    storeStatus = eStoreStatus.New;
                                    storeDone = true;
                                }
                            }
                        }

                        //******************************************************
                        // If current week falls within the NON-COMP store range... 
                        //******************************************************
                        if (!storeDone)
                        {
                            if (noncompEnd != null)
                            {
                                if (noncompBegin.Key <= baseWeek.Key &&
                                    noncompEnd.Key >= baseWeek.Key)
                                {
                                    storeStatus = eStoreStatus.NonComp;
                                    storeDone = true;
                                }
                            }
                        }

                        //******************************************************
                        // If the store didn't fall into either of the other ranges,
                        // it must be COMP
                        //******************************************************
                        if (!storeDone)
                        {
                            storeStatus = eStoreStatus.Comp;
                        }
                    }
                }

                return storeStatus;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }



        static public void AcquireCompleteWriterLock()
        {
            try
            {
                AcquireStoreWriterLock();
                //AcquireStoreGroupWriterLock();
                //AcquireStoreCharacteristicsWriterLock();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        static public void ReleaseCompleteWriterLock()
        {
            try
            {
                ReleaseStoreWriterLock();
                //ReleaseStoreGroupWriterLock();
                //ReleaseStoreCharacteristicsWriterLock();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }


        static public void AcquireStoreWriterLock()
        {
            try
            {
                stores_rwl.AcquireWriterLock(WriterLockTimeOut);
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreWriterLock writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreWriterLock writer lock has timed out");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }


        static public void AcquireStoreReaderLock()
        {
            try
            {
                stores_rwl.AcquireReaderLock(ReaderLockTimeOut);
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreReaderLock reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreReaderLock reader lock has timed out");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }


        static public void ReleaseStoreWriterLock()
        {
            try
            {
                stores_rwl.ReleaseWriterLock();
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreWriterLock writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseStoreWriterLock writer lock has timed out");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

        

      

      


    

        //===========================================================
        // Keeping thisd in here to help furture locking debugging
        //===========================================================
        #region alternate locking for debugging
        //static public void AcquireCompleteWriterLock(string who)
        //{
        //    try
        //    {
        //        AcquireStoreWriterLock(who);
        //        AcquireStoreGroupWriterLock(who);
        //        //AcquireStoreCharacteristicsWriterLock(who);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}

        //static public void ReleaseCompleteWriterLock(string who)
        //{
        //    try
        //    {
        //        ReleaseStoreWriterLock(who);
        //        ReleaseStoreGroupWriterLock(who);
        //        //ReleaseStoreCharacteristicsWriterLock(who);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}

        //static public void AcquireStoreWriterLock(string who)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Acquire Store Lock --- " + who);
        //        stores_rwl.AcquireWriterLock(WriterLockTimeOut);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}
        //static public void ReleaseStoreWriterLock(string who)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Release Store Lock --- " + who);
        //        stores_rwl.ReleaseWriterLock();
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}

        //static public void AcquireStoreGroupWriterLock(string who)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Acquire Store Group Lock --- " + who);
        //        storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}
        //static public void ReleaseStoreGroupWriterLock(string who)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Release Store Group Lock --- " + who);
        //        storeGroup_rwl.ReleaseWriterLock();
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}

        //static public void AcquireStoreCharacteristicsWriterLock(string who)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Acquire Store Char Lock --- " + who);
        //        storeCharacteristics_rwl.AcquireWriterLock(WriterLockTimeOut);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}
        //static public void ReleaseStoreCharacteristicsWriterLock(string who)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Release Store CHar Lock --- " + who);
        //        storeCharacteristics_rwl.ReleaseWriterLock();
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex, _module);
        //        }
        //        throw;
        //    }
        //}
        #endregion


        //Begin TT#1517-MD -jsobek -Store Service Optimization
        public static int GetReaderLockTimeOut()
        {
            return ReaderLockTimeOut;
        }
        public static int GetWriterLockTimeOut()
        {
            return WriterLockTimeOut;
        }
        //End TT#1517-MD -jsobek -Store Service Optimization

        static internal ProfileList GetAllStoresList()
        {
            try
            {
                stores_rwl.AcquireReaderLock(ReaderLockTimeOut);
                try
                {
                    return (ProfileList)_allStoreList.Clone();
                }
                finally
                {
                    // Ensure that the lock is released.
                    stores_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The reader lock request timed out.
                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetAllStoresList reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetAllStoresList reader lock has timed out");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

     

    

      

        public static void AddStoreProfileToList(StoreProfile sp)
        {
            try
            {
                AcquireStoreWriterLock();
                try
                {

                    _allStoreList.Add(sp);
                    // Begin TT#5703 - JSmith - IndexOutOfRange error updating new store
                    DataRow dr = _dtAllStores.NewRow();
                    sp.LoadDataRowFromFields(ref dr);
                    _dtAllStores.Rows.Add(dr);
                    // End TT#5703 - JSmith - IndexOutOfRange error updating new store
                    StoreMgmt.StoreProfile_AddInStoreMgmt(sp);  // TT#5800 - JSmith - Store Load API reverting VSW characteristic back to value prior to a manual change
                }
                finally
                {
                    ReleaseStoreWriterLock();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

      

        public static void UpdateStoreProfileInList(StoreProfile sp)
        {
            try
            {
                AcquireStoreWriterLock();
                try
                {
                    _allStoreList.Remove(sp);
                    _allStoreList.Add(sp);
                    // Begin TT#2095-MD - JSmith - Discrepancy in Store Status
                    DataRow[] dr = _dtAllStores.Select("ST_RID=" + sp.Key);
                    if (dr != null)
                    {
                        sp.LoadDataRowFromFields(ref dr[0]);
                    }
                    // End TT#2095-MD - JSmith - Discrepancy in Store Status
                    StoreMgmt.StoreProfile_UpdateInStoreMgmt(sp);  // TT#5800 - JSmith - Store Load API reverting VSW characteristic back to value prior to a manual change
                }
                finally
                {
                    ReleaseStoreWriterLock();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }


        static internal void RefreshStoreText()
        {
            try
            {
                GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                gop.LoadOptions();
                _globalStoreDisplayOption = gop.StoreDisplay;

                foreach (StoreProfile sp in _allStoreList)
                {
                    sp.SetText(Include.GetStoreDisplay(_globalStoreDisplayOption, sp.StoreId, sp.StoreName, sp.StoreDescription));
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

      
 



        /// <summary>
        /// The Load method is called by the service or client to trigger the instantiation of the static StoreServerGlobal
        /// object
        /// </summary>
        static public void Load(bool aLocal)
        {
            try
            {
                try
                {
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //LoadBase(eProcesses.storeService);
                    lock (_loadLock.SyncRoot)
                    {
                        if (!_loaded)
                        {
                            //Begin TT#5320-VStuart-deadlock issues-FinishLine
                            if (!aLocal)
                            {
                                MarkRunningProcesses(eProcesses.storeService);  // TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
                            }
                            //End TT#5320-VStuart-deadlock issues-FinishLine

                            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                            //_audit = new Audit(eProcesses.storeService, Include.AdministratorUserRID);
                            if (!aLocal)
                            {
                                _audit = new Audit(eProcesses.storeService, Include.AdministratorUserRID);
                            }
                            //End TT#189  
                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            int messagingInterval = Include.Undefined;
                            object parm = MIDConfigurationManager.AppSettings["MessagingInterval"];
                            if (parm != null)
                            {
                                messagingInterval = Convert.ToInt32(parm);
                            }
                            //LoadBase();
                            LoadBase(eMIDMessageSenderRecepient.storeService, messagingInterval, aLocal, eProcesses.storeService);
                            // End TT#2307;

                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            EventLog.WriteEntry("MIDStoreService", "MIDStoreService begin building store information", EventLogEntryType.Information);

                            // create reader locks
                            //stores_rwl.AcquireWriterLock(WriterLockTimeOut);

                            //===============================================
                            // get posting date
                            //===============================================
                            DateTime postingDate = DateTime.Now;
                            MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                            DataTable dt = mhd.PostingDate_Read();
                            if (dt.Rows.Count == 1)
                            {
                                DataRow dr = dt.Rows[0];
                                if (dr["POSTING_DATE"] != DBNull.Value)
                                {
                                    postingDate = Convert.ToDateTime(dr["POSTING_DATE"], CultureInfo.CurrentUICulture);
                                }
                            }
                            Calendar.SetPostingDate(postingDate);

                            //=========================
                            // Build Global Area
                            //=========================
                            BuildStoreServerGlobalArea();

                            // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
                            //StoreServerGlobal.RefreshSortOfGroupLevels();
                            // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors

                            SetInitialMemoryCounts();
                            EventLog.WriteEntry("MIDStoreService", "MIDStoreService completed building store information", EventLogEntryType.Information);
                            //_lastRefreshDate = _createdDate;
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            MessageProcessor.Messaging.OnMessageSentHandler += new MessageEvent.MessageEventHandler(Messaging_OnMessageSentHandler);
                            // End TT#2307

                            // Begin TT#195 MD - JSmith - Add environment authentication
                            if (!aLocal)
                            {
                                RegisterServiceStart();
                            }
                            // End TT#195 MD

                            _loaded = true;
                        }
                    }
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                }
                finally
                {
                    // Ensure that the lock is released.
                    //stores_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static void Messaging_OnMessageSentHandler(object source, MessageEventArgs e)
        {
            try
            {
                switch (e.MessageCode)
                {
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307 

       
        /// <summary>
        /// Cleans up all resources for the service
        /// </summary>
        static public void CleanUp()
        {
            try
            {
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                if (isExecutingLocal &&
                    MessageProcessor.isListeningForMessages)
                {
                    StopMessageListener();
                }
                // End TT#2307

                if (Audit != null)
                {
                    Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", Audit.HighestMessageLevel);
                    // Begin TT#1243 - JSmith - Audit Performance
                    Audit.CloseUpdateConnection();
                    // End TT#1243
                }
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
            }
        }

    }


}
