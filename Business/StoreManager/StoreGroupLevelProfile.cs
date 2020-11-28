using System;
using System.IO;
using System.Collections;
using System.Data;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Used to hold the information for a single Store Group Level.
	/// </summary>
	[Serializable()]
	public class StoreGroupLevelProfile : Profile
	{
		private string _SGL_ID;		//Store_Group_Level Name - Name
		private int _SGL_SEQUENCE;	//Store_Group_Level Sequence within Group - Sequence
		private int _sg_rid;
        private int _levelVersion = Include.NoRID;
        private eGroupLevelTypes _levelType;
		private ProfileList _stores;
        //private bool _storesFilled = false;

		public string Name 
		{
			get { return _SGL_ID ; }
			set { _SGL_ID = value; }
		}
		public int Sequence 
		{
			get { return _SGL_SEQUENCE ; }
			set { _SGL_SEQUENCE = value; }
		}

		


		/// <summary>
		/// List of all store profiles belonging to store group level whether Static or Dynamic.
		/// </summary>
		public ProfileList Stores 
		{
			get { return _stores ; }
			set { _stores = value; }
		}

        //public bool StoresFilled
        //{
        //    get { return _storesFilled; }
        //    set { _storesFilled = value; }
        //}


		public int GroupRid 
		{
			get { return _sg_rid ; }
			set { _sg_rid = value; }
		}

        //Begin TT#1517-MD -jsobek -Store Service Optimization
        public int LevelVersion
        {
            get { return _levelVersion; }
            set { _levelVersion = value; }
        }
        public eGroupLevelTypes LevelType
        {
            get { return _levelType; }
            set { _levelType = value; }
        }
        //End TT#1517-MD -jsobek -Store Service Optimization

		public StoreGroupLevelProfile(int aKey)
			: base(aKey)
		{
			//this.GroupLevelRID = aKey;
			_stores = new ProfileList(eProfileType.Store);
			//_sqlStatementList = new ArrayList();
			//_staticStoreList = new ArrayList();
		}

		public override bool Equals(object obj)
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

		public override int GetHashCode()
		{
			return this.Key;
		}

		public int CompareTo(StoreGroupLevelProfile x)    
		{      
			int result = this.Sequence.CompareTo(x.Sequence);
			return result;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreGroupLevel;
			}
		}

		/// <summary>
		/// Makes a deep copy of this StoreGroupLevelProfile.
		/// does NOT copy stores.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			StoreGroupLevelProfile sglp = new StoreGroupLevelProfile(_key);
			sglp.Name = this._SGL_ID;
			sglp.Sequence = this._SGL_SEQUENCE;
			//sglp.SqlStatement = this._sqlStatement;
			sglp.GroupRid = this._sg_rid;
            //foreach (StoreGroupLevelStatementItem sglItem in _sqlStatementList)
            //{
            //    StoreGroupLevelStatementItem sglClone = sglItem.Clone();
            //    sglp.SqlStatementList.Add(sglClone);
            //}
            //foreach (int storeRid in _staticStoreList)
            //{
            //    sglp.StaticStoreList.Add(storeRid);
            //}
			return sglp;
		}

        //public object Clone(bool fillWithStores)
        //{
        //    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)Clone();
        //    if (fillWithStores)
        //    {
        //        sglp.Stores = new ProfileList(eProfileType.Store);
        //        int storeCnt = _stores.Count;
        //        for (int j=0;j<storeCnt;j++)
        //        {
        //            StoreProfile sp = (StoreProfile)_stores[j];
        //            StoreProfile spClone = (StoreProfile)sp.Clone();
        //            sglp.Stores.Add(spClone);
        //        }
        //    }
        //    return sglp;
        //}

        // override ToString(); this is what the checkbox control displays as text
        public override string ToString()
        {
            return this.Name;
        }
	}

	[Serializable()]
	public class StoreGroupLevelListViewProfile : Profile
	{
		private string _SGL_ID;		//Store_Group_Level Name - Name
		private int _SGL_SEQUENCE;	//Store_Group_Level Sequence within Group - Sequence
		private int _sg_rid;
		private ProfileList _stores;

		public string Name 
		{
			get { return _SGL_ID ; }
			set { _SGL_ID = value; }
		}
		public int Sequence 
		{
			get { return _SGL_SEQUENCE ; }
			set { _SGL_SEQUENCE = value; }
		}
		public int GroupRid 
		{
			get { return _sg_rid ; }
			set { _sg_rid = value; }
		}
		/// <summary>
		/// ProfileList of StoreProfiles
		/// </summary>
		public ProfileList Stores 
		{
			get { return _stores ; }
			set { _stores = value; }
		}
		
		public StoreGroupLevelListViewProfile(int aKey)
			: base(aKey)
		{
			_stores = null;
		}

		public override bool Equals(object obj)
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

		public override int GetHashCode()
		{
			return this.Key;
		}

		public int CompareTo(StoreGroupLevelListViewProfile x)    
		{      
			int result = this.Sequence.CompareTo(x.Sequence);
			return result;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreGroupLevelListView;
			}
		}

        // override ToString(); this is what the checkbox control displays as text
        public override string ToString()
        {
            return this.Name;
        }
	}


	/// <summary>
	/// used to put Store Group Level lists in Sequence number order
	/// </summary>
	public class SGLSequenceComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is StoreGroupLevelProfile) && !(y is StoreGroupLevelProfile))        
			{          
				throw new ArgumentException("only allows StoreGroupLevelProfile objects");        
			}        
			return ((StoreGroupLevelProfile)x).CompareTo((StoreGroupLevelProfile)y);      
		}    
	}

	/// <summary>
	/// used to put Store Group Level List View lists in Sequence number order
	/// </summary>
	public class SGLLVSequenceComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is StoreGroupLevelListViewProfile) && !(y is StoreGroupLevelListViewProfile))        
			{          
				throw new ArgumentException("only allows StoreGroupLevelListViewProfile objects");        
			}        
			return ((StoreGroupLevelListViewProfile)x).CompareTo((StoreGroupLevelListViewProfile)y);      
		}    
	}


}
