using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class MethodConflictException : Exception
	{
	}

	/// <summary>
	/// The MethodConflict class stores information regarding a conflict during the enqueue of a method.
	/// </summary>

	public struct MethodConflict
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
		/// Creates a new instance of MethodConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public MethodConflict(int aUserRID, string aUserName)
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
	/// The MethodEnqueue class provides functionality for enqueuing a method before using.
	/// </summary>
	/// <remarks>
	/// A method is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class MethodEnqueue
	{
		//=======
		// FIELDS
		//=======

		private int _MethodRID;
		private int _userRID;
		private int _clientThreadID;

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of MethodEnqueue using the given method information.
		/// </summary>
		/// <param name="aMethodRID">
		/// The record ID of the method.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public MethodEnqueue(
			int aMethodRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_MethodRID = aMethodRID;
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
		/// Returns a boolean indicating if the method is in conflict.
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
		/// Returns an arraylist of MethodConflict objects.
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

        // Begin TT#2627 - JSmith - Conflict Error Message will not release
        /// <summary>
		/// This method attempts to enqueue the method.  If no existing enqueue exists for this method, the method is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a MethodConflictExcection is thrown.
		/// </summary>

        public void EnqueueMethod()
        {
            EnqueueMethod(true);
        }

		/// <summary>
		/// This method attempts to enqueue the method.  If no existing enqueue exists for this method, the method is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a MethodConflictExcection is thrown.
		/// </summary>
        /// <param name="aLockResource">Identifies if the resource is to be locked. Use false to only look for locks.</param>

        //public void EnqueueMethod()
        public void EnqueueMethod(bool aLockResource)
        // End TT#2627 - JSmith - Conflict Error Message will not release
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				lockTable = _MIDEnqueueData.Enqueue_Read(eLockType.Method,_MethodRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new MethodConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new MethodConflictException();
				}
                // Begin TT#2627 - JSmith - Conflict Error Message will not release
                //else
                else if (aLockResource)
                // End TT#2627 - JSmith - Conflict Error Message will not release
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.Enqueue_Insert(eLockType.Method,_MethodRID, _userRID, _clientThreadID);

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

		public void DequeueMethod()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.Enqueue_Delete(eLockType.Method, _MethodRID, _userRID);

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
