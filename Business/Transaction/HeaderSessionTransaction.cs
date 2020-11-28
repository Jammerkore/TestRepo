using System;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Collections;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Transaction class defines the "local" workspace for a series of functions in the client application.
	/// </summary>
	/// <remarks>
	/// This class gives the user the ability to store information that is "local", or unique, to a series of screens or functions.  This allows a Client
	/// application to open multiple functions of the same type, yet each has its own copy of information contained in this class.
	/// </remarks>

	public class HeaderSessionTransaction : Transaction
	{
		//=======
		// FIELDS
		//=======
		private System.Collections.Hashtable _profileHash;
		private int _maxStoreRID = 0;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public HeaderSessionTransaction(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			_profileHash = new System.Collections.Hashtable();
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

        //Begin TT#1517-MD -jsobek -Store Service Optimization -unused function
		/// <summary>
		/// This method will retrieve the current ProfileList stored in this transaction.  If the ProfileList has not yet been created, the
		/// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
		/// be thrown.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList to retieve.
		/// </param>
		/// <returns>
		/// The ProfileList object for the given eProfileType.
		/// </returns>
        //public ProfileList GetProfileList(eProfileType aProfileType)
        //{
        //    ProfileList profileList;

        //    profileList = (ProfileList)_profileHash[aProfileType];

        //    if (profileList == null)
        //    {
        //        switch (aProfileType)
        //        {
        //            case eProfileType.Store:

        //                profileList = SAB.HeaderServerSession.GetProfileList(aProfileType);
        //                _profileHash.Add(profileList.ProfileType, profileList);
        //                _maxStoreRID = profileList.MaxValue;

        //                break;

        //            default:

        //                profileList = SAB.HeaderServerSession.GetProfileList(aProfileType);
        //                _profileHash.Add(profileList.ProfileType, profileList);

        //                break;
        //        }
        //    }

        //    return profileList;
        //}
        //End TT#1517-MD -jsobek -Store Service Optimization
	}

}
