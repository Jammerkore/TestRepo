using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Data;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Diagnostics;

namespace MIDRetail.Business.Allocation
{
     #region AllocationProfileList
    /// <summary>
    /// List of allocation profiles.
    /// </summary>
    public class AllocationProfileList : ProfileList
    {
        internal MIDRetail.Data.Header _headerDataRecord;	// used to read/write profile to the data layer
        private SessionAddressBlock _SAB;
        private Transaction _transaction;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public AllocationProfileList(eProfileType aProfileType)
            : base(aProfileType)
        {
            _headerDataRecord = null;
            _transaction = null;
            _SAB = null;

        }

        /// <summary>
        /// Creates Allocation Profiles and adds them to this Profile List.
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock associated with this profile list.</param>
        /// <param name="aAssortmentRIDList">List of Assortment RIDs excluding Header RIDs</param>
        /// <param name="aHeaderRIDList">List of Header RIDs excluding Assortment RIDs</param>
        /// <param name="aSession">The session where the headers are being loaded</param>
        /// <remarks>This version of the load does not allow store information to be loaded or referenced.</remarks>
        public void LoadHeaders(SessionAddressBlock aSAB, int[] aAssortmentRIDList, int[] aHeaderRIDList, Session aSession) // TT#488 - MD - Jellis - Group Allocation
        {
            bool duplicateIgnored = false;
            if (_SAB == null)
            {
                _SAB = aSAB;
            }

            // begin TT#488 - MD - Jellis - Group Allocation
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            if (aAssortmentRIDList != null)
            {
            // End TT#1966-MD - JSmith - DC Fulfillment
                AssortmentProfile asp;
                foreach (int assortmentRID in aAssortmentRIDList)
                {
                    if (this.Contains(assortmentRID))
                    {
                        duplicateIgnored = true;
                    }
                    else
                    {
                        asp = new AssortmentProfile(_SAB, null, assortmentRID, aSession);
                        this.Add(asp);
                    }
                }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            }
            // End TT#1966-MD - JSmith - DC Fulfillment
            // end TT#488 - MD - jellis - Group Allocation

            // Begin TT#1966-MD - JSmith - DC Fulfillment
            if (aHeaderRIDList != null)
            {
            // End TT#1966-MD - JSmith - DC Fulfillment
                AllocationProfile ap;
                foreach (int headerRID in aHeaderRIDList)
                {
                    if (this.Contains(headerRID))
                    {
                        duplicateIgnored = true;
                    }
                    else
                    {
                        ap = new AllocationProfile(_SAB, null, headerRID, aSession);
                        this.Add(ap);
                    }
                }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            }
            // End TT#1966-MD - JSmith - DC Fulfillment

            if (duplicateIgnored)
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_al_DuplicateHeaderIgnored),
                    aSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateHeaderIgnored));
            }
        }

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Creates Allocation Profiles and adds them to this Profile List.
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock associated with this profile list.</param>
        /// <param name="aHeaderRIDList">List of Header RIDs</param>
        /// <param name="aSession">The session where the headers are being loaded</param>
        /// <remarks>This version of the load does not allow store information to be loaded or referenced.</remarks>
        public void LoadHeaders(SessionAddressBlock aSAB, AllocationHeaderProfileList aAHPL, Session aSession)
        {
            bool duplicateIgnored = false;
            if (_SAB == null)
            {
                _SAB = aSAB;
            }

            AllocationProfile ap;
            AssortmentProfile asp;
            foreach (AllocationHeaderProfile headerProfile in aAHPL)
            {
                if (this.Contains(headerProfile.Key))
                {
                    duplicateIgnored = true;
                }
                else
                {
                    if (headerProfile.HeaderType == eHeaderType.Assortment)
                    {
                        asp = new AssortmentProfile(_SAB, headerProfile, aSession);
                        this.Add(asp);
                    }
                    else
                    {
                        ap = new AllocationProfile(_SAB, headerProfile, aSession);
                        this.Add(ap);
                    }
                }
            }
            if (duplicateIgnored)
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_al_DuplicateHeaderIgnored),
                    aSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateHeaderIgnored));
            }
        }

        // end TT#488 - MD - Jellis - Group Allocation

        /// <summary>
        /// Creates Allocation Profiles and adds them to this Profile List.
        /// </summary>
        /// <param name="aTransaction">Transaction associated with this profile list.</param>
        /// <param name="aAssortmentRIDList">List of AssortmentRIDs excluding Header RIDs</param>
        /// <param name="aHeaderRIDList">List of Header RIDs excluding Assortment RIDs</param>
        /// <param name="aSession">The session where the headers are being loaded</param>
        public void LoadHeaders(Transaction aTransaction, int[] aAssortmentRIDList, int[] aHeaderRIDList, Session aSession) // TT#488 - MD - Jellis - Group Allocation
        {
			// begin TT#1137 (MID Track 4351) Rebuild Intransit Utility
			LoadHeaders (aTransaction, aAssortmentRIDList, aHeaderRIDList, aSession, true); // TT#488 - MD - Jellis - Group Allocation
		}

        // Begin TT#2136-MD - JSmith - Assortment Selection Error
        //public void LoadHeaders(Transaction aTransaction, int[] aAssortmentRIDList, int[] aHeaderRIDList, Session aSession, bool aIncludeInSubtotals) // TT#488 - MD - Jellis - Group Allocation
        public void LoadHeaders(Transaction aTransaction, int[] aAssortmentRIDList, int[] aHeaderRIDList, Session aSession, bool aIncludeInSubtotals, AllocationProfileList previousList = null) // TT#488 - MD - Jellis - Group Allocation
        // End TT#2136-MD - JSmith - Assortment Selection Error
        {
            // end TT#1137 (MID Track 4351) Rebuild Intransit Utility
            bool duplicateIgnored = false;
            if (_transaction == null)
            {
                _transaction = aTransaction;
            }
            else
            {
                if (_transaction != aTransaction)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_al_CannotChgTransactionAftProfileCreated),
                        aSession.Audit.GetText(eMIDTextCode.msg_al_CannotChgTransactionAftProfileCreated));
                }
            }

            // Begin TT#2136-MD - JSmith - Assortment Selection Error
            //AllocationProfile ap;
            AllocationProfile ap = null;
            // End TT#2136-MD - JSmith - Assortment Selection Error
            foreach (int headerRID in aHeaderRIDList)
            {
                if (this.Contains(headerRID))
                {
                    duplicateIgnored = true;
                }
                else
                {
                    ap = null;  // TT#5811 - JSmith - Multiple Selected Headers Are Not Processed
                    // Begin TT#2136-MD - JSmith - Assortment Selection Error
                    //ap = new AllocationProfile(_transaction, null, headerRID, aSession, aIncludeInSubtotals); // TT#1137 (MID Track 4351) RebuildIntransit Utility
                    if (previousList != null)
                    {
                        ap = (AllocationProfile)previousList.FindKey(headerRID);
                    }
                    if (ap == null)
                    {
                        ap = new AllocationProfile(_transaction, null, headerRID, aSession, aIncludeInSubtotals); // TT#1137 (MID Track 4351) RebuildIntransit Utility
                    }
                    // End TT#2136-MD - JSmith - Assortment Selection Error
                    this.Add(ap);
                }
            }
            // begin TT#488 - MD - JEllis - Group Allocation
            // Begin TT#2136-MD - JSmith - Assortment Selection Error
            //AssortmentProfile asp;
            AssortmentProfile asp = null;
            // End TT#2136-MD - JSmith - Assortment Selection Error
            // Begin TT#973 - MD - stodd - null reference
            if (aAssortmentRIDList != null)
            {
                foreach (int assortmentRID in aAssortmentRIDList)
                {
                    if (this.Contains(assortmentRID))
                    {
                        duplicateIgnored = true;
                    }
                    else
                    {
                        // Begin TT#2136-MD - JSmith - Assortment Selection Error
                        //asp = new AssortmentProfile(_transaction, null, assortmentRID, aSession, aIncludeInSubtotals);
                        if (previousList != null)
                        {
                            asp = (AssortmentProfile)previousList.FindKey(assortmentRID);
                        }
                        if (asp == null)
                        {
                            asp = new AssortmentProfile(_transaction, null, assortmentRID, aSession, aIncludeInSubtotals);
                        }
                        // End TT#2136-MD - JSmith - Assortment Selection Error
                        this.Add(asp);
                    }
                }
            }
            // End TT#973 - MD - stodd - null reference
            // end TT#488 - MD - Jellis - Group Allocation

            if (duplicateIgnored)
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_al_DuplicateHeaderIgnored),
                    aSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateHeaderIgnored));
            }
        }
        // Begin Assortment Change (J.Ellis)
        public void LoadHeaders(Transaction aTransaction, SelectedHeaderList aHeaderList, Session aSession, bool throwErrorIfDuplicateFound = true) //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
        {
            bool duplicateIgnored = false;
            if (_transaction == null)
            {
                _transaction = aTransaction;
            }
            else
            {
                if (_transaction != aTransaction)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_al_CannotChgTransactionAftProfileCreated),
                        aSession.Audit.GetText(eMIDTextCode.msg_al_CannotChgTransactionAftProfileCreated));
                }
            }
            AllocationProfile ap;
            AssortmentProfile asp;
            foreach (SelectedHeaderProfile selectedHeader in aHeaderList)
            {
                if (this.Contains(selectedHeader.Key))
                {
                    duplicateIgnored = true;
                }
                else
                {
                    if (selectedHeader.HeaderType == eHeaderType.Assortment)
                    {
                        asp = new AssortmentProfile(_transaction, null, selectedHeader.Key, aSession);
                        this.Add(asp);
                    }
                    else
                    {
                        ap = new AllocationProfile(_transaction, null, selectedHeader.Key, aSession);
                        this.Add(ap);
                    }
                }
            }
            if (duplicateIgnored && throwErrorIfDuplicateFound) //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_al_DuplicateHeaderIgnored),
                    aSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateHeaderIgnored));
            }
        }
        // End Assortment Change (J.Ellis)
        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Creates Allocation Profiles and adds them to this Profile List.
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock associated with this profile list.</param>
        /// <param name="aHeaderRIDList">List of Header RIDs</param>
        /// <param name="aSession">The session where the headers are being loaded</param>
        /// <remarks>This version of the load does not allow store information to be loaded or referenced.</remarks>
        public void LoadHeaders(ApplicationSessionTransaction aTran, AllocationHeaderProfileList aAHPL, Session aSession)
        {
            bool duplicateIgnored = false;
            if (_SAB == null)
            {
                _SAB = aTran.SAB;
            }

            AllocationProfile ap;
            AssortmentProfile asp;
            foreach (AllocationHeaderProfile headerProfile in aAHPL)
            {
                if (this.Contains(headerProfile.Key))
                {
                    duplicateIgnored = true;
                }
                else
                {
                    if (headerProfile.HeaderType == eHeaderType.Assortment)
                    {
                        asp = new AssortmentProfile(aTran, headerProfile.HeaderID, headerProfile.Key, aSession);
                        this.Add(asp);
                    }
                    else
                    {
                        ap = new AllocationProfile(aTran, headerProfile.HeaderID, headerProfile.Key, aSession);
                        this.Add(ap);
                    }
                }
            }
            if (duplicateIgnored)
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_al_DuplicateHeaderIgnored),
                    aSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateHeaderIgnored));
            }
        }

        // end TT#488 - MD - Jellis - Group Allocation


        /// <summary>
        /// Creates Allocation Profiles and adds them to this Profile List.
        /// </summary>
        /// <param name="aTransaction">Transaction associated with this profile list.</param>
        /// <param name="aHeaderRIDList">List of Header RIDs</param>
        /// <param name="aSession">The session where the headers are being loaded</param>
        public void LoadLinkedHeaders(ApplicationSessionTransaction aTransaction, int[] aHeaderRIDList, Session aSession)
        {
            bool duplicateIgnored = false;
            if (_transaction == null)
            {
                _transaction = aTransaction;
            }
            else
            {
                if (_transaction != aTransaction)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_al_CannotChgTransactionAftProfileCreated),
                        aSession.Audit.GetText(eMIDTextCode.msg_al_CannotChgTransactionAftProfileCreated));
                }
            }
            AllocationProfileList apl = (AllocationProfileList)aTransaction.GetMasterProfileList(eProfileType.Allocation);
            AllocationProfile ap;
            foreach (int headerRID in aHeaderRIDList)
            {
                if (this.Contains(headerRID))
                {
                    duplicateIgnored = true;
                }
                else
                {
                    // only add if not selected
                    ap = (AllocationProfile)apl.FindKey(headerRID);
                    if (ap == null)
                    {
                        ap = new AllocationProfile(_transaction, null, headerRID, aSession);
                    }
                    this.Add(ap);
                    aTransaction.AddLinkedHeader(ap);
                }
            }
            if (duplicateIgnored)
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_al_DuplicateHeaderIgnored),
                    aSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateHeaderIgnored));
            }
        }

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

        ///// <summary>
        ///// Loads all headers
        ///// </summary>
        ///// <param name="aSAB">SessionAddressBlock assocated with the headers</param>
        ///// <param name="aGetInterfacedHeaders">Get headers loaded through the API</param>
        ///// <param name="aGetNonInterfacedHeaders">Get headers keyed online</param>
        ///// <remarks>This version of the load does not allow store information to be loaded or referenced.</remarks>
        ///// <param name="aSession">The session where the headers are being loaded</param>
        // BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
        //		public void LoadAll(SessionAddressBlock aSAB, Session aSession)
        //public void LoadAll(SessionAddressBlock aSAB, Session aSession, bool aGetInterfacedHeaders,
        //    bool aGetNonInterfacedHeaders)
        //// END MID Track #4357
        //{
        //    bool oldway = false;
        //    _SAB = aSAB;
        //    AllocationProfile ap;
        //    AssortmentProfile asp; // TT#488 - MD - Jellis _ Group Allocation
        //    AllocationTypeFlags atf = new AllocationTypeFlags(); // TT#488 - MD - Jellis - Group Allocation
        //    DateTime startTime = DateTime.Now;
        //    if (oldway)
        //    {
        //        if (_headerDataRecord == null) _headerDataRecord = new Header();


        //        System.Data.DataTable dt = _headerDataRecord.GetHeaders();
        //        foreach (System.Data.DataRow dr in dt.Rows)
        //        {
        //            //				ap = new AllocationProfile(_SAB, dr["HDR_ID"].ToString(),
        //            //					Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture), aSession);
        //            // begin TT#488 - MD - Jellis - Group Allocation
        //            atf.AllFlags = Convert.ToUInt32(dr["ALLOCATION_TYPE_FLAGS"], CultureInfo.CurrentUICulture);
        //            if (atf.Assortment)
        //            {
        //                asp = new AssortmentProfile(_SAB, dr, aSession);
        //                if (asp.IsInterfaced)
        //                {
        //                    if (!aGetInterfacedHeaders)
        //                    {
        //                        continue;
        //                    }
        //                }
        //                else if (!aGetNonInterfacedHeaders)
        //                {
        //                    continue;
        //                }
        //                this.Add(asp);
        //            }
        //            else
        //            {
        //                // end TT#488 - MD - Jellis - Group Allocation
        //                ap = new AllocationProfile(_SAB, dr, aSession);
        //                // BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
        //                if (ap.IsInterfaced)
        //                {
        //                    if (!aGetInterfacedHeaders)
        //                    {
        //                        continue;
        //                    }
        //                }
        //                else if (!aGetNonInterfacedHeaders)
        //                {
        //                    continue;
        //                }
        //                // END MID Track #4357
        //                this.Add(ap);
        //            }   // TT#488 - MD - Jellis - Group Allocation

        //        }
        //    }
        //    else
        //    {

        //        ArrayList headers = aSAB.HeaderServerSession.GetHeadersForUser(aSAB.ClientServerSession.UserRID, aGetInterfacedHeaders, aGetNonInterfacedHeaders);
        //        foreach (AllocationHeaderProfile ahp in headers)
        //        {
        //            // begin TT#488 - MD - Jellis - Group Allocation
        //            if (ahp.HeaderType == eHeaderType.Assortment)
        //            {
        //                asp = new AssortmentProfile(_SAB, ahp, aSession);
        //                Add(asp);
        //            }
        //            else
        //            {
        //                // end TT#488 - MD - Jellis - Group Allocation
        //                ap = new AllocationProfile(_SAB, ahp, aSession);
        //                this.Add(ap);
        //            }   // TT#488 - MD - Jellis - Group Allocation
        //        }
        //    }
        //    //			TimeSpan duration = DateTime.Now.Subtract(startTime);
        //    //			string strDuration = Convert.ToString(duration,CultureInfo.CurrentUICulture);
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

        /// <summary>
        /// Loads all headers
        /// </summary>
        /// <param name="aTransaction">Transaction assocated with the headers</param>
        /// <param name="aSession">The session where the headers are being loaded</param>
        public void LoadAll(Transaction aTransaction, Session aSession)
        {
            _transaction = aTransaction;
            AllocationProfile ap;
            if (_headerDataRecord == null) _headerDataRecord = new Header();

            System.Data.DataTable dt = _headerDataRecord.GetHeaders();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                ap = new AllocationProfile(_transaction, dr["HDR_ID"].ToString(),
                    Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture), aSession);
                this.Add(ap);
            }
        }

        /// <summary>
        /// Gets the grand total units to allocate across all headers in this profile list.
        /// </summary>
        public int GrandTotalToAllocate
        {
            get
            {
                int grandTotalToAllocate = 0;
                foreach (AllocationProfile ap in this)
                {
                    grandTotalToAllocate += ap.TotalUnitsToAllocate;
                }
                return grandTotalToAllocate;
            }
        }
    }
    #endregion AllocationProfileList
}