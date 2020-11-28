using System;
using System.Collections;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the store capacities for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SimilarStoreProfile  : Profile
	{
		// Fields

		private ArrayList			_simStores;
		private eSimilarStoreType	_simStoreType;
		private double				_simStoreRatio;
		private int					_simStoreUntilDateRangeRID;
		private ProfileList			_simStoreWeekList;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SimilarStoreProfile(int aKey)
			: base(aKey)
		{
			_simStores					= new ArrayList();
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SimilarStore;
			}
		}

		/// <summary>
		/// Gets or sets the type of simlar store identified for the store.
		/// </summary>
		public eSimilarStoreType SimStoreType 
		{
			get { return _simStoreType ; }
			set { _simStoreType = value; }
		}
		/// <summary>
		/// Gets or sets the list of simlar stores identified for the store.
		/// </summary>
		public ArrayList SimStores 
		{
			get { return _simStores ; }
			set { _simStores = value; }
		}
		/// <summary>
		/// Gets or sets the ratio of simlar store for the store.
		/// </summary>
		public double SimStoreRatio 
		{
			get { return _simStoreRatio ; }
			set { _simStoreRatio = value; }
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
		/// Gets or sets the week list of the date range to use for the similar store.
		/// </summary>
		public ProfileList SimStoreWeekList 
		{
			get { return _simStoreWeekList ; }
			set { _simStoreWeekList = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of store similar stores
	/// </summary>
	[Serializable()]
	public class SimilarStoreList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SimilarStoreList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
