// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Windows.Forms;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Windows.Controls
{
	/// <summary>
	/// Summary description for MIDTreeNode.
	/// </summary>
	abstract public class MIDTreeNode : TreeNode, IComparable
	{
		//=======
		// FIELDS
		//=======

		//Begin Track #6201 - JScott - Store Count removed from attr sets
		const string cLeftCountString = " [";
		const string cRightCountString = "]";
		//End Track #6201 - JScott - Store Count removed from attr sets

		private SessionAddressBlock			_SAB;
		//Begin Track #6201 - JScott - Store Count removed from attr sets
		private string						_internalText;
		//End Track #6201 - JScott - Store Count removed from attr sets
		private eTreeNodeType _treeNodeType;
		protected Profile					_profile;
		private bool						_alwaysLoadedOnExpand;
		private int							_parentId;
		private int							_userId;
		//Begin Track #6321 - JScott - User has ability to to create folders when security is view
		//protected FunctionSecurityProfile	_functionSecurityProfile;
		protected MIDTreeNodeSecurityGroup	_nodeSecurityGroup;
		//End Track #6321 - JScott - User has ability to to create folders when security is view
		public  int _collapsedImageIndex;
        public int _selectedCollapsedImageIndex;
        public int _expandedImageIndex;
        public int _selectedExpandedImageIndex;
		private eChangeType					_nodeChangeType;
		private string						_nodeID;
		private bool						_bChildrenLoaded;
		private bool						_bAllowDrop;
		private DragDropEffects				_dragDropeffect;
		private string						_sMessage;
		private bool						_hasChildren;
		private bool						_displayChildren;
		private bool						_canBeDeleted;
        private bool						_bExpanded;
		private int							_savedImageIndex;
        private int							_ownerUserRID;
        private bool						_bNeedsRedrawn;
        private bool						_bShowTooltip;
		//Begin Track #6201 - JScott - Store Count removed from attr sets
		private bool						_bAddChildCountToText;
		//End Track #6201 - JScott - Store Count removed from attr sets
		private FolderDataLayer _dlFolder;

		//=============
		// CONSTRUCTORS
		//=============

        public MIDTreeNode()
		{
		}

		public MIDTreeNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			bool aAlwaysLoadedOnExpand)
		{
			ConstructorCommon();

			_SAB = aSAB;
			_treeNodeType = aTreeNodeType;
			_profile = aProfile;
			_alwaysLoadedOnExpand = aAlwaysLoadedOnExpand;
			_parentId = Include.NoRID;
			_userId = Include.NoRID;
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//_functionSecurityProfile = aFunctionSecurityProfile;
			_nodeSecurityGroup = aNodeSecurityGroup;
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			_collapsedImageIndex = -1;
			_selectedCollapsedImageIndex = -1;
			_expandedImageIndex = -1;
			_selectedExpandedImageIndex = -1;
		}

		public MIDTreeNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			bool aAlwaysLoadedOnExpand,
			string aText)
			: base(aText)
		{
			ConstructorCommon();

			_SAB = aSAB;
			//Begin Track #6201 - JScott - Store Count removed from attr sets
			_internalText = aText;
			//End Track #6201 - JScott - Store Count removed from attr sets
			_treeNodeType = aTreeNodeType;
			_profile = aProfile;
			_alwaysLoadedOnExpand = aAlwaysLoadedOnExpand;
			_parentId = Include.NoRID;
			_userId = Include.NoRID;
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//_functionSecurityProfile = aFunctionSecurityProfile;
			_nodeSecurityGroup = aNodeSecurityGroup;
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			_collapsedImageIndex = -1;
			_selectedCollapsedImageIndex = -1;
			_expandedImageIndex = -1;
			_selectedExpandedImageIndex = -1;
		}

		public MIDTreeNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			bool aAlwaysLoadedOnExpand,
			string aText,
			int ImageIndex,
			int SelectedImageIndex)
			: base(aText, ImageIndex, SelectedImageIndex)
		{
			ConstructorCommon();

			_SAB = aSAB;
			//Begin Track #6201 - JScott - Store Count removed from attr sets
			_internalText = aText;
			//End Track #6201 - JScott - Store Count removed from attr sets
			_treeNodeType = aTreeNodeType;
			_profile = aProfile;
			_alwaysLoadedOnExpand = aAlwaysLoadedOnExpand;
			_parentId = Include.NoRID;
			_userId = Include.NoRID;
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//_functionSecurityProfile = aFunctionSecurityProfile;
			_nodeSecurityGroup = aNodeSecurityGroup;
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			_collapsedImageIndex = ImageIndex;
			_selectedCollapsedImageIndex = SelectedImageIndex;
			_expandedImageIndex = ImageIndex;
			_selectedExpandedImageIndex = SelectedImageIndex;
		}

		public MIDTreeNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			bool aAlwaysLoadedOnExpand,
			string aText,
			int aParentId,
			int aUserId,
			int aImageIndex,
			int aSelectedImageIndex,
			int aOwnerUserId)
			: base(aText, aImageIndex, aSelectedImageIndex)
		{
			ConstructorCommon();

			_SAB = aSAB;
			//Begin Track #6201 - JScott - Store Count removed from attr sets
			_internalText = aText;
			//End Track #6201 - JScott - Store Count removed from attr sets
			_treeNodeType = aTreeNodeType;
			_profile = aProfile;
			_alwaysLoadedOnExpand = aAlwaysLoadedOnExpand;
			_parentId = aParentId;
			_userId = aUserId;
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//_functionSecurityProfile = aFunctionSecurityProfile;
			_nodeSecurityGroup = aNodeSecurityGroup;
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			_ownerUserRID = aOwnerUserId;
			_collapsedImageIndex = ImageIndex;
			_selectedCollapsedImageIndex = SelectedImageIndex;
			_expandedImageIndex = ImageIndex;
			_selectedExpandedImageIndex = SelectedImageIndex;
		}

		public MIDTreeNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			bool aAlwaysLoadedOnExpand,
			string aText,
			int aParentId,
			int aUserId,
			int aCollapsedImageIndex,
			int aSelectedCollapsedImageIndex,
			int aExpandedImageIndex,
			int aSelectedExpandedImageIndex,
			int aOwnerUserId)
			: base(aText, aCollapsedImageIndex, aSelectedCollapsedImageIndex)
		{
			ConstructorCommon();

			_SAB = aSAB;
			//Begin Track #6201 - JScott - Store Count removed from attr sets
			_internalText = aText;
			//End Track #6201 - JScott - Store Count removed from attr sets
			_treeNodeType = aTreeNodeType;
			_profile = aProfile;
			_alwaysLoadedOnExpand = aAlwaysLoadedOnExpand;
			_parentId = aParentId;
			_userId = aUserId;
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//_functionSecurityProfile = aFunctionSecurityProfile;
			_nodeSecurityGroup = aNodeSecurityGroup;
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			_ownerUserRID = aOwnerUserId;
			_collapsedImageIndex = aCollapsedImageIndex;
			_selectedCollapsedImageIndex = aSelectedCollapsedImageIndex;
			_expandedImageIndex = aExpandedImageIndex;
			_selectedExpandedImageIndex = aSelectedExpandedImageIndex;
		}

		private void ConstructorCommon()
		{
			_bChildrenLoaded = !_alwaysLoadedOnExpand;
			_bAllowDrop = true;
			_dragDropeffect = DragDropEffects.None;
			_bExpanded = false;
			_savedImageIndex = Include.Undefined;
			_ownerUserRID = Include.NoRID;
			_bNeedsRedrawn = false;
			_bShowTooltip = true;
			//Begin Track #6201 - JScott - Store Count removed from attr sets
			_bAddChildCountToText = false;
			//End Track #6201 - JScott - Store Count removed from attr sets
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #6201 - JScott - Store Count removed from attr sets
		/// <summary>
		/// Gets the displayable Text for the MIDTreeNode.  Can not be set.  Use InternalText to set the field.
		/// </summary>

		new public string Text
		{
			get
			{
				return base.Text;
			}
		}

		//Begin Track #6201 - JScott - Store Count removed from attr sets
		/// <summary>
		/// Gets the SessionAddressBlock.
		/// </summary>

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

        /// <summary>
        /// The data layer for folders
        /// </summary>
        public FolderDataLayer DlFolder
        {
            get
            {
                if (_dlFolder == null)
                {
                    _dlFolder = new FolderDataLayer();
                }
                return _dlFolder;
            }
        }

		//Begin Track #6201 - JScott - Store Count removed from attr sets
		public string InternalText
		{
			get
			{
				return _internalText;
			}
			set
			{
				_internalText = value;
				UpdateExternalText();
			}
		}

		//End Track #6201 - JScott - Store Count removed from attr sets
		/// <summary>
		/// Gets or sets the record id of node in the hierarchy.
		/// </summary>

		public int NodeRID
		{
			get
			{
				return _profile.Key;
			}
		}

		/// <summary>
		/// Returns the eTreeNodeType of this node
		/// </summary>

		public eTreeNodeType TreeNodeType
		{
			get
			{
				return _treeNodeType;
			}
			set
			{
				_treeNodeType = value;
			}
		}

		/// <summary>
		/// Gets the profile associated with the node.
		/// </summary>

		public Profile Profile
		{
			get
			{
				return _profile;
			}
            set
			{
                _profile = value;
			}
		}

		/// <summary>
		/// Gets or sets the ParentId of this node
		/// </summary>

		public int ParentId
		{
			get
			{
				return _parentId;
			}
			set
			{
				_parentId = value;
			}
		}

		/// <summary>
		/// Gets or sets the UserId of this node
		/// </summary>

		public int UserId
		{
			get
			{
				return _userId;
			}
			set
			{
				_userId = value;
			}
		}

		//Begin Track #6321 - JScott - User has ability to to create folders when security is view
		///// <summary>
		///// Gets the FunctionSecurityProfile of this node.
		///// </summary>

		//virtual public FunctionSecurityProfile FunctionSecurityProfile
		//{
		//    get
		//    {
		//        return _functionSecurityProfile;
		//    }
		//    set
		//    {
		//        _functionSecurityProfile = value;
		//    }
		//}
		/// <summary>
		/// Gets the FunctionSecurityProfile of this node.
		/// </summary>

		virtual public MIDTreeNodeSecurityGroup NodeSecurityGroup
		{
			get
			{
				return _nodeSecurityGroup;
			}
			set
			{
				_nodeSecurityGroup = value;
			}
		}

		/// <summary>
		/// Gets the FunctionSecurityProfile of this node.
		/// </summary>

		virtual public FunctionSecurityProfile FunctionSecurityProfile
		{
			get
			{
				if (_nodeSecurityGroup != null)
				{
					return _nodeSecurityGroup.FunctionSecurityProfile;
				}
				else
				{
					return null;
				}
			}
			set
			{
				if (_nodeSecurityGroup == null)
				{
					_nodeSecurityGroup = new MIDTreeNodeSecurityGroup();
				}

				_nodeSecurityGroup.FunctionSecurityProfile = value;
			}
		}

		/// <summary>
		/// Gets the FolderSecurityProfile of this node.
		/// </summary>

		virtual public FunctionSecurityProfile FolderSecurityProfile
		{
			get
			{
				if (_nodeSecurityGroup != null)
				{
					if (_nodeSecurityGroup.FolderSecurityProfile != null)
					{
						return _nodeSecurityGroup.FolderSecurityProfile;
					}
					else
					{
						return FunctionSecurityProfile;
					}
				}
				else
				{
					return null;
				}
			}
			set
			{
				if (_nodeSecurityGroup == null)
				{
					_nodeSecurityGroup = new MIDTreeNodeSecurityGroup();
				}

				_nodeSecurityGroup.FolderSecurityProfile = value;
			}
		}
		//End Track #6321 - JScott - User has ability to to create folders when security is view

		/// <summary>
		/// Gets or sets the change type for the node.
		/// </summary>

		public eChangeType NodeChangeType
		{
			get
			{
				return _nodeChangeType;
			}
			set
			{
				_nodeChangeType = value;
			}
		}

		/// <summary>
		/// Gets or sets the id of the node.
		/// </summary>

		public string NodeID
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
		/// Gets the collapsed ImageIndex
		/// </summary>

		public int CollapsedImageIndex
		{
			get
			{
				return _collapsedImageIndex;
			}
		}

		/// <summary>
		/// Gets the selected collapsed ImageIndex
		/// </summary>

		public int SelectedCollapsedImageIndex
		{
			get
			{
				return _selectedCollapsedImageIndex;
			}
		}

		/// <summary>
		/// Gets the expanded ImageIndex
		/// </summary>

		public int ExpandedImageIndex
		{
			get
			{
				return _expandedImageIndex;
			}
		}

		/// <summary>
		/// Gets the selected expanded ImageIndex
		/// </summary>

		public int SelectedExpandedImageIndex
		{
			get
			{
				return _selectedExpandedImageIndex;
			}
		}

		/// <summary>
		/// Gets or sets the flag to identify if the children for the node have been loaded.
		/// </summary>

		public bool ChildrenLoaded
		{
			get
			{
				return _bChildrenLoaded;
			}
			set
			{
				_bChildrenLoaded = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag to identify if the node will allow a drop.
		/// </summary>
		/// <value>The default is true</value>

		public bool AllowDrop
		{
			get
			{
				return _bAllowDrop;
			}
			set
			{
				_bAllowDrop = value;
			}
		}

		/// <summary>
		/// Gets or sets the eDropAction associated with the node.
		/// </summary>

		public DragDropEffects DropAction
		{
			get
			{
				return _dragDropeffect;
			}
			set
			{
				_dragDropeffect = value;
			}
		}

		/// <summary>
		/// Gets or sets a message associated with the node.
		/// </summary>

		public string Message
		{
			get
			{
				return _sMessage;
			}
			set
			{
				_sMessage = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag to identify if the node has children.
		/// </summary>

		public bool HasChildren
		{
			get
			{
                if (Nodes.Count != 0)
                {
                    return true;
                }
                else
                {
                    return _hasChildren;
                }
			}
            set
            {
                _hasChildren = value;
            }
		}

		/// <summary>
		/// Gets or sets the flag to identify if the children of the node are to be displayed.
		/// </summary>

		public bool DisplayChildren
		{
			get
			{
				return _displayChildren;
			}
			set
			{
				_displayChildren = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag to identify if the node can be deleted.
		/// </summary>

		public bool CanBeDeleted
		{
			get
			{
				return _canBeDeleted;
			}
			set
			{
				_canBeDeleted = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag to identify if the node should be expanded.
		/// </summary>

		public bool Expanded
		{
			get
			{
				return _bExpanded;
			}
			set
			{
				_bExpanded = value;
			}
		}

		/// <summary>
		/// Gets or sets the saved image index.
		/// </summary>

		public int SavedImageIndex
		{
			get
			{
				return _savedImageIndex;
			}
			set
			{
				_savedImageIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the key of the owner of the Method or Workflow.
		/// </summary>

		public int OwnerUserRID
		{
			get
			{
				return _ownerUserRID;
			}
			set
			{
				_ownerUserRID = value;
			}
		}

		/// <summary>
		/// Gets or sets a flag identifying if the node needs redrawn.
		/// </summary>

		public bool NeedsRedrawn
		{
			get
			{
				return _bNeedsRedrawn;
			}
			set
			{
				_bNeedsRedrawn = value;
			}
		}

		/// <summary>
		/// Gets or sets a flag identifying if the tooltip should display for this node.
		/// </summary>

		public bool ShowTooltip
		{
			get
			{
				return _bShowTooltip;
			}
			set
			{
				_bShowTooltip = value;
			}
		}

		//Begin Track #6201 - JScott - Store Count removed from attr sets
		public bool AddChildCountToText
		{
			get
			{
				return _bAddChildCountToText;
			}
			set
			{
				_bAddChildCountToText = value;
			}
		}

		//End Track #6201 - JScott - Store Count removed from attr sets
		/// <summary>
		/// Returns the eProfileType of the Profile contained in this node
		/// </summary>

		public eProfileType NodeProfileType
		{
			get
			{
				return _profile.ProfileType;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is an ObjectNode
		/// </summary>

		public bool isObject
		{
			get
			{
				return _treeNodeType == eTreeNodeType.ObjectNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is an FolderNode
		/// </summary>

		public bool isFolder
		{
			get
			{
				return _treeNodeType == eTreeNodeType.MainFavoriteFolderNode ||
					_treeNodeType == eTreeNodeType.MainSourceFolderNode ||
					_treeNodeType == eTreeNodeType.SubFolderNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is a MainFolderNode
		/// </summary>

		public bool isMainFolder
		{
			get
			{
				return _treeNodeType == eTreeNodeType.MainFavoriteFolderNode ||
					_treeNodeType == eTreeNodeType.MainSourceFolderNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is a MainFavoriteFolderNode
		/// </summary>

		public bool isMainFavoriteFolder
		{
			get
			{
				return _treeNodeType == eTreeNodeType.MainFavoriteFolderNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is a SubFolderNode
		/// </summary>

		public bool isSubFolder
		{
			get
			{
				return _treeNodeType == eTreeNodeType.SubFolderNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is a shortcut node
		/// </summary>

		public bool isShortcut
		{
			get
			{
				return _treeNodeType == eTreeNodeType.FolderShortcutNode ||
					_treeNodeType == eTreeNodeType.ChildFolderShortcutNode ||
                    _treeNodeType == eTreeNodeType.ChildObjectShortcutNode ||
					_treeNodeType == eTreeNodeType.ObjectShortcutNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is a FolderShortcutNode
		/// </summary>

		public bool isFolderShortcut
		{
			get
			{
				return _treeNodeType == eTreeNodeType.FolderShortcutNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is a ChildShortcutNode
		/// </summary>

		public bool isChildShortcut
		{
			get
			{
				return _treeNodeType == eTreeNodeType.ChildFolderShortcutNode ||
                    _treeNodeType == eTreeNodeType.ChildObjectShortcutNode;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this node is an ObjectShortcutNode
		/// </summary>

		public bool isObjectShortcut
		{
			get
			{
				return _treeNodeType == eTreeNodeType.ObjectShortcutNode;
			}
		}

        /// <summary>
        /// Returns a boolean indicating if this node is a ChildObjectShortcutNode
        /// </summary>
        /// 
        public bool isChildObjectShortcut
        {
            get
            {
                return _treeNodeType == eTreeNodeType.ChildObjectShortcutNode;
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this node is a ChildFolderShortcutNode
        /// </summary>
        /// 
        public bool isChildFolderShortcut
        {
            get
            {
                return _treeNodeType == eTreeNodeType.ChildFolderShortcutNode;
            }
        }

        // Begin Track #6202 - JSmith - select not allowed
        /// <summary>
        /// Returns a boolean indicating if this MIDTreeNode can be accessed by the user
        /// </summary>

        virtual public bool isAccessAllowed
        {
            get
            {
                if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
                {
                    return false;
                }
                return true;
            }
        }
        // End Track #6202

        /// <summary>
        /// Returns a boolean indicating if this node was shared by another user
        /// </summary>

        public bool isShared
        {
            get
            {
                return _userId != _ownerUserRID;
            }
        }

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode can be selected
		/// </summary>
		/// <param name="aMultiSelect">
		/// A boolean indication if multiselect is being performed.
		/// </param>
		/// <param name="aSelectedNodes">
		/// An ArrayList of the currently selected nodes.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode can be selected
		/// </returns>

		abstract public bool isSelectAllowed(bool aMultiSelect, ArrayList aSelectedNodes);

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode is draggable for a given DragDropEffects
		/// </summary>
		/// <param name="aDragDropEffects">
		/// The current DragDropEffects type being processed.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode is draggable for a given DragDropEffects
		/// </returns>

		abstract public bool isDragAllowed(DragDropEffects aCurrentEffect);

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode can be dropped in the given MIDTreeNode
		/// </summary>
		/// <param name="aDropAction">
		/// The DragDropEffects that is being processed.
		/// </param>
		/// <param name="aSelectedNode">
		/// The MIDTreeNode that this node is being dragged over.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode can be dropped in the given MIDTreeNode
		/// </returns>

		abstract public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDestinationNode);

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode's label can be edited
		/// </summary>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode's label can be edited
		/// </returns>

		abstract public bool isLabelEditAllowed();

        /// <summary>
        /// Virtual method that indicates if this MIDTreeNode's children are to be sorted
        /// </summary>
        /// <returns>
        /// A boolean indicating if this MIDTreeNode's children are to be sorted
        /// </returns>

        virtual public bool isChildrenSorted()
        {
            return true;
        }

		/// <summary>
		/// Abstract method that refreshes the shortcut node
		/// </summary>

		abstract public void RefreshShortcutNode(MIDTreeNode aChangedNode);

		/// <summary>
		/// Retrieves a list of children for the node
		/// </summary>
		/// <returns>An ArrayList containing profiles for each child</returns>

		virtual public void BuildChildren()
		{
		}

		//Begin Track #6201 - JScott - Store Count removed from attr sets
		/// <summary>
		/// Retrieves a count of the number of child nodes
		/// </summary>
		/// <returns>An ArrayList containing profiles for each child</returns>

		virtual public int GetChildCount()
		{
			try
			{
				return Nodes.Count;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #6201 - JScott - Store Count removed from attr sets
		virtual public int CompareTo(object obj)
		{
            int compareValue;
			try
			{
				compareValue = string.Compare(this.Text, ((MIDTreeNode)obj).Text);
				if (compareValue == 0)
                {
                    if (Profile.ProfileType < ((MIDTreeNode)obj).Profile.ProfileType)
                    {
                        return -1;
                    }
                    else if (Profile.ProfileType > ((MIDTreeNode)obj).Profile.ProfileType)
                    {
                        return 1;
                    }
                    else
                    {
                        if (UserId < ((MIDTreeNode)obj).UserId)
                        {
                            return -1;
                        }
                        else if (UserId > ((MIDTreeNode)obj).UserId)
                        {
                            return 1;
                        }
                        else
                        {
                            if (Profile.Key < ((MIDTreeNode)obj).Profile.Key)
                            {
                                return -1;
                            }
                            else if (Profile.Key > ((MIDTreeNode)obj).Profile.Key)
                            {
                                return 1;
                            }
                            else 
                            {
                                // should never get here, but set order anyway
                                return -1;
                            }
                        }
                    }
                }

                return compareValue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public object Clone()
		{
			MIDTreeNode tn;

			try
			{
                tn = (MIDTreeNode)base.Clone();

                tn._SAB = _SAB;
				//Begin Track #6201 - JScott - Store Count removed from attr sets
				tn._internalText = _internalText;
				//End Track #6201 - JScott - Store Count removed from attr sets
				tn._treeNodeType = _treeNodeType;
                tn._profile = _profile;
				tn._alwaysLoadedOnExpand = _alwaysLoadedOnExpand;
                tn._parentId = _parentId;
                tn._userId = _userId;
				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//if (_functionSecurityProfile != null)
				//{
				//    tn._functionSecurityProfile = (FunctionSecurityProfile)_functionSecurityProfile.Clone();
				//}
				if (_nodeSecurityGroup != null)
				{
					tn._nodeSecurityGroup = (MIDTreeNodeSecurityGroup)_nodeSecurityGroup.Clone();
				}
				//End Track #6321 - JScott - User has ability to to create folders when security is view
				tn._collapsedImageIndex = _collapsedImageIndex;
				tn._selectedCollapsedImageIndex = _selectedCollapsedImageIndex;
				tn._expandedImageIndex = _expandedImageIndex;
				tn._selectedExpandedImageIndex = _selectedExpandedImageIndex;
				tn._nodeChangeType = _nodeChangeType;
				tn._nodeID = _nodeID;
				tn._bChildrenLoaded = _bChildrenLoaded;
				tn._bAllowDrop = _bAllowDrop;
				tn._dragDropeffect = _dragDropeffect;
				tn._sMessage = _sMessage;
				tn._hasChildren = _hasChildren;
				tn._displayChildren = _displayChildren;
				tn._canBeDeleted = _canBeDeleted;
                tn._bExpanded = _bExpanded;
				tn._savedImageIndex = _savedImageIndex;
                tn._ownerUserRID = _ownerUserRID;
                tn._bNeedsRedrawn = _bNeedsRedrawn;
                tn._bShowTooltip = _bShowTooltip;

				return tn;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        virtual public object CloneNode(Profile aProfile)
        {
			MIDTreeNode tn;

            try
            {
                tn = (MIDTreeNode)Clone();
				tn._profile = aProfile;

                return tn;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		/// <summary>
		/// Method that determines if this TreeNode is a child of the given TreeNode
		/// </summary>
		/// <param name="baseNode">
		/// The parent TreeNode to search.
		/// </param>
		/// <returns>
		/// A boolean indicating if this TreeNode is a child of the given TreeNode
		/// </returns>

		public bool isChildOf(TreeNode baseNode)
		{
			try
			{
				if (baseNode.Nodes.Contains(this))
				{
					return true;
				}

				foreach (TreeNode node in baseNode.Nodes)
				{
					if (isChildOf(node))
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetExpandImage()
		{
			try
			{
				this.ImageIndex = _expandedImageIndex;
				this.SelectedImageIndex = _selectedExpandedImageIndex;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#21 - JSmith - Folder colors do not change when updated in Hierarchy Properties
        public void SetExpandImageIndexes(int aExpandedImageIndex, int aSelectedExpandedImageIndex)
        {
            try
            {
                _expandedImageIndex = aExpandedImageIndex;
                _selectedExpandedImageIndex = aSelectedExpandedImageIndex;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#21

		public void SetCollapseImage()
		{
			try
			{
				this.ImageIndex = _collapsedImageIndex;
				this.SelectedImageIndex = _selectedCollapsedImageIndex;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#21 - JSmith - Folder colors do not change when updated in Hierarchy Properties
        public void SetCollapseImageIndexes(int aCollapsedImageIndex, int aSelectedCollapsedImageIndex)
        {
            try
            {
                _collapsedImageIndex = aCollapsedImageIndex;
                _selectedCollapsedImageIndex = aSelectedCollapsedImageIndex;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#21
		
		public MIDTreeNode GetParentNode()
        {
            try
            {
                if (this.Parent != null)
                {
                    return (MIDTreeNode)this.Parent;
                }
                else
                {
                    return this;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		public MIDTreeNode GetTopNode()
		{
			MIDTreeNode node;

			try
			{
				node = this;

				while (node.Parent != null)
				{
					node = (MIDTreeNode)node.Parent;
				}

				return node;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDTreeNode GetTopSourceNode()
		{
			MIDTreeNode node;

			try
			{
				node = this;

				while (node.Parent != null && ((MIDTreeNode)node.Parent).TreeNodeType != eTreeNodeType.MainNonSourceFolderNode)
				{
					node = (MIDTreeNode)node.Parent;
				}

				return node;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public eProfileType GetParentNodeType()
        {
            try
            {
                return GetParentNode().Profile.ProfileType;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        protected string GetUserName(int aUserRID)
        {
            try
            {
                switch (aUserRID)
                {
                    case Include.GlobalUserRID:
                        return "Global";

                    default:
                        return SAB.ClientServerSession.GetUserName(aUserRID);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		//Begin Track #6201 - JScott - Store Count removed from attr sets

		public void UpdateExternalText()
		{
			try
			{
				base.Text = _internalText;

				if (AddChildCountToText)
				{
                    //Begin TT#828 - JSmith - Renaming Attribute Set retains [#] of Store in naming convention
                    // strip of count before rename
                    int index1 = base.Text.LastIndexOf(cLeftCountString.Trim());
                    int index2;
                    string val;
                    if (index1 > 0)
                    {
                        index2 = base.Text.LastIndexOf(cRightCountString);
                        if (index2 > index1)
                        {
                            val = base.Text.Substring(index1 + 1, index2 - index1 - 1).Trim();
                            try
                            {
                                int ival = Convert.ToInt32(val);
                                base.Text = base.Text.Substring(0, index1).Trim();
                            }
                            catch
                            {
                            }
                        }
                    }
                    //End TT#828
						
					base.Text += cLeftCountString + GetChildCount() + cRightCountString;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6201 - JScott - Store Count removed from attr sets
	}
	//Begin Track #6321 - JScott - User has ability to to create folders when security is view

	public class MIDTreeNodeSecurityGroup : ICloneable
	{
		//=======
		// FIELDS
		//=======

		private FunctionSecurityProfile _functionSecurityProfile;
		private FunctionSecurityProfile _folderSecurityProfile;

		//=============
		// CONSTRUCTORS
		//=============

		public MIDTreeNodeSecurityGroup()
		{
			_functionSecurityProfile = null;
			_folderSecurityProfile = null;
		}

		public MIDTreeNodeSecurityGroup(FunctionSecurityProfile aFunctionSecurityProfile, FunctionSecurityProfile aFolderSecurityProfile)
		{
			_functionSecurityProfile = aFunctionSecurityProfile;
			_folderSecurityProfile = aFolderSecurityProfile;
		}

		//===========
		// PROPERTIES
		//===========

		public FunctionSecurityProfile FunctionSecurityProfile
		{
			get
			{
				return _functionSecurityProfile;
			}
			set
			{
				_functionSecurityProfile = value;
			}
		}

		public FunctionSecurityProfile FolderSecurityProfile
		{
			get
			{
				return _folderSecurityProfile;
			}
			set
			{
				_folderSecurityProfile = value;
			}
		}

		//========
		// METHODS
		//========

		public object Clone()
		{
			MIDTreeNodeSecurityGroup newSecGrp;

			try
			{
				newSecGrp = new MIDTreeNodeSecurityGroup();

				if (_functionSecurityProfile != null)
				{
					newSecGrp._functionSecurityProfile = (FunctionSecurityProfile)_functionSecurityProfile.Clone();
				}

				if (_folderSecurityProfile != null)
				{
					newSecGrp._folderSecurityProfile = (FunctionSecurityProfile)_folderSecurityProfile.Clone();
				}

				return newSecGrp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	//End Track #6321 - JScott - User has ability to to create folders when security is view
}
