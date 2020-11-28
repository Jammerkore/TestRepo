using System;
using System.Collections;
using System.Collections.Generic; // TT#1185 - Verify Header ENQ before Update
using System.Data;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for DataLayer.
	/// </summary>
	/// 
	abstract public class DataLayer
	{
		public DatabaseAccess _dba = null;
		private ResourceLock _rl = null;
		//private Hashtable _rlHash = null;  // TT#1185 - Verify Header ENQ before Update
        private Dictionary<Int64, ResourceLock> _rlDictionary; // TT#1185 - Verify Header ENQ before Update
		private bool _connectionIsOpen = false;
        private bool _allowRetryOnDatabaseUpdate;

		public DataLayer()
		{
			try
			{
                _allowRetryOnDatabaseUpdate = true;  // TT#1185 - Verify Enq before update
				_dba = new DatabaseAccess();
                _rlDictionary = new Dictionary<Int64, ResourceLock>(); // TT#1185 - Verify Enq before Update
                //_rlHash = new Hashtable(); // TT#1185 - Verify Enq before Update
			}
			catch 
			{
				throw;
			}
		}
		public DataLayer(string aConnectionString)
		{
			try
			{
                _allowRetryOnDatabaseUpdate = true;  // TT#1185 - Verify Enq before update
				_dba = new DatabaseAccess(aConnectionString);
                _rlDictionary = new Dictionary<Int64, ResourceLock>(); // TT#1185 - Verify Enq before Update
                //_rlHash = new Hashtable(); // TT#1185 - Verify Enq before Update
            }
			catch 
			{
				throw;
			}
		}
		public DataLayer(DatabaseAccess dba)
		{
			try
			{
                _allowRetryOnDatabaseUpdate = true;  // TT#1185 - Verify Enq before update
                _dba = dba;
                _rlDictionary = new Dictionary<Int64, ResourceLock>(); // TT#1185 - Verify Enq before Update
                //_rlHash = new Hashtable(); // TT#1185 - Verify Enq before Update
            }
			catch 
			{
				throw;
			}
		}
        // begin TT#1185 - Verify Enq Before Update
        public DataLayer(bool aAllowRetryOnDatabaseUpdate)
        {
            _allowRetryOnDatabaseUpdate = aAllowRetryOnDatabaseUpdate;
            _dba = new DatabaseAccess();
            _rlDictionary = new Dictionary<Int64, ResourceLock>(); // TT#1185 - Verify Enq before Update
            //_rlHash = new Hashtable(); // TT#1185 - Verify Enq before Update
        }
        public DataLayer(string aConnectionString, bool aAllowRetryOnDatabaseUpdate)
        {
            _allowRetryOnDatabaseUpdate = aAllowRetryOnDatabaseUpdate;
            _dba = new DatabaseAccess(aConnectionString);
            _rlDictionary = new Dictionary<Int64, ResourceLock>(); // TT#1185 - Verify Enq before Update
            //_rlHash = new Hashtable(); // TT#1185 - Verify Enq before Update
        }
        public DataLayer(DatabaseAccess aDBA, bool aAllowRetryOnDatabaseUpdate)
        {
            _allowRetryOnDatabaseUpdate = aAllowRetryOnDatabaseUpdate;
            _dba = aDBA;
            _rlDictionary = new Dictionary<Int64, ResourceLock>(); // TT#1185 - Verify Enq before Update
            //_rlHash = new Hashtable(); // TT#1185 - Verify Enq before Update
        }
        // end TT#1185 - Verifry Enq Before Update

		public bool ConnectionIsOpen 
		{
			get 
            {
                // Begin TT#1243 - JSmith - Audit Performance
                //return _connectionIsOpen ; 
                if (_dba.UpdateConnectionState == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                // End TT#1243
            }
		}

		public DatabaseAccess DBA 
		{
			get { return _dba ; }
		}

        //Begin TT#1313-MD -jsobek -Header Filters -unused function
        //public DatabaseAccess GetDBA()
        //{
        //    return DBA;
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function

		public void OpenUpdateConnection()
		{
			try
			{
                //_dba.OpenUpdateConnection();                            // TT#1185 - Verify Enq Before Update
				_dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate); // TT#1185 - Verify Enq Before Update
				_connectionIsOpen = true;
			}
			catch 
			{
				throw;
			}
		}
		public void OpenUpdateConnection(eLockType lockType)
		{
			try
			{
                //_dba.OpenUpdateConnection();                            // TT#1185 - Verify Enq Before Update
                _dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate); // TT#1185 - Verify Enq Before Update
                _connectionIsOpen = true;
				_rl = new ResourceLock(_dba, lockType, "ALL");
			}
			catch 
			{
				throw;
			}
		}
        //Begin TT#1313-MD -jsobek -Header Filters -unused function
        //public void OpenUpdateConnection(eLockType lockType, int lockRID, string lockID)
        //{
        //    try
        //    {
        //        //_dba.OpenUpdateConnection();                            // TT#1185 - Verify Enq Before Update 
        //        _dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate); // TT#1185 - Verify Enq Before Update
        //        _connectionIsOpen = true;
        //        _rl = new ResourceLock(_dba, lockType, lockRID, lockID);
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function
		public void OpenUpdateConnection(eLockType lockType, int lockRID)
		{
			try
			{
                //_dba.OpenUpdateConnection();                            // TT#1185 - Verify Enq Before Update
                _dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate); // TT#1185 - Verify Enq Before Update
                _connectionIsOpen = true;
				_rl = new ResourceLock(_dba, lockType, lockRID);
			}
			catch 
			{
				throw;
			}
		}
		public void OpenUpdateConnection(eLockType lockType, string lockID)
		{
			try
			{
                //_dba.OpenUpdateConnection();                            // TT#1185 - Verify Enq Before Update
                _dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate); // TT#1185 - Verify Enq Before Update
                _connectionIsOpen = true;
				_rl = new ResourceLock(_dba, lockType, lockID);
			}
			catch 
			{
				throw;
			}
		}

        //Begin TT#1313-MD -jsobek -Header Filters -unused function
		// BEGIN MID stodd multi-header changes
		/// <summary>
		/// opens DB connection for update and Locks all keys in list.
		/// </summary>
		/// <param name="lockType"></param>
		/// <param name="lockRID"></param>
        //public void OpenUpdateConnection(eLockType lockType, ArrayList lockRIDList)
        //{
        //    try
        //    {
        //        //_dba.OpenUpdateConnection();                            // TT#1185 - Verify Enq Before Update
        //        _dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate); // TT#1185 - Verify Enq Before Update
        //        _connectionIsOpen = true;
        //        AddResourceLocks(lockType, lockRIDList); // TT#1185 - Verify Enq Before Update

        //        // begin TT#1185 - Verify Enq before update
        //        //ResourceLock rl = null;
        //        //foreach(int lockRid in lockRIDList)
        //        //{
        //        //    if (!_rlHash.ContainsKey(lockRid))
        //        //    {
        //        //        rl = new ResourceLock(_dba, lockType, lockRid);
        //        //        _rlHash.Add(lockRid, rl);
        //        //    }
        //        //}
        //        // end TT#1185 - Verify Enq before update
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function

        // begin TT#1185 - Verify ENQ before Update
        public void OpenUpdateConnection(List<Resource> aResourceList)
        {
            try
            {
                _dba.OpenUpdateConnection(_allowRetryOnDatabaseUpdate);
                _connectionIsOpen = true;
                AddResourceLocks(aResourceList);
            }
            catch
            {
                throw;
            }
        }
        // end TT#1185 - Verify ENQ before Update

        //Begin TT#1313-MD -jsobek -Header Filters -unused function
        //public void AddResourceLocks(eLockType lockType, ArrayList lockRIDList)
        //{
        //    // begin TT#1185 - Verify Enq before update
        //    List<Resource> resourceList = new List<Resource>();
        //    foreach (int lockRID in lockRIDList)
        //    {
        //        resourceList.Add(new Resource(lockType, lockRID, string.Empty));
        //    }
        //    AddResourceLocks(resourceList);
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function
        public void AddResourceLocks(List<Resource> aResourceList)
        {
			try
			{
				ResourceLock rl = null;
                Int64 rlLastKey = -1;
                Int64 rlKey; // TT#1185 - Verify Enq Before Update
                //foreach (int lockRid in lockRIDList)  // TT#1185 - Verify ENQ before update
                aResourceList.Sort();
                foreach (Resource resource in aResourceList)
                {
                    // begin TT#1185 - Verify Enq before update
                    rlKey = ((Int64)resource.LockType << 32) + resource.LockRID;
                    if (rlKey != rlLastKey
                        && !_rlDictionary.TryGetValue(rlKey, out rl))
                    {
                        rl = new ResourceLock(_dba, resource.LockType, resource.LockRID, resource.LockID);
                        _rlDictionary.Add(rlKey, rl);
                    }
                    rlLastKey = rlKey;
                }
                //foreach (int lockRid in lockRIDList)
                //{
                    //if (!_rlHash.ContainsKey(lockRid))
                    //{
                    //    rl = new ResourceLock(_dba, lockType, lockRid);
                    //    _rlHash.Add(lockRid, rl);
                    //}
                //}
                // end TT#1185 - Verify Enq before update
            }
			catch 
			{
				throw;
			}
		}		


		// END MID stodd multi-header changes

        // Begin TT#739-MD - JSmith - Delete Stores
        //public void CommitData()
        public virtual void CommitData()
        // End TT#739-MD - JSmith - Delete Stores
		{
			try
			{
				if (_rl != null)
				{
					_rl.Delete();
					_rl = null;		// lock is only good for one commit
				}

				// BEGIN MID stodd multi-header changes
				//foreach(ResourceLock rl in _rlHash.Values) // TT#1185 - Verify Enq before update
                foreach(ResourceLock rl in _rlDictionary.Values) // TT#1185 - Verfiry Enq before update
				{
					rl.Delete();
				}
                _rlDictionary.Clear(); // TT#1185 - Verfiry Enq before update
				//_rlHash.Clear();
				// END MID stodd multi-header changes

				_dba.CommitData();
			}
			catch 
			{
				throw;
			}
		}

        // Begin TT#2255 - JSmith - Getting an error code loading running the hierarchy load
        public void RemoveLocks()
        {
            try
            {
                if (_rl != null)
                {
                    _rl.Delete();
                    _rl = null;
                }

                foreach (ResourceLock rl in _rlDictionary.Values) 
                {
                    rl.Delete();
                }
                _rlDictionary.Clear();
            }
            catch
            {
                throw;
            }
        }
        // End TT#2255

		public void CloseUpdateConnection()
		{
			try
			{
				if (_connectionIsOpen)
				{
					_dba.CloseUpdateConnection();
					_connectionIsOpen = false;
				}
			}
			catch 
			{
				throw;
			}
		}

		public void Rollback()
		{
			try
			{
				_dba.RollBack();
			}
			catch 
			{
				throw;
			}
		}

		public string GetDatabaseName()
		{
			try
			{
				return _dba.GetDBName();
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Executes a non query command in the database
		/// </summary>
		public void ExecuteNonQuery(string command)
		{
			try
			{
				_dba.ExecuteNonQuery(command);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        /// <summary>
        /// Executes a  query command in the database
        /// </summary>
        public DataTable ExecuteSQLQuery(string aCommand, string aTableName)
        {
            try
            {
                return _dba.ExecuteSQLQuery(aCommand, aTableName);

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //Begin TT#1430-MD -jsobek -Null reference after canceling a product search
        public DataTable ExecuteSQLQuery(string aCommand, string aTableName, int commandTimeOut)
        {
            try
            {
                return _dba.ExecuteSQLQuery(aCommand, aTableName, commandTimeOut);

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1430-MD -jsobek -Null reference after canceling a product search

        // Begin TT#310 - JSmith - HeaderCharacteristic length error in workspace
        /// <summary>
        /// Return the size of a column in a database table
        /// </summary>
        public int GetColumnSize(string aTableName, string aColumnName)
        {
            try
            {
                return _dba.GetColumnSize(aTableName, aColumnName);

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#310

	}
}
