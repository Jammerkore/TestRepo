using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.DataCommon
{
    /// <summary>
    /// Holds a list of all users names that were defined at the time the application started.
    /// </summary>
    public static class UserNameStorage
    {

        

        private class UserNameEntry
        {
            public string userName;
            public int userRID;

            public UserNameEntry(string userName, int userRID)
            {
                this.userName = userName;
                this.userRID = userRID;
            }
        }
        private static List<UserNameEntry> listUserNames;

        public static void PopulateUserNameStorageCache(GetSelectedHeaderRIDsFromWorkspace getSelectedHeaderRIDsFromWorkspace)
        {
        }
        public static void PopulateUserNameStorageCache(DataTable dtUsers)
        {
            listUserNames = new List<UserNameEntry>();
            foreach (DataRow dr in dtUsers.Rows)
            {
                listUserNames.Add(new UserNameEntry((string)dr["USER_NAME"], (int)dr["USER_RID"]));
            }
        }
        //Handle newly added users
        public delegate string GetUnknownUserNameDelegate(int userRID);
        private static GetUnknownUserNameDelegate _getUnknownUserNameDelegate;
        public static void SetDelegate_GetUnknownUserName(GetUnknownUserNameDelegate getUnknownUserNameDelegate)
        {
            _getUnknownUserNameDelegate = getUnknownUserNameDelegate;
        }

        /// <summary>
        /// Returns cached userName based on userRID
        /// If not found, returns an empty string
        /// </summary>
        /// <param name="userRID"></param>
        /// <returns></returns>
        public static string GetUserName(int userRID)
        {
            if (listUserNames != null)
            {
                UserNameEntry result = listUserNames.Find(delegate(UserNameEntry o) { return o.userRID == userRID; });
                if (result != null)
                {
                    return result.userName;
                }
                else
                {
                    if (userRID == -1)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        //Handle newly added users by attempting to get the name and adding it to the cache.
                        //using a delegate since we do not have access to the database here.
                        string userName = _getUnknownUserNameDelegate(userRID);
                        if (userName != string.Empty)
                        {
                            listUserNames.Add(new UserNameEntry(userName, userRID));
                        }
                        return userName;
                    }
                }
            }
            else
            {
                throw new Exception("User Name Cache was not populated prior to referencing.");
            }

        }

        /// <summary>
        /// Returns cached userRID based on userName
        /// If not found, returns Include.NoRID
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int GetUserRID(string userName)
        {
            if (listUserNames != null)
            {
                UserNameEntry result = listUserNames.Find(delegate (UserNameEntry o) { return o.userName.ToLower() == userName.ToLower(); });
                if (result != null)
                {
                    return result.userRID;
                }
                else
                {
                    return Include.NoRID;
                }
            }
            else
            {
                throw new Exception("User Name Cache was not populated prior to referencing.");
            }

        }
    }
}
