using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
     //====================================================================
	// ISSUE 4827 - this entire class was created because of this issue
	//====================================================================
	/// <summary>
	/// Summary description for CustomBusinessRoutines.
	/// Includes custom business code for various clients.
	/// </summary>
	public class CustomBusinessRoutines : BaseBusinessRoutines
	{
        // Begin TT#1705 - JSmith - Reset Header with Piggybacking
        private AllocationProfile _processsingAllocationProfile = null;
        // End TT#1705
        
        public CustomBusinessRoutines(SessionAddressBlock sab, 
			ApplicationSessionTransaction transaction, ProfileList storeList, int nodeRid)
			: base(sab, transaction)
		{

		}

		// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
		public CustomBusinessRoutines(SessionAddressBlock sab, ApplicationSessionTransaction transaction)
			: base(sab, transaction)
		{

		}
		// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

        // Begin TT#1705 - JSmith - Reset Header with Piggybacking
        public AllocationProfile ProcesssingAllocationProfile
        {
            get { return _processsingAllocationProfile; }
            set { _processsingAllocationProfile = value; }
        }
        // End TT#1705

        // Begin TT#1705 - JSmith - Reset Header with Piggybacking
        public bool CustomHeaderResetAction(MIDRetail.Business.Allocation.AllocationProfile aAllocationProfile, Session aSession)
        {
            return true;
		}

        public bool AllowElibigilityPMPlusSales()
        {
            return false;
        }

        // BEGIN MID Track #6394 - update custom charactersitic
        public void CustomCancelAllocationAction(Header aHeader, AllocationProfile ap, GeneralComponent aComponent, eHeaderAllocationStatus aHeaderAllocationStatus)
        {

        }
        // END MID Track #6394 - update custom charactersitic

	}
}
