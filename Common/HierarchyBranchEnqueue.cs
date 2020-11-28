using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class HierarchyBranchConflictException : Exception
	{
	}

	/// <summary>
	/// The HierarchyBranchConflict class stores information regarding a conflict during the enqueue of a hierarchy node.
	/// </summary>

	public struct HierarchyBranchConflict
	{
		//=======
		// FIELDS
		//=======

		private int _userRID;
		private string _userName;
		private int _nodeRID;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyBranchConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>
		/// <param name="aNodeRID">
		/// The record ID of the node in conflict.
		/// </param>

		public HierarchyBranchConflict(int aUserRID, string aUserName, int aNodeRID)
		{
			_userRID = aUserRID;
			_userName = aUserName;
			_nodeRID = aNodeRID;
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

		/// <summary>
		/// Gets the record ID of the node in the conflict.
		/// </summary>

		public int NodeRID
		{
			get
			{
				return _nodeRID;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The HierarchyBranchEnqueue class provides functionality for enqueuing a hierarchy node before using.
	/// </summary>
	/// <remarks>
	/// A hierarchy node is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class HierarchyBranchEnqueue
	{
		//=======
		// FIELDS
		//=======

		private int _hierarchyRID;
		private int _hierarchyNodeRID;
		private int _userRID;
		private int _clientThreadID;
        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
        //private NodeAncestorList _nal;
        //private NodeDescendantList _ndl;
        // End TT#2015

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyBranchEnqueue using the given hierarchy node information.
		/// </summary>
		/// <param name="aHierarchyRID">
		/// The record ID of the hierarchy.
		/// </param>
		/// <param name="aHierarchyNodeRID">
		/// The record ID of the hierarchy node.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>
		/// <param name="aNodeAncestorList">
		/// An instance of the NodeAncestorList class containing all ancestors of the node being locked.
		/// </param>
		/// <param name="aNodeDescendantList">
		/// An instance of the NodeDescendantList class containing all descendants of the node being locked.
		/// </param>

        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
        //public HierarchyBranchEnqueue(
        //    int aHierarchyRID,
        //    int aHierarchyNodeRID,
        //    int aUserRID,
        //    int aClientThreadID,
        //    NodeAncestorList aNodeAncestorList,
        //    NodeDescendantList aNodeDescendantList)
        //{
        //    try
        //    {
        //        _hierarchyRID = aHierarchyRID;
        //        _hierarchyNodeRID = aHierarchyNodeRID;
        //        _userRID = aUserRID;
        //        _clientThreadID = aClientThreadID;
        //        _nal = aNodeAncestorList;
        //        _ndl = aNodeDescendantList;
        //        _MIDEnqueueData = new Data.MIDEnqueue();
        //        _conflictList = new System.Collections.ArrayList();
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
        // End TT#2015

		/// <summary>
		/// Creates a new instance of HierarchyBranchEnqueue using the given hierarchy node information.
		/// </summary>
		/// <param name="aHierarchyRID">
		/// The record ID of the hierarchy.
		/// </param>
		/// <param name="aHierarchyNodeRID">
		/// The record ID of the hierarchy node.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>
		
		public HierarchyBranchEnqueue(
			int aHierarchyRID,
			int aHierarchyNodeRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_hierarchyRID = aHierarchyRID;
				_hierarchyNodeRID = aHierarchyNodeRID;
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
		/// Returns a boolean indicating if the hierarchy node is in conflict.
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
		/// Returns an arraylist of HierarchyBranchConflict objects.
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
		/// This method attempts to enqueue the hierarchy node.  If no existing enqueue exists for this hierarchy node, the hierarchy node is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a HierarchyBranchConflictExcection is thrown.
		/// </summary>

		public void EnqueueHierarchyBranch()
		{
			System.Data.DataTable lockTable;
            // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
            Data.MIDEnqueue dataLock = null;
            // End TT#2015

			try
			{
                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                dataLock = new MIDEnqueue();
                dataLock.OpenUpdateConnection(eLockType.HierarchyNode, Include.NodeEnqueueLockID);
                // End TT#2015

				_conflictList.Clear();

                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
				// Check for node lock of descendants
                //if (_ndl != null && _ndl.Count > 0)
                //{
                //    lockTable = _MIDEnqueueData.HierarchyNodes_Read(_hierarchyRID);

                //    foreach (System.Data.DataRow dataRow in lockTable.Rows)
                //    {
                //        // if a locked node is in the descendant list, add to conflict list
                //        if (_ndl.Contains(System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)))
                //        {
                //            _conflictList.Add(
                //                new HierarchyBranchConflict(
                //                System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                //                System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                //                System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                //        }
                //    }
                //}

                //// Check for lock of branches
                //if (_ndl != null && _ndl.Count > 0 ||
                //    _nal != null && _nal.Count > 0)
                //{
                //    lockTable = _MIDEnqueueData.HierarchyBranches_Read(_hierarchyRID);

                //    foreach (System.Data.DataRow dataRow in lockTable.Rows)
                //    {
                //        // if a locked node is in the ancestor list, add to conflict list
                //        if (_nal != null && _nal.Count > 0)
                //        {
                //            if (_nal.Contains(System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)))
                //            {
                //                _conflictList.Add(
                //                    new HierarchyBranchConflict(
                //                    System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                //                    System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                //                    System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                //            }
                //        }

                //        // if a locked node is in the descendant list, add to conflict list
                //        if (_ndl != null && _ndl.Count > 0)
                //        {
                //            if (_hierarchyNodeRID == System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture) ||
                //                _ndl.Contains(System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)))
                //            {
                //                _conflictList.Add(
                //                    new HierarchyBranchConflict(
                //                    System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                //                    System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                //                    System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                //            }
                //        }
                //    }
                //}
                lockTable = _MIDEnqueueData.AnyDescendantsLocked(_hierarchyNodeRID);
                foreach (System.Data.DataRow dataRow in lockTable.Rows)
                {
                    _conflictList.Add(
                        new HierarchyNodeConflict(
                        System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                        System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                        System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                }
                // End TT#2015
	

				if (_conflictList.Count > 0)
				{
					throw new HierarchyBranchConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.HierarchyBranch_Insert(_hierarchyRID, _hierarchyNodeRID, _userRID, _clientThreadID);

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
            // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
            finally
            {
                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
            }
            // End TT#2015
		}

		/// <summary>
		/// This method attempts to enqueue the hierarchy node and all descendants for delete.  In order to successfully 
		/// enqueue the branch for delete, no ancestor can have a branch lock and no descendant can have a lock of
		/// any kink.  If there is an existing enqueue, the ConflictList is updated and a 
		/// HierarchyBranchConflictExcection is thrown.
		/// </summary>

		public void EnqueueHierarchyBranchForDelete()
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
				// Check for branch lock with ancestors
                //if (_nal != null && _nal.Count > 0)
                //{
                //    lockTable = _MIDEnqueueData.HierarchyAncestorBranch_Read(_hierarchyRID, _nal);
			
                //    foreach (System.Data.DataRow dataRow in lockTable.Rows)
                //    {
                //        _conflictList.Add(
                //            new HierarchyBranchConflict(
                //            System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                //            System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                //            System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                //    }
                //}

                //// Check for any lock of descendants
                //if (_ndl != null && _ndl.Count > 0)
                //{
                //    lockTable = _MIDEnqueueData.AllEnqueuesWithNodes_Read();

                //    foreach (System.Data.DataRow dataRow in lockTable.Rows)
                //    {
                //        // if a locked node is in the descendant list, add to conflict list
                //        if (_hierarchyNodeRID == System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture) ||
                //            _ndl.Contains(System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)))
                //        {
                //            _conflictList.Add(
                //                new HierarchyBranchConflict(
                //                System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                //                System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                //                System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                //        }
                //    }
                //}
                // End TT#2015

				if (_conflictList.Count > 0)
				{
					throw new HierarchyBranchConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.HierarchyBranch_Insert(_hierarchyRID, _hierarchyNodeRID, _userRID, _clientThreadID);

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
		/// This method dequeues a hierarchy node.
		/// </summary>

		public void DequeueHierarchyBranch()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.HierarchyBranch_Delete(_hierarchyRID, _hierarchyNodeRID, _userRID);

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
