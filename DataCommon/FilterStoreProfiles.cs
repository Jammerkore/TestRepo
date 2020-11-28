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
        private bool _isLimited;
        private int _resultLimit;


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
		private StoreFilterProfile(int aKey, string aName, int aUserRID, int aOwnerUserRID, bool isLimited, int resultLimit)
			: base(aKey)
		{
			_name = aName;
			_userRID = aUserRID;
			_ownerUserRID = aOwnerUserRID;
            _isLimited = isLimited;
            _resultLimit = resultLimit;
		}
		//End Track #4815

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.FilterStore;
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
        public bool IsLimited
        {
            get
            {
                return _isLimited;
            }
            set
            {
                _isLimited = value;
            }
        }
        public int ResultLimit
        {
            get
            {
                return _resultLimit;
            }
            set
            {
                _resultLimit = value;
            }
        }
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
			return new StoreFilterProfile(_key, _name, _userRID, _ownerUserRID, _isLimited, _resultLimit);
		}
		//End Track #4815

		public void LoadFromDataRow(DataRow aRow)
		{
			_key = Convert.ToInt32(aRow["FILTER_RID"], CultureInfo.CurrentUICulture);
			_name = Convert.ToString(aRow["FILTER_NAME"], CultureInfo.CurrentUICulture);
			if (aRow["USER_RID"] == DBNull.Value)
			{
				_userRID = Include.AdministratorUserRID;
			}
			else
			{
				_userRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
			}
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
            if (aRow.Table.Columns.Contains("OWNER_USER_RID"))
            {
                if (aRow["OWNER_USER_RID"] == DBNull.Value)
                {
                    _ownerUserRID = Include.AdministratorUserRID;
                }
                else
                {
                    _ownerUserRID = Convert.ToInt32(aRow["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
                }
            }
            else
            {
                _ownerUserRID = _userRID;
            }
			
            //_isLimited = Include.ConvertIntToBool((int)aRow["IS_LIMITED"]);
            if (aRow["IS_LIMITED"] == DBNull.Value)
            {
                _isLimited = false;
            }
            else
            {
                _isLimited = (bool)aRow["IS_LIMITED"];
            }
            if (aRow["RESULT_LIMIT"] == DBNull.Value)
            {
                _resultLimit = 5000;
            }
            else
            {
                _resultLimit = Convert.ToInt32(aRow["RESULT_LIMIT"], CultureInfo.CurrentUICulture);
            }
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			aRow["FILTER_RID"] = _key;
			aRow["FILTER_NAME"] = _name;
			aRow["USER_RID"] = _userRID;
            aRow["IS_LIMITED"] = _isLimited;
            aRow["RESULT_LIMIT"] = _resultLimit;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			//aRow["OWNER_USER_RID"] = _ownerUserRID;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance

			return aRow;
		}
	}
}
