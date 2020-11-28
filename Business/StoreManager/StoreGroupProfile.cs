using System;
using System.IO;
using System.Collections;
using System.Data;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Reflection.Emit;

namespace MIDRetail.Business
{
	[Serializable()]
	public class StoreGroupProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		protected string _name;
		protected ProfileList _groupLevels;
		protected bool _levelsAndStoresFilled;
		protected bool _isDynamicGroup;
		protected int _ownerUserRID;
        protected int _filterRID;
        protected int _version;
		//private ArrayList _dynamicGroupDesc;
		private bool _visible;

		//=============
		// CONSTRUCTORS
		//=============

		public StoreGroupProfile(int aKey)
			: base(aKey)
		{
			_groupLevels = new ProfileList(eProfileType.StoreGroupLevel);
			_levelsAndStoresFilled = false;
			_isDynamicGroup = false;
			//_dynamicGroupDesc = new ArrayList();
			_visible = true;
		}

		public StoreGroupProfile(StoreGroupListViewProfile aStrGrpLstViewProf)
			: base(aStrGrpLstViewProf.Key)
		{
			_name = aStrGrpLstViewProf._name;
			_levelsAndStoresFilled = false;
			_isDynamicGroup = aStrGrpLstViewProf._isDynamicGroup;
			_ownerUserRID = aStrGrpLstViewProf._ownerUserRID;
            _filterRID = aStrGrpLstViewProf.FilterRID;
            _version = aStrGrpLstViewProf.Version;
			_visible = aStrGrpLstViewProf._visible;
			_groupLevels = new ProfileList(eProfileType.StoreGroupLevel);
			//_dynamicGroupDesc = new ArrayList();
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreGroup;
			}
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string GroupId
		{
			get { return _name; }
			set { _name = value; }
		}

		public ProfileList GroupLevels
		{
			get { return _groupLevels; }
			set { _groupLevels = value; }
		}

		public bool LevelsAndStoresFilled
		{
			get { return _levelsAndStoresFilled; }
			set { _levelsAndStoresFilled = value; }
		}

		public bool IsDynamicGroup
		{
			get { return _isDynamicGroup; }
			set { _isDynamicGroup = value; }
		}

        //public ArrayList DynamicGroupDesc
        //{
        //    get { return _dynamicGroupDesc; }
        //    set { _dynamicGroupDesc = value; }
        //}

		public int OwnerUserRID
		{
			get { return _ownerUserRID; }
			set { _ownerUserRID = value; }
		}

        public int FilterRID
        {
            get { return _filterRID; }
            set { _filterRID = value; }
        }

        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		//========
		// METHODS
		//========

		override public int GetHashCode()
		{
			return Key;
		}

		override public bool Equals(object obj)
		{
			try
			{
				if (obj == null || obj.GetType() == typeof(System.DBNull) || ((Profile)obj).ProfileType != ProfileType)
				{
					return Key == Include.NoRID;
				}
				else
				{
					return (Key == ((Profile)obj).Key);
				}
			}
			catch
			{
				throw;
			}
		}

		override public string ToString()
		{
			return _name;
		}

		public int CompareTo(StoreGroupProfile aStrGrpProf)
		{
			try
			{
				return _name.CompareTo(aStrGrpProf._name);
			}
			catch
			{
				throw;
			}
		}

		virtual public ProfileList GetGroupLevelList(bool aFillStores)
		{
			try
			{
				return _groupLevels;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Makes a deep copy of this StoreGroupProfile.
		/// Note: if a StoreGroupLevelProfile in the GroupLevels list contains Stores,
		/// the Stores are NOT cloned.  That is the Stores list is empty in all StoreGroupLevelProfiles.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			StoreGroupProfile sgp;
			StoreGroupLevelProfile sglp;
			int levelCnt;
			int i;

			try
			{
				sgp = new StoreGroupProfile(_key);
				sgp.Name = this._name;
				sgp.IsDynamicGroup = this._isDynamicGroup;
				sgp.OwnerUserRID = this._ownerUserRID;
                sgp.FilterRID = this._filterRID;
                sgp.Version = this._version;
				sgp.Visible = this._visible;
				sgp.LevelsAndStoresFilled = false;

				levelCnt = this._groupLevels.Count;

				for (i = 0; i < levelCnt; i++)
				{
					sglp = (StoreGroupLevelProfile)this._groupLevels[i];
					sgp.GroupLevels.Add((StoreGroupLevelProfile)sglp.Clone());
				}

				return sgp;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Makes a deep copy of this StoreGroupProfile: with or without stores populated.
		/// </summary>
		/// <param name="fillWithStores"></param>
		/// <returns></returns>

        //public object Clone(bool fillWithStores)
        //{
        //    StoreGroupProfile sgp;
        //    StoreGroupLevelProfile sglp;
        //    int levelCnt;
        //    int i;

        //    try
        //    {
        //        sgp = new StoreGroupProfile(_key);
        //        sgp.Name = this._name;
        //        sgp.IsDynamicGroup = this._isDynamicGroup;
        //        sgp.OwnerUserRID = this._ownerUserRID;
        //        sgp.FilterRID = this._filterRID;
        //        sgp.Filled = false;

        //        levelCnt = this._groupLevels.Count;

        //        for (i = 0; i < levelCnt; i++)
        //        {
        //            sglp = (StoreGroupLevelProfile)this._groupLevels[i];

        //            if (fillWithStores)
        //            {
        //                sgp.GroupLevels.Add((StoreGroupLevelProfile)sglp.Clone(true));
        //            }
        //            else
        //            {
        //                sgp.GroupLevels.Add((StoreGroupLevelProfile)sglp.Clone());
        //            }
        //        }

        //        return sgp;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
	}

	[Serializable()]
	public class StoreGroupListViewProfile : StoreGroupProfile
	{
		//=======
		// FIELDS
		//=======

		private bool _storesFilled;

		//=============
		// CONSTRUCTORS
		//=============

		public StoreGroupListViewProfile(int aKey)
			: base(aKey)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreGroupListView;
			}
		}

		//========
		// METHODS
		//========

		override public ProfileList GetGroupLevelList(bool aFillStores)
		{
			try
			{
				if (_groupLevels == null || !_levelsAndStoresFilled || (aFillStores && !_storesFilled))
				{
                    _groupLevels = StoreMgmt.StoreGroup_GetLevelListViewList(Key, aFillStores); //aStoreSession.GetStoreGroupLevelListViewList(Key, aFillStores);
					_levelsAndStoresFilled = true;
					_storesFilled = aFillStores;
				}

				return _groupLevels;
			}
			catch
			{
				throw;
			}
		}
	}

	/// <summary>
	/// used to put Store Group lists in alpha order
	/// </summary>
	public class SGSequenceComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if (!(x is StoreGroupProfile) && !(y is StoreGroupProfile))
			{
				throw new ArgumentException("only allows StoreGroupProfile objects");
			}
			return ((StoreGroupProfile)x).CompareTo((StoreGroupProfile)y);
		}
	}

	/// <summary>
	/// used to put Store Group lists in alpha order
	/// </summary>
	public class SGLVSequenceComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if (!(x is StoreGroupListViewProfile) && !(y is StoreGroupListViewProfile))
			{
				throw new ArgumentException("only allows StoreGroupListViewProfile objects");
			}
			return ((StoreGroupListViewProfile)x).CompareTo((StoreGroupListViewProfile)y);
		}
	}
}

