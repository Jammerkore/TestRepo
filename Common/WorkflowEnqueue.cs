using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class WorkflowConflictException : Exception
	{
	}

	/// <summary>
	/// The WorkflowConflict class stores information regarding a conflict during the enqueue of a workflow.
	/// </summary>

	public struct WorkflowConflict
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
		/// Creates a new instance of WorkflowConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public WorkflowConflict(int aUserRID, string aUserName)
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
	/// The WorkflowEnqueue class provides functionality for enqueuing a workflow before using.
	/// </summary>
	/// <remarks>
	/// A workflow is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class WorkflowEnqueue
	{
		//=======
		// FIELDS
		//=======

		private int _WorkflowRID;
		private int _userRID;
		private int _clientThreadID;

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of WorkflowEnqueue using the given workflow information.
		/// </summary>
		/// <param name="aWorkflowRID">
		/// The record ID of the workflow.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public WorkflowEnqueue(
			int aWorkflowRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_WorkflowRID = aWorkflowRID;
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
		/// Returns a boolean indicating if the workflow is in conflict.
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
		/// Returns an arraylist of WorkflowConflict objects.
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
		/// This method attempts to enqueue the workflow.  If no existing enqueue exists for this workflow, the workflow is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a WorkflowConflictExcection is thrown.
		/// </summary>

		public void EnqueueWorkflow()
        {
            EnqueueWorkflow(true);
        }

        /// <summary>
        /// This method attempts to enqueue the workflow.  If no existing enqueue exists for this workflow, the workflow is enqueued.  If there is an
        /// existing enqueue, the ConflictList is updated and a WorkflowConflictExcection is thrown.
        /// </summary>
        /// <param name="aLockResource">Identifies if the resource is to be locked. Use false to only look for locks.</param>

        //public void EnqueueWorkflow()
        public void EnqueueWorkflow(bool aLockResource)
        // End TT#2627 - JSmith - Conflict Error Message will not release
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				lockTable = _MIDEnqueueData.Enqueue_Read(eLockType.Workflow, _WorkflowRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new WorkflowConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new WorkflowConflictException();
				}
                // Begin TT#2627 - JSmith - Conflict Error Message will not release
                //else
                else if (aLockResource)
                // End TT#2627 - JSmith - Conflict Error Message will not release
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.Enqueue_Insert(eLockType.Workflow, _WorkflowRID, _userRID, _clientThreadID);

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
		/// This method dequeues a workflow.
		/// </summary>

		public void DequeueWorkflow()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.Enqueue_Delete(eLockType.Workflow, _WorkflowRID, _userRID);

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
