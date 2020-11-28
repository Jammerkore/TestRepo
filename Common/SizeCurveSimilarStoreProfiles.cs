using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the size curve similar store for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeCurveSimilarStoreProfile : Profile
	{
		// Fields

		private eChangeType			_similarStoreChangeType;
		private bool				_recordExists;

		private bool				_simStoreIsInherited;
		private int					_simStoreInheritedFromNodeRID;
		private int					_simStoreRID;
		private int					_simStoreUntilDateRangeRID;
		private string				_simStoreDisplayDate;
		private ProfileList			_simStoreWeekList;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveSimilarStoreProfile(int aKey)
			: base(aKey)
		{
			_similarStoreChangeType		= eChangeType.none;
			_recordExists				= false;

			_simStoreIsInherited			= false;
			_simStoreInheritedFromNodeRID	= Include.NoRID;
			_simStoreRID				= Include.NoRID;
			_simStoreUntilDateRangeRID	= Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCurveSimilarStore;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a store's eligibility information.
		/// </summary>
		public eChangeType SimilarStoreChangeType 
		{
			get { return _similarStoreChangeType; }
			set { _similarStoreChangeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a store's eligibility information is inherited.
		/// </summary>
		public bool RecordExists 
		{
			get { return _recordExists ; }
			set { _recordExists = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a store's similar store information is inherited.
		/// </summary>
		public bool SimStoreIsInherited 
		{
			get { return _simStoreIsInherited ; }
			set { _simStoreIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the store's similar store information is inherited from.
		/// </summary>
		public int SimStoreInheritedFromNodeRID 
		{
			get { return _simStoreInheritedFromNodeRID ; }
			set { _simStoreInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the list of simlar stores identified for the store.
		/// </summary>
		public int SimStoreRID 
		{
			get { return _simStoreRID ; }
			set { _simStoreRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the date range to use for the similar store.
		/// </summary>
		public int SimStoreUntilDateRangeRID 
		{
			get { return _simStoreUntilDateRangeRID ; }
			set { _simStoreUntilDateRangeRID = value; }
		}
		/// <summary>
		/// Gets or sets the display date to use for the similar store.
		/// </summary>
		public string SimStoreDisplayDate 
		{
			get { return _simStoreDisplayDate ; }
			set { _simStoreDisplayDate = value; }
		}
		/// <summary>
		/// Gets or sets the week list of the date range to use for the similar store.
		/// </summary>
		public ProfileList SimStoreWeekList 
		{
			get { return _simStoreWeekList ; }
			set { _simStoreWeekList = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of eligibility information
	/// </summary>
	[Serializable()]
	public class SizeCurveSimilarStoreList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveSimilarStoreList()
			: base(eProfileType.SizeCurveSimilarStore)
		{
		}
	}
}
