using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	public class SizeCurveGroupConflictException : Exception
	{
	}

	/// <summary>
	/// The SizeCurveGroupConflict class stores information regarding a conflict during the enqueue of a size curve group.
	/// </summary>

	public struct SizeCurveGroupConflict
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
		/// Creates a new instance of SizeCurveGroupConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public SizeCurveGroupConflict(int aUserRID, string aUserName)
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
	/// The SizeCurveGroupEnqueue class provides functionality for enqueuing a size curve group before using.
	/// </summary>
	/// <remarks>
	/// A Size Curve Group is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class SizeCurveGroupEnqueue
	{
		//=======
		// FIELDS
		//=======

		private int _sizeCurveGroupRID;
		private int _userRID;
		private int _clientThreadID;

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SizeCurveGroupEnqueue using the given size curve group information.
		/// </summary>
		/// <param name="aSizeCurveGroupRID">
		/// The record ID of the size curve group.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public SizeCurveGroupEnqueue(
			int aSizeCurveGroupRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_sizeCurveGroupRID = aSizeCurveGroupRID;
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
		/// Returns a boolean indicating if the size curve group is in conflict.
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
		/// Returns an arraylist of SizeCurveGroupConflict objects.
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
		/// This method attempts to enqueue the size curve group.  If no existing enqueue exists for this size curve group, the size curve group is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a SizeCurveGroupConflictExcection is thrown.
		/// </summary>

		public void EnqueueSizeCurveGroup()
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				lockTable = _MIDEnqueueData.Enqueue_Read(eLockType.SizeCurveGroup, _sizeCurveGroupRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new SizeCurveGroupConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new SizeCurveGroupConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.Enqueue_Insert(eLockType.SizeCurveGroup, _sizeCurveGroupRID, _userRID, _clientThreadID);

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
		/// This method dequeues a size curve group.
		/// </summary>

		public void DequeueSizeCurveGroup()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.Enqueue_Delete(eLockType.SizeCurveGroup, _sizeCurveGroupRID, _userRID);

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
