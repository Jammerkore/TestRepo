using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The CubeGroup class contains a collection of Cubes.
	/// </summary>
	/// <remarks>
	/// Groups of Cubes can be related through the used of a CubeGroup class.  Group level functions can be added to inherited classes from this abstract
	/// base class.
	/// </remarks>

	abstract public class WorkArea : MIDMarshalByRefObject
	{
		//=======
		// FIELDS
		//=======

		protected SessionAddressBlock _SAB;
		protected ApplicationSessionTransaction _transaction;
		protected System.Collections.Hashtable _profileHash;
		protected System.Collections.Hashtable _profileListGroupHash;
		protected System.Collections.Hashtable _profileXRefHash;
        //private ProfileList _profileHashLastProfileList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeGroup using the given SessionAddressBlock and Transaction.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to the SessionAddressBlock for this user session.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to the Transaction for this functional transaction.
		/// </param>

        public WorkArea(SessionAddressBlock aSAB, Transaction aTransaction)
			: base(aSAB.Sponsor)
		{
			try
			{
				_SAB = aSAB;
				_transaction = (ApplicationSessionTransaction)aTransaction;
				_profileHash = new Hashtable();
				_profileListGroupHash = new Hashtable();
				_profileXRefHash = new Hashtable();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the SessionAddressBlock.
		/// </summary>

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		/// <summary>
		/// Gets the Transaction.
		/// </summary>

		public ApplicationSessionTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		/// <summary>
		/// Gets or sets the StoreGroupProfile that describes the current Store Group.
		/// </summary>

		public StoreGroupProfile CurrentStoreGroupProfile
		{
			get
			{
				try
				{
					return _transaction.CurrentStoreGroupProfile;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					_profileListGroupHash.Remove(eProfileType.StoreGroupLevel);
					_profileXRefHash.Remove(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
					_transaction.CurrentStoreGroupProfile = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		//========================
		// ProfileList functions
		//========================

		virtual public ProfileList GetProfileList(eProfileType aProfileType)
		{
			try
			{
				return null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ProfileList as requested by the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that identifies which ProfileList to retrieve.
		/// </param>
		/// <returns>
		/// The ProfileList requested.
		/// </returns>

		private ProfileListGroup GetProfileListGroup(eProfileType aProfileType)
		{
			ProfileListGroup profileListGroup;
			ProfileList profileList;

			try
			{
				profileListGroup = (ProfileListGroup)_profileListGroupHash[aProfileType];

				if (profileListGroup == null)
				{
					profileListGroup = _transaction.GetProfileListGroup(aProfileType).Copy();
					profileList = GetProfileList(aProfileType);

					if (profileListGroup == null)
					{
						if (profileList == null)
						{
							profileList = Transaction.GetProfileList(aProfileType);
						}

						profileListGroup = new ProfileListGroup();
						profileListGroup.MasterProfileList = profileList;
						_profileListGroupHash[aProfileType] = profileListGroup;
					}
					else
					{
						if (profileList != null)
						{
							profileListGroup = profileListGroup.Copy();
							profileListGroup.MasterProfileList = profileList;
						}
					}

					_profileListGroupHash[aProfileType] = profileListGroup;
				}

				return profileListGroup;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Applies the given filter to a ProfileList.
		/// </summary>
		/// <param name="aFilter">
		/// The Filter to apply.
		/// </param>

		protected void ApplyFilter(Filter aFilter, eFilterType aFilterType)
		{
			try
			{
				GetProfileListGroup(aFilter.ProfileType).ApplyFilter(aFilter, aFilterType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Resets the filters on a ProfileList.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileList of the list to clear.
		/// </param>

		protected void ResetFilteredList(eProfileType aProfileType)
		{
			try
			{
				GetProfileListGroup(aProfileType).ResetFilteredList();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


		/// <summary>
		/// Returns the master (unfiltered) ProfileList of the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList requested.
		/// </param>
		/// <returns>
		/// The ProfileList requested.
		/// </returns>

		public ProfileList GetMasterProfileList(eProfileType aProfileType)
		{
			try
			{
				return GetProfileListGroup(aProfileType).MasterProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the filtered ProfileList of the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList requested.
		/// </param>
		/// <returns>
		/// The ProfileList requested.
		/// </returns>

		public ProfileList GetFilteredProfileList(eProfileType aProfileType)
		{
			try
			{
				return GetProfileListGroup(aProfileType).FilteredProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the master (unfiltered) ProfileList to the given ProfileList.
		/// </summary>
		/// <param name="aProfileList">
		/// The ProfileList to assign.
		/// </param>

		public void SetMasterProfileList(ProfileList aProfileList)
		{
			try
			{
				GetProfileListGroup(aProfileList.ProfileType).MasterProfileList = aProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the filtered ProfileList to the given ProfileList.
		/// </summary>
		/// <param name="aProfileList">
		/// The ProfileList to assign.
		/// </param>

		public void SetFilteredProfileList(ProfileList aProfileList)
		{
			try
			{
				GetProfileListGroup(aProfileList.ProfileType).FilteredProfileList = aProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//========================
		// ProfileXRef functions
		//========================

		/// <summary>
		/// Retrieves the ProfileXRef as requested by the given total and detail eProfileTypes.
		/// </summary>
        /// <param name="aProfXRef">
        /// The profile xref.
		/// </param>
		/// <returns>
		/// The requested ProfileXRef.
		/// </returns>

		public BaseProfileXRef GetProfileXRef(BaseProfileXRef aProfXRef)
		{
			BaseProfileXRef profileXRef;

			try
			{
				profileXRef = (BaseProfileXRef)_profileXRefHash[aProfXRef];

				if (profileXRef == null)
				{
					profileXRef = _transaction.GetProfileXRef(aProfXRef);

					if (profileXRef != null)
					{
						_profileXRefHash.Add(profileXRef, profileXRef);
					}
				}

				return profileXRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method adds the given ProfileXRefIdList to the ProfileXRef identified by the given total and detail eProfileTypes.  If the ProfileXRef
		/// does not exist, it is created.
		/// </summary>
		/// <param name="aProfileXRef">
		/// The ProfileXRef to add.
		/// </param>

		public void SetProfileXRef(BaseProfileXRef aProfileXRef)
		{
			try
			{
				_profileXRefHash[aProfileXRef] = aProfileXRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}