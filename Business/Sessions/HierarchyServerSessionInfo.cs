using System;
using System.Collections;
using System.Collections.Generic;	// TT#936 - MD - Prevent the saving of empty Group Allocations
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// Contains the information about a hierarchy. 
	/// </summary>
	/// <remarks>
	/// Uses Hashtables to contain information about the hierarchy levels and parent/child relationiships.
	/// </remarks>
	[Serializable()]
	public class HierarchyInfo : ICloneable
	{
		// Fields
		//Begin Track #6269 - JScott - Error logging on after auto upgrade
		private Audit _audit;
		//End Track #6269 - JScott - Error logging on after auto upgrade
		private int _hierarchyRID;
		private int _hierarchyRootNodeRID;
		private string _hierarchyID;
		private string _hierarchyColor;
		private eHierarchyType _hierarchyType;
		private eHierarchyRollupOption _hierarchyRollupOption;
		private eOTSPlanLevelType _planLevelType;
		private int _hierarchyDBLevelsCount = 0;
		private int _owner;
		private DateTime _postingDate;

		private Hashtable _hierarchyLevels = new Hashtable();
        private Dictionary<int, NodeRelationship> _parentChildRelationship = new Dictionary<int, NodeRelationship>();
		/// <summary>
		/// Contains the list of children for a parent in the hierarchy.
		/// </summary>
		public struct NodeRelationship
		{
			public int[] parents;
			public bool childrenBuilt;  //  this is used for dynamic build of color and size information
			public int[] children;

			public int ParentsCount()
			{
				try
				{
					if (parents == null)
					{
						return 0;
					}
					else
					{
						return parents.Length;
					}
				}
				catch
				{
					throw;
				}
			}

			public bool ParentsContains(int aParentRID)
			{
				try
				{
					if (parents != null)
					{
						foreach (int parentRID in parents)
						{
							if (parentRID == aParentRID)
							{
								return true;
							}
						}
					}
					return false;
				}
				catch
				{
					throw;
				}
			}

			public void AddParent(int aParentRID)
			{
				try
				{
					int[] objectVector;
					if (parents == null)
					{
						parents = new int[1];
					}
					else
					{
						objectVector = new int[parents.Length + 1];
						System.Array.Copy(parents, objectVector, parents.Length);
						parents = objectVector;
					}
					parents[parents.Length-1] = aParentRID;
				}
				catch
				{
					throw;
				}
			}

			public void ParentsRemove(int aParentRID)
			{
				try
				{
					ArrayList objectVector = new ArrayList();
					objectVector.AddRange(parents);
					objectVector.Remove(aParentRID);
					if (objectVector.Count == 0)
					{
						parents = null;
					}
					else
					{
						parents = new int[objectVector.Count];
						objectVector.CopyTo(0,parents,0,objectVector.Count);
					}
				}
				catch
				{
					throw;
				}
			}

			public int ChildrenCount()
			{
				try
				{
					if (children == null)
					{
						return 0;
					}
					else
					{
						return children.Length;
					}
				}
				catch
				{
					throw;
				}
			}

			public bool ChildrenContains(int aChildRID)
			{
				try
				{
					if (children != null)
					{
						foreach (int childRID in children)
						{
							if (childRID == aChildRID)
							{
								return true;
							}
						}
					}
					return false;
				}
				catch
				{
					throw;
				}
			}

			public void AddChild(int aChildRID)
			{
				try
				{
					int[] objectVector;
					if (children == null)
					{
						children = new int[1];
					}
					else
					{
						objectVector = new int[children.Length + 1];
						System.Array.Copy(children, objectVector, children.Length);
						children = objectVector;
					}
					children[children.Length-1] = aChildRID;
				}
				catch
				{
					throw;
				}
			}

			public void ChildrenRemove(int aChildRID)
			{
				try
				{
                    // Begin TT#384 - JSmith - Error removing child from hierarchy
                    // protect if child already removed
                    if (children == null)
                    {
                        return;
                    }
                    // Begin TT#384
					ArrayList objectVector = new ArrayList();
					objectVector.AddRange(children);
					objectVector.Remove(aChildRID);
					if (objectVector.Count == 0)
					{
						children = null;
					}
					else
					{
						children = new int[objectVector.Count];
						objectVector.CopyTo(0,children,0,objectVector.Count);
					}
				}
				catch
				{
					throw;
				}
			}

		}

		// Properties

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		//Begin Track #6269 - JScott - Error logging on after auto upgrade
		//public HierarchyInfo()
		//{
		//}
		public HierarchyInfo(Audit aAudit)
		{
			_audit = aAudit;
		}
		//End Track #6269 - JScott - Error logging on after auto upgrade

		/// <summary>
		/// Gets or sets the hierarchy's record id.
		/// </summary>
		public int HierarchyRID 
		{
			get { return _hierarchyRID ; }
			set { _hierarchyRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id for the root node in the hierarchy.
		/// </summary>
		public int HierarchyRootNodeRID 
		{
			get { return _hierarchyRootNodeRID ; }
			set { _hierarchyRootNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the id of the hierarchy.
		/// </summary>
		public string HierarchyID 
		{
			get { return _hierarchyID ; }
			set { _hierarchyID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the color of the folder associated with the hierarchy.
		/// </summary>
		public string HierarchyColor 
		{
			get { return _hierarchyColor ; }
			set { _hierarchyColor = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the type of the hierarchy.
		/// </summary>
		public eHierarchyType HierarchyType 
		{
			get { return _hierarchyType ; }
			set { _hierarchyType = value; }
		}
		/// <summary>
		/// Gets or sets the rollup option of the hierarchy.
		/// </summary>
		public eHierarchyRollupOption HierarchyRollupOption 
		{
			get { return _hierarchyRollupOption ; }
			set { _hierarchyRollupOption = value; }
		}
		/// <summary>
		/// Gets or sets the type of the hierarchy.
		/// </summary>
		public eOTSPlanLevelType OTSPlanLevelType 
		{
			get { return _planLevelType ; }
			set { _planLevelType = (value != 0) ? value : eOTSPlanLevelType.Total; }
		}
		/// <summary>
		/// Gets or sets the levels of the hierarchy.
		/// </summary>		
		public Hashtable HierarchyLevels
		{
			get { return _hierarchyLevels ; }
			set { _hierarchyLevels = value; }
		}
		/// <summary>
		/// Gets or sets the parent/child relationships of the hierarchy.
		/// </summary>	
        public Dictionary<int, NodeRelationship> ParentChildRelationship
		{
			get { return _parentChildRelationship ; }
			set { _parentChildRelationship = value; }
		}
		/// <summary>
		/// Gets or sets the number of levels of the hierarchy found on the database.
		/// </summary>
		/// <remarks>
		/// This field is used to know how many levels are on the database when it is necessary
		/// to delete the levels from the database.
		/// </remarks>
		public int HierarchyDBLevelsCount
		{
			get { return _hierarchyDBLevelsCount ; }
			set { _hierarchyDBLevelsCount = value; }
		}

		/// <summary>
		/// Gets or sets the owner of the hierarchy.
		/// </summary>
		/// <remarks>
		/// This is used to control the "My Hierarchy" for each user.
		/// </remarks>
		public int Owner
		{
			get { return _owner ; }
			set { _owner = value; }
		}

		/// <summary>
		/// Gets or sets the posting date of the hierarchy.
		/// </summary>
		public DateTime PostingDate
		{
			get { return _postingDate ; }
			set { _postingDate = value; }
		}

        /// <summary>
        /// Gets flag identifying if hierarchy is a user hierarchy.
        /// </summary>
        public bool IsPersonalHierarchy
        {
            get { return _owner != Include.GlobalUserRID; }
        }

        // Begin TT#374 - JSmith - Adding new guests to alternate hierarchy fails with Orphaned hierarchy error
		/// <summary>
		/// Used to determine if a NodeRelationship between the children and a parent exists.
		/// </summary>
		/// <param name="nodeRID">The record id of the parent</param>
		/// <returns></returns>
		public bool ChildRelationshipExists(int nodeRID)
		{
			if (ParentChildRelationship.ContainsKey(nodeRID))
			{
				return true; 
			}
			else
			{
                return false;
			}
		}
        // End TT#374

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Used to determine if a NodeRelationship between the children and a parent exists.
		/// </summary>
		/// <param name="nodeRID">The record id of the parent</param>
		/// <returns></returns>
		public bool ChildAnyRelationshipExists(int nodeRID, NodeAncestorList aHomeAncestorList)
		{
			foreach (NodeAncestorProfile nap in aHomeAncestorList)
			{
				if (ParentChildRelationship.ContainsKey(nap.Key))
				{
					return true;
				}
			}

			return false;
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
        /// Used to get a NodeRelationship which contains an ArrayList of the children for a parent.
        /// </summary>
        /// <param name="nodeRID">The record id of the parent</param>
        /// <returns></returns>
        public NodeRelationship GetChildRelationship(int nodeRID)
        {
            NodeRelationship nodeRelationship;

            if (ParentChildRelationship.TryGetValue(nodeRID, out nodeRelationship))
            {
                return nodeRelationship;
            }
            else
            {
                //Begin Track #6269 - JScott - Error logging on after auto upgrade
                //throw new RelationshipNotFoundException();
                if (nodeRID != Include.NoRID)
                {
                    //_audit.Add_Msg(
                    //    eMIDMessageLevel.Warning,
                    //    eMIDTextCode.msg_Orphaned_Node,
                    //    string.Format(_audit.GetText(eMIDTextCode.msg_Orphaned_Node, false), nodeRID),
                    //    System.Reflection.MethodBase.GetCurrentMethod().Name);
					// Begin TT#189 - JScott - Remove excessive entries from the Audit
					//_audit.Add_Msg(eMIDMessageLevel.Warning, "Orphaned Hierarchy Node found: HN_RID = " + nodeRID, System.Reflection.MethodBase.GetCurrentMethod().Name);
					if (_audit != null)
					{
						_audit.Add_Msg(eMIDMessageLevel.Warning, "Orphaned Hierarchy Node found: HN_RID = " + nodeRID, System.Reflection.MethodBase.GetCurrentMethod().Name);
					}
					// End TT#189 - JScott - Remove excessive entries from the Audit
				}

                return new NodeRelationship();
                //End Track #6269 - JScott - Error logging on after auto upgrade
            }
        }

		/// <summary>
		/// Used to construct the parent/child relationships of the hierarchy.
		/// </summary>
		/// <param name="mhd">The Data layer object used to retrieve hierarchy information</param>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		public void BuildParentChild(MerchandiseHierarchyData mhd, int hierarchyRID)
		{
			try
			{
				NodeRelationship nodeRelationship = new NodeRelationship();
				nodeRelationship.childrenBuilt = false;   
				NodeRelationship child = new NodeRelationship();
				child.childrenBuilt = false;   
				int currentParent = Include.NoRID;
				int currentChild = Include.NoRID;
				//			object[] hierarchyList;
				try
				{
					DataTable dt = mhd.Hierarchy_Join_Read(hierarchyRID);
					// get the first parent RID
					if (dt.Rows.Count > 0)
					{
						DataRow x = dt.Rows[0];
						currentParent = Convert.ToInt32(x["PARENT_HN_RID"], CultureInfo.CurrentUICulture);
						if (currentParent == 0) // root node
						{
							_hierarchyRootNodeRID = Convert.ToInt32(x["HN_RID"], CultureInfo.CurrentUICulture);

						}
						else
						{
							throw(new Exception("No root node for hierarchy"));
						}
					}

					foreach(DataRow dr in dt.Rows)
					{
						//  update parent record with children
						int wrkCurrentParent = 0;
						wrkCurrentParent = Convert.ToInt32(dr["PARENT_HN_RID"], CultureInfo.CurrentUICulture);

						if (wrkCurrentParent != currentParent)
						{
							if (_parentChildRelationship.ContainsKey(currentParent))
							{
								_parentChildRelationship[currentParent] = nodeRelationship;
							}
							else
							{
								_parentChildRelationship.Add(currentParent, nodeRelationship);
							}
							currentParent = wrkCurrentParent;
							if (_parentChildRelationship.ContainsKey(currentParent))
							{
								nodeRelationship = (NodeRelationship)_parentChildRelationship[currentParent];
							}
							else
							{
								nodeRelationship = new NodeRelationship();
								nodeRelationship.childrenBuilt = false;
							}
						}
						currentChild = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						nodeRelationship.AddChild(currentChild);

						//  update child record with parent
                        if (_parentChildRelationship.TryGetValue(currentChild, out child))
						{
							if (!child.ParentsContains(currentParent))
							{
								child.AddParent(currentParent);
							}
							_parentChildRelationship[currentChild] = child;
						}
						else
						{
							child = new NodeRelationship();
							child.childrenBuilt = false;
							if (!child.ParentsContains(currentParent))
							{
								child.AddParent(currentParent);
							}
							_parentChildRelationship.Add(currentChild, child);
						}
					}

					if (_parentChildRelationship.ContainsKey(currentParent))
					{
						_parentChildRelationship[currentParent] = nodeRelationship;
					}
					else
					{
						_parentChildRelationship.Add(currentParent, nodeRelationship);
					}
				}
				catch ( Exception ex )
				{
					string message = ex.ToString();
					throw;
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}

		}

		public object NewObject()
		{
			try
			{
				//Begin Track #6269 - JScott - Error logging on after auto upgrade
				//HierarchyInfo hi = new HierarchyInfo();
				HierarchyInfo hi = new HierarchyInfo(_audit);
				//End Track #6269 - JScott - Error logging on after auto upgrade
				hi._hierarchyColor = this._hierarchyColor;
				hi._hierarchyDBLevelsCount = this._hierarchyDBLevelsCount;
				hi._hierarchyID = this._hierarchyID;
				hi.HierarchyLevels = new Hashtable();
				foreach (DictionaryEntry hierarchyLevel in this.HierarchyLevels) 
				{
					HierarchyLevelInfo hli = (HierarchyLevelInfo)hierarchyLevel.Value;
					hi.HierarchyLevels.Add(hierarchyLevel.Key, hli.Clone());
				}
				hi._hierarchyRID = this._hierarchyRID;
				hi._hierarchyRootNodeRID = this._hierarchyRootNodeRID;
				hi._hierarchyType = this._hierarchyType;
				hi._hierarchyRollupOption = this._hierarchyRollupOption;
				hi._planLevelType = this._planLevelType;
				hi._owner = this._owner;
				hi._postingDate = this._postingDate;
				return hi;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		#region ICloneable Members

		public object Clone()
		{
			try
			{
				HierarchyInfo hi = (HierarchyInfo)this.MemberwiseClone();
				hi.HierarchyLevels = new Hashtable();
				foreach (DictionaryEntry hierarchyLevel in this.HierarchyLevels) 
				{
					HierarchyLevelInfo hli = (HierarchyLevelInfo)hierarchyLevel.Value;
					hi.HierarchyLevels.Add(hierarchyLevel.Key, hli.Clone());
				}
				return hi;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		#endregion
	}

	/// <summary>
	/// Contains the information about the levels of a hierarchy
	/// </summary>
	[Serializable()]
	public class HierarchyLevelInfo : ICloneable
	{
		// Fields
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		//private int							_levelNodeCount;
		private bool							_levelNodesExist;
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		private int							_level;
		private string						_levelID;
		private string						_levelColor;
		private eLevelLengthType			_levelLengthType;
		private int							_levelRequiredSize;
		private int							_levelSizeRangeFrom;
		private int							_levelSizeRangeTo;
		private eHierarchyLevelType			_levelType;
		private eOTSPlanLevelType			_levelOTSPlanLevelType;
		private eHierarchyDisplayOptions	_levelDisplayOption;
		private eHierarchyIDFormat			_levelIDFormat;
		private ePurgeTimeframe				_purgeDailyHistoryTimeframe;
		private int							_purgeDailyHistory;
		private ePurgeTimeframe				_purgeWeeklyHistoryTimeframe;
		private int							_purgeWeeklyHistory;
		private ePurgeTimeframe				_purgePlansTimeframe;
		private int							_purgePlans;
        //private ePurgeTimeframe				_purgeHeadersTimeframe;
        //private int							_purgeHeaders;
        // add all purge types with properties
        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        private ePurgeTimeframe _purgeHtASNTimeframe;
        private ePurgeTimeframe _purgeHtDropShipTimeframe;
        private ePurgeTimeframe _purgeHtDummyTimeframe;
        private ePurgeTimeframe _purgeHtPurchaseOrderTimeframe;
        private ePurgeTimeframe _purgeHtReceiptTimeframe;
        private ePurgeTimeframe _purgeHtReserveTimeframe;
        private ePurgeTimeframe _purgeHtVSWTimeframe;
        private ePurgeTimeframe _purgeHtWorkUpTotTimeframe;
        private int _purgeHtASN;
        private int _purgeHtDropShip;
        private int _purgeHtDummy;
        private int _purgeHtPurchaseOrder;
        private int _purgeHtReceipt;
        private int _purgeHtReserve;
        private int _purgeHtVSW;
        private int _purgeHtWorkUpTot;
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

		// Properties

		/// <summary>
		/// Gets or sets if the hierarchy has nodes defined to this level.
		/// </summary>
		/// <remarks>
		/// This property is used to determine if this level can be expanded on the hierarchy explorer.
		/// </remarks>
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		//public int LevelNodeCount 
		//{
		//    get { return _levelNodeCount ; }
		//    set { _levelNodeCount = value; }
		//}
		public bool LevelNodesExist
		{
			get { return _levelNodesExist; }
			set { _levelNodesExist = value; }
		}
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		/// <summary>
		/// Gets or sets the relative position of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// Levels begin with 1.
		/// </remarks>
		public int Level 
		{
			get { return _level ; }
			set { _level = value; }
		}
		/// <summary>
		/// Gets or sets the ID (name) of the level in the hierarchy.
		/// </summary>
		public string LevelID 
		{
			get { return _levelID ; }
			set { _levelID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the color to use for the folder of the level in the hierarchy.
		/// </summary>
		public string LevelColor 
		{
			get { return _levelColor ; }
			set { _levelColor = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the type of length (unrestricted, required, or range) for the level in the hierarchy.
		/// </summary>
		public eLevelLengthType LevelLengthType 
		{
			get { return _levelLengthType ; }
			set { _levelLengthType = value; }
		}
		/// <summary>
		/// Gets or sets the required size of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelLengthType is set to required.
		/// </remarks>
		public int LevelRequiredSize 
		{
			get { return _levelRequiredSize ; }
			set { _levelRequiredSize = value; }
		}
		/// <summary>
		/// Gets or sets the from size of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelLengthType is set to range.
		/// </remarks>
		public int LevelSizeRangeFrom 
		{
			get { return _levelSizeRangeFrom ; }
			set { _levelSizeRangeFrom = value; }
		}
		/// <summary>
		/// Gets or sets the to size of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelLengthType is set to range.
		/// </remarks>
		public int LevelSizeRangeTo 
		{
			get { return _levelSizeRangeTo ; }
			set { _levelSizeRangeTo = value; }
		}
		/// <summary>
		/// Gets or sets the OTS level type of this level in the hierarchy.
		/// </summary>
		public eHierarchyLevelType LevelType 
		{
			get { return _levelType ; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets the OTS plan level type of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelType is "Plan Level".</remarks>
		public eOTSPlanLevelType LevelOTSPlanLevelType
		{
			get { return _levelOTSPlanLevelType ; }
			set { _levelOTSPlanLevelType = value; }
		}
		/// <summary>
		/// Gets or sets the display option of this level in the hierarchy.
		/// </summary>
		public eHierarchyDisplayOptions LevelDisplayOption
		{
			get { return _levelDisplayOption ; }
			set { _levelDisplayOption = value; }
		}
		/// <summary>
		/// Gets or sets the ID format of this level in the hierarchy.
		/// </summary>
		public eHierarchyIDFormat LevelIDFormat
		{
			get { return _levelIDFormat ; }
			set { _levelIDFormat = value; }
		}
		/// <summary>
		/// Gets or sets the timeframe of the history purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgeDailyHistoryTimeframe
		{
			get { return _purgeDailyHistoryTimeframe ; }
			set { _purgeDailyHistoryTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the history purge information of this level in the hierarchy.
		/// </summary>
		public int PurgeDailyHistory
		{
			get { return _purgeDailyHistory ; }
			set { _purgeDailyHistory = value; }
		}
		/// <summary>
		/// Gets or sets the timeframe of the forecast purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgeWeeklyHistoryTimeframe
		{
			get { return _purgeWeeklyHistoryTimeframe ; }
			set { _purgeWeeklyHistoryTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the forecast purge information of this level in the hierarchy.
		/// </summary>
		public int PurgeWeeklyHistory
		{
			get { return _purgeWeeklyHistory ; }
			set { _purgeWeeklyHistory = value; }
		}
		/// <summary>
		/// Gets or sets the timeframe of the distro purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgePlansTimeframe
		{
			get { return _purgePlansTimeframe ; }
			set { _purgePlansTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the distro purge information of this level in the hierarchy.
		/// </summary>
		public int PurgePlans
		{
			get { return _purgePlans ; }
			set { _purgePlans = value; }
		}
        ///// <summary>
        ///// Gets or sets the timeframe of the header purge information of this level in the hierarchy.
        ///// </summary>
        //public ePurgeTimeframe PurgeHeadersTimeframe
        //{
        //    get { return _purgeHeadersTimeframe ; }
        //    set { _purgeHeadersTimeframe = value; }
        //}
        ///// <summary>
        ///// Gets or sets the time of the header purge information of this level in the hierarchy.
        ///// </summary>
        //public int PurgeHeaders
        //{
        //    get { return _purgeHeaders ; }
        //    set { _purgeHeaders = value; }
        //}

        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        /// <summary>
        /// Gets or sets the timeframe of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtASNTimeframe
        {
            get { return _purgeHtASNTimeframe; }
            set { _purgeHtASNTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtASN
        {
            get { return _purgeHtASN; }
            set { _purgeHtASN = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtDropShipTimeframe
        {
            get { return _purgeHtDropShipTimeframe; }
            set { _purgeHtDropShipTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtDropShip
        {
            get { return _purgeHtDropShip; }
            set { _purgeHtDropShip = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtDummyTimeframe
        {
            get { return _purgeHtDummyTimeframe; }
            set { _purgeHtDummyTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtDummy
        {
            get { return _purgeHtDummy; }
            set { _purgeHtDummy = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the PurchaseOrder header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtPurchaseOrderTimeframe
        {
            get { return _purgeHtPurchaseOrderTimeframe; }
            set { _purgeHtPurchaseOrderTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the PurchaseOrder header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtPurchaseOrder
        {
            get { return _purgeHtPurchaseOrder; }
            set { _purgeHtPurchaseOrder = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtReceiptTimeframe
        {
            get { return _purgeHtReceiptTimeframe; }
            set { _purgeHtReceiptTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtReceipt
        {
            get { return _purgeHtReceipt; }
            set { _purgeHtReceipt = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Reserve header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtReserveTimeframe
        {
            get { return _purgeHtReserveTimeframe; }
            set { _purgeHtReserveTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Reserve header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtReserve
        {
            get { return _purgeHtReserve; }
            set { _purgeHtReserve = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the VSW header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtVSWTimeframe
        {
            get { return _purgeHtVSWTimeframe; }
            set { _purgeHtVSWTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the VSW header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtVSW
        {
            get { return _purgeHtVSW; }
            set { _purgeHtVSW = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the WorkUpTot header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtWorkUpTotTimeframe
        {
            get { return _purgeHtWorkUpTotTimeframe; }
            set { _purgeHtWorkUpTotTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the WorkUpTot header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtWorkUpTot
        {
            get { return _purgeHtWorkUpTot; }
            set { _purgeHtWorkUpTot = value; }
        }
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyLevelInfo()
		{
			_levelColor = Include.MIDDefaultColor;
			_levelLengthType = 0;
			_levelRequiredSize = 0;
			_levelSizeRangeFrom = 0;
			_levelSizeRangeTo = 0;
			//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
			//_levelNodeCount = 0;
			_levelNodesExist = false;
			//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
			_levelType = eHierarchyLevelType.Undefined;
			_levelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
			_levelDisplayOption = eHierarchyDisplayOptions.NameOnly;
			_levelIDFormat = eHierarchyIDFormat.Unique;
			_purgeDailyHistoryTimeframe = ePurgeTimeframe.None;
			_purgeDailyHistory = Include.Undefined;
			_purgeWeeklyHistoryTimeframe = ePurgeTimeframe.None;
			_purgeWeeklyHistory = Include.Undefined;
			_purgePlansTimeframe = ePurgeTimeframe.None;
			_purgePlans = Include.Undefined;
            //_purgeHeadersTimeframe = ePurgeTimeframe.None;
            //_purgeHeaders = Include.Undefined;
            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            _purgeHtASN = Include.Undefined;
            _purgeHtASNTimeframe = ePurgeTimeframe.None;
            _purgeHtDropShip = Include.Undefined;
            _purgeHtDropShipTimeframe = ePurgeTimeframe.None;
            _purgeHtDummy = Include.Undefined;
            _purgeHtDummyTimeframe = ePurgeTimeframe.None;
            _purgeHtPurchaseOrder = Include.Undefined;
            _purgeHtPurchaseOrderTimeframe = ePurgeTimeframe.None;
            _purgeHtReceipt = Include.Undefined;
            _purgeHtReceiptTimeframe = ePurgeTimeframe.None;
            _purgeHtReserve = Include.Undefined;
            _purgeHtReserveTimeframe = ePurgeTimeframe.None;
            _purgeHtVSW = Include.Undefined;
            _purgeHtVSWTimeframe = ePurgeTimeframe.None;
            _purgeHtWorkUpTot = Include.Undefined;
            _purgeHtWorkUpTotTimeframe = ePurgeTimeframe.None;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        }

		#region ICloneable Members

		public object Clone()
		{
			try
			{
				HierarchyLevelInfo hli = (HierarchyLevelInfo)this.MemberwiseClone();
				return hli;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		#endregion
	}

	/// <summary>
	/// Contains the information about a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeInfo : ICloneable
	{
		// Fields

		private int					_nodeRID;
		private eHierarchyLevelType	_levelType;
		private string				_nodeID;
		private string				_nodeName;
		private string				_nodeDescription;
		private int					_homeHierarchyRID;
		private int					_homeHierarchyLevel;
		private bool				_productTypeIsOverridden;
		private eProductType		_productType;
		private bool				_OTSPlanLevelIsOverridden;
		private ePlanLevelSelectType _OTSPlanLevelSelectType;
		private ePlanLevelLevelType	_OTSPlanLevelLevelType;
		private int					_OTSPlanLevelHierarchyRID;
		private int					_OTSPlanLevelHierarchyLevelSequence;
		private int					_OTSPlanLevelAnchorNode;
		private eMaskField			_OTSPlanLevelMaskField;
		private string				_OTSPlanLevelMask;
		private bool				_OTSPlanLevelTypeIsOverridden;
		private eOTSPlanLevelType	_OTSPlanLevelType;
		private bool				_useBasicReplenishment;
		private int					_colorOrSizeCodeRID;
		private int					_purgeDailyHistoryAfter;
		private int					_purgeWeeklyHistoryAfter;
		private int					_purgeOTSPlansAfter;
        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        //private int					_purgeHeadersAfter;
        private int                 _purgeHtASNAfter;
        private int                 _purgeHtDropShipAfter;
        private int                 _purgeHtDummyAfter;
        private int                 _purgeHtReceiptAfter;
        private int                 _purgeHtPurchaseOrderAfter;
        private int                 _purgeHtReserveAfter;
        private int                 _purgeHtVSWAfter;
        private int                 _purgeHtWorkUpTotAfter;
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        private int                 _stockMinMaxSgRID;
		private bool				_virtualInd;
        private ePurpose            _purpose;
		private bool				_productCharsLoadedInd;
		private object[]			_productCharValues;
        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        private bool                _activeInd;
        // End TT#988
        // BEGIN TT#1399 - GRT - Alternate Hierarchy Inherit Node Properties
        private int                 _applyHNRIDFrom;
        // END TT#1399            
        private bool _deleteNode;	// TT#3630 - JSmith - Delete My Hierarchy

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeInfo()
		{
			_homeHierarchyRID = Include.NoRID; 
			_colorOrSizeCodeRID = Include.NoRID;
			_purgeDailyHistoryAfter = Include.Undefined;
			_purgeWeeklyHistoryAfter = Include.Undefined;
			_purgeOTSPlansAfter = Include.Undefined;
            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            //_purgeHeadersAfter = Include.Undefined;
            _purgeHtASNAfter = Include.Undefined;
            _purgeHtDropShipAfter = Include.Undefined;
            _purgeHtDummyAfter = Include.Undefined;
            _purgeHtReceiptAfter = Include.Undefined;
            _purgeHtPurchaseOrderAfter = Include.Undefined;
            _purgeHtReserveAfter = Include.Undefined;
            _purgeHtVSWAfter = Include.Undefined;
            _purgeHtWorkUpTotAfter = Include.Undefined;
            //END TT#400-MD-VSt			uart-Add Header Purge Criteria by Header Type
			_OTSPlanLevelMaskField = eMaskField.Undefined;
			_OTSPlanLevelSelectType = ePlanLevelSelectType.Undefined;
			_OTSPlanLevelLevelType  = ePlanLevelLevelType.Undefined;
			_stockMinMaxSgRID = Include.NoRID;
			_virtualInd = false;
			_productCharsLoadedInd = false;
            _purpose = ePurpose.Default;
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            _activeInd = true;
            // End TT#988
			// BEGIN TT#1399 - GRT - Alternate Hierarchy Inherit Node Properties
            //_nodeInheritedFrom = Include.NoRID;
            _applyHNRIDFrom = Include.NoRID;
            // END TT#1399 
            _deleteNode = false;  // TT#3630 - JSmith - Delete My Hierarchy
		}

		// Properties

        // Begin TT#1399
        public int ApplyHNRIDFrom
        {
            get { return _applyHNRIDFrom; }
            set { _applyHNRIDFrom = value; }
        }
        // End TT#1399

		/// <summary>
		/// Gets or sets the record id of a node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the type of the node.
		/// </summary>
		public eHierarchyLevelType LevelType 
		{
			get { return _levelType ; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets the id of a node.
		/// </summary>
		public string NodeID 
		{
			get { return _nodeID ; }
			set { _nodeID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the name of a node.
		/// </summary>
		public string NodeName
		{
			get { return _nodeName ; }
			set { _nodeName = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the description of a node.
		/// </summary>
		public string NodeDescription 
		{
			get { return _nodeDescription ; }
			set { _nodeDescription = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the record id of the home hierarchy of a node.
		/// </summary>
		/// <remarks>
		/// The home hierarchy of a node is the location where the node was first created.  Or, the hierarchy
		/// where a node was moved during a reorganization.</remarks>
		public int HomeHierarchyRID 
		{
			get { return _homeHierarchyRID ; }
			set { _homeHierarchyRID = value; }
		}
		/// <summary>
		/// Gets or sets the level of the node in the home hierarchy.
		/// </summary>
		/// <remarks>
		/// This is used to determine the color to use for this node in an alternate hierarchy.
		/// </remarks>
		public int HomeHierarchyLevel 
		{
			get { return _homeHierarchyLevel ; }
			set { _homeHierarchyLevel = value; }
		}

		/// <summary>
		/// Gets or sets whether to OTS Plan Level is overridden.
		/// </summary>
		public bool ProductTypeIsOverridden
		{
			get { return _productTypeIsOverridden ; }
			set { _productTypeIsOverridden = value; }
		}

		/// <summary>
		/// Gets or sets the product type of the node.
		/// </summary>
		public eProductType ProductType 
		{
			get { return _productType ; }
			set { _productType = value; }
		}

		/// <summary>
		/// Gets or sets whether to OTS Plan Level is overridden.
		/// </summary>
		public bool OTSPlanLevelIsOverridden
		{
			get { return _OTSPlanLevelIsOverridden ; }
			set { _OTSPlanLevelIsOverridden = value; }
		}

		/// <summary>
		/// Gets or sets OTS Plan Level level select type.
		/// </summary>
		public ePlanLevelSelectType OTSPlanLevelSelectType
		{
			get { return _OTSPlanLevelSelectType ; }
			set { _OTSPlanLevelSelectType = value; }
		}

		/// <summary>
		/// Gets or sets OTS Plan Level level type.
		/// </summary>
		public ePlanLevelLevelType OTSPlanLevelLevelType
		{
			get { return _OTSPlanLevelLevelType ; }
			set { _OTSPlanLevelLevelType = value; }
		}

		/// <summary>
		/// Gets or sets the type of override for the OTS Plan Level.
		/// </summary>
		public int OTSPlanLevelHierarchyRID
		{
			get { return _OTSPlanLevelHierarchyRID ; }
			set { _OTSPlanLevelHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the type of override for the OTS Plan Level.
		/// </summary>
		public int OTSPlanLevelHierarchyLevelSequence
		{
			get { return _OTSPlanLevelHierarchyLevelSequence ; }
			set { _OTSPlanLevelHierarchyLevelSequence = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level anchor node.
		/// </summary>
		public int OTSPlanLevelAnchorNode
		{
			get { return _OTSPlanLevelAnchorNode ; }
			set { _OTSPlanLevelAnchorNode = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level mask field.
		/// </summary>
		public eMaskField OTSPlanLevelMaskField
		{
			get { return _OTSPlanLevelMaskField ; }
			set { _OTSPlanLevelMaskField = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level mask.
		/// </summary>
		public string OTSPlanLevelMask
		{
			get { return _OTSPlanLevelMask ; }
			set { _OTSPlanLevelMask = value; }
		}

		/// <summary>
		/// Gets or sets whether to OTS Plan Level Type is overridden.
		/// </summary>
		public bool OTSPlanLevelTypeIsOverridden
		{
			get { return _OTSPlanLevelTypeIsOverridden ; }
			set { _OTSPlanLevelTypeIsOverridden = value; }
		}

		/// <summary>
		/// Gets or sets the type of override for the OTS Plan Level.
		/// </summary>
		public eOTSPlanLevelType OTSPlanLevelType
		{
			get { return _OTSPlanLevelType ; }
			set { _OTSPlanLevelType = value; }
		}

		public bool UseBasicReplenishment 
		{
			get { return _useBasicReplenishment ; }
			set { _useBasicReplenishment = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the color or size code.
		/// </summary>
		public int ColorOrSizeCodeRID 
		{
			get { return _colorOrSizeCodeRID ; }
			set { _colorOrSizeCodeRID = value; }
		}

		public int PurgeDailyHistoryAfter
		{
			get { return _purgeDailyHistoryAfter ; }
			set { _purgeDailyHistoryAfter = value; }
		}

		/// <summary>
		/// Gets or sets purge criteria for weekly history.
		/// </summary>
		public int PurgeWeeklyHistoryAfter
		{
			get { return _purgeWeeklyHistoryAfter ; }
			set { _purgeWeeklyHistoryAfter = value; }
		}

		/// <summary>
		/// Gets or sets purge criteria for OTS plans.
		/// </summary>
		public int PurgeOTSPlansAfter
		{
			get { return _purgeOTSPlansAfter ; }
			set { _purgeOTSPlansAfter = value; }
		}

        /* BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */
        ///// <summary>
        ///// Gets or sets purge criteria for headers.
        ///// </summary>
        //public int PurgeHeadersAfter
        //{
        //    get { return _purgeHeadersAfter ; }
        //    set { _purgeHeadersAfter = value; }
        //}

        /// <summary>
        /// Gets or sets purge criteria for ASN header type.
        /// </summary>
        public int PurgeHtASNAfter
        {
            get { return _purgeHtASNAfter; }
            set { _purgeHtASNAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for DropShip header type.
        /// </summary>
        public int PurgeHtDropShipAfter
        {
            get { return _purgeHtDropShipAfter; }
            set { _purgeHtDropShipAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for Dummy header type.
        /// </summary>
        public int PurgeHtDummyAfter
        {
            get { return _purgeHtDummyAfter; }
            set { _purgeHtDummyAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for Receipt header type.
        /// </summary>
        public int PurgeHtReceiptAfter
        {
            get { return _purgeHtReceiptAfter; }
            set { _purgeHtReceiptAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for PurchaseOrder header type.
        /// </summary>
        public int PurgeHtPurchaseOrderAfter
        {
            get { return _purgeHtPurchaseOrderAfter; }
            set { _purgeHtPurchaseOrderAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for Reserve header type.
        /// </summary>
        public int PurgeHtReserveAfter
        {
            get { return _purgeHtReserveAfter; }
            set { _purgeHtReserveAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for VSW header type.
        /// </summary>
        public int PurgeHtVSWAfter
        {
            get { return _purgeHtVSWAfter; }
            set { _purgeHtVSWAfter = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for WorkUpTotal header type.
        /// </summary>
        public int PurgeHtWorkUpTotAfter
        {
            get { return _purgeHtWorkUpTotAfter; }
            set { _purgeHtWorkUpTotAfter = value; }
        }
        /* END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */

        /// <summary>
		/// Gets or sets the store group RID used for stock min/maxes.
		/// </summary>
		public int StockMinMaxSgRID
		{
			get { return _stockMinMaxSgRID ; }
			set { _stockMinMaxSgRID = value; }
		}

		/// <summary>
		/// Gets or sets the flag identifying if the node is a virtual node.
		/// </summary>
		public bool IsVirtual
		{
			get { return _virtualInd ; }
			set { _virtualInd = value; }
		}

        /// <summary>
        /// Gets or sets the field identifying the purpose of the node.
        /// </summary>
        public ePurpose Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }

		/// <summary>
		/// Gets or sets the flag identifying if the characteristics have been loaded.
		/// </summary>
		public bool ProductCharsLoaded
		{
			get { return _productCharsLoadedInd; }
			set { _productCharsLoadedInd = value; }
		}
		public object[] ProductCharValues
		{
			get { return _productCharValues; }
			set { _productCharValues = value; }
		}

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        public bool Active
        {
            get { return _activeInd; }
            set { _activeInd = value; }
        }
        // End TT#988

        // Begin TT#3630 - JSmith - Delete My Hierarchy
        public bool DeleteNode
        {
            get { return _deleteNode; }
            set { _deleteNode = value; }
        }
        // End // TT#3630 - JSmith - Delete My Hierarchy

		#region ICloneable Members

		public object Clone()
		{
			try
			{
				NodeInfo ni = (NodeInfo)this.MemberwiseClone();
				return ni;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		#endregion

		#region Characteristic Management
		public int CharValueCount()
		{
			try
			{
				if (_productCharValues == null)
				{
					return 0;
				}
				else
				{
					return _productCharValues.Length;
				}
			}
			catch
			{
				throw;
			}
		}

		public bool ContainsCharValue(int aCharacteristicRID)
		{
			try
			{
				if (_productCharValues != null)
				{
					foreach (int CharacteristicRID in _productCharValues)
					{
						if (CharacteristicRID == aCharacteristicRID)
						{
							return true;
						}
					}
				}
				return false;
			}
			catch
			{
				throw;
			}
		}

		public void AddCharValue(int aCharacteristicRID)
		{
			try
			{
				object[] objectVector;
				if (_productCharValues == null)
				{
					_productCharValues = new object[1];
				}
				else
				{
					objectVector = new object[_productCharValues.Length + 1];
					System.Array.Copy(_productCharValues, objectVector, _productCharValues.Length);
					_productCharValues = objectVector;
				}
				_productCharValues[_productCharValues.Length - 1] = aCharacteristicRID;
			}
			catch
			{
				throw;
			}
		}

		public void RemoveCharValue(int aCharacteristicRID)
		{
			try
			{
				ArrayList objectVector = new ArrayList();
				objectVector.AddRange(_productCharValues);
				objectVector.Remove(aCharacteristicRID);
				if (objectVector.Count == 0)
				{
					_productCharValues = null;
				}
				else
				{
					_productCharValues = new object[objectVector.Count];
					objectVector.CopyTo(0, _productCharValues, 0, objectVector.Count);
				}
			}
			catch
			{
				throw;
			}
		}

		public void ClearCharValues()
		{
			try
			{
				_productCharValues = null;
			}
			catch
			{
				throw;
			}
		}
		#endregion
	}

    //BEGIN TT#1401 - Reservation (IMO) Stores - gtaylor
    /// <summary>
    /// Contains the information about the imo info for a  node in a hierarchy
    /// </summary>
    [Serializable()]
    public class NodeIMOInfo
    {
        // Fields

        private Hashtable _imo;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public NodeIMOInfo()
        {
            _imo = new Hashtable();
        }

        // Properties

        /// <summary>
        /// Gets or sets the imo Hashtable.
        /// </summary>
        public Hashtable IMO
        {
            get { return _imo; }
            set { _imo = value; }
        }
    }
    //END TT#1401 - Reservation (IMO) Stores - gtaylor

	/// <summary>
	/// Contains the information about the eligibility for a  node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeEligibilityInfo
	{
		// Fields

		private Hashtable			_storeEligibility;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeEligibilityInfo()
		{
			_storeEligibility = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Gets or sets the store eligibility Hashtable.
		/// </summary>
		public Hashtable StoreEligibility
		{
			get { return _storeEligibility ; }
			set { _storeEligibility = value; }
		}
	}

	//Begin TT#155 - JScott - Add Size Curve info to Node Properties
	/// <summary>
	/// Contains the information about the size curve criteria for a  node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeSizeCurveCriteriaInfo
	{
		// Fields

		private Hashtable _sizeCurveCriteria;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeSizeCurveCriteriaInfo()
		{
			_sizeCurveCriteria = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Gets or sets the size curve criteria Hashtable.
		/// </summary>
		public Hashtable SizeCurveCriteria
		{
			get { return _sizeCurveCriteria; }
			set { _sizeCurveCriteria = value; }
		}
	}

	[Serializable()]
	public class NodeSizeCurveDefaultCriteriaInfo
	{
		// Fields

		private SizeCurveDefaultCriteriaInfo _sizeCurveDefaultCriteriaInfo;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeSizeCurveDefaultCriteriaInfo()
		{
			_sizeCurveDefaultCriteriaInfo = new SizeCurveDefaultCriteriaInfo();
		}

		// Properties

		/// <summary>
		/// Gets or sets the size curve default criteria object.
		/// </summary>
		public SizeCurveDefaultCriteriaInfo SizeCurveDefaultCriteriaInfo
		{
			get { return _sizeCurveDefaultCriteriaInfo; }
		}
	}

	[Serializable()]
	public class NodeSizeCurveToleranceInfo
	{
		// Fields

		private SizeCurveToleranceInfo _sizeCurveToleranceInfo;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeSizeCurveToleranceInfo()
		{
			_sizeCurveToleranceInfo = new SizeCurveToleranceInfo();
		}

		// Properties

		/// <summary>
		/// Gets or sets the size curve default criteria object.
		/// </summary>
		public SizeCurveToleranceInfo SizeCurveToleranceInfo
		{
			get { return _sizeCurveToleranceInfo; }
		}
	}

	[Serializable()]
	public class NodeSizeCurveSimilarStoreInfo
	{
		// Fields

		private Hashtable _storeSizeCurveSimilarStore;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeSizeCurveSimilarStoreInfo()
		{
			_storeSizeCurveSimilarStore = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Gets or sets the store size curve similar store Hashtable.
		/// </summary>
		public Hashtable StoreSizeCurveSimilarStore
		{
			get { return _storeSizeCurveSimilarStore; }
			set { _storeSizeCurveSimilarStore = value; }
		}
	}

	//End TT#155 - JScott - Add Size Curve info to Node Properties
	//Begin TT#483 -- Add Size Lost Sales criteria and processing
	[Serializable()]
	public class NodeSizeSellThruInfo
	{
		// Fields

		private SizeSellThruInfo _sizeSellThruInfo;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeSizeSellThruInfo()
		{
			_sizeSellThruInfo = new SizeSellThruInfo();
		}

		// Properties

		/// <summary>
		/// Gets or sets the store size curve similar store Hashtable.
		/// </summary>
		public SizeSellThruInfo SizeSellThruInfo
		{
			get { return _sizeSellThruInfo; }
		}
	}

	//End TT#483 -- Add Size Lost Sales criteria and processing
	/// <summary>
	/// Contains the information about the store grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeStoreGradesInfo
	{
		// Fields

		private bool				_storeGradesFound;
		private bool				_minMaxesFound;
		private bool				_storeGradesLoaded;
		private ArrayList			_storeGrades;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStoreGradesInfo()
		{
			_storeGradesLoaded = false;
			_storeGradesFound = false;
			_minMaxesFound = false;
			_storeGrades = new ArrayList();
		}

		// Properties

		/// <summary>
		/// Gets or sets the flag that identifies if store grades has been found for the node.
		/// </summary>
		public bool StoreGradesFound
		{
			get { return _storeGradesFound ; }
			set { _storeGradesFound = value; }
		}

		/// <summary>
		/// Gets or sets the flag that identifies if min/maxes have been found for the node.
		/// </summary>
		public bool MinMaxesFound
		{
			get { return _minMaxesFound ; }
			set { _minMaxesFound = value; }
		}

		/// <summary>
		/// Gets or sets the flag that identifies if store grades has been loaded for the node.
		/// </summary>
		public bool StoreGradesLoaded
		{
			get { return _storeGradesLoaded ; }
			set { _storeGradesLoaded = value; }
		}

		/// <summary>
		/// Gets or sets the store grades ArrayList.
		/// </summary>
		public ArrayList StoreGrades
		{
			get { return _storeGrades ; }
			set { _storeGrades = value; }
		}
	}

	/// <summary>
	/// Contains the information about the store capacity for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeStoreCapacityInfo
	{
		// Fields

		private Hashtable			_storeCapacity;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStoreCapacityInfo()
		{
			_storeCapacity = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Gets or sets the store capacity Hashtable.
		/// </summary>
		public Hashtable StoreCapacity
		{
			get { return _storeCapacity ; }
			set { _storeCapacity = value; }
		}
	}

    //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
    /// <summary>
    /// Contains the information about the Chain Set Percentages for a node in a hierarchy
    /// </summary>
    [Serializable()]
    public class NodeChainSetPercentInfo
    {
        // Fields

        private Hashtable _chainSetPercent;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public NodeChainSetPercentInfo()
        {
            _chainSetPercent = new Hashtable();
        }

        // Properties

        /// <summary>
        /// Gets or sets the store capacity Hashtable.
        /// </summary>
        public Hashtable ChainSetPercent
        {
            get { return _chainSetPercent; }
            set { _chainSetPercent = value; }
        }
    }
    //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3

	/// <summary>
	/// Contains the information about the velocity grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeVelocityGradesInfo
	{
		// Fields

		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		private bool				_velocityGradesFound;
		private bool				_velocityMinMaxesFound;
		//End TT#505 - JScott - Velocity - Apply Min/Max
		private ArrayList			_velocityGrades;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeVelocityGradesInfo()
		{
			_velocityGrades = new ArrayList();
		}

		// Properties

		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Gets or sets the flag that identifies if velocity grades has been found for the node.
		/// </summary>
		public bool VelocityGradesFound
		{
			get { return _velocityGradesFound ; }
			set { _velocityGradesFound = value; }
		}

		/// <summary>
		/// Gets or sets the flag that identifies if velocity min/maxes have been found for the node.
		/// </summary>
		public bool VelocityMinMaxesFound
		{
			get { return _velocityMinMaxesFound; }
			set { _velocityMinMaxesFound = value; }
		}
		//End TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Gets or sets the velocity grades ArrayList.
		/// </summary>
		public ArrayList VelocityGrades
		{
			get { return _velocityGrades ; }
			set { _velocityGrades = value; }
		}
	}

	/// <summary>
	/// Contains the information about the sell thru percents for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeSellThruPctsInfo
	{
		// Fields

		private ArrayList			_sellThruPcts;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeSellThruPctsInfo()
		{
			_sellThruPcts = new ArrayList();
		}

		// Properties

		/// <summary>
		/// Gets or sets the sell thru percents ArrayList.
		/// </summary>
		public ArrayList SellThruPcts
		{
			get { return _sellThruPcts ; }
			set { _sellThruPcts = value; }
		}
	}

	/// <summary>
	/// Contains the information about the daily percentages for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeDailyPercentagesInfo
	{
		// Fields

        private Hashtable			_storeDailyPercentages;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeDailyPercentagesInfo()
		{
            _storeDailyPercentages = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Gets or sets the daily percentages Hashtable.
		/// </summary>
        public Hashtable StoreDailyPercentages
		{
			get { return _storeDailyPercentages ; }
			set { _storeDailyPercentages = value; }
		}
	}

    //BEGIN TT#1401 - Reservation (IMO) Stores - gtaylor
    /// <summary>
    /// contains the information about the imo detail for a store for a node in a hierarchy
    /// </summary>
    ///
    [Serializable()]
    public partial class IMOBaseInfo
    {
        private int _imoMinShipQty;
        private double _imoPackQty;
        private int _imoMaxValue;
        private int _imoStoreRID;
        private double _imoFWOSMax;
        private int _imoFWOSMaxRID; //TT#108 - MD - DOConnell - FWOS Max Model
        private eModifierType _imoFWOSMaxType; //TT#108 - MD - DOConnell - FWOS Max Model
        public int IMOMinShipQty
        {
            get { return _imoMinShipQty; }
            set { _imoMinShipQty = value; }
        }
        public double IMOPackQty
        {
            get { return _imoPackQty; }
            set { _imoPackQty = value; }
        }
        public int IMOMaxValue
        {
            get { return _imoMaxValue; }
            set { _imoMaxValue = value; }
        }
        public int IMOStoreRID
        {
            get { return _imoStoreRID; }
            set { _imoStoreRID = value; }
        }
        public double IMOFWOSMax
        {
            get { return _imoFWOSMax; }
            set { _imoFWOSMax = value; }
        }
        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
        public int IMOFWOSMaxRID
        {
            get { return _imoFWOSMaxRID; }
            set { _imoFWOSMaxRID = value; }
        }

        public eModifierType IMOFWOSMaxType
        {
            get { return _imoFWOSMaxType; }
            set { _imoFWOSMaxType = value; }
        }
        //END TT#108 - MD - DOConnell - FWOS Max Model
        public IMOBaseInfo()     
        {
            _imoMinShipQty = 0;
            _imoPackQty = .5;
            _imoFWOSMax = 0;
            _imoMaxValue = int.MaxValue;
            _imoStoreRID = Include.NoRID;
            _imoFWOSMaxRID = Include.NoRID; //TT#108 - MD - DOConnell - FWOS Max Model
        }
    }

    [Serializable()]
    public partial class IMOInfo : IMOBaseInfo
    {
        // Fields
        private int     _imoPshToBackStock;
        private int     _imoNodeRID;

        // Properties
        public int      IMOPshToBackStock
        {
            get { return _imoPshToBackStock; }
            set { _imoPshToBackStock = value; }
        }
        public int      IMONodeRID
        {
            get { return _imoNodeRID; }
            set { _imoNodeRID = value; }
        }

        public IMOInfo()
        {
            _imoPshToBackStock = Include.UndefinedCalendarDateRange;
            _imoNodeRID = Include.NoRID;
        }    
    }

    [Serializable()]
    public partial class IMOMethodOverrideInfo : IMOBaseInfo
    {
        private int _imoMethodRID;
        public int IMOMethodRID
        {
            get { return _imoMethodRID; }
            set { _imoMethodRID = value; }
        }
        public IMOMethodOverrideInfo()
        {
            _imoMethodRID = Include.NoRID;
        }
    }
    //END TT#1401 - Reservation (IMO) Stores - gtaylor

	/// <summary>
	/// Contains the information about the eligibility for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreEligibilityInfo
	{
		// Fields

		private int					_storeRID;
		private bool				_useEligibleModel;
		private int					_eligibilityModelRID;
		private bool				_useStoreEligibility;
		private bool				_storeIneligible;
		private eModifierType		_stkModType;
		private int					_stkModModelRID;
		private double				_stkModPct;
		private eModifierType		_slsModType;
		private int					_slsModModelRID;
		private double				_slsModPct;
		private eModifierType		_FWOSModType;
		private int					_FWOSModModelRID;
		private double				_FWOSModPct;
		private eSimilarStoreType	_simStoreType;
		private ArrayList			_simStores;
		private double				_simStoreRatio;
		private int					_simStoreUntilDateRangeRID;
		private int					_origSimStoreUntilDateRangeRID;
		private DateRangeProfile	_dateRangeProfile;
		private bool				_presPlusSalesIsSet;
		private bool				_presPlusSalesInd;
        //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
        private int                 _StkLeadWeeks;
        //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreEligibilityInfo()
		{
			_storeRID					= Include.NoRID;
			_useEligibleModel			= false;
			_eligibilityModelRID		= Include.NoRID;
			_storeIneligible			= false;
			_useStoreEligibility		= false;
			_stkModType					= eModifierType.None;
			_stkModModelRID				= Include.NoRID;
			_stkModPct					= 0;
			_slsModType					= eModifierType.None;
			_slsModModelRID				= Include.NoRID;
			_slsModPct					= 0;
			_FWOSModType				= eModifierType.None;
			_FWOSModModelRID			= Include.NoRID;
			_FWOSModPct					= 0;
			_simStoreType				= eSimilarStoreType.None;
			_simStores					= new ArrayList();
			_simStoreRatio				= 0;
			_simStoreUntilDateRangeRID	= Include.NoRID;
			_origSimStoreUntilDateRangeRID	= Include.NoRID;
			_presPlusSalesIsSet			= false;
			_presPlusSalesInd			= false;
            //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
            _StkLeadWeeks               = Include.NoRID;
            //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
		}

		// Properties

		/// <summary>
		/// Gets or sets the store record id.
		/// </summary>
		public int StoreRID 
		{
			get { return _storeRID ; }
			set { _storeRID = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the eligibility model is to be used.
		/// </summary>
		public bool UseEligibleModel 
		{
			get { return _useEligibleModel ; }
			set { _useEligibleModel = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the eligibility model for the store.
		/// </summary>
		public int EligModelRID 
		{
			get { return _eligibilityModelRID ; }
			set { _eligibilityModelRID = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the store eligibility setting is to be used.
		/// </summary>
		public bool UseStoreEligibility 
		{
			get { return _useStoreEligibility ; }
			set { _useStoreEligibility = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying that a store is ineligible.
		/// </summary>
		public bool StoreIneligible 
		{
			get { return _storeIneligible ; }
			set { _storeIneligible = value; }
		}
		/// <summary>
		/// Gets or sets the type of stock modifier.
		/// </summary>
		public eModifierType StkModType 
		{
			get { return _stkModType ; }
			set { _stkModType = value; }
		}
		/// <summary>
		/// Gets or sets the record id for the stock modifier model.
		/// </summary>
		public int StkModModelRID 
		{
			get { return _stkModModelRID ; }
			set { _stkModModelRID = value; }
		}
		/// <summary>
		/// Gets or sets the stock modifier percent for the store if a model is not used.
		/// </summary>
		public double StkModPct 
		{
			get { return _stkModPct ; }
			set { _stkModPct = value; }
		}
		/// <summary>
		/// Gets or sets the sales modifier.
		/// </summary>
		public eModifierType SlsModType 
		{
			get { return _slsModType ; }
			set { _slsModType = value; }
		}
		/// <summary>
		/// Gets or sets the record id for the sales modifier model.
		/// </summary>
		public int SlsModModelRID 
		{
			get { return _slsModModelRID ; }
			set { _slsModModelRID = value; }
		}
		/// <summary>
		/// Gets or sets the sales modifier percent for the store if a model is not used.
		/// </summary>
		public double SlsModPct 
		{
			get { return _slsModPct ; }
			set { _slsModPct = value; }
		}
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		/// <summary>
		/// Gets or sets the FWOS modifier.
		/// </summary>
		public eModifierType FWOSModType 
		{
			get { return _FWOSModType ; }
			set { _FWOSModType = value; }
		}
		/// <summary>
		/// Gets or sets the record id for the FWOS modifier model.
		/// </summary>
		public int FWOSModModelRID 
		{
			get { return _FWOSModModelRID ; }
			set { _FWOSModModelRID = value; }
		}
		/// <summary>
		/// Gets or sets the FWOS modifier percent for the store if a model is not used.
		/// </summary>
		public double FWOSModPct 
		{
			get { return _FWOSModPct ; }
			set { _FWOSModPct = value; }
		}
		// END MID Track #4370
		/// <summary>
		/// Gets or sets the type of simlar store identified for the store.
		/// </summary>
		public eSimilarStoreType SimStoreType 
		{
			get { return _simStoreType ; }
			set { _simStoreType = value; }
		}
		/// <summary>
		/// Gets or sets the list of simlar stores identified for the store.
		/// </summary>
		public ArrayList SimStores 
		{
			get { return _simStores ; }
			set { _simStores = value; }
		}
		/// <summary>
		/// Gets or sets the ratio of simlar store for the store.
		/// </summary>
		public double SimStoreRatio 
		{
			get { return _simStoreRatio ; }
			set { _simStoreRatio = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the date range to use for the similar store.
		/// </summary>
		public int SimStoreUntilDateRangeRID 
		{
			get { return _simStoreUntilDateRangeRID ; }
			set { _simStoreUntilDateRangeRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the date range to use for the similar store.
		/// </summary>
		public int OrigSimStoreUntilDateRangeRID 
		{
			get { return _origSimStoreUntilDateRangeRID ; }
			set { _origSimStoreUntilDateRangeRID = value; }
		}
		/// <summary>
		/// Gets or sets the date range profile to use for the similar store.
		/// </summary>
		public DateRangeProfile	DateRangeProfile 
		{
			get 
			{ 
				if (_dateRangeProfile == null)
				{
					_dateRangeProfile = new DateRangeProfile(0);
				}
				return _dateRangeProfile ; 
			}
			set { _dateRangeProfile = value; }
		}
		// BEGIN MID Track #4827 - John Smith - Presentation plus sales
		/// <summary>
		/// Gets or sets the flag identifying if the presentation plus sales indicator was set.
		/// </summary>
		public bool PresPlusSalesIsSet 
		{
			get { return _presPlusSalesIsSet ; }
			set { _presPlusSalesIsSet = value; }
		}
		public bool PresPlusSalesInd 
		{
			get { return _presPlusSalesInd ; }
			set { _presPlusSalesInd = value; }
		}
		// END MID Track #4827
        //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
        public int StkLeadWeeks
        {
            get { return _StkLeadWeeks ; }
            set { _StkLeadWeeks = value; }
        }
        //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
	}

	//Begin TT#155 - JScott - Add Size Curve info to Node Properties
	[Serializable()]
	public class SizeCurveCriteriaInfo
	{
		// Fields

		private int _key;
		private eLowLevelsType _levelType;
		private int _levelRID;
		private int _levelSequence;
		private int _levelOffset;
		private int _dateRID;
		private bool _applyLostSalesInd;
		private int _OLLRID;
		private int _customOLLRID;
		private int _sizeGroupRID;
		private string _curveName;
		//Begin TT#1076 - JScott - Size Curves by Set
		private int _sgRID;
		//End TT#1076 - JScott - Size Curves by Set

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveCriteriaInfo(int aKey)
		{
			_key = aKey;
			_levelType = eLowLevelsType.None;
			_levelRID = Include.NoRID;
			_levelSequence = 0;
			_levelOffset = 0;
			_dateRID = Include.NoRID;
			_applyLostSalesInd = true;
			_OLLRID = Include.NoRID;
			_customOLLRID = Include.NoRID;
			_sizeGroupRID = Include.NoRID;
			_curveName = string.Empty;
			//Begin TT#1076 - JScott - Size Curves by Set
			_sgRID = Include.NoRID;
			//End TT#1076 - JScott - Size Curves by Set
		}

		// Properties

		/// <summary>
		/// Gets or sets the criteria key value.
		/// </summary>
		public int Key
		{
			get { return _key; }
		}
		/// <summary>
		/// Gets or sets the criteria level type value.
		/// </summary>
		public eLowLevelsType LevelType
		{
			get { return _levelType; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level value.
		/// </summary>
		public int LevelRID
		{
			get { return _levelRID; }
			set { _levelRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level sequence value.
		/// </summary>
		public int LevelSequence
		{
			get { return _levelSequence; }
			set { _levelSequence = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level offset value.
		/// </summary>
		public int LevelOffset
		{
			get { return _levelOffset; }
			set { _levelOffset = value; }
		}
		/// <summary>
		/// Gets or sets the criteria date value.
		/// </summary>
		public int DateRID
		{
			get { return _dateRID; }
			set { _dateRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria apply lost sales indicator.
		/// </summary>
		public bool ApplyLostSalesInd
		{
			get { return _applyLostSalesInd; }
			set { _applyLostSalesInd = value; }
		}
		/// <summary>
		/// Gets or sets the criteria override low-level value.
		/// </summary>
		public int OLLRID
		{
			get { return _OLLRID; }
			set { _OLLRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria custom override low-level value.
		/// </summary>
		public int CustomOLLRID
		{
			get { return _customOLLRID; }
			set { _customOLLRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria size group value.
		/// </summary>
		public int SizeGroupRID
		{
			get { return _sizeGroupRID; }
			set { _sizeGroupRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria curve name value.
		/// </summary>
		public string CurveName
		{
			get { return _curveName; }
			set { _curveName = value; }
		}
		//Begin TT#1076 - JScott - Size Curves by Set
		/// <summary>
		/// Gets or sets the criteria curve name value.
		/// </summary>
		public int SgRID
		{
			get { return _sgRID; }
			set { _sgRID = value; }
		}
		//End TT#1076 - JScott - Size Curves by Set
	}

	[Serializable()]
	public class SizeCurveDefaultCriteriaInfo
	{
		// Fields

		private int _defaultCriteriaRID;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveDefaultCriteriaInfo()
		{
			_defaultCriteriaRID = Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Gets or sets the default criteria RID value.
		/// </summary>
		public int DefaultCriteriaRID
		{
			get { return _defaultCriteriaRID; }
			set { _defaultCriteriaRID = value; }
		}
	}

	[Serializable()]
	public class SizeCurveToleranceInfo
	{
		// Fields

		private double _minAvg;
		private eLowLevelsType _levelType;
		private int _levelRID;
		private int _levelSeq;
		private int _levelOffset;
		private double _salesToler;
		private eNodeChainSalesType _idxUnitsInd;
        private double _minToler;
        private double _maxToler;
        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        private bool _applyMinToZeroTolerance;
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
        public SizeCurveToleranceInfo()
        {
            _minAvg = Include.Undefined;
            _levelType = eLowLevelsType.None;
            _levelRID = Include.NoRID;
            _levelSeq = Include.Undefined;
            _levelOffset = Include.Undefined;
            _salesToler = Include.Undefined;
            _idxUnitsInd = eNodeChainSalesType.None;
            _minToler = Include.Undefined;
            _maxToler = Include.Undefined;
            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
            _applyMinToZeroTolerance = false;
            //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        }

		// Properties

		/// <summary>
		/// Gets or sets the minimum average value.
		/// </summary>
		public double MinimumAvg
		{
			get { return _minAvg; }
			set { _minAvg = value; }
		}
		/// <summary>
		/// Gets or sets the level type value.
		/// </summary>
		public eLowLevelsType LevelType
		{
			get { return _levelType; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets the level RID value.
		/// </summary>
		public int LevelRID
		{
			get { return _levelRID; }
			set { _levelRID = value; }
		}
		/// <summary>
		/// Gets or sets the level sequence value.
		/// </summary>
		public int LevelSeq
		{
			get { return _levelSeq; }
			set { _levelSeq = value; }
		}
		/// <summary>
		/// Gets or sets the level offset value.
		/// </summary>
		public int LevelOffset
		{
			get { return _levelOffset; }
			set { _levelOffset = value; }
		}
		/// <summary>
		/// Gets or sets the sales tolerance value.
		/// </summary>
		public double SalesTolerance
		{
			get { return _salesToler; }
			set { _salesToler = value; }
		}
		/// <summary>
		/// Gets or sets the index units indicator value.
		/// </summary>
		public eNodeChainSalesType IndexUnitsInd
		{
			get { return _idxUnitsInd; }
			set { _idxUnitsInd = value; }
		}
		/// <summary>
		/// Gets or sets the minimum tolerance value.
		/// </summary>
		public double MinimumTolerance
		{
			get { return _minToler; }
			set { _minToler = value; }
		}
		/// <summary>
		/// Gets or sets the maximum tolerance value.
		/// </summary>
		public double MaximumTolerance
		{
			get { return _maxToler; }
			set { _maxToler = value; }
		}
        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        /// <summary>
        /// Gets or sets the maximum tolerance value.
        /// </summary>
        public bool ApplyMinToZeroTolerance
        {
            get { return _applyMinToZeroTolerance; }
            set { _applyMinToZeroTolerance = value; }
        }
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
	}

	[Serializable()]
	public class SizeCurveSimilarStoreInfo
	{
		// Fields

		private bool _changed;
		private int _storeRID;
		private int _simStoreRID;
		private int _untilDateRID;
		private int _origUntilDateRID;
		private DateRangeProfile _dateRangeProfile;
		private string _displayDate;
		private ProfileList _weekList;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveSimilarStoreInfo()
		{
			_changed = false;
			_storeRID = Include.NoRID;
			_simStoreRID = Include.NoRID;
			_untilDateRID = Include.NoRID;
			_origUntilDateRID = Include.NoRID;
			_displayDate = string.Empty;
			_weekList = null;
		}

		// Properties

		/// <summary>
		/// Gets or sets the switch to identify if the similar stores have been changed.
		/// </summary>
		public bool Changed
		{
			get { return _changed; }
			set { _changed = value; }
		}
		/// <summary>
		/// Gets or sets the list of simlar stores identified for the store.
		/// </summary>
		public int StoreRID
		{
			get { return _storeRID; }
			set { _storeRID = value; }
		}
		/// <summary>
		/// Gets or sets the list of simlar stores identified for the store.
		/// </summary>
		public int SimilarStoreRID
		{
			get { return _simStoreRID; }
			set { _simStoreRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the date range to use for the similar store.
		/// </summary>
		public int UntilDateRID
		{
			get { return _untilDateRID; }
			set { _untilDateRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the original date range to use for the similar store.
		/// </summary>
		public int OrigUntilDateRID
		{
			get { return _origUntilDateRID; }
			set { _origUntilDateRID = value; }
		}
		/// <summary>
		/// Gets or sets the date range profile to use for the similar store.
		/// </summary>
		public DateRangeProfile DateRangeProfile
		{
			get
			{
				if (_dateRangeProfile == null)
				{
					_dateRangeProfile = new DateRangeProfile(0);
				}
				return _dateRangeProfile;
			}
			set { _dateRangeProfile = value; }
		}
		/// <summary>
		/// Gets or sets the display date to use for the similar store.
		/// </summary>
		public string DisplayDate
		{
			get { return _displayDate; }
			set { _displayDate = value; }
		}
		/// <summary>
		/// Gets or sets the week list of the date range to use for the similar store.
		/// </summary>
		public ProfileList WeekList
		{
			get { return _weekList; }
			set { _weekList = value; }
		}
	}

	//End TT#155 - JScott - Add Size Curve info to Node Properties
	//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
	[Serializable()]
	public class SizeSellThruInfo
	{
		// Fields

		private bool _changed;
		private float _sellThruLimit;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeSellThruInfo()
		{
			_changed = false;
			_sellThruLimit = Include.Undefined;
		}
		// Properties

		/// <summary>
		/// Gets or sets the switch to identify if the similar stores have been changed.
		/// </summary>
		public bool Changed
		{
			get { return _changed; }
			set { _changed = value; }
		}
		/// <summary>
		/// Gets or sets the key for the group level.
		/// </summary>
		public float SellThruLimit
		{
			get { return _sellThruLimit; }
			set { _sellThruLimit = value; }
		}
	}

	//End TT#483 - JScott - Add Size Lost Sales criteria and processing
	/// <summary>
	/// Contains the information about the store grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreGradeInfo
	{
		// Fields

		private string				_storeGrade;
		private int					_boundary;
		private int					_wosIndex;
		private int					_minStock;
		private int					_maxStock;
		private int					_minAd;
		private int					_minColor;
		private int					_maxColor;
        private int                 _shipUpTo;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreGradeInfo()
		{
			_boundary = Include.Undefined;
			_wosIndex = Include.Undefined;
			_minStock = Include.Undefined;
			_maxStock = Include.Undefined;
			_minAd = Include.Undefined;
			_minColor = Include.Undefined;
			_maxColor = Include.Undefined;
            _shipUpTo = Include.Undefined;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
		}

		// Properties
		
		/// <summary>
		/// Gets or sets the code for the store grade.
		/// </summary>
		public string StoreGrade 
		{
			get { return _storeGrade ; }
			set { _storeGrade = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the boundary for the store grade.
		/// </summary>
		public int Boundary 
		{
			get { return _boundary ; }
			set { _boundary = value; }
		}
		/// <summary>
		/// Gets or sets the WOS Index for the store grade.
		/// </summary>
		public int WosIndex 
		{
			get { return _wosIndex ; }
			set { _wosIndex = value; }
		}
		/// <summary>
		/// Gets or sets the Min Stock for the store grade.
		/// </summary>
		public int MinStock 
		{
			get { return _minStock ; }
			set { _minStock = value; }
		}
		/// <summary>
		/// Gets or sets the Max Stock for the store grade.
		/// </summary>
		public int MaxStock 
		{
			get { return _maxStock ; }
			set { _maxStock = value; }
		}
		/// <summary>
		/// Gets or sets the Min Ad for the store grade.
		/// </summary>
		public int MinAd 
		{
			get { return _minAd ; }
			set { _minAd = value; }
		}

        /// <summary>
		/// Gets or sets the Color Min for the store grade.
		/// </summary>
		public int MinColor 
		{
			get { return _minColor ; }
			set { _minColor = value; }
		}
		/// <summary>
		/// Gets or sets the Color Max for the store grade.
		/// </summary>
		public int MaxColor 
		{
			get { return _maxColor ; }
			set { _maxColor = value; }
		}
        // BEGIN TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
        public int ShipUpTo
        {
            get { return _shipUpTo; }
            set { _shipUpTo = value; }
        }
        // END TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a node in a hierarchy
	/// </summary>
	public class NodeStockMinMaxesInfo
	{
		// Fields

		private ArrayList	_nodeSetList;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxesInfo()
		{
			_nodeSetList = null;
		}

		// Properties

		/// <summary>
		/// Gets or sets the NodeSetList.
		/// </summary>
		public ArrayList NodeSetList 
		{
			get 
			{ 
				if (_nodeSetList == null)
				{
					_nodeSetList = new ArrayList();
				}
				return _nodeSetList ; 
			}
			set { _nodeSetList = value; }
		}
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a set
	/// </summary>
	public class NodeStockMinMaxSetInfo
	{
		// Fields

		private int							_setRID;					
		private NodeStockMinMaxBoundaryInfo	_defaults;
		private ArrayList					_boundaryList;
		

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxSetInfo()
		{
			_setRID = Include.NoRID;
			_boundaryList = null;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxSetInfo(int aSetRID)
		{
			_setRID = aSetRID;
			_boundaryList = null;
		}

		// Properties

		/// <summary>
		/// Gets or sets the record ID for the set.
		/// </summary>
		public int SetRID 
		{
			get { return _setRID ; }
			set { _setRID = value; }
		}
		/// <summary>
		/// Gets or sets the default values for the set.
		/// </summary>
		public NodeStockMinMaxBoundaryInfo Defaults 
		{
			get 
			{ 
				if (_defaults == null)
				{
					_defaults = new NodeStockMinMaxBoundaryInfo();
				}
				return _defaults ; 
			}
			set { _defaults = value; }
		}
		/// <summary>
		/// Gets or sets the boundary list for the set.
		/// </summary>
		public ArrayList BoundaryList 
		{
			get 
			{ 
				if (_boundaryList == null)
				{
					_boundaryList = new ArrayList();
				}
				return _boundaryList ; 
			}
			set { _boundaryList = value; }
		}		
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a store grade
	/// </summary>
	public class NodeStockMinMaxBoundaryInfo
	{
		// Fields

		private int					_boundary;
		private ArrayList			_minMaxList;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxBoundaryInfo()
		{
			_minMaxList = null;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxBoundaryInfo(int aBoundary)
		{
			_boundary = aBoundary;
			_minMaxList = null;
		}

		// Properties

		/// <summary>
		/// Gets or sets the boundary for the store grade.
		/// </summary>
		public int Boundary 
		{
			get { return _boundary ; }
			set { _boundary = value; }
		}
		/// <summary>
		/// Gets or sets the list of min/maxes for the store grade.
		/// </summary>
		public ArrayList MinMaxList 
		{
			get 
			{ 
				if (_minMaxList == null)
				{
					_minMaxList = new ArrayList();
				}
				return _minMaxList ; 
			}
			set { _minMaxList = value; }
		}
	}

	/// <summary>
	/// Contains the information about the stock min/maxes
	/// </summary>
	public class NodeStockMinMaxInfo
	{
		// Fields

		private int					_dateRangeKey;
		private int					_minimum;
		private int					_maximum;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxInfo()
		{
			_minimum = int.MinValue;
			_maximum = int.MaxValue;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxInfo(int aDateRangeKey, int aMinimum, int aMaximum)
		{
			_dateRangeKey = aDateRangeKey;
			_minimum = aMinimum;
			_maximum = aMaximum;
		}

		// Properties

		/// <summary>
		/// Gets or sets the date range key for the min/max values.
		/// </summary>
		public int DateRangeKey 
		{
			get { return _dateRangeKey ; }
			set { _dateRangeKey = value; }
		}
		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		public int Minimum 
		{
			get { return _minimum ; }
			set { _minimum = value; }
		}
		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		public int Maximum 
		{
			get { return _maximum ; }
			set { _maximum = value; }
		}
	}

	/// <summary>
	/// Contains the information about the capacity for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreCapacityInfo
	{
		// Fields

		private int					_storeRID;
		private int					_storeCapacity;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreCapacityInfo()
		{
			_storeRID					= Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Gets or sets the store record id.
		/// </summary>
		public int StoreRID 
		{
			get { return _storeRID ; }
			set { _storeRID = value; }
		}
		/// <summary>
		/// Gets or sets the capacity for the store.
		/// </summary>
		public int StoreCapacity 
		{
			get { return _storeCapacity ; }
			set { _storeCapacity = value; }
		}
	}

    //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
    /// <summary>
    /// Contains the information about the Chain set Percentages for a store for a node in a hierarchy
    /// </summary>
    [Serializable()]
    public class ChainSetPercentInfo
    {
        // Fields
        //private int _storeWeekID;
        private int _storeGroupRID;
        private string _storeGroupID;
        private int _storeGroupVersion; // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        private int _storeGroupLevelRID;
        private string _storeGroupLevelID;
        private decimal _chainSetPercent;
        private int _TimeID;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChainSetPercentInfo()
        {
            _storeGroupRID = Include.NoRID;
        }

        // Properties

        /// <summary>
        /// Gets or sets the Store Week Combo Id.
        /// </summary>
        //public int StoreWeekID
        //{
        //    get { return _storeWeekID; }
        //    set { _storeWeekID = value; }
        //}

        /// <summary>
        /// Gets or sets the Store Group Rid.
        /// </summary>
        public int StoreGroupRID
        {
            get { return _storeGroupRID; }
            set { _storeGroupRID = value; }
        }

        /// <summary>
        /// Gets or sets the Store Group id.
        /// </summary>
        public string storeGroupId
        {
            get { return _storeGroupID; }
            set { _storeGroupID = value; }
        }

        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        public int StoreGroupVersion
        {
            get { return _storeGroupVersion; }
            set { _storeGroupVersion = value; }
        }
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.

        /// <summary>
        /// Gets or sets the Store Group Level record id.
        /// </summary>
        public int StoreGroupLevelRID
        {
            get { return _storeGroupLevelRID; }
            set { _storeGroupLevelRID = value; }
        }

        /// <summary>
        /// Gets or sets the Store Group Level id.
        /// </summary>
        public string storeGroupLevelId
        {
            get { return _storeGroupLevelID; }
            set { _storeGroupLevelID = value; }
        }

        /// <summary>
        /// Gets or sets the Percentage for the store.
        /// </summary>
        public decimal ChainSetPercent
        {
            get { return _chainSetPercent; }
            set { _chainSetPercent = value; }
        }

        /// <summary>
        /// Gets or sets the Time ID for the Node.
        /// </summary>
        public int TimeID
        {
            get { return _TimeID; }
            set { _TimeID = value; }
        }

        /// <summary>
        /// Gets the YYYYDDD of the item.
        /// </summary>
        public int YearDay
        {
            get 
            { 
                return Convert.ToInt32(TimeID.ToString().Substring(0,7)); 
            }
        }

    }
    //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3

	/// <summary>
	/// Contains the information about the velocity grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class VelocityGradeInfo
	{
		// Fields

		private string				_velocityGrade;
		private int					_boundary;
//		private int					_sellThruPct;
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		private int					_velocityMinStock;
		private int					_velocityMaxStock;
		private int					_velocityMinAd;
		//End TT#505 - JScott - Velocity - Apply Min/Max
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public VelocityGradeInfo()
		{
			//Begin TT#1176 - JScott - Node properties when delete min or max and Apply or select OK and go back in the columns are populated with 0's, instead of being blank
			_boundary = Include.Undefined;
			_velocityMinStock = Include.Undefined;
			_velocityMaxStock = Include.Undefined;
			_velocityMinAd = Include.Undefined;
			//End TT#1176 - JScott - Node properties when delete min or max and Apply or select OK and go back in the columns are populated with 0's, instead of being blank
		}

		// Properties
		
		/// <summary>
		/// Gets or sets the code for the velocity grade.
		/// </summary>
		public string VelocityGrade 
		{
			get { return _velocityGrade ; }
			set { _velocityGrade = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the boundary for the velocity grade.
		/// </summary>
		public int Boundary 
		{
			get { return _boundary ; }
			set { _boundary = value; }
		}
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		/// <summary>
		/// Gets or sets the Min Stock for the velocity grade.
		/// </summary>
		public int VelocityMinStock
		{
			get { return _velocityMinStock; }
			set { _velocityMinStock = value; }
		}
		/// <summary>
		/// Gets or sets the Max Stock for the velocity grade.
		/// </summary>
		public int VelocityMaxStock
		{
			get { return _velocityMaxStock; }
			set { _velocityMaxStock = value; }
		}
		/// <summary>
		/// Gets or sets the Min Ad for the velocity grade.
		/// </summary>
		public int VelocityMinAd
		{
			get { return _velocityMinAd; }
			set { _velocityMinAd = value; }
		}
		//End TT#505 - JScott - Velocity - Apply Min/Max
	}


	/// <summary>
	/// Contains the information about the velocity grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SellThruPctInfo
	{
		// Fields

		private int					_sellThruPct;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SellThruPctInfo()
		{
		}

		// Properties
		
		/// <summary>
		/// Gets or sets the sell thru percent for the velocity grade.
		/// </summary>
		public int SellThruPct 
		{
			get { return _sellThruPct ; }
			set { _sellThruPct = value; }
		}
	}

	/// <summary>
	/// Contains the information about a model
	/// </summary>
	[Serializable()]
	abstract public class ModelInfo
	{
		private int							_modelRID;
		private string						_modelID;
		private ArrayList					_modelEntries;
		private DateTime					_updateDateTime;
		private Hashtable					_modelDateEntries;
		private bool						_containsDynamicDates;
		private bool						_containsStoreDynamicDates;
		private bool						_containsPlanDynamicDates;
		private bool						_containsReoccurringDates;
		private bool						_needsRebuilt;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private bool _modelDateEntriesLoadedByStore;
        // End TT#2307

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelInfo()
		{
			_modelEntries = new ArrayList();
			_updateDateTime = DateTime.Now;
			_modelDateEntries = new Hashtable();
            // Begin TT#2307 - JSmith - Incorrect Stock Values
            _modelDateEntriesLoadedByStore = false;
            // End TT#2307
			_containsDynamicDates = false;
			_containsStoreDynamicDates = false;
			_containsPlanDynamicDates = false;
			_containsReoccurringDates = false;
			_needsRebuilt = true;
		}

		/// <summary>
		/// Gets or sets the record id for the model.
		/// </summary>
		public int ModelRID 
		{
			get { return _modelRID ; }
			set { _modelRID = value; }
		}
		/// <summary>
		/// Gets or sets the id of the  model.
		/// </summary>
		public string ModelID 
		{
			get { return _modelID ; }
			set { _modelID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the list of model entries.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of inforamation for the type of model
		/// </remarks>
		public ArrayList ModelEntries 
		{
			get { return _modelEntries ; }
			set { _modelEntries = value; }
		}
		/// <summary>
		/// Gets or sets the date and time stamp of the last time this model was updated.
		/// </summary>
		public DateTime	UpdateDateTime 
		{
			get { return _updateDateTime ; }
			set { _updateDateTime = value; }
		}
		/// <summary>
		/// Gets the string value for date and time stamp of the last time this model was updated.
		/// </summary>
		public string UpdateDateTimeString 
		{
			get 
			{ 
				return _updateDateTime.ToShortDateString() + " " + _updateDateTime.ToShortTimeString() ; 
			}
		}
		/// <summary>
		/// Gets or sets the list of model date entries.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of date information for the model.  The key to the entry is the date, the value 
		/// is the information for the date.
		/// </remarks>
		public Hashtable ModelDateEntries 
		{
			get { return _modelDateEntries ; }
			set { _modelDateEntries = value; }
		}
		/// <summary>
		/// Gets or sets a flag indenifying if the model date information needs rebuilt before it can be used.
		/// </summary>
		public bool NeedsRebuilt 
		{
			get { return _needsRebuilt ; }
			set { _needsRebuilt = value; }
		}
		/// <summary>
		/// Gets or sets a flag indenifying if the model contains dynamic date information.
		/// </summary>
		/// <remarks>
		/// If the model contains dynamic date information, it must be rebuilt before it can be used if the 
		/// needs rebuilt flag is set.
		/// </remarks>
		public bool ContainsDynamicDates 
		{
			get { return _containsDynamicDates ; }
			set { _containsDynamicDates = value; }
		}
		/// <summary>
		/// Gets or sets a flag indenifying if the model contains dynamic date information relative to the store 
		/// open dates.
		/// </summary>
		/// <remarks>
		/// If the model contains dynamic date information relative to the store open date,
		/// it must be rebuilt each time before it can be used because a change to the store information
		/// is not known 
		/// </remarks>
		public bool ContainsStoreDynamicDates 
		{
			get { return _containsStoreDynamicDates ; }
			set { _containsStoreDynamicDates = value; }
		}
		/// <summary>
		/// Gets or sets a flag indenifying if the model contains dynamic date information relative to the plan dates. 
		/// </summary>
		/// <remarks>
		/// If the model contains dynamic date information relative to the plan date,
		/// it must be rebuilt each time before it can be used.
		/// </remarks>
		public bool ContainsPlanDynamicDates 
		{
			get { return _containsPlanDynamicDates ; }
			set { _containsPlanDynamicDates = value; }
		}
		/// <summary>
		/// Gets or sets a flag indenifying if the model contains reoccurring date information.
		/// </summary>
		/// <remarks>
		/// If the model contains reoccurring date information, it requires special procesing for the reoccurring 
		/// dates.
		/// </remarks>
		public bool ContainsReoccurringDates 
		{
			get { return _containsReoccurringDates ; }
			set { _containsReoccurringDates = value; }
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Gets or sets a flag which identifies if dates by store have been loaded.
        /// </summary>
        /// <remarks>
        /// If the model does not contain date information by store, this object is not built. 
        /// </remarks>
        public bool ModelDateEntriesLoadedByStore
        {
            get { return _modelDateEntriesLoadedByStore; }
            set { _modelDateEntriesLoadedByStore = value; }
        }
        // End TT#2307
	}

	/// <summary>
	/// Contains the information about the entries in a model
	/// </summary>
	[Serializable()]
	abstract public class ModelEntryInfo
	{
		private DateRangeProfile			_dateRange;
		private int							_modelEntrySeq;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelEntryInfo()
		{
		}

		/// <summary>
		/// Gets or sets the date range information for the model entry.
		/// </summary>
		public DateRangeProfile DateRange 
		{
			get { return _dateRange ; }
			set { _dateRange = value; }
		}
		/// <summary>
		/// Gets or sets the sequence of the model entry.
		/// </summary>
		public int ModelEntrySeq 
		{
			get { return _modelEntrySeq ; }
			set { _modelEntrySeq = value; }
		}
	}

	/// <summary>
	/// Contains the information about an eligibility model
	/// </summary>
	[Serializable()]
	public class EligModelInfo : ModelInfo
	{
		private ArrayList					_salesEligibilityEntries;
		private ArrayList					_priorityShippingEntries;
		private Hashtable					_salesEligibilityModelDateEntries;
		private Hashtable					_priorityShippingModelDateEntries;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private bool _salesEligibilityModelDateEntriesLoadedByStore;
        private bool _salesEligibilityContainsDynamicDates;
        private bool _salesEligibilityContainsStoreDynamicDates;
        private bool _salesEligibilityContainsPlanDynamicDates;
        private bool _salesEligibilityContainsReoccurringDates;
        private bool _salesEligibilityNeedsRebuilt;
        private bool _priorityShippingModelDateEntriesLoadedByStore;
        private bool _priorityShippingContainsDynamicDates;
        private bool _priorityShippingContainsStoreDynamicDates;
        private bool _priorityShippingContainsPlanDynamicDates;
        private bool _priorityShippingContainsReoccurringDates;
        private bool _priorityShippingNeedsRebuilt;
        // End TT#2307
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public EligModelInfo()
		{
			_salesEligibilityEntries = new ArrayList();
			_priorityShippingEntries = new ArrayList();
			_salesEligibilityModelDateEntries = new Hashtable();
			_priorityShippingModelDateEntries = new Hashtable();
            // Begin TT#2307 - JSmith - Incorrect Stock Values
            _salesEligibilityModelDateEntriesLoadedByStore = false;
            _salesEligibilityContainsDynamicDates = false;
            _salesEligibilityContainsStoreDynamicDates = false;
            _salesEligibilityContainsPlanDynamicDates = false;
            _salesEligibilityContainsReoccurringDates = false;
            _salesEligibilityNeedsRebuilt = true;
            _priorityShippingModelDateEntriesLoadedByStore = false;
            _priorityShippingContainsDynamicDates = false;
            _priorityShippingContainsStoreDynamicDates = false;
            _priorityShippingContainsPlanDynamicDates = false;
            _priorityShippingContainsReoccurringDates = false;
            _priorityShippingNeedsRebuilt = true;
            // End TT#2307
		}

		/// <summary>
		/// Gets or sets the list of stock eligibility model entries.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of inforamation for the type of model
		/// </remarks>
		public ArrayList SalesEligibilityEntries 
		{
			get { return _salesEligibilityEntries ; }
			set { _salesEligibilityEntries = value; }
		}

		/// <summary>
		/// Gets or sets the list of priority shipping model entries.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of inforamation for the type of model
		/// </remarks>
		public ArrayList PriorityShippingEntries 
		{
			get { return _priorityShippingEntries ; }
			set { _priorityShippingEntries = value; }
		}
		/// <summary>
		/// Gets or sets the list of model date entries for sales eligibility.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of date information for the model.  The key to the entry is the date, the value 
		/// is the information for the date.
		/// </remarks>
		public Hashtable SalesEligibilityModelDateEntries 
		{
			get { return _salesEligibilityModelDateEntries ; }
			set { _salesEligibilityModelDateEntries = value; }
		}
		/// <summary>
		/// Gets or sets the list of model date entries for priority shipping.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of date information for the model.  The key to the entry is the date, the value 
		/// is the information for the date.
		/// </remarks>
		public Hashtable PriorityShippingModelDateEntries 
		{
			get { return _priorityShippingModelDateEntries ; }
			set {_priorityShippingModelDateEntries = value; }
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
		/// Gets or sets the flag identifying if model date entries for sales eligibility have been loaded by store.
		/// </summary>
		public bool SalesEligibilityModelDateEntriesLoadedByStore 
		{
			get { return _salesEligibilityModelDateEntriesLoadedByStore ; }
			set {_salesEligibilityModelDateEntriesLoadedByStore = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for sales eligibility contain dynamic dates.
		/// </summary>
		public bool SalesEligibilityContainsDynamicDates 
		{
			get { return _salesEligibilityContainsDynamicDates ; }
			set {_salesEligibilityContainsDynamicDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for sales eligibility contain dynamic dates by store.
		/// </summary>
		public bool SalesEligibilityContainsStoreDynamicDates 
		{
			get { return _salesEligibilityContainsStoreDynamicDates ; }
			set {_salesEligibilityContainsStoreDynamicDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for sales eligibility contain dynamic dates by plan.
		/// </summary>
		public bool SalesEligibilityContainsPlanDynamicDates 
		{
			get { return _salesEligibilityContainsPlanDynamicDates ; }
			set {_salesEligibilityContainsPlanDynamicDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for sales eligibility contain reoccurring dates.
		/// </summary>
		public bool SalesEligibilityContainsReoccurringDates 
		{
			get { return _salesEligibilityContainsReoccurringDates ; }
			set {_salesEligibilityContainsReoccurringDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for sales eligibility need rebuild.
		/// </summary>
		public bool SalesEligibilityNeedsRebuilt 
		{
			get { return _salesEligibilityNeedsRebuilt ; }
			set {_salesEligibilityNeedsRebuilt = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for priority shipping have been loaded by store.
		/// </summary>
		public bool PriorityShippingModelDateEntriesLoadedByStore 
		{
			get { return _priorityShippingModelDateEntriesLoadedByStore ; }
			set {_priorityShippingModelDateEntriesLoadedByStore = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for priority shipping contain dynamic dates.
		/// </summary>
		public bool PriorityShippingContainsDynamicDates 
		{
			get { return _priorityShippingContainsDynamicDates ; }
			set {_priorityShippingContainsDynamicDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for priority shipping contain dynamic dates by store.
		/// </summary>
		public bool PriorityShippingContainsStoreDynamicDates 
		{
			get { return _priorityShippingContainsStoreDynamicDates ; }
			set {_priorityShippingContainsStoreDynamicDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for priority shipping contain dynamic dates by plan.
		/// </summary>
		public bool PriorityShippingContainsPlanDynamicDates 
		{
			get { return _priorityShippingContainsPlanDynamicDates ; }
			set {_priorityShippingContainsPlanDynamicDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for priority shipping contain reoccurring dates.
		/// </summary>
		public bool PriorityShippingContainsReoccurringDates 
		{
			get { return _priorityShippingContainsReoccurringDates ; }
			set {_priorityShippingContainsReoccurringDates = value; }
		}

        /// <summary>
		/// Gets or sets the flag identifying if model date entries for priority shipping need rebuild.
		/// </summary>
		public bool PriorityShippingNeedsRebuilt 
		{
			get { return _priorityShippingNeedsRebuilt ; }
			set {_priorityShippingNeedsRebuilt = value; }
		}
        // End TT#2307
	}

	/// <summary>
	/// Contains the information about the entries in an eligibility model
	/// </summary>
	[Serializable()]
	public class EligModelEntryInfo : ModelEntryInfo
	{
		private eEligModelEntryType			_eligModelEntryType;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public EligModelEntryInfo()
		{
		}

		/// <summary>
		/// Gets or sets the type of item for the eligibile model entry.
		/// </summary>
		public eEligModelEntryType EligModelEntryType 
		{
			get { return _eligModelEntryType ; }
			set { _eligModelEntryType = value; }
		}
	}
	
	/// <summary>
	/// Contains the information about the entries in a stock modifier model
	/// </summary>
	[Serializable()]
	public class StkModModelInfo : ModelInfo
	{
		private double				_stkModModelDefault;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StkModModelInfo()
		{
		}

		/// <summary>
		/// Gets or sets the default value of the stock modifier model.
		/// </summary>
		public double StkModModelDefault 
		{
			get { return _stkModModelDefault ; }
			set { _stkModModelDefault = value; }
		}
	}

	/// <summary>
	/// Contains the information about the entries in a stock modifier model entry
	/// </summary>
	[Serializable()]
	public class StkModModelEntryInfo : ModelEntryInfo
	{
		private double				_stkModModelEntryValue;
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StkModModelEntryInfo()
		{
		}
		/// <summary>
		/// Gets or sets the sales modifier entry value of the sales modifier model.
		/// </summary>
		public double StkModModelEntryValue 
		{
			get { return _stkModModelEntryValue ; }
			set { _stkModModelEntryValue = value; }
		}
	}

	/// <summary>
	/// Contains the information about the entries in a sales modifier model
	/// </summary>
	[Serializable()]
	public class SlsModModelInfo : ModelInfo
	{
		private double				_slsModModelDefault;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SlsModModelInfo()
		{
		}

		/// <summary>
		/// Gets or sets the default value of the sales modifier model.
		/// </summary>
		public double SlsModModelDefault 
		{
			get { return _slsModModelDefault ; }
			set { _slsModModelDefault = value; }
		}
	}

	/// <summary>
	/// Contains the information about the entries in a sales modifier model entry
	/// </summary>
	[Serializable()]
	public class SlsModModelEntryInfo : ModelEntryInfo
	{
		private double				_slsModModelEntryValue;
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SlsModModelEntryInfo()
		{
		}
		/// <summary>
		/// Gets or sets the sales modifier entry value of the sales modifier model.
		/// </summary>
		public double SlsModModelEntryValue 
		{
			get { return _slsModModelEntryValue ; }
			set { _slsModModelEntryValue = value; }
		}
		
	}

	/// <summary>
	/// Contains the information about the entries in a FWOS modifier model
	/// </summary>
	[Serializable()]
	public class FWOSModModelInfo : ModelInfo
	{
		private double				_FWOSModModelDefault;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public FWOSModModelInfo()
		{
		}

		/// <summary>
		/// Gets or sets the default value of the FWOS modifier model.
		/// </summary>
		public double FWOSModModelDefault 
		{
			get { return _FWOSModModelDefault ; }
			set { _FWOSModModelDefault = value; }
		}
	}

	/// <summary>
	/// Contains the information about the entries in a FWOS modifier model entry
	/// </summary>
	[Serializable()]
	public class FWOSModModelEntryInfo : ModelEntryInfo
	{
		private double				_FWOSModModelEntryValue;
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public FWOSModModelEntryInfo()
		{
		}
		/// <summary>
		/// Gets or sets the sales modifier entry value of the FWOS modifier model.
		/// </summary>
		public double FWOSModModelEntryValue 
		{
			get { return _FWOSModModelEntryValue ; }
			set { _FWOSModModelEntryValue = value; }
		}
		
	}

    //BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancement
    /// <summary>
    /// Contains the information about the entries in a FWOS max model
    /// </summary>
    [Serializable()]
    public class FWOSMaxModelInfo : ModelInfo
    {
        private double _FWOSMaxModelDefault;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public FWOSMaxModelInfo()
        {
        }

        /// <summary>
        /// Gets or sets the default value of the FWOS max model.
        /// </summary>
        public double FWOSMaxModelDefault
        {
            get { return _FWOSMaxModelDefault; }
            set { _FWOSMaxModelDefault = value; }
        }
    }

    /// <summary>
    /// Contains the information about the entries in a FWOS modifier model entry
    /// </summary>
    [Serializable()]
    public class FWOSMaxModelEntryInfo : ModelEntryInfo
    {
        private double _FWOSMaxModelEntryValue;
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public FWOSMaxModelEntryInfo()
        {
        }
        /// <summary>
        /// Gets or sets the sales modifier entry value of the FWOS modifier model.
        /// </summary>
        public double FWOSMaxModelEntryValue
        {
            get { return _FWOSMaxModelEntryValue; }
            set { _FWOSMaxModelEntryValue = value; }
        }

    }
    //END TT#108 - MD - DOConnell - FWOS Max Model Enahancement

	/// <summary>
	/// Contains the information about the daily percentages for a range of time
	/// Key is calendar date range record ID
	/// </summary>
	[Serializable()]
	public class DailyPercentagesInfo
	{
		// Fields

		private DateRangeProfile	_dateRange;
		private double				_day1;
		private double				_day2;
		private double				_day3;
		private double				_day4;
		private double				_day5;
		private double				_day6;
		private double				_day7;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public DailyPercentagesInfo()
		{
		}

		// Properties

		/// <summary>
		/// Gets or sets the date range profile of a daily percentages information.
		/// </summary>
		public DateRangeProfile	DateRange
		{
			get { return _dateRange ; }
			set { _dateRange = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 1 of the daily percentages for the range of time.
		/// </summary>
		public double Day1 
		{
			get { return _day1 ; }
			set { _day1 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 2 of the daily percentages for the range of time.
		/// </summary>
		public double Day2 
		{
			get { return _day2 ; }
			set { _day2 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 3 of the daily percentages for the range of time.
		/// </summary>
		public double Day3 
		{
			get { return _day3 ; }
			set { _day3 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 4 of the daily percentages for the range of time.
		/// </summary>
		public double Day4 
		{
			get { return _day4 ; }
			set { _day4 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 5 of the daily percentages for the range of time.
		/// </summary>
		public double Day5 
		{
			get { return _day5 ; }
			set { _day5 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 6 of the daily percentages for the range of time.
		/// </summary>
		public double Day6 
		{
			get { return _day6 ; }
			set { _day6 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 7 of the daily percentages for the range of time.
		/// </summary>
		public double Day7 
		{
			get { return _day7 ; }
			set { _day7 = value; }
		}
	}

	/// <summary>
	/// Contains the information about the daily percentages time ranges for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreDailyPercentagesInfo
	{
		// Fields

		private int						_storeRID;
		private bool					_hasDefaultValues;
		private double					_day1Default;
		private double					_day2Default;
		private double					_day3Default;
		private double					_day4Default;
		private double					_day5Default;
		private double					_day6Default;
		private double					_day7Default;
        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
        //private Hashtable				_dailyPercentagesList;
        private SortedList              _dailyPercentagesList;
        // End TT#2621 - JSmith - Duplicate weeks in daily sales

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreDailyPercentagesInfo()
		{
			_hasDefaultValues = false;
			_storeRID = Include.NoRID;
		}

		// Properties
		
		/// <summary>
		/// Gets or sets the store record id.
		/// </summary>
		public int StoreRID 
		{
			get { return _storeRID ; }
			set { _storeRID = value; }
		}
		/// <summary>
		/// Gets or sets the flag that identifies if the node has daily percentages default values.
		/// </summary>
		public bool HasDefaultValues 
		{
			get { return _hasDefaultValues ; }
			set { _hasDefaultValues = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 1 of the daily percentages .
		/// </summary>
		public double Day1Default 
		{
			get { return _day1Default ; }
			set { _day1Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 2 of the daily percentages.
		/// </summary>
		public double Day2Default 
		{
			get { return _day2Default ; }
			set { _day2Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 3 of the daily percentages.
		/// </summary>
		public double Day3Default 
		{
			get { return _day3Default ; }
			set { _day3Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 4 of the daily percentages.
		/// </summary>
		public double Day4Default 
		{
			get { return _day4Default ; }
			set { _day4Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 5 of the daily percentages.
		/// </summary>
		public double Day5Default 
		{
			get { return _day5Default ; }
			set { _day5Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 6 of the daily percentages.
		/// </summary>
		public double Day6Default 
		{
			get { return _day6Default ; }
			set { _day6Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 7 of the daily percentages.
		/// </summary>
		public double Day7Default 
		{
			get { return _day7Default ; }
			set { _day7Default = value; }
		}
		/// <summary>
		/// Gets or sets the list of daily percentages time ranges containing DailyPercentagesInfo objects.
		/// </summary>
        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
        //public Hashtable DailyPercentagesList
        public SortedList DailyPercentagesList
        // End TT#2621 - JSmith - Duplicate weeks in daily sales
		{
			get { return _dailyPercentagesList ; }
			set { _dailyPercentagesList = value; }
		}
	}

	
	/// <summary>
	/// Contains the information about the color codes in the system
	/// </summary>
	[Serializable()]
		public class ColorCodeInfo 
	{
		private int				_colorCodeRID;
		private string			_colorCodeID;
		private string			_colorCodeName;
		private string			_colorCodeGroup;
        private bool _virtualInd;
        private ePurpose _purpose;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ColorCodeInfo()
        {
            _purpose = ePurpose.Default;
        }

		/// <summary>
		/// Gets or sets the record ID of the color.
		/// </summary>
		public int ColorCodeRID 
		{
			get { return _colorCodeRID ; }
			set { _colorCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the ID of the color.
		/// </summary>
		public string ColorCodeID 
		{
			get { return _colorCodeID ; }
			set { _colorCodeID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the name of the color.
		/// </summary>
		public string ColorCodeName 
		{
			get { return _colorCodeName ; }
			set { _colorCodeName = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the group of the color.
		/// </summary>
		public string ColorCodeGroup 
		{
			get { return _colorCodeGroup ; }
			set { _colorCodeGroup = (value == null) ? value : value.Trim(); }
		}
        /// <summary>
        /// Gets or sets the virtualInd of the color.
        /// </summary>
        public bool VirtualInd
        {
            get { return _virtualInd; }
            set { _virtualInd = value; }
        }
        /// <summary>
        /// Gets or sets the purpose of the color.
        /// </summary>
        public ePurpose Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
    }

	/// <summary>
	/// Contains the information about the size codes in the system
	/// </summary>
	[Serializable()]
		public class SizeCodeInfo 
	{
		private int				_sizeCodeRID;
		private string			_sizeCodeID;
		private string			_sizeCodeName;
		private string			_sizeCodePrimary;
		private string			_sizeCodePrimaryUPPER;
		private string			_sizeCodeSecondary;
		private string			_sizeCodeSecondaryUPPER;
		private string			_sizeCodeProductCategory;
		private string			_sizeCodeProductCategoryUPPER;
		private int             _sizeCodePrimaryRID;
		private int             _sizeCodeSecondaryRID;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCodeInfo()
		{
			_sizeCodePrimaryRID = Include.NoRID;
			_sizeCodeSecondaryRID = Include.NoRID;
		}

		/// <summary>
		/// Gets or sets the record ID of the size.
		/// </summary>
		public int SizeCodeRID 
		{
			get { return _sizeCodeRID ; }
			set { _sizeCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the ID of the size.
		/// </summary>
		public string SizeCodeID 
		{
			get { return _sizeCodeID ; }
			set { _sizeCodeID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the name of the size.
		/// </summary>
		public string SizeCodeName 
		{
			get { return _sizeCodeName ; }
			set { _sizeCodeName = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the primary name of the size.
		/// </summary>
		public string SizeCodePrimary 
		{
			get { return _sizeCodePrimary ; }
			set 
			{ 
				_sizeCodePrimary = value;
				_sizeCodePrimaryUPPER = _sizeCodePrimary.Trim().ToUpper(CultureInfo.CurrentUICulture);
			}
		}
		/// <summary>
		/// Gets or sets the primary name of the size.
		/// </summary>
		public string SizeCodePrimaryUPPER 
		{
			get { return _sizeCodePrimaryUPPER ; }
		}
		/// <summary>
		/// Gets or sets the secondary name of the size.
		/// </summary>
		public string SizeCodeSecondary 
		{
			get { return _sizeCodeSecondary ; }
			set 
			{ 
				_sizeCodeSecondary = value; 
				if (_sizeCodeSecondary != null)
				{
					if (_sizeCodeSecondary.Trim().Length == 0)
					{
						_sizeCodeSecondaryUPPER = null;
					}
					else
					{
						_sizeCodeSecondaryUPPER = _sizeCodeSecondary.Trim().ToUpper(CultureInfo.CurrentUICulture);
					}
				}
			}
		}
		public string SizeCodeSecondaryUPPER
		{
			get { return _sizeCodeSecondaryUPPER; }
		}
		/// <summary>
		/// Gets or sets the product category of the size.
		/// </summary>
		public string SizeCodeProductCategory 
		{
			get { return _sizeCodeProductCategory ; }
			set 
			{ 
				_sizeCodeProductCategory = value; 
				_sizeCodeProductCategoryUPPER = _sizeCodeProductCategory.Trim().ToUpper(CultureInfo.CurrentUICulture);
			}
		}
		public string SizeCodeProductCategoryUPPER
		{
			get 
			{
				return _sizeCodeProductCategoryUPPER;
			}
		}

		public int SizeCodePrimaryRID
		{
			get { return _sizeCodePrimaryRID ; }
			set { _sizeCodePrimaryRID = value; }
		}
		public int SizeCodeSecondaryRID
		{
			get { return _sizeCodeSecondaryRID ;}
			set { _sizeCodeSecondaryRID = value;}
		}
	}

	/// <summary>
	/// Contains the information about the product characteristics in the system
	/// </summary>
	[Serializable()]
	public class ProductCharInfo
	{
		private int _productCharRID;
		private string _productCharID;
		private object[] _productCharValues;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ProductCharInfo()
		{
		}

		/// <summary>
		/// Gets or sets the record ID of the product characteristic.
		/// </summary>
		public int ProductCharRID
		{
			get { return _productCharRID; }
			set { _productCharRID = value; }
		}
		/// <summary>
		/// Gets or sets the name of the product characteristic.
		/// </summary>
		public string ProductCharID
		{
			get { return _productCharID; }
			set { _productCharID = value; }
		}
		public object[] ProductCharValues
		{
			get { return _productCharValues; }
			set { _productCharValues = value; }
		}
		public int CharValueCount()
		{
			try
			{
				if (_productCharValues == null)
				{
					return 0;
				}
				else
				{
					return _productCharValues.Length;
				}
			}
			catch
			{
				throw;
			}
		}

		public bool ContainsCharValue(int aCharValueRID)
		{
			try
			{
				if (_productCharValues != null)
				{
					foreach (int charValueRID in _productCharValues)
					{
						if (charValueRID == aCharValueRID)
						{
							return true;
						}
					}
				}
				return false;
			}
			catch
			{
				throw;
			}
		}

		public void AddCharValue(int aCharValueRID)
		{
			try
			{
				object[] objectVector;
				if (_productCharValues == null)
				{
					_productCharValues = new object[1];
				}
				else
				{
					objectVector = new object[_productCharValues.Length + 1];
					System.Array.Copy(_productCharValues, objectVector, _productCharValues.Length);
					_productCharValues = objectVector;
				}
				_productCharValues[_productCharValues.Length - 1] = aCharValueRID;
			}
			catch
			{
				throw;
			}
		}

		public void RemoveCharValue(int aCharValueRID)
		{
			try
			{
				ArrayList objectVector = new ArrayList();
				objectVector.AddRange(_productCharValues);
				objectVector.Remove(aCharValueRID);
				if (objectVector.Count == 0)
				{
					_productCharValues = null;
				}
				else
				{
					_productCharValues = new object[objectVector.Count];
					objectVector.CopyTo(0, _productCharValues, 0, objectVector.Count);
				}
			}
			catch
			{
				throw;
			}
		}

		public void ClearCharValues()
		{
			try
			{
				_productCharValues = null;
			}
			catch
			{
				throw;
			}
		}
	}

	/// <summary>
	/// Contains the information about the product characteristic values in the system
	/// </summary>
	[Serializable()]
	public class ProductCharValueInfo
	{
		private int _productCharValueRID;
		private string _productCharValue;
		private int _productCharRID;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ProductCharValueInfo()
		{
		}

		/// <summary>
		/// Gets or sets the record ID of the product characteristic value.
		/// </summary>
		public int ProductCharValueRID
		{
			get { return _productCharValueRID; }
			set { _productCharValueRID = value; }
		}
		/// <summary>
		/// Gets or sets the value of the product characteristic value.
		/// </summary>
		public string ProductCharValue
		{
			get { return _productCharValue; }
			set { _productCharValue = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the product characteristic.
		/// </summary>
		public int ProductCharRID
		{
			get { return _productCharRID; }
			set { _productCharRID = value; }
		}
	}

    //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
    /// <summary>
    /// Contains the information about the product characteristics in the system
    /// </summary>
    [Serializable()]
    public class ChnSetPctInfo
    {
        private int _storeGroupRID;
        private string _storeGroupID;
        private int _storeGroupLevelRID;
        private string _storeGroupLevelID;
        private object[] _chnSetPctValue;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChnSetPctInfo()
        {
        }

        /// <summary>
        /// Gets or sets the record ID of the product characteristic.
        /// </summary>
        public int storeGroupRID
        {
            get { return _storeGroupRID; }
            set { _storeGroupRID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the product characteristic.
        /// </summary>
        public string storeGroupID
        {
            get { return _storeGroupID; }
            set { _storeGroupID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the product characteristic.
        /// </summary>
        public int storeGroupLevelRID
        {
            get { return _storeGroupLevelRID; }
            set { _storeGroupLevelRID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the product characteristic.
        /// </summary>
        public string storeGroupLevelID
        {
            get { return _storeGroupLevelID; }
            set { _storeGroupLevelID = value; }
        }

        public object[] ChnSetPctValue
        {
            get { return _chnSetPctValue; }
            set { _chnSetPctValue = value; }
        }
        public int ChnSetPctValueCount()
        {
            try
            {
                if (_chnSetPctValue == null)
                {
                    return 0;
                }
                else
                {
                    return _chnSetPctValue.Length;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool ContainsChnSetPctValue(int aChnSetPctValueRID)
        {
            try
            {
                if (_chnSetPctValue != null)
                {
                    foreach (int ChnSetPctValueRID in _chnSetPctValue)
                    {
                        if (ChnSetPctValueRID == aChnSetPctValueRID)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        public void AddChnSetPctValue(int aChnSetPctValueRID)
        {
            try
            {
                object[] objectVector;
                if (_chnSetPctValue == null)
                {
                    _chnSetPctValue = new object[1];
                }
                else
                {
                    objectVector = new object[_chnSetPctValue.Length + 1];
                    System.Array.Copy(_chnSetPctValue, objectVector, _chnSetPctValue.Length);
                    _chnSetPctValue = objectVector;
                }
                _chnSetPctValue[_chnSetPctValue.Length - 1] = aChnSetPctValueRID;
            }
            catch
            {
                throw;
            }
        }

        public void RemoveChnSetPctValue(int aChnSetPctValueRID)
        {
            try
            {
                ArrayList objectVector = new ArrayList();
                objectVector.AddRange(_chnSetPctValue);
                objectVector.Remove(aChnSetPctValueRID);
                if (objectVector.Count == 0)
                {
                    _chnSetPctValue = null;
                }
                else
                {
                    _chnSetPctValue = new object[objectVector.Count];
                    objectVector.CopyTo(0, _chnSetPctValue, 0, objectVector.Count);
                }
            }
            catch
            {
                throw;
            }
        }

        public void ClearChnSetPctValues()
        {
            try
            {
                _chnSetPctValue = null;
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Contains the information about the product characteristic values in the system
    /// </summary>
    [Serializable()]
    public class ChnSetPctValueInfo
    {
        private int _storeGroupLevelValueRID;
        private string _ChnSetPctValue;
        private int _storeGroupLevelRID;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChnSetPctValueInfo()
        {
        }

        /// <summary>
        /// Gets or sets the record ID of the product characteristic value.
        /// </summary>
        public int StoreGroupLevelValueRID
        {
            get { return _storeGroupLevelValueRID; }
            set { _storeGroupLevelValueRID = value; }
        }
        /// <summary>
        /// Gets or sets the value of the product characteristic value.
        /// </summary>
        public string ChnSetPctValue
        {
            get { return _ChnSetPctValue; }
            set { _ChnSetPctValue = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the product characteristic.
        /// </summary>
        public int StoreGroupLevelRID
        {
            get { return _storeGroupLevelRID; }
            set { _storeGroupLevelRID = value; }
        }
    }

    // Begin TT#3573 - JSmith - Improve performance loading color level
    public class ColorNodeInfo
    {
        private int _nodeRID;
        private string _description;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ColorNodeInfo()
        {
        }

        public ColorNodeInfo(int aNodeRID, string aDescription)
        {
            _nodeRID = aNodeRID;
            _description = aDescription;
        }

        /// <summary>
        /// Gets or sets the key of the color node.
        /// </summary>
        public int NodeRID
        {
            get { return _nodeRID; }
            set { _nodeRID = value; }
        }
        /// <summary>
        /// Gets or sets the description of the color node.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
    // End TT#3573 - JSmith - Improve performance loading color level

    //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
}
