using System;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// The ProfileListGroup class stores a master and filtered ProfileList object.
	/// </summary>
	/// <remarks>
	/// This class is used to store the ProfileList objects that are associated with a eProfileType.  Both a master and a filtered list is stored.  If a filter
	/// has not been applied, reference to the filtered list returns the master list.
	/// </remarks>

	[Serializable]
	public class ProfileListGroup : MIDObject
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _masterProfileList;
		private ProfileList _filteredProfileList;
		private ArrayList _appliedPermanentFilters;
		private ArrayList _appliedTemporaryFilters;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileListGroup.
		/// </summary>

		public ProfileListGroup()
		{
			_appliedPermanentFilters = new ArrayList();
			_appliedTemporaryFilters = new ArrayList();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (Filter filter in _appliedPermanentFilters)
				{
					filter.Dispose();
				}
				foreach (Filter filter in _appliedTemporaryFilters)
				{
					filter.Dispose();
				}
			}
			base.Dispose (disposing);
		}


		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the master ProfileList.
		/// </summary>

		public ProfileList MasterProfileList
		{
			get
			{
				return _masterProfileList;
			}
			set
			{
				_masterProfileList = value;
				_filteredProfileList = null;
			}
		}

		/// <summary>
		/// Gets or sets the filtered ProfileList.
		/// </summary>

		public ProfileList FilteredProfileList
		{
			get
			{
				if (_filteredProfileList != null)
				{
					return _filteredProfileList;
				}
				else
				{
					_filteredProfileList = _masterProfileList;

					foreach (Filter filter in _appliedPermanentFilters)
					{
						_filteredProfileList = filter.ApplyFilter(_filteredProfileList);
					}

					foreach (Filter filter in _appliedTemporaryFilters)
					{
						_filteredProfileList = filter.ApplyFilter(_filteredProfileList);
					}

					return _filteredProfileList;
				}
			}
			set
			{
				_filteredProfileList = value;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// This method applies the given Filter to the master ProfileList, producing the filtered ProfileList.
		/// </summary>

		public void ApplyFilter(Filter aFilter, eFilterType aFilterType)
		{
			_filteredProfileList = aFilter.ApplyFilter(FilteredProfileList);

			if (aFilterType == eFilterType.Permanent)
			{
				_appliedPermanentFilters.Add(aFilter);
			}
			else
			{
				_appliedTemporaryFilters.Add(aFilter);
			}
		}

		/// <summary>
		/// This method clears the filtered list.
		/// </summary>

		public void ResetFilteredList()
		{
			_appliedTemporaryFilters.Clear();
			_filteredProfileList = null;
		}

		/// <summary>
		/// Returns a copy of this ProfileListGroup.
		/// </summary>
		/// <returns>
		/// A copy of this ProfileListGroup.
		/// </returns>

		public ProfileListGroup Copy()
		{
			ProfileListGroup profListGrp;

			profListGrp = new ProfileListGroup();
			profListGrp._masterProfileList = _masterProfileList;
			profListGrp._filteredProfileList = _filteredProfileList;
			profListGrp._appliedPermanentFilters = (ArrayList)_appliedPermanentFilters.Clone();
			profListGrp._appliedTemporaryFilters = (ArrayList)_appliedTemporaryFilters.Clone();

			return profListGrp;
		}
	}
}
