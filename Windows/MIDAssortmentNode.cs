using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class AssortmentTreeView : MIDTreeView
	{
		//=======
		// FIELDS
		//=======

		private int cFavoritesImage;
		private int cClosedFolderImage;
		private int cOpenFolderImage;
		private int cClosedShortcutFolderImage;
		private int cOpenShortcutFolderImage;
		private int cAssortmentSelectedImage;
		private int cAssortmentUnselectedImage;
		private int cAssortmentShortcutSelectedImage;
		private int cAssortmentShortcutUnselectedImage;
		//private int cSharedClosedFolderImage;
		//private int cSharedOpenFolderImage;
		//private int cSharedUserFilterSelectedImage;
		//private int cSharedUserFilterUnselectedImage;

		//private Hashtable _secLvlHash;

		private Header _dlAssortment;
		private FolderDataLayer _dlFolder;

		//private MIDAssortmentNode _userNode;
		private MIDAssortmentNode _globalNode;
		//private MIDFilterNode _sharedNode;

		private ExplorerAddressBlock _EAB;
		private Hashtable _assortmentSecGrpHash;
		// BEGIN Stodd - 4.0 to 4.1 Manual merge
        //private HeaderEnqueue _headerEnqueue = null;
		ApplicationSessionTransaction _enqueueTransaction = null;
		// END Stodd - 4.0 to 4.1 Manual merge

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentTreeView(ExplorerAddressBlock aEAB)
		{
			_EAB = aEAB;
		}

		//===========
		// PROPERTIES
		//===========

		internal MIDTreeNodeSecurityGroup UserAssortmentGroupSecGrp
		{
			get
			{
				return (MIDTreeNodeSecurityGroup)_assortmentSecGrpHash[SAB.ClientServerSession.UserRID];
			}
		}

        internal MIDTreeNodeSecurityGroup GlobalAssortmentGroupSecGrp
		{
			get
			{
				return (MIDTreeNodeSecurityGroup)_assortmentSecGrpHash[Include.GlobalUserRID];
			}
		}

		private FunctionSecurityProfile UserAssortmentGroupSecLvl
		{
			get
			{
				return UserAssortmentGroupSecGrp.FunctionSecurityProfile;
			}
		}

		private FunctionSecurityProfile GlobalAssortmentGroupSecLvl
		{
			get
			{
				return GlobalAssortmentGroupSecGrp.FunctionSecurityProfile;
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
				// BEGIN TT#1636 - stodd
				cAssortmentSelectedImage = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
				cAssortmentUnselectedImage = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
				cAssortmentShortcutSelectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.GlobalImage);
				cAssortmentShortcutUnselectedImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.GlobalImage);
				// END TT#1636 - stodd
				//cSharedClosedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.ClosedTreeFolder);
				//cSharedOpenFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.OpenTreeFolder);
				//cSharedUserFilterSelectedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);
				//cSharedUserFilterUnselectedImage = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);

				// Start Merge changes for 3.2 - stodd
				//_secLvlHash = new Hashtable();
				//_secLvlHash[SAB.ClientServerSession.UserRID] = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment);
				//_secLvlHash[Include.GlobalUserRID] = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment);

				_assortmentSecGrpHash = new Hashtable();

				//_storeGroupSecGrpHash[SAB.ClientServerSession.UserRID] = new MIDTreeNodeSecurityGroup(
				//    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser),
				//    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersStoreFoldersUser));

                // Begin TT#1998-MD - JSmith - Security - Assortment Permissions
                //_assortmentSecGrpHash[SAB.ClientServerSession.UserRID] = new MIDTreeNodeSecurityGroup(
                //    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment),
                //    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment));

                _assortmentSecGrpHash[SAB.ClientServerSession.UserRID] = new MIDTreeNodeSecurityGroup(
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentProperties),
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentExplorerFoldersUser));
                // End TT#1998-MD - JSmith - Security - Assortment Permissions

				//_storeGroupSecGrpHash[Include.GlobalUserRID] = new MIDTreeNodeSecurityGroup(
				//    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesGlobal),
				//    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersStoreFoldersGlobal));

                // Begin TT#1998-MD - JSmith - Security - Assortment Permissions
                //_assortmentSecGrpHash[Include.GlobalUserRID] = new MIDTreeNodeSecurityGroup(
                //   SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment),
                //   SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment));

                _assortmentSecGrpHash[Include.GlobalUserRID] = new MIDTreeNodeSecurityGroup(
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentProperties),
                    SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentExplorerFoldersGlobal));
                // End TT#1998-MD - JSmith - Security - Assortment Permissions

				// End Merge changes for 3.2 - stodd

				_dlAssortment = new Header();
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
			DataTable dtAssortments = null;
			DataTable dtFolderItems = null;
			DataTable dtFolderShortcuts = null;
			DataTable dtAssortmentShortcuts = null;
			MIDAssortmentNode newNode;
			FolderShortcut newShortcut;
			MIDAssortmentNode parentNode;
			MIDAssortmentNode childNode;
			DataTable dtUserFolders;
			FunctionSecurityProfile userSecLvl;
			MIDAssortmentNode userNode;

			try
			{
				Nodes.Clear();

				//----------------------
				// Build Faviorites node
				//----------------------

				//if (!UserSecLvl.AccessDenied)
				//{
					dtFolders = _dlFolder.Folder_Read(SAB.ClientServerSession.UserRID, eProfileType.AssortmentMainFavoritesFolder);

					if (dtFolders == null || dtFolders.Rows.Count == 0)
					{
						folderProf = new FolderProfile(Include.NoRID, SAB.ClientServerSession.UserRID, eProfileType.AssortmentMainFavoritesFolder, "My Favorites", SAB.ClientServerSession.UserRID);

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

					FavoritesNode = new MIDAssortmentNode(
						SAB,
						eTreeNodeType.MainFavoriteFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						FavoritesFolderSecGrp,
						cFavoritesImage,
						cFavoritesImage,
						cFavoritesImage,
						cFavoritesImage,
						folderProf.OwnerUserRID);

					FolderNodeHash[folderProf.Key] = FavoritesNode;

					Nodes.Add(FavoritesNode);
				//}

				//---------------------------
				// Build global node
				//---------------------------

					if (!GlobalAssortmentGroupSecLvl.AccessDenied)
				{
					dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.AssortmentMainFolder);

					if (dtFolders == null || dtFolders.Rows.Count == 0)
					{
						throw new Exception("Global Assortment Folder not defined");
					}
					//else if (dtFolders.Rows.Count == 0)
					//{
					//    throw new Exception("More than one Global Assortment Folder is defined");
					//}

					folderProf = new FolderProfile(dtFolders.Rows[0]);

					_globalNode = new MIDAssortmentNode(
						SAB,
						eTreeNodeType.MainSourceFolderNode,
						folderProf,
						folderProf.Name,
						Include.NoRID,
						folderProf.UserRID,
						UserAssortmentGroupSecGrp,
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

				if (!UserAssortmentGroupSecLvl.AccessDenied)
				{
					userRIDList.Add(SAB.ClientServerSession.UserRID);
				}

				if (!GlobalAssortmentGroupSecLvl.AccessDenied)
				{
					userRIDList.Add(Include.GlobalUserRID);
				}

				folderTypeList = new ArrayList();
				folderTypeList.Add((int)eProfileType.AssortmentMainFavoritesFolder);
				folderTypeList.Add((int)eProfileType.AssortmentMainFolder);
				folderTypeList.Add((int)eProfileType.AssortmentSubFolder);

				if (userRIDList.Count > 0)
				{
					//dtFilters = _dlAssortment.StoreFilter_ReadParent(userRIDList);
					dtAssortments = _dlAssortment.AssortmentProperties_ReadAll();
					dtFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.AssortmentSubFolder, true, false);
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.Assortment, true, false);
					//dtAssortmentShortcuts = _dlFolder.Folder_Shortcut_Item_Read(userRIDList, eProfileType.Assortment);
					dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.AssortmentHeader, true, false);
					dtAssortmentShortcuts = _dlFolder.Folder_Shortcut_Item_Read(userRIDList, eProfileType.AssortmentHeader);
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					dtFolderShortcuts = _dlFolder.Folder_Shortcut_Folder_Read(userRIDList, folderTypeList);

					dtAssortments.PrimaryKey = new DataColumn[] { dtAssortments.Columns["HDR_RID"] };
					dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

					if (!UserAssortmentGroupSecLvl.AccessDenied)
					{
						BuildFolderBranch(SAB.ClientServerSession.UserRID, FavoritesNode.Profile.Key, FavoritesNode, dtFolderItems, dtFolders, dtAssortments);
					}

					if (!GlobalAssortmentGroupSecLvl.AccessDenied)
					{
						BuildFolderBranch(Include.GlobalUserRID, _globalNode.Profile.Key, _globalNode, dtFolderItems, dtFolders, dtAssortments);  // Issue 3806
					}

					foreach (DataRow row in dtAssortmentShortcuts.Rows)
					{
						newShortcut = new FolderShortcut(row);

						parentNode = (MIDAssortmentNode)FolderNodeHash[newShortcut.ParentFolderId];
						childNode = (MIDAssortmentNode)ItemNodeHash[newShortcut.ShortcutId];

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

						parentNode = (MIDAssortmentNode)FolderNodeHash[newShortcut.ParentFolderId];
						childNode = (MIDAssortmentNode)FolderNodeHash[newShortcut.ShortcutId];

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

					if (_globalNode != null)
					{
						SortChildNodes(_globalNode);
					}

				}

				//--------------------------------
				// Read and Load Shared User Nodes
				//--------------------------------

				//userRIDList.Clear();
				//userRIDList.Add(SAB.ClientServerSession.UserRID);

				//dtUserFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.AssortmentMainUserFolder, false, true);

				//if (dtUserFolders.Rows.Count > 0)
				//{
				//    folderProf = new FolderProfile(
				//        Include.NoRID,
				//        SAB.ClientServerSession.UserRID,
				//        eProfileType.FilterMainSharedFolder,
				//        MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName),
				//        SAB.ClientServerSession.UserRID);

				//    userSecLvl = new FunctionSecurityProfile(-1);
				//    userSecLvl.SetAllowView();

				//    _sharedNode = new MIDFilterNode(
				//        SAB,
				//        eTreeNodeType.MainSourceFolderNode,
				//        folderProf,
				//        folderProf.Name,
				//        Include.NoRID,
				//        folderProf.UserRID,
				//        userSecLvl,
				//        cSharedClosedFolderImage,
				//        cSharedClosedFolderImage,
				//        cSharedOpenFolderImage,
				//        cSharedOpenFolderImage,
				//        folderProf.OwnerUserRID);

				//    Nodes.Add(_sharedNode);

				//    dtFilters = _dlFilters.StoreFilter_ReadParent(userRIDList);
				//    dtFolders = _dlFolder.Folder_Read(userRIDList, eProfileType.FilterSubFolder, false, true);
				//    dtFolderItems = _dlFolder.Folder_Item_Read(userRIDList, folderTypeList, eProfileType.StoreFilter, false, true);

				//    dtFilters.PrimaryKey = new DataColumn[] { dtFilters.Columns["STORE_FILTER_RID"] };
				//    dtFolders.PrimaryKey = new DataColumn[] { dtFolders.Columns["FOLDER_RID"] };

				//    foreach (DataRow row in dtUserFolders.Rows)
				//    {
				//        folderProf = new FolderProfile(row);

				//        folderProf.Name = DlSecurity.GetUserName(folderProf.OwnerUserRID);

				//        userSecLvl = (FunctionSecurityProfile)UserSecLvl.Clone();
				//        userSecLvl.SetDenyDelete();
				//        _secLvlHash[folderProf.OwnerUserRID] = userSecLvl;

				//        userNode = new MIDFilterNode(
				//            SAB,
				//            eTreeNodeType.MainSourceFolderNode,
				//            folderProf,
				//            folderProf.Name,
				//            Include.NoRID,
				//            folderProf.UserRID,
				//            userSecLvl,
				//            cClosedFolderImage,
				//            cClosedFolderImage,
				//            cOpenFolderImage,
				//            cOpenFolderImage,
				//            folderProf.OwnerUserRID);

				//        FolderNodeHash[folderProf.Key] = userNode;

				//        _sharedNode.Nodes.Add(userNode);

				//        BuildFolderBranch(SAB.ClientServerSession.UserRID, userNode.Profile.Key, userNode, dtFolderItems, dtFolders, dtFilters);
				//    }

				//    SortChildNodes(_sharedNode);
				//}
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

		//override protected void CreateNewItem(MIDTreeNode aParentNode)
		/// <returns>
		/// The new node that was created.  If node is returned, it will be placed in edit mode.
		/// If node is not available or edit mode is not desired, return null.
		/// </returns>

		override protected MIDTreeNode CreateNewItem(MIDTreeNode aParentNode)
		{
			frmAssortmentProperties assortmentProperties;
			OnAssortmentPropertiesCloseClass closeHandler;
			ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();
			AssortmentProfile ap = new AssortmentProfile(appTransaction, string.Empty, Include.NoRID, SAB.ApplicationServerSession);

			try
			{
				assortmentProperties = new frmAssortmentProperties(SAB, _EAB, aParentNode, ap, false);
				closeHandler = new OnAssortmentPropertiesCloseClass(appTransaction);	// TT#508 - md -stodd enqueue error
				assortmentProperties.OnAssortmentPropertiesChangeHandler += new frmAssortmentProperties.AssortmentPropertiesChangeEventHandler(OnAssortmentPropertiesChange);
				assortmentProperties.OnAssortmentPropertiesCloseHandler += new frmAssortmentProperties.AssortmentPropertiesCloseEventHandler(closeHandler.OnClose);
				assortmentProperties.MdiParent = MDIParentForm;

				assortmentProperties.Show();
				assortmentProperties.BringToFront();

				return null;
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
			MIDAssortmentNode newNode;

			try
			{
				newNodeName = FindNewFolderName("New Folder", aUserId, aNode.Profile.Key, eProfileType.AssortmentSubFolder);

				_dlFolder.OpenUpdateConnection();

				try
				{
					newFolderProf = new FolderProfile(Include.NoRID, aUserId, eProfileType.AssortmentSubFolder, newNodeName, aUserId);
					newFolderProf.Key = _dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
					_dlFolder.Folder_Item_Insert(aNode.Profile.Key, newFolderProf.Key, eProfileType.AssortmentSubFolder);

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
					newNode = new MIDAssortmentNode(
						SAB,
						eTreeNodeType.SubFolderNode,
						newFolderProf,
						newFolderProf.Name,
						aNode.Profile.Key,
						aUserId,
						aNode.NodeSecurityGroup,
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

        // BEGIN TT#2026-MD - AGallagher - Asst Explorer - Right Click>Select In Use and receive System Exception
        override protected void InUseNode(MIDTreeNode aNode)
        {
            try
            {
                if (aNode != null)
                {
                    //InUseAssortmentNode(aNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // END TT#2026-MD - AGallagher - Asst Explorer - Right Click>Select In Use and receive System Exception
		/// <summary>
		/// Virtual method that is called after a label has been updated
		/// </summary>
		/// <returns>
		/// A boolean indicating if post-processing was successful
		/// </returns>

		override protected bool AfterLabelUpdate(MIDTreeNode aNode, string aNewName)
		{
			int key;
			//AssortmentProfile AssortmentProf;
			AssortmentHeaderProfile ahp;
			FolderProfile folderProf;
			GenericEnqueue objEnqueue;
			// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
			List<GenericEnqueue> enqueueList = new List<GenericEnqueue>(); 
			// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

			try
			{
				switch (aNode.NodeProfileType)
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					case eProfileType.AssortmentHeader:
					//case eProfileType.Assortment:
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

						//objEnqueue = EnqueueObject((AssortmentProfile)aNode.Profile, false);
						objEnqueue = EnqueueObject((AssortmentHeaderProfile)aNode.Profile, false);
						if (objEnqueue == null)
						{
							return false;
						}
						// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
						enqueueList.Add(objEnqueue);
						// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

						try
						{
							key = _dlAssortment.HeaderAssortment_GetKey(aNewName);

							if (key != -1)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AssortmentNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							//AssortmentProf = (AssortmentProfile)aNode.Profile;
							//AssortmentProf.HeaderID = aNewName;
							ahp = (AssortmentHeaderProfile)aNode.Profile;
							ahp.HeaderID = aNewName;

							// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
							// Enqueue placeholders
							DataTable dtPlaceholders = _dlAssortment.GetPlaceholdersForAssortment(aNode.Profile.Key);
							foreach (DataRow row in dtPlaceholders.Rows)
							{
								int phKey = int.Parse(row["HDR_RID"].ToString());
								AllocationHeaderProfile apPlaceholder = (AllocationHeaderProfile)SAB.HeaderServerSession.GetHeaderData(phKey, false, false, true);
								AssortmentHeaderProfile asrtPlaceholder = new AssortmentHeaderProfile(apPlaceholder.HeaderID, apPlaceholder.Key);
								GenericEnqueue objEnqueuePh = EnqueueObject(asrtPlaceholder, false);
								if (objEnqueue == null)
								{
									return false;
								}
								enqueueList.Add(objEnqueuePh);
							}
							// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
							
							_dlAssortment.OpenUpdateConnection();

							try
							{
								_dlAssortment.AssortmentHeader_Update(aNode.Profile.Key, aNewName);

								// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
								foreach (DataRow row in dtPlaceholders.Rows)
								{
									int phKey = int.Parse(row["HDR_RID"].ToString());
									string oldId = row["HDR_ID"].ToString();
									int index = oldId.IndexOf("PhStyle");
									string phNewName = aNewName + " " +oldId.Substring(index);
									_dlAssortment.AssortmentHeader_Update(phKey, phNewName);
								}
								// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

								_dlAssortment.CommitData();
							}
							catch (Exception exc)
							{
								string message = exc.ToString();
								throw;
							}
							finally
							{
								_dlAssortment.CloseUpdateConnection();
                                UpdateAssortmentWorkpsace(aNode.Profile.Key);
							}
						}
						catch (Exception error)
						{
							string message = error.ToString();
							throw;
						}
						finally
						{
							// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
							foreach (GenericEnqueue ge in enqueueList)
							{
								ge.DequeueGeneric();
								//objEnqueue.DequeueGeneric();
							}
							// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
						}

						break;

					case eProfileType.AssortmentMainFavoritesFolder:
					case eProfileType.AssortmentMainFolder:
					case eProfileType.AssortmentSubFolder:

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
			MIDAssortmentNode toNode;
			MIDAssortmentNode newNode = null;

			try
			{
				BeginUpdate();

				try
				{
					switch (aToNode.NodeProfileType)
					{
						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						case eProfileType.AssortmentHeader:
						//case eProfileType.Assortment:
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

							toNode = (MIDAssortmentNode)aToNode.Parent;
							break;

						default:

							toNode = (MIDAssortmentNode)aToNode;
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
						case eProfileType.AssortmentSubFolder:

							newNode = BuildRootShortcutNode((MIDAssortmentNode)aFromNode, toNode);

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

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						case eProfileType.AssortmentHeader:
						//case eProfileType.Assortment:
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

							newNode = BuildObjectShortcutNode((MIDAssortmentNode)aFromNode, toNode);

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
			MIDAssortmentNode toNode;

			try
			{
				switch (aToNode.NodeProfileType)
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					case eProfileType.AssortmentHeader:
					//case eProfileType.Assortment:
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

						toNode = (MIDAssortmentNode)aToNode.Parent;
						break;

					default:

						toNode = (MIDAssortmentNode)aToNode;
						break;
				}

				try
				{
					return MoveAssortmentNode((MIDAssortmentNode)aFromNode, toNode);
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
			MIDAssortmentNode toNode;

			try
			{
				switch (aToNode.NodeProfileType)
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					case eProfileType.AssortmentHeader:
					//case eProfileType.Assortment:
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

						toNode = (MIDAssortmentNode)aToNode.Parent;
						break;

					default:

						toNode = (MIDAssortmentNode)aToNode;
						break;
				}

				try
				{
					return CopyAssortmentNode((MIDAssortmentNode)aFromNode, toNode, aFindUniqueName);
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
					DeleteAssortmentNode((MIDAssortmentNode)aNode);
                    _EAB.AllocationWorkspaceExplorer.IRefresh();
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
			MIDAssortmentNode node;
			AssortmentProfile assortmentProf;
			AssortmentHeaderProfile ahp;
			frmAssortmentProperties assortmentProperties;
			OnAssortmentPropertiesCloseClass closeHandler;
			// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
			//GenericEnqueue objEnqueue = null;
			// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue

			try
			{
				if (aNode != null)
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//if (aNode.Profile.ProfileType == eProfileType.Assortment)
					if (aNode.Profile.ProfileType == eProfileType.AssortmentHeader)
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					{
						node = (MIDAssortmentNode)aNode;

						//assortmentProf = (AssortmentProfile)node.Profile;
						ahp = (AssortmentHeaderProfile)node.Profile;
						ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();

						//BEGIN TT#562-MD - stodd -  Moved this code up so reread wasn't neccessary
						string aHdrConflictMsg = string.Empty;
						List<int> asrtList = new List<int>();
						asrtList.Add(ahp.Key);
						bool isEnqueued = appTransaction.EnqueueHeaders(asrtList, out aHdrConflictMsg);
						bool readOnly = false;
						if (!isEnqueued)
						{
							readOnly = true;
						}
						// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
						assortmentProf = new AssortmentProfile(appTransaction, ahp.HeaderID, ahp.Key, SAB.ApplicationServerSession, false, false);
						// END TT#773-MD - Stodd - replace hashtable with dictionary
						//assortmentProf.AppSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();

						// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
						//string aHdrConflictMsg = string.Empty;
						//List<int> asrtList = new List<int>();
						//asrtList.Add(ahp.Key);
						//bool isEnqueued = appTransaction.EnqueueHeaders(asrtList, out aHdrConflictMsg);
						//bool readOnly = false;
						//if (!isEnqueued)
						//{
						//    readOnly = true;
						//}
						//objEnqueue = EnqueueObject(ahp, true);
						
						//if (objEnqueue != null)
						//{
						//assortmentProf.ReReadHeader();
						//END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders

						assortmentProperties = new frmAssortmentProperties(SAB, _EAB, (MIDTreeNode)node.Parent, assortmentProf, readOnly);
						//assortmentProperties = new frmAssortmentProperties(SAB, _EAB, (MIDTreeNode)node.Parent, assortmentProf, objEnqueue.IsInConflict);
						closeHandler = new OnAssortmentPropertiesCloseClass(appTransaction);
						assortmentProperties.OnAssortmentPropertiesChangeHandler += new frmAssortmentProperties.AssortmentPropertiesChangeEventHandler(OnAssortmentPropertiesChange);
						assortmentProperties.OnAssortmentPropertiesCloseHandler += new frmAssortmentProperties.AssortmentPropertiesCloseEventHandler(closeHandler.OnClose);

						assortmentProperties.MdiParent = MDIParentForm;

						assortmentProperties.Show();
						assortmentProperties.BringToFront();

						//}
						// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			//finally
			//{
			//    if (objEnqueue != null)
			//    {
			//        objEnqueue.DequeueGeneric();
			//    }
			//}
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
			// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
			//return aClipboardDataType == eProfileType.Assortment;
			return aClipboardDataType == eProfileType.AssortmentHeader;
			// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
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
				foreach (MIDAssortmentNode node in aStartNode.Nodes)
				{
					if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
					{
						node.RefreshShortcutNode(aChangedNode);

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						//if (node.Profile.ProfileType != eProfileType.Assortment)
						if (node.Profile.ProfileType != eProfileType.AssortmentHeader)
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
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
					else if (node.NodeProfileType == eProfileType.AssortmentSubFolder || node.isFolderShortcut)
					{
						RefreshShortcuts(node, aChangedNode);
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
				// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				//if (aSelectedNodes.ClipboardDataType == eProfileType.Assortment ||
				//	aSelectedNodes.ClipboardDataType == eProfileType.AssortmentSubFolder)
				if (aSelectedNodes.ClipboardDataType == eProfileType.AssortmentHeader ||
					aSelectedNodes.ClipboardDataType == eProfileType.AssortmentSubFolder)
				// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
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

		//--------------
		//PUBLIC METHODS
		//--------------

		// Begin TT#1227 - stodd
		public void InitialExpand()
		{
			try
			{
				if (FavoritesNode != null)
				{
					FavoritesNode.Expand();
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
		// End TT#1227 - stodd

		public void CreateShortcutChildren(MIDTreeNode aFromNode, MIDTreeNode aToNode)
		{
			MIDAssortmentNode newNode = null;

			try
			{
				foreach (MIDAssortmentNode node in aFromNode.Nodes)
				{
					switch (node.NodeProfileType)
					{
						case eProfileType.AssortmentSubFolder:

							newNode = new MIDAssortmentNode(
								SAB,
								eTreeNodeType.ChildFolderShortcutNode,
								node.Profile,
								node.Text,
								node.ParentId,
								node.UserId,
								node.NodeSecurityGroup,
								cClosedFolderImage,
								cClosedFolderImage,
								cOpenFolderImage,
								cOpenFolderImage,
								node.OwnerUserRID);

							break;

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						case eProfileType.AssortmentHeader:
						//case eProfileType.Assortment:
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

							newNode = new MIDAssortmentNode(
								SAB,
								eTreeNodeType.ChildObjectShortcutNode,
								node.Profile,
								node.Text,
								node.ParentId,
								node.UserId,
								node.NodeSecurityGroup,
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

				foreach (MIDAssortmentNode node in deleteList)
				{
					if (node.Profile.Key == aDeleteNode.Profile.Key && node.Profile.ProfileType == aDeleteNode.Profile.ProfileType &&
						node.isShortcut)
					{
						DeleteChildNodes(node);
						node.Remove();
					}
					else if (node.NodeProfileType == eProfileType.AssortmentSubFolder ||
						node.isFolderShortcut ||
						(node.isChildShortcut && node.Profile.ProfileType == eProfileType.AssortmentSubFolder))
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

		public string FindNewAssortmentName(string aAssortmentName, int aUserRID)
		{
			int index;
			string newName;
			int key;

			try
			{
				index = 0;
				newName = aAssortmentName;
				key = _dlAssortment.HeaderAssortment_GetKey(newName);

				while (key != -1)
				{
					index++;

					if (index > 1)
					{
						newName = "Copy (" + index + ") of " + aAssortmentName;
					}
					else
					{
						newName = "Copy of " + aAssortmentName;
					}

					key = _dlAssortment.HeaderAssortment_GetKey(newName);
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

		private void BuildFolderBranch(int aUserRID, int aParentFolderRID, MIDTreeNode aParentNode, DataTable aFolderItems, DataTable aFolders, DataTable aAssortments)
		{
			DataRow[] folderItemList;
			DataRow itemRow;
			FolderProfile folderProf;
			//AssortmentProfile assortmentProf;
			AssortmentHeaderProfile ahp;
			MIDAssortmentNode newNode = null;

			try
			{
				folderItemList = aFolderItems.Select("USER_RID = " + aUserRID + " AND PARENT_FOLDER_RID = " + aParentFolderRID);

				foreach (DataRow row in folderItemList)
				{
					switch ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"]))
					{
						case eProfileType.AssortmentSubFolder:

							itemRow = aFolders.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Assortment", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
							folderProf = new FolderProfile(itemRow);
							newNode = BuildSubFolderNode(folderProf, aParentNode);
							BuildFolderBranch(aUserRID, newNode.Profile.Key, newNode, aFolderItems, aFolders, aAssortments);
							break;

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						case eProfileType.AssortmentHeader:
						//case eProfileType.Assortment:
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

							itemRow = aAssortments.Rows.Find(Convert.ToInt32(row["CHILD_ITEM_RID"]));
                            // Begin TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
                            if (itemRow == null)
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_ExplorerFolderBuildError, "Assortment", ((eProfileType)Convert.ToInt32(row["CHILD_ITEM_TYPE"])).ToString(), Convert.ToInt32(row["CHILD_ITEM_RID"]).ToString()), this.GetType().Name);
                                continue;
                            }
                            // End TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.)
							try
							{
								//ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();
								//assortmentProf = new AssortmentProfile(appTransaction, itemRow, SAB.ApplicationServerSession, false, false);	// TT#1183 - stodd - assortment
								AllocationHeaderProfile allp = (AllocationHeaderProfile)SAB.HeaderServerSession.GetHeaderData(Convert.ToInt32(row["CHILD_ITEM_RID"]), false, false, true);
								ahp = new AssortmentHeaderProfile(allp.HeaderID, allp.Key);
								//BuildAssortmentNode(assortmentProf, aParentNode, aUserRID);
								BuildAssortmentNode(ahp, aParentNode, aUserRID);
							}
							catch (Exception exc)
							{
								string assortmentId = "(Assortment = " + itemRow["HDR_ID"].ToString() + ") ";
								SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, assortmentId + exc.ToString(), this.ToString());

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

		private MIDAssortmentNode BuildSubFolderNode(FolderProfile aFolderProf, MIDTreeNode aParentNode)
		{
			MIDAssortmentNode newNode;

			try
			{
				newNode = new MIDAssortmentNode(
					SAB,
					eTreeNodeType.SubFolderNode,
					aFolderProf,
					aFolderProf.Name,
					aParentNode.NodeRID,
					aFolderProf.UserRID,
                    //FavoritesSecGrp,
                    (aFolderProf.UserRID == Include.GlobalUserRID ? GlobalAssortmentGroupSecGrp : FavoritesSecGrp),
					cClosedFolderImage,
					cClosedFolderImage,
					cOpenFolderImage,
					cOpenFolderImage,
					aFolderProf.OwnerUserRID);

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

		//public MIDAssortmentNode BuildAssortmentNode(AssortmentProfile aProfile, MIDTreeNode aParentNode, int aUserId)
		public MIDAssortmentNode BuildAssortmentNode(AssortmentHeaderProfile aProfile, MIDTreeNode aParentNode, int aUserId)
		{
			MIDAssortmentNode newNode;

			try
			{
				newNode = new MIDAssortmentNode(
					SAB,
					eTreeNodeType.ObjectNode,
					aProfile,
					aProfile.HeaderID,
					aParentNode.Profile.Key,
					aUserId,
					aParentNode.NodeSecurityGroup,
					cAssortmentUnselectedImage,
					cAssortmentSelectedImage,
					Include.GlobalUserRID);
				
				ItemNodeHash[aProfile.Key] = newNode;
				aParentNode.Nodes.Add(newNode);

				return newNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MIDAssortmentNode BuildRootShortcutNode(MIDAssortmentNode aFromNode, MIDAssortmentNode aToNode)
		{
			MIDAssortmentNode newNode;

			try
			{
                MIDTreeNodeSecurityGroup securityGroup;  // TT#2014-MD - JSmith - Assortment Security

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

				newNode = new MIDAssortmentNode(
					SAB,
					eTreeNodeType.FolderShortcutNode,
					aFromNode.Profile,
					aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
					aToNode.Profile.Key,
					aFromNode.UserId,
                    // Begin TT#2014-MD - JSmith - Assortment Security
                    //aFromNode.NodeSecurityGroup,
                    securityGroup,
                    // End TT#2014-MD - JSmith - Assortment Security
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

		private MIDAssortmentNode BuildObjectShortcutNode(MIDAssortmentNode aFromNode, MIDAssortmentNode aToNode)
		{
			MIDAssortmentNode newNode;
            MIDTreeNodeSecurityGroup securityGroup;  // TT#2014-MD - JSmith - Assortment Security 

			try
			{
				//if (aFromNode.UserId == Include.GlobalUserRID)
				//{
				//    newNode = new MIDAssortmentNode(
				//        SAB,
				//        eTreeNodeType.ObjectShortcutNode,
				//        aFromNode.Profile,
				//        aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
				//        aToNode.Profile.Key,
				//        aFromNode.UserId,
				//        aFromNode.FunctionSecurityProfile,
				//        cGlobalFilterShortcutUnselectedImage,
				//        cGlobalFilterShortcutSelectedImage,
				//        aFromNode.UserId);
				//}
				//else
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

					newNode = new MIDAssortmentNode(
						SAB,
						eTreeNodeType.ObjectShortcutNode,
						aFromNode.Profile,
						aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
						aToNode.Profile.Key,
						aFromNode.UserId,
                        // Begin TT#2014-MD - JSmith - Assortment Security
                        //aFromNode.NodeSecurityGroup,
                        securityGroup,
                        // End TT#2014-MD - JSmith - Assortment Security
						cAssortmentShortcutUnselectedImage,
						cAssortmentShortcutSelectedImage,
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

		private GenericEnqueue EnqueueObject(AssortmentHeaderProfile ahp, bool aAllowReadOnly)
		//private GenericEnqueue EnqueueObject(AssortmentProfile aAssortmentProf, bool aAllowReadOnly)
		{
			GenericEnqueue objEnqueue;
			string errMsg;

			try
			{
				//objEnqueue = new GenericEnqueue(eLockType.Assortment, aAssortmentProf.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
				objEnqueue = new GenericEnqueue(eLockType.Assortment, ahp.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

				try
				{
					objEnqueue.EnqueueGeneric();
				}
				catch (GenericConflictException)
				{
					//errMsg = "The Assortment \"" + aAssortmentProf.HeaderID + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
					errMsg = "The Assortment \"" + ahp.HeaderID + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";

					if (aAllowReadOnly)
					{
						errMsg += System.Environment.NewLine + System.Environment.NewLine;
						errMsg += "Do you wish to continue with the Assortment as read-only?";

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

		private MIDAssortmentNode MoveAssortmentNode(MIDAssortmentNode aFromNode, MIDAssortmentNode aToNode)
		{
			MIDAssortmentNode newNode = null;
			FolderProfile folderProf;
			//AssortmentProfile AssortmentProf;
			AssortmentHeaderProfile ahp;
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
				else if (aFromNode.NodeProfileType == eProfileType.AssortmentSubFolder)
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

						folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.AssortmentSubFolder);
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

					foreach (MIDAssortmentNode node in moveArray)
					{
						MoveAssortmentNode(node, newNode);
					}

					aFromNode.Remove();

					return newNode;
				}
				// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				//else if (aFromNode.NodeProfileType == eProfileType.Assortment)
				else if (aFromNode.NodeProfileType == eProfileType.AssortmentHeader)
				// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//AssortmentProf = (AssortmentProfile)aFromNode.Profile;
					ahp = (AssortmentHeaderProfile)aFromNode.Profile;
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

					if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
					{
						DlSecurity.OpenUpdateConnection();

						try
						{
							// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
							//DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(AssortmentProf.ProfileType), AssortmentProf.Key);
							//DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(AssortmentProf.ProfileType), AssortmentProf.Key, aToNode.UserId);
							DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(ahp.ProfileType), ahp.Key);
							DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(ahp.ProfileType), ahp.Key, aToNode.UserId);
							// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
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
						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						//AssortmentProf.HeaderID = FindNewAssortmentName(AssortmentProf.HeaderID, aToNode.UserId);
						//AssortmentProf.AssortmentUserRid = aToNode.UserId;
						ahp.HeaderID = FindNewAssortmentName(ahp.HeaderID, aToNode.UserId);
						ahp.AsrtUserRid = aToNode.UserId;
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					}

					_dlAssortment.OpenUpdateConnection();
					_dlFolder.OpenUpdateConnection();

					try
					{
						//_dlAssortment.AssortmentDetailHeaders_Update(AssortmentProf);
						_dlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
						_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

						_dlFolder.CommitData();
						_dlAssortment.CommitData();
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_dlFolder.CloseUpdateConnection();
						_dlAssortment.CloseUpdateConnection();
					}

					aFromNode.Remove();
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//newNode = BuildAssortmentNode(AssortmentProf, aToNode, AssortmentProf.AssortmentUserRid);
					newNode = BuildAssortmentNode(ahp, aToNode, ahp.AsrtUserRid);
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
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

		private MIDAssortmentNode CopyAssortmentNode(MIDAssortmentNode aFromNode, MIDAssortmentNode aToNode, bool aFindUniqueName)
		{
			//DataTable dtAssortmentObjects;
			FolderProfile folderProf;
			AssortmentProfile assortmentProf;
			// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
			AssortmentHeaderProfile ahp;
			// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
			MIDAssortmentNode newNode = null;

			try
			{
			    if (aFromNode.isSubFolder)
			    {
			        folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
			        folderProf.UserRID = aToNode.UserId;
			        folderProf.OwnerUserRID = aToNode.UserId;

			        if (aFindUniqueName)
			        {
			            folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, aToNode.Profile.Key, eProfileType.AssortmentSubFolder);
			        }

			        _dlFolder.OpenUpdateConnection();

			        try
			        {
			            folderProf.Key = _dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, eProfileType.AssortmentSubFolder);
			            _dlFolder.Folder_Item_Insert(aToNode.Profile.Key, folderProf.Key, eProfileType.AssortmentSubFolder);

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

			        foreach (MIDAssortmentNode node in aFromNode.Nodes)
			        {
			            CopyAssortmentNode(node, newNode, aFindUniqueName);
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
					try
					{
                        //HierarchyMaintenance hierMaint = new HierarchyMaintenance(SAB); // TT#1599 - comment out; not used
						//============================
						// Copy Assortment header
						//============================
						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						ApplicationSessionTransaction aTrans =  SAB.ApplicationServerSession.CreateTransaction();
						ahp = (AssortmentHeaderProfile)aFromNode.Profile;
						AssortmentProfile origAsrtProfile = new AssortmentProfile(aTrans, ahp.HeaderID, ahp.Key, SAB.ApplicationServerSession);
						assortmentProf = (AssortmentProfile)(origAsrtProfile).Clone();
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						
						assortmentProf.Key = Include.NoRID;
						assortmentProf.AssortmentUserRid = Include.GlobalUserRID;
						DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRangeClone(assortmentProf.AssortmentCalendarDateRangeRid);
						assortmentProf.AssortmentCalendarDateRangeRid = drp.Key;
                        assortmentProf.AsrtRID = Include.NoRID;     // TT#902 - MD - stodd - assortment copy fails
                        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                        // Begin TT#2114-MD - JSmith - Calendar Error Copying Assortment
                        if (assortmentProf.AssortmentBeginDayCalendarDateRangeRid != Include.UndefinedCalendarDateRange)
                        {
                        // End TT#2114-MD - JSmith - Calendar Error Copying Assortment
                            DateRangeProfile drpBeginDay = SAB.ClientServerSession.Calendar.GetDateRangeClone(assortmentProf.AssortmentBeginDayCalendarDateRangeRid);
                            assortmentProf.AssortmentBeginDayCalendarDateRangeRid = drpBeginDay.Key;
                        // Begin TT#2114-MD - JSmith - Calendar Error Copying Assortment
                        }
                        // End TT#2114-MD - JSmith - Calendar Error Copying Assortment
						// End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                        
                        // Begin TT#1599 - RMatelic - Removing a Placeholder (style) from the Contents tab, does not remove it from the Merch Explorer.
                        // Deleting an Assortment also does not delete the Placeholders from the Merch Explorer.
                        // It was determined as part of this Test Track that an Assortment Node is not necessary. 
						//HierarchyNodeProfile assortHnp = hierMaint.GetAssortmentNode();
                        //assortmentProf.StyleHnRID = assortHnp.Key;
                        // End TT#1599
						if (aFindUniqueName)
						{
							assortmentProf.HeaderID = FindNewAssortmentName(assortmentProf.HeaderID, aToNode.UserId);
						}

						_dlAssortment.OpenUpdateConnection();
						_dlFolder.OpenUpdateConnection();
						assortmentProf.AppSessionTransaction.NewAllocationMasterProfileList();
						assortmentProf.WriteHeader();
						assortmentProf.BuildAssortmentSummary();
						assortmentProf.AssortmentSummaryProfile.Key = assortmentProf.Key;
						

						//=====================================
						// Copy Attached Placeholder headers
						//=====================================
						Hashtable placeholderHash = new Hashtable();
						DataTable dtPlaceholders = _dlAssortment.GetPlaceholdersForAssortment(aFromNode.Profile.Key);
						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						HierarchyNodeList hierarchyNodeList = SAB.HierarchyServerSession.GetPlaceholderStyles(assortmentProf.AssortmentAnchorNodeRid,
								dtPlaceholders.Rows.Count, 0, assortmentProf.Key);
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

						for (int i = 0; i<dtPlaceholders.Rows.Count;i++)
						{
							DataRow phRow = dtPlaceholders.Rows[i];
							int phRid = int.Parse(phRow["HDR_RID"].ToString());
							string phId = phRow["HDR_ID"].ToString();
							// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
							ApplicationSessionTransaction aPHTrans = SAB.ApplicationServerSession.CreateTransaction();
                            aPHTrans.AddAssortmentMemberProfile(assortmentProf);    // TT#1260-MD - stodd - ‘Copy’ action from Assortment explorer gets “Object Reference” error.
							//AllocationProfile phOrigProfile = new AllocationProfile(SAB.ApplicationServerSession.CreateTransaction(), phId, phRid, SAB.ApplicationServerSession);
							AllocationProfile phOrigProfile = new AllocationProfile(aPHTrans, phId, phRid, SAB.ApplicationServerSession);
							// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
							AllocationProfile phProfile = (AllocationProfile)(phOrigProfile).Clone();

							phProfile.Key = Include.NoRID;
							phProfile.AsrtRID = assortmentProf.Key;
							// BEGIN TT#419-MD - stodd - assortment copy/paste
							phProfile.AppSessionTransaction.NewAllocationMasterProfileList();
							int[] selectedHeaderArray = new int[1];
							selectedHeaderArray[0] = phProfile.Key;
							//BEGIN TT#636 - MD - DOConnell - Copy & Paste an Assortment.  The Matrix values for the placeholders goes to 0.  The Characteristic tab does not show the colors.
                            phProfile.AppSessionTransaction.AddAllocationProfile(phProfile);
							//phProfile.AppSessionTransaction.LoadHeaders(selectedHeaderArray);
							//END TT#636 - MD - DOConnell - Copy & Paste an Assortment.  The Matrix values for the placeholders goes to 0.  The Characteristic tab does not show the colors.
							phProfile.HeaderID = FindNewAssortmentName(phProfile.HeaderID, aToNode.UserId);
							phProfile.WriteHeader();
							
							//BEGIN TT#620 - MD - DOConnell - Tried to copy &  paste an assortment and receive a database error referencing syntax Invalid Cast Exception.
                            //phProfile.TotalUnitsToAllocate = 0;
                            //END TT#620 - MD - DOConnell - Tried to copy &  paste an assortment and receive a database error referencing syntax Invalid Cast Exception.
							// Set Color units to allocate (qty) to 0
                            //BEGIN TT#636 - MD - DOConnell - Copy & Paste an Assortment.  The Matrix values for the placeholders goes to 0.  The Characteristic tab does not show the colors.
							//Uncommented code below
							if (phProfile.BulkColors != null)
                            {
                                foreach (HdrColorBin hcb in phProfile.BulkColors.Values)
                                {
                                    phProfile.SetColorUnitsToAllocate(hcb.ColorCodeRID, 0);
                                    if (hcb.ColorSizes != null)
                                    {
                                        foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                                        {
                                            phProfile.SetSizeUnitsToAllocate(hcb.ColorCodeRID, hsb.SizeCodeRID, 0);
                                        }
                                    }
                                }
                            }


                            ProfileList stores = StoreMgmt.StoreProfiles_GetActiveStoresList(); //SAB.StoreServerSession.GetActiveStoresList();
                            phProfile.SetAllocatedUnits(stores, 0);
							//END TT#636 - MD - DOConnell - Copy & Paste an Assortment.  The Matrix values for the placeholders goes to 0.  The Characteristic tab does not show the colors.

							// Remove Packs
							if (phProfile.Packs != null)
							{
                                // Begin TT#2048-MD - JSmith - Copy Paste an Asst in the Asst Explorer and receive a Collection was modified:enumeration operation may not execute message.
                                //foreach (PackHdr pack in phProfile.Packs.Values)
                                //{
                                //    phProfile.RemovePack(pack.PackName);
                                //}
                                List<string> packsToRemove = new List<string>();
                                foreach (PackHdr pack in phProfile.Packs.Values)
                                {
                                    packsToRemove.Add(pack.PackName);
                                }
                                foreach (string packName in packsToRemove)
                                {
                                    phProfile.RemovePack(packName);
                                }
                                // End TT#2048-MD - JSmith - Copy Paste an Asst in the Asst Explorer and receive a Collection was modified:enumeration operation may not execute message.
							}

							//=========================================
							// Cancel any allocation on Cloned Header
							//=========================================
							GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
							bool aReviewFlag = false;
							bool aUseSystemTolerancePercent = false;
							double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
							int aStoreFilter = Include.AllStoreFilterRID;
							int aWorkFlowStepKey = -1;
                            phProfile.StyleHnRID = ((HierarchyNodeProfile)hierarchyNodeList[i]).Key; //TT#620 - MD - DOConnell - Tried to copy &  paste an assortment and receive a database error referencing syntax Invalid Cast Exception.
							ApplicationBaseAction aMethod = phProfile.AppSessionTransaction.CreateNewMethodAction(eMethodType.BackoutAllocation);
							AllocationWorkFlowStep aAllocationWorkFlowStep
							        = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
							phProfile.AppSessionTransaction.DoAllocationAction(aAllocationWorkFlowStep);
							// END TT#419-MD - stodd - assortment copy/paste
							//BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
							//phProfile.Action(eAllocationMethodType.BackoutAllocation, new GeneralComponent(eGeneralComponentType.Total), 0, Include.UndefinedStoreFilter, true);
                            //phProfile.StyleHnRID = ((HierarchyNodeProfile)hierarchyNodeList[i]).Key; //TT#620 - MD - DOConnell - Tried to copy &  paste an assortment and receive a database error referencing syntax Invalid Cast Exception.
							phProfile.WriteHeader();
							aPHTrans.Dispose();
							// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
							placeholderHash.Add(phRid, phProfile.Key);
						}

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						//=========================================
						// Build Summary data for copied assortment
						//=========================================
						List<int> hierNodeList = new List<int>();
						List<int> versionList = new List<int>();
						List<int> dateRangeList = new List<int>();
						List<double> weightList = new List<double>();
						foreach (AssortmentBasis ab in assortmentProf.AssortmentBasisList)
						{
							hierNodeList.Add(ab.HierarchyNodeProfile.Key);
							versionList.Add(ab.VersionProfile.Key);
							dateRangeList.Add(ab.HorizonDate.Key);
							weightList.Add(ab.Weight);
						}
						assortmentProf.AssortmentSummaryProfile.ClearAssortmentSummaryTable(); 
						assortmentProf.AssortmentSummaryProfile.Process(aTrans, assortmentProf.AssortmentAnchorNodeRid, assortmentProf.AssortmentVariableType, hierNodeList,
							   versionList, dateRangeList, weightList, assortmentProf.AssortmentIncludeSimilarStores, assortmentProf.AssortmentIncludeIntransit,
							   assortmentProf.AssortmentIncludeOnhand, assortmentProf.AssortmentIncludeCommitted, assortmentProf.AssortmentAverageBy, true, true);
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						
						//===============================
						// Copy Assortment Properties
						//===============================
						assortmentProf.HeaderDataRecord = _dlAssortment;
						assortmentProf.WriteAssortment();
						assortmentProf.AssortmentSummaryProfile.WriteAssortmentStoreEligibility();
						assortmentProf.AssortmentSummaryProfile.WriteAssortmentStoreSummary(assortmentProf.HeaderDataRecord);
						CopyAssortmentStyleClosedList(placeholderHash);

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						_dlFolder.Folder_Item_Insert(aToNode.Profile.Key, assortmentProf.Key, eProfileType.AssortmentHeader);
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

						_dlAssortment.CommitData();
			            _dlFolder.CommitData();

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						aTrans.Dispose();
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

                        UpdateAssortmentWorkpsace(assortmentProf.Key);
			        }
			        catch (Exception exc)
			        {
			            string message = exc.ToString();
			            throw;
			        }
			        finally
			        {
						_dlAssortment.CloseUpdateConnection();
			            _dlFolder.CloseUpdateConnection();
			        }

					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					AllocationHeaderProfile allp = (AllocationHeaderProfile)SAB.HeaderServerSession.GetHeaderData(assortmentProf.Key, false, false, true);
					ahp = new AssortmentHeaderProfile(allp.HeaderID, allp.Key);
					BuildAssortmentNode(ahp, aToNode, assortmentProf.AssortmentUserRid);
					//newNode = BuildAssortmentNode(assortmentProf, aToNode, assortmentProf.AssortmentUserRid);
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

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

		private void CopyAssortmentStyleClosedList(Hashtable placeholderHash)
		{

			AssortmentDetailData assortDetailData = new AssortmentDetailData();
			try
			{
				if (!assortDetailData.ConnectionIsOpen)
					assortDetailData.OpenUpdateConnection();

				assortDetailData.AssortmentStyleClosed_Copy(placeholderHash);
				assortDetailData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				if (assortDetailData.ConnectionIsOpen)
				{
					assortDetailData.CloseUpdateConnection();
				}
			}
		}

		private bool DeleteAssortmentNode(MIDAssortmentNode aNode)
		{
			GenericEnqueue objEnqueue;
			object[] deleteArray;

			try
			{
				if (aNode.isObject)
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//objEnqueue = EnqueueObject((AssortmentProfile)aNode.Profile, false);
					objEnqueue = EnqueueObject((AssortmentHeaderProfile)aNode.Profile, false);
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

					if (objEnqueue == null)
					{
						return false;
					}
                    if (!HeadersEnqueued(aNode))
                    {
                        objEnqueue.DequeueGeneric();
                        return false;
                    }
 
					try
					{
                        ArrayList styleAL = new ArrayList();
                        DataTable dtAssortment = _dlAssortment.GetHeader(aNode.Profile.Key);
                        foreach (DataRow row in dtAssortment.Rows)
                        {
                            styleAL.Add(Convert.ToInt32(row["STYLE_HNRID"], CultureInfo.CurrentUICulture));
                        }
                        DataTable dtPlaceholders = _dlAssortment.GetPlaceholdersForAssortment(aNode.Profile.Key);
                        foreach (DataRow row in dtPlaceholders.Rows)
                        {
                            styleAL.Add(Convert.ToInt32(row["STYLE_HNRID"], CultureInfo.CurrentUICulture));
                        }
						_dlAssortment.OpenUpdateConnection();
						_dlFolder.OpenUpdateConnection();

						try
						{
							// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
							_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.AssortmentHeader);
							_dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.AssortmentHeader);
							//_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.Assortment);
							//_dlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, eProfileType.Assortment);
							// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

                            // Begin TT#1320 - stodd - Cannot Delete Assortment from Assortment Explorer - 
                            //_dlAssortment.DeleteEntireAssortment(aNode.Profile.Key, dtPlaceholders);
                            _dlAssortment.DeleteAssortment(aNode.Profile.Key);
                            // End TT#1320 - stodd - Cannot Delete Assortment from Assortment Explorer - 

							_dlAssortment.CommitData();
							_dlFolder.CommitData();

                            if (styleAL.Count > 0)
                            {
                                foreach (int styleRID in styleAL)
                                {
                                    DeletePlaceholderStyle(styleRID);
                                }
                            }
                            //_EAB.AssortmentWorkspaceExplorer.IRefresh();
                            int[] hdrRIDs = new int[1];
                            hdrRIDs[0] = aNode.Profile.Key;
                            _EAB.AssortmentWorkspaceExplorer.DeleteAssortmentRows(hdrRIDs);
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
							_dlAssortment.CloseUpdateConnection();
							_dlFolder.CloseUpdateConnection();
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
                        DequeueHeaders();
					}
				}
				else if (aNode.isSubFolder)
				{
					deleteArray = new object[aNode.Nodes.Count];
					aNode.Nodes.CopyTo(deleteArray, 0);

					foreach (MIDAssortmentNode node in deleteArray)
					{
						DeleteAssortmentNode(node);
					}

					if (aNode.Nodes.Count == 0)
					{
						_dlFolder.OpenUpdateConnection();

						try
						{
							_dlFolder.Folder_Item_Delete(aNode.Profile.Key, eProfileType.AssortmentSubFolder);
							_dlFolder.Folder_Delete(aNode.Profile.Key, eProfileType.AssortmentSubFolder);
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
						DeleteChildNodes((MIDAssortmentNode)aNode);
						aNode.Remove();
					}
				}
				else if (aNode.isObjectShortcut)
				{
					_dlFolder.OpenUpdateConnection();

					try
					{
						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						//_dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.Assortment);
						_dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.AssortmentHeader);
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
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
						_dlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, eProfileType.AssortmentSubFolder);
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

					DeleteChildNodes((MIDAssortmentNode)aNode);
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

        private void DeletePlaceholderStyle(int aStyleRID)
        {
            try
            {
                EditMsgs em = new EditMsgs();
                HierarchyNodeProfile styleHnp = SAB.HierarchyServerSession.GetNodeData(aStyleRID);
                if (styleHnp.IsVirtual && (styleHnp.Purpose == ePurpose.Placeholder || styleHnp.Purpose == ePurpose.Assortment))
                {
                    HierarchyMaintenance hierMaint = new HierarchyMaintenance(SAB);
                    hierMaint.DeletePlaceholderStyleAnchorNode(aStyleRID, ref em);
                }
                if (em.ErrorFound)
                {
                    DisplayMessages.Show(em, SAB, Text);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateAssortmentWorkpsace(int asrtRID)
        {
            try
            {   // Begin #1286 - RMatelic - Asst - At what point does the Assortment show in the Assortment Workspace
                // ReloadUpdatedAssortments method doesn't take into account the Assortment Workspace Filter so change to Refresh
                //int[] hdrRIDs = new int[1];
                //hdrRIDs[0] = asrtRID;
                //_EAB.AssortmentWorkspaceExplorer.ReloadUpdatedAssortments(hdrRIDs);
                _EAB.AssortmentWorkspaceExplorer.IRefresh();
                // End TT#1286 
            }
            catch
            {
                throw;
            }
        }

        #region HeadersEnqueueDequeue
        private bool HeadersEnqueued(MIDAssortmentNode aNode)
        {
            bool processOK = false;
            AllocationHeaderProfileList headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
			// BEGIN Stodd - 4.0 to 4.1 Manual merge
            //ApplicationSessionTransaction appTransaction = null;
			string enqMessage = string.Empty;
			// END Stodd - 4.0 to 4.1 Manual merge
            try
            {
                ArrayList selectedNodeAL = new ArrayList();
                selectedNodeAL.Add(aNode);

				// BEGIN Stodd - 4.0 to 4.1 Manual merge
				_enqueueTransaction = NewTransFromSelectedHeaders(selectedNodeAL);
				// END Stodd - 4.0 to 4.1 Manual merge

                ArrayList headerListAL = GetAllHeadersInAssortment(selectedNodeAL);

                foreach (int hdrRID in headerListAL)
                {
                    AllocationHeaderProfile headerProfile = SAB.HeaderServerSession.GetHeaderData(hdrRID, false, false, false);
                    headerList.Add(headerProfile);
                }
               
				// BEGIN Stodd - 4.0 to 4.1 Manual merge
                //_headerEnqueue = new HeaderEnqueue(appTransaction, headerList);
                //_headerEnqueue.EnqueueHeaders();
				// Begin TT#903 - MD - "There are no headers to enqueue" error
                if (headerList.Count > 0)
                {
                    if (!_enqueueTransaction.EnqueueHeaders(headerList, out enqMessage))
                    {
                        throw new HeaderConflictException();
                    }
                }
				// End TT#903 - MD - "There are no headers to enqueue" error
				// END Stodd - 4.0 to 4.1 Manual merge

                processOK = true;
            }
            catch (HeaderConflictException)
            {
				// BEGIN Stodd - 4.0 to 4.1 Manual merge
				//DisplayEnqueueConflict(_enqueueTransaction, headerList);
				DisplayEnqueueConflict(enqMessage);
				// END Stodd - 4.0 to 4.1 Manual merge
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            return processOK;
        }

		// BEGIN Stodd - 4.0 to 4.1 Manual merge
        //private void DisplayEnqueueConflict(ApplicationSessionTransaction aAppTransaction, AllocationHeaderProfileList aHeaderList, string enqMessage)
		private void DisplayEnqueueConflict(string enqMessage)
        {
			//SecurityAdmin secAdmin = new SecurityAdmin();
			//string errMsg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_HeadersInUse) + ":" + System.Environment.NewLine;

			//foreach (HeaderConflict hdrCon in _headerEnqueue.HeaderConflictList)
			//{
			//    SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
			//    SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList.FindKey(System.Convert.ToInt32(hdrCon.HeaderRID, CultureInfo.CurrentUICulture));
			//    if (shp != null)
			//    {
			//        errMsg += System.Environment.NewLine + shp.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
			//    }
			//    else
			//    {
			//        foreach (AllocationHeaderProfile ahp in aHeaderList)
			//        {
			//            if (ahp.Key == hdrCon.HeaderRID)
			//            {
			//                errMsg += System.Environment.NewLine + ahp.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
			//                break;
			//            }
			//        }
			//    }
			//}

            //errMsg += System.Environment.NewLine + System.Environment.NewLine;
			DialogResult diagResult = _enqueueTransaction.SAB.MessageCallback.HandleMessage(
				enqMessage,
                "Header Lock Conflict",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
        }
		// END Stodd - 4.0 to 4.1 Manual merge

        public void DequeueHeaders()
        {
            try
            {
				// BEGIN Stodd - 4.0 to 4.1 Manual merge
				//if (_headerEnqueue != null)
				//{
				//    _headerEnqueue.DequeueHeaders();
				//    _headerEnqueue = null;
				//}

				_enqueueTransaction.DequeueHeaders();
				// END Stodd - 4.0 to 4.1 Manual merge
            }
            catch
            {
                throw;
            }
        }
        #endregion

		// BEGIN TT#483-MD - stodd - InUse error removing an assortment
		override protected bool AllowInUseDelete(ArrayList aDeleteList)
		{
			return true;
		}
		// END TT#483-MD - stodd 

        public void OpenAssortmentReview(MIDAssortmentNode aNode)
		{
			try
			{
				// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				//if (aNode.Profile.ProfileType != eProfileType.Assortment)
				if (aNode.Profile.ProfileType != eProfileType.AssortmentHeader)
				// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				{
					return;
				}

				if (aNode.Profile.Key == 0)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PlanViewNotAvailable));
				}
				else
					//if (SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aNode.Profile.Key, eSecurityFunctions.ForecastReview, (int)eSecurityTypes.Chain | (int)eSecurityTypes.Store).AccessDenied)
					//{
					//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
					//}
					//else
					{
						


						ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();
						System.Windows.Forms.Form frm = new MIDRetail.Windows.AssortmentView(_EAB, appTransaction, eAssortmentWindowType.Assortment); 

						((AssortmentView)frm).Initialize();
						if (((MIDRetail.Windows.AssortmentView)frm).FormLoaded)
						{
							frm.MdiParent = MDIParentForm;
							frm.WindowState = FormWindowState.Maximized;
							frm.Show();
						}
					}
				Cursor.Current = Cursors.Default;
			}
			catch
			{
				throw;
			}
		}

		public void OpenReviewSelection(ArrayList selectedNodes)
		{
			try
			{
				foreach (MIDAssortmentNode node in selectedNodes)
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//if (node.Profile.ProfileType != eProfileType.Assortment)
					if (node.Profile.ProfileType != eProfileType.AssortmentHeader)
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					{
						return;
					}
				}

				if (selectedNodes.Count == 0)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PlanViewNotAvailable));
				}
				else
				//if (SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aNode.Profile.Key, eSecurityFunctions.ForecastReview, (int)eSecurityTypes.Chain | (int)eSecurityTypes.Store).AccessDenied)
				//{
				//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
				//}
				//else
				{
					MIDRetail.Windows.AssortmentViewSelection form;
					ApplicationSessionTransaction appTransaction = NewTransFromSelectedHeaders(selectedNodes);
					//appTransaction.AllocationWorkspaceExplorer = AllocationWorkspaceExplorer1;
					form = new MIDRetail.Windows.AssortmentViewSelection(this._EAB, SAB, appTransaction, null, false);
					form.MdiParent = this.MDIParentForm;
					//form.MdiParent = this;
					//if (e.Tool.Key != "Select" && e.Tool.Key != Include.btSelect)
					//{
					//    form.WindowState = FormWindowState.Maximized;
					//}
					form.DetermineWindow(eAllocationSelectionViewType.None);



					//ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();
					//System.Windows.Forms.Form frm = new MIDRetail.Windows.AssortmentView(_EAB, appTransaction, eAssortmentWindowType.Assortment);

					//((AssortmentView)frm).Initialize();
					//if (((MIDRetail.Windows.AssortmentView)frm).FormLoaded)
					//{
					//    frm.MdiParent = MDIParentForm;
					//    frm.WindowState = FormWindowState.Maximized;
					//    frm.Show();
					//}
				}
				Cursor.Current = Cursors.Default;
			}
			catch
			{
				throw;
			}
		}

		public void OpenAssortmentWorkspace(MIDAssortmentNode aNode)
		{
			try
			{
				_EAB.Explorer.ShowExplorer(Include.tbbAssortmentWorkspace);
				Cursor.Current = Cursors.Default;
			}
			catch
			{
				throw;
			}
		}

		private ApplicationSessionTransaction NewTransFromSelectedHeaders(ArrayList selectedNodes)
		{
			try
			{
				ApplicationSessionTransaction newTrans = SAB.ApplicationServerSession.CreateTransaction();


				newTrans.NewAllocationMasterProfileList();
				
				ArrayList selectedHeaderKeyList = GetAllHeadersInAssortment(selectedNodes);

				int[] selectedHeaderArray = new int[selectedHeaderKeyList.Count];
				selectedHeaderKeyList.CopyTo(selectedHeaderArray);

                // begin TT#488 - MD - Jellis - Group Allocation
                int[] selectedAssortmentArray = new int[selectedNodes.Count];

                //BEGIN TT#876-MD-DOConnell-Getting an Invalid Cast Exception when trying to delete an assortment from the Assortment Explorer
                ArrayList selectedAssortmentKeyList = new ArrayList();
                foreach (MIDAssortmentNode node in selectedNodes)
                {
                    selectedAssortmentKeyList.Add(node.NodeRID);
                }
                selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);
                //selectedNodes.CopyTo(selectedAssortmentArray);
                //END TT#876-MD-DOConnell-Getting an Invalid Cast Exception when trying to delete an assortment from the Assortment Explorer
                // end TT#488 - MD - Jellis - Group Allocation

				newTrans.LoadHeaders(selectedAssortmentArray, selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
				return newTrans;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Fills in _selectedHeaderKeyList from aAsrtRID.
		/// </summary>
		/// <param name="aAsrtRID"></param>
        private ArrayList GetAllHeadersInAssortment(ArrayList selectedNodes)
		{
			ArrayList selectedHeaderKeyList = new ArrayList();
			try
			{
				foreach (MIDAssortmentNode node in selectedNodes)
				{
					ArrayList al = SAB.HeaderServerSession.GetHeadersInAssortment(node.NodeRID);
					for (int i = 0; i < al.Count; i++)
					{
						int hdrRID = (int)al[i];
                        if (hdrRID != node.NodeRID) // TT#488- MD - Jellis - Group Allocation
                        {     // TT#488 - MD - Jellis - Group ALlocation
                            selectedHeaderKeyList.Add(hdrRID);
                        }    // TT#488 - MD - Jellis - Group Allcoation
					}
				}
				return selectedHeaderKeyList;
			}
			catch
			{
				throw;
			}
		}

		private void OnAssortmentPropertiesChange(object source, AssortmentPropertiesChangeEventArgs e)
		{
			MIDAssortmentNode node = null;
			MIDAssortmentNode parentNode = null;

			try
			{
				// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				//node = (MIDAssortmentNode)ItemNodeHash[e.AssortmentProfile.Key];
				node = (MIDAssortmentNode)ItemNodeHash[e.AssortmentHeaderProfile.Key];
				// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

				if (node == null)
				{
					switch (e.ParentNode.Profile.ProfileType)
					{
						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						case eProfileType.AssortmentHeader:
						//case eProfileType.Assortment:
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

							parentNode = (MIDAssortmentNode)e.ParentNode.Parent;
							break;

						default:

							parentNode = (MIDAssortmentNode)e.ParentNode;
							break;
					}


					_dlFolder.OpenUpdateConnection();

					try
					{

						// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
						//_dlFolder.Folder_Item_Insert(e.ParentNode.Profile.Key, e.AssortmentProfile.Key, eProfileType.Assortment);
						_dlFolder.Folder_Item_Insert(e.ParentNode.Profile.Key, e.AssortmentHeaderProfile.Key, eProfileType.AssortmentHeader);
						// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

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
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//node = BuildAssortmentNode(e.AssortmentProfile, parentNode, Include.GlobalUserRID);
					node = BuildAssortmentNode(e.AssortmentHeaderProfile, parentNode, Include.GlobalUserRID);
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile

					SelectedNode = node;
				}
				else
				{
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					parentNode = (MIDAssortmentNode)node.Parent;
					//node.InternalText = e.AssortmentProfile.HeaderID;
					node.InternalText = e.AssortmentHeaderProfile.HeaderID;
					((AllocationHeaderProfile)node.Profile).HeaderID = e.AssortmentHeaderProfile.HeaderID;	//TT#207-MD - Stodd - argument exception after changing name of assortment
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
				}

				SortChildNodes(parentNode);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

    public class OnAssortmentPropertiesCloseClass
    {
		// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
        //private GenericEnqueue _objEnqueue;
		private ApplicationSessionTransaction _trans;

		//public OnAssortmentPropertiesCloseClass(GenericEnqueue aObjEnqueue)
		//{
		//    _objEnqueue = aObjEnqueue;
		//}

		public OnAssortmentPropertiesCloseClass(ApplicationSessionTransaction aTrans)
		{
			_trans = aTrans;
		}

        public void OnClose(object source, AssortmentPropertiesCloseEventArgs e)
        {
            try
            {
				if (_trans != null)
				{
					_trans.DequeueHeaders();
				}
				//if (_objEnqueue != null)
				//{
				//    if (!_objEnqueue.IsInConflict)
				//    {
				//        _objEnqueue.DequeueGeneric();
				//    }
				//}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
    }

	public class MIDAssortmentNode : MIDTreeNode
	{
		//=============
		// CONSTRUCTORS
		//=============

        public MIDAssortmentNode()
            : base()
		{
		}

		public MIDAssortmentNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			string aText,
			int aParentId,
			int aUserId,
			MIDTreeNodeSecurityGroup aMIDTreeNodeSecurityGroup,
			int aImageIndex,
			int aSelectedImageIndex,
			int aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, aMIDTreeNodeSecurityGroup, false, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
		{
		}

		public MIDAssortmentNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			string aText,
			int aParentId,
			int aUserId,
			MIDTreeNodeSecurityGroup aMIDTreeNodeSecurityGroup,
			int aCollapsedImageIndex,
			int aSelectedCollapsedImageIndex,
			int aExpandedImageIndex,
			int aSelectedExpandedImageIndex,
			int aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, aMIDTreeNodeSecurityGroup, false, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
		{
		}

		//===========
		// PROPERTIES
		//===========

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
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//(NodeProfileType == eProfileType.Assortment ||
					(NodeProfileType == eProfileType.AssortmentHeader ||
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					NodeProfileType == eProfileType.AssortmentSubFolder))
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

						if (!aDestinationNode.FunctionSecurityProfile.AllowUpdate)
						{
							return false;
						}

						if (aDropAction == DragDropEffects.Copy)
						{
							if (aDestinationNode.isChildOf(this))
							{
								return false;
							}
							else
							{
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
                // Begin TT#1998-MD - JSmith - Security - Assortment Permissions
                //if (FunctionSecurityProfile.AllowUpdate && !isShortcut &&
                //    (NodeProfileType == eProfileType.AssortmentMainFavoritesFolder ||
                //    NodeProfileType == eProfileType.AssortmentSubFolder ||
                //    // BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
                //    //NodeProfileType == eProfileType.Assortment))
                //    NodeProfileType == eProfileType.AssortmentHeader))
                //    // END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                if (isShortcut)
                {
                    return false;
                }

                if (NodeProfileType == eProfileType.AssortmentSubFolder)
                {
                    if (OwnerUserRID == Include.GlobalUserRID)
                    {
                        if (((AssortmentTreeView)TreeView).GlobalAssortmentGroupSecGrp.FolderSecurityProfile.AllowUpdate)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (GetTopNode().isMainFavoriteFolder)
                        {
                            return true;
                        }
                        else if (((AssortmentTreeView)TreeView).UserAssortmentGroupSecGrp.FolderSecurityProfile.AllowUpdate)
                        {
                            return true;
                        }
                    }
                }
                else if (NodeProfileType == eProfileType.AssortmentHeader)
                {
                    if (((AssortmentTreeView)TreeView).GlobalAssortmentGroupSecGrp.FunctionSecurityProfile.AllowUpdate)
                    {
                        return true;
                    }
                }
                
                return false;
                // End TT#1998-MD - JSmith - Security - Assortment Permissions
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

					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//if (Profile.ProfileType == eProfileType.Assortment)
					if (Profile.ProfileType == eProfileType.AssortmentHeader)
					{
						//InternalText = ((AssortmentProfile)aChangedNode.Profile).HeaderID + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((AssortmentHeaderProfile)aChangedNode.Profile).HeaderID + " (" + GetUserName(aChangedNode.UserId) + ")";
					}
					// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					
				}
				else if (isChildShortcut)
				{
					UserId = aChangedNode.UserId;

					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					//if (((Profile)Profile).ProfileType == eProfileType.Assortment)
					if (((Profile)Profile).ProfileType == eProfileType.AssortmentHeader)
					// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
					{
						//InternalText = ((AssortmentProfile)aChangedNode.Profile).Name;
						InternalText = ((AssortmentHeaderProfile)aChangedNode.Profile).HeaderID;
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
}
