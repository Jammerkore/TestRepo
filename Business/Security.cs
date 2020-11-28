using System;
using System.Data;
using System.Collections;
using System.Globalization;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;


namespace MIDRetail.Business
{
	/// <summary>
	/// Other than the tree chase functionality in GetUserNodeAssignment(), this class 
	/// exposes a subset of the functionality of MIDRetail.DATA.SecurityAdmin
	/// However, unlike SecurityAdmin, this class is intended for use by entire application.
	/// 
	/// At this point, no security data is being cached in a session, all calls go to the
	/// database.
	/// </summary>
	public class Security
	{
		int _userRID;
		SecurityAdmin _secAdmin;
		Hashtable _functionPath = null;
		DataTable _functionsDataTable = null;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		Hashtable _functionActionHash = null;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		public Security()
		{
			_userRID = 0;
			_secAdmin = new SecurityAdmin();
			_functionPath = new Hashtable();
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
			_functionActionHash = new Hashtable();
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		}

		public int UserRID
		{
			get { return _userRID ; }
		}

		public eSecurityAuthenticate SetUserPassword(string userName, string oldPassword, string newPassword)
		{
			try
			{
				_secAdmin.OpenUpdateConnection();
				eSecurityAuthenticate retVal = _secAdmin.SetUserPassword(userName,  oldPassword, newPassword, true);
				_secAdmin.CommitData();
				return retVal;
			}
			catch (Exception err)
			{
				throw(err);
			}
			finally
			{
				_secAdmin.CloseUpdateConnection();
			}
		}

		public eSecurityAuthenticate AuthenticateUser(string userName, string userPassword)
		{
			try
			{
				eSecurityAuthenticate retVal = _secAdmin.AuthenticateUser(userName,  userPassword);
				_userRID = _secAdmin.UserRID;	// make the userRID available
				return retVal;
			}
			catch
			{
				throw;
			}
		}

		public eSecurityAuthenticate AuthenticateUser(int aUserRID)
		{
			try
			{
				eSecurityAuthenticate retVal = _secAdmin.AuthenticateUser(aUserRID);
				_userRID = _secAdmin.UserRID;	// make the userRID available
				return retVal;
			}
			catch
			{
				throw;
			}
		}

		public string GetUserName(int userRID)
		{
            //Begin TT#827-MD -jsobek -Allocation Reviews Performance
            //try
            //{
            //    return _secAdmin.GetUserName(userRID);
            //}
            //catch
            //{
            //    throw;
            //}

            return UserNameStorage.GetUserName(userRID);
          
            //End TT#827-MD -jsobek -Allocation Reviews Performance
		}

		public VersionSecurityProfile GetGroupVersionAssignment(int groupRID, int forVerRID, int aSecurityType)
		{
			try
			{
				return GetGroupVersionAssignment(groupRID, forVerRID, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}

		public VersionSecurityProfile GetGroupVersionAssignment(int groupRID, int forVerRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				VersionSecurityProfile securityProfile = new VersionSecurityProfile(forVerRID);

				ArrayList databaseSecurityTypes = GetSecurityDatabaseTypes(aSecurityType);

				DataTable versionActionsDataTable = _secAdmin.GetActionsForVersion();

				// default security so actions will be known
				foreach (DataRow actionRow in versionActionsDataTable.Rows)
				{
					eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					securityProfile.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Version, forVerRID, 0, aGetInheritancePath, false);
					securityProfile.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				}

				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable groupVersionAssignmentDataTable = _secAdmin.GetGroupVersionAssignment(groupRID, forVerRID, databaseSecurityType);

					foreach (DataRow dr in groupVersionAssignmentDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
						securityProfile.SetSecurity(action, securityLevel);
					}
				}
				return securityProfile;
			}
			catch
			{
				throw;
			}
		}

		public VersionSecurityProfile GetUserVersionAssignment(int userRID, int forVerRID, int aSecurityType)
		{
			try
			{
				return GetUserVersionAssignment(userRID, forVerRID, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}

		public VersionSecurityProfile GetUserVersionAssignment(int userRID, int forVerRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				int securityInheritanceOffset = 0;
				VersionSecurityProfile securityProfile = new VersionSecurityProfile(forVerRID);

				DataTable versionActionsDataTable = _secAdmin.GetActionsForVersion();

				// default security so actions will be known
				foreach (DataRow actionRow in versionActionsDataTable.Rows)
				{
					eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					securityProfile.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Version, forVerRID, 0, aGetInheritancePath, false);
					securityProfile.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				}

				if (userRID == Include.AdministratorUserRID || userRID == Include.SystemUserRID || userRID == Include.GlobalUserRID)
				{
					securityProfile.SetFullControl();
					return securityProfile;
				}

				// if need inheritance path, create a record for each action in the function
				if (aGetInheritancePath)
				{
					foreach (DataRow actionRow in versionActionsDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
						securityProfile.SetInheritancePath(action, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Version, forVerRID, eSecurityLevel.NotSpecified, false);
					}
					// add each group user is in to inheritance path
					DataTable userGroupsDataTable = _secAdmin.GetGroups(userRID);
					foreach (DataRow groupRow in userGroupsDataTable.Rows)
					{
						int groupRID = Convert.ToInt32(groupRow["GROUP_RID"], CultureInfo.CurrentUICulture);
						foreach (DataRow actionRow in versionActionsDataTable.Rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
							securityProfile.SetInheritancePath(action, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Version, forVerRID, eSecurityLevel.NotSpecified, false);
						}
					}
				}

				ArrayList databaseSecurityTypes = GetSecurityDatabaseTypes(aSecurityType);

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				// get security for user
//				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
//				{
//					DataTable userVersionAssignmentDataTable = _secAdmin.GetUserVersionAssignment(userRID, forVerRID, databaseSecurityType);
//
//					foreach (DataRow dr in userVersionAssignmentDataTable.Rows)
//					{
//						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
//						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
//						securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Version, forVerRID, 0, aGetInheritancePath, false);
//					}
//				}
//
//				// get security for groups
//				// set offset to 1 since can only inherit from groups
//				securityInheritanceOffset = 1;
//				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
//				{
//					DataTable userVersionAssignmentDataTable = _secAdmin.GetUserGroupsVersionAssignment(userRID, forVerRID, databaseSecurityType);
//
//					foreach (DataRow dr in userVersionAssignmentDataTable.Rows)
//					{
//						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
//						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
//						int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
//						securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Version, forVerRID, securityInheritanceOffset, aGetInheritancePath, true);
//					}
//				}
				// get security for groups
				// set offset to 1 since can only inherit from groups
				securityInheritanceOffset = 1;
				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable userVersionAssignmentDataTable = _secAdmin.GetUserGroupsVersionAssignment(userRID, forVerRID, databaseSecurityType);

					foreach (DataRow dr in userVersionAssignmentDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
						int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
                        // Begin Track #5858 - JSmith - Validating store security only
                        //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Version, forVerRID, securityInheritanceOffset, aGetInheritancePath, true);
                        securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Version, forVerRID, securityInheritanceOffset, aGetInheritancePath, true, databaseSecurityType);
                        // End Track #5858
					}
				}

				// get security for user
				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable userVersionAssignmentDataTable = _secAdmin.GetUserVersionAssignment(userRID, forVerRID, databaseSecurityType);

					foreach (DataRow dr in userVersionAssignmentDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
                        // Begin Track #5858 - JSmith - Validating store security only
                        //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Version, forVerRID, 0, aGetInheritancePath, false);
                        securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Version, forVerRID, 0, aGetInheritancePath, false, databaseSecurityType);
                        // End Track #5858
					}
				}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				return securityProfile;
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetGroupFunctionAssignment(int groupRID, eSecurityFunctions aFuncID)
		{
			try
			{
				return GetGroupFunctionAssignment(groupRID, aFuncID, false);
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetGroupFunctionAssignment(int groupRID, eSecurityFunctions aFuncID, bool aGetInheritancePath)
		{
			try
			{
				int securityInheritanceOffset = 0;
				FunctionSecurityProfile securityProfile= new FunctionSecurityProfile(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
				ArrayList functions = GetFunctionPath (aFuncID);

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				DataTable functionActionsDataTable = _secAdmin.GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
				DataTable functionActionsDataTable = GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				// default security so actions will be known
				foreach (DataRow actionRow in functionActionsDataTable.Rows)
				{
					eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					securityProfile.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(aFuncID, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false);
					securityProfile.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				}

				// if need inheritance path, create a record for each action in the function
				if (aGetInheritancePath)
				{
					// add user to inheritance path
					foreach (eSecurityFunctions securityFunction in functions)
					{
//						DataTable functionActionsDataTable = _secAdmin.GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
						foreach (DataRow actionRow in functionActionsDataTable.Rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
							securityProfile.SetInheritancePath(action, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), eSecurityLevel.NotSpecified, false);
						}
					}
				}

				// determine security for group
				foreach (eSecurityFunctions securityFunction in functions)
				{
					DataTable groupFunctionAssignmentDataTable = _secAdmin.GetGroupFunctionAssignment(groupRID, securityFunction);

					foreach (DataRow dr in groupFunctionAssignmentDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
                        // Begin Track #5858 - JSmith - Validating store security only
                        //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false);
                        securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false, eDatabaseSecurityTypes.NotSpecified);
                        // End Track #5858
					}
					++securityInheritanceOffset;
				}
				return securityProfile;
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetUserFunctionAssignment(int userRID, eSecurityFunctions aFuncID)
		{
			try
			{
				return GetUserFunctionAssignment(userRID, aFuncID, false);
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetUserFunctionAssignment(int userRID, eSecurityFunctions aFuncID, bool aGetInheritancePath)
		{
			try
			{
				int securityInheritanceOffset = 0;
				FunctionSecurityProfile securityProfile= new FunctionSecurityProfile(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				DataTable functionActionsDataTable = _secAdmin.GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
				DataTable functionActionsDataTable = GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				// default security so actions will be known
				foreach (DataRow actionRow in functionActionsDataTable.Rows)
				{
					eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					securityProfile.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(aFuncID, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false);
					securityProfile.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				}

				if (userRID == Include.AdministratorUserRID || userRID == Include.SystemUserRID || userRID == Include.GlobalUserRID)
				{
					securityProfile.SetFullControl();
					return securityProfile;
				}

				ArrayList functions = GetFunctionPath (aFuncID);

				// if need inheritance path, create a record for each action in the function
				if (aGetInheritancePath)
				{
					// add user to inheritance path
					foreach (eSecurityFunctions securityFunction in functions)
					{
//						DataTable functionActionsDataTable = _secAdmin.GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
						foreach (DataRow actionRow in functionActionsDataTable.Rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
							securityProfile.SetInheritancePath(action, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), eSecurityLevel.NotSpecified, false);
						}
					}
					// add each group user is in to inheritance path
					DataTable userGroupsDataTable = _secAdmin.GetGroups(userRID);
					foreach (eSecurityFunctions securityFunction in functions)
					{
//						DataTable functionActionsDataTable = _secAdmin.GetActionsForFunction(Convert.ToInt32(aFuncID, CultureInfo.CurrentCulture));
						foreach (DataRow groupRow in userGroupsDataTable.Rows)
						{
							int groupRID = Convert.ToInt32(groupRow["GROUP_RID"], CultureInfo.CurrentUICulture);
							foreach (DataRow actionRow in functionActionsDataTable.Rows)
							{
								eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
								securityProfile.SetInheritancePath(action, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), eSecurityLevel.NotSpecified, false);
							}
						}
					}
				}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				// get security for user
//				foreach (eSecurityFunctions securityFunction in functions)
//				{
//					DataTable userFunctionAssignmentDataTable = _secAdmin.GetUserFunctionAssignment(userRID, securityFunction);
//			
//					foreach (DataRow dr in userFunctionAssignmentDataTable.Rows)
//					{
//						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
//						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
//						securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false);
//					}
//					++securityInheritanceOffset;
//				}
//
//				// get security for groups
//				foreach (eSecurityFunctions securityFunction in functions)
//				{
//					DataTable userFunctionAssignmentDataTable = _secAdmin.GetUserGroupsFunctionAssignment(userRID, securityFunction);
//			
//					foreach (DataRow dr in userFunctionAssignmentDataTable.Rows)
//					{
//						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
//						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
//						int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
//						securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, true);
//					}
//					++securityInheritanceOffset;
//				}
				// get security for groups
				foreach (eSecurityFunctions securityFunction in functions)
				{
					DataTable userFunctionAssignmentDataTable = _secAdmin.GetUserGroupsFunctionAssignment(userRID, securityFunction);
					// Begin TT#4302 - stodd - "Batch Mode Only" Security Issue when User in multiple groups
                    // for AdminBatchOnlyMode only it looks at the record for the Administrators group ONLY if that group is present.
                    if (securityFunction == eSecurityFunctions.AdminBatchOnlyMode)
                    {
                        DataRow[] rows = userFunctionAssignmentDataTable.Select("GROUP_RID = 1");
                        if (rows.Length == 1)
                        {
                            DataRow[] deleteRows = userFunctionAssignmentDataTable.Select("GROUP_RID <> 1");
                            foreach (DataRow aRow in deleteRows)
                            {
                                userFunctionAssignmentDataTable.Rows.Remove(aRow);
                            }
                        }
                    }
					// End TT#4302 - stodd - "Batch Mode Only" Security Issue when User in multiple groups
			
					foreach (DataRow dr in userFunctionAssignmentDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
						int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
                        // Begin Track #5858 - JSmith - Validating store security only
                        //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, true);
                        securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, true, eDatabaseSecurityTypes.NotSpecified);
                        // End Track #5858
					}
					++securityInheritanceOffset;
				}

				// get security for user
				foreach (eSecurityFunctions securityFunction in functions)
				{
					DataTable userFunctionAssignmentDataTable = _secAdmin.GetUserFunctionAssignment(userRID, securityFunction);
			
					foreach (DataRow dr in userFunctionAssignmentDataTable.Rows)
					{
						eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
                        // Begin Track #5858 - JSmith - Validating store security only
                        //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false);
                        securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.Function, Convert.ToInt32(securityFunction, CultureInfo.CurrentUICulture), securityInheritanceOffset, aGetInheritancePath, false, eDatabaseSecurityTypes.NotSpecified);
                        // End Track #5858
					}
					++securityInheritanceOffset;
				}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				return securityProfile;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetGroupNodeAssignment(SessionAddressBlock aSAB, int userRID, int nodeRID, int aSecurityType)
		{
			try
			{
				return GetGroupNodeAssignment(aSAB, userRID, nodeRID, -1, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetGroupNodeAssignment(SessionAddressBlock aSAB, int userRID, int nodeRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				return GetGroupNodeAssignment(aSAB, userRID, nodeRID, -1, aSecurityType, aGetInheritancePath);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetGroupNodeAssignment(SessionAddressBlock aSAB, int groupRID, int nodeRID, int hierarchyRID, int aSecurityType)
		{
			try
			{
				return GetGroupNodeAssignment(aSAB, groupRID, nodeRID, hierarchyRID, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetGroupNodeAssignment(SessionAddressBlock aSAB, int groupRID, int nodeRID, int hierarchyRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				int securityInheritanceOffset = 0;
				HierarchyNodeSecurityProfile securityProfile = new HierarchyNodeSecurityProfile(nodeRID);
				ArrayList databaseSecurityTypes = GetSecurityDatabaseTypes(aSecurityType);

				// get the node ancestor list, so that we can chase up the tree
				NodeAncestorList nal;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				if (hierarchyRID == -1)		// will chase the "home" hierarchy
//				{
//					nal = aSAB.HierarchyServerSession.GetNodeAncestorList(nodeRID);
//				}
//				else						// will chase the specified hierarchy
//				{
//					nal = aSAB.HierarchyServerSession.GetNodeAncestorList(nodeRID, hierarchyRID);
//				}
				nal = GetHierarchyPath(aSAB, nodeRID, hierarchyRID);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				DataTable merchandiseActionsDataTable = _secAdmin.GetActionsForMerchandise();

				// default security so actions will be known
				foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
				{
					eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					securityProfile.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode, nodeRID, securityInheritanceOffset, aGetInheritancePath, false);
					securityProfile.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				}

				// if need inheritance path, create a record for each action in the merchandise path
				if (aGetInheritancePath)
				{
					foreach (NodeAncestorProfile nap in nal)
					{
//						DataTable merchandiseActionsDataTable = _secAdmin.GetActionsForMerchandise();
						foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
							securityProfile.SetInheritancePath(action, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode, nap.Key, eSecurityLevel.NotSpecified, false);
						}
					}
				}

				// determine security for group
				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					foreach (NodeAncestorProfile nap in nal)
					{
						DataTable groupNodesAssignmentDataTable = _secAdmin.GetGroupNodeAssignment(groupRID, nap.Key, databaseSecurityType);

						foreach (DataRow dr in groupNodesAssignmentDataTable.Rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
							eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
                            // Begin Track #5858 - JSmith - Validating store security only
                            //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode,nap.Key, securityInheritanceOffset, aGetInheritancePath, false);
                            securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode, nap.Key, securityInheritanceOffset, aGetInheritancePath, false, databaseSecurityType);
                            // End Track #5858
						}
						// Begin MID Track# 2553 - corrected security chase for group nodes.
						++securityInheritanceOffset;
						// End MID Track# 2553
					}
				}
				return securityProfile;
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetUserNodeAssignment(SessionAddressBlock aSAB, int userRID, int nodeRID, int aSecurityType)
		{
			try
			{
				return GetUserNodeAssignment(aSAB, userRID, nodeRID, -1, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetUserNodeAssignment(SessionAddressBlock aSAB, int userRID, int nodeRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				return GetUserNodeAssignment(aSAB, userRID, nodeRID, -1, aSecurityType, aGetInheritancePath);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetUserNodeAssignment(SessionAddressBlock aSAB, int userRID, int nodeRID, int hierarchyRID, int aSecurityType)
		{
			try
			{
				return GetUserNodeAssignment(aSAB, userRID, nodeRID, hierarchyRID, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetUserNodeAssignment(SessionAddressBlock aSAB, int userRID, int nodeRID, int hierarchyRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				int securityInheritanceOffset = 0;
				HierarchyNodeSecurityProfile securityProfile = new HierarchyNodeSecurityProfile(nodeRID);

				DataTable merchandiseActionsDataTable = _secAdmin.GetActionsForMerchandise();

				// default security so actions will be known
				foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
				{
					eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					securityProfile.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode, nodeRID, securityInheritanceOffset, aGetInheritancePath, false);
					securityProfile.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				}

				if (userRID == Include.AdministratorUserRID || userRID == Include.SystemUserRID || userRID == Include.GlobalUserRID)
				{
					securityProfile.SetFullControl();
					return securityProfile;
				}

				ArrayList databaseSecurityTypes = GetSecurityDatabaseTypes(aSecurityType);

				// get the node ancestor list, so that we can chase up the tree
				NodeAncestorList nal;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				if (hierarchyRID == -1)		// will chase the "home" hierarchy
//				{
//					nal = aSAB.HierarchyServerSession.GetNodeAncestorList(nodeRID);
//				}
//				else						// will chase the specified hierarchy
//				{
//					nal = aSAB.HierarchyServerSession.GetNodeAncestorList(nodeRID, hierarchyRID);
//				}
				nal = GetHierarchyPath(aSAB, nodeRID, hierarchyRID);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

				// if need inheritance path, create a record for each action in the merchandise path
				if (aGetInheritancePath)
				{
					foreach (NodeAncestorProfile nap in nal)
					{
						foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
							securityProfile.SetInheritancePath(action, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode, nap.Key, eSecurityLevel.NotSpecified, false);
						}
					}
					// add each group user is in to inheritance path
					DataTable userGroupsDataTable = _secAdmin.GetGroups(userRID);
					foreach (NodeAncestorProfile nap in nal)
					{
						foreach (DataRow groupRow in userGroupsDataTable.Rows)
						{
							int groupRID = Convert.ToInt32(groupRow["GROUP_RID"], CultureInfo.CurrentUICulture);
							foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
							{
								eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
								securityProfile.SetInheritancePath(action, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode, nap.Key, eSecurityLevel.NotSpecified, false);
							}
						}
					}
				}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				// get security for user
//				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
//				{
//					DataTable userNodesAssignmentDataTable = _secAdmin.GetUserNodesAssignment(userRID, databaseSecurityType);
//
//					// chase the tree.
//					foreach (NodeAncestorProfile nap in nal)
//					{
//						DataRow [] rows = userNodesAssignmentDataTable.Select("HN_RID = " + nap.Key.ToString(CultureInfo.CurrentUICulture));
//						foreach (DataRow dr in rows)
//						{
//							eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
//							eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
//							securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode,nap.Key, securityInheritanceOffset, aGetInheritancePath, false);
//						}
//						++securityInheritanceOffset;
//					}
//				}
//				// get security for groups
//				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
//				{
//					DataTable userNodesAssignmentDataTable = _secAdmin.GetUserGroupsNodesAssignment(userRID, databaseSecurityType);
//
//					// chase the tree.
//					foreach (NodeAncestorProfile nap in nal)
//					{
//						DataRow [] rows = userNodesAssignmentDataTable.Select("HN_RID = " + nap.Key.ToString(CultureInfo.CurrentUICulture));
//						foreach (DataRow dr in rows)
//						{
//							eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
//							eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
//							int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
//							securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode,nap.Key, securityInheritanceOffset, aGetInheritancePath, true);
//						}
//						++securityInheritanceOffset;
//					}
//				}
				// get security for groups
				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable userNodesAssignmentDataTable = _secAdmin.GetUserGroupsNodesAssignment(userRID, databaseSecurityType);

					// chase the tree.
					foreach (NodeAncestorProfile nap in nal)
					{
						DataRow [] rows = userNodesAssignmentDataTable.Select("HN_RID = " + nap.Key.ToString(CultureInfo.CurrentUICulture));
						foreach (DataRow dr in rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
							eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
							int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
                            // Begin Track #5858 - JSmith - Validating store security only
                            //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode,nap.Key, securityInheritanceOffset, aGetInheritancePath, true);
                            securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.Group, groupRID, eSecurityInheritanceTypes.HierarchyNode, nap.Key, securityInheritanceOffset, aGetInheritancePath, true, databaseSecurityType);
                            // End Track #5858
						}
						++securityInheritanceOffset;
					}
				}

				// get security for user
				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable userNodesAssignmentDataTable = _secAdmin.GetUserNodesAssignment(userRID, databaseSecurityType);

					// chase the tree.
					foreach (NodeAncestorProfile nap in nal)
					{
						DataRow [] rows = userNodesAssignmentDataTable.Select("HN_RID = " + nap.Key.ToString(CultureInfo.CurrentUICulture));
						foreach (DataRow dr in rows)
						{
							eSecurityActions action = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
							eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
                            // Begin Track #5858 - JSmith - Validating store security only
                            //securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode,nap.Key, securityInheritanceOffset, aGetInheritancePath, false);
                            securityProfile.SetSecurity(action, securityLevel, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode, nap.Key, securityInheritanceOffset, aGetInheritancePath, false, databaseSecurityType);
                            // End Track #5858
						}
						++securityInheritanceOffset;
					}
				}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				return securityProfile;
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfileList GetUserNodesAssignment(int userRID, int aSecurityType)
		{
			try
			{
				return GetUserNodesAssignment(userRID, aSecurityType, false);
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Given a user RID, this method returns a profile list of HierarchyNodeSecurityProfiles
		/// </summary>
		/// <param name="userRID"></param>
		/// <returns>ProfileList of HierarchyNodeSecurityProfiles</returns>
		public HierarchyNodeSecurityProfileList GetUserNodesAssignment(int userRID, int aSecurityType, bool aGetInheritancePath)
		{
			try
			{
				ArrayList databaseSecurityTypes = GetSecurityDatabaseTypes(aSecurityType);

				DataTable merchandiseActionsDataTable = _secAdmin.GetActionsForMerchandise();

				HierarchyNodeSecurityProfileList al = null;
				HierarchyNodeSecurityProfile hnsp = null;
				bool addNew = false;
				// get security for user
                // Begin Track #5858 - JSmith- Validating store security only
                al = new HierarchyNodeSecurityProfileList(eProfileType.SecurityHierarchyNode);
                // End Track #5858

				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable userNodesAssignmentDataTable = _secAdmin.GetUserNodesAssignment(userRID, databaseSecurityType);

                    // Begin Track #5858 - JSmith- Validating store security only
                    //al = new HierarchyNodeSecurityProfileList(eProfileType.SecurityHierarchyNode);
                    // End Track #5858
					foreach (DataRow dr in userNodesAssignmentDataTable.Rows)
					{
						int hnRid = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						if (al.Contains(hnRid))
						{
							hnsp = (HierarchyNodeSecurityProfile)al.FindKey(hnRid); 
							addNew = false;
						}
						else
						{
							hnsp = new HierarchyNodeSecurityProfile(hnRid);
							addNew = true;
							// default security so actions will be known
							foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
							{
								eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//								hnsp.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode, hnRid, 0, aGetInheritancePath, false);
								hnsp.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
							}
						}
						eSecurityActions actionID = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
						// Begin Track #5892 - JSmith - Security set to view for Merchandise and Hierarchies, but no hierarchies show up.
                        //hnsp.SetSecurity(actionID, securityLevel);
                        hnsp.SetSecurity(actionID, securityLevel, databaseSecurityType);
                        // End Track #5892

						if (addNew)
						{
							al.Add(hnsp);
						}
					}
				}

				// get security for groups
				foreach (eDatabaseSecurityTypes databaseSecurityType in databaseSecurityTypes)
				{
					DataTable userNodesAssignmentDataTable = _secAdmin.GetUserGroupsNodesAssignment(userRID, databaseSecurityType);

					foreach (DataRow dr in userNodesAssignmentDataTable.Rows)
					{
						int hnRid = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						if (al.Contains(hnRid))
						{
							hnsp = (HierarchyNodeSecurityProfile)al.FindKey(hnRid);
							addNew = false;
						}
						else
						{
							hnsp = new HierarchyNodeSecurityProfile(hnRid);
							addNew = true;
							// default security so actions will be known
							foreach (DataRow actionRow in merchandiseActionsDataTable.Rows)
							{
								eSecurityActions action = (eSecurityActions)Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//								hnsp.SetSecurity(action, eSecurityLevel.Initialize, eSecurityOwnerType.User, userRID, eSecurityInheritanceTypes.HierarchyNode, hnRid, 0, aGetInheritancePath, false);
								hnsp.AddSecurityAction(action);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
							}
						}
						eSecurityActions actionID = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
						eSecurityLevel securityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);
                        // Begin Track #5892 - JSmith - Security set to view for Merchandise and Hierarchies, but no hierarchies show up.
                        //hnsp.SetSecurity(actionID, securityLevel);
                        hnsp.SetSecurity(actionID, securityLevel, databaseSecurityType);
                        // End Track #5892
						if (addNew)
						{
							al.Add(hnsp);
						}
					}
				}
			
				return al;
			}
			catch
			{
				throw;
			}
		}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		public DataTable GetActionsForFunction(int aFuncID)
		{
			DataTable dtTempActions;
			DataTable dtActions;
			DataTable dtChildActions;
			DataTable dtChildren;

			try
			{
				dtActions = (DataTable)_functionActionHash[aFuncID];

				if (dtActions == null)
				{
					dtTempActions = _secAdmin.GetActionsForFunction(aFuncID);
					dtActions = dtTempActions.Clone();
					dtActions.PrimaryKey = new DataColumn[] { dtActions.Columns["ACTION_ID"] };

					foreach (DataRow row in dtTempActions.Rows)
					{
						if (!dtActions.Rows.Contains(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)))
						{
							dtActions.Rows.Add(row.ItemArray);
						}
					}

					dtChildren = _secAdmin.GetChildrenFunctions((eSecurityFunctions)aFuncID);

					foreach (DataRow childRow in dtChildren.Rows)
					{
						dtChildActions = GetActionsForFunction(Convert.ToInt32(childRow["FUNC_ID"], CultureInfo.CurrentUICulture));

						foreach (DataRow actionRow in dtChildActions.Rows)
						{
							if (!dtActions.Rows.Contains(Convert.ToInt32(actionRow["ACTION_ID"], CultureInfo.CurrentUICulture)))
							{
								dtActions.Rows.Add(actionRow.ItemArray);
							}
						}
					}

					dtActions.AcceptChanges();

					_functionActionHash[aFuncID] = dtActions;
				}

				return dtActions;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		private ArrayList GetSecurityDatabaseTypes(int aSecurityType)
		{
			try
			{
				ArrayList al = new ArrayList();

				if (System.Convert.ToBoolean(aSecurityType & (int)eSecurityTypes.Allocation, CultureInfo.CurrentUICulture))
				{
					al.Add(eDatabaseSecurityTypes.Allocation);
				}

				if (System.Convert.ToBoolean(aSecurityType & (int)(int)eSecurityTypes.Chain, CultureInfo.CurrentUICulture))
				{
					al.Add(eDatabaseSecurityTypes.Chain);
				}

				if (System.Convert.ToBoolean(aSecurityType & (int)eSecurityTypes.Store, CultureInfo.CurrentUICulture))
				{
					al.Add(eDatabaseSecurityTypes.Store);
				}

				return al;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Builds an ArrayList of eSecurityFunctions function IDs from the provided function to the root function
		/// </summary>
		/// <param name="aFuncID">The eSecurityFunctions of the function</param>
		/// <returns>ArrayList of eSecurityFunctions function IDs</returns>
		private ArrayList GetFunctionPath (eSecurityFunctions aFuncID)
		{
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
			Stack funcStack;

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			try
			{
				if (_functionsDataTable == null)
				{
					_functionsDataTable = _secAdmin.GetSecurityFunctions();
				}

				ArrayList funcPath = null;
				if (_functionPath.Contains(aFuncID))
				{
					funcPath = (ArrayList)_functionPath[aFuncID];
				}
				else
				{
					int funcID = Convert.ToInt32(aFuncID, CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					funcPath = new ArrayList();
					funcStack = new Stack();
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				
					while (funcID != 0)
					{
						// add function
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//						funcPath.Add((eSecurityFunctions)funcID);
						funcStack.Push((eSecurityFunctions)funcID);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

						// get next parent function
						DataRow [] rows = _functionsDataTable.Select("FUNC_ID = " + funcID.ToString(CultureInfo.CurrentUICulture));
						if (rows != null)
						{
							DataRow dr = rows[0];
							funcID = Convert.ToInt32(dr["FUNC_PARENT"], CultureInfo.CurrentUICulture);
						}
					}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
					funcPath = new ArrayList();

					while (funcStack.Count > 0)
					{
						funcPath.Add(funcStack.Pop());
					}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
					_functionPath.Add(aFuncID, funcPath);
				}

				return funcPath;
			}
			catch
			{
				throw;
			}
		}
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes

		/// <summary>
		/// Builds an ArrayList of eSecurityFunctions function IDs from the provided function to the root function
		/// </summary>
		/// <returns>ArrayList of eSecurityFunctions function IDs</returns>
		private NodeAncestorList GetHierarchyPath (SessionAddressBlock aSAB, int aNodeRID, int aHierarchyRID)
		{
			NodeAncestorList nal;
			NodeAncestorList outNal;
			Stack nalStack;

			try
			{
				if (aHierarchyRID == -1)
				{
                    // Begin TT#5556 - JSmith - Global Interfaced Article List Edit Button Removed from Node Properties
					//nal = aSAB.HierarchyServerSession.GetNodeAncestorList(aNodeRID);
                    nal = aSAB.HierarchyServerSession.GetNodeAncestorList(aNodeRID, Include.NoRID, eHierarchySearchType.HomeHierarchyOnly, false);
                    // End TT#5556 - JSmith - Global Interfaced Article List Edit Button Removed from Node Properties
				}
				else
				{
                    // Begin TT#5556 - JSmith - Global Interfaced Article List Edit Button Removed from Node Properties
					//nal = aSAB.HierarchyServerSession.GetNodeAncestorList(aNodeRID, aHierarchyRID);
                    nal = aSAB.HierarchyServerSession.GetNodeAncestorList(aNodeRID, aHierarchyRID, eHierarchySearchType.HomeHierarchyOnly, false);
                    // End TT#5556 - JSmith - Global Interfaced Article List Edit Button Removed from Node Properties
				}

				nalStack = new Stack();

				foreach (NodeAncestorProfile nap in nal)
				{
					nalStack.Push(nap);
				}

				outNal = new NodeAncestorList(eProfileType.HierarchyNodeAncestor);

				while (nalStack.Count > 0)
				{
					outNal.Add((NodeAncestorProfile)nalStack.Pop());
				}

				return outNal;
			}
			catch
			{
				throw;
			}
		}
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
	}


}
