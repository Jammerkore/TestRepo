using System;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
    // begin tt#1185 - Verify ENQ before Update 
	[Serializable]
    public struct Resource : IComparable
    {
        private eLockType _lockType;
        private int _lockRID;
        private string _lockID;
        public Resource(eLockType aLockType, int aLockRID, string aLockID)
        {
            _lockType = aLockType;
            _lockRID = aLockRID;
            _lockID = aLockID;
        }
        public eLockType LockType
        {
            get { return _lockType; }
        }
        public int LockRID
        {
            get { return _lockRID; }
        }
        public string LockID
        {
            get { return _lockID; }
        }
        public int CompareTo(object obj)
        {
            Resource r;
            if (obj.GetType() == typeof(Resource))
            {
                r = (Resource)obj;
                if (this.LockType == r.LockType)
                {
                    if (this.LockRID < r.LockRID)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (this.LockType < r.LockType)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
    // end TT#1185 - Verify ENQ before Update

	/// <summary>
	/// Resource locking class.
	/// </summary>
    [Serializable]
	public partial class ResourceLock
	{
		private DatabaseAccess _dba;
		private eLockType _lockType;
		private int _lockRID;
		private string _lockID;

		public ResourceLock(DatabaseAccess dba, eLockType lockType, int lockRID, string lockID)
		{
			_dba = dba;
			_lockType = lockType;
			_lockRID = lockRID;
			_lockID = lockID;
			Insert();
		}
		public ResourceLock(DatabaseAccess dba, eLockType lockType, int lockRID)
		{
			_dba = dba;
			_lockType = lockType;
			_lockRID = lockRID;
			_lockID = "";
			Insert();
		}
		public ResourceLock(DatabaseAccess dba, eLockType lockType, string lockID)
		{
			_dba = dba;
			_lockType = lockType;
			_lockRID = Include.NoRID;
			_lockID = lockID;
			Insert();
		}

		/// <summary>
		/// Inserts a resource lock
		/// </summary>
		/// <returns></returns>
		private void Insert()
		{
            //Begin TT#4384 -jsobek -History Load Error -keep virtual lock calls non-static
            //StoredProcedures.MID_VIRTUAL_LOCK_INSERT.Insert(_dba,
            //                                                    LOCK_TYPE: (int)_lockType,
            //                                                    LOCK_ID: _lockRID.ToString(CultureInfo.CurrentUICulture) + "~" + _lockID
            //                                                    );
            MIDDbParameter[] InParams = { new MIDDbParameter("@LOCK_TYPE", (int)_lockType, eDbType.Int, eParameterDirection.Input),
										  new MIDDbParameter("@LOCK_ID", _lockRID.ToString(CultureInfo.CurrentUICulture) + "~" + _lockID, eDbType.Text, eParameterDirection.Input)};

            _dba.ExecuteStoredProcedure("MID_VIRTUAL_LOCK_INSERT", InParams);
            //End TT#4384 -jsobek -History Load Error -keep virtual lock calls non-static
		}


		/// <summary>
		/// Deletes a resource lock
		/// </summary>
		/// <returns></returns>
		public void Delete()
		{
            //Begin TT#4384 -jsobek -History Load Error -keep virtual lock calls non-static
            //StoredProcedures.MID_VIRTUAL_LOCK_DELETE.Delete(_dba,
            //                                                    LOCK_TYPE: (int)_lockType,
            //                                                    LOCK_ID: _lockRID.ToString(CultureInfo.CurrentUICulture) + "~" + _lockID
            //                                                    );
            MIDDbParameter[] InParams = { new MIDDbParameter("@LOCK_TYPE", (int)_lockType, eDbType.Int, eParameterDirection.Input),
										  new MIDDbParameter("@LOCK_ID", _lockRID.ToString(CultureInfo.CurrentUICulture) + "~" + _lockID, eDbType.Text, eParameterDirection.Input)};

            _dba.ExecuteStoredProcedure("MID_VIRTUAL_LOCK_DELETE", InParams);
            //End TT#4384 -jsobek -History Load Error -keep virtual lock calls non-static
		}

	}
}


