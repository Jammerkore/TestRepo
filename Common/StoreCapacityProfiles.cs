using System;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the store capacities for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreCapacityProfile  : Profile
	{
		// Fields

		private bool				_newRecord;
		private eChangeType			_storeCapacityChangeType;
		private bool				_storeCapacityIsInherited;
		private int					_storeCapacityInheritedFromNodeRID;
		private int					_storeCapacity;
		private int					_nodeRID;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreCapacityProfile(int aKey)
			: base(aKey)
		{
			_storeCapacityChangeType		= eChangeType.none;
			_newRecord = true;
			_storeCapacityIsInherited			= false;
			_storeCapacityInheritedFromNodeRID	= Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreCapacity;
			}
		}

		/// <summary>
		/// Gets or sets the switch to identify if the store has a value saved to the database.
		/// </summary>
		public bool NewRecord 
		{
			get { return _newRecord ; }
			set { _newRecord = value; }
		}
		/// <summary>
		/// Gets or sets the change type of a store grade.
		/// </summary>
		public eChangeType StoreCapacityChangeType 
		{
			get { return _storeCapacityChangeType ; }
			set { _storeCapacityChangeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a store's capacity information is inherited.
		/// </summary>
		public bool StoreCapacityIsInherited 
		{
			get { return _storeCapacityIsInherited ; }
			set { _storeCapacityIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the store's capacity information is inherited from.
		/// </summary>
		public int StoreCapacityInheritedFromNodeRID 
		{
			get { return _storeCapacityInheritedFromNodeRID ; }
			set { _storeCapacityInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the code for the store capacity.
		/// </summary>
		public int StoreCapacity 
		{
			get { return _storeCapacity ; }
			set { _storeCapacity = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the store capacity was found.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of store capacities
	/// </summary>
	[Serializable()]
	public class StoreCapacityList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreCapacityList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
