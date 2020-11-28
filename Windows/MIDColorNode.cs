// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	public class ColorTreeView : MIDTreeView
	{
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// For designer.
		/// </summary>
		public ColorTreeView()
		{
		}

		override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ClipboardListBase aSelectedNodes)
		{
			ArrayList nodes;

			try
			{
                if (aSelectedNodes.ClipboardDataType == eProfileType.ColorCode)
				{
                    nodes = new ArrayList();
                    foreach (TreeNodeClipboardProfile item in aSelectedNodes.ClipboardItems)
                    {
                        nodes.Add(GetColorNode(item));
                    }

                    return isDropAllowed(aDropAction, aDropNode, nodes);

				}

				return false;
			}
			catch
			{
				throw;
			}
		}
		
        override public bool isAllowedDataType(eProfileType aClipboardDataType)
        {
            return aClipboardDataType == eProfileType.ColorCode;
        }

        private MIDColorNode GetColorNode(TreeNodeClipboardProfile aClipboardProfile)
		{
			try
			{
                MIDColorNode treeNode = (MIDColorNode)FindTreeNode(this.Nodes, aClipboardProfile.Node.Profile.ProfileType, aClipboardProfile.Key);
				if (treeNode == null)
				{
					return BuildColorNode(aClipboardProfile.Key);
				}
				else
				{
					return treeNode;
				}
			}
			catch
			{
				throw;
			}
		}

        public MIDColorNode BuildColorGroupNode(string aColorGroupName, int akey)
		{
            ColorGroupProfile cgp;
            int collapsedImageIndex, expandedImageIndex;

			try
			{
                collapsedImageIndex = GetFolderImageIndex(MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(MIDGraphics.OpenFolder);

                cgp = new ColorGroupProfile(akey);
                cgp.ColorGroupName = aColorGroupName;

                MIDColorNode node = new MIDColorNode(
                    SAB,
                    eTreeNodeType.ObjectNode,
                    cgp,
                    cgp.ColorGroupName,
                    Include.NoRID,
                    Include.SystemUserRID,
                    SetSecurity(cgp.Key),
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    Include.SystemUserRID
                    );

				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//node.Text = aColorGroupName;
				node.InternalText = aColorGroupName;
				//End Track #6201 - JScott - Store Count removed from attr sets
				node.NodeID = string.Empty;
				node.NodeName = string.Empty;
				node.ColorGroup = aColorGroupName;
				node.NodeChangeType = eChangeType.none;

                node.HasChildren = true;
				node.DisplayChildren = true;

				if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
				}

				if (node.HasChildren
					&& node.DisplayChildren)
				{
                    node.Nodes.Add(new MIDColorNode());
					node.ChildrenLoaded = false;
				}
				else
				{
					node.ChildrenLoaded = true;
				}
				if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
				}

				return node;
			}
			catch
			{
				throw;
			}
		}

        public MIDColorNode BuildColorNode(int aNodeRID)
		{
            int collapsedImageIndex, expandedImageIndex;

			try
			{
                collapsedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.ThemeImage);
                expandedImageIndex = collapsedImageIndex;

				ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(aNodeRID);
                MIDColorNode node = new MIDColorNode(
                    SAB,
                    eTreeNodeType.ObjectNode,
                    ccp,
                    ccp.ColorCodeID,
                    Include.NoRID,
                    Include.SystemUserRID,
                    SetSecurity(ccp.Key),
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    Include.SystemUserRID
                    );

				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//node.Text = ccp.Text;
				node.InternalText = ccp.Text;
				//End Track #6201 - JScott - Store Count removed from attr sets
				node.NodeID = ccp.ColorCodeID;
				node.NodeName = ccp.ColorCodeName;
				node.ColorGroup = ccp.ColorCodeGroup;
				node.NodeChangeType = eChangeType.none;

                node.HasChildren = false;
				node.DisplayChildren = false;

				if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
				}

				if (node.HasChildren
					&& node.DisplayChildren)
				{
                    node.Nodes.Add(new MIDColorNode());
					node.ChildrenLoaded = false;
				}
				else
				{
					node.ChildrenLoaded = true;
				}
				if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
				}

				return node;
			}
			catch
			{
				throw;
			}
		}

		public int GetFolderImageIndex(string aFolderType)
		{
			try
			{
				return MIDGraphics.ImageIndexWithDefault(Include.MIDDefaultColor, aFolderType);
			}
			catch
			{
				throw;
			}
		}

        public FunctionSecurityProfile SetSecurity(int aKey)
		{
            FunctionSecurityProfile securityProfile;
            securityProfile = new FunctionSecurityProfile(aKey);
            securityProfile.SetAllowUpdate();
            return securityProfile;
		}

        /// <summary>
        /// Virtual method that is called after a label has been updated
        /// </summary>
        /// <returns>
        /// A boolean indicating if post-processing was successful
        /// </returns>

        override protected bool AfterLabelUpdate(MIDTreeNode aNode, string aNewName)
        {
            return true;
        }
	}

	/// <summary>
	/// Used as a node in the treeview for the Color Browser
	/// </summary>
	public class MIDColorNode : MIDTreeNode
	{
        //=============
        // FIELDS
        //=============
		//  If new fields are added, the clone method below must also be changed
		private string						_nodeName;
		private string						_colorGroup;

        //=============
        // CONSTRUCTORS
        //=============
        public MIDColorNode()
            : base()
		{
            CommonLoad();
		}

        public MIDColorNode(
            SessionAddressBlock aSAB,
            eTreeNodeType aTreeNodeType,
            Profile aProfile,
            string aText,
            int aParentId,
            int aUserId,
            FunctionSecurityProfile aFunctionSecurityProfile,
            int aCollapsedImageIndex,
            int aSelectedCollapsedImageIndex,
            int aExpandedImageIndex,
            int aSelectedExpandedImageIndex,
            int aOwnerUserRID)
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, true, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, new MIDTreeNodeSecurityGroup(aFunctionSecurityProfile, null), true, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
			//End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            CommonLoad();
        }

        public MIDColorNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			string aText,
			int aParentId,
			int aUserId,
			FunctionSecurityProfile aFunctionSecurityProfile,
			int aImageIndex,
			int aSelectedImageIndex,
			int aOwnerUserRID)
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, true, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, new MIDTreeNodeSecurityGroup(aFunctionSecurityProfile, null), true, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
			//End Track #6321 - JScott - User has ability to to create folders when security is view
		{
            CommonLoad();
		}

        private void CommonLoad()
        {
            CanBeDeleted = false;
        }

        public eProfileType NodeType
        {
            get { return Profile.ProfileType; }
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
		/// Gets or sets the group of the color.
		/// </summary>
		public string ColorGroup 
		{
			get { return _colorGroup; }
			set { _colorGroup = value; }
		}

		/// <summary>
		/// Used to clone or copy a node.
		/// </summary>
		/// <remarks>
		/// This method must be change when fields are added or removed from the class.
		/// </remarks>
		public MIDColorNode CloneNode() 
		{
			MIDColorNode mtn = (MIDColorNode) base.Clone();
			mtn.NodeChangeType = this.NodeChangeType;
			mtn.NodeName = this.NodeName;
			mtn.ColorGroup = this.ColorGroup;
			return mtn;
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
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

		override public bool isSelectAllowed(bool aMultiSelect, ArrayList aSelectedNodes)
		{
			bool allowSelect;

			try
			{
				allowSelect = true;

				if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
				{
					allowSelect = false;
				}

				return allowSelect;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode is draggable for a given DragDropEffects
		/// </summary>
		/// <param name="aDragDropEffects">
		/// The current DragDropEffects type being processed.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode is draggable for a given DragDropEffects
		/// </returns>

		override public bool isDragAllowed(DragDropEffects aCurrentEffect)
		{
			bool allowDrag;

			try
			{
                if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
                {
                    return false;
                }

				allowDrag = true;

				switch (NodeType)
				{
                    //case eColorNodeType.GroupFolder:
                    case eProfileType.ColorGroup:
						allowDrag = false;
						break;
				}

				return allowDrag;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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

		override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDestinationNode)
		{
			MIDColorNode destNode;

			try
			{
				destNode = (MIDColorNode)aDestinationNode;

                //if (destNode.NodeType == eColorNodeType.ColorNode)
                if (destNode.NodeType == eProfileType.ColorCode)
				{
					return false;
				}

                // do not allow drop in shared path or shared node in favorites
                if (aDestinationNode.isShared ||
                    (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
                    this.isShared))
                {
                    return false;
                }

				// do not allow drop on same node
				if (destNode == this)
				{
					return false;
				}

				// do not allow drop in same parent
				if (destNode == Parent)
				{
					return false;
				}

				DropAction = aDropAction;

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode's label can be edited
		/// </summary>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode's label can be edited
		/// </returns>

		override public bool isLabelEditAllowed()
		{
			try
			{
				//TODO: MUST BE IMPLELENTED TO USE BASE TREEVIEW
				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that refreshes the shortcut node
		/// </summary>

		override public void RefreshShortcutNode(MIDTreeNode aChangedNode)
		{
			try
			{
				//TODO: MUST BE IMPLELENTED TO USE BASE TREEVIEW
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
