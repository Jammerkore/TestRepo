using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Xml;
using MIDRetail.Encryption;
using System.Collections;
using System.Diagnostics;

namespace MIDRetailInstaller
{
    class ConfigFiles
    {
        DataSet install_data = null;
		// Begin TT#1668 - JSmith - Install Log
        InstallerFrame frame = null;
		// End TT#1668
        ucInstallationLog log;
        string ConfigurationFileName;
        int MaxEncryptedFieldSize = 1000;
       
        DataTable dtSubstitutions;
        DataTable dtRemovals;  // TT#581-MD - JSmith - Configuration Cleanup

        public Hashtable htEncryptedFields = null;

        MIDEncryption encryption = new MIDEncryption();

        // Begin TT#1668 - JSmith - Install Log
		//public ConfigFiles(DataSet p_install_data, ucInstallationLog p_log)
        public ConfigFiles(InstallerFrame p_frame, DataSet p_install_data, ucInstallationLog p_log)
		// End TT#1668
        {
		    // Begin TT#1668 - JSmith - Install Log
            frame = p_frame;
			// End TT#1668
            install_data = p_install_data;
            log = p_log;
            ConfigurationFileName = ConfigurationManager.AppSettings["MIDSettings_config"].ToString();
            MaxEncryptedFieldSize = Convert.ToInt32(ConfigurationManager.AppSettings["EncryptedMaxFieldSize"].ToString());
            dtSubstitutions = install_data.Tables[InstallerConstants.cTable_Substitution];
            dtRemovals = install_data.Tables[InstallerConstants.cTable_Remove];  // TT#581-MD - JSmith - Configuration Cleanup
            GetEncryptedFields();
        }

        private void GetEncryptedFields()
        {
            htEncryptedFields = new Hashtable();

            string field = ConfigurationManager.AppSettings["EncryptedFields"].ToString();
            string[] fields = field.Split(';');
            foreach (string f in fields)
            {
                htEncryptedFields.Add(f.Trim(), null);
            }
        }

        public void MakeConfigBackup(string installation_dir)
        {
		    // Begin TT#581-MD - JSmith - Configuration Cleanup
			//CopyConfigFiles(installation_dir);
            CopyConfigFiles(installation_dir, true);
			// End TT#581-MD - JSmith - Configuration Cleanup
        }

        // Begin TT#581-MD - JSmith - Configuration Cleanup
        public void MakeConfigBackup(string installation_dir, bool blRecursive)
        {
            CopyConfigFiles(installation_dir, blRecursive);
        }
		// End TT#581-MD - JSmith - Configuration Cleanup

        // Begin TT#581-MD - JSmith - Configuration Cleanup
		//private void CopyConfigFiles(string sDir)
        private void CopyConfigFiles(string sDir, bool blRecursive)
		// End TT#581-MD - JSmith - Configuration Cleanup
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*.config"))
                {
                    // make sure the file is not read only
                    if (File.Exists(f + InstallerConstants.cBackupExtension))
                    {
                        File.SetAttributes(f + InstallerConstants.cBackupExtension, File.GetAttributes(f + InstallerConstants.cBackupExtension) & ~(FileAttributes.ReadOnly));
                    }
                    System.IO.File.Copy(f, f + InstallerConstants.cBackupExtension, true);
                }

                // Begin TT#581-MD - JSmith - Configuration Cleanup
                //foreach (string d in Directory.GetDirectories(sDir))
                //{
                //    CopyConfigFiles(d);
                //}				
                if (blRecursive)
                {
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        CopyConfigFiles(d, blRecursive);
                    }
                }
				// End TT#581-MD - JSmith - Configuration Cleanup
            }
            catch 
            {
                throw;
            }
        }
        public void CopyOldConfigFiles(InstallerFrame p_frame, string installation_dir)
        {
            string upgradeFilesLocation = ConfigurationManager.AppSettings["UpgradeFilesLocation"].ToString();
            if (upgradeFilesLocation != null &&
                Directory.Exists(upgradeFilesLocation))
            {
                CopyOldConfigFile(p_frame, installation_dir, upgradeFilesLocation);
            }
        }
        private void CopyOldConfigFile(InstallerFrame p_frame, string sDir, string sUpgradeLocation)
        {
            string oldFileLocation = null;
            FileInfo fileInfo;
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*.config"))
                {
                    if (File.Exists(f + InstallerConstants.cBackupExtension))
                    {
                        File.SetAttributes(f + InstallerConstants.cBackupExtension, File.GetAttributes(f + InstallerConstants.cBackupExtension) & ~(FileAttributes.ReadOnly));
                    }
                    fileInfo = new FileInfo(f);
                    string name = p_frame.GetOldRebrandName(fileInfo.Name);
                    if (name == null)
                    {
                        name = fileInfo.Name;
                    }
                    oldFileLocation = null;
                    getOldFileLocation(sUpgradeLocation, name, ref oldFileLocation);
                    if (oldFileLocation != null)
                    {
                        System.IO.File.Copy(oldFileLocation, f + InstallerConstants.cBackupExtension, true);
                    }
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    CopyOldConfigFile(p_frame, d, sUpgradeLocation);
                }
            }
            catch
            {
                throw;
            }
        }
        private void getOldFileLocation(string sDir, string sFile, ref string oldFileLocation)
        {
            FileInfo fileInfo;
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*.config"))
                {
                    fileInfo = new FileInfo(f);
                    if (fileInfo.Name == sFile)
                    {
                        oldFileLocation = f;
                       return;
                    }
                }
                if (oldFileLocation == null)
                {
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        getOldFileLocation(d, sFile, ref oldFileLocation);
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        public void UpgradeConfigFiles(string installation_dir, string sRootInstallFolder, bool bIgnoreParmsWithLocalPaths)
        {
		    // Begin TT#581-MD - JSmith - Configuration Cleanup
			//MergeConfigFiles(installation_dir, sRootInstallFolder, bIgnoreParmsWithLocalPaths);
            MergeConfigFiles(installation_dir, sRootInstallFolder, bIgnoreParmsWithLocalPaths, true);
			// End TT#581-MD - JSmith - Configuration Cleanup
        }

        // Begin TT#581-MD - JSmith - Configuration Cleanup
        public void UpgradeConfigFiles(string installation_dir, string sRootInstallFolder, bool bIgnoreParmsWithLocalPaths, bool blRecursive)
        {
            MergeConfigFiles(installation_dir, sRootInstallFolder, bIgnoreParmsWithLocalPaths, blRecursive);
        }
		// End TT#581-MD - JSmith - Configuration Cleanup

        // Begin TT#581-MD - JSmith - Configuration Cleanup
		//private void MergeConfigFiles(string sDir, string sRootInstallFolder, bool bIgnoreParmsWithLocalPaths)
        private void MergeConfigFiles(string sDir, string sRootInstallFolder, bool bIgnoreParmsWithLocalPaths, bool blRecursive)
		// End TT#581-MD - JSmith - Configuration Cleanup
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*.config"))
                {
                    if (File.Exists(f + InstallerConstants.cBackupExtension))
                    {
                        MergeConfigFile(f, sRootInstallFolder, bIgnoreParmsWithLocalPaths);
                    }
                }

                // Begin TT#581-MD - JSmith - Configuration Cleanup
                //foreach (string d in Directory.GetDirectories(sDir))
                //{
                //    MergeConfigFiles(d, sRootInstallFolder, bIgnoreParmsWithLocalPaths);
                //}				
                if (blRecursive)
                {
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        MergeConfigFiles(d, sRootInstallFolder, bIgnoreParmsWithLocalPaths, blRecursive);
                    }
                }
				// End TT#581-MD - JSmith - Configuration Cleanup
            }
            catch
            {
                throw;
            }
        }

        private void MergeConfigFile(string newFile, string sRootInstallFolder, bool bIgnoreParmsWithLocalPaths)
        {
            string originalFile;
            XmlDocument newDoc, originalDoc;
            string strParent = InstallerConstants.cParent_AppSettings;
            string strLookupType = InstallerConstants.cLookupType_Child;
            try
            {
                originalFile = newFile + InstallerConstants.cBackupExtension;
                newDoc = GetXmlDocument(newFile);
                originalDoc = GetXmlDocument(originalFile);

                XmlNode appSettingsNode = originalDoc.SelectSingleNode(InstallerConstants.cParent_ConfigurationAppSettings);

                if (appSettingsNode == null)
                {
                    appSettingsNode = originalDoc.SelectSingleNode(InstallerConstants.cParent_AppSettings);
                }

                //Begin TT#1053 - The MIDSetting.config location is being overwritten in the MIDRetail.exe.config - apicchetti - 12/30/2010
                // Begin TT#1090 - JSmith - Null reference error when upgrading with prior version config files
                //UpdateKey(newDoc, "file", appSettingsNode.Attributes["file"].Value.ToString());
                if (!bIgnoreParmsWithLocalPaths && KeyExists(newDoc, strParent, strLookupType, "file"))
                {
				    // Begin TT#1668 - JSmith - Install Log
					//UpdateKey(newDoc, "file", appSettingsNode.Attributes["file"].Value.ToString());
                    UpdateKey(newFile, newDoc, strParent, strLookupType, "file", appSettingsNode.Attributes["file"].Value.ToString());
					// End TT#1668
                }
                // End TT#1090
                //End TT#1053 - The MIDSetting.config location is being overwritten in the MIDRetail.exe.config - apicchetti - 12/30/2010

                // Attempt to locate the requested setting.
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        if (!KeyExists(newDoc, strParent, strLookupType, childNode.Attributes["key"].Value))
                        {
						    // Begin TT#1668 - JSmith - Install Log
							//AddKey(newDoc, childNode.Attributes["key"].Value.ToString(), childNode.Attributes["value"].Value.ToString());
                            AddKey(newFile, newDoc, strParent, strLookupType, childNode.Attributes["key"].Value.ToString(), childNode.Attributes["value"].Value.ToString());
							// End TT#1668
                        }
                        else if (!bIgnoreParmsWithLocalPaths || !childNode.Attributes["value"].Value.ToString().Contains(@":\"))
                        {
						    // Begin TT#1668 - JSmith - Install Log
							//UpdateKey(newDoc, childNode.Attributes["key"].Value.ToString(), childNode.Attributes["value"].Value.ToString());
                            UpdateKey(newFile, newDoc, strParent, strLookupType, childNode.Attributes["key"].Value.ToString(), childNode.Attributes["value"].Value.ToString());
							// End TT#1668
                        }
                    }
                }

                string value, key;
                DataTable dtConfiguration = install_data.Tables[InstallerConstants.cTable_Configuration];
                string strConfigFile = Path.GetFileName(newFile);
                DataRow[] drConfigRows = dtConfiguration.Select("config_file='" + strConfigFile + "' or config_file='ALL'", "setting ASC");

                if (drConfigRows != null
                    && drConfigRows.Length > 0)
                {
                    foreach (DataRow dr in drConfigRows)
                    {
                        if (dr.Table.Columns.Contains(InstallerConstants.cConfigurationField_Parent)
                            && !dr.IsNull(InstallerConstants.cConfigurationField_Parent))
                        {
                            strParent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                        }
                        else
                        {
                            strParent = InstallerConstants.cParent_AppSettings;
                        }
                        if (dr.Table.Columns.Contains(InstallerConstants.cConfigurationField_LookupType)
                            && !dr.IsNull(InstallerConstants.cConfigurationField_LookupType))
                        {
                            strLookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                        }
                        else
                        {
                            strLookupType = InstallerConstants.cLookupType_Child;
                        }
                        if (strParent != InstallerConstants.cParent_AppSettings)
                        {
                            key = dr.Field<string>(InstallerConstants.cConfigurationField_Setting);
                            value = GetValue(originalDoc, strParent, strLookupType, key);
                            if (!KeyExists(newDoc, strParent, strLookupType, key))
                            {
                                AddKey(newFile, newDoc, strParent, strLookupType, key, value);
                            }
                            else if (!bIgnoreParmsWithLocalPaths || !value.Contains(@":\"))
                            {
                                UpdateKey(newFile, newDoc, strParent, strLookupType, key, value);
                            }
                        }
                    }
                }

                newDoc.Save(newFile);
                if (File.Exists(newFile + InstallerConstants.cBackupExtension))
                {
                    File.Delete(newFile + InstallerConstants.cBackupExtension);
                }
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#581-MD - JSmith - Configuration Cleanup
        // remove unwanted key values
        public Dictionary<string, string> RemoveUnneededConfigSettings(string aDocLocation)
        {
            Dictionary<string, string> RemovedConfigValues = new Dictionary<string, string>();
            XmlDocument doc;
            string strParent;
            string strLookupType;
            string strKey;
            string strValue;
            bool blFileUpdated = false;
            try
            {

                doc = GetXmlDocument(aDocLocation);

                foreach (DataRow dr in dtRemovals.Rows)
                {
                    if (aDocLocation.Contains(dr.Field<string>(InstallerConstants.cConfigurationField_Config_File)))
                    {
                        strKey = dr.Field<string>(InstallerConstants.cConfigurationField_Setting);
                        strParent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent);
                        strLookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType);
                        if (KeyExists(doc, strParent, strLookupType, strKey))
                        {
                            strValue = GetValue(doc, strParent, strLookupType, strKey);
                            RemovedConfigValues.Add(strKey, strValue);
                            DeleteKey(doc, strParent, strLookupType, strKey);
                            blFileUpdated = true;
                        }
                    }
                }

                if (blFileUpdated)
                {
                    doc.Save(aDocLocation);
                }

            }
            catch
            {
            }

            return RemovedConfigValues;
        }


        public void ApplyRemovedGlobalConfigRows(Dictionary<string, string> diConfigChanges, Dictionary<string, string> diServiceLocation)
        {
            string configFile = null;
            string strDirectory;
            string strParent = InstallerConstants.cParent_AppSettings;
            string strLookupType = InstallerConstants.cLookupType_Child;

            XmlDocument doc;
            ArrayList alFileLocation;

            if (diConfigChanges == null ||
                diConfigChanges.Count == 0)
            {
                return;
            }

            DataTable dtConfiguration = install_data.Tables[InstallerConstants.cTable_Configuration];

            foreach (KeyValuePair<string, string> keyPair in diConfigChanges)
            {
                DataRow[] drConfigRows = dtConfiguration.Select("setting = '" + keyPair.Key.Trim() + "'");
                foreach (DataRow dr in drConfigRows)
                {
                    configFile = Convert.ToString(dr[InstallerConstants.cConfigurationField_Config_File]);
                    strParent = Convert.ToString(dr[InstallerConstants.cConfigurationField_Parent]);
                    strLookupType = Convert.ToString(dr[InstallerConstants.cConfigurationField_LookupType]);
                    alFileLocation = new ArrayList();
                    foreach (KeyValuePair<string, string> serviceKeyPair in diServiceLocation)
                    {
                        if (serviceKeyPair.Key.Contains(InstallerConstants.cBatchKey))
                        {
                            strDirectory = serviceKeyPair.Value;
                        }
                        else
                        {
                            strDirectory = Directory.GetParent(serviceKeyPair.Value).ToString().Trim();
                        }
                        frame.DirSearch(strDirectory, configFile, alFileLocation);
                    }
                    foreach (string docLocation in alFileLocation)
                    {
                        if (!string.IsNullOrEmpty(docLocation))
                        {
                            doc = GetXmlDocument(docLocation);
                            if (!KeyExists(doc, strParent, strLookupType, keyPair.Key.Trim()))
                            {
                                AddKey(docLocation, doc, strParent, strLookupType, keyPair.Key.Trim(), keyPair.Value.Trim());
                            }
                            else
                            {
                                UpdateKey(docLocation, doc, strParent, strLookupType, keyPair.Key.Trim(), keyPair.Value.Trim());
                            }

                            doc.Save(docLocation);
                        }
                    }
                }
            }
        }
        // End TT#581-MD - JSmith - Configuration Cleanup

        // Begin TT#581-MD - JSmith - Configuration Cleanup
        public void ReplaceDefaultsInConfigFiles(string sDir, string sRootInstallFolder, string sGlobalConfigurationLocation, eConfigMachineBy configMachineBy, string sDrive)
        {
            ReplaceDefaultsInConfigFiles(sDir, sRootInstallFolder, sGlobalConfigurationLocation, configMachineBy, sDrive, true);
        }
		// End TT#581-MD - JSmith - Configuration Cleanup

        // Begin TT#581-MD - JSmith - Configuration Cleanup
		//public void ReplaceDefaultsInConfigFiles(string sDir, string sRootInstallFolder, string sGlobalConfigurationLocation, eConfigMachineBy configMachineBy, string sDrive)
        public void ReplaceDefaultsInConfigFiles(string sDir, string sRootInstallFolder, string sGlobalConfigurationLocation, eConfigMachineBy configMachineBy, string sDrive, bool blRecursive)
		// End TT#581-MD - JSmith - Configuration Cleanup
        {
            try
            {
                // Get configuration settings for file
                DataTable dtConfiguration = install_data.Tables[InstallerConstants.cTable_Configuration];

                foreach (string f in Directory.GetFiles(sDir, "*.config"))
                {
                    ReplaceDefaultsInConfigFile(f, sRootInstallFolder, sGlobalConfigurationLocation, configMachineBy, sDrive, dtConfiguration);
                }

                // Begin TT#581-MD - JSmith - Configuration Cleanup
                //foreach (string d in Directory.GetDirectories(sDir))
                //{
                //    ReplaceDefaultsInConfigFiles(d, sRootInstallFolder, sGlobalConfigurationLocation, configMachineBy, sDrive);
                //}				
                if (blRecursive)
                {
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        ReplaceDefaultsInConfigFiles(d, sRootInstallFolder, sGlobalConfigurationLocation, configMachineBy, sDrive, blRecursive);
                    }
                }
				// End TT#581-MD - JSmith - Configuration Cleanup
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#644-MD - JSmith - Modify install values for several configuration settings
        public void ConvertRelieveHeadersConfigSettings(string aDocLocation)
        {
            XmlDocument doc;
            string strKey;
            string strValue;
            bool blFileUpdated = false;
            string strParent = InstallerConstants.cParent_AppSettings;
            string strLookupType = InstallerConstants.cLookupType_Child;
            try
            {

                doc = GetXmlDocument(aDocLocation);

                strKey = "HeaderSQL";
                if (KeyExists(doc, strParent, strLookupType, strKey))
                {
                    strValue = GetValue(doc, strParent, strLookupType, strKey);
                    if (strValue.ToUpper().Contains("HDR_ID") &&
                        strValue.ToUpper().Contains("BN_ID"))
                    {
                        string fileLocation = aDocLocation.Replace("RelieveHeaders.exe.config", "RelieveHeaders.sql");
                        UpdateKey(aDocLocation, doc, strParent, strLookupType, strKey, fileLocation);

                        blFileUpdated = true;
                        StreamWriter sqlFile = new StreamWriter(fileLocation);
                        sqlFile.WriteLine(strValue);
                        sqlFile.Close();
                    }
                }


                if (blFileUpdated)
                {
                    doc.Save(aDocLocation);
                }

            }
            catch
            {
            }
        }
        // End TT#644-MD - JSmith - Modify install values for several configuration settings

        private void ReplaceDefaultsInConfigFile(string newFile, string sRootInstallFolder, string sGlobalConfigurationLocation, eConfigMachineBy configMachineBy, string sDrive, DataTable dtConfiguration)
        {
            XmlDocument doc;
            bool blFileUpdated = false;
            XmlNode appSettingsNode;
            string machine;

            try
            {
                // make sure the file is not read only
                File.SetAttributes(newFile, File.GetAttributes(newFile) & ~(FileAttributes.ReadOnly));

                if (configMachineBy == eConfigMachineBy.IP)
                {
                    machine = GetIPAddress();
                }
                else
                {
                    machine = GetMachineName();
                }
                doc = GetXmlDocument(newFile);

                string strConfigFile = Path.GetFileName(newFile);
                DataRow[] drConfigRows = dtConfiguration.Select("config_file='" + strConfigFile + "' or config_file='ALL'", "setting ASC");

                appSettingsNode = doc.SelectSingleNode(InstallerConstants.cParent_ConfigurationAppSettings);
                if (appSettingsNode != null)
                {
				    // Begin TT#1668 - JSmith - Install Log
					//blFileUpdated = ReplaceDefaultsInConfigFile(doc, appSettingsNode, sRootInstallFolder, sGlobalConfigurationLocation, machine, sDrive);
                    blFileUpdated = ReplaceDefaultsInConfigFile(newFile, doc, appSettingsNode, sRootInstallFolder, sGlobalConfigurationLocation, machine, sDrive, drConfigRows);
					// End TT#1668
                }
                else
                {
                    appSettingsNode = doc.SelectSingleNode(InstallerConstants.cParent_AppSettings);
                    if (appSettingsNode != null)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//blFileUpdated = ReplaceDefaultsInConfigFile(doc, appSettingsNode, sRootInstallFolder, sGlobalConfigurationLocation, machine, sDrive);
                        blFileUpdated = ReplaceDefaultsInConfigFile(newFile, doc, appSettingsNode, sRootInstallFolder, sGlobalConfigurationLocation, machine, sDrive, drConfigRows);
						// End TT#1668
                    }
                }

                if (blFileUpdated)
                {
                    doc.Save(newFile);
                }
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#1668 - JSmith - Install Log
	    //private bool ReplaceDefaultsInConfigFile(XmlDocument doc, XmlNode appSettingsNode, string sRootInstallFolder, string sGlobalConfigurationLocation, string machine, string sDrive)
        private bool ReplaceDefaultsInConfigFile(string fileName, XmlDocument doc, XmlNode appSettingsNode, string sRootInstallFolder, string sGlobalConfigurationLocation, string machine, string sDrive, DataRow[] drConfigRows)
		// End TT#1668
        {
            string value, key;
            bool blFileUpdated = false;
            string strParent = InstallerConstants.cParent_AppSettings;
            string strLookupType = InstallerConstants.cLookupType_Child;

            try
            {
                if (AttributeExists(appSettingsNode, "file"))
                {
                    key = "file";
                    value = appSettingsNode.Attributes["file"].Value.ToString();
                    if (sGlobalConfigurationLocation != null)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//ReplaceValueInConfigLine(ref value, sGlobalConfigurationLocation, machine, sDrive);
                        ReplaceValueInConfigLine(fileName, key, ref value, sGlobalConfigurationLocation, machine, sDrive);
						// End TT#1668
                        UpdateAttribute(appSettingsNode, key, sGlobalConfigurationLocation);
                        blFileUpdated = true;
                    }
                    else
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//if (ReplaceValueInConfigLine(ref value, sRootInstallFolder, machine, sDrive))
                        if (ReplaceValueInConfigLine(fileName, key, ref value, sRootInstallFolder, machine, sDrive))
						// End TT#1668
                        {
                            UpdateAttribute(appSettingsNode, key, value);
                            blFileUpdated = true;
                        }
                    }
                }

                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        key = childNode.Attributes["key"].Value.ToString();
                        value = childNode.Attributes["value"].Value.ToString();
						// Begin TT#1668 - JSmith - Install Log
						//if (ReplaceValueInConfigLine(ref value, sRootInstallFolder, machine, sDrive))
                        //{
                            //UpdateKey(doc, key, value);
                            //blFileUpdated = true;
                        //}
                        if (ReplaceValueInConfigLine(fileName, key, ref value, sRootInstallFolder, machine, sDrive))
                        {
                            UpdateKey(fileName, doc, strParent, strLookupType, key, value);
                            blFileUpdated = true;
                        }
						// End TT#1668
                    }
                }

                if (drConfigRows != null
                    && drConfigRows.Length > 0)
                {
                    foreach (DataRow dr in drConfigRows)
                    {
                        if (dr.Table.Columns.Contains(InstallerConstants.cConfigurationField_Parent)
                            && !dr.IsNull(InstallerConstants.cConfigurationField_Parent))
                        {
                            strParent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                        }
                        else
                        {
                            strParent = InstallerConstants.cParent_AppSettings;
                        }
                        if (dr.Table.Columns.Contains(InstallerConstants.cConfigurationField_LookupType)
                            && !dr.IsNull(InstallerConstants.cConfigurationField_LookupType))
                        {
                            strLookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                        }
                        else
                        {
                            strLookupType = InstallerConstants.cLookupType_Child;
                        }
                        if (strParent != InstallerConstants.cParent_AppSettings)
                        {
                            key = dr.Field<string>(InstallerConstants.cConfigurationField_Setting);
                            if (KeyExists(doc, strParent, strLookupType, key))
                            {
                                value = GetValue(doc, strParent, strLookupType, key);
                                if (ReplaceValueInConfigLine(fileName, key, ref value, sRootInstallFolder, machine, sDrive))
                                {
                                    UpdateKey(fileName, doc, strParent, strLookupType, key, value);
                                    blFileUpdated = true;
                                }
                            }
                        }
                    }
                }

                return blFileUpdated;

            }
            catch
            {
                throw;
            }
        }

        // Begin TT#1668 - JSmith - Install Log
		//private bool ReplaceValueInConfigLine(ref string value, string sRootInstallFolder, string machine, string sDrive)
        private bool ReplaceValueInConfigLine(string fileName, string key, ref string value, string sRootInstallFolder, string machine, string sDrive)
		// End TT#1668
        {
            string oldValue, newValue;
            bool blKeyUpdated = false;
			// Begin TT#1668 - JSmith - Install Log
            string msg;
			// End TT#1668
            try
            {
                foreach (DataRow dr in dtSubstitutions.Rows)
                {
                    oldValue = ReplaceSystemVariables(dr.Field<string>("old"), sRootInstallFolder, machine, sDrive);
                    newValue = ReplaceSystemVariables(dr.Field<string>("new"), sRootInstallFolder, machine, sDrive);
                    if (value.Contains(oldValue))
                    {
                        value = value.Replace(oldValue, newValue);
                        blKeyUpdated = true;
                    }
                }
                return blKeyUpdated;
            }
            catch
            {
                throw;
            }
        }

        private string ReplaceSystemVariables(string line, string sRootInstallFolder, string machine, string sDrive)
        {
            string uncReplacePath = "\\\\" + machine + "\\" + sRootInstallFolder.Substring(3);

            line = line.Replace("%machine%", machine);
            line = line.Replace("%installdir%", sRootInstallFolder);
            line = line.Replace("%unc_installdir%", uncReplacePath);
            line = line.Replace("%drive%", sDrive);
            
            return line;
        }

        public bool MIDSettings_config_Exists(string installation_dir)
        {
            string strConfigFile = ConfigurationManager.AppSettings["MIDSettings_config"].ToString().Trim();
            return File.Exists(installation_dir + @"\" + strConfigFile);
        }

        public void CopyMIDSettings_config(string application_dir, string installation_dir)
        {
            System.IO.File.Copy(application_dir + @"\Install Files\ConfigFile\" + ConfigurationFileName, installation_dir + @"\" + ConfigurationFileName);
        }

        public string GetConfigValue(string strParent, string strLookupType, string setting, string ID, XmlDocument config, XmlDocument MIDSettings, DataRow dr, out eConfigValueFrom from)
        {
            string value;
            if (setting == "file")
            {
                from = eConfigValueFrom.Config;
                value = GetMIDSettingsLocation(config);
            }
            else if (MIDSettings != null &&
                KeyExists(MIDSettings, strParent, strLookupType, setting))
            {
                from = eConfigValueFrom.MIDSettings;
                value = GetValue(MIDSettings, strParent, strLookupType, setting);
            }
            else if (config != null &&
                KeyExists(config, strParent, strLookupType, setting))
            {
                from = eConfigValueFrom.Config;
                value = GetValue(config, strParent, strLookupType, setting);
            }
            else
            {
                from = eConfigValueFrom.Default;
                value = GetDefault(ID);
            }

            if (htEncryptedFields.ContainsKey(setting))
            {
                return encryption.Decrypt(value);
            }
            else
            {
                return value;
            }
        }

        public XmlDocument GetXmlDocument(string file)
        {
            if (!File.Exists(file))
                return null;

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return doc;
        }

        public XmlDocument GetMIDSettings(XmlDocument config)
        {
            string file = null;
            XmlNode appSettingsNode = GetSettingNode(config, InstallerConstants.cParent_AppSettings);
            if (AttributeExists(appSettingsNode, "file"))
            {
                file = GetMIDSettingsLocation(config);
                return GetXmlDocument(file);
            }
            else
            {
                return null;
            }
        }

        public string GetMIDSettingsLocation(XmlDocument config)
        {
            XmlNode appSettingsNode = GetSettingNode(config, InstallerConstants.cParent_AppSettings);
            if (AttributeExists(appSettingsNode, "file"))
            {
                return GetAttributeValue(appSettingsNode, "file");
            }
            else
            {
                return null;
            }
        }

        public void SetConfigValue(string sFile, string strParent, string strLookupType, string strKey, string strValue)
        {
            XmlDocument doc;
            try
            {
                doc = GetXmlDocument(sFile);
                if (doc != null)
                {
				    // Begin TT#1668 - JSmith - Install Log
					//SetConfigValue(doc, strKey, strValue);
                    SetConfigValue(sFile, doc, strParent, strLookupType, strKey, strValue);
					// End TT#1668
                    doc.Save(sFile);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Adds a key and value to the App.config
		// Begin TT#1668 - JSmith - Install Log
		//public void SetConfigValue(XmlDocument xmlDoc, string strKey, string strValue)
        public void SetConfigValue(string fileName, XmlDocument xmlDoc, string strParent, string strLookupType, string strKey, string strValue)
		// End TT#1668
        {
            XmlNode appSettingsNode = GetSettingNode(xmlDoc, strParent);
            strValue = strValue.Trim();
            try
            {
                if (KeyExists(xmlDoc, strParent, strLookupType, strKey))
                {
				    // Begin TT#1668 - JSmith - Install Log
				    //UpdateKey(xmlDoc, strKey, strValue);
                    UpdateKey(fileName, xmlDoc, strParent, strLookupType , strKey, strValue);
					// End TT#1668
                }
                else
                {
				    // Begin TT#1668 - JSmith - Install Log
					//AddKey(xmlDoc, strKey, strValue);
                    AddKey(fileName, xmlDoc, strParent, strLookupType , strKey, strValue);
					// End TT#1668
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Adds a key and value to the App.config
		// Begin TT#1668 - JSmith - Install Log
		//public void AddKey(XmlDocument xmlDoc, string strKey, string strValue)
        public void AddKey(string fileName, XmlDocument xmlDoc, string strParent, string strLookupType, string strKey, string strValue)
		// End TT#1668
        {
            // Begin TT#1088 - JSmith - Null reference error merging configuration files
            XmlElement newElement;
			// Begin TT#1668 - JSmith - Install Log
            string msg;
			// End TT#1668
            // End TT#1088
            XmlNode appSettingsNode = GetSettingNode(xmlDoc, strParent);
            try
            {
			    // Begin TT#1668 - JSmith - Install Log
                if (fileName != null)
                {
                    msg = frame.GetText("ConfigAdded");
                    msg = msg.Replace("{0}", strKey);
                    msg = msg.Replace("{1}", fileName);
                    msg = msg.Replace("{2}", strValue);
                    frame.SetLogMessage(msg, eErrorType.message);
                }
				// End TT#1668

                if (KeyExists(xmlDoc, strParent, strLookupType, strKey))
                {
                    log.AddLogEntry("Key name: <" + strKey +
                              "> already exists in the configuration.", eErrorType.error);
                    return;
                }
                XmlNode newChild = null;
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        newChild = childNode.Clone();
                        break;
                    }
                }

                strValue = CheckIfEncrypted(strKey, strValue, fileName);

                // Begin TT#1088 - JSmith - Null reference error merging configuration files
                if (newChild == null)
                {
                    newElement = xmlDoc.CreateElement("add");
                    newElement.SetAttribute("key", strKey);
                    newElement.SetAttribute("value", strValue);
                    appSettingsNode.AppendChild(newElement);
                }
                else
                {
                    newChild.Attributes["key"].Value = strKey;
                    newChild.Attributes["value"].Value = strValue;
                    appSettingsNode.AppendChild(newChild);
                }
                //newChild.Attributes["key"].Value = strKey;
                //newChild.Attributes["value"].Value = strValue;
                //appSettingsNode.AppendChild(newChild);
                // End TT#1088
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Updates a key within the App.config
		// Begin TT#1668 - JSmith - Install Log
		//public void UpdateKey(XmlDocument xmlDoc, string strKey, string newValue)
        public void UpdateKey(string fileName, XmlDocument xmlDoc, string strParent, string strLookupType, string strKey, string newValue)
		// End TT#1668
        {
            // Begin TT#2178 - JSmith - Install received an error message
            try
            {
			    // Begin TT#1668 - JSmith - Install Log
                string msg;
				// End TT#1668
            // End TT#2178
                

                if (!KeyExists(xmlDoc, strParent, strLookupType, strKey))
                {
                    log.AddLogEntry("Key name: <" + strKey +
                          "> does not exist in the configuration. Update failed.", eErrorType.error);
                }
                XmlNode appSettingsNode = GetSettingNode(xmlDoc, strParent);
                if (strKey.ToLower() == "file")
                {
                    // Begin TT#1668 - JSmith - Install Log
                    if (fileName != null &&
                        appSettingsNode.Attributes["file"].Value != newValue)
                    {
                        msg = frame.GetText("ConfigChange");
                        msg = msg.Replace("{0}", strKey);
                        msg = msg.Replace("{1}", fileName);
                        msg = msg.Replace("{2}", appSettingsNode.Attributes["file"].Value);
                        msg = msg.Replace("{3}", newValue);
                        frame.SetLogMessage(msg, eErrorType.message);
                    }
                    // End TT#1668

                    appSettingsNode.Attributes["file"].Value = newValue;
                }
                else if (strLookupType == InstallerConstants.cLookupType_Parent)
                {
                    if (appSettingsNode.Attributes[GetAttributeFromKey(strKey)].Value != newValue)
                    {
                        msg = frame.GetText("ConfigChange");
                        msg = msg.Replace("{0}", strKey);
                        msg = msg.Replace("{1}", fileName);
                        msg = msg.Replace("{2}", appSettingsNode.Attributes[GetAttributeFromKey(strKey)].Value);
                        msg = msg.Replace("{3}", newValue);
                        frame.SetLogMessage(msg, eErrorType.message);
                    }

                    appSettingsNode.Attributes[GetAttributeFromKey(strKey)].Value = newValue;
                }
                else
                {
                    // Begin TT#2792 - JSmith - Installer Crash
                    //if (htEncryptedFields.ContainsKey(strKey))
                    //{
                    //    newValue = encryption.Encrypt(newValue);
                    //}
                    //if (htEncryptedFields.ContainsKey(strKey))
                    //{
                    //    int len = MaxEncryptedFieldSize + 1;
                    //    int count = 0;
                    //    string origValue = newValue;
                    //    bool stop = false;
                    //    while (!stop)
                    //    {
                    //        newValue = encryption.Encrypt(newValue);
                    //         len = newValue.Length;
                    //         if (len > MaxEncryptedFieldSize)
                    //         {
                    //             // wait on second and get new encryption object and try again.
                    //             System.Threading.Thread.Sleep(1000);
                    //             encryption = new MIDEncryption();
                    //             newValue = origValue;
                    //             ++count;
                    //             if (count > 3)
                    //             {
                    //                 stop = true;
                    //                 msg = frame.GetText("EncryptError");
                    //                 msg = msg.Replace("{0}", strKey);
                    //                 msg = msg.Replace("{1}", fileName);
                    //                 frame.SetLogMessage(msg, eErrorType.error);
                    //                 log.AddLogEntry(msg, eErrorType.error);
                    //                 frame.PopUpWarning(msg);
                    //             }
                    //         }
                    //         else
                    //         {
                    //             stop = true;
                    //         }
                    //    }
                    //    // End TT#2792 - JSmith - Installer Crash
                    //}

                    newValue = CheckIfEncrypted(strKey, newValue, fileName);

                    // Attempt to locate the requested setting.
                    foreach (XmlNode childNode in appSettingsNode)
                    {
                        if (childNode.GetType() != typeof(System.Xml.XmlComment))
                        {
                            if (strParent == InstallerConstants.cParent_AppSettings)
                            {
                                if (childNode.Attributes["key"].Value == strKey)
                                {
                                    // Begin TT#1668 - JSmith - Install Log
                                    if (fileName != null &&
                                        childNode.Attributes["value"].Value != newValue)
                                    {
                                        msg = frame.GetText("ConfigChange");
                                        msg = msg.Replace("{0}", strKey);
                                        msg = msg.Replace("{1}", fileName);
                                        msg = msg.Replace("{2}", childNode.Attributes["value"].Value);
                                        msg = msg.Replace("{3}", newValue);
                                        frame.SetLogMessage(msg, eErrorType.message);
                                    }
                                    // End TT#1668

                                    childNode.Attributes["value"].Value = newValue;
                                    break;
                                }
                            }
                            else
                            {
                                childNode.Attributes[strKey].Value = newValue;
                                break;
                            }
                        }
                    }
                }
            // Begin TT#2178 - JSmith - Install received an error message
            }
            catch (Exception err)
            {
                log.AddLogEntry("Update Key: " + strKey + " |Value: " + newValue + " |Error: " + err.Message, eErrorType.error);
                throw;
            }
            // End TT#2178
        }

        private string CheckIfEncrypted(string strKey, string strValue, string fileName)
        {
            string msg;
            if (htEncryptedFields.ContainsKey(strKey))
            {
                int len = MaxEncryptedFieldSize + 1;
                int count = 0;
                string origValue = strValue;
                bool stop = false;
                while (!stop)
                {
                    strValue = encryption.Encrypt(strValue);
                    len = strValue.Length;
                    if (len > MaxEncryptedFieldSize)
                    {
                        // wait on second and get new encryption object and try again.
                        System.Threading.Thread.Sleep(1000);
                        encryption = new MIDEncryption();
                        strValue = origValue;
                        ++count;
                        if (count > 3)
                        {
                            stop = true;
                            msg = frame.GetText("EncryptError");
                            msg = msg.Replace("{0}", strKey);
                            msg = msg.Replace("{1}", fileName);
                            frame.SetLogMessage(msg, eErrorType.error);
                            log.AddLogEntry(msg, eErrorType.error);
                            frame.PopUpWarning(msg);
                        }
                    }
                    else
                    {
                        stop = true;
                    }
                }
            }

            return strValue;
        }

        // Deletes a key from the App.config
        public void DeleteKey(XmlDocument xmlDoc, string strParent, string strLookupType, string strKey)
        {
            if (!KeyExists(xmlDoc, strParent, strLookupType, strKey))
            {
                return;
            }
            XmlNode appSettingsNode = GetSettingNode(xmlDoc, strParent);
            // Attempt to locate the requested setting.
            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.GetType() != typeof(System.Xml.XmlComment))
                {
                    if (childNode.Attributes["key"].Value == strKey)
                    {
                        appSettingsNode.RemoveChild(childNode);
                        break;
                    }
                }
            }
        }

        public bool KeyExists(XmlDocument xmlDoc, string strParent, string strLookupType, string strKey)
        {
            if (xmlDoc == null)
            {
                return false;
            }

            XmlNode appSettingsNode = GetSettingNode(xmlDoc, strParent);
            if (appSettingsNode != null)
            {
                if (strKey == "file")
                {
                    return AttributeExists(appSettingsNode, strKey);
                }
                else if (strLookupType == InstallerConstants.cLookupType_Parent)
                {
                    return AttributeExists(appSettingsNode, GetAttributeFromKey(strKey));
                }
                else
                {
                    // Attempt to locate the requested setting.
                    foreach (XmlNode childNode in appSettingsNode)
                    {
                        if (childNode.GetType() != typeof(System.Xml.XmlComment))
                        {
                            if (strParent == InstallerConstants.cParent_AppSettings)
                            {
                                if (childNode.Attributes["key"].Value == strKey)
                                    return true;
                            }
                            else
                            {
                                if (childNode.Attributes != null
                                    && childNode.Attributes[strKey] != null)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool AttributeExists(XmlNode xmlNode, string strKey)
        {
            if (xmlNode == null)
            {
                return false;
            }

            foreach (XmlNode childNode in xmlNode.Attributes)
            {
                if (childNode.GetType() != typeof(System.Xml.XmlComment))
                {
                    if (childNode.Name == strKey)
                        return true;
                }
            }
            return false;
        }

        private string GetAttributeFromKey(string strKey)
        {
            string[] values = strKey.Split(':');
            if (values.Length > 1)
            {
                return values[values.Length - 1].Trim();
            }
            else if (strKey.StartsWith("Log File"))
            {
                return InstallerConstants.cAttribute_Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetAttributeValue(XmlNode xmlNode, string strKey)
        {
            if (xmlNode == null)
            {
                return string.Empty;
            }

            foreach (XmlNode childNode in xmlNode.Attributes)
            {
                if (childNode.GetType() != typeof(System.Xml.XmlComment))
                {
                    if (childNode.Name == strKey)
                    {
                        return childNode.Value;
                    }
                }
            }
            return string.Empty;
        }

        // Updates a key within the App.config
        public void UpdateAttribute(XmlNode xmlNode, string strKey, string newValue)
        {
            if (!AttributeExists(xmlNode, strKey))
            {
                log.AddLogEntry("Attribute name: <" + strKey +
                      "> does not exist in the configuration. Update failed.", eErrorType.error);
            }
            foreach (XmlNode childNode in xmlNode.Attributes)
            {
                if (childNode.GetType() != typeof(System.Xml.XmlComment))
                {
                    if (childNode.Name == strKey)
                    {
                        childNode.Value = newValue;
                        break;
                    }
                }
            }
        }

        public string GetValue(XmlDocument xmlDoc, string strParent, string strLookupType, string strKey)
        {
            if (xmlDoc == null)
            {
                return string.Empty;
            }

            XmlNode appSettingsNode = GetSettingNode(xmlDoc, strParent);
            // Begin TT#74 MD - JSmith - One-button Upgrade
            if (strKey == "File")
            {
                return appSettingsNode.Attributes["file"].Value;
            }
            else if (strLookupType == InstallerConstants.cLookupType_Parent)
            {
                return appSettingsNode.Attributes[GetAttributeFromKey(strKey)].Value;
            }
            // End TT#74 MD
            // Attempt to locate the requested setting.
            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.GetType() != typeof(System.Xml.XmlComment))
                {
                    if (strParent == InstallerConstants.cParent_AppSettings)
                    {
                        if (childNode.Attributes["key"].Value == strKey)
                        {
                            return childNode.Attributes["value"].Value;
                        }
                    }
                    else
                    {
                        return childNode.Attributes[strKey].Value;
                    }
                }
            }
            return string.Empty;
        }

        private XmlNode GetSettingNode(XmlDocument xmlDoc, string strParent)
        {
            XmlNode appSettingsNode;
            if (strParent == InstallerConstants.cParent_AppSettings)
            {
                appSettingsNode = xmlDoc.SelectSingleNode(InstallerConstants.cParent_ConfigurationAppSettings);
                if (appSettingsNode == null)
                {
                    appSettingsNode = xmlDoc.SelectSingleNode(InstallerConstants.cParent_AppSettings);
                }
            }
            else
            {
                appSettingsNode = xmlDoc.SelectSingleNode(strParent);
            }
            return appSettingsNode;
        }

        private string GetDefault(string ID)
        {
            //variable for description
            string strDefault = "";

            //get the description datatable
            DataTable dtDesc = install_data.Tables[InstallerConstants.cTable_Description];

            //select the correct description
            DataRow[] rows = dtDesc.Select("id = '" + ID + "'");

            //set the description
            strDefault = rows[0].Field<string>(InstallerConstants.cSettingValue_DefaultValue);
            if (strDefault != null)
            {
                strDefault = strDefault.Trim();
            }

            //return the description
            return strDefault;
        }

        /// <summary>
        /// Returns the machine name where the session is located.
        /// </summary>

        private string GetMachineName()
        {
            try
            {
                // Get machine name
                string hostName = System.Net.Dns.GetHostName();
                string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                string fqdn = "";
                if (!hostName.Contains(domainName))
                {
                    if (domainName != null && domainName.Trim().Length > 0)
                    {
                        fqdn = hostName + "." + domainName;
                    }
                    else
                    {
                        fqdn = hostName;
                    }
                }
                else
                {
                    fqdn = hostName;
                }
                return fqdn;

                //return System.Net.Dns.GetHostName();
                //return Environment.MachineName + "." + Environment.GetEnvironmentVariable("USERDNSDOMAIN");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the machine name where the session is located.
        /// </summary>

        private string GetIPAddress()
        {
            try
            {
                // Get machine name
                string machineName = System.Net.Dns.GetHostName();
                // Then using host name, get the IP address list..
                System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(machineName);
                System.Net.IPAddress[] addr = ipEntry.AddressList;
                return addr[0].ToString();
            }
            catch
            {
                throw;
            }
        }

        //public static string Encrypt(string originalString)
        //{
        //    if (String.IsNullOrEmpty(originalString))
        //    {
        //        throw new ArgumentNullException
        //               ("The string which needs to be encrypted can not be null.");
        //    }
        //    byte[] bytes = ASCIIEncoding.ASCII.GetBytes(InstallerConstants.cEncryptKey);
        //    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //    MemoryStream memoryStream = new MemoryStream();
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream,
        //        cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
        //    StreamWriter writer = new StreamWriter(cryptoStream);
        //    writer.Write(originalString);
        //    writer.Flush();
        //    cryptoStream.FlushFinalBlock();
        //    writer.Flush();
        //    return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        //}

        ///// <summary>
        ///// Decrypt a crypted string.
        ///// </summary>
        ///// <param name="cryptedString">The crypted string.</param>
        ///// <returns>The decrypted string.</returns>
        ///// <exception cref="ArgumentNullException">This exception will be thrown 
        ///// when the crypted string is null or empty.</exception>
        //public static string Decrypt(string cryptedString)
        //{
        //    if (String.IsNullOrEmpty(cryptedString))
        //    {
        //        throw new ArgumentNullException
        //           ("The string which needs to be decrypted can not be null.");
        //    }
        //    byte[] bytes = ASCIIEncoding.ASCII.GetBytes(InstallerConstants.cEncryptKey);
        //    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //    MemoryStream memoryStream = new MemoryStream
        //            (Convert.FromBase64String(cryptedString));
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream,
        //        cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
        //    StreamReader reader = new StreamReader(cryptoStream);
        //    return reader.ReadToEnd();
        //}
    }
}
