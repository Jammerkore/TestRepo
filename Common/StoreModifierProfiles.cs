using System;
using System.Collections;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the modifier for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreWeekModifierProfile : Profile
	{
		// Fields

		private int					_yearWeek;
		private double				_storeModifier;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreWeekModifierProfile(int aKey)
			: base(aKey)
		{
			_yearWeek = -1;
			_storeModifier = 1;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreModifier;
			}
		}

		/// <summary>
		/// Gets or sets the year/week for which eligibility is to be determined.
		/// </summary>
		public int YearWeek 
		{
			get { return _yearWeek ; }
			set { _yearWeek = value; }
		}
		/// <summary>
		/// Gets or sets the value of the modifier for the.
		/// </summary>
		public double StoreModifier 
		{
			get { return _storeModifier ; }
			set { _storeModifier = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of modifier information for the store/week combinations
	/// </summary>
	[Serializable()]
	public class StoreWeekModifierList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreWeekModifierList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	// BEGIN MID Track #4827 - John Smith - Presentation plus sales
	/// <summary>
	/// Contains the information about the presentation minimum plus sales setting for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StorePMPlusSalesProfile : Profile
	{
		// Fields

		private bool				_applyPMPlusSales;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StorePMPlusSalesProfile(int aKey)
			: base(aKey)
		{
			_applyPMPlusSales = false;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.PMPlusSales;
			}
		}

		/// <summary>
		/// Gets or sets the flag identifing if the presentation minimum plus sales option 
		/// is to be used for the store.
		/// </summary>
		public bool ApplyPMPlusSales 
		{
			get { return _applyPMPlusSales ; }
			set { _applyPMPlusSales = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of modifier information for the store/week combinations
	/// </summary>
	[Serializable()]
	public class StorePMPlusSalesList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StorePMPlusSalesList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}
	// END MID Track #4827
}
