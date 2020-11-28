using System;
using System.Data;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	[Serializable]
	public class StoreFilterProfile : Profile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		private string _name;
		private int _userRID;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private int _ownerUserRID;
		//End Track #4815

		//=============
		// CONSTRUCTORS
		//=============

		public StoreFilterProfile(int aKey)
			: base(aKey)
		{
			_name = "";
			_userRID = 0;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			_ownerUserRID = 0;
			//End Track #4815
		}

		public StoreFilterProfile(DataRow aRow)
			: base(-1)
		{
			LoadFromDataRow(aRow);
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//		private StoreFilterProfile(int aKey, string aName, int aUserRID)
//			: base(aKey)
//		{
//			_name = aName;
//			_userRID = aUserRID;
//		}
		private StoreFilterProfile(int aKey, string aName, int aUserRID, int aOwnerUserRID)
			: base(aKey)
		{
			_name = aName;
			_userRID = aUserRID;
			_ownerUserRID = aOwnerUserRID;
		}
		//End Track #4815

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreFilter;
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
//			return new StoreFilterProfile(_key, _name, _userRID);
//		}
		public object Clone()
		{
			return new StoreFilterProfile(_key, _name, _userRID, _ownerUserRID);
		}
		//End Track #4815

		private void LoadFromDataRow(DataRow aRow)
		{
			_key = Convert.ToInt32(aRow["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
			_name = Convert.ToString(aRow["STORE_FILTER_NAME"], CultureInfo.CurrentUICulture);
			if (aRow["USER_RID"] == DBNull.Value)
			{
				_userRID = Include.AdministratorUserRID;
			}
			else
			{
				_userRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
			}
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			if (aRow["OWNER_USER_RID"] == DBNull.Value)
			{
				_ownerUserRID = Include.AdministratorUserRID;
			}
			else
			{
				_ownerUserRID = Convert.ToInt32(aRow["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
			}
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			aRow["STORE_FILTER_RID"] = _key;
			aRow["STORE_FILTER_NAME"] = _name;
			aRow["USER_RID"] = _userRID;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			//aRow["OWNER_USER_RID"] = _ownerUserRID;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance

			return aRow;
		}
	}
}
