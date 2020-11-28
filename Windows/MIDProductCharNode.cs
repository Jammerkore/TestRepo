// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	public class ProductCharTreeView : MIDTreeView
	{
        // add event to update explorer when node is changed
        public delegate void NodeChangeEventHandler(object source, NodeChangeEventArgs e);
        public event NodeChangeEventHandler OnNodeChanged;

        //=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// For designer.
		/// </summary>
		public ProductCharTreeView()
		{
		}

		override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ClipboardListBase aSelectedNodes)
		{
			MIDProductCharNode dropNode;
			ArrayList nodes;
            //BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
            MIDProductCharNode selectedNode;
            //END TT#3962-VStuart-Dragged Values never allowed to drop-MID

			try
			{
				dropNode = (MIDProductCharNode)aDropNode;

                //BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                if (aSelectedNodes.ClipboardDataType == eProfileType.ProductCharacteristicValue)
                {
                    //nodes = new ArrayList();

                    //foreach (ProductCharacteristicClipboardProfile item in aSelectedNodes.ClipboardItems)
                    //{
                    //    //nodes.Add(GetProductCharNode(item));
                    //    selectedNode = GetProductCharNode(item);
                    //    if (!selectedNode.isDropAllowed(aDropAction, aDropNode))
                    //    {
                    //        return false;
                    //    }
                    //    nodes.Add(selectedNode);
                    //}

                    //return isDropAllowed(aDropAction, aDropNode, nodes);
                    //return true;
                    return false;
                    //END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                }
                else if (aSelectedNodes.ClipboardDataType == eProfileType.HierarchyNode)
                {
                    if (dropNode.NodeType == eProfileType.ProductCharacteristicValue)
                    {
                        if ((dropNode.NodeChangeType != eChangeType.delete) &&
                            (dropNode.NodeChangeType != eChangeType.add) &&
                            (((MIDProductCharNode)dropNode.Parent).NodeChangeType != eChangeType.delete) &&
                            (((MIDProductCharNode)dropNode.Parent).NodeChangeType != eChangeType.add))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
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
            return aClipboardDataType == eProfileType.ProductCharacteristic;
        }

        //BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
        private MIDProductCharNode GetProductCharNode(ProductCharacteristicClipboardProfile aClipboardProfile)
		{
			try
			{
                MIDProductCharNode treeNode = (MIDProductCharNode)FindTreeNode(this.Nodes, eProfileType.ProductCharacteristicValue, aClipboardProfile.Key);
                //END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                if (treeNode == null)
				{
					return BuildProductCharValueNode(string.Empty, new ProductCharValueProfile(aClipboardProfile.Key), -1);
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

		public MIDProductCharNode BuildProductCharFolder(string aProductCharFolderName)
		{
            FolderProfile folderProfile;
            int collapsedImageIndex, expandedImageIndex;
            FunctionSecurityProfile functionSecurityProfile;
			try
			{
                folderProfile = Folder_Get(Include.GlobalUserRID, eProfileType.MerchandiseMainProductCharFolder, aProductCharFolderName);

                collapsedImageIndex = GetFolderImageIndex(MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(MIDGraphics.OpenFolder);
                functionSecurityProfile = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesCharacteristics);

                MIDProductCharNode node = new MIDProductCharNode(
                    SAB, 
                    eTreeNodeType.MainSourceFolderNode, 
                    folderProfile, 
                    aProductCharFolderName,
                    Include.NoRID,
                    Include.SystemUserRID,
                    functionSecurityProfile,
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    Include.SystemUserRID
                    );
				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//node.Text = aProductCharFolderName;
				node.InternalText = aProductCharFolderName;
				//End Track #6201 - JScott - Store Count removed from attr sets
				node.NodeID = aProductCharFolderName;
				node.ProductCharGroupKey = Include.NoRID;
				node.NodeChangeType = eChangeType.none;

				if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
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

        public MIDProductCharNode BuildProductCharNode(string aProductCharName, ProductCharProfile aProductCharProfile)
		{
            int collapsedImageIndex, expandedImageIndex;
			                
			try
			{
                collapsedImageIndex = GetFolderImageIndex(MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(MIDGraphics.OpenFolder);

                MIDProductCharNode node = new MIDProductCharNode(
                    SAB,
                    eTreeNodeType.ObjectNode,
                    aProductCharProfile,
                    aProductCharName,
                    Include.NoRID,
                    Include.SystemUserRID,
                    SetSecurity(aProductCharProfile.Key),
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    Include.SystemUserRID
                    );

				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//node.Text = aProductCharName;
				node.InternalText = aProductCharName;
				//End Track #6201 - JScott - Store Count removed from attr sets
				node.NodeID = aProductCharName;
                node.ProductCharGroupKey = aProductCharProfile.Key;
				node.NodeChangeType = eChangeType.none;

                if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
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

        public MIDProductCharNode BuildProductCharValueNode(string aProductCharValue, ProductCharValueProfile aProductCharValueProfile, int aProductCharGroupKey)
		{
            int collapsedImageIndex, expandedImageIndex;
			                
			try
			{
                collapsedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NotesImage);
                expandedImageIndex = collapsedImageIndex;


                MIDProductCharNode node = new MIDProductCharNode(
                    SAB,
                    eTreeNodeType.ObjectNode,
                    aProductCharValueProfile,
                    aProductCharValue,
                    aProductCharGroupKey,
                    Include.SystemUserRID,
                    SetSecurity(aProductCharValueProfile.Key),
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    Include.SystemUserRID
                    );

				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//node.Text = aProductCharValue;
				node.InternalText = aProductCharValue;
				//End Track #6201 - JScott - Store Count removed from attr sets
				node.NodeID = aProductCharValue;
				node.ProductCharGroupKey = aProductCharGroupKey;
				node.NodeChangeType = eChangeType.none;

				node.HasChildren = false;
				node.DisplayChildren = false;

				if (node.FunctionSecurityProfile.AccessDenied)
				{
					node.ForeColor = SystemColors.InactiveCaption;
				}

				node.ChildrenLoaded = true;
				node.AllowDrop = true;
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

        override public ClipboardListBase BuildClipboardList(ArrayList aNodes, DragDropEffects aAction)
        {
            ProductCharacteristicClipboardList clipboardList;

            try
            {
                clipboardList = new ProductCharacteristicClipboardList();

                foreach (MIDTreeNode node in aNodes)
                {
                    clipboardList.ClipboardItems.Add(BuildClipboardProfile(node, aAction));
                }

                return clipboardList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private ProductCharacteristicClipboardProfile BuildClipboardProfile(MIDTreeNode aNode, DragDropEffects aAction)
        {
            try
            {
                ProductCharacteristicClipboardProfile cbp = new ProductCharacteristicClipboardProfile(aNode.Profile.Key, aNode.Text, aNode.FunctionSecurityProfile);
                cbp.ProductCharGroupKey = ((MIDProductCharNode)aNode).ProductCharGroupKey;
                
                cbp.DragImage = this.ImageList.Images[aNode.SelectedImageIndex];
                cbp.DragImageHeight = aNode.Bounds.Height;
                cbp.DragImageWidth = aNode.Bounds.Width;
                cbp.Action = aAction;

                return cbp;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
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
            try
            {
                string labelText = aNewName;

                if (labelText == null)
                {
                    labelText = aNode.Text;
                }

                if (labelText == "")
                {
                    return false;
                }
                else
                {
                    MIDProductCharNode node = (MIDProductCharNode)aNode;
                    if (ValidName(node, labelText))
                    {
                        node.NodeID = labelText;
                        // set node as being updated
                        if (node.NodeChangeType == eChangeType.none)
                        {
                            node.NodeChangeType = eChangeType.update;
                        }
                        // if value, set parent characteristic node as being updated
                        if (node.NodeType == eProfileType.ProductCharacteristicValue)
                        {
                            if (((MIDProductCharNode)node.Parent).NodeChangeType == eChangeType.none)
                            {
                                ((MIDProductCharNode)node.Parent).NodeChangeType = eChangeType.update;
                            }
                        }
                        NodeChangeEventArgs ea = new NodeChangeEventArgs(node);
                        if (OnNodeChanged != null)  // throw event to explorer to make changes
                        {
                            OnNodeChanged(this, ea);
                        }
                        return true;
                    }
                    else
                    {
                        if (node.NodeType == eProfileType.ProductCharacteristic)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        return false;
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        public bool ValidName(MIDProductCharNode aNode, string aNewName)
        {
            try
            {
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                if (aNode.NodeType == eProfileType.ProductCharacteristic)
                {
                    return hm.IsProductCharNameValid(aNode.Profile.Key, aNewName);
                }
                else
                {
                    return hm.IsProductCharValueValid(aNode.Profile.Key, ((MIDProductCharNode)aNode.Parent).Profile.Key, aNewName);
                }
            }
            catch 
            {
                throw;
            }
        }
	}

	/// <summary>
	/// Used as a node in the treeview for the ProductChar Browser
	/// </summary>
	public class MIDProductCharNode : MIDTreeNode
	{
        //=============
        // FIELDS
        //=============
		//  If new fields are added, the clone method below must also be changed
		private int							_productCharGroupKey;
		private bool						_hasBeenMoved;

        //=============
        // CONSTRUCTORS
        //=============
        public MIDProductCharNode()
            : base()
		{
            CommonLoad();
		}

        public MIDProductCharNode(
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

        public MIDProductCharNode(
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

        public eProfileType NodeType 
		{
			get { return Profile.ProfileType ; }
		}

		public int ProductCharGroupKey 
		{
			get { return _productCharGroupKey; }
			set { _productCharGroupKey = value; }
		}

		/// <summary>
		/// Gets or sets the flag to identify if the node has been moved.
		/// </summary>
		public bool HasBeenMoved
		{
			get { return _hasBeenMoved; }
			set { _hasBeenMoved = value; }
		}

        private void CommonLoad()
        {
            _productCharGroupKey = Include.NoRID;
            CanBeDeleted = false;
            _hasBeenMoved = false;
        }

		/// <summary>
		/// Used to clone or copy a node.
		/// </summary>
		/// <remarks>
		/// This method must be change when fields are added or removed from the class.
		/// </remarks>
		public MIDProductCharNode CloneNode() 
		{
			MIDProductCharNode mtn = (MIDProductCharNode) base.Clone();
			mtn.NodeChangeType = this.NodeChangeType;
            mtn._productCharGroupKey = this._productCharGroupKey;
            return mtn;
		}

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

				// do not allow two values from the same characteristic to be selected
				if (aMultiSelect)
				{
					foreach (MIDProductCharNode node in aSelectedNodes)
					{
                        if (NodeType == eProfileType.ProductCharacteristicValue &&
                            node.ProductCharGroupKey == ProductCharGroupKey)
						{
							allowSelect = false;
							break;
						}
					}
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
                    case eProfileType.Folder:
						allowDrag = false;
						break;
                    case eProfileType.ProductCharacteristic:
						allowDrag = false;
						break;
                    case eProfileType.ProductCharacteristicValue:
						if (NodeRID == Include.NoRID)
						{
							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustClickApplyBeforeDrag));
							allowDrag = false;
						}
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
			MIDProductCharNode destNode;

			try
			{
				destNode = (MIDProductCharNode)aDestinationNode;

                if (destNode.NodeType == eProfileType.MerchandiseMainProductCharFolder)
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

				// do not allow drop on target if target is deleted already
                if (((destNode.NodeType == eProfileType.ProductCharacteristic) && (destNode.NodeChangeType == eChangeType.delete)) ||
                    ((destNode.NodeType == eProfileType.ProductCharacteristicValue) && (destNode.NodeChangeType == eChangeType.delete)) ||
                   ((destNode.NodeType == eProfileType.ProductCharacteristicValue) && (((MIDProductCharNode)destNode.Parent).NodeChangeType == eChangeType.delete)))
				{
					return false;
				}

				// do not allow drop on target if target is added already
                if (((destNode.NodeType == eProfileType.ProductCharacteristic) && (destNode.NodeChangeType == eChangeType.add)) ||
                    ((destNode.NodeType == eProfileType.ProductCharacteristicValue) && (destNode.NodeChangeType == eChangeType.add)) ||
                   ((destNode.NodeType == eProfileType.ProductCharacteristicValue) && (((MIDProductCharNode)destNode.Parent).NodeChangeType == eChangeType.add)))
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
                if ((NodeType == eProfileType.ProductCharacteristic ||
                    NodeType == eProfileType.ProductCharacteristicValue) &&
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//_functionSecurityProfile.AllowUpdate)
					FunctionSecurityProfile.AllowUpdate)
					//End Track #6321 - JScott - User has ability to to create folders when security is view
                {
                    return true;
                }

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

    public class NodeChangeEventArgs : EventArgs
    {
        MIDTreeNode _node;

        public NodeChangeEventArgs(MIDTreeNode node)
        {
            _node = node;
        }

        public MIDTreeNode Node
        {
            get { return _node; }
            set { _node = value; }
        }
        
    }
}
