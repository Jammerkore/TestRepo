using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        private string _modifypath;
        public string ModifyPath
        {
            get { return _modifypath; }
            set { _modifypath = value; }
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

        private string _urlinfoabout;

        private string _urlupdateinfo;

        private int _version;
        public int Version
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
        string keyName = this.ProductCode.ToString();
        string[] productFolders = hkInstallProperties.GetSubKeyNames();
        int keycount = 0;
        using (RegistryKey key = hkInstallProperties.OpenSubKey(keyName, true))
        {
            //  Find the Application's key
            foreach (String p in productFolders)
            {
                RegistryKey installProperties = hkInstallProperties.OpenSubKey(p + @"\InstallProperties");
                if (installProperties != null)
                {
                    //  keycount will save us from traversing the registry again
                    keycount++;
                    string displayName = (string)installProperties.GetValue("DisplayName");
                    if ((displayName != null) && (displayName.Contains(strApplication)))
                    {
                        //  delete this key off the main branch
                        hkInstallProperties.DeleteSubKeyTree(p);
                        keycount--;
                    }
                }
            }
        }

        //  look in Folders
        //  if present, remove
        //      there is only a value stored here
        //      this call will not throw an exception on fail
        //  if this is the last application then remove this value as well
        if ((strApplication == null) || (keycount <= 0)) { hkFolders.DeleteValue(this.InstallFolder.name, false); }      

        //  look in Uninstall
        //  if present, remove
        //  if there are no other keys
        if (keycount <= 0)
        {
            using (RegistryKey key = hkUninstaller.OpenSubKey(keyName, true))
            {
                //  if the key exists, delete the entire tree
                if (key.OpenSubKey(keyName) != null) { key.DeleteSubKeyTree(keyName); }
            }
        }
        //
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
        //
        //  add new keys
        //
        //  folders
        //if (hkFolders.GetValue(this.InstallFolder.name) == null)
        //{
        //    hkFolders.SetValue(this.InstallFolder.name, "", RegistryValueKind.String);
        //}
        //
        ////  InstallProperties
        //RegistryKey _productcode = hkInstallProperties.CreateSubKey(ProductCode.ToString());
        ////      create subkeys
        ////          Features
        ////          InstallProperties
        ////          Patches
        ////          Usage
        //_productcode.CreateSubKey("Features");
        //_productcode.CreateSubKey("InstallProperties");
        //_productcode.CreateSubKey("Patches");
        //_productcode.CreateSubKey("Usage");
        //
        //  Uninstaller

        foreach (String s in hkCurrentVersion.GetSubKeyNames())
        {
            Console.WriteLine(s);
        }

        RegistryKey _uninstaller = hkUninstaller.CreateSubKey("{" + ProductCode.ToString().ToUpper() + "}");
        //RegistryKey _installproperties = _productcode.OpenSubKey("InstallProperties");
        //  use the scruct list to create the values needed here
        //  assign values to the key from the struct
        _uninstaller.SetValue("DisplayName", _uninstallkey.DisplayName);
        _uninstaller.SetValue("DisplayVersion", _uninstallkey.DisplayVersion);
        _uninstaller.SetValue("EstimatedSize", _uninstallkey.EstimatedSize);
        _uninstaller.SetValue("InstallDate", _uninstallkey.InstallDate);
        _uninstaller.SetValue("InstallLocation", _uninstallkey.InstallLocation);
        _uninstaller.SetValue("InstallSource", _uninstallkey.InstallSource);
        _uninstaller.SetValue("InstallSource", _uninstallkey.Language);
        _uninstaller.SetValue("ModifyPath", _uninstallkey.ModifyPath);
        _uninstaller.SetValue("Publisher", _uninstallkey.Publisher);
        _uninstaller.SetValue("UninstallString", _uninstallkey.UninstallString);
        _uninstaller.SetValue("Version", _uninstallkey.Version);
        _uninstaller.SetValue("VersionMajor", _uninstallkey.VersionMajor);
        _uninstaller.SetValue("VersionMinor", _uninstallkey.VersionMinor);
        _uninstaller.SetValue("WindowsInstaller", _uninstallkey.WindowsInstaller);
        //  Empty values
        _uninstaller.SetValue("AuthorizedCDFPrefix","");
        _uninstaller.SetValue("Comments","");
        _uninstaller.SetValue("Contact","");
        _uninstaller.SetValue("HelpLink","");
        _uninstaller.SetValue("HelpTelephone","");
        _uninstaller.SetValue("Readme","");
        _uninstaller.SetValue("Size","");
        _uninstaller.SetValue("URLInfoAbout","");
        _uninstaller.SetValue("URLUpdateInfo","");

        //FieldInfo[] fields = ((Uninstall)_uninstallkey).GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        //foreach (FieldInfo field in fields)
        //{
        //    //MessageBox.Show(field.GetValue(_uninstallkey).ToString());
        //    _uninstaller.SetValue(field.Name.ToString(), field.GetValue(_uninstallkey));
        //    //_installproperties.SetValue(field.Name.ToString(), field.GetValue(_uninstallkey));
        //}
    }
}
