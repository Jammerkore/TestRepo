using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{

	public class MIDFilterNode : MIDTreeNode
	{
		//=============
		// CONSTRUCTORS
		//=============

        //public MIDFilterNode()
        //    : base()
        //{
        //}

		public MIDFilterNode(
            filterTypes filterType,
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			string aText,
			int aParentId,
			int aUserId,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			int aImageIndex,
			int aSelectedImageIndex,
			int aOwnerUserRID)
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, false, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, aNodeSecurityGroup, false, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
			//End Track #6321 - JScott - User has ability to to create folders when security is view
		{
            this._filterType = filterType;
            SetProfileTypes();
		}

		public MIDFilterNode(
            filterTypes filterType,
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			string aText,
			int aParentId,
			int aUserId,
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile aFunctionSecurityProfile,
			MIDTreeNodeSecurityGroup aNodeSecurityGroup,
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			int aCollapsedImageIndex,
			int aSelectedCollapsedImageIndex,
			int aExpandedImageIndex,
			int aSelectedExpandedImageIndex,
			int aOwnerUserRID)
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, false, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, aNodeSecurityGroup, false, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
			//End Track #6321 - JScott - User has ability to to create folders when security is view
		{
            this._filterType = filterType;
            SetProfileTypes();
		}

		//===========
		// PROPERTIES
		//===========

        private filterTypes _filterType;
        private eProfileType _filterProfileType;
        private eProfileType _MainFavoritesFolderProfileType;
        private eProfileType _MainUserFolderProfileType;
        private eProfileType _MainSubFolderProfileType;


		//========
		// METHODS
		//========

        private void SetProfileTypes()
        {
            if (this._filterType == filterTypes.StoreFilter)
            {
                _filterProfileType = eProfileType.FilterStore;
                _MainFavoritesFolderProfileType = eProfileType.FilterStoreMainFavoritesFolder;
                _MainUserFolderProfileType = eProfileType.FilterStoreMainUserFolder;
                _MainSubFolderProfileType = eProfileType.FilterStoreSubFolder;
            }
            else if (this._filterType == filterTypes.HeaderFilter)  //TT#1313-MD -jsobek -Header Filters
            {
                _filterProfileType = eProfileType.FilterHeader;
                _MainFavoritesFolderProfileType = eProfileType.FilterHeaderMainFavoritesFolder;
                _MainUserFolderProfileType = eProfileType.FilterHeaderMainUserFolder;
                _MainSubFolderProfileType = eProfileType.FilterHeaderSubFolder;
            }
            else if (this._filterType == filterTypes.AssortmentFilter) //TT#1313-MD -jsobek -Header Filters
            {
                _filterProfileType = eProfileType.FilterAssortment;
                _MainFavoritesFolderProfileType = eProfileType.FilterAssortmentMainFavoritesFolder;
                _MainUserFolderProfileType = eProfileType.FilterAssortmentMainUserFolder;
                _MainSubFolderProfileType = eProfileType.FilterAssortmentSubFolder;
            }
        }

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
            bool allowSelect = true;

			try
			{
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
		override public bool isDragAllowed(DragDropEffects aDragDropEffects)
		{
			bool allowDrag;

			try
			{
                if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
                {
                    return false;
                }

				allowDrag = false;

				if (!isChildShortcut &&
					(NodeProfileType == _filterProfileType ||
					NodeProfileType == _MainSubFolderProfileType))
				{
					allowDrag = true;
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
			try
			{
				// do not allow drop in shared path or shared node in favorites
				if (aDestinationNode.isShared ||
					(aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
					this.isShared))
				{
					return false;
				}

				// do not allow drop on same node
				if (aDestinationNode == this)
				{
					return false;
				}

				switch (aDestinationNode.TreeNodeType)
				{
					case eTreeNodeType.MainFavoriteFolderNode:
					case eTreeNodeType.MainSourceFolderNode:
					case eTreeNodeType.ObjectNode:
					case eTreeNodeType.SubFolderNode:

						if (GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
							aDestinationNode.GetTopSourceNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode)
						{
							return false;
						}

						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						if (isFolder)
						{
							if (!aDestinationNode.FolderSecurityProfile.AllowUpdate)
							{
								return false;
							}
						}
						else
						{
						//End Track #6321 - JScott - User has ability to to create folders when security is view
							if (!aDestinationNode.FunctionSecurityProfile.AllowUpdate)
							{
								return false;
							}
						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						}
						//End Track #6321 - JScott - User has ability to to create folders when security is view

                        // Begin TT#3996 - JSmith - Task List Explorer - Invalid message when copying a task
                        if (aDestinationNode.TreeNodeType == eTreeNodeType.ChildObjectShortcutNode ||
                            aDestinationNode.TreeNodeType == eTreeNodeType.ObjectNode)
                        {
                            return false;
                        }
                        // End TT#3996 - JSmith - Task List Explorer - Invalid message when copying a task

						if (aDropAction == DragDropEffects.Copy)
						{
							if (aDestinationNode.isChildOf(this))
							{
								return false;
							}
							else
							{
                                // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                                }
                                else
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
								return true;
							}
						}
						else
						{
							if (aDestinationNode == GetParentNode() || aDestinationNode.isChildOf(this) ||
								(aDestinationNode.isObject && aDestinationNode.GetParentNode() == GetParentNode()))
							{
								return false;
							}

							if (aDestinationNode.GetTopSourceNode().Profile.ProfileType != GetTopSourceNode().Profile.ProfileType)
							{
								return FunctionSecurityProfile.AllowDelete;
							}
							else
							{
								return FunctionSecurityProfile.AllowUpdate;
							}
						}

					default:

						return false;
				}
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
				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				if (!isShortcut &&
					    (
                            (
                                FolderSecurityProfile.AllowUpdate &&
					            (
                                    NodeProfileType == _MainFavoritesFolderProfileType  //eProfileType.FilterStoreMainFavoritesFolder
                                    || NodeProfileType == _MainUserFolderProfileType //eProfileType.FilterStoreMainUserFolder
                                    || NodeProfileType == _MainSubFolderProfileType //eProfileType.FilterStoreSubFolder
                                )
                            ) ||
					        (
                                FunctionSecurityProfile.AllowUpdate
                                && NodeProfileType == _filterProfileType //eProfileType.FilterStore
                            )
                        )
                   )
				//End Track #6321 - JScott - User has ability to to create folders when security is view
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that refreshes the shortcut node
		/// </summary>
		override public void RefreshShortcutNode(MIDTreeNode aChangedNode)
		{
			try
			{
				if (isObjectShortcut || isFolderShortcut)
				{
					UserId = aChangedNode.UserId;

					if (Profile.ProfileType == eProfileType.FilterStore)
					{
						InternalText = ((StoreFilterProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
					}
                    else if (Profile.ProfileType == eProfileType.FilterHeader)
                    {
                        InternalText = ((HeaderFilterProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                    }
                    else if (Profile.ProfileType == eProfileType.FilterAssortment)
                    {
                        InternalText = ((AssortmentFilterProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                    }
                    else if (Profile.ProfileType == eProfileType.FilterStoreGroup)
                    {
                        InternalText = ((StoreGroupFilterProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                    }
					else
					{
						InternalText = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
					}
				}
				else if (isChildShortcut)
				{
					UserId = aChangedNode.UserId;

					if (((Profile)Profile).ProfileType == eProfileType.FilterStore)
					{
						InternalText = ((StoreFilterProfile)aChangedNode.Profile).Name;
					}
                    else if (((Profile)Profile).ProfileType == eProfileType.FilterHeader)
                    {
                        InternalText = ((HeaderFilterProfile)aChangedNode.Profile).Name;
                    }
                    else if (((Profile)Profile).ProfileType == eProfileType.FilterAssortment)
                    {
                        InternalText = ((AssortmentFilterProfile)aChangedNode.Profile).Name;
                    }
                    else if (((Profile)Profile).ProfileType == eProfileType.FilterStoreGroup)
                    {
                        InternalText = ((StoreGroupFilterProfile)aChangedNode.Profile).Name;
                    }
					else
					{
						InternalText = ((FolderProfile)aChangedNode.Profile).Name;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public object Clone() 
		{
			try
			{
				return base.Clone();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}


    public class OnFilterPropertiesCloseClass
    {
        private GenericEnqueue _objEnqueue;

        public OnFilterPropertiesCloseClass(GenericEnqueue aObjEnqueue)
        {
            _objEnqueue = aObjEnqueue;
        }

        public void OnClose(object source, MIDRetail.Windows.frmFilterBuilder.FilterPropertiesCloseEventArgs e)
        {
            try
            {
                if (_objEnqueue != null)
                {
                    if (!_objEnqueue.IsInConflict)
                    {
                        _objEnqueue.DequeueGeneric();
                    }
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
