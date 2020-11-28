using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms; 

namespace MIDRetail.DataCommon
{
    public class SysInfo : Component
    {
        System.Security.Principal.WindowsIdentity _currentUser;
        
        public SysInfo() 
        {
            _currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
        }

        public string UserNameWithDomain
        {
            get
            {
                return _currentUser.Name;
            }
        }

        public string UserName
        {
            get
            {
                return SystemInformation.UserName;
            }
        }

        public string ComputerName
        {
            get
            {
                return SystemInformation.ComputerName;
            }
        }

        public bool ConnectedToNetwork
        {
            get
            {
                if (SystemInformation.Network)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public int MonitorCount
        {
            get
            {
                return SystemInformation.MonitorCount;
            }
        }

        public bool MousePresent
        {
            get
            {
                return SystemInformation.MousePresent;
            }
        }

        public int NumberMouseButtons
        {
            get
            {
                if (SystemInformation.MousePresent)
                {
                    return SystemInformation.MouseButtons;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool TerminalServerSession
        {
            get
            {
                return SystemInformation.TerminalServerSession;
            }
        }

        public BootMode BootMode
        {
            get
            {
                return SystemInformation.BootMode;
            }
        }

        public  System.Security.Principal.WindowsIdentity CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }
    }
}
