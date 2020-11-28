using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Win32;

public class RegistryOperations
{
    //  BEGIN TT#1283 - Windows® 7 Client Software Logo
    //
    //  Registry Operations
    //
    //  Class to construct, insert registry entries
    //    
    //  Emulate Install, Uninstall, Modify Operations
    //      yes, I know I am reinventing the wheel - go with it
    //

    //  HLKM and such
    static internal RegistryKey hklm = Registry.LocalMachine;
    static internal RegistryKey hkSoftware = hklm.OpenSubKey("Software");
    static internal RegistryKey hkMicrosoft = hkSoftware.OpenSubKey("Microsoft");
    static internal RegistryKey hkWindows = hkMicrosoft.OpenSubKey("Windows");
    static internal RegistryKey hkCurrentVersion = hkWindows.OpenSubKey("CurrentVersion");
    static internal RegistryKey hkFolders = hkCurrentVersion.OpenSubKey("\\Installer\\Folders");

    //  hkInstallProperties + "\\" + [Product Code] + "\\" + InstallProperties
    static internal RegistryKey hkInstallProperties = hkCurrentVersion.OpenSubKey("\\Installer\\UserData\\S-1-5-18\\Products");
    static internal RegistryKey hkUninstaller = hkCurrentVersion.OpenSubKey("Uninstall", true);

    //  Product Code
    private Guid ProductCode = new Guid("{72C44ADC-77FB-4D17-B5F0-71CC45D95F16}");

    //  Package Code
    //      this package code does not change
    private Guid PackageCode = new Guid("{72C44ADC-77FB-4D17-B5F0-71CC45D95F16}");

    //  Registry struct
    public struct Uninstall
    {
        private string _authorizedcdfprefix;

        private string _comments;

        private string _contact;

        private string _displayname;
        public string DisplayName
        {
            get { return _displayname; }
            set { _displayname = value; }
        }

        private string _displayicon;
        public string DisplayIcon
        {
            get { return _displayicon; }
            set { _displayicon = value; }
        }

        private string _displayversion;
        public string DisplayVersion
        {
            get { return _displayversion; }
            set { _displayversion = value; }
        }

        private int _estimatedsize;
        public int EstimatedSize
        {
            get { return _estimatedsize; }
            set { _estimatedsize = value; }
        }

        private string _helplink;

        private string _helptelephone;

        private string _installdate;
        public string InstallDate
        {
            get { return _installdate; }
            set { _installdate = value; }
        }
        
        private string _installsource;
        public string InstallSource
        {
            get { return _installsource; }
            set { _installsource = value; }
        }

        private string _installlocation;
        public string InstallLocation
        {
            get { return _installlocation; }
            set { _installlocation = value; }
        }

        private int _language;
        public int Language
        {
            get { return _language; }
            set { _language = value; }
        }

        private string _publisher;
        public string Publisher
        {
            get { return _publisher; }
            set { _publisher = value; }
        }

        private string _readme;

        private string _settingsidentifier;

        private string _uninstallstring;
        public string UninstallString
        {
            get { return _uninstallstring; }
            set { _uninstallstring = value; }
        }

        private string _uninstalllocation;
        public string UninstallLocation
        {
            get { return _uninstalllocation; }
            set { _uninstalllocation = value; }
        }

        private string _urlinfoabout;

        private string _urlupdateinfo;

        private string _version;
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private int _versionmajor;
        public int VersionMajor
        {
            get { return _versionmajor; }
            set { _versionmajor = value; }
        }

        private int _versionminor;
        public int VersionMinor
        {
            get { return _versionminor; }
            set { _versionminor = value; }
        }

        private bool _windowsinstaller;
        public bool WindowsInstaller
        {
            get { return _windowsinstaller; }
            set { _windowsinstaller = value; }
        }
    }

    //  class exception
    private Exception _error;
    public Exception error
    {
        get { return _error; }
        set { _error = value; }
    }

    //  the uninstall registry struct
    public Uninstall Uninstaller = new Uninstall();

    // this is the installer/folder key name
    public struct InstallerFolder
    {
        private string _name;
        private string _data;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string data
        {
            get { return _data; }
            set { _data = value; }
        }
    }

    //  this registry struct
    public InstallerFolder InstallFolder = new InstallerFolder();

    public RegistryOperations()
	{
	}

    public RegistryOperations(InstallerFolder _installerfolder, Uninstall _uninstaller)
    {
        this.InstallFolder = _installerfolder;
        this.Uninstaller = _uninstaller;
    }

    //  Modify
    //      which package are we modifying?
    public void ModifyInstallation(Uninstall _uninstallkey, string strApplication)
    {
        // Go to InstallApplication
        InstallApplication(_uninstallkey, strApplication);
    }

    //  Uninstall
    //      check for existing keys
    //      if missing, move on
    //      if present, remove
    //      allow uninstallation of a single service or the application
    public void UninstallApplication()
    {
        //  uninstall all
        UninstallApplication(null);
    }
    public void UninstallApplication(string strApplication)
    {     
        //
        //  look in InstallProperties
        //  if present, remove
        string keyName = "{" + ProductCode.ToString().ToUpper() + "}";

        try
        {
            //  if the MIDRetailInc Key DOES NOT exist then delete
            if (hkSoftware.OpenSubKey("MIDRetailInc").SubKeyCount == 0)
            {
                using (RegistryKey key = hkUninstaller.OpenSubKey(keyName, true))
                {
                    //  if the key exists, delete the entire tree
                    if (hkUninstaller.OpenSubKey(keyName, true) != null)
                    {
                        key.DeleteValue("DisplayIcon");
                        key.DeleteValue("DisplayName");
                        key.DeleteValue("DisplayVersion");
                        key.DeleteValue("EstimatedSize");
                        key.DeleteValue("InstallDate");
                        key.DeleteValue("InstallLocation");
                        key.DeleteValue("InstallSource");
                        key.DeleteValue("Language");
                        key.DeleteValue("Publisher");
                        key.DeleteValue("UninstallString");
                        key.DeleteValue("UninstallLocation");
                        key.DeleteValue("Version");
                        key.DeleteValue("VersionMajor");
                        key.DeleteValue("VersionMinor");
                        key.DeleteValue("WindowsInstaller");
                        key.DeleteValue("NoModify");
                        key.DeleteValue("NoRepair");

                        //  Empty values
                        key.DeleteValue("AuthorizedCDFPrefix");
                        key.DeleteValue("Comments");
                        key.DeleteValue("Contact");
                        key.DeleteValue("HelpLink");
                        key.DeleteValue("HelpTelephone");
                        key.DeleteValue("Readme");
                        key.DeleteValue("Size");
                        key.DeleteValue("URLInfoAbout");
                        key.DeleteValue("URLUpdateInfo");
                        //
                        hkUninstaller.DeleteSubKey(keyName);
                    }
                    else
                    {
                        //throw new Exception("Unable to Open UNINSTALL Key [" + hkUninstaller.Name.ToString() + "\\" + keyName + "]");
                    }
                }
            }
        }
        catch (Exception ex)
        { throw ex; }
    }

    //  Install
    //      add all keys
    public void InstallApplication(Uninstall _uninstkey)
    {
        InstallApplication(_uninstkey, null);
    }

    //  
    //  create the uninstall struct for registry loading
    //  create the install folder
    //  
    //
    public void InstallApplication(Uninstall _uninstallkey, string strApplication)
    { 
        try
        {
            //
            //  add new keys
            //
            //  Product Version
            //  
            string assemblyName = _uninstallkey.InstallSource + @"Install Files\Client\MIDRetail.Windows.dll";
            FileVersionInfo fvi;
            string productVersion;
            string productMinor;
            string productMajor;
            if (File.Exists(assemblyName))
            {
                fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyName);
                productVersion = fvi.ProductVersion;
                productMajor = fvi.FileMajorPart.ToString();
                productMinor = fvi.FileMinorPart.ToString();
            }
            else
            {
                productVersion = "1.0.0";
                productMajor = "0";
                productMinor = "0";
            }

            RegistryKey _uninstaller = hkUninstaller.CreateSubKey("{" + ProductCode.ToString().ToUpper() + "}");
            //  use the scruct list to create the values needed here
            //  assign values to the key from the struct
            _uninstaller.SetValue("DisplayIcon", _uninstallkey.DisplayIcon, RegistryValueKind.String);
            _uninstaller.SetValue("DisplayName", _uninstallkey.DisplayName, RegistryValueKind.String);
            _uninstaller.SetValue("DisplayVersion", _uninstallkey.DisplayVersion, RegistryValueKind.String);
            _uninstaller.SetValue("EstimatedSize", _uninstallkey.EstimatedSize, RegistryValueKind.DWord);
            _uninstaller.SetValue("InstallDate", _uninstallkey.InstallDate, RegistryValueKind.String);
            _uninstaller.SetValue("InstallLocation", _uninstallkey.InstallLocation, RegistryValueKind.String);
            _uninstaller.SetValue("InstallSource", _uninstallkey.InstallSource, RegistryValueKind.String);
            _uninstaller.SetValue("Language", _uninstallkey.Language, RegistryValueKind.DWord);
            _uninstaller.SetValue("Publisher", _uninstallkey.Publisher, RegistryValueKind.String);
            _uninstaller.SetValue("UninstallString", _uninstallkey.InstallSource + "MIDRetailInstaller.exe", RegistryValueKind.ExpandString);
            _uninstaller.SetValue("UninstallLocation", _uninstallkey.UninstallLocation, RegistryValueKind.String);
            _uninstaller.SetValue("Version", productVersion);
            _uninstaller.SetValue("VersionMajor", productMajor);
            _uninstaller.SetValue("VersionMinor", productMinor);
            _uninstaller.SetValue("WindowsInstaller", _uninstallkey.WindowsInstaller, RegistryValueKind.DWord);
            _uninstaller.SetValue("NoModify", 1, RegistryValueKind.DWord);
            _uninstaller.SetValue("NoRepair", 1, RegistryValueKind.DWord);
            //  Empty values
            _uninstaller.SetValue("AuthorizedCDFPrefix", "", RegistryValueKind.String);
            _uninstaller.SetValue("Comments", "", RegistryValueKind.String);
            _uninstaller.SetValue("Contact", "", RegistryValueKind.String);
            _uninstaller.SetValue("HelpLink", "", RegistryValueKind.ExpandString);
            _uninstaller.SetValue("HelpTelephone","", RegistryValueKind.String);
            _uninstaller.SetValue("Readme", "", RegistryValueKind.String);
            _uninstaller.SetValue("Size", "", RegistryValueKind.String);
            _uninstaller.SetValue("URLInfoAbout", "", RegistryValueKind.String);
            _uninstaller.SetValue("URLUpdateInfo", "", RegistryValueKind.String);
            }
        catch (Exception ex)
        { throw ex; }
    }
}
