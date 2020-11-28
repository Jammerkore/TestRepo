// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Collections;
using System.Collections.Generic; //TT#1388-MD -jsobek -Product Filters
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

	public class HierarchyTreeView : MIDTreeView
	{
        //=============
		// FIELDS
		//=============

        private int cClosedFolderImage;
        private int cOpenFolderImage;
        private int cClosedShortcutFolderImage;
        private int cOpenShortcutFolderImage;

        private MIDHierarchyNode _myHierarchyNode;
        private MIDHierarchyNode _organizationalNode;
        private MIDHierarchyNode _alternateNode;
        private MIDHierarchyNode _sharedNode;

        private FunctionSecurityProfile _securityAltUser;
        private FunctionSecurityProfile _securityAltGlobal;
        private FunctionSecurityProfile _securityOrg;

        private HierarchyMaintenance _hm;
        private FolderDataLayer _dlFolder;

        private ArrayList _lockControl = new ArrayList();

        private bool _bContinueProcessing;

		//=============
		// CONSTRUCTORS
		//=============

        /// <summary>
		/// Creates a new instace of HierarchyTreeView.
		/// </summary>
		public HierarchyTreeView()
		{
		}

        //=============
        // PROPERTIES
        //=============

        /// <summary>
        /// Gets or sets the main My Hierarchy node.
        /// </summary>
        public MIDHierarchyNode MyHierarchyNode
        {
            get { return _myHierarchyNode; }
            set { _myHierarchyNode = value; }
        }

        /// <summary>
        /// Gets or sets the main Organizational node.
        /// </summary>
        public MIDHierarchyNode OrganizationalNode
        {
            get { return _organizationalNode; }
            set { _organizationalNode = value; }
        }

        /// <summary>
        /// Gets or sets the main Alternate node.
        /// </summary>
        public MIDHierarchyNode AlternateNode
        {
            get { return _alternateNode; }
            set { _alternateNode = value; }
        }

        /// <summary>
        /// Gets or sets the main Shares node.
        /// </summary>
        public MIDHierarchyNode SharedNode
        {
            get { return _sharedNode; }
            set { _sharedNode = value; }
        }

        /// <summary>
        /// Gets user's FunctionSecurityProfile for the user alternate hierarchies.
        /// </summary>
        public FunctionSecurityProfile SecurityAltUser
        {
            get { return _securityAltUser; }
        }

        /// <summary>
        /// Gets user's FunctionSecurityProfile for the global alternate hierarchies.
        /// </summary>
        public FunctionSecurityProfile SecurityAltGlobal
        {
            get { return _securityAltGlobal; }
        }

        /// <summary>
        /// Gets user's FunctionSecurityProfile for the organizational hierarchy.
        /// </summary>
        public FunctionSecurityProfile SecurityOrg
        {
            get { return _securityOrg; }
        }

        //=============
        // METHODS
        //=============

        override public void InitializeTreeView(SessionAddressBlock aSAB, bool aAllowMultiSelect, Form aMDIParentForm)
		{
            base.InitializeTreeView(aSAB, aAllowMultiSelect, aMDIParentForm);

            cClosedFolderImage = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
            cOpenFolderImage = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);
            cClosedShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.ClosedTreeFolder);
			cOpenShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.OpenTreeFolder);

            _securityAltUser = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltUser);
            _securityAltGlobal = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltGlobal);
            _securityOrg = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesOrg);

            _hm = new HierarchyMaintenance(aSAB);
            _dlFolder = new FolderDataLayer();
		}
		
        /// <summary>
		/// Virtual method that loads the nodes for the MIDTreeView
		/// </summary>

        override public void LoadNodes()
        {
            try
            {
                SAB.HierarchyServerSession.BuildAvailableNodeList();

                BuildHierarchyFolders();

                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Loading Explorer", this.GetType().Name);

                Nodes.Clear();
                Nodes.AddRange(GetRootNodes());
                // build shared users
                if (SharedNode != null)
                {
                    SharedNode.BuildChildren();
                    SortChildNodes(SharedNode);
                    SharedNode.ChildrenLoaded = true;
                }
                GetHierarchies();

                OrderHierarchyFolders(); // order hierarchy types
                if (MyHierarchyNode.HasChildren
                    && MyHierarchyNode.DisplayChildren) // and expand
                {
                    MyHierarchyNode.Expand();
                }
                if (OrganizationalNode.HasChildren
                    && OrganizationalNode.DisplayChildren)
                {
                    OrganizationalNode.Expand();
                }
                if (AlternateNode.HasChildren
                    && AlternateNode.DisplayChildren)
                {
                    AlternateNode.Expand();
                }
                if (SharedNode != null)
                {
                    SharedNode.Expand();
                }
            }

            catch 
            {
                throw;
            }

        }

        /// <summary>
        /// Builds a node for each type of hierarchy
        /// </summary>
        private void BuildHierarchyFolders()
        {
            int closedImageIndex, expandedImageIndex;

            FolderProfile folderProfile;
            FunctionSecurityProfile functionSecurity;
            DataTable dtSharedFolders;
            ArrayList userList;

            try
            {
                // Favorites folder
                folderProfile = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.MerchandiseMainFavoritesFolder, "My Favorites");
                closedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage);
                expandedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage);
                FavoritesNode = new MIDHierarchyNode(
                    SAB, 
                    eTreeNodeType.MainFavoriteFolderNode, 
                    folderProfile, 
                    folderProfile.Name, 
                    Include.NoRID, 
                    folderProfile.UserRID, 
                    null, 
                    closedImageIndex, 
                    closedImageIndex, 
                    expandedImageIndex, 
                    expandedImageIndex, 
                    folderProfile.OwnerUserRID);

                // check for children
                if (Folder_Children_Exists(SAB.ClientServerSession.UserRID, FavoritesNode.Profile.Key))
                {
                    FavoritesNode.Nodes.Add(new MIDHierarchyNode());
                    FavoritesNode.ChildrenLoaded = false;
                    FavoritesNode.HasChildren = true;
                    FavoritesNode.DisplayChildren = true;
                }
                else
                {
                    FavoritesNode.ChildrenLoaded = true;
                }
                ((MIDHierarchyNode)FavoritesNode).HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(FavoritesNode.Profile.Key);
                ((MIDHierarchyNode)FavoritesNode).HierarchyNodeSecurityProfile.SetFullControl();

                // User folder
                folderProfile = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.MerchandiseMainUserFolder, SAB.ClientServerSession.MyHierarchyName);
                closedImageIndex = MIDGraphics.ImageIndexWithDefault(SAB.ClientServerSession.MyHierarchyColor, MIDGraphics.ClosedFolder);
                expandedImageIndex = MIDGraphics.ImageIndexWithDefault(SAB.ClientServerSession.MyHierarchyColor, MIDGraphics.OpenFolder);
                functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltUser);
                functionSecurity.SetDenyDelete();
                MyHierarchyNode = new MIDHierarchyNode(
                    SAB, 
                    eTreeNodeType.MainSourceFolderNode, 
                    folderProfile, 
                    folderProfile.Name, 
                    Include.NoRID,
                    folderProfile.UserRID, 
                    functionSecurity, 
                    closedImageIndex, 
                    closedImageIndex, 
                    expandedImageIndex, 
                    expandedImageIndex,
                    folderProfile.OwnerUserRID);

                // Organizational folder
                folderProfile = Folder_Get(Include.GlobalUserRID, eProfileType.MerchandiseMainOrganizationalFolder, MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type_Organizational));
                closedImageIndex = GetFolderImageIndex(false, Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(false, Include.MIDDefaultColor, MIDGraphics.OpenFolder);
                functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesOrgNodes);
                functionSecurity.SetDenyDelete();
                OrganizationalNode = new MIDHierarchyNode(
                    SAB, 
                    eTreeNodeType.MainSourceFolderNode, 
                    folderProfile, 
                    folderProfile.Name, 
                    Include.NoRID,
                    folderProfile.UserRID, 
                    functionSecurity, 
                    closedImageIndex, 
                    closedImageIndex, 
                    expandedImageIndex, 
                    expandedImageIndex,
                    folderProfile.OwnerUserRID);

                // alternate folder
                folderProfile = Folder_Get(Include.GlobalUserRID, eProfileType.MerchandiseMainAlternatesFolder, MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type_Alternate));
                closedImageIndex = GetFolderImageIndex(false, Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(false, Include.MIDDefaultColor, MIDGraphics.OpenFolder);
                functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltNodes);
                functionSecurity.SetDenyDelete();
                AlternateNode = new MIDHierarchyNode(
                    SAB, 
                    eTreeNodeType.MainSourceFolderNode, 
                    folderProfile, 
                    folderProfile.Name, 
                    Include.NoRID,
                    folderProfile.UserRID, 
                    functionSecurity, 
                    closedImageIndex, 
                    closedImageIndex, 
                    expandedImageIndex, 
                    expandedImageIndex,
                    folderProfile.OwnerUserRID);

                // shared folder
               SharedNode = null;
               userList = new ArrayList();
               userList.Add(SAB.ClientServerSession.UserRID);
               dtSharedFolders = DlFolder.Folder_Read(userList, eProfileType.MerchandiseMainUserFolder, false, true);
               if (dtSharedFolders.Rows.Count > 0)
               {
                   folderProfile = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.MerchandiseMainSharedFolder, MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName), SAB.ClientServerSession.UserRID);
                   closedImageIndex = MIDGraphics.ImageSharedIndexWithDefault(Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
                   expandedImageIndex = MIDGraphics.ImageSharedIndexWithDefault(Include.MIDDefaultColor, MIDGraphics.OpenFolder);
                   functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltUser);
                   SharedNode = new MIDHierarchyNode(
                       SAB,
                       eTreeNodeType.MainSourceFolderNode,
                       folderProfile,
                       folderProfile.Name,
                       Include.NoRID,
                       folderProfile.UserRID,
                       functionSecurity,
                       closedImageIndex,
                       closedImageIndex,
                       expandedImageIndex,
                       expandedImageIndex,
                       folderProfile.OwnerUserRID);

                   // add dummy node for expanding
                   SharedNode.Nodes.Add(new MIDHierarchyNode());
                   SharedNode.ChildrenLoaded = false;
                   SharedNode.HasChildren = true;
                   SharedNode.DisplayChildren = true;
               }
            }
            catch
            {
                throw;
            }
        }

        private MIDHierarchyNode[] GetRootNodes()
        {
            Sorted = true;

            ArrayList treenodes = new ArrayList();

            treenodes.Add(FavoritesNode);

            if (!SecurityAltUser.AccessDenied)
            {
                treenodes.Add(MyHierarchyNode);
            }
            if (!SecurityOrg.AccessDenied)
            {
                treenodes.Add(OrganizationalNode);
            }
            if (!SecurityAltGlobal.AccessDenied)
            {
                treenodes.Add(AlternateNode);
            }
            if (SharedNode != null)
            {
                treenodes.Add(SharedNode);
            }

            return (MIDHierarchyNode[])treenodes.ToArray(typeof(MIDHierarchyNode));
        }

        private MIDHierarchyNode[] GetHierarchies()
        {
            Sorted = true;
            HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetRootNodes();

            ArrayList treenodes = new ArrayList();

            foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
            {
                MIDHierarchyNode mtn = BuildNode(hnp.HomeHierarchyRID, hnp.HomeHierarchyType, null, hnp, false);
                mtn.isHierarchyRoot = true;

                if (DebugActivated)
                {
                    mtn.InternalText += " : #" + mtn.Profile.Key.ToString();
                }

                HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID);
                if (hp.Owner != Include.GlobalUserRID) // personal hierarchy		// Issue 3806
                {
                    if (hp.Owner == SAB.ClientServerSession.UserRID)
                    {
                        mtn.HierarchyNodeSecurityProfile.SetFullControl();	// override security for personal hierarchies to full access
                        MyHierarchyNode.Nodes.Add(mtn);
                        MyHierarchyNode.HasChildren = true;
                        MyHierarchyNode.DisplayChildren = true;
                        MyHierarchyNode.ChildrenLoaded = true;
                    }
                    else if (SharedNode != null)
                    {
                        // find correct user
                        foreach (MIDHierarchyNode node in SharedNode.Nodes)
                        {
                            if (hp.Owner == node.OwnerUserRID)
                            {
                                //mtn.HierarchyNodeSecurityProfile.SetReadOnly();	// override security for personal hierarchies to full access
                                node.Nodes.Add(mtn);
                                node.HasChildren = true;
                                node.DisplayChildren = true;
                                node.ChildrenLoaded = true;
                                mtn.OwnerUserRID = hp.Owner;
                                //mtn.ImageIndex = MIDGraphics.ImageSharedIndexWithDefault(mtn.NodeColor, MIDGraphics.ClosedFolder);
                                //mtn.SelectedImageIndex = mtn.ImageIndex;
                            }
                        }
                    }
                }
                else
                    if (hp.HierarchyType == eHierarchyType.organizational)
                    {
                        OrganizationalNode.Nodes.Add(mtn);
                        OrganizationalNode.HasChildren = true;
                        OrganizationalNode.DisplayChildren = true;
                        OrganizationalNode.ChildrenLoaded = true;
                    }
                    else
                    {
                        AlternateNode.Nodes.Add(mtn);
                        AlternateNode.HasChildren = true;
                        AlternateNode.DisplayChildren = true;
                        AlternateNode.ChildrenLoaded = true;
                    }
            }

            return (MIDHierarchyNode[])treenodes.ToArray(typeof(MIDHierarchyNode));
        }

        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
        override public bool isPasteFromClipboardAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode)
        {
            DataObject data = (DataObject)Clipboard.GetDataObject();
            if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                return isDropAllowed(aDropAction, aDropNode, (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList)));
            }
            else if (data.GetDataPresent(typeof(ProductCharacteristicClipboardList)))
            {
                return isDropAllowed(aDropAction, aDropNode, (ProductCharacteristicClipboardList)data.GetData(typeof(ProductCharacteristicClipboardList)));
            }
            else if (data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
            {
                return isDropAllowed(aDropAction, aDropNode, (HierarchyNodeClipboardList)data.GetData(typeof(HierarchyNodeClipboardList)));
            }
            else if (data.GetDataPresent(typeof(List<int>))) //TT#1388-MD -jsobek -Product Filters
            {
                List<int> aSelectedNodes = (List<int>)data.GetData(typeof(List<int>));
                return isDropAllowed(aDropAction, aDropNode, aSelectedNodes);
            }
            else
            {
                return false;
            }
        }
        // End TT# 394

		override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ClipboardListBase aSelectedNodes)
		{
			MIDHierarchyNode dropNode;

			try
			{
				dropNode = (MIDHierarchyNode)aDropNode;

                //if (dropNode.NodeID == "JS11")
                //{
                //    bool stop = true;
                //}

                // Begin TT#28 - JSmith - cannot drag/drop from My Hierarchy
                //if (aSelectedNodes.ClipboardDataType == eProfileType.HierarchyNode ||
                //    aSelectedNodes.ClipboardDataType == eProfileType.Folder)
                if (aSelectedNodes.ClipboardDataType == eProfileType.HierarchyNode ||
                    aSelectedNodes.ClipboardDataType == eProfileType.Folder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MerchandiseSubFolder)
                // End TT#28
				{
                    if (aSelectedNodes is TreeNodeClipboardList)
                    {
                        foreach (TreeNodeClipboardProfile item in aSelectedNodes.ClipboardItems)
                        {
                            if (!GetHierarchyNode(((TreeNodeClipboardList)aSelectedNodes).ClipboardProfile).isDropAllowed(aDropAction, aDropNode))
                            {
                                return false;
                            }
                        }
                    }
                    else if (aSelectedNodes is HierarchyNodeClipboardList)
                    {
                        foreach (HierarchyNodeClipboardProfile item in aSelectedNodes.ClipboardItems)
                        {
                            if (!GetHierarchyNode(item.HomeHierarchyRID, item.HomeHierarchyType, item.NodeType, item.Key).isDropAllowed(aDropAction, aDropNode))
                            {
                                return false;
                            }
                        }
                    }
                    // Begin TT#3975 - JSmith - Cannot drag/drop node to personal alternate after copy/paste
                    aDropNode.DropAction = DragDropEffects.Move;
                    CurrentEffect = DragDropEffects.Move;
                    // End TT#3975 - JSmith - Cannot drag/drop node to personal alternate after copy/paste
                    return true;
				}
                else if (aSelectedNodes.ClipboardDataType == eProfileType.ProductCharacteristicValue)
				{
                    if (dropNode.Profile.ProfileType == eProfileType.HierarchyNode)
                    {
                        aDropNode.DropAction = DragDropEffects.Move;
                        CurrentEffect = DragDropEffects.Move;
                        return true;
                    }
                    else
                    {
                        return false;
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

        //Begin TT#1388-MD -jsobek -Product Filters
        public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, List<int> aSelectedNodes)
        {
            MIDHierarchyNode dropNode;

            try
            {
                if (aSelectedNodes == null)
                {
                    return false;
                }
                else
                {
                    dropNode = (MIDHierarchyNode)aDropNode;
                    foreach (int hnRID in aSelectedNodes)
                    {
                        MIDHierarchyNode hn = (MIDHierarchyNode)BuildNode(null, hnRID);
                        if (!hn.isDropAllowed(aDropAction, aDropNode))
                        {
                            return false;
                        }
                    }
                }
                return true;             
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1388-MD -jsobek -Product Filters
	
		override public bool isAllowedDataType(eProfileType aClipboardDataType)
        {
            return aClipboardDataType == eProfileType.HierarchyNode;
        }

        private MIDHierarchyNode GetHierarchyNode(TreeNodeClipboardProfile aClipboardProfile)
		{
            MIDHierarchyNode treeNode = null;
			try
			{
                if (aClipboardProfile.Node != null)
                {
                    return (MIDHierarchyNode)aClipboardProfile.Node;
                }
                else
                {
                    treeNode = FindHierarchyNode(this.Nodes, ((MIDHierarchyNode)aClipboardProfile.Node).HomeHierarchyRID,
                        ((MIDHierarchyNode)aClipboardProfile.Node).HomeHierarchyType, aClipboardProfile.Node.Profile.ProfileType, aClipboardProfile.Key);

                    if (treeNode == null)
                    {
                        return (MIDHierarchyNode)BuildNode(null, aClipboardProfile.Key);
                    }
                    else
                    {
                        return treeNode;
                    }
                }
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private MIDHierarchyNode GetHierarchyNode(int aHomeHierarchyRID, eHierarchyType aHomeHierarchyType, eProfileType aNodeType, int aKey)
        {
            MIDHierarchyNode treeNode = null;
            try
            {
                treeNode = FindHierarchyNode(this.Nodes, aHomeHierarchyRID, aHomeHierarchyType, aNodeType, aKey);

                if (treeNode == null)
                {
                    return (MIDHierarchyNode)BuildNode(null, aKey);
                }
                else
                {
                    return treeNode;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		public MIDHierarchyNode FindHierarchyNode(TreeNodeCollection aNodes, int aHierarchyRID, eHierarchyType aHierarchyType,
            eProfileType aNodeType, int aNodeRID)
        {
            MIDHierarchyNode findNode;
            HierarchyNodeProfile hnp;

            try
            {
                foreach (MIDHierarchyNode tn in aNodes)
                {
                    if (tn.HierarchyRID == aHierarchyRID &&
                    tn.Profile.ProfileType == aNodeType &&
                    tn.Profile.Key == aNodeRID)
                    {

                        return tn;
                    }
                }

                foreach (MIDHierarchyNode tn in aNodes)
                {
                    findNode = FindHierarchyNode(tn.Nodes, aHierarchyRID, aHierarchyType, aNodeType, aNodeRID);

                    if (findNode != null)
                    {
                        return findNode;
                    }
                }

                hnp = SAB.HierarchyServerSession.GetNodeData(aNodeRID, false);
                return BuildNode(aHierarchyRID, aHierarchyType, null, hnp, false);
            }
            catch
            {
                throw;
            }
        }

        public void OrderHierarchyFolders()
        {
            Sorted = false;  // order hierarchy types and expand
            if (!_securityOrg.AccessDenied)
            {
                Nodes.Remove(OrganizationalNode);
                Nodes.Insert(0, OrganizationalNode);
            }
            if (!_securityAltUser.AccessDenied)
            {
                Nodes.Remove(MyHierarchyNode);
                Nodes.Insert(0, MyHierarchyNode);
            }

            Nodes.Remove(FavoritesNode);
            Nodes.Insert(0, FavoritesNode);
        }

        override public void RefreshShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
        {
            try
            {
                // Begin TT#62 - JSmith - Object reference error when double-click folder
                if (aChangedNode != null)
                {
                // End TT#62
                    foreach (MIDTreeNode node in aStartNode.Nodes)
                    {
                        if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
                        {
                            node.RefreshShortcutNode(aChangedNode);
                        }
                        else if (node.isSubFolder || node.isFolderShortcut ||
                            (node.isChildShortcut && node.isSubFolder))
                        {
                            RefreshShortcuts(node, aChangedNode);
                        }
                    }
                // Begin TT#62
                }
                // End TT#62
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called after a label has been updated
        /// </summary>
        /// <returns>
        /// A boolean indicating if post-processing was successful
        /// </returns>

        override protected bool AfterLabelUpdate(MIDTreeNode aNode, string aNewName)
        {
            MIDHierarchyNode node;
            FolderProfile folderProf;

            try
            {
                node = (MIDHierarchyNode)aNode;

                if (node.isMainFavoritesFolder ||
                    node.isFavoritesSubFolder)
                {
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Rename(aNode.Profile.Key, aNewName);
                        _dlFolder.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        _dlFolder.CloseUpdateConnection();
                    }

                    folderProf = (FolderProfile)aNode.Profile;
                    folderProf.Name = aNewName;

                }
                else if (node.isMainUserFolder)  // My Hierarchy
                {
                    SAB.ClientServerSession.UpdateMyHierarchy(aNewName, null);
                }
                else if (node.NodeLevel == 0)  // hierarchy
                {
                    // Begin TT#65 - JSmith - Merchandise Explorer "MY" Hierarchy when copy and paste receive Hierarchy Node Conflict message.
                    //HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(node.HierarchyRID);
                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyDataForUpdate(node.HierarchyRID, false);
                    if (hp.HierarchyLockStatus != eLockStatus.Locked)
                    {
                        return false;
                    }
                    // End TT#65
                    if (hp.HierarchyID != aNewName)
                    {
                        HierarchyProfile newhp = SAB.HierarchyServerSession.GetHierarchyData(aNewName);
                        if (newhp.Key == -1)
                        {
                            hp.HierarchyID = aNewName;
                            hp.HierarchyChangeType = eChangeType.update;
                            SAB.HierarchyServerSession.HierarchyUpdate(hp);
                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HierarchyAlreadyExists));
                            return false;
                        }
                    }
                    SAB.HierarchyServerSession.DequeueHierarchy(hp.Key);
                }
                else
                {
                    EditMsgs em = new EditMsgs();
                    // Begin TT#65 - JSmith - Merchandise Explorer "MY" Hierarchy when copy and paste receive Hierarchy Node Conflict message.
                    //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(node.Profile.Key, true, true);
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeDataForUpdate(node.Profile.Key, false);
                    if (hnp.NodeLockStatus != eLockStatus.Locked)
                    {
                        return false;
                    }
                    // End TT#65
                    switch (node.DisplayOption)
                    {
                        case eHierarchyDisplayOptions.IdOnly:
                            hnp.NodeID = aNewName;
                            break;
                        case eHierarchyDisplayOptions.NameOnly:
                            hnp.NodeName = aNewName;
                            break;
                        case eHierarchyDisplayOptions.DescriptionOnly:
                            hnp.NodeDescription = aNewName;
                            break;
                    }
                    hnp.HierarchyRID = node.HierarchyRID;
                    hnp.Parents = node.Parents;
                    hnp.HomeHierarchyParentRID = node.HomeHierarchyParentRID;
                    hnp.NodeChangeType = eChangeType.update;
                    _hm.ProcessNodeProfileInfo(ref em, hnp);
                    SAB.HierarchyServerSession.DequeueNode(hnp.Key);
                    if (em.ErrorFound)
                    {
                        DisplayMessages(em);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method used to determine if a shortcut should be make for this situation
        /// </summary>
        /// <param name="aFromNode">
        /// The MIDTreeNode being copied
        /// </param>
        /// <param name="aToNode">
        /// The MIDTreeNode where new node is being copied to
        /// </param>

        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
        //override protected bool MakeShortcutNode(MIDTreeNode aToNode, ArrayList aNodes))
        override protected bool MakeShortcutNode(MIDTreeNode aToNode, ArrayList aNodes, eCutCopyOperation aCutCopyOperation)
        // End TT#394
        {
            MIDHierarchyNode node;
            TreeNodeClipboardProfile cbProfile;
            bool makeShortcut = false;
            MIDHierarchyNode toNode = (MIDHierarchyNode)aToNode;

            // Begin TT#394 - JSmith - Cut and Paste not performing consistently
            //if (((MIDHierarchyNode)toNode.GetTopSourceNode()).isMainUserFolder ||
            //    ((MIDHierarchyNode)toNode.GetTopSourceNode()).isMainAlternatesFolder)
            if ((((MIDHierarchyNode)toNode.GetTopSourceNode()).isMainUserFolder ||
                ((MIDHierarchyNode)toNode.GetTopSourceNode()).isMainAlternatesFolder ||
                toNode.GetTopSourceNode().isMainFavoriteFolder) &&
                     aCutCopyOperation == eCutCopyOperation.Copy)
            // End TT#394
            {
                foreach (object item in aNodes)
                {
                    if (item is MIDHierarchyNode)
                    {
                        node = (MIDHierarchyNode)item;
                        // Begin Track #6312 - JSmith - Null error when logging in
                        //if (toNode.HomeHierarchyRID != node.HomeHierarchyRID)
                        //{
                        //    makeShortcut = true;
                        //}
                        if (toNode.HierarchyType == eHierarchyType.alternate &&
                            node.HierarchyType == eHierarchyType.organizational)
                        {
                            makeShortcut = true;
                        }
                        // End Track #6312 
                        // Begin Track #6389 - JSmith - Alternate hierarchy making copy instead of reference
                        else if (toNode.HierarchyType == eHierarchyType.alternate &&
                            node.HierarchyRID != toNode.HierarchyRID)
                        {
                            makeShortcut = true;
                        }
                        // End Track #6389
                        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                        else if (node.TreeNodeType == eTreeNodeType.FolderShortcutNode)
                        {
                            makeShortcut = true;
                        }
                        // End TT#394
                    }
                    else if (item is TreeNodeClipboardProfile)
                    {
                        cbProfile = (TreeNodeClipboardProfile)item;
                        // Begin Track #6312 - JSmith - Null error when logging in
                        //if (toNode.HomeHierarchyRID != ((MIDHierarchyNode)cbProfile.Node).HomeHierarchyRID)
                        //{
                        //    makeShortcut = true;
                        //}
                        if (toNode.HierarchyType == eHierarchyType.alternate &&
                            ((MIDHierarchyNode)cbProfile.Node).HierarchyType == eHierarchyType.organizational)
                        {
                            makeShortcut = true;
                        }
                        // Begin Track #6389 - JSmith - Alternate hierarchy making copy instead of reference
                        else if (toNode.HierarchyType == eHierarchyType.alternate &&
                            ((MIDHierarchyNode)cbProfile.Node).HierarchyRID != toNode.HierarchyRID)
                        {
                            makeShortcut = true;
                        }
                        // End Track #6389
                        // End Track #6312 
                    }
                }
            }

            return makeShortcut;
        }

        /// <summary>
        /// Virtual method used to create a shortcut in another folder
        /// </summary>
        /// <param name="aFromNode">
        /// The MIDTreeNode being copied
        /// </param>
        /// <param name="aToNode">
        /// The MIDTreeNode where new node is being copied to
        /// </param>

        override public void CreateShortcut(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
			if (aToNode.GetTopSourceNode().isMainFavoriteFolder)
            {
                CreateFavoriteShortcut(aFromNode, aToNode);
            }
            else
            {
                MakeShortCut((MIDHierarchyNode)aFromNode, (MIDHierarchyNode)aToNode);
            }
        }

        private void CreateFavoriteShortcut(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDHierarchyNode toNode;
            MIDHierarchyNode newNode = null;
            int collapsedImageIndex;
            int expandedImageIndex;
            HierarchyNodeProfile hnp;

            try
            {
                switch (aToNode.NodeProfileType)
                {
                    case eProfileType.HierarchyNode:

                        toNode = (MIDHierarchyNode)aToNode.Parent;
                        break;

                    default:

                        toNode = (MIDHierarchyNode)aToNode;
                        break;
                }

                if (_dlFolder.Folder_Shortcut_Exists(toNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType))
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                switch (aFromNode.NodeProfileType)
                {
                    case eProfileType.MerchandiseSubFolder:
                        newNode = new MIDHierarchyNode(
                            SAB,
                            eTreeNodeType.FolderShortcutNode,
                            aFromNode.Profile,
                            aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                            toNode.Profile.Key,
                            aFromNode.UserId,
                            aFromNode.FunctionSecurityProfile,
                            cClosedShortcutFolderImage,
                            cClosedShortcutFolderImage,
                            cOpenShortcutFolderImage,
                            cOpenShortcutFolderImage,
                            aFromNode.OwnerUserRID);

                        if (Folder_Children_Exists(SAB.ClientServerSession.UserRID, aFromNode.Profile.Key))
                        {
                            newNode.Nodes.Add(BuildPlaceHolderNode());
                            newNode.ChildrenLoaded = false;
                            newNode.HasChildren = true;
                            newNode.DisplayChildren = true;
                        }
                        else
                        {
                            newNode.ChildrenLoaded = true;
                            newNode.HasChildren = false;
                        }

                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, eProfileType.MerchandiseSubFolder);
                            _dlFolder.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            _dlFolder.CloseUpdateConnection();
                        }

                        break;

                    case eProfileType.HierarchyNode:
                        // Begin TT#27 - JSmith - The “children” do not come over under the folder after copy/paste
                        //hnp = (HierarchyNodeProfile)aFromNode.Profile;
                        hnp = SAB.HierarchyServerSession.GetNodeData(((HierarchyNodeProfile)aFromNode.Profile).Key, false);
                        // End TT#27
                        collapsedImageIndex = GetFolderImageIndex(true, hnp.NodeColor, MIDGraphics.ClosedFolder);
                        expandedImageIndex = GetFolderImageIndex(true, hnp.NodeColor, MIDGraphics.OpenFolder);

                        newNode = new MIDHierarchyNode(
                        SAB,
                        eTreeNodeType.FolderShortcutNode,
                        aFromNode.Profile,
                        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                        //aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                        aFromNode.Text,
                        // End TT#394
                        toNode.Profile.Key,
                        SAB.ClientServerSession.UserRID,
                        null,
                        collapsedImageIndex,
                        collapsedImageIndex,
                        expandedImageIndex,
                        expandedImageIndex,
                        hnp.HomeHierarchyOwner);

                        // Begin TT#5644 - JSmith - Security Set Up - Users Need to Remove Article Lists from My Favorites
                        newNode.FunctionSecurityProfile.SetAllowDelete();
                        newNode.HierarchyNodeSecurityProfile.SetAllowDelete();
                        // End TT#5644 - JSmith - Security Set Up - Users Need to Remove Article Lists from My Favorites

                        if (hnp.HasChildren)
                        {
                            newNode.Nodes.Add(BuildPlaceHolderNode());
                            newNode.ChildrenLoaded = false;
                            newNode.HasChildren = true;
                            newNode.DisplayChildren = hnp.DisplayChildren;
                        }
                        else
                        {
                            newNode.ChildrenLoaded = true;
                            newNode.HasChildren = false;
                        }

                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, newNode.Profile.ProfileType);
                            _dlFolder.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            _dlFolder.CloseUpdateConnection();
                        }

                        break;
                }

                try
                {
                    toNode.Nodes.Add(newNode);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method executed after the New Folder menu item has been clicked.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode that was clicked on
        /// </param>
        /// <param name="aUserId">
        /// The user Id to create the Folder under
        /// </param>

		override protected MIDTreeNode CreateNewFolder(MIDTreeNode aNode, int aUserId)
        {
            FolderProfile newFolderProf;
            string newNodeName;
            MIDHierarchyNode newNode;

            try
            {
                newNodeName = FindNewFolderName("New Folder", aUserId, aNode.Profile.Key, eProfileType.MerchandiseSubFolder);

                newFolderProf = Folder_Create_New(aNode.Profile.Key, aUserId, eProfileType.MerchandiseSubFolder);

                //BeginUpdate();

                try
                {
                    newNode = new MIDHierarchyNode(
                        SAB,
                        eTreeNodeType.SubFolderNode,
                        newFolderProf,
                        newFolderProf.Name,
                        aNode.Profile.Key,
                        SAB.ClientServerSession.UserRID,
                        null,
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        SAB.ClientServerSession.UserRID);

                    newNode.HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(newNode.Profile.Key);
                    newNode.HierarchyNodeSecurityProfile.SetFullControl();

                    _folderNodeHash[newFolderProf.Key] = newNode;
                    aNode.Nodes.Insert(0, newNode);
                    RebuildShortcuts(_favoritesNode, aNode);
                    aNode.Expand();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                
                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void UpdateShortcutChildren(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDHierarchyNode newNode = null;
            ArrayList children;
            bool nodeFound;

            try
            {
                children = ((MIDHierarchyNode)aFromNode).BuildChildrenList();
                // add new nodes
                foreach (MIDHierarchyNode addNode in children)
                {
                    nodeFound = false;
                    foreach (MIDHierarchyNode currNode in aToNode.Nodes)
                    {
                        if (currNode.Profile.ProfileType == addNode.Profile.ProfileType &&
                            currNode.Profile.Key == addNode.Profile.Key)
                        {
                            nodeFound = true;
                            break;
                        }
                    }
                    if (!nodeFound)
                    {
                        if (aToNode.Profile is HierarchyNodeProfile)
                        {
                            newNode = BuildNode(((MIDHierarchyNode)aToNode).HierarchyRID,
                            ((MIDHierarchyNode)aToNode).HierarchyType,
                            (MIDHierarchyNode)aToNode,
                            (HierarchyNodeProfile)addNode.Profile,
                            false);
                            aToNode.Nodes.Add(newNode);
                        }
                    }
                }

                // remove deleted nodes
                foreach (MIDHierarchyNode currNode in aToNode.Nodes)
                {
                    nodeFound = false;
                    foreach (MIDHierarchyNode delNode in children)
                    {
                        if (currNode.Profile.ProfileType == delNode.Profile.ProfileType &&
                            currNode.Profile.Key == delNode.Profile.Key)
                        {
                            nodeFound = true;
                            break;
                        }
                    }
                    if (!nodeFound)
                    {
                        aToNode.Nodes.Remove(currNode);
                    }
                }

                SortChildNodes(aToNode);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void RebuildShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
        {
            try
            {
                foreach (MIDHierarchyNode node in aStartNode.Nodes)
                {
                    if (node.ChildrenLoaded)
                    {
                        if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
                        {
                            node.RefreshShortcutNode(aChangedNode);

							if (node.isObjectShortcut || node.isFolderShortcut)
                            {
                                if (node.Profile.ProfileType == eProfileType.HierarchyNode)
                                {
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									//node.Text = ((HierarchyNodeProfile)aChangedNode.Profile).Text + " (" + GetUserName(aChangedNode.UserId) + ")";
									node.InternalText = ((HierarchyNodeProfile)aChangedNode.Profile).Text + " (" + GetUserName(aChangedNode.UserId) + ")";
									//End Track #6201 - JScott - Store Count removed from attr sets
                                    UpdateShortcutChildren(aChangedNode, node);
                                }
                                else 
                                {
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									//node.Text = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
									node.InternalText = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
									//End Track #6201 - JScott - Store Count removed from attr sets
                                    UpdateShortcutChildren(aChangedNode, node);
                                    //DeleteChildNodes(node);
                                    //CreateShortcutChildren(aChangedNode, node);
                                }
                            }
                            else if (node.isChildShortcut)
                            {
                                if (node.Profile.ProfileType == eProfileType.HierarchyNode)
                                {
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									//node.Text = ((HierarchyNodeProfile)aChangedNode.Profile).Text + " (" + GetUserName(aChangedNode.UserId) + ")";
									node.InternalText = ((HierarchyNodeProfile)aChangedNode.Profile).Text + " (" + GetUserName(aChangedNode.UserId) + ")";
									//End Track #6201 - JScott - Store Count removed from attr sets
                                    UpdateShortcutChildren(aChangedNode, node);
                                }
                                else
                                {
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									//node.Text = ((FolderProfile)aChangedNode.Profile).Name;
									node.InternalText = ((FolderProfile)aChangedNode.Profile).Name;
									//End Track #6201 - JScott - Store Count removed from attr sets
                                    UpdateShortcutChildren(aChangedNode, node);
                                    //DeleteChildNodes(node);
                                    //CreateShortcutChildren(aChangedNode, node);
                                }
                            }
                        }
                        else if (node.NodeProfileType == eProfileType.MerchandiseSubFolder ||
							node.isFolderShortcut ||
                            (node.isChildShortcut && node.Profile.ProfileType == eProfileType.MerchandiseSubFolder))
                        {
                            RebuildShortcuts(node, aChangedNode);
                        }
                        else if (node.NodeProfileType == eProfileType.HierarchyNode ||
							node.isFolderShortcut ||
                            (node.isChildShortcut && node.Profile.ProfileType == eProfileType.HierarchyNode))
                        {
                            RebuildShortcuts(node, aChangedNode);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void DeleteShortcuts(MIDTreeNode aStartNode, MIDTreeNode aDeleteNode)
        {
            object[] deleteList;

            try
            {
                deleteList = new object[aStartNode.Nodes.Count];
                aStartNode.Nodes.CopyTo(deleteList, 0);

                foreach (MIDHierarchyNode node in deleteList)
                {
                    if (node.Profile == null)
                    {
                        
                    }
                    else if (node.Profile.Key == aDeleteNode.Profile.Key && node.Profile.ProfileType == aDeleteNode.Profile.ProfileType &&
                        (node.isShortcut || node.isChildShortcut))
                    {
                        DeleteChildNodes(node);
                        node.Remove();
                    }
                    else if (node.NodeProfileType == eProfileType.MerchandiseSubFolder ||
						node.isFolderShortcut ||
                        (node.isChildShortcut && node.Profile.ProfileType == eProfileType.MerchandiseSubFolder))
                    {
                        DeleteShortcuts(node, aDeleteNode);
                    }
                    else if (node.NodeProfileType == eProfileType.HierarchyNode ||
						node.isFolderShortcut ||
                        (node.isChildShortcut && node.Profile.ProfileType == eProfileType.HierarchyNode))
                    {
                        DeleteShortcuts(node, aDeleteNode);
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
        /// Virtual method used to move a MIDTreeNode from one place to another
        /// </summary>
        /// <param name="aFromNode">
        /// The MIDTreeNode being moved
        /// </param>
        /// <param name="aToNode">
        /// The MIDTreeNode where new node is being move to
        /// </param>

        override protected MIDTreeNode MoveNode(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDProductCharNode charNode;
            try
            {
                try
                {
					if (((MIDHierarchyNode)aFromNode.GetTopSourceNode()).isFavoritesFolder)
                    {
                        MoveFolderNode((MIDHierarchyNode)aFromNode, (MIDHierarchyNode)aToNode);
                    }
                    else
                    {
                        MoveHierarchyNode((MIDHierarchyNode)aFromNode, (MIDHierarchyNode)aToNode);
                    }
                }
                catch
                {
                    throw;
                }
                return null;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override protected string FindNewFolderName(string aFolderName, int aUserRID, int aParentRID, eProfileType aItemType)
        {
            return Folder_Find_New_Name(aFolderName, aUserRID, aParentRID, aItemType);
        }

        private void MoveHierarchyNode(MIDHierarchyNode aFromNode, MIDHierarchyNode aToNode)
        {
            // Begin Track #6312 - JSmith - Null error when logging in
            HierarchyNodeProfile hnp = null;
            // End Track #6312
            //Begin TT#394 - JSmith - Cut and Paste not performing consistently
            bool bRebuildNode = false;
            //End TT#394

            try
            {
                int hierarchyRID;
                int parentRID;
                int parentNodeLevel;
                MIDHierarchyNode fromNode = (MIDHierarchyNode)aFromNode.Parent;
                if (fromNode != null)
                {
                    hierarchyRID = fromNode.HierarchyRID;
                    parentRID = fromNode.Profile.Key;
                    parentNodeLevel = fromNode.NodeLevel;
                }
                else
                {
                    // Begin Track #6312 - JSmith - Null error when logging in
                    //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(aFromNode.Profile.Key);
                    hnp = SAB.HierarchyServerSession.GetNodeData(aFromNode.Profile.Key);
                    // End Track #6312
                    hierarchyRID = hnp.HomeHierarchyRID;
                    parentRID = hnp.HomeHierarchyParentRID;
                    parentNodeLevel = hnp.HomeHierarchyLevel - 1;
                }
                HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                hjp.JoinChangeType = eChangeType.update;
                hjp.OldHierarchyRID = hierarchyRID;
                hjp.OldParentRID = parentRID;
                hjp.NewHierarchyRID = aToNode.HierarchyRID;
                hjp.NewParentRID = aToNode.Profile.Key;
                hjp.Key = aFromNode.Profile.Key;
                HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID);
                if (hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[parentNodeLevel];
                    hjp.LevelType = hlp.LevelType;
                }
                SAB.HierarchyServerSession.OpenUpdateConnection();

                SAB.HierarchyServerSession.JoinUpdate(hjp);

                // Begin Track #6312 - JSmith - Null error when logging in
                if (!aFromNode.isShortcut &&
                    aFromNode.HomeHierarchyRID != aToNode.HomeHierarchyRID)
                {
                    // Begin Track #6318 - JSmith - Tried to drag node with the same name into another user
                    //hnp = SAB.HierarchyServerSession.GetNodeData(aFromNode.Profile.Key);
                    //hnp.HomeHierarchyRID = aToNode.HomeHierarchyRID;
                    //hnp.NodeChangeType = eChangeType.update;
                    //SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                    hnp = SAB.HierarchyServerSession.GetNodeData(aFromNode.Profile.Key);
                    HierarchyProfile newhp = SAB.HierarchyServerSession.GetHierarchyData(aToNode.HomeHierarchyRID);
                    SAB.HierarchyServerSession.UpdateHomeHierarchy(hnp, hp, newhp, aToNode.HomeHierarchyLevel + 1);
                    // End Track #6318

                    //Begin TT#394 - JSmith - Cut and Paste not performing consistently
                    bRebuildNode = true;
                    //End TT#394
                }
                // End Track #6312
                //Begin TT#394 - JSmith - Cut and Paste not performing consistently
                else if (aFromNode.HomeHierarchyRID != aToNode.HomeHierarchyRID)
                {
                    bRebuildNode = true;
                }
                //End TT#394

                if (aFromNode.HierarchyType == eHierarchyType.alternate &&
                    aFromNode.HomeHierarchyRID == aFromNode.HierarchyRID &&
                    aFromNode.HomeHierarchyLevel != aToNode.HomeHierarchyLevel + 1)
                {
                    ((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyLevel = aToNode.HomeHierarchyLevel + 1;
                    ((HierarchyNodeProfile)aFromNode.Profile).NodeLevel = aFromNode.HomeHierarchyLevel;
                    SAB.HierarchyServerSession.UpdateHomeLevel(aFromNode.Profile.Key, aFromNode.HomeHierarchyRID, aFromNode.HomeHierarchyLevel);
                    EditRecursiveHomeLevel(aFromNode);
                }
                SAB.HierarchyServerSession.CommitData();

                Cursor.Current = Cursors.WaitCursor;
                BeginUpdate();
                Nodes.Remove(aFromNode);
                // Begin TT#23 - JSmith - Cannot Cut/Paste referenced node in My Hierarchies
                //((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyRID = aToNode.HierarchyRID;
                //((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyParentRID = aToNode.Profile.Key;
                //((HierarchyNodeProfile)aFromNode.Profile).Parents = aToNode.Parents;
                if (aFromNode.TreeNodeType != eTreeNodeType.FolderShortcutNode)
                {
                    // Begin TT#355 - JSmith - deleting node in alternate also deleted from organizational hierarchy
                    //((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyRID = aToNode.HierarchyRID;
                    //((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyParentRID = aToNode.Profile.Key;
                    if (((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyRID == aToNode.HierarchyRID)
                    {
                        ((HierarchyNodeProfile)aFromNode.Profile).HomeHierarchyParentRID = aToNode.Profile.Key;
                    }
                    ((HierarchyNodeProfile)aFromNode.Profile).Parents = aToNode.Parents;
                    // End TT#355
                }
                // End TT#23
                //Begin TT#394 - JSmith - Cut and Paste not performing consistently
                if (bRebuildNode)
                {
                    hnp = SAB.HierarchyServerSession.GetNodeData(aFromNode.Profile.Key);
                    aFromNode = BuildNode(aToNode.HierarchyRID, aToNode.HierarchyType, aToNode, hnp, aToNode.FunctionSecurityProfile.AccessDenied);
                }
                //End TT#394
                aToNode.Nodes.Add(aFromNode);
                SortChildNodes(aToNode);

                TreeNodeCollection nodes = Nodes;  // update all occurrances of the node
                foreach (MIDHierarchyNode tn in nodes)
                {
                    if (tn.Profile.Key == aFromNode.Profile.Key &&
                        tn.Profile.ProfileType == aFromNode.Profile.ProfileType)
                    {
                        ((HierarchyNodeProfile)tn.Profile).HomeHierarchyRID = aFromNode.HomeHierarchyRID;
                    }
                    EditRecursiveHomeHierarchy(tn, aFromNode);
                }

                EndUpdate();
            }
            catch 
            {
                throw;
            }
            finally
            {
                if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                {
                    SAB.HierarchyServerSession.CloseUpdateConnection();
                }

                EndUpdate();
            }
        }

        private void MoveFolderNode(MIDHierarchyNode aFromNode, MIDHierarchyNode aToNode)
        {
            try
            {
                if (aFromNode.isHierarchyNode)
                {
                    Folder_Move_Shortcut(((MIDHierarchyNode)aFromNode.Parent).Profile.Key,
                        aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                }
                else
                {
                    Folder_Move(((MIDHierarchyNode)aFromNode.Parent).Profile.Key,
                        aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                }
                Cursor.Current = Cursors.WaitCursor;
                BeginUpdate();
                Nodes.Remove(aFromNode);
                aToNode.Nodes.Add(aFromNode);
            }
            catch
            {
                throw;
            }
            finally
            {
                EndUpdate();
            }
        }

        private void MakeShortCut(MIDHierarchyNode copyNode, MIDHierarchyNode toNode)
        {
            try
            {
                if (toNode.isHierarchyNode)
                {
                    HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                    hjp.JoinChangeType = eChangeType.add;
                    hjp.NewHierarchyRID = toNode.HierarchyRID;
                    hjp.NewParentRID = toNode.Profile.Key;
                    hjp.Key = copyNode.Profile.Key;
                    // Begin TT#4760 - JSmith - Search function in Alternate Hierarchy-> Select Filter-> highlight product in filter-> get DB error message-> still copies correct node into Alternate- need a better message
                    if (SAB.HierarchyServerSession.IsParentChild(hjp.NewHierarchyRID, hjp.NewParentRID, hjp.Key))
                    {
                        return;
                    }
                    // End TT#4760 - JSmith - Search function in Alternate Hierarchy-> Select Filter-> highlight product in filter-> get DB error message-> still copies correct node into Alternate- need a better message
                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(toNode.HierarchyRID);
                    if (hp.HierarchyType == eHierarchyType.organizational)
                    {
                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[toNode.NodeLevel];
                        hjp.LevelType = hlp.LevelType;
                    }
                    SAB.HierarchyServerSession.JoinUpdate(hjp);

                    Cursor.Current = Cursors.WaitCursor;
                    BeginUpdate();
                    //MIDHierarchyNode mtn = (MIDHierarchyNode)copyNode.Clone();
                    MIDHierarchyNode mtn = BuildNode(toNode.HierarchyRID, toNode.HierarchyType, (MIDHierarchyNode)toNode, (HierarchyNodeProfile)copyNode.Profile, toNode.FunctionSecurityProfile.AccessDenied);
                    if (toNode.HomeHierarchyType == eHierarchyType.alternate)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//mtn.Text = SAB.HierarchyServerSession.GetNodeData(mtn.Profile.Key, true, true).Text;
						mtn.InternalText = SAB.HierarchyServerSession.GetNodeData(mtn.Profile.Key, true, true).Text;
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
                    mtn.ImageIndex = GetFolderImageIndex(mtn.HierarchyRID != mtn.HomeHierarchyRID, mtn.NodeColor, MIDGraphics.ClosedFolder);
                    mtn.SelectedImageIndex = mtn.ImageIndex;
                    if (mtn.HasChildren
                        && mtn.DisplayChildren)
                    {
                        mtn.Nodes.Add(BuildPlaceHolderNode());
                        mtn.ChildrenLoaded = false;
                    }
                    else
                    {
                        mtn.ChildrenLoaded = true;
                    }
                    toNode.Nodes.Add(mtn);
                    //InsertNode(mtn, toNode);
                }
                else if (toNode.isFavoritesFolder)
                {
                    Folder_Create_Shortcut(toNode.Profile.Key, copyNode.Profile.Key, copyNode.Profile.ProfileType);

                    Cursor.Current = Cursors.WaitCursor;
                    BeginUpdate();
                    MIDHierarchyNode mtn = (MIDHierarchyNode)copyNode.Clone();
                    if (toNode.HomeHierarchyType == eHierarchyType.alternate)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//mtn.Text = SAB.HierarchyServerSession.GetNodeData(mtn.Profile.Key, true, true).Text;
						mtn.InternalText = SAB.HierarchyServerSession.GetNodeData(mtn.Profile.Key, true, true).Text;
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
                    mtn.ImageIndex = GetFolderImageIndex(mtn.HierarchyRID != mtn.HomeHierarchyRID, mtn.NodeColor, MIDGraphics.ClosedFolder);
                    mtn.SelectedImageIndex = mtn.ImageIndex;
                    if (mtn.isHierarchyNode &&
                        mtn.HasChildren
                        && mtn.DisplayChildren)
                    {
                        mtn.Nodes.Add(BuildPlaceHolderNode());
                        mtn.ChildrenLoaded = false;
                    }
                    else if (mtn.isFavoritesSubFolder &&
                        Folder_Children_Exists(SAB.ClientServerSession.UserRID, copyNode.Profile.Key))
                    {
                        mtn.Nodes.Add(BuildPlaceHolderNode());
                        mtn.ChildrenLoaded = false;
                    }
                    else
                    {
                        mtn.ChildrenLoaded = true;
                    }
                    //InsertNode(mtn, toNode);
                    toNode.Nodes.Add(mtn);
                }
                SortChildNodes(toNode);
            }
            catch 
            {
                throw;
            }

            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Virtual method used to copy a MIDTreeNode from one place to another
        /// </summary>
        /// <param name="aFromNode">
        /// The MIDTreeNode being copied
        /// </param>
        /// <param name="aToNode">
        /// The MIDTreeNode where new node is being copied to
        /// </param>
        /// <param name="aFindUniqueName">
        /// A boolean indicating if the procedure should insure create a unique name in case of a duplicate
        /// </param>

        override protected MIDTreeNode CopyNode(MIDTreeNode aFromNode, MIDTreeNode aToNode, bool aFindUniqueName)
        {
            if (aFromNode.Profile.ProfileType == eProfileType.HierarchyNode)
            {
                return CopyHierarchyNode((MIDHierarchyNode)aFromNode, (MIDHierarchyNode)aToNode, aFindUniqueName);
            }
            else
            {
                return CopyFolderNode((MIDHierarchyNode)aFromNode, (MIDHierarchyNode)aToNode, aFindUniqueName);
            }
        }

        private MIDTreeNode CopyHierarchyNode(MIDHierarchyNode aFromNode, MIDHierarchyNode aToNode, bool aFindUniqueName)
        {
            MIDHierarchyNode newNode;
            try
            {
                newNode = CopyNodeInTreeview(aFromNode, aToNode, aFromNode.HasChildren);
                if (newNode != null &&
                    aFromNode.HasChildren)
                {
                    EditMsgs em = new EditMsgs();
                    HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetHierarchyChildren(aFromNode.NodeLevel, aFromNode.HierarchyRID, aFromNode.HomeHierarchyRID, aFromNode.Profile.Key, false, eNodeSelectType.All, aFromNode.HierarchyNodeSecurityProfile.AccessDenied);
                    foreach (HierarchyNodeProfile hnp in hierarchyChildrenList) // copy children
                    {
                        if (hnp.HomeHierarchyRID == newNode.HierarchyRID) 
                        {
                            CopyDescendants(ref em, hnp.Key, hnp.NodeLevel, hnp.HierarchyRID, hnp.HomeHierarchyRID, newNode.Profile.Key);
                        }
                    }

                    CopyShortCuts(aFromNode, newNode); 
                }
                OrderHierarchyFolders();
                return newNode;
            }
            catch
            {
                throw;
            }
        }

        private MIDHierarchyNode CopyFolderNode(MIDHierarchyNode aFromNode, MIDHierarchyNode aToNode, bool aFindUniqueName)
        {
            FolderProfile folderProf;
            MIDHierarchyNode newNode;

            try
            {
                if (aFromNode.isSubFolder)
                {
                    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                    folderProf.UserRID = aToNode.UserId;

                    if (aFindUniqueName)
                    {
                        folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.FilterStoreSubFolder);
                    }

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        folderProf.Key = _dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, eProfileType.FilterStoreSubFolder);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, folderProf.Key, eProfileType.FilterStoreSubFolder);

                        _dlFolder.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        _dlFolder.CloseUpdateConnection();
                    }

                    newNode = aFromNode.BuildFolderNode(folderProf, false);

                    aToNode.Nodes.Add(newNode);

                    foreach (MIDHierarchyNode node in aFromNode.Nodes)
                    {
                        CopyFolderNode(node, newNode, aFindUniqueName);
                    }

                    return newNode;
                }
				else if (aFromNode.isFolderShortcut)
                {
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Shortcut_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType);
                        _dlFolder.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        _dlFolder.CloseUpdateConnection();
                    }

                    newNode = new MIDHierarchyNode(
                        SAB,
                        eTreeNodeType.FolderShortcutNode,
                        aFromNode.Profile,
                        aFromNode.Text,
                        aToNode.Profile.Key,
                        aFromNode.UserId,
                        aFromNode.FunctionSecurityProfile,
                        cClosedShortcutFolderImage,
                        cClosedShortcutFolderImage,
                        cOpenShortcutFolderImage,
                        cOpenShortcutFolderImage,
                        aFromNode.OwnerUserRID);

                    if (aFromNode.HasChildren && aFromNode.DisplayChildren)
                    {
                        newNode.Nodes.Add(BuildPlaceHolderNode());
                        newNode.ChildrenLoaded = false;
                    }
                    else
                    {
                        newNode.ChildrenLoaded = true;
                    }

                    aToNode.Nodes.Add(newNode);

                    return newNode;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch 
            {
                throw;
            }
        }

        private void CopyShortCuts(MIDHierarchyNode copyNode, MIDHierarchyNode newNode)
        {
            ArrayList copyChildNodes = copyNode.BuildChildrenList();
            ArrayList newChildNodes = newNode.BuildChildrenList();
            int arrayIndex = -1;

            foreach (MIDHierarchyNode node in copyChildNodes)
            {
                if (node.HomeHierarchyRID != newNode.HierarchyRID)
                {
                    // Begin TT#400 - JSmith - Merchandise Explorer Copy/Paste making a Copy of in the Alternate Hierarchy received a Object Reference Error.
                    IncrementProgressStatusCount();
                    // End TT#400
                    MakeShortCut(node, newNode);
                }
                else
                {
                    arrayIndex++;
                    if (node.HasChildren)
                    {
                        CopyShortCuts(node, (MIDHierarchyNode)newChildNodes[arrayIndex]);
                    }
                }
            }
        }

        private void CopyDescendants(ref EditMsgs em, int copyNodeRID, int copyNodeLevel,
            int copyNodeHierarchyRID, int copyNodeHomeHierarchyRID, int toNodeRID)
        {
            try
            {
                IncrementProgressStatusCount();
                HierarchyNodeProfile newNode = _hm.CopyNode(ref em, copyNodeRID, toNodeRID);
                HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetHierarchyChildren(copyNodeLevel, copyNodeHierarchyRID, copyNodeHomeHierarchyRID, copyNodeRID, false, eNodeSelectType.All);
                foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                {
                    if (hnp.HomeHierarchyRID == newNode.HierarchyRID) 
                    {
                        CopyDescendants(ref em, hnp.Key, hnp.NodeLevel, hnp.HierarchyRID, hnp.HomeHierarchyRID, newNode.Key);
                    }

                }
            }
            catch 
            {
                throw;
            }
        }

        private MIDHierarchyNode CopyNodeInTreeview(MIDHierarchyNode copyNode, MIDHierarchyNode toNode, bool copyDescendants)
        {
            try
            {
                EditMsgs em = new EditMsgs();
                IncrementProgressStatusCount();
                HierarchyNodeProfile hnp = _hm.CopyNode(ref em, copyNode.Profile.Key, toNode.Profile.Key);
                if (em.ErrorFound)
                {
                    DisplayMessages(em);
                }
                else
                {
                    Sorted = true;
                    Cursor.Current = Cursors.WaitCursor;
                    MIDHierarchyNode mtn = BuildNode(toNode.HierarchyRID, toNode.HierarchyType, (MIDHierarchyNode)toNode, hnp, false);
                    ((HierarchyNodeProfile)mtn.Profile).HomeHierarchyParentRID = toNode.Profile.Key;
                    ((HierarchyNodeProfile)mtn.Profile).Parents = toNode.Parents;
                    if (toNode.HierarchyType == eHierarchyType.alternate)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//mtn.Text = hnp.Text;
						mtn.InternalText = hnp.Text;
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
                    else
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//mtn.Text = hnp.LevelText;
						mtn.InternalText = hnp.LevelText;
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
                    mtn.NodeID = hnp.NodeID;
                    mtn.NodeChangeType = eChangeType.none;
                    mtn.ImageIndex = GetFolderImageIndex(mtn.HierarchyRID != mtn.HomeHierarchyRID, mtn.NodeColor, MIDGraphics.ClosedFolder);
                    mtn.SelectedImageIndex = mtn.ImageIndex;
                    mtn.HasChildren = copyDescendants;
                    mtn.DisplayChildren = hnp.DisplayChildren;
                    
                    if (mtn.HasChildren && mtn.DisplayChildren)
                    {
                        mtn.Nodes.Add(BuildPlaceHolderNode());
                        mtn.ChildrenLoaded = false;
                    }
                    else
                    {
                        mtn.ChildrenLoaded = true;
                    }
                    toNode.Nodes.Add(mtn);
                    return mtn;
                }
                return null;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Virtual method used to determine the number if items to be deleted
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being deleted
        /// </param>

        override protected int GetDeleteNodeCount(MIDTreeNode aNode)
        {
            MIDHierarchyNode deleteNode;
            Stack folderItemsToDelete;
            SecurityProfile functionSecurityProfile;
            SecurityProfile securityProfile;
            NodeLockRequestList lockRequestList;
            NodeLockConflictList conflictList;
            NodeLockRequestProfile lockProfile;

            try
            {
                deleteNode = (MIDHierarchyNode)aNode;

                if (deleteNode.isMainUserFolder ||
                    deleteNode.isMainOrganizationalFolder ||
                    deleteNode.isMainAlternatesFolder)
                {
                    // do not allow root nodes to be deleted
                    functionSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
                    functionSecurityProfile.SetAccessDenied();
                    securityProfile = new HierarchyNodeSecurityProfile(Include.NoRID);
                    securityProfile.SetAccessDenied();
                }
                // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                //else if (deleteNode.HierarchyRID != deleteNode.HomeHierarchyRID)
                else if (deleteNode.HierarchyRID != deleteNode.HomeHierarchyRID &&
                    ((MIDHierarchyNode)(deleteNode.Parent)).TreeNodeType != eTreeNodeType.MainFavoriteFolderNode)
                // End TT#373
                {
                    // for reference, check security of parent
                    functionSecurityProfile = ((MIDHierarchyNode)deleteNode.Parent).FunctionSecurityProfile;
                    securityProfile = ((MIDHierarchyNode)deleteNode.Parent).HierarchyNodeSecurityProfile;
                }
                else
                {
                    // check security of hierarchy
                    functionSecurityProfile = ((MIDHierarchyNode)deleteNode).FunctionSecurityProfile;
                    securityProfile = ((MIDHierarchyNode)deleteNode).HierarchyNodeSecurityProfile;
                }

                // check security
                if (!functionSecurityProfile.AllowDelete || !securityProfile.AllowDelete)
                {
                    return 0;
                }

                // lock node
                if (deleteNode.isHierarchyNode &&
                    deleteNode.HierarchyRID == deleteNode.HomeHierarchyRID)
                {
                    lockRequestList = new NodeLockRequestList(eProfileType.NodeLockRequest);
                    conflictList = new NodeLockConflictList(eProfileType.NodeLockConflict);
                    lockProfile = new NodeLockRequestProfile(deleteNode.Profile.Key);
                    lockProfile.HierarchyRID = deleteNode.HomeHierarchyRID;
                    lockProfile.NodeType = deleteNode.Profile.ProfileType;
                    lockRequestList.Add(lockProfile);
                    conflictList = SAB.HierarchyServerSession.LockHierarchyBranchForDelete(lockRequestList);
                    if (conflictList.Count > 0)
                    {
                        string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeLockConflictHeading, false) + System.Environment.NewLine;
                        foreach (NodeLockConflictProfile conflictProfile in conflictList)
                        {
                            // Begin TT#1159 - APicchetti - Improve Messaging
                            string[] errParms = new string[3];
                            errParms.SetValue("Node Lock Conflict", 0);
                            errParms.SetValue(conflictProfile.InUseNodeName.Trim(), 1);
                            errParms.SetValue(conflictProfile.InUseByUserName, 2);
                            string newConflict = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);
                            //string newConflict = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeLockConflictUser, false);
                            //newConflict = newConflict.Replace("{0}", conflictProfile.InUseNodeName);
                            //newConflict = newConflict.Replace("{1}", conflictProfile.InUseByUserName);
                            // End TT#1159 - APicchetti - Improve Messaging
                            text += System.Environment.NewLine + newConflict;
                        }
                        text += System.Environment.NewLine;
                        text += System.Environment.NewLine + SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AllNodesInUse, false);
                        MessageBox.Show(text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return 0;
                    }
                }

                if (deleteNode.HierarchyRID != deleteNode.HomeHierarchyRID)  // must be a reference
                {
                    return 1;
                }
                else
                {
                    if (deleteNode.isHierarchyNode)
                    {
                        return SAB.HierarchyServerSession.GetDescendantCount(deleteNode.HomeHierarchyRID, deleteNode.Profile.Key) + 1;
                    }
                    else
                    {
                        folderItemsToDelete = new Stack();
                        folderItemsToDelete = deleteNode.Get_Folder_Descendants(deleteNode.Profile);
                        return folderItemsToDelete.Count + 1;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Virtual method used to determine the number of descendants for the node
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode 
        /// </param>

        override protected int GetDescendantCount(MIDTreeNode aNode)
        {
            int descendantCount;
            MIDHierarchyNode node;

            try
            {
                node = (MIDHierarchyNode)aNode;
                if (node.Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    descendantCount = SAB.HierarchyServerSession.GetDescendantCount(node.HomeHierarchyRID, node.Profile.Key);
                }
                else
                {
                    // TODO - get count of folder descendants
                    descendantCount = 1;
                }

                return descendantCount;
            }
            catch
            {
                throw;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Virtual method used to determine if a MIDTreeNode is InUse.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being evaluated.
        /// </param>

        override protected void InUseNode(MIDTreeNode aNode)
        {
            try
            {
                if (aNode != null)
                {
                    InUseHierachyNode(aNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override protected bool AllowInUseDelete(ArrayList aDeleteList)
        {
            return InUseDeleteNode(aDeleteList);
        }
        //END TT#110-MD-VStuart - In Use Tool

        /// <summary>
        /// Virtual method used to delete a MIDTreeNode
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being deleted
        /// </param>

        // Begin TT#3630 - JSmith - Delete My Hierarchy
        //override protected void DeleteNode(MIDTreeNode aNode)
        override protected void DeleteNode(MIDTreeNode aNode, out bool aDeleteCancelled)
        // End TT#3630 - JSmith - Delete My Hierarchy
        {
            ////BEGIN TT#110-MD-VStuart - In Use Tool
            //var allowDelete = false;
            //var _nodeArrayList = new ArrayList();
            //_nodeArrayList.Add(aNode.NodeRID);
            //var _eProfileType = new eProfileType();
            //_eProfileType = aNode.NodeProfileType;
            //string inUseTitle =
            //    Regex.Replace(_eProfileType.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
            //DisplayInUseForm(_nodeArrayList, _eProfileType, inUseTitle, false, out allowDelete);
            //if (!allowDelete)
            // //END TT#110-MD-VStuart - In Use Tool
            {
            Stack folderItemsToDelete = new Stack();
            FolderRelationship folderRelationship;
            MIDHierarchyNode deleteNode = null;
            EditMsgs em;
            string msg;  // TT#3630 - JSmith - Delete My Hierarchy
            DialogResult retCode;  // TT#3630 - JSmith - Delete My Hierarchy
            
            try
            {
                // Begin TT#3630 - JSmith - Delete My Hierarchy
                aDeleteCancelled = false;  
                deleteNode = (MIDHierarchyNode) aNode;

                // Begin TT#1338-MD - JSmith - Hierarchy Delete Gives Warning for Shortcut
				// if (((HierarchyNodeProfile)deleteNode.Profile).HomeHierarchyType == eHierarchyType.organizational)
                if (deleteNode.Profile.ProfileType != eProfileType.MerchandiseSubFolder &&  // TT#1433-MD - JSmith - Invalid cast deleting My Favorites sub folder
                    ((HierarchyNodeProfile)deleteNode.Profile).HomeHierarchyType == eHierarchyType.organizational &&
                    !((HierarchyNodeProfile)deleteNode.Profile).isShortcut &&
                    !deleteNode.isShortcut) // TT#1355 - JSmith - Delete from My Favorites gives message
                // End TT#1338-MD - JSmith - Hierarchy Delete Gives Warning for Shortcut					
                {
                    msg = "Deleting nodes from the organizational hierarchy may temporarily lockup the application for all users and processes.";
                    msg += Environment.NewLine + "It is highly recommended that organizational nodes are deleted using Hierarchy Reclass.";
                    msg += Environment.NewLine + "Do you want to cancel the delete?";
                    retCode = MessageBox.Show(msg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (retCode == DialogResult.Yes)
                    {
                        aDeleteCancelled = true;
                        return;
                    }
                }
                // End TT#3630 - JSmith - Delete My Hierarchy

                em = new EditMsgs();
                if (deleteNode.HierarchyRID != deleteNode.HomeHierarchyRID) // must be a reference
                {
                    IncrementProgressStatusCount();
                    try
                    {
                        if (((MIDHierarchyNode) deleteNode.Parent).isFavoritesFolder)
                        {
                            DeleteFolderShortcut(ref em, (MIDHierarchyNode) deleteNode.Parent, deleteNode);
                        }
                        else
                        {
                            DeleteNode(ref em, deleteNode.HierarchyRID, deleteNode.Profile.Key,
                                       deleteNode.HomeHierarchyRID,
                                       deleteNode.NodeLevel, ((MIDHierarchyNode) deleteNode.Parent).Profile.Key,
                                       ((HierarchyNodeProfile)deleteNode.Profile).HomeHierarchyType);  // TT#3630 - JSmith - Delete My Hierarchy
                        }
                    }
                    catch (DatabaseForeignKeyViolation)
                    {
                        // condition is already handled in hierarchy maintenance
                    }
                    catch
                    {
                        throw;
                    }
                    if (em.ErrorFound)
                    {
                        DisplayMessages(em);
                        return;
                    }
                    else
                    {
                        MIDHierarchyNode parent = (MIDHierarchyNode) deleteNode.Parent;
                        RemoveSelectedNode(deleteNode);
                        Nodes.Remove(deleteNode);
                        if (parent.Nodes.Count == 0)
                        {
                            parent.HasChildren = false;
                            parent.ImageIndex = GetFolderImageIndex(parent.HierarchyRID != parent.HomeHierarchyRID,
                                                                    parent.NodeColor, MIDGraphics.ClosedFolder);
                            parent.SelectedImageIndex = parent.ImageIndex;
                        }
                    }
                }
                else
                {
                    _bContinueProcessing = true;
                    if (deleteNode.isHierarchyNode)
                    {
                        HierarchyNodeList hierarchyChildrenList =
                            SAB.HierarchyServerSession.GetHierarchyChildren(deleteNode.NodeLevel,
                                                                            deleteNode.HierarchyRID,
                                                                            deleteNode.HomeHierarchyRID,
                                                                            deleteNode.Profile.Key, false,
                                                                            eNodeSelectType.All);
                        foreach (HierarchyNodeProfile hnp in hierarchyChildrenList) // delete descendants
                        {
                            try
                            {
                                DeleteDescendants(ref em, deleteNode.Profile.Key, hnp);
                                // Begin TT#1045 - JSmith - No message when deleting style with active headers
                                if (em.ErrorFound)
                                {
                                    _bContinueProcessing = false;
                                    break;
                                }
                                // End TT#1045
                            }
                            catch // discontinue if error
                            {
                                _bContinueProcessing = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Begin TT#25 - Explorer Favorites tried to delete a folder that has an organizational node and receive a Invalid Cast Exception
                        folderItemsToDelete = deleteNode.Get_Folder_Descendants(deleteNode.Profile);
                        // End TT#25
                        while (folderItemsToDelete.Count > 0)
                        {
                            folderRelationship = (FolderRelationship) folderItemsToDelete.Pop();
                            switch (folderRelationship.ChildProfile.ProfileType)
                            {
                                case eProfileType.HierarchyNode:
                                    Folder_Delete_Shortcut(folderRelationship.ParentProfile.Key,
                                                           folderRelationship.ChildProfile.Key,
                                                           folderRelationship.ChildProfile.ProfileType);
                                    break;
                                default:
                                    Folder_Delete(folderRelationship.ChildProfile.Key,
                                                  folderRelationship.ChildProfile.ProfileType);
                                    break;
                            }
                            // Begin TT#26 - JSmith - Deleting Percent Complete not correct
                            IncrementProgressStatusCount();
                            // End TT#26
                        }
                    }
                    // delete parent
                    IncrementProgressStatusCount();
                    try
                    {
                        // Begin TT#1045 - JSmith - No message when deleting style with active headers
                        if (_bContinueProcessing)
                        {
                            // End TT#1045
                            if (deleteNode.isHierarchyNode)
                            {
                                DeleteNode(ref em, deleteNode.HierarchyRID, deleteNode.Profile.Key,
                                           deleteNode.HomeHierarchyRID,
                                           deleteNode.NodeLevel, deleteNode.HomeHierarchyParentRID,
                                           ((HierarchyNodeProfile)deleteNode.Profile).HomeHierarchyType);
                            }
                            else
                            {
                                Folder_Delete(deleteNode.Profile.Key, deleteNode.Profile.ProfileType);
                            }
                            // Begin TT#1045 - JSmith - No message when deleting style with active headers
                        }
                        if (em.ErrorFound)
                        {
                            DisplayMessages(em);
                            return;
                        }
                        // End TT#1045
                    }
                    catch (DatabaseForeignKeyViolation)
                    {
                        // condition is already handled in hierarchy maintenance
                    }
                    catch
                    {
                    }
                    if (!em.ErrorFound)
                    {
                        DeleteShortcuts(_favoritesNode, aNode);
                        DeleteShortcuts(_myHierarchyNode, aNode);
                        DeleteShortcuts(_alternateNode, aNode);
                        MIDHierarchyNode parent = (MIDHierarchyNode) deleteNode.Parent;
                        Nodes.Remove(deleteNode);
                        if (parent.Nodes.Count == 0)
                        {
                            parent.HasChildren = false;
                            parent.ImageIndex = GetFolderImageIndex(parent.HierarchyRID != parent.HomeHierarchyRID,
                                                                    parent.NodeColor, MIDGraphics.ClosedFolder);
                            parent.SelectedImageIndex = parent.ImageIndex;
                        }
                    }
                        //Begin TT#1586 - DOConnell - Can't Delete an Alternate Hirearchy
                    else
                    {
                        DisplayMessages(em);
                        return;
                    }
                    //End TT#1586 - DOConnell - Can't Delete an Alternate Hirearchy
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (deleteNode != null)
                {
                    SAB.HierarchyServerSession.DequeueBranch(deleteNode.HomeHierarchyRID, deleteNode.Profile.Key);
                }
            }
        }
}

        //Begin TT#1388-MD -jsobek -Product Filters
        //public void CleanUpExplorer(int aParentRID, int aChildRID)
        //{
        //    try
        //    {
        //        TreeNodeCollection nodes = Nodes;
        //        foreach (MIDHierarchyNode tn in nodes)
        //        {
        //            CleanUpExplorer(aParentRID, aChildRID, tn);
        //        }
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //}
        

        //public void CleanUpExplorer(int aParentRID, int aChildRID, MIDHierarchyNode treeNode)
        //{
        //    // Check each node recursively.
        //    foreach (MIDHierarchyNode tn in treeNode.Nodes)
        //    {
        //        if (tn.Text != "" &&
        //            tn.Profile.Key > 0)	// do not check place holder node for parents with children if parent has not been expanded
        //        {
        //            if (!SAB.HierarchyServerSession.NodeExists(tn.Profile.Key) ||
        //                (tn.Parent != null &&
        //                ((MIDHierarchyNode)tn.Parent).Profile.Key == aParentRID &&
        //                ((MIDHierarchyNode)tn).Profile.Key == aChildRID))
        //            {
        //                treeNode.Nodes.Remove(tn);
        //                if (treeNode.Nodes.Count == 0)
        //                {
        //                    treeNode.HasChildren = false;
        //                    treeNode.ImageIndex = GetFolderImageIndex(treeNode.HierarchyRID != treeNode.HomeHierarchyRID, treeNode.NodeColor, MIDGraphics.ClosedFolder);
        //                    treeNode.SelectedImageIndex = treeNode.ImageIndex;
        //                }
        //            }
        //        }

        //        CleanUpExplorer(aParentRID, aChildRID, tn);
        //    }
        //}
        //End TT#1388-MD -jsobek -Product Filters

        private void DeleteDescendants(ref EditMsgs em, int parentRID, HierarchyNodeProfile delete_hnp)
        {
            try
            {
                if (delete_hnp.HierarchyRID == delete_hnp.HomeHierarchyRID)		// only delete children if node is home
                {
                    HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetHierarchyChildren(delete_hnp.NodeLevel, delete_hnp.HierarchyRID, delete_hnp.HomeHierarchyRID, delete_hnp.Key, false, eNodeSelectType.All);
                    if (hierarchyChildrenList.Count > 0)
                    {
                        foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                        {
                            DeleteDescendants(ref em, delete_hnp.Key, hnp);
                        }
                    }
                }
                if (_bContinueProcessing)
                {
                    IncrementProgressStatusCount();
                    
                    try
                    {
                        // Begin TT#5672 - JSmith - Purge Error on Sunday
						if (delete_hnp.HierarchyRID != delete_hnp.HomeHierarchyRID  // Do not check In Use for shortcuts
                            || !InUseHierachyNode(delete_hnp.Key, delete_hnp.HomeHierarchyOwner))
                        {
						// End TT#5672 - JSmith - Purge Error on Sunday
                            DeleteNode(ref em, delete_hnp.HierarchyRID, delete_hnp.Key, delete_hnp.HomeHierarchyRID,
                                delete_hnp.NodeLevel, parentRID, delete_hnp.HomeHierarchyType);
                        // Begin TT#5672 - JSmith - Purge Error on Sunday
						}
                        else
                        {
                            em.ErrorFound = true;
                            _bContinueProcessing = false;
                        }
						// End TT#5672 - JSmith - Purge Error on Sunday
                    }
                    catch (DatabaseForeignKeyViolation)
                    {
                        // condition is already handled in hierarchy maintenance
                    }
                    catch (MIDDatabaseUnavailableException err)
                    {
                        string exceptionMessage = err.Message;
                        _bContinueProcessing = false;
                    }
                    catch (Exception err)
                    {
                        string exceptionMessage = err.Message;
                        string errors = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorDeleting, false);
                        errors = errors.Replace("{0}", delete_hnp.LevelText);
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errors, this.GetType().Name);
                        errors += Environment.NewLine + SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors);
                        for (int i = 0; i < em.EditMessages.Count; i++)
                        {
                            EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                            errors += Environment.NewLine + "     ";
                            if (emm.messageByCode)
                            {
                                errors += SAB.ClientServerSession.Audit.GetText(emm.code);
                            }
                            else
                            {
                                errors += emm.msg;
                            }
                        }
                        errors += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ContinueQuestion);
                        if (MessageBox.Show(errors, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.No)
                        {
                            _bContinueProcessing = false;
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        // Begin TT#3630 - JSmith - Delete My Hierarchy
        //private void DeleteNode(ref EditMsgs em, int hierarchyRID, int nodeRID, int homeHierarchyRID, int nodeLevel, int parentRID)
        private void DeleteNode(ref EditMsgs em, int hierarchyRID, int nodeRID, int homeHierarchyRID, int nodeLevel, int parentRID, eHierarchyType aHierarchyType)
        // End TT#3630 - JSmith - Delete My Hierarchy
        {
            try
            {
                // Begin TT#1045 - JSmith - No message when deleting style with active headers
                //em.ClearMsgs();
                // End TT#1045
                if (hierarchyRID != homeHierarchyRID)  // delete reference
                {
                    HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                    hjp.JoinChangeType = eChangeType.delete;
                    hjp.OldHierarchyRID = hierarchyRID;
                    hjp.OldParentRID = parentRID;
                    hjp.Key = nodeRID;
                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID);
                    if (hp.HierarchyType == eHierarchyType.organizational)
                    {
                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[nodeLevel];
                        hjp.LevelType = hlp.LevelType;
                    }
                    SAB.HierarchyServerSession.JoinUpdate(hjp);
                }
                else
                {
                    if (nodeLevel == 0)   // delete hierarchy
                    {
                        HierarchyProfile hp = new HierarchyProfile(hierarchyRID);
                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                        if (aHierarchyType == eHierarchyType.organizational)
                        {
                            hp.HierarchyChangeType = eChangeType.delete;
                        }
                        else
                        {
                            hp.HierarchyChangeType = eChangeType.markedForDelete;
                        }
                        // End TT#3630 - JSmith - Delete My Hierarchy
                        _hm.ProcessHierarchyData(ref em, hp);
                    }
                    else
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nodeRID);
                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                        if (aHierarchyType == eHierarchyType.organizational)
                        {
                            hnp.NodeChangeType = eChangeType.delete;
                        }
                        else
                        {
                            hnp.DeleteNode = true;
                            hnp.NodeChangeType = eChangeType.markedForDelete;
                        }
                        // End TT#3630 - JSmith - Delete My Hierarchy
                        hnp.HierarchyRID = hierarchyRID;
                        hnp.HomeHierarchyParentRID = parentRID;
                        if (!hnp.Parents.Contains(parentRID))
                        {
                            hnp.Parents.Add(parentRID);
                        }
                        _hm.ProcessNodeProfileInfo(ref em, hnp);
                    }

                    // Begin TT#29 - JSmith - Deleting node is not removing reference in favorites
                    // Begin TT#1045 - JSmith - No message when deleting style with active headers
                    //Folder_DeleteAll_Shortcut(nodeRID, eProfileType.HierarchyNode);
                    if (!em.ErrorFound)
                    {
                        Folder_DeleteAll_Shortcut(nodeRID, eProfileType.HierarchyNode);
                    }
                    // End TT#1045
                    // End TT#29
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                // condition is already handled in hierarchy maintenance
                throw;
            }
            catch 
            {
                throw;
            }
        }

        private void DeleteFolderShortcut(ref EditMsgs em, MIDHierarchyNode aFolderProfile, MIDHierarchyNode aShortcutItem)
        {
            try
            {
                em.ClearMsgs();
                Folder_Delete_Shortcut(aFolderProfile.Profile.Key, aShortcutItem.Profile.Key, aShortcutItem.Profile.ProfileType);
            }
            catch (DatabaseForeignKeyViolation)
            {
                // condition is already handled in hierarchy maintenance
                throw;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Virtual method executed after the New Item menu item has been clicked.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode that was clicked on
        /// </param>
		//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename

		//override protected void CreateNewItem(MIDTreeNode aParentNode)
		/// <returns>
		/// The new node that was created.  If node is returned, it will be placed in edit mode.
		/// If node is not available or edit mode is not desired, return null.
		/// </returns>

		override protected MIDTreeNode CreateNewItem(MIDTreeNode aParentNode)
		//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
		{
            try
            {
                if (aParentNode.TreeNodeType == eTreeNodeType.MainSourceFolderNode)
                {
                    NewHierarchy((MIDHierarchyNode)aParentNode);
                }
                else
                {
                    NewNode((MIDHierarchyNode)aParentNode);
                }
				//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename

				return null;
				//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
			}
            catch
            {
                throw;
            }
        }

        private void NewHierarchy(MIDHierarchyNode aParentNode)
        {
            // Get the type of hierarchy folder
            MIDHierarchyNode selectedNodeFolder = null;
            if (aParentNode.Parent == null)
            {
                selectedNodeFolder = (MIDHierarchyNode)aParentNode;
            }
            else
            {
                selectedNodeFolder = (MIDHierarchyNode)aParentNode.Parent;
                while (selectedNodeFolder.Parent != null)
                {
                    selectedNodeFolder = (MIDHierarchyNode)selectedNodeFolder.Parent;
                }
            }

            if (selectedNodeFolder.isMainUserFolder)
            {
                frmHierarchyProperties formHierarchyProperties = null;
                formHierarchyProperties = new frmHierarchyProperties(SAB);
                formHierarchyProperties.OnHierPropertyChangeHandler += new frmHierarchyProperties.HierPropertyChangeEventHandler(OnHierPropertiesChange);
                formHierarchyProperties.NewHierarchy(SAB.ClientServerSession.UserRID, eHierarchyType.alternate);
                formHierarchyProperties.MdiParent = MDIParentForm;
                formHierarchyProperties.Show();
            }
            else
            {
                eHierarchyType hierarchyType;
                if (selectedNodeFolder.isMainOrganizationalFolder)
                {
                    hierarchyType = eHierarchyType.organizational;
                }
                else
                {
                    hierarchyType = eHierarchyType.alternate;
                }
                if (hierarchyType == eHierarchyType.organizational &&
                    selectedNodeFolder.Nodes.Count >= 1)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_OnlyOneOrganizational));
                }
                else
                {
                    frmHierarchyProperties formHierarchyProperties = null;
                    formHierarchyProperties = new frmHierarchyProperties(SAB);
                    formHierarchyProperties.OnHierPropertyChangeHandler += new frmHierarchyProperties.HierPropertyChangeEventHandler(OnHierPropertiesChange);
                    formHierarchyProperties.NewHierarchy(Include.GlobalUserRID, hierarchyType);  
                    formHierarchyProperties.MdiParent = MDIParentForm;
                    formHierarchyProperties.Show();
                }
            }
        }

        private void NewNode(MIDHierarchyNode aParentNode)
        {
            if (aParentNode.isMainAlternatesFolder ||
                aParentNode.isMainUserFolder ||
                aParentNode.isMainOrganizationalFolder)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotAddNodesToFolders));
            }
            else
            {
                bool allowAdd = true;
                HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(aParentNode.HierarchyRID);
                if (hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[aParentNode.NodeLevel];
                    if (hp.HierarchyLevels.Count == aParentNode.NodeLevel)		// last level in hierarchy
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotAddNodesBeyondDef));
                        allowAdd = false;
                    }
                    else
                        if (aParentNode.NodeLevel > 0 &&
                            hlp.LevelType == eHierarchyLevelType.Style)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(aParentNode.Profile.Key);
                            if (hnp.ProductType == eProductType.Hardline)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotAddColorToHardlines));
                                allowAdd = false;
                            }
                        }

                }
                if (allowAdd)
                {
                    frmNodeProperties formNodeProperties;
                    formNodeProperties = new frmNodeProperties(SAB);
                    formNodeProperties.OnNodePropertyChangeHandler += new frmNodeProperties.NodePropertyChangeEventHandler(OnNodePropertiesChange);
                    formNodeProperties.NewNode(aParentNode);
                    formNodeProperties.MdiParent = MDIParentForm;
                    formNodeProperties.Show();
                }
            }
        }

        /// <summary>
        /// Virtual method used to edit a MIDTreeNode
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being edited
        /// </param>

        override protected void EditNode(MIDTreeNode aNode)
        {
            try
            {
                OpenForecastReview((MIDHierarchyNode)aNode);
            
            }
            catch
            {
                throw;
            }
        }

        public void OpenForecastReview(MIDHierarchyNode aNode)
        {
            try
            {
                // Begin Track #6198 - JSmith - error double clicking on folder
                // Begin Track #6233 - JSmith - double click recieves relationship not found exception.
                //if (aNode.Profile.ProfileType == eProfileType.MerchandiseSubFolder)
                if (aNode.Profile.ProfileType != eProfileType.HierarchyNode)
                // End Track #6233
                {
                    return;
                }
                // End Track #6198

                if (aNode.Profile.Key == 0)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PlanViewNotAvailable));
                }
                else
                    if (SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aNode.Profile.Key, eSecurityFunctions.ForecastReview, (int)eSecurityTypes.Chain | (int)eSecurityTypes.Store).AccessDenied)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
                    }
                    else
                    {
                        System.Windows.Forms.Form frm = new MIDRetail.Windows.OTSPlanSelection(SAB, aNode.Profile.Key);

                        ((MIDRetail.Windows.OTSPlanSelection)frm).PopulateForm();
                        if (((MIDRetail.Windows.OTSPlanSelection)frm).FormLoaded)
                        {
                            frm.MdiParent = MDIParentForm;
                            // Begin VS2010 WindowState Fix - RMatelic - Maximized window state incorrect when window first opened >>> move WindowState to after Show()
                            //frm.WindowState = FormWindowState.Maximized;
                            frm.Show();
                            frm.WindowState = FormWindowState.Maximized;
                            // End VS2010 WindowState Fix
                        }
                    }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                throw;
            }
        }

        public void EditNodeProperties(MIDHierarchyNode aNode)
        {
            try
            {
                if (aNode.isMainAlternatesFolder ||
                    aNode.isMainOrganizationalFolder ||
                    aNode.isMainUserFolder)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PropertiesNotAvailable));
                }
                else
                {
                    frmNodeProperties formNodeProperties = null;
                    bool nodeFound = false;
                    foreach (Form frm in MDIParentForm.MdiChildren)  // see if store profile already open
                    {
                        if (frm.GetType() == typeof(frmNodeProperties))
                        {
                            frmNodeProperties fnp = (frmNodeProperties)frm;
                            if (fnp.NodeRID == aNode.Profile.Key &&
                                !fnp.IsDisposed)
                            {
                                nodeFound = true;
                                formNodeProperties = fnp;
                                break;
                            }
                        }
                    }

                    if (nodeFound)
                    {
                        formNodeProperties.Focus();
                    }
                    else
                    {
                        aNode.NodeChangeType = eChangeType.add;
                        formNodeProperties = new frmNodeProperties(SAB);
                        formNodeProperties.OnNodePropertyChangeHandler += new frmNodeProperties.NodePropertyChangeEventHandler(OnNodePropertiesChange);
                        SecurityProfile securityProfile = aNode.HierarchyNodeSecurityProfile;
                        if (securityProfile.AccessDenied)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
                        }
                        else
                        {
                            try
                            {
                                formNodeProperties.InitializeForm(aNode, aNode.HomeHierarchyParentRID);

                                formNodeProperties.MdiParent = MDIParentForm;
                                formNodeProperties.Show();
                            }
                            catch (HierarchyNodeConflictException)
                            {
                                // catch exception and do not show form
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void EditHierarchyProperties(MIDHierarchyNode aNode)
        {
            try
            {
                if (aNode.isMainAlternatesFolder ||
                        aNode.isMainOrganizationalFolder)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PropertiesNotAvailable));
                }
                else
                {
                    MIDHierarchyNode hierarchyNode;
                    // get hierarchy node for selected node
                    if (aNode.isMainUserFolder ||
                        aNode.isAlternateHierarchyRoot ||
                        aNode.isOrganizationalHierarchyRoot ||
                        aNode.isMyHierarchyRoot) // stop if hierarchy
                    {
                        hierarchyNode = aNode;
                    }
                    else
                    {
                        hierarchyNode = (MIDHierarchyNode)aNode.Parent;
                        while (!hierarchyNode.isAlternateHierarchyRoot &&
                            !hierarchyNode.isOrganizationalHierarchyRoot &&
                            !hierarchyNode.isMyHierarchyRoot)
                        {
                            hierarchyNode = (MIDHierarchyNode)hierarchyNode.Parent;
                        }
                    }
                    frmHierarchyProperties formHierarchyProperties = null;
                    bool nodeFound = false;
                    foreach (Form frm in MDIParentForm.MdiChildren)  // see if store profile already open
                    {
                        if (frm.GetType() == typeof(frmHierarchyProperties))
                        {
                            frmHierarchyProperties fhp = (frmHierarchyProperties)frm;
                            if (fhp.HierarchyRID == aNode.HomeHierarchyRID)
                            {
                                nodeFound = true;
                                formHierarchyProperties = fhp;
                                break;
                            }
                        }
                    }

                    if (nodeFound)
                    {
                        formHierarchyProperties.Level_Information_Load(aNode.HomeHierarchyLevel - 1);
                        formHierarchyProperties.Focus();
                    }
                    else
                    {
                        // if no form, create one
                        formHierarchyProperties = new frmHierarchyProperties(SAB);
                        formHierarchyProperties.OnHierPropertyChangeHandler += new frmHierarchyProperties.HierPropertyChangeEventHandler(OnHierPropertiesChange);
                        FunctionSecurityProfile securityProfile;
                        if (aNode.isAlternateHierarchyRoot)
                        {
                            securityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aNode.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobal, (int)eSecurityTypes.All);
                        }
                        else
                        {
                            securityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aNode.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodeProperty, (int)eSecurityTypes.All);
                        }

                        if (securityProfile.AccessDenied)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
                        }
                        else
                        {
                            try
                            {
                                formHierarchyProperties.InitializeForm(aNode, aNode.HomeHierarchyLevel);
                                formHierarchyProperties.MdiParent = MDIParentForm;
                                formHierarchyProperties.Show();
                            }
                            catch (HierarchyConflictException)
                            {
                                // catch exception and do not show form
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        override protected MIDTreeNode BuildNode(string aText, int aKey)
        {
            HierarchyNodeProfile hnp;
            try
            {
                hnp = SAB.HierarchyServerSession.GetNodeData(aKey, false);

                return BuildNode(hnp.HomeHierarchyRID, hnp.HomeHierarchyType, null, hnp, false);
            }
            catch
            {
                throw;
            }
        }

        public MIDHierarchyNode BuildNode(int aHierarchyRID, eHierarchyType aHierarchyType, MIDHierarchyNode aParentNode, HierarchyNodeProfile aHierarchyNodeProfile, bool aParentAccessDenied)
        {
            MIDHierarchyNode newNode;
            int collapsedImageIndex;
            int expandedImageIndex;
            eTreeNodeType treeNodeType;
            int userRID;

            try
            {
                collapsedImageIndex = GetFolderImageIndex(aHierarchyNodeProfile.HomeHierarchyRID != aHierarchyRID, aHierarchyNodeProfile.NodeColor, MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(aHierarchyNodeProfile.HomeHierarchyRID != aHierarchyRID, aHierarchyNodeProfile.NodeColor, MIDGraphics.OpenFolder);

                if (aParentNode != null &&
                    aParentNode.HierarchyRID != aHierarchyNodeProfile.HomeHierarchyRID)
                {
                    treeNodeType = eTreeNodeType.FolderShortcutNode;
                }
                else if (aHierarchyNodeProfile.HomeHierarchyRID != aHierarchyRID)
                {
                    treeNodeType = eTreeNodeType.ChildObjectShortcutNode;
                }
                else
                {
                    treeNodeType = eTreeNodeType.ObjectNode;
                }

                if (aHierarchyNodeProfile.HomeHierarchyOwner == Include.GlobalUserRID ||
                    aHierarchyNodeProfile.HomeHierarchyOwner == SAB.ClientServerSession.UserRID)
                {
                    userRID = aHierarchyNodeProfile.HomeHierarchyOwner;
                }
                else
                {
                    userRID = SAB.ClientServerSession.UserRID;
                }

                newNode = new MIDHierarchyNode(
                            SAB,
                            treeNodeType,
                            aHierarchyNodeProfile,
                            aHierarchyNodeProfile.Text,
                            aHierarchyNodeProfile.Key,
                            userRID,
                            null,
                            collapsedImageIndex,
                            collapsedImageIndex,
                            expandedImageIndex,
                            expandedImageIndex,
                            aHierarchyNodeProfile.HomeHierarchyOwner);

				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//newNode.Text = aHierarchyNodeProfile.Text;
				newNode.InternalText = aHierarchyNodeProfile.Text;
				//End Track #6201 - JScott - Store Count removed from attr sets
				newNode.NodeID = aHierarchyNodeProfile.NodeID;
                newNode.NodeChangeType = eChangeType.none;
                newNode.HierarchyRID = aHierarchyRID;
                newNode.HierarchyType = aHierarchyType;

                newNode.HasChildren = aHierarchyNodeProfile.HasChildren;
                newNode.DisplayChildren = aHierarchyNodeProfile.DisplayChildren;

                if (newNode.NodeLevel == 0 ||
                    aParentAccessDenied)
                {
                    newNode.SetSecurity();
                }

                if (newNode.SecurityProfileIsInitialized &&
                    newNode.HierarchyNodeSecurityProfile.AccessDenied)
                {
                    newNode.ForeColor = SystemColors.InactiveCaption;
                }

                if (newNode.HasChildren
                    && newNode.DisplayChildren)
                {
                    newNode.Nodes.Add(BuildPlaceHolderNode());
                    newNode.ChildrenLoaded = false;
                }
                else
                {
                    newNode.ChildrenLoaded = true;
                }

                if (newNode.Profile.ProfileType == eProfileType.Hierarchy)
                {
                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HomeHierarchyRID);
                    if (hp.Owner > Include.GlobalUserRID) // personal hierarchy		
                    {
                        newNode.HierarchyNodeSecurityProfile.SetFullControl();	// override security for personal hierarchies to full access
                    }
                }
                return newNode;
            }
            catch
            {
                throw;
            }
        }

        public MIDHierarchyNode BuildPlaceHolderNode()
        {
            MIDHierarchyNode newNode;
            
            try
            {
                newNode = new MIDHierarchyNode(
                            SAB,
                            eTreeNodeType.ObjectNode,
                            new HierarchyNodeProfile(Include.NoRID),
                            null,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID,
                            new FunctionSecurityProfile(Include.NoRID),
                            Include.NoRID,
                            Include.NoRID,
                            Include.NoRID,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID);

                
                return newNode;
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#5672 - JSmith - Purge Error on Sunday
        private bool InUseHierachyNode(int aNodeRID, int aHomeHierarchyOwner)
        {
           bool dialogShown = false;

            try
            {
                int scgRid = aNodeRID;
                var ridList = new ArrayList();
                bool[] myHierarchyList = new bool[1];

                if (scgRid > 0)
                {
                    ridList.Add(scgRid);
                    myHierarchyList[0] = aHomeHierarchyOwner != Include.GlobalUserRID;
                }

                eProfileType etype = eProfileType.HierarchyNode;
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);

                DisplayInUseForm(ridList, myHierarchyList, etype, inUseTitle, false, out dialogShown);
                return dialogShown;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#5672 - JSmith - Purge Error on Sunday

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private bool InUseHierachyNode(MIDTreeNode aNode)
        {
            const bool inUseSuccessful = false;

            try
            {
                if (aNode != null)
                {
                    int scgRid = aNode.NodeRID;
                    var ridList = new ArrayList();
                    //BEGIN TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID
                    //if (aNode.FunctionSecurityProfile.AllowView)
                    //{
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    bool[] myHierarchyList = new bool[1];

                    if (scgRid > 0)
                    {
                        ridList.Add(scgRid);
                        myHierarchyList[0] = ((MIDHierarchyNode)aNode).HomeHierarchyOwner != Include.GlobalUserRID;
                    }

                    eProfileType etype = aNode.NodeProfileType;
                    //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                    bool display = false;
                    const bool inQuiry = true;
                    DisplayInUseForm(ridList, myHierarchyList, etype, inUseTitle, ref display, inQuiry);
                    // End TT#3630 - JSmith - Delete My Hierarchy
                    //}
                    //END TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID
                }
                return inUseSuccessful;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private bool InUseDeleteNode(ArrayList aDeleteList)
        {
            bool allowDelete = true;
            bool dialogShown = false;
            ArrayList nodeArrayList = new ArrayList();
            eProfileType profileType = eProfileType.None;
            string inUseTitle;

            try
            {
                if (aDeleteList != null)
                {
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    bool[] myHierarchyList = new bool[aDeleteList.Count];  
                    int i = 0;
                    // End TT#3630 - JSmith - Delete My Hierarchy

                    foreach (MIDTreeNode aNode in aDeleteList)
                    {
                        nodeArrayList.Add(aNode.NodeRID);
                        profileType = aNode.NodeProfileType;
                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                        myHierarchyList[i] = ((MIDHierarchyNode)aNode).HomeHierarchyOwner != Include.GlobalUserRID;
                        i++;
                        // End TT#3630 - JSmith - Delete My Hierarchy
                    }
                    //BEGIN TT#647-MD-VStuart-Cannot Remove Node Shortcut in Merchandise Explorer
                    if (!SelectedNode.isShortcut)
                    {
                        if (nodeArrayList.Count > 0)
                        {
                            //inUseTitle = Regex.Replace(profileType.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                            inUseTitle = InUseUtility.GetInUseTitleFromProfileType(profileType); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                            DisplayInUseForm(nodeArrayList, myHierarchyList, profileType, inUseTitle, false, out dialogShown);
                            if (dialogShown)
                            {
                                allowDelete = false;
                            }
                        }
                    }
                    //END   TT#647-MD-VStuart-Cannot Remove Node Shortcut in Merchandise Explorer
                }
                return allowDelete;

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //END TT#110-MD-VStuart - In Use Tool

        //BEGIN TT#110-MD-VStuart - In Use Tool
        // This is a duplicate from C:\SCMVS2010\Working 5.x In Use Tool UC9\Windows\MIDFormBase.cs
        /// <summary>
        /// This is the base object from which the the InUse Info tool is called from.
        /// It's intent is to allow users to be able to find out what objects are in use by other objects.
        /// </summary>
        /// <param name="userRids">This is the list of RIDs we are investigating.</param>
        /// <param name="myEnum">This is the eprofileType that we are investigating.</param>
        /// <param name="itemTitle">This is the title of inquiry.</param>
        /// <param name="display">Indicates that the InUse Dialog should be displayed.</param>
        /// <param name="inQuiry">Indicates if this just an user inquiry or is a mandatory check.</param>
        public void DisplayInUseForm(ArrayList userRids, bool[] myHierarchyList, eProfileType myEnum, string itemTitle, ref bool display, bool inQuiry)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            myfrm.PersonalHierarchy = myHierarchyList;  // TT#3630 - JSmith - Delete My Hierarchy
            myfrm.ResolveInUseData(ref display, inQuiry);
            if (display == true)
            { myfrm.ShowDialog(); }
        }

        public void DisplayInUseForm(ArrayList userRids, bool[] myHierarchyList, eProfileType myEnum, string itemTitle, bool inQuiry, out bool deleting)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            bool display = false;
            bool showDialog = false;
            myfrm.PersonalHierarchy = myHierarchyList;  // TT#3630 - JSmith - Delete My Hierarchy
            myfrm.ResolveInUseData(ref display, inQuiry, true, out showDialog);
            deleting = showDialog;
            if (showDialog == true)
            { myfrm.ShowDialog(); }
        }
        //END TT#110-MD-VStuart - In Use Tool

        // Begin TT#564 - JSmith - Copy/Paste from search not working
        //public void Search()
        public void Search(ExplorerAddressBlock aEAB)
        // End TT#564
        {
            //eHierarchySelectType hierarchySelectType;
            try
            {
                //Begin TT#1388-MD -jsobek -Product Filter
                //ArrayList nodeList = new ArrayList();
                //foreach (MIDHierarchyNode selectedNode in GetSelectedNodes())
                //{
                //    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(selectedNode.Profile.Key, false, true);
                //    switch (selectedNode.Profile.ProfileType)
                //    {
                //        case eProfileType.MerchandiseMainAlternatesFolder:
                //            hierarchySelectType = eHierarchySelectType.AlternateHierarchyFolder;
                //            break;
                //        case eProfileType.MerchandiseMainFavoritesFolder:
                //            hierarchySelectType = eHierarchySelectType.FavoritesFolder;
                //            break;
                //        case eProfileType.MerchandiseMainOrganizationalFolder:
                //            hierarchySelectType = eHierarchySelectType.OrganizationalHierarchyFolder;
                //            break;
                //        case eProfileType.MerchandiseMainUserFolder:
                //            hierarchySelectType = eHierarchySelectType.MyHierarchyFolder;
                //            break;
                //        default:
                //            if (hnp.HomeHierarchyLevel == 0)
                //            {
                //                if (hnp.HomeHierarchyType == eHierarchyType.organizational)
                //                {
                //                    hierarchySelectType = eHierarchySelectType.OrganizationalHierarchyRoot;
                //                }
                //                else
                //                {
                //                    hierarchySelectType = eHierarchySelectType.AlternateHierarchyRoot;
                //                }
                //            }
                //            else
                //            {
                //                hierarchySelectType = eHierarchySelectType.HierarchyNode;
                //            }
                //            break;
                //    }
                //    SelectedHierarchyNode shn = new SelectedHierarchyNode(hierarchySelectType, hnp);
                //    nodeList.Add(shn);
                //    break;
                //}
      
                //// Begin TT#564 - JSmith - Copy/Paste from search not working
                ////MerchandiseNodeSearch searchForm = new MerchandiseNodeSearch(SAB, nodeList);
                ////MerchandiseNodeSearch searchForm = new MerchandiseNodeSearch(SAB, nodeList);
                //MerchandiseNodeSearch searchForm = new MerchandiseNodeSearch(SAB, nodeList, aEAB);
                //// End TT#564
                //searchForm.MerchandiseNodeLocateEvent += new MerchandiseNodeSearch.MerchandiseNodeLocateEventHandler(searchForm_OnMerchandiseNodeLocateHandler);
                //searchForm.MerchandiseNodeRenameEvent += new MerchandiseNodeSearch.MerchandiseNodeRenameEventHandler(searchForm_MerchandiseNodeRenameEvent);
                //searchForm.MerchandiseNodeDeleteEvent += new MerchandiseNodeSearch.MerchandiseNodeDeleteEventHandler(searchForm_MerchandiseNodeDeleteEvent);
                //searchForm.MdiParent = MDIParentForm;
                //searchForm.Show();

                //frmFilterBuilder frmFilter = SharedRoutines.GetFilterFormForNewFilters(filterTypes.ProductFilter, SAB, aEAB, SAB.ClientServerSession.UserRID);
                //frmFilter.MdiParent = MDIParentForm;

                //frmFilter.Show();
                //frmFilter.BringToFront();


                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    //save the default node
                    //SharedControlRoutines.GetFirstSelectedMerchandiseNode = new SharedControlRoutines.GetFirstSelectedMerchandiseNodeDelegate(this.GetFirstSelectedNode);
                    filterUtility.GetFirstSelectedMerchandiseNode = new filterUtility.GetFirstSelectedMerchandiseNodeDelegate(this.GetFirstSelectedNode);


                    object[] args = new object[] { SAB, aEAB, filterTypes.ProductFilter };
                    System.Windows.Forms.Form frmSearchResults = SharedRoutines.GetForm(aEAB.Explorer.MdiChildren,typeof(SearchResultsForm), args, false);
                    //((SearchResultsForm)frmSearchResults).DefaultNodeProfile = hnpFirstSelected;
                    frmSearchResults.MdiParent = MDIParentForm;
                    frmSearchResults.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    frmSearchResults.Show();
                    frmSearchResults.BringToFront();
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }


                //End TT#1388-MD -jsobek -Product Filter
            }
            catch
            {
                throw;
            }
        }

        public HierarchyNodeProfile GetFirstSelectedNode()
        {
            HierarchyNodeProfile hnpFirstSelected = null;
            ArrayList nodeList = GetSelectedNodes();
            if (nodeList.Count > 0)
            {
                MIDHierarchyNode selectedNode = (MIDHierarchyNode)nodeList[0];
                hnpFirstSelected = SAB.HierarchyServerSession.GetNodeData(selectedNode.Profile.Key, false, true);
            }
            return hnpFirstSelected;
        }

        //Begin TT#1388-MD -jsobek -Product Filter
        //private void searchForm_MerchandiseNodeDeleteEvent(object source, MerchandiseNodeDeleteEventArgs e)
        //{
        //    try
        //    {
        //        lock (_lockControl.SyncRoot)
        //        {
        //            Cursor.Current = Cursors.WaitCursor;
        //            CleanUpExplorer(e.ParentKey, e.Key);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        Cursor.Current = Cursors.Default;
        //    }
        //}
   

        //private void searchForm_MerchandiseNodeRenameEvent(object source, MerchandiseNodeRenameEventArgs e)
        //{
        //    MIDHierarchyNode node;

        //    try
        //    {
        //        lock (_lockControl.SyncRoot)
        //        {
        //            Cursor.Current = Cursors.WaitCursor;
        //            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(e.Key, true, true);
        //            if (hnp.HomeHierarchyLevel == 0)
        //            {
        //                HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID);
        //                node = BuildNode(hnp.HomeHierarchyRID, hnp.HomeHierarchyType, null, hnp, false);
        //            }
        //            else
        //            {
        //                node = BuildNode(hnp.HomeHierarchyRID, hnp.HomeHierarchyType, null, hnp, false);
        //            }

        //            TreeNodeCollection nodes = Nodes;  // update all occurrances of the node
        //            foreach (MIDHierarchyNode tn in nodes)
        //            {
        //                if (tn.Profile.Key == node.Profile.Key &&
        //                    tn.Profile.ProfileType == node.Profile.ProfileType)
        //                {
        //                    //Begin Track #6201 - JScott - Store Count removed from attr sets
        //                    //tn.Text = hnp.Text;
        //                    tn.InternalText = hnp.Text;
        //                    //End Track #6201 - JScott - Store Count removed from attr sets
        //                }
        //                EditRecursiveText(tn, node, hnp.Text);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        Cursor.Current = Cursors.Default;
        //    }
        //}


        //private void searchForm_OnMerchandiseNodeLocateHandler(object source, MerchandiseNodeLocateEventArgs e)
        public void LocateAndDisplayNode(int hnRID, int hierarchyRID)
        {
            HierarchyProfile hp;
            try
            {
                lock (_lockControl.SyncRoot)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //SelectedNode = null;
                    SimulateMultiSelect = true;
                    ArrayList ancestorLists = SAB.HierarchyServerSession.GetAllNodeAncestorLists(hnRID);
                    foreach (NodeAncestorList nal in ancestorLists)
                    {
                        // locate main folder
                        NodeAncestorProfile root = (NodeAncestorProfile)nal[nal.Count - 1];
                        HierarchyNodeProfile rootProfile = SAB.HierarchyServerSession.GetNodeData(root.Key);
                        MIDHierarchyNode treeNode;
                        if (rootProfile.HierarchyRID != hierarchyRID)
                        {
                            continue;
                        }
                        if (rootProfile.HomeHierarchyType == eHierarchyType.alternate)
                        {
                            hp = SAB.HierarchyServerSession.GetHierarchyData(rootProfile.HomeHierarchyRID);
                            if (hp.Owner == Include.GlobalUserRID)
                            {
                                treeNode = AlternateNode;
                            }
                            else if (hp.Owner == SAB.ClientServerSession.UserRID)
                            {
                                treeNode = MyHierarchyNode;
                            }
                            else // in someone else's hierarchy
                            {
                                continue;
                            }
                        }
                        else
                        {
                            treeNode = OrganizationalNode;
                        }

                        for (int i = nal.Count - 1; i >= 0; --i)
                        {
                            if (treeNode != null)
                            {
                                NodeAncestorProfile nap = (NodeAncestorProfile)nal[i];
                                if (!treeNode.IsExpanded)
                                {
                                    treeNode.Expand();

                                    bool loop = true;
                                    while (loop)
                                    {
                                        if (treeNode.ChildrenLoaded &&
                                            treeNode.IsExpanded)
                                        {
                                            loop = false;
                                        }
                                        else
                                        {
                                            System.Threading.Thread.Sleep(10);
                                        }
                                    }
                                }
                                treeNode = LocateNode(nap.Key, treeNode);
                            }
                        }
                        SelectedNode = treeNode;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                SimulateMultiSelect = false;
                Cursor.Current = Cursors.Default;
            }
        }
        public void ClearSelectedNode()
        {
            SelectedNode = null;
        }
        //End TT#1388-MD -jsobek -Product Filters
       
        private MIDHierarchyNode LocateNode(int aNodeRID, MIDHierarchyNode aParentFolder)
        {
            try
            {
                foreach (MIDHierarchyNode node in aParentFolder.Nodes)
                {
                    if (node.Profile.Key == aNodeRID)
                    {
                        return node;
                    }
                }
                MessageBox.Show("Node not found");
                return null;
            }
            catch
            {
                throw;
            }
        }

        //Begin TT#1388-MD -jsobek -Product Filters
        //public void EditRecursiveText(MIDHierarchyNode treeNode, MIDHierarchyNode changedNode, string newName)
        //{
        //    // Edit each node recursively.
        //    foreach (MIDHierarchyNode tn in treeNode.Nodes)
        //    {
        //        if (tn.Profile.Key == changedNode.Profile.Key &&
        //            tn.Profile.ProfileType == changedNode.Profile.ProfileType)
        //        {
        //            //Begin Track #6201 - JScott - Store Count removed from attr sets
        //            //tn.Text = newName;
        //            tn.InternalText = newName;
        //            //End Track #6201 - JScott - Store Count removed from attr sets
        //        }
        //        EditRecursiveText(tn, changedNode, newName);
        //    }
        //}
        //End TT#1388-MD -jsobek -Product Filters

        public void EditRecursiveHomeHierarchy(MIDHierarchyNode treeNode, MIDHierarchyNode changedNode)
        {
            // Edit each node recursively.
            foreach (MIDHierarchyNode tn in treeNode.Nodes)
            {
                if (tn.Profile.Key == changedNode.Profile.Key &&
                    tn.Profile.ProfileType == changedNode.Profile.ProfileType)
                {
                    ((HierarchyNodeProfile)tn.Profile).HomeHierarchyRID = changedNode.HomeHierarchyRID;
                }
                EditRecursiveHomeHierarchy(tn, changedNode);
            }
        }

        public void EditRecursiveHomeLevel(MIDHierarchyNode changedNode)
        {
            foreach (MIDHierarchyNode tn in changedNode.Nodes)
            {
                if (tn.HomeHierarchyRID == changedNode.HomeHierarchyRID)
                {
                    ((HierarchyNodeProfile)tn.Profile).HomeHierarchyLevel = changedNode.HomeHierarchyLevel + 1;
                    ((HierarchyNodeProfile)tn.Profile).NodeLevel = tn.HomeHierarchyLevel;
                    EditRecursiveHomeLevel(tn);
                }
            }
        }

        private void DisplayMessages(EditMsgs em)
        {
            // Begin TT#5672 - JSmith - Purge Error on Sunday
			if (em.EditMessages.Count == 0)
            {
                return;
            }
			// End TT#5672 - JSmith - Purge Error on Sunday
            MIDRetail.Windows.DisplayMessages.Show(em, SAB, Include.MIDMerchandiseExplorer);
        }

        public void EditNodesRecursive(MIDHierarchyNode hierarchyNode, MIDHierarchyNode treeNode, LevelChange lc)
        {
            int folderIndex = 0;
            // Edit each node recursively.
            foreach (MIDHierarchyNode tn in treeNode.Nodes)
            {
                if (tn.HomeHierarchyRID == hierarchyNode.HierarchyRID && tn.HomeHierarchyLevel == lc.Level)
                {
                    if (lc.LevelColorChanged)
                    {
                        if (tn.IsExpanded)
                        {
                            folderIndex = MIDGraphics.ImageIndex(lc.LevelColor + MIDGraphics.OpenFolder);
                        }
                        else
                        {
                            folderIndex = MIDGraphics.ImageIndex(lc.LevelColor + MIDGraphics.ClosedFolder);
                        }
                        tn.ImageIndex = folderIndex;
                        tn.SelectedImageIndex = folderIndex;
                        ((HierarchyNodeProfile)tn.Profile).NodeColor = lc.LevelColor;

                        // Begin TT#21 - JSmith - Folder colors do not change when updated in Hierarchy Properties
                        folderIndex = MIDGraphics.ImageIndexWithDefault(lc.LevelColor, MIDGraphics.OpenFolder);
                        tn.SetExpandImageIndexes(folderIndex, folderIndex);
                        folderIndex = MIDGraphics.ImageIndexWithDefault(lc.LevelColor, MIDGraphics.ClosedFolder);
                        tn.SetCollapseImageIndexes(folderIndex, folderIndex);
                        // End TT#21
                    }
                    if (lc.LevelDisplayOptionChanged)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//tn.Text = Include.GetNodeDisplay(lc.LevelDisplayOption, tn.NodeID, tn.NodeName, tn.NodeDescription);
						tn.InternalText = Include.GetNodeDisplay(lc.LevelDisplayOption, tn.NodeID, tn.NodeName, tn.NodeDescription);
						//End Track #6201 - JScott - Store Count removed from attr sets
						((HierarchyNodeProfile)tn.Profile).DisplayOption = lc.LevelDisplayOption;
                    }
                }
                EditNodesRecursive(hierarchyNode, tn, lc);
            }
        }

        public void EditNodesRecursive(HierarchyNodeProfile hnp, MIDHierarchyNode treeNode)
        {
            // Edit each node recursively.
            foreach (MIDHierarchyNode tn in treeNode.Nodes)
            {
                if (tn.Profile != null &&
                    tn.Profile.Key == hnp.Key)
                {
                    tn.NodeID = hnp.NodeID;
                    ((HierarchyNodeProfile)tn.Profile).NodeName = hnp.NodeName;
                    ((HierarchyNodeProfile)tn.Profile).NodeDescription = hnp.NodeDescription;
					//Begin Track #6201 - JScott - Store Count removed from attr sets
					//tn.Text = Include.GetNodeDisplay(hnp.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
					tn.InternalText = Include.GetNodeDisplay(hnp.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
					//End Track #6201 - JScott - Store Count removed from attr sets
				}
                EditNodesRecursive(hnp, tn);
            }
        }

        void OnNodePropertiesChange(object source, NodePropertyChangeEventArgs e)
        {
            ChangeNodeProperties(source, e);
        }

        public void ChangeNodeProperties(object source, NodePropertyChangeEventArgs e)
        {
            HierarchyNodeProfile hnp = e.hnp;
            MIDHierarchyNode node = e.Node;
            MIDHierarchyNode mtn;

            switch (hnp.NodeChangeType)
            {
                case eChangeType.update:
                    {
                        TreeNodeCollection nodes = Nodes;
                        foreach (MIDHierarchyNode tn in nodes)
                        {
                            // loop through all hierarchies looking for updates
                            EditNodesRecursive(hnp, tn);
                        }
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//node.Text = Include.GetNodeDisplay(hnp.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
						node.InternalText = Include.GetNodeDisplay(hnp.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
						//End Track #6201 - JScott - Store Count removed from attr sets
						SortChildNodes((MIDTreeNode)node.Parent);
                        break;
                    }
                case eChangeType.add:
                    {
                        if (!node.ChildrenLoaded) //  add children
                        {
                            if (!node.IsExpanded)
                            {
                                node.Expand();
                            }
                        }
                        else
                        {
                            mtn = BuildNode(hnp.HomeHierarchyRID, hnp.HomeHierarchyType, node, hnp, false);
                            ((HierarchyNodeProfile)mtn.Profile).HomeHierarchyParentRID = node.Profile.Key;
                            ((HierarchyNodeProfile)mtn.Profile).Parents = node.Parents;
                            mtn.NodeID = hnp.NodeID;
                            ((HierarchyNodeProfile)mtn.Profile).NodeChangeType = eChangeType.none;
                            ((HierarchyNodeProfile)mtn.Profile).NodeLevel = node.NodeLevel + 1;
                            HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hnp.HierarchyRID);
                            if (hp.HierarchyType == eHierarchyType.organizational)
                            {
                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[mtn.NodeLevel];
                                ((HierarchyNodeProfile)mtn.Profile).DisplayOption = hlp.LevelDisplayOption;
                                ((HierarchyNodeProfile)mtn.Profile).NodeColor = hlp.LevelColor;
                            }
                            else
                            {
                                ((HierarchyNodeProfile)mtn.Profile).DisplayOption = SAB.ClientServerSession.GlobalOptions.ProductLevelDisplay;
                                ((HierarchyNodeProfile)mtn.Profile).NodeColor = Include.MIDDefaultColor;
                            }

                            mtn.ImageIndex = GetFolderImageIndex(mtn.HierarchyRID != mtn.HomeHierarchyRID, mtn.NodeColor, MIDGraphics.ClosedFolder);
                            mtn.SelectedImageIndex = mtn.ImageIndex;
							//Begin Track #6201 - JScott - Store Count removed from attr sets
							//mtn.Text = Include.GetNodeDisplay(mtn.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
							mtn.InternalText = Include.GetNodeDisplay(mtn.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
							//End Track #6201 - JScott - Store Count removed from attr sets
							mtn.HasChildren = false;
                            mtn.DisplayChildren = node.DisplayChildren;
                            node.Nodes.Add(mtn);
                            RebuildShortcuts(_favoritesNode, node);
                            RebuildShortcuts(_myHierarchyNode, node);
                            RebuildShortcuts(_alternateNode, node);
                            SortChildNodes(node);
                        }

                        node.HasChildren = true;
                        Sorted = false;

                        break;
                    }
            }
        }

        void OnHierPropertiesChange(object source, HierPropertyChangeEventArgs e)
        {
            ChangeHierProperties(source, e);
        }

        public void ChangeHierProperties(object source, HierPropertyChangeEventArgs e)
        {
            //_allowNodeSelect = false;
            int folderIndex = 0;
            MIDHierarchyNode node = e.Node;
            ArrayList levelChanges = e.LevelChanges;
            switch (node.NodeChangeType)
            {
                case eChangeType.update:
                    {
                        Sorted = true;
                        MIDHierarchyNode updateNode = node;
                        if (updateNode.IsExpanded)
                        {
                            folderIndex = MIDGraphics.ImageIndexWithDefault(node.NodeColor, MIDGraphics.OpenFolder);
                        }
                        else
                        {
                            folderIndex = MIDGraphics.ImageIndexWithDefault(node.NodeColor, MIDGraphics.ClosedFolder);
                        }
                        updateNode.ImageIndex = folderIndex;
                        updateNode.SelectedImageIndex = folderIndex;

                        // Begin TT#21 - JSmith - Folder colors do not change when updated in Hierarchy Properties
                        folderIndex = MIDGraphics.ImageIndexWithDefault(node.NodeColor, MIDGraphics.OpenFolder);
                        updateNode.SetExpandImageIndexes(folderIndex, folderIndex);
                        folderIndex = MIDGraphics.ImageIndexWithDefault(node.NodeColor, MIDGraphics.ClosedFolder);
                        updateNode.SetCollapseImageIndexes(folderIndex, folderIndex);
                        // End TT#21

                        updateNode.NodeChangeType = eChangeType.none;
                        Sorted = false;
                        TreeNodeCollection nodes = Nodes;
                        foreach (LevelChange lc in levelChanges)
                        {
                            foreach (MIDHierarchyNode tn in nodes)
                            {
                                // loop through all hierarchies looking for guest relationships
                                EditNodesRecursive(node, tn, lc);
                            }
                        }
                        OrderHierarchyFolders();
                        break;
                    }
                case eChangeType.add:
                    {
                        folderIndex = MIDGraphics.ImageIndexWithDefault(node.NodeColor, MIDGraphics.ClosedFolder);
                        node.ImageIndex = folderIndex;
                        node.SelectedImageIndex = folderIndex;
                        node.NodeChangeType = eChangeType.none;
                        node.DisplayChildren = true;
                        node.ChildrenLoaded = true;
                        if (e.IsMyHierarchy) // personal hierarchy
                        {
                            _myHierarchyNode.Nodes.Add(node);
                            if (!_myHierarchyNode.IsExpanded)
                            {
                                _myHierarchyNode.Expand();
                            }
                            else
                            {
                                SortChildNodes(_myHierarchyNode);
                            }
                        }
                        else
                        {
                            HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(node.HomeHierarchyRID);
                            if (hp.HierarchyType == eHierarchyType.organizational)
                            {
                                _organizationalNode.Nodes.Add(node);
                                if (!_organizationalNode.IsExpanded)
                                {
                                    _organizationalNode.Expand();
                                }
                                else
                                {
                                    SortChildNodes(_organizationalNode);
                                }
                            }
                            else
                            {
                                _alternateNode.Nodes.Add(node);
                                if (!_alternateNode.IsExpanded)
                                {
                                    _alternateNode.Expand();
                                }
                                else
                                {
                                    SortChildNodes(_alternateNode);
                                }
                            }
                        }
                        break;
                    }
            }
            node.TreeView.Select();
        }

        override protected void CustomDragDrop(object sender, DragEventArgs e)
        {
            Point currPoint;
            MIDHierarchyNode currNode;
            ProductCharacteristicClipboardList pcList;
            HierarchyNodeClipboardList hcList;

            try
            {
                currPoint = this.PointToClient(new Point(e.X, e.Y));
                currNode = (MIDHierarchyNode)this.GetNodeAt(currPoint.X, currPoint.Y);

                if (currNode != null)
                {
                    if (e.Data.GetDataPresent(typeof(ProductCharacteristicClipboardList)))
                    {
                        pcList = (ProductCharacteristicClipboardList)e.Data.GetData(typeof(ProductCharacteristicClipboardList));
                        if (pcList.ClipboardDataType == eProfileType.ProductCharacteristicValue)
                        {
                            PasteCharacteristics(currNode, pcList);
                        }
                    }
                    else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                    {
                        hcList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                        if (hcList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            PasteProducts(currNode, hcList);
                        }
                    }
                    else if (e.Data.GetDataPresent(typeof(List<int>))) //TT#1388-MD -jsobek -Product Filters
                    {
                        List<int> drSelectedList = (List<int>)e.Data.GetData(typeof(List<int>));
                        PasteProducts(currNode, drSelectedList);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#564 - JSmith - Copy/Paste from search not working
        override protected bool CustomPasteFromClipboard(eCutCopyOperation aCutCopyOperation)
        {
            MIDHierarchyNode currNode;
            ProductCharacteristicClipboardList pcList;
            HierarchyNodeClipboardList hcList;

            try
            {
                DataObject data = (DataObject)Clipboard.GetDataObject();
                currNode = (MIDHierarchyNode)this.SelectedNode;

                if (currNode != null)
                {
                    if (data.GetDataPresent(typeof(ProductCharacteristicClipboardList)))
                    {
                        pcList = (ProductCharacteristicClipboardList)data.GetData(typeof(ProductCharacteristicClipboardList));
                        if (pcList.ClipboardDataType == eProfileType.ProductCharacteristicValue)
                        {
                            PasteCharacteristics(currNode, pcList);
                        }
                    }
                    else if (data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                    {
                        hcList = (HierarchyNodeClipboardList)data.GetData(typeof(HierarchyNodeClipboardList));
                        if (hcList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            PasteProducts(currNode, hcList);
                        }
                    }
                    else if (data.GetDataPresent(typeof(List<int>))) //TT#1388-MD -jsobek -Product Filters
                    {
                        List<int> drSelectedList = (List<int>)data.GetData(typeof(List<int>));
                        PasteProducts(currNode, drSelectedList);
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        // End TT#564

        /// <summary>
        /// Paste products being dragged from external source
        /// </summary>
        /// <param name="aNode">The node where the items are to be pasted</param>
        /// <param name="aClipboardList">The list of items to paste</param>
        public void PasteProducts(MIDHierarchyNode aNode, HierarchyNodeClipboardList aClipboardList)
        {
			DragDropEffects dropAction = DragDropEffects.None;
            MIDHierarchyNode node;

            try
            {
                ArrayList nodes = new ArrayList();

                if (aClipboardList != null)
                {
                    if (aClipboardList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        foreach (HierarchyNodeClipboardProfile item in aClipboardList.ClipboardItems)
                        {
                            node = FindHierarchyNode(this.Nodes, item.HomeHierarchyRID, item.HomeHierarchyType, item.ProfileType, item.Key);
                            if (node.TreeView == null) // if node not built in explorer yet, use action on destination node
                            {
                                node.DropAction = aNode.DropAction;
                            }
                            else if (aNode.isFavoritesSubFolder)
                            {
                                node.DropAction = DragDropEffects.Link;
                            }
                            else
                            {
                                node.DropAction = item.Action;
                            }
                            nodes.Add(node);
                        }
                    }
                    PasteNodes(dropAction, aNode, nodes);
                }
            }
            catch 
            {
                throw;
            }
        }

        //Begin TT#1388-MD -jsobek -Product Filters
        //public void PasteProducts(MIDHierarchyNode aNode, TreeNodeClipboardList aClipboardList)
        //{
        //    DragDropEffects dropAction = DragDropEffects.None;

        //    try
        //    {
        //        ArrayList nodes = new ArrayList();

        //        if (aClipboardList != null)
        //        {
        //            if (aClipboardList.ClipboardDataType == eProfileType.HierarchyNode ||
        //                aClipboardList.ClipboardDataType == eProfileType.Folder)
        //            {
        //                foreach (TreeNodeClipboardProfile item in aClipboardList.ClipboardItems)
        //                {
        //                    dropAction = item.Action;
        //                    nodes.Add(item.Node);
        //                }
        //            }
        //            PasteNodes(dropAction, aNode, nodes);
        //        }
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //}

  
        public void PasteProducts(MIDHierarchyNode aNode, List<int> drSource)
        {
            DragDropEffects dropAction = DragDropEffects.None;

            try
            {
                ArrayList nodes = new ArrayList();

                if (drSource != null)
                {

                    foreach (int hnRID in drSource)
                    {
                        //dropAction = item.Action;
                        //MIDHierarchyNode hn = new MIDHierarchyNode();
                        //MIDHierarchyNode hn = (MIDHierarchyNode)BuildNode(null, hnRID);
                        //hn.DropAction = DragDropEffects.Copy;
                        //nodes.Add(hn);

                        //            HierarchyNodeClipboardProfile cbp;
                        //            FunctionSecurityProfile securityProfile;
                        //            try
                        //            {
                        //                MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)aItem.Tag;
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hnRID, false);
                        //FunctionSecurityProfile securityProfile = new FunctionSecurityProfile(hnRID);
                        //                securityProfile.SetFullControl();
                        //                cbp = new HierarchyNodeClipboardProfile(itemTag.Key, hnp.Text, securityProfile);
                        //                cbp.Action = aAction;

                        //                // Begin TT#564 - JSmith - Copy/Paste from search not working
                        //                cbp.HierarchyRID = hnp.HomeHierarchyRID;
                        //                cbp.HierarchyType = hnp.HomeHierarchyType;
                        //                cbp.HomeHierarchyRID = hnp.HomeHierarchyRID;
                        //                cbp.HomeHierarchyType = hnp.HomeHierarchyType;
                        //                // End TT#564

                        //                // use temp tree node to calculate dimensions so calculations for image dragging are consistent
                        //                HierarchyTreeView tempTreeView = new HierarchyTreeView();
                        MIDHierarchyNode tempNode = new MIDHierarchyNode(SAB, eTreeNodeType.ObjectNode, hnp, hnp.Text, Include.NoRID, Include.NoRID, null, Include.NoRID, Include.NoRID, Include.NoRID);
                        //                tempTreeView.Nodes.Add(tempNode);
                        //                cbp.DragImage = lvNodes.SmallImageList.Images[aItem.ImageIndex];
                        //                cbp.DragImageHeight = tempNode.Bounds.Height;
                        //                cbp.DragImageWidth = tempNode.Bounds.Width;
                        tempNode.HasChildren = hnp.HasChildren;

                        if (aNode.HomeHierarchyRID == tempNode.HomeHierarchyRID)
                        {
                            if (aNode.isFavoritesSubFolder)
                            {
                                tempNode.DropAction = DragDropEffects.Link;
                            }
                            else
                            {
                                tempNode.DropAction = DragDropEffects.Copy;
                            }
                        }
                        else
                        {
                            tempNode.DropAction = DragDropEffects.Link;
                        }
                        nodes.Add(tempNode);
                    }

                    PasteNodes(dropAction, aNode, nodes);
                }
            }
            catch
            {
                throw;
            }
        }
        //End TT#1388-MD -jsobek -Product Filters

		public void PasteNodes(DragDropEffects aDropAction, MIDHierarchyNode aToNode, ArrayList aNodes)
        {
            try
            {
                if (aNodes.Count == 0)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NothingToPaste));
                }
                else
                {
                    bool movingNode = false;
                    bool copyingNode = false;
                    bool makingShortCut = false;
                    int actions = 0;
                    // determine message
                    string message = null;
                    string title = null;
                    foreach (MIDHierarchyNode treeNode in aNodes)
                    {
                        // it's a folder, drop the file
                        switch (treeNode.DropAction)
                        {
                            case DragDropEffects.Move:
                                if (!movingNode)
                                {
                                    ++actions;
                                }
                                movingNode = true;
                                break;
                            case DragDropEffects.Copy:
                                if (!copyingNode)
                                {
                                    ++actions;
                                }
                                copyingNode = true;
                                break;
                            case DragDropEffects.Link:
                                if (!makingShortCut)
                                {
                                    ++actions;
                                }
                                makingShortCut = true;
                                break;
                        }
                    }

                    if (actions > 1)
                    {
                    }
                    else if (movingNode)
                    {
                        title = "Move";
                        if (aNodes.Count == 1)
                        {
                            MIDHierarchyNode moveNode = (MIDHierarchyNode)aNodes[0];
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmMoveNode, false);
                            message = message.Replace("{0}", moveNode.Text);
                            message = message.Replace("{1}", aToNode.Text);
                        }
                        else
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmMoveNodes, false);
                            message = message.Replace("{0}", aToNode.Text);
                        }
                    }
                    else if (copyingNode)
                    {
                        title = "Copy";
                        if (aNodes.Count == 1)
                        {
                            MIDHierarchyNode copyNode = (MIDHierarchyNode)aNodes[0];
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNode, false);
                            message = message.Replace("{0}", copyNode.Text);
                            message = message.Replace("{1}", aToNode.Text);
                        }
                        else
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNodes, false);
                            message = message.Replace("{0}", aToNode.Text);
                        }
                    }
                    else if (makingShortCut)
                    {
                        title = "Reference";
                        if (aNodes.Count == 1)
                        {
                            MIDHierarchyNode copyNode = (MIDHierarchyNode)aNodes[0];
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortCut, false);
                            message = message.Replace("{0}", copyNode.Text);
                            message = message.Replace("{1}", aToNode.Text);
                        }
                        else
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortcuts, false);
                            message = message.Replace("{0}", aToNode.Text);
                        }
                    }

                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, this.GetType().Name);
                    if (MessageBox.Show(message, title,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.No)
                    {
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    foreach (MIDHierarchyNode treeNode in aNodes)
                    {
                        // it's a folder, drop the file
                        switch (treeNode.DropAction)
                        {
                            case DragDropEffects.Move:
                                MoveNode(treeNode, aToNode);
                                break;
                            case DragDropEffects.Copy:
                                CopyNode(treeNode, aToNode, true);
                                break;
                            case DragDropEffects.Link:
                                MakeShortCut(treeNode, aToNode);
                                break;
                        }
                    }
                    if (!aToNode.IsExpanded)
                    {
                        aToNode.Expand();
                    }
                }
            }
            catch
            {
                throw;
            }
            //Begin Track #6262 - JSmith - Hour Glass continues to display after search & Alt Hier created
            finally
            {
                this.Cursor = Cursors.Default;
            }
            //End Track #6262
        }

        public void PasteCharacteristics(MIDHierarchyNode aNode, ProductCharacteristicClipboardList aClipboardList)
        {
            try
            {

                ArrayList nodes = new ArrayList();

                if (aClipboardList != null)
                {
                    if (aClipboardList.ClipboardDataType == eProfileType.ProductCharacteristicValue)
                    {
                        foreach (ProductCharacteristicClipboardProfile item in aClipboardList.ClipboardItems)
                        {
                            PasteCharacteristic(aNode, item.ProductCharGroupKey, item.Key, item.Text);
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        private void PasteCharacteristic(MIDHierarchyNode aNode, int aProductCharRID, int aProductCharValueRID, string aValue)
        {
            try
            {
                // check to see if the node is assigned a different group value
                string currentValue;
                int currentValueRID;
                if (_hm.IsProductCharAlreadyAssigned(aNode.Profile.Key, aProductCharRID, aProductCharValueRID, out currentValueRID, out currentValue))
                {
                    string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProductCharAlreadyAssigned, false);
                    text = text.Replace("{0}", aNode.Text);
                    text = text.Replace("{1}", currentValue);
                    text = text.Replace("{2}", aValue);
                    if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.No)
                    {
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    _hm.UpdateProductCharValue(aNode.Profile.Key, aProductCharRID, currentValueRID, eChangeType.delete);
                }
                this.Cursor = Cursors.WaitCursor;
                _hm.UpdateProductCharValue(aNode.Profile.Key, aProductCharRID, aProductCharValueRID, eChangeType.add);
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
	}

    /// <summary>
    /// Used as a node in the treeview for the Merchandise Explorer
    /// </summary>
    public class MIDHierarchyNode : MIDTreeNode
    {
        //  If new fields are added, the clone method below must also be changed
        public HierarchyNodeSecurityProfile _hierarchyNodeSecurityProfile;
        private bool _isHierarchyRoot;
        private int _hierarchyRID;
        private eHierarchyType _hierarchyType;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public MIDHierarchyNode()
            : base()
        {
            CommonLoad();
        }

        public MIDHierarchyNode(
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

        public MIDHierarchyNode(
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
            HasChildren = false;
            ChildrenLoaded = false;
            CanBeDeleted = false;
            _isHierarchyRoot = false;
            _hierarchyNodeSecurityProfile = null;
        }

        private void LoadFromDataRow(DataRow aRow)
        {	
            eProfileType folderType;
            try
            {
                OwnerUserRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
                folderType = (eProfileType)Convert.ToInt32(aRow["FOLDER_TYPE"], CultureInfo.CurrentUICulture);
				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//Text = (aRow["FOLDER_ID"] != DBNull.Value) ? Convert.ToString(aRow["FOLDER_ID"], CultureInfo.CurrentUICulture) : null;
				InternalText = (aRow["FOLDER_ID"] != DBNull.Value) ? Convert.ToString(aRow["FOLDER_ID"], CultureInfo.CurrentUICulture) : null;
				//End Track #6201 - JScott - Store Count removed from attr sets
				OwnerUserRID = Convert.ToInt32(aRow["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//private void LoadFromFolderProfile(FolderProfile aFolderProfile)
		//{
		//    try
		//    {
		//        OwnerUserRID = aFolderProfile.OwnerUserRID;
		//        Text = aFolderProfile.Name;
		//        Profile = aFolderProfile;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		public bool DebugActivated 
        {
            get 
            {
                if (TreeView == null)
                {
                    return false;
                }
                else 
                {
                    return ((MIDTreeView)TreeView).DebugActivated;
                }
            }
        }

        public bool isHierarchyRoot
        {
            get { return _isHierarchyRoot; }
            set { _isHierarchyRoot = value; }
        }

        /// <summary>
        /// Returns the FunctionSecurityProfile of this node.
        /// </summary>

        public override FunctionSecurityProfile FunctionSecurityProfile
        {
            get
            {
                int hierarchyOwner;

                try
                {
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//if (_functionSecurityProfile == null)
					//{
					//    if (((MIDHierarchyNode)this).isHierarchyNode)
					//    {
					//        // Begin Track #6258 - JSmith - Attempt to remove sku under node>is not an option
					//        //hierarchyOwner = SAB.HierarchyServerSession.GetHierarchyOwner(HomeHierarchyRID);
					//        hierarchyOwner = SAB.HierarchyServerSession.GetHierarchyOwner(HierarchyRID);
					//        // End Track #6258

					//        if (HierarchyType == eHierarchyType.organizational)
					//        {
					//            _functionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodes, (int)eSecurityTypes.All);
					//        }
					//        else if (hierarchyOwner == Include.GlobalUserRID)
					//        {
					//            _functionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesAltNodes, (int)eSecurityTypes.All);
					//        }
					//        else
					//        {
					//            _functionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
					//        }
					//    }
					//    else if (((MIDHierarchyNode)this).isFavoritesSubFolder)
					//    {
					//        if (_functionSecurityProfile == null)
					//        {
					//            _functionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
					//            _functionSecurityProfile.SetFullControl();
					//        }
					//    }
					//    else if (((MIDHierarchyNode)this).isMainFavoritesFolder ||
					//        ((MIDHierarchyNode)this).isMainAlternatesFolder ||
					//        ((MIDHierarchyNode)this).isMainOrganizationalFolder)
					//    {
					//        if (_functionSecurityProfile == null)
					//        {
					//            _functionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
					//            _functionSecurityProfile.SetFullControl();
					//            _functionSecurityProfile.SetDenyDelete();
					//        }
					//    }
					//    else
					//    {
					//        if (_functionSecurityProfile == null)
					//        {
					//            if (Profile != null)
					//            {
					//                _functionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
					//            }
					//            else
					//            {
					//                _functionSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
					//            }
					//        }
					//    }
					//}

					//return _functionSecurityProfile;
					if (_nodeSecurityGroup == null)
					{
						_nodeSecurityGroup = new MIDTreeNodeSecurityGroup();
					}

					if (_nodeSecurityGroup.FunctionSecurityProfile == null)
					{
						if (((MIDHierarchyNode)this).isHierarchyNode)
						{
							// Begin Track #6258 - JSmith - Attempt to remove sku under node>is not an option
							//hierarchyOwner = SAB.HierarchyServerSession.GetHierarchyOwner(HomeHierarchyRID);
							hierarchyOwner = SAB.HierarchyServerSession.GetHierarchyOwner(HierarchyRID);
							// End Track #6258

							if (HierarchyType == eHierarchyType.organizational)
							{
								_nodeSecurityGroup.FunctionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodes, (int)eSecurityTypes.All);
							}
							else if (hierarchyOwner == Include.GlobalUserRID)
							{
								_nodeSecurityGroup.FunctionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesAltNodes, (int)eSecurityTypes.All);
                                // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                                if (this.TreeNodeType == eTreeNodeType.ChildObjectShortcutNode)
                                {
                                    _nodeSecurityGroup.FunctionSecurityProfile.SetDenyDelete();
                                }
                                // End TT#373
							}
							else
							{
								_nodeSecurityGroup.FunctionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                                // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                                if (this.TreeNodeType == eTreeNodeType.ChildObjectShortcutNode)
                                {
                                    _nodeSecurityGroup.FunctionSecurityProfile.SetDenyDelete();
                                }
                                // End TT#373
							}
						}
						else if (((MIDHierarchyNode)this).isFavoritesSubFolder)
						{
							if (_nodeSecurityGroup.FunctionSecurityProfile == null)
							{
								_nodeSecurityGroup.FunctionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
								_nodeSecurityGroup.FunctionSecurityProfile.SetFullControl();
							}
						}
						else if (((MIDHierarchyNode)this).isMainFavoritesFolder ||
							((MIDHierarchyNode)this).isMainAlternatesFolder ||
							((MIDHierarchyNode)this).isMainOrganizationalFolder)
						{
							if (_nodeSecurityGroup.FunctionSecurityProfile == null)
							{
								_nodeSecurityGroup.FunctionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
								_nodeSecurityGroup.FunctionSecurityProfile.SetFullControl();
								_nodeSecurityGroup.FunctionSecurityProfile.SetDenyDelete();
							}
						}
						else
						{
							if (_nodeSecurityGroup.FunctionSecurityProfile == null)
							{
								if (Profile != null)
								{
									_nodeSecurityGroup.FunctionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
								}
								else
								{
									_nodeSecurityGroup.FunctionSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
								}
							}
						}
					}

					return _nodeSecurityGroup.FunctionSecurityProfile;
					//End Track #6321 - JScott - User has ability to to create folders when security is view
				}
				catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the SecurityProfile of this node.
        /// </summary>

        public HierarchyNodeSecurityProfile HierarchyNodeSecurityProfile
        {
            get
            {
                try
                {
                    if (_hierarchyNodeSecurityProfile == null)
                    {
                        if (((MIDHierarchyNode)this).isHierarchyNode &&
                            Profile.Key != Include.NoRID)
                        {
                            _hierarchyNodeSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(Profile.Key, (int)eSecurityTypes.All);
                        }
                        else if (Profile.ProfileType == eProfileType.MerchandiseSubFolder)
                        {
                            if (_hierarchyNodeSecurityProfile == null)
                            {
                                _hierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(Profile.Key);
                                _hierarchyNodeSecurityProfile.SetFullControl();
                            }
                        }
                        else if (((MIDHierarchyNode)this).isMainFavoritesFolder ||
                            ((MIDHierarchyNode)this).isMainAlternatesFolder ||
                            ((MIDHierarchyNode)this).isMainOrganizationalFolder ||
                            ((MIDHierarchyNode)this).isMainUserFolder)
                        {
                            if (_hierarchyNodeSecurityProfile == null)
                            {
                                _hierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(Profile.Key);
                                _hierarchyNodeSecurityProfile.SetFullControl();
                                _hierarchyNodeSecurityProfile.SetDenyDelete();
                            }
                        }
                        else
                        {
                            if (_hierarchyNodeSecurityProfile == null)
                            {
                                _hierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(Profile.Key);
                            }
                        }
                    }

                    return _hierarchyNodeSecurityProfile;
                }
                catch
                {
                    throw;
                }
            }
			set
			{
				_hierarchyNodeSecurityProfile = value;
			}
        }

        /// <summary>
        /// Gets a boolean indicating if the SecurityProfile for this node has been initialized.
        /// </summary>

        public bool SecurityProfileIsInitialized
        {
            get
            {
                return _hierarchyNodeSecurityProfile != null;
            }
        }

        /// <summary>
        /// Gets or sets the display option of the node.
        /// </summary>
        public eHierarchyDisplayOptions DisplayOption
        {
            get
            {
                if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).DisplayOption;
                }
                else
                {
                    return eHierarchyDisplayOptions.NameOnly;
                }
            }
        }
        /// <summary>
        /// Gets or sets the name of the child.
        /// </summary>
        public string NodeName
        {
            get
            {
                if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).NodeName;
                }
                else
                {
                    return Text;
                }
            }
        }
        /// <summary>
        /// Gets or sets the description of the child.
        /// </summary>
        public string NodeDescription
        {
            get
            {
                if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).NodeDescription;
                }
                else
                {
                    return Text;
                }
            }
        }
        /// <summary>
        /// Gets or sets the folder color for the node.
        /// </summary>
        public string NodeColor
        {
            get
            {
                if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).NodeColor;
                }
                else
                {
                    return Include.MIDDefaultColor;
                }
            }
        }
        /// <summary>
        /// Gets or sets the relative level for the node in the hierarchy in the current path.
        /// </summary>
        public int NodeLevel
        {
            get
            {
                if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).NodeLevel;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Gets or sets the relative level for the node in the hierarchy in the home path.
        /// </summary>
        public int HomeHierarchyLevel
        {
            get
            {
                if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).HomeHierarchyLevel;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Gets or sets the record id of the hierarchy where the node is located.
        /// </summary>
        public int HierarchyRID
        {
            get { return _hierarchyRID; }
            set { _hierarchyRID = value; }
        }
        /// <summary>
        /// Gets or sets the type of hierarchy where the node is located.
        /// </summary>
        public eHierarchyType HierarchyType
        {
            get { return _hierarchyType; }
            set { _hierarchyType = value; }
        }
        /// <summary>
        /// Gets the record id of the home hierarchy for the node.
        /// </summary>
        public int HomeHierarchyRID
        {
            get
            {
                if (Profile != null && 
                    Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).HomeHierarchyRID;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Gets the record id of the parent node in the hierarchy.
        /// </summary>
        public int HomeHierarchyParentRID
        {
            get
            {
                if (Profile != null && 
                    Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).HomeHierarchyParentRID;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Gets the type of hierarchy where the node is located.
        /// </summary>
        public eHierarchyType HomeHierarchyType
        {
            get
            {
                if (Profile != null &&
                    Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).HomeHierarchyType;
                }
                else
                {
                    return eHierarchyType.None;
                }
            }
        }

        /// <summary>
        /// Gets the owner of the hierarchy.
        /// </summary>
        public int HomeHierarchyOwner
        {
            get
            {
                if (Profile != null &&
                    Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).HomeHierarchyOwner;
                }
                else
                {
                    return Include.GlobalUserRID;
                }
            }
        }

        /// <summary>
        /// Gets or sets the record id(s) of the parent node(s) in the hierarchy.
        /// </summary>
        public ArrayList Parents
        {
            get
            {
                if (Profile != null && 
                    Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    return ((HierarchyNodeProfile)Profile).Parents;
                }
                else
                {
                    return new ArrayList();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        public eProductType ProductType
        {
            get
            {
                if (Profile == null)
                {
                    return eProductType.Undefined;
                }
                else
                {
                    if (Profile.ProfileType == eProfileType.HierarchyNode)
                    {
                        return ((HierarchyNodeProfile)Profile).ProductType;
                    }
                    else
                    {
                        return eProductType.Undefined;
                    }
                }
            }
            //get { return _productType ; }
            //set { _productType = value; }
        }

        public bool isOrganizationalHierarchyRoot
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    if (Profile.ProfileType == eProfileType.HierarchyNode &&
                        ((HierarchyNodeProfile)Profile).HomeHierarchyType == eHierarchyType.organizational &&
                        ((HierarchyNodeProfile)Profile).HomeHierarchyLevel == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool isAlternateHierarchyRoot
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    if (Profile.ProfileType == eProfileType.HierarchyNode &&
                        ((HierarchyNodeProfile)Profile).HomeHierarchyType == eHierarchyType.alternate &&
                        ((HierarchyNodeProfile)Profile).HomeHierarchyLevel == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool isMyHierarchyRoot
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    if (Profile.ProfileType == eProfileType.HierarchyNode &&
                        ((HierarchyNodeProfile)Profile).HomeHierarchyType == eHierarchyType.alternate &&
                        ((HierarchyNodeProfile)Profile).HomeHierarchyOwner == SAB.ClientServerSession.UserRID)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool isMainUserFolder
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return Profile.ProfileType == eProfileType.MerchandiseMainUserFolder;
                }
            }
        }

        public bool isMainAlternatesFolder
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return Profile.ProfileType == eProfileType.MerchandiseMainAlternatesFolder;
                }
            }
        }

        public bool isMainOrganizationalFolder
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return Profile.ProfileType == eProfileType.MerchandiseMainOrganizationalFolder;
                }
            }
        }

        public bool isMainFavoritesFolder
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return Profile.ProfileType == eProfileType.MerchandiseMainFavoritesFolder;
                }
            }
        }

        public bool isFavoritesSubFolder
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return Profile.ProfileType == eProfileType.MerchandiseSubFolder;
                }
            }
        }

        public bool isFavoritesFolder
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return (Profile.ProfileType == eProfileType.MerchandiseMainFavoritesFolder ||
                        Profile.ProfileType == eProfileType.MerchandiseSubFolder);
                }
            }
        }

        public bool isHierarchyNode
        {
            get
            {
                if (Profile == null)
                {
                    return false;
                }
                else
                {
                    return (Profile.ProfileType == eProfileType.HierarchyNode);
                }
            }
        }

        // Begin Track #6202 - JSmith - select not allowed
        /// <summary>
        /// Returns a boolean indicating if this MIDTreeNode can be accessed by the user
        /// </summary>

        override public bool isAccessAllowed
        {
            get
            {
                if ((HierarchyNodeSecurityProfile != null && HierarchyNodeSecurityProfile.AccessDenied) &&
                    (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied))
                {
                    return false;
                }
                return true;
            }
        }
        // End Track #6202

        public void SetSecurity()
        {
            // Begin Track #6258 - JSmith - Attempt to remove sku under node>is not an option
            //int hierarchyOwner = SAB.HierarchyServerSession.GetHierarchyOwner(HomeHierarchyRID);
            int hierarchyOwner = SAB.HierarchyServerSession.GetHierarchyOwner(HierarchyRID);
            // End Track #6258

            _hierarchyNodeSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(Profile.Key, (int)eSecurityTypes.All);

            if (HierarchyType == eHierarchyType.organizational)
            {
                FunctionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodes, (int)eSecurityTypes.All);
            }
            else if (hierarchyOwner == Include.GlobalUserRID)
            {
                FunctionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesAltNodes, (int)eSecurityTypes.All);
            }
            else
            {
                FunctionSecurityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
            }
        }

        public MIDHierarchyNode GetHierarchyRootNode()
        {
            try
            {
                MIDHierarchyNode node;

                node = this;

                while (node.Parent != null)
                {
                    node = (MIDHierarchyNode)node.Parent;
                    if (node.isHierarchyRoot)
                    {
                        return node;
                    }
                }

                return null;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Used to clone or copy a node.
        /// </summary>
        /// <remarks>
        /// This method must be change when fields are added or removed from the class.
        /// </remarks>
        override public object Clone()
        {
            MIDHierarchyNode mtn = (MIDHierarchyNode)base.Clone();
            mtn.NodeChangeType = this.NodeChangeType;
            mtn.HierarchyNodeSecurityProfile = this._hierarchyNodeSecurityProfile;
            mtn.isHierarchyRoot = this._isHierarchyRoot;
            return mtn;
        }

        /// <summary>
        /// Used to clone or copy a node.
        /// </summary>
        /// <param name="aProfile">
        /// The profile to use for the new node.
        /// </param>
        /// <remarks>
        /// This method must be change when fields are added or removed from the class.
        /// </remarks>
        override public object CloneNode(Profile aProfile)
        {
            MIDHierarchyNode mtn = (MIDHierarchyNode)base.CloneNode(aProfile);
            if (aProfile is HierarchyNodeProfile)
            {
				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//mtn.Text = ((HierarchyNodeProfile)aProfile).Text;
				mtn.InternalText = ((HierarchyNodeProfile)aProfile).Text;
				//End Track #6201 - JScott - Store Count removed from attr sets
			}
            else if (aProfile is HierarchyProfile)
            {
				//Begin Track #6201 - JScott - Store Count removed from attr sets
				//mtn.Text = ((HierarchyProfile)aProfile).HierarchyID;
				mtn.InternalText = ((HierarchyProfile)aProfile).HierarchyID;
				//End Track #6201 - JScott - Store Count removed from attr sets
			}
            mtn.NodeChangeType = this.NodeChangeType;
            mtn.HierarchyNodeSecurityProfile = this._hierarchyNodeSecurityProfile;
            mtn.isHierarchyRoot = this._isHierarchyRoot;
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

                // Begin Track #6202 - JSmith - select not allowed
                //if (HierarchyNodeSecurityProfile != null && HierarchyNodeSecurityProfile.AccessDenied)
                //{
                //    allowSelect = false;
                //}
                if ((HierarchyNodeSecurityProfile != null && HierarchyNodeSecurityProfile.AccessDenied) &&
                    (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied))
                {
                    allowSelect = false;
                }
                // End Track #6202

                return allowSelect;
            }
            catch
            {
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
                if ((FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied) &&
                    (HierarchyNodeSecurityProfile != null && HierarchyNodeSecurityProfile.AccessDenied))
                {
                    return false;
                }
                 
                allowDrag = true;

                switch (Profile.ProfileType)
                {
                    case eProfileType.MerchandiseMainUserFolder:
                        allowDrag = false;
                        break;
                    case eProfileType.MerchandiseMainOrganizationalFolder:
                        allowDrag = false;
                        break;
                    case eProfileType.MerchandiseMainAlternatesFolder:
                        allowDrag = false;
                        break;
                    case eProfileType.MerchandiseMainFavoritesFolder:
                        allowDrag = false;
                        break;
                }

                return allowDrag;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Abstract method that indicates if this MIDTreeNode can be dropped in the given MIDTreeNode
        /// </summary>
        /// <param name="aDropAction">
        /// The eDropAction that is being processed.
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
                if (this == aDestinationNode)
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

                // Begin Track #6211 - JSmith - Do not allow hierarchy dropped in hierarchy
                // Begin TT#24 - JSmith - Cannot copy/paste a node from My Hierarchies to Favorites to make a reference
                //if (this.isHierarchyRoot)
                if (this.isHierarchyRoot &&
                    !aDestinationNode.isFolder)
                // End TT#24
                {
                    return false;
                }
                // End Track #6211

                if (((MIDHierarchyNode)aDestinationNode).isMainOrganizationalFolder ||
                    ((MIDHierarchyNode)aDestinationNode).isMainAlternatesFolder)
                {
                    return false;
                }

                // Begin TT#18 - JSmith - Drag/drop incorrectly allowed from Favorites to User Hierarchies
                // do not allow drag from favorites or alternates to organizational
                if (this.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
                    aDestinationNode.GetTopSourceNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode)
                {
                    return false;
                }
                else if (this.HierarchyType == eHierarchyType.alternate &&
                    ((MIDHierarchyNode)aDestinationNode).HierarchyType == eHierarchyType.organizational)
                {
                    return false;
                }
                // End TT#18

                //Begin Track #6456 - JSmith - Users have ability to move nodes in alternate hierarchy despite security settings
                //if (((MIDHierarchyNode)aDestinationNode).FunctionSecurityProfile == null ||
                //    (_hierarchyType == eHierarchyType.organizational &&
                //    ((MIDHierarchyNode)aDestinationNode).HierarchyType == eHierarchyType.organizational &&
                //    !aDestinationNode.FunctionSecurityProfile.AllowMove))
                //{
                //    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized);
                //    return false;
                //}
                //Begin TT#1744 - JSmith - Security for the Alternate Hierarchy
                //if (((MIDHierarchyNode)aDestinationNode).FunctionSecurityProfile == null ||
                //    !aDestinationNode.FunctionSecurityProfile.AllowMove)
                //{
                //    //Begin TT#474 - JScott - Create My Hier w dash - errors
                //    //Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized);
                //    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized, false);
                //    //End TT#474 - JScott - Create My Hier w dash - errors
                //    return false;
                //}
                ////End Track #6456
                if (((MIDHierarchyNode)aDestinationNode).FunctionSecurityProfile == null)
                {
                    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized, false);
                    return false;
                }
                else if (!aDestinationNode.FunctionSecurityProfile.AllowMove &&
                    aDropAction == DragDropEffects.Move)
                {
                    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized, false);
                    return false;
                }
                else if (!aDestinationNode.FunctionSecurityProfile.AllowUpdate &&
                    aDropAction == DragDropEffects.Copy)
                {
                    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized, false);
                    return false;
                }
                //End TT#1744

                if (((MIDHierarchyNode)aDestinationNode).isFavoritesFolder)
                {
                    return isFolderDropAllowed(aDropAction, (MIDHierarchyNode)aDestinationNode);
                }
                else
                {
                    return isNodeDropAllowed(aDropAction, (MIDHierarchyNode)aDestinationNode);
                }
            }
            catch
            {
                throw;
            }
        }

		private bool isNodeDropAllowed(DragDropEffects aDropAction, MIDHierarchyNode aDestinationNode)
        {
            eHierarchyType selectedNodeHierarchyType;
            eHierarchyType dropNodeHierarchyType;
            MIDHierarchyNode selectedNodeParent;
            MIDHierarchyNode selectedNodeFolder;
            MIDHierarchyNode dropNodeFolder;
            HierarchyProfile selectedHierarchy;
            HierarchyProfile dropHierarchy;
            HierarchyLevelProfile selectedHlp;
            HierarchyLevelProfile dropHlp;
            HierarchyNodeProfile hnp;
            bool circularRelationship;
            MIDHierarchyNode dropNodePath;
            bool nodeFound;
            bool validLevels;
            int fromLevel;
            int toLevel;

            try
            {
                if (((MIDHierarchyNode)this).isFavoritesFolder)
                {
                    return false;
                }

                // Begin TT#954 - JSmith - Unhandled exception in Merchandise Explorer
                if (!(Profile is HierarchyNodeProfile))
                {
                    return false;
                }
                // End TT#954

                selectedNodeHierarchyType = eHierarchyType.organizational;
                dropNodeHierarchyType = eHierarchyType.organizational;
                selectedNodeParent = null;

                if (Parent != null && Parent is MIDHierarchyNode)
                {
                    selectedNodeParent = (MIDHierarchyNode)Parent;
                }

                selectedNodeFolder = null;
                dropNodeFolder = null;
                selectedHierarchy = SAB.HierarchyServerSession.GetHierarchyData(HomeHierarchyRID);
                dropHierarchy = SAB.HierarchyServerSession.GetHierarchyData(aDestinationNode.HomeHierarchyRID);
                selectedHlp = (HierarchyLevelProfile)selectedHierarchy.HierarchyLevels[NodeLevel];
                dropHlp = (HierarchyLevelProfile)dropHierarchy.HierarchyLevels[aDestinationNode.NodeLevel];
                hnp = (HierarchyNodeProfile)Profile;

                // get folder for node being moved or copied
                if (Parent == null) // is folder
                {
                    selectedNodeFolder = (MIDHierarchyNode)this;
                }
                else
                {
                    selectedNodeFolder = (MIDHierarchyNode)Parent;

                    while (selectedNodeFolder.Parent != null)
                    {
                        selectedNodeFolder = (MIDHierarchyNode)selectedNodeFolder.Parent;
                    }

                    selectedNodeHierarchyType = SAB.HierarchyServerSession.GetHierarchyType(HierarchyRID);
                }

                // get folder for node where the node is being copied to
                if (aDestinationNode.Parent == null)
                {
                    dropNodeFolder = (MIDHierarchyNode)aDestinationNode;
                }
                else
                {
                    dropNodeFolder = (MIDHierarchyNode)aDestinationNode.Parent;

                    while (dropNodeFolder.Parent != null)
                    {
                        dropNodeFolder = (MIDHierarchyNode)dropNodeFolder.Parent;
                    }

                    dropNodeHierarchyType = SAB.HierarchyServerSession.GetHierarchyType(aDestinationNode.HierarchyRID);
                }

                // can't move or copy from My Hierarchy to another type
                if (aDestinationNode.Profile.ProfileType == eProfileType.MerchandiseMainAlternatesFolder ||
                    aDestinationNode.Profile.ProfileType == eProfileType.MerchandiseMainUserFolder ||
                    aDestinationNode.Profile.ProfileType == eProfileType.MerchandiseMainOrganizationalFolder)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotDropOnFolder);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotDropOnFolder, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                // can't move or copy from My Hierarchy to another type
                else if (selectedNodeFolder.Profile.ProfileType == eProfileType.MerchandiseMainUserFolder &&
                        dropNodeFolder.Profile.ProfileType != eProfileType.MerchandiseMainUserFolder)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidMyHierarchyCopy);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidMyHierarchyCopy, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                // can't drag from Alternate to Organizational
                else if (selectedNodeFolder.Profile.ProfileType == eProfileType.MerchandiseMainAlternatesFolder &&
                        dropNodeFolder.Profile.ProfileType == eProfileType.MerchandiseMainOrganizationalFolder)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidAlternateCopy);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidAlternateCopy, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                // nodes must be same level in same Organizational hierarchy
                else if (dropNodeHierarchyType == eHierarchyType.organizational &&
                        aDestinationNode.HomeHierarchyRID == HomeHierarchyRID &&
                        aDestinationNode.NodeLevel != NodeLevel - 1)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeSameLevel);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeSameLevel, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                // can not drop node in path if not in home hierarchy
                else if (aDestinationNode.HierarchyRID != aDestinationNode.HomeHierarchyRID)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotDropInSharedPath);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotDropInSharedPath, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                else if (HomeHierarchyParentRID == aDestinationNode.Profile.Key &&
                        HierarchyType == eHierarchyType.organizational &&
                        (selectedHlp.LevelType == eHierarchyLevelType.Color || selectedHlp.LevelType == eHierarchyLevelType.Size))
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotCopyColorSizeToSelf);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotCopyColorSizeToSelf, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                //else if (this == null)
                //{
                //    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NothingToPaste);
                //    return false;
                //}
                // check for circular relationship.  See if from node is in to node path
                else if (aDestinationNode.HomeHierarchyRID == HomeHierarchyRID &&
                        dropNodeHierarchyType == eHierarchyType.alternate)
                {
                    circularRelationship = false;
                    dropNodePath = aDestinationNode;

                    while (dropNodePath.Parent != null)
                    {
                        if (dropNodePath.HomeHierarchyRID == HomeHierarchyRID &&
                            dropNodePath.Profile.Key == Profile.Key)
                        {
                            circularRelationship = true;
                            break;
                        }
                        else
                        {
                            dropNodePath = (MIDHierarchyNode)dropNodePath.Parent;
                        }
                    }

                    if (circularRelationship)
                    {
						//Begin TT#474 - JScott - Create My Hier w dash - errors
						//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CircularNodeRelationship);
						Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CircularNodeRelationship, false);
						//End TT#474 - JScott - Create My Hier w dash - errors
						return false;
                    }
                }
                else if (selectedNodeParent != null &&
                    HierarchyRID != HomeHierarchyRID &&
                    HomeHierarchyRID == selectedNodeParent.HomeHierarchyRID &&
                    HomeHierarchyRID != aDestinationNode.HomeHierarchyRID)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveFromSharedPath);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveFromSharedPath, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }

				if (aDropAction == DragDropEffects.Link ||
					aDropAction == DragDropEffects.Move ||
					(aDropAction == DragDropEffects.Copy && dropNodeHierarchyType == eHierarchyType.alternate))
                {
                    nodeFound = false;

                    // Begin Track #6389 - JSmith - Alternate hierarchy making copy instead of reference
                    // Load children if not already loaded before checking if already a child
                    if (aDestinationNode.HasChildren && aDestinationNode.DisplayChildren && !aDestinationNode.ChildrenLoaded)
                    {
                        try
                        {
                            //Begin TT#453 - JSmith - Product Characteristics  receive a null reference exception 
                            //TreeView.BeginUpdate();
                            aDestinationNode.TreeView.BeginUpdate();
                            //End TT#453

                            aDestinationNode.Nodes.Clear();
                            aDestinationNode.BuildChildren();
                            //Begin TT#453 - JSmith - Product Characteristics  receive a null reference exception
                            //((HierarchyTreeView)TreeView).SortChildNodes(aDestinationNode);
                            ((HierarchyTreeView)aDestinationNode.TreeView).SortChildNodes(aDestinationNode);
                            //End TT#453

                            aDestinationNode.ChildrenLoaded = true;
                        }
                        catch 
                        {
                            throw;
                        }
                        finally
                        {
                            //Begin TT#453 - JSmith - Product Characteristics  receive a null reference exception
                            //TreeView.EndUpdate();
                            aDestinationNode.TreeView.EndUpdate();
                            //End TT#453
                        }
                    }
                    // End Track #6389

                    // Begin TT#564 - JSmith - Copy/Paste from search not working
                    //// check if node already a child
                    //foreach (MIDHierarchyNode mtn in aDestinationNode.Nodes)
                    //{
                    //    // Begin Track #6389 - JSmith - Alternate hierarchy making copy instead of reference
                    //    //if (mtn.HomeHierarchyType == eHierarchyType.organizational && mtn.Profile.Key == Profile.Key)
                    //    if (mtn.Profile.Key == Profile.Key)
                    //    // End Track #6389
                    //    {
                    //        nodeFound = true;
                    //        break;
                    //    }
                    //}
                    //if (nodeFound)
                    //{
                    //    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeAlreadyInParent);
                    //    return false;
                    //}
                    // End TT#564 
                }

                // Begin TT#564 - JSmith - Copy/Paste from search not working
                // check if node already a child
                nodeFound = false;
                foreach (MIDHierarchyNode mtn in aDestinationNode.Nodes)
                {
                    if (mtn.Profile.Key == Profile.Key)
                    {
                        nodeFound = true;
                        break;
                    }
                    // Begin TT#1074 - JSmith - Drag/Drop allows duplicate colors
                    else if (aDestinationNode.HierarchyType == eHierarchyType.organizational &&
                        hnp.ColorOrSizeCodeRID > 0 &&
                        ((HierarchyNodeProfile)mtn.Profile).HomeHierarchyLevel == hnp.HomeHierarchyLevel &&
                        ((HierarchyNodeProfile)mtn.Profile).ColorOrSizeCodeRID == hnp.ColorOrSizeCodeRID)
                    {
                        nodeFound = true;
                        break;
                    }
                    // End TT#1074
                
                }
                if (nodeFound)
                {
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeAlreadyInParent);
					Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeAlreadyInParent, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
					return false;
                }
                // End TT#564 

                // if both organizational and not the same hierarchy, check levels
                if ((selectedNodeHierarchyType == eHierarchyType.organizational &&
                    dropNodeHierarchyType == eHierarchyType.organizational) &&
                    (HomeHierarchyRID != aDestinationNode.HomeHierarchyRID))
                {
                    validLevels = true;
                    fromLevel = NodeLevel;

                    // to node is parent, so start at next level
                    toLevel = aDestinationNode.NodeLevel + 1;

                    if ((selectedHierarchy.HierarchyLevels.Count - fromLevel) != (dropHierarchy.HierarchyLevels.Count - toLevel))
                    {
                        validLevels = false;
                    }
                    else
                    {
                        for (int i = fromLevel; i < selectedHierarchy.HierarchyLevels.Count; i++, toLevel++)
                        {
                            selectedHlp = (HierarchyLevelProfile)selectedHierarchy.HierarchyLevels[i];
                            dropHlp = (HierarchyLevelProfile)dropHierarchy.HierarchyLevels[toLevel];

                            if (selectedHlp.LevelType != dropHlp.LevelType)
                            {
                                validLevels = false;
                            }
                            else if (selectedHlp.LevelRequiredSize != dropHlp.LevelRequiredSize)
                            {
                                validLevels = false;
                            }
                            else if (selectedHlp.LevelSizeRangeFrom != dropHlp.LevelSizeRangeFrom ||
                                    selectedHlp.LevelSizeRangeTo != dropHlp.LevelSizeRangeTo)
                            {
                                validLevels = false;
                            }
                        }
                    }

                    if (!validLevels)
                    {
						//Begin TT#474 - JScott - Create My Hier w dash - errors
						//Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LevelsDoNotMatch);
						Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LevelsDoNotMatch, false);
						//End TT#474 - JScott - Create My Hier w dash - errors
						return false;
                    }
                }

                DropAction = aDropAction;

                // same hierarchy and parent so make copy
                if ((selectedNodeHierarchyType == eHierarchyType.organizational) &&
                    (HierarchyRID == aDestinationNode.HierarchyRID) &&
                    (HomeHierarchyParentRID == aDestinationNode.Profile.Key))
                {
					DropAction = DragDropEffects.Copy;
                }
                // can only make shortcuts from hierarchy to alternate
                else if (dropNodeHierarchyType == eHierarchyType.alternate &&
                        HierarchyRID != aDestinationNode.HierarchyRID)
                {
					DropAction = DragDropEffects.Link;
                }

                aDestinationNode.DropAction = DropAction;

                //Debug.WriteLine("Node " + aDestinationNode.Text + " - " + DropAction);
                return true;
            }
            catch
            {
                throw;
            }
        }

		private bool isFolderDropAllowed(DragDropEffects aDropAction, MIDHierarchyNode aDestinationNode)
        {
            MIDHierarchyNode dropNodeFolder = null;
            eProfileType folderChildType;
            MIDHierarchyNode selectedNodeParent;

            try
            {
                // can't drop on itself
                if (this == aDestinationNode)
                {
                    return false;
                }

                // can't drop on descendant
                dropNodeFolder = (MIDHierarchyNode)aDestinationNode;
                while (dropNodeFolder != null)
                {
                    if (this == dropNodeFolder)
                    {
                        return false;
                    }
                    if (dropNodeFolder.Parent != null)
                    {
                        dropNodeFolder = (MIDHierarchyNode)dropNodeFolder.Parent;
                    }
                    else
                    {
                        dropNodeFolder = null;
                    }
                }

                // check for duplicate
                if (Profile.ProfileType == eProfileType.MerchandiseSubFolder)
                {
                    folderChildType = eProfileType.MerchandiseSubFolder;
                }
                else
                {
                    folderChildType = eProfileType.HierarchyNode;
                }

                if (DlFolder.Folder_Child_Exists(aDestinationNode.Profile.Key, Profile.Key, folderChildType) ||
                    DlFolder.Folder_Shortcut_Exists(aDestinationNode.Profile.Key, Profile.Key, folderChildType))
                {
                    return false;
                }

                selectedNodeParent = null;

                if (Parent != null && Parent is MIDHierarchyNode)
                {
                    selectedNodeParent = (MIDHierarchyNode)Parent;
                }

                DropAction = aDropAction;

                if (isFavoritesFolder
                    && aDestinationNode.isFavoritesFolder)
                {
					DropAction = DragDropEffects.Move;
                }
                else if (selectedNodeParent != null &&
                    ((MIDHierarchyNode)Parent).isFavoritesFolder
                    && aDestinationNode.isFavoritesFolder)
                {
					DropAction = DragDropEffects.Move;
                }
                else
                {
					DropAction = DragDropEffects.Link;
                }

                aDestinationNode.DropAction = DropAction;

                //Debug.WriteLine("Folder " + aDestinationNode.Text + " - " + DropAction);

                return true;
            }
            catch
            {
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
            HierarchyNodeProfile hnp;
            HierarchyProfile hp;

            try
            {
                if (isMainAlternatesFolder ||
                    isMainOrganizationalFolder)
                {
                    return false;
                }
                // cannot rename if is reference
                else if (HierarchyRID != HomeHierarchyRID) 
                {
                    return false;
                }
                // cannot rename complex display options
                else if (DisplayOption != eHierarchyDisplayOptions.IdOnly &&  
                         DisplayOption != eHierarchyDisplayOptions.NameOnly &&
                         DisplayOption != eHierarchyDisplayOptions.DescriptionOnly)
                {
                    return false;
                }
                else if (isMainFavoritesFolder ||
                         isFavoritesSubFolder)
                {
                    return true;
                }
                else if (Profile.ProfileType == eProfileType.HierarchyNode)
                {
                    // Begin TT#65 - JSmith - Merchandise Explorer "MY" Hierarchy when copy and paste receive Hierarchy Node Conflict message.
                    //// lock the node for update
                    //if (isAlternateHierarchyRoot ||
                    //    isOrganizationalHierarchyRoot)
                    //{
                    //    hp = SAB.HierarchyServerSession.GetHierarchyDataForUpdate(HierarchyRID, false);
                    //    if (hp.HierarchyLockStatus != eLockStatus.Locked)
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    hnp = SAB.HierarchyServerSession.GetNodeDataForUpdate(Profile.Key, false);
                    //    if (hnp.NodeLockStatus != eLockStatus.Locked)
                    //    {
                    //        return false;
                    //    }
                    //}
                    // End TT#65
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
				if (isObjectShortcut || isFolderShortcut)
                {
                    UserId = aChangedNode.UserId;

                    if (Profile.ProfileType == eProfileType.HierarchyNode)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((HierarchyNodeProfile)aChangedNode.Profile).Text + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((HierarchyNodeProfile)aChangedNode.Profile).Text + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                    else
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                }
                else if (isChildShortcut)
                {
                    UserId = aChangedNode.UserId;

                    if (((Profile)Profile).ProfileType == eProfileType.HierarchyNode)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((HierarchyNodeProfile)aChangedNode.Profile).Text;
						InternalText = ((HierarchyNodeProfile)aChangedNode.Profile).Text;
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                    else
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((FolderProfile)aChangedNode.Profile).Name;
						InternalText = ((FolderProfile)aChangedNode.Profile).Name;
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
		}

		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        // Begin Track #5005 - JSmith - Explorer Organization
        /// <summary>
        /// Retrieves a list of children for the node
        /// </summary>
        /// <returns>An ArrayList containing profiles for each child</returns>
        override public void BuildChildren()
        {
            ArrayList children;

            try
            {
                children = BuildChildrenList();
                Nodes.AddRange((MIDTreeNode[])children.ToArray(typeof(MIDTreeNode)));
            }
            catch
            {
                throw;
            }
        }

        public ArrayList BuildChildrenList()
        {
            ArrayList children;

            try
            {
                children = new ArrayList();

                switch (Profile.ProfileType)
                {
                    case eProfileType.MerchandiseMainFavoritesFolder:
                        children = BuildFolderChildren();
                        // Begin TT#5644 - JSmith - Security Set Up - Users Need to Remove Article Lists from My Favorites
                        foreach (MIDHierarchyNode node in children)
                        {
                            node.FunctionSecurityProfile.SetAllowDelete();
                            node.HierarchyNodeSecurityProfile.SetAllowDelete();
                        }
                        // End TT#5644 - JSmith - Security Set Up - Users Need to Remove Article Lists from My Favorites
                        break;
                    case eProfileType.MerchandiseSubFolder:
                        children = BuildFolderChildren();
                        break;
                    case eProfileType.MerchandiseMainSharedFolder:
                        children = BuildSharedChildren();
                        break;
                    default:
                        children = BuildHierarchyChildren();
                        break;
                }
            }
            catch
            {
                throw;
            }
            return children;
        }

        // Begin Track #5005 - JSmith - Explorer Organization
        /// <summary>
        /// Retrieves a stack of descendants for the folder
        /// </summary>
        /// <param name="aProfile">
        /// The profile of the folder for which descendants are to be retrieved
        /// </param>
        /// <returns>A Stack containing FolderRelationship objects for each descendant</returns>
        public Stack Get_Folder_Descendants(Profile aProfile)
        {
            Stack descendantStack;
            try
            {
                descendantStack = new Stack();
                Get_Folder_Descendants(null, aProfile, descendantStack);
                return descendantStack;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a stack of descendants for the folder
        /// </summary>
        /// <returns>A Stack containing FolderRelationship objects for each descendant</returns>
        private void Get_Folder_Descendants(Profile aParentProfile, Profile aChildProfile, Stack aDescendantList)
        {
            ArrayList children;

            try
            {
                if (aParentProfile != null)
                {
                    aDescendantList.Push(new FolderRelationship(aParentProfile, aChildProfile));
                }

                if (aChildProfile.ProfileType != eProfileType.HierarchyNode)
                {
                    children = BuildFolderChildren(((FolderProfile)aChildProfile).OwnerUserRID, aChildProfile.Key);
                    // Begin TT#25 - Explorer Favorites tried to delete a folder that has an organizational node and receive a Invalid Cast Exception
                    //foreach (Profile profile in children)
                    //{
                    //    Get_Folder_Descendants(aChildProfile, profile, aDescendantList);
                    //}
                    foreach (object profile in children)
                    {
                        if (profile is MIDHierarchyNode)
                        {
                            Get_Folder_Descendants(aChildProfile, ((MIDHierarchyNode)profile).Profile, aDescendantList);
                        }
                        else
                        {
                            Get_Folder_Descendants(aChildProfile, (FolderProfile)profile, aDescendantList);
                        }
                    }
                    // End TT#25
                }
            }
            catch
            {
                throw;
            }
        }

		private ArrayList BuildHierarchyChildren()
        {
            try
            {
                bool parentAccessDenied = false;
                eNodeSelectType selectType = eNodeSelectType.NoVirtual;
                if (DebugActivated)
                {
                    selectType = eNodeSelectType.All;
                }
                HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetHierarchyChildren(HomeHierarchyLevel, HierarchyRID,
                    HomeHierarchyRID, Profile.Key, false, selectType, FunctionSecurityProfile.AccessDenied);

                HierarchyProfile parentHierarchyProfile = SAB.HierarchyServerSession.GetHierarchyData(HomeHierarchyRID);
                ArrayList treenodes = new ArrayList();

                if (SecurityProfileIsInitialized &&
                  HierarchyNodeSecurityProfile.AccessDenied)
                {
                    parentAccessDenied = true;
                }

                foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                {
                    MIDHierarchyNode mtn = BuildHierarchyNode(HierarchyRID, HierarchyType, hnp, parentAccessDenied);

                    if (DebugActivated)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//mtn.Text += " : #" + mtn.Profile.Key.ToString();
						mtn.InternalText += " : #" + mtn.Profile.Key.ToString();
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
                    treenodes.Add(mtn);
                }

                return treenodes;
            }
            catch
            {
                throw;
            }
        }

        private ArrayList BuildFolderChildren()
        {
            return BuildFolderChildren(SAB.ClientServerSession.UserRID, Profile.Key);
        }

        private ArrayList BuildFolderChildren(int aUserRID, int aKey)
        {
            ArrayList children;
            FolderDataLayer dlFolder;
            DataTable dtFolder;
            eProfileType childItemType;
            int childItemRID, userRID, ownerUserRID;
            HierarchyNodeProfile hnp;
            FolderProfile folderProf;
            MIDHierarchyNode newNode = null;

            children = new ArrayList();
            dlFolder = new FolderDataLayer();
            dtFolder = dlFolder.Folder_Children_Read(aUserRID, aKey);
            foreach (DataRow dr in dtFolder.Rows)
            {
                childItemType = (eProfileType)Convert.ToInt32(dr["CHILD_ITEM_TYPE"]);
                childItemRID = Convert.ToInt32(dr["CHILD_ITEM_RID"]);
                userRID = Convert.ToInt32(dr["USER_RID"]);
                ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
                switch (childItemType)
                {
                    case eProfileType.MerchandiseSubFolder:
                        folderProf = new FolderProfile(childItemRID, userRID, childItemType, dlFolder.Folder_GetName(childItemRID), ownerUserRID);

                        newNode = BuildFolderNode(folderProf, false);

                        newNode.HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(newNode.Profile.Key);
                        newNode.HierarchyNodeSecurityProfile.SetFullControl();
                        if (((MIDTreeView)TreeView).Folder_Children_Exists(SAB.ClientServerSession.UserRID, newNode.Profile.Key))
                        {
                            newNode.Nodes.Add(((HierarchyTreeView)TreeView).BuildPlaceHolderNode());
                            newNode.ChildrenLoaded = false;
                            newNode.HasChildren = true;
                            newNode.DisplayChildren = true;
                        }
                        else
                        {
                            newNode.ChildrenLoaded = true;
                        }
                        children.Add(newNode);
                        break;
                    case eProfileType.HierarchyNode:
                        hnp = SAB.HierarchyServerSession.GetNodeData(childItemRID);
                        children.Add(BuildHierarchyNode(HierarchyRID, HierarchyType, hnp, false));
                        break;
                }
            }

            return children;
        }

        // End Track #5005

        private ArrayList BuildSharedChildren()
        {
            ArrayList children;
            FolderDataLayer dlFolder;
            DataTable dtFolder;
            eProfileType childItemType;
            int childItemRID, userRID, ownerUserRID;
            FolderProfile folderProf;
            MIDHierarchyNode newNode = null;
            ArrayList userList;

            try
            {
                children = new ArrayList();
                dlFolder = new FolderDataLayer();
                userList = new ArrayList();
                userList.Add(SAB.ClientServerSession.UserRID);
                dtFolder = DlFolder.Folder_Read(userList, eProfileType.MerchandiseMainUserFolder, false, true);
                foreach (DataRow dr in dtFolder.Rows)
                {
                    childItemType = (eProfileType)Convert.ToInt32(dr["ITEM_TYPE"]);
                    childItemRID = Convert.ToInt32(dr["FOLDER_RID"]);
                    userRID = Convert.ToInt32(dr["USER_RID"]);
                    ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
                    folderProf = new FolderProfile(childItemRID, userRID, childItemType, GetUserName(ownerUserRID), ownerUserRID);

                    newNode = BuildFolderNode(folderProf, false);

                    newNode.HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(newNode.Profile.Key);
                    newNode.HierarchyNodeSecurityProfile.SetReadOnly();
                    if (((MIDTreeView)TreeView).Folder_Children_Exists(SAB.ClientServerSession.UserRID, newNode.Profile.Key))
                    {
                        newNode.Nodes.Add(((HierarchyTreeView)TreeView).BuildPlaceHolderNode());
                        newNode.ChildrenLoaded = false;
                        newNode.HasChildren = true;
                        newNode.DisplayChildren = true;
                    }
                    else
                    {
                        newNode.ChildrenLoaded = true;
                    }
                    children.Add(newNode);

                }
                return children;
            }
            catch
            {
                throw;
            }
        }

        private MIDHierarchyNode BuildHierarchyNode(int aHierarchyRID, eHierarchyType aHierarchyType, HierarchyNodeProfile aHierarchyNodeProfile,
           bool aParentAccessDenied)
        {
            HierarchyNodeProfile hnp;
            int collapsedImageIndex, expandedImageIndex;
            eTreeNodeType treeNodeType;
            int userRID;

            try
            {

                hnp = aHierarchyNodeProfile;

                collapsedImageIndex = GetFolderImageIndex(hnp.HomeHierarchyRID != aHierarchyRID, hnp.NodeColor, MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(hnp.HomeHierarchyRID != aHierarchyRID, hnp.NodeColor, MIDGraphics.OpenFolder);

                if (aHierarchyNodeProfile.HomeHierarchyRID != aHierarchyRID)
                {
                    if (HomeHierarchyRID == aHierarchyRID)
                    {
                        treeNodeType = eTreeNodeType.FolderShortcutNode;
                    }
                    else 
                    {
                        treeNodeType = eTreeNodeType.ChildObjectShortcutNode;
                    }
                }
                else
                {
                    treeNodeType = eTreeNodeType.ObjectNode;
                }

                if (hnp.HomeHierarchyOwner == Include.GlobalUserRID ||
                    hnp.HomeHierarchyOwner == SAB.ClientServerSession.UserRID)
                {
                    userRID = hnp.HomeHierarchyOwner;
                }
                else
                {
                    userRID = SAB.ClientServerSession.UserRID;
                }

                MIDHierarchyNode mtn = new MIDHierarchyNode(
                    SAB,
                    treeNodeType,
                    hnp,
                    hnp.Text,
                    hnp.HomeHierarchyParentRID,
                    userRID,
                    null,
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    hnp.HomeHierarchyOwner
                    );

                mtn.NodeID = hnp.NodeID;
                mtn.NodeChangeType = eChangeType.none;
                mtn.HierarchyRID = aHierarchyRID;
                mtn.HierarchyType = aHierarchyType;

                mtn.HasChildren = hnp.HasChildren;
                mtn.DisplayChildren = hnp.DisplayChildren;
                if (mtn.NodeLevel == 0 ||
                    aParentAccessDenied)
                {
                    if (mtn.HierarchyNodeSecurityProfile.AccessDenied)
                    {
                        mtn.ForeColor = SystemColors.InactiveCaption;
                    }

                }

                if (mtn.HasChildren
                    && mtn.DisplayChildren)
                {
                    // Begin TT#400 - JSmith - Merchandise Explorer Copy/Paste making a Copy of in the Alternate Hierarchy received a Object Reference Error.
                    //mtn.Nodes.Add(((HierarchyTreeView)TreeView).BuildPlaceHolderNode());
                    if ((HierarchyTreeView)TreeView != null)
                    {
                        mtn.Nodes.Add(((HierarchyTreeView)TreeView).BuildPlaceHolderNode());
                    }
                    // End TT#400
                    mtn.ChildrenLoaded = false;
                }
                else
                {
                    mtn.ChildrenLoaded = true;
                }

                if (mtn.Profile.ProfileType == eProfileType.Hierarchy)
                {
                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID);
                    if (hp.Owner > Include.GlobalUserRID) // personal hierarchy		
                    {
                        mtn.HierarchyNodeSecurityProfile.SetFullControl();	// override security for personal hierarchies to full access
                    }
                }
                return mtn;
            }
            catch
            {
                throw;
            }
        }

        public MIDHierarchyNode BuildFolderNode(FolderProfile aFolderProf, bool aParentAccessDenied)
        {
            int collapsedImageIndex, expandedImageIndex;
            eTreeNodeType treeNodeType;
            MIDHierarchyNode newNode;

            try
            {
                collapsedImageIndex = GetFolderImageIndex(false, Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
                expandedImageIndex = GetFolderImageIndex(false, Include.MIDDefaultColor, MIDGraphics.OpenFolder);

                treeNodeType = eTreeNodeType.SubFolderNode;

                newNode = new MIDHierarchyNode(
                    SAB,
                    treeNodeType,
                    aFolderProf,
                    aFolderProf.Name,
                    Include.NoRID,
                    aFolderProf.UserRID,
                    null,
                    collapsedImageIndex,
                    collapsedImageIndex,
                    expandedImageIndex,
                    expandedImageIndex,
                    aFolderProf.OwnerUserRID
                    );

                newNode.HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(newNode.Profile.Key);
                newNode.HierarchyNodeSecurityProfile.SetFullControl();
                if (((MIDTreeView)TreeView).Folder_Children_Exists(SAB.ClientServerSession.UserRID, newNode.Profile.Key))
                {
                    newNode.Nodes.Add(((HierarchyTreeView)TreeView).BuildPlaceHolderNode());
                    newNode.ChildrenLoaded = false;
                    newNode.HasChildren = true;
                    newNode.DisplayChildren = true;
                }
                else
                {
                    newNode.ChildrenLoaded = true;
                }
                return newNode;
            }
            catch
            {
                throw;
            }
        }

        public int GetFolderImageIndex(bool aIsReference, string aColor, string aFolderType)
        {
            try
            {
                if (aIsReference)
                {
                    return MIDGraphics.ImageShortcutIndexWithDefault(aColor, aFolderType);
                }
                else
                {
                    return MIDGraphics.ImageIndexWithDefault(aColor, aFolderType);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
