using System;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the store grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SellThruPctProfile  : Profile
	{
		// Fields

		private eChangeType			_SellThruPctChangeType;
		private int					_sellThruPct;
		private bool				_sellThruPctIsInherited;
		private int					_sellThruPctInheritedFromNodeRID;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SellThruPctProfile(int aKey)
			: base(aKey)
		{
			_SellThruPctChangeType				= eChangeType.none;
			_sellThruPctIsInherited				= false;
			_sellThruPctInheritedFromNodeRID	= Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SellThruPct;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a store grade.
		/// </summary>
		public eChangeType SellThruPctChangeType 
		{
			get { return _SellThruPctChangeType ; }
			set { _SellThruPctChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the sell thru %.
		/// </summary>
		public int SellThruPct 
		{
			get { return _sellThruPct ; }
			set { _sellThruPct = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if sell thru percent information is inherited.
		/// </summary>
		public bool SellThruPctIsInherited 
		{
			get { return _sellThruPctIsInherited ; }
			set { _sellThruPctIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the sell thru percent information is inherited from.
		/// </summary>
		public int SellThruPctInheritedFromNodeRID 
		{
			get { return _sellThruPctInheritedFromNodeRID ; }
			set { _sellThruPctInheritedFromNodeRID = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of store grades
	/// </summary>
	[Serializable()]
	public class SellThruPctList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SellThruPctList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
