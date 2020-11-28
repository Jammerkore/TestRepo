using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class FolderDataLayer : DataLayer {

        public FolderDataLayer() : base()
		{

		}

        public FolderDataLayer(TransactionData td)
            : base(td.DBA, true) 
        {

        }

		#region FOLDER

		/// <summary>
		/// Returns the folder with the given folder id.
		/// </summary>
		/// <param name="aFolderRID">the id of the folder to retreive</param>
		/// <returns>the dtatrow of with the given info from the database otherwise returns null</returns>
		public DataTable Folder_Read(int aUserRID, eProfileType aFolderType)
		{
			ArrayList userList;
			try
			{		
				userList = new ArrayList();
				userList.Add(aUserRID);
				return Folder_Read(userList, aFolderType, true, false);
			}
			catch
			{
				throw;
			}
		}


		public DataTable Folder_Read(ArrayList aUserRIDList, eProfileType aFolderType, bool aIncludeOwned, bool aIncludeAssigned)
		{
			try
			{

                int FOLDER_TYPE = Convert.ToInt32(aFolderType);
                int ITEM_TYPE = Convert.ToInt32(aFolderType); //Folder type is also used for item type

                DataTable dtUserList = null;
                if (aUserRIDList != null)
                {
                    dtUserList = new DataTable();
                    dtUserList.Columns.Add("USER_RID", typeof(int));
                    foreach (int userRID in aUserRIDList)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                        {
                            DataRow dr = dtUserList.NewRow();
                            dr["USER_RID"] = userRID;
                            dtUserList.Rows.Add(dr);
                        }
                    }
                }


				if (aIncludeAssigned && aIncludeOwned)
				{
                    if (aUserRIDList != null)
                    {
                        return StoredProcedures.MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS.Read(_dba,
                                                                                          FOLDER_TYPE: FOLDER_TYPE,
                                                                                          ITEM_TYPE: ITEM_TYPE,
                                                                                          USER_RID_LIST: dtUserList
                                                                                          );
                    }
                    else
                    {
                        return StoredProcedures.MID_FOLDER_READ_ASSIGNED_AND_OWNED.Read(_dba,
                                                                                FOLDER_TYPE: FOLDER_TYPE,
                                                                                ITEM_TYPE: ITEM_TYPE
                                                                                );
                    }
    
				}
				else if (aIncludeAssigned)
				{
                    if (aUserRIDList != null)
                    {
                        return StoredProcedures.MID_FOLDER_READ_ASSIGNED_FOR_USERS.Read(_dba,
                                                                                FOLDER_TYPE: FOLDER_TYPE,
                                                                                ITEM_TYPE: ITEM_TYPE,
                                                                                USER_RID_LIST: dtUserList
                                                                                );
                    }
                    else
                    {
                        return StoredProcedures.MID_FOLDER_READ_ASSIGNED.Read(_dba,
                                                                      FOLDER_TYPE: FOLDER_TYPE,
                                                                      ITEM_TYPE: ITEM_TYPE
                                                                      );
                    }
         
				}
				else if (aIncludeOwned)
				{
                    if (aUserRIDList != null)
                    {
                        return StoredProcedures.MID_FOLDER_READ_OWNED_FOR_USERS.Read(_dba,
                                                                                FOLDER_TYPE: FOLDER_TYPE,
                                                                                ITEM_TYPE: ITEM_TYPE,
                                                                                USER_RID_LIST: dtUserList
                                                                                );
                    }
                    else
                    {
                        return StoredProcedures.MID_FOLDER_READ_OWNED.Read(_dba,
                                                                   FOLDER_TYPE: FOLDER_TYPE,
                                                                   ITEM_TYPE: ITEM_TYPE
                                                                   );
                    }
                   
				}

                return StoredProcedures.MID_FOLDER_READ.Read(_dba,
                                                             FOLDER_TYPE: FOLDER_TYPE,
                                                             ITEM_TYPE: ITEM_TYPE
                                                             );

			}
			catch
			{
				throw;
			}
		}

    
		public DataTable Folder_Read(int aFolderRID)
		{
			try
			{
                return StoredProcedures.MID_FOLDER_READ_FROM_RID.Read(_dba, FOLDER_RID: aFolderRID);
			}
			catch
			{
				throw;
			}
		}

        public string Folder_GetName(int aFolderRID)
        {
            DataTable dt;
            string name = string.Empty;
            try
            {
                dt = Folder_Read(aFolderRID);
                if (dt != null && dt.Rows.Count == 1)
                {
                    name = Convert.ToString(dt.Rows[0]["FOLDER_ID"], CultureInfo.CurrentCulture);
                }
                return name;
            }
            catch
            {
                throw;
            }
        }

        public eProfileType Folder_GetType(int aFolderRID)
        {
            DataTable dt;
            eProfileType folderType = eProfileType.None;
            try
            {
                dt = Folder_Read(aFolderRID);
                if (dt != null && dt.Rows.Count == 1)
                {
                    folderType = (eProfileType)Convert.ToInt32(dt.Rows[0]["FOLDER_TYPE"]);
                }
                return folderType;
            }
            catch
            {
                throw;
            }
        }

		public int Folder_GetKey(int aUserRID, string aFolderName, int aParentRID, eProfileType aFolderType)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_FOLDER_READ_KEY.Read(_dba,
                                                                 FOLDER_TYPE: Convert.ToInt32(aFolderType),
                                                                 USER_RID: aUserRID,
                                                                 FOLDER_ID: aFolderName,
                                                                 PARENT_FOLDER_RID: aParentRID
				                                                 );

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["FOLDER_RID"], CultureInfo.CurrentUICulture));
				}
				else
				{
					return -1;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Creates a new folder with the given name, it must still be placed in another folder
		/// for it to display anywhere
		/// </summary>
		/// <param name="name">Name of the folder</param>
		/// <param name="aUserRID">the user rid of the owner, system for global folders</param>
		/// <returns>true if successful</returns>
		public int Folder_Create(int aUserRID, string aName, eProfileType aFolderType)
		{
			try
			{
                int folderRID = StoredProcedures.SP_MID_FOLDER_INSERT.InsertAndReturnRID(_dba,
                                                                                         USER_ID: aUserRID,
                                                                                         FOLDER_ID: aName,
                                                                                         FOLDER_TYPE: (int)aFolderType
                                                                                         );

				

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(aUserRID, Convert.ToInt32(aFolderType), folderRID, aUserRID);
                }
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    sa.AddUserItem(aUserRID, Convert.ToInt32(aFolderType), folderRID, aUserRID);
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login


				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				return folderRID;
				//End Track #4815
			}
			catch
			{
				throw;
			}
		}

		public void Folder_Update(int aFolderRID, int aUserRID, string aName, eProfileType aFolderType)
		{
			try
			{
                StoredProcedures.MID_FOLDER_UPDATE.Update(_dba,
                                                          FOLDER_RID: aFolderRID,
                                                          USER_RID: aUserRID,
                                                          FOLDER_ID: aName,
                                                          FOLDER_TYPE: (int)aFolderType
                                                          );
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// removes all the items from the folder and then removes all instaces of this folder in other folders and then
		/// finally delete's the folder
		/// </summary>
		/// <param name="aFolderRID">rid of the folder to delete</param>
		/// <returns>true if all level's are successful</returns>
		public void Folder_Delete(int aFolderRID, eProfileType aSharedDataType)
		{			
			try
			{
                StoredProcedures.MID_FOLDER_DELETE.Delete(_dba, FOLDER_RID: aFolderRID);

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.DeleteUserItemByTypeAndRID(Convert.ToInt32(aSharedDataType), aFolderRID);
                }

                //// Begin Track #4872 - JSmith - Global/User Attributes
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    sa.DeleteUserItemByTypeAndRID(Convert.ToInt32(aSharedDataType), aFolderRID);
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //// End Track #4872
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// Rename a folder with the given aFolderRID to the new name
		/// </summary>
		/// <param name="aFolderRID">the id of the exisiting folder</param>
		/// <param name="aNewName">the new name to rename it</param>
		/// <returns>true if successful</returns>	
		public void Folder_Rename(int aFolderRID, string aNewName) 
		{
			try
			{
                StoredProcedures.MID_FOLDER_UPDATE_ID.Update(_dba,
                                                             FOLDER_RID: aFolderRID,
                                                             FOLDER_ID: aNewName
                                                             );
			}
			catch
			{
				throw;
			}
		}
		
		#endregion

		#region FOLDER ITEMS

		public DataTable Folder_Item_Read(ArrayList aUserRIDList, ArrayList aFolderTypeList, eProfileType aFolderChildType, bool aIncludeOwned, bool aIncludeAssigned)
		{
			try
			{
                DataTable dtFolderTypeList = null;
                if (aFolderTypeList != null)
                {
                    dtFolderTypeList = new DataTable();
                    dtFolderTypeList.Columns.Add("FOLDER_TYPE", typeof(int));
                    foreach (int folderType in aFolderTypeList)
                    {
                        //ensure folderTypes are distinct, and only added to the datatable one time
                        if (dtFolderTypeList.Select("FOLDER_TYPE=" + folderType.ToString()).Length == 0)
                        {
                            DataRow dr = dtFolderTypeList.NewRow();
                            dr["FOLDER_TYPE"] = folderType;
                            dtFolderTypeList.Rows.Add(dr);
                        }
                    }
                }

                DataTable dtUserList = null;
                if (aUserRIDList != null)
                {
                    dtUserList = new DataTable();
                    dtUserList.Columns.Add("USER_RID", typeof(int));
                    foreach (int userRID in aUserRIDList)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                        {
                            DataRow dr = dtUserList.NewRow();
                            dr["USER_RID"] = userRID;
                            dtUserList.Rows.Add(dr);
                        }
                    }
                }



				if (aIncludeAssigned && aIncludeOwned)
				{
					if (aUserRIDList != null)
					{
                        return StoredProcedures.MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED.Read(_dba,
                                                                                             FOLDER_TYPE_LIST: dtFolderTypeList,
                                                                                             CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType),
                                                                                             USER_RID_LIST: dtUserList
                                                                                             );
					}
				}
				else if (aIncludeAssigned)
				{
					if (aUserRIDList != null)
					{
                        return StoredProcedures.MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED.Read(_dba,
                                                                                   FOLDER_TYPE_LIST: dtFolderTypeList,
                                                                                   CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType),
                                                                                   USER_RID_LIST: dtUserList
                                                                                   );
					}

                    return StoredProcedures.MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS.Read(_dba,
                                                                                            FOLDER_TYPE_LIST: dtFolderTypeList,
                                                                                            CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType)
                                                                                            );
				}
				else if (aIncludeOwned)
				{
					if (aUserRIDList != null)
					{
                        return StoredProcedures.MID_FOLDER_READ_CHILDREN_FOR_OWNED.Read(_dba,
                                                                                FOLDER_TYPE_LIST: dtFolderTypeList,
                                                                                CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType),
                                                                                USER_RID_LIST: dtUserList
                                                                                );
					}
				}

                return StoredProcedures.MID_FOLDER_READ_CHILDREN.Read(_dba,
                                                                      FOLDER_TYPE_LIST: dtFolderTypeList,
                                                                      CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType)
                                                                      );
			}
			catch
			{
				throw;
			}
		}

       
		public DataTable Folder_Children_Read(int aUserRID, int aFolderRID)
		{
            DataTable dt;
            DataTable itemDt;
            bool isShortcut;
            int childItemType;
            int childItemRID;

			try
			{

                dt = StoredProcedures.MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS.Read(_dba,
                                                                                     PARENT_FOLDER_RID: aFolderRID,
                                                                                     USER_RID: aUserRID
                                                                                     );


                // replace real owner for shortcuts 
                foreach (DataRow dr in dt.Rows)
                {
                    isShortcut = Include.ConvertCharToBool(Convert.ToChar(dr["SHORTCUT_IND"], CultureInfo.CurrentUICulture));
                    if (isShortcut)
                    {
                        childItemType = Convert.ToInt32(dr["CHILD_ITEM_TYPE"]);
                        childItemRID = Convert.ToInt32(dr["CHILD_ITEM_RID"]);
                        if (Enum.IsDefined(typeof(eMethodProfileType),childItemType))
                        {
                            itemDt = StoredProcedures.MID_METHOD_READ_USER.Read(_dba, METHOD_RID: childItemRID);
                            if (itemDt.Rows.Count > 0)
                            {
                                dr["OWNER_USER_RID"] = Convert.ToInt32(itemDt.Rows[0]["USER_RID"]);
                            }
                        }
                        else if (Enum.IsDefined(typeof(eWorkflowProfileType),childItemType))
                        {
                            itemDt = StoredProcedures.MID_WORKFLOW_READ_USER.Read(_dba, WORKFLOW_RID: childItemRID);
                            if (itemDt.Rows.Count > 0)
                            {
                                dr["OWNER_USER_RID"] = Convert.ToInt32(itemDt.Rows[0]["WORKFLOW_USER_RID"]);
                            }
                        }
                        else
                        {
                            itemDt = StoredProcedures.MID_FOLDER_READ_USER.Read(_dba, FOLDER_RID: childItemRID);
                            if (itemDt.Rows.Count > 0)
                            {
                                dr["OWNER_USER_RID"] = Convert.ToInt32(itemDt.Rows[0]["USER_RID"]);
                            }
                        }
                    }
                }
                return dt;
			}
			catch
			{
				throw;
			}
		}

      
		/// <summary>
		/// Add a reference to a child item to the folder
		/// </summary>
		/// <param name="aUserRID">user id of the user</param>
		/// <param name="aFolderRID">the folder id of the folder to add to</param>
		/// <param name="aChildRID">the id of the child item</param>
		/// <param name="aChildType">the type of child it is</param>
		/// <returns></returns>
		public void Folder_Item_Insert(int aFolderRID, int aChildRID, eProfileType aFolderChildType)
		{
			try
			{
                StoredProcedures.MID_FOLDER_JOIN_INSERT.Insert(_dba,
                                                               PARENT_FOLDER_RID: aFolderRID,
                                                               CHILD_ITEM_RID: aChildRID,
                                                               CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType)
                                                               );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Delete all joins for a certain type
		/// </summary>
		/// <param name="aChildRID">the child id of the type</param>
		/// <param name="aChildType">the type of the child</param>
		/// <returns></returns>
		public void Folder_Item_Delete(int aChildRID, eProfileType aFolderChildType)
		{
			try
			{
                StoredProcedures.MID_FOLDER_JOIN_DELETE.Delete(_dba,
                                                               CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType),
                                                               CHILD_ITEM_RID: aChildRID
                                                               );
			}
			catch
			{
				throw;
			}
		}

        // Begin Track #5005 - JSmith - Explorer Organization
        public bool Folder_Children_Exists(int aUserRID, int aParentFolderRID)
        {
            try
            {
                DataTable dt = Folder_Children_Read(aUserRID, aParentFolderRID);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }


		public bool Folder_Child_Exists(int aFolderRID, int aChildRID, eProfileType aFolderChildType)
		{
            try
            {
                DataTable dt = StoredProcedures.MID_FOLDER_JOIN_READ.Read(_dba,
                                                                  PARENT_FOLDER_RID: aFolderRID,
                                                                  CHILD_ITEM_RID: aChildRID,
                                                                  CHILD_ITEM_TYPE: Convert.ToInt32(aFolderChildType)
                                                                  );
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
		
		#endregion

		#region FOLDER SHORTCUTS

		public bool Folder_Shortcut_Exists(int aFolderRID, int aChildRID, eProfileType aFolderChildType)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_FOLDER_SHORTCUT_READ.Read(_dba,
                                                                      CHILD_SHORTCUT_TYPE: Convert.ToInt32(aFolderChildType),
                                                                      CHILD_SHORTCUT_RID: aChildRID,
                                                                      PARENT_FOLDER_RID: aFolderRID
				                                                      );
				if (dt.Rows.Count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable Folder_Shortcut_Folder_Read(ArrayList aUserRIDList, ArrayList aFolderTypeList)
		{
			try
			{
                DataTable dtFolderTypeList = null;
                if (aFolderTypeList != null)
                {
                    dtFolderTypeList = new DataTable();
                    dtFolderTypeList.Columns.Add("FOLDER_TYPE", typeof(int));
                    foreach (int folderType in aFolderTypeList)
                    {
                        //ensure folderTypes are distinct, and only added to the datatable one time
                        if (dtFolderTypeList.Select("FOLDER_TYPE=" + folderType.ToString()).Length == 0)
                        {
                            DataRow dr = dtFolderTypeList.NewRow();
                            dr["FOLDER_TYPE"] = folderType;
                            dtFolderTypeList.Rows.Add(dr);
                        }
                    }
                }

                DataTable dtUserList = null;
                if (aUserRIDList != null)
                {
                    dtUserList = new DataTable();
                    dtUserList.Columns.Add("USER_RID", typeof(int));
                    foreach (int userRID in aUserRIDList)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                        {
                            DataRow dr = dtUserList.NewRow();
                            dr["USER_RID"] = userRID;
                            dtUserList.Rows.Add(dr);
                        }
                    }
                }


				if (aUserRIDList != null)
				{
                    return StoredProcedures.MID_FOLDER_SHORTCUT_READ_FOR_USERS.Read(_dba,
                                                                                USER_RID_LIST: dtUserList,
                                                                                FOLDER_TYPE_LIST: dtFolderTypeList
                                                                                );
				}
				else
				{
                    return StoredProcedures.MID_FOLDER_SHORTCUT_READ_FOR_TYPES.Read(_dba, FOLDER_TYPE_LIST: dtFolderTypeList);
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable Folder_Shortcut_Item_Read(ArrayList aUserRIDList, eProfileType aFolderChildType)
		{
			try
			{
				if (aUserRIDList != null)
				{
                    DataTable dtUserList = new DataTable();
                    dtUserList.Columns.Add("USER_RID", typeof(int));
                    foreach (int userRID in aUserRIDList)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                        {
                            DataRow dr = dtUserList.NewRow();
                            dr["USER_RID"] = userRID;
                            dtUserList.Rows.Add(dr);
                        }
                    }
                    return StoredProcedures.MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS.Read(_dba,
                                                                                       USER_RID_LIST: dtUserList,
                                                                                       CHILD_SHORTCUT_TYPE: Convert.ToInt32(aFolderChildType)
                                                                                       );
				}
				else
				{
                    return StoredProcedures.MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE.Read(_dba, CHILD_SHORTCUT_TYPE: Convert.ToInt32(aFolderChildType));
				}
			}
			catch
			{
				throw;
			}
		}

		public void Folder_Shortcut_Insert(int aFolderRID, int aChildRID, eProfileType aFolderChildType)
		{
			try
			{
                StoredProcedures.MID_FOLDER_SHORTCUT_INSERT.Insert(_dba,
                                                                   PARENT_FOLDER_RID: aFolderRID,
                                                                   CHILD_SHORTCUT_RID: aChildRID,
                                                                   CHILD_SHORTCUT_TYPE: Convert.ToInt32(aFolderChildType)
                                                                   );
			}
			catch
			{
				throw;
			}
		}

		public void Folder_Shortcut_Delete(int aFolderRID, int aChildRID, eProfileType aFolderChildType)
		{
			try
			{
                StoredProcedures.MID_FOLDER_SHORTCUT_DELETE.Delete(_dba,
                                                                   PARENT_FOLDER_RID: aFolderRID,
                                                                   CHILD_SHORTCUT_RID: aChildRID,
                                                                   CHILD_SHORTCUT_TYPE: Convert.ToInt32(aFolderChildType) 
                                                                   );
			}
			catch
			{
				throw;
			}
		}

		public void Folder_Shortcut_DeleteAll(int aChildRID, eProfileType aFolderChildType)
		{
			try
			{
                StoredProcedures.MID_FOLDER_SHORTCUT_DELETE_ALL.Delete(_dba,
                                                                       CHILD_SHORTCUT_RID: aChildRID,
                                                                       CHILD_SHORTCUT_TYPE: Convert.ToInt32(aFolderChildType)
                                                                       );
			}
			catch
			{
				throw;
			}
		}
		
		#endregion

    
	}
}
