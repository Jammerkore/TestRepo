using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    //
    //  BEGIN TT#1653 - GRT - Create new class to replace shortcut code in the installer to allow for VS2010
    //
    public class ShellShortcut : IDisposable
    {
        public enum CSIDL : int
        {
            CSIDL_DESKTOP = 0x0000,    // <desktop>
            CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)
            CSIDL_PROGRAMS = 0x0002,    // Start Menu\Programs
            CSIDL_CONTROLS = 0x0003,    // My Computer\Control Panel
            CSIDL_PRINTERS = 0x0004,    // My Computer\Printers
            CSIDL_PERSONAL = 0x0005,    // My Documents
            CSIDL_FAVORITES = 0x0006,    // <user name>\Favorites
            CSIDL_STARTUP = 0x0007,    // Start Menu\Programs\Startup
            CSIDL_RECENT = 0x0008,    // <user name>\Recent
            CSIDL_SENDTO = 0x0009,    // <user name>\SendTo
            CSIDL_BITBUCKET = 0x000a,    // <desktop>\Recycle Bin
            CSIDL_STARTMENU = 0x000b,    // <user name>\Start Menu
            CSIDL_MYDOCUMENTS = 0x000c,    // logical "My Documents" desktop icon
            CSIDL_MYMUSIC = 0x000d,    // "My Music" folder
            CSIDL_MYVIDEO = 0x000e,    // "My Videos" folder
            CSIDL_DESKTOPDIRECTORY = 0x0010,    // <user name>\Desktop
            CSIDL_DRIVES = 0x0011,    // My Computer
            CSIDL_NETWORK = 0x0012,    // Network Neighborhood (My Network Places)
            CSIDL_NETHOOD = 0x0013,    // <user name>\nethood
            CSIDL_FONTS = 0x0014,    // windows\fonts
            CSIDL_TEMPLATES = 0x0015,
            CSIDL_COMMON_STARTMENU = 0x0016,    // All Users\Start Menu
            CSIDL_COMMON_PROGRAMS = 0X0017,    // All Users\Start Menu\Programs
            CSIDL_COMMON_STARTUP = 0x0018,    // All Users\Startup
            CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop
            CSIDL_APPDATA = 0x001a,    // <user name>\Application Data
            CSIDL_PRINTHOOD = 0x001b,    // <user name>\PrintHood
            CSIDL_LOCAL_APPDATA = 0x001c,    // <user name>\Local Settings\Applicaiton Data (non roaming)
            CSIDL_ALTSTARTUP = 0x001d,    // non localized startup
            CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup
            CSIDL_COMMON_FAVORITES = 0x001f,
            CSIDL_INTERNET_CACHE = 0x0020,
            CSIDL_COOKIES = 0x0021,
            CSIDL_HISTORY = 0x0022,
            CSIDL_COMMON_APPDATA = 0x0023,    // All Users\Application Data
            CSIDL_WINDOWS = 0x0024,    // GetWindowsDirectory()
            CSIDL_SYSTEM = 0x0025,    // GetSystemDirectory()
            CSIDL_PROGRAM_FILES = 0x0026,    // C:\Program Files
            CSIDL_MYPICTURES = 0x0027,    // C:\Program Files\My Pictures
            CSIDL_PROFILE = 0x0028,    // USERPROFILE
            CSIDL_SYSTEMX86 = 0x0029,    // x86 system directory on RISC
            CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC
            CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common
            CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC
            CSIDL_COMMON_TEMPLATES = 0x002d,    // All Users\Templates
            CSIDL_COMMON_DOCUMENTS = 0x002e,    // All Users\Documents
            CSIDL_COMMON_ADMINTOOLS = 0x002f,    // All Users\Start Menu\Programs\Administrative Tools
            CSIDL_ADMINTOOLS = 0x0030,    // <user name>\Start Menu\Programs\Administrative Tools
            CSIDL_CONNECTIONS = 0x0031,    // Network and Dial-up Connections
            CSIDL_COMMON_MUSIC = 0x0035,    // All Users\My Music
            CSIDL_COMMON_PICTURES = 0x0036,    // All Users\My Pictures
            CSIDL_COMMON_VIDEO = 0x0037,    // All Users\My Video
            CSIDL_CDBURN_AREA = 0x003b    // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
       }

        private const int INFOTIPSIZE = 1024;
        private const int MAX_PATH = 260;

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWMINNOACTIVE = 7;


        #if UNICODE
            private IShellLinkW m_Link;
        #else
            private IShellLinkA m_Link;
        #endif
            private string m_sPath;

        ///
        /// <param name='linkPath'>
        ///   Path to new or existing shortcut file (.lnk).
        /// </param>
        ///
        public ShellShortcut(string linkPath)
        {
            IPersistFile pf;

            m_sPath = linkPath;

            #if UNICODE
                m_Link = (IShellLinkW) new ShortcutNative();
#else
            m_Link = (IShellLinkA)new ShortcutNative();
            #endif

            if ( File.Exists( linkPath ) ) {
                pf = (IPersistFile)m_Link;
                pf.Load( linkPath, 0 );
            }
        }

        //
        //  IDisplosable implementation
        //
        public void Dispose()
        {
            if ( m_Link != null ) {
                Marshal.ReleaseComObject( m_Link );
                m_Link = null;
            }
        }

        /// <value>
        ///   Gets or sets the argument list of the shortcut.
        /// </value>
        public string Arguments
        {
            get
            {
                StringBuilder sb = new StringBuilder( INFOTIPSIZE );
                m_Link.GetArguments( sb, sb.Capacity );
                return sb.ToString();
            }
            set { m_Link.SetArguments( value ); }
        }

        /// <value>
        ///   Gets or sets a description of the shortcut.
        /// </value>
        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder( INFOTIPSIZE );
                m_Link.GetDescription( sb, sb.Capacity );
                return sb.ToString();
            }
            set { m_Link.SetDescription( value ); }
        }

        /// <value>
        ///   Gets or sets the working directory (aka start in directory) of the shortcut.
        /// </value>
        public string WorkingDirectory
        {
            get
            {
                StringBuilder sb = new StringBuilder( MAX_PATH );
                m_Link.GetWorkingDirectory( sb, sb.Capacity );
                return sb.ToString();
            }
            set { m_Link.SetWorkingDirectory( value ); }
        }

        //
        // If Path returns an empty string, the shortcut is associated with
        // a PIDL instead, which can be retrieved with IShellLink.GetIDList().
        // This is beyond the scope of this wrapper class.
        //
        /// <value>
        ///   Gets or sets the target path of the shortcut.
        /// </value>
        public string Path
        {
            get
            {
                #if UNICODE
                    WIN32_FIND_DATAW wfd = new WIN32_FIND_DATAW();
                #else
                    WIN32_FIND_DATAA wfd = new WIN32_FIND_DATAA();
                #endif
                StringBuilder sb = new StringBuilder( MAX_PATH );

                m_Link.GetPath( sb, sb.Capacity, out wfd, SLGP_FLAGS.SLGP_UNCPRIORITY );
                return sb.ToString();
            }
            set { m_Link.SetPath( value ); }
        }

        /// <value>
        ///   Gets or sets the path of the <see cref="Icon"/> assigned to the shortcut.
        /// </value>
        /// <summary>
        ///   <seealso cref="IconIndex"/>
        /// </summary>
        public string IconPath
        {
            get
            {
                StringBuilder sb = new StringBuilder( MAX_PATH );
                int nIconIdx;
                m_Link.GetIconLocation( sb, sb.Capacity, out nIconIdx );
                return sb.ToString();
            }
            set { m_Link.SetIconLocation( value, IconIndex ); }
        }

        /// <value>
        ///   Gets or sets the index of the <see cref="Icon"/> assigned to the shortcut.
        ///   Set to zero when the <see cref="IconPath"/> property specifies a .ICO file.
        /// </value>
        /// <summary>
        ///   <seealso cref="IconPath"/>
        /// </summary>
        public int IconIndex
        {
            get
            {
                StringBuilder sb = new StringBuilder( MAX_PATH );
                int nIconIdx;
                m_Link.GetIconLocation( sb, sb.Capacity, out nIconIdx );
                return nIconIdx;
            }
            set { m_Link.SetIconLocation( IconPath, value ); }
        }

        /// <value>
        ///   Retrieves the Icon of the shortcut as it will appear in Explorer.
        ///   Use the <see cref="IconPath"/> and <see cref="IconIndex"/>
        ///   properties to change it.
        /// </value>
        public Icon Icon
        {
            get
            {
                StringBuilder sb = new StringBuilder( MAX_PATH );
                int nIconIdx;
                IntPtr hIcon, hInst;
                Icon ico, clone;


                m_Link.GetIconLocation( sb, sb.Capacity, out nIconIdx );
                hInst = Marshal.GetHINSTANCE( this.GetType().Module );
                hIcon = Native.ExtractIcon( hInst, sb.ToString(), nIconIdx );
                if ( hIcon == IntPtr.Zero )
                return null;

                // Return a cloned Icon, because we have to free the original ourselves.
                ico = Icon.FromHandle( hIcon );
                clone = (Icon)ico.Clone();
                ico.Dispose();
                Native.DestroyIcon( hIcon );
                return clone;
            }
        }

        /// <value>
        ///   Gets or sets the System.Diagnostics.ProcessWindowStyle value
        ///   that decides the initial show state of the shortcut target. Note that
        ///   ProcessWindowStyle.Hidden is not a valid property value.
        /// </value>
        public ProcessWindowStyle WindowStyle
        {
            get
            {
                int nWS;
                m_Link.GetShowCmd( out nWS );

                switch ( nWS ) {
                    case SW_SHOWMINIMIZED:
                    case SW_SHOWMINNOACTIVE:
                        return ProcessWindowStyle.Minimized;
                    case SW_SHOWMAXIMIZED:
                        return ProcessWindowStyle.Maximized;
                    default:
                        return ProcessWindowStyle.Normal;
                }
            }
            set
            {
                int nWS;
                switch ( value ) {
                    case ProcessWindowStyle.Normal:
                        nWS = SW_SHOWNORMAL;
                        break;
                    case ProcessWindowStyle.Minimized:
                        nWS = SW_SHOWMINNOACTIVE;
                        break;
                    case ProcessWindowStyle.Maximized:
                        nWS = SW_SHOWMAXIMIZED;
                        break;
                    default: // ProcessWindowStyle.Hidden
                        throw new ArgumentException("Unsupported ProcessWindowStyle value.");
                }
                m_Link.SetShowCmd( nWS );
            }
        }

        /// <value>
        ///   Gets or sets the hotkey for the shortcut.
        /// </value>
        public Keys Hotkey
        {
            get
            {
                short wHotkey;
                int dwHotkey;

                m_Link.GetHotkey(out wHotkey);
                //
                // Convert from IShellLink 16-bit format to Keys enumeration 32-bit value
                // IShellLink: 0xMMVK
                // Keys:  0x00MM00VK        
                //   MM = Modifier (Alt, Control, Shift)
                //   VK = Virtual key code
                //       
                dwHotkey = ((wHotkey & 0xFF00) << 8) | (wHotkey & 0xFF);
                return (Keys)dwHotkey;
            }
            set
            {
                short wHotkey;

                if ( (value & Keys.Modifiers) == 0 )
                    throw new ArgumentException("Hotkey must include a modifier key.");

                //    
                // Convert from Keys enumeration 32-bit value to IShellLink 16-bit format
                // IShellLink: 0xMMVK
                // Keys:  0x00MM00VK        
                //   MM = Modifier (Alt, Control, Shift)
                //   VK = Virtual key code
                //       
                wHotkey = unchecked((short) ( ((int) (value & Keys.Modifiers) >> 8) | (int) (value & Keys.KeyCode) ));
                m_Link.SetHotkey( wHotkey );

            }
        }

        /// <summary>
        ///   Saves the shortcut to disk.
        /// </summary>
        public void Save()
        {
            IPersistFile pf = (IPersistFile) m_Link;
            pf.Save( m_sPath, true );
        }

        /// <summary>
        ///   Returns a reference to the internal ShellLink object,
        ///   which can be used to perform more advanced operations
        ///   not supported by this wrapper class, by using the
        ///   IShellLink interface directly.
        /// </summary>
        public object ShortcutNative
        {
            get { return m_Link; }
        }

#region Native Win32 API functions
        private class Native
        {
            [DllImport("shell32.dll", CharSet=CharSet.Auto)]
            public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

            [DllImport("user32.dll")]
            public static extern bool DestroyIcon(IntPtr hIcon);
        }
#endregion

    }
    //
    //  END TT#1653 - GRT - Create new class to replace shortcut code in the installerto allow for VS2010
    //
}
