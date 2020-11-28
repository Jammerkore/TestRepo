using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;

namespace MIDRetail.Business
{
    public static class ActiveDirectoryCheck
    {
        public static bool IsCurrentUserValidInActiveDirectory()
        {
            bool isValidADUser = false;
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Environment.UserDomainName);
                if (ctx != null)
                {
                    System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                    //System.Security.Principal.IPrincipal wp = new System.Security.Principal.WindowsPrincipal((System.Security.Principal.WindowsIdentity)currentUser);

                    //UserPrincipal insUserPrincipal = new UserPrincipal(ctx);
                    UserPrincipal insUserPrincipal = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, currentUser.Name);

                    // UserPrincipal insUserPrincipal = System.DirectoryServices.AccountManagement.UserPrincipal.Current;

                    //DateTime? lastlogon = insUserPrincipal.LastLogon;
                    if (insUserPrincipal.IsAccountLockedOut() == false)
                    {
                        isValidADUser = true; // currentUser.IsAuthenticated;
                        // bool isValidADUser = ctx.ValidateCredentials(currentUser.Name, currentUser. , ContextOptions.SimpleBind);
                    }
                }
            }
            catch (Exception ex)
            {
                #if (DEBUG)
                ExceptionHandler.HandleException(ex);
                #else
               
                #endif
            }
            return isValidADUser;
        }
    }
}
