using System;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Used to hold information about low level version overrides.
	/// </summary>
	/// <remarks>
	/// The key is the record ID of the low level node
	/// </remarks>
	[Serializable()]
	public class LowLevelVersionOverrideProfile : Profile
	{
		private HierarchyNodeProfile	_nodeProfile;
		private bool					_versionIsOverridden;
        private HierarchyNodeProfile    _versionOverrideNodeProfile;
		private VersionProfile			_versionProfile;
        private bool                    _excludeIsOverridden;
        private HierarchyNodeProfile    _excludeOverrideNodeProfile;
		private bool					_exclude;
		private VersionProfile			_OverrideVersionProfile;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public LowLevelVersionOverrideProfile(int aKey)
			: base(aKey)
		{
            CommonLoad();
			_exclude = false;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public LowLevelVersionOverrideProfile(int aKey, HierarchyNodeProfile aNodeProfile, bool aVersionIsOverridden, VersionProfile	aVersionProfile, bool aExclude)
			: base(aKey)
		{
            CommonLoad();
			_nodeProfile = aNodeProfile;
			_versionIsOverridden = aVersionIsOverridden;
			_versionProfile = aVersionProfile;
			_exclude = aExclude;
		}

        private void CommonLoad()
        {
            _nodeProfile = null;
            _versionIsOverridden = false;
            _versionOverrideNodeProfile = null;
            _excludeIsOverridden = false;
            _excludeOverrideNodeProfile = null;
        }

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.LowLevelVersionOverride;
			}
		}

		/// <summary>
		/// Gets or sets the HierarchyNodeProfile of the low level node.
		/// </summary>
		public HierarchyNodeProfile NodeProfile 
		{
			get { return _nodeProfile ; }
			set { _nodeProfile = value; }
		}

		/// <summary>
		/// Gets or sets a flag identifying if the version has been overridden for this node.
		/// </summary>
		public bool VersionIsOverridden 
		{
			get { return _versionIsOverridden ; }
			set { _versionIsOverridden = value; }
		}

		/// <summary>
		/// Gets or sets the selected version.
		/// </summary>
		public VersionProfile VersionProfile 
		{
			get { return _versionProfile ; }
			set { _versionProfile = value; }
		}

		/// <summary>
		/// Gets or sets whether the low level node is to be excluded.
		/// </summary>
		public bool Exclude 
		{
			get { return _exclude ; }
			set { _exclude = value; }
		}

		// BEGIN Issue 4858 stodd 11.15.2007 method security
		/// <summary>
		/// Gets or sets the override version.
		/// </summary>
		public VersionProfile OverrideVersionProfile 
		{
			get { return _OverrideVersionProfile ; }
			set { _OverrideVersionProfile = value; }
		}
		// END Issue 4858 stodd 11.15.2007 method security

        /// <summary>
        /// Gets or sets the profile of the node where the version was overridden.
        /// </summary>
        public HierarchyNodeProfile VersionOverrideNodeProfile
        {
            get { return _versionOverrideNodeProfile; }
            set { _versionOverrideNodeProfile = value; }
        }

        /// <summary>
        /// Gets or sets a flag identifying if the exclude flag has been overridden for this node.
        /// </summary>
        public bool ExcludeIsOverridden
        {
            get { return _excludeIsOverridden; }
            set { _excludeIsOverridden = value; }
        }

        /// <summary>
        /// Gets or sets the profile of the node where the exclude flag was overridden.
        /// </summary>
        public HierarchyNodeProfile ExcludeOverrideNodeProfile
        {
            get { return _excludeOverrideNodeProfile; }
            set { _excludeOverrideNodeProfile = value; }
        }

		public LowLevelVersionOverrideProfile Copy()
		{
			try
			{
				LowLevelVersionOverrideProfile llvop = (LowLevelVersionOverrideProfile)this.MemberwiseClone();
				llvop.Key = Key;
				llvop.NodeProfile = NodeProfile;
				llvop.VersionIsOverridden = VersionIsOverridden;
				llvop.VersionProfile = VersionProfile;
				llvop.Exclude = Exclude;
				llvop.OverrideVersionProfile = OverrideVersionProfile;
				return llvop;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Used to retrieve a list of low level version override profiles
	/// </summary>
	[Serializable()]
	public class LowLevelVersionOverrideProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public LowLevelVersionOverrideProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	//Begin Track #4371 - JSmith - Multi-level forecasting.
	/// <summary>
	/// Used to hold information about low level exclusions.
	/// </summary>
	/// <remarks>
	/// The key is the record ID of the low level node
	/// </remarks>
	[Serializable()]
	public class LowLevelExcludeProfile : Profile
	{
		private HierarchyNodeProfile	_nodeProfile;
		private bool					_exclude;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public LowLevelExcludeProfile(int aKey)
			: base(aKey)
		{
			_exclude = false;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public LowLevelExcludeProfile(int aKey, HierarchyNodeProfile aNodeProfile, bool aExclude)
			: base(aKey)
		{
			_nodeProfile = aNodeProfile;
			_exclude = aExclude;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.LowLevelExclusions;
			}
		}

		/// <summary>
		/// Gets or sets the HierarchyNodeProfile of the low level node.
		/// </summary>
		public HierarchyNodeProfile NodeProfile 
		{
			get { return _nodeProfile ; }
			set { _nodeProfile = value; }
		}

		/// <summary>
		/// Gets or sets whether the low level node is to be excluded.
		/// </summary>
		public bool Exclude 
		{
			get { return _exclude ; }
			set { _exclude = value; }
		}

		public LowLevelExcludeProfile Copy()
		{
			try
			{
				LowLevelExcludeProfile llep = (LowLevelExcludeProfile)this.MemberwiseClone();
				llep.Key = Key;
				llep.NodeProfile = NodeProfile;
				llep.Exclude = Exclude;
				return llep;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Used to retrieve a list of low level exclusion profiles
	/// </summary>
	[Serializable()]
	public class LowLevelExclusionProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public LowLevelExclusionProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}
	//End Track #4371

}
