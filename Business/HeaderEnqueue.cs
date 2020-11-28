using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic; // TT#1185 - Verify ENQ before Update
using System.Data;
using System.Text;                // TT#1185 - Verify ENQ before Update
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
// begin TT#1185 - Verify ENQ before Update (re-worked code)
{
    /// <summary>
    /// A Header Enqueue Dictionary. Key: Transaction ID, Member is the associated HeaderEnqueue Object  
    /// </summary>
    [Serializable]
    public class EnqueueDictionary : Dictionary<long, HeaderEnqueue>
    {
    }
    /// <summary>
    /// The HeaderEnqueue class provides functionality for enqueuing 
    /// an Allocation header before accessing a View.
    /// </summary>
    /// <remarks>
    /// A header is only allowed to be in use for update once for all users. 
    /// </remarks> 
    [Serializable]
    public class HeaderEnqueue
    {
        //=======
        // FIELDS
        //=======

        private List<HeaderConflict> _conflictList;
        private Dictionary<int, int> _enqueuedHdrRidDict; 
        private MIDEnqueue _midNQ;
        private int _thisUserRID; 
        private int _clientThreadID;
        private long _transactionID;

        //=============
        // CONSTRUCTORS
        //=============
        // begin TT#1185 - JEllis - Verify ENQ before Update (Part 2)
        /// <summary>
        /// Creates a new instance of HeaderEnqueue using the given information
        /// </summary>
        /// <param name="aThreadID">Client Thread ID that owns the enqueue</param>
        /// <param name="aTransactionID">Transaction ID that owns the enqueue</param>
        /// <param name="aUserRID">User RID that owns the enqueue</param>
        /// <remarks>Client Thread Id and Transaction Id uniquely identify this object since the user ID is identified by the Client Server Session where the Thread ID originates.</remarks>
        public HeaderEnqueue(int aThreadID, long aTransactionID, int aUserRID)
        {
            _midNQ = new MIDEnqueue();
            _conflictList = new List<HeaderConflict>();
            _clientThreadID = aThreadID;
            _transactionID = aTransactionID;
            _thisUserRID = aUserRID;
            _enqueuedHdrRidDict = new Dictionary<int, int>();
        }
        ///// <summary>
        ///// Creates a new instance of HeaderEnqueue using the given information.
        ///// </summary>
        ///// <param name="aClientServerSession">Client Server Session requesting the enqueue</param>
        ///// <remarks>Headers cannot be "owned" by any transaction.  This enqueue is used to lock headers whose non-store data is to be updated.</remarks>
        //public HeaderEnqueue(ClientServerSession aClientServerSession)
        //{
        //    ConstructCommon(aClientServerSession, Include.NoTransaction);
        //}
        ///// <summary>
        ///// Creates a new instance of HeaderEnqueue using the given information.
        ///// </summary>
        ///// <param name="aTrans">Transaction requesting the enqueue.  NOTE:  This transaction must own the headers to be enqueued.  The AllocationHeader MasterProfileList identifies the headers to be enqueued</param>
        ///// <remarks>Headers enqueued with this option must be owned by the given transaction (ie. the constructor used to populate the associated AllocationProfile must include the same transaction in the parameters)</remarks>
        //public HeaderEnqueue(ApplicationSessionTransaction aTrans)
        //{
        //    ConstructCommon(aTrans.SAB.ClientServerSession, aTrans.TransactionID);
        //}
        ///// <summary>
        ///// This ia the common constructor for the HeaderEnqueue object
        ///// </summary>
        ///// <param name="aClientServerSession">The client server session requesting the enqueue.</param>
        ///// <param name="aTransactionID">The transaction ID requesting the enqueue (When the ID is "0", no transaction is present and the associated headers cannot be instantiated with a constructor having a transaction)</param>
        //private void ConstructCommon(ClientServerSession aClientServerSession, long aTransactionID)
        //{
        //    _midNQ = new MIDEnqueue();
        //    _conflictList = new List<HeaderConflict>();
        //    _thisUserRID = aClientServerSession.UserRID;
        //    _clientThreadID = aClientServerSession.ThreadID;
        //    _transactionID = aTransactionID;
        //    _enqueuedHdrRidDict = new Dictionary<int, int>();
        //    _midNQ = new MIDEnqueue();
        //}
        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)
        //===========
        // PROPERTIES
        //===========

        /// <summary>
        /// Returns a boolean indicating if any header is in conflict.
        /// </summary>
        public bool isAnyHeaderInConflict
        {
            get
            {
                return (_conflictList.Count != 0);
            }
        }

        /// <summary>
        /// Returns an array of HeaderConflict objects.
        /// </summary>
        public HeaderConflict[] HeaderConflictList
        {
            get
            {
                return _conflictList.ToArray();
            }
        }
        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
        ///// <summary>
        ///// Returns an array of the header RIDs that are enqueued by this object (enqueue may or may not still exist on the database)
        ///// </summary>
        //public int[] EnqueuedHeaderRIDs
        //{
        //    get
        //    {
        //        int[] enqueuedHeaderRIDs = new int[_enqueuedHdrRidDict.Count];
        //        _enqueuedHdrRidDict.Values.CopyTo(enqueuedHeaderRIDs, 0);
        //        return enqueuedHeaderRIDs;
        //    }
        //}
        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

        //========
        // METHODS
        //========
        /// <summary>
        /// Gets a list of all headers that must be enqueued at same time as the headers provided
        /// </summary>
        /// <param name="aHdrRidList">List of headers that are to be enqueued</param>
        /// <returns>List of all header RIDs that must be enqueued at same time as the provided headers</returns>
        public List<int> GetHdrsToEnq(List<int> aHdrRidList)
        {
            return _midNQ.GetHeadersToENQ(aHdrRidList);
        }
        /// <summary>
        /// Returns "True" if given header RID was enqueued by this object (NOTE:  does not check to see if enqueue is still on database; it could have been removed by ReleaseResouces)
        /// </summary>
        /// <param name="aHdrRID">Header to verify</param>
        /// <returns>True: Header was enqueued by this object; False: Header was NOT enqueued by this object</returns>
        public bool IsHeaderEnqueued(int aHdrRID)
        {
            int hdrRID; 
            return _enqueuedHdrRidDict.TryGetValue(aHdrRID, out hdrRID);
        }

        /// <summary>
        /// This method attempts to enqueue headers.  
        /// If no existing enqueue by another thread/transaction/user exists for any of 
        /// the headers, the headers are enqueued (return is true).  Headers already enqueued by 
        /// THIS thread/transacton/user are ignored and considered successfully enqueued.
        /// If there is an existing enqueue by another thread/transaction/user for any of
        /// the headers, the HeaderConflictList is updated and the enqueue fails (return is false)
        /// </summary>
        /// <param name="aHdrKeyList">List of Header RIDs to enqueue</param>
        public bool EnqueueHeaders(List<int> aHdrKeyList)
        {
            aHdrKeyList.Sort();
            int lastRID = int.MinValue;
            List<int> hdrRidList = new List<int>();
            foreach (int hdrKey in aHdrKeyList)
            {
                if (hdrKey != lastRID)
                {
                    if (!_enqueuedHdrRidDict.TryGetValue(hdrKey, out lastRID))
                    {
                        hdrRidList.Add(hdrKey);
                    }
                    lastRID = hdrKey;
                }
            }
            try
            {
                // BEGIN TT#67-MD - stodd - if list has no entries, created a 0 rid entry in the MID_ENQUEUE table
                if (hdrRidList.Count == 0)
                {
                    return true;
                }
                else
                {
                    if (_midNQ.Header_Enqueue(eLockType.Header, _thisUserRID, _clientThreadID, _transactionID, hdrRidList, out _conflictList))
                    {
                        foreach (int hdrRID in hdrRidList)
                        {
                            _enqueuedHdrRidDict.Add(hdrRID, hdrRID);
                        }
                        return true;
                    }
                }
                // END TT#67-MD - stodd - if list has no entries, created a 0 rid entry in the MID_ENQUEUE table
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }


        /// <summary>
        /// This method dequeues all Allocation header that were enqueued by this thread/transaction/user.
        /// </summary>
        public void DequeueHeaders()
        {
            if (_enqueuedHdrRidDict.Count > 0)
            {

                try
                {
                    _midNQ.OpenUpdateConnection();
                    foreach (int hdrRID in _enqueuedHdrRidDict.Values)
                    {
                        _midNQ.Enqueue_Delete(eLockType.Header, hdrRID, _thisUserRID, _clientThreadID, _transactionID);
                    }
                    _midNQ.CommitData();
                    _enqueuedHdrRidDict.Clear();
                }
                catch (Exception)
                {
                    _midNQ.Rollback();
                    throw;
                }
                finally
                {
                    _midNQ.CloseUpdateConnection();
                }
            }
        }
        /// <summary>
        /// Dequeues the specified header RIDs provided the headers were enqueued by this thread/transaction/user
        /// </summary>
        /// <param name="aHdrRidList">List of headers to dequeue.</param>
        /// <remarks>The dequeue does not fail if one of the headers to be dequeued is not currently enqueued by this thread/transaction/user</remarks>
        public void DequeueHeaders(List<int> aHdrRidList)
        {
            try
            {
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //_midNQ.OpenUpdateConnection();
                _midNQ.OpenUpdateConnection(eLockType.Header);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                foreach (int hdrRID in aHdrRidList)
                {
                    _midNQ.Enqueue_Delete(eLockType.Header, hdrRID, _thisUserRID, _clientThreadID, _transactionID);
                }
                _midNQ.CommitData();
                foreach (int hdrRID in aHdrRidList)
                {
                    _enqueuedHdrRidDict.Remove(hdrRID);
                }
            }
            catch (Exception)
            {
                _midNQ.Rollback();
                throw;
            }
            finally
            {
                _midNQ.CloseUpdateConnection();
            }
        }
        //private SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
        private string headerInUse = string.Empty; //TT#827-MD -jsobek -Allocation Reviews Performance
        private string newLine;
        /// <summary>
        /// Formats a standard Header Enqueue Conflict message.
        /// </summary>
        /// <returns>Header Enqueue Conflict Message</returns>
        public string FormatHeaderConflictMsg()
        {
            if (!MIDEnvironment.isWindows)
            {
                return FormatHeaderConflictWebMsg();
            }
            HeaderConflict[] hc = HeaderConflictList;
            //Begin TT#827-MD -jsobek -Allocation Reviews Performance
            //if (secAdmin == null)
            //{
            //    secAdmin = new SecurityAdmin();
            //    newLine = System.Environment.NewLine;
            //    headerInUse = MIDText.GetText(eMIDTextCode.msg_al_HeadersInUse) + ":" + newLine;
            //}
            if (headerInUse == string.Empty)
            {
                //secAdmin = new SecurityAdmin();
                newLine = System.Environment.NewLine;
                headerInUse = MIDText.GetText(eMIDTextCode.msg_al_HeadersInUse) + ":" + newLine;
            }
            //End TT#827-MD -jsobek -Allocation Reviews Performance

            StringBuilder enqMsg = new StringBuilder();
            enqMsg.Append(headerInUse);

            string userName = string.Empty;
            foreach (HeaderConflict hdrCon in hc)
            {
                // Begin TT#4515 - stodd - enqueue message
#if (DEBUG)
                enqMsg.Append(newLine + "Header: " + hdrCon.HeaderID);
                enqMsg.Append(", Enqueued Time: " + hdrCon.DateTimeEnqueued);
                enqMsg.Append(", User: " + UserNameStorage.GetUserName(hdrCon.UserRID)); //secAdmin.GetUserName(hdrCon.UserRID));
                enqMsg.Append(", Thread: " + hdrCon.ThreadID.ToString(CultureInfo.CurrentUICulture));
                enqMsg.Append(", Trans ID: " + hdrCon.TransactionID.ToString());
#else
                enqMsg.Append(newLine + "Header: " + hdrCon.HeaderID);
                enqMsg.Append(", User: " + UserNameStorage.GetUserName(hdrCon.UserRID)); 
#endif
                // End TT#4515 - stodd - enqueue message
            }
            enqMsg.Append(newLine + newLine);
            return enqMsg.ToString();
        }

        /// <summary>
        /// Formats a standard Header Enqueue Conflict message.
        /// </summary>
        /// <returns>Header Enqueue Conflict Message</returns>
        public string FormatHeaderConflictWebMsg()
        {
            HeaderConflict[] hc = HeaderConflictList;
            if (headerInUse == string.Empty)
            {
                newLine = System.Environment.NewLine;
                headerInUse = MIDText.GetText(eMIDTextCode.msg_al_WorklistItemsInUse) + ":" + newLine;
            }

            StringBuilder enqMsg = new StringBuilder();
            enqMsg.Append(headerInUse);

            string userName = string.Empty;
            string userLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_User);
            string itemLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_WorklistItem);
            foreach (HeaderConflict hdrCon in hc)
            {
#if (DEBUG)
                enqMsg.Append(newLine + itemLabel + ": " + hdrCon.HeaderID);
                //enqMsg.Append(", Enqueued Time: " + hdrCon.DateTimeEnqueued);
                enqMsg.Append(", " + userLabel + ": " + UserNameStorage.GetUserName(hdrCon.UserRID));
                //enqMsg.Append(", Thread: " + hdrCon.ThreadID.ToString(CultureInfo.CurrentUICulture));
                //enqMsg.Append(", Trans ID: " + hdrCon.TransactionID.ToString());
#else
                enqMsg.Append(newLine + itemLabel + ": " + hdrCon.HeaderID);
                enqMsg.Append(", " + userLabel + ": " + UserNameStorage.GetUserName(hdrCon.UserRID)); 
#endif
            }
            enqMsg.Append(newLine);
            return enqMsg.ToString();
        }
    }
}

//{
//    /// <summary>
//    /// The HeaderConflict class stores information regarding a conflict during the enqueue of a header.
//    /// </summary>

//    public struct HeaderConflict
//    {
//        //=======
//        // FIELDS
//        //=======
//        private int _headerRID;
//        private int _userRID;
	
//        //=============
//        // CONSTRUCTORS
//        //=============

//        /// <summary>
//        /// Creates a new instance of HeaderConflict using the user RID of the conflict.
//        /// </summary>
//        /// <param name="aUserRID">
//        /// The user RID of the conflict.
//        /// </param>

//        public HeaderConflict(int aHeaderRID, int aUserRID)
//        {
//            _headerRID = aHeaderRID;
//            _userRID = aUserRID;
//        }

//        //===========
//        // PROPERTIES
//        //===========
//        /// <summary>
//        /// Gets the header RID of the conflict.
//        /// </summary>

//        public int HeaderRID
//        {
//            get
//            {
//                return _headerRID;
//            }
//        }
//        /// <summary>
//        /// Gets the user RID of the conflict.
//        /// </summary>

//        public int UserRID
//        {
//            get
//            {
//                return _userRID;
//            }
//        }

//        //========
//        // METHODS
//        //========
//    }

//    /// <summary>
//    /// The HeaderEnqueue class provides functionality for enqueuing 
//    /// an Allocation header before accessing a View.
//    /// </summary>
//    /// <remarks>
//    /// A header is only allowed to be in use for update once for all users. 
//    /// </remarks> 
	
//    public class HeaderEnqueue
//    {
//        //=======
//        // FIELDS
//        //=======

//        private System.Collections.ArrayList _conflictList;
//        private ApplicationSessionTransaction _trans;
//        private AllocationHeaderProfileList _headerList;
//        //private int[] _hdrIdList;                         // Assortment BEGIN change to ArrayList 
//        private ArrayList _hdrIdList;                       // Assortment END
//        private MIDEnqueue _midNQ;
//        private int _thisUserRID;
//        private int _clientThreadID;
//        private int _headerRID;

//        //=============
//        // CONSTRUCTORS
//        //=============

//        /// <summary>
//        /// Creates a new instance of HeaderEnqueue using the given information.
//        /// </summary>
		
//        public HeaderEnqueue(ApplicationSessionTransaction Trans)
//        {
//            try
//            {
//                _trans = Trans;
//                _headerList =(AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader);
//                _midNQ  = new MIDEnqueue();
//                _conflictList = new System.Collections.ArrayList();
//                _thisUserRID = _trans.SAB.ClientServerSession.UserRID;
//                _clientThreadID = _trans.SAB.ClientServerSession.ThreadID;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//        /// <summary>
//        /// Overload creates a new instance of HeaderEnqueue using the given information.
//        /// </summary>
//        public HeaderEnqueue(ApplicationSessionTransaction Trans, AllocationHeaderProfileList aHdrList)
//        {
//            try
//            {
//                _trans = Trans;
//                _headerList = aHdrList;
//                _midNQ  = new MIDEnqueue();
//                _conflictList = new System.Collections.ArrayList();
//                _thisUserRID = _trans.SAB.ClientServerSession.UserRID;
//                _clientThreadID = _trans.SAB.ClientServerSession.ThreadID;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        /// <summary>
//        /// Overload creates a new instance of HeaderEnqueue using the given information.
//        /// </summary>
//        public HeaderEnqueue(ApplicationSessionTransaction Trans, int aHeaderRID)
//        {
//            try
//            {
//                _trans = Trans;
//                _headerRID = aHeaderRID;
//                _midNQ  = new MIDEnqueue();
//                _conflictList = new System.Collections.ArrayList();
//                _thisUserRID = _trans.SAB.ClientServerSession.UserRID;
//                _clientThreadID = _trans.SAB.ClientServerSession.ThreadID;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//        //===========
//        // PROPERTIES
//        //===========

//        /// <summary>
//        /// Returns a boolean indicating if the header is in conflict.
//        /// </summary>

//        public bool isHeaderInConflict
//        {
//            get
//            {
//                try
//                {
//                    return (_conflictList.Count != 0);
//                }
//                catch (Exception exc)
//                {
//                    string message = exc.ToString();
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// Returns an arraylist of HeaderConflict objects.
//        /// </summary>

//        public System.Collections.ArrayList HeaderConflictList
//        {
//            get
//            {
//                return _conflictList;
//            }
//        }

//        //========
//        // METHODS
//        //========

//        /// <summary>
//        /// This method attempts to enqueue the headers.  If no existing enqueue exists for any header,
//        /// the headers are enqueued.  If there is an existing enqueue,
//        /// the HeaderConflictList is updated and a HeaderConflictExcection is thrown.
//        /// </summary>

//        public void EnqueueHeaders()
//        {						
//            try
//            {
//                 _hdrIdList = new ArrayList();
			
//                foreach (AllocationHeaderProfile ahp in _headerList)
//                {	
//                    _hdrIdList.Add(Convert.ToInt32(ahp.Key, CultureInfo.CurrentUICulture));
//                }
//                Enqueue();
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public void EnqueueHeadersAdded(ArrayList aHdrKeyList)
//        {
//            try
//            {
//                // add to existing header list so dequeue won't change
//                foreach (int key in aHdrKeyList)
//                {
//                    _hdrIdList.Add(key);
//                }
//                EnqueueAppend(aHdrKeyList);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        /// <summary>
//        /// This method attempts to enqueue the headers.  If no existing enqueue exists for any header,
//        /// the headers are enqueued.  If there is an existing enqueue,
//        /// the HeaderConflictList is updated and a HeaderConflictExcection is thrown.
//        /// </summary>

//        public void EnqueueHeaderRID()
//        {						
//            try
//            {
//                //_hdrIdList = new int[1];
//                //_hdrIdList[0] = _headerRID;
//                _hdrIdList = new ArrayList();
//                _hdrIdList.Add(_headerRID);
//                Enqueue();
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        /// <summary>
//        /// This method attempts to enqueue the headers.  If no existing enqueue exists for any header,
//        /// the headers are enqueued.  If there is an existing enqueue,
//        /// the HeaderConflictList is updated and a HeaderConflictExcection is thrown.
//        /// </summary>

//        private void Enqueue()
//        {
//            DataTable lockTable;
//            try
//            {
//                _conflictList.Clear();
//                lockTable = HeaderEnqueue_Read();
				
//                foreach (System.Data.DataRow dataRow in lockTable.Rows)
//                {
//                    _conflictList.Add(
//                        new HeaderConflict((System.Convert.ToInt32(dataRow["RID"], CultureInfo.CurrentUICulture)),
//                        System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture)));
//                }

//                if (_conflictList.Count > 0)
//                {
//                    //_hdrIdList = new int[0];            // RonM-Assortment - get rid of list
//                    _hdrIdList.Clear();
//                    throw new HeaderConflictException();
//                }
//                else
//                {
//                    _midNQ.OpenUpdateConnection();
//                    HeaderEnqueue_Insert();
//                    _midNQ.CommitData();
//                    _midNQ.CloseUpdateConnection();
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
        
//        private DataTable HeaderEnqueue_Read()
//        {
//            string hdrIds = string.Empty, keyString;
		
//            for (int i = 0; i < _hdrIdList.Count; i++)
//            {
//                keyString = Convert.ToString(_hdrIdList[i], CultureInfo.CurrentUICulture);
//                if (i == 0)
//                    hdrIds = " AND RID = " + keyString; 
//                else
//                    hdrIds = hdrIds + " OR RID = " + keyString;
//            }
//            return _midNQ.Enqueue_Read(eLockType.Header, hdrIds);
//        }

        
//        private void HeaderEnqueue_Insert()
//        {
//            int hdrRID ;
         
//            for (int i = 0; i < _hdrIdList.Count; i++)
//            {
//                hdrRID = (int)_hdrIdList[i];
//                _midNQ.Enqueue_Insert(eLockType.Header, hdrRID, _thisUserRID, _clientThreadID);
//            }	
//        }

//        private void EnqueueAppend(ArrayList aHdrKeyList)
//        {
//            DataTable lockTable;
//            try
//            {
//                _conflictList.Clear();
//                lockTable = HeaderEnqueueAppend_Read(aHdrKeyList);

//                foreach (System.Data.DataRow dataRow in lockTable.Rows)
//                {
//                    _conflictList.Add(
//                        new HeaderConflict((System.Convert.ToInt32(dataRow["RID"], CultureInfo.CurrentUICulture)),
//                        System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture)));
//                }

//                if (_conflictList.Count > 0)
//                {
//                    throw new HeaderConflictException();
//                }
//                else
//                {
//                    _midNQ.OpenUpdateConnection();
//                    HeaderEnqueueAppend_Insert(aHdrKeyList);
//                    _midNQ.CommitData();
//                    _midNQ.CloseUpdateConnection();
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private DataTable HeaderEnqueueAppend_Read(ArrayList aHdrKeyList)
//        {
//            string hdrIds = string.Empty, keyString;

//            for (int i = 0; i < aHdrKeyList.Count; i++)
//            {
//                keyString = Convert.ToString(aHdrKeyList[i], CultureInfo.CurrentUICulture);
//                if (i == 0)
//                {
//                    hdrIds = " AND RID = " + keyString;
//                }
//                else
//                {
//                    hdrIds = hdrIds + " OR RID = " + keyString;
//                }
//            }
//            return _midNQ.Enqueue_Read(eLockType.Header, hdrIds);
//        }


//        private void HeaderEnqueueAppend_Insert(ArrayList aHdrKeyList)
//        {
//            int hdrRID;
      
//            for (int i = 0; i < aHdrKeyList.Count; i++)
//            {
//                hdrRID = (int)aHdrKeyList[i];
//                _midNQ.Enqueue_Insert(eLockType.Header, hdrRID, _thisUserRID, _clientThreadID);
//            }
//        }

//        /// <summary>
//        /// This method dequeues an Allocation header.
//        /// </summary>

//        public void DequeueHeaders()
//        {
//            try
//            {
//                _midNQ.OpenUpdateConnection();
//                HeaderEnqueue_Delete();
//                _midNQ.CommitData();
//            }
//            catch (Exception exc)
//            {
//                _midNQ.Rollback();
//                string message = exc.ToString();
//                throw;
//            }
//            finally
//            {
//                _midNQ.CloseUpdateConnection();
//            }
//        }
		
//        private void HeaderEnqueue_Delete()
//        {	
//            int hdrRID ;
//            for (int i = 0; i < _hdrIdList.Count; i++)
//            {
//                hdrRID = (int)_hdrIdList[i];
//                _midNQ.Enqueue_Delete(eLockType.Header, hdrRID, _thisUserRID);
//            }	
//        }
//    }
//}
// end TT#1185 - Verify ENQ before Update (re-worked code) 
