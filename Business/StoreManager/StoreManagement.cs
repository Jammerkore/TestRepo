using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public static class StoreMgmt
    {
        private const string _module = "StoreManagement.cs";
        private static ProfileList _allStoreList = null;
        private static ProfileList _groupList = null;
        private static bool _bLoadInactiveGroups;     // TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
        private static List<StoreGroupLevelProfile> _levelList = new List<StoreGroupLevelProfile>();
        private static int ReaderLockTimeOut;
        private static int WriterLockTimeOut;
        //private static MIDReaderWriterLock stores_rwl = new MIDReaderWriterLock();
        //private static MIDReaderWriterLock storeGroup_rwl = new MIDReaderWriterLock();
        private static MIDReaderWriterLock StoreMgmt_rwl = new MIDReaderWriterLock();
        private static Audit _audit = null;
        private static SessionAddressBlock _SAB = null;
        private static Session _session;   // TT#1861-MD - JSmith - Serialization error accessing the Audit
        private static int _userID = -1;
        private static GlobalOptionsProfile _gop;
        private static List<int> _usersAssignedToMe;   // TT#5664 - JSmith - All User Store Attributes appear in User Methods 
        // Begin TT#2090-MD - JSmith - Performance
        private static Dictionary<int, Dictionary<int, eStoreStatus>> _storeSalesStatusByWeek = new Dictionary<int,Dictionary<int,eStoreStatus>>();
        private static Dictionary<int, Dictionary<int, eStoreStatus>> _storeStockStatusByWeek = new Dictionary<int, Dictionary<int, eStoreStatus>>();
        // End TT#2090-MD - JSmith - Performance

        public static int UserID
        {
            get
            {
                if (_userID == -1)
                {
                    _userID = _SAB.ClientServerSession.UserRID;
                    BuildUsersAssignedToMe();  // TT#5664 - JSmith - All User Store Attributes appear in User Methods 
                }
                return _userID;
            }
        }
        public static int ReserveStoreRID
        {
            get
            {
                return _gop.ReserveStoreRID;
            }
        }

        // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
        public static bool StoresAdded
        {
            get
            {
                return _allStoreList.Count != _SAB.StoreServerSession.GetAllStoresListCount();
            }
        }

        public static bool StoreGroupsAdded
        {
            get
            {
                StoreGroupMaint groupData = new StoreGroupMaint();
                return _groupList.Count != groupData.StoreGroup_Count(_bLoadInactiveGroups);
            }
        }
        // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail

        // Begin TT#5664 - JSmith - All User Store Attributes appear in User Methods 
        public static void BuildUsersAssignedToMe()
        {
            try
            {
                AcquireWriterLock();
                _usersAssignedToMe = new List<int>();
                SecurityAdmin dlSecurity = new SecurityAdmin();
                DataTable dtUsers = dlSecurity.GetUsersAssignedToMe(_userID);

                foreach (DataRow drUsers in dtUsers.Rows)
                {
                    _usersAssignedToMe.Add(Convert.ToInt32(drUsers["OWNER_USER_RID"]));
                }
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseWriterLock();
            }
        }
        // End TT#5664 - JSmith - All User Store Attributes appear in User Methods 

        // Begin TT#1861-MD - JSmith - Serialization error accessing the Audit
        //public static void LoadInitialStoresAndGroups(SessionAddressBlock SAB)
        // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
        //public static void LoadInitialStoresAndGroups(SessionAddressBlock SAB, Session session)
        public static void LoadInitialStoresAndGroups(SessionAddressBlock SAB, Session session, bool bLoadInactiveGroups = false, bool bDoingRefresh = false)
        // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
        // End TT#1861-MD - JSmith - Serialization error accessing the Audit
        {
            // Begin TT#5662 - JSmith - Object Reference error in Store Session Initialize
            AcquireWriterLock();
            try
            {
                // area already initialized
                if (_SAB != null
                    && !bDoingRefresh)
                {
                    return;
                }
                // End TT#5662 - JSmith - Object Reference error in Store Session Initialize

                _SAB = SAB;
                // Begin TT#1861-MD - JSmith - Serialization error accessing the Audit
                _session = session;
                //if (SAB.RemoteServices)
                //{
                //    _audit = new Audit(eProcesses.storeService, Include.AdministratorUserRID);  //TO DO - use a different process than store service
                //}
                _audit = _session.Audit;
                // End TT#1861-MD - JSmith - Serialization error accessing the Audit
                _allStoreList = SAB.StoreServerSession.GetAllStoresList();
                ReaderLockTimeOut = SAB.StoreServerSession.GetReaderLockTimeOut();
                WriterLockTimeOut = SAB.StoreServerSession.GetWriterLockTimeOut();

                _gop = new GlobalOptionsProfile(-1);
                _gop.LoadOptions();
                //_globalStoreDisplayOption = _gop.StoreDisplay;

                // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                //StoreGroups_Populate();
                StoreGroups_Populate(bLoadInactiveGroups);
                // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

                // Begin TT#2090-MD - JSmith - Performance
                _storeSalesStatusByWeek = new Dictionary<int, Dictionary<int, eStoreStatus>>();
                _storeStockStatusByWeek = new Dictionary<int, Dictionary<int, eStoreStatus>>();
                // End TT#2090-MD - JSmith - Performance
            }
            // Begin TT#5662 - JSmith - Object Reference error in Store Session Initialize
            finally
            {
                // Ensure that the lock is released.
                ReleaseWriterLock();
            }
            // End TT#5662 - JSmith - Object Reference error in Store Session Initialize
        }

        #region "Audit Messages"

        public enum AuditCodesForStoreMgmt
        {
            StoreProfile = 0,
            StaticStoreGroup = 1,
            DynamicStoreGroup = 2,
            StoreCharacteristic = 3,
            HeaderCharacteristic = 4
        }
        public static void AuditMessage_Add(AuditCodesForStoreMgmt auditCode, string msg)
        {
            eMIDTextCode textCode = eMIDTextCode.msg_StoreProfileChangeAuditLevel;
            string reportingModule = string.Empty;
            if (auditCode == AuditCodesForStoreMgmt.StoreProfile)
            {
                textCode = eMIDTextCode.msg_StoreProfileChangeAuditLevel;
                reportingModule = "Store Profile";
            }
            else if (auditCode == AuditCodesForStoreMgmt.StaticStoreGroup)
            {
                textCode = eMIDTextCode.msg_StaticStoreGroupChangeAuditLevel;
                reportingModule = "Store Attribute";
            }
            else if (auditCode == AuditCodesForStoreMgmt.DynamicStoreGroup)
            {
                textCode = eMIDTextCode.msg_DynamicStoreGroupChangeAuditLevel;
                reportingModule = "Store Dynamic Attribute";
            }
            else if (auditCode == AuditCodesForStoreMgmt.StoreCharacteristic)
            {
                textCode = eMIDTextCode.msg_StoreCharacteristicChangeAuditLevel;
                reportingModule = "Store Characteristic";
            }
            else if (auditCode == AuditCodesForStoreMgmt.HeaderCharacteristic)
            {
                textCode = eMIDTextCode.msg_HeaderCharacteristicChangeAuditLevel;
                reportingModule = "Header Characteristic";
            }
            else
            {
                return;
            }
            //string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreProfileChangeAuditLevel, false, groupName);
            // Begin TT#1861-MD - JSmith - Serialization error accession the Audit
            //eMIDMessageLevel messageLevel = _SAB.ClientServerSession.Audit.GetMessageLevel(textCode);
            //_SAB.ClientServerSession.Audit.Add_Msg(messageLevel, msg, reportingModule, true);
            eMIDMessageLevel messageLevel = _session.Audit.GetMessageLevel(textCode);
            _session.Audit.Add_Msg(messageLevel, msg, reportingModule, true);
            // End TT#1861-MD - JSmith - Serialization error accession the Audit
        }

        #endregion


        #region "Store Profiles"
        public static ProfileList StoreProfiles_GetActiveStoresList()
        {
            try
            {
                AcquireReaderLock();
                ProfileList activeList = new ProfileList(eProfileType.Store);
                foreach (StoreProfile sp in _allStoreList.ArrayList)
                {
                    if (sp.ActiveInd)
                    {
                        activeList.Add(sp);
                    }
                }
                activeList.ArrayList.Sort();  // TT#1853-MD - JSmith - Create Str Attribute - When Store List is selected the stores are not in the correct order.  Expect the stores to be in Numerical then Alphabetical in the selection list.
                return activeList;
            }
            catch (Exception err)
            {
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseReaderLock();
            }
        }

        // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
        //public static void StoreProfiles_RefreshFromService()
        public static void StoreProfiles_RefreshFromService(bool refreshSessions)
        // End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {
            try
            {
                AcquireWriterLock();
                _allStoreList = _SAB.StoreServerSession.GetAllStoresList();
                // Begin TT#1908-MD - JSmith - Versioning Test_ Interface in new Store_ process methods user arobinson_ process methods as user pam _ receive system argument exception
                // Refresh sessions since store list refreshed.
                // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
                if (refreshSessions)
                {
                    // End TT#2078-MD - JSmith - Object Reference error updating Store Group
                    if (_SAB.ClientServerSession != null)
                    {
                        _SAB.ClientServerSession.Refresh();
                    }
                    if (_SAB.ApplicationServerSession != null)
                    {
                        _SAB.ApplicationServerSession.Refresh();
                    }
                    if (_SAB.HierarchyServerSession != null)
                    {
                        _SAB.HierarchyServerSession.Refresh();
                    }
                    if (_SAB.HeaderServerSession != null)
                    {
                        _SAB.HeaderServerSession.Refresh();
                    }
                }  // TT#2078-MD - JSmith - Object Reference error updating Store Group
                   // End TT#1908-MD - JSmith - Versioning Test_ Interface in new Store_ process methods user arobinson_ process methods as user pam _ receive system argument exception
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseWriterLock();
            }
        }

        public static int StoreProfile_GetStoreRidFromId(string storeId)
        {
            try
            {
                //Changed for removal of store datatable - stodd 2.11.2007
                StoreProfile sp = StoreProfile_Get(storeId);
                return sp.Key;
            }
            catch (Exception err)
            {
                string msg = "GetStoreRID(): " + err.ToString();
                throw;
            }
        }

        /// <summary>
        /// will return StoreProfile with the StoreRID = -1, if the store was not found
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static StoreProfile StoreProfile_Get(string storeId)
        {
            try
            {
                AcquireReaderLock();
                //Changed for removal of store datatable - stodd 2.11.2007
                StoreProfile sp = null;

                int storeCnt = _allStoreList.Count;
                for (int i = 0; i < storeCnt; i++)
                {
                    StoreProfile tempStore = (StoreProfile)_allStoreList[i];
                    if (tempStore.StoreId == storeId)
                    {
                        sp = tempStore;
                        break;
                    }
                }

                if (sp == null)
                {
                    sp = new StoreProfile(Include.UndefinedStoreRID);
                    sp.SetFieldsToUnknownStore();
                }

                return sp;
            }
            catch (Exception err)
            {
                string msg = "GetStoreProfile(): " + err.ToString();

                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseReaderLock();
            }
        }

        /// <summary>
        /// will return StoreProfile with the StoreRID = -1, if the store was not found
        /// </summary>
        /// <param name="storeRecId"></param>
        /// <returns></returns>
        public static StoreProfile StoreProfile_Get(int storeRecId)
        {
            try
            {
                AcquireReaderLock();
                StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRecId);
                if (sp == null)
                {
                    sp = new StoreProfile(Include.UndefinedStoreRID);
                    sp.SetFieldsToUnknownStore();
                }
                return sp;
            }
            catch (Exception err)
            {
                string msg = "GetStoreProfile(): " + err.ToString();

                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseReaderLock();
            }
        }

        // Begin TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
        //public static int StoreGroup_GetRidFromId(string groupId)
        public static int StoreGroup_GetRidFromId(string groupId, int ownerUserRID = Include.NoRID)
        // End TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
        {
            try
            {
                AcquireReaderLock();
                //Changed for removal of store datatable - stodd 2.11.2007
                //StoreGroupProfile sgp = GetStoreGroupProfile(storeId);
                //Changed for removal of store datatable - stodd 2.11.2007
                StoreGroupProfile sgp = null;

                int storeGrpCnt = _groupList.Count;
                for (int i = 0; i < storeGrpCnt; i++)
                {
                    // BEGIN -- discovered error while testing issue 4000
                    StoreGroupProfile tempStoreGroup = (StoreGroupProfile)_groupList[i];
                    // Begin TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
                    //if (tempStoreGroup.GroupId == groupId)
                    if (tempStoreGroup.GroupId == groupId
                        && (ownerUserRID == Include.NoRID || tempStoreGroup.OwnerUserRID == ownerUserRID))
                    // End TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
                    {
                        sgp = tempStoreGroup;
                        break;
                    }
                    // END --
                }

                if (sgp == null)
                {
                    sgp = new StoreGroupProfile(Include.UndefinedStoreRID);
                    sgp.GroupId = "UNKNOWN";
                    sgp.Name = "UNKNOWN STORE";
                }


                return sgp.Key;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupRID(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseReaderLock();
            }
        }

        /// <summary>
        /// returns a hashtable keyed by Store ID.  The value is the store's KEY/RID
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetStoreIDHash()
        {
            try
            {
                AcquireReaderLock();
                Hashtable storeHash = new Hashtable();

                foreach (StoreProfile sp in _allStoreList.ArrayList)
                {
                    storeHash.Add(sp.StoreId, sp.Key);
                }

                return storeHash;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreIDHash(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseReaderLock();
            }
        }
        public static string GetStoreDisplayText(int storeRid)
        {
            try
            {
                StoreProfile sp = StoreProfile_Get(storeRid);
                return sp.Text;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AcquireReaderLock()
        {
            try
            {
                StoreMgmt_rwl.AcquireReaderLock(ReaderLockTimeOut);
            }
            catch (ApplicationException ex)
            {
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw new MIDException(eErrorLevel.severe, 0, "StoreManagement:AcquireReaderLock writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }
        private static void ReleaseReaderLock()
        {
            try
            {
                StoreMgmt_rwl.ReleaseReaderLock();
            }
            catch (ApplicationException ex)
            {
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw new MIDException(eErrorLevel.severe, 0, "StoreManagement:ReleaseReaderLock writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

        private static void AcquireWriterLock()
        {
            try
            {
                StoreMgmt_rwl.AcquireWriterLock(WriterLockTimeOut);
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The writer lock request timed out.
                //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireWriterLock writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "StoreManagement:AcquireWriterLock writer lock has timed out");
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }
        private static void ReleaseWriterLock()
        {
            try
            {
                StoreMgmt_rwl.ReleaseWriterLock();
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The writer lock request timed out.
                //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseWriterLock writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "StoreManagement:ReleaseWriterLock writer lock has timed out");
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }




        public static int StoreProfile_Add(StoreProfile store, List<storeCharInfo> charList, ProgressBarOptions progressbarOptions, bool refreshStoreGroups = false)
        {
            try
            {
                AcquireWriterLock();
                //AcquireStoreGroupWriterLock();



                //add to database

                StoreMaint _storeData = new StoreMaint();
                _storeData.OpenUpdateConnection();
                int storeRid = _storeData.StoreProfile_Insert(store);
                _storeData.CommitData();
                _storeData.CloseUpdateConnection();

                store.Key = storeRid;


                foreach (storeCharInfo charInfo in charList)
                {
                    int scRID = Include.NoRID;
                    if (charInfo.action == storeCharInfoAction.InsertValue) //non list values
                    {
                        int scRidDuplicate = Include.NoRID;
                        if (StoreMgmt.DoesStoreCharValueAlreadyExist(charInfo.anyValue, charInfo.dataType, charInfo.scgRID, Include.NoRID, ref scRidDuplicate) == false)
                        {
                            //insert characteristic value                          
                            string stringVal = Include.NullForStringValue;
                            DateTime? dateVal = null;
                            float? numericVal = null;
                            float? dollarVal = null;
                            if (charInfo.dataType == fieldDataTypes.DateNoTime)
                            {
                                dateVal = Convert.ToDateTime(charInfo.anyValue);
                            }
                            else if (charInfo.dataType == fieldDataTypes.NumericDouble)
                            {
                                numericVal = Convert.ToSingle(charInfo.anyValue);
                            }
                            else if (charInfo.dataType == fieldDataTypes.NumericDollar)
                            {
                                dollarVal = Convert.ToSingle(charInfo.anyValue);
                            }
                            else
                            {
                                stringVal = charInfo.anyValue;
                            }
                            scRID = StoreCharValue_Insert(charInfo.scgRID, stringVal, dateVal, numericVal, dollarVal);
                        }
                        else
                        {
                            scRID = scRidDuplicate;
                        }

                    }
                    else if (charInfo.action == storeCharInfoAction.UseValue)
                    {
                        scRID = charInfo.scRID;
                    }

                    if (charInfo.action != storeCharInfoAction.Skip)
                    {
                        //insert the join for the new store
                        StoreCharMaint storeCharMaint = new StoreCharMaint();
                        storeCharMaint.StoreCharValueJoin_Insert(storeRid, scRID, charInfo.scgRID);
                    }
                }

                //refresh char list
                // StoreValidation.SetCharListValuesLists(); 


                // update store status
                StoreProfile_UpdateStatusAndText(ref store);

                _SAB.StoreServerSession.AddStoreProfileToList(store);


                // add to datatable
                //AddStoreProfileToDataTable(store);

                // Update store Profile in List
                _allStoreList.Add(store);

                //ReleaseWriterLock();
                //ReleaseStoreGroupWriterLock();


                if (store.ActiveInd == true && refreshStoreGroups)
                {
                    StoreGroups_Refresh(progressbarOptions);
                }


                return store.Key;
            }
            catch (Exception err)
            {
                //string msg = "AddStoreProfile(): " + err.ToString();
                ////EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (_audit != null)
                //{
                //    _audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                ReleaseWriterLock();
            }
        }

        // Begin TT#5800 - JSmith - Store Load API reverting VSW characteristic back to value prior to a manual change
        public static int StoreProfile_AddInStoreMgmt(StoreProfile store)
        {
            try
            {
                // update store status
                StoreProfile_UpdateStatusAndText(ref store);

                // Update store Profile in List
                _allStoreList.Add(store);

                return store.Key;
            }
            catch (Exception err)
            {
                throw;
            }
        }
        // End TT#5800 - JSmith - Store Load API reverting VSW characteristic back to value prior to a manual change

        // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
		//public static void StoreGroups_Refresh(ProgressBarOptions progressbarOptions)
		public static void StoreGroups_Refresh(ProgressBarOptions progressbarOptions, bool refreshSessions = true)
		// End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {
            //Load all store group filters
            FilterData filterData = new FilterData();
            DataTable dtStaticGroupFilters = filterData.ReadFiltersForType(filterTypes.StoreGroupFilter);
            DataTable dtDynamicGroupFilters = filterData.ReadFiltersForType(filterTypes.StoreGroupDynamicFilter);
            List<int> filterRidList = new List<int>();
            foreach (DataRow drFilter in dtStaticGroupFilters.Rows)
            {
                filterRidList.Add((int)drFilter["FILTER_RID"]);
            }
            foreach (DataRow drFilter in dtDynamicGroupFilters.Rows)
            {
                filterRidList.Add((int)drFilter["FILTER_RID"]);
            }

            //Execute all store group filters and clear results (new results will be gotton on demand)
			// Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
			//StoreGroups_UpdateFromFilterList(filterRidList, progressbarOptions);
            StoreGroups_UpdateFromFilterList(filterRidList, progressbarOptions, refreshSessions);
			// End TT#2078-MD - JSmith - Object Reference error updating Store Group

        }

        // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
        // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
		//public static void StoreGroups_Refresh(ProgressBarOptions progressbarOptions, List<string> fieldNameList)
		public static void StoreGroups_Refresh(ProgressBarOptions progressbarOptions, List<string> fieldNameList, bool refreshSessions = true)
		// End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {
            //Load all store group filters
            FilterData filterData = new FilterData();
            List<int> filterRidList = new List<int>();
            foreach (string fieldName in fieldNameList)
            {
                DataTable dtStaticGroupFilters = filterData.ReadFiltersForTypeAndFieldName(filterTypes.StoreGroupFilter, fieldName);
                DataTable dtDynamicGroupFilters = filterData.ReadFiltersForTypeAndFieldName(filterTypes.StoreGroupDynamicFilter, fieldName);
                
                foreach (DataRow drFilter in dtStaticGroupFilters.Rows)
                {
                    filterRidList.Add((int)drFilter["FILTER_RID"]);
                }
                foreach (DataRow drFilter in dtDynamicGroupFilters.Rows)
                {
                    filterRidList.Add((int)drFilter["FILTER_RID"]);
                }
            }

            //Execute all store group filters and clear results (new results will be gotton on demand)
			// Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
			//StoreGroups_UpdateFromFilterList(filterRidList, progressbarOptions);
            StoreGroups_UpdateFromFilterList(filterRidList, progressbarOptions, refreshSessions);
			// End TT#2078-MD - JSmith - Object Reference error updating Store Group

        }
        // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields

        public delegate void ProgressBar_UpdateMaxDelegate(StoreMgmt.ProgressBarOptions opt, int newMax);
        public delegate void ProgressBar_UpdateTextDelegate(StoreMgmt.ProgressBarOptions opt, string newText);
        public delegate void ProgressBar_IncrementDelegate(StoreMgmt.ProgressBarOptions opt);
        public delegate void ProgressBar_CloseDelegate(StoreMgmt.ProgressBarOptions opt);
        public class ProgressBarOptions
        {
            public bool useProgressBar = false;
            public ProgressBar_UpdateMaxDelegate progressBarUpdateMax = null;
            public ProgressBar_UpdateTextDelegate progressBarUpdateText = null;
            public ProgressBar_IncrementDelegate progressBarIncrement = null;
            public ProgressBar_CloseDelegate progressBarClose = null;
            public System.Windows.Forms.Form frm;
        }

        // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
		//public static void StoreGroups_RefreshForChangedFieldsAndChars(List<storeCharInfo> charList, List<int> fieldChangedList, ProgressBarOptions progressbarOptions)
		public static void StoreGroups_RefreshForChangedFieldsAndChars(List<storeCharInfo> charList, List<int> fieldChangedList, ProgressBarOptions progressbarOptions, bool refreshSessions = true)
		// End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {
            DataTable dtFieldsChanged = new DataTable();
            dtFieldsChanged.Columns.Add("FIELD_INDEX", typeof(int));
            for (int i = 0; i < fieldChangedList.Count; i++)
            {
                if (dtFieldsChanged.Select("FIELD_INDEX=" + fieldChangedList[i].ToString()).Length == 0)
                {
                    DataRow dr = dtFieldsChanged.NewRow();
                    dr["FIELD_INDEX"] = fieldChangedList[i];
                    dtFieldsChanged.Rows.Add(dr);
                }
            }

            DataTable dtCharsChanged = new DataTable();
            dtCharsChanged.Columns.Add("SCG_RID", typeof(int));
            foreach (storeCharInfo charInfo in charList)
            {
                if (charInfo.isDirty)
                {
                    if (dtCharsChanged.Select("SCG_RID=" + charInfo.scgRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtCharsChanged.NewRow();
                        dr["SCG_RID"] = charInfo.scgRID;
                        dtCharsChanged.Rows.Add(dr);
                    }
                }
            }




            FilterData filterData = new FilterData();
            DataTable dtFilterRids = filterData.ReadFiltersForStoreGroupRefresh(dtFieldsChanged, dtCharsChanged);

            //Execute all store group filters and clear results (new results will be gotton on demand)
			// Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
			//StoreGroups_UpdateFromFilterList(dtFilterRids, progressbarOptions);
            StoreGroups_UpdateFromFilterList(dtFilterRids, progressbarOptions, refreshSessions);
			// End TT#2078-MD - JSmith - Object Reference error updating Store Group
        }

        // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
		//private static void StoreGroups_UpdateFromFilterList(DataTable dtFilterRids, ProgressBarOptions pBarOpt)
		private static void StoreGroups_UpdateFromFilterList(DataTable dtFilterRids, ProgressBarOptions pBarOpt, bool refreshSessions = true)
		// End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {

            if (pBarOpt.useProgressBar)
            {
                pBarOpt.progressBarUpdateMax(pBarOpt, dtFilterRids.Rows.Count);
            }

            foreach (DataRow dr in dtFilterRids.Rows)
            {
                int filterRID = (int)dr["FILTER_RID"];
                filter f = filterDataHelper.LoadExistingFilter(filterRID);
                if (pBarOpt.useProgressBar)
                {
                    pBarOpt.progressBarUpdateText(pBarOpt, "Updating group: " + f.filterName);
                    pBarOpt.progressBarIncrement(pBarOpt);
                }
				// Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
				//StoreGroup_AddOrUpdate(f, isNewGroup: false, loadNewResults: false);
                StoreGroup_AddOrUpdate(f, isNewGroup: false, loadNewResults: false, refreshSessions: refreshSessions);
				// End TT#2078-MD - JSmith - Object Reference error updating Store Group
            }

            if (pBarOpt.useProgressBar)
            {
                pBarOpt.progressBarClose(pBarOpt);
            }
        }
        
		// Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
		//private static void StoreGroups_UpdateFromFilterList(List<int> filterRidList, ProgressBarOptions pBarOpt)
		private static void StoreGroups_UpdateFromFilterList(List<int> filterRidList, ProgressBarOptions pBarOpt, bool refreshSessions = true)
		// End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {

            if (pBarOpt.useProgressBar)
            {
                pBarOpt.progressBarUpdateMax(pBarOpt, filterRidList.Count);
            }

            foreach (int filterRID in filterRidList)
            {
                filter f = filterDataHelper.LoadExistingFilter(filterRID);
                if (pBarOpt.useProgressBar)
                {
                    pBarOpt.progressBarUpdateText(pBarOpt, "Updating group: " + f.filterName);
                    pBarOpt.progressBarIncrement(pBarOpt);
                }
                // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
				//StoreGroup_AddOrUpdate(f, isNewGroup: false, loadNewResults: false);
				StoreGroup_AddOrUpdate(f, isNewGroup: false, loadNewResults: false, refreshSessions: refreshSessions);
				// End TT#2078-MD - JSmith - Object Reference error updating Store Group
            }
            if (pBarOpt.useProgressBar)
            {
                pBarOpt.progressBarClose(pBarOpt);
            }
        }


        public static void StoreGroups_RefreshFiltersForStoreCharGroup(int scgRID, ProgressBarOptions pBarOpt)
        {

            DataTable dtFieldsChanged = new DataTable();
            dtFieldsChanged.Columns.Add("FIELD_INDEX", typeof(int));


            DataTable dtCharsChanged = new DataTable();
            dtCharsChanged.Columns.Add("SCG_RID", typeof(int));


            DataRow drChar = dtCharsChanged.NewRow();
            drChar["SCG_RID"] = scgRID;
            dtCharsChanged.Rows.Add(drChar);




            FilterData filterData = new FilterData();
            DataTable dtFilterRids = filterData.ReadFiltersForStoreGroupRefresh(dtFieldsChanged, dtCharsChanged);

            //Execute all store group filters and clear results (new results will be gotton on demand)
            StoreGroups_UpdateFromFilterList(dtFilterRids, pBarOpt);
        }

        // Begin TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed
        public static DataTable StoreGroup_GetFiltersForStoreCharGroup(int scgRID)
        {

            DataTable dtFieldsChanged = new DataTable();
            dtFieldsChanged.Columns.Add("FIELD_INDEX", typeof(int));


            DataTable dtCharsChanged = new DataTable();
            dtCharsChanged.Columns.Add("SCG_RID", typeof(int));


            DataRow drChar = dtCharsChanged.NewRow();
            drChar["SCG_RID"] = scgRID;
            dtCharsChanged.Rows.Add(drChar);

            FilterData filterData = new FilterData();
            return filterData.ReadFiltersForStoreGroupRefresh(dtFieldsChanged, dtCharsChanged);
        }
        // End TT#1848-MD - JSmith - Store Characteristic Values not checking In Use and are Removed

        public static void StoreGroup_SetDynamicGroupsInactiveForStoreCharGroup(int scgRID)
        {

            DataTable dtFieldsChanged = new DataTable();
            dtFieldsChanged.Columns.Add("FIELD_INDEX", typeof(int));


            DataTable dtCharsChanged = new DataTable();
            dtCharsChanged.Columns.Add("SCG_RID", typeof(int));


            DataRow drChar = dtCharsChanged.NewRow();
            drChar["SCG_RID"] = scgRID;
            dtCharsChanged.Rows.Add(drChar);




            FilterData filterData = new FilterData();
            DataTable dtFilterRids = filterData.ReadFiltersForStoreGroupRefresh(dtFieldsChanged, dtCharsChanged);

            //Set dynamic groups for these filters as inactive
            foreach (DataRow dr in dtFilterRids.Rows)
            {
                int filterRID = (int)dr["FILTER_RID"];
                filter f = filterDataHelper.LoadExistingFilter(filterRID);
                if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    StoreGroupProfile groupProf = StoreGroup_GetFromFilter(filterRID);
                    if (groupProf != null)
                    {
                        StoreGroup_SetInactive(groupProf.Key);
                    }
                }
            }
        }

        public static void StoreProfile_Update(StoreProfile store, bool doRefreshGroups, ref bool doRefreshNodes, ProgressBarOptions progessbarOptions, List<storeCharInfo> charList, List<int> fieldChangedList = null)
        {
            try
            {
                AcquireWriterLock();
                //AcquireStoreGroupWriterLock();
                try
                {
                    //Object[] sa = store.ItemArray();

                    // Get store's current charateristic list
                    //ArrayList currCharList = GetStoreCharacteristicList(store.Key);

                    // loop through store char group changes
                    //foreach (StoreCharGroupProfile scgp in store.Characteristics)
                    //{
                    //    // get new store char RID, if needed
                    //    if (scgp.CharacteristicValue.SC_RID == Include.UndefinedStoreCharRID)
                    //    {
                    //        if (scgp.CharacteristicValue.CharValue != DBNull.Value)
                    //            scgp.CharacteristicValue.SC_RID = AddStoreChar(scgp.Name, scgp.CharacteristicValue.CharValue);
                    //    }
                    //    // BEGIN TT#1401 - stodd - add resevation stores (IMO)
                    //    // remove store char join record for old value
                    //    if (currCharList != null)
                    //    {
                    //        foreach (StoreCharGroupProfile currCharGroup in currCharList)
                    //        {
                    //            if (currCharGroup.Name == scgp.Name)
                    //            {
                    //                // delete old store char join record if necessary
                    //                if (currCharGroup.CharacteristicValue.SC_RID != Include.UndefinedStoreCharRID
                    //                    && currCharGroup.CharacteristicValue.SC_RID != scgp.CharacteristicValue.SC_RID)
                    //                {
                    //                    DeleteStoreCharJoin(store.Key, currCharGroup.CharacteristicValue.SC_RID);
                    //                }
                    //                // if the store char key is still = undefined then the value will be set to null
                    //                if (scgp.CharacteristicValue.SC_RID == Include.UndefinedStoreCharRID)
                    //                {
                    //                    currCharGroup.CharacteristicValue.SC_RID = 0;
                    //                    currCharGroup.CharacteristicValue.CharValue = DBNull.Value;
                    //                }
                    //                else if (currCharGroup.CharacteristicValue.SC_RID != scgp.CharacteristicValue.SC_RID)
                    //                {
                    //                    AddStoreCharJoin(store.Key, scgp.CharacteristicValue.SC_RID);
                    //                    //UpdateCharacteristicValueInAStore(store.Key, scgp.Name, newValue);
                    //                    currCharGroup.CharacteristicValue.SC_RID = scgp.CharacteristicValue.SC_RID;
                    //                    currCharGroup.CharacteristicValue.CharValue = scgp.CharacteristicValue.CharValue;
                    //                }

                    //                break;
                    //            }
                    //        }
                    //    }
                    //    // END TT#1401 - stodd - add resevation stores (IMO)
                    //    // Begin TT#614-MD - JSmith - Adding new stores with new characteristics fails to added characteristic values to all stores.
                    //    else
                    //    {
                    //        AddStoreCharJoin(store.Key, scgp.CharacteristicValue.SC_RID);
                    //    }
                    //    // End TT#614-MD - JSmith - Adding new stores with new characteristics fails to added characteristic values to all stores.
                    //}



                    foreach (storeCharInfo charInfo in charList)
                    {
                        if (charInfo.isDirty && charInfo.stRID == store.Key)
                        {
                            int scRID = Include.NoRID;
                            if (charInfo.action == storeCharInfoAction.InsertValue) //non list values
                            {
                                int scRidDuplicate = Include.NoRID;
                                if (StoreMgmt.DoesStoreCharValueAlreadyExist(charInfo.anyValue, charInfo.dataType, charInfo.scgRID, Include.NoRID, ref scRidDuplicate) == false)
                                {
                                    //insert characteristic value                          
                                    string stringVal = Include.NullForStringValue;
                                    DateTime? dateVal = null;
                                    float? numericVal = null;
                                    float? dollarVal = null;
                                    if (charInfo.dataType == fieldDataTypes.DateNoTime)
                                    {
                                        dateVal = Convert.ToDateTime(charInfo.anyValue);
                                    }
                                    else if (charInfo.dataType == fieldDataTypes.NumericDouble)
                                    {
                                        numericVal = Convert.ToSingle(charInfo.anyValue);
                                    }
                                    else if (charInfo.dataType == fieldDataTypes.NumericDollar)
                                    {
                                        dollarVal = Convert.ToSingle(charInfo.anyValue);
                                    }
                                    else
                                    {
                                        stringVal = charInfo.anyValue;
                                    }
                                    scRID = StoreCharValue_Insert(charInfo.scgRID, stringVal, dateVal, numericVal, dollarVal);
                                }
                                else
                                {
                                    scRID = scRidDuplicate;
                                }

                            }
                            else if (charInfo.action == storeCharInfoAction.UseValue)
                            {
                                scRID = charInfo.scRID;
                            }

                            if (charInfo.action != storeCharInfoAction.Skip)
                            {
                                //insert the join for the new store
                                StoreCharMaint storeCharMaint = new StoreCharMaint();
                                storeCharMaint.StoreCharValueJoin_Insert(store.Key, scRID, charInfo.scgRID);
                            }
                            else
                            {
                                //delete any old values for this store and group
                                StoreCharMaint storeCharMaint = new StoreCharMaint();
                                storeCharMaint.StoreCharValueJoin_DeleteForGroupAndStore(store.Key, charInfo.scgRID);
                            }
                        }


                    }


                    // Begin TT#614-MD - JSmith - Adding new stores with new characteristics fails to added characteristic values to all stores.
                    //store.Characteristics = currCharList;
                    //if (currCharList != null)
                    //{
                    //    store.Characteristics = currCharList;
                    //}
                    // End TT#614-MD - JSmith - Adding new stores with new characteristics fails to added characteristic values to all stores.

                    // update store status
                    // BEGIN TT#190 - MD - stodd - store service looping
                    //WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
                    //_SAB.StoreServerSession.UpdateStatusAndTextOnStoreProfile(ref store);

                    //store.Status = _SAB.StoreServerSession.GetStoreStatusForCurrentWeek(store.SellingOpenDt, store.SellingCloseDt);
                    //store.StockStatus = _SAB.StoreServerSession.GetStoreStatusForCurrentWeek(store.StockOpenDt, store.StockCloseDt);
                    //store.Text = Include.GetStoreDisplay(_gop.StoreDisplay, store.StoreId, store.StoreName, store.StoreDescription);
                    StoreProfile_UpdateStatusAndText(ref store);

                    // updates on database
                    StoreMaint _storeData = new StoreMaint();
                    _storeData.OpenUpdateConnection();
                    _storeData.StoreProfile_Update(store);
                    _storeData.CommitData();
                    _storeData.CloseUpdateConnection();

                    // update datatable
                    //DeleteStoreProfileFromDataTable(store);
                    //AddStoreProfileToDataTable(store);

                    _SAB.StoreServerSession.UpdateStoreProfileInList(store);

                    // Update store Profile in List
                    StoreProfile_UpdateInList(store);
                }
                finally
                {
                    ReleaseWriterLock();
                    //ReleaseStoreGroupWriterLock();
                }
                //Do special processing for the reserve store
                if (store.Key == StoreMgmt.ReserveStoreRID)
                {
                    StoreMgmt.StoreProfile_DoSpecialProcessingForReserveStore(store);
                }

                if (doRefreshGroups && ((charList != null && charList.Count > 0) || fieldChangedList.Count > 0))
                {
                    doRefreshNodes = true;
                    StoreGroups_RefreshForChangedFieldsAndChars(charList, fieldChangedList, progessbarOptions);
                }
            }
            catch (Exception err)
            {
                //string msg = "UpdateStoreProfile(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (_audit != null)
                //{
                //    _audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }

        // Begin TT#5800 - JSmith - Store Load API reverting VSW characteristic back to value prior to a manual change
        public static void StoreProfile_UpdateInStoreMgmt(StoreProfile store)
        {
            try
            {
                StoreProfile_UpdateStatusAndText(ref store);

                // Update store Profile in List
                StoreProfile_UpdateInList(store);

                //Do special processing for the reserve store
                if (store.Key == StoreMgmt.ReserveStoreRID)
                {
                    StoreMgmt.StoreProfile_DoSpecialProcessingForReserveStore(store);
                }

            }
            catch (Exception err)
            {

                throw;
            }
        }
        // End TT#5800 - JSmith - Store Load API reverting VSW characteristic back to value prior to a manual change

        public static void StoreProfile_DoSpecialProcessingForReserveStore(StoreProfile storeProfile)
        {
            GlobalOptions _go = new GlobalOptions();
            int globalOptionsRid = _SAB.ClientServerSession.GlobalOptions.Key;

            _go.OpenUpdateConnection();
            if (storeProfile.ActiveInd)  // set active store
                _go.UpdateReserveStore(globalOptionsRid, StoreMgmt.ReserveStoreRID);
            else			// unset active store
                _go.UpdateReserveStore(globalOptionsRid, 0);
            _go.CommitData();
            _go.CloseUpdateConnection();

            // refresh session with cached Global Options
            _SAB.ClientServerSession.RefreshGlobalOptions();
            _SAB.ApplicationServerSession.RefreshGlobalOptions();
        }

        public static void StoreProfile_UpdateStatusAndText(ref StoreProfile sp)
        {
            try
            {
               
                sp.SetStatus(_SAB.StoreServerSession.GetStoreStatusForCurrentWeek(sp.SellingOpenDt, sp.SellingCloseDt));
                sp.SetStockStatus(_SAB.StoreServerSession.GetStoreStatusForCurrentWeek(sp.StockOpenDt, sp.StockCloseDt));
                sp.SetText(Include.GetStoreDisplay(_gop.StoreDisplay, sp.StoreId, sp.StoreName, sp.StoreDescription));
            }
            catch (Exception err)
            {
                throw;
            }
        }

        static internal void StoreProfile_UpdateInList(StoreProfile sp)
        {
            try
            {
                AcquireWriterLock();
                try
                {
                    //if (sp.Characteristics != null)
                    //{
                    //    _allStoreCharacteristicsHash.Remove(sp.Key);
                    //    ArrayList charList = new ArrayList();
                    //    int cnt = sp.Characteristics.Count;
                    //    for (int i = 0; i < cnt; i++)
                    //    {
                    //        StoreCharGroupProfile scgp = (StoreCharGroupProfile)sp.Characteristics[i];
                    //        charList.Add(scgp);
                    //    }

                    //    _allStoreCharacteristicsHash.Add(sp.Key, charList);
                    //    sp.Characteristics = null;
                    //}

                    _allStoreList.Remove(sp);
                    //sp.Characteristics = null;
                    _allStoreList.Add(sp);
                }
                finally
                {
                    ReleaseWriterLock();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

        /// <summary>
        /// Takes a begin date and a store profile list and returns the In Store On Hand Dates
        /// for each store.
        /// </summary>
        /// <param name="beginDates"></param>
        /// <param name="storeList"></param>
        /// <returns></returns>
        // Begin TT#697 - JSmith - Performance
        //public ArrayList DetermineInStoreOnHandDay(ArrayList beginDates, ProfileList storeList)
        public static ArrayList DetermineInStoreOnHandDay(ArrayList beginDates, int[] storeList, Session aSession)
        // End TT#697
        {
            try
            {
                DayProfile shipDay;
                ArrayList dateList = new ArrayList();

                // Begin TT#697 - JSmith - Performance
                //for (int s=0;s<storeList.ArrayList.Count;s++)
                //{
                //    StoreProfile sp = (StoreProfile)storeList[s];
                for (int s = 0; s < storeList.Length; s++)
                {
                    StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeList[s]);
                    // End TT#697
                    shipDay = (DayProfile)beginDates[s];
                    bool matchDay = false;

                    //************************************************************************************
                    // If no ship on day is specified we default to the first day of the following week.
                    //************************************************************************************
                    if (!sp.ShipOnSunday && !sp.ShipOnMonday && !sp.ShipOnTuesday && !sp.ShipOnWednesday
                        && !sp.ShipOnThursday && !sp.ShipOnFriday && !sp.ShipOnSaturday)
                    {
                        WeekProfile thisWeek = aSession.Calendar.GetWeek(shipDay.Date); //StoreServerGlobal.Calendar.GetWeek(shipDay.Date);
                        WeekProfile nextWeek = aSession.Calendar.Add(thisWeek, 1); //StoreServerGlobal.Calendar.Add(thisWeek, 1);
                        DayProfile defaultDay = (DayProfile)nextWeek.Days[0];
                        dateList.Add(defaultDay);

                    }
                    else
                    {

                        while (!matchDay)
                        {
                            if (shipDay.DayOfWeek == DayOfWeek.Sunday && sp.ShipOnSunday)
                                matchDay = true;
                            if (shipDay.DayOfWeek == DayOfWeek.Monday && sp.ShipOnMonday)
                                matchDay = true;
                            if (shipDay.DayOfWeek == DayOfWeek.Tuesday && sp.ShipOnTuesday)
                                matchDay = true;
                            if (shipDay.DayOfWeek == DayOfWeek.Wednesday && sp.ShipOnWednesday)
                                matchDay = true;
                            if (shipDay.DayOfWeek == DayOfWeek.Thursday && sp.ShipOnThursday)
                                matchDay = true;
                            if (shipDay.DayOfWeek == DayOfWeek.Friday && sp.ShipOnFriday)
                                matchDay = true;
                            if (shipDay.DayOfWeek == DayOfWeek.Saturday && sp.ShipOnSaturday)
                                matchDay = true;

                            if (!matchDay)
                                shipDay = aSession.Calendar.Add(shipDay, 1); //StoreServerGlobal.Calendar.Add(shipDay, 1);
                        }

                        // since we want the On Hand date, we need to add one to the lead time
                        int leadTime = sp.LeadTime + 1;

                        DayProfile newDay = aSession.Calendar.Add(shipDay, leadTime); //StoreServerGlobal.Calendar.Add(shipDay, leadTime);
                        dateList.Add(newDay);
                    }
                }

                return dateList;
            }
            catch (Exception err)
            {
                //string msg = "DetermineInStoreOnHandDay(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        /// <summary>
        /// Returns a hashtable where the key is the ST_RID and the data is
        /// an array of bools representing which days are receipt days for that store.
        /// the array lines up to the current fiscal week makeup of days.  That is if 
        /// the first day of the week is Thursday, position [0] in the array will represent Thursday.
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetInStoreReceiptDates(Session aSession)
        {
            try
            {
                Hashtable ht = new Hashtable();
                DayProfile monday = null;
                DayProfile tuesday = null;
                DayProfile wednesday = null;
                DayProfile thursday = null;
                DayProfile friday = null;
                DayProfile saturday = null;
                DayProfile sunday = null;
                //DayProfile beginDay = Calendar.CurrentDate;
                DayProfile receiptDay = null;
                DayProfile holdDay = null;
                int d = 0;
                //DayOfWeek [] dow = new DayOfWeek[7];
                Hashtable daySubHash = new Hashtable();
                DayProfile beginDay = aSession.Calendar.FirstDayOfWeek;

                //***********************************************************************************
                // The daySubHash table holds the subscipt (based off of the first day of the week) for any
                // other day of the week.
                //***********************************************************************************
                //*****************************************************************
                // set up each day of the week from which we'll start our calculations
                // the DATE isn't important, we only want a date for each DAY of the week.
                //*****************************************************************
                for (d = 0; d < 7; d++)
                {
                    holdDay = aSession.Calendar.Add(beginDay, d);

                    daySubHash.Add(holdDay.DayOfWeek, d);

                    switch (holdDay.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            sunday = holdDay;
                            break;
                        case DayOfWeek.Monday:
                            monday = holdDay;
                            break;
                        case DayOfWeek.Tuesday:
                            tuesday = holdDay;
                            break;
                        case DayOfWeek.Wednesday:
                            wednesday = holdDay;
                            break;
                        case DayOfWeek.Thursday:
                            thursday = holdDay;
                            break;
                        case DayOfWeek.Friday:
                            friday = holdDay;
                            break;
                        case DayOfWeek.Saturday:
                            saturday = holdDay;
                            break;
                    }
                }

                //*******************************************************************************
                // for each store, look at the Ship On settings and calc a reciept day for 
                // each setting.
                //*******************************************************************************
                for (int s = 0; s < _allStoreList.ArrayList.Count; s++)
                {
                    StoreProfile sp = (StoreProfile)_allStoreList[s];
                    bool[] receiptDayArray = new bool[7] { false, false, false, false, false, false, false };

                    if (sp.ShipOnSunday)
                    {
                        receiptDay = aSession.Calendar.Add(sunday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }
                    if (sp.ShipOnMonday)
                    {
                        receiptDay = aSession.Calendar.Add(monday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }
                    if (sp.ShipOnTuesday)
                    {
                        receiptDay = aSession.Calendar.Add(tuesday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }
                    if (sp.ShipOnWednesday)
                    {
                        receiptDay = aSession.Calendar.Add(wednesday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }
                    if (sp.ShipOnThursday)
                    {
                        receiptDay = aSession.Calendar.Add(thursday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }
                    if (sp.ShipOnFriday)
                    {
                        receiptDay = aSession.Calendar.Add(friday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }
                    if (sp.ShipOnSaturday)
                    {
                        receiptDay = aSession.Calendar.Add(saturday, sp.LeadTime);
                        receiptDayArray[(int)daySubHash[receiptDay.DayOfWeek]] = true;
                    }

                    ht.Add(sp.Key, receiptDayArray);
                }


                return ht;
            }
            catch (Exception err)
            {
                //string msg = "GetInStoreReceiptDates(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        public static bool DoesIMOExist(string IMO_ID)
        {
            bool IMOExists = false;
            try
            {
                for (int s = 0; s < _allStoreList.ArrayList.Count; s++)
                {
                    StoreProfile sp = (StoreProfile)_allStoreList[s];
                    if (sp.IMO_ID != null && sp.IMO_ID != string.Empty)
                    {
                        if ((sp.IMO_ID.Trim()).ToLower() == (IMO_ID.Trim()).ToLower())
                        {
                            IMOExists = true;
                            break;
                        }
                    }
                }
                return IMOExists;
            }
            catch (Exception err)
            {
                //string msg = "DoesIMOExist(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }

        /// <summary>
        /// Gets the sales status of each store for the year/week provided
        /// </summary>
        /// <param name="yearWeek"></param>
        /// <returns></returns>
        // Begin TT#4988 - JSmith - Performance
        //public static Hashtable GetStoreSalesStatusHash(int yearWeek, Session aSession)
        // Begin TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
        //public static Dictionary<int, eStoreStatus> GetStoreSalesStatusHash(int yearWeek, Session aSession)
        public static Dictionary<int, eStoreStatus> GetStoreSalesStatusHash(int yearWeek, Session aSession, SessionAddressBlock SessionAddressBlock, ProfileList allStoreList)
        // End TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
        // End TT#4988 - JSmith - Performance
        {
            try
            {
                AcquireWriterLock();
                // Begin TT#4988 - JSmith - Performance
                //Hashtable storeStatusHash = new Hashtable();
                // Begin TT#2090-MD - JSmith - Performance
                //Dictionary<int, eStoreStatus> storeStatusHash = new Dictionary<int, eStoreStatus>();
                Dictionary<int, eStoreStatus> storeStatusHash;
                // End TT#4988 - JSmith - Performance

                bool updateStoreList = false;
                if (_storeSalesStatusByWeek.TryGetValue(yearWeek, out storeStatusHash))
                {
                    // verify store list
                    if (allStoreList.ArrayList.Count == storeStatusHash.Count)
                    {
                        foreach (StoreProfile sp in allStoreList.ArrayList)
                        {
                            if (!storeStatusHash.ContainsKey(sp.Key))
                            {
                                updateStoreList = true;
                                break;
                            }
                        }
                        if (!updateStoreList)
                        {
                            return storeStatusHash;
                        }
                    }
                }
                // End TT#2090-MD - JSmith - Performance

                //WeekProfile baseWeek = _SAB.StoreServerSession.CalendarGlobal.GetWeek(yearWeek); //Calendar.GetWeek(yearWeek);
                WeekProfile baseWeek = aSession.Calendar.GetWeek(yearWeek); //Calendar.GetWeek(yearWeek); //TT#1517-MD Store Service Optimization - SRisch

                // Begin TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                //foreach (StoreProfile sp in _allStoreList.ArrayList)
                //{
                //    eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt);  //StoreServerGlobal.GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt);
                //    storeStatusHash.Add(sp.Key, storeStatus);
                //}
                // Begin TT#2090-MD - JSmith - Performance
                //foreach (StoreProfile sp in allStoreList.ArrayList)
                //{
                //    eStoreStatus storeStatus = SessionAddressBlock.StoreServerSession.GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt);
                //    storeStatusHash.Add(sp.Key, storeStatus);
                //}
                // End TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.

                storeStatusHash = SessionAddressBlock.StoreServerSession.GetAllStoresSalesStatus(baseWeek, allStoreList.ArrayList);

                _storeSalesStatusByWeek[yearWeek] = storeStatusHash;
                // End TT#2090-MD - JSmith - Performance

                return storeStatusHash;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreSalesStatusHash(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Gets the stock status of each store for the year/week provided
        /// </summary>
        /// <param name="yearWeek"></param>
        /// <returns></returns>
        // Begin TT#4988 - JSmith - Performance
        //public static Hashtable GetStoreSalesStatusHash(int yearWeek, Session aSession)
        // Begin TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
        //public static Dictionary<int, eStoreStatus> GetStoreStockStatusHash(int yearWeek, Session aSession)
        public static Dictionary<int, eStoreStatus> GetStoreStockStatusHash(int yearWeek, Session aSession, SessionAddressBlock SessionAddressBlock, ProfileList allStoreList)
        // End TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
        // End TT#4988 - JSmith - Performance
        {
            try
            {
                AcquireWriterLock();
                // Begin TT#4988 - JSmith - Performance
                //Hashtable storeStatusHash = new Hashtable();
                // Begin TT#2090-MD - JSmith - Performance
                //Dictionary<int, eStoreStatus> storeStatusHash = new Dictionary<int, eStoreStatus>();
                Dictionary<int, eStoreStatus> storeStatusHash;
                // End TT#4988 - JSmith - Performance
                bool updateStoreList = false;
                if (_storeStockStatusByWeek.TryGetValue(yearWeek, out storeStatusHash))
                {
                    // verify store list
                    if (allStoreList.ArrayList.Count == storeStatusHash.Count)
                    {
                        foreach (StoreProfile sp in allStoreList.ArrayList)
                        {
                            if (!storeStatusHash.ContainsKey(sp.Key))
                            {
                                updateStoreList = true;
                                break;
                            }
                        }
                        if (!updateStoreList)
                        {
                            return storeStatusHash;
                        }
                    }
                }
                // End TT#2090-MD - JSmith - Performance

                WeekProfile baseWeek = aSession.Calendar.GetWeek(yearWeek); //Calendar.GetWeek(yearWeek);

                // Begin TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                //foreach (StoreProfile sp in _allStoreList.ArrayList)
                //{
                //    eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt); //StoreServerGlobal.GetStoreStatus(baseWeek, sp.StockOpenDt, sp.StockCloseDt);
                //    storeStatusHash.Add(sp.Key, storeStatus);
                //}
                // Begin TT#2090-MD - JSmith - Performance
                //foreach (StoreProfile sp in allStoreList.ArrayList)
                //{
                //    eStoreStatus storeStatus = SessionAddressBlock.StoreServerSession.GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt); //StoreServerGlobal.GetStoreStatus(baseWeek, sp.StockOpenDt, sp.StockCloseDt);
                //    storeStatusHash.Add(sp.Key, storeStatus);
                //}
                // End TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.

                storeStatusHash = SessionAddressBlock.StoreServerSession.GetAllStoresStockStatus(baseWeek, allStoreList.ArrayList);

                _storeStockStatusByWeek[yearWeek] = storeStatusHash;
                // End TT#2090-MD - JSmith - Performance

                return storeStatusHash;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreStockStatusHash(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                ReleaseWriterLock();
            }
        }

        // BEGIN TT#739-MD - STodd - delete stores
        public static void StoreProfile_MarkForDelete(StoreProfile store, bool markForDelete)
        {
            try
            {
                AcquireWriterLock();
                store.SetDeleteStore(markForDelete);    // TT#3272 - stodd - mark store fro delete not refreshing
                                                        // updates on database
                                                        //StoreServerGlobal.OpenUpdateConnection();
                                                        //StoreServerGlobal.MarkStoreForDelete(store.Key, markForDelete);
                                                        //StoreServerGlobal.CommitData();
                                                        //StoreServerGlobal.CloseUpdateConnection();

                StoreData _storeData = new StoreData();
                _storeData.OpenUpdateConnection();
                _storeData.StoreProfile_MarkForDelete(store.Key, markForDelete);
                _storeData.CommitData();
                _storeData.CloseUpdateConnection();

                // Begin TT#3272 - stodd - mark store fro delete not refreshing
                // update datatable
                //DeleteStoreProfileFromDataTable(store);
                //AddStoreProfileToDataTable(store);
                // Update store Profile in List
                StoreProfile_UpdateInList(store);
                // End TT#3272 - stodd - mark store fro delete not refreshing
            }
            catch (Exception err)
            {
                //string msg = "UpdateStoreProfile(): " + err.ToString();
                ////EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (_audit != null)
                //{
                //    _audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                ReleaseWriterLock();
                //ReleaseStoreGroupWriterLock();
            }

        }
        // END TT#739-MD - STodd - delete stores

        // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
        public static bool AllowRemoveVSWID(int stRID)
        {
            try
            {
                StoreData _storeData = new StoreData();
                return _storeData.AllowRemoveVSWID(stRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store

        #endregion


        #region "Store Groups"
        // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
		//private static void StoreGroups_Populate()
        private static void StoreGroups_Populate(bool bLoadInactiveGroups = false)
        // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
        {
            try
            {
                if (_groupList == null)
                    _groupList = new ProfileList(eProfileType.StoreGroup);
                else
                    _groupList.Clear();


                _levelList.Clear();

                StoreGroupMaint groupData = new StoreGroupMaint();

                DataTable _dtStoreGroup;
                //AcquireWriterLock();
                _bLoadInactiveGroups = bLoadInactiveGroups;     // TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                try
                {
                    // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
					//_dtStoreGroup = groupData.StoreGroup_Read(eDataOrderBy.RID);
                    _dtStoreGroup = groupData.StoreGroup_Read(eDataOrderBy.RID, bLoadInactiveGroups);
                    // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                    _dtStoreGroup.PrimaryKey = new DataColumn[] { _dtStoreGroup.Columns["SG_RID"] };
                }
                finally
                {
                    // Ensure that the lock is released.
                    //ReleaseWriterLock();
                }
                DataTable _dtStoreGroupLevel;
                //AcquireWriterLock();
                try
                {
                    _dtStoreGroupLevel = groupData.StoreGroupLevel_ReadAll();
                    //_dtStoreGroupLevel.PrimaryKey = new DataColumn[] { _dtStoreGroupLevel.Columns["SGL_RID"] };
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Loading Attribute Sets..." + System.Environment.NewLine + ex.ToString());
                }
                finally
                {
                    // Ensure that the lock is released.
                    //ReleaseWriterLock();
                }

                //DataTable _dtStoreGroupLevelResults;
                //AcquireWriterLock();
                try
                {
                    //_dtStoreGroupLevelResults = groupData.StoreGroupLevelResults_ReadAll();
                }
                finally
                {
                    // Ensure that the lock is released.
                    //ReleaseWriterLock();
                }



                char dynamicInd;
                int groupRID;

                StoreGroupProfile groupProf = null;

                //========================================
                // loop through Store groups (attributes)
                //========================================
                foreach (DataRow storeGroupRow in _dtStoreGroup.Rows)
                {
                    groupRID = Convert.ToInt32(storeGroupRow["SG_RID"]);
                    groupProf = new StoreGroupProfile(groupRID);
                    groupProf.Name = (string)storeGroupRow["SG_ID"]; ;
                    dynamicInd = Convert.ToChar(storeGroupRow["SG_DYNAMIC_GROUP_IND"]);
                    groupProf.IsDynamicGroup = Include.ConvertCharToBool(dynamicInd);
                    groupProf.OwnerUserRID = Convert.ToInt32(storeGroupRow["USER_RID"]);
                    groupProf.FilterRID = Convert.ToInt32(storeGroupRow["FILTER_RID"]);
                    groupProf.Version = Convert.ToInt32(storeGroupRow["SG_VERSION"]);

                    //========================================================
                    // loop through child store group levels (attribute sets)
                    //========================================================
                    DataRow[] childRowsA = _dtStoreGroupLevel.Select("SG_RID=" + groupRID.ToString());
                    foreach (DataRow sglRow in childRowsA)
                    {
                        int sglRid = Convert.ToInt32(sglRow["SGL_RID"]);

                        string levelName = null;
                        if (sglRow["SGL_ID"] != DBNull.Value)
                        {
                            levelName = (string)sglRow["SGL_ID"];
                        }

                        int levelSeq = 0;
                        if (sglRow["SGL_SEQUENCE"] != DBNull.Value)
                        {
                            levelSeq = Convert.ToInt32(sglRow["SGL_SEQUENCE"]);
                        }

                        int levelVersion = Include.NoRID;
                        if (sglRow["SGL_VERSION"] != DBNull.Value)
                        {
                            levelVersion = Convert.ToInt32(sglRow["SGL_VERSION"]);
                        }

                        eGroupLevelTypes levelType = eGroupLevelTypes.Normal;
                        if (sglRow["LEVEL_TYPE"] != DBNull.Value)
                        {
                            levelType = (eGroupLevelTypes)Convert.ToInt32(sglRow["LEVEL_TYPE"]);
                        }

                        StoreGroupLevelProfile levelProf = StoreGroup_AddLevelToGroup(ref groupProf, sglRid, levelName, levelSeq, levelVersion, levelType);

                        //delay adding stores to the group until they are needed (on demand)
                        levelProf.Stores = new ProfileList(eProfileType.Store);

                        //DataRow[] drStoresForLevel = _dtStoreGroupLevelResults.Select("SG_RID=" + groupRID.ToString() + " AND SGL_RID=" + sglRid.ToString());
                        //foreach (DataRow drStore in drStoresForLevel)
                        //{
                        //    int storeRID = (int)drStore["ST_RID"];
                        //    StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
                        //    levelProf.Stores.Add(sp);
                        //}

                        groupProf.LevelsAndStoresFilled = false;
                        //levelProf.StoresFilled = false;

                    }

                    _groupList.Add(groupProf);
                }

                // Begin TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors
                StoreGroup_SortList();
                // End TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors



                // Don't need these any longer...
                //_dtStoreGroup = null;
                //_dtStoreGroupLevel = null;

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        private static StoreGroupProfile StoreGroup_GetFromFilter(int filterRID)
        {
            StoreGroupProfile storeGroup = null;
            foreach (StoreGroupProfile sg in _groupList)
            {
                //if (storeGroup != null)
                //    break;


                if (sg.FilterRID == filterRID)
                {
                    storeGroup = sg;
                    break;
                }

            }
            return storeGroup;
        }
        private static void StoreGroup_SortList()
        {
            _groupList.ArrayList.Sort(new SGSequenceComparer());
        }

        public static StoreGroupProfile StoreGroup_Get(int groupRid)
        {
            return (StoreGroupProfile)_groupList.FindKey(groupRid);
        }
        public static StoreGroupProfile StoreGroup_GetFilled(int groupRid)
        {
            StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRid);
            if (sg.LevelsAndStoresFilled == false)
            {
                StoreGroup_FillStoreResults(sg.Key);
            }

            return sg;
        }
        public static string StoreGroup_GetName(int groupRid)
        {
            try
            {
                StoreGroupProfile sgp = (StoreGroupProfile)_groupList.FindKey(groupRid);
                string groupName = string.Empty;
                if (sgp != null)
                    groupName = sgp.Name;
                else
                    groupName = "Not Found";
                return groupName;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupName(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }

        // Begin TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
        public static int StoreGroup_GetVersion(int groupRid)
        {
            StoreGroupProfile sgp = StoreGroup_Get(groupRid);
            if (sgp != null)
            {
                return sgp.Version;
            }
            else
            {
                return Include.Undefined;
            }
        }
        // End TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error

        public static string StoreGroupLevel_GetName(int groupRid, int groupLevelRid)
        {
            try
            {
                StoreGroupProfile sgp = (StoreGroupProfile)_groupList.FindKey(groupRid);
                StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sgp.GroupLevels.FindKey(groupLevelRid);
                return sglp.Name;
            }
            catch (Exception err)
            {
                //string msg = "GetGroupSetName(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        public static ProfileList StoreGroup_GetLevelListFilled(int groupRid)
        {
            try
            {
                StoreGroupProfile sgp = (StoreGroupProfile)_groupList.FindKey(groupRid);
                //if (!sgp.Filled)
                //{
                //    int levelCnt = sgp.GroupLevels.Count;
                //    for (int i = 0; i < levelCnt; i++)
                //    {
                //        StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sgp.GroupLevels[i];
                //        sglp.Stores = StoreServerGlobal.GetStoresInGroupLevel(sglp.Key);
                //    }
                //    sgp.Filled = true;
                //}

                if (sgp.LevelsAndStoresFilled == false)
                {
                    StoreGroup_FillStoreResults(groupRid);
                }

                return sgp.GroupLevels;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupLevelList(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        public static ProfileList StoreGroup_GetLevelListViewList(int groupRid, bool fillStores)
        {
            try
            {
                ProfileList pl = new ProfileList(eProfileType.StoreGroupLevelListView);
                StoreGroupProfile sgp = (StoreGroupProfile)_groupList.FindKey(groupRid);

                if (fillStores && sgp.LevelsAndStoresFilled == false)
                {
                    StoreGroup_FillStoreResults(groupRid);
                }

                int groupCnt = sgp.GroupLevels.Count;
                for (int i = 0; i < groupCnt; i++)
                {
                    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sgp.GroupLevels[i];
                    StoreGroupLevelListViewProfile view = new StoreGroupLevelListViewProfile(sglp.Key);
                    view.GroupRid = sglp.GroupRid;
                    view.Name = sglp.Name;
                    view.Sequence = sglp.Sequence;

                    if (fillStores)
                    {
                        view.Stores = sglp.Stores;  //StoreServerGlobal.GetStoresInGroupLevel(sglp.Key);
                    }
                    else
                    {
                        view.Stores = null;
                    }
                    pl.Add(view);
                }
                pl.ArrayList.Sort(new SGLLVSequenceComparer());

                return pl;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupLevelListViewList(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        /// <summary>
        /// Returns the store group Level list using the smaller format of the StoreGroupLevelListViewProfile.
        /// Stores are NOT filled in and are NULL
        /// </summary>
        /// <param name="groupRid"></param>
        /// <returns></returns>
        public static ProfileList StoreGroup_GetLevelListViewList(int groupRid)
        {
            try
            {
                ProfileList groupLevelList = StoreGroup_GetLevelListViewList(groupRid, false);

                return groupLevelList;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupLevelListViewList(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        /// <summary>
        /// Returns a list in the smaller format of the StoreGroupListViewProfile
        /// </summary>
        /// <param name="aStoreGroupSelectType">
        /// A filter value of store groups to select.
        /// </param>
        /// <returns>
        /// A ProfileList of StoreGroupListViewProfiles for the groups that were selected.
        /// </returns>
        public static ProfileList StoreGroup_GetListViewList(eStoreGroupSelectType aStoreGroupSelectType, bool aAddGlobalUserLabel)
        {
            // Begin Track #4872 - JSmith - Global/User Attributes
            bool selectGroup = false;
            //SecurityAdmin secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance
            // End Track #4872
            try
            {


                ProfileList pl = new ProfileList(eProfileType.StoreGroupListView);
                int groupCnt = _groupList.Count;
                // Begin Track #4872 - JSmith - Global/User Attributes
                // No reason to show global/user label if all global
                if (aStoreGroupSelectType == eStoreGroupSelectType.GlobalOnly)
                {
                    aAddGlobalUserLabel = false;
                }
                // End Track #4872
                for (int i = 0; i < groupCnt; i++)
                {
                    StoreGroupProfile sgp = (StoreGroupProfile)_groupList[i];
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    if (sgp.Visible)
                    {
                        selectGroup = true;
                        switch (aStoreGroupSelectType)
                        {
                            case eStoreGroupSelectType.GlobalOnly:
                                if (sgp.OwnerUserRID != Include.GlobalUserRID)
                                {
                                    selectGroup = false;
                                }
                                break;
                            case eStoreGroupSelectType.MyUserOnly:
                                if (sgp.OwnerUserRID != UserID)
                                {
                                    selectGroup = false;
                                }
                                break;
                            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                            case eStoreGroupSelectType.MySharedOnly:
                                if (sgp.OwnerUserRID == Include.GlobalUserRID ||
                                    sgp.OwnerUserRID == UserID)
                                {
                                    selectGroup = false;
                                }
                                break;
                            case eStoreGroupSelectType.MyUserAndGlobal:
                                if (sgp.OwnerUserRID != Include.GlobalUserRID &&
                                    sgp.OwnerUserRID != UserID)
                                {
                                    selectGroup = false;
                                }
                                break;
                            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                            case eStoreGroupSelectType.MyUserAndShared:
                                if (sgp.OwnerUserRID == Include.GlobalUserRID)
                                {
                                    selectGroup = false;
                                }
                                break;
                            // Begin TT#5664 - JSmith - All User Store Attributes appear in User Methods 
                            case eStoreGroupSelectType.All:
                                if (sgp.OwnerUserRID != Include.GlobalUserRID &&
                                    sgp.OwnerUserRID != UserID &&
                                    !_usersAssignedToMe.Contains(sgp.OwnerUserRID))
                                {
                                    selectGroup = false;
                                }
                                break;
                            // End TT#5664 - JSmith - All User Store Attributes appear in User Methods 
                        }

                        if (selectGroup)
                        {
                            // End Track #4872
                            StoreGroupListViewProfile view = new StoreGroupListViewProfile(sgp.Key);
                            view.Name = sgp.Name;
                            // Begin TT#1125 - JSmith - Global/User should be consistent
                            //if (aAddGlobalUserLabel)
                            //{
                            //    if (sgp.OwnerUserRID == Include.GlobalUserRID)
                            //    {
                            //        view.Name += " (Global)";
                            //    }
                            //    else
                            //    {
                            //        if (sgp.OwnerUserRID == Include.UndefinedUserRID)
                            //            view.Name += " (User)";
                            //        else
                            //        {
                            //            view.Name += " (" + secAdmin.GetUserName(sgp.OwnerUserRID) + ")";
                            //        }
                            //    }
                            //}
                            if (aAddGlobalUserLabel)
                            {
                                if (sgp.OwnerUserRID != Include.GlobalUserRID)
                                {
                                    //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                                    //view.Name += " (" + secAdmin.GetUserName(sgp.OwnerUserRID) + ")";
                                    view.Name += " (" + UserNameStorage.GetUserName(sgp.OwnerUserRID) + ")";
                                    //End TT#827-MD -jsobek -Allocation Reviews Performance
                                }
                            }
                            // End TT#1125
                            view.IsDynamicGroup = sgp.IsDynamicGroup;
                            // Begin Track #4872 - JSmith - Global/User Attributes
                            view.OwnerUserRID = sgp.OwnerUserRID;
                            view.FilterRID = sgp.FilterRID;
                            view.Version = sgp.Version;
                            // End Track #4872
                            pl.Add(view);
                        }
                    }
                    // End Track #4872
                }
                pl.ArrayList.Sort(new SGLVSequenceComparer());
                return pl;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupListViewList(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        public static ProfileList StoreGroupLevel_GetStoreProfileList(int groupRid, int groupLevelRid)
        {
            try
            {
                StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRid);
                StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sg.GroupLevels.FindKey(groupLevelRid);

                if (sg.LevelsAndStoresFilled == false)
                {
                    StoreGroup_FillStoreResults(groupRid);
                }

                return sglp.Stores;
            }
            catch (Exception err)
            {
                //string msg = "GetStoresInGroup(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }


        /// <summary>
        /// Will populate stores if needed.
        /// </summary>
        /// <param name="storeGroupKey"></param>
        /// <returns></returns>
        public static ProfileXRef GetStoreGroupLevelXRef(int storeGroupKey)
        {
            try
            {
                StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(storeGroupKey);
                if (sg.LevelsAndStoresFilled == false)
                {
                    StoreGroup_FillStoreResults(sg.Key);
                }

                ProfileXRef profileXRef = new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store);

                int levelCnt = sg.GroupLevels.Count;
                for (int i = 0; i < levelCnt; i++)
                {
                    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sg.GroupLevels[i];
                    int storeCnt = sglp.Stores.Count;
                    for (int j = 0; j < storeCnt; j++)
                    {
                        StoreProfile sp = (StoreProfile)sglp.Stores[j];
                        profileXRef.AddXRefIdEntry(sglp.Key, sp.Key);
                    }
                }

                return profileXRef;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupLevelXRef(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }

        /// <summary>
        /// Will populate stores if needed.
        /// </summary>
        /// <param name="storeGroupKey"></param>
        /// <returns></returns>
        public static Hashtable GetStoreGroupLevelHashTable(int storeGroupKey)
        {
            try
            {
                StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(storeGroupKey);
                if (sg.LevelsAndStoresFilled == false)
                {
                    StoreGroup_FillStoreResults(sg.Key);
                }


                Hashtable storeHash = new Hashtable();

                int levelCnt = sg.GroupLevels.Count;
                for (int i = 0; i < levelCnt; i++)
                {
                    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sg.GroupLevels[i];
                    int storeCnt = sglp.Stores.Count;
                    for (int j = 0; j < storeCnt; j++)
                    {
                        StoreProfile sp = (StoreProfile)sglp.Stores[j];
                        storeHash.Add(sp.Key, sglp.Key);
                    }
                }

                return storeHash;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupLevelHashTable(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }

        public static string StoreGroupLevel_GetName(int storeGroupLevelKey)
        {
            try
            {
                StoreGroupLevelProfile sgli = _levelList.Find(x => x.Key == storeGroupLevelKey);
                return sgli.Name;
            }
            catch (Exception err)
            {
                //string msg = "GetStaticStoresInGroup(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }


        /// <summary>
        /// Creates a attribute set filter with a dynamic condition
        /// </summary>
        /// <param name="scgRID"></param>
        /// <param name="charGroupName"></param>
        /// <param name="userRID"></param>
        public static int StoreGroup_InsertDynamic(int scgRID, string charGroupName, int userRID)
        {
            FilterData filterData = new FilterData();
            return filterData.InsertFilterForDynamicStoreGroup(scgRID, charGroupName, userRID);
        }


        private static void StoreGroup_CreateLevelsForStaticFilter(ref List<levelInfo> tempLevelList, filter f)
        {
            ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq());
            int levelSeq = 0; //start level sequence at zero
            foreach (ConditionNode cn in conditionRoot.ConditionNodes)
            {
                if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupName)
                {
                    string levelName = cn.condition.valueToCompare;

                    int tempSeq = levelSeq;
                    eGroupLevelTypes levelType = eGroupLevelTypes.Normal;
                    if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupName)
                    {
                        levelSeq++;
                    }

                    int levelRID = cn.condition.fieldIndex;


                    //int levelRID = groupData.StoreGroupLevel_Insert(tempSeq, groupRID, levelName, isActive: true, conditionRID: cn.condition.conditionRID, levelType: (int)levelType);
                    tempLevelList.Add(new levelInfo(levelRID, levelName, tempSeq, cn.condition.conditionRID, levelType, 1)); //put in a list so we can add to the profile after the db changes are committed.

                }
            }
        }
        private static void StoreGroup_CreateLevelsForDynamicFilter(ref List<levelInfo> tempLevelList, filter f)
        {
            //Add Dynamic Levels
            ConditionNode conditionRoot = f.FindConditionNode(f.GetRootConditionSeq());

            if (conditionRoot == null)
            {
                return;
            }


            DataTable dtNested = new DataTable();
            dtNested.Columns.Add("LevelName");
            dtNested.Columns.Add("LevelValues");
            dtNested.Columns.Add("LevelFields");
            dtNested.Columns.Add("OBJECT_TYPE", typeof(int));

            string delim = "~";

            DataTable dtNestedClone = new DataTable();
            dtNestedClone = dtNested.Clone();

            string valueFieldName;
            string valueReferenceName;
            bool firstDynamicGroup = true;
            storeObjectTypes storeObjectType;
            bool includeEmptyStrings = false;
			// Need to know if multiple conditions so can include null entries for compound dynamic attribute
            bool multipleConditions = conditionRoot.ConditionNodes.Count > 0;
            foreach (ConditionNode cn in conditionRoot.ConditionNodes)
            {
                if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic)
                {
                    string sortString;
                    if (cn.condition.valueToCompareInt2 == filterSortByDirectionTypes.Ascending.dbIndex)
                    {
                        sortString = " ASC";
                    }
                    else
                    {
                        sortString = " DESC";
                    }


                    dtNestedClone.Rows.Clear();
                    filterStoreFieldTypes filterStoreFieldType = null;
                    //Get the characteristic group
                    int charGroupRID = cn.condition.operatorIndex;
                    //Get all values for this characteristic from the database.
                    DataTable dtCharValuesForGroup;
                    if (cn.condition.valueToCompareInt == storeObjectTypes.StoreFields)
                    {
                        List<DataRow> selectedFields = new List<DataRow>();
                        List<fieldColumnMap> columnMapList = null;
                        DataTable dtAllStoreFields = null;

                        int fieldIndex = charGroupRID * -1;
                        filterStoreFieldType = filterStoreFieldTypes.FromIndex(fieldIndex);
                        DataTable dtTempField = new DataTable();
                        dtTempField.Columns.Add("OBJECT_TYPE", typeof(int));
                        dtTempField.Columns.Add("FIELD_INDEX", typeof(int));
                        DataRow drTemp = dtTempField.NewRow();
                        drTemp["OBJECT_TYPE"] = storeObjectTypes.StoreFields.Index;
                        drTemp["FIELD_INDEX"] = filterStoreFieldType.storeFieldType.fieldIndex;
                        dtTempField.Rows.Add(drTemp);

                        selectedFields.Add(drTemp);

                        DataTable dtStoreFieldValues = StoreValidation.ReadFieldAndCharacteristicValuesForSelectedFields(_SAB, selectedFields, ref columnMapList, ref dtAllStoreFields);
                        dtCharValuesForGroup = dtStoreFieldValues.DefaultView.ToTable(true, filterStoreFieldType.storeFieldType.Name);
                        valueFieldName = filterStoreFieldType.storeFieldType.Name;
                        valueReferenceName = filterStoreFieldType.storeFieldType.Name;
                        storeObjectType = storeObjectTypes.StoreFields;
                        includeEmptyStrings = false;
                    }
                    else
                    {
					    // Begin TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
						//dtCharValuesForGroup = filterDataHelper.StoreCharacteristicsGetValuesForGroup(charGroupRID);
                        dtCharValuesForGroup = filterDataHelper.StoreCharacteristicsGetValuesForGroup(charGroupRID, sortString);
						// End TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
                        valueFieldName = "CHAR_VALUE";
                        valueReferenceName = "SC_RID";
                        storeObjectType = storeObjectTypes.StoreCharacteristics;
                        includeEmptyStrings = true;
                        // Add row with characteristic group RID to handle value not set if multiple conditions in compound dynamic attribute
                        if (multipleConditions)
                        {
                            DataRow drNullRow = dtCharValuesForGroup.NewRow();
                            drNullRow["CHAR_VALUE"] = DBNull.Value;
                            drNullRow["SC_RID"] = charGroupRID;
                            dtCharValuesForGroup.Rows.Add(drNullRow);
                        }
                    }
                    //sort the characteristics before adding them
					// Begin TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
                    // Begin TT#1941-MD - JSmith - Store Attribute Sets not appearing in alpha numeric order
					//DataRow[] drCharValuesForGroupSorted = dtCharValuesForGroup.Select("", valueFieldName + sortString);
                    //DataRow[] drCharValuesForGroupSorted = new DataRow[dtCharValuesForGroup.Rows.Count];
                    //dtCharValuesForGroup.Rows.CopyTo(drCharValuesForGroupSorted, 0);
                    DataRow[] drCharValuesForGroupSorted;
                    if (cn.condition.valueToCompareInt == storeObjectTypes.StoreFields)
                    {
                        drCharValuesForGroupSorted = dtCharValuesForGroup.Select("", valueFieldName + sortString);
                    }
                    else
                    {
                        drCharValuesForGroupSorted = new DataRow[dtCharValuesForGroup.Rows.Count];
                        dtCharValuesForGroup.Rows.CopyTo(drCharValuesForGroupSorted, 0);
                    }
                    // End TT#1941-MD - JSmith - Store Attribute Sets not appearing in alpha numeric order
					// End TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.

                    if (firstDynamicGroup)
                    {
                        foreach (DataRow drCharValue in drCharValuesForGroupSorted) //ABC
                        {
                            if (drCharValue[valueFieldName] != DBNull.Value && (includeEmptyStrings == true || drCharValue[valueFieldName].ToString() != string.Empty))
                            {
                                DataRow drNestedClone = dtNestedClone.NewRow();
								// Begin TT#1808-MD - JSmith - Store Load Error
								//drNestedClone["LevelName"] = drCharValue[valueFieldName];
                                if (filterStoreFieldType != null 
                                    && filterStoreFieldType.storeFieldType.dataType == fieldDataTypes.DateNoTime)
                                {
                                    drNestedClone["LevelName"] = Convert.ToDateTime(drCharValue[valueFieldName]).ToString("MM/dd/yyyy");
                                }
                                else
                                {
                                    drNestedClone["LevelName"] = drCharValue[valueFieldName];
                                }
								// End TT#1808-MD- JSmith - Store Load Error
                                drNestedClone["LevelValues"] = drCharValue[valueReferenceName].ToString();
                                if (filterStoreFieldType != null)
                                {
                                    drNestedClone["LevelFields"] = filterStoreFieldType.dbIndex;
                                }
                                else
                                {
                                    drNestedClone["LevelFields"] = "-99";
                                }
                                drNestedClone["OBJECT_TYPE"] = storeObjectType.Index;
                                dtNestedClone.Rows.Add(drNestedClone);
                            }
                            // Handle value not set if multiple conditions in compound dynamic attribute
                            else if (multipleConditions && drCharValue[valueFieldName] == DBNull.Value)
                            {
                                DataRow drNestedClone = dtNestedClone.NewRow();
                                drNestedClone["LevelName"] = "null";
                                drNestedClone["LevelValues"] = "null:" + drCharValue[valueReferenceName].ToString();  // Embed characteristic group RID with null value to be used in query
                                if (filterStoreFieldType != null)
                                {
                                    drNestedClone["LevelFields"] = filterStoreFieldType.dbIndex;
                                }
                                else
                                {
                                    drNestedClone["LevelFields"] = "-99";
                                }
                                drNestedClone["OBJECT_TYPE"] = storeObjectType.Index;
                                dtNestedClone.Rows.Add(drNestedClone);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow drExistingNested in dtNested.Rows) //ABC
                        {
                            foreach (DataRow drCharValue in drCharValuesForGroupSorted)  //DEF
                            {
                                if (drCharValue[valueFieldName] != DBNull.Value && (includeEmptyStrings == true || drCharValue[valueFieldName].ToString() != string.Empty))
                                {

                                    //A : D, B : D, C : D     A : E, B : E, C : E      A : F, B : F, C : F
                                    DataRow drNestedClone = dtNestedClone.NewRow();
                                    // Begin TT#1922-MD - JSmith - Dynamic Store Attribute - Select the Up Arrow to move a Conditon and receive a System Invalid Cast Exception.
                                    //drNestedClone["LevelName"] = (string)drExistingNested["LevelName"] + " : " + (string)drCharValue[valueFieldName];
                                    //drNestedClone["LevelValues"] = (string)drExistingNested["LevelValues"] + delim + drCharValue[valueReferenceName].ToString();
                                    //if (filterStoreFieldType != null)
                                    //{
                                    //    drNestedClone["LevelFields"] = (string)drExistingNested["LevelFields"] + delim + filterStoreFieldType.dbIndex.ToString();
                                    //}
                                    //else
                                    //{
                                    //    drNestedClone["LevelFields"] = (string)drExistingNested["LevelFields"] + delim + "-99";
                                    //}
                                    drNestedClone["LevelName"] = Convert.ToString(drExistingNested["LevelName"]) + " : " + Convert.ToString(drCharValue[valueFieldName]);
                                    drNestedClone["LevelValues"] = Convert.ToString(drExistingNested["LevelValues"]) + delim + Convert.ToString(drCharValue[valueReferenceName]);
                                    if (filterStoreFieldType != null)
                                    {
                                        drNestedClone["LevelFields"] = Convert.ToString(drExistingNested["LevelFields"]) + delim + Convert.ToString(filterStoreFieldType.dbIndex);
                                    }
                                    else
                                    {
                                        drNestedClone["LevelFields"] = Convert.ToString(drExistingNested["LevelFields"]) + delim + "-99";
                                    }

                                    drNestedClone["OBJECT_TYPE"] = storeObjectType.Index;
                                    dtNestedClone.Rows.Add(drNestedClone);
                                }
                                // Handle value not set if multiple conditions in compound dynamic attribute
                                else if (multipleConditions && drCharValue[valueFieldName] == DBNull.Value)
                                {

                                    //A : D, B : D, C : D     A : E, B : E, C : E      A : F, B : F, C : F
                                    DataRow drNestedClone = dtNestedClone.NewRow();
                                    drNestedClone["LevelName"] = (string)drExistingNested["LevelName"] + " : null";
                                    drNestedClone["LevelValues"] = (string)drExistingNested["LevelValues"] + delim + "null:" + drCharValue[valueReferenceName].ToString();  // Embed characteristic group RID with null value to be used in query
                                    if (filterStoreFieldType != null)
                                    {
                                        drNestedClone["LevelFields"] = (string)drExistingNested["LevelFields"] + delim + filterStoreFieldType.dbIndex.ToString();
                                    }
                                    else
                                    {
                                        drNestedClone["LevelFields"] = (string)drExistingNested["LevelFields"] + delim + "-99";
                                    }

                                    drNestedClone["OBJECT_TYPE"] = storeObjectType.Index;
                                    dtNestedClone.Rows.Add(drNestedClone);
                                }
                            }
                        }
                    }
                    dtNested.Rows.Clear();
                    foreach (DataRow drClone in dtNestedClone.Rows)
                    {
                        DataRow drNested = dtNested.NewRow();
                        drNested["LevelName"] = (string)drClone["LevelName"];
                        drNested["LevelValues"] = (string)drClone["LevelValues"];
                        drNested["LevelFields"] = (string)drClone["LevelFields"];
                        drNested["OBJECT_TYPE"] = (int)drClone["OBJECT_TYPE"];
                        dtNested.Rows.Add(drNested);
                    }

                    firstDynamicGroup = false;
                } //if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic)

            } //foreach (ConditionNode cn in conditionRoot.ConditionNodes)


            //Put results from the nested table as group levels (aka attribute sets)
            int levelSeq = 0; //start level sequence at zero
            foreach (DataRow drNested in dtNested.Rows)
            {
                eGroupLevelTypes levelType = eGroupLevelTypes.DynamicSet;
                levelSeq++;
                string levelName = (string)drNested["LevelName"];
                string levelValues = (string)drNested["LevelValues"];
                //int levelRID = groupData.StoreGroupLevel_Insert(levelSeq, groupRID, levelName, isActive: true, conditionRID: -1, levelType: (int)levelType);
                levelInfo li = new levelInfo(-1, levelName, levelSeq, -1, levelType, 1);
                li.levelValues = levelValues;
                li.storeObjectType = storeObjectTypes.FromIndex((int)drNested["OBJECT_TYPE"]);
                li.levelFields = (string)drNested["LevelFields"];
                tempLevelList.Add(li); //put in a list so we can add to the profile after the db changes are committed.       
            }
        }

        private static StoreGroupLevelProfile StoreGroup_AddLevelToGroup(ref StoreGroupProfile groupProf, int levelRID, string levelName, int levelSeq, int levelVersion, eGroupLevelTypes levelType)
        {

            StoreGroupLevelProfile levelProf = new StoreGroupLevelProfile(levelRID);
            levelProf.GroupRid = groupProf.Key;
            levelProf.Name = levelName;
            levelProf.Sequence = levelSeq;
            levelProf.LevelVersion = levelVersion;
            levelProf.LevelType = levelType;

            //groupProf.Filled = true;
            groupProf.GroupLevels.Add(levelProf);
            _levelList.Add(levelProf);  //_groupLevelHash.Add(stGroupLevel.Key, stGroupLevel);         

            return levelProf;
        }
        // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
        //public static StoreGroupProfile StoreGroup_AddOrUpdate(filter f, bool isNewGroup, bool loadNewResults)
        public static StoreGroupProfile StoreGroup_AddOrUpdate(filter f, bool isNewGroup, bool loadNewResults, bool refreshSessions = true)
        // End TT#2078-MD - JSmith - Object Reference error updating Store Group
        {
            StoreGroupProfile groupProf = null;
            try
            {
                //Refresh store profiles
                // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
                //StoreProfiles_RefreshFromService();
                StoreProfiles_RefreshFromService(refreshSessions);
                // End TT#2078-MD - JSmith - Object Reference error updating Store Group

                //Add levels from filter into a temp list
                List<levelInfo> tempLevelList = new List<levelInfo>();
                if (f.filterType == filterTypes.StoreGroupFilter)
                {
                    StoreGroup_CreateLevelsForStaticFilter(ref tempLevelList, f);
                }
                else if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    StoreGroup_CreateLevelsForDynamicFilter(ref tempLevelList, f);
                }

                int groupRID = -1;
                int groupVersion = -1;
                if (isNewGroup == false)
                {
                    groupProf = StoreMgmt.StoreGroup_GetFromFilter(f.filterRID);

                    if (groupProf == null)
                    {
                        return null;
                    }

                    groupRID = groupProf.Key;
                    groupVersion = groupProf.Version;
                }


                //Add Available Stores level
                string availLevelName = Include.AvailableStoresGroupLevelName;
                if (groupRID == Include.AllStoreGroupRID)
                {
                    availLevelName = "All Stores Set";
                }
                int availLevelVersion = 1;

                tempLevelList.Add(new levelInfo(-1, availLevelName, int.MaxValue, -1, eGroupLevelTypes.AvailableStoreSet, availLevelVersion)); //put in a list so we can add to the profile after the db changes are committed.

                //Execute filter results


                DataSet dsResults = filterEngineSQLForStoreGroup.ExecuteFilter(ref tempLevelList, f, groupRID, groupVersion);

                if (dsResults == null)
                {
                    return null;
                }


                DataTable dtFinalLevels = dsResults.Tables[0];
                DataTable dtGroupInfo = dsResults.Tables[1];
                groupRID = (int)dtGroupInfo.Rows[0]["SG_RID"];
                groupVersion = (int)dtGroupInfo.Rows[0]["SG_VERSION"];


                //Add/update the group profile from the filter
                bool renamingGroup = false;  // TT#1923-MD - JSmith - Store Attribute name not holding after change.  Close and Reopen application the name changes back to the original name.
                if (isNewGroup)
                {
                    groupProf = new StoreGroupProfile(groupRID);
                }
                // Begin TT#1923-MD - JSmith - Store Attribute name not holding after change.  Close and Reopen application the name changes back to the original name.
                else if (groupProf.Name != f.filterName)
                {
                    renamingGroup = true;
                }
                // End TT#1923-MD - JSmith - Store Attribute name not holding after change.  Close and Reopen application the name changes back to the original name.
                bool isDynamic = false;
                if (f.filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    isDynamic = true;
                }
                groupProf.Name = f.filterName;
                groupProf.OwnerUserRID = f.ownerUserRID;
                groupProf.FilterRID = f.filterRID;
                groupProf.IsDynamicGroup = isDynamic;
                groupProf.Version = groupVersion;

                if (isNewGroup)
                {
                    _groupList.Add(groupProf);
                    if (groupProf.IsDynamicGroup)
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.DynamicStoreGroup, "Added: " + groupProf.Name);
                    }
                    else
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StaticStoreGroup, "Added: " + groupProf.Name);
                    }
                }
                else
                {
                    //remove old group profile from the cached list
                    //remove old levels from the cached list
                    //ArrayList groupLevelKeyList = new ArrayList();
                    //StoreGroupProfile sgOld = (StoreGroupProfile)_groupList.FindKey(groupRID);
                    //foreach (StoreGroupLevelProfile sglp in sgOld.GroupLevels)
                    //{
                    //    groupLevelKeyList.Add(sglp.Key);
                    //}

                    //foreach (int key in groupLevelKeyList)
                    //{
                    //    StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sgOld.GroupLevels.FindKey(key);
                    //    sgOld.GroupLevels.Remove(sgl);
                    //    _levelList.Remove(sgl);
                    //}
                    StoreGroup_RemoveCurrentLevels(groupRID);
                    StoreGroupProfile sgOld = (StoreGroupProfile)_groupList.FindKey(groupRID);
                    _groupList.Remove(sgOld);

                    //Now add the updated group profile
                    _groupList.Add(groupProf);
                    // Begin TT#1923-MD - JSmith - Store Attribute name not holding after change.  Close and Reopen application the name changes back to the original name.
                    if (renamingGroup)
                    {
                        StoreMgmt.StoreGroup_Rename(groupRID, groupProf.Name);
                    }
                    // End TT#1923-MD - JSmith - Store Attribute name not holding after change.  Close and Reopen application the name changes back to the original name.
                    if (groupProf.IsDynamicGroup)
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.DynamicStoreGroup, "Updated: " + groupProf.Name);
                    }
                    else
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StaticStoreGroup, "Updated: " + groupProf.Name);
                    }
                }


                StoreGroupMaint groupData = new StoreGroupMaint();
                DataTable dtResultsForGroup = null;
                if (loadNewResults)
                {
                    dtResultsForGroup = groupData.StoreGroupLevelResults_ReadLatest(groupRID);  //Read the store results for the levels
                }
                //add levels to groups from the final levels table
                foreach (DataRow drLevel in dtFinalLevels.Rows)
                {
					// Begin TT#1517-MD - stodd - protect if "Available Stores" doesn't exist
                    int sglRID = -1;
                    if (drLevel["SGL_RID"] != DBNull.Value)
                    {
                        sglRID = (int)drLevel["SGL_RID"];
                    }
            		// End TT#1517-MD - stodd - protect if "Available Stores" doesn't exist
					
                    string levelName = (string)drLevel["SGL_ID"];
                    int levelSeq = (int)drLevel["SGL_SEQUENCE"];
                    int levelConditionRID = (int)drLevel["CONDITION_RID"];
                    eGroupLevelTypes levelType = (eGroupLevelTypes)((int)drLevel["LEVEL_TYPE"]);

                    StoreGroupLevelProfile levelProf = StoreGroup_AddLevelToGroup(ref groupProf, sglRID, levelName, levelSeq, levelConditionRID, levelType);
                    levelProf.LevelVersion = (int)drLevel["SGL_VERSION"];  // TT#1945-MD - JSmith - Store Attributes - Dynamic - Same Store in 2 Sets in the Store Explorer

                    if (loadNewResults)
                    {
                        DataRow[] drResultsForLevel = dtResultsForGroup.Select("SGL_RID=" + sglRID.ToString());
                        foreach (DataRow drResult in drResultsForLevel)
                        {
                            int storeRID = (int)drResult["ST_RID"];
                            StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
                            levelProf.Stores.Add(sp);
                        }
                    }
                }
                if (loadNewResults)
                {
                    groupProf.LevelsAndStoresFilled = true;
                }
                else
                {
                    groupProf.LevelsAndStoresFilled = false;
                }


                StoreGroup_SortList();
            }
            catch (Exception ex)
            {
                
                ExceptionHandler.HandleException(ex);
            }
            return groupProf;
        }

        private static void StoreGroup_RemoveCurrentLevels(int groupRID)
        {
            //remove old levels from the cached list
            ArrayList groupLevelKeyList = new ArrayList();
            StoreGroupProfile sgOld = (StoreGroupProfile)_groupList.FindKey(groupRID);
            // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
            if (sgOld == null)
            {
                return;
            }
            // End TT#2078-MD - JSmith - Object Reference error updating Store Group
            foreach (StoreGroupLevelProfile sglp in sgOld.GroupLevels)
            {
                groupLevelKeyList.Add(sglp.Key);
            }

            foreach (int key in groupLevelKeyList)
            {
                StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sgOld.GroupLevels.FindKey(key);
                sgOld.GroupLevels.Remove(sgl);
                _levelList.Remove(sgl);
            }
        }
        private static void StoreGroup_FillStoreResults(int groupRID)
        {
            StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRID);

            StoreGroupMaint groupData = new StoreGroupMaint();
            DataTable dtResultsForGroup = groupData.StoreGroupLevelResults_ReadVersion(groupRID, sg.Version);  //Read the store results for the levels for this version

            foreach (StoreGroupLevelProfile levelProf in sg.GroupLevels)
            {
                levelProf.Stores.Clear();
                DataRow[] drResultsForLevel = dtResultsForGroup.Select("SGL_RID=" + levelProf.Key.ToString());
                foreach (DataRow drResult in drResultsForLevel)
                {
                    int storeRID = (int)drResult["ST_RID"];
                    StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
                    levelProf.Stores.Add(sp);
                }
            }
            sg.LevelsAndStoresFilled = true;
        }

        public static void StoreGroup_UpdateIdAndUser(StoreGroupProfile sgp)
        {
            try
            {
                StoreGroupMaint groupData = new StoreGroupMaint();
                //AcquireStoreGroupWriterLock();
                try
                {
                    groupData.OpenUpdateConnection();
                    //_storeGroupRelations.UpdateStoreGroup(sgp);


                    groupData.StoreGroup_UpdateIdAndUser(sgp.Key, sgp.Name, sgp.OwnerUserRID);

                    StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(sgp.Key);
                    sg.Name = sgp.Name;
                    sg.OwnerUserRID = sgp.OwnerUserRID;

                    // Begin TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors
                    StoreGroup_SortList();


                    groupData.CommitData();

                }
                finally
                {
                    // Ensure that the lock is released.
                    //ReleaseStoreGroupWriterLock();
                    groupData.CloseUpdateConnection();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The reader lock request timed out.
                //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:UpdateStoreGroup reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "StoreManagement:UpdateStoreGroup reader lock has timed out");
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }

        public static void StoreGroup_SetInactive(int groupRID)
        {
            try
            {
                StoreGroupMaint groupData = new StoreGroupMaint();
                //AcquireStoreGroupWriterLock();
                try
                {
                    ArrayList groupLevelKeyList = new ArrayList();
                    StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRID);
                    foreach (StoreGroupLevelProfile sglp in sg.GroupLevels)
                    {
                        groupLevelKeyList.Add(sglp.Key);
                    }


                    groupData.OpenUpdateConnection();
                    groupData.StoreGroup_SetInactive(groupRID);
                    groupData.CommitData();

                    foreach (int key in groupLevelKeyList)
                    {
                        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels.FindKey(key);
                        sg.GroupLevels.Remove(sgl);
                        // remove from hash
                        _levelList.Remove(sgl);
                    }

                    //Audit Message
                    if (sg.IsDynamicGroup)
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.DynamicStoreGroup, "Removed: " + sg.Name);
                    }
                    else
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StaticStoreGroup, "Removed: " + sg.Name);
                    }

                    // remove from list
                    _groupList.Remove(sg);


                }
                finally
                {
                    //ReleaseStoreGroupWriterLock();
                    groupData.CloseUpdateConnection();
                }
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Warning); //Issue 4585
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }

        }

        // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
        public static void StoreGroup_Delete(int groupRID)
        {
            try
            {
                StoreGroupMaint groupData = new StoreGroupMaint();
                try
                {
                    ArrayList groupLevelKeyList = new ArrayList();
                    StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRID);
                    foreach (StoreGroupLevelProfile sglp in sg.GroupLevels)
                    {
                        groupLevelKeyList.Add(sglp.Key);
                    }

                    groupData.OpenUpdateConnection();
                    groupData.StoreGroup_Delete(groupRID);
                    groupData.CommitData();

                    foreach (int key in groupLevelKeyList)
                    {
                        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels.FindKey(key);
                        sg.GroupLevels.Remove(sgl);
                        // remove from hash
                        _levelList.Remove(sgl);
                    }

                    //Audit Message
                    if (sg.IsDynamicGroup)
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.DynamicStoreGroup, "Deleted: " + sg.Name);
                    }
                    else
                    {
                        StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StaticStoreGroup, "Deleted: " + sg.Name);
                    }

                    // remove from list
                    _groupList.Remove(sg);
                }
                finally
                {
                    groupData.CloseUpdateConnection();
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }

        }
        // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

        public static void StoreGroup_Rename(int groupRID, string newName)
        {

            try
            {
                // on DB
                StoreGroupMaint groupData = new StoreGroupMaint();
                groupData.StoreGroup_Update(groupRID, newName);

                // rename in store group list
                StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRID);
                sg.Name = newName;

                //filter on DB
                int filterRID = sg.FilterRID;
                groupData.StoreGroupFilter_UpdateName(filterRID, newName);


                StoreGroup_SortList();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		// Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
		//public static void StoreGroupLevel_Rename(int groupLevelRID, int groupLevelVersion, string newName)
        public static void StoreGroupLevel_Rename(int groupLevelRID, int groupLevelVersion, string newName, bool updateStoreGroupLevel = false)
		// End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
        {
            try
            {
                //filter
                StoreGroupLevelProfile sgl = _levelList.Find(x => x.Key == groupLevelRID);
                int groupRID = sgl.GroupRid;
                StoreGroupProfile sg = (StoreGroupProfile)_groupList.FindKey(groupRID);
                int filterRID = sg.FilterRID;
                string oldLevelName = sgl.Name;
                StoreGroupMaint groupData = new StoreGroupMaint();
                //Update static filter level names based on filter rid and old name
                groupData.StoreGroupFilter_UpdateLevelName(filterRID, oldLevelName, newName);

                // on DB

                // Begin TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
				//public static void StoreGroupLevel_Rename(int groupLevelRID, int groupLevelVersion, string newName)
                groupData.StoreGroupLevel_Update(groupLevelRID, groupLevelVersion, newName, updateStoreGroupLevel);
				// End TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.

                // in store group list and hash

                sgl.Name = newName;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public static void StoreGroupLevel_UpdateSequence(int groupLevelRID, int groupLevelVersion, int sglSeq)
        {
            try
            {
                // update database
                StoreGroupMaint groupData = new StoreGroupMaint();
                groupData.StoreGroupLevel_UpdateSequence(groupLevelRID, groupLevelVersion, sglSeq);

                // update store group list and hash
                //StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
                StoreGroupLevelProfile sgl = _levelList.Find(x => x.Key == groupLevelRID);
                sgl.Sequence = sglSeq;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		// Begin TT#1517-MD - stodd - new sets not getting added to database
        public static void StoreGroupJoin_FirstTimeInit()
        {
            try
            {
                StoreGroupMaint groupData = new StoreGroupMaint();

                try
                {
                    groupData.OpenUpdateConnection();

                    groupData.StoreGroupJoin_FirstTimeInit();

                    groupData.CommitData();
                }
                finally
                {
                    groupData.CloseUpdateConnection();
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		// End TT#1517-MD - stodd - new sets not getting added to database

        public static void StoreGroup_SortLevels(int storeGroupRid)
        {
            StoreGroupProfile sg = StoreGroup_Get(storeGroupRid);

            sg.GroupLevels.ArrayList.Sort(new SGLSequenceComparer());
        }
        public static StoreGroupProfile StoreGroup_Copy(StoreGroupProfile groupProf, string newGroupName, int newUserRid, ref filter newFilter)
        {

            try
            {
                int currentGroupRID = groupProf.Key;
                int currentFilterRID = groupProf.FilterRID;

                //copy the filter first
                FilterData _dlFilters = new FilterData();
                filter currentFilter = filterDataHelper.LoadExistingFilter(currentFilterRID);

                int newFilterRID = _dlFilters.InsertFilter(currentFilter.filterType, newUserRid, newUserRid, newGroupName, currentFilter.isLimited, (int)currentFilter.resultLimit);

                //read conditions to datatable
                DataTable dtConditions = _dlFilters.FilterReadConditions(currentFilterRID);
                DataTable dtListValues = _dlFilters.FilterReadListValues(currentFilterRID);

                foreach (DataRow drCondition in dtConditions.Rows)
                {
                    int oldConditionRID = (int)drCondition["CONDITION_RID"];
                    drCondition["FILTER_RID"] = newFilterRID;

                    filterCondition fc = new filterCondition();
                    fc.LoadFromDataRow(drCondition);
                    //copy the name to the name condition
                    if (fc.dictionaryIndex == filterDictionary.FilterName)
                    {
                        fc.valueToCompare = newGroupName;
                    }
                    if (fc.dictionaryIndex == filterDictionary.FilterFolder)
                    {
                        //if (filterProf.UserRID == Include.GlobalUserRID)
                        //{
                        //    fc.valueToCompareInt = Include.GlobalUserRID;
                        //}
                        //else
                        //{
                        //    fc.valueToCompareInt = -1;
                        //}
                        fc.valueToCompareInt = newUserRid;//currentFilter.ownerUserRID; //we need to update the condition so we can build formatted text
                    }
					// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
					//int newConditionRID = _dlFilters.InsertCondition(fc.conditionFilterRID, fc.Seq, fc.ParentSeq, fc.SiblingSeq, fc.dictionaryIndex, fc.logicIndex, fc.fieldIndex, fc.operatorIndex, fc.valueTypeIndex, fc.dateTypeIndex, fc.numericTypeIndex, fc.valueToCompare, fc.valueToCompareDouble, fc.valueToCompareDouble2, fc.valueToCompareInt, fc.valueToCompareInt2, fc.valueToCompareBool, fc.valueToCompareDateFrom, fc.valueToCompareDateTo, fc.valueToCompareDateBetweenFromDays, fc.valueToCompareDateBetweenToDays, fc.variable1_Index, fc.variable1_VersionIndex, fc.variable1_HN_RID, fc.variable1_CDR_RID, fc.variable1_VariableValueTypeIndex, fc.variable1_TimeTypeIndex, fc.operatorVariablePercentageIndex, fc.variable2_Index, fc.variable2_VersionIndex, fc.variable2_HN_RID, fc.variable2_CDR_RID, fc.variable2_VariableValueTypeIndex, fc.variable2_TimeTypeIndex, fc.headerMerchandise_HN_RID, fc.sortByTypeIndex, fc.sortByFieldIndex, fc.listConstantType.dbIndex);
                    int newConditionRID = _dlFilters.InsertCondition(fc.conditionFilterRID, fc.Seq, fc.ParentSeq, fc.SiblingSeq, fc.dictionaryIndex, fc.logicIndex, fc.fieldIndex, fc.operatorIndex, fc.valueTypeIndex, fc.dateTypeIndex, fc.numericTypeIndex, fc.valueToCompare, fc.valueToCompareDouble, fc.valueToCompareDouble2, fc.valueToCompareInt, fc.valueToCompareInt2, fc.valueToCompareBool, fc.valueToCompareDateFrom, fc.valueToCompareDateTo, fc.valueToCompareDateBetweenFromDays, fc.valueToCompareDateBetweenToDays, fc.variable1_Index, fc.variable1_VersionIndex, fc.variable1_HN_RID, fc.variable1_CDR_RID, fc.variable1_VariableValueTypeIndex, fc.variable1_TimeTypeIndex, fc.operatorVariablePercentageIndex, fc.variable2_Index, fc.variable2_VersionIndex, fc.variable2_HN_RID, fc.variable2_CDR_RID, fc.variable2_VariableValueTypeIndex, fc.variable2_TimeTypeIndex, fc.headerMerchandise_HN_RID, fc.sortByTypeIndex, fc.sortByFieldIndex, fc.listConstantType.dbIndex, fc.date_CDR_RID);
					// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only


                    DataRow[] drListValues = dtListValues.Select("CONDITION_RID=" + oldConditionRID);

                    DataTable dtNewListValues = filterCondition.GetListValuesDataTable();
                    foreach (DataRow drListValue in drListValues)
                    {
                        DataRow drNew = dtNewListValues.NewRow();
                        filterUtility.DataRowCopy(drListValue, drNew);
                        drNew["CONDITION_RID"] = newConditionRID;
                        dtNewListValues.Rows.Add(drNew);
                    }
                    _dlFilters.InsertListValues(dtNewListValues);
                }


                //filter f = new filter(filterProf.UserRID, filterProf.OwnerUserRID);
                //f.filterName = filterProf.Name;
                //f.filterRID = filterProf.Key;
                //newNode = AfterSave(f);
                newFilter = filterDataHelper.LoadExistingFilter(newFilterRID);



                StoreGroupProfile sgp = StoreGroup_AddOrUpdate(newFilter, true, true);

                //StoreGroupMaint groupData = new StoreGroupMaint();
                //newGroupRID = groupData.CopyStoreGroup(currentGroupRID, newGroupName, newUserRid); //TODO copy filter - this also sometimes makes a USER_ITEM entry

                //TODO make store groups from filter

                // BuildStoreGroup(groupRID);
                // Begin TT#20 - JSmith - Move fails after copy
                //return GetStoreGroup(groupRID);
                // StoreGroupProfile sgp = GetStoreGroup(newGroupRID);
                //RefreshStoresInGroup(groupRID);
                return sgp;
                // End TT#20
            }
            catch
            {
                throw;
            }

        }
        public static bool DoesGroupNameExist(int groupRid, string name, int ownerRID)
        {
            bool exists = false;

            StoreGroupProfile sgp = (StoreGroupProfile)_groupList.FindKey(groupRid);


            if (sgp.Name.ToLower() == name.ToLower())
            {
                exists = true;
            }


            return exists;
        }
        public static bool DoesGroupNameExist(string name, int aUserRID)
        {
            // Begin TT#1928-MD - JSmith - Store Explorer allow user to rename attribute to existing name
            //bool exists = false;
            //int groupCnt = _groupList.ArrayList.Count;
            //for (int i = 0; i < groupCnt; i++)
            //{
            //    StoreGroupProfile sgp = (StoreGroupProfile)_groupList[i];
            //    //Begin Track #4880 - JScott - Error encountered when selecting "Region" attribute
            //    //				if (sgp.Name == name)
            //    // Begin Track #4872 - JSmith - Global/User Attributes
            //    //if (sgp.Name.ToLower() == name.ToLower())
            //    if (sgp.Name.ToLower() == name.ToLower() &&
            //        sgp.OwnerUserRID == aUserRID)
            //    // End Track #4872
            //    //End Track #4880 - JScott - Error encountered when selecting "Region" attribute
            //    {
            //        exists = true;
            //        break;
            //    }
            //}

            //return exists;
            // End TT#1928-MD - JSmith - Store Explorer allow user to rename attribute to existing name
            return StoreMgmt.StoreGroup_GetRidFromId(name, aUserRID) != Include.NoRID;
        }
        // End TT#1928-MD - JSmith - Store Explorer allow user to rename attribute to existing name
        public static bool DoesGroupLevelNameExist(int groupRid, string name)
        {
            bool exists = false;

            StoreGroupProfile sgp = (StoreGroupProfile)_groupList.FindKey(groupRid);

            int levelCnt = sgp.GroupLevels.Count;
            for (int i = 0; i < levelCnt; i++)
            {
                StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sgp.GroupLevels[i];

                if (sglp.Name.ToLower() == name.ToLower())
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
        private static ProfileList StoreGroup_GetListWithoutStores(int aUserRID)
        {
            try
            {
                //sharedGroups = GetSharedGroups(aUserRID);
                Hashtable sharedGroups = new Hashtable();
                StoreGroupMaint groupData = new StoreGroupMaint();
                DataTable dt = groupData.SharedStoreGroups_Read(aUserRID);
                foreach (DataRow dr in dt.Rows)
                {
                    sharedGroups.Add(Convert.ToInt32(dr["SG_RID"]), null);
                }





                // Begin TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors
                //AcquireWriterLock();
                AcquireReaderLock();
                // End TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors

                try
                {
                    ProfileList clonedStoreGroupList = new ProfileList(eProfileType.StoreGroup);
                    //ProfileList storeGroupList = _storeGroupRelations.GetStoreGroupList();
                    int groupCnt = _groupList.Count;
                    for (int i = 0; i < groupCnt; i++)
                    {
                        StoreGroupProfile sgp = (StoreGroupProfile)((StoreGroupProfile)_groupList[i]).Clone();
                        if (sgp.OwnerUserRID == Include.GlobalUserRID ||
                            sgp.OwnerUserRID == aUserRID ||
                            sharedGroups.ContainsKey(sgp.Key))
                        {
                            sgp.Visible = true;
                        }
                        else
                        {
                            sgp.Visible = false;
                        }
                        clonedStoreGroupList.Add(sgp);
                    }
                    return clonedStoreGroupList;
                }
                finally
                {
                    // Ensure that the lock is released.
                    // Begin TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors
                    //ReleaseWriterLock();
                    ReleaseReaderLock();
                    // End TT#3352 - JSmith - ANF Adults - AcquireWriterLock errors
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                // End TT#189
                // The reader lock request timed out.
                //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetStoreGroupList reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "StoreManagement:GetStoreGroupList reader lock has timed out");
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
                if (_audit != null)
                {
                    _audit.Log_Exception(ex, _module);
                }
                throw;
            }
        }
        public static ProfileList StoreGroup_GetList()
        {
            try
            {
                AcquireReaderLock();
                return _groupList;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupList(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
            finally
            {
                ReleaseReaderLock();
            }
        }

        // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
        /// <summary>
        /// Returns a list in the smaller format of the StoreGroupListViewProfile
        /// </summary>
        /// <param name="aStoreGroupSelectType">
        /// A filter value of store groups to select.
        /// </param>
        /// <returns>
        /// A ProfileList of StoreGroupListViewProfiles for the groups that were selected.
        /// </returns>
        public static ProfileList StoreGroup_GetListViewList(int aUserRID)
        {
            bool selectGroup = false;
            SecurityAdmin secAdmin = new SecurityAdmin();
            ProfileList storeGroupList;
            try
            {
                storeGroupList = StoreGroup_GetListWithoutStores(aUserRID);
                ProfileList pl = new ProfileList(eProfileType.StoreGroupListView);
                int groupCnt = _groupList.Count;
                for (int i = 0; i < groupCnt; i++)
                {
                    StoreGroupProfile sgp = (StoreGroupProfile)storeGroupList[i];
                    selectGroup = true;
                    if (sgp.OwnerUserRID != aUserRID)
                    {
                        selectGroup = false;
                    }

                    if (selectGroup)
                    {
                        StoreGroupListViewProfile view = new StoreGroupListViewProfile(sgp.Key);
                        view.Name = sgp.Name;
                        view.IsDynamicGroup = sgp.IsDynamicGroup;
                        view.OwnerUserRID = sgp.OwnerUserRID;
                        view.FilterRID = sgp.FilterRID;
                        view.Version = sgp.Version;
                        pl.Add(view);
                    }
                }
                pl.ArrayList.Sort(new SGLVSequenceComparer());
                return pl;
            }
            catch (Exception err)
            {
                //string msg = "GetStoreGroupListViewList(): " + err.ToString();
                //EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Error);
                //if (Audit != null)
                //{
                //    Audit.Log_Exception(err, GetType().Name);
                //}
                throw;
            }
        }
        // End Track #6302
        #endregion

        public static int StoreCharGroup_Insert(ref List<MIDMsg> em, ref bool didInsertNewDynamicStoreGroup, ref int newFilterRid, bool addInfoMsgUponInsert, int userRID, string charGroupName, eStoreCharType aCharType, bool hasList, string storeId = "")
        {
            didInsertNewDynamicStoreGroup = false;
            // Begin TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
            StoreGroupMaint groupMaint = new StoreGroupMaint();
            if (groupMaint.DoesStoreGroupNameAlreadyExist(charGroupName, Include.GlobalUserRID) == true)
            {
                string msgDetails = string.Empty;
                if (storeId != string.Empty)
                {
                    msgDetails += "Store: " + storeId;
                }
                msgDetails += " Duplicate Attribute Exists: " + charGroupName;
                em.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msg = msgDetails });
                return Include.NoRID;
            }
            // End TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
            // Begin TT#1933-MD - JSmith - SVC - Store Profiles select Edit Fields and receive Data Duplicate Name Exception
            // Check against Store Fields
            else if (StoreMgmt.DoesStoreCharGroupNameAlreadyExist(charGroupName, Include.NoRID))
            {
                string msgDetails = string.Empty;
                if (storeId != string.Empty)
                {
                    msgDetails += "Store: " + storeId;
                }
                msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_DuplicateCharGroupName) + ": " + charGroupName;
                em.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateCharGroupName, msg = msgDetails });
                return Include.NoRID;
            }
            // End TT#1933-MD - JSmith - SVC - Store Profiles select Edit Fields and receive Data Duplicate Name Exception
            StoreCharMaint charMaint = new StoreCharMaint();
            int scgRID = charMaint.StoreCharGroup_Insert(charGroupName, aCharType, hasList: hasList);
            StoreMgmt.AuditMessage_Add(StoreMgmt.AuditCodesForStoreMgmt.StoreCharacteristic, "Added: " + charGroupName);
            if (addInfoMsgUponInsert)
            {
                string msgDetails = MIDText.GetText(eMIDTextCode.msg_AutoAddedCharacteristic, false);
                msgDetails = msgDetails.Replace("{0}", charGroupName);
                em.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Information, textCode = eMIDTextCode.msg_AutoAddedCharacteristic, msg = msgDetails });
            }

            //Automatically add a dynamic store attribute that corresponds with the new store characteristic
            // Begin TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
            //StoreGroupMaint groupMaint = new StoreGroupMaint();
            //if (groupMaint.DoesStoreGroupNameAlreadyExist(charGroupName, Include.GlobalUserRID) == true)
            //{
            //    string msgDetails = string.Empty;
            //    if (storeId != string.Empty)
            //    {
            //        msgDetails += "Store: " + storeId;
            //    }
            //    msgDetails += " Duplicate Attribute Exists: " + charGroupName;
            //    em.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msg = msgDetails });

            //}
            //else
            // End TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
            {
                newFilterRid = StoreGroup_InsertDynamic(scgRID, charGroupName, userRID);
                didInsertNewDynamicStoreGroup = true;
            }
            return scgRID;
        }

        public static void StoreCharGroup_Update(int scgRID, string charGroupName, eStoreCharType aCharType, bool hasList)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            charMaint.StoreCharGroup_Update(scgRID, charGroupName, aCharType, hasList);

            //Name (aka ID) changes propogate to store groups during the validation
            //Type changes are not allowed if there are values
        }
        public static void StoreCharGroup_Delete(int scgRid)
        {
            // Begin TT#2131-MD - JSmith - Halo Integration
            string Characteristic = string.Empty;
            if (_SAB.ROExtractEnabled)
            {
                // Get ID
                StoreCharMaint charMaint = new StoreCharMaint();
                DataTable dtCharGroup = charMaint.StoreCharGroup_Read(scgRid);

                if (dtCharGroup != null
                    && dtCharGroup.Rows.Count > 0)
                {
                    Characteristic = Convert.ToString(dtCharGroup.Rows[0]["SCG_ID"]);
                }
            }
            // End TT#2131-MD - JSmith - Halo Integration

            StoreCharMaint storeCharMaint = new StoreCharMaint();
            storeCharMaint.StoreCharGroup_Delete(scgRid);

            //In Use for char groups should already have been checked before here

            //Handle dynamic groups by marking them inactive
            StoreMgmt.StoreGroup_SetDynamicGroupsInactiveForStoreCharGroup(scgRid);

            // Begin TT#2131-MD - JSmith - Halo Integration
            if (_SAB.ROExtractEnabled
                && !string.IsNullOrEmpty(Characteristic))
            {
                // Delete from extract table if succesfully deleted fro RO database
                ROExtractData ROExtractData = null;
                try
                {
                    ROExtractData = new ROExtractData(_SAB.ROExtractConnectionString);
                    ROExtractData.Stores_Characteristics_Delete(Characteristic);
                    ROExtractData.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    if (ROExtractData.ConnectionIsOpen)
                    {
                        ROExtractData.CloseUpdateConnection();
                    }
                }
            }
            // End TT#2131-MD - JSmith - Halo Integration

        }

        public static int StoreCharGroup_Find(string charGroupName)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            DataRow drCharGroup = charMaint.StoreCharGroupReadFromId(charGroupName);

            int scgRID = Include.NoRID;

            if (drCharGroup != null)
            {
                scgRID = (int)drCharGroup["SCG_RID"];
            }

            return scgRID;
        }

        // Begin TT#1865-MD - JSmith - Duplicates Characteristic Values
        public static eStoreCharType StoreCharGroup_GetType(int scgRid)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            DataTable dtCharGroup = charMaint.StoreCharGroup_Read(scgRid);

            eStoreCharType storeCharType = eStoreCharType.unknown;

            if (dtCharGroup != null 
                && dtCharGroup.Rows.Count > 0)
            {
                storeCharType = (eStoreCharType)Convert.ToInt32(dtCharGroup.Rows[0]["SCG_TYPE"]);
            }

            return storeCharType;
        }
        // End TT#1865-MD - JSmith - Duplicates Characteristic Values

        public static bool DoesStoreCharGroupNameAlreadyExist(string proposedName, int charGroupEditRID)
        {
            // Begin TT#1933-MD - JSmith - SVC - Store Profiles select Edit Fields and receive Data Duplicate Name Exception
            storeFieldTypes storeField = storeFieldTypes.FromString(proposedName);
            if (storeField != null)
            {
                return true;
            }
            // End TT#1933-MD - JSmith - SVC - Store Profiles select Edit Fields and receive Data Duplicate Name Exception

            StoreCharMaint charMaint = new StoreCharMaint();
            return charMaint.DoesStoreCharGroupNameAlreadyExist(proposedName, charGroupEditRID); //also check for duplicate store group names
        }
        public static DataTable StoreCharGroup_ReadValues(int scgRID)
        {
            StoreCharMaint storeCharMaint = new StoreCharMaint();
            return storeCharMaint.StoreCharGroup_ReadValues(scgRID);
        }


        public static int StoreCharValue_Insert(int scgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            int scRID = Include.NoRID;

            StoreCharMaint charMaint = new StoreCharMaint();
            scRID = charMaint.StoreCharValue_Insert(scgRID, textValue, dateValue, numberValue, dollarValue);

            return scRID;
        }
        public static void StoreCharValue_Update(int scRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            charMaint.StoreCharValue_Update(scRID, textValue, dateValue, numberValue, dollarValue);
        }
        public static void StoreCharValue_Delete(int scRid)
        {
            StoreCharMaint storeCharMaint = new StoreCharMaint();
            storeCharMaint.StoreCharValue_Delete(scRid);
        }
        public static bool DoesStoreCharValueAlreadyExist(object proposedValue, fieldDataTypes charGroupFieldType, int charGroupRID, int valueRID, ref int scRID)
        {
            StoreCharMaint charMaintData = new StoreCharMaint();
            bool doesValueAlreadyExist = false;

            if (charGroupFieldType == fieldDataTypes.Text)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueTextAlreadyExist(Convert.ToString(proposedValue), charGroupRID, valueRID, ref scRID);
            }
            else if (charGroupFieldType == fieldDataTypes.DateNoTime)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueDateAlreadyExist(Convert.ToDateTime(proposedValue), charGroupRID, valueRID, ref scRID);
            }
            else if (charGroupFieldType == fieldDataTypes.NumericDouble)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueNumberAlreadyExist(Convert.ToSingle(proposedValue), charGroupRID, valueRID, ref scRID);
            }
            else if (charGroupFieldType == fieldDataTypes.NumericDollar)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueDollarAlreadyExist(Convert.ToSingle(proposedValue), charGroupRID, valueRID, ref scRID);
            }

            return doesValueAlreadyExist;
        }

       

    }
}
