using System;
using System.Data;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	public class FolderProfile : Profile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		private int _userRID;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//private eFolderType _folderType;
		private eProfileType _folderType;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private string _name;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private int _ownerUserRID;
		//End Track #4815

		//=============
		// CONSTRUCTORS
		//=============

		public FolderProfile(int aKey)
			: base(aKey)
		{
			_userRID = -1;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_folderType = eFolderType.None;
			_folderType = eProfileType.None;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			_name = "";
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			_ownerUserRID = 0;
			//End Track #4815
		}

		public FolderProfile(DataRow aRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		////		public FolderProfile(int aKey, int aUserRID, eFolderType aFolderType, string aName)
		////			: base(aKey)
		////		{
		////			_userRID = aUserRID;
		////			_folderType = aFolderType;
		////			_name = aName;
		////		}
		//public FolderProfile(int aKey, int aUserRID, eFolderType aFolderType, string aName, int aOwnerUserRID)
		//    : base(aKey)
		//{
		//    _userRID = aUserRID;
		//    _folderType = aFolderType;
		//    _name = aName;
		//    _ownerUserRID = aOwnerUserRID;
		//}
		////End Track #4815
		public FolderProfile(int aKey, int aUserRID, eProfileType aFolderType, string aName, int aOwnerUserRID)
			: base(aKey)
		{
			_userRID = aUserRID;
			_folderType = aFolderType;
			_name = aName;
			_ownerUserRID = aOwnerUserRID;
		}
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
                // Begin Track #5005 - JSmith - Explorer Organization
                //return eProfileType.Folder;
                return _folderType;
                // End Track #5005
			}
		}

		public int UserRID
		{
			get
			{
				return _userRID;
			}
			set
			{
				_userRID = value;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public eFolderType FolderType
		public eProfileType FolderType
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
			get
			{
				return _folderType;
			}
			set
			{
				_folderType = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		public int OwnerUserRID
		{
			get
			{
				return _ownerUserRID;
			}
			set
			{
				_ownerUserRID = value;
			}
		}
		//End Track #4815

		//========
		// METHODS
		//========

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//		public object Clone()
//		{
//			try
//			{
//				return new FolderProfile(_key, _userRID, _folderType, _name);
//			}
//			catch (Exception exc)
//			{
//				string message = exc.ToString();
//				throw;
//			}
//		}
		public object Clone()
		{
			try
			{
				return new FolderProfile(_key, _userRID, _folderType, _name, _ownerUserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #4815

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["FOLDER_RID"], CultureInfo.CurrentUICulture);
				_userRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//_folderType = (eFolderType)Convert.ToInt32(aRow["FOLDER_TYPE"], CultureInfo.CurrentUICulture);
				_folderType = (eProfileType)Convert.ToInt32(aRow["FOLDER_TYPE"], CultureInfo.CurrentUICulture);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_name = (aRow["FOLDER_ID"] != DBNull.Value) ? Convert.ToString(aRow["FOLDER_ID"], CultureInfo.CurrentUICulture) : null;
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
				_ownerUserRID = Convert.ToInt32(aRow["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
				//End Track #4815
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["FOLDER_RID"] = _key;
				aRow["USER_RID"] = _userRID;
				aRow["FOLDER_TYPE"] = _folderType;
				aRow["FOLDER_ID"] = _name;

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

    // Begin Track #5005 - JSmith - Explorer Organization
    public class FolderRelationship
    {
        //=======
        // FIELDS
        //=======

        private Profile _parentProfile;
        private Profile _childProfile;

        //=============
        // CONSTRUCTORS
        //=============

        public FolderRelationship(Profile aParentProfile, Profile aChildProfile)
        {
            _parentProfile = aParentProfile;
            _childProfile = aChildProfile;
        }
        
        //===========
        // PROPERTIES
        //===========

        public Profile ParentProfile
        {
            get
            {
                return _parentProfile;
            }
            set
            {
                _parentProfile = value;
            }
        }

        public Profile ChildProfile
        {
            get
            {
                return _childProfile;
            }
            set
            {
                _childProfile = value;
            }
        }

        //========
        // METHODS
        //========
    }
// End Track #5005
}