using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//Begin Track #4387 - JSmith - Merchandise Explorer performance
using MID.MRS.Business;
//End Track #4387
using MID.MRS.Common;
using MID.MRS.DataCommon;


namespace MID.MRS.Windows
{
	/// <summary>
	/// Used as a node in the treeview for the Merchandise Explorer
	/// </summary>
	public class MIDTreeNode : TreeNode
	{
		//  If new fields are added, the clone method below must also be changed
		private eChangeType					_nodeChangeType;
		private eHierarchyNodeType			_nodeType;
//		private string						_nodePath;
		private eHierarchyDisplayOptions	_displayOption;
		private string						_nodeID;
		private string						_nodeName;
		private string						_nodeDescription;
		private string						_nodeColor;
		private int							_nodeLevel;
		private int							_hierarchyRID;
		private eHierarchyType				_hierarchyType;
		private int							_homeHierarchyRID;
		private int							_homeHierarchyLevel;
		private int							_homeHierarchyParentRID;
		private eHierarchyType				_homeHierarchyType;
		private ArrayList					_parents;
		private int							_nodeRID;
		private bool						_hasChildren;
		private bool						_displayChildren;
		private bool						_childrenLoaded;
		private bool						_expanded;
		private bool						_canBeDeleted;
//		private bool						_isMyMerchandise;
		private HierarchyNodeSecurityProfile		_nodeSecurity;
		private FunctionSecurityProfile		_functionSecurity;
//Begin Track #4387 - JSmith - Merchandise Explorer performance
//		private eProductType				_productType;
		SessionAddressBlock					_SAB;
//End Track #4387
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private int							_ownerUserRID;
		//End Track #4815

//Begin Track #4387 - JSmith - Merchandise Explorer performance
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public MIDTreeNode() : base()
		{
			HasChildren = false;
			ChildrenLoaded = false;
			Expanded = false;
			CanBeDeleted = false;
			_displayOption = eHierarchyDisplayOptions.NameOnly;
			_nodeType = eHierarchyNodeType.TreeNode;
			_nodeSecurity = null;
			_functionSecurity = null;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			_ownerUserRID = Include.NoRID;
			//End Track #4815
		}
//
//		/// <summary>
//		/// Used to construct an instance of the class.
//		/// </summary>
//		/// <param name="Text"></param>
//		public MIDTreeNode(string Text) : base( Text )
//		{
//			HasChildren = false;
//			ChildrenLoaded = false;
//			Expanded = false;
//			CanBeDeleted = false;
//			//			IsMyMerchandise = false;
//			_displayOption = eHierarchyDisplayOptions.NameOnly;
//			_nodeType = eHierarchyNodeType.TreeNode;
//		}
//
//		/// <summary>
//		/// Used to construct an instance of the class.
//		/// </summary>
//		/// <param name="Text"></param>
//		/// <param name="ImageIndex"></param>
//		/// <param name="SelectedImageIndex"></param>
//		public MIDTreeNode(string Text, int ImageIndex, int SelectedImageIndex) : base( Text, ImageIndex, SelectedImageIndex )
//		{
//			HasChildren = false;
//			ChildrenLoaded = false;
//			Expanded = false;
//			CanBeDeleted = false;
//			//			IsMyMerchandise = false;
//			_displayOption = eHierarchyDisplayOptions.NameOnly;
//			_nodeType = eHierarchyNodeType.TreeNode;
//		}
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public MIDTreeNode(SessionAddressBlock aSAB) : base()
		{
//			HasChildren = false;
//			ChildrenLoaded = false;
//			Expanded = false;
//			CanBeDeleted = false;
//			IsMyMerchandise = false;
//			_displayOption = eHierarchyDisplayOptions.NameOnly;
//			_nodeType = eHierarchyNodeType.TreeNode;
			CommonLoad(aSAB);
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		/// <param name="Text"></param>
		public MIDTreeNode(string Text, SessionAddressBlock aSAB) : base( Text )
		{
//			HasChildren = false;
//			ChildrenLoaded = false;
//			Expanded = false;
//			CanBeDeleted = false;
//			IsMyMerchandise = false;
//			_displayOption = eHierarchyDisplayOptions.NameOnly;
//			_nodeType = eHierarchyNodeType.TreeNode;
			CommonLoad(aSAB);
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="ImageIndex"></param>
		/// <param name="SelectedImageIndex"></param>
		public MIDTreeNode(string Text, int ImageIndex, int SelectedImageIndex, SessionAddressBlock aSAB) : base( Text, ImageIndex, SelectedImageIndex )
		{
//			HasChildren = false;
//			ChildrenLoaded = false;
//			Expanded = false;
//			CanBeDeleted = false;
//			IsMyMerchandise = false;
//			_displayOption = eHierarchyDisplayOptions.NameOnly;
//			_nodeType = eHierarchyNodeType.TreeNode;
			CommonLoad(aSAB);
		}

		private void CommonLoad(SessionAddressBlock aSAB)
		{
			HasChildren = false;
			ChildrenLoaded = false;
			Expanded = false;
			CanBeDeleted = false;
			_displayOption = eHierarchyDisplayOptions.NameOnly;
			_nodeType = eHierarchyNodeType.TreeNode;
			_nodeSecurity = null;
			_functionSecurity = null;
			_SAB = aSAB;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			_ownerUserRID = Include.NoRID;
			//End Track #4815
		}
//End Track #4387

		/// <summary>
		/// Gets or sets the change type for the node.
		/// </summary>
		public eChangeType NodeChangeType 
		{
			get { return _nodeChangeType ; }
			set { _nodeChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the type of node.
		/// </summary>
		public eHierarchyNodeType NodeType 
		{
			get { return _nodeType ; }
			set { _nodeType = value; }
		}
		/// <summary>
		/// Gets or sets the display option of the node.
		/// </summary>
		public eHierarchyDisplayOptions DisplayOption 
		{
			get { return _displayOption ; }
			set { _displayOption = value; }
		}
		/// <summary>
		/// Gets or sets the id of the child.
		/// </summary>
		public string NodeID 
		{
			get { return _nodeID ; }
			set { _nodeID = value; }
		}
		/// <summary>
		/// Gets or sets the name of the child.
		/// </summary>
		public string NodeName 
		{
			get { return _nodeName ; }
			set { _nodeName = value; }
		}
		/// <summary>
		/// Gets or sets the description of the child.
		/// </summary>
		public string NodeDescription 
		{
			get { return _nodeDescription ; }
			set { _nodeDescription = value; }
		}
		/// <summary>
		/// Gets or sets the folder color for the node.
		/// </summary>
		public string NodeColor 
		{
			get { return _nodeColor ; }
			set { _nodeColor = value; }
		}
		/// <summary>
		/// Gets or sets the relative level for the node in the hierarchy in the current path.
		/// </summary>
		public int NodeLevel 
		{
			get { return _nodeLevel ; }
			set { _nodeLevel = value; }
		}
		/// <summary>
		/// Gets or sets the relative level for the node in the hierarchy in the home path.
		/// </summary>
		public int HomeHierarchyLevel 
		{
			get { return _homeHierarchyLevel ; }
			set { _homeHierarchyLevel = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the hierarchy where the node is located.
		/// </summary>
		public int HierarchyRID 
		{
			get { return _hierarchyRID ; }
			set { _hierarchyRID = value; }
		}
		/// <summary>
		/// Gets or sets the type of hierarchy where the node is located.
		/// </summary>
		public eHierarchyType HierarchyType 
		{
			get { return _hierarchyType ; }
			set { _hierarchyType = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the home hierarchy for the node.
		/// </summary>
		public int HomeHierarchyRID 
		{
			get { return _homeHierarchyRID ; }
			set { _homeHierarchyRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id of the parent node in the hierarchy.
		/// </summary>
		public int HomeHierarchyParentRID 
		{
			get { return _homeHierarchyParentRID ; }
			set { _homeHierarchyParentRID = value; }
		}
		/// <summary>
		/// Gets or sets the type of hierarchy where the node is located.
		/// </summary>
		public eHierarchyType HomeHierarchyType 
		{
			get { return _homeHierarchyType ; }
			set { _homeHierarchyType = value; }
		}
		/// <summary>
		/// Gets or sets the record id(s) of the parent node(s) in the hierarchy.
		/// </summary>
		public ArrayList Parents
		{
			get { return _parents ; }
			set { _parents = value; }
		}
		/// <summary>
		/// Gets or sets the record id of node in the hierarchy.
		/// </summary>
		public int NodeRID
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the node has children.
		/// </summary>
		public bool HasChildren
		{
			get { return _hasChildren ; }
			set { _hasChildren = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the children of the node are to be displayed.
		/// </summary>
		public bool DisplayChildren
		{
			get { return _displayChildren ; }
			set { _displayChildren = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the children for the node have been loaded.
		/// </summary>
		public bool ChildrenLoaded
		{
			get { return _childrenLoaded ; }
			set { _childrenLoaded = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the node has been expanded.
		/// </summary>
		/// <remarks>
		/// This is used during changes and during refresh to know which nodes should remain expanded</remarks>
		public bool Expanded
		{
			get { return _expanded ; }
			set { _expanded = value; }
		}
		/// <summary>
		/// Gets or sets the flag to identify if the node can be deleted.
		/// </summary>
		public bool CanBeDeleted
		{
			get { return _canBeDeleted ; }
			set { _canBeDeleted = value; }
		}
//		/// <summary>
//		/// Gets or sets the flag to identify if the node is the root for a personal hierarchy.
//		/// </summary>
//		public bool IsMyMerchandise
//		{
//			get { return _isMyMerchandise ; }
//			set { _isMyMerchandise = value; }
//		}
		/// <summary>
		/// Gets or sets the security profile of the product for the user.
		/// </summary>
		public HierarchyNodeSecurityProfile NodeSecurity
		{
//Begin Track #4387 - JSmith - Merchandise Explorer performance
//			get { return _nodeSecurity ; }
			get 
			{ 
				if (_nodeSecurity == null)
				{
					SetSecurity();
				}
				return _nodeSecurity ; 
			}
//End Track #4387
			set { _nodeSecurity = value; }
		}
		/// <summary>
		/// Gets or sets the security profile of the nodes function for the product for the user.
		/// </summary>
		public FunctionSecurityProfile NodesFunctionSecurity
		{
//Begin Track #4387 - JSmith - Merchandise Explorer performance
//			get { return _functionSecurity ; }
			get 
			{ 
				if (_functionSecurity == null)
				{
					SetSecurity();
				}
				return _functionSecurity ; 
			}
//End Track #4387
			set { _functionSecurity = value; }
		}
//Begin Track #4387 - JSmith - Merchandise Explorer performance
//		/// <summary>
//		/// Gets or sets the type of the product.
//		/// </summary>
//		public eProductType ProductType
//		{
//			get { return _productType ; }
//			set { _productType = value; }
//		}
//End Track #4387
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		/// <summary>
		/// Gets or sets the key of the owner of the Method or Workflow.
		/// </summary>
		public int OwnerUserRID
		{
			get { return _ownerUserRID ; }
			set { _ownerUserRID = value; }
		}
		//End Track #4815

//Begin Track #4387 - JSmith - Merchandise Explorer performance
		private void SetSecurity ()
		{
			_nodeSecurity = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(NodeRID, (int)eSecurityTypes.All);

			int hierarchyOwner = _SAB.HierarchyServerSession.GetHierarchyOwner(HomeHierarchyRID);
			if (HierarchyType == eHierarchyType.organizational)
			{
				_functionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(NodeRID, eSecurityFunctions.AdminHierarchiesOrgNodes, (int)eSecurityTypes.All);
			}
			else if (hierarchyOwner == Include.GlobalUserRID)	
			{
				_functionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(NodeRID, eSecurityFunctions.AdminHierarchiesAltNodes, (int)eSecurityTypes.All);
			}
			else 
			{
				_functionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(NodeRID, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
			}
		}
//End Track #4387
		
		/// <summary>
		/// Used to clone or copy a node.
		/// </summary>
		/// <remarks>
		/// This method must be change when fields are added or removed from the class.
		/// </remarks>
		public MIDTreeNode CloneNode(SessionAddressBlock aSAB) 
		{
			MIDTreeNode mtn = (MIDTreeNode) base.Clone();
			mtn._SAB = aSAB;
			mtn._nodeChangeType = this._nodeChangeType;
//			mtn._nodePath = this._nodePath;
			mtn._displayOption = this._displayOption;
			mtn._nodeID = this._nodeID;
			mtn._nodeName = this._nodeName;
			mtn._nodeDescription = this._nodeDescription;
			mtn._nodeColor = this._nodeColor;
			mtn._nodeLevel = this._nodeLevel;
			mtn._hierarchyRID = this._hierarchyRID;
			mtn._hierarchyType = this._hierarchyType;
			mtn._homeHierarchyParentRID = this._homeHierarchyParentRID;
			mtn._parents = this._parents;
			mtn._nodeRID = this._nodeRID;
			mtn._hasChildren = this._hasChildren;
			mtn._childrenLoaded = this._childrenLoaded;
			mtn._expanded = this._expanded;
			mtn._canBeDeleted = this._canBeDeleted;
//			mtn._isMyMerchandise = this._isMyMerchandise;
			mtn._homeHierarchyLevel = this._homeHierarchyLevel;
			mtn._homeHierarchyRID = this._homeHierarchyRID;
			mtn._homeHierarchyType = this._homeHierarchyType;
//Begin Track #4387 - JSmith - Merchandise Explorer performance
//			mtn._nodeSecurity = this._nodeSecurity;
//			mtn._functionSecurity = this._functionSecurity;
			mtn._nodeSecurity = null;
			mtn._functionSecurity = null;
//End Track #4387
			mtn._nodeType = this._nodeType;
//Begin Track #4387 - JSmith - Merchandise Explorer performance
//			mtn._productType = this._productType;
//End Track #4387
			mtn._displayChildren = this._displayChildren;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			mtn._ownerUserRID = this._ownerUserRID;
		    //End Track #4815
			return mtn;
		}

//		public static bool operator ==(MIDTreeNode left, MIDTreeNode right)
//		{
//			return left.Text == right.Text;
//		}
//		public static bool operator !=(MIDTreeNode left, MIDTreeNode right)
//		{
//			return left.Text != right.Text;
//		}
//
//		public int CompareTo (object o)
//		{
//			if (IsMyMerchandise == true)
//				return 1;
//			else
//			if (((MIDTreeNode)o).IsMyMerchandise == true)
//				return 1;
//			else
//			if (Text == ((MIDTreeNode)o).Text)
//				return 1;
//			else
//				return 0;
//		}

//		public override Sort (object o)
//		{
//			if (IsMyMerchandise == true)
//				return 0;
//			else
//				if (((MIDTreeNode)o).IsMyMerchandise == true)
//				return 0;
//			else
//				if (Text == ((MIDTreeNode)o).Text)
//				return 1;
//			else
//				return 0;
//		}

	}

}
