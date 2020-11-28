using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class HierarchyConflictException : Exception
	{
	}

	/// <summary>
	/// The HierarchyConflict class stores information regarding a conflict during the enqueue of a hierarchy.
	/// </summary>

	public struct HierarchyConflict
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
		/// Creates a new instance of HierarchyConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public HierarchyConflict(int aUserRID, string aUserName)
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
	/// The HierarchyEnqueue class provides functionality for enqueuing a hierarchy before using.
	/// </summary>
	/// <remarks>
	/// A hierarchy node is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class HierarchyEnqueue
	{
		//=======
		// FIELDS
		//=======

		private int _HierarchyRID;
		private int _userRID;
		private int _clientThreadID;

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyEnqueue using the given hierarchy node information.
		/// </summary>
		/// <param name="aHierarchyRID">
		/// The record ID of the hierarchy node.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public HierarchyEnqueue(
			int aHierarchyRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_HierarchyRID = aHierarchyRID;
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
		/// Returns a boolean indicating if the hierarchy is in conflict.
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
		/// Returns an arraylist of HierarchyConflict objects.
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

		/// <summary>
		/// This method attempts to enqueue the hierarchy.  If no existing enqueue exists for this hierarchy, the hierarchy is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a HierarchyConflictExcection is thrown.
		/// </summary>

		public void EnqueueHierarchy()
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				lockTable = _MIDEnqueueData.Hierarchy_Read(_HierarchyRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new HierarchyConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new HierarchyConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.Hierarchy_Insert(_HierarchyRID, _userRID, _clientThreadID);

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
		/// This method dequeues a hierarchy.
		/// </summary>

		public void DequeueHierarchy()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.Hierarchy_Delete(_HierarchyRID, _userRID);

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
