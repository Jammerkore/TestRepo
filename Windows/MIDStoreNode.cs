using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public class StoreTreeView : MIDTreeView
    {
        //=======
        // FIELDS
        //=======

        private ExplorerAddressBlock _EAB;

        private int cFavoritesImage;
        private int cClosedFolderImage;
        private int cOpenFolderImage;
        private int cClosedShortcutFolderImage;
        private int cOpenShortcutFolderImage;
        private int cClosedSharedFolderImage;
        private int cOpenSharedFolderImage;
        private int cGlobalStoreGroupDynamicClosedImage;
        private int cGlobalStoreGroupStaticClosedImage;
        private int cGlobalStoreGroupDynamicOpenImage;
        private int cGlobalStoreGroupStaticOpenImage;
        private int cGlobalStoreGroupDynamicClosedShortcutImage;
        private int cGlobalStoreGroupStaticClosedShortcutImage;
        private int cGlobalStoreGroupDynamicOpenShortcutImage;
        private int cGlobalStoreGroupStaticOpenShortcutImage;
        private int cGlobalStoreGroupDynamicClosedSharedImage;
        private int cGlobalStoreGroupStaticClosedSharedImage;
        private int cGlobalStoreGroupDynamicOpenSharedImage;
        private int cGlobalStoreGroupStaticOpenSharedImage;
        private int cUserStoreGroupImage;
        private int cUserStoreGroupShortcutImage;
        private int cUserStoreGroupSharedImage;
        private int cStoreGroupLevelClosedImage;
        private int cStoreGroupLevelOpenImage;
        private int cStoreDynamicUnselectedImage;
        private int cStoreStaticUnselectedImage;
        private int cStoreSelectedImage;

        // Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
        //private string cAvailableStoresText;

        //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
        //private Hashtable _storeGroupSecLvlHash;
        //private FunctionSecurityProfile _storeSecLvl;
        //private FunctionSecurityProfile _storeCharSecLvl;
        private Hashtable _storeGroupSecGrpHash;
        //End Track #6321 - JScott - User has ability to to create folders when security is view

        private FolderDataLayer _dlFolder;

        private MIDStoreNode _userNode = null;
        private MIDStoreNode _globalNode = null;
        private MIDStoreNode _sharedNode;

        //=============
        // CONSTRUCTORS
        //=============

        public StoreTreeView(ExplorerAddressBlock aEAB)
        {
            _EAB = aEAB;
        }

        //===========
        // PROPERTIES
        //===========

        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
        private MIDTreeNodeSecurityGroup UserStoreGroupSecGrp
        {
            get
            {
                return (MIDTreeNodeSecurityGroup)_storeGroupSecGrpHash[SAB.ClientServerSession.UserRID];
            }
        }

        private MIDTreeNodeSecurityGroup GlobalStoreGroupSecGrp
        {
            get
            {
                return (MIDTreeNodeSecurityGroup)_storeGroupSecGrpHash[Include.GlobalUserRID];
            }
        }

        //End Track #6321 - JScott - User has ability to to create folders when security is view
        private FunctionSecurityProfile UserStoreGroupSecLvl
        {
            get
            {
                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //return (FunctionSecurityProfile)_storeGroupSecLvlHash[SAB.ClientServerSession.UserRID];
                return UserStoreGroupSecGrp.FunctionSecurityProfile;
                //End Track #6321 - JScott - User has ability to to create folders when security is view
            }
        }

        private FunctionSecurityProfile GlobalStoreGroupSecLvl
        {
            get
            {
                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //return (FunctionSecurityProfile)_storeGroupSecLvlHash[Include.GlobalUserRID];
                return GlobalStoreGroupSecGrp.FunctionSecurityProfile;
                //End Track #6321 - JScott - User has ability to to create folders when security is view
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
                cClosedFolderImage = MIDGraphics.ImageIndex(MIDGraphics.DefaultClosedFolder);
                cOpenFolderImage = MIDGraphics.ImageIndex(MIDGraphics.DefaultOpenFolder);
                cClosedShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.DefaultClosedFolder);
                cOpenShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.DefaultOpenFolder);
                cClosedSharedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.DefaultClosedFolder);
                cOpenSharedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.DefaultOpenFolder);
                cGlobalStoreGroupDynamicClosedImage = MIDGraphics.ImageIndex(MIDGraphics.ClosedFolderDynamic);
                cGlobalStoreGroupStaticClosedImage = MIDGraphics.ImageIndexWithDefault("yellow", MIDGraphics.ClosedFolder);
                cGlobalStoreGroupDynamicOpenImage = MIDGraphics.ImageIndex(MIDGraphics.OpenedFolderDynamic);
                cGlobalStoreGroupStaticOpenImage = MIDGraphics.ImageIndexWithDefault("yellow", MIDGraphics.OpenFolder);
                cGlobalStoreGroupDynamicClosedShortcutImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.ClosedFolderDynamic);
                cGlobalStoreGroupStaticClosedShortcutImage = MIDGraphics.ImageShortcutIndexWithDefault("yellow", MIDGraphics.ClosedFolder);
                cGlobalStoreGroupDynamicOpenShortcutImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.OpenedFolderDynamic);
                cGlobalStoreGroupStaticOpenShortcutImage = MIDGraphics.ImageShortcutIndexWithDefault("yellow", MIDGraphics.OpenFolder);
                cGlobalStoreGroupDynamicClosedSharedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.ClosedFolderDynamic);
                cGlobalStoreGroupStaticClosedSharedImage = MIDGraphics.ImageSharedIndexWithDefault("yellow", MIDGraphics.ClosedFolder);
                cGlobalStoreGroupDynamicOpenSharedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.OpenedFolderDynamic);
                cGlobalStoreGroupStaticOpenSharedImage = MIDGraphics.ImageSharedIndexWithDefault("yellow", MIDGraphics.OpenFolder);
                cUserStoreGroupImage = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                cUserStoreGroupShortcutImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.SecUserImage);
                cUserStoreGroupSharedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);
                // Begin TT#717 - JSmith - Store explorer is using the same folder graphic for folders and for store attribute sets.
                //cStoreGroupLevelClosedImage = MIDGraphics.ImageIndex(MIDGraphics.DefaultClosedFolder);
                //cStoreGroupLevelOpenImage = MIDGraphics.ImageIndex(MIDGraphics.DefaultOpenFolder);
                cStoreGroupLevelClosedImage = MIDGraphics.ImageIndexWithDefault("blue", MIDGraphics.ClosedFolder);
                cStoreGroupLevelOpenImage = MIDGraphics.ImageIndexWithDefault("blue", MIDGraphics.OpenFolder);
                // End TT#717
                cStoreDynamicUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.StoreDynamic);
                cStoreStaticUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.StoreStatic);
                cStoreSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.StoreSelected);

                //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
                //cAvailableStoresText = MIDText.GetTextOnly(eMIDTextCode.lbl_AvailableStores);

                //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
                //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //_storeGroupSecLvlHash = new Hashtable();
                //_storeGroupSecLvlHash[SAB.ClientServerSession.UserRID] = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                //_storeGroupSecLvlHash[Include.GlobalUserRID] = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesGlobal);

                //_storeSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStores);
                //_storeCharSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresCharacteristics);
                _storeGroupSecGrpHash = new Hashtable();

                _storeGroupSecGrpHash[SAB.ClientServerSession.UserRID] = new MIDTreeNodeSecurityGroup(
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser),
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersStoreFoldersUser));

                _storeGroupSecGrpHash[Include.GlobalUserRID] = new MIDTreeNodeSecurityGroup(
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesGlobal),
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersStoreFoldersGlobal));
                //End Track #6321 - JScott - User has ability to to create folders when security is view

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
            ProfileList storeGroupList;
            DataTable dtFolderItems = null;
            DataTable dtFolderShortcuts = null;
            DataTable dtStoreGroupShortcuts = null;
            MIDStoreNode newNode;
            FolderShortcut newShortcut;
            MIDStoreNode parentNode;
            MIDStoreNode childNode;
            DataTable dtUserFolders;
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //FunctionSecurityProfile userSecLvl;
            MIDTreeNodeSecurityGroup userSecGrp;
            //End Track #6321 - JScott - User has ability to to create folders when security is view
            MIDStoreNode userNode;

            try
            {
                Nodes.Clear();
                ItemNodeHash.Clear();
                FolderNodeHash.Clear();

                //----------------------
                // Build Faviorites node
                //----------------------

                // Begin Track #6406 - JSmith - Double-clicking an attribute generates a Null error
                //if (!UserStoreGroupSecLvl.AccessDenied)
                //{
                // End Track #6406
                dtFolders = _dlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.StoreGroupMainFavoritesFolder);

                //Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                //if (dtFolders == null || dtFolders.Rows.Count != 1)
                if (dtFolders == null || dtFolders.Rows.Count == 0)
                //End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                {
                    folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.StoreGroupMainFavoritesFolder, "My Favorites", SAB.ClientServerSession.UserRID);

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

                FavoritesNode = new MIDStoreNode(
                    SAB,
                    eTreeNodeType.MainFavoriteFolderNode,
                    folderProf,
                    folderProf.Name,
                    Include.NoRID,
                    folderProf.UserRID,
                    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                    //UserStoreGroupSecLvl,
                    // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                    //UserStoreGroupSecGrp,
                    // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                    //FavoritesSecGrp,
                    FavoritesFolderSecGrp,
                    // End TT#373
                    // End TT#42
                    //End Track #6321 - JScott - User has ability to to create folders when security is view
                    cFavoritesImage,
                    cFavoritesImage,
                    cFavoritesImage,
                    cFavoritesImage,
                    folderProf.OwnerUserRID,
                    false,
                    -2);

                FolderNodeHash[folderProf.Key] = FavoritesNode;

                Nodes.Add(FavoritesNode);
                // Begin Track #6406 - JSmith - Double-clicking an attribute generates a Null error
                //}
                // End Track #6406

                //----------------
                // Build User node
                //----------------

                if (!UserStoreGroupSecLvl.AccessDenied)
                {
                    dtFolders = _dlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.StoreGroupMainUserFolder);

                    //Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                    //if (dtFolders == null || dtFolders.Rows.Count != 1)
                    if (dtFolders == null || dtFolders.Rows.Count == 0)
                    //End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
                    {
                        folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.StoreGroupMainUserFolder, "My Attributes", SAB.ClientServerSession.UserRID);

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

                    _userNode = new MIDStoreNode(
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //UserStoreGroupSecLvl,
                        UserStoreGroupSecGrp,
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        folderProf.OwnerUserRID,
                        false,
                        -2);

                    FolderNodeHash[folderProf.Key] = _userNode;

                    Nodes.Add(_userNode);
                }

                //------------------
                // Build Global node
                //------------------

                if (!GlobalStoreGroupSecLvl.AccessDenied)
                {
                    dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.StoreGroupMainGlobalFolder);

                    if (dtFolders == null)
                    {
                        throw new Exception("Global Attributes Folder not defined");
                    }
                    else if (dtFolders.Rows.Count != 1)
                    {
                        throw new Exception("More than one Global Attributes Folder is defined");
                    }

                    folderProf = new FolderProfile(dtFolders.Rows[0]);

                    _globalNode = new MIDStoreNode(
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //GlobalStoreGroupSecLvl,
                        GlobalStoreGroupSecGrp,
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        folderProf.OwnerUserRID,
                        false,
                        -2);

                    FolderNodeHash[folderProf.Key] = _globalNode;

                    Nodes.Add(_globalNode);
                }

                //---------------------------
                // Read and Load Detail Nodes
                //---------------------------

                userRIDList = new ArrayList();

                if (!UserStoreGroupSecLvl.AccessDenied)
                {
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                if (!GlobalStoreGroupSecLvl.AccessDenied)
                {
                    userRIDList.Add(Include.GlobalUserRID);
                }

                folderTypeList = new ArrayList();
                folderTypeList.Add((int)eProfileType.StoreGroupMainFavoritesFolder);
                folderTypeList.Add((int)eProfileType.StoreGroupMainUserFolder);
                folderTypeList.Add((int)eProfileType.StoreGroupMainGlobalFolder);
                folderTypeList.Add((int)eProfileType.StoreGroupSubFolder);

                if (userRIDList.Count > 0)
                {
                    storeGroupList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, false); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.MyUserAndGlobal, false);
                    dtFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.StoreGroupSubFolder, true, false);
                    dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.StoreGroup, true, false);
                    dtStoreGroupShortcuts = _dlFolder.Folder_Shortcut_Item_Read(userRIDList, eProfileType.StoreGroup);
                    dtFolderShortcuts = _dlFolder.Folder_Shortcut_Folder_Read(userRIDList, folderTypeList);

                    dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

                    if (!UserStoreGroupSecLvl.AccessDenied)
                    {
                        BuildFolderBranch(SAB.ClientServerSession.UserRID, FavoritesNode.Profile.Key, FavoritesNode, dtFolderItems, dtFolders, storeGroupList);
                        BuildFolderBranch(SAB.ClientServerSession.UserRID, _userNode.Profile.Key, _userNode, dtFolderItems, dtFolders, storeGroupList);
                    }

                    if (!GlobalStoreGroupSecLvl.AccessDenied)
                    {
                        BuildFolderBranch(Include.GlobalUserRID, _globalNode.Profile.Key, _globalNode, dtFolderItems, dtFolders, storeGroupList);  // Issue 3806
                    }

                    foreach (DataRow row in dtStoreGroupShortcuts.Rows)
                    {
                        newShortcut = new FolderShortcut(row);

                        parentNode = (MIDStoreNode)FolderNodeHash[newShortcut.ParentFolderId];
                        childNode = (MIDStoreNode)ItemNodeHash[new HashKeyObject(newShortcut.ShortcutId, (int)eProfileType.StoreGroup)];

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

                        parentNode = (MIDStoreNode)FolderNodeHash[newShortcut.ParentFolderId];
                        childNode = (MIDStoreNode)FolderNodeHash[newShortcut.ShortcutId];

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

                dtUserFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.StoreGroupMainUserFolder, false, true);

                if (dtUserFolders.Rows.Count > 0)
                {
                    folderProf = new FolderProfile(
                        Include.NoRID,
                        SAB.ClientServerSession.UserRID,
                        eProfileType.StoreGroupMainSharedFolder,
                        MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName),
                        SAB.ClientServerSession.UserRID);

                    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                    //userSecLvl = new FunctionSecurityProfile(-1);
                    //userSecLvl.SetAllowView();
                    userSecGrp = new MIDTreeNodeSecurityGroup(new FunctionSecurityProfile(-1), new FunctionSecurityProfile(-1));
                    userSecGrp.FunctionSecurityProfile.SetAllowView();
                    userSecGrp.FolderSecurityProfile.SetAllowView();
                    //End Track #6321 - JScott - User has ability to to create folders when security is view

                    _sharedNode = new MIDStoreNode(
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
                        cClosedSharedFolderImage,
                        cClosedSharedFolderImage,
                        cOpenSharedFolderImage,
                        cOpenSharedFolderImage,
                        folderProf.OwnerUserRID,
                        false,
                        -2);

                    Nodes.Add(_sharedNode);

                    storeGroupList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MySharedOnly, false); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.MySharedOnly, false);
                    dtFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.StoreGroupSubFolder, false, true);
                    dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.StoreGroup, false, true);

                    dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

                    foreach (DataRow row in dtUserFolders.Rows)
                    {
                        folderProf = new FolderProfile(row);
                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        //folderProf.Name = DlSecurity.GetUserName(folderProf.OwnerUserRID);
                        folderProf.Name = UserNameStorage.GetUserName(folderProf.OwnerUserRID);
                        //End TT#827-MD -jsobek -Allocation Reviews Performance

                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //userSecLvl = (FunctionSecurityProfile)UserStoreGroupSecLvl.Clone();
                        //userSecLvl.SetDenyDelete();
                        //_storeGroupSecLvlHash[folderProf.OwnerUserRID] = userSecLvl;
                        userSecGrp = (MIDTreeNodeSecurityGroup)UserStoreGroupSecGrp.Clone();
                        userSecGrp.FunctionSecurityProfile.SetDenyDelete();
                        userSecGrp.FolderSecurityProfile.SetDenyDelete();
                        _storeGroupSecGrpHash[folderProf.OwnerUserRID] = userSecGrp;
                        //End Track #6321 - JScott - User has ability to to create folders when security is view

                        userNode = new MIDStoreNode(
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
                            folderProf.OwnerUserRID,
                            false,
                            -2);

                        FolderNodeHash[folderProf.Key] = userNode;

                        _sharedNode.Nodes.Add(userNode);

                        BuildFolderBranch(SAB.ClientServerSession.UserRID, userNode.Profile.Key, userNode, dtFolderItems, dtFolders, storeGroupList);
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
        /// <param name="aParentNode">
        /// The MIDTreeNode that was clicked on
        /// </param>
        /// <returns>
        /// The new node that was created.  If node is returned, it will be placed in edit mode.
        /// If node is not available or edit mode is not desired, return null.
        /// </returns>
        override protected MIDTreeNode CreateNewItem(MIDTreeNode aParentNode)
        {
            try
            {
                // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                if (StoreMgmt.StoresAdded
                    || StoreMgmt.StoreGroupsAdded)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoresOrGroupsChanged), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return null;
                }
                // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail

                int ownerUserRID = aParentNode.GetTopSourceNode().UserId;
                System.Windows.Forms.Form frmFilter = SharedRoutines.GetFilterFormForNewFilters(filterTypes.StoreGroupFilter, SAB, _EAB, ownerUserRID);
                frmFilter.MdiParent = MDIParentForm;
                frmFilter.Show();
                frmFilter.BringToFront();
                return null;
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
            MIDStoreNode newNode;
            // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
            MIDTreeNodeSecurityGroup nodeSecurityGroup;
            // End TT#373

            try
            {
                newNodeName = FindNewFolderName("New Folder", aUserId, aNode.Profile.Key, eProfileType.StoreGroupSubFolder);

                _dlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserId, eProfileType.StoreGroupSubFolder, newNodeName, aUserId);
                    newFolderProf.Key = _dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    _dlFolder.Folder_Item_Insert(aNode.Profile.Key, newFolderProf.Key, eProfileType.StoreGroupSubFolder);

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

                    newNode = new MIDStoreNode(
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
                        aUserId,
                        false,
                        -2);

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
            //Do not allow rename if Store Load API is running
            GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            if (genericEnqueueStoreLoad.DoesHaveConflicts())
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                return false;
            }


            MIDStoreNode node;
            int key;
            FolderProfile folderProf;

            try
            {
                node = (MIDStoreNode)aNode;

                switch (node.NodeProfileType)
                {
                    case eProfileType.StoreGroup:
                        // Begin TT#1928-MD - JSmith - Store Explorer allow user to rename attribute to existing name
                        //if (StoreMgmt.DoesGroupNameExist(((MIDStoreNode)node).GroupRID, aNewName, node.OwnerUserRID)) //SAB.StoreServerSession.DoesGroupNameExist(aNewName, node.OwnerUserRID))
                        //{
                        //    string msg = "An Attribute with the name you specified already exists.  Please specify a different name.";
                        //    MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return false;
                        //}
                        if (StoreMgmt.DoesGroupNameExist(aNewName, node.OwnerUserRID)) 
                        {
                            string msgText = MIDText.GetText(eMIDTextCode.msg_AttributeNameExists);
                            string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_AttributeAlreadyExists);
                            MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        // End TT#1928-MD - JSmith - Store Explorer allow user to rename attribute to existing name

                        StoreMgmt.StoreGroup_Rename(node.Profile.Key, aNewName); //SAB.StoreServerSession.RenameGroup(node.Profile.Key, aNewName);

                        ((StoreGroupProfile)aNode.Profile).Name = aNewName;

                        break;

                    case eProfileType.StoreGroupLevel:

                        if (StoreMgmt.DoesGroupLevelNameExist(((MIDStoreNode)node).GroupRID, aNewName)) //SAB.StoreServerSession.DoesGroupLevelNameExist(((MIDStoreNode)node).GroupRID, aNewName))
                        {
                            string msg = "An Attribute Set with the name you specified already exists.  Please specify a different name.";
                            MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        StoreMgmt.StoreGroupLevel_Rename(node.Profile.Key, ((StoreGroupLevelProfile)aNode.Profile).LevelVersion, aNewName); //SAB.StoreServerSession.RenameGroupLevel(node.Profile.Key, aNewName);

                        ((StoreGroupLevelProfile)aNode.Profile).Name = aNewName;

                        break;

                    case eProfileType.StoreGroupMainFavoritesFolder:
                    case eProfileType.StoreGroupMainUserFolder:
                    case eProfileType.StoreGroupSubFolder:

                        key = _dlFolder.Folder_GetKey(node.UserId, aNewName, node.ParentId, node.NodeProfileType);

                        if (key != -1)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FolderNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Rename(node.Profile.Key, aNewName);
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

                        folderProf = (FolderProfile)node.Profile;
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

        public MIDStoreNode FindStoreGroup(int sgRID)
        {
            return (MIDStoreNode)ItemNodeHash[new HashKeyObject(sgRID, (int)eProfileType.StoreGroup)];
        }
        public void SelectNode(MIDStoreNode mNode)
        {
            SelectedNode = mNode;
        }
        public void ClearSelectedNode()
        {
            SelectedNode = null;
        }
        /// <summary>
        /// Handles the filter folder for both insert and update
        /// </summary>
        /// <param name="f"></param>
		// Begin TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
		//public MIDStoreNode AfterSave(filter f, StoreGroupProfile sgProf)
        public MIDStoreNode AfterSave(filter f, StoreGroupProfile sgProf, MIDStoreNode parentNode = null)
		// End TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
        {
            //MIDStoreNode parentNode;  // TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
            MIDStoreNode nodeToReturn = null;
            try
            {
               // MIDStoreNode node = (MIDStoreNode)ItemNodeHash[sgProf.Key];
                MIDStoreNode node = (MIDStoreNode)ItemNodeHash[new HashKeyObject(sgProf.Key, (int)eProfileType.StoreGroup)];

                if (node == null)
                {
                    int ownerRID;
                    int userRID;
                    if (f.ownerUserRID == Include.GlobalUserRID)
                    {
                        ownerRID = Include.GlobalUserRID;
                        userRID = Include.GlobalUserRID;
                        //Check to see if the current selected folder is a child of the global folder
                        // Begin TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                        if (parentNode == null)
                        {
                        // End TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                            if (SelectedNode != null && SelectedNode != _globalNode && SelectedNode.isChildOf(_globalNode))
                            {
                                if (SelectedNode.isSubFolder)
                                {
                                    parentNode = (MIDStoreNode)SelectedNode;
                                }
                                else
                                {
                                    parentNode = (MIDStoreNode)SelectedNode.Parent;
                                }
                            }
                            else
                            {
                                //Use the default global folder
                                parentNode = _globalNode;
                            }
                        // Begin TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                        }
                        // End TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                    }
                    else //it is a user folder
                    {
                        ownerRID = SAB.ClientServerSession.UserRID;
                        userRID = SAB.ClientServerSession.UserRID;
                        //Check to see if the current selected folder is a child of the main user folder
                        // Begin TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                        if (parentNode == null)
                        {
                        // End TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                            if (SelectedNode != null && SelectedNode != _userNode && SelectedNode.isChildOf(_userNode))
                            {
                                if (SelectedNode.isSubFolder)
                                {
                                    parentNode = (MIDStoreNode)SelectedNode;
                                }
                                else
                                {
                                    parentNode = (MIDStoreNode)SelectedNode.Parent;
                                }
                            }
                            else
                            {
                                //Use the default user folder
                                parentNode = _userNode;
                            }
                        // Begin TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                        }
                        // End TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                    }


                    ////// update profile after Available Stores group has been added
                    ////storeGroupProf = SAB.StoreServerSession.GetStoreGroup(storeGroupProf.Key);

                    //StoreGroupProfile storeGroupProf = StoreMgmt.AddStoreGroup()


                    //save filter to folder
                    FolderDataLayer _dlFolder = new FolderDataLayer();
                    _dlFolder.OpenUpdateConnection();
                    try
                    {
                        //_dlFolder.Folder_Item_Insert(parentNode.NodeRID, f.filterRID, eProfileType.FilterStore);
                        _dlFolder.Folder_Item_Insert(parentNode.Profile.Key, sgProf.Key, eProfileType.StoreGroup);
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

                    //End TT#1424-MD -jsobek -Header Filter- Result Limit - Copy-Paste a Global filter to user that has a restricted limit after the copy to user the Result limit changes to Unrestricted.
                    node = BuildStoreGroupNode(sgProf, parentNode);
                    nodeToReturn = node;
                    SelectedNode = node;
                }
                else
                {
                    parentNode = (MIDStoreNode)node.Parent;
                    node.Profile = sgProf;

                    node.Nodes.Clear();
                    node.Nodes.Add(new MIDStoreGroupLevelNode());
                    node.ChildrenLoaded = false;
                    node.HasChildren = true;
                    node.DisplayChildren = true;
                    //node.InternalText = f.filterName;
                    //Begin TT#1424-MD -jsobek -Header Filter- Result Limit - Copy-Paste a Global filter to user that has a restricted limit after the copy to user the Result limit changes to Unrestricted.
                    //Begin TT#1423-MD -jsobek -Filters-copy/paste filter, rename copied filter and copy/paste that filter, and the diplay of the new filter reverts to original filter name
                    //StoreFilterProfile prof = (StoreFilterProfile)node.Profile;
                    //prof.Name = f.filterName;
                    //prof.IsLimited = f.isLimited;
                    //prof.ResultLimit = (int)f.resultLimit;
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
                            //UpdateNodeImage(SelectedNode, cGlobalFilterUnselectedImage, cGlobalFilterSelectedImage);
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
                            //UpdateNodeImage(SelectedNode, cUserFilterUnselectedImage, cUserFilterSelectedImage);
                            SelectedNode.InternalText = f.filterName;
                            parentNode = _userNode;
                        }
                    }

                    // Begin TT#1921-MD - JSmith - Str Attribute Name - change name select Apply, Save, & Close.  Does not update in the Store Explorer until a Refresh is done.
                    if (SelectedNode.InternalText != f.filterName)
                    {
                        SelectedNode.InternalText = f.filterName;
                    }
                    // End TT#1921-MD - JSmith - Str Attribute Name - change name select Apply, Save, & Close.  Does not update in the Store Explorer until a Refresh is done.

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
        //private void UpdateNodeImage(MIDTreeNode node, int ImageIndex, int SelectedImageIndex)
        //{
        //    node._collapsedImageIndex = ImageIndex;
        //    node._selectedCollapsedImageIndex = SelectedImageIndex;
        //    node._expandedImageIndex = ImageIndex;
        //    node._selectedExpandedImageIndex = SelectedImageIndex;
        //    node.SetCollapseImage();
        //}
        private MIDStoreNode JustMoveFilterNodeAndDoNotRename(MIDStoreNode aFromNode, MIDStoreNode aToNode)
        {
            MIDStoreNode newNode;
            //FolderProfile folderProf;
            StoreGroupProfile filterProf;
            //object[] moveArray;

            try
            {


                if (aFromNode.NodeProfileType == eProfileType.StoreGroup)
                {
                    filterProf = (StoreGroupProfile)aFromNode.Profile;

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
                    newNode = BuildStoreGroupNode(filterProf, aToNode);
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
            MIDStoreNode toNode;
            MIDStoreNode newNode = null;

            try
            {
                BeginUpdate();

                try
                {
                    switch (aToNode.NodeProfileType)
                    {
                        case eProfileType.StoreGroup:

                            toNode = (MIDStoreNode)aToNode.Parent;
                            break;

                        default:

                            toNode = (MIDStoreNode)aToNode;
                            break;
                    }

                    if (_dlFolder.Folder_Shortcut_Exists(toNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType))
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Cursor.Current = Cursors.WaitCursor;

                    switch (aFromNode.NodeProfileType)
                    {
                        case eProfileType.StoreGroupSubFolder:

                            newNode = BuildRootShortcutNode((MIDStoreNode)aFromNode, toNode);

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

                        case eProfileType.StoreGroup:

                            newNode = BuildObjectShortcutNode((MIDStoreNode)aFromNode, toNode);

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
            catch (Exception error)
            {
                string message = error.ToString();
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
            MIDStoreNode toNode;

            try
            {
                switch (aToNode.NodeProfileType)
                {
                    case eProfileType.Store:

                        toNode = (MIDStoreNode)aToNode.Parent;
                        break;

                    case eProfileType.StoreGroupLevel:

                        toNode = (MIDStoreNode)aToNode;
                        break;

                    case eProfileType.StoreGroup:

                        if (aFromNode.NodeProfileType == eProfileType.StoreGroupLevel)
                        {
                            toNode = (MIDStoreNode)aToNode;
                        }
                        else
                        {
                            toNode = (MIDStoreNode)aToNode.Parent;
                        }

                        break;

                    default:

                        toNode = (MIDStoreNode)aToNode;
                        break;
                }

                try
                {
                    return MoveStoreNode((MIDStoreNode)aFromNode, toNode);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    // Begin TT#3371 - JSmith - Attribute Set Order is not being honored
                    if (aFromNode.NodeProfileType == eProfileType.StoreGroupLevel)
                    {
                        SAB.ApplicationServerSession.RemoveProfileList(eProfileType.StoreGroup);
                        SAB.ApplicationServerSession.RemoveProfileList(eProfileType.StoreGroupListView);
                    }
                    // End TT#3371 - JSmith - Attribute Set Order is not being honored
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
            MIDStoreNode toNode;

            try
            {
                switch (aToNode.NodeProfileType)
                {
                    case eProfileType.Store:

                        toNode = (MIDStoreNode)aToNode.Parent;
                        break;

                    case eProfileType.StoreGroupLevel:

                        toNode = (MIDStoreNode)aToNode;
                        break;

                    case eProfileType.StoreGroup:

                        if (aFromNode.NodeProfileType == eProfileType.StoreGroupLevel)
                        {
                            toNode = (MIDStoreNode)aToNode;
                        }
                        else
                        {
                            toNode = (MIDStoreNode)aToNode.Parent;
                        }

                        break;

                    default:

                        toNode = (MIDStoreNode)aToNode;
                        break;
                }

                try
                {
                    return CopyStoreNode((MIDStoreNode)aFromNode, toNode, aFindUniqueName);
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
                    InUseStoreNode(aNode);
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
                    ////BEGIN TT#110-MD-VStuart - In Use Tool
                    //var allowDelete = false;
                    //var _nodeArrayList = new ArrayList();
                    //_nodeArrayList.Add(aNode.NodeRID);
                    //var _eProfileType = new eProfileType();
                    //_eProfileType = aNode.NodeProfileType;
                    //string inUseTitle = Regex.Replace(_eProfileType.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                    //DisplayInUseForm(_nodeArrayList, _eProfileType, inUseTitle, false, out allowDelete);
                    //if (!allowDelete)
                    ////END TT#110-MD-VStuart - In Use Tool
                    DeleteStoreNode((MIDStoreNode)aNode);
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
            try
            {
                if (aNode != null)
                {
                    MIDStoreNode node = (MIDStoreNode)aNode;

                    if (aNode.NodeProfileType == eProfileType.Store)
                    {
                        StoreProfileMaintForm storeProfMaint = GetStoreProfileMaintWindow();

                        if (storeProfMaint == null || storeProfMaint.IsDisposed)
                        {
                            storeProfMaint = new StoreProfileMaintForm(SAB, _EAB);
                            storeProfMaint.MdiParent = MDIParentForm;
                            storeProfMaint.Anchor = AnchorStyles.Right;
                            storeProfMaint.Dock = DockStyle.Fill;
                            storeProfMaint.LoadStores();
                        }

                        storeProfMaint.Show();

                        storeProfMaint.ShowStore(node.Profile.Key);
                        storeProfMaint.BringToFront();
                    }
                    else if (aNode.NodeProfileType == eProfileType.StoreGroup)  //TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                    {
                 
                        if ( node.Profile.Key != Include.AllStoreGroupRID)
                        {
                            // Begin TT#5783 - JSmith - Fill Size Error - System Error
                            // Insure Attribute and store information is current before allowing maintenance
                            // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                            //StoreMgmt.LoadInitialStoresAndGroups(SAB: SAB, session: SAB.ClientServerSession, bLoadInactiveGroups: false, bDoingRefresh: true);
                            // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                            // End TT#5783 - JSmith - Fill Size Error - System Error

                            StoreGroupProfile groupProf = (StoreGroupProfile)node.Profile;
                            int filterRID = groupProf.FilterRID;

                            //frmFilterBuilder frmFilter = SharedRoutines.GetFilterFormForExistingFilter(filterRID, SAB, _EAB, false, false);
                            //frmFilter.MdiParent = MDIParentForm;
                            //frmFilter.Show();
                            //frmFilter.BringToFront();
                            // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                            //ShowFormForExistingFilter(filterRID, aNode, groupProf);
                            if (StoreMgmt.StoresAdded
                                || StoreMgmt.StoreGroupsAdded)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoresOrGroupsChanged), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                            {
                                ShowFormForExistingFilter(filterRID, aNode, groupProf);
                            }
                            // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                        }
                  
                    }
                    else if (aNode.NodeProfileType == eProfileType.StoreGroupLevel)  //TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                    {

                        if (((MIDStoreGroupLevelNode)node).Sequence != int.MaxValue && node.Profile.Key != Include.AllStoreGroupLevelRID)
                        {
                            // Begin TT#5783 - JSmith - Fill Size Error - System Error
                            // Insure Attribute and store information is current before allowing maintenance
                            // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                            //StoreMgmt.LoadInitialStoresAndGroups(SAB: SAB, session: SAB.ClientServerSession, bLoadInactiveGroups: false, bDoingRefresh: true);
                            // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                            // End TT#5783 - JSmith - Fill Size Error - System Error

                            StoreGroupLevelProfile levelProf = (StoreGroupLevelProfile)node.Profile;

                            StoreGroupProfile groupProf = StoreMgmt.StoreGroup_Get(levelProf.GroupRid);

                            int filterRID = groupProf.FilterRID;

                            //frmFilterBuilder frmFilter = SharedRoutines.GetFilterFormForExistingFilter(filterRID, SAB, _EAB, false, false);
                            //frmFilter.MdiParent = MDIParentForm;
                            //frmFilter.SelectCondition(levelProf.Sequence);
                            //frmFilter.Show();
                            //frmFilter.BringToFront();
                            // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                            //ShowFormForExistingFilter(filterRID, aNode, groupProf, levelProf.Key, levelProf.Name);
                            if (StoreMgmt.StoresAdded
                                || StoreMgmt.StoreGroupsAdded)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoresOrGroupsChanged), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                            {
                                ShowFormForExistingFilter(filterRID, aNode, groupProf, levelProf.Key, levelProf.Name);
                            }
                            // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
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

        private void ShowFormForExistingFilter(int filterRID, MIDTreeNode node, StoreGroupProfile groupProf, int levelRID = -1, string levelName = "")
        {
            bool doesFormExist = false;
            frmFilterBuilder frmFilter = SharedRoutines.DoesFilterFormExist(filterRID, ref doesFormExist, _EAB);
            if (doesFormExist == false)
            {
                GenericEnqueue objEnqueue = EnqueueObject(groupProf, true);
                if (objEnqueue != null)
                {

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



                    frmFilter = SharedRoutines.GetFilterFormForExistingFilter(filterRID, SAB, _EAB, isReadOnly, false, groupProf);

                    OnFilterPropertiesCloseClass closeHandler = new OnFilterPropertiesCloseClass(objEnqueue);

                    frmFilter.OnFilterPropertiesCloseHandler += new frmFilterBuilder.FilterPropertiesCloseEventHandler(closeHandler.OnClose);
                    frmFilter.MdiParent = MDIParentForm;
                    frmFilter.Show();
                    frmFilter.BringToFront();
                }
            }
            else
            {
                frmFilter.BringToFront();
            }
            if (levelRID != -1 || levelName != "")
            {
                frmFilter.SelectCondition(levelRID, levelName);
            }
        }
        private GenericEnqueue EnqueueObject(StoreGroupProfile groupProf, bool aAllowReadOnly)
        {
            GenericEnqueue objEnqueue;
            string errMsg;

            try
            {
                objEnqueue = new GenericEnqueue(eLockType.StoreGroup, groupProf.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

                try
                {
                    objEnqueue.EnqueueGeneric();
                }
                catch (GenericConflictException)
                {
                    /* Begin TT#1159 - Improve Messaging */
                    string[] errParms = new string[3];
                    errParms.SetValue("Attribute", 0);
                    errParms.SetValue(groupProf.Name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                    //errMsg = "The Filter \"" + aFilterProf.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
                    /* End TT#1159 - Improve Messaging */

                    if (aAllowReadOnly)
                    {
                        errMsg += System.Environment.NewLine + System.Environment.NewLine;
                        errMsg += "Do you wish to continue with the Attribute as read-only?";

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
                //if (aStartNode.ChildrenLoaded)
                //{
                if (aStartNode.ChildrenLoaded &&
                    aChangedNode != null)
                {
                    // End TT#62
                    foreach (MIDStoreNode node in aStartNode.Nodes)
                    {
                        if (node.Profile.Key == aChangedNode.Profile.Key && node.NodeProfileType == aChangedNode.NodeProfileType)
                        {
                            node.RefreshShortcutNode(aChangedNode);

                            if (node.NodeProfileType == eProfileType.StoreGroup)
                            {
                                if (node.isObjectShortcut)
                                {
                                    //Begin Track #6201 - JScott - Store Count removed from attr sets
                                    //node.Text = ((StoreGroupProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                                    node.InternalText = ((StoreGroupProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                                    //End Track #6201 - JScott - Store Count removed from attr sets
                                    RefreshStoreGroup(node);
                                }
                                else if (node.isChildShortcut)
                                {
                                    //Begin Track #6201 - JScott - Store Count removed from attr sets
                                    //node.Text = ((StoreGroupProfile)aChangedNode.Profile).Name;
                                    node.InternalText = ((StoreGroupProfile)aChangedNode.Profile).Name;
                                    //End Track #6201 - JScott - Store Count removed from attr sets
                                    RefreshStoreGroup(node);
                                }
                            }
                            else if (node.NodeProfileType == eProfileType.StoreGroupSubFolder)
                            {
                                if (node.isFolderShortcut)
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
                        else if (node.isSubFolder || node.isFolderShortcut)
                        {
                            RefreshShortcuts(node, aChangedNode);
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
            return aClipboardDataType == eProfileType.StoreGroup ||
                    aClipboardDataType == eProfileType.StoreGroupLevel ||
                    aClipboardDataType == eProfileType.Store;
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
                if (aSelectedNodes.ClipboardDataType == eProfileType.StoreGroup ||
                    aSelectedNodes.ClipboardDataType == eProfileType.StoreGroupLevel ||
                    aSelectedNodes.ClipboardDataType == eProfileType.Store ||
                    aSelectedNodes.ClipboardDataType == eProfileType.StoreGroupSubFolder)
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
        private bool InUseStoreNode(MIDTreeNode aNode)
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
            if (display == true
                && MIDEnvironment.isWindows)
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
            if (showDialog == true
                && MIDEnvironment.isWindows)
            { myfrm.ShowDialog(); }
        }
        //END TT#110-MD-VStuart - In Use Tool

        //--------------
        //PUBLIC METHODS
        //--------------

        public void InitialExpand()
        {
            try
            {
                if (FavoritesNode != null)
                {
                    FavoritesNode.Expand();
                }

                if (_userNode != null)
                {
                    _userNode.Expand();
                }

                if (_globalNode != null)
                {
                    _globalNode.Expand();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDStoreNode BuildStoreGroupLevelNode(StoreGroupLevelProfile aStoreGroupLevelProf, MIDTreeNode aParentNode)
        {
            //Begin TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
            MIDTreeNodeSecurityGroup grpLvlSecGrp;
            //End TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
            MIDStoreNode newNode;

            try
            {
                //Begin TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
                grpLvlSecGrp = (MIDTreeNodeSecurityGroup)aParentNode.NodeSecurityGroup.Clone();

                // Begin TT#3898 - JSmith - Do not allow the "All Store" Attribute or "All Stores Set" within the attribute to be deleted.
                //if (aStoreGroupLevelProf.Sequence == int.MaxValue)
                if (aStoreGroupLevelProf.Sequence == int.MaxValue ||
                    aStoreGroupLevelProf.LevelType == eGroupLevelTypes.AvailableStoreSet ||
                    aStoreGroupLevelProf.Key == Include.AllStoreGroupLevelRID)
                // End TT#3898 - JSmith - Do not allow the "All Store" Attribute or "All Stores Set" within the attribute to be deleted.
                {
                    // Begin TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                    //grpLvlSecGrp.FunctionSecurityProfile.SetDenyDelete();
                    //grpLvlSecGrp.FolderSecurityProfile.SetDenyDelete();
                    grpLvlSecGrp.FunctionSecurityProfile.SetReadOnly();
                    grpLvlSecGrp.FolderSecurityProfile.SetReadOnly();
                    // End TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                }

                grpLvlSecGrp.FolderSecurityProfile.SetReadOnly();  // TT#4328 - JSmith - New Folder is not valid for attribute set

                //End TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
                newNode = new MIDStoreGroupLevelNode(
                    SAB,
                    eTreeNodeType.ObjectNode,
                    aStoreGroupLevelProf,
                    aStoreGroupLevelProf.Name,
                    aParentNode.NodeRID,
                    aParentNode.UserId,
                    aStoreGroupLevelProf.Sequence,
                    //Begin TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
                    ////Begin Track #6321 - JScott - User has ability to to create folders when security is view
                    ////aParentNode.FunctionSecurityProfile,
                    //aParentNode.NodeSecurityGroup,
                    ////End Track #6321 - JScott - User has ability to to create folders when security is view
                    grpLvlSecGrp,
                    //End TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
                    cStoreGroupLevelClosedImage,
                    cStoreGroupLevelClosedImage,
                    cStoreGroupLevelOpenImage,
                    cStoreGroupLevelOpenImage,
                    aParentNode.OwnerUserRID,
                    ((MIDStoreNode)aParentNode).isDynamic,
                    ((MIDStoreNode)aParentNode).FilterRID);

                //ItemNodeHash[new HashKeyObject(aStoreGroupLevelProf.Key, (int)eProfileType.StoreGroupLevel)] = newNode;
                aParentNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDStoreNode BuildStoreNode(StoreProfile aStoreProf, MIDTreeNode aParentNode)
        {
            MIDStoreNode newNode;

            try
            {
                //if (aStoreProf.DynamicStore)
                //{
                //    newNode = new MIDStoreNode(
                //        SAB,
                //        eTreeNodeType.ObjectNode,
                //        aStoreProf,
                //        aStoreProf.Text,
                //        aParentNode.NodeRID,
                //        aParentNode.UserId,
                //        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //        //aParentNode.FunctionSecurityProfile,
                //        aParentNode.NodeSecurityGroup,
                //        //End Track #6321 - JScott - User has ability to to create folders when security is view
                //        cStoreDynamicUnselectedImage,
                //        cStoreSelectedImage,
                //        aParentNode.OwnerUserRID,
                //        false,
                //        -2);
                //}
                //else
                //{
                    newNode = new MIDStoreNode(
                        SAB,
                        eTreeNodeType.ObjectNode,
                        aStoreProf,
                        aStoreProf.Text,
                        aParentNode.NodeRID,
                        aParentNode.UserId,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //aParentNode.FunctionSecurityProfile,
                        aParentNode.NodeSecurityGroup,
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cStoreStaticUnselectedImage,
                        cStoreSelectedImage,
                        aParentNode.OwnerUserRID,
                        false,
                        -2);
                //}

                //ItemNodeHash[new HashKeyObject(aStoreProf.Key, (int)eProfileType.Store)] = newNode;
                aParentNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CreateShortcutChildren(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDStoreNode newNode = null;

            try
            {
                if (aFromNode.ChildrenLoaded)
                {
                    foreach (MIDStoreNode node in aFromNode.Nodes)
                    {
                        newNode = (MIDStoreNode)node.Clone();
                        newNode.TreeNodeType = eTreeNodeType.ChildObjectShortcutNode;

                        aToNode.Nodes.Add(newNode);

                        CreateShortcutChildren(node, newNode);
                    }
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

                foreach (MIDStoreNode node in deleteList)
                {
                    if (node.Profile.Key == aDeleteNode.Profile.Key && node.NodeProfileType == aDeleteNode.NodeProfileType &&
                        node.isShortcut)
                    {
                        DeleteChildNodes(node);
                        node.Remove();
                    }
                    else if (node.NodeProfileType == eProfileType.StoreGroupSubFolder ||
                        node.isFolderShortcut)
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

        public void AddStoreGroup(StoreGroupProfile aStoreGroupProf, int aParentFolderKey)
        {
            try
            {
                BuildStoreGroupNode(aStoreGroupProf, (MIDTreeNode)FolderNodeHash[aParentFolderKey]);
            }
            catch
            {
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
        //private FunctionSecurityProfile GetUserStoreGroupSecLvl(int aUserRID)
        //{
        //    return (FunctionSecurityProfile)_storeGroupSecLvlHash[aUserRID];
        //}
        private MIDTreeNodeSecurityGroup GetUserStoreGroupSecGrp(int aUserRID)
        {
            return (MIDTreeNodeSecurityGroup)_storeGroupSecGrpHash[aUserRID];
        }
        //End Track #6321 - JScott - User has ability to to create folders when security is view

        private void BuildFolderBranch(int aUserRID, int aParentFolderRID, MIDTreeNode aParentNode, DataTable aFolderItems, DataTable aFolders, ProfileList aStoreGroups)
        {
            DataRow[] folderItemList;
            DataRow itemRow;

            FolderProfile folderProf;
            StoreGroupProfile storeGroupProf;
            MIDStoreNode newNode;

            try
            {
                folderItemList = aFolderItems.Select("USER_RID = " + aUserRID + " AND PARENT_FOLDER_RID = " + aParentFolderRID);

                foreach (DataRow row in folderItemList)
                {
                    switch ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"]))
                    {
                        case eProfileType.StoreGroupSubFolder:

                            itemRow = aFolders.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Store", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            folderProf = new FolderProfile(itemRow);
                            newNode = BuildSubFolderNode(folderProf, aParentNode);
                            BuildFolderBranch(aUserRID, newNode.Profile.Key, newNode, aFolderItems, aFolders, aStoreGroups);
                            break;

                        default:

                            storeGroupProf = (StoreGroupProfile)aStoreGroups.FindKey(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (storeGroupProf == null)
                            {
                                //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Store", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            BuildStoreGroupNode(storeGroupProf, aParentNode);
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

        private MIDStoreNode BuildSubFolderNode(FolderProfile aFolderProf, MIDTreeNode aParentNode)
        {
            MIDStoreNode newNode;

            try
            {
                // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                //newNode = new MIDStoreNode(
                //        SAB,
                //        eTreeNodeType.SubFolderNode,
                //        aFolderProf,
                //        aFolderProf.Name,
                //        aParentNode.NodeRID,
                //        aFolderProf.UserRID,
                //    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //    //GetUserStoreGroupSecLvl(aFolderProf.OwnerUserRID),
                //        GetUserStoreGroupSecGrp(aFolderProf.OwnerUserRID),
                //    //End Track #6321 - JScott - User has ability to to create folders when security is view
                //        cClosedFolderImage,
                //        cClosedFolderImage,
                //        cOpenFolderImage,
                //        cOpenFolderImage,
                //        aFolderProf.OwnerUserRID,
                //        ((MIDStoreNode)aParentNode).isDynamic);
                if (aParentNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    newNode = new MIDStoreNode(
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
                        aFolderProf.OwnerUserRID,
                        ((MIDStoreNode)aParentNode).isDynamic,
                        ((MIDStoreNode)aParentNode).FilterRID);
                }
                else
                {
                    newNode = new MIDStoreNode(
                        SAB,
                        eTreeNodeType.SubFolderNode,
                        aFolderProf,
                        aFolderProf.Name,
                        aParentNode.NodeRID,
                        aFolderProf.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //GetUserStoreGroupSecLvl(aFolderProf.OwnerUserRID),
                        GetUserStoreGroupSecGrp(aFolderProf.OwnerUserRID),
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        aFolderProf.OwnerUserRID,
                        ((MIDStoreNode)aParentNode).isDynamic,
                        ((MIDStoreNode)aParentNode).FilterRID);
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

        private MIDStoreNode BuildStoreGroupNode(StoreGroupProfile aStoreGroupProf, MIDTreeNode aParentNode)
        {
            MIDStoreNode newNode;

            try
            {
                if (aStoreGroupProf.OwnerUserRID == Include.GlobalUserRID)
                {
                    if (aStoreGroupProf.IsDynamicGroup)
                    {
                        newNode = new MIDStoreGroupNode(
                            SAB,
                            eTreeNodeType.ObjectNode,
                            aStoreGroupProf,
                            aStoreGroupProf.Name,
                            aParentNode.NodeRID,
                            aStoreGroupProf.OwnerUserRID,
                            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                            //GetUserStoreGroupSecLvl(aStoreGroupProf.OwnerUserRID),
                            GetUserStoreGroupSecGrp(aStoreGroupProf.OwnerUserRID),
                            //End Track #6321 - JScott - User has ability to to create folders when security is view
                            cGlobalStoreGroupDynamicClosedImage,
                            cGlobalStoreGroupDynamicClosedImage,
                            cGlobalStoreGroupDynamicOpenImage,
                            cGlobalStoreGroupDynamicOpenImage,
                            aStoreGroupProf.OwnerUserRID,
                            aStoreGroupProf.IsDynamicGroup,
                            aStoreGroupProf.FilterRID);
                    }
                    else
                    {
                        newNode = new MIDStoreGroupNode(
                            SAB,
                            eTreeNodeType.ObjectNode,
                            aStoreGroupProf,
                            aStoreGroupProf.Name,
                            aParentNode.NodeRID,
                            aStoreGroupProf.OwnerUserRID,
                            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                            //GetUserStoreGroupSecLvl(aStoreGroupProf.OwnerUserRID),
                            GetUserStoreGroupSecGrp(aStoreGroupProf.OwnerUserRID),
                            //End Track #6321 - JScott - User has ability to to create folders when security is view
                            cGlobalStoreGroupStaticClosedImage,
                            cGlobalStoreGroupStaticClosedImage,
                            cGlobalStoreGroupStaticOpenImage,
                            cGlobalStoreGroupStaticOpenImage,
                            aStoreGroupProf.OwnerUserRID,
                            aStoreGroupProf.IsDynamicGroup,
                            aStoreGroupProf.FilterRID);
                    }
                }
                else
                {
                    newNode = new MIDStoreGroupNode(
                        SAB,
                        eTreeNodeType.ObjectNode,
                        aStoreGroupProf,
                        aStoreGroupProf.Name,
                        aParentNode.NodeRID,
                        SAB.ClientServerSession.UserRID,
                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                        //GetUserStoreGroupSecLvl(SAB.ClientServerSession.UserRID),
                        GetUserStoreGroupSecGrp(SAB.ClientServerSession.UserRID),
                        //End Track #6321 - JScott - User has ability to to create folders when security is view
                        cUserStoreGroupImage,
                        cUserStoreGroupImage,
                        cUserStoreGroupImage,
                        cUserStoreGroupImage,
                        aStoreGroupProf.OwnerUserRID,
                        aStoreGroupProf.IsDynamicGroup,
                        aStoreGroupProf.FilterRID);
                }

                // Begin TT#3898 - JSmith - Do not allow the "All Store" Attribute or "All Stores Set" within the attribute to be deleted.
                if (newNode.Profile.Key == Include.AllStoreGroupRID)
                {
                    // clone security class or sets all nodes by reference
                    newNode.NodeSecurityGroup = (MIDTreeNodeSecurityGroup)newNode.NodeSecurityGroup.Clone();
                    // Begin TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                    //newNode.FunctionSecurityProfile.SetDenyDelete();
                    //newNode.FolderSecurityProfile.SetDenyDelete();
                    newNode.FunctionSecurityProfile.SetReadOnly();
                    newNode.FunctionSecurityProfile.SetAllowMove();
                    newNode.FolderSecurityProfile.SetReadOnly();
                    // End TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                }
                // End TT#3898 - JSmith - Do not allow the "All Store" Attribute or "All Stores Set" within the attribute to be deleted.

                ItemNodeHash[new HashKeyObject(aStoreGroupProf.Key, (int)eProfileType.StoreGroup)] = newNode;
                aParentNode.Nodes.Add(newNode);

                if (aStoreGroupProf.GetGroupLevelList(false).Count > 0)
                {
                    newNode.Nodes.Add(new MIDStoreGroupLevelNode());
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDStoreNode BuildRootShortcutNode(MIDStoreNode aFromNode, MIDStoreNode aToNode)
        {
            MIDStoreNode newNode;
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

                newNode = new MIDStoreNode(
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
                    aFromNode.OwnerUserRID,
                    aFromNode.isDynamic,
                    aFromNode.FilterRID);

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

        private MIDStoreNode BuildObjectShortcutNode(MIDStoreNode aFromNode, MIDStoreNode aToNode)
        {
            MIDStoreNode newNode;
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

                if (aFromNode.OwnerUserRID == Include.GlobalUserRID)
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
                    if (aFromNode.isDynamic)
                    {
                        newNode = new MIDStoreNode(
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
                            cGlobalStoreGroupDynamicClosedShortcutImage,
                            cGlobalStoreGroupDynamicClosedShortcutImage,
                            cGlobalStoreGroupDynamicOpenShortcutImage,
                            cGlobalStoreGroupDynamicOpenShortcutImage,
                            aFromNode.OwnerUserRID,
                            aFromNode.isDynamic,
                            aFromNode.FilterRID);
                    }
                    else
                    {
                        newNode = new MIDStoreNode(
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
                            cGlobalStoreGroupStaticClosedShortcutImage,
                            cGlobalStoreGroupStaticClosedShortcutImage,
                            cGlobalStoreGroupStaticOpenShortcutImage,
                            cGlobalStoreGroupStaticOpenShortcutImage,
                            aFromNode.OwnerUserRID,
                            aFromNode.isDynamic,
                            aFromNode.FilterRID);
                    }

                    // Begin TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                    // Allow All Stores shortcut to be removed from My Favorites
                    if (aFromNode.Profile.Key == Include.AllStoreGroupRID &&
                        aToNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                    {
                        newNode.FunctionSecurityProfile.SetAllowDelete();
                    }
                    // End TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
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
                    newNode = new MIDStoreNode(
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
                        cUserStoreGroupShortcutImage,
                        cUserStoreGroupShortcutImage,
                        cUserStoreGroupShortcutImage,
                        cUserStoreGroupShortcutImage,
                        aFromNode.OwnerUserRID,
                        aFromNode.isDynamic,
                        aFromNode.FilterRID);
                }

                aToNode.Nodes.Add(newNode);

                if (aFromNode.Nodes.Count > 0)
                {
                    newNode.Nodes.Add(new MIDStoreGroupLevelNode());
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //private GenericEnqueue EnqueueObject(StoreGroupProfile aProfile, eLockType aLockType, bool aAllowReadOnly)
        //{
        //    GenericEnqueue objEnqueue;
        //    string errMsg;

        //    try
        //    {
        //        objEnqueue = new GenericEnqueue(aLockType, aProfile.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

        //        try
        //        {
        //            objEnqueue.EnqueueGeneric();
        //        }
        //        catch (GenericConflictException)
        //        {
        //            /* Begin TT#1159 - Improve Messaging */
        //            string[] errParms = new string[3];
        //            errParms.SetValue("Store Group", 0);
        //            errParms.SetValue(aProfile.Name.Trim(), 1);
        //            errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
        //            errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

        //            //errMsg = "The Store Group \"" + aProfile.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
        //            /* End TT#1159 - Improve Messaging */



        //            if (aAllowReadOnly)
        //            {
        //                errMsg += System.Environment.NewLine + System.Environment.NewLine;
        //                errMsg += "Do you wish to continue with the Store Group as read-only?";

        //                if (MessageBox.Show(errMsg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //                {
        //                    objEnqueue = null;
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show(errMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //                objEnqueue = null;
        //            }
        //        }

        //        return objEnqueue;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private GenericEnqueue EnqueueObject(StoreGroupLevelProfile aProfile, eLockType aLockType, bool aAllowReadOnly)
        //{
        //    GenericEnqueue objEnqueue;
        //    string errMsg;

        //    try
        //    {
        //        objEnqueue = new GenericEnqueue(aLockType, aProfile.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

        //        try
        //        {
        //            objEnqueue.EnqueueGeneric();
        //        }
        //        catch (GenericConflictException)
        //        {
        //            /* Begin TT#1159 - Improve Messaging */
        //            string[] errParms = new string[3];
        //            errParms.SetValue("Store Group Level", 0);
        //            errParms.SetValue(aProfile.Name.Trim(), 1);
        //            errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
        //            errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

        //            //errMsg = "The Store Group Level \"" + aProfile.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
        //            /* End TT#1159 */

        //            if (aAllowReadOnly)
        //            {
        //                errMsg += System.Environment.NewLine + System.Environment.NewLine;
        //                errMsg += "Do you wish to continue with the Store Group Level as read-only?";

        //                if (MessageBox.Show(errMsg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //                {
        //                    objEnqueue = null;
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show(errMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //                objEnqueue = null;
        //            }
        //        }

        //        return objEnqueue;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private GenericEnqueue EnqueueObject(StoreProfile aProfile, eLockType aLockType, bool aAllowReadOnly)
        //{
        //    GenericEnqueue objEnqueue;
        //    string errMsg;

        //    try
        //    {
        //        objEnqueue = new GenericEnqueue(aLockType, aProfile.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

        //        try
        //        {
        //            objEnqueue.EnqueueGeneric();
        //        }
        //        catch (GenericConflictException)
        //        {
        //            /* Begin TT#1159 - Improve Messaging */
        //            string[] errParms = new string[3];
        //            errParms.SetValue("Store", 0);
        //            errParms.SetValue(aProfile.Text.Trim(), 1);
        //            errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
        //            errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

        //            //errMsg = "The Store \"" + aProfile.Text + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
        //            /* End TT#1159*/

        //            if (aAllowReadOnly)
        //            {
        //                errMsg += System.Environment.NewLine + System.Environment.NewLine;
        //                errMsg += "Do you wish to continue with the Store as read-only?";

        //                if (MessageBox.Show(errMsg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //                {
        //                    objEnqueue = null;
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show(errMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //                objEnqueue = null;
        //            }
        //        }

        //        return objEnqueue;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        private StoreProfileMaintForm GetStoreProfileMaintWindow()
        {
            try
            {
                foreach (Form childForm in MDIParentForm.MdiChildren)
                {
                    if (childForm.GetType() == typeof(StoreProfileMaintForm))
                    {
                        return (StoreProfileMaintForm)childForm;
                    }
                }

                return null;
            }
            catch (Exception error)
            {
                string message = error.ToString();
                throw;
            }
        }

        private string FindNewStoreGroupName(string aStoreGroupName, int aUserRID)
        {
            int index;
            string newName;

            try
            {
                index = 1;
                newName = aStoreGroupName;

                while (StoreMgmt.DoesGroupNameExist(newName, aUserRID)) //SAB.StoreServerSession.DoesGroupNameExist(newName, aUserRID))
                {
                    index++;
                    newName = aStoreGroupName + " (" + index + ")";
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private string FindNewStoreGroupLevelName(int aGroupRID, string aStoreGroupLevelName)
        {
            int index;
            string newName;

            try
            {
                index = 1;
                newName = aStoreGroupLevelName;

                while (StoreMgmt.DoesGroupLevelNameExist(aGroupRID, newName)) //SAB.StoreServerSession.DoesGroupLevelNameExist(aGroupRID, newName))
                {
                    index++;
                    newName = aStoreGroupLevelName + " (" + index + ")";
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDStoreNode MoveStoreNode(MIDStoreNode aFromNode, MIDStoreNode aToNode)
        {
            //Do not allow moving if Store Load API is running
            GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            if (genericEnqueueStoreLoad.DoesHaveConflicts())
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                return null;
            }



            MIDStoreNode parentNode;
            MIDStoreNode newNode;
            FolderProfile folderProf;
            int i, j;
            MIDStoreGroupLevelNode childNode;
            StoreGroupProfile strGrpProf;
            object[] moveArray;
            //Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
            MIDStoreNode groupNode;
            eProfileType lastNodeType;
            int lastNodeKey;
            //End Track #6199 - JScott - Create attr w/3 sets - move str results in err

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
                        _dlFolder.Folder_Shortcut_Delete(aFromNode.ParentId, aFromNode.Profile.Key, aFromNode.NodeProfileType);
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
                else if (aFromNode.NodeProfileType == eProfileType.StoreGroupSubFolder)
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

                        folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.StoreGroupSubFolder);
                        folderProf.UserRID = aToNode.UserId;
                        folderProf.OwnerUserRID = aToNode.UserId;
                    }

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Update(folderProf.Key, folderProf.UserRID, folderProf.Name, folderProf.ProfileType);
                        _dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.NodeProfileType);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType);

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

                    foreach (MIDStoreNode node in moveArray)
                    {
                        MoveStoreNode(node, newNode);
                    }

                    aFromNode.Remove();

                    return newNode;
                }
                else if (aFromNode.NodeProfileType == eProfileType.Store)
                {
                    //SAB.StoreServerSession.MoveStore(aFromNode.Profile.Key, ((MIDStoreNode)aFromNode.Parent).Profile.Key, aToNode.Profile.Key);

                    ////Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
                    ////aFromNode.Remove();
                    ////newNode = BuildStoreNode((StoreProfile)aFromNode.Profile, aToNode);
                    ////return newNode;
                    //lastNodeType = aFromNode.Profile.ProfileType;
                    //lastNodeKey = aFromNode.Profile.Key;
                    //groupNode = aFromNode.GetStoreGroupNode();

                    //SAB.StoreServerSession.RefreshStoresInGroup(groupNode.Profile.Key);
                    //RefreshStoreGroup(groupNode);

                    //return (MIDStoreNode)FindTreeNode(groupNode.Nodes, lastNodeType, lastNodeKey);
                    ////End Track #6199 - JScott - Create attr w/3 sets - move str results in err
                    return null;
                }
                else if (aFromNode.NodeProfileType == eProfileType.StoreGroupLevel)
                {
                    parentNode = (MIDStoreNode)aFromNode.Parent;

                    aFromNode.Remove();

                    if (aToNode.NodeProfileType == eProfileType.StoreGroup)
                    {
                        aToNode.Nodes.Insert(0, aFromNode);
                    }
                    else
                    {
                        aToNode.Parent.Nodes.Insert(aToNode.Index, aFromNode);
                    }

                    for (i = 0, j = 0; i < parentNode.Nodes.Count; i++)
                    {
                        childNode = (MIDStoreGroupLevelNode)parentNode.Nodes[i];

                        if (childNode.Sequence != int.MaxValue)
                        {
                            j++;
                            childNode.Sequence = j;
                            StoreMgmt.StoreGroupLevel_UpdateSequence(childNode.Profile.Key, ((StoreGroupLevelProfile)childNode.Profile).LevelVersion, childNode.Sequence);  //SAB.StoreServerSession.UpdateGroupLevelSequence(childNode.Profile.Key, childNode.Sequence);
                        }
                    }

                    StoreMgmt.StoreGroup_SortLevels(aToNode.GetStoreGroupNode().Profile.Key); // SAB.StoreServerSession.SortGroupLevels(aToNode.GetStoreGroupNode().Profile.Key);

                    return aFromNode;
                }
                else if (aFromNode.NodeProfileType == eProfileType.StoreGroup)
                {
                    strGrpProf = StoreMgmt.StoreGroup_Get(aFromNode.Profile.Key); // SAB.StoreServerSession.GetStoreGroup(aFromNode.Profile.Key);

                    if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                    {
                        DlSecurity.OpenUpdateConnection();

                        try
                        {
                            DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(aFromNode.NodeProfileType), aFromNode.Profile.Key);
                            DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(aFromNode.NodeProfileType), aFromNode.Profile.Key, aToNode.UserId);

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

                        strGrpProf.Name = FindNewStoreGroupName(((StoreGroupProfile)aFromNode.Profile).Name, aToNode.UserId);
                        strGrpProf.OwnerUserRID = aToNode.UserId;
                        StoreMgmt.StoreGroup_UpdateIdAndUser(strGrpProf); //SAB.StoreServerSession.UpdateStoreGroup(strGrpProf);
                    }

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.NodeProfileType);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType);

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
                    newNode = BuildStoreGroupNode(strGrpProf, aToNode);
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

        private MIDStoreNode CopyStoreNode(MIDStoreNode aFromNode, MIDStoreNode aToNode, bool aFindUniqueName)
        {
            //Do not allow copying if Store Load API is running
            GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            if (genericEnqueueStoreLoad.DoesHaveConflicts())
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                return null;
            }



            FolderProfile folderProf;
            MIDStoreNode newNode;
            string newName;
            StoreGroupProfile strGrpProf;

            try
            {
                if (aFromNode.isSubFolder)
                {
                    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                    folderProf.UserRID = aToNode.UserId;
                    folderProf.OwnerUserRID = aToNode.UserId;

                    if (aFindUniqueName)
                    {
                        folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.StoreGroupSubFolder);
                    }

                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        folderProf.Key = _dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, eProfileType.StoreGroupSubFolder);
                        _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, folderProf.Key, eProfileType.StoreGroupSubFolder);

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

                    foreach (MIDStoreNode node in aFromNode.Nodes)
                    {
                        CopyStoreNode(node, newNode, aFindUniqueName);
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
                    if (aFromNode.NodeProfileType == eProfileType.StoreGroup)
                    {
                        strGrpProf = (StoreGroupProfile)aFromNode.Profile;

                        if (aFindUniqueName)
                        {
                            newName = FindNewStoreGroupName(strGrpProf.Name, aToNode.UserId);
                        }
                        else
                        {
                            newName = strGrpProf.Name;
                        }

                        filter newFilter = null;
                        strGrpProf = StoreMgmt.StoreGroup_Copy(strGrpProf, newName, aToNode.OwnerUserRID, ref newFilter); //SAB.StoreServerSession.CopyStoreGroup(strGrpProf.Key, newName, aToNode.OwnerUserRID);
						// Begin TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
						//                        newNode = ((StoreTreeView)(this._EAB.StoreGroupExplorer.TreeView)).AfterSave(newFilter, strGrpProf);  //Adds db entry to the FOLDER table
                        newNode = ((StoreTreeView)(this._EAB.StoreGroupExplorer.TreeView)).AfterSave(newFilter, strGrpProf, aToNode);  //Adds db entry to the FOLDER table
						// End TT#1929-MD - JSmith - Store Attributes - Copy Folder with contents from Global to User OR User to Global. 
                        //_dlFolder.OpenUpdateConnection();

                        //try
                        //{
                        //    _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, strGrpProf.Key, eProfileType.StoreGroup);

                        //    _dlFolder.CommitData();
                        //}
                        //catch (Exception exc)
                        //{
                        //    string message = exc.ToString();
                        //    throw;
                        //}
                        //finally
                        //{
                        //    _dlFolder.CloseUpdateConnection();
                        //}

                      // newNode = BuildStoreGroupNode(strGrpProf, aToNode);

                        return newNode;
                    }

                    return null;
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

        private bool DeleteStoreNode(MIDStoreNode aNode)
        {


            //Do not allow deleting if Store Load API is running
            GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            if (genericEnqueueStoreLoad.DoesHaveConflicts())
            {
                MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                return false;
            }



            string msg;
            //MIDStoreNode parentNode;
            //int availableRID;
            //MIDStoreNode groupNode;
            //int[] storeRIDs;
            object[] deleteArray;

            try
            {
                if (aNode.isObject)
                {
                    switch (aNode.NodeProfileType)
                    {
                        case eProfileType.StoreGroup:


                            try
                            {
                                StoreMgmt.StoreGroup_SetInactive(aNode.Profile.Key); //SAB.StoreServerSession.DeleteGroup(aNode.Profile.Key);
                            }
                            catch (DatabaseForeignKeyViolation fkv)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse) + System.Environment.NewLine + "Error: " + fkv.ToString());
                                return false;
                            }

                            _dlFolder.OpenUpdateConnection();

                            try
                            {
                                _dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.StoreGroup);

                                // Begin TT#20 - JSmith - Deleting attribute causes error when opening application
                                _dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.StoreGroup);
                                // End TT#20

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

                            DeleteShortcuts(FavoritesNode, aNode);
                            aNode.Remove();

                            break;

                        case eProfileType.StoreGroupLevel:

                            //msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveWithRefresh);
                            //msg = msg.Replace("{0}", aNode.Text);

                            //if (MessageBox.Show(msg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            //{
                            //    return false;
                            //}

                            //try
                            //{
                            //    StoreMgmt.DeleteGroupLevel(aNode.Profile.Key);  //SAB.StoreServerSession.DeleteGroupLevel(aNode.Profile.Key);
                            //}
                            //catch (DatabaseForeignKeyViolation)
                            //{
                            //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
                            //    return false;
                            //}

                            //SAB.StoreServerSession.RefreshStoresInGroup(((MIDStoreNode)aNode.Parent).Profile.Key);
                            //RefreshStoreGroup((MIDStoreNode)aNode.Parent);

                            break;

                        case eProfileType.Store:

                            ////Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //groupNode = aNode.GetStoreGroupNode();

                            ////End Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //if (aNode.isDynamic)
                            //{
                            //    availableRID = 0;
                            //    //Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //    //groupNode = aNode.GetStoreGroupNode();
                            //    //End Track #6199 - JScott - Create attr w/3 sets - move str results in err

                            //    foreach (MIDStoreGroupLevelNode strGrpLvlNode in groupNode.Nodes)
                            //    {
                            //        if (strGrpLvlNode.Sequence == int.MaxValue)
                            //        {
                            //            availableRID = strGrpLvlNode.Profile.Key;
                            //            break;
                            //        }
                            //    }

                            //    storeRIDs = new int[1];
                            //    storeRIDs[0] = aNode.Profile.Key;

                            //    SAB.StoreServerSession.AddGroupLevelJoin(storeRIDs, availableRID);
                            //    //Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //    //RefreshStoreGroup(groupNode);
                            //    //End Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //}
                            //else
                            //{
                            //    parentNode = (MIDStoreNode)aNode.Parent;
                            //    SAB.StoreServerSession.DeleteGroupLevelJoin(parentNode.Profile.Key, aNode.Profile.Key);
                            //    //Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err

                            //    //groupNode = aNode.GetStoreGroupNode();
                            //    //RefreshStoreGroup(groupNode);
                            //    //End Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //}

                            ////Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            //SAB.StoreServerSession.RefreshStoresInGroup(groupNode.Profile.Key);
                            //RefreshStoreGroup(groupNode);

                            //End Track #6199 - JScott - Create attr w/3 sets - move str results in err
                            break;
                    }
                }
                else if (aNode.isSubFolder)
                {
                    deleteArray = new object[aNode.Nodes.Count];
                    aNode.Nodes.CopyTo(deleteArray, 0);

                    foreach (MIDStoreNode node in deleteArray)
                    {
                        DeleteStoreNode(node);
                    }

                    if (aNode.Nodes.Count == 0)
                    {
                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.StoreGroupSubFolder);
                            _dlFolder.Folder_Delete(aNode.Profile.Key, eProfileType.StoreGroupSubFolder);
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
                        DeleteChildNodes((MIDStoreNode)aNode);
                        aNode.Remove();
                    }
                }
                else if (aNode.isObjectShortcut)
                {
                    _dlFolder.OpenUpdateConnection();

                    try
                    {
                        _dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.StoreGroup);
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
                        _dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.StoreGroupSubFolder);
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

                    DeleteChildNodes((MIDStoreNode)aNode);
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

        //private void OnStoreGroupLevelPropertiesChange(object source, StoreGroupLevelChangeEventArgs e)
        //{
        //    try
        //    {
        //        SAB.StoreServerSession.RefreshStoresInGroup(e.SG_NODE.Profile.Key);
        //        RefreshStoreGroup(e.SG_NODE);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private void OnRefreshStoreGroupEvent(object source, StoreGroupLevelChangeEventArgs e)
        //{
        //    try
        //    {
        //        RefreshStoreGroup(e.SG_NODE);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        private void RefreshStoreGroup(MIDStoreNode aGroupNode)
        {
            MIDTreeNode currNode;
            //Begin Track #6243 - JScott - null reference when adding new attribute from Global folder
            MIDTreeNode findNode;
            MIDStoreNode groupNode;
            //End Track #6243 - JScott - null reference when adding new attribute from Global folder
            Hashtable expandedList;

            try
            {
                //Begin Track #6243 - JScott - null reference when adding new attribute from Global folder
                //currNode = SelectedNode;
                currNode = null;
                findNode = null;
                groupNode = (MIDStoreNode)((MIDStoreNode)SelectedNode).GetStoreGroupNode();

                // Begin Track #6254 stodd - error creating new store attribute set
                //if (groupNode == null || groupNode != aGroupNode)
                if (groupNode == null || (MIDStoreNode)SelectedNode == aGroupNode || groupNode != aGroupNode)
                // End Track #6254 stodd - error creating new store attribute set
                {
                    currNode = aGroupNode;
                }
                else
                {
                    findNode = SelectedNode;
                }

                //End Track #6243 - JScott - null reference when adding new attribute from Global folder
                expandedList = new Hashtable();

                SaveExpandedNodes(aGroupNode, expandedList);

                aGroupNode.Collapse();
                aGroupNode.Nodes.Clear();
                aGroupNode.Nodes.Add(new MIDStoreGroupLevelNode());
                aGroupNode.ChildrenLoaded = false;
                aGroupNode.HasChildren = true;
                aGroupNode.DisplayChildren = true;

                ResetExpandedNodes(aGroupNode, expandedList);

                //Begin Track #6243 - JScott - null reference when adding new attribute from Global folder
                //if (currNode != null)
                //{
                //    //Begin Track #6199 - JScott - Create attr w/3 sets - move str results in err
                //    //SelectedNode = GetNodeByItemRID(aGroupNode.Parent.Nodes, currNode.Profile.Key, currNode.NodeProfileType);
                //    SelectedNode = GetNodeByItemRID(aGroupNode.Nodes, currNode.Profile.Key, currNode.NodeProfileType);
                //    //End Track #6199 - JScott - Create attr w/3 sets - move str results in err

                //    if (SelectedNode != null)
                //    {
                //        SelectedNode.EnsureVisible();
                //    }
                //}
                if (findNode != null)
                {
                    currNode = GetNodeByItemRID(aGroupNode.Nodes, findNode.Profile.Key, findNode.NodeProfileType);
                }

                SelectedNode = currNode;

                if (SelectedNode != null)
                {
                    SelectedNode.EnsureVisible();
                }
                //End Track #6243 - JScott - null reference when adding new attribute from Global folder
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveExpandedNodes(MIDTreeNode aStartNode, Hashtable aExpandedList)
        {
            try
            {
                if (aStartNode.IsExpanded)
                {
                    aExpandedList[new HashKeyObject((int)aStartNode.TreeNodeType, (int)aStartNode.NodeProfileType, aStartNode.Profile.Key)] = null;
                }

                if (aStartNode.ChildrenLoaded)
                {
                    foreach (MIDTreeNode node in aStartNode.Nodes)
                    {
                        SaveExpandedNodes(node, aExpandedList);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResetExpandedNodes(MIDTreeNode aStartNode, Hashtable aExpandedList)
        {
            try
            {
                if (aExpandedList.Contains(new HashKeyObject((int)aStartNode.TreeNodeType, (int)aStartNode.NodeProfileType, aStartNode.Profile.Key)))
                {
                    aStartNode.Expand();
                }

                if (aStartNode.ChildrenLoaded)
                {
                    foreach (MIDTreeNode node in aStartNode.Nodes)
                    {
                        ResetExpandedNodes(node, aExpandedList);
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

    public class MIDStoreNode : MIDTreeNode
    {
        //=======
        // FIELDS
        //=======

        private bool _isDynamic;
        private int _filterRID;

        //=============
        // CONSTRUCTORS
        //=============

        public MIDStoreNode()
            : base()
        {
        }

        public MIDStoreNode(
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
            int aOwnerUserRID,
            bool aIsDynamic,
            int aFilterRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, false, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
            : base(aSAB, aTreeNodeType, aProfile, aNodeSecurityGroup, false, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            _isDynamic = aIsDynamic;
            _filterRID = aFilterRID;
        }

        public MIDStoreNode(
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
            int aOwnerUserRID,
            bool aIsDynamic,
            int aFilterRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, false, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
            : base(aSAB, aTreeNodeType, aProfile, aNodeSecurityGroup, false, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            _isDynamic = aIsDynamic;
            _filterRID = aFilterRID;
        }

        //===========
        // PROPERTIES
        //===========

        virtual public int GroupRID
        {
            get
            {
                return Include.NoRID;
            }
        }

        public bool isDynamic
        {
            get
            {
                return _isDynamic;
            }
            set
            {
                _isDynamic = value;
            }
        }

        public int FilterRID
        {
            get
            {
                return _filterRID;
            }
            set
            {
                _filterRID = value;
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
                allowDrag = false;

                // Begin TT#30 - JSmith - Drag and Drop an attribute from the Store Explorer into a method or view does not work
                //if (!isChildShortcut &&
                //    (NodeProfileType == eProfileType.StoreGroupSubFolder ||
                //    NodeProfileType == eProfileType.StoreGroup ||
                //    NodeProfileType == eProfileType.StoreGroupLevel ||
                //    NodeProfileType == eProfileType.Store))
                //{
                //    allowDrag = true;
                //}
                if (NodeProfileType == eProfileType.StoreGroupSubFolder ||
                    NodeProfileType == eProfileType.StoreGroup ||
                    NodeProfileType == eProfileType.StoreGroupLevel ||
                    NodeProfileType == eProfileType.Store)
                {
                    allowDrag = true;
                }
                // End TT#30 

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
            MIDStoreNode destNode;

            try
            {
                // do not allow drop on same node
                if (aDestinationNode == this)
                {
                    return false;
                }

                // Begin TT#30 - JSmith - Drag and Drop an attribute from the Store Explorer into a method or view does not work
                if (isChildShortcut)
                {
                    return false;
                }
                // End TT#30 

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

                destNode = (MIDStoreNode)aDestinationNode;

                switch (NodeProfileType)
                {
                    case eProfileType.Store:
                        return false; //TT#1517-MD -jsobek -Store Service Optimization -No dropping on stores.
                        //if (aDropAction == DragDropEffects.Copy)
                        //{
                        //    return false;
                        //}

                        //if (destNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                        //{
                        //    return false;
                        //}

                        //if (destNode.NodeProfileType != eProfileType.StoreGroupLevel &&
                        //    destNode.NodeProfileType != eProfileType.Store)
                        //{
                        //    return false;
                        //}

                        //if (GetStoreGroupNode() != ((MIDStoreNode)destNode).GetStoreGroupNode())
                        //{
                        //    return false;
                        //}

                        //if (destNode.NodeProfileType == eProfileType.Store)
                        //{
                        //    if (Parent == destNode.Parent)
                        //    {
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    if (Parent == destNode)
                        //    {
                        //        return false;
                        //    }
                        //}

                        //// Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                        //if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                        //{
                        //    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                        //}
                        //else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                        //{
                        //    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                        //}
                        //else
                        //{
                        //    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                        //}
                        //// End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                        //return true;

                    case eProfileType.StoreGroupLevel:
                        return false; //TT#1517-MD -jsobek -Store Service Optimization -No dropping on levels.

                        //// Begin Track #6301 - JSmith - Select PreOpen results in Error
                        //// Do not allow the Available Stores set to be moved from last
                        //if (((MIDStoreGroupLevelNode)this).Sequence == int.MaxValue)
                        //{
                        //    return false;
                        //}
                        //// End Track #6301

                        //if (aDropAction == DragDropEffects.Copy)
                        //{
                        //    return false;
                        //}

                        //if (destNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                        //{
                        //    return false;
                        //}

                        //if (destNode.NodeProfileType != eProfileType.StoreGroup &&
                        //    destNode.NodeProfileType != eProfileType.StoreGroupLevel)
                        //{
                        //    return false;
                        //}

                        //if (GetStoreGroupNode() != ((MIDStoreNode)destNode).GetStoreGroupNode())
                        //{
                        //    return false;
                        //}

                        //if (destNode.NodeProfileType == eProfileType.StoreGroupLevel)
                        //{
                        //    if (Parent != destNode.Parent)
                        //    {
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    if (Parent != destNode)
                        //    {
                        //        return false;
                        //    }
                        //}

                        //// Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                        //if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                        //{
                        //    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                        //}
                        //else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                        //{
                        //    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                        //}
                        //else
                        //{
                        //    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                        //}
                        //// End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                        //return true;

                    case eProfileType.StoreGroup:
                    case eProfileType.StoreGroupSubFolder:

                        if (GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
                            destNode.GetTopSourceNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode)
                        {
                            return false;
                        }

                        if (destNode.isChildShortcut)
                        {
                            return false;
                        }

                        //if (destNode.GetTopNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode &&
                        //    GetTopNode().NodeProfileType != destNode.GetTopNode().NodeProfileType)
                        //{
                        //    return false;
                        //}

                        if (destNode.TreeNodeType != eTreeNodeType.MainFavoriteFolderNode &&
                            destNode.TreeNodeType != eTreeNodeType.MainSourceFolderNode &&
                            destNode.TreeNodeType != eTreeNodeType.ObjectNode &&
                            destNode.TreeNodeType != eTreeNodeType.SubFolderNode)
                        {
                            return false;
                        }

                        if (destNode.TreeNodeType == eTreeNodeType.ObjectNode && destNode.NodeProfileType != eProfileType.StoreGroup)
                        {
                            return false;
                        }

                        // Begin TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                        if (Profile.Key == Include.AllStoreGroupRID)
                        {
                            // Do not allow All Stores to be pasted to My Attributes
                            if (destNode.GetTopSourceNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode &&
                                destNode.GetTopSourceNode().OwnerUserRID != Include.GlobalUserRID)
                            {
                                return false;
                            }
                            // Do not allow All Stores to be pasted to Global
                            else if (destNode.GetTopSourceNode().OwnerUserRID == Include.GlobalUserRID)
                            {
                                return false;
                            }
                        }
                        // End TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed

                        if (aDropAction == DragDropEffects.Copy)
                        {
                            if (destNode.isChildOf(this))
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
                            if (destNode == GetParentNode() || destNode.isChildOf(this) ||
                                (destNode.isObject && destNode.GetParentNode() == GetParentNode()))
                            {
                                return false;
                            }

                            // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                            if (FunctionSecurityProfile.AllowMove)
                            {
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
                            }
                            // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders

                            // Begin TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
                            //return FunctionSecurityProfile.AllowDelete;
                            return FunctionSecurityProfile.AllowMove;
                            // End TT#4321 - JSmith - All Stores Attribute and All Stores Set should not be allowed to be changed
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
                    //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
                    (NodeProfileType != eProfileType.StoreGroupLevel || (NodeProfileType == eProfileType.StoreGroupLevel && ((StoreGroupLevelProfile)Profile).Sequence != int.MaxValue)) &&
                    //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
                    ((FolderSecurityProfile.AllowUpdate &&
                    (NodeProfileType == eProfileType.StoreGroupMainFavoritesFolder ||
                    NodeProfileType == eProfileType.StoreGroupMainUserFolder ||
                    NodeProfileType == eProfileType.StoreGroupSubFolder)) ||
                    (FunctionSecurityProfile.AllowUpdate &&
                    (NodeProfileType == eProfileType.StoreGroup ||
                    NodeProfileType == eProfileType.StoreGroupLevel))))
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
        /// Abstract method that refreshes the shortcut node
        /// </summary>

        override public void RefreshShortcutNode(MIDTreeNode aChangedNode)
        {
            try
            {
                if (isObjectShortcut || isFolderShortcut)
                {
                    UserId = aChangedNode.UserId;

                    if (NodeProfileType == eProfileType.StoreGroup)
                    {
                        //Begin Track #6201 - JScott - Store Count removed from attr sets
                        //Text = ((StoreGroupProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                        InternalText = ((StoreGroupProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
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

                    if (NodeProfileType == eProfileType.StoreGroup)
                    {
                        //Begin Track #6201 - JScott - Store Count removed from attr sets
                        //Text = ((StoreGroupProfile)aChangedNode.Profile).Name;
                        InternalText = ((StoreGroupProfile)aChangedNode.Profile).Name;
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

        /// <summary>
        /// Retrieves a list of children for the node
        /// </summary>
        /// <returns>An ArrayList containing profiles for each child</returns>

        override public void BuildChildren()
        {
            StoreGroupProfile strGrpProf;
            MIDStoreNode grpLvlNode;
            MIDStoreNode strNode;
            //Begin Track #6214 - JScott - Stores not in numeric order
            ArrayList storeList;
            //End Track #6214 - JScott - Stores not in numeric order

            try
            {
                if (NodeProfileType == eProfileType.StoreGroup)
                {
                    strGrpProf = StoreMgmt.StoreGroup_GetFilled(Profile.Key); //SAB.StoreServerSession.GetStoreGroupFilled(Profile.Key);

                    foreach (StoreGroupLevelProfile strGrpLvlProf in strGrpProf.GroupLevels)
                    {
                        grpLvlNode = ((StoreTreeView)TreeView).BuildStoreGroupLevelNode(strGrpLvlProf, this);

                        if (this.isChildShortcut || this.isObjectShortcut)
                        {
                            grpLvlNode.TreeNodeType = eTreeNodeType.ChildObjectShortcutNode;
                        }

                        //Begin Track #6214 - JScott - Stores not in numeric order
                        //foreach (StoreProfile strProf in strGrpLvlProf.Stores)
                        storeList = (ArrayList)strGrpLvlProf.Stores.ArrayList.Clone();
                        storeList.Sort(new StoreIDComparer());

                        foreach (StoreProfile strProf in storeList)
                        //End Track #6214 - JScott - Stores not in numeric order
                        {
                            strNode = ((StoreTreeView)TreeView).BuildStoreNode(strProf, grpLvlNode);

                            if (this.isChildShortcut || this.isObjectShortcut)
                            {
                                strNode.TreeNodeType = eTreeNodeType.ChildObjectShortcutNode;
                            }
                        }

                        //Begin Track #6201 - JScott - Store Count removed from attr sets
                        grpLvlNode.UpdateExternalText();
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

        //--------------
        //PUBLIC METHODS
        //--------------

        public MIDStoreNode GetStoreGroupNode()
        {
            MIDStoreNode node;

            try
            {
                node = this;

                while (node != null && node.NodeProfileType != eProfileType.StoreGroup)
                {
                    node = (MIDStoreNode)node.Parent;
                }

                //Begin Track #6243 - JScott - null reference when adding new attribute from Global folder
                //if (node == null)
                //{
                //    throw new Exception("Store Group Node not found");
                //}

                //End Track #6243 - JScott - null reference when adding new attribute from Global folder
                return node;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //Begin Track #6214 - JScott - Stores not in numeric order

        private class StoreIDComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                try
                {
                    return ((StoreProfile)x).StoreId.CompareTo(((StoreProfile)y).StoreId);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        //End Track #6214 - JScott - Stores not in numeric order
    }

    public class MIDStoreGroupNode : MIDStoreNode
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public MIDStoreGroupNode()
            : base()
        {
        }

        public MIDStoreGroupNode(
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
            int aOwnerUserRID,
            bool aIsDynamic,
            int aFilterRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            ////Begin Track #6201 - JScott - Store Count removed from attr sets
            ////: base(aSAB, aTreeNodeType, aProfile, aText, aParentId, aUserId, aFunctionSecurityProfile, aImageIndex, aSelectedImageIndex, aOwnerUserRID, aIsDynamic)
            //: base(aSAB, aTreeNodeType, (aProfile.ProfileType == eProfileType.StoreGroupListView ? new StoreGroupProfile((StoreGroupListViewProfile)aProfile) : aProfile), aText, aParentId, aUserId, aFunctionSecurityProfile, aImageIndex, aSelectedImageIndex, aOwnerUserRID, aIsDynamic)
            ////End Track #6201 - JScott - Store Count removed from attr sets
            : base(aSAB, aTreeNodeType, (aProfile.ProfileType == eProfileType.StoreGroupListView ? new StoreGroupProfile((StoreGroupListViewProfile)aProfile) : aProfile), aText, aParentId, aUserId, aNodeSecurityGroup, aImageIndex, aSelectedImageIndex, aOwnerUserRID, aIsDynamic, aFilterRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            //Begin Track #6201 - JScott - Store Count removed from attr sets
            //if (aProfile.ProfileType == eProfileType.StoreGroupListView)
            //{
            //    Profile = new StoreGroupProfile((StoreGroupListViewProfile)aProfile);
            //}
            //End Track #6201 - JScott - Store Count removed from attr sets
        }

        public MIDStoreGroupNode(
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
            int aOwnerUserRID,
            bool aIsDynamic,
            int aFilterRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            ////Begin Track #6201 - JScott - Store Count removed from attr sets
            ////: base(aSAB, aTreeNodeType, aProfile, aText, aParentId, aUserId, aFunctionSecurityProfile, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID, aIsDynamic)
            //: base(aSAB, aTreeNodeType, (aProfile.ProfileType == eProfileType.StoreGroupListView ? new StoreGroupProfile((StoreGroupListViewProfile)aProfile) : aProfile), aText, aParentId, aUserId, aFunctionSecurityProfile, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID, aIsDynamic)
            ////End Track #6201 - JScott - Store Count removed from attr sets
            : base(aSAB, aTreeNodeType, (aProfile.ProfileType == eProfileType.StoreGroupListView ? new StoreGroupProfile((StoreGroupListViewProfile)aProfile) : aProfile), aText, aParentId, aUserId, aNodeSecurityGroup, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID, aIsDynamic, aFilterRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            //Begin Track #6201 - JScott - Store Count removed from attr sets
            //if (aProfile.ProfileType == eProfileType.StoreGroupListView)
            //{
            //    Profile = new StoreGroupProfile((StoreGroupListViewProfile)aProfile);
            //}
            //End Track #6201 - JScott - Store Count removed from attr sets
        }

        //===========
        // PROPERTIES
        //===========

        override public int GroupRID
        {
            get
            {
                return Profile.Key;
            }
        }

        //========
        // METHODS
        //========

        override public int CompareTo(object obj)
        {
            try
            {
                return base.CompareTo(obj);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    public class MIDStoreGroupLevelNode : MIDStoreNode
    {
        //=======
        // FIELDS
        //=======

        private int _sequence;
        private int _groupRID;

        //=============
        // CONSTRUCTORS
        //=============

        public MIDStoreGroupLevelNode()
            : base()
        {
        }

        public MIDStoreGroupLevelNode(
            SessionAddressBlock aSAB,
            eTreeNodeType aTreeNodeType,
            Profile aProfile,
            string aText,
            int aParentId,
            int aUserId,
            int aSequence,
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //FunctionSecurityProfile aFunctionSecurityProfile,
            MIDTreeNodeSecurityGroup aNodeSecurityGroup,
            //End Track #6321 - JScott - User has ability to to create folders when security is view
            int aImageIndex,
            int aSelectedImageIndex,
            int aOwnerUserRID,
            bool aIsDynamic,
            int aFilterRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //: base(aSAB, aTreeNodeType, aProfile, aText, aParentId, aUserId, aFunctionSecurityProfile, aImageIndex, aSelectedImageIndex, aOwnerUserRID, aIsDynamic)
            : base(aSAB, aTreeNodeType, aProfile, aText, aParentId, aUserId, aNodeSecurityGroup, aImageIndex, aSelectedImageIndex, aOwnerUserRID, aIsDynamic, aFilterRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            _sequence = aSequence;
            _groupRID = aParentId;
            //Begin Track #6201 - JScott - Store Count removed from attr sets
            AddChildCountToText = true;
            //End Track #6201 - JScott - Store Count removed from attr sets
        }

        public MIDStoreGroupLevelNode(
            SessionAddressBlock aSAB,
            eTreeNodeType aTreeNodeType,
            Profile aProfile,
            string aText,
            int aParentId,
            int aUserId,
            int aSequence,
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //FunctionSecurityProfile aFunctionSecurityProfile,
            MIDTreeNodeSecurityGroup aNodeSecurityGroup,
            //End Track #6321 - JScott - User has ability to to create folders when security is view
            int aCollapsedImageIndex,
            int aSelectedCollapsedImageIndex,
            int aExpandedImageIndex,
            int aSelectedExpandedImageIndex,
            int aOwnerUserRID,
            bool aIsDynamic,
            int aFilterRID)
            //Begin Track #6321 - JScott - User has ability to to create folders when security is view
            //: base(aSAB, aTreeNodeType, aProfile, aText, aParentId, aUserId, aFunctionSecurityProfile, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID, aIsDynamic)
            : base(aSAB, aTreeNodeType, aProfile, aText, aParentId, aUserId, aNodeSecurityGroup, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID, aIsDynamic, aFilterRID)
        //End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            _sequence = aSequence;
            _groupRID = aParentId;
            //Begin Track #6201 - JScott - Store Count removed from attr sets
            AddChildCountToText = true;
            //End Track #6201 - JScott - Store Count removed from attr sets
        }

        //===========
        // PROPERTIES
        //===========

        override public int GroupRID
        {
            get
            {
                return _groupRID;
            }
        }

        public int Sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }

        //========
        // METHODS
        //========

        override public int CompareTo(object obj)
        {
            try
            {
                if (_sequence < ((MIDStoreGroupLevelNode)obj)._sequence)
                {
                    return -1;
                }
                else if (_sequence > ((MIDStoreGroupLevelNode)obj)._sequence)
                {
                    return 1;
                }
                else
                {
                    return 0;
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