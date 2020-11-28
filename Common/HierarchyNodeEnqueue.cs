using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class HierarchyNodeConflictException : Exception
	{
	}

	/// <summary>
	/// The HierarchyNodeConflict class stores information regarding a conflict during the enqueue of a hierarchy node.
	/// </summary>

	public struct HierarchyNodeConflict
	{
		//=======
		// FIELDS
		//=======

		private int _userRID;
		private string _userName;
        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
        private int _HNRID;
        // End TT#2015

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyNodeConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public HierarchyNodeConflict(int aUserRID, string aUserName)
		{
			_userRID = aUserRID;
			_userName = aUserName;
            // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
            _HNRID = Include.NoRID;
            // End TT#2015
		}

        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
        public HierarchyNodeConflict(int aUserRID, string aUserName, int aHNRID)
        {
            _userRID = aUserRID;
            _userName = aUserName;
            _HNRID = aHNRID;
        }
        // End TT#2015

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

        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
        /// <summary>
        /// Gets the user name of the conflict.
        /// </summary>

        public int HnRID
        {
            get
            {
                return _HNRID;
            }
        }
        // End TT#2015

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The HierarchyNodeEnqueue class provides functionality for enqueuing a hierarchy node before using.
	/// </summary>
	/// <remarks>
	/// A hierarchy node is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class HierarchyNodeEnqueue
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
        // End TT#2015

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of HierarchyNodeEnqueue using the given hierarchy node information.
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

		public HierarchyNodeEnqueue(
			int aHierarchyRID,
			int aHierarchyNodeRID,
			int aUserRID,
            // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
            //int aClientThreadID,
            //NodeAncestorList aNodeAncestorList)
            int aClientThreadID)
            // End TT#2015
		{
			try
			{
				_hierarchyRID = aHierarchyNodeRID;
				_hierarchyNodeRID = aHierarchyNodeRID;
				_userRID = aUserRID;
				_clientThreadID = aClientThreadID;
                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                //_nal = aNodeAncestorList;
                // End TT#2015
				_MIDEnqueueData = new Data.MIDEnqueue();
				_conflictList = new System.Collections.ArrayList();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of HierarchyNodeEnqueue using the given hierarchy node information.
		/// </summary>
		/// <param name="aHierarchyNodeRID">
		/// The record ID of the hierarchy node.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>
		
		public HierarchyNodeEnqueue(
			int aHierarchyNodeRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_hierarchyRID = Include.NoRID;
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
		/// Returns an arraylist of HierarchyNodeConflict objects.
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
		/// existing enqueue, the ConflictList is updated and a HierarchyNodeConflictExcection is thrown.
		/// </summary>

		public void EnqueueHierarchyNode()
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

                lockTable = _MIDEnqueueData.HierarchyNode_Read(_hierarchyNodeRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new HierarchyNodeConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

                // if no conflict, check for branch lock with ancestors
                if (_conflictList.Count == 0)
                {
                    // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                    //    if (_nal != null && _nal.Count > 0)
                    //    {
                    //        lockTable = _MIDEnqueueData.HierarchyAncestorBranch_Read(_hierarchyRID, _nal);

                    //        foreach (System.Data.DataRow dataRow in lockTable.Rows)
                    //        {
                    //            _conflictList.Add(
                    //                new HierarchyNodeConflict(
                    //                System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                    //                System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
                    //        }
                    //    }
                    lockTable = _MIDEnqueueData.AnyAncestorsLocked(_hierarchyNodeRID);
                    foreach (System.Data.DataRow dataRow in lockTable.Rows)
                    {
                        _conflictList.Add(
                            new HierarchyNodeConflict(
                            System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
                            System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture),
                            System.Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture)));
                    }
                    // End TT#2015
                }

				if (_conflictList.Count > 0)
				{
					throw new HierarchyNodeConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.HierarchyNode_Insert(_hierarchyNodeRID, _userRID, _clientThreadID);

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
		/// This method dequeues a hierarchy node.
		/// </summary>

		public void DequeueHierarchyNode()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					_MIDEnqueueData.HierarchyNode_Delete(_hierarchyNodeRID, _userRID);

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
