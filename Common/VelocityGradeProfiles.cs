using System;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the store grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class VelocityGradeProfile  : Profile
	{
		// Fields

		private eChangeType			_velocityGradeChangeType;
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		private bool				_velocityGradeFound;
		//End TT#505 - JScott - Velocity - Apply Min/Max
		private string				_velocityGrade;
		private bool				_velocityGradeIsInherited;
		private int					_velocityGradeInheritedFromNodeRID;
		private int					_boundary;

		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		private eChangeType			_velocityMinMaxChangeType;
		private bool				_velocityMinMaxFound;
		private bool				_velocityMinMaxesIsInherited;
		private int					_velocityMinMaxesInheritedFromNodeRID;
		private int					_velocityMinStock;
		private int					_velocityMaxStock;
		private int					_velocityMinAd;
        
		//End TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public VelocityGradeProfile(int aKey)
			: base(aKey)
		{
			_velocityGradeChangeType			= eChangeType.none;
			_velocityGradeIsInherited			= false;
			_velocityGradeInheritedFromNodeRID	= Include.NoRID;
            _velocityMinStock = -1;
            _velocityMaxStock = -1;
            _velocityMinAd = -1;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.VelocityGrade;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a store grade.
		/// </summary>
		public eChangeType VelocityGradeChangeType 
		{
			get { return _velocityGradeChangeType ; }
			set { _velocityGradeChangeType = value; }
		}
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Gets or sets the flag that the velocity grade was found.
		/// </summary>
		public bool VelocityGradeFound
		{
			get { return _velocityGradeFound; }
			set { _velocityGradeFound = value; }
		}
		//End TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Gets or sets the code for the store grade.
		/// </summary>
		public string VelocityGrade 
		{
			get { return _velocityGrade ; }
			set { _velocityGrade = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if velocity grade information is inherited.
		/// </summary>
		public bool VelocityGradeIsInherited 
		{
			get { return _velocityGradeIsInherited ; }
			set { _velocityGradeIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the velocity grade information is inherited from.
		/// </summary>
		public int VelocityGradeInheritedFromNodeRID 
		{
			get { return _velocityGradeInheritedFromNodeRID ; }
			set { _velocityGradeInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the boundary for the velocity grade.
		/// </summary>
		public int Boundary 
		{
			get { return _boundary ; }
			set { _boundary = value; }
		}
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Gets or sets the change type of the velocity min/maxes.
		/// </summary>
		public eChangeType VelocityMinMaxChangeType
		{
			get { return _velocityMinMaxChangeType; }
			set { _velocityMinMaxChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the flag that velocity min/maxes were found.
		/// </summary>
		public bool VelocityMinMaxFound
		{
			get { return _velocityMinMaxFound; }
			set { _velocityMinMaxFound = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if velocity min/max information is inherited.
		/// </summary>
		public bool VelocityMinMaxesIsInherited
		{
			get { return _velocityMinMaxesIsInherited; }
			set { _velocityMinMaxesIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the velocity min/max information is inherited from.
		/// </summary>
		public int VelocityMinMaxesInheritedFromNodeRID
		{
			get { return _velocityMinMaxesInheritedFromNodeRID; }
			set { _velocityMinMaxesInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the Min Stock for the velocity grade.
		/// </summary>
		public int VelocityMinStock
		{
			get { return _velocityMinStock; }
			set { _velocityMinStock = value; }
		}
		/// <summary>
		/// Gets or sets the Max Stock for the velocity grade.
		/// </summary>
		public int VelocityMaxStock
		{
			get { return _velocityMaxStock; }
			set { _velocityMaxStock = value; }
		}
		/// <summary>
		/// Gets or sets the Min Ad for the velocity grade.
		/// </summary>
		public int VelocityMinAd
		{
			get { return _velocityMinAd; }
			set { _velocityMinAd = value; }
		}
		//End TT#505 - JScott - Velocity - Apply Min/Max
	}

	/// <summary>
	/// Used to retrieve a list of store grades
	/// </summary>
	[Serializable()]
	public class VelocityGradeList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public VelocityGradeList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
