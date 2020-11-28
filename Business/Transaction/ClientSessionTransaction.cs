using System;

using MIDRetail.Common;
using MIDRetail.DataCommon;
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

	public class ClientSessionTransaction : Transaction
	{
		//=======
		// FIELDS
		//=======

		private System.Collections.Hashtable _profileListGroupHash;
		private System.Collections.Hashtable _profileXRefHash;
//		private System.Collections.Hashtable _subtotalKeyHash;
//		private System.Collections.Hashtable _subtotalPacks;
//		private System.Collections.Hashtable _storeRID_IndexXref;
//		private System.Collections.Hashtable _inStoreReceiptDays;
//		private StoreGroupProfile _currStoreGroupProfile;
//		private CubeGroup _allocationCubeGroup;
//		private string _grandTotal;
//		private Index_RID _reserveStoreIndexRID;
//		private IntransitReader _intransitRdr;
//		private OnHandReader _onHandRdr;


		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public ClientSessionTransaction(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			_profileListGroupHash = new System.Collections.Hashtable();
			_profileXRefHash = new System.Collections.Hashtable();
			
//			_subtotalKeyHash = new System.Collections.Hashtable();
//			_subtotalPacks = null;
//			_storeRID_IndexXref = null;
//			_reserveStoreIndexRID = new Index_RID(0, Include.UndefinedStoreRID);
//			_grandTotal = null;
//			_inStoreReceiptDays = null;
//			_intransitRdr = null;
//			_onHandRdr = null;
//			_allocationCubeGroup = null;
		}

		override protected void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					foreach (ProfileListGroup plg in _profileListGroupHash.Values)
					{
						plg.Dispose();
					}
				}

				base.Dispose(disposing);
			}
			catch (Exception)
			{
			}
		}

		//=============
		// METHODS
		//=============

		//========================
		// ProfileList functions
		//========================

// NOTE: The following was removed because of the reference to the ApplicationServerSession.  The ClientTransaction should never assume that
// the ApplicationServerSession is the place to retrieve unknown lists, since some lists may be built or populated by the ApplicationTransaction
// object.  All ProfileLists needed by the Client must be built or retrieved from the correct resource by the Client, who can then store it here
// for future use with the SetMasterProfileList and SetFilteredProfileList functions.
//
//		/// <summary>
//		/// Retrieves the master ProfileList as requested by the given eProfileType.
//		/// </summary>
//		/// <param name="aProfileType">
//		/// The eProfileType that identifies which ProfileList to retrieve.
//		/// </param>
//		/// <returns>
//		/// The ProfileList requested.
//		/// </returns>
//
//		public ProfileListGroup GetProfileListGroup(eProfileType aProfileType)
//		{
//			ProfileList profileList;
//			ProfileListGroup profileListGroup;
//
//			profileListGroup = (ProfileListGroup)_profileListGroupHash[aProfileType];
//
//			if (profileListGroup == null)
//			{
//				profileList = SAB.ApplicationServerSession.GetProfileList(aProfileType);
//
//				profileListGroup = new ProfileListGroup();
//				profileListGroup.MasterProfileList = profileList;
//				_profileListGroupHash[aProfileType] = profileListGroup;
//			}
//
//			return profileListGroup;
//		}

		public void ApplyFilter(Filter aFilter, eFilterType aFilterType)
		{
//			GetProfileListGroup(aFilter.ProfileType).ApplyFilter(aFilter, aFilterType);
			ProfileListGroup profileListGroup;

			profileListGroup = (ProfileListGroup)_profileListGroupHash[aFilter.ProfileType];
			if (profileListGroup != null)
			{
				profileListGroup.ApplyFilter(aFilter, aFilterType);
			}
		}

		public ProfileList GetMasterProfileList(eProfileType aProfileType)
		{
//			return GetProfileListGroup(aProfileType).MasterProfileList;
			return ((ProfileListGroup)_profileListGroupHash[aProfileType]).MasterProfileList;
		}

		public void SetMasterProfileList(ProfileList aProfileList)
		{
//			GetProfileListGroup(aProfileList.ProfileType).MasterProfileList = aProfileList;
			((ProfileListGroup)_profileListGroupHash[aProfileList.ProfileType]).MasterProfileList = aProfileList;
		}

//        public ProfileList GetFilteredProfileList(eProfileType aProfileType)
//        {
////			return GetProfileListGroup(aProfileType).FilteredProfileList;
//            return ((ProfileListGroup)_profileListGroupHash[aProfileType]).FilteredProfileList;
//        }

//        public void SetFilteredProfileList(ProfileList aProfileList)
//        {
////			GetProfileListGroup(aProfileList.ProfileType).FilteredProfileList = aProfileList;
//            ((ProfileListGroup)_profileListGroupHash[aProfileList.ProfileType]).FilteredProfileList = aProfileList;
//        }

		/// <summary>
		/// This method removes the ProfileList identified by the given eProfileType from the Transaction's stored list.  This will cause the 
		/// list to be retrieved from the SAB at the next reference.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that identifies the ProfileList to refresh.
		/// </param>

		public void RefreshProfileLists(eProfileType aProfileType)
		{
			_profileListGroupHash.Remove(aProfileType);
		}

		//========================
		// ProfileXRef functions
		//========================

		/// <summary>
		/// Retrieves the ProfileXRef as requested by the given total and detail eProfileTypes.
		/// </summary>
		/// <param name="aTotalType">
		/// The eProfileType of the total profile.
		/// </param>
		/// <param name="aDetailType">
		/// The eProfileType of the detail profile.
		/// </param>
		/// <returns>
		/// The requested ProfileXRef.
		/// </returns>

		public ProfileXRef GetProfileXRef(eProfileType aTotalType, eProfileType aDetailType)
		{
			ProfileXRef profileXRef;

			profileXRef = (ProfileXRef)_profileXRefHash[new ProfileXRef(aTotalType, aDetailType)];

			if (profileXRef == null)
			{
				profileXRef = (ProfileXRef)SAB.ApplicationServerSession.GetProfileXRef(new ProfileXRef(aTotalType, aDetailType));

				_profileXRefHash.Add(profileXRef, profileXRef);
			}

			return profileXRef;
		}

		/// <summary>
		/// This method adds the given ProfileXRefIdList to the ProfileXRef identified by the given total and detail eProfileTypes.  If the ProfileXRef
		/// does not exist, it is created.
		/// </summary>
		/// <param name="aProfileXRef">
		/// The ProfileXRef to add.
		/// </param>

		public void SetProfileXRef(ProfileXRef aProfileXRef)
		{
			_profileXRefHash[aProfileXRef] = aProfileXRef;
		}

		//======================
		// Allocation functions
		//======================
		#region DoAllocationAction
		public bool DoAllocationAction(AllocationWorkFlowStep aAllocationWorkFlowStep)
		{
			return false;
		}
		#endregion DoAllocationAction

		#region NewAllocationMasterProfileList

		/// <summary>
		/// Create a new allocation profile list on the application server
		/// and set it to the allocation master profile list
		/// </summary>
		public void NewAllocationMasterProfileList()
		{
			AllocationProfileList allocationList = new AllocationProfileList(eProfileType.Allocation);
			SetMasterProfileList(allocationList);
		}
		#endregion

		#region GetAllocationProfile
		public AllocationProfile GetAllocationProfile(int aHeaderRID)
		{
			AllocationProfileList apl = (AllocationProfileList) this.GetMasterProfileList(eProfileType.Allocation);
			return (AllocationProfile)apl.FindKey(aHeaderRID);
		}
		public int[] GetAllocationProfileKeys()
		{
			AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
			if (apl == null)
			{
				return null;
			}
			int [] key = new int[apl.Count];
			for (int i=0; i<apl.Count; i++)
			{
				key [i] = ((AllocationProfile)apl[i]).Key;
			}
			return key;
		}
		#endregion GetAllocationProfile

		#region AddAllocationProfiles
		/// <summary>
		/// Adds an allocation profile to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to add</param>
		public void AddAllocationProfile(AllocationProfile aAllocationProfile)
		{
			AllocationProfileList allocationList 
				= (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
			if (allocationList == null)
			{
				allocationList = new AllocationProfileList(eProfileType.Allocation);
				SetMasterProfileList(allocationList);
			}
			allocationList.Add(aAllocationProfile);
		}

		/// <summary>
		/// Adds Allocation Profiles to the transcation tracked allocations
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to add.</param>
		public void AddAllocationProfile(AllocationProfileList aAllocationList)
		{
			AddAllocationProfile(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Adds Allocation Profiles to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationList">ICollection of Allocation Profiles to add</param>
		public void AddAllocationProfile(System.Collections.ICollection aAllocationList)
		{
			foreach (AllocationProfile ap in aAllocationList)
			{
				AddAllocationProfile(ap);
			}
		}
		#endregion AddAllocationProfiles
		
		#region RemoveAllocationProfiles
		/// <summary>
		/// Removes an allocation profile from the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to Remove</param>
		public void RemoveAllocationProfile(AllocationProfile aAllocationProfile)
		{
			AllocationProfileList allocationList 
				= (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
			if (allocationList != null)
			{
				allocationList.Remove(aAllocationProfile);
			}
		}

		/// <summary>
		/// Removes Allocation Profiles from the transcation tracked allocations
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to Remove.</param>
		public void RemoveAllocationProfile(AllocationProfileList aAllocationList)
		{
			RemoveAllocationProfile(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Removes Allocation Profiles from the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationList">ICollection of Allocation Profiles to Remove</param>
		public void RemoveAllocationProfile(System.Collections.ICollection aAllocationList)
		{
			foreach (AllocationProfile ap in aAllocationList)
			{
				RemoveAllocationProfile(ap);
			}
		}
		#endregion RemoveAllocationProfiles

	}
}
