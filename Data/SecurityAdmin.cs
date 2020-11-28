using System;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	public partial class SecurityAdmin : DataLayer
	{

		int _userRID;
		string _userName;
		string _userDescription;
		bool _activeInd;

		public SecurityAdmin()
		{
			_userRID = 0;
			_userName = "";
			_userDescription = "";
			_activeInd = false;
		}
		//Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
		public SecurityAdmin(DatabaseAccess dba)
        {
            _dba = dba;
            _userRID = 0;
            _userName = "";
            _userDescription = "";
            _activeInd = false;
        }
		//End TT#1564 - DOConnell - Missing Tasklist record prevents Login
		public int UserRID
		{
			get { return _userRID ; }
			set { _userRID = value; }
		}
		public string UserName
		{
			get { return _userName ; }
		}
		public string UserDescription
		{
			get { return _userDescription ; }
		}
		public bool ActiveInd
		{
			get { return _activeInd ; }
		}


		// return value will be stored in a password field(varchar) or will be compared against the value in sql.
		private string EncryptPassword(string password)
		{
			byte[] passwordToHash = (new UnicodeEncoding()).GetBytes(password);
			byte[] passwordHashValue = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passwordToHash);
			return BitConverter.ToString(passwordHashValue);
		}
        //Begin TT#898-MD -jsobek -Single User Instance System Option
        //public bool IsUserValid(string userName)
        //{
        //    DataTable dt = StoredProcedures.MID_APPLICATION_USER_READ_FROM_NAME.Read(_dba, USER_NAME: userName);
        //    if (dt.Rows.Count > 0)
        //    {
        //        if ((string)dt.Rows[0]["USER_ACTIVE_IND"] == "1" && !Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["USER_DELETE_IND"])))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public bool IsUserAlreadySignedOn(string userName, out string loginDate, out string loginComputerName)
        {
            bool alreadySignedIn = false;
            loginDate = string.Empty;
            loginComputerName = string.Empty;
            DataTable dt = StoredProcedures.MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME.Read(_dba, USER_NAME: userName);
            if (dt.Rows.Count > 0)
            {
                alreadySignedIn = true;
                if (dt.Rows[0]["LOGIN_DATETIME"] != DBNull.Value)
                {
                    DateTime dateLogIn = (DateTime)dt.Rows[0]["LOGIN_DATETIME"];
                    loginDate = dateLogIn.ToShortDateString() + " " + dateLogIn.ToShortTimeString();
                }
                if (dt.Rows[0]["COMPUTER_NAME"] != DBNull.Value)
                {
                    loginComputerName = (string)dt.Rows[0]["COMPUTER_NAME"];
                }
            }
            return alreadySignedIn;
        }
        //End TT#898-MD -jsobek -Single User Instance System Option
        //Begin TT#901-MD -jsobek -Batch Only Mode
        public void GetControlServiceStartOptions(out bool useBatchOnlyMode, out bool startWithBatchOnlyModeOn)
        {
            useBatchOnlyMode = false;
            startWithBatchOnlyModeOn = false;

            DataTable dt = StoredProcedures.MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS.Read(_dba);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["USE_BATCH_ONLY_MODE"] != DBNull.Value)
                {
                    string tmpUseBatchOnlyMode = (string)dt.Rows[0]["USE_BATCH_ONLY_MODE"];
                    if (tmpUseBatchOnlyMode == "1")
                    {
                        useBatchOnlyMode = true;
                    }

                }
                if (dt.Rows[0]["CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON"] != DBNull.Value)
                {
                    string tmpStartWithBatchOnlyModeOn = (string)dt.Rows[0]["CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON"];
                    if (tmpStartWithBatchOnlyModeOn == "1")
                    {
                        startWithBatchOnlyModeOn = true;
                    }
                }
            }

        }
        public bool UseBatchOnlyMode()
        {
            bool useBatchOnlyMode = false;

            DataTable dt = StoredProcedures.MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS.Read(_dba);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["USE_BATCH_ONLY_MODE"] != DBNull.Value)
                {
                    string tmpUseBatchOnlyMode = (string)dt.Rows[0]["USE_BATCH_ONLY_MODE"];
                    if (tmpUseBatchOnlyMode == "1")
                    {
                        useBatchOnlyMode = true;
                    }

                }
           
            }
            return useBatchOnlyMode;
        }
        //End TT#901-MD -jsobek -Batch Only Mode


		public eSecurityAuthenticate AuthenticateUser(string userName, string userPassword)
		{
			try
			{
				string encryptedPassword = null;
				string databasePassword = null;
				// encrypt password, then compare against password stored in database
				if (userPassword.Length > 0)
				{
					encryptedPassword = EncryptPassword(userPassword);
				}


                //MID Track # 2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_APPLICATION_USER_READ_FROM_NAME.Read(_dba, USER_NAME: userName);


				foreach(DataRow dr in dt.Rows)
				{
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//					if ((string)dr["USER_ACTIVE_IND"] == "1") 
					if ((string)dr["USER_ACTIVE_IND"] == "1" &&
						!Include.ConvertCharToBool(Convert.ToChar(dr["USER_DELETE_IND"]))) 
					//End Track #4815
					{
						if ((dr["USER_PASSWORD"] == System.DBNull.Value || Convert.ToString(dr["USER_PASSWORD"], CultureInfo.CurrentUICulture) == ""))
						{
							databasePassword = null;
						}
						else
						{
							databasePassword = Convert.ToString(dr["USER_PASSWORD"], CultureInfo.CurrentUICulture);
						}

						if (encryptedPassword == databasePassword)
						{
							_userRID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
							return eSecurityAuthenticate.UserAuthenticated;
						}
						else
						{
							return eSecurityAuthenticate.IncorrectPassword;
						}
					}
					else
					{
						return eSecurityAuthenticate.InactiveUser;
					}
				}
// BEGIN Track #3650 - JSmith - Login security
//				return eSecurityAuthenticate.UnknownUser;
				return CheckAllUsers(userName, encryptedPassword);
// End Track #3650
			}
			catch ( InvalidOperationException )
			{
				return eSecurityAuthenticate.UnknownUser;
			}
		}
// BEGIN Track #3650 - JSmith - Login security
		/// <summary>
		/// Checks all users with case insensitivity.
		/// </summary>
		/// <param name="aUserName"></param>
		/// <param name="aUserPassword"></param>
		/// <returns></returns>
		private eSecurityAuthenticate CheckAllUsers(string aUserName, string aUserPassword)
		{
			try
			{
				string databasePassword = null;
				string upperUserName = aUserName.Trim().ToUpper();

                DataTable dt = StoredProcedures.MID_APPLICATION_USER_READ_ALL.Read(_dba);
				
				foreach(DataRow dr in dt.Rows)
				{
					if(upperUserName == (Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentUICulture)).Trim().ToUpper())
					{
						if ((string)dr["USER_ACTIVE_IND"] == "1") 
						{
							if ((dr["USER_PASSWORD"] == System.DBNull.Value || Convert.ToString(dr["USER_PASSWORD"], CultureInfo.CurrentUICulture) == ""))
							{
								databasePassword = null;
							}
							else
							{
								databasePassword = Convert.ToString(dr["USER_PASSWORD"], CultureInfo.CurrentUICulture);
							}

							if (aUserPassword == databasePassword)
							{
								_userRID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
								return eSecurityAuthenticate.UserAuthenticated;
							}
							else
							{
								return eSecurityAuthenticate.IncorrectPassword;
							}
						}
						else
						{
							return eSecurityAuthenticate.InactiveUser;
						}
					}
				}
				return eSecurityAuthenticate.UnknownUser;
			}
			catch ( InvalidOperationException )
			{
				return eSecurityAuthenticate.UnknownUser;
			}
		}
// END Track #3650

		public eSecurityAuthenticate AuthenticateUser(int aUserRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_APPLICATION_USER_READ_FROM_RID.Read(_dba, USER_RID: aUserRID);

				foreach(DataRow dr in dt.Rows)
				{
					if ((string)dr["USER_ACTIVE_IND"] == "1") 
					{
						_userRID = aUserRID;
						return eSecurityAuthenticate.UserAuthenticated;
					}
					else
					{
						return eSecurityAuthenticate.InactiveUser;
					}
				}

				return eSecurityAuthenticate.UnknownUser;
			}
			catch ( InvalidOperationException )
			{
				return eSecurityAuthenticate.UnknownUser;
			}
		}

		public eSecurityAuthenticate SetUserPassword(string userName, string currentPassword, string newPassword,
			bool aAuthenticateUser)
		{
			try
			{
				eSecurityAuthenticate retVal = eSecurityAuthenticate.UserAuthenticated;
				if (aAuthenticateUser)
				{
					retVal = AuthenticateUser(userName, currentPassword);
				}
				if (retVal == eSecurityAuthenticate.UserAuthenticated) 
				{
					// encrypt password, store in database
					string encryptedPassword = null;
					if (newPassword != null && newPassword.Length > 0)
					{
						encryptedPassword = EncryptPassword(newPassword);
					}

					// Update
                    StoredProcedures.MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME.Update(_dba,
                                                                                  USER_NAME: userName,
                                                                                  USER_PASSWORD: encryptedPassword
                                                                                  );
					return(eSecurityAuthenticate.PasswordChanged);
				}
				else
				{
					return(retVal);
				}
			}
			catch 
			{
				throw;
			}
		}


		private eValidUsername ValidateUsername(string userName)
		{

			return (eValidUsername.Valid);
		}

		private eValidPassword ValidatePassword(string password)
		{

			return (eValidPassword.Valid);
		}

		/// <summary>
		/// Create new user, returning the RID of the created user.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userPassword"></param>
		/// <param name="userDescription"></param>
		/// <param name="userActivation"></param>
		/// <returns></returns>
		public int CreateUser(string userName, string userPassword, string userFullname, string userDescription, eSecurityActivation userActivation)
		{
			try
			{
				// Make sure username is valid
				eValidUsername validUsernameRetVal = ValidateUsername( userName );
				if (validUsernameRetVal != eValidUsername.Valid)
				{
					throw new Exception(MIDText.GetText((eMIDTextCode)validUsernameRetVal));
				}

				// Make sure password is valid
				eValidPassword validPasswordRetVal = ValidatePassword( userPassword );
				if (validPasswordRetVal != eValidPassword.Valid)
				{
					throw new Exception(MIDText.GetText((eMIDTextCode)validPasswordRetVal));
				}

                //string strUserActiveInd;
                //if (userActivation == eSecurityActivation.Activate)
                //    strUserActiveInd = "1";
                //else
                //    strUserActiveInd = "0";

                char userActiveInd;
                if (userActivation == eSecurityActivation.Activate)
                    userActiveInd = '1';
                else
                    userActiveInd = '0';

				string encryptedPassword = null;
				if (userPassword != null && userPassword.Length > 0)
				{
					encryptedPassword = EncryptPassword(userPassword);
				}

                int user_RID = StoredProcedures.SP_MID_USER_INSERT.InsertAndReturnRID(_dba,
                                                                                    USER_NAME: userName,
                                                                                    USER_PASSWORD: encryptedPassword,
                                                                                    USER_FULLNAME: userFullname,
                                                                                    USER_DESCRIPTION: userDescription,
                                                                                    USER_ACTIVE_IND: userActiveInd
                                                                                    );

				InsertUserOptions (user_RID, Include.MyHierarchyName, Include.MIDDefault);
				return user_RID;

			}
			catch 
			{
				if (AuthenticateUser(userName, "") != eSecurityAuthenticate.UnknownUser)
				{
					throw new Exception(MIDText.GetText((eMIDTextCode)eValidUsername.InvalidAlreadyExists));
				}
				else
				{
					throw;
				}
			}
		}

        /// <summary>
        /// Create new session for user.
        /// </summary>
        /// <param name="aUserRID"></param>
        /// <returns>The RID of the created session</returns>
        public int CreateUserSession(int aUserRID, string aComputerName)
        {
            try
            {
                int session_RID = StoredProcedures.SP_MID_USER_SESSION_INSERT.InsertAndReturnRID(_dba,
                                                                                              USER_RID: aUserRID,
                                                                                              SESSION_STATUS: (int)eSessionStatus.LoggedIn,
                                                                                              LOGIN_DATETIME: DateTime.Now,
                                                                                              COMPUTER_NAME: aComputerName
                                                                                              );

                return session_RID;

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update session for user.
        /// </summary>
        /// <param name="aSessionRID">The key of the session</param>
        /// <param name="aSessionStatus">The status of the session</param>
        public void UpdateUserSession(int aSessionRID, eSessionStatus aSessionStatus)
        {
            try
            {
                StoredProcedures.MID_USER_SESSION_UPDATE_STATUS.Update(_dba,
                                                                       SESSION_RID: aSessionRID,
                                                                       SESSION_STATUS: (int)aSessionStatus
                                                                       );

            }
            catch
            {
                throw;
            }
        }

		//Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
		/// <summary>
		/// Returns the session row for a given User with the given eSesionStatus.
		/// </summary>
		/// <param name="aUserRID">The key of the User</param>
		/// <param name="aStatus">The status to select</param>
		public DataTable GetUserSession(int aUserRID, eSessionStatus aStatus)
		{
			try
			{
                return StoredProcedures.MID_USER_SESSION_READ.Read(_dba,
                                                                   USER_RID: aUserRID,
                                                                   SESSION_STATUS: (int)aStatus
                                                                   );
			}
			catch
			{
				throw;
			}
		}

		//End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
		/// <summary>
        /// Close the session for user.
        /// </summary>
        /// <param name="aSessionRID">The key of the session</param>
        public void CloseUserSession(int aSessionRID)
        {
            try
            {
                StoredProcedures.MID_USER_SESSION_UPDATE_FOR_CLOSE.Update(_dba,
                                                                          SESSION_RID: aSessionRID,
                                                                          SESSION_STATUS: (int)eSessionStatus.LoggedOut,
                                                                          LOGOUT_DATETIME: DateTime.Now
                                                                          );
            }
            catch
            {
                throw;
            }
        }

		//Begin TT#1206 - JScott - Unable to Assign a User to Another User
		/// <summary>
		/// Close the session for all users.
		/// </summary>
		public void CloseAllUserSessions()
		{
			try
			{
                StoredProcedures.MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE.Update(_dba, LOGOUT_DATETIME: DateTime.Now);
			}
			catch
			{
				throw;
			}
		}

		//End TT#1206 - JScott - Unable to Assign a User to Another User
		//Begin TT#755 - JScott - Constraint error encountered during Purge
		/// <summary>
		/// Deletes all User Session rows for a given user.
		/// </summary>
		/// <param name="aUserRID">The user to remove</param>
		public void DeleteAllUserSession(int aUserRID)
		{
			try
			{
                StoredProcedures.MID_USER_SESSION_DELETE_FROM_USER.Delete(_dba, USER_RID: aUserRID);
			}
			catch
			{
				throw;
			}
		}

		//End TT#755 - JScott - Constraint error encountered during Purge
		private eValidGroupname ValidateGroupname(string userName)
		{

			return (eValidGroupname.Valid);
		}

		public int CreateGroup(string groupName, string groupDescription, eSecurityActivation groupActivation)
		{
			try
			{
				// Make sure groupname is valid
				eValidGroupname validGroupnameRetVal = ValidateGroupname( groupName );
				if (validGroupnameRetVal != eValidGroupname.Valid)
				{
					throw new Exception(MIDText.GetText((eMIDTextCode)validGroupnameRetVal));
				}

                //string strGroupActiveInd;
                //if (groupActivation == eSecurityActivation.Activate)
                //    strGroupActiveInd = "1";
                //else
                //    strGroupActiveInd = "0";

                char groupActiveInd;
                if (groupActivation == eSecurityActivation.Activate)
                    groupActiveInd = '1';
                else
                    groupActiveInd = '0';

                return StoredProcedures.SP_MID_GROUP_INSERT.InsertAndReturnRID(_dba,
                                                                             GROUP_NAME: groupName,
                                                                             GROUP_DESCRIPTION: groupDescription,
                                                                             GROUP_ACTIVE_IND: groupActiveInd
                                                                             );
			}
			catch
			{
				// see if group already exists
				DataTable dt = GetGroup(groupName);
				if (dt.Rows.Count > 0)
				{
					throw new Exception(MIDText.GetText((eMIDTextCode)eValidGroupname.InvalidAlreadyExists));
				}
				else
				{
					throw;
				}
			}
		}



		/// <summary>
		/// Update application users.  Null input parameters indicate "no change" in this field; 
		/// use "eSecurityActivation.NoChange" to leave USER_ACTIVE_IND unchanged.
		/// </summary>
		/// <param name="userRID"></param>
		/// <param name="userName"></param>
		/// <param name="userPassword"></param>
		/// <param name="userDescription"></param>
		/// <param name="userActivation"></param>
		/// <returns></returns>
		public bool UpdateUser(int userRID, string userName, string userPassword, string userFullname, string userDescription, eSecurityActivation userActivation)
		{
			try
			{
                


                char? userActiveInd = null;
                if (userActivation == eSecurityActivation.Activate)
                    userActiveInd = '1';
                else if (userActivation == eSecurityActivation.Deactivate)
                    userActiveInd = '0';

                //validate that at least one field is being updated
                if (userName == null && userPassword == null && userFullname == null && userDescription == null && userActiveInd == null)
                {
                    return false;
                }

                int rowsUpdated = 0;
                if (userName != null)
                {
                    rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_NAME.Update(_dba,
                                                                                           USER_RID: userRID,
                                                                                           USER_NAME: userName
                                                                                           );
                }
                if (userPassword != null)
                {
                    rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_PWD.Update(_dba,
                                                                                          USER_RID: userRID,
                                                                                          USER_PASSWORD: EncryptPassword(userPassword)
                                                                                          );
                }
                if (userFullname != null)
                {
                    rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_FULL_NAME.Update(_dba,
                                                                                                USER_RID: userRID,
                                                                                                USER_FULLNAME: userFullname
                                                                                                );
                }
                if (userDescription != null)
                {
                    rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_DESCRIPTION.Update(_dba,
                                                                                                  USER_RID: userRID,
                                                                                                  USER_DESCRIPTION: userDescription
                                                                                                  );
                }
                if (userActiveInd != null)
                {
                    rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_ACTIVE_IND.Update(_dba,
                                                                                                 USER_RID: userRID,
                                                                                                 USER_ACTIVE_IND: userActiveInd
                                                                                                 );
                }
                return (rowsUpdated > 0);
			}
			catch
			{
				//Begin TT#200 - JScott - Security When changing a UserID, which happens to be a duplicate, received a database error.  Would expect a message telling me the UserID is a duplicate.
				//throw;
				if (AuthenticateUser(userName, "") != eSecurityAuthenticate.UnknownUser)
				{
					throw new Exception(MIDText.GetText((eMIDTextCode)eValidUsername.InvalidAlreadyExists));
				}
				else
				{
					throw;
				}
				//End TT#200 - JScott - Security When changing a UserID, which happens to be a duplicate, received a database error.  Would expect a message telling me the UserID is a duplicate.
			}
		}


	
		/// <summary>
		/// Update user group.  Null input parameters indicate "no change" in this field; 
		/// use "eSecurityActivation.NoChange" to leave GROUP_ACTIVE_IND unchanged.
		/// </summary>
		/// <param name="groupRID"></param>
		/// <param name="groupName"></param>
		/// <param name="groupDescription"></param>
		/// <param name="groupActivation"></param>
 		/// <returns></returns>
		public bool UpdateGroup(int groupRID, string groupName, string groupDescription, eSecurityActivation groupActivation)
		{
			try
			{
                char? groupActiveInd = null;
                if (groupActivation == eSecurityActivation.Activate)
                    groupActiveInd = '1';
                else if (groupActivation == eSecurityActivation.Deactivate)
                    groupActiveInd = '0';


                //validate that at least one field is being updated
                if (groupName == null && groupDescription == null && groupActiveInd == null)
                    return false;

                int rowsUpdated = 0;
                if (groupName != null)
                {
                    rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_NAME.Update(_dba,
                                                                                     GROUP_RID: groupRID,
                                                                                     GROUP_NAME: groupName
                                                                                     );
                }

                if (groupDescription != null)
                {
                    rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_DESCRIPTION.Update(_dba,
                                                                                            GROUP_RID: groupRID,
                                                                                            GROUP_DESCRIPTION: groupDescription
                                                                                            );
                }

                if (groupActiveInd != null)
                {
                    rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_ACTIVE_IND.Update(_dba,
                                                                                           GROUP_RID: groupRID,
                                                                                           GROUP_ACTIVE_IND: groupActiveInd
                                                                                           );
                }

                return (rowsUpdated > 0);
			}
			catch 
			{
				throw;
			}
		}

        
        //public void DeleteUserFilters(int userRID)
        //{
        //    try
        //    {
        //        StoredProcedures.SP_MID_USER_FILTERS_DELETE.Delete(_dba, UserRID: userRID);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public int DeleteUserFilters(int userRID, int aCommitLimit, DateTime aShutdownTime, out bool aAutomaticStopExceeded)
        {
            int recordsDeleted;
            int totalDeleted = 0;
            aAutomaticStopExceeded = false;
            try
            {
                recordsDeleted = 1;
                while (recordsDeleted > 0)
                {
                    if (DateTime.Now.TimeOfDay > aShutdownTime.TimeOfDay)
                    {
                        aAutomaticStopExceeded = true;
                    }
                    recordsDeleted = StoredProcedures.SP_MID_USER_FILTERS_DELETE.Delete(_dba, UserRID: userRID, COMMIT_LIMIT: aCommitLimit);
                    _dba.CommitData();
                    totalDeleted += recordsDeleted;
                }

                return totalDeleted;
            }
            catch
            {
                throw;
            }
        }

        //public void DeleteUserMethods(int userRID)
        //{
        //    try
        //    {
        //        StoredProcedures.SP_MID_USER_METHODS_DELETE.Delete(_dba, UserRID: userRID);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public int DeleteUserMethods(int userRID, int aCommitLimit, DateTime aShutdownTime, out bool aAutomaticStopExceeded)
        {
            int recordsDeleted;
            int totalDeleted = 0;
            aAutomaticStopExceeded = false;
            try
            {
                recordsDeleted = 1;
                while (recordsDeleted > 0)
                {
                    if (DateTime.Now.TimeOfDay > aShutdownTime.TimeOfDay)
                    {
                        aAutomaticStopExceeded = true;
                    }
                    recordsDeleted = StoredProcedures.SP_MID_USER_METHODS_DELETE.Delete(_dba, UserRID: userRID, COMMIT_LIMIT: aCommitLimit);
                    _dba.CommitData();
                    totalDeleted += recordsDeleted;
                }

                return totalDeleted;
            }
            catch
            {
                throw;
            }
        }

        //public void DeleteUserWorkflows(int userRID)
        //{
        //    try
        //    {
        //        StoredProcedures.SP_MID_USER_WORKFLOWS_DELETE.Delete(_dba, UserRID: userRID);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public int DeleteUserWorkflows(int userRID, int aCommitLimit, DateTime aShutdownTime, out bool aAutomaticStopExceeded)
        {
            int recordsDeleted;
            int totalDeleted = 0;
            aAutomaticStopExceeded = false;
            try
            {
                recordsDeleted = 1;
                while (recordsDeleted > 0)
                {
                    if (DateTime.Now.TimeOfDay > aShutdownTime.TimeOfDay)
                    {
                        aAutomaticStopExceeded = true;
                    }
                    recordsDeleted = StoredProcedures.SP_MID_USER_WORKFLOWS_DELETE.Delete(_dba, UserRID: userRID, COMMIT_LIMIT: aCommitLimit);
                    _dba.CommitData();
                    totalDeleted += recordsDeleted;
                }

                return totalDeleted;
            }
            catch
            {
                throw;
            }
        }

        //public void DeleteUserTasklists(int userRID)
        //{
        //    try
        //    {
        //        StoredProcedures.SP_MID_USER_TASKLISTS_DELETE.Delete(_dba, UserRID: userRID);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public int DeleteUserTasklists(int userRID, int aCommitLimit, DateTime aShutdownTime, out bool aAutomaticStopExceeded)
        {
            int recordsDeleted;
            int totalDeleted = 0;
            aAutomaticStopExceeded = false;
            try
            {
                recordsDeleted = 1;
                while (recordsDeleted > 0)
                {
                    if (DateTime.Now.TimeOfDay > aShutdownTime.TimeOfDay)
                    {
                        aAutomaticStopExceeded = true;
                    }
                    recordsDeleted = StoredProcedures.SP_MID_USER_TASKLISTS_DELETE.Delete(_dba, UserRID: userRID, COMMIT_LIMIT: aCommitLimit);
                    _dba.CommitData();
                    totalDeleted += recordsDeleted;
                }

                return totalDeleted;
            }
            catch
            {
                throw;
            }
        }

		public void DeleteUser(int userRID)
		{
			try
			{
                StoredProcedures.SP_MID_USER_DELETE.Delete(_dba, UserRID: userRID);
			}
			catch
			{
				throw;
			}
		}

		public bool MarkUserActive(int aUserRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_ACTIVE_IND.Update(_dba, USER_RID: aUserRID, USER_ACTIVE_IND: '1');
                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool MarkUserInactive(int aUserRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_ACTIVE_IND.Update(_dba, USER_RID: aUserRID, USER_ACTIVE_IND: '0');
                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool MarkUserForDelete(int aUserRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_FOR_DELETION.Update(_dba,
                                                                                                   USER_RID: aUserRID,
                                                                                                   USER_DELETE_DATETIME: DateTime.Now
                                                                                                   );
                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool RecoverDeletedUser(int aUserRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_APPLICATION_USER_UPDATE_FOR_RECOVERY.Update(_dba, USER_RID: aUserRID);
                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetUserItems(int aOwnerRID)
		{
			try
			{
                return StoredProcedures.MID_USER_ITEM_READ_FROM_OWNER.Read(_dba, OWNER_USER_RID: aOwnerRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserItems(int aUserRID, int aOwnerRID)
		{
			try
			{
                return StoredProcedures.MID_USER_ITEM_READ.Read(_dba,
                                                                USER_RID: aUserRID,
                                                                OWNER_USER_RID: aOwnerRID
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool AddUserItem(int aUserRID, int aItemType, int aItemRID, int aOwnerRID)
		{
			try
			{
                int rowsInserted = StoredProcedures.MID_USER_ITEM_INSERT.Insert(_dba,
                                                             USER_RID: aUserRID,
                                                             ITEM_TYPE: aItemType,
                                                             ITEM_RID: aItemRID,
                                                             OWNER_USER_RID: aOwnerRID
                                                             );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteUserItemByTypeAndRID(int aItemType, int aItemRID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM.Delete(_dba,
                                                                                                  ITEM_TYPE: aItemType,
                                                                                                  ITEM_RID: aItemRID
                                                                                                  );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteUserItem(int aUserRID, int aItemType, int aItemRID, int aOwnerRID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_USER_ITEM_DELETE.Delete(_dba,
                                                                               USER_RID: aUserRID,
                                                                               ITEM_TYPE: aItemType,
                                                                               ITEM_RID: aItemRID,
                                                                               OWNER_USER_RID: aOwnerRID
                                                                               );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetAssignedToUsers(int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_USER_ITEM_READ_ASSIGNED.Read(_dba, USER_RID: aUserRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUsersAssignedToMe(int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_USER_ITEM_READ_ASSIGNED_TO_ME.Read(_dba, USER_RID: aUserRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DeleteGroup(int aGroupRID)
		{
			try
			{
                StoredProcedures.SP_MID_USER_GROUP_DELETE.Delete(_dba, GroupRID: aGroupRID);
			}
			catch
			{
				throw;
			}
		}

		public bool MarkGroupActive(int aGroupRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR.Update(_dba,
                                                                                                 GROUP_RID: aGroupRID,
                                                                                                 GROUP_ACTIVE_IND: '1'
                                                                                                 );

                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool MarkGroupInactive(int aGroupRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR.Update(_dba,
                                                                                       GROUP_RID: aGroupRID,
                                                                                       GROUP_ACTIVE_IND: '0'
                                                                                       );

                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool MarkGroupForDelete(int aGroupRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_FOR_DELETION.Update(_dba, 
                                                                                             GROUP_RID: aGroupRID,
                                                                                             GROUP_DELETE_DATETIME: DateTime.Now
                                                                                             );
                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool RecoverDeletedGroup(int aGroupRID)
		{
			try
			{
                int rowsUpdated = StoredProcedures.MID_USER_GROUP_UPDATE_FOR_RECOVERY.Update(_dba, GROUP_RID: aGroupRID);
                return (rowsUpdated > 0);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetDeletedUsers()
		{
			try
			{
                return StoredProcedures.MID_APPLICATION_USER_READ_DELETED_USERS.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetDeletedGroups()
		{
			try
			{
                return StoredProcedures.MID_USER_GROUP_READ_DELETED_GROUPS.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//End Track #4815

		public bool AddUserToGroup(int userRID, int groupRID)
		{
			try
			{
                int insertedRows = StoredProcedures.MID_USER_GROUP_JOIN_INSERT.Insert(_dba,
                                                                                      GROUP_RID: groupRID,
                                                                                      USER_RID: userRID
                                                                                      );
                return (insertedRows > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool RemoveUserFromGroup(int userRID, int groupRID)
		{
			try
			{
                int deletedRows = StoredProcedures.MID_USER_GROUP_JOIN_DELETE.Delete(_dba,
                                                                                     GROUP_RID: groupRID,
                                                                                     USER_RID: userRID
                                                                                     );
                return (deletedRows > 0);
			}
			catch
			{
				throw;
			}
		}



        public DataTable GetUserNameStorageCache()
        {
            return StoredProcedures.MID_APPLICATION_USER_READ_NAMES.Read(_dba);
        }
        public string GetUserNameFromRID(int userRID)
        {
            return (string)StoredProcedures.MID_APPLICATION_USER_READ_NAME_FROM_RID.ReadValue(_dba, USER_RID: userRID);
        }


		public DataTable GetUser(int userRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_APPLICATION_USER_READ.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
  

		public DataTable GetUsers()
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetActiveUsers()
		{
			try
			{
                //begin MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_APPLICATION_USER_READ_ALL_ACTIVE.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public DataTable GetActiveUsersWithGroupRID()
        {   
            return StoredProcedures.MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP.Read(_dba);
        }
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

        //Begin TT#1521-MD -jsobek -Active Directory Authentication
        public bool IsUserInAdminGroup(int userRID)
        {
            bool isInAdminGroup = false;
            DataTable dt = StoredProcedures.MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP.Read(_dba);
            DataRow[] drUser = dt.Select("USER_RID=" + userRID.ToString(), "GROUP_RID ASC");
            if (drUser.Length > 0)
            {
                int groupRID = Convert.ToInt32(drUser[0]["GROUP_RID"]);
                if (groupRID == 1) //1=Admin Group
                {
                    isInAdminGroup = true;
                }
            }
            if (userRID == Include.AdministratorUserRID)
            {
                isInAdminGroup = true;
            }
            return isInAdminGroup;
        }
        //End TT#1521-MD -jsobek -Active Directory Authentication


		public DataTable GetUsers(int groupRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_APPLICATION_USER_READ_FROM_GROUP.Read(_dba, GROUP_RID: groupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetGroup(int groupRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_GROUP_READ.Read(_dba, GROUP_RID: groupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetGroup(string groupName)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_GROUP_READ_FROM_NAME.Read(_dba, GROUP_NAME: groupName);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetGroups()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_GROUP_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetActiveGroups()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_GROUP_READ_ALL_ACTIVE.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        public DataTable GetActiveGroupsNameFirst()
        {
            try
            {
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		public DataTable GetGroups(int userRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_GROUP_READ_FROM_USER.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		

		public DataTable GetGroupNodesAssignment(int groupRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP.Read(_dba, GROUP_RID: groupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public DataTable GetDistinctGroupNodesAssignment(int groupRID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP.Read(_dba, GROUP_RID: groupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetDistinctNodeGroupsAssignment(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public DataTable GetGroupVersionsAssignment(int groupRID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP.Read(_dba, GROUP_RID: groupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetGroupFunctionsAssignment(int groupRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP.Read(_dba, GROUP_RID: groupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool RemoveUserVersion(int userRID, int forVerRID)
		{
			try
			{
                //first delete any existing assignment
                int rowsDeleted = StoredProcedures.MID_SECURITY_USER_VERSION_DELETE.Delete(_dba,
                                                                                           USER_RID: userRID,
                                                                                           FV_RID: forVerRID
                                                                                           );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}


		public bool AssignUserVersion(int userRID, int forVerRID, eSecurityActions aActionID, eDatabaseSecurityTypes aSecurityType, eSecurityLevel secLevelID)
		{
			try
			{
				// first delete any existing assignment
                StoredProcedures.MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE.Delete(_dba,
                                                                                              USER_RID: userRID,
                                                                                              FV_RID: forVerRID,
                                                                                              ACTION_ID: (int)aActionID,
                                                                                              SEC_TYPE: (int)aSecurityType
                                                                                              );

				if (secLevelID <= eSecurityLevel.NotSpecified) 
				{
					return true;	// "not specified", we're done
				}

				// make new assignment
                int rowsInserted = StoredProcedures.MID_SECURITY_USER_VERSION_INSERT.Insert(_dba,
                                                                                             USER_RID: userRID,
                                                                                             FV_RID: forVerRID,
                                                                                             ACTION_ID: (int)aActionID,
                                                                                             SEC_TYPE: (int)aSecurityType,
                                                                                             SEC_LVL_ID: (int)secLevelID
                                                                                             );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool RemoveUserFunction(int userRID, eSecurityFunctions funcID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_SECURITY_USER_FUNCTION_DELETE.Delete(_dba,
                                                                                              USER_RID: userRID,
                                                                                              FUNC_ID: (int)funcID
                                                                                              );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool AssignUserFunction(int userRID, eSecurityFunctions funcID, eSecurityActions aActionID, eSecurityLevel secLevelID)
		{
			try
			{
                StoredProcedures.MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION.Delete(_dba,
                                                                                      USER_RID: userRID,
                                                                                      FUNC_ID: (int)funcID,
                                                                                      ACTION_ID: (int)aActionID
                                                                                      );

				if (secLevelID == eSecurityLevel.NotSpecified) 
				{
					return true;	// "not specified" or "no access", we're done
				}

				// make new assignment
                int rowsInserted = StoredProcedures.MID_SECURITY_USER_FUNCTION_INSERT.Insert(_dba,
                                                                          USER_RID: userRID,
                                                                          FUNC_ID: (int)funcID,
                                                                          ACTION_ID: (int)aActionID,
                                                                          SEC_LVL_ID: (int)secLevelID
                                                                          );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool RemoveUserNode(int userRID, int nodeRID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE.Delete(_dba,
                                                                                                                       USER_RID: userRID,
                                                                                                                       HN_RID: nodeRID
                                                                                                                       );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool AssignUserNode(int userRID, int nodeRID, eSecurityActions aActionID, eDatabaseSecurityTypes aSecurityType, eSecurityLevel secLevelID)
		{
			try
			{
                StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_DELETE.Delete(_dba,
                                                                                USER_RID: userRID,
                                                                                HN_RID: nodeRID,
                                                                                ACTION_ID: (int)aActionID,
                                                                                SEC_TYPE: (int)aSecurityType
                                                                                );

				if (secLevelID == eSecurityLevel.NotSpecified) 
				{
					return true;	// "not specified" or "no access", we're done
				}

				// make new assignment
                int rowsInserted = StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_INSERT.Insert(_dba,
                                                                                                    USER_RID: userRID,
                                                                                                    HN_RID: nodeRID,
                                                                                                    ACTION_ID: (int)aActionID,
                                                                                                    SEC_TYPE: (int)aSecurityType,
                                                                                                    SEC_LVL_ID: (int)secLevelID
                                                                                                    );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool RemoveGroupVersion(int groupRID, int forVerRID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION.Delete(_dba,
                                                                                                                   GROUP_RID: groupRID,
                                                                                                                   FV_RID: forVerRID
                                                                                                                  );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool AssignGroupVersion(int groupRID, int forVerRID, eSecurityActions aActionID, eDatabaseSecurityTypes aSecurityType, eSecurityLevel secLevelID)
		{
			try
			{
				// first delete any existing assignment
                StoredProcedures.MID_SECURITY_GROUP_VERSION_DELETE.Delete(_dba,
                                                                          GROUP_RID: groupRID,
                                                                          FV_RID: forVerRID,
                                                                          ACTION_ID: (int)aActionID,
                                                                          SEC_TYPE: (int)aSecurityType
                                                                          );

				if (secLevelID == eSecurityLevel.NotSpecified) 
				{
					return true;	// "not specified" or "no access", we're done
				}

				// make new assignment
                int rowsInserted = StoredProcedures.MID_SECURITY_GROUP_VERSION_INSERT.Insert(_dba,
                                                                                             GROUP_RID: groupRID,
                                                                                             FV_RID: forVerRID,
                                                                                             ACTION_ID: (int)aActionID,
                                                                                             SEC_TYPE: (int)aSecurityType,
                                                                                             SEC_LVL_ID: (int)secLevelID
                                                                                             );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}


		public bool RemoveGroupFunction(int groupRID, eSecurityFunctions funcID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION.Delete(_dba,
                                                                                                                     GROUP_RID: groupRID,
                                                                                                                     FUNC_ID: (int)funcID
                                                                                                                     );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool AssignGroupFunction(int groupRID, eSecurityFunctions funcID, eSecurityActions aActionID, eSecurityLevel secLevelID)
		{
			try
			{
                StoredProcedures.MID_SECURITY_GROUP_FUNCTION_DELETE.Delete(_dba,
                                                                           GROUP_RID: groupRID,
                                                                           FUNC_ID: (int)funcID,
                                                                           ACTION_ID: (int)aActionID
                                                                           );

				if (secLevelID == eSecurityLevel.NotSpecified) 
				{
					return true;	// "not specified" or "no access", we're done
				}

				// make new assignment
                int rowsInserted = StoredProcedures.MID_SECURITY_GROUP_FUNCTION_INSERT.Insert(_dba,
                                                                                              GROUP_RID: groupRID,
                                                                                              FUNC_ID: (int)funcID,
                                                                                              ACTION_ID: (int)aActionID,
                                                                                              SEC_LVL_ID: (int)secLevelID
                                                                                             );
                return (rowsInserted > 0);                                                 
			}
			catch
			{
				throw;
			}
		}

		public bool RemoveGroupNode(int groupRID, int nodeRID)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE.Delete(_dba,
                                                                                                                       GROUP_RID: groupRID,
                                                                                                                       HN_RID: nodeRID
                                                                                                                       );
                return (rowsDeleted > 0);
			}
			catch
			{
				throw;
			}
		}

		public bool AssignGroupNode(int groupRID, int nodeRID, eSecurityActions aActionID, eDatabaseSecurityTypes aSecurityType, eSecurityLevel secLevelID)
		{
			try
			{
                StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE.Delete(_dba,
                                                                                 GROUP_RID: groupRID,
                                                                                 HN_RID: nodeRID,
                                                                                 ACTION_ID: (int)aActionID,
                                                                                 SEC_TYPE: (int)aSecurityType
                                                                                 );

				if (secLevelID == eSecurityLevel.NotSpecified) 
				{
					return true;	// "not specified" or "no access", we're done
				}

				// make new assignment
                int rowsInserted = StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT.Insert(_dba,
                                                                                                    GROUP_RID: groupRID,
                                                                                                    HN_RID: nodeRID,
                                                                                                    ACTION_ID: (int)aActionID,
                                                                                                    SEC_TYPE: (int)aSecurityType,
                                                                                                    SEC_LVL_ID: (int)secLevelID
                                                                                                    );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}


		

		public DataTable GetUserVersionAssignment(int userRID, int forVerRID, eDatabaseSecurityTypes aSecurityType)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_USER_VERSION_READ.Read(_dba,
                                                                            USER_RID: userRID,
                                                                            FV_RID: forVerRID,
                                                                            SEC_TYPE: (int)aSecurityType
                                                                            );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserGroupsVersionAssignment(int userRID, int forVerRID, eDatabaseSecurityTypes aSecurityType)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_GROUP_VERSION_READ_FROM_USER.Read(_dba,
                                                                                       USER_RID: userRID,
                                                                                       FV_RID: forVerRID,
                                                                                       SEC_TYPE: (int)aSecurityType
                                                                                       );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		public DataTable GetGroupVersionAssignment(int groupRID, int forVerRID, eDatabaseSecurityTypes aSecurityType)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_GROUP_VERSION_READ.Read(_dba,
                                                                             GROUP_RID: groupRID,
                                                                             FV_RID: forVerRID,
                                                                             SEC_TYPE: (int)aSecurityType
                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public DataTable GetUserVersionsAssignment(int userRID)
		{

			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserFunctionsAssignment(int userRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_USER_FUNCTION_READ_FROM_USER.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public DataTable GetUserFunctionAssignment(int userRID, eSecurityFunctions funcID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_USER_FUNCTION_READ.Read(_dba,
                                                                             FUNC_ID: (int)funcID,
                                                                             USER_RID: userRID
                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserGroupsFunctionAssignment(int userRID, eSecurityFunctions funcID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS.Read(_dba,
                                                                                              FUNC_ID: (int)funcID,
                                                                                              USER_RID: userRID
                                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetSecurityFunctions()
		{
			try
			{
                return StoredProcedures.MID_SECURITY_FUNCTION_JOIN_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetSecurityFunction(eSecurityFunctions aFuncID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_FUNCTION_JOIN_READ.Read(_dba, FUNC_ID: (int)aFuncID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		public DataTable GetChildrenFunctions(eSecurityFunctions aFuncParentID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN.Read(_dba, FUNC_PARENT: (int)aFuncParentID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        //End Track #5091 - JScott - Secuirty Lights don't change when permission changes
	
		public DataTable GetActionsForFunction(int aFunctionID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION.Read(_dba, FUNC_ID: aFunctionID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetActionsForVersion()
		{
			try
			{
                return StoredProcedures.MID_SECURITY_VERSION_ACTIONS_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetActionsForMerchandise()
		{
			try
			{
                //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -workspace performance
                if (SecurityStorage.dtMerchActionNames == null)
                {
                    SecurityStorage.dtMerchActionNames = StoredProcedures.MID_SECURITY_MERCH_ACTIONS_READ_ALL.Read(_dba);
                }

                return SecurityStorage.dtMerchActionNames;
                //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -workspace performance
                
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		public DataTable GetGroupFunctionAssignment(int groupRID, eSecurityFunctions funcID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION.Read(_dba,
                                                                                                      GROUP_RID: groupRID,
                                                                                                      FUNC_ID: (int)funcID
                                                                                                      );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public bool IsUserNodeAssigned(int userRID, int nodeRID)
		{
			try
			{
                int recordCount = StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT.ReadRecordCount(_dba,
                                                                                             USER_RID: userRID,
                                                                                             HN_RID: nodeRID
                                                                                             );
				
				if (recordCount == 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        
		
		public DataTable GetDistinctUserNodesAssignment(int userRID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserNodesAssignment(int userRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserNodesAssignment(int userRID, eDatabaseSecurityTypes aSecurityType)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE.Read(_dba,
                                                                                                      USER_RID: userRID,
                                                                                                      SEC_TYPE: (int)aSecurityType
                                                                                                      );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		public DataTable GetDistinctUserGroupsNodesAssignment(int userRID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetDistinctNodeUsersAssignment(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserGroupsNodesAssignment(int userRID, eDatabaseSecurityTypes aSecurityType)
		{
			try
			{
                return StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT.Read(_dba,
                                                                                                     USER_RID: userRID,
                                                                                                     SEC_TYPE: (int)aSecurityType
                                                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetGroupNodeAssignment(int groupRID, int nodeRID, eDatabaseSecurityTypes aSecurityType)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE.Read(_dba,
                                                                                                             GROUP_RID: groupRID,
                                                                                                             HN_RID: nodeRID,
                                                                                                             SEC_TYPE: (int)aSecurityType
                                                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool IsGroupNodeAssigned(int groupRID, int nodeRID)
		{
			try
			{
                int recordCount = StoredProcedures.MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT.ReadRecordCount(_dba,
                                                                                              GROUP_RID: groupRID,
                                                                                              HN_RID: nodeRID
                                                                                              );
				
				if (recordCount == 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetUserOptions(int userRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_OPTIONS_READ.Read(_dba, USER_RID: userRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
 
		public void InsertUserOptions(int userRID, string myHierarchy, string myHierarchyColor)
		{
			try
			{
                StoredProcedures.MID_USER_OPTIONS_INSERT_MY_HIERARCHY.Insert(_dba,
                                                                             USER_RID: userRID,
                                                                             MY_HIERARCHY: myHierarchy,
                                                                             MY_HIERARCHY_COLOR: myHierarchyColor
                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // TT#4243 - RMatelic - Add SHOW_SIGNOFF_PROMPT column
		public void InsertUserOptions(int userRID, string myHierarchy, string myHierarchyColor, 
			bool aForecastMonitorIsOn, string aForecastMonitorDirectory,
			bool aModifySalesMonitorIsOn, string aModifySalesMonitorDirectory,
			eMIDMessageLevel aAuditLoggingLevel, bool aShowLogin, bool aShowSignOffPrompt,
            bool aDCFulfillmentMonitorIsOn, string aDCFulfillmentMonitorDirectory)
		{
			try
			{
                StoredProcedures.MID_USER_OPTIONS_INSERT.Insert(_dba,
                                                                USER_RID: userRID,
                                                                MY_HIERARCHY: myHierarchy,
                                                                MY_HIERARCHY_COLOR: myHierarchyColor,
                                                                FORECAST_MONITOR_ACTIVE: Include.ConvertBoolToChar(aForecastMonitorIsOn),
                                                                FORECAST_MONITOR_DIRECTORY: aForecastMonitorDirectory,
                                                                MODIFY_SALES_MONITOR_ACTIVE: Include.ConvertBoolToChar(aModifySalesMonitorIsOn),
                                                                MODIFY_SALES_MONITOR_DIRECTORY: aModifySalesMonitorDirectory,
                                                                AUDIT_LOGGING_LEVEL: Convert.ToInt32(aAuditLoggingLevel),
                                                                SHOW_LOGIN: Include.ConvertBoolToChar(aShowLogin),
                                                                SHOW_SIGNOFF_PROMPT: Include.ConvertBoolToChar(aShowSignOffPrompt),
                                                                DCFULFILLMENT_MONITOR_ACTIVE: Include.ConvertBoolToChar(aDCFulfillmentMonitorIsOn),
                                                                DCFULFILLMENT_MONITOR_DIRECTORY: aDCFulfillmentMonitorDirectory
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Insert My Workflow/Methods folder text
		/// </summary>
		/// <param name="userRID">UserRID to insert</param>
		/// <param name="myWorkflowMethods">name of user's MyWorkflowMethods folder</param>
		public void InsertUserOptions(int userRID, string myWorkflowMethods)
		{
			try
			{
                StoredProcedures.MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS.Insert(_dba,
                                                                                    USER_RID: userRID,
                                                                                    MY_WORKFLOWMETHODS: myWorkflowMethods
                                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Insert ThemeRID
		/// </summary>
		/// <param name="userRID">UserRID to insert</param>
		/// <param name="myWorkflowMethods">ThemeRID</param>
		public void InsertUserOptions(int userRID, int themeRID)
		{
			try
			{
                StoredProcedures.MID_USER_OPTIONS_INSERT_THEME.Insert(_dba,
                                                                      USER_RID: userRID,
                                                                      THEME_RID: themeRID
                                                                      );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public void UpdateMyHierarchy(int userRID, string myHierarchy, string myHierarchyColor)
		{
			try
			{
				if (!User_OptionsRowExists(userRID))
				{
					InsertUserOptions (userRID, myHierarchy, myHierarchyColor);
				}
				else
				{
                    StoredProcedures.MID_USER_OPTIONS_UPDATE_MY_HIERARCHY.Update(_dba,
                                                                             USER_RID: userRID,
                                                                             MY_HIERARCHY: myHierarchy,
                                                                             MY_HIERARCHY_COLOR: myHierarchyColor
                                                                             );
				}
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // TT#4243 - RMatelic - Add SHOW_SIGNOFF_PROMPT column
		public void UpdateUserOptions(int userRID, string myHierarchy, string myHierarchyColor, 
			bool aForecastMonitorIsOn, string aForecastMonitorDirectory,
			bool aModifySalesMonitorIsOn, string aModifySalesMonitorDirectory,
            eMIDMessageLevel aAuditLoggingLevel, bool aShowLogin, bool aShowSignOffPrompt,
            bool aDCFulfillmentMonitorIsOn, string aDCFulfillmentMonitorDirectory)
		{
			try
			{
				if (!User_OptionsRowExists(userRID))
				{
                    InsertUserOptions(userRID, myHierarchy, myHierarchyColor,
                        aForecastMonitorIsOn, aForecastMonitorDirectory,
                        aModifySalesMonitorIsOn, aModifySalesMonitorDirectory,
                        aAuditLoggingLevel, aShowLogin, aShowSignOffPrompt,
                        aDCFulfillmentMonitorIsOn, aDCFulfillmentMonitorDirectory);
				}
				else
				{
                    StoredProcedures.MID_USER_OPTIONS_UPDATE.Update(_dba,
                                                                USER_RID: userRID,
                                                                MY_HIERARCHY: myHierarchy,
                                                                MY_HIERARCHY_COLOR: myHierarchyColor,
                                                                FORECAST_MONITOR_ACTIVE: Include.ConvertBoolToChar(aForecastMonitorIsOn),
                                                                FORECAST_MONITOR_DIRECTORY: aForecastMonitorDirectory,
                                                                MODIFY_SALES_MONITOR_ACTIVE: Include.ConvertBoolToChar(aModifySalesMonitorIsOn),
                                                                MODIFY_SALES_MONITOR_DIRECTORY: aModifySalesMonitorDirectory,
                                                                AUDIT_LOGGING_LEVEL: (int)aAuditLoggingLevel,
                                                                SHOW_LOGIN: Include.ConvertBoolToChar(aShowLogin),
                                                                SHOW_SIGNOFF_PROMPT: Include.ConvertBoolToChar(aShowSignOffPrompt),
                                                                DCFULFILLMENT_MONITOR_ACTIVE: Include.ConvertBoolToChar(aDCFulfillmentMonitorIsOn),
                                                                DCFULFILLMENT_MONITOR_DIRECTORY: aDCFulfillmentMonitorDirectory
                                                                );
				}
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Update user's MyWorkflowMethods folder text
		/// </summary>
		/// <param name="userRID">UserRID to update</param>
		/// <param name="myWorkflowMethods">New My Workflow/Methods text</param>
		public void UpdateMyWorkflowMethodsText(int userRID, string myWorkflowMethods)
		{
			try
			{
				if (User_OptionsRowExists(userRID))
				{
                    StoredProcedures.MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS.Update(_dba,
                                                                                    USER_RID: userRID,
                                                                                    MY_WORKFLOWMETHODS: myWorkflowMethods
                                                                                    );
					return;
				}
				else
				{
					InsertUserOptions (userRID, myWorkflowMethods);
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Update user's show login flag
		/// </summary>
		/// <param name="aUserRID">UserRID to update</param>
		/// <param name="aShowLogin">New show login flag</param>
		public void UpdateShowLogin(int aUserRID, bool aShowLogin)
		{
			try
			{
				if (User_OptionsRowExists(aUserRID))
				{
                    StoredProcedures.MID_USER_OPTIONS_UPDATE_SHOW_LOGIN.Update(_dba,
                                                                           USER_RID: aUserRID,
                                                                           SHOW_LOGIN: Include.ConvertBoolToChar(aShowLogin)
                                                                           );
					return;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Update user's ThemeRID
		/// </summary>
		/// <param name="userRID">UserRID to update</param>
		/// <param name="myWorkflowMethods">ThemeRID</param>
		public void UpdateThemeRID(int userRID, int themeRID)
		{
			try
			{
				if (User_OptionsRowExists(userRID))
				{
                    StoredProcedures.MID_USER_OPTIONS_UPDATE_THEME.Update(_dba,
                                                                      USER_RID: userRID,
                                                                      THEME_RID: themeRID
                                                                      );
					return;
				}
				else
				{
					InsertUserOptions (userRID, themeRID);
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private bool User_OptionsRowExists(int userRID)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            int recordCount = StoredProcedures.MID_USER_OPTIONS_READ_COUNT.ReadRecordCount(_dba, USER_RID: userRID);
				
			if (recordCount == 0)
				return false;

			return true;
		}

	}
}
