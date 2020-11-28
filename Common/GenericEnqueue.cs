using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class GenericConflictException : Exception
	{
	}

	/// <summary>
	/// The GenericConflict class stores information regarding a conflict during the enqueue of a generic object.
	/// </summary>

	public struct GenericConflict
	{
		//=======
		// FIELDS
		//=======

		private int _userRID;
		private string _userName;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of GenericConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public GenericConflict(int aUserRID, string aUserName)
		{
			_userRID = aUserRID;
			_userName = aUserName;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the user RID of the conflict.
		/// </summary>

		public int UserRID
		{
			get
			{
				return _userRID;
			}
		}

		/// <summary>
		/// Gets the user name of the conflict.
		/// </summary>

		public string UserName
		{
			get
			{
				return _userName;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The GenericEnqueue class provides functionality for enqueuing a generic object before using.
	/// </summary>
	/// <remarks>
	/// A generic object is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class GenericEnqueue
	{
		//=======
		// FIELDS
		//=======

		private eLockType _lockType;
		private int _objectRID;
		private int _userRID;
		private int _clientThreadID;

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of GenericEnqueue using the given generic object information.
		/// </summary>
		/// <param name="aObjectRID">
		/// The record ID of the generic object.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public GenericEnqueue(
			eLockType aLockType,
			int aObjectRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_lockType = aLockType;
				_objectRID = aObjectRID;
				_userRID = aUserRID;
				_clientThreadID = aClientThreadID;
				_MIDEnqueueData = new Data.MIDEnqueue();
				_conflictList = new System.Collections.ArrayList();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns a boolean indicating if the generic object is in conflict.
		/// </summary>

		public bool IsInConflict
		{
			get
			{
				try
				{
					return (_conflictList.Count != 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns an arraylist of GenericConflict objects.
		/// </summary>

		public System.Collections.ArrayList ConflictList
		{
			get
			{
				return _conflictList;
			}
		}

		//========
		// METHODS
		//========

        public bool DoesHaveConflicts()
        {
            System.Data.DataTable lockTable;

            try
            {
                _conflictList.Clear();

                lockTable = _MIDEnqueueData.Enqueue_Read(_lockType, _objectRID);

                foreach (System.Data.DataRow dataRow in lockTable.Rows)
                {
                    _conflictList.Add(
                        new GenericConflict(
                        System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                        System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
                }

                return IsInConflict;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


		/// <summary>
		/// This method attempts to enqueue the generic object.  If no existing enqueue exists for this object, the object is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a GenericConflictExcection is thrown.
		/// </summary>
		public void EnqueueGeneric()
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				lockTable = _MIDEnqueueData.Enqueue_Read(_lockType,_objectRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new GenericConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new GenericConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.Enqueue_Insert(_lockType, _objectRID, _userRID, _clientThreadID);

						_MIDEnqueueData.CommitData();
					}
					catch (Exception exc)
					{
						_MIDEnqueueData.Rollback();
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_MIDEnqueueData.CloseUpdateConnection();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method dequeues a method.
		/// </summary>

		public void DequeueGeneric()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.Enqueue_Delete(_lockType, _objectRID, _userRID);

					_MIDEnqueueData.CommitData();
				}
				catch (Exception exc)
				{
					_MIDEnqueueData.Rollback();
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_MIDEnqueueData.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
