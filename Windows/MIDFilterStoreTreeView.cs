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
    public class FilterStoreTreeView : MIDTreeView
    {
        //=======
        // FIELDS
        //=======

        private int cFavoritesImage;
        private int cClosedFolderImage;
        private int cOpenFolderImage;
        private int cClosedShortcutFolderImage;
        private int cOpenShortcutFolderImage;
        private int cUserFilterSelectedImage;
        private int cUserFilterUnselectedImage;
        private int cUserFilterShortcutSelectedImage;
        private int cUserFilterShortcutUnselectedImage;
        private int cGlobalFilterSelectedImage;
        private int cGlobalFilterUnselectedImage;
        private int cGlobalFilterShortcutSelectedImage;
        private int cGlobalFilterShortcutUnselectedImage;
        private int cSharedClosedFolderImage;
        private int cSharedOpenFolderImage;
        private int cSharedUserFilterSelectedImage;
        private int cSharedUserFilterUnselectedImage;

        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
        //private Hashtable _secLvlHash;
        private Hashtable _secGrpHash;
        //End Track #6321 - JScott - User has ability to to create folders when security is view

        //private StoreFilterData _dlFilters;
        private FilterData _dlFilters;
        private FolderDataLayer _dlFolder;

        private MIDFilterNode _userNode;
        private MIDFilterNode _globalNode;
        private MIDFilterNode _sharedNode;

        public ExplorerAddressBlock EAB = null;

        private filterTypes treeFilterType = filterTypes.StoreFilter;

        //=============
        // CONSTRUCTORS
        //=============

        public FilterStoreTreeView()
        {

        }

        //===========
        // PROPERTIES
        //===========

        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
        private MIDTreeNodeSecurityGroup UserSecGrp
        {
            get
            {
                return (MIDTreeNodeSecurityGroup)_secGrpHash[SAB.ClientServerSession.UserRID];
            }
        }

        private MIDTreeNodeSecurityGroup GlobalSecGrp
        {
            get
            {
                return (MIDTreeNodeSecurityGroup)_secGrpHash[Include.GlobalUserRID];
            }
        }

        //End Track #6321 - JScott - User has ability to to create folders when security is view
        private FunctionSecurityProfile UserSecLvl
        {
            get
            {
                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //return (FunctionSecurityProfile)_secLvlHash[SAB.ClientServerSession.UserRID];
                return UserSecGrp.FunctionSecurityProfile;
                //End Track #6321 - JScott - User has ability to to create folders when security is view
            }
        }

        private FunctionSecurityProfile GlobalSecLvl
        {
            get
            {
                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //return (FunctionSecurityProfile)_secLvlHash[Include.GlobalUserRID];
                // Begin TT #73 - JSmith - Global Filters not appearing
                //return UserSecGrp.FolderSecurityProfile;
                return GlobalSecGrp.FunctionSecurityProfile;
                // End TT #73
                //End Track #6321 - JScott - User has ability to to create folders when security is view
            }
        }

        public MIDFilterNode UserNode
        {
            get
            {
                return _userNode;
            }
        }
        public MIDFilterNode GlobalNode
        {
            get
            {
                return _globalNode;
            }
        }

        //========
        // METHODS
        //========

        //----------------------------------------------
        //OVERRIDES TO VIRTUAL METHODS IN THE BASE CLASS
        //----------------------------------------------

        /// <summary>
        /// Virtual method that initializes the MIDTreeView
        /// </summary>
        /// <param name="aSAB">
        /// The SessionAddressBlock
        /// </param>
        /// <param name="aAllowMultiSelect">
        /// A boolean indicating if multi-select is allowed
        /// </param>
        /// <param name="aMDIParentForm">
        /// The MDI parent form
        /// </param>

        override public void InitializeTreeView(SessionAddressBlock aSAB, bool aAllowMultiSelect, Form aMDIParentForm)
        {
            try
            {
                base.InitializeTreeView(aSAB, aAllowMultiSelect, aMDIParentForm);

                cFavoritesImage = MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage);
                cClosedFolderImage = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
                cOpenFolderImage = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);
                cClosedShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.ClosedTreeFolder);
                cOpenShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.OpenTreeFolder);
                cUserFilterSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.UserImage);
                cUserFilterUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.UserImage);
                cUserFilterShortcutSelectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.UserImage);
                cUserFilterShortcutUnselectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.UserImage);
                cGlobalFilterSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                cGlobalFilterUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                cGlobalFilterShortcutSelectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.GlobalImage);
                cGlobalFilterShortcutUnselectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.GlobalImage);
                cSharedClosedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.ClosedTreeFolder);
                cSharedOpenFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.OpenTreeFolder);
                cSharedUserFilterSelectedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);
                cSharedUserFilterUnselectedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);

                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //_secLvlHash = new Hashtable();
                //_secLvlHash[SAB.ClientServerSession.UserRID] = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersUser);
                //_secLvlHash[Include.GlobalUserRID] = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersGlobal);
                _secGrpHash = new Hashtable();

                _secGrpHash[SAB.ClientServerSession.UserRID] = new MIDTreeNodeSecurityGroup(
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser),
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersFilterStoreFoldersUser));

                _secGrpHash[Include.GlobalUserRID] = new MIDTreeNodeSecurityGroup(
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal),
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersFilterStoreFoldersGlobal));
                //End Track #6321 - JScott - User has ability to to create folders when security is view

                _dlFilters = new FilterData();
                _dlFolder = new FolderDataLayer();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Virtual method that loads the nodes for the MIDTreeView
        /// </summary>

        override public void LoadNodes()
        {
            DataTable dtFolders;
            FolderProfile folderProf;

            ArrayList userRIDList;
            ArrayList folderTypeList;
            DataTable dtFilters = null;
            DataTable dtFolderItems = null;
            DataTable dtFolderShortcuts = null;
            DataTable dtFilterShortcuts = null;
            MIDFilterNode newNode;
            FolderShortcut newShortcut;
            MIDFilterNode parentNode;
            MIDFilterNode childNode;
            DataTable dtUserFolders;
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //FunctionSecurityProfile userSecLvl;
            MIDTreeNodeSecurityGroup userSecGrp;
            //End Track #6321 - JScott - User has ability to to create folders when security is view
            MIDFilterNode userNode;

            try
            {
                Nodes.Clear();

                //----------------------
                // Build Faviorites node
                //----------------------

                // Begin TT#107 - JSmith - No Favorites folder if user filters are denied.
                //if (!UserSecLvl.AccessDenied)
                //{
                // End TT#107
                dtFolders = _dlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.FilterStoreMainFavoritesFolder);

                //Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                //if (dtFolders == null || dtFolders.Rows.Count != 1)
                if (dtFolders == null || dtFolders.Rows.Count == 0)
                //End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                {
                    folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.FilterStoreMainFavoritesFolder, "My Favorites", SAB.ClientServerSession.UserRID);

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        folderProf.Key = _dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, folderProf.FolderType);
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
                }
                else
                {
                    folderProf = new FolderProfile(dtFolders.Rows[0]);
                }

                // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                //FavoritesNode = new MIDFilterNode(
                //    SAB,
                //    eTreeNodeType.MainFavoriteFolderNode,
                //    folderProf,
                //    folderProf.Name,
                //    Include.NoRID,
                //    folderProf.UserRID,
                //    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //    //UserSecLvl,
                //    UserSecGrp,
                //    //End Track #6321 - JScott - User has ability to to create folders when security is view
                //    cFavoritesImage,
                //    cFavoritesImage,
                //    cFavoritesImage,
                //    cFavoritesImage,
                //    folderProf.OwnerUserRID);
                FavoritesNode = new MIDFilterNode(treeFilterType,
                    SAB,
                    eTreeNodeType.MainFavoriteFolderNode,
                    folderProf,
                    folderProf.Name,
                    Include.NoRID,
                    folderProf.UserRID,
                    // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                    //FavoritesSecGrp,
                    FavoritesFolderSecGrp,
                    // End TT#373
                    cFavoritesImage,
                    cFavoritesImage,
                    cFavoritesImage,
                    cFavoritesImage,
                    folderProf.OwnerUserRID);
                // Begin TT#42

                FolderNodeHash[folderProf.Key] = FavoritesNode;

                Nodes.Add(FavoritesNode);
                // Begin TT#107 - JSmith - No Favorites folder if user filters are denied.
                //}
                // End TT#107

                //----------------
                // Build User node
                //----------------

                if (!UserSecLvl.AccessDenied)
                {
                    dtFolders = _dlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.FilterStoreMainUserFolder);

                    //Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                    //if (dtFolders == null || dtFolders.Rows.Count != 1)
                    if (dtFolders == null || dtFolders.Rows.Count == 0)
                    //End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                    {
                        folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.FilterStoreMainUserFolder, "My Filters", SAB.ClientServerSession.UserRID);

                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            folderProf.Key = _dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, folderProf.FolderType);
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
                    }
                    else
                    {
                        folderProf = new FolderProfile(dtFolders.Rows[0]);
                    }

                    _userNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //UserSecLvl,
                        UserSecGrp,
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        folderProf.OwnerUserRID);

                    FolderNodeHash[folderProf.Key] = _userNode;

                    Nodes.Add(_userNode);
                }

                //------------------
                // Build Global node
                //------------------

                if (!GlobalSecLvl.AccessDenied)
                {
                    dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.FilterStoreMainGlobalFolder);

                    if (dtFolders == null)
                    {
                        throw new Exception("Global Filter Folder not defined");
                    }
                    else if (dtFolders.Rows.Count != 1)
                    {
                        throw new Exception("More than one Global Filter Folder is defined");
                    }

                    folderProf = new FolderProfile(dtFolders.Rows[0]);

                    _globalNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //GlobalSecLvl,
                        GlobalSecGrp,
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        folderProf.OwnerUserRID);

                    FolderNodeHash[folderProf.Key] = _globalNode;

                    Nodes.Add(_globalNode);
                }

                //---------------------------
                // Read and Load Detail Nodes
                //---------------------------

                userRIDList = new ArrayList();

                if (!UserSecLvl.AccessDenied)
                {
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                if (!GlobalSecLvl.AccessDenied)
                {
                    userRIDList.Add(Include.GlobalUserRID);
                }

                folderTypeList = new ArrayList();
                folderTypeList.Add((int)eProfileType.FilterStoreMainFavoritesFolder);
                folderTypeList.Add((int)eProfileType.FilterStoreMainUserFolder);
                folderTypeList.Add((int)eProfileType.FilterStoreMainGlobalFolder);
                folderTypeList.Add((int)eProfileType.FilterStoreSubFolder);

                if (userRIDList.Count > 0)
                {
                    dtFilters = _dlFilters.FilterReadParent(treeFilterType, eProfileType.FilterStore, userRIDList);
                    dtFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.FilterStoreSubFolder, true, false);
                    dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.FilterStore, true, false);
                    dtFilterShortcuts = _dlFolder.Folder_Shortcut_Item_Read(userRIDList, eProfileType.FilterStore);
                    dtFolderShortcuts = _dlFolder.Folder_Shortcut_Folder_Read(userRIDList, folderTypeList);

                    dtFilters.PrimaryKey = new DataColumn[] { dtFilters.Columns["FILTER_RID"] };
                    dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

                    if (!UserSecLvl.AccessDenied)
                    {
                        BuildFolderBranch(SAB.ClientServerSession.UserRID, FavoritesNode.Profile.Key, FavoritesNode, dtFolderItems, dtFolders, dtFilters);
                        BuildFolderBranch(SAB.ClientServerSession.UserRID, _userNode.Profile.Key, _userNode, dtFolderItems, dtFolders, dtFilters);
                    }

                    if (!GlobalSecLvl.AccessDenied)
                    {
                        BuildFolderBranch(Include.GlobalUserRID, _globalNode.Profile.Key, _globalNode, dtFolderItems, dtFolders, dtFilters);  // Issue 3806
                    }

                    foreach (DataRow row in dtFilterShortcuts.Rows)
                    {
                        newShortcut = new FolderShortcut(row);

                        parentNode = (MIDFilterNode)FolderNodeHash[newShortcut.ParentFolderId];
                        childNode = (MIDFilterNode)ItemNodeHash[newShortcut.ShortcutId];

                        // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                        if (parentNode == null ||
                            childNode == null)
                        {
                            continue;
                        }
                        // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view

                        newNode = BuildObjectShortcutNode(childNode, parentNode);
                    }

                    foreach (DataRow row in dtFolderShortcuts.Rows)
                    {
                        newShortcut = new FolderShortcut(row);

                        parentNode = (MIDFilterNode)FolderNodeHash[newShortcut.ParentFolderId];
                        childNode = (MIDFilterNode)FolderNodeHash[newShortcut.ShortcutId];

                        // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                        if (parentNode == null ||
                            childNode == null)
                        {
                            continue;
                        }
                        // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view

                        newNode = BuildRootShortcutNode(childNode, parentNode);
                    }

                    if (FavoritesNode != null)
                    {
                        SortChildNodes(FavoritesNode);
                    }

                    if (_userNode != null)
                    {
                        SortChildNodes(_userNode);
                    }

                    if (_globalNode != null)
                    {
                        SortChildNodes(_globalNode);
                    }
                }

                //--------------------------------
                // Read and Load Shared User Nodes
                //--------------------------------

                userRIDList.Clear();
                userRIDList.Add(SAB.ClientServerSession.UserRID);

                dtUserFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.FilterStoreMainUserFolder, false, true);

                if (dtUserFolders.Rows.Count > 0)
                {
                    folderProf = new FolderProfile(
                        Include.NoRID,
                        SAB.ClientServerSession.UserRID,
                        eProfileType.FilterMainSharedFolder,
                        MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName),
                        SAB.ClientServerSession.UserRID);

                    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                    //userSecLvl = new FunctionSecurityProfile(-1);
                    //userSecLvl.SetAllowView();
                    userSecGrp = new MIDTreeNodeSecurityGroup(new FunctionSecurityProfile(-1), new FunctionSecurityProfile(-1));
                    userSecGrp.FunctionSecurityProfile.SetAllowView();
                    userSecGrp.FolderSecurityProfile.SetAllowView();
                    //End Track #6321 - JScott - User has ability to to create folders when security is view

                    _sharedNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //userSecLvl,
                        userSecGrp,
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cSharedClosedFolderImage,
                        cSharedClosedFolderImage,
                        cSharedOpenFolderImage,
                        cSharedOpenFolderImage,
                        folderProf.OwnerUserRID);

                    Nodes.Add(_sharedNode);

                    dtFilters = _dlFilters.FilterReadParent(treeFilterType, eProfileType.FilterStore, userRIDList);
                    dtFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.FilterStoreSubFolder, false, true);
                    dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.FilterStore, false, true);

                    dtFilters.PrimaryKey = new DataColumn[] { dtFilters.Columns["FILTER_RID"] };
                    dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

                    foreach (DataRow row in dtUserFolders.Rows)
                    {
                        folderProf = new FolderProfile(row);

                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        //folderProf.Name = DlSecurity.GetUserName(folderProf.OwnerUserRID);
                        folderProf.Name = UserNameStorage.GetUserName(folderProf.OwnerUserRID);
                        //End TT#827-MD -jsobek -Allocation Reviews Performance

                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //userSecLvl = (FunctionSecurityProfile)UserSecLvl.Clone();
                        //userSecLvl.SetDenyDelete();
                        //_secLvlHash[folderProf.OwnerUserRID] = userSecLvl;
                        userSecGrp = (MIDTreeNodeSecurityGroup)UserSecGrp.Clone();
                        userSecGrp.FunctionSecurityProfile.SetDenyDelete();
                        userSecGrp.FolderSecurityProfile.SetDenyDelete();
                        _secGrpHash[folderProf.OwnerUserRID] = userSecGrp;
                        //End Track #6321 - JScott - User has ability to to create folders when security is view

                        userNode = new MIDFilterNode(treeFilterType,
                            SAB,
                            eTreeNodeType.MainSourceFolderNode,
                            folderProf,
                            folderProf.Name,
                            Include.NoRID,
                            folderProf.UserRID,
                            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                            //userSecLvl,
                            userSecGrp,
                            //End Track #6321 - JScott - User has ability to to create folders when security is view
                            cClosedFolderImage,
                            cClosedFolderImage,
                            cOpenFolderImage,
                            cOpenFolderImage,
                            folderProf.OwnerUserRID);

                        FolderNodeHash[folderProf.Key] = userNode;

                        _sharedNode.Nodes.Add(userNode);

                        BuildFolderBranch(SAB.ClientServerSession.UserRID, userNode.Profile.Key, userNode, dtFolderItems, dtFolders, dtFilters);
                    }

                    SortChildNodes(_sharedNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
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
            //frmFilterProperties filterProperties;
            frmFilterBuilder frmFilter;
            OnFilterPropertiesCloseClass closeHandler;

            try
            {
                //Begin TT#1255 - JScott - Tasklist disappears after save and refresh
                if (aParentNode.TreeNodeType == eTreeNodeType.ObjectNode)
                {
                    aParentNode = aParentNode.GetParentNode();
                }

                //End TT#1255 - JScott - Tasklist disappears after save and refresh
                //filterProperties = new frmFilterProperties(SAB, (MIDFilterNode)aParentNode, _userNode, _globalNode, aParentNode.UserId, aParentNode.OwnerUserRID, false);

                //Determine default owner based on the node currently selected
                int defaultOwnerUserRID;
                // Begin TT#4568 - JSmith - Creating new user filter fails if user does not have authority for global filters
                //if (aParentNode == _globalNode || aParentNode.isChildOf(_globalNode))
                if (_globalNode != null && (aParentNode == _globalNode || aParentNode.isChildOf(_globalNode)))
                // End TT#4568 - JSmith - Creating new user filter fails if user does not have authority for global filters
                {
                    defaultOwnerUserRID = Include.GlobalUserRID;
                }
                else
                {
                    defaultOwnerUserRID = SAB.ClientServerSession.UserRID;
                }

                frmFilter = SharedRoutines.GetFilterFormForNewFilters(treeFilterType, SAB, EAB, defaultOwnerUserRID);




                closeHandler = new OnFilterPropertiesCloseClass(null);

                frmFilter.OnFilterPropertiesCloseHandler += new frmFilterBuilder.FilterPropertiesCloseEventHandler(closeHandler.OnClose);
                frmFilter.MdiParent = MDIParentForm;

                frmFilter.Show();
                frmFilter.BringToFront();
                //Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename

                return null;
                //End Track #6257 - JScott - Create New Attribute requires user to right-click rename
            }
            catch
            {
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
            MIDFilterNode newNode;
            // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
            MIDTreeNodeSecurityGroup nodeSecurityGroup;
            // End TT#373

            try
            {
                newNodeName = FindNewFolderName("New Folder", aUserId, aNode.Profile.Key, eProfileType.FilterStoreSubFolder);

                _dlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserId, eProfileType.FilterStoreSubFolder, newNodeName, aUserId);
                    newFolderProf.Key = _dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    _dlFolder.Folder_Item_Insert(aNode.Profile.Key, newFolderProf.Key, eProfileType.FilterStoreSubFolder);

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

                try
                {
                    // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                    if (aNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                    {
                        nodeSecurityGroup = FavoritesSecGrp;
                    }
                    else
                    {
                        nodeSecurityGroup = aNode.NodeSecurityGroup;
                    }
                    // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse

                    newNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.SubFolderNode,
                        newFolderProf,
                        newFolderProf.Name,
                        aNode.Profile.Key,
                        aUserId,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //aNode.FunctionSecurityProfile,
                        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                        //aNode.NodeSecurityGroup,
                        nodeSecurityGroup,
                        // Begin TT#373
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        aUserId);

                    _folderNodeHash[newFolderProf.Key] = newNode;
                    aNode.Nodes.Insert(0, newNode);
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

        /// <summary>
        /// Virtual method that is called after a label has been updated
        /// </summary>
        /// <returns>
        /// A boolean indicating if post-processing was successful
        /// </returns>

        override protected bool AfterLabelUpdate(MIDTreeNode aNode, string aNewName)
        {
            int key;
            StoreFilterProfile filterProf;
            FolderProfile folderProf;
            GenericEnqueue objEnqueue;

            try
            {
                switch (aNode.NodeProfileType)
                {
                    case eProfileType.FilterStore:

                        objEnqueue = EnqueueObject((StoreFilterProfile)aNode.Profile, false);

                        if (objEnqueue == null)
                        {
                            return false;
                        }

                        try
                        {
                            key = _dlFilters.FilterGetKey(treeFilterType, aNode.UserId, aNewName);

                            if (key != -1)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FilterNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            //_dlFilters.OpenUpdateConnection();

                            //try
                            //{
                            _dlFilters.UpdateFilterNameAndUser(aNode.Profile.Key, aNode.UserId, aNewName);
                            //	_dlFilters.CommitData();
                            //}
                            //catch (Exception exc)
                            //{
                            //    string message = exc.ToString();
                            //    throw;
                            //}
                            //finally
                            //{
                            //    _dlFilters.CloseUpdateConnection();
                            //}

                            filterProf = (StoreFilterProfile)aNode.Profile;
                            filterProf.Name = aNewName;
                        }
                        catch (Exception error)
                        {
                            string message = error.ToString();
                            throw;
                        }
                        finally
                        {
                            objEnqueue.DequeueGeneric();
                        }

                        break;

                    case eProfileType.FilterStoreMainFavoritesFolder:
                    case eProfileType.FilterStoreMainUserFolder:
                    case eProfileType.FilterStoreMainGlobalFolder:
                    case eProfileType.FilterStoreSubFolder:

                        key = _dlFolder.Folder_GetKey(aNode.UserId, aNewName, aNode.ParentId, aNode.NodeProfileType);

                        if (key != -1)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FolderNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

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

                        break;
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
            MIDFilterNode toNode;
            MIDFilterNode newNode = null;

            try
            {
                BeginUpdate();

                try
                {
                    switch (aToNode.NodeProfileType)
                    {
                        case eProfileType.FilterStore:

                            toNode = (MIDFilterNode)aToNode.Parent;
                            break;

                        default:

                            toNode = (MIDFilterNode)aToNode;
                            break;
                    }

                    if (_dlFolder.Folder_Shortcut_Exists(toNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType))
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Cursor.Current = Cursors.WaitCursor;

                    switch (aFromNode.NodeProfileType)
                    {
                        case eProfileType.FilterStoreSubFolder:

                            newNode = BuildRootShortcutNode((MIDFilterNode)aFromNode, toNode);

                            _dlFolder.OpenUpdateConnection();

                            try
                            {
                                _dlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, aFromNode.NodeProfileType);
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

                        case eProfileType.FilterStore:

                            newNode = BuildObjectShortcutNode((MIDFilterNode)aFromNode, toNode);

                            _dlFolder.OpenUpdateConnection();

                            try
                            {
                                _dlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, aFromNode.NodeProfileType);
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

                    SortChildNodes(toNode);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    EndUpdate();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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
            MIDFilterNode toNode;

            try
            {
                switch (aToNode.NodeProfileType)
                {
                    case eProfileType.FilterStore:

                        toNode = (MIDFilterNode)aToNode.Parent;
                        break;

                    default:

                        toNode = (MIDFilterNode)aToNode;
                        break;
                }

                try
                {
                    return MoveFilterNode((MIDFilterNode)aFromNode, toNode);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    EndUpdate();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
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
            MIDFilterNode toNode;

            try
            {
                switch (aToNode.NodeProfileType)
                {
                    case eProfileType.FilterStore:

                        toNode = (MIDFilterNode)aToNode.Parent;
                        break;

                    default:

                        toNode = (MIDFilterNode)aToNode;
                        break;
                }

                try
                {
                    return CopyFilterNode((MIDFilterNode)aFromNode, toNode, aFindUniqueName);
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
                    InUseFilterNode(aNode);
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
            // Begin TT#4285 - JSmith - Foreign Key Error When Delete Attribute Folder
            //return InUseDeleteNode(aDeleteList);
            return InUseDeleteNode(GetObjectNodes(aDeleteList));
            // End TT#4285 - JSmith - Foreign Key Error When Delete Attribute Folder
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
            try
            {
                aDeleteCancelled = false;  // TT#3630 - JSmith - Delete My Hierarchy
                if (aNode != null)
                {
                    DeleteFilterNode((MIDFilterNode)aNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
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
            MIDFilterNode node;
            StoreFilterProfile filterProf;
            //frmFilterProperties filterProperties;
            frmFilterBuilder filterProperties;
            OnFilterPropertiesCloseClass closeHandler;
            GenericEnqueue objEnqueue;

            try
            {
                if (aNode != null)
                {
                    if (aNode.Profile.ProfileType == eProfileType.FilterStore)
                    {
                        node = (MIDFilterNode)aNode;

                        filterProf = (StoreFilterProfile)node.Profile;
                        objEnqueue = EnqueueObject(filterProf, true);

                        if (objEnqueue != null)
                        {
                            //filterProperties = new frmFilterProperties(SAB, (MIDFilterNode)node.Parent, _userNode, _globalNode, (StoreFilterProfile)node.Profile, objEnqueue.IsInConflict);
                            int _initialUserRID = ((StoreFilterProfile)node.Profile).UserRID;
                            int _initialOwnerRID = ((StoreFilterProfile)node.Profile).OwnerUserRID;


                            bool isReadOnly = false;
                            bool isShared = false;
                            if (_sharedNode != null && node.isChildOf(_sharedNode))
                            {
                                isShared = true;
                            }
                            if (objEnqueue.IsInConflict || isShared)
                            {
                                isReadOnly = true;
                            }
                            // Begin TT#4559 - JSmith - Filter permissions
                            if (node.FunctionSecurityProfile.IsReadOnly)
                            {
                                isReadOnly = true;
                            }
                            // End TT#4559 - JSmith - Filter permissions

                            //filterProperties = new frmFilterBuilder(SAB, EAB, isReadOnly);  //TT#1313-MD -jsobek -Header Filters  

                            filterProperties = SharedRoutines.GetFilterFormForExistingFilter(filterProf.Key, SAB, EAB, isReadOnly, executeAfterEditing: false); //TT#1313-MD -jsobek -Header Filters  



                            closeHandler = new OnFilterPropertiesCloseClass(objEnqueue);

                            filterProperties.OnFilterPropertiesCloseHandler += new frmFilterBuilder.FilterPropertiesCloseEventHandler(closeHandler.OnClose);
                            filterProperties.MdiParent = MDIParentForm;
                            filterProperties.Show();
                            filterProperties.BringToFront();
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

        /// <summary>
        /// Virtual method that indicates if the data in the clipboard is valid
        /// </summary>
        /// <param name="aClipboardDataType">
        /// The eProfileType of the clipboard data
        /// </param>
        /// A boolean indicating if the data in the clipboard is valid
        /// <returns></returns>

        override public bool isAllowedDataType(eProfileType aClipboardDataType)
        {
            return aClipboardDataType == eProfileType.FilterStore;
        }

        /// <summary>
        /// Virtual method used to refresh a favorites branch
        /// </summary>
        /// <param name="aStartNode">
        /// The MIDTreeNode to start searching for changed node
        /// </param>
        /// <param name="aChangedNode">
        /// The MIDTreeNode to that was changed
        /// </param>

        override public void RefreshShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
        {
            try
            {
                // Begin TT#62 - JSmith - Object reference error when double-click folder
                if (aChangedNode != null)
                {
                    // End TT#62
                    foreach (MIDFilterNode node in aStartNode.Nodes)
                    {
                        if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
                        {
                            node.RefreshShortcutNode(aChangedNode);

                            if (node.Profile.ProfileType != eProfileType.FilterStore)
                            {
                                if (node.isObjectShortcut || node.isFolderShortcut)
                                {
                                    //Begin Track #6201 - JScott - Store Count removed from attr sets
                                    //node.Text = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                                    node.InternalText = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                                    //End Track #6201 - JScott - Store Count removed from attr sets
                                    DeleteChildNodes(node);
                                    CreateShortcutChildren(aChangedNode, node);
                                }
                                else if (node.isChildShortcut)
                                {
                                    //Begin Track #6201 - JScott - Store Count removed from attr sets
                                    //node.Text = ((FolderProfile)aChangedNode.Profile).Name;
                                    node.InternalText = ((FolderProfile)aChangedNode.Profile).Name;
                                    //End Track #6201 - JScott - Store Count removed from attr sets
                                    DeleteChildNodes(node);
                                    CreateShortcutChildren(aChangedNode, node);
                                }
                            }
                        }
                        else if (node.NodeProfileType == eProfileType.FilterStoreSubFolder || node.isFolderShortcut)
                        {
                            RefreshShortcuts(node, aChangedNode);
                        }
                    }
                    // Begin TT#62 - JSmith - Object reference error when double-click folder
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
        /// Virtual method used to determine if a drop is allowed for a ClipboardListBase
        /// </summary>
        /// <param name="aDropAction">
        /// The DragDropEffects of the action being performed.
        /// </param>
        /// <param name="aDropNode">
        /// The node being dropped on.
        /// </param>
        /// <param name="aClipboardList">
        /// The ClipboardListBase of nodes being dropped.
        /// </param>
        /// <returns>
        /// A boolean indicating if a drop is allowed for a ClipboardListBase
        /// </returns>

        override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ClipboardListBase aSelectedNodes)
        {
            try
            {
                if (aSelectedNodes.ClipboardDataType == eProfileType.FilterStore ||
                    aSelectedNodes.ClipboardDataType == eProfileType.FilterStoreSubFolder)
                {
                    return ((TreeNodeClipboardList)aSelectedNodes).ClipboardProfile.Node.isDropAllowed(aDropAction, aDropNode);
                }

                return false;
            }
            catch
            {
                throw;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private bool InUseFilterNode(MIDTreeNode aNode)
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
                    if (scgRid > 0)
                        ridList.Add(scgRid);

                    eProfileType etype = aNode.NodeProfileType;
                    //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                    bool display = false;
                    const bool inQuiry = true;
                    DisplayInUseForm(ridList, etype, inUseTitle, ref display, inQuiry);
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
                    foreach (MIDTreeNode aNode in aDeleteList)
                    {
                        // Begin TT#4326 - JSmith - Unable to Remove shortcuts that are In Use
                        //nodeArrayList.Add(aNode.NodeRID);
                        //profileType = aNode.NodeProfileType;
                        if (!aNode.isShortcut)
                        {
                            nodeArrayList.Add(aNode.NodeRID);
                            profileType = aNode.NodeProfileType;
                        }
                        // End TT#4326 - JSmith - Unable to Remove shortcuts that are In Use
                    }

                    if (nodeArrayList.Count > 0)
                    {
                        //inUseTitle = Regex.Replace(profileType.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                        inUseTitle = InUseUtility.GetInUseTitleFromProfileType(profileType); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                        DisplayInUseForm(nodeArrayList, profileType, inUseTitle, false, out dialogShown);
                        if (dialogShown)
                        {
                            allowDelete = false;
                        }
                    }
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
        public void DisplayInUseForm(ArrayList userRids, eProfileType myEnum, string itemTitle, ref bool display, bool inQuiry)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            myfrm.ResolveInUseData(ref display, inQuiry);
            if (display == true)
            { myfrm.ShowDialog(); }
        }

        public void DisplayInUseForm(ArrayList userRids, eProfileType myEnum, string itemTitle, bool inQuiry, out bool deleting)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            bool display = false;
            bool showDialog = false;
            myfrm.ResolveInUseData(ref display, inQuiry, true, out showDialog);
            deleting = showDialog;
            if (showDialog == true)
            { myfrm.ShowDialog(); }
        }
        //END TT#110-MD-VStuart - In Use Tool

        //--------------
        //PUBLIC METHODS
        //--------------

        public void CreateShortcutChildren(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDFilterNode newNode = null;

            try
            {
                foreach (MIDFilterNode node in aFromNode.Nodes)
                {
                    switch (node.NodeProfileType)
                    {
                        case eProfileType.FilterStoreSubFolder:

                            newNode = new MIDFilterNode(treeFilterType,
                                SAB,
                                eTreeNodeType.ChildFolderShortcutNode,
                                node.Profile,
                                node.Text,
                                node.ParentId,
                                node.UserId,
                                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                                //node.FunctionSecurityProfile,
                                node.NodeSecurityGroup,
                                //End Track #6321 - JScott - User has ability to to create folders when security is view
                                cClosedFolderImage,
                                cClosedFolderImage,
                                cOpenFolderImage,
                                cOpenFolderImage,
                                node.OwnerUserRID);

                            break;

                        case eProfileType.FilterStore:

                            newNode = new MIDFilterNode(treeFilterType,
                                SAB,
                                eTreeNodeType.ChildObjectShortcutNode,
                                node.Profile,
                                node.Text,
                                node.ParentId,
                                node.UserId,
                                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                                //node.FunctionSecurityProfile,
                                node.NodeSecurityGroup,
                                //End Track #6321 - JScott - User has ability to to create folders when security is view
                                node.CollapsedImageIndex,
                                node.SelectedCollapsedImageIndex,
                                node.OwnerUserRID);

                            break;
                    }

                    aToNode.Nodes.Add(newNode);

                    CreateShortcutChildren(node, newNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public void DeleteShortcuts(MIDTreeNode aStartNode, MIDTreeNode aDeleteNode)
        {
            object[] deleteList;

            try
            {
                deleteList = new object[aStartNode.Nodes.Count];
                aStartNode.Nodes.CopyTo(deleteList, 0);

                foreach (MIDFilterNode node in deleteList)
                {
                    if (node.Profile.Key == aDeleteNode.Profile.Key && node.Profile.ProfileType == aDeleteNode.Profile.ProfileType &&
                        node.isShortcut)
                    {
                        DeleteChildNodes(node);
                        node.Remove();
                    }
                    else if (node.NodeProfileType == eProfileType.FilterStoreSubFolder ||
                        node.isFolderShortcut ||
                        (node.isChildShortcut && node.Profile.ProfileType == eProfileType.FilterStoreSubFolder))
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

        public string FindNewFilterName(string aFilterName, int aUserRID)
        {
            int index;
            string newName;
            int key;

            try
            {
                index = 0;
                newName = aFilterName;
                key = _dlFilters.FilterGetKey(treeFilterType, aUserRID, newName);

                while (key != -1)
                {
                    index++;

                    //if (index > 1)
                    //{
                    //    newName = "Copy (" + index + ") of " + aFilterName;
                    //}
                    //else
                    //{
                    //    newName = "Copy of " + aFilterName;
                    //}
                    newName = Include.GetNewName(name: aFilterName, index: index);

                    key = _dlFilters.FilterGetKey(treeFilterType, aUserRID, newName);
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //-----------------
        //PROTECTED METHODS
        //-----------------

        //---------------
        //PRIVATE METHODS
        //---------------

        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
        //private FunctionSecurityProfile GetUserSecLvl(int aUserRID)
        //{
        //    return (FunctionSecurityProfile)_secLvlHash[aUserRID];
        //}
        private MIDTreeNodeSecurityGroup GetUserSecGrp(int aUserRID)
        {
            return (MIDTreeNodeSecurityGroup)_secGrpHash[aUserRID];
        }
        //End Track #6321 - JScott - User has ability to to create folders when security is view

        private void BuildFolderBranch(int aUserRID, int aParentFolderRID, MIDTreeNode aParentNode, DataTable aFolderItems, DataTable aFolders, DataTable aStoreFilters)
        {
            DataRow[] folderItemList;
            DataRow itemRow;
            FolderProfile folderProf;
            StoreFilterProfile filterProf;
            MIDFilterNode newNode = null;

            try
            {
                folderItemList = aFolderItems.Select("USER_RID = " + aUserRID + " AND PARENT_FOLDER_RID = " + aParentFolderRID);

                foreach (DataRow row in folderItemList)
                {
                    switch ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"]))
                    {
                        case eProfileType.FilterStoreSubFolder:

                            itemRow = aFolders.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Filter", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            folderProf = new FolderProfile(itemRow);
                            newNode = BuildSubFolderNode(folderProf, aParentNode);
                            BuildFolderBranch(aUserRID, newNode.Profile.Key, newNode, aFolderItems, aFolders, aStoreFilters);
                            break;

                        case eProfileType.FilterStore:

                            itemRow = aStoreFilters.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Filter", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            filterProf = new StoreFilterProfile(itemRow);
                            BuildFilterNode(filterProf, aParentNode, aUserRID);
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDFilterNode BuildSubFolderNode(FolderProfile aFolderProf, MIDTreeNode aParentNode)
        {
            MIDFilterNode newNode;

            try
            {
                // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                //newNode = new MIDFilterNode(
                //    SAB,
                //    eTreeNodeType.SubFolderNode,
                //    aFolderProf,
                //    aFolderProf.Name,
                //    aParentNode.NodeRID,
                //    aFolderProf.UserRID,
                //    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //    //GetUserSecLvl(aFolderProf.OwnerUserRID),
                //    GetUserSecGrp(aFolderProf.OwnerUserRID),
                //    //End Track #6321 - JScott - User has ability to to create folders when security is view
                //    cClosedFolderImage,
                //    cClosedFolderImage,
                //    cOpenFolderImage,
                //    cOpenFolderImage,
                //    aFolderProf.OwnerUserRID);
                if (aParentNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    newNode = new MIDFilterNode(treeFilterType,
                    SAB,
                    eTreeNodeType.SubFolderNode,
                    aFolderProf,
                    aFolderProf.Name,
                    aParentNode.NodeRID,
                    aFolderProf.UserRID,
                    FavoritesSecGrp,
                    cClosedFolderImage,
                    cClosedFolderImage,
                    cOpenFolderImage,
                    cOpenFolderImage,
                    aFolderProf.OwnerUserRID);
                }
                else
                {
                    newNode = new MIDFilterNode(treeFilterType,
                    SAB,
                    eTreeNodeType.SubFolderNode,
                    aFolderProf,
                    aFolderProf.Name,
                    aParentNode.NodeRID,
                    aFolderProf.UserRID,
                    GetUserSecGrp(aFolderProf.OwnerUserRID),
                    cClosedFolderImage,
                    cClosedFolderImage,
                    cOpenFolderImage,
                    cOpenFolderImage,
                    aFolderProf.OwnerUserRID);
                }
                // End TT#42

                FolderNodeHash[aFolderProf.Key] = newNode;
                aParentNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        public MIDFilterNode BuildFilterNode(StoreFilterProfile prof, MIDTreeNode aParentNode, int aUserId)
        {
            MIDFilterNode newNode;

            try
            {




                if (prof.OwnerUserRID == Include.GlobalUserRID)
                {
                    newNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.ObjectNode,
                        prof,
                        prof.Name,
                        aParentNode.Profile.Key,
                        aUserId,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //GetUserSecLvl(aProfile.OwnerUserRID),
                        GetUserSecGrp(prof.OwnerUserRID),
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cGlobalFilterUnselectedImage,
                        cGlobalFilterSelectedImage,
                        prof.OwnerUserRID);
                }
                else
                {
                    newNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.ObjectNode,
                        prof,
                        prof.Name,
                        aParentNode.Profile.Key,
                        aUserId,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //GetUserSecLvl(aProfile.OwnerUserRID),
                        GetUserSecGrp(prof.OwnerUserRID),
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cUserFilterUnselectedImage,
                        cUserFilterSelectedImage,
                        prof.OwnerUserRID);
                }

                ItemNodeHash[prof.Key] = newNode;
                aParentNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDFilterNode BuildRootShortcutNode(MIDFilterNode aFromNode, MIDFilterNode aToNode)
        {
            MIDFilterNode newNode;
            MIDTreeNodeSecurityGroup securityGroup;  // TT#2014-MD - JSmith - Assortment Security

            try
            {
                // Begin TT#2014-MD - JSmith - Assortment Security
                if (aToNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    securityGroup = (MIDTreeNodeSecurityGroup)aFromNode.NodeSecurityGroup.Clone();
                    securityGroup.FunctionSecurityProfile.SetAllowDelete();
                    securityGroup.FolderSecurityProfile.SetAllowDelete();
                }
                else
                {
                    securityGroup = aFromNode.NodeSecurityGroup;
                }
                // End TT#2014-MD - JSmith - Assortment Security

                newNode = new MIDFilterNode(treeFilterType,
                    SAB,
                    eTreeNodeType.FolderShortcutNode,
                    aFromNode.Profile,
                    aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                    aToNode.Profile.Key,
                    aFromNode.UserId,
                    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                    //aFromNode.FunctionSecurityProfile,
                    // Begin TT#2014-MD - JSmith - Assortment Security
                    //aFromNode.NodeSecurityGroup,
                    securityGroup,
                    // End TT#2014-MD - JSmith - Assortment Security
                    //End Track #6321 - JScott - User has ability to to create folders when security is view
                    cClosedShortcutFolderImage,
                    cClosedShortcutFolderImage,
                    cOpenShortcutFolderImage,
                    cOpenShortcutFolderImage,
                    aFromNode.OwnerUserRID);

                aToNode.Nodes.Add(newNode);

                CreateShortcutChildren(aFromNode, newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDFilterNode BuildObjectShortcutNode(MIDFilterNode aFromNode, MIDFilterNode aToNode)
        {
            MIDFilterNode newNode;
            // Begin TT#394 - JSmith - Cut and Paste not performing consistently
            string text;
            // End TT#394
            MIDTreeNodeSecurityGroup securityGroup;  // TT#2014-MD - JSmith - Assortment Security

            try
            {
                // Begin TT#2014-MD - JSmith - Assortment Security
                if (aToNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    securityGroup = (MIDTreeNodeSecurityGroup)aFromNode.NodeSecurityGroup.Clone();
                    securityGroup.FunctionSecurityProfile.SetAllowDelete();
                    securityGroup.FolderSecurityProfile.SetAllowDelete();
                }
                else
                {
                    securityGroup = aFromNode.NodeSecurityGroup;
                }
                // End TT#2014-MD - JSmith - Assortment Security

                if (aFromNode.UserId == Include.GlobalUserRID)
                {
                    // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                    if (aFromNode.TreeNodeType == eTreeNodeType.ObjectShortcutNode)
                    {
                        text = aFromNode.Text;
                    }
                    else
                    {
                        text = aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")";
                    }
                    // End TT#394
                    newNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.ObjectShortcutNode,
                        aFromNode.Profile,
                        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                        //aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                        text,
                        // End TT#394
                        aToNode.Profile.Key,
                        aFromNode.UserId,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //aFromNode.FunctionSecurityProfile,
                        // Begin TT#2014-MD - JSmith - Assortment Security
                        //aFromNode.NodeSecurityGroup,
                        securityGroup,
                        // End TT#2014-MD - JSmith - Assortment Security
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cGlobalFilterShortcutUnselectedImage,
                        cGlobalFilterShortcutSelectedImage,
                        aFromNode.UserId);
                }
                else
                {
                    // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                    if (aFromNode.TreeNodeType == eTreeNodeType.ObjectShortcutNode)
                    {
                        text = aFromNode.Text;
                    }
                    else
                    {
                        text = aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")";
                    }
                    // End TT#394
                    newNode = new MIDFilterNode(treeFilterType,
                        SAB,
                        eTreeNodeType.ObjectShortcutNode,
                        aFromNode.Profile,
                        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                        //aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                        text,
                        // End TT#394
                        aToNode.Profile.Key,
                        aFromNode.UserId,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //aFromNode.FunctionSecurityProfile,
                        // Begin TT#2014-MD - JSmith - Assortment Security
                        //aFromNode.NodeSecurityGroup,
                        securityGroup,
                        // End TT#2014-MD - JSmith - Assortment Security
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cUserFilterShortcutUnselectedImage,
                        cUserFilterShortcutSelectedImage,
                        aFromNode.OwnerUserRID);
                }

                aToNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private GenericEnqueue EnqueueObject(StoreFilterProfile aFilterProf, bool aAllowReadOnly)
        {
            GenericEnqueue objEnqueue;
            string errMsg;

            try
            {
                objEnqueue = new GenericEnqueue(eLockType.Filter, aFilterProf.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

                try
                {
                    objEnqueue.EnqueueGeneric();
                }
                catch (GenericConflictException)
                {
                    /* Begin TT#1159 - Improve Messaging */
                    string[] errParms = new string[3];
                    errParms.SetValue("MID Filter Node", 0);
                    errParms.SetValue(aFilterProf.Name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                    //errMsg = "The Filter \"" + aFilterProf.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
                    /* End TT#1159 - Improve Messaging */

                    if (aAllowReadOnly)
                    {
                        errMsg += System.Environment.NewLine + System.Environment.NewLine;
                        errMsg += "Do you wish to continue with the Filter as read-only?";

                        if (MessageBox.Show(errMsg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            objEnqueue = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(errMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        objEnqueue = null;
                    }
                }

                return objEnqueue;
            }
            catch
            {
                throw;
            }
        }

        private MIDFilterNode MoveFilterNode(MIDFilterNode aFromNode, MIDFilterNode aToNode)
        {
            MIDFilterNode newNode;
            FolderProfile folderProf;
            StoreFilterProfile filterProf;
            object[] moveArray;

            try
            {
                if (aFromNode.isFolderShortcut || aFromNode.isObjectShortcut)
                {
                    if (_dlFolder.Folder_Shortcut_Exists(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType))
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return aFromNode;
                    }

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Shortcut_Delete(aFromNode.ParentId, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                        _dlFolder.Folder_Shortcut_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

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

                    aFromNode.Remove();

                    if (aFromNode.isFolderShortcut)
                    {
                        newNode = BuildRootShortcutNode(aFromNode, aToNode);
                    }
                    else
                    {
                        newNode = BuildObjectShortcutNode(aFromNode, aToNode);
                    }

                    return aFromNode;
                }
                else if (aFromNode.NodeProfileType == eProfileType.FilterStoreSubFolder)
                {
                    folderProf = (FolderProfile)aFromNode.Profile;

                    if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                    {
                        DlSecurity.OpenUpdateConnection();

                        try
                        {
                            DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(folderProf.ProfileType), folderProf.Key);
                            DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(folderProf.ProfileType), folderProf.Key, aToNode.UserId);

                            DlSecurity.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            DlSecurity.CloseUpdateConnection();
                        }

                        folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.FilterStoreSubFolder);
                        folderProf.UserRID = aToNode.UserId;
                        folderProf.OwnerUserRID = aToNode.UserId;
                    }

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Update(folderProf.Key, folderProf.UserRID, folderProf.Name, folderProf.ProfileType);
                        _dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

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

                    newNode = BuildSubFolderNode(folderProf, aToNode);

                    moveArray = new object[aFromNode.Nodes.Count];
                    aFromNode.Nodes.CopyTo(moveArray, 0);

                    foreach (MIDFilterNode node in moveArray)
                    {
                        MoveFilterNode(node, newNode);
                    }

                    aFromNode.Remove();

                    return newNode;
                }
                else if (aFromNode.NodeProfileType == eProfileType.FilterStore)
                {
                    filterProf = (StoreFilterProfile)aFromNode.Profile;

                    if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                    {
                        DlSecurity.OpenUpdateConnection();

                        try
                        {
                            DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(filterProf.ProfileType), filterProf.Key);
                            DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(filterProf.ProfileType), filterProf.Key, aToNode.UserId);

                            DlSecurity.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            DlSecurity.CloseUpdateConnection();
                        }

                        filterProf.Name = FindNewFilterName(filterProf.Name, aToNode.UserId);
                        filterProf.UserRID = aToNode.UserId;
                        filterProf.OwnerUserRID = aToNode.UserId;
                    }

                    _dlFilters.OpenUpdateConnection();
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFilters.FilterUpdate(filterProf);
                        _dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

                        _dlFolder.CommitData();
                        _dlFilters.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        _dlFolder.CloseUpdateConnection();
                        _dlFilters.CloseUpdateConnection();
                    }

                    aFromNode.Remove();
                    newNode = BuildFilterNode(filterProf, aToNode, filterProf.UserRID);
                    return newNode;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        private MIDFilterNode JustMoveFilterNodeAndDoNotRename(MIDFilterNode aFromNode, MIDFilterNode aToNode)
        {
            MIDFilterNode newNode;
            //FolderProfile folderProf;
            StoreFilterProfile filterProf;
            //object[] moveArray;

            try
            {


                if (aFromNode.NodeProfileType == eProfileType.FilterStore)
                {
                    filterProf = (StoreFilterProfile)aFromNode.Profile;

                    if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                    {
                        DlSecurity.OpenUpdateConnection();

                        try
                        {
                            DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(filterProf.ProfileType), filterProf.Key);
                            DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(filterProf.ProfileType), filterProf.Key, aToNode.UserId);

                            DlSecurity.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            DlSecurity.CloseUpdateConnection();
                        }

                        //filterProf.Name = FindNewFilterName(filterProf.Name, aToNode.UserId);
                        //filterProf.UserRID = aToNode.UserId;
                        //filterProf.OwnerUserRID = aToNode.UserId;
                    }

                    //_dlFilters.OpenUpdateConnection();
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        //_dlFilters.FilterUpdate(filterProf);
                        _dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

                        _dlFolder.CommitData();
                        //_dlFilters.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        _dlFolder.CloseUpdateConnection();
                        //_dlFilters.CloseUpdateConnection();
                    }

                    aFromNode.Remove();
                    newNode = BuildFilterNode(filterProf, aToNode, filterProf.UserRID);
                    return newNode;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        private MIDFilterNode CopyFilterNode(MIDFilterNode aFromNode, MIDFilterNode aToNode, bool aFindUniqueName)
        {
            //DataTable dtFilterObjects;
            FolderProfile folderProf;
            StoreFilterProfile filterProf;
            MIDFilterNode newNode;

            try
            {
                if (aFromNode.isSubFolder)
                {
                    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                    folderProf.UserRID = aToNode.UserId;
                    folderProf.OwnerUserRID = aToNode.UserId;

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

                    newNode = BuildSubFolderNode(folderProf, aToNode);

                    foreach (MIDFilterNode node in aFromNode.Nodes)
                    {
                        CopyFilterNode(node, newNode, aFindUniqueName);
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

                    newNode = BuildRootShortcutNode(aFromNode, aToNode);

                    return newNode;
                }
                else if (aFromNode.isObjectShortcut)
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

                    newNode = BuildObjectShortcutNode(aFromNode, aToNode);

                    return newNode;
                }
                else if (aFromNode.isObject)
                {
                    filterProf = (StoreFilterProfile)((StoreFilterProfile)aFromNode.Profile).Clone();
                    filterProf.UserRID = aToNode.UserId;
                    filterProf.OwnerUserRID = aToNode.UserId;

                    if (aFindUniqueName)
                    {                       
                        filterProf.Name = FindNewFilterName(filterProf.Name, aToNode.UserId);
                    }


                    //_dlFolder.OpenUpdateConnection();

                    try
                    {
                        //Insert a copy of the filter, its conditions, and its list values
                        // int folderUserRID = aToNode.UserId;
                        int oldFilterRID = filterProf.Key;

                        //int userForCopiedFilter;
                        ////Check to see if the current selected folder is a child of the global folder
                        //if (SelectedNode != null && SelectedNode != _globalNode && SelectedNode.isChildOf(_globalNode))
                        //{
                        //    userForCopiedFilter = Include.GlobalUserRID;
                        //}
                        //else
                        //{
                        //    userForCopiedFilter = SAB.ClientServerSession.UserRID;
                        //}



                        filterProf.Key = _dlFilters.InsertFilter(treeFilterType, filterProf.UserRID, filterProf.OwnerUserRID, filterProf.Name, filterProf.IsLimited, filterProf.ResultLimit);

                        //read conditions to datatable
                        DataTable dtConditions = _dlFilters.FilterReadConditions(oldFilterRID);
                        DataTable dtListValues = _dlFilters.FilterReadListValues(oldFilterRID);

                        foreach (DataRow drCondition in dtConditions.Rows)
                        {
                            int oldConditionRID = (int)drCondition["CONDITION_RID"];
                            drCondition["FILTER_RID"] = filterProf.Key;

                            filterCondition fc = new filterCondition();
                            fc.LoadFromDataRow(drCondition);
                            //copy the name to the name condition
                            if (fc.dictionaryIndex == filterDictionary.FilterName)
                            {
                                fc.valueToCompare = filterProf.Name;
                            }
                            if (fc.dictionaryIndex == filterDictionary.FilterFolder)
                            {
                                //if (filterProf.UserRID == Include.GlobalUserRID)
                                //{
                                //    fc.valueToCompareInt = Include.GlobalUserRID;
                                //}
                                //else
                                //{
                                //    fc.valueToCompareInt = -1;
                                //}
                                fc.valueToCompareInt = filterProf.OwnerUserRID; //we need to update the condition so we can build formatted text
                            }
							// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
							//int newConditionRID = _dlFilters.InsertCondition(fc.conditionFilterRID, fc.Seq, fc.ParentSeq, fc.SiblingSeq, fc.dictionaryIndex, fc.logicIndex, fc.fieldIndex, fc.operatorIndex, fc.valueTypeIndex, fc.dateTypeIndex, fc.numericTypeIndex, fc.valueToCompare, fc.valueToCompareDouble, fc.valueToCompareDouble2, fc.valueToCompareInt, fc.valueToCompareInt2, fc.valueToCompareBool, fc.valueToCompareDateFrom, fc.valueToCompareDateTo, fc.valueToCompareDateBetweenFromDays, fc.valueToCompareDateBetweenToDays, fc.variable1_Index, fc.variable1_VersionIndex, fc.variable1_HN_RID, fc.variable1_CDR_RID, fc.variable1_VariableValueTypeIndex, fc.variable1_TimeTypeIndex, fc.operatorVariablePercentageIndex, fc.variable2_Index, fc.variable2_VersionIndex, fc.variable2_HN_RID, fc.variable2_CDR_RID, fc.variable2_VariableValueTypeIndex, fc.variable2_TimeTypeIndex, fc.headerMerchandise_HN_RID, fc.sortByTypeIndex, fc.sortByFieldIndex, fc.listConstantType.dbIndex);
                            int newConditionRID = _dlFilters.InsertCondition(fc.conditionFilterRID, fc.Seq, fc.ParentSeq, fc.SiblingSeq, fc.dictionaryIndex, fc.logicIndex, fc.fieldIndex, fc.operatorIndex, fc.valueTypeIndex, fc.dateTypeIndex, fc.numericTypeIndex, fc.valueToCompare, fc.valueToCompareDouble, fc.valueToCompareDouble2, fc.valueToCompareInt, fc.valueToCompareInt2, fc.valueToCompareBool, fc.valueToCompareDateFrom, fc.valueToCompareDateTo, fc.valueToCompareDateBetweenFromDays, fc.valueToCompareDateBetweenToDays, fc.variable1_Index, fc.variable1_VersionIndex, fc.variable1_HN_RID, fc.variable1_CDR_RID, fc.variable1_VariableValueTypeIndex, fc.variable1_TimeTypeIndex, fc.operatorVariablePercentageIndex, fc.variable2_Index, fc.variable2_VersionIndex, fc.variable2_HN_RID, fc.variable2_CDR_RID, fc.variable2_VariableValueTypeIndex, fc.variable2_TimeTypeIndex, fc.headerMerchandise_HN_RID, fc.sortByTypeIndex, fc.sortByFieldIndex, fc.listConstantType.dbIndex, fc.date_CDR_RID);
							// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only


                            DataRow[] drListValues = dtListValues.Select("CONDITION_RID=" + oldConditionRID);

                            DataTable dtNewListValues = filterCondition.GetListValuesDataTable();
                            foreach (DataRow drListValue in drListValues)
                            {
                                DataRow drNew = dtNewListValues.NewRow();
                                filterUtility.DataRowCopy(drListValue, drNew);
                                drNew["CONDITION_RID"] = newConditionRID;
                                dtNewListValues.Rows.Add(drNew);
                            }
                            _dlFilters.InsertListValues(dtNewListValues);
                        }


                        filter f = new filter(filterProf.UserRID, filterProf.OwnerUserRID);
                        f.filterName = filterProf.Name;
                        f.filterRID = filterProf.Key;
						// Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error 
						//newNode = AfterSave(f);
                        newNode = AfterSave(f, aToNode);
						// End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error 
                        //_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, filterProf.Key, eProfileType.StoreFilter);


                        //_dlFolder.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {

                        //_dlFolder.CloseUpdateConnection();
                    }

                    //newNode = BuildFilterNode(filterProf, aToNode, filterProf.UserRID);

                    return newNode;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private bool DeleteFilterNode(MIDFilterNode aNode)
        {
            GenericEnqueue objEnqueue;
            object[] deleteArray;

            try
            {
                if (aNode.isObject)
                {
                    objEnqueue = EnqueueObject((StoreFilterProfile)aNode.Profile, false);

                    if (objEnqueue == null)
                    {
                        return false;
                    }

                    try
                    {
                        _dlFilters.OpenUpdateConnection();
                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.FilterStore);
                            _dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.FilterStore);
                            _dlFilters.FilterDelete(aNode.Profile.Key);
                            //_dlFilters.StoreFilterObject_Delete(aNode.Profile.Key);

                            _dlFilters.CommitData();
                            _dlFolder.CommitData();
                        }
                        catch (DatabaseForeignKeyViolation)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            _dlFilters.CloseUpdateConnection();
                            _dlFolder.CloseUpdateConnection();
                        }

                        //delete filter user item
                        SecurityAdmin secAdmin = new SecurityAdmin();
                        secAdmin.OpenUpdateConnection();
                        try
                        {
                            secAdmin.DeleteUserItemByTypeAndRID((int)eProfileType.FilterStore, aNode.Profile.Key);
                            secAdmin.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            secAdmin.CloseUpdateConnection();
                        }


                        DeleteShortcuts(_favoritesNode, aNode);
                        aNode.Remove();
                    }
                    catch (Exception error)
                    {
                        string message = error.ToString();
                        throw;
                    }
                    finally
                    {
                        objEnqueue.DequeueGeneric();
                    }
                }
                else if (aNode.isSubFolder)
                {
                    deleteArray = new object[aNode.Nodes.Count];
                    aNode.Nodes.CopyTo(deleteArray, 0);

                    foreach (MIDFilterNode node in deleteArray)
                    {
                        DeleteFilterNode(node);
                    }

                    if (aNode.Nodes.Count == 0)
                    {
                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.FilterStoreSubFolder);
                            _dlFolder.Folder_Delete(aNode.Profile.Key, eProfileType.FilterStoreSubFolder);
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

                        DeleteShortcuts(_favoritesNode, aNode);
                        DeleteChildNodes((MIDFilterNode)aNode);
                        aNode.Remove();
                    }
                }
                else if (aNode.isObjectShortcut)
                {
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.FilterStore);
                        _dlFolder.CommitData();
                    }
                    catch (DatabaseForeignKeyViolation)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
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

                    aNode.Remove();
                }
                else if (aNode.isFolderShortcut)
                {
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.FilterStoreSubFolder);
                        _dlFolder.CommitData();
                    }
                    catch (DatabaseForeignKeyViolation)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
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

                    DeleteChildNodes((MIDFilterNode)aNode);
                    aNode.Remove();
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
        /// Handles the filter folder for both insert and update
        /// </summary>
        /// <param name="f"></param>
        // Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
        //public MIDFilterNode AfterSave(filter f)
        public MIDFilterNode AfterSave(filter f, MIDFilterNode parentNode = null)
        // End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
        {
            //MIDFilterNode parentNode;  // TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
            MIDFilterNode nodeToReturn = null;
            try
            {
                MIDFilterNode node = (MIDFilterNode)ItemNodeHash[f.filterRID];

                if (node == null)
                {
                    int ownerRID;
                    int userRID;
                    if (f.ownerUserRID == Include.GlobalUserRID)
                    {
                        ownerRID = Include.GlobalUserRID;
                        userRID = Include.GlobalUserRID;
                        //Check to see if the current selected folder is a child of the global folder
                        // Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
                        if (parentNode == null)
                        {
                        // End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
                            if (SelectedNode != null && SelectedNode != _globalNode && SelectedNode.isChildOf(_globalNode))
                            {
                                if (SelectedNode.isSubFolder)
                                {
                                    parentNode = (MIDFilterNode)SelectedNode;
                                }
                                else
                                {
                                    parentNode = (MIDFilterNode)SelectedNode.Parent;
                                }
                            }
                            else
                            {
                                //Use the default global folder
                                parentNode = _globalNode;
                            }
                        // Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
                        }
                        // End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
                    }
                    else //it is a user folder
                    {
                        ownerRID = SAB.ClientServerSession.UserRID;
                        userRID = SAB.ClientServerSession.UserRID;
                        //Check to see if the current selected folder is a child of the main user folder
                        // Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
                        if (parentNode == null)
                        {
                        // End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error  
                            if (SelectedNode != null && SelectedNode != _userNode && SelectedNode.isChildOf(_userNode))
                            {
                                if (SelectedNode.isSubFolder)
                                {
                                    parentNode = (MIDFilterNode)SelectedNode;
                                }
                                else
                                {
                                    parentNode = (MIDFilterNode)SelectedNode.Parent;
                                }
                            }
                            else
                            {
                                //Use the default user folder
                                parentNode = _userNode;
                            }
                        // Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error 
                        }
                        // End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error 
                    }


                    //save filter to user item
                    SecurityAdmin secAdmin = new SecurityAdmin();
                    secAdmin.OpenUpdateConnection();
                    try
                    {
                        secAdmin.DeleteUserItemByTypeAndRID((int)eProfileType.FilterStore, f.filterRID);
                        //secAdmin.AddUserItem(f.userRID, (int)eProfileType.FilterStore, f.filterRID, ownerRID);
                        secAdmin.AddUserItem(userRID, (int)eProfileType.FilterStore, f.filterRID, ownerRID);
                        secAdmin.CommitData();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        secAdmin.CloseUpdateConnection();
                    }



                    //save filter to folder
                    FolderDataLayer _dlFolder = new FolderDataLayer();
                    _dlFolder.OpenUpdateConnection();
                    try
                    {
                        _dlFolder.Folder_Item_Insert(parentNode.NodeRID, f.filterRID, eProfileType.FilterStore);
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


                    //Make a new node, select it, and return it                 
                    StoreFilterProfile prof = new StoreFilterProfile(f.filterRID);
                    prof.Name = f.filterName;
                    prof.UserRID = f.userRID;
                    prof.OwnerUserRID = f.ownerUserRID;
                    //Begin TT#1424-MD -jsobek -Header Filter- Result Limit - Copy-Paste a Global filter to user that has a restricted limit after the copy to user the Result limit changes to Unrestricted.
                    prof.IsLimited = f.isLimited;
                    prof.ResultLimit = (int)f.resultLimit;
                    //End TT#1424-MD -jsobek -Header Filter- Result Limit - Copy-Paste a Global filter to user that has a restricted limit after the copy to user the Result limit changes to Unrestricted.
                    node = BuildFilterNode(prof, parentNode, f.userRID);
                    nodeToReturn = node;
                    SelectedNode = node;
                }
                else
                {
                    parentNode = (MIDFilterNode)node.Parent;
                    node.InternalText = f.filterName;
                    //Begin TT#1424-MD -jsobek -Header Filter- Result Limit - Copy-Paste a Global filter to user that has a restricted limit after the copy to user the Result limit changes to Unrestricted.
                    //Begin TT#1423-MD -jsobek -Filters-copy/paste filter, rename copied filter and copy/paste that filter, and the diplay of the new filter reverts to original filter name
                    StoreFilterProfile prof = (StoreFilterProfile)node.Profile;
                    prof.Name = f.filterName;
                    prof.IsLimited = f.isLimited;
                    prof.ResultLimit = (int)f.resultLimit;
                    //End TT#1423-MD -jsobek -Filters-copy/paste filter, rename copied filter and copy/paste that filter, and the diplay of the new filter reverts to original filter name
                    //End TT#1424-MD -jsobek -Header Filter- Result Limit - Copy-Paste a Global filter to user that has a restricted limit after the copy to user the Result limit changes to Unrestricted.

                    //see if we need to move folders (the user can change from global to user folders, and from user to global folders)
                    if (f.ownerUserRID == Include.GlobalUserRID)
                    {
                        //ensure the current parent folder is a child of the main global folder
                        if (parentNode != _globalNode && parentNode.isChildOf(_globalNode) == false)
                        {
                            //we need to move this node 

                            SelectedNode = JustMoveFilterNodeAndDoNotRename(node, _globalNode);
                            UpdateNodeImage(SelectedNode, cGlobalFilterUnselectedImage, cGlobalFilterSelectedImage);
                            SelectedNode.InternalText = f.filterName;
                            parentNode = _globalNode;
                        }
                    }
                    else
                    {
                        //ensure the current parent folder is a child of the main user folder
                        if (parentNode != _userNode && parentNode.isChildOf(_userNode) == false)
                        {
                            //we need to move this node

                            nodeToReturn = JustMoveFilterNodeAndDoNotRename(node, _userNode);
                            SelectedNode = nodeToReturn;
                            UpdateNodeImage(SelectedNode, cUserFilterUnselectedImage, cUserFilterSelectedImage);
                            SelectedNode.InternalText = f.filterName;
                            parentNode = _userNode;
                        }
                    }



                }

                SortChildNodes(parentNode);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            return nodeToReturn;
        }
        private void UpdateNodeImage(MIDTreeNode node, int ImageIndex, int SelectedImageIndex)
        {
            node._collapsedImageIndex = ImageIndex;
            node._selectedCollapsedImageIndex = SelectedImageIndex;
            node._expandedImageIndex = ImageIndex;
            node._selectedExpandedImageIndex = SelectedImageIndex;
            node.SetCollapseImage();
        }
    }

}
