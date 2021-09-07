// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
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
    public class TaskListTreeView : MIDTreeView
    {
		//=======
		// FIELDS
		//=======

		private int cFavoritesImage;
		private int cClosedFolderImage;
		private int cOpenFolderImage;
		private int cClosedShortcutFolderImage;
		private int cOpenShortcutFolderImage;
		private int cClosedSharedFolderImage;
		private int cOpenSharedFolderImage;
		private int cTaskListUnselectedImage;
		private int cTaskListSelectedImage;
		private int cTaskListShortcutUnselectedImage;
		private int cTaskListShortcutSelectedImage;
		private int cTaskListSharedUnselectedImage;
		private int cTaskListSharedSelectedImage;
		private int cJobUnselectedImage;
		private int cJobSelectedImage;
		private int cJobShortcutUnselectedImage;
		private int cJobShortcutSelectedImage;
		private int cSpecialRequestUnselectedImage;
		private int cSpecialRequestSelectedImage;
		private int cSpecialRequestShortcutUnselectedImage;
		private int cSpecialRequestShortcutSelectedImage;

		//Begin Track #6321 - JScott - User has ability to to create folders when security is view
		//private Hashtable _tskLstSecLvlHash;
		//private FunctionSecurityProfile _systemJobSecLvl;
		//private FunctionSecurityProfile _systemSpecialSecLvl;
		private Hashtable _tskLstSecGrpHash;
		private MIDTreeNodeSecurityGroup _systemJobSecGrp;
		private MIDTreeNodeSecurityGroup _systemSpecialSecGrp;
		//End Track #6321 - JScott - User has ability to to create folders when security is view

		private ScheduleData _dlSchedule;
		private FolderDataLayer _dlFolder;

		private MIDTaskListNode _mainTaskListsnode = null;
		private MIDTaskListNode _userTaskListsNode = null;
		private MIDTaskListNode _globalTaskListsNode = null;
		private MIDTaskListNode _systemTaskListsNode = null;
		private MIDTaskListNode _sharedTaskListsNode = null;
		private MIDTaskListNode _jobsNode = null;
		private MIDTaskListNode _specialReqNode = null;

        private Hashtable _htAvailableTasks = new Hashtable();  // TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.

        //=============
        // CONSTRUCTORS
        //=============

        public TaskListTreeView()
        {
        }

		//===========
		// PROPERTIES
		//===========

		//Begin Track #6321 - JScott - User has ability to to create folders when security is view
		private MIDTreeNodeSecurityGroup UserTskLstSecGrp
		{
			get
			{
				return (MIDTreeNodeSecurityGroup)_tskLstSecGrpHash[SAB.ClientServerSession.UserRID];
			}
		}

		private MIDTreeNodeSecurityGroup GlobalTskLstSecGrp
		{
			get
			{
				return (MIDTreeNodeSecurityGroup)_tskLstSecGrpHash[Include.GlobalUserRID];
			}
		}

		private MIDTreeNodeSecurityGroup SystemTskLstSecGrp
		{
			get
			{
				return (MIDTreeNodeSecurityGroup)_tskLstSecGrpHash[Include.SystemUserRID];
			}
		}

		//End Track #6321 - JScott - User has ability to to create folders when security is view
		private FunctionSecurityProfile UserTskLstSecLvl
		{
			get
			{
				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//return (FunctionSecurityProfile)_tskLstSecLvlHash[SAB.ClientServerSession.UserRID];
				return UserTskLstSecGrp.FunctionSecurityProfile;
				//End Track #6321 - JScott - User has ability to to create folders when security is view
			}
		}

		private FunctionSecurityProfile GlobalTskLstSecLvl
		{
			get
			{
				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//return (FunctionSecurityProfile)_tskLstSecLvlHash[Include.GlobalUserRID];
				return GlobalTskLstSecGrp.FunctionSecurityProfile;
				//End Track #6321 - JScott - User has ability to to create folders when security is view
			}
		}

		private FunctionSecurityProfile SystemTskLstSecLvl
		{
			get
			{
				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//return (FunctionSecurityProfile)_tskLstSecLvlHash[Include.SystemUserRID];
				return SystemTskLstSecGrp.FunctionSecurityProfile;
				//End Track #6321 - JScott - User has ability to to create folders when security is view
			}
		}

        // Begin TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
        public Hashtable AvailableTasks
        {
            get
            {
                return _htAvailableTasks;
            }
        }
        // End TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.

		//Begin Track #6321 - JScott - User has ability to to create folders when security is view
		private FunctionSecurityProfile SystemJobSecLvl
		{
			get
			{
				return _systemJobSecGrp.FunctionSecurityProfile;
			}
		}

		private FunctionSecurityProfile SystemSpecialSecLvl
		{
			get
			{
				return _systemSpecialSecGrp.FunctionSecurityProfile;
			}
		}

		//End Track #6321 - JScott - User has ability to to create folders when security is view
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
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile userTskLstSecLvl;
			//FunctionSecurityProfile globalTskLstSecLvl;
			//FunctionSecurityProfile systemTskLstSecLvl;
			MIDTreeNodeSecurityGroup userTskLstSecGrp;
			MIDTreeNodeSecurityGroup globalTskLstSecGrp;
			MIDTreeNodeSecurityGroup systemTskLstSecGrp;
			//End Track #6321 - JScott - User has ability to to create folders when security is view

			try
			{
				base.InitializeTreeView(aSAB, aAllowMultiSelect, aMDIParentForm);

				cFavoritesImage = MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage);
				cClosedFolderImage = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
				cOpenFolderImage = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);
				cClosedShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.ClosedTreeFolder);
				cOpenShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.OpenTreeFolder);
				cClosedSharedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.ClosedTreeFolder);
				cOpenSharedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.OpenTreeFolder);
				cTaskListUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.TaskListUnselected);
				cTaskListSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.TaskListSelected);
				cTaskListShortcutUnselectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.TaskListUnselected);
				cTaskListShortcutSelectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.TaskListSelected);
				cTaskListSharedUnselectedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.TaskListUnselected);
				cTaskListSharedSelectedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.TaskListSelected);
				cJobUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.JobUnselected);
				cJobSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.JobSelected);
				cJobShortcutUnselectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.JobUnselected);
				cJobShortcutSelectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.JobSelected);
				cSpecialRequestUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.SpecialRequestUnselected);
				cSpecialRequestSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.SpecialRequestSelected);
				cSpecialRequestShortcutUnselectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.SpecialRequestUnselected);
				cSpecialRequestShortcutSelectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.SpecialRequestSelected);

				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//userTskLstSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerUserTaskLists);
				//globalTskLstSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerGlobalTaskLists);
				//systemTskLstSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemTaskLists);

				//if (!systemTskLstSecLvl.AccessDenied)
				//{
				//    if (userTskLstSecLvl.AccessDenied)
				//    {
				//        userTskLstSecLvl.SetReadOnly();
				//    }
				//    if (globalTskLstSecLvl.AccessDenied)
				//    {
				//        globalTskLstSecLvl.SetReadOnly();
				//    }
				//}

				//_tskLstSecLvlHash = new Hashtable();
				//_tskLstSecLvlHash[SAB.ClientServerSession.UserRID] = userTskLstSecLvl;
				//_tskLstSecLvlHash[Include.GlobalUserRID] = globalTskLstSecLvl;
				//_tskLstSecLvlHash[Include.SystemUserRID] = systemTskLstSecLvl;

				//_systemJobSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemJobs);
				//_systemSpecialSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemSpecialReq);
				userTskLstSecGrp = new MIDTreeNodeSecurityGroup(
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerUserTaskLists),
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklistFoldersUser));

				globalTskLstSecGrp = new MIDTreeNodeSecurityGroup(
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerGlobalTaskLists),
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklistFoldersGlobal));

				systemTskLstSecGrp = new MIDTreeNodeSecurityGroup(
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemTaskLists),
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklistFoldersSystem));

				if (!systemTskLstSecGrp.FunctionSecurityProfile.AccessDenied)
				{
					if (userTskLstSecGrp.FunctionSecurityProfile.AccessDenied)
					{
						userTskLstSecGrp.FunctionSecurityProfile.SetReadOnly();
					}
					if (globalTskLstSecGrp.FunctionSecurityProfile.AccessDenied)
					{
						globalTskLstSecGrp.FunctionSecurityProfile.SetReadOnly();
					}
				}

				if (!systemTskLstSecGrp.FolderSecurityProfile.AccessDenied)
				{
					if (userTskLstSecGrp.FolderSecurityProfile.AccessDenied)
					{
						userTskLstSecGrp.FolderSecurityProfile.SetReadOnly();
					}
					if (globalTskLstSecGrp.FolderSecurityProfile.AccessDenied)
					{
						globalTskLstSecGrp.FolderSecurityProfile.SetReadOnly();
					}
				}

                // Begin TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
                foreach (eTaskType taskType in Include.GetAvailableTasks(userTskLstSecGrp.FunctionSecurityProfile, globalTskLstSecGrp.FunctionSecurityProfile, systemTskLstSecGrp.FunctionSecurityProfile, SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled))
                {
                    if (taskType != eTaskType.None)
                    {
                        _htAvailableTasks.Add(taskType.GetHashCode(), null);
                    }
                }
                // ENd TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.

				_tskLstSecGrpHash = new Hashtable();
				_tskLstSecGrpHash[SAB.ClientServerSession.UserRID] = userTskLstSecGrp;
				_tskLstSecGrpHash[Include.GlobalUserRID] = globalTskLstSecGrp;
				_tskLstSecGrpHash[Include.SystemUserRID] = systemTskLstSecGrp;

				_systemJobSecGrp = new MIDTreeNodeSecurityGroup(
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemJobs),
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklistFoldersSystem));

				_systemSpecialSecGrp = new MIDTreeNodeSecurityGroup(
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemSpecialReq),
					SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklistFoldersSystem));
				//End Track #6321 - JScott - User has ability to to create folders when security is view

				_dlSchedule = new ScheduleData();
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
			Hashtable userTaskListsNodeList = null;

			ArrayList userRIDList;
			ArrayList favoritesUserRIDList;
			ArrayList folderTypeList;
			DataTable dtTasks = null;
			DataTable dtFolderItems = null;
			DataTable dtJobs = null;
			DataTable dtSpecialRequests = null;
			DataTable dtTaskListShortcuts = null;
			DataTable dtJobShortcuts = null;
			DataTable dtSpecialRequestShortcuts = null;
			DataTable dtFolderShortcuts = null;

			FolderShortcut newShortcut;
			MIDTaskListNode parentNode;
			MIDTaskListNode childNode;
			DataTable dtUserFolders;
			MIDTaskListNode userNode;

			MIDTaskListNode newNode;
			IDictionaryEnumerator iEnum;

			try
			{
				Nodes.Clear();

				//----------------------
				// Build Faviorites node
				//----------------------

                // Begin TT#108 - JSmith - No Favorites folder if user tasklists are denied.
                //if (!UserTskLstSecLvl.AccessDenied || !SystemTskLstSecLvl.AccessDenied)
                //{
                // End TT#108
					dtFolders = DlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.TaskListMainFavoritesFolder);

					//Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
					//if (dtFolders == null || dtFolders.Rows.Count != 1)
					if (dtFolders == null || dtFolders.Rows.Count == 0)
					//End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
					{
						folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.TaskListMainFavoritesFolder, "My Favorites", SAB.ClientServerSession.UserRID);

						DlFolder.OpenUpdateConnection();

						try
						{
							folderProf.Key = DlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, folderProf.FolderType);
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
					}
					else
					{
						folderProf = new FolderProfile(dtFolders.Rows[0]);
					}

                    // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                    //FavoritesNode = new MIDTaskListNode(
                    //    SAB,
                    //    eTreeNodeType.MainFavoriteFolderNode,
                    //    folderProf,
                    //    folderProf.Name,
                    //    Include.NoRID,
                    //    folderProf.UserRID,
                    //    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                    //    //UserTskLstSecLvl,
                    //    UserTskLstSecGrp,
                    //    //End Track #6321 - JScott - User has ability to to create folders when security is view
                    //    cFavoritesImage,
                    //    cFavoritesImage,
                    //    cFavoritesImage,
                    //    cFavoritesImage,
                    //    folderProf.UserRID);
                    FavoritesNode = new MIDTaskListNode(
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
                        folderProf.UserRID);
                    // Begin TT#42

					FolderNodeHash[folderProf.Key] = FavoritesNode;
					Nodes.Add(FavoritesNode);
                // Begin TT#108 - JSmith - No Favorites folder if user tasklists are denied.
                //}
                // End TT#108

				//---------------------------
				// Build Main Task Lists node
				//---------------------------

				dtFolders = DlFolder.Folder_Read(Include.UndefinedUserRID, eProfileType.TaskListTaskListMainFolder);

				if (dtFolders == null)
				{
					throw new Exception("Main Task List Folder not defined");
				}
				else if (dtFolders.Rows.Count != 1)
				{
					throw new Exception("More than one Main Task List Folder is defined");
				}

				folderProf = new FolderProfile(dtFolders.Rows[0]);

				_mainTaskListsnode = new MIDTaskListNode(
					SAB,
					eTreeNodeType.MainNonSourceFolderNode,
					folderProf,
					folderProf.Name,
					Include.NoRID,
					folderProf.UserRID,
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//UserTskLstSecLvl,
					UserTskLstSecGrp,
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					cClosedFolderImage,
					cClosedFolderImage,
					cOpenFolderImage,
					cOpenFolderImage,
					folderProf.OwnerUserRID);

				FolderNodeHash[folderProf.Key] = _mainTaskListsnode;
				Nodes.Add(_mainTaskListsnode);

				//---------------------------
				// Build User Task Lists node
				//---------------------------

				if (!UserTskLstSecLvl.AccessDenied || !SystemTskLstSecLvl.AccessDenied)
				{
					dtFolders = DlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.TaskListTaskListMainUserFolder);

					//Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
					//if (dtFolders == null || dtFolders.Rows.Count != 1)
					if (dtFolders == null || dtFolders.Rows.Count == 0)
					//End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
					{
						folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.TaskListTaskListMainUserFolder, "My Task Lists", SAB.ClientServerSession.UserRID);

						DlFolder.OpenUpdateConnection();

						try
						{
							folderProf.Key = DlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, folderProf.FolderType);
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
					}
					else
					{
						folderProf = new FolderProfile(dtFolders.Rows[0]);
					}

					_userTaskListsNode = new MIDTaskListNode(
						SAB,
						eTreeNodeType.MainSourceFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						//UserTskLstSecLvl,
						UserTskLstSecGrp,
						//End Track #6321 - JScott - User has ability to to create folders when security is view
						cClosedFolderImage,
						cClosedFolderImage,
						cOpenFolderImage,
						cOpenFolderImage,
						folderProf.OwnerUserRID);

					FolderNodeHash[folderProf.Key] = _userTaskListsNode;
					_mainTaskListsnode.Nodes.Add(_userTaskListsNode);
				}

				//-----------------------------
				// Build User Global Lists node
				//-----------------------------

				if (!GlobalTskLstSecLvl.AccessDenied || !SystemTskLstSecLvl.AccessDenied)
				{
					dtFolders = DlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.TaskListTaskListMainGlobalFolder);

					if (dtFolders == null)
					{
						throw new Exception("Global Task List Folder not defined");
					}
					else if (dtFolders.Rows.Count != 1)
					{
						throw new Exception("More than one Global Task List Folder is defined");
					}

					folderProf = new FolderProfile(dtFolders.Rows[0]);

					_globalTaskListsNode = new MIDTaskListNode(
						SAB,
						eTreeNodeType.MainSourceFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						//GlobalTskLstSecLvl,
						GlobalTskLstSecGrp,
						//End Track #6321 - JScott - User has ability to to create folders when security is view
						cClosedFolderImage,
						cClosedFolderImage,
						cOpenFolderImage,
						cOpenFolderImage,
						folderProf.OwnerUserRID);

					FolderNodeHash[folderProf.Key] = _globalTaskListsNode;
					_mainTaskListsnode.Nodes.Add(_globalTaskListsNode);
				}

				//-----------------------------
				// Build User System Lists node
				//-----------------------------

				if (!SystemTskLstSecLvl.AccessDenied)
				{
					dtFolders = DlFolder.Folder_Read(Include.SystemUserRID, eProfileType.TaskListTaskListMainSystemFolder);

					if (dtFolders == null)
					{
						throw new Exception("System Task List Folder not defined");
					}
					else if (dtFolders.Rows.Count != 1)
					{
						throw new Exception("More than one System Task List Folder is defined");
					}

					folderProf = new FolderProfile(dtFolders.Rows[0]);

					_systemTaskListsNode = new MIDTaskListNode(
						SAB,
						eTreeNodeType.MainSourceFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						//SystemTskLstSecLvl,
						SystemTskLstSecGrp,
						//End Track #6321 - JScott - User has ability to to create folders when security is view
						cClosedFolderImage,
						cClosedFolderImage,
						cOpenFolderImage,
						cOpenFolderImage,
						folderProf.OwnerUserRID);

					FolderNodeHash[folderProf.Key] = _systemTaskListsNode;
					_mainTaskListsnode.Nodes.Add(_systemTaskListsNode);
				}

				//---------------------
				// Build User Jobs node
				//---------------------

				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//if (!_systemJobSecLvl.AccessDenied)
				if (!SystemJobSecLvl.AccessDenied)
				//End Track #6321 - JScott - User has ability to to create folders when security is view
				{
					dtFolders = DlFolder.Folder_Read(Include.SystemUserRID, eProfileType.TaskListJobMainFolder);

					if (dtFolders == null)
					{
						throw new Exception("Job Folder not defined");
					}
					else if (dtFolders.Rows.Count != 1)
					{
						throw new Exception("More than one Job Folder is defined");
					}

					folderProf = new FolderProfile(dtFolders.Rows[0]);

					_jobsNode = new MIDTaskListNode(
						SAB,
						eTreeNodeType.MainSourceFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						//SystemTskLstSecLvl,
						SystemTskLstSecGrp,
						//End Track #6321 - JScott - User has ability to to create folders when security is view
						cClosedFolderImage,
						cClosedFolderImage,
						cOpenFolderImage,
						cOpenFolderImage,
						folderProf.OwnerUserRID);

					FolderNodeHash[folderProf.Key] = _jobsNode;
					Nodes.Add(_jobsNode);
				}

				//---------------------------------
				// Build User Special Requests node
				//---------------------------------

				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//if (!_systemSpecialSecLvl.AccessDenied)
				if (!SystemSpecialSecLvl.AccessDenied)
				//End Track #6321 - JScott - User has ability to to create folders when security is view
				{
					dtFolders = DlFolder.Folder_Read(Include.SystemUserRID, eProfileType.TaskListSpecialRequestMainFolder);

					if (dtFolders == null)
					{
						throw new Exception("System Request Folder not defined");
					}
					else if (dtFolders.Rows.Count != 1)
					{
						throw new Exception("More than one System Request Folder is defined");
					}

					folderProf = new FolderProfile(dtFolders.Rows[0]);

					_specialReqNode = new MIDTaskListNode(
						SAB,
						eTreeNodeType.MainSourceFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						//Begin Track #6321 - JScott - User has ability to to create folders when security is view
						//SystemTskLstSecLvl,
						SystemTskLstSecGrp,
						//End Track #6321 - JScott - User has ability to to create folders when security is view
						cClosedFolderImage,
						cClosedFolderImage,
						cOpenFolderImage,
						cOpenFolderImage,
						folderProf.OwnerUserRID);

					FolderNodeHash[folderProf.Key] = _specialReqNode;
					Nodes.Add(_specialReqNode);

				}

				//---------------------------
				// Read and Load Detail Nodes
				//---------------------------

				userRIDList = new ArrayList();

				if (!UserTskLstSecLvl.AccessDenied)
				{
					userRIDList.Add(SAB.ClientServerSession.UserRID);
				}

				if (!GlobalTskLstSecLvl.AccessDenied)
				{
					userRIDList.Add(Include.GlobalUserRID);
				}

				if (!SystemTskLstSecLvl.AccessDenied)
				{
					userRIDList.Add(Include.SystemUserRID);
				}

				favoritesUserRIDList = new ArrayList();
				favoritesUserRIDList.Add(SAB.ClientServerSession.UserRID);

				folderTypeList = new ArrayList();
				folderTypeList.Add((int)eProfileType.TaskListMainFavoritesFolder);
				folderTypeList.Add((int)eProfileType.TaskListTaskListMainFolder);
				folderTypeList.Add((int)eProfileType.TaskListTaskListMainUserFolder);
				folderTypeList.Add((int)eProfileType.TaskListTaskListMainGlobalFolder);
				folderTypeList.Add((int)eProfileType.TaskListTaskListMainSystemFolder);
				folderTypeList.Add((int)eProfileType.TaskListJobMainFolder);
				folderTypeList.Add((int)eProfileType.TaskListSpecialRequestMainFolder);
				folderTypeList.Add((int)eProfileType.TaskListSubFolder);

				if (userRIDList == null || userRIDList.Count > 0)
				{
					dtFolders = DlFolder.Folder_Read(userRIDList, eProfileType.TaskListSubFolder, true, false);
					dtFolderItems = DlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.TaskList, true, false);
					dtTasks = _dlSchedule.TaskList_Read(userRIDList, true, false);

					dtTasks.PrimaryKey = new DataColumn[] { dtTasks.Columns["TASKLIST_RID"] };
					dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

					if (!UserTskLstSecLvl.AccessDenied || !SystemTskLstSecLvl.AccessDenied)
					{
						BuildFolderBranch(SAB.ClientServerSession.UserRID, FavoritesNode.Profile.Key, FavoritesNode, dtFolderItems, dtFolders, dtTasks);
						BuildFolderBranch(SAB.ClientServerSession.UserRID, _userTaskListsNode.Profile.Key, _userTaskListsNode, dtFolderItems, dtFolders, dtTasks);
					}

					if (!GlobalTskLstSecLvl.AccessDenied || !SystemTskLstSecLvl.AccessDenied)
					{
						BuildFolderBranch(Include.GlobalUserRID, _globalTaskListsNode.Profile.Key, _globalTaskListsNode, dtFolderItems, dtFolders, dtTasks);
					}

					if (!SystemTskLstSecLvl.AccessDenied)
					{
						BuildFolderBranch(Include.SystemUserRID, _systemTaskListsNode.Profile.Key, _systemTaskListsNode, dtFolderItems, dtFolders, dtTasks);
					}

					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//if (!_systemJobSecLvl.AccessDenied)
					if (!SystemJobSecLvl.AccessDenied)
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					{
						dtFolderItems = DlFolder.Folder_Item_Read(null, folderTypeList, eProfileType.Job, true, false);
						dtJobs = _dlSchedule.Job_ReadNonSystemParent();
						dtJobs.PrimaryKey = new DataColumn[] { dtJobs.Columns["JOB_RID"] };
						BuildFolderBranch(Include.SystemUserRID, _jobsNode.Profile.Key, _jobsNode, dtFolderItems, dtFolders, dtJobs);
					}

					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//if (!_systemSpecialSecLvl.AccessDenied)
					if (!SystemSpecialSecLvl.AccessDenied)
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					{
						dtFolderItems = DlFolder.Folder_Item_Read(null, folderTypeList, eProfileType.SpecialRequest, true, false);
						dtSpecialRequests = _dlSchedule.SpecialRequest_ReadParent();
						dtSpecialRequests.PrimaryKey = new DataColumn[] { dtSpecialRequests.Columns["SPECIAL_REQ_RID"] };
						BuildFolderBranch(Include.SystemUserRID, _specialReqNode.Profile.Key, _specialReqNode, dtFolderItems, dtFolders, dtSpecialRequests);
					}

					if (!UserTskLstSecLvl.AccessDenied)
					{
						dtTaskListShortcuts = DlFolder.Folder_Shortcut_Item_Read(favoritesUserRIDList, eProfileType.TaskList);

						foreach (DataRow row in dtTaskListShortcuts.Rows)
						{
							newShortcut = new FolderShortcut(row);

							parentNode = (MIDTaskListNode)FolderNodeHash[newShortcut.ParentFolderId];
							childNode = (MIDTaskListNode)ItemNodeHash[new HashKeyObject(newShortcut.ShortcutId, (int)newShortcut.ShortcutType)];

                            // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                            if (parentNode == null ||
                                childNode == null)
                            {
                                continue;
                            }
                            // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view

							newNode = BuildObjectShortcutNode(childNode, parentNode);
						}

						dtJobShortcuts = DlFolder.Folder_Shortcut_Item_Read(favoritesUserRIDList, eProfileType.Job);

						foreach (DataRow row in dtJobShortcuts.Rows)
						{
							newShortcut = new FolderShortcut(row);

							parentNode = (MIDTaskListNode)FolderNodeHash[newShortcut.ParentFolderId];
							childNode = (MIDTaskListNode)ItemNodeHash[new HashKeyObject(newShortcut.ShortcutId, (int)newShortcut.ShortcutType)];

                            // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                            if (parentNode == null ||
                                childNode == null)
                            {
                                continue;
                            }
                            // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view

							newNode = BuildObjectShortcutNode(childNode, parentNode);
						}

						dtSpecialRequestShortcuts = DlFolder.Folder_Shortcut_Item_Read(favoritesUserRIDList, eProfileType.SpecialRequest);

						foreach (DataRow row in dtSpecialRequestShortcuts.Rows)
						{
							newShortcut = new FolderShortcut(row);

							parentNode = (MIDTaskListNode)FolderNodeHash[newShortcut.ParentFolderId];
							childNode = (MIDTaskListNode)ItemNodeHash[new HashKeyObject(newShortcut.ShortcutId, (int)newShortcut.ShortcutType)];

                            // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                            if (parentNode == null ||
                                childNode == null)
                            {
                                continue;
                            }
                            // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view

							newNode = BuildObjectShortcutNode(childNode, parentNode);
						}

						dtFolderShortcuts = DlFolder.Folder_Shortcut_Folder_Read(favoritesUserRIDList, folderTypeList);

						foreach (DataRow row in dtFolderShortcuts.Rows)
						{
							newShortcut = new FolderShortcut(row);

							parentNode = (MIDTaskListNode)FolderNodeHash[newShortcut.ParentFolderId];
							childNode = (MIDTaskListNode)FolderNodeHash[newShortcut.ShortcutId];

                            // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                            if (parentNode == null ||
                                childNode == null)
                            {
                                continue;
                            }
                            // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view

							newNode = BuildRootShortcutNode(childNode, parentNode);
						}
					}
				}

				if (FavoritesNode != null)
				{
					SortChildNodes(FavoritesNode);
				}

				if (_userTaskListsNode != null)
				{
					SortChildNodes(_userTaskListsNode);
				}

				if (_globalTaskListsNode != null)
				{
					SortChildNodes(_globalTaskListsNode);
				}

				if (_systemTaskListsNode != null)
				{
					SortChildNodes(_systemTaskListsNode);
				}

				if (_jobsNode != null)
				{
					SortChildNodes(_jobsNode);
				}

				if (_specialReqNode != null)
				{
					SortChildNodes(_specialReqNode);
				}

				//--------------------------------
				// Read and Load Shared User Nodes
				//--------------------------------

				if (!SystemTskLstSecLvl.AccessDenied)
				{
					BuildSharedNode();

					dtFolders = DlFolder.Folder_Read(null, eProfileType.TaskListSubFolder, false, false);
					dtFolderItems = DlFolder.Folder_Item_Read(null, folderTypeList, eProfileType.TaskList, true, false);
					dtTasks = _dlSchedule.TaskList_Read(null, false, false);

					dtTasks.PrimaryKey = new DataColumn[] { dtTasks.Columns["TASKLIST_RID"] };
					dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

					userTaskListsNodeList = new Hashtable();
                    // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
                    //dtUserFolders = DlFolder.Folder_Read(null, eProfileType.TaskListTaskListMainUserFolder, true, false);

                    //foreach (DataRow row in dtUserFolders.Rows)
                    //{
                    //    folderProf = new FolderProfile(row);

                    //    if (folderProf.UserRID != SAB.ClientServerSession.UserRID &&
                    //        folderProf.UserRID != Include.GlobalUserRID &&
                    //        folderProf.UserRID != Include.SystemUserRID)
                    //    {
                    //        userTaskListsNodeList[folderProf.UserRID] = folderProf;
                    //    }
                    //}

                    //iEnum = userTaskListsNodeList.GetEnumerator();

                    //while (iEnum.MoveNext())
                    //{
                    //    folderProf = (FolderProfile)iEnum.Value;

                    //    if (dtFolderItems.Select("USER_RID = " + folderProf.UserRID + " AND PARENT_FOLDER_RID = " + folderProf.Key).Length > 0)
                    //    {
                    //        userNode = BuildSharedUserNode(folderProf);
                    //        BuildFolderBranch(userNode.UserId, userNode.Profile.Key, userNode, dtFolderItems, dtFolders, dtTasks);
                    //    }
                    //}
					dtUserFolders = DlFolder.Folder_Read(null, eProfileType.TaskListTaskListMainUserFolder, false, false);

					foreach (DataRow row in dtUserFolders.Rows)
					{
						folderProf = new FolderProfile(row);

                        if (folderProf.OwnerUserRID != SAB.ClientServerSession.UserRID &&
                            folderProf.OwnerUserRID != Include.GlobalUserRID &&
                            folderProf.OwnerUserRID != Include.SystemUserRID)
						{
							userTaskListsNodeList[folderProf.OwnerUserRID] = folderProf;
						}
					}

					iEnum = userTaskListsNodeList.GetEnumerator();

					while (iEnum.MoveNext())
					{
						folderProf = (FolderProfile)iEnum.Value;

                        if (dtFolderItems.Select("OWNER_USER_RID = " + folderProf.OwnerUserRID + " AND PARENT_FOLDER_RID = " + folderProf.Key).Length > 0)
						{
							userNode = BuildSharedUserNode(folderProf);
							BuildFolderBranch(userNode.UserId, userNode.Profile.Key, userNode, dtFolderItems, dtFolders, dtTasks);
						}
					}
                    // End TT#72

					SortChildNodes(_sharedTaskListsNode);
				}
				else
				{
					userRIDList.Clear();
					userRIDList.Add(SAB.ClientServerSession.UserRID);

					dtUserFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.TaskListTaskListMainUserFolder, false, true);

					if (dtUserFolders.Rows.Count > 0)
					{
						BuildSharedNode();

						dtFolders = DlFolder.Folder_Read(userRIDList, eProfileType.TaskListSubFolder, false, true);
						dtFolderItems = DlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.TaskList, false, true);
						dtTasks = _dlSchedule.TaskList_Read(userRIDList, false, true);

						dtTasks.PrimaryKey = new DataColumn[] { dtTasks.Columns["TASKLIST_RID"] };
						dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

						foreach (DataRow row in dtUserFolders.Rows)
						{
							folderProf = new FolderProfile(row);
							userNode = BuildSharedUserNode(folderProf);
							BuildFolderBranch(SAB.ClientServerSession.UserRID, userNode.Profile.Key, userNode, dtFolderItems, dtFolders, dtTasks);
						}

						SortChildNodes(_sharedTaskListsNode);
					}
				}
			}
			catch (Exception error)
			{
				string message = error.ToString();
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
			frmTaskListProperties taskListProperties;
			OnTaskListPropertiesCloseClass taskListCloseHandler;
			frmJobProperties jobProperties;
			OnJobPropertiesCloseClass jobCloseHandler;
			frmSpecialRequestProperties specialRequestProperties;
			OnSpecialRequestPropertiesCloseClass specialRequestCloseHandler;

			try
			{
				//Begin TT#1255 - JScott - Tasklist disappears after save and refresh
				if (aParentNode.TreeNodeType == eTreeNodeType.ObjectNode)
				{
					aParentNode = aParentNode.GetParentNode();
				}

				//End TT#1255 - JScott - Tasklist disappears after save and refresh
				if (aParentNode.GetTopSourceNode().NodeProfileType == eProfileType.TaskListJobMainFolder)
				{
					jobProperties = new frmJobProperties(SAB, (MIDTaskListNode)aParentNode, false);
					jobCloseHandler = new OnJobPropertiesCloseClass(null);
					jobProperties.OnJobPropertiesSaveHandler += new frmJobProperties.JobPropertiesSaveEventHandler(OnJobPropertiesSave);
					jobProperties.OnJobPropertiesCloseHandler += new frmJobProperties.JobPropertiesCloseEventHandler(jobCloseHandler.OnClose);
					jobProperties.MdiParent = MDIParentForm;

					jobProperties.Show();
					jobProperties.BringToFront();
				}
				else if (aParentNode.GetTopSourceNode().NodeProfileType == eProfileType.TaskListSpecialRequestMainFolder)
				{
					specialRequestProperties = new frmSpecialRequestProperties(SAB, (MIDTaskListNode)aParentNode, false);
					specialRequestCloseHandler = new OnSpecialRequestPropertiesCloseClass(null);
					specialRequestProperties.OnSpecialRequestPropertiesSaveHandler += new frmSpecialRequestProperties.SpecialRequestPropertiesSaveEventHandler(OnSpecialRequestPropertiesSave);
					specialRequestProperties.OnSpecialRequestPropertiesCloseHandler += new frmSpecialRequestProperties.SpecialRequestPropertiesCloseEventHandler(specialRequestCloseHandler.OnClose);
					specialRequestProperties.MdiParent = MDIParentForm;

					specialRequestProperties.Show();
					specialRequestProperties.BringToFront();
				}
				else
				{
					taskListProperties = new frmTaskListProperties(SAB, (MIDTaskListNode)aParentNode, _userTaskListsNode, _globalTaskListsNode, _systemTaskListsNode, aParentNode.UserId, aParentNode.OwnerUserRID, false);
					taskListCloseHandler = new OnTaskListPropertiesCloseClass(null);
					taskListProperties.OnTaskListPropertiesSaveHandler += new frmTaskListProperties.TaskListPropertiesSaveEventHandler(OnTaskListPropertiesSave);
					taskListProperties.OnTaskListPropertiesCloseHandler += new frmTaskListProperties.TaskListPropertiesCloseEventHandler(taskListCloseHandler.OnClose);
					taskListProperties.MdiParent = MDIParentForm;

					taskListProperties.Show();
					taskListProperties.BringToFront();
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
			MIDTaskListNode newNode;
            // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
            MIDTreeNodeSecurityGroup nodeSecurityGroup;
            // End TT#373

			try
			{
				newNodeName = FindNewFolderName("New Folder", aUserId, aNode.Profile.Key, eProfileType.TaskListSubFolder);

				_dlFolder.OpenUpdateConnection();

				try
				{
					newFolderProf = new FolderProfile(Include.NoRID, aUserId, eProfileType.TaskListSubFolder, newNodeName, aUserId);
					newFolderProf.Key = _dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
					_dlFolder.Folder_Item_Insert(aNode.Profile.Key, newFolderProf.Key, eProfileType.TaskListSubFolder);

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

					newNode = new MIDTaskListNode(
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
					if (MIDEnvironment.isWindows)
					{
						aNode.Nodes.Insert(0, newNode);
						aNode.Expand();
					}
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
			TaskListProfile taskListProf;
			JobProfile jobProf;
			SpecialRequestProfile specialRequestProf;
			FolderProfile folderProf;
			GenericEnqueue objEnqueue;

			try
			{
				switch (aNode.NodeProfileType)
				{
					case eProfileType.TaskList:

						objEnqueue = EnqueueObject((TaskListProfile)aNode.Profile, eLockType.TaskList, false);

						if (objEnqueue == null)
						{
							return false;
						}

						try
						{
							key = _dlSchedule.TaskList_GetKey(aNewName, aNode.UserId);

							if (key != -1)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							_dlSchedule.OpenUpdateConnection();

							try
							{
								_dlSchedule.TaskList_UpdateName(aNode.Profile.Key, aNewName, SAB.ClientServerSession.UserRID);
								_dlSchedule.CommitData();
							}
							catch (Exception error)
							{
								string message = error.ToString();
								throw;
							}
							finally
							{
								_dlSchedule.CloseUpdateConnection();
							}

							taskListProf = (TaskListProfile)aNode.Profile;
							taskListProf.Name = aNewName;
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

					case eProfileType.Job:

						objEnqueue = EnqueueObject((JobProfile)aNode.Profile, eLockType.Job, false);

						if (objEnqueue == null)
						{
							return false;
						}

						try
						{
							key = _dlSchedule.Job_GetKey(aNewName);

							if (key != -1)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_JobNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							_dlSchedule.OpenUpdateConnection();

							try
							{
								_dlSchedule.Job_UpdateName(aNode.Profile.Key, aNewName, SAB.ClientServerSession.UserRID);
								_dlSchedule.CommitData();
							}
							catch (Exception error)
							{
								string message = error.ToString();
								throw;
							}
							finally
							{
								_dlSchedule.CloseUpdateConnection();
							}

							jobProf = (JobProfile)aNode.Profile;
							jobProf.Name = aNewName;
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

					case eProfileType.SpecialRequest:

						objEnqueue = EnqueueObject((SpecialRequestProfile)aNode.Profile, eLockType.SpecialRequest, false);

						if (objEnqueue == null)
						{
							return false;
						}

						try
						{
							key = _dlSchedule.SpecialRequest_GetKey(aNewName);

							if (key != -1)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SpecialRequestNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							_dlSchedule.OpenUpdateConnection();

							try
							{
								_dlSchedule.SpecialRequest_UpdateName(aNode.Profile.Key, aNewName);
								_dlSchedule.CommitData();
							}
							catch (Exception error)
							{
								string message = error.ToString();
								throw;
							}
							finally
							{
								_dlSchedule.CloseUpdateConnection();
							}

							specialRequestProf = (SpecialRequestProfile)aNode.Profile;
							specialRequestProf.Name = aNewName;
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

					case eProfileType.TaskListMainFavoritesFolder:
					case eProfileType.TaskListTaskListMainUserFolder:
					case eProfileType.TaskListTaskListMainGlobalFolder:
					case eProfileType.TaskListTaskListMainSystemFolder:
					case eProfileType.TaskListJobMainFolder:
					case eProfileType.TaskListSpecialRequestMainFolder:
					case eProfileType.TaskListSubFolder:

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
			MIDTaskListNode toNode;
			MIDTaskListNode newNode = null;

			try
			{
				BeginUpdate();

				try
				{
					switch (aToNode.NodeProfileType)
					{
						case eProfileType.TaskList:
						case eProfileType.Job:
						case eProfileType.SpecialRequest:

							toNode = (MIDTaskListNode)aToNode.Parent;
							break;

						default:

							toNode = (MIDTaskListNode)aToNode;
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
						case eProfileType.TaskListSubFolder:

							newNode = BuildRootShortcutNode((MIDTaskListNode)aFromNode, toNode);

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

						case eProfileType.TaskList:
						case eProfileType.Job:
						case eProfileType.SpecialRequest:

							newNode = BuildObjectShortcutNode((MIDTaskListNode)aFromNode, toNode);

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
			MIDTaskListNode toNode;

			try
			{
				switch (aToNode.NodeProfileType)
				{
					case eProfileType.TaskList:
					case eProfileType.Job:
					case eProfileType.SpecialRequest:

						toNode = (MIDTaskListNode)aToNode.Parent;
						break;

					default:

						toNode = (MIDTaskListNode)aToNode;
						break;
				}

				try
				{
					return MoveTaskListNode((MIDTaskListNode)aFromNode, toNode);
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
			MIDTaskListNode toNode;

			try
			{
				switch (aToNode.NodeProfileType)
				{
					case eProfileType.TaskList:
					case eProfileType.Job:
					case eProfileType.SpecialRequest:

						toNode = (MIDTaskListNode)aToNode.Parent;
						break;

					default:

						toNode = (MIDTaskListNode)aToNode;
						break;
				}

				try
				{
					return CopyTaskListNode((MIDTaskListNode)aFromNode, toNode, aFindUniqueName);
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
                    InUseTaskListNode(aNode);
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
					DeleteTaskListNode((MIDTaskListNode)aNode);
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
			MIDTaskListNode node;
			TaskListProfile taskListProf;
			JobProfile jobProf;
			SpecialRequestProfile specialRequestProf;
			DataTable dtScheduledJobs;
			DataRow[] runningJobs;
			ArrayList heldJobs = null;
			//Begin TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
			//frmTaskListProperties taskListProperties;
			//OnTaskListPropertiesCloseClass taskListCloseHandler;
			//frmJobProperties jobProperties;
			//OnJobPropertiesCloseClass jobCloseHandler;
			//End TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
			frmSpecialRequestProperties specialRequestProperties;
			OnSpecialRequestPropertiesCloseClass specialRequestCloseHandler;
			//Begin TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
			////Begin TT#572 - JScott - Cannot view a running tasklist
			////GenericEnqueue objEnqueue;
			//GenericEnqueue objEnqueue = null;
			//bool openReadOnly;
			////End TT#572 - JScott - Cannot view a running tasklist
			GenericEnqueue objEnqueue = null;
			//End TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.

			try
			{
				if (aNode != null)
				{
					node = (MIDTaskListNode)aNode;

					switch (aNode.Profile.ProfileType)
					{
						case eProfileType.TaskList:

							taskListProf = (TaskListProfile)node.Profile;

							dtScheduledJobs = _dlSchedule.ReadActiveJobsByTaskList(taskListProf.Key);

							//Begin TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
							//openReadOnly = false;

							//if (dtScheduledJobs.Rows.Count > 0)
							//{
							//    if (SAB.SchedulerServerSession != null)
							//    {
							//        //Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
							//        //runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running);
							//        runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);
							//        //End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job

							//        if (runningJobs.Length > 0)
							//        {
							//            if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningTaskList), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//            {
							//                openReadOnly = true;
							//            }
							//            else
							//            {
							//                return;
							//            }
							//        }
							//        else
							//        {
							//            runningJobs = dtScheduledJobs.Select(
							//                "EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
							//                " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

							//            if (runningJobs.Length > 0)
							//            {
							//                if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_TaskListIsActive), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//                {
							//                    //Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
							//                    //heldJobs = SAB.SchedulerServerSession.HoldAllJobs(taskListProf);
							//                    try
							//                    {
							//                        heldJobs = SAB.SchedulerServerSession.HoldAllJobs(taskListProf);
							//                    }
							//                    catch (InvalidJobStatusForAction)
							//                    {
							//                        if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningTaskList), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//                        {
							//                            openReadOnly = true;
							//                        }
							//                        else
							//                        {
							//                            return;
							//                        }
							//                    }
							//                    catch (Exception)
							//                    {
							//                        throw;
							//                    }
							//                    //End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
							//                }
							//                else
							//                {
							//                    if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_OpenTasklistAsReadOnly), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//                    {
							//                        openReadOnly = true;
							//                    }
							//                    else
							//                    {
							//                        return;
							//                    }
							//                }
							//            }
							//        }
							//    }
							//    else
							//    {
							//        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							//    }
							//}

							//if (!openReadOnly)
							//{
							//    objEnqueue = this.EnqueueObject(taskListProf, eLockType.TaskList, true);

							//    if (objEnqueue != null)
							//    {
							//        openReadOnly = objEnqueue.IsInConflict;
							//    }
							//    //Begin TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//    //else
							//    //{
							//    //    openReadOnly = true;
							//    //}
							//    //End TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//}

							////Begin TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//if (objEnqueue != null)
							//{
							////End TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//    taskListProperties = new frmTaskListProperties(SAB, (MIDTaskListNode)node.Parent, _userTaskListsNode, _globalTaskListsNode, _systemTaskListsNode, taskListProf, heldJobs, openReadOnly);
							//    taskListCloseHandler = new OnTaskListPropertiesCloseClass(objEnqueue);
							//    taskListProperties.OnTaskListPropertiesSaveHandler += new frmTaskListProperties.TaskListPropertiesSaveEventHandler(OnTaskListPropertiesSave);
							//    taskListProperties.OnTaskListPropertiesCloseHandler += new frmTaskListProperties.TaskListPropertiesCloseEventHandler(taskListCloseHandler.OnClose);
							//    taskListProperties.MdiParent = MDIParentForm;

							//    taskListProperties.Show();
							//    taskListProperties.BringToFront();
							////Begin TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//}
							////End TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							if (dtScheduledJobs.Rows.Count > 0)
							{
								if (SAB.SchedulerServerSession != null)
								{
									runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);

									if (runningJobs.Length > 0)
									{
										if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningTaskList), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
										{
											OpenTaskList(node, taskListProf, null, true, null);
										}

										return;
									}
									else
									{
										runningJobs = dtScheduledJobs.Select(
											"EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
											" OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

										if (runningJobs.Length > 0)
										{
											if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_TaskListIsActive), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
											{
												try
												{
													heldJobs = SAB.SchedulerServerSession.HoldAllJobs(taskListProf);
												}
												catch (InvalidJobStatusForAction)
												{
													if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningTaskList), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
													{
														OpenTaskList(node, taskListProf, heldJobs, true, null);
													}

													return;
												}
												catch (Exception)
												{
													throw;
												}
											}
											else
											{
												if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_OpenTasklistAsReadOnly), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
												{
													OpenTaskList(node, taskListProf, null, true, null);
												}

												return;
											}
										}
									}
								}
								else
								{
									MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								}
							}

							objEnqueue = this.EnqueueObject(taskListProf, eLockType.TaskList, true);

							if (objEnqueue != null)
							{
								OpenTaskList(node, taskListProf, heldJobs, objEnqueue.IsInConflict, objEnqueue);
							}
							//End TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.

							break;

						case eProfileType.Job:

							jobProf = (JobProfile)node.Profile;

							dtScheduledJobs = _dlSchedule.ReadScheduledJob(jobProf.Key);

							//Begin TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
							//openReadOnly = false;

							//if (dtScheduledJobs.Rows.Count > 0)
							//{
							//    if (SAB.SchedulerServerSession != null)
							//    {
							//        //Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
							//        //runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running);
							//        runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);
							//        //End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job

							//        if (runningJobs.Length > 0)
							//        {
							//            if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningJob), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//            {
							//                openReadOnly = true;
							//            }
							//            else
							//            {
							//                return;
							//            }
							//        }
							//        else
							//        {
							//            runningJobs = dtScheduledJobs.Select(
							//                "EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
							//                " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

							//            if (runningJobs.Length > 0)
							//            {
							//                if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_JobIsActive), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//                {
							//                    //Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
							//                    //heldJobs = SAB.SchedulerServerSession.HoldAllJobs(jobProf);
							//                    try
							//                    {
							//                        heldJobs = SAB.SchedulerServerSession.HoldAllJobs(jobProf);
							//                    }
							//                    catch (InvalidJobStatusForAction)
							//                    {
							//                        if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningJob), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//                        {
							//                            openReadOnly = true;
							//                        }
							//                        else
							//                        {
							//                            return;
							//                        }
							//                    }
							//                    catch (Exception)
							//                    {
							//                        throw;
							//                    }
							//                    //End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
							//                }
							//                else
							//                {
							//                    if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_OpenJobAsReadOnly), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							//                    {
							//                        openReadOnly = true;
							//                    }
							//                    else
							//                    {
							//                        return;
							//                    }
							//                }
							//            }
							//        }
							//    }
							//    else
							//    {
							//        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							//    }
							//}

							//if (!openReadOnly)
							//{
							//    objEnqueue = this.EnqueueObject(jobProf, eLockType.Job, true);

							//    if (objEnqueue != null)
							//    {
							//        openReadOnly = objEnqueue.IsInConflict;
							//    }
							//    //Begin TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//    //else
							//    //{
							//    //    openReadOnly = true;
							//    //}
							//    //End TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//}

							////Begin TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//if (objEnqueue != null)
							//{
							////End TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//    jobProperties = new frmJobProperties(SAB, (MIDTaskListNode)node.Parent, jobProf, heldJobs, openReadOnly);
							//    jobCloseHandler = new OnJobPropertiesCloseClass(objEnqueue);
							//    jobProperties.OnJobPropertiesSaveHandler += new frmJobProperties.JobPropertiesSaveEventHandler(OnJobPropertiesSave);
							//    jobProperties.OnJobPropertiesCloseHandler += new frmJobProperties.JobPropertiesCloseEventHandler(jobCloseHandler.OnClose);
							//    jobProperties.MdiParent = MDIParentForm;

							//    jobProperties.Show();
							//    jobProperties.BringToFront();
							//    //End TT#572 - JScott - Cannot view a running tasklist
							////Begin TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							//}
							////End TT#1123 - JScott - In use - open as read-only?  Opens regardless of clicking Yes or No.
							if (dtScheduledJobs.Rows.Count > 0)
							{
								if (SAB.SchedulerServerSession != null)
								{
									runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);

									if (runningJobs.Length > 0)
									{
										if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningJob), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
										{
											OpenJob(node, jobProf, null, true, null);
										}

										return;
									}
									else
									{
										runningJobs = dtScheduledJobs.Select(
											"EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
											" OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

										if (runningJobs.Length > 0)
										{
											if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_JobIsActive), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
											{
												try
												{
													heldJobs = SAB.SchedulerServerSession.HoldAllJobs(jobProf);
												}
												catch (InvalidJobStatusForAction)
												{
													if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_CannotUpdateRunningJob), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
													{
														OpenJob(node, jobProf, heldJobs, true, null);
													}

													return;
												}
												catch (Exception)
												{
													throw;
												}
											}
											else
											{
												if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_OpenJobAsReadOnly), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
												{
													OpenJob(node, jobProf, null, true, null);
												}

												return;
											}
										}
									}
								}
								else
								{
									MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								}
							}

							objEnqueue = this.EnqueueObject(jobProf, eLockType.Job, true);

							if (objEnqueue != null)
							{
								OpenJob(node, jobProf, heldJobs, objEnqueue.IsInConflict, objEnqueue);
							}
							//End TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.

							break;

						case eProfileType.SpecialRequest:

							specialRequestProf = (SpecialRequestProfile)node.Profile;

							objEnqueue = this.EnqueueObject(specialRequestProf, eLockType.SpecialRequest, true);

							if (objEnqueue != null)
							{
								specialRequestProperties = new frmSpecialRequestProperties(SAB, (MIDTaskListNode)node.Parent, specialRequestProf, heldJobs, objEnqueue.IsInConflict);
								specialRequestCloseHandler = new OnSpecialRequestPropertiesCloseClass(objEnqueue);
								specialRequestProperties.OnSpecialRequestPropertiesSaveHandler += new frmSpecialRequestProperties.SpecialRequestPropertiesSaveEventHandler(OnSpecialRequestPropertiesSave);
								specialRequestProperties.OnSpecialRequestPropertiesCloseHandler += new frmSpecialRequestProperties.SpecialRequestPropertiesCloseEventHandler(specialRequestCloseHandler.OnClose);
								specialRequestProperties.MdiParent = MDIParentForm;

								specialRequestProperties.Show();
								specialRequestProperties.BringToFront();
							}

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

		//Begin TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
		private void OpenTaskList(MIDTaskListNode aNode, TaskListProfile aTaskListProfile, ArrayList aHeldJobs, bool aOpenReadOnly, GenericEnqueue aObjEnqueue)
		{
			frmTaskListProperties taskListProperties;
			OnTaskListPropertiesCloseClass taskListCloseHandler;

			try
			{
				taskListProperties = new frmTaskListProperties(SAB, (MIDTaskListNode)aNode.Parent, _userTaskListsNode, _globalTaskListsNode, _systemTaskListsNode, aTaskListProfile, aHeldJobs, aOpenReadOnly);
				taskListCloseHandler = new OnTaskListPropertiesCloseClass(aObjEnqueue);
				taskListProperties.OnTaskListPropertiesSaveHandler += new frmTaskListProperties.TaskListPropertiesSaveEventHandler(OnTaskListPropertiesSave);
				taskListProperties.OnTaskListPropertiesCloseHandler += new frmTaskListProperties.TaskListPropertiesCloseEventHandler(taskListCloseHandler.OnClose);
				taskListProperties.MdiParent = MDIParentForm;

				taskListProperties.Show();
				taskListProperties.BringToFront();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void OpenJob(MIDTaskListNode aNode, JobProfile aJobProfile, ArrayList aHeldJobs, bool aOpenReadOnly, GenericEnqueue aObjEnqueue)
		{
			frmJobProperties jobProperties;
			OnJobPropertiesCloseClass jobCloseHandler;

			try
			{
				jobProperties = new frmJobProperties(SAB, (MIDTaskListNode)aNode.Parent, aJobProfile, aHeldJobs, aOpenReadOnly);
				jobCloseHandler = new OnJobPropertiesCloseClass(aObjEnqueue);
				jobProperties.OnJobPropertiesSaveHandler += new frmJobProperties.JobPropertiesSaveEventHandler(OnJobPropertiesSave);
				jobProperties.OnJobPropertiesCloseHandler += new frmJobProperties.JobPropertiesCloseEventHandler(jobCloseHandler.OnClose);
				jobProperties.MdiParent = MDIParentForm;

				jobProperties.Show();
				jobProperties.BringToFront();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#1133 - JScott - When running a Task List or a Scheduled Job and selecting "yes" to read only nothing appears.
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
                    foreach (MIDTaskListNode node in aStartNode.Nodes)
                    {
                        if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
                        {
                            node.RefreshShortcutNode(aChangedNode);

                            if (node.Profile.ProfileType != eProfileType.TaskList &&
                                node.Profile.ProfileType != eProfileType.Job &&
                                node.Profile.ProfileType != eProfileType.SpecialRequest)
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
                        else if (node.isSubFolder || node.isFolderShortcut)
                        {
                            RefreshShortcuts(node, aChangedNode);
                        }
                    }
                // Begin TT#62 - JSmith - Object reference error when double-click folder
                }
                // End TT#62 - JSmith
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
			return aClipboardDataType == eProfileType.TaskList ||
					aClipboardDataType == eProfileType.Job ||
					aClipboardDataType == eProfileType.SpecialRequest;
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
				if (aSelectedNodes.ClipboardDataType == eProfileType.TaskList ||
					aSelectedNodes.ClipboardDataType == eProfileType.Job ||
					aSelectedNodes.ClipboardDataType == eProfileType.SpecialRequest ||
					aSelectedNodes.ClipboardDataType == eProfileType.TaskListSubFolder)
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
        private bool InUseTaskListNode(MIDTreeNode aNode)
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
                        //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
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
				if (_mainTaskListsnode != null)
				{
					_mainTaskListsnode.Expand();
				}

				if (FavoritesNode != null)
				{
					FavoritesNode.Expand();
				}

				if (_userTaskListsNode != null)
				{
					_userTaskListsNode.Expand();
				}

				if (_jobsNode != null)
				{
					_jobsNode.Expand();
				}

				if (_specialReqNode != null)
				{
					_specialReqNode.Expand();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void CreateShortcutChildren(MIDTreeNode aFromNode, MIDTreeNode aToNode)
		{
			MIDTaskListNode newNode = null;

			try
			{
				foreach (MIDTaskListNode node in aFromNode.Nodes)
				{
					switch (node.NodeProfileType)
					{
						case eProfileType.TaskListSubFolder:

							newNode = new MIDTaskListNode(
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

						case eProfileType.TaskList:
						case eProfileType.Job:
						case eProfileType.SpecialRequest:

							newNode = new MIDTaskListNode(
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

				foreach (MIDTaskListNode node in deleteList)
				{
					if (node.Profile.Key == aDeleteNode.Profile.Key && node.Profile.ProfileType == aDeleteNode.Profile.ProfileType &&
						node.isShortcut)
					{
						DeleteChildNodes(node);
						node.Remove();
					}
					else if (node.NodeProfileType == eProfileType.TaskListSubFolder ||
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

		//-----------------
		//PROTECTED METHODS
		//-----------------

		//---------------
		//PRIVATE METHODS
		//---------------

		//Begin Track #6321 - JScott - User has ability to to create folders when security is view
		//private FunctionSecurityProfile GetUserTskLstSecLvl(int aUserRID)
		//{
		//    return (FunctionSecurityProfile)_tskLstSecLvlHash[aUserRID];
		//}
		private MIDTreeNodeSecurityGroup GetUserTskLstSecGrp(int aUserRID)
		{
			return (MIDTreeNodeSecurityGroup)_tskLstSecGrpHash[aUserRID];
		}
		//End Track #6321 - JScott - User has ability to to create folders when security is view

		private void BuildSharedNode()
		{
			FolderProfile folderProf;
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile userSecLvl;
			MIDTreeNodeSecurityGroup userSecGrp;
			//End Track #6321 - JScott - User has ability to to create folders when security is view

			try
			{
				folderProf = new FolderProfile(
					Include.NoRID,
					SAB.ClientServerSession.UserRID,
					eProfileType.TaskListTaskListMainSharedFolder,
					MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName),
					SAB.ClientServerSession.UserRID);

				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//userSecLvl = new FunctionSecurityProfile(-1);
				//userSecLvl.SetAllowView();
				userSecGrp = new MIDTreeNodeSecurityGroup(new FunctionSecurityProfile(-1), new FunctionSecurityProfile(-1));
				userSecGrp.FunctionSecurityProfile.SetAllowView();
				userSecGrp.FolderSecurityProfile.SetAllowView();
				//End Track #6321 - JScott - User has ability to to create folders when security is view

				_sharedTaskListsNode = new MIDTaskListNode(
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
					folderProf.OwnerUserRID);

                _sharedTaskListsNode.isSharedNode = true;  // TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

				FolderNodeHash[folderProf.Key] = _sharedTaskListsNode;
				_mainTaskListsnode.Nodes.Add(_sharedTaskListsNode);
			}
			catch
			{
				throw;
			}
		}

		private MIDTaskListNode BuildSharedUserNode(FolderProfile aUserFolderProf)
		{
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//FunctionSecurityProfile userSecLvl;
			MIDTreeNodeSecurityGroup userSecGrp;
			//End Track #6321 - JScott - User has ability to to create folders when security is view
			MIDTaskListNode sharedUserNode;

			try
			{
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
				//aUserFolderProf.Name = DlSecurity.GetUserName(aUserFolderProf.OwnerUserRID);
                aUserFolderProf.Name = UserNameStorage.GetUserName(aUserFolderProf.OwnerUserRID);
                //End TT#827-MD -jsobek -Allocation Reviews Performance

				//Begin Track #6321 - JScott - User has ability to to create folders when security is view
				//userSecLvl = (FunctionSecurityProfile)UserTskLstSecLvl.Clone();
				//userSecLvl.SetDenyDelete();
				//_tskLstSecLvlHash[aUserFolderProf.OwnerUserRID] = userSecLvl;
				userSecGrp = (MIDTreeNodeSecurityGroup)UserTskLstSecGrp.Clone();
				userSecGrp.FunctionSecurityProfile.SetDenyDelete();
				userSecGrp.FolderSecurityProfile.SetDenyDelete();
				_tskLstSecGrpHash[aUserFolderProf.OwnerUserRID] = userSecGrp;
				//End Track #6321 - JScott - User has ability to to create folders when security is view

				sharedUserNode = new MIDTaskListNode(
					SAB,
					eTreeNodeType.MainSourceFolderNode,
					aUserFolderProf,
					aUserFolderProf.Name,
					Include.NoRID,
					aUserFolderProf.UserRID,
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//userSecLvl,
					userSecGrp,
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					cClosedFolderImage,
					cClosedFolderImage,
					cOpenFolderImage,
					cOpenFolderImage,
					aUserFolderProf.OwnerUserRID);

                sharedUserNode.isSharedNode = true;  // TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

				FolderNodeHash[aUserFolderProf.Key] = sharedUserNode;
				_sharedTaskListsNode.Nodes.Add(sharedUserNode);

				return sharedUserNode;
			}
			catch
			{
				throw;
			}
		}

		private MIDTaskListNode BuildTaskListNode(TaskListProfile aTaskListProf, MIDTreeNode aParentNode)
		{
			MIDTaskListNode newNode;

			try
			{
				newNode = new MIDTaskListNode(
					SAB,
					eTreeNodeType.ObjectNode,
					aTaskListProf,
					aTaskListProf.Name,
					aParentNode.NodeRID,
					aTaskListProf.UserRID,
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//GetUserTskLstSecLvl(aTaskListProf.OwnerUserRID),
					GetUserTskLstSecGrp(aTaskListProf.OwnerUserRID),
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					cTaskListUnselectedImage,
					cTaskListSelectedImage,
					aTaskListProf.OwnerUserRID);

                newNode.isSharedNode = ((MIDTaskListNode)aParentNode).isSharedNode;  // TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

				ItemNodeHash[new HashKeyObject(aTaskListProf.Key, (int)aTaskListProf.ProfileType)] = newNode;
				aParentNode.Nodes.Add(newNode);

				return newNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildFolderBranch(int aUserRID, int aParentFolderRID, MIDTreeNode aParentNode, DataTable aFolderItems, DataTable aFolders, DataTable aObjects)
		{
			DataRow[] folderItemList;
			DataRow itemRow;
			FolderProfile folderProf;
			TaskListProfile taskListProf;
			JobProfile jobProf;
			SpecialRequestProfile specialRequestProf;
			MIDTaskListNode newNode;

			try
			{
				folderItemList = aFolderItems.Select("USER_RID = " + aUserRID + " AND PARENT_FOLDER_RID = " + aParentFolderRID);

				foreach (DataRow row in folderItemList)
				{
					switch ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"]))
					{
						case eProfileType.TaskListSubFolder:

							itemRow = aFolders.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Tasklist", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
							folderProf = new FolderProfile(itemRow);
							newNode = BuildSubFolderNode(folderProf, aParentNode);
							BuildFolderBranch(aUserRID, newNode.Profile.Key, newNode, aFolderItems, aFolders, aObjects);
							break;

						case eProfileType.TaskList:

							itemRow = aObjects.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Tasklist", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
							taskListProf = new TaskListProfile(itemRow);
							BuildTaskListNode(taskListProf, aParentNode);
							break;

						case eProfileType.Job:

							itemRow = aObjects.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Tasklist", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
							jobProf = new JobProfile(itemRow);
							BuildJobNode(jobProf, aParentNode);
							break;

						default:

							itemRow = aObjects.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Tasklist", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
							specialRequestProf = new SpecialRequestProfile(itemRow);
							BuildSpecialRequestNode(specialRequestProf, aParentNode);
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

		private MIDTaskListNode BuildSubFolderNode(FolderProfile aFolderProf, MIDTreeNode aParentNode)
		{
			MIDTaskListNode newNode;

			try
			{
                // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                //newNode = new MIDTaskListNode(
                //    SAB,
                //    eTreeNodeType.SubFolderNode,
                //    aFolderProf,
                //    aFolderProf.Name,
                //    aParentNode.NodeRID,
                //    aFolderProf.UserRID,
                //    //Begin Track #6321 - JScott - User has ability to to create folders when security is view
                //    //GetUserTskLstSecLvl(aFolderProf.OwnerUserRID),
                //    GetUserTskLstSecGrp(aFolderProf.OwnerUserRID),
                //    //End Track #6321 - JScott - User has ability to to create folders when security is view
                //    cClosedFolderImage,
                //    cClosedFolderImage,
                //    cOpenFolderImage,
                //    cOpenFolderImage,
                //    aFolderProf.OwnerUserRID);
                if (aParentNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    newNode = new MIDTaskListNode(
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
                    newNode = new MIDTaskListNode(
                    SAB,
                    eTreeNodeType.SubFolderNode,
                    aFolderProf,
                    aFolderProf.Name,
                    aParentNode.NodeRID,
                    aFolderProf.UserRID,
                    GetUserTskLstSecGrp(aFolderProf.OwnerUserRID),
                    cClosedFolderImage,
                    cClosedFolderImage,
                    cOpenFolderImage,
                    cOpenFolderImage,
                    aFolderProf.OwnerUserRID);
                }
                // End TT#42

                newNode.isSharedNode = ((MIDTaskListNode)aParentNode).isSharedNode;  // TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

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

		private MIDTaskListNode BuildJobNode(JobProfile aJobProf, MIDTreeNode aParentNode)
		{
			MIDTaskListNode newNode;

			try
			{
				newNode = new MIDTaskListNode(
					SAB,
					eTreeNodeType.ObjectNode,
					aJobProf,
					aJobProf.Name,
					aParentNode.NodeRID,
					Include.SystemUserRID,
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//_systemJobSecLvl,
					_systemJobSecGrp,
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					cJobUnselectedImage,
					cJobSelectedImage,
					Include.SystemUserRID);

				ItemNodeHash[new HashKeyObject(aJobProf.Key, (int)aJobProf.ProfileType)] = newNode;
				aParentNode.Nodes.Add(newNode);

				return newNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MIDTaskListNode BuildSpecialRequestNode(SpecialRequestProfile aSpecialRequestProf, MIDTreeNode aParentNode)
		{
			MIDTaskListNode newNode;

			try
			{
				newNode = new MIDTaskListNode(
					SAB,
					eTreeNodeType.ObjectNode,
					aSpecialRequestProf,
					aSpecialRequestProf.Name,
					aParentNode.NodeRID,
					Include.SystemUserRID,
					//Begin Track #6321 - JScott - User has ability to to create folders when security is view
					//_systemSpecialSecLvl,
					_systemSpecialSecGrp,
					//End Track #6321 - JScott - User has ability to to create folders when security is view
					cSpecialRequestUnselectedImage,
					cSpecialRequestSelectedImage,
					Include.SystemUserRID);

				ItemNodeHash[new HashKeyObject(aSpecialRequestProf.Key, (int)aSpecialRequestProf.ProfileType)] = newNode;
				aParentNode.Nodes.Add(newNode);

				return newNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MIDTaskListNode BuildRootShortcutNode(MIDTaskListNode aFromNode, MIDTaskListNode aToNode)
		{
			MIDTaskListNode newNode;
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

				newNode = new MIDTaskListNode(
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

		private MIDTaskListNode BuildObjectShortcutNode(MIDTaskListNode aFromNode, MIDTaskListNode aToNode)
		{
			MIDTaskListNode newNode;
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

				if (aFromNode.NodeProfileType == eProfileType.TaskList)
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
					newNode = new MIDTaskListNode(
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
						cTaskListShortcutUnselectedImage,
						cTaskListShortcutSelectedImage,
						aFromNode.OwnerUserRID);
				}
				else if (aFromNode.NodeProfileType == eProfileType.Job)
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
					newNode = new MIDTaskListNode(
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
						cJobShortcutUnselectedImage,
						cJobShortcutSelectedImage,
						aFromNode.OwnerUserRID);
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
					newNode = new MIDTaskListNode(
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
						cSpecialRequestUnselectedImage,
						cSpecialRequestShortcutSelectedImage,
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

		private GenericEnqueue EnqueueObject(TaskListProfile aProfile, eLockType aLockType, bool aAllowReadOnly)
		{
			GenericEnqueue objEnqueue;
			string errMsg;

			try
			{
				objEnqueue = new GenericEnqueue(aLockType, aProfile.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

				try
				{
					objEnqueue.EnqueueGeneric();
				}
				catch (GenericConflictException)
				{
                    /* Begin TT#1159 - Improve Messaging */
                    string[] errParms = new string[3];
                    errParms.SetValue("Task List", 0);
                    errParms.SetValue(aProfile.Name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

					//errMsg = "The Task List \"" + aProfile.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
                    /* End TT#1159 */

					if (aAllowReadOnly)
					{
						errMsg += System.Environment.NewLine + System.Environment.NewLine;
						errMsg += "Do you wish to continue with the Task List as read-only?";

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

		private GenericEnqueue EnqueueObject(JobProfile aProfile, eLockType aLockType, bool aAllowReadOnly)
		{
			GenericEnqueue objEnqueue;
			string errMsg;

			try
			{
				objEnqueue = new GenericEnqueue(aLockType, aProfile.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

				try
				{
					objEnqueue.EnqueueGeneric();
				}
				catch (GenericConflictException)
				{
                    /* Begin TT#1159 - Improve Messaging */
                    string[] errParms = new string[3];
                    errParms.SetValue("Job", 0);
                    errParms.SetValue(aProfile.Name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

					//errMsg = "The Job \"" + aProfile.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
                    /* End TT#1159 */

					if (aAllowReadOnly)
					{
						errMsg += System.Environment.NewLine + System.Environment.NewLine;
						errMsg += "Do you wish to continue with the Job as read-only?";

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

		private GenericEnqueue EnqueueObject(SpecialRequestProfile aProfile, eLockType aLockType, bool aAllowReadOnly)
		{
			GenericEnqueue objEnqueue;
			string errMsg;

			try
			{
				objEnqueue = new GenericEnqueue(aLockType, aProfile.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

				try
				{
					objEnqueue.EnqueueGeneric();
				}
				catch (GenericConflictException)
				{
                    /* Begin TT#1159 - Improve Messaging */
                    string[] errParms = new string[3];
                    errParms.SetValue("Special Request", 0);
                    errParms.SetValue(aProfile.Name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

					//errMsg = "The Special Request \"" + aProfile.Name + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
                    /* End TT#1159 */

					if (aAllowReadOnly)
					{
						errMsg += System.Environment.NewLine + System.Environment.NewLine;
						errMsg += "Do you wish to continue with the Special Request as read-only?";

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

		private string FindNewTaskListName(string aTaskListName, int aUserRID)
		{
			int index;
			string newName;
			int key;

			try
			{
				index = 0;
				newName = aTaskListName;
				key = _dlSchedule.TaskList_GetKey(newName, aUserRID);

				while (key != -1)
				{
					index++;

					//if (index > 1)
					//{
					//	newName = "Copy (" + index + ") of " + aTaskListName;
					//}
					//else
					//{
					//	newName = "Copy of " + aTaskListName;
					//}
                    newName = Include.GetNewName(name: aTaskListName, index: index);

					key = _dlSchedule.TaskList_GetKey(newName, aUserRID);
				}

				return newName;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private string FindNewJobName(string aJobName, int aUserRID)
		{
			int index;
			string newName;
			int key;

			try
			{
				index = 0;
				newName = aJobName;
				key = _dlSchedule.Job_GetKey(newName);

				while (key != -1)
				{
					index++;

                    //if (index > 1)
                    //{
                    //	newName = "Copy (" + index + ") of " + aJobName;
                    //}
                    //else
                    //{
                    //	newName = "Copy of " + aJobName;
                    //}
                    newName = Include.GetNewName(name: aJobName, index: index);

					key = _dlSchedule.Job_GetKey(newName);
				}

				return newName;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private string FindNewSpecialRequestName(string aSpecialRequestName, int aUserRID)
		{
			int index;
			string newName;
			int key;

			try
			{
				index = 0;
				newName = aSpecialRequestName;
				key = _dlSchedule.SpecialRequest_GetKey(newName);

				while (key != -1)
				{
					index++;

					//if (index > 1)
					//{
					//	newName = "Copy (" + index + ") of " + aSpecialRequestName;
					//}
					//else
					//{
					//	newName = "Copy of " + aSpecialRequestName;
					//}
                    newName = Include.GetNewName(name: aSpecialRequestName, index: index);

					key = _dlSchedule.SpecialRequest_GetKey(newName);
				}

				return newName;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MIDTaskListNode MoveTaskListNode(MIDTaskListNode aFromNode, MIDTaskListNode aToNode)
		{
			MIDTaskListNode newNode;
			FolderProfile folderProf;
			TaskListProfile taskListProf;
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
				else if (aFromNode.NodeProfileType == eProfileType.TaskListSubFolder)
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

						folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.TaskListSubFolder);
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

                    foreach (MIDTaskListNode node in moveArray)
					{
						MoveTaskListNode(node, newNode);
					}

					aFromNode.Remove();

					return newNode;
				}
				else if (aFromNode.NodeProfileType == eProfileType.TaskList)
				{
					taskListProf = (TaskListProfile)aFromNode.Profile;

					if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
					{
						DlSecurity.OpenUpdateConnection();

						try
						{
							DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(taskListProf.ProfileType), taskListProf.Key);
							DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(taskListProf.ProfileType), taskListProf.Key, aToNode.UserId);

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

						taskListProf.Name = FindNewTaskListName(taskListProf.Name, aToNode.UserId);
						taskListProf.UserRID = aToNode.UserId;
						taskListProf.OwnerUserRID = aToNode.UserId;
					}

					_dlSchedule.OpenUpdateConnection();
					_dlFolder.OpenUpdateConnection();

					try
					{
						_dlSchedule.TaskList_Update(taskListProf, SAB.ClientServerSession.UserRID);
						_dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
						_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

						_dlFolder.CommitData();
						_dlSchedule.CommitData();
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_dlFolder.CloseUpdateConnection();
						_dlSchedule.CloseUpdateConnection();
					}

					aFromNode.Remove();
					newNode = BuildTaskListNode(taskListProf, aToNode);
					return newNode;
				}
				else if (aFromNode.NodeProfileType == eProfileType.Job || aFromNode.NodeProfileType == eProfileType.SpecialRequest)
				{
					_dlFolder.OpenUpdateConnection();

					try
					{
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

					aFromNode.Remove();

					if (aFromNode.NodeProfileType == eProfileType.Job)
					{
						newNode = BuildJobNode((JobProfile)aFromNode.Profile, aToNode);
					}
					else
					{
						newNode = BuildSpecialRequestNode((SpecialRequestProfile)aFromNode.Profile, aToNode);
					}

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

		private MIDTaskListNode CopyTaskListNode(MIDTaskListNode aFromNode, MIDTaskListNode aToNode, bool aFindUniqueName)
		{
			FolderProfile folderProf;
			TaskListProfile taskListProf;
			JobProfile jobProf;
			SpecialRequestProfile specialRequestProf;
			MIDTaskListNode newNode;

			try
			{
				if (aFromNode.isSubFolder)
				{
					folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
					folderProf.UserRID = aToNode.UserId;
					folderProf.OwnerUserRID = aToNode.UserId;

					if (aFindUniqueName)
					{
						folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.TaskListSubFolder);
					}

					_dlFolder.OpenUpdateConnection();

					try
					{
						folderProf.Key = _dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, eProfileType.TaskListSubFolder);
						_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, folderProf.Key, eProfileType.TaskListSubFolder);

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

					foreach (MIDTaskListNode node in aFromNode.Nodes)
					{
						CopyTaskListNode(node, newNode, aFindUniqueName);
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
					switch (aFromNode.NodeProfileType)
					{
						case eProfileType.TaskList:

                            //Begin TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
                            //Do not allow allocate tasks with user filters to be pasted into global
                            DataTable dtTaskAllocate = _dlSchedule.TaskAllocate_ReadByTaskList(aFromNode.Profile.Key);
                            if (dtTaskAllocate.Rows.Count > 0) //is this an allocate task?
                            {
                                if (aToNode.OwnerUserRID == Include.GlobalUserRID || aToNode.OwnerUserRID == Include.SystemUserRID) //is the To Node a global or system node?
                                {
                                    //is the filter on this allocate task a user filter?
                                    if (dtTaskAllocate.Rows[0]["FILTER_RID"] != DBNull.Value)
                                    {
                                        int allocateTaskHeaderFilterRID = Convert.ToInt32(dtTaskAllocate.Rows[0]["FILTER_RID"]);
                                        FilterData fd = new FilterData();
                                        DataTable dtAllocateTaskHeaderFilter = fd.FilterRead(allocateTaskHeaderFilterRID);
                                        if (dtAllocateTaskHeaderFilter.Rows.Count > 0)
                                        {
                                            int filterOwnerUserRID = Convert.ToInt32(dtAllocateTaskHeaderFilter.Rows[0]["OWNER_USER_RID"]);
                                            if (filterOwnerUserRID != Include.GlobalUserRID)
                                            {
                                                //this is a user filter, so display a message and return null;
                                                string msgText = MIDText.GetText(eMIDTextCode.msg_DenyCopyingUserAllocateTaskToGlobal);
                                                string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_DenyCopyingUserAllocateTaskToGlobalCaption);
                                                MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                return null;
                                            }
                                        }
                                       
                                    }
                                }
                            }
                            //End TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number

							taskListProf = (TaskListProfile)((TaskListProfile)aFromNode.Profile).Clone();
							taskListProf.UserRID = aToNode.UserId;
							taskListProf.OwnerUserRID = aToNode.UserId;

							if (aFindUniqueName)
							{
								taskListProf.Name = FindNewTaskListName(taskListProf.Name, aToNode.UserId);
							}
                            // Begin TT#1966 - JSmith - ExecuteNonQuery Connection Error
                            DataTable dtTask = _dlSchedule.Task_ReadByTaskList(aFromNode.Profile.Key);
                            //DataTable dtTaskAllocate = _dlSchedule.TaskAllocate_ReadByTaskList(aFromNode.Profile.Key);   //TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
                            DataTable dtTaskAllocateDetail = _dlSchedule.TaskAllocateDetail_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskForecast = _dlSchedule.TaskForecast_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskForecastDetail = _dlSchedule.TaskForecastDetail_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskPosting = _dlSchedule.TaskPosting_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskProgram = _dlSchedule.TaskProgram_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskRollup = _dlSchedule.TaskRollup_ReadByTaskList(aFromNode.Profile.Key);
                            // End TT#1966
                            //BEGIN TT#3999-VStuart-Task List Explorer-Size Day to Week Summary task does not copy fields
                            DataTable dtTaskSizeDyWkSum = _dlSchedule.TaskSizeDayToWeekSummary_ReadByTaskList(aFromNode.Profile.Key);
                            //END TT#3999-VStuart-Task List Explorer-Size Day to Week Summary task does not copy fields
                            //BEGIN TT#3997-Task List Explorer - Size Curve Method Task does not copy properly
                            //DataTable dtTaskSizeCurveMeth = _dlSchedule.TaskSizeCurveMethod_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskSizeCurveGenMeth = _dlSchedule.TaskSizeCurveMethod_ReadByTaskList(aFromNode.Profile.Key);
                            //END TT#3997-Task List Explorer - Size Curve Method Task does not copy properly
                            // Begin TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
                            DataTable dtTaskSizeCurve = _dlSchedule.TaskSizeCurves_ReadByTaskList(aFromNode.Profile.Key);
                            DataTable dtTaskSizeCurveGenNode = _dlSchedule.TaskSizeCurveGenerateNode_ReadByTaskList(aFromNode.Profile.Key);
                            // End TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
							_dlSchedule.OpenUpdateConnection();
							_dlFolder.OpenUpdateConnection();

							try
							{
								taskListProf.Key = _dlSchedule.TaskList_Insert(taskListProf, taskListProf.OwnerUserRID);
                                // Begin TT#1966 - JSmith - ExecuteNonQuery Connection Error
                                //_dlSchedule.Task_Insert(_dlSchedule.Task_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskAllocate_Insert(_dlSchedule.TaskAllocate_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskAllocateDetail_Insert(_dlSchedule.TaskAllocateDetail_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskForecast_Insert(_dlSchedule.TaskForecast_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskForecastDetail_Insert(_dlSchedule.TaskForecastDetail_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskPosting_Insert(_dlSchedule.TaskPosting_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskProgram_Insert(_dlSchedule.TaskProgram_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                //_dlSchedule.TaskRollup_Insert(_dlSchedule.TaskRollup_ReadByTaskList(aFromNode.Profile.Key), taskListProf.Key);
                                _dlSchedule.Task_Insert(dtTask, taskListProf.Key);
                                _dlSchedule.TaskAllocate_Insert(dtTaskAllocate, taskListProf.Key);
                                _dlSchedule.TaskAllocateDetail_Insert(dtTaskAllocateDetail, taskListProf.Key);
                                _dlSchedule.TaskForecast_Insert(dtTaskForecast, taskListProf.Key);
                                _dlSchedule.TaskForecastDetail_Insert(dtTaskForecastDetail, taskListProf.Key);
                                _dlSchedule.TaskPosting_Insert(dtTaskPosting, taskListProf.Key);
                                _dlSchedule.TaskProgram_Insert(dtTaskProgram, taskListProf.Key);
                                _dlSchedule.TaskRollup_Insert(dtTaskRollup, taskListProf.Key);
                                // End TT#1966
                                //BEGIN TT#3999-VStuart-Task List Explorer-Size Day to Week Summary task does not copy fields
                                _dlSchedule.TaskSizeDayToWeekSummary_Insert(dtTaskSizeDyWkSum, taskListProf.Key);
                                //END TT#3999-VStuart-Task List Explorer-Size Day to Week Summary task does not copy fields
                                //BEGIN TT#3997-VStuart-Task List Explorer - Size Curve Method Task does not copy properly
                                _dlSchedule.TaskSizeCurveGenerate_Insert(dtTaskSizeCurveGenMeth, taskListProf.Key, eSizeCurveGenerateType.Method);
                                //END TT#3997-VStuart-Task List Explorer - Size Curve Method Task does not copy properly
                                // Begin TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
                                _dlSchedule.TaskSizeCurves_Insert(dtTaskSizeCurve, taskListProf.Key);
                                _dlSchedule.TaskSizeCurveGenerateNode_Insert(dtTaskSizeCurveGenNode, taskListProf.Key);
                                // End TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
                                _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, taskListProf.Key, eProfileType.TaskList);

								_dlSchedule.CommitData();
								_dlFolder.CommitData();
							}
							catch (Exception exc)
							{
								string message = exc.ToString();
								throw;
							}
							finally
							{
								_dlSchedule.CloseUpdateConnection();
								_dlFolder.CloseUpdateConnection();
							}

							newNode = BuildTaskListNode(taskListProf, aToNode);

							return newNode;

						case eProfileType.Job:

							jobProf = (JobProfile)((JobProfile)aFromNode.Profile).Clone();

							if (aFindUniqueName)
							{
								jobProf.Name = FindNewJobName(jobProf.Name, aToNode.UserId);
							}

                            // Begin TT#1966 - JSmith - ExecuteNonQuery Connection Error
                            DataTable dtJobTaskListJoin = _dlSchedule.JobTaskListJoin_ReadByJob(aFromNode.Profile.Key);
                            // End TT#1966

							_dlSchedule.OpenUpdateConnection();
							_dlFolder.OpenUpdateConnection();

							try
							{
								jobProf.Key = _dlSchedule.Job_Insert(jobProf, Include.SystemUserRID);
                                // Begin TT#1966 - JSmith - ExecuteNonQuery Connection Error
                                //_dlSchedule.JobTaskListJoin_Insert(_dlSchedule.JobTaskListJoin_ReadByJob(aFromNode.Profile.Key), jobProf.Key);
                                _dlSchedule.JobTaskListJoin_Insert(dtJobTaskListJoin, jobProf.Key);
                                // End TT#1966
								_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, jobProf.Key, eProfileType.Job);

								_dlSchedule.CommitData();
								_dlFolder.CommitData();
							}
							catch (Exception exc)
							{
								string message = exc.ToString();
								throw;
							}
							finally
							{
								_dlSchedule.CloseUpdateConnection();
								_dlFolder.CloseUpdateConnection();
							}

							newNode = BuildJobNode(jobProf, aToNode);

							return newNode;

						case eProfileType.SpecialRequest:

							specialRequestProf = (SpecialRequestProfile)((SpecialRequestProfile)aFromNode.Profile).Clone();

							if (aFindUniqueName)
							{
								specialRequestProf.Name = FindNewSpecialRequestName(specialRequestProf.Name, aToNode.UserId);
							}

                            // Begin TT#1966 - JSmith - ExecuteNonQuery Connection Error
                            DataTable dtSpecialRequestJoin = _dlSchedule.SpecialRequestJoin_ReadBySpecialRequest(aFromNode.Profile.Key);
                            // End TT#1966

							_dlSchedule.OpenUpdateConnection();
							_dlFolder.OpenUpdateConnection();

							try
							{
								specialRequestProf.Key = _dlSchedule.SpecialRequest_Insert(specialRequestProf);
                                // Begin TT#1966 - JSmith - ExecuteNonQuery Connection Error
                                //_dlSchedule.SpecialRequestJoin_Insert(_dlSchedule.SpecialRequestJoin_ReadBySpecialRequest(aFromNode.Profile.Key), specialRequestProf.Key);
                                _dlSchedule.SpecialRequestJoin_Insert(dtSpecialRequestJoin, specialRequestProf.Key);
                                // End TT#1966
								_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, specialRequestProf.Key, eProfileType.SpecialRequest);

								_dlSchedule.CommitData();
								_dlFolder.CommitData();
							}
							catch (Exception exc)
							{
								string message = exc.ToString();
								throw;
							}
							finally
							{
								_dlSchedule.CloseUpdateConnection();
								_dlFolder.CloseUpdateConnection();
							}

							newNode = BuildSpecialRequestNode(specialRequestProf, aToNode);

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

		private bool DeleteTaskListNode(MIDTaskListNode aNode)
		{
			GenericEnqueue objEnqueue;
			TaskListProfile taskListProf;
			JobProfile jobProf;
			SpecialRequestProfile specialRequestProf;
			DataTable dtScheduledJobs;
			DataTable dtJobs;
			DataRow[] runningJobs;
			bool invalidJobStatusFound;
			object[] deleteArray;

			try
			{
				if (aNode.isObject)
				{
					switch (aNode.NodeProfileType)
					{
						case eProfileType.TaskList:

							taskListProf = (TaskListProfile)aNode.Profile;

							objEnqueue = EnqueueObject(taskListProf, eLockType.TaskList, false);

							if (objEnqueue == null)
							{
								return false;
							}

							try
							{
								dtScheduledJobs = _dlSchedule.ReadNonSystemJobsByTaskList(taskListProf.Key);

								if (dtScheduledJobs.Rows.Count > 0)
								{
									MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}

								dtScheduledJobs = _dlSchedule.ReadScheduledSystemJobsByTaskList(taskListProf.Key);

								if (dtScheduledJobs.Rows.Count > 0)
								{
									if (SAB.SchedulerServerSession == null)
									{
										MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}
									else
									{
										//Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
										//runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running);
										runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);
										//End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job

										if (runningJobs.Length > 0)
										{
											MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteRunningTaskList), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
											return false;
										}

										runningJobs = dtScheduledJobs.Select(
											"EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
											" OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

										if (runningJobs.Length > 0)
										{
											if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_TaskListIsScheduled), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
											{
												return false;
											}
										}
									}
								}

								dtJobs = _dlSchedule.ReadSystemJobsByTaskList(taskListProf.Key);

								_dlSchedule.OpenUpdateConnection();
								_dlFolder.OpenUpdateConnection();

								try
								{
									if (dtScheduledJobs.Rows.Count > 0)
									{
										invalidJobStatusFound = SAB.SchedulerServerSession.DeleteSchedulesFromList(dtScheduledJobs);
									}
									else
									{
										invalidJobStatusFound = false;
									}

									if (!invalidJobStatusFound)
									{
										if (dtJobs.Rows.Count > 0)
										{
											_dlSchedule.JobTaskListJoin_DeleteSystemFromList(dtJobs);
											_dlSchedule.Job_DeleteSystemFromList(dtJobs);
										}

										_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.TaskList);
										_dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.TaskList);
										_dlSchedule.TaskList_Delete(taskListProf.Key);

										_dlSchedule.CommitData();
										_dlFolder.CommitData();
									}
									else
									{
										MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}
								}
								catch (DatabaseForeignKeyViolation)
								{
									MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
								catch (Exception error)
								{
									string message = error.ToString();
									throw;
								}
								finally
								{
									_dlSchedule.CloseUpdateConnection();
									_dlFolder.CloseUpdateConnection();

									RefreshScheduleBrowserWindow();
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

							break;

						case eProfileType.Job:

							jobProf = (JobProfile)aNode.Profile;

							objEnqueue = EnqueueObject(jobProf, eLockType.Job, false);

							if (objEnqueue == null)
							{
								return false;
							}
							try
							{
								dtScheduledJobs = _dlSchedule.ReadSpecialRequestsByJob(jobProf.Key);

								if (dtScheduledJobs.Rows.Count > 0)
								{
									MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}

								dtScheduledJobs = _dlSchedule.ReadScheduledJob(jobProf.Key);

								if (dtScheduledJobs.Rows.Count > 0)
								{
									if (SAB.SchedulerServerSession == null)
									{
										MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}
									else
									{
										//Begin TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job
										//runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running);
										runningJobs = dtScheduledJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);
										//End TT#628 - JScott - Receive "InvalidJobStatusForAction" exception when editing a Job

										if (runningJobs.Length > 0)
										{
											MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteRunningJob), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
											return false;
										}

										runningJobs = dtScheduledJobs.Select(
											"EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
											" OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

										if (runningJobs.Length > 0)
										{
											if (MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_TaskListIsScheduled), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
											{
												return false;
											}
										}
									}
								}

								_dlSchedule.OpenUpdateConnection();
								_dlFolder.OpenUpdateConnection();

								try
								{
									if (dtScheduledJobs.Rows.Count > 0)
									{
										invalidJobStatusFound = SAB.SchedulerServerSession.DeleteSchedulesFromList(dtScheduledJobs);
									}
									else
									{
										invalidJobStatusFound = false;
									}

									if (!invalidJobStatusFound)
									{
										_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.Job);
										_dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.Job);
										_dlSchedule.JobTaskListJoin_DeleteByJob(jobProf.Key);
										_dlSchedule.Job_Delete(jobProf.Key);

										_dlSchedule.CommitData();
										_dlFolder.CommitData();
									}
									else
									{
										MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}
								}
								catch (Exception error)
								{
									string message = error.ToString();
									throw;
								}
								finally
								{
									_dlSchedule.CloseUpdateConnection();
									_dlFolder.CloseUpdateConnection();
									if (MIDEnvironment.isWindows)
										RefreshScheduleBrowserWindow();
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

							break;

						case eProfileType.SpecialRequest:

							specialRequestProf = (SpecialRequestProfile)aNode.Profile;

							objEnqueue = EnqueueObject(specialRequestProf, eLockType.SpecialRequest, false);

							if (objEnqueue == null)
							{
								return false;
							}

							try
							{
								_dlSchedule.OpenUpdateConnection();
								_dlFolder.OpenUpdateConnection();

								try
								{
									_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.SpecialRequest);
									_dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.SpecialRequest);
									_dlSchedule.SpecialRequestJoin_DeleteBySpecialRequest(specialRequestProf.Key);
									_dlSchedule.SpecialRequest_Delete(specialRequestProf.Key);

									_dlSchedule.CommitData();
									_dlFolder.CommitData();
								}
								catch (Exception error)
								{
									string message = error.ToString();
									throw;
								}
								finally
								{
									_dlSchedule.CloseUpdateConnection();
									_dlFolder.CloseUpdateConnection();

									RefreshScheduleBrowserWindow();
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

							break;
					}
				}
				else if (aNode.isSubFolder)
				{
					deleteArray = new object[aNode.Nodes.Count];
					aNode.Nodes.CopyTo(deleteArray, 0);

					foreach (MIDTaskListNode node in deleteArray)
					{
						DeleteTaskListNode(node);
					}

					if (aNode.Nodes.Count == 0)
					{
						_dlFolder.OpenUpdateConnection();

						try
						{
							_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.TaskListSubFolder);
							_dlFolder.Folder_Delete(aNode.Profile.Key, eProfileType.TaskListSubFolder);
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
						DeleteChildNodes((MIDTaskListNode)aNode);
						aNode.Remove();
					}
				}
				else if (aNode.isObjectShortcut)
				{
					_dlFolder.OpenUpdateConnection();

					try
					{
						_dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, aNode.NodeProfileType);
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
						_dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.TaskListSubFolder);
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

					DeleteChildNodes((MIDTaskListNode)aNode);
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

		private void OnJobPropertiesSave(object source, JobPropertiesSaveEventArgs e)
		{
			try
			{
				AfterPropertiesSave(e.ParentNode, e.JobProfile, e.JobProfile.Name, Include.SystemUserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void OnSpecialRequestPropertiesSave(object source, SpecialRequestPropertiesSaveEventArgs e)
		{
			try
			{
				AfterPropertiesSave(e.ParentNode, e.SpecialRequestProfile, e.SpecialRequestProfile.Name, Include.SystemUserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void OnTaskListPropertiesSave(object source, TaskListPropertiesSaveEventArgs e)
		{
			try
			{
				AfterPropertiesSave(e.ParentNode, e.TaskListProfile, e.TaskListProfile.Name, e.TaskListProfile.UserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void AfterPropertiesSave(MIDTaskListNode aToNode, Profile aProfile, string aProfileName, int aUserRID)
		{
            MIDTaskListNode node;
			MIDTaskListNode parentNode;

            try
            {
				node = (MIDTaskListNode)ItemNodeHash[new HashKeyObject(aProfile.Key, (int)aProfile.ProfileType)];

                if (node == null)
                {
					switch (aToNode.NodeProfileType)
					{
						case eProfileType.TaskList:
						case eProfileType.Job:
						case eProfileType.SpecialRequest:

							parentNode = (MIDTaskListNode)aToNode.Parent;
							break;

						default:

							parentNode = (MIDTaskListNode)aToNode;
							break;
					}

					switch (aProfile.ProfileType)
					{
						case eProfileType.TaskList:
							node = BuildTaskListNode((TaskListProfile)aProfile, parentNode);
							break;

						case eProfileType.Job:
							node = BuildJobNode((JobProfile)aProfile, parentNode);
							break;

						default:
							node = BuildSpecialRequestNode((SpecialRequestProfile)aProfile, parentNode);
							break;
					}

					SelectedNode = node;
                }
                else
                {
                    parentNode = (MIDTaskListNode)node.Parent;
					//Begin Track #6201 - JScott - Store Count removed from attr sets
					//node.Text = aProfileName;
					node.InternalText = aProfileName;
					//End Track #6201 - JScott - Store Count removed from attr sets
				}

                SortChildNodes(parentNode);
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		private void RefreshScheduleBrowserWindow()
		{
			frmScheduleBrowser schedBrowser;

			try
			{
				schedBrowser = GetScheduleBrowserWindow();

				if (schedBrowser != null)
				{
					schedBrowser.Refresh();
				}
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		private frmScheduleBrowser GetScheduleBrowserWindow()
		{
			try
			{
				foreach (Form childForm in MDIParentForm.MdiChildren)
				{
					if (childForm.GetType() == typeof(frmScheduleBrowser))
					{
						return (frmScheduleBrowser)childForm;
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
    }

	public class OnTaskListPropertiesCloseClass
	{
		private GenericEnqueue _objEnqueue;

		public OnTaskListPropertiesCloseClass(GenericEnqueue aObjEnqueue)
		{
			_objEnqueue = aObjEnqueue;
		}

		public void OnClose(object source, TaskListPropertiesCloseEventArgs e)
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

	public class OnJobPropertiesCloseClass
	{
		private GenericEnqueue _objEnqueue;

		public OnJobPropertiesCloseClass(GenericEnqueue aObjEnqueue)
		{
			_objEnqueue = aObjEnqueue;
		}

		public void OnClose(object source, JobPropertiesCloseEventArgs e)
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

	public class OnSpecialRequestPropertiesCloseClass
	{
		private GenericEnqueue _objEnqueue;

		public OnSpecialRequestPropertiesCloseClass(GenericEnqueue aObjEnqueue)
		{
			_objEnqueue = aObjEnqueue;
		}

		public void OnClose(object source, SpecialRequestPropertiesCloseEventArgs e)
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

	public class MIDTaskListNode : MIDTreeNode
	{
        private bool _isSharedNode = false;  // TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

		//=============
		// CONSTRUCTORS
		//=============

		public MIDTaskListNode()
			: base()
		{
		}

		public MIDTaskListNode(
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
		}

		public MIDTaskListNode(
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
		}

		//===========
		// PROPERTIES
		//===========

        // Begin TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer
        public bool isSharedNode
        {
            get { return _isSharedNode; }
            set { _isSharedNode = value; }
        }
        // End TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

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
                if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
                {
                    return false;
                }

				allowDrag = false;

				if (!isChildShortcut &&
					(NodeProfileType == eProfileType.TaskListSubFolder ||
					NodeProfileType == eProfileType.TaskList ||
					NodeProfileType == eProfileType.Job ||
					NodeProfileType == eProfileType.SpecialRequest))
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

						if (aDestinationNode.GetTopNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode &&
							GetTopNode().NodeProfileType != aDestinationNode.GetTopNode().NodeProfileType)
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
                                // Begin TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
                                //return true;
                                ScheduleData dlSchedule = new ScheduleData();
                                DataTable dtTask = dlSchedule.Task_ReadByTaskList(Profile.Key);
                                foreach (DataRow dr in dtTask.Rows)
                                {
                                    if (!((TaskListTreeView)this.TreeView).AvailableTasks.ContainsKey(Convert.ToInt32(dr["TASK_TYPE"])))
                                    {
                                        return false;
                                    }

                                }
                                // End TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
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

							return FunctionSecurityProfile.AllowDelete;
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
				//if (FunctionSecurityProfile.AllowUpdate && !isShortcut &&
				//    (NodeProfileType == eProfileType.TaskListMainFavoritesFolder ||
				//    NodeProfileType == eProfileType.TaskListTaskListMainUserFolder ||
				//    NodeProfileType == eProfileType.TaskListSubFolder ||
				//    NodeProfileType == eProfileType.TaskList ||
				//    NodeProfileType == eProfileType.Job ||
				//    NodeProfileType == eProfileType.SpecialRequest))
				if (!isShortcut &&
					((FolderSecurityProfile.AllowUpdate && 
					(NodeProfileType == eProfileType.TaskListMainFavoritesFolder ||
					NodeProfileType == eProfileType.TaskListTaskListMainUserFolder ||
					NodeProfileType == eProfileType.TaskListSubFolder)) ||
					(FunctionSecurityProfile.AllowUpdate && 
					(NodeProfileType == eProfileType.TaskList ||
					NodeProfileType == eProfileType.Job ||
					NodeProfileType == eProfileType.SpecialRequest))))
				//End Track #6321 - JScott - User has ability to to create folders when security is view
				{
                    // Begin TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer
                    if (isSharedNode)
                    {
                        return false;
                    }
                    // End TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer
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

					if (Profile.ProfileType == eProfileType.TaskList)
					{
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((TaskListProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((TaskListProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
					else if (Profile.ProfileType == eProfileType.Job)
					{
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((JobProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((JobProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
					else if (Profile.ProfileType == eProfileType.SpecialRequest)
					{
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((SpecialRequestProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((SpecialRequestProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
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

					if (Profile.ProfileType == eProfileType.TaskList)
					{
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((TaskListProfile)aChangedNode.Profile).Name;
						InternalText = ((TaskListProfile)aChangedNode.Profile).Name;
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
					else if (Profile.ProfileType == eProfileType.Job)
					{
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((JobProfile)aChangedNode.Profile).Name;
						InternalText = ((JobProfile)aChangedNode.Profile).Name;
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
					else if (Profile.ProfileType == eProfileType.SpecialRequest)
					{
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((SpecialRequestProfile)aChangedNode.Profile).Name;
						InternalText = ((SpecialRequestProfile)aChangedNode.Profile).Name;
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
	}
}
