using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public partial class ROAdministration : ROWebFunction
    {

        /// <summary>
        /// Get User information
        /// </summary>
        /// <returns>Class containing user information</returns>
        private ROOut GetUserInformation(RONoParms parms)
        {
            ROUserInformation ROUserInformation = new ROUserInformation(
                user: GetName.GetUser(SAB),
                userName: SAB.ClientServerSession.UserName,
                userFullName: SAB.ClientServerSession.UserFullName,
                userDescription: SAB.ClientServerSession.UserDescription,
                isActive: SAB.ClientServerSession.UserIsActive,
                isSetToBeDeleted: SAB.ClientServerSession.UserIsSetToBeDeleted,
                dateTimeWhenDeleted: SAB.ClientServerSession.UserDateTimeWhenDeleted
                );
            ROUserInformationOut ROUserInformationOut = new ROUserInformationOut(eROReturnCode.Successful, null, parms.ROInstanceID, ROUserInformation);

            return ROUserInformationOut;
        }

        /// <summary>
        /// Get User information
        /// </summary>
        /// <returns>Class containing user information</returns>
        private ROOut GetUserInformation(ROStringParms parms)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = null;

            int userKey = UserNameStorage.GetUserRID(parms.ROString);
            ROUserInformation userInformation = GetUserInformation(userKey: userKey, returnCode: out returnCode, message: out message);
            ROUserInformationOut ROUserInformationOut = new ROUserInformationOut(returnCode, message, parms.ROInstanceID, userInformation);

            return ROUserInformationOut;
        }

        /// <summary>
        /// Get User information
        /// </summary>
        /// <returns>Class containing user information</returns>
        private ROOut GetUserInformation(ROKeyParms parms)
        {
            eROReturnCode returnCode = eROReturnCode.Successful; 
            string message = null;

            ROUserInformation userInformation = GetUserInformation(userKey: parms.Key, returnCode: out returnCode, message: out message);
            ROUserInformationOut ROUserInformationOut = new ROUserInformationOut(returnCode, message, parms.ROInstanceID, userInformation);

            return ROUserInformationOut;
        }

        private ROUserInformation GetUserInformation (int userKey, out eROReturnCode returnCode, out string message)
        {
            returnCode = eROReturnCode.Successful;
            message = null;
            SecurityAdmin securityData = new SecurityAdmin();
            string userName = null, userFullName = null, userDescription = null;
            bool userIsActive = true, userIsSetToBeDeleted = false;
            DateTime userDateTimeWhenDeleted = DateTime.MinValue;

            DataTable dt = securityData.GetUser(userRID: userKey);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0].IsNull("USER_NAME") == false)
                {
                    userName = (string)dt.Rows[0]["USER_NAME"];
                }
                if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
                {
                    userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
                }
                if (dt.Rows[0].IsNull("USER_DESCRIPTION") == false)
                {
                    userDescription = (string)dt.Rows[0]["USER_DESCRIPTION"];
                }
                if (dt.Rows[0].IsNull("USER_ACTIVE_IND") == false)
                {
                    userIsActive = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["USER_ACTIVE_IND"]));
                }
                if (dt.Rows[0].IsNull("USER_DELETE_IND") == false)
                {
                    userIsSetToBeDeleted = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["USER_DELETE_IND"]));
                }
                if (dt.Rows[0].IsNull("USER_DELETE_DATETIME") == false)
                {
                    userDateTimeWhenDeleted = (DateTime)dt.Rows[0]["USER_DELETE_DATETIME"];
                }

            }
            else
            {
                returnCode = eROReturnCode.Failure;
                message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_User) }
                            );
            }

            ROUserInformation ROUserInformation = new ROUserInformation(
                user: GetName.GetUser(userKey: userKey),
                userName: userName,
                userFullName: userFullName,
                userDescription: userDescription,
                isActive: userIsActive,
                isSetToBeDeleted: userIsSetToBeDeleted,
                dateTimeWhenDeleted: userDateTimeWhenDeleted
                );

            return ROUserInformation;
        }

        private ROOut SaveUserInformation(RUserInformationParms parms)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;
            bool keyLookedUp = false;

            SecurityAdmin securityData = new SecurityAdmin();
            Security security = new Security();

            int userKey = parms.ROUserInformation.User.Key;

            if (userKey == Include.NoRID
                && parms.ROUserInformation.LookupUserKey)
            {
                userKey = securityData.GetUserRIDFromName(parms.ROUserInformation.User.Value);
                parms.ROUserInformation.User = new KeyValuePair<int, string>(userKey, parms.ROUserInformation.User.Value);
                keyLookedUp = true;
            }

            if (parms.ROUserInformation.CreateUserLike.Key <= 0
                && !string.IsNullOrEmpty(parms.ROUserInformation.CreateUserLike.Value))
            {
                int createLikeUserKey = securityData.GetUserRIDFromName(parms.ROUserInformation.CreateUserLike.Value);
                parms.ROUserInformation.CreateUserLike = new KeyValuePair<int, string>(createLikeUserKey, parms.ROUserInformation.CreateUserLike.Value);
            }

            if (userKey == Include.NoRID)
            {
                // check if user name already exists
                if (!keyLookedUp)
                {
                    userKey = securityData.GetUserRIDFromName(parms.ROUserInformation.User.Value);
                    if (userKey != Include.NoRID)
                    {
                        return new ROIntOut(eROReturnCode.Failure, MIDText.GetText((eMIDTextCode)eValidUsername.InvalidAlreadyExists), parms.ROInstanceID, Include.NoRID);
                    }
                }
                if (!AddUser(security: security, parms: parms, message: ref message, userKey: out userKey))
                {
                    returnCode = eROReturnCode.Failure;
                }
            }
            else
            {
                // make sure name does not belong to a different user
                if (!keyLookedUp)
                {
                    userKey = securityData.GetUserRIDFromName(parms.ROUserInformation.User.Value);
                    if (userKey != parms.ROUserInformation.User.Key)
                    {
                        return new ROIntOut(eROReturnCode.Failure, MIDText.GetText((eMIDTextCode)eValidUsername.InvalidAlreadyExists), parms.ROInstanceID, parms.ROUserInformation.User.Key);
                    }
                }
                if (!UpdateUser(security: security, parms: parms, message: ref message))
                {
                    returnCode = eROReturnCode.Failure;
                }
            }

            ROIntOut ROUserSaveOut = new ROIntOut(returnCode, message, parms.ROInstanceID, userKey);

            return ROUserSaveOut;
        }

        private bool AddUser(Security security, RUserInformationParms parms, ref string message, out int userKey)
        {
            userKey = Include.NoRID;
            try
            {
                userKey = security.AddUser(userID: parms.ROUserInformation.User.Value,
                                            fullName: parms.ROUserInformation.FullName,
                                            description: parms.ROUserInformation.Description,
                                            createUserLikeKey: parms.ROUserInformation.CreateUserLike.Key);

                if (!string.IsNullOrEmpty(parms.ROUserInformation.AddToGroup))
                {
                    security.AssignUserToGroup(userKey: userKey, groupID: parms.ROUserInformation.AddToGroup);
                }

                SAB.ClientServerSession.RefreshSecurity();
            }
            catch (Exception error)
            {
                message = error.Message;
                return false;
            }

            return true;
        }

        private bool UpdateUser(Security security, RUserInformationParms parms, ref string message)
        {

            try
            {
                security.UpdateUser(userKey: parms.ROUserInformation.User.Key,
                        userID: parms.ROUserInformation.User.Value,
                        fullName: parms.ROUserInformation.FullName,
                        description: parms.ROUserInformation.Description,
                        isActive: parms.ROUserInformation.IsActive);

                if (!string.IsNullOrEmpty(parms.ROUserInformation.AddToGroup))
                {
                    security.AssignUserToGroup(userKey: parms.ROUserInformation.User.Key, groupID: parms.ROUserInformation.AddToGroup);
                }

                SAB.ClientServerSession.RefreshSecurity();
            }
            catch (Exception error)
            {
                message = error.Message;
                return false;
            }

            return true;
        }

        
    }
}
