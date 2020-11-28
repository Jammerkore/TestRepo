using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanConflict class stores information regarding a conflict during the enqueue of a plan.
	/// </summary>

	public struct PlanConflict
	{
		//=======
		// FIELDS
		//=======

		private int _startWeek;
		private int _endWeek;
		private int _userRID;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanConflict using the given beginning and ending week RIDs.
		/// </summary>
		/// <param name="aStartWeek">
		/// The beginning week RID of the conflict.
		/// </param>
		/// <param name="aEndWeek">
		/// The ending week RID of the conflict.
		/// </param>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>

		public PlanConflict(int aStartWeek, int aEndWeek, int aUserRID)
		{
			_startWeek = aStartWeek;
			_endWeek = aEndWeek;
			_userRID = aUserRID;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the beginning week RID of the conflict.
		/// </summary>

		public int StartWeek
		{
			get
			{
				return _startWeek;
			}
		}

		/// <summary>
		/// Gets the ending week RID of the conflict.
		/// </summary>

		public int EndWeek
		{
			get
			{
				return _endWeek;
			}
		}

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

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The PlanEnqueue class provides functionality for enqueuing a plan before opening.
	/// </summary>
	/// <remarks>
	/// A plan is only allowed to be in use for update once for all users.  Since a plan encompasses a range of weeks, any overlap of those
	/// ranges is a conflict for the entire plan.  This class will inspect all existing enqueues and create a list of PlanConflict objects
	/// for each conflict that exists.
	/// </remarks>

	public class PlanEnqueue
	{
		//=======
		// FIELDS
		//=======

		private ePlanType _planType;
		private int _versionRID;
		private int _hierarchyNodeRID;
		private int _startWeek;
		private int _endWeek;
		private int _userRID;
		private int _clientThreadID;

		private bool _enqueued;
		private Data.MIDEnqueue _midEnqueue;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanEnqueue using the given plan information.
		/// </summary>
		/// <param name="aPlanType">
		/// An ePlanType value that indicates whether the plan is Chain or Store.
		/// </param>
		/// <param name="aVersionRID">
		/// The VersionRID of the plan.
		/// </param>
		/// <param name="aHierarchyNodeRID">
		/// The HierarchyNodeRID of the plan.
		/// </param>
		/// <param name="aStartWeek">
		/// The beginning week RID of the plan.
		/// </param>
		/// <param name="aEndWeek">
		/// The ending week RID of the plan.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public PlanEnqueue(
			ePlanType aPlanType,
			int aVersionRID,
			int aHierarchyNodeRID,
			int aStartWeek,
			int aEndWeek,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_planType = aPlanType;
				_versionRID = aVersionRID;
				_hierarchyNodeRID = aHierarchyNodeRID;
				_startWeek = aStartWeek;
				_endWeek = aEndWeek;
				_userRID = aUserRID;
				_clientThreadID = aClientThreadID;
				_enqueued = false;
				_midEnqueue = new MIDRetail.Data.MIDEnqueue();
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
		/// Returns a boolean indicating if the plan is in conflict.
		/// </summary>

		public bool isEnqueued
		{
			get
			{
				try
				{
					return _enqueued;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the plan is in conflict.
		/// </summary>

		public bool isPlanInConflict
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
		/// Returns an arraylist of PlanConflict objects.
		/// </summary>

		public System.Collections.ArrayList PlanConflictList
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
		/// This method attempts to enqueue the plan.  If no existing enqueue exists for this plan, the plan is enqueued.  If there is an
		/// existing enqueue, the PlanConflictList is updated and a PlanConflictExcection is thrown.
		/// </summary>

		public void EnqueuePlan()
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				switch (_planType)
				{
					case ePlanType.Chain:

						lockTable = _midEnqueue.ChainWeekEnqueue_Read(_hierarchyNodeRID, _versionRID, _startWeek, _endWeek);
						break;

					case ePlanType.Store:

						lockTable = _midEnqueue.StoreWeekEnqueue_Read(_hierarchyNodeRID, _versionRID, _startWeek, _endWeek);
						break;

					default:
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_InvalidChainStoreCode,
							MIDText.GetText(eMIDTextCode.msg_pl_InvalidChainStoreCode));
				}

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new PlanConflict(
						System.Math.Max(System.Convert.ToInt32(dataRow["START_WEEK"], CultureInfo.CurrentUICulture), _startWeek),
						System.Math.Min(System.Convert.ToInt32(dataRow["END_WEEK"], CultureInfo.CurrentUICulture), _endWeek),
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new PlanConflictException();
				}
				else
				{
					_midEnqueue.OpenUpdateConnection();

					try
					{
						switch (_planType)
						{
							case ePlanType.Chain:

								_midEnqueue.ChainWeekEnqueue_Insert(_hierarchyNodeRID, _versionRID, _startWeek, _endWeek, _userRID, _clientThreadID);
								break;

							case ePlanType.Store:

								_midEnqueue.StoreWeekEnqueue_Insert(_hierarchyNodeRID, _versionRID, _startWeek, _endWeek, _userRID, _clientThreadID);
								break;

							default:
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_pl_InvalidChainStoreCode,
									MIDText.GetText(eMIDTextCode.msg_pl_InvalidChainStoreCode));
						}

						_midEnqueue.CommitData();

						_enqueued = true;
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_midEnqueue.CloseUpdateConnection();
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
		/// This method dequeues a plan.
		/// </summary>

		public void DequeuePlan()
		{
			try
			{
				_midEnqueue.OpenUpdateConnection();

				try
				{
					switch (_planType)
					{
						case ePlanType.Chain:

							_midEnqueue.ChainWeekEnqueue_Delete(_hierarchyNodeRID, _versionRID, _startWeek, _endWeek, _userRID);
							break;

						case ePlanType.Store:

							_midEnqueue.StoreWeekEnqueue_Delete(_hierarchyNodeRID, _versionRID, _startWeek, _endWeek, _userRID);
							break;

						default:
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_InvalidChainStoreCode,
								MIDText.GetText(eMIDTextCode.msg_pl_InvalidChainStoreCode));
					}

					_midEnqueue.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_midEnqueue.CloseUpdateConnection();
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
