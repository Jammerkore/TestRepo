using System;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Diagnostics;

namespace MIDRetail.Business.Allocation 
{
	#region NomineeNodeAllocated
	/// <summary>
	/// Container to track and control the units allocated to an allocation nominee.
	/// </summary>
	/// <remarks>
	/// <para>A nominee is a potential recipient of an allocation.  For example, a store is a nominee for an allocation.  Size is a nominee when trying to breakout a store color allocation by size.</para>
	/// <para>A Node is the source of the units to allocate.  If you want to allocate "units received" to the stores, then the source or description of the "units received" is a node.  When you are trying to breakout a store color allocation by size, the node is the store color allocation.</para>
	/// <para>The NomineeNodeAllocated structure contains the following fields:
	/// <list type="bullet">
	/// <item>IncludeNode: this is an allocation work field used dynamically to include or exclude a node from the allocation process.</item>
	/// <item>RuleExcludesNode: If the chosen rule for this node is not a minimum rule or the user manually changed the allocation for this node, then this node should be excluded from the allocation process.</item>
	/// <item>UnitsAllocated: Tracks the total units allocated to this node.  Must be initialized with units already allocated.</item>
	/// <item>WrkAllocated: A work field used during the allocation process.</item>
	/// <item>Minimum:  The minimum acceptable allocation.  If UnitsAllocated plus desired units allocated is less than the Minimum, then the node will not get the desired UnitsAllocated (ie. UnitsAllocated will remain ASIS.</item>
	/// <item>Maximum:  The maximum acceptable allocation.  If UnitsAllocated is equal to or greater than the Maximum, then no more units will be allocated to this node.</item>
	/// <item>UnitsAvailableIndex: Identifies the location that tracks units available to allocate.</item>
    /// <item>Multiple:  The multiple of the nominee on the associated node.</item>
	/// </list></para>
	/// </remarks>
	internal struct NomineeNodeAllocated
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private bool _includeNode;
		private bool _ruleExcludesNode;
		private int _allocated;
		private int _wrkAllocated;
		private MinMaxAllocationBin _minMax;
        // begin TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max broken
        private int _capacityMax;
        private int _capacityAllocated;
        private int _wrkCapacityAllocated;
        private int _inventoryMin;
        private int _inventoryMax;
        private int _inventoryAllocated;
        private int _groupMemberAlreadyAllocated; // TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
        private int _groupMemberBulkAlreadyAllocated;  // TT#1828 - MD - JSmith - Size Need not allocatde to size
        private int _wrkInventoryAllocated;
        // end TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
		private int _unitsAvailableIndex;  // used by Fill Size Holes
        private int _multiple;             // TT#1478 - Size Multiple Broken
		#endregion Fields

		//=============
		// CONSTRUCTORS
		//=============
		
		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Indicates whether a node is included in the allocation for the nominee. 
		/// </summary>
		internal bool IncludeNode
		{
			get
			{
				return _includeNode;
			}
			set
			{
				_includeNode = value;
			}
		}

		/// <summary>
		/// Indicates whether a rule has excluded this node from the allocation for the nominee.
		/// </summary>
		internal bool RuleExcludedNode
		{
			get
			{
				return _ruleExcludesNode;
			}
			set
			{
				_ruleExcludesNode = value;
			}
		}

		/// <summary>
		/// Units allocated for the nominee in this node.
		/// </summary>
		internal int UnitsAllocated
		{
			get
			{
				return _allocated;
			}
			set
			{
				if (value < 0)
				{	
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg));
				}
				else
				{
					_allocated = value;
				}
			}
		}

		/// <summary>
		/// A work field used during the allocation process to hold the desired allocation.
		/// </summary>
		internal int WrkAllocated
		{
			get
			{
				return _wrkAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg));
				}
				else
				{
					_wrkAllocated = value;
				}
			}
		}
        
        // begin TT#1074 - MD - Jellis - Group Allocaiton - Inventory Min Max Broken
        /// <summary>
        /// A work field used during the allocation process to hold the effects of desired allocation.
        /// </summary>
        internal int WrkInventoryAllocated
        {
            get
            {
                return _wrkInventoryAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg) + " : WrkInventoryAllocated");
                }
                else
                {
                    _wrkInventoryAllocated = value;
                }
            }
        }

        /// <summary>
        /// A work field used during the allocation process to hold the desired allocation.
        /// </summary>
        internal int WrkCapacityAllocated
        {
            get
            {
                return _wrkCapacityAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg) + " : WrkCapacityAllocated");
                }
                else
                {
                    _wrkCapacityAllocated = value;
                }
            }
        }
        // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken

		/// <summary>
		/// Smallest acceptable allocation.  If desired units allocated plus units already allocated is less than the minimum, no additional units are allocated.
		/// </summary>
		internal int Minimum
		{
			get
			{
				return _minMax.Minimum;
			}
			set
			{
				_minMax.SetMinimum(value);
			}
		}

		/// <summary>
		/// Largest acceptable allocation.  Additional units will be allocated upto but not exceeding the maximum allocation.  If units already allocated exceeds or equals the maximum, no additional units will be allocated.
		/// </summary>
		internal int Maximum
		{
			get
			{
				return _minMax.Maximum;
			}
			set
			{
				_minMax.SetMaximum(value);
			}
		}
        // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
        internal int CapacityMaximum
        {
            get
            {
                return _capacityMax;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Capacity cannot be less than zero");
                    // To Do add this message to Enums and message text
                }
                _capacityMax = value;
            }
        }
        internal int CapacityAllocated
        {
            get
            {
                return _capacityAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Capacity Allocated cannot be less than zero");
                    // To Do add this message to Enums and message text
                }
                _capacityAllocated = value;
            }
        }
        internal int InventoryMaximum
        {
            get
            {
                return _inventoryMax;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Inventory Max cannot be less than zero");
                    // To Do add this message to Enums and message text
                }

                _inventoryMax = value;
            }
        }
        internal int InventoryMinimum
        {
            get
            {
                return _inventoryMin;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Inventory Mincannot be less than zero");
                    // To Do add this message to Enums and message text
                }
                _inventoryMin = value;
            }
        }
        internal int InventoryAllocated
        {
            get
            {
                return _inventoryAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Inventory Allocated cannot be less than zero");
                    // To Do add this message to Enums and message text
                }
                _inventoryAllocated = value;
            }
        }
        // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken

        // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
        internal int GroupMemberAlreadyAllocated
        {
            get
            {
                return _groupMemberAlreadyAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("GroupMemberAlreadyAllocated cannot be less than zero");
                }
                _groupMemberAlreadyAllocated = value;
            }
        }
        // end TT#1176 - MD - Jellis - Group Allocation - Size need not observing inv min max

        // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
        internal int GroupMemberBulkAlreadyAllocated
        {
            get
            {
                return _groupMemberBulkAlreadyAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("GroupMemberBulkAlreadyAllocated cannot be less than zero");
                }
                _groupMemberBulkAlreadyAllocated = value;
            }
        }
        // End TT#1828 - MD - JSmith - Size Need not allocatde to size
        
		/// <summary>
		/// This index identifies the location of the "node" field that indicates the available units to allocate
		/// </summary>
		internal int UnitsAvailableIndex
		{
			get
			{
				return _unitsAvailableIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new Exception("Units Available Index cannot be less than zero");
					// To Do add this message to Enums and message text
				}
				_unitsAvailableIndex = value;
			}
		}
        // begin TT#1478 - Size Multiple Broken
        /// <summary>
        /// Multiple for the Nominiee on the associated node.
        /// </summary>
        internal int Multiple
        {
            get
            {
                return _multiple;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Multiple cannot be less than zero");
                }
                _multiple = value;
            }
        }
        // end TT#1478 - Size Multiple Broken
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#endregion Methods
	}
	#endregion NomineeNodeAllocated

	#region Node Component
	/// <summary>
	/// A node component describes the source of the units to allocate and tracks the units allocated in the node.
	/// Nominees within the node are the potential recipients of the units allocated in the node. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// A node component is a hierarchical structure that describes units to allocate and identifies nominees as
	/// potential recipients of any units allocated.
	/// </para><para>
	/// The top node in the hierarchy describes the total units to allocate, any units already allocated to the
	/// designated nominees, and any other constraints the allocation process must observe for the node during the
	/// allocation process.  The top node is always allocated based on the needs of the nominees.
	/// </para><para>
	/// The top node may be divided into subnodes that further describe the units to allocate. The subnodes may or
	/// may not be allocated.  A subnode may be marked for allocation only when its immediate parent node is also
	/// marked for allocation.
	/// </para><para>
	/// When a subnode is marked for allocation, it is allocated proportionally to a nominee from units allocated 
	/// to the nominee in the immediate parent node of the subnode. The proportions are based on the units remaining
	/// to allocate in each subnode having the same immediate parent node (assumption: the units to allocate in all 
	/// the subnodes balance to the units to allocate in the immediate parent node).
	/// </para><para>
	/// Subnodes that are not marked for allocation are used to determine the units available to a nominee.  The units
	/// remaining to allocate in these subnodes and whether the subnode has been excluded by rule or maximum allocation
	/// constraints are used in the calculation of the units available to a nominee.
	/// </para><para>
	/// The following fields are defined in NodeComponent:
	/// <list type="bullet">
	/// <item>UnitsToAllocate: This is the total units to allocate in the node.</item>
	/// <item>UnitsAllocated: This is the total units allocated in the node.</item>
	/// <item>NodeMultiple:  This is the "unit" of allocation in the node.  The quantity allocated must be divisible by this multiple.</item>
	/// <item>NodeType: Identifies the node type: Assortment, Style Total, MIDGeneric Type, Detail Type, MIDGeneric Pack, NonMIDGeneric Pack, Bulk, Bulk Color, Bulk Color-Size.</item>
	/// <item>AllocateSubNode: This is a bool flag. True indicates that during the allocation process if a nominee is allocated any units in this node, then those units must be spread proportionally (allocated) to the next lower level of subnodes. It is always assumed that the top node is allocated by need; all other nodes, if allocated, are spreads from the top.</item>
	/// <item>NomineeNodeAllocated: This is an array of the nominees (a list of all potential recipients) for units allocated.  The list contains constraints for each nominee in the node and tracks any units allocated to the nominee.</item>
	/// <item>ParentNodeCOmponent: This is the parent node for this this node in the node-component hierarchy (null indicates no parent.</item>
	/// <item>SubnodeComponent: This is an array of subnodes to this node in the node-component hierarchy. Units to allocate in this node are assumed to equal the sum of the subnode units to allocate.</item>
	/// </list></para>
	/// </remarks>
	internal class NodeComponent
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private HdrAllocationBin _nodeToAllocate;
        //private eAllocationNode _nodeType;   // TT#488 - MD - JEllis Urban Group Allocation 
        private eNeedAllocationNode _nodeType; // TT#488 - MD - JEllis Urban Group Allocation
		private object _nodeID;
		private bool _allocateSubNodes;
		private bool _candidateNode;
		private int _candidateUnits;
		private int[] _unitsAvailable;  // used in Fill Size Holes
		private NodeComponent _parentNode; // MID Track 3120 Min/Max Constraints not observed
		private NomineeNodeAllocated[] _nomineeNodeAllocated;
		private NodeComponent[] _subNodeComponent;
        // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
        private List<NodeComponent> _capacityAllocationNode;
        private List<NodeComponent> _inventoryAllocationNode;
        // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
        private string _nodeDescription;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class
		/// </summary>
		internal NodeComponent()
		{
			// BEGIN MID Track 3120 Min/max constraints not observed
			InitializeComponent(null);
		}
		private NodeComponent(NodeComponent aParentNode)
		{
			InitializeComponent(aParentNode);
		}
		private void InitializeComponent(NodeComponent aParentNode)
		{
			// END MID Track 3120
			_nodeToAllocate = new HdrAllocationBin();
			_nodeToAllocate.IsChanged = false;
			_nodeToAllocate.IsNew = false;
			_nodeToAllocate.SetQtyAllocated(0);
			_nodeToAllocate.SetQtyToAllocate(0);
			_nodeToAllocate.Sequence = 0;
			_nodeToAllocate.SetFilterQtyAllocated(0);
			_nodeToAllocate.SetUnitMultiple(1);
            //_nodeType = eAllocationNode.None;  // TT#488 - MD - JEllis - Urban Group Allocation
            _nodeType = eNeedAllocationNode.None; // TT#488 - MD - Jellis - Urban Group Allocation
			_allocateSubNodes = false;
			_candidateNode = false;
			_candidateUnits = 0;
			_nomineeNodeAllocated = null;
			_subNodeComponent = null;
			_unitsAvailable = new int[1];
			_unitsAvailable[0] = int.MaxValue;
			_parentNode = aParentNode; // MID Track 3120 Min/Max constraints not observed 
            // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
            _capacityAllocationNode = new List<NodeComponent>();
            _inventoryAllocationNode = new List<NodeComponent>();
            // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
		}
		#endregion Constructors
        
		#region Properties
		//===========
		// PROPERTIES
		//===========
		// BEGIN MID Track 3120 Min/Max Consraints not followed when Need performed on single component
		/// <summary>
		/// Indicates whether this is the top node. True: this is the top node; False: this is not the top node.
		/// </summary>
		internal bool IsTheTopNode
		{
			get
			{
				return (ParentNodeComponent == null);
			}
		}
		/// <summary>
	    /// Gets the parent node of this node
	    /// </summary>
		internal NodeComponent ParentNodeComponent
		{
			get
			{
				return _parentNode;
			}
		}
		// END MID Track 3120 Min/Max Consraints not followed when Need performed on single component
		/// <summary>
		/// Gets the array of children nodes for this node(the subnodes on the next allocation level).
		/// </summary>
		internal NodeComponent[] SubNodeComponents
		{
			get
			{
				return _subNodeComponent;
			}
		}
		/// <summary>
		/// Total units to allocate for this node.
		/// </summary>
		internal int NodeUnitsToAllocate
		{
			get
			{
				return _nodeToAllocate.QtyToAllocate;
			}
			set
			{
				_nodeToAllocate.SetQtyToAllocate(value);
			}
		}

		/// <summary>
		/// Total units allocated to nominees for this node.
		/// </summary>
		internal int NodeUnitsAllocated
		{
			get
			{
				return _nodeToAllocate.QtyAllocated;
			}
			set
			{
				_nodeToAllocate.SetQtyAllocated(value);
			}
		}

		/// <summary>
		/// Indicates whether this node has at least one candidate nominee with IncludeNode equal to true.
		/// </summary>
		internal bool CandidateNode
		{
			get
			{
				return _candidateNode;
			}
			set
			{
				_candidateNode = value;
			}
		}

		/// <summary>
		/// Gets or sets total available units to allocate for all included subnodes.
		/// </summary>
		internal int CandidateUnits
		{
			get
			{
				return _candidateUnits;
			}
			set 
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyToAllocateCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyToAllocateCannotBeNeg));
				}
				else
				{
					_candidateUnits = value;
				}
			}
		}

		/// <summary>
		/// Gets the remaining units to allocate for this node.
		/// </summary>
		public int NodeRemainingToAllocate
		{
			get
			{
				if (NodeUnitsAllocated >= NodeUnitsToAllocate)
				{
					return 0;
				}
				else
				{
					return NodeUnitsToAllocate - NodeUnitsAllocated;
				}
			}
		}

		/// <summary>
		/// The allocation multiple for this node.  All "new" units allocated by "need" in this node are divisible by this multiple. 
		/// </summary>
		internal int NodeMultiple
		{
			get
			{
				return _nodeToAllocate.UnitMultiple;
			}
			set
			{
				_nodeToAllocate.SetUnitMultiple(value);
			}
		}

		/// <summary>
		/// Number of nominees.  The number of nominees must be the same across all nodes.
		/// </summary>
		internal int NomineeDimension
		{
			get
			{
				if (_nomineeNodeAllocated == null)
				{
					return 0;
				}
				else
				{
					return _nomineeNodeAllocated.Length;
				}
			}
		}

		/// <summary>
		/// Gets or sets AllocateSubNodes.
		/// </summary>
		/// <remarks>
		/// True indicates that subnodes on next level will be allocated proportionally.  False indicates that no allocation of the subnodes will occur.</remarks>
		public bool AllocateSubNodes
		{
			get 
			{
				return _allocateSubNodes;
			}
			set
			{
				_allocateSubNodes = value;
			}
		}
        
		/// <summary>
		/// Indicates which allocation node this node represents.
		/// </summary>
        //public eAllocationNode NodeType    // TT#488 - MD - JEllis - Urban Group Allocation
        public eNeedAllocationNode NodeType  // TT#488 - MD - JEllis - Urban Group Allocation
		{
			get
			{
				return _nodeType;
			}
			set
			{
				_nodeType = value;
			}
		}

		/// <summary>
		/// Gets or sets the node ID
		/// </summary>
		public object NodeID
		{
			get
			{
				return _nodeID;
			}
			set
			{
				_nodeID = value;
			}
		}

        /// <summary>
        /// Gets or sets the node Description
        /// </summary>
        public string NodeDescription
        {
            get
            {
                return _nodeDescription;
            }
            set
            {
                _nodeDescription = value;
            }
        }

		/// <summary>
		/// Gets the number of subnodes for this node.
		/// </summary>
		internal int SubnodeDimension
		{
			get
			{
				if (_subNodeComponent == null)
				{
					return 0;
				}
				else
				{
					return _subNodeComponent.Length;
				}
			}
		}
		/// <summary>
		/// Gets or sets the Available Units for this node
		/// </summary>
		/// <remarks>
		/// Available units are an additional constraint on a need allocation.  The constraint may vary by nominee.
		/// Fill Size Holes uses this array to identify and track the units available for each store-width-size nominee.
		/// When the value goes to zero or less, a store-width-size nominee with that constraint cannot get anymore allocation. 
		/// </remarks>
		public int[] UnitsAvailable
		{
			get
			{
				return _unitsAvailable;
			}
			set
			{
				_unitsAvailable = value;

			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		/// <summary>
		/// Sets the subnode dimension
		/// </summary>
		/// <param name="aDimension">Dimension value.</param>
		public void SetSubNodeDimension (int aDimension)
		{
			if (aDimension > 0)
			{
				_subNodeComponent = new NodeComponent[aDimension];
				for (int i = 0; i < aDimension; i++)
				{
					_subNodeComponent[i] = new NodeComponent(this);
					_subNodeComponent[i].SetNomineeDimension(this.NomineeDimension);
//					_subNodeComponent[i].NodeID = null;
//					_subNodeComponent[i].AllocateSubNodes = false;
//					_subNodeComponent[i].NodeUnitsAllocated = 0;
//					_subNodeComponent[i].NodeUnitsToAllocate = 0;
//					_subNodeComponent[i].NodeMultiple = 1;
//					_subNodeComponent[i].SetNomineeDimension(this.NomineeDimension);
				}
			}
			else
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_DimMustBeGrThan0,
					MIDText.GetText(eMIDTextCode.msg_DimMustBeGrThan0));
			}
		}

        // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
        /// <summary>
        /// Adds a target node to the capacity allocation node list; this list identifies the nodes whose allocation affects the targets capacity.
        /// </summary>
        /// <param name="aNodeComponent">Node Component where allocation of current node affects capacity decisions</param>
        public void AddCapacityAllocationNode(NodeComponent aNodeComponent)
        {
            if (!_capacityAllocationNode.Exists(n => n == aNodeComponent))
            {
                _capacityAllocationNode.Add(aNodeComponent);
                // begin TT#1175 - MD - Jellis -Group Allocation - Running Need Twice does not observ Inv Min Max
                for (int nomineeIndex = 0; nomineeIndex < _nomineeNodeAllocated.Length; nomineeIndex++)
                {
                    aNodeComponent.SetNomineeCapacityAllocation(nomineeIndex, GetNomineeUnitsAllocated(nomineeIndex));
                }
                // end TT#1175 - MD - Jellis - Group Allocation - Running Need twice does not observe Inve Min Max
            }
        }
        /// <summary>
        /// Gets a Nominee's capacity maximum value (adjusted by other relevant nodes' allocations)
        /// </summary>
        /// <param name="aNomineeIndex"></param>
        /// <returns></returns>
        public int GetNomineeCapacityMaximum(int aNomineeIndex)
        {
            return Math.Max
                (0, 
                 _nomineeNodeAllocated[aNomineeIndex].CapacityMaximum 
                 - GetNomineeCapacityAllocation(aNomineeIndex)
                 - GetNomineeWrkCapacityAllocation(aNomineeIndex));
        }
        /// <summary>
        /// Gets the total allocation from all other nodes that affects this node's capacity decisions
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index that identifies location of the Nominee</param>
        /// <returns></returns>
        public int GetNomineeCapacityAllocation(int aNomineeIndex)
        {
            return _nomineeNodeAllocated[aNomineeIndex].CapacityAllocated;
        }
        /// <summary>
        /// Sets the total allocation from all other nodes that affects this node's capacity decisions
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index identifying location of the Nominee</param>
        /// <param name="aQtyAllocated">Quantity allocation on other nodes that affects this node's capacity decisions</param>
        public void SetNomineeCapacityAllocation(int aNomineeIndex, int aQtyAllocated)
        {
            _nomineeNodeAllocated[aNomineeIndex].CapacityAllocated = aQtyAllocated;
        }
        /// <summary>
        /// Gets Nominee Pending Capacity allocation total from other nodes.
        /// </summary>
        /// <param name="aNomineeIndex"></param>
        /// <returns></returns>
        public int GetNomineeWrkCapacityAllocation(int aNomineeIndex)
        {
           return _nomineeNodeAllocated[aNomineeIndex].WrkCapacityAllocated;
        }
        /// <summary>
        /// Sets the total pending allocation from all other nodes that affects this node's capacity decisions
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index identifying location of the Nominee</param>
        /// <param name="aQtyAllocated">Pending quantity allocation on other nodes that affects this node's capacity decisions</param>
        public void SetNomineeWrkCapacityAllocation(int aNomineeIndex, int aQtyAllocated)
        {
            _nomineeNodeAllocated[aNomineeIndex].WrkCapacityAllocated = aQtyAllocated;
        }
        /// <summary>
        /// Adds a target node to the inventory allocation node list; this list identifies the nodes whose allocation affects the target's inventory minimum/maximum decisions. 
        /// </summary>
        /// <param name="aNodeComponent">Node component that affects inventory decisions on this node</param>
        public void AddInventoryAllocationNode(NodeComponent aNodeComponent)
        {
            if (!_inventoryAllocationNode.Exists(n => n == aNodeComponent))
            {
                _inventoryAllocationNode.Add(aNodeComponent);
                // begin TT#1175 - MD - Jellis -Group Allocation - Running Need Twice does not observ Inv Min Max
                for (int nomineeIndex = 0; nomineeIndex < _nomineeNodeAllocated.Length; nomineeIndex++)
                {
                    aNodeComponent.SetNomineeInventoryAllocation(nomineeIndex, GetNomineeUnitsAllocated(nomineeIndex));
                }
                // end TT#1175 - MD - Jellis - Group Allocation - Running Need twice does not observe Inve Min Max
            }
        }
        /// <summary>
        /// Gets Nominee's Inventory Maximum
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index identifies the location of the nominee</param>
        /// <returns></returns>
        public int GetNomineeInventoryMaximum(int aNomineeIndex)
        {
            return Math.Max
                (0,
                 _nomineeNodeAllocated[aNomineeIndex].InventoryMaximum 
                 - GetNomineeInventoryAllocation(aNomineeIndex)
                 - GetNomineeWrkInventoryAllocation(aNomineeIndex));
        }
        /// <summary>
        /// Gets Nominee Inventory Minimum
        /// </summary>
        /// <param name="aNomineeIndex">Nomine Index identifies location of the nominee</param>
        /// <returns></returns>
        public int GetNomineeInventoryMinimum(int aNomineeIndex)
        {
            return Math.Max
                (0,
                _nomineeNodeAllocated[aNomineeIndex].InventoryMinimum
                - GetNomineeInventoryAllocation(aNomineeIndex)
                - GetNomineeWrkInventoryAllocation(aNomineeIndex));
        }
        /// <summary>
        /// Gets the Nominee's allocation total from other nodes that affects this node's inventory decisions
        /// </summary>
        /// <param name="aNomineeIndex"></param>
        /// <returns></returns>
        public int GetNomineeInventoryAllocation(int aNomineeIndex)
        {
            return _nomineeNodeAllocated[aNomineeIndex].InventoryAllocated;
        }
        /// <summary>
        /// Sets Nominee's total allocation from other nodes that affects this node's inventory decisions
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index identifies location of nominee</param>
        /// <param name="aQtyAllocated">Total Qty Allocated on other nodes that affect this node's inventory decisions</param>
        public void SetNomineeInventoryAllocation(int aNomineeIndex, int aQtyAllocated)
        {
            _nomineeNodeAllocated[aNomineeIndex].InventoryAllocated = aQtyAllocated;
        }
        /// <summary>
        /// Get Nominee's pending allocation from other nodes that affect this node's inventory decisions
        /// </summary>
        /// <param name="aNomineeIndex"></param>
        /// <returns></returns>
        public int GetNomineeWrkInventoryAllocation(int aNomineeIndex)
        {
            return _nomineeNodeAllocated[aNomineeIndex].WrkInventoryAllocated;
        }
        /// <summary>
        /// Sets Nominee's pending allocations from other nodes that affect this node's inventory decisions
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index identifies location of the nominee</param>
        /// <param name="aQtyAllocated">Pending allocation quantity from other nodes that affect this node's inventory decisions</param>
        public void SetNomineeWrkInventoryAllocation(int aNomineeIndex, int aQtyAllocated)
        {
            _nomineeNodeAllocated[aNomineeIndex].WrkInventoryAllocated = aQtyAllocated;
        }
        // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken


		/// <summary>
		/// Sets Nominee Dimension.
		/// </summary>
		/// <param name="aDimension">Dimension value.</param>
		public void SetNomineeDimension (int aDimension)
		{
			if (aDimension > 0)
			{
				_nomineeNodeAllocated = new NomineeNodeAllocated[aDimension];
				for (int i = 0; i < aDimension; i++)
				{
					_nomineeNodeAllocated[i].IncludeNode = false;
					_nomineeNodeAllocated[i].WrkAllocated = 0;
					_nomineeNodeAllocated[i].Maximum = int.MaxValue;
					_nomineeNodeAllocated[i].Minimum = 0;
					_nomineeNodeAllocated[i].RuleExcludedNode = false;
					_nomineeNodeAllocated[i].UnitsAllocated = 0;
					_nomineeNodeAllocated[i].UnitsAvailableIndex = 0;
                    _nomineeNodeAllocated[i].Multiple = 0; // TT#1478 - Size Multiple Broken
                    // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    _nomineeNodeAllocated[i].CapacityMaximum = int.MaxValue;
                    _nomineeNodeAllocated[i].CapacityAllocated = 0;
                    _nomineeNodeAllocated[i].InventoryMaximum = int.MaxValue;
                    _nomineeNodeAllocated[i].InventoryMinimum = 0;
                    _nomineeNodeAllocated[i].InventoryAllocated = 0;
                    // end TT31074 - MD - Jellis - Group Alloction - Inventory Min Max Broken
				}
			}
			else
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_DimMustBeGrThan0,
					MIDText.GetText(eMIDTextCode.msg_DimMustBeGrThan0));
			}
		}

		/// <summary>
		/// Gets the specified Subnode Component.
		/// </summary>
		/// <param name="aNodeComponent">Identifies the "top" node of the component structure from which the subnode component is to be retrieved.</param>
		/// <param name="aSubNodeIndex">An array of subnode indices that identify the path within the component hierarchy to the desired subnode. Item 0 in this array identifies the 1st level subnode within the "top" node, item 1 identifies the level 2 subnode within the level 1 subnode, and so on down to the level containing the desired subnode.</param>
		/// <returns>The node component for the requested subnode</returns>
		public NodeComponent GetSubNodeComponent (NodeComponent aNodeComponent, int[] aSubNodeIndex)
		{
			NodeComponent n = aNodeComponent;
			foreach (int s in aSubNodeIndex)
			{
				n = GetChildNodeComponent (n, s);
			}
			return n;
		}

		/// <summary>
		/// Gets the specified child node component within the specified parent node. 
		/// </summary>
		/// <param name="aParentNodeComponent">Parent node.</param>
		/// <param name="aChildNodeIndex">Index of the child node.</param>
		/// <returns></returns>
		public NodeComponent GetChildNodeComponent (NodeComponent aParentNodeComponent, int aChildNodeIndex)
		{
			NodeComponent n = aParentNodeComponent;
			if (aChildNodeIndex < n.SubnodeDimension)
			{
				n = n._subNodeComponent[aChildNodeIndex];
			}
			else
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_IndexOutOfRange,
					MIDText.GetText(eMIDTextCode.msg_IndexOutOfRange));
			}
			return n; 
		}

		/// <summary>
		/// Adds a subordinate component to a node structure.
		/// </summary>
		/// <param name="aSubNodeIndex">An array of subnode indices that identify the path within the component hierarchy to the desired subnode. Item 0 in this array identifies the 1st level subnode within the "top" node, item 1 identifies the level 2 subnode within the level 1 subnode, and so on down to the level containing the desired subnode.</param>
		/// <param name="aSubLevelDimension">The dimension of the next level of subnodes below this subnode.</param>
		/// <param name="aNodeType">Allocation Node Type.</param>
		/// <param name="aNodeID">Id of the node (pack name, color RID, colorRID/sizeRID, etc.)</param>
		/// <param name="aUnitsToAllocate">Total units to allocate for this subnode.</param>
		/// <param name="aAllocateSubNodes">True indicates that the next level of subnodes are to be allocated proportionally based on units remaining to allocate in each subnode.</param>
		/// <param name="aUnitsAllocated">Total units allocated to the nominees for this subnode.</param>
		/// <param name="aMultiple">Allocation multiple for this subnode.</param>
        // begin TT#488 - MD - JEllis - Urban Group Allocation
        public void AddSubordinateNode(int[] aSubNodeIndex, int aSubLevelDimension, eNeedAllocationNode aNodeType, object aNodeID,
            int aUnitsToAllocate, bool aAllocateSubNodes, int aUnitsAllocated,
            int aMultiple)
        //public void AddSubordinateNode (int[] aSubNodeIndex, int aSubLevelDimension, eAllocationNode aNodeType, object aNodeID,
        //    int aUnitsToAllocate, bool aAllocateSubNodes, int aUnitsAllocated,
        //    int aMultiple)
            // end TT#488 - MD - JEllis - Urban Group Allocation
		{
			NodeComponent n = this;
			n = GetSubNodeComponent(n, aSubNodeIndex);
			n.NodeUnitsToAllocate = aUnitsToAllocate;
			n.NodeUnitsAllocated = aUnitsAllocated;
			n.NodeMultiple = aMultiple;
			n.NodeType = aNodeType;
			n.CandidateNode = false;
			n.CandidateUnits = 0;
			n.AllocateSubNodes = aAllocateSubNodes;
			n.SetSubNodeDimension(aSubLevelDimension);
			n.SetNomineeDimension(this.NomineeDimension);
		}

		/// <summary>
		/// Gets the specified nominee's units allocated within a node. 
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Units allocated to the specified nominee in the node.</returns>
		public int GetNomineeUnitsAllocated (int aNomineeIndex)
		{
			return _nomineeNodeAllocated[aNomineeIndex].UnitsAllocated;
		}

		/// <summary>
		/// Sets the specified nominee's units allocated within a node.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aUnitsAllocated">Units allocated.</param>
		public void SetNomineeUnitsAllocated (int aNomineeIndex, int aUnitsAllocated)
		{
            // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min max broken
            int difference =
                aUnitsAllocated
                - _nomineeNodeAllocated[aNomineeIndex].UnitsAllocated;
            foreach (NodeComponent n in _inventoryAllocationNode)
            {
                n.SetNomineeInventoryAllocation(aNomineeIndex, n.GetNomineeInventoryAllocation(aNomineeIndex) + difference);
            }
            foreach (NodeComponent n in _capacityAllocationNode)
            {
                n.SetNomineeCapacityAllocation(aNomineeIndex, n.GetNomineeCapacityAllocation(aNomineeIndex) + difference);
            }
            // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
			_nomineeNodeAllocated[aNomineeIndex].UnitsAllocated = aUnitsAllocated;
		}
		
        // begin TT#1176 - MD - Group Allocation - Size need not observing inv min max
        /// <summary>
        /// Gets the nominee's units already allocated by other group members
        /// </summary>
        /// <param name="aNomineeIndex">Index that identifies the nominee</param>
        /// <returns>aUnitsAllocated</returns>
        public int GetNomineeGroupUnitsAlreadyAllocated(int aNomineeIndex)
        {
            return _nomineeNodeAllocated[aNomineeIndex].GroupMemberAlreadyAllocated;
        }
        /// <summary>
        /// Sets the nominee's units already allocated by other group members
        /// </summary>
        /// <param name="aNomineeIndex">Index that identifies the nominee</param>
        /// <param name="aUnitsAllocated">Units Allocated</param>
        public void SetNomineeGroupUnitsAlreadyAllocated(int aNomineeIndex, int aUnitsAllocated)
        {
            _nomineeNodeAllocated[aNomineeIndex].GroupMemberAlreadyAllocated = aUnitsAllocated;
        }
        // end TT#1176 - MD - Group Allocation - Size Need not observing inv min max

        // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
        /// <summary>
        /// Gets the nominee's bulk units already allocated by other group members
        /// </summary>
        /// <param name="aNomineeIndex">Index that identifies the nominee</param>
        /// <returns>aUnitsAllocated</returns>
        public int GetNomineeGroupBulkUnitsAlreadyAllocated(int aNomineeIndex)
        {
            return _nomineeNodeAllocated[aNomineeIndex].GroupMemberBulkAlreadyAllocated;
        }
        /// <summary>
        /// Sets the nominee's bulk units already allocated by other group members
        /// </summary>
        /// <param name="aNomineeIndex">Index that identifies the nominee</param>
        /// <param name="aUnitsAllocated">Units Allocated</param>
        public void SetNomineeGroupBulkUnitsAlreadyAllocated(int aNomineeIndex, int aUnitsAllocated)
        {
            _nomineeNodeAllocated[aNomineeIndex].GroupMemberBulkAlreadyAllocated = aUnitsAllocated;
        }
		// End TT#1828 - MD - JSmith - Size Need not allocatde to size

		/// <summary>
		/// Gets the specified nominee's WrkUnitsAllocated (a work field for the allocation process.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>WrkUnitsAllocated, the desired allocation.</returns>
		public int GetNomineeWrkUnitsAllocated (int aNomineeIndex)
		{
			return _nomineeNodeAllocated[aNomineeIndex].WrkAllocated;
		}

		/// <summary>
		/// Sets the specified nominee's WrkUnitsAllocated (a work field for the allocation process.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aWrkUnitsAllocated">WrkUnitsAllocated value.</param>
		public void SetNomineeWrkUnitsAllocated (int aNomineeIndex, int aWrkUnitsAllocated)
		{
            // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min max broken
            int difference =
                aWrkUnitsAllocated
                - _nomineeNodeAllocated[aNomineeIndex].WrkAllocated;
            foreach (NodeComponent n in _inventoryAllocationNode)
            {
                n.SetNomineeWrkInventoryAllocation(aNomineeIndex, n.GetNomineeWrkInventoryAllocation(aNomineeIndex) + difference);
            }
            foreach (NodeComponent n in _capacityAllocationNode)
            {
                n.SetNomineeWrkCapacityAllocation(aNomineeIndex, n.GetNomineeWrkCapacityAllocation(aNomineeIndex) + difference);
            }
            // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
			_nomineeNodeAllocated[aNomineeIndex].WrkAllocated = aWrkUnitsAllocated;
		}

		/// <summary>
		/// Gets Nominee's Desired Allocation(including any units already allocated).
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Total desired allocation for the specified nominee.</returns>
		public int GetNomineeDesiredAllocation (int aNomineeIndex)
		{
			return GetNomineeUnitsAllocated (aNomineeIndex) + GetNomineeWrkUnitsAllocated(aNomineeIndex);
		}

		/// <summary>
		/// Gets the specified nominee's minimum allocation for the node.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Minimum allocation for the specified nominee in the node.</returns>
		public int GetNomineeMinimum (int aNomineeIndex)
		{
            // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
            return
                Math.Max(
                    _nomineeNodeAllocated[aNomineeIndex].Minimum,
                    GetNomineeInventoryMinimum(aNomineeIndex));
            //return _nomineeNodeAllocated[aNomineeIndex].Minimum;
            // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
		}

		/// <summary>
		/// Sets the specified nominee's minimum allocation for the node.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aMinimum">Minimum allocation value.</param>
		public void SetNomineeMinimum (int aNomineeIndex, int aMinimum)
		{
			_nomineeNodeAllocated[aNomineeIndex].Minimum = aMinimum;
		}

		/// <summary>
		/// Gets the specified nominee's maximum allocation for the node.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Maximum allocaiton for the specified nominee in the node.</returns>
		public int GetNomineeMaximum (int aNomineeIndex)
		{
            // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
            return
                Math.Min(
                    _nomineeNodeAllocated[aNomineeIndex].Maximum,
                    Math.Min(GetNomineeCapacityMaximum(aNomineeIndex),
                             GetNomineeInventoryMaximum(aNomineeIndex)));
            //return _nomineeNodeAllocated[aNomineeIndex].Maximum;
            // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
		}

		/// <summary>
		/// Sets the specified nominee's maximum allocation for the node.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aMaximum">Maximum allocation value.</param>
		public void SetNomineeMaximum (int aNomineeIndex, int aMaximum)
		{
			_nomineeNodeAllocated[aNomineeIndex].Maximum = aMaximum;
		}

        // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
        public void SetNomineeCapacityMaximum(int aNomineeIndex, int aCapacityMaximum)
        {
            _nomineeNodeAllocated[aNomineeIndex].CapacityMaximum = aCapacityMaximum;
        }
        public void SetNomineeInventoryMaximum(int aNomineeIndex, int aInventoryMaximum)
        {
            _nomineeNodeAllocated[aNomineeIndex].InventoryMaximum = aInventoryMaximum;
        }
        public void SetNomineeInventoryMinimum(int aNomineeIndex, int aInventoryMinimum)
        {
            _nomineeNodeAllocated[aNomineeIndex].InventoryMinimum = aInventoryMinimum;
        }
        // end TT#1074 - MD - Jellis -Group ALlocation Inventory Min Max Broken

		/// <summary>
		/// Gets the specified nominee's include node flag value.
		/// </summary>
		/// <remarks>
		/// The include node flag is used by the need allocation process.  It is controlled exclusively by the process. It changes value throughout the process.
		/// </remarks>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Include Node Flag value. True indicates the node is "included" in the current action of the process.</returns>
		internal bool GetNomineeIncludeNode (int aNomineeIndex)
		{
			return _nomineeNodeAllocated[aNomineeIndex].IncludeNode;
		}

		/// <summary>
		/// Sets the specified nominee's include node flag value.
		/// </summary>
		/// <remarks>
		/// The include node flag is used by the need allocation process.  It is controlled exclusively by the process.  It changes value throughout the process.
		/// </remarks>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aFlagValue">Flag value. True indicates the node is included for this nominee for the current action of the process.</param>
		public void SetNomineeIncludeNode (int aNomineeIndex, bool aFlagValue)
		{
			_nomineeNodeAllocated[aNomineeIndex].IncludeNode = aFlagValue;
		}

		/// <summary>
		/// Gets the specified nominee's rule excluded node flag.
		/// </summary>
		/// <remarks>
		/// True indicates that this node is excluded from all need process actions. No units will be allocated to this nominee in this node when this flag is true.
		/// </remarks>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>True when the node is excluded from the need allocation; false otherwise.</returns>
		public bool GetNomineeRuleExcludedNode (int aNomineeIndex)
		{
			return _nomineeNodeAllocated[aNomineeIndex].RuleExcludedNode;
		}

		/// <summary>
		/// Sets the specified nominee's rule excluded node flag.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aFlagValue">Flag value. True indicates the node is excluded from the need allocation.</param>
		public void SetNomineeRuleExcludedNode (int aNomineeIndex, bool aFlagValue)
		{
			_nomineeNodeAllocated[aNomineeIndex].RuleExcludedNode = aFlagValue;
		}
		/// <summary>
		/// Sets the nominee's units available index (location of additional constraints on the available units to allocate to this nominee)
		/// </summary>
		/// <param name="aNomineeIndex">Index of Nominee</param>
		/// <param name="aUnitsAvailableIndex">Index Value where the units available constraint for this nominee is located</param>
		public void SetNomineeUnitsAvaialbleIndex(int aNomineeIndex, int aUnitsAvailableIndex)
		{
			_nomineeNodeAllocated[aNomineeIndex].UnitsAvailableIndex = aUnitsAvailableIndex;
		}
		/// <summary>
		/// Gets the nominee's units available to allocate
		/// </summary>
		/// <param name="aNomineeIndex">Index of the nominee</param>
		/// <returns>Units available for allocation to this nominee</returns>
		internal int GetNomineeUnitsAvailable (int aNomineeIndex)
		{
			return this._unitsAvailable[_nomineeNodeAllocated[aNomineeIndex].UnitsAvailableIndex];
		}
		/// <summary>
		/// Sets the nominee's units available to allocate
		/// </summary>
		/// <param name="aNomineeIndex">Index of the nominee</param>
		/// <param name="aUnitsAvailable">Units available to allocate</param>
		internal void SetNomineeUnitsAvailable(int aNomineeIndex, int aUnitsAvailable)
		{
			int unitsAvailable = aUnitsAvailable;
			if (aUnitsAvailable < 0)
			{
				unitsAvailable = 0;
			}
			this._unitsAvailable[_nomineeNodeAllocated[aNomineeIndex].UnitsAvailableIndex] = unitsAvailable;
		}
		// begin TT#1478 - Size Multiple Broken
        /// <summary>
        /// Gets the multiple for the specified nominee on the associated node
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index</param>
        /// <returns>Multiple</returns>
        internal int GetNomineeMultiple(int aNomineeIndex)
        {
            int multiple = _nomineeNodeAllocated[aNomineeIndex].Multiple;
            if (multiple < 1)
            {
                multiple = NodeMultiple;
            }
            return multiple;
        }
        /// <summary>
        /// Sets the multiple for the specified nominee on the associated node
        /// </summary>
        /// <param name="aNomineeIndex">Nominee Index</param>
        /// <param name="aMultiple">Multiple</param>
        internal void SetNomineeMultiple(int aNomineeIndex, int aMultiple)
        {
            this._nomineeNodeAllocated[aNomineeIndex].Multiple = aMultiple;
        }
        // end TT#1478 - Size Multiple Broken
		#endregion Methods
	}
	#endregion Node Component

	#region Nominee Basis Plan
	/// <summary>
	/// This is the nominee's basis plan, ownership and other criteria for a need allocation.
	/// </summary>
	/// <remarks><para>
	/// A nominee is a potential recipient of units allocated during an allocation process. This structure
	/// describes the necessary information to calculate the nominee's need as well as describing other pertinent
	/// constraints and criteria for the nominee.  This structure exists once for a nominee at the top node of the
	/// component structure for an allocation.  In addition, this structure contains workspace for the need allocation
	/// process.
	/// </para><para>
	/// The following briefly describes the fields:
	/// <list type="bullet">
	/// <item>HasAllocationPriority: Bool that identifies a nominee as an Allocation Priority. When True this nominee must be given preference in allocation regardles of NEED.  Two nominees with the same priviledge are considered in need order.</item>
	/// <item>IsAllocationCandidate: Bool that dynamically tracks when this nominee is a candidate for an allocation.  This is a work field that changes frequently during the process.</item>
	/// <item>IsOut: Bool that is set by the calling routine to force a nominee out during the need process (usually set in response to a user-specified rule).</item>
	/// <item>UnitPlan: The total plan for the nominee for the "need" horizon.</item>
	/// <item>OnHand: The beginning inventory for the nominee for the "need" horizon.</item>
	/// <item>InTransit: The total intransit for the "need" horizon.</item>
	/// <item>UnitNeed: The unit need for the nominee. This is a work field that changes frequently during the need process as units are allocated to the nominee.</item>
	/// <item>PercentNeed: the percent need for the nominee.  This is a work field that changes frequently during the need process are unit need changes.</item>
	/// </list></para>
	/// </remarks>
	internal struct NomineeAllocationBasis
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private bool _hasAllocationPriority;
		private bool _isAllocationCandidate;
		private bool _isOut;
		private double _plan;
		private double _onHand;
		private int _intransit;
		private double _unitNeed;
		private double _percentNeed;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// True indicates Nominee has allocation priority regardless of need (such as new stores); False indicates Nominee has no priority special priviledges.
		/// </summary>
		internal bool HasAllocationPriority
		{
			get
			{
				return _hasAllocationPriority;
			}
			set
			{
				_hasAllocationPriority = value;
			}
		}

        /// <summary>
        /// Indicates whether a nominee is an allocation candidate.
        /// </summary>
        /// <remarks>
        /// Used by the allocate by need process to identify nominees that may receive an allocation. Flag values may change several times during the process.
        /// </remarks>
		internal bool IsAllocationCandidate
		{
			get
			{
				return _isAllocationCandidate;
			}
			set
			{
				_isAllocationCandidate = value;
			}
		}

		/// <summary>
		/// Indicates whether a nominee is "permanently" excluded from the allocation process.
		/// </summary>
		/// <remarks>
		/// Flag value is set by the calling module.
		/// </remarks>
		internal bool IsOut
		{
			get
			{
				return _isOut;
			}
			set
			{
				_isOut = value;
			}
		}

		/// <summary>
		/// The total plan over the need horizon for the nominee.
		/// </summary>
		internal double TotalPlan
		{
			get
			{
				return _plan;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PlanCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_PlanCannotBeNeg));;
				}
				else
				{
					_plan = value;
				}
			}
		}

		/// <summary>
		/// The total onhand at the beginning of the need horizon for the nominee.
		/// </summary>
		internal double OnHand
		{
			get
			{
				return _onHand;
			}
			set
			{
				if (value < 0)
				{
					_onHand = 0;
				}
				else
				{
					_onHand = value;
				}
			}
		}

		/// <summary>
		/// Total intransit across the entire need horizon for the nominee.
		/// </summary>
		internal int InTransit
		{
			get
			{
				return _intransit;
			}
			set
			{
				if (value < 0)
				{
					_intransit = 0;
				}
				else
				{
					_intransit = value;
				}
			}
		}

        /// <summary>
        /// Unit Need for the nominee in the need horizon.
        /// </summary>
		internal double UnitNeed
		{
			get
			{
				return _unitNeed;
			}
			set
			{
				_unitNeed = value;
			}
		}

		/// <summary>
		/// Percent need for the nominee in the need horizon.
		/// </summary>
		internal double PercentNeed
		{
			get
			{
				return _percentNeed;
			}
			set
			{
				_percentNeed = value;
			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#endregion Methods
	}
	#endregion Nominee Basis Plan

	#region Need Algorithms
	/// <summary>
	/// NeedAlgorithms is a set of algorithms that allocate merchandise based on need.
	/// </summary>
	/// <remarks>
	/// The basic algorithms:
	/// <list type="bullet">
	/// <item>EqualizePercentNeed: Iteratively allocates sufficient units to nominees to achieve an equalization of percent needs.</item>
	/// <item>GiveMinOrUpToMax: Iteratively allocates either the minimum or one multiple up to the maximum to the nominee with the greatest need.</item>
	/// <item>ExceedMaximums: Iteratively allocates one multiple to the nominee with an allocation and with the greatest need.</item>
	/// </list>
	/// </remarks>
	public class NeedAlgorithms
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private ApplicationSessionTransaction _transaction;
		private double _percentNeedLimit;
		private int[] _unitNeedLimit; // MID Track 5525 AnF Defect 1618: Round Off Error
		private int _candidateCount;
		//private double _allocationRoundingFactor; // MID Track 4278 Percent Need Limit not observed // MID Track 5525 AnF Defect 1618: Round Off Error
		private NodeComponent _nodeComponent;
		private NomineeAllocationBasis[] _nomineeBasis;
		private bool _unitsAllocatedInPass;
		private bool _thereAreCandidates;
		private bool _allCandidatesAccepted;
		private double _candidatePlan;
		private double _candidateOnHand;
		private int _candidateInTransit;
		private int _candidateUnitsAllocated;
		private double _objectiveNeed;
		private double _objectivePercentNeed;
		private int _allocateValue;
		//private double _pctNeedThresh; // MID Track 5525 AnF Defect 1618: Round Off Error
		private bool _workUpBuy;
		private eWorkUpBuyAllocationType _workUpBuyAllocationType;
        private bool _observeMultiples;
		#endregion Fields

		#region Constructor
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates a new instance of NeedAlgorithms.
		/// </summary>
		/// <param name="aNumberOfNominees">Number of potential recipients of any units allocated. (eg. number of stores)</param>
		public NeedAlgorithms(ApplicationSessionTransaction aTransaction, int aNumberOfNominees, string aDescription)
		{
			_transaction = aTransaction;
			//_percentNeedLimit = int.MinValue;  // MID Track 5525 Defect ID 1618: Round Error when applying % NEED LIMIT
			_percentNeedLimit = Include.DefaultPercentNeedLimit;  // MID Track 5525 Defext ID 1618: Round Error when applying % Need Limit
			_candidateCount = 0;
			_nodeComponent = new NodeComponent();
			_nodeComponent.NodeMultiple = 1;
			_nodeComponent.NodeUnitsAllocated = 0;
			_nodeComponent.NodeUnitsToAllocate = 0;
			_nodeComponent.CandidateNode = false;
			_nodeComponent.CandidateUnits = 0;
			_nomineeBasis = new NomineeAllocationBasis[aNumberOfNominees];
			_nodeComponent.SetNomineeDimension(aNumberOfNominees);
			_unitNeedLimit = new int[aNumberOfNominees]; // MID Track 5525 Defect ID 1618: Round Error when applying % Need Limitt
            _nodeComponent.NodeDescription = aDescription;
		}            
		#endregion Constructor

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		///Gets or sets the smallest objective percent need.  A nominee with this percent need will not get any addition units allocated.
		/// </summary>
		public double PercentNeedLimit
		{
			get
			{
				return _percentNeedLimit;
			}
			set
			{
				_percentNeedLimit = value;
			}
		}

		/// <summary>
		/// Gets or sets total Units to Allocate for the node.
		/// </summary>
		public int UnitsToAllocate
		{
			get
			{
				return _nodeComponent.NodeUnitsToAllocate;
			}
			set
			{
				_nodeComponent.NodeUnitsToAllocate = value;
			}
		}

		/// <summary>
		/// Gets or sets total units allocated across all nominees.
		/// </summary>
		public int UnitsAllocated
		{
			get
			{
				return _nodeComponent.NodeUnitsAllocated;
			}
			set
			{
				_nodeComponent.NodeUnitsAllocated = value;
			}
		}

		public int RemainingUnitsToAllocate
		{
			get
			{
				if (UnitsAllocated >= UnitsToAllocate)
				{
					return 0;
				}
				else
				{
					return UnitsToAllocate - UnitsAllocated;
				}
			}
		}

		/// <summary>
		/// Gets or sets multiple for the node.
		/// </summary>
		public int Multiple
		{
			get
			{
				return _nodeComponent.NodeMultiple;
			}
			set
			{
				_nodeComponent.NodeMultiple = value;
			}
		}

		/// <summary>
		/// UnitsAllocatedInPass is a bool value used to control the primary loop in Need Allocation.
		/// </summary>
		/// <remarks>
		/// True indicates units were allocated during the process, so the process may continue provided
		/// there are units remaining to be allocated. 
		/// False indicates no units were allocated during the last process, so need processing should stop.
		/// </remarks>
		internal bool UnitsAllocatedInPass
		{
			get
			{
				return _unitsAllocatedInPass;
			}
			set
			{
				_unitsAllocatedInPass = value;
			}
		}

		/// <summary>
		/// ThereAreCandidates is a bool value in Need Allocation 
		/// </summary>
		/// <remarks>
		/// True indicates candidates were selected for need allocation.  False indicates there are no
		/// candidates for need allocation.
		/// </remarks>
		internal bool ThereAreCandidates
		{
			get
			{
				return _thereAreCandidates;
			}
			set
			{
				_thereAreCandidates = value;
			}
		}

		/// <summary>
		/// AllCandidatesAccepted is a bool value in Need Allocation.
		/// </summary>
		/// <remarks>
		/// True indicates that no candidate has achieved its objective %Need, so sufficient units may 
		/// be allocated to each candidate to bring its %need to the objective.  
		/// False indicates that at least one candidate has already achieved its objective %Need, so 
		/// it was eliminated as a candidate in this pass and the identification of candidates must continue.
		/// </remarks>
		internal bool AllCandidatesAccepted
		{
			get
			{
				return _allCandidatesAccepted;
			}
			set
			{
				_allCandidatesAccepted = value;
			}
		}

		/// <summary>
		/// CandidateUnitPlan is the total plan across all candidates.
		/// </summary>
		internal double CandidatePlan
		{
			get
			{
				return _candidatePlan;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PlanCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_PlanCannotBeNeg));;
				}
				else
				{
					_candidatePlan = value;
				}
			}
		}
		
		/// <summary>
		/// CandidateOnHand is the total beginning inventory onhand for the need horizon across all candidates.
		/// </summary>
		internal double CandidateOnHand
		{
			get
			{
				return _candidateOnHand;
			}
			set
			{
				if (value < 0)
				{
					_candidateOnHand = 0;
				}
				else
				{
					_candidateOnHand = value;
				}
			}
		}

		/// <summary>
		/// CandidateInTransit is the total intransit in the need horizon across all candidates
		/// </summary>
		public int CandidateInTransit
		{
			get
			{
				return _candidateInTransit;
			}
			set
			{
				if (value >= 0)
				{
					_candidateInTransit = value;
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_InTransitCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_InTransitCannotBeNeg));
				}
			}
		}

		/// <summary>
		/// CandidateUnitsAllocated is the total units allocated across all candidates.
		/// </summary>
		public int CandidateUnitsAllocated
		{
			get 
			{
				return _candidateUnitsAllocated;
			}
			set 
			{
				_candidateUnitsAllocated = value;
			}
		}

		/// <summary>
		/// ObjectiveNeed is the unit need for the average candidate.
		/// </summary>
		/// <remarks>
		/// ObjectiveNeed = CandidateUnitPlan - CandidateUnitsOwned - UnitsAllocated - UnitsRemainingToAllocate
		/// </remarks>
		internal double ObjectiveNeed
		{
			get 
			{
				return _objectiveNeed;
			}
			set
			{
				_objectiveNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets ObjectivePercentNeed, the desired percent need for each candidate.
		/// </summary>
		/// <remarks>
		/// ObjectivePercentNeed = ObjectiveNeed * 100 / CandidatePlan
		/// </remarks>
		internal double ObjectivePercentNeed
		{
			get
			{
				return _objectivePercentNeed;
			}
			set 
			{
				_objectivePercentNeed = value;
			}
		}

		/// <summary>
		/// Gets nominee dimension.
		/// </summary>
		internal int NomineeDimension
		{
			get
			{
				return _nomineeBasis.Length;
			}
		}

		/// <summary>
		/// Gets or sets Node Type.
		/// </summary>
        internal eNeedAllocationNode NodeType // TT#488 - MD - JEllis - Urban Group Allocation
        //internal eAllocationNode NodeType   // TT#488 - MD - JEllis - Urban Group Allocation
		{
			get
			{
				return _nodeComponent.NodeType;
			}
			set
			{
				_nodeComponent.NodeType = value;
			}
		}
		/// <summary>
		/// Gets or sets the Node Available Units constraint
		/// </summary>
		public int[] UnitsAvailableConstraint
		{
			get
			{
				return _nodeComponent.UnitsAvailable;
			}
			set
			{
				_nodeComponent.UnitsAvailable = value;
			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========

		#region Nominee Criteria 
		#region NomineeIsCandidate
		/// <summary>
		/// Gets the specified nominee's IsCandidate flag value.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>True: indicates the nominee is a candidate for allocation. False: indicates the nominee is not a candidate for allocation.</returns>
		public bool GetNomineeIsCandidate (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].IsAllocationCandidate;
		}

		/// <summary>
		/// Sets the specified nominee's IsCandidate flag value.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aFlagValue">Flag value.</param>
		public void SetNomineeIsCandidate (int aNomineeIndex, bool aFlagValue)
		{
			_nomineeBasis[aNomineeIndex].IsAllocationCandidate = aFlagValue;
		}
		#endregion NomineeIsCandidate

		#region NomineeHasAllocationPriority
		/// <summary>
		/// Gets the specified nominee's HasAllocationPriority flag value
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>True: indicates the Nominee has Allocation Priority; False: indicates the nominee does not have Allocation Priority</returns>
		public bool GetNomineeHasAllocationPriority (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].HasAllocationPriority;
		}

		/// <summary>
		/// Sets the specified nominee's HasAllocationPriority flag value
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee</param>
		/// <param name="aFlagValue">True: indicates the Nominee has Allocation Priority; False: indicates the nominee does not have Allocation Priority</param>
		public void SetNomineeHasAllocationPriority (int aNomineeIndex, bool aFlagValue)
		{
			_nomineeBasis[aNomineeIndex].HasAllocationPriority = aFlagValue;
		}
		#endregion NomineeHasAllocationPriority

		#region NomineeIsOut
		/// <summary>
		/// Gets the nominee IsOut flag value.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>True: indicates the nominee cannot be a candidate for allocation. False: indicates the nominee may be a candidate for allocation.</returns>
		public bool GetNomineeIsOut (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].IsOut;
		}

		/// <summary>
		/// Sets nominee's IsOut flag value.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aFlagValue">Flag value.</param>
		public void SetNomineeIsOut (int aNomineeIndex, bool aFlagValue)
		{
			_nomineeBasis[aNomineeIndex].IsOut = aFlagValue;
		}
		#endregion NomineeIsOut

		#region Nominee Plan
		/// <summary>
		/// Gets nominee's plan.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Plan value for the specified nominee.</returns>
		public double GetNomineePlan (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].TotalPlan;
		}

		/// <summary>
		/// Sets the nominee's plan.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aPlan">Plan value.</param>
		public void SetNomineePlan (int aNomineeIndex, double aPlan)
		{
			_nomineeBasis[aNomineeIndex].TotalPlan = aPlan;
		}
		#endregion Nominee Plan

		#region Nominee OnHand
		/// <summary>
		/// Gets the nominee's onhand.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Onhand value for the specified nominee.</returns>
		public double GetNomineeOnHand (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].OnHand;
		}

		/// <summary>
		/// Sets the nominee's onhand.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aOnHand">Onhand value for the specified nominee.</param>
		public void SetNomineeOnHand (int aNomineeIndex, double aOnHand)
		{
			_nomineeBasis[aNomineeIndex].OnHand = aOnHand;
		}
		#endregion Nominee OnHand

		#region Nominee InTransit
		/// <summary>
		/// Gets the nominee's intransit.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Intransit value for the specified nominee.</returns>
		public int GetNomineeInTransit (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].InTransit;
		}

		/// <summary>
		/// Sets the nominee's intransit.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aInTransit">Intransit value for the specified nominee.</param>
		public void SetNomineeInTransit (int aNomineeIndex, int aInTransit)
		{
			_nomineeBasis[aNomineeIndex].InTransit = aInTransit;
		}
		#endregion Nominee InTransit

		#region Nominee Unit Need
		/// <summary>
		/// Gets the nominee's unit need.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Unit need value for the specified nominee.</returns>
		public double GetNomineeUnitNeed (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].UnitNeed;
		}

		/// <summary>
		/// Sets the nominee's unit need.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aUnitNeed">Unit Need value for the specified nominee.</param>
		public void SetNomineeUnitNeed (int aNomineeIndex, double aUnitNeed)
		{
			_nomineeBasis[aNomineeIndex].UnitNeed = aUnitNeed;
		}
		#endregion Nominee Unit Need

		// begin MID Track 5525 AnF Defect 1618: Rounding Error
		#region Nominee Unit Need Limit
		/// <summary>
		/// Gets the nominee's unit need Limit.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Unit need value for the specified nominee.</returns>
		public int GetNomineeUnitNeedLimit (int aNomineeIndex)
		{
			return _unitNeedLimit[aNomineeIndex];
		}
		#endregion Nominee Unit Need Limit
		// end MID Track 5525 AnF Defect 1618: Rounding Error

		#region Nominee Percent Need
		/// <summary>
		/// Gets the nominee's percent need.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Percent need value for the specified nominee.</returns>
		public double GetNomineePercentNeed (int aNomineeIndex)
		{
			return _nomineeBasis[aNomineeIndex].PercentNeed;
		}

		/// <summary>
		/// Sets the nominee's percent need.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aPercentNeed">Percent need value for the specified nominee.</param>
		public void SetNomineePercentNeed (int aNomineeIndex, double aPercentNeed)
		{
			_nomineeBasis[aNomineeIndex].PercentNeed = aPercentNeed;
		}
		#endregion Nominee Percent Need

		#region Nominee Units Allocated
		/// <summary>
		/// Gets nominee's units allocated.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Units Allocated to this nominee in the node.</returns>
		public int GetNomineeUnitsAllocated (int aNomineeIndex)
		{
			return _nodeComponent.GetNomineeUnitsAllocated(aNomineeIndex);
	    }

		/// <summary>
		/// Sets nominee's units allocated.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aUnitsAllocated">Units allocated value.</param>
		public void SetNomineeUnitsAllocated (int aNomineeIndex, int aUnitsAllocated)
		{
			_nodeComponent.SetNomineeUnitsAllocated (aNomineeIndex, aUnitsAllocated);
		}

        // begin TT#1176 - MD - Jellis- Group Allocation Size Need not observing inv min max
        /// <summary>
        /// Gets nominee's units allocated by other members of a group allocation
        /// </summary>
        /// <param name="aNomineeIndex">Index that identifies the nominee</param>
        /// <returns>aUnitsAllocated</returns>
        public int GetNomineeGroupUnitsAlreadyAllocated(int aNomineeIndex)
        {
            return _nodeComponent.GetNomineeGroupUnitsAlreadyAllocated(aNomineeIndex);
        }
        public void SetNomineeGroupUnitsAlreadyAllocated(int aNomineeIndex, int aUnitsAllocated)
        {
            _nodeComponent.SetNomineeGroupUnitsAlreadyAllocated(aNomineeIndex, aUnitsAllocated);
        }
        // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
        /// <summary>
        /// Gets nominee's bulk units allocated by other members of a group allocation
        /// </summary>
        /// <param name="aNomineeIndex">Index that identifies the nominee</param>
        /// <returns>aUnitsAllocated</returns>
        public int GetNomineeGroupBulkUnitsAlreadyAllocated(int aNomineeIndex)
        {
            return _nodeComponent.GetNomineeGroupBulkUnitsAlreadyAllocated(aNomineeIndex);
        }
        public void SetNomineeGroupBulkUnitsAlreadyAllocated(int aNomineeIndex, int aUnitsAllocated)
        {
            _nodeComponent.SetNomineeGroupBulkUnitsAlreadyAllocated(aNomineeIndex, aUnitsAllocated);
        }
        // End TT#1828 - MD - JSmith - Size Need not allocatde to size

		/// <summary>
		/// Gets nominee's work units allocated.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Nominee's work units allocated.</returns>
		/// <remarks>
		/// Work units allocated is a holding field used by the need allocation process to calulate additional 
		/// desired units allocated.  The calculation consists of several steps.  The steps include: calculating
		/// enough units to bring the nominee to the objective, comparing and adjusting the calculation according
		/// to minimums and maximums, and spreading the result to included subcomponents for the nominee (the
		/// spread may change the results due to observance of multiples, minimums and maximums for each subnode).
		/// </remarks>
		private int GetNomineeWrkUnitsAllocated (int aNomineeIndex)
		{
			return _nodeComponent.GetNomineeWrkUnitsAllocated(aNomineeIndex);
		}

		/// <summary>
		/// Sets nominee's work units allocated.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aWrkUnitsAllocated">Work Units allocated value.</param>
		/// <remarks>
		/// Work units allocated is a holding field used by the need allocation process to calulate additional 
		/// desired units allocated.  The calculation consists of several steps.  The steps include: calculating
		/// enough units to bring the nominee to the objective, comparing and adjusting the calculation according
		/// to minimums and maximums, and spreading the result to included subcomponents for the nominee (the
		/// spread may change the results due to observance of multiples, minimums and maximums for each subnode).
		/// </remarks>
		private void SetNomineeWrkUnitsAllocated (int aNomineeIndex, int aWrkUnitsAllocated)
		{
			_nodeComponent.SetNomineeWrkUnitsAllocated(aNomineeIndex, aWrkUnitsAllocated);
		}

		private int GetNomineeUnitsAvailable (int aNomineeIndex)
		{
			return _nodeComponent.GetNomineeUnitsAvailable(aNomineeIndex);
		}
		/// <summary>
		/// Gets nominee's total desired allocation (including units already allocated).
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Total desired allocation for this nominee in this node.</returns>
		private int NomineeTotalDesiredAllocation (int aNomineeIndex)
		{
			return (GetNomineeUnitsAllocated(aNomineeIndex) + GetNomineeWrkUnitsAllocated(aNomineeIndex));
		}
		#endregion Nominee Units Allocated

		#region Nominee Maximum
		/// <summary>
		/// Gets nominee's maximum allocation.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Nominee's maximum allocation.</returns>
		internal int GetNomineeMaximum (int aNomineeIndex)
		{
            return _nodeComponent.GetNomineeMaximum(aNomineeIndex);
		}

		/// <summary>
		/// Sets nominee's maximum allocation.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aMaximum">A maximum value.</param>
		internal void SetNomineeMaximum (int aNomineeIndex, int aMaximum)
		{
			_nodeComponent.SetNomineeMaximum(aNomineeIndex, aMaximum);
		}
		#endregion Nominee Maximum

		#region Nominee Minimum
		/// <summary>
		/// Gets nominee's minimum allocation.
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Nominee's minimum allocation.</returns>
		internal int GetNomineeMinimum (int aNomineeIndex)
		{
			return _nodeComponent.GetNomineeMinimum(aNomineeIndex);
		}

		/// <summary>
		/// Sets nominee's minimum allocation/
		/// </summary>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <param name="aMinimum">A minimum value.</param>
		internal void SetNomineeMinimum (int aNomineeIndex, int aMinimum)
		{
			_nodeComponent.SetNomineeMinimum(aNomineeIndex, aMinimum);
		}
		#endregion Nominee Minimum

        // begin TT#1478 - Size Multiple Broken
        #region Nominee Multiple
        private int GetNomineeMultiple (int aNomineeIndex)
        {
            int multiple = _nodeComponent.GetNomineeMultiple(aNomineeIndex);
            if (multiple == 0)
            {
                multiple = Multiple;
            }
            return multiple;
        }
        #endregion Nominee Multiple
        // end TT#1478 - Size Multiple Broken

		#endregion Nominee Criteria

		#region Nodes
		#region Add Subordinate Node
		/// <summary>
		/// Adds a subordinate node to the allocation node hierarchy.
		/// </summary>
		/// <param name="aSubNodeIndex">An array of subnode indices that identifies the path to the subnode being added.  Item 0 in this array identifies the subnode in the 1st level below the top node; item 1 identifies the subnode in the 2nd level; and so on down to the level being added.</param>
		/// <param name="aSubLevelDimension">Dimension of the subnodes subordinate to the level being added.</param>
		/// <param name="aUnitsToAllocate">Total Units to Allocate for the subnode being added.</param>
		/// <param name="aAllocateSubNodes">True indicates that subnodes subordinate to the subnode being added are to be allocated proportionally based on the subordinate subnode's units remaining to allocate.  False: indicates the subnodes subordinate to the subnode being added are for information purposes only.</param>
		/// <param name="aUnitsAllocated">Total Units Allocated for the subnode being added.</param>
		/// <param name="aMultiple">Allocation multiple for the subnode being added.</param>
		public void AddSubordinateNode (
			int[] aSubNodeIndex, 
			int aSubLevelDimension,
            //eAllocationNode aAllocationNode,   // TT#488 - MD - JEllis - Group Allocation
            eNeedAllocationNode aAllocationNode, // TT#488 - MD - JEllis - Group Allocation
			object aNodeID,
			int aUnitsToAllocate, 
			bool aAllocateSubNodes,
			int aUnitsAllocated, 
			int aMultiple)
		{
			_nodeComponent.AddSubordinateNode(
				aSubNodeIndex, 
				aSubLevelDimension, 
				aAllocationNode,
				aNodeID,
				aUnitsToAllocate, 
				aAllocateSubNodes, 
				aUnitsAllocated, 
				aMultiple);
		}
		#endregion Add Subordinate Node

		#region Node Component
		/// <summary>
		/// Gets Node Component
		/// </summary>
		internal NodeComponent GetNodeComponent()
		{
			return _nodeComponent;
		}
		#endregion NodeComponent
		#endregion Nodes

		#region Need Allocation Algorithms
		#region Allocate By Need Main
        /// <summary>
        /// Allocates units to each nominee based on need
        /// </summary>
        /// <param name="aAllocatePriorityNominees">True: Gives priority nominees first choice; False: Priority nominees are not given first choice</param>
        /// <param name="aGiveNeedUptoMax">True: Gives every nominee what it needs up to its maximum;</param>
        /// <param name="aEqualizeNeeds">Equalizes the needs of the nominees</param>
        /// <param name="aGiveMinUptoMax">Gives the minimum or upto the maximum</param>
        /// <param name="aGiveOverMax">Gives over the maximum</param>
        /// <param name="aWorkUpBuy">Identifies a work up buy. True: this is a work up buy; False: this is not a work up buy</param>
        public void AllocateByNeed(
            bool aAllocatePriorityNominees,
            bool aGiveNeedUptoMax, // MID Track 3786 Change Fill Size Holes Algorithm // MID Track 3810 Size Allocation GT Style Allocation
            bool aEqualizeNeeds,
            bool aGiveMinUptoMax,
            bool aGiveOverMax,
            eWorkUpBuyAllocationType aWorkUpBuy)  // MID Track 3810 Size Allocation GT Style Allocation 
        {
            AllocateByNeed(aAllocatePriorityNominees, aGiveNeedUptoMax, aEqualizeNeeds, aGiveMinUptoMax, aGiveOverMax, aWorkUpBuy, false);
        }
        /// <summary>
		/// Allocates units to each nominee based on need
		/// </summary>
		/// <param name="aAllocatePriorityNominees">True: Gives priority nominees first choice; False: Priority nominees are not given first choice</param>
		/// <param name="aGiveNeedUptoMax">True: Gives every nominee what it needs up to its maximum;</param>
		/// <param name="aEqualizeNeeds">Equalizes the needs of the nominees</param>
		/// <param name="aGiveMinUptoMax">Gives the minimum or upto the maximum</param>
		/// <param name="aGiveOverMax">Gives over the maximum</param>
		/// <param name="aWorkUpBuy">Identifies a work up buy. True: this is a work up buy; False: this is not a work up buy</param>
        /// <param name="aObserveMultiples">True:  all multiples oberserved; False: Multiples observed until last moment when multiple may be broken to get all units allocated</param>
        public void AllocateByNeed(
            bool aAllocatePriorityNominees,
            bool aGiveNeedUptoMax, // MID Track 3786 Change Fill Size Holes Algorithm // MID Track 3810 Size Allocation GT Style Allocation
            bool aEqualizeNeeds,
            bool aGiveMinUptoMax,
            bool aGiveOverMax,
            eWorkUpBuyAllocationType aWorkUpBuy,  // MID Track 3810 Size Allocation GT Style Allocation  // TT#1478 - Size Multiple Broken
            bool aObserveMultiples)               // TT#1478 - Size Multiple Broken 
        {
            // begin MID Track 3810 Size Allocation GT Style Allocation
            _workUpBuyAllocationType = aWorkUpBuy;
            _observeMultiples = aObserveMultiples; // TT#1478 - Size multiple broken
            if (aWorkUpBuy == eWorkUpBuyAllocationType.NotWorkUpAllocationBuy)
            {
                _workUpBuy = false;
            }
            else
            {
                _workUpBuy = true;
            }
            // end MID Track 3810 Size Allocation GT Style Allocation
            int nomineePlan; // MID Track 5525 AnF Defect 1618: Rounding Error when applying % Need Limit
            for (int NomineeIndex = 0; NomineeIndex < NomineeDimension; NomineeIndex++)
            {
                // begin MID Track 5525 AnF Defect 1618: Rounding error when applying %Need Limit
                //SetNomineeUnitNeed(NomineeIndex, 
                //	Need.UnitNeed(GetNomineePlan(NomineeIndex),GetNomineeOnHand(NomineeIndex),GetNomineeInTransit(NomineeIndex),GetNomineeUnitsAllocated(NomineeIndex)));
                //SetNomineePercentNeed(NomineeIndex, Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex), GetNomineePlan(NomineeIndex)));
                nomineePlan = (int)GetNomineePlan(NomineeIndex);
				
				// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                double OH = GetNomineeOnHand(NomineeIndex);
                int IT = GetNomineeInTransit(NomineeIndex);
                int group = GetNomineeGroupUnitsAlreadyAllocated(NomineeIndex);
                int groupBulk = GetNomineeGroupBulkUnitsAlreadyAllocated(NomineeIndex);  // TT#1828 - MD - JSmith - Size Need not allocatde to size
                int unitsAlloc = GetNomineeUnitsAllocated(NomineeIndex);

                // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                // Get member bulk units only since allocated pack units are included in the InTransit value
                //double need = Need.UnitNeed(nomineePlan, GetNomineeOnHand(NomineeIndex),
                //    GetNomineeInTransit(NomineeIndex) + GetNomineeGroupUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex)); // TT#1176 - MD - Jellis- Group Allocation Size Need not observing inv min max
                double need = Need.UnitNeed(nomineePlan, GetNomineeOnHand(NomineeIndex),
                    GetNomineeInTransit(NomineeIndex) + GetNomineeGroupBulkUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex));
                // End TT#1828 - MD - JSmith - Size Need not allocatde to size

                Debug.WriteLine("AllocateByNeed() INDEX: " + NomineeIndex + " OH: " + OH + " IT: " + IT + " Group: " + group + " Group Bulk: " + groupBulk
                    + " ALLOC: " + unitsAlloc
                    + " PLAN: " + nomineePlan
                    + " NEED: " + need
                    );
					
				// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk

                // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                // Get member bulk units only since allocated pack units are included in the InTransit value
                //SetNomineeUnitNeed(NomineeIndex,
                //    Need.UnitNeed(nomineePlan, GetNomineeOnHand(NomineeIndex), GetNomineeInTransit(NomineeIndex) + GetNomineeGroupUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex))); // TT#1176 - MD - Jellis- Group Allocation Size Need not observing inv min max
                SetNomineeUnitNeed(NomineeIndex,
                    Need.UnitNeed(nomineePlan, GetNomineeOnHand(NomineeIndex), GetNomineeInTransit(NomineeIndex) + GetNomineeGroupBulkUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex)));
                // End TT#1828 - MD - JSmith - Size Need not allocatde to size
                SetNomineePercentNeed(NomineeIndex, Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex), nomineePlan));
                if (PercentNeedLimit > Include.DefaultPercentNeedLimit)
                {
                    _unitNeedLimit[NomineeIndex] =
                        (int)((((PercentNeedLimit) * (double)nomineePlan) / 100) + .5);
                    if (nomineePlan != 0)
                    {
                        if (((double)_unitNeedLimit[NomineeIndex] * 100 / (double)nomineePlan)
                            < PercentNeedLimit)
                        {
                            _unitNeedLimit[NomineeIndex]++; // increase limit because %NEED LIMIT is not on an unit need integer boundary.
                        }
                    }
                }
                else
                {
                    _unitNeedLimit[NomineeIndex] = int.MinValue;
                }
                // end   MID Track 5525 AnF Defect 1618: Rounding error when applying %Need Limit
            }

            if (aWorkUpBuy == eWorkUpBuyAllocationType.WorkUpTotalAllocationBuy) // MID Track 3810 Size Allocation GT Style Allocation
            {
                if (aAllocatePriorityNominees)
                {
                    GiveNeedUpToMaximum(false); // MID Track 3810 Size Allocation GT Style Allocation
                }
                if (aGiveNeedUptoMax)
                {
                    GiveNeedUpToMaximum(true); // MID Track 3790 Fill Size Holes // MID Track 3810 Size Allocation GT Style Allocation
                }
                if (aEqualizeNeeds)
                {
                    ProcessWorkUpBuy();
                }
                if (aGiveMinUptoMax)
                {
                    GiveMinUptoMax();
                }

            }
            else
            {
                if (aAllocatePriorityNominees
                   && this.UnitsToAllocate > this.UnitsAllocated)
                {
                    GiveNeedUpToMaximum(false); // MID Track 3790 Fill Size Holes // MID Track 3810 Size Allocation GT Style Allocation
                }
                if (aGiveNeedUptoMax
                    && this.UnitsToAllocate > this.UnitsAllocated)
                {
                    GiveNeedUpToMaximum(true); // MID Track 3790 Fill Size Holes // MID Track 3810 Size Allocation GT Style Allocation
                }
                if (aEqualizeNeeds
                    && this.UnitsToAllocate > this.UnitsAllocated)
                {
                    EqualizePctNeed();
                }
                if (aGiveMinUptoMax
                    && this.UnitsToAllocate > this.UnitsAllocated)
                {
                    GiveMinUptoMax();
                }
                if (aGiveOverMax
                    && this.UnitsToAllocate > this.UnitsAllocated)
                {
                    GiveOverMax();
                }
            }

        }
		#endregion Allocate By Need Main

		#region Give Need Up To Maximum   // MID Track 3810 Size Allocation GT Style Allocation
		/// <summary>
		/// Gives each qualified Nominee its need upto its maximum
		/// </summary>
		/// <param name="aAllNomineesCandidate">True: all nominees are candidates (Fill Size Holes does this); False: only nominees marked as priority are treated as priority stores</param>
		/// <remarks>Used to allocate merchandise to "new" stores.  New stores are allocated what they need up to the percent need threshhold or their Max whichever is less.</remarks>
		private void GiveNeedUpToMaximum(bool aAllNomineesCandidate) // MID Track 3786 Change Fill Size holes Algorithm  // MID Track 3810 Size Allocation GT Style Allocation
		{
			_candidateCount = 0;
			this.ObjectivePercentNeed = this.PercentNeedLimit;
			//_allocationRoundingFactor = 0; // MID track 4278 Percent Need Limit not observed  // MID Track 5525 AnF Defect 1618: Rounding Error
			if (this.ObjectivePercentNeed < 0.0)
			{
				this.ObjectivePercentNeed = 0.0;
			}
			IdentifyCandidates(!aAllNomineesCandidate); // MID Track 3786 Change Fill Size Holes Algorithm
			if (this.ThereAreCandidates)
			{
				bool ThereArePriorityCandidates = false;
				for (int i = 0; i < NomineeDimension; i++)
				{
					if (GetNomineeIsCandidate(i))
					{
						// begin MID Track 5525 AnF Defect 1618: Rounding Error
						//if (GetNomineePercentNeed(i) <= this.PercentNeedLimit)
						//{
						//	SetNomineeIsCandidate(i, false);
						//}
						if (GetNomineeUnitNeed(i) <= _unitNeedLimit[i])
						{
							SetNomineeIsCandidate(i, false);
						}
						// end MID Track 5525 AnF Defect 1618: Rounding Error
						if (GetNomineePercentNeed(i) <= 0)
						{
							SetNomineeIsCandidate(i, false);
						}
					}
					if (GetNomineeIsCandidate(i))
					{
						ThereArePriorityCandidates = true;
				    	_candidateCount++;
					}
				}
				if (ThereArePriorityCandidates)
				{
					AllocateToCandidates();
				}
			}
		}
		#endregion Give Need Up to Maximum 

		#region Process WorkUpBuy
		/// <summary>
		/// Allocates to Priority Nominees
		/// </summary>
		/// <remarks>Used to allocate merchandise to "new" stores.  New stores are allocated what they need up to the percent need threshhold or their Max whichever is less.</remarks>
		private void ProcessWorkUpBuy()
		{
			_candidateCount = 0;
			if (this.ObjectivePercentNeed < 0)
			{
				this.ObjectivePercentNeed = 0;
			}
			//_allocationRoundingFactor = 0; // MID Track 4278 Percent Need Limit not observed // MID Track 5525 AnF Defect 1618: Rounding Error
			IdentifyCandidates(false);
			if (this.ThereAreCandidates)
			{
				bool ThereAreWorkUpBuyCandidates = false;
				for (int i = 0; i < NomineeDimension; i++)
				{
					if (GetNomineeIsCandidate(i))
					{
						// begin MID Track 5525 AnF Defect 1618: Rounding error
						//if (GetNomineePercentNeed(i) <= this.PercentNeedLimit)
						//{
						//	SetNomineeIsCandidate(i, false);
						//}
						if (GetNomineeUnitNeed(i) <= _unitNeedLimit[i])
						{
							SetNomineeIsCandidate(i, false);
						}
						// end MID Track 5525 AnF Defect 1618: Rounding Error
						if (GetNomineePercentNeed(i) <= 0)
						{
							SetNomineeIsCandidate(i, false);
						}
					}
					if (GetNomineeIsCandidate(i))
					{
						ThereAreWorkUpBuyCandidates = true;
					    _candidateCount++;
					}	
				}
				if (ThereAreWorkUpBuyCandidates)
				{
					AllocateToCandidates();
				}
			}
		}
		#endregion Process WorkUpBuy

		#region Equalize Percent Need Algorithm
		#region Equalize Percent Need Main
		/// <summary>
		/// EqualizePctNeed allocates merchandise by equalizing percent needs.
		/// </summary>
		private void EqualizePctNeed ()
		{
			UnitsAllocatedInPass = true;

			while ((UnitsAllocatedInPass) && UnitsToAllocate > UnitsAllocated)
			{
				IdentifyCandidates(false);
				UnitsAllocatedInPass = false;
				AllCandidatesAccepted = false;
				while (!AllCandidatesAccepted)
				{
					AllCandidatesAccepted = true;
					_candidateCount = 0;
					if (ThereAreCandidates)
					{
						ThereAreCandidates = false;
						CalculateCandidateTotals();
						if (_nodeComponent.CandidateUnits > 0)
						{
							CalculateObjectives();
							ExcludeCandidatesMeetingObjective();
						}
					}
				}
				if (ThereAreCandidates)
				{
					AllocateToCandidates();
				}
			}
		}
		#endregion Equalize Percent Need Main

		#region Determine Candidate Nominees
		/// <summary>
		/// Determines the "universe" of candidate nominees for a single pass of the need algorithm.
		/// </summary>
        private void IdentifyCandidates(bool aIdentifyPriorityCandidates) 
		{
			NodeComponent n = _nodeComponent;
            ResetCandidateNodes(n);
			ThereAreCandidates = false;
			for (int i = 0; i < NomineeDimension; i++)
			{
				n.SetNomineeIncludeNode (i, false);
				SetNomineeIsCandidate (i, false);
				if (!GetNomineeIsOut(i) == true)
				{
					SetNomineeIsCandidate(i, IdentifySubNodeCandidates(i, n));
				}
				// begin MID Track 5525 AnF Defect 1618: Rounding Error
				//if (GetNomineePercentNeed(i) <= this.PercentNeedLimit)
				//{
				//	SetNomineeIsCandidate(i, false);
				//}
				if (GetNomineeUnitNeed(i) <= _unitNeedLimit[i])
				{
					SetNomineeIsCandidate(i, false);
				}
				// end MID Track 5525 AnF Defect 1618: Rounding Error
				if (this.GetNomineePlan(i) <= 0)
				{
					SetNomineeIsCandidate(i, false);
				}
                // begin TT#1478 - Size Multple Broken
                if (n.NodeRemainingToAllocate < GetNomineeMultiple(i)  // TT#1525 - Need results in 0 for WUB
                    && !_workUpBuy)                                    // TT#1524 - Need results in 0 for WUB
                {
                    SetNomineeIsCandidate(i, false);
                }
                // end TT#1478 - Size Multiple Broken
				if (aIdentifyPriorityCandidates == true)
				{
					if (GetNomineeHasAllocationPriority(i) == false)
					{
						SetNomineeIsCandidate(i, false);
					}
				}
				else
				{
					if (GetNomineeHasAllocationPriority(i) == true)
					{
						SetNomineeIsCandidate(i, false);
					}
				}
				if (GetNomineeIsCandidate(i) == true)
				{
					n.CandidateNode = true;
					ThereAreCandidates = true;
				}
			}
		}
        
		/// <summary>
		/// Determines if node and nominee are candidates for allocation
		/// </summary>
		/// <param name="NomineeIndex"></param>
		/// <param name="aNodeComponent"></param>
		/// <returns>True:  node and nominee are candidates; False: nominee is not a candidate for this node.</returns>
		internal bool IdentifySubNodeCandidates(int NomineeIndex, NodeComponent aNodeComponent)
		{
			bool include = true;
			if (aNodeComponent.AllocateSubNodes == true && aNodeComponent.SubnodeDimension > 0)
			{
				bool subNodeInclude = false;
				for (int j = 0; j < aNodeComponent.SubnodeDimension; j++)
				{
					int [] J = {j};
					NodeComponent s = aNodeComponent.GetSubNodeComponent(aNodeComponent, J);
					if (IdentifySubNodeCandidates(NomineeIndex, s) == true)
					{
						subNodeInclude = true;
					}
				}
				include = subNodeInclude;
			}

			if ((aNodeComponent.NodeUnitsAllocated >= aNodeComponent.NodeUnitsToAllocate && !_workUpBuy)
			    || aNodeComponent.GetNomineeRuleExcludedNode(NomineeIndex) == true
				|| aNodeComponent.GetNomineeUnitsAllocated(NomineeIndex) >= aNodeComponent.GetNomineeMaximum(NomineeIndex)
				|| aNodeComponent.GetNomineeUnitsAvailable(NomineeIndex) <= 0
				|| (!_workUpBuy && ((aNodeComponent.NodeRemainingToAllocate + aNodeComponent.GetNomineeUnitsAllocated(NomineeIndex)) < aNodeComponent.GetNomineeMinimum(NomineeIndex))))  // MID Track ##2457 WorkUpTotal Buy gets multiple error if multiple not 1.
			{
				include = false;
			}
			aNodeComponent.SetNomineeIncludeNode(NomineeIndex, include);
            aNodeComponent.SetNomineeWrkUnitsAllocated(NomineeIndex,0);  // MID Track 3149 Color Min/Max not observed.
			if (include == true)
			{
				aNodeComponent.CandidateNode = include;
			}
			return include;
		}
		#endregion Determine Candidate Nominees

		#region Reset Candidate Nodes
		/// <summary>
		/// Resets the CandidateNode flag and CandidateUnits value for the specifed node.
		/// </summary>
		/// <param name="aNodeComponent">The node component for which a reset is required.</param>
		/// <remarks>
		/// This is a recursive procedure that walks the entire allocation node structure from the given node to the lowest level nodes.
		/// </remarks>
		internal void ResetCandidateNodes (NodeComponent aNodeComponent)
		{
			aNodeComponent.CandidateNode = false;
			aNodeComponent.CandidateUnits = 0;
			if (aNodeComponent.SubNodeComponents != null)
			{
				foreach (NodeComponent s in aNodeComponent.SubNodeComponents)
				{
					ResetCandidateNodes(s);
				}
			}
		}
		#endregion Reset Candidate Nodes

		#region Calculate candidate totals
		/// <summary>
		/// Calculates the total plan, onhand, intransit, units allocated for the candidate nominees and units available to allocate to the candidates.
		/// </summary>
		private void CalculateCandidateTotals ()
		{
			CandidatePlan = 0;
			CandidateOnHand = 0;
			CandidateInTransit = 0;
			CandidateUnitsAllocated = 0;
			for (int i = 0; i < NomineeDimension; i++)
			{
				if (GetNomineeIsCandidate(i) == true)
				{
					CandidatePlan += _nomineeBasis[i].TotalPlan;
					CandidateOnHand += _nomineeBasis[i].OnHand;
					CandidateInTransit += _nomineeBasis[i].InTransit;
					CandidateUnitsAllocated += _nodeComponent.GetNomineeUnitsAllocated(i);
				}
			}
			CalcCandidateAvailableToAllocate(_nodeComponent);
		}

		/// <summary>
		/// Calculate the total units available to allocate to the candidate nominees for the given allocation node.
		/// </summary>
		/// <param name="aNodeComponent">The node component for which the calculation is to occur.</param>
		/// <remarks>
		/// This is a recursive procedure that calculate the units available to allocate to the candidate nominees.
		/// Only units remaining to allocate in subnodes included by the candidate nominees are accumulated. 
		/// Consequently, if every candidate node excludes a particular node, then that node is not included in the total.
		/// The result of the calculation is saved in aNodeComponent.CandidateUnits.
		/// </remarks>
		internal void CalcCandidateAvailableToAllocate(NodeComponent aNodeComponent)
		{
			NodeComponent n = aNodeComponent;
			n.CandidateUnits = 0;
			if (n.CandidateNode == true)
			{
				if (n.AllocateSubNodes == true)
				{
					foreach (NodeComponent s in n.SubNodeComponents)
					{
						CalcCandidateAvailableToAllocate(s);
						n.CandidateUnits += s.CandidateUnits;
					}
					if (n.CandidateUnits > n.NodeRemainingToAllocate)
					{
						n.CandidateUnits = n.NodeRemainingToAllocate;
					}
				}
				else
				{
					n.CandidateUnits = n.NodeRemainingToAllocate;
				}
			}
		}
		#endregion Calculate Candidate Totals

		#region Calculate Objectives
		/// <summary>
		/// Calculate the objective unit need and percent need for a need allocation.
		/// </summary>
		private void CalculateObjectives()
		{
			ObjectiveNeed = Need.UnitNeed(CandidatePlan, CandidateOnHand, CandidateInTransit, CandidateUnitsAllocated + (_nodeComponent.CandidateUnits));
			ObjectivePercentNeed = 
				Need.PctUnitNeed(ObjectiveNeed, CandidatePlan);
			ObjectivePercentNeed = 
				Math.Max(ObjectivePercentNeed, PercentNeedLimit);
			// begin MID Track 5525 AnF Defect 1618: Rounding Error
			// begin MID Track 4278 Percent need Limit not observed
			//if (ObjectivePercentNeed > PercentNeedLimit)
			//{
			//	_allocationRoundingFactor = .5; 
			//}
			//else
			//{
			//	_allocationRoundingFactor = 0;
			//}
			// end MID Track 4278 Percent need Limit not observed
			// end MID Track 5525 AnF Defect 1618: Rounding Error
		}		
		#endregion Calculate Objectives
	
		#region Exclude Candidates meeting objective
		/// <summary>
		/// Removes candidate nominees that already meet the objective.
		/// </summary>
		private void ExcludeCandidatesMeetingObjective ()
		{
			AllCandidatesAccepted = true;
			for (int i = 0; i < NomineeDimension; i++)
			{
				if (GetNomineeIsCandidate(i) == true)
				{
					if (GetNomineePercentNeed(i) <= ObjectivePercentNeed)
					{
						AllCandidatesAccepted = false;
						SetNomineeIsCandidate (i, false);
					}
					else
					{
						ThereAreCandidates = true;
						_candidateCount++;
					}
				}
			}
		}
		#endregion Exclude Candidates meeting objective

		#region Allocate to Candidates
		/// <summary>
		/// For each candidate nominee, calculates the desired total allocation based on the objectives.
		/// </summary>
		private void AllocateToCandidates ()
		{
			MIDGenericSortItem[] _nse = new MIDGenericSortItem[_candidateCount];
			int _nseEntry = 0;
			for (int i = 0; i < NomineeDimension; i++)
			{
				if (GetNomineeIsCandidate(i))
				{
					if (_nseEntry < _candidateCount)
					{
						_nse[_nseEntry].Item = i;
						_nse[_nseEntry].SortKey = new double[3] ;
						_nse[_nseEntry].SortKey[0] = GetNomineePercentNeed(i);
						_nse[_nseEntry].SortKey[1] = GetNomineeUnitNeed(i);
						_nse[_nseEntry].SortKey[2] = _transaction.GetRandomDouble();
						_nseEntry++;
					}
				}
			}
			Array.Sort(_nse,new MIDGenericSortDescendingComparer());
			//double HalfMultiple = Multiple * .5d;  // MID Track 5525 AnF defect 1618: Rounding Error
			double nomineePercentNeed;
			double nomineePlan;
			int unitsAllocated;
			int unitsAvailable;
            int multiple; // TT#1478 - Size Multiple Broken
            int nomineeMaximum; // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
            int nomineeMinimum; // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
			for (int srt = 0; srt < _candidateCount; srt++)
			{
				int NomineeIndex = _nse[srt].Item;
                // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                // get before effect of this allocation
                nomineeMaximum = GetNomineeMaximum(NomineeIndex); 
                nomineeMinimum = GetNomineeMinimum(NomineeIndex);
                nomineePercentNeed = GetNomineePercentNeed(NomineeIndex);
                // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
				if (nomineePercentNeed > ObjectivePercentNeed)
				{
					nomineePlan = GetNomineePlan(NomineeIndex);    // MID track 5525 AnF Defect 1618: Rounding error
                    unitsAllocated = 
						(int)((
						((nomineePercentNeed - ObjectivePercentNeed)
						* nomineePlan)
						/ 100d) + .5d);                         // MID Track 5525 AnF Defect 1618: Rounding error when applying % Need Limit
					//	/ 100) + _allocationRoundingFactor);  // MID Track 4278 Percent Need Limit not observed // MID Track 5525 AnF Defect 1618: Rounding error when applying % Need Limit
					if (unitsAllocated > 0
						&& (GetNomineeUnitNeed(NomineeIndex) - unitsAllocated
					    	< _unitNeedLimit[NomineeIndex]))
					{
						// adjust units Allocated so that we do not violate need limit
						if ((int)GetNomineeUnitNeed(NomineeIndex) > _unitNeedLimit[NomineeIndex])
						{
							unitsAllocated = (int)GetNomineeUnitNeed(NomineeIndex) - _unitNeedLimit[NomineeIndex];
						}
						else
						{
							unitsAllocated = 0;
						}
					}
					// begin MID Track 4569 Change Need Rounding
					//    Give at least 1 unit to any store that is in the list of stores provided 
					//    the percent need limit is observed
					else if (unitsAllocated == 0
						//&& ObjectivePercentNeed > this.PercentNeedLimit // MID Track 5525 AnF Defect 1618: Rounding Error
						&& _unitNeedLimit[NomineeIndex] < GetNomineeUnitNeed(NomineeIndex)  // MID Track 5525 AnF Defect 1618: Rounding Error
						&& GetNomineePlan(NomineeIndex) > 0)
					{
						unitsAllocated = 1;
					}
					// end MID Track 4569 Change Need Rounding
					unitsAvailable = Math.Max(0, GetNomineeUnitsAvailable(NomineeIndex));
					SetNomineeWrkUnitsAllocated(NomineeIndex,   
						Math.Min(unitsAvailable, unitsAllocated));
				}
				else
				{
					SetNomineeWrkUnitsAllocated(NomineeIndex, 0);
				}
//				SetNomineeWrkUnitsAllocated(NomineeIndex, 
//					(int)((double)(GetNomineeWrkUnitsAllocated(NomineeIndex) + HalfMultiple) / (double)Multiple));
                // begin TT#1478 - Size Multiple Broken
                //SetNomineeWrkUnitsAllocated(NomineeIndex, 
                //    (int)((double)GetNomineeWrkUnitsAllocated(NomineeIndex) / (double)Multiple));
                //SetNomineeWrkUnitsAllocated(NomineeIndex,
                //    GetNomineeWrkUnitsAllocated(NomineeIndex) * Multiple);
                multiple = GetNomineeMultiple(NomineeIndex);
                SetNomineeWrkUnitsAllocated(NomineeIndex,
                    (int)((double)GetNomineeWrkUnitsAllocated(NomineeIndex) / (double)multiple));
                SetNomineeWrkUnitsAllocated(NomineeIndex,
                    GetNomineeWrkUnitsAllocated(NomineeIndex) * multiple);
                // end TT#1478 - Size Multiple Broken
                //int nomineeMaximum = GetNomineeMaximum(Nominee Index);  // get before effect of this allocation // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
				if (NomineeTotalDesiredAllocation(NomineeIndex) 
					> nomineeMaximum)
				{
					int nomineeAllocated = GetNomineeUnitsAllocated(NomineeIndex);
					if (nomineeMaximum > nomineeAllocated)
					{
                        // begin TT#1054 - MD - Jellis - Group Allocation Inv Max not observed
                        //SetNomineeWrkUnitsAllocated(NomineeIndex,
                        //    nomineeMaximum - nomineeAllocated);
                        SetNomineeWrkUnitsAllocated(NomineeIndex,
                            (int)(((double)(nomineeMaximum - nomineeAllocated)) / (double)multiple));
                        SetNomineeWrkUnitsAllocated(NomineeIndex,
                            GetNomineeWrkUnitsAllocated(NomineeIndex) * multiple);
                        // end TT#1054 - MD - Jellis - Group ALlocation Inv Max not observed
					}
					else
					{
						SetNomineeWrkUnitsAllocated(NomineeIndex,0);
					}
				}

				//if (!_workUpBuy && GetNomineeWrkUnitsAllocated(NomineeIndex) > RemainingUnitsToAllocate) // MID Track 3810 Size Allocation GT Style Allocation
                if (_workUpBuyAllocationType != eWorkUpBuyAllocationType.WorkUpTotalAllocationBuy          // MID Track 3810 Size Allocation GT Style Allocation
					&& GetNomineeWrkUnitsAllocated(NomineeIndex) > RemainingUnitsToAllocate)              // MID Track 3810 Size Allocation GT Style Allocation
				{
                    // begin TT#1478 - Size Multiple Broken
                    //SetNomineeWrkUnitsAllocated(NomineeIndex,
                    //    (int)(RemainingUnitsToAllocate / Multiple));
                    //SetNomineeWrkUnitsAllocated(NomineeIndex,
                    //    (int)(GetNomineeWrkUnitsAllocated(NomineeIndex) * Multiple));
                    SetNomineeWrkUnitsAllocated(NomineeIndex,
                        (int)(RemainingUnitsToAllocate / multiple));
                    SetNomineeWrkUnitsAllocated(NomineeIndex,
                        (int)(GetNomineeWrkUnitsAllocated(NomineeIndex) * multiple));
                    // end TT#1478 - Size Multile Broken
				}

                //Debug.WriteLine("Before BreakoutProportional: NomineeIndex=" + NomineeIndex + ";Quantity=" + _nodeComponent.GetNomineeWrkUnitsAllocated(NomineeIndex));

				SetNomineeWrkUnitsAllocated(NomineeIndex, BreakoutProportional(_nodeComponent, NomineeIndex));

                //Debug.WriteLine("After BreakoutProportional: NomineeIndex=" + NomineeIndex + ";Quantity=" + _nodeComponent.GetNomineeWrkUnitsAllocated(NomineeIndex));

                // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                //if (GetNomineeWrkUnitsAllocated(NomineeIndex) < 0 
                //    || NomineeTotalDesiredAllocation(NomineeIndex) < GetNomineeMinimum(NomineeIndex)
                //    || GetNomineeUnitNeed(NomineeIndex) + GetNomineeWrkUnitsAllocated(NomineeIndex) < _unitNeedLimit[NomineeIndex]) // MID Track 5525 AnF Defect 1618: Rounding error
                if (GetNomineeWrkUnitsAllocated(NomineeIndex) < 0
                    || NomineeTotalDesiredAllocation(NomineeIndex) < nomineeMinimum
                    || GetNomineeUnitNeed(NomineeIndex) + GetNomineeWrkUnitsAllocated(NomineeIndex) < _unitNeedLimit[NomineeIndex])                    
                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                {
                    // begin TT#946 - MD - Jellis - Group Allocation Not Working
                    //SetNomineeWrkUnitsAllocated(NomineeIndex, 0);
                    ZeroOutNomineeWrkUnitsAllocated(_nodeComponent, NomineeIndex);
                    // end TT#946 - MD - Jellis - Group Allocation Not Working
				}
				if (GetNomineeWrkUnitsAllocated(NomineeIndex) > 0)
				{
					UnitsAllocatedInPass = true;
					ApplyDesiredAllocation(_nodeComponent, NomineeIndex);
                    // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                    // Get member bulk units only since allocated pack units are included in the InTransit value
                    //SetNomineeUnitNeed(NomineeIndex, 
                    //    Need.UnitNeed(GetNomineePlan(NomineeIndex),GetNomineeOnHand(NomineeIndex),GetNomineeInTransit(NomineeIndex) + GetNomineeGroupUnitsAlreadyAllocated(NomineeIndex),GetNomineeUnitsAllocated(NomineeIndex))); // TT31176 - MD - Jellis - Group Allocation Size need not observing inv min max
                    SetNomineeUnitNeed(NomineeIndex,
                        Need.UnitNeed(GetNomineePlan(NomineeIndex), GetNomineeOnHand(NomineeIndex), GetNomineeInTransit(NomineeIndex) + GetNomineeGroupBulkUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex)));
                    // End TT#1828 - MD - JSmith - Size Need not allocatde to size
					SetNomineePercentNeed(NomineeIndex, Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex), GetNomineePlan(NomineeIndex)));
				}
			}
		}
		#endregion Allocate to Candidates
		#endregion Equalize Percent Needs Algorithm

		#region GiveMinUptoMax
		private void GiveMinUptoMax()
		{
			this.IdentifyCandidates(false); 
			_candidateCount = 0;
			if (this.ThereAreCandidates)
			{
				bool ThereAreMinMaxCandidates = false;
				for (int i = 0; i < NomineeDimension; i++)
				{
					if (GetNomineeIsCandidate(i))
					{
						// begin MID Track 5525 AnF Defect 1618: Rounding Error
						//if (GetNomineePercentNeed(i) <= this.PercentNeedLimit)
						//{
						//	SetNomineeIsCandidate(i, false);
						if (GetNomineeUnitNeed(i) <= _unitNeedLimit[i])
						{
							SetNomineeIsCandidate(i, false);
							// end MID Track 5525 AnF Defect 1618: Rounding Error
						} 
						else if (this.GetNomineeUnitsAllocated(i) >= this.GetNomineeMaximum(i))
						{
							SetNomineeIsCandidate(i, false);
						}
					}
					if (GetNomineeIsCandidate(i))
					{
						ThereAreMinMaxCandidates = true;
						_candidateCount++;
					}
				}
				if (ThereAreMinMaxCandidates)
				{
					MIDGenericSortItem[] _nse = new MIDGenericSortItem[_candidateCount];
					int _nseEntry = 0;
					for (int i = 0; i < NomineeDimension; i++)
					{
						if (GetNomineeIsCandidate(i))
						{
							if (_nseEntry < _candidateCount)
							{
								_nse[_nseEntry].Item = i;
								_nse[_nseEntry].SortKey = new double[3] ;
								_nse[_nseEntry].SortKey[0] = GetNomineePercentNeed(i);
								_nse[_nseEntry].SortKey[1] = GetNomineeUnitNeed(i);
								_nse[_nseEntry].SortKey[2] = _transaction.GetRandomDouble();
								_nseEntry++;
							}
						}
					}
					Array.Sort(_nse,new MIDGenericSortDescendingComparer());
                    //double HalfMultiple = Multiple * .5d;  // TT#1478 - Size Multiple Broken
					int srt = 0;

                    int multiple; // TT#1478 - Size Multiple Broken
                    // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    int nomineeMinimum;
                    int nomineeMaximum;
                    // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
					while (srt < _candidateCount
						&& RemainingUnitsToAllocate > 0)
					{
						int NomineeIndex = _nse[srt].Item;
                        // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                        // get min/max before effect of this allocation
                        nomineeMinimum = GetNomineeMinimum(NomineeIndex);
                        nomineeMaximum = GetNomineeMaximum(NomineeIndex);
                        // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min max Broken
                        multiple = GetNomineeMultiple(NomineeIndex); // TT#1478 - Size Multiple Broken
						UnitsAllocatedInPass = false;
						if (this.GetNomineeUnitsAllocated(NomineeIndex)
                            < nomineeMaximum)                    // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                            //< this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
						{
							if (this.GetNomineeUnitsAllocated(NomineeIndex) == 0 
                                || GetNomineeUnitsAllocated(NomineeIndex) < nomineeMinimum) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                //|| this.GetNomineeUnitsAllocated(NomineeIndex) < this.GetNomineeMinimum(NomineeIndex)) // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
							{
								// Allocate at least the min
								int minValue = 0;
                                if (nomineeMinimum == 0)                       // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                //if (this.GetNomineeMinimum(NomineeIndex) == 0) // TT#1074 - MD - Jellis - Group Allocaiton Inventory Min Max Broken
								{
                                    //if (Multiple <= this.GetNomineeMaximum(NomineeIndex))  // TT#1478 - Size Multiple Broken
                                    //if (multiple <= GetNomineeMaximum(NomineeIndex))         // TT#1478 - Size Multiple Broken // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
                                    if (multiple <= nomineeMaximum)                          // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
									{
                                        minValue = multiple;                                  // TT#1478 - Size Multiple Broken
                                        //minValue = Multiple;                               // TT#1478 - Size Multiple Broken
									}
									else
									{
										minValue = 0;
									}
								}
									// Begin MID Track # 2366     Corrects Store Getting more than capacity 
                                //else if (this.GetNomineeMinimum(NomineeIndex) <= this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
                                else if (nomineeMinimum <= nomineeMaximum)                                               // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
								{
                                    //minValue = this.GetNomineeMinimum(NomineeIndex) - GetNomineeUnitsAllocated(NomineeIndex); // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
                                    minValue = nomineeMinimum - GetNomineeUnitsAllocated(NomineeIndex);                        // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
								}
								else
								{
                                    // Begin TT#4711 - JSmith - Store is allocated less than the Group min, but cannot get more because they hit capacity
                                    //minValue = 0;
                                    minValue = nomineeMinimum;
                                    // End TT#4711 - JSmith - Store is allocated less than the Group min, but cannot get more because they hit capacity
								}
								//  End MID Track # 2366
								SetNomineeWrkUnitsAllocated(
									NomineeIndex,
									minValue);

								if (minValue <= RemainingUnitsToAllocate
									&& minValue <= this.GetNomineeUnitsAvailable(NomineeIndex)
                                    && nomineeMinimum <= nomineeMaximum)  // TT#1737-MD - JSmith - GA - Capacity not honored for single bulk header, non-group Allocation
								{
									// begin MID Track 5525 Defect ID 1618: Rounding error
									//_pctNeedThresh = 
									//	Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex) - minValue, GetNomineePlan(NomineeIndex));
									//if (_pctNeedThresh >= this.PercentNeedLimit)
									if ((GetNomineeUnitNeed(NomineeIndex) - minValue) >= _unitNeedLimit[NomineeIndex])
										// end MID Track 5525 Defect ID 1618: Rounding Error
									{
										if (minValue > 0 
											&& this.BreakoutProportional(_nodeComponent, NomineeIndex) == minValue)
										{
											UnitsAllocatedInPass = true;
										}
										else
										{
											if (this._nodeComponent.AllocateSubNodes)
											{
												// Allocate one pack or color as a min--clone/modify BreakoutProportional for this!
                                                //_allocateValue = AllocateSubnodeSubstitute(this._nodeComponent, NomineeIndex, Multiple, true);                   // TT#1478 - Size Multiple Broken
                                                _allocateValue = AllocateSubnodeSubstitute(_nodeComponent, NomineeIndex, GetNomineeMultiple(NomineeIndex), true); // TT#1478 - Size Multiple Broken
                                                //if (_allocateValue > 0 &&         // TT#1152 - Units Allocated less than min
                                                if (_allocateValue >= minValue &&   // TT#1152 - Units ALlocated less than min
                                                    (_allocateValue + this.GetNomineeUnitsAllocated(NomineeIndex)) 
                                                    <= nomineeMaximum)                       // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken   
                                                    //<= this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken 
												{
													// begin MID Track 5525 Defect ID 1618; Rounding Error
													//_pctNeedThresh = 
													//	Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex) - _allocateValue, GetNomineePlan(NomineeIndex));
													//if (_pctNeedThresh >= this.PercentNeedLimit)
													if ((GetNomineeUnitNeed(NomineeIndex) - _allocateValue) >= _unitNeedLimit[NomineeIndex])
														// end MID Track 5525 Defect ID 1618: Rounding Error													
													{
														SetNomineeWrkUnitsAllocated(
															NomineeIndex,
															_allocateValue);
                                                        // Begin TT#4680 - stodd - GA with Capacity setting - Need Action does not respond, hangs up application
                                                        if (_allocateValue > 0)
                                                        {
                                                            UnitsAllocatedInPass = true;
                                                        }
                                                        // End TT#4680 - stodd - GA with Capacity setting - Need Action does not respond, hangs up application
													}
												}
											}
										}
									}
								}
							}
							else
							{
								// Allocate a multiple till we get to max
                                //if (Multiple <= GetNomineeUnitsAvailable(NomineeIndex))  // TT#1478 - Size Multiple Broken
                                if (multiple <= GetNomineeUnitsAvailable(NomineeIndex))    // TT#1478 - Size Multiple Broken
								{
                                    //if (Multiple > RemainingUnitsToAllocate)             // TT#1478 - Size Multiple Broken
                                    if (multiple > RemainingUnitsToAllocate)               // TT#1478 - Size Multiple Broken
									{
                                        // begin TT#1478 - Size Multiple Broken
                                        if (!_observeMultiples)
                                        {
                                            // end TT#1478 - Size Multiple Broken
                                            SetNomineeWrkUnitsAllocated(NomineeIndex, RemainingUnitsToAllocate);
                                            //if (this.NomineeTotalDesiredAllocation(NomineeIndex) > this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                            if (this.NomineeTotalDesiredAllocation(NomineeIndex) > nomineeMaximum) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                            {
                                                SetNomineeWrkUnitsAllocated
                                                    (
                                                    NomineeIndex,
                                                    Math.Max(0, (nomineeMaximum - this.GetNomineeUnitsAllocated(NomineeIndex))) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                                    //Math.Max(0, (this.GetNomineeMaximum(NomineeIndex) - this.GetNomineeUnitsAllocated(NomineeIndex))) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                                    );
                                            }
                                            // begin MID Track 5525 AnF Defect 1618: Rounding Error
                                            //_pctNeedThresh = 
                                            //	Need.PctUnitNeed
                                            //	(
                                            //	GetNomineeUnitNeed(NomineeIndex) - GetNomineeWrkUnitsAllocated(NomineeIndex),
                                            //	GetNomineePlan(NomineeIndex)
                                            //	);
                                            //if (_pctNeedThresh < this.PercentNeedLimit)
                                            //{
                                            //	if (GetNomineePercentNeed(NomineeIndex) > this.PercentNeedLimit)
                                            //    {
                                            //    	SetNomineeWrkUnitsAllocated(NomineeIndex,   
                                            // 		(int)(((GetNomineePercentNeed(NomineeIndex) - this.PercentNeedLimit)
                                            // 		* GetNomineePlan(NomineeIndex)
                                            //		/ 100)));         // MID Track 4121 Size need Overallocates stores
                                            //	// / 100) + .5)); // MID Track 4121 Size Need Overallocates stores
                                            //    }
                                            if ((int)GetNomineeUnitNeed(NomineeIndex) - GetNomineeWrkUnitsAllocated(NomineeIndex)
                                                < _unitNeedLimit[NomineeIndex])
                                            {
                                                if (GetNomineeUnitNeed(NomineeIndex) > _unitNeedLimit[NomineeIndex])
                                                {
                                                    SetNomineeWrkUnitsAllocated(NomineeIndex,
                                                        (int)GetNomineeUnitNeed(NomineeIndex) - _unitNeedLimit[NomineeIndex]);
                                                }
                                                else
                                                {
                                                    SetNomineeWrkUnitsAllocated(NomineeIndex, 0);
                                                }
                                            }
                                            if (this.BreakoutProportional(_nodeComponent, NomineeIndex) > 0)
                                            {
                                                UnitsAllocatedInPass = true;
                                            }
                                        }  // TT#1478 - Size Multple Broken
									}
									else 
									{
                                        //SetNomineeWrkUnitsAllocated(NomineeIndex, Multiple);  // TT#1478 - Size Multiple Broken
                                        SetNomineeWrkUnitsAllocated(NomineeIndex, multiple);    // TT#1478 - Size Multiple Broken
                                        //if (this.NomineeTotalDesiredAllocation(NomineeIndex) <= this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
                                        if (this.NomineeTotalDesiredAllocation(NomineeIndex) <= nomineeMaximum) // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
										{
											// begin MID Track 5525 AnF Defect 1618: Rounding Error
											//_pctNeedThresh = 
											//	Need.PctUnitNeed
											//	(
											//	GetNomineeUnitNeed(NomineeIndex) - Multiple,
											//	GetNomineePlan(NomineeIndex)
											//	);
											//if (_pctNeedThresh >= this.PercentNeedLimit
											//	&& (this.BreakoutProportional(_nodeComponent, NomineeIndex) == Multiple))
                                            // begin TT#1478 - Size Multiple Broken
                                            //if (GetNomineeUnitNeed(NomineeIndex) - Multiple >= _unitNeedLimit[NomineeIndex]
                                            //    && (this.BreakoutProportional(_nodeComponent, NomineeIndex) == Multiple))
												// end MID Track 5525 AnF Defect 1618: Rounding Error
                                            if (GetNomineeUnitNeed(NomineeIndex) - multiple >= _unitNeedLimit[NomineeIndex]
                                                && (BreakoutProportional(_nodeComponent, NomineeIndex) == multiple))
                                            // end TT#1478 - Size Multiple Broken
										    {
												UnitsAllocatedInPass = true;
											}
											else 
											{
												if (this._nodeComponent.AllocateSubNodes)
												{
													// Allocate one pack or color as a substitute--clone/modify BreakoutProportional for this! 
                                                    //_allocateValue = AllocateSubnodeSubstitute(this._nodeComponent, NomineeIndex, Multiple, true);  // TT#1478 - Size Multiple Broken
                                                    _allocateValue = AllocateSubnodeSubstitute(_nodeComponent, NomineeIndex, multiple, true);        // TT#1478 - Size Multiple Broken
													if (_allocateValue > 0 &&
														(_allocateValue + this.GetNomineeUnitsAllocated(NomineeIndex)) 
                                                        <= nomineeMaximum)                       // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
                                                        //<= this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
													{
														// begin MID track 5525 AnF Defect 1618: Rounding Error
														//_pctNeedThresh = 
														//	Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex) - _allocateValue, GetNomineePlan(NomineeIndex));
														//if (_pctNeedThresh >= this.PercentNeedLimit)
														if (GetNomineeUnitNeed(NomineeIndex) - _allocateValue >= _unitNeedLimit[NomineeIndex])
															// end MID Track 5525 AnF Defect 1618: Rounding Error
														{
															SetNomineeWrkUnitsAllocated(
																NomineeIndex,
																_allocateValue);
                                                            // Begin TT#4680 - stodd - GA with Capacity setting - Need Action does not respond, hangs up application
                                                            if (_allocateValue > 0)
                                                            {
                                                                UnitsAllocatedInPass = true;
                                                            }
                                                            // End TT#4680 - stodd - GA with Capacity setting - Need Action does not respond, hangs up application
														}
													}
												}
											}
										}
									}
								}
							}
						}
						if (UnitsAllocatedInPass)
						{
							this.ApplyDesiredAllocation(_nodeComponent, NomineeIndex);
                            // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                            // Get member bulk units only since allocated pack units are included in the InTransit value
                            //SetNomineeUnitNeed(NomineeIndex, 
                            //    Need.UnitNeed(GetNomineePlan(NomineeIndex),GetNomineeOnHand(NomineeIndex),GetNomineeInTransit(NomineeIndex) + GetNomineeGroupUnitsAlreadyAllocated(NomineeIndex),GetNomineeUnitsAllocated(NomineeIndex))); // TT#1176 - MD - Jellis - Group Allocation - Size Need Not observing inv min max
                            SetNomineeUnitNeed(NomineeIndex,
                                Need.UnitNeed(GetNomineePlan(NomineeIndex), GetNomineeOnHand(NomineeIndex), GetNomineeInTransit(NomineeIndex) + GetNomineeGroupBulkUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex)));
                            // End TT#1828 - MD - JSmith - Size Need not allocatde to size
							SetNomineePercentNeed(NomineeIndex, Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex), GetNomineePlan(NomineeIndex)));
							_nse[srt].SortKey[0] = GetNomineePercentNeed(NomineeIndex);
							_nse[srt].SortKey[1] = GetNomineeUnitNeed(NomineeIndex);
							Array.Sort(_nse,srt,_candidateCount - srt, new MIDGenericSortDescendingComparer());
						}
						else
						{
							srt++;
						}
					}
				}
			}
		}
		#endregion GiveMinUptoMax

		#region GiveOverMax
		private void GiveOverMax()
		{
			this.IdentifyCandidates(false);
			_candidateCount = 0;
            // begin TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
            int nomineeMinimum;
            int nomineeMaximum;
            // end // TT#1074 - MD - Jelli s- Group Allocation Inventory Min Max Broken
			if (this.ThereAreCandidates)
			{
				bool ThereAreOverMaxCandidates = false;
				for (int i = 0; i < NomineeDimension; i++)
				{
					if (GetNomineeIsCandidate(i))
					{
						// begin MID Track 5525 AnF Defect 1618: Rounding Error
						//if (GetNomineePercentNeed(i) <= this.PercentNeedLimit)
						//{
						//	SetNomineeIsCandidate(i, false);
						//}
						if (GetNomineeUnitNeed(i) <= GetNomineeUnitNeedLimit(i))
						{
							SetNomineeIsCandidate(i, false);
						}
							// end MID Track 5525 AnF Defect 1618: Rounding Error
                        else if (this.GetNomineeUnitsAllocated(i) < this.GetNomineeMinimum(i))  // "ok" since no units pending allocation // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
						{
							SetNomineeIsCandidate(i, false);
						}
					}
					if (GetNomineeIsCandidate(i))
					{
						ThereAreOverMaxCandidates = true;
						_candidateCount++;
					}
				}
				if (ThereAreOverMaxCandidates)
				{
					MIDGenericSortItem[] _nse = new MIDGenericSortItem[_candidateCount];
					int _nseEntry = 0;
					for (int i = 0; i < NomineeDimension; i++)
					{
						if (GetNomineeIsCandidate(i))
						{
							if (_nseEntry < _candidateCount)
							{
								_nse[_nseEntry].Item = i;
								_nse[_nseEntry].SortKey = new double[3] ;
								_nse[_nseEntry].SortKey[0] = GetNomineePercentNeed(i);
								_nse[_nseEntry].SortKey[1] = GetNomineeUnitNeed(i);
								_nse[_nseEntry].SortKey[2] = _transaction.GetRandomDouble();
								_nseEntry++;
							}
						}
					}
					Array.Sort(_nse,new MIDGenericSortDescendingComparer());
                    //double HalfMultiple = Multiple * .5d;   // TT#1478 - Size Multiple Broken
                    int multiple;                             // TT#1478 - Size Multiple Broken
					int srt = 0;
					
					while (srt < _candidateCount
						&& RemainingUnitsToAllocate > 0)
					{
						int NomineeIndex = _nse[srt].Item;
                        nomineeMaximum = GetNomineeMaximum(NomineeIndex); // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                        nomineeMinimum = GetNomineeMinimum(NomineeIndex); // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken

                        multiple = GetNomineeMultiple(NomineeIndex); // TT#1478 - Size Multiple Broken
						UnitsAllocatedInPass = false;
						if (this.GetNomineeUnitsAllocated(NomineeIndex)
                            > nomineeMinimum) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                            //> this.GetNomineeMinimum(NomineeIndex)) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
						{
								// Allocate a multiple till we run out of units
                            // begin TT#1478 - Size Multiple Broken
                            //if (Multiple <= GetNomineeUnitsAvailable(NomineeIndex))
                            //{
                            //    if (Multiple > RemainingUnitsToAllocate)
                            if (multiple <= GetNomineeUnitsAvailable(NomineeIndex))
                            {
                                if (multiple > RemainingUnitsToAllocate)
                                    // end TT#1478 - Size Multiple Broken
								{
                                    // begin TT#1478 - Size Multiple Broken
                                    if (!_observeMultiples)
                                    {
                                        // end TT#1478 - Size Multiple Broken
                                        SetNomineeWrkUnitsAllocated(NomineeIndex, RemainingUnitsToAllocate);
                                        if (GetNomineeUnitNeed(NomineeIndex) - RemainingUnitsToAllocate >= _unitNeedLimit[NomineeIndex] // MID Track 5525 AnF Defect 1618: rounding error
                                            && this.BreakoutProportional(_nodeComponent, NomineeIndex) > 0)                              // MID Track 5525 AnF Defect 1618: rounding error
                                        {
                                            UnitsAllocatedInPass = true;
                                        }
                                    }   // TT#1478 - Size Multiple Broken
								}
								else 
								{
                                    // begin TT#1478 - Size Multiple Broken
                                    //SetNomineeWrkUnitsAllocated(NomineeIndex, Multiple);
                                    //if (GetNomineeUnitNeed(NomineeIndex) - Multiple >= _unitNeedLimit[NomineeIndex] // MID Track 5525 AnF Defect 1618: rounding error
                                    //    && this.BreakoutProportional(_nodeComponent, NomineeIndex) == Multiple)     // MID Track 5525 AnF Defect 1618: rounding error
                                    SetNomineeWrkUnitsAllocated(NomineeIndex, multiple);
                                    if (GetNomineeUnitNeed(NomineeIndex) - multiple >= _unitNeedLimit[NomineeIndex] // MID Track 5525 AnF Defect 1618: rounding error
                                        && this.BreakoutProportional(_nodeComponent, NomineeIndex) == multiple)     // MID Track 5525 AnF Defect 1618: rounding error
                                    // end TT#1478 - Size Multiple Broken
									{
										UnitsAllocatedInPass = true;
									}
									else
									{
										if (this._nodeComponent.AllocateSubNodes)
										{
											// Allocate one pack or color as a substitute--clone/modify BreakoutProportional for this! 
                                            //_allocateValue = AllocateSubnodeSubstitute(this._nodeComponent, NomineeIndex, Multiple, false);  // TT#1478 - Size Multiple Broken
                                            _allocateValue = AllocateSubnodeSubstitute(_nodeComponent, NomineeIndex, multiple, false);         // TT#1478 - Size Multiple Broken
											if (_allocateValue > 0 &&
												(_allocateValue + this.GetNomineeUnitsAllocated(NomineeIndex)) 
                                                <= nomineeMaximum)                       // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                                //<= this.GetNomineeMaximum(NomineeIndex)) // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
											{
												// begin MID Track 5525 AnF Defect 1618: Rounding Error
												//_pctNeedThresh = 
												//	Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex) - _allocateValue, GetNomineePlan(NomineeIndex));
												//if (_pctNeedThresh >= this.PercentNeedLimit)
												if (GetNomineeUnitNeed(NomineeIndex) - _allocateValue >= _unitNeedLimit[NomineeIndex])
													// end MID Track 5525 AnF Defect 1618: Rounding Error
												{
													SetNomineeWrkUnitsAllocated(
														NomineeIndex,
														_allocateValue);
													UnitsAllocatedInPass = true;
												}
											}
										}
									}
								}
							}
						}
						if (UnitsAllocatedInPass)
						{
							this.ApplyDesiredAllocation(_nodeComponent, NomineeIndex);
                            // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                            // Get member bulk units only since allocated pack units are included in the InTransit value
                            //SetNomineeUnitNeed(NomineeIndex, 
                            //    Need.UnitNeed(GetNomineePlan(NomineeIndex),GetNomineeOnHand(NomineeIndex),GetNomineeInTransit(NomineeIndex) + GetNomineeGroupUnitsAlreadyAllocated(NomineeIndex),GetNomineeUnitsAllocated(NomineeIndex))); // TT#1176 - MD - Jellis- Group Allocation Size Need not observing inv min max
                            SetNomineeUnitNeed(NomineeIndex,
                                Need.UnitNeed(GetNomineePlan(NomineeIndex), GetNomineeOnHand(NomineeIndex), GetNomineeInTransit(NomineeIndex) + GetNomineeGroupBulkUnitsAlreadyAllocated(NomineeIndex), GetNomineeUnitsAllocated(NomineeIndex)));
                            // End TT#1828 - MD - JSmith - Size Need not allocatde to size
							SetNomineePercentNeed(NomineeIndex, Need.PctUnitNeed(GetNomineeUnitNeed(NomineeIndex), GetNomineePlan(NomineeIndex)));
							_nse[srt].SortKey[0] = GetNomineePercentNeed(NomineeIndex);
							_nse[srt].SortKey[1] = GetNomineeUnitNeed(NomineeIndex);
							Array.Sort(_nse,srt,_candidateCount - srt, new MIDGenericSortDescendingComparer());
						}
						else
						{
							srt++;
						}
					}
				}
			}
		}
		#endregion GiveOverMax
		#endregion Need Allocation Algorithms

		#region Breakout Sub-nodes Proportionally
		/// <summary>
		/// Proportionally spreads the specified node's WrkUnitsAllocated to the subnodes based on remaining to allocate.
		/// </summary>
		/// <param name="aNodeComponent">Node component containing the WrkUnitsAllocated to spread.</param>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Accumulated results of the spread.  This accumulated total may differ from the original WrkUnitsAllocated because of subnode multiples, minimums, maximums and other considerations.</returns>
		internal int BreakoutProportional(NodeComponent aNodeComponent, int aNomineeIndex)
     	{
            //if (aNomineeIndex == 61)
            //{
            //    Debug.WriteLine("Begin BreakoutProportional: NodeType=" + aNodeComponent.NodeType + ";NodeDescription=" + aNodeComponent.NodeDescription + " NomineeIndex=" + aNomineeIndex + ";Quantity=" + aNodeComponent.GetNomineeWrkUnitsAllocated(aNomineeIndex));
            //}
			int _returnTotal = 0;
			NodeComponent n = aNodeComponent;
			if (n.AllocateSubNodes == true)
			{
				MIDGenericSortItem[] _bse = new MIDGenericSortItem[n.SubnodeDimension];
				int _oldTotal = 0;
				NodeComponent s;
				int _srt = 0;
				for (int i = 0; i < n.SubnodeDimension; i++)
				{
					s = n.GetChildNodeComponent(n, i);
					s.SetNomineeWrkUnitsAllocated(aNomineeIndex,0);
					if (s.GetNomineeIncludeNode(aNomineeIndex) == true)
					{
                        //if (aNomineeIndex == 61)
                        //{
                        //    Debug.WriteLine("GetNomineeIncludeNode: NodeType=" + s.NodeType + ";NodeDescription=" + s.NodeDescription + ";NomineeIndex=" + aNomineeIndex + ";Quantity=" + s.GetNomineeWrkUnitsAllocated(aNomineeIndex));
                        //}
						_oldTotal += s.NodeRemainingToAllocate;
						_bse[_srt].SortKey = new double[4];
						_bse[_srt].Item = i;
                        //if (s.NodeType == eAllocationNode.DetailType)    // TT#488 - MD - JEllis - Urban Group Allocation
                        if (s.NodeType == eNeedAllocationNode.DetailType)  // TT#488 - MD - JEllis - Urban Group Allocation
						{
							_bse[_srt].SortKey[0] = 1;
						}
						else
						{
                            //if (s.NodeType == eAllocationNode.Bulk)    // TT#488 - MD - JEllis - Urban Group Allocation
                            if (s.NodeType == eNeedAllocationNode.Bulk)  // TT#488 - MD - JEllis - Urban Group Allocation
							{
								_bse[_srt].SortKey[0] = 0;
							}
							else
							{
								_bse[_srt].SortKey[0] = 2;
							}
						}
						// BEGIN MID Track # 2473 Color min/max not applied correctly
                        // _bse[_srt].SortKey[1] = s.NodeRemainingToAllocate;
                        //if (s.NodeType == eAllocationNode.BulkColor)  // TT#488 - MD - JEllis - Urban Group Allocation
                        if (s.NodeType == eNeedAllocationNode.BulkColor) // TT#488 - MD - JEllis - Urban Group Allocation
						{
							_bse[_srt].SortKey[1] = -s.NodeRemainingToAllocate;
						}
						else
						{
							_bse[_srt].SortKey[1] = s.NodeRemainingToAllocate;
						}
                        //int wrkUnitsAllocated = s.GetNomineeUnitsAvailable(aNomineeIndex);
						// END MID Track # 2473 
						_bse[_srt].SortKey[2] = s.NodeMultiple;
						_bse[_srt].SortKey[3] = _transaction.GetRandomDouble();
	    				_srt++;
					}
				}
				Array.Sort(_bse,0,_srt,new MIDGenericSortDescendingComparer());
				// Begin TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color
                //===========================================================================================
                // How this works for WUB/Placeholders with more than 1 subcomponent:
                // The reason it couldn't work previously is because the old total (_oldTotal) was always zero.
                // To get around this and to cause an even spread, the old total is set to the number of subcomponents (the value of _srt).
                // Later as each subcomponent is spread to, the old total is decremented by 1.
                //===========================================================================================
                if (_workUpBuy && _srt > 1)
                {
                    _oldTotal = _srt;
                }
				// End TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color
				int _newTotal = n.GetNomineeWrkUnitsAllocated(aNomineeIndex);
                int multiple;  // TT#1478 - Size Multple Broken
                // begin TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
                int nomineeMaximum;
                int nomineeMinimum;
                // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
				for (int i = 0; i < _srt; i++)
				{
					int _nodeIndex = _bse[i].Item;
					s = n.GetChildNodeComponent(n,_nodeIndex);
                    //if (aNomineeIndex == 61)
                    //{
                    //    Debug.WriteLine("After Sort: Keys=" + _bse[i].SortKey[0] + "|" + _bse[i].SortKey[1] + "|" + _bse[i].SortKey[2] + ";NodeType=" + s.NodeType + ";NodeDescription=" + s.NodeDescription + ";NomineeIndex=" + aNomineeIndex + ";Quantity=" + s.NodeRemainingToAllocate);
                    //}
                    // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    // get min and max before the effects of this subtotal's wrkUnitsAllocated
                    nomineeMaximum = s.GetNomineeMaximum(aNomineeIndex);
                    nomineeMinimum = s.GetNomineeMinimum(aNomineeIndex);
                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
					if (_oldTotal > 0)
					{
						// Begin TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color
                        if (_workUpBuy && _srt > 1)
                        {
                            // Determine this component's part of the total.
                            int wrkUnitsAllocated = (int)(((double)_newTotal / (double)_oldTotal) + .5d);
                            s.SetNomineeWrkUnitsAllocated(aNomineeIndex, wrkUnitsAllocated);
                        }
                        else
                        {
						// End TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color
                            int wrkUnitsAllocated = (int)((((double)_newTotal * (double)s.NodeRemainingToAllocate) / (double)_oldTotal) + .5d);
                            if (wrkUnitsAllocated > s.NodeRemainingToAllocate)
                            {
                                wrkUnitsAllocated = s.NodeRemainingToAllocate;
                            }
                            //int availUnits = s.GetNomineeUnitsAvailable(aNomineeIndex);  // Restore VERSION 3.2
                            if (wrkUnitsAllocated > s.GetNomineeUnitsAvailable(aNomineeIndex))
                            {
                                wrkUnitsAllocated = s.GetNomineeUnitsAvailable(aNomineeIndex);
                            }
                            s.SetNomineeWrkUnitsAllocated(aNomineeIndex, wrkUnitsAllocated);
                        }	// TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color
						
					}
					else
					{
						s.SetNomineeWrkUnitsAllocated(aNomineeIndex, 0);
                        // begin TT#51 Work Up Buy not following Need Limit
                        if (_workUpBuy
                            && _srt == 1)
                        {
                            s.SetNomineeWrkUnitsAllocated(aNomineeIndex, _newTotal);
                        }
                        // end MID TT#51 Work Up Buy not following Need Limit
                    }
                    // begin TT#1478 - Size Multiple Broken
                    //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                    //    (int)(((double)s.GetNomineeWrkUnitsAllocated(aNomineeIndex) / (double)s.NodeMultiple) + .5d));
                    //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                    //    (s.GetNomineeWrkUnitsAllocated(aNomineeIndex) * s.NodeMultiple));
                    multiple = s.GetNomineeMultiple(aNomineeIndex);
                    s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                        (int)(((double)s.GetNomineeWrkUnitsAllocated(aNomineeIndex) / (double)multiple) + .5d));
                    s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                        (s.GetNomineeWrkUnitsAllocated(aNomineeIndex) * multiple));
                    // end TT#1478 - Size Multiple Broken
					if (s.GetNomineeWrkUnitsAllocated(aNomineeIndex) >
						_newTotal)
					{
                        // begin TT#1478 - Size Multiple Broken
                        //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                        //    (int)(((double)_newTotal / (double) s.NodeMultiple)));
                        //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                        //    s.GetNomineeWrkUnitsAllocated(aNomineeIndex) * s.NodeMultiple);
                        s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                            (int)(((double)_newTotal / (double)multiple)));
                        s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                            s.GetNomineeWrkUnitsAllocated(aNomineeIndex) * multiple);
                        // end TT#1478 - Size Multiple Broken
					}
                    //if (s.GetNomineeDesiredAllocation(aNomineeIndex) > s.GetNomineeMaximum(aNomineeIndex)) // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
					if (s.GetNomineeDesiredAllocation(aNomineeIndex) > nomineeMaximum)        // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
					{
                        //if (s.GetNomineeUnitsAllocated(aNomineeIndex) >= s.GetNomineeMaximum(aNomineeIndex))  // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                        if (s.GetNomineeUnitsAllocated(aNomineeIndex) >= nomineeMaximum)    // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
						{
							s.SetNomineeWrkUnitsAllocated(aNomineeIndex, 0);
						}
						else
						{
							s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                                nomineeMaximum - s.GetNomineeUnitsAllocated(aNomineeIndex));   // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                            //s.GetNomineeMaximum(aNomineeIndex) - s.GetNomineeUnitsAllocated(aNomineeIndex)); // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
							// begin TT#1478 - Size Multiple Broken
                            //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                            //    (int)(((double)s.GetNomineeWrkUnitsAllocated(aNomineeIndex) / (double)s.NodeMultiple)));
                            //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                            //    (s.GetNomineeWrkUnitsAllocated(aNomineeIndex) * s.NodeMultiple));
                            s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                                (int)(((double)s.GetNomineeWrkUnitsAllocated(aNomineeIndex) / (double)multiple)));
                            s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
                                (s.GetNomineeWrkUnitsAllocated(aNomineeIndex) * multiple));
                            // end TT#1478 - Size Multiple Broken
						}
					}
                    //if (aNomineeIndex == 61)
                    //{
                    //    Debug.WriteLine("After Breakout: Keys=" + _bse[i].SortKey[0] + "|" + _bse[i].SortKey[1] + "|" + _bse[i].SortKey[2] + ";NodeType=" + s.NodeType + ";NodeDescription=" + s.NodeDescription + ";NomineeIndex=" + aNomineeIndex + ";Quantity=" + s.GetNomineeWrkUnitsAllocated(aNomineeIndex));
                    //}
					s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
						BreakoutProportional(s,aNomineeIndex));
                    //if (aNomineeIndex == 61)
                    //{
                    //    Debug.WriteLine("After Breakout2: Keys=" + _bse[i].SortKey[0] + "|" + _bse[i].SortKey[1] + "|" + _bse[i].SortKey[2] + ";NodeType=" + s.NodeType + ";NodeDescription=" + s.NodeDescription + ";NomineeIndex=" + aNomineeIndex + ";Quantity=" + s.GetNomineeWrkUnitsAllocated(aNomineeIndex));
                    //}
                    //if (s.GetNomineeDesiredAllocation(aNomineeIndex) < s.GetNomineeMinimum(aNomineeIndex)) // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    if (s.GetNomineeDesiredAllocation(aNomineeIndex) < nomineeMinimum)   // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
					{
                        // begin TT#946 - MD - Jellis - Group Allocation Not Working
                        //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,0);
                        ZeroOutNomineeWrkUnitsAllocated(s, aNomineeIndex);
                        // end TT#946 - MD - Jellis - Group Allocation Not Working
					}
					
					// Begin TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color
                    //=============================================================================
                    // This is where the old total is decremented by 1 for WUBs/Placeholders
                    // with more than 1 subcomponent.
                    //=============================================================================
                    _newTotal -= s.GetNomineeWrkUnitsAllocated(aNomineeIndex);
                    if (_workUpBuy && _srt > 1)
                    {
                        _oldTotal--;
                    }
                    else
                    {
                        // Normal decrement
                        _oldTotal -= s.NodeRemainingToAllocate;
                    }
					// End TT#1555-MD - stodd - Need cannot allocated WUB/Placeholder with more than 1 color

					_returnTotal += s.GetNomineeWrkUnitsAllocated(aNomineeIndex);
				}
			}
			else
			{
				_returnTotal = n.GetNomineeWrkUnitsAllocated(aNomineeIndex);
			}
			return _returnTotal;
		}

        // begin TT#946 - MD - Jellis - Group Allocation Not Working
        private void ZeroOutNomineeWrkUnitsAllocated(NodeComponent n, int aNomineeIndex)
        {
            n.SetNomineeWrkUnitsAllocated(aNomineeIndex, 0);
            if (n.AllocateSubNodes)
            {
                foreach (NodeComponent ns in n.SubNodeComponents)
                {
                    ZeroOutNomineeWrkUnitsAllocated(ns, aNomineeIndex);
                }
            }
        }
        // end TT#946 - MD - Jellis - Group Allocation Not Working
		#endregion Breakout sub nodes Proportionally

		#region Allocate Subnode Substitute
		/// <summary>
		/// Identifies a sub-node (pack or color) and allocates a multiple of it as a Minimum or Upto Maximum substitute.
		/// </summary>
		/// <param name="aNodeComponent">Node component containing the WrkUnitsAllocated to spread.</param>
		/// <param name="aNomineeIndex">Index that identifies the nominee.</param>
		/// <returns>Quantity Allocated</returns>
		private int AllocateSubnodeSubstitute(NodeComponent aNodeComponent, int aNomineeIndex, int aMultiple, bool aObserveMax)
		{
			int _returnTotal;
			if (aNodeComponent.AllocateSubNodes == true)
			{

				int sValue;
				MIDGenericSortItem[] _bse = new MIDGenericSortItem[aNodeComponent.SubnodeDimension];
				NodeComponent s;
				int _srt = 0;
                int multiple; // TT#1478 - Size Multiple Broken
                // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                // get min and max before the effects of any substitute allocation 
                int nomineeMaximum;
                int nomineeMinimum;
                // end   TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken
				for (int i = 0; i < aNodeComponent.SubnodeDimension; i++)
				{
					s = aNodeComponent.GetChildNodeComponent(aNodeComponent, i);
                    // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    nomineeMaximum = s.GetNomineeMaximum(aNomineeIndex);
                    nomineeMinimum = s.GetNomineeMinimum(aNomineeIndex);
                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    //sValue = MIDMath.LeastCommonMultiple(s.NodeMultiple, aMultiple);  // TT#1478 - Size Multiple Broken
                    multiple = s.GetNomineeMultiple(aNomineeIndex);                     // TT#1478 - Size Multiple Broken
                    sValue = MIDMath.LeastCommonMultiple(multiple, aMultiple);          // TT#1478 - Size Multiple Broken

                    // begin TT#946 - MD - Jellis - Group Allocation Not Working
                    //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,0);
                    ZeroOutNomineeWrkUnitsAllocated(s, aNomineeIndex);
                    // end TT#946 - MD - Jellis - Group Allocation Not Working
					int desiredAllocation = (s.GetNomineeUnitsAllocated(aNomineeIndex) + sValue); //  MID Track 6446 Need Action gets Index Out of Range

					// begin MID Track 5525 AnF Defect 1618: Rounding Error
					//_pctNeedThresh = 
					//	Need.PctUnitNeed
					//	(
					//	GetNomineeUnitNeed(aNomineeIndex) - sValue,
					//	GetNomineePlan(aNomineeIndex)
					//	);
					// end MID Track 5525 AnF Defect 1618: Rounding Error

					// begin MID Track 6446 Need Action gets Index Out of Range
					//if (s.GetNomineeIncludeNode(aNomineeIndex) == true
					//	&& s.NodeRemainingToAllocate >= sValue
					//	&& aNodeComponent.GetNomineeMinimum(i) <= desiredAllocation
					//	&& ((aObserveMax && desiredAllocation <= s.GetNomineeMaximum(i)) || !aObserveMax)
					//	//&& _pctNeedThresh >= this.PercentNeedLimit  // MID Track 5525 AnF Defect 1618: Rounding Error
					//	&& ((GetNomineeUnitNeed(aNomineeIndex) - sValue) >= _unitNeedLimit[i]) // MID Track 5525 AnF Defect 1618: Rounding Error
					//	&& sValue <= s.GetNomineeUnitsAvailable(aNomineeIndex))
					if (s.GetNomineeIncludeNode(aNomineeIndex) == true
						&& s.NodeRemainingToAllocate >= sValue
                        // begin TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken
                        && nomineeMinimum <= desiredAllocation
                        && ((aObserveMax && desiredAllocation <= nomineeMaximum) || !aObserveMax)
                        //&& aNodeComponent.GetNomineeMinimum(aNomineeIndex) <= desiredAllocation
                        //&& ((aObserveMax && desiredAllocation <= s.GetNomineeMaximum(aNomineeIndex)) || !aObserveMax)
                        // end   TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken
						//&& _pctNeedThresh >= this.PercentNeedLimit  // MID Track 5525 AnF Defect 1618: Rounding Error
						&& ((GetNomineeUnitNeed(aNomineeIndex) - sValue) >= _unitNeedLimit[aNomineeIndex]) // MID Track 5525 AnF Defect 1618: Rounding Error
						&& sValue <= s.GetNomineeUnitsAvailable(aNomineeIndex))
						// end MID Track 6446 Need Action gets Index Out of Range
					{
						_bse[_srt].SortKey = new double[4];
						_bse[_srt].Item = i;
                        //if (s.NodeType == eAllocationNode.DetailType)  // TT#488 - MD - JEllis - Urban Group Allocation
                        if (s.NodeType == eNeedAllocationNode.DetailType) // TT#488 - MD - JEllis - Urban Group Allocation
						{
							_bse[_srt].SortKey[0] = 1;
						}
						else
						{
                            //if (s.NodeType == eAllocationNode.Bulk)  // TT#488 - MD - JEllis - Urban Group Allocation
                            if (s.NodeType == eNeedAllocationNode.Bulk) // TT#488 - MD - JEllis - Urban Group Allocation
							{
								_bse[_srt].SortKey[0] = 0;
							}
							else
							{
								_bse[_srt].SortKey[0] = 2;
							}
						}
						_bse[_srt].SortKey[1] = s.NodeRemainingToAllocate;
						_bse[_srt].SortKey[2] = sValue;
						_bse[_srt].SortKey[3] = _transaction.GetRandomDouble();
						_srt++;
					}
				}
				if (_srt > 0)
				{
					Array.Sort(_bse,0,_srt,new MIDGenericSortDescendingComparer());
					int _nodeIndex = _bse[0].Item;
					s = aNodeComponent.GetChildNodeComponent(aNodeComponent,_nodeIndex);
                    // begin   TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken
                    nomineeMaximum = s.GetNomineeMaximum(aNomineeIndex);
                    nomineeMinimum = s.GetNomineeMinimum(aNomineeIndex);
                    // end   TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken

					s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
						(int)(_bse[0].SortKey[2]));
					s.SetNomineeWrkUnitsAllocated(aNomineeIndex,
						BreakoutProportional(s,aNomineeIndex));
                    //if (s.GetNomineeDesiredAllocation(aNomineeIndex) < s.GetNomineeMinimum(aNomineeIndex))  // TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken
                    if (s.GetNomineeDesiredAllocation(aNomineeIndex) < nomineeMinimum)  // TT#1074 - MD - Jellis - Group allocation Inventory Min Max Broken
					{
                        // begin TT#946 - MD - Jellis - Group Allocation Not Working
                        //s.SetNomineeWrkUnitsAllocated(aNomineeIndex,0);
                        ZeroOutNomineeWrkUnitsAllocated(s, aNomineeIndex);
                        // end TT#946 - MD - Jellis - Group Allocation Not Working
					}
					_returnTotal = s.GetNomineeWrkUnitsAllocated(aNomineeIndex);
				}
				else
				{
					_returnTotal = 0;
				}
			}
			else
			{
				_returnTotal = aNodeComponent.GetNomineeWrkUnitsAllocated(aNomineeIndex);
			}
			return _returnTotal;
		}
		#endregion Allocate Subnode Substitute

		#region Apply Desired Allocation
		internal void ApplyDesiredAllocation(NodeComponent aNodeComponent, int aNomineeIndex)
		{
			NodeComponent n = aNodeComponent;
			int nWrkUnitsAllocated = n.GetNomineeWrkUnitsAllocated(aNomineeIndex);
			n.SetNomineeUnitsAllocated(aNomineeIndex, 
				n.GetNomineeUnitsAllocated(aNomineeIndex) + nWrkUnitsAllocated);
			n.NodeUnitsAllocated += nWrkUnitsAllocated;
			if (n.GetNomineeUnitsAvailable(aNomineeIndex) < int.MaxValue)
			{
				n.SetNomineeUnitsAvailable(
					aNomineeIndex, 
					Math.Max(0, n.GetNomineeUnitsAvailable(aNomineeIndex) - nWrkUnitsAllocated));
			}
			if (n.AllocateSubNodes == true)
			{
				foreach (NodeComponent s in n.SubNodeComponents)
				{
					ApplyDesiredAllocation(s,aNomineeIndex);
				}
			}
			// begin MID Track 3810 Size Allocation GT Style Allocation
			//if (_workUpBuy)
			//{
			//	n.NodeUnitsToAllocate = n.NodeUnitsAllocated;
			//}
			// end MID Track 3810 Size Allocation GT Style Allocation
		}
		#endregion Apply Desired Allocation
		#endregion Methods
	}
	#endregion Need Algorithms
}