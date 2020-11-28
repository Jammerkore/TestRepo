using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class ROHierarchy : ROWebFunction
    {

        private List<String> DebugKeyList = new List<string>();
        private Dictionary<string, MyHierarchyNode> NodeDictionary;
        private MyHierarchyTreeView treeView;
        private Dictionary<string, int> iconColors;

        public ROHierarchy(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            NodeDictionary = new Dictionary<string, MyHierarchyNode>();
            treeView = new MyHierarchyTreeView();
            iconColors = new Dictionary<string, int>();

            foreach (eIconColor iColor in Enum.GetValues(typeof(eIconColor)))
            {
                iconColors[iColor.ToString().ToLower()] = (int)iColor;
            }
            iconColors["kaki"] = (int)eIconColor.Khaki; // fix spelling error - I think it's in the database
        }

        override public void CleanUp()
        {

        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.GetHierarchies:
                    return GetHierarchyTable();
                case eRORequest.GetHierarchyChildren:
                    ROPagingParms parms = (ROPagingParms)Parms;
                    return GetChildNodes(parms.RONodeAddress, parms.ROStartRecord, parms.ROMaxRecords);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        private ROOut GetHierarchyTable()
        {
            DataTable dt = BuildHierarchyDataTable();

            FillHierarchyTable(dt);

            return new RODataTableOut(eROReturnCode.Successful, null, ROInstanceID, dt);
        }

        private DataTable BuildHierarchyDataTable()
        {
            DataTable dt = new DataTable("Hierarchy");

            dt.Columns.Add(new DataColumn("NAME", typeof(string)));
            dt.Columns.Add(new DataColumn("ID", typeof(string)));
            dt.Columns.Add(new DataColumn("NODE_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("PARENT_ADDRESS", typeof(string)));
            dt.Columns.Add(new DataColumn("FETCHABLE_CHILDREN", typeof(string)));
            dt.Columns.Add(new DataColumn("COLLAPSED_IMAGE_FILENAME", typeof(string)));
            dt.Columns.Add(new DataColumn("SELECTED_COLLAPSED_IMage_FILENAME", typeof(string)));
            dt.Columns.Add(new DataColumn("EXPANDED_IMAGE_FILENAME", typeof(string)));
            dt.Columns.Add(new DataColumn("SELECTED_EXPANDED_IMAGE_FILENAME", typeof(string)));
            dt.Columns.Add(new DataColumn("ICON_COLOR", typeof(int)));
            dt.Columns.Add(new DataColumn("ICON_TYPE", typeof(int)));
            dt.Columns.Add(new DataColumn("LEVEL_NAME", typeof(string)));
            dt.Columns.Add(new DataColumn("IS_ORGANIZATIONAL", typeof(string)));

            return dt;
        }

        private void FillHierarchyTable(DataTable dt) // see LoadNodes() in MyHierarchyNode
        {
            treeView.InitializeTreeView(SAB, ROWebTools);
            treeView.LoadNodes();

            foreach (var treeNode in treeView.Nodes)
            {
                MyHierarchyNode hierarchyNode = (MyHierarchyNode)treeNode;

                treeNode.ChildrenLoaded = false;
                AddNodeAndChildren(hierarchyNode, dt, Include.NoRID.ToString());
            }
        }

        private void AddNodeAndChildren(MyHierarchyNode treeNode, DataTable dt, string sParentAddress)
        {
            if (!IsPlaceHolderNode(treeNode))
            {
                // Ignore place-holder nodes.
                treeView.SortChildNodes(treeNode);
                if (PublishNodeToDictionary(sParentAddress, treeNode))
                {
                    PublishNodeToTable(sParentAddress, treeNode, dt);
                }
            }
        }

        private bool IsPlaceHolderNode(MyHierarchyNode treeNode)
        {
            return string.IsNullOrEmpty(treeNode.Text.Trim());
        }

        private string GetNodeAddress(string sParentNodeAddress, MyHierarchyNode treeNode)
        {
            return sParentNodeAddress + "/" + GetNodeIDFromTreeNode(treeNode);
        }

        private bool PublishNodeToDictionary(string sParentAddress, MyHierarchyNode treeNode)
        {
            string sNodeAddress = GetNodeAddress(sParentAddress, treeNode);
            bool addedNode = false;

            DebugKeyList.Add(sNodeAddress);

            try
            {
                NodeDictionary.Add(sNodeAddress, treeNode);
                addedNode = true;
            }
            catch (Exception exc)
            {
                string sMsg = string.Format("PublishNodeToDictionary({0}) failed: {1}", sNodeAddress, exc.ToString());

                ROWebTools.LogMessage(eROMessageLevel.Information, sMsg);
                //throw exc;
            }

            return addedNode;
        }

        private MyHierarchyNode LookupNode(string sNodeAddress)
        {
            MyHierarchyNode treeNode = null;

            NodeDictionary.TryGetValue(sNodeAddress, out treeNode);

            return treeNode;
        }

        private void FetchNodeChildren(MyHierarchyNode treeNode)
        {
            if (treeNode.HasChildren && treeNode.DisplayChildren && !treeNode.ChildrenLoaded)
            {
                if (treeNode.Nodes.Count > 0
                    && IsPlaceHolderNode((MyHierarchyNode)treeNode.Nodes[0]))
                {
                    treeNode.Nodes.Clear();
                }

                treeNode.BuildChildren();
                treeView.SortChildNodes(treeNode);
                treeNode.ChildrenLoaded = true;
            }
        }

        private ROOut GetChildNodes(string sNodeAddress, int iStartRecord, int iMaxRecords)
        {
            MyHierarchyNode treeNode = LookupNode(sNodeAddress);
            DataTable dt = BuildHierarchyDataTable();

            try
            {
                if (treeNode != null)
                {
                    if (!treeNode.ChildrenLoaded)
                    {
                        FetchNodeChildren(treeNode);
                    }

                    foreach (MyHierarchyNode childNode in treeNode.Nodes)
                    {
                        AddNodeAndChildren(childNode, dt, sNodeAddress);
                    }
                }
            }
            catch (Exception exc)
            {
                string sMsg = string.Format("Caught exception in GetChildNodes({0}): {1}", sNodeAddress, exc.ToString());

                ROWebTools.LogMessage(eROMessageLevel.Information, sMsg);
                throw exc;
            }

            int totalRecords = dt.Rows.Count;
            if (totalRecords == 0)
            {
                //paging issue - all nodes are already loaded by 
                foreach (MyHierarchyNode childNode in treeNode.Nodes)
                {
                    PublishNodeToTable(sNodeAddress, childNode, dt);
                }
                totalRecords = dt.Rows.Count;
            }
			
            if (totalRecords > 0)
            {
                dt = dt.Select().Skip(iStartRecord).Take(iMaxRecords).CopyToDataTable();
                dt.TableName = "Hierarchy";
                dt.ExtendedProperties.Add("TotalRecordsForPaging", totalRecords);
            }

            return new RODataTableOut(eROReturnCode.Successful, null, ROInstanceID, dt);
        }

        private string GetNodeIDFromTreeNode(MyHierarchyNode treeNode)
        {
            return treeNode.NodeRID.ToString();
        }

        private void PublishNodeToTable(string sParentAddress, MyHierarchyNode treeNode, DataTable dt)
        {
            string sNodeID = GetNodeAddress(sParentAddress, treeNode);
            string sNodeName = treeNode.Text;
            bool fetchableChildren = treeNode.HasChildren && treeNode.DisplayChildren && !treeNode.ChildrenLoaded;
            string sCollapsedImageFileName = treeNode.sCollapsedImage;
            string sSelectedCollapsedImageFileName = treeNode.sSelectedCollapsedImage;
            string sExpandedImageFileName = treeNode.sExpandedImage;
            string sSelectedExpandedImageFileName = treeNode.sSelectedExpandedImage;

            DataRow dr = dt.NewRow();

            dr["ID"] = sNodeID;
            dr["NAME"] = sNodeName;
            dr["NODE_ID"] = treeNode.NodeID;
            dr["PARENT_ADDRESS"] = sParentAddress;
            dr["FETCHABLE_CHILDREN"] = fetchableChildren ? "1" : "0";
            dr["COLLAPSED_IMAGE_FILENAME"] = sCollapsedImageFileName;
            dr["SELECTED_COLLAPSED_IMage_FILENAME"] = sSelectedCollapsedImageFileName;
            dr["EXPANDED_IMAGE_FILENAME"] = sExpandedImageFileName;
            dr["SELECTED_EXPANDED_IMAGE_FILENAME"] = sSelectedExpandedImageFileName;
            dr["ICON_COLOR"] = GetIconColor(treeNode);
            dr["ICON_TYPE"] = GetIconType(treeNode);
            dr["LEVEL_NAME"] = treeNode.LevelName;
            dr["IS_ORGANIZATIONAL"] = (treeNode.HierarchyType == eHierarchyType.organizational).ToString();

            dt.Rows.Add(dr);
        }

        private int GetIconColor(MyHierarchyNode treeNode)
        {
            int colorID = (int)eIconColor.Default;
            HierarchyNodeProfile nodeProfile = treeNode.Profile as HierarchyNodeProfile;

            if (nodeProfile != null)
            {
                if (nodeProfile.NodeColor != null)
                {
                    string nodeColor = nodeProfile.NodeColor.ToLower();


                    if (iconColors.ContainsKey(nodeColor))
                    {
                        colorID = iconColors[nodeColor];
                    }
                }
            }

            return colorID;
        }

        private int GetIconType(MyHierarchyNode treeNode)
        {
            eIconType iconType = eIconType.Plain;

            if (treeNode.UseSharedIcon)
            {
                iconType = eIconType.Shared;
            }
            else if (treeNode.isShortcut)
            {
                iconType = eIconType.Shortcut;
            }

            return (int)iconType;
        }
    }

    class MyHierarchyTreeView
    {
        private SessionAddressBlock SAB;
        private ROWebTools ROWebTools;
        public List<MIDTreeNode> Nodes = new List<MIDTreeNode>();

        private string sClosedFolderImage;
        private string sOpenFolderImage;
        private string sClosedShortcutFolderImage;
        private string sOpenShortcutFolderImage;

        private MyHierarchyNode _myHierarchyNode;
        private MyHierarchyNode _organizationalNode;
        private MyHierarchyNode _alternateNode;
        private MyHierarchyNode _sharedNode;
        protected MIDTreeNode _favoritesNode = null;

        private FunctionSecurityProfile _securityAltUser;
        private FunctionSecurityProfile _securityAltGlobal;
        private FunctionSecurityProfile _securityOrg;

        private FolderDataLayer _dlFolder;
        private SecurityAdmin _secAdmin;


        //=============
        // PROPERTIES
        //=============

        public MIDTreeNode SelectedNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the main My Hierarchy node.
        /// </summary>
        public MyHierarchyNode MyHierarchyNode
        {
            get { return _myHierarchyNode; }
            set { _myHierarchyNode = value; }
        }

        /// <summary>
        /// Gets or sets the main Organizational node.
        /// </summary>
        public MyHierarchyNode OrganizationalNode
        {
            get { return _organizationalNode; }
            set { _organizationalNode = value; }
        }

        /// <summary>
        /// Gets or sets the main Alternate node.
        /// </summary>
        public MyHierarchyNode AlternateNode
        {
            get { return _alternateNode; }
            set { _alternateNode = value; }
        }

        /// <summary>
        /// Gets or sets the main Shares node.
        /// </summary>
        public MyHierarchyNode SharedNode
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

        public FolderDataLayer DlFolder
        {
            get
            {
                return _dlFolder;
            }
        }

        public SecurityAdmin DlSecurity
        {
            get
            {
                return _secAdmin;
            }
        }

        public MIDTreeNode FavoritesNode
        {
            get
            {
                return _favoritesNode;
            }
            set
            {
                _favoritesNode = value;
            }
        }

        public bool Sorted
        {
            get;
            set;
        }

        public MyHierarchyTreeView()
        {
            _secAdmin = new SecurityAdmin();
        }

        public void InitializeTreeView(SessionAddressBlock SAB, ROWebTools ROWebTools)
        {
            this.SAB = SAB;
            this.ROWebTools = ROWebTools;

            sClosedFolderImage = MyGraphics.ClosedTreeFolder;
            sOpenFolderImage = MyGraphics.OpenTreeFolder;
            sClosedShortcutFolderImage = MyGraphics.ClosedTreeFolder;
            sOpenShortcutFolderImage = MyGraphics.OpenTreeFolder;

            _securityAltUser = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltUser);
            _securityAltGlobal = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltGlobal);
            _securityOrg = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesOrg);

            _dlFolder = new FolderDataLayer();
        }

        public void LoadNodes()
        {
            SAB.HierarchyServerSession.BuildAvailableNodeList();

            BuildHierarchyFolders();

            //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Loading Explorer", this.GetType().Name);

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
                SortChildNodes(AlternateNode);
            }
            if (SharedNode != null)
            {
                SharedNode.Expand();
            }
        }


        /// <summary>
        /// Builds a node for each type of hierarchy
        /// </summary>
        private void BuildHierarchyFolders()
        {
            string sClosedImage, sExpandedImage;

            FolderProfile folderProfile;
            FunctionSecurityProfile functionSecurity;
            DataTable dtSharedFolders;
            ArrayList userList;

            try
            {
                // Favorites folder
                folderProfile = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.MerchandiseMainFavoritesFolder, "My Favorites");
                sClosedImage = MyGraphics.FavoritesImage;
                sExpandedImage = MyGraphics.FavoritesImage;
                FavoritesNode = new MyHierarchyNode(
                    SAB,
                    eTreeNodeType.MainFavoriteFolderNode,
                    folderProfile,
                    folderProfile.Name,
                    Include.NoRID,
                    folderProfile.UserRID,
                    null,
                    sClosedImage,
                    sClosedImage,
                    sExpandedImage,
                    sExpandedImage,
                    folderProfile.OwnerUserRID);

                // check for children
                if (Folder_Children_Exists(SAB.ClientServerSession.UserRID, FavoritesNode.Profile.Key))
                {
                    FavoritesNode.Nodes.Add(new MyHierarchyNode());
                    FavoritesNode.ChildrenLoaded = false;
                    FavoritesNode.HasChildren = true;
                    FavoritesNode.DisplayChildren = true;
                }
                else
                {
                    FavoritesNode.ChildrenLoaded = true;
                }
                ((MyHierarchyNode)FavoritesNode).HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(FavoritesNode.Profile.Key);
                ((MyHierarchyNode)FavoritesNode).HierarchyNodeSecurityProfile.SetFullControl();

                // User folder
                folderProfile = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.MerchandiseMainUserFolder, SAB.ClientServerSession.MyHierarchyName);
                sClosedImage = SAB.ClientServerSession.MyHierarchyColor + MyGraphics.ClosedFolder;
                sExpandedImage = SAB.ClientServerSession.MyHierarchyColor + MyGraphics.OpenFolder;
                functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltUser);
                functionSecurity.SetDenyDelete();
                MyHierarchyNode = new MyHierarchyNode(
                    SAB,
                    eTreeNodeType.MainSourceFolderNode,
                    folderProfile,
                    folderProfile.Name,
                    Include.NoRID,
                    folderProfile.UserRID,
                    functionSecurity,
                    sClosedImage,
                    sClosedImage,
                    sExpandedImage,
                    sExpandedImage,
                    folderProfile.OwnerUserRID);

                // Organizational folder
                folderProfile = Folder_Get(Include.GlobalUserRID, eProfileType.MerchandiseMainOrganizationalFolder, MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type_Organizational));
                sClosedImage = GetFolderImageIndex(false, Include.MIDDefaultColor, MyGraphics.ClosedFolder);
                sExpandedImage = GetFolderImageIndex(false, Include.MIDDefaultColor, MyGraphics.OpenFolder);
                functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesOrgNodes);
                functionSecurity.SetDenyDelete();
                OrganizationalNode = new MyHierarchyNode(
                    SAB,
                    eTreeNodeType.MainSourceFolderNode,
                    folderProfile,
                    folderProfile.Name,
                    Include.NoRID,
                    folderProfile.UserRID,
                    functionSecurity,
                    sClosedImage,
                    sClosedImage,
                    sExpandedImage,
                    sExpandedImage,
                    folderProfile.OwnerUserRID);

                // alternate folder
                folderProfile = Folder_Get(Include.GlobalUserRID, eProfileType.MerchandiseMainAlternatesFolder, MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type_Alternate));
                sClosedImage = GetFolderImageIndex(false, Include.MIDDefaultColor, MyGraphics.ClosedFolder);
                sExpandedImage = GetFolderImageIndex(false, Include.MIDDefaultColor, MyGraphics.OpenFolder);
                functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltNodes);
                functionSecurity.SetDenyDelete();
                AlternateNode = new MyHierarchyNode(
                    SAB,
                    eTreeNodeType.MainSourceFolderNode,
                    folderProfile,
                    folderProfile.Name,
                    Include.NoRID,
                    folderProfile.UserRID,
                    functionSecurity,
                    sClosedImage,
                    sClosedImage,
                    sExpandedImage,
                    sExpandedImage,
                    folderProfile.OwnerUserRID);

                // shared folder
                SharedNode = null;
                userList = new ArrayList();
                userList.Add(SAB.ClientServerSession.UserRID);
                dtSharedFolders = DlFolder.Folder_Read(userList, eProfileType.MerchandiseMainUserFolder, false, true);
                if (dtSharedFolders.Rows.Count > 0)
                {
                    folderProfile = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.MerchandiseMainSharedFolder, MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName), SAB.ClientServerSession.UserRID);
                    sClosedImage = Include.MIDDefaultColor + MyGraphics.ClosedFolder;
                    sExpandedImage = Include.MIDDefaultColor + MyGraphics.OpenFolder;
                    functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesAltUser);
                    SharedNode = new MyHierarchyNode(
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProfile,
                        folderProfile.Name,
                        Include.NoRID,
                        folderProfile.UserRID,
                        functionSecurity,
                        sClosedImage,
                        sClosedImage,
                        sExpandedImage,
                        sExpandedImage,
                        folderProfile.OwnerUserRID);

                    // add dummy node for expanding
                    SharedNode.Nodes.Add(new MyHierarchyNode());
                    SharedNode.ChildrenLoaded = false;
                    SharedNode.HasChildren = true;
                    SharedNode.DisplayChildren = true;
                    SharedNode.UseSharedIcon = true;
                }
            }
            catch
            {
                throw;
            }
        }

        public string GetFolderImageIndex(bool aIsReference, string aColor, string aFolderType)
        {
            try
            {
                if (aIsReference)
                {
                    return aColor + aFolderType;
                }
                else
                {
                    return aColor + aFolderType;
                }
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

        private MyHierarchyNode[] GetRootNodes()
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

            return (MyHierarchyNode[])treenodes.ToArray(typeof(MyHierarchyNode));
        }

        private MyHierarchyNode[] GetHierarchies()
        {
            Sorted = true;
            HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetRootNodes();

            ArrayList treenodes = new ArrayList();

            foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
            {
                MyHierarchyNode mtn = BuildNode(hnp.HomeHierarchyRID, hnp.HomeHierarchyType, null, hnp, false);
                mtn.isHierarchyRoot = true;

                //if (DebugActivated)
                //{
                //    mtn.InternalText += " : #" + mtn.Profile.Key.ToString();
                //}

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
                        foreach (MyHierarchyNode node in SharedNode.Nodes)
                        {
                            if (hp.Owner == node.OwnerUserRID)
                            {
                                //mtn.HierarchyNodeSecurityProfile.SetReadOnly();	// override security for personal hierarchies to full access
                                node.Nodes.Add(mtn);
                                node.HasChildren = true;
                                node.DisplayChildren = true;
                                node.ChildrenLoaded = true;
                                mtn.OwnerUserRID = hp.Owner;
                                //mtn.ImageIndex = MyGraphics.ImageSharedIndexWithDefault(mtn.NodeColor, MyGraphics.ClosedFolder);
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

            return (MyHierarchyNode[])treenodes.ToArray(typeof(MyHierarchyNode));
        }

        public void SortChildNodes(MIDTreeNode aParentNode)
        {
            MIDTreeNode selectedNode;
            MIDTreeNode[] nodeArray;
            int i;
            SortedList folderList;
            SortedList itemList;
            IDictionaryEnumerator dictEnum;

            try
            {
                //if (!aParentNode.isChildrenSorted())
                //{
                //    return;
                //}
                //BeginUpdate();

                try
                {
                    selectedNode = SelectedNode;

                    if (aParentNode.Nodes.Count > 0)
                    {
                        nodeArray = new MIDTreeNode[aParentNode.Nodes.Count];
                        aParentNode.Nodes.CopyTo(nodeArray, 0);
                        aParentNode.Nodes.Clear();

                        folderList = new SortedList();
                        itemList = new SortedList();

                        for (i = 0; i < nodeArray.Length; i++)
                        {
                            if (nodeArray[i].TreeNodeType == eTreeNodeType.SubFolderNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.MainNonSourceFolderNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.MainSourceFolderNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.FolderShortcutNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.ChildFolderShortcutNode)
                            {
                                if (nodeArray[i].Nodes.Count > 0)
                                {
                                    SortChildNodes(nodeArray[i]);
                                }

                                folderList.Add(nodeArray[i], nodeArray[i]);
                            }
                            else if (nodeArray[i].TreeNodeType == eTreeNodeType.ObjectNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.ChildObjectShortcutNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.ObjectShortcutNode)
                            {
                                itemList.Add(nodeArray[i], nodeArray[i]);
                            }
                        }

                        dictEnum = folderList.GetEnumerator();

                        while (dictEnum.MoveNext())
                        {
                            aParentNode.Nodes.Add((MIDTreeNode)dictEnum.Value);
                        }

                        dictEnum = itemList.GetEnumerator();

                        while (dictEnum.MoveNext())
                        {
                            aParentNode.Nodes.Add((MIDTreeNode)dictEnum.Value);
                        }
                    }

                    SelectedNode = (MIDTreeNode)selectedNode;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    //EndUpdate();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#176 - JSmith - New database has errors
        public FolderProfile Folder_Get(int aUserRID, eProfileType aFolderType, string aDefaultText)
        {
            bool newFolder = false;

            try
            {
                return Folder_Get(aUserRID, aFolderType, aDefaultText, ref newFolder);
            }
            catch
            {
                throw;
            }
        }
        // End TT#176

        /// <summary>
        /// Retrieves a FolderProfile object for the user and profile type.
        /// </summary>
        /// <param name="aUserRID">The key of the user</param>
        /// <param name="aFolderType">The type of folder</param>
        /// <param name="aDefaultText">The text to use to create the node if not found</param>
        /// <returns></returns>

        // Begin TT#176 - JSmith - New database has errors
        //public FolderProfile Folder_Get(int aUserRID, eProfileType aFolderType, string aDefaultText)
        public FolderProfile Folder_Get(int aUserRID, eProfileType aFolderType, string aDefaultText, ref bool aNewFolder)
        // End TT#176
        {
            DataTable dtFolders;
            FolderProfile folderProf;
            int key;

            try
            {
                dtFolders = DlFolder.Folder_Read(aUserRID, aFolderType);
                if (dtFolders == null || dtFolders.Rows.Count == 0)
                {
                    key = Folder_Create(aUserRID, aDefaultText, aFolderType);
                    folderProf = new FolderProfile(key, aUserRID, aFolderType, aDefaultText, aUserRID);
                    // Begin TT#176 - JSmith - New database has errors
                    aNewFolder = true;
                    // End TT#176
                }
                else
                {
                    folderProf = new FolderProfile(dtFolders.Rows[0]);
                    // Begin TT#176 - JSmith - New database has errors
                    aNewFolder = false;
                    // End TT#176
                }
            }
            catch
            {
                throw;
            }

            return folderProf;
        }

        /// <summary>
        /// Creates a folder for a user
        /// </summary>
        /// <param name="aUserRID">The key of the user</param>
        /// <param name="aText">The text for the folder</param>
        /// <param name="aFolderType">The type of the folder</param>
        /// <returns>The key of the new folder</returns>

        protected int Folder_Create(int aUserRID, string aText, eProfileType aFolderType)
        {
            int key;
            try
            {
                DlFolder.OpenUpdateConnection();

                try
                {
                    key = DlFolder.Folder_Create(aUserRID, aText, aFolderType);
                    DlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    DlFolder.CloseUpdateConnection();
                }

                return key;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of folder for a list of users and profile type
        /// </summary>
        /// <param name="aUserRIDList">A list of user keys</param>
        /// <param name="aFolderType">The type of folder</param>
        /// <returns></returns>

        protected DataTable Folder_Get(ArrayList aUserRIDList, eProfileType aFolderType)
        {
            try
            {
                return DlFolder.Folder_Read(aUserRIDList, aFolderType, true, false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Check is children exist for the folder
        /// </summary>
        /// <param name="aUserRID">
        /// The key of the user for the folder
        /// </param>
        /// <param name="aKey">The key of the folder</param>
        /// <returns>A flag identifying if children exist for the folder</returns>

        public bool Folder_Children_Exists(int aUserRID, int aKey)
        {
            try
            {
                return DlFolder.Folder_Children_Exists(aUserRID, aKey);
            }
            catch
            {
                throw;
            }
        }

        public MyHierarchyNode BuildNode(int aHierarchyRID, eHierarchyType aHierarchyType, MyHierarchyNode aParentNode, HierarchyNodeProfile aHierarchyNodeProfile, bool aParentAccessDenied)
        {
            MyHierarchyNode newNode;
            string sCollapsedImage;
            string sExpandedImage;
            eTreeNodeType treeNodeType;
            int userRID;

            try
            {
                sCollapsedImage = aHierarchyNodeProfile.NodeColor + MyGraphics.ClosedFolder;
                sExpandedImage = aHierarchyNodeProfile.NodeColor + MyGraphics.OpenFolder;

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

                newNode = new MyHierarchyNode(
                            SAB,
                            treeNodeType,
                            aHierarchyNodeProfile,
                            aHierarchyNodeProfile.Text,
                            aHierarchyNodeProfile.Key,
                            userRID,
                            null,
                            sCollapsedImage,
                            sCollapsedImage,
                            sExpandedImage,
                            sExpandedImage,
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

                //if (newNode.SecurityProfileIsInitialized &&
                //    newNode.HierarchyNodeSecurityProfile.AccessDenied)
                //{
                //    newNode.ForeColor = SystemColors.InactiveCaption;
                //}

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

        public MyHierarchyNode BuildPlaceHolderNode()
        {
            MyHierarchyNode newNode;

            try
            {
                newNode = new MyHierarchyNode(
                            SAB,
                            eTreeNodeType.ObjectNode,
                            new HierarchyNodeProfile(Include.NoRID),
                            null,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID,
                            new FunctionSecurityProfile(Include.NoRID),
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            SAB.ClientServerSession.UserRID);


                return newNode;
            }
            catch
            {
                throw;
            }
        }
    }

    public class MyGraphics
    {
        public const string Blank = "Blank";
        public const string AllocationSummaryImage = "AllocationSummary.gif";	// TT#707 - separate summary image from assortment
        public const string ApplicationIcon = "MIDRetail.ico";
        public const string ApplicationImage = "MIDRetail.bmp";
        public const string AssortmentImage = "Assortment.gif";
        public const string CalendarImage = "Calendar.gif";
        public const string ClosedFolder = ".closedfolder.gif";
        public const string CopyImage = "Copy.GIF";
        public const string CopyIcon = "Copy.ico";
        public const string CollapseImage = "Collapse.GIF";
        public const string CutImage = "Cut.GIF";
        public const string DeleteImage = "Delete.gif";
        public const string DeleteIcon = "Delete.ico";
        public const string ExpandImage = "Expand.GIF";
        public const string FindImage = "FIND.GIF";
        public const string UndoImage = "Undo.GIF";
        public const string ThemeImage = "Palette.ico";
        public const string NavigateImage = "compass.ico";
        public const string FirstImage = "first.ico";
        public const string PreviousImage = "left.ico";
        public const string NextImage = "right.ico";
        public const string LastImage = "last.ico";
        public const string ReplaceImage = "Replace.GIF";
        public const string DefaultClosedFolder = "default.closedfolder.gif";
        public const string DefaultOpenFolder = "default.openfolder.gif";
        public const string DownArrow = "DownArrow.gif";
        public const string ErrorImage = "Error16.ico";
        public const string WarningImage = "Warning16.ico";
        public const string Folder = "*.*folder.gif";
        public const string ForecastImage = "Forecast.gif";
        public const string GlobalImage = "globe.ico";
        public const string HierarchyImage = "Hierarchy.gif";
        public const string HierarchyExplorerImage = "HierarchyExplorer.gif";
        public const string InheritanceImage = "Inheritance.ico";
        public const string InsertImage = "Insert.bmp";
        public const string InsertBeforeImage = "InsertBefore.bmp";
        public const string InsertAfterImage = "InsertAfter.bmp";
        public const string MagnifyingGlassImage = "MagnifyingGlass.gif";
        public const string MethodsImage = "Methods.GIF";
        public const string NewImage = "New.GIF";
        public const string OpenImage = "Open.GIF";
        public const string PasteImage = "Paste.GIF";
        public const string OpenFolder = ".openfolder.gif";
        public const string ReoccurringImage = "dynamic.gif";
        public const string DynamicToPlanImage = "DynamicToPlan.gif";
        public const string DynamicToCurrentImage = "DynamicToCurrent.gif";
        public const string SaveImage = "Save.GIF";
        public const string SaveIcon = "Save.ico";
        public const string SchedulerImage = "Scheduler.GIF";
        public const string SecurityImage = "Security.gif";
        public const string SizeImage = "Size.gif";
        public const string StoreImage = "store.gif";
        public const string StoreProfileImage = "StoreProfile.GIF";
        public const string TaskListImage = "TaskList.GIF";
        public const string UpArrow = "UpArrow.gif";
        public const string RightSelectArrow = "RightSelectArrow.GIF";
        public const string WorkspaceImage = "Workspace.GIF";
        public const string StyleViewImage = "StyleView.gif";
        public const string FilterExplorerImage = "FilterExplorer.gif";
        public const string ClosedTreeFolder = "ClosedTreeFolder.gif";
        public const string OpenTreeFolder = "OpenTreeFolder.gif";
        public const string FilterSelected = "Filter_selected.ico";
        public const string FilterUnselected = "Filter_unselected.ico";
        public const string BalanceImage = "balance.ico";
        public const string LockImage = "lock.gif";
        public const string DynamicSwitchImage = "DynamicSwitch.gif";
        public const string TaskListSelected = "TaskList_selected.gif";
        public const string TaskListUnselected = "TaskList_unselected.gif";
        public const string JobSelected = "Job_selected.gif";
        public const string JobUnselected = "Job_unselected.gif";
        public const string SpecialRequestSelected = "SpecReq_selected.gif";
        public const string SpecialRequestUnselected = "SpecReq_unselected.gif";
        public const string NotesImage = "Notes.ico";
        public const string FavoritesImage = "Favorites.ico";
        public const string AddToFavoritesImage = "AddToFavorites.ico";
        public const string UserImage = "User.gif";
        public const string StoreDynamic = "store_dynamic.gif";
        public const string StoreStatic = "store_static.gif";
        public const string StoreSelected = "store_selected.gif";
        public const string ClosedFolderDynamic = "yellow.closedfolderdynamic.GIF";
        public const string OpenedFolderDynamic = "yellow.openfolderdynamic.GIF";
        public const string SecUserImage = "User.gif";
        public const string SecGroupImage = "Group.bmp";
        public const string SecClosedFolderImage = "CLSDFOLD.ICO";
        public const string SecOpenFolderImage = "OPENFOLD.ICO";
        public const string SecNoAccessImage = "TRFFC10C.ico";
        public const string SecReadOnlyAccessImage = "TRFFC10B.ico";
        public const string SecFullAccessImage = "TRFFC10A.ico";
        public const string ExcelImage = "Excel.ico";   // TT#1135 - AGallagher - Export headers from allocation workspace
        public const string Windows7CertImage = "windows7_certified.png";   // TT#1183 - JSmith - Windows 7 Logo
        public const string Logility = "Logility2.bmp";   // TT#???? - AGallagher - Logility - Rebranding #1
        public const string msgOKImage = "msg_ok.png";
        public const string msgWarningImage = "msg_warning.png";
        public const string msgEditImage = "msg_edit.png";
        public const string msgErrorImage = "msg_error.png";
        public const string msgSevereImage = "msg_severe.png";
        public const string ChartImage = "chart_pie.png";
        public const string UserActivityImage = "app_view_list.png";
        public const string msgDebugImage = "msg_debug.png"; //TT#595-MD -jsobek -Add Debug to My Activity level 
        public const string targetImage = "target.png"; //TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek
        public const string ToolBarApplyImage = "ToolBarApply.png";
        public const string ToolBarEmailImage = "ToolBarEmail.png";
        public const string ToolBarExportImage = "ToolBarExport.png";
        public const string ToolBarGridImage = "ToolBarGrid.png";
        public const string ToolBarGroupByImage = "ToolBarGroupBy.png";
        public const string ToolBarProcessImage = "ToolBarProcess.png";
        public const string ToolBarSaveViewImage = "ToolBarSaveView.png";
        public const string ToolBarExpandImage = "ToolBarExpand.png";
        public const string ToolBarCollapseImage = "ToolBarCollapse.png";
    }

    /// <summary>
    /// Used as a node in the treeview for the Merchandise Explorer
    /// </summary>
    public class MyHierarchyNode : MIDTreeNode
    {
        //  If new fields are added, the clone method below must also be changed
        public HierarchyNodeSecurityProfile _hierarchyNodeSecurityProfile;
        private bool _isHierarchyRoot;
        private bool _useSharedIcon;
        private int _hierarchyRID;
        private eHierarchyType _hierarchyType;
        private string _sCollapsedImage;
        private string _sSelectedCollapsedImage;
        private string _sExpandedImage;
        private string _sSelectedExpandedImage;
        private eIconType _iconType;
        private eIconColor _iconColor;
        private string _levelName;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public MyHierarchyNode()
            : base()
        {
            CommonLoad();
        }

        public MyHierarchyNode(
            SessionAddressBlock aSAB,
            eTreeNodeType aTreeNodeType,
            Profile aProfile,
            string aText,
            int aParentId,
            int aUserId,
            FunctionSecurityProfile aFunctionSecurityProfile,
            string sCollapsedImage,
            string sSelectedCollapsedImage,
            string sExpandedImage,
            string sSelectedExpandedImage,
            int aOwnerUserRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, true, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
            : base(aSAB, aTreeNodeType, aProfile, new MIDTreeNodeSecurityGroup(aFunctionSecurityProfile, null), true, aText, aParentId, aUserId, 0, 0, 0, 0, aOwnerUserRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            _sCollapsedImage = sCollapsedImage;
            _sSelectedCollapsedImage = sSelectedCollapsedImage;
            _sExpandedImage = sExpandedImage;
            _sSelectedExpandedImage = sSelectedExpandedImage;
            CommonLoad();
        }

        public MyHierarchyNode(
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
            _useSharedIcon = false;
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

        public bool UseSharedIcon
        {
            get { return _useSharedIcon; }
            set { _useSharedIcon = value; }
        }

        public string sCollapsedImage { get { return _sCollapsedImage; } }
        public string sSelectedCollapsedImage { get { return _sSelectedCollapsedImage; } }
        public string sExpandedImage { get { return _sExpandedImage; } }
        public string sSelectedExpandedImage { get { return _sSelectedExpandedImage; } }
        public string LevelName { get { return _levelName; } set { _levelName = value; } }
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
                    //    if (((MyHierarchyNode)this).isHierarchyNode)
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
                    //    else if (((MyHierarchyNode)this).isFavoritesSubFolder)
                    //    {
                    //        if (_functionSecurityProfile == null)
                    //        {
                    //            _functionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
                    //            _functionSecurityProfile.SetFullControl();
                    //        }
                    //    }
                    //    else if (((MyHierarchyNode)this).isMainFavoritesFolder ||
                    //        ((MyHierarchyNode)this).isMainAlternatesFolder ||
                    //        ((MyHierarchyNode)this).isMainOrganizationalFolder)
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
                        if (((MyHierarchyNode)this).isHierarchyNode)
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
                        else if (((MyHierarchyNode)this).isFavoritesSubFolder)
                        {
                            if (_nodeSecurityGroup.FunctionSecurityProfile == null)
                            {
                                _nodeSecurityGroup.FunctionSecurityProfile = new FunctionSecurityProfile(Profile.Key);
                                _nodeSecurityGroup.FunctionSecurityProfile.SetFullControl();
                            }
                        }
                        else if (((MyHierarchyNode)this).isMainFavoritesFolder ||
                            ((MyHierarchyNode)this).isMainAlternatesFolder ||
                            ((MyHierarchyNode)this).isMainOrganizationalFolder)
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
                        if (((MyHierarchyNode)this).isHierarchyNode &&
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
                        else if (((MyHierarchyNode)this).isMainFavoritesFolder ||
                            ((MyHierarchyNode)this).isMainAlternatesFolder ||
                            ((MyHierarchyNode)this).isMainOrganizationalFolder ||
                            ((MyHierarchyNode)this).isMainUserFolder)
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

        public MyHierarchyNode GetHierarchyRootNode()
        {
            try
            {
                MyHierarchyNode node;

                node = this;

                while (node.Parent != null)
                {
                    node = (MyHierarchyNode)node.Parent;
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
            MyHierarchyNode mtn = (MyHierarchyNode)base.Clone();
            mtn.NodeChangeType = this.NodeChangeType;
            mtn.HierarchyNodeSecurityProfile = this._hierarchyNodeSecurityProfile;
            mtn.isHierarchyRoot = this._isHierarchyRoot;
            mtn.UseSharedIcon = this._useSharedIcon;
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
            MyHierarchyNode mtn = (MyHierarchyNode)base.CloneNode(aProfile);
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

                if (((MyHierarchyNode)aDestinationNode).isMainOrganizationalFolder ||
                    ((MyHierarchyNode)aDestinationNode).isMainAlternatesFolder)
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
                    ((MyHierarchyNode)aDestinationNode).HierarchyType == eHierarchyType.organizational)
                {
                    return false;
                }
                // End TT#18

                //Begin Track #6456 - JSmith - Users have ability to move nodes in alternate hierarchy despite security settings
                //if (((MyHierarchyNode)aDestinationNode).FunctionSecurityProfile == null ||
                //    (_hierarchyType == eHierarchyType.organizational &&
                //    ((MyHierarchyNode)aDestinationNode).HierarchyType == eHierarchyType.organizational &&
                //    !aDestinationNode.FunctionSecurityProfile.AllowMove))
                //{
                //    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized);
                //    return false;
                //}
                //Begin TT#1744 - JSmith - Security for the Alternate Hierarchy
                //if (((MyHierarchyNode)aDestinationNode).FunctionSecurityProfile == null ||
                //    !aDestinationNode.FunctionSecurityProfile.AllowMove)
                //{
                //    //Begin TT#474 - JScott - Create My Hier w dash - errors
                //    //Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized);
                //    Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized, false);
                //    //End TT#474 - JScott - Create My Hier w dash - errors
                //    return false;
                //}
                ////End Track #6456
                if (((MyHierarchyNode)aDestinationNode).FunctionSecurityProfile == null)
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

                if (((MyHierarchyNode)aDestinationNode).isFavoritesFolder)
                {
                    return isFolderDropAllowed(aDropAction, (MyHierarchyNode)aDestinationNode);
                }
                else
                {
                    return isNodeDropAllowed(aDropAction, (MyHierarchyNode)aDestinationNode);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool isNodeDropAllowed(DragDropEffects aDropAction, MyHierarchyNode aDestinationNode)
        {
            eHierarchyType selectedNodeHierarchyType;
            eHierarchyType dropNodeHierarchyType;
            MyHierarchyNode selectedNodeParent;
            MyHierarchyNode selectedNodeFolder;
            MyHierarchyNode dropNodeFolder;
            HierarchyProfile selectedHierarchy;
            HierarchyProfile dropHierarchy;
            HierarchyLevelProfile selectedHlp;
            HierarchyLevelProfile dropHlp;
            HierarchyNodeProfile hnp;
            bool circularRelationship;
            MyHierarchyNode dropNodePath;
            bool nodeFound;
            bool validLevels;
            int fromLevel;
            int toLevel;

            try
            {
                if (((MyHierarchyNode)this).isFavoritesFolder)
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

                if (Parent != null && Parent is MyHierarchyNode)
                {
                    selectedNodeParent = (MyHierarchyNode)Parent;
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
                    selectedNodeFolder = (MyHierarchyNode)this;
                }
                else
                {
                    selectedNodeFolder = (MyHierarchyNode)Parent;

                    while (selectedNodeFolder.Parent != null)
                    {
                        selectedNodeFolder = (MyHierarchyNode)selectedNodeFolder.Parent;
                    }

                    selectedNodeHierarchyType = SAB.HierarchyServerSession.GetHierarchyType(HierarchyRID);
                }

                // get folder for node where the node is being copied to
                if (aDestinationNode.Parent == null)
                {
                    dropNodeFolder = (MyHierarchyNode)aDestinationNode;
                }
                else
                {
                    dropNodeFolder = (MyHierarchyNode)aDestinationNode.Parent;

                    while (dropNodeFolder.Parent != null)
                    {
                        dropNodeFolder = (MyHierarchyNode)dropNodeFolder.Parent;
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
                            dropNodePath = (MyHierarchyNode)dropNodePath.Parent;
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
                    //foreach (MyHierarchyNode mtn in aDestinationNode.Nodes)
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
                foreach (MyHierarchyNode mtn in aDestinationNode.Nodes)
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

        private bool isFolderDropAllowed(DragDropEffects aDropAction, MyHierarchyNode aDestinationNode)
        {
            MyHierarchyNode dropNodeFolder = null;
            eProfileType folderChildType;
            MyHierarchyNode selectedNodeParent;

            try
            {
                // can't drop on itself
                if (this == aDestinationNode)
                {
                    return false;
                }

                // can't drop on descendant
                dropNodeFolder = (MyHierarchyNode)aDestinationNode;
                while (dropNodeFolder != null)
                {
                    if (this == dropNodeFolder)
                    {
                        return false;
                    }
                    if (dropNodeFolder.Parent != null)
                    {
                        dropNodeFolder = (MyHierarchyNode)dropNodeFolder.Parent;
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

                if (Parent != null && Parent is MyHierarchyNode)
                {
                    selectedNodeParent = (MyHierarchyNode)Parent;
                }

                DropAction = aDropAction;

                if (isFavoritesFolder
                    && aDestinationNode.isFavoritesFolder)
                {
                    DropAction = DragDropEffects.Move;
                }
                else if (selectedNodeParent != null &&
                    ((MyHierarchyNode)Parent).isFavoritesFolder
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
            //HierarchyNodeProfile hnp;
            //HierarchyProfile hp;

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
                        if (profile is MyHierarchyNode)
                        {
                            Get_Folder_Descendants(aChildProfile, ((MyHierarchyNode)profile).Profile, aDescendantList);
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

                HierarchyProfile hp = SAB.HierarchyServerSession.GetMainHierarchyData();
                foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                {
                    MyHierarchyNode mtn = BuildHierarchyNode(HierarchyRID, HierarchyType, hnp, parentAccessDenied);
                    int LevelIndex = hnp.HomeHierarchyLevel;
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[LevelIndex];
                    mtn.LevelName = hlp.LevelID;
                    if (DebugActivated)
                    {
                        //Begin Track #6201 - JScott - Store Count removed from attr sets
                        //mtn.Text += " : #" + mtn.Profile.Key.ToString();
                        mtn.InternalText += " : #" + mtn.Profile.Key.ToString() + LevelName;
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
            MyHierarchyNode newNode = null;

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
            MyHierarchyNode newNode = null;
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
                    if (DlFolder.Folder_Children_Exists(SAB.ClientServerSession.UserRID, newNode.Profile.Key))
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

        private MyHierarchyNode BuildHierarchyNode(int aHierarchyRID, eHierarchyType aHierarchyType, HierarchyNodeProfile aHierarchyNodeProfile,
           bool aParentAccessDenied)
        {
            HierarchyNodeProfile hnp;
            string sCollapsedImage, sExpandedImageIndex;
            eTreeNodeType treeNodeType;
            int userRID;

            try
            {

                hnp = aHierarchyNodeProfile;

                sCollapsedImage = hnp.NodeColor + MyGraphics.ClosedFolder;
                sExpandedImageIndex = hnp.NodeColor + MyGraphics.OpenFolder;

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

                MyHierarchyNode mtn = new MyHierarchyNode(
                    SAB,
                    treeNodeType,
                    hnp,
                    hnp.Text,
                    hnp.HomeHierarchyParentRID,
                    userRID,
                    null,
                    sCollapsedImage,
                    sCollapsedImage,
                    sExpandedImageIndex,
                    sExpandedImageIndex,
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
                        mtn.ForeColor = System.Drawing.SystemColors.InactiveCaption;
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

        public MyHierarchyNode BuildFolderNode(FolderProfile aFolderProf, bool aParentAccessDenied)
        {
            string sCollapsedImage, sExpandedImage;
            eTreeNodeType treeNodeType;
            MyHierarchyNode newNode;

            try
            {
                sCollapsedImage = Include.MIDDefaultColor + MyGraphics.ClosedFolder;
                sExpandedImage = Include.MIDDefaultColor + MyGraphics.OpenFolder;

                treeNodeType = eTreeNodeType.SubFolderNode;

                newNode = new MyHierarchyNode(
                    SAB,
                    treeNodeType,
                    aFolderProf,
                    aFolderProf.Name,
                    Include.NoRID,
                    aFolderProf.UserRID,
                    null,
                    sCollapsedImage,
                    sCollapsedImage,
                    sExpandedImage,
                    sExpandedImage,
                    aFolderProf.OwnerUserRID
                    );

                newNode.HierarchyNodeSecurityProfile = new HierarchyNodeSecurityProfile(newNode.Profile.Key);
                newNode.HierarchyNodeSecurityProfile.SetFullControl();
                if (DlFolder.Folder_Children_Exists(SAB.ClientServerSession.UserRID, newNode.Profile.Key))
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
    }
}
